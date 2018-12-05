Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Partial Public Class Rep_POSMSurvey
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "POSMSurvey"

    Private Const PageID As String = "P278"
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
                If Not Request.QueryString("ID") Is Nothing Then
                    txtFromDate.Text = Format(Now().Date, "dd-MMM-yyyy")
                    txtToDate.Text = Format(Now().Date, "dd-MMM-yyyy")

                    Dim dt As New DataTable
                    dt = (New SalesWorx.BO.Common.Common).GetSalesOrgbyFsr(Err_No, Err_Desc, Request.QueryString("ID"))
                    If dt.Rows.Count > 0 Then
                        If Not ddlOrganization.Items.FindByValue(dt.Rows(0)("MAS_Org_ID").ToString) Is Nothing Then
                            ddlOrganization.ClearSelection()
                            ddlOrganization.Items.FindByValue(dt.Rows(0)("MAS_Org_ID").ToString).Selected = True

                            Dim objUserAccess As UserAccess
                            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

                            ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
                            ddlVan.DataBind()
                            ddlVan.Items.Insert(0, New ListItem("-- Select a value --", "0"))
                           

                            ddlCustomer.DataSource = ObjCommon.GetCustomerByCriteria(Err_No, Err_Desc, ddlOrganization.SelectedValue)
                            ddlCustomer.DataBind()
                            ddlCustomer.Items.Insert(0, New ListItem("-- Select a value --", "0$0"))
                            BindData()

                        End If
                    End If
                Else
                    txtFromDate.Text = Format(Now().Date, "dd-MMM-yyyy")
                    txtToDate.Text = Format(Now().Date, "dd-MMM-yyyy")
                End If



                'End If
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
            If ddlOrganization.SelectedItem.Value = "-- Select a value --" Then
                MessageBoxValidation("Select an Organization.")
                Exit Sub
            End If

            InitReportViewer()

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

    Private Sub InitReportViewer()
        Try

            Dim CustID As String
            Dim SiteUseID As String
            Dim Cust() As String
            Cust = ddlCustomer.SelectedItem.Value.Trim.Split("$")
            CustID = Cust(0)
            SiteUseID = Cust(1)

            Dim OrgID As New ReportParameter
            OrgID = New ReportParameter("orgID", CStr(ddlOrganization.SelectedValue.ToString()))

            Dim FDate As New ReportParameter

            If txtFromDate.Text.Trim() IsNot String.Empty Then
                FDate = New ReportParameter("Fromdate", txtFromDate.Text.Trim().ToString())
            Else
                FDate = New ReportParameter("Fromdate", Date.Now.ToShortDateString())
            End If


            Dim TDate As New ReportParameter
           
            If txtToDate.Text.Trim() IsNot String.Empty Then
                TDate = New ReportParameter("Todate", txtToDate.Text.Trim().ToString())
            Else
                TDate = New ReportParameter("Todate", Date.Now.ToShortDateString())
            End If

            Dim SalesRepID As New ReportParameter
            SalesRepID = New ReportParameter("FsrID", ddlVan.SelectedItem.Value)

         
            Dim CID As New ReportParameter
            CID = New ReportParameter("CustomerID", CustID)

            Dim SiteID As New ReportParameter
            SiteID = New ReportParameter("SiteID", SiteUseID)


            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                ' .ServerReport.SetParameters(New ReportParameter() {OrgID, FDate, TDate, SalesRepID, CID, SiteID})
                .ServerReport.SetParameters(New ReportParameter() {CID, SiteID, SalesRepID, OrgID, FDate, TDate})
                .ServerReport.Refresh()

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
            ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
            ddlVan.DataBind()
            ddlVan.Items.Insert(0, New ListItem("-- Select a value --", "0"))

            ddlCustomer.DataSource = ObjCommon.GetCustomerByCriteria(Err_No, Err_Desc, ddlOrganization.SelectedValue)
            ddlCustomer.DataBind()
            ddlCustomer.Items.Insert(0, New ListItem("-- Select a value --", "0$0"))

            'Dim dt As New DataTable
            'dt = ObjCommon.GetCurrencyByOrg(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
            'If dt.Rows.Count > 0 Then
            '    hfCurrency.Value = dt.Rows(0)(0).ToString()
            '    hfDecimal.Value = dt.Rows(0)(1).ToString()
            'End If
            RVMain.Reset()
        End If

    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub
End Class