Imports System.Data.SqlClient
Imports System.Configuration
Public Class DAL_SyncCutOffDate
    Private _objDB As DatabaseConnection
    Private Shared _strSQLConn As String = ConfigurationSettings.AppSettings("SQLConnString")
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Public Function DeleteSyncCutOffDate(ByRef Err_No As Long, ByRef Err_Desc As String, Country As String, year As String, month As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sRetVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("Delete from TBL_Sync_CutOff_Dates where [Period_Year]=@Year and [Period_Month]=@Month and Country=@Country", objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add(New SqlParameter("@Year", SqlDbType.Int))
            objSQLCmd.Parameters("@Year").Value = year
            objSQLCmd.Parameters.Add(New SqlParameter("@Month", SqlDbType.Int))
            objSQLCmd.Parameters("@Month").Value = month
            objSQLCmd.Parameters.Add(New SqlParameter("@Country", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Country").Value = Country
            objSQLCmd.ExecuteNonQuery()


            sRetVal = True

            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74014"
            Err_Desc = ex.Message
            sRetVal = False
            Throw ex
        Finally
            objSQLCmd = Nothing
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        DeleteSyncCutOffDate = sRetVal
    End Function

    Public Function SaveSyncCutOffDate(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Year As String, Month As String, CutoffDate As String, UserID As String, Country As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sRetVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("app_SaveSyncCutoffDate", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@Date", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Date").Value = CutoffDate
            objSQLCmd.Parameters.Add(New SqlParameter("@Year", SqlDbType.Int))
            objSQLCmd.Parameters("@Year").Value = Year
            objSQLCmd.Parameters.Add(New SqlParameter("@Month", SqlDbType.Int))
            objSQLCmd.Parameters("@Month").Value = Month
            objSQLCmd.Parameters.Add(New SqlParameter("@Created_By", SqlDbType.Int))
            objSQLCmd.Parameters("@Created_By").Value = UserID
            objSQLCmd.Parameters.Add(New SqlParameter("@Country", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Country").Value = Country
            objSQLCmd.ExecuteNonQuery()


            sRetVal = True

            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74014"
            Err_Desc = ex.Message
            sRetVal = False
            Throw ex
        Finally
            objSQLCmd = Nothing
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        SaveSyncCutOffDate = sRetVal
    End Function

    Public Function GetYears(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtsurvey As New DataTable
        Try

            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("select distinct Period_Year as Year  from TBL_Sync_CutOff_Dates ", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.Fill(dtsurvey)

        Catch ex As Exception
            Err_No = "75016"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetYears = dtsurvey

    End Function
    Public Function GetCountries(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtsurvey As New DataTable
        Try
            Dim qry As String
            qry = "Select * from TBL_App_Codes where code_type='COUNTRY' order by Code_Description"
            objSQLConn = _objDB.GetSQLConnection

            objSQLDA = New SqlDataAdapter(qry, objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.Fill(dtsurvey)

        Catch ex As Exception
            Err_No = "75016"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetCountries = dtsurvey

    End Function
    Public Function GetSyncCutOffDates(ByRef Err_No As Long, ByRef Err_Desc As String, year As String, Month As String, Country As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtsurvey As New DataTable
        Try
            Dim qry As String
            qry = "select *,case when CONVERT(datetime,cast([Period_Year] AS varchar)+'-'+cast([Period_Month]AS varchar)+'-01')> =CONVERT(datetime,cast(YEAR(getdate()) AS varchar)+'-'+cast(Month(getdate()) AS varchar)+'-01') And Sync_CutOff_Time >= DateAdd(Minute, 10, GETDATE()) then 1 else 0 end as editable,case when CONVERT(datetime,cast([Period_Year] AS varchar)+'-'+cast([Period_Month]AS varchar)+'-01')>DATEADD(day,-1*(day(getdate())-1), DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())))   then 1 else 0 end as deletable  from TBL_Sync_CutOff_Dates where 1=1"
            objSQLConn = _objDB.GetSQLConnection
            If year <> "0" Then
                qry = qry & " and Period_Year=" & year
            End If
            If Month <> "0" Then
                qry = qry & " and Period_Month=" & Month
            End If

            If Country <> "0" Then
                qry = qry & " and Country='" & Country & "'"
            End If
            qry = qry & " order by Period_Year,Period_Month desc"

            objSQLDA = New SqlDataAdapter(qry, objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.Fill(dtsurvey)

        Catch ex As Exception
            Err_No = "75016"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetSyncCutOffDates = dtsurvey

    End Function

End Class
