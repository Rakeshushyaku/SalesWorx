Imports System.Data.SqlClient
Imports System.Configuration
Public Class DAL_ApprovalCode
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub

    Private _objDB As DatabaseConnection
    Private Shared _strSQLConn As String = ConfigurationSettings.AppSettings("SQLConnString")
    Private dtCode As New DataTable

    Public Function GetApprovalCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal FSR As Integer, ByVal UserID As Integer) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_GetApprovalCode", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@User_ID", UserID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SalesRep_ID", FSR)
            objSQLDA.Fill(dtCode)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "74016"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetApprovalCode = dtCode
    End Function
End Class
