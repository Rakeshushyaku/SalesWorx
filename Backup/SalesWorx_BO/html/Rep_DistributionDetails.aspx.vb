Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports System.Globalization

Partial Public Class Rep_DistributionDetails
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "DistributionCheckReport"

    Private Const PageID As String = "P126"
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


                ddlMonth.DataSource = ObjCommon.GetMonths(Err_No, Err_Desc)
                ddlMonth.DataTextField = "monthName"
                ddlMonth.DataValueField = "monVal"
                ddlMonth.DataBind()
                ddlMonth.Items.Insert(0, New ListItem("-- Select a value --", "0"))


                ddlyear.DataSource = ObjCommon.GetYear_Distribution(Err_No, Err_Desc)
                ddlyear.DataTextField = "yr"
                ddlyear.DataValueField = "yr"
                ddlyear.DataBind()
                ddlyear.Items.Insert(0, New ListItem("-- Select a value --", "0"))
              
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

            

            Dim SiteID As New ReportParameter
            Dim CusID As New ReportParameter
            Dim Outlet As New ReportParameter
            Dim RepName As New ReportParameter

            If ddlCustomer.SelectedIndex <> 0 Then
                Dim Arr As Array = ddlCustomer.SelectedItem.Value.Split("$")
                CusID = New ReportParameter("CID", CInt(Arr(0)))
            Else
                CusID = New ReportParameter("CID", 0)
            End If

            Dim SID As New ReportParameter
            If ddlVan.SelectedIndex <> 0 Then
                SID = New ReportParameter("SID", CInt(ddlVan.SelectedItem.Value))

            Else
                SID = New ReportParameter("SID", 0)

            End If

            Dim OrgID As New ReportParameter
            OrgID = New ReportParameter("OID", ddlOrganization.SelectedItem.Value)

            Dim MOnth As New ReportParameter
            MOnth = New ReportParameter("Month", ddlMonth.SelectedItem.Value)

            Dim Year As New ReportParameter
            Year = New ReportParameter("Year", ddlyear.SelectedItem.Value)

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

            Dim USRID As New ReportParameter
            USRID = New ReportParameter("Uid", objUserAccess.UserID)

            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                .ServerReport.SetParameters(New ReportParameter() {Year, MOnth, OrgID, CusID, SID, USRID})
                .ServerReport.Refresh()

            End With
        Catch Ex As Exception
            '  log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub

    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch1.Click

        If ddlOrganization.SelectedValue = "-- Select a value --" Then
            MessageBoxValidation("Select organization.")
            Exit Sub
        End If
        If ddlMonth.SelectedValue = "0" Then
            MessageBoxValidation("Select month.")
            Exit Sub
        End If
        If ddlyear.SelectedValue = "0" Then
            MessageBoxValidation("Select year.")
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
            ddlVan.Items.Insert(0, New ListItem("-- All --", "0"))

            ddlCustomer.DataSource = ObjCommon.GetOutlet(Err_No, Err_Desc, ddlOrganization.SelectedValue)
            ddlCustomer.DataBind()
            ddlCustomer.Items.Insert(0, New ListItem("-- All --", "0"))

        End If
        RVMain.Reset()
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub

   
End Class