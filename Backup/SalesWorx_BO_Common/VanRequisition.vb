Public Class VanRequisition
#Region "Private Variables"
    Private _StockRequisition_ID As Object
    Private _Request_Date As DateTime
    Private _SalesRep_ID As Int32
    Private _Dest_Org_ID As String
    Private _SalesRep_Name As String
    Private _Comments As String
    Private _Signature As Byte()
    Private _Status As String
    Private _Emp_Code As String
    Private _Emp_Name As String
    Private _Approved_By As Int32
    Dim objDALVanRequisitions As DAL.DAL_VanRequisitions
#End Region

#Region "Public Property"
    Public Property StockRequisition_ID() As Object
        Get
            Return _StockRequisition_ID
        End Get
        Set(ByVal value As Object)
            _StockRequisition_ID = value
        End Set
    End Property
    Public Property Request_Date() As DateTime
        Get
            Return _Request_Date
        End Get
        Set(ByVal value As DateTime)
            _Request_Date = value
        End Set
    End Property
    Public Property SalesRep_ID() As Int32
        Get
            Return _SalesRep_ID
        End Get
        Set(ByVal value As Int32)
            _SalesRep_ID = value
        End Set
    End Property
    Public Property Dest_Org_ID() As String
        Get
            Return _Dest_Org_ID
        End Get
        Set(ByVal value As String)
            _Dest_Org_ID = value
        End Set
    End Property
    Public Property SalesRepName() As String
        Get
            Return _SalesRep_Name
        End Get
        Set(ByVal value As String)
            _SalesRep_Name = value
        End Set
    End Property
    Public Property Comments() As String
        Get
            Return _Comments
        End Get
        Set(ByVal value As String)
            _Comments = value
        End Set
    End Property
    Public Property Signature() As Byte()
        Get
            Return _Signature
        End Get
        Set(ByVal value As Byte())
            _Signature = value
        End Set
    End Property
    Public Property Status() As String
        Get
            Return _Status
        End Get
        Set(ByVal value As String)
            _Status = value
        End Set
    End Property
    Public Property Emp_Code() As String
        Get
            Return _Emp_Code
        End Get
        Set(ByVal value As String)
            _Emp_Code = value
        End Set
    End Property
    Public Property Emp_Name() As String
        Get
            Return _Emp_Name
        End Get
        Set(ByVal value As String)
            _Emp_Name = value
        End Set
    End Property
    Public Property Approved_By() As Int32
        Get
            Return _Approved_By
        End Get
        Set(ByVal value As Int32)
            _Approved_By = value
        End Set
    End Property

#End Region
#Region "Constructor"
    Public Sub New()
        objDALVanRequisitions = New DAL.DAL_VanRequisitions()
    End Sub
#End Region
#Region "Public Method"
    Public Function GetOutstandingStockRequistion(ByVal Criteria As String) As DataTable
        Try
            Return objDALVanRequisitions.GetOutstandingStockRequistion(Criteria)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function Load() As Boolean
        Dim dt As New DataTable
        dt = GetOutstandingStockRequistion("StockRequisition_ID='" + _StockRequisition_ID + "'")
        Dim dr As DataRow = Nothing
        If dt.Rows.Count > 0 Then
            dr = dt.Rows(0)
            _SalesRep_ID = dr("SalesRep_ID")
            _SalesRep_Name = dr("SalesRep_Name").ToString()
            _Emp_Code = dr("Emp_Code").ToString()
            _Emp_Name = dr("Emp_Name").ToString()
            _Request_Date = dr("Request_Date").ToString()
            _Dest_Org_ID = dr("Dest_Org_ID").ToString()
            _Comments = dr("Comments").ToString()
            _Status = dr("Status").ToString()
            If Not IsDBNull(dr("Signature")) Then
                _Signature = dr("Signature")
            End If
        End If
    End Function
    Public Function Approve()
        Try
            Return objDALVanRequisitions.Approve(_Approved_By, _StockRequisition_ID.ToString())
        Catch ex As Exception
            Throw ex
        End Try
    End Function

#End Region
End Class
