Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Data
Public Class DAL_SalesTarget
 Private _objDB As DatabaseConnection
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Public Function CheckValidCustomer(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustNo As String, ByVal SalesRepNo As String, PrimaryClass As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = ""
            If PrimaryClass = "Customer" Then
                QueryString = String.Format("select COUNT(*) FROM V_FSR_CustomerShipAddress AS A  WHERE A.Customer_No =@CustNo AND A.SalesRep_ID IN(SELECT SalesRep_ID FROM TBL_FSR WHERE SalesRep_Number=@SID) ")
            ElseIf PrimaryClass = "Area" Then
                'QueryString = String.Format("select COUNT(*) FROM TBL_Area_FSR_Map AS A INNER JOIN TBL_Sales_District AS B On A.Sales_District_ID=B.Sales_District_ID  WHERE B.Description=@CustNo AND A.SalesRep_ID IN(SELECT SalesRep_ID FROM TBL_FSR WHERE SalesRep_Number=@SID) ")
                QueryString = String.Format("select COUNT(*) FROM TBL_Customer AS A INNER JOIN V_FSR_CustomerShipAddress AS B  On A.Customer_No=B.Customer_No  WHERE A.Area=@CustNo AND B.SalesRep_ID IN(SELECT SalesRep_ID FROM TBL_FSR WHERE SalesRep_Number=@SID) ")
            End If

            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@CustNo", SqlDbType.VarChar, 50).Value = CustNo
            objSQLCmd.Parameters.Add("@SID", SqlDbType.VarChar, 50).Value = SalesRepNo
            Dim cnt As Integer = 0
            cnt = Convert.ToInt32(objSQLCmd.ExecuteScalar())
            If cnt > 0 Then
                success = True
            End If
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "33430"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function



    Public Function CheckAgencyOrCategory(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Mode As String, ByVal ClassValue As String, ByVal SalesRepNo As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = Nothing

            If Mode = "A" Then
                QueryString = String.Format("select COUNT(*) from TBL_Product where Agency=@ClassValue ")
            ElseIf Mode = "K" Then
                QueryString = String.Format("select COUNT(*) from TBL_Product where Category=@ClassValue ")
            ElseIf Mode = "B" Then
                QueryString = String.Format("select COUNT(*) from TBL_Product where Brand_Code=@ClassValue")
            ElseIf Mode = "P" Then
                QueryString = String.Format("select COUNT(*) from TBL_Product where Item_Code=@ClassValue")
            End If
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@ClassValue", SqlDbType.VarChar, 500).Value = ClassValue
            objSQLCmd.Parameters.Add("@SID", SqlDbType.VarChar, 250).Value = SalesRepNo
            Dim cnt As Integer = 0
            cnt = Convert.ToInt32(objSQLCmd.ExecuteScalar())

            objSQLCmd.Dispose()

            If cnt > 0 Then
                success = True
            End If
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "27841"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
    Public Function UploadFSRSalesTarget(ByVal dtData As DataTable, ByVal dtDeletedFSR As DataTable, ByVal TargetType As String, ByVal ValueType As String, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CreatedBy As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Dim tran As SqlTransaction = Nothing
        Dim QueryString As String = Nothing
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            tran = objSQLConn.BeginTransaction()

            'Delete  FSR Target item data for the current or future month and year from the excel sheet
            For Each r As DataRow In dtDeletedFSR.Rows
                QueryString = "DELETE FROM TBL_Sales_Target_Items WHERE Target_Month =@Month AND Sales_Target_ID IN(SELECT Sales_Target_ID FROM TBL_Sales_Target WHERE SalesRep_ID IN(SELECT SalesRep_ID FROM TBL_FSR WHERE SalesRep_Number=@SalesRepID) AND Target_Year =@Year)"
                objSQLCmd = New SqlCommand(QueryString, objSQLConn)
                objSQLCmd.Parameters.AddWithValue("@SalesRepID", r("SalesRepID").ToString())
                objSQLCmd.Parameters.AddWithValue("@Year", r("Year").ToString())
                objSQLCmd.Parameters.AddWithValue("@Month", r("Month").ToString())
                objSQLCmd.Transaction = tran
                objSQLCmd.ExecuteNonQuery()
                objSQLCmd.Dispose()
            Next
           

            For Each r As DataRow In dtData.Rows
                If r("IsValid").ToString() = "Y" Then

                    QueryString = "usp_UploadSalesTarget"

                    Dim sTarget_Type As String = TargetType
                    Dim sValue_Type As String = ValueType


                    objSQLCmd.CommandText = QueryString
                    objSQLCmd.CommandType = CommandType.StoredProcedure
                    objSQLCmd.Parameters.Clear()

                    objSQLCmd.Parameters.Add(New SqlParameter("@SalesRep_No", SqlDbType.VarChar))
                    objSQLCmd.Parameters("@SalesRep_No").Value = r("SalesRepNo").ToString()

                    objSQLCmd.Parameters.Add(New SqlParameter("@Target_Type", SqlDbType.Char, 1))
                    objSQLCmd.Parameters("@Target_Type").Value = sTarget_Type

                    objSQLCmd.Parameters.Add(New SqlParameter("@Value_Type", SqlDbType.Char, 1))
                    objSQLCmd.Parameters("@Value_Type").Value = sValue_Type

                    objSQLCmd.Parameters.Add(New SqlParameter("@Target_Year", SqlDbType.Int))
                    objSQLCmd.Parameters("@Target_Year").Value = r("Year").ToString()

                    objSQLCmd.Parameters.Add(New SqlParameter("@Last_Updated_By", SqlDbType.Int))
                    objSQLCmd.Parameters("@Last_Updated_By").Value = CreatedBy

                    objSQLCmd.Parameters.Add(New SqlParameter("@Month", SqlDbType.Int))
                    objSQLCmd.Parameters("@Month").Value = r("Month").ToString()


                    objSQLCmd.Parameters.Add(New SqlParameter("@Agency", SqlDbType.VarChar))
                    objSQLCmd.Parameters("@Agency").Value = r("SecondaryClassification").ToString()



                    objSQLCmd.Parameters.Add(New SqlParameter("@Classification", SqlDbType.VarChar))
                    objSQLCmd.Parameters("@Classification").Value = r("PrimaryClassification").ToString()

                    objSQLCmd.Parameters.Add(New SqlParameter("@Target_Value", SqlDbType.VarChar))
                    objSQLCmd.Parameters("@Target_Value").Value = r("TargetValue").ToString()

                    objSQLCmd.Parameters.Add(New SqlParameter("@TargetQty", SqlDbType.VarChar))
                    objSQLCmd.Parameters("@TargetQty").Value = r("TargetQty").ToString()

                    objSQLCmd.Parameters.Add(New SqlParameter("@Mode", SqlDbType.Char, 1))
                    objSQLCmd.Parameters("@Mode").Value = "A"

                    objSQLCmd.Parameters.Add(New SqlParameter("@SaveMethod", SqlDbType.VarChar))
                    objSQLCmd.Parameters("@SaveMethod").Value = "Upload"

                    objSQLCmd.Transaction = tran
                    objSQLCmd.ExecuteNonQuery()
                    objSQLCmd.Dispose()
                End If
            Next



            success = True
            tran.Commit()
        Catch ex As Exception
            Err_No = "52167"
            Err_Desc = ex.Message
            tran.Rollback()
            Throw ex
        Finally
            objSQLCmd = Nothing
            tran.Dispose()
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function

    Public Function ExportFSRTargetTemplate(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal OrgID As String, ByVal SID As Integer, ByVal TargetMonth As Date) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim sQry As String
        Dim sRetVal As String = ""
        Dim dtFSRCustomers As New DataTable
        Try

            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            sQry = "app_ExportFSRTargetTemplate"
            objSQLDA = New SqlDataAdapter(sQry, objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            objSQLDA.SelectCommand.Parameters.Add("@SID", SqlDbType.BigInt).Value = SID
            objSQLDA.SelectCommand.Parameters.Add("@TargetMonth", SqlDbType.DateTime).Value = TargetMonth
            objSQLDA.Fill(dtFSRCustomers)
            objSQLDA.Dispose()
        Catch ex As Exception
            sRetVal = sRetVal & ex.Message
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtFSRCustomers
    End Function
    Public Function GetTargetDefinitionforExport(ByRef Err_No As Long, ByRef Err_Desc As String, ByRef OrgId As String, ByVal SalesRepID As String, ByVal Year As String, ByVal Month As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
         Dim objSQLDA As SqlDataAdapter
         Dim dt As New DataSet
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_GetSalesTargetDefinitionforExport", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SalesRepID", SalesRepID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SalesYear", Year)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SalesMonth", Month)
            objSQLDA.Fill(dt)
            objSQLDA.Dispose()
            Return dt
        Catch ex As Exception
            Err_No = "74061"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    Public Function GetTargetDefinition(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRepID As String, ByVal Year As String, ByVal Month As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
         Dim objSQLDA As SqlDataAdapter
         Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_GetSalesTargetDefinition", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SalesRepID", SalesRepID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SalesYear", Year)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SalesMonth", Month)
            objSQLDA.Fill(dt)
            objSQLDA.Dispose()
            Return dt
        Catch ex As Exception
            Err_No = "74061"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    Public Function GetTargetYear(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
         Dim objSQLDA As SqlDataAdapter
         Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("select * from (Select distinct Target_Year from TBL_Sales_Target) as X order by Target_Year", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
         
            objSQLDA.Fill(dt)
            objSQLDA.Dispose()
            Return dt
        Catch ex As Exception
            Err_No = "74061"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    Public Function SaveSalesTarget(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserID As String, ByVal SalesRepID As String, ByVal Year As String, ByVal Month As String, ByVal Value_type As String, ByVal SalesTargetDt As DataTable) As Boolean
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
            Dim Sales_Target_ID As String = ""
            objSQLCmd = New SqlCommand("usp_SaveSalesTarget", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Transaction = objSQLtrans
             objSQLCmd.Parameters.Add(New SqlParameter("@SalesRep_ID", SqlDbType.Int))
            objSQLCmd.Parameters("@SalesRep_ID").Value = SalesRepID
            objSQLCmd.Parameters.Add(New SqlParameter("@Value_Type", SqlDbType.Char))
            objSQLCmd.Parameters("@Value_Type").Value = Value_type
            objSQLCmd.Parameters.Add(New SqlParameter("@Target_Year", SqlDbType.Int))
            objSQLCmd.Parameters("@Target_Year").Value = Val(Year)

            objSQLCmd.Parameters.Add(New SqlParameter("@UserID", SqlDbType.Int))
            objSQLCmd.Parameters("@UserID").Value = UserID
            objSQLCmd.Parameters.Add("@Sales_Target_ID", SqlDbType.Int)
            objSQLCmd.Parameters("@Sales_Target_ID").Direction = ParameterDirection.Output

            objSQLCmd.ExecuteNonQuery()
            Dim id As String
            id = objSQLCmd.Parameters("@Sales_Target_ID").Value.ToString()

            If Val(id) > 0 Then
                Sales_Target_ID = id
                bMainTableSaved = True
            Else
                bMainTableSaved = False
            End If

            If bMainTableSaved = True Then
                     If SalesTargetDt.Rows.Count > 0 Then

                         Dim dt As New DataTable
                         dt.Columns.Add("Target_Item_ID", System.Type.GetType("System.Guid"))
                         dt.Columns.Add("Sales_Target_ID", System.Type.GetType("System.Int32"))
                         dt.Columns.Add("Target_Month", System.Type.GetType("System.Int32"))
                         dt.Columns.Add("Classification_1", System.Type.GetType("System.String"))
                         dt.Columns.Add("Target_Value_1", System.Type.GetType("System.Decimal"))
                         dt.Columns.Add("Target_Value_2", System.Type.GetType("System.Decimal"))
                         dt.Columns.Add("Last_Updated_At", System.Type.GetType("System.DateTime"))


                         For Each dr As DataRow In SalesTargetDt.Rows
                            If dr("Target_Value_1").ToString <> "" Or dr("Target_Value_2").ToString <> "" Then
                                Dim drTarget As DataRow
                                drTarget = dt.NewRow
                                drTarget(0) = Guid.NewGuid()
                                drTarget(1) = Sales_Target_ID
                                drTarget(2) = Month
                                drTarget(3) = dr("Item_Code").ToString
                                If dr("Target_Value_1").ToString = "" Then
                                  drTarget(4) = "0"
                                Else
                                  drTarget(4) = dr("Target_Value_1").ToString
                                End If

                                If dr("Target_Value_2").ToString = "" Then
                                  drTarget(5) = "0"
                                Else
                                  drTarget(5) = dr("Target_Value_2").ToString
                                End If
                                drTarget(6) = Now

                                dt.Rows.Add(drTarget)
                            End If
                         Next

                         Dim objSQLCmdNew As SqlCommand
                         Dim QueryString As String
                         QueryString = "Delete from TBL_Sales_Target_Items WHERE Sales_Target_ID=@Sales_Target_ID AND Target_Month=@Target_Month"
                         objSQLCmdNew = New SqlCommand(QueryString, objSQLConn)
                         objSQLCmdNew.CommandType = CommandType.Text
                         objSQLCmdNew.Parameters.AddWithValue("@Sales_Target_ID", Sales_Target_ID)
                         objSQLCmdNew.Parameters.AddWithValue("@Target_Month", Month)
                         objSQLCmdNew.Transaction = objSQLtrans
                         objSQLCmdNew.ExecuteNonQuery()


                        Using bulkCopy As SqlBulkCopy = _
                          New SqlBulkCopy(objSQLConn, SqlBulkCopyOptions.Default, objSQLtrans)
                            bulkCopy.DestinationTableName = "dbo.TBL_Sales_Target_Items"
                            bulkCopy.ColumnMappings.Add("Target_Item_ID", "Target_Item_ID")
                            bulkCopy.ColumnMappings.Add("Sales_Target_ID", "Sales_Target_ID")
                            bulkCopy.ColumnMappings.Add("Target_Month", "Target_Month")
                            bulkCopy.ColumnMappings.Add("Classification_1", "Classification_1")
                            bulkCopy.ColumnMappings.Add("Target_Value_1", "Target_Value_1")
                            bulkCopy.ColumnMappings.Add("Target_Value_2", "Target_Value_2")
                            bulkCopy.ColumnMappings.Add("Last_Updated_At", "Last_Updated_At")

                            bulkCopy.WriteToServer(dt)
                            bChilTblSaved = True
                        End Using

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
    Public Function SalesrepIDfromNumber(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRepNo As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select salesrep_ID FROM  TBL_FSR where  Salesrep_Number=@SalesRepNo")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@SalesRepNo", SqlDbType.VarChar, 100).Value = SalesRepNo

          Dim dr As SqlDataReader

            dr = objSQLCmd.ExecuteReader
            If dr.Read Then
             Return dr(0).ToString
            End If
            dr.Close()
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "11322"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
    Public Function CheckValidFSRID(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRepNo As String, ByVal OrgID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select COUNT(*) FROM TBL_Org_CTL_DTL AS A INNER JOIN TBL_FSR AS B ON A.SalesRep_ID=B.SalesRep_ID WHERE B.Salesrep_Number=@SalesRepNo AND MAS_Org_ID=@OrgID ")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@SalesRepNo", SqlDbType.VarChar, 100).Value = SalesRepNo
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            Dim cnt As Integer = 0
            cnt = Convert.ToInt32(objSQLCmd.ExecuteScalar())
            If cnt > 0 Then
                success = True
            End If
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "11322"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
    Public Function CheckValidProductCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select COUNT(*) FROM TBL_Product  WHERE Item_Code=@ItemCode  ")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 100).Value = ItemCode
                        Dim cnt As Integer = 0
            cnt = Convert.ToInt32(objSQLCmd.ExecuteScalar())
            If cnt > 0 Then
                success = True
            End If
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "11322"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
     Public Function UploadSalesTarget(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserID As String, ByVal Year As String, ByVal Month As String, ByVal SalesTargetDt As DataTable) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Dim bChilTblSaved As Boolean = False
        Dim bMainTableSaved As Boolean = False
        Try

            
        Dim view As New DataView(SalesTargetDt)
        Dim distinctValues As DataTable

        distinctValues = view.ToTable(True, "SalesRepNumber")
        objSQLConn = _objDB.GetSQLConnection
            For Each salesrepdr In distinctValues.Rows
             Dim objSQLtrans As SqlTransaction
            objSQLtrans = objSQLConn.BeginTransaction()
            Try
            Dim salesrepid As String
            salesrepid = SalesrepIDfromNumber(Err_No, Err_Desc, salesrepdr("SalesRepNumber").ToString)
            Dim Sales_Target_ID As String = ""
            objSQLCmd = New SqlCommand("usp_SaveSalesTarget", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Transaction = objSQLtrans
             objSQLCmd.Parameters.Add(New SqlParameter("@SalesRep_ID", SqlDbType.Int))
            objSQLCmd.Parameters("@SalesRep_ID").Value = salesrepid
            objSQLCmd.Parameters.Add(New SqlParameter("@Value_Type", SqlDbType.Char))
            objSQLCmd.Parameters("@Value_Type").Value = "B"
            objSQLCmd.Parameters.Add(New SqlParameter("@Target_Year", SqlDbType.Int))
            objSQLCmd.Parameters("@Target_Year").Value = Val(Year)

            objSQLCmd.Parameters.Add(New SqlParameter("@UserID", SqlDbType.Int))
            objSQLCmd.Parameters("@UserID").Value = UserID
            objSQLCmd.Parameters.Add("@Sales_Target_ID", SqlDbType.Int)
            objSQLCmd.Parameters("@Sales_Target_ID").Direction = ParameterDirection.Output

            objSQLCmd.ExecuteNonQuery()
            Dim id As String
            id = objSQLCmd.Parameters("@Sales_Target_ID").Value.ToString()

            If Val(id) > 0 Then
                Sales_Target_ID = id
                bMainTableSaved = True
            Else
                bMainTableSaved = False
            End If

            If bMainTableSaved = True Then
                     If SalesTargetDt.Rows.Count > 0 Then

                         Dim dt As New DataTable
                         dt.Columns.Add("Target_Item_ID", System.Type.GetType("System.Guid"))
                         dt.Columns.Add("Sales_Target_ID", System.Type.GetType("System.Int32"))
                         dt.Columns.Add("Target_Month", System.Type.GetType("System.Int32"))
                         dt.Columns.Add("Classification_1", System.Type.GetType("System.String"))
                         dt.Columns.Add("Target_Value_1", System.Type.GetType("System.Decimal"))
                         dt.Columns.Add("Target_Value_2", System.Type.GetType("System.Decimal"))
                         dt.Columns.Add("Last_Updated_At", System.Type.GetType("System.DateTime"))

                         Dim seldr() As DataRow
                         seldr = SalesTargetDt.Select("SalesrepNumber='" & salesrepdr("SalesrepNumber").ToString & "'")
                            For Each dr As DataRow In SalesTargetDt.Rows
                                If dr("SalesRepNumber").ToString() = salesrepdr("SalesrepNumber").ToString Then
                                    If dr("TargetQty").ToString <> "" Or dr("TargetValue").ToString <> "" Then
                                        Dim drTarget As DataRow
                                        drTarget = dt.NewRow
                                        drTarget(0) = Guid.NewGuid()
                                        drTarget(1) = Sales_Target_ID
                                        drTarget(2) = Month
                                        drTarget(3) = dr("ItemCode").ToString
                                        If dr("TargetQty").ToString = "" Then
                                            drTarget(4) = "0"
                                        Else
                                            drTarget(4) = dr("TargetQty").ToString
                                        End If

                                        If dr("TargetValue").ToString = "" Then
                                            drTarget(5) = "0"
                                        Else
                                            drTarget(5) = dr("TargetValue").ToString
                                        End If
                                        drTarget(6) = Now

                                        dt.Rows.Add(drTarget)
                                    End If
                                End If
                            Next

                         Dim objSQLCmdNew As SqlCommand
                         Dim QueryString As String
                         QueryString = "Delete from TBL_Sales_Target_Items WHERE Sales_Target_ID=@Sales_Target_ID AND Target_Month=@Target_Month"
                         objSQLCmdNew = New SqlCommand(QueryString, objSQLConn)
                         objSQLCmdNew.CommandType = CommandType.Text
                         objSQLCmdNew.Parameters.AddWithValue("@Sales_Target_ID", Sales_Target_ID)
                         objSQLCmdNew.Parameters.AddWithValue("@Target_Month", Month)
                         objSQLCmdNew.Transaction = objSQLtrans
                         objSQLCmdNew.ExecuteNonQuery()


                        Using bulkCopy As SqlBulkCopy = _
                          New SqlBulkCopy(objSQLConn, SqlBulkCopyOptions.Default, objSQLtrans)
                            bulkCopy.DestinationTableName = "dbo.TBL_Sales_Target_Items"
                            bulkCopy.ColumnMappings.Add("Target_Item_ID", "Target_Item_ID")
                            bulkCopy.ColumnMappings.Add("Sales_Target_ID", "Sales_Target_ID")
                            bulkCopy.ColumnMappings.Add("Target_Month", "Target_Month")
                            bulkCopy.ColumnMappings.Add("Classification_1", "Classification_1")
                            bulkCopy.ColumnMappings.Add("Target_Value_1", "Target_Value_1")
                            bulkCopy.ColumnMappings.Add("Target_Value_2", "Target_Value_2")
                            bulkCopy.ColumnMappings.Add("Last_Updated_At", "Last_Updated_At")

                            bulkCopy.WriteToServer(dt)
                            bChilTblSaved = True
                        End Using

                     End If


            End If
            objSQLtrans.Commit()

            objSQLCmd.Dispose()
            objSQLCmd = Nothing
            Catch ex As Exception
                objSQLtrans.Rollback()
            End Try
       Next
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

