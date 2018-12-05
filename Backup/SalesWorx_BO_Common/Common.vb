Imports System.Data.SqlClient
Imports SalesWorx.BO.DAL
Public Class Common
    Dim ObjDALCommon As New DAL_Common
    Function GetOrganisation(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UD_SUB_QRY As String) As DataTable
        Try
            Return ObjDALCommon.GetOrganisation(Err_No, Err_Desc, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetERPSyncTable(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDALCommon.GetERPSyncTable(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSyncType(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDALCommon.GetSyncType(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function LoadOrgLogo(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDALCommon.LoadOrgLogo(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetDiscountRule(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDALCommon.GetDiscountRule(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetOrgsWthNoDiscount(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDALCommon.GetOrgsWthNoDiscount(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetAppConfig(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Control_Key As String) As String
        Try
            Return ObjDALCommon.GetAppConfig(Err_No, Err_Desc, Control_Key)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetOrgLogoPath(ByRef Err_No As Long, ByRef Err_Desc As String) As String
        Try
            Return ObjDALCommon.GetOrgLogoPath(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetLogoPath(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Type As String) As String
        Try
            Return ObjDALCommon.GetLogoPath(Err_No, Err_Desc, Type)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

     Function DeleteClientLogo(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RowId As String) As Boolean
        Try
            Return ObjDALCommon.DeleteClientLogo(Err_No, Err_Desc, RowId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function DeleteCustomInfo(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RowId As String) As Boolean
        Try
            Return ObjDALCommon.DeleteCustomInfo(Err_No, Err_Desc, RowId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function SaveOrgLogo(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal OrgId As String, ByVal LogoPath As String, ByVal CreatedBy As Integer) As Boolean
        Try
            Return ObjDALCommon.SaveorgLogo(Error_No, Error_Desc, OrgId, LogoPath, CreatedBy)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetClientLogo(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RowId As String) As DataTable
        Try
            Return ObjDALCommon.GetClientLogo(Err_No, Err_Desc, RowId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetVansWthNoLogos(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDALCommon.GetVansWthNoLogos(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetClientsWthNoLogos(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDALCommon.GetClientsWthNoLogos(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetOrgsWthNoLogos(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDALCommon.GetOrgsWthNoLogos(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetPrintHeaders(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Type As String, ByVal filter As String) As DataTable
        Try
            Return ObjDALCommon.GetPrintHeaders(Err_No, Err_Desc, Type, filter)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetClientLogos(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal client As String) As DataTable
        Try
            Return ObjDALCommon.GetClientLogos(Err_No, Err_Desc, client)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function SaveClientLogo(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal Client As String, ByVal Line1 As String, ByVal Line2 As String, ByVal Line3 As String, ByVal CreatedBy As Integer) As Boolean
        Try
            Return ObjDALCommon.SaveClientLogo(Error_No, Error_Desc, Client, Line1, Line2, Line3, CreatedBy)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function SaveDiscountRule(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal Org As String, ByVal Min As String, ByVal max As String, ByVal Value1 As String, ByVal CreatedBy As String) As Boolean
        Try
            Return ObjDALCommon.SaveDiscountRule(Error_No, Error_Desc, Org, Min, max, Value1, CreatedBy)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
     
    Function SavePrintHeader(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal Client As String, ByVal Line1 As String, ByVal Line2 As String, ByVal Line3 As String, ByVal CreatedBy As Integer, ByVal Logo As String, ByVal Type As String, ByVal Mode As String, ByVal Image4inch As String) As Boolean
        Try
            Return ObjDALCommon.SavePrintHeader(Error_No, Error_Desc, Client, Line1, Line2, Line3, CreatedBy, Logo, Type, Mode, Image4inch)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetCustomerSegmentList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Query As String) As DataTable
        Return ObjDALCommon.GetCustomerSegmentList(Err_No, Err_Desc, Query)
    End Function
    Function GetCustomer(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Query As String) As DataTable
        Return ObjDALCommon.GetCustomer(Err_No, Err_Desc, Query)
    End Function
    Function GetCustomerfromOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Return ObjDALCommon.GetCustomerfromOrg(Err_No, Err_Desc, OrgID)
    End Function
    Function GetCustomerShipfromOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Return ObjDALCommon.GetCustomerShipfromOrg(Err_No, Err_Desc, OrgID)
    End Function
    Function GetCustomerTypeList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Query As String) As DataTable
        Return ObjDALCommon.GetCustomerTypeList(Err_No, Err_Desc, Query)
    End Function
    Function GetCustomerClass(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Query As String) As DataTable
        Return ObjDALCommon.GetCustomerClass(Err_No, Err_Desc, Query)
    End Function
    Function GetCollectionTypeList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Query As String) As DataTable
        Return ObjDALCommon.GetCollectionTypeList(Err_No, Err_Desc, Query)
    End Function
    Function GetSalesDistrictList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Query As String) As DataTable
        Return ObjDALCommon.GetSalesDistrictList(Err_No, Err_Desc, Query)
    End Function
    Function GetAllVan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Query As String) As DataTable
        Return ObjDALCommon.GetAllVan(Err_No, Err_Desc, Query)
    End Function

    Function GetDeviceConfig(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesrepID As String) As DataTable
        Return ObjDALCommon.GetDeviceConfig(Err_No, Err_Desc, SalesRepID)
    End Function

    Function UpdateDeviceConfig(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesrepID As String, ByVal RowID As String, ByVal Value As String, ByVal Updatedby As Integer) As Boolean
        Return ObjDALCommon.UpdateDeviceConfig(Err_No, Err_Desc, SalesrepID, RowID, Value, Updatedby)
    End Function

    Function GetVanByOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Id As String) As DataTable
        Return ObjDALCommon.GetVanByOrg(Err_No, Err_Desc, OrgID, Id)
    End Function
     Function GetVanByOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Id As String, ByVal filter As String) As DataTable
        Return ObjDALCommon.GetVanByOrg(Err_No, Err_Desc, OrgID, Id, filter)
    End Function
    Function GetVanByOrgForSync(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Id As String) As DataTable
        Return ObjDALCommon.GetVanByOrgforSync(Err_No, Err_Desc, OrgID, Id)
    End Function
    Function GetVanFromMultipleOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Id As String) As DataTable
        Return ObjDALCommon.GetVanFromMultipleOrg(Err_No, Err_Desc, OrgID, Id)
    End Function
    Function GetAgencyByOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Id As String) As DataTable
        Return ObjDALCommon.GetAgencyByOrg(Err_No, Err_Desc, OrgID, Id)
    End Function

    Public Function GetDistinct(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserId As Integer) As DataTable
        Dim SiteTbl As New DataTable
        Dim ObjDAlRoutePlanner As New DAL_RoutePlan
        Try
            SiteTbl = ObjDAlRoutePlanner.GetDistinctSite(Err_No, Err_Desc, UserId)
            Return SiteTbl
        Catch ex As Exception
            Throw ex
        Finally
            SiteTbl = Nothing
            ObjDAlRoutePlanner = Nothing
        End Try
    End Function
    Function GetPriceTypeList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal Query As String) As DataTable
        Return ObjDALCommon.GetPriceTypeList(Err_No, Err_Desc, SearchQuery, Query)
    End Function

    Function GetFSRVisitedDates(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRepID As Integer) As DataTable
        Return ObjDALCommon.GetFSRVisitedDates(Err_No, Err_Desc, SalesRepID)
    End Function

    Public Function GetSalesRepQry(ByVal Id As String) As String
        Return "Select SalesRep_Id from app_GetControlInfo(" & Id & ")"
    End Function
    Public Function GetVanByOrg(ByVal Id As String, ByVal OrgID As Integer) As String
        Return "Select SalesRep_Id from app_GetControlInfo(" & Id & ") WHERE MAS_Org_ID='" & OrgID.ToString() & "'"
    End Function
    Public Function GetMinTransDate(ByRef Err_No As Long, ByRef Err_Desc As String) As String
        Return ObjDALCommon.GetMinTransDate(Err_No, Err_Desc)
    End Function

    Function GetAllProducts(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Return ObjDALCommon.GetAllProducts(Err_No, Err_Desc)
    End Function

    Function GetAllProductsByOrgID(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As Integer) As DataTable
        Return ObjDALCommon.GetAllProductsByOrg(Err_No, Err_Desc, OrgID)
    End Function
    Function GetAllProductsByOrg_Agency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As Integer, ByVal Agency As String) As DataTable
        Return ObjDALCommon.GetAllProductsByOrg_Agency(Err_No, Err_Desc, OrgID, Agency)
    End Function
    Function GetAllProductsByOrg_Van(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As Integer, ByVal Van As String) As DataTable
        Return ObjDALCommon.GetAllProductsByOrg_Van(Err_No, Err_Desc, OrgID, Van)
    End Function
    Function GetAllProductsByOrg_Van_Agency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As Integer, ByVal Van As String, ByVal Agency As String) As DataTable
        Return ObjDALCommon.GetAllProductsByOrg_Van_Agency(Err_No, Err_Desc, OrgID, Van, Agency)
    End Function
    Function GetAllUOM(ByRef Err_No As Long, ByRef Err_Desc As String, Optional ByVal Inventory_item_ID As Integer = 0) As DataTable
        Return ObjDALCommon.GetAllUOM(Err_No, Err_Desc, Inventory_item_ID)
    End Function
    Function GetAllAgency(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Return ObjDALCommon.GetAllAgency(Err_No, Err_Desc)
    End Function
    Function GetAllAgencyByOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As Integer) As DataTable
        Return ObjDALCommon.GetAllAgencyByOrg(Err_No, Err_Desc, OrgID)
    End Function
    Function GetAllAgencyByOrg_Van(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As Integer, ByVal Van As String) As DataTable
        Return ObjDALCommon.GetAllAgencyByOrg_Van(Err_No, Err_Desc, OrgID, Van)
    End Function
    Function GetVanAuditReport(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QueryStr As String) As DataTable
        Return ObjDALCommon.GetVanAuditReport(Err_No, Err_Desc, QueryStr)
    End Function
    Function GetPrevAuditDate(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal DateofAudit As String, ByVal SalesRepID As String) As String
        Return ObjDALCommon.GetPrev_AuditDate(Err_No, Err_Desc, DateofAudit, SalesRepID)
    End Function
    Function GetCurrencyByOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As Integer) As DataTable
        Return ObjDALCommon.GetCurrencyByOrg(Err_No, Err_Desc, OrgID)
    End Function
    Function GetCustomerByCriteria(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QueryStr As String) As DataTable
        Return ObjDALCommon.GetCustomerByCriteria(Err_No, Err_Desc, QueryStr)
    End Function

    Function GetCustomerLocation(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Return ObjDALCommon.GetCustomerLocation(Err_No, Err_Desc)
    End Function
    Function GetOutlet(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Return ObjDALCommon.GetOutlet(Err_No, Err_Desc)
    End Function
    Function GetOutlet(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Return ObjDALCommon.GetOutlet(Err_No, Err_Desc, OrgID)
    End Function
    Function GetSKU(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Return ObjDALCommon.GetSKU(Err_No, Err_Desc)
    End Function
    Function GetSKU(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String) As DataTable
        Return ObjDALCommon.GetSKU(Err_No, Err_Desc, Org_ID)
    End Function
    Function GetAgencyList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String) As DataSet
        Return ObjDALCommon.GetAgencyList(Err_No, Err_Desc, Org_ID)
    End Function
    Function GetProductsByOrg_Agency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String, Optional ByVal Agency As String = "0") As DataTable
        Return ObjDALCommon.GetProductsByOrg_Agency(Err_No, Err_Desc, Org_ID, Agency)
    End Function
    Function GetProductsByOrgAgency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String, Optional ByVal Agency As String = "0") As DataTable
        Return ObjDALCommon.GetProductsByOrg_Agency(Err_No, Err_Desc, Org_ID, Agency)
    End Function
    Function GetSalesOrgbyFsr(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Salerep_ID As String) As DataTable
        Return ObjDALCommon.GetSalesOrgbyFsr(Err_No, Err_Desc, Salerep_ID)
    End Function
    Function GetProductsByOrg_Agency_UOM(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String, Optional ByVal Agency As String = "0", Optional ByVal UOM As String = "0") As DataTable
        Return ObjDALCommon.GetProductsByOrg_Agency_UOM(Err_No, Err_Desc, Org_ID, Agency, UOM)
    End Function
    Function GetCustomerSegments(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SID As String) As DataTable
        Return ObjDALCommon.GetCustomerSegments(Err_No, Err_Desc, SID)
    End Function
    Function GetYear(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Return ObjDALCommon.GetYear(Err_No, Err_Desc)
    End Function
    Function GetYearForRevDispersion(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Return ObjDALCommon.GetYearForRevDispersion(Err_No, Err_Desc)
    End Function
    Function GetYearForReceivables(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Return ObjDALCommon.GetYearForReceivables(Err_No, Err_Desc)
    End Function
    Public Function GetDayEndTime(ByRef Err_No As Long, ByRef Err_Desc As String) As String
        Return ObjDALCommon.GetDayEndTime(Err_No, Err_Desc)
    End Function

    Public Function GetMonths(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Return ObjDALCommon.GetMonths(Err_No, Err_Desc)
    End Function
    Public Function GetYear_Distribution(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Return ObjDALCommon.GetYear_Distribution(Err_No, Err_Desc)
    End Function

    Public Function GetFsrCustRelation(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Return ObjDALCommon.GetFsrCustRelation(Err_No, Err_Desc)
    End Function

 Public Function GetSummaryforExport(ByRef Err_No As Long, ByRef Err_Desc As String, ByRef Fromdate As String, ByVal Todate As String) As DataSet
        Try
            Return ObjDALCommon.GetSummaryforExport(Err_No, Err_Desc, Fromdate, Todate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetCustomerVisitMonth(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Dim SiteTbl As New DataTable
        Dim ObjDAlRoutePlanner As New DAL_RoutePlan
        Try
            SiteTbl = ObjDAlRoutePlanner.GetCustomerVisitMonth(Err_No, Err_Desc, OrgID)
            Return SiteTbl
        Catch ex As Exception
            Throw ex
        Finally
            SiteTbl = Nothing
            ObjDAlRoutePlanner = Nothing
        End Try
    End Function
     Function GetYearforMonthlySales(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Return ObjDALCommon.GetYearforMonthlySales(Err_No, Err_Desc)
    End Function
     Function GetYearforLoad(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Return ObjDALCommon.GetYearforLoad(Err_No, Err_Desc)
    End Function
    Function GetYearforOrder(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Return ObjDALCommon.GetYearforOrder(Err_No, Err_Desc)
    End Function
    Function GetArea(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Return ObjDALCommon.GetArea(Err_No, Err_Desc)
    End Function
    Function GetCustomerLocationByOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Return ObjDALCommon.GetCustomerLocationByOrg(Err_No, Err_Desc, OrgID)
    End Function
    Function GetCustomerfromOrg_Loc(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Location As String) As DataTable
        Return ObjDALCommon.GetCustomerfromOrg_Loc(Err_No, Err_Desc, OrgID, Location)
    End Function
End Class
