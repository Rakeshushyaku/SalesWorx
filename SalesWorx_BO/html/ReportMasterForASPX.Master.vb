Public Partial Class ReportMasterForASPX
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsNothing(Session("USER_ACCESS")) Then
            lbl_User.Text = (New SalesWorx.BO.Common.User).GetUserName(Session("USER_ACCESS").UserID).ToString
            lblUserLogin.Visible = True
        Else
            lbl_User.Text = ""
            lblUserLogin.Visible = False

        End If
    End Sub

End Class