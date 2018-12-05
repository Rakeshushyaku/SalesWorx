Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Partial Public Class Rep_FOCFeedBack
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Dim ObjCustomer As Customer
    Private ReportPath As String = "FOCFeedback"

    Private Const PageID As String = "P306"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub RepVanLoad_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
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
                ddlOrganization.Items.Insert(0, New ListItem("-- Select a value --", "-1"))


            Dim objUserAccess As UserAccess
                            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)



            ddl_customer.DataSource = ObjCommon.GetCustomerShipfromOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue)

            ddl_customer.Items.Clear()
            ddl_customer.DataValueField = "CustomerID"
            ddl_customer.DataTextField = "Customer"
            ddl_customer.DataBind()
            ddl_customer.Items.Insert(0, New ListItem("-- Select --", "0$0"))

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
        Try

         If ddlOrganization.SelectedValue = "-1" Then
                MessageBoxValidation("Please select an organization")
                Exit Sub
         End If
         
         If txtFromDate.Text = "" Or txtToDate.Text = "" Then
                MessageBoxValidation("Select a date range.")
                Exit Sub
            End If
        If txtFromDate.Text.Trim() = "" Then

                MessageBoxValidation("Enter a valid from date.")
                Exit Sub
        Else
        If Not IsDate(txtFromDate.Text) Then

                MessageBoxValidation("Enter a valid from date.")
                Exit Sub
            End If
        End If

        If txtToDate.Text.Trim() = "" Then

            MessageBoxValidation("Enter a valid to date.")
            Exit Sub
        Else
                If Not IsDate(txtToDate.Text) Then

                    MessageBoxValidation("Enter a valid to date.")
                    Exit Sub
                End If
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


             Dim Selcustomer As String
                Selcustomer = ddl_customer.SelectedItem.Value
                Dim ids() As String
                ids = Selcustomer.Split("$")

            Dim ORGID As New ReportParameter
            ORGID = New ReportParameter("OrgId", ddlOrganization.SelectedItem.Value)

            Dim Customer_ID As New ReportParameter
            Customer_ID = New ReportParameter("CustID", ids(0))

            Dim Site_ID As New ReportParameter
            Site_ID = New ReportParameter("SiteID", ids(1))

            Dim FDate As New ReportParameter
            FDate = New ReportParameter("FromDate", txtFromDate.Text)

            Dim TDate As New ReportParameter
            TDate = New ReportParameter("Todate", txtToDate.Text)
            
            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                .ServerReport.SetParameters(New ReportParameter() {ORGID, Customer_ID, Site_ID, FDate, TDate})
                .ServerReport.Refresh()

            End With


        Catch Ex As Exception
            '  log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub
    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        MpInfoError.Show()
        Exit Sub
    End Sub

    Private Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrganization.SelectedIndexChanged
 ObjCommon = New Common()
           If Not (ddlOrganization.SelectedItem.Value = "-1") Then


            ddl_customer.DataSource = ObjCommon.GetCustomerfromOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue)

            ddl_customer.Items.Clear()
            ddl_customer.DataValueField = "CustomerID"
            ddl_customer.DataTextField = "Customer"
            ddl_customer.DataBind()
             ddl_customer.Items.Insert(0, New ListItem("-- Select --", "0$0"))

            RVMain.Reset()

        End If

    End Sub

    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        BindData()
    End Sub


End Class