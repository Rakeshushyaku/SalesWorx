Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Public Class Rep_WeeklyReturnSummary
    Inherits System.Web.UI.Page

    'Dim dtItem As New DataTable
    Dim dtCust As New DataTable

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "WeeklyReturnHistory"

    Private Const PageID As String = "P359"
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
                dtCust.Columns.Add("ID")
                dtCust.Columns.Add("Desc")
                'ViewState("DtItem") = dtItem
                ViewState("dtCust") = dtCust
                ObjCommon = New SalesWorx.BO.Common.Common()

                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))

                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                End If


                txtFromMonth.SelectedDate = DateAdd(DateInterval.Month, -1, Now)
                txtToMonth.SelectedDate = Now()

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
        Else
            loadCustomer()
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
            Hcustomer.Value = ""
            HSiteID.Value = ""
            lbl_customer.Text = ""
            dtCust = CType(ViewState("dtCust"), DataTable)

            If dtCust.Rows.Count > 0 Then
                For Each dr In dtCust.Rows
                    Dim custids() As String
                    custids = dr("ID").ToString.Split("$")
                    Hcustomer.Value = Hcustomer.Value & custids(0) & ","
                    HSiteID.Value = HSiteID.Value & custids(1) & ","
                    lbl_customer.Text = lbl_customer.Text & dr("Desc").ToString & ","
                Next
                Hcustomer.Value = Hcustomer.Value.Substring(0, Hcustomer.Value.Length - 1)
                HSiteID.Value = HSiteID.Value.Substring(0, HSiteID.Value.Length - 1)
                lbl_customer.Text = lbl_customer.Text.Substring(0, lbl_customer.Text.Length - 1)
            Else
                Hcustomer.Value = "0"
                HSiteID.Value = "0"
                lbl_customer.Text = "All"
            End If

            Args.Visible = True

            lbl_Todt.Visible = True
            lbl_Totxt.Visible = True

            Dim fromdate As Date
            Dim Todate As Date
            fromdate = DateAdd(DateInterval.Day, -1 * (CDate(txtFromMonth.SelectedDate).Day - 1), CDate(txtFromMonth.SelectedDate))
            Todate = DateAdd(DateInterval.Day, -1 * (CDate(txtToMonth.SelectedDate).Day - 1), CDate(txtToMonth.SelectedDate))
            Todate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, Todate))





            HDate.Value = fromdate.ToString("dd-MMM-yyyy")
            lbl_Fromdt.Text = CDate(txtFromMonth.SelectedDate).ToString("MMM-yyyy")
            lbl_fromtxt.Text = "From Month:"

            HToDate.Value = Todate.ToString("dd-MMM-yyyy")
            lbl_Todt.Text = CDate(txtToMonth.SelectedDate).ToString("MMM-yyyy")
            lbl_Totxt.Text = "To Month:"


            BindOverAllReport()
            BindOverAllChart()

        Else
            rptsect.Visible = False
            'summary.InnerHtml = ""
            Args.Visible = False
        End If

    End Sub

    
    Protected Sub ddlCust_EntryAdded(sender As Object, e As AutoCompleteEntryEventArgs) Handles ddl_Customer.EntryAdded
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
        rptsect.Visible = False
        Args.Visible = False
    End Sub
    Protected Sub ddlCust_EntryRemoved(sender As Object, e As AutoCompleteEntryEventArgs) Handles ddl_Customer.EntryRemoved
        dtCust = CType(ViewState("dtCust"), DataTable)
        Dim dr() As DataRow
        dr = dtCust.Select("ID='" & e.Entry.Value & "'")
        If dr.Length > 0 Then
            dtCust.Rows.Remove(dr(0))
        End If
        ViewState("dtCust") = dtCust
        rptsect.Visible = False
        Args.Visible = False
    End Sub


    Function ValidateInput() As Boolean
        Dim bretval As Boolean = False
        If ddlOrganization.SelectedItem.Value = "0" Then
            MessageBoxValidation("Please Select the Organisation", "Validation")
            Return bretval
        End If
        'If Math.Abs(DateDiff(DateInterval.Month, CDate(txtFromMonth.SelectedDate), CDate(CDate(txtToMonth.SelectedDate)))) > 3 Then
        '    MessageBoxValidation("Please Select a date range of 3 months", "Validation")
        '    Return bretval
        'End If
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


            loadCustomer()


        Else

            ddl_Van.Items.Clear()
        End If

    End Sub

    Sub loadCustomer()
        ObjCommon = New SalesWorx.BO.Common.Common()
        Dim dt As New DataTable
        dt = ObjCommon.GetCustomerfromOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue)
        ddl_Customer.DataSource = dt
        ddl_Customer.DataTextField = "Customer"
        ddl_Customer.DataValueField = "CustomerID"
        ddl_Customer.DataBind()
    End Sub
   
    Sub BindOverAllReport()
        Try

            Dim objRep As New SalesWorx.BO.Common.Reports

            Dim dt As New DataTable
            Dim Fromdate As Date
            Dim Todate As Date

        
                Fromdate = DateAdd(DateInterval.Day, -1 * (CDate(txtFromMonth.SelectedDate).Day - 1), CDate(txtFromMonth.SelectedDate))
                Todate = DateAdd(DateInterval.Day, -1 * (CDate(txtToMonth.SelectedDate).Day - 1), CDate(txtToMonth.SelectedDate))
                Todate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, Todate))


            dt = objRep.GetWeeklyReturnSummary(Err_No, Err_Desc, HorgID.Value, HVan.Value, HUID.Value, Fromdate.ToString("dd-MMM-yyyy"), Todate.ToString("dd-MMM-yyyy"), Hcustomer.Value, HSiteID.Value)


            gvRep.DataSource = dt
            gvRep.DataBind()

             
            lbl_Currency.Text = HCurrency.Value
            divCurrency.Visible = True



            Dim StrSummary As String = ""
            Dim Returns = dt.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("RMA")))

            StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>Total Returns " & HCurrency.Value & "<div class='text-primary'>" & Format(Returns, lblDecimal.Text) & "</div></div></div>"



            summary.InnerHtml = StrSummary
            'divSummaryNet.InnerHtml = StrSummary
            'If dt.Rows.Count > 8 And dt.Rows.Count < 14 Then
            '    Chartwrapper.Style.Add("height", (dt.Rows.Count * 47).ToString & "px")
            'ElseIf dt.Rows.Count > 14 Then
            '    Chartwrapper.Style.Add("height", (dt.Rows.Count * 40).ToString & "px")
            'Else
            '    Chartwrapper.Style.Add("height", "400px")
            'End If
            If dt.Rows.Count > 8 And dt.Rows.Count < 14 Then
                Chartwrapper.Style.Add("width", (dt.Rows.Count * 45).ToString & "px")
            ElseIf dt.Rows.Count > 14 Then
                Chartwrapper.Style.Add("width", (dt.Rows.Count * 40).ToString & "px")
            End If

        Catch ex As Exception
            log.Error(ex.ToString())
        End Try
    End Sub
    Sub BindOverAllChart()
        ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
    End Sub
    
    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender
        For Each column As GridColumn In gvRep.MasterTableView.AutoGeneratedColumns
            If column.UniqueName <> "Description" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                column.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
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

        
            FromDate = DateAdd(DateInterval.Day, -1 * (CDate(txtFromMonth.SelectedDate).Day - 1), CDate(txtFromMonth.SelectedDate))
            ToDate = DateAdd(DateInterval.Day, -1 * (CDate(txtToMonth.SelectedDate).Day - 1), CDate(txtToMonth.SelectedDate))
            ToDate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, ToDate))
         

        Dim PFromDate As New ReportParameter
        PFromDate = New ReportParameter("Fromdate", FromDate.ToString("dd-MMM-yyyy"))


        Dim PToDate As New ReportParameter
        PToDate = New ReportParameter("ToDate", ToDate.ToString("dd-MMM-yyyy"))

        Dim UID As New ReportParameter
        UID = New ReportParameter("UID", objUserAccess.UserID)

       

        Dim Cust As New ReportParameter
        Dim SiteID As New ReportParameter
        dtCust = CType(ViewState("dtCust"), DataTable)
        Dim strCustID As String = ""
        Dim strSiteID As String = ""
        If dtCust.Rows.Count > 0 Then
            For Each dr In dtCust.Rows
                Dim custids() As String
                custids = dr("ID").ToString.Split("$")
                strCustID = strCustID & custids(0) & ","
                strSiteID = strSiteID & custids(1) & ","
            Next
            strCustID = strCustID.Substring(0, strCustID.Length - 1)
            strSiteID = strSiteID.Substring(0, strSiteID.Length - 1)
            Cust = New ReportParameter("Customer", strCustID)
            SiteID = New ReportParameter("SiteID", strSiteID)
        Else
            Cust = New ReportParameter("Customer", 0)
            SiteID = New ReportParameter("SiteID", 0)
        End If

        rview.ServerReport.SetParameters(New ReportParameter() {UID, OrgId, SID, PFromDate, PToDate, Cust, SiteID})

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
            Response.AddHeader("Content-disposition", "attachment;filename=WeeklyReturns.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=WeeklyReturns.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()

    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        Args.Visible = False
        ddl_Van.ClearCheckedItems()
        ddl_Van.Items.Clear()
        ddl_Customer.Entries.Clear()
        txtFromMonth.SelectedDate = DateAdd(DateInterval.Month, -1, Now)
        txtToMonth.SelectedDate = Now()

        LoadOrgDetails()
        rptsect.Visible = False

    End Sub
    Protected Sub gvRep_ItemCommand(sender As Object, e As GridCommandEventArgs)
        BindOverAllReport()
        If e.CommandName = RadGrid.ExportToExcelCommandName Then
            gvRep.ExportSettings.Excel.Format = GridExcelExportFormat.Biff
            gvRep.ExportSettings.IgnorePaging = True
            gvRep.ExportSettings.ExportOnlyData = True
            gvRep.ExportSettings.OpenInNewWindow = True
            gvRep.ExportSettings.FileName = "WeeklyReturns"
        End If

    End Sub
End Class