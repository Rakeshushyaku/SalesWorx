Imports System.Data.SqlClient
Imports System.Configuration
Public Class DAL_LatiLongitude
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Private _objDB As DatabaseConnection
    Private Shared _strSQLConn As String = ConfigurationSettings.AppSettings("SQLConnString")
    Private dtLatLong As New DataTable

    Dim objSQLConn As SqlConnection
    Dim objSQLDA As SqlDataAdapter
      
    Public Function FillCusShipAddress(ByRef Err_No As Long, ByRef Err_Desc As String, Org As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT *,Customer_Name +N'-' +ISNUll(Location,'N/A') AS CustName,ISNULL(Cust_Lat,'0')AS CustLat,ISNULL(Cust_Long,'0')AS CustLong FROM dbo.app_GetOrgCustomerShipAddress(@OrgID) ORDER BY Customer_No ", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", Org)
            objSQLDA.Fill(dt)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "90123"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        FillCusShipAddress = dt
    End Function
    Public Function FillCusShipAddress(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT *,Customer_Name +N'-' +ISNUll(Location,'N/A') AS CustName,ISNULL(Cust_Lat,'0')AS CustLat,ISNULL(Cust_Long,'0')AS CustLong FROM TBL_Customer_Ship_Address ORDER BY Customer_No ", objSQLConn)
            objSQLDA.Fill(dtLatLong)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "740021"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtLatLong
    End Function
    Public Function SearchLatiLongitude(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal FilterBy As String, ByVal FilterValue As String, Org As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter

        Dim Query As String = ""
        Try
            If FilterBy = "Customer_No" Then
                Query = "SELECT *,Customer_Name +N'-' +ISNUll(Location,'N/A') AS CustName,ISNULL(Cust_Lat,'0')AS CustLat,ISNULL(Cust_Long,'0')AS CustLong FROM dbo.app_GetOrgCustomerShipAddress('" & Org & "') WHERE Customer_No Like '%" + FilterValue.ToUpper() + "%' ORDER BY Customer_No"
            ElseIf FilterBy = "Customer_Name" Then
                Query = "SELECT *,Customer_Name +N'-' +ISNUll(Location,'N/A') AS CustName,ISNULL(Cust_Lat,'0')AS CustLat,ISNULL(Cust_Long,'0')AS CustLong FROM dbo.app_GetOrgCustomerShipAddress('" & Org & "')  WHERE Customer_Name LIKE '%" + FilterValue + "%' ORDER BY Customer_No"
            ElseIf FilterBy = "Address" Then
                Query = "SELECT *,Customer_Name +N'-' +ISNUll(Location,'N/A') AS CustName,ISNULL(Cust_Lat,'0')AS CustLat,ISNULL(Cust_Long,'0')AS CustLong FROM dbo.app_GetOrgCustomerShipAddress('" & Org & "')  WHERE Address LIKE '%" + FilterValue + "%' ORDER BY Customer_No"
            Else
                Query = "SELECT *,Customer_Name +N'-' +ISNUll(Location,'N/A') AS CustName,ISNULL(Cust_Lat,'0')AS CustLat,ISNULL(Cust_Long,'0')AS CustLong FROM dbo.app_GetOrgCustomerShipAddress('" & Org & "')  ORDER BY Customer_No"
            End If
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter(Query, objSQLConn)
            objSQLDA.Fill(dtLatLong)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "740022"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtLatLong
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

    Public Function GetLastVisit(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustID As String, ByVal SiteID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtvisit As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("select ISNULL(Latitude,0) as Last_Latitude,ISNULL(Longitude,0) as Last_Longitude,* from TBL_FSR_Actual_Visits  where Customer_ID ='" & CustID & "' and Site_Use_ID ='" & SiteID & "' and Visit_Start_Date=(select MAX(Visit_Start_Date) from TBL_FSR_Actual_Visits  where Customer_ID ='" & CustID & "' and Site_Use_ID ='" & SiteID & "' ) ", objSQLConn)
            objSQLDA.Fill(dtvisit)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "740025"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtvisit
    End Function
    Public Function GetGEO_loc_mod(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT * FROM TBL_App_Control  where Control_Key='UPDATE_GEO_LOC_MOD_BO' ", objSQLConn)
            objSQLDA.Fill(dtLatLong)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "740025"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtLatLong
    End Function


    Public Function GetExpGEOLocation(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustID As String, ByVal SiteID As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtGeo As New DataTable

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_GetExpGEOLocation", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CustID", CustID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", SiteID)
            objSQLDA.Fill(dtGeo)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "74204"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetExpGEOLocation = dtGeo
    End Function
End Class
