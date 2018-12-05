Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports System.Globalization
Partial Public Class RepExcessReturns
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "ExcessReturnReport"

    Private Const PageID As String = "P252"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single

    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub RepOutletSKUwiseSalesReturn_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
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
                RVMain.Visible = False
                ObjCommon = New Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()


                txtFromDate.Text = Format(Now().Date, "dd-MMM-yyyy")
                txtToDate.Text = Format(Now().Date, "dd-MMM-yyyy")

                ObjCommon = New Common()
                ddlCustomer.DataSource = ObjCommon.GetOutlet(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
                ddlCustomer.DataBind()
                ddlCustomer.Items.Insert(0, New ListItem("-- All --"))

                ddSKU.DataSource = ObjCommon.GetSKU(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
                ddSKU.DataBind()
                ddSKU.Items.Insert(0, New ListItem("-- All --"))

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
    Private Sub InitReportViewer()
        Try


            Dim FDate As New ReportParameter
            FDate = New ReportParameter("Fromdate", txtFromDate.Text.Trim())

            Dim TDate As New ReportParameter
            TDate = New ReportParameter("Todate", txtToDate.Text.Trim)

            Dim SiteID As New ReportParameter
            Dim CusID As New ReportParameter

            If ddlCustomer.SelectedIndex <> 0 Then
                Dim Arr As Array = ddlCustomer.SelectedItem.Value.Split("$")
                SiteID = New ReportParameter("SiteUSeid", CInt(Arr(1)))
                CusID = New ReportParameter("CustomerID", CInt(Arr(0)))

            Else
                SiteID = New ReportParameter("SiteUSeid", 0)
                CusID = New ReportParameter("CustomerID", 0)

            End If

            Dim InvID As New ReportParameter
            If ddSKU.SelectedIndex <> 0 Then
                InvID = New ReportParameter("SKUID", CInt(ddSKU.SelectedItem.Value))
            Else
                InvID = New ReportParameter("SKUID", 0)
            End If

            Dim OrgID As New ReportParameter
            Dim Cutoff As New ReportParameter

            OrgID = New ReportParameter("OrgID", ddlOrganization.SelectedItem.Value)
            Cutoff = New ReportParameter("Cutoff", ConfigurationSettings.AppSettings("Excess" & ddlOrganization.SelectedItem.Text.Replace(" ", "")))


            With RVMain
                .Visible = True
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                .ServerReport.SetParameters(New ReportParameter() {FDate, TDate, SiteID, CusID, InvID, Cutoff, OrgID})
                '.ServerReport.Refresh()

            End With


        Catch Ex As Exception
            log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            If txtFromDate.Text = "" Or txtToDate.Text = "" Then
                MessageBoxValidation("Select a date range.")
                Exit Sub
            End If
            If ddlOrganization.SelectedValue = "-- Select a value --" Then
                MessageBoxValidation("Select organization.")
                Exit Sub
            End If
            InitReportViewer()
        Catch ex As Exception

        End Try
    End Sub
    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        Exit Sub
    End Sub

    Private Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrganization.SelectedIndexChanged
        Try
            RVMain.Visible = False
            ddlCustomer.DataSource = Nothing
            ddlCustomer.Items.Clear()
            ddSKU.DataSource = Nothing
            ddSKU.Items.Clear()
            ObjCommon = New Common()
            ddlCustomer.DataSource = ObjCommon.GetOutlet(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
            ddlCustomer.DataBind()
            ddlCustomer.Items.Insert(0, New ListItem("-- All --"))

            ddSKU.DataSource = ObjCommon.GetSKU(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
            ddSKU.DataBind()
            ddSKU.Items.Insert(0, New ListItem("-- All --"))
        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            ObjCommon = Nothing
        End Try
    End Sub
End Class