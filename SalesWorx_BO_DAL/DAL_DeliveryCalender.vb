Imports System.Configuration
Imports System.Data.SqlClient
Public Class DAL_DeliveryCalender
    Private dtDlvclndr As New DataTable
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Private _objDB As DatabaseConnection
    Private Shared _strSQLConn As String = ConfigurationSettings.AppSettings("SQLConnString")
    Public Function GetDeliveryCalender(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal OPT As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dt_DC_rslt As New DataTable

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_GetDeliveryCalender", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OPT", OPT)
            objSQLDA.Fill(dt_DC_rslt)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "564204"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetDeliveryCalender = dt_DC_rslt
    End Function

    Public Function CheckDC_ExDateExist(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Organization_ID As String, ByVal Delivery_Date As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtIncentive As New DataTable
        Dim Query As String = ""
        Dim bRetVal As Boolean = False
        Try


            Query = "SELECT *  from TBL_Delivery_Calender where Organization_ID='" & Organization_ID.Trim() & "' AND  CAST(delivery_date AS DATE)= '" & Delivery_Date.Trim() & "'"
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter(Query, objSQLConn)
            objSQLDA.Fill(dtIncentive)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "540023"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtIncentive
    End Function


    Public Function ManageDeliveryCalenderDate(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Row_ID As String, ByVal Organization_ID As String, ByVal DeliveryDate As String, ByVal Is_Working As String, ByVal UserID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_ManageDeliveryCalenderDate", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@Row_ID", Row_ID)
            objSQLCmd.Parameters.AddWithValue("@OrgID", Organization_ID)
            objSQLCmd.Parameters.AddWithValue("@DeliveryDate", DeliveryDate)
            objSQLCmd.Parameters.AddWithValue("@Is_Working", Is_Working)
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

    Public Function GetDeliveryCalender_Details(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Row_ID As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dt_DC_rslt As New DataTable

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_GetDeliveryCalender_Details", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Row_ID", Row_ID)
            objSQLDA.Fill(dt_DC_rslt)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "564204"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetDeliveryCalender_Details = dt_DC_rslt
    End Function

    Public Function DeleteDeliveryCalenderDate(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Row_ID As String, ByVal Organization_ID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_DeleteDeliveryCalenderDate", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@Row_ID", Row_ID)
            objSQLCmd.Parameters.AddWithValue("@OrgID", Organization_ID)
            '  objSQLCmd.ExecuteNonQuery()
            Dim RowsAffected As Integer = objSQLCmd.ExecuteNonQuery()
            If RowsAffected > 0 Then
                bRetVal = True
            End If

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


    Public Function ManageDeliveryCalenderDays(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Organization_ID As String, ByVal Daylst As String, ByVal UserID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_ManageDeliveryCalenderDAYS", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@OrgID", Organization_ID)
            objSQLCmd.Parameters.AddWithValue("@Daylst", Daylst)
            objSQLCmd.Parameters.AddWithValue("@UserID", UserID)
            Dim RowsAffected As Integer = objSQLCmd.ExecuteNonQuery()
            If RowsAffected > 0 Then
                bRetVal = True
            Else
                bRetVal = False
            End If

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

    Public Function SerchDeliveryCalender(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal FromDate As String, ByVal ToDate As String, ByVal Is_Working As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dt_DC_rslt As New DataTable

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_SerachDeliveryCalender", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Is_Working", Is_Working)
            objSQLDA.Fill(dt_DC_rslt)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "564204"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        SerchDeliveryCalender = dt_DC_rslt
    End Function
    Public Function IsValidOrganization(ByVal OrgID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "SELECT COUNT(*) FROM  TBL_Org_CTL_DTL WHERE MAS_Org_ID=@OrgID "
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            Dim cnt As Integer = 0
            cnt = Convert.ToInt32(objSQLCmd.ExecuteScalar())
            If cnt > 0 Then
                success = True
            End If
            objSQLCmd.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function


    Public Function ExistsDeliveryCalenderDate(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Organization_ID As String, ByVal DeliveryDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim bRetVal As Boolean = False
        Dim dt_DC_rslt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_ExistsDeliveryCalenderDate", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", Organization_ID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@DeliveryDate", DeliveryDate)

            objSQLDA.Fill(dt_DC_rslt)
            If dt_DC_rslt.Rows.Count > 0 Then
                bRetVal = True
            Else
                bRetVal = False
            End If

            objSQLDA.Dispose()
            objSQLDA = Nothing
        Catch ex As Exception
            Err_No = "90127"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt_DC_rslt
    End Function
End Class
