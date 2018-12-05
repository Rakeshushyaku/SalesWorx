Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports System.Globalization
Partial Public Class Rep_NearExpiry
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "NearExpiryProducts"

    Private Const PageID As String = "P281"
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
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
            Exit Sub
        End If
        Try
            If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then

                Dim objUserAccess As UserAccess
                objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
                ObjCommon = New Common()
                ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
                ddlVan.DataBind()
                ddlvan.Items.Insert(0, "(Select)")
                ddlvan.Items(0).Value = "-1"

                ddlCustomer.DataSource = ObjCommon.GetOutlet(Err_No, Err_Desc, ddlOrganization.SelectedValue)
                ddlCustomer.DataBind()
                ddlCustomer.DataTextField = "Outlet"
                ddlCustomer.DataValueField = "CustomerID"
                ddlCustomer.Items.Insert(0, New ListItem("-- All --", "-1$-1"))

                ddlSKU.DataSource = Nothing
                ddlSKU.DataSource = ObjCommon.GetSKU(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
                ddlSKU.DataTextField = "SKU"
                ddlSKU.DataValueField = "Inventory_item_ID"
                ddlSKU.DataBind()
                ddlSKU.Items.Insert(0, New ListItem("-- All --", "-1"))
                RVMain.Reset()

            Else
                ddlVan.Items.Clear()
                ddlVan.Items.Insert(0, New ListItem("-- Select a value --"))
                RVMain.Reset()
            End If
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
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


               
                Dim OrgID As New ReportParameter
                Dim OrgName As New ReportParameter
                OrgID = New ReportParameter("OrgID", ddlOrganization.SelectedItem.Value)
                OrgName = New ReportParameter("OrgName", ddlOrganization.SelectedItem.Text)

                Dim objUserAccess As UserAccess
                objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
                ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
                ddlVan.DataTextField = "SalesRep_Name"
                ddlVan.DataValueField = "SalesRep_ID"
                ddlVan.DataBind()

                ddlCustomer.DataSource = ObjCommon.GetOutlet(Err_No, Err_Desc, ddlOrganization.SelectedValue)
                ddlCustomer.DataBind()
                ddlCustomer.DataTextField = "Outlet"
                ddlCustomer.DataValueField = "CustomerID"
                ddlCustomer.Items.Insert(0, New ListItem("-- All --", "-1$-1"))

                ddlSKU.DataSource = Nothing
                ddlSKU.DataSource = ObjCommon.GetSKU(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
                ddlSKU.DataTextField = "SKU"
                ddlSKU.DataValueField = "Inventory_item_ID"
                ddlSKU.DataBind()
                ddlSKU.Items.Insert(0, New ListItem("-- All --", "-1"))
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

Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
    If ddlOrganization.SelectedItem.Value = "0" Then
            MessageBoxValidation("Please select the organization")
            Exit Sub
    End If
    
    If txt_nofodays.Text.Trim() = "" Then
            MessageBoxValidation("Please Enter the no of days")
            Exit Sub
    End If
     Dim OrgId As New ReportParameter
        OrgId = New ReportParameter("OrgId", CStr(ddlOrganization.SelectedItem.Value.ToString()))

        Dim VanID As New ReportParameter
        VanID = New ReportParameter("FSRID", ddlvan.SelectedItem.Value)

        Dim SKUID As New ReportParameter
        SKUID = New ReportParameter("inventoryItemID", ddlSKU.SelectedItem.Value)

        Dim scustomerid As String
        scustomerid = ddlCustomer.SelectedItem.Value
        Dim customerids() As String
        customerids = scustomerid.Split("$")

        Dim CustomerID As New ReportParameter
        CustomerID = New ReportParameter("CustomerID", customerids(0))

        Dim SiteUseID As New ReportParameter
        SiteUseID = New ReportParameter("siteUseID", customerids(1))

        Dim Noofdays As New ReportParameter
        Noofdays = New ReportParameter("Noofdays", txt_nofodays.Text)
        With RVMain
            .Reset()
            .Visible = True
            .ShowParameterPrompts = False
            .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
            .ServerReport.SetParameters(New ReportParameter() {OrgId, VanID, SKUID, CustomerID, SiteUseID, Noofdays})
            '.ServerReport.Refresh()

        End With
End Sub
Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        Exit Sub
    End Sub
End Class