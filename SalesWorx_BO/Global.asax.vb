Imports System.Web.SessionState
Imports System.IO
Imports System.IO.Compression
Imports System.Web.Optimization
Imports System.Web.Routing
Imports SalesWorx.BO.Common
Imports System.Web.HttpContext

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)

        ' Fires when the application is started
        BundleConfig.RegisterBundles(BundleTable.Bundles)
        AuthConfig.RegisterOpenAuth()
        RouteConfig.RegisterRoutes(RouteTable.Routes)

        log4net.Config.XmlConfigurator.Configure()
        ConfigureLogging()
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ConfigureLogging()
        ' Fires when the session is started
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
        ''  HttpCompress(DirectCast(sender, HttpApplication))
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub
    Protected Sub ConfigureLogging()
        Dim logFile As String = Path.Combine(HttpRuntime.AppDomainAppPath, ConfigurationManager.AppSettings("Log4NetConfigFile"))
        If System.IO.File.Exists(logFile) Then
            log4net.Config.XmlConfigurator.ConfigureAndWatch(New FileInfo(logFile))
        End If
    End Sub

    'Private Sub HttpCompress(ByVal app As HttpApplication)
    '    Try
    '        Dim accept As String = app.Request.Headers("Accept-Encoding")
    '        If accept IsNot Nothing AndAlso accept.Length > 0 Then
    '            If CompressScript(Request.ServerVariables("SCRIPT_NAME")) Then
    '                Dim stream As Stream = app.Response.Filter
    '                accept = accept.ToLower()
    '                If accept.Contains("gzip") Then
    '                    app.Response.Filter = New GZipStream(stream, CompressionMode.Compress)
    '                    app.Response.AppendHeader("Content-Encoding", "gzip")
    '                ElseIf accept.Contains("deflate") Then
    '                    app.Response.Filter = New DeflateStream(stream, CompressionMode.Compress)
    '                    app.Response.AppendHeader("Content-Encoding", "deflate")
    '                End If
    '            End If
    '        End If
    '    Catch ex As Exception
    '        'handle the exception
    '    End Try
    'End Sub

    Private Function CompressScript(ByVal scriptName As String) As Boolean
        If scriptName.ToLower().Contains(".aspx") Then
            Return True
        Else
            Return False
        End If
    End Function


End Class