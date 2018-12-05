Imports SalesWorx.BO.Common
Imports System.Data
Imports System.Web
Imports System.Resources
Imports System.Web.UI.WebControls
Imports log4net
Imports System.IO
Imports System.Data.SqlClient

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
        Else

            dtSalesRepSelected = TryCast(ViewState("dtSR"), DataTable)


        End If
        ViewState("dtSR") = dtSalesRepSelected
    End Sub

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
        ddSurvey.DataSource = objSurvey.LoadSurveys(Err_No, Err_Desc)
        ddSurvey.DataTextField = "Survey_title"
        ddSurvey.DataValueField = "Survey_Id"
        ddSurvey.DataBind()
    End Sub

    Private Sub ResetFields()
        ddlTypeCode.Items.Clear()
        Dim cls As New Purpose
        ddlTypeCode.DataSource = cls.BindSurveyTypeCode
        ddlTypeCode.DataTextField = "Key"
        ddlTypeCode.DataValueField = "Value"
        ddlTypeCode.DataBind()
        Me.txtExpDate.Text = ""
        Me.txtStartDate.Text = ""
        Me.txtTitle.Text = ""
        Me.ddlTypeCode.SelectedIndex = 1
        Me.ddSurvey.SelectedIndex = 0
        Me.ddSurvey.Visible = True
        Me.lblSurvey.Visible = True
        Me.lblTitle.Visible = False
        Me.txtTitle.Visible = False
        Me.txtTitle.Enabled = True
        Me.btnAdd.Visible = True
        Me.btnModify.Visible = True
        Me.btnDelete.Visible = True
        Me.btnCancel.Visible = False
        Me.btnSave.Visible = False
        Me.btnSave.Text = "Save"
        Me.btnModify.Enabled = False
        Me.btnDelete.Enabled = False
        Me.chkSalRep.SelectedValue = Nothing
        If dtSalesRepSelected.Rows.Count > 0 Then
            dtSalesRepSelected.Rows.Clear()
        End If
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
        ResetFields()
        ToggleEnabled(True)
        Me.ddSurvey.Visible = False
        Me.lblSurvey.Visible = False
        Me.lblTitle.Visible = True
        Me.txtTitle.Visible = True
        Me.btnAdd.Visible = False
        Me.btnModify.Visible = False
        Me.btnDelete.Visible = False
        Me.btnCancel.Visible = True
        Me.btnSave.Visible = True
        Me.btnSave.Text = "Save"
        Me.txtStartDate.Text = Now.Date.AddDays(1).ToString("MM/dd/yyyy")
        Me.txtExpDate.Text = Now.Date.AddMonths(1).ToString("MM/dd/yyyy")
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        ResetFields()
    End Sub


    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        
        If Me.txtTitle.Text = "" Or Me.txtStartDate.Text = "" Or Me.txtExpDate.Text = "" Then
            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Title,start and expiry date required."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Exit Sub
        End If

        If DateTime.Parse(Me.txtStartDate.Text) <= Now.Date And Me.btnSave.Text = "Save" Then
            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Start date should be greater than today's date."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Exit Sub
        End If


        If DateTime.Parse(Me.txtExpDate.Text) <= DateTime.Parse(Me.txtStartDate.Text) Then
            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Expiry date should be greater than start date."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Exit Sub
        End If
        Dim success As Boolean = False
        Try

            objSurvey.Title = IIf(Me.txtTitle.Text = "", "0", Me.txtTitle.Text)
            objSurvey.TypeCode = Me.ddlTypeCode.SelectedValue.ToString()
            objSurvey.StartDate = DateTime.Parse(Me.txtStartDate.Text)
            objSurvey.ExpiryDate = DateTime.Parse(Me.txtExpDate.Text)


            If Me.btnSave.Text = "Save" Then
                If objSurvey.CheckduplicateSurvey(Err_No, Err_Desc) = True Then
                    Me.lblinfo.Text = "Validation"
                    Me.lblMessage.Text = "Survey title already Exist."
                    Me.lblMessage.ForeColor = Drawing.Color.Red
                    Me.MpInfoError.Show()
                    Exit Sub
                End If
                For Each itm As ListItem In Me.chkSalRep.Items
                    If itm.Selected = True Then
                        Dim dr As DataRow = dtSalesRepSelected.NewRow()
                        dr(1) = itm.Value
                        dtSalesRepSelected.Rows.Add(dr)
                    End If
                Next
                objSurvey._dtSR = dtSalesRepSelected
                If objSurvey.AddSurvey(Err_No, Err_Desc) = True Then
                    objLogin.SaveUserLog(Err_No, Err_Desc, "I", "SURVEY", "ADMIN SURVEY", Me.txtTitle.Text, "Survey Title: " & Me.txtTitle.Text & "/ Start Date :  " & Me.txtStartDate.Text & "/ End Date :  " & Me.txtStartDate.Text & "/ Type : " & Me.ddlTypeCode.SelectedItem.Text & "/ Assigned Sales Rep : " & dtSalesRepSelected.Rows.Count, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                    success = True
                    Me.lblMessage.Text = "Successfully saved."
                    lblMessage.ForeColor = Drawing.Color.Green
                    lblinfo.Text = "Information"
                    MpInfoError.Show()
                    btnClose.Focus()
                End If
            ElseIf Me.btnSave.Text = "Update" Then
                For Each itm As ListItem In Me.chkSalRep.Items
                    If itm.Selected = True Then
                        Dim dr As DataRow
                        dr = dtSalesRepSelected.NewRow
                        dr(0) = Me.ddSurvey.SelectedValue.ToString()
                        dr(1) = itm.Value.ToString()
                        dtSalesRepSelected.Rows.Add(dr)
                    End If
                Next
                objSurvey._dtSR = dtSalesRepSelected
                If objSurvey.ModifySurvey(Err_No, Err_Desc, Me.ddSurvey.SelectedValue.ToString()) = True Then
                    objLogin.SaveUserLog(Err_No, Err_Desc, "U", "SURVEY", "ADMIN SURVEY", Me.txtTitle.Text, "Survey Title: " & Me.txtTitle.Text & "/ Start Date :  " & Me.txtStartDate.Text & "/ End Date :  " & Me.txtStartDate.Text & "/ Type : " & Me.ddlTypeCode.SelectedItem.Text & "/ Assigned Sales Rep : " & dtSalesRepSelected.Rows.Count, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                    success = True
                    Me.lblMessage.Text = "Successfully updated."
                    lblMessage.ForeColor = Drawing.Color.Green
                    lblinfo.Text = "Information"
                    MpInfoError.Show()
                    btnClose.Focus()
                End If
            End If
            If success = True Then
                LoadCombo()
                ResetFields()

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

    Protected Sub ddSurvey_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddSurvey.SelectedIndexChanged
        If Me.ddSurvey.SelectedIndex > 0 Then

            Dt = objSurvey.EditSurveys(Err_No, Err_Desc, Me.ddSurvey.SelectedValue.ToString())
            If Dt.Rows.Count > 0 Then
                Me.txtTitle.Text = Dt.Rows(0)("Survey_Title").ToString()
                Me.ddlTypeCode.SelectedValue = Dt.Rows(0)("Survey_Type_Code").ToString()
                Me.txtStartDate.Text = DateTime.Parse(Dt.Rows(0)("Start_Time").ToString()).ToString("MM/dd/yyyy")
                Me.txtExpDate.Text = DateTime.Parse(Dt.Rows(0)("End_Time").ToString()).ToString("MM/dd/yyyy")
                Me.btnModify.Enabled = True
                Me.btnDelete.Enabled = True
            End If

            Dim Temptbl As New DataTable
            Temptbl = objSurvey.LoadSurveysFSR(Err_No, Err_Desc, Me.ddSurvey.SelectedValue.ToString())
            Me.chkSalRep.SelectedValue = Nothing
            If Temptbl.Rows.Count > 0 Then
                For Each dr As DataRow In Temptbl.Rows
                    For Each itm As ListItem In Me.chkSalRep.Items
                        If itm.Value = dr(1).ToString() Then
                            itm.Selected = True
                        End If
                    Next
                Next
            End If
            ToggleEnabled(False)
        Else
            ResetFields()

            ToggleEnabled(True)
        End If
    End Sub

    Protected Sub btnModify_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModify.Click
        If Me.ddSurvey.SelectedIndex > 0 Then
            'If DateTime.Parse(Me.txtStartDate.Text) <= Now.Date Then
            '    Me.lblinfo.Text = "Validation"
            '    Me.lblMessage.Text = "Start date should be greater than today date for modify."
            '    Me.lblMessage.ForeColor = Drawing.Color.Red
            '    Me.MpInfoError.Show()
            '    Exit Sub
            'End If
            If Now.Date <= DateTime.Parse(Me.txtExpDate.Text) Then
                ToggleEnabled(False)
                Me.chkSalRep.Enabled = True
                Me.txtTitle.Enabled = False
                Me.ddSurvey.Visible = False
                Me.lblSurvey.Visible = False
                Me.lblTitle.Visible = True
                Me.txtTitle.Visible = True
                Me.btnAdd.Visible = False
                Me.btnModify.Visible = False
                Me.btnDelete.Visible = False
                Me.btnCancel.Visible = True
                Me.btnSave.Visible = True
                Me.btnSave.Text = "Update"
                Exit Sub
            Else
                Me.lblinfo.Text = "Validation"
                Me.lblMessage.Text = "End date should be greater than or equal to today date for modify."
                Me.lblMessage.ForeColor = Drawing.Color.Red
                Me.MpInfoError.Show()
                Exit Sub
            End If
            'ToggleEnabled(True)
            'Me.ddSurvey.Visible = False
            'Me.lblSurvey.Visible = False
            'Me.lblTitle.Visible = True
            'Me.txtTitle.Visible = True
            'Me.btnAdd.Visible = False
            'Me.btnModify.Visible = False
            'Me.btnDelete.Visible = False
            'Me.btnCancel.Visible = True
            'Me.btnSave.Visible = True
            'Me.btnSave.Text = "Update"
        End If
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
        Dim success As Boolean = False
        If Me.ddSurvey.SelectedIndex > 0 Then
            If DateTime.Parse(Me.txtStartDate.Text) <= Now.Date Then
                Me.lblinfo.Text = "Validation"
                Me.lblMessage.Text = "Start date should be greater than today's date for delete."
                Me.lblMessage.ForeColor = Drawing.Color.Red
                Me.MpInfoError.Show()
                Exit Sub
            End If

            For Each itm As ListItem In Me.chkSalRep.Items
                If itm.Selected = True Then
                    Dim dr As DataRow
                    dr = dtSalesRepSelected.NewRow
                    dr(0) = Me.ddSurvey.SelectedValue.ToString()
                    dr(1) = itm.Value.ToString()
                    dtSalesRepSelected.Rows.Add(dr)
                End If
            Next

            If objSurvey.RemoveSurvey(Err_No, Err_Desc, Me.ddSurvey.SelectedValue.ToString()) = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "D", "SURVEY", "ADMIN SURVEY", Me.txtTitle.Text, "Survey Title: " & Me.txtTitle.Text & "/ Start Date :  " & Me.txtStartDate.Text & "/ End Date :  " & Me.txtStartDate.Text & "/ Type : " & Me.ddlTypeCode.SelectedItem.Text & "/ Assigned Sales Rep : " & dtSalesRepSelected.Rows.Count, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                success = True
                Me.lblMessage.Text = "Successfully deleted."
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
            End If
        End If
        If success = True Then
            LoadCombo()
            ResetFields()
            ToggleEnabled(True)
        Else

            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_AdminSurvey_001") & "&next=AdminSurvey.aspx&Title=Message", False)
        End If
    End Sub

    Private Sub ToggleEnabled(ByVal value As Boolean)
        Me.txtStartDate.Enabled = value
        Me.txtExpDate.Enabled = value
        Me.ddlTypeCode.Enabled = value
        Me.chkSalRep.Enabled = value
        Me.IbED.Enabled = value
        Me.ibSD.Enabled = value
    End Sub
End Class