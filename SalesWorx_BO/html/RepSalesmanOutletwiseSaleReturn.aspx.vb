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
Partial Public Class RepSalesmanOutletwiseSaleReturn
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "SalesmanOutletwise"

    Private Const PageID As String = "P230"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single

    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not IsNothing(Me.Master) Then

            Dim masterScriptManager As ScriptManager
            masterScriptManager = CType(Master.FindControl("ScriptManager1"), ScriptManager)

            ' Make sure our master page has the script manager we're looking for
            If Not IsNothing(masterScriptManager) Then

                ' Turn off partial page postbacks for this page
                masterScriptManager.EnablePartialRendering = False
            End If

        End If

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
                ddlOrganization.DataTextField = "Description"
                ddlOrganization.DataValueField = "MAS_Org_ID"
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))

                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                End If

                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")

                If Not Err_Desc Is Nothing Then
                    log.Error(Err_Desc)
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
                End If
            Catch ex As Exception
                Err_No = "741266"
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


    Private Sub InitReportViewer()
        Try

            lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_org.Text = ddlOrganization.SelectedItem.Text

            Dim FDate As New ReportParameter
            FDate = New ReportParameter("FromDate", CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"))



            Dim TDate As New ReportParameter
            TDate = New ReportParameter("ToDate", CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))


            Dim SiteID As New ReportParameter
            Dim CusID As New ReportParameter
            Dim Outlet As New ReportParameter
            Dim RepName As New ReportParameter

            If ddlCustomer.SelectedIndex > 0 Then
                Dim Arr As Array = ddlCustomer.SelectedItem.Value.Split("$")
                SiteID = New ReportParameter("SID", CInt(Arr(1)))
                CusID = New ReportParameter("CID", CInt(Arr(0)))
                Outlet = New ReportParameter("Outlet", ddlCustomer.SelectedItem.Text.Trim())
                lbl_Cust.Text = ddlCustomer.SelectedItem.Text
            Else
                SiteID = New ReportParameter("SID", 0)
                CusID = New ReportParameter("CID", 0)
                Outlet = New ReportParameter("Outlet", "All")
                lbl_Cust.Text = "All"
            End If

            Dim RepID As New ReportParameter
            If ddlVan.SelectedIndex > 0 Then
                RepID = New ReportParameter("RepID", CInt(ddlVan.SelectedItem.Value))
                RepName = New ReportParameter("Salesman", ddlVan.SelectedItem.Text.Trim())
                lbl_van.Text = ddlVan.SelectedItem.Text
            Else
                RepID = New ReportParameter("RepID", 0)
                RepName = New ReportParameter("Salesman", "All")
                lbl_van.Text = "All"
            End If

            Dim OrgID As New ReportParameter
            Dim OrgName As New ReportParameter
            OrgID = New ReportParameter("OrgID", ddlOrganization.SelectedItem.Value)
            OrgName = New ReportParameter("OrgName", ddlOrganization.SelectedItem.Text)


            Dim UID As New ReportParameter
            UID = New ReportParameter("UID", CType(Session.Item("USER_ACCESS"), UserAccess).UserID)

            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                .ServerReport.SetParameters(New ReportParameter() {FDate, TDate, SiteID, CusID, OrgID, RepID, Outlet, OrgName, RepName, UID})
                .ServerReport.Refresh()

            End With
        Catch Ex As Exception
            '  log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If (ddlOrganization.SelectedValue = "0") Then
            MessageBoxValidation("Please select the Organisation", "Validation")
            Return bretval
        ElseIf Not IsDate(txtFromDate.SelectedDate) Then
            MessageBoxValidation("Enter From date.", "Validation")
            Return bretval
        ElseIf Not IsDate(txtToDate.SelectedDate) Then
            MessageBoxValidation("Enter To date.", "Validation")
            Return bretval
        ElseIf CDate(txtFromDate.SelectedDate) > CDate(txtToDate.SelectedDate) Then
            MessageBoxValidation("From Date should not be greater than To Date.", "Validation")
            Return bretval
        Else
            bretval = True
            Return bretval
        End If
    End Function
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            rpbFilter.Items(0).Expanded = False
            InitReportViewer()
            Args.Visible = True
            RVMain.Visible = True
            reportblocker.Visible = True


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
        Else
            Args.Visible = False
            RVMain.Visible = False
            reportblocker.Visible = False
        End If
    End Sub
    Sub MessageBoxValidation(ByVal str As String, Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Sub LoadOrgDetials()
        ddlCustomer.ClearSelection()
        ddlVan.ClearSelection()
        ddlCustomer.DataSource = Nothing
        ddlCustomer.Items.Clear()
        ddlVan.DataSource = Nothing
        ddlVan.Items.Clear()
        If Not (ddlOrganization.SelectedItem.Value = "0") Then

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New SalesWorx.BO.Common.Common()
            ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
            ddlVan.DataBind()
            ddlVan.Items.Insert(0, New RadComboBoxItem("Select Van", "0"))

            ddlCustomer.DataSource = ObjCommon.GetOutlet(Err_No, Err_Desc, ddlOrganization.SelectedValue)
            ddlCustomer.DataBind()
            ddlCustomer.Items.Insert(0, New RadComboBoxItem("Select Outlet", "0$0"))
        End If
    End Sub
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged

        LoadOrgDetials()
        RVMain.Reset()
        Args.Visible = False
        RVMain.Visible = False
        reportblocker.Visible = False
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        LoadOrgDetials()
        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")
        Args.Visible = False
        RVMain.Visible = False
        reportblocker.Visible = False
        RVMain.Reset()
        lblmsgUOM.Text = ""
    End Sub
End Class
