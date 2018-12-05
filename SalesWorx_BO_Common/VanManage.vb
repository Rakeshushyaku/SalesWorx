Imports System.Data.SqlClient
Imports SalesWorx.BO.DAL
Public Class VanManage
    Dim ObjDALVanmanage As New DAL_VanManage
    
    Public Function SaveVan(ByRef Error_No As Long, ByRef Error_Desc As String, Van_Org_ID As String, Sales_Org_ID As String, Van_Name As String, Emp_Code As String, Emp_Name As String, Emp_Phone As String, Prefix_No As String) As Boolean
        Return ObjDALVanmanage.SaveVan(Error_No, Error_Desc, Van_Org_ID, Sales_Org_ID, Van_Name, Emp_Code, Emp_Name, Emp_Phone, Prefix_No)
    End Function
    Public Function UpdateVan(ByRef Error_No As Long, ByRef Error_Desc As String, Van_Org_ID As String, Sales_Org_ID As String, Van_Name As String, Emp_Code As String, Emp_Name As String, Emp_Phone As String, Prefix_No As String) As Boolean
        Return ObjDALVanmanage.UpdateVan(Error_No, Error_Desc, Van_Org_ID, Sales_Org_ID, Van_Name, Emp_Code, Emp_Name, Emp_Phone, Prefix_No)
    End Function
    Public Function DeleteVan(ByRef Error_No As Long, ByRef Error_Desc As String, Van_Org_ID As String, Sales_Org_ID As String) As Boolean
        Return ObjDALVanmanage.DeleteVan(Error_No, Error_Desc, Van_Org_ID, Sales_Org_ID)
    End Function

    Public Function CheckVancodeDuplicate(ByRef Error_No As Long, ByRef Error_Desc As String, Van_Org_ID As String) As Boolean
        Return ObjDALVanmanage.CheckVancodeDuplicate(Error_No, Error_Desc, Van_Org_ID)
    End Function

    Public Function CheckEmpcodeDuplicate(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal empcode As String, ByVal Van_org_ID As String) As Boolean
        Return ObjDALVanmanage.CheckEmpcodeDuplicate(Err_No, Err_Desc, empcode, Van_org_ID)
    End Function
    Public Function LoadExportVanTemplate() As DataSet
        Return ObjDALVanmanage.LoadExportVanTemplate()
    End Function

    Public Function SearchVanGrid(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal FilterBy As String, ByVal FilterValue As String) As DataTable
        Try
            Return ObjDALVanmanage.SearchVanGrid(Err_No, Err_Desc, FilterBy, FilterValue)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetVanDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Van_ID As String, ByVal Org_ID As String) As DataTable
        Try
            Return ObjDALVanmanage.GetVanDetails(Err_No, Err_Desc, Van_ID, Org_ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Function ValidOrgnization(ByRef Error_No As Long, ByRef Error_Desc As String, Org_ID As String) As Boolean
        Return ObjDALVanmanage.ValidOrgnization(Error_No, Error_Desc, Org_ID)
    End Function
End Class
