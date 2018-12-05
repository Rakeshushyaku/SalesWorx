Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Imports OfficeOpenXml
Partial Public Class Rep_LogReport
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "LogReport"

    Private Const PageID As String = "P105"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single

    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    
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
                ddlOrganization.DataTextField = "Description"
                ddlOrganization.DataValueField = "MAS_Org_ID"
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))
                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                End If
                LoadOrgDetails()
                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")


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


        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

        Dim USRID As New ReportParameter
        USRID = New ReportParameter("Uid", objUserAccess.UserID)


        Dim SalesRepID As New ReportParameter
        SalesRepID = New ReportParameter("SID", ddl_Van.SelectedItem.Value)

        Dim FDate As New ReportParameter
        FDate = New ReportParameter("FromDate", CDate(txtFromDate.SelectedDate))

        Dim TDate As New ReportParameter
        TDate = New ReportParameter("ToDate", CDate(txtToDate.SelectedDate))


        Dim OID As New ReportParameter
        OID = New ReportParameter("OID", CStr(ddlOrganization.SelectedValue.ToString()))


        Dim tcustID As String = "0"
        Dim tSiteID As String = "0"
        If Not String.IsNullOrEmpty(ddl_Customer.SelectedValue) Then

            Dim ids() As String
            ids = ddl_Customer.SelectedValue.Split("$")
            tcustID = ids(0)
            tSiteID = ids(1)
      
        End If

        Dim CustID As New ReportParameter
        CustID = New ReportParameter("CustID", tcustID)


        Dim SiteID As New ReportParameter
        SiteID = New ReportParameter("SiteID", tSiteID)


        Dim DocType As New ReportParameter
        DocType = New ReportParameter("DocType", ddl_type.SelectedItem.Value)

        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)

        rview.ServerReport.SetParameters(New ReportParameter() {USRID, OID, SalesRepID, FDate, TDate, CustID, SiteID, DocType})

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
            Response.AddHeader("Content-disposition", "attachment;filename=LogReport.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=LogReport.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()

    End Sub
    Private Sub BindReport()
        If Not ddlOrganization.SelectedItem Is Nothing Then




            Dim van As String = ddl_Van.SelectedItem.Value


            If van = "0" Then
                lbl_van.Text = "All"
            Else
                lbl_van.Text = ddl_Van.SelectedItem.Text
            End If


            lbl_org.Text = ddlOrganization.SelectedItem.Text
            lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")

            Dim custID As String = "0"
            Dim SiteID As String = "0"
            If Not String.IsNullOrEmpty(ddl_Customer.SelectedValue) Then

                Dim ids() As String
                ids = ddl_Customer.SelectedValue.Split("$")
                Dim custdt As New DataTable
                custdt = (New SalesWorx.BO.Common.Customer).GetCustomerDeatils(Err_No, Err_Desc, ids(0), ids(1))
                If custdt.Rows.Count > 0 Then
                    lbl_Customer.Text = custdt.Rows(0)("Customer_no").ToString & " - " & custdt.Rows(0)("Customer_name").ToString
                End If
                custID = ids(0)
                SiteID = ids(1)
            Else
                lbl_Customer.Text = "All"
            End If

            lbl_DocType.Text = ddl_type.SelectedItem.Text
            Args.Visible = True

            HSID.Value = van
            hfSMonth.Value = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            hfEMonth.Value = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")
            hfOrg.Value = ddlOrganization.SelectedItem.Value

            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetLogReport(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, van, txtFromDate.SelectedDate, txtToDate.SelectedDate, custID, SiteID, ddl_type.SelectedItem.Value)

            gvRep.DataSource = dt
            gvRep.DataBind()


            Dim dtcurrency As DataTable
            dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

            Dim Currency As String = ""
            If dtcurrency.Rows.Count > 0 Then
                Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
                lblDecimal.Text = "N" & dtcurrency.Rows(0)("Decimal_Digits")
            End If


            Dim dtSummary As New DataTable
            dtSummary = ObjReport.GetLogReportSummary(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, van, txtFromDate.SelectedDate, txtToDate.SelectedDate, custID, SiteID)

           
            Dim StrSummary As String = ""
            Dim StrSummaryR As String = ""
            Dim i As Integer = 0
            For Each dr As DataRow In dtSummary.Rows
                'Dim dr As DataRow
                'dr = DtSummary.NewRow
                'dr(0) = "Total " & x.PayMode & Currency
                'dr(1) = Format(x.Total, "#,##0.00")
                'DtSummary.Rows.Add(dr)
                If Val(dr("Cust").ToString) = 0 Then
                    Dim cur As String = ""
                    If ddl_type.SelectedItem.Value = "0" Then
                        If dr("CurrCode").ToString <> "" Then
                            cur = cur & " (" & dr("CurrCode").ToString & ")"
                            StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>" & dr("Description") & cur & "$INFOICON$<div class='text-primary'>" & Format(Val(dr("amount").ToString), lblDecimal.Text) & "</div></div></div>"
                        Else
                            StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>" & dr("Description") & " $INFOICON$<div class='text-primary'>" & dr("amount").ToString & "</div></div></div>"
                        End If
                    Else
                        If ddl_type.SelectedItem.Value = "Invoice" Then
                            If dr("Description").ToString() = "Total Sales" Or dr("Description").ToString() = "Total Calls" Or dr("Description").ToString() = "Total Productive Calls" Then
                                If dr("CurrCode").ToString <> "" Then
                                    cur = cur & " (" & dr("CurrCode").ToString & ")"
                                    StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>" & dr("Description") & cur & "$INFOICON$<div class='text-primary'>" & Format(Val(dr("amount").ToString), lblDecimal.Text) & "</div></div></div>"
                                Else
                                    StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>" & dr("Description") & "$INFOICON$<div class='text-primary'>" & dr("amount").ToString & "</div></div></div>"
                                End If
                            End If
                        End If
                        If ddl_type.SelectedItem.Value = "Credit Note" Then
                            If dr("Description").ToString() = "Total Returns" Or dr("Description").ToString() = "Total Calls" Or dr("Description").ToString() = "Total Productive Calls" Then
                                If dr("CurrCode").ToString <> "" Then
                                    cur = cur & " (" & dr("CurrCode").ToString & ")"
                                    StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>" & dr("Description") & cur & "$INFOICON$<div class='text-primary'>" & Format(Val(dr("amount").ToString), lblDecimal.Text) & "</div></div></div>"
                                Else
                                    StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>" & dr("Description") & "$INFOICON$<div class='text-primary'>" & dr("amount").ToString & "</div></div></div>"
                                End If
                            End If
                        End If
                        If ddl_type.SelectedItem.Value = "Collection" Then
                            If dr("Description").ToString() = "Total Collections" Or dr("Description").ToString() = "Total Calls" Or dr("Description").ToString() = "Total Productive Calls" Then
                                If dr("CurrCode").ToString <> "" Then
                                    cur = cur & " (" & dr("CurrCode").ToString & ")"
                                    StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>" & dr("Description") & cur & "$INFOICON$<div class='text-primary'>" & Format(Val(dr("amount").ToString), lblDecimal.Text) & "</div></div></div>"
                                Else
                                    StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>" & dr("Description") & "$INFOICON$<div class='text-primary'>" & dr("amount").ToString & "</div></div></div>"
                                End If
                            End If
                        End If
                        
                    End If
                    If dr("Description").ToString().ToLower.Contains("calls") Then
                        If dr("Description").ToString().ToLower.Contains("productive") Then
                            StrSummary = StrSummary.Replace("$INFOICON$", "<i class='fa fa-info-circle' data-toggle='tooltip' data-container='body' title='No. of Calls in which Invoice is taken'></i>")
                        Else
                            If CType(Session.Item("CONTROL_PARAMS"), ControlParams).USE_DISTR_IN_CALLS = "Y" Then
                                StrSummary = StrSummary.Replace("$INFOICON$", "<i class='fa fa-info-circle' data-toggle='tooltip' data-container='body' title='No. of Visits in which Distribution check was performed.'></i>")
                            Else
                                StrSummary = StrSummary.Replace("$INFOICON$", "<i class='fa fa-info-circle' data-toggle='tooltip' data-container='body' title='No. of Visits'></i>")
                            End If
                        End If
                    Else
                        StrSummary = StrSummary.Replace("$INFOICON$", "")
                    End If

                End If
                i = i + 1
            Next
            summary.InnerHtml = StrSummary
        End If
    End Sub
    Private Sub ddl_Customer_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddl_Customer.ItemsRequested
        summary.InnerText = ""
        gvRep.Visible = False
        Args.Visible = False
        RepDiv.Visible = False
        Dim Objrep As New SalesWorx.BO.Common.Reports()
        Dim dt As New DataTable
        dt = Objrep.GetCustomerfromOrgtext(Err_No, Err_Desc, ddlOrganization.SelectedValue, e.Text)


        Dim ItemsPerRequest As Integer = 100 'Set the number of values to show
        Dim itemOffset As Integer = e.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
        e.EndOfItems = endOffset = dt.Rows.Count

        'Loop through the values to populate the combo box
        For i As Integer = itemOffset To endOffset - 1
            Dim item As New RadComboBoxItem()
            item.Text = dt.Rows(i).Item("Customer").ToString
            item.Value = dt.Rows(i).Item("CustomerID").ToString

            ddl_Customer.Items.Add(item)
            item.DataBind()
        Next
    End Sub
    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender
        For Each column As GridColumn In gvRep.MasterTableView.Columns
            If column.UniqueName = "Order_Amt" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                column.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
                col.HeaderText = "Order Amt" & lbl_currency.Text
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
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If ddlOrganization.SelectedIndex > 0 Then
            If ddl_Van.SelectedItem.Value = "0" Then
                MessageBoxValidation("Select a Van", "Validation")
                SetFocus(ddl_Van)
                Return bretval
            End If

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
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            gvRep.Visible = True
            RepDiv.Visible = True
            BindReport()
            rpbFilter.Items(0).Expanded = False
        Else
            Args.Visible = False
            summary.InnerHtml = ""
            gvRep.Visible = False
            RepDiv.Visible = False
            rpbFilter.Items(0).Expanded = True
        End If

    End Sub
    Sub MessageBoxValidation(ByVal str As String, Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged

        LoadOrgDetails()
       
    End Sub
    Sub LoadOrgDetails()
        If Not (ddlOrganization.SelectedItem.Value = "0") Then

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New SalesWorx.BO.Common.Common()


            ddl_Van.DataTextField = "SalesRep_Name"
            ddl_Van.DataValueField = "SalesRep_ID"
            ddl_Van.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)

            ddl_Van.DataBind()
            ddl_Van.Items.Insert(0, New RadComboBoxItem("Select Van", 0))
            Dim dtcurrency As DataTable
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

            Dim Currency As String = ""
            If dtcurrency.Rows.Count > 0 Then
                Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
                lblDecimal.Text = "N" & dtcurrency.Rows(0)("Decimal_Digits")
                lbl_currency.Text = Currency
            End If

        Else
            ddl_Van.Items.Clear()
        End If

    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        ddl_Van.ClearSelection()
        ddl_Van.Items.Clear()
        ddl_type.ClearSelection()
        ddl_Customer.ClearSelection()
        ddl_Customer.Text = ""
        ddl_type.SelectedIndex = 0
        LoadOrgDetails()
        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")
        Args.Visible = False
        summary.InnerHtml = ""
        gvRep.Visible = False
        RepDiv.Visible = False
    End Sub
    Protected Sub gvRep_ItemCommand(sender As Object, e As GridCommandEventArgs)
        BindReport()
        If e.CommandName = RadGrid.ExportToExcelCommandName Then
            gvRep.ExportSettings.Excel.Format = GridExcelExportFormat.Biff
            gvRep.ExportSettings.IgnorePaging = True
            gvRep.ExportSettings.ExportOnlyData = True
            gvRep.ExportSettings.OpenInNewWindow = True
            gvRep.ExportSettings.FileName = "VanLogReport"
        End If

    End Sub

    Private Sub BtnExportBiffExcel_Click(sender As Object, e As EventArgs) Handles BtnExportBiffExcel.Click
        If Not ddlOrganization.SelectedItem Is Nothing Then




            Dim van As String = ddl_Van.SelectedItem.Value


            If van = "0" Then
                lbl_van.Text = "All"
            Else
                lbl_van.Text = ddl_Van.SelectedItem.Text
            End If


            lbl_org.Text = ddlOrganization.SelectedItem.Text
            lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")

            Dim custID As String = "0"
            Dim SiteID As String = "0"
            If Not String.IsNullOrEmpty(ddl_Customer.SelectedValue) Then

                Dim ids() As String
                ids = ddl_Customer.SelectedValue.Split("$")
                Dim custdt As New DataTable
                custdt = (New SalesWorx.BO.Common.Customer).GetCustomerDeatils(Err_No, Err_Desc, ids(0), ids(1))
                If custdt.Rows.Count > 0 Then
                    lbl_Customer.Text = custdt.Rows(0)("Customer_no").ToString & " - " & custdt.Rows(0)("Customer_name").ToString
                End If
                custID = ids(0)
                SiteID = ids(1)
            Else
                lbl_Customer.Text = "All"
            End If

            lbl_DocType.Text = ddl_type.SelectedItem.Text
            Args.Visible = True

            HSID.Value = van
            hfSMonth.Value = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            hfEMonth.Value = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")
            hfOrg.Value = ddlOrganization.SelectedItem.Value

            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            Dim tblData As New DataTable
            dt = ObjReport.GetLogReport(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, van, txtFromDate.SelectedDate, txtToDate.SelectedDate, custID, SiteID, ddl_type.SelectedItem.Value)
            tblData = dt.DefaultView.ToTable(False, "Customer", "DocType", "Orig_Sys_Document_Ref", "Order_Amt", "Start_Time", "CC")

            If tblData.Rows.Count > 0 Then

                Using package As New ExcelPackage()
                    ' add a new worksheet to the empty workbook
                    Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
                    Worksheet.Cells("A1").LoadFromDataTable(tblData, True)
                    Worksheet.Column(5).Style.Numberformat.Format = "dd-MMM-yyyy hh:mm AM/PM"
                    Worksheet.Cells.AutoFitColumns()
                    Worksheet.Cells("A1").Value = "Customer"
                    Worksheet.Cells("B1").Value = "DocType"
                    Worksheet.Cells("C1").Value = "Document Ref No."
                    Worksheet.Cells("D1").Value = "Order Amount"
                    Worksheet.Cells("E1").Value = "Start Time"
                    Worksheet.Cells("F1").Value = "Credit Customer"
                    

                    Response.Clear()
                    Response.Buffer = True
                    Response.Charset = ""

                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    Response.AddHeader("content-disposition", "attachment;filename= VanLogReport.xlsx")

                    Using MyMemoryStream As New MemoryStream()
                        package.SaveAs(MyMemoryStream)
                        MyMemoryStream.WriteTo(Response.OutputStream)
                        Response.AddHeader("Content-Length", MyMemoryStream.Length)
                        Response.Flush()
                        Response.Close()
                    End Using
                End Using
            End If
        End If
    End Sub
End Class