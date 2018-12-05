Imports System.Data.SqlClient
Imports SalesWorx.BO.DAL
Public Class Returns
    Dim ObjDAL_Returns As New DAL_Returns
    Function GetReturnNotes(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustomerID As String, SiteID As String, ByVal SalesRepID As String, Fromdate As String, Todate As String, RefNo As String, UserID As String) As DataTable
        Try
            Return ObjDAL_Returns.GetReturnNotes(Err_No, Err_Desc, CustomerID, SiteID, SalesRepID, Fromdate, Todate, RefNo, UserID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetReturnNoteDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RowID As String) As DataSet
        Try
            Return ObjDAL_Returns.GetReturnNoteDetails(Err_No, Err_Desc, RowID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetInvoiceToAttach(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRepID As String, CustomerID As String, ShipSiteID As String, InventoryItemID As String, maxDate As String) As DataTable
        Try
            Return ObjDAL_Returns.GetInvoiceToAttach(Err_No, Err_Desc, SalesRepID, CustomerID, ShipSiteID, InventoryItemID, maxDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ConfirmReturnNote(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RowID As String, ByVal Dt As DataTable, ByVal Confirmedby As String) As Boolean
        Try
            Return ObjDAL_Returns.ConfirmReturnNote(Err_No, Err_Desc, RowID, Dt, Confirmedby)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetConversion(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, OrgID As String, ByVal UOM As String) As String
        Try
            Return ObjDAL_Returns.GetConversion(Err_No, Err_Desc, ItemCode, OrgID, UOM)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetDeliveryNote(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RefNo As String, OrgID As String, Type As String) As DataSet
        Try
            Return ObjDAL_Returns.GetDeliveryNote(Err_No, Err_Desc, RefNo, OrgID, Type)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
