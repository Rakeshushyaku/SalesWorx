Imports SalesWorx.BO.Common
Imports System.Data
Imports System.Web
Imports System.Resources
Imports System.Web.UI.WebControls
Imports log4net
Imports System.IO
Imports System.Data.SqlClient
Imports Telerik.Web.UI

Partial Public Class AdminSurveyQues
    Inherits System.Web.UI.Page
    Private objSurvey As New Survey
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim Dt As New DataTable
    Dim frmStatus As String = ""
    Private Const PageID As String = "P13"
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
            ResetFields()
            ToggleEnabled(False)
        End If
    End Sub

    Private Sub LoadSurvey()
        ddSurvey.DataSource = objSurvey.LoadSurveys(Err_No, Err_Desc)
        ddSurvey.DataTextField = "Survey_title"
        ddSurvey.DataValueField = "Survey_Id"
        ddSurvey.DataBind()
        ddSurvey.SelectedIndex = 0
    End Sub


    Private Sub ResetFields()
        ' Me.ddSurvey.SelectedIndex = 0
        Me.ddlQuestions.Visible = True
        Me.lblQuestion1.Visible = True
        Me.ddlResponses.Visible = True
        Me.lblResponse.Visible = True
        Me.txtQuestion.Visible = False
        Me.lblQuestion.Visible = False
        divQues.Visible = False
        Me.btnAdd.Visible = True
        Me.btnModify.Visible = True
        Me.btnDelete.Visible = True
        Me.btnCancel.Visible = False
        Me.btnSave.Visible = False
        Me.btnSave.Text = "Save"
        Me.btnModify.Enabled = False
        Me.btnDelete.Enabled = False
        Me.ddlQuestions.Items.Clear()
        Me.ddlResponses.Items.Clear()
        ddlQuestions.AppendDataBoundItems = True
        ddlQuestions.Items.Insert(0, New RadComboBoxItem("--Select a Question--"))
        '' ''ddlQuestions.Items(0).Value = ""
        ddlResponses.AppendDataBoundItems = True
        ddlResponses.Items.Insert(0, New RadComboBoxItem("--Select a Response--", ""))
        '' ''ddlResponses.Items.Insert(0, "--Select a Response--")
        '' ''ddlResponses.Items(0).Value = ""
        Me.txtQuestion.Text = ""
        frmStatus = ""
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
        If Me.ddSurvey.SelectedIndex > 0 Then
            objSurvey.SurveyID = Me.ddSurvey.SelectedValue.ToString()
            If objSurvey.GetSurveyDate(Err_No, Err_Desc) <= Now.Date Then
                'Me.lblinfo.Text = "Validation"
                'Me.lblMessage.Text = "Survey start date should be greater than today's date for add question."
                'Me.lblMessage.ForeColor = Drawing.Color.Red
                'Me.MpInfoError.Show()
                MessageBoxValidation("Survey start date should be greater than today's date for add question.", "Validation")
                Exit Sub
            End If
        End If
        ResetFields()
        ToggleEnabled(False)
        frmStatus = "NEW"
        Me.ddlQuestions.Visible = False
        Me.lblQuestion1.Visible = False
        Me.txtQuestion.Visible = True
        Me.lblQuestion.Visible = True
        divQues.Visible = True
        Me.ddlResponses.Visible = False
        Me.lblResponse.Visible = False
        Me.btnAdd.Visible = False
        Me.btnModify.Visible = False
        Me.btnDelete.Visible = False
        Me.btnCancel.Visible = True
        Me.btnSave.Visible = True
        Me.btnSave.Text = "Save"
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        ResetFields()
        Me.ddSurvey.SelectedIndex = 0
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        If Me.ddSurvey.SelectedIndex > 0 Then
            objSurvey.SurveyID = Me.ddSurvey.SelectedValue.ToString()
            If objSurvey.GetSurveyDate(Err_No, Err_Desc) <= Now.Date Then
                'Me.lblinfo.Text = "Validation"
                'Me.lblMessage.Text = "Survey start date should be greater than today's date for add question."
                'Me.lblMessage.ForeColor = Drawing.Color.Red
                'Me.MpInfoError.Show()
                MessageBoxValidation("Survey start date should be greater than today's date for add question.", "Validation")
                Exit Sub
            End If
        End If
        If Me.txtQuestion.Text = "" Or Me.ddSurvey.SelectedIndex <= 0 Then
            'Me.lblinfo.Text = "Validation"
            'Me.lblMessage.Text = "Survey and questions are required."
            'Me.lblMessage.ForeColor = Drawing.Color.Red
            'Me.MpInfoError.Show()
            MessageBoxValidation("Survey and questions are required.", "Validation")
            Exit Sub
        End If

        Dim success As Boolean = False
        Try

            objSurvey.Question = IIf(Me.txtQuestion.Text = "", "0", Me.txtQuestion.Text)
            objSurvey.SurveyID = Me.ddSurvey.SelectedValue.ToString()
            objSurvey.ResponseID = Integer.Parse(IIf(Me.ddlResponses.SelectedIndex <= 0, "0", Me.ddlResponses.SelectedValue.ToString()))


            If Me.btnSave.Text = "Save" Then
                If objSurvey.AddSurveyQuestions(Err_No, Err_Desc) = True Then
                    objLogin.SaveUserLog(Err_No, Err_Desc, "I", "SURVEY", "SURVEY QUESTIONS", Me.ddSurvey.SelectedValue.ToString(), "Survey Title: " & Me.ddSurvey.SelectedItem.Text & "/ Question :  " & IIf(Me.txtQuestion.Text = "", "0", Me.txtQuestion.Text) & "/ Default Response :  " & IIf(Me.ddlResponses.SelectedIndex > 0, Me.ddlResponses.SelectedItem.Text, ""), CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                    success = True
                    'Me.lblMessage.Text = "Successfully saved."
                    'lblMessage.ForeColor = Drawing.Color.Green
                    'lblinfo.Text = "Information"
                    'MpInfoError.Show()
                    'btnClose.Focus()
                    MessageBoxValidation("Successfully saved.", "Information")
                End If
            ElseIf Me.btnSave.Text = "Update" Then
                If objSurvey.ModifyQuestions(Err_No, Err_Desc, Me.ddlQuestions.SelectedValue.ToString()) = True Then
                    objLogin.SaveUserLog(Err_No, Err_Desc, "U", "SURVEY", "SURVEY QUESTIONS", Me.ddSurvey.SelectedValue.ToString(), "Survey Title: " & Me.ddSurvey.SelectedItem.Text & "/ Question :  " & IIf(Me.txtQuestion.Text = "", "0", Me.txtQuestion.Text) & "/ Default Response :  " & IIf(Me.ddlResponses.SelectedIndex > 0, Me.ddlResponses.SelectedItem.Text, ""), CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                    success = True
                    'Me.lblMessage.Text = "Successfully updated."
                    'lblMessage.ForeColor = Drawing.Color.Green
                    'lblinfo.Text = "Information"
                    'MpInfoError.Show()
                    'btnClose.Focus()
                    MessageBoxValidation("Successfully updated.", "Information")
                End If
            End If
            If success = True Then
                ResetFields()
                Me.ddSurvey.SelectedIndex = 0
                ToggleEnabled(False)
            Else

                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_AdminSurveyQues_001") & "&next=AdminSurveyQues.aspx&Title=Message", False)
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
        If Me.ddSurvey.SelectedIndex > 0 And frmStatus = "" Then
            objSurvey.SurveyID = Me.ddSurvey.SelectedValue.ToString()
            ddlResponses.Items.Clear()
            ddlResponses.AppendDataBoundItems = True
            ddlResponses.Items.Insert(0, New RadComboBoxItem("--Select a Response--", ""))
            '' '' ddlResponses.Items(0).Value = ""
            ddlQuestions.Items.Clear()
            ddlQuestions.DataSource = objSurvey.LoadQuestions(Err_No, Err_Desc)
            ddlQuestions.DataTextField = "Question_Text"
            ddlQuestions.DataValueField = "Question_Id"
            ddlQuestions.DataBind()
            ddlQuestions.SelectedIndex = 0
            ' ToggleEnabled(False)
        Else
            ResetFields()
        End If
    End Sub

    Protected Sub btnModify_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModify.Click
        If Me.ddSurvey.SelectedIndex > 0 Then
            objSurvey.SurveyID = Me.ddSurvey.SelectedValue.ToString()
            If objSurvey.GetSurveyDate(Err_No, Err_Desc) <= Now.Date Then
                ''Me.lblinfo.Text = "Validation"
                'Me.lblMessage.Text = "Survey start date should be greater than today's date for modify question."
                'Me.lblMessage.ForeColor = Drawing.Color.Red
                'Me.MpInfoError.Show()
                MessageBoxValidation("Survey start date should be greater than today's date for modify question.", "Validation")
                Exit Sub
            End If
        End If
        frmStatus = "EDIT"
        If Me.ddSurvey.SelectedIndex > 0 And Me.ddlQuestions.SelectedIndex > 0 Then
            ToggleEnabled(True)
            Me.ddlQuestions.Visible = False
            Me.lblQuestion1.Visible = False
            Me.txtQuestion.Visible = True
            Me.lblQuestion.Visible = True
            divQues.Visible = True
            Me.ddlResponses.Visible = True
            Me.lblResponse.Visible = True
            Me.btnAdd.Visible = False
            Me.btnModify.Visible = False
            Me.btnDelete.Visible = False
            Me.btnCancel.Visible = True
            Me.btnSave.Visible = True
            Me.btnSave.Text = "Update"
        End If
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
        If Me.ddSurvey.SelectedIndex > 0 Then
            objSurvey.SurveyID = Me.ddSurvey.SelectedValue.ToString()
            If objSurvey.GetSurveyDate(Err_No, Err_Desc) <= Now.Date Then
                'Me.lblinfo.Text = "Validation"
                'Me.lblMessage.Text = "Survey start date should be greater than today's date for delete question."
                'Me.lblMessage.ForeColor = Drawing.Color.Red
                'Me.MpInfoError.Show()
                MessageBoxValidation("Survey start date should be greater than today's date for delete question.", "Validation")
                Exit Sub
            End If
        End If
        Dim success As Boolean = False
        If Me.ddSurvey.SelectedIndex > 0 And Me.ddlQuestions.SelectedIndex > 0 Then
            If objSurvey.RemoveSurveyQuestions(Err_No, Err_Desc, Me.ddlQuestions.SelectedValue.ToString()) = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "D", "SURVEY", "SURVEY QUESTIONS", Me.ddSurvey.SelectedValue.ToString(), "Survey Title: " & Me.ddSurvey.SelectedItem.Text & "/ Question :  " & IIf(Me.txtQuestion.Text = "", "0", Me.txtQuestion.Text) & "/ Default Response :  " & IIf(Me.ddlResponses.SelectedIndex > 0, Me.ddlResponses.SelectedItem.Text, ""), CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                success = True
                'Me.lblMessage.Text = "Successfully deleted."
                'lblMessage.ForeColor = Drawing.Color.Green
                'lblinfo.Text = "Information"
                'MpInfoError.Show()
                'btnClose.Focus()
                MessageBoxValidation("Successfully deleted.", "Information")
            End If
        End If
        If success = True Then
            ResetFields()
            Me.ddSurvey.SelectedIndex = 0
            ToggleEnabled(False)

        Else

            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_AdminSurvey_001") & "&next=AdminSurvey.aspx&Title=Message", False)
        End If
    End Sub

    Private Sub ToggleEnabled(ByVal value As Boolean)
        Me.ddlResponses.Enabled = value
        'Me.ddlQuestions.Enabled = value
    End Sub

    Protected Sub ddlQuestions_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlQuestions.SelectedIndexChanged
        If Me.ddlQuestions.SelectedIndex > 0 And Me.ddSurvey.SelectedIndex > 0 And frmStatus = "" Then
            ddlResponses.Items.Clear()
            ddlResponses.DataSource = objSurvey.LoadResponses(Err_No, Err_Desc, Me.ddlQuestions.SelectedValue.ToString())
            ddlResponses.DataTextField = "Response_Text"
            ddlResponses.DataValueField = "Response_Id"
            ddlResponses.DataBind()
            ddlResponses.SelectedIndex = 0
            objSurvey.SurveyID = IIf(Me.ddSurvey.SelectedIndex > 0, Me.ddSurvey.SelectedValue.ToString(), "0")
            Dt = objSurvey.EditSurveysQuestions(Err_No, Err_Desc, IIf(Me.ddlQuestions.SelectedIndex > 0, Me.ddlQuestions.SelectedValue.ToString(), "0"), IIf(Me.ddSurvey.SelectedIndex > 0, Me.ddSurvey.SelectedValue.ToString(), "0"))
            If Dt.Rows.Count > 0 Then
                Me.txtQuestion.Text = Dt.Rows(0)("Question_Text").ToString()
                If Dt.Rows(0)("Default_Response_Id") Is DBNull.Value Or Dt.Rows(0)("Default_Response_Id").ToString() = "0" Then
                    Me.ddlResponses.SelectedItem.Text = "--Select a Response--"
                Else
                    Me.ddlResponses.SelectedValue = Dt.Rows(0)("Default_Response_Id").ToString()
                End If
                Me.btnModify.Enabled = True
                Me.btnDelete.Enabled = True
                ToggleEnabled(False)
            End If
        Else
            Me.ddlResponses.Items.Clear()
            ddlResponses.AppendDataBoundItems = True
            ddlResponses.Items.Insert(0, New RadComboBoxItem("--Select a Response--", ""))
            '' ''ddlResponses.Items.Insert(0, "--Select a Response--")
            '' ''ddlResponses.Items(0).Value = ""
            ToggleEnabled(False)
        End If
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        str = str.Replace("'", "&#39;")
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
End Class