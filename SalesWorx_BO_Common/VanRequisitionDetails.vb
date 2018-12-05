Public Class VanRequisitionDetails
#Region "Private Variables"
    Private _StockRequisition_Item_ID As Object
    Private _StockRequisition_ID As Object
    Private _Inventory_Item_ID As Int32
    Private _Inventory_Item As String
    Private _ItemCode As String
    Private _BrandCode As String
    Private _Qty As Double
    Private _Item_UOM As String
    Private objVanReqDetails As New DAL.DAL_VanRequisitionDetails
#End Region

#Region "Public Property"
    Public Property StockRequisition_Item_ID() As Object
        Get
            Return _StockRequisition_Item_ID
        End Get
        Set(ByVal value As Object)
            _StockRequisition_Item_ID = value
        End Set
    End Property
    Public Property StockRequisition_ID() As Object
        Get
            Return _StockRequisition_ID
        End Get
        Set(ByVal value As Object)
            _StockRequisition_ID = value
        End Set
    End Property
    Public Property Inventory_Item_ID() As Int32
        Get
            Return _Inventory_Item_ID
        End Get
        Set(ByVal value As Int32)
            _Inventory_Item_ID = value
        End Set
    End Property
    Public Property Qty() As Double
        Get
            Return _Qty
        End Get
        Set(ByVal value As Double)
            _Qty = value
        End Set
    End Property
    Public Property Item_UOM() As String
        Get
            Return _Item_UOM
        End Get
        Set(ByVal value As String)
            _Item_UOM = value
        End Set
    End Property
    Public Property Item() As String
        Get
            Return _Inventory_Item
        End Get
        Set(ByVal value As String)
            _Inventory_Item = value
        End Set
    End Property
    Public Property ItemCode() As String
        Get
            Return _ItemCode
        End Get
        Set(ByVal value As String)
            _ItemCode = value
        End Set
    End Property
    Public Property BrandCode() As String
        Get
            Return _BrandCode
        End Get
        Set(ByVal value As String)
            _BrandCode = value
        End Set
    End Property
#End Region

    Public Function GetVanRequisitionDetails(ByVal SalesRep_ID As Integer) As DataTable
        Return objVanReqDetails.GetVanRequisitionDetails(_StockRequisition_ID, SalesRep_ID)
    End Function

End Class
