Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports System.Collections.Generic
Imports Telerik.Web.UI
Imports Telerik.Charting

Partial Public Class RepSurveyStatistics
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim objSurvey As Survey
    'Dim SortField As String = ""
    'Dim SortFieldDtl As String = ""
    Public DisplayHTML As String
    Private Const PageID As String = "P211"
    Private ReportPath As String = "SurveyStatistics"
    Dim ObjCommon As SalesWorx.BO.Common.Common

    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)




    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
            Exit Sub
        End If
        If Not IsPostBack Then
            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            If Not HasPermission Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If
            ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
            Try

                ObjCommon = New SalesWorx.BO.Common.Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))
                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                End If

                LoadSurvey()

                If Request.QueryString("b") IsNot Nothing Then
                    If Session("O") IsNot Nothing AndAlso Session("S") IsNot Nothing Then
                        ddlOrganization.SelectedValue = Session("O")
                        ddlSurvey.SelectedValue = Session("S")

                        Session("O") = Nothing
                        Session("S") = Nothing

                        LoadReports()



                    End If
                End If

                If Not Err_Desc Is Nothing Then
                    log.Error(Err_Desc)
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
                End If
            Catch ex As Exception
                Err_No = "74066"
                If Err_Desc Is Nothing Then
                    log.Error(GetExceptionInfo(ex))
                Else
                    log.Error(Err_Desc)
                End If
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
            Finally

                ErrorResource = Nothing
            End Try

            gvRep.MasterTableView.ExpandCollapseColumn.Visible = False

        End If
    End Sub
    Private Sub LoadSurvey()
        Try
            Session("SurveyTbl") = Nothing
            Dim SearchQuery As String = ""
            objSurvey = New Survey()
            'SearchQuery = " And Survey_Type_Code='" & ddlTypeCode.SelectedValue & "'"
            SearchQuery = " And Survey_Type_Code IN ('N','A')"
            Dim dtSurvey As DataTable = objSurvey.GetAllSurvey(Err_No, Err_Desc, SearchQuery)
            ddlSurvey.DataSource = dtSurvey
            Session("SurveyTbl") = dtSurvey
            ddlSurvey.DataBind()
            ddlSurvey.Items.Insert(0, New RadComboBoxItem("Select Survey", "0"))
            lblStartDateval.Text = ""
            lblEndDateval.Text = ""
            objSurvey = Nothing
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub



    Private Sub BtnExport_Click(sender As Object, e As EventArgs) Handles BtnExportExcel.Click
        Try
            If ValidateInputs() Then
                Export("Excel")
            End If
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub
    Private Sub BtnPDF_Click(sender As Object, e As EventArgs) Handles BtnExportPDF.Click
        Try
            If ValidateInputs() Then
                Export("PDF")
            End If
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub

    Function BuildQuery() As String
        Dim SearchQuery As String = ""
        If ddlSurvey.SelectedValue <> "0" Then
            SearchQuery = ddlSurvey.SelectedValue
        End If
        Return SearchQuery
    End Function

    Sub Export(format As String)

        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

        Dim SID As New ReportParameter
        SID = New ReportParameter("SurveyID", CStr(IIf(Me.ddlSurvey.SelectedIndex = 0, "ALL", ddlSurvey.SelectedValue)))

        Dim OrgID As New ReportParameter
        OrgID = New ReportParameter("OrgID", ddlOrganization.SelectedItem.Value)

        Dim OrgName As New ReportParameter
        OrgName = New ReportParameter("OrgName", ddlOrganization.SelectedItem.Text)

        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)

        rview.ServerReport.SetParameters(New ReportParameter() {SID, OrgID, OrgName})

        Dim mimeType As String = Nothing
        Dim encoding As String = Nothing
        Dim extension As String = Nothing
        Dim deviceInfo As String = "<DeviceInfo></DeviceInfo>"
        Dim streamids As String() = Nothing
        Dim warnings As Microsoft.Reporting.WebForms.Warning() = Nothing

        Dim bytes As Byte() = rview.ServerReport.Render(format, deviceInfo, mimeType, encoding, extension, streamids, warnings)


        Response.Clear()
        If format = "PDF" Then
            Response.ContentType = "application/pdf"
            Response.AddHeader("Content-disposition", "attachment;filename=SurveyStatistics.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=SurveyStatistics.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()

    End Sub
    Private Sub BindReport()
        Dim ObjReport As New SalesWorx.BO.Common.Reports
        Try
            Session("SurveyQuesTbl") = Nothing
            If Not ddlOrganization.SelectedItem Is Nothing Then
                Dim SearchQry As String

                SearchQry = BuildQuery()
                lbl_org.Text = ddlOrganization.SelectedItem.Text
                lbl_Survey.Text = ddlSurvey.SelectedItem.Text
                Args.Visible = True

                If Session("SurveyTbl") IsNot Nothing Then
                    Dim Surveydt As DataTable = CType(Session("SurveyTbl"), DataTable)
                    If Surveydt IsNot Nothing AndAlso Surveydt.Rows.Count > 0 Then
                        Dim SelRow() As DataRow = Surveydt.Select("Survey_ID=" & ddlSurvey.SelectedValue & "")
                        If SelRow.Count > 0 Then
                            lbl_startDate.Text = CDate(SelRow(0)("Start_Time")).ToString("dd-MMM-yyyy")
                            lbl_EndDate.Text = CDate(SelRow(0)("End_Time")).ToString("dd-MMM-yyyy")
                            hfSurveyType.Value = SelRow(0)("Survey_Type_Code")
                        End If
                    End If

                End If

                rpbFilter.Items(0).Expanded = False

                Dim dt As New DataTable
                dt = ObjReport.GetSurveyStatistics(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, ddlSurvey.SelectedValue)
                Details.Visible = True

                lbl_Survey1.Text = ddlSurvey.Text
                If dt.Rows.Count > 0 Then
                    lbl_Count.Text = dt.Rows(0)("SurveyCount")
                End If


                Session("SurveyQuesTbl") = dt

                '' Taking distinct Question
                Dim ColumnNames(3) As String
                ColumnNames = {"QuestionID", "QuestionText", "ResponseTypeID"}

                Dim surTbl As DataTable = dt.DefaultView.ToTable(True, ColumnNames)

                If surTbl.Rows.Count > 0 Then
                    gvRep.DataSource = surTbl.Select("ResponseTypeID <> 1 ", "QuestionID ASC").CopyToDataTable()
                Else
                    gvRep.DataSource = surTbl
                End If

                gvRep.DataBind()

            End If
        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            ObjReport = Nothing
        End Try

    End Sub

    Private Sub gvRep_ColumnCreated(sender As Object, e As GridColumnCreatedEventArgs) Handles gvRep.ColumnCreated
        '' For Hiding Expand Colapse
        If TypeOf e.Column Is GridGroupSplitterColumn Then
            e.Column.HeaderStyle.Width = Unit.Pixel(1)
            e.Column.HeaderStyle.Font.Size = FontUnit.Point(1)
            e.Column.ItemStyle.Width = Unit.Pixel(1)
            e.Column.ItemStyle.Font.Size = FontUnit.Point(1)
            e.Column.Resizable = False
        End If
        '' For Hiding Expand Colapse end
    End Sub

    Private Sub gvRep_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles gvRep.ItemDataBound
        ''If TypeOf e.Item Is GridGroupHeaderItem Then
        ''    Dim item As GridGroupHeaderItem = CType(e.Item, GridGroupHeaderItem)
        ''    Dim groupDataRow As DataRowView = CType(e.Item.DataItem, DataRowView)
        ''    item.DataCell.Text = String.Format("{0} &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Surveyed At:&nbsp {1}", item.DataCell.Text,
        ''                                        CDate(groupDataRow("Survey_Timestamp")).ToString("dd-MMM-yyyy hh:mm tt"))
        ''End If
        Dim ObjReport As New SalesWorx.BO.Common.Reports
        Try

            If TypeOf e.Item Is GridGroupHeaderItem Then
                Dim groupHeader As GridGroupHeaderItem = DirectCast(e.Item, GridGroupHeaderItem)
                If True Then
                    groupHeader.DataCell.Text = groupHeader.DataCell.Text.Split(":"c)(1).ToString()
                    If groupHeader.DataCell.Text.Trim = "-1" Then
                        groupHeader.DataCell.Text = "N/A"
                    End If
                End If
            End If

            If TypeOf e.Item Is GridDataItem Then
                Dim item As GridDataItem = TryCast(e.Item, GridDataItem)
                If item IsNot Nothing Then
                    Dim QuesID As HiddenField = TryCast(item.Cells(2).FindControl("hfID"), HiddenField)
                    Dim QuesDIV As HtmlGenericControl = TryCast(item.Cells(2).FindControl("divTbl"), HtmlGenericControl)

                    Dim chart As RadHtmlChart = TryCast(item.Cells(2).FindControl("surveyChart"), RadHtmlChart)

                    If Session("SurveyQuesTbl") IsNot Nothing AndAlso QuesID IsNot Nothing Then
                        '' Populating Top Table
                        Dim dtQuestions As DataTable = Session("SurveyQuesTbl")

                        Dim dtTopTbl As DataTable = dtQuestions.Select("QuestionId=" & QuesID.Value & "", "QuestionId ASC").CopyToDataTable()

                        If dtTopTbl.Rows.Count > 0 Then
                            Dim htmlStr As String = ""
                            htmlStr &= "<table class='table'>"
                            htmlStr &= "<tr>"
                            htmlStr &= "<th>Response</th>"
                            htmlStr &= "<th class='text-center'>Response %</th>"
                            htmlStr &= "<th class='text-center'>Response Count</th>"
                            htmlStr &= "</tr>"

                            For Each drow As DataRow In dtTopTbl.Rows
                                htmlStr &= "<tr>"
                                htmlStr &= String.Format("<td>{0}</td>", drow("Response"))
                                htmlStr &= String.Format("<td align='right'>{0}</td>", FormatNumber(CDbl(drow("Response_Count") / drow("TotResp") * 100), 2))
                                htmlStr &= String.Format("<td align='right'>{0}</td>", drow("Response_Count"))
                                htmlStr &= "</tr>"
                            Next

                            htmlStr &= "</table>"

                            QuesDIV.InnerHtml = htmlStr

                            '' Placing Chart

                            Dim ChartTbl As DataTable = ObjReport.GetSurveyStatisticsChart(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value,
                                                                                            ddlSurvey.SelectedValue, QuesID.Value)


                            ChartTbl.Columns.Add("ResponsePercentage", Type.GetType("System.Decimal"))

                            '' Calculating Percentage

                            For Each drow As DataRow In ChartTbl.Rows
                                drow("ResponsePercentage") = FormatNumber(CDbl(drow("Response_Count") / drow("TotResp") * 100))
                            Next

                            chart.DataSource = ChartTbl
                            chart.DataBind()


                        End If

                    End If

                End If
            End If
        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            ObjReport = Nothing
        End Try

    End Sub

    Private Sub dgv_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvRep.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindReport()
    End Sub

    Private Sub dgv_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged

        BindReport()
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


    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If ddlOrganization.SelectedIndex > 0 Then
            If ddlSurvey.SelectedIndex = 0 Then
                MessageBoxValidation("Please select survey", "Validation")
                Return bretval
            End If
            bretval = True
            Return bretval
        Else
            MessageBoxValidation("Please select the Organisation", "Validation")
            Return bretval
        End If
    End Function
    Private Sub LoadReports()
        Session("SurveyQuesTbl") = Nothing
        If ValidateInputs() Then
            gvRep.Visible = True
            RepDiv.Visible = True
            divOtherResponses.Visible = True
            BindReport()

            BindOtherResponses()

            If gvRep.Items.Count > 0 Or gvOtherResponse.Items.Count > 0 Then
                lnkView.Visible = True
            Else
                lnkView.Visible = False
            End If
        Else
            Args.Visible = False
            summary.InnerHtml = ""
            gvRep.Visible = False
            RepDiv.Visible = False
            Details.Visible = False
            divOtherResponses.Visible = False
            lnkView.Visible = False
        End If
    End Sub

    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        LoadReports()
    End Sub

    Private Sub BindOtherResponses()
        Dim ObjReport As New SalesWorx.BO.Common.Reports
        Try
            Dim dt As DataTable = ObjReport.GetSurveyOtherResponses(Err_No, Err_Desc, ddlOrganization.SelectedValue, ddlSurvey.SelectedValue, hfSurveyType.Value)

            If dt IsNot Nothing Then
                dt.Columns.Add("Respondent", Type.GetType("System.String"))


                If dt.Rows.Count > 0 Then
                    dt = dt.Select("Response_Type_Id=1").CopyToDataTable()
                End If

                If dt.Rows.Count = 0 Then '' Hide the div if there is no records
                    divOtherResponses.Visible = False
                    Exit Sub
                End If
               

                For Each drow As DataRow In dt.Rows
                    If hfSurveyType.Value = "N" Then
                        drow("Respondent") = String.Format("{0} - {1}      Surveyed At : {2}", drow("Customer_No"), drow("Customer_Name"), CDate(drow("Survey_TimeStamp")).ToString("dd-MMM-yyyy hh:mm tt"))
                    Else
                        drow("Respondent") = String.Format("{0} [{1}]", drow("SalesRep_Number"), drow("SalesRep_Name"))
                    End If
                Next

            End If

            '' Taking distinct Responses
            Dim ColumnNames(3) As String
            ColumnNames = {"Respondent", "Question_Id", "Response_Id", "Question_Text", "Response"}

            dt = dt.DefaultView.ToTable(True, ColumnNames)

            gvOtherResponse.DataSource = dt
            gvOtherResponse.DataBind()

        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            ObjReport = Nothing
        End Try
    End Sub

    Sub MessageBoxValidation(ByVal str As String, Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub


    Private Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrganization.SelectedIndexChanged
        RepDiv.Visible = False
        Details.Visible = False
    End Sub

    Private Sub gvRep_ItemCreated(sender As Object, e As GridItemEventArgs) Handles gvRep.ItemCreated
        '' For Hiding Expand Colapse
        If TypeOf e.Item Is GridGroupHeaderItem Then
            TryCast(e.Item, GridGroupHeaderItem).Cells(0).Controls.Clear()
        End If
        '' For Hiding Expand Colapse end
    End Sub

    Private Sub gvOtherResponse_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles gvOtherResponse.ItemDataBound
        Try
            For Each item As GridHeaderItem In gvOtherResponse.MasterTableView.GetItems(GridItemType.Header)
                item("Respondent").Text = IIf(hfSurveyType.Value = "N", "Customer", "Van")
            Next
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub gvOtherResponse_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvOtherResponse.PageIndexChanged
        BindOtherResponses()
    End Sub

    Private Sub gvOtherResponse_PreRender(sender As Object, e As EventArgs) Handles gvOtherResponse.PreRender
        ''Try
        ''    Dim headerItem As GridHeaderItem = TryCast(gvOtherResponse.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
        ''    headerItem("Respondent").Text = IIf(hfSurveyType.Value = "N", "Customer", "Van")
        ''Catch ex As Exception
        ''    log.Error(ex.Message)
        ''End Try
    End Sub

    Private Sub gvOtherResponse_SortCommand(sender As Object, e As GridSortCommandEventArgs) Handles gvOtherResponse.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindOtherResponses()
    End Sub

    Private Sub divOtherResponses_PreRender(sender As Object, e As EventArgs) Handles divOtherResponses.PreRender
        
    End Sub

    Private Sub lnkView_Click(sender As Object, e As EventArgs) Handles lnkView.Click
        Try
            Session("O") = ddlOrganization.SelectedValue
            Session("S") = ddlSurvey.SelectedValue

            Response.Redirect("SurveyAllResponses.aspx?o=" & ddlOrganization.SelectedValue & "&s=" & ddlSurvey.SelectedValue & "&t=" & hfSurveyType.Value)
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        ddlSurvey.ClearSelection()

        Args.Visible = False
        summary.InnerHtml = ""
        gvRep.Visible = False
        RepDiv.Visible = False
        Details.Visible = False
        divOtherResponses.Visible = False
        lnkView.Visible = False
    End Sub
End Class