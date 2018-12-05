Imports SalesWorx.BO.Common
Imports System.Data
Imports System.Web
Imports System.Resources
Imports System.Web.UI.WebControls
Imports log4net
Imports System.Text.RegularExpressions
Partial Public Class AdminPOS_MasterSetUp
    Inherits System.Web.UI.Page
    Dim objPOSM As New POSM
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim Dt As New DataTable
    Private Const PageID As String = "P276"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        If Not Page.IsPostBack Then
            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            If Not HasPermission Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If
            LoadQuestionGroup()
            loadFilterQuestion()
            Dt = objPOSM.GetQuestions(Err_No, Err_Desc, "0", "0", "")
            LoadGridData()
            Resetfields()
        End If
    End Sub
    Sub LoadQuestionGroup()
        Dim ObjCommon As New SalesWorx.BO.Common.POSM
        Dim dt As New DataTable

        dt = ObjCommon.GetQuestionGroups(Err_No, Err_Desc)

        ddl_QGroup.DataSource = dt
        ddl_QGroup.DataTextField = "code_description"
        ddl_QGroup.DataValueField = "code_Value"
        ddl_QGroup.DataBind()
        ddl_QGroup.Items.Insert(0, New ListItem("-- Select a value --", "0"))

        ddQGroup.DataSource = dt
        ddQGroup.DataTextField = "code_description"
        ddQGroup.DataValueField = "code_Value"
        ddQGroup.DataBind()
        ddQGroup.Items.Insert(0, New ListItem("-- Select a value --", "0"))

    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Try
            Me.MPECurrency.Hide()
            Resetfields()
        Catch

        End Try
    End Sub

    Public Sub Resetfields()
        Me.txtCode.Text = ""
        Me.txtDescription.Text = ""
        ddl_QGroup.ClearSelection()
        ddl_Question.ClearSelection()
        Me.btnSave.Text = "Save"

        'Me.btnAdd.Focus()
        ''Me.lblMessage.Text = ""

    End Sub

    
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Me.ddl_QGroup.SelectedItem.Value = "0" Then
            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Question Group is required."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Me.MPECurrency.Show()
            Exit Sub
        End If

        If Me.ddl_Question.SelectedItem.Value = "0" Then
            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Question is required."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Me.MPECurrency.Show()
            Exit Sub
        End If

        If Me.txtCode.Text = "" Or Me.txtDescription.Text = "" Then
            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Code and description are required."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Me.MPECurrency.Show()
            Exit Sub
        End If

        Dim success As Boolean = False
        Try
            Dim msg As String = ""
            success = objPOSM.ManagePOSMAppcodes(Err_No, Err_Desc, 1, ddl_Question.SelectedItem.Value, txtCode.Text, txtDescription.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), msg)
            If success = True Then
                Me.lblMessage.Text = "Successfully saved."
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
            Else
                log.Error(msg)
                Me.lblMessage.Text = msg
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
                Me.MPECurrency.Show()
                btnClose.Focus()
            End If
            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "POSMAPPCODE", ddl_Question.SelectedItem.Value, "Code: " & Me.txtCode.Text & "/ Desc :  " & Me.txtDescription.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                Dt = objPOSM.GetQuestions(Err_No, Err_Desc, ddQGroup.SelectedItem.Value, ddQuestion.SelectedItem.Value, txtFilterVal.Text)
                LoadGridData()
                Resetfields()
                Me.MPECurrency.Hide()

            End If

        Catch ex As Exception
            Err_No = "74061"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Private Sub LoadGridData()
        Me.gvPOSM.DataSource = Dt
        Me.gvPOSM.DataBind()

        Dim dv As New DataView(Dt)
        If ViewState("SortField") <> "" Then
            dv.Sort = (ViewState("SortField") & " ") + SortDirection
        End If
        gvPOSM.DataSource = dv
        gvPOSM.DataBind()
      
    End Sub

    Public Function IsNumeric(ByVal inputString As String) As Boolean
        Dim _isNumber As System.Text.RegularExpressions.Regex = New  _
Regex("(^[-+]?\d+(,?\d*)*\.?\d*([Ee][-+]\d*)?$)|(^[-+]?\d?(,?\d*)*\.\d+([Ee][-+]\d*)?$)")
        Return _isNumber.Match(inputString).Success
    End Function

    Public Function IsAlpha(ByVal strToCheck As String) As Boolean
        Dim objAlphaPattern As Regex = New Regex("[^a-zA-Z]")
        Return Not objAlphaPattern.IsMatch(strToCheck)
    End Function


    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If Me.ddl_QGroup.SelectedItem.Value = "0" Then
            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Question Group is required."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Me.MPECurrency.Show()
            Exit Sub
        End If

        If Me.ddl_Question.SelectedItem.Value = "0" Then
            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Question is required."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Me.MPECurrency.Show()
            Exit Sub
        End If
        If Me.txtCode.Text = "" Or Me.txtDescription.Text = "" Then
            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Code and description rate are required."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Me.MPECurrency.Show()
            Exit Sub
        End If


        Dim success As Boolean = False
        Try

            Dim msg As String = ""
            success = objPOSM.ManagePOSMAppcodes(Err_No, Err_Desc, 2, ddl_Question.SelectedItem.Value, txtCode.Text, txtDescription.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), msg)
            If success = True Then
                Me.lblMessage.Text = "Successfully saved."
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
            Else
                log.Error(msg)
                Me.lblMessage.Text = msg
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
                Me.MPECurrency.Show()
                btnClose.Focus()
            End If

            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "U", "MAIN MENU", "POSMAPPCODE", ddl_Question.SelectedItem.Value, "Code: " & Me.txtCode.Text & "/ Desc :  " & Me.txtDescription.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                Dt = objPOSM.GetQuestions(Err_No, Err_Desc, ddQGroup.SelectedItem.Value, ddQuestion.SelectedItem.Value, txtFilterVal.Text)
                LoadGridData()
                Resetfields()
                Me.MPECurrency.Hide()
            End If

        Catch ex As Exception
            Err_No = "74062"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Protected Sub btnDeleteAll_Click()
        Try
            Dim dr As GridViewRow
            Dim Success As Boolean = False
            For Each dr In gvPOSM.Rows
                Dim RowCheckBox As System.Web.UI.WebControls.CheckBox = dr.FindControl("chkDelete")

                If RowCheckBox.Checked = True Then
                    Dim Lbl As System.Web.UI.WebControls.Label = dr.FindControl("lbl_Code")
                    Dim lbl_CodeType As System.Web.UI.WebControls.Label = dr.FindControl("lbl_CodeType")
                    HidVal.Value = Lbl.Text
                    Success = objPOSM.ManagePOSMAppcodes(Err_No, Err_Desc, 3, lbl_CodeType.Text, HidVal.Value, "", CType(Session("User_Access"), UserAccess).UserID.ToString(), "")
                    If Success = True Then
                        objLogin.SaveUserLog(Err_No, Err_Desc, "D", "MAIN MENU", "POSMAPPCODE", lbl_CodeType.Text, "Code: " & dr.Cells(1).Text & "/ Desc :  " & dr.Cells(2).Text & "/ Rate:  " & dr.Cells(3).Text & "/ Decimal : " & dr.Cells(4).Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                        Success = True
                    End If
                End If
            Next
            If (Success = True) Then
                lblMessage.Text = "Deleted successfully."
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
                Dt = objPOSM.GetQuestions(Err_No, Err_Desc, ddQGroup.SelectedItem.Value, ddQuestion.SelectedItem.Value, txtFilterVal.Text)
                LoadGridData()
                Resetfields()
            Else
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & Err_Desc & "&next=AdminPOS_MasterSetup.aspx&Title=Message", False)
                Exit Try
            End If

            'ClassUpdatePnl.Update()
        Catch ex As Exception
            Err_No = "74063"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Dim success As Boolean = False
        Try
            Dt = objPOSM.GetQuestions(Err_No, Err_Desc, ddQGroup.SelectedItem.Value, ddQuestion.SelectedItem.Value, txtFilterVal.Text)
            LoadGridData()
            ClassUpdatePnl.Update()
            success = True
            If success = False Then
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & Err_Desc & "&next=AdminPOS_MasterSetUp.aspx&Title=Message", False)
                Exit Try
            End If
        Catch ex As Exception
            Err_No = "74064"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btndelete As ImageButton = TryCast(sender, ImageButton)
        Dim row As GridViewRow = DirectCast(btndelete.NamingContainer, GridViewRow)

        Dim success As Boolean = False
        Dim Lbl As System.Web.UI.WebControls.Label = row.FindControl("lbl_Code")
        Dim lbl_CodeType As System.Web.UI.WebControls.Label = row.FindControl("lbl_CodeType")
        HidVal.Value = Lbl.Text
       
        Try

            success = objPOSM.ManagePOSMAppcodes(Err_No, Err_Desc, 3, lbl_CodeType.Text, HidVal.Value, "", CType(Session("User_Access"), UserAccess).UserID.ToString(), "")


            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "D", "MAIN MENU", "POSMAPPCODE", lbl_CodeType.Text, "Code: " & row.Cells(1).Text & "/ Desc :  " & row.Cells(2).Text & "/ Rate:  " & row.Cells(3).Text & "/ Decimal : " & row.Cells(4).Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                lblMessage.Text = "Successfully deleted."
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
                Dt = objPOSM.GetQuestions(Err_No, Err_Desc, ddQGroup.SelectedItem.Value, ddQuestion.SelectedItem.Value, txtFilterVal.Text)
                LoadGridData()
                Resetfields()
            Else
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & Err_Desc & "&next=AdminPOS_MasterSetUp.aspx&Title=Message", False)
                Exit Try
            End If

        Catch ex As Exception
            Err_No = "74065"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub


    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        btnUpdate.Visible = False
        btnSave.Visible = True
        txtCode.Enabled = True
        ddl_QGroup.Enabled = True
        ddl_Question.Enabled = True
        Resetfields()
        ClassUpdatePnl.Update()
        Me.MPECurrency.Show()
    End Sub


    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try

            btnUpdate.Visible = True
            btnSave.Visible = False
            Resetfields()
            ddl_QGroup.Enabled = False
            ddl_Question.Enabled = False
            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
            Dim Lbl As System.Web.UI.WebControls.Label = row.FindControl("lbl_Code")
            Dim lbl_CodeType As System.Web.UI.WebControls.Label = row.FindControl("lbl_CodeType")
            Dim lbl_GroupCode As System.Web.UI.WebControls.Label = row.FindControl("lbl_GroupCode")

            If Not ddl_QGroup.Items.FindByValue(lbl_GroupCode.Text) Is Nothing Then
                ddl_QGroup.ClearSelection()
                ddl_QGroup.Items.FindByValue(lbl_GroupCode.Text).Selected = True
            End If
            loadddlQuestion()
            If Not ddl_Question.Items.FindByValue(lbl_CodeType.Text) Is Nothing Then
                ddl_Question.ClearSelection()
                ddl_Question.Items.FindByValue(lbl_CodeType.Text).Selected = True
            End If
            txtCode.Text = Lbl.Text
            txtDescription.Text = Trim(row.Cells(3).Text)
            Me.txtCode.Enabled = False
            ' ClassUpdatePnl.Update()
            MPECurrency.Show()
        Catch ex As Exception
            Err_No = "74066"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & Err_Desc & "&next=AdminPOS_MasterSetUp.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub

    Private Sub gvCurrency_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPOSM.PageIndexChanging
        gvPOSM.PageIndex = e.NewPageIndex
        Dt = objPOSM.GetQuestions(Err_No, Err_Desc, ddQGroup.SelectedItem.Value, ddQuestion.SelectedItem.Value, txtFilterVal.Text)
        LoadGridData()

    End Sub

    Private Sub gvCurrency_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPOSM.RowDataBound
        'If e.Row.RowType.Equals(DataControlRowType.Pager) Then
        '    Dim pTableRow As TableRow = _
        '             CType(e.Row.Cells(0).Controls(0).Controls(0), TableRow)
        '    For Each cell As TableCell In pTableRow.Cells
        '        For Each control As Control In cell.Controls
        '            If TypeOf control Is LinkButton Then
        '                Dim lb As LinkButton = CType(control, LinkButton)
        '                lb.Attributes.Add("onclick", "ScrollToTop();")
        '            End If
        '        Next
        '    Next
        'End If
    End Sub
    Private Sub gvCurrency_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvPOSM.Sorting
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        Dt = objPOSM.GetQuestions(Err_No, Err_Desc, ddQGroup.SelectedItem.Value, ddQuestion.SelectedItem.Value, txtFilterVal.Text)
        LoadGridData()
    End Sub
    Private Property SortDirection() As String
        Get
            If ViewState("SortDirection") Is Nothing Then
                ViewState("SortDirection") = "ASC"
            End If
            Return ViewState("SortDirection").ToString()
        End Get
        Set(ByVal value As String)
            Dim s As String = SortDirection

            If value = "flip" Then
                s = If(s = "ASC", "DESC", "ASC")
            Else
                s = value
            End If

            ViewState("SortDirection") = s
        End Set
    End Property

    Private Sub ddQGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddQGroup.SelectedIndexChanged
        loadFilterQuestion()
    End Sub
    Sub loadFilterQuestion()
        Dim dt As New DataTable
        dt.Columns.Add("Question")
        dt.Columns.Add("Code")
        If ddQGroup.SelectedItem.Value = "CP" Then
            Dim dr As DataRow
            dr = dt.NewRow
            dr(0) = "Product"
            dr(1) = "POSM_QUES_CP_PROD"
            dt.Rows.Add(dr)

            Dim dr1 As DataRow
            dr1 = dt.NewRow
            dr1(0) = "Measure"
            dr1(1) = "POSM_QUES_CP_MEASURE"
            dt.Rows.Add(dr1)

        ElseIf ddQGroup.SelectedItem.Value = "SGA" Then
            Dim dr As DataRow
            dr = dt.NewRow
            dr(0) = "Asset"
            dr(1) = "POSM_QUES_SGA_ASSET"
            dt.Rows.Add(dr)
        ElseIf ddQGroup.SelectedItem.Value = "GNRL" Then
            Dim dr As DataRow
            dr = dt.NewRow
            dr(0) = "Item"
            dr(1) = "POSM_QUES_GNRL_ITEM"
            dt.Rows.Add(dr)
        ElseIf ddQGroup.SelectedItem.Value = "POSM" Then
            Dim dr As DataRow
            dr = dt.NewRow
            dr(0) = "Item"
            dr(1) = "POSM_QUES_POSM_ITEM"
            dt.Rows.Add(dr)
        End If

        ddQuestion.DataSource = dt
        ddQuestion.DataTextField = "Question"
        ddQuestion.DataValueField = "Code"
        ddQuestion.DataBind()
        ddQuestion.Items.Insert(0, New ListItem("-- Select a value --", "0"))
    End Sub
    Sub loadddlQuestion()
        Dim dt As New DataTable
        dt.Columns.Add("Question")
        dt.Columns.Add("Code")
        If ddl_QGroup.SelectedItem.Value = "CP" Then
            Dim dr As DataRow
            dr = dt.NewRow
            dr(0) = "Product"
            dr(1) = "POSM_QUES_CP_PROD"
            dt.Rows.Add(dr)

            Dim dr1 As DataRow
            dr1 = dt.NewRow
            dr1(0) = "Measure"
            dr1(1) = "POSM_QUES_CP_MEASURE"
            dt.Rows.Add(dr1)

        ElseIf ddl_QGroup.SelectedItem.Value = "SGA" Then
            Dim dr As DataRow
            dr = dt.NewRow
            dr(0) = "Asset"
            dr(1) = "POSM_QUES_SGA_ASSET"
            dt.Rows.Add(dr)
        ElseIf ddl_QGroup.SelectedItem.Value = "GNRL" Then
            Dim dr As DataRow
            dr = dt.NewRow
            dr(0) = "Item"
            dr(1) = "POSM_QUES_GNRL_ITEM"
            dt.Rows.Add(dr)
        ElseIf ddl_QGroup.SelectedItem.Value = "POSM" Then
            Dim dr As DataRow
            dr = dt.NewRow
            dr(0) = "Item"
            dr(1) = "POSM_QUES_POSM_ITEM"
            dt.Rows.Add(dr)
        End If

        ddl_Question.DataSource = dt
        ddl_Question.DataTextField = "Question"
        ddl_Question.DataValueField = "Code"
        ddl_Question.DataBind()
        ddl_Question.Items.Insert(0, New ListItem("-- Select a value --", "0"))
    End Sub
    Private Sub ddl_QGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_QGroup.SelectedIndexChanged
        loadddlQuestion()
        MPECurrency.Show()
    End Sub

    Protected Sub Btn_reset_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Btn_reset.Click
        ddQGroup.ClearSelection()
        ddQuestion.ClearSelection()
        txtFilterVal.Text = ""
        Dt = objPOSM.GetQuestions(Err_No, Err_Desc, ddQGroup.SelectedItem.Value, ddQuestion.SelectedItem.Value, txtFilterVal.Text)
        LoadGridData()
        ClassUpdatePnl.Update()
    End Sub
End Class