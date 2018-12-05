Imports System.Data.SqlClient
Imports System.Configuration
Public Class DAL_CustomerSegment
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Private _objDB As DatabaseConnection
    Private Shared _strSQLConn As String = ConfigurationSettings.AppSettings("SQLConnString")
    Private dtCurrency As New DataTable

   
    Public Function GetCustomerSegment(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ID As String) As DataTable

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
        GetCustomerSegment = dtCurrency
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

   

    Public Function ManageCustomerSegment(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ID As String, ByVal Description As String, ByVal Opt As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False

        Try
            Dim Dr As SqlDataReader
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_ManageCustomerSegment", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
             objSQLCmd.Parameters.Add(New SqlParameter("@OPT", SqlDbType.Int))
            objSQLCmd.Parameters("@OPT").Value = Opt
            objSQLCmd.Parameters.Add(New SqlParameter("@ID", SqlDbType.VarChar))
            objSQLCmd.Parameters("@ID").Value = ID
            objSQLCmd.Parameters.Add(New SqlParameter("@Description", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Description").Value = Description
            Dr = objSQLCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If Dr.Read Then
                If Val(Dr(0).ToString) > 0 Then
                    bRetVal = True
                Else
                    If Opt = 1 Or Opt = 2 Then
                     Err_Desc = "The customer segment already exists."
                    Else
                     Err_Desc = "The customer segment is referenced in Van Territory Mapping/Customer ship address. You can not delete this."
                    End If
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

    Public Function SearchCustomerSegmentGrid(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal FilterValue As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter

        Dim Query As String = ""
        Try

            If FilterValue.Trim() <> "" Then
                Query = "SELECT * FROM TBL_Customer_Segments WHERE Description LIKE '%" + FilterValue + "%' ORDER BY Description"
            Else
                Query = "SELECT * FROM TBL_Customer_Segments ORDER BY Description"
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

  
End Class
