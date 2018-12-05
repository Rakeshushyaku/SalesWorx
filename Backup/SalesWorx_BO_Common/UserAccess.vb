Public Class UserAccess

    Private _sUserID As Integer
    Private _sSalesRepID As Integer
    Private _AssignedSalesReps As ArrayList
    Private _IUserType As Integer
    Private _IsSS As Char
    Private _SalesRep_Name As String
    Private _Van_Name As ArrayList
    Private _sMenuID As ArrayList
    Private _sPageID As ArrayList
    Private _sFieldRights As ArrayList
    Private _sOrgID As ArrayList
    Private _sSite As ArrayList
    Private _lPDARights As String
    Private _sDesignation As String

    Private _sCurrencyCode As String
    Private _sDecimalDigits As String
    Private _sLabelDecimalDigits As String

    Public Property LabelDecimalDigits() As String
        Get
            LabelDecimalDigits = _sLabelDecimalDigits
        End Get
        Set(ByVal Value As String)
            _sLabelDecimalDigits = Value
        End Set
    End Property

    Public Property DecimalDigits() As String
        Get
            DecimalDigits = _sDecimalDigits
        End Get
        Set(ByVal Value As String)
            _sDecimalDigits = Value
        End Set
    End Property

    Public Property CurrencyCode() As String
        Get
            CurrencyCode = _sCurrencyCode
        End Get
        Set(ByVal Value As String)
            _sCurrencyCode = Value
        End Set
    End Property


    Public Property UserID() As Integer
        Get
            UserID = _sUserID
        End Get
        Set(ByVal Value As Integer)
            _sUserID = Value
        End Set
    End Property

    Public Property SalesRepID() As Integer
        Get
            SalesRepID = _sSalesRepID
        End Get
        Set(ByVal Value As Integer)
            _sSalesRepID = Value
        End Set
    End Property

    Public Property AssignedSalesReps() As ArrayList
        Get
            AssignedSalesReps = _AssignedSalesReps
        End Get
        Set(ByVal Value As ArrayList)
            _AssignedSalesReps = Value
        End Set
    End Property

    Public Property VanName() As ArrayList
        Get
            VanName = _Van_Name
        End Get
        Set(ByVal Value As ArrayList)
            _Van_Name = Value
        End Set
    End Property

    Public Property IsSS() As Char
        Get
            IsSS = _IsSS
        End Get
        Set(ByVal Value As Char)
            _IsSS = Value
        End Set
    End Property
    Public Property UserType() As Integer
        Get
            UserType = _IUserType
        End Get
        Set(ByVal Value As Integer)
            _IUserType = Value
        End Set
    End Property
    Public Property SalesRep_Name() As String
        Get
            SalesRep_Name = _SalesRep_Name
        End Get
        Set(ByVal Value As String)
            _SalesRep_Name = Value
        End Set
    End Property
    Public Property MenuID() As ArrayList
        Get
            MenuID = _sMenuID
        End Get
        Set(ByVal Value As ArrayList)
            _sMenuID = Value
        End Set
    End Property

    Public Property PageID() As ArrayList
        Get
            PageID = _sPageID
        End Get
        Set(ByVal Value As ArrayList)
            _sPageID = Value
        End Set
    End Property

    Public Property FieldRights() As ArrayList
        Get
            FieldRights = _sFieldRights
        End Get
        Set(ByVal Value As ArrayList)
            _sFieldRights = Value
        End Set
    End Property
    Public Property OrgID() As ArrayList
        Get
            OrgID = _sOrgID
        End Get
        Set(ByVal Value As ArrayList)
            _sOrgID = Value
        End Set
    End Property

    Public Property Site() As ArrayList
        Get
            Site = _sSite
        End Get
        Set(ByVal Value As ArrayList)
            _sSite = Value
        End Set
    End Property

    Public Property PDARights() As String
        Get
            PDARights = _lPDARights
        End Get
        Set(ByVal Value As String)
            _lPDARights = Value
        End Set
    End Property

    Public Property Designation() As String
        Get
            Designation = _sDesignation
        End Get
        Set(ByVal Value As String)
            _sDesignation = Value
        End Set
    End Property
End Class
