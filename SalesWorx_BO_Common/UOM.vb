Imports System.Data.SqlClient
Imports SalesWorx.BO.DAL
Public Class UOM
    Private _dItem_UOM_ID As Integer
    Private _sItem_Code As String
    Private _sOrganization_ID As String
    Private _sItem_UOM As String
    Private _dConversion As Decimal
    Private _sSellableUom As String
    Dim ObjDALProduct As New DAL_Product
    Public Property Item_UOM_ID() As Integer
        Set(ByVal value As Integer)
            _dItem_UOM_ID = value
        End Set
        Get
            Return _dItem_UOM_ID
        End Get
    End Property
    Public Property SellableUom() As String
        Set(ByVal value As String)
            _sSellableUom = value
        End Set
        Get
            Return _sSellableUom
        End Get
    End Property
    Public Property Item_Code() As String
        Set(ByVal value As String)
            _sItem_Code = value
        End Set
        Get
            Return _sItem_Code
        End Get
    End Property
    Public Property Organization_ID() As String
        Set(ByVal value As String)
            _sOrganization_ID = value
        End Set
        Get
            Return _sOrganization_ID
        End Get
    End Property
    Public Property Item_UOM() As String
        Set(ByVal value As String)
            _sItem_UOM = value
        End Set
        Get
            Return _sItem_UOM
        End Get
    End Property

    Public Property Conversion() As Decimal
        Set(ByVal value As Decimal)
            _dConversion = value
        End Set
        Get
            Return _dConversion
        End Get
    End Property
    Public Function FillItemUOMGrid(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDALProduct.FillItemUOMGrid(Err_No, Err_Desc, _sItem_Code, _sOrganization_ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function FillItemUOMGrid(ByRef Err_No As Long, ByRef Err_Desc As String, Item_Code As String, Organization_ID As String) As DataTable
        Try
            Return ObjDALProduct.FillItemUOMGrid(Err_No, Err_Desc, Item_Code, Organization_ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function InsertItemUOM(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDALProduct.InsertItemUOM(Err_No, Err_Desc, _dItem_UOM_ID, _sItem_Code, _sOrganization_ID, _sItem_UOM, _dConversion, _sSellableUom)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function UpdateItemUOM(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDALProduct.UpdateItemUOM(Err_No, Err_Desc, _dItem_UOM_ID, _sItem_Code, _sOrganization_ID, _sItem_UOM, _dConversion, _sSellableUom)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function DeleteItemUOM(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDALProduct.DeleteItemUOM(Err_No, Err_Desc, _sItem_UOM, _sItem_Code, _sOrganization_ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SaveItemUOM(ByRef Err_No As Long, ByRef Err_Desc As String, ByRef Item_Code As String, ByRef Organization_ID As String, ByRef Item_UOM As String, ByRef Conversion As Decimal, Is_Sellable As String) As Boolean
        Try
            Return ObjDALProduct.InsertItemUOM(Err_No, Err_Desc, 0, Item_Code, Organization_ID, Item_UOM, Conversion, Is_Sellable)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ValidatePrimaryUOM(ByVal Item_Code As String, ByVal Org_ID As String, ByVal UOM As String) As Boolean
        Return ObjDALProduct.ValidatePrimaryUOM(Item_Code, Org_ID, UOM)



    End Function

    Public Function SearchUOM(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Item_Code As String, ByVal Org_ID As String, ByVal FilterBy As String, ByVal FilterValue As String) As DataTable
        Try
            Return ObjDALProduct.SearchUOMGrid(Err_No, Err_Desc, Item_Code, Org_ID, FilterBy, FilterValue)

        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
