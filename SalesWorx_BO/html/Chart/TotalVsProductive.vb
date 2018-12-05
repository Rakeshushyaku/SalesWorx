Public Class TotalVsProductive

    Public Sub New()
    End Sub

    Public Sub New(ByVal TotValue As String, ByVal Description As String)

        Me.Unit = TotValue
        Me.Description = Description

    End Sub

    Public Property Unit() As Decimal
        Get
            Return m_Unit
        End Get
        Set(ByVal value As Decimal)
            m_Unit = value
        End Set
    End Property
    Private m_Unit As Decimal

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