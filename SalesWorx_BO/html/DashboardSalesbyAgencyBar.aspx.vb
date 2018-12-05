Imports SalesWorx.BO.Common
Public Class DashboardSalesbyAgencyBar
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        HUID.Value = objUserAccess.UserID
    End Sub

End Class