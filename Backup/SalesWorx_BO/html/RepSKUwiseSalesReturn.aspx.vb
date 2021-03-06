﻿Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports System.Globalization
Partial Public Class RepSKUwiseSalesReturn
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "SKUwiseReturns"

    Private Const PageID As String = "P253"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single

    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub RepOutletSKUwiseSalesReturn_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
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

                txtFromDate.Text = Format(Now().Date, "dd/MM/yyyy")
                txtToDate.Text = Format(Now().Date, "dd/MM/yyyy")

                Dim OrgID As New ReportParameter
                Dim OrgName As New ReportParameter
                OrgID = New ReportParameter("OrgID", ddlOrganization.SelectedItem.Value)
                OrgName = New ReportParameter("OrgName", ddlOrganization.SelectedItem.Text)

                ''  InitReportViewer()

                If Not Err_Desc Is Nothing Then
                    log.Error(Err_Desc)
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
                End If


            Catch ex As Exception
                Err_No = "74166"
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


            Dim FDate As New ReportParameter

            If txtFromDate.Text.Trim() IsNot String.Empty Then
                FDate = New ReportParameter("FromDate", DateTime.ParseExact(txtFromDate.Text.Trim().ToString(), "dd/MM/yyyy", CultureInfo.CurrentCulture).ToString())
            Else
                FDate = New ReportParameter("FromDate", Date.Now.ToShortDateString())
            End If


            Dim TDate As New ReportParameter
            If txtToDate.Text.Trim() IsNot String.Empty Then
                TDate = New ReportParameter("ToDate", DateTime.ParseExact(txtToDate.Text.Trim().ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString())
            Else
                TDate = New ReportParameter("ToDate", Date.Now.ToShortDateString())
            End If

            Dim SKU As New ReportParameter


            Dim InvID As New ReportParameter
            If ddSKU.SelectedIndex <> 0 Then
                InvID = New ReportParameter("InID", CInt(ddSKU.SelectedItem.Value))
                SKU = New ReportParameter("SKU", ddSKU.SelectedItem.Text.Trim())
            Else
                InvID = New ReportParameter("InID", 0)
                SKU = New ReportParameter("SKU", "All")
            End If

            Dim OrgID As New ReportParameter
            Dim OrgName As New ReportParameter
            OrgID = New ReportParameter("OrgID", ddlOrganization.SelectedItem.Value)
            OrgName = New ReportParameter("OrgName", ddlOrganization.SelectedItem.Text)


            With RVMain
                .Reset()
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                .ServerReport.SetParameters(New ReportParameter() {FDate, TDate, InvID, SKU, OrgID, OrgName})
                '.ServerReport.Refresh()

            End With


        Catch Ex As Exception
            log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            If ddlOrganization.SelectedValue = "-- Select a value --" Then
                MessageBoxValidation("Select organization.")
                Exit Sub
            End If
            InitReportViewer()
        Catch ex As Exception

        End Try
    End Sub
    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        Exit Sub
    End Sub


    Private Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrganization.SelectedIndexChanged
        Try
            ddSKU.DataSource = Nothing
            ddSKU.Items.Clear()
            If ddlOrganization.SelectedIndex <> 0 Then
                ObjCommon = New Common()
                ddSKU.DataSource = ObjCommon.GetSKU(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
                ddSKU.DataBind()
                ddSKU.Items.Insert(0, New ListItem("-- All --"))
            End If

        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            ObjCommon = Nothing
        End Try
    End Sub

End Class