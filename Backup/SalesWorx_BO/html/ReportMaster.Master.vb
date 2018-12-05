Imports SalesWorx.BO.Common
Partial Public Class ReportMaster
    Inherits System.Web.UI.MasterPage
    Dim HasPermission As Boolean = False
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsNothing(Session("USER_ACCESS")) Then
            lbl_User.Text = (New SalesWorx.BO.Common.User).GetUserName(Session("USER_ACCESS").UserID).ToString
            lblUserLogin.Visible = True
        Else
            lbl_User.Text = ""
            lblUserLogin.Visible = False

        End If
    End Sub
    Protected Sub logo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles logo.Click
        If Not IsNothing(Session("USER_ACCESS")) Then
            ManageAuthentication.HasPermission(Session("USER_ACCESS"), "P87", HasPermission)
            If HasPermission = True Then

                Response.Redirect("Dashboard.aspx", False)
            
            Else
                Response.Redirect("Welcome.aspx", False)
            End If
        End If
    End Sub
End Class