Imports System.Data.SqlClient
Imports SalesWorx.BO.DAL
Public Class Collection
    Dim ObjDALCollection As New DAL_Collection
    Function GetCollectionList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal UD_SUB_QRY As String, ByVal OrgID As String) As DataSet
        Try
            Return ObjDALCollection.GetCollectionList(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCollectionDetailList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal UD_SUB_QRY As String) As DataSet
        Try
            Return ObjDALCollection.GetCollectionDetailList(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function ReleaseCollectionList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CollectionID As String, ByVal ReleasedBy As Integer) As Boolean
        Try
            Return ObjDALCollection.ReleaseCollectionList(Err_No, Err_Desc, CollectionID, ReleasedBy)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetHeldPDC(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal UD_SUB_QRY As String, ByVal OrgID As String) As DataSet
        Try
            Return ObjDALCollection.GetHeldPDC(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
