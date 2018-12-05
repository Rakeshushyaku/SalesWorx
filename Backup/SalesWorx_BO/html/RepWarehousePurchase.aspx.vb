Imports System.Configuration.ConfigurationManager


Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports System.Globalization
Partial Public Class RepWarehousePurchase
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Dim ObjPrice As Price
    Private ReportPath As String = "WareHousePurchase_Hdr"
    Private Const PageID As String = "P224"
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
            txtFromDate.Text = Now.ToString("dd-MMM-yyyy")
            txtTodate.Text = Now.ToString("dd-MMM-yyyy")
            
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
                ddlOrganization.Items.Insert(0, New ListItem("-- Select a value --", "0"))


                LoadAgency()
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
    Private Sub InitReportViewer()
        Try

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

            With RVMain

                .Reset()
                .Visible = True
                .ShowParameterPrompts = False
                .ShowBackButton = True

                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)

                Dim ReqDate As New ReportParameter

                If txtFromDate.Text.Trim() IsNot String.Empty Then
                    ReqDate = New ReportParameter("Date", txtFromDate.Text)
                Else
                    ReqDate = New ReportParameter("Date", Date.Now.ToString("dd-MMM-yyyy"))
                End If

                Dim ReqToDate As New ReportParameter
                If txtTodate.Text.Trim() IsNot String.Empty Then
                    ReqToDate = New ReportParameter("ToDate", txtTodate.Text)
                Else
                    ReqToDate = New ReportParameter("ToDate", Date.Now.ToString("dd-MMM-yyyy"))
                End If

                Dim Org_ID As New ReportParameter
                Org_ID = New ReportParameter("Org_ID", ddlOrganization.SelectedItem.Value)
                

                Dim Agency As New ReportParameter
                Agency = New ReportParameter("Agency", ddlAgency.SelectedItem.Value)


                .ServerReport.SetParameters(New ReportParameter() {ReqDate, Org_ID, Agency, ReqToDate})
                '.ServerReport.Refresh()

            End With


        Catch Ex As Exception
            '  log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub
   
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        RVMain.Visible = False
        LoadAgency()
    End Sub
    Sub LoadAgency()
        ddlAgency.DataSource = (New SalesWorx.BO.Common.Stock).GetAgencyList(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, txtFromDate.Text)
        ddlAgency.DataTextField = "Agency"
        ddlAgency.DataValueField = "Agency"
        ddlAgency.DataBind()
        ddlAgency.Items.Insert(0, "--Select Agency--")
        ddlAgency.Items(0).Value = "0"
    End Sub
    Protected Sub SearchBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SearchBtn.Click
        If ddlOrganization.SelectedItem.Value = "0" Then
            MessageBoxValidation("Select an organization.")
            Exit Sub
        ElseIf txtFromDate.Text = "" Or IsDate(txtFromDate.Text) = False Then
            MessageBoxValidation("Please enter a from valid date.")
            Exit Sub
        ElseIf txtTodate.Text = "" Or IsDate(txtTodate.Text) = False Then
            MessageBoxValidation("Please enter a to valid date.")
            Exit Sub
        Else
            InitReportViewer()
        End If
    End Sub
    Sub MessageBoxValidation(ByVal str As String)
        Me.lblMessage.Text = str
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        MpInfoError.Show()
        btnClose.Focus()
        Exit Sub
    End Sub
End Class