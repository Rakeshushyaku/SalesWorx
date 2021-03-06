﻿Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Imports OfficeOpenXml
Public Class Rep_SR
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer
    Public ENABLE_PRINT_RETURN_BO As Boolean
    Private ReportPath As String = "SR"
    Private Const PageID As String = "P407"
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

        If Not IsPostBack Then

           
            ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
            Try





                Dim dtPaymentType As New DataTable

                If dtPaymentType.Rows.Count > 0 Then
                    dtPaymentType.Rows.Clear()
                End If

                dtPaymentType.Columns.Add("Value", GetType(String))
                dtPaymentType.Columns.Add("Description", GetType(String))

                dtPaymentType.Rows.Add("0", "Both")
                dtPaymentType.Rows.Add("Y", "Cash")
                dtPaymentType.Rows.Add("N", "Credit")

                ddlPaymentType.DataSource = dtPaymentType
                ddlPaymentType.DataValueField = "Value"
                ddlPaymentType.DataTextField = "Description"
                ddlPaymentType.DataBind()

                ObjCommon = New SalesWorx.BO.Common.Common()


                ddl_Status.DataSource = ObjCommon.GetDocStatus(Err_No, Err_Desc, "'R'")
                ddl_Status.DataTextField = "Description"
                ddl_Status.DataValueField = "Status"
                ddl_Status.DataBind()

                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))
                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                End If
                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)

                txtToDate.SelectedDate = Now()
                LoadorgDetails()



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

    Private Function BuildQuery() As String
        Dim SearchQuery As String = ""
        Try
            ObjCustomer = New Customer()
            ObjCommon = New SalesWorx.BO.Common.Common()
            Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems

            Dim van As String = ""
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","
            Next

            If van = "" Then
                van = "0"
            End If

            If txtRefNo.Text <> "" Then
                SearchQuery = SearchQuery & " And A.Orig_Sys_Document_Ref like '%" & Utility.ProcessSqlParamString(txtRefNo.Text) & "%'"
            End If

            If van <> "0" Then
                SearchQuery = SearchQuery & " And A.Created_By in(Select item from SplitQuotedString('" & van & "'))"
            Else
                SearchQuery = SearchQuery & " And A.Created_By in (" & ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID) & ")"
            End If

            If Not String.IsNullOrEmpty(ddlCustomer.SelectedValue) Then
                SearchQuery = SearchQuery & " AND (LTRIM(STR(B.Customer_ID)) + '$' + LTRIM(STR(B.Site_Use_ID)))='" & ddlCustomer.SelectedValue & "' "
            End If

            If txtFromDate.DateInput.Text <> "" Then
                SearchQuery = SearchQuery & " And A.Creation_Date >= '" & CDate(txtFromDate.SelectedDate) & "'"

            End If
            If txtToDate.DateInput.Text <> "" Then
                SearchQuery = SearchQuery & " And A.Creation_Date <= '" & CDate(txtToDate.SelectedDate) & " 23:59:59'"
            End If

            If Not ddlPaymentType.SelectedItem.Value = "0" Then
                SearchQuery = "AND CN.Cash_cust='" + ddlPaymentType.SelectedItem.Value + "' " + SearchQuery
            End If

            Dim St As String = ""
            For Each li As RadComboBoxItem In ddl_Status.CheckedItems
                St = St & li.Value & ","
            Next

            If St.Trim <> "" Then
                St = St.Substring(0, St.Length - 1)
            End If

            If St.Trim <> "" Then
                SearchQuery = SearchQuery & " And A.Order_Status in(select item from SplitQuotedString('" & St & "'))"
            End If

            log.Debug(SearchQuery)
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
    Private Sub ddl_Customer_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddlCustomer.ItemsRequested

        Dim Objrep As New SalesWorx.BO.Common.Reports()

        Dim selectedPayment As String = ddlPaymentType.SelectedValue

        Dim dt As New DataTable

        If dt.Rows.Count > 0 Then
            dt.Rows.Clear()
        End If

        'dt = Objrep.GetCustomerfromOrgtext(Err_No, Err_Desc, ddlOrganization.SelectedValue, e.Text)

        dt = Objrep.GetCustomerfromPaymentType(Err_No, Err_Desc, ddlOrganization.SelectedValue, e.Text, selectedPayment)

        Dim ItemsPerRequest As Integer = 100 'Set the number of values to show
        Dim itemOffset As Integer = e.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
        e.EndOfItems = endOffset = dt.Rows.Count

        'Loop through the values to populate the combo box
        For i As Integer = itemOffset To endOffset - 1
            Dim item As New RadComboBoxItem()
            item.Text = dt.Rows(i).Item("Customer").ToString
            item.Value = dt.Rows(i).Item("CustomerID").ToString

            ddlCustomer.Items.Add(item)
            item.DataBind()
        Next
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

        Dim SearchParams As String = ""
        SearchParams = BuildQuery()
        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
        Dim Searchvalue As New ReportParameter
        Searchvalue = New ReportParameter("SearchParams", CStr(IIf(SearchParams Is Nothing, "", SearchParams)))

        Dim VisitID As New ReportParameter
        VisitID = New ReportParameter("VisitID", "0")

        Dim OrgID As New ReportParameter
        OrgID = New ReportParameter("OrgID", CStr(ddlOrganization.SelectedValue.ToString()))

        Dim OrgName As New ReportParameter
        OrgName = New ReportParameter("OrgName", CStr(IIf(ddlOrganization.SelectedIndex = 0, "0", ddlOrganization.SelectedItem.Text.Trim())))

        Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems

        Dim van As String = ""
        For Each li As RadComboBoxItem In collection
            van = van & li.Value & ","
        Next

        If van = "" Then
            van = "0"
        End If

        Dim SID As New ReportParameter
        SID = New ReportParameter("Van", van)

        Dim RefNo As New ReportParameter
        RefNo = New ReportParameter("RefNo", txtRefNo.Text)

        Dim CustType As New ReportParameter
        CustType = New ReportParameter("CustType", ddlPaymentType.SelectedItem.Text)

        Dim Customer As New ReportParameter
        If Not String.IsNullOrEmpty(ddlCustomer.SelectedValue) Then
            Dim ids() As String
            ids = ddlCustomer.SelectedValue.Split("$")
            Dim custdt As New DataTable
            custdt = (New SalesWorx.BO.Common.Customer).GetCustomerShipDeatils(Err_No, Err_Desc, ids(0), ids(1))
            If custdt.Rows.Count > 0 Then
                Dim strcust As String
                strcust = custdt.Rows(0)("Customer_no").ToString & " - " & custdt.Rows(0)("Customer_name").ToString
                Customer = New ReportParameter("Customer", strcust)
            End If
        Else
            Customer = New ReportParameter("Customer", "All")
        End If




        Dim FromDate As New ReportParameter
        FromDate = New ReportParameter("FromDate", CStr(CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")))

        Dim ToDate As New ReportParameter
        ToDate = New ReportParameter("Todate", CStr(CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")))

        rview.ServerReport.SetParameters(New ReportParameter() {Searchvalue, OrgID, VisitID, OrgName, SID, Customer, FromDate, ToDate, RefNo, CustType})

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
            Response.AddHeader("Content-disposition", "attachment;filename=SalesReturns.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=SalesReturns.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()
    End Sub

    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            gvRep.Visible = True
            BindReport()
        Else
            summary.InnerHtml = ""
            gvRep.Visible = False
            Args.Visible = False
        End If

    End Sub
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


            lbl_org.Text = ddlOrganization.SelectedItem.Text
            lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")
            If Not String.IsNullOrEmpty(ddlCustomer.SelectedValue) Then
                Dim ids() As String
                ids = ddlCustomer.SelectedValue.Split("$")
                Dim custdt As New DataTable
                custdt = (New SalesWorx.BO.Common.Customer).GetCustomerShipDeatils(Err_No, Err_Desc, ids(0), ids(1))
                If custdt.Rows.Count > 0 Then
                    lbl_Customer.Text = custdt.Rows(0)("Customer_no").ToString & " - " & custdt.Rows(0)("Customer_name").ToString
                End If
            Else
                lbl_Customer.Text = "All"
            End If

            Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems

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
            dt = ObjReport.GetSR(Err_No, Err_Desc, SearchQuery, ddlOrganization.SelectedItem.Value, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))
            gvRep.DataSource = dt
            gvRep.DataBind()


            Dim dtcurrency As DataTable
            dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
            Dim Currency As String = ""
            If dtcurrency.Rows.Count > 0 Then
                Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
            End If

            Dim StrSummary As String = ""

            Dim sum = dt.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("OrderAmount")))

            StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>Total Returns " & Currency & "<div class='text-primary'>" & Format(Math.Abs(sum), lblDecimal.Text) & "</div></div></div>"


            Dim tot_vat As Decimal = 0
            If dt.Rows.Count > 0 Then
                tot_vat = Convert.ToDecimal(dt.Compute("SUM(VatAmount)", String.Empty))
                StrSummary = StrSummary & "<div class='col-sm-3'><div class='widgetblk'>" & "Total VAT Amount" & Currency & "<div class='text-primary'>" & Format(tot_vat, lblDecimal.Text) & "</div></div></div>"



            End If


            summary.InnerHtml = StrSummary

            


        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub

    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender
        For Each column As GridColumn In gvRep.MasterTableView.Columns
            If column.UniqueName = "OrderAmount" Then
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
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged

        LoadorgDetails()
    End Sub
    Sub LoadorgDetails()
        If Not (ddlOrganization.SelectedItem.Value = "0") Then
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New SalesWorx.BO.Common.Common()
            ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())
            ddlVan.DataBind()

            For Each itm As RadComboBoxItem In ddlVan.Items
                itm.Checked = True
            Next


            'ddlCustomer.DataSource = ObjCommon.GetCustomerByCriteria(Err_No, Err_Desc, ddlOrganization.SelectedValue)
            'ddlCustomer.DataBind()
            'ddlCustomer.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))

            Dim dtcurrency As DataTable
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

            Dim Currency As String = ""
            If dtcurrency.Rows.Count > 0 Then
                Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
                lblDecimal.Text = "N" & dtcurrency.Rows(0)("Decimal_Digits")
            End If


        End If
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        ddlVan.ClearCheckedItems()
        ddlVan.Items.Clear()
        LoadorgDetails()
        ddlCustomer.ClearSelection()
        ddlCustomer.Text = ""
        txtRefNo.Text = ""
        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Now()
        summary.InnerText = ""
        gvRep.Visible = False
        Args.Visible = False
    End Sub

    Protected Sub ddlPaymentType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPaymentType.SelectedIndexChanged

        ddlCustomer.ClearSelection()

    End Sub
    Private Sub BtnExportBiffExcel_Click(sender As Object, e As EventArgs) Handles BtnExportBiffExcel.Click
        Dim SearchQuery As String
        SearchQuery = BuildQuery()
        Dim tblData As New DataTable
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        Dim ObjReport As New SalesWorx.BO.Common.Reports


        Dim dt As New DataTable
        dt = ObjReport.GetSR(Err_No, Err_Desc, SearchQuery, ddlOrganization.SelectedItem.Value, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))
        tblData = dt.DefaultView.ToTable(False, "Orig_Sys_Document_Ref", "Creation_Date", "SalesRep_Name", "Customer_No", "Customer", "OrderAmount", "VatAmount", "Order_Status", "Customer_Ref_No")

        If tblData.Rows.Count > 0 Then

            Using package As New ExcelPackage()
                ' add a new worksheet to the empty workbook
                Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
                Worksheet.Cells("A1").LoadFromDataTable(tblData, True)
                Worksheet.Column(2).Style.Numberformat.Format = "dd-MMM-yyyy"
                Worksheet.Column(6).Style.Numberformat.Format = "#,###.00"
                Worksheet.Column(7).Style.Numberformat.Format = "#,###.00"

                Worksheet.Cells.AutoFitColumns()

                Worksheet.Cells("A1").Value = "Ref No"
                Worksheet.Cells("B1").Value = "Creation Date"
                Worksheet.Cells("C1").Value = "Van"
                Worksheet.Cells("D1").Value = "Customer No"
                Worksheet.Cells("E1").Value = "Customer Name"
                Worksheet.Cells("F1").Value = "Amount"
                Worksheet.Cells("G1").Value = "Vat Amount"
                Worksheet.Cells("H1").Value = "Status"
                Worksheet.Cells("I1").Value = "Customer Ref No."

                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename= Returns.xlsx")

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
End Class