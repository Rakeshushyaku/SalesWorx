Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Partial Public Class RepPrincipal_SalesManWiseSales
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer
    Private ReportPath As String = "PrincipalWiseFSRSalesReturns"

    Private Const PageID As String = "P231"
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

                LoadOrgDetails()

                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                txtToDate.SelectedDate = Now()


                ddlAgency.DataSource = ObjCommon.GetAgencyList(Err_No, Err_Desc, ddlOrganization.SelectedValue)
                ddlAgency.DataBind()


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
    Sub LoaAgency()
        ddlAgency.DataTextField = "Agency"
        ddlAgency.DataValueField = "Agency"
        ObjCommon = New SalesWorx.BO.Common.Common()
        Dim Objrep As New SalesWorx.BO.Common.Reports()
        ddlAgency.DataSource = ObjCommon.GetAgencyList(Err_No, Err_Desc, ddlOrganization.SelectedValue)
        ddlAgency.DataBind()
        ddlAgency.Items.Insert(0, New RadComboBoxItem("Select Agency", "0"))
        Objrep = Nothing
    End Sub
    Sub LoadOrgDetails()
        If Not (ddlOrganization.SelectedItem.Value = "0") Then

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New SalesWorx.BO.Common.Common()


            ddlVan.DataTextField = "SalesRep_Name"
            ddlVan.DataValueField = "SalesRep_ID"
            ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)

            ddlVan.DataBind()

            For Each itm As RadComboBoxItem In ddlVan.Items
                itm.Checked = True
            Next

           

            LoaAgency()

        Else

            ddlVan.Items.Clear()
        End If

    End Sub
    Private Sub BindReport()
        Dim SearchQuery As String = ""

        rpbFilter.Items(0).Expanded = False
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        Dim ObjReport As New SalesWorx.BO.Common.Reports

        Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems

        Dim van As String = ""
        Dim vantxt As String = ""
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

        Dim NoofVans As Integer = 0
        If van = "0" Then
            lbl_van.Text = "All"
            NoofVans = ddlVan.Items.Count
        Else
            NoofVans = ddlVan.CheckedItems.Count
            lbl_van.Text = vantxt
        End If

        Dim Agency As String = "0"
        If ddlAgency.SelectedIndex > 0 Then
            Agency = ddlAgency.SelectedItem.Value
            lbl_Principle.Text = ddlAgency.SelectedItem.Text
        Else
            lbl_Principle.Text = "All"
        End If

         
        lbl_org.Text = ddlOrganization.SelectedItem.Text
        lbl_From.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
        lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")


        HORGID.Value = ddlOrganization.SelectedItem.Value
        HDate.Value = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
        HToDate.Value = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")
        HCID.Value = Agency
        HFSRID.Value = van

        Args.Visible = True
        Dim dt As New DataTable
        dt = ObjReport.GetSalesManWiseAgencyWiseSalesAndReturns(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, van, objUserAccess.UserID, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"), Agency)
        gvRep.DataSource = dt
        gvRep.DataBind()

        Dim dtcurrency As DataTable
        dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
        Dim Currency As String = ""
        If dtcurrency.Rows.Count > 0 Then
            lblDecimal.Text = "N" & dtcurrency.Rows(0)("Decimal_Digits")
            Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
        End If
        lbl_Currency.Text = Currency


        Dim StrSummary As String = ""
        Dim sum = dt.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("SValue")))

        StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>Total Sales " & Currency & "<div class='text-primary'>" & Format(sum, lblDecimal.Text) & "</div></div></div>"

        Dim sumR = dt.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("RValue")))

        StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>Total Returns " & Currency & "<div class='text-primary'>" & Format(sumR, lblDecimal.Text) & "</div></div></div>"


        summary.InnerHtml = StrSummary

    End Sub
    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender
        For Each column As GridColumn In gvRep.MasterTableView.Columns
            If column.UniqueName = "SValue" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
            End If
            If column.UniqueName = "RValue" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
            End If
        Next
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
        Dim FDate As New ReportParameter
        FDate = New ReportParameter("Fromdate", CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"))
        Dim TDate As New ReportParameter
        TDate = New ReportParameter("Todate", CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))


        Dim OrgId As New ReportParameter
        OrgId = New ReportParameter("OrgID", CStr(ddlOrganization.SelectedItem.Value.ToString()))


        Dim van As String = ""
        Dim vantxt As String = ""
        For Each li As RadComboBoxItem In ddlVan.CheckedItems
            van = van & li.Value & ","
            vantxt = vantxt & li.Text & ","
        Next
        If vantxt <> "" Then
            vantxt = vantxt.Substring(0, vantxt.Length - 1)
        End If
        If van = "" Then
            van = "0"
        End If

        Dim SID As New ReportParameter
        SID = New ReportParameter("Salesrep_ID", van)

        Dim AgencyID As New ReportParameter


        Dim Agency As String = "0"
        If ddlAgency.SelectedIndex >= 0 Then
            Agency = ddlAgency.SelectedItem.Value
        End If

        AgencyID = New ReportParameter("Agency", Agency)


        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

        Dim Uid As New ReportParameter
        Uid = New ReportParameter("Uid", objUserAccess.UserID)


        rview.ServerReport.SetParameters(New ReportParameter() {OrgId, SID, FDate, TDate, AgencyID, Uid})

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
            Response.AddHeader("Content-disposition", "attachment;filename=PrincipalVanWiseSales.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=PrincipalVanWiseSales.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()
    End Sub
    Private Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrganization.SelectedIndexChanged
        LoadOrgDetails()
        Args.Visible = False
        gvRep.Visible = False
        divCurrency.Visible = False
        summary.InnerHtml = ""
    End Sub

    Private Sub SearchBtn_Click(sender As Object, e As EventArgs) Handles SearchBtn.Click

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
        If ValidateInputs() Then
            gvRep.Visible = True
            divCurrency.Visible = True
            BindReport()
        Else
            gvRep.Visible = False
            Args.Visible = False
            divCurrency.Visible = False
            summary.InnerHtml = ""
        End If
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
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

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        ddlVan.ClearCheckedItems()
        ddlVan.Items.Clear()

        ddlAgency.ClearSelection()
        ddlAgency.Items.Clear()
        LoadOrgDetails()


        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Now()

        Args.Visible = False
        gvRep.Visible = False
        divCurrency.Visible = False
        summary.InnerHtml = ""
        lblmsgUOM.Text = ""
    End Sub
End Class