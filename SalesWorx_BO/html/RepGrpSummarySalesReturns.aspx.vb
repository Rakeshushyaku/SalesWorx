Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI

Partial Public Class RepGrpSummarySalesReturns
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer
    Private ReportPath As String = "GrpSummaryCustOutletSalesReturns"

    Private Const PageID As String = "P246"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub RepPrincipal_SalesManWiseSales_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
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
                RVMain.Visible = False
                ObjCommon = New SalesWorx.BO.Common.Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))

              

                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1

                    Loaddds()
                End If

                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")

                ddlCustomer.DataSource = ObjCommon.GetCustomerLocation(Err_No, Err_Desc)
                ddlCustomer.DataBind()
                ddlCustomer.Items.Insert(0, New RadComboBoxItem("Select Customer"))


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
    Private Sub Loaddds()
        Dim objUserAccess As UserAccess
        Try
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)


            ObjCommon = New SalesWorx.BO.Common.Common()
            ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())
            ddlVan.DataBind()
            ddlVan.Items.Insert(0, New RadComboBoxItem("Select Van"))

            ddlAgency.DataSource = ObjCommon.GetAgencyList(Err_No, Err_Desc, ddlOrganization.SelectedValue)
            ddlAgency.DataBind()
            ddlAgency.Items.Insert(0, New RadComboBoxItem("Select Principle"))

        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            objUserAccess = Nothing
        End Try
    End Sub

    Private Sub BindData()
        Dim SearchQuery As String = ""
        Try
            If ddlOrganization.SelectedValue = "0" Then
                MessageBoxValidation("Select organization.", "Validation")
                Exit Sub
            End If
            If Not IsDate(txtFromDate.SelectedDate) Then
                MessageBoxValidation("Enter a valid from date.", "Validation")
                Exit Sub
            End If
            If Not IsDate(txtToDate.SelectedDate) Then
                MessageBoxValidation("Enter a valid to date.", "Validation")
                Exit Sub
            End If
            If CDate(txtFromDate.SelectedDate) > CDate(txtToDate.SelectedDate) Then
                MessageBoxValidation("Start Date should not be greater than End Date.", "Validation")
                Exit Sub
            End If

            lbl_org.Text = ddlOrganization.SelectedItem.Text
            lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")

            If ddlCustomer.SelectedValue = "" Then
                lbl_Cust.Text = "All"
            Else
                lbl_Cust.Text = ddlCustomer.SelectedItem.Text
            End If

            If ddlAgency.SelectedValue = "" Then
                lbl_Principle.Text = "All"
            Else
                lbl_Principle.Text = ddlAgency.SelectedItem.Text
            End If

            If ddlVan.SelectedValue = "" Then
                lbl_van.Text = "All"
            Else
                lbl_van.Text = ddlVan.SelectedItem.Text
            End If

            rpbFilter.Items(0).Expanded = False
            Args.Visible = True
            reportblocker.Visible = True

            InitReportViewer()
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
    End Sub
    Private Sub InitReportViewer()
        Try

            RVMain.Visible = True
            Args.Visible = True

            reportblocker.Visible = True


            Dim FromDate As New ReportParameter
            FromDate = New ReportParameter("Fromdate", txtFromDate.SelectedDate.Value.ToString("dd-MMM-yyyy"))

            Dim ToDate As New ReportParameter
            ToDate = New ReportParameter("Todate", txtToDate.SelectedDate.Value.ToString("dd-MMM-yyyy"))

            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("OrgID", CStr(ddlOrganization.SelectedItem.Value.ToString()))

            Dim SID As New ReportParameter
            SID = New ReportParameter("SalesRep_ID", CStr(IIf(ddlVan.SelectedValue = "", "0", ddlVan.SelectedValue)))

            Dim AgencyID As New ReportParameter
            AgencyID = New ReportParameter("Agency", CStr(IIf(ddlAgency.SelectedValue = "", "0", ddlAgency.SelectedValue)))

            Dim CustomerID As New ReportParameter
            CustomerID = New ReportParameter("Customer", CStr(IIf(ddlCustomer.SelectedValue = "", "0", ddlCustomer.SelectedValue)))

            With RVMain
                .Reset()
                .Visible = True
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                .ServerReport.SetParameters(New ReportParameter() {OrgId, SID, FromDate, ToDate, AgencyID, CustomerID})
                '.ServerReport.Refresh()

            End With


        Catch Ex As Exception
            log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub
    Sub MessageBoxValidation(ByVal str As String, Title As String)
      
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Private Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrganization.SelectedIndexChanged
        ''Dim objUserAccess As UserAccess
        ''objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        ''ObjCommon = New Common()
        ''ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())
        ''ddlVan.DataBind()
        ''ddlVan.Items.Insert(0, New ListItem("-- Select a value --", "0"))
        ''ddlAgency.DataSource = ObjCommon.GetAgencyList(Err_No, Err_Desc, ddlOrganization.SelectedValue)
        ''ddlAgency.DataBind()
        ''ddlAgency.Items.Insert(0, New ListItem("-- Select a value --", "0"))
        ''RVMain.Visible = False

        If Not (ddlOrganization.SelectedItem.Value = "0") Then
            Loaddds()
        Else
            ddlVan.Items.Clear()
            ddlVan.Text = ""
            ddlAgency.Items.Clear()
            ddlAgency.Text = ""
            RVMain.Visible = False
            reportblocker.Visible = False
        End If
    End Sub
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If (ddlOrganization.SelectedItem.Value = "") Then
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
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
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
        Args.Visible = False
        reportblocker.Visible = False
        If ValidateInputs() Then
            BindData()
        End If
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


   

            Dim FromDate As New ReportParameter
            FromDate = New ReportParameter("Fromdate", txtFromDate.SelectedDate.Value.ToString("dd-MMM-yyyy"))

            Dim ToDate As New ReportParameter
            ToDate = New ReportParameter("Todate", txtToDate.SelectedDate.Value.ToString("dd-MMM-yyyy"))

            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("OrgID", CStr(ddlOrganization.SelectedItem.Value.ToString()))

            Dim SID As New ReportParameter
            SID = New ReportParameter("SalesRep_ID", CStr(IIf(ddlVan.SelectedValue = "", "0", ddlVan.SelectedValue)))

            Dim AgencyID As New ReportParameter
            AgencyID = New ReportParameter("Agency", CStr(IIf(ddlAgency.SelectedValue = "", "0", ddlAgency.SelectedValue)))

            Dim CustomerID As New ReportParameter
            CustomerID = New ReportParameter("Customer", CStr(IIf(ddlCustomer.SelectedValue = "", "0", ddlCustomer.SelectedValue)))






            rview.ServerReport.SetParameters(New ReportParameter() {FromDate, ToDate, OrgId, AgencyID, CustomerID, SID})

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
                Response.AddHeader("Content-disposition", "attachment;filename=GrpSummaryCustOutletSalesReturns.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=GrpSummaryCustOutletSalesReturns.xls")
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
        ddlVan.ClearSelection()
        ddlVan.Items.Clear()

        ddlCustomer.ClearSelection()
        ddlCustomer.Text = ""

        ddlAgency.ClearSelection()
        ddlAgency.Text = ""
        ddlAgency.Items.Clear()

        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If

        Loaddds()

        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Now()

        RVMain.Visible = False
        Args.Visible = False
        reportblocker.Visible = False
        lblmsgUOM.Text = ""
    End Sub
End Class