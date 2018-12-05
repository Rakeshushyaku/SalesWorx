Public Class DashBoardSales
    Public Sub New()
    End Sub

    Public Sub New(ByVal unit As String, ByVal Description As String)
        ' TODO: Complete member initialization
        'Me.Country = country

        Me.Amount = Amount
        Me.Description = Description
        '  Me.DemoRate = DemoRate
        'Me.Solar = solar
        'Me.Hydro = hydro
        'Me.Wind = wind
        'Me.Nuclear = nuclear
    End Sub



    Public Property Amount() As Decimal
        Get
            Return m_Amount
        End Get
        Set(ByVal value As Decimal)
            m_Amount = value
        End Set
    End Property
    Private m_Amount As Decimal

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
