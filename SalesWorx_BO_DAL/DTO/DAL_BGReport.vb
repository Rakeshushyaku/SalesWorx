Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Data
Public Class DAL_BGReport
    Private _objDB As DatabaseConnection
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
   Public Function GetBGReports(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal FromDate As String, ByVal Todate As String, ByVal Status As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "SELECT A.Report_ID,A.Created_At,A.OutputFile, case A.Status when 'N' then 'New' when 'F' then 'Failed' when 'P' then 'Processing' when 'Y' then 'Processed' when 'X' then 'Cancelled' end as Status,B.Username,Case a.Status when 'Y' then 'true' else 'false' end as DownloadlIsVisible,Case a.Status when 'N' then 'true' else 'false' end as DelIsVisible from TBL_BG_Report A inner join TBL_User B on A.Created_By=B.User_ID   where 1=1"
            If FromDate <> "" Then
                QueryString = QueryString & " and A.Created_At>=convert(datetime,'" & FromDate & "',103)"
            End If
            If Todate <> "" Then
                QueryString = QueryString & " and A.Created_At<=convert(datetime,'" & Todate & " 23:59:59',103)"
            End If
             If Status <> "-1" Then
                QueryString = QueryString & " and A.Status='" & Status & "'"
            End If
            QueryString = QueryString & "  order by A.Created_At Desc"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetBGReports = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetBGReportDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "Select * from TBL_BG_Report_Criteria where Report_ID=" & Val(RID)
           
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetBGReportDetails = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function SaveBGReport(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Opt As String, ByVal RID As String, ByVal UserID As String, ByVal RepDt As DataTable) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Dim bChilTblSaved As Boolean = False
        Dim bMainTableSaved As Boolean = False
        Try

            objSQLConn = _objDB.GetSQLConnection
             Dim objSQLtrans As SqlTransaction
            objSQLtrans = objSQLConn.BeginTransaction()
        Try
            Dim Report_ID As String = ""
            objSQLCmd = New SqlCommand("app_SaveBGReport", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Transaction = objSQLtrans
            objSQLCmd.Parameters.Add(New SqlParameter("@Opt", SqlDbType.Int))
            objSQLCmd.Parameters("@Opt").Value = Opt
            objSQLCmd.Parameters.Add(New SqlParameter("@ID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@ID").Value = Val(RID)
            objSQLCmd.Parameters.Add(New SqlParameter("@UserID", SqlDbType.Int))
            objSQLCmd.Parameters("@UserID").Value = UserID
            objSQLCmd.Parameters.Add("@Report_ID", SqlDbType.BigInt)
            objSQLCmd.Parameters("@Report_ID").Direction = ParameterDirection.Output

            objSQLCmd.ExecuteNonQuery()
            Dim id As String
            id = objSQLCmd.Parameters("@Report_ID").Value.ToString()

            If Val(id) > 0 Then
                Report_ID = id
                bMainTableSaved = True
            Else
                bMainTableSaved = False
            End If

            If bMainTableSaved = True Then
             If Opt = "1" Then
                     If RepDt.Rows.Count > 0 Then

                         Dim dt As New DataTable
                         dt.Columns.Add("Row_ID", System.Type.GetType("System.Guid"))
                         dt.Columns.Add("Report_ID", System.Type.GetType("System.Int32"))
                         dt.Columns.Add("Column_Name", System.Type.GetType("System.String"))
                         dt.Columns.Add("Value", System.Type.GetType("System.String"))
                         dt.Columns.Add("Display_Value", System.Type.GetType("System.String"))

                         For Each dr As DataRow In RepDt.Rows

                                Dim drTarget As DataRow
                                drTarget = dt.NewRow
                                drTarget(0) = Guid.NewGuid()
                                drTarget(1) = Report_ID
                                drTarget("Column_Name") = dr("Column_Name").ToString
                                drTarget("Value") = dr("Value").ToString
                                drTarget("Display_Value") = dr("Display_Value").ToString
                                dt.Rows.Add(drTarget)

                         Next

                        Using bulkCopy As SqlBulkCopy = _
                          New SqlBulkCopy(objSQLConn, SqlBulkCopyOptions.Default, objSQLtrans)
                            bulkCopy.DestinationTableName = "dbo.TBL_BG_Report_Criteria"
                            bulkCopy.ColumnMappings.Add("Row_ID", "Row_ID")
                            bulkCopy.ColumnMappings.Add("Report_ID", "Report_ID")
                            bulkCopy.ColumnMappings.Add("Column_Name", "Column_Name")
                            bulkCopy.ColumnMappings.Add("Value", "Value")
                            bulkCopy.ColumnMappings.Add("Display_Value", "Display_Value")

                            bulkCopy.WriteToServer(dt)
                            bChilTblSaved = True
                        End Using

                     End If
              Else
                bChilTblSaved = True
              End If

            End If
            objSQLtrans.Commit()

            objSQLCmd.Dispose()
            objSQLCmd = Nothing
            Catch ex As Exception
                objSQLtrans.Rollback()
            End Try
        Catch ex As Exception

            Err_No = "7400920"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        If bMainTableSaved = True And bChilTblSaved = True Then
            bRetVal = True
        End If
        Return bRetVal
    End Function
End Class
