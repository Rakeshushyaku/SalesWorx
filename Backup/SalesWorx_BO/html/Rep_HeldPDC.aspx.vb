Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports System.Globalization
Partial Public Class Rep_HeldPDC
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager

    Dim ObjCustomer As Customer

    Private ReportPath As String = "HeldPDCs"

    Private Const PageID As String = "P254"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single
    Dim StartDate As Date
    Dim ToyearSelected As String
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private Sub RepSales_WeeklyMonthly_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
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
            RVMain.Reset()
            Dim ObjCommon As Common
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
                ddlOrganization.DataTextField = "Description"
                ddlOrganization.DataValueField = "MAS_Org_ID"
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New ListItem("-- Select a value --", "0"))

                txtfromDate.Text = Now.ToString("dd-MMM-yyyy")
                txtToDate.Text = Now.ToString("dd-MMM-yyyy")
                If Not Err_Desc Is Nothing Then
                    log.Error(Err_Desc)
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
                End If

                ObjCommon = Nothing
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
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click

        If ddlOrganization.SelectedItem.Value = "0" Then
            MessageBoxValidation("Please select the organization")
            Exit Sub
        End If

        If IsDate(txtfromDate.Text) = False Then
            MessageBoxValidation("Please enter a valid from date")
            Exit Sub
        End If
        If IsDate(txtToDate.Text) = False Then
            MessageBoxValidation("Please enter a valid to date")
            Exit Sub
        End If

        If Not (CDate(txtfromDate.Text) <= CDate(txtToDate.Text)) Then
            MessageBoxValidation("Invalid Date range selection")
            Exit Sub
        End If


        Dim pFromDate As New ReportParameter
        pFromDate = New ReportParameter("FromDate", txtfromDate.Text)

        Dim pToDate As New ReportParameter
        pToDate = New ReportParameter("ToDate", txtToDate.Text)

        Dim OrgId As New ReportParameter
        OrgId = New ReportParameter("OID", CStr(ddlOrganization.SelectedItem.Value.ToString()))

        Dim CollRefNo As New ReportParameter
        CollRefNo = New ReportParameter("CollRefNo", txt_CollectionRef.Text)

        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        Dim myParamUserId As New ReportParameter
        myParamUserId = New ReportParameter("Uid", objUserAccess.UserID)

        With RVMain
            .Reset()
            .Visible = True
            .ShowParameterPrompts = False
            .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))

            .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
            .ServerReport.SetParameters(New ReportParameter() {CollRefNo, OrgId, myParamUserId, pFromDate, pToDate})
            .ServerReport.Refresh()

        End With
    End Sub
    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        Exit Sub
    End Sub

   
End Class