Imports SalesWorx.BO.Common
Imports System.IO

Partial Public Class Welcome
    Inherits System.Web.UI.Page
    Private Const ModuleName As String = "Welcome.aspx"
    Private Const PageID As String = "P78"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("USER_ACCESS") Is Nothing Then
            If Not IsPostBack Then
                Try
                    lblWelcome.Text = AppInfo.GetName()
                    lblVersion.Text = AppInfo.GetVersion()

                    Dim HasPermission As Boolean = False
                    ' ManageAuthentication.HasPermission(Session("USER_ACCESS"), PageID, HasPermission)
                    ' If HasPermission Then
                    ManageAuthentication.HasPermission(Session("USER_ACCESS"), "P87", HasPermission)
                    If HasPermission = True Then

                        Response.Redirect("Dashboard.aspx", False)
                    
                    End If
                    ' End If
                    'those who have access to dashboard will redirect to dashboard.as[px

                    'If (Session("USER_ACCESS").UserType) = ConfigurationSettings.AppSettings("Skincare_Promoter") Then
                    '    Response.Redirect("DashboardFSR.aspx", False)
                    'ElseIf (Session("USER_ACCESS").UserType) = ConfigurationSettings.AppSettings("Skincare_Supervisor") Then
                    '    Response.Redirect("DashboardMGR_SVR.aspx", False)
                    'Else
                    '    'Welcome Page
                    'End If
                Catch ex As Exception
                    'Err_No = "75017"
                    'Err_Desc = ex.Message
                    'If Err_Desc <> "Thread was being aborted." Then
                    '    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & ex.Message & "&next=login.aspx", False)
                    'End If
                End Try
            End If
        Else
            Dim RequestPath As String
            RequestPath = Replace(Path.GetDirectoryName(Request.Path), "\", "/").ToLower()
            Response.Redirect("Login.aspx")
        End If
    End Sub


End Class