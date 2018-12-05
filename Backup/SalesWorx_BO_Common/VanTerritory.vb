Public Class VanTerritory

    Private _sSalesRepID As Integer
    Private _sCustomerSegment As String
    Private _sSalesDistrict As String
    Private objVanTerritory As DAL.DAL_VanTerritory
    Public Property SalesRepID() As Integer
        Get
            SalesRepID = _sSalesRepID
        End Get
        Set(ByVal Value As Integer)
            _sSalesRepID = Trim(Value)
        End Set
    End Property
    Public Property CustomerSegment() As String
        Get
            CustomerSegment = _sCustomerSegment
        End Get
        Set(ByVal Value As String)
            _sCustomerSegment = Trim(Value)
        End Set
    End Property
    Public Property SalesDistrict() As String
        Get
            SalesDistrict = _sSalesDistrict
        End Get
        Set(ByVal Value As String)
            _sSalesDistrict = Trim(Value)
        End Set
    End Property
    Public Sub New()
        objVanTerritory = New DAL.DAL_VanTerritory
    End Sub
    Public Function ReturnAllSalesRep(ByRef ds As DataSet, ByRef Err_No As Long, ByRef Err_Desc As String)
        objVanTerritory.ReturnAllSalesRep(ds, Err_No, Err_Desc)
        If ds.Tables.Count > 0 Then
            Dim dr As DataRow
            dr = ds.Tables(0).NewRow()
            dr(0) = "0"
            dr(1) = "--Select a Van--"
            ds.Tables(0).Rows.InsertAt(dr, 0)
        End If
        Return True
    End Function
    Public Function ReturnAllCustomerSegments(ByRef ds As DataSet, ByRef Err_No As Long, ByRef Err_Desc As String)
        objVanTerritory.ReturnAllCustomerSegments(ds, Err_No, Err_Desc)
        If ds.Tables.Count > 0 Then
            Dim dr As DataRow
            dr = ds.Tables(0).NewRow()
            dr(0) = "0"
            dr(1) = "--Select Customer Segment--"
            ds.Tables(0).Rows.InsertAt(dr, 0)
        End If
        Return True
    End Function
    Public Function ReturnAllSalesDistricts(ByRef ds As DataSet, ByRef Err_No As Long, ByRef Err_Desc As String)
        objVanTerritory.ReturnAllSalesDistricts(ds, Err_No, Err_Desc)
        If ds.Tables.Count > 0 Then
            Dim dr As DataRow
            dr = ds.Tables(0).NewRow()
            dr(0) = "0"
            dr(1) = "--Select Sales District--"
            ds.Tables(0).Rows.InsertAt(dr, 0)
        End If
        Return True
    End Function
    Public Function ReturnAllVanTerritories(ByRef ds As DataSet, ByVal Criteria As String, ByRef Err_No As Long, ByRef Err_Desc As String)
        Return objVanTerritory.GetVanTerritories(ds, Criteria, Err_No, Err_Desc)
    End Function
    Public Function AssignVantoTerritory(ByRef Error_No As Long, ByRef Error_Desc As String) As Boolean
        Try
            Return objVanTerritory.AssignVanToSalesDistrict(_sCustomerSegment, _sSalesDistrict, _sSalesRepID, Error_No, Error_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function UpdateAssignVantoTerritory(ByVal ID As Integer, ByRef Error_No As Long, ByRef Error_Desc As String) As Boolean
        Try
            Return objVanTerritory.UpdateAssignVanToSalesDistrict(ID, _sCustomerSegment, _sSalesDistrict, _sSalesRepID, Error_No, Error_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function DeleteVanTerritoryAssignment(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal MultipleMapIDs As String) As Boolean
        Try
            Return objVanTerritory.DeleteVanTerritoryAssignment(MultipleMapIDs, Error_No, Error_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
