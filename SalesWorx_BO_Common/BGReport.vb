Imports System.Data.SqlClient
Imports SalesWorx.BO.DAL
Public Class BGReport
 Dim ObjDALBGReport As New DAL_BGReport
Function GetBGReports(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal FromDate As String, ByVal Todate As String, ByVal Status As String) As DataTable
        Return ObjDALBGReport.GetBGReports(Err_No, Err_Desc, FromDate, Todate, Status)
    End Function
    Public Function SaveBGReport(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Opt As String, ByVal RID As String, ByVal UserID As String, ByVal Dt As DataTable) As Boolean
         Try
            Return ObjDALBGReport.SaveBGReport(Err_No, Err_Desc, Opt, RID, UserID, Dt)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetBGReportDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RID As String) As DataTable
         Try
            Return ObjDALBGReport.GetBGReportDetails(Err_No, Err_Desc, RID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
