

Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI

Partial Public Class RepInvoiceDiscount
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer
    Private ReportPath As String = "InvoiceDiscountDetails"
    Dim dv As New DataView
    Private Const PageID As String = "P212"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private total As Double = 0

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ''If Not IsNothing(Me.Master) Then

        ''    Dim masterScriptManager As ScriptManager
        ''    masterScriptManager = CType(Master.FindControl("ScriptManager1"), ScriptManager)

        ''    ' Make sure our master page has the script manager we're looking for
        ''    If Not IsNothing(masterScriptManager) Then

        ''        ' Turn off partial page postbacks for this page
        ''        masterScriptManager.EnablePartialRendering = False
        ''    End If

        ''End If

    End Sub

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
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization"))
                txtFromDate.SelectedDate = FirstDayOfMonth(Now().Date)
                txtToDate.SelectedDate = Now().Date

                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1

                    ''Filling Currency and decimal
                    Dim dtCur As New DataTable
                    dtCur = ObjCommon.GetCurrencyByOrg(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
                    If dtCur.Rows.Count > 0 Then
                        hfCurrency.Value = dtCur.Rows(0)(0).ToString()
                        hfDecimal.Value = dtCur.Rows(0)(1).ToString()
                        lbl_Currency.Text = dtCur.Rows(0)(0).ToString()
                    End If

                    LoadVan()
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
                ObjCommon = Nothing
                ErrorResource = Nothing
            End Try
        End If
    End Sub
    Public Function FirstDayOfMonth(ByVal sourceDate As DateTime) As DateTime
        Return New DateTime(sourceDate.Year, sourceDate.Month, 1)
    End Function
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Private Sub InitReportViewer(ByVal FilterValue As String)
        Try

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim Searchvalue As New ReportParameter
            Searchvalue = New ReportParameter("SearchParams", CStr(IIf(FilterValue Is Nothing, "", FilterValue)))



            Dim OrgID As New ReportParameter
            OrgID = New ReportParameter("OrgID", CStr(ddlOrganization.SelectedValue.ToString()))

            With RVMain
                .Reset()
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                .ServerReport.SetParameters(New ReportParameter() {Searchvalue, OrgID})
                '.ServerReport.Refresh()

            End With


        Catch Ex As Exception
            '  log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub
    Private Sub BindData()
        Dim SearchQuery As String = ""
        ObjCommon = New SalesWorx.BO.Common.Common()
        Dim ObjReport As New SalesWorx.BO.Common.Reports
        Try
            If ValidateInputs() Then
                rpbFilter.Items(0).Expanded = False

                lbl_org.Text = ddlOrganization.SelectedItem.Text

                If ddlVan.SelectedValue <> "" Then
                    SearchQuery = " And A.Created_By=" & ddlVan.SelectedValue
                    lbl_van.Text = ddlVan.SelectedItem.Text
                Else
                    SearchQuery = " And A.Created_By in (" & ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID) & ")"
                    lbl_van.Text = "All"
                End If

                If ddlCustomer.SelectedValue <> "" Then
                    SearchQuery = SearchQuery & " AND (LTRIM(STR(Y.Customer_ID)) + '$' + LTRIM(STR(Y.Site_Use_ID)))='" & ddlCustomer.SelectedValue & "' "
                    lbl_Customer.Text = ddlCustomer.Text
                Else
                    lbl_Customer.Text = "All"
                End If

                If Not txtFromDate.SelectedDate Is Nothing Then
                    SearchQuery = SearchQuery & " And A.System_Order_Date >=convert(datetime,'" & CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy") & "',103)"
                End If
                If Not txtToDate.SelectedDate Is Nothing Then
                    SearchQuery = SearchQuery & " And A.System_Order_Date <=convert(datetime,'" & CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy") & " 23:59:59',103)"
                End If

                lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
                lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")


                Dim dt As New DataTable
                dt = ObjReport.GetInvDiscountDetails(Err_No, Err_Desc, ddlOrganization.SelectedValue, SearchQuery)

                If dt.Rows.Count > 0 Then
                    total = dt.Compute("Sum(Order_Amt)", "")
                End If


                gvRep.DataSource = dt
                gvRep.DataBind()

            End If
        Catch ex As Exception
            If Err_Desc IsNot Nothing Then
                log.Error(Err_Desc)
            Else
                log.Error(GetExceptionInfo(ex))
            End If
            Err_No = "74089"
            '  Err_Desc = ex.Message
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
        Finally
            ObjCommon = Nothing
            ObjReport = Nothing
        End Try
    End Sub

    Private Sub BtnExportExcel_Click(sender As Object, e As EventArgs) Handles BtnExportExcel.Click
        Try
            If ValidateInputs() Then
                Export("Excel")
            End If
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub
    Sub Export(format As String)
        Dim ObjCommon As SalesWorx.BO.Common.Common
        Try

            ObjCommon = New SalesWorx.BO.Common.Common()
            Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
            rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)


            Dim SearchQuery As String = ""


            If ddlVan.SelectedValue <> "" Then
                SearchQuery = " And A.Created_By=" & ddlVan.SelectedValue
                lbl_van.Text = ddlVan.SelectedItem.Text
            Else
                SearchQuery = " And A.Created_By in (" & ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID) & ")"
                lbl_van.Text = "All"
            End If

            If ddlCustomer.SelectedValue <> "" Then
                SearchQuery = SearchQuery & " AND (LTRIM(STR(Y.Customer_ID)) + '$' + LTRIM(STR(Y.Site_Use_ID)))='" & ddlCustomer.SelectedValue & "' "
                lbl_Customer.Text = ddlCustomer.Text
            Else
                lbl_Customer.Text = "All"
            End If

            If Not txtFromDate.SelectedDate Is Nothing Then
                SearchQuery = SearchQuery & " And A.System_Order_Date >=convert(datetime,'" & CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy") & "',103)"
            End If
            If Not txtToDate.SelectedDate Is Nothing Then
                SearchQuery = SearchQuery & " And A.System_Order_Date <=convert(datetime,'" & CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy") & " 23:59:59',103)"
            End If



            Dim OrgID As New ReportParameter
            OrgID = New ReportParameter("OrgID", CStr(ddlOrganization.SelectedValue.ToString()))



            Dim Searchvalue As New ReportParameter
            Searchvalue = New ReportParameter("SearchParams", SearchQuery)


            
            Dim SID As New ReportParameter
            If ddlVan.SelectedValue <> "" Then
                SID = New ReportParameter("SID", ddlVan.SelectedValue)
            Else
                SID = New ReportParameter("SID", "0")
            End If
            Dim Customer As New ReportParameter
            Dim Site As New ReportParameter
            If Not String.IsNullOrEmpty(ddlCustomer.SelectedValue) Then
                Dim ids() As String
                ids = ddlCustomer.SelectedValue.Split("$")
                Dim custdt As New DataTable
                custdt = (New SalesWorx.BO.Common.Customer).GetCustomerDeatils(Err_No, Err_Desc, ids(0), ids(1))
                If custdt.Rows.Count > 0 Then
                    Dim strcust As String
                    strcust = custdt.Rows(0)("Customer_no").ToString & " - " & custdt.Rows(0)("Customer_name").ToString
                    Customer = New ReportParameter("CustID", ids(0))
                    Site = New ReportParameter("SiteID", ids(1))
                End If
            Else
                Customer = New ReportParameter("CustID", "0")
                Site = New ReportParameter("SiteID", "0")
            End If




            Dim FromDate As New ReportParameter
            FromDate = New ReportParameter("FromDate", CStr(CDate(txtFromDate.SelectedDate)))

            Dim ToDate As New ReportParameter
            ToDate = New ReportParameter("ToDate", CStr(CDate(txtToDate.SelectedDate)))



            rview.ServerReport.SetParameters(New ReportParameter() {Searchvalue, OrgID, SID, Customer, Site, FromDate, ToDate})

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
                Response.AddHeader("Content-disposition", "attachment;filename=InvoiceDiscount.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=InvoiceDiscount.xls")
                Response.AddHeader("Content-Length", bytes.Length)
            End If
            Response.OutputStream.Write(bytes, 0, bytes.Length)
            Response.OutputStream.Flush()
            Response.OutputStream.Close()
            Response.Flush()
            Response.Close()
        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            ObjCommon = Nothing
        End Try
    End Sub

    Private Sub BtnExportPDF_Click(sender As Object, e As EventArgs) Handles BtnExportPDF.Click
        Try
            If ValidateInputs() Then
                Export("PDF")
            End If
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub

    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If ddlOrganization.SelectedValue = "" Then
            MessageBoxValidation("Select an Organization.", "Validation")
            Return bretval
        End If
        If txtFromDate.SelectedDate Is Nothing Then
            MessageBoxValidation("Enter a valid From Date", "Validation")
            Return bretval
        End If
        If txtToDate.SelectedDate Is Nothing Then
            MessageBoxValidation("Enter a valid To Date", "Validation")
            Return bretval
        End If
        If CDate(txtFromDate.SelectedDate) > CDate(txtToDate.SelectedDate) Then
            MessageBoxValidation("Start Date should not be greater than End Date.", "Validation")
            Exit Function
        Else
            Return True
        End If
    End Function
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        divCurrency.Visible = False
        gvRep.Visible = False
        Args.Visible = False
        If ValidateInputs() Then
            divCurrency.Visible = True
            gvRep.Visible = True
            Args.Visible = True
            BindData()
        End If
    End Sub

    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        ObjCommon = New SalesWorx.BO.Common.Common()
        Try
            If Not (ddlOrganization.SelectedItem.Value = "") Then
                'Dim objUserAccess As UserAccess
                'objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
                'ObjCommon = New SalesWorx.BO.Common.Common()
                'ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())
                'ddlVan.DataBind()
                'ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --"))

                'ddlCustomer.DataSource = ObjCommon.GetCustomerByCriteria(Err_No, Err_Desc, ddlOrganization.SelectedValue)
                'ddlCustomer.DataBind()
                'ddlCustomer.Items.Insert(0, New RadComboBoxItem("-- Select a value --"))

                ''Filling Currency and decimal
                Dim dtCur As New DataTable
                dtCur = ObjCommon.GetCurrencyByOrg(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
                If dtCur.Rows.Count > 0 Then
                    hfCurrency.Value = dtCur.Rows(0)(0).ToString()
                    hfDecimal.Value = dtCur.Rows(0)(1).ToString()
                    lbl_Currency.Text = dtCur.Rows(0)(0).ToString()
                End If

                LoadVan()
            Else
                ddlVan.ClearSelection()
                ddlVan.Items.Clear()
                ddlVan.Items.Insert(0, New RadComboBoxItem("Select a value", "0"))

                ddlCustomer.ClearSelection()
                ddlCustomer.Items.Clear()
                ddlCustomer.Text = ""
            End If
        Catch ex As Exception
            ObjCommon = Nothing
        End Try

    End Sub

    Private Sub ddlCustomer_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddlCustomer.ItemsRequested

        Dim ObjCommon As New SalesWorx.BO.Common.Common()
        Try
            Dim dt As New DataTable


            ddlCustomer.Items.Clear()

            dt = ObjCommon.GetCustomerByCriteriaandText(Err_No, Err_Desc, ddlOrganization.SelectedValue, "", e.Text)

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
            log.Error(ex.Message)
        Finally
            ObjCommon = Nothing
        End Try
    End Sub

    Sub LoadVan()
        Try
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New SalesWorx.BO.Common.Common()
            ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
            ddlVan.DataValueField = "SalesRep_ID"
            ddlVan.DataTextField = "SalesRep_Name"
            ddlVan.DataBind()

            ddlVan.Items.Insert(0, New RadComboBoxItem("Select Van"))

        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub gvRep_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        BindData()
    End Sub

    Private Sub gvRep_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles gvRep.ItemDataBound
        Try
            If TypeOf e.Item Is GridDataItem Then
                Dim item As GridDataItem = TryCast(e.Item, GridDataItem)
                If item IsNot Nothing Then
                    '' item.Cells(5).Text = FormatNumber(CDbl(item.Cells(5).Text), hfDecimal.Value)
                    Dim type As HiddenField = CType(item.Cells(6).FindControl("HType"), HiddenField)
                    Dim val As HiddenField = CType(item.Cells(6).FindControl("HDisVal"), HiddenField)
                    Dim lblDis As Label = CType(item.Cells(6).FindControl("lbl_DisCount"), Label)
                    Dim deci As HiddenField = CType(item.Cells(6).FindControl("hDC"), HiddenField)

                    If type IsNot Nothing AndAlso val IsNot Nothing AndAlso lblDis IsNot Nothing Then
                        If type.Value = "P" And CDec(val.Value) > 0 Then
                            lblDis.Text = val.Value & " %"
                        ElseIf type.Value = "V" And CDec(val.Value) > 0 Then
                            lblDis.Text = FormatNumber(CDbl(val.Value), hfDecimal.Value)
                        End If
                    End If

                    item.Cells(7).Text = FormatNumber(CDbl(item.Cells(7).Text), hfDecimal.Value)
                    item.Cells(9).Text = FormatNumber(CDbl(item.Cells(9).Text), hfDecimal.Value)
                    item.Cells(10).Text = FormatNumber(CDbl(item.Cells(10).Text), hfDecimal.Value)
                End If
            End If

            '' For Footer
            If TypeOf e.Item Is GridFooterItem Then
                Dim item As GridFooterItem = TryCast(e.Item, GridFooterItem)

                If item IsNot Nothing Then
                    Dim strtxt As String = item("ItemPrice").Text
                    ''Net Amount : 159321.860000
                    strtxt = strtxt.Replace("Net Amount :", "")

                    If Not String.IsNullOrEmpty(strtxt) AndAlso Not String.IsNullOrEmpty(hfDecimal.Value) Then
                        ''strtxt = FormatNumber(CDbl(strtxt), hfDecimal.Value)
                        ''item("ItemPrice").Text = "Grand Total : " & strtxt

                        item("ItemPrice").Text = "Grand Total : " & FormatNumber(strtxt, hfDecimal.Value)

                    End If
                End If

            End If


            '' For Group Footer
            If TypeOf e.Item Is GridGroupFooterItem Then
                Dim item As GridGroupFooterItem = TryCast(e.Item, GridGroupFooterItem)
                If item IsNot Nothing Then

                    '' Getting Order Amount 

                    ' ''Dim hfOrderAmt As HiddenField = CType(item.Cells(1).FindControl("hfOrderAmount"), HiddenField)
                    ' ''If hfOrderAmt IsNot Nothing Then
                    ' ''    total = total + CDbl(hfOrderAmt.Value)
                    ' ''End If


                    '' For Net Amount 
                    Dim lblNet As Label = CType(item.Cells(1).FindControl("lbl_ItemPrice"), Label)
                    Dim hfDisType As HiddenField = CType(item.Cells(1).FindControl("hfDisType"), HiddenField)
                    Dim hfDC As HiddenField = CType(item.Cells(1).FindControl("hfDC"), HiddenField)

                    If lblNet IsNot Nothing AndAlso hfDC IsNot Nothing Then
                        lblNet.Text = FormatNumber(CDbl(lblNet.Text), hfDecimal.Value)
                        hfDecimal.Value = hfDC.Value
                    End If

                    '' Setting Discount value

                    Dim lblDis As Label = CType(item.Cells(1).FindControl("lbl_Discount"), Label)
                    Dim lblDisVal As HiddenField = CType(item.Cells(1).FindControl("hfDisValue"), HiddenField)

                    If lblDis IsNot Nothing AndAlso lblDisVal IsNot Nothing AndAlso hfDisType IsNot Nothing Then
                        If hfDisType.Value = "V" Then
                            lblDis.Text = lblDisVal.Value
                        ElseIf hfDisType.Value = "P" Then
                            lblDis.Text = (CDbl(lblDis.Text) * lblDisVal.Value / 100)
                        Else
                            lblDis.Text = 0
                        End If
                    End If

                    '' Formating Discount
                    If lblDis IsNot Nothing AndAlso hfDC IsNot Nothing Then
                        lblDis.Text = FormatNumber(CDbl(lblDis.Text), hfDecimal.Value)
                    End If


                    ''Setting Total 
                    Dim lblTotal As Label = CType(item.Cells(1).FindControl("lbl_Total"), Label)

                    If lblTotal IsNot Nothing AndAlso lblDis IsNot Nothing Then
                        lblTotal.Text = FormatNumber(CDbl(lblNet.Text) - CDbl(lblDis.Text), hfDecimal.Value)
                    End If

                End If
            End If

            '' Updating Group Header Text

            If TypeOf e.Item Is GridGroupHeaderItem Then
                Dim item As GridGroupHeaderItem = CType(e.Item, GridGroupHeaderItem)
                Dim groupDataRow As DataRowView = CType(e.Item.DataItem, DataRowView)
                item.DataCell.Text = String.Format("{0} - {1}&nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; {2} &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  Customer : {3} - {4}", item.DataCell.Text, groupDataRow("SalesRep_Name").ToString(),
                                                    CDate(groupDataRow("Creation_Date")).ToString("dd-MMM-yyyy"), groupDataRow("Customer_No").ToString(), groupDataRow("Customer_Name").ToString())
            End If
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender
        Try
            For Each item As GridGroupFooterItem In gvRep.MasterTableView.GetItems(GridItemType.GroupFooter)
                'set the colspan, so that the template cells are aligned with the grid columns
                For i As Integer = 0 To item.Cells.Count - 1
                    TryCast(item.Cells(i), TableCell).ColumnSpan = 1
                Next
            Next

            ''IIf(Fields!DisType.Value = "P" And CDec(Fields!Discount.Value) > 0, Fields!Discount.Value & " % ", IIf(Fields!DisType.Value = "V" And CDec(Fields!Discount.Value) > 0, Fields!Discount.Value, ""))
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
    Private Sub gvRep_SortCommand(sender As Object, e As GridSortCommandEventArgs) Handles gvRep.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindData()
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

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        ddlVan.ClearSelection()
        ddlVan.Items.Clear()
        ObjCommon = New SalesWorx.BO.Common.Common()
        Try
            If Not (ddlOrganization.SelectedItem.Value = "") Then
                'Dim objUserAccess As UserAccess
                'objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
                'ObjCommon = New SalesWorx.BO.Common.Common()
                'ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())
                'ddlVan.DataBind()
                'ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --"))

                'ddlCustomer.DataSource = ObjCommon.GetCustomerByCriteria(Err_No, Err_Desc, ddlOrganization.SelectedValue)
                'ddlCustomer.DataBind()
                'ddlCustomer.Items.Insert(0, New RadComboBoxItem("-- Select a value --"))

                ''Filling Currency and decimal
                Dim dtCur As New DataTable
                dtCur = ObjCommon.GetCurrencyByOrg(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
                If dtCur.Rows.Count > 0 Then
                    hfCurrency.Value = dtCur.Rows(0)(0).ToString()
                    hfDecimal.Value = dtCur.Rows(0)(1).ToString()
                    lbl_Currency.Text = dtCur.Rows(0)(0).ToString()
                End If

                LoadVan()
            Else
                ddlVan.ClearSelection()
                ddlVan.Items.Clear()
                ddlVan.Items.Insert(0, New RadComboBoxItem("Select a value", "0"))

                ddlCustomer.ClearSelection()
                ddlCustomer.Items.Clear()
                ddlCustomer.Text = ""
            End If
        Catch ex As Exception
            ObjCommon = Nothing
        End Try

        ddlCustomer.ClearSelection()
        ddlCustomer.Text = ""

        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Now()

        gvRep.Visible = False
        Args.Visible = False
    End Sub
End Class