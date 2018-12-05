Imports System.Configuration
Imports System.Data.SqlClient

Public Class DAL_DivConfig
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Private _objDB As DatabaseConnection
    Private Shared _strSQLConn As String = ConfigurationSettings.AppSettings("SQLConnString")

    Public Function GetDivConfig(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Criteria As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_LoadDivControl", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", Criteria)
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
        GetDivConfig = dtDivConfig
    End Function
    Public Function GetAllDivisions(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("select ORG_HE_ID ,Description,Site from dbo.TBL_Org_CTL_H", objSQLConn)
            objSQLDA.Fill(dtDivConfig)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "74207"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetAllDivisions = dtDivConfig
    End Function
    Public Function InsertDivConfig(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As Integer, ByVal Allow_Manual_FOC As String, ByVal Odo_Reading_At_Visit As String, ByVal Advance_PDC_Posting As Integer, ByVal FSRConfigList As ArrayList, ByVal PrintFormat As String, ByVal CollectionOutputFolder As String, ByVal CNLimit As Decimal, ByVal CreatedBy As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_InsertDivControl", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@Org_ID", SqlDbType.Int))
            objSQLCmd.Parameters("@Org_ID").Value = OrgID
            objSQLCmd.Parameters.Add(New SqlParameter("@Allow_Manual_FOC", SqlDbType.VarChar, 1))
            objSQLCmd.Parameters("@Allow_Manual_FOC").Value = Allow_Manual_FOC
            objSQLCmd.Parameters.Add(New SqlParameter("@Odo_Reading_At_Visit", SqlDbType.VarChar, 1))
            objSQLCmd.Parameters("@Odo_Reading_At_Visit").Value = Odo_Reading_At_Visit
            objSQLCmd.Parameters.Add(New SqlParameter("@Advance_PDC_Posting", SqlDbType.Int))
            objSQLCmd.Parameters("@Advance_PDC_Posting").Value = Advance_PDC_Posting
            objSQLCmd.Parameters.Add(New SqlParameter("@PrintFormat", SqlDbType.VarChar, 50))
            objSQLCmd.Parameters("@PrintFormat").Value = PrintFormat
            objSQLCmd.Parameters.Add(New SqlParameter("@CollectionOutputFolder", SqlDbType.VarChar, 50))
            objSQLCmd.Parameters("@CollectionOutputFolder").Value = CollectionOutputFolder
            objSQLCmd.Parameters.Add(New SqlParameter("@CNLimit", SqlDbType.VarChar, 100))
            objSQLCmd.Parameters("@CNLimit").Value = CStr(CNLimit)
            objSQLCmd.Parameters.Add(New SqlParameter("@CreatedBy", SqlDbType.Int))
            objSQLCmd.Parameters("@CreatedBy").Value = CreatedBy
            '  Dim objOutputParameter As New SqlParameter("@Div_Config_ID", SqlDbType.Int)
            ' objSQLCmd.Parameters.Add(objOutputParameter)
            ' objOutputParameter.Direction = ParameterDirection.Output
            objSQLCmd.ExecuteNonQuery()

            For Each objStr As String In FSRConfigList
                objSQLCmd = New SqlCommand("app_UpdateFSRConfig", objSQLConn)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.Parameters.Add(New SqlParameter("@SalesRep_ID", SqlDbType.BigInt))
                objSQLCmd.Parameters("@SalesRep_ID").Value = objStr.Split("|")(0)
                objSQLCmd.Parameters.Add(New SqlParameter("@Is_DC_Optional", SqlDbType.VarChar, 1))
                objSQLCmd.Parameters("@Is_DC_Optional").Value = objStr.Split("|")(1)
                objSQLCmd.ExecuteNonQuery()
            Next

            bRetVal = True
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        Catch ex As Exception
            Err_No = "74205"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function


    Public Function UpdateDivConfig(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As Integer, ByVal Allow_Manual_FOC As String, ByVal Odo_Reading_At_Visit As String, ByVal Advance_PDC_Posting As Integer, ByVal DivConfigID As Integer, ByVal FSRConfigList As ArrayList, ByVal PrintFormat As String, ByVal CollectionOutputFolder As String, ByVal CNLimit As Decimal, ByVal Updatedby As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_UpdateDivControl", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@Org_ID", SqlDbType.Int))
            objSQLCmd.Parameters("@Org_ID").Value = OrgID
            objSQLCmd.Parameters.Add(New SqlParameter("@Allow_Manual_FOC", SqlDbType.VarChar, 1))
            objSQLCmd.Parameters("@Allow_Manual_FOC").Value = Allow_Manual_FOC
            objSQLCmd.Parameters.Add(New SqlParameter("@Odo_Reading_At_Visit", SqlDbType.VarChar, 1))
            objSQLCmd.Parameters("@Odo_Reading_At_Visit").Value = Odo_Reading_At_Visit
            objSQLCmd.Parameters.Add(New SqlParameter("@Advance_PDC_Posting", SqlDbType.Int))
            objSQLCmd.Parameters("@Advance_PDC_Posting").Value = Advance_PDC_Posting
            objSQLCmd.Parameters.Add(New SqlParameter("@PrintFormat", SqlDbType.VarChar, 50))
            objSQLCmd.Parameters("@PrintFormat").Value = PrintFormat
            'objSQLCmd.Parameters.Add(New SqlParameter("@Div_Config_ID", SqlDbType.Int))
            'objSQLCmd.Parameters("@Div_Config_ID").Value = DivConfigID
            objSQLCmd.Parameters.Add(New SqlParameter("@CollectionOutputFolder", SqlDbType.VarChar, 50))
            objSQLCmd.Parameters("@CollectionOutputFolder").Value = CollectionOutputFolder
            objSQLCmd.Parameters.Add(New SqlParameter("@CNLimit", SqlDbType.VarChar, 100))
            objSQLCmd.Parameters("@CNLimit").Value = CStr(CNLimit)
            objSQLCmd.Parameters.Add(New SqlParameter("@UpdatedBy", SqlDbType.Int))
            objSQLCmd.Parameters("@UpdatedBy").Value = UpdatedBy
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()
            objSQLCmd = Nothing

            For Each objStr As String In FSRConfigList
                objSQLCmd = New SqlCommand("app_UpdateFSRConfig", objSQLConn)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.Parameters.Add(New SqlParameter("@SalesRep_ID", SqlDbType.BigInt))
                objSQLCmd.Parameters("@SalesRep_ID").Value = objStr.Split("|")(0)
                objSQLCmd.Parameters.Add(New SqlParameter("@Is_DC_Optional", SqlDbType.VarChar, 1))
                objSQLCmd.Parameters("@Is_DC_Optional").Value = objStr.Split("|")(1)
                objSQLCmd.ExecuteNonQuery()
            Next

            bRetVal = True
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        Catch ex As Exception
            Err_No = "74206"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function
    Public Function CheckDivControlExist(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select COUNT(*) FROM TBL_Div_Control WHERE Org_ID=@OrgID  ")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            Dim cnt As Integer = 0
            cnt = Convert.ToInt32(objSQLCmd.ExecuteScalar())
            If cnt > 0 Then
                success = True
            End If
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24066"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
    Public Function DeleteDivConfig(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal _Div_Config_ID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        ' Dim sQry As String
        Dim iRowsAffected As Integer = 0
        Dim retVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            Try

                'getting MSSQL DB connection.....

                objSQLCmd = New SqlCommand("app_DeleteDivControl", objSQLConn)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.Parameters.Add(New SqlParameter("@Div_Config_ID", SqlDbType.VarChar, 500))
                objSQLCmd.Parameters("@Div_Config_ID").Value = _Div_Config_ID
                'sQry = String.Format("delete from TBL_Div_Config where Div_Config_ID in ({0})", _Div_Config_ID)
                'objSQLCmd = New SqlCommand(sQry, objSQLConn)
                objSQLCmd.ExecuteNonQuery()
                retVal = True
                'If iRowsAffected > 0 Then
                '    retVal = True
                'Else
                '    Error_No = 74210
                '    Error_Desc = "Unable to delete configuration."
                'End If

            Catch ex As Exception
                Throw ex
            End Try
        Catch ex As Exception

            Error_No = 74210
            Error_Desc = String.Format("Error while deleting configuration: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        DeleteDivConfig = retVal
    End Function
End Class
