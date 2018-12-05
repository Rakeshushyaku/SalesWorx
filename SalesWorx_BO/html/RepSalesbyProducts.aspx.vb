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
Imports System.Globalization

Partial Public Class RepSalesbyProducts
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer
    Private ReportPath As String = "SalesbyProducts"
    Dim dv As New DataView
    Dim dtCust As New DataTable
    Private Const PageID As String = "P390"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)



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
                dtCust.Columns.Add("ID")
                dtCust.Columns.Add("Desc")
                ViewState("dtCust") = dtCust

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
        Else
            LoadProdcut()
        End If
    End Sub
    Private Sub ddl_Customer_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddlCustomer.ItemsRequested

        Try



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
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try
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

        Try


            Dim SearchParams As String = ""
            SearchParams = BuildQuery()
            Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
            rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim Searchvalue As New ReportParameter
            Searchvalue = New ReportParameter("SearchParams", CStr(IIf(SearchParams Is Nothing, "", SearchParams)))

            Dim VisitID As New ReportParameter
            VisitID = New ReportParameter("VisitID", "0")

            Dim OrgID As New ReportParameter
            OrgID = New ReportParameter("OrgID", CStr(ddlOrganization.SelectedValue.ToString()))

            Dim FromDate As New ReportParameter
            FromDate = New ReportParameter("FromDate", CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"))

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

            Dim ToDate As New ReportParameter
            ToDate = New ReportParameter("ToDate", CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))

            Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems

            Dim van As String = ""
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","
            Next

            If van = "" Then
                van = "0"
            End If

            Dim SalesRep_ID As New ReportParameter
            SalesRep_ID = New ReportParameter("SID", van)

            Dim OrgName As New ReportParameter
            OrgName = New ReportParameter("OrgName", CStr(IIf(ddlOrganization.SelectedIndex = 0, "0", ddlOrganization.SelectedItem.Text.Trim())))


            Dim UID As New ReportParameter
            UID = New ReportParameter("UID", objUserAccess.UserID)



            rview.ServerReport.SetParameters(New ReportParameter() {Searchvalue, OrgID, VisitID, SalesRep_ID, FromDate, ToDate, OrgName, Customer, RefNo, CustType, UID})

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
                Response.AddHeader("Content-disposition", "attachment;filename=SalesbyProducts.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=SalesbyProducts.xls")
                Response.AddHeader("Content-Length", bytes.Length)
            End If
            Response.OutputStream.Write(bytes, 0, bytes.Length)
            Response.OutputStream.Flush()
            Response.OutputStream.Close()
            Response.Flush()
            Response.Close()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender
        For Each column As GridColumn In gvRep.MasterTableView.Columns
            If column.UniqueName = "Unit_Selling_Price" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
            ElseIf column.UniqueName = "Net_Total_Price" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
                col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
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
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            gvRep.Visible = True
            BindReport()
        Else
            summary.InnerText = ""
            gvRep.Visible = False
            Args.Visible = False
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


            lbl_org.Text = ddlOrganization.SelectedItem.Text
            lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_OrderRefno.Text = txtRefNo.Text
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
            Dim dt1 As New DataTable

            'dt = ObjReport.GetOrderListing(Err_No, Err_Desc, SearchQuery, ddlOrganization.SelectedItem.Value)
            dt = ObjReport.GetSalesByProduct(Err_No, Err_Desc, SearchQuery, ddlOrganization.SelectedItem.Value, ddlPaymentType.SelectedItem.Value, van, CType(Session("User_Access"), UserAccess).UserID)
            dt1 = ObjReport.GetSalesByProductVatamount(Err_No, Err_Desc, SearchQuery, ddlOrganization.SelectedItem.Value, ddlPaymentType.SelectedItem.Value, van, CType(Session("User_Access"), UserAccess).UserID)

            'GetSalesByProductVatamount
            gvRep.DataSource = dt
            gvRep.DataBind()

            Dim StrSummary As String = ""
            Dim TotolSales As Decimal = 0
            Dim TotolReturns As Decimal = 0
            Dim NetSales As Decimal = 0
            Dim NetSales1 As Decimal = 0



            Dim dtcurrency As DataTable
            dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
            Dim Currency As String = ""
            If dtcurrency.Rows.Count > 0 Then
                Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
            End If



            Dim query = (From UserEntry In dt _
                        Where UserEntry.Field(Of String)("SType") = "S" _
                       Group UserEntry By key = UserEntry.Field(Of String)("SType") Into Group _
                       Select PayMode = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("Net_Total_Price"))).ToList
            If query.Count > 0 Then
                For Each x In query
                    StrSummary = StrSummary & "<div class='col-sm-3'><div class='widgetblk'>" & "Total  Sales " & Currency & "<div class='text-primary'>" & Format(x.Total, lblDecimal.Text) & "</div></div></div>"
                    TotolSales = TotolSales + x.Total
                Next
            Else
                StrSummary = StrSummary & "<div class='col-sm-3'><div class='widgetblk'>" & "Total Sales" & Currency & "<div class='text-primary'>" & Format(0, lblDecimal.Text) & "</div></div></div>"
            End If



            Dim query1 = (From UserEntry In dt _
                        Where UserEntry.Field(Of String)("RType") = "R" _
                       Group UserEntry By key = UserEntry.Field(Of String)("RType") Into Group _
                       Select PayMode = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("Net_Total_Price"))).ToList


            If query1.Count > 0 Then
                For Each x In query1
                    StrSummary = StrSummary & "<div class='col-sm-3'><div class='widgetblk'>" & "Total Returns" & Currency & "<div class='text-primary'>" & Format(Math.Abs(x.Total), lblDecimal.Text) & "</div></div></div>"
                    TotolReturns = TotolReturns + x.Total
                Next
            Else
                StrSummary = StrSummary & "<div class='col-sm-3'><div class='widgetblk'>" & "Total Returns" & Currency & "<div class='text-primary'>" & Format(0, lblDecimal.Text) & "</div></div></div>"
            End If


            'task8
            'Dim query12 = (From UserEntry In dt _
            '          Where UserEntry.Field(Of String)("RType") = "R" _
            '         Group UserEntry By key = UserEntry.Field(Of String)("RType") Into Group _
            '         Select PayMode = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("Net_Total_Price"))).ToList



            'If query12.Count > 0 Then
            '    For Each x In query12
            '        StrSummary = StrSummary & "<div class='col-sm-3'><div class='widgetblk'>" & "Total Returns" & Currency & "<div class='text-primary'>" & Format(Math.Abs(x.Total), lblDecimal.Text) & "</div></div></div>"
            '        TotolReturns = TotolReturns + x.Total
            '    Next
            'Else
            '    StrSummary = StrSummary & "<div class='col-sm-3'><div class='widgetblk'>" & "Total Returns" & Currency & "<div class='text-primary'>" & Format(0, lblDecimal.Text) & "</div></div></div>"
            'End If

            NetSales1 = (dt1.Rows(0).Item(0).ToString())

            StrSummary = StrSummary & "<div class='col-sm-3'><div class='widgetblk'>" & "Total Vat Amount" & Currency & "<div class='text-primary'>" & Format(NetSales1, lblDecimal.Text) & "</div></div></div>"



            NetSales = TotolSales - Math.Abs(TotolReturns)


           
            summary.InnerHtml = StrSummary
            summary.InnerHtml = summary.InnerHtml & "<div class='col-sm-3'><div class='widgetblk'>" & "Net  Sales" & Currency & "<div class='text-primary'>" & Format(NetSales, lblDecimal.Text) & "</div></div></div>"




            Dim ClientCode As String
            ClientCode = (New SalesWorx.BO.Common.Common).GetAppConfig(Err_No, Err_Desc, "CLIENT_CODE")
            If ClientCode = "GYMA" Then
                gvRep.MasterTableView.Columns.FindByUniqueName("CH").Visible = True
                gvRep.MasterTableView.Columns.FindByUniqueName("Route").Visible = False
                gvRep.MasterTableView.Columns.FindByUniqueName("erprefno").Visible = True
                gvRep.MasterTableView.Columns.FindByUniqueName("TrnsType").Visible = False
                gvRep.MasterTableView.Columns.FindByUniqueName("CusotmerCode").Visible = False
                gvRep.MasterTableView.Columns.FindByUniqueName("Category").Visible = True
                gvRep.MasterTableView.Columns.FindByUniqueName("Catdesc").Visible = True
                gvRep.MasterTableView.Columns.FindByUniqueName("UP").Visible = False
                gvRep.MasterTableView.Columns.FindByUniqueName("tot").Visible = True
                gvRep.MasterTableView.Columns.FindByUniqueName("CG").Visible = False

                gvRep.MasterTableView.Columns.FindByUniqueName("CH").HeaderText = "Chain ID"
                gvRep.MasterTableView.Columns.FindByUniqueName("erprefno").HeaderText = "Invoice ID"
                gvRep.MasterTableView.Columns.FindByUniqueName("Category").HeaderText = "Prod Cat."
                gvRep.MasterTableView.Columns.FindByUniqueName("Catdesc").HeaderText = "Category Description"
                gvRep.MasterTableView.Columns.FindByUniqueName("tot").HeaderText = "Sale Amount (b4 Tax)"

                gvRep.MasterTableView.Columns.FindByUniqueName("outletcode").HeaderText = "Customer No."
                gvRep.MasterTableView.Columns.FindByUniqueName("Outletname").HeaderText = "Customer Name"

            Else
                gvRep.MasterTableView.Columns.FindByUniqueName("CH").Visible = False
                gvRep.MasterTableView.Columns.FindByUniqueName("Route").Visible = True
                gvRep.MasterTableView.Columns.FindByUniqueName("erprefno").Visible = False
                gvRep.MasterTableView.Columns.FindByUniqueName("TrnsType").Visible = True
                gvRep.MasterTableView.Columns.FindByUniqueName("CusotmerCode").Visible = True
                gvRep.MasterTableView.Columns.FindByUniqueName("Category").Visible = False
                gvRep.MasterTableView.Columns.FindByUniqueName("Catdesc").Visible = False
                gvRep.MasterTableView.Columns.FindByUniqueName("UP").Visible = True
                gvRep.MasterTableView.Columns.FindByUniqueName("tot").Visible = False
                gvRep.MasterTableView.Columns.FindByUniqueName("CG").Visible = True

            End If

            'Dim dtcurrency As DataTable
            dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
            'Dim Currency As String = ""
            If dtcurrency.Rows.Count > 0 Then
                Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
            End If

        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub

    Private Function BuildQuery()

        Dim SearchQuery As String = ""
        Try
            Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems

            Dim van As String = ""
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","
            Next

            If van = "" Then
                van = "0"
            End If

            ObjCustomer = New Customer()
            ObjCommon = New SalesWorx.BO.Common.Common()
            If van <> "0" Then
                SearchQuery = " And H.Created_By in(select item from SplitQuotedString('" & van & "'))"
            Else
                SearchQuery = " And H.Created_By in (" & ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID) & ")"
            End If

            If txtRefNo.Text <> "" Then
                SearchQuery = SearchQuery & " And H.Orig_Sys_Document_Ref like '%" & Utility.ProcessSqlParamString(txtRefNo.Text) & "%'"
            End If

            If Not String.IsNullOrEmpty(ddlCustomer.SelectedValue) Then
                SearchQuery = SearchQuery & " AND (LTRIM(STR(B.Customer_ID)) + '$' + LTRIM(STR(B.Site_Use_ID)))='" & ddlCustomer.SelectedValue & "' "
            End If


            If txtFromDate.DateInput.Text <> "" Then
                SearchQuery = SearchQuery & " And H.Start_Time >=convert(datetime,'" & CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy") & "',103)"
            End If
            If txtToDate.DateInput.Text <> "" Then
                SearchQuery = SearchQuery & " And H.Start_Time <= convert(datetime,'" & CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy") & " 23:59:59',103)"
            End If

            If Not ddlPaymentType.SelectedItem.Value = "0" Then
                SearchQuery = "AND CN.Cash_cust='" + ddlPaymentType.SelectedItem.Value + "' " + SearchQuery
            End If

            If Not ddlPaymentType.SelectedItem.Value = "0" Then
                SearchQuery = "AND CN.Cash_cust='" + ddlPaymentType.SelectedItem.Value + "' " + SearchQuery
            End If

            dtCust = CType(ViewState("dtCust"), DataTable)
            Dim Invid As String = ""
            If dtCust.Rows.Count > 0 Then
                For Each dr In dtCust.Rows

                    Invid = Invid & dr("ID").ToString & ","
                    lbl_Product.Text = lbl_Product.Text & dr("Desc").ToString & ","
                Next
                Invid = Invid.Substring(0, Invid.Length - 1)

                lbl_Product.Text = lbl_Product.Text.Substring(0, lbl_Product.Text.Length - 1)
            Else
                Invid = "0"
                lbl_Product.Text = "All"
            End If

            'If Not ddl_item.SelectedItem Is Nothing Then
            '    If Not ddl_item.SelectedItem.Value = "0" Then
            '        SearchQuery = " AND V.Item_code='" + ddl_item.SelectedItem.Value + "' " + SearchQuery
            '    End If
            'End If

            If Invid <> "0" Then
                SearchQuery = SearchQuery & " AND I.Inventory_item_ID in (Select item from dbo.SplitQuotedString ('" & Invid & "'))"
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
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        Try
            gvRep.Visible = False
            If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then

                LoadorgDetails()
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub

    Protected Sub ddlPaymentType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPaymentType.SelectedIndexChanged
        gvRep.Visible = False
        ddlCustomer.ClearSelection()

    End Sub


    Sub LoadorgDetails()
        Try
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
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        Try
            ddlOrganization.ClearSelection()
            If ddlOrganization.Items.Count = 2 Then
                ddlOrganization.SelectedIndex = 1
            End If
            ddlVan.ClearCheckedItems()
            ddlVan.Items.Clear()
            LoadorgDetails()
            ddlCustomer.ClearSelection()
            ddlCustomer.Text = ""
            ddlPaymentType.ClearSelection()
            txtRefNo.Text = ""
            txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
            txtToDate.SelectedDate = Now()
            ' ddlOrderType.ClearSelection()
            summary.InnerText = ""
            gvRep.Visible = False
            Args.Visible = False
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub BtnExportBiffExcel_Click(sender As Object, e As EventArgs) Handles BtnExportBiffExcel.Click
        Try
            Dim tblData As New DataTable

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
            lbl_OrderRefno.Text = txtRefNo.Text
            If Not String.IsNullOrEmpty(ddlCustomer.SelectedValue) Then
                Dim ids() As String
                ids = ddlCustomer.SelectedValue.Split("$")
                Dim custdt As New DataTable
                custdt = (New SalesWorx.BO.Common.Customer).GetCustomerDeatils(Err_No, Err_Desc, ids(0), ids(1))
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
            dt = ObjReport.GetSalesByProduct(Err_No, Err_Desc, SearchQuery, ddlOrganization.SelectedItem.Value, ddlPaymentType.SelectedItem.Value, van, CType(Session("User_Access"), UserAccess).UserID)
            Dim ClientCode As String
            ClientCode = (New SalesWorx.BO.Common.Common).GetAppConfig(Err_No, Err_Desc, "CLIENT_CODE")
            If ClientCode = "GYMA" Then
                tblData = dt.DefaultView.ToTable(False, "CH", "OutletCode", "Outletname", "Category", "CatDesc", "Item_Code", "Description", "Display_Order_Quantity", "Net_Total_Price", "tot", "Start_Time", "erprefno", "OrderNumber")

                For Each col In tblData.Columns
                    If col.ColumnName = "Start_Time" Then
                        col.ColumnName = "Invoice Date"
                    End If

                    If col.ColumnName = "OrderNumber" Then
                        col.ColumnName = "Cust.Ref.No."
                    End If
                    If col.ColumnName = "CH" Then
                        col.ColumnName = "Chain ID"
                    End If
                    If col.ColumnName = "Category" Then
                        col.ColumnName = "Prod.Cat."
                    End If
                    If col.ColumnName = "OutletCode" Then
                        col.ColumnName = "Customer No."

                    End If
                    If col.ColumnName = "Outletname" Then
                        col.ColumnName = "Customer Name"
                    End If
                    If col.ColumnName = "Display_Order_Quantity" Then
                        col.ColumnName = "Quantity"
                    End If
                    If col.ColumnName = "CatDesc" Then
                        col.ColumnName = "Category Description"
                    End If
                    If col.ColumnName = "tot" Then
                        col.ColumnName = "Sale Amount without Tax"
                    End If
                    If col.ColumnName = "Net_Total_Price" Then
                        col.ColumnName = "Sales with Tax"
                    End If
                    If col.ColumnName = "erprefno" Then
                        col.ColumnName = "Invoice ID"
                    End If

                Next

                If tblData.Rows.Count > 0 Then

                    Using package As New ExcelPackage()
                        ' add a new worksheet to the empty workbook
                        Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
                        Worksheet.Cells("A1").LoadFromDataTable(tblData, True)
                        Worksheet.Column(11).Style.Numberformat.Format = "dd-MMM-yyyy"
                        Worksheet.Column(9).Style.Numberformat.Format = "#,###.0000"
                        Worksheet.Column(10).Style.Numberformat.Format = "#,###.00"

                        Worksheet.Cells.AutoFitColumns()

                        Response.Clear()
                        Response.Buffer = True
                        Response.Charset = ""

                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                        Response.AddHeader("content-disposition", "attachment;filename= SalesbyProducts.xlsx")

                        Using MyMemoryStream As New MemoryStream()
                            package.SaveAs(MyMemoryStream)
                            MyMemoryStream.WriteTo(Response.OutputStream)
                            Response.AddHeader("Content-Length", MyMemoryStream.Length)
                            Response.Flush()
                            Response.Close()
                        End Using
                    End Using
                End If
            Else
                tblData = dt.DefaultView.ToTable(False, "Start_Time", "Route", "SalesRep", "OrderNumber", "TransactionType", "Customercode", "OutletCode", "Outletname", "Item_Code", "Description", "Display_Order_Quantity", "Display_UOM", "Unit_Selling_Price", "Net_Total_Price", "CustomerGroup")

                For Each col In tblData.Columns
                    If col.ColumnName = "Start_Time" Then
                        col.ColumnName = "Date"
                    End If

                    If col.ColumnName = "OrderNumber" Then
                        col.ColumnName = "Document  Number"
                    End If
                    If col.ColumnName = "TransactionType" Then
                        col.ColumnName = "Customer Type"
                    End If
                    If col.ColumnName = "Customercode" Then
                        col.ColumnName = "Customer Code"
                    End If
                    If col.ColumnName = "OutletCode" Then
                        col.ColumnName = "Outlet Code"
                    End If
                    If col.ColumnName = "Outletname" Then
                        col.ColumnName = "Outlet Name"
                    End If
                    If col.ColumnName = "Display_Order_Quantity" Then
                        col.ColumnName = "Quantity"
                    End If
                    If col.ColumnName = "Order_Quantity_UOM" Then
                        col.ColumnName = "UOM"
                    End If
                    If col.ColumnName = "Unit_Selling_Price" Then
                        col.ColumnName = "Item Price"
                    End If
                    If col.ColumnName = "Net_Total_Price" Then
                        col.ColumnName = "Line Total"
                    End If
                    If col.ColumnName = "CustomerGroup" Then
                        col.ColumnName = "Customer Group"
                    End If
                Next

                If tblData.Rows.Count > 0 Then

                    Using package As New ExcelPackage()
                        ' add a new worksheet to the empty workbook
                        Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
                        Worksheet.Cells("A1").LoadFromDataTable(tblData, True)
                        Worksheet.Column(1).Style.Numberformat.Format = "dd-MMM-yyyy"
                        Worksheet.Column(11).Style.Numberformat.Format = "#,###.0000"
                        Worksheet.Column(13).Style.Numberformat.Format = "#,###.00"
                        Worksheet.Column(14).Style.Numberformat.Format = "#,###.00"
                        Worksheet.Cells.AutoFitColumns()

                        Response.Clear()
                        Response.Buffer = True
                        Response.Charset = ""

                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                        Response.AddHeader("content-disposition", "attachment;filename= SalesbyProducts.xlsx")

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

        Catch ex As Exception
            If Err_Desc IsNot Nothing Then
                log.Error(Err_Desc)
            Else
                log.Error(GetExceptionInfo(ex))
            End If
        End Try
    End Sub
    'Private Sub ddl_item_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddl_item.ItemsRequested

    '    Dim strgency As String = ""
    '    If strgency = "" Then
    '        strgency = "0"
    '    End If
    '    Dim Objrep As New SalesWorx.BO.Common.Reports()
    '    Dim dt As New DataTable
    '    dt = Objrep.GetItemFromAgency(Err_No, Err_Desc, ddlOrganization.SelectedValue, strgency, e.Text)


    '    Dim ItemsPerRequest As Integer = 100 'Set the number of values to show
    '    Dim itemOffset As Integer = e.NumberOfItems
    '    Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
    '    e.EndOfItems = endOffset = dt.Rows.Count

    '    'Loop through the values to populate the combo box
    '    For i As Integer = itemOffset To endOffset - 1
    '        Dim item As New RadComboBoxItem()
    '        item.Text = dt.Rows(i).Item("Description").ToString
    '        item.Value = dt.Rows(i).Item("Inventory_Item_ID").ToString

    '        ddl_item.Items.Add(item)
    '        item.DataBind()
    '    Next

    'End Sub
    Sub LoadProdcut()
        Try


            ObjCommon = New SalesWorx.BO.Common.Common()
            Dim dt As New DataTable
            dt = ObjCommon.GetProductsByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue)
            ddl_Product.DataSource = dt
            ddl_Product.DataTextField = "Description"
            ddl_Product.DataValueField = "Inventory_Item_ID"
            ddl_Product.DataBind()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Protected Sub ddl_Product_EntryAdded(sender As Object, e As AutoCompleteEntryEventArgs) Handles ddl_Product.EntryAdded
        Try
            dtCust = CType(ViewState("dtCust"), DataTable)
            Dim seldr() As DataRow
            seldr = dtCust.Select("ID='" & e.Entry.Value & "'")
            If seldr.Length <= 0 Then
                Dim dr As DataRow
                dr = dtCust.NewRow()
                dr(0) = e.Entry.Value
                dr(1) = e.Entry.Text
                dtCust.Rows.Add(dr)
            End If
            ViewState("dtCust") = dtCust
            gvRep.Visible = False
            Args.Visible = False
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Protected Sub ddl_Product_EntryRemoved(sender As Object, e As AutoCompleteEntryEventArgs) Handles ddl_Product.EntryRemoved
        Try
            dtCust = CType(ViewState("dtCust"), DataTable)
            Dim dr() As DataRow
            dr = dtCust.Select("ID='" & e.Entry.Value & "'")
            If dr.Length > 0 Then
                dtCust.Rows.Remove(dr(0))
            End If
            ViewState("dtCust") = dtCust
            gvRep.Visible = False
            Args.Visible = False
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
End Class