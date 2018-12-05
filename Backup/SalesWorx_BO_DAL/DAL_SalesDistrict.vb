Imports System.Data.SqlClient
Imports System.Configuration
Public Class DAL_SalesDistrict
 Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Private _objDB As DatabaseConnection
    Private Shared _strSQLConn As String = ConfigurationSettings.AppSettings("SQLConnString")
    Private dtCurrency As New DataTable

    Public Function DeleteSalesDistrict(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sRetVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            Dim Dr As SqlDataReader
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("app_DeleteSalesDistrict", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@ID", SqlDbType.VarChar))
            objSQLCmd.Parameters("@ID").Value = ID
            Dr = objSQLCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If Dr.Read Then
                If Val(Dr(0).ToString) > 0 Then
                    sRetVal = True
                Else
                    Err_Desc = "The Sales district is referenced in Van Territory Mapping/Customer ship address. You can not delete this."
                End If
            End If
            Dr.Close()
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
        DeleteSalesDistrict = sRetVal
    End Function

    Public Function GetSalesDistrict(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ID As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("select * from TBL_Sales_District where Sales_District_ID=@ID", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ID", ID)
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
        GetSalesDistrict = dtCurrency
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

    Public Function InsertSalesDistrict(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Description As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            Dim Dr As SqlDataReader
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_InsertSalesDistrict", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@Description", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Description").Value = Description
            Dr = objSQLCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If Dr.Read Then
                If Val(Dr(0).ToString) > 0 Then
                    bRetVal = True
                Else
                    Err_Desc = "The sales district already exists."
                End If
            End If
            Dr.Close()
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


    Public Function UpdateSalesDistrict(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ID As String, ByVal Description As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False

        Try
            Dim Dr As SqlDataReader
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_UpdateSalesDistrict", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@ID", SqlDbType.VarChar))
            objSQLCmd.Parameters("@ID").Value = ID
            objSQLCmd.Parameters.Add(New SqlParameter("@Description", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Description").Value = Description
            Dr = objSQLCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If Dr.Read Then
                If Val(Dr(0).ToString) > 0 Then
                    bRetVal = True
                Else
                    Err_Desc = "The sales district already exists."
                End If
            End If
            Dr.Close()
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

    Public Function SearchSalesDistrictGrid(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal FilterValue As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter

        Dim Query As String = ""
        Try

            If FilterValue.Trim() <> "" Then
                Query = "SELECT * FROM TBL_Sales_District WHERE Description LIKE '%" + FilterValue + "%' ORDER BY Description"
            Else
                Query = "SELECT * FROM TBL_Sales_District ORDER BY Description"
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

    Public Function CheckDuplicate(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Description As String) As Boolean
        Dim success As Boolean = False
        Dim objSQLConn As SqlConnection
        Dim objSQLCMD As SqlCommand
        Dim Rcnt As Integer
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCMD = New SqlCommand("SELECT COUNT(*) FROM TBL_Sales_District WHERE Description='" + Description + "'", objSQLConn)
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
