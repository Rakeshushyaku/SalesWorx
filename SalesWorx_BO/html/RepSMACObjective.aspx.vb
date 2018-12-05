﻿Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports System.Globalization
Imports Telerik.Web.UI
Partial Public Class RepSMACObjective
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "SMACObjective"

    Private Const PageID As String = "P403"
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
                ddlOrganization.DataTextField = "Description"
                ddlOrganization.DataValueField = "MAS_Org_ID"
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))

                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                End If

                txtFromDate.SelectedDate = Now
                BindCombo(IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue))
                If Not Err_Desc Is Nothing Then
                    log.Error(Err_Desc)
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
                End If
            Catch ex As Exception
                Err_No = "741266"
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

    Private Sub BindCombo(OrgStr)
        Dim ObjRep As New Reports

        Dim x As New DataTable
        x = ObjRep.GetAllOrgVan(Err_No, Err_Desc, IIf(OrgStr Is Nothing, "0", OrgStr), CType(Session("User_Access"), UserAccess).UserID)

        Dim r As DataRow = x.NewRow
        r(0) = "0"
        r(1) = "Select a van"
        x.Rows.InsertAt(r, 0)

        ddlVan.DataSource = x
        ddlVan.DataTextField = "SalesRep_Name"
        ddlVan.DataValueField = "SalesRep_ID"
        ddlVan.DataBind()

        Me.ddlVan.SelectedIndex = 0


    End Sub
    Private Sub InitReportViewer()
        Try

            lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")

            lbl_org.Text = ddlOrganization.SelectedItem.Text



            Dim VanStr As String = ""
            Dim vancnt As Integer = 0
            Dim vantxt As String = ""


            
                    vancnt = vancnt + 1
            VanStr = VanStr & ddlVan.SelectedValue
            vantxt = vantxt & ddlVan.SelectedItem.Text
            
            
                lbl_van.Text = vantxt









            hfSMonth.Value = txtFromDate.SelectedDate
            hfVans.Value = VanStr
            hfOrgID.Value = Me.ddlOrganization.SelectedValue



            Dim FDate As New ReportParameter
            FDate = New ReportParameter("VisitDate", CDate(txtFromDate.SelectedDate.Value).ToString("yyyy-MM-dd"))
            Dim OID As New ReportParameter

            OID = New ReportParameter("OrgID", ddlOrganization.SelectedValue)


       
            Dim OrgID As New ReportParameter

            OrgID = New ReportParameter("OrgName", ddlOrganization.SelectedItem.Text)

            Dim UID As New ReportParameter
            UID = New ReportParameter("Uid", CStr(CType(Session.Item("USER_ACCESS"), UserAccess).UserID))
            Dim vans As New ReportParameter
            vans = New ReportParameter("SalesRepID", VanStr)
            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                .ServerReport.SetParameters(New ReportParameter() {UID, OrgID, vans, FDate, OID})
                .ServerReport.Refresh()

            End With
        Catch Ex As Exception
            '  log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False


        If Me.ddlOrganization.SelectedIndex <= 0 Then
            MessageBoxValidation("Please select a organization.", "Validation")
            Return bretval
        End If

      

        If Me.ddlVan.SelectedIndex <= 0 Then
            MessageBoxValidation("Please select a van.", "Validation")
            Return bretval
        End If

        bretval = True

        Return bretval

    End Function

    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            rpbFilter.Items(0).Expanded = False
            InitReportViewer()
            Args.Visible = True
            RVMain.Visible = True
            reportblocker.Visible = True
        Else
            Args.Visible = False
            RVMain.Visible = False
            reportblocker.Visible = False
        End If
    End Sub
    Sub MessageBoxValidation(ByVal str As String, Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged

        BindCombo(IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue))
        RVMain.Reset()
        Args.Visible = False
        RVMain.Visible = False
        reportblocker.Visible = False
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        BindCombo(IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue))
        Me.txtFromDate.SelectedDate = Now.Date
        Args.Visible = False
        RVMain.Visible = False
        reportblocker.Visible = False
        RVMain.Reset()
    End Sub
End Class
