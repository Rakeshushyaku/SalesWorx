Imports System.Data.SqlClient
Imports SalesWorx.BO.DAL
Public Class Product
    Dim ObjDALProduct As New DAL_Product
    Function GetProductList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal UD_SUB_QRY As String) As DataSet
        Try
            Return ObjDALProduct.GetProductList(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetBonusData(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal OrgID As String, ByVal ShowInactive As String, ByVal BnsPlanID As String) As DataTable
        Try
            Return ObjDALProduct.LoadBonusData(Err_No, Err_Desc, ItemCode, OrgID, ShowInactive, BnsPlanID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckValidFSRID(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRepID As String, ByVal OrgID As String) As Boolean
        Try
            Return ObjDALProduct.CheckValidFSRID(Err_No, Err_Desc, SalesRepID, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetPriceListData(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal ItemID As String, ByVal PriceListID As String) As DataTable
        Try
            Return ObjDALProduct.GetPriceListData(Err_No, Err_Desc, OrgID, ItemID, PriceListID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetProductImages(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, Inventory_Item_ID As String) As DataTable
        Try
            Return ObjDALProduct.GetProductImages(Err_No, Err_Desc, OrgID, Inventory_Item_ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetItemUOMs(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal OrgID As String) As DataTable
        Try
            Return ObjDALProduct.GetItemUOMs(Err_No, Err_Desc, ItemCode, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetItemUOMsWithPrice(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal OrgID As String, PriceList As String) As DataTable
        Try
            Return ObjDALProduct.GetItemUOMsWithPrice(Err_No, Err_Desc, ItemCode, OrgID, PriceList)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetProdcutPrice(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal OrgID As String, PriceList As String, UOM As String) As DataTable
        Try
            Return ObjDALProduct.GetProdcutPrice(Err_No, Err_Desc, ItemCode, OrgID, PriceList, UOM)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetDiscountData(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal PlanID As String) As DataTable
        Try
            Return ObjDALProduct.LoadDiscountData(Err_No, Err_Desc, OrgID, PlanID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckPriceListName(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal PriceListName As String) As Boolean
        Try
            Return ObjDALProduct.CheckPriceListName(Err_No, Err_Desc, OrgID, PriceListName)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckPriceDataExists(ByRef Err_No As Long, ByRef Err_Desc As String, ItemID As Integer, ByVal OrgID As String, ByVal PriceListID As String, UOM As String) As Boolean
        Try
            Return ObjDALProduct.CheckPriceDataExists(Err_No, Err_Desc, ItemID, OrgID, PriceListID, UOM)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function CheckRedemptionItemExists(ByRef Err_No As Long, ByRef Err_Desc As String, RuleID As Integer, RedItem As String, ByVal OrgID As String, ValidFrom As Date, ValidTo As Date) As Boolean
        Try
            Return ObjDALProduct.CheckRedemptionItemExists(Err_No, Err_Desc, RuleID, RedItem, OrgID, ValidFrom, ValidTo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckMultiTransRuleExists(ByRef Err_No As Long, ByRef Err_Desc As String, RuleID As Integer, SalesItem As String, ByVal OrgID As String, ValidFrom As Date, ValidTo As Date, TransactionType As String) As Boolean
        Try
            Return ObjDALProduct.CheckMultiTransRuleExists(Err_No, Err_Desc, RuleID, SalesItem, OrgID, ValidFrom, ValidTo, TransactionType)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetDiscountFOCItem(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal ItemID As String) As DataTable
        Try
            Return ObjDALProduct.GetDiscountFOCItem(Err_No, Err_Desc, OrgID, ItemID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ExportPriceData(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal PriceListID As Integer) As DataTable
        Try
            Return ObjDALProduct.ExportPriceData(Err_No, Err_Desc, OrgID, PriceListID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ExportBonusData(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal BnsPlanID As Integer) As DataTable
        Try
            Return ObjDALProduct.ExportBonusData(Err_No, Err_Desc, OrgID, BnsPlanID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function UpdateBonusData(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal LineId As String, ByVal BItemCode As String, ByVal BUOM As String, ByVal TypeCode As String, ByVal FromQty As Long, ByVal ToQty As Long, ByVal GetQty As Long, ByVal GetPer As Decimal, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, ByVal CreatedBy As Integer, ByVal BnsPlanID As Integer, MaxQty As Long) As Boolean
        Try
            Return ObjDALProduct.UpdateBonusData(Error_No, Error_Desc, LineId, BItemCode, BUOM, TypeCode, FromQty, ToQty, GetQty, GetPer, ValidFrom, ValidTo, CreatedBy, BnsPlanID, MaxQty)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function UpdateInvoiceRule(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal InvValue As Decimal, MinItem As Integer, ByVal OrgId As String, ByVal FocItem As String, ByVal GetQty As Long, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, ByVal CreatedBy As Integer, ByVal BnsPlanID As Integer, LineID As Integer) As Boolean
        Try
            Return ObjDALProduct.UpdateInvoiceRule(Error_No, Error_Desc, InvValue, MinItem, OrgId, FocItem, GetQty, ValidFrom, ValidTo, CreatedBy, BnsPlanID, LineID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function UpdatePriceData(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal LineId As String, ByVal ItemID As String, ByVal DUOM As String, ByVal UnitPrice As Decimal, ByVal PriceListID As Integer) As Boolean
        Try
            Return ObjDALProduct.UpdatePriceData(Error_No, Error_Desc, LineId, ItemID, DUOM, UnitPrice, PriceListID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function UpdateBonusStatus(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal LineId As String, ByVal IsValid As String, ByVal UpdatedBy As Integer) As Boolean
        Try
            Return ObjDALProduct.UpdateBonusStatus(Error_No, Error_Desc, LineId, IsValid, UpdatedBy)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function UploadDiscount(ByVal dtData As DataTable, ByVal OrgID As String, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PlanID As String) As Boolean
        Try
            Return ObjDALProduct.UploadDiscount(dtData, OrgID, Err_No, Err_Desc, PlanID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function SaveBonusData(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal ItemCode As String, ByVal OrgId As String, ByVal DUOm As String, ByVal BItemCode As String, ByVal BUOM As String, ByVal TypeCode As String, ByVal FromQty As Long, ByVal ToQty As Long, ByVal GetQty As Long, ByVal GetPer As Decimal, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, ByVal UpdatedBy As Integer, ByVal BnsPlanID As Integer, MaxQty As Long) As Boolean
        Try
            Return ObjDALProduct.SaveBonusData(Error_No, Error_Desc, ItemCode, OrgId, DUOm, BItemCode, BUOM, TypeCode, FromQty, ToQty, GetQty, GetPer, ValidFrom, ValidTo, UpdatedBy, BnsPlanID, MaxQty)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SaveInvoiceRule(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal InvValue As Decimal, MinItem As Integer, ByVal OrgId As String, ByVal FocItem As String, ByVal GetQty As Long, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, ByVal CreatedBy As Integer, ByVal BnsPlanID As Integer) As Boolean
        Try
            Return ObjDALProduct.SaveInvoiceRule(Error_No, Error_Desc, InvValue, MinItem, OrgId, FocItem, GetQty, ValidFrom, ValidTo, CreatedBy, BnsPlanID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function SavePriceData(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal ItemID As String, ByVal OrgId As String, ByVal DUOm As String, ByVal UnitPrice As Decimal, ByVal PriceListID As Integer) As Boolean
        Try
            Return ObjDALProduct.SavePriceData(Error_No, Error_Desc, ItemID, OrgId, DUOm, UnitPrice, PriceListID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function DeletePriceData(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal LineId As String) As Boolean
        Try
            Return ObjDALProduct.DeletePriceData(Err_No, Err_Desc, LineId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function DeleteBonusData(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal LineId As String) As Boolean
        Try
            Return ObjDALProduct.DeleteBonusData(Err_No, Err_Desc, LineId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function DeleteInvoiceRule(ByRef Err_No As Long, ByRef Err_Desc As String, PlanID As Integer, ByVal LineId As String, DeletedBy As Integer) As Boolean
        Try
            Return ObjDALProduct.DeleteInvoiceRule(Err_No, Err_Desc, PlanID, LineId, DeletedBy)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetBonusProductDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal Description As String, ByVal OrgID As String) As DataRow
        Try
            Return ObjDALProduct.GetBonusProductDetails(Err_No, Err_Desc, ItemCode, Description, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function CheckBonusData(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal FromQty As Long, ByVal ToQty As Long, ByVal OrgID As String, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime) As Boolean
        Try
            Return ObjDALProduct.CheckBonusData(Err_No, Err_Desc, ItemCode, FromQty, ToQty, OrgID, ValidFrom, ValidTo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function DeleteBonusPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PlanId As String, ByVal UpdatedBy As Integer) As Boolean
        Try
            Return ObjDALProduct.DeleteBonusPlan(Err_No, Err_Desc, PlanId, UpdatedBy)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function DeletePriceList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PriceListID As String) As Boolean
        Try
            Return ObjDALProduct.DeletePriceList(Err_No, Err_Desc, PriceListID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function DeleteMultiTransRule(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RuleID As String) As Boolean
        Try
            Return ObjDALProduct.DeleteMultiTransRule(Err_No, Err_Desc, RuleID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function DeleteRedemptionItem(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RuleID As String) As Boolean
        Try
            Return ObjDALProduct.DeleteRedemption(Err_No, Err_Desc, RuleID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function DeleteSimpleBonusPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PlanId As String) As Boolean
        Try
            Return ObjDALProduct.DeleteSimpleBonusPlan(Err_No, Err_Desc, PlanId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function CheckBonusDataActiveRange(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal FromQty As Long, ByVal ToQty As Long, ByVal OrgID As String, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, ByVal LineId As Integer, OUOM As String, PlanID As Integer) As DataTable
        Try
            Return ObjDALProduct.CheckBonusDataActiveRange(Err_No, Err_Desc, ItemCode, FromQty, ToQty, OrgID, ValidFrom, ValidTo, LineId, OUOM, PlanID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckBonusInvValueActiveRange(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal MinInvValue As Decimal, ByVal MinItem As Integer, ByVal OrgID As String, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, ByVal LineID As Integer, BnsPlanID As Integer) As Boolean
        Try
            Return ObjDALProduct.CheckBonusInvValueActiveRange(Err_No, Err_Desc, MinInvValue, MinItem, OrgID, ValidFrom, ValidTo, LineID, BnsPlanID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function CheckIfBonusDateCanbeChanged(ByRef Err_No As Long, ByRef Err_Desc As String, PlanID As String, ByVal ItemCode As String, ByVal OrgID As String, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime) As DataTable
        Try
            Return ObjDALProduct.CheckIfBonusDateCanbeChanged(Err_No, Err_Desc, PlanID, ItemCode, OrgID, ValidFrom, ValidTo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function UpdateDiscountData(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal LineID As Integer, ByVal ItemCode As String, ByVal OrgId As String, ByVal TypeCode As String, ByVal FromQty As Long, ByVal Rate As Decimal, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, ByVal PlanID As String) As Boolean
        Try
            Return ObjDALProduct.UpdateDiscountData(Error_No, Error_Desc, LineID, ItemCode, OrgId, TypeCode, FromQty, Rate, ValidFrom, ValidTo, PlanID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function UpdateCostPrice(ByVal ItemID As Integer, CostPrice As Decimal, ByVal OrgID As String, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDALProduct.UpdateCostPrice(ItemID, CostPrice, OrgID, Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function UploadCostPrice(ByVal dtData As DataTable, ByVal OrgID As String, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDALProduct.UploadCostPrice(dtData, OrgID, Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function UpdateMultiTransRule(RuleID As String, RedQty As String, GivenQty As String, IsActive As String, UpdatedBy As Integer, ByRef Err_No As Long, ByRef Err_Desc As String, ValidFrom As Date, ValidTo As Date, TrnsactionType As String) As Boolean
        Try
            Return ObjDALProduct.UpdateMultiTransRule(RuleID, RedQty, GivenQty, IsActive, UpdatedBy, Err_No, Err_Desc, ValidFrom, ValidTo, TrnsactionType)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function UpdateRedemptionRule(RuleID As String, RedQty As String, GivenQty As String, IsActive As String, UpdatedBy As Integer, ByRef Err_No As Long, ByRef Err_Desc As String, ValidFrom As Date, ValidTo As Date) As Boolean
        Try
            Return ObjDALProduct.UpdateRedemptionRule(RuleID, RedQty, GivenQty, IsActive, UpdatedBy, Err_No, Err_Desc, ValidFrom, ValidTo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SaveDiscountData(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal ItemCode As String, ByVal OrgId As String, ByVal TypeCode As String, ByVal FromQty As Long, ByVal Rate As Decimal, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, ByVal PlanID As String) As Boolean
        Try
            Return ObjDALProduct.SaveDiscountData(Error_No, Error_Desc, ItemCode, OrgId, TypeCode, FromQty, Rate, ValidFrom, ValidTo, PlanID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function DeleteDiscountData(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal LineId As Integer) As Boolean
        Try
            Return ObjDALProduct.DeleteDiscountData(Err_No, Err_Desc, LineId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function CheckDiscountDataActiveRange(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal FromQty As Long, ByVal OrgID As String, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, ByVal LineId As Integer, ByVal PlanID As Integer) As Boolean
        Try
            Return ObjDALProduct.CheckDiscountDataRange(Err_No, Err_Desc, ItemCode, FromQty, OrgID, ValidFrom, ValidTo, LineId, PlanID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function LoadAssortmentSlabs(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PlanId As Integer) As DataTable
        Try
            Return ObjDALProduct.LoadAssortmentSlabs(Err_No, Err_Desc, PlanId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function LoadAssortmentItems(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PlanId As Integer, ByVal OrgID As String) As DataTable
        Try
            Return ObjDALProduct.LoadAssortmentItems(Err_No, Err_Desc, PlanId, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SaveAssortment(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PlanID As String, ByVal dtOrdItems As DataTable, ByVal dtGetItems As DataTable, ByVal dtSlabs As DataTable, ByVal CreatedBy As Integer) As Boolean
        Try
            Return ObjDALProduct.SaveAssortment(Err_No, Err_Desc, PlanID, dtOrdItems, dtGetItems, dtSlabs, CreatedBy)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SaveAssortmentMinQty(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PlanID As String, ByVal dtOrdItems As DataTable, ByVal dtGetItems As DataTable, ByVal CreatedBy As Integer, TypeCode As String, GetQty As Decimal) As Boolean
        Try
            Return ObjDALProduct.SaveAssortmentMinQty(Err_No, Err_Desc, PlanID, dtOrdItems, dtGetItems, CreatedBy, TypeCode, GetQty)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckAssortmentItem(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal PlanID As String, ByVal ItemCode As String) As Boolean
        Try
            Return ObjDALProduct.CheckAssortmentItem(Err_No, Err_Desc, OrgID, PlanID, ItemCode)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckAssortmentSlab(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PlanID As String, ByVal SlabID As String, ByVal FromQTy As Long, ByVal ToQty As Long) As Boolean
        Try
            Return ObjDALProduct.CheckAssortmentSlab(Err_No, Err_Desc, PlanID, SlabID, FromQTy, ToQty)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetFOCDefinition(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Try
            Return ObjDALProduct.GetFOCDefinition(Err_No, Err_Desc, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SaveDiscountFOC(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, ByRef Inventory_item_ID As String, ByVal Opt As String) As Boolean
        Try
            Return ObjDALProduct.SaveDiscountFOC(Err_No, Err_Desc, OrgId, Inventory_item_ID, Opt)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetProductListByOrgID(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Try
            Return ObjDALProduct.GetProductListByOrgID(Err_No, Err_Desc, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function LoadBonusProductList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Try
            Return ObjDALProduct.LoadBonusProductList(Err_No, Err_Desc, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function LoadBonusProductList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, Optional text As String = "") As DataTable
        Try
            Return ObjDALProduct.LoadBonusProductList(Err_No, Err_Desc, OrgID, text)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function LoadRedemptionProductList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, Optional text As String = "") As DataTable
        Try
            Return ObjDALProduct.LoadRedemptionProductList(Err_No, Err_Desc, OrgID, text)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function LoadPriceProductList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, Optional text As String = "") As DataTable
        Try
            Return ObjDALProduct.LoadPriceProductList(Err_No, Err_Desc, OrgID, text)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function LoadBonusProductListDesc(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, Optional text As String = "") As DataTable
        Try
            Return ObjDALProduct.LoadBonusProductListDesc(Err_No, Err_Desc, OrgID, text)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function LoadProductList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, Optional text As String = "") As DataTable
        Try
            Return ObjDALProduct.LoadProductList(Err_No, Err_Desc, OrgID, text)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function LoadProductList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Try
            Return ObjDALProduct.LoadProductList(Err_No, Err_Desc, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetOrgsHeads(ByRef Err_No As Long, ByRef Err_Desc As String) As DataSet
        Try
            Return ObjDALProduct.GetOrgsHeads(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function SaveVATRule(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, Customer_ID As String, SiteUse_ID As String, Inventory_item_Id As String, VatValue As String, Vat_code As String, ByVal userID As Integer) As Boolean
        Try
            Return ObjDALProduct.SaveVATRule(Err_No, Err_Desc, OrgID, Customer_ID, SiteUse_ID, Inventory_item_Id, VatValue, Vat_code, userID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SaveMultiTransRule(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal OrgId As String, ByVal RedItem As String, ByRef RedQty As String, GivenItem As String, GivenQty As String, IsActive As String, ByVal CreatedBy As Integer, ValidFrom As Date, ValidTo As Date, ByRef RuleID As String, Transaction_Type As String) As Boolean
        Try
            Return ObjDALProduct.SaveMultiTransRule(Error_No, Error_Desc, OrgId, RedItem, RedQty, GivenItem, GivenQty, IsActive, CreatedBy, ValidFrom, ValidTo, RuleID, Transaction_Type)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function SaveRedemptionRule(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, RedItem As String, RedQty As String, GivenItem As String, GivenQty As String, ISactive As String, ByVal userID As Integer, ValidFrom As Date, ValidTo As Date) As Boolean
        Try
            Return ObjDALProduct.SaveRedemptionRule(Err_No, Err_Desc, OrgID, RedItem, RedQty, GivenItem, GivenQty, ISactive, userID, ValidFrom, ValidTo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function DeleteVATRule(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, Vat_Rule_ID As String) As Boolean
        Try
            Return ObjDALProduct.DeleteVATRule(Err_No, Err_Desc, OrgID, Vat_Rule_ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function ExportVATRule(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Try
            Return ObjDALProduct.GetVATRule(Err_No, Err_Desc, OrgID, 0, 0, 0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function ExportMSL(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, SalesRep_ID As String) As DataTable
        Try
            Return ObjDALProduct.ExportMSL(Err_No, Err_Desc, OrgID, SalesRep_ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ExportMSLGroup(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, PG_ID As String) As DataTable
        Try
            Return ObjDALProduct.ExportMSLGroup(Err_No, Err_Desc, OrgID, PG_ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ExportMSLCustomerGroup(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, PG_ID As String) As DataTable
        Try
            Return ObjDALProduct.ExportMSLCustomerGroup(Err_No, Err_Desc, OrgID, PG_ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ExportCustomerVanMap(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Try
            Return ObjDALProduct.ExportCustomerVanMap(Err_No, Err_Desc, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function ExportRedemptionRule(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Try
            Return ObjDALProduct.ExportRedemptionRule(Err_No, Err_Desc, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function ExportMultiTransRule(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Try
            Return ObjDALProduct.ExportMultiTransRule(Err_No, Err_Desc, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetVATRule(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, Customer_ID As String, SiteUseId As String, ItemID As String) As DataTable
        Try
            Return ObjDALProduct.GetVATRule(Err_No, Err_Desc, OrgID, Customer_ID, SiteUseId, ItemID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function LoadBonusPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal userID As Integer, ByVal OrgID As String) As DataTable
        Try
            Return ObjDALProduct.LoadBonusPlan(Err_No, Err_Desc, userID, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckCustBonusFlag(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDALProduct.CheckCustBonusFlag(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function LoadSimpleBonusPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal userID As Integer, ByVal OrgID As String) As DataTable
        Try
            Return ObjDALProduct.LoadSimpleBonusPlan(Err_No, Err_Desc, userID, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSimpleBonusCategoryMap(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PlanID As Integer) As DataTable
        Try
            Return ObjDALProduct.GetSimpleBonusCategoryMap(Err_No, Err_Desc, PlanID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetMultTrxCategoryMap(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RuleID As Integer) As DataTable
        Try
            Return ObjDALProduct.GetMultTrxCategoryMap(Err_No, Err_Desc, RuleID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetDiscountCategoryMap(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PlanID As Integer) As DataTable
        Try
            Return ObjDALProduct.GetDiscountCategoryMap(Err_No, Err_Desc, PlanID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetAssortmentBonusCategoryMap(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PlanID As Integer) As DataTable
        Try
            Return ObjDALProduct.GetAssortmentBonusCategoryMap(Err_No, Err_Desc, PlanID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function LoadPriceList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal userID As Integer, ByVal OrgID As String) As DataTable
        Try
            Return ObjDALProduct.LoadPriceList(Err_No, Err_Desc, userID, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function UpdateBonusPlan(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal PlanId As String, ByVal PlanName As String, ByVal CreatedBy As Integer, ByVal IsActive As String, ByVal ValidFrom As DateTime, ByVal Validto As DateTime, TransactionType As String) As Boolean
        Try
            Return ObjDALProduct.UpdateBonusPlan(Error_No, Error_Desc, PlanId, PlanName, CreatedBy, IsActive, ValidFrom, Validto, TransactionType)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function UpdateSimpleBonusPlan(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal PlanId As String, ByVal PlanName As String, ByVal CreatedBy As Integer, TransactionType As String) As Boolean
        Try
            Return ObjDALProduct.UpdateSimpleBonusPlan(Error_No, Error_Desc, PlanId, PlanName, CreatedBy, TransactionType)
        Catch ex As Exception
            Throw ex
        End Try
    End Function



    Function SaveBonusPlan(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal OrgId As String, ByVal PlanName As String, ByVal CreatedBy As Integer, ByRef PlanId As String, ByVal IsActive As String, ByVal ValidFrom As DateTime, ByVal Validto As DateTime, PlanType As String, TransactionType As String) As String
        Try
            Return ObjDALProduct.SaveBonusPlan(Error_No, Error_Desc, OrgId, PlanName, CreatedBy, PlanId, IsActive, ValidFrom, Validto, PlanType, TransactionType)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function SaveSimpleBonusPlan(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal OrgId As String, ByVal PlanName As String, ByVal CreatedBy As Integer, ByRef PlanId As String, PlanType As String, TransactionType As String) As String
        Try
            Return ObjDALProduct.SaveSimpleBonusPlan(Error_No, Error_Desc, OrgId, PlanName, CreatedBy, PlanId, PlanType, TransactionType)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function SaveBonusCategoryMap(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal PlanID As String, DtCategory As DataTable, CreatedBy As String, PlanType As String, InsertMode As String, OrgID As String, TransactionType As String, ByRef dt As DataTable) As Boolean
        Try
            Return ObjDALProduct.SaveBonusCategoryMap(Error_No, Error_Desc, PlanID, DtCategory, CreatedBy, PlanType, "U", OrgID, TransactionType, dt)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function SaveAssortmentBonusCategoryMap(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal PlanID As String, DtCategory As DataTable, CreatedBy As String, PlanType As String, InsertMode As String, OrgID As String, TransactionType As String, ByRef dt As DataTable) As Boolean
        Try
            Return ObjDALProduct.SaveAssortmentBonusCategoryMap(Error_No, Error_Desc, PlanID, DtCategory, CreatedBy, PlanType, InsertMode, OrgID, TransactionType, dt)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function UpdatePriceList(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal PriceListId As String, ByVal PriceListName As String, ByVal Code As String, ByVal CreatedBy As Integer) As Boolean
        Try
            Return ObjDALProduct.UpdatePriceList(Error_No, Error_Desc, PriceListId, PriceListName, Code, CreatedBy)
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Function SavePriceList(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal OrgId As String, ByVal PriceListName As String, ByVal CreatedBy As Integer, ByRef PriceListId As String, ByVal Code As String) As String
        Try
            Return ObjDALProduct.SavePriceList(Error_No, Error_Desc, OrgId, PriceListName, CreatedBy, PriceListId, Code)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function CheckBonusPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Planname As String) As Boolean
        Try
            Return ObjDALProduct.CheckBonusPlan(Err_No, Err_Desc, OrgID, Planname)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function CheckSimpleBonusPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Planname As String) As Boolean
        Try
            Return ObjDALProduct.CheckSimpleBonusPlan(Err_No, Err_Desc, OrgID, Planname)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetOrganisation(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UD_SUB_QRY As String) As DataTable
        Try
            Return ObjDALProduct.GetOrganisation(Err_No, Err_Desc, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetProductUOM(ByRef Err_No As Long, ByRef Err_Desc As String, OrgID As String, ItemID As Integer) As DataTable
        Try
            Return ObjDALProduct.GetProductUOM(Err_No, Err_Desc, OrgID, ItemID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function DeleteProductImage(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal MediaFileID As String) As Boolean
        Try
            Return ObjDALProduct.DeleteProductImage(Error_No, Error_Desc, MediaFileID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SaveProductImage(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal MediaFileID As String, ByVal ItemID As Integer, ByVal OrgID As Integer, ByVal MediaType As String, ByVal MediaFile As String, ByVal ThumbNail As String, ByVal Caption As String, ByVal CreatedBy As String, isdefault As String) As Boolean
        Try
            Return ObjDALProduct.SaveProductImage(Error_No, Error_Desc, MediaFileID, ItemID, OrgID, MediaType, MediaFile, ThumbNail, Caption, CreatedBy, isdefault)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSalesRepID(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRepNo As String) As DataTable
        Try
            Return ObjDALProduct.GetSalesRepId(Err_No, Err_Desc, SalesRepNo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetDefault(ByVal ID As String, ByVal VanID As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim TempTbl As New DataTable
        Try
            TempTbl = ObjDALProduct.GetDefaultProduct(ID, VanID, Err_No, Err_Desc)
            Dim ProdTbl As New DataTable
            ProdTbl.Columns.Add("Inventory_Item_ID", GetType(Int64))
            ProdTbl.Columns.Add("Description", GetType(String))
            ProdTbl.Columns.Add("Item_Code", GetType(String))

            Dim MyRow As DataRow

            If TempTbl IsNot Nothing Then
                For Each dr As DataRow In TempTbl.Rows
                    MyRow = ProdTbl.NewRow()
                    MyRow(0) = dr.Item("Inventory_Item_ID")
                    MyRow(1) = "[" & dr.Item("Item_Code") & "]-" & dr.Item("Description")
                    MyRow(2) = dr.Item("Item_Code")
                    ProdTbl.Rows.Add(MyRow)
                Next
            End If

            GetDefault = ProdTbl
        Catch ex As Exception
            If Err_Desc Is Nothing Then
                Err_Desc = ex.Message
            End If
            Throw ex
        End Try
    End Function
    Function GetSelected(ByVal ID As String, ByVal VanID As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim TempTbl As New DataTable
        Try
            TempTbl = ObjDALProduct.GetSelectedProduct(ID, VanID, Err_No, Err_Desc)
            Dim ProdTbl As New DataTable
            ProdTbl.Columns.Add("Inventory_Item_ID", GetType(Int64))
            ProdTbl.Columns.Add("Description", GetType(String))
            ProdTbl.Columns.Add("Item_Code", GetType(String))

            Dim MyRow As DataRow

            If TempTbl IsNot Nothing Then
                For Each dr As DataRow In TempTbl.Rows
                    MyRow = ProdTbl.NewRow()
                    MyRow(0) = dr.Item("Inventory_Item_ID")
                    MyRow(1) = "[" & dr.Item("Item_Code") & "]-" & dr.Item("Description")
                    MyRow(2) = dr.Item("Item_Code")
                    ProdTbl.Rows.Add(MyRow)
                Next
            End If

            GetSelected = ProdTbl
        Catch ex As Exception
            If Err_Desc Is Nothing Then
                Err_Desc = ex.Message
            End If
            Throw ex
        End Try
    End Function
    Public Sub InsertProduct(ByVal Org_ID As String, ByVal ItemId As Integer, ByVal ItemCode As String, ByVal VanId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String)
        Try
            ObjDALProduct.InsertMSL(Org_ID, ItemId, ItemCode, VanId, Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Public Sub DeleteProduct(ByVal Org_ID As String, ByVal ItemId As Integer, ByVal ItemCode As String, ByVal VanId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String)
        Try
            ObjDALProduct.DeleteMSL(Org_ID, ItemId, ItemCode, VanId, Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function CheckProductValid(ByVal tbl As String, ByVal ItemCode As String, ByVal Org_ID As String, ByVal VanID As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDALProduct.CheckProductExist(tbl, ItemCode, Org_ID, VanID, Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function RebuildAllProduct(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal VanId As Integer) As Boolean
        Try
            Return ObjDALProduct.DeleteAll(Err_No, Err_Desc, VanId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function CheckOrgID(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As Boolean
        Try
            Return ObjDALProduct.CheckOrgID(Err_No, Err_Desc, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckItemCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal OrgID As String) As Boolean
        Try
            Return ObjDALProduct.CheckItemCode(Err_No, Err_Desc, ItemCode, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function CheckItemUOM(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal OrgID As String, UOM As String) As Boolean
        Try
            Return ObjDALProduct.CheckItemUOM(Err_No, Err_Desc, ItemCode, OrgID, UOM)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckValidCustomer(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustomerNo As String, SiteNo As String, ByVal OrgID As String) As Boolean
        Try
            Return ObjDALProduct.CheckValidCustomer(Err_No, Err_Desc, CustomerNo, SiteNo, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckValidShipAddress(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustomerNo As String, SiteNo As String, ByVal OrgID As String) As Boolean
        Try
            Return ObjDALProduct.CheckValidShipAddress(Err_No, Err_Desc, CustomerNo, SiteNo, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckValidVan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, Van As String) As Boolean
        Try
            Return ObjDALProduct.CheckValidVan(Err_No, Err_Desc, OrgID, Van)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckCustomerNo(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustomerNo As String, ByVal OrgID As String) As Boolean
        Try
            Return ObjDALProduct.CheckCustomerNo(Err_No, Err_Desc, CustomerNo, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetItemUOM(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal OrgID As String) As String
        Try
            Return ObjDALProduct.GetItemUOM(Err_No, Err_Desc, ItemCode, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetConversion(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal OrgID As String, UOM As String) As String
        Try
            Return ObjDALProduct.GetConversion(Err_No, Err_Desc, ItemCode, OrgID, UOM)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetInventoryItemID(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, OrgID As String) As String
        Try
            Return ObjDALProduct.GetInventoryItemID(Err_No, Err_Desc, ItemCode, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetItemName(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemID As String, OrgID As String) As String
        Try
            Return ObjDALProduct.GetItemName(Err_No, Err_Desc, ItemID, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetProdName(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemID As String) As String
        Try
            Return ObjDALProduct.GetProdName(Err_No, Err_Desc, ItemID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetItemNameFromCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, OrgID As String) As String
        Try
            Return ObjDALProduct.GetItemNameFromCode(Err_No, Err_Desc, ItemCode, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetItemNameOnlyFromCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, OrgID As String) As String
        Try
            Return ObjDALProduct.GetItemNameOnlyFromCode(Err_No, Err_Desc, ItemCode, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub GetCustomerID(ByRef Err_No As Long, ByRef Err_Desc As String, OrgID As String, CustomerNo As String, ByRef CustomerID As String, ByRef SiteID As String)
        Try
            ObjDALProduct.GetCustomerID(Err_No, Err_Desc, OrgID, CustomerNo, CustomerID, SiteID)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function UploadFOCDiscount(ByVal dtData As DataTable, ByVal OrgID As String, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserID As String) As Boolean
        Try
            Return ObjDALProduct.UploadFOCDiscount(dtData, OrgID, Err_No, Err_Desc, UserID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function LoadBrandList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Try
            Return ObjDALProduct.LoadBrandList(Err_No, Err_Desc, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function UploadFSRProduct(ByVal OrgID As String, ByVal SalesRepID As Integer, ByVal dtData As DataTable, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDALProduct.UploadFSRProduct(OrgID, SalesRepID, dtData, Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function LoadFSRProductTemplate(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal SalesRepID As Integer) As DataTable
        Try
            Return ObjDALProduct.LoadFSRProductTemplate(Err_No, Err_Desc, OrgID, SalesRepID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub InsertProductGroupFSR(ByVal Org_ID As String, ByVal GroupId As Integer, ByVal SalesRepId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Mode As String, ByRef Msg As String)
        Try
            ObjDALProduct.InsertProductGroupFSR(Org_ID, GroupId, SalesRepId, Err_No, Err_Desc, Mode, Msg)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function InsertCollectionGroupFSR(ByVal OrgId As String, ByVal GroupId As String, ByVal SalesRepId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Mode As String, CreatedBy As Integer) As Boolean
        Try
            ObjDALProduct.InserCollectionGroupFSR(OrgId, GroupId, SalesRepId, Err_No, Err_Desc, Mode, CreatedBy)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function InsertCustomerMSLGroup(ByVal OrgId As String, ByVal CustomerId As Integer, SiteID As String, ByVal CustomerNo As String, ByVal GroupId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CreatedBy As Integer, ByRef IsDeleted As Boolean, Mode As String) As Boolean
        Try
            ObjDALProduct.InsertCustomerMSLGroup(OrgId, CustomerId, SiteID, CustomerNo, GroupId, Err_No, Err_Desc, CreatedBy, IsDeleted, Mode)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub InsertProductGroup(ByVal Org_ID As String, ByVal ItemId As Integer, ByVal ItemCode As String, ByVal GroupId As Integer, ByVal GroupName As String, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CreatedBy As Integer)
        Try
            ObjDALProduct.InsertProductGroup(Org_ID, ItemId, ItemCode, GroupId, GroupName, Err_No, Err_Desc, CreatedBy)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function InsertCustomerVanMap(CallFrom As String, ByVal OrgId As String, ByVal CustomerId As Integer, SiteID As String, ByVal CustomerNo As String, ByVal SId As String, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CreatedBy As Integer, ByRef IsDeleted As Boolean, Mode As String, VanList As String) As Boolean
        Try
            ObjDALProduct.InsertCustomerVanMap(CallFrom, OrgId, CustomerId, SiteID, CustomerNo, SId, Err_No, Err_Desc, CreatedBy, IsDeleted, Mode, VanList)
        Catch ex As Exception
            Throw ex
        End Try
    End Function



    Public Sub InsertProductMSLGroup(ByVal Org_ID As String, ByVal ItemId As Integer, ByVal ItemCode As String, ByVal GroupId As Integer, ByVal GroupName As String, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CreatedBy As Integer, ByRef IsDeleted As Boolean, Mode As String)
        Try
            ObjDALProduct.InsertProductMSLGroup(Org_ID, ItemId, ItemCode, GroupId, GroupName, Err_No, Err_Desc, CreatedBy, IsDeleted, Mode)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Function DeleteProductGroupALL(ByVal groupID As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDALProduct.DeleteProductGroupAll(groupID, Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Function DeleteProductMSLGroupALL(ByVal groupID As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDALProduct.DeleteProductMSLGroupAll(groupID, Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Function GetProductMSLGroup(ByRef Err_No As Long, ByRef Err_Desc As String, OrgID As String) As DataTable
        Try
            Return ObjDALProduct.GetProductMSLGroup(Err_No, Err_Desc, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetProductGroup(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDALProduct.GetProductGroup(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub InsertFSRProduct(ByVal InsertMode As String, ByVal Org_ID As String, ByVal ItemId As Integer, ByVal ItemCode As String, ByVal VanId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemList As String)
        Try
            ObjDALProduct.InsertFSRProduct(InsertMode, Org_ID, ItemId, ItemCode, VanId, Err_No, Err_Desc, ItemList)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function GetSelectedCollectionGroupFSR(ByVal OrgID As String, ByVal SalesRepID As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDALProduct.GetSelectedCollectionGroupFSR(OrgID, SalesRepID, Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSelectedProductGroupFSR(ByVal OrgID As String, ByVal SalesRepID As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim TempTbl As New DataTable
        Try
            TempTbl = ObjDALProduct.GetSelectedProductGroupFSR(OrgID, SalesRepID, Err_No, Err_Desc)
            Dim ProdTbl As New DataTable
            ProdTbl.Columns.Add("PG_ID", GetType(Int64))
            ProdTbl.Columns.Add("Description", GetType(String))



            Dim MyRow As DataRow

            If TempTbl IsNot Nothing Then
                For Each dr As DataRow In TempTbl.Rows
                    'If dr.Item("Inventory_Item_ID").ToString() = "0" Then
                    '    Dim r As DataRow = ProdTbl.NewRow()
                    '    r(0) = 0
                    '    r(1) = ""
                    '    ProdTbl.Rows.Add(r)
                    'End If
                    MyRow = ProdTbl.NewRow()
                    MyRow(0) = dr.Item("PG_ID")
                    'If dr.Item("Inventory_Item_ID").ToString() = "0" Then
                    '    MyRow(1) = dr.Item("Item_Code").ToString()
                    'Else
                    MyRow(1) = dr.Item("Description")
                    'End If
                    ProdTbl.Rows.Add(MyRow)
                    'If dr.Item("Inventory_Item_ID").ToString() = "0" Then
                    '    Dim r As DataRow = ProdTbl.NewRow()
                    '    r(0) = 0
                    '    r(1) = ""
                    '    ProdTbl.Rows.Add(r)
                    'End If
                Next
            End If

            GetSelectedProductGroupFSR = ProdTbl
        Catch ex As Exception
            If Err_Desc Is Nothing Then
                Err_Desc = ex.Message
            End If
            Throw ex
        End Try
    End Function
    Function GetSelectedProductGroup(ByVal OrgID As String, ByVal GroupID As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim TempTbl As New DataTable
        Try
            TempTbl = ObjDALProduct.GetSelectedProductGroup(OrgID, GroupID, Err_No, Err_Desc)

            GetSelectedProductGroup = TempTbl
        Catch ex As Exception
            If Err_Desc Is Nothing Then
                Err_Desc = ex.Message
            End If
            Throw ex
        End Try
    End Function

    Function GetSelectedProductMSLGroup(ByVal OrgID As String, ByVal GroupID As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim TempTbl As New DataTable
        Try
            TempTbl = ObjDALProduct.GetSelectedProductMSLGroup(OrgID, GroupID, Err_No, Err_Desc)

            GetSelectedProductMSLGroup = TempTbl
        Catch ex As Exception
            If Err_Desc Is Nothing Then
                Err_Desc = ex.Message
            End If
            Throw ex
        End Try
    End Function
    Public Function GetAvailCustomersByVan(ByVal OrgId As String, ByVal SId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim TempTbl As New DataTable
        Try
            TempTbl = ObjDALProduct.GetAvailCustomersByVan(OrgId, SId, Err_No, Err_Desc)

            GetAvailCustomersByVan = TempTbl
        Catch ex As Exception
            If Err_Desc Is Nothing Then
                Err_Desc = ex.Message
            End If
            Throw ex
        End Try
    End Function
    Public Function GetAvailCustomersByMSL(ByVal OrgId As String, ByVal PGId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim TempTbl As New DataTable
        Try
            TempTbl = ObjDALProduct.GetAvailCustomersByMSL(OrgId, PGId, Err_No, Err_Desc)

            GetAvailCustomersByMSL = TempTbl
        Catch ex As Exception
            If Err_Desc Is Nothing Then
                Err_Desc = ex.Message
            End If
            Throw ex
        End Try
    End Function
    Public Function GetAssignedCustomerByVan(ByVal OrgID As String, ByVal SID As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim TempTbl As New DataTable
        Try
            TempTbl = ObjDALProduct.GetAssignedCustomerByVan(OrgID, SID, Err_No, Err_Desc)

            GetAssignedCustomerByVan = TempTbl
        Catch ex As Exception
            If Err_Desc Is Nothing Then
                Err_Desc = ex.Message
            End If
            Throw ex
        End Try
    End Function
    Public Function GetAssignedMSLCustomer(ByVal OrgId As String, ByVal PGId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim TempTbl As New DataTable
        Try
            TempTbl = ObjDALProduct.GetAssignedMSLCustomer(OrgId, PGId, Err_No, Err_Desc)

            GetAssignedMSLCustomer = TempTbl
        Catch ex As Exception
            If Err_Desc Is Nothing Then
                Err_Desc = ex.Message
            End If
            Throw ex
        End Try
    End Function
    Function GetSelectedFSRProduct(ByVal OrgID As String, ByVal VanID As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim TempTbl As New DataTable
        Try
            TempTbl = ObjDALProduct.GetSelectedFSRProduct(OrgID, VanID, Err_No, Err_Desc)

            GetSelectedFSRProduct = TempTbl
        Catch ex As Exception
            If Err_Desc Is Nothing Then
                Err_Desc = ex.Message
            End If
            Throw ex
        End Try
    End Function

    Public Function GetDefaultCollectionGroupFSR(ByVal OrgId As String, ByVal SalesRepId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim TempTbl As New DataTable
        Try
            TempTbl = ObjDALProduct.GetDefaultCollectionGroupFSR(OrgId, SalesRepId, Err_No, Err_Desc)

            GetDefaultCollectionGroupFSR = TempTbl
        Catch ex As Exception
            If Err_Desc Is Nothing Then
                Err_Desc = ex.Message
            End If
            Throw ex
        End Try
    End Function
    Function GetDefaultProdctGroupFSR(ByVal OrgId As String, ByVal SalesRepID As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim TempTbl As New DataTable
        Try
            TempTbl = ObjDALProduct.GetDefaultProductGroupFSR(OrgId, SalesRepID, Err_No, Err_Desc)
            Dim ProdTbl As New DataTable
            ProdTbl.Columns.Add("PG_ID", GetType(Int64))
            ProdTbl.Columns.Add("Description", GetType(String))


            Dim MyRow As DataRow

            If TempTbl IsNot Nothing Then
                For Each dr As DataRow In TempTbl.Rows
                    MyRow = ProdTbl.NewRow()
                    MyRow(0) = dr.Item("PG_ID")
                    MyRow(1) = dr.Item("Description")
                    ProdTbl.Rows.Add(MyRow)
                Next
            End If

            GetDefaultProdctGroupFSR = ProdTbl
        Catch ex As Exception
            If Err_Desc Is Nothing Then
                Err_Desc = ex.Message
            End If
            Throw ex
        End Try
    End Function
    Function GetDefaultProdctGroup(ByVal OrgId As String, ByVal Category As String, ByVal GroupId As String, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Mode As String, ByVal Brand As String, ByVal Agency As String) As DataTable
        Dim TempTbl As New DataTable
        Try
            TempTbl = ObjDALProduct.GetDefaultProductGroup(OrgId, GroupId, Err_No, Err_Desc)
            Dim ProdTbl As New DataTable
            ProdTbl.Columns.Add("Inventory_Item_ID", GetType(Int64))
            ProdTbl.Columns.Add("Description", GetType(String))


            Dim MyRow As DataRow

            If TempTbl IsNot Nothing Then
                For Each dr As DataRow In TempTbl.Rows
                    MyRow = ProdTbl.NewRow()
                    MyRow(0) = dr.Item("Inventory_Item_ID")
                    MyRow(1) = "[" & dr.Item("Item_Code") & "]-" & dr.Item("Description")
                    ProdTbl.Rows.Add(MyRow)
                Next
            End If

            GetDefaultProdctGroup = ProdTbl
        Catch ex As Exception
            If Err_Desc Is Nothing Then
                Err_Desc = ex.Message
            End If
            Throw ex
        End Try
    End Function
    Function GetDefaultProdctFSR(ByVal OrgId As String, ByVal VanID As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim TempTbl As New DataTable
        Try
            TempTbl = ObjDALProduct.GetDefaultProductFSR(OrgId, VanID, Err_No, Err_Desc)
            Dim ProdTbl As New DataTable
            ProdTbl.Columns.Add("Inventory_Item_ID", GetType(Int64))
            ProdTbl.Columns.Add("Description", GetType(String))


            Dim MyRow As DataRow

            If TempTbl IsNot Nothing Then
                For Each dr As DataRow In TempTbl.Rows
                    MyRow = ProdTbl.NewRow()
                    MyRow(0) = dr.Item("Inventory_Item_ID")
                    MyRow(1) = "[" & dr.Item("Item_Code") & "]-" & dr.Item("Description")
                    ProdTbl.Rows.Add(MyRow)
                Next
            End If

            GetDefaultProdctFSR = ProdTbl
        Catch ex As Exception
            If Err_Desc Is Nothing Then
                Err_Desc = ex.Message
            End If
            Throw ex
        End Try
    End Function
    Public Sub DeleteProductGroupFSR(ByVal DeleteMode As String, ByVal Org_ID As String, ByVal GroupId As Integer, ByVal SalesRepId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String)
        Try
            ObjDALProduct.DeleteProductGroupFSR(DeleteMode, Org_ID, GroupId, SalesRepId, Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function DeleteCollectionGroupFSR(ByVal DeleteMode As String, ByVal OrgId As String, ByVal GroupId As String, ByVal SalesRepId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            ObjDALProduct.DeleteCollectionGroupFSR(DeleteMode, OrgId, GroupId, SalesRepId, Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function DeleteCustomerMSLGroup(ByVal DeleteMode As String, ByVal OrgId As String, ByVal GroupId As Integer, ByVal CustomerID As Integer, SiteID As String, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            ObjDALProduct.DeleteCustomerMSLGroup(DeleteMode, OrgId, GroupId, CustomerID, SiteID, Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function DeleteCustomerVanMap(ByVal DeleteMode As String, ByVal OrgId As String, ByVal VanId As String, ByVal CustomerID As Integer, SiteID As String, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            ObjDALProduct.DeleteCustomerVanMap(DeleteMode, OrgId, VanId, CustomerID, SiteID, Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub DeleteProductGroup(ByVal DeleteMode As String, ByVal Org_ID As String, ByVal ItemId As Integer, ByVal ItemCode As String, ByVal GroupId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Groupname As String)
        Try
            ObjDALProduct.DeleteProductGroup(DeleteMode, Org_ID, ItemId, ItemCode, GroupId, Err_No, Err_Desc, Groupname)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub DeleteProductMSLGroup(ByVal DeleteMode As String, ByVal Org_ID As String, ByVal ItemId As Integer, ByVal ItemCode As String, ByVal GroupId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Groupname As String)
        Try
            ObjDALProduct.DeleteProductMSLGroup(DeleteMode, Org_ID, ItemId, ItemCode, GroupId, Err_No, Err_Desc, Groupname)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub DeleteFSRProduct(ByVal DeleteMode As String, ByVal Org_ID As String, ByVal ItemId As Integer, ByVal ItemCode As String, ByVal VanId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemList As String)
        Try
            ObjDALProduct.DeleteFSRProduct(DeleteMode, Org_ID, ItemId, ItemCode, VanId, Err_No, Err_Desc, ItemList)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function CheckProductGroup(ByVal Groupname As String, ByRef Err_No As Long, ByRef Err_Desc As String) As DataRow
        Dim r As DataRow = Nothing
        Try

            r = ObjDALProduct.CheckDuplicateGroup(Err_No, Err_Desc, Groupname)
        Catch ex As Exception
            Throw ex
        End Try
        Return r
    End Function
    Public Function CheckProductMSLGroup(ByVal Groupname As String, ByRef Err_No As Long, ByRef Err_Desc As String) As DataRow
        Dim r As DataRow = Nothing
        Try

            r = ObjDALProduct.CheckDuplicateMSLGroup(Err_No, Err_Desc, Groupname)
        Catch ex As Exception
            Throw ex
        End Try
        Return r
    End Function
    Function GetDefaultProdctMSLGroup(ByVal OrgId As String, ByVal GroupId As String, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim TempTbl As New DataTable
        Try
            TempTbl = ObjDALProduct.GetDefaultProductMSLGroup(OrgId, GroupId, Err_No, Err_Desc)
            Dim ProdTbl As New DataTable
            ProdTbl.Columns.Add("Inventory_Item_ID", GetType(Int64))
            ProdTbl.Columns.Add("Description", GetType(String))


            Dim MyRow As DataRow

            If TempTbl IsNot Nothing Then
                For Each dr As DataRow In TempTbl.Rows
                    MyRow = ProdTbl.NewRow()
                    MyRow(0) = dr.Item("Inventory_Item_ID")
                    MyRow(1) = "[" & dr.Item("Item_Code") & "]-" & dr.Item("Description")
                    ProdTbl.Rows.Add(MyRow)
                Next
            End If

            GetDefaultProdctMSLGroup = ProdTbl
        Catch ex As Exception
            If Err_Desc Is Nothing Then
                Err_Desc = ex.Message
            End If
            Throw ex
        End Try
    End Function


    Function GetDefaultProdctGroup(ByVal OrgId As String, ByVal GroupId As String, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim TempTbl As New DataTable
        Try
            TempTbl = ObjDALProduct.GetDefaultProductGroup(OrgId, GroupId, Err_No, Err_Desc)
            Dim ProdTbl As New DataTable
            ProdTbl.Columns.Add("Inventory_Item_ID", GetType(Int64))
            ProdTbl.Columns.Add("Description", GetType(String))


            Dim MyRow As DataRow

            If TempTbl IsNot Nothing Then
                For Each dr As DataRow In TempTbl.Rows
                    MyRow = ProdTbl.NewRow()
                    MyRow(0) = dr.Item("Inventory_Item_ID")
                    MyRow(1) = "[" & dr.Item("Item_Code") & "]-" & dr.Item("Description")
                    ProdTbl.Rows.Add(MyRow)
                Next
            End If

            GetDefaultProdctGroup = ProdTbl
        Catch ex As Exception
            If Err_Desc Is Nothing Then
                Err_Desc = ex.Message
            End If
            Throw ex
        End Try
    End Function
    Public Function CheckCustDiscountFlag(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDALProduct.CheckCustDiscountFlag(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function CheckDiscountPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Planname As String) As Boolean
        Try
            Return ObjDALProduct.CheckDiscountPlan(Err_No, Err_Desc, OrgID, Planname)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function SaveDiscountPlan(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal OrgId As String, ByVal PlanName As String, ByVal CreatedBy As Integer, ByRef PlanId As String, TransactionType As String) As String
        Try
            Return ObjDALProduct.SaveDiscountPlan(Error_No, Error_Desc, OrgId, PlanName, CreatedBy, PlanId, TransactionType)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function SaveDiscountCategoryMap(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal PlanID As String, DtCategory As DataTable, CreatedBy As String) As Boolean
        Try
            Return ObjDALProduct.SaveDiscountCategoryMap(Error_No, Error_Desc, PlanID, DtCategory, CreatedBy)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function SaveMultiTrxCategoryMap(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal RuleID As String, DtCategory As DataTable, CreatedBy As String) As Boolean
        Try
            Return ObjDALProduct.SaveMultiTrxCategoryMap(Error_No, Error_Desc, RuleID, DtCategory, CreatedBy)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function UpdateDiscountPlan(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal PlanId As String, ByVal PlanName As String, ByVal CreatedBy As Integer, TransactionType As String) As Boolean
        Try
            Return ObjDALProduct.UpdateDiscountPlan(Error_No, Error_Desc, PlanId, PlanName, CreatedBy, TransactionType)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function LoadDiscountPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal userID As Integer, ByVal OrgID As String) As DataTable
        Try
            Return ObjDALProduct.LoadDiscountPlan(Err_No, Err_Desc, userID, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function LoadExportProductsTemplate(ByVal Org_ID As String, ByVal Item_code As String, ByVal Description As String, ByVal UOM As String, ByVal Agency As String) As DataSet
        Return ObjDALProduct.LoadExportProductsTemplate(Org_ID, Item_code, Description, UOM, Agency)
    End Function
    Public Function IsValidUOM(UOM As String) As Boolean
        Return ObjDALProduct.IsValidUOM(UOM)
    End Function
    Public Function IsValidBaseUOM(Code As String, UOM As String) As Boolean
        Return ObjDALProduct.IsValidBaseUOM(Code, UOM)
    End Function
    Public Function IsValidDefUOM(Code As String, UOM As String) As Boolean
        Return ObjDALProduct.IsValidDefUOM(Code, UOM)
    End Function

    Public Function IsValidRestrictiveR(RET_MODE_val As String) As Boolean
        Return ObjDALProduct.IsValidRestrictiveR(RET_MODE_val)
    End Function
    Public Function IsValidOrganization(ByVal OrgID As String) As Boolean
        Return ObjDALProduct.IsValidOrganization(OrgID)
    End Function
    Public Function SaveProduct(ByRef Error_No As Long, ByRef Error_Desc As String, ProdCode As String, ProdName As String, Brand As String, Agency As String, UOM As String, Barcode As String, CostPrice As Decimal, Category As String, ProdNo As String, ItemSize As String, SubCategory As String, DefaultUOM As String, RestrictiveReturn As String, HasLots As String, AllowPriceChange As String, Inv_ID As Integer, Org_ID As String, Stock_UOM As String) As Boolean
        Return ObjDALProduct.SaveProduct(Error_No, Error_Desc, ProdCode, ProdName, Brand, Agency, UOM, Barcode, CostPrice, Category, ProdNo, ItemSize, SubCategory, DefaultUOM, RestrictiveReturn, HasLots, AllowPriceChange, Inv_ID, Org_ID, Stock_UOM)
    End Function
    Public Function SaveProductNew(ByRef Error_No As Long, ByRef Error_Desc As String, ProdCode As String, ProdName As String, Brand As String, Agency As String, UOM As String, Barcode As String, CostPrice As Decimal, Category As String, ProdNo As String, ItemSize As String, SubCategory As String, DefaultUOM As String, RestrictiveReturn As String, HasLots As String, AllowPriceChange As String, Inv_ID As Integer, Org_ID As String, dtUOM As DataTable, stockUOM As String) As Boolean
        Return ObjDALProduct.SaveProductNew(Error_No, Error_Desc, ProdCode, ProdName, Brand, Agency, UOM, Barcode, CostPrice, Category, ProdNo, ItemSize, SubCategory, DefaultUOM, RestrictiveReturn, HasLots, AllowPriceChange, Inv_ID, Org_ID, dtUOM, stockUOM)
    End Function
    Public Function UpdateProduct(ByRef Error_No As Long, ByRef Error_Desc As String, ProdCode As String, ProdName As String, Brand As String, Agency As String, UOM As String, Barcode As String, CostPrice As Decimal, Category As String, ProdNo As String, ItemSize As String, SubCategory As String, DefaultUOM As String, RestrictiveReturn As String, HasLots As String, AllowPriceChange As String, Inv_ID As Integer, Org_ID As String, Stock_UOM As String) As Boolean
        Return ObjDALProduct.UpdateProduct(Error_No, Error_Desc, ProdCode, ProdName, Brand, Agency, UOM, Barcode, CostPrice, Category, ProdNo, ItemSize, SubCategory, DefaultUOM, RestrictiveReturn, HasLots, AllowPriceChange, Inv_ID, Org_ID, Stock_UOM)
    End Function
    Public Function UpdateProductNew(ByRef Error_No As Long, ByRef Error_Desc As String, ProdCode As String, ProdName As String, Brand As String, Agency As String, UOM As String, Barcode As String, CostPrice As Decimal, Category As String, ProdNo As String, ItemSize As String, SubCategory As String, DefaultUOM As String, RestrictiveReturn As String, HasLots As String, AllowPriceChange As String, Inv_ID As Integer, Org_ID As String, dtUOM As DataTable, StockUOM As String) As Boolean
        Return ObjDALProduct.UpdateProductNew(Error_No, Error_Desc, ProdCode, ProdName, Brand, Agency, UOM, Barcode, CostPrice, Category, ProdNo, ItemSize, SubCategory, DefaultUOM, RestrictiveReturn, HasLots, AllowPriceChange, Inv_ID, Org_ID, dtUOM, StockUOM)
    End Function

    Public Function DeleteProduct(ByRef Error_No As Long, ByRef Error_Desc As String, Inv_ID As Integer, Org_ID As String) As Boolean
        Return ObjDALProduct.DeleteProduct(Error_No, Error_Desc, Inv_ID, Org_ID)
    End Function

    Public Function ActivateProduct(ByRef Error_No As Long, ByRef Error_Desc As String, Inv_ID As Integer, Org_ID As String) As Boolean
        Return ObjDALProduct.ActivateProduct(Error_No, Error_Desc, Inv_ID, Org_ID)
    End Function
    Public Function LoadExportItemUOMTemplate(ByVal OrgID As String) As DataTable
        Return ObjDALProduct.LoadExportItemUOMTemplate(OrgID)
    End Function
    Public Function GetProductsbyID(ByRef Error_No As Long, ByRef Error_Desc As String, ProdID As String, OrgID As String) As DataTable

        Return ObjDALProduct.GetProductsByID(Error_No, Error_Desc, ProdID, OrgID)

    End Function
    Public Function GetProductsWithUOM(ByRef Error_No As Long, ByRef Error_Desc As String, ProdID As String, OrgID As String) As DataSet

        Return ObjDALProduct.GetProductsWithUOM(Error_No, Error_Desc, ProdID, OrgID)

    End Function
    Public Function ProductCodeExists(ProdCode As String, ProdID As String, Org As String) As Boolean
        Return ObjDALProduct.ProductCodeExists(ProdCode, ProdID, Org)
    End Function
    Public Function GetBrand() As DataTable
        Dim dt As New DataTable
        dt = ObjDALProduct.GetBrand()
        Dim dr As DataRow
        dr = dt.NewRow()
        dr(0) = "0"
        dr(1) = ""
        dt.Rows.InsertAt(dr, 0)
        Return dt
    End Function
    Public Function GetUOM() As DataTable
        Dim dt As New DataTable
        dt = ObjDALProduct.GetUOM()
        Dim dr As DataRow
        dr = dt.NewRow()
        dr(0) = "0"
        dr(1) = ""
        dt.Rows.InsertAt(dr, 0)
        Return dt
    End Function
    Public Function GetAgency() As DataTable
        Dim dt As New DataTable
        dt = ObjDALProduct.GetAgency()
        Dim dr As DataRow
        dr = dt.NewRow()
        dr(0) = "0"
        dr(1) = ""
        dt.Rows.InsertAt(dr, 0)
        Return dt
    End Function
    Public Function GetCategory() As DataTable
        Dim dt As New DataTable
        dt = ObjDALProduct.GetCategory()
        Dim dr As DataRow
        dr = dt.NewRow()
        dr(0) = "0"
        dr(1) = ""
        dt.Rows.InsertAt(dr, 0)
        Return dt
    End Function

    Public Function GetRestrictiveReturn() As DataTable
        Dim dt As New DataTable
        dt = ObjDALProduct.GetRestrictiveReturn()
        Dim dr As DataRow
        dr = dt.NewRow()
        dr(0) = "0"
        dr(1) = ""
        dt.Rows.InsertAt(dr, 0)
        Return dt
    End Function
    Public Function GetProductGrid(ByVal opt As String, ByVal Org_ID As String, ByVal Item_code As String, ByVal Description As String, ByVal UOM As String, ByVal Agency As String) As DataTable
        Dim dt As New DataTable
        dt = ObjDALProduct.GetProductGrid(opt, Org_ID, Item_code, Description, UOM, Agency)

        Return dt
    End Function

    Public Function GetOrgofITEM(ByVal Itemcode As String) As DataTable
        Return ObjDALProduct.GetOrgofITEM(Itemcode)
    End Function
    Public Function SwapInvoiceRule(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal _ScreatedBy As Integer, ByVal PlanID As Integer, ByVal Source As String, ByVal Dest As String) As Boolean
        Return ObjDALProduct.SwapInvoiceRule(Error_No, Error_Desc, _ScreatedBy, PlanID, Source, Dest)
    End Function

    Public Function ValidatePriceListCode(ByVal OrgID As String, ByVal Price_List_Code As String) As Boolean
        Dim success As Boolean = False
        Try
            success = ObjDALProduct.ValidatePriceListCode(OrgID, Price_List_Code)

        Catch ex As Exception
            Throw ex
        End Try
        Return success
    End Function

    Public Function IsProductCodeExists(ByVal ProdCode As String, ByVal Org As String) As Boolean
        Return ObjDALProduct.IsProductCodeExists(ProdCode, Org)
    End Function

    Public Function IsValidBrand(ByVal BrandCode As String) As Boolean
        Return ObjDALProduct.IsValidBrand(BrandCode)
    End Function
    Public Function IsValidCategory(ByVal CategoryCode As String) As Boolean
        Return ObjDALProduct.IsValidCategory(CategoryCode)
    End Function
    Public Function IsValidAgency(ByVal AgencyCode As String) As Boolean
        Return ObjDALProduct.IsValidAgency(AgencyCode)
    End Function
    Function GetOrg(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDALProduct.GetOrg(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function CheckBonusDataValidity(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal OrgID As String, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, PlanID As String) As DataSet
        Try
            Return ObjDALProduct.CheckBonusDataValidity(Err_No, Err_Desc, ItemCode, OrgID, ValidFrom, ValidTo, PlanID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function CheckAssortmentBonusDataValidity(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, PlanID As String) As DataSet
        Try
            Return ObjDALProduct.CheckAssortmentBonusDataValidity(Err_No, Err_Desc, ItemCode, PlanID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckItemUOMandGetfromDB(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal OrgID As String, ByRef UOM As String) As Boolean
        Try
            Return ObjDALProduct.CheckItemUOMandGetfromDB(Err_No, Err_Desc, ItemCode, OrgID, UOM)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
