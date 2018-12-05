Imports System.Data.SqlClient
Imports SalesWorx.BO.DAL

Public Class AssetType

    Private _sAssetTypeId As String
    Private _sDescription As String
    Private _sParam1 As String
    Private _sParam2 As String
    Private _sParam3 As String
    Private _sParam4 As String
    Private _sParam5 As String
    Dim ObjDAlAssetType As New DAL_AssetType

    Public Property AssetTypeId() As String
        Set(ByVal value As String)
            _sAssetTypeId = value
        End Set
        Get
            Return _sAssetTypeId
        End Get
    End Property

    Public Property AssetType() As String
        Set(ByVal value As String)
            _sDescription = value
        End Set
        Get
            Return _sDescription
        End Get
    End Property


    Public Property Param1() As String
        Set(ByVal value As String)
            _sParam1 = value
        End Set
        Get
            Return _sParam1
        End Get
    End Property

    Public Property Param2() As String
        Set(ByVal value As String)
            _sParam2 = value
        End Set
        Get
            Return _sParam2
        End Get
    End Property

    Public Property Param3() As String
        Set(ByVal value As String)
            _sParam3 = value
        End Set
        Get
            Return _sParam3
        End Get
    End Property

    Public Property Param4() As String
        Set(ByVal value As String)
            _sParam4 = value
        End Set
        Get
            Return _sParam4
        End Get
    End Property

    Public Property Param5() As String
        Set(ByVal value As String)
            _sParam5 = value
        End Set
        Get
            Return _sParam5
        End Get
    End Property









    Public Function RebuildAll(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDAlAssetType.DeleteAll(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function InsertAssetType(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal userId As Integer) As Boolean
        Try
            Return ObjDAlAssetType.InsertAssetType(Err_No, Err_Desc, _sDescription, userId, _sParam1, _sParam2, _sParam3, CInt(_sParam4), CDec(_sParam5))
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function InsertAssets(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal userId As Integer, ByVal ASsetType As Integer, ByVal CustID As Integer, ByVal SiteID As Integer, ByVal AssetCode As String, ByVal Description As String, ByVal IsActive As String, AssetValue As Decimal) As Boolean
        Try
            Return ObjDAlAssetType.InsertAssets(Err_No, Err_Desc, userId, ASsetType, CustID, SiteID, AssetCode, Description, IsActive, AssetValue)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetCustomerandSiteID(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustNo As String, ByVal OrgID As String) As DataRow
        Try
            Return ObjDAlAssetType.GetCustomerandsiteId(CustNo, OrgID, Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function UpdateAssets(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal userId As Integer, ByVal AssetId As String, ByVal ASsetType As Integer, ByVal CustID As Integer, ByVal SiteID As Integer, ByVal AssetCode As String, ByVal Description As String, ByVal IsActive As String, AssetValue As Decimal) As Boolean
        Try
            Return ObjDAlAssetType.UpdateAssets(Err_No, Err_Desc, AssetId, ASsetType, userId, CustID, SiteID, AssetCode, Description, IsActive, AssetValue)
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Function UpdateAssetType(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal userId As Integer) As Boolean
        Try
            Return ObjDAlAssetType.UpdateAssetType(Err_No, Err_Desc, CInt(_sAssetTypeId), _sDescription, userId, _sParam1, _sParam2, _sParam3, CInt(_sParam4), CDec(_sParam5))
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function DeleteAssetType(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDAlAssetType.DeleteAssetType(Err_No, Err_Desc, CInt(_sAssetTypeId))
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function DeleteAssets(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal AssetID As String, ByVal UserID As Integer) As Boolean
        Try
            Return ObjDAlAssetType.DeleteAssets(Err_No, Err_Desc, AssetID, UserID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function FillAssetType(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDAlAssetType.FillAssetTypeGrid(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function FillAssets(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal orgId As String) As DataTable
        Try
            Return ObjDAlAssetType.FillAssets(Err_No, Err_Desc, orgId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetAssetByID(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal AssetId As String, ByVal orgId As String) As DataTable
        Try
            Return ObjDAlAssetType.GetAssetID(Err_No, Err_Desc, AssetId, orgId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function CheckValidAssetTypeID(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal AssetTypeID As String) As Boolean
        Try
            Return ObjDAlAssetType.CheckValidAssetTypeID(Err_No, Err_Desc, AssetTypeID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckDuplicate(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal AssetTypeID As String) As Boolean
        Try
            Return ObjDAlAssetType.CheckDuplicate(Err_No, Err_Desc, _sDescription, AssetTypeID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function UploadAssetsToCustomer(ByVal dtData As DataTable, ByVal OrgID As String, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CreatedBy As Integer) As Boolean
        Try
            Return ObjDAlAssetType.UploadAssetsToCustomer(dtData, OrgID, Err_No, Err_Desc, CreatedBy)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckAssetNo(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal AssetNo As String, ByVal AssetId As String) As Boolean
        Try
            Return ObjDAlAssetType.CheckAssetNo(Err_No, Err_Desc, AssetNo, AssetId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Function CheckExist(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDAlAssetType.CheckExist(Err_No, Err_Desc, CInt(_sAssetTypeId))
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
