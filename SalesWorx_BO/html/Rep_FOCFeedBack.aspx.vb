Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Partial Public Class Rep_FOCFeedBack
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer
    Private ReportPath As String = "FOCFeedback"

    Private Const PageID As String = "P306"
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
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "-1"))


                Dim objUserAccess As UserAccess
                            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)



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


        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
        Dim ids() As String
        Dim CustomerID As New ReportParameter
        Dim SiteID As New ReportParameter
        If Not String.IsNullOrEmpty(ddl_Customer.SelectedValue) Then
            ids = ddl_Customer.SelectedValue.Split("$")
            CustomerID = New ReportParameter("CustID", ids(0))
            SiteID = New ReportParameter("SiteID", ids(1))
        Else
            CustomerID = New ReportParameter("CustID", "0")
            SiteID = New ReportParameter("SiteID", "0")
        End If

        Dim ORGID As New ReportParameter
        ORGID = New ReportParameter("OrgId", ddlOrganization.SelectedItem.Value)

        

        Dim FDate As New ReportParameter
        FDate = New ReportParameter("FromDate", CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"))

        Dim TDate As New ReportParameter
        TDate = New ReportParameter("Todate", CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))

        rview.ServerReport.SetParameters(New ReportParameter() {FDate, ORGID, TDate, CustomerID, SiteID})

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
            Response.AddHeader("Content-disposition", "attachment;filename=FOCFeedback.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=FOCFeedback.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()
    End Sub
    Private Sub BindData()
        Dim SearchQuery As String = ""
        Try


            rpbFilter.Items(0).Expanded = False
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable


            lbl_org.Text = ddlOrganization.SelectedItem.Text
            Dim custid As String = "0"
            Dim Siteid As String = "0"
            If Not String.IsNullOrEmpty(ddl_Customer.SelectedValue) Then
                Dim ids() As String
                ids = ddl_Customer.SelectedValue.Split("$")
                Dim custdt As New DataTable
                custdt = (New SalesWorx.BO.Common.Customer).GetCustomerDeatils(Err_No, Err_Desc, ids(0), ids(1))
                If custdt.Rows.Count > 0 Then
                    lbl_Customer.Text = custdt.Rows(0)("Customer_no").ToString & " - " & custdt.Rows(0)("Customer_name").ToString
                End If
                custid = ids(0)
                Siteid = ids(1)
            Else
                lbl_Customer.Text = "All"
            End If

            lbl_FromDate.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_ToDate.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")

            Args.Visible = True
             
            dt = ObjReport.GetFocFeedeback(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"), custid, Siteid)
            gvRep.DataSource = dt
            gvRep.DataBind()

        Catch ex As Exception
            If Err_Desc IsNot Nothing Then
                log.Error(Err_Desc)
            Else
                log.Error(GetExceptionInfo(ex))
            End If
            Err_No = "74067"
        Finally
            ObjCustomer = Nothing
        End Try
    End Sub
    Private Sub dgv_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvRep.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindData()
    End Sub
    Private Sub dgv_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged

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
    

    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            gvRep.Visible = True
            BindData()
        Else
            gvRep.Visible = False
            Args.Visible = False
        End If
    End Sub


    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        ddl_Customer.ClearSelection()
        ddl_Customer.Text = ""
        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Now()
        gvRep.Visible = False
        Args.Visible = False
    End Sub
End Class