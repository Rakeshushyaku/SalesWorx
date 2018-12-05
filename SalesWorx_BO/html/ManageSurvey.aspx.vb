Imports log4net
Imports SalesWorx.BO.Common
Imports System.IO
Imports Telerik.Web
Imports Telerik.Web.UI
Imports System.Data.OleDb
Imports System.Text.RegularExpressions
Imports Telerik
Public Class ManageSurvey
    Inherits System.Web.UI.Page

    Private Const ModuleName As String = "ManageSurvey.aspx"
    Private Const PageID As String = "P12"
    Private dtAvail As New DataTable
    Private dtAssigned As New DataTable
    Private dtResponses As New DataTable
    Private dtQuestions As New DataTable
    Private objSurvey As New Survey
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub txtLocName_TextChanged(sender As Object, e As EventArgs) Handles txtTitle.TextChanged
        Me.lblSurveyName.Text = Me.txtTitle.Text
        If Me.txtTitle.Text <> "" Then
            Me.lblMsg.Text = ""
        End If
    End Sub
    Private Sub LoadSurveyType()
        Dim k As New DataTable
        k = objSurvey.GetSurveyType(Err_No, Err_Desc)
        ddlTypeCode.DataSource = k
        ddlTypeCode.DataTextField = "Key"
        ddlTypeCode.DataValueField = "Value"
        ddlTypeCode.DataBind()
        Me.ddlTypeCode.SelectedIndex = 0
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Not Session.Item("USER_ACCESS") Is Nothing Then

                If Not HasAuthentication() Then
                    Err_No = 500
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
                End If


            Else
                Session.Add("BringmeBackHere", ModuleName)
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            ResetFields()
            LoadResponseType()
            LoadSurveyType()
            Session.Remove("dtSurveyAvail")
            Session.Remove("dtSurveyAssigned")
            Session.Remove("dtQuestions")
            Session.Remove("dtResponses")
            Session.Remove("dtQResponses")
            SetAssignedTable()
            SetAvailTable()
            SetQuestionsTable()
            SetResponseTable()
            '  FillAvailableFSR()
            ' FillAssignedFSR()

        Else
            dtAvail = Session("dtSurveyAvail")
            dtAssigned = Session("dtSurveyAssigned")
            dtQuestions = Session("dtQuestions")
            dtResponses = Session("dtResponses")

            Me.DocWindow.VisibleOnPageLoad = False
        End If
    End Sub
    Private Sub SetAssignedTable()
        Dim col As DataColumn


        col = New DataColumn()
        col.ColumnName = "SalesRep_ID"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtAssigned.Columns.Add(col)


        col = New DataColumn()
        col.ColumnName = "SalesRep_Name"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtAssigned.Columns.Add(col)

        Session.Remove("dtSurveyAssigned")
        Session("dtSurveyAssigned") = dtAssigned
    End Sub
    Sub FillAvailableFSR()
        Dim t As New DataTable
        t = objSurvey.GetAvailFSRBySurveyID(Err_No, Err_Desc, CInt(IIf(Me.lblSurveyID.Text = "", "0", Me.lblSurveyID.Text)))

        If t.Rows.Count > 0 Then
            t.Rows.RemoveAt(0)
        End If

        If dtAvail.Rows.Count > 0 Then
            dtAvail.Rows.Clear()
        End If

        For Each r As DataRow In t.Rows
            Dim Orow As DataRow = dtAvail.NewRow
            Orow("SalesRep_ID") = r("SalesRep_ID").ToString()
            Orow("SalesRep_Name") = r("SalesRep_Name").ToString()
            dtAvail.Rows.Add(Orow)
        Next

        '  Dim dv1 As New DataView
        '  dv1 = dtAssigned.DefaultView


        rlvAvaile.Items.Clear()
        rlvAvaile.DataTextField = "SalesRep_Name"
        rlvAvaile.DataValueField = "SalesRep_ID"
        rlvAvaile.DataSource = dtAvail
        rlvAvaile.DataBind()

        Session.Remove("dtSurveyAvail")
        Session("dtSurveyAvail") = dtAvail

    End Sub

    Sub FillAssignedFSR()
        Dim t As New DataTable
        t = objSurvey.GetAssignedFSRBySurveyID(Err_No, Err_Desc, CInt(IIf(Me.lblSurveyID.Text = "", "0", Me.lblSurveyID.Text)))

        If t.Rows.Count > 0 Then
            t.Rows.RemoveAt(0)
        End If

        If dtAssigned.Rows.Count > 0 Then
            dtAssigned.Rows.Clear()
        End If

        For Each r As DataRow In t.Rows
            Dim Orow As DataRow = dtAssigned.NewRow
            Orow("SalesRep_ID") = r("SalesRep_ID").ToString()
            Orow("SalesRep_Name") = r("SalesRep_Name").ToString()
            dtAssigned.Rows.Add(Orow)
        Next

        '  Dim dv1 As New DataView
        '  dv1 = dtAssigned.DefaultView


        rlvAssigne.Items.Clear()
        rlvAssigne.DataTextField = "SalesRep_Name"
        rlvAssigne.DataValueField = "SalesRep_ID"
        rlvAssigne.DataSource = dtAssigned
        rlvAssigne.DataBind()

        Session.Remove("dtSurveyAssigned")
        Session("dtSurveyAssigned") = dtAssigned

    End Sub
    Private Sub LoadResponseType()
        ddlResponsType.Items.Clear()
        ddlResponsType.ClearSelection()
        ddlResponsType.Text = ""

        ddlResponsType.DataSource = objSurvey.LoadResponseTypes(Err_No, Err_Desc)
        ddlResponsType.DataTextField = "Response_Type"
        ddlResponsType.DataValueField = "Response_Type_Id"
        ddlResponsType.DataBind()
        ddlResponsType.SelectedIndex = 0
    End Sub
    Private Sub SetAvailTable()
        Dim col As DataColumn


        col = New DataColumn()
        col.ColumnName = "SalesRep_ID"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtAvail.Columns.Add(col)


        col = New DataColumn()
        col.ColumnName = "SalesRep_Name"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtAvail.Columns.Add(col)

        Session.Remove("dtSurveyAvail")
        Session("dtSurveyAvail") = dtAvail
    End Sub
    Private Sub ResetFields()
        Me.ddlTypeCode.SelectedIndex = 0
        Me.txtTitle.Text = ""
        Me.txtStartDate.SelectedDate = Now.Date.AddDays(1)
        Me.txtExpDate.SelectedDate = Now.Date.AddMonths(1)
        Me.txtStartDate.Enabled = True
        Me.txtExpDate.Enabled = True
        Me.txtFilter.Text = ""
        Me.lblSurveyID.Text = "0"
        Me.lblSurveyStatus.Text = "New"
        If Me.dtAvail.Rows.Count > 0 Then

            dtAvail.Rows.Clear()

            Session.Remove("dtSurveyAvail")
            Session("dtSurveyAvail") = dtAvail

            rlvAvaile.Items.Clear()
            rlvAvaile.DataSource = dtAvail

        End If
        If Me.dtAssigned.Rows.Count > 0 Then

            dtAssigned.Rows.Clear()

            Session.Remove("dtSurveyAssigned")
            Session("dtSurveyAssigned") = dtAssigned

            rlvAssigne.Items.Clear()
            rlvAssigne.DataSource = dtAssigned

        End If

        If Me.dtQuestions.Rows.Count > 0 Then

            dtQuestions.Rows.Clear()

            Session.Remove("dtQuestions")
            Session("dtQuestions") = dtQuestions

            Me.rgQuestions.DataSource = dtQuestions
            Me.rgQuestions.DataBind()

        End If

        If Me.dtResponses.Rows.Count > 0 Then

            dtResponses.Rows.Clear()

            Session.Remove("dtResponses")
            Session("dtResponses") = dtResponses


        End If
        Me.btnAddQuest.Text = "Save Question & Response"
        Me.hfQuestionID.Value = ""
        Me.lblSurveyName.Text = ""
        ddlResponsType.ClearSelection()
        ddlResponsType.Text = ""
        Me.ddlResponsType.SelectedIndex = 0
        Me.txtQuestion.Text = ""
        Me.ddlResponsType.SelectedIndex = 0
        Me.txtResponse.Text = ""
        Me.gvResponseList.DataSource = Nothing
        gvResponseList.DataBind()
        Me.rsDefault.Checked = False
        divRespList.Visible = False
        divResponse.Visible = False
        divIsDefault.Visible = False
    End Sub
    Private Function HasAuthentication() As Boolean
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
    End Function
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try



            lbl_imgName.Text = ""
            file_Imag.Visible = False
            Dim btn1 As ImageButton = TryCast(sender, ImageButton)
            Dim item As GridDataItem = DirectCast(btn1.NamingContainer, GridDataItem)
            Dim SurveyID As Label = DirectCast(item.FindControl("lblSurveyDelID"), Label)
            Dim StatusCode As Label = DirectCast(item.FindControl("lblStatCode"), Label)
            Me.lblSurveyID.Text = SurveyID.Text

            Me.lblVMsg.Text = ""
            Me.lblMsg.Text = ""
            Me.lblMsg1.Text = ""

            If StatusCode.Text = "D" Then
                Me.btnSave.Visible = False
                Me.btnCancel.Visible = False
                Me.txtQuestion.Enabled = False
                Me.ddlResponsType.Enabled = False
                Me.btnAddQuest.Visible = False

                Me.txtTitle.Enabled = False
                Me.txtStartDate.Enabled = False
                Me.txtExpDate.Enabled = False
                Me.ddlTypeCode.Enabled = False
                divRespList.Visible = False
                divResponse.Visible = False
                divIsDefault.Visible = False
                btnSearch.Enabled = False
                Me.btnClear.Visible = False
                btnAddQuest.Visible = False
                Me.btnCancel.Visible = False
                btnReset.Enabled = False
                Me.rlvAssigne.Enabled = False
                Me.rlvAvaile.Enabled = False
                Me.txtFilter.Enabled = False
                chk_mandatoryOnConf.Visible = False
                VanAudit.Visible = False
            ElseIf StatusCode.Text = "A" Then
                Me.btnSave.Visible = True
                Me.btnCancel.Visible = True
                Me.txtQuestion.Enabled = False
                Me.ddlResponsType.Enabled = False
                Me.btnAddQuest.Visible = False
                Me.btnClear.Visible = False
                Me.txtTitle.Enabled = False
                Me.txtStartDate.Enabled = False
                Me.txtExpDate.Enabled = True
                Me.ddlTypeCode.Enabled = False
                divRespList.Visible = False
                divResponse.Visible = False
                divIsDefault.Visible = False
                btnSearch.Enabled = True
                Me.btnCancel.Visible = True
                Me.btnReset.Enabled = True
                Me.txtFilter.Enabled = True
                Me.rlvAssigne.Enabled = True
                Me.rlvAvaile.Enabled = True
                chk_mandatoryOnConf.Visible = True

               


            ElseIf StatusCode.Text = "N" Then

                Me.btnSave.Visible = True
                Me.btnCancel.Visible = True
                Me.txtQuestion.Enabled = True
                Me.ddlResponsType.Enabled = True
                Me.btnAddQuest.Visible = True
                Me.txtTitle.Enabled = True
                Me.txtStartDate.Enabled = True
                Me.txtExpDate.Enabled = True
                Me.ddlTypeCode.Enabled = True
                btnSearch.Enabled = True
                Me.btnClear.Visible = True
                Me.btnCancel.Visible = True
                Me.btnReset.Enabled = True
                Me.txtFilter.Enabled = True
                Me.rlvAssigne.Enabled = True
                Me.rlvAvaile.Enabled = True
                chk_mandatoryOnConf.Visible = False
                VanAudit.Visible = False
            End If

         











            If StatusCode.Text = "N" Then
                Me.lblSurveyStatus.Text = "New"
            ElseIf StatusCode.Text = "A" Then
                Me.lblSurveyStatus.Text = "Active"
            ElseIf StatusCode.Text = "D" Then
                Me.lblSurveyStatus.Text = "Deactivated"
            End If

            Dim x As New DataTable
            x = objSurvey.GetSurveyDetailsByID(Err_No, Err_Desc, lblSurveyID.Text)


            If x.Rows.Count > 0 Then
                Me.txtTitle.Text = x.Rows(0)("Survey_Title").ToString()
                Me.lblSurveyName.Text = Me.txtTitle.Text
                Me.ddlTypeCode.SelectedValue = x.Rows(0)("Survey_Type_Code").ToString()
                Me.txtStartDate.SelectedDate = x.Rows(0)("Start_Time").ToString()
                Me.txtExpDate.SelectedDate = x.Rows(0)("End_Time").ToString()
            End If

            FillAssignedFSR()
            FillAvailableFSR()

            Dim y As New DataTable
            y = objSurvey.GetSurveyQuestionsBySurveyID(Err_No, Err_Desc, lblSurveyID.Text)

            If dtQuestions.Rows.Count > 0 Then
                dtQuestions.Rows.Clear()
            End If
            Session.Remove("dtQuestions")
            Session("dtQuestions") = dtQuestions
            dtQuestions = Session("dtQuestions")


            For Each r As DataRow In y.Rows
                Dim orow As DataRow = dtQuestions.NewRow()
                orow("Question_Id") = r("Question_ID").ToString()
                orow("Question_Text") = r("Question_Text").ToString()
                orow("Default_Response_ID") = r("Default_Response_ID").ToString()
                orow("Response_Type_ID") = r("Response_Type_ID").ToString()
                orow("Response_Type") = r("Response_Type").ToString()
                orow("ResponsesList") = r("ResponsesList").ToString()
                orow("IsMandatory") = IIf(r("Is_Mandatory").ToString() = "Y", "1", "0")
                orow("Group_Text") = r("Group_Text").ToString()
                orow("HasImage") = IIf(r("Has_Image").ToString() = "Y", "1", "0")
                orow("ImageName") = r("Image_File").ToString()
                orow("IsDeleted") = "N"
                orow("Mandatory_On_Confirmation") = r("Mandatory_On_Confirmation").ToString()
                orow("Remarks_Required") = r("Remarks_Required").ToString()
                orow("No_Of_Lines_For_Text") = r("No_Of_Lines_For_Text")
                orow("No_Of_Lines_For_Remarks") = r("No_Of_Lines_For_Remarks").ToString()
                orow("Default_Star_Rating") = r("Default_Star_Rating").ToString()
                orow("Sequence") = r("Sequence").ToString()
                dtQuestions.Rows.Add(orow)
            Next
            Session.Remove("dtQuestions")
            Session("dtQuestions") = dtQuestions
            Me.rgQuestions.DataSource = dtQuestions
            Me.rgQuestions.DataBind()

            If StatusCode.Text = "A" Or StatusCode.Text = "D" Then
                rgQuestions.Columns(0).Visible = False
                rgQuestions.Columns(1).Visible = False
            Else
                rgQuestions.Columns(0).Visible = True
                rgQuestions.Columns(1).Visible = True
            End If

            If Me.dtResponses.Rows.Count > 0 Then

                dtResponses.Rows.Clear()
            End If

            Session.Remove("dtResponses")
            Session("dtResponses") = dtResponses


            dtResponses = Session("dtResponses")
            Dim z As New DataTable
            z = objSurvey.GetSurveyResponsesBySurveyID(Err_No, Err_Desc, lblSurveyID.Text)

            For Each r As DataRow In z.Rows
                Dim hRow As DataRow = dtResponses.NewRow()

                hRow("Question_Id") = r("Question_ID").ToString()
                hRow("Question_Text") = r("Question_Text").ToString()
                hRow("Response_Id") = r("Response_ID").ToString()
                hRow("Response_Text") = r("Response_Text").ToString()
                hRow("Response_Type_ID") = r("Response_Type_ID").ToString()
                hRow("Response_Type") = r("Response_Type").ToString()
                hRow("DefValue") = IIf(r("DefValue").ToString() = "Y", "1", "0")
                hRow("ShowCommentBox") = IIf(r("Show_Comment_Box").ToString() = "Y", "1", "0")
                hRow("IsDeleted") = "N"

                dtResponses.Rows.Add(hRow)
            Next


            Session.Remove("dtResponses")
            Session("dtResponses") = dtResponses
            RadTabStrip1.SelectedIndex = 0
            RadTabStrip1.SelectedTab.TabIndex = 0

            RadTabStrip1.SelectedTab.Selected = True
            RadMultiPage1.SelectedIndex = 0

            ShowQuestionImage()

            txt_groupTxt.Text = ""
            chk_mandatory.Checked = False
            chk_Image.Checked = False
            Me.DocWindow.VisibleOnPageLoad = True



            If ddlTypeCode.SelectedItem.Value = "S" Then
                divQuestionImage.Visible = True
                divMandatory.Visible = True
                chk_showComment.Visible = True
                chk_mandatoryOnConf.Visible = False
                VanAudit.Visible = False
            ElseIf ddlTypeCode.SelectedItem.Value = "A" Then
                divQuestionImage.Visible = False
                chk_showComment.Visible = True
                divMandatory.Visible = True
                chk_mandatoryOnConf.Visible = True
                VanAudit.Visible = True
            Else
                divQuestionImage.Visible = False
                chk_showComment.Visible = False
                divMandatory.Visible = False
                chk_mandatoryOnConf.Visible = False
                VanAudit.Visible = False
            End If
        Catch ex As Exception
            Err_No = "20421"
            log.Error(Err_Desc)
            log.Error(ex.ToString)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("PHX_006") & "&next=ManageSurvey.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Protected Sub lbActive_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try





            Dim btn1 As LinkButton = TryCast(sender, LinkButton)
            Dim item As GridDataItem = DirectCast(btn1.NamingContainer, GridDataItem)
            Dim SurveyID As Label = DirectCast(item.FindControl("lblSurveyDelID"), Label)
            Dim StatusCode As Label = DirectCast(item.FindControl("lblStatCode"), Label)

            Dim oldStatus As String = StatusCode.Text
            Dim NewStatus As String = ""

            If StatusCode.Text = "N" Then
                NewStatus = "A"
            ElseIf StatusCode.Text = "A" Then
                NewStatus = "D"
            End If

            If objSurvey.UpdateSurveyStatus(Err_No, Err_Desc, SurveyID.Text, oldStatus, NewStatus) Then
                MessageBoxValidation("Successfully survey status updated", "Information")
                Me.rgSurvey.Rebind()
            Else
                MessageBoxValidation("Error while updating survey status", "Information")
                Exit Sub
            End If



        Catch ex As Exception
            Err_No = "20421"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("PHX_006") & "&next=ManageSurvey.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub

    Protected Sub rgSurvey_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles rgSurvey.ItemCommand

        Try
            If e.CommandName = "DeleteSelected" Then
                Dim item As GridDataItem = DirectCast(e.Item, GridDataItem)
                Dim SurveyID As Label = DirectCast(item.FindControl("lblSurveyDelID"), Label)
                Dim StatusCode As Label = DirectCast(item.FindControl("lblStatCode"), Label)

                If StatusCode.Text <> "N" Then
                    MessageBoxValidation("You can delete a survey only if the status is New", "Information")
                    Exit Sub
                End If

                If objSurvey.RemoveSurvey(Err_No, Err_Desc, SurveyID.Text) Then
                    MessageBoxValidation("Successfully deleted", "Information")
                    rgSurvey.Rebind()
                Else
                    MessageBoxValidation("Error while deleting", "Validation")
                    Exit Sub
                End If
            End If


        Catch ex As Exception
            Err_No = "64224"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("PHX_006") & "&next=ManageSurvey.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub


    Protected Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        Me.txtFilter.Text = ""




        dtAvail = Session("dtSurveyAvail")


        dtAssigned = Session("dtSurveyAssigned")



        dtAvail.DefaultView.RowFilter = "(1=1)"
        dtAssigned.DefaultView.RowFilter = "(1=1)"


        rlvAvaile.Items.Clear()
        rlvAvaile.DataTextField = "SalesRep_Name"
        rlvAvaile.DataValueField = "SalesRep_ID"
        rlvAvaile.DataSource = dtAvail
        rlvAvaile.DataBind()

        Session.Remove("dtSurveyAvail")
        Session("dtSurveyAvail") = dtAvail






        rlvAssigne.Items.Clear()
        rlvAssigne.DataTextField = "SalesRep_Name"
        rlvAssigne.DataValueField = "SalesRep_ID"
        rlvAssigne.DataSource = dtAssigned.DefaultView
        rlvAssigne.DataBind()



        Session.Remove("dtSurveyAssigned")
        Session("dtSurveyAssigned") = dtAssigned


    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

        dtAvail = Session("dtSurveyAvail")


        dtAssigned = Session("dtSurveyAssigned")


        If Me.txtFilter.Text <> "" Then
            dtAvail.DefaultView.RowFilter = "(SalesRep_Name LIKE '%" & Me.txtFilter.Text & "%')"
            dtAssigned.DefaultView.RowFilter = "(SalesRep_Name LIKE '%" & Me.txtFilter.Text & "%')"
        End If

        '  Dim dv As New DataView




        rlvAssigne.Items.Clear()
        rlvAssigne.DataTextField = "SalesRep_Name"
        rlvAssigne.DataValueField = "SalesRep_ID"
        rlvAssigne.DataSource = dtAssigned
        rlvAssigne.DataBind()






        rlvAvaile.Items.Clear()
        rlvAvaile.DataTextField = "SalesRep_Name"
        rlvAvaile.DataValueField = "SalesRep_ID"
        rlvAvaile.DataSource = dtAvail
        rlvAvaile.DataBind()

        Session.Remove("dtSurveyAvail")
        Session("dtSurveyAvail") = dtAvail

        Session.Remove("dtSurveyAssigned")
        Session("dtSurveyAssigned") = dtAssigned


    End Sub
    
    Protected Sub imgMoveLeft_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try



            Dim btn1 As ImageButton = TryCast(sender, ImageButton)
            Dim item As RadListBoxItem = DirectCast(btn1.NamingContainer, RadListBoxItem)
            Dim SID As Label = DirectCast(item.FindControl("lblSalesRepID"), Label)

            dtAvail = Session("dtSurveyAvail")

            dtAssigned = Session("dtSurveyAssigned")


            For Each f As DataRow In dtAvail.Rows
                If f("SalesRep_ID").ToString() = SID.Text Then
                    dtAssigned.ImportRow(f)
                    dtAvail.Rows.Remove(f)
                    Exit For
                End If
            Next

            ' Dim dv As New DataView
            ' dv = dtAssigned.DefaultView

            rlvAssigne.ClearSelection()
            rlvAssigne.Items.Clear()
            rlvAssigne.DataTextField = "SalesRep_Name"
            rlvAssigne.DataValueField = "SalesRep_ID"
            rlvAssigne.DataSource = dtAssigned
            rlvAssigne.DataBind()

            Session.Remove("dtSurveyAssigned")
            Session("dtSurveyAssigned") = dtAssigned

            '  Dim dv1 As New DataView
            '  dv1 = dtAvail.DefaultView


            rlvAvaile.ClearSelection()
            rlvAvaile.Items.Clear()
            rlvAvaile.DataTextField = "SalesRep_Name"
            rlvAvaile.DataValueField = "SalesRep_ID"
            rlvAvaile.DataSource = dtAvail
            rlvAvaile.DataBind()

            Session.Remove("dtSurveyAvail")
            Session("dtSurveyAvail") = dtAvail


        Catch ex As Exception
            Err_No = "14381"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("PHX_006") & "&next=ManageSurvey.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub

    Protected Sub imgMoveRight_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try



            Dim btn1 As ImageButton = TryCast(sender, ImageButton)
            Dim item As RadListBoxItem = DirectCast(btn1.NamingContainer, RadListBoxItem)
            Dim SID As Label = DirectCast(item.FindControl("lblSalesRepID"), Label)

            dtAvail = Session("dtSurveyAvail")

            dtAssigned = Session("dtSurveyAssigned")


            For Each f As DataRow In dtAssigned.Rows
                If f("SalesRep_ID").ToString() = SID.Text Then
                    dtAvail.ImportRow(f)
                    dtAssigned.Rows.Remove(f)
                    Exit For
                End If
            Next

            Dim dv As New DataView
            dv = dtAssigned.DefaultView


            rlvAssigne.ClearSelection()
            rlvAssigne.Items.Clear()
            rlvAssigne.DataTextField = "SalesRep_Name"
            rlvAssigne.DataValueField = "SalesRep_ID"
            rlvAssigne.DataSource = dv
            rlvAssigne.DataBind()

            Session.Remove("dtSurveyAssigned")
            Session("dtSurveyAssigned") = dtAssigned


            Dim dv1 As New DataView
            dv1 = dtAvail.DefaultView

            rlvAvaile.ClearSelection()
            rlvAvaile.Items.Clear()
            rlvAvaile.DataTextField = "SalesRep_Name"
            rlvAvaile.DataValueField = "SalesRep_ID"
            rlvAvaile.DataSource = dv1
            rlvAvaile.DataBind()

            Session.Remove("dtSurveyAvail")
            Session("dtSurveyAvail") = dtAvail



        Catch ex As Exception
            Err_No = "54384"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("PHX_006") & "&next=ManageSurvey.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub
    Private Sub SetQuestionsTable()

        Dim col As DataColumn

        col = New DataColumn()
        col.ColumnName = "Question_ID"
        col.DataType = System.Type.GetType("System.String")
        col.ReadOnly = False
        col.Unique = False
        dtQuestions.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "Question_Text"
        col.DataType = System.Type.GetType("System.String")
        col.ReadOnly = False
        col.Unique = False
        dtQuestions.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "Default_Response_ID"
        col.DataType = System.Type.GetType("System.String")
        col.ReadOnly = False
        col.Unique = False
        dtQuestions.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "Response_Type_ID"
        col.DataType = System.Type.GetType("System.String")
        col.ReadOnly = False
        col.Unique = False
        dtQuestions.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "Response_Type"
        col.DataType = System.Type.GetType("System.String")
        col.ReadOnly = False
        col.Unique = False
        dtQuestions.Columns.Add(col)


        col = New DataColumn()
        col.ColumnName = "IsDeleted"
        col.DataType = System.Type.GetType("System.String")
        col.ReadOnly = False
        col.Unique = False
        dtQuestions.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "ResponsesList"
        col.DataType = System.Type.GetType("System.String")
        col.ReadOnly = False
        col.Unique = False
        dtQuestions.Columns.Add(col)


        col = New DataColumn()
        col.ColumnName = "IsMandatory"
        col.DataType = System.Type.GetType("System.String")
        col.ReadOnly = False
        col.Unique = False
        dtQuestions.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "Group_Text"
        col.DataType = System.Type.GetType("System.String")
        col.ReadOnly = False
        col.Unique = False
        dtQuestions.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "HasImage"
        col.DataType = System.Type.GetType("System.String")
        col.ReadOnly = False
        col.Unique = False
        dtQuestions.Columns.Add(col)


        col = New DataColumn()
        col.ColumnName = "ImageName"
        col.DataType = System.Type.GetType("System.String")
        col.ReadOnly = False
        col.Unique = False
        dtQuestions.Columns.Add(col)


        col = New DataColumn()
        col.ColumnName = "Mandatory_On_Confirmation"
        col.DataType = System.Type.GetType("System.String")
        col.ReadOnly = False
        col.Unique = False
        dtQuestions.Columns.Add(col)


        col = New DataColumn()
        col.ColumnName = "Remarks_Required"
        col.DataType = System.Type.GetType("System.String")
        col.ReadOnly = False
        col.Unique = False
        dtQuestions.Columns.Add(col)


        col = New DataColumn()
        col.ColumnName = "No_Of_Lines_For_Text"
        col.DataType = System.Type.GetType("System.String")
        col.ReadOnly = False
        col.Unique = False
        dtQuestions.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "No_Of_Lines_For_Remarks"
        col.DataType = System.Type.GetType("System.String")
        col.ReadOnly = False
        col.Unique = False
        dtQuestions.Columns.Add(col)


        col = New DataColumn()
        col.ColumnName = "Default_Star_Rating"
        col.DataType = System.Type.GetType("System.String")
        col.ReadOnly = False
        col.Unique = False
        dtQuestions.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "Sequence"
        col.DataType = System.Type.GetType("System.String")
        col.ReadOnly = False
        col.Unique = False
        dtQuestions.Columns.Add(col)


        Session.Remove("dtQuestions")
        Session("dtQuestions") = dtQuestions
    End Sub
    Private Sub SetResponseTable()

        Dim col As DataColumn

        col = New DataColumn()
        col.ColumnName = "Question_ID"
        col.DataType = System.Type.GetType("System.String")
        col.ReadOnly = False
        col.Unique = False
        dtResponses.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "Question_Text"
        col.DataType = System.Type.GetType("System.String")
        col.ReadOnly = False
        col.Unique = False
        dtResponses.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "Response_ID"
        col.DataType = System.Type.GetType("System.String")
        col.ReadOnly = False
        col.Unique = False
        dtResponses.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "Response_Type_ID"
        col.DataType = System.Type.GetType("System.String")
        col.ReadOnly = False
        col.Unique = False
        dtResponses.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "Response_Type"
        col.DataType = System.Type.GetType("System.String")
        col.ReadOnly = False
        col.Unique = False
        dtResponses.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "Response_Text"
        col.DataType = System.Type.GetType("System.String")
        col.ReadOnly = False
        col.Unique = False
        dtResponses.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "DefValue"
        col.DataType = System.Type.GetType("System.String")
        col.ReadOnly = False
        col.Unique = False
        dtResponses.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "IsDeleted"
        col.DataType = System.Type.GetType("System.String")
        col.ReadOnly = False
        col.Unique = False
        dtResponses.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "ShowCommentBox"
        col.DataType = System.Type.GetType("System.String")
        col.ReadOnly = False
        col.Unique = False
        dtResponses.Columns.Add(col)

        Session.Remove("dtResponses")
        Session("dtResponses") = dtResponses

        Session.Remove("dtQResponses")


    End Sub
    'Private Sub SetQResponseTable()


    '    Dim dtQResponses As New DataTable
    '    Dim col As DataColumn

    '    col = New DataColumn()
    '    col.ColumnName = "Response_ID"
    '    col.DataType = System.Type.GetType("System.String")
    '    col.ReadOnly = False
    '    col.Unique = False
    '    dtQResponses.Columns.Add(col)


    '    col = New DataColumn()
    '    col.ColumnName = "Response_Text"
    '    col.DataType = System.Type.GetType("System.String")
    '    col.ReadOnly = False
    '    col.Unique = False
    '    dtQResponses.Columns.Add(col)

    '    col = New DataColumn()
    '    col.ColumnName = "DefValue"
    '    col.DataType = System.Type.GetType("System.String")
    '    col.ReadOnly = False
    '    col.Unique = False
    '    dtQResponses.Columns.Add(col)

    '    col = New DataColumn()
    '    col.ColumnName = "IsDeleted"
    '    col.DataType = System.Type.GetType("System.String")
    '    col.ReadOnly = False
    '    col.Unique = False
    '    dtQResponses.Columns.Add(col)

    '    col = New DataColumn()
    '    col.ColumnName = "ShowCommentBox"
    '    col.DataType = System.Type.GetType("System.String")
    '    col.ReadOnly = False
    '    col.Unique = False
    '    dtQResponses.Columns.Add(col)


    '    Session.Remove("dtQResponses")
    '    Session("dtQResponses") = dtQResponses
    '    dtQResponses = Nothing
    'End Sub
    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Try

        
        ResetFields()
        lbl_imgName.Text = ""
        Me.btnSave.Visible = True
        Me.btnCancel.Visible = True
        Me.txtQuestion.Enabled = True
        Me.ddlResponsType.Enabled = True
        Me.btnAddQuest.Visible = True
        Me.txtTitle.Enabled = True
        Me.txtStartDate.Enabled = True
        Me.txtExpDate.Enabled = True
        Me.ddlTypeCode.Enabled = True
        btnSearch.Enabled = True
        Me.btnClear.Visible = True
        Me.btnCancel.Visible = True
        Me.btnReset.Enabled = True
        Me.txtFilter.Enabled = True
        Me.rlvAssigne.Enabled = True
        Me.rlvAvaile.Enabled = True
        Me.lblSurveyName.Text = ""
        Me.lblMsg.Text = ""


        Me.lblSurveyStatus.Text = "New"
        FillAvailableFSR()
        FillAssignedFSR()
        rgQuestions.Columns(0).Visible = True
        rgQuestions.Columns(1).Visible = True

        RadTabStrip1.SelectedIndex = 0
        RadTabStrip1.SelectedTab.TabIndex = 0

        RadTabStrip1.SelectedTab.Selected = True
        RadMultiPage1.SelectedIndex = 0

        txt_groupTxt.Text = ""
        chk_mandatory.Checked = False
        chk_Image.Checked = False
        file_Imag.Visible = False

        DocWindow.VisibleOnPageLoad = True
        ShowQuestionImage()
        Catch ex As Exception
            log.Debug(ex.ToString)
        End Try
    End Sub
    Protected Sub ddlResponsType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlResponsType.SelectedIndexChanged
        If Me.ddlResponsType.SelectedIndex > 0 Then
            If Me.ddlResponsType.SelectedValue.ToString() = 1 Then
                Me.txtResponse.Text = ""
                Me.rsDefault.Checked = False
                ' divRespButtons.Visible = False
                divRespList.Visible = False
                divResponse.Visible = False
                divIsDefault.Visible = False
                gvResponseList.Visible = False
                Me.gvResponseList.DataSource = Nothing
                gvResponseList.DataBind()
                If ddlTypeCode.SelectedValue = "A" Then
                    NoofLines.Visible = True
                    lbl_resptxt.Text = "No. of Lines"
                    txt_noOfLines.Visible = True
                    ddlStarRating.Visible = False
                Else
                    NoofLines.Visible = False
                End If
            ElseIf Me.ddlResponsType.SelectedValue.ToString() = 8 Then
                Me.txtResponse.Text = ""
                Me.rsDefault.Checked = False
                ' divRespButtons.Visible = False
                divRespList.Visible = False
                divResponse.Visible = False
                divIsDefault.Visible = False
                gvResponseList.Visible = False
                Me.gvResponseList.DataSource = Nothing
                gvResponseList.DataBind()
                If ddlTypeCode.SelectedValue = "A" Then
                    NoofLines.Visible = True
                    lbl_resptxt.Text = "Default Star Rating"
                    txt_noOfLines.Visible = False
                    ddlStarRating.Visible = True
                Else
                    NoofLines.Visible = False
                End If
            ElseIf Me.ddlResponsType.SelectedValue.ToString() = 2 Or Me.ddlResponsType.SelectedValue.ToString() = 3 Or ddlResponsType.SelectedValue.ToString() = 4 Or ddlResponsType.SelectedValue.ToString() = 14 Then
                Me.txtResponse.Text = ""
                Me.rsDefault.Checked = False
                Me.gvResponseList.DataSource = Nothing
                gvResponseList.DataBind()
                ' divRespButtons.Visible = True
                divRespList.Visible = True
                divIsDefault.Visible = True
                divResponse.Visible = True
                gvResponseList.Visible = True
                NoofLines.Visible = False
            Else
                Me.txtResponse.Text = ""
                Me.rsDefault.Checked = False
                ' divRespButtons.Visible = False
                divRespList.Visible = False
                divResponse.Visible = False
                divIsDefault.Visible = False
                gvResponseList.Visible = False
                Me.gvResponseList.DataSource = Nothing
                gvResponseList.DataBind()
                NoofLines.Visible = False

            End If
        End If
    End Sub


    'Protected Sub btnRemoveResp_Click(sender As Object, e As EventArgs) Handles btnRemoveResp.Click

    'End Sub


    Protected Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        Me.lblMsg.Text = ""
        Me.txtQuestion.Text = ""
        Me.txtResponse.Text = ""
        Me.rsDefault.Checked = False
        Me.gvResponseList.DataSource = Nothing
        gvResponseList.DataBind()
        Me.ddlResponsType.SelectedIndex = 0
        gvResponseList.Visible = False
        Me.btnAddQuest.Text = "Save Question & Response"
        Me.hfQuestionID.Value = ""
        Me.txtQuestion.Focus()
        txt_groupTxt.Text = ""
        chk_mandatory.Checked = False
        chk_Image.Checked = False
        divResponse.Visible = False
        divIsDefault.Visible = False
        divRespList.Visible = False
        lbl_imgName.Text = ""
        file_Imag.Visible = False
    End Sub

    Protected Sub btnAddQuest_Click(sender As Object, e As EventArgs) Handles btnAddQuest.Click
        Dim PhysicalPath As String = ""
        PhysicalPath = (New SalesWorx.BO.Common.Common).GetLogoPath(Err_No, Err_Desc, "QUEST_IMG")

        If Me.txtTitle.Text = "" Or Me.txtTitle.Text = "0" Then
            Me.lblMsg.Text = "Please define the survey first"
            RadTabStrip1.SelectedIndex = 0
            RadTabStrip1.SelectedTab.TabIndex = 0

            RadTabStrip1.SelectedTab.Selected = True
            RadMultiPage1.SelectedIndex = 0
            Me.DocWindow.VisibleOnPageLoad = True
            Exit Sub
        End If

        If Me.txtQuestion.Text = "" Or Me.ddlResponsType.SelectedIndex <= 0 Or Me.txtQuestion.Text = "0" Then
            Me.lblMsg.Text = "Please enter the question/response type"
            RadTabStrip1.SelectedIndex = 1
            RadTabStrip1.SelectedTab.TabIndex = 1
            RadTabStrip1.SelectedTab.Selected = True
            RadMultiPage1.SelectedIndex = 1
            Me.DocWindow.VisibleOnPageLoad = True
            Exit Sub
        End If
        If chk_Image.Checked Then
            Dim AuditFiles As String = Nothing
            If txtImage.UploadedFiles.Count = 0 And lbl_imgName.Text = "" Then
                Me.lblMsg.Text = "Please select the file to upload"
                RadTabStrip1.SelectedIndex = 1
                RadTabStrip1.SelectedTab.TabIndex = 1
                RadTabStrip1.SelectedTab.Selected = True
                RadMultiPage1.SelectedIndex = 1
                Me.DocWindow.VisibleOnPageLoad = True
                Exit Sub

            End If
        End If
        If Me.ddlResponsType.SelectedIndex > 0 Then
            If Me.ddlResponsType.SelectedValue = 2 Or Me.ddlResponsType.SelectedValue = 3 Or Me.ddlResponsType.SelectedValue = 4 Or Me.ddlResponsType.SelectedValue = 14 Then
                'If Me.ddlResponseList.Items.Count = 0 Then
                '    Me.lblMsg.Text = "Please enter the response"
                '    RadTabStrip1.SelectedIndex = 1
                '    RadTabStrip1.SelectedTab.TabIndex = 1
                '    RadTabStrip1.SelectedTab.Selected = True
                '    RadMultiPage1.SelectedIndex = 1
                '    Me.DocWindow.VisibleOnPageLoad = True
                '    Exit Sub
                'End If
                If Me.gvResponseList.Rows.Count = 0 Then
                    Me.lblMsg.Text = "Please enter the response"
                    RadTabStrip1.SelectedIndex = 1
                    RadTabStrip1.SelectedTab.TabIndex = 1
                    RadTabStrip1.SelectedTab.Selected = True
                    RadMultiPage1.SelectedIndex = 1
                    Me.DocWindow.VisibleOnPageLoad = True
                    Exit Sub
                End If
            End If
            If Me.ddlTypeCode.SelectedValue = "A" And Me.ddlResponsType.SelectedValue = 1 Then
                If Val(txt_noOfLines.Text) <= 0 Then
                    Me.lblMsg.Text = "Please enter the valid no of lines"
                    RadTabStrip1.SelectedIndex = 1
                    RadTabStrip1.SelectedTab.TabIndex = 1
                    RadTabStrip1.SelectedTab.Selected = True
                    RadMultiPage1.SelectedIndex = 1
                    Me.DocWindow.VisibleOnPageLoad = True
                    Exit Sub
                End If
            End If
            If Me.ddlTypeCode.SelectedValue = "A" And Me.chk_remark.Checked = True Then
                If Val(txt_NoofLinesRemark.Text) <= 0 Then
                    Me.lblMsg.Text = "Please enter the valid no of lines of remarks"
                    RadTabStrip1.SelectedIndex = 1
                    RadTabStrip1.SelectedTab.TabIndex = 1
                    RadTabStrip1.SelectedTab.Selected = True
                    RadMultiPage1.SelectedIndex = 1
                    Me.DocWindow.VisibleOnPageLoad = True
                    Exit Sub
                End If
            End If
        End If

        dtQuestions = Session("dtQuestions")
        dtResponses = Session("dtResponses")


        For id As Integer = dtQuestions.Rows.Count - 1 To 0 Step -1
            Dim dr1 As DataRow = dtQuestions.Rows(id)
            If dr1("Question_ID").ToString() = hfQuestionID.Value Then
                dtQuestions.Rows.Remove(dr1)
            End If
        Next



        dtResponses = Session("dtResponses")


        For id As Integer = dtResponses.Rows.Count - 1 To 0 Step -1
            Dim dr1 As DataRow = dtResponses.Rows(id)
            If dr1("Question_ID").ToString() = hfQuestionID.Value Then
                dtResponses.Rows.Remove(dr1)
            End If
        Next


        Dim ResponsesList As String = ""


        If Me.ddlResponsType.SelectedValue <> 1 Then
            For Each itm As GridViewRow In gvResponseList.Rows
                ResponsesList = ResponsesList & itm.Cells(1).Text & ","
            Next
        Else
            ResponsesList = ResponsesList & ""
        End If



        Dim QuestID As String = ""
        '  If Me.btnAddQuest.Text = "Save Question & Response" Then
        Dim oRow As DataRow = dtQuestions.NewRow()
        QuestID = Guid.NewGuid().ToString()
        oRow("Question_Id") = QuestID
        oRow("Question_Text") = Me.txtQuestion.Text
        oRow("Default_Response_ID") = "0"
        oRow("Response_Type_ID") = Me.ddlResponsType.SelectedValue
        oRow("Response_Type") = Me.ddlResponsType.SelectedItem.Text
        oRow("Group_text") = txt_groupTxt.Text
        oRow("HasImage") = IIf(chk_Image.Checked, "1", "0")
        If chk_Image.Checked Then
            If txtImage.UploadedFiles.Count > 0 Then
                For Each file As UploadedFile In txtImage.UploadedFiles
                    oRow("ImageName") = Guid.NewGuid().ToString & ".png"

                    file.SaveAs(PhysicalPath & "\" & oRow("ImageName").ToString)

                Next
            Else
                oRow("ImageName") = lbl_imgName.Text
            End If
        Else
            oRow("ImageName") = ""
        End If


        oRow("IsMandatory") = IIf(chk_mandatory.Checked, "1", "0")
        oRow("IsDeleted") = "N"
        oRow("ResponsesList") = ResponsesList

        If ddlTypeCode.SelectedValue = "A" Then
            oRow("Mandatory_On_Confirmation") = IIf(chk_mandatoryOnConf.Checked = True, 1, 0)
            oRow("Remarks_Required") = IIf(chk_remark.Checked = True, 1, 0)
            oRow("No_Of_Lines_For_Text") = Val(txt_noOfLines.Text)
            oRow("No_Of_Lines_For_Remarks") = Val(txt_NoofLinesRemark.Text)
            oRow("Default_Star_Rating") = ddlStarRating.SelectedValue.ToString

        End If

        oRow("Sequence") = dtQuestions.Rows.Count + 1

        dtQuestions.Rows.Add(oRow)

        Dim RespID As String = ""

        If Me.ddlResponsType.SelectedValue = 2 Or Me.ddlResponsType.SelectedValue = 3 Or Me.ddlResponsType.SelectedValue = 4 Or Me.ddlResponsType.SelectedValue = 14 Then

            For Each itm As GridViewRow In gvResponseList.Rows
                Dim hRow As DataRow = dtResponses.NewRow()
                RespID = Guid.NewGuid.ToString()
                hRow("Question_Id") = QuestID
                hRow("Question_Text") = Me.txtQuestion.Text
                hRow("Response_Id") = RespID
                hRow("Response_Text") = itm.Cells(1).Text
                hRow("Response_Type_ID") = Me.ddlResponsType.SelectedValue
                hRow("Response_Type") = Me.ddlResponsType.SelectedItem.Text
                Dim ChkdefResponse As New CheckBox
                ChkdefResponse = CType(itm.Cells(0).FindControl("chkDR"), CheckBox)
                hRow("DefValue") = IIf(ChkdefResponse.Checked = True, "1", "0")

                Dim chkShowCommentBox As New CheckBox
                chkShowCommentBox = CType(itm.Cells(0).FindControl("chkShowCommentBox"), CheckBox)

                hRow("ShowCommentBox") = IIf(chkShowCommentBox.Checked = True, "1", "0")
                hRow("IsDeleted") = "N"
                dtResponses.Rows.Add(hRow)
            Next
        Else
            Dim hRow As DataRow = dtResponses.NewRow()
            RespID = Guid.NewGuid.ToString()
            hRow("Question_Id") = QuestID
            hRow("Question_Text") = Me.txtQuestion.Text
            hRow("Response_Id") = RespID
            hRow("Response_Text") = "0"
            hRow("Response_Type_ID") = Me.ddlResponsType.SelectedValue
            hRow("DefValue") = "0"
            hRow("Response_Type") = Me.ddlResponsType.SelectedItem.Text
            hRow("IsDeleted") = "N"
            hRow("ShowCommentBox") = "0"
            dtResponses.Rows.Add(hRow)
        End If
        'End If



        Me.btnAddQuest.Text = "Save Question & Response"
        Me.txtQuestion.Text = ""
        Me.ddlResponsType.SelectedIndex = 0
        Me.rsDefault.Checked = False
        chk_showComment.Checked = False
        chk_mandatory.Checked = True
        Me.txtResponse.Text = ""
        txt_groupTxt.Text = ""
        chk_Image.Checked = False

        Me.gvResponseList.DataSource = Nothing
        gvResponseList.DataBind()
        hfQuestionID.Value = ""

        divRespList.Visible = False
        divResponse.Visible = False
        divIsDefault.Visible = False

        Session.Remove("dtQuestions")
        Session("dtQuestions") = dtQuestions

        Session.Remove("dtResponses")
        Session("dtResponses") = dtResponses


        'dtResponses.DefaultView.RowFilter = "IsDeleted='N'"
        Me.rgQuestions.DataSource = dtQuestions
        Me.rgQuestions.DataBind()
        lbl_imgName.Text = ""
    End Sub

    Protected Sub chkDR_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btnEdit As CheckBox = TryCast(sender, CheckBox)
        Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
        Dim d As New DataTable
        d = CType(Session("dtResponses"), DataTable).Copy
        If btnEdit.Checked = True Then
            For Each r As GridViewRow In gvResponseList.Rows
                Dim rid As New HiddenField
                rid = CType(r.FindControl("HresponseID"), HiddenField)

                Dim item As New HiddenField
                item = CType(row.FindControl("HresponseID"), HiddenField)
                If rid.Value <> item.Value Then
                    Dim chk As New CheckBox
                    chk = CType(r.FindControl("chkDR"), CheckBox)
                    chk.Checked = False
                End If
            Next
        End If
    End Sub
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btndelete As ImageButton = TryCast(sender, ImageButton)
        Dim row As GridViewRow = DirectCast(btndelete.NamingContainer, GridViewRow)
        Dim d As New DataTable
        'If Session("dtQResponses") Is Nothing Then
        '    SetQResponseTable()
        'End If
        If Session("dtResponses") Is Nothing Then
            SetResponseTable()
        End If
        Dim hid As New HiddenField
        hid = CType(row.FindControl("HresponseID"), HiddenField)

        d = CType(Session("dtResponses"), DataTable).Copy

        Dim dtTemp As DataTable
        If d.Select("Response_ID<>'" & hid.Value & "'").Length > 0 Then
            dtTemp = d.Select("Response_ID<>'" & hid.Value & "'").CopyToDataTable
            Session("dtResponses") = dtTemp.Copy
            d = dtTemp.Copy
        Else
            dtTemp = Nothing
            Session("dtResponses") = Nothing
        End If



        Dim seldr() As DataRow
        seldr = d.Select("Question_ID='" & hfQuestionID.Value & "'")
        If seldr.Length > 0 Then
            gvResponseList.DataSource = seldr.CopyToDataTable
            gvResponseList.DataBind()
            If ddlTypeCode.SelectedItem.Value = "S" Then
                gvResponseList.Columns(2).Visible = True
            Else
                gvResponseList.Columns(2).Visible = False
            End If
        Else
            gvResponseList.DataSource = Nothing
            gvResponseList.DataBind()
        End If


      
    End Sub
     

    Protected Sub rgQuestions_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles rgQuestions.ItemCommand
        Try
            If e.CommandName = "DeleteQuestion" Then
                Dim item As GridDataItem = DirectCast(e.Item, GridDataItem)
                Dim QuestionID As Label = DirectCast(item.FindControl("lblQuestionID"), Label)


                dtQuestions = Session("dtQuestions")

                'For Each j As DataRow In dtQuestions.Rows
                '    If j("Question_ID").ToString() = QuestionID.Text Then
                '        j("IsDeleted") = "Y"
                '    End If
                'Next

                For id As Integer = dtQuestions.Rows.Count - 1 To 0 Step -1
                    Dim dr1 As DataRow = dtQuestions.Rows(id)
                    If dr1("Question_ID").ToString() = QuestionID.Text Then
                        dtQuestions.Rows.Remove(dr1)
                    End If
                Next

                dtResponses = Session("dtResponses")

                'For Each j As DataRow In dtResponses.Rows
                '    If j("Question_ID").ToString() = QuestionID.Text Then
                '        j("IsDeleted") = "Y"
                '    End If
                'Next

                For id As Integer = dtResponses.Rows.Count - 1 To 0 Step -1
                    Dim dr1 As DataRow = dtResponses.Rows(id)
                    If dr1("Question_ID").ToString() = QuestionID.Text Then
                        dtResponses.Rows.Remove(dr1)
                    End If
                Next




                'dtResponses.DefaultView.RowFilter = "IsDeleted='N'"

                Session.Remove("dtQuestions")
                Session("dtQuestions") = dtQuestions

                Session.Remove("dtResponses")
                Session("dtResponses") = dtResponses

                Me.rgQuestions.DataSource = dtQuestions
                Me.rgQuestions.DataBind()


            End If
            If e.CommandName = "EditQuestion" Then
                Dim item As GridDataItem = DirectCast(e.Item, GridDataItem)
                Dim QuestionID As Label = DirectCast(item.FindControl("lblQuestionID"), Label)


                dtQuestions = Session("dtQuestions")
                Me.hfQuestionID.Value = QuestionID.Text
                For Each j As DataRow In dtQuestions.Rows
                    If j("Question_ID").ToString() = QuestionID.Text Then

                        Me.txtQuestion.Text = j("Question_text").ToString()
                        Me.ddlResponsType.SelectedValue = j("Response_Type_Id").ToString()
                        Me.txt_groupTxt.Text = j("Group_Text").ToString()
                        If j("IsMandatory") = "1" Then
                            Me.chk_mandatory.Checked = True
                        Else
                            Me.chk_mandatory.Checked = False
                        End If

                        If j("HasImage") = "1" Then
                            Me.chk_Image.Checked = True
                            file_Imag.Visible = True
                            lbl_imgName.Text = j("ImageName").ToString()
                            lbl_imgName.Visible = True
                        Else
                            Me.chk_Image.Checked = False
                            file_Imag.Visible = False
                            lbl_imgName.Visible = False
                        End If

                        Exit For

                    End If
                Next


                dtResponses = Session("dtResponses")
                Dim seldr() As DataRow
                seldr = dtResponses.Select("Question_ID='" & QuestionID.Text & "'")
                Dim temp_dt As New DataTable
                If seldr.Length > 0 Then
                    temp_dt = seldr.CopyToDataTable
                    gvResponseList.DataSource = temp_dt
                    gvResponseList.DataBind()
                    If ddlTypeCode.SelectedItem.Value = "S" Then
                        gvResponseList.Columns(2).Visible = True
                    Else
                        gvResponseList.Columns(2).Visible = False
                    End If
                Else
                    gvResponseList.DataSource = Nothing
                    gvResponseList.DataBind()
                End If

                
                'Session("dtQResponses") = dtResponses.Copy
                'For Each k As DataRow In dtResponses.Rows
                '    Dim rqid As String
                '    rqid=CType(k.findcontrol("")
                '    If k("Question_ID").ToString() = QuestionID.Text Then
                '        Dim r As New RadListBoxItem
                '        r.Text = k("Response_Text").ToString()
                '        r.Value = k("Response_ID").ToString()
                '        r.Checked = IIf(k("DefValue").ToString() = "Y", True, False)
                '        ddlResponseList.Items.Add(r)
                '    End If
                'Next

                If Me.ddlResponsType.SelectedValue.ToString() = 2 Or Me.ddlResponsType.SelectedValue.ToString() = 3 Or Me.ddlResponsType.SelectedValue.ToString() = 4 Or Me.ddlResponsType.SelectedValue.ToString() = 14 Then
                    Me.txtResponse.Text = ""
                    Me.rsDefault.Checked = False
                    divRespList.Visible = True
                    divIsDefault.Visible = True
                    divResponse.Visible = True
                    gvResponseList.Visible = True
                Else
                    Me.txtResponse.Text = ""
                    Me.rsDefault.Checked = False
                    ' divRespButtons.Visible = False
                    divRespList.Visible = False
                    divResponse.Visible = False
                    divIsDefault.Visible = False
                    gvResponseList.Visible = False
                    Me.gvResponseList.DataSource = Nothing
                    Me.gvResponseList.DataBind()
                End If

                Me.btnAddQuest.Text = "Update Question & Response"

            End If



        Catch ex As Exception
            Err_No = "64224"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("PHX_006") & "&next=ManageSurvey.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub
    'Protected Sub txtExpDate_Load(sender As Object, e As EventArgs) Handles txtExpDate.Load
    '    DirectCast(sender, RadDatePicker).MinDate = DateTime.Today
    'End Sub
    'Protected Sub txtStartDate_Load(sender As Object, e As EventArgs) Handles txtStartDate.Load
    '    DirectCast(sender, RadDatePicker).MinDate = DateTime.Today
    'End Sub

    Protected Sub btnAddResp_Click(sender As Object, e As ImageClickEventArgs) Handles btnAddResp.Click
        If Me.txtTitle.Text = "" Or Me.txtTitle.Text = "0" Then
            Me.lblMsg.Text = "Please define the survey first"
            RadTabStrip1.SelectedIndex = 0
            RadTabStrip1.SelectedTab.TabIndex = 0

            RadTabStrip1.SelectedTab.Selected = True
            RadMultiPage1.SelectedIndex = 0
            Me.DocWindow.VisibleOnPageLoad = True
            Exit Sub
        End If

        If Me.txtQuestion.Text = "" Or Me.ddlResponsType.SelectedIndex <= 0 Then
            Me.lblMsg.Text = "Please enter the mandatory fields"
            RadTabStrip1.SelectedIndex = 1
            RadTabStrip1.SelectedTab.TabIndex = 1
            RadTabStrip1.SelectedTab.Selected = True
            RadMultiPage1.SelectedIndex = 1
            Me.DocWindow.VisibleOnPageLoad = True
            Exit Sub
        End If
        Dim respid As String = Guid.NewGuid.ToString()
        If Me.ddlResponsType.SelectedValue <> 1 Then
            If Me.txtResponse.Text = "" Or Me.txtResponse.Text = "0" Then
                Me.lblMsg.Text = "Please enter the response text"
                RadTabStrip1.SelectedIndex = 1
                RadTabStrip1.SelectedTab.TabIndex = 1
                RadTabStrip1.SelectedTab.Selected = True
                RadMultiPage1.SelectedIndex = 1
                Me.DocWindow.VisibleOnPageLoad = True
                Exit Sub
            Else

                Dim d As New DataTable
                If Session("dtResponses") Is Nothing Then
                    SetResponseTable()
                End If
                d = CType(Session("dtResponses"), DataTable).Copy

                For Each r As GridViewRow In gvResponseList.Rows


                    Dim rid As New HiddenField
                    rid = CType(r.FindControl("HresponseID"), HiddenField)

                    Dim seldr1() As DataRow
                    seldr1 = d.Select("Response_Id='" & rid.Value & "'")

                    Dim chkDR As New CheckBox
                    chkDR = CType(r.FindControl("chkDR"), CheckBox)
                    If chkDR.Checked Then
                        seldr1(0)("DefValue") = "1"
                    End If
                    Dim chkShowCommentBox As New CheckBox
                    chkShowCommentBox = CType(r.FindControl("chkShowCommentBox"), CheckBox)
                    If chkShowCommentBox.Checked Then
                        seldr1(0)("ShowCommentBox") = "1"
                    End If
                Next


                Dim dr As DataRow
                dr = d.NewRow

                dr("Question_ID") = hfQuestionID.Value
                dr("Response_Text") = Me.txtResponse.Text
                dr("Response_ID") = respid
                If rsDefault.Checked = True Then
                    dr("DefValue") = "1"
                    For Each Rdr In d.Rows
                        Rdr("DefValue") = "0"
                    Next
                    d.AcceptChanges()
                Else
                    dr("DefValue") = "0"
                End If
                If chk_showComment.Checked = True Then
                    dr("showCommentBox") = "1"
                Else
                    dr("showCommentBox") = "0"
                End If
                d.Rows.Add(dr)
                'Dim r As New RadListBoxItem
                'r.Text = Me.txtResponse.Text
                'r.Value = respid
                'r.Checked = IIf(Me.rsDefault.Checked = True, True, False)
                'ddlResponseList.Items.Add(r)



                Dim seldr() As DataRow
                seldr = d.Select("Question_ID='" & hfQuestionID.Value & "'")
                If seldr.Length > 0 Then
                    gvResponseList.DataSource = seldr.CopyToDataTable
                    gvResponseList.DataBind()
                    If ddlTypeCode.SelectedItem.Value = "S" Then
                        gvResponseList.Columns(2).Visible = True
                    Else
                        gvResponseList.Columns(2).Visible = False
                    End If
                Else
                    gvResponseList.DataSource = Nothing
                    gvResponseList.DataBind()
                End If
                Session("dtResponses") = d.Copy
            End If
        End If

        'For Each h As RadListBoxItem In ddlResponseList.Items
        '    If h.Value <> respid And rsDefault.Checked = True Then
        '        h.Checked = False
        '    End If
        'Next

        Me.txtResponse.Text = ""
        Me.rsDefault.Checked = False
        Me.lblMsg.Text = ""

    End Sub

    Protected Sub btnRemoveResp_Click(sender As Object, e As ImageClickEventArgs) Handles btnRemoveResp.Click
        

        'If Me.txtTitle.Text = "" Then
        '    Me.lblMsg.Text = "Please define the survey first"
        '    RadTabStrip1.SelectedIndex = 0
        '    RadTabStrip1.SelectedTab.TabIndex = 0

        '    RadTabStrip1.SelectedTab.Selected = True
        '    RadMultiPage1.SelectedIndex = 1
        '    Me.DocWindow.VisibleOnPageLoad = True
        '    Exit Sub
        'End If

        'If Me.txtQuestion.Text = "" Or Me.ddlResponsType.SelectedIndex <= 0 Then
        '    Me.lblMsg.Text = "Please enter the question and response type"
        '    RadTabStrip1.SelectedIndex = 1
        '    RadTabStrip1.SelectedTab.TabIndex = 1
        '    RadTabStrip1.SelectedTab.Selected = True
        '    RadMultiPage1.SelectedIndex = 1
        '    Me.DocWindow.VisibleOnPageLoad = True
        '    Exit Sub
        'End If

        '' '' ''If Me.ddlResponseList.SelectedItem Is Nothing Then
        '' '' ''    Me.lblMsg.Text = "Please select the response from the list"
        '' '' ''    RadTabStrip1.SelectedIndex = 1
        '' '' ''    RadTabStrip1.SelectedTab.TabIndex = 1
        '' '' ''    RadTabStrip1.SelectedTab.Selected = True
        '' '' ''    RadMultiPage1.SelectedIndex = 1
        '' '' ''    Me.DocWindow.VisibleOnPageLoad = True
        '' '' ''    Exit Sub
        '' '' ''Else
        '' '' ''    ddlResponseList.Items.Remove(ddlResponseList.SelectedItem)
        '' '' ''    Me.DocWindow.VisibleOnPageLoad = True
        '' '' ''End If
        '' '' ''Me.txtResponse.Text = ""
        '' '' ''Me.rsDefault.Checked = False

    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        ResetFields()
        FillAssignedFSR()
        FillAvailableFSR()
        rgQuestions.Columns(0).Visible = True
        rgQuestions.Columns(1).Visible = True

        DocWindow.VisibleOnPageLoad = False
        ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "CloseWindow();", True)
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If Me.txtTitle.Text = "" Or Me.txtTitle.Text = "0" Then
            Me.lblMsg.Text = "Please define the survey first"
            RadTabStrip1.SelectedIndex = 0
            RadTabStrip1.SelectedTab.TabIndex = 0

            RadTabStrip1.SelectedTab.Selected = True
            RadMultiPage1.SelectedIndex = 0
            Me.DocWindow.VisibleOnPageLoad = True
            Exit Sub
        End If

        dtResponses = Session("dtResponses")
        dtQuestions = Session("dtQuestions")
        dtAssigned = Session("dtSurveyAssigned")

        If Me.txtStartDate.SelectedDate <= Now.Date And Me.lblSurveyStatus.Text = "New" Then
            Me.lblMsg.Text = "Start should be greater than current date "
            RadTabStrip1.SelectedIndex = 0
            RadTabStrip1.SelectedTab.TabIndex = 0

            RadTabStrip1.SelectedTab.Selected = True
            RadMultiPage1.SelectedIndex = 0
            Me.DocWindow.VisibleOnPageLoad = True
            Exit Sub
        End If

        If Me.txtExpDate.SelectedDate < Me.txtStartDate.SelectedDate Then
            Me.lblMsg.Text = "End date should be greater than start date "
            RadTabStrip1.SelectedIndex = 0
            RadTabStrip1.SelectedTab.TabIndex = 0

            RadTabStrip1.SelectedTab.Selected = True
            RadMultiPage1.SelectedIndex = 0
            Me.DocWindow.VisibleOnPageLoad = True
            Exit Sub
        End If

        If dtResponses.Rows.Count <= 0 Or dtQuestions.Rows.Count <= 0 Then
            Me.lblMsg.Text = "Please add some questions & responses"
            RadTabStrip1.SelectedIndex = 1
            RadTabStrip1.SelectedTab.TabIndex = 1

            RadTabStrip1.SelectedTab.Selected = True
            RadMultiPage1.SelectedIndex = 1
            Me.DocWindow.VisibleOnPageLoad = True
            Exit Sub
        End If
        objSurvey.Title = Me.txtTitle.Text
        If objSurvey.CheckduplicateSurvey(Err_No, Err_Desc, IIf(Me.lblSurveyID.Text = "", "0", Me.lblSurveyID.Text)) = True Then
            Me.lblMsg.Text = "Survey title already exists"
            RadTabStrip1.SelectedIndex = 0
            RadTabStrip1.SelectedTab.TabIndex = 0
            RadTabStrip1.SelectedTab.Selected = True
            RadMultiPage1.SelectedIndex = 0
            Me.DocWindow.VisibleOnPageLoad = True
            Exit Sub
        End If

        If objSurvey.CheckSurveyStatus(Err_No, Err_Desc, IIf(Me.lblSurveyID.Text = "", "0", Me.lblSurveyID.Text)) = True Then
            Me.lblMsg.Text = "Currently the survey is deactivated. you can not update any changes now."
            RadTabStrip1.SelectedIndex = 0
            RadTabStrip1.SelectedTab.TabIndex = 0
            RadTabStrip1.SelectedTab.Selected = True
            RadMultiPage1.SelectedIndex = 0
            Me.DocWindow.VisibleOnPageLoad = True
            Exit Sub
        End If
        If objSurvey.SaveSurvey(Err_No, Err_Desc, IIf(Me.lblSurveyID.Text = "", "0", Me.lblSurveyID.Text), Me.txtTitle.Text, Me.txtStartDate.SelectedDate, Me.txtExpDate.SelectedDate, Me.ddlTypeCode.SelectedValue, dtAssigned, dtQuestions, dtResponses, CType(Session.Item("CONTROL_PARAMS"), ControlParams).MSG_MODULE_VERSION, CType(Session("USER_ACCESS"), UserAccess).UserID) = True Then
            MessageBoxValidation(IIf(Me.lblSurveyID.Text = "0", "Successfully survey created.", "Successfully survey updated"), "Validation")
            Me.DocWindow.VisibleOnPageLoad = False
            rgSurvey.Rebind()
            rgSurvey.MasterTableView.Rebind()
            ResetFields()
            FillAssignedFSR()
            FillAvailableFSR()
            ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "CloseWindow();", True)
        Else
            MessageBoxValidation("Error while creating survey", "Validation")
            Me.DocWindow.VisibleOnPageLoad = True
            Exit Sub
        End If
    End Sub

    Private Sub ddlTypeCode_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlTypeCode.SelectedIndexChanged
        HSurveyType.Value = ddlTypeCode.SelectedItem.Value
        If ddlTypeCode.SelectedItem.Value = "S" Then
            divQuestionImage.Visible = True
            divMandatory.Visible = True
            chk_showComment.Visible = True
            chk_mandatoryOnConf.Visible = False
            VanAudit.Visible = False
        ElseIf ddlTypeCode.SelectedItem.Value = "A" Then
            divQuestionImage.Visible = False
            chk_showComment.Visible = True
            divMandatory.Visible = True
            chk_mandatoryOnConf.Visible = True
            VanAudit.Visible = True
        Else
            divQuestionImage.Visible = False
            chk_showComment.Visible = False
            divMandatory.Visible = False
            chk_mandatoryOnConf.Visible = False
            VanAudit.Visible = False
        End If
    End Sub

    Private Sub chk_Image_CheckedChanged(sender As Object, e As EventArgs) Handles chk_Image.CheckedChanged
        If chk_Image.Checked Then
            file_Imag.Visible = True
            lbl_imgName.Visible = True
        Else
            file_Imag.Visible = False
            lbl_imgName.Visible = False
        End If
    End Sub
    Sub ShowQuestionImage()
        Try
            If ddlTypeCode.SelectedItem.Value = "S" Then
                divQuestionImage.Visible = True
                chk_showComment.Visible = True
                divMandatory.Visible = True
                chk_mandatoryOnConf.Visible = False
                VanAudit.Visible = False
            ElseIf ddlTypeCode.SelectedItem.Value = "A" Then
                divQuestionImage.Visible = False
                chk_showComment.Visible = True
                divMandatory.Visible = True
            Else
                divQuestionImage.Visible = False
                chk_showComment.Visible = False
                divMandatory.Visible = False
            End If
        Catch ex As Exception
            log.Error(ex.ToString)
        End Try
    End Sub
    Private Sub rgQuestions_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles rgQuestions.PageIndexChanged
        dtQuestions = Session("dtQuestions")
        Me.rgQuestions.DataSource = dtQuestions
        Me.rgQuestions.DataBind()
        Me.DocWindow.VisibleOnPageLoad = True
    End Sub

    Private Sub chk_remark_CheckedChanged(sender As Object, e As EventArgs) Handles chk_remark.CheckedChanged
        If chk_remark.Checked = True Then
            divNoOfRemars.Visible = True
        Else
            divNoOfRemars.Visible = False
        End If
    End Sub
End Class