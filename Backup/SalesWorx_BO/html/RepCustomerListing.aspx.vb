Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms

Partial Public Class RepCustomerListing
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Dim ObjCustomer As Customer
    Private ReportPath As String = "CustomerListing"
    Private Const PageID As String = "P202"
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
                ddlSegment.DataSource = ObjCommon.GetCustomerSegmentList(Err_No, Err_Desc, SubQry)
                ddlSegment.DataBind()
                ddlSegment.Items.Insert(0, New ListItem("-- Select a value --"))
                ddlSalesDist.DataSource = ObjCommon.GetSalesDistrictList(Err_No, Err_Desc, SubQry)
                ddlSalesDist.DataBind()
                ddlSalesDist.Items.Insert(0, New ListItem("-- Select a value --"))
                'ddlType.DataSource = ObjCommon.GetCustomerTypeList(Err_No, Err_Desc, SubQry)
                'ddlType.DataBind()
                'ddlType.Items.Insert(0, New ListItem("-- Select a value --"))

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
            ObjCustomer = New Customer()
            If (ddlSegment.SelectedItem.Value = "-- Select a value --" And ddlSalesDist.SelectedItem.Value = "-- Select a value --" And ddlCust_Stat.SelectedItem.Text = "All" And txtCustomerNo.Text = "" And txtCustomerName.Text = "") Then
                SearchQuery = ""
            Else
                If ddlSegment.SelectedValue <> "-- Select a value --" Then
                    SearchQuery = " And B.Customer_Segment_ID='" & ddlSegment.SelectedValue & "'"
                End If
                If ddlSalesDist.SelectedValue <> "-- Select a value --" Then
                    SearchQuery = SearchQuery & " And B.Sales_District_ID='" & ddlSalesDist.SelectedValue & "'"
                End If
                'If ddlType.SelectedValue <> "-- Select a value --" Then
                '    SearchQuery = SearchQuery & " And A.Customer_Type='" & ddlType.SelectedValue & "'"
                'End If
                If ddlCust_Stat.SelectedValue = 1 Then
                    SearchQuery = SearchQuery & " And A.Cust_Status='Y' AND A.Cash_Cust='N'"
                ElseIf ddlCust_Stat.SelectedValue = 2 Then
                    SearchQuery = SearchQuery & " And A.Cust_Status='N' OR A.Credit_hold='Y'"
                ElseIf ddlCust_Stat.SelectedValue = 3 Then
                    SearchQuery = SearchQuery & " And A.Cust_Status='Y' AND A.Cash_Cust='Y'"
                End If
                If txtCustomerNo.Text <> "" Then
                    SearchQuery = SearchQuery & " And A.Customer_No like '%" & Utility.ProcessSqlParamString(txtCustomerNo.Text) & "%'"
                End If
                If txtCustomerName.Text <> "" Then
                    SearchQuery = SearchQuery & " And A.Customer_Name like '%" & Utility.ProcessSqlParamString(txtCustomerName.Text) & "%'"
                End If
            End If
            If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then
                ' SearchQuery = SearchQuery & " And A.Site_Use_ID=" & ddlOrganization.SelectedItem.Value
                InitReportViewer(SearchQuery)
            Else
                MessageBoxValidation("Select an organization.")
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
   
    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        ' MpInfoError.Show()
        Exit Sub
    End Sub
   

   
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        BindData()
    End Sub

   


    Private Sub InitReportViewer(ByVal FilterValue As String)
        Try

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim Searchvalue As New ReportParameter
            Searchvalue = New ReportParameter("SearchParams", CStr(IIf(FilterValue Is Nothing, "", FilterValue)))
            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("OrgId", CStr(ddlOrganization.SelectedItem.Value.ToString()))



            With RVMain
                .Reset()
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                .ServerReport.SetParameters(New ReportParameter() {Searchvalue, OrgId})
                '.ServerReport.Refresh()

            End With


        Catch Ex As Exception
            '  log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub


End Class
