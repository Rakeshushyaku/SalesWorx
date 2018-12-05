
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Partial Public Class EditCashVanAudit
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjSurvey As New SalesWorx.BO.Common.Survey
    Private dtItem As New DataTable
    Private dtDetails As New DataTable
    Public DivHTML As String = ""
    Private Const PageID As String = "P97"
    'Private SurveyID As String = ""
    'Private SalesRepId As String = ""
    'Private AVanName As String = ""
    'Private EmpCode As String = ""
    Dim SalesPersonName As String = ""
    Dim DateOfAudit As String = ""
    Dim PrevAuditDAte As String = ""
    Dim Division_SalesOrgID As String = ""
    Dim CheckedBy As String = ""
    Private OrgID As String = ""


    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objLogin As New SalesWorx.BO.Common.Login

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            '  Response.Redirect("Login.aspx")
        End If
        If Not IsPostBack Then
            Dim HasPermission As Boolean = False
            'SalesWorx.BO.Common.ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            'If Not HasPermission Then
            'Err_No = 500
            'Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            ' End If
            'ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
            Try
                ObjCommon = New SalesWorx.BO.Common.Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlVan.DataSource = ObjCommon.GetAllVan(Err_No, Err_Desc, SubQry)
                ddlVan.DataBind()
                ddlVan.Items.Insert(0, New ListItem("-- Select a Van --"))
            Catch ex As Exception
                Err_No = "74066"
                If Err_Desc Is Nothing Then
                    log.Error(GetExceptionInfo(ex))
                Else
                    log.Error(Err_Desc)
                End If
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
            Finally
                ObjCommon = Nothing
                ErrorResource = Nothing

            End Try
            SetItemTable()
        Else
            dtItem = Session("dtLine")
        End If
    End Sub

    Private Sub SetItemTable()
        Dim col As DataColumn


        col = New DataColumn()
        col.ColumnName = "LineId"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtItem.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "QuestId"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtItem.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "Question"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtItem.Columns.Add(col)


        col = New DataColumn()
        col.ColumnName = "RespId"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtItem.Columns.Add(col)


        col = New DataColumn()
        col.ColumnName = "Response"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtItem.Columns.Add(col)


        col = New DataColumn()
        col.ColumnName = "ISDefault"
        col.DataType = System.Type.[GetType]("System.Boolean")
        col.[ReadOnly] = False
        col.Unique = False
        dtItem.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "RespTypeId"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtItem.Columns.Add(col)



        Session.Remove("dtLine")
        Session("dtLine") = dtItem


    End Sub

    Private Sub BindDetails(ByVal dt1 As DataTable)

        dtDetails = dt1
        dtItem = Session("dtLine")
        If dtItem.Rows.Count > 0 Then
            dtItem.Rows.Clear()
            Me.gvResponse.DataSource = Nothing
            Me.gvResponse.DataSource = dtItem
            Me.gvResponse.DataBind()
            Session.Remove("dtLine")
            Session("dtLine") = dtItem
        End If

        Dim dv As DataView = dtDetails.DefaultView
        dv.Sort = "Question_Id, ISDefault DESC"
        dtDetails = dv.ToTable()


        Dim b As String = "A"
        Dim i As Integer = 0
        Dim QID As String = Nothing
        Dim RID As String = Nothing
        If dtDetails.Rows.Count > 0 Then
            For Each dr As DataRow In dtDetails.Rows
                i = i + 1
                For Each d As DataRow In dtItem.Rows
                    If d("QuestId").ToString() = dr("Question_Id").ToString() And dr("Response").ToString() = d("Respid").ToString() Then
                        b = "X"
                        Exit For
                    End If
                    If d("QuestId").ToString() = dr("Question_Id").ToString() And dr("Response").ToString() <> d("Respid").ToString() Then
                        QID = dr("Question_Id").ToString()
                        RID = dr("Response").ToString()
                        For Each x As DataRow In dtItem.Rows
                            If x("QuestId").ToString() = QID And RID = x("Respid").ToString() Then
                                b = "X"
                                Exit For
                            End If
                        Next
                        b = "N"
                    End If

                Next
                If b = "A" Then
                    Dim oRow1 As DataRow = dtItem.NewRow()
                    oRow1("LineId") = ""
                    oRow1("QuestId") = ""
                    oRow1("Question") = ""
                    oRow1("RespId") = ""
                    oRow1("Response") = ""
                    oRow1("ISDefault") = CBool("0")
                    oRow1("RespTypeId") = "0"
                    dtItem.Rows.InsertAt(oRow1, dtItem.Rows.Count + 1)
                End If
                If b <> "X" Then
                    Dim oRow As DataRow = dtItem.NewRow()
                    oRow("LineId") = IIf(b = "A", i, "")
                    oRow("QuestId") = dr("Question_Id").ToString()
                    oRow("Question") = IIf(b = "A", dr("Question_text").ToString(), "")
                    If dr("Response_Type_ID").ToString() <> "1" Then
                        oRow("RespId") = dr("Response").ToString()
                    Else
                        oRow("RespId") = "0"
                    End If
                    If dr("Response_Type_ID").ToString() = "1" Then
                        oRow("Response") = dr("ResText").ToString()
                    Else
                        oRow("Response") = dr("ResponseText").ToString()
                    End If
                    oRow("ISDefault") = CBool(dr("IsDefault").ToString())
                    oRow("RespTypeId") = dr("Response_type_id").ToString()
                    dtItem.Rows.InsertAt(oRow, dtItem.Rows.Count + 1)
                End If
                If b = "X" Or b = "N" Then
                    i = i - 1
                End If
                b = "A"
            Next
            Me.gvResponse.DataSource = Nothing
            Me.gvResponse.DataSource = dtItem
            Me.gvResponse.DataBind()
            Session.Remove("dtLine")
            Session("dtLine") = dtItem
            Dim PrevQuestId As String = Nothing
            Dim CurrentQuestId As String = Nothing
            For Each gvr As GridViewRow In gvResponse.Rows
                If (gvr.RowType = DataControlRowType.DataRow) Then
                    Dim lbType As CheckBox = DirectCast(gvr.FindControl("ChkDefault"), CheckBox)
                    Dim lbR As RadioButton = DirectCast(gvr.FindControl("Rb"), RadioButton)
                    Dim lbRespType As Label = DirectCast(gvr.FindControl("lbRespType"), Label)
                    Dim txtRes As TextBox = DirectCast(gvr.FindControl("txtResponse"), TextBox)
                    Dim lbQuestId As Label = DirectCast(gvr.FindControl("lbQuestId"), Label)
                    CurrentQuestId = lbQuestId.Text
                    If lbRespType.Text = "1" Then
                        lbType.Visible = False
                        txtRes.Visible = True
                        lbR.Visible = False
                    ElseIf lbRespType.Text = "2" Then
                        lbType.Visible = False
                        txtRes.Visible = False
                        lbR.Visible = True
                        'If CurrentQuestId = PrevQuestId Or PrevQuestId Is Nothing Then
                        '    lbR.GroupName = "rbg" & CurrentQuestId
                        'End If

                    ElseIf lbRespType.Text = "3" Then
                        lbType.Visible = True
                        txtRes.Visible = False
                        lbR.Visible = False
                    ElseIf lbRespType.Text = "0" Then
                        lbType.Visible = False
                        txtRes.Visible = False
                        lbR.Visible = False
                    End If


                End If
                PrevQuestId = CurrentQuestId
            Next
        End If
    End Sub


    Protected Sub rb_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim rb As RadioButton = DirectCast(sender, RadioButton)
        Dim row As GridViewRow = DirectCast(rb.NamingContainer, GridViewRow)
        Dim lbQuest As Label = DirectCast(row.FindControl("lbQuestId"), Label)
        dtItem = Session("dtLine")
        For Each oldrow As GridViewRow In gvResponse.Rows
            If DirectCast(oldrow.FindControl("lbQuestId"), Label).Text = lbQuest.Text Then
                DirectCast(oldrow.FindControl("Rb"), RadioButton).Checked = False
                dtItem.Rows(oldrow.RowIndex)("ISDefault") = False
            End If
        Next
        dtItem.AcceptChanges()
        'Set the new selected row 
        DirectCast(row.FindControl("Rb"), RadioButton).Checked = True
        dtItem.Rows(row.RowIndex)("ISDefault") = True
        Me.gvResponse.DataSource = Nothing
        Me.gvResponse.DataSource = dtItem
        Me.gvResponse.DataBind()
        Session.Remove("dtLine")
        Session("dtLine") = dtItem
        Dim PrevQuestId As String = Nothing
        Dim CurrentQuestId As String = Nothing
        For Each gvr As GridViewRow In gvResponse.Rows
            If (gvr.RowType = DataControlRowType.DataRow) Then
                Dim lbType As CheckBox = DirectCast(gvr.FindControl("ChkDefault"), CheckBox)
                Dim lbR As RadioButton = DirectCast(gvr.FindControl("Rb"), RadioButton)
                Dim lbRespType As Label = DirectCast(gvr.FindControl("lbRespType"), Label)
                Dim txtRes As TextBox = DirectCast(gvr.FindControl("txtResponse"), TextBox)
                Dim lbQuestId As Label = DirectCast(gvr.FindControl("lbQuestId"), Label)
                CurrentQuestId = lbQuestId.Text
                If lbRespType.Text = "1" Then
                    lbType.Visible = False
                    txtRes.Visible = True
                    lbR.Visible = False
                ElseIf lbRespType.Text = "2" Then
                    lbType.Visible = False
                    txtRes.Visible = False
                    lbR.Visible = True
                    'If CurrentQuestId = PrevQuestId Or PrevQuestId Is Nothing Then
                    '    lbR.GroupName = "rbg" & CurrentQuestId
                    'End If

                ElseIf lbRespType.Text = "3" Then
                    lbType.Visible = True
                    txtRes.Visible = False
                    lbR.Visible = False
                ElseIf lbRespType.Text = "0" Then
                    lbType.Visible = False
                    txtRes.Visible = False
                    lbR.Visible = False
                End If


            End If
            PrevQuestId = CurrentQuestId
        Next
    End Sub

    Protected Sub Chk_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim rb As CheckBox = DirectCast(sender, CheckBox)
        Dim row As GridViewRow = DirectCast(rb.NamingContainer, GridViewRow)
        'Set the new selected row 
        ' DirectCast(row.FindControl("ChkDefault"), CheckBox).Checked = True
        dtItem = Session("dtLine")
        dtItem.Rows(row.RowIndex)("ISDefault") = CBool(IIf(rb.Checked = True, 1, 0))
        dtItem.AcceptChanges()
        Me.gvResponse.DataSource = Nothing
        Me.gvResponse.DataSource = dtItem
        Me.gvResponse.DataBind()
        Session.Remove("dtLine")
        Session("dtLine") = dtItem

        Dim PrevQuestId As String = Nothing
        Dim CurrentQuestId As String = Nothing
        For Each gvr As GridViewRow In gvResponse.Rows
            If (gvr.RowType = DataControlRowType.DataRow) Then
                Dim lbType As CheckBox = DirectCast(gvr.FindControl("ChkDefault"), CheckBox)
                Dim lbR As RadioButton = DirectCast(gvr.FindControl("Rb"), RadioButton)
                Dim lbRespType As Label = DirectCast(gvr.FindControl("lbRespType"), Label)
                Dim txtRes As TextBox = DirectCast(gvr.FindControl("txtResponse"), TextBox)
                Dim lbQuestId As Label = DirectCast(gvr.FindControl("lbQuestId"), Label)
                CurrentQuestId = lbQuestId.Text
                If lbRespType.Text = "1" Then
                    lbType.Visible = False
                    txtRes.Visible = True
                    lbR.Visible = False
                ElseIf lbRespType.Text = "2" Then
                    lbType.Visible = False
                    txtRes.Visible = False
                    lbR.Visible = True
                    'If CurrentQuestId = PrevQuestId Or PrevQuestId Is Nothing Then
                    '    lbR.GroupName = "rbg" & CurrentQuestId
                    'End If

                ElseIf lbRespType.Text = "3" Then
                    lbType.Visible = True
                    txtRes.Visible = False
                    lbR.Visible = False
                ElseIf lbRespType.Text = "0" Then
                    lbType.Visible = False
                    txtRes.Visible = False
                    lbR.Visible = False
                End If


            End If
            PrevQuestId = CurrentQuestId
        Next
    End Sub

    Protected Sub txtResponse_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim txt As TextBox = DirectCast(sender, TextBox)
        Dim row As GridViewRow = DirectCast(txt.NamingContainer, GridViewRow)
        dtItem = Session("dtLine")
        dtItem.Rows(row.RowIndex)("Response") = IIf(txt.Text = "", "-", txt.Text)
        dtItem.AcceptChanges()
        Me.gvResponse.DataSource = Nothing
        Me.gvResponse.DataSource = dtItem
        Me.gvResponse.DataBind()
        Session.Remove("dtLine")
        Session("dtLine") = dtItem
        Dim PrevQuestId As String = Nothing
        Dim CurrentQuestId As String = Nothing
        For Each gvr As GridViewRow In gvResponse.Rows
            If (gvr.RowType = DataControlRowType.DataRow) Then
                Dim lbType As CheckBox = DirectCast(gvr.FindControl("ChkDefault"), CheckBox)
                Dim lbR As RadioButton = DirectCast(gvr.FindControl("Rb"), RadioButton)
                Dim lbRespType As Label = DirectCast(gvr.FindControl("lbRespType"), Label)
                Dim txtRes As TextBox = DirectCast(gvr.FindControl("txtResponse"), TextBox)
                Dim lbQuestId As Label = DirectCast(gvr.FindControl("lbQuestId"), Label)
                CurrentQuestId = lbQuestId.Text
                If lbRespType.Text = "1" Then
                    lbType.Visible = False
                    txtRes.Visible = True
                    lbR.Visible = False
                ElseIf lbRespType.Text = "2" Then
                    lbType.Visible = False
                    txtRes.Visible = False
                    lbR.Visible = True
                    'If CurrentQuestId = PrevQuestId Or PrevQuestId Is Nothing Then
                    '    lbR.GroupName = "rbg" & CurrentQuestId
                    'End If

                ElseIf lbRespType.Text = "3" Then
                    lbType.Visible = True
                    txtRes.Visible = False
                    lbR.Visible = False
                ElseIf lbRespType.Text = "0" Then
                    lbType.Visible = False
                    txtRes.Visible = False
                    lbR.Visible = False
                End If


            End If
            PrevQuestId = CurrentQuestId
        Next
    End Sub



    Protected Sub SearchBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SearchBtn.Click
        If Me.ddlVan.SelectedIndex > 0 Then
            LoadDetails()
        Else
            If dtItem.Rows.Count > 0 Then
                dtItem.Rows.Clear()
                Me.gvResponse.DataSource = Nothing
                Me.gvResponse.DataSource = dtItem
                Me.gvResponse.DataBind()
                Session.Remove("dtLine")
                Session("dtLine") = dtItem
            End If
            Me.lblMessage.Text = "Please select a van"
            lblMessage.ForeColor = Drawing.Color.Red
            lblinfo.Text = "Information"
            MpInfoError.Show()
            btnClose.Focus()
            Exit Sub
        End If
    End Sub

    'Protected Sub ddlDivision_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
    '    LoadDetails()
    'End Sub

    Sub LoadDetails()
        ObjCommon = New SalesWorx.BO.Common.Common()
        Dim SubQry As String = " A.SalesRep_ID='" + ddlVan.SelectedValue + "'"
        Dim dt As New DataTable
        dt = ObjCommon.GetVanAuditReport(Err_No, Err_Desc, SubQry)

        Dim HeaderTemplate As String = "<tr><td height='12' width='5%' class='tdstyle' style='font-size: 11px; border:1px solid;border-color: #FFFFFF'>&nbsp;</td> <td class='tdstyle'  style='font-size: 11px; border:1px solid;border-color: #FFFFFF'> $INFO$ </td></tr>"
        'Dim RowTemplate As String = "<tr> <td height='12' width='5%' class='tdstyle' style='border:1px solid;border-color: #FFFFFF'>$SLNO$</td><td class='tdstyle' width='60%'  style='border:1px solid;border-color: #FFFFFF'> $QUEST$ </td><td class='tdstyle' width='35%'  style='border:1px solid;border-color: #FFFFFF'>$ANS$</td></tr>"
        Dim AuditTemplate As String = "<tr><td height='12' width='5%' class='tdstyle' style='font-size: 11px; border:1px solid;border-color: #FFFFFF'>&nbsp;</td> <td height='12' width='60%'  class='tdstyle' style='font-size: 11px; border:1px solid;border-color: #FFFFFF'>$INFO$</td><td class='tdstyle'  style='font-size: 11px;border:1px solid;border-color: #FFFFFF'> $ANS$ </td></tr>"

        SalesPersonName = ""
        DateOfAudit = ""
        PrevAuditDAte = ""
        Division_SalesOrgID = ""
        CheckedBy = ""
        OrgID = ""
        If Not IsNothing(dt) Then
            If dt.Rows.Count > 0 Then
                SalesPersonName = dt.Rows(0)("Emp_Name").ToString() & "-" & dt.Rows(0)("Emp_Code").ToString()
                DateOfAudit = Convert.ToDateTime(dt.Rows(0)("Survey_Timestamp")).ToString("dd/MM/yyyy")
                Division_SalesOrgID = dt.Rows(0)("Site").ToString()
                OrgID = dt.Rows(0)("Sales_ORG_ID").ToString()
                PrevAuditDAte = ObjCommon.GetPrevAuditDate(Err_No, Err_Desc, dt.Rows(0)("Survey_Timestamp").ToString(), dt.Rows(0)("SalesRep_ID"))
                CheckedBy = dt.Rows(0)("UserName").ToString()
                Me.txtSurveyId.Text = dt.Rows(0)("Survey_Id").ToString()
                Me.txtSalesRepid.Text = dt.Rows(0)("SalesRep_Id").ToString()
                Me.txtEmpCode.Text = dt.Rows(0)("Emp_Code").ToString()
                txtAVanName.Text = dt.Rows(0)("SalesRepName").ToString()
                Me.txtSurveyTime.Text = dt.Rows(0)("Survey_Timestamp").ToString()
            End If
        End If


        Dim sb As New StringBuilder("")
        sb.Append("<table width='100%'  border='1' style='font-weight: bold; font-size: 11px; border:1px solid;border-color: #FFFFFF' cellpadding='6'  cellspacing = '0' > ")
        sb.Append(HeaderTemplate.Replace("$INFO$", "Name of Salesman : " & SalesPersonName))
        sb.Append(HeaderTemplate.Replace("$INFO$", "Organization: " & Division_SalesOrgID))
        sb.Append("</table>")
        sb.Append("<div height='15'>&nbsp; </div>")
        sb.Append("<table width='100%'  border='1' style='font-weight: bold;font-size: 11px; border:1px solid;border-color: #FFFFFF' cellpadding='6'  cellspacing = '0' > ")
        sb.Append(AuditTemplate.Replace("$INFO$", "Date of Audit").Replace("$ANS$", DateOfAudit))
        sb.Append(AuditTemplate.Replace("$INFO$", "Date of previous audit").Replace("$ANS$", PrevAuditDAte))
        sb.Append("</table>")
        'sb.Append("<div height='15'>&nbsp; </div>")
        'sb.Append("<table width='100%'  border='1' style='border:1px solid;border-color: #FFFFFF' cellpadding='6'  cellspacing = '0' > ")
        'Dim i As Integer = 1
        'For Each dr As DataRow In dt.Rows
        '    sb.Append(RowTemplate.Replace("$SLNO$", i.ToString()).Replace("$QUEST$", dr("Question_text").ToString()).Replace("$ANS$", dr("ResponseText").ToString()))
        '    i += 1
        'Next
        'sb.Append("</table>")
        'sb.Append("<table width='100%'  border='0' style='border:0px solid;border-color: #FFFFFF' cellpadding='6'  cellspacing = '0' > ")
        'sb.Append("<tr><td width='5%'>&nbsp;</td><td width='60%'>Checked By </td><td width='35%'>Sales Manager</td></tr>")
        'sb.Append("<tr><td width='5%'>&nbsp;</td><td width='60%'>[" + CheckedBy + "]</td><td width='35%'>&nbsp;</td></tr>")
        'sb.Append("</table>")

        DivHTML = sb.ToString()

        '' Literal1.Text = sb.ToString()

        'ObjCommon = New SalesWorx.BO.Common.Common()
        ''  Dim SubQry As String = "A.SalesRep_ID='" + ddlVan.SelectedValue + "'"
        'gvResponse.DataSource = dt
        'gvResponse.DataBind()
        If Not IsNothing(dt) Then
            If dt.Rows.Count > 0 Then
                BindDetails(dt)
            Else
                dtItem = Session("dtLine")
                If dtItem.Rows.Count > 0 Then
                    dtItem.Rows.Clear()
                    Me.gvResponse.DataSource = Nothing
                    Me.gvResponse.DataSource = dtItem
                    Me.gvResponse.DataBind()
                    Session.Remove("dtLine")
                    Session("dtLine") = dtItem
                    txtSalesRepid.Text = ""
                    txtSurveyId.Text = ""
                    txtAVanName.Text = ""
                    txtEmpCode.Text = ""
                End If
            End If
        End If
    End Sub


    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        If Me.ddlVan.SelectedIndex > 0 And gvResponse.Rows.Count > 0 Then
            dtItem = Session("dtLine")
            If ddlVan.SelectedItem.Text <> txtAVanName.Text Then
                Me.lblMessage.Text = "The data currenlty loaded for '" & txtAVanName.Text & "'. Please select a proper van."
                lblMessage.ForeColor = Drawing.Color.Red
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
                Exit Sub
            End If
            Try
                Err_No = Nothing
                Err_Desc = Nothing
                Dim success As Boolean



                success = ObjSurvey.SaveVanAudit(Err_No, Err_Desc, Me.txtEmpCode.Text, Me.txtSalesRepid.Text, Me.txtSurveyId.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), dtItem, Me.txtSurveyTime.Text)

                If success = True Then
                    Me.lblMessage.Text = "Successfully updated"
                    lblMessage.ForeColor = Drawing.Color.Green
                    lblinfo.Text = "Information"
                    MpInfoError.Show()
                    btnClose.Focus()

                Else
                    Me.lblMessage.Text = "Error while updating survey"
                    lblMessage.ForeColor = Drawing.Color.Red
                    lblinfo.Text = "Validation"
                    MpInfoError.Show()
                    btnClose.Focus()
                    Exit Sub
                End If

            Catch ex As Exception
                log.Error(ex.Message)
                Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_CashVanAudit_006") & "&next=EdiCashVanAudit.aspx&Title=CashVanAudit", False)
            End Try
        Else

            Me.lblMessage.Text = "Please select a van"
            lblMessage.ForeColor = Drawing.Color.Red
            lblinfo.Text = "Information"
            MpInfoError.Show()
            btnClose.Focus()
            Exit Sub
        End If

    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button2.Click
        If Me.ddlVan.SelectedIndex > 0 And gvResponse.Rows.Count > 0 Then

            If ddlVan.SelectedItem.Text <> txtAVanName.Text Then
                Me.lblMessage.Text = "The data currently loaded for '" & txtAVanName.Text & "'. Please select a proper van."
                lblMessage.ForeColor = Drawing.Color.Red
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
                Exit Sub
            End If
            '  Dim s As String() = ddlVan.SelectedItem.Text.Split("-")
            Dim VanID As String = ddlVan.SelectedValue
            '  If s.Length > 1 Then
            'VanID = s(1)
            '  End If

            dtItem = Session("dtLine")

            Try
                Err_No = Nothing
                Err_Desc = Nothing
                Dim success As Boolean
                success = ObjSurvey.ConfirmVanAudit(Err_No, Err_Desc, txtSalesRepid.Text, txtSurveyId.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), Me.txtSurveyTime.Text)
                objLogin.SaveUserLog(Err_No, Err_Desc, "A", "SURVEY", "MODIFY AUDIT SURVEY", VanID.Trim(), "SurveyID: " & txtSurveyId.Text & "/Audit Date: " & DateOfAudit & "/ Survey Times : " & Me.txtSurveyTime.Text & "/ Checked By : " & CheckedBy & " Sales Rep: " & SalesPersonName & "/ Status: Confirmed", CType(Session("User_Access"), UserAccess).UserID.ToString(), VanID.Trim(), OrgID)
                If success = True Then
                    Me.lblMessage.Text = "Successfully confirmed"
                    lblMessage.ForeColor = Drawing.Color.Green
                    lblinfo.Text = "Information"
                    MpInfoError.Show()
                    btnClose.Focus()
                    If dtItem.Rows.Count > 0 Then
                        dtItem.Rows.Clear()
                        Me.gvResponse.DataSource = Nothing
                        Me.gvResponse.DataSource = dtItem
                        Me.gvResponse.DataBind()
                        Session.Remove("dtLine")
                        Session("dtLine") = dtItem
                        Me.ddlVan.SelectedIndex = 0
                    End If
                    txtSalesRepid.Text = ""
                    txtSurveyId.Text = ""
                    txtAVanName.Text = ""
                    txtEmpCode.Text = ""
                Else
                    Me.lblMessage.Text = "Error while confirm survey"
                    lblMessage.ForeColor = Drawing.Color.Red
                    lblinfo.Text = "Validation"
                    MpInfoError.Show()
                    btnClose.Focus()
                    Exit Sub
                End If

            Catch ex As Exception
                log.Error(ex.Message)
                Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_CashVanAudit_006") & "&next=EdiCashVanAudit.aspx&Title=CashVanAudit", False)
            End Try
        Else
            Me.lblMessage.Text = "Please select a van"
            lblMessage.ForeColor = Drawing.Color.Red
            lblinfo.Text = "Information"
            MpInfoError.Show()
            btnClose.Focus()
            Exit Sub

        End If

    End Sub
End Class