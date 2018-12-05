Public Partial Class DashInformation
    Inherits System.Web.UI.Page

    Private Sub DashInformation_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = Request.QueryString("Title")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

End Class