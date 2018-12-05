Imports System.Data.SqlClient
Imports System.Configuration

Public Class DAL_RoutePlan
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Private _objDB As DatabaseConnection
    Private Shared _strSQLConn As String = ConfigurationSettings.AppSettings("SQLConnString")
    Private mExpiryDate As Integer = CInt(ConfigurationSettings.AppSettings("MESSAGE_EXPIRY_DAYS"))
    Public Function DeleteDefaultPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Rp_ID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sRetVal As Boolean = True
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("app_DeleteDefaultPlan", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@Rp_ID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@Rp_ID").Value = Rp_ID
            Dim parameter As SqlParameter = objSQLCmd.Parameters.Add(New SqlParameter("@RowsAffected", SqlDbType.Int))
            parameter.Direction = ParameterDirection.Output
            objSQLCmd.ExecuteNonQuery()
            Dim RowsAffected As Integer = parameter.Value

            If (RowsAffected > 0) Then
                sRetVal = True
            Else
                sRetVal = False
            End If
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
        DeleteDefaultPlan = sRetVal
    End Function
    Public Function GetRoutePlanID(ByRef Err_No As Long, ByRef Err_Desc As String) As ArrayList

        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim objSQLDR As SqlDataReader
        Dim sQry As String
        Dim alRoutePlans As New ArrayList

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("select Distinct(Default_Plan_ID) from TBL_Route_Plan_FSR", objSQLConn)
            objSQLDR = objSQLCmd.ExecuteReader()

            While objSQLDR.Read
                alRoutePlans.Add(CStr(objSQLDR.GetValue(0)))
            End While
            objSQLDR.Close()
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74001"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetRoutePlanID = alRoutePlans
    End Function
    Public Function ShowExistingPlan(ByVal Site As String, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim objSQLDR As SqlDataReader
        Dim sQry As String

        Try
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("select Default_Plan_ID, Description, Start_Date, End_Date,Site from TBL_Route_Plan_Default where End_Date >= getdate() and Site='" & Site & "' order by Start_Date ASC", objSQLConn)
            objSQLDR = objSQLCmd.ExecuteReader(CommandBehavior.CloseConnection)

            Dim tempDBVal As Object
            Dim sRPID As String
            Dim sStartDate As Date
            Dim sEndDate As Date
            Dim isAssigned As String
            Dim SiteId As String

            Dim MyDT As New DataTable
            Dim MyRow As DataRow
            MyDT.Columns.Add(New DataColumn("Default_Plan_ID", _
                GetType(Int32)))
            MyDT.Columns.Add(New DataColumn("Details", _
                GetType(String)))


            MyRow = MyDT.NewRow()
            MyRow(0) = 0
            MyRow(1) = "  -- Select an existing plan -- "
            MyDT.Rows.Add(MyRow)

            While objSQLDR.Read
                sRPID = CStr(objSQLDR.GetValue(0))
                tempDBVal = objSQLDR.GetValue(1)
                tempDBVal = IIf(IsDBNull(tempDBVal), "NA", tempDBVal)
                sStartDate = objSQLDR.GetDateTime(2)
                sEndDate = objSQLDR.GetDateTime(3)
                SiteId = CStr(objSQLDR.GetValue(4))

                '                isAssigned = tempDBVal & " (" & sStartDate.Day & "/" & sStartDate.Month & "/" & sStartDate.Year & " - " & sEndDate.Day & "/" & sEndDate.Month & "/" & sEndDate.Year & ") "
                isAssigned = tempDBVal & " [" & SiteId & "]"

                MyRow = MyDT.NewRow()
                MyRow(0) = sRPID
                MyRow(1) = isAssigned
                MyDT.Rows.Add(MyRow)
            End While

            objSQLDR.Close()
            objSQLCmd.Dispose()

            ShowExistingPlan = MyDT
        Catch ex As Exception
            Err_No = "74002"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
    End Function
    Public Function IsPlanUsed(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sPlanID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim sRetVal As Boolean = True
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            sQry = String.Format("select count(1) from TBL_Route_Plan_FSR where Default_Plan_ID={0}", _sPlanID)
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            If CInt(objSQLCmd.ExecuteScalar) > 0 Then
                sRetVal = True
            Else
                sRetVal = False
            End If

            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74004"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        IsPlanUsed = sRetVal
    End Function
    Public Function GetDefaultPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Rp_ID As Integer) As DataRow
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim sRetVal As Boolean = True
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_GetDefaultPlanByID", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@Rp_ID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@Rp_ID").Value = Rp_ID
            Dim RP_Ds As New DDL_TBL_Route_Plan_Default
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(RP_Ds, "DefaultPlan")
            Dim dt As Integer = RP_Ds.Tables.Count()

            GetDefaultPlan = RP_Ds.Tables("DefaultPlan").Rows(0)
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74005"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function UpdateDefaultPlanDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal STime As Date, ByVal ETime As Date, ByVal Day As Integer, ByVal DType As Char, ByVal UComments As String, ByVal CanVisit As Char, ByVal DefPlanDetailID As Integer, ByRef SqlConn As SqlConnection, ByRef objSQLCmd As SqlCommand, ByRef transaction As SqlTransaction) As Boolean
        Dim objSQLConn As SqlConnection
        Dim bRetVal As Boolean = False
        Dim iRowsAffected As Integer = 0
        Try

            objSQLConn = SqlConn
            objSQLCmd = New SqlCommand("app_UpdateDefaultPlanDetails", objSQLConn)
            objSQLCmd.Transaction = transaction
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@DefPlanDetailID", SqlDbType.Char))
            objSQLCmd.Parameters("@DefPlanDetailID").Value = DefPlanDetailID
            objSQLCmd.Parameters.Add(New SqlParameter("@VarDay", SqlDbType.SmallInt))
            objSQLCmd.Parameters("@VarDay").Value = Day
            objSQLCmd.Parameters.Add(New SqlParameter("@VarSTime", SqlDbType.DateTime))
            objSQLCmd.Parameters("@VarSTime").Value = IIf(STime = "1/1/1900", DBNull.Value, STime)
            objSQLCmd.Parameters.Add(New SqlParameter("@VarETime", SqlDbType.DateTime))
            objSQLCmd.Parameters("@VarETime").Value = IIf(ETime = "1/1/1900", DBNull.Value, ETime)
            objSQLCmd.Parameters.Add(New SqlParameter("@VarDayType", SqlDbType.Char))
            objSQLCmd.Parameters("@VarDayType").Value = DType
            objSQLCmd.Parameters.Add(New SqlParameter("@VarUComments", SqlDbType.VarChar, 50))
            objSQLCmd.Parameters("@VarUComments").Value = IIf(UComments = "", DBNull.Value, UComments)
            objSQLCmd.Parameters.Add(New SqlParameter("@VarCanPlanVisit", SqlDbType.Char))
            objSQLCmd.Parameters("@VarCanPlanVisit").Value = CanVisit

            iRowsAffected = objSQLCmd.ExecuteNonQuery()

        Catch ex As Exception
            Err_No = "74012"
            Err_Desc = ex.Message
            transaction.Rollback()
        Finally
        End Try
    End Function
    Public Function GetDefaultCalendarDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RP_ID As Integer) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("app_GetDefaultPlanDetails", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@Rp_ID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@Rp_ID").Value = RP_ID
            Dim DefaultDetailsDs As New DDL_TBL_Route_Plan_Default_Details
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(DefaultDetailsDs, "DefaultPlan")

            GetDefaultCalendarDetails = DefaultDetailsDs
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74007"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetDefaultCalendarDetails_New(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RP_ID As Integer) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("app_GetDefaultPlanDetails", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@Rp_ID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@Rp_ID").Value = RP_ID
            Dim DefaultDetailsDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(DefaultDetailsDs, "DefaultPlan")

            GetDefaultCalendarDetails_New = DefaultDetailsDs
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74007"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function IsValidDateRange(ByVal _dStartDate As Date, ByVal _dEndDate As String, ByVal _sPlanID As Int64, ByVal _sSite As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection

            sQry = String.Format("select count(*) from TBL_Route_Plan_Default where Site='{2}' AND ((Start_Date Between '{0}' AND '{1}') OR (End_Date Between '{0}' AND '{1}') OR (Start_Date='{0}' AND End_Date='{1}') OR (Start_Date<'{0}' AND End_Date>'{1}'))", _dStartDate, _dEndDate, _sSite)
            objSQLCmd = New SqlCommand(sQry, objSQLConn)

            If CInt(objSQLCmd.ExecuteScalar()) > 0 Then
                bRetVal = False
            Else
                bRetVal = True
            End If

            objSQLCmd.Dispose()

        Catch ex As Exception
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        IsValidDateRange = bRetVal
    End Function
    Public Function GetConnection() As SqlConnection
        Dim objSQLConn As SqlConnection
        objSQLConn = _objDB.GetSQLConnection
        Return objSQLConn
    End Function
    Public Sub CloseConnection(ByRef ObjCloseConn As SqlConnection)
        ' Dim objSQLConn As SqlConnection
        If ObjCloseConn IsNot Nothing Then
            _objDB.CloseSQLConnection(ObjCloseConn)
        End If
    End Sub
    Public Function InsertDefaultPlan(ByRef Process_Results As ArrayList, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Desc As String, ByVal SDate As Date, ByVal EDate As Date, ByVal NoOfWorkingDays As Integer, ByVal Site As String) As Integer
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Process_Results = New ArrayList
            objSQLCmd = New SqlCommand("app_InsertDefaultPlan", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@VarDescription", SqlDbType.VarChar, 50))
            objSQLCmd.Parameters("@VarDescription").Value = Desc
            objSQLCmd.Parameters.Add(New SqlParameter("@VarSDate", SqlDbType.DateTime))
            objSQLCmd.Parameters("@VarSDate").Value = SDate
            objSQLCmd.Parameters.Add(New SqlParameter("@VarEDate", SqlDbType.DateTime))
            objSQLCmd.Parameters("@VarEDate").Value = EDate
            objSQLCmd.Parameters.Add(New SqlParameter("@VarWDays", SqlDbType.SmallInt))
            objSQLCmd.Parameters("@VarWDays").Value = NoOfWorkingDays
            objSQLCmd.Parameters.Add(New SqlParameter("@VarSite", SqlDbType.VarChar, 25))
            objSQLCmd.Parameters("@VarSite").Value = Site
            Dim parameter As SqlParameter = objSQLCmd.Parameters.Add(New SqlParameter("@DefaultPlanId", SqlDbType.Int))
            parameter.Direction = ParameterDirection.Output

            Dim PlanID As Integer
            objSQLCmd.ExecuteNonQuery()
            PlanID = parameter.Value
            If PlanID > 0 Then
                InsertDefaultPlan = PlanID
            Else
                Process_Results.Add("No records were inserted.")
            End If

            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        Catch ex As Exception
            Err_No = "74009"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function InsertDefaultPlanDetails(ByRef Process_Results As ArrayList, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal DefPlanID As Integer, ByVal STime As Date, ByVal ETime As Date, ByVal Day As Integer, ByVal DType As Char, ByVal UComments As String, ByVal CanVisit As Char, ByRef SqlConn As SqlConnection, ByRef objSQLCmd As SqlCommand, ByRef transaction As SqlTransaction) As Boolean
        Dim objSQLConn As SqlConnection
        Dim bRetVal As Boolean = False
        Dim iRowsAffected As Integer = 0
        Try
            objSQLConn = SqlConn
            Process_Results = New ArrayList
            objSQLCmd = New SqlCommand("app_InsertDefaultPlanDetails", objSQLConn)
            objSQLCmd.Transaction = transaction
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@VarDefaultPlanID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@VarDefaultPlanID").Value = DefPlanID
            objSQLCmd.Parameters.Add(New SqlParameter("@VarDay", SqlDbType.SmallInt))
            objSQLCmd.Parameters("@VarDay").Value = Day
            objSQLCmd.Parameters.Add(New SqlParameter("@VarSTime", SqlDbType.DateTime))
            objSQLCmd.Parameters("@VarSTime").Value = IIf(STime = "1/1/1900", DBNull.Value, STime)
            objSQLCmd.Parameters.Add(New SqlParameter("@VarETime", SqlDbType.DateTime))
            objSQLCmd.Parameters("@VarETime").Value = IIf(ETime = "1/1/1900", DBNull.Value, ETime)
            objSQLCmd.Parameters.Add(New SqlParameter("@VarDayType", SqlDbType.Char))
            objSQLCmd.Parameters("@VarDayType").Value = DType
            objSQLCmd.Parameters.Add(New SqlParameter("@VarUComments", SqlDbType.VarChar, 50))
            objSQLCmd.Parameters("@VarUComments").Value = IIf(UComments = "", DBNull.Value, UComments)
            objSQLCmd.Parameters.Add(New SqlParameter("@VarCanPlanVisit", SqlDbType.Char))
            objSQLCmd.Parameters("@VarCanPlanVisit").Value = CanVisit

            iRowsAffected = objSQLCmd.ExecuteNonQuery()

        Catch ex As Exception
            Err_No = "74011"
            Err_Desc = ex.Message
            transaction.Rollback()
        Finally
        End Try
    End Function
    Public Function ShowSalesRepsByUD(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UD_SUB_QRY As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            Dim SqlStr As String
            SqlStr = String.Format("select SalesRep_ID, SalesRep_Name from TBL_FSR where SalesRep_ID In ({0}) order by SalesRep_Name ASC", UD_SUB_QRY)

            objSQLCmd = New SqlCommand(SqlStr, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            Dim SalesRepDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(SalesRepDs, "SalesRepList")
            ShowSalesRepsByUD = SalesRepDs.Tables("SalesRepList")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74015"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function ShowDefPlans(ByRef Err_No As Long, ByRef Err_desc As String, ByVal Site As String, ByVal SalesRepId As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("app_GetDefaultPlanBySalesRep", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@Site", SqlDbType.VarChar, 50))
            objSQLCmd.Parameters("@Site").Value = Site
            objSQLCmd.Parameters.Add(New SqlParameter("@SalesRep_ID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@SalesRep_ID").Value = SalesRepId
            Dim DefRouteDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(DefRouteDs, "DefaultPlan")

            ShowDefPlans = DefRouteDs.Tables("DefaultPlan")
            objSQLCmd.Dispose()

            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74016"
            Err_desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
    End Function
    Public Function SetCommentPanelVisibility(ByVal RepID As Int64) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("[app_GetUserIsSS]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@SalesRep_ID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@SalesRep_ID").Value = RepID

            Dim ISSS As Char
            Dim SqlReader As SqlDataReader = objSQLCmd.ExecuteReader()
            If (SqlReader.Read()) Then
                ISSS = SqlReader("IsSS")
            End If
            If (ISSS <> "N") Then
                Return True
            Else
                Return True
            End If
            objSQLCmd.Dispose()
        Catch ex As Exception
            Return False
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    'Public Function GetFSRDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RP_ID As Integer) As DataSet
    '    Dim objSQLConn As SqlConnection
    '    Dim objSQLCmd As SqlCommand
    '    Dim sQry As String

    '    Try
    '        'getting MSSQL DB connection.....
    '        objSQLConn = _objDB.GetSQLConnection

    '        objSQLCmd = New SqlCommand("app_GetFSRPlanDetails", objSQLConn)
    '        objSQLCmd.CommandType = CommandType.StoredProcedure
    '        objSQLCmd.Parameters.Add(New SqlParameter("@Rp_ID", SqlDbType.BigInt))
    '        objSQLCmd.Parameters("@Rp_ID").Value = RP_ID
    '        Dim DefaultDetailsDs As New DDL_TBL_Route_Plan_FSR_Details
    '        Dim SqlAd As SqlDataAdapter
    '        SqlAd = New SqlDataAdapter(objSQLCmd)
    '        SqlAd.Fill(DefaultDetailsDs, "FSRPlan")

    '        GetFSRDetails = DefaultDetailsDs
    '        objSQLCmd.Dispose()

    '    Catch ex As Exception
    '        Err_No = "74019"
    '        Err_Desc = ex.Message
    '        Throw ex
    '    Finally
    '        objSQLCmd = Nothing
    '        If objSQLConn IsNot Nothing Then
    '            _objDB.CloseSQLConnection(objSQLConn)
    '        End If
    '    End Try
    'End Function
    Public Function GetFSRDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RP_ID As Integer) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("app_GetFSRPlanDetails", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@Rp_ID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@Rp_ID").Value = RP_ID
            Dim DefaultDetailsDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(DefaultDetailsDs, "FSRPlan")

            GetFSRDetails = DefaultDetailsDs
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74019"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
    End Function
    Public Function GetCustomerList(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal SalesRepID As Int64) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String

        Try

            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            ''** orginal***   sQry = String.Format("SELECT A.Customer_ID, A.Site_Use_ID, (A.Customer_Name+'-'+ISNULL(A.Location,'N/A')) As Customer_Name, B.Customer_Class, B.Address2, B.City FROM TBL_Customer_Ship_Address AS A INNER JOIN TBL_Customer As B ON A.Customer_ID=B.Customer_ID WHERE (B.Primary_SalesRep_ID='{0}' OR (B.Primary_SalesRep_ID In (SELECT DISTINCT(SalesRep_WS_ID) FROM TBL_Org_CTL_DTL WHERE SalesRep_ID='{0}'))) AND A.Dept IN ({1}) ORDER BY Customer_Name ASC", SalesRepID, SubQuery)
            '  sQry = "SELECT A.Customer_ID, A.Site_Use_ID, (A.Customer_Name+'-'+ISNULL(A.Location,'N/A')) As Customer_Name, B.Customer_Class, B.Address, B.City FROM TBL_Customer_Ship_Address  AS A INNER JOIN TBL_Customer As B ON A.Customer_ID=B.Customer_ID  "
            ' sQry = "SELECT A.Customer_ID, A.Site_Use_ID, (A.Customer_Name+'-'+ISNULL(A.Location,'N/A')) As Customer_Name,B.Customer_Class,A.Address, A.City FROM V_FSR_CustomerShipAddress As A inner join TBL_Customer As B ON A.Customer_ID=B.Customer_ID And A.Site_Use_ID = B.Site_Use_ID where A.SalesRep_Id=" & SalesRepID
            sQry = "SELECT DISTINCT  A.Customer_ID, A.Site_Use_ID, (A.Customer_Name) As Customer_Name,A.Customer_No,B.Customer_Class,A.Address, A.City FROM V_FSR_CustomerShipAddress As A INNER JOIN TBL_Customer_Ship_Address_Map AS Q ON Q.Ship_To_Customer_ID=A.Customer_ID AND Q.Ship_To_Site_ID=A.Site_Use_ID INNER join TBL_Customer As B ON Q.Inv_To_Customer_ID=B.Customer_ID And Q.Inv_To_Site_ID = B.Site_Use_ID  where A.SalesRep_Id=" & SalesRepID & " Order By A.Customer_Name"
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add(New SqlParameter("@SalesRepID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@SalesRepID").Value = SalesRepID
            Dim DefRouteDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(DefRouteDs, "CustomerList")

            GetCustomerList = DefRouteDs.Tables("CustomerList")
            objSQLCmd.Dispose()
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.ExecuteReader(CommandBehavior.CloseConnection)

            objSQLCmd.Dispose()
        Catch ex As Exception
            Error_No = "74020"
            Error_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
    End Function

    Public Function GetCustomerListForRoutePlan(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal SalesRepID As Int64, Optional ByVal Searchfield As String = "", Optional ByVal SearchValue As String = "") As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String

        Try

            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            ''** orginal***   sQry = String.Format("SELECT A.Customer_ID, A.Site_Use_ID, (A.Customer_Name+'-'+ISNULL(A.Location,'N/A')) As Customer_Name, B.Customer_Class, B.Address2, B.City FROM TBL_Customer_Ship_Address AS A INNER JOIN TBL_Customer As B ON A.Customer_ID=B.Customer_ID WHERE (B.Primary_SalesRep_ID='{0}' OR (B.Primary_SalesRep_ID In (SELECT DISTINCT(SalesRep_WS_ID) FROM TBL_Org_CTL_DTL WHERE SalesRep_ID='{0}'))) AND A.Dept IN ({1}) ORDER BY Customer_Name ASC", SalesRepID, SubQuery)
            '  sQry = "SELECT A.Customer_ID, A.Site_Use_ID, (A.Customer_Name+'-'+ISNULL(A.Location,'N/A')) As Customer_Name, B.Customer_Class, B.Address, B.City FROM TBL_Customer_Ship_Address  AS A INNER JOIN TBL_Customer As B ON A.Customer_ID=B.Customer_ID  "
            ' sQry = "SELECT A.Customer_ID, A.Site_Use_ID, (A.Customer_Name+'-'+ISNULL(A.Location,'N/A')) As Customer_Name,B.Customer_Class,A.Address, A.City FROM V_FSR_CustomerShipAddress As A inner join TBL_Customer As B ON A.Customer_ID=B.Customer_ID And A.Site_Use_ID = B.Site_Use_ID where A.SalesRep_Id=" & SalesRepID
            sQry = "SELECT DISTINCT  A.Customer_ID, A.Site_Use_ID, (A.Customer_Name) As Customer_Name,A.Customer_No,B.Customer_Class,A.Address, A.City FROM V_FSR_CustomerShipAddress As A INNER JOIN TBL_Customer_Ship_Address_Map AS Q ON Q.Ship_To_Customer_ID=A.Customer_ID AND Q.Ship_To_Site_ID=A.Site_Use_ID INNER join TBL_Customer As B ON Q.Inv_To_Customer_ID=B.Customer_ID And Q.Inv_To_Site_ID = B.Site_Use_ID  where A.SalesRep_Id=@SalesRepID " & Searchfield & "  Order By A.Customer_Name"
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.CommandText = sQry
            objSQLCmd.Parameters.Add(New SqlParameter("@SalesRepID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@SalesRepID").Value = SalesRepID
            If Searchfield.Trim() <> "" Then
                objSQLCmd.Parameters.Add(New SqlParameter("@SearchVal", SqlDbType.VarChar))
                objSQLCmd.Parameters("@SearchVal").Value = SearchValue
            End If
            Dim DefRouteDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(DefRouteDs, "CustomerList")

            GetCustomerListForRoutePlan = DefRouteDs.Tables("CustomerList")
            objSQLCmd.Dispose()
           
        Catch ex As Exception
            Error_No = "74020"
            Error_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
    End Function

    Public Function GetCommentsByFSR(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal FSRID As Int64) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("[app_GetCommentsByFSR]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@FSRID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@FSRID").Value = FSRID
            Dim CommentsDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(CommentsDs, "FSRComments")
            GetCommentsByFSR = CommentsDs.Tables("FSRComments")
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74020"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetMoveCopyDays(ByRef Err_No As Long, ByRef Err_desc As String, ByVal DefaultPlanID As Integer, ByVal StartDate As Date, ByVal EndDate As Date) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("app_GetMoveCopyDays", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@DefaultPlan_ID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@DefaultPlan_ID").Value = DefaultPlanID
            objSQLCmd.Parameters.Add(New SqlParameter("@StartDay", SqlDbType.Int))
            objSQLCmd.Parameters("@StartDay").Value = StartDate.Day
            objSQLCmd.Parameters.Add(New SqlParameter("@EndDay", SqlDbType.Int))
            objSQLCmd.Parameters("@EndDay").Value = EndDate.Day
            Dim DefRouteDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(DefRouteDs, "DefaultPlanDays")

            GetMoveCopyDays = DefRouteDs.Tables("DefaultPlanDays")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74026"
            Err_desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    Public Function GetClassList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRepID As Int64) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("[app_GetCustomerClassBySalesRepID]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@SalesRepID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@SalesRepID").Value = SalesRepID
            Dim DefRouteDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(DefRouteDs, "ClassList")

            GetClassList = DefRouteDs.Tables("ClassList")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74036"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetCustomerAddress2(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRepID As Int64) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("[app_GetCustomerAddress2BySalesRepID]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@SalesRepID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@SalesRepID").Value = SalesRepID
            Dim DefRouteDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(DefRouteDs, "ClassList")

            GetCustomerAddress2 = DefRouteDs.Tables("ClassList")
            objSQLCmd.Dispose()
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74037"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetCustomerCity(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRepID As Int64) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("[app_GetCustomerCityBySalesRepID]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@SalesRepID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@SalesRepID").Value = SalesRepID
            Dim DefRouteDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(DefRouteDs, "ClassList")

            GetCustomerCity = DefRouteDs.Tables("ClassList")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74038"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function



    Public Function GetCustomerNo(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRepID As Int64) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            'objSQLCmd = New SqlCommand("SELECT DISTINCT CAST(Customer_No AS BIGINT) AS Customer_No FROM TBL_Customer ORDER BY CAST(Customer_No AS BIGINT)", objSQLConn)
            ' objSQLCmd = New SqlCommand("SELECT DISTINCT Customer_No  AS Customer_No FROM TBL_Customer ORDER BY Customer_No", objSQLConn)
            Dim sQry As String = "SELECT DISTINCT  A.Customer_No FROM V_FSR_CustomerShipAddress As A where A.SalesRep_Id=" & SalesRepID & " Order By A.Customer_No"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            Dim DefRouteDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(DefRouteDs, "CustomerNo")

            GetCustomerNo = DefRouteDs.Tables("CustomerNo")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74039"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function





    Public Function InsertApprovalComments(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RP_ID As Int64, ByVal Message As String, ByVal SenderID As Int64, ByRef SqlConn As SqlConnection, ByRef objSQLCmd As SqlCommand, ByRef transaction As SqlTransaction) As Integer
        Dim objSQLConn As SqlConnection
        'Dim objSQLCmd As SqlCommand
        Dim sRetVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = SqlConn
            objSQLCmd = New SqlCommand("app_InsertApprovalComments", objSQLConn)
            objSQLCmd.Transaction = transaction
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@Rp_ID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@Rp_ID").Value = RP_ID
            objSQLCmd.Parameters.Add(New SqlParameter("@Message", SqlDbType.Text))
            objSQLCmd.Parameters("@Message").Value = Message
            objSQLCmd.Parameters.Add(New SqlParameter("@Sender_ID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@Sender_ID").Value = SenderID
            objSQLCmd.Parameters.Add(New SqlParameter("@Msg_Title", SqlDbType.VarChar, 25))
            objSQLCmd.Parameters("@Msg_Title").Value = "Comments"
            objSQLCmd.Parameters.Add(New SqlParameter("@Expiry_Date", SqlDbType.DateTime))
            objSQLCmd.Parameters("@Expiry_Date").Value = IIf(mExpiryDate <> 0, Today.AddDays(mExpiryDate - 1), 0)
            Dim parameter As SqlParameter = objSQLCmd.Parameters.Add(New SqlParameter("@MessageId", SqlDbType.Int))
            parameter.Direction = ParameterDirection.Output

            Dim MessageID As Integer
            objSQLCmd.ExecuteNonQuery()
            MessageID = parameter.Value
            If MessageID > 0 Then
                InsertApprovalComments = MessageID
            Else
                InsertApprovalComments = 0
            End If

        Catch ex As Exception
            Err_No = "74041"
            Err_Desc = ex.Message
            transaction.Rollback()
        Finally
        End Try

    End Function
    Public Function AssignMessage(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal MessageID As Integer, ByVal SalesRepID As Integer, ByVal MessageReadStat As Char, ByRef SqlConn As SqlConnection, ByRef objSQLCmd As SqlCommand, ByRef transaction As SqlTransaction) As Integer
        Dim objSQLConn As SqlConnection
        'Dim objSQLCmd As SqlCommand
        Dim sRetVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = SqlConn
            objSQLCmd = New SqlCommand("app_AssignMessage", objSQLConn)
            objSQLCmd.Transaction = transaction
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@MessageID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@MessageID").Value = MessageID
            objSQLCmd.Parameters.Add(New SqlParameter("@SalesRep_ID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@SalesRep_ID").Value = SalesRepID
            objSQLCmd.Parameters.Add(New SqlParameter("@Read_Stat", SqlDbType.Char))
            objSQLCmd.Parameters("@Read_Stat").Value = MessageReadStat
            Dim parameter As SqlParameter = objSQLCmd.Parameters.Add(New SqlParameter("@RowsAffected", SqlDbType.Int))
            parameter.Direction = ParameterDirection.Output

            objSQLCmd.ExecuteNonQuery()
            Dim RowsAffected As Integer = parameter.Value

            If RowsAffected > 0 Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Err_No = "74042"
            Err_Desc = ex.Message
            transaction.Rollback()
            Return False
        Finally
        End Try

    End Function
    Public Function ApproveRoutePlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ApprovalStat As Char, ByVal ApprovedBy As Int64, ByVal FSR_ID As Int64) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sRetVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("app_ApproveFSRPlan", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@Rp_ID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@Rp_ID").Value = FSR_ID
            objSQLCmd.Parameters.Add(New SqlParameter("@Approved_By", SqlDbType.BigInt))
            objSQLCmd.Parameters("@Approved_By").Value = ApprovedBy
            objSQLCmd.Parameters.Add(New SqlParameter("@App_Stat", SqlDbType.Char))
            objSQLCmd.Parameters("@App_Stat").Value = ApprovalStat
            Dim parameter As SqlParameter = objSQLCmd.Parameters.Add(New SqlParameter("@RowsAffected", SqlDbType.Int))
            parameter.Direction = ParameterDirection.Output

            objSQLCmd.ExecuteNonQuery()
            Dim RowsAffected As Integer = parameter.Value

            If (RowsAffected > 0) Then
                sRetVal = True
            Else
                sRetVal = False
            End If
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74043"
            Err_Desc = ex.Message
            sRetVal = False
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        ApproveRoutePlan = sRetVal
    End Function
    Public Function InsertFSRPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRepID As Integer, ByVal DefaultPlanID As Integer, ByVal AppStat As Char) As Integer
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_InsertFSRPlan", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@SalesRepID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@SalesRepID").Value = SalesRepID
            objSQLCmd.Parameters.Add(New SqlParameter("@Default_Plan_ID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@Default_Plan_ID").Value = DefaultPlanID
            objSQLCmd.Parameters.Add(New SqlParameter("@Approved", SqlDbType.Char))
            objSQLCmd.Parameters("@Approved").Value = AppStat
            Dim parameter As SqlParameter = objSQLCmd.Parameters.Add(New SqlParameter("@FSRPlanId", SqlDbType.Int))
            parameter.Direction = ParameterDirection.Output

            Dim PlanID As Integer
            objSQLCmd.ExecuteNonQuery()
            PlanID = parameter.Value
            If PlanID > 0 Then
                InsertFSRPlan = PlanID
            Else
                InsertFSRPlan = 0
            End If

            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        Catch ex As Exception
            Err_No = "74044"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function InsertFSRPlanDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal FSRPlanID As Integer, ByVal Day As Integer, ByVal CustomerID As Integer, ByVal SiteID As Integer, ByVal STime As Date, ByVal ETime As Date, ByRef SqlConn As SqlConnection, ByRef objSQLCmd As SqlCommand, ByRef transaction As SqlTransaction, ByVal DayType As Char, ByVal UserComm As String, ByVal Sequence As String, ByVal Allow_Optimization As String) As Boolean

        Dim objSQLConn As SqlConnection
        Dim bRetVal As Boolean = False
        Dim iRowsAffected As Integer = 0
        Try

            objSQLConn = SqlConn
            objSQLCmd = New SqlCommand("app_InsertFSRPlanDetails", objSQLConn)
            objSQLCmd.Transaction = transaction
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@FSR_Plan_ID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@FSR_Plan_ID").Value = FSRPlanID
            objSQLCmd.Parameters.Add(New SqlParameter("@VarDay", SqlDbType.SmallInt))
            objSQLCmd.Parameters("@VarDay").Value = Day
            objSQLCmd.Parameters.Add(New SqlParameter("@VarStartTime", SqlDbType.DateTime))
            objSQLCmd.Parameters("@VarStartTime").Value = IIf(STime = "1/1/1900", DBNull.Value, STime)
            objSQLCmd.Parameters.Add(New SqlParameter("@VarEndTime", SqlDbType.DateTime))
            objSQLCmd.Parameters("@VarEndTime").Value = IIf(ETime = "1/1/1900", DBNull.Value, ETime)
            objSQLCmd.Parameters.Add(New SqlParameter("@VarSCustomerID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@VarSCustomerID").Value = IIf(CustomerID = 0, DBNull.Value, CustomerID)
            objSQLCmd.Parameters.Add(New SqlParameter("@VarSiteUseID", SqlDbType.Int))
            objSQLCmd.Parameters("@VarSiteUseID").Value = IIf(SiteID = 0, DBNull.Value, SiteID)
            objSQLCmd.Parameters.Add(New SqlParameter("@VarDay_Type", SqlDbType.Char))
            objSQLCmd.Parameters("@VarDay_Type").Value = DayType
            objSQLCmd.Parameters.Add(New SqlParameter("@VarUserComments", SqlDbType.VarChar, 50))
            objSQLCmd.Parameters("@VarUserComments").Value = IIf(UserComm = "", DBNull.Value, UserComm)
            objSQLCmd.Parameters.Add(New SqlParameter("@Sequence", SqlDbType.Int, 50))
            objSQLCmd.Parameters("@Sequence").Value = Val(Sequence)
            objSQLCmd.Parameters.Add(New SqlParameter("@Allow_Opt", SqlDbType.Char, 1))
            objSQLCmd.Parameters("@Allow_Opt").Value = IIf(Allow_Optimization = "", DBNull.Value, Allow_Optimization)
            iRowsAffected = objSQLCmd.ExecuteNonQuery()

        Catch ex As Exception
            Err_No = "74046"
            Err_Desc = ex.Message
            transaction.Rollback()
        Finally
        End Try
    End Function
    Public Function GetSequence(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal FSRPlanID As Integer, ByVal Day As Integer, ByVal SalesRep_ID As String, ByRef SqlConn As SqlConnection, ByRef objSQLCmd As SqlCommand, ByRef transaction As SqlTransaction) As String

        Dim objSQLConn As SqlConnection
        Dim bRetVal As Boolean = False
        Dim tSequence As Object
        Dim Sqns As String = "0"
        Try

            objSQLConn = SqlConn
            objSQLCmd = New SqlCommand("select isnull(max(isnull(Visit_Sequence,0)),0)+1 as Sequence from TBL_Route_Plan_FSR_Details A inner join TBL_Route_Plan_FSR B on A.FSR_Plan_ID=b.FSR_Plan_ID inner join TBL_Org_CTL_DTL C on b.SalesRep_ID=C.SalesRep_ID INNER JOIN TBL_FSR D ON D.SalesRep_Id=C.SalesRep_Id where A.FSR_Plan_ID=" & FSRPlanID & " and Day=" & Day & " and SalesRep_Number='" & SalesRep_ID & "'", objSQLConn)
            objSQLCmd.Transaction = transaction
            objSQLCmd.CommandType = CommandType.Text
            tSequence = objSQLCmd.ExecuteScalar
            If Not tSequence Is Nothing Then
                Sqns = tSequence.ToString
            End If
        Catch ex As Exception
            Err_No = "74046"
            Err_Desc = ex.Message
        Finally
        End Try
        Return Sqns
    End Function
    Public Function UpdateFSRPLan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RP_ID As Int64, ByVal ApproveStat As Char, ByRef SqlConn As SqlConnection, ByRef objSQLCmd As SqlCommand, ByRef transaction As SqlTransaction) As Boolean
        Dim objSQLConn As SqlConnection
        'Dim objSQLCmd As SqlCommand
        Dim sRetVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = SqlConn

            objSQLCmd = New SqlCommand("app_UpdateFSRPlan", objSQLConn)
            objSQLCmd.Transaction = transaction
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@Rp_ID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@Rp_ID").Value = RP_ID
            objSQLCmd.Parameters.Add(New SqlParameter("@Approved_Stat", SqlDbType.Char))
            objSQLCmd.Parameters("@Approved_Stat").Value = ApproveStat
            Dim parameter As SqlParameter = objSQLCmd.Parameters.Add(New SqlParameter("@RowsAffected", SqlDbType.Int))
            parameter.Direction = ParameterDirection.Output

            objSQLCmd.ExecuteNonQuery()
            Dim RowsAffected As Integer = parameter.Value

            If (RowsAffected > 0) Then
                sRetVal = True
            Else
                sRetVal = False
            End If

        Catch ex As Exception
            Err_No = "74048"
            Err_Desc = ex.Message
            transaction.Rollback()
            sRetVal = False
        Finally
        End Try
        UpdateFSRPLan = sRetVal
    End Function
    Public Function DeleteRoutePlanByFSRID(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Rp_ID As Integer, ByVal Day As Integer, ByRef SQLConn As SqlConnection, ByRef objSQLCmd As SqlCommand, ByRef transaction As SqlTransaction) As Boolean

        Dim objSQLConn As SqlConnection
        Dim sRetVal As Boolean = True
        Try

            objSQLConn = SQLConn
            objSQLCmd = New SqlCommand("app_DeleteRoutePlanByFSRID", objSQLConn)
            objSQLCmd.Transaction = transaction
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@Rp_ID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@Rp_ID").Value = Rp_ID
            objSQLCmd.Parameters.Add(New SqlParameter("@Day", SqlDbType.BigInt))
            objSQLCmd.Parameters("@Day").Value = Day
            Dim parameter As SqlParameter = objSQLCmd.Parameters.Add(New SqlParameter("@RowsAffected", SqlDbType.Int))
            parameter.Direction = ParameterDirection.Output

            objSQLCmd.ExecuteNonQuery()
            Dim RowsAffected As Integer = parameter.Value

            If (RowsAffected > 0) Then
                sRetVal = True
            Else
                sRetVal = False
            End If

        Catch ex As Exception
            Err_No = "74049"
            Err_Desc = ex.Message
            transaction.Rollback()
            sRetVal = False
        Finally
        End Try
        DeleteRoutePlanByFSRID = sRetVal
    End Function

    Public Function DeleteFSRPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Rp_ID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sRetVal As Boolean = True
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("app_DeleteFSRPlan", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@Rp_ID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@Rp_ID").Value = Rp_ID
            Dim parameter As SqlParameter = objSQLCmd.Parameters.Add(New SqlParameter("@RowsAffected", SqlDbType.Int))
            parameter.Direction = ParameterDirection.Output

            objSQLCmd.ExecuteNonQuery()
            Dim RowsAffected As Integer = parameter.Value

            If (RowsAffected > 0) Then
                sRetVal = True
            Else
                sRetVal = False
            End If
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74052"
            Err_Desc = ex.Message
            sRetVal = False
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        DeleteFSRPlan = sRetVal
    End Function
    Public Function ShowFSRPlans(ByRef Err_No As Long, ByRef Err_desc As String, ByVal SalesRepId As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("app_GetFSRPlanBySalesRep", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@SalesRep_ID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@SalesRep_ID").Value = SalesRepId
            Dim DefRouteDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(DefRouteDs, "DefaultPlan")

            ShowFSRPlans = DefRouteDs.Tables("DefaultPlan")
            objSQLCmd.Dispose()

            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74054"
            Err_desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    Public Function ShowPlanListForApprovalByUD(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal SubQuery As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String

        Try

            sQry = String.Format("Select B.FSR_Plan_ID,A.SalesRep_Name,C.Description,C.Start_Date, C.End_Date,C.No_of_Working_Days,B.Default_Plan_ID,B.SalesRep_ID,C.Site from TBL_FSR as A, TBL_Route_Plan_FSR as B,TBL_Route_Plan_Default as C where A.SalesRep_ID = B.SalesRep_ID AND B.Default_Plan_ID = C.Default_Plan_ID AND(B.Approved = 'N' OR B.Approved = 'U')  AND A.SalesRep_ID IN ({0}) ORDER By A.SalesRep_Name ASC, C.Start_Date ASC", SubQuery)
            'sQry = String.Format("Select B.FSR_Plan_ID,A.SalesRep_Name,C.Description,C.Start_Date, C.End_Date,C.No_of_Working_Days,B.Default_Plan_ID,B.SalesRep_ID from TBL_FSR as A, TBL_Route_Plan_FSR as B,TBL_Route_Plan_Default as C where A.SalesRep_ID = B.SalesRep_ID AND B.Default_Plan_ID = C.Default_Plan_ID AND(B.Approved = 'N' OR B.Approved = 'U')ORDER By A.SalesRep_Name ASC, C.Start_Date ASC", SubQuery)
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            Dim AppPlansDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(AppPlansDs, "PlansForApproval")

            ShowPlanListForApprovalByUD = AppPlansDs.Tables("PlansForApproval")
            objSQLCmd.Dispose()
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.ExecuteReader(CommandBehavior.CloseConnection)
            objSQLCmd.Dispose()

        Catch ex As Exception
            Error_No = "74062"
            Error_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetNoOfVisits(ByVal RP_ID As Int64, ByVal StartDate As Date, ByVal EndDate As Date) As Integer
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sRetVal As Integer
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection()
            objSQLCmd = New SqlCommand("app_GetNoOfVisits", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@Rp_ID", SqlDbType.BigInt))
            objSQLCmd.Parameters("@Rp_ID").Value = RP_ID

            Dim NoOfVisit As Integer = objSQLCmd.ExecuteScalar()
            If (NoOfVisit > 0) Then
                sRetVal = NoOfVisit
            Else
                sRetVal = 0
            End If
            objSQLCmd.Dispose()

        Catch ex As Exception
            sRetVal = 0
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        GetNoOfVisits = sRetVal
    End Function
    Public Function ShowCopyFromPlans(ByRef Err_No As Long, ByRef Err_desc As String, ByVal SalesRepId As Integer, ByVal Site As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            '            Dim sQry As String = String.Format("Select B.FSR_Plan_ID,A.Description,A.Start_Date,A.End_Date,B.Approved,A.Default_Plan_ID from TBL_Route_Plan_Default as A, TBL_Route_Plan_FSR as B where A.Default_Plan_ID=B.Default_Plan_ID AND B.SalesRep_ID={0} AND Site='{1}' and A.End_Date > getdate() order by A.Start_Date ASC ", SalesRepId, Site)
            Dim sQry As String = String.Format("Select B.FSR_Plan_ID,A.Description,A.Start_Date,A.End_Date,B.Approved,A.Default_Plan_ID,A.Site from TBL_Route_Plan_Default as A, TBL_Route_Plan_FSR as B where A.Default_Plan_ID=B.Default_Plan_ID AND B.SalesRep_ID={0} AND Site='{1}'  order by A.Start_Date ASC ", SalesRepId, Site)
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            Dim DefRouteDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(DefRouteDs, "FSRPlan")
            ShowCopyFromPlans = DefRouteDs.Tables("FSRPlan")
            objSQLCmd.Dispose()

            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74069"
            Err_desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    Public Function ShowCopyToPlans(ByRef Err_No As Long, ByRef Err_desc As String, ByVal SalesRepId As Integer, ByVal Site As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            'Dim sQry As String = String.Format("Select Default_Plan_ID,Description,Start_Date,End_Date,No_Of_Working_Days from TBL_Route_Plan_Default where Site='{1}' AND Default_Plan_ID Not In (select Default_Plan_ID from TBL_Route_Plan_FSR where SalesRep_ID='{0}' AND (Approved='Y' OR Approved='U')) and End_Date > getdate() order by Start_Date ASC", SalesRepId, Site)
            Dim sQry As String = String.Format("Select Default_Plan_ID,Description,Start_Date,End_Date,No_Of_Working_Days,Site from TBL_Route_Plan_Default where Site='{1}' AND Default_Plan_ID Not In (select Default_Plan_ID from TBL_Route_Plan_FSR where SalesRep_ID='{0}' AND (Approved='Y')) and (End_Date > getdate() OR Start_Date > getDate()) order by Start_Date ASC", SalesRepId, Site)
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            Dim DefRouteDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(DefRouteDs, "DefaultPlan")
            ShowCopyToPlans = DefRouteDs.Tables("DefaultPlan")
            objSQLCmd.Dispose()
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74072"
            Err_desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    Public Function CopyFSRPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CopyFromDefID As Int64, ByVal CopyFromFSRID As Int64, ByVal CopyToDefID As Int64, ByVal SalesRep_ID As Int64) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sRetVal As Boolean = False
        Dim transaction As SqlTransaction
        Dim FSRDS As New DDL_TBL_Route_Plan_Default_Details
        Dim FSRPlanDS As New DDL_TBL_Route_Plan_Default
        Dim SqlAd As SqlDataAdapter
        Dim FromStartdate As Date
        Dim ToStartDate As Date
        Dim ToEndDate As Date

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection()
            Dim TempCmd As SqlCommand
            Dim TempCon As SqlConnection
            TempCon = _objDB.GetSQLConnection()


            ' TempCmd = New SqlCommand("SELECT * FROM TBL_Route_Plan_FSR_Details where FSR_Plan_ID=" & CopyFromFSRID, TempCon)
            TempCmd = New SqlCommand("SELECT   DISTINCT   A.FSR_Plan_Detail_ID, A.FSR_Plan_ID, A.Day, A.Customer_ID, A.Site_Use_ID, A.Start_Time, A.End_Time, A.Day_Type, A.User_Comments,A.Visit_Sequence,A.Allow_Optimization FROM         TBL_Route_Plan_FSR_Details AS A INNER JOIN   V_FSR_CustomerShipAddress AS B ON A.Customer_ID = B.Customer_ID AND A.Site_Use_ID = B.Site_Use_ID where B.SalesRep_ID IN(SELECT Salesrep_ID FROM TBL_Route_Plan_FSR WHERE FSR_Plan_ID=A.Fsr_Plan_ID) AND  A.FSR_Plan_ID=" & CopyFromFSRID, TempCon)
            TempCmd.CommandType = CommandType.Text
            SqlAd = New SqlDataAdapter(TempCmd)
            FSRDS = New DDL_TBL_Route_Plan_Default_Details
            SqlAd.Fill(FSRDS, "FSRTblDetails")
            TempCmd.Dispose()
            SqlAd.Dispose()


            TempCmd = New SqlCommand("SELECT FSR_PLAN_ID FROM TBL_Route_Plan_FSR WHERE Default_Plan_ID=" & CopyToDefID & " and SalesRep_ID=" & SalesRep_ID, TempCon)
            FSRPlanDS = New DDL_TBL_Route_Plan_Default
            SqlAd = New SqlDataAdapter(TempCmd)
            SqlAd.Fill(FSRPlanDS, "FSRTBL")
            TempCmd.Dispose()
            SqlAd.Dispose()

            TempCmd = New SqlCommand("SELECT Start_Date FROM TBL_Route_Plan_Default where Default_Plan_ID=" & CopyFromDefID, TempCon)
            TempCmd.CommandType = CommandType.Text
            Dim Dr As SqlDataReader
            Dr = TempCmd.ExecuteReader()
            If Dr.Read() Then
                FromStartdate = Dr("Start_Date")
            End If
            Dr.Close()
            TempCmd.Dispose()

            TempCmd = New SqlCommand("SELECT Start_Date,End_Date FROM TBL_Route_Plan_Default where Default_Plan_ID=" & CopyToDefID, TempCon)
            TempCmd.CommandType = CommandType.Text
            Dr = TempCmd.ExecuteReader()
            If Dr.Read() Then
                ToStartDate = Dr("Start_Date")
                ToEndDate = Dr("End_Date")
            End If
            Dr.Close()
            TempCmd.Dispose()

            Dim CopyTbl As New DataTable
            Dim MyRow As DataRow
            CopyTbl.Columns.Add(New DataColumn("FSR_Plan_ID", _
                GetType(Int32)))
            CopyTbl.Columns.Add(New DataColumn("Day", _
                GetType(String)))
            CopyTbl.Columns.Add(New DataColumn("Customer_ID", _
                GetType(Int32)))
            CopyTbl.Columns.Add(New DataColumn("Site_Use_ID", _
               GetType(Int32)))
            CopyTbl.Columns.Add(New DataColumn("Start_Time", _
               GetType(Date)))
            CopyTbl.Columns.Add(New DataColumn("End_Time", _
               GetType(Date)))
            CopyTbl.Columns.Add(New DataColumn("Day_Type", _
              GetType(Char)))
            CopyTbl.Columns.Add(New DataColumn("User_Comments", _
           GetType(String)))
            CopyTbl.Columns.Add(New DataColumn("Visit_Sequence", _
                GetType(Int32)))

            CopyTbl.Columns.Add(New DataColumn("Allow_Optimization", _
             GetType(Char)))
            If (FSRPlanDS.Tables("FSRTBL").Rows.Count() = 0) Then
                objSQLCmd = New SqlCommand("app_InsertFSRPlan", TempCon)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.Parameters.Add(New SqlParameter("@SalesRepID", SqlDbType.BigInt))
                objSQLCmd.Parameters("@SalesRepID").Value = SalesRep_ID
                objSQLCmd.Parameters.Add(New SqlParameter("@Default_Plan_ID", SqlDbType.BigInt))
                objSQLCmd.Parameters("@Default_Plan_ID").Value = CopyToDefID
                objSQLCmd.Parameters.Add(New SqlParameter("@Approved", SqlDbType.Char))
                objSQLCmd.Parameters("@Approved").Value = "N"
                Dim parameter As SqlParameter = objSQLCmd.Parameters.Add(New SqlParameter("@FSRPlanId", SqlDbType.Int))
                parameter.Direction = ParameterDirection.Output

                Dim PlanID As Integer
                objSQLCmd.ExecuteNonQuery()
                PlanID = parameter.Value
                Dim dayoff As String = ""
                dayoff = GetOffDays()

                For Each DaR As DataRow In FSRDS.Tables("FSRTblDetails").Rows

                    If (CInt(ToEndDate.Day) >= DaR("Day")) Then
                        Dim TempToDate As Date
                        TempToDate = Convert.ToDateTime(ToStartDate.Month & "/" & DaR("Day") & "/" & ToStartDate.Year)

                        Dim tempDBVal As Object
                        MyRow = CopyTbl.NewRow()
                        MyRow(0) = PlanID
                        MyRow(1) = DaR("Day")
                        MyRow(2) = DaR("Customer_ID")
                        MyRow(3) = DaR("Site_Use_ID")
                        tempDBVal = DaR("Start_Time")
                        If Not tempDBVal Is DBNull.Value Then
                            Dim Dat As Date
                            Dat = DaR("Start_Time")
                            tempDBVal = Convert.ToDateTime(ToStartDate.Month & "/" & DaR("Day") & "/" & ToStartDate.Year & " " & Dat.ToShortTimeString())
                        End If
                        tempDBVal = IIf(IsDBNull(tempDBVal), DBNull.Value, tempDBVal)
                        MyRow(4) = tempDBVal
                        tempDBVal = DaR("End_Time")
                        If Not tempDBVal Is DBNull.Value Then
                            Dim Dat As Date
                            Dat = DaR("End_Time")
                            tempDBVal = Convert.ToDateTime(ToStartDate.Month & "/" & DaR("Day") & "/" & ToStartDate.Year & " " & Dat.ToShortTimeString())
                        End If
                        tempDBVal = IIf(IsDBNull(tempDBVal), DBNull.Value, tempDBVal)
                        MyRow(5) = tempDBVal
                        tempDBVal = DaR("Day_Type")
                        tempDBVal = IIf(IsDBNull(tempDBVal), DBNull.Value, tempDBVal)
                        MyRow(6) = tempDBVal 'Convert.ToDateTime("1/1/2001") '
                        tempDBVal = DaR("User_Comments")
                        tempDBVal = IIf(IsDBNull(tempDBVal), DBNull.Value, tempDBVal)
                        MyRow(7) = tempDBVal 'Convert.ToDateTime("1/1/2001

                        tempDBVal = DaR("Visit_Sequence")
                        tempDBVal = IIf(IsDBNull(tempDBVal), DBNull.Value, tempDBVal)
                        MyRow(8) = tempDBVal 'Convert.ToDateTime("1/1/2001


                        tempDBVal = DaR("Allow_Optimization")
                        tempDBVal = IIf(IsDBNull(tempDBVal), DBNull.Value, tempDBVal)
                        MyRow(9) = tempDBVal 'Convert.ToDateTime("1/1/2001

                        If DaR("Day_Type") = "V" Then
                            If Not IsOffDay(TempToDate, dayoff) Then
                                CopyTbl.Rows.Add(MyRow)
                            End If
                        End If

                    End If
                Next

                Dim TargetBulkOp As SqlBulkCopy
                TargetBulkOp = New SqlBulkCopy(_strSQLConn, _
                 SqlBulkCopyOptions.UseInternalTransaction)
                TargetBulkOp.DestinationTableName = "TBL_Route_Plan_FSR_Details"
                TargetBulkOp.ColumnMappings.Add("FSR_Plan_ID", "FSR_Plan_ID")
                TargetBulkOp.ColumnMappings.Add("Day", "Day")
                TargetBulkOp.ColumnMappings.Add("Customer_ID", "Customer_ID")
                TargetBulkOp.ColumnMappings.Add("Site_Use_ID", "Site_Use_ID")
                TargetBulkOp.ColumnMappings.Add("Start_Time", "Start_Time")
                TargetBulkOp.ColumnMappings.Add("End_Time", "End_Time")
                TargetBulkOp.ColumnMappings.Add("Day_Type", "Day_Type")
                TargetBulkOp.ColumnMappings.Add("Visit_Sequence", "Visit_Sequence")
                TargetBulkOp.ColumnMappings.Add("Allow_Optimization", "Allow_Optimization")
                TargetBulkOp.BulkCopyTimeout = 500000000
                TargetBulkOp.WriteToServer(CopyTbl)
                objSQLCmd.Dispose()

            Else
                objSQLCmd = New SqlCommand("DELETE FROM TBL_Route_Plan_FSR_Details WHERE FSR_PLAN_ID=" & FSRPlanDS.Tables("FSRTBL").Rows(0).Item(0), objSQLConn)
                objSQLCmd.ExecuteNonQuery()
                objSQLCmd.Dispose()
                Dim dayoff As String = ""
                dayoff = GetOffDays()
                For Each DaR As DataRow In FSRDS.Tables("FSRTblDetails").Rows

                    If (CInt(ToEndDate.Day) >= DaR("Day")) Then
                        Dim TempToDate As Date
                        TempToDate = Convert.ToDateTime(ToStartDate.Month & "/" & DaR("Day") & "/" & ToStartDate.Year)

                        Dim tempDBVal As Object
                        MyRow = CopyTbl.NewRow()
                        MyRow(0) = FSRPlanDS.Tables("FSRTBL").Rows(0).Item(0)
                        MyRow(1) = DaR("Day")
                        MyRow(2) = DaR("Customer_ID")
                        MyRow(3) = DaR("Site_Use_ID")
                        tempDBVal = DaR("Start_Time")
                        If Not tempDBVal Is DBNull.Value Then
                            Dim Dat As Date
                            Dat = DaR("Start_Time")
                            tempDBVal = Convert.ToDateTime(ToStartDate.Month & "/" & DaR("Day") & "/" & ToStartDate.Year & " " & Dat.ToShortTimeString())
                        End If
                        tempDBVal = IIf(IsDBNull(tempDBVal), DBNull.Value, tempDBVal)
                        MyRow(4) = tempDBVal
                        tempDBVal = DaR("End_Time")
                        If Not tempDBVal Is DBNull.Value Then
                            Dim Dat As Date
                            Dat = DaR("End_Time")
                            tempDBVal = Convert.ToDateTime(ToStartDate.Month & "/" & DaR("Day") & "/" & ToStartDate.Year & " " & Dat.ToShortTimeString())
                        End If
                        tempDBVal = IIf(IsDBNull(tempDBVal), DBNull.Value, tempDBVal)
                        MyRow(5) = tempDBVal
                        tempDBVal = DaR("Day_Type")
                        tempDBVal = IIf(IsDBNull(tempDBVal), DBNull.Value, tempDBVal)
                        MyRow(6) = tempDBVal 'Convert.ToDateTime("1/1/2001") '
                        tempDBVal = DaR("User_Comments")
                        tempDBVal = IIf(IsDBNull(tempDBVal), DBNull.Value, tempDBVal)
                        MyRow(7) = tempDBVal 'Convert.ToDateTime("1/1/2001

                        tempDBVal = DaR("Visit_Sequence")
                        tempDBVal = IIf(IsDBNull(tempDBVal), DBNull.Value, tempDBVal)
                        MyRow(8) = tempDBVal 'Convert.ToDateTime("1/1/2001


                        tempDBVal = DaR("Allow_Optimization")
                        tempDBVal = IIf(IsDBNull(tempDBVal), DBNull.Value, tempDBVal)
                        MyRow(9) = tempDBVal 'Convert.ToDateTime("1/1/2001

                        If DaR("Day_Type") = "V" Then
                            If Not IsOffDay(TempToDate, dayoff) Then
                                CopyTbl.Rows.Add(MyRow)
                            End If
                        End If

                    End If
                Next

                Dim TargetBulkOp As SqlBulkCopy
                TargetBulkOp = New SqlBulkCopy(_strSQLConn, _
                 SqlBulkCopyOptions.UseInternalTransaction)
                TargetBulkOp.DestinationTableName = "TBL_Route_Plan_FSR_Details"
                TargetBulkOp.ColumnMappings.Add("FSR_Plan_ID", "FSR_Plan_ID")
                TargetBulkOp.ColumnMappings.Add("Day", "Day")
                TargetBulkOp.ColumnMappings.Add("Customer_ID", "Customer_ID")
                TargetBulkOp.ColumnMappings.Add("Site_Use_ID", "Site_Use_ID")
                TargetBulkOp.ColumnMappings.Add("Start_Time", "Start_Time")
                TargetBulkOp.ColumnMappings.Add("End_Time", "End_Time")
                TargetBulkOp.ColumnMappings.Add("Day_Type", "Day_Type")
                TargetBulkOp.ColumnMappings.Add("Visit_Sequence", "Visit_Sequence")
                TargetBulkOp.ColumnMappings.Add("Allow_Optimization", "Allow_Optimization")
                TargetBulkOp.BulkCopyTimeout = 500000000
                TargetBulkOp.WriteToServer(CopyTbl)
                objSQLCmd.Dispose()
            End If
            _objDB.CloseSQLConnection(TempCon)
            Return True
            CopyTbl = Nothing
        Catch ex As Exception
            Err_No = "74076"
            Err_Desc = ex.Message
            Return False
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function CopyFSRPlanByWeekDays(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CopyFromDefID As Int64, ByVal CopyFromFSRID As Int64, ByVal CopyToDefID As Int64, ByVal SalesRep_ID As Int64) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sRetVal As Boolean = False
        Dim transaction As SqlTransaction
        Dim FSRDS As New DDL_TBL_Route_Plan_Default_Details
        Dim FSRPlanDS As New DDL_TBL_Route_Plan_Default
        Dim SqlAd As SqlDataAdapter
        Dim FromStartdate As Date
        Dim ToStartDate As Date
        Dim ToEndDate As Date

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection()

            Dim TempCmd As SqlCommand
            Dim TempCon As SqlConnection
            TempCon = _objDB.GetSQLConnection()

            'TempCmd = New SqlCommand("SELECT * FROM TBL_Route_Plan_FSR_Details where FSR_Plan_ID=" & CopyFromFSRID, TempCon)
            TempCmd = New SqlCommand("SELECT   DISTINCT  A.FSR_Plan_Detail_ID, A.FSR_Plan_ID, A.Day, A.Customer_ID, A.Site_Use_ID, A.Start_Time, A.End_Time, A.Day_Type, A.User_Comments,A.Visit_Sequence,A.Allow_Optimization FROM         TBL_Route_Plan_FSR_Details AS A INNER JOIN   V_FSR_CustomerShipAddress AS B ON A.Customer_ID = B.Customer_ID AND A.Site_Use_ID = B.Site_Use_ID where B.SalesRep_ID IN(SELECT Salesrep_ID FROM TBL_Route_Plan_FSR WHERE FSR_Plan_ID=A.Fsr_Plan_ID) AND A.FSR_Plan_ID=" & CopyFromFSRID, TempCon)

            TempCmd.CommandType = CommandType.Text
            SqlAd = New SqlDataAdapter(TempCmd)
            FSRDS = New DDL_TBL_Route_Plan_Default_Details
            SqlAd.Fill(FSRDS, "FSRTblDetails")
            TempCmd.Dispose()
            SqlAd.Dispose()


            TempCmd = New SqlCommand("SELECT FSR_PLAN_ID FROM TBL_Route_Plan_FSR WHERE Default_Plan_ID=" & CopyToDefID & " and SalesRep_ID=" & SalesRep_ID, TempCon)
            FSRPlanDS = New DDL_TBL_Route_Plan_Default
            SqlAd = New SqlDataAdapter(TempCmd)
            SqlAd.Fill(FSRPlanDS, "FSRTBL")
            TempCmd.Dispose()
            SqlAd.Dispose()


            TempCmd = New SqlCommand("SELECT Start_Date FROM TBL_Route_Plan_Default where Default_Plan_ID=" & CopyFromDefID, TempCon)
            TempCmd.CommandType = CommandType.Text
            Dim Dr As SqlDataReader
            Dr = TempCmd.ExecuteReader()
            If Dr.Read() Then
                FromStartdate = Dr("Start_Date")
            End If
            Dr.Close()
            TempCmd.Dispose()

            TempCmd = New SqlCommand("SELECT Start_Date,End_Date FROM TBL_Route_Plan_Default where Default_Plan_ID=" & CopyToDefID, TempCon)
            TempCmd.CommandType = CommandType.Text
            Dr = TempCmd.ExecuteReader()
            If Dr.Read() Then
                ToStartDate = Dr("Start_Date")
                ToEndDate = Dr("End_Date")
            End If
            Dr.Close()
            TempCmd.Dispose()

            Dim CopyTbl As New DataTable
            Dim MyRow As DataRow
            CopyTbl.Columns.Add(New DataColumn("FSR_Plan_ID", _
                GetType(Int32)))
            CopyTbl.Columns.Add(New DataColumn("Day", _
                GetType(String)))
            CopyTbl.Columns.Add(New DataColumn("Customer_ID", _
                GetType(Int32)))
            CopyTbl.Columns.Add(New DataColumn("Site_Use_ID", _
               GetType(Int32)))
            CopyTbl.Columns.Add(New DataColumn("Start_Time", _
               GetType(Date)))
            CopyTbl.Columns.Add(New DataColumn("End_Time", _
               GetType(Date)))
            CopyTbl.Columns.Add(New DataColumn("Day_Type", _
              GetType(Char)))
            CopyTbl.Columns.Add(New DataColumn("User_Comments", _
           GetType(String)))

            CopyTbl.Columns.Add(New DataColumn("Visit_Sequence", _
                GetType(Int32)))

            CopyTbl.Columns.Add(New DataColumn("Allow_Optimization", _
             GetType(Char)))
            If (FSRPlanDS.Tables("FSRTBL").Rows.Count() = 0) Then
                objSQLCmd = New SqlCommand("app_InsertFSRPlan", TempCon)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.Parameters.Add(New SqlParameter("@SalesRepID", SqlDbType.BigInt))
                objSQLCmd.Parameters("@SalesRepID").Value = SalesRep_ID
                objSQLCmd.Parameters.Add(New SqlParameter("@Default_Plan_ID", SqlDbType.BigInt))
                objSQLCmd.Parameters("@Default_Plan_ID").Value = CopyToDefID
                objSQLCmd.Parameters.Add(New SqlParameter("@Approved", SqlDbType.Char))
                objSQLCmd.Parameters("@Approved").Value = "N"
                Dim parameter As SqlParameter = objSQLCmd.Parameters.Add(New SqlParameter("@FSRPlanId", SqlDbType.Int))
                parameter.Direction = ParameterDirection.Output

                Dim PlanID As Integer
                objSQLCmd.ExecuteNonQuery()
                PlanID = parameter.Value

                For Each DaR As DataRow In FSRDS.Tables("FSRTblDetails").Rows


                    Dim TempDate As Date
                    Dim Week_Of_the_Year As Integer
                    Dim WeekDay As System.DayOfWeek
                    TempDate = Convert.ToDateTime(FromStartdate.Month & "/" & DaR("Day") & "/" & FromStartdate.Year)
                    WeekDay = TempDate.DayOfWeek
                    Week_Of_the_Year = DateDiff(DateInterval.WeekOfYear, FromStartdate, TempDate)
                    Dim dayoff As String = ""
                    dayoff = GetOffDays()
                    For i = 1 To CInt(ToEndDate.Day)
                        Dim TempToDate As Date
                        TempToDate = Convert.ToDateTime(ToStartDate.Month & "/" & i & "/" & ToStartDate.Year)
                        Dim To_Week_Of_the_Year As Integer
                        Dim To_WeekDay As System.DayOfWeek
                        To_WeekDay = TempToDate.DayOfWeek
                        To_Week_Of_the_Year = DateDiff(DateInterval.WeekOfYear, ToStartDate, TempToDate)

                        If WeekDay = To_WeekDay And Week_Of_the_Year = To_Week_Of_the_Year Then
                            Dim tempDBVal As Object
                            MyRow = CopyTbl.NewRow()
                            MyRow(0) = PlanID
                            MyRow(1) = i
                            MyRow(2) = DaR("Customer_ID")
                            MyRow(3) = DaR("Site_Use_ID")
                            tempDBVal = DaR("Start_Time")
                            If Not tempDBVal Is DBNull.Value Then
                                Dim Dat As Date
                                Dat = DaR("Start_Time")
                                tempDBVal = Convert.ToDateTime(ToStartDate.Month & "/" & i & "/" & ToStartDate.Year & " " & Dat.ToShortTimeString())
                            End If
                            tempDBVal = IIf(IsDBNull(tempDBVal), DBNull.Value, tempDBVal)
                            MyRow(4) = tempDBVal
                            tempDBVal = DaR("End_Time")
                            If Not tempDBVal Is DBNull.Value Then
                                Dim Dat As Date
                                Dat = DaR("End_Time")
                                tempDBVal = Convert.ToDateTime(ToStartDate.Month & "/" & i & "/" & ToStartDate.Year & " " & Dat.ToShortTimeString())
                            End If
                            tempDBVal = IIf(IsDBNull(tempDBVal), DBNull.Value, tempDBVal)
                            MyRow(5) = tempDBVal
                            tempDBVal = DaR("Day_Type")
                            tempDBVal = IIf(IsDBNull(tempDBVal), DBNull.Value, tempDBVal)
                            MyRow(6) = tempDBVal 'Convert.ToDateTime("1/1/2001") '
                            tempDBVal = DaR("User_Comments")
                            tempDBVal = IIf(IsDBNull(tempDBVal), DBNull.Value, tempDBVal)
                            MyRow(7) = tempDBVal 'Convert.ToDateTime("1/1/2001

                            tempDBVal = DaR("Visit_Sequence")
                            tempDBVal = IIf(IsDBNull(tempDBVal), DBNull.Value, tempDBVal)
                            MyRow(8) = tempDBVal 'Convert.ToDateTime("1/1/2001


                            tempDBVal = DaR("Allow_Optimization")
                            tempDBVal = IIf(IsDBNull(tempDBVal), DBNull.Value, tempDBVal)
                            MyRow(9) = tempDBVal 'Convert.ToDateTime("1/1/2001

                            If DaR("Day_Type") = "V" Then
                                If Not IsOffDay(TempToDate, dayoff) Then
                                    CopyTbl.Rows.Add(MyRow)
                                End If
                            End If
                            Exit For
                        End If

                    Next
                Next

                Dim TargetBulkOp As SqlBulkCopy
                TargetBulkOp = New SqlBulkCopy(_strSQLConn, _
                 SqlBulkCopyOptions.UseInternalTransaction)
                TargetBulkOp.DestinationTableName = "TBL_Route_Plan_FSR_Details"
                TargetBulkOp.ColumnMappings.Add("FSR_Plan_ID", "FSR_Plan_ID")
                TargetBulkOp.ColumnMappings.Add("Day", "Day")
                TargetBulkOp.ColumnMappings.Add("Customer_ID", "Customer_ID")
                TargetBulkOp.ColumnMappings.Add("Site_Use_ID", "Site_Use_ID")
                TargetBulkOp.ColumnMappings.Add("Start_Time", "Start_Time")
                TargetBulkOp.ColumnMappings.Add("End_Time", "End_Time")
                TargetBulkOp.ColumnMappings.Add("Day_Type", "Day_Type")
                TargetBulkOp.ColumnMappings.Add("User_Comments", "User_Comments")
                TargetBulkOp.ColumnMappings.Add("Visit_Sequence", "Visit_Sequence")
                TargetBulkOp.ColumnMappings.Add("Allow_Optimization", "Allow_Optimization")
                TargetBulkOp.BulkCopyTimeout = 500000000
                TargetBulkOp.WriteToServer(CopyTbl)
                objSQLCmd.Dispose()

            Else
                objSQLCmd = New SqlCommand("DELETE FROM TBL_Route_Plan_FSR_Details WHERE FSR_PLAN_ID=" & FSRPlanDS.Tables("FSRTBL").Rows(0).Item(0), objSQLConn)
                objSQLCmd.ExecuteNonQuery()
                objSQLCmd.Dispose()
                Dim dayoff As String = ""
                dayoff = GetOffDays()
                For Each DaR As DataRow In FSRDS.Tables("FSRTblDetails").Rows


                    Dim TempDate As Date
                    Dim Week_Of_the_Year As Integer
                    Dim WeekDay As System.DayOfWeek
                    TempDate = Convert.ToDateTime(FromStartdate.Month & "/" & DaR("Day") & "/" & FromStartdate.Year)
                    WeekDay = TempDate.DayOfWeek
                    Week_Of_the_Year = DateDiff(DateInterval.WeekOfYear, FromStartdate, TempDate)

                    For i = 1 To CInt(ToEndDate.Day)
                        Dim TempToDate As Date
                        TempToDate = Convert.ToDateTime(ToStartDate.Month & "/" & i & "/" & ToStartDate.Year)
                        Dim To_Week_Of_the_Year As Integer
                        Dim To_WeekDay As System.DayOfWeek
                        To_WeekDay = TempToDate.DayOfWeek
                        To_Week_Of_the_Year = DateDiff(DateInterval.WeekOfYear, ToStartDate, TempToDate)

                        If WeekDay = To_WeekDay And Week_Of_the_Year = To_Week_Of_the_Year Then
                            Dim tempDBVal As Object
                            MyRow = CopyTbl.NewRow()
                            MyRow(0) = FSRPlanDS.Tables("FSRTBL").Rows(0).Item(0)
                            MyRow(1) = i
                            MyRow(2) = DaR("Customer_ID")
                            MyRow(3) = DaR("Site_Use_ID")
                            tempDBVal = DaR("Start_Time")
                            If Not tempDBVal Is DBNull.Value Then
                                Dim Dat As Date
                                Dat = DaR("Start_Time")
                                tempDBVal = Convert.ToDateTime(ToStartDate.Month & "/" & i & "/" & ToStartDate.Year & " " & Dat.ToShortTimeString())
                            End If
                            tempDBVal = IIf(IsDBNull(tempDBVal), DBNull.Value, tempDBVal)
                            MyRow(4) = tempDBVal
                            tempDBVal = DaR("End_Time")
                            If Not tempDBVal Is DBNull.Value Then
                                Dim Dat As Date
                                Dat = DaR("End_Time")
                                tempDBVal = Convert.ToDateTime(ToStartDate.Month & "/" & i & "/" & ToStartDate.Year & " " & Dat.ToShortTimeString())
                            End If
                            tempDBVal = IIf(IsDBNull(tempDBVal), DBNull.Value, tempDBVal)
                            MyRow(5) = tempDBVal
                            tempDBVal = DaR("Day_Type")
                            tempDBVal = IIf(IsDBNull(tempDBVal), DBNull.Value, tempDBVal)
                            MyRow(6) = tempDBVal 'Convert.ToDateTime("1/1/2001") '
                            tempDBVal = DaR("User_Comments")
                            tempDBVal = IIf(IsDBNull(tempDBVal), DBNull.Value, tempDBVal)
                            MyRow(7) = tempDBVal 'Convert.ToDateTime("1/1/2001

                            tempDBVal = DaR("Visit_Sequence")
                            tempDBVal = IIf(IsDBNull(tempDBVal), DBNull.Value, tempDBVal)
                            MyRow(8) = tempDBVal 'Convert.ToDateTime("1/1/2001


                            tempDBVal = DaR("Allow_Optimization")
                            tempDBVal = IIf(IsDBNull(tempDBVal), DBNull.Value, tempDBVal)
                            MyRow(9) = tempDBVal 'Convert.ToDateTime("1/1/2001


                            If DaR("Day_Type") = "V" Then
                                If Not IsOffDay(TempToDate, dayoff) Then
                                    CopyTbl.Rows.Add(MyRow)
                                End If
                            End If
                            Exit For
                        End If

                    Next
                Next

                Dim TargetBulkOp As SqlBulkCopy
                TargetBulkOp = New SqlBulkCopy(_strSQLConn, SqlBulkCopyOptions.KeepIdentity And SqlBulkCopyOptions.UseInternalTransaction And SqlBulkCopyOptions.KeepNulls)
                TargetBulkOp.DestinationTableName = "TBL_Route_Plan_FSR_Details"
                TargetBulkOp.ColumnMappings.Add("FSR_Plan_ID", "FSR_Plan_ID")
                TargetBulkOp.ColumnMappings.Add("Day", "Day")
                TargetBulkOp.ColumnMappings.Add("Customer_ID", "Customer_ID")
                TargetBulkOp.ColumnMappings.Add("Site_Use_ID", "Site_Use_ID")
                TargetBulkOp.ColumnMappings.Add("Start_Time", "Start_Time")
                TargetBulkOp.ColumnMappings.Add("End_Time", "End_Time")
                TargetBulkOp.ColumnMappings.Add("Day_Type", "Day_Type")
                TargetBulkOp.ColumnMappings.Add("User_Comments", "User_Comments")
                TargetBulkOp.ColumnMappings.Add("Visit_Sequence", "Visit_Sequence")
                TargetBulkOp.ColumnMappings.Add("Allow_Optimization", "Allow_Optimization")
                TargetBulkOp.BulkCopyTimeout = 500000000
                TargetBulkOp.WriteToServer(CopyTbl)
                objSQLCmd.Dispose()
            End If
            _objDB.CloseSQLConnection(TempCon)
            CopyTbl = Nothing
            Return True
        Catch ex As Exception
            Err_No = "74078"
            Err_Desc = ex.Message
            Return False
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetOffDays() As String
        Dim con As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Try
            Dim dayoffs As String = ""
            con = _objDB.GetSQLConnection

            Dim objSQLDA As SqlDataAdapter
            Dim dtOrders As New DataTable

            Dim Sql As String
            Sql = "Select * from TBL_App_Control where Control_Key='ROUTEPLAN_DAY_OFF'"

            objSQLDA = New SqlDataAdapter(Sql, con)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.Fill(dtOrders)
            If dtOrders.Rows.Count > 0 Then
                dayoffs = dtOrders.Rows(0)("Control_Value").ToString
            End If
            Return dayoffs
            dtOrders = Nothing
        Catch ex As Exception
        Finally
            _objDB.CloseSQLConnection(con)
        End Try

    End Function
    Function IsOffDay(ByVal PDate As Date, ByVal OffDays As String) As Boolean
        Dim bRetval As Boolean = False
        Try
            Dim days() As String
            days = OffDays.Split(",")
            For i As Integer = 0 To days.Length - 1
                If PDate.ToString("ddd").ToUpper = days(i).ToUpper Then
                    bRetval = True
                    Exit For
                End If
            Next
        Catch ex As Exception

        End Try
        Return bRetval
    End Function
    Public Function CheckValidFSR(ByVal SalesRep As String, ByVal UserID As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim RetVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            Dim sQry As String = "Select COUNT(*) from app_GetControlInfo(1)AS A INNER JOIN TBL_FSR AS B ON A.SalesRep_ID =B.SalesRep_ID  WHERE B.SalesRep_Number =@SalesRepNo"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.Add("@SalesRepNo", SqlDbType.VarChar, 50).Value = SalesRep
            objSQLCmd.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID
            objSQLCmd.CommandType = CommandType.Text
            Dim DefFSRDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(DefFSRDs, "FSR")
            If DefFSRDs IsNot Nothing Then
                If DefFSRDs.Tables(0).Rows.Count > 0 Then
                    RetVal = True
                End If
            End If
            Return RetVal
        Catch ex As Exception
            Err_No = "74085"
            Err_Desc = ex.Message
            Return False
        Finally
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function DefaultPlanExist(ByVal Dat As DateTime, ByVal SalesRep As String, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim RetVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            'Dim sQry As String = String.Format("Select Default_Plan_ID,Description,Start_Date,End_Date,No_Of_Working_Days from TBL_Route_Plan_Default where Site='{1}' AND Default_Plan_ID Not In (select Default_Plan_ID from TBL_Route_Plan_FSR where SalesRep_ID='{0}' AND (Approved='Y' OR Approved='U')) and End_Date > getdate() order by Start_Date ASC", SalesRepId, Site)
            Dim sQry As String = "app_CheckDefPlanExist"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.Add("@Dat", SqlDbType.DateTime).Value = Dat
            objSQLCmd.Parameters.Add("@SalesRepNo", SqlDbType.VarChar, 50).Value = SalesRep
            objSQLCmd.CommandType = CommandType.StoredProcedure
            Dim DefRouteDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(DefRouteDs, "DefaultPlan")
            If DefRouteDs IsNot Nothing Then
                If DefRouteDs.Tables(0).Rows.Count > 0 Then
                    RetVal = True
                End If
            End If
            Return RetVal
        Catch ex As Exception
            Err_No = "74081"
            Err_Desc = ex.Message
            Return False
        Finally
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetCustomerId(ByRef transaction As SqlTransaction, ByRef SqlConn As SqlConnection, ByRef objSQLCmd As SqlCommand, ByVal CustNo As String, ByVal SiteID As String, ByRef Err_No As Long, ByRef Err_Desc As String) As String
        Dim objSQLConn As SqlConnection
        Dim CustId As String = Nothing
        Try
            objSQLConn = SqlConn
            'objSQLCmd = New SqlCommand("select DISTINCT Customer_ID from TBL_Customer where Customer_No=@CustNo ", objSQLConn)
            objSQLCmd = New SqlCommand("select DISTINCT Customer_ID from TBL_Customer_Ship_Address where Customer_No=@CustNo AND Site_Use_ID=@SiteID ", objSQLConn)
            objSQLCmd.Transaction = transaction
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@CustNo", SqlDbType.VarChar).Value = CustNo
            objSQLCmd.Parameters.Add("@SiteID", SqlDbType.VarChar).Value = SiteID

            CustId = Convert.ToString(objSQLCmd.ExecuteScalar())
        Catch ex As Exception
            Err_No = "74099"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        End Try
        Return CustId
    End Function

    Public Function CheckVanNCustomer(ByVal VanID As String, ByVal CustomerID As String, ByVal SiteID As String, ByRef Err_No As Long, ByRef Err_Desc As String) As Integer
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim RetVal As String = Nothing
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            'Dim sQry As String = String.Format("Select Default_Plan_ID,Description,Start_Date,End_Date,No_Of_Working_Days from TBL_Route_Plan_Default where Site='{1}' AND Default_Plan_ID Not In (select Default_Plan_ID from TBL_Route_Plan_FSR where SalesRep_ID='{0}' AND (Approved='Y' OR Approved='U')) and End_Date > getdate() order by Start_Date ASC", SalesRepId, Site)
            Dim sQry As String = "app_CheckVanNCustomer"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            '            objSQLCmd.Parameters.Add("@VanID", SqlDbType.Int).Value = CInt(VanID)
            objSQLCmd.Parameters.Add("@VanID", SqlDbType.VarChar, 50).Value = VanID
            objSQLCmd.Parameters.Add("@CustomerID", SqlDbType.VarChar, 50).Value = CustomerID
            objSQLCmd.Parameters.Add("@SiteID", SqlDbType.VarChar, 50).Value = SiteID
            objSQLCmd.CommandType = CommandType.StoredProcedure
            Dim DefRouteDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(DefRouteDs, "DefaultPlan")
            If DefRouteDs IsNot Nothing Then
                If DefRouteDs.Tables(0).Rows.Count > 0 Then
                    RetVal = DefRouteDs.Tables(0).Rows(0).Item(1)
                End If
            End If
            Return RetVal
        Catch ex As Exception
            Err_No = "74082"
            Err_Desc = ex.Message & " VanID= " & VanID & "  CustomerID= " & CustomerID
            Return 0
        Finally
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function RoutePlanExist(ByVal Dat As DateTime, ByVal SalesRep As String, ByRef Err_No As Long, ByRef Err_Desc As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim RetVal As String = ""
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            Dim sQry As String = "app_CheckRoutePlanExist"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.Add("@Dat", SqlDbType.DateTime).Value = Dat
            'objSQLCmd.Parameters.Add("@SalesRep", SqlDbType.Int).Value = CInt(SalesRep)
            objSQLCmd.Parameters.Add("@SalesRepNo", SqlDbType.VarChar, 50).Value = SalesRep
            objSQLCmd.CommandType = CommandType.StoredProcedure
            Dim parameter As SqlParameter = objSQLCmd.Parameters.Add(New SqlParameter("@IsExist", SqlDbType.VarChar, 50))
            parameter.Direction = ParameterDirection.Output
            objSQLCmd.ExecuteNonQuery()
            RetVal = parameter.Value

            Return RetVal
        Catch ex As Exception
            Err_No = "74081"
            Err_Desc = ex.Message
            Return ""
        Finally
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetFSRPlanID(ByRef transaction As SqlTransaction, ByRef SqlConn As SqlConnection, ByRef objSQLCmd As SqlCommand, ByVal DefPlanID As Integer, ByVal SalesRep As String, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserId As String) As String
        Dim objSQLConn As SqlConnection
        Dim RetVal As Integer = 0
        Try

            objSQLConn = SqlConn
            objSQLCmd = New SqlCommand("app_GetDefPlanID", objSQLConn)
            objSQLCmd.Transaction = transaction
            objSQLCmd.Parameters.Add("@DefPlanID", SqlDbType.Int).Value = DefPlanID
            '            objSQLCmd.Parameters.Add("@SalesRep", SqlDbType.Int).Value = CInt(SalesRep)
            objSQLCmd.Parameters.Add("@SalesRepNo", SqlDbType.VarChar).Value = SalesRep
            objSQLCmd.Parameters.Add("@UserId", SqlDbType.VarChar).Value = UserId
            objSQLCmd.CommandType = CommandType.StoredProcedure
            Dim parameter As SqlParameter = objSQLCmd.Parameters.Add(New SqlParameter("@ID", SqlDbType.Int))
            parameter.Direction = ParameterDirection.Output
            objSQLCmd.ExecuteNonQuery()
            RetVal = parameter.Value

            Return RetVal
        Catch ex As Exception
            Err_No = "74086"
            Err_Desc = ex.Message
            Return RetVal
        Finally
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        End Try
    End Function

    Public Function GetDistinctSite(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserId As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim SiteTbl As New DataTable
        Dim objSQLCmd As SqlCommand
        Try

            objSQLConn = _objDB.GetSQLConnection
            'objSQLCmd = New SqlCommand("Select distinct(site) from app_GetControlInfo(" & UserId & " )", objSQLConn)
            'objSQLCmd = New SqlCommand("SELECT DISTINCT B.Site,A.Description FROM dbo.TBL_Org_CTL_H AS A LEFT OUTER JOIN app_GetControlInfo(" & UserId & ") AS B ON A.ORG_HE_ID=Mas_Org_ID ORDER BY A.Description ", objSQLConn)
            '            objSQLCmd = New SqlCommand("SELECT DISTINCT B.Site,A.Description FROM dbo.TBL_Org_CTL_H AS A INNER JOIN app_GetControlInfo(" & UserId & ") AS B ON A.ORG_HE_ID=Mas_Org_ID ORDER BY A.Description ", objSQLConn)
            objSQLCmd = New SqlCommand("SELECT DISTINCT  B.Site,A.Description FROM dbo.TBL_Org_CTL_H AS A INNER JOIN app_GetControlInfo(" & UserId & ") AS B ON A.ORG_HE_ID=Mas_Org_ID ORDER BY A.Description ", objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            Dim SqlAd As New SqlDataAdapter(objSQLCmd)
            Dim Ds As New DataSet
            SqlAd.Fill(Ds)
            If Ds IsNot Nothing Then
                Return Ds.Tables(0)
            End If
        Catch ex As Exception
            Err_No = "74091"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function


    Public Function CanVisitCustomer(ByRef transaction As SqlTransaction, ByRef SqlConn As SqlConnection, ByRef objSQLCmd As SqlCommand, ByVal DefPlanID As Integer, ByVal VisitDat As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As String
        Dim objSQLConn As SqlConnection
        Dim DayType As String = Nothing
        Try
            objSQLConn = SqlConn
            objSQLCmd = New SqlCommand("SELECT Day_type  FROM TBL_route_Plan_default_Details WHERE Day=@VisitDat AND Default_Plan_Id=@DefPlanId ", objSQLConn)
            objSQLCmd.Transaction = transaction
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@DefPlanID", SqlDbType.Int).Value = DefPlanID
            objSQLCmd.Parameters.Add("@VisitDat", SqlDbType.Int).Value = VisitDat
            DayType = Convert.ToString(objSQLCmd.ExecuteScalar())
        Catch ex As Exception
            Err_No = "74099"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        End Try
        Return DayType
    End Function



    Public Function CheckVisitExist(ByRef transaction As SqlTransaction, ByRef SqlConn As SqlConnection, ByRef objSQLCmd As SqlCommand, ByVal FSRPlanID As Integer, ByVal CustId As Integer, ByVal SiteID As Integer, ByVal VisitDat As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim Cnt As Integer
        Dim success As Boolean = False
        Try
            objSQLConn = SqlConn
            objSQLCmd = New SqlCommand("SELECT COUNT(*)  FROM TBL_route_Plan_FSR_Details WHERE Day=@VisitDat AND FSR_Plan_Id=@FSRPlanId AND Customer_Id=@CustID AND Site_Use_ID=@SiteID ", objSQLConn)
            objSQLCmd.Transaction = transaction
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@FSRPlanID", SqlDbType.Int).Value = FSRPlanID
            objSQLCmd.Parameters.Add("@CustID", SqlDbType.Int).Value = CustId
            objSQLCmd.Parameters.Add("@SiteID", SqlDbType.Int).Value = SiteID
            objSQLCmd.Parameters.Add("@VisitDat", SqlDbType.Int).Value = VisitDat
            Cnt = Convert.ToInt32(objSQLCmd.ExecuteScalar())
            If Cnt > 0 Then
                success = True
            End If
        Catch ex As Exception
            Err_No = "74011"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        End Try
        Return success
    End Function



    Public Function CheckPlanID(ByVal DefPlanID As Integer, ByVal SalesRep As String, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim SiteTbl As New DataTable
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Dim i As Integer = 0
        Try

            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("SELECT COUNT(*) FROM tbl_Route_plan_FSR  WHERE Default_plan_id=@DefPlanID AND SalesRep_ID=@SalesRepNo", objSQLConn)

            objSQLCmd.Parameters.Add("@DefPlanID", SqlDbType.Int).Value = DefPlanID
            objSQLCmd.Parameters.Add("@SalesRepNo", SqlDbType.VarChar).Value = SalesRep
            i = Convert.ToInt32(objSQLCmd.ExecuteScalar())

            If i > 0 Then
                success = True
            End If

        Catch ex As Exception
            Err_No = "74086"
            Err_Desc = ex.Message
        Finally
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
Public Function GetCustomerVisitMonth(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            Dim SqlStr As String
            SqlStr = "SELECT monthYear,monthYearV FROM (Select DISTINCT CONVERT(VARCHAR, DATEPART(year,Visit_Start_Date))+ '-' + DATENAME(month,Visit_Start_Date)  monthYear, CONVERT(VARCHAR, DATEPART(year,Visit_Start_Date))+ '-' + CONVERT(VARCHAR, month(Visit_Start_Date))  monthYearv,CONVERT(varchar(6),Visit_Start_Date, 112) orderCol FROM TBL_FSR_Actual_Visits A Inner Join TBL_Org_CTL_DTL B On A.SalesRep_ID=B.SalesRep_ID WHERE B.MAS_Org_ID='" & OrgID & "') A ORDER BY CONVERT(INT,orderCol) desc"

            objSQLCmd = New SqlCommand(SqlStr, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            Dim SalesRepDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(SalesRepDs, "SalesRepList")
            GetCustomerVisitMonth = SalesRepDs.Tables("SalesRepList")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74015"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function





End Class
