Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Partial Public Class Rep_MonthlyVisitReport
    Inherits System.Web.UI.Page

     Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "MonthlyCustomerVisits"

    Private Const PageID As String = "P298"
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
                ObjCommon = New SalesWorx.BO.Common.Common()

                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New ListItem("-- Select a value --"))

                ddl_year.DataSource = ObjCommon.GetCustomerVisitMonth(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
                ddl_year.DataTextField = "monthYear"
                ddl_year.DataValueField = "monthYearv"
                ddl_year.DataBind()

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

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

            Dim collection As IList(Of RadComboBoxItem) = ddl_Van.CheckedItems

            Dim van As String = ""
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","
            Next

            If (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then
                MessageBoxValidation("Select an Organization.")
                Exit Sub
            End If
            If van = "" Then
                MessageBoxValidation("Select van.")
                Exit Sub
            Else
            van = van.Substring(0, van.Length - 1)
            End If
            Dim seldate() As String
            seldate = ddl_year.SelectedItem.Value.Split("-")
            If seldate(0) = 0 Then
                MessageBoxValidation("Select a month year.")
                Exit Sub
            End If
            InitReportViewer(van, ddlOrganization.SelectedItem.Value)
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

    Private Sub InitReportViewer(ByVal SIDList As String, ByVal OID As String)
        Try

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim myParamUserId As New ReportParameter
            myParamUserId = New ReportParameter("Uid", objUserAccess.UserID)

            Dim fromdate As String
            Dim Todate As String


            Dim seldate() As String
            seldate = ddl_year.SelectedItem.Value.Split("-")
            fromdate = CDate(seldate(1) & "/01/" & seldate(0)).ToString("dd-MMM-yyyy")
            Todate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(seldate(1) & "/01/" & seldate(0)))).ToString("dd-MMM-yyyy")
            Dim SearchParams As String
            SearchParams = "  and  A.SalesRep_ID in(" & SIDList & ")"
            SearchParams = SearchParams & " and A.Visit_Start_Date>=convert(datetime,'" & fromdate & "',103) and A.Visit_Start_Date<=convert(datetime,'" & Todate & " 23:59:59',103)"
            Dim SID As New ReportParameter
            SID = New ReportParameter("SearchParams", SearchParams)

            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("OrgId", ddlOrganization.SelectedItem.Value)

           Dim Month As New ReportParameter
            Month = New ReportParameter("Month", ddl_year.SelectedItem.Text)

            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                .ServerReport.SetParameters(New ReportParameter() {SID, OrgId, Month})
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
        MpInfoError.Show()
        Exit Sub
    End Sub

    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged


        If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New SalesWorx.BO.Common.Common()


            ddl_Van.DataTextField = "SalesRep_Name"
            ddl_Van.DataValueField = "SalesRep_ID"
            ddl_Van.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)

            ddl_Van.DataBind()

            ddl_year.DataSource = ObjCommon.GetCustomerVisitMonth(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
            ddl_year.DataTextField = "monthYear"
            ddl_year.DataValueField = "monthYearv"
            ddl_year.DataBind()
            ddl_year.Items.Insert(0, New ListItem("Select", "0-0"))
        Else
            ddl_Van.Items.Clear()
        End If
        RVMain.Reset()
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub

End Class