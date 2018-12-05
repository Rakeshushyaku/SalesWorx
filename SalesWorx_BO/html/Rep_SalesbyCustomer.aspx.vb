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

Public Class Rep_SalesbyCustomer
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "SalesbyCustomer_V2"

    Private Const PageID As String = "P354"
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
            'Dim HasPermission As Boolean = False
            'ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            'If Not HasPermission Then
            '    Err_No = 500
            '    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            'End If
            'ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
            Try
                'dtItem.Columns.Add("ID")
                'dtItem.Columns.Add("Desc")
                'dtCust.Columns.Add("ID")
                'dtCust.Columns.Add("Desc")
                'ViewState("DtItem") = dtItem
                'ViewState("dtCust") = dtCust
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
        If ValidateInput() Then
            rpbFilter.Items(0).Expanded = False
            rptsect.Visible = True
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
            If Not String.IsNullOrEmpty(ddl_item.SelectedValue) Then
                HItem.Value = ddl_item.SelectedValue
                lbl_SkU.Text = (New SalesWorx.BO.Common.Product).GetItemName(Err_No, Err_Desc, ddl_item.SelectedValue, ddlOrganization.SelectedItem.Value)
            Else
                HItem.Value = "0"
                lbl_SkU.Text = "All"

            End If
            If Not String.IsNullOrEmpty(ddl_Customer.SelectedValue) Then
                Hcustomer.Value = ddl_Customer.SelectedValue
                Dim ids() As String
                ids = ddl_Customer.SelectedValue.Split("$")
                Dim custdt As New DataTable
                custdt = (New SalesWorx.BO.Common.Customer).GetCustomerDeatils(Err_No, Err_Desc, ids(0), ids(1))
                If custdt.Rows.Count > 0 Then
                    lbl_customer.Text = custdt.Rows(0)("Customer_no").ToString & " - " & custdt.Rows(0)("Customer_name").ToString
                End If
                HCount.Value = ""
            Else
                Hcustomer.Value = "0"
                lbl_customer.Text = "All"
                HCount.Value = " - Top 5 Customers"
            End If



            Dim collectionAgency As IList(Of RadComboBoxItem) = ddl_Agency.CheckedItems
            Dim Agencytxt As String = ""
            Dim Agency As String = ""
            For Each li As RadComboBoxItem In collectionAgency
                Agency = Agency & li.Value & ","
                Agencytxt = Agencytxt & li.Text & ","
            Next
            If Agencytxt <> "" Then
                Agencytxt = Agencytxt.Substring(0, Agencytxt.Length - 1)
            End If
            If Agency = "" Then
                Agency = "0"
            End If

            If Agency = "0" Then
                lbl_agency.Text = "All"
            Else
                lbl_agency.Text = Agencytxt
            End If
            HAgency.Value = Agency

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


            BindOverAllReport()
            BindOverAllChart()

        Else
            rptsect.Visible = False
            summary.InnerHtml = ""
            Args.Visible = False
        End If

    End Sub

    'Protected Sub ddlitem_EntryAdded(sender As Object, e As AutoCompleteEntryEventArgs) Handles ddlitem.EntryAdded
    '    dtItem = CType(ViewState("DtItem"), DataTable)
    '    Dim seldr() As DataRow
    '    seldr = dtItem.Select("ID=" & e.Entry.Value)
    '    If seldr.Length <= 0 Then
    '        Dim dr As DataRow
    '        dr = dtItem.NewRow()
    '        dr(0) = e.Entry.Value
    '        dr(1) = e.Entry.Text
    '        dtItem.Rows.Add(dr)
    '    End If
    '    ViewState("DtItem") = dtItem
    'End Sub
    'Protected Sub ddlitem_EntryRemoved(sender As Object, e As AutoCompleteEntryEventArgs) Handles ddlitem.EntryRemoved
    '    dtItem = CType(ViewState("DtItem"), DataTable)
    '    Dim dr() As DataRow
    '    dr = dtItem.Select("ID=" & e.Entry.Value)
    '    If dr.Length > 0 Then
    '        dtItem.Rows.Remove(dr(0))
    '    End If
    '    ViewState("DtItem") = dtItem
    'End Sub

    'Protected Sub ddlCust_EntryAdded(sender As Object, e As AutoCompleteEntryEventArgs) Handles ddlCustomer.EntryAdded
    '    dtCust = CType(ViewState("dtCust"), DataTable)
    '    Dim seldr() As DataRow
    '    seldr = dtCust.Select("ID=" & e.Entry.Value)
    '    If seldr.Length <= 0 Then
    '        Dim dr As DataRow
    '        dr = dtCust.NewRow()
    '        dr(0) = e.Entry.Value
    '        dr(1) = e.Entry.Text
    '        dtCust.Rows.Add(dr)
    '    End If
    '    ViewState("dtCust") = dtCust
    'End Sub
    'Protected Sub ddlCust_EntryRemoved(sender As Object, e As AutoCompleteEntryEventArgs) Handles ddlCustomer.EntryRemoved
    '    dtCust = CType(ViewState("dtCust"), DataTable)
    '    Dim dr() As DataRow
    '    dr = dtCust.Select("ID=" & e.Entry.Value)
    '    If dr.Length > 0 Then
    '        dtCust.Rows.Remove(dr(0))
    '    End If
    '    ViewState("dtCust") = dtCust
    'End Sub


    Function ValidateInput() As Boolean
        Dim bretval As Boolean = False
        If ddlOrganization.SelectedItem.Value = "0" Then
            MessageBoxValidation("Please Select the Organisation", "Validation")
            Return bretval
        End If
        bretval = True
        Return bretval
    End Function
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

            LoaAgency()



        Else

            ddl_Van.Items.Clear()
        End If

    End Sub
    Sub LoaAgency()
        ddl_Agency.DataTextField = "Agency"
        ddl_Agency.DataValueField = "Agency"
        Dim Objrep As New SalesWorx.BO.Common.Reports()
        ddl_Agency.DataSource = Objrep.GetAgency(Err_No, Err_Desc, ddlOrganization.SelectedValue)
        ddl_Agency.DataBind()
        Objrep = Nothing
    End Sub

    Sub LoadCustomer()


    End Sub


    Sub BindOverAllReport()
        Try

            Dim objRep As New SalesWorx.BO.Common.Reports

            Dim dt As New DataTable
            Dim Fromdate As Date
            Dim Todate As Date


            Fromdate = CDate(txtFromDate.SelectedDate)
            Todate = CDate(txtToDate.SelectedDate)


            dt = objRep.GetSalesReturnsbyCustomer(Err_No, Err_Desc, HorgID.Value, HVan.Value, HUID.Value, HType.Value, Fromdate.ToString("dd-MMM-yyyy"), Todate.ToString("dd-MMM-yyyy"), HAgency.Value, HItem.Value, Hcustomer.Value)

            gvRep.DataSource = dt
            gvRep.DataBind()
            lbl_Currency.Text = HCurrency.Value
            divCurrency.Visible = True

            Dim StrSummary As String = ""
            Dim SalesCash = dt.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("Sales")))

            StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>Total Sales " & HCurrency.Value & "<div class='text-primary'>" & Format(SalesCash, lblDecimal.Text) & "</div></div></div>"

            Dim Returns = dt.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("Returns")))
            StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>Total Returns " & HCurrency.Value & "<div class='text-primary'>" & Format(Returns, lblDecimal.Text) & "</div></div></div>"

            Dim Net = dt.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("Net")))
            StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>Net Sales " & HCurrency.Value & "<div class='text-primary'>" & Format(Net, lblDecimal.Text) & "</div></div></div>"

            summary.InnerHtml = StrSummary
            divSummaryNet.InnerHtml = StrSummary
            'If dt.Rows.Count > 8 And dt.Rows.Count < 14 Then
            '    Chartwrapper.Style.Add("width", (dt.Rows.Count * 60).ToString & "px")
            '    Chartwrapper1.Style.Add("width", (dt.Rows.Count * 60).ToString & "px")
            'ElseIf dt.Rows.Count > 14 Then
            '    Chartwrapper.Style.Add("width", (dt.Rows.Count * 40).ToString & "px")
            '    Chartwrapper1.Style.Add("width", (dt.Rows.Count * 40).ToString & "px")
            'Else
            If Hcustomer.Value <> "0" Then
                Chartwrapper.Style.Add("width", "1000px")
                Chartwrapper1.Style.Add("width", "1000px")
            Else
                Chartwrapper.Style.Add("width", "500px")
                Chartwrapper1.Style.Add("width", "500px")
            End If
            'End If

        Catch ex As Exception
            log.Error(ex.ToString())
        End Try
    End Sub
    Sub BindOverAllChart()
        ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshNetChart();", True)
        ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
    End Sub

    Private Sub ddl_item_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddl_item.ItemsRequested

        Dim strgency As String = ""
        For Each itm As RadComboBoxItem In ddl_Agency.Items
            If itm.Checked = True Then
                strgency = strgency & itm.Value & ","
            End If
        Next
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

    Private Sub ddl_Customer_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddl_Customer.ItemsRequested
        rptsect.Visible = False
        Args.Visible = False
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
            If column.UniqueName = "Sales" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                column.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
            ElseIf column.UniqueName = "Returns" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
                col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
            ElseIf column.UniqueName = "Net" Then
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
        BindOverAllReport()
    End Sub
    Private Sub dgv_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged

        BindOverAllReport()
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

    Private Sub ddl_Customer_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddl_Customer.SelectedIndexChanged
        rptsect.Visible = False
        Args.Visible = False
    End Sub
    Private Sub BtnExport_Click(sender As Object, e As EventArgs) Handles BtnExportExcel.Click
        Try
            If ValidateInput() Then
                Export("Excel")
            End If
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub
    Private Sub BtnPDF_Click(sender As Object, e As EventArgs) Handles BtnExportPDF.Click
        Try
            If ValidateInput() Then
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

        Dim OrgId As New ReportParameter
        OrgId = New ReportParameter("OID", CStr(ddlOrganization.SelectedItem.Value.ToString()))

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

        Dim FromDate As Date
        Dim ToDate As Date


        FromDate = CDate(txtFromDate.SelectedDate)
        ToDate = CDate(txtToDate.SelectedDate)


        Dim PFromDate As New ReportParameter
        PFromDate = New ReportParameter("Fromdate", FromDate.ToString("dd-MMM-yyyy"))


        Dim PToDate As New ReportParameter
        PToDate = New ReportParameter("ToDate", ToDate.ToString("dd-MMM-yyyy"))

        Dim UID As New ReportParameter
        UID = New ReportParameter("UID", objUserAccess.UserID)




        Dim collectionAgency As IList(Of RadComboBoxItem) = ddl_Agency.CheckedItems

        Dim Agency As String = ""
        For Each li As RadComboBoxItem In collectionAgency
            Agency = Agency & li.Value & ","

        Next

        If Agency = "" Then
            Agency = "0"
        End If

        Dim AgencyP As New ReportParameter
        AgencyP = New ReportParameter("Agency", Agency)

        Dim Item As New ReportParameter
        If Not String.IsNullOrEmpty(ddl_item.SelectedValue) Then
            Item = New ReportParameter("Item", ddl_item.SelectedValue)
        Else
            Item = New ReportParameter("Item", 0)
        End If


        Dim Cust As New ReportParameter
        Dim SiteID As New ReportParameter
        If Not String.IsNullOrEmpty(ddl_Customer.SelectedValue) Then
            Dim ids() As String
            ids = ddl_Customer.SelectedValue.Split("$")
            Cust = New ReportParameter("Customer", ids(0))
            SiteID = New ReportParameter("SiteID", ids(1))
        Else
            Cust = New ReportParameter("Customer", 0)
            SiteID = New ReportParameter("SiteID", 0)
        End If


        Dim Type As New ReportParameter
        Type = New ReportParameter("Type", 0)

        rview.ServerReport.SetParameters(New ReportParameter() {UID, OrgId, SID, PFromDate, PToDate, AgencyP, Item, Cust, SiteID, Type})

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
            Response.AddHeader("Content-disposition", "attachment;filename=SalesbyCustomer.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=SalesbyCustomer.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()

    End Sub

    Private Sub Salestab_TabClick(sender As Object, e As RadTabStripEventArgs) Handles Salestab.TabClick

        If Args.Visible = True Then
            If Salestab.Tabs(1).Selected = True Then
                ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
            End If
            If Salestab.Tabs(0).Selected = True Then
                ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshNetChart();", True)
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
        ddl_Agency.ClearCheckedItems()
        ddl_Agency.Items.Clear()

        rptsect.Visible = False

        Args.Visible = False
        ddl_Customer.ClearSelection()
        ddl_Customer.Text = ""

        ddl_item.ClearSelection()
        ddl_item.Text = ""

        LoadOrgDetails()

        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Now()

    End Sub
    Protected Sub gvRep_ItemCommand(sender As Object, e As GridCommandEventArgs)
        BindOverAllReport()
        If e.CommandName = RadGrid.ExportToExcelCommandName Then
            gvRep.ExportSettings.Excel.Format = GridExcelExportFormat.Biff
            gvRep.ExportSettings.IgnorePaging = True
            gvRep.ExportSettings.ExportOnlyData = True
            gvRep.ExportSettings.OpenInNewWindow = True
            gvRep.ExportSettings.FileName = "SalesbyCustomer"
        End If

    End Sub
    Private Sub BtnExportBiffExcel_Click(sender As Object, e As EventArgs) Handles BtnExportBiffExcel.Click
        Try

            Dim objRep As New SalesWorx.BO.Common.Reports

            Dim dt As New DataTable
            Dim Fromdate As Date
            Dim Todate As Date


            Fromdate = CDate(txtFromDate.SelectedDate)
            Todate = CDate(txtToDate.SelectedDate)


            dt = objRep.GetSalesReturnsbyCustomer(Err_No, Err_Desc, HorgID.Value, HVan.Value, HUID.Value, HType.Value, Fromdate.ToString("dd-MMM-yyyy"), Todate.ToString("dd-MMM-yyyy"), HAgency.Value, HItem.Value, Hcustomer.Value)

            Dim tblData As New DataTable
            tblData = dt.DefaultView.ToTable(False, "Customer", "Sales", "Returns", "Net")

            tblData.Columns("Sales").ColumnName = "Total Sales"
            tblData.Columns("Returns").ColumnName = "Total Returns"
            tblData.Columns("Net").ColumnName = "Net Sales"

            If tblData.Rows.Count > 0 Then


                Using package As New ExcelPackage()

                    Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
                    Worksheet.Cells("A1").LoadFromDataTable(tblData, True)
                    Worksheet.Column(2).Style.Numberformat.Format = "#,##0.00"
                    Worksheet.Column(3).Style.Numberformat.Format = "#,##0.00"
                    Worksheet.Column(4).Style.Numberformat.Format = "#,##0.00"
                    Worksheet.Cells.AutoFitColumns()
                    Response.Clear()
                    Response.Buffer = True
                    Response.Charset = ""

                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    Response.AddHeader("content-disposition", "attachment;filename= SalesbyCustomer.xlsx")

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
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
End Class