Imports System.Data.SqlClient
Imports SalesWorx.BO.DAL
Public Class Reports
    Dim ObjDALReport As New DAL_Reports
    Function GetCollectionListing(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchParams As String, ByVal OrgId As String) As DataTable
        Try
            Return ObjDALReport.GetCollectionListing(Err_No, Err_Desc, SearchParams, OrgId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetMonthlyVisits(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchParams As String, ByVal OrgId As String) As DataTable
        Try
            Return ObjDALReport.GetMonthlyVisits(Err_No, Err_Desc, SearchParams, OrgId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetPaymentReceivedSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PaymentType As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String, Uid As String) As DataTable
        Try
            Return ObjDALReport.GetPaymentReceivedSummary(Err_No, Err_Desc, PaymentType, OrgId, SID, FromDate, ToDate, Uid)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCollectionDiscount(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String, Uid As String, ByVal CustID As String, ByVal SiteID As String) As DataTable
        Try
            Return ObjDALReport.GetCollectionDiscount(Err_No, Err_Desc, OrgId, SID, FromDate, ToDate, Uid, CustID, SiteID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetAllOrgAgency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, UID As Integer) As DataTable
        Try
            Return ObjDALReport.GetAllOrgAgency(Err_No, Err_Desc, OrgId, UID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetAllOrgBrand(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, UID As Integer, AgencyList As String) As DataTable
        Try
            Return ObjDALReport.GetAllOrgBrand(Err_No, Err_Desc, OrgId, UID, AgencyList)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetOrgSKU(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, searchtext As String, BrandList As String) As DataTable
        Try
            Return ObjDALReport.GetOrgSKU(Err_No, Err_Desc, OrgId, searchtext, BrandList)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetAllOrgSKU(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, UID As Integer) As DataTable
        Try
            Return ObjDALReport.GetAllOrgSKU(Err_No, Err_Desc, OrgId, UID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetMerchandisingSurveyCustomer(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Qry As String) As DataTable
        Try
            Return ObjDALReport.GetMerchandisingSurveyCustomer(Err_No, Err_Desc, Qry)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetAllOrgVan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, UID As Integer) As DataTable
        Try
            Return ObjDALReport.GetAllOrgVan(Err_No, Err_Desc, OrgId, UID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SMTeamPerformance(ByRef Err_No As Long, ByRef Err_Desc As String, OID As String, ByVal SMID As String, ByVal MonthYear As String) As DataTable
        Try
            Return ObjDALReport.SMTeamPerformance(Err_No, Err_Desc, OID, SMID, MonthYear)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SMLast3MonthsSalesgrowth(ByRef Err_No As Long, ByRef Err_Desc As String, OID As String, ByVal SMID As String, ByVal MonthYear As String) As DataTable
        Try
            Return ObjDALReport.SMLast3MonthsSalesgrowth(Err_No, Err_Desc, OID, SMID, MonthYear)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SMTop10Customers(ByRef Err_No As Long, ByRef Err_Desc As String, OID As String, ByVal SMID As String, ByVal MonthYear As String) As DataTable
        Try
            Return ObjDALReport.SMTop10Customers(Err_No, Err_Desc, OID, SMID, MonthYear)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SMTop10CustomersTop10(ByRef Err_No As Long, ByRef Err_Desc As String, OID As String, ByVal SMID As String, ByVal MonthYear As String) As DataTable
        Try
            Return ObjDALReport.SMTop10CustomersTop10(Err_No, Err_Desc, OID, SMID, MonthYear)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SMVanLog(ByRef Err_No As Long, ByRef Err_Desc As String, OID As String, ByVal SMID As String, ByVal MonthYear As String) As DataTable
        Try
            Return ObjDALReport.SMVanLog(Err_No, Err_Desc, OID, SMID, MonthYear)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetSalesManagerByOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String) As DataTable
        Try
            Return ObjDALReport.GetSalesManagerByOrg(Err_No, Err_Desc, OrgId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetVanTransactions(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Try
            Return ObjDALReport.GetVanTransactions(Err_No, Err_Desc, OrgId, SID, FromDate, ToDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetAuditMissedVans(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SurveyID As String) As DataTable
        Try
            Return ObjDALReport.GetAuditMissedVans(Err_No, Err_Desc, OrgId, SurveyID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetFSRCollection(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal uid As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Try
            Return ObjDALReport.GetFSRCollection(Err_No, Err_Desc, uid, OrgId, SID, FromDate, ToDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetVanSalesbyCustomer(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal uid As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Try
            Return ObjDALReport.GetVanSalesbyCustomer(Err_No, Err_Desc, uid, OrgId, SID, FromDate, ToDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetVanSalesbyMonth(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal uid As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Try
            Return ObjDALReport.GetVanSalesbyMonth(Err_No, Err_Desc, uid, OrgId, SID, FromDate, ToDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetPDCRecievablesbyMonth(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal uid As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Try
            Return ObjDALReport.GetPDCRecievablesbyMonth(Err_No, Err_Desc, uid, OrgId, SID, FromDate, ToDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetDelayedCustomerCount(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal uid As String, ByVal OrgId As String, FromDate As String, ToDate As String) As DataTable
        Try
            Return ObjDALReport.GetDelayedCustomerCount(Err_No, Err_Desc, uid, OrgId, FromDate, ToDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetHeldPDC(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, CollectionRefNo As String, FromDate As String, ToDate As String, Uid As String) As DataTable
        Try
            Return ObjDALReport.GetHeldPDC(Err_No, Err_Desc, OrgId, CollectionRefNo, FromDate, ToDate, Uid)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetPDCReceivables(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String) As DataTable
        Try
            Return ObjDALReport.GetPDCReceivables(Err_No, Err_Desc, OrgId, SID, Uid)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetCurrencyfromVan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal VanList As String) As DataTable
        Try
            Return ObjDALReport.GetCurrency(Err_No, Err_Desc, VanList)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCurrency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String) As DataTable
        Try
            Return ObjDALReport.GetCurrency(Err_No, Err_Desc, OrgId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCustomerListing(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchParams As String, ByVal OrgId As String) As DataTable
        Try
            Return ObjDALReport.GetCustomerListing(Err_No, Err_Desc, SearchParams, OrgId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetGoodsReceipt(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, Van As String, ApprovalCode As String, ApprovedBy As String) As DataTable
        Try
            Return ObjDALReport.GetGoodsReceipt(Err_No, Err_Desc, OrgId, Fromdate, ToDate, Van, ApprovalCode, ApprovedBy)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetStockRequisition(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, Van As String) As DataTable
        Try
            Return ObjDALReport.GetStockRequisition(Err_No, Err_Desc, OrgId, Fromdate, ToDate, Van)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetMonthlyVanLoad(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, Van As String) As DataTable
        Try
            Return ObjDALReport.GetMonthlyVanLoad(Err_No, Err_Desc, OrgId, Fromdate, ToDate, Van)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetMonthlySalesVsReturn(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, Van As String, ItemId As String) As DataTable
        Try
            Return ObjDALReport.GetMonthlySalesVsReturn(Err_No, Err_Desc, OrgId, Fromdate, ToDate, Van, ItemId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetOffTakeSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, Van As String, ItemId As String) As DataTable
        Try
            Return ObjDALReport.GetOffTakeSummary(Err_No, Err_Desc, OrgId, Fromdate, ToDate, Van, ItemId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetMonthlyWastageReport(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, Van As String, ItemId As String) As DataTable
        Try
            Return ObjDALReport.GetMonthlyWastageReport(Err_No, Err_Desc, OrgId, Fromdate, ToDate, Van, ItemId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCoveredvsBilled(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, Van As String) As DataTable
        Try
            Return ObjDALReport.GetCoveredvsBilled(Err_No, Err_Desc, OrgId, Fromdate, ToDate, Van)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetVanPerformance(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, Van As String, SalesDistrictID As String, DisplayMode As String) As DataTable
        Try
            Return ObjDALReport.GetVanPerformance(Err_No, Err_Desc, OrgId, Fromdate, ToDate, Van, SalesDistrictID, DisplayMode)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetVanPerformance_ASR(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As String, ToDate As String, Van As String, SalesDistrictID As String, UID As String) As DataTable
        Try
            Return ObjDALReport.GetVanPerformance_ASR(Err_No, Err_Desc, OrgId, Fromdate, ToDate, Van, SalesDistrictID, UID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCustomerVisits(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Qry As String, Van As String, UID As String, Fromdate As String, Todate As String) As DataTable
        Try
            Return ObjDALReport.GetCustomerVisits(Err_No, Err_Desc, OrgId, Qry, Van, UID, Fromdate, Todate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetDistributionCheckList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Qry As String, van As String, UID As String) As DataTable
        Try
            Return ObjDALReport.GetDistributionCheckList(Err_No, Err_Desc, OrgId, Qry, van, UID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetInvDiscountDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Qry As String) As DataTable
        Try
            Return ObjDALReport.GetInvDiscountDetails(Err_No, Err_Desc, OrgId, Qry)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCustomerVisitsSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, SID As String, Uid As String, CustID As String, CustType As String) As DataTable
        Try
            Return ObjDALReport.GetCustomerVisitsSummary(Err_No, Err_Desc, OrgId, Fromdate, ToDate, SID, Uid, CustID, CustType)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetOrderListing(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchParams As String, ByVal OrgId As String, FromDate As String, ToDate As String) As DataTable
        Try
            Return ObjDALReport.GetOrderListing(Err_No, Err_Desc, SearchParams, OrgId, FromDate, ToDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetDailySalesReport_Order(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, FromDate As String, ToDate As String) As DataTable
        Try
            Return ObjDALReport.GetDailySalesReport_Order(Err_No, Err_Desc, OrgId, SID, Uid, FromDate, ToDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSalesManWiseAgencyWiseSalesAndReturns(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, FromDate As String, ToDate As String, Agency As String) As DataTable
        Try
            Return ObjDALReport.GetSalesManWiseAgencyWiseSalesAndReturns(Err_No, Err_Desc, OrgId, SID, Uid, FromDate, ToDate, Agency)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetDailySalesReport_Return(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, FromDate As String, ToDate As String) As DataTable
        Try
            Return ObjDALReport.GetDailySalesReport_Return(Err_No, Err_Desc, OrgId, SID, Uid, FromDate, ToDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetReturns(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchParams As String, ByVal OrgId As String, FromDate As String, Todate As String) As DataTable
        Try
            Return ObjDALReport.GetReturns(Err_No, Err_Desc, SearchParams, OrgId, FromDate, Todate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetDistributionCheck(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchParams As String, ByVal OrgId As String) As DataTable
        Try
            Return ObjDALReport.GetDistributionCheck(Err_No, Err_Desc, SearchParams, OrgId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetTransactionStatus(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Status As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String, Uid As String, CustomerId As String, SiteID As String) As DataTable
        Try
            Return ObjDALReport.GetTransactionStatus(Err_No, Err_Desc, Status, OrgId, SID, FromDate, ToDate, Uid, CustomerId, SiteID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetFocFeedeback(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, FromDate As String, ToDate As String, CustomerId As String, SiteId As String) As DataTable
        Try
            Return ObjDALReport.GetFocFeedeback(Err_No, Err_Desc, OrgId, FromDate, ToDate, CustomerId, SiteId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetVanActivity(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, FromDate As String, ToDate As String, Uid As String, VanID As String, SyncType As String) As DataTable
        Try
            Return ObjDALReport.GetVanActivity(Err_No, Err_Desc, OrgId, FromDate, ToDate, Uid, VanID, SyncType)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetTargetvsSalesSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, VanList As String, FMonth As DateTime, Mode As String) As DataTable
        Try
            Return ObjDALReport.GetTargetvsSalesSummary(Err_No, Err_Desc, OrgId, VanList, FMonth, Mode)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetVanKPI(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, VanList As String, FMonth As DateTime, Mode As String) As DataTable
        Try
            Return ObjDALReport.GetVanKPI(Err_No, Err_Desc, OrgId, VanList, FMonth, Mode)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetScroeCardSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, VanList As String, FMonth As DateTime, Mode As String) As DataTable
        Try
            Return ObjDALReport.GetScroeCardSummary(Err_No, Err_Desc, OrgId, VanList, FMonth, Mode)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetScoreCardOutlet(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, VanList As String, FMonth As DateTime, Mode As String) As DataTable
        Try
            Return ObjDALReport.GetScoreCardOutlet(Err_No, Err_Desc, OrgId, VanList, FMonth, Mode)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetMonthlyFOCByVanSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, VanList As String, FMonth As DateTime) As DataTable
        Try
            Return ObjDALReport.GetMonthlyFOCByVanSummary(Err_No, Err_Desc, OrgId, VanList, FMonth)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetBillCutsByVanSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, VanList As String, FMonth As String, Todate As String) As DataTable
        Try
            Return ObjDALReport.GetBillCutsByVanSummary(Err_No, Err_Desc, OrgId, VanList, FMonth, Todate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetLineCutsByVanSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, VanList As String, FMonth As String, Todate As String, CustomerID As String, SiteUSeID As String) As DataTable
        Try
            Return ObjDALReport.GetLineCutsByVanSummary(Err_No, Err_Desc, OrgId, VanList, FMonth, Todate, CustomerID, SiteUSeID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetBestSellersSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, VanList As String, FMonth As DateTime, TMonth As DateTime, Mode As String, ChartMode As String) As DataTable
        Try
            Return ObjDALReport.GetBestSellersSummary(Err_No, Err_Desc, OrgId, VanList, FMonth, TMonth, Mode, ChartMode)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetDivisionCollection(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, FromDate As String, ToDate As String, Uid As String) As DataTable
        Try
            Return ObjDALReport.GetDivisionCollection(Err_No, Err_Desc, OrgId, FromDate, ToDate, Uid)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetManagersByOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String) As DataTable
        Try
            Return ObjDALReport.GetManagersByOrg(Err_No, Err_Desc, OrgId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetSalesDist(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDALReport.GetSalesDist(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetDelayedCollectionByVan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgStr As String, ByVal MgrStr As String, LocStr As String, SDate As String, EDate As String) As DataTable
        Try
            Return ObjDALReport.GetDelayedCollectionByVan(Err_No, Err_Desc, OrgStr, MgrStr, LocStr, SDate, EDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetMBRByVan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgStr As String, ByVal VanStr As String, AgencyStr As String, SDate As DateTime, EDate As DateTime) As DataTable
        Try
            Return ObjDALReport.GetMBRByVan(Err_No, Err_Desc, OrgStr, VanStr, AgencyStr, SDate, EDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetMBRByAgencySummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgStr As String, ByVal VanStr As String, AgencyStr As String, SDate As String, EDate As String) As DataTable
        Try
            Return ObjDALReport.GetMBRByAgencySummary(Err_No, Err_Desc, OrgStr, VanStr, AgencyStr, SDate, EDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetMBRSummaryByVan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgStr As String, ByVal VanStr As String, AgencyStr As String, SDate As DateTime, EDate As DateTime) As DataTable
        Try
            Return ObjDALReport.GetMBRSummaryByVan(Err_No, Err_Desc, OrgStr, VanStr, AgencyStr, SDate, EDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetMBRByAgency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgStr As String, ByVal VanStr As String, AgencyStr As String, SDate As DateTime, EDate As DateTime) As DataTable
        Try
            Return ObjDALReport.GetMBRByAgency(Err_No, Err_Desc, OrgStr, VanStr, AgencyStr, SDate, EDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetMonthlyTargetAndSales(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgStr As String, ByVal VanStr As String, AgencyStr As String, SDate As String, EDate As String) As DataTable
        Try
            Return ObjDALReport.GetMonthlyTargetAndSales(Err_No, Err_Desc, OrgStr, VanStr, AgencyStr, SDate, EDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetMBRSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgStr As String, ByVal VanStr As String, AgencyStr As String, SDate As DateTime, EDate As DateTime) As DataSet
        Try
            Return ObjDALReport.GetMBRSummary(Err_No, Err_Desc, OrgStr, VanStr, AgencyStr, SDate, EDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetMBRTargetvsSalesByMonths(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgStr As String, ByVal VanStr As String, AgencyStr As String, SDate As DateTime, EDate As DateTime) As DataTable
        Try
            Return ObjDALReport.GetMBRTargetvsSalesByMonths(Err_No, Err_Desc, OrgStr, VanStr, AgencyStr, SDate, EDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetMBRTargetvsSalesPerMonths(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgStr As String, ByVal VanStr As String, SDate As DateTime) As DataTable
        Try
            Return ObjDALReport.GetMBRTargetvsSalesPerMonths(Err_No, Err_Desc, OrgStr, VanStr, SDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetMonthlyDistribution(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgStr As String, ByVal VanStr As String, AgencyStr As String, SDate As String, EDate As String, GroupBy As String, Brand As String, SKU As String, ChartMode As String) As DataTable
        Try
            Return ObjDALReport.GetMonthlyDistribution(Err_No, Err_Desc, OrgStr, VanStr, AgencyStr, SDate, EDate, GroupBy, Brand, SKU, ChartMode)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetReceivables(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Year As String, Month As String, UserID As String, Van As String) As DataTable
        Try
            Return ObjDALReport.GetReceivables(Err_No, Err_Desc, OrgId, Year, Month, UserID, Van)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetBrand(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Agency As String) As DataTable
        Try
            Return ObjDALReport.GetBrand(Err_No, Err_Desc, OrgId, Agency)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetBrand(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String) As DataTable
        Try
            Return ObjDALReport.GetAgency(Err_No, Err_Desc, OrgId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetItemFromAgency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Agencies As String, Text As String) As DataTable
        Try
            Return ObjDALReport.GetItemFromAgency(Err_No, Err_Desc, OrgId, Agencies, Text)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetItemFromAgencyandBrand(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Agencies As String, Text As String, brandlist As String) As DataTable
        Try
            Return ObjDALReport.GetItemFromAgencyandBrand(Err_No, Err_Desc, OrgId, Agencies, Text, brandlist)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetItemFromAgencyAndUOM(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Agencies As String, UOM As String, Text As String) As DataTable
        Try
            Return ObjDALReport.GetItemFromAgencyAndUOM(Err_No, Err_Desc, OrgId, Agencies, UOM, Text)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetAllItems(ByRef Err_No As Long, ByRef Err_Desc As String, Text As String) As DataTable
        Try
            Return ObjDALReport.GetAllItems(Err_No, Err_Desc, Text)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetItemByInventory(ByRef Err_No As Long, ByRef Err_Desc As String, Text As String) As DataTable
        Try
            Return ObjDALReport.GetItemsByInvetory(Err_No, Err_Desc, Text)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetAllItemsByOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Text As String) As DataTable
        Try
            Return ObjDALReport.GetAllItemsbyOrg(Err_No, Err_Desc, OrgId, Text)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetAllActiveItemsbyOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Text As String) As DataTable
        Try
            Return ObjDALReport.GetAllActiveItemsbyOrg(Err_No, Err_Desc, OrgId, Text)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetCustomerfromOrgtext(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Text As String) As DataTable
        Try
            Return ObjDALReport.GetCustomerfromOrgtext(Err_No, Err_Desc, OrgId, Text)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetCustomerfromPaymentType(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Text As String, selectedPayment As String) As DataTable
        Try
            Return ObjDALReport.GetCustomerfromPaymentType(Err_No, Err_Desc, OrgId, selectedPayment, Text)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetOutletsfromOrgtext(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Text As String) As DataTable
        Try
            Return ObjDALReport.GetOutletfromOrgtext(Err_No, Err_Desc, OrgId, Text)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetBrandList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_id As String) As DataTable
        Try
            Return ObjDALReport.GetBrandList(Err_No, Err_Desc, Org_id)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSKU(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_id As String, Agency As String, Brand As String) As DataTable
        Try
            Return ObjDALReport.GetSKU(Err_No, Err_Desc, Org_id, Agency, Brand)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetSalesbyVanForMonth(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal param1 As String, ByVal param2 As String) As DataTable
        Try
            Return ObjDALReport.GetSalesbyVanForMonth(Err_No, Err_Desc, param1, param2)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetSalesbyAgency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal param1 As String, ByVal param2 As String) As DataTable
        Try
            Return ObjDALReport.GetSalesbyAgency(Err_No, Err_Desc, param1, param2)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetCollectionbyVanForMonth(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal param1 As String, ByVal param2 As String) As DataTable
        Try
            Return ObjDALReport.GetCollectionbyVanForMonth(Err_No, Err_Desc, param1, param2)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetDelayedCollectionBySupervisor(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String, ByVal param5 As String) As DataTable
        Try
            Return ObjDALReport.GetDelayedCollectionBySupervisor(Err_No, Err_Desc, param1, param2, param3, param4, param5)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetDelayedCollectionByLocation(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String, ByVal param5 As String) As DataTable
        Try
            Return ObjDALReport.GetDelayedCollectionByLocation(Err_No, Err_Desc, param1, param2, param3, param4, param5)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetDelayedCollectionByDiv(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String, ByVal param5 As String) As DataTable
        Try
            Return ObjDALReport.GetDelayedCollectionByDiv(Err_No, Err_Desc, param1, param2, param3, param4, param5)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetBankListing(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Code As String, ByVal name As String) As DataTable
        Try
            Return ObjDALReport.GetBankListing(Err_No, Err_Desc, Code, name)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetHolidayListing(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Code As String, ByVal FromDate As String, ByVal ToDate As String) As DataTable
        Try
            Return ObjDALReport.GetHolidayListing(Err_No, Err_Desc, Code, FromDate, ToDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetReceivablesbyMonth(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal uid As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Try
            Return ObjDALReport.GetReceivablesbyMonth(Err_No, Err_Desc, uid, OrgId, SID, FromDate, ToDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetCashVanAudit(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Van As Integer) As DataTable
        Try
            Return ObjDALReport.GetCashVanAudit(Err_No, Err_Desc, OrgId, Van)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetFSRReturn(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, FromDate As String, ToDate As String, Type As String) As DataTable
        Try
            Return ObjDALReport.GetFSRReturn(Err_No, Err_Desc, OrgId, SID, Uid, FromDate, ToDate, Type)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetMissedVisits(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, FromDate As String, ToDate As String) As DataTable
        Try
            Return ObjDALReport.GetMissedVisits(Err_No, Err_Desc, OrgId, SID, Uid, FromDate, ToDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSalesReturnsbyVan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, Type As String, FromDate As String, ToDate As String, Agency As String, Item As String, Customer As String) As DataTable
        Try
            Return ObjDALReport.GetSalesReturnsbyVan(Err_No, Err_Desc, OrgId, SID, Uid, Type, FromDate, ToDate, Agency, Item, Customer)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSalesReturnsbyVanDetailed(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, Type As String, FromDate As String, ToDate As String, Agency As String, Item As String, Customer As String) As DataTable
        Try
            Return ObjDALReport.GetSalesReturnsbyVanDetailed(Err_No, Err_Desc, OrgId, SID, Uid, Type, FromDate, ToDate, Agency, Item, Customer)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSalesReturnsbyAgencyDetailed(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, Type As String, FromDate As String, ToDate As String, Agency As String, Item As String, Customer As String) As DataTable
        Try
            Return ObjDALReport.GetSalesReturnsbyAgencyDetailed(Err_No, Err_Desc, OrgId, SID, Uid, Type, FromDate, ToDate, Agency, Item, Customer)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSalesReturnsbyAgency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, Type As String, FromDate As String, ToDate As String, Agency As String, Item As String, Customer As String) As DataTable
        Try
            Return ObjDALReport.GetSalesReturnsbyAgency(Err_No, Err_Desc, OrgId, SID, Uid, Type, FromDate, ToDate, Agency, Item, Customer)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSalesReturnsbySKU(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, Type As String, FromDate As String, ToDate As String, Agency As String, Item As String, Customer As String) As DataTable
        Try
            Return ObjDALReport.GetSalesReturnsbySKU(Err_No, Err_Desc, OrgId, SID, Uid, Type, FromDate, ToDate, Agency, Item, Customer)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSalesReturnsbyCustomer(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, Type As String, FromDate As String, ToDate As String, Agency As String, Item As String, Customer As String) As DataTable
        Try
            Return ObjDALReport.GetSalesReturnsbyCustomer(Err_No, Err_Desc, OrgId, SID, Uid, Type, FromDate, ToDate, Agency, Item, Customer)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetProducts(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchParams As String) As DataTable
        Try
            Return ObjDALReport.GetProducts(Err_No, Err_Desc, SearchParams)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetPriceListing(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchParams As String) As DataTable
        Try
            Return ObjDALReport.GetPriceListing(Err_No, Err_Desc, SearchParams)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetRevenueDispersion(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, FromDate As String) As DataTable
        Try
            Return ObjDALReport.GetRevenueDispersion(Err_No, Err_Desc, OrgId, SID, Uid, FromDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSKUDispersion(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, FromDate As String) As DataTable
        Try
            Return ObjDALReport.GetSKUDispersion(Err_No, Err_Desc, OrgId, SID, Uid, FromDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetAgeing(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, CustId As String, SiteID As String) As DataTable
        Try
            Return ObjDALReport.GetAgeing(Err_No, Err_Desc, OrgId, CustId, SiteID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetVanSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Fromdate As String, Todate As String, Perc As String, Item As String) As DataTable
        Try
            Return ObjDALReport.GetVanSummary(Err_No, Err_Desc, OrgId, SID, Fromdate, Todate, Perc, Item)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetZerobilledOutlets(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Fromdate As String, Todate As String, uid As String) As DataTable
        Try
            Return ObjDALReport.GetZerobilledOutlets(Err_No, Err_Desc, OrgId, SID, Fromdate, Todate, uid)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetEOTSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Fromdate As String, Todate As String, uid As String) As DataTable
        Try
            Return ObjDALReport.GetEOTSummary(Err_No, Err_Desc, OrgId, SID, Fromdate, Todate, uid)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetAgency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_id As String) As DataTable
        Try
            Return ObjDALReport.GetAgency(Err_No, Err_Desc, Org_id)
        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Public Function GetAllAgency(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDALReport.GetAllAgency(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Function GetEOTDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Fromdate As String) As DataTable
        Try
            Return ObjDALReport.GetEOTDetails(Err_No, Err_Desc, OrgId, SID, Fromdate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCustomerVisitsForEOT(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchParams As String, ByVal OrgId As String) As DataTable
        Try
            Return ObjDALReport.GetCustomerVisitsForEOT(Err_No, Err_Desc, SearchParams, OrgId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetDailyStockReconciliation(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Item As String, Fromdate As String, Todate As String) As DataTable
        Try
            Return ObjDALReport.GetDailyStockReconciliation(Err_No, Err_Desc, OrgId, SID, Item, Fromdate, Todate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetDailyStockReport(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Item As String, Fromdate As String) As DataTable
        Try
            Return ObjDALReport.GetDailyStockReport(Err_No, Err_Desc, OrgId, SID, Item, Fromdate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetVanStockSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, uid As String, Item As String, Agency As String, Brand As String) As DataTable
        Try
            Return ObjDALReport.GetVanStockSummary(Err_No, Err_Desc, OrgId, SID, uid, Item, Agency, Brand)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetVanload(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Item As String, FromDate As String, Todate As String) As DataTable
        Try
            Return ObjDALReport.GetVanload(Err_No, Err_Desc, OrgId, SID, Item, FromDate, Todate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetVanUnload(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Item As String, FromDate As String, Todate As String) As DataTable
        Try
            Return ObjDALReport.GetVanUnload(Err_No, Err_Desc, OrgId, SID, Item, FromDate, Todate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetVisitDetail(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal VisitID As String) As DataTable
        Try
            Return ObjDALReport.GetVanUnload(Err_No, Err_Desc, VisitID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetWeeklyReturnSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, FromDate As String, ToDate As String, Customer As String, SiteID As String) As DataTable
        Try
            Return ObjDALReport.GetWeeklyReturnSummary(Err_No, Err_Desc, OrgId, SID, Uid, FromDate, ToDate, Customer, SiteID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetPlannedCoverage(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Try
            Return ObjDALReport.GetPlannedCoverage(Err_No, Err_Desc, OrgId, SID, FromDate, ToDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCoverageReport(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String) As DataTable
        Try
            Return ObjDALReport.GetCoverageReport(Err_No, Err_Desc, OrgId, SID, Uid)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetOverallCoverage(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Try
            Return ObjDALReport.GetOverallCoverage(Err_No, Err_Desc, OrgId, SID, FromDate, ToDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetLogReport(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String, CustID As String, SiteID As String, DocType As String) As DataTable
        Try
            Return ObjDALReport.GetLogReport(Err_No, Err_Desc, OrgId, SID, FromDate, ToDate, CustID, SiteID, DocType)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetLogReportSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String, CustID As String, SiteID As String) As DataTable
        Try
            Return ObjDALReport.GetLogReportSummary(Err_No, Err_Desc, OrgId, SID, FromDate, ToDate, CustID, SiteID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetFSRVisitTracking(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, VisitDate As DateTime, Van As String) As DataTable
        Try
            Return ObjDALReport.GetFSRVisitTracking(Err_No, Err_Desc, OrgId, VisitDate, Van)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetFuelExpenses(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, FromDate As DateTime, EndDate As DateTime, Van As String) As DataTable
        Try
            Return ObjDALReport.GetFuelExpenses(Err_No, Err_Desc, OrgId, FromDate, EndDate, Van)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetVanProcessTransactions(ByRef Err_No As Long, ByRef Err_Desc As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Try
            Return ObjDALReport.GetVanProcessTransactions(Err_No, Err_Desc, SID, FromDate, ToDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetApprovalCodeusage(ByRef Err_No As Long, ByRef Err_Desc As String, OID As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Try
            Return ObjDALReport.GetApprovalCodeusage(Err_No, Err_Desc, OID, SID, FromDate, ToDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSynExceptionReport(ByRef Err_No As Long, ByRef Err_Desc As String, Hours As String, SID As String, SyncType As String) As DataTable
        Try
            Return ObjDALReport.GetSynExceptionReport(Err_No, Err_Desc, Hours, SID, SyncType)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetDeviceSynLog(ByRef Err_No As Long, ByRef Err_Desc As String, SID As String, FromDate As String, ToDate As String, Stype As String) As DataTable
        Try
            Return ObjDALReport.GetDeviceSynLog(Err_No, Err_Desc, SID, FromDate, ToDate, Stype)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetOverallCoverage_Visited(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Try
            Return ObjDALReport.GetOverallCoverage_Visited(Err_No, Err_Desc, OrgId, SID, FromDate, ToDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetOverallCoverage_Planned(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Try
            Return ObjDALReport.GetOverallCoverage_Planned(Err_No, Err_Desc, OrgId, SID, FromDate, ToDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetOverallCoverage_ZeroBilled(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Try
            Return ObjDALReport.GetOverallCoverage_ZeroBilled(Err_No, Err_Desc, OrgId, SID, FromDate, ToDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetOverallCoverage_NotVisited(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String) As DataTable
        Try
            Return ObjDALReport.GetOverallCoverage_NotVisited(Err_No, Err_Desc, OrgId, SID, FromDate, ToDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCustomerSurvey(ByRef Err_No As Long, ByRef Err_Desc As String, SearchParam As String, Orgid As String) As DataTable
        Try
            Return ObjDALReport.GetCustomerSurvey(Err_No, Err_Desc, SearchParam, OrgId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function ArtilcleMovement(ByRef Err_No As Long, ByRef Err_Desc As String, Orgid As String, SID As String, UID As Integer, FromDate As String, Todate As String, DocType As String, InvID As String) As DataTable
        Try
            Return ObjDALReport.ArtilcleMovement(Err_No, Err_Desc, Orgid, SID, UID, FromDate, Todate, DocType, InvID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetAuditSurvey(ByRef Err_No As Long, ByRef Err_Desc As String, SearchParam As String, Orgid As String, Status As String, SalesRepid As String) As DataTable
        Try
            Return ObjDALReport.GetAuditSurvey(Err_No, Err_Desc, SearchParam, Orgid, Status, SalesRepid)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetJPAdherence(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal uid As String, ByVal OrgId As String, SID As String, Month As String, year As String) As DataTable
        Try
            Return ObjDALReport.GetJPAdherence(Err_No, Err_Desc, uid, OrgId, SID, Month, year)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetProductAvailablity(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, ByVal Van As String, ByVal UID As String, ByVal Agency As String, ByVal Item As String, ByVal FromDate As String, ByVal ToDate As String, ByVal Type As String) As DataTable
        Try
            Return ObjDALReport.GetProductAvailablity(Err_No, Err_Desc, OrgId, Van, UID, Agency, Item, FromDate, ToDate, Type)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetDailyReport(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, Van As String, Uid As String) As DataTable
        Try
            Return ObjDALReport.GetDailyReport(Err_No, Err_Desc, OrgId, Fromdate, ToDate, Van, Uid)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSKUWiseSalesReturns(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As String, ToDate As String, Van As String, Agency As String, CustID As String, InID As String, UID As Integer) As DataTable
        Try
            Return ObjDALReport.GetSKUWiseSalesReturns(Err_No, Err_Desc, OrgId, Fromdate, ToDate, Van, Agency, CustID, InID, UID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSalesAndDsicount(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As String, ToDate As String, Van As String, Uid As String, CustID As String, SiteId As String) As DataTable
        Try
            Return ObjDALReport.GetSalesAndDsicount(Err_No, Err_Desc, OrgId, Fromdate, ToDate, Van, Uid, CustID, SiteId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetAssets(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, OnlyActive As String, CustomerID As String, SiteID As String, AssetType As String) As DataTable
        Try
            Return ObjDALReport.GetAssets(Err_No, Err_Desc, OrgId, OnlyActive, CustomerID, SiteID, AssetType)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetOfferDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, Brand As String, Mode As String) As DataTable
        Try
            Return ObjDALReport.GetOfferDetails(Err_No, Err_Desc, OrgId, Fromdate, ToDate, Brand, Mode)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GeAssortmentDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, Brand As String, Mode As String) As DataTable
        Try
            Return ObjDALReport.GeAssortmentDetails(Err_No, Err_Desc, OrgId, Fromdate, ToDate, Brand, Mode)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetUserChangeLog(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchParams As String) As DataTable
        Try
            Return ObjDALReport.GetUserChangeLog(Err_No, Err_Desc, SearchParams)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetProductiveCalls(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, UID As String) As DataTable
        Try
            Return ObjDALReport.GetProductiveCalls(Err_No, Err_Desc, OrgId, SID, UID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSalesbySKU(ByRef Err_No As Long, ByRef Err_Desc As String, Fromdate As String, Todate As String, ByVal OrgId As String, InvID As String) As DataTable
        Try
            Return ObjDALReport.GetSalesbySKU(Err_No, Err_Desc, Fromdate, Todate, OrgId, InvID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSalesbyVanAgencyQty(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, UID As String) As DataTable
        Try
            Return ObjDALReport.GetSalesbyVanAgencyQty(Err_No, Err_Desc, OrgId, UID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSalesbyVanAgency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, UID As String) As DataTable
        Try
            Return ObjDALReport.GetSalesbyVanAgency(Err_No, Err_Desc, OrgId, UID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetDistributionforMSL(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, UID As String, Fromdate As String, Todate As String) As DataTable
        Try
            Return ObjDALReport.GetDistributionforMSL(Err_No, Err_Desc, OrgId, SID, UID, Fromdate, Todate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetProductivityperMSL(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, UID As String, Invid As String) As DataTable
        Try
            Return ObjDALReport.GetProductivityperMSL(Err_No, Err_Desc, OrgId, SID, UID, Invid)

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetSurveyStatistics(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SurveyID As String) As DataTable
        Try
            Return ObjDALReport.GetSurveyStatistics(Err_No, Err_Desc, OrgId, SurveyID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSurveyStatisticsChart(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SurveyID As String, QuesID As String) As DataTable
        Try
            Return ObjDALReport.GetSurveyStatisticsChart(Err_No, Err_Desc, OrgId, SurveyID, QuesID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetSurveyOtherResponses(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SurveyID As String, Type As String) As DataTable
        Try
            Return ObjDALReport.GetSurveyOtherResponses(Err_No, Err_Desc, OrgId, SurveyID, Type)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetDistributionDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, Month As String, Year As String, Cid As String, SiteID As String, Item As String) As DataTable
        Try
            Return ObjDALReport.GetDistributionDetails(Err_No, Err_Desc, OrgId, SID, Uid, Month, Year, Cid, SiteID, Item)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetMultiTrxFOC(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, FromDate As String, ToDate As String, Customer As String, Site As String) As DataTable
        Try
            Return ObjDALReport.GetMultiTrxFOC(Err_No, Err_Desc, OrgId, FromDate, ToDate, Customer, Site)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSalesSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, Van As String, UserID As String) As DataTable
        Try
            Return ObjDALReport.GetSalesSummary(Err_No, Err_Desc, OrgId, Fromdate, ToDate, Van, UserID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSalesPrinciple(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, Agency As String) As DataTable
        Try
            Return ObjDALReport.GetSalesPrinciple(Err_No, Err_Desc, OrgId, Fromdate, ToDate, Agency)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetWareHousePurchase(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Agency As String, FromDate As String, Todate As String) As DataTable
        Try
            Return ObjDALReport.GetWareHousePurchase(Err_No, Err_Desc, OrgId, Agency, FromDate, Todate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetERPSyncLog(ByRef Err_No As Long, ByRef Err_Desc As String, FromDate As String, ToDate As String, ERPtable As String) As DataTable
        Try
            Return ObjDALReport.GetERPSyncLog(Err_No, Err_Desc, FromDate, ToDate, ERPtable)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSalesReturnsbyClient(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, Type As String, FromDate As String, ToDate As String, Customer As String) As DataTable
        Try
            Return ObjDALReport.GetSalesReturnsbyClient(Err_No, Err_Desc, OrgId, SID, Uid, Type, FromDate, ToDate, Customer)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSalesbyBrand(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, FromDate As String, ToDate As String, Brand As String, Mode As String) As DataTable
        Try
            Return ObjDALReport.GetSalesbyBrand(Err_No, Err_Desc, OrgId, SID, FromDate, ToDate, Brand, Mode)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSKUPeriodWiseReturns(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, FromDate As String, ToDate As String, SKUID As String) As DataTable
        Try
            Return ObjDALReport.GetSKUPeriodWiseReturns(Err_No, Err_Desc, OrgId, FromDate, ToDate, SKUID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetOutletwiseSaleReturn(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, CustID As Integer, SiteID As Integer, UserID As Integer) As DataTable
        Try
            Return ObjDALReport.GetOutletwiseSaleReturn(Err_No, Err_Desc, OrgId, Fromdate, ToDate, CustID, SiteID, UserID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetOutletwiseSKUwiseSaleReturn(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, INID As String, SiteID As String, Agency As String, FSR As String, UID As Integer) As DataTable
        Try
            Return ObjDALReport.GetOutletwiseSKUwiseSaleReturn(Err_No, Err_Desc, OrgId, Fromdate, ToDate, INID, SiteID, Agency, FSR, UID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetItemFromOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Text As String) As DataTable
        Try
            Return ObjDALReport.GetItemFromOrg(Err_No, Err_Desc, OrgId, Text)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetExcessReturns(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, CustID As Integer, SiteID As Integer, SKUId As Integer, Cutoff As Decimal) As DataTable
        Try
            Return ObjDALReport.GetExcessReturns(Err_No, Err_Desc, OrgId, Fromdate, ToDate, CustID, SiteID, SKUId, Cutoff)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetOutletSKUwiseReturns(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, CustID As Integer, SiteID As Integer, SKUId As Integer, UID As Integer) As DataTable
        Try
            Return ObjDALReport.GetOutletSKUwiseReturns(Err_No, Err_Desc, OrgId, Fromdate, ToDate, CustID, SiteID, SKUId, UID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetPurchaseReport(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As DateTime, ToDate As DateTime, Van As String) As DataTable
        Try
            Return ObjDALReport.GetPurchaseReport(Err_No, Err_Desc, OrgId, Fromdate, ToDate, Van)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetTotalSummarySalesReturns(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As String, ToDate As String, Van As String, Agency As String, Customer As String) As DataTable
        Try
            Return ObjDALReport.GetTotalSummarySalesReturns(Err_No, Err_Desc, OrgId, Fromdate, ToDate, Van, Agency, Customer)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSummaryPurchaseSalesReturns(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Fromdate As String, ToDate As String, Van As String, Agency As String, Type As String) As DataTable
        Try
            Return ObjDALReport.GetSummaryPurchaseSalesReturns(Err_No, Err_Desc, OrgId, Fromdate, ToDate, Van, Agency, Type)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetAllZeroBilledCustomers(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal VanList As String, Fdate As String) As DataTable
        Try
            Return ObjDALReport.GetAllZeroBilledCustomers(Err_No, Err_Desc, VanList, Fdate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetTargetType(ByRef Err_No As Long, ByRef Err_Desc As String) As String
        Try
            Return ObjDALReport.GetTargetType(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetBeaconDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ID As String) As DataTable
        Try
            Return ObjDALReport.GetBeaconDetails(Err_No, Err_Desc, ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetMerchandasingResult(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SessionID As String) As DataTable
        Try
            Return ObjDALReport.GetMerchandasingResult(Err_No, Err_Desc, SessionID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetMerchandasingSessions(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, VanID As String, SurveyID As String, FromDate As String, Todate As String, CustomerId As String, SiteID As String) As DataTable
        Try
            Return ObjDALReport.GetMerchandasingSessions(Err_No, Err_Desc, OrgID, VanID, SurveyID, FromDate, Todate, CustomerId, SiteID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetMerchandasingSessionDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SessionID As String) As DataTable
        Try
            Return ObjDALReport.GetMerchandasingSessionDetails(Err_No, Err_Desc, SessionID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetOrderListingByPaymentType(Err_No As Long, Err_Desc As String, SearchQuery As String, Organization As String, Payment As String, FromDate As String, Todate As String) As DataTable
        Try
            Return ObjDALReport.GetOrderListingByPaymentType(Err_No, Err_Desc, SearchQuery, Organization, Payment, FromDate, Todate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetDeliveryNotes(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustomerID As String, SiteID As String, SalesRepID As String, Fromdate As String, Todate As String, RefNo As String, UserID As String) As DataTable
        Try
            Return ObjDALReport.GetDeliveryNotes(Err_No, Err_Desc, CustomerID, SiteID, SalesRepID, Fromdate, Todate, RefNo, UserID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetWindowServKPI(Err_No As Long, Err_Desc As String, Organization As String, SID As String, Fromdate As String, Todate As String, USerID As String) As DataTable
        Try
            Return ObjDALReport.GetWindowServKPI(Err_No, Err_Desc, Organization, SID, Fromdate, Todate, USerID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetWindowServKPISummary(Err_No As Long, Err_Desc As String, Organization As String, SID As String, Fromdate As String, Todate As String, USerID As String) As DataTable
        Try
            Return ObjDALReport.GetWindowServKPISummary(Err_No, Err_Desc, Organization, SID, Fromdate, Todate, USerID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetCashVanAudit_Asr(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Van As Integer) As DataTable
        Try
            Return ObjDALReport.GetCashVanAudit_Asr(Err_No, Err_Desc, OrgId, Van)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCustomerVanListing(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, ByVal CustID As String) As DataTable
        Try
            Return ObjDALReport.GetCustomerVanListing(Err_No, Err_Desc, OrgId, CustID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetDistributionCheckDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Qry As String, van As String, Fromdate As String, Todate As String, UID As String) As DataTable
        Try
            Return ObjDALReport.GetDistributionCheckDetails(Err_No, Err_Desc, OrgId, Qry, van, Fromdate, Todate, UID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetSalesByProduct(Err_No As Long, Err_Desc As String, SearchQuery As String, Organization As String, Payment As String, van As String, UID As Integer) As DataTable
        Try
            Return ObjDALReport.GetSalesByProduct(Err_No, Err_Desc, SearchQuery, Organization, Payment, van, UID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetEmpIncentive(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, ByVal EMP_CODE As String, ByVal YEAR As String) As DataTable
        Try
            Return ObjDALReport.GetEmpIncentive(Err_No, Err_Desc, OrgId, EMP_CODE, YEAR)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function ExportToExcelMerchandasingResult(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SessionID As String) As DataTable
        Try
            Return ObjDALReport.ExportToExcelMerchandasingResult(Err_No, Err_Desc, SessionID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function ExportToExcelMerchandasingResult_Blk(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal SurveyID As String, ByVal FromDate As String, ByVal Todate As String, ByVal SalesRep_ID As String, ByVal CustID As String, ByVal SiteID As String) As DataTable
        Try
            Return ObjDALReport.ExportToExcelMerchandasingResult_Blk(Err_No, Err_Desc, OrgID, SurveyID, FromDate, Todate, SalesRep_ID, CustID, SiteID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetCreditHistoryDetails(ByRef Err_No As Long, ByRef Err_Desc As String, FromDate As String, ToDate As String, Customer As String, User As String, Org As String, Site As String) As DataTable
        Try
            Return ObjDALReport.GetCreditHistoryDetails(Err_No, Err_Desc, FromDate, ToDate, Customer, User, Org, Site)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetAppControlFlag(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Key As String) As DataTable
        Try
            Return ObjDALReport.GetAppControlFlag(Err_No, Err_Desc, Key)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetMonthDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Opt As Integer, ByVal Org_ID As String, ByVal Incentive_Month As Integer, ByVal Incentive_Year As Integer, ByVal Emp_Code As String, ByVal FromDate As String, ByVal ToDate As String, ByVal Ref_No As String) As DataTable
        Try
            Return ObjDALReport.Rep_GetMonthDetails(Err_No, Err_Desc, Opt, Org_ID, Incentive_Month, Incentive_Year, Emp_Code, FromDate, ToDate, Ref_No)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetReturnByProduct(Err_No As Long, Err_Desc As String, SearchQuery As String, Organization As String, Payment As String) As DataTable
        Try
            Return ObjDALReport.GetReturnByProduct(Err_No, Err_Desc, SearchQuery, Organization, Payment)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSalesVolumeByProductDetails(Err_No As Long, Err_Desc As String, SearchQuery As String, Organization As String, VanID As String, UID As String) As DataTable
        Try
            Return ObjDALReport.GetSalesVolumeByProductDetails(Err_No, Err_Desc, SearchQuery, Organization, VanID, UID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSalesVolumeByProduct(Err_No As Long, Err_Desc As String, SearchQuery As String, Organization As String, VanID As String, UID As String) As DataTable
        Try
            Return ObjDALReport.GetSalesVolumeByProduct(Err_No, Err_Desc, SearchQuery, Organization, VanID, UID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetStockMovementReport(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Item As String, Fromdate As String, Todate As String) As DataTable
        Try
            Return ObjDALReport.GetStockMovementReport(Err_No, Err_Desc, OrgId, SID, Item, Fromdate, Todate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SMLast3MonthsTargetVsAchiev(ByRef Err_No As Long, ByRef Err_Desc As String, OID As String, ByVal SMID As String, ByVal MonthYear As String) As DataTable
        Try
            Return ObjDALReport.SMLast3MonthsTargetVsAchiev(Err_No, Err_Desc, OID, SMID, MonthYear)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetZerobilled_Planned(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Fromdate As String, Todate As String, uid As String) As DataTable
        Try
            Return ObjDALReport.GetZerobilled_Planned(Err_No, Err_Desc, OrgId, SID, Fromdate, Todate, uid)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetZerobilled_NotVisited(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Fromdate As String, Todate As String, uid As String) As DataTable
        Try
            Return ObjDALReport.GetZerobilled_NotVisited(Err_No, Err_Desc, OrgId, SID, Fromdate, Todate, uid)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetZerobilled_NotBilled(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Fromdate As String, Todate As String, uid As String) As DataTable
        Try
            Return ObjDALReport.GetZerobilled_NotBilled(Err_No, Err_Desc, OrgId, SID, Fromdate, Todate, uid)
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Function GetOpenInvoice(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Uid As String, FromDate As String, ToDate As String, Customer As String, Overdue As String) As DataTable
        Try
            Return ObjDALReport.GetOpenInvoice(Err_No, Err_Desc, OrgId, SID, Uid, FromDate, ToDate, Customer, Overdue)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetDailyProductSales(ByRef Err_No As Long, ByRef Err_Desc As String, Orgid As String, SID As String, UID As Integer, FromDate As String, Todate As String, Agency As String, Category As String, InvID As String, CustID As String) As DataTable
        Try
            Return ObjDALReport.GetDailyProductSales(Err_No, Err_Desc, Orgid, SID, UID, FromDate, Todate, Agency, Category, InvID, CustID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetRowIDFromOrigSysDocNo(DocNO As String, ByVal Type As String) As String
        Try
            Return ObjDALReport.GetRowIDFromOrigSysDocNo(DocNO, Type)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSR(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchParams As String, ByVal OrgId As String, FromDate As String, Todate As String) As DataTable
        Try
            Return ObjDALReport.GetSR(Err_No, Err_Desc, SearchParams, OrgId, FromDate, Todate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function Reconciliation(ByRef Err_No As Long, ByRef Err_Desc As String, Orgid As String, SID As String, UID As Integer, FromDate As String, Todate As String) As DataSet
        Try
            Return ObjDALReport.Reconciliation(Err_No, Err_Desc, Orgid, SID, UID, FromDate, Todate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetAssetViewHistory(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, AssetID As String, FromDate As String, Todate As String) As DataTable
        Try
            Return ObjDALReport.GetAssetViewHistory(Err_No, Err_Desc, OrgId, AssetID, FromDate, Todate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetAssetViewHistory_Dates(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, AssetID As String) As DataTable
        Try
            Return ObjDALReport.GetAssetViewHistory_Dates(Err_No, Err_Desc, OrgId, AssetID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetAssetViewHistory_Images(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, AssetID As String) As DataTable
        Try
            Return ObjDALReport.GetAssetViewHistory_Images(Err_No, Err_Desc, OrgID, AssetID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetOrderHeaderDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, RowID As String) As DataTable
        Try
            Return ObjDALReport.GetOrderHeaderDetails(Err_No, Err_Desc, OrgID, RowID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetOrderItemDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, RowID As String) As DataTable
        Try
            Return ObjDALReport.GetOrderItemDetails(Err_No, Err_Desc, OrgID, RowID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetDiscountDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, RowID As String, Type As String) As DataTable
        Try
            Return ObjDALReport.GetDiscountDetails(Err_No, Err_Desc, OrgID, RowID, Type)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetLPOImages(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, RefNo As String) As DataTable
        Try
            Return ObjDALReport.GetLPOImages(Err_No, Err_Desc, OrgID, RefNo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCollectionHeaderDetails(ByRef Err_No As Long, ByRef Err_Desc As String, RowID As String) As DataTable
        Try
            Return ObjDALReport.GetCollectionHeaderDetails(Err_No, Err_Desc, RowID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCollectionInvoiceDetails(ByRef Err_No As Long, ByRef Err_Desc As String, RowID As String) As DataTable
        Try
            Return ObjDALReport.GetCollectionInvoiceDetails(Err_No, Err_Desc, RowID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetChequeImages(ByRef Err_No As Long, ByRef Err_Desc As String, RefNo As String) As DataTable
        Try
            Return ObjDALReport.GetChequeImages(Err_No, Err_Desc, RefNo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetReturnHeaderDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, RowID As String) As DataTable
        Try
            Return ObjDALReport.GetReturnHeaderDetails(Err_No, Err_Desc, OrgID, RowID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetReturnItemDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, RowID As String) As DataTable
        Try
            Return ObjDALReport.GetReturnItemDetails(Err_No, Err_Desc, OrgID, RowID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Function GetSRHeaderDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, RowID As String) As DataTable
        Try
            Return ObjDALReport.GetSRHeaderDetails(Err_No, Err_Desc, OrgID, RowID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSRItemDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, RowID As String) As DataTable
        Try
            Return ObjDALReport.GetSRItemDetails(Err_No, Err_Desc, OrgID, RowID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCustomerfromOrgtextforDiscount(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, Text As String) As DataTable
        Try
            Return ObjDALReport.GetCustomerfromOrgtextforDiscount(Err_No, Err_Desc, OrgId, Text)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'Task 5 Rakesh EOT Report
    Function GetEOTReportDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Fromdate As String) As DataTable
        Try
            Return ObjDALReport.GetEOTReportDetails(Err_No, Err_Desc, OrgId, SID, Fromdate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'Task 5 Rakesh For EOT Reports
    Function GetEOTReport(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, SID As String, Fromdate As String, Todate As String, uid As String) As DataTable
        Try
            Return ObjDALReport.GetEOTReportSummary(Err_No, Err_Desc, OrgId, SID, Fromdate, Todate, uid)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetSalesByProductVatamount(Err_No As Long, Err_Desc As String, SearchQuery As String, Organization As String, Payment As String, van As String, UID As Integer) As DataTable
        Try
            Return ObjDALReport.GetSalesByProductValue(Err_No, Err_Desc, SearchQuery, Organization, Payment, van, UID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
