Imports System.Data.SqlClient
Imports System.Configuration
Public Class DAL_VanRequisitionDetails
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Private _objDB As DatabaseConnection
    Private Shared _strSQLConn As String = ConfigurationSettings.AppSettings("SQLConnString")

    Public Function GetVanRequisitionDetails(ByVal ReqID As String, ByVal SalesRepID As String) As DataTable
        Try


            Dim objSQLConn As SqlConnection
            Dim objSQLDa As New SqlDataAdapter
            Dim objSQLCmd As New SqlCommand
            Dim dt As New DataTable

            Dim sQry As String

            Try
                'getting MSSQL DB connection.....
                objSQLConn = _objDB.GetSQLConnection
                objSQLCmd = New SqlCommand("app_GetStockRequisitionDetails", objSQLConn)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.Parameters.AddWithValue("@ID", ReqID)
                objSQLCmd.Parameters.AddWithValue("@SalesRepID", SalesRepID)
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

End Class

