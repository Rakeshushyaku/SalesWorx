Imports System
Imports System.Data
Imports System.Configuration
Imports System.Net
Imports System.Security.Principal
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports Microsoft.Reporting.WebForms

<Serializable()> _
    Public NotInheritable Class CustomReportServerCredentials
    Implements IReportServerCredentials

    Public ReadOnly Property ImpersonationUser() As WindowsIdentity Implements Microsoft.Reporting.WebForms.IReportServerCredentials.ImpersonationUser
        Get
            ' Use the default Windows user.  Credentials will be
            ' provided by the NetworkCredentials property.
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property NetworkCredentials() As ICredentials Implements Microsoft.Reporting.WebForms.IReportServerCredentials.NetworkCredentials
        Get
            ' Read the user information from the Web.config file.  
            ' By reading the information on demand instead of 
            ' storing it, the credentials will not be stored in 
            ' session, reducing the vulnerable surface area to the
            ' Web.config file, which can be secured with an ACL.

            ' User name
            Dim userName As String = ConfigurationManager.AppSettings("RSImpersonateUsername")

            If String.IsNullOrEmpty(userName) Then
                Throw New Exception("Missing user name from web.config file")
            End If

            ' Password
            Dim password As String = ConfigurationManager.AppSettings("RSImpersonatePassword")

            If String.IsNullOrEmpty(password) Then
                Throw New Exception("Missing password from web.config file")
            End If

            ' Domain
            Dim domain As String = ConfigurationManager.AppSettings("RSImpersonateDomain")

            If String.IsNullOrEmpty(domain) Then
                Throw New Exception("Missing domain from web.config file")
            End If

            Return New NetworkCredential(userName, password, domain)
        End Get
    End Property

    Public Function GetFormsCredentials(ByRef authCookie As Cookie, ByRef userName As String, ByRef password As String, ByRef authority As String) As Boolean Implements Microsoft.Reporting.WebForms.IReportServerCredentials.GetFormsCredentials
        authCookie = Nothing
        userName = Nothing
        password = Nothing
        authority = Nothing
        Return False
    End Function

End Class
