Imports System.Data.SqlClient
Imports System.Configuration
Public Class DAL_Bank
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Private _objDB As DatabaseConnection
    Private Shared _strSQLConn As String = ConfigurationSettings.AppSettings("SQLConnString")
    Private dtBank As New DataTable

    Public Function FillCurrency(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter(" SELECT NULL as Currency_Code, NULL as Description ,NULL as Conversion_Rate,NULL as Decimal_Digits, 'SELECT'  as Country FROM TBL_Currency UNION SELECT * FROM TBL_Currency ", objSQLConn)
            objSQLDA.Fill(dtBank)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "740021"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtBank
    End Function

    Public Function FillBankCodeGrid(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT * FROM  TBL_App_Codes where Code_Type='BANK'", objSQLConn)
            objSQLDA.Fill(dtBank)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "740021"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtBank
    End Function

    Public Function CheckDuplicate(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal BankCode As String) As Boolean
        Dim success As Boolean = False
        Dim objSQLConn As SqlConnection
        Dim objSQLCMD As SqlCommand
        Dim Rcnt As Integer
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCMD = New SqlCommand("SELECT COUNT(*) FROM  TBL_App_Codes where Code_Type='BANK' AND LTRIM(RTRIM(Code_Value))='" + BankCode.Trim() + "'", objSQLConn)
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

    Public Function InsertBankCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal BankCode As String, ByVal Description As String, ByVal Currency As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_InsertBankCode", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@BankCode", SqlDbType.VarChar, 50))
            objSQLCmd.Parameters("@BankCode").Value = BankCode
            objSQLCmd.Parameters.Add(New SqlParameter("@Description", SqlDbType.VarChar, 100))
            objSQLCmd.Parameters("@Description").Value = Description
            objSQLCmd.Parameters.Add(New SqlParameter("@Currency", SqlDbType.VarChar, 10))
            objSQLCmd.Parameters("@Currency").Value = Currency
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

    Public Function UpdateBankCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal BankCode As String, ByVal Description As String, ByVal Currency As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_UpdateBankCode", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@BankCode", SqlDbType.VarChar, 50))
            objSQLCmd.Parameters("@BankCode").Value = BankCode
            objSQLCmd.Parameters.Add(New SqlParameter("@Description", SqlDbType.VarChar, 100))
            objSQLCmd.Parameters("@Description").Value = Description
            objSQLCmd.Parameters.Add(New SqlParameter("@Currency", SqlDbType.VarChar, 10))
            objSQLCmd.Parameters("@Currency").Value = Currency
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

    Public Function DeleteBankCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal BankCode As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sRetVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("app_DeleteBankCode", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@BankCode", SqlDbType.VarChar, 50))
            objSQLCmd.Parameters("@BankCode").Value = BankCode
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
        DeleteBankCode = sRetVal
    End Function

    Public Function SearchBankCodeGrid(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal FilterBy As String, ByVal FilterValue As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter

        Dim Query As String = ""
        Try
            If FilterBy = "Bank Code" Then
                Query = "SELECT * FROM TBL_App_Codes WHERE  Code_Type='BANK' AND Code_Value LIKE '%" + FilterValue.ToUpper() + "%' ORDER BY Code_Value"
            ElseIf FilterBy = "Description" Then
                Query = "SELECT * FROM TBL_App_Codes WHERE Code_Type='BANK' AND Code_Description LIKE '%" + FilterValue + "%' ORDER BY Code_Value"
            Else
                Query = "SELECT * FROM TBL_App_Codes WHERE  Code_Type='BANK' ORDER BY Code_Value"
            End If
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter(Query, objSQLConn)
            objSQLDA.Fill(dtBank)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "740022"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtBank
    End Function
End Class
