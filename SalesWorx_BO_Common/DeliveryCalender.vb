
Imports System.Data.SqlClient
Imports SalesWorx.BO.DAL
Public Class DeliveryCalender
    Dim ObjDAL_DlvClndr As New DAL_DeliveryCalender
    Public Function GetDeliveryCalender(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal OPT As String) As DataTable
        Try
            Return ObjDAL_DlvClndr.GetDeliveryCalender(Err_No, Err_Desc,OrgID ,OPT)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SerachDeliveryCalender(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal FromDate As String, ByVal ToDate As String, ByVal Is_Working As String) As DataTable
        Try
            Return ObjDAL_DlvClndr.SerchDeliveryCalender(Err_No, Err_Desc, OrgID, FromDate, ToDate, Is_Working)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckDC_ExDateExist(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Organization_ID As String, ByVal Delivery_Date As String) As DataTable
        Try
            Return ObjDAL_DlvClndr.CheckDC_ExDateExist(Err_No, Err_Desc, Organization_ID, Delivery_Date)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ManageDeliveryCalenderDate(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Row_ID As String, ByVal Organization_ID As String, ByVal DeliveryDate As String, ByVal Is_Working As String, ByVal UserID As Integer) As Boolean
        Try
            Return ObjDAL_DlvClndr.ManageDeliveryCalenderDate(Err_No, Err_Desc, Row_ID, Organization_ID, DeliveryDate, Is_Working, UserID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetDeliveryCalender_Details(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Row_ID As String) As DataTable
        Try
            Return ObjDAL_DlvClndr.GetDeliveryCalender_Details(Err_No, Err_Desc, OrgID, Row_ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function DeleteDeliveryCalenderDate(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Row_ID As String, ByVal Organization_ID As String) As Boolean
        Try
            Return ObjDAL_DlvClndr.DeleteDeliveryCalenderDate(Err_No, Err_Desc, Row_ID, Organization_ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ManageDeliveryCalenderDays(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Organization_ID As String, ByVal Daylst As String, ByVal UserID As Integer) As Boolean
        Try
            Return ObjDAL_DlvClndr.ManageDeliveryCalenderDays(Err_No, Err_Desc, Organization_ID, Daylst, UserID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function IsValidOrganization(ByVal OrgID As String) As Boolean
        Return ObjDAL_DlvClndr.IsValidOrganization(OrgID)
    End Function




    Public Function ExistsDeliveryCalenderDate(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Organization_ID As String, ByVal DeliveryDate As String) As DataTable
        Try
            Return ObjDAL_DlvClndr.ExistsDeliveryCalenderDate(Err_No, Err_Desc, Organization_ID, DeliveryDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
