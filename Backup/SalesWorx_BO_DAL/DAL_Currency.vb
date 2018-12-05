Imports System.Data.SqlClient
Imports System.Configuration

Public Class DAL_Currency
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Private _objDB As DatabaseConnection
    Private Shared _strSQLConn As String = ConfigurationSettings.AppSettings("SQLConnString")
    Private dtCurrency As New DataTable

    Public Function DeleteCurrency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CurrencyCode As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sRetVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("app_DeleteCurrency", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@CurrencyCode", SqlDbType.VarChar, 10))
            objSQLCmd.Parameters("@CurrencyCode").Value = CurrencyCode
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
        DeleteCurrency = sRetVal
    End Function

    Public Function GetCurrency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CurrencyCode As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_GetCurrency", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CurrencyCode", CurrencyCode)
            objSQLDA.Fill(dtCurrency)
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
        GetCurrency = dtCurrency
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

    Public Function InsertCurrency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CurrencyCode As String, ByVal Description As String, ByVal ConvertRate As Decimal, ByVal DecimalDigits As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_InsertCurrency", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@CurrencyCode", SqlDbType.VarChar, 10))
            objSQLCmd.Parameters("@CurrencyCode").Value = CurrencyCode
            objSQLCmd.Parameters.Add(New SqlParameter("@Description", SqlDbType.VarChar, 100))
            objSQLCmd.Parameters("@Description").Value = Description
            objSQLCmd.Parameters.Add(New SqlParameter("@ConvertRate", SqlDbType.Decimal))
            objSQLCmd.Parameters("@ConvertRate").Value = ConvertRate
            objSQLCmd.Parameters.Add(New SqlParameter("@DecimalDigits", SqlDbType.SmallInt))
            objSQLCmd.Parameters("@DecimalDigits").Value = DecimalDigits

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


    Public Function UpdateCurrency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CurrencyCode As String, ByVal Description As String, ByVal ConvertRate As Decimal, ByVal DecimalDigits As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_UpdateCurrency", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@CurrencyCode", SqlDbType.VarChar, 10))
            objSQLCmd.Parameters("@CurrencyCode").Value = CurrencyCode
            objSQLCmd.Parameters.Add(New SqlParameter("@Description", SqlDbType.VarChar, 100))
            objSQLCmd.Parameters("@Description").Value = Description
            objSQLCmd.Parameters.Add(New SqlParameter("@ConvertRate", SqlDbType.Decimal))
            objSQLCmd.Parameters("@ConvertRate").Value = ConvertRate
            objSQLCmd.Parameters.Add(New SqlParameter("@DecimalDigits", SqlDbType.SmallInt))
            objSQLCmd.Parameters("@DecimalDigits").Value = DecimalDigits
            objSQLCmd.ExecuteNonQuery()
            bRetVal = True
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        Catch ex As Exception
            Err_No = "7400920"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function

    Public Function FillCurrencyGrid(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT * FROM TBL_Currency ORDER BY Currency_Code", objSQLConn)
            objSQLDA.Fill(dtCurrency)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "740021"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtCurrency
    End Function


    Public Function SearchCurrencyGrid(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal FilterBy As String, ByVal FilterValue As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter

        Dim Query As String = ""
        Try
            If FilterBy = "Currency Code" Then
                Query = "SELECT * FROM TBL_Currency WHERE Currency_Code='" + FilterValue.ToUpper() + "' ORDER BY Currency_Code"
            ElseIf FilterBy = "Description" Then
                Query = "SELECT * FROM TBL_Currency WHERE Description LIKE '%" + FilterValue + "%' ORDER BY Currency_Code"
            Else
                Query = "SELECT * FROM TBL_Currency ORDER BY Currency_Code"
            End If
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter(Query, objSQLConn)
            objSQLDA.Fill(dtCurrency)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "740022"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtCurrency
    End Function

    Public Function CheckDuplicate(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CurrencyCode As String) As Boolean
        Dim success As Boolean = False
        Dim objSQLConn As SqlConnection
        Dim objSQLCMD As SqlCommand
        Dim Rcnt As Integer
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCMD = New SqlCommand("SELECT COUNT(*) FROM TBL_Currency WHERE Currency_Code='" + CurrencyCode.ToUpper() + "'", objSQLConn)
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

End Class
