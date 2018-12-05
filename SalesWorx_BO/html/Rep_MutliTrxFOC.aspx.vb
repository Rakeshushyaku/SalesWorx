Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Public Class Rep_MutliTrxFOC
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "MultiTrxFOC"

    Private Const PageID As String = "P372"
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
    Private Sub ddl_Customer_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddl_Customer.ItemsRequested
        rptsect.Visible = False
        Args.Visible = False
        Dim Objrep As New SalesWorx.BO.Common.Reports()
        Dim dt As New DataTable
        dt = Objrep.GetOutletsfromOrgtext(Err_No, Err_Desc, ddlOrganization.SelectedValue, e.Text)


        Dim ItemsPerRequest As Integer = 100 'Set the number of values to show
        Dim itemOffset As Integer = e.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
        e.EndOfItems = endOffset = dt.Rows.Count

        'Loop through the values to populate the combo box
        For i As Integer = itemOffset To endOffset - 1
            Dim item As New RadComboBoxItem()
            item.Text = dt.Rows(i).Item("Outlet").ToString
            item.Value = dt.Rows(i).Item("CustomerID").ToString

            ddl_Customer.Items.Add(item)
            item.DataBind()
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
    Sub BindReport()
        Try

            Dim objRep As New SalesWorx.BO.Common.Reports
            lbl_org.Text = ddlOrganization.SelectedItem.Text
            lbl_Fromdt.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_Todt.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")
            Args.Visible = True
            Dim dt As New DataTable
            Dim Fromdate As Date
            Dim Todate As Date
            Dim Hcustomer As String = "0"
            Dim Custid As String = "0"
            Dim SiteID As String = "0"
            If Not String.IsNullOrEmpty(ddl_Customer.SelectedValue) Then
                Hcustomer = ddl_Customer.SelectedValue
                Dim ids() As String
                ids = ddl_Customer.SelectedValue.Split("$")
                Custid = ids(0)
                SiteID = ids(1)
                Dim custdt As New DataTable
                custdt = (New SalesWorx.BO.Common.Customer).GetCustomerShipAddressDeatils(Err_No, Err_Desc, ids(0), ids(1))
                If custdt.Rows.Count > 0 Then
                    lbl_customer.Text = custdt.Rows(0)("Customer_no").ToString & " - " & custdt.Rows(0)("Customer_name").ToString
                End If

            Else
                Hcustomer = "0"
                lbl_customer.Text = "All"
            End If


            Fromdate = CDate(txtFromDate.SelectedDate)
            Todate = CDate(txtToDate.SelectedDate)


            dt = objRep.GetMultiTrxFOC(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, Fromdate.ToString("dd-MMM-yyyy"), Todate.ToString("dd-MMM-yyyy"), Custid, SiteID)

            gvRep.DataSource = dt
            gvRep.DataBind()
             
        Catch ex As Exception
            log.Error(ex.ToString())
        End Try
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
    Private Sub SearchBtn_Click(sender As Object, e As EventArgs) Handles SearchBtn.Click
        If ValidateInput() Then
            rpbFilter.Items(0).Expanded = False
            rptsect.Visible = True
            BindReport()
        Else
            rptsect.Visible = False
            Args.Visible = False
        End If
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
       

       

        Dim OrgID As New ReportParameter
        OrgID = New ReportParameter("OrgID", CStr(ddlOrganization.SelectedValue.ToString()))

        Dim FromDate As New ReportParameter
        FromDate = New ReportParameter("Fromdate", CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"))

        Dim CustomerID As New ReportParameter
        Dim SiteID As New ReportParameter

        Dim Custid As String = "0"
        Dim Site_ID As String = "0"

        If Not String.IsNullOrEmpty(ddl_Customer.SelectedValue) Then
            Dim ids() As String
            ids = ddl_Customer.SelectedValue.Split("$")
            Custid = ids(0)
            Site_ID = ids(1)
        End If

        CustomerID = New ReportParameter("CustID", Custid)
        SiteID = New ReportParameter("SiteID", Site_ID)

        Dim ToDate As New ReportParameter
        ToDate = New ReportParameter("Todate", CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))

         

        rview.ServerReport.SetParameters(New ReportParameter() {OrgID, FromDate, ToDate, CustomerID, SiteID})

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
            Response.AddHeader("Content-disposition", "attachment;filename=MUltiTrxFOC.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=MUltiTrxFOC.xls")
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
        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Now()
        ddl_Customer.ClearSelection()
        ddl_Customer.Text = ""
        Args.Visible = False
        rptsect.Visible = False
    End Sub
End Class