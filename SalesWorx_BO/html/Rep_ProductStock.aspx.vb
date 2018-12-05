Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Partial Public Class Rep_ProductStock
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer
    Private ReportPath As String = "ProductStock"

    Private Const PageID As String = "P243"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub RepPrincipalWiseSales_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'If Not IsNothing(Me.Master) Then

        '    Dim masterScriptManager As ScriptManager
        '    masterScriptManager = CType(Master.FindControl("ScriptManager1"), ScriptManager)

        '    ' Make sure our master page has the script manager we're looking for
        '    If Not IsNothing(masterScriptManager) Then

        '        ' Turn off partial page postbacks for this page
        '        masterScriptManager.EnablePartialRendering = False
        '    End If

        'End If
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


                Dim objUserAccess As UserAccess
                objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

                ddlAgency.DataSource = ObjCommon.GetAgencyList(Err_No, Err_Desc, ddlOrganization.SelectedValue)
                ddlAgency.DataBind()
                ddlAgency.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))


                ddlSKU.DataSource = ObjCommon.GetProductsByOrg_Agency(Err_No, Err_Desc, ddlOrganization.SelectedValue, ddlAgency.SelectedItem.Value)
                ddlSKU.DataTextField = "Description"
                ddlSKU.DataValueField = "Inventory_Item_ID"
                ddlSKU.DataBind()
                ddlSKU.Items.Insert(0, New RadComboBoxItem("-- Select a product --", "0"))

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
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Private Sub InitReportViewer()
        Try

            rpbFilter.Items(0).Expanded = False
            RepSec.Visible = True
            RVMain.Visible = True
            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("Orgid", CStr(ddlOrganization.SelectedItem.Value.ToString()))


            Dim AgencyID As New ReportParameter
            AgencyID = New ReportParameter("AGENCY", ddlAgency.SelectedItem.Value)

            Dim SKUID As New ReportParameter
            SKUID = New ReportParameter("SKUID", ddlSKU.SelectedItem.Value)

            With RVMain
                .Reset()
                .Visible = True
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                .ServerReport.SetParameters(New ReportParameter() {OrgId, AgencyID, SKUID})
                .ServerReport.Refresh()

            End With


        Catch Ex As Exception
            '  log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub
    

    Private Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrganization.SelectedIndexChanged
        ObjCommon = New SalesWorx.BO.Common.Common()
        ddlAgency.DataSource = ObjCommon.GetAgencyList(Err_No, Err_Desc, ddlOrganization.SelectedValue)
        ddlAgency.DataBind()
        ddlAgency.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))
        RVMain.Reset()
    End Sub

    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ddlOrganization.SelectedItem.Value = "0" Then
            RepSec.Visible = False
            RVMain.Visible = False
            MessageBoxValidation("Please select the organisation", "Validation")
        Else

            InitReportViewer()

        End If

    End Sub

    Protected Sub ddlAgency_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAgency.SelectedIndexChanged
        ObjCommon = New SalesWorx.BO.Common.Common
        ddlSKU.DataSource = ObjCommon.GetProductsByOrg_Agency(Err_No, Err_Desc, ddlOrganization.SelectedValue, ddlAgency.SelectedItem.Value)
        ddlSKU.DataTextField = "Description"
        ddlSKU.DataValueField = "Inventory_Item_ID"
        ddlSKU.DataBind()
        ddlSKU.Items.Insert(0, New RadComboBoxItem("-- Select a product --", "0"))
        RVMain.Reset()
    End Sub
End Class