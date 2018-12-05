Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Data
Public Class DAL_OrgConfig
    Private _objDB As DatabaseConnection
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Public Function GetOrganisation(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QueryStr As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("SELECT DISTINCT MAS_Org_ID, Description  FROM TBL_Org_CTL_DTL WHERE SalesRep_ID IN ({0}) ORDER BY MAS_Org_ID DESC", QueryStr)
            'Dim QueryString As String = String.Format("select * from (SELECT DISTINCT Org_ID,Description FROM TBL_Org_CTL_DTL WHERE SalesRep_ID IN ({0})) as X ORDER BY Description Asc", QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")

            GetOrganisation = MsgDs.Tables(0)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74091"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    Public Function GetDistinctVanOrgs(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("SELECT A.Org_ID,B.SalesRep_Name from TBL_Org_CTL_DTL A inner join TBL_FSR B on a.org_ID=b.SalesRep_Number order by  B.SalesRep_Name ")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustomerTbl")

            GetDistinctVanOrgs = MsgDs.Tables("CustomerTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetDistinctWareHouses(ByRef Err_No As Long, ByRef Err_Desc As String, Optional ByVal QueryStr As String = "") As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String
            If Trim(QueryStr) = "" Then
                QueryString = String.Format("select * from (SELECT distinct Org_ID,Description from TBL_Org_Info) as X order by Description ")
            Else
                QueryString = String.Format("select * from (SELECT distinct Org_ID,Description from TBL_Org_Info where Custom_Attribute_1='" & QueryStr & "') as X order by Description ")
            End If
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustomerTbl")

            GetDistinctWareHouses = MsgDs.Tables("CustomerTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetOrgConfig(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Criteria As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_GetOrgConfig", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Criteria", Criteria)
            objSQLDA.Fill(dtDivConfig)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "74204"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetOrgConfig = dtDivConfig
    End Function
       Public Function GetCurrency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("select top 1 Currency_Code  from TBL_Org_CTL_DTL where Mas_org_ID='" & OrgId & "'")
             objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustomerTbl")

            GetCurrency = MsgDs.Tables("CustomerTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetSalesMan(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("select * from(SELECT distinct Emp_Code,Emp_Name from TBL_Emp_Info where Emp_Code not in(select Emp_Code from TBL_Van_Info) ) as X order by Emp_Name")
             objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustomerTbl")

            GetSalesMan = MsgDs.Tables("CustomerTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function UpdateOrgConfig(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Mas_org_ID As String, ByVal Sales_org_ID As String, ByVal Emp_Code As String, DocPrefix As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_UpdateOrgConfig", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@Org_ID", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Org_ID").Value = OrgID
            objSQLCmd.Parameters.Add(New SqlParameter("@Mas_Org_ID", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Mas_Org_ID").Value = Mas_org_ID
            objSQLCmd.Parameters.Add(New SqlParameter("@Sales_Org_ID", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Sales_Org_ID").Value = Sales_org_ID
            objSQLCmd.Parameters.Add(New SqlParameter("@Empcode", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Empcode").Value = Emp_Code
            objSQLCmd.Parameters.Add(New SqlParameter("@DocPrefix", SqlDbType.VarChar, 10))
            objSQLCmd.Parameters("@DocPrefix").Value = DocPrefix
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()
            objSQLCmd = Nothing

            bRetVal = True

        Catch ex As Exception
            Err_No = "74206"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function
End Class
