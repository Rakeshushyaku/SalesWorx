Imports SalesWorx.BO.Common
Imports System.IO
Public Class AboutSalesworx
    Inherits System.Web.UI.Page
    Private Const ModuleName As String = "Welcome.aspx"
    Private Const PageID As String = "P78"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("USER_ACCESS") Is Nothing Then
            If Not IsPostBack Then
                Try
                    lblWelcome.Text = AppInfo.GetName()
                    lblVersion.Text = AppInfo.GetVersion()

                     
                Catch ex As Exception
                    
                End Try
            End If
        Else
            Dim RequestPath As String
            RequestPath = Replace(Path.GetDirectoryName(Request.Path), "\", "/").ToLower()
            Response.Redirect("Login.aspx")
        End If
    End Sub


End Class