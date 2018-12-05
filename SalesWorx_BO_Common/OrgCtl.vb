Imports System.Data.SqlClient
Imports SalesWorx.BO.DAL
Public Class OrgCtl

    Dim ObjDALOrgCTL As New DAL_OrgCtl
    Public Function GetOrgCTL(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDALOrgCTL.GetOrgCTL(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function InsertOrgCTL(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_des As String, ByVal Currency As String) As Boolean
        Try
            Return ObjDALOrgCTL.InsertOrgCtl(Err_No, Err_Desc, Org_des, Currency)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function UpdateOrgCTL(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_Id As Integer, ByVal Org_des As String, ByVal Currency As String) As Boolean
        Try
            Return ObjDALOrgCTL.UpdateOrgCtl(Err_No, Err_Desc, Org_Id, Org_des, Currency)
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Function DeleteOrgCtl(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_Id As Integer) As Boolean
        Try
            Return ObjDALOrgCTL.DeleteOrgCtl(Err_No, Err_Desc, Org_Id)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function FillCurrency(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDALOrgCTL.FillCurrencyGrid(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ValidateDescription(ByVal Org_Id As Integer, ByVal Org_des As String) As Boolean
        Try
            Return ObjDALOrgCTL.ValidateDescription(Org_Id, Org_des)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

   

    Public Function ValidateDescription(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_Id As Integer) As Boolean
        Try
            Return ObjDALOrgCTL.DeleteValidation(Err_No, Err_Desc, Org_Id)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetSearchResultOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Criteria As String) As DataTable
        Try
            Return ObjDALOrgCTL.GetSearchResultOrg(Err_No, Err_Desc, OrgID, Criteria)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
