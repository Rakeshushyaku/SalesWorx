Imports SalesWorx.BO.Common
Imports System.Data
Imports System.Web
Imports System.Resources
Imports System.Web.UI.WebControls
Imports log4net
Imports System.IO
Imports System.Data.SqlClient
Imports Telerik.Web.UI

Partial Public Class AdminSurveyResp
    Inherits System.Web.UI.Page
    Private dtResponse As DataTable
    Private objSurvey As New Survey
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim Dt As New DataTable
    Dim frmStatus As String = ""
    Private Const PageID As String = "P14"
    Private MaxOptCnt As Integer = Integer.Parse(ConfigurationSettings.AppSettings("MaxOptCount").ToString())
    Private MinOptCnt As Integer = Integer.Parse(ConfigurationSettings.AppSettings("MinOptCount").ToString())
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
            LoadSurvey()
            LoadResponseType()
            dtResponse = New DataTable()
            SetResponseTable()
            Me.ddlOptCount.Items.Clear()
            While MinOptCnt <= MaxOptCnt
                Me.ddlOptCount.Items.Add(New RadComboBoxItem(MinOptCnt, MinOptCnt))
                MinOptCnt += 1
            End While
            ResetFields()
            ToggleEnabled(False)
            Me.ddlQuestions.Items.Clear()
            ddlQuestions.AppendDataBoundItems = True
            ddlQuestions.Items.Insert(0, New RadComboBoxItem("--Select a Question--", ""))
            '' ''ddlQuestions.Items(0).Value = ""
        Else

            dtResponse = TryCast(ViewState("dtR"), DataTable)
            frmStatus = ViewState("frmStatus")
        End If
        ViewState("dtR") = dtResponse
        ViewState("frmStatus") = frmStatus


    End Sub

    Private Sub LoadSurvey()
        ddSurvey.DataSource = objSurvey.LoadSurveys(Err_No, Err_Desc)
        ddSurvey.DataTextField = "Survey_title"
        ddSurvey.DataValueField = "Survey_Id"
        ddSurvey.DataBind()
        ddSurvey.SelectedIndex = 0
    End Sub

    Private Sub LoadResponseType()
        ddlResponsType.Items.Clear()

        ddlResponsType.DataSource = objSurvey.LoadResponseTypes(Err_No, Err_Desc, IIf(Me.ddSurvey.SelectedIndex <= 0, "0", Me.ddSurvey.SelectedValue))
        ddlResponsType.DataTextField = "Response_Type"
        ddlResponsType.DataValueField = "Response_Type_Id"
        ddlResponsType.DataBind()
        ddlResponsType.SelectedIndex = 0
    End Sub

    Private Sub SetResponseTable()

        Dim col As DataColumn

        col = New DataColumn()
        col.ColumnName = "SrNo"
        col.DataType = System.Type.GetType("System.Int32")
        col.ReadOnly = False
        col.Unique = False
        dtResponse.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "OptValue"
        col.DataType = System.Type.GetType("System.String")
        col.ReadOnly = False
        col.Unique = False
        dtResponse.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "DefValue"
        col.DataType = System.Type.GetType("System.Boolean")
        col.ReadOnly = False
        col.Unique = False
        dtResponse.Columns.Add(col)


    End Sub

    Protected Sub ddlOptCount_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOptCount.SelectedIndexChanged
        If frmStatus = "NEW" Then
            If dtResponse.Rows.Count > 0 Then
                dtResponse.Rows.Clear()
            End If
            Dim optE As Integer = 1
            While optE <= Integer.Parse(Me.ddlOptCount.SelectedValue.ToString())
                Dim dr As DataRow = dtResponse.NewRow
                dr(0) = optE
                dr(1) = ""
                dr(2) = 0
                dtResponse.Rows.Add(dr)
                optE += 1
            End While
            Me.gvResponse.DataSource = dtResponse
            Me.gvResponse.DataBind()
            Me.gvResponse.Visible = True


        ElseIf frmStatus = "EDIT" Then
            If dtResponse.Rows.Count < Integer.Parse(Me.ddlOptCount.SelectedValue.ToString()) Then
                Dim optE As Integer = dtResponse.Rows.Count
                While optE < Integer.Parse(Me.ddlOptCount.SelectedValue.ToString())
                    Dim dr As DataRow = dtResponse.NewRow
                    dr(0) = dtResponse.Rows.Count + 1
                    dr(1) = ""
                    dr(2) = 0
                    dtResponse.Rows.Add(dr)
                    optE += 1
                End While
                Me.gvResponse.DataSource = dtResponse
                Me.gvResponse.DataBind()
                Me.gvResponse.Visible = True
            End If

            If dtResponse.Rows.Count > Integer.Parse(Me.ddlOptCount.SelectedValue.ToString()) Then
                Dim OptDelete As Integer = (dtResponse.Rows.Count) - (Integer.Parse(Me.ddlOptCount.SelectedValue.ToString()))
                While OptDelete > 0
                    dtResponse.Rows.RemoveAt(dtResponse.Rows.Count - 1)
                    OptDelete -= 1
                End While
                Me.gvResponse.DataSource = dtResponse
                Me.gvResponse.DataBind()
                Me.gvResponse.Visible = True
            End If
        End If
        Me.gvResponse.Focus()
    End Sub

    Protected Sub ChkDefault_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Me.gvResponse.Focus()
        UpdateGrid()
        Dim chk As CheckBox = TryCast(sender, CheckBox)
        Dim row As GridViewRow = TryCast(chk.NamingContainer, GridViewRow)
        Dim txtOptValue As TextBox = TryCast(row.FindControl("txtOptValue"), TextBox)
        If txtOptValue.Text <> "" Then
            dtResponse.Rows(row.RowIndex)(1) = txtOptValue.Text
            dtResponse.Rows(row.RowIndex)(2) = IIf(chk.Checked = True, True, False)
            For i As Integer = 0 To dtResponse.Rows.Count - 1
                If i <> row.RowIndex And CBool(dtResponse.Rows(i)(2)) = True Then
                    dtResponse.Rows(i)(2) = False
                End If
            Next
        Else
            txtOptValue.Text = ""
            chk.Checked = False
            dtResponse.Rows(row.RowIndex)(1) = txtOptValue.Text
            dtResponse.Rows(row.RowIndex)(2) = IIf(chk.Checked = True, True, False)
        End If
        dtResponse.AcceptChanges()

        ''Dim gvr As GridViewRow = Me.gvResponse.Rows(gvResponse.Rows.Count - 1)
        ''Dim txt As TextBox = TryCast(gvr.FindControl("txtOptValue"), TextBox)
        ''txt.Focus()

        Me.gvResponse.DataSource = dtResponse
        Me.gvResponse.DataBind()
        ''For Each r As GridViewRow In Me.gvResponse.Rows
        ''    If (r.RowType = DataControlRowType.DataRow) Then
        ''        Dim txt As TextBox = TryCast(r.FindControl("txtOptValue"), TextBox)
        ''        Dim ch As CheckBox = TryCast(r.FindControl("ChkDefault"), CheckBox)
        ''        If txt.Text = "" Then
        ''            dtResponse.Rows(r.RowIndex)(1) = ""
        ''            dtResponse.Rows(r.RowIndex)(2) = False
        ''        End If
        ''    End If
        ''Next
        'dtResponse.AcceptChanges()

        ' UpdateGrid(row.RowIndex)

    End Sub

    Private Sub UpdateGrid()
        For Each r As GridViewRow In Me.gvResponse.Rows
            If (r.RowType = DataControlRowType.DataRow) Then
                Dim txt As TextBox = TryCast(r.FindControl("txtOptValue"), TextBox)
                Dim ch As CheckBox = TryCast(r.FindControl("ChkDefault"), CheckBox)
                If txt.Text = "" Then
                    dtResponse.Rows(r.RowIndex)(1) = ""
                    dtResponse.Rows(r.RowIndex)(2) = False
                Else
                    dtResponse.Rows(r.RowIndex)(1) = txt.Text
                End If
            End If
        Next

        'For i As Integer = 0 To dtResponse.Rows.Count - 1
        '    If i <> j And CBool(dtResponse.Rows(i)(2)) = True Then
        '        dtResponse.Rows(i)(2) = False
        '    End If
        'Next


        dtResponse.AcceptChanges()
        Me.gvResponse.DataSource = dtResponse
        Me.gvResponse.DataBind()
    End Sub

    'Protected Sub RowBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
    '    If (e.Row.RowType = DataControlRowType.DataRow) Then
    '        Dim txtOptValue As TextBox = TryCast(e.Row.FindControl("txtOptValue"), TextBox)
    '        Dim chk As CheckBox = TryCast(e.Row.FindControl("ChkDefault"), CheckBox)
    '        If CBool(dtResponse.Rows(e.Row.RowIndex)(2)) = True And (txtOptValue.Text = "" Or txtOptValue.Text = "0") Then
    '            dtResponse.Rows(e.Row.RowIndex)(1) = ""
    '            dtResponse.Rows(e.Row.RowIndex)(2) = False
    '            dtResponse.AcceptChanges()
    '            Me.gvResponse.DataSource = dtResponse
    '            gvResponse.DataBind()
    '        End If
    '    End If
    'End Sub


    Private Sub ResetFields()
        ddlOptCount.SelectedIndex = 0
        Me.ddlResponsType.SelectedIndex = 0
        Me.lblOptCnt.Visible = False
        Me.ddlOptCount.Visible = False
        If dtResponse.Rows.Count > 0 Then
            dtResponse.Rows.Clear()
            Me.gvResponse.DataSource = dtResponse
            Me.gvResponse.DataBind()
        End If
        Me.gvResponse.Visible = False
        Me.btnAdd.Visible = True
        Me.btnModify.Visible = True
        Me.btnDelete.Visible = True
        Me.btnCancel.Visible = False
        Me.btnSave.Visible = False
        Me.btnSave.Text = "Save"
        Me.btnModify.Enabled = False
        Me.btnDelete.Enabled = False
        ''Me.ddlQuestions.Items.Clear()
        ''ddlQuestions.AppendDataBoundItems = True
        ''ddlQuestions.Items.Insert(0, "--Select a Question--")
        ''ddlQuestions.Items(0).Value = ""
        frmStatus = ""
        'Me.ddSurvey.Enabled = True
        ' Me.ddlQuestions.Enabled = True
        ViewState("frmStatus") = frmStatus
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
        If Me.ddSurvey.SelectedIndex > 0 Then
            objSurvey.SurveyID = IIf(Me.ddSurvey.SelectedIndex > 0, Me.ddSurvey.SelectedValue.ToString(), "0")
            If objSurvey.GetSurveyDate(Err_No, Err_Desc) <= Now.Date Then
                MessageBoxValidation("Survey start date should be greater than today's date for add response.", "Validation")
                Exit Sub
            End If
        End If
        ResetFields()
        ToggleEnabled(True)
        frmStatus = "NEW"
        Me.btnAdd.Visible = False
        Me.btnModify.Visible = False
        Me.btnDelete.Visible = False
        Me.btnCancel.Visible = True
        Me.btnSave.Visible = True
        Me.btnSave.Text = "Save"
        Me.ddlOptCount.Visible = False
        Me.lblOptCnt.Visible = False
        Me.gvResponse.Visible = False
        ViewState("frmStatus") = frmStatus
        'Me.ddSurvey.Enabled = False
        'Me.ddlQuestions.Enabled = False
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        ResetFields()
        Me.ddSurvey.SelectedIndex = 0
        ToggleEnabled(False)
        Me.ddlQuestions.Items.Clear()
        ddlQuestions.AppendDataBoundItems = True
        ddlQuestions.Items.Insert(0, New RadComboBoxItem("--Select a Question--", ""))
        '' ''ddlQuestions.Items(0).Value = ""
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        If Me.ddlQuestions.SelectedIndex <= 0 Or Me.ddlResponsType.SelectedIndex <= 0 Then
            MessageBoxValidation("Question and response type are required.", "Validation")
            Exit Sub
        End If
        UpdateGrid()
        Dim success As Boolean = False
        Try
            Dim msg As String = String.Empty

            Dim defResPonse As Boolean = False
            If Me.btnSave.Text = "Save" Then
                If ddlResponsType.SelectedValue.ToString = 2 Or ddlResponsType.SelectedValue.ToString = 3 Then
                    For Each dr As DataRow In dtResponse.Rows
                        objSurvey.ResponseQuestID = IIf(Me.ddlQuestions.SelectedIndex <= 0, "0", Me.ddlQuestions.SelectedValue.ToString())
                        objSurvey.ResponseText = IIf(dr(1) Is DBNull.Value, "0", dr(1).ToString())

                        objSurvey.ResponseTypeID = Integer.Parse(IIf(Me.ddlResponsType.SelectedIndex <= 0, "0", Me.ddlResponsType.SelectedValue.ToString()))
                        If CBool(dr(2).ToString()) = True Then
                            defResPonse = True
                        End If
                        If objSurvey.AddSurveyResponses(Err_No, Err_Desc, defResPonse) = True Then
                            objLogin.SaveUserLog(Err_No, Err_Desc, "I", "SURVEY", "SURVEY RESPONSES", Me.ddlQuestions.SelectedValue.ToString(), "Survey Title: " & Me.ddSurvey.SelectedItem.Text & "/ Question :  " & IIf(Me.ddlQuestions.SelectedIndex > 0, Me.ddlQuestions.SelectedItem.Text, "") & "/ Response :  " & IIf(dr(1) Is DBNull.Value, "0", dr(1).ToString()) & "/ Response Type:" & Me.ddlResponsType.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                            success = True
                            msg = "Successfully saved."
                            defResPonse = False
                        End If
                    Next

                Else

                    If ddlResponsType.SelectedValue.ToString() = "4" Then
                        objSurvey.ResponseText = Nothing
                    Else
                        objSurvey.ResponseText = "0"
                    End If
                    objSurvey.ResponseQuestID = IIf(Me.ddlQuestions.SelectedIndex <= 0, "0", Me.ddlQuestions.SelectedValue.ToString())
                    objSurvey.ResponseTypeID = Integer.Parse(IIf(Me.ddlResponsType.SelectedIndex <= 0, "0", Me.ddlResponsType.SelectedValue.ToString()))

                    If objSurvey.AddSurveyResponses(Err_No, Err_Desc, defResPonse) = True Then
                        objLogin.SaveUserLog(Err_No, Err_Desc, "I", "SURVEY", "SURVEY RESPONSES", Me.ddlQuestions.SelectedValue.ToString(), "Survey Title: " & Me.ddSurvey.SelectedItem.Text & "/ Question :  " & IIf(Me.ddlQuestions.SelectedIndex > 0, Me.ddlQuestions.SelectedItem.Text, "") & "/ Response :  " & "0" & "/ Response Type:" & Me.ddlResponsType.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                        success = True
                        msg = "Successfully saved."
                        defResPonse = False
                    End If

                End If


            ElseIf Me.btnSave.Text = "Update" Then
                objSurvey.RemoveSurveyResponses(Err_No, Err_Desc, IIf(Me.ddlQuestions.SelectedIndex > 0, Me.ddlQuestions.SelectedValue.ToString(), "0"))
                If ddlResponsType.SelectedValue.ToString = 2 Or ddlResponsType.SelectedValue.ToString = 3 Then
                    For Each dr As DataRow In dtResponse.Rows
                        objSurvey.ResponseQuestID = IIf(Me.ddlQuestions.SelectedIndex <= 0, "0", Me.ddlQuestions.SelectedValue.ToString())
                        objSurvey.ResponseText = IIf(dr(1) Is DBNull.Value, "0", dr(1).ToString())
                        objSurvey.ResponseTypeID = Integer.Parse(IIf(Me.ddlResponsType.SelectedIndex <= 0, "0", Me.ddlResponsType.SelectedValue.ToString()))
                        If CBool(dr(2).ToString()) = True Then
                            defResPonse = True
                        End If

                        If objSurvey.AddSurveyResponses(Err_No, Err_Desc, defResPonse) = True Then
                            objLogin.SaveUserLog(Err_No, Err_Desc, "U", "SURVEY", "SURVEY RESPONSES", Me.ddlQuestions.SelectedValue.ToString(), "Survey Title: " & Me.ddSurvey.SelectedItem.Text & "/ Question :  " & IIf(Me.ddlQuestions.SelectedIndex > 0, Me.ddlQuestions.SelectedItem.Text, "") & "/ Response :  " & IIf(dr(1) Is DBNull.Value, "0", dr(1).ToString()) & "/ Response Type:" & Me.ddlResponsType.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                            success = True
                            msg = "Successfully updated."
                            defResPonse = False
                        End If
                    Next
                Else

                    If ddlResponsType.SelectedValue.ToString() = "4" Then
                        objSurvey.ResponseText = Nothing
                    Else
                        objSurvey.ResponseText = "0"
                    End If

                    objSurvey.ResponseQuestID = IIf(Me.ddlQuestions.SelectedIndex <= 0, "0", Me.ddlQuestions.SelectedValue.ToString())
                    objSurvey.ResponseTypeID = Integer.Parse(IIf(Me.ddlResponsType.SelectedIndex <= 0, "0", Me.ddlResponsType.SelectedValue.ToString()))
                    If objSurvey.AddSurveyResponses(Err_No, Err_Desc, defResPonse) = True Then
                        objLogin.SaveUserLog(Err_No, Err_Desc, "U", "SURVEY", "SURVEY RESPONSES", Me.ddlQuestions.SelectedValue.ToString(), "Survey Title: " & Me.ddSurvey.SelectedItem.Text & "/ Question :  " & IIf(Me.ddlQuestions.SelectedIndex > 0, Me.ddlQuestions.SelectedItem.Text, "") & "/ Response :  " & "0" & "/ Response Type:" & Me.ddlResponsType.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                        success = True
                        msg = "Successfully updated."
                        defResPonse = False
                    End If
                End If
            End If
            If success = True Then
                MessageBoxValidation(msg, "Information")
                ResetFields()
                ToggleEnabled(False)
                Me.ddSurvey.SelectedIndex = 0
                Me.ddlQuestions.Items.Clear()
                ddlQuestions.AppendDataBoundItems = True
                ddlQuestions.Items.Insert(0, New RadComboBoxItem("--Select a Question--", ""))
                ''''   ddlQuestions.Items(0).Value = ""
                Me.ddlQuestions.SelectedIndex = 0
            Else

                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_AdminSurveyResp_001") & "&next=AdminSurveyResp.aspx&Title=Message", False)
                Exit Try
            End If

        Catch ex As Exception
            Err_No = "74081"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Protected Sub ddSurvey_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddSurvey.SelectedIndexChanged
        'If Me.ddSurvey.SelectedIndex > 0 And frmStatus = "" Then
        ResetFields()
        objSurvey.SurveyID = IIf(Me.ddSurvey.SelectedIndex > 0, Me.ddSurvey.SelectedValue.ToString(), "0")
        ddlQuestions.Items.Clear()
        ddlQuestions.DataSource = objSurvey.LoadQuestions(Err_No, Err_Desc)
        ddlQuestions.DataTextField = "Question_Text"
        ddlQuestions.DataValueField = "Question_Id"
        ddlQuestions.DataBind()
        ddlQuestions.SelectedIndex = 0

        LoadResponseType()
        ToggleEnabled(False)
        'Else
        'ResetFields()
        'ddlQuestions.Items.Clear()
        'ToggleEnabled(False)
        'End If
    End Sub

    Protected Sub btnModify_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModify.Click
        If Me.ddSurvey.SelectedIndex > 0 Then
            objSurvey.SurveyID = Me.ddSurvey.SelectedValue.ToString()
            If objSurvey.GetSurveyDate(Err_No, Err_Desc) <= Now.Date Then
                MessageBoxValidation("Survey start date should be greater than today's date for modify response.", "Validation")
                Exit Sub
            End If
        End If
        If Me.ddSurvey.SelectedIndex > 0 And Me.ddlQuestions.SelectedIndex > 0 Then
            Me.btnAdd.Visible = False
            Me.btnModify.Visible = False
            Me.btnDelete.Visible = False
            Me.btnCancel.Visible = True
            Me.btnSave.Visible = True
            Me.btnSave.Text = "Update"
            ToggleEnabled(True)
            Me.ddlOptCount.Enabled = True
            Me.gvResponse.Enabled = True
            ' Me.ddSurvey.Enabled = False
            'Me.ddlQuestions.Enabled = False
        Else
            ResetFields()
        End If
        frmStatus = "EDIT"
        ViewState("frmStatus") = frmStatus
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
        If Me.ddSurvey.SelectedIndex > 0 Then
            objSurvey.SurveyID = Me.ddSurvey.SelectedValue.ToString()
            If objSurvey.GetSurveyDate(Err_No, Err_Desc) <= Now.Date Then
                MessageBoxValidation("Survey start date should be greater than today's date for delete response.", "Validation")
                Exit Sub
            End If
        End If
        Dim success As Boolean = False
        If Me.ddSurvey.SelectedIndex > 0 And Me.ddlQuestions.SelectedIndex > 0 Then
            If objSurvey.RemoveSurveyResponses(Err_No, Err_Desc, Me.ddlQuestions.SelectedValue.ToString()) = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "D", "SURVEY", "SURVEY RESPONSES", Me.ddlQuestions.SelectedValue.ToString(), "Survey Title: " & Me.ddSurvey.SelectedItem.Text & "/ Question :  " & IIf(Me.ddlQuestions.SelectedIndex > 0, Me.ddlQuestions.SelectedItem.Text, "") & "/ Response :  " & "0" & "/ Response Type:" & Me.ddlResponsType.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                success = True
                MessageBoxValidation("Successfully deleted.", "Information")
            End If
        End If
        If success = True Then
            ResetFields()
            Me.ddSurvey.SelectedIndex = 0
            Me.ddlQuestions.Items.Clear()
            ddlQuestions.AppendDataBoundItems = True
            ddlQuestions.Items.Insert(0, New RadComboBoxItem("--Select a Question--", ""))
            '''' ddlQuestions.Items(0).Value = ""
            Me.ddlQuestions.SelectedIndex = 0
            ToggleEnabled(False)
        Else

            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_AdminSurvey_001") & "&next=AdminSurvey.aspx&Title=Message", False)
        End If
    End Sub

    Private Sub ToggleEnabled(ByVal value As Boolean)
        Me.ddlResponsType.Enabled = value
    End Sub

    Protected Sub ddlResponsType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlResponsType.SelectedIndexChanged
        If Me.ddlResponsType.SelectedIndex > 0 Then
            If Me.ddlResponsType.SelectedValue.ToString() = 2 Or Me.ddlResponsType.SelectedValue.ToString() = 3 Then
                Me.ddlOptCount.Visible = True
                Me.lblOptCnt.Visible = True
                If Me.ddlResponsType.SelectedValue.ToString() = 3 Then
                    Me.ddlOptCount.Text = 3
                ElseIf Me.ddlResponsType.SelectedValue.ToString() = 2 Then
                    Me.ddlOptCount.Text = 2
                End If

                If dtResponse.Rows.Count > 0 Then
                    dtResponse.Rows.Clear()
                End If
                Dim optE As Integer = 1
                While optE <= Integer.Parse(Me.ddlOptCount.SelectedValue.ToString())
                    Dim dr As DataRow = dtResponse.NewRow
                    dr(0) = optE
                    dr(1) = ""
                    dr(2) = 0
                    dtResponse.Rows.Add(dr)
                    optE += 1
                End While
                Me.gvResponse.DataSource = dtResponse
                Me.gvResponse.DataBind()
                Me.gvResponse.Visible = True
                Me.ddlOptCount.Enabled = True
                Me.gvResponse.Enabled = True

            Else
                Me.ddlOptCount.Visible = False
                Me.lblOptCnt.Visible = False
                Me.gvResponse.Visible = False
                Me.ddlOptCount.Enabled = False
                Me.gvResponse.Enabled = False
                If dtResponse.Rows.Count > 0 Then
                    dtResponse.Rows.Clear()
                    Me.gvResponse.DataSource = dtResponse
                    Me.gvResponse.DataBind()
                End If
            End If
        End If
    End Sub

    Protected Sub ddlQuestions_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlQuestions.SelectedIndexChanged
        If Me.ddSurvey.SelectedIndex > 0 And Me.ddlQuestions.SelectedIndex > 0 Then
            ResetFields()
            objSurvey.ResponseQuestID = IIf(Me.ddlQuestions.SelectedIndex > 0, Me.ddlQuestions.SelectedValue.ToString(), "0")
            If Dt.Rows.Count > 0 Then
                Dt.Rows.Clear()
            End If
            If dtResponse.Rows.Count > 0 Then
                dtResponse.Rows.Clear()
            End If
            Dt = objSurvey.EditSurveysResponses(Err_No, Err_Desc, IIf(Me.ddlQuestions.SelectedIndex > 0, Me.ddlQuestions.SelectedValue.ToString(), "0"))
            If Dt.Rows.Count > 0 Then
                If Dt.Rows(0)("Response_Type_Id") Is DBNull.Value Or Dt.Rows(0)("Response_type_Id").ToString() = "0" Then
                    Me.ddlResponsType.SelectedItem.Text = "--Select Response Type--"
                Else
                    Me.ddlResponsType.SelectedValue = Dt.Rows(0)("Response_Type_Id").ToString()
                End If
                If Me.ddlResponsType.SelectedValue.ToString() = 2 Or Me.ddlResponsType.SelectedValue.ToString() = 3 Then
                    Me.ddlOptCount.SelectedValue = Dt.Rows(0)("OptCnt").ToString()
                    Dim Optcnt As Integer = 0

                    For Each dr1 As DataRow In Dt.Rows
                        Dim dr As DataRow = dtResponse.NewRow
                        dr(0) = Dt.Rows.IndexOf(dr1) + 1
                        dr(1) = IIf(dr1("Response_Text") Is DBNull.Value Or dr1("Response_text").ToString() = "0", "", dr1("Response_text").ToString())
                        dr(2) = IIf(dr1("Defvalue") Is DBNull.Value Or dr1("Defvalue").ToString() = "0", False, True)
                        dtResponse.Rows.Add(dr)

                    Next
                    Me.gvResponse.DataSource = dtResponse
                    Me.gvResponse.DataBind()
                    Me.gvResponse.Visible = True
                    Me.ddlOptCount.Visible = True
                    Me.lblOptCnt.Visible = True
                Else
                    Me.ddlOptCount.Visible = False
                    Me.lblOptCnt.Visible = False
                    Me.gvResponse.Visible = False
                End If
                Me.btnModify.Enabled = True
                Me.btnDelete.Enabled = True
                Me.btnCancel.Visible = True
                Me.btnAdd.Visible = False
                Me.ddlOptCount.Enabled = False
                Me.gvResponse.Enabled = False
                ToggleEnabled(False)
                ' Me.ddSurvey.Enabled = False
            Else
                ResetFields()
                ToggleEnabled(False)
            End If
        Else
            ResetFields()
            ToggleEnabled(False)
        End If
    End Sub

    Protected Sub txtOptValue_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        ''Me.gvResponse.Focus()
        Dim txtOpt As TextBox = TryCast(sender, TextBox)
        Dim row As GridViewRow = TryCast(txtOpt.NamingContainer, GridViewRow)
        Dim chk As CheckBox = TryCast(row.FindControl("ChkDefault"), CheckBox)
        If txtOpt.Text <> "" And txtOpt.Text <> "0" Then
            dtResponse.Rows(row.RowIndex)(1) = txtOpt.Text
            dtResponse.Rows(row.RowIndex)(2) = IIf(chk.Checked = True And txtOpt.Text <> "", True, False)
            '    ''For i As Integer = 0 To dtResponse.Rows.Count - 1
            '    ''    If i <> row.RowIndex And CBool(dtResponse.Rows(i)(2)) = True Then
            '    ''        dtResponse.Rows(i)(2) = False
            '    ''    End If
            '    ''Next
        Else
            txtOpt.Text = ""
            chk.Checked = False
            dtResponse.Rows(row.RowIndex)(1) = txtOpt.Text
            dtResponse.Rows(row.RowIndex)(2) = False
            '    '  Me.Focus()
        End If
        dtResponse.AcceptChanges()
        ' ''For Each r As GridViewRow In Me.gvResponse.Rows
        ' ''    If (r.RowType = DataControlRowType.DataRow) Then
        ' ''        Dim txt As TextBox = TryCast(r.FindControl("txtOptValue"), TextBox)
        ' ''        Dim ch As CheckBox = TryCast(r.FindControl("ChkDefault"), CheckBox)
        ' ''        If txt.Text = "" Then
        ' ''            dtResponse.Rows(r.RowIndex)(1) = ""
        ' ''            dtResponse.Rows(r.RowIndex)(2) = False
        ' ''        End If
        ' ''    End If
        ' ''Next
        'dtResponse.AcceptChanges()

        'chk.Focus()
        Me.gvResponse.DataSource = dtResponse
        Me.gvResponse.DataBind()

        If row.RowIndex = gvResponse.Rows.Count - 1 Then
            Dim gvr As GridViewRow = Me.gvResponse.Rows(0)
            Dim Ch As CheckBox = TryCast(gvr.FindControl("ChkDefault"), CheckBox)
            Ch.Focus()
        End If

        '  UpdateGrid(row.RowIndex)
        'Dim gvr As GridViewRow = Me.gvResponse.Rows(row.RowIndex)
        'Dim ch As CheckBox = TryCast(gvr.FindControl("ChkDefault"), CheckBox)
        'chk.Focus()
        Dim id As Integer = row.RowIndex + 1
        If id <= gvResponse.Rows.Count - 1 Then
            Dim gvr As GridViewRow = Me.gvResponse.Rows(id)
            Dim txt As TextBox = TryCast(gvr.FindControl("txtOptValue"), TextBox)
            txt.Focus()
        End If

    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        str = str.Replace("'", "&#39;")
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
End Class