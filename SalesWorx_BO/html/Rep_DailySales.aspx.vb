Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Imports System.Globalization
Imports OfficeOpenXml
Public Class Rep_DailySales
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer
    Private ReportPath As String = "DailySales"
    Dim dv As New DataView
    Private Const PageID As String = "P321"
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
            Session("OrdersDT") = Nothing
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
                LoadOrgDetails()

                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)

                txtToDate.SelectedDate = Now()
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
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
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
    Sub Export(format As String)

        
        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

        Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems

        Dim van As String = ""
        For Each li As RadComboBoxItem In collection
            van = van & li.Value & ","
        Next
        If van <> "" Then
            van = van.Substring(0, van.Length - 1)
        End If
        If van = "" Then
            van = "0"
        End If


        Dim OrgID As New ReportParameter
        OrgID = New ReportParameter("OID", CStr(ddlOrganization.SelectedValue.ToString()))

        Dim FromDate As New ReportParameter
        FromDate = New ReportParameter("FromDate", CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"))

        Dim ToDate As New ReportParameter
        ToDate = New ReportParameter("Todate", CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))

        Dim SalesRep_ID As New ReportParameter
        SalesRep_ID = New ReportParameter("SID", van)

        Dim Uid As New ReportParameter
        Uid = New ReportParameter("Uid", objUserAccess.UserID)

        rview.ServerReport.SetParameters(New ReportParameter() {OrgID, SalesRep_ID, FromDate, ToDate, Uid})

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
            Response.AddHeader("Content-disposition", "attachment;filename=DailySales.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=DailySales.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()
    End Sub

    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender
        For Each column As GridColumn In gvRep.MasterTableView.Columns
            If column.UniqueName = "Order_amt" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
            End If
        Next
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
    Private Property SortReturnDirection() As String
        Get
            If ViewState("SortReturnDirection") Is Nothing Then
                ViewState("SortReturnDirection") = "ASC"
            End If
            Return ViewState("SortReturnDirection").ToString()
        End Get
        Set(ByVal value As String)
            Dim s As String = SortReturnDirection

            If value = "flip" Then
                s = If(s = "ASC", "DESC", "ASC")
            Else
                s = value
            End If

            ViewState("SortReturnDirection") = s
        End Set
    End Property
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            rep.Visible = True
            BindReport()
        Else
            summary.InnerText = ""
            rep.Visible = False
            Args.Visible = False
        End If
    End Sub
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If ddlOrganization.SelectedIndex > 0 Then
            Dim TemFromDateStr As String = CDate(txtFromDate.SelectedDate).ToString("dd/MM/yyyy")
            Dim DateArr As Array = TemFromDateStr.Split("/")
            If DateArr.Length = 3 Then
                TemFromDateStr = DateArr(1) & "/" & DateArr(0) & "/" & DateArr(2)
            End If
            Dim TemToDateStr As String = CDate(txtToDate.SelectedDate).ToString("dd/MM/yyyy")
            Dim DateArr1 As Array = TemToDateStr.Split("/")
            If DateArr1.Length = 3 Then
                TemToDateStr = DateArr1(1) & "/" & DateArr1(0) & "/" & DateArr1(2)
            End If

            If Not IsDate(TemFromDateStr) Then
                MessageBoxValidation("Enter valid ""From date"".", "Validation")
                SetFocus(txtFromDate)
                Return bretval
            End If

            If Not IsDate(TemToDateStr) Then
                MessageBoxValidation("Enter valid ""To date"".", "Validation")
                SetFocus(TemToDateStr)
                Return bretval
            End If

            If CDate(TemFromDateStr) > CDate(TemToDateStr) Then
                MessageBoxValidation("Start Date should not be greater than End Date.", "Validation")
                Return bretval
            End If
            bretval = True
            Return bretval
        Else
            MessageBoxValidation("Please select the Organisation", "Validation")
            Return bretval
        End If
    End Function
    Private Sub BindReport()
        Try
            Dim SearchQuery As String = ""
            Rep.visible = True
            rpbFilter.Items(0).Expanded = False
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjReport As New SalesWorx.BO.Common.Reports

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

            Dim NoofVans As Integer = 0
            If van = "0" Then
                lbl_Van.Text = "All"
                NoofVans = ddlVan.Items.Count
            Else
                NoofVans = ddlVan.CheckedItems.Count
                lbl_Van.Text = vantxt
            End If

            lbl_org.Text = ddlOrganization.SelectedItem.Text
            lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")

            Args.Visible = True
            Dim dt As New DataTable
            dt = ObjReport.GetDailySalesReport_Order(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, van, objUserAccess.UserID, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))
            gvRep.DataSource = dt
            gvRep.DataBind()

            Dim StrSummary As String = ""
            Dim TotolSales As Decimal = 0
            Dim NetSales As Decimal = 0

            Dim dtcurrency As DataTable
            dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
            Dim Currency As String = ""
            If dtcurrency.Rows.Count > 0 Then
                Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
            End If

            Dim SalesVat = dt.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("Vat_amount")))

            Dim query = (From UserEntry In dt _
                        Where UserEntry.Field(Of String)("Mode") = "CREDIT" _
                       Group UserEntry By key = UserEntry.Field(Of String)("Mode") Into Group _
                       Select PayMode = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("Order_amt"))).ToList
            If query.Count > 0 Then
                For Each x In query
                    StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>" & "Total " & CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.PayMode.ToLower) & " Sales " & Currency & "<div class='text-primary'>" & Format(x.Total, lblDecimal.Text) & "</div></div></div>"
                    TotolSales = TotolSales + x.Total
                Next
            Else
                StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>" & "Total Credit Sales" & Currency & "<div class='text-primary'>" & Format(0, lblDecimal.Text) & "</div></div></div>"
            End If

            Dim query1 = (From UserEntry In dt _
                         Where UserEntry.Field(Of String)("Mode") = "CASH" _
                        Group UserEntry By key = UserEntry.Field(Of String)("Mode") Into Group _
                        Select PayMode = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("Order_amt"))).ToList


            If query1.Count > 0 Then
                For Each x In query1
                    StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>" & "Total " & CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.PayMode.ToLower) & " Sales" & Currency & "<div class='text-primary'>" & Format(x.Total, lblDecimal.Text) & "</div></div></div>"
                    TotolSales = TotolSales + x.Total
                Next
            Else
                StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>" & "Total Cash Sales" & Currency & "<div class='text-primary'>" & Format(0, lblDecimal.Text) & "</div></div></div>"
            End If

            summary.InnerHtml = "<div class='col-sm-4'><div class='widgetblk'>" & "Total  Sales" & Currency & "<div class='text-primary'>" & Format(TotolSales, lblDecimal.Text) & "</div></div></div>"

            Dim dtReturn As New DataTable
            dtReturn = ObjReport.GetDailySalesReport_Return(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, van, objUserAccess.UserID, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))
            gvRepReturn.DataSource = dtReturn
            gvRepReturn.DataBind()

            Dim sumResellable = dtReturn.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("Resellable")))
            Dim sumNonResellable = dtReturn.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("NonResellable")))
            Dim sumReturns = dtReturn.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("Total")))
            Dim ReturnsVat = dtReturn.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("Vat_amount")))

            Dim NetVat As Decimal = SalesVat - ReturnsVat
            'StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk widgetblkinsmall'>Total Returns " & Currency & "<div class='text-primary'> " & Format(sumReturns, lblDecimal.Text) & "</div><div class='row'><div class='col-xs-6'><small>Resellable " & Currency & " <span>" & Format(sumResellable, lblDecimal.Text) & "</span></small></div><div class='col-xs-6'><small>Non Resellable " & Currency & " <span>" & Format(sumNonResellable, lblDecimal.Text) & "</span></small></div></div></div></div>"
            'StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>Total Resellable " & Currency & "<div class='text-primary'>" & Format(sumResellable, "#,##0.00") & "</div></div></div>"
            ''StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>Total Non Resellable " & Currency & "<div class='text-primary'>" & Format(sumNonResellable, "#,##0.00") & "</div></div></div>"
            StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>" & "Total Returns" & Currency & "<div class='text-primary'>" & Format(sumReturns, lblDecimal.Text) & "</div></div></div>"
            StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>" & "Resellable" & Currency & "<div class='text-primary'>" & Format(sumResellable, lblDecimal.Text) & "</div></div></div>"
            StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>" & "Non - Resellable" & Currency & "<div class='text-primary'>" & Format(sumNonResellable, lblDecimal.Text) & "</div></div></div>"
            NetSales = TotolSales - sumReturns
            summary.InnerHtml = summary.InnerHtml & StrSummary
            summary.InnerHtml = summary.InnerHtml & "<div class='col-sm-4'><div class='widgetblk'>" & "Net  Sales" & Currency & "<div class='text-primary'>" & Format(NetSales, lblDecimal.Text) & "</div></div></div>"
            summary.InnerHtml = summary.InnerHtml & "<div class='col-sm-4'><div class='widgetblk'>" & "Net  Vat" & Currency & "<div class='text-primary'>" & Format(NetVat, lblDecimal.Text) & "</div></div></div>"
            Dim avgSalesbyVan As Decimal = 0
            If NoofVans <> 0 Then
                avgSalesbyVan = NetSales / NoofVans
            Else
                avgSalesbyVan = 0
            End If
            summary.InnerHtml = summary.InnerHtml & "<div class='col-sm-4'><div class='widgetblk'>" & "Avg  Sales by Van" & Currency & "<div class='text-primary'>" & Format(avgSalesbyVan, lblDecimal.Text) & "</div></div></div>"
        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub

    
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        If Not (ddlOrganization.SelectedItem.Value = "0") Then
            LoadOrgDetails()
        Else
            Rep.visible = False
        End If

    End Sub
    Sub LoadOrgDetails()
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        ObjCommon = New SalesWorx.BO.Common.Common()
        ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())
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
            lblDecimal.Text = "N" & dtcurrency.Rows(0)("Decimal_Digits")
        End If


    End Sub
    Private Sub gvRepReturn_PreRender(sender As Object, e As EventArgs) Handles gvRepReturn.PreRender
        For Each column As GridColumn In gvRepReturn.MasterTableView.Columns
            If column.UniqueName = "Resellable" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
            End If
            If column.UniqueName = "NonResellable" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
            End If
            If column.UniqueName = "Total" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
            End If
        Next
    End Sub
    Private Sub gvRepReturn_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvRepReturn.SortCommand
        ViewState("SortField") = e.SortExpression
        SortReturnDirection = "flip"
        BindReport()
    End Sub
    Private Sub gvRepReturn_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvRepReturn.PageIndexChanged
        BindReport()
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        Args.Visible = False
        ddlVan.ClearCheckedItems()
        ddlVan.Items.Clear()
        If Not (ddlOrganization.SelectedItem.Value = "0") Then
            LoadOrgDetails()
        Else
            rep.Visible = False
        End If
        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)

        txtToDate.SelectedDate = Now()

        summary.InnerText = ""
        rep.Visible = False
        Args.Visible = False
    End Sub

    Private Sub BtnExportBiffExcel_Click(sender As Object, e As EventArgs) Handles BtnExportBiffExcel.Click
        Dim tblData As New DataTable
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        Dim ObjReport As New SalesWorx.BO.Common.Reports

        Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems

        Dim van As String = ""

        For Each li As RadComboBoxItem In collection
            van = van & li.Value & ","

        Next
        
        If van = "" Then
            van = "0"
        End If

        Dim dt As New DataTable
        dt = ObjReport.GetDailySalesReport_Order(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, van, objUserAccess.UserID, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))
        tblData = dt.DefaultView.ToTable(False, "Orig_Sys_Document_Ref", "Start_Time", "SalesRep_Name", "Customer_NO", "Customer", "Order_amt", "Vat_Amount", "Mode", "Customer_PO_Number")

        If tblData.Rows.Count > 0 Then
            Using package As New ExcelPackage()
                ' add a new worksheet to the empty workbook
                Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
                Worksheet.Cells("A1").LoadFromDataTable(tblData, True)
                Worksheet.Column(2).Style.Numberformat.Format = "dd-MMM-yyyy"
                Worksheet.Cells.AutoFitColumns()

                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename= DailySalesOrders.xlsx")

                Using MyMemoryStream As New MemoryStream()
                    package.SaveAs(MyMemoryStream)
                    MyMemoryStream.WriteTo(Response.OutputStream)
                    Response.AddHeader("Content-Length", MyMemoryStream.Length)
                    Response.Flush()
                    Response.Close()
                End Using
            End Using
        End If
    End Sub

    Private Sub BtnExportBiffExcel_Stocks_Click(sender As Object, e As EventArgs) Handles BtnExportBiffExcel_Stocks.Click
        Dim tblData As New DataTable
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        Dim ObjReport As New SalesWorx.BO.Common.Reports

        Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems

        Dim van As String = ""

        For Each li As RadComboBoxItem In collection
            van = van & li.Value & ","

        Next

        If van = "" Then
            van = "0"
        End If

        Dim dt As New DataTable
        dt = ObjReport.GetDailySalesReport_Return(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, van, objUserAccess.UserID, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))
        tblData = dt.DefaultView.ToTable(False, "Orig_Sys_Document_Ref", "Start_Time", "SalesRep_Name", "Customer_NO", "Customer", "Resellable", "NonResellable", "Total", "Vat_Amount", "Customer_Ref_No")

        Using package As New ExcelPackage()
            ' add a new worksheet to the empty workbook
            Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
            Worksheet.Cells("A1").LoadFromDataTable(tblData, True)
            Worksheet.Column(2).Style.Numberformat.Format = "dd-MMM-yyyy"
            Worksheet.Cells.AutoFitColumns()

            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""

            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename= DailySalesReturns.xlsx")

            Using MyMemoryStream As New MemoryStream()
                package.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.AddHeader("Content-Length", MyMemoryStream.Length)
                Response.Flush()
                Response.Close()
            End Using
        End Using

    End Sub
End Class