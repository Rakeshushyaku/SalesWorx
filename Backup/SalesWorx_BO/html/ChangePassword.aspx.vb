Imports log4net
Imports SalesWorx.BO.Common

Partial Public Class ChangePassword
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim objUser As New User
    Dim objCrypt As New Crypto
    Private Const ModuleName As String = "AreaVanMapping.aspx"
    Private Const PageID As String = "P15"
    ' Private Const CryptKey As String = "Unique"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objLogin As New SalesWorx.BO.Common.Login
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                If Session.Item("USER_ACCESS") Is Nothing Then
                    Session.Add("BringmeBackHere", ModuleName)
                    Response.Redirect("Login.aspx", False)
                    Exit Sub
                End If
                If Not IsPostBack Then
                    Dim HasPermission As Boolean = False
                    ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
                    If Not HasPermission Then
                        Err_No = 500
                        Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
                    End If

                End If
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        Try
            If Session.Item("USER_ACCESS") Is Nothing Then
                Session.Add("BringmeBackHere", ModuleName)
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            Dim Valid As Boolean = True
            Dim strMsg As String = "<div style='padding-left:13px' align='left'>Please fill the below mandatory field(s). "
            If txtOldPwd.Text.Trim() = "" Then
                strMsg += "<li>Old Password</li>"
                Valid = False
            End If

            If txtNewPwd.Text.Trim() = "" Then
                strMsg += "<li>New Password</li>"
                Valid = False
            End If

            If txtConfirmPwd.Text.Trim() = "" Then
                strMsg += "<li>Confirm Password</li>"
                Valid = False
            End If
            strMsg += "</div>"
            If Valid = False Then
                MessageBoxValidation(strMsg)
                Return
            End If


            If txtConfirmPwd.Text.Trim() <> txtNewPwd.Text.Trim() Then
                MessageBoxValidation("New password and confirm password do not match.")
                Return
            End If

            'objUser.OldPassword = objCrypt.encrypt(txtOldPwd.Text.Trim(), CryptKey)
            'objUser.NewPassword = objCrypt.encrypt(txtNewPwd.Text.Trim(), CryptKey)
            objUser.OldPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(txtOldPwd.Text.Trim(), "SHA1")
            objUser.NewPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(txtNewPwd.Text.Trim(), "SHA1")
            objUser.UserID = CType(Session.Item("USER_ACCESS"), UserAccess).UserID
            If objUser.ChangePassword(Err_No, Err_Desc) Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "L", "USER MANAGEMENT", "PASSWORD CHANGED", CType(Session("User_Access"), UserAccess).UserID.ToString(), "Password Changed", CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                MessageBoxInfo("Your password has been changed.")
            Else
                If Err_No = 74212 Then
                    MessageBoxValidation(Err_Desc)
                End If
                If Err_No = 74213 Then
                    MessageBoxValidation("Error occured while changing the password.")
                End If
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            MessageBoxValidation("Error occured while changing the password.")
        End Try
    End Sub
    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        Exit Sub
    End Sub
    Sub MessageBoxInfo(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Green
        lblinfo.Text = "Information"
        lblMessage.Text = str
        MpInfoError.Show()
        Exit Sub
    End Sub
End Class