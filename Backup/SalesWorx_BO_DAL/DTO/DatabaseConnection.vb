Option Explicit On 
Option Strict On

Imports System.Data.OracleClient
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Configuration

Public Class DatabaseConnection

    Private _strSQLConn As String = ConfigurationSettings.AppSettings("SQLConnString")
    Private Shared _strBODBName As String = String.Format("SalesWorx_BO_{0}", ConfigurationSettings.AppSettings("CUST_SUFFIX"))
    Private Shared _strBOSyncDBName As String = String.Format("SalesWorx_BO_Sync_{0}", ConfigurationSettings.AppSettings("CUST_SUFFIX"))
    Private _objSQLConn As SqlConnection


    Public Function GetSQLConnection() As SqlConnection
        _objSQLConn = New SqlConnection(_strSQLConn)
        _objSQLConn.Open()
        GetSQLConnection = _objSQLConn
    End Function

    Public Sub CloseSQLConnection(ByRef objSQLConn As SqlConnection)
        Try
            If objSQLConn.State = ConnectionState.Open Then
                objSQLConn.Close()
            End If
        Catch ex As Exception
        Finally
            objSQLConn = Nothing
        End Try
    End Sub
    Public Shared Function BODatabase() As String
        Return _strBODBName
    End Function

    Public Shared Function BOSyncDatabase() As String
        Return _strBOSyncDBName
    End Function
End Class
