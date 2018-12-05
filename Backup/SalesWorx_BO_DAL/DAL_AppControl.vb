Imports System.Data.SqlClient
Imports System.Configuration

Public Class DAL_AppControl
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Private _objDB As DatabaseConnection
    Private Shared _strSQLConn As String = ConfigurationSettings.AppSettings("SQLConnString")
    Public Function GetControlParams() As Long
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim lRetVal As Long = 0
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            sQry = "SELECT TOP 1 CAST(Control_Params As BIGINT) FROM TBL_App_Config"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)

            Dim tempDBVal As Object = objSQLCmd.ExecuteScalar()

            If Not IsNothing(tempDBVal) Then
                If Not IsDBNull(tempDBVal) Then
                    lRetVal = CType(tempDBVal, Long)
                End If
            End If
        Catch ex As Exception
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return lRetVal
    End Function
    Public Function GeLoginMode(ByRef Error_No As Long, ByRef Error_Desc As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Dim LoginMode As String = Nothing
        Try
            objSQLConn = _objDB.GetSQLConnection
            '            objSQLCmd = New SqlCommand("SELECT Control_Value FROM TBL_App_Control WHERE Control_Key='ENABLE_WINDOWS_AUTH' AND Custom_Attribute_2='N'", objSQLConn)
            objSQLCmd = New SqlCommand("SELECT Control_Value FROM TBL_App_Control WHERE Control_Key='ENABLE_WINDOWS_AUTH'", objSQLConn)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            If dt.Rows.Count > 0 Then
                LoginMode = dt.Rows(0)(0).ToString()
            End If
            objSQLCmd.Dispose()
        Catch ex As Exception
            Error_No = "34078"
            Error_Desc = ex.Message
            ' log.Error(ex.ToString())
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return LoginMode
    End Function

    Public Function InitialiazeAppControlParams(ByRef Error_No As Long, ByRef Error_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("SELECT Control_Key,Control_Value FROM TBL_App_Control WHERE Custom_Attribute_2='N'", objSQLConn)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Error_No = "34078"
            Error_Desc = ex.Message
            ' log.Error(ex.ToString())
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function




    Public Function CopyLoadAppControlParams(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal ParentNode As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_LoadAppParameters", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@ParamType", ParentNode)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Error_No = "14078"
            Error_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function


    Public Function LoadAppParamType(ByRef Error_No As Long, ByRef Error_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("SELECT DISTINCT Custom_Attribute_1 AS Code,Custom_Attribute_1 AS Description FROM TBL_App_Control WHERE Custom_Attribute_2 ='N' AND Custom_Attribute_1 <>'System' ORDER BY  Custom_Attribute_1 ", objSQLConn)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Error_No = "92674"
            Error_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function









    Public Function LoadAppControlParams(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal ParentNode As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_LoadAppControlsParams", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            ' objSQLCmd.Parameters.AddWithValue("@VersionNo", VersionNo)
            objSQLCmd.Parameters.AddWithValue("@ParentNode", ParentNode)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Error_No = "14078"
            Error_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function LoadParamDropdown(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal ControlKey As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("SELECT Code_Value as Code, Code_Description As Description FROM TBL_App_Codes WHERE Code_Type=@ControlKey Order By Code_Description", objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@ControlKey", ControlKey)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Error_No = "13078"
            Error_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function LoadParentNode(ByRef Error_No As Long, ByRef Error_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("SELECT DISTINCT Custom_Attribute_1  AS ParentID, Custom_Attribute_1 AS Value FROm TBL_App_Control WHERE Custom_Attribute_1 <>'Others' AND  Custom_Attribute_2='N'  ORDER BY  Custom_Attribute_1  DESC", objSQLConn)
       
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Error_No = "14078"
            Error_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function CopyLoadParentNode(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal ParentNode As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            '            objSQLCmd = New SqlCommand("SELECT DISTINCT Custom_Attribute_1  AS ParentID, Custom_Attribute_1 AS Value FROm TBL_App_Control WHERE Custom_Attribute_1 <>'Others' AND  Custom_Attribute_2='N'  ORDER BY  Custom_Attribute_1  DESC", objSQLConn)
            objSQLCmd = New SqlCommand("SELECT DISTINCT Custom_Attribute_1  AS ParentID, Custom_Attribute_1 AS Value FROm TBL_App_Control WHERE Custom_Attribute_1 =@ParentNode AND  Custom_Attribute_2='N'  ORDER BY  Custom_Attribute_1  DESC", objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@ParentNode", ParentNode)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Error_No = "14078"
            Error_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function LoadOtherAppControlParams(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal VersionNo As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_LoadOtherAppControlsParams", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            ' objSQLCmd.Parameters.AddWithValue("@VersionNo", VersionNo)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Error_No = "14078"
            Error_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function UpdateAppParams(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal ModifiedBy As Integer, ByVal ControlKey As String, ByVal ControlValue As String) As Boolean

        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim bRetVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection



            sQry = "UPDATE TBL_App_Control SET Control_Value = @ControlValue ,Last_Updated_At =GETDATE(),Last_Updated_By =@ModifiedBy WHERE Control_Key =@ControlKey"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@ControlValue", ControlValue)
            objSQLCmd.Parameters.AddWithValue("@ControlKey", ControlKey)
            objSQLCmd.Parameters.AddWithValue("@ModifiedBy", ModifiedBy)
            objSQLCmd.ExecuteNonQuery()

            objSQLCmd.Dispose()
            objSQLCmd = Nothing

            bRetVal = True
        Catch ex As Exception
            Error_No = -1
            Error_Desc = ex.ToString()
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function
    Public Function UpdateAppControlParams(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal ModifiedBy As Integer, ByVal ALLOW_EXCESS_CASH_COLLECTION As String, ByVal ALLOW_LOAD_QTY_CHANGE As String, ByVal ALLOW_ORDER_DISCOUNT As String, ByVal ALLOW_PARTIAL_UNLOAD As String, ByVal ALLOW_UNLOAD_QTY_CHANGE As String, ByVal CN_LIMIT_MODE As String, ByVal COLLECTION_MODE As String, ByVal DISCOUNT_MODE As String, ByVal ENABLE_COLLECTIONS As String, ByVal ENABLE_CUSTOMER_SIGNATURE As String, ByVal ENABLE_DISTRIB_CHECK As String, ByVal ENABLE_LOT_SELECTION As String, ByVal ENABLE_MARKET_SURVEY As String, ByVal ENABLE_ORDER_HISTORY As String, ByVal ENABLE_SHORT_DOC_REF As String, ByVal EOD_ON_UNLOAD As String, ByVal MERGE_RETURN_STOCK As String, ByVal OPTIONAL_RETURN_HDR_REASON As String, ByVal OPTIONAL_RETURN_LOT As String, ByVal RETURN_STOCK_MERGE_MODE As String, ByVal UNLOAD_QTY_CHANGE_MODE As String, ByVal VAN_LOAD_TYPE As String, ByVal VAN_UNLOAD_TYPE As String, ByVal DISC_LIMIT_MIN As String, ByVal DISC_LIMIT_MAX As String) As Boolean

        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim bRetVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection



            sQry = "app_UpdateAppConttolsParams"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@ALLOW_EXCESS_CASH_COLLECTION", ALLOW_EXCESS_CASH_COLLECTION)
            objSQLCmd.Parameters.AddWithValue("@ALLOW_LOAD_QTY_CHANGE", ALLOW_LOAD_QTY_CHANGE)
            objSQLCmd.Parameters.AddWithValue("@ALLOW_ORDER_DISCOUNT", ALLOW_ORDER_DISCOUNT)

            objSQLCmd.Parameters.AddWithValue("@ALLOW_PARTIAL_UNLOAD", ALLOW_PARTIAL_UNLOAD)
            objSQLCmd.Parameters.AddWithValue("@ALLOW_UNLOAD_QTY_CHANGE", ALLOW_UNLOAD_QTY_CHANGE)
            objSQLCmd.Parameters.AddWithValue("@CN_LIMIT_MODE", CN_LIMIT_MODE)
            objSQLCmd.Parameters.AddWithValue("@COLLECTION_MODE", COLLECTION_MODE)
            objSQLCmd.Parameters.AddWithValue("@DISCOUNT_MODE", DISCOUNT_MODE)
            objSQLCmd.Parameters.AddWithValue("@ENABLE_COLLECTIONS", ENABLE_COLLECTIONS)
            objSQLCmd.Parameters.AddWithValue("@ENABLE_CUSTOMER_SIGNATURE", ENABLE_CUSTOMER_SIGNATURE)
            objSQLCmd.Parameters.AddWithValue("@ENABLE_DISTRIB_CHECK", ENABLE_DISTRIB_CHECK)
            objSQLCmd.Parameters.AddWithValue("@ENABLE_LOT_SELECTION", ENABLE_LOT_SELECTION)
            objSQLCmd.Parameters.AddWithValue("@ENABLE_MARKET_SURVEY", ENABLE_MARKET_SURVEY)
            objSQLCmd.Parameters.AddWithValue("@ENABLE_ORDER_HISTORY", ENABLE_ORDER_HISTORY)
            objSQLCmd.Parameters.AddWithValue("@ENABLE_SHORT_DOC_REF", ENABLE_SHORT_DOC_REF)
            objSQLCmd.Parameters.AddWithValue("@EOD_ON_UNLOAD", EOD_ON_UNLOAD)
            objSQLCmd.Parameters.AddWithValue("@MERGE_RETURN_STOCK", MERGE_RETURN_STOCK)
            objSQLCmd.Parameters.AddWithValue("@OPTIONAL_RETURN_HDR_REASON", OPTIONAL_RETURN_HDR_REASON)
            objSQLCmd.Parameters.AddWithValue("@OPTIONAL_RETURN_LOT", OPTIONAL_RETURN_LOT)
            objSQLCmd.Parameters.AddWithValue("@RETURN_STOCK_MERGE_MODE", RETURN_STOCK_MERGE_MODE)
            objSQLCmd.Parameters.AddWithValue("@UNLOAD_QTY_CHANGE_MODE", UNLOAD_QTY_CHANGE_MODE)
            objSQLCmd.Parameters.AddWithValue("@VAN_LOAD_TYPE", VAN_LOAD_TYPE)
            objSQLCmd.Parameters.AddWithValue("@VAN_UNLOAD_TYPE", VAN_UNLOAD_TYPE)
            objSQLCmd.Parameters.AddWithValue("@DISC_LIMIT_MIN", DISC_LIMIT_MIN)
            objSQLCmd.Parameters.AddWithValue("@DISC_LIMIT_MAX", DISC_LIMIT_MAX)
            objSQLCmd.Parameters.AddWithValue("@ModifiedBy", ModifiedBy)
            objSQLCmd.ExecuteNonQuery()

            objSQLCmd.Dispose()
            objSQLCmd = Nothing

            bRetVal = True
        Catch ex As Exception
            Error_No = -1
            Error_Desc = ex.ToString()
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function
    Public Function UpdateControlParams(ByVal PointData As String, ByVal SubPointData As String, ByRef Error_No As Long, ByRef Error_Desc As String) As Boolean

        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim bRetVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            If PointData <> "" Then
                If SubPointData <> "" Then
                    PointData = PointData & "," & SubPointData
                End If
            Else
                PointData = SubPointData
            End If

            Dim lControlParams As Long

            If PointData <> "" Then
                Dim arrPoints() As String = Split(PointData, ",")
                For Each cPoint As String In arrPoints
                    If cPoint.Length > 2 Then
                        If IsNumeric(cPoint.Substring(2)) Then
                            lControlParams = lControlParams + Math.Pow(2, CType(cPoint.Substring(2), Long) - 1)
                        End If
                    End If
                Next
            End If

            sQry = String.Format("UPDATE TBL_App_Config SET Control_Params={0}", lControlParams)

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.ExecuteNonQuery()

            objSQLCmd.Dispose()
            objSQLCmd = Nothing

            ' sQry = String.Format("UPDATE " & _BO_SYNC_DB & "..TBL_User SET Control_Params={0}", lControlParams)

            'objSQLCmd = New SqlCommand(sQry, objSQLConn)

            'objSQLCmd.ExecuteNonQuery()

            bRetVal = True
        Catch ex As Exception
            Error_No = -1
            Error_Desc = ex.ToString()
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function

    Public Function LoadOtherParams(ByRef _iRouteSyncDays As Integer, ByRef _iRoutePlanDueDate As Integer, ByRef _iFC As String, ByRef _iCLOD As String, ByRef _iOrdPending As String, ByRef _iRMADays As String, ByRef _iRMALimit As Decimal, ByRef _IOd As String, ByRef _IAutoBlock As String, ByRef _iOverDueLimit As Decimal) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim objSQLDR As SqlDataReader
        Try
            Dim sQry As String = "SELECT TOP 1 Route_Sync_Days, RoutePlan_Due_Date,AutoCheckCL,AutoCheckFC,OrderPendingDays,RMAPendingDays,RMALimit,AutoCheckOD,AutoBlock,OverDueLimit FROM TBL_App_Config"


            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLDR = objSQLCmd.ExecuteReader()

            While objSQLDR.Read()
                _iRouteSyncDays = objSQLDR.GetInt16(0)
                _iRoutePlanDueDate = objSQLDR.GetInt16(1)
                _iCLOD = CStr(IIf(objSQLDR(2) Is DBNull.Value, "N", objSQLDR(2).ToString()))
                _iFC = CStr(IIf(objSQLDR(3) Is DBNull.Value, "N", objSQLDR(3).ToString()))
                _iOrdPending = CStr(IIf(objSQLDR(4) Is DBNull.Value, "10", objSQLDR(4).ToString()))
                _iRMADays = CStr(IIf(objSQLDR(5) Is DBNull.Value, "10", objSQLDR(5).ToString()))
                _iRMALimit = CDec(IIf(objSQLDR(6) Is DBNull.Value, "10000", objSQLDR(6).ToString())).ToString("0.00")
                _IOd = CStr(IIf(objSQLDR(7) Is DBNull.Value, "N", objSQLDR(7).ToString()))
                _IAutoBlock = CStr(IIf(objSQLDR(8) Is DBNull.Value, "N", objSQLDR(8).ToString()))
                _iOverDueLimit = CDec(IIf(objSQLDR(9) Is DBNull.Value, "1000", objSQLDR(9).ToString())).ToString("0.00")
            End While

            objSQLDR.Close()
            objSQLCmd.Dispose()
        Catch ex As Exception
            Throw ex
        Finally
            objSQLDR = Nothing
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function UpdateOtherParams(ByVal _iRouteSyncDays As Integer, ByVal _iRoutePlanDueDate As Integer, ByVal _iFC As String, ByVal _ICLOD As String, ByVal _IOrdPending As String, ByVal _iRMADays As String, ByVal _IRMALimit As Decimal, ByVal _IOd As String, ByVal _IAutoBlock As String, ByVal _IOverDuelimit As Decimal, Optional ByRef SQLConn As SqlConnection = Nothing) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            Dim sQry As String = String.Format("UPDATE TBL_App_Config SET Route_Sync_Days={0}, RoutePlan_Due_Date={1},AutoCheckCL={2},AutoCheckFC={3},OrderPendingDays={4},RMApendingDays={5},RMALimit={6},AutoCheckOd={7},AutoBlock={8},OverDuelimit={9}", _iRouteSyncDays, _iRoutePlanDueDate, "'" & _ICLOD.ToString() & "'", "'" & _iFC.ToString() & "'", "'" & _IOrdPending & "'", "'" & _iRMADays & "'", _IRMALimit, "'" & _IOd & "'", "'" & _IAutoBlock & "'", _IOverDuelimit)

            If IsNothing(SQLConn) Then
                'getting MSSQL DB connection.....
                objSQLConn = _objDB.GetSQLConnection

                objSQLCmd = New SqlCommand(sQry, objSQLConn)
            Else
                objSQLCmd = New SqlCommand(sQry, SQLConn)
            End If

            bRetVal = (objSQLCmd.ExecuteNonQuery() > 0)

            objSQLCmd.Dispose()
        Catch ex As Exception
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function
    Public Function GetRouteSyncDays() As Integer
        'GetRouteSyncDays
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim lRetVal As Long = 0
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            sQry = "SELECT TOP 1 CAST(Route_Sync_Days As BIGINT) FROM TBL_App_Config"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)

            Dim tempDBVal As Object = objSQLCmd.ExecuteScalar()

            If Not IsNothing(tempDBVal) Then
                If Not IsDBNull(tempDBVal) Then
                    lRetVal = CType(tempDBVal, Long)
                End If
            End If
        Catch ex As Exception
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return lRetVal
    End Function
End Class
