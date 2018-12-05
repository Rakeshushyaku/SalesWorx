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

Public Class Rep_SalesVolumebyProduct
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "SalesVolumebyProduct"

    Private Const PageID As String = "P393"
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
                        Try
               
                ObjCommon = New SalesWorx.BO.Common.Common()

                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))

                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1

                    Dim dtcurrency As DataTable
                    Dim ObjReport As New SalesWorx.BO.Common.Reports
                    dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

                    Dim Currency As String = ""
                    If dtcurrency.Rows.Count > 0 Then
                        lblDecimal.Text = "N" & dtcurrency.Rows(0)("Decimal_Digits")
                        Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
                    End If
                    HCurrency.Value = Currency
                End If



                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                txtToDate.SelectedDate = Now()


                LoadOrgDetails()

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

    Private Sub SearchBtn_Click(sender As Object, e As EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            rpbFilter.Items(0).Expanded = False
            rptsect.Visible = True
            BindReport()

            HUID.Value = CType(Session("User_Access"), UserAccess).UserID
            HorgID.Value = ddlOrganization.SelectedItem.Value

            lbl_org.Text = ddlOrganization.SelectedItem.Text
            Dim collection As IList(Of RadComboBoxItem) = ddl_Van.CheckedItems

            Dim vantxt As String = ""
            Dim van As String = ""
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

            lbl_org.Text = ddlOrganization.SelectedItem.Text
            If van = "0" Then
                lbl_van.Text = "All"
            Else
                lbl_van.Text = vantxt
            End If
            HVan.Value = van

            Args.Visible = True

            lbl_Todt.Visible = True
            lbl_Totxt.Visible = True

            Dim fromdate As Date
            Dim Todate As Date

            fromdate = CDate(txtFromDate.SelectedDate)
            Todate = CDate(txtToDate.SelectedDate)

            HDate.Value = fromdate.ToString("dd-MMM-yyyy")
            HToDate.Value = Todate.ToString("dd-MMM-yyyy")

            Salestab.Tabs(0).Selected = True
            RadMultiPage21.PageViews(0).Selected = True




        Else
            rptsect.Visible = False
            'summary.InnerHtml = ""
            Args.Visible = False
        End If

    End Sub

  


  
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        rptsect.Visible = False
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

            For Each itm As RadComboBoxItem In ddl_Van.Items
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


        Else

            ddl_Van.Items.Clear()
        End If

    End Sub
    

    


   
  

    Private Sub ddl_item_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddl_item.ItemsRequested

        Dim strgency As String = ""
        If strgency = "" Then
            strgency = "0"
        End If
        Dim Objrep As New SalesWorx.BO.Common.Reports()
        Dim dt As New DataTable
        dt = Objrep.GetItemFromAgency(Err_No, Err_Desc, ddlOrganization.SelectedValue, strgency, e.Text)


        Dim ItemsPerRequest As Integer = 100 'Set the number of values to show
        Dim itemOffset As Integer = e.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
        e.EndOfItems = endOffset = dt.Rows.Count

        'Loop through the values to populate the combo box
        For i As Integer = itemOffset To endOffset - 1
            Dim item As New RadComboBoxItem()
            item.Text = dt.Rows(i).Item("Description").ToString
            item.Value = dt.Rows(i).Item("Inventory_Item_ID").ToString

            ddl_item.Items.Add(item)
            item.DataBind()
        Next

    End Sub

   

    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender
        For Each column As GridColumn In gvRep.MasterTableView.Columns
            If column.UniqueName = "TotalSalesValue" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                column.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                col.DataType = System.Type.GetType("System.Decimal")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
            ElseIf column.UniqueName = "TotalReturnValue" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Decimal")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
                col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
            ElseIf column.UniqueName = "NetSales" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Decimal")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
                col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
            End If
        Next
    End Sub

    Private Sub gvRep_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvRep.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindReport()
    End Sub
    Private Sub gvRep_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged

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



    Private Sub ddl_item_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddl_item.SelectedIndexChanged
        rptsect.Visible = False
        Args.Visible = False
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


        Dim SearchQuery As String = ""
        SearchQuery = BuildQuery()
        Dim SearchParams As New ReportParameter
        SearchParams = New ReportParameter("SearchParams", SearchQuery)

        Dim OrgId As New ReportParameter
        OrgId = New ReportParameter("OrgID", CStr(ddlOrganization.SelectedItem.Value.ToString()))

        Dim collection As IList(Of RadComboBoxItem) = ddl_Van.CheckedItems
        Dim van As String = ""
        For Each li As RadComboBoxItem In collection
            van = van & li.Value & ","
        Next
        If van = "" Then
            van = "0"
        End If
        Dim SID As New ReportParameter
        SID = New ReportParameter("SID", van)


        Dim UID As New ReportParameter
        UID = New ReportParameter("UID", objUserAccess.UserID)

        Dim FromDate As Date
        Dim ToDate As Date
        FromDate = CDate(txtFromDate.SelectedDate)
        ToDate = CDate(txtToDate.SelectedDate)
        Dim PFromDate As New ReportParameter
        PFromDate = New ReportParameter("Fromdate", FromDate.ToString("dd-MMM-yyyy"))


        Dim PToDate As New ReportParameter
        PToDate = New ReportParameter("ToDate", ToDate.ToString("dd-MMM-yyyy"))


        Dim Item As New ReportParameter
        If Not String.IsNullOrEmpty(ddl_item.SelectedValue) Then
            Item = New ReportParameter("Item", ddl_item.SelectedValue)
        Else
            Item = New ReportParameter("Item", 0)
        End If


        rview.ServerReport.SetParameters(New ReportParameter() {SearchParams, OrgId, SID, UID, PFromDate, PToDate, Item})

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
            Response.AddHeader("Content-disposition", "attachment;filename=SalesVolumebyProduct.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=SalesVolumebyProduct.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()

    End Sub

    Private Sub Salestab_TabClick(sender As Object, e As RadTabStripEventArgs) Handles Salestab.TabClick


        ' BindOverAllReport()
        If Args.Visible = True Then
            If Salestab.Tabs(1).Selected = True Then
                If ValidateInputs() Then
                    BindChart()
                End If

            End If
            If Salestab.Tabs(0).Selected = True Then
                If ValidateInputs() Then
                    BindReport()
                End If

            End If
        End If
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        rptsect.Visible = False
        Args.Visible = False
        ddl_Van.ClearCheckedItems()
        ddl_Van.Items.Clear()


        rptsect.Visible = False

        Args.Visible = False

        ddl_item.ClearSelection()
        ddl_item.Text = ""

        LoadOrgDetails()

        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Now()


    End Sub

    Protected Sub gvRep_ItemCommand(sender As Object, e As GridCommandEventArgs)
        '  BindOverAllReport()
        If e.CommandName = RadGrid.ExportToExcelCommandName Then
            gvRep.ExportSettings.Excel.Format = GridExcelExportFormat.Biff
            gvRep.ExportSettings.IgnorePaging = True
            gvRep.ExportSettings.ExportOnlyData = True
            gvRep.ExportSettings.OpenInNewWindow = True
            gvRep.ExportSettings.FileName = "SalesbySKU"
        End If

    End Sub
    Private Sub BtnExportBiffExcel_Click(sender As Object, e As EventArgs) Handles BtnExportBiffExcel.Click
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        Dim ObjReport As New SalesWorx.BO.Common.Reports


        Dim SearchQuery As String = ""
        If Not (ddlOrganization.SelectedItem.Value = "0") Then
            SearchQuery = BuildQuery()
        Else
            MessageBoxValidation("Select an organization.", "Validation")
            Exit Sub
        End If





        Dim collection As IList(Of RadComboBoxItem) = ddl_Van.CheckedItems

        Dim van As String = ""
        Dim vantxt As String = ""
        For Each li As RadComboBoxItem In collection
            van = van & li.Value & ","
            vantxt = vantxt & li.Text & ","
        Next

        If vantxt.Trim() <> "" Then
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

        Args.Visible = True

     
        Dim dt As New DataTable
        Dim dt_s As New DataTable



        Dim Fromdate As Date
        Dim Todate As Date
        Fromdate = CDate(txtFromDate.SelectedDate)
        Todate = CDate(txtToDate.SelectedDate)

        dt_s = ObjReport.GetSalesVolumeByProduct(Err_No, Err_Desc, SearchQuery, ddlOrganization.SelectedItem.Value, van, CType(Session("User_Access"), UserAccess).UserID)
        Dim tblData_s As New DataTable
        tblData_s = dt_s.DefaultView.ToTable(False, "VAN", "TotalSalesQty", "TotalSalesValue", "TotalReturnQty", "TotalReturnValue", "NetSalesQty", "NetSales")
        For Each col_s In tblData_s.Columns
            If col_s.ColumnName = "TotalSalesQty" Then
                col_s.ColumnName = "Total Sales Qty"
            End If

            If col_s.ColumnName = "TotalSalesValue" Then
                col_s.ColumnName = "Total Sales Value"
            End If
            If col_s.ColumnName = "TotalReturnQty" Then
                col_s.ColumnName = " Total Return Qty"
            End If
            If col_s.ColumnName = "TotalReturnValue" Then
                col_s.ColumnName = "Total Return Value"
            End If
            If col_s.ColumnName = "NetSalesQty" Then
                col_s.ColumnName = "Net Sales Qty"
            End If
            If col_s.ColumnName = "NetSales" Then
                col_s.ColumnName = "Net Sales"
            End If

        Next


        dt = ObjReport.GetSalesVolumeByProductDetails(Err_No, Err_Desc, SearchQuery, ddlOrganization.SelectedItem.Value, van, CType(Session("User_Access"), UserAccess).UserID)
        Dim tblData As New DataTable
        tblData = dt.DefaultView.ToTable(False, "VAN", "Product", "Item_Code", "UOM", "TotalSalesQty", "TotalSalesValue", "TotalReturnQty", "TotalReturnValue", "NetSalesQty", "NetSales")

        For Each col In tblData.Columns

            If col.ColumnName = "Item_Code" Then
                col.ColumnName = "Product Code"
            End If

            If col.ColumnName = "TotalSalesQty" Then
                col.ColumnName = "Total Sales Qty"
            End If

            If col.ColumnName = "TotalSalesValue" Then
                col.ColumnName = "Total Sales Value"
            End If
            If col.ColumnName = "TotalReturnQty" Then
                col.ColumnName = " Total Return Qty"
            End If
            If col.ColumnName = "TotalReturnValue" Then
                col.ColumnName = "Total Return Value"
            End If
            If col.ColumnName = "NetSalesQty" Then
                col.ColumnName = "Net Sales Qty"
            End If
            If col.ColumnName = "NetSales" Then
                col.ColumnName = "Net Sales"
            End If

        Next


        If tblData.Rows.Count > 0 Then


            Using package As New ExcelPackage()
                ' add a new worksheet to the empty workbook
                'Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
                'Worksheet.Cells("A3").LoadFromDataTable(tblData, True)
                'Worksheet.Cells("A1").Value = "From Date:"
                'Worksheet.Cells("B1").Value = Fromdate.ToString("dd-MMM-yyyy")
                'Worksheet.Cells("A2").Value = "To Date:"
                'Worksheet.Cells("B2").Value = Todate.ToString("dd-MMM-yyyy")
                'Worksheet.Cells("A3").Value = "Product"
                'Worksheet.Cells("B3").Value = "UOM"
                'Worksheet.Cells("C3").Style.Numberformat.Format = "#,###.00"
                'Worksheet.Cells("C3").Value = "Total Qty Sold"
                'Worksheet.Cells("D3").Value = "Total Sales"
                'Worksheet.Cells("E3").Value = "Total Qty Returned"
                'Worksheet.Cells("F3").Value = "Total Returns"
                'Worksheet.Cells("G3").Value = "Net Sales"
                'Worksheet.Column(3).Style.Numberformat.Format = "#,###.00"
                'Worksheet.Column(4).Style.Numberformat.Format = "#,###.00"
                'Worksheet.Column(5).Style.Numberformat.Format = "#,###.00"
                'Worksheet.Column(6).Style.Numberformat.Format = "#,###.00"
                'Worksheet.Column(7).Style.Numberformat.Format = "#,###.00"

                'Worksheet.Cells.AutoFitColumns()
                Dim Worksheet_s As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
                Worksheet_s.Cells("A1").LoadFromDataTable(tblData_s, True)
                Worksheet_s.Column(2).Style.Numberformat.Format = "#,##0.0###"
                Worksheet_s.Column(3).Style.Numberformat.Format = "#,##0.00"
                Worksheet_s.Column(4).Style.Numberformat.Format = "#,##0.0###"
                Worksheet_s.Column(5).Style.Numberformat.Format = "#,##0.00"
                Worksheet_s.Column(6).Style.Numberformat.Format = "#,##0.0###"
                Worksheet_s.Column(7).Style.Numberformat.Format = "#,##0.00"
                Worksheet_s.Cells.AutoFitColumns()

                Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet2")
                Worksheet.Cells("A1").LoadFromDataTable(tblData, True)
                Worksheet.Column(5).Style.Numberformat.Format = "#,##0.0###"
                Worksheet.Column(6).Style.Numberformat.Format = "#,##0.00"
                Worksheet.Column(7).Style.Numberformat.Format = "#,##0.0###"
                Worksheet.Column(8).Style.Numberformat.Format = "#,##0.00"
                Worksheet.Column(9).Style.Numberformat.Format = "#,##0.0###"
                Worksheet.Column(10).Style.Numberformat.Format = "#,##0.00"
                Worksheet.Cells.AutoFitColumns()




                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename= Rep_SalesVolumebyProduct.xlsx")

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
    Function ValidateInputs() As Boolean
        Try
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
                ' LoadCurrency()

                bretval = True
                Return bretval
            Else
                MessageBoxValidation("Please select the organization", "Validation")
                Return bretval
            End If


        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try
    End Function
    Private Sub BindReport()
        Try
            Dim SearchQuery As String = ""
            If Not (ddlOrganization.SelectedItem.Value = "0") Then
                SearchQuery = BuildQuery()
            Else
                MessageBoxValidation("Select an organization.", "Validation")
                Exit Sub
            End If

            rpbFilter.Items(0).Expanded = False


            Dim collection As IList(Of RadComboBoxItem) = ddl_Van.CheckedItems

            Dim van As String = ""
            Dim vantxt As String = ""
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","
                vantxt = vantxt & li.Text & ","
            Next

            If vantxt.Trim() <> "" Then
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

            Args.Visible = True

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetSalesVolumeByProductDetails(Err_No, Err_Desc, SearchQuery, ddlOrganization.SelectedItem.Value, van, CType(Session("User_Access"), UserAccess).UserID)

            '   dt.DefaultView.Sort = "VAN DESC"

            gvRep.DataSource = dt
            gvRep.DataBind()
            gvRep.Visible = True

        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub
    Private Function BuildQuery()

        Dim SearchQuery As String = ""
        Try
            Dim collection As IList(Of RadComboBoxItem) = ddl_Van.CheckedItems

            Dim van As String = ""
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","
            Next

            If van = "" Then
                van = "0"
            End If

            ObjCustomer = New Customer()
            ObjCommon = New SalesWorx.BO.Common.Common()

            If txtFromDate.DateInput.Text <> "" Then
                SearchQuery = SearchQuery & " And H.Start_Time >='" & CDate(txtFromDate.SelectedDate) & " 00:00:00 '"
            End If
            If txtToDate.DateInput.Text <> "" Then
                SearchQuery = SearchQuery & " And H.Start_Time <='" & CDate(txtToDate.SelectedDate) & " 23:59:59 '"
            End If

            ''If Not ddlPaymentType.SelectedItem.Value = "0" Then
            ''    SearchQuery = "AND CN.Cash_cust='" + ddlPaymentType.SelectedItem.Value + "' " + SearchQuery
            ''End If

            If Not ddl_item.SelectedValue Is Nothing Then
                If Not ddl_item.SelectedValue = "0" And Not ddl_item.SelectedValue = "" Then
                    SearchQuery = " AND V.inventory_item_ID='" + ddl_item.SelectedValue + "' " + SearchQuery
                End If
            End If




            Return SearchQuery
        Catch ex As Exception
            If Err_Desc IsNot Nothing Then
                log.Error(Err_Desc)
            Else
                log.Error(GetExceptionInfo(ex))
            End If
            Err_No = "74067"
            '  Err_Desc = ex.Message
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
        Finally
            ObjCustomer = Nothing
        End Try
    End Function

    Private Sub BindChart()
        Try

            Dim SearchQuery As String = ""
            If Not (ddlOrganization.SelectedItem.Value = "0") Then
                SearchQuery = BuildQuery()
            Else
                MessageBoxValidation("Select an organization.", "Validation")
                Exit Sub
            End If

            Dim collection As IList(Of RadComboBoxItem) = ddl_Van.CheckedItems

            Dim van As String = ""
            Dim vantxt As String = ""
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","
                vantxt = vantxt & li.Text & ","
            Next

            If vantxt.Trim() <> "" Then
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

            Args.Visible = True

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetSalesVolumeByProduct(Err_No, Err_Desc, SearchQuery, ddlOrganization.SelectedItem.Value, van, CType(Session("User_Access"), UserAccess).UserID)



            gvRep_Summary.DataSource = dt
            gvRep_Summary.DataBind()


            dt.DefaultView.RowFilter = " VAN <> 'All Vans'"
            ' dt.DefaultView.Sort = "Incentive_Month ASC"

            Chart.DataSource = dt
            Chart.DataBind()
           

            Chartwrapper.Style.Add("width", "1000px")

            If dt.Rows.Count > 0 Then
                Chart.Visible = True
            Else
                Chart.Visible = False
            End If
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub

    'Private Sub gvRep_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles gvRep.ItemDataBound
    '    Try
    '        If TypeOf e.Item Is GridDataItem Then
    '            Dim item As GridDataItem = TryCast(e.Item, GridDataItem)
    '            If item IsNot Nothing Then
    '                item.Cells(6).Text = FormatNumber(CDbl(item.Cells(6).Text), lblDecimal.Text.Replace("N", ""))
    '                item.Cells(8).Text = FormatNumber(CDbl(item.Cells(8).Text), lblDecimal.Text.Replace("N", ""))
    '                item.Cells(10).Text = FormatNumber(CDbl(item.Cells(8).Text), lblDecimal.Text.Replace("N", ""))
    '            End If
    '        End If


    '        '' For Footer
    '        'If TypeOf e.Item Is GridFooterItem Then
    '        '    Dim item As GridFooterItem = TryCast(e.Item, GridFooterItem)

    '        '    If item IsNot Nothing Then
    '        '        If Not String.IsNullOrEmpty(lblDecimal.Text) Then
    '        '            ' item("SALES").Text = FormatNumber(item("SALES").Text, lblDecimal.Text.Replace("N", ""))
    '        '            item("TotalSalesValue").Text = FormatNumber(item("TotalSalesValue").Text, lblDecimal.Text.Replace("N", ""))
    '        '            'item("Returns").Text = FormatNumber(item("Returns").Text, lblDecimal.Text.Replace("N", ""))
    '        '            ' item("ReturnValue").Text = FormatNumber(item("ReturnValue").Text, lblDecimal.Text.Replace("N", ""))
    '        '            'item("SaleValue").Attributes.Add("align", "right")
    '        '            ' item("ReturnValue").Attributes.Add("align", "right")
    '        '        End If

    '        '        If item("Product") IsNot Nothing Then
    '        '            item("Product").Text = "Grand Total"
    '        '        End If
    '        '    End If

    '        'End If

    '        '' For Group Footer

    '        'If TypeOf e.Item Is GridGroupFooterItem Then
    '        '    Dim item As GridGroupFooterItem = TryCast(e.Item, GridGroupFooterItem)
    '        '    If item IsNot Nothing Then
    '        '        If Not String.IsNullOrEmpty(lblDecimal.Text) Then
    '        '            item("TotalSalesValue").Text = FormatNumber(item("TotalSalesValue").Text, lblDecimal.Text.Replace("N", ""))
    '        '            ' item("ReturnValue").Text = FormatNumber(item("ReturnValue").Text, lblDecimal.Text.Replace("N", ""))
    '        '        End If


    '        '        If item("Product") IsNot Nothing Then  '' Finding Group Footers
    '        '            If IsNumeric(item.GroupIndex) Then
    '        '                item("Product").Text = "Outlet Total"
    '        '                item.Attributes.Add("id", "clsOutlet")
    '        '            Else
    '        '                item("Product").Text = "Total"
    '        '                item.Attributes.Add("id", "clsAgency")
    '        '            End If

    '        '        End If
    '        '    End If
    '        'End If

    '    Catch ex As Exception
    '        log.Error(ex.Message)
    '    End Try
    'End Sub

    Private Sub gvRep_Summary_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvRep_Summary.PageIndexChanged
        BindSummaryGrid()
    End Sub

    Private Sub gvRep_Summary_SortCommand(sender As Object, e As GridSortCommandEventArgs) Handles gvRep_Summary.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindSummaryGrid()
    End Sub

    Private Sub BindSummaryGrid()
        Try

            Dim SearchQuery As String = ""
            If Not (ddlOrganization.SelectedItem.Value = "0") Then
                SearchQuery = BuildQuery()
            Else
                MessageBoxValidation("Select an organization.", "Validation")
                Exit Sub
            End If

            Dim collection As IList(Of RadComboBoxItem) = ddl_Van.CheckedItems

            Dim van As String = ""
            Dim vantxt As String = ""
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","
                vantxt = vantxt & li.Text & ","
            Next

            If vantxt.Trim() <> "" Then
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

            Args.Visible = True

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetSalesVolumeByProduct(Err_No, Err_Desc, SearchQuery, ddlOrganization.SelectedItem.Value, van, CType(Session("User_Access"), UserAccess).UserID)



            gvRep_Summary.DataSource = dt
            gvRep_Summary.DataBind()


           
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub

End Class