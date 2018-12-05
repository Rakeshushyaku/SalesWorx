Imports System.Data.SqlClient
Imports SalesWorx.BO.DAL
Public Class SalesDistrict
    Private _sDescription As String
    Private _sID As String
    Private _sCode As String
    Dim ObjSalesDistrict As New DAL_SalesDistrict
    Public Property Description() As String
        Set(ByVal value As String)
            _sDescription = value
        End Set
        Get
            Return _sDescription
        End Get
    End Property
    Public Property ID() As String
        Set(ByVal value As String)
            _sID = value
        End Set
        Get
            Return _sID
        End Get
    End Property
    Public Property Code() As String
        Set(ByVal value As String)
            _sCode = value
        End Set
        Get
            Return _sCode
        End Get
    End Property
    Public Function InsertSalesDistrict(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjSalesDistrict.InsertSalesDistrict(Err_No, Err_Desc, _sDescription, _sCode)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function UpdateSalesDistrict(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjSalesDistrict.UpdateSalesDistrict(Err_No, Err_Desc, _sID, _sDescription, _sCode)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function DeleteSalesDistrict(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjSalesDistrict.DeleteSalesDistrict(Err_No, Err_Desc, _sID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetSalesDistrict(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim success As Boolean = False
        Try

            Dim dt As New DataTable
            dt = ObjSalesDistrict.GetSalesDistrict(Err_No, Err_Desc, _sID)
            If dt.Rows.Count > 0 Then
                _sDescription = dt.Rows(0)("Description").ToString()
                success = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return success
    End Function

    Public Function SearchSalesDistrictGrid(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal FilterBy As String, ByVal FilterValue As String) As DataTable
        Try
            Return ObjSalesDistrict.SearchSalesDistrictGrid(Err_No, Err_Desc, FilterBy, FilterValue)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Validatecode(ByVal Sales_District_Code As String) As Boolean
        Dim success As Boolean = False
        Try
            success = ObjSalesDistrict.Validatecode(Sales_District_Code)

        Catch ex As Exception
            Throw ex
        End Try
        Return success
    End Function

    Public Function LoadExportSalesDistrictTemplate() As DataTable
        Try
            Return ObjSalesDistrict.LoadExportSalesDistrictTemplate()
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SaveSalesDistrict(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Description As String, ByVal Code As String) As Boolean
        Try
            Return ObjSalesDistrict.InsertSalesDistrict(Err_No, Err_Desc, Description, Code)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ValidateDescription(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Description As String) As Boolean
        Dim success As Boolean = False
        Try
            success = ObjSalesDistrict.CheckDuplicate(Err_No, Err_Desc, Description)

        Catch ex As Exception
            Throw ex
        End Try
        Return success
    End Function
End Class
