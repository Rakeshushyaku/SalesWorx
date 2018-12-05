Public Partial Class WebForm1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

       
    End Sub
    Public Function GetWeekNumber(Tdate As Date) As String
        Dim sRetval As String = "0"
        Dim dt As New DataTable
        dt.Columns.Add("FromDate", System.Type.GetType("System.DateTime"))
        dt.Columns.Add("ToDate", System.Type.GetType("System.DateTime"))
        dt.Columns.Add("WeekNO", System.Type.GetType("System.Int32"))
        Dim startofmonth As Date
        startofmonth = Tdate.Month & "/01/" & Tdate.Year
        Dim endofmonth As Date
        endofmonth = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, startofmonth))
        Dim dayoffirst As Integer
        dayoffirst = startofmonth.DayOfWeek

        Dim stdate As Date
        Dim enddate As Date

        Select Case dayoffirst
            Case 0
                stdate = startofmonth
                enddate = DateAdd(DateInterval.Day, 6, startofmonth)
            Case 1
                stdate = startofmonth
                enddate = DateAdd(DateInterval.Day, 5, startofmonth)
            Case 2
                stdate = startofmonth
                enddate = DateAdd(DateInterval.Day, 4, startofmonth)
            Case 3
                stdate = startofmonth
                enddate = DateAdd(DateInterval.Day, 3, startofmonth)
            Case 4
                stdate = startofmonth
                enddate = DateAdd(DateInterval.Day, 2, startofmonth)
            Case 5
                stdate = startofmonth
                enddate = DateAdd(DateInterval.Day, 1, startofmonth)
            Case 6
                stdate = startofmonth
                enddate = DateAdd(DateInterval.Day, 0, startofmonth)
        End Select

        For i = 1 To 6
            If stdate < endofmonth Then
                Dim dr As DataRow
                dr = dt.NewRow
                dr("FromDate") = stdate
                dr("ToDate") = enddate
                dr("WeekNO") = i
                dt.Rows.Add(dr)
                stdate = DateAdd(DateInterval.Day, 1, enddate)
                enddate = DateAdd(DateInterval.Day, 6, stdate)
            End If
        Next

        Dim seldr() As DataRow
        seldr = dt.Select("FromDate<='" & Tdate & "' and ToDate>='" & Tdate & "'")
        If seldr.Length > 0 Then
            sRetval = seldr(0)("WeekNO")
        End If
        Return sRetval
        dt = Nothing
    End Function

    Private Sub test_Click(sender As Object, e As EventArgs) Handles test.Click
        txt.Text = GetWeekNumber(txt.Text)
    End Sub
End Class