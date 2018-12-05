Public Partial Class ViewImg
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ReqID As String = Request.QueryString("id")
        Dim type As String = Request.QueryString("type").ToString()
        imgSig.ImageUrl = "ViewImage.aspx?type=order&id=" & ReqID
    End Sub

End Class