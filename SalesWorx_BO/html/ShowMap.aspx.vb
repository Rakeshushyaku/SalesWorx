Public Class ShowMap
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Val(Request.QueryString("Lat")) = 0 And Val(Request.QueryString("Long")) = 0 Then
                lblNoMap.Visible = True
                mapwrap.Visible = False
            Else
                lblNoMap.Visible = False
                hfLng.Value = Request.QueryString("Long")
                hflat.Value = Request.QueryString("Lat")

            End If
            If Not Request.QueryString("Type") Is Nothing And Request.QueryString("Type") = "Visits" Then
                tips.Visible = True
                CustLng.Value = Request.QueryString("CustLong")
                CustLat.Value = Request.QueryString("CustLat")
                ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "ShowVisit();", True)
            Else
                tips.Visible = False
                ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
            End If
        End If
    End Sub

End Class