Imports SalesWorx.BO.Common
Imports System.Data
Imports System.Web
Imports System.Resources
Imports System.Web.UI.WebControls
Imports log4net
Imports System.IO
Imports System.Data.SqlClient
Imports Telerik.Web.UI

Partial Public Class AdminSurvey
    Inherits System.Web.UI.Page
    Private objSurvey As New Survey
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim Dt As New DataTable
    Dim dtSalesRepSelected As DataTable
    Private Const PageID As String = "P12"
    'Private _objUserAccess As UserAccess
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objLogin As New SalesWorx.BO.Common.Login

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
            Dim cls As New Purpose
            ddlTypeCode.DataSource = cls.BindSurveyTypeCode
            ddlTypeCode.DataTextField = "Key"
            ddlTypeCode.DataValueField = "Value"
            ddlTypeCode.DataBind()
            LoadCombo()

            '_objUserAccess = New UserAccess
            '_objUserAccess.AssignedSalesReps = New ArrayList
            chkSalRep.DataSource = objSurvey.LoadSalesRep(Err_No, Err_Desc)
            chkSalRep.DataTextField = "SalesRep_Name"
            chkSalRep.DataValueField = "SalesRep_ID"
            chkSalRep.DataBind()

            dtSalesRepSelected = New DataTable()
            SetSalRepTable()
            ResetFields()

            Dt = objSurvey.LoadSurveyData(Err_No, Err_Desc)
            BindSurveyData()

        Else

            MPEDetails.VisibleOnPageLoad = False
            dtSalesRepSelected = TryCast(ViewState("dtSR"), DataTable)


        End If
        ViewState("dtSR") = dtSalesRepSelected
    End Sub
    Private Sub BindSurveyData()
        Me.gvSurvey.DataSource = Dt
        Me.gvSurvey.DataBind()

        Dim dv As New DataView(Dt)
        If ViewState("SortField") <> "" Then
            dv.Sort = (ViewState("SortField") & " ") + SortDirection
        End If
        gvSurvey.DataSource = dv
        gvSurvey.DataBind()
        'Session.Remove("CurrencyCode")
        'Session("CurrencyCode") = Dt
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
    Private Sub SetSalRepTable()

        Dim col As DataColumn
        col = New DataColumn()
        col.ColumnName = "Survey_ID"
        col.DataType = System.Type.GetType("System.Int64")
        col.ReadOnly = False
        col.Unique = False
        dtSalesRepSelected.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "SalesRep_ID"
        col.DataType = System.Type.GetType("System.Int32")
        col.ReadOnly = False
        col.Unique = False
        dtSalesRepSelected.Columns.Add(col)


    End Sub

    Private Sub LoadCombo()
        '' ''ddSurvey.DataSource = objSurvey.LoadSurveys(Err_No, Err_Desc)
        '' ''ddSurvey.DataTextField = "Survey_title"
        '' ''ddSurvey.DataValueField = "Survey_Id"
        '' ''ddSurvey.DataBind()
    End Sub

    Private Sub ResetFields()
        ddlTypeCode.Items.Clear()
        Dim cls As New Purpose
        ddlTypeCode.DataSource = cls.BindSurveyTypeCode
        ddlTypeCode.DataTextField = "Key"
        ddlTypeCode.DataValueField = "Value"
        ddlTypeCode.DataBind()
        Me.txtExpDate.SelectedDate = Now.Date.AddDays(1)
        Me.txtStartDate.SelectedDate = Now.Date.AddDays(1)
        Me.txtTitle.Text = ""
        Me.ddlTypeCode.SelectedIndex = 1
        '' ''Me.ddSurvey.SelectedIndex = 0
        '' ''Me.ddSurvey.Visible = True
        '' ''Me.lblSurvey.Visible = True
        '' ''Me.lblTitle.Visible = False
        Me.txtTitle.Visible = True
        Me.txtTitle.Enabled = True
        '' ''  Me.btnAdd.Visible = True
        '' ''Me.btnModify.Visible = True
        '' ''Me.btnDelete.Visible = True
        '' ''Me.btnCancel.Visible = False
        Me.btnSave.Visible = False
        Me.btnSave.Text = "Save"
        '' ''Me.btnModify.Enabled = False
        '' ''Me.btnDelete.Enabled = False
        Me.chkSalRep.SelectedValue = Nothing
        Me.chkSalRep.ClearChecked()
        lblPop.Text = ""
        If dtSalesRepSelected.Rows.Count > 0 Then
            dtSalesRepSelected.Rows.Clear()
        End If

        txtTitle.Enabled = True
        ddlTypeCode.Enabled = True
        txtStartDate.Enabled = True
        txtExpDate.Enabled = True
        chkSalRep.Enabled = True
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
        btnUpdate.Visible = False
        btnSave.Visible = True
        ResetFields()
        ClassUpdatePnl.Update()
        Me.lblPop.Text = ""
        Me.MPEDetails.VisibleOnPageLoad = True
        ResetFields()
        ToggleEnabled(True)
        '' ''Me.ddSurvey.Visible = False
        '' ''Me.lblSurvey.Visible = False
        '' ''Me.lblTitle.Visible = True
        Me.txtTitle.Visible = True
        Me.txtTitle.Enabled = True
        '' ''  Me.btnAdd.Visible = False
        '' ''Me.btnModify.Visible = False
        '' ''Me.btnDelete.Visible = False
        '' ''  Me.btnCancel.Visible = True
        Me.btnSave.Visible = True
        Me.btnSave.Text = "Save"
        Me.txtStartDate.SelectedDate = Now.Date.AddDays(1).ToString("MM/dd/yyyy")
        Me.txtExpDate.SelectedDate = Now.Date.AddMonths(1).ToString("MM/dd/yyyy")
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        ResetFields()
    End Sub


    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click

        If Me.txtTitle.Text = "" Or Me.txtStartDate.SelectedDate Is Nothing Or Me.txtExpDate.SelectedDate Is Nothing Then
            Me.MPEDetails.VisibleOnPageLoad = True
            '' MessageBoxValidation("Title,start and expiry date required.", "Validation")
            lblPop.Text = "Title,start and expiry date required."
            Exit Sub
        End If

        If DateTime.Parse(Me.txtStartDate.SelectedDate) <= Now.Date And Me.btnSave.Text = "Save" Then
            Me.MPEDetails.VisibleOnPageLoad = True
            ''''  MessageBoxValidation("Start date should be greater than today's date.", "Validation")
            lblPop.Text = "Start date should be greater than today's date."
            Exit Sub
        End If


        If DateTime.Parse(Me.txtExpDate.SelectedDate) <= DateTime.Parse(Me.txtStartDate.SelectedDate) Then
            Me.MPEDetails.VisibleOnPageLoad = True
            ''  MessageBoxValidation("Expiry date should be greater than start date.", "Validation")
            lblPop.Text = "Expiry date should be greater than start date."
            Exit Sub
        End If
        Dim success As Boolean = False
        Try

            objSurvey.Title = IIf(Me.txtTitle.Text = "", "0", Me.txtTitle.Text)
            objSurvey.TypeCode = Me.ddlTypeCode.SelectedValue.ToString()
            objSurvey.StartDate = DateTime.Parse(Me.txtStartDate.SelectedDate)
            objSurvey.ExpiryDate = DateTime.Parse(Me.txtExpDate.SelectedDate)


            If objSurvey.CheckduplicateSurvey(Err_No, Err_Desc) = True Then
                '' MessageBoxValidation("Survey title already Exist.", "Validation")
                lblPop.Text = "Survey title already Exist."
                Me.MPEDetails.VisibleOnPageLoad = True
                Exit Sub
            End If
            For Each itm As RadListBoxItem In Me.chkSalRep.Items
                If itm.Checked = True Then
                    Dim dr As DataRow = dtSalesRepSelected.NewRow()
                    dr(1) = itm.Value
                    dtSalesRepSelected.Rows.Add(dr)
                End If
            Next
            objSurvey._dtSR = dtSalesRepSelected
            If objSurvey.AddSurvey(Err_No, Err_Desc) = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "SURVEY", "ADMIN SURVEY", Me.txtTitle.Text, "Survey Title: " & Me.txtTitle.Text & "/ Start Date :  " & Me.txtStartDate.SelectedDate & "/ End Date :  " & Me.txtStartDate.SelectedDate & "/ Type : " & Me.ddlTypeCode.SelectedItem.Text & "/ Assigned Sales Rep : " & dtSalesRepSelected.Rows.Count, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                success = True
                MessageBoxValidation("Successfully saved.", "Information")
            End If

            If success = True Then
                Dt = objSurvey.LoadSurveyData(Err_No, Err_Desc)
                BindSurveyData()
                ResetFields()
                ClassUpdatePnl.Update()
                Me.MPEDetails.VisibleOnPageLoad = False
            Else

                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_AdminSurvey_001") & "&next=AdminSurvey.aspx&Title=Message", False)
                Exit Try
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

    '' ''Protected Sub ddSurvey_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddSurvey.SelectedIndexChanged
    '' ''If Me.ddSurvey.SelectedIndex > 0 Then

    '' ''    Dt = objSurvey.EditSurveys(Err_No, Err_Desc, Me.ddSurvey.SelectedValue.ToString())
    '' ''    If Dt.Rows.Count > 0 Then
    '' ''        Me.txtTitle.Text = Dt.Rows(0)("Survey_Title").ToString()
    '' ''        Me.ddlTypeCode.SelectedValue = Dt.Rows(0)("Survey_Type_Code").ToString()
    '' ''        Me.txtStartDate.SelectedDate = DateTime.Parse(Dt.Rows(0)("Start_Time").ToString()).ToString("MM/dd/yyyy")
    '' ''        Me.txtExpDate.SelectedDate = DateTime.Parse(Dt.Rows(0)("End_Time").ToString()).ToString("MM/dd/yyyy")
    '' ''        Me.btnModify.Enabled = True
    '' ''        Me.btnDelete.Enabled = True
    '' ''    End If

    '' ''    Dim Temptbl As New DataTable
    '' ''    Temptbl = objSurvey.LoadSurveysFSR(Err_No, Err_Desc, Me.ddSurvey.SelectedValue.ToString())
    '' ''    Me.chkSalRep.SelectedValue = Nothing
    '' ''    If Temptbl.Rows.Count > 0 Then
    '' ''        For Each dr As DataRow In Temptbl.Rows
    '' ''            For Each itm As ListItem In Me.chkSalRep.Items
    '' ''                If itm.Value = dr(1).ToString() Then
    '' ''                    itm.Selected = True
    '' ''                End If
    '' ''            Next
    '' ''        Next
    '' ''    End If
    '' ''    ToggleEnabled(False)
    '' ''Else
    '' ''    ResetFields()

    '' ''    ToggleEnabled(True)
    '' ''End If
    '' ''End Sub

    '' ''Protected Sub btnModify_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModify.Click
    '' ''If Me.ddSurvey.SelectedIndex > 0 Then
    '' ''    'If DateTime.Parse(Me.txtStartDate.Text) <= Now.Date Then
    '' ''    '    Me.lblinfo.Text = "Validation"
    '' ''    '    Me.lblMessage.Text = "Start date should be greater than today date for modify."
    '' ''    '    Me.lblMessage.ForeColor = Drawing.Color.Red
    '' ''    '    Me.MpInfoError.Show()
    '' ''    '    Exit Sub
    '' ''    'End If
    '' ''    If Now.Date <= DateTime.Parse(Me.txtExpDate.SelectedDate) Then
    '' ''        ToggleEnabled(False)
    '' ''        Me.chkSalRep.Enabled = True
    '' ''        Me.txtTitle.Enabled = False
    '' ''        Me.ddSurvey.Visible = False
    '' ''        Me.lblSurvey.Visible = False
    '' ''        Me.lblTitle.Visible = True
    '' ''        Me.txtTitle.Visible = True
    '' ''        Me.btnAdd.Visible = False
    '' ''        Me.btnModify.Visible = False
    '' ''        Me.btnDelete.Visible = False
    '' ''        Me.btnCancel.Visible = True
    '' ''        Me.btnSave.Visible = True
    '' ''        Me.btnSave.Text = "Update"
    '' ''        Exit Sub
    '' ''    Else
    '' ''        MessageBoxValidation("End date should be greater than or equal to today date for modify.", "Validation")
    '' ''        Exit Sub
    '' ''    End If
    '' ''    'ToggleEnabled(True)
    '' ''    'Me.ddSurvey.Visible = False
    '' ''    'Me.lblSurvey.Visible = False
    '' ''    'Me.lblTitle.Visible = True
    '' ''    'Me.txtTitle.Visible = True
    '' ''    'Me.btnAdd.Visible = False
    '' ''    'Me.btnModify.Visible = False
    '' ''    'Me.btnDelete.Visible = False
    '' ''    'Me.btnCancel.Visible = True
    '' ''    'Me.btnSave.Visible = True
    '' ''    'Me.btnSave.Text = "Update"
    '' ''End If
    '' ''End Sub

    '' ''Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
    '' ''Dim success As Boolean = False
    '' ''If Me.ddSurvey.SelectedIndex > 0 Then
    '' ''    If DateTime.Parse(Me.txtStartDate.SelectedDate) <= Now.Date Then
    '' ''        MessageBoxValidation("Start date should be greater than today's date for delete.", "Validation")
    '' ''        Exit Sub
    '' ''    End If

    '' ''    For Each itm As ListItem In Me.chkSalRep.Items
    '' ''        If itm.Selected = True Then
    '' ''            Dim dr As DataRow
    '' ''            dr = dtSalesRepSelected.NewRow
    '' ''            dr(0) = Me.ddSurvey.SelectedValue.ToString()
    '' ''            dr(1) = itm.Value.ToString()
    '' ''            dtSalesRepSelected.Rows.Add(dr)
    '' ''        End If
    '' ''    Next

    '' ''    If objSurvey.RemoveSurvey(Err_No, Err_Desc, Me.ddSurvey.SelectedValue.ToString()) = True Then
    '' ''        objLogin.SaveUserLog(Err_No, Err_Desc, "D", "SURVEY", "ADMIN SURVEY", Me.txtTitle.Text, "Survey Title: " & Me.txtTitle.Text & "/ Start Date :  " & Me.txtStartDate.SelectedDate & "/ End Date :  " & Me.txtStartDate.SelectedDate & "/ Type : " & Me.ddlTypeCode.SelectedItem.Text & "/ Assigned Sales Rep : " & dtSalesRepSelected.Rows.Count, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
    '' ''        success = True
    '' ''        MessageBoxValidation("uccessfully deleted.", "Information")
    '' ''    End If
    '' ''End If
    '' ''If success = True Then
    '' ''    LoadCombo()
    '' ''    ResetFields()
    '' ''    ToggleEnabled(True)
    '' ''Else

    '' ''    log.Error(Err_Desc)
    '' ''    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_AdminSurvey_001") & "&next=AdminSurvey.aspx&Title=Message", False)
    '' ''End If
    '' ''End Sub

    Private Sub ToggleEnabled(ByVal value As Boolean)
        '' ''Me.txtStartDate.Enabled = value
        '' ''Me.txtExpDate.Enabled = value
        '' ''Me.ddlTypeCode.Enabled = value
        '' ''Me.chkSalRep.Enabled = value
        ' '' ''Me.IbED.Enabled = value
        ' '' ''Me.ibSD.Enabled = value
    End Sub

    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        str = str.Replace("'", "&#39;")
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub btnDeleteAll_Click()
        Try
            Dim dr As GridViewRow
            Dim Success As Boolean = False
            For Each dr In gvSurvey.Rows
                Dim RowCheckBox As System.Web.UI.WebControls.CheckBox = dr.FindControl("chkDelete")

                If RowCheckBox.Checked = True Then
                    Dim Lbl As System.Web.UI.WebControls.Label = dr.FindControl("lblSurveyId")

                    If DateTime.Parse(dr.Cells(3).Text).ToString("MM/dd/yyyy") <= Now.Date Then
                        Continue For
                    End If

                    HidVal.Value = Lbl.Text
                    If objSurvey.RemoveSurvey(Err_No, Err_Desc, Lbl.Text) = True Then
                        objLogin.SaveUserLog(Err_No, Err_Desc, "D", "SURVEY", "ADMIN SURVEY", Me.txtTitle.Text, "Survey Title: " & Me.txtTitle.Text & "/ Start Date :  " & Me.txtStartDate.SelectedDate & "/ End Date :  " & Me.txtStartDate.SelectedDate & "/ Type : " & Me.ddlTypeCode.SelectedItem.Text & "/ Assigned Sales Rep : " & dtSalesRepSelected.Rows.Count, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                        Success = True
                    End If
                End If
            Next
            If (Success = True) Then
                MessageBoxValidation("Survey(s) deleted successfully.", "Information")
                Dt = objSurvey.LoadSurveyData(Err_No, Err_Desc)
                BindSurveyData()
                ResetFields()
            Else
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_AdminSurvey_001") & "&next=AdminSurvey.aspx&Title=Message", False)
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
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim success As Boolean = False
        Dim btnDelete As ImageButton = TryCast(sender, ImageButton)
        Dim row As GridViewRow = DirectCast(btnDelete.NamingContainer, GridViewRow)
        Dim lblId As Label = DirectCast(row.FindControl("lblSurveyId"), Label)

        If lblId.Text > 0 Then
            If DateTime.Parse(row.Cells(3).Text).ToString("MM/dd/yyyy") <= Now.Date Then
                MessageBoxValidation("Start date should be greater than today's date for delete.", "Validation")
                ''  MessageBoxValidation("Successfully deleted.", "Information")
                Exit Sub
            End If

            For Each itm As RadListBoxItem In Me.chkSalRep.Items
                If itm.Checked = True Then
                    Dim dr As DataRow
                    dr = dtSalesRepSelected.NewRow
                    dr(0) = lblId.Text
                    dr(1) = itm.Value.ToString()
                    dtSalesRepSelected.Rows.Add(dr)
                End If
            Next

            If objSurvey.RemoveSurvey(Err_No, Err_Desc, lblId.Text) = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "D", "SURVEY", "ADMIN SURVEY", Me.txtTitle.Text, "Survey Title: " & Me.txtTitle.Text & "/ Start Date :  " & Me.txtStartDate.SelectedDate & "/ End Date :  " & Me.txtStartDate.SelectedDate & "/ Type : " & Me.ddlTypeCode.SelectedItem.Text & "/ Assigned Sales Rep : " & dtSalesRepSelected.Rows.Count, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                success = True
                MessageBoxValidation("Successfully deleted.", "Information")
            End If
        End If
        If success = True Then
            Dt = objSurvey.LoadSurveyData(Err_No, Err_Desc)
            BindSurveyData()
        Else

            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_AdminSurvey_001") & "&next=AdminSurvey.aspx&Title=Message", False)
        End If
    End Sub
    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            btnUpdate.Visible = True
            btnSave.Visible = False
            ResetFields()
            Me.txtTitle.Enabled = False
            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
            Dim lblId As Label = DirectCast(row.FindControl("lblSurveyId"), Label)
            Dim lblType As Label = DirectCast(row.FindControl("TypeCode"), Label)
            HidVal.Value = lblId.Text
            Me.txtTitle.Text = Trim(row.Cells(1).Text)
            Me.ddlTypeCode.SelectedValue = lblType.Text
            Me.txtStartDate.SelectedDate = DateTime.Parse(row.Cells(3).Text).ToString("MM/dd/yyyy")
            Me.txtExpDate.SelectedDate = DateTime.Parse(row.Cells(4).Text).ToString("MM/dd/yyyy")

            Dim Temptbl As New DataTable
            Temptbl = objSurvey.LoadSurveysFSR(Err_No, Err_Desc, lblId.Text.ToString())
            Me.chkSalRep.SelectedValue = Nothing
            Me.chkSalRep.ClearChecked()
            If Temptbl.Rows.Count > 0 Then
                For Each dr As DataRow In Temptbl.Rows
                    For Each itm As RadListBoxItem In Me.chkSalRep.Items
                        If itm.Value = dr(1).ToString() Then
                            itm.Checked = True
                        End If
                    Next
                Next
            End If

            MPEDetails.VisibleOnPageLoad = True
        Catch ex As Exception
            Err_No = "74066"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_AdminSurvey_001") & "&next=AdminSurvey.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Protected Sub btnView_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try

            ResetFields()
            btnUpdate.Visible = False
            btnSave.Visible = False
            Me.txtTitle.Enabled = False
            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
            Dim lblId As Label = DirectCast(row.FindControl("lblSurveyId"), Label)
            Dim lblType As Label = DirectCast(row.FindControl("TypeCode"), Label)
            HidVal.Value = lblId.Text
            Me.txtTitle.Text = Trim(row.Cells(1).Text)
            Me.ddlTypeCode.SelectedValue = lblType.Text
            Me.txtStartDate.SelectedDate = DateTime.Parse(row.Cells(3).Text).ToString("MM/dd/yyyy")
            Me.txtExpDate.SelectedDate = DateTime.Parse(row.Cells(4).Text).ToString("MM/dd/yyyy")

            Dim Temptbl As New DataTable
            Temptbl = objSurvey.LoadSurveysFSR(Err_No, Err_Desc, lblId.Text.ToString())
            Me.chkSalRep.SelectedValue = Nothing
            Me.chkSalRep.ClearChecked()
            If Temptbl.Rows.Count > 0 Then
                For Each dr As DataRow In Temptbl.Rows
                    For Each itm As RadListBoxItem In Me.chkSalRep.Items
                        If itm.Value = dr(1).ToString() Then
                            itm.Checked = True
                        End If
                    Next
                Next
            End If

            MPEDetails.VisibleOnPageLoad = True
            txtTitle.Enabled = False
            ddlTypeCode.Enabled = False
            txtStartDate.Enabled = False
            txtExpDate.Enabled = False
            chkSalRep.Enabled = False
        Catch ex As Exception
            Err_No = "74066"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_AdminSurvey_001") & "&next=AdminSurvey.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub
    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Me.txtTitle.Text = "" Or Me.txtStartDate.SelectedDate Is Nothing Or Me.txtExpDate.SelectedDate Is Nothing Then
            '''' MessageBoxValidation("Title,start and expiry date required.", "Validation")
            lblPop.Text = "Title,start and expiry date required."
            Me.MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If

        If DateTime.Parse(Me.txtStartDate.SelectedDate) <= Now.Date And Me.btnSave.Text = "Save" Then
            '' MessageBoxValidation("Start date should be greater than today's date.", "Validation")
            lblPop.Text = "Start date should be greater than today's date."
            Me.MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If


        If DateTime.Parse(Me.txtExpDate.SelectedDate) <= DateTime.Parse(Me.txtStartDate.SelectedDate) Then
            ''  MessageBoxValidation("Expiry date should be greater than start date.", "Validation")
            lblPop.Text = "Expiry date should be greater than start date."
            Me.MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If
        Dim success As Boolean = False
        Try

            objSurvey.Title = IIf(Me.txtTitle.Text = "", "0", Me.txtTitle.Text)
            objSurvey.TypeCode = Me.ddlTypeCode.SelectedValue.ToString()
            objSurvey.StartDate = DateTime.Parse(Me.txtStartDate.SelectedDate)
            objSurvey.ExpiryDate = DateTime.Parse(Me.txtExpDate.SelectedDate)



            For Each itm As RadListBoxItem In Me.chkSalRep.Items
                If itm.Checked = True Then
                    Dim dr As DataRow
                    dr = dtSalesRepSelected.NewRow
                    dr(0) = HidVal.Value.ToString()
                    dr(1) = itm.Value.ToString()
                    dtSalesRepSelected.Rows.Add(dr)
                End If
            Next
            objSurvey._dtSR = dtSalesRepSelected
            If objSurvey.ModifySurvey(Err_No, Err_Desc, HidVal.Value) = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "U", "SURVEY", "ADMIN SURVEY", Me.txtTitle.Text, "Survey Title: " & Me.txtTitle.Text & "/ Start Date :  " & Me.txtStartDate.SelectedDate & "/ End Date :  " & Me.txtStartDate.SelectedDate & "/ Type : " & Me.ddlTypeCode.SelectedItem.Text & "/ Assigned Sales Rep : " & dtSalesRepSelected.Rows.Count, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                success = True
                MessageBoxValidation("Successfully updated.", "Information")
            End If

            If success = True Then
                Dt = objSurvey.LoadSurveyData(Err_No, Err_Desc)
                BindSurveyData()
                LoadCombo()
                ResetFields()
                ClassUpdatePnl.Update()
                Me.MPEDetails.VisibleOnPageLoad = False
            Else

                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_AdminSurvey_001") & "&next=AdminSurvey.aspx&Title=Message", False)
                Exit Try
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

    Private Sub gvSurvey_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvSurvey.PageIndexChanging
        gvSurvey.PageIndex = e.NewPageIndex
        Dt = objSurvey.LoadSurveyData(Err_No, Err_Desc)
        BindSurveyData()
    End Sub

    Private Sub gvSurvey_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvSurvey.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            '' Disabling edit and delete button for the surveys those have past dates

            If DateTime.Parse(e.Row.Cells(4).Text) <= Now.Date Then
                Dim ImgEdit As ImageButton = DirectCast(e.Row.FindControl("btnEdit"), ImageButton)
                Dim ImgDelete As ImageButton = DirectCast(e.Row.FindControl("btnDelete"), ImageButton)
                Dim chk As CheckBox = DirectCast(e.Row.FindControl("chkDelete"), CheckBox)
                ImgEdit.Visible = False
                ImgDelete.Visible = False
                chk.Visible = False
            End If


            Dim cls As New Purpose
            Dim ht As Hashtable = cls.BindSurveyTypeCode

            Dim keys As New List(Of String)(ht.Keys.OfType(Of String)())

            Dim key As String = ht.Values.OfType(Of String)() _
              .Select(Function(htI, i) New With {.Key = keys(i), .Value = htI}) _
              .Where(Function(htKVP) htKVP.Value = e.Row.Cells(2).Text) _
              .Select(Function(htKVP) htKVP.Key) _
              .FirstOrDefault()

            If Not String.IsNullOrEmpty(key) Then
                e.Row.Cells(2).Text = key
            End If
        End If

    End Sub

    Private Sub gvSurvey_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvSurvey.Sorting
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        Dt = objSurvey.LoadSurveyData(Err_No, Err_Desc)
        BindSurveyData()
    End Sub

    Private Sub btnFilter_Click(sender As Object, e As EventArgs) Handles btnFilter.Click
        Dim success As Boolean = False
        Try
            Dt = objSurvey.SearchSurveyGrid(Err_No, Err_Desc, Me.ddFilterBy.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text))
            BindSurveyData()
            ClassUpdatePnl.Update()
            success = True
            If success = False Then
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_CurrencyCode_004") & "&next=CurrencyCode.aspx&Title=Message", False)
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
End Class