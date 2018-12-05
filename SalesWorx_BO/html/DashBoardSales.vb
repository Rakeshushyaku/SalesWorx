Public Class DashBoardSales
    Public Sub New()
    End Sub

    Public Sub New(ByVal unit As String, ByVal Description As String)
        ' TODO: Complete member initialization
        'Me.Country = country

        Me.Amount = Amount
        Me.ReturnAmount = ReturnAmount
        Me.Description = Description
        Me.Amount1 = Amount1
        Me.Amount2 = Amount2
        Me.Amount3 = Amount3
        Me.Count = Count
        Me.Count1 = Count1
        '  Me.DemoRate = DemoRate
        'Me.Solar = solar
        'Me.Hydro = hydro
        'Me.Wind = wind
        'Me.Nuclear = nuclear
    End Sub
    Public Property Count1() As Decimal
        Get
            Return m_Count1
        End Get
        Set(ByVal value As Decimal)
            m_Count1 = value
        End Set
    End Property
    Private m_Count1 As Decimal
    Public Property Count() As Decimal
        Get
            Return m_Count
        End Get
        Set(ByVal value As Decimal)
            m_Count = value
        End Set
    End Property
    Private m_Count As Decimal

    Public Property ReturnAmount() As Decimal
        Get
            Return m_ReturnAmount
        End Get
        Set(ByVal value As Decimal)
            m_ReturnAmount = value
        End Set
    End Property
    Private m_ReturnAmount As Decimal


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
    Public Property Description1() As String
        Get
            Return m_Description1
        End Get
        Set(ByVal value As String)
            m_Description1 = value
        End Set
    End Property
    Private m_Description1 As String

     
    Public Property Amount1() As Decimal
        Get
            Return m_Amount1
        End Get
        Set(ByVal value As Decimal)
            m_Amount1 = value
        End Set
    End Property
    Private m_Amount1 As Decimal


    Public Property Amount2() As Decimal
        Get
            Return m_Amount2
        End Get
        Set(ByVal value As Decimal)
            m_Amount2 = value
        End Set
    End Property
    Private m_Amount2 As Decimal

    Public Property Amount3() As Decimal
        Get
            Return m_Amount3
        End Get
        Set(ByVal value As Decimal)
            m_Amount3 = value
        End Set
    End Property
    Private m_Amount3 As Decimal




    Public Property Description2() As String
        Get
            Return m_Description2
        End Get
        Set(ByVal value As String)
            m_Description2 = value
        End Set
    End Property
    Private m_Description2 As String
End Class
