Imports System.Data.SqlClient
Imports SalesWorx.BO.DAL
Public Class Price
    Dim ObjDALPrice As New DAL_Price
    Function GetPriceList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal UD_SUB_QRY As String) As DataSet
        Try
            Return ObjDALPrice.GetPriceList(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
     Function GetDefaultPriceList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal DefaultPriceListID As String) As DataSet
        Try
            Return ObjDALPrice.GetDefaultPriceList(Err_No, Err_Desc, DefaultPriceListID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
 Function GetPriceListHeader(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal UD_SUB_QRY As String) As DataSet
        Try
            Return ObjDALPrice.GetPriceListHeader(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
