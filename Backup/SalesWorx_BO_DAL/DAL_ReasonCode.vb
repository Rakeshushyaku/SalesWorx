Imports System.Data.SqlClient
Imports System.Configuration

Public Class DAL_ReasonCode
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Private _objDB As DatabaseConnection
    Private Shared _strSQLConn As String = ConfigurationSettings.AppSettings("SQLConnString")
    Private dtReasonCode As New DataTable

    Public Function DeleteReasonCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ReasonCode As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sRetVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("app_DeleteReasonCode", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@ReasonCode", SqlDbType.VarChar, 10))
            objSQLCmd.Parameters("@ReasonCode").Value = ReasonCode
            objSQLCmd.ExecuteNonQuery()
            sRetVal = True
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74015"
            Err_Desc = ex.Message
            sRetVal = False
            Throw ex
        Finally
            objSQLCmd = Nothing
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        DeleteReasonCode = sRetVal
    End Function

    Public Function GetReasonCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ReasonCode As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_GetReasonCode", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ReasonCode", ReasonCode)
            objSQLDA.Fill(dtReasonCode)
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
        GetReasonCode = dtReasonCode
    End Function



    Public Function GetConnection() As SqlConnection
        Dim objSQLConn As SqlConnection
        objSQLConn = _objDB.GetSQLConnection
        Return objSQLConn
    End Function
    Public Sub CloseConnection(ByRef ObjCloseConn As SqlConnection)
        ' Dim objSQLConn As SqlConnection
        If ObjCloseConn IsNot Nothing Then
            _objDB.CloseSQLConnection(ObjCloseConn)
        End If
    End Sub

    Public Function InsertReasonCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ReasonCode As String, ByVal Description As String, ByVal Purpose As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_InsertReasonCode", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@ReasonCode", SqlDbType.VarChar, 10))
            objSQLCmd.Parameters("@ReasonCode").Value = ReasonCode
            objSQLCmd.Parameters.Add(New SqlParameter("@Description", SqlDbType.VarChar, 100))
            objSQLCmd.Parameters("@Description").Value = Description
            objSQLCmd.Parameters.Add(New SqlParameter("@Purpose", SqlDbType.VarChar, 10))
            objSQLCmd.Parameters("@Purpose").Value = Purpose
            objSQLCmd.ExecuteNonQuery()
            bRetVal = True
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        Catch ex As Exception
            Err_No = "740019"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function


    Public Function UpdateReasonCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ReasonCode As String, ByVal Description As String, ByVal Purpose As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_UpdateReasonCode", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@ReasonCode", SqlDbType.VarChar, 10))
            objSQLCmd.Parameters("@ReasonCode").Value = ReasonCode
            objSQLCmd.Parameters.Add(New SqlParameter("@Description", SqlDbType.VarChar, 100))
            objSQLCmd.Parameters("@Description").Value = Description
            objSQLCmd.Parameters.Add(New SqlParameter("@Purpose", SqlDbType.VarChar, 10))
            objSQLCmd.Parameters("@Purpose").Value = Purpose
            objSQLCmd.ExecuteNonQuery()
            bRetVal = True
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        Catch ex As Exception
            Err_No = "740020"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function

    Public Function FillReasonCodeGrid(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT * FROM TBL_Reason_Codes ORDER BY Reason_Code", objSQLConn)
            objSQLDA.Fill(dtReasonCode)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "740021"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtReasonCode
    End Function


    Public Function SearchReasonCodeGrid(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal FilterBy As String, ByVal FilterValue As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter

        Dim Query As String = ""
        Try
            If FilterBy = "Reason Code" Then
                Query = "SELECT * FROM TBL_Reason_Codes WHERE Reason_Code='" + FilterValue.ToUpper() + "' ORDER BY Reason_Code"
            ElseIf FilterBy = "Description" Then
                Query = "SELECT * FROM TBL_Reason_Codes WHERE Description LIKE '%" + FilterValue + "%' ORDER BY Reason_Code"
            Else
                Query = "SELECT * FROM TBL_Reason_Codes ORDER BY Reason_Code"
            End If
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter(Query, objSQLConn)
            objSQLDA.Fill(dtReasonCode)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "740022"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtReasonCode
    End Function

    Public Function CheckDuplicate(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ReasonCode As String) As Boolean
        Dim success As Boolean = False
        Dim objSQLConn As SqlConnection
        Dim objSQLCMD As SqlCommand
        Dim Rcnt As Integer
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCMD = New SqlCommand("SELECT COUNT(*) FROM TBL_Reason_Codes WHERE Reason_Code='" + ReasonCode + "'", objSQLConn)
            Rcnt = Convert.ToInt32(objSQLCMD.ExecuteScalar())
            objSQLCMD.Dispose()
            If Rcnt > 0 Then
                success = True
            End If
        Catch ex As Exception
            Err_No = "740023"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function


    Public Function DeleteAll(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sRetVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("DELETE FROM TBL_Reason_Codes", objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.ExecuteNonQuery()
            sRetVal = True
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74016"
            Err_Desc = ex.Message
            sRetVal = False
            Throw ex
        Finally
            objSQLCmd = Nothing
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        DeleteAll = sRetVal
    End Function

End Class

