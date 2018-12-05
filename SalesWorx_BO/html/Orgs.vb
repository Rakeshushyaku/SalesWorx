Public Class Orgs
 Public Sub New()
    End Sub

    Public Sub New(ByVal unit As String, ByVal Description As String)
        ' TODO: Complete member initialization
        'Me.Country = country

        Me.id = id
        Me.name = name
        '  Me.DemoRate = DemoRate
        'Me.Solar = solar
        'Me.Hydro = hydro
        'Me.Wind = wind
        'Me.Nuclear = nuclear
    End Sub

    Public Property id() As Integer
        Get
            Return m_id
        End Get
        Set(ByVal value As Integer)
            m_id = value
        End Set
    End Property
    Private m_id As Decimal

    Public Property name() As String
        Get
            Return m_name
        End Get
        Set(ByVal value As String)
            m_name = value
        End Set
    End Property
    Private m_name As String
End Class
