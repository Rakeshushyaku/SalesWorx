Imports System.Configuration
Imports System.Data.SqlClient
Public Class DAL_OrgCtl
    Private dtCurrency As New DataTable
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Private _objDB As DatabaseConnection
    Private Shared _strSQLConn As String = ConfigurationSettings.AppSettings("SQLConnString")
    Public Function GetOrgCTL(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtOrgCtl As New DataTable

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_GetOrgCtl", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            'objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", Criteria)
            objSQLDA.Fill(dtOrgCtl)
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
        GetOrgCTL = dtOrgCtl
    End Function


    Public Function InsertOrgCtl(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_des As String, ByVal Currency As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            Dim Dr As SqlDataReader
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_InsertOrgCtl", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure


            objSQLCmd.Parameters.Add(New SqlParameter("@Description", SqlDbType.VarChar, 50))
            objSQLCmd.Parameters("@Description").Value = Org_des

            objSQLCmd.Parameters.Add(New SqlParameter("@Currency", SqlDbType.VarChar, 10))
            objSQLCmd.Parameters("@Currency").Value = Currency

            Dr = objSQLCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If Dr.Read Then
                If Val(Dr(0).ToString) > 0 Then
                    bRetVal = True
                Else
                    Err_Desc = "The Description  already exists."
                End If
            End If
            Dr.Close()
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        Catch ex As Exception
            Err_No = "84205"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function


    Public Function UpdateOrgCtl(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ID As String, ByVal Description As String, ByVal Currency As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False

        Try
            Dim Dr As SqlDataReader
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_UpdateOrgCtl", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@ID", SqlDbType.VarChar))
            objSQLCmd.Parameters("@ID").Value = ID
            objSQLCmd.Parameters.Add(New SqlParameter("@Description", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Description").Value = Description
            objSQLCmd.Parameters.Add(New SqlParameter("@Currency", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Currency").Value = Currency
            Dr = objSQLCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If Dr.Read Then
                If Val(Dr(0).ToString) > 0 Then
                    bRetVal = True
                Else
                    Err_Desc = "The Description already exists."
                End If
            End If
            Dr.Close()
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        Catch ex As Exception
            Err_No = "8400920"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function
    Public Function FillCurrencyGrid(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT * FROM TBL_Currency ORDER BY Currency_Code", objSQLConn)
            objSQLDA.Fill(dtCurrency)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "740021"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtCurrency
    End Function

    Public Function ValidateDescription(ByVal Org_Id As Integer, ByVal Description As String) As Boolean
        Dim success As Boolean = False
        Dim objSQLConn As SqlConnection
        Dim objSQLCMD As SqlCommand
        Dim Rcnt As Integer
        Try
            Dim qry As String

            
            qry = "SELECT COUNT(*) FROM TBL_Org_CTL_H WHERE CASE CHARINDEX(' ', Description, 1) WHEN 0 THEN Description ELSE SUBSTRING(Description, 1, CHARINDEX(' ', Description, 1) - 1) END='" + Description + "'"


            If Org_Id > 0 Then
                qry = qry & " AND ORG_HE_ID<>" & Org_Id
            End If

            objSQLConn = _objDB.GetSQLConnection
            objSQLCMD = New SqlCommand(qry, objSQLConn)
            Rcnt = Convert.ToInt32(objSQLCMD.ExecuteScalar())
            objSQLCMD.Dispose()
            If Rcnt > 0 Then
                success = True
            End If
        Catch ex As Exception

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
    Public Function DeleteValidation(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ORG_ID As String) As Boolean

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtOrgCtl As New DataTable



        Dim objSQLCmd As SqlCommand
        Dim sRetVal As Boolean = False
        Try
            'objSQLConn = _objDB.GetSQLConnection
            'objSQLDA = New SqlDataAdapter("app_DeleteValidationOrgCtl", objSQLConn)
            'objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            'objSQLDA.SelectCommand.Parameters.AddWithValue("@ORG_ID", ORG_ID)
            'objSQLDA.Fill(dtOrgCtl)
            'objSQLDA.Dispose()



            Dim Dr As SqlDataReader
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("app_DeleteValidationOrgCtl", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@ORG_ID", SqlDbType.VarChar))
            objSQLCmd.Parameters("@ORG_ID").Value = ORG_ID
            Dr = objSQLCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If Dr.Read Then
                If Val(Dr(0).ToString) <= 0 Then
                    sRetVal = True
                Else
                    Err_Desc = "The Organization  is referenced in Customer/Product/Price List/ Org_CTL_DTL. You can not delete this."
                End If
            End If
            Dr.Close()

            objSQLCmd.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        ''   DeleteValidation = dtOrgCtl
        DeleteValidation = sRetVal
    End Function

    Public Function DeleteOrgCtl(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sRetVal As Boolean = False
        Try

            Dim Dr As SqlDataReader
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("app_DeleteOrgCtl", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@ID", SqlDbType.VarChar))
            objSQLCmd.Parameters("@ID").Value = ID
            Dr = objSQLCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If Dr.Read Then
                If Val(Dr(0).ToString) > 0 Then
                    sRetVal = True
                Else
                    Err_Desc = "The organization can not delete ."
                End If
            End If
            Dr.Close()
            sRetVal = True
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74015"
            Err_Desc = ex.Message
            sRetVal = False
            Throw ex
        Finally
            objSQLCmd = Nothing
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        DeleteOrgCtl = sRetVal
    End Function

    Public Function GetSearchResultOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Criteria As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtOrg As New DataTable

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_LoadOrg", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FVALUE", Criteria)
            objSQLDA.Fill(dtOrg)
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
        GetSearchResultOrg = dtOrg
    End Function
End Class
