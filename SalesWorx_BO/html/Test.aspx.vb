Public Class test
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ObjReport As New SalesWorx.BO.Common.Reports
        Dim dt As New DataTable
        dt = ObjReport.GetDailyReport(Err_No, Err_Desc, "2100", "08-01-2018", "09-30-2018", "30,31,32,33,34", 1)

        Dim sourceTbl As DataTable = dt.Copy()
        Dim query = From row In sourceTbl.Copy()
                Group row By FSR = row.Field(Of String)("FSR") Into VanGroup = Group
                Select New With {
                    Key FSR,
                    .TotalCallVisited = VanGroup.Sum(Function(r) r.Field(Of Int32)("TotalCallVisited")),
                    .TotalProductiveCall = VanGroup.Sum(Function(r) r.Field(Of Int32)("TotalProductiveCall")),
                    .Sales = VanGroup.Sum(Function(r) r.Field(Of Decimal)("Sales")),
                    .Return = VanGroup.Sum(Function(r) r.Field(Of Decimal)("TotReturns"))
               }

        summaryChart.DataSource = query
        summaryChart.DataBind()
    End Sub

End Class