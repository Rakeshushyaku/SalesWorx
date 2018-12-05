Public Class UserRoleAcces
    Private _sFieldID As String
    Private _dtLastModifiedAt As DateTime = Now
    Private _sLastModifiedBy As String
    Private _sMenuID As String
    Private _sPageID As String

    Private _iUserRoleAccessID As Integer
    Public Property FieldID() As String
        Get
            FieldID = _sFieldID
        End Get
        Set(ByVal Value As String)
            _sFieldID = Value
        End Set
    End Property
    Public Property LastModifiedAt() As DateTime
        Get
            LastModifiedAt = _dtLastModifiedAt
        End Get
        Set(ByVal Value As DateTime)
            _dtLastModifiedAt = Value
        End Set
    End Property
    Public Property LastModifiedBy() As String
        Get
            LastModifiedBy = _sLastModifiedBy
        End Get
        Set(ByVal Value As String)
            _sLastModifiedBy = Value
        End Set
    End Property
    Public Property MenuID() As String
        Get
            MenuID = _sMenuID
        End Get
        Set(ByVal Value As String)
            _sMenuID = Value
        End Set
    End Property
    Public Property PageID() As String
        Get
            PageID = _sPageID
        End Get
        Set(ByVal Value As String)
            _sPageID = Value
        End Set
    End Property
    
    Public Property UserRoleAccessID() As Integer
        Get
            UserRoleAccessID = _iUserRoleAccessID
        End Get
        Set(ByVal Value As Integer)
            _iUserRoleAccessID = Value
        End Set
    End Property
End Class
