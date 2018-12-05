Imports SalesWorx.BO.Common
Partial Public Class DashBoardSalesbyVan
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim Start_Date As New ReportParameter
        'Start_Date = New ReportParameter("Fromdate", DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now).ToString("dd-MMM-yyyy"))

        'Dim End_Date As New ReportParameter
        'End_Date = New ReportParameter("Todate", Now.ToString("dd-MMM-yyyy"))


        'With RVMainCoverage
        '    .ShowToolBar = False
        '    .Reset()
        '    .ShowParameterPrompts = False
        '    .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        '    .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        '    .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), "DashBoardSalesbyVan")
        '    .ServerReport.SetParameters(New ReportParameter() {Start_Date, End_Date})
        '    .ServerReport.Refresh()

        'End With
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        UId.Value = objUserAccess.UserID

    End Sub

End Class