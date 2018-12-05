Imports System.Data.SqlClient
Imports System.Configuration
Public Class DAL_LatiLongitude
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Private _objDB As DatabaseConnection
    Private Shared _strSQLConn As String = ConfigurationSettings.AppSettings("SQLConnString")
    Private dtCurrency As New DataTable
    Public Function FillCusShipAddress(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT *,Customer_Name +N'-' +ISNUll(Location,'N/A') AS CustName,ISNULL(Cust_Lat,'0')AS CustLat,ISNULL(Cust_Long,'0')AS CustLong FROM TBL_Customer_Ship_Address ORDER BY Customer_No ", objSQLConn)
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
    Public Function SearchLatiLongitude(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal FilterBy As String, ByVal FilterValue As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter

        Dim Query As String = ""
        Try
            If FilterBy = "Customer_No" Then
                Query = "SELECT *,Customer_Name +N'-' +ISNUll(Location,'N/A') AS CustName,ISNULL(Cust_Lat,'0')AS CustLat,ISNULL(Cust_Long,'0')AS CustLong FROM TBL_Customer_Ship_Address WHERE Customer_No Like '%" + FilterValue.ToUpper() + "%' ORDER BY Customer_No"
            ElseIf FilterBy = "Customer_Name" Then
                Query = "SELECT *,Customer_Name +N'-' +ISNUll(Location,'N/A') AS CustName,ISNULL(Cust_Lat,'0')AS CustLat,ISNULL(Cust_Long,'0')AS CustLong FROM TBL_Customer_Ship_Address WHERE Customer_Name LIKE '%" + FilterValue + "%' ORDER BY Customer_No"
            ElseIf FilterBy = "Address" Then
                Query = "SELECT *,Customer_Name +N'-' +ISNUll(Location,'N/A') AS CustName,ISNULL(Cust_Lat,'0')AS CustLat,ISNULL(Cust_Long,'0')AS CustLong FROM TBL_Customer_Ship_Address WHERE Address LIKE '%" + FilterValue + "%' ORDER BY Customer_No"
            Else
                Query = "SELECT *,Customer_Name +N'-' +ISNUll(Location,'N/A') AS CustName,ISNULL(Cust_Lat,'0')AS CustLat,ISNULL(Cust_Long,'0')AS CustLong FROM TBL_Customer_Ship_Address ORDER BY Customer_No"
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
    Public Function UpdateLatiLongitude(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Latitude As Double, ByVal Longitude As Double, ByVal CusID As Integer, ByVal SiteID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("UPDATE TBL_Customer_Ship_Address SET Cust_Lat=@Latitude,Cust_Long=@Longitude Where Customer_ID=@CusID AND Site_Use_ID=@SiteID", objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add(New SqlParameter("@Latitude", SqlDbType.Decimal))
            objSQLCmd.Parameters("@Latitude").Value = Latitude
            objSQLCmd.Parameters.Add(New SqlParameter("@Longitude", SqlDbType.Decimal))
            objSQLCmd.Parameters("@Longitude").Value = Longitude
            objSQLCmd.Parameters.Add(New SqlParameter("@CusID", SqlDbType.Int))
            objSQLCmd.Parameters("@CusID").Value = CusID
            objSQLCmd.Parameters.Add(New SqlParameter("@SiteID", SqlDbType.Int))
            objSQLCmd.Parameters("@SiteID").Value = SiteID
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
    Public Function SaveData(ByVal DataTbl As DataTable, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False

        Try
            ''Doing a batch update
            ' Dim MainRow As Integer = 0
            ' Dim SubRow As Integer = 0
            Dim Qry As String = ""
            objSQLConn = New SqlConnection(_strSQLConn)
            For Each dr As DataRow In DataTbl.Rows

                '  If SubRow = 0 Then
                If Not (dr(0) Is DBNull.Value And dr(1) Is DBNull.Value And dr(2) Is DBNull.Value) Then
                    Qry = " UPDATE  TBL_Customer_Ship_Address SET Sync_TimeStamp=GETDATE(), Cust_Lat ='" & dr(1) & "',Cust_Long='" & dr(2) & "' WHERE Customer_No='" & dr(0) & "'"
                    ' Else
                    '  Qry += " ; "
                    '  Qry += " UPDATE  TBL_Customer_Ship_Address SET Cust_Lat ='" & dr(1) & "',Cust_Long='" & dr(2) & "' WHERE Customer_No='" & dr(0) & "'"
                    '  End If

                    '  SubRow = SubRow + 1
                    '  MainRow = MainRow + 1

                    '  If (SubRow = 25 Or MainRow = DataTbl.Rows.Count) And Qry <> "" Then
                    Try
                        objSQLCmd = New SqlCommand(Qry, objSQLConn)
                        objSQLCmd.CommandType = CommandType.Text
                        objSQLConn.Open()
                        objSQLCmd.ExecuteNonQuery()
                        bRetVal = True
                        objSQLCmd.Dispose()
                        objSQLCmd = Nothing
                        objSQLConn.Close()
                    Catch ex As Exception
                        Err_No = "74020"
                        Err_Desc += ex.Message
                        Throw ex
                    Finally
                        objSQLConn.Close()
                    End Try

                    '   SubRow = 0
                    Qry = ""
                End If
                ' End If
            Next
        Catch ex As Exception
            Err_No = "74020"
            Err_Desc += ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function
End Class
