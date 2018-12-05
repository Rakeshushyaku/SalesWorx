Option Explicit On
Option Strict On

Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Configuration
Public Class DB_Handler
    Private _strSQLConn As String = ConfigurationSettings.AppSettings("BOConnectionString")
    Private _objSQLConn As SqlConnection

    Public Function GetSQLConnection() As SqlConnection
        Try
            _objSQLConn = New SqlConnection(_strSQLConn)
            _objSQLConn.Open()
        Catch ex As Exception
            Throw ex
        End Try
        GetSQLConnection = _objSQLConn
    End Function

    Public Sub CloseSQLConnection(ByRef objSQLConn As SqlConnection)
        Try
            If objSQLConn.State = ConnectionState.Open Then
                objSQLConn.Close()
            End If
        Catch ex As Exception
            Throw ex
        Finally
            objSQLConn = Nothing
        End Try
    End Sub
End Class
