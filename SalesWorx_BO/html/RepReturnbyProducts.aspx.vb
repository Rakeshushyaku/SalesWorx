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

Partial Public Class RepReturnbyProducts
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer
    Private ReportPath As String = "ReturnbyProducts"
    Dim dv As New DataView
    Private Const PageID As String = "P392"
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

                    LoadCurrency()

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

            log.Error("1")
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


            log.Error("Searchvalue=" & SearchParams)
            log.Error("OrgID=" & CStr(ddlOrganization.SelectedValue.ToString()))
            log.Error("VisitID=")
            log.Error("SalesRep_ID=" & van)
            log.Error("FromDate=" & CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"))
            log.Error("ToDate=" & CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))
            log.Error("OrgName=" & CStr(IIf(ddlOrganization.SelectedIndex = 0, "0", ddlOrganization.SelectedItem.Text.Trim())))
            log.Error("RefNo=" & txtRefNo.Text)
            log.Error("CustType=" & ddlPaymentType.SelectedItem.Text)

            rview.ServerReport.SetParameters(New ReportParameter() {Searchvalue, OrgID, VisitID, SalesRep_ID, FromDate, ToDate, OrgName, Customer, RefNo, CustType})

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
                Response.AddHeader("Content-disposition", "attachment;filename=ReturnbyProducts.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=ReturnbyProducts.xls")
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
                LoadCurrency()

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
            dt = ObjReport.GetReturnByProduct(Err_No, Err_Desc, SearchQuery, ddlOrganization.SelectedItem.Value, ddlPaymentType.SelectedItem.Value)
            gvRep.DataSource = dt
            gvRep.DataBind()








            Dim dtcurrency As DataTable
            dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
            Dim Currency As String = ""
            If dtcurrency.Rows.Count > 0 Then
                Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
            End If

            Dim query = (From UserEntry In dt _
                        Group UserEntry By key = UserEntry.Field(Of String)("OrderType") Into Group _
                        Select PayMode = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("OrderAmount"))).ToList

           

            Dim StrSummary As String = ""
            Dim StrSummaryR As String = ""
            Dim i As Integer = 0
            For Each x In query
                StrSummary = StrSummary & "<div class='col-sm-3'><div class='widgetblk'>" & "Total " & x.PayMode & Currency & "<div class='text-primary'>" & Format(x.Total, lblDecimal.Text) & "</div></div></div>"
                i = i + 1
            Next

            Dim tot_vat As Decimal = Convert.ToDecimal(dt.Compute("SUM(VatAmount)", String.Empty))
            StrSummary = StrSummary & "<div class='col-sm-3'><div class='widgetblk'>" & "Total VAT Amount" & Currency & "<div class='text-primary'>" & Format(tot_vat, lblDecimal.Text) & "</div></div></div>"
            i = i + 1
            summary.InnerHtml = StrSummary

            Dim ShowDrillDownforDelNote As String = "N"
            ShowDrillDownforDelNote = (New SalesWorx.BO.Common.Common).GetAppConfig(Err_No, Err_Desc, "SHOW_DELV_NOTE_DRILLDOWN_BO")
            If ShowDrillDownforDelNote = "Y" Then
                gvRep.MasterTableView.Columns.FindByUniqueName("DeliveryNote").Visible = True
            Else
                gvRep.MasterTableView.Columns.FindByUniqueName("DeliveryNote").Visible = False
            End If



            Dim ClientCode As String
            ClientCode = (New SalesWorx.BO.Common.Common).GetAppConfig(Err_No, Err_Desc, "CLIENT_CODE")
            If ClientCode = "AST" Then
                gvRep.MasterTableView.Columns.FindByUniqueName("Amount").HeaderText = "Taxable Amount"
                gvRep.MasterTableView.Columns.FindByUniqueName("Gross_Inc_Amount").Visible = True
            Else
                gvRep.MasterTableView.Columns.FindByUniqueName("Gross_Inc_Amount").Visible = False
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
            'If ddlOrderType.SelectedValue = "Invoice" Then
            '    SearchQuery = SearchQuery & " AND OrderType='Invoice'"
            'End If
            'If ddlOrderType.SelectedValue = "Sales Order" Then
            '    SearchQuery = SearchQuery & " AND OrderType='Sales Order'"
            'End If
            'If ddlOrderType.SelectedValue = "Proforma Order" Then
            '    SearchQuery = SearchQuery & " AND OrderType='Proforma Order'"
            'End If

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
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
    Sub LoadCurrency()
        Try
            Dim dtcurrency As DataTable
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

            Dim Currency As String = ""
            If dtcurrency.Rows.Count > 0 Then
                lblDecimal.Text = "N" & dtcurrency.Rows(0)("Decimal_Digits")
                Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
            End If
            ' HCurrency.Value = Currency
        Catch ex As Exception

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
            dt = ObjReport.GetReturnByProduct(Err_No, Err_Desc, SearchQuery, ddlOrganization.SelectedItem.Value, ddlPaymentType.SelectedItem.Value)

            tblData = dt.DefaultView.ToTable(False, "Start_Time", "Route", "SalesRep", "OrderNumber", "TransactionType", "Customercode", "OutletCode", "Outletname", "Item_Code", "Description", "Display_Order_Quantity", "Display_UOM", "Unit_Selling_Price", "Net_Total_Price", "CustomerGroup")







            For Each col In tblData.Columns
                If col.ColumnName = "Start_Time" Then
                    col.ColumnName = "Date"
                End If

                If col.ColumnName = "OrderNumber" Then
                    col.ColumnName = "Document Number"
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
                    Response.AddHeader("content-disposition", "attachment;filename= ReturnbyProducts.xlsx")

                    Using MyMemoryStream As New MemoryStream()
                        package.SaveAs(MyMemoryStream)
                        MyMemoryStream.WriteTo(Response.OutputStream)
                        Response.AddHeader("Content-Length", MyMemoryStream.Length)
                        Response.Flush()
                        Response.Close()
                    End Using
                End Using
            End If
        Catch ex As Exception
            If Err_Desc IsNot Nothing Then
                log.Error(Err_Desc)
            Else
                log.Error(GetExceptionInfo(ex))
            End If
        End Try
    End Sub

   
End Class