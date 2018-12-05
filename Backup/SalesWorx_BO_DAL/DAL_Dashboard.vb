Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Data
Public Class DAL_Dashboard
    Private _objDB As DatabaseConnection
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Public Function GetVANRequisitions(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select A.StockRequisition_ID,A.Request_Date,A.SalesRep_ID,A.Dest_Org_ID,A.Comments,A.Status,A.Emp_Code,A.Approved_By,B.SalesRep_Name From TBL_Stock_Requisition As A LEFT JOIN TBL_FSR As B ON B.SalesRep_ID=A.SalesRep_ID Where A.Status ='N' {0} order by A.Request_Date Asc", _sSearchParams, QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VANReqTbl")

            GetVANRequisitions = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74061"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function

    Public Function GetDistributionCheck(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserID As Integer, ByVal StartDate As Date, ByVal EndDate As Date) As DataSet
        Try
            Dim objSQLConn As SqlConnection
            Dim objSQLDa As New SqlDataAdapter
            Dim objSQLCmd As New SqlCommand
            Dim dt As New DataTable
            Try
                objSQLConn = _objDB.GetSQLConnection
                objSQLCmd = New SqlCommand("[app_Rep_DistributionCheck]", objSQLConn)
                objSQLCmd.CommandTimeout = 300
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.Parameters.AddWithValue("@UserID", UserID)
                objSQLCmd.Parameters.AddWithValue("@StartDate", StartDate)
                objSQLCmd.Parameters.AddWithValue("@EndDate", EndDate)
                Dim MsgDs As New DataSet
                Dim SqlAd As SqlDataAdapter
                SqlAd = New SqlDataAdapter(objSQLCmd)
                SqlAd.Fill(MsgDs, "DisChkTbl")
                GetDistributionCheck = MsgDs
                objSQLCmd.Dispose()
            Catch ex As Exception
                Err_No = "74961"
                Err_Desc = ex.Message
                Throw ex
            Finally
                objSQLCmd = Nothing
                _objDB.CloseSQLConnection(objSQLConn)
            End Try
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetVanLog(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserID As String, ByVal StartDate As Date, ByVal EndDate As Date) As DataSet
        Try
            Dim objSQLConn As SqlConnection
            Dim objSQLDa As New SqlDataAdapter
            Dim objSQLCmd As New SqlCommand
            Dim dt As New DataTable
            Try
                objSQLConn = _objDB.GetSQLConnection
                objSQLCmd = New SqlCommand("[app_GetVANLog]", objSQLConn)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.Parameters.AddWithValue("@UserID", UserID)
                objSQLCmd.Parameters.AddWithValue("@StartDate", StartDate)
                objSQLCmd.Parameters.AddWithValue("@EndDate", EndDate)
                Dim MsgDs As New DataSet
                Dim SqlAd As SqlDataAdapter
                SqlAd = New SqlDataAdapter(objSQLCmd)
                SqlAd.Fill(MsgDs, "VanLogTbl")
                GetVanLog = MsgDs
                objSQLCmd.Dispose()
            Catch ex As Exception
                Err_No = "74061"
                Err_Desc = ex.Message
                Throw ex
            Finally
                objSQLCmd = Nothing
                _objDB.CloseSQLConnection(objSQLConn)
            End Try
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetSalesbyAgency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserID As String, ByVal StartDate As String, ByVal EndDate As String) As DataSet
        Try
            Dim objSQLConn As SqlConnection
            Dim objSQLDa As New SqlDataAdapter
            Dim objSQLCmd As New SqlCommand
            Dim dt As New DataTable
            Try
                objSQLConn = _objDB.GetSQLConnection
                objSQLCmd = New SqlCommand("[Rep_SalesByAgency_DashBoard]", objSQLConn)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.Parameters.AddWithValue("@Uid", UserID)
                objSQLCmd.Parameters.AddWithValue("@FromDate", StartDate)
                objSQLCmd.Parameters.AddWithValue("@ToDate", EndDate)
                Dim MsgDs As New DataSet
                Dim SqlAd As SqlDataAdapter
                SqlAd = New SqlDataAdapter(objSQLCmd)
                SqlAd.Fill(MsgDs, "VanLogTbl")
                GetSalesbyAgency = MsgDs
                objSQLCmd.Dispose()
            Catch ex As Exception
                Err_No = "74061"
                Err_Desc = ex.Message
                Throw ex
            Finally
                objSQLCmd = Nothing
                _objDB.CloseSQLConnection(objSQLConn)
            End Try
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetSalesbyVan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserID As String, ByVal StartDate As String, ByVal EndDate As String) As DataSet
        Try
            Dim objSQLConn As SqlConnection
            Dim objSQLDa As New SqlDataAdapter
            Dim objSQLCmd As New SqlCommand
            Dim dt As New DataTable
            Try
                objSQLConn = _objDB.GetSQLConnection
                objSQLCmd = New SqlCommand("[Rep_SalesByVan_DashBoard]", objSQLConn)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.Parameters.AddWithValue("@Uid", UserID)
                objSQLCmd.Parameters.AddWithValue("@FromDate", StartDate)
                objSQLCmd.Parameters.AddWithValue("@ToDate", EndDate)
                Dim MsgDs As New DataSet
                Dim SqlAd As SqlDataAdapter
                SqlAd = New SqlDataAdapter(objSQLCmd)
                SqlAd.Fill(MsgDs, "VanLogTbl")
                GetSalesbyVan = MsgDs
                objSQLCmd.Dispose()
            Catch ex As Exception
                Err_No = "74061"
                Err_Desc = ex.Message
                Throw ex
            Finally
                objSQLCmd = Nothing
                _objDB.CloseSQLConnection(objSQLConn)
            End Try
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetRouteCoverage(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal FSRID As String, ByVal StartDate As String, ByVal EndDate As String, ByVal OID As String) As DataSet
        Try
            Dim objSQLConn As SqlConnection
            Dim objSQLDa As New SqlDataAdapter
            Dim objSQLCmd As New SqlCommand
            Dim dt As New DataTable
            Try
                objSQLConn = _objDB.GetSQLConnection
                objSQLCmd = New SqlCommand("[rep_GetRouteCoverage]", objSQLConn)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.Parameters.AddWithValue("@FSRID", FSRID)
                objSQLCmd.Parameters.AddWithValue("@Start_Date", StartDate)
                objSQLCmd.Parameters.AddWithValue("@End_Date", EndDate)
                objSQLCmd.Parameters.AddWithValue("@OID", OID)

                Dim MsgDs As New DataSet
                Dim SqlAd As SqlDataAdapter
                SqlAd = New SqlDataAdapter(objSQLCmd)
                SqlAd.Fill(MsgDs, "VanLogTbl")
                GetRouteCoverage = MsgDs
                objSQLCmd.Dispose()
            Catch ex As Exception
                Err_No = "74061"
                Err_Desc = ex.Message
                Throw ex
            Finally
                objSQLCmd = Nothing
                _objDB.CloseSQLConnection(objSQLConn)
            End Try
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
