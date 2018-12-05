Imports System.Data.SqlClient
Imports SalesWorx.BO.DAL
Public Class Incentive
    Dim ObjDAlIncentive As New DAL_Incentive
    Public Function GetIncentiveParameters(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String) As DataTable
        Try
            Return ObjDAlIncentive.GetIncentiveParameters(Err_No, Err_Desc, Org_ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetOrganisation(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UD_SUB_QRY As String) As DataTable
        Try
            Return ObjDAlIncentive.GetOrganisation(Err_No, Err_Desc, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetPayoutSlabs(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String) As DataTable
        Try
            Return ObjDAlIncentive.GetPayoutSlabs(Err_No, Err_Desc, Org_ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ManageIncentive_Slabs(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Row_ID As String, ByVal Organization_ID As String, ByVal Parameter_Code As String, ByVal From_Percentage As Decimal, ByVal To_Percentage As Decimal, ByVal Payout_Percentage As Decimal, ByVal Mode As String, ByVal UserID As Integer) As Boolean
        Try
            Return ObjDAlIncentive.ManageIncentive_Slabs(Err_No, Err_Desc, Row_ID, Organization_ID, Parameter_Code, From_Percentage, To_Percentage, Payout_Percentage, Mode, UserID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ManageIncentive(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Row_ID As String, ByVal Organization_ID As String, ByVal Parameter_Code As String, ByVal Weightage As Decimal, ByVal Mode As String, ByVal UserID As Integer) As Boolean
        Try
            Return ObjDAlIncentive.ManageIncentive(Err_No, Err_Desc, Row_ID, Organization_ID, Parameter_Code, Weightage, Mode, UserID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function UpdateIncentiveActive(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Row_ID As String, ByVal Active As String, ByVal UserID As Integer) As Boolean
        Try
            Return ObjDAlIncentive.UpdateIncentiveActive(Err_No, Err_Desc, Row_ID, Active, UserID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function IsValidslabPercentage(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String, ByVal ROW_ID As String, ByVal Parameter_Code As String, ByVal From_Percentage As Decimal, ByVal To_Percentage As Decimal) As Boolean
        Try
            Return ObjDAlIncentive.IsValidslabPercentage(Err_No, Err_Desc, Org_ID, ROW_ID, Parameter_Code, From_Percentage, To_Percentage)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function UpdateIncentiveActive_Slab(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Row_ID As String, ByVal Active As String, ByVal UserID As Integer) As Boolean
        Try
            Return ObjDAlIncentive.UpdateIncentiveActive_Slab(Err_No, Err_Desc, Row_ID, Active, UserID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SearchSlab(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal FilterBy As String, ByVal FilterValue As String, ByVal Org_ID As String) As DataTable
        Try
            Return ObjDAlIncentive.SearchSlab(Err_No, Err_Desc, FilterBy, FilterValue, Org_ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetIncentiveTarget(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String, ByVal Incentive_Month As Integer, ByVal Incentive_Year As Integer, ByVal FilterBy As String, ByVal FilterValue As String) As DataTable
        Try
            Return ObjDAlIncentive.GetIncentiveTarget(Err_No, Err_Desc, Org_ID, Incentive_Month, Incentive_Year, FilterBy, FilterValue)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetEmpCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String) As DataTable
        Try
            Dim dt As New DataTable
            dt = ObjDAlIncentive.GetEmpCode(Err_No, Err_Desc, Org_ID)
            Dim dr As DataRow
            dr = dt.NewRow()
            dr(0) = "0"
            dr(1) = "All"
            dt.Rows.InsertAt(dr, 0)

            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ManageIncentive_Target(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Row_ID As String, ByVal Organization_ID As String, ByVal Emp_Code As String, ByVal Parameter_Code As String, ByVal Target As Decimal, ByVal Incentive_Month As Integer, ByVal Incentive_Year As Integer, ByVal Mode As String, ByVal UserID As Integer) As Boolean
        Try
            Return ObjDAlIncentive.ManageIncentive_Target(Err_No, Err_Desc, Row_ID, Organization_ID, Emp_Code, Parameter_Code, Target, Incentive_Month, Incentive_Year, Mode, UserID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function CheckIncentive_TargetExist(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Organization_ID As String, ByVal Emp_Code As String, ByVal Parameter_Code As String, ByVal Incentive_Month As Integer, ByVal Incentive_Year As Integer) As Boolean
        Try
            Return ObjDAlIncentive.CheckIncentive_TargetExist(Err_No, Err_Desc, Organization_ID, Emp_Code, Parameter_Code, Incentive_Month, Incentive_Year)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function UpdateIncentiveActive_Target(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Row_ID As String, ByVal Active As String, ByVal UserID As Integer) As Boolean
        Try
            Return ObjDAlIncentive.UpdateIncentiveActive_Target(Err_No, Err_Desc, Row_ID, Active, UserID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function IsValidEmpCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal empcode As String) As Boolean
        Return ObjDAlIncentive.IsValidEmpCode(Err_No, Err_Desc, OrgID, empcode)
    End Function

    Public Function IsParameter_Code(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String, ByVal Parameter_Code As String) As Boolean
        Return ObjDAlIncentive.IsParameter_Code(Err_No, Err_Desc, Org_ID, Parameter_Code)
    End Function
    Public Function CheckIncentiveTargetExist(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Organization_ID As String, ByVal Emp_Code As String, ByVal Parameter_Code As String, ByVal Incentive_Month As Integer, ByVal Incentive_Year As Integer) As DataTable
        Try
            Return ObjDAlIncentive.CheckIncentiveTargetExist(Err_No, Err_Desc, Organization_ID, Emp_Code, Parameter_Code, Incentive_Month, Incentive_Year)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function LoadExportIncentiveTarget(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String, ByVal Incentive_Month As Integer, ByVal Incentive_Year As Integer, ByVal FilterBy As String, ByVal FilterValue As String) As DataTable
        Try
            Return ObjDAlIncentive.LoadExportIncentiveTarget(Err_No, Err_Desc, Org_ID, Incentive_Month, Incentive_Year, FilterBy, FilterValue)

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetIncentiveCommission(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String, ByVal Incentive_Month As Integer, ByVal Incentive_Year As Integer, ByVal FilterBy As String, ByVal FilterValue As String) As DataTable
        Try
            Return ObjDAlIncentive.GetIncentiveCommission(Err_No, Err_Desc, Org_ID, Incentive_Month, Incentive_Year, FilterBy, FilterValue)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetIncentiveStatement(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String, EmpCode As String) As DataTable
        Try
            Return ObjDAlIncentive.GetIncentiveStatement(Err_No, Err_Desc, Org_ID, EmpCode)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetIncentiveGenerated(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String, Month As String, Year As String) As DataTable
        Try
            Return ObjDAlIncentive.GetIncentiveGenerated(Err_No, Err_Desc, Org_ID, Month, Year)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetUsersNotFullSynced(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String, Month As String, Year As String) As DataTable
        Try
            Return ObjDAlIncentive.GetUsersNotFullSynced(Err_No, Err_Desc, Org_ID, Month, Year)

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GenerateIncentive(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String) As Boolean
        Try
            Return ObjDAlIncentive.GenerateIncentive(Err_No, Err_Desc, Org_ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function CheckIncentive_CommissionExist(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Organization_ID As String, ByVal Classification As String, ByVal Incentive_Month As Integer, ByVal Incentive_Year As Integer) As DataTable
        Try
            Return ObjDAlIncentive.CheckIncentive_CommissionExist(Err_No, Err_Desc, Organization_ID, Classification, Incentive_Month, Incentive_Year)
        Catch ex As Exception
            Throw ex
        End Try
    End Function



    Public Function GetClassification(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String, ByVal txt As String) As DataTable
        Try
            Return ObjDAlIncentive.GetClassification(Err_No, Err_Desc, Org_ID, txt)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ManageIncentive_Commission(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Row_ID As String, ByVal Organization_ID As String, ByVal Classification As String, ByVal Commission As Decimal, ByVal UOM As String, ByVal Incentive_Month As Integer, ByVal Incentive_Year As Integer, ByVal UserID As Integer) As Boolean
        Try
            Return ObjDAlIncentive.ManageIncentive_Commission(Err_No, Err_Desc, Row_ID, Organization_ID, Classification, Commission, UOM, Incentive_Month, Incentive_Year, UserID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Function GetUOM(Err_No As Long, Err_Desc As String, OrgId As String, Text As String) As DataTable
        Try
            Return ObjDAlIncentive.GetUOM(Err_No, Err_Desc, OrgId, Text)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function UpdateIncentiveActive_Commission(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Row_ID As String, ByVal Active As String, ByVal UserID As Integer) As Boolean
        Try
            Return ObjDAlIncentive.UpdateIncentiveActive_Commission(Err_No, Err_Desc, Row_ID, Active, UserID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function IsValidClassification(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Organization_ID As String, ByVal Classification As String) As Boolean
        Try
            Return ObjDAlIncentive.IsValidClassification(Err_No, Err_Desc, Organization_ID, Classification)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function IsValidUOM(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Organization_ID As String, ByVal Classification As String, ByVal uom As String) As Boolean
        Try
            Return ObjDAlIncentive.IsValidUOM(Err_No, Err_Desc, Organization_ID, Classification, uom)
        Catch ex As Exception
            Throw ex
        End Try
    End Function



    Public Function LoadExportIncentiveCommission(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String, ByVal Incentive_Month As Integer, ByVal Incentive_Year As Integer, ByVal FilterBy As String, ByVal FilterValue As String) As DataTable
        Try
            Return ObjDAlIncentive.LoadExportIncentiveCommission(Err_No, Err_Desc, Org_ID, Incentive_Month, Incentive_Year, FilterBy, FilterValue)

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function PayIncentive(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal EmpCode As String, ByVal Organization_ID As String, ByVal Amount As String) As Boolean
        Try
            Return ObjDAlIncentive.PayIncentive(Err_No, Err_Desc, EmpCode, Organization_ID, Amount)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetTotalWeightage(ByRef Err_No As Long, ByRef Err_Desc As String, OrgId As String) As DataTable
        Try
            Return ObjDAlIncentive.GetTotalWeightage(Err_No, Err_Desc, OrgId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetIncentiveMPYear(ByRef Err_No As Long, ByRef Err_Desc As String) As Integer
        Try
            Return ObjDAlIncentive.GetIncentiveMPYear(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetEmp(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String) As DataTable
        Try
            Dim dt As New DataTable
            dt = ObjDAlIncentive.GetEmpCode(Err_No, Err_Desc, Org_ID)
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
