
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports Telerik.Web.UI
Partial Public Class EditCashVanAudit
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjSurvey As New SalesWorx.BO.Common.Survey


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
    Private row_edit As Boolean = False

    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objLogin As New SalesWorx.BO.Common.Login

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

      
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
                NoSurvey.Visible = False
                vandetails.Visible = False
                ObjCommon = New SalesWorx.BO.Common.Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlVan.DataSource = ObjCommon.GetAllVan(Err_No, Err_Desc, SubQry)
                ddlVan.DataBind()
                ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a Van --", "0"))
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


            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        End Try
    End Sub

    

    'Private Sub BindDetails(ByVal dt1 As DataTable)

    '    dtDetails = dt1
    '    dtItem = Session("dtLine")
    '    If dtItem.Rows.Count > 0 Then
    '        dtItem.Rows.Clear()
    '        Me.gvResponse.DataSource = Nothing
    '        Me.gvResponse.DataSource = dtItem
    '        Me.gvResponse.DataBind()
    '        Session.Remove("dtLine")
    '        Session("dtLine") = dtItem
    '    End If

    '    Dim dv As DataView = dtDetails.DefaultView
    '    dv.Sort = "Question_Id, ISDefault DESC"
    '    dtDetails = dv.ToTable()


    '    Dim b As String = "A"
    '    Dim i As Integer = 0
    '    Dim QID As String = Nothing
    '    Dim RID As String = Nothing
    '    If dtDetails.Rows.Count > 0 Then
    '        For Each dr As DataRow In dtDetails.Rows
    '            i = i + 1
    '            For Each d As DataRow In dtItem.Rows
    '                If d("QuestId").ToString() = dr("Question_Id").ToString() And dr("Response").ToString() = d("Respid").ToString() Then
    '                    b = "X"
    '                    Exit For
    '                End If
    '                If d("QuestId").ToString() = dr("Question_Id").ToString() And dr("Response").ToString() <> d("Respid").ToString() Then
    '                    QID = dr("Question_Id").ToString()
    '                    RID = dr("Response").ToString()
    '                    For Each x As DataRow In dtItem.Rows
    '                        If x("QuestId").ToString() = QID And RID = x("Respid").ToString() Then
    '                            b = "X"
    '                            Exit For
    '                        End If
    '                    Next
    '                    b = "N"
    '                End If

    '            Next
    '            If b = "A" Then
    '                Dim oRow1 As DataRow = dtItem.NewRow()
    '                oRow1("LineId") = ""
    '                oRow1("QuestId") = ""
    '                oRow1("Question") = ""
    '                oRow1("RespId") = ""
    '                oRow1("Response") = ""
    '                oRow1("ISDefault") = CBool("0")
    '                oRow1("RespTypeId") = "0"
    '                dtItem.Rows.InsertAt(oRow1, dtItem.Rows.Count + 1)
    '            End If
    '            If b <> "X" Then
    '                Dim oRow As DataRow = dtItem.NewRow()
    '                oRow("LineId") = IIf(b = "A", i, "")
    '                oRow("QuestId") = dr("Question_Id").ToString()
    '                oRow("Question") = IIf(b = "A", dr("Question_text").ToString(), "")
    '                If dr("Response_Type_ID").ToString() <> "1" Then
    '                    oRow("RespId") = dr("Response").ToString()
    '                Else
    '                    oRow("RespId") = "0"
    '                End If
    '                If dr("Response_Type_ID").ToString() = "1" Then
    '                    oRow("Response") = dr("ResText").ToString()
    '                Else
    '                    oRow("Response") = dr("ResponseText").ToString()
    '                End If
    '                oRow("ISDefault") = CBool(dr("IsDefault").ToString())
    '                oRow("RespTypeId") = dr("Response_type_id").ToString()
    '                dtItem.Rows.InsertAt(oRow, dtItem.Rows.Count + 1)
    '            End If
    '            If b = "X" Or b = "N" Then
    '                i = i - 1
    '            End If
    '            b = "A"
    '        Next
    '        Me.gvResponse.DataSource = Nothing
    '        Me.gvResponse.DataSource = dtItem
    '        Me.gvResponse.DataBind()
    '        Session.Remove("dtLine")
    '        Session("dtLine") = dtItem
    '        Dim PrevQuestId As String = Nothing
    '        Dim CurrentQuestId As String = Nothing
    '        For Each gvr As GridViewRow In gvResponse.Rows
    '            If (gvr.RowType = DataControlRowType.DataRow) Then
    '                Dim lbType As CheckBox = DirectCast(gvr.FindControl("ChkDefault"), CheckBox)
    '                Dim lbR As RadioButton = DirectCast(gvr.FindControl("Rb"), RadioButton)
    '                Dim lbRespType As Label = DirectCast(gvr.FindControl("lbRespType"), Label)
    '                Dim txtRes As TextBox = DirectCast(gvr.FindControl("txtResponse"), TextBox)
    '                Dim lbQuestId As Label = DirectCast(gvr.FindControl("lbQuestId"), Label)
    '                CurrentQuestId = lbQuestId.Text
    '                If lbRespType.Text = "1" Then
    '                    lbType.Visible = False
    '                    txtRes.Visible = True
    '                    lbR.Visible = False
    '                ElseIf lbRespType.Text = "2" Then
    '                    lbType.Visible = False
    '                    txtRes.Visible = False
    '                    lbR.Visible = True
    '                    'If CurrentQuestId = PrevQuestId Or PrevQuestId Is Nothing Then
    '                    '    lbR.GroupName = "rbg" & CurrentQuestId
    '                    'End If

    '                ElseIf lbRespType.Text = "3" Then
    '                    lbType.Visible = True
    '                    txtRes.Visible = False
    '                    lbR.Visible = False
    '                ElseIf lbRespType.Text = "0" Then
    '                    lbType.Visible = False
    '                    txtRes.Visible = False
    '                    lbR.Visible = False
    '                End If


    '            End If
    '            PrevQuestId = CurrentQuestId
    '        Next
    '    End If
    'End Sub

    Sub BindDetails(dt As DataTable)
        Try

       
        Dim dtqids As New DataTable
        Dim dtItem As New DataTable

        Dim col As DataColumn

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


        col = New DataColumn()
        col.ColumnName = "Restxt"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtItem.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "Editable"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtItem.Columns.Add(col)

            col = New DataColumn()
            col.ColumnName = "Mandatory_On_Confirmation"
            col.DataType = System.Type.[GetType]("System.String")
            col.[ReadOnly] = False
            col.Unique = False
            dtItem.Columns.Add(col)


            col = New DataColumn()
            col.ColumnName = "Remarks_Required"
            col.DataType = System.Type.[GetType]("System.String")
            col.[ReadOnly] = False
            col.Unique = False
            dtItem.Columns.Add(col)


            col = New DataColumn()
            col.ColumnName = "Remarks"
            col.DataType = System.Type.[GetType]("System.String")
            col.[ReadOnly] = False
            col.Unique = False
            dtItem.Columns.Add(col)

        dtqids = dt.DefaultView.ToTable(True, "Question_ID")


        For Each qdr In dtqids.Rows
            Dim seldr() As DataRow
            seldr = dt.Select("Question_ID=" & qdr("Question_ID"))
            If seldr.Length > 0 Then
                Dim drnew As DataRow
                drnew = dtItem.NewRow
                drnew("QuestId") = seldr(0)("Question_ID")
                drnew("Question") = seldr(0)("Question_text")
                drnew("RespTypeId") = seldr(0)("Response_Type_ID")
                drnew("Restxt") = seldr(0)("Restext")
                    drnew("Editable") = seldr(0)("Editable")
                    drnew("Mandatory_On_Confirmation") = seldr(0)("Mandatory_On_Confirmation")
                    drnew("Remarks_Required") = seldr(0)("Remarks_Required")
                    drnew("Remarks") = seldr(0)("Remarks")

                dtItem.Rows.Add(drnew)
            End If
        Next
        gvResponse.DataSource = dtItem
        gvResponse.DataBind()
            bindControls(dt)
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        End Try
    End Sub
    Sub bindControls(dt As DataTable)
        Try

       
        For Each gr As GridViewRow In gvResponse.Rows
            Dim respType As String = "0"
            If Not CType(gr.FindControl("lbRespType"), Label) Is Nothing Then
                respType = CType(gr.FindControl("lbRespType"), Label).Text
                Dim lblReps As Label = DirectCast(gr.FindControl("lblReps"), Label)
                Dim lblQID As Label = DirectCast(gr.FindControl("lbQuestId"), Label)

                Dim Chk As CheckBoxList = DirectCast(gr.FindControl("Chk_resp"), CheckBoxList)
                Dim Rdo As RadioButtonList = DirectCast(gr.FindControl("Rdo_resp"), RadioButtonList)
                Dim lbRespType As Label = DirectCast(gr.FindControl("lbRespType"), Label)
                Dim txtRes As TextBox = DirectCast(gr.FindControl("txtResponse"), TextBox)
                Dim txtRes1 As TextBox = DirectCast(gr.FindControl("txtResponse1"), TextBox)
                Dim txtRes2 As TextBox = DirectCast(gr.FindControl("txtResponse2"), TextBox)
                Dim txtRes3 As TextBox = DirectCast(gr.FindControl("txtResponse3"), TextBox)
                Dim lbQuestId As Label = DirectCast(gr.FindControl("lbQuestId"), Label)

                Dim txtQuest1 As Label = DirectCast(gr.FindControl("txtQuest"), Label)
                Dim txtQuest2 As Label = DirectCast(gr.FindControl("txtQuest1"), Label)
                Dim txtQuest3 As Label = DirectCast(gr.FindControl("txtQuest2"), Label)
                Dim lblEnabled As Label = DirectCast(gr.FindControl("lblEnabled"), Label)
                    Dim lblremark As Label = DirectCast(gr.FindControl("lblremark"), Label)



                Dim raddate As RadDatePicker = DirectCast(gr.FindControl("txtFromDate"), RadDatePicker)
                Dim raddate1 As RadDatePicker = DirectCast(gr.FindControl("txtToDate"), RadDatePicker)




                If respType = "1" Then
                    txtQuest1.Visible = True
                    txtRes.Visible = True
                    txtRes.Text = lblReps.Text
                    Rdo.Visible = False
                    Chk.Visible = False
                    txtRes1.Visible = False
                    txtRes2.Visible = False
                    txtQuest2.Visible = False
                    raddate.Visible = False
                    raddate1.Visible = False
                    If lblEnabled.Text = 1 Then
                        txtRes.Enabled = True
                    Else
                        txtRes.Enabled = False
                    End If


                    Dim line() As DataRow
                    line = dt.Select("Question_ID=" & lblQID.Text & " and No_Of_Lines_For_Text>1")
                    If line.Count > 0 Then
                        txtRes.TextMode = TextBoxMode.MultiLine
                        'txtRes.TextMode.MultiLine.   = Convert.ToInt32(line(0)("No_Of_Lines_For_Text"))

                    End If


                    Dim remark_r() As DataRow
                    remark_r = dt.Select("Question_ID=" & lblQID.Text & " and Remarks_Required='1'")
                    If remark_r.Count > 0 Then
                            txtRes1.Visible = True
                            txtRes1.Text = lblremark.Text
                    End If

                    Dim line_r() As DataRow
                    line_r = dt.Select("Question_ID=" & lblQID.Text & " and Remarks_Required='1' and No_Of_Lines_For_Remarks>1")
                    If line_r.Count > 0 Then
                        txtRes1.TextMode = TextBoxMode.MultiLine
                    End If


                ElseIf respType = "2" Then
                    Dim seldr() As DataRow
                    seldr = dt.Select("Question_ID=" & lblQID.Text)
                    If seldr.Length > 0 Then
                        Dim optdt As New DataTable
                        optdt = seldr.CopyToDataTable
                        txtQuest1.Visible = True
                        txtRes.Visible = False
                        Rdo.Visible = True
                        Chk.Visible = False
                        txtRes.Visible = False
                        txtRes1.Visible = False
                        txtRes2.Visible = False
                        txtQuest2.Visible = False
                        txtQuest3.Visible = False
                        raddate.Visible = False
                        raddate1.Visible = False
                        Rdo.DataValueField = "Response_ID"
                        Rdo.DataTextField = "Responsetext"
                        Rdo.DataSource = optdt
                        Rdo.DataBind()


                        Dim seldrDefault() As DataRow
                        seldrDefault = dt.Select("Question_ID=" & lblQID.Text & " and IsDefault=1")
                        If seldrDefault.Length > 0 Then
                            For Each itm As ListItem In Rdo.Items
                                If itm.Value = seldrDefault(0)("Response_ID") Then
                                    itm.Selected = True
                                End If
                            Next
                        End If


                        Dim selected() As DataRow
                        selected = dt.Select("Question_ID=" & lblQID.Text & " and Responsetext='" & lblReps.Text & "'")
                        If selected.Length > 0 Then
                            For Each itm As ListItem In Rdo.Items
                                If itm.Value = selected(0)("Response_ID") Then
                                    itm.Selected = True
                                End If
                            Next
                        End If

                        If lblEnabled.Text = 1 Then
                            Rdo.Enabled = True
                        Else
                            Rdo.Enabled = False
                        End If

                        Dim remark_r() As DataRow
                        remark_r = dt.Select("Question_ID=" & lblQID.Text & " and Remarks_Required='1'")
                        If remark_r.Count > 0 Then
                                txtRes1.Visible = True
                                txtRes1.Text = lblremark.Text
                        End If

                        Dim line_r() As DataRow
                        line_r = dt.Select("Question_ID=" & lblQID.Text & " and Remarks_Required='1' and No_Of_Lines_For_Remarks>1")
                        If line_r.Count > 0 Then
                            txtRes1.TextMode = TextBoxMode.MultiLine
                        End If


                    End If
                ElseIf respType = "3" Then
                    Dim seldr() As DataRow
                    seldr = dt.Select("Question_ID=" & lblQID.Text)
                    If seldr.Length > 0 Then
                        Dim optdt As New DataTable
                        optdt = seldr.CopyToDataTable
                        txtQuest1.Visible = True
                        txtRes.Visible = False
                        Rdo.Visible = False
                        Chk.Visible = True
                        txtRes.Visible = False
                        txtRes1.Visible = False
                        txtRes2.Visible = False
                        txtQuest2.Visible = False
                        txtQuest3.Visible = False
                        raddate.Visible = False
                        raddate1.Visible = False
                        Chk.DataValueField = "Response_ID"
                        Chk.DataTextField = "Responsetext"
                        Chk.DataSource = optdt
                        Chk.DataBind()

                        Dim seldrDefault() As DataRow
                        seldrDefault = dt.Select("Question_ID=" & lblQID.Text & " and IsDefault=1")
                        If seldrDefault.Length > 0 Then
                            For Each itm As ListItem In Chk.Items
                                If itm.Value = seldrDefault(0)("Response_ID") Then
                                    itm.Selected = True
                                End If
                            Next
                        End If
                        If lblEnabled.Text = 1 Then
                            Chk.Enabled = True
                        Else
                            Chk.Enabled = False
                        End If
                    End If

                ElseIf respType = "6" Or respType = "20" Then
                    txtQuest1.Visible = True
                    txtRes.Visible = False
                    Rdo.Visible = False
                    Chk.Visible = False
                    txtRes1.Visible = False
                    txtRes2.Visible = False
                    txtQuest3.Visible = False
                    txtQuest2.Visible = False
                    raddate.Visible = True
                    raddate1.Visible = False
                    If IsDate(lblReps.Text) Then
                        raddate.SelectedDate = CDate(lblReps.Text)
                    Else
                        raddate.SelectedDate = Now.Date
                    End If
                    If lblEnabled.Text = 1 Then
                        raddate.Enabled = True
                    Else
                        raddate.Enabled = False
                    End If
                ElseIf respType = "5" Then
                    Dim dates() As String
                    dates = lblReps.Text.Split("To")
                    Dim sdate As String = ""
                    Dim Edate As String = ""
                    txtQuest1.Visible = True
                    txtRes.Visible = False
                    Rdo.Visible = False
                    Chk.Visible = False
                    txtRes1.Visible = False
                    txtRes2.Visible = False
                    txtQuest3.Visible = False
                    txtQuest2.Visible = False
                    raddate.Visible = True
                    raddate1.Visible = True
                    If dates.Length > 0 Then
                        sdate = dates(0)
                    End If
                    If dates.Length > 1 Then
                        Edate = dates(1)
                    End If
                    If IsDate(sdate) Then
                        raddate.SelectedDate = CDate(sdate)
                    Else
                        raddate.SelectedDate = Now.Date
                    End If
                    If IsDate(Edate) Then
                        raddate1.SelectedDate = CDate(Edate)
                    Else
                        raddate1.SelectedDate = Now.Date
                    End If
                    If lblEnabled.Text = 1 Then
                        raddate.Enabled = True
                        raddate1.Enabled = True
                    Else
                        raddate.Enabled = False
                        raddate1.Enabled = False
                    End If
                ElseIf respType = "7" Or respType = "16" Or respType = "21" Then
                    txtQuest1.Visible = True
                    txtRes.Visible = True
                    txtRes.Text = lblReps.Text
                    txtRes.Attributes.Add("Onkeypress", "javascript:return NumericOnly(event)")
                    txtRes.Width = 200
                    txtQuest1.Visible = True
                    Rdo.Visible = False
                    Chk.Visible = False
                    txtRes1.Visible = False
                    txtRes2.Visible = False
                    txtQuest3.Visible = False
                    txtQuest2.Visible = False
                    raddate.Visible = False
                    raddate1.Visible = False
                    If lblEnabled.Text = 1 Then
                        txtRes.Enabled = True
                    Else
                        txtRes.Enabled = False
                    End If


                    Dim remark_r() As DataRow
                    remark_r = dt.Select("Question_ID=" & lblQID.Text & " and Remarks_Required='1'")
                    If remark_r.Count > 0 Then
                            txtRes1.Visible = True
                            txtRes1.Text = lblremark.Text
                    End If

                    Dim line_r() As DataRow
                    line_r = dt.Select("Question_ID=" & lblQID.Text & " and Remarks_Required='1' and No_Of_Lines_For_Remarks>1")
                    If line_r.Count > 0 Then
                        txtRes1.TextMode = TextBoxMode.MultiLine

                    End If
                ElseIf respType = "8" Then
                    txtQuest1.Visible = True
                    txtRes.Visible = True
                    txtRes.Text = lblReps.Text
                    txtRes.Attributes.Add("Onkeypress", "return IntegerOnly(event)")
                    txtRes.Width = 100
                    Rdo.Visible = False
                    Chk.Visible = False
                    txtRes1.Visible = False
                    txtRes2.Visible = False
                    txtQuest3.Visible = False
                    txtQuest2.Visible = False
                    raddate.Visible = False
                    raddate1.Visible = False
                    If lblEnabled.Text = 1 Then
                        txtRes.Enabled = True
                    Else
                        txtRes.Enabled = False
                    End If
                    Dim remark_r() As DataRow
                    remark_r = dt.Select("Question_ID=" & lblQID.Text & " and Remarks_Required='1'")
                    If remark_r.Count > 0 Then
                            txtRes1.Visible = True
                            txtRes1.Text = lblremark.Text
                    End If

                    Dim line_r() As DataRow
                    line_r = dt.Select("Question_ID=" & lblQID.Text & " and Remarks_Required='1' and No_Of_Lines_For_Remarks>1")
                    If line_r.Count > 0 Then
                        txtRes1.TextMode = TextBoxMode.MultiLine

                    End If
                ElseIf respType = "9" Then
                    Dim Vals() As String
                    Vals = lblReps.Text.Split(",")
                    txtQuest1.Visible = True
                    txtRes.Width = 100
                    txtRes1.Width = 100
                    txtRes2.Width = 100
                    txtRes.Visible = True
                    txtQuest1.Visible = True
                    Rdo.Visible = False
                    Chk.Visible = False
                    txtRes1.Visible = True
                    txtRes2.Visible = True
                    txtQuest3.Visible = False
                    txtQuest2.Visible = False
                    raddate.Visible = False
                    raddate1.Visible = False
                    If Vals.Length > 0 Then
                        txtRes.Text = Vals(0)
                    End If
                    If Vals.Length > 1 Then
                        txtRes1.Text = Vals(1)
                    End If
                    If Vals.Length > 2 Then
                        txtRes2.Text = Vals(2)
                    End If
                    If lblEnabled.Text = 1 Then
                        txtRes.Enabled = True
                        txtRes1.Enabled = True
                        txtRes2.Enabled = True
                    Else
                        txtRes.Enabled = False
                        txtRes1.Enabled = False
                        txtRes2.Enabled = False
                    End If

                ElseIf respType = "18" Or respType = "19" Then

                    txtQuest1.Visible = True
                    txtRes.Visible = True
                    txtRes.Text = lblReps.Text
                    Rdo.Visible = False
                    Chk.Visible = False
                    txtRes1.Visible = False
                    txtRes2.Visible = False
                    txtQuest2.Visible = False
                    raddate.Visible = False
                    raddate1.Visible = False
                    txtRes.Enabled = False

                        If lblEnabled.Text = 1 Then
                            txtRes.Enabled = True
                        Else
                            txtRes.Enabled = False
                        End If

                End If


            End If
            Next
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        End Try
    End Sub
    

   



    Protected Sub SearchBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SearchBtn.Click
        Try
            If Me.ddlVan.SelectedIndex > 0 Then

                LoadDetails()

            Else
                MessageBoxValidation("Please select a van", "Information")

                Exit Sub
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        End Try
    End Sub

    'Protected Sub ddlDivision_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
    '    LoadDetails()
    'End Sub

    Sub LoadDetails()
        Try
            ObjCommon = New SalesWorx.BO.Common.Common()
            Dim SubQry As String = " A.SalesRep_ID='" + ddlVan.SelectedValue + "'"
            Dim dt As New DataTable
            dt = ObjCommon.GetVanAuditReport(Err_No, Err_Desc, ddlVan.SelectedValue)

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
                    vandetails.Visible = True
                    NoSurvey.Visible = False
                    SalesPersonName = dt.Rows(0)("Emp_Name").ToString() & "-" & dt.Rows(0)("Emp_Code").ToString()
                    DateOfAudit = Convert.ToDateTime(dt.Rows(0)("Survey_Timestamp")).ToString("dd/MM/yyyy")
                    Division_SalesOrgID = dt.Rows(0)("Site").ToString()
                    OrgID = dt.Rows(0)("Sales_ORG_ID").ToString()
                    'PrevAuditDAte = ObjCommon.GetPrevAuditDate(Err_No, Err_Desc, dt.Rows(0)("Survey_Timestamp").ToString(), dt.Rows(0)("SalesRep_ID"))
                    CheckedBy = dt.Rows(0)("UserName").ToString()
                    Me.txtSurveyId.Text = dt.Rows(0)("Survey_Id").ToString()
                    Me.txtSalesRepid.Text = dt.Rows(0)("SalesRep_Id").ToString()
                    Me.txtEmpCode.Text = dt.Rows(0)("Emp_Code").ToString()
                    txtAVanName.Text = dt.Rows(0)("SalesRepName").ToString()
                    Me.txtSurveyTime.Text = dt.Rows(0)("Survey_Timestamp").ToString()


                    lbl_van.Text = dt.Rows(0)("Emp_Name").ToString() & "-" & dt.Rows(0)("Emp_Code").ToString()
                    lbl_surveyedat.Text = Convert.ToDateTime(dt.Rows(0)("Survey_Timestamp")).ToString("dd/MM/yyyy hh:mm")
                    lbl_surveyedBy.Text = dt.Rows(0)("UserName").ToString()
                    lbl_Site.Text = dt.Rows(0)("Site").ToString()
                Else
                    vandetails.Visible = False
                    NoSurvey.Visible = True
                End If
            Else
                vandetails.Visible = False
                NoSurvey.Visible = True
            End If


            'Dim sb As New StringBuilder("")
            'sb.Append("<table width='100%'  border='1' style='font-weight: bold; font-size: 11px; border:1px solid;border-color: #FFFFFF' cellpadding='6'  cellspacing = '0' > ")
            'sb.Append(HeaderTemplate.Replace("$INFO$", "Name of Salesman : " & SalesPersonName))
            'sb.Append(HeaderTemplate.Replace("$INFO$", "Organization: " & Division_SalesOrgID))
            'sb.Append("</table>")
            'sb.Append("<div height='15'>&nbsp; </div>")
            'sb.Append("<table width='100%'  border='1' style='font-weight: bold;font-size: 11px; border:1px solid;border-color: #FFFFFF' cellpadding='6'  cellspacing = '0' > ")
            'sb.Append(AuditTemplate.Replace("$INFO$", "Date of Audit").Replace("$ANS$", DateOfAudit))
            'sb.Append(AuditTemplate.Replace("$INFO$", "Date of previous audit").Replace("$ANS$", PrevAuditDAte))
            'sb.Append("</table>")
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

            'DivHTML = sb.ToString()

            '' Literal1.Text = sb.ToString()

            'ObjCommon = New SalesWorx.BO.Common.Common()
            ''  Dim SubQry As String = "A.SalesRep_ID='" + ddlVan.SelectedValue + "'"
            'gvResponse.DataSource = dt
            'gvResponse.DataBind()
            If Not IsNothing(dt) Then
                If dt.Rows.Count > 0 Then
                    BindDetails(dt)
                Else
                    Me.gvResponse.DataSource = Nothing
                    Me.gvResponse.DataBind()

                End If
            Else
                Me.gvResponse.DataSource = Nothing
                Me.gvResponse.DataBind()
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        End Try
    End Sub


    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click

        ObjCommon = New SalesWorx.BO.Common.Common()
        If Me.ddlVan.SelectedIndex > 0 And gvResponse.Rows.Count > 0 Then

            If ddlVan.SelectedItem.Text <> txtAVanName.Text Then
                MessageBoxValidation("The data currenlty loaded for '" & txtAVanName.Text & "'. Please select a proper van.", "Information")
                Exit Sub
            End If
            Try
                Err_No = Nothing
                Err_Desc = Nothing
                Dim success As Boolean

                Dim dtAns As New DataTable
                Dim col As DataColumn

                col = New DataColumn()
                col.ColumnName = "QuestId"
                col.DataType = System.Type.[GetType]("System.String")
                col.[ReadOnly] = False
                col.Unique = False
                dtAns.Columns.Add(col)

                col = New DataColumn()
                col.ColumnName = "Question"
                col.DataType = System.Type.[GetType]("System.String")
                col.[ReadOnly] = False
                col.Unique = False
                dtAns.Columns.Add(col)



                col = New DataColumn()
                col.ColumnName = "RespId"
                col.DataType = System.Type.[GetType]("System.String")
                col.[ReadOnly] = False
                col.Unique = False
                dtAns.Columns.Add(col)


                col = New DataColumn()
                col.ColumnName = "Response"
                col.DataType = System.Type.[GetType]("System.String")
                col.[ReadOnly] = False
                col.Unique = False
                dtAns.Columns.Add(col)


                col = New DataColumn()
                col.ColumnName = "ISDefault"
                col.DataType = System.Type.[GetType]("System.Boolean")
                col.[ReadOnly] = False
                col.Unique = False
                dtAns.Columns.Add(col)

                col = New DataColumn()
                col.ColumnName = "RespTypeId"
                col.DataType = System.Type.[GetType]("System.String")
                col.[ReadOnly] = False
                col.Unique = False
                dtAns.Columns.Add(col)


                col = New DataColumn()
                col.ColumnName = "Restxt"
                col.DataType = System.Type.[GetType]("System.String")
                col.[ReadOnly] = False
                col.Unique = False
                dtAns.Columns.Add(col)

                col = New DataColumn()
                col.ColumnName = "Editable"
                col.DataType = System.Type.[GetType]("System.String")
                col.[ReadOnly] = False
                col.Unique = False
                dtAns.Columns.Add(col)

                col = New DataColumn()
                col.ColumnName = "Mandatory_On_Confirmation"
                col.DataType = System.Type.[GetType]("System.String")
                col.[ReadOnly] = False
                col.Unique = False
                dtAns.Columns.Add(col)

                col = New DataColumn()
                col.ColumnName = "Remarks"
                col.DataType = System.Type.[GetType]("System.String")
                col.[ReadOnly] = False
                col.Unique = False
                dtAns.Columns.Add(col)


                For Each gr As GridViewRow In gvResponse.Rows
                    Dim respType As String = "0"
                    Dim dr As DataRow
                    dr = dtAns.NewRow
                    If Not CType(gr.FindControl("lbRespType"), Label) Is Nothing Then
                        Dim Answered As Boolean = False

                        respType = CType(gr.FindControl("lbRespType"), Label).Text
                        Dim lblReps As Label = DirectCast(gr.FindControl("lblReps"), Label)
                        Dim lblQID As Label = DirectCast(gr.FindControl("lbQuestId"), Label)

                        Dim lblMand_Confirm As Label = DirectCast(gr.FindControl("lblMand_Confirm"), Label)
                        Dim txtQuest As Label = DirectCast(gr.FindControl("txtQuest"), Label)

                        Dim Chk As CheckBoxList = DirectCast(gr.FindControl("Chk_resp"), CheckBoxList)
                        Dim Rdo As RadioButtonList = DirectCast(gr.FindControl("Rdo_resp"), RadioButtonList)
                        Dim lbRespType As Label = DirectCast(gr.FindControl("lbRespType"), Label)
                        Dim txtRes As TextBox = DirectCast(gr.FindControl("txtResponse"), TextBox)
                        Dim txtRes1 As TextBox = DirectCast(gr.FindControl("txtResponse1"), TextBox)
                        Dim txtRes2 As TextBox = DirectCast(gr.FindControl("txtResponse2"), TextBox)
                        Dim lbQuestId As Label = DirectCast(gr.FindControl("lbQuestId"), Label)

                        Dim txtQuest1 As Label = DirectCast(gr.FindControl("txtQuest"), Label)
                        Dim txtQuest2 As Label = DirectCast(gr.FindControl("txtQuest1"), Label)
                        Dim txtQuest3 As Label = DirectCast(gr.FindControl("txtQuest2"), Label)
                        Dim lblEnabled As Label = DirectCast(gr.FindControl("lblEnabled"), Label)

                        Dim raddate As RadDatePicker = DirectCast(gr.FindControl("txtFromDate"), RadDatePicker)
                        Dim raddate1 As RadDatePicker = DirectCast(gr.FindControl("txtToDate"), RadDatePicker)


                        Dim lblremark_requ As Label = DirectCast(gr.FindControl("lblremark_requ"), Label)


                        If lblMand_Confirm.Text.Trim() = "1" Then


                            '*************************
                            If respType = "1" Then
                                If (txtRes.Text.Trim().Length = 0 Or txtRes.Text = "") Then
                                    MessageBoxValidation("""" & txtQuest.Text & """   Is Mandatory ", "Information")
                                    Exit Sub
                                End If
                            ElseIf respType = "2" Then
                                If Rdo.SelectedIndex < 0 Then
                                    MessageBoxValidation("""" & txtQuest.Text & """   Is Mandatory ", "Information")
                                    Exit Sub
                                End If
                            ElseIf respType = "3" Then
                                If Chk.SelectedIndex < 0 Then
                                    MessageBoxValidation("""" & txtQuest.Text & """   Is Mandatory ", "Information")
                                    Exit Sub
                                End If
                            ElseIf respType = "6" Or respType = "20" Then
                                If raddate.SelectedDate Is Nothing Then
                                    MessageBoxValidation("""" & txtQuest.Text & """   Is Mandatory ", "Information")
                                    Exit Sub
                                End If
                            ElseIf respType = "5" Then
                                If raddate1.SelectedDate Is Nothing Then
                                    MessageBoxValidation("""" & txtQuest.Text & """   Is Mandatory ", "Information")
                                    Exit Sub
                                End If
                            ElseIf respType = "7" Or respType = "16" Or respType = "21" Then
                                If (txtRes.Text.Trim().Length = 0 Or txtRes.Text = "") Then
                                    MessageBoxValidation("""" & txtQuest.Text & """   Is Mandatory ", "Information")
                                    Exit Sub
                                End If
                            ElseIf respType = "8" Then
                                If (txtRes.Text.Trim().Length = 0 Or txtRes.Text = "") Then
                                    MessageBoxValidation("""" & txtQuest.Text & """   Is Mandatory ", "Information")
                                    Exit Sub
                                End If
                            ElseIf respType = "9" Then
                                If (txtRes.Text.Trim().Length = 0 Or txtRes.Text = "") Then
                                    MessageBoxValidation("""" & txtQuest.Text & """   Is Mandatory ", "Information")
                                    Exit Sub
                                End If
                            ElseIf respType = "18" Or respType = "19" Then
                                If (txtRes.Text.Trim().Length = 0 Or txtRes.Text = "") Then
                                    MessageBoxValidation("""" & txtQuest.Text & """   Is Mandatory ", "Information")
                                    Exit Sub
                                End If

                            End If


                            '*************************

                        End If





                        dr("QuestId") = lblQID.Text
                        dr("RespTypeId") = respType
                        If respType = "1" Or respType = "18" Or respType = "19" Then
                            dr("Response") = txtRes.Text
                            dr("RespId") = "0"
                            dr("Remarks") = txtRes1.Text
                        ElseIf respType = "2" Then
                            dr("Response") = ""
                            If Rdo.SelectedIndex >= 0 Then
                              
                                dr("RespId") = Rdo.SelectedValue
                                dr("Response") = ObjCommon.GetResponsesTextByID(Rdo.SelectedValue) ' 
                            Else
                                dr("RespId") = 0
                            End If
                            dr("Remarks") = txtRes1.Text
                        ElseIf respType = "3" Then
                            dr("Response") = ""
                            If Chk.SelectedIndex >= 0 Then
                                dr("RespId") = Chk.SelectedValue
                            Else
                                dr("RespId") = 0
                            End If
                            dr("Remarks") = txtRes1.Text
                        ElseIf respType = "6" Then
                            Dim s As String = ""
                            If Not raddate.SelectedDate Is Nothing Then
                                s = CDate(raddate.SelectedDate)
                            End If
                            dr("Response") = s
                        ElseIf respType = "5" Then
                            Dim s As String = ""
                            If Not raddate.SelectedDate Is Nothing Then
                                s = CDate(raddate.SelectedDate) & " To "
                            End If
                            If Not raddate1.SelectedDate Is Nothing Then
                                s = s & CDate(raddate1.SelectedDate)
                            End If
                            dr("Response") = s
                        ElseIf respType = "7" Or respType = "16" Or respType = "21" Then
                            dr("Response") = txtRes.Text
                            dr("RespId") = "0"
                            dr("Remarks") = txtRes1.Text
                        ElseIf respType = "8" Then
                            dr("Response") = txtRes.Text
                            dr("RespId") = "0"
                            dr("Remarks") = txtRes1.Text
                        ElseIf respType = "9" Then
                            Dim s As String = ""
                            s = txtRes.Text & "," & txtRes1.Text & "," & txtRes2.Text
                            dr("Response") = s
                        End If
                        dtAns.Rows.Add(dr)
                    End If
                Next
                success = ObjSurvey.SaveVanAudit(Err_No, Err_Desc, Me.txtEmpCode.Text, Me.txtSalesRepid.Text, Me.txtSurveyId.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), dtAns, Me.txtSurveyTime.Text)

                If success = True Then
                    MessageBoxValidation("Successfully updated", "Information")

                Else
                    MessageBoxValidation("Error while updating survey", "Information")

                    Exit Sub
                End If

            Catch ex As Exception
                log.Error(ex.Message)
                Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_CashVanAudit_006") & "&next=EdiCashVanAudit.aspx&Title=CashVanAudit", False)
            End Try
        Else
            MessageBoxValidation("Please select a van with an audit survey initialised", "Information")

            Exit Sub
        End If

    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConfirm.Click
        If Me.ddlVan.SelectedIndex > 0 Then

            If gvResponse.Rows.Count > 0 Then

                If ddlVan.SelectedItem.Text <> txtAVanName.Text Then

                    MessageBoxValidation("The data currently loaded for '" & txtAVanName.Text & "'. Please select a proper van.", "Information")
                    Exit Sub

                End If



                



            '  Dim s As String() = ddlVan.SelectedItem.Text.Split("-")
            Dim VanID As String = ddlVan.SelectedValue
            '  If s.Length > 1 Then
            'VanID = s(1)
            '  End If

            For Each gr As GridViewRow In gvResponse.Rows
                Dim respType As String = "0"

                Dim lbQuestId As Label = DirectCast(gr.FindControl("lbQuestId"), Label)
                Dim lblMand_Confirm As Label = DirectCast(gr.FindControl("lblMand_Confirm"), Label)
                Dim txtQuest As Label = DirectCast(gr.FindControl("txtQuest"), Label)
                Dim lblReps As Label = DirectCast(gr.FindControl("lblReps"), Label)
                respType = CType(gr.FindControl("lbRespType"), Label).Text
                Dim lblQID As Label = DirectCast(gr.FindControl("lbQuestId"), Label)

                Dim Chk As CheckBoxList = DirectCast(gr.FindControl("Chk_resp"), CheckBoxList)
                Dim Rdo As RadioButtonList = DirectCast(gr.FindControl("Rdo_resp"), RadioButtonList)
                Dim lbRespType As Label = DirectCast(gr.FindControl("lbRespType"), Label)
                Dim txtRes As TextBox = DirectCast(gr.FindControl("txtResponse"), TextBox)
                Dim txtRes1 As TextBox = DirectCast(gr.FindControl("txtResponse1"), TextBox)
                Dim txtRes2 As TextBox = DirectCast(gr.FindControl("txtResponse2"), TextBox)


                Dim txtQuest1 As Label = DirectCast(gr.FindControl("txtQuest"), Label)
                Dim txtQuest2 As Label = DirectCast(gr.FindControl("txtQuest1"), Label)
                Dim txtQuest3 As Label = DirectCast(gr.FindControl("txtQuest2"), Label)
                Dim lblEnabled As Label = DirectCast(gr.FindControl("lblEnabled"), Label)

                Dim raddate As RadDatePicker = DirectCast(gr.FindControl("txtFromDate"), RadDatePicker)
                Dim raddate1 As RadDatePicker = DirectCast(gr.FindControl("txtToDate"), RadDatePicker)



                If lblMand_Confirm.Text.Trim() = "1" Then






                    '*************************
                    If respType = "1" Then
                        If (txtRes.Text.Trim().Length = 0 Or txtRes.Text = "") Then
                            MessageBoxValidation("""" & txtQuest.Text & """   Is Mandatory ", "Information")
                            Exit Sub
                        End If
                    ElseIf respType = "2" Then
                        If Rdo.SelectedIndex < 0 Then
                            MessageBoxValidation("""" & txtQuest.Text & """   Is Mandatory ", "Information")
                            Exit Sub
                        End If
                    ElseIf respType = "3" Then
                        If Chk.SelectedIndex < 0 Then
                            MessageBoxValidation("""" & txtQuest.Text & """   Is Mandatory ", "Information")
                            Exit Sub
                        End If
                    ElseIf respType = "6" Or respType = "20" Then
                        If raddate.SelectedDate Is Nothing Then
                            MessageBoxValidation("""" & txtQuest.Text & """   Is Mandatory ", "Information")
                            Exit Sub
                        End If
                    ElseIf respType = "5" Then
                        If raddate1.SelectedDate Is Nothing Then
                            MessageBoxValidation("""" & txtQuest.Text & """   Is Mandatory ", "Information")
                            Exit Sub
                        End If
                    ElseIf respType = "7" Or respType = "16" Or respType = "21" Then
                        If (txtRes.Text.Trim().Length = 0 Or txtRes.Text = "") Then
                            MessageBoxValidation("""" & txtQuest.Text & """   Is Mandatory ", "Information")
                            Exit Sub
                        End If
                    ElseIf respType = "8" Then
                        If (txtRes.Text.Trim().Length = 0 Or txtRes.Text = "") Then
                            MessageBoxValidation("""" & txtQuest.Text & """   Is Mandatory ", "Information")
                            Exit Sub
                        End If
                    ElseIf respType = "9" Then
                        If (txtRes.Text.Trim().Length = 0 Or txtRes.Text = "") Then
                            MessageBoxValidation("""" & txtQuest.Text & """   Is Mandatory ", "Information")
                            Exit Sub
                        End If
                    ElseIf respType = "18" Or respType = "19" Then
                        If (txtRes.Text.Trim().Length = 0 Or txtRes.Text = "") Then
                            MessageBoxValidation("""" & txtQuest.Text & """   Is Mandatory ", "Information")
                            Exit Sub
                        End If

                    End If


                    '*************************





                End If
            Next

            Try
                Err_No = Nothing
                Err_Desc = Nothing
                Dim success As Boolean
                success = ObjSurvey.ConfirmVanAudit(Err_No, Err_Desc, txtSalesRepid.Text, txtSurveyId.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), Me.txtSurveyTime.Text)
                objLogin.SaveUserLog(Err_No, Err_Desc, "A", "SURVEY", "MODIFY AUDIT SURVEY", VanID.Trim(), "SurveyID: " & txtSurveyId.Text & "/Audit Date: " & DateOfAudit & "/ Survey Times : " & Me.txtSurveyTime.Text & "/ Checked By : " & CheckedBy & " Sales Rep: " & SalesPersonName & "/ Status: Confirmed", CType(Session("User_Access"), UserAccess).UserID.ToString(), VanID.Trim(), OrgID)
                If success = True Then

                    MessageBoxValidation("Successfully confirmed", "Information")
                    Me.ddlVan.SelectedIndex = 0
                    txtSalesRepid.Text = ""
                    txtSurveyId.Text = ""
                    txtAVanName.Text = ""
                    txtEmpCode.Text = ""
                    gvResponse.DataSource = Nothing
                    gvResponse.DataBind()
                    vandetails.Visible = False

                Else
                    MessageBoxValidation("Error while confirm survey", "Information")
                    Exit Sub
                End If

            Catch ex As Exception
                log.Error(ex.Message)
                Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_CashVanAudit_006") & "&next=EdiCashVanAudit.aspx&Title=CashVanAudit", False)
            End Try
        Else
            MessageBoxValidation("No unconfirmed Surevy to confirm", "Information")
            Exit Sub
        End If
        Else
        MessageBoxValidation("Please select a van", "Information")

        Exit Sub

        End If

    End Sub

   
End Class