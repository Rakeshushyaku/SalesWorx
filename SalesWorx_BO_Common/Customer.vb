Imports System.Data.SqlClient
Imports SalesWorx.BO.DAL
Public Class Customer
    Dim ObjDALCustomer As New DAL_customer
    Function GetCustomerList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal UD_SUB_QRY As String) As DataSet
        Try
            Return ObjDALCustomer.GetCustomerList(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCustomerDiscountDefinition(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustomerID As String, ByVal SiteUseID As String) As DataTable
        Try
            Return ObjDALCustomer.GetCustomerDiscountDefinition(Err_No, Err_Desc, CustomerID, SiteUseID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCustomerNo(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Try
            Return ObjDALCustomer.GetCustomerNo(Err_No, Err_Desc, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCustomerOrderDiscountByID(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustomerID As String, ByVal SiteUseID As String) As DataTable
        Try
            Return ObjDALCustomer.GetCustomerOrderDiscountByID(Err_No, Err_Desc, CustomerID, SiteUseID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function SaveCustVanMap(ByRef Err_No As Long, ByRef Err_Desc As String, ByRef CustomerID As String, ByVal OrgID As Integer, ByRef VanID As String, ByRef AvailBal As Decimal, ByRef DailyInvoice As Decimal) As Boolean

        Try
            Return ObjDALCustomer.SaveCustVanMap(Err_No, Err_Desc, CustomerID, OrgID, VanID, AvailBal, DailyInvoice)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function LoadCustomerVanList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Try
            Return ObjDALCustomer.LoadCustomerVanList(Err_No, Err_Desc, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function UpdateCustomerVanData(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustomerID As String, ByVal VanID As String, ByVal Site_Use_ID As String) As Boolean
        Try
            Return ObjDALCustomer.UpdateCustomerVanData(Err_No, Err_Desc, CustomerID, VanID, Site_Use_ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function ExportCustVanMap(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Try
            Return ObjDALCustomer.ExportCustVanMap(Err_No, Err_Desc, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function LoadCustVanMap(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Try
            Return ObjDALCustomer.LoadCustVanMap(Err_No, Err_Desc, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetCustomerOrderDiscount(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Try
            Return ObjDALCustomer.GetCustomerOrderDiscount(Err_No, Err_Desc, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ExportCustomerBonusPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, PlanType As String, PlanID As String) As DataTable
        Try
            Return ObjDALCustomer.ExportCustomerBonusPlan(Err_No, Err_Desc, OrgID, PlanType, PlanID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ExportCustomerPriceList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, PriceListID As Integer) As DataTable
        Try
            Return ObjDALCustomer.ExportCustomerPriceList(Err_No, Err_Desc, OrgID, PriceListID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetDiscountDefinition(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Try
            Return ObjDALCustomer.GetDiscountDefinition(Err_No, Err_Desc, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCustomerVisits(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal UD_SUB_QRY As String) As DataSet
        Try
            Return ObjDALCustomer.GetCustomerVisits(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCustomerVisitDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal UD_SUB_QRY As String) As DataTable
        Try
            Return ObjDALCustomer.GetCustomerVisitDetails(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function Is_DC_Done(ByVal visitID As String) As Boolean
        Try
            Return ObjDALCustomer.IsDistributionCheckDone(visitID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function HasOrder(ByVal visitID As String) As Boolean
        Try
            Return ObjDALCustomer.HasOrder(visitID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function HasOrderReturn(ByVal visitID As String) As Boolean
        Try
            Return ObjDALCustomer.HasOrderReturn(visitID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetDistributionChecks(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal UD_SUB_QRY As String) As DataSet
        Try
            Return ObjDALCustomer.GetDistributionChecks(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetDistributionCheckDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal UD_SUB_QRY As String) As DataTable
        Try
            Return ObjDALCustomer.GetDistributionCheckDetails(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetDistributionChecksLineItem(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal UD_SUB_QRY As String) As DataSet
        Try
            Return ObjDALCustomer.GetDistributionChecksLineItem(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetOrders(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal UD_SUB_QRY As String) As DataSet
        Try
            Return ObjDALCustomer.GetOrders(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetHeldNewCustomers(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal OrgID As String) As DataTable
        Try
            Return ObjDALCustomer.GetHeldNewCustomers(Err_No, Err_Desc, SearchQuery, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetHeldReceipt(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal InvoiceNo As String) As DataTable
        Try
            Return ObjDALCustomer.GetHeldReceipt(Err_No, Err_Desc, InvoiceNo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function UpdateReleaseOrders(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrderRefNo As String, ByVal UpdatedBy As Integer) As Boolean
        Try
            Return ObjDALCustomer.UpdateReleaseOrder(Err_No, Err_Desc, OrderRefNo, UpdatedBy)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function UpdateReconcileOrders(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrderRefNo As String, ByVal UpdatedBy As Integer, ByVal Remarks As String) As Boolean
        Try
            Return ObjDALCustomer.UpdateReConcileOrder(Err_No, Err_Desc, OrderRefNo, UpdatedBy, Remarks)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetConcileOrderDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrdRefNo As String) As DataTable
        Try
            Return ObjDALCustomer.GetConcileOrderDetails(Err_No, Err_Desc, OrdRefNo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetOrderDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal UD_SUB_QRY As String) As DataTable
        Try
            Return ObjDALCustomer.GetOrderDetails(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetUnconfirmedOrderDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal UD_SUB_QRY As String) As DataTable
        Try
            Return ObjDALCustomer.GetUnconfirmedOrderDetails(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetOrdersLineItem(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal UD_SUB_QRY As String) As DataSet
        Try
            Return ObjDALCustomer.GetOrdersLineItem(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetUnconfirmedOrdersLineItem(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal UD_SUB_QRY As String) As DataSet
        Try
            Return ObjDALCustomer.GetUnconfirmedOrdersLineItem(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetOrdersReturn(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal UD_SUB_QRY As String) As DataSet
        Try
            Return ObjDALCustomer.GetOrdersReturn(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetOrderReturnDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal UD_SUB_QRY As String) As DataTable
        Try
            Return ObjDALCustomer.GetOrderReturnDetails(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetOrdersReturnLineItem(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal UD_SUB_QRY As String) As DataSet
        Try
            Return ObjDALCustomer.GetOrdersReturnLineItem(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetCustomerVisitReport_TotalSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Fromdate As DateTime, ByVal Todate As DateTime, ByVal OrgID As Integer, ByVal SalesRepId As Integer, ByVal CustID As Integer, ByVal UID As Integer) As DataTable
        Try
            Return ObjDALCustomer.GetCustomerVisitReport_TotalSummary(Err_No, Err_Desc, Fromdate, Todate, OrgID, SalesRepId, CustID, UID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetCashCustomerDetailsByVisitID(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal VisitID As String) As DataTable
        Try
            Return ObjDALCustomer.GetCashCustomerDetailsByVisitID(Err_No, Err_Desc, VisitID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetCustomerShipAddressDeatils(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_ID As String, ByVal SiteUseId As String) As DataTable
        Try
            Return ObjDALCustomer.GetCustomerShipAddressDeatils(Err_No, Err_Desc, Customer_ID, SiteUseId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetShipAddressVans(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_ID As String, ByVal SiteUseId As String) As DataTable
        Try
            Return ObjDALCustomer.GetShipAddressVans(Err_No, Err_Desc, Customer_ID, SiteUseId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCustomerDeatils(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_ID As String, ByVal SiteUseId As String) As DataTable
        Try
            Return ObjDALCustomer.GetCustomerDeatils(Err_No, Err_Desc, Customer_ID, SiteUseId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCustomerShipDeatils(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_ID As String, ByVal SiteUseId As String) As DataTable
        Try
            Return ObjDALCustomer.GetCustomerShipDeatils(Err_No, Err_Desc, Customer_ID, SiteUseId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCustomersFromSWX(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal FilterName As String, ByVal FilterNo As String) As DataTable
        Try
            Return ObjDALCustomer.GetCustomersFromSWX(Err_No, Err_Desc, OrgID, FilterName, FilterNo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCustomerShipAddress(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_ID As String, ByVal FilterName As String, ByVal FilterNo As String, Org As String) As DataTable
        Try
            Return ObjDALCustomer.GetCustomerShipAddress(Err_No, Err_Desc, Customer_ID, FilterName, FilterNo, Org)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SaveCustomer(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal opt As String, ByRef CustomerID As String, ByRef SiteUseID As String, ByVal CustomerName As String, ByVal CustomerNo As String, ByVal Contact As String, ByVal Location As String, ByVal Address As String, ByVal City As String, ByVal Phone As String, ByVal CashCust As String, ByVal CreditPeriod As String, ByVal CreditLimit As String, ByVal AavailBalance As String, ByVal CreditHold As String, ByVal PriceList As String, ByVal OrgId As String, ByVal CustType As String, ByVal CustClass As String, ByVal CollectionGrp As String, ByVal Generic_Cash As String, ByVal TRN As String) As Boolean
        Try
            Return ObjDALCustomer.SaveCustomer(Err_No, Err_Desc, opt, CustomerID, SiteUseID, CustomerName, CustomerNo, Contact, Location, Address, Phone, City, CashCust, CreditPeriod, CreditLimit, AavailBalance, CreditHold, PriceList, OrgId, CustType, CustClass, CollectionGrp, Generic_Cash, TRN)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SaveCustomerShipAddress(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal opt As String, ByVal CustomerID As String, ByRef SiteUseID As String, ByVal CustomerName As String, ByVal CustomerNo As String, ByVal Address As String, ByVal PO As String, ByVal City As String, ByVal Customer_Segment_ID As String, ByVal Cust_Lat As String, ByVal Cust_Long As String, ByVal Cust_Status As String, ByVal OrgID As String, ByVal Sales_District_ID As String, ByVal Vans As String, ByVal Location As String, ByVal Customer_Barcode As String, ByVal Beacon_UUID As String, ByVal Beacon_Major As String, ByVal Beacon_Minor As String) As Boolean
        Try
            Return ObjDALCustomer.SaveCustomerShipAddress(Err_No, Err_Desc, opt, CustomerID, SiteUseID, CustomerName, CustomerNo, Address, PO, City, Customer_Segment_ID, Cust_Lat, Cust_Long, Cust_Status, OrgID, Sales_District_ID, Vans, Location, Customer_Barcode, Beacon_UUID, Beacon_Major, Beacon_Minor)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetOpenInvoices(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_ID As String, ByVal SiteID As String, ByVal Fromdate As String, ByVal ToDate As String, RefNo As String) As DataTable
        Try
            Return ObjDALCustomer.GetOpenInvoices(Err_No, Err_Desc, Customer_ID, SiteID, Fromdate, ToDate, RefNo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetOpenReturns(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_ID As String, ByVal SiteID As String, ByVal Fromdate As String, ByVal ToDate As String, RefNo As String) As DataTable
        Try
            Return ObjDALCustomer.GetOpenReturns(Err_No, Err_Desc, Customer_ID, SiteID, Fromdate, ToDate, RefNo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SaveSettlement(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_ID As String, ByRef Site_Use_ID As String, ByRef userid As String, ByVal Doc_No_1 As String, ByVal Doc_No_2 As String, ByVal Settlement_Amount As String, ByVal Actual_Amount_1 As String, ByVal Actual_Amount_2 As String) As Boolean
        Try
            Return ObjDALCustomer.SaveSettlement(Err_No, Err_Desc, Customer_ID, Site_Use_ID, userid, Doc_No_1, Doc_No_2, Settlement_Amount, Actual_Amount_1, Actual_Amount_2)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SaveCustOrderDiscount(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_ID As String, ByRef Site_Use_ID As String, ByVal MinOrderValue As String, ByVal MinDisc As String, ByVal MaxDisc As String, TransactionType As String) As Boolean
        Try
            Return ObjDALCustomer.SaveCustOrderDiscount(Err_No, Err_Desc, Customer_ID, Site_Use_ID, MinOrderValue, MinDisc, MaxDisc, TransactionType)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SaveOrderLvlCustomerDiscount(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_ID As String, ByRef Site_Use_ID As String, ByVal DiscountType As String, ByVal MinOrderval As String, ByVal Discount As String, TransactionType As String) As Boolean
        Try
            Return ObjDALCustomer.SaveOrderLvlCustomerDiscount(Err_No, Err_Desc, Customer_ID, Site_Use_ID, DiscountType, MinOrderval, Discount, TransactionType)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function DeleteDiscountDefinition(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_ID As String, ByRef Site_Use_ID As String) As Boolean
        Try
            Return ObjDALCustomer.DeleteDiscountDefinition(Err_No, Err_Desc, Customer_ID, Site_Use_ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function DeleteCustOrderDiscount(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_ID As String, ByRef Site_Use_ID As String) As Boolean
        Try
            Return ObjDALCustomer.DeleteCustOrderDiscount(Err_No, Err_Desc, Customer_ID, Site_Use_ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function DeleteBonusPlanToCustomer(ByVal CustID As Integer, ByVal SiteUseID As Integer, ByVal OrgId As String, ByVal PlanId As Integer, ByVal Mode As String, ByRef Err_No As Long, ByRef Err_Desc As String, PlanType As String) As Boolean
        Try
            Return ObjDALCustomer.DeleteBonusPlanToCustomer(CustID, SiteUseID, OrgId, PlanId, Mode, Err_No, Err_Desc, PlanType)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function DeletePriceListToCustomer(ByVal CustID As Integer, ByVal SiteUseID As Integer, ByVal OrgId As String, ByVal PriceListID As Integer, ByVal Mode As String, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDALCustomer.DeletePriceListToCustomer(CustID, SiteUseID, OrgId, PriceListID, Mode, Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetAvailableCustomersBonusPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PlanId As Integer, ByVal OrgID As String, PlanType As String) As DataTable
        Try
            Return ObjDALCustomer.GetAvailableCustomersBonusPlan(Err_No, Err_Desc, PlanId, OrgID, PlanType)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetAvailableCustomersPriceList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PriceListID As Integer, ByVal OrgID As String) As DataTable
        Try
            Return ObjDALCustomer.GetAvailableCustomersPriceList(Err_No, Err_Desc, PriceListID, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function InsertBonusPlanToCustomer(ByVal CustID As Integer, ByVal SiteUseID As Integer, ByVal OrgId As String, ByVal PlanId As Integer, ByVal Mode As String, ByRef Err_No As Long, ByRef Err_Desc As String, ByRef DtError As DataTable, PlanType As String) As Boolean
        Try
            Return ObjDALCustomer.InsertBonusPlanToCustomer(CustID, SiteUseID, OrgId, PlanId, Mode, Err_No, Err_Desc, DtError, PlanType)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function InsertPriceListToCustomer(ByVal CustID As Integer, ByVal SiteUseID As Integer, ByVal OrgId As String, ByVal PriceListID As Integer, ByVal Mode As String, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDALCustomer.InsertPriceListToCustomer(CustID, SiteUseID, OrgId, PriceListID, Mode, Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function UploadCustBonusPlan(ByVal dtData As DataTable, ByVal OrgID As String, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserID As String, ByRef DtError As DataTable, PlanType As String) As Boolean
        Try
            Return ObjDALCustomer.UploadCustBonusPlan(dtData, OrgID, Err_No, Err_Desc, UserID, DtError, PlanType)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function UploadCustPriceList(ByVal dtData As DataTable, ByVal OrgID As String, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserID As String) As Boolean
        Try
            Return ObjDALCustomer.UploadCustPriceList(dtData, OrgID, Err_No, Err_Desc, UserID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetAssignedCustomersBonusPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal PlanId As Integer, PlanType As String) As DataTable
        Try
            Return ObjDALCustomer.GetAssignedCustomersBonusPlan(Err_No, Err_Desc, OrgID, PlanId, PlanType)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetAssignedCustomersPriceList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal PriceListID As Integer) As DataTable
        Try
            Return ObjDALCustomer.GetAssignedCustomersPriceList(Err_No, Err_Desc, OrgID, PriceListID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function UploadCustOrderDiscount(ByVal dtData As DataTable, ByVal OrgID As String, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserID As String) As Boolean
        Try
            Return ObjDALCustomer.UploadCustOrderDiscount(dtData, OrgID, Err_No, Err_Desc, UserID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function CheckCustomerShipExist(ByVal OrgId As String, ByVal CustomerNo As String, ByRef Customer_ID As String, ByRef Site_ID As String) As Boolean
        Try
            Return ObjDALCustomer.CheckCustomerShipExist(OrgId, CustomerNo, Customer_ID, Site_ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckCustomerNoExist(ByVal OrgId As String, ByVal CustomerNo As String, ByRef Customer_ID As String, ByRef Site_ID As String) As Boolean
        Try
            Return ObjDALCustomer.CheckCustomerNoExist(OrgId, CustomerNo, Customer_ID, Site_ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckOrgShipCustomerNoExist(ByVal OrgId As String, ByVal CustomerNo As String, ByRef Customer_ID As String, ByRef Site_ID As String) As Boolean
        Try
            Return ObjDALCustomer.CheckOrgShipCustomerNoExist(OrgId, CustomerNo, Customer_ID, Site_ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckBonusPlanIsValid(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal PlanID As String, PlanType As String) As Boolean
        Try
            Return ObjDALCustomer.CheckBonusPlanIsValid(Err_No, Err_Desc, OrgID, PlanID, PlanType)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckPriceListIsValid(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal PriceListID As String) As Boolean
        Try
            Return ObjDALCustomer.CheckPriceListIsValid(Err_No, Err_Desc, OrgID, PriceListID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function UploadDiscount(ByVal dtData As DataTable, ByVal OrgID As String, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserID As String) As Boolean
        Try
            Return ObjDALCustomer.UploadDiscount(dtData, OrgID, Err_No, Err_Desc, UserID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckDiscountPlanIsValid(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal PlanID As String) As Boolean
        Try
            Return ObjDALCustomer.CheckDiscountPlanIsValid(Err_No, Err_Desc, OrgID, PlanID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function UploadCustDiscountPlan(ByVal dtData As DataTable, ByVal OrgID As String, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserID As String, ByRef DtError As DataTable) As Boolean
        Try
            Return ObjDALCustomer.UploadCustDiscountPlan(dtData, OrgID, Err_No, Err_Desc, UserID, DtError)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ExportCustomerDiscountPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Try
            Return ObjDALCustomer.ExportCustomerDiscountPlan(Err_No, Err_Desc, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetAvailableCustomersDiscountPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PlanId As Integer, ByVal OrgID As String) As DataTable
        Try
            Return ObjDALCustomer.GetAvailableCustomersDiscountPlan(Err_No, Err_Desc, PlanId, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetAssignedCustomersDiscountPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal PlanId As Integer) As DataTable
        Try
            Return ObjDALCustomer.GetAssignedCustomersDiscountPlan(Err_No, Err_Desc, OrgID, PlanId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function InsertDiscountPlanToCustomer(ByVal CustID As Integer, ByVal SiteUseID As Integer, ByVal OrgId As String, ByVal PlanId As Integer, ByVal Mode As String, ByRef Err_No As Long, ByRef Err_Desc As String, ByRef DtError As DataTable) As Boolean
        Try
            Return ObjDALCustomer.InsertDiscountPlanToCustomer(CustID, SiteUseID, OrgId, PlanId, Mode, Err_No, Err_Desc, DtError)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function DeleteDiscountPlanToCustomer(ByVal CustID As Integer, ByVal SiteUseID As Integer, ByVal OrgId As String, ByVal PlanId As Integer, ByVal Mode As String, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDALCustomer.DeleteDiscountPlanToCustomer(CustID, SiteUseID, OrgId, PlanId, Mode, Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetCustomerDetails(ByRef Err_No As Long, ByRef Err_Desc As String, OrgID As String) As DataTable
        Try
            Return ObjDALCustomer.GetCustomerDetails(Err_No, Err_Desc, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetOrganisationByName(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDALCustomer.GetOrganisationByName(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCustomerListfromOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByRef OrgId As String) As DataTable
        Try
            Return ObjDALCustomer.GetCustomerListfromOrg(Err_No, Err_Desc, OrgId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCustomerTypefromOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByRef OrgId As String) As DataTable
        Try
            Return ObjDALCustomer.GetCustomerTypefromOrg(Err_No, Err_Desc, OrgId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckCustVanExists(ByRef Err_No As Long, ByRef Err_Desc As String, ByRef Customer_ID As Integer, ByVal VanID As Integer, ByVal OrgID As String) As Boolean
        Try
            Return ObjDALCustomer.CheckCustVanExists(Err_No, Err_Desc, Customer_ID, VanID, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckCustomerId(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustomerID As String, ByVal OrgID As String) As Boolean
        Try
            Return ObjDALCustomer.CheckCustomerId(Err_No, Err_Desc, CustomerID, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckCustomerName(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustomerName As String, ByVal OrgID As String) As Boolean
        Try
            Return ObjDALCustomer.CheckCustomerName(Err_No, Err_Desc, CustomerName, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetOrganisation(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UD_SUB_QRY As String) As DataTable
        Try
            Return ObjDALCustomer.GetOrganisation(Err_No, Err_Desc, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetBlockingParamsforCust(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal OrgID As String, ByVal CustomerName As String, ByVal CustomerNo As String) As DataTable
        Try
            Return ObjDALCustomer.GetBlockingParamsforCust(Error_No, Error_Desc, OrgID, CustomerName, CustomerNo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function UpdateBlockingCriteria(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustID As Integer, ByVal SiteUseID As Integer, ByVal BlockCriteria As String) As Boolean
        Try
            Return ObjDALCustomer.UpdateBlockingCriteria(Err_No, Err_Desc, CustID, SiteUseID, BlockCriteria)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetTradeImages(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal VisitID As String) As DataTable
        Try
            Return ObjDALCustomer.GetTradeImages(Err_No, Err_Desc, VisitID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function LoadExportCustomersTemplate(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal FilterName As String, ByVal FilterNo As String) As DataSet
        Try
            Return ObjDALCustomer.LoadExportCustomersTemplate(Err_No, Err_Desc, OrgID, FilterName, FilterNo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function IsValidPriceList_code(PriceList_Code As String) As Boolean
        Return ObjDALCustomer.IsValidPriceList_code(PriceList_Code)
    End Function
    Public Function IsValidCustomerClass(ByVal Customer_class As String) As Boolean
        Return ObjDALCustomer.IsValidCustomerClass(Customer_class)
    End Function
    Public Function IsValidCustomerType(ByVal Customer_type As String) As Boolean
        Return ObjDALCustomer.IsValidCustomerType(Customer_type)
    End Function
    Public Function IsValidCustomerSegment(ByVal Customer_Segment_Code As String) As Boolean
        Return ObjDALCustomer.IsValidCustomerSegment(Customer_Segment_Code)
    End Function
    Public Function IsValidCustomerDistrict(ByVal Sales_District_Code As String) As Boolean
        Return ObjDALCustomer.IsValidCustomerDistrict(Sales_District_Code)
    End Function
    Public Function IsValidOrganization(ByVal ORG_ID As String) As Boolean
        Return ObjDALCustomer.IsValidOrganization(ORG_ID)
    End Function
    Function GetCustomerPriceList_ID(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PriceList_Code As String) As DataTable
        Try
            Return ObjDALCustomer.GetCustomerPriceList_ID(Err_No, Err_Desc, PriceList_Code)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCustomerSales_District_ID(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Sales_District_Code As String) As DataTable
        Try
            Return ObjDALCustomer.GetCustomerSales_District_ID(Err_No, Err_Desc, Sales_District_Code)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCustomerSegment_ID(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Segment_Code As String) As DataTable
        Try
            Return ObjDALCustomer.GetCustomerSegment_ID(Err_No, Err_Desc, Segment_Code)
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Function SaveCustomerCreditLimit(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Trx_ID As String, ByRef CustomerID As String, ByRef SiteUseID As String, ByVal Credit_Limit As String, ByVal Avail_Bal As String, ByVal Previous_Credit_Limit As String, ByVal Previous_Avail_Bal As String, ByVal Comment As String, ByVal Created_By As String) As Boolean
        Try
            Return ObjDALCustomer.SaveCustomerCreditLimit(Err_No, Err_Desc, Trx_ID, CustomerID, SiteUseID, Credit_Limit, Avail_Bal, Previous_Credit_Limit, Previous_Avail_Bal, Comment, Created_By)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CustomerNoExists(ByVal CustomerNo As String, ByVal OrgID As String) As Boolean
        Return ObjDALCustomer.CustomerNoExists(CustomerNo, OrgID)
    End Function
    Function GetCustomer(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustomerNo As String, ByVal OrgID As String) As DataTable
        Try
            Return ObjDALCustomer.GetCustomer(Err_No, Err_Desc, CustomerNo, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function ExistsCustomerShipAddress(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustomerNo As String, ByVal OrgID As String, Customer_Name As String) As DataTable
        Try
            Return ObjDALCustomer.ExistsCustomerShipAddress(Err_No, Err_Desc, CustomerNo, OrgID, Customer_Name)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetCustomerOpenInvoices(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_ID As String, ByVal SiteUseId As String) As DataTable
        Try
            Return ObjDALCustomer.GetCustomerOpenInvoices(Err_No, Err_Desc, Customer_ID, SiteUseId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function CheckEnableCreditforCashCust(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDALCustomer.CheckEnableCreditforCashCust(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetDistribution_ctl(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal SID As String, Customer_ID As String, SiteUseId As String) As DataTable
        Try
            Return ObjDALCustomer.GetDistribution_ctl(Err_No, Err_Desc, OrgID, SID, Customer_ID, SiteUseId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCustfromOrgtext_Distribution_ctl(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, ByVal SID As String, Text As String) As DataTable
        Try
            Return ObjDALCustomer.GetCustfromOrgtext_Distribution_ctl(Err_No, Err_Desc, OrgId, SID, Text)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function SaveDistribution_CTL(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal OrgId As String, ByVal SID As String, ByVal CustomerID As String, ByRef SiteUSeID As String, ByVal Is_Optional As String, ByVal CreatedBy As Integer) As Boolean
        Try
            Return ObjDALCustomer.SaveDistribution_CTL(Error_No, Error_Desc, OrgId, SID, CustomerID, SiteUSeID, Is_Optional, CreatedBy)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function DeleteDistribution_CTL(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, Distribution_CTL_ID As String) As Boolean
        Try
            Return ObjDALCustomer.DeleteDistribution_CTL(Err_No, Err_Desc, OrgID, Distribution_CTL_ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function ValidateExportDistribution_ctl(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_No As String, ByVal Site_No As String, ByVal SalesRep_Number As String, ByVal Opt As String) As DataTable
        Try
            Return ObjDALCustomer.ValidateExportDistribution_ctl(Err_No, Err_Desc, Customer_No, Site_No, SalesRep_Number, Opt)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function ValidateExportDistribution_ctl_FSR(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRep_Number As String) As DataTable
        Try
            Return ObjDALCustomer.ValidateExportDistribution_ctl_FSR(Err_No, Err_Desc, SalesRep_Number)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetReceiptMethods(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SID As String) As DataTable
        Try
            Return ObjDALCustomer.GetReceiptMethods(Err_No, Err_Desc, SID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Function GetBankAccounts(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SID As String) As DataTable
        Try
            Return ObjDALCustomer.GetBankAccounts(Err_No, Err_Desc, SID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
