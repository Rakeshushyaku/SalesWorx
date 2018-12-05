Imports System.Data
Imports System.Data.SqlClient
Public Class DAL_CustomerManagement
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
End Class
