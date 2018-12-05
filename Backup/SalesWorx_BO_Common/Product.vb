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

    Function GetDiscountData(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Try
            Return ObjDALProduct.LoadDiscountData(Err_No, Err_Desc, OrgID)
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

    Public Function ExportBonusData(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal BnsPlanID As Integer) As DataTable
        Try
            Return ObjDALProduct.ExportBonusData(Err_No, Err_Desc, OrgID, BnsPlanID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function UpdateBonusData(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal LineId As String, ByVal BItemCode As String, ByVal BUOM As String, ByVal TypeCode As String, ByVal FromQty As Long, ByVal ToQty As Long, ByVal GetQty As Long, ByVal GetPer As Decimal, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, ByVal CreatedBy As Integer, ByVal BnsPlanID As Integer) As Boolean
        Try
            Return ObjDALProduct.UpdateBonusData(Error_No, Error_Desc, LineId, BItemCode, BUOM, TypeCode, FromQty, ToQty, GetQty, GetPer, ValidFrom, ValidTo, CreatedBy, BnsPlanID)
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
    Public Function UploadDiscount(ByVal dtData As DataTable, ByVal OrgID As String, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDALProduct.UploadDiscount(dtData, OrgID, Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function SaveBonusData(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal ItemCode As String, ByVal OrgId As String, ByVal DUOm As String, ByVal BItemCode As String, ByVal BUOM As String, ByVal TypeCode As String, ByVal FromQty As Long, ByVal ToQty As Long, ByVal GetQty As Long, ByVal GetPer As Decimal, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, ByVal UpdatedBy As Integer, ByVal BnsPlanID As Integer) As Boolean
        Try
            Return ObjDALProduct.SaveBonusData(Error_No, Error_Desc, ItemCode, OrgId, DUOm, BItemCode, BUOM, TypeCode, FromQty, ToQty, GetQty, GetPer, ValidFrom, ValidTo, UpdatedBy, BnsPlanID)
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
    Function DeleteSimpleBonusPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PlanId As String) As Boolean
        Try
            Return ObjDALProduct.DeleteSimpleBonusPlan(Err_No, Err_Desc, PlanId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function CheckBonusDataActiveRange(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal FromQty As Long, ByVal ToQty As Long, ByVal OrgID As String, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, ByVal LineId As Integer) As DataTable
        Try
            Return ObjDALProduct.CheckBonusDataActiveRange(Err_No, Err_Desc, ItemCode, FromQty, ToQty, OrgID, ValidFrom, ValidTo, LineID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function UpdateDiscountData(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal LineID As Integer, ByVal ItemCode As String, ByVal OrgId As String, ByVal TypeCode As String, ByVal FromQty As Long, ByVal Rate As Decimal, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime) As Boolean
        Try
            Return ObjDALProduct.UpdateDiscountData(Error_No, Error_Desc, LineID, ItemCode, OrgId, TypeCode, FromQty, Rate, ValidFrom, ValidTo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SaveDiscountData(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal ItemCode As String, ByVal OrgId As String, ByVal TypeCode As String, ByVal FromQty As Long, ByVal Rate As Decimal, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime) As Boolean
        Try
            Return ObjDALProduct.SaveDiscountData(Error_No, Error_Desc, ItemCode, OrgId, TypeCode, FromQty, Rate, ValidFrom, ValidTo)
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
    Function CheckDiscountDataActiveRange(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal FromQty As Long, ByVal OrgID As String, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, ByVal LineId As Integer) As Boolean
        Try
            Return ObjDALProduct.CheckDiscountDataRange(Err_No, Err_Desc, ItemCode, FromQty, OrgID, ValidFrom, ValidTo, LineId)
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
    Function UpdateBonusPlan(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal PlanId As String, ByVal PlanName As String, ByVal CreatedBy As Integer, ByVal IsActive As String, ByVal ValidFrom As DateTime, ByVal Validto As DateTime) As Boolean
        Try
            Return ObjDALProduct.UpdateBonusPlan(Error_No, Error_Desc, PlanId, PlanName, CreatedBy, IsActive, ValidFrom, Validto)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function UpdateSimpleBonusPlan(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal PlanId As String, ByVal PlanName As String, ByVal CreatedBy As Integer) As Boolean
        Try
            Return ObjDALProduct.UpdateSimpleBonusPlan(Error_No, Error_Desc, PlanId, PlanName, CreatedBy)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function SaveBonusPlan(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal OrgId As String, ByVal PlanName As String, ByVal CreatedBy As Integer, ByRef PlanId As String, ByVal IsActive As String, ByVal ValidFrom As DateTime, ByVal Validto As DateTime) As String
        Try
            Return ObjDALProduct.SaveBonusPlan(Error_No, Error_Desc, OrgId, PlanName, CreatedBy, PlanId, IsActive, ValidFrom, Validto)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function SaveSimpleBonusPlan(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal OrgId As String, ByVal PlanName As String, ByVal CreatedBy As Integer, ByRef PlanId As String) As String
        Try
            Return ObjDALProduct.SaveSimpleBonusPlan(Error_No, Error_Desc, OrgId, PlanName, CreatedBy, PlanId)
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


            Dim MyRow As DataRow

            If TempTbl IsNot Nothing Then
                For Each dr As DataRow In TempTbl.Rows
                    MyRow = ProdTbl.NewRow()
                    MyRow(0) = dr.Item("Inventory_Item_ID")
                    MyRow(1) = "[" & dr.Item("Item_Code") & "]-" & dr.Item("Description")
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


            Dim MyRow As DataRow

            If TempTbl IsNot Nothing Then
                For Each dr As DataRow In TempTbl.Rows
                    MyRow = ProdTbl.NewRow()
                    MyRow(0) = dr.Item("Inventory_Item_ID")
                    MyRow(1) = "[" & dr.Item("Item_Code") & "]-" & dr.Item("Description")
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
    Public Function GetItemUOM(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal OrgID As String) As String
        Try
            Return ObjDALProduct.GetItemUOM(Err_No, Err_Desc, ItemCode, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function UploadFOCDiscount(ByVal dtData As DataTable, ByVal OrgID As String, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserID As String) As Boolean
        Try
            Return ObjDALProduct.UploadFOCDiscount(dtData, OrgID, Err_No, Err_Desc, UserID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
