Imports System.Configuration
Imports System.Data.SqlClient

Public Class DAL_Login
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Private _objDB As DatabaseConnection
    Private Shared _strSQLConn As String = ConfigurationSettings.AppSettings("SQLConnString")

    Public Function ValidateUser(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal UserName As String, ByVal Password As String, ByVal Mode As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        ' Dim objSQLDR As SqlDataReader

        Dim dt As New DataTable

        Dim sQry As String
        Dim retVal As Boolean = False
        Try
            ' If UserName <> "" And Password <> "" Then
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            If Mode = "SWX" Then
                sQry = "SELECT A.User_ID, A.SalesRep_ID, B.Menu_Id, B.Page_Id, B.Field_Rights, A.Is_SS, C.Org_ID, C.Site, D.PDA_Rights ,D.User_Type_ID,(select top 1 D.Currency_Code from dbo.TBL_Org_CTL_DTL D inner join TBL_Currency Z on  D.Currency_Code=Z.Currency_Code  where SalesRep_Id=A.SalesRep_ID) AS Currency,(select top 1 Z.Decimal_Digits from dbo.TBL_Org_CTL_DTL D inner join TBL_Currency Z on  D.Currency_Code=Z.Currency_Code  where SalesRep_Id=A.SalesRep_ID) AS DecimalDigits from TBL_User As A INNER JOIN TBL_User_Rights as B ON A.User_Type_Id=B.User_Type_Id LEFT JOIN TBL_Org_CTL_DTL As C ON A.SalesRep_ID=C.SalesRep_ID INNER JOIN TBL_User_Types As D ON A.User_Type_ID=D.User_Type_ID where A.Username=@USERNAME AND A.Password=@PASSWORD AND B.Menu_Id NOT LIKE 'HM%' order by B.Menu_Id, B.Page_Id, B.Field_Rights"
            Else
                sQry = "SELECT A.User_ID, A.SalesRep_ID, B.Menu_Id, B.Page_Id, B.Field_Rights, A.Is_SS, C.Org_ID, C.Site,D.PDA_Rights,D.User_Type_ID,(select top 1 D.Currency_Code from dbo.TBL_Org_CTL_DTL D inner join TBL_Currency Z on  D.Currency_Code=Z.Currency_Code  where SalesRep_Id=A.SalesRep_ID) AS Currency,(select top 1 Z.Decimal_Digits from dbo.TBL_Org_CTL_DTL D inner join TBL_Currency Z on  D.Currency_Code=Z.Currency_Code  where SalesRep_Id=A.SalesRep_ID) AS DecimalDigits from TBL_User As A INNER JOIN TBL_User_Rights as B ON A.User_Type_Id=B.User_Type_Id LEFT JOIN TBL_Org_CTL_DTL As C ON A.SalesRep_ID=C.SalesRep_ID INNER JOIN TBL_User_Types As D ON A.User_Type_ID=D.User_Type_ID where A.Username=@USERNAME  AND B.Menu_Id NOT LIKE 'HM%' order by B.Menu_Id, B.Page_Id, B.Field_Rights"
            End If

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@USERNAME", UserName)
            objSQLCmd.Parameters.AddWithValue("@PASSWORD", Password)
            Dim objSQLDa As New SqlDataAdapter(objSQLCmd)
            objSQLDa.Fill(dt)
            ' Else
            ' Error_No = 75001
            ' Error_Desc = "Invalid username/password."
            ' End If

        Catch ex As Exception
            Error_No = 75001
            Error_Desc = String.Format("Error while validating: {0}", ex.Message)

        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function AssignedSalesRep(ByVal UserId As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim objSQLDt As New DataTable
        Dim objSQLDa As New SqlDataAdapter
        Dim sQry As String
        Dim bRetBool As Boolean = False

        Try
            If UserId <> 0 Then
                'getting MSSQL DB connection.....
                objSQLConn = _objDB.GetSQLConnection
                sQry = String.Format("select * from app_GetControlInfo({0})", UserId)
                '  sQry = String.Format("select User_ID,SalesRep_ID  from  TBL_User_FSR_Map  where User_Id={0}", UserId)
                objSQLCmd = New SqlCommand(sQry, objSQLConn)
                objSQLDa = New SqlDataAdapter(objSQLCmd)
                objSQLDa.Fill(objSQLDt)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(objSQLConn) Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        Return objSQLDt
    End Function

    'Insert User Log
    Public Function InsertUserLog(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal TranType As Char, ByVal ModuleName As String, ByVal SubModule As String, ByVal KeyValue As String, ByVal Description As String, ByVal LoggedBy As String, ByVal Van As String, ByVal OrgId As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "SaveUserLog"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@TranType", SqlDbType.Char, 1).Value = TranType
            objSQLCmd.Parameters.Add("@Module", SqlDbType.VarChar, 100).Value = ModuleName
            objSQLCmd.Parameters.Add("@SubModule", SqlDbType.VarChar, 100).Value = SubModule
            objSQLCmd.Parameters.Add("@KeyValue", SqlDbType.VarChar, 100).Value = KeyValue
            objSQLCmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = Description
            objSQLCmd.Parameters.Add("@ModifiedBy", SqlDbType.VarChar, 100).Value = LoggedBy
            objSQLCmd.Parameters.Add("@Van", SqlDbType.VarChar, 25).Value = Van
            objSQLCmd.Parameters.Add("@SalesOrg", SqlDbType.VarChar, 25).Value = OrgId
            objSQLCmd.Parameters.Add("@CustomAttribute1", SqlDbType.VarChar, 1000).Value = "0"
            objSQLCmd.Parameters.Add("@CustomAttribute2", SqlDbType.VarChar, 50).Value = "0"
            objSQLCmd.Parameters.Add("@CustomAttribute3", SqlDbType.Int).Value = 0

            objSQLCmd.ExecuteNonQuery()
            success = True
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74095"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function

    Public Function GetModule() As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable

        Dim sQry As String

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            sQry = "select DISTINCT Module  from TBL_Audit_Log order by Module ASC"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLDa = New SqlDataAdapter(objSQLCmd)
            objSQLDa.Fill(dt)
            objSQLCmd.Dispose()
            Return dt
        Catch ex As Exception
            Throw ex
        Finally

            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return dt
    End Function

    Public Function GetSubModule() As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable

        Dim sQry As String

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            sQry = "select DISTINCT Sub_Module from TBL_Audit_Log order by  Sub_Module ASC"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLDa = New SqlDataAdapter(objSQLCmd)
            objSQLDa.Fill(dt)
            objSQLCmd.Dispose()
            Return dt
        Catch ex As Exception
            Throw ex
        Finally

            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return dt
    End Function
End Class
