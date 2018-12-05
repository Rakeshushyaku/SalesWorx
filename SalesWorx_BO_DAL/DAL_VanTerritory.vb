Imports System.Data
Imports System.Data.SqlClient
Public Class DAL_VanTerritory
    Private _objDB As DatabaseConnection
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub

    Public Function ReturnVanBasedCustomers(ByRef ds As DataSet, ByVal SalesRepID As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As New SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim SqlAd As SqlDataAdapter

        Try

            objSQLConn = _objDB.GetSQLConnection
            With objSQLCmd
                .Connection = objSQLConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "app_GReturnVanBasedCustomers"
                objSQLCmd.Parameters.Add(New SqlParameter("@SalesRep_ID", SqlDbType.Int))
                objSQLCmd.Parameters("@SalesRep_ID").Value = SalesRepID
            End With

            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(ds)

            Return True

        Catch ex As Exception
            Err_No = "1101"
            Err_Desc = ex.Message
            Return False
        Finally

            If Not objSQLCmd Is Nothing Then
                objSQLCmd.Dispose()
                objSQLCmd = Nothing
                SqlAd = Nothing

            End If

            _objDB.CloseSQLConnection(objSQLConn)

        End Try
    End Function
    Public Function ReturnVanBasedFilteredCustomers(ByRef ds As DataSet, ByVal SubQuery As String, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As New SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim SqlAd As SqlDataAdapter

        Try

            objSQLConn = _objDB.GetSQLConnection
            With objSQLCmd
                .Connection = objSQLConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "app_ReturnVanBasedFilteredCustomerss"
                objSQLCmd.Parameters.Add(New SqlParameter("@StrFitler", SqlDbType.VarChar, 600))
                objSQLCmd.Parameters("@StrFitler").Value = SubQuery
            End With

            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(ds)

            Return True

        Catch ex As Exception
            Err_No = "1102"
            Err_Desc = ex.Message
            Return False
        Finally

            If Not objSQLCmd Is Nothing Then
                objSQLCmd.Dispose()
                objSQLCmd = Nothing
                SqlAd = Nothing

            End If

            _objDB.CloseSQLConnection(objSQLConn)

        End Try
    End Function
    Public Function ReturnAllSalesRep(ByRef ds As DataSet, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As New SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim SqlAd As SqlDataAdapter

        Try

            objSQLConn = _objDB.GetSQLConnection
            With objSQLCmd
                .Connection = objSQLConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "app_ReturnAllSalesRep"
            End With

            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(ds)

            Return True

        Catch ex As Exception
            Err_No = "1103"
            Err_Desc = ex.Message
            Return False
        Finally

            If Not objSQLCmd Is Nothing Then
                objSQLCmd.Dispose()
                objSQLCmd = Nothing
                SqlAd = Nothing

            End If

            _objDB.CloseSQLConnection(objSQLConn)

        End Try
    End Function
    Public Function ReturnVanBasedCustomerSegment(ByRef ds As DataSet, ByVal SalesRepID As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As New SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim SqlAd As SqlDataAdapter

        Try

            objSQLConn = _objDB.GetSQLConnection
            With objSQLCmd
                .Connection = objSQLConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "app_ReturnVanBasedCustomerSegment"
                objSQLCmd.Parameters.Add(New SqlParameter("@SalesRep_ID", SqlDbType.Int))
                objSQLCmd.Parameters("@SalesRep_ID").Value = SalesRepID
            End With

            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(ds)

            Return True

        Catch ex As Exception
            Err_No = "1104"
            Err_Desc = ex.Message
            Return False
        Finally

            If Not objSQLCmd Is Nothing Then
                objSQLCmd.Dispose()
                objSQLCmd = Nothing
                SqlAd = Nothing

            End If

            _objDB.CloseSQLConnection(objSQLConn)

        End Try
    End Function
    Public Function ReturnVanBasedSalesDistrict(ByRef ds As DataSet, ByVal SalesRep_ID As Integer, ByVal Customer_Segment_ID As String, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As New SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim SqlAd As SqlDataAdapter

        Try

            objSQLConn = _objDB.GetSQLConnection
            With objSQLCmd
                .Connection = objSQLConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "app_ReturnVanBasedSalesDistrict"
                objSQLCmd.Parameters.Add(New SqlParameter("@SalesRep_ID", SqlDbType.Int))
                objSQLCmd.Parameters("@SalesRep_ID").Value = SalesRep_ID
                objSQLCmd.Parameters.Add(New SqlParameter("@Customer_Segment_ID", SqlDbType.VarChar, 3))
                objSQLCmd.Parameters("@Customer_Segment_ID").Value = Customer_Segment_ID

            End With

            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(ds)

            Return True

        Catch ex As Exception
            Err_No = "1104"
            Err_Desc = ex.Message
            Return False
        Finally

            If Not objSQLCmd Is Nothing Then
                objSQLCmd.Dispose()
                objSQLCmd = Nothing
                SqlAd = Nothing

            End If

            _objDB.CloseSQLConnection(objSQLConn)

        End Try
    End Function

    Public Function AssignVanToSalesDistrict(ByVal CustSegmentId As String, ByVal SalesDistrictID As String, ByVal SalesRepID As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Dim objSQLConn As SqlConnection
            Dim Result As Boolean = False
            objSQLConn = _objDB.GetSQLConnection

            Dim objSQLCmd As New SqlCommand("app_AssignVanToSalesDistrict", objSQLConn)
            With objSQLCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.AddWithValue("@SalesRep_ID", SalesRepID)
                .Parameters.AddWithValue("@Sales_District_ID", SalesDistrictID)
                .Parameters.AddWithValue("@Customer_Segment_ID", CustSegmentId)
            End With

            Dim objOutputParameter As New SqlParameter("@Result", SqlDbType.Int)
            objSQLCmd.Parameters.Add(objOutputParameter)
            objOutputParameter.Direction = ParameterDirection.Output
            objSQLCmd.ExecuteNonQuery()
            If Convert.ToInt32(objOutputParameter.Value) = -1 Then
                Throw New Exception("ALREADY EXISTS")

            End If
            Return True
        Catch ex As Exception
            Err_No = 74201
            Err_Desc = "Error occured while assigning van to territory"
            Throw ex
        End Try
    End Function

    Public Function UpdateAssignVanToSalesDistrict(ByVal AF_MAP_ID As Integer, ByVal CustSegmentId As String, ByVal SalesDistrictID As String, ByVal SalesRepID As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Dim objSQLConn As SqlConnection
            Dim Result As Boolean = False
            objSQLConn = _objDB.GetSQLConnection

            Dim objSQLCmd As New SqlCommand("app_UpdateAssignVanToSalesDistrict", objSQLConn)
            With objSQLCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.AddWithValue("@AF_Map_ID", AF_MAP_ID)
                .Parameters.AddWithValue("@SalesRep_ID", SalesRepID)
                .Parameters.AddWithValue("@Sales_District_ID", SalesDistrictID)
                .Parameters.AddWithValue("@Customer_Segment_ID", CustSegmentId)
            End With

            Dim objOutputParameter As New SqlParameter("@Result", SqlDbType.Int)
            objSQLCmd.Parameters.Add(objOutputParameter)
            objOutputParameter.Direction = ParameterDirection.Output
            objSQLCmd.ExecuteNonQuery()
            If Convert.ToInt32(objOutputParameter.Value) = -1 Then
                Throw New Exception("ALREADY EXISTS")

            End If
            Return True
        Catch ex As Exception
            Err_No = 74201
            Err_Desc = "Error occured while assigning van to territory"
            Throw ex
        End Try
    End Function
    Public Function ReturnAllCustomerSegments(ByRef ds As DataSet, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As New SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim SqlAd As SqlDataAdapter

        Try

            objSQLConn = _objDB.GetSQLConnection
            With objSQLCmd
                .Connection = objSQLConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "app_ReturnAllCustomerSegments"

            End With

            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(ds)

            Return True

        Catch ex As Exception
            Err_No = "1201"
            Err_Desc = ex.Message
            Return False
        Finally

            If Not objSQLCmd Is Nothing Then
                objSQLCmd.Dispose()
                objSQLCmd = Nothing
                SqlAd = Nothing

            End If

            _objDB.CloseSQLConnection(objSQLConn)

        End Try
    End Function
    Public Function ReturnAllSalesDistricts(ByRef ds As DataSet, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As New SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim SqlAd As SqlDataAdapter

        Try

            objSQLConn = _objDB.GetSQLConnection
            With objSQLCmd
                .Connection = objSQLConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "app_ReturnAllSalesDistricts"

            End With

            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(ds)

            Return True

        Catch ex As Exception
            Err_No = "1202"
            Err_Desc = ex.Message
            Return False
        Finally

            If Not objSQLCmd Is Nothing Then
                objSQLCmd.Dispose()
                objSQLCmd = Nothing
                SqlAd = Nothing

            End If

            _objDB.CloseSQLConnection(objSQLConn)

        End Try
    End Function
    Public Function GetVanTerritories(ByRef ds As DataSet, ByVal Criteria As String, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As New SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim SqlAd As SqlDataAdapter

        Try

            objSQLConn = _objDB.GetSQLConnection
            With objSQLCmd
                .Connection = objSQLConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "app_GetVanTerritories"
                .Parameters.AddWithValue("@Criteria", Criteria)
            End With

            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(ds)

            Return True

        Catch ex As Exception
            Err_No = "74202"
            Err_Desc = "Error while retrieving VanTerritories "
            Return False
        Finally

            If Not objSQLCmd Is Nothing Then
                objSQLCmd.Dispose()
                objSQLCmd = Nothing
                SqlAd = Nothing

            End If

            _objDB.CloseSQLConnection(objSQLConn)

        End Try
    End Function

    Public Function DeleteVanTerritoryAssignment(ByVal IDCollection As String, ByRef Err_No As Long, ByRef Err_Desc As String)
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim iRowsAffected As Integer = 0
        Dim retVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            sQry = String.Format("delete from TBL_Area_FSR_Map where AF_Map_ID in ({0})", IDCollection)
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            iRowsAffected = objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()

            If iRowsAffected > 0 Then
                retVal = True
            End If
        Catch ex As Exception

            Err_No = 74203
            Err_Desc = String.Format("Error while deleting van territory assignment: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        DeleteVanTerritoryAssignment = retVal
    End Function
End Class
