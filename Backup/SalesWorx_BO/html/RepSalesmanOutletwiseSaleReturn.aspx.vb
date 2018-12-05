Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports System.Globalization

Partial Public Class RepSalesmanOutletwiseSaleReturn
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
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
                ObjCommon = New Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New ListItem("-- Select a value --"))
                txtFromDate.Text = Format(Now().Date, "dd-MMM-yyyy")
                txtToDate.Text = Format(Now().Date, "dd-MMM-yyyy")

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

            Dim FDate As New ReportParameter
            If txtFromDate.Text.Trim() IsNot String.Empty Then
                FDate = New ReportParameter("FromDate", txtFromDate.Text.Trim())
            Else
                FDate = New ReportParameter("FromDate", Now.ToString("dd-MMM-yyyy"))
            End If


            Dim TDate As New ReportParameter
            If txtToDate.Text.Trim() IsNot String.Empty Then
                TDate = New ReportParameter("ToDate", txtToDate.Text)
            Else
                TDate = New ReportParameter("ToDate", Now.ToString("dd-MMM-yyyy"))
            End If

            Dim SiteID As New ReportParameter
            Dim CusID As New ReportParameter
            Dim Outlet As New ReportParameter
            Dim RepName As New ReportParameter

            If ddlCustomer.SelectedIndex <> 0 Then
                Dim Arr As Array = ddlCustomer.SelectedItem.Value.Split("$")
                SiteID = New ReportParameter("SID", CInt(Arr(1)))
                CusID = New ReportParameter("CID", CInt(Arr(0)))
                Outlet = New ReportParameter("Outlet", ddlCustomer.SelectedItem.Text.Trim())
            Else
                SiteID = New ReportParameter("SID", 0)
                CusID = New ReportParameter("CID", 0)
                Outlet = New ReportParameter("Outlet", "All")
            End If

            Dim RepID As New ReportParameter
            If ddlVan.SelectedIndex <> 0 Then
                RepID = New ReportParameter("RepID", CInt(ddlVan.SelectedItem.Value))
                RepName = New ReportParameter("Salesman", ddlVan.SelectedItem.Text.Trim())
            Else
                RepID = New ReportParameter("RepID", 0)
                RepName = New ReportParameter("Salesman", "All")
            End If

            Dim OrgID As New ReportParameter
            Dim OrgName As New ReportParameter
            OrgID = New ReportParameter("OrgID", ddlOrganization.SelectedItem.Value)
            OrgName = New ReportParameter("OrgName", ddlOrganization.SelectedItem.Text)


            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                .ServerReport.SetParameters(New ReportParameter() {FDate, TDate, SiteID, CusID, OrgID, RepID, Outlet, OrgName, RepName})
                .ServerReport.Refresh()

            End With
        Catch Ex As Exception
            '  log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub

    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click

        If ddlOrganization.SelectedValue = "-- Select a value --" Then
            MessageBoxValidation("Select organization.")
            Exit Sub
        End If
        If Not IsDate(txtFromDate.Text) Then
            MessageBoxValidation("Enter a valid from date.")
            Exit Sub
        End If
        If Not IsDate(txtToDate.Text) Then
            MessageBoxValidation("Enter a valid to date.")
            Exit Sub
        End If
        If CDate(txtFromDate.Text) > CDate(txtToDate.Text) Then
            MessageBoxValidation("Start Date should not be greater than End Date.")
            Exit Sub
        End If
        InitReportViewer()
        
    End Sub
    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        MpInfoError.Show()
        Exit Sub
    End Sub
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged

        ddlCustomer.DataSource = Nothing
        ddlCustomer.Items.Clear()
        ddlVan.DataSource = Nothing
        ddlVan.Items.Clear()
        If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New Common()
            ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
            ddlVan.DataBind()
            ddlVan.Items.Insert(0, New ListItem("-- All --"))

            ddlCustomer.DataSource = ObjCommon.GetOutlet(Err_No, Err_Desc, ddlOrganization.SelectedValue)
            ddlCustomer.DataBind()
            ddlCustomer.Items.Insert(0, New ListItem("-- All --"))

        End If
        RVMain.Reset()
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub

End Class
