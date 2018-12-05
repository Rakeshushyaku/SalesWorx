Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Partial Public Class Rep_SalesByAgency
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "SalesByAgency_new"

    Private Const PageID As String = "P110"
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
    Private Sub BindData()
        Dim SearchQuery As String = ""
        Dim SalesRepId As Integer = 0
        Dim CustId As Integer = 0
        Dim fromdate As DateTime
        Dim todate As DateTime
        fromdate = System.Data.SqlTypes.SqlDateTime.Null
        todate = System.Data.SqlTypes.SqlDateTime.Null


        Try
            ObjCustomer = New Customer()
            ObjCommon = New SalesWorx.BO.Common.Common()
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
           

            Dim Agency As String = Nothing
            Dim collection As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddl_Agency.CheckedItems


            For Each li As Telerik.Web.UI.RadComboBoxItem In collection
                If String.IsNullOrEmpty(Agency) Then
                    Agency = li.Value
                Else
                    Agency = Agency & "," & li.Value
                End If
            Next


            If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then
                SearchQuery = SearchQuery & " And Site_Use_ID=" & ddlOrganization.SelectedItem.Value
                InitReportViewer(Agency, ddlOrganization.SelectedItem.Value)
            Else
                MessageBoxValidation("Select an Organization.")
            End If
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

    Private Sub InitReportViewer(ByVal AgencyList As String, ByVal OID As String)
        Try

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim myParamUserId As New ReportParameter
            myParamUserId = New ReportParameter("Uid", objUserAccess.UserID)

            Dim Agency As New ReportParameter
            Agency = New ReportParameter("Agency", CStr(IIf(AgencyList Is Nothing, "0", AgencyList)))


            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("OrgID", CStr(IIf(OID Is Nothing, "0", OID)))
            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                .ServerReport.SetParameters(New ReportParameter() {Agency, OrgId, myParamUserId})
                .ServerReport.Refresh()
            End With


        Catch Ex As Exception
            '  log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub



    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click

        If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then
            BindData()

        Else
            MessageBoxValidation("Select an organization.")
        End If
    End Sub
    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()

        Exit Sub
    End Sub

    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged


        If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New Common()
           

            ddl_Agency.DataTextField = "Agency"
            ddl_Agency.DataValueField = "Agency"
            ddl_Agency.DataSource = ObjCommon.GetAgencyByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)

            ddl_Agency.DataBind()
        Else
            ddl_Agency.Items.Clear()
        End If
        RVMain.Reset()
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub

    Protected Sub ddl_Agency_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ddl_Agency.SelectedIndexChanged

    End Sub
End Class