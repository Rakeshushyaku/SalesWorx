Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports System.Collections.Generic
Imports Telerik.Web.UI

Public Class REp_CreditDetHistory
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "HistoryofCreditDet"

    Private Const PageID As String = "P391"
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
                Err_No = "774066"
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
    Private Sub BindData()

        Try

            ObjCommon = New SalesWorx.BO.Common.Common()

            Args.Visible = True



            Dim fromdate As Date
            Dim Todate As Date

            fromdate = CDate(txtFromDate.SelectedDate)
            Todate = CDate(txtToDate.SelectedDate)

            lbl_Fromdt.Text = fromdate.ToString("dd-MMM-yyyy")
            lbl_Todt.Text = Todate.ToString("dd-MMM-yyyy")
            Dim User As String = "ALL"

            If ddl_User.SelectedIndex > 0 Then
                User = ddl_User.SelectedItem.Value
                lbl_User.Text = ddl_User.SelectedItem.Text
            Else
                lbl_User.Text = User
            End If


            Dim Customer As String = "ALL"
            Dim Site As String = ""
            If Not String.IsNullOrEmpty(ddlCustomer.SelectedValue) Then
                Dim ids() As String
                ids = ddlCustomer.SelectedValue.Split("$")
                Customer = ids(0)
                Site = ids(1)
                lbl_Customer.Text = ddlCustomer.Text
            Else
                lbl_Customer.Text = Customer
            End If

            lbl_Org.Text = ddlOrganization.SelectedItem.Text


            Dim objRep As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = objRep.GetCreditHistoryDetails(Err_No, Err_Desc, CDate(txtFromDate.SelectedDate).ToString("MM-dd-yyyy"), CDate(txtToDate.SelectedDate).ToString("MM-dd-yyyy"), Customer, User, ddlOrganization.SelectedItem.Value, Site)

            gvRep.DataSource = dt
            gvRep.DataBind()

            Args.Visible = True
            gvRep.Visible = True


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

        End Try
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


        Dim fromdate As Date
        Dim Todate As Date

        fromdate = CDate(txtFromDate.SelectedDate)
        Todate = CDate(txtToDate.SelectedDate)

        lbl_Fromdt.Text = fromdate.ToString("dd-MMM-yyyy")
        lbl_Todt.Text = Todate.ToString("dd-MMM-yyyy")
        Dim User As String = "ALL"

        If ddl_User.SelectedIndex > 0 Then
            User = ddl_User.SelectedItem.Value
            lbl_User.Text = ddl_User.SelectedItem.Text
        Else
            lbl_User.Text = User
        End If


        Dim Customer As String = "ALL"
        Dim Site As String = ""
        If Not String.IsNullOrEmpty(ddlCustomer.SelectedValue) Then
            Dim ids() As String
            ids = ddlCustomer.SelectedValue.Split("$")
            Customer = ids(0)
            Site = ids(1)
            lbl_Customer.Text = ddlCustomer.Text
        Else
            lbl_Customer.Text = Customer
        End If

        lbl_Org.Text = ddlOrganization.SelectedItem.Text

        Dim FDate As New ReportParameter
        FDate = New ReportParameter("FromDate", fromdate.ToString("MM-dd-yyyy"))

        Dim TDate As New ReportParameter
        TDate = New ReportParameter("ToDate", Todate.ToString("MM-dd-yyyy"))


        Dim CustomerParam As New ReportParameter
        CustomerParam = New ReportParameter("Customer", Customer)

        Dim SiteParam As New ReportParameter
        SiteParam = New ReportParameter("SiteID", Site)

        Dim UserParam As New ReportParameter
        UserParam = New ReportParameter("User", User)

        Dim OrgParam As New ReportParameter
        OrgParam = New ReportParameter("Org", ddlOrganization.SelectedItem.Value)

        Dim OrgNameParam As New ReportParameter
        OrgNameParam = New ReportParameter("OrgName", ddlOrganization.Text)

        Dim Username As New ReportParameter
        Username = New ReportParameter("Username", lbl_User.Text)

        Dim CustName As New ReportParameter
        CustName = New ReportParameter("CustName", lbl_Customer.Text)

        rview.ServerReport.SetParameters(New ReportParameter() {FDate, TDate, CustomerParam, SiteParam, UserParam, OrgParam, OrgNameParam, Username, CustName})

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
            Response.AddHeader("Content-disposition", "attachment;filename=HistoryofCreditDet.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=HistoryofCreditDet.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()

    End Sub
    'Private Sub InitReportViewer(ByVal fromdate As Date, ByVal Todate As Date)
    '    Try





    '        Dim FDate As New ReportParameter
    '        FDate = New ReportParameter("FromDate", fromdate.ToString())

    '        Dim TDate As New ReportParameter
    '        TDate = New ReportParameter("ToDate", Todate.ToString())


    '        Dim ERPTable As New ReportParameter
    '        ERPTable = New ReportParameter("ERPTable", CStr(IIf(Me.ddl_ERPTable.SelectedIndex <= 0, "ALL", Me.ddl_ERPTable.SelectedValue)))



    '        With RVMain
    '            .Reset()
    '            .ShowParameterPrompts = False
    '            .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
    '            .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
    '            .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
    '            .ServerReport.SetParameters(New ReportParameter() {FDate, TDate, ERPTable})
    '            .ServerReport.Refresh()

    '        End With


    '    Catch Ex As Exception
    '        '  log.Error(GetExceptionInfo(Ex))
    '    End Try
    'End Sub
    'Protected Sub RVMain_BookmarkNavigation(ByVal sender As Object, ByVal e As Microsoft.Reporting.WebForms.BookmarkNavigationEventArgs) Handles RVMain.BookmarkNavigation
    '    Me.lblPopmsg.Text = ""
    '    Me.lblPopmsg.Text = e.BookmarkId.ToString()

    '    Me.MapWindow.VisibleOnPageLoad = True



    'End Sub
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInput() Then
            rpbFilter.Items(0).Expanded = False

            BindData()
        Else


            Args.Visible = False
        End If


    End Sub
    Function ValidateInput() As Boolean
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
            MessageBoxValidation("Please select the Organization", "Validation")
            Return bretval
        End If
    End Function
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub
    Private Sub ddl_Customer_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddlCustomer.ItemsRequested
        Try
            If ddlOrganization.SelectedIndex > 0 Then
                Dim Objrep As New SalesWorx.BO.Common.Reports()


                Dim dt As New DataTable

                If dt.Rows.Count > 0 Then
                    dt.Rows.Clear()
                End If


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

                    ddlCustomer.Items.Add(item)
                    item.DataBind()
                Next
            End If

        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        Try
            ObjCommon = New SalesWorx.BO.Common.Common()

            Me.ddl_User.DataSource = ObjCommon.GetCreditDetailUsers(Err_No, Err_Desc, ddlOrganization.SelectedValue)
            Me.ddl_User.DataTextField = "Username"
            Me.ddl_User.DataValueField = "User_ID"
            Me.ddl_User.DataBind()
            Me.ddl_User.SelectedIndex = 0
        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            ObjCommon = Nothing
        End Try

    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()

        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If

        ddlCustomer.ClearSelection()
        ddlCustomer.Items.Clear()
        ddlCustomer.Text = ""
        ddl_User.ClearSelection()

        BindData()

        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Now()

        Args.Visible = False
        gvRep.Visible = False
  

    End Sub
End Class