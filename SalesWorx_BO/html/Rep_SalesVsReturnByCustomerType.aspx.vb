Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Public Class Rep_SalesVsReturnByCustomerType
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "SalesVsReturnByCustomerType"

    Private Const PageID As String = "P339"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single

    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    'Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
    '    If Not IsNothing(Me.Master) Then

    '        Dim masterScriptManager As ScriptManager
    '        masterScriptManager = CType(Master.FindControl("ScriptManager1"), ScriptManager)

    '        ' Make sure our master page has the script manager we're looking for
    '        If Not IsNothing(masterScriptManager) Then

    '            ' Turn off partial page postbacks for this page
    '            masterScriptManager.EnablePartialRendering = False
    '        End If

    '    End If

    'End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
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
                UID.Value = CType(Session("User_Access"), UserAccess).UserID
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))

                If ddlOrganization.Items.Count > 1 Then
                    ddlOrganization.SelectedIndex = 1
                End If

                LoadOrgDetails()
                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                txtToDate.SelectedDate = Now
                HFrom.Value = txtFromDate.SelectedDate
                HTo.Value = txtToDate.SelectedDate




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
                ObjCommon = Nothing
                ErrorResource = Nothing
            End Try
        End If
    End Sub
    Sub Export(format As String)


        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)



        Dim USRID As New ReportParameter
        USRID = New ReportParameter("UID", CType(Session.Item("USER_ACCESS"), UserAccess).UserID)

        Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems

        Dim van As String = ""
        For Each li As RadComboBoxItem In collection
            van = van & li.Value & ","
        Next

        If van = "" Then
            van = "0"
        End If

        Dim SalesRepID As New ReportParameter
        SalesRepID = New ReportParameter("SID", van)


        Dim fromdate As String
        fromdate = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")

        Dim todate As String
        todate = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")

        Dim FDate As New ReportParameter
        FDate = New ReportParameter("Fromdate", fromdate)

        Dim TDate As New ReportParameter
        TDate = New ReportParameter("ToDate", todate)


        Dim OID As New ReportParameter
        OID = New ReportParameter("OID", CStr(ddlOrganization.SelectedValue.ToString()))



        rview.ServerReport.SetParameters(New ReportParameter() {USRID, OID, SalesRepID, FDate, TDate})

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
            Response.AddHeader("Content-disposition", "attachment;filename=SalesVsReturn.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=SalesVsReturn.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()
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
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            gvRep.Visible = True
            BindReport()
            BindChart()
        Else
            Args.Visible = False
            gvRep.Visible = False
        End If
    End Sub
    Private Sub BindChart()
        Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems

        Dim van As String = ""
        For Each li As RadComboBoxItem In collection
            van = van & li.Value & ","
        Next

        If van = "" Then
            van = "0"
        End If
        HfVan.Value = van
        ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
    End Sub
    Private Sub BindReport()
        Try
            Dim SearchQuery As String = ""
            If ValidateInputs() Then
                rpbFilter.Items(0).Expanded = False
                Args.Visible = True
                lbl_org.Text = ddlOrganization.SelectedItem.Text
                Dim objUserAccess As UserAccess
                objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
                Dim ObjReport As New SalesWorx.BO.Common.Reports

                Dim fromdate As DateTime
                fromdate = CDate(txtFromDate.SelectedDate)

                Dim todate As DateTime
                todate = CDate(txtToDate.SelectedDate)

                Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems

                Dim van As String = ""
                Dim vantxt As String = ""
                For Each li As RadComboBoxItem In collection
                    van = van & li.Value & ","
                    vantxt = vantxt & li.Text & ","
                Next
                If vantxt <> "" Then
                    vantxt = vantxt.Substring(0, vantxt.Length - 1)
                End If
                If van = "" Then
                    van = "0"
                End If
                If van = "0" Then
                    lbl_van.Text = "All"
                Else
                    lbl_van.Text = vantxt
                End If
                lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
                lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")
                Dim dt As New DataTable
                dt = ObjReport.GetVanSalesbyCustomer(Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, ddlOrganization.SelectedItem.Value, van, fromdate.ToString("dd-MMM-yyyy"), todate.ToString("dd-MMM-yyyy"))


                gvRep.DataSource = dt
                gvRep.DataBind()


                Dim dtcurrency As DataTable
                dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
                Dim Currency As String = ""
                If dtcurrency.Rows.Count > 0 Then
                    Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
                End If



                Dim StrSummary As String = ""

                Dim sum = dt.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("Sales")))

                StrSummary = StrSummary & "<div class='col-sm-6'><div class='widgetblk'>Total Sales " & Currency & "<div class='text-primary'>" & Format(sum, lblDecimal.Text) & "</div></div></div>"

                Dim sumRma = dt.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("Returns")))

                StrSummary = StrSummary & "<div class='col-sm-6'><div class='widgetblk'>Total Returns " & Currency & "<div class='text-primary'>" & Format(sumRma, lblDecimal.Text) & "</div></div></div>"


                summary.InnerHtml = StrSummary

                If dt.Rows.Count > 8 And dt.Rows.Count < 14 Then
                    Chartwrapper1.Style.Add("width", (dt.Rows.Count * 60).ToString & "px")
                ElseIf dt.Rows.Count > 14 Then
                    Chartwrapper1.Style.Add("width", (dt.Rows.Count * 40).ToString & "px")
                Else
                    Chartwrapper1.Style.Add("width", "600px")
                End If

            End If
        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If ddlOrganization.SelectedItem.Value = "0" Then
            MessageBoxValidation("Select an Organisation", "Validation")
            SetFocus(ddlOrganization)
            Return bretval
        End If

        If txtFromDate.DateInput.Text = "" Then
            MessageBoxValidation("Enter valid ""From date"".", "Validation")
            SetFocus(txtFromDate)
            Return bretval
        End If

        If txtToDate.DateInput.Text = "" Then
            MessageBoxValidation("Enter valid ""To date"".", "Validation")
            SetFocus(txtToDate)
            Return bretval
        End If

        If CDate(txtFromDate.SelectedDate) > CDate(txtToDate.SelectedDate) Then
            MessageBoxValidation("Start Date should not be greater than End Date.", "Validation")
            Return bretval
        End If
        bretval = True
        Return bretval
    End Function

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

    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged

        LoadOrgDetails()
        summary.InnerHtml = ""
        gvRep.Visible = False
    End Sub
    Sub LoadOrgDetails()

        If Not (ddlOrganization.SelectedItem.Value = "0") Then

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New SalesWorx.BO.Common.Common()
            ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
            ddlVan.DataBind()

            For Each itm As RadComboBoxItem In ddlVan.Items
                itm.Checked = True
            Next

            Dim dtcurrency As DataTable
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

            Dim Currency As String = ""
            If dtcurrency.Rows.Count > 0 Then
                Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
                HCurrency.Value = Currency
                lblDecimal.Text = "N" & dtcurrency.Rows(0)("Decimal_Digits")
            End If

            hfOrgID.Value = ddlOrganization.SelectedItem.Value
        End If
    End Sub
    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender

        For Each column As GridColumn In gvRep.MasterTableView.Columns
            If column.UniqueName = "Sales" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
                column.HeaderText = "Total Sales" & HCurrency.Value
                'col.Aggregate = GridAggregateFunction.Sum
                'col.FooterAggregateFormatString = "{0:" & lblDecimal.Text & "}"

            End If
            If column.UniqueName = "Returns" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
                column.HeaderText = "Total Returns" & HCurrency.Value
                'col.Aggregate = GridAggregateFunction.Sum
                'col.FooterAggregateFormatString = "{0:" & lblDecimal.Text & "}"
            End If
        Next
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub

    Private Sub txtFromDate_SelectedDateChanged(sender As Object, e As Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs) Handles txtFromDate.SelectedDateChanged
        HFrom.Value = txtFromDate.SelectedDate
    End Sub

    Private Sub txtToDate_SelectedDateChanged(sender As Object, e As Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs) Handles txtToDate.SelectedDateChanged
        HTo.Value = txtToDate.SelectedDate
    End Sub
    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        ddlVan.ClearCheckedItems()
        ddlVan.Items.Clear()
        LoadOrgDetails()
        summary.InnerHtml = ""
        Args.Visible = False
        gvRep.Visible = False
        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Now
        HFrom.Value = txtFromDate.SelectedDate
        HTo.Value = txtToDate.SelectedDate
    End Sub
    Protected Sub gvRep_ItemCommand(sender As Object, e As GridCommandEventArgs)
        BindReport()
        If e.CommandName = RadGrid.ExportToExcelCommandName Then
            gvRep.ExportSettings.Excel.Format = GridExcelExportFormat.Biff
            gvRep.ExportSettings.IgnorePaging = True
            gvRep.ExportSettings.ExportOnlyData = True
            gvRep.ExportSettings.OpenInNewWindow = True
            gvRep.ExportSettings.FileName = "SalesVsReturnsbyCustType"
        End If

    End Sub
End Class