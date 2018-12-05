
Public Class TargetVsSalesbyVan

    Public Sub New()
    End Sub

    Public Sub New(ByVal AgencyName As String, ByVal unit As String, ByVal Description As String, ByVal DispOrder As String, MonthYear As String, Count As String)
        ' TODO: Complete member initialization
        'Me.Country = country
        Me.AgencyName = AgencyName
        Me.Unit = unit
        Me.Description = Description
        Me.DispOrder = DispOrder
        Me.MonthYear = MonthYear
        Me.Count = Count
        '  Me.DemoRate = DemoRate
        'Me.Solar = solar
        'Me.Hydro = hydro
        'Me.Wind = wind
        'Me.Nuclear = nuclear
    End Sub

    Public Property Count() As String
        Get
            Return m_Count
        End Get
        Set(ByVal value As String)
            m_Count = value
        End Set
    End Property
    Private m_Count As String
    Public Property MonthYear() As String
        Get
            Return m_MonthYear
        End Get
        Set(ByVal value As String)
            m_MonthYear = value
        End Set
    End Property
    Private m_MonthYear As String

    Public Property AgencyName() As String
        Get
            Return m_AgencyName
        End Get
        Set(ByVal value As String)
            m_AgencyName = value
        End Set
    End Property
    Private m_AgencyName As String
    Public Property Unit() As Decimal
        Get
            Return m_Unit
        End Get
        Set(ByVal value As Decimal)
            m_Unit = value
        End Set
    End Property
    Private m_Unit As Decimal
    Public Property DispOrder() As String
        Get
            Return m_DispOrder
        End Get
        Set(ByVal value As String)
            m_DispOrder = value
        End Set
    End Property
    Private m_DispOrder As String
    Public Property Description() As String
        Get
            Return m_Description
        End Get
        Set(ByVal value As String)
            m_Description = value
        End Set
    End Property
    Private m_Description As String
End Class