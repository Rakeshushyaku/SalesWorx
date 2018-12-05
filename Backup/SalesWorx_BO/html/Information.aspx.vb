Public Partial Class Information
    Inherits System.Web.UI.Page

    Private Sub Information_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = Request.QueryString("Title")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If CStr(Session.Item("LoggedInUserName")) = "" Then
        '    Response.Redirect("Login.aspx", False)
        '    Exit Sub
        'End If
        'If Request.QueryString("Title") <> "" Then
        '    '  lblTitle.Text = Request.QueryString("Title")
        'Else
        '    Response.Redirect("Welcome.aspx", False)

        'End If
    End Sub

End Class