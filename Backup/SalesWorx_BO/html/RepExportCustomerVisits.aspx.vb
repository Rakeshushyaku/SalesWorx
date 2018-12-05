Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms

Partial Public Class RepExportCustomerVisits
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "CustomerVisitsExport"

    Private Const PageID As String = "P212"
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
            ObjCommon = New Common()
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            If txtFromDate.Text <> "" Then
                fromdate = Convert.ToDateTime(txtFromDate.Text)
                SearchQuery = SearchQuery & " AND PlannedDate>=convert(datetime,'" & txtFromDate.Text & "',103)"

            End If

            If txtToDate.Text <> "" Then
                todate = Convert.ToDateTime(txtToDate.Text)
                SearchQuery = SearchQuery & " AND PlannedDate<=convert(datetime,'" & txtToDate.Text & " 23:59:59',103)"
            End If

            'If Not TemFromDateStr Is Nothing And Not TemToDateStr Is Nothing Then
            'SearchQuery = SearchQuery & "  And ( (CAST(Planned_Visit_Date AS DATETIME) BETWEEN   CAST(Convert(VARCHAr(10),'" & TemFromDateStr & "',121)AS DateTime) AND   CAST(Convert(VARCHAr(10),'" & TemToDateStr & "',121)AS DateTime))  OR (CAST(Actual_Visit_Date AS DATETIME) BETWEEN   CAST(Convert(VARCHAr(10),'" & TemFromDateStr & "',121)AS DateTime) AND   CAST(Convert(VARCHAr(10),'" & TemToDateStr & "',121)AS DateTime))) "
            'End If
            If Me.txtCustFrom.Text <> "" And Me.txtCustTo.Text <> "" Then
                SearchQuery = SearchQuery & " And  Customer_No BETWEEN '" & Utility.ProcessSqlParamString(txtCustFrom.Text) + "'  AND '" + Utility.ProcessSqlParamString(txtCustTo.Text) + "'"
            End If

            Dim FSR As String = Nothing
            For Each li As ListItem In chkSalesRep.Items
                If li.Selected = True Then
                    FSR = FSR & li.Value & ","
                End If
            Next

            Dim Agency As String = Nothing
            For Each li As ListItem In chkAgency.Items
                If li.Selected = True Then
                    Agency = Agency & li.Value & ","
                End If
            Next

            If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then
                'SearchQuery = SearchQuery & " And Site_Use_ID=" & ddlOrganization.SelectedItem.Value
                InitReportViewer(SearchQuery, Agency, FSR, Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text), ddlOrganization.SelectedItem.Value)
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

    Private Sub InitReportViewer(ByVal FilterValue As String, ByVal AgencyList As String, ByVal SalesRepList As String, ByVal sdate As String, ByVal edate As String, ByVal OID As String)
        Try

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim Searchvalue As New ReportParameter
            Searchvalue = New ReportParameter("SearchParams", CStr(IIf(FilterValue Is Nothing, "", FilterValue)))

            Dim ArticleFrom As New ReportParameter
            ArticleFrom = New ReportParameter("ArticleFrom", CStr(IIf(txtArticleFrom.Text = "", "0", Utility.ProcessSqlParamString(txtArticleFrom.Text))))

            Dim ArticleTo As New ReportParameter
            ArticleTo = New ReportParameter("ArticleTo", CStr(IIf(txtArticleTo.Text = "", "0", Utility.ProcessSqlParamString(txtArticleTo.Text))))

            Dim Agency As New ReportParameter
            Agency = New ReportParameter("Agency", CStr(IIf(AgencyList Is Nothing, "0", AgencyList)))

            Dim VanList As New ReportParameter
            VanList = New ReportParameter("VanList", CStr(IIf(SalesRepList Is Nothing, "0", SalesRepList)))

            Dim FDate As New ReportParameter
            FDate = New ReportParameter("FDate", CStr(IIf(sdate Is Nothing, Now.ToString(), sdate)))

            Dim TDate As New ReportParameter
            TDate = New ReportParameter("TDate", CStr(IIf(edate Is Nothing, Now.ToString(), edate)))

            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("OrgId", CStr(IIf(OID Is Nothing, "0", OID)))
            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                .ServerReport.SetParameters(New ReportParameter() {Searchvalue, ArticleFrom, ArticleTo, Agency, VanList, FDate, TDate, OrgId})
                '.ServerReport.Refresh()

            End With


        Catch Ex As Exception
            '  log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub

 

    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click

        If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then

            

            If Not IsDate(txtFromDate.Text) Then
                MessageBoxValidation("Enter valid ""From date"".")
                SetFocus(txtFromDate)
                Exit Sub
            End If

            If Not IsDate(txtToDate.Text) Then
                MessageBoxValidation("Enter valid ""To date"".")
                SetFocus(txtToDate)
                Exit Sub
            End If

            If CDate(txtFromDate.Text) > CDate(txtToDate.Text) Then
                MessageBoxValidation("Start Date should not be greater than End Date.")
                Exit Sub
            End If
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
        MpInfoError.Show()
        Exit Sub
    End Sub
  
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged


        If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New Common()
            chkSalesRep.DataTextField = "SalesRep_Name"
            chkSalesRep.DataValueField = "SalesRep_ID"
            chkSalesRep.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
            
            chkSalesRep.DataBind()

            chkAgency.DataTextField = "Agency"
            chkAgency.DataValueField = "Agency"
            chkAgency.DataSource = ObjCommon.GetAgencyByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
        
            chkAgency.DataBind()
        End If

    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub


   
End Class

