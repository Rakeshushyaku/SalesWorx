Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports Microsoft.Reporting.WebForms
Imports System.Configuration.ConfigurationManager
Partial Public Class Rep_Orders
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Private Const PageID As String = "P91"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private ReportPath As String = "Orders"
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
    Public Sub SetReportDetails(ByVal path As String)
        Me.ReportPath = path
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        If Not IsPostBack Then
            Try
                Dim HasPermission As Boolean = False
                ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
                If Not HasPermission Then
                    Err_No = 500
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
                End If
                InitReportViewer()
            Catch ex As Exception
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
            Finally
                ObjCommon = Nothing
                ErrorResource = Nothing
            End Try
        End If
    End Sub
    Private Sub InitReportViewer()
        Try
            ' If Not Me.IsPostBack Then
            Dim myParamDesg As New ReportParameter
            Dim myParamVisitID As New ReportParameter

            If Not IsNothing(Session("USER_ACCESS")) Then
                Dim Desg As String = CType(Session("USER_ACCESS"), UserAccess).Designation
                myParamDesg = New ReportParameter("Designation", Desg)
            Else
                myParamDesg = New ReportParameter("Designation", "")
            End If
            If Not IsNothing(Request.QueryString("visitid")) Then
                Dim VisitID As String = Request.QueryString("visitid").ToString()
                myParamVisitID = New ReportParameter("VisitID", VisitID)
            Else
                myParamVisitID = New ReportParameter("VisitID", "")
            End If

            With RVMain
                .Reset()

                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                .ServerReport.SetParameters(New ReportParameter() {myParamDesg, myParamVisitID})
                .ServerReport.Refresh()
            End With
        Catch Ex As Exception
            log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub
End Class