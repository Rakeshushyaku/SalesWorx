
Public Class ControlParams

    Private _ALLOW_EXCESS_CASH_COLLECTION As String
    Private _ALLOW_LOAD_QTY_CHANGE As String
    Private _ALLOW_ORDER_DISCOUNT As String
    Private _ALLOW_PARTIAL_UNLOAD As String
    Private _ALLOW_UNLOAD_QTY_CHANGE As String
    Private _CN_LIMIT_MODE As String
    Private _COLLECTION_MODE As String
    Private _DISCOUNT_MODE As String
    Private _ENABLE_COLLECTIONS As String
    Private _ENABLE_CUSTOMER_SIGNATURE As String
    Private _ENABLE_DISTRIB_CHECK As String
    Private _ENABLE_LOT_SELECTION As String
    Private _ENABLE_MARKET_SURVEY As String
    Private _ENABLE_ORDER_HISTORY As String
    Private _ENABLE_SHORT_DOC_REF As String
    Private _EOD_ON_UNLOAD As String
    Private _MERGE_RETURN_STOCK As String
    Private _OPTIONAL_RETURN_HDR_REASON As String
    Private _OPTIONAL_RETURN_LOT As String
    Private _RETURN_STOCK_MERGE_MODE As String
    Private _UNLOAD_QTY_CHANGE_MODE As String
    Private _VAN_LOAD_TYPE As String
    Private _VAN_UNLOAD_TYPE As String

    Public Property ALLOW_EXCESS_CASH_COLLECTION() As String
        Get
            ALLOW_EXCESS_CASH_COLLECTION = _ALLOW_EXCESS_CASH_COLLECTION
        End Get
        Set(ByVal Value As String)
            _ALLOW_EXCESS_CASH_COLLECTION = Value
        End Set
    End Property
    Public Property ALLOW_LOAD_QTY_CHANGE() As String
        Get
            ALLOW_LOAD_QTY_CHANGE = _ALLOW_LOAD_QTY_CHANGE
        End Get
        Set(ByVal Value As String)
            _ALLOW_LOAD_QTY_CHANGE = Value
        End Set
    End Property

    Public Property ALLOW_ORDER_DISCOUNT() As String
        Get
            ALLOW_ORDER_DISCOUNT = _ALLOW_ORDER_DISCOUNT
        End Get
        Set(ByVal Value As String)
            _ALLOW_ORDER_DISCOUNT = Value
        End Set
    End Property
    Public Property ALLOW_PARTIAL_UNLOAD() As String
        Get
            ALLOW_PARTIAL_UNLOAD = _ALLOW_PARTIAL_UNLOAD
        End Get
        Set(ByVal Value As String)
            _ALLOW_PARTIAL_UNLOAD = Value
        End Set
    End Property
    Public Property ALLOW_UNLOAD_QTY_CHANGE() As String
        Get
            ALLOW_UNLOAD_QTY_CHANGE = _ALLOW_UNLOAD_QTY_CHANGE
        End Get
        Set(ByVal Value As String)
            _ALLOW_UNLOAD_QTY_CHANGE = Value
        End Set
    End Property
    Public Property CN_LIMIT_MODE() As String
        Get
            CN_LIMIT_MODE = _CN_LIMIT_MODE
        End Get
        Set(ByVal Value As String)
            _CN_LIMIT_MODE = Value
        End Set
    End Property

    Public Property COLLECTION_MODE() As String
        Get
            COLLECTION_MODE = _COLLECTION_MODE
        End Get
        Set(ByVal Value As String)
            _COLLECTION_MODE = Value
        End Set
    End Property

    Public Property DISCOUNT_MODE() As String
        Get
            DISCOUNT_MODE = _DISCOUNT_MODE
        End Get
        Set(ByVal Value As String)
            _DISCOUNT_MODE = Value
        End Set
    End Property

    Public Property ENABLE_COLLECTIONS() As String
        Get
            ENABLE_COLLECTIONS = _ENABLE_COLLECTIONS
        End Get
        Set(ByVal Value As String)
            _ENABLE_COLLECTIONS = Value
        End Set
    End Property

    Public Property ENABLE_CUSTOMER_SIGNATURE() As String
        Get
            ENABLE_CUSTOMER_SIGNATURE = _ENABLE_CUSTOMER_SIGNATURE
        End Get
        Set(ByVal Value As String)
            _ENABLE_CUSTOMER_SIGNATURE = Value
        End Set
    End Property
    Public Property ENABLE_DISTRIB_CHECK() As String
        Get
            ENABLE_DISTRIB_CHECK = _ENABLE_DISTRIB_CHECK
        End Get
        Set(ByVal Value As String)
            _ENABLE_DISTRIB_CHECK = Value
        End Set
    End Property
    Public Property ENABLE_LOT_SELECTION() As String
        Get
            ENABLE_LOT_SELECTION = _ENABLE_LOT_SELECTION
        End Get
        Set(ByVal Value As String)
            _ENABLE_LOT_SELECTION = Value
        End Set
    End Property

    Public Property ENABLE_MARKET_SURVEY() As String
        Get
            ENABLE_MARKET_SURVEY = _ENABLE_MARKET_SURVEY
        End Get
        Set(ByVal Value As String)
            _ENABLE_MARKET_SURVEY = Value
        End Set
    End Property

    Public Property ENABLE_ORDER_HISTORY() As String
        Get
            ENABLE_ORDER_HISTORY = _ENABLE_ORDER_HISTORY
        End Get
        Set(ByVal Value As String)
            _ENABLE_ORDER_HISTORY = Value
        End Set
    End Property

    Public Property ENABLE_SHORT_DOC_REF() As String
        Get
            ENABLE_SHORT_DOC_REF = _ENABLE_SHORT_DOC_REF
        End Get
        Set(ByVal Value As String)
            _ENABLE_SHORT_DOC_REF = Value
        End Set
    End Property
    Public Property EOD_ON_UNLOAD() As String
        Get
            EOD_ON_UNLOAD = _EOD_ON_UNLOAD
        End Get
        Set(ByVal Value As String)
            _EOD_ON_UNLOAD = Value
        End Set
    End Property
    Public Property MERGE_RETURN_STOCK() As String
        Get
            MERGE_RETURN_STOCK = _MERGE_RETURN_STOCK
        End Get
        Set(ByVal Value As String)
            _MERGE_RETURN_STOCK = Value
        End Set
    End Property
    Public Property OPTIONAL_RETURN_HDR_REASON() As String
        Get
            OPTIONAL_RETURN_HDR_REASON = _OPTIONAL_RETURN_HDR_REASON
        End Get
        Set(ByVal Value As String)
            _OPTIONAL_RETURN_HDR_REASON = Value
        End Set
    End Property
    Public Property OPTIONAL_RETURN_LOT() As String
        Get
            OPTIONAL_RETURN_LOT = _OPTIONAL_RETURN_LOT
        End Get
        Set(ByVal Value As String)
            _OPTIONAL_RETURN_LOT = Value
        End Set
    End Property
    Public Property RETURN_STOCK_MERGE_MODE() As String
        Get
            RETURN_STOCK_MERGE_MODE = _RETURN_STOCK_MERGE_MODE
        End Get
        Set(ByVal Value As String)
            _RETURN_STOCK_MERGE_MODE = Value
        End Set
    End Property
    Public Property UNLOAD_QTY_CHANGE_MODE() As String
        Get
            UNLOAD_QTY_CHANGE_MODE = _UNLOAD_QTY_CHANGE_MODE
        End Get
        Set(ByVal Value As String)
            _UNLOAD_QTY_CHANGE_MODE = Value
        End Set
    End Property
    Public Property VAN_LOAD_TYPE() As String
        Get
            VAN_LOAD_TYPE = _VAN_LOAD_TYPE
        End Get
        Set(ByVal Value As String)
            _VAN_LOAD_TYPE = Value
        End Set
    End Property
    Public Property VAN_UNLOAD_TYPE() As String
        Get
            VAN_UNLOAD_TYPE = _VAN_UNLOAD_TYPE
        End Get
        Set(ByVal Value As String)
            _VAN_UNLOAD_TYPE = Value
        End Set
    End Property
End Class
