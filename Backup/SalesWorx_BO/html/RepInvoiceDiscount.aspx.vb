

Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms

Partial Public Class RepInvoiceDiscount
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Dim ObjCustomer As Customer
    Private ReportPath As String = "InvoiceDiscountDetails"
    Dim dv As New DataView
    Private Const PageID As String = "P212"
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
            Session("OrdersDT") = Nothing
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
                txtFromDate.Text = Format(Now().Date, "dd/MM/yyyy")
                txtToDate.Text = Format(Now().Date, "dd/MM/yyyy")
              
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
   
    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        MpInfoError.Show()
        Exit Sub
    End Sub

    Private Sub InitReportViewer(ByVal FilterValue As String)
        Try

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim Searchvalue As New ReportParameter
            Searchvalue = New ReportParameter("SearchParams", CStr(IIf(FilterValue Is Nothing, "", FilterValue)))



            Dim OrgID As New ReportParameter
            OrgID = New ReportParameter("OrgID", CStr(ddlOrganization.SelectedValue.ToString()))

            With RVMain
                .Reset()
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                .ServerReport.SetParameters(New ReportParameter() {Searchvalue, OrgID})
                '.ServerReport.Refresh()

            End With


        Catch Ex As Exception
            '  log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub
    Private Sub BindData()
        Dim SearchQuery As String = ""
        Try
            ObjCustomer = New Customer()
            ObjCommon = New Common()
            If (ddlCustomer.SelectedItem.Value = "-- Select a value --" And ddlVan.SelectedItem.Value = "-- Select a value --" And txtFromDate.Text = "" And txtToDate.Text = "") Then
                SearchQuery = ""
            Else
                If ddlVan.SelectedValue <> "-- Select a value --" Then
                    SearchQuery = " And A.Created_By=" & ddlVan.SelectedValue
                Else
                    SearchQuery = " And A.Created_By in (" & ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID) & ")"
                End If

                If ddlCustomer.SelectedValue <> "-- Select a value --" Then
                    SearchQuery = SearchQuery & " AND (LTRIM(STR(Y.Customer_ID)) + '$' + LTRIM(STR(Y.Site_Use_ID)))='" & ddlCustomer.SelectedValue & "' "
                End If

                'If ddlOrderType.SelectedValue = "Invoice" Then
                '    SearchQuery = SearchQuery & " AND OrderType='Invoice'"
                'End If
                'If ddlOrderType.SelectedValue = "Order" Then
                '    SearchQuery = SearchQuery & " AND OrderType='Order'"
                'End If


                If txtFromDate.Text <> "" Then
                    Dim TemFromDateStr As String = txtFromDate.Text
                    Dim DateArr As Array = TemFromDateStr.Split("/")
                    TemFromDateStr = DateArr(1) & "/" & DateArr(0) & "/" & DateArr(2)
                    SearchQuery = SearchQuery & " And A.System_Order_Date >= '" & TemFromDateStr & "'"
                    If txtToDate.Text = "" Then
                        SearchQuery = SearchQuery & " And A.System_Order_Date <= '" & TemFromDateStr & " 23:59:59'"
                    End If
                End If
                If txtToDate.Text <> "" Then
                    Dim TemToDateStr As String = txtToDate.Text
                    Dim DateArr As Array = TemToDateStr.Split("/")
                    TemToDateStr = DateArr(1) & "/" & DateArr(0) & "/" & DateArr(2)
                    SearchQuery = SearchQuery & " And A.System_Order_Date <= '" & TemToDateStr & " 23:59:59'"
                End If
            End If
            If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then
                ' SearchQuery = SearchQuery & " And A.Ship_To_Site_ID=" & ddlOrganization.SelectedItem.Value
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
            Err_No = "74089"
            '  Err_Desc = ex.Message
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
        Finally
            ObjCustomer = Nothing
        End Try
    End Sub

   
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click

        Dim TemFromDateStr As String = txtFromDate.Text
        Dim DateArr As Array = TemFromDateStr.Split("/")
        If DateArr.Length = 3 Then
            TemFromDateStr = DateArr(1) & "/" & DateArr(0) & "/" & DateArr(2)
        End If
        Dim TemToDateStr As String = txtToDate.Text
        Dim DateArr1 As Array = TemToDateStr.Split("/")
        If DateArr1.Length = 3 Then
            TemToDateStr = DateArr1(1) & "/" & DateArr1(0) & "/" & DateArr1(2)
        End If

        If Not IsDate(TemFromDateStr) Then
            MessageBoxValidation("Enter valid ""From date"".")
            SetFocus(txtFromDate)
            Exit Sub
        End If

        If Not IsDate(TemToDateStr) Then
            MessageBoxValidation("Enter valid ""To date"".")
            SetFocus(TemToDateStr)
            Exit Sub
        End If
        If CDate(TemFromDateStr) > CDate(TemToDateStr) Then
            MessageBoxValidation("Start Date should not be greater than End Date.")
            Exit Sub
        End If
        
        BindData()
    End Sub

    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New Common()
            ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())
            ddlVan.DataBind()
            ddlVan.Items.Insert(0, New ListItem("-- Select a value --"))

            ddlCustomer.DataSource = ObjCommon.GetCustomerByCriteria(Err_No, Err_Desc, ddlOrganization.SelectedValue)
            ddlCustomer.DataBind()
            ddlCustomer.Items.Insert(0, New ListItem("-- Select a value --"))

        End If

    End Sub


End Class