Public Class TargetVsAchiv

    Public Sub New()
    End Sub

    Public Sub New(ByVal MonYear As String, ByVal Description As String, ByVal TargentValue As String, ByVal AchValue As String, TotalCalls As String, ProductiveCalls As String, MonthYear As Date, Type As String)
        ' TODO: Complete member initialization
        'Me.Country = country
        Me.MonYear = MonYear
        Me.Description = Description
        Me.TargentValue = TargentValue
        Me.AchValue = AchValue

        Me.TotalCalls = TotalCalls
        Me.ProductiveCalls = ProductiveCalls

        Me.MonthYear = MonthYear
        Me.Type = Type
      

        '  Me.DemoRate = DemoRate
        'Me.Solar = solar
        'Me.Hydro = hydro
        'Me.Wind = wind
        'Me.Nuclear = nuclear
    End Sub


    Public Property Type() As String
        Get
            Return m_Type
        End Get
        Set(ByVal value As String)
            m_Type = value
        End Set
    End Property
    Private m_Type As String

    Public Property MonthYear() As Date
        Get
            Return m_MonthYear
        End Get
        Set(ByVal value As Date)
            m_MonthYear = value
        End Set
    End Property
    Private m_MonthYear As Date
    Public Property Description() As String
        Get
            Return m_Description
        End Get
        Set(ByVal value As String)
            m_Description = value
        End Set
    End Property
    Private m_Description As String

    Public Property TargentValue() As String
        Get
            Return m_TargentValue
        End Get
        Set(ByVal value As String)
            m_TargentValue = value
        End Set
    End Property
    Private m_TargentValue As String

    Public Property AchValue() As String
        Get
            Return m_AchValue
        End Get
        Set(ByVal value As String)
            m_AchValue = value
        End Set
    End Property
    Private m_AchValue As String
    Public Property TotalCalls() As String
        Get
            Return m_TotalCalls
        End Get
        Set(ByVal value As String)
            m_TotalCalls = value
        End Set
    End Property
    Private m_TotalCalls As String
   

    Public Property ProductiveCalls() As String
        Get
            Return m_ProductiveCalls
        End Get
        Set(ByVal value As String)
            m_ProductiveCalls = value
        End Set
    End Property
    Private m_ProductiveCalls As String
    Public Property MonYear() As String
        Get
            Return m_MonYear
        End Get
        Set(ByVal value As String)
            m_MonYear = value
        End Set
    End Property
    Private m_MonYear As String
    

  
End Class