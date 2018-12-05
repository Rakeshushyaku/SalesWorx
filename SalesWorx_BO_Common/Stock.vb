Public Class Stock
    Dim ObjStock As New DAL.DAL_Stock
    Private _sSalesMan As String = ""
    Private _sVanMan As String = ""
    Private _sProduct As String = ""
    Private _sQty As String = "0"
    Private _sOrg_ID As String = ""
    Public _sUOM As String = ""
    Public Property SalesMan() As String
        Get
            Return Me._sSalesMan
        End Get

        Set(ByVal value As String)
            Me._sSalesMan = value
        End Set
    End Property
    Public Property Van() As String
        Get
            Return Me._sVanMan
        End Get

        Set(ByVal value As String)
            Me._sVanMan = value
        End Set
    End Property

    Public Property Product() As String
        Get
            Return Me._sProduct
        End Get

        Set(ByVal value As String)
            Me._sProduct = value
        End Set
    End Property
    Public Property Qty() As String
        Get
            Return Me._sQty
        End Get

        Set(ByVal value As String)
            Me._sQty = value
        End Set
    End Property
    Public Property Org_ID() As String
        Get
            Return Me._sOrg_ID
        End Get

        Set(ByVal value As String)
            Me._sOrg_ID = value
        End Set
    End Property
    Public Property UOM() As String
        Get
            Return Me._sUOM
        End Get

        Set(ByVal value As String)
            Me._sUOM = value
        End Set
    End Property

    Public Function ConfirmStockRequisitionbyID(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal StockRequisition_ID As String, ByVal Confirmedby As String, ByVal PORefno As String) As Boolean
        Try
            Return ObjStock.ConfirmStockRequisitionbyID(Err_No, Err_Desc, StockRequisition_ID, Confirmedby, PORefno)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ConfirmStockRequisition(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRep As String, ByVal Agency As String, ByVal Confirmedby As String, ByVal PORefno As String) As Boolean
        Try
            Return ObjStock.ConfirmStockRequisition(Err_No, Err_Desc, SalesRep, Agency, Confirmedby, PORefno)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ConfirmStockRequisitionbyVan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRep As String, ByVal Confirmedby As String, ByVal PORefno As String) As Boolean
        Try
            Return ObjStock.ConfirmStockRequisitionbyVan(Err_No, Err_Desc, SalesRep, Confirmedby, PORefno)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function UpdateStockRequisition(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String) As Boolean
        Try
            Return ObjStock.UpdateStockRequisition(Err_No, Err_Desc, _sSalesMan, _sProduct, Qty, Org_ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function UpdateStockRequisitionByvan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String) As Boolean
        Try
            Return ObjStock.UpdateStockRequisitionByvan(Err_No, Err_Desc, _sVanMan, _sProduct, Qty, Org_ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetOrgsHeads(ByRef Err_No As Long, ByRef Err_Desc As String) As DataSet
        Try
            Return ObjStock.GetOrgsHeads(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSalesMan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataSet
        Try
            Return ObjStock.GetSalesMan(Err_No, Err_Desc, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetAgencyList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_id As String) As DataSet
        Try
            Return ObjStock.GetAgencyList(Err_No, Err_Desc, Org_id)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
     
    Function GetAgencyList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_id As String, ByVal ReqDate As String) As DataSet
        Try
            Return ObjStock.GetAgencyList(Err_No, Err_Desc, Org_id, ReqDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    
    Function GetAgencyListbySalesMan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_id As String, ByVal SalesMan As String) As DataSet
        Try
            Return ObjStock.GetAgencyListbySalesMan(Err_No, Err_Desc, Org_id, SalesMan)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
Function GetItemListbySalesMan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_id As String) As DataSet
        Try
            Return ObjStock.GetItemListbySalesMan(Err_No, Err_Desc, Org_id)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetStockRequisition(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Agency As String, ByVal Org_id As String) As DataTable
        Try
            Return ObjStock.GetStockRequisition(Err_No, Err_Desc, Agency, Org_id)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetStockRequisitionbyVan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Agency As String, ByVal Org_id As String) As DataTable
        Try
            Return ObjStock.GetStockRequisitionbyVan(Err_No, Err_Desc, Agency, Org_id)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetStockRequisition_UOM(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Agency As String, ByVal Org_id As String) As DataTable
        Try
            Return ObjStock.GetStockRequisition_UOM(Err_No, Err_Desc, Agency, Org_id)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetNotConfirmed(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjStock.GetNotConfirmed(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function CheckStockReqConfirmed(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Agency As String, ByVal Org_id As String, ByRef ConfirmedAt As String) As Boolean
        Try
            Return ObjStock.CheckStockReqConfirmed(Err_No, Err_Desc, Agency, Org_id, ConfirmedAt)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetWH_Type(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_id As String) As String
        Try
            Dim WHType As String
            WHType = ObjStock.GetWH_Type(Err_No, Err_Desc, Org_id)
            If WHType.ToUpper = "3P" Then
                Return "N"
            Else
                Return "Y"
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetStockGenerate(ByRef Err_No As Long, ByRef Err_Desc As String) As String
        Try
            Dim WHType As String
            WHType = ObjStock.GetStockGenerate(Err_No, Err_Desc)
            Return WHType
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetRequestedQty(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal SalesRep As String, ByVal Agency As String, ByVal SKU As String) As String
        Try
            Return ObjStock.GetRequestedQty(Err_No, Err_Desc, OrgID, SalesRep, Agency, SKU)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetRequestedQty(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal SalesRep As String, ByVal SKU As String) As String
        Try
            Return ObjStock.GetRequestedQty(Err_No, Err_Desc, OrgID, SalesRep, SKU)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetProductsByOrg_Agency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String, Optional ByVal Agency As String = "0") As DataTable
        Return ObjStock.GetProductsByOrg_Agency(Err_No, Err_Desc, Org_ID, Agency)
    End Function

  Function GetUnconfirmedStockRequisition(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_id As String, ByVal Van As String) As DataTable
        Try
            Return ObjStock.GetUnconfirmedStockRequisition(Err_No, Err_Desc, Org_id, Van)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function StockRequisitionItemsbyOrgforExport(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Try
            Return ObjStock.StockRequisitionItemsbyOrgforExport(Err_No, Err_Desc, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
      Function StockRequisitionItems(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RowID As String) As DataTable
        Try
            Return ObjStock.StockRequisitionItems(Err_No, Err_Desc, RowID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function StockRequisitionItemsforExport(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RowID As String) As DataTable
        Try
            Return ObjStock.StockRequisitionItemsForExport(Err_No, Err_Desc, RowID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function IsValidVan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Van As String, ByRef Salesrep_ID As String) As Boolean
        Try
            Return ObjStock.IsValidVan(Err_No, Err_Desc, OrgID, Van, Salesrep_ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
     Public Function ValidItemUOM(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Item As String, ByVal UOM As String, ByRef Conversion As String) As Boolean
        Try
            Return ObjStock.ValidItemUOM(Err_No, Err_Desc, OrgID, Item, UOM, Conversion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ValidItem(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Item As String, ByRef Item_ID As String) As Boolean
        Try
            Return ObjStock.ValidItem(Err_No, Err_Desc, OrgID, Item, Item_ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
     Public Function AlreadyConfirmed(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Salesrep_ID As String) As Boolean
        Try
            Return ObjStock.AlreadyConfirmed(Err_No, Err_Desc, OrgID, Salesrep_ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ImportStockRequisitions(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal dt As DataTable, ByRef errorDt As DataTable) As Boolean
        Try
            Return ObjStock.ImportStockRequisitions(Err_No, Err_Desc, OrgID, dt, errorDt)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetConverion(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Item As String, UOm As String) As String
        Try
            Return ObjStock.GetConverion(Err_No, Err_Desc, OrgID, Item, UOm)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetItemUOM(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Item As String) As String
        Try
            Return ObjStock.GetItemUOM(Err_No, Err_Desc, OrgID, Item)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetItemUOMs(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Item As String) As DataTable
        Try
            Return ObjStock.GetItemUOMs(Err_No, Err_Desc, OrgID, Item)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSalesRepforPurchaseReport(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Agency As String, ByVal SDate As String) As DataTable
        Try
            Return ObjStock.SalesRepforPurchaseReport(Err_No, Err_Desc, OrgID, Agency, SDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetPurchaseReport(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Agency As String, ByVal SDate As String, ByVal Sid As String) As DataTable
        Try
            Return ObjStock.GetPurchaseReport(Err_No, Err_Desc, OrgID, Agency, SDate, Sid)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function HasLots(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Item_Code As String) As Boolean
        Try
            Return ObjStock.HasLots(Err_No, Err_Desc, OrgID, Item_Code)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetImportStockTbls(ByRef Err_No As Long, ByRef Err_Desc As String) As DataSet
        Try
            Return ObjStock.GetImportStockTbls(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetItemDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Item_code As String) As DataTable
        Try
            Return ObjStock.GetItemDetails(Err_No, Err_Desc, OrgID, Item_code)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ImportStock(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal xml_str As String, ByVal OrgID As String) As Boolean
        Try
            Return ObjStock.ImportStock(Err_No, Err_Desc, xml_str, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function Get_ImportStockUnConfirm(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal xml_str As String, ByVal OrgID As String) As DataSet
        Try
            Return ObjStock.Get_ImportStockUnConfirm(Err_No, Err_Desc, xml_str, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Get_ImportStockUnConfirm_Updated(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal xml_str As String, ByVal OrgID As String) As DataSet
        Try
            Return ObjStock.Get_ImportStockUnConfirmUpdated(Err_No, Err_Desc, xml_str, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Get_ImportStockConfirm(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal xml_str As String, ByVal OrgID As String, UID As Integer) As Boolean
        Try
            Return ObjStock.Get_ImportStockConfirm(Err_No, Err_Desc, xml_str, OrgID, UID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function IsValidItem(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByRef Item_Code As String) As Boolean
        Try
            Return ObjStock.IsValidItem(Err_No, Err_Desc, OrgID, Item_Code)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function CheckStockReqConfirmedByVan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal van As String, ByVal Org_id As String, ByRef ConfirmedAt As String) As Boolean
        Try
            Return ObjStock.CheckStockReqConfirmedByVan(Err_No, Err_Desc, van, Org_id, ConfirmedAt)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetUOMforConverion(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Item As String) As String
        Try
            Return ObjStock.GetUOMforConverion(Err_No, Err_Desc, OrgID, Item)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetConversion(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Item As String, ByVal UOM As String) As String
        Try
            Return ObjStock.GetConversion(Err_No, Err_Desc, OrgID, Item, UOM)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ValidUOM(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Item As String, ByVal UOM As String) As Boolean
        Try
            Return ObjStock.ValidUOM(Err_No, Err_Desc, OrgID, Item, UOM)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
