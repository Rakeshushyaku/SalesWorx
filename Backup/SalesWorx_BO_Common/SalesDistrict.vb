Imports System.Data.SqlClient
Imports SalesWorx.BO.DAL
Public Class SalesDistrict
    Private _sDescription As String
    Private _sID As String
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

    Public Function InsertSalesDistrict(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjSalesDistrict.InsertSalesDistrict(Err_No, Err_Desc, _sDescription)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function UpdateSalesDistrict(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjSalesDistrict.UpdateSalesDistrict(Err_No, Err_Desc, _sID, _sDescription)
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

    Public Function SearchSalesDistrictGrid(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal FilterValue As String) As DataTable
        Try
            Return ObjSalesDistrict.SearchSalesDistrictGrid(Err_No, Err_Desc, FilterValue)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
