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
    Function GetCustomerOrderDiscountByID(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustomerID As String, ByVal SiteUseID As String) As DataTable
        Try
            Return ObjDALCustomer.GetCustomerOrderDiscountByID(Err_No, Err_Desc, CustomerID, SiteUseID)
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
    Public Function ExportCustomerBonusPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Try
            Return ObjDALCustomer.ExportCustomerBonusPlan(Err_No, Err_Desc, OrgID)
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
    Function GetCustomersFromSWX(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal FilterName As String, ByVal FilterNo As String) As DataTable
        Try
            Return ObjDALCustomer.GetCustomersFromSWX(Err_No, Err_Desc, OrgID, FilterName, FilterNo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCustomerShipAddress(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_ID As String, ByVal FilterName As String, ByVal FilterNo As String) As DataTable
        Try
            Return ObjDALCustomer.GetCustomerShipAddress(Err_No, Err_Desc, Customer_ID, FilterName, FilterNo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SaveCustomer(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal opt As String, ByRef CustomerID As String, ByRef SiteUseID As String, ByVal CustomerName As String, ByVal CustomerNo As String, ByVal Contact As String, ByVal Location As String, ByVal Address As String, ByVal City As String, ByVal Phone As String, ByVal CashCust As String, ByVal CreditPeriod As String, ByVal CreditLimit As String, ByVal AavailBalance As String, ByVal CreditHold As String, ByVal PriceList As String, ByVal OrgId As String) As Boolean
        Try
            Return ObjDALCustomer.SaveCustomer(Err_No, Err_Desc, opt, CustomerID, SiteUseID, CustomerName, CustomerNo, Contact, Location, Address, Phone, City, CashCust, CreditPeriod, CreditLimit, AavailBalance, CreditHold, PriceList, OrgId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SaveCustomerShipAddress(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal opt As String, ByVal CustomerID As String, ByRef SiteUseID As String, ByVal CustomerName As String, ByVal CustomerNo As String, ByVal Address As String, ByVal PO As String, ByVal City As String, ByVal Customer_Segment_ID As String, ByVal Cust_Lat As String, ByVal Cust_Long As String, ByVal OrgID As String, ByVal Sales_District_ID As String, ByVal Vans As String, ByVal Location As String) As Boolean
        Try
            Return ObjDALCustomer.SaveCustomerShipAddress(Err_No, Err_Desc, opt, CustomerID, SiteUseID, CustomerName, CustomerNo, Address, PO, City, Customer_Segment_ID, Cust_Lat, Cust_Long, OrgID, Sales_District_ID, Vans, Location)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
     Function GetOpenInvoices(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_ID As String, ByVal SiteID As String, ByVal Fromdate As String, ByVal ToDate As String) As DataTable
        Try
            Return ObjDALCustomer.GetOpenInvoices(Err_No, Err_Desc, Customer_ID, SiteID, Fromdate, ToDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
     Function GetOpenReturns(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_ID As String, ByVal SiteID As String, ByVal Fromdate As String, ByVal ToDate As String) As DataTable
        Try
            Return ObjDALCustomer.GetOpenReturns(Err_No, Err_Desc, Customer_ID, SiteID, Fromdate, ToDate)
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
    Public Function SaveCustOrderDiscount(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_ID As String, ByRef Site_Use_ID As String, ByVal MinOrderValue As String, ByVal MinDisc As String, ByVal MaxDisc As String) As Boolean
        Try
            Return ObjDALCustomer.SaveCustOrderDiscount(Err_No, Err_Desc, Customer_ID, Site_Use_ID, MinOrderValue, MinDisc, MaxDisc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
     Public Function SaveOrderLvlCustomerDiscount(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_ID As String, ByRef Site_Use_ID As String, ByVal DiscountType As String, ByVal MinOrderval As String, ByVal Discount As String) As Boolean
        Try
            Return ObjDALCustomer.SaveOrderLvlCustomerDiscount(Err_No, Err_Desc, Customer_ID, Site_Use_ID, DiscountType, MinOrderval, Discount)
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
    Function DeleteBonusPlanToCustomer(ByVal CustID As Integer, ByVal SiteUseID As Integer, ByVal OrgId As String, ByVal PlanId As Integer, ByVal Mode As String, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDALCustomer.DeleteBonusPlanToCustomer(CustID, SiteUseID, OrgId, PlanId, Mode, Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetAvailableCustomersBonusPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PlanId As Integer, ByVal OrgID As String) As DataTable
        Try
            Return ObjDALCustomer.GetAvailableCustomersBonusPlan(Err_No, Err_Desc, PlanId, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function InsertBonusPlanToCustomer(ByVal CustID As Integer, ByVal SiteUseID As Integer, ByVal OrgId As String, ByVal PlanId As Integer, ByVal Mode As String, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDALCustomer.InsertBonusPlanToCustomer(CustID, SiteUseID, OrgId, PlanId, Mode, Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function UploadCustBonusPlan(ByVal dtData As DataTable, ByVal OrgID As String, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserID As String) As Boolean
        Try
            Return ObjDALCustomer.UploadCustBonusPlan(dtData, OrgID, Err_No, Err_Desc, UserID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetAssignedCustomersBonusPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal PlanId As Integer) As DataTable
        Try
            Return ObjDALCustomer.GetAssignedCustomersBonusPlan(Err_No, Err_Desc, OrgID, PlanId)
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
    Public Function CheckBonusPlanIsValid(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal PlanID As String) As Boolean
        Try
            Return ObjDALCustomer.CheckBonusPlanIsValid(Err_No, Err_Desc, OrgID, PlanID)
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
End Class
