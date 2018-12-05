Imports Microsoft.Reporting.WebForms
Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Partial Public Class DashBoardRouteCoverage
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Dim ObjCommon As Common
        'ObjCommon = New Common()
        'Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)

        'Dim dt As DataTable

        'dt = ObjCommon.GetAllVan(Err_No, Err_Desc, SubQry)
        'Dim fsr As String = ""
        'For Each dr As DataRow In dt.Rows
        '    fsr = fsr & dr("Salesrep_ID").ToString & ","
        'Next
        'If fsr.Trim <> "" Then
        '    fsr = fsr.Substring(0, fsr.Length - 1)
        'End If

        'Dim FSRID As New ReportParameter
        'FSRID = New ReportParameter("FSRID", fsr)

        'Dim Start_Date As New ReportParameter
        'Start_Date = New ReportParameter("Start_Date", DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now).ToString("dd-MMM-yyyy"))

        'Dim End_Date As New ReportParameter
        'End_Date = New ReportParameter("End_Date", Now.ToString("dd-MMM-yyyy"))


        'With RVMainCoverage
        '    .ShowToolBar = False
        '    .Reset()
        '    .ShowParameterPrompts = False
        '    .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        '    .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        '    .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), "DashBoradRouteCoverage")
        '    .ServerReport.SetParameters(New ReportParameter() {FSRID, Start_Date, End_Date})
        '    .ServerReport.Refresh()

        'End With
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

        Dim ObjCommon As Common
        ObjCommon = New Common()
        Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)


        Dim fsr As String = ""
        For Each item In objUserAccess.AssignedSalesReps
            fsr = fsr & item & ","
        Next
        If fsr.Trim <> "" Then
            fsr = fsr.Substring(0, fsr.Length - 1)
        End If

        Dim Org_ID As String = ""
        For Each item In objUserAccess.OrgID
            Org_ID = Org_ID & item & ","
        Next
        If Org_ID.Trim <> "" Then
            Org_ID = Org_ID.Substring(0, Org_ID.Length - 1)
        End If

        Dim ds As DataSet
        ds = (New SalesWorx.BO.Common.DashboardCom).GetRouteCoverage(Err_No, Err_Desc, fsr, DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now).ToString("dd-MMM-yyyy"), Now.ToString("dd-MMM-yyyy"), Org_ID)
        If ds.Tables(0).Rows.Count > 0 Then
            Maxval.Value = 100
            PointVal.Value = (Val(ds.Tables(0).Compute("Sum(Visits)", "").ToString) / Val(ds.Tables(0).Compute("Sum(Planned)", "").ToString)) * 100
        End If
    End Sub

End Class