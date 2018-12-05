Imports SalesWorx.BO.Common
Imports log4net
Imports System.Data.SqlClient
Imports System.DirectoryServices.DirectorySearcher
Imports System.DirectoryServices
Partial Public Class Login
    Inherits System.Web.UI.Page

    Dim objLogin As SalesWorx.BO.Common.Login
    Dim objCrypt As New Crypto
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
        Me.Page.Form.DefaultButton = btnLogin.UniqueID
        Dim ObjControl As New AppControl
        Dim LoginMode As String = "N"
        LoginMode = ObjControl.GeLoginMode(Err_No, Err_Desc)
            'Session.Remove("USER_ACCESS")
        Session("LoginMode") = LoginMode

        If Session("LoginMode") = "N" Then
            logmoderow.Visible = False
        Else
            logmoderow.Visible = True
        End If
        Catch ex As Exception

    End Try
    End Sub

    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLogin.Click
        'objLogin = New SalesWorx.BO.Common.Login
        'objLogin.UserName = txtUserName.Text.Trim()

        'objLogin.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(txtPassword.Text.Trim(), "SHA1")

        'If objLogin.ValidateUser(Err_No, Err_Desc) Then

        '    Session("USER_ACCESS") = objLogin.UserAccess

        '    objLogin.SaveUserLog(Err_No, Err_Desc, "L", "USER MANAGEMENT", "LOGIN", CType(Session("User_Access"), UserAccess).UserID.ToString(), "Login By: " & Me.txtUserName.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
        '    Response.Redirect("Welcome.aspx", False)
        'Else
        '    lblMsg.Text = "Invalid Login."
        'End If
        Session.Remove("USER_ACCESS")

        If IsValidatedInSystem() Then
            log.Debug("Successfully checked in the system")
          
            If ddlLoginMode.SelectedValue = "Windows" Then
                ''Validating against AD
                If IsValidatedInAD() Then
                    objLogin = New SalesWorx.BO.Common.Login
                    objLogin.UserName = txtUserName.Text.Trim()
                    objLogin.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(txtPassword.Text.Trim(), "SHA1")
                    If objLogin.ValidateUser(Err_No, Err_Desc, "ADL") Then

                        Session("USER_ACCESS") = objLogin.UserAccess

                        objLogin.SaveUserLog(Err_No, Err_Desc, "L", "USER MANAGEMENT", "LOGIN", CType(Session("User_Access"), UserAccess).UserID.ToString(), "Login By: " & Me.txtUserName.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                        Response.Redirect("Welcome.aspx", False)
                    Else
                        lblMsg.Text = "Invalid user."
                    End If

                Else

                    lblMsg.Text = "Details for the Username you entered are not available in the Active Directory!"
                End If

            Else
                objLogin = New SalesWorx.BO.Common.Login
                objLogin.UserName = txtUserName.Text.Trim()

                objLogin.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(txtPassword.Text.Trim(), "SHA1")

                If objLogin.ValidateUser(Err_No, Err_Desc, "SWX") Then

                    Session("USER_ACCESS") = objLogin.UserAccess

                    objLogin.SaveUserLog(Err_No, Err_Desc, "L", "USER MANAGEMENT", "LOGIN", CType(Session("User_Access"), UserAccess).UserID.ToString(), "Login By: " & Me.txtUserName.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                    Response.Redirect("Welcome.aspx", False)
                Else
                    lblMsg.Text = "Invalid Login."
                End If
            End If
        Else
            lblMsg.Text = "Invalid user name."
        End If

    End Sub
  
    Private Function IsValidatedInSystem() As Boolean
        log.Debug("Validating against system started.")
        Dim bExists As Boolean = False

        Dim objUser As New User
        Dim bsuccess As Boolean = False

        Try
            objUser.UserName = txtUserName.Text
            objUser.Password = txtPassword.Text

            If objUser.UserExists() Then
                bExists = True
            Else
                bExists = False
            End If

            Return bExists
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Return False
        Finally
            objUser = Nothing
        End Try


    End Function
    Private Function IsValidatedInAD() As Boolean
        Try
            If ValidateActiveDirectoryLogin(ConfigurationManager.AppSettings("LDAPServerPath"), txtUserName.Text, txtPassword.Text) Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Return False
        End Try
    End Function

    Private Function ValidateActiveDirectoryLogin(ByVal LDAP As String, ByVal Username As String, ByVal Password As String) As Boolean
        Dim Success As Boolean = False
        Dim Entry As New System.DirectoryServices.DirectoryEntry(LDAP, Username, Password)
        Dim Searcher As New System.DirectoryServices.DirectorySearcher(Entry)
        Searcher.SearchScope = DirectoryServices.SearchScope.OneLevel
        Try
            Dim Results As System.DirectoryServices.SearchResult = Searcher.FindOne
            Success = Not (Results Is Nothing)
            Return Success
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
          
            lblMsg.Text = "Error while validating active directory"
            Return False
        End Try
    End Function





   
    
End Class