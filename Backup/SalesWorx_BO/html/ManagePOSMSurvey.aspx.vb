Imports SalesWorx.BO.Common
Imports System.Data
Imports System.Web
Imports System.Resources
Imports System.Web.UI.WebControls
Imports log4net
Imports System.Text.RegularExpressions
Partial Public Class ManagePOSMSurvey
    Inherits System.Web.UI.Page

    Dim objPOSM As New POSM
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim Dt As New DataTable
    Private Const PageID As String = "P277"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        If Not Page.IsPostBack Then
            btnFilter.Attributes.Add("OnClick", "javascript:return checkform()")
            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            If Not HasPermission Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If
            LoadQuestionGroup()
            LoadResponseTypes()
            Resetfields()
        End If
    End Sub
    Sub LoadResponseTypes()
        Dim dtQ1 As New DataTable
        dtQ1 = objPOSM.GetQuestionCodes(Err_No, Err_Desc, "POSM_RESPONSE_TYPE")

        ddl_Response.DataSource = dtQ1
        ddl_Response.DataTextField = "Code_Description"
        ddl_Response.DataValueField = "Code_Value"
        ddl_Response.DataBind()
        ddl_Response.Items.Insert(0, New ListItem("-- Select a value --", "0"))

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
        ddl_QGroup.ClearSelection()
        pnl_details.Visible = False
        ddl_Question2.Visible = False
        lbl_Question2.Visible = False

        ddl_Question1.ClearSelection()
        ddl_Question2.ClearSelection()

        Me.btnSave.Text = "Save"

        'Me.btnAdd.Focus()
        ''Me.lblMessage.Text = ""

    End Sub

    'Protected Sub CheckRequiredFields()
    '    If Me.txtCurrencyCode.Text = "" Or Me.txtDescription.Text = "" Or Me.txtRate.Text = "" Then
    '        Me.lblinfo.Text = "Validation"
    '        Me.lblMessage.Text = "Currency code,description and conversion rate are required."
    '        Me.lblMessage.ForeColor = Drawing.Color.Red
    '        Me.MpInfoError.Show()
    '        Me.MPECurrency.Show()
    '        Exit Sub
    '    End If

    '    If IsAlpha(Me.txtCurrencyCode.Text) = False Then
    '        Me.lblinfo.Text = "Validation"
    '        Me.lblMessage.Text = "Currency code should be in characters."
    '        Me.lblMessage.ForeColor = Drawing.Color.Red
    '        Me.MpInfoError.Show()
    '        Me.MPECurrency.Show()
    '        Exit Sub
    '    End If


    '    If IsNumeric(Me.txtRate.Text) = False Then
    '        Me.lblinfo.Text = "Validation"
    '        Me.lblMessage.Text = "Conversion rate should be in number."
    '        Me.lblMessage.ForeColor = Drawing.Color.Red
    '        Me.MpInfoError.Show()
    '        Me.MPECurrency.Show()
    '        Exit Sub
    '    End If
    'End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Me.ddl_QGroup.SelectedItem.Value = "0" Then
            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Question Group is required."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Me.MPECurrency.Show()
            Exit Sub
        End If

        If Me.ddl_Question1.SelectedItem.Value = "0" Then
            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = lbl_Question1.Text & " is required."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Me.MPECurrency.Show()
            Exit Sub
        End If
        If Me.ddl_QGroup.SelectedItem.Value = "CP" Then
            If Me.ddl_Question2.SelectedItem.Value = "0" Then
                Me.lblinfo.Text = "Validation"
                Me.lblMessage.Text = lbl_Question2.Text & " is required."
                Me.lblMessage.ForeColor = Drawing.Color.Red
                Me.MpInfoError.Show()
                Me.MPECurrency.Show()
                Exit Sub
            End If
        End If
        Dim success As Boolean = False
        Try
            Dim msg As String = ""
            success = objPOSM.ManagePOSMSurvey(Err_No, Err_Desc, 1, ddl_QGroup.SelectedItem.Value, ddl_Question1.SelectedItem.Value, ddl_Question2.SelectedItem.Value, CType(Session("User_Access"), UserAccess).UserID.ToString(), ddl_Response.SelectedItem.Value, 0, msg)
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
                Dt = objPOSM.GetSurvey(Err_No, Err_Desc, ddl_QGroup.SelectedItem.Value)
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
        HColCount.Value = Dt.Columns.Count

        gvPOSM.DataSource = dv
        gvPOSM.DataBind()
        For Each dr As GridViewRow In gvPOSM.Rows
            dr.Cells(Dt.Columns.Count).Visible = False
        Next
        gvPOSM.HeaderRow.Cells(Dt.Columns.Count).Visible = False
          
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

        If Me.ddl_Question1.SelectedItem.Value = "0" Then
            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = lbl_Question1.Text & " is required."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Me.MPECurrency.Show()
            Exit Sub
        End If
        If Me.ddl_QGroup.SelectedItem.Value = "CP" Then
            If Me.ddl_Question2.SelectedItem.Value = "0" Then
                Me.lblinfo.Text = "Validation"
                Me.lblMessage.Text = lbl_Question2.Text & " is required."
                Me.lblMessage.ForeColor = Drawing.Color.Red
                Me.MpInfoError.Show()
                Me.MPECurrency.Show()
                Exit Sub
            End If
        End If

        Dim success As Boolean = False
        Try

              Dim msg As String = ""
            success = objPOSM.ManagePOSMSurvey(Err_No, Err_Desc, 2, ddl_QGroup.SelectedItem.Value, ddl_Question1.SelectedItem.Value, ddl_Question2.SelectedItem.Value, CType(Session("User_Access"), UserAccess).UserID.ToString(), ddl_Response.SelectedItem.Value, HidVal.Value, msg)

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
                Dt = objPOSM.GetSurvey(Err_No, Err_Desc, ddl_QGroup.SelectedItem.Value)
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
            Dim sel As String = "0"
            Dim dr As GridViewRow
            Dim Success As Boolean = False
            For Each dr In gvPOSM.Rows
                Dim RowCheckBox As System.Web.UI.WebControls.CheckBox = dr.FindControl("chkDelete")

                If RowCheckBox.Checked = True Then
                    HidVal.Value = dr.Cells(HColCount.Value).Text
                    Dim msg As String
                    Dim ds As DataTable
                    ds = objPOSM.GetSurveyQuestionDetails(Err_No, Err_Desc, HidVal.Value)
                    If ds.Rows.Count > 0 Then
                        sel = ds.Rows(0)("Question_Group_1").ToString
                    End If

                    Success = objPOSM.ManagePOSMSurvey(Err_No, Err_Desc, 3, "", "", "", CType(Session("User_Access"), UserAccess).UserID.ToString(), "", HidVal.Value, msg)

                End If
            Next
            If (Success = True) Then
                lblMessage.Text = "Deleted successfully."
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
                If Not ddQGroup.Items.FindByValue(sel) Is Nothing Then
                    ddQGroup.ClearSelection()
                    ddQGroup.Items.FindByValue(sel).Selected = True
                End If
                Dt = objPOSM.GetSurvey(Err_No, Err_Desc, ddQGroup.SelectedItem.Value)
                LoadGridData()
                Resetfields()
            Else
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & Err_Desc & "&next=ManagePOSMSurvey.aspx&Title=Message", False)
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
            ViewState("SortField") = ""
            Dt = objPOSM.GetSurvey(Err_No, Err_Desc, ddQGroup.SelectedItem.Value)
            LoadGridData()
            ClassUpdatePnl.Update()
            success = True
            If success = False Then
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & Err_Desc & "&next=ManagePOSMSurvey.aspx&Title=Message", False)
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
        Dim sel As String = "0"
        Try
            Dim msg As String = ""
            Dim ds As DataTable
            ds = objPOSM.GetSurveyQuestionDetails(Err_No, Err_Desc, row.Cells(HColCount.Value).Text)
            If ds.Rows.Count > 0 Then
                sel = ds.Rows(0)("Question_Group_1").ToString
            End If

            success = objPOSM.ManagePOSMSurvey(Err_No, Err_Desc, 3, "", "", "", CType(Session("User_Access"), UserAccess).UserID.ToString(), "", row.Cells(HColCount.Value).Text, msg)
            If success = True Then
               

                lblMessage.Text = "Successfully deleted."
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
                If Not ddQGroup.Items.FindByValue(sel) Is Nothing Then
                    ddQGroup.ClearSelection()
                    ddQGroup.Items.FindByValue(sel).Selected = True
                End If
                Dt = objPOSM.GetSurvey(Err_No, Err_Desc, ddQGroup.SelectedItem.Value)
                LoadGridData()
                Resetfields()
            Else
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & Err_Desc & "&next=ManagePOSMSurvey.aspx&Title=Message", False)
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
        ddl_QGroup.ClearSelection()
        pnl_details.Visible = False
        Resetfields()
        ClassUpdatePnl.Update()
        Me.MPECurrency.Show()
    End Sub


    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try

            btnUpdate.Visible = True
            btnSave.Visible = False
            Resetfields()
            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
            Dim Qid As String = row.Cells(HColCount.Value).Text
            HidVal.Value = Qid
            Dim dt As New DataTable
            dt = objPOSM.GetSurveyQuestionDetails(Err_No, Err_Desc, HidVal.Value)
            If dt.Rows.Count > 0 Then
                If Not ddl_QGroup.Items.FindByValue(dt.Rows(0)("Question_Group_1").ToString) Is Nothing Then
                    ddl_QGroup.ClearSelection()
                    ddl_QGroup.Items.FindByValue(dt.Rows(0)("Question_Group_1").ToString).Selected = True
                End If
                pnl_details.Visible = True
                loadddlQuestion()

                If Not ddl_Question1.Items.FindByValue(dt.Rows(0)("Question_1").ToString) Is Nothing Then
                    ddl_Question1.ClearSelection()
                    ddl_Question1.Items.FindByValue(dt.Rows(0)("Question_1").ToString).Selected = True
                End If
                If Not ddl_Question2.Items.FindByValue(dt.Rows(0)("Question_2").ToString) Is Nothing Then
                    ddl_Question2.ClearSelection()
                    ddl_Question2.Items.FindByValue(dt.Rows(0)("Question_2").ToString).Selected = True
                End If
                If Not ddl_Response.Items.FindByValue(dt.Rows(0)("Response_Type").ToString) Is Nothing Then
                    ddl_Response.ClearSelection()
                    ddl_Response.Items.FindByValue(dt.Rows(0)("Response_Type").ToString).Selected = True
                End If
            End If
            MPECurrency.Show()
        Catch ex As Exception
            Err_No = "74066"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & Err_Desc & "&next=ManagePOSMSurvey.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub

    Private Sub gvCurrency_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPOSM.PageIndexChanging
        gvPOSM.PageIndex = e.NewPageIndex
        Dt = objPOSM.GetSurvey(Err_No, Err_Desc, ddQGroup.SelectedItem.Value)
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
        Dt = objPOSM.GetSurvey(Err_No, Err_Desc, ddQGroup.SelectedItem.Value)
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

    Sub loadddlQuestion()
        If ddl_QGroup.SelectedItem.Value = "CP" Then
            Dim dtQ1 As New DataTable
            dtQ1 = objPOSM.GetQuestionCodes(Err_No, Err_Desc, "POSM_QUES_CP_PROD")
            lbl_Question1.Text = "Products"
            ddl_Question1.DataSource = dtQ1
            ddl_Question1.DataTextField = "Code_Description"
            ddl_Question1.DataValueField = "Code_Value"
            ddl_Question1.DataBind()
            ddl_Question1.Items.Insert(0, New ListItem("-- Select a value --", "0"))

            Dim dtQ2 As New DataTable
            dtQ2 = objPOSM.GetQuestionCodes(Err_No, Err_Desc, "POSM_QUES_CP_MEASURE")
            lbl_Question2.Text = "Measure"
            lbl_Question2.Visible = True

            ddl_Question2.DataSource = dtQ2
            ddl_Question2.DataTextField = "Code_Description"
            ddl_Question2.DataValueField = "Code_Value"
            ddl_Question2.DataBind()
            ddl_Question2.Visible = True
            If Not ddl_Response.Items.FindByValue(1) Is Nothing Then
                ddl_Response.ClearSelection()
                ddl_Response.Items.FindByValue(1).Selected = True
            End If
            ddl_Response.Enabled = False
        ElseIf ddl_QGroup.SelectedItem.Value = "SGA" Then

            Dim dtQ1 As New DataTable
            dtQ1 = objPOSM.GetQuestionCodes(Err_No, Err_Desc, "POSM_QUES_SGA_ASSET")
            lbl_Question1.Text = "Assets"
            ddl_Question1.DataSource = dtQ1
            ddl_Question1.DataTextField = "Code_Description"
            ddl_Question1.DataValueField = "Code_Value"
            ddl_Question1.DataBind()
            ddl_Question1.Items.Insert(0, New ListItem("-- Select a value --", "0"))
            lbl_Question2.Visible = False
            ddl_Question2.Visible = False
            If Not ddl_Response.Items.FindByValue(1) Is Nothing Then
                ddl_Response.ClearSelection()
                ddl_Response.Items.FindByValue(1).Selected = True
            End If
            ddl_Response.Enabled = False

        ElseIf ddl_QGroup.SelectedItem.Value = "GNRL" Then
            Dim dtQ1 As New DataTable
            dtQ1 = objPOSM.GetQuestionCodes(Err_No, Err_Desc, "POSM_QUES_GNRL_ITEM")
            lbl_Question1.Text = "Items"
            ddl_Question1.DataSource = dtQ1
            ddl_Question1.DataTextField = "Code_Description"
            ddl_Question1.DataValueField = "Code_Value"
            ddl_Question1.DataBind()
            ddl_Question1.Items.Insert(0, New ListItem("-- Select a value --", "0"))
            lbl_Question2.Visible = False
            ddl_Question2.Visible = False
            ddl_Response.Enabled = True
        ElseIf ddl_QGroup.SelectedItem.Value = "POSM" Then
            Dim dtQ1 As New DataTable
            dtQ1 = objPOSM.GetQuestionCodes(Err_No, Err_Desc, "POSM_QUES_POSM_ITEM")
            lbl_Question1.Text = "Items"
            ddl_Question1.DataSource = dtQ1
            ddl_Question1.DataTextField = "Code_Description"
            ddl_Question1.DataValueField = "Code_Value"
            ddl_Question1.DataBind()
            ddl_Question1.Items.Insert(0, New ListItem("-- Select a value --", "0"))
            lbl_Question2.Visible = False
            ddl_Question2.Visible = False
            If Not ddl_Response.Items.FindByValue(1) Is Nothing Then
                ddl_Response.ClearSelection()
                ddl_Response.Items.FindByValue(1).Selected = True
            End If
            ddl_Response.Enabled = False
        End If

        ddl_Question2.Items.Insert(0, New ListItem("-- Select a value --", "0"))

    End Sub
    Private Sub ddl_QGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_QGroup.SelectedIndexChanged
        pnl_details.Visible = True

        loadddlQuestion()
        MPECurrency.Show()
    End Sub

   
End Class