Imports System.Data.SqlClient
Imports System.Configuration
Public Class DAL_CustomerSegment
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Private _objDB As DatabaseConnection
    Private Shared _strSQLConn As String = ConfigurationSettings.AppSettings("SQLConnString")
    Private dtSegErrors As New DataTable

   
    Public Function GetCustomerSegment(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ID As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("select * from TBL_Sales_District where Sales_District_ID=@ID", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ID", ID)
            objSQLDA.Fill(dtSegErrors)
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
        GetCustomerSegment = dtSegErrors
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

   

    Public Function ManageCustomerSegment(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ID As String, ByVal Description As String, ByVal Opt As String, ByVal Code As String) As Boolean
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
            objSQLCmd.Parameters.Add(New SqlParameter("@Code", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Code").Value = Code
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

    Public Function SearchCustomerSegmentGrid(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal FilterBy As String, ByVal FilterValue As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter

        Dim Query As String = ""
        Try


            If FilterBy = "Segment Code" Then
                Query = "SELECT * FROM TBL_Customer_Segments WHERE Customer_Segment_Code LIKE '%" + FilterValue.ToUpper() + "%' ORDER BY Customer_Segment_Code"
            ElseIf FilterBy = "Description" Then
                Query = "SELECT * FROM TBL_Customer_Segments WHERE Description LIKE '%" + FilterValue + "%' ORDER BY Description"
            Else
                Query = "SELECT * FROM TBL_Customer_Segments ORDER BY Description"
            End If


            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter(Query, objSQLConn)
            objSQLDA.Fill(dtSegErrors)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "740022"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtSegErrors
    End Function
    Public Function CheckDuplicate(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Description As String) As Boolean
        Dim success As Boolean = False
        Dim objSQLConn As SqlConnection
        Dim objSQLCMD As SqlCommand
        Dim Rcnt As Integer
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCMD = New SqlCommand("SELECT COUNT(*) FROM TBL_Customer_Segments WHERE Description='" + Description + "'", objSQLConn)
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

    Public Function Validatecode(ByVal Customer_Segment_Code As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim iRowsAffected As Integer = 0
        Dim retVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection

            sQry = "Select count(*) from TBL_Customer_Segments  where Customer_Segment_Code =@Customer_Segment_Code"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@Customer_Segment_Code", Customer_Segment_Code)
            iRowsAffected = CInt(objSQLCmd.ExecuteScalar())
            If iRowsAffected > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function LoadExportCustomerSegmentTemplate() As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Dim Ds As New DataSet
        Dim sQry As String

        Try

            objSQLConn = _objDB.GetSQLConnection
            objSQLDa = New SqlDataAdapter("SELECT Description AS Description,Customer_Segment_Code AS  Code FROM TBL_Customer_Segments", objSQLConn)
            objSQLDa.SelectCommand.CommandType = CommandType.Text
            objSQLDa.Fill(dtSegErrors)
            objSQLDa.Dispose()

        Catch ex As Exception

            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        LoadExportCustomerSegmentTemplate = dtSegErrors
    End Function
End Class
