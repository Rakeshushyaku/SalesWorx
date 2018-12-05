Imports System.Data.SqlClient
Imports System.Configuration
Public Class DAL_VanRequisitions
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Private _objDB As DatabaseConnection
    Private Shared _strSQLConn As String = ConfigurationSettings.AppSettings("SQLConnString")


    Public Function GetOutstandingStockRequistion(ByVal Criteria As String) As DataTable
        Try


            Dim objSQLConn As SqlConnection
            Dim objSQLDa As New SqlDataAdapter
            Dim objSQLCmd As New SqlCommand
            Dim dt As New DataTable

            Dim sQry As String

            Try
                'getting MSSQL DB connection.....
                objSQLConn = _objDB.GetSQLConnection
                objSQLCmd = New SqlCommand("app_GetOutstandingStockRequistion", objSQLConn)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.Parameters.AddWithValue("@criteria", Criteria)
                objSQLDa = New SqlDataAdapter(objSQLCmd)
                objSQLDa.Fill(dt)
                objSQLCmd.Dispose()
                Return dt
            Catch ex As Exception
                Throw ex
            Finally

                objSQLCmd = Nothing
                _objDB.CloseSQLConnection(objSQLConn)
            End Try

            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Approve(ByVal ApprovedBy As Integer, ByVal ReqID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim iRowsAffected As Integer = 0
        Dim retVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            sQry = String.Format("update TBL_Stock_Requisition set Status='Y', Approved_By='{0}' where StockRequisition_ID='{1}'", ApprovedBy, ReqID)
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            iRowsAffected = objSQLCmd.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
