Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Data
Imports log4net
Public Class DAL_Reports
    Private _objDB As DatabaseConnection
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private _getAssetsViewHistory As DataTable

    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub

    Property GetAssetsViewHistory(Err_No As Long, Err_Desc As String, OrgId As String, AssetID As String) As DataTable
        Get
            Return _getAssetsViewHistory
        End Get
        Set(value As DataTable)
            _getAssetsViewHistory = value
        End Set
    End Property

    Public Function GetUserChangeLog(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchParams As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_UserLogHistory", objSQLConn)
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
    Public Function GetVanPerformance(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, Van As String, SalesDistrictID As String, DisplayMode As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_VanPerformance", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", Van)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SalesDistrictID", SalesDistrictID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@DisplayMode", DisplayMode)
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

    Public Function GetVanPerformance_ASR(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As String, ToDate As String, Van As String, SalesDistrictID As String, UID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_VanPerformance_asr", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", Van)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SalesDistrictID", SalesDistrictID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UId", UID)

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


    Public Function GetCustomerVisits(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Qry As String, Van As String, UID As String, Fromdate As String, Todate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_CustomerVisits", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgId", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SearchParams", Qry)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", Van)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", UID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", Todate)

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

    Public Function GetDistributionCheckList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Qry As String, van As String, UID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_LoadDisCheckList", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgId", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SearchParams", Qry)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SPID", van)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", UID)

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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
    Public Function GetCustomerVisitsSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, SID As String, Uid As String, CustID As String, CustType As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_CustomerVisit_TotalAmt", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", Uid)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CustID", CustID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CustType", CustType)

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

    Public Function GetSalesManWiseAgencyWiseSalesAndReturns(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, FromDate As String, ToDate As String, Agency As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetPrincipalFSRWiseSalesReturns", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Agency", Agency)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Salesrep_ID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", Uid)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Fromdate", FromDate)
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
    Public Function GetMultiTrxFOC(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, FromDate As String, ToDate As String, Customer As String, SiteId As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetMutltTrxBonus", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CustID", Customer)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", SiteId)
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

    Public Function GetMissedVisits(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, FromDate As String, ToDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("usp_RepGetMissedVisits", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@orgID", OrgId)
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
    Public Function GetVanActivity(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, FromDate As String, ToDate As String, Uid As String, VanID As String, SyncType As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_VanActivity", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@VanID", VanID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SyncType", SyncType)
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
    Public Function GetFocFeedeback(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, FromDate As String, ToDate As String, CustomerId As String, SiteId As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_FocFeedeback", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Todate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgId", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CustID", CustomerId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", SiteId)

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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
    Function GetCollectionDiscount(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String, Uid As String, ByVal CustID As String, ByVal SiteID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_CollectionDiscount", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", Uid)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CustID", CustID)
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
    Public Function GetJPAdherence(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal uid As String, ByVal OrgId As String, SID As String, Month As String, year As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_JourneyAdheranceNew", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Month", Month)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Year", Year)
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
    Public Function GetOrderListing(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchParams As String, ByVal OrgId As String, FromDate As String, Todate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_LoadOrderList", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SearchParams", SearchParams)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgId", OrgId)
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
    Public Function GetDailyStockReport(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Item As String, Fromdate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("rep_GetDailyStock", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@itemid", Item)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate)


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
    Public Function GetDailyStockReconciliation(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Item As String, Fromdate As String, Todate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("usp_RepDailyStockSummary", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@itemid", Item)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate)
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
    Public Function GetVanStockSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, uid As String, Item As String, Agency As String, Brand As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("rep_GetVanStockSummary", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@itemid", Item)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Agency", Agency)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Brand", Brand)

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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
    Public Function GetReturns(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchParams As String, ByVal OrgId As String, FromDate As String, Todate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_LoadReturnList", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SearchParams", SearchParams)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgId", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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

    Public Function SMVanLog(ByRef Err_No As Long, ByRef Err_Desc As String, OID As String, ByVal SMID As String, ByVal MonthYear As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "Rep_SMTop5VanSales"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
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

    Public Function SMTop10Customers(ByRef Err_No As Long, ByRef Err_Desc As String, OID As String, ByVal SMID As String, ByVal MonthYear As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "Rep_SMTop10CustomerSales"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@OID", OID)
            objSQLCmd.Parameters.AddWithValue("@SMID", SMID)
            objSQLCmd.Parameters.AddWithValue("@MonthYear", MonthYear)
            objSQLCmd.Parameters.AddWithValue("@Mode", "All")
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
    Public Function SMTop10CustomersTop10(ByRef Err_No As Long, ByRef Err_Desc As String, OID As String, ByVal SMID As String, ByVal MonthYear As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "Rep_SMTop10CustomerSales"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@OID", OID)
            objSQLCmd.Parameters.AddWithValue("@SMID", SMID)
            objSQLCmd.Parameters.AddWithValue("@MonthYear", MonthYear)
            objSQLCmd.Parameters.AddWithValue("@Mode", "Top 10")
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
    Public Function SMLast3MonthsSalesgrowth(ByRef Err_No As Long, ByRef Err_Desc As String, OID As String, ByVal SMID As String, ByVal MonthYear As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "Rep_SMLast3MonthsSalesgrowth"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
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
    Public Function SMTeamPerformance(ByRef Err_No As Long, ByRef Err_Desc As String, OID As String, ByVal SMID As String, ByVal MonthYear As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "Rep_SMTeamPerformance"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.Fill(dt)

            Dim r As DataRow = dt.NewRow
            r(0) = "0"
            r(1) = "Select Sales Manager"
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
    Public Function GetAllOrgBrand(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, UID As Integer, AgencyList As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Get_ListOfBrandByOrg", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", UID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SalesOrg", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@AgencyList", AgencyList)
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
    Public Function GetOrgSKU(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, search As String, BrandList As String) As DataTable
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
            'objSQLDA = New SqlDataAdapter("SELECT   DISTINCT   B.Inventory_item_ID AS ItemCode, B.Item_Code +'-'+ B.Description AS Description FROM     TBL_Product AS B WHERE Organization_id=@OrgID and  (Item_Code LIKE '%' + @ItemCode + '%' OR  B.Description LIKE '%' + @ItemCode + '%')  Order by Description ", objSQLConn)
            objSQLDA = New SqlDataAdapter("app_GetProductListByBrand", objSQLConn)
            '  End If

            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.Add("@ItemCode", SqlDbType.VarChar, 100).Value = search
            objSQLDA.SelectCommand.Parameters.AddWithValue("@BrandList", BrandList)
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
    Public Function GetMerchandisingSurveyCustomer(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QueryStr As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT DISTINCT (LTRIM(STR(A.Customer_ID)) + '$' + LTRIM(STR(A.Site_Use_ID))) as ID, (A.Customer_Name+'-'+ISNULL(A.Location,'N/A')) As Name from TBL_Customer_Ship_Address A,TBL_Survey_Session B where A.Customer_Id=B.Customer_Id AND A.Site_Use_ID=B.Site_Use_ID {0} order by Name ASC", QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "tblCustSurvey")
            GetMerchandisingSurveyCustomer = MsgDs.Tables(0)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74091"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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

    Public Function GetLineCutsByVanSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, VanList As String, FMonth As String, Todate As String, CustomerID As String, SiteUSeID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_LineCutsByVanSummary", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600

            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@VanList", VanList)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FDate", FMonth)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@TDate", Todate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Customer_ID", CustomerID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", SiteUSeID)


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
    Public Function GetBillCutsByVanSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, VanList As String, FMonth As String, TMonth As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_BillCutsByVanSummary", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600

            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@VanList", VanList)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FDate", FMonth)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@TDate", TMonth)

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
    Public Function GetMonthlyFOCByVanSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, VanList As String, FMonth As DateTime) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_MonthlyFOCByVan", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600

            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@VanList", VanList)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FMonth", FMonth)

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

    Public Function GetScroeCardSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, VanList As String, FMonth As DateTime, Mode As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_ScoreCardTargetvsSales", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600

            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@VanList", VanList)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FMonth", FMonth)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Mode", Mode)

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
    Public Function GetScoreCardOutlet(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, VanList As String, FMonth As DateTime, Mode As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_ScoreCardOutletCategory", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600

            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@VanList", VanList)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FMonth", FMonth)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Mode", Mode)

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
    Public Function GetVanKPI(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, VanList As String, FMonth As DateTime, Mode As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_VanKPISummary", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600

            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@VanList", VanList)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FDate", FMonth)
            'objSQLDA.SelectCommand.Parameters.AddWithValue("@Mode", Mode)

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
            objSQLDA.SelectCommand.CommandTimeout = 600

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
            objSQLDA.SelectCommand.CommandTimeout = 600

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
            objSQLDA.SelectCommand.CommandTimeout = 600
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

        Dim targettype As String = "A"
        Dim Procname As String = "Rep_MonthlyBusinessReport_Agency"
        targettype = GetTargetType(Err_No, Err_Desc)
        Try
            objSQLConn = _objDB.GetSQLConnection
            If targettype = "P" Then
                Procname = "Rep_MonthlyBusinessReport_Item"
            ElseIf targettype = "B" Then
                Procname = "Rep_MonthlyBusinessReport_Brand"
            Else
                Procname = "Rep_MonthlyBusinessReport_Agency"
            End If
            objSQLDA = New SqlDataAdapter(Procname, objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 900
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgStr)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@VanList", IIf(String.IsNullOrEmpty(VanStr), "-1", VanStr))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Agency", IIf(String.IsNullOrEmpty(AgencyStr), "-1", AgencyStr))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FMonth", SDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@TMonth", EDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Mode", "AgencyTable")
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

    Public Function GetMonthlyTargetAndSales(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgStr As String, ByVal VanStr As String, AgencyStr As String, SDate As String, EDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter


        Dim Procname As String = "Rep_MonthlyTargetvsSalesByVan"

        Try
            objSQLConn = _objDB.GetSQLConnection

            objSQLDA = New SqlDataAdapter(Procname, objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 900
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgStr)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@VanList", IIf(String.IsNullOrEmpty(VanStr), "-1", VanStr))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@AgencyList", IIf(String.IsNullOrEmpty(AgencyStr), "-1", AgencyStr))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromMonth", SDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToMonth", EDate)

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
    Public Function GetMBRTargetvsSalesPerMonths(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgStr As String, ByVal VanStr As String, SDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter

        Dim targettype As String = "A"
        Dim Procname As String = "Rep_TargetvsSalesByVan"
        targettype = GetTargetType(Err_No, Err_Desc)
        Try

            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter(Procname, objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 900
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgStr)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@VanList", IIf(String.IsNullOrEmpty(VanStr), "-1", VanStr))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FMonth", SDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Mode", "Page")

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
    Public Function GetMBRTargetvsSalesByMonths(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgStr As String, ByVal VanStr As String, AgencyStr As String, SDate As String, EDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter

        Dim targettype As String = "A"
        Dim Procname As String = "Rep_MonthlyBusinessReport_TVA"
        targettype = GetTargetType(Err_No, Err_Desc)
        Try
            If targettype = "P" Then
                Procname = "Rep_MonthlyBusinessReport_Item"
            ElseIf targettype = "B" Then
                Procname = "Rep_MonthlyBusinessReport_Brand"
            Else
                Procname = "Rep_MonthlyBusinessReport_TVA"
            End If
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter(Procname, objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 900
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgStr)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@VanList", IIf(String.IsNullOrEmpty(VanStr), "-1", VanStr))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Agency", IIf(String.IsNullOrEmpty(AgencyStr), "-1", AgencyStr))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FMonth", SDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@TMonth", EDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Mode", "Chart")

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
    Public Function GetMBRByVan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgStr As String, ByVal VanStr As String, AgencyStr As String, SDate As String, EDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_MonthlyBusinessReportCopy", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 900
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
    Public Function GetMBRByAgencySummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgStr As String, ByVal VanStr As String, AgencyStr As String, SDate As String, EDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_MonthlyBusinessReportCopy", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 900
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgStr)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@VanList", IIf(String.IsNullOrEmpty(VanStr), "-1", VanStr))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Agency", IIf(String.IsNullOrEmpty(AgencyStr), "-1", AgencyStr))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FMonth", SDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@TMonth", EDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Mode", "Agency Summary")
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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

    Public Function GetMBRSummaryByVan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgStr As String, ByVal VanStr As String, AgencyStr As String, SDate As String, EDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_MonthlyBusinessSummaryCopy", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 900
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

    Public Function GetMBRSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgStr As String, ByVal VanStr As String, AgencyStr As String, SDate As String, EDate As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim dt As New DataSet
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Dim targettype As String = "A"
        Dim Procname As String = "Rep_MonthlyBusinessSummary_Agency"
        targettype = GetTargetType(Err_No, Err_Desc)
        Try
            objSQLConn = _objDB.GetSQLConnection
            If targettype = "P" Then
                Procname = "Rep_MonthlyBusinessSummary_itemCode"
            ElseIf targettype = "B" Then
                Procname = "Rep_MonthlyBusinessSummary_brand"
            Else
                Procname = "Rep_MonthlyBusinessSummary_Agency"
            End If
            objSQLDA = New SqlDataAdapter(Procname, objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 900
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
    Public Function GetReceivables(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, Year As String, Month As String, UserID As String, Van As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetCurrentMonth_Receivables", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Month", Month)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Yr", Year)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", UserID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Van", Van)

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
            Dim QueryString As String
            objSQLConn = _objDB.GetSQLConnection

            QueryString = "SELECT (LTRIM(STR(Customer_ID)) + '$' + LTRIM(STR(Site_Use_ID))) as CustomerID,'['+IsNULL(Customer_No,'N/A')+']-'+IsNULL(Customer_Name,'N/A') as Customer from dbo.app_GetOrgCustomers ('" & OrgID & "')"


            If text <> "" Then
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

    Public Function GetOutletfromOrgtext(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, Optional text As String = "") As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "select *,CONVERT(VARCHAR(25),cast(Customer_ID as varchar) + '$' + cast(Site_Use_ID as varchar)) AS CustomerID ,CONVERT(VARCHAR(200), Customer_No + '-' + Customer_Name) AS Outlet from [app_GetOrgCustomerShipAddress]('" & OrgID & "')"
            If text <> "" Then
                QueryString = QueryString & " where (Customer_no LIKE '%' + @txt + '%' OR Customer_no +'-'+ Customer_name LIKE '%' + @txt + '%')"
            End If
            QueryString = QueryString & " order by Customer_No "
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            If text <> "" Then
                objSQLCmd.Parameters.Add("@txt", SqlDbType.VarChar, 100).Value = text
            End If
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetOutletfromOrgtext = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Function GetItemFromAgencyandBrand(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Agencies As String, Text As String, brandlist As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("usp_RepGetProducts", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600

            objSQLDA.SelectCommand.Parameters.Add("@OrgId", SqlDbType.VarChar, 100).Value = OrgId
            objSQLDA.SelectCommand.Parameters.Add("@Agency", SqlDbType.VarChar, 100).Value = Agencies
            objSQLDA.SelectCommand.Parameters.Add("@brandlist", SqlDbType.VarChar, 100).Value = brandlist
            objSQLDA.SelectCommand.Parameters.Add("@ItemCode", SqlDbType.VarChar, 100).Value = Text

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
    Public Function GetItemFromOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String, Optional text As String = "")
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("Select * from( select distinct  Inventory_Item_ID,item_code +'-'+ Description as Description,cast(Inventory_Item_ID as varchar)+'$'+item_code as item_code,item_code as itemNo from TBL_Product where Organization_ID='" & Org_ID & "'")
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

            GetItemFromOrg = MsgDs
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "7421158"
            Err_Desc = ex.Message
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
    Public Function GetAllActiveItemsbyOrg(ByRef Err_No As Long, ByRef Err_Desc As String, Org_id As String, Optional text As String = "") As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("Select * from( select distinct  Inventory_Item_ID,item_code +'-'+ Description as Description,cast(Inventory_Item_ID as varchar)+'$'+item_code as item_code,item_code as itemNo from TBL_Product where isnull(Is_Active,'Y')='Y' and Organization_ID='" & Org_id & "'")
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

            GetAllActiveItemsbyOrg = MsgDs
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


    Public Function GetAllAgency(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("Select distinct isnull(Agency,'N/A') as Agency From TBL_Product order by Agency")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs)

            GetAllAgency = MsgDs
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
    Public Function GetBrand(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_id As String, Agencystr As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("Select * from (Select distinct isnull(Brand_Code,'N/A') as Brand_Code,Isnull(B.Code_Description,Brand_Code) as Description  From TBL_Product A Left join TBL_App_Codes B on A.Brand_Code=B.Code_Value and B.Code_Type='PROD_BRAND' where Organization_ID=@OrgID ")
            If Agencystr.Trim <> "" Then
                QueryString = QueryString & " and agency in(Select item from dbo.SplitQuotedString(@AgencyList)) "
            End If
            QueryString = QueryString & " ) as X order by Description"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@OrgID", Org_id)
            If Agencystr.Trim <> "" Then
                objSQLCmd.Parameters.Add("@AgencyList", Agencystr)
            End If
            Dim MsgDs As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs)

            GetBrand = MsgDs
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
    Public Function GetDelayedCollectionByLocation(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String, ByVal param5 As String) As Object


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
            objSQLDA = New SqlDataAdapter("Rep_DeleyedCollectionBySalesDistrict", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
    Public Function GetOffTakeSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, Van As String, ItemId As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("rep_MonthlyOffTakeSummary", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate.ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate.ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", Van)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ItemID", ItemId)


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
    Public Function GetMonthlyWastageReport(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, Van As String, ItemId As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("rep_MonthlyDamagedExpired", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate.ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate.ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", Van)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ItemID", ItemId)


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
    Public Function GetMonthlySalesVsReturn(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, Van As String, ItemId As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("rep_MonthlySold", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate.ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate.ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", Van)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ItemID", ItemId)


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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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

    Public Function GetSalesbyVanAgency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, UID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_SalesByVanAgency", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", UID)

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
    Public Function GetSalesbySKU(ByRef Err_No As Long, ByRef Err_Desc As String, Fromdate As String, Todate As String, ByVal OrgId As String, InvID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetSalesSKUWise", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", Todate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SKUID", InvID)

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
    Public Function GetSalesbyVanAgencyQty(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, UID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_SalesByAgencyQty", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", UID)

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
    Public Function GetProductiveCalls(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, UID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_ProductiveCalls_v2", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600

            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", UID)

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
    Public Function GetProductivityperMSL(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, UID As String, Invid As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_ProductivityPerMSLLine", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600

            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", UID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@InvID", Invid)

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
    Public Function GetDistributionforMSL(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, UID As String, Fromdate As String, Todate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_DistibutionbyMSL_v3", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600

            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", UID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@fromdate", Fromdate)
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
            objSQLDA.SelectCommand.CommandTimeout = 600
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
    Public Function GetLogReport(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String, CustID As String, SiteID As String, DocType As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_LogSummary", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", CDate(FromDate).ToString("MM-dd-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", CDate(ToDate).ToString("MM-dd-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CustID", CustID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", SiteID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@DocType", DocType)
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

    Public Function GetLogReportSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String, CustID As String, SiteID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_LogSummary_TotalAmt", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", CDate(FromDate).ToString("MM-dd-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", CDate(ToDate).ToString("MM-dd-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CustID", CustID)
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

            objSQLDA.SelectCommand.CommandTimeout = 600
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

            objSQLDA.SelectCommand.CommandTimeout = 600
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
    Function ArtilcleMovement(ByRef Err_No As Long, ByRef Err_Desc As String, Orgid As String, SID As String, UID As Integer, FromDate As String, Todate As String, DocType As String, InvID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_ArticleMovements", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", Todate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", Orgid)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@DocType", DocType)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", UID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@InvID", InvID)

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
    Public Function GetCustomerSurvey(ByRef Err_No As Long, ByRef Err_Desc As String, SearchParam As String, Orgid As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_LoadCustomerSurveyDetails", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SearchParams", SearchParam)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Orgid", Orgid)

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
    Public Function GetAuditSurvey(ByRef Err_No As Long, ByRef Err_Desc As String, SearchParam As String, Orgid As String, Status As String, SalesRepid As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_LoadAuditSurveyList_v2", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SearchParams", SearchParam)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Orgid", Orgid)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Status", Status)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SalesRep_ID", SalesRepid)

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

    Public Function GetDeviceSynLog(ByRef Err_No As Long, ByRef Err_Desc As String, SID As String, FromDate As String, ToDate As String, Stype As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_DeviceDBSyncLog", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", CDate(FromDate).ToString("MM-dd-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", CDate(ToDate).ToString("MM-dd-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UserName", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Sync_Type", Stype)

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
    Public Function GetSynExceptionReport(ByRef Err_No As Long, ByRef Err_Desc As String, Hours As String, SID As String, SyncType As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_LastDeviceDBSyncLog", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SyncType", SyncType)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UserName", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Hours", Hours)

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
    Public Function GetApprovalCodeusage(ByRef Err_No As Long, ByRef Err_Desc As String, OID As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_ApprovalCodesUsage", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Dat", CDate(FromDate).ToString("MM-dd-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Dat1", CDate(ToDate).ToString("MM-dd-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OID)

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
    Public Function GetVanProcessTransactions(ByRef Err_No As Long, ByRef Err_Desc As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_VanProcessTransaction", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Dat", CDate(FromDate).ToString("MM-dd-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Dat1", CDate(ToDate).ToString("MM-dd-yyyy"))
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

    Public Function GetOverallCoverage_Visited(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_Coverage_Det_Visited", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", CDate(FromDate).ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Todate", CDate(ToDate).ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
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
    Public Function GetOverallCoverage_Planned(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_Coverage_Det_planned", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", CDate(FromDate).ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Todate", CDate(ToDate).ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
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
    Public Function GetOverallCoverage_NotVisited(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_Coverage_Det_NotVisited", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", CDate(FromDate).ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Todate", CDate(ToDate).ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
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
    Public Function GetOverallCoverage_ZeroBilled(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_Coverage_Det_ZeroBilled", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", CDate(FromDate).ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Todate", CDate(ToDate).ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
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
    Public Function GetProductAvailablity(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, ByVal Van As String, ByVal UID As String, ByVal Agency As String, ByVal Item As String, ByVal FromDate As String, ByVal ToDate As String, ByVal Type As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_ProductAvailibility", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", Van)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Agency", Agency)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@InvID", Item)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Availability", Type)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", UID)
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
    Public Function GetSKUWiseSalesReturns(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As String, ToDate As String, Van As String, Agency As String, CustID As String, InID As String, UID As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetSKUwiseSalesReturnValue", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Todate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FSRID", Van)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Agency", Agency)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@InID", InID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", CustID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", UID)

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
    Public Function GetSalesAndDsicount(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As String, ToDate As String, Van As String, Uid As String, CustID As String, SiteId As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_SalesAndDiscount", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Todate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FSRID", Van)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", Uid)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CustID", CustID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", SiteId)

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
    Public Function GetDailyReport(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, Van As String, Uid As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("rep_GetDailyReport", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Start_Date", Fromdate.ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@End_Date", ToDate.ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgId", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FSRID", Van)
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
    Public Function GetAssets(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, OnlyActive As String, CustomerID As String, SiteID As String, AssetType As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetAssetsList_V2", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgId", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OnlyActive", OnlyActive)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CustomerID", CustomerID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", SiteID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@AssetType", AssetType)

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
    Public Function GetOfferDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, Brand As String, Mode As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_OfferDetails", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Dat", Fromdate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Dat1", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Brand", Brand)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Mode", Mode)

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

    Public Function GeAssortmentDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, Brand As String, Mode As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_AssortmentBonusDetails", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Dat", Fromdate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Dat1", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Brand", Brand)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Mode", Mode)

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

    Public Function GetSurveyStatistics(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SurveyID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_RepSurveyStatisticsNew", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SurveyID", SurveyID)

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

    Public Function GetSurveyStatisticsChart(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SurveyID As String, QuesID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_RepSurveyStatisticsNewChart", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SurveyID", SurveyID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@QuestID", QuesID)

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

    Public Function GetDistributionDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, Month As String, Year As String, Cid As String, SiteID As String, Item As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection

            objSQLDA = New SqlDataAdapter("Rep_DistributionCheck", objSQLConn)

            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CID", Cid)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", SiteID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Month", Month)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Year", Year)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", Uid)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Item", Item)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600

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

    Public Function GetSurveyOtherResponses(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SurveyID As String, Type As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            If Type = "N" Then
                objSQLDA = New SqlDataAdapter("Rep_LoadCustomerSurveyDetails", objSQLConn)
                objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            Else
                objSQLDA = New SqlDataAdapter("Rep_LoadAuditSurveyDetails", objSQLConn)
            End If
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SearchParams", SurveyID)

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


    Public Function GetSalesSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, Van As String, UserID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_SalesSummary", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate.ToString("dd-MM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate.ToString("dd-MM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", Van)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", UserID)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "2124069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetSalesPrinciple(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, Agency As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetPrincipalWiseSalesReturns", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate.ToString("dd-MM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate.ToString("dd-MM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Agency", IIf(String.IsNullOrEmpty(Agency), DBNull.Value, Agency))
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "2224069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function GetWareHousePurchase(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Agency As String, FromDate As String, Todate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("usp_RepWareHousePurchase_Hdr", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Date", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", Todate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Org_ID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Agency", IIf(String.IsNullOrEmpty(Agency), 0, Agency))
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "2224069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetERPSyncLog(ByRef Err_No As Long, ByRef Err_Desc As String, FromDate As String, ToDate As String, ERPtable As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_ERPBOSyncLog", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ERPTable", ERPtable)

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
    Public Function GetSalesbyBrand(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String, Brand As String, Mode As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_BrandSummary", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Dat", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Dat1", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Brand", Brand)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Mode", Mode)

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
    Public Function GetSalesReturnsbyClient(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, Type As String, FromDate As String, ToDate As String, Customer As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetCustomerSalesReturns", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SalesRep_ID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", Uid)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Type", Type)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Customer", Customer)

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

    Public Function GetSKUPeriodWiseReturns(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, FromDate As String, Todate As String, SKUID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetSKUWiseReturns", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", Todate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@InID", SKUID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "2224069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetOutletwiseSaleReturn(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, CustID As Integer, SiteID As Integer, UserID As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetOutletwiseSalesReturn", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate.ToString("dd-MM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate.ToString("dd-MM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SiteID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CID", CustID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", userID)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "2724069"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
            objSQLDA = Nothing
        End Try
        Return dt
    End Function
    Public Function GetOutletwiseSKUwiseSaleReturn(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, INID As String, SiteID As String, Agency As String, FSR As String, UID As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetOutletSKUWise", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate.ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate.ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SiteID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@InID", INID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Agency", Agency)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FSRID", FSR)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", UID)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "2724069"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
            objSQLDA = Nothing
        End Try
        Return dt
    End Function
    Public Function GetExcessReturns(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, CustID As Integer, SiteID As Integer, SKUId As Integer, Cutoff As Decimal) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetExcessReturn", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate.ToString("dd-MM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate.ToString("dd-MM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteUSeid", SiteID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CustomerID", CustID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SKUID", SKUId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CutOff", Cutoff)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "2724069"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
            objSQLDA = Nothing
        End Try
        Return dt
    End Function

    Public Function GetOutletSKUwiseReturns(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, CustID As Integer, SiteID As Integer, SKUId As Integer, UID As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetOutletSKUWiseReturnAlert", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate.ToString("dd-MM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate.ToString("dd-MM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SiteID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CID", CustID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@InID", SKUId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", UID)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "2724069"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
            objSQLDA = Nothing
        End Try
        Return dt
    End Function
    Public Function GetPurchaseReport(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, Van As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_PurchaseReport", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ReqFromDate", Fromdate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ReqTodateDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", Van)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "2124069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetTotalSummarySalesReturns(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As String, ToDate As String, Van As String, Agency As String, Customer As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetGrpSummCustomerSalesReturns", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SalesRep_ID", Van)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Agency", Agency)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Customer", Customer)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "21240169"
            Err_Desc = ex.Message

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetSummaryPurchaseSalesReturns(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As String, ToDate As String, Van As String, Agency As String, Type As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("usp_GetSummaryWeeklyMonthly", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", CDate(Fromdate).ToString("dd-MMM-yyyy hh:mm tt"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", CDate(ToDate).ToString("dd-MMM-yyyy hh:mm tt"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", Van)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Agency", Agency)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@type", Type)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "21240169"
            Err_Desc = ex.Message

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function GetMerchandasingSessions(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, VanID As String, SurveyID As String, FromDate As String, Todate As String, CustomerId As String, SiteID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetMerchandasingSurveySessions", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SurveyID", SurveyID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Todate", Todate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SalesRep_ID", VanID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CustID", CustomerId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", SiteID)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "21240169"
            Err_Desc = ex.Message

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetMerchandasingSessionDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SessionID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetMerchandasingSessionDetails", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Survey_Session_ID", SessionID)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "21240169"
            Err_Desc = ex.Message

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetMerchandasingResult(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SessionID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetMerchandasingSurveyResp", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Survey_Session_ID", SessionID)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "21240169"
            Err_Desc = ex.Message

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetBeaconDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetBeaconDetails", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@VisitID", ID)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "21240169"
            Err_Desc = ex.Message

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetAllZeroBilledCustomers(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal VanList As String, Fdate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_DashZeroBilledCustomers", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@MonthYEAR", Fdate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@VanList", VanList)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "21240169"
            Err_Desc = ex.Message

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetTargetType(ByRef Err_No As Long, ByRef Err_Desc As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim Name As String = Nothing

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("Select top 1 Target_Type from TBL_Sales_Target order by Created_At desc")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            Name = Convert.ToString(objSQLCmd.ExecuteScalar())

            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24066"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return Name
    End Function

    Function GetCustomerfromPaymentType(Err_No As Long, Err_Desc As String, OrgId As String, selectedPayment As String, Text As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            Dim QueryString As String
            objSQLConn = _objDB.GetSQLConnection

            If selectedPayment = "0" Then
                QueryString = "SELECT (LTRIM(STR(B.Customer_ID)) + '$' + LTRIM(STR(B.Site_Use_ID))) as CustomerID,'['+IsNULL(B.Customer_No,'N/A')+']-'+IsNULL(B.Customer_Name,'N/A') as Customer from dbo.app_GetOrgCustomers ('" & OrgId & "') A inner join app_GetOrgCustomerShipAddress  ('" & OrgId & "') B on A.Customer_no=B.Customer_no where 1=1"
            Else
                QueryString = "SELECT (LTRIM(STR(B.Customer_ID)) + '$' + LTRIM(STR(B.Site_Use_ID))) as CustomerID,'['+IsNULL(B.Customer_No,'N/A')+']-'+IsNULL(B.Customer_Name,'N/A') as Customer from  dbo.app_GetOrgCustomers ('" & OrgId & "') A inner join app_GetOrgCustomerShipAddress  ('" & OrgId & "') B on A.Customer_no=B.Customer_no  where Cash_Cust='" + selectedPayment + "' "

            End If

            If Text <> "" Then
                QueryString = QueryString & " and (B.Customer_no LIKE '%' + @txt + '%' OR B.Customer_no +'-'+ B.Customer_name LIKE '%' + @txt + '%')"
            End If


            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            If Text <> "" Then
                objSQLCmd.Parameters.Add("@txt", SqlDbType.VarChar, 100).Value = Text
            End If
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetCustomerfromPaymentType = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Function GetWindowServKPI(Err_No As Long, Err_Desc As String, Organization As String, SID As String, Fromdate As String, Todate As String, USerID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try



            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetWindowsCustServiceKPI", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", Organization)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Fromdate", Fromdate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", Todate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", USerID)


            'objSQLDA.SelectCommand.Parameters.AddWithValue("@Cust", Payment)
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
    Public Function GetDeliveryNotes(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustomerID As String, SiteID As String, SalesRepID As String, Fromdate As String, Todate As String, RefNo As String, UserID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetDeliveryNote", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@Fromdate", Fromdate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Todate", Todate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SalesRep", SalesRepID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CustID", CustomerID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", SiteID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@DocNo", RefNo)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UserID", UserID)

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
    Function GetOrderListingByPaymentType(Err_No As Long, Err_Desc As String, SearchQuery As String, Organization As String, Payment As String, FromDate As String, Todate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            If Not Payment = "0" Then
                SearchQuery = " AND CN.Cash_cust='" + Payment + "' " + SearchQuery
            End If


            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_LoadOrderList", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SearchParams", SearchQuery)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgId", Organization)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", Todate)



            'objSQLDA.SelectCommand.Parameters.AddWithValue("@Cust", Payment)
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

    Function GetWindowServKPISummary(Err_No As Long, Err_Desc As String, Organization As String, SID As String, Fromdate As String, Todate As String, USerID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try



            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetWindowsCustServiceKPI_Summary", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", Organization)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Fromdate", Fromdate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", Todate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", USerID)

            'objSQLDA.SelectCommand.Parameters.AddWithValue("@Cust", Payment)
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
    Public Function GetCashVanAudit_Asr(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OID As String, ByVal SID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetCashVanAudit", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SalesRepID", SID)



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


    Public Function GetCustomerVanListing(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, ByVal CustID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_LoadCustomerVanList", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgId", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CustID", CustID)

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
    Public Function GetDistributionCheckDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Qry As String, van As String, Fromdate As String, Todate As String, UID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_DisCheckDetails", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgId", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SearchParams", Qry)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SPID", van)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Fromdate", Fromdate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", Todate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", UID)

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

    Function GetSalesByProduct(Err_No As Long, Err_Desc As String, SearchQuery As String, Organization As String, Payment As String, van As String, UID As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            If Not Payment = "0" Then
                SearchQuery = " AND CN.Cash_cust='" + Payment + "' " + SearchQuery
            End If


            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_LoadSalesbyProduct", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SearchParams", SearchQuery)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgId", Organization)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", van)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", UID)

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

    'task 8
    Function GetSalesByProductValue(Err_No As Long, Err_Desc As String, SearchQuery As String, Organization As String, Payment As String, van As String, UID As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            If Not Payment = "0" Then
                SearchQuery = " AND CN.Cash_cust='" + Payment + "' " + SearchQuery
            End If


            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_SaleRep_Vat", objSQLConn)
           

            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SearchParams", SearchQuery)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgId", Organization)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", van)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", UID)
           

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


    Public Function GetEmpIncentive(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, ByVal EMP_CODE As String, ByVal YEAR As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_Loademp_Incentive", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgId", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@EMP_CODE", EMP_CODE)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@YEAR", YEAR)
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
    Public Function ExportToExcelMerchandasingResult(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SessionID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_ExportExcelMerchandasingSurveyResp", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Survey_Session_ID", SessionID)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "21240170"
            Err_Desc = ex.Message

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function


    Public Function ExportToExcelMerchandasingResult_Blk(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal SurveyID As String, ByVal FromDate As String, ByVal Todate As String, ByVal SalesRep_ID As String, ByVal CustID As String, ByVal SiteID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_ExportExcelMerchandasingSurveyResp_blk", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SurveyID", SurveyID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Todate", Todate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SalesRep_ID", SalesRep_ID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CustID", CustID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", SiteID)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "21240170"
            Err_Desc = ex.Message

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetCreditHistoryDetails(ByRef Err_No As Long, ByRef Err_Desc As String, FromDate As String, ToDate As String, Customer As String, User As String, Org As String, Site As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetCreditHistoryDetails", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Customer", Customer)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", Site)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@User", User)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Org", Org)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "274069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetAppControlFlag(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Key As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_AppControlFlag", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Key", Key)
            objSQLDA.Fill(dt)
        Catch ex As Exception
            Err_No = "25121069"
            Err_Desc = ex.Message
            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function Rep_GetMonthDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Opt As Integer, ByVal Org_ID As String, ByVal Incentive_Month As Integer, ByVal Incentive_Year As Integer, ByVal Emp_Code As String, ByVal FromDate As String, ByVal ToDate As String, ByVal Ref_No As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtIncentive As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetIncentiveMonthDetails", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Option", Opt)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Org_ID", Org_ID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Incentive_Month", Incentive_Month)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Incentive_Year", Incentive_Year)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Emp_Code", Emp_Code)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Ref_No", Ref_No)
            objSQLDA.Fill(dtIncentive)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "90157"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        Rep_GetMonthDetails = dtIncentive
    End Function

    Function GetReturnByProduct(Err_No As Long, Err_Desc As String, SearchQuery As String, Organization As String, Payment As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            If Not Payment = "0" Then
                SearchQuery = " AND CN.Cash_cust='" + Payment + "' " + SearchQuery
            End If


            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_LoadReturnbyProduct", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SearchParams", SearchQuery)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgId", Organization)
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
    Public Function GetSalesVolumeByProductDetails(ByRef Err_No As Long, ByRef Err_Desc As String, SearchQuery As String, ByVal OrgId As String, VanID As String, UID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_SalesVolumnbyProductDetails", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SearchParams", SearchQuery)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", VanID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", UID)

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
    Public Function GetSalesVolumeByProduct(ByRef Err_No As Long, ByRef Err_Desc As String, SearchQuery As String, ByVal OrgId As String, VanID As String, UID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_SalesVolumnbyProduct", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SearchParams", SearchQuery)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", VanID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", UID)

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
    Function GetStockMovementReport(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Item As String, Fromdate As String, Todate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("rep_StockMovementDetail", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@itemid", Item)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
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
    Public Function SMLast3MonthsTargetVsAchiev(ByRef Err_No As Long, ByRef Err_Desc As String, OID As String, ByVal SMID As String, ByVal MonthYear As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "Rep_SMLast3MonthsTargetVsAchiev"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
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

    Function GetZerobilled_Planned(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Fromdate As String, Todate As String, uid As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_ZB_Det_Planned", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Todate", Todate)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24079"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Function GetZerobilled_NotVisited(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Fromdate As String, Todate As String, uid As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_ZB_Det_NotVisited", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Todate", Todate)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "240789"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Function GetZerobilled_NotBilled(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Fromdate As String, Todate As String, uid As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_ZB_Det_NotBilled", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", Fromdate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Todate", Todate)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "240765"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function GetOpenInvoice(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, FromDate As String, ToDate As String, Customer As String, Overdue As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_OpenInvoices", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CUSTID", Customer)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OVERDUE", Overdue)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", Uid)

            'If Customer = "0" Then

            '    objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", 0)
            'Else
            '    Dim ids() As String
            '    ids = Customer.Split("$")
            '    objSQLDA.SelectCommand.Parameters.AddWithValue("@Customer", ids(0))
            '    objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", ids(1))
            'End If
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
    Function GetDailyProductSales(ByRef Err_No As Long, ByRef Err_Desc As String, Orgid As String, SID As String, UID As Integer, FromDate As String, Todate As String, Agency As String, Category As String, InvID As String, CustID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_DailySalesByProduct", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", Todate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", Orgid)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Agency", Agency)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Category", Category)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", UID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ItemID", InvID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CustomerID", CustID)

            log.Debug(FromDate)
            log.Debug(Todate)
            log.Debug(Orgid)
            log.Debug(SID)
            log.Debug(Agency)
            log.Debug(Category)
            log.Debug(InvID)
            log.Debug(CustID)
            log.Debug(UID)

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
    Public Function GetRowIDFromOrigSysDocNo(DocNO As String, ByVal Type As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String
            If Type = "R" Then
                QueryString = String.Format("SELECT Row_ID  from TBL_RMA  WHERE Orig_Sys_Document_Ref='" & DocNO & "'")
            ElseIf Type = "O" Then
                QueryString = String.Format("SELECT Row_ID  from TBL_Order  WHERE Orig_Sys_Document_Ref='" & DocNO & "'")
            End If

            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            If dt.Rows.Count > 0 Then
                ProductPath = dt.Rows(0)(0).ToString()
            End If
            objSQLCmd.Dispose()
        Catch ex As Exception


            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return ProductPath
    End Function
    Public Function GetSR(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchParams As String, ByVal OrgId As String, fromdate As String, Todate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_LoadSRList", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SearchParams", SearchParams)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgId", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Fromdate", fromdate)
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
    Function Reconciliation(ByRef Err_No As Long, ByRef Err_Desc As String, Orgid As String, SID As String, UID As Integer, FromDate As String, Todate As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim ds As New DataSet
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_ReconcileReport", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", Todate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", Orgid)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", SID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Uid", UID)


            objSQLDA.Fill(ds)

        Catch ex As Exception
            Err_No = "74069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return ds
    End Function

    Public Function GetCustomerfromOrgtext_DisCtl(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, Optional text As String = "") As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            Dim QueryString As String
            objSQLConn = _objDB.GetSQLConnection

            QueryString = "SELECT (LTRIM(STR(Customer_ID)) + '$' + LTRIM(STR(Site_Use_ID))) as CustomerID,'['+IsNULL(Customer_No,'N/A')+']-'+IsNULL(Customer_Name,'N/A') as Customer from dbo.app_GetOrgCustomers ('" & OrgID & "')"


            If text <> "" Then
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

            GetCustomerfromOrgtext_DisCtl = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function GetAssetViewHistory(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, AssetID As String, FromDate As String, Todate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetAssetHistoryByID", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@AssetID", AssetID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", Todate)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "54069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetAssetViewHistory_Dates(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, AssetID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            Dim QueryString As String
            objSQLConn = _objDB.GetSQLConnection

            QueryString = "SELECT  MIN(Logged_At) AS MINdate, MIN(Logged_At) AS MAXdate  FROM TBL_Asset_History WHERE Asset_ID ='" & AssetID & "'"



            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "AssetHistoryTbl")
            GetAssetViewHistory_Dates = MsgDs.Tables("AssetHistoryTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function GetAssetViewHistory_Images(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, Asset_ID As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_GetAssetHistroryImages", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Asset_ID", Asset_ID)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "54069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt

    End Function

    Public Function GetOrderHeaderDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, RowID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetOrderHeaderDetails", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@RowID", RowID)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "54069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function GetOrderItemDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, RowID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetOrderDetailsbyRowID", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@RowID", RowID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgID)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "54069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function GetDiscountDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, RowID As String, Type As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetDiscountbyID", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@RowID", RowID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@type", Type)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "54069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetLPOImages(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, RefNo As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_GetLPOImages", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@RefNo", RefNo)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "54069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function GetCollectionHeaderDetails(ByRef Err_No As Long, ByRef Err_Desc As String, RowID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_Collection_Header", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CollectionID", RowID)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "54069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function GetCollectionInvoiceDetails(ByRef Err_No As Long, ByRef Err_Desc As String, RowID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_CollectionInvoices", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CollectionID", RowID)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "54069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function GetChequeImages(ByRef Err_No As Long, ByRef Err_Desc As String, RefNo As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetChequeImages", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CollectionID", RefNo)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "54069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetReturnHeaderDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, RowID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_LoadOrderReturnHeaderDetails", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@RowID", RowID)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "54069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function


    Public Function GetReturnItemDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, RowID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_LoadReturnItemDetails", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@RowID", RowID)
            ' objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgID)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "54069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function


    Public Function GetSRHeaderDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, RowID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_LoadSRHeaderDetails", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@RowID", RowID)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "54069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function


    Public Function GetSRItemDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, RowID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_LoadSRItemDetails", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@RowID", RowID)
            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "54069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetCustomerfromOrgtextforDiscount(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, Optional text As String = "") As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            Dim QueryString As String
            objSQLConn = _objDB.GetSQLConnection

            QueryString = "SELECT (LTRIM(STR(Customer_ID)) + '$' + LTRIM(STR(Site_Use_ID))) as CustomerID,'['+IsNULL(Customer_No,'N/A')+']-'+IsNULL(Customer_Name,'N/A') as Customer from dbo.app_GetOrgCustomers ('" & OrgID & "') A"

            QueryString = QueryString & " where not exists(Select 1 from TBL_Customer_Addl_Info adl where a.Customer_ID=adl.Customer_ID and A.Site_use_ID=adl.Site_use_ID and adl.Attrib_Name='DISCOUNT')"

            If text <> "" Then
                QueryString = QueryString & " and (Customer_no LIKE '%' + @txt + '%' OR Customer_no +'-'+ Customer_name LIKE '%' + @txt + '%')"
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

            GetCustomerfromOrgtextforDiscount = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function


    Function GetEOTReportSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Fromdate As String, Todate As String, uid As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_EOTSummaryPresale", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
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
    'Task 5 Rakesh EOT Report

    Function GetEOTReportDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Fromdate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_EOTDetailed_PreSale", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
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


End Class
