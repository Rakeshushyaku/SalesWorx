Imports System.Data.SqlClient
Imports SalesWorx.BO.DAL
Public Class LatiLongitude
    Private _customerID As Integer
    Private _siteUserId As Integer
    Private _dLatitude As Double
    Private _dLongitude As Double
    Dim ObjDALLati As New DAL_LatiLongitude

    Public Property CustomerId() As Integer
        Set(ByVal value As Integer)
            _customerID = value
        End Set
        Get
            Return _customerID
        End Get
    End Property
    Public Property SiteUserId() As Integer
        Set(ByVal value As Integer)
            _siteUserId = value
        End Set
        Get
            Return _siteUserId
        End Get
    End Property
    Public Property Longitude() As Double
        Set(ByVal value As Double)
            _dLongitude = value
        End Set
        Get
            Return _dLongitude
        End Get
    End Property
    Public Property Latitude() As Double
        Set(ByVal value As Double)
            _dLatitude = value
        End Set
        Get
            Return _dLatitude
        End Get
    End Property
    Public Function FillCusShipAddress(ByRef Err_No As Long, ByRef Err_Desc As String, Org As String) As DataTable
        Try
            Return ObjDALLati.FillCusShipAddress(Err_No, Err_Desc, Org)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function FillCusShipAddress(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDALLati.FillCusShipAddress(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SearchLatiLongitude(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal FilterBy As String, ByVal FilterValue As String, Org As String) As DataTable
        Try
            Return ObjDALLati.SearchLatiLongitude(Err_No, Err_Desc, FilterBy, FilterValue, Org)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function UpdateLatiLongitude(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDALLati.UpdateLatiLongitude(Err_No, Err_Desc, _dLatitude, _dLongitude, _customerID, _siteUserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SaveData(ByVal DataTbl As DataTable, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDALLati.SaveData(DataTbl, Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetLastVisit(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustID As String, ByVal SiteID As String) As DataTable
        Try
            Return ObjDALLati.GetLastVisit(Err_No, Err_Desc, CustID, SiteID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetGEO_loc_mod(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDALLati.GetGEO_loc_mod(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetExpGEOLocation(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustID As String, ByVal SiteID As String) As DataTable
        Try
            Return ObjDALLati.GetExpGEOLocation(Err_No, Err_Desc, CustID, SiteID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
