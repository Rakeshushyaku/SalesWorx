Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms

Partial Public Class RepProductListing
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Dim ObjProduct As Product
    Private ReportPath As String = "ProductListing"
    Private Const PageID As String = "P201"
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
                ' BindData()
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
        Try
            ObjProduct = New Product()
            If (ddlOrganization.SelectedItem.Value = "-- Select a value --" And txtItemCode.Text = "" And txtDescription.Text = "") Then
                SearchQuery = ""
            ElseIf (txtItemCode.Text = "" And txtDescription.Text = "" And ddlOrganization.SelectedItem.Value <> "-- Select a Division --") Then
                SearchQuery = " And b.Organization_ID=" & ddlOrganization.SelectedItem.Value
            ElseIf (txtItemCode.Text <> "" And txtDescription.Text = "" And ddlOrganization.SelectedItem.Value = "-- Select a Division --") Then
                SearchQuery = "  and  b.Item_Code like '%" & Utility.ProcessSqlParamString(txtItemCode.Text) & "%'"
            ElseIf (txtItemCode.Text <> "" And txtDescription.Text <> "" And ddlOrganization.SelectedItem.Value = "-- Select a Division --") Then
                SearchQuery = " and  b.Item_Code like '%" & Utility.ProcessSqlParamString(txtItemCode.Text) & "%'" & " AND b.[Description] like '%" & Utility.ProcessSqlParamString(txtDescription.Text) & "%'"
            ElseIf (txtItemCode.Text <> "" And txtDescription.Text = "" And ddlOrganization.SelectedItem.Value <> "-- Select a Division --") Then
                SearchQuery = " And b.Organization_ID=" & ddlOrganization.SelectedItem.Value & " and  b.Item_Code like '%" & Utility.ProcessSqlParamString(txtItemCode.Text) & "%'"
            ElseIf (txtItemCode.Text <> "" And txtDescription.Text <> "" And ddlOrganization.SelectedItem.Value <> "-- Select a Division --") Then
                SearchQuery = " And b.Organization_ID=" & ddlOrganization.SelectedItem.Value & " and  b.Item_Code like '%" & Utility.ProcessSqlParamString(txtItemCode.Text) & "%'" & " AND b.[Description] like '%" & Utility.ProcessSqlParamString(txtDescription.Text) & "%'"
            ElseIf (txtItemCode.Text = "" And txtDescription.Text <> "" And ddlOrganization.SelectedItem.Value <> "-- Select a Division --") Then
                SearchQuery = " And b.Organization_ID=" & ddlOrganization.SelectedItem.Value & " AND b.[Description] like '%" & Utility.ProcessSqlParamString(txtDescription.Text) & "%'"
            End If
            If ddlProductType.SelectedValue = 1 Then
                SearchQuery = SearchQuery & " And a.Row_ID is not null"
            ElseIf ddlProductType.SelectedValue = 2 Then
                SearchQuery = SearchQuery & " And a.Row_ID is null"
            Else
                SearchQuery = SearchQuery
            End If

            InitReportViewer(SearchQuery)

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
            ObjProduct = Nothing
        End Try
    End Sub
 
    Private Sub InitReportViewer(ByVal FilterValue As String)
        Try

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim Searchvalue As New ReportParameter
            Searchvalue = New ReportParameter("SearchParams", CStr(IIf(FilterValue Is Nothing, "", FilterValue)))

            Dim Org As New ReportParameter
            Org = New ReportParameter("Org_ID", CStr(IIf(ddlOrganization.SelectedItem.Value = "0", "All", ddlOrganization.SelectedItem.Text)))

            Dim Type As New ReportParameter
            Type = New ReportParameter("Type", ddlProductType.SelectedItem.Text)


            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                .ServerReport.SetParameters(New ReportParameter() {Searchvalue, Org, Type})
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
        BindData()
    End Sub
    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        Exit Sub
    End Sub



End Class