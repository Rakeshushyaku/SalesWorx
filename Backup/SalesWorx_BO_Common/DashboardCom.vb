Imports System.Data.SqlClient
Imports SalesWorx.BO.DAL
Public Class DashboardCom
    Dim ObjDALDashboard As New DAL_Dashboard
    Function GetVANRequisitions(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal UD_SUB_QRY As String) As DataSet
        Try
            Return ObjDALDashboard.GetVANRequisitions(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetDistributionCheck(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal userid As Integer, ByVal startdate As Date, ByVal enddate As Date) As DataSet
        Return ObjDALDashboard.GetDistributionCheck(Err_No, Err_Desc, userid, startdate, enddate)
    End Function
    Function GetVanLog(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserID As Integer, ByVal startdate As Date, ByVal enddate As Date) As DataSet
        Try
            Return ObjDALDashboard.GetVanLog(Err_No, Err_Desc, UserID, startdate, enddate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSalesbyAgency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserID As Integer, ByVal startdate As String, ByVal enddate As String) As DataSet
        Try
            Return ObjDALDashboard.GetSalesbyAgency(Err_No, Err_Desc, UserID, startdate, enddate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSalesbyVan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserID As Integer, ByVal startdate As String, ByVal enddate As String) As DataSet
        Try
            Return ObjDALDashboard.GetSalesbyVan(Err_No, Err_Desc, UserID, startdate, enddate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetRouteCoverage(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal FSRID As String, ByVal startdate As String, ByVal enddate As String, ByVal OrgID As String) As DataSet
        Return ObjDALDashboard.GetRouteCoverage(Err_No, Err_Desc, FSRID, startdate, enddate, OrgID)
    End Function
End Class
