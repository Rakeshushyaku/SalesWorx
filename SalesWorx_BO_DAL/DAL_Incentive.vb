Imports System.Data.SqlClient
Imports System.Configuration
Public Class DAL_Incentive
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Private _objDB As DatabaseConnection
    Private Shared _strSQLConn As String = ConfigurationSettings.AppSettings("SQLConnString")


    Public Function GetIncentiveParameters(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtIncentive As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_GetIncentiveParameters", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Org_ID", Org_ID)
            objSQLDA.Fill(dtIncentive)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "90123"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetIncentiveParameters = dtIncentive
    End Function
    Public Function GetOrganisation(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QueryStr As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try

            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT DISTINCT MAS_Org_ID,Description FROM TBL_Org_CTL_DTL WHERE SalesRep_ID IN ({0}) ORDER BY MAS_Org_ID DESC", QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")

            GetOrganisation = MsgDs.Tables(0)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "90124"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function

    Public Function GetPayoutSlabs(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtIncentive As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_GetPayoutSlabs", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Org_ID", Org_ID)
            objSQLDA.Fill(dtIncentive)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "90125"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetPayoutSlabs = dtIncentive
    End Function

    'Public Function SearchCurrencyGrid(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal FilterBy As String, ByVal FilterValue As String) As DataTable
    '    Dim objSQLConn As SqlConnection
    '    Dim objSQLDA As SqlDataAdapter
    '    Dim dtIncentive As New DataTable
    '    Dim Query As String = ""
    '    Try
    '        If FilterBy = "Currency Code" Then
    '            Query = "SELECT * FROM TBL_Currency WHERE Currency_Code='" + FilterValue.ToUpper() + "' ORDER BY Currency_Code"
    '        ElseIf FilterBy = "Description" Then
    '            Query = "SELECT * FROM TBL_Currency WHERE Description LIKE '%" + FilterValue + "%' ORDER BY Currency_Code"
    '        Else
    '            Query = "SELECT * FROM TBL_Currency ORDER BY Currency_Code"
    '        End If
    '        objSQLConn = _objDB.GetSQLConnection
    '        objSQLDA = New SqlDataAdapter(Query, objSQLConn)
    '        objSQLDA.Fill(dtCurrency)
    '        objSQLDA.Dispose()
    '    Catch ex As Exception
    '        Err_No = "740022"
    '        Err_Desc = ex.Message
    '        Throw ex
    '    Finally
    '        _objDB.CloseSQLConnection(objSQLConn)
    '    End Try
    '    Return dtCurrency
    'End Function

    Public Function ManageIncentive_Slabs(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Row_ID As String, ByVal Organization_ID As String, ByVal Parameter_Code As String, ByVal From_Percentage As Decimal, ByVal To_Percentage As Decimal, ByVal Payout_Percentage As Decimal, ByVal Mode As String, ByVal UserID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_ManageIncentive_Slabs", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@Row_ID", Row_ID)
            objSQLCmd.Parameters.AddWithValue("@Organization_ID", Organization_ID)
            objSQLCmd.Parameters.AddWithValue("@Parameter_Code", Parameter_Code)
            objSQLCmd.Parameters.AddWithValue("@From_Percentage", From_Percentage)
            objSQLCmd.Parameters.AddWithValue("@To_Percentage", To_Percentage)
            objSQLCmd.Parameters.AddWithValue("@Payout_Percentage", Payout_Percentage)
            objSQLCmd.Parameters.AddWithValue("@Mode", Mode)
            objSQLCmd.Parameters.AddWithValue("@UserID", UserID)
            objSQLCmd.ExecuteNonQuery()
            bRetVal = True
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        Catch ex As Exception
            Err_No = "90126"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function


    Public Function ManageIncentive(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Row_ID As String, ByVal Organization_ID As String, ByVal Parameter_Code As String, ByVal Weightage As Decimal, ByVal Mode As String, ByVal UserID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_ManageIncentive", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@Row_ID", Row_ID)
            objSQLCmd.Parameters.AddWithValue("@Organization_ID", Organization_ID)
            objSQLCmd.Parameters.AddWithValue("@Parameter_Code", Parameter_Code)
            objSQLCmd.Parameters.AddWithValue("@Weightage", Weightage)
            objSQLCmd.Parameters.AddWithValue("@Mode", Mode)
            objSQLCmd.Parameters.AddWithValue("@UserID", UserID)
            objSQLCmd.ExecuteNonQuery()
            bRetVal = True
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        Catch ex As Exception
            Err_No = "90127"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function


    Public Function UpdateIncentiveActive(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal Row_ID As String, ByVal Active As String, ByVal UserID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim retVal As Boolean = False


        Try
            'objSQLConn = _objDB.GetSQLConnection
            'sQry = "UPDATE TBL_Incentive_Weightage SET Is_Active='N' WHERE Row_ID=@Row_ID"
            'objSQLCmd = New SqlCommand(sQry, objSQLConn)
            'objSQLCmd.CommandType = CommandType.Text
            'objSQLCmd.Parameters.AddWithValue("@Row_ID", Row_ID)
            'objSQLCmd.ExecuteNonQuery()
            'objSQLCmd.Dispose()

            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_UpdateIncentiveActive", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@Row_ID", Row_ID)
            objSQLCmd.Parameters.AddWithValue("@Active", Active)
            objSQLCmd.Parameters.AddWithValue("@UserID", UserID)
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
            retVal = True
        Catch ex As Exception

            Error_No = 90128
            Error_Desc = String.Format("Error while Updating Incentive  Weightage: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return retVal
    End Function

    Public Function IsValidslabPercentage(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String, ByVal ROW_ID As String, ByVal Parameter_Code As String, ByVal From_Percentage As Decimal, ByVal To_Percentage As Decimal) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtIncentive As New DataTable
        Dim retVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("IsValidslabPercentage", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Org_ID", Org_ID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ROW_ID", ROW_ID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Parameter_Code", Parameter_Code)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@From_Percentage", From_Percentage)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@To_Percentage", To_Percentage)
            objSQLDA.Fill(dtIncentive)
            objSQLDA.Dispose()
            If dtIncentive.Rows.Count > 0 Then
                If Convert.ToInt32(dtIncentive.Rows(0)(0)) = 0 Then
                    retVal = True
                End If
            End If
        Catch ex As Exception
            Err_No = "90125"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        Return retVal
    End Function

    Public Function UpdateIncentiveActive_Slab(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal Row_ID As String, ByVal Active As String, ByVal UserID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim retVal As Boolean = False


        Try

            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_UpdateIncentiveActive_Slab", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@Row_ID", Row_ID)
            objSQLCmd.Parameters.AddWithValue("@Active", Active)
            objSQLCmd.Parameters.AddWithValue("@UserID", UserID)
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
            retVal = True
        Catch ex As Exception

            Error_No = 90128
            Error_Desc = String.Format("Error while Updating Incentive  Weightage: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return retVal
    End Function

    Public Function SearchSlab(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal FilterBy As String, ByVal FilterValue As String, ByVal Org_ID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtIncentive As New DataTable
        Dim Query As String = ""
        Try
            If FilterBy = "Parameter Code" Then
                Query = "SELECT S.*,C.Code_Description,case  ISNULL(Is_Active,'N') when 'Y' then 'Disable' else 'Enable' end as Status  FROM TBL_Incentive_Payout_Slabs S  INNER JOIN  TBL_App_Codes C ON S.Parameter_Code =C.Code_Value WHERE Is_Active='Y' And  Organization_ID='" + Org_ID + "' And (Parameter_Code LIKE  '%" + FilterValue.ToUpper() + "%' or C.Code_Description Like '%" + FilterValue.ToUpper() + "%' ) ORDER BY Parameter_Code"
            ElseIf FilterBy = "From Percentage" Then
                Query = "SELECT S.*,C.Code_Description,case  ISNULL(Is_Active,'N') when 'Y' then 'Disable' else 'Enable' end as Status  FROM TBL_Incentive_Payout_Slabs S  INNER JOIN  TBL_App_Codes C ON S.Parameter_Code =C.Code_Value WHERE Is_Active='Y' And Organization_ID='" + Org_ID + "' And From_Percentage LIKE '%" + FilterValue + "%' ORDER BY Parameter_Code"
            ElseIf FilterBy = "To Percentage" Then
                Query = "SELECT S.*,C.Code_Description,case  ISNULL(Is_Active,'N') when 'Y' then 'Disable' else 'Enable' end as Status  FROM TBL_Incentive_Payout_Slabs S  INNER JOIN  TBL_App_Codes C ON S.Parameter_Code =C.Code_Value WHERE Is_Active='Y' And Organization_ID='" + Org_ID + "' And To_Percentage LIKE '%" + FilterValue + "%' ORDER BY Parameter_Code"
            ElseIf FilterBy = "Payout Percentage" Then
                Query = "SELECT S.*,C.Code_Description,case  ISNULL(Is_Active,'N') when 'Y' then 'Disable' else 'Enable' end as Status  FROM TBL_Incentive_Payout_Slabs S  INNER JOIN  TBL_App_Codes C ON S.Parameter_Code =C.Code_Value WHERE Is_Active='Y' And Organization_ID='" + Org_ID + "' And  Payout_Percentage LIKE '%" + FilterValue + "%' ORDER BY Parameter_Code"
            Else
                Query = "SELECT S.*,C.Code_Description,case  ISNULL(Is_Active,'N') when 'Y' then 'Disable' else 'Enable' end as Status  FROM TBL_Incentive_Payout_Slabs S  INNER JOIN  TBL_App_Codes C ON S.Parameter_Code =C.Code_Value WHERE Is_Active='Y' And Organization_ID='" + Org_ID + "'   ORDER BY Parameter_Code"
            End If
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter(Query, objSQLConn)
            objSQLDA.Fill(dtIncentive)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "940022"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtIncentive
    End Function

    Public Function GetIncentiveTarget(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String, ByVal Incentive_Month As Integer, ByVal Incentive_Year As Integer, ByVal FilterBy As String, ByVal FilterValue As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtIncentive As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_GetIncentive_Target", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Org_ID", Org_ID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Incentive_Month", Incentive_Month)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Incentive_Year", Incentive_Year)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FilterBy", FilterBy)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FilterValue", FilterValue)
            objSQLDA.Fill(dtIncentive)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "90127"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetIncentiveTarget = dtIncentive
    End Function

    Public Function GetEmpCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtIncentive As New DataTable
        Dim Query As String = ""
        Try
            Query = "SELECT DISTINCT E.Emp_Code,E.Emp_Name   FROM TBL_Emp_Info E inner join  TBL_Van_Info V on E.emp_code= V.emp_code INNER JOIN   TBL_Org_CTL_DTL O  on O.MAS_Org_ID=V.Sales_Org_ID WHERE  V.Sales_Org_ID='" + Org_ID + "'   ORDER BY Emp_Name"
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter(Query, objSQLConn)
            objSQLDA.Fill(dtIncentive)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "940022"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtIncentive
    End Function

    Public Function ManageIncentive_Target(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Row_ID As String, ByVal Organization_ID As String, ByVal Emp_Code As String, ByVal Parameter_Code As String, ByVal Target As Decimal, ByVal Incentive_Month As Integer, ByVal Incentive_Year As Integer, ByVal Mode As String, ByVal UserID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_ManageIncentive_Target", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@Row_ID", Row_ID)
            objSQLCmd.Parameters.AddWithValue("@Organization_ID", Organization_ID)
            objSQLCmd.Parameters.AddWithValue("@Emp_Code", Emp_Code)
            objSQLCmd.Parameters.AddWithValue("@Parameter_Code", Parameter_Code)
            objSQLCmd.Parameters.AddWithValue("@Target", Target)
            objSQLCmd.Parameters.AddWithValue("@Incentive_Month", Incentive_Month)
            objSQLCmd.Parameters.AddWithValue("@Incentive_Year", Incentive_Year)
            objSQLCmd.Parameters.AddWithValue("@Mode", Mode)
            objSQLCmd.Parameters.AddWithValue("@UserID", UserID)
            objSQLCmd.ExecuteNonQuery()
            bRetVal = True
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        Catch ex As Exception
            Err_No = "90127"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function


    Public Function CheckIncentive_TargetExist(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Organization_ID As String, ByVal Emp_Code As String, ByVal Parameter_Code As String, ByVal Incentive_Month As Integer, ByVal Incentive_Year As Integer) As Boolean

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtIncentive As New DataTable
        Dim Query As String = ""
        Dim bRetVal As Boolean = False
        Try
            Query = "SELECT * FROM TBL_Incentive_Target  WHERE Is_Active='Y' and  Organization_ID='" & Organization_ID & "' AND Emp_Code ='" & Emp_Code & "' AND Parameter_Code='" & Parameter_Code & "' AND Incentive_Month='" & Incentive_Month & "' AND Incentive_Year='" & Incentive_Year & "'"
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter(Query, objSQLConn)
            objSQLDA.Fill(dtIncentive)
            If dtIncentive.Rows.Count > 0 Then
                bRetVal = True
            End If
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "940023"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function
    Public Function UpdateIncentiveActive_Target(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal Row_ID As String, ByVal Active As String, ByVal UserID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim retVal As Boolean = False


        Try

            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_UpdateIncentiveActive_Target", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@Row_ID", Row_ID)
            objSQLCmd.Parameters.AddWithValue("@Active", Active)
            objSQLCmd.Parameters.AddWithValue("@UserID", UserID)
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
            retVal = True
        Catch ex As Exception

            Error_No = 90128
            Error_Desc = String.Format("Error while Updating Incentive  Weightage: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return retVal
    End Function

    Public Function IsValidEmpCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String, ByVal empcode As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtIncentive As New DataTable
        Dim Query As String = ""
        Dim retVal As Boolean = False
        Try
            Query = "SELECT DISTINCT E.Emp_Code,E.Emp_Name   FROM TBL_Emp_Info E inner join  TBL_Van_Info V on E.emp_code= V.emp_code INNER JOIN   TBL_Org_CTL_DTL O  on O.MAS_Org_ID=V.Sales_Org_ID WHERE  V.Sales_Org_ID='" & Org_ID & "' and E.Emp_Code= '" & empcode & "'  ORDER BY Emp_Name"
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter(Query, objSQLConn)
            objSQLDA.Fill(dtIncentive)
            If dtIncentive.Rows.Count > 0 Then
                retVal = True
            End If
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "940022"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return retVal
    End Function
    Public Function IsParameter_Code(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String, ByVal Parameter_Code As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtIncentive As New DataTable
        Dim Query As String = ""
        Dim retVal As Boolean = False
        Try
            Query = " SELECT   * FROM  TBL_App_Codes C left join TBL_Incentive_Weightage I ON  C.Code_Value=I.Parameter_Code  AND I.Organization_ID='" & Org_ID & "'    WHERE C.Code_Type ='INCENTIVE_PARAM' AND C.Code_Value='" & Parameter_Code & "' "
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter(Query, objSQLConn)
            objSQLDA.Fill(dtIncentive)
            If dtIncentive.Rows.Count > 0 Then
                retVal = True
            End If
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "940022"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return retVal
    End Function
    Public Function CheckIncentiveTargetExist(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Organization_ID As String, ByVal Emp_Code As String, ByVal Parameter_Code As String, ByVal Incentive_Month As Integer, ByVal Incentive_Year As Integer) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtIncentive As New DataTable
        Dim Query As String = ""
        Dim bRetVal As Boolean = False
        Try
            Query = "SELECT * FROM TBL_Incentive_Target  WHERE Organization_ID='" & Organization_ID & "' AND Emp_Code ='" & Emp_Code & "' AND Parameter_Code='" & Parameter_Code & "' AND Incentive_Month='" & Incentive_Month & "' AND Incentive_Year='" & Incentive_Year & "'"
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter(Query, objSQLConn)
            objSQLDA.Fill(dtIncentive)

            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "940023"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtIncentive
    End Function

    Public Function LoadExportIncentiveTarget(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String, ByVal Incentive_Month As Integer, ByVal Incentive_Year As Integer, ByVal FilterBy As String, ByVal FilterValue As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtIncentive As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_LoadExportIncentive_Target", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Org_ID", Org_ID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Incentive_Month", Incentive_Month)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Incentive_Year", Incentive_Year)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FilterBy", FilterBy)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FilterValue", FilterValue)
            objSQLDA.Fill(dtIncentive)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "90127"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        LoadExportIncentiveTarget = dtIncentive
    End Function
    Public Function GetIncentiveCommission(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String, ByVal Incentive_Month As Integer, ByVal Incentive_Year As Integer, ByVal FilterBy As String, ByVal FilterValue As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtIncentive As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_GetIncentive_Commission", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Org_ID", Org_ID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Incentive_Month", Incentive_Month)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Incentive_Year", Incentive_Year)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FilterBy", FilterBy)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FilterValue", FilterValue)
            objSQLDA.Fill(dtIncentive)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "90127"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetIncentiveCommission = dtIncentive
    End Function
    Public Function GetIncentiveStatement(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String, ByVal EmpCode As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtIncentive As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_GetOutstandingIncentive", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", Org_ID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@EmpCode", EmpCode)

            objSQLDA.Fill(dtIncentive)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "90127"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetIncentiveStatement = dtIncentive
    End Function
    Public Function GetIncentiveGenerated(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String, ByVal Month As String, ByVal year As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtIncentive As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_IfIncentive_Generated", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Org_ID", Org_ID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Month", Month)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Year", year)
            objSQLDA.Fill(dtIncentive)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "90127"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetIncentiveGenerated = dtIncentive
    End Function
    Public Function GetUsersNotFullSynced(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String, ByVal Month As String, ByVal year As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtIncentive As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_GetUsersNotSynced", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Org_ID", Org_ID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Month", Month)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Year", year)
            objSQLDA.Fill(dtIncentive)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "90127"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetUsersNotFullSynced = dtIncentive
    End Function
    Public Function GenerateIncentive(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Organization_ID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_CalcOverallIncentive", objSQLConn)
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@OrgID", Organization_ID)

            objSQLCmd.ExecuteNonQuery()
            bRetVal = True
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        Catch ex As Exception
            Err_No = "90127"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function


    Public Function CheckIncentive_CommissionExist(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Organization_ID As String, ByVal Classification As String, ByVal Incentive_Month As Integer, ByVal Incentive_Year As Integer) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtIncentive As New DataTable
        Dim Query As String = ""
        Dim bRetVal As Boolean = False
        Try
            Query = "SELECT * FROM TBL_Incentive_Commission  WHERE Is_Active='Y' AND Organization_ID='" & Organization_ID & "' AND Classification ='" & Classification & "' AND Incentive_Month='" & Incentive_Month & "' AND Incentive_Year='" & Incentive_Year & "'"
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter(Query, objSQLConn)
            objSQLDA.Fill(dtIncentive)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "940023"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtIncentive
    End Function

    Public Function GetClassification(Err_No As Long, Err_Desc As String, OrgId As String, Text As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            Dim QueryString As String
            objSQLConn = _objDB.GetSQLConnection
            Dim dt_appkey As DataTable
            dt_appkey = GetIncentiveAPP_key()

            If dt_appkey.Rows.Count > 0 Then


                If dt_appkey.Rows(0)("Control_Value").ToString().Trim().ToUpper() = "A" Then
                    QueryString = "SELECT DISTINCT Agency as Classification FROM TBL_Product WHERE Organization_ID='" & OrgId & "'  AND Agency  like'%" & Text & "%'"
                ElseIf dt_appkey.Rows(0)("Control_Value").ToString().Trim().ToUpper() = "P" Then
                    QueryString = "SELECT DISTINCT Item_Code as Classification  FROM TBL_Product WHERE Organization_ID='" & OrgId & "'  AND Item_Code  like'%" & Text & "%'"
                ElseIf dt_appkey.Rows(0)("Control_Value").ToString().Trim().ToUpper() = "C" Then
                    QueryString = "SELECT DISTINCT Category as Classification  FROM TBL_Product WHERE Organization_ID='" & OrgId & "'  AND Category  like'%" & Text & "%'"
                ElseIf dt_appkey.Rows(0)("Control_Value").ToString().Trim().ToUpper() = "S" Then
                    QueryString = "SELECT DISTINCT Sub_Category as Classification FROM TBL_Product WHERE Organization_ID='" & OrgId & "'  AND Sub_Category  like'%" & Text & "%'"
                End If


            End If


            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "ClassificationTbl")
            GetClassification = MsgDs.Tables("ClassificationTbl")
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function GetIncentiveAPP_key() As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtIncentive As New DataTable
        Dim Query As String = ""
        Dim bRetVal As Boolean = False
        Try
            Query = " SELECT *   from TBL_App_Control  where Control_Key='INCENTIVE_CLASSIFICATION'"
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter(Query, objSQLConn)
            objSQLDA.Fill(dtIncentive)
            objSQLDA.Dispose()
        Catch ex As Exception
            ' Err_No = "940023"
            '  Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtIncentive
    End Function

    Public Function ManageIncentive_Commission(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Row_ID As String, ByVal Organization_ID As String, ByVal Classification As String, ByVal Commission As Decimal, ByVal UOM As String, ByVal Incentive_Month As Integer, ByVal Incentive_Year As Integer, ByVal UserID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_ManageIncentive_Commission", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@Row_ID", Row_ID)
            objSQLCmd.Parameters.AddWithValue("@Organization_ID", Organization_ID)
            objSQLCmd.Parameters.AddWithValue("@Classification", Classification)
            objSQLCmd.Parameters.AddWithValue("@Commission", Commission)
            objSQLCmd.Parameters.AddWithValue("@UOM", UOM)
            objSQLCmd.Parameters.AddWithValue("@Incentive_Month", Incentive_Month)
            objSQLCmd.Parameters.AddWithValue("@Incentive_Year", Incentive_Year)
            objSQLCmd.Parameters.AddWithValue("@UserID", UserID)
            objSQLCmd.ExecuteNonQuery()
            bRetVal = True
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        Catch ex As Exception
            Err_No = "90127"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function

    Public Function GetUOM(Err_No As Long, Err_Desc As String, OrgId As String, Text As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            Dim QueryString As String
            objSQLConn = _objDB.GetSQLConnection
            Dim dt_appkey As DataTable
            dt_appkey = GetIncentiveAPP_key()

            If dt_appkey.Rows.Count > 0 Then


                If dt_appkey.Rows(0)("Control_Value").ToString().Trim().ToUpper() = "A" Then
                    QueryString = "SELECT DISTINCT Item_UOM  FROM TBL_Item_UOM U INNER JOIN TBL_Product P  ON U.ITEM_CODE=P.Item_Code  WHERE U.Organization_ID='" & OrgId & "'  AND P.Agency ='" & Text & "'"
                ElseIf dt_appkey.Rows(0)("Control_Value").ToString().Trim().ToUpper() = "P" Then
                    QueryString = "SELECT DISTINCT Item_UOM  FROM TBL_Item_UOM U INNER JOIN TBL_Product P  ON U.ITEM_CODE=P.Item_Code  WHERE U.Organization_ID='" & OrgId & "'  AND P.Item_Code ='" & Text & "'"
                ElseIf dt_appkey.Rows(0)("Control_Value").ToString().Trim().ToUpper() = "C" Then
                    QueryString = "SELECT DISTINCT Item_UOM  FROM TBL_Item_UOM U INNER JOIN TBL_Product P  ON U.ITEM_CODE=P.Item_Code  WHERE U.Organization_ID='" & OrgId & "'  AND P.Category ='" & Text & "'"
                ElseIf dt_appkey.Rows(0)("Control_Value").ToString().Trim().ToUpper() = "S" Then
                    QueryString = "SELECT DISTINCT Item_UOM  FROM TBL_Item_UOM U INNER JOIN TBL_Product P  ON U.ITEM_CODE=P.Item_Code  WHERE U.Organization_ID='" & OrgId & "'  AND P.Sub_Category ='" & Text & "'"
                End If


            End If


            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "UOMTbl")
            GetUOM = MsgDs.Tables("UOMTbl")
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function UpdateIncentiveActive_Commission(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal Row_ID As String, ByVal Active As String, ByVal UserID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim retVal As Boolean = False
        Try

            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_UpdateIncentiveActive_Commission", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@Row_ID", Row_ID)
            objSQLCmd.Parameters.AddWithValue("@Active", Active)
            objSQLCmd.Parameters.AddWithValue("@UserID", UserID)
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
            retVal = True
        Catch ex As Exception

            Error_No = 90128
            Error_Desc = String.Format("Error while Updating Incentive  Weightage: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return retVal
    End Function
    Public Function PayIncentive(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal EmpCode As String, ByVal Organization_ID As String, ByVal Amount As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim retVal As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_PayIncentive", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@OrgID", Organization_ID)
            objSQLCmd.Parameters.AddWithValue("@EmpCode", EmpCode)
            objSQLCmd.Parameters.AddWithValue("@Amount", Val(Amount))
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
            retVal = True
        Catch ex As Exception
            Error_No = 90128
            Error_Desc = String.Format("Error while Updating Incentive  Weightage: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return retVal
    End Function
    Public Function IsValidClassification(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Organization_ID As String, ByVal Classification As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtIncentive As New DataTable
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_ValidClassification", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Org_ID", Organization_ID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Classification", Classification)

            objSQLDA.Fill(dtIncentive)

            If dtIncentive.Rows.Count > 0 Then
                If dtIncentive.Rows(0)(0) = "0" Then
                    bRetVal = True
                End If

            End If
            objSQLDA.Dispose()


        Catch ex As Exception
            Err_No = "90127"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function
    Public Function IsValidUOM(Err_No As Long, Err_Desc As String, OrgId As String, Text As String, ByVal uom As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            Dim QueryString As String
            objSQLConn = _objDB.GetSQLConnection
            Dim dt_appkey As DataTable
            dt_appkey = GetIncentiveAPP_key()

            If dt_appkey.Rows.Count > 0 Then


                If dt_appkey.Rows(0)("Control_Value").ToString().Trim().ToUpper() = "A" Then
                    QueryString = "SELECT *  FROM TBL_Item_UOM U INNER JOIN TBL_Product P  ON U.ITEM_CODE=P.Item_Code  WHERE U.Organization_ID='" & OrgId & "'  AND P.Agency ='" & Text & "' AND U.Item_UOM='" & uom & "'"
                ElseIf dt_appkey.Rows(0)("Control_Value").ToString().Trim().ToUpper() = "P" Then
                    QueryString = "SELECT *  FROM TBL_Item_UOM U INNER JOIN TBL_Product P  ON U.ITEM_CODE=P.Item_Code  WHERE U.Organization_ID='" & OrgId & "'  AND P.Item_Code ='" & Text & "' AND U.Item_UOM='" & uom & "'"
                ElseIf dt_appkey.Rows(0)("Control_Value").ToString().Trim().ToUpper() = "C" Then
                    QueryString = "SELECT *  FROM TBL_Item_UOM U INNER JOIN TBL_Product P  ON U.ITEM_CODE=P.Item_Code  WHERE U.Organization_ID='" & OrgId & "'  AND P.Category ='" & Text & "' AND U.Item_UOM='" & uom & "'"
                ElseIf dt_appkey.Rows(0)("Control_Value").ToString().Trim().ToUpper() = "S" Then
                    QueryString = "SELECT *  FROM TBL_Item_UOM U INNER JOIN TBL_Product P  ON U.ITEM_CODE=P.Item_Code  WHERE U.Organization_ID='" & OrgId & "'  AND P.Sub_Category ='" & Text & "' AND U.Item_UOM='" & uom & "'"
                End If


            End If


            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "UOMTbl")

            If MsgDs.Tables("UOMTbl").Rows.Count > 0 Then
                bRetVal = True
            End If

            IsValidUOM = bRetVal
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function LoadExportIncentiveCommission(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String, ByVal Incentive_Month As Integer, ByVal Incentive_Year As Integer, ByVal FilterBy As String, ByVal FilterValue As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtIncentive As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_LoadIncentive_Commission", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Org_ID", Org_ID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Incentive_Month", Incentive_Month)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Incentive_Year", Incentive_Year)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FilterBy", FilterBy)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FilterValue", FilterValue)
            objSQLDA.Fill(dtIncentive)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "90127"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        LoadExportIncentiveCommission = dtIncentive


    End Function
    Public Function GetTotalWeightage(ByRef Err_No As Long, ByRef Err_Desc As String, OrgId As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Dim Ds As New DataSet
        Dim sQry As String
        Dim dtIncentive As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDa = New SqlDataAdapter("SELECT  SUM(Weightage) AS Weightage FROM TBL_Incentive_Weightage WHERE Organization_ID ='" & OrgId & "' AND Is_Active ='Y'", objSQLConn)
            objSQLDa.SelectCommand.CommandType = CommandType.Text
            objSQLDa.Fill(dtIncentive)
            objSQLDa.Dispose()

        Catch ex As Exception

            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetTotalWeightage = dtIncentive
    End Function
    Public Function GetIncentiveMPYear(ByRef Err_No As Long, ByRef Err_Desc As String) As Integer
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtIncentive As New DataTable
        Dim Query As String = ""
        Dim year As Integer = 0
        Try
            Query = "SELECT ISNULL (MIN(Incentive_Year),0) FROM TBL_Incentive_MP"
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter(Query, objSQLConn)
            objSQLDA.Fill(dtIncentive)
            If dtIncentive.Rows.Count > 0 Then
                year = Convert.ToInt32(dtIncentive.Rows(0)(0).ToString())
            End If
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "940022"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return year
    End Function
    
End Class
