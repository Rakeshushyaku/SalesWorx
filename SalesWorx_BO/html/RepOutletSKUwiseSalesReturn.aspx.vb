Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports System.Globalization
Imports Telerik.Web.UI

Partial Public Class RepOutletSKUwiseSalesReturn
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "OutletSKUWiseSalesReturn"

    Private Const PageID As String = "P223"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single

    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub RepOutletSKUwiseSalesReturn_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
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

                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1

                    LoadAgency()
                    LoadVan()
                    LoadCurrency()
                    LoadVan()
                    LoadCustomer()
                    LoadSKU()


                End If
                HUID.Value = CType(Session.Item("USER_ACCESS"), UserAccess).UserID

                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")

                Dim OrgID As New ReportParameter
                Dim OrgName As New ReportParameter
                OrgID = New ReportParameter("OrgID", ddlOrganization.SelectedItem.Value)
                OrgName = New ReportParameter("OrgName", ddlOrganization.SelectedItem.Text)

             

                ''  InitReportViewer()

                If Not Err_Desc Is Nothing Then
                    log.Error(Err_Desc)
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
                End If


            Catch ex As Exception
                Err_No = "74166"
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
    Sub LoadAgency()
        Try
            ObjCommon = New SalesWorx.BO.Common.Common()
            ddlAgency.DataSource = ObjCommon.GetAgencyList(Err_No, Err_Desc, ddlOrganization.SelectedValue)
            ddlAgency.DataBind()
            ddlAgency.Items.Insert(0, New RadComboBoxItem("Select Agency"))
        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            ObjCommon = Nothing
        End Try
    End Sub

    Sub LoadVan()
        Try
            ObjCommon = New SalesWorx.BO.Common.Common()
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
            ddlVan.DataTextField = "SalesRep_Name"
            ddlVan.DataValueField = "SalesRep_ID"
            ddlVan.DataBind()

            'For Each itm As RadComboBoxItem In ddlVan.Items
            '    itm.Checked = True
            'Next
        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            ObjCommon = Nothing
        End Try
    End Sub

    Sub LoadCurrency()
        Try
            Dim dtcurrency As DataTable
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

            Dim Currency As String = ""
            If dtcurrency.Rows.Count > 0 Then
                Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
                hfDigit.Value = "N" & dtcurrency.Rows(0)("Decimal_Digits")
                lbl_Currency.Text = Currency
            End If
            ObjReport = Nothing
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub InitReportViewer()
        Try


            Dim FDate As New ReportParameter

            ''If txtFromDate.Text.Trim() IsNot String.Empty Then
            ''    FDate = New ReportParameter("FromDate", txtFromDate.Text.Trim())
            ''Else
            ''    FDate = New ReportParameter("FromDate", Now.ToString("dd-MMM-yyyy"))
            ''End If


            Dim TDate As New ReportParameter
            ''If txtToDate.Text.Trim() IsNot String.Empty Then
            ''    TDate = New ReportParameter("ToDate", txtToDate.Text)
            ''Else
            ''    TDate = New ReportParameter("ToDate", Now.ToString("dd-MMM-yyyy"))
            ''End If


            Dim SiteID As New ReportParameter
            Dim Cid As String = Nothing
            Dim collectionCid As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddlCustomer.CheckedItems


            For Each li As Telerik.Web.UI.RadComboBoxItem In collectionCid
                Dim Arr As Array = li.Value.Split("$")
                If String.IsNullOrEmpty(Cid) Then
                    Cid = Arr(0) & "~" & Arr(1)
                Else
                    Cid = Cid & "|" & Arr(0) & "~" & Arr(1)
                End If
            Next

            If String.IsNullOrEmpty(Cid) Then
                SiteID = New ReportParameter("SID", "-1")
            Else
                SiteID = New ReportParameter("SID", Cid & "|")
            End If

            Dim InvID As New ReportParameter
            Dim Invids As String = Nothing
            Dim collection As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddSKU.CheckedItems


            For Each li As Telerik.Web.UI.RadComboBoxItem In collection
                If String.IsNullOrEmpty(Invids) Then
                    Invids = li.Value
                Else
                    Invids = Invids & "|" & li.Value
                End If
            Next
            If String.IsNullOrEmpty(Invids) Then
                InvID = New ReportParameter("InID", "-1")
            Else
                InvID = New ReportParameter("InID", Invids & "|")
            End If

            Dim Agency As New ReportParameter
            Agency = New ReportParameter("Agency", ddlAgency.SelectedItem.Value)

            Dim OrgID As New ReportParameter
            Dim OrgName As New ReportParameter
            OrgID = New ReportParameter("OrgID", ddlOrganization.SelectedItem.Value)
            OrgName = New ReportParameter("OrgName", ddlOrganization.SelectedItem.Text)
            Dim Fsrids As String = Nothing
            Dim collectionFsr As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddlVan.CheckedItems


            For Each li As Telerik.Web.UI.RadComboBoxItem In collectionFsr
                If String.IsNullOrEmpty(Fsrids) Then
                    Fsrids = li.Value
                Else
                    Fsrids = Fsrids & "|" & li.Value
                End If
            Next


            Dim RepID As New ReportParameter
            If String.IsNullOrEmpty(Fsrids) Then
                RepID = New ReportParameter("FSRID", "-1")
            Else
                RepID = New ReportParameter("FSRID", Fsrids & "|")
            End If

            ''With RVMain
            ''    .Reset()
            ''    .ShowParameterPrompts = False
            ''    .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            ''    .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            ''    .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
            ''    .ServerReport.SetParameters(New ReportParameter() {FDate, TDate, SiteID, InvID, OrgID, OrgName, Agency, RepID})
            ''    .ServerReport.Refresh()

            ''End With


        Catch Ex As Exception
            log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        Try
            summary.InnerHtml = ""
            Args.Visible = False
            gvRep.Visible = False
            divCurrency.Visible = False
            If ValidateInputs() Then
                BindData()
                Dim SHOW_UOM_MSG_BO_REPORTS As String = "N"
                Dim dt_app As New DataTable
                dt_app = (New SalesWorx.BO.Common.Common).GetAppControl(Err_No, Err_Desc, "SHOW_UOM_MSG_BO_REPORTS")
                If dt_app.Rows.Count > 0 Then
                    SHOW_UOM_MSG_BO_REPORTS = dt_app.Rows(0)("Control_Value").ToString().ToUpper()
                    If SHOW_UOM_MSG_BO_REPORTS = "Y" Then
                        lblmsgUOM.Text = "All the quantities displayed in this report are in Stock UOM"
                    Else
                        lblmsgUOM.Text = ""

                    End If
                End If
                Hfrom.Value = txtFromDate.SelectedDate.Value.ToString("dd-MMM-yyyy")
                HTo.Value = txtToDate.SelectedDate.Value.ToString("dd-MMM-yyyy")
                HorgID.Value = ddlOrganization.SelectedValue
            End If

        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
    Private Sub BindData()
        Dim SearchQuery As String = ""
        Dim objUserAccess As UserAccess
        Try



            Dim Cid As String = Nothing
            Dim collectionCid As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddlCustomer.CheckedItems
            Dim outlet As String = String.Empty

            For Each li As Telerik.Web.UI.RadComboBoxItem In collectionCid
                Dim Arr As Array = li.Value.Split("$")
                If String.IsNullOrEmpty(Cid) Then
                    Cid = Arr(0) & "~" & Arr(1)
                    outlet = li.Text
                Else
                    Cid = Cid & "|" & Arr(0) & "~" & Arr(1)
                    outlet = outlet & "," & li.Text
                End If
            Next

            If String.IsNullOrEmpty(Cid) Then
                Cid = "-1"
                lbl_Outlet.Text = "All"
            Else
                Cid = Cid & "|"
                lbl_Outlet.Text = outlet
            End If


            Dim Invids As String = Nothing
            Dim collection As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddSKU.CheckedItems
            Dim SKUtxt As String = String.Empty

            For Each li As Telerik.Web.UI.RadComboBoxItem In collection
                If String.IsNullOrEmpty(Invids) Then
                    Invids = li.Value
                    SKUtxt = li.Text
                Else
                    Invids = Invids & "|" & li.Value
                    SKUtxt = SKUtxt & "," & li.Text
                End If
            Next

            If String.IsNullOrEmpty(Invids) Then
                Invids = "-1"
                lbl_SKU.Text = "All"
            Else
                Invids = Invids & "|"
                lbl_SKU.Text = SKUtxt
            End If

            Dim Agency As String
            Agency = ddlAgency.SelectedValue

            If String.IsNullOrEmpty(Agency) Then
                Agency = "0"
                lbl_Agency.Text = "All"
            Else
                lbl_Agency.Text = ddlAgency.Text
            End If
          
            Dim Fsrids As String = Nothing
            Dim collectionFsr As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddlVan.CheckedItems

            Dim vantxt As String = String.Empty
            For Each li As Telerik.Web.UI.RadComboBoxItem In collectionFsr
                If String.IsNullOrEmpty(Fsrids) Then
                    Fsrids = li.Value
                    vantxt = li.Text
                Else
                    Fsrids = Fsrids & "|" & li.Value
                    vantxt = vantxt & "," & li.Text
                End If
            Next


            Dim RepID As New ReportParameter
            If String.IsNullOrEmpty(Fsrids) Then
                Fsrids = "-1"
                lbl_van.Text = "All"
            Else
                Fsrids = Fsrids & "|"
                lbl_van.Text = vantxt
            End If

            '' Binding Data

            rpbFilter.Items(0).Expanded = False
            lbl_org.Text = ddlOrganization.SelectedItem.Text
            lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")

            Dim dtcurrency As DataTable
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

            Dim Currency As String = ""
            If dtcurrency.Rows.Count > 0 Then
                lblDecimal.Text = "N" & dtcurrency.Rows(0)("Decimal_Digits")
                Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
            End If

            lbl_Currency.Text = Currency

            Args.Visible = True
            divCurrency.Visible = True


            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

            Dim dt As New DataTable
            dt = ObjReport.GetOutletwiseSKUwiseSaleReturn(Err_No, Err_Desc, ddlOrganization.SelectedValue, txtFromDate.SelectedDate, txtToDate.SelectedDate, Invids, Cid, Agency, Fsrids, HUID.Value)


            gvRep.DataSource = dt
            gvRep.DataBind()
            gvRep.Visible = True

            Dim StrSummary As String = ""
            Dim sum = dt.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("SaleValue")))

            StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>Total Sales " & Currency & "<div class='text-primary'>" & Format(sum, lblDecimal.Text) & "</div></div></div>"

            Dim sumR = dt.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("ReturnValue")))

            StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>Total Returns " & Currency & "<div class='text-primary'>" & Format(sumR, lblDecimal.Text) & "</div></div></div>"


            Dim sumNet = dt.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("NetSales")))

            StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>Net Sales " & Currency & "<div class='text-primary'>" & Format(sumNet, lblDecimal.Text) & "</div></div></div>"


            Dim sumRP = dt.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("ReturnValue")))

            StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>Total Returns% " & Currency & "<div class='text-primary'>" & Format((sumRP / 100), lblDecimal.Text) & "</div></div></div>"

            summary.InnerHtml = StrSummary


        Catch ex As Exception
            If Err_Desc IsNot Nothing Then
                log.Error(Err_Desc)
            Else
                log.Error(GetExceptionInfo(ex))
            End If
            Err_No = "7406712"
            '  Err_Desc = ex.Message
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
        Finally
            ObjCustomer = Nothing
            objUserAccess = Nothing
        End Try
    End Sub
    Private Sub gvRep_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        BindData()
    End Sub

    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender
        Try

       
        For Each column As GridColumn In gvRep.MasterTableView.Columns
            If column.UniqueName = "NetSales" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Decimal")
                col.DataFormatString = "{0:" & hfDigit.Value & "}"
            ElseIf column.UniqueName = "SaleValue" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Decimal")
                col.DataFormatString = "{0:" & hfDigit.Value & "}"
            ElseIf column.UniqueName = "ReturnValue" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Decimal")
                col.DataFormatString = "{0:" & hfDigit.Value & "}"
            End If
            Next
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Private Sub gvRep_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles gvRep.ItemDataBound
        Try
            If TypeOf e.Item Is GridDataItem Then
                Dim item As GridDataItem = TryCast(e.Item, GridDataItem)
                If item IsNot Nothing Then
                    item.Cells(6).Text = FormatNumber(CDbl(item.Cells(6).Text), hfDigit.Value.Replace("N", ""))
                    item.Cells(8).Text = FormatNumber(CDbl(item.Cells(8).Text), hfDigit.Value.Replace("N", ""))
                    item.Cells(10).Text = FormatNumber(CDbl(item.Cells(10).Text), hfDigit.Value.Replace("N", ""))
                End If
            End If


            '' For Footer
            If TypeOf e.Item Is GridFooterItem Then
                Dim item As GridFooterItem = TryCast(e.Item, GridFooterItem)

                If item IsNot Nothing Then
                    If Not String.IsNullOrEmpty(hfDigit.Value) Then
                        item("SALES").Text = FormatNumber(item("SALES").Text, hfDigit.Value.Replace("N", ""))
                        item("SaleValue").Text = FormatNumber(item("SaleValue").Text, hfDigit.Value.Replace("N", ""))

                        item("Returns").Text = FormatNumber(item("Returns").Text, hfDigit.Value.Replace("N", ""))
                        item("ReturnValue").Text = FormatNumber(item("ReturnValue").Text, hfDigit.Value.Replace("N", ""))

                        item("NetSalesQty").Text = FormatNumber(item("NetSalesQty").Text, hfDigit.Value.Replace("N", ""))
                        item("NetSales").Text = FormatNumber(item("NetSales").Text, hfDigit.Value.Replace("N", ""))

                        item("SaleValue").Attributes.Add("align", "right")
                        item("ReturnValue").Attributes.Add("align", "right")
                        item("NetSales").Attributes.Add("align", "right")
                    End If

                    If item("SKU") IsNot Nothing Then
                        item("SKU").Text = "Grand Total"
                    End If
                End If

            End If

            '' For Group Footer

            If TypeOf e.Item Is GridGroupFooterItem Then
                Dim item As GridGroupFooterItem = TryCast(e.Item, GridGroupFooterItem)
                If item IsNot Nothing Then
                    If Not String.IsNullOrEmpty(hfDigit.Value) Then
                        item("SaleValue").Text = FormatNumber(item("SaleValue").Text, hfDigit.Value.Replace("N", ""))
                        item("ReturnValue").Text = FormatNumber(item("ReturnValue").Text, hfDigit.Value.Replace("N", ""))
                        item("NetSales").Text = FormatNumber(item("NetSales").Text, hfDigit.Value.Replace("N", ""))
                    End If


                    If item("SKU") IsNot Nothing Then  '' Finding Group Footers
                        If IsNumeric(item.GroupIndex) Then
                            item("SKU").Text = "Outlet Total"
                            item.Attributes.Add("id", "clsOutlet")
                        Else
                            item("SKU").Text = "Total"
                            item.Attributes.Add("id", "clsAgency")
                        End If

                    End If
                End If
            End If

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
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If (ddlOrganization.SelectedValue = "") Then
            MessageBoxValidation("Please select the Organisation", "Validation")
            Return bretval
        ElseIf Not IsDate(txtFromDate.SelectedDate) Then
            MessageBoxValidation("Enter a valid from date.", "Validation")
            Return bretval
        ElseIf Not IsDate(txtToDate.SelectedDate) Then
            MessageBoxValidation("Enter a valid to date.", "Validation")
            Return bretval
        ElseIf CDate(txtFromDate.SelectedDate) > CDate(txtToDate.SelectedDate) Then
            MessageBoxValidation("Start Date should not be greater than End Date.", "Validation")
            Return bretval
        Else
            bretval = True
            Return bretval
        End If
    End Function
    Sub MessageBoxValidation(ByVal str As String, Title As String)
        ''lblMessage.ForeColor = Drawing.Color.Red
        ''lblinfo.Text = "Validation"
        ''lblMessage.Text = str
        ''MpInfoError.Show()
        ''MpInfoError.Show()
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Private Sub LoadCustomer()
        Try
            ObjCommon = New SalesWorx.BO.Common.Common()
            ddlCustomer.DataSource = ObjCommon.GetOutlet(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
            ddlCustomer.DataTextField = "Outlet"
            ddlCustomer.DataValueField = "CustomerID"
            ddlCustomer.DataBind()
        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            ObjCommon = Nothing
        End Try
    End Sub
    Private Sub LoadSKU()
        Try
            ObjCommon = New SalesWorx.BO.Common.Common()
            ddSKU.DataSource = ObjCommon.GetSKU(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
            ddSKU.DataTextField = "SKU"
            ddSKU.DataValueField = "Inventory_Item_ID"
            ddSKU.DataBind()
        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            ObjCommon = Nothing
        End Try
    End Sub

    Private Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrganization.SelectedIndexChanged
        Try
            ddlCustomer.DataSource = Nothing
            ddlCustomer.Items.Clear()
            ddSKU.DataSource = Nothing
            ddSKU.Items.Clear()
            ddlAgency.DataSource = Nothing
            ddlAgency.Items.Clear()
            ddlVan.DataSource = Nothing
            ddlVan.Items.Clear()
            summary.InnerHtml = ""
            Args.Visible = False
            gvRep.Visible = False
            divCurrency.Visible = False
            If ddlOrganization.SelectedIndex <> 0 Then
                'ObjCommon = New SalesWorx.BO.Common.Common()
                'ddlCustomer.DataSource = ObjCommon.GetOutlet(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
                'ddlCustomer.DataTextField = "Outlet"
                'ddlCustomer.DataValueField = "CustomerID"
                'ddlCustomer.DataBind()
                LoadCustomer()

                'ddlAgency.DataSource = ObjCommon.GetAgencyList(Err_No, Err_Desc, ddlOrganization.SelectedValue)
                'ddlAgency.DataBind()
                'ddlAgency.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))

                LoadAgency()

                ''ddSKU.DataSource = ObjCommon.GetSKU(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
                ''ddSKU.DataTextField = "SKU"
                ''ddSKU.DataValueField = "Inventory_Item_ID"
                ''ddSKU.DataBind()
                LoadSKU()

                'Dim objUserAccess As UserAccess
                'objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
                'ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
                'ddlVan.DataTextField = "SalesRep_Name"
                'ddlVan.DataValueField = "SalesRep_ID"
                'ddlVan.DataBind()

                LoadVan()

            Else
                'ddlCustomer.DataSource = Nothing
                'ddlCustomer.Items.Insert(0, New ListItem("-- All --"))
                'ddSKU.DataSource = Nothing
                'ddSKU.Items.Insert(0, New ListItem("-- All --"))
            End If
            ''RVMain.Reset()
        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            ObjCommon = Nothing
        End Try
    End Sub

    Protected Sub ddlAgency0_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAgency.SelectedIndexChanged
        Try
            ObjCommon = New SalesWorx.BO.Common.Common
            ddSKU.DataSource = ObjCommon.GetProductsByOrg_AgencyNew(Err_No, Err_Desc, ddlOrganization.SelectedValue, ddlAgency.SelectedItem.Value)
            ddSKU.DataTextField = "Description"
            ddSKU.DataValueField = "Inventory_Item_ID"
            ddSKU.DataBind()
        Catch ex As Exception
            log.Error(ex.Message)
        End Try



        ''RVMain.Reset()
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

        Try


            Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
            rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim Searchvalue As New ReportParameter


            Dim FDate As New ReportParameter
            FDate = New ReportParameter("FromDate", txtFromDate.SelectedDate.Value.ToString("dd-MMM-yyyy"))


            Dim TDate As New ReportParameter
            TDate = New ReportParameter("ToDate", txtToDate.SelectedDate.Value.ToString("dd-MMM-yyyy"))



            Dim SiteID As New ReportParameter
            Dim Cid As String = Nothing
            Dim collectionCid As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddlCustomer.CheckedItems


            For Each li As Telerik.Web.UI.RadComboBoxItem In collectionCid
                Dim Arr As Array = li.Value.Split("$")
                If String.IsNullOrEmpty(Cid) Then
                    Cid = Arr(0) & "~" & Arr(1)
                Else
                    Cid = Cid & "|" & Arr(0) & "~" & Arr(1)
                End If
            Next

            If String.IsNullOrEmpty(Cid) Then
                SiteID = New ReportParameter("SID", "-1")
            Else
                SiteID = New ReportParameter("SID", Cid & "|")
            End If

            Dim InvID As New ReportParameter
            Dim Invids As String = Nothing
            Dim collection As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddSKU.CheckedItems


            For Each li As Telerik.Web.UI.RadComboBoxItem In collection
                If String.IsNullOrEmpty(Invids) Then
                    Invids = li.Value
                Else
                    Invids = Invids & "|" & li.Value
                End If
            Next
            If String.IsNullOrEmpty(Invids) Then
                InvID = New ReportParameter("InID", "-1")
            Else
                InvID = New ReportParameter("InID", Invids & "|")
            End If

            Dim Agency As New ReportParameter
            Agency = New ReportParameter("Agency", CStr(IIf(ddlAgency.SelectedValue = "", "0", ddlAgency.SelectedValue)))

            Dim OrgID As New ReportParameter
            Dim OrgName As New ReportParameter
            OrgID = New ReportParameter("OrgID", ddlOrganization.SelectedItem.Value)
            OrgName = New ReportParameter("OrgName", ddlOrganization.SelectedItem.Text)
            Dim Fsrids As String = Nothing
            Dim collectionFsr As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddlVan.CheckedItems


            For Each li As Telerik.Web.UI.RadComboBoxItem In collectionFsr
                If String.IsNullOrEmpty(Fsrids) Then
                    Fsrids = li.Value
                Else
                    Fsrids = Fsrids & "|" & li.Value
                End If
            Next


            Dim RepID As New ReportParameter
            If String.IsNullOrEmpty(Fsrids) Then
                RepID = New ReportParameter("FSRID", "-1")
            Else
                RepID = New ReportParameter("FSRID", Fsrids & "|")
            End If


            Dim UID As New ReportParameter
            UID = New ReportParameter("UID", CStr(HUID.Value))


            rview.ServerReport.SetParameters(New ReportParameter() {FDate, TDate, OrgID, OrgName, SiteID, InvID, RepID, Agency, UID})

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
                Response.AddHeader("Content-disposition", "attachment;filename=OutletSKUWiseSalesReturn.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=OutletSKUWiseSalesReturn.xls")
                Response.AddHeader("Content-Length", bytes.Length)
            End If
            Response.OutputStream.Write(bytes, 0, bytes.Length)
            Response.OutputStream.Flush()
            Response.OutputStream.Close()
            Response.Flush()
            Response.Close()
        Catch ex As Exception
            log.Error(ex.Message)
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

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()

        ddlCustomer.ClearCheckedItems()
        ddlAgency.ClearSelection()
        ddlVan.ClearCheckedItems()
        ddSKU.ClearCheckedItems()


        ddlCustomer.Items.Clear()
        ddlAgency.Items.Clear()
        ddlVan.Items.Clear()
        ddSKU.Items.Clear()

        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
            LoadAgency()
            LoadVan()
            LoadCurrency()
            LoadVan()
            LoadCustomer()
            LoadSKU()
        End If

        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")


        summary.InnerHtml = ""
        gvRep.Visible = False
        Args.Visible = False
        divCurrency.Visible = False
        lblmsgUOM.Text = ""
    End Sub

    'Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender

    '    For Each column As GridColumn In gvRep.MasterTableView.Columns
    '        If column.UniqueName = "ReturnValue" Then
    '            Dim col As GridBoundColumn
    '            col = (CType(column, GridBoundColumn))
    '            col.DataType = System.Type.GetType("System.Double")
    '            col.DataFormatString = "{0:" & lblDecimal.Text & "}"
    '        End If
    '        If column.UniqueName = "SaleValue" Then
    '            Dim col As GridBoundColumn
    '            col = (CType(column, GridBoundColumn))
    '            col.DataType = System.Type.GetType("System.Double")
    '            col.DataFormatString = "{0:" & lblDecimal.Text & "}"
    '        End If
    '    Next
    'End Sub
End Class