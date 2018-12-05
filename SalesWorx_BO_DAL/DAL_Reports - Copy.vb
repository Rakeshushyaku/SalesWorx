Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Data
Public Class DAL_Reports
    Private _objDB As DatabaseConnection
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub

    Public Function GetCustomerListing(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchParams As String, ByVal OrgId As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_LoadCustomerList", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@SearchParams", SearchParams)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgId", OrgId)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function


    Public Function GetCoveredvsBilled(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, Van As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_OutletsCoveredBilled", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", Van)

            objSQLDA.Fill(dt)


            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim dRow As DataRow = dt.Rows(i)

                    If dRow("Covered") IsNot DBNull.Value And dRow("Billed") IsNot DBNull.Value Then
                        If dRow("Covered") = 0 Or dRow("Billed") = 0 Then
                            dRow("Percentage") = 0
                        Else
                            dRow("Percentage") = (CInt(dRow("Billed")) / CInt(dRow("Covered"))) * 100
                        End If

                    End If
                Next
                dt.AcceptChanges()
            End If

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetVanPerformance(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, Van As String, SalesDistrictID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_VanPerformance", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", Van)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SalesDistrictID", SalesDistrictID)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function GetCustomerVisits(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Qry As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_CustomerVisits", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgId", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SearchParams", Qry)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "247769"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function GetDistributionCheckList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Qry As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_LoadDisCheckList", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgId", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SearchParams", Qry)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "2477769"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetInvDiscountDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Qry As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetInvoiceDiscountDetails", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgId", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SearchParams", Qry)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "2477769"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetCustomerVisitsSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, SID As String, Uid As String, CustID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_CustomerVisit_TotalAmt", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", Uid)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CustID", CustID)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "247769"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetDailySalesReport_Order(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, FromDate As String, ToDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_DailySalesReport_Order", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", Uid)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetSalesReturnsbyAgencyDetailed(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, Type As String, FromDate As String, ToDate As String, Agency As String, Item As String, Customer As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_SalesByAgency_V2_MonthlyWeekly", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", Uid)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Type", Type)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Agency", Agency)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Item", Item)
            If Customer = "0" Then
                objSQLDA.SelectCommand.Parameters.AddWithValue("@Customer", 0)
                objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", 0)
            Else
                Dim ids() As String
                ids = Customer.Split("$")
                objSQLDA.SelectCommand.Parameters.AddWithValue("@Customer", ids(0))
                objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", ids(1))
            End If
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetSalesReturnsbyVanDetailed(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, Type As String, FromDate As String, ToDate As String, Agency As String, Item As String, Customer As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_SalesByVan_V2_MonthlyWeekly", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", Uid)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Type", Type)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Agency", Agency)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Item", Item)
            If Customer = "0" Then
                objSQLDA.SelectCommand.Parameters.AddWithValue("@Customer", 0)
                objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", 0)
            Else
                Dim ids() As String
                ids = Customer.Split("$")
                objSQLDA.SelectCommand.Parameters.AddWithValue("@Customer", ids(0))
                objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", ids(1))
            End If
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetSalesReturnsbyAgency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, Type As String, FromDate As String, ToDate As String, Agency As String, Item As String, Customer As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_SalesByAgency_V2", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", Uid)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Type", Type)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Agency", Agency)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Item", Item)
            If Customer = "0" Then
                objSQLDA.SelectCommand.Parameters.AddWithValue("@Customer", 0)
                objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", 0)
            Else
                Dim ids() As String
                ids = Customer.Split("$")
                objSQLDA.SelectCommand.Parameters.AddWithValue("@Customer", ids(0))
                objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", ids(1))
            End If
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Function GetRevenueDispersion(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, FromDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_RevenueDispersion", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", Uid)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Year", CDate(FromDate).Year)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Month", CDate(FromDate).Month)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetSalesReturnsbyCustomer(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, Type As String, FromDate As String, ToDate As String, Agency As String, Item As String, Customer As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_SalesByCustomer_V2", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", Uid)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Type", Type)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Agency", Agency)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Item", Item)
            If Customer = "0" Then
                objSQLDA.SelectCommand.Parameters.AddWithValue("@Customer", 0)
                objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", 0)
            Else
                Dim ids() As String
                ids = Customer.Split("$")
                objSQLDA.SelectCommand.Parameters.AddWithValue("@Customer", ids(0))
                objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", ids(1))
            End If
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetSalesReturnsbySKU(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, Type As String, FromDate As String, ToDate As String, Agency As String, Item As String, Customer As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_SalesBySKU_V2", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", Uid)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Type", Type)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Agency", Agency)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Item", Item)
            If Customer = "0" Then
                objSQLDA.SelectCommand.Parameters.AddWithValue("@Customer", 0)
                objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", 0)
            Else
                Dim ids() As String
                ids = Customer.Split("$")
                objSQLDA.SelectCommand.Parameters.AddWithValue("@Customer", ids(0))
                objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", ids(1))
            End If
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetSalesReturnsbyVan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, Type As String, FromDate As String, ToDate As String, Agency As String, Item As String, Customer As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_SalesByVan_V2", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", Uid)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Type", Type)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Agency", Agency)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Item", Item)
            If Customer = "0" Then
                objSQLDA.SelectCommand.Parameters.AddWithValue("@Customer", 0)
                objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", 0)
            Else
                Dim ids() As String
                ids = Customer.Split("$")
                objSQLDA.SelectCommand.Parameters.AddWithValue("@Customer", ids(0))
                objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", ids(1))
            End If
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetFSRReturn(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, FromDate As String, ToDate As String, Type As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_DailyFSR_Returns", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", Uid)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Type", Type)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetDailySalesReport_Return(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, FromDate As String, ToDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_DailySalesReport_Returns", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", Uid)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetPDCReceivables(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_PDC_Receivables", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", Uid)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function GetHeldPDC(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, CollectionRefNo As String, FromDate As String, ToDate As String, Uid As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_HeldPDC", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CollRefNo", CollectionRefNo)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", Uid)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetVanActivity(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, FromDate As String, ToDate As String, Uid As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_VanActivity", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", Uid)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetTransactionStatus(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Status As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String, Uid As String, CustomerId As String, SiteId As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_TransactionStatus", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", Uid)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Status", Status)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CustomerID", CustomerId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteUseID", SiteId)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetPaymentReceivedSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PaymentType As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String, Uid As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_PaymentReceivedSummary", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", Uid)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@PaymentType", PaymentType)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function GetMonthlyVisits(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchParams As String, ByVal OrgId As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_MonthlyCustomerVisits", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@SearchParams", SearchParams)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgId", OrgId)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetVanSalesbyCustomer(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal uid As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_SalesByCustomerType", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", uid)


            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetDivisionCollection(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, FromDate As String, ToDate As String, Uid As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_DeleyedCollectionByOrg", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ORGID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", Uid)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetReceivablesbyMonth(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal uid As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_Market_Receivables_V2", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", uid)


            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetVanSalesbyMonth(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal uid As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_MonthlySalesByVan", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", uid)


            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetPDCRecievablesbyMonth(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal uid As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_PDC_Receivables_V2", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", uid)


            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetDelayedCustomerCount(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal uid As String, ByVal OrgId As String, FromDate As String, ToDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_DelayedCustomerCountByOrg", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", uid)


            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function GetAuditMissedVans(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SurveyID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_AuditMissedVan", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@SurveyID", SurveyID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@orgID", OrgId)


            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetVanTransactions(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_TransactionsSummary", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@FDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@TDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)


            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetFSRCollection(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal uid As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_FSRCollection", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", uid)


            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetOrderListing(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchParams As String, ByVal OrgId As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_LoadOrderList", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@SearchParams", SearchParams)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgId", OrgId)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Function GetVanUnload(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Item As String, FromDate As String, Todate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_VanUnload", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@itemid", Item)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", Todate)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Function GetVanload(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Item As String, FromDate As String, Todate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_VanLoad", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@itemid", Item)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", Todate)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetVanStockSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, uid As String, Item As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("rep_GetVanStockSummary", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@itemid", Item)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function GetCustomerVisitsForEOT(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchParams As String, ByVal OrgId As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_CustomerVisitsForEOT", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@SearchParams", SearchParams)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgId", OrgId)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetReturns(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchParams As String, ByVal OrgId As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_LoadReturnList", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@SearchParams", SearchParams)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgId", OrgId)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetDistributionCheck(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchParams As String, ByVal OrgId As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_LoadDisCheckList", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@SearchParams", SearchParams)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgId", OrgId)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "2784069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetPriceListing(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchParams As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_LoadPriceList", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@SearchParams", SearchParams)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetProducts(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchParams As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_LoadProductList", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@SearchParams", SearchParams)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetCollectionListing(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchParams As String, ByVal OrgId As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_LoadCollectionList", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@SearchParams", SearchParams)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgId", OrgId)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetCurrency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Dim Currency As String = ""
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Select top 1  O.Currency_Code ,Decimal_Digits from dbo.TBL_Org_CTL_DTL O inner join TBL_Currency C on O.Currency_Code=C.Currency_Code where MAS_Org_ID=@OrgId", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text

            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgId", OrgId)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetCurrencyfromVan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal VanList As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Dim Currency As String = ""
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Select top 1  O.Currency_Code ,Decimal_Digits from dbo.TBL_Org_CTL_DTL O  where SalesRep_ID in(Select item from dbo.SplitQuotedString('" & VanList & "'))", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text

            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgId", VanList)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function SMTeamPerformance(ByRef Err_No As Long, ByRef Err_Desc As String, OID As String, ByVal SMID As String, ByVal MonthYear As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "Rep_SMTeamPerformance"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@OID", OID)
            objSQLCmd.Parameters.AddWithValue("@SMID", SMID)
            objSQLCmd.Parameters.AddWithValue("@MonthYear", MonthYear)

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "23416"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetSalesManagerByOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable

        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT User_ID AS SalesManagerID,UserName as SalesManagerName FROm TBL_user WHERE Org_HE_ID=@OrgID AND Is_SS='M'", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.Fill(dt)

            Dim r As DataRow = dt.NewRow
            r(0) = "0"
            r(1) = "--Select--"
            dt.Rows.InsertAt(r, 0)

        Catch ex As Exception
            Err_No = "24762"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetAllOrgVan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, UID As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Get_ListOfAllOrgVans", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", UID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SalesOrg", OrgId)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24762"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function GetAllOrgAgency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, UID As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Get_ListOfAllOrgAgency", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", UID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SalesOrg", OrgId)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24762"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetAllOrgBrand(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, UID As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Get_ListOfBrandByOrg", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", UID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SalesOrg", OrgId)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24762"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetOrgSKU(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, search As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            'If search = "" Then
            '    objSQLDA = New SqlDataAdapter("SELECT   DISTINCT   B.Inventory_item_ID AS ItemCode, B.Item_Code +'-'+ B.Description AS Description FROM     TBL_Product AS B WHERE Organization_id=@OrgID  Order by Description", objSQLConn)
            'Else
            objSQLDA = New SqlDataAdapter("SELECT   DISTINCT   B.Inventory_item_ID AS ItemCode, B.Item_Code +'-'+ B.Description AS Description FROM     TBL_Product AS B WHERE Organization_id=@OrgID and  (Item_Code LIKE '%' + @ItemCode + '%' OR  B.Description LIKE '%' + @ItemCode + '%')  Order by Description ", objSQLConn)
            '  End If

            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.Add("@ItemCode", SqlDbType.VarChar, 100).Value = search
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24762"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetAllOrgSKU(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, UID As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Get_ListOfSKUByOrg", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", UID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SalesOrg", OrgId)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24762"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetManagersByOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetMgrsByOrg", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@ORGID", OrgId)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24769"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function GetTargetvsSalesSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, VanList As String, FMonth As DateTime, Mode As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_TargetvsSalesByVan", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure


            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@VanList", VanList)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FMonth", FMonth)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Mode", "Summary")

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24769"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function GetBestSellersSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, VanList As String, FMonth As DateTime, TMonth As DateTime, Mode As String, ChartMode As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_BestSellers", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure


            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@VanList", VanList)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FMonth", FMonth)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@TMonth", TMonth)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Mode", "Summary")
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ChartMode", ChartMode)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24769"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetSalesDist(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetSalesDist", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "247269"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function GetDelayedCollectionByVan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgStr As String, ByVal MgrStr As String, LocStr As String, SDate As String, EDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_LoadDelayedCollectionByVan", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgStr", OrgStr)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@MgrStr", IIf(String.IsNullOrEmpty(MgrStr), "-1", MgrStr))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@LocStr", IIf(String.IsNullOrEmpty(LocStr), "-1", LocStr))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Sdate", SDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Edate", EDate)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24121069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function GetMBRByAgency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgStr As String, ByVal VanStr As String, AgencyStr As String, SDate As String, EDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_MonthlyBusinessReport", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgStr)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@VanList", IIf(String.IsNullOrEmpty(VanStr), "-1", VanStr))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Agency", IIf(String.IsNullOrEmpty(AgencyStr), "-1", AgencyStr))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FMonth", SDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@TMonth", EDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Mode", "Agency Details")
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24121069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function


    Public Function GetMonthlyDistribution(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgStr As String, ByVal VanStr As String, AgencyStr As String, SDate As String, EDate As String, GroupBy As String, Brand As String, SKU As String, ChartMode As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_MonthlyDistribution", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgStr)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@VanList", IIf(String.IsNullOrEmpty(VanStr), "-1", VanStr))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Agency", IIf(String.IsNullOrEmpty(AgencyStr), "-1", AgencyStr))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Brand", IIf(String.IsNullOrEmpty(Brand), "-1", Brand))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SKUList", IIf(String.IsNullOrEmpty(SKU), "-1", SKU))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FMonth", SDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@TMonth", EDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Mode", "Distribution Details")
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ViewBy", GroupBy)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ChartMode", ChartMode)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24121069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function GetMBRSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgStr As String, ByVal VanStr As String, AgencyStr As String, SDate As String, EDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_MonthlyBusinessSummary", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgStr)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@VanList", IIf(String.IsNullOrEmpty(VanStr), "-1", VanStr))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Agency", IIf(String.IsNullOrEmpty(AgencyStr), "-1", AgencyStr))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FMonth", SDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@TMonth", EDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Mode", "Grid")
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "20121069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetReceivables(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, Year As String, Month As String, UserID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetCurrentMonth_Receivables", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Month", Month)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Yr", Year)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", UserID)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24121069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetCustomerfromOrgtext(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, Optional text As String = "") As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "SELECT (LTRIM(STR(Customer_ID)) + '$' + LTRIM(STR(Site_Use_ID))) as CustomerID,'['+IsNULL(Customer_No,'N/A')+']-'+IsNULL(Customer_Name,'N/A') as Customer from dbo.app_GetOrgCustomers ('" & OrgID & "')"
            If Text <> "" Then
                QueryString = QueryString & " where (Customer_no LIKE '%' + @txt + '%' OR Customer_no +'-'+ Customer_name LIKE '%' + @txt + '%')"
            End If
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            If text <> "" Then
                objSQLCmd.Parameters.Add("@txt", SqlDbType.VarChar, 100).Value = text
            End If
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetCustomerfromOrgtext = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetItemFromAgency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_id As String, Agency As String, Optional text As String = "") As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("Select * from( select distinct  Inventory_Item_ID,item_code +'-'+ Description as Description,cast(Inventory_Item_ID as varchar)+'$'+item_code as item_code,item_code as itemNo from TBL_Product where Organization_ID='" & Org_id & "'")
            If Agency <> "0" Then
                QueryString = QueryString & " and (Agency in(select item from dbo.SplitQuotedString('" + Agency + "'))"
                If Agency.Contains("N/A") Then
                    QueryString = QueryString & " Or Agency is null"
                End If
                QueryString = QueryString & ")"
            End If
            If text <> "" Then
                QueryString = QueryString & " and (Item_Code LIKE '%' + @ItemCode + '%' OR Item_Code +'-'+ Description LIKE '%' + @ItemCode + '%')"
            End If
            QueryString = QueryString & " ) as X order by Description "
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            If text <> "" Then
                objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 100).Value = text
            End If
            Dim MsgDs As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs)

            GetItemFromAgency = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74061"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function

    Public Function GetItemFromAgencyAndUOM(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_id As String, Agency As String, UOM As String, Optional text As String = "") As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("Select * from( select distinct  Inventory_Item_ID,item_code +'-'+ Description as Description,cast(Inventory_Item_ID as varchar)+'$'+item_code as item_code,item_code as itemNo from TBL_Product where Organization_ID='" & Org_id & "'")
            If Agency <> "0" Then
                QueryString = QueryString & " and (Agency in(select item from dbo.SplitQuotedString('" + Agency + "'))"
                If Agency.Contains("N/A") Then
                    QueryString = QueryString & " Or Agency is null"
                End If
                QueryString = QueryString & ")"
            End If
          
            If text <> "" Then
                QueryString = QueryString & " and (Item_Code LIKE '%' + @ItemCode + '%' OR Item_Code +'-'+ Description LIKE '%' + @ItemCode + '%')"
            End If
            QueryString = QueryString & " ) as X order by Description "
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            If text <> "" Then
                objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 100).Value = text
            End If
            Dim MsgDs As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs)

            GetItemFromAgencyAndUOM = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74061"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function

    Public Function GetAllItemsbyOrg(ByRef Err_No As Long, ByRef Err_Desc As String, Org_id As String, Optional text As String = "") As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("Select * from( select distinct  Inventory_Item_ID,item_code +'-'+ Description as Description,cast(Inventory_Item_ID as varchar)+'$'+item_code as item_code,item_code as itemNo from TBL_Product where Organization_ID='" & Org_id & "'")
            If text <> "" Then
                QueryString = QueryString & " and (Item_Code LIKE '%' + @ItemCode + '%' OR Item_Code +'-'+ Description LIKE '%' + @ItemCode + '%')"
            End If
            QueryString = QueryString & " ) as X order by Description "
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            If text <> "" Then
                objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 100).Value = text
            End If
            Dim MsgDs As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs)

            GetAllItemsbyOrg = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74761"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function


    Public Function GetAllItems(ByRef Err_No As Long, ByRef Err_Desc As String, Optional text As String = "") As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("Select * from( select distinct  Inventory_Item_ID,item_code +'-'+ Description as Description,cast(Inventory_Item_ID as varchar)+'$'+item_code as item_code,item_code as itemNo from TBL_Product ")
            If text <> "" Then
                QueryString = QueryString & " Where (Item_Code LIKE '%' + @ItemCode + '%' OR Item_Code +'-'+ Description LIKE '%' + @ItemCode + '%')"
            End If
            QueryString = QueryString & " ) as X order by Description "
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            If text <> "" Then
                objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 100).Value = text
            End If
            Dim MsgDs As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs)

            GetAllItems = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74761"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function

    Public Function GetItemsByInvetory(ByRef Err_No As Long, ByRef Err_Desc As String, Optional text As String = "") As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("Select * from( select distinct  Inventory_Item_ID,item_code +'-'+ Description as Description,cast(Inventory_Item_ID as varchar)+'$'+item_code as item_code,item_code as itemNo from TBL_Product ")
            If text <> "" Then
                QueryString = QueryString & " Where Inventory_Item_ID = @ItemCode"
            End If
            QueryString = QueryString & " ) as X order by Description "
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            If text <> "" Then
                objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 100).Value = text
            End If
            Dim MsgDs As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs)

            GetItemsByInvetory = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74761"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function

    Public Function GetAgency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_id As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("Select distinct isnull(Agency,'N/A') as Agency From TBL_Product where Organization_ID='" & Org_id & "' order by Agency")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs)

            GetAgency = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74061"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function

    Public Function GetSKU(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_id As String, Agency As String, Brand As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("Select * from( select distinct  Inventory_Item_ID,item_code +'-'+ Description as Description,cast(Inventory_Item_ID as varchar)+'$'+item_code as item_code from TBL_Product where Organization_ID='" & Org_id & "'")
            If Agency <> "0" Then
                QueryString = QueryString & " and (Agency in(select item from dbo.SplitQuotedString('" + Agency + "'))"
                If Agency.Contains("N/A") Then
                    QueryString = QueryString & " Or Agency is null"
                End If
                QueryString = QueryString & ")"
            End If
            If Brand <> "0" Then
                QueryString = QueryString & " and (Brand_Code in(select item from dbo.SplitQuotedString('" + Brand + "'))"
                If Brand.Contains("N/A") Then
                    QueryString = QueryString & " Or Brand_Code is null"
                End If
                QueryString = QueryString & ")"
            End If
            QueryString = QueryString & " ) as X order by Description "
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs)

            GetSKU = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74061"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    Public Function GetBrandList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_id As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("Select distinct isnull(Brand_Code,'N/A') as Brand_Code  From TBL_Product where Organization_ID='" & Org_id & "'")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs)

            GetBrandList = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74061"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    Public Function GetHolidayListing(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Code As String, ByVal FromDate As String, ByVal ToDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_HolidayListing", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@Code", Code)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)


            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24121069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetBankListing(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Code As String, ByVal name As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_BankListing", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@Code", Code)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Name", name)


            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24121069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function GetCollectionbyVanForMonth(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal param1 As String, ByVal param2 As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_VanCollections_DashBoard", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgIds", param1)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", CDate(param2).ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(param2))).ToString("dd-MMM-yyyy"))


            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24121069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetSalesbyAgency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal param1 As String, ByVal param2 As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_DashBoard_SalesByAgency", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@VanIDs", param1)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", CDate(param2).ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(param2))).ToString("dd-MMM-yyyy"))


            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24121069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function GetCashVanAudit(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OID As String, ByVal SID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_CashVanAuditReport", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@VanID", SID)



            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24121069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetSalesbyVanForMonth(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal param1 As String, ByVal param2 As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_DashBoard_SalesByVan", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgIDs", param1)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", CDate(param2).ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(param2))).ToString("dd-MMM-yyyy"))


            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24121069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetDelayedCollectionBySupervisor(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String, ByVal param5 As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            Dim tdate As String = ""
            param2 = "01/" & param2
            tdate = "01/" & param3
            tdate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(param3))).ToString("dd/MMM/yyyy") & " 23:59:59"

            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_DeleyedCollectionBySuperVisor", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", param1)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Fromdate", CDate(param2).ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", CDate(tdate).ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", param4)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Loc", param5)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24121069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function GetDelayedCollectionByDiv(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String, ByVal param5 As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            Dim tdate As String = ""
            param2 = "01/" & param2
            tdate = "01/" & param3
            tdate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(param3))).ToString("dd/MMM/yyyy") & " 23:59:59"

            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_DeleyedCollectionByDivision", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", param1)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Fromdate", CDate(param2).ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", CDate(tdate).ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", param4)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Loc", param5)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24121069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Function GetSKUDispersion(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, FromDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_SKUDispersion", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", Uid)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Year", CDate(FromDate).Year)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Month", CDate(FromDate).Month)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Function GetVanSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Fromdate As String, Todate As String, Perc As String, Item As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("usp_GetSummaryStock", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", Todate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Per", Val(Perc))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ItemID", Item)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Function GetAgeing(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, CustId As String, SiteID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_Ageing", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@orgId", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CustID", CustId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", SiteID)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Function GetZerobilledOutlets(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Fromdate As String, Todate As String, uid As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_ZerobilledOutlets", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", uid)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Todate", Todate)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Function GetEOTSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Fromdate As String, Todate As String, uid As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_EOTSummary", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", uid)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Todate", Todate)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Function GetEOTDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Fromdate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_EOTDetailed", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate)

            objSQLDA.Fill(dt)
            Dim i As String
        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetGoodsReceipt(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, Van As String, ApprovalCode As String, ApprovedBy As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GoodsReceipt", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", Van)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ApprovalCode", ApprovalCode)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ApprovedBy", ApprovedBy)


            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function GetStockRequisition(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, Van As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_StockRequisition", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@Date", Fromdate.ToString("dd-MM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate.ToString("dd-MM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FSRID", Van)



            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function


    Public Function GetMonthlyVanLoad(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, Van As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("rep_MonthlyVanload", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate.ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate.ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", Van)


            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Function GetVanUnload(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal VisitID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetVisitDetails", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@RowID", VisitID)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "2406779"
            Err_Desc = ex.Message
            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetWeeklyReturnSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, FromDate As String, ToDate As String, Customer As String, SiteID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_WeeklyReturns", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@Fromdate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", Uid)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Customer", Customer)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", SiteID)


            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetPlannedCoverage(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_PlannedCoverageByFSR_V2", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@Dat", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Dat1", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function GetCoverageReport(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_CoverageReport", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@Month", Now.Month)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Year", Now.Year)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", Uid)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetOverallCoverage(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_OverAllCoverageByFSR", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@Dat", CDate(FromDate).ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Dat1", CDate(ToDate).ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
<<<<<<< .mine
    Public Function GetFSRVisitTracking(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, VisitDate As DateTime, Van As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_FSRVisitTracking", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure


            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", Van)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@VisitDate", VisitDate)


            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "247069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function GetFuelExpenses(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, StartDate As DateTime, EndDate As DateTime, Van As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("rep_FuelExpense", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure


            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FsrID", Van)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Fromdate", StartDate.ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", EndDate.ToString("dd-MMM-yyyy"))

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "247069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
=======
    Public Function GetLogReport(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_LogSummary", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", CDate(FromDate).ToString("MM-dd-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", CDate(ToDate).ToString("MM-dd-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function GetLogReportSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_LogSummary_TotalAmt", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", CDate(FromDate).ToString("MM-dd-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", CDate(ToDate).ToString("MM-dd-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
>>>>>>> .r230
End Class
