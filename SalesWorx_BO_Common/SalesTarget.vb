Imports System.Data.SqlClient
Imports SalesWorx.BO.DAL
Public Class SalesTarget
Dim ObjDALSalesTarget As New DAL_SalesTarget
    Function GetTargetDefinition(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRepID As String, ByVal Year As String, ByVal Month As String) As DataTable
        Try
            Return ObjDALSalesTarget.GetTargetDefinition(Err_No, Err_Desc, SalesRepID, Year, Month)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SaveSalesTarget(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserID As String, ByVal SalesRepID As String, ByVal Year As String, ByVal Month As String, ByVal Value_type As String, ByVal SalesTargetDt As DataTable) As Boolean
         Try
            Return ObjDALSalesTarget.SaveSalesTarget(Err_No, Err_Desc, UserID, SalesRepID, Year, Month, Value_type, SalesTargetDt)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetTargetYear(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDALSalesTarget.GetTargetYear(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function CheckAgencyOrCategory(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Mode As String, ByVal ClassValue As String, ByVal SalesRepID As String) As Boolean
        Try
            Return ObjDALSalesTarget.CheckAgencyOrCategory(Err_No, Err_Desc, Mode, ClassValue, SalesRepID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckValidCustomer(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustNo As String, ByVal SalesRepNo As String, PrimaryClass As String) As Boolean
        Try
            Return ObjDALSalesTarget.CheckValidCustomer(Err_No, Err_Desc, CustNo, SalesRepNo, PrimaryClass)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ExportFSRTargetTemplate(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal OrgID As String, ByVal SID As Integer, ByVal TargetMonth As Date) As DataTable
        Try
            Return ObjDALSalesTarget.ExportFSRTargetTemplate(Error_No, Error_Desc, OrgID, SID, TargetMonth)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetTargetDefinitionforExport(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, ByVal SalesRepID As String, ByVal Year As String, ByVal Month As String) As DataSet
        Try
            Return ObjDALSalesTarget.GetTargetDefinitionforExport(Err_No, Err_Desc, OrgId, SalesRepID, Year, Month)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckValidFSRID(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRepNo As String, ByVal OrgID As String) As Boolean
         Try
            Return ObjDALSalesTarget.CheckValidFSRID(Err_No, Err_Desc, SalesRepNo, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function UploadFSRSalesTarget(ByVal dtData As DataTable, ByVal dtDeletedFSR As DataTable, ByVal TargetType As String, ByVal ValueType As String, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CreatedBy As Integer) As Boolean
        Try
            Return ObjDALSalesTarget.UploadFSRSalesTarget(dtData, dtDeletedFSR, TargetType, ValueType, Err_No, Err_Desc, CreatedBy)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckValidProductCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String) As Boolean
         Try
            Return ObjDALSalesTarget.CheckValidProductCode(Err_No, Err_Desc, ItemCode)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function UploadSalesTarget(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserID As String, ByVal Year As String, ByVal Month As String, ByVal SalesTargetDt As DataTable) As Boolean
         Try
            Return ObjDALSalesTarget.UploadSalesTarget(Err_No, Err_Desc, UserID, Year, Month, SalesTargetDt)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
