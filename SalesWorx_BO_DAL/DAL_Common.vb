Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Data
Imports System.Text
Public Class DAL_Common
    Private _objDB As DatabaseConnection
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Public Function GetAppControl(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Key As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("SELECT * from TBL_App_Control where Control_Key='" & Key & "'")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustomerTbl")

            GetAppControl = MsgDs.Tables("CustomerTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
     Public Function SaveDiscountRule(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal Org As String, ByVal Min As String, ByVal max As String, ByVal Value1 As String, ByVal CreatedBy As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "DELETE FROM TBL_Custom_Info WHERE Info_key=@Org AND Info_Type='ORG_ORDER_DISC_STD';INSERT INTO TBL_Custom_Info (Info_Type ,Info_key ,Value_1,Value_5,Value_6,Last_Updated_At,Last_Updated_By)VALUES('ORG_ORDER_DISC_STD',@Org,@Value_1,@Line_5,@Line_6,GetDate(),@CreatedBy)"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@Line_5", Val(Min) / 100)
            objSQLCmd.Parameters.AddWithValue("@Line_6", Val(max) / 100)
            objSQLCmd.Parameters.AddWithValue("@Org", Org)
            objSQLCmd.Parameters.AddWithValue("@Value_1", Value1)
            objSQLCmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()

            sucess = True
        Catch ex As Exception
            Error_No = 75014

        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return sucess

    End Function
    Public Function SavePrintHeader(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal Client As String, ByVal Line1 As String, ByVal Line2 As String, ByVal Line3 As String, ByVal CreatedBy As Integer, ByVal Logo As String, ByVal Type As String, ByVal Mode As String, ByVal Image4inch As String, ByVal Line7 As String, ByVal Line8 As String, ByVal Line9 As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "DELETE FROM TBL_Custom_Info WHERE Info_key=@Client AND Info_Type='" & Type & "';INSERT INTO TBL_Custom_Info (Info_Type ,Info_key ,Value_1,Value_5,Value_4,Value_3,Last_Updated_At,Last_Updated_By,Value_6,Value_7,Value_8,Value_9)VALUES('" & Type & "',@Client,@Value_1,@Line_5,@Line_4,@Line_3,GetDate(),@CreatedBy,@Value_6,@Line_7,@Line_8,@Line_9)"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)

            If Mode = "T" Then
                Logo = "N/A"
            End If
            If Mode = "I" Then
                objSQLCmd.Parameters.AddWithValue("@Line_5", DBNull.Value)
                objSQLCmd.Parameters.AddWithValue("@Line_4", DBNull.Value)
                objSQLCmd.Parameters.AddWithValue("@Line_3", DBNull.Value)
            Else
                objSQLCmd.Parameters.AddWithValue("@Line_5", Line1)
                objSQLCmd.Parameters.AddWithValue("@Line_4", Line2)
                objSQLCmd.Parameters.AddWithValue("@Line_3", Line3)
            End If
            objSQLCmd.Parameters.AddWithValue("@Client", Client)
            objSQLCmd.Parameters.AddWithValue("@Value_1", Logo)
            objSQLCmd.Parameters.AddWithValue("@Value_6", Image4inch)
            objSQLCmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)


            objSQLCmd.Parameters.AddWithValue("@Line_7", Line7)
            objSQLCmd.Parameters.AddWithValue("@Line_8", Line8)
            objSQLCmd.Parameters.AddWithValue("@Line_9", Line9)



            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()

            sucess = True
        Catch ex As Exception
            Error_No = 75014

        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return sucess

    End Function
    Public Function SaveCommonPrintHeader(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal Client As String, ByVal Line1 As String, ByVal Line2 As String, ByVal Line3 As String, ByVal CreatedBy As Integer, ByVal Logo As String, ByVal Type As String, ByVal Mode As String, ByVal Image4inch As String, value2 As String, value7 As String, value8 As String, value9 As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "DELETE FROM TBL_Custom_Info WHERE Info_key=@Client AND Info_Type='" & Type & "';INSERT INTO TBL_Custom_Info (Info_Type ,Info_key ,Value_1,Value_5,Value_4,Value_3,Last_Updated_At,Last_Updated_By,Value_6,Value_2,Value_7,Value_8,Value_9)VALUES('" & Type & "',@Client,@Value_1,@Line_5,@Line_4,@Line_3,GetDate(),@CreatedBy,@Value_6,@Value_2,@Value_7,@Value_8,@Value_9)"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)

            If Mode = "T" Then
                Logo = "N/A"
            End If
            If Mode = "I" Then
                objSQLCmd.Parameters.AddWithValue("@Line_5", DBNull.Value)
                objSQLCmd.Parameters.AddWithValue("@Line_4", DBNull.Value)
                objSQLCmd.Parameters.AddWithValue("@Line_3", DBNull.Value)
            Else
                objSQLCmd.Parameters.AddWithValue("@Line_5", Line1)
                objSQLCmd.Parameters.AddWithValue("@Line_4", Line2)
                objSQLCmd.Parameters.AddWithValue("@Line_3", Line3)
            End If
            objSQLCmd.Parameters.AddWithValue("@Client", Client)
            objSQLCmd.Parameters.AddWithValue("@Value_1", Logo)
            objSQLCmd.Parameters.AddWithValue("@Value_6", Image4inch)
            objSQLCmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
            objSQLCmd.Parameters.AddWithValue("@Value_2", value2)

            objSQLCmd.Parameters.AddWithValue("@Value_7", value7)
            objSQLCmd.Parameters.AddWithValue("@Value_8", value8)
            objSQLCmd.Parameters.AddWithValue("@Value_9", value9)






            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()

            sucess = True
        Catch ex As Exception
            Error_No = 75014

        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return sucess

    End Function
    Public Function SaveClientLogo(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal Client As String, ByVal Line1 As String, ByVal Line2 As String, ByVal Line3 As String, ByVal CreatedBy As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "DELETE FROM TBL_Custom_Info WHERE Info_key=@Client AND Info_Type='CLIENT_LOGO';INSERT INTO TBL_Custom_Info (Info_Type ,Info_key ,Value_1,Value_5,Value_4,Value_3,Last_Updated_At,Last_Updated_By)VALUES('CLIENT_LOGO',@Client,'N/A',@Line_5,@Line_4,@Line_3,GetDate(),@CreatedBy)"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@Client", Client)
            objSQLCmd.Parameters.AddWithValue("@Line_5", Line1)
            objSQLCmd.Parameters.AddWithValue("@Line_4", Line2)
            objSQLCmd.Parameters.AddWithValue("@Line_3", Line3)
            objSQLCmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()




            sucess = True
        Catch ex As Exception
            Error_No = 75014

        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return sucess

    End Function
    Public Function SaveDefaultCL(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal CL As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "app_UpdateDefaultCreditLimit"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@CL", CL)
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()




            sucess = True
        Catch ex As Exception
            Error_No = 75014

        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return sucess

    End Function


    Public Function SaveorgLogo(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal OrgId As String, ByVal LogoPath As String, ByVal CreatedBy As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "DELETE FROM TBL_Custom_Info WHERE Info_key=@OrgID AND Info_Type='ORG_LOGO';INSERT INTO TBL_Custom_Info (Info_Type ,Info_key ,Value_1,Last_Updated_At,Last_Updated_By)VALUES('ORG_LOGO',@OrgId,@LogoPath,GetDate(),@CreatedBy)"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@OrgId", OrgId)
            objSQLCmd.Parameters.AddWithValue("@LogoPath", LogoPath)
            objSQLCmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()




            sucess = True
        Catch ex As Exception
            Error_No = 75014

        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return sucess

    End Function

    Public Function DeleteCommonVanLogo(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RowId As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "DELETE FROM TBL_Custom_Info WHERE Info_Type='VAN_LOGO' and Value_2=@Row_id"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@Row_id", RowId)
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()




            sucess = True
        Catch ex As Exception
            Err_No = 75014

        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return sucess
    End Function
    Public Function DeleteClientLogo(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RowId As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "DELETE FROM TBL_Custom_Info WHERE Row_id=@Row_id"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@Row_id", RowId)
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()




            sucess = True
        Catch ex As Exception
            Err_No = 75014

        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return sucess
    End Function
    Public Function DeleteCustomInfo(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RowId As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "DELETE FROM TBL_Custom_Info WHERE Row_id=@Row_id"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@Row_id", RowId)
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()




            sucess = True
        Catch ex As Exception
            Err_No = 75014

        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return sucess
    End Function

    Public Function GetClientLogo(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RowId As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT *  from TBL_Custom_Info  WHERE Row_id='" & RowId & "'")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetDiscountRule(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT A.Row_ID,A.Info_Key,B.Description,cast(isnull(A.Value_5,0) as decimal(18,2))*100 as Value_5,cast(isnull(A.Value_6,0) as decimal(18,2))*100 as Value_6,cast(isnull(A.Value_1,0) as decimal(18,2)) as Value_1  from TBL_Custom_Info A inner join TBL_Org_CTL_H B on A.Info_Key=B.ORG_HE_ID WHERE  Info_Type='ORG_ORDER_DISC_STD'")

            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetClientLogos(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal client As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT *  from TBL_Custom_Info  WHERE  Info_Type='CLIENT_LOGO'")
            If client.Trim <> "" Then
                QueryString = QueryString & "  and Info_key like '%" & client & "%'"
            End If
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetPrintHeaders(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Type As String, ByVal filter As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_GetInvoicePrintHeaders", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@Type", Type)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Filter", filter)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetOrgsWthNoDiscount(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT Distinct Description,ORG_HE_ID from TBL_Org_CTL_H where ORG_HE_ID not in(select Info_key from TBL_Custom_Info  WHERE  Info_Type='ORG_ORDER_DISC_STD') and isnull(Description,'')<>'' order by Description")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetOrgsWthNoLogos(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT Distinct Description,ORG_HE_ID from TBL_Org_CTL_H where ORG_HE_ID not in(select Info_key from TBL_Custom_Info  WHERE  Info_Type='ORG_LOGO') and isnull(Description,'')<>'' order by Description")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetVansSharingCommonHdr(ByRef Err_No As Long, ByRef Err_Desc As String, Id As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT Distinct Info_Key from TBL_Custom_Info where Info_Type='VAN_LOGO' and Value_2='" & Id & "'")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetVanfromUser(ByRef Err_No As Long, ByRef Err_Desc As String, userId As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT Distinct Org_ID from app_GetControlInfo(" & userId & ")")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetVansWthNoLogos(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT Distinct Org_ID from TBL_Org_CTL_DTL where Org_ID not in(select Info_key from TBL_Custom_Info  WHERE  Info_Type='VAN_LOGO') and isnull(Org_ID,'')<>'' order by Org_ID")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetClientsWthNoLogos(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT Distinct Location from tbl_Customer where location not in(select Info_key from TBL_Custom_Info  WHERE  Info_Type='CLIENT_LOGO') and isnull(Location,'')<>'' order by Location")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetAppConfig(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Control_Key As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT *  from TBL_App_Control  WHERE Control_Key='" & Control_Key & "'")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            If dt.Rows.Count > 0 Then
                ProductPath = dt.Rows(0)("Control_Value").ToString()
            End If
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return ProductPath
    End Function

    Public Function GetMediaPath(ByRef Err_No As Long, ByRef Err_Desc As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT Physical_Base_Path+'\'+File_Path  from  TBL_Media_Types  WHERE Media_Type='Image'")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            If dt.Rows.Count > 0 Then
                ProductPath = dt.Rows(0)(0).ToString()
            End If
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return ProductPath
    End Function
    Public Function GetLogoPath(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Type As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT Physical_Path  from TBL_File_Types  WHERE File_Type='" & Type & "'")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            If dt.Rows.Count > 0 Then
                ProductPath = dt.Rows(0)(0).ToString()
            End If
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return ProductPath
    End Function

    Public Function GetOrgLogoPath(ByRef Err_No As Long, ByRef Err_Desc As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT Physical_Path  from TBL_File_Types  WHERE File_Type='ORG_LOGO'")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            If dt.Rows.Count > 0 Then
                ProductPath = dt.Rows(0)(0).ToString()
            End If
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return ProductPath
    End Function
    Public Function GetFileTypes(ByRef Err_No As Long, ByRef Err_Desc As String, File_Type As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT *  from TBL_File_Types  WHERE File_Type='" & File_Type & "'")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)

            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetERPSyncTable(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("SELECT DISTINCT Source_Table AS Code,Source_Table AS Value FROM  TBL_ERP_Sync_Log ORDER BY Source_Table  ")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs)
            Dim r As DataRow = MsgDs.NewRow()
            r(0) = "0"
            r(1) = "Select ERP table"

            MsgDs.Rows.InsertAt(r, 0)
            GetERPSyncTable = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "14091"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    Public Function GetSubChannel(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("SELECT distinct isnull(Customer_Class,'N/A')as Customer_Class from dbo.app_GetOrgCustomers ('" & OrgID & "')")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetSubChannel = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetChannel(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("SELECT distinct isnull(Customer_Type,'N/A')as Customer_Type from dbo.app_GetOrgCustomers ('" & OrgID & "')")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetChannel = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetSyncType(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("SELECT DISTINCT Sync_Type AS Code,Sync_Type AS Value FROM TBL_Device_DB_Sync_Log")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs)
            Dim r As DataRow = MsgDs.NewRow()
            r(0) = "0"
            r(1) = "Select Sync Type"

            MsgDs.Rows.InsertAt(r, 0)
            GetSyncType = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "14091"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    Public Function GetDocStatus(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QueryStr As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("SELECT distinct status,Description FROM TBL_Doc_Status WHERE Doc_Type IN ({0}) order by Description ", QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")

            GetDocStatus = MsgDs.Tables(0)
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
    Public Function GetStockRequestStatus(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QueryStr As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("select distinct status as StatusKey,case when Status='N' then 'Pending' when Status='Y' then 'Processed' end  Status from TBL_Opening_Stock_Requests ", QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")

            GetStockRequestStatus = MsgDs.Tables(0)
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
    Public Function GetStockRequestListFilter(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchParams As String, ByVal OrgId As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_StockRequestListParams", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.CommandTimeout = 600
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SearchParams", SearchParams)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgId", OrgId)

            objSQLDA.Fill(dt)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function GetOrganisationName(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            'Dim QueryString As String = String.Format("SELECT DISTINCT MAS_Org_ID, Description FROM TBL_Org_CTL_DTL WHERE SalesRep_ID IN ({0}) ORDER BY MAS_Org_ID DESC", QueryStr)
            Dim QueryString As String = String.Format("Select * from TBL_Org_CTL_H where ORG_HE_ID=@Org_ID")

            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@Org_ID", ID)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")

            GetOrganisationName = MsgDs.Tables(0)
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
    Public Function GetOrganisation(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QueryStr As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            'Dim QueryString As String = String.Format("SELECT DISTINCT MAS_Org_ID, Description FROM TBL_Org_CTL_DTL WHERE SalesRep_ID IN ({0}) ORDER BY MAS_Org_ID DESC", QueryStr)
            Dim QueryString As String = String.Format("SELECT DISTINCT MAS_Org_ID, Description,Currency_Code ,(SELECT Decimal_Digits FROM TBL_Currency WHERE Currency_Code =A.Currency_Code)AS DecimalDigits,(SELECT Country FROM TBL_Currency WHERE Currency_Code =A.Currency_Code) AS Country  FROM TBL_Org_CTL_DTL AS A WHERE SalesRep_ID IN ({0}) ORDER BY MAS_Org_ID DESC", QueryStr)

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
    Public Function LoadOrgLogo(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("app_GetOrgLogos")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "54068"
            Err_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetCustomerSegmentList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QueryStr As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT Customer_Segment_ID, Description from TBL_Customer_Segments ORDER BY Description ASC")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustomerSegmentTbl")

            GetCustomerSegmentList = MsgDs.Tables("CustomerSegmentTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetCustomer(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QueryStr As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("SELECT (LTRIM(STR(Customer_ID)) + '$' + LTRIM(STR(Site_Use_ID))) as CustomerID,'['+IsNULL(Customer_No,'N/A')+']-'+IsNULL(Customer_Name,'N/A')+'-'+ISNULL(Location,'N/A') as Customer from TBL_Customer_Ship_Address  ORDER BY '['+IsNULL(Customer_No,'N/A')+']-'+IsNULL(Customer_Name,'N/A')+'-'+ISNULL(Location,'N/A')  ASC  ")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustomerTbl")

            GetCustomer = MsgDs.Tables("CustomerTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetCustomerLocation(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("SELECT DISTINCT CASE WHEN Location IS NULL or Location='' THEN 'N/A' ELSE Location END AS Location,CASE WHEN Location IS NULL or Location='' THEN 'N/A' ELSE Location END as LocCode from TBL_Customer  ORDER BY Location ASC  ")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustomerTbl")

            GetCustomerLocation = MsgDs.Tables("CustomerTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetCustomerTypeList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QueryStr As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT Distinct CASE WHEN Customer_Type IS NULL OR Customer_Type ='' THEN 'N/A' ELSE Customer_Type END Customer_Type from TBL_Customer ORDER BY Customer_Type ASC")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustomerTypeTbl")

            GetCustomerTypeList = MsgDs.Tables("CustomerTypeTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetCustomerClass(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QueryStr As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT Distinct CASE WHEN Customer_Class IS NULL OR Customer_Class ='' THEN 'N/A' ELSE Customer_Class END Customer_Class from TBL_Customer ORDER BY Customer_Class ASC")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustomerClassTbl")

            GetCustomerClass = MsgDs.Tables("CustomerClassTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetCollectionTypeList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QueryStr As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT Distinct Collection_Type from TBL_Collection ORDER BY Collection_Type ASC")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CollectionTypeTbl")

            GetCollectionTypeList = MsgDs.Tables("CollectionTypeTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetSalesDistrictList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QueryStr As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT Sales_District_ID, Description from TBL_Sales_District ORDER BY Description ASC")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "SalesDistrictTbl")

            GetSalesDistrictList = MsgDs.Tables("SalesDistrictTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function GetVanAuditors(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("select USER_ID,username from TBL_User  a inner join TBL_User_Types b on a.User_Type_ID=b.User_Type_ID and b.Designation='Y' and a.Org_HE_ID=@org_ID order by username")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@org_ID", SqlDbType.VarChar)
            objSQLCmd.Parameters("@org_ID").Value = OrgID
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetVanAuditors = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetAllVan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QueryStr As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("SELECT SalesRep_ID, SalesRep_Name from TBL_FSR WHERE SalesRep_ID in ({0}) ORDER BY SalesRep_Name ASC", QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetAllVan = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function


    Public Function UpdateDeviceConfig(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal RowID As String, ByVal Value As String, ByVal CreatedBy As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "UPDATE TBL_FSR_Device_Config  SET Config_Value=@Value ,Last_Updated_At=GetDate(),Last_Updated_By=@CreatedBy WHERE Row_ID=@RowID"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@RowID", RowID)
            objSQLCmd.Parameters.AddWithValue("@Value", Value)
            objSQLCmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()



            sucess = True
        Catch ex As Exception
            Error_No = 75014

        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return sucess

    End Function
    Public Function GetDeviceConfig(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRepID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("SELECT Row_ID, Config_Type,Config_Key,Config_Value,F.SalesRep_name FROM TBL_FSR_Device_Config A inner join tbl_fsr F on F.SalesRep_ID=A.SalesRep_ID WHERE A.SalesRep_ID in (Select item from dbo.SplitQuotedString(@SID))")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@SID", SalesRepID)

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetDeviceConfig = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function GetVanByDesignation(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal ID As String, Designation As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = ""
            If Designation = "Y" Then
                QueryString = "SELECT SalesRep_ID, SalesRep_Name from TBL_FSR Where SalesRep_ID IN( Select SalesRep_Id from app_GetControlInfo(" & ID & ") WHERE MAS_Org_ID='" & OrgID & "' ) ORDER BY SalesRep_Name ASC"
            Else
                QueryString = "SELECT SalesRep_ID, SalesRep_Name from TBL_FSR Where SalesRep_ID IN( Select SalesRep_Id from app_GetControlInfo(" & ID & ")) ORDER BY SalesRep_Name ASC"
            End If
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetVanByDesignation = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function GetUsersForGroup(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal UID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_GetUsersForGroup"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            objSQLCmd.Parameters.AddWithValue("@UID", UID)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetUsersForGroup = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetVanByOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal ID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("SELECT SalesRep_ID, SalesRep_Name from TBL_FSR Where SalesRep_ID IN( Select SalesRep_Id from app_GetControlInfo(" & ID & ") WHERE MAS_Org_ID='" & OrgID & "' ) ORDER BY SalesRep_Name ASC")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetVanByOrg = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetVanOrgIDByOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal ID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("SELECT TBL_FSR.SalesRep_ID,TBL_FSR.SalesRep_Number, TBL_FSR.SalesRep_Name,TBL_Org_CTL_DTL.Org_ID from TBL_FSR INNER JOIN TBL_Org_CTL_DTL on TBL_FSR.SalesRep_ID = TBL_Org_CTL_DTL.SalesRep_ID Where TBL_FSR.SalesRep_ID IN( Select SalesRep_Id from app_GetControlInfo(" & ID & ") WHERE MAS_Org_ID='" & OrgID & "' ) ORDER BY SalesRep_Name ASC")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetVanOrgIDByOrg = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    ' Get all vans of all org
    Public Function GetAllVans(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("SELECT TBL_FSR.SalesRep_ID,TBL_FSR.SalesRep_Number, TBL_FSR.SalesRep_Name,TBL_Org_CTL_DTL.Org_ID from TBL_FSR INNER JOIN TBL_Org_CTL_DTL on TBL_FSR.SalesRep_ID = TBL_Org_CTL_DTL.SalesRep_ID Where TBL_FSR.SalesRep_ID IN( Select SalesRep_Id from app_GetControlInfo(" & ID & ")) ORDER BY SalesRep_Name ASC")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetAllVans = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function GetDefaultPlanIdForNextMonth(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable
        Dim sRetVal As String = ""
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("select Default_Plan_ID from TBL_Route_Plan_Default as a inner join TBL_Org_CTL_H as b on a.Site = b.Site where b.ORG_HE_ID = '" + OrgID + "' and a.Start_Date In (SELECT DATEADD(mm, DATEDIFF(mm, 0, GETDATE()) + 1, 0))", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text

            objSQLDA.Fill(dtDivConfig)
            objSQLDA.Dispose()
            If dtDivConfig.Rows.Count > 0 Then
                GetDefaultPlanIdForNextMonth = dtDivConfig.Rows(0)("Default_Plan_ID").ToString
            End If
        Catch ex As Exception
            Err_No = "74206"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function GetVanIdFromVanNo(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRepNum As String, ByVal SalesRepName As String, ByVal OrgID As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable
        Dim sRetVal As String = ""
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter(" select * from TBL_FSR where SalesRep_Name ='" + SalesRepName + "' and SalesRep_Number = '" + SalesRepNum + "'", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text

            objSQLDA.Fill(dtDivConfig)
            objSQLDA.Dispose()
            If dtDivConfig.Rows.Count > 0 Then
                GetVanIdFromVanNo = dtDivConfig.Rows(0)("SalesRep_ID").ToString
            End If
        Catch ex As Exception
            Err_No = "74206"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function GetEmpByOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal ID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("SELECT E.* from TBL_FSR A inner join TBL_Org_CTL_DTL D on D.SalesRep_ID=A.SalesRep_ID inner join TBL_Van_Info V On V.Van_Org_ID=D.Org_ID inner join TBL_Emp_Info E on E.Emp_Code=V.Emp_Code  Where A.SalesRep_ID IN( Select SalesRep_Id from app_GetControlInfo(" & ID & ") WHERE MAS_Org_ID='" & OrgID & "' ) ORDER BY SalesRep_Name ASC")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetEmpByOrg = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetAllSS(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format(" SELECT User_ID,Username FROM TBL_User A WHERE A.Is_SS = 'Y' AND  Org_HE_ID='" & OrgID & "' ORDER BY Username ASC")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "SSTbl")

            GetAllSS = MsgDs.Tables("SSTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetVanByOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal ID As String, ByVal Filter As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("select * from (SELECT SalesRep_ID, SalesRep_Name from TBL_FSR Where SalesRep_ID IN( Select SalesRep_Id from app_GetControlInfo(" & ID & ") WHERE MAS_Org_ID='" & OrgID & "' ) ) as x where SalesRep_Name like '%" & Filter & "%'")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetVanByOrg = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetVanByOrgforSync(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal ID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            ''  Dim QueryString As String = String.Format("SELECT (Select User_ID from TBL_User U where U.SalesRep_ID =TBL_FSR.SalesRep_ID) as SalesRep_ID, SalesRep_Name from TBL_FSR Where SalesRep_ID IN( Select SalesRep_Id from app_GetControlInfo(" & ID & ") WHERE MAS_Org_ID=" & OrgID & " ) ORDER BY SalesRep_Number ASC")
            Dim QueryString As String = String.Format("select b.SalesRep_Name,a.User_ID as SalesRep_ID from  TBL_User a inner join tbl_fsr b  on a.SalesRep_ID=b.SalesRep_ID inner join TBL_Org_CTL_DTL c on c.SalesRep_ID=b.SalesRep_ID Where a.SalesRep_ID IN( Select SalesRep_Id from app_GetControlInfo(" & ID & ") WHERE MAS_Org_ID in(select item from dbo.SplitQuotedString('" & OrgID & "'))) ORDER BY SalesRep_Number ASC")

            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetVanByOrgforSync = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function GetVanFromMultipleOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgIDs As String, ByVal ID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("SELECT SalesRep_ID, SalesRep_Name from TBL_FSR Where SalesRep_ID IN( Select SalesRep_Id from app_GetControlInfo(" & ID & ") WHERE MAS_Org_ID in(" & OrgIDs & ") ) ORDER BY SalesRep_Name ASC")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetVanFromMultipleOrg = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetCustomerShipfromOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "SELECT (LTRIM(STR(Customer_ID)) + '$' + LTRIM(STR(Site_Use_ID))) as CustomerID,'['+IsNULL(Customer_No,'N/A')+']-'+IsNULL(Customer_Name,'N/A') as Customer from dbo.app_GetOrgCustomerShipAddress ('" & OrgID & "')"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetCustomerShipfromOrg = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetCustomerfromOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "SELECT (LTRIM(STR(Customer_ID)) + '$' + LTRIM(STR(Site_Use_ID))) as CustomerID,'['+IsNULL(Customer_No,'N/A')+']-'+IsNULL(Customer_Name,'N/A') as Customer from dbo.app_GetOrgCustomers ('" & OrgID & "')"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetCustomerfromOrg = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function GetAgencyByOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal ID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("select distinct Agency FROm TBL_product WHERE Organization_id='" & OrgID & "' AND Agency <>'' Order By Agency  ")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetAgencyByOrg = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetCategoryByOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal ID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("select distinct Category FROm TBL_product WHERE Organization_id='" & OrgID & "' AND Category <>'' Order By Category  ")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetCategoryByOrg = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetPriceTypeList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT Price_List_ID,[Description] as Price_List_Type from TBL_Price_List_H  {0} or Organization_ID=0 ORDER BY [Description] ASC", _sSearchParams)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "PriceTypeTbl")

            GetPriceTypeList = MsgDs.Tables("PriceTypeTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function


    Public Function GetFSRVisitedDates(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRepID As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "app_FSRVisitedDates"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@SalesRepID", SalesRepID)

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VisitsTbl")

            GetFSRVisitedDates = MsgDs.Tables("VisitsTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74052"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetAllProducts(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT distinct  Item_Code ,Item_Code +'-'+Description as Description  from TBL_Product  ORDER BY [Item_Code] ASC")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "Product")

            GetAllProducts = MsgDs.Tables("Product")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetAllProductsByOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByRef OrgID As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format(" Select Inventory_Item_ID,Item_Code+'-'+Description AS Description,Item_Code from TBL_Product Where Organization_ID='{0}' ORDER BY [Description] ASC", OrgID)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "Product")

            GetAllProductsByOrg = MsgDs.Tables("Product")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetAllProductsByOrg_Agency(ByRef Err_No As Long, ByRef Err_Desc As String, ByRef OrgID As Integer, ByVal Agency As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format(" Select Inventory_Item_ID,item_code +'-'+ Description as Description,Item_Code from TBL_Product Where Organization_ID='{0}' and isnull(Agency,'N/A')='{1}' ORDER BY [Description] ASC", OrgID, Agency)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "Product")

            GetAllProductsByOrg_Agency = MsgDs.Tables("Product")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetAllProductsByOrg_Van(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As Integer, ByVal Van As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format(" Select distinct  Inventory_Item_ID,P.Description from TBL_Product P inner join TBL_Org_CTL_DTL O on O.MAS_Org_ID=P.Organization_ID  Where O.SalesRep_ID in ({0}) and Organization_ID='{1}'", Van, OrgID)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "Product")

            GetAllProductsByOrg_Van = MsgDs.Tables("Product")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetAllProductsByOrg_Van_Agency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As Integer, ByVal Van As String, ByVal Agency As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format(" Select Inventory_Item_ID,P.Description from TBL_Product P inner join TBL_Org_CTL_DTL O on O.MAS_Org_ID=P.Organization_ID  Where O.SalesRep_ID in ({0}) and Organization_ID='{1}' and Agency='{2}'", Van, OrgID, Agency)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "Product")

            GetAllProductsByOrg_Van_Agency = MsgDs.Tables("Product")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetAllUOM(ByRef Err_No As Long, ByRef Err_Desc As String, Optional ByVal Inventory_item_ID As Integer = 0) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String
            If Inventory_item_ID <> 0 Then
                QueryString = String.Format("SELECT distinct  Item_UOM   from TBL_Price_List where Inventory_item_Id=" & Inventory_item_ID & " ORDER BY [Item_UOM] ASC")
            Else
                QueryString = String.Format("SELECT distinct  Item_UOM   from TBL_Price_List ORDER BY [Item_UOM] ASC")
            End If

            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "UOM")

            GetAllUOM = MsgDs.Tables("UOM")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetCurrencyByOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("Select top 1  O.Currency_Code ,Decimal_Digits from dbo.TBL_Org_CTL_DTL O inner join TBL_Currency C on O.Currency_Code=C.Currency_Code where MAS_Org_ID='" & OrgID & "'")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "Curr")

            GetCurrencyByOrg = MsgDs.Tables("Curr")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetAllAgency(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT distinct  Agency   from TBL_Product ORDER BY [Agency] ASC")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "Agency")

            GetAllAgency = MsgDs.Tables("Agency")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetAllAgencyByOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select distinct ISNULL(Agency,'N/A')AS Agency from TBL_Product Where Organization_ID='{0}' ORDER BY [Agency] ASC", OrgId)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "Agency")

            GetAllAgencyByOrg = MsgDs.Tables("Agency")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetAllAgencyByOrg_Van(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As Integer, ByVal SalesRepID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("Select distinct Agency from TBL_Product P inner join TBL_Org_CTL_DTL O on O.MAS_Org_ID=P.Organization_ID  Where O.SalesRep_ID in ({0}) and Organization_ID='{1}'  ORDER BY [Agency] ASC", SalesRepID, OrgId)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "Agency")

            GetAllAgencyByOrg_Van = MsgDs.Tables("Agency")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function



    Public Function GetVanAuditReport(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SPID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            'Dim QueryString As String = String.Format("select  A.SalesRep_ID,A.Survey_Timestamp , B.Survey_Title,D.Question_Text,A.Response,E.Response_Type_ID,A.Question_ID,CASE ISNULL (E.Response_Type_ID,'') when '' Then A.Response Else E.Response_Text End as ResponseText,V.Emp_Name,V.Emp_Code,U.UserName,V.Sales_Org_ID,H.Site from TBL_Survey_Audit_Responses as A INNER JOIN TBL_Survey as B ON A.Survey_ID=B.Survey_ID INNER JOIN TBL_Survey_Questions as D on A.Question_ID=D.Question_ID  LEFT OUTER JOIN TBL_Survey_Responses as E ON A.Response=LTRIM(STR(E.Response_ID)) LEFT OUTER JOIN TBL_Van_Info V  on V.Emp_Code=A.Emp_Code LEFT OUTER JOIN TBL_User U on U.User_ID=A.Surveyed_BY  LEFT OUTER JOIN dbo.TBL_Org_CTL_H H on H.ORG_HE_ID=V.Sales_Org_ID WHERE 1=1 and  Survey_Timestamp=(select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses A Where  1=1 and {0}  ORDER BY Survey_Timestamp DESC)  and {0} ORDER BY A.Question_ID", QueryStr)
            'Dim QueryString As String = String.Format("select  A.SalesRep_ID,A.Survey_Timestamp , B.Survey_Title,D.Question_Text,A.Response,E.Response_Type_ID,A.Question_ID,CASE ISNULL (E.Response_Type_ID,'') when '' Then A.Response Else E.Response_Text End as ResponseText,V.Emp_Name,V.Emp_Code,U.UserName,V.Sales_Org_ID,H.Site,'0' AS IsDefault from TBL_Survey_Audit_Responses as A INNER JOIN TBL_Survey as B ON A.Survey_ID=B.Survey_ID INNER JOIN TBL_Survey_Questions as D on A.Question_ID=D.Question_ID  LEFT OUTER JOIN TBL_Survey_Responses as E ON A.Question_Id=E.question_id LEFT OUTER JOIN TBL_Van_Info V  on V.Emp_Code=A.Emp_Code LEFT OUTER JOIN TBL_User U on U.User_ID=A.Surveyed_BY  LEFT OUTER JOIN dbo.TBL_Org_CTL_H H on H.ORG_HE_ID=V.Sales_Org_ID WHERE 1=1 and  Survey_Timestamp=(select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses A Where  1=1 and {0}  ORDER BY Survey_Timestamp DESC)  and {0} ORDER BY A.Question_ID", QueryStr)
            'Dim QueryString As String = String.Format("select  A.SalesRep_ID,A.Survey_Timestamp ,A.Response AS ResText, B.Survey_Title,D.Question_Text,CASE ISNULL (E.Response_Type_ID,'') when '' Then A.Response Else E.Response_Id End as Response,E.Response_Type_ID,A.Question_ID,CASE ISNULL (E.Response_Type_ID,'') when '' Then A.Response Else E.Response_Text End as ResponseText,V.Emp_Name,V.Emp_Code,U.UserName,V.Sales_Org_ID,H.Site,(CASE WHEN  E.Response_Type_ID=1  THEN '0' ELSE (CASE WHEN E.Response_Type_ID<>1 AND A.Response= E.Response_Id THEN '1' ELSE '0' End) END)  AS ISDefault,A.Survey_Id,A.SalesRep_Id,(SELECT Top 1 SalesRep_Name from TBL_FSR WHERE SalesRep_Id=A.SalesRep_Id)AS SalesRepName from TBL_Survey_Audit_Responses as A INNER JOIN TBL_Survey as B ON A.Survey_ID=B.Survey_ID INNER JOIN TBL_Survey_Questions as D on A.Question_ID=D.Question_ID  LEFT OUTER JOIN TBL_Survey_Responses as E ON A.Question_Id=E.question_id AND A.Response=CAST(E.Response_id AS Varchar) AND E.Response_type_id<>1 LEFT OUTER JOIN TBL_Van_Info V  on V.Emp_Code=A.Emp_Code LEFT OUTER JOIN TBL_User U on U.User_ID=A.Surveyed_BY  LEFT OUTER JOIN dbo.TBL_Org_CTL_H H on H.ORG_HE_ID=V.Sales_Org_ID WHERE 1=1 and  Survey_Timestamp=(select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses  A Where Status='N' AND  1=1 and {0}     ORDER BY Survey_Timestamp DESC) AND  Survey_Timestamp>= CAST(ISNULL((select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses  A Where Status='C' AND  1=1 and {0}  ORDER BY Survey_Timestamp DESC),'01-01-1900') AS Datetime)  and A.Status='N' AND {0} UNION ALL select  A.SalesRep_ID,A.Survey_Timestamp ,A.Response AS ResText, B.Survey_Title,D.Question_Text,CASE ISNULL (E.Response_Type_ID,'') when '' Then A.Response Else E.Response_Id End as Response,E.Response_Type_ID,A.Question_ID,CASE ISNULL (E.Response_Type_ID,'') when '' Then A.Response Else E.Response_Text End as ResponseText,V.Emp_Name,V.Emp_Code,U.UserName,V.Sales_Org_ID,H.Site,'0'  AS ISDefault,A.Survey_Id,A.SalesRep_Id,(SELECT Top 1 SalesRep_Name from TBL_FSR WHERE SalesRep_Id=A.SalesRep_Id)AS SalesRepName from TBL_Survey_Audit_Responses as A INNER JOIN TBL_Survey as B ON A.Survey_ID=B.Survey_ID INNER JOIN TBL_Survey_Questions as D on A.Question_ID=D.Question_ID  LEFT OUTER JOIN TBL_Survey_Responses as E ON A.Question_Id=E.question_id AND A.Response=CAST(E.Response_id AS Varchar) AND E.Response_type_id=1  LEFT OUTER JOIN TBL_Van_Info V  on V.Emp_Code=A.Emp_Code LEFT OUTER JOIN TBL_User U on U.User_ID=A.Surveyed_BY  LEFT OUTER JOIN dbo.TBL_Org_CTL_H H on H.ORG_HE_ID=V.Sales_Org_ID WHERE 1=1 and  Survey_Timestamp=(select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses  A Where Status='N' AND  1=1 and {0}     ORDER BY Survey_Timestamp DESC) AND  Survey_Timestamp>= CAST(ISNULL((select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses  A Where Status='C' AND  1=1 and {0}  ORDER BY Survey_Timestamp DESC),'01-01-1900') AS Datetime)  and A.Status='N' AND {0} ORDER BY A.Question_ID", QueryStr)
            '            Dim QueryString As String = String.Format("select  A.SalesRep_ID,A.Survey_Timestamp ,A.Response AS ResText, B.Survey_Title,D.Question_Text, E.Response_text as Response,E.Response_Type_ID,A.Question_ID, A.Response as ResponseText,V.Emp_Name,V.Emp_Code,U.UserName,V.Sales_Org_ID,H.Site,(CASE WHEN CAST(E.Response_type_id AS VARCHAR)<>'1' AND A.Response= E.Response_Id THEN '1' ELSE '0' End)  AS ISDefault,A.Survey_Id,A.SalesRep_Id,(SELECT Top 1 SalesRep_Name from TBL_FSR WHERE SalesRep_Id=A.SalesRep_Id)AS SalesRepName from TBL_Survey_Audit_Responses as A INNER JOIN TBL_Survey as B ON A.Survey_ID=B.Survey_ID INNER JOIN TBL_Survey_Questions as D on A.Question_ID=D.Question_ID  LEFT OUTER JOIN TBL_Survey_Responses as E ON A.Question_Id=E.question_id LEFT OUTER JOIN TBL_Van_Info V  on V.Emp_Code=A.Emp_Code LEFT OUTER JOIN TBL_User U on U.User_ID=A.Surveyed_BY  LEFT OUTER JOIN dbo.TBL_Org_CTL_H H on H.ORG_HE_ID=V.Sales_Org_ID WHERE (CASE WHEN  CASt(A.Response AS VARCHAR)=CAST(E.Response_ID AS Varchar) THEN E.Response_type_id ELSE '1' END)<>'1'  AND   1=1 and  Survey_Timestamp=(select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses  A Where Status='N' AND  1=1 and {0}       ORDER BY Survey_Timestamp DESC) AND  Survey_Timestamp>= CAST(ISNULL((select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses  A Where Status='C' AND  1=1 and {0}     ORDER BY Survey_Timestamp DESC),'01-01-1900') AS Datetime)   and A.Status='N'  and {0}     UNION ALL select  A.SalesRep_ID,A.Survey_Timestamp ,A.Response AS ResText, B.Survey_Title,D.Question_Text, A.Response  as Response,E.Response_Type_ID,A.Question_ID,A.Response as ResponseText,V.Emp_Name,V.Emp_Code,U.UserName,V.Sales_Org_ID,H.Site,'0'  AS ISDefault,A.Survey_Id,A.SalesRep_Id,(SELECT Top 1 SalesRep_Name from TBL_FSR WHERE SalesRep_Id=A.SalesRep_Id)AS SalesRepName from TBL_Survey_Audit_Responses as A INNER JOIN TBL_Survey as B ON A.Survey_ID=B.Survey_ID INNER JOIN TBL_Survey_Questions as D on A.Question_ID=D.Question_ID  LEFT OUTER JOIN TBL_Survey_Responses as E ON A.Question_Id=E.question_id  AND E.Response_type_id='1'  LEFT OUTER JOIN TBL_Van_Info V  on V.Emp_Code=A.Emp_Code LEFT OUTER JOIN TBL_User U on U.User_ID=A.Surveyed_BY  LEFT OUTER JOIN dbo.TBL_Org_CTL_H H on H.ORG_HE_ID=V.Sales_Org_ID WHERE 1=1 and  Survey_Timestamp=(select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses  A Where Status='N' AND  1=1 and {0}    ORDER BY Survey_Timestamp DESC) AND  Survey_Timestamp>= CAST(ISNULL((select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses  A Where Status='C' AND  1=1 and {0}     ORDER BY Survey_Timestamp DESC),'01-01-1900') AS Datetime)  and A.Status='N'  and {0}   AND E.Response_type_id='1' ORDER BY A.Question_ID", QueryStr)
            ' Dim QueryString As String = String.Format("SELECT SalesRep_Id, Convert(Varchar(19),Survey_Timestamp,120) AS Survey_Timestamp ,ResText,Survey_Title ,Question_Text,Response,Response_Type_ID,Question_ID, ResponseText,Emp_Name,Emp_Code,UserName,Sales_Org_ID,Site,            ISDefault, Survey_Id, SalesRepName            from (select A.SalesRep_Id, A.Survey_Timestamp,A.Response AS ResText, B.Survey_Title,D.Question_Text, CASE ISNULL (E.Response_Type_ID,'') when '' Then A.Response Else E.Response_Id End as Response,E.Response_Type_ID,A.Question_ID,CASE ISNULL (E.Response_Type_ID,'') when '' Then A.Response Else E.Response_Text End as ResponseText,V.Emp_Name,V.Emp_Code,U.UserName,V.Sales_Org_ID,H.Site,(CASE WHEN CAST(E.Response_type_id AS VARCHAR)<>'1' AND cast(A.Response AS VARCHAR)= CAST(E.Response_type_id AS VARCHAR) THEN '1' ELSE '0' End)  AS ISDefault,A.Survey_Id,(SELECT Top 1 SalesRep_Name from TBL_FSR WHERE SalesRep_Id=A.SalesRep_Id)AS SalesRepName from TBL_Survey_Audit_Responses as A INNER JOIN TBL_Survey as B ON A.Survey_ID=B.Survey_ID INNER JOIN TBL_Survey_Questions as D on A.Question_ID=D.Question_ID  LEFT OUTER JOIN TBL_Survey_Responses as E ON A.Question_Id=E.question_id LEFT OUTER JOIN TBL_Van_Info V  on V.Emp_Code=A.Emp_Code LEFT OUTER JOIN TBL_User U on U.User_ID=A.Surveyed_BY  LEFT OUTER JOIN dbo.TBL_Org_CTL_H H on H.ORG_HE_ID=V.Sales_Org_ID WHERE   1=1 and  Survey_Timestamp=(select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses  A Where Status='N' AND  1=1 and  {0}       ORDER BY Survey_Timestamp DESC) AND  Survey_Timestamp>= CAST(ISNULL((select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses  A Where Status='C' AND  1=1 and {0}    ORDER BY Survey_Timestamp DESC),'01-01-1900') AS Datetime)   and A.Status='N'  and  {0} ) #t ", QueryStr)

            objSQLCmd = New SqlCommand("app_EditCashVanAudit", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@SalesRepID", SPID)


            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetVanAuditReport = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetPrev_AuditDate(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal DateofAudit As String, ByVal VanID As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses where Status='N' AND Survey_Timestamp <= '{0}' and SalesRep_ID='{1}'  order by Survey_Timestamp desc", DateofAudit, VanID)

            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim prevDate As String = IIf(IsNothing(objSQLCmd.ExecuteScalar()), "-", Convert.ToDateTime(objSQLCmd.ExecuteScalar()).ToString("dd/MM/yyyy"))
            objSQLCmd.Dispose()
            Return prevDate

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetCustomerByCriteria(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QueryStr As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "SELECT DISTINCT (LTRIM(STR(Customer_ID)) + '$' + LTRIM(STR(Site_Use_ID))) as CustomerID,'['+IsNULL(Customer_No,'N/A')+']-'+IsNULL(Customer_Name,'N/A')+'-'+ISNULL(Location,'N/A') as Customer from dbo.app_GetOrgCustomerShipAddress(@OID)  ORDER BY '['+IsNULL(Customer_No,'N/A')+']-'+IsNULL(Customer_Name,'N/A')+'-'+ISNULL(Location,'N/A') ASC  "
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@OID", QueryStr)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustomerTbl")

            GetCustomerByCriteria = MsgDs.Tables("CustomerTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetCustomerByCriteria(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OID As String, ByVal QueryStr As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "SELECT DISTINCT (LTRIM(STR(A.Customer_ID)) + '$' + LTRIM(STR(A.Site_Use_ID))) as CustomerID,'['+IsNULL(A.Customer_No,'N/A')+']-'+IsNULL(A.Customer_Name,'N/A')+'-'+ISNULL(A.Location,'N/A') as Customer from dbo.app_GetOrgCustomerShipAddress(@OID) A"
            If QueryStr <> "" Then
                QueryString = QueryString & " inner join tbl_Customer B on A.Customer_No=b.Customer_No "
            End If
            QueryString = QueryString & " where 1=1 " & QueryStr & " ORDER BY '['+IsNULL(A.Customer_No,'N/A')+']-'+IsNULL(A.Customer_Name,'N/A')+'-'+ISNULL(A.Location,'N/A') ASC  "
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@OID", OID)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustomerTbl")

            GetCustomerByCriteria = MsgDs.Tables("CustomerTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetCustomerByCriteriaandText(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OID As String, ByVal QueryStr As String, searchTxt As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            If Not String.IsNullOrEmpty(searchTxt) Then
                QueryStr = QueryStr & " AND (A.Customer_no LIKE '%" & searchTxt & "%' OR A.Customer_no +'-'+ A.Customer_name LIKE '%" & searchTxt & "%') "
            End If

            Dim QueryString As String = "SELECT DISTINCT (LTRIM(STR(A.Customer_ID)) + '$' + LTRIM(STR(A.Site_Use_ID))) as CustomerID,'['+IsNULL(A.Customer_No,'N/A')+']-'+IsNULL(A.Customer_Name,'N/A')+'-'+ISNULL(A.Location,'N/A') as Customer from dbo.app_GetOrgCustomerShipAddress(@OID) A"
            If QueryStr <> "" Then
                QueryString = QueryString & " inner join tbl_Customer B on A.Customer_No=b.Customer_No "
            End If
            QueryString = QueryString & " where 1=1 " & QueryStr & " ORDER BY '['+IsNULL(A.Customer_No,'N/A')+']-'+IsNULL(A.Customer_Name,'N/A')+'-'+ISNULL(A.Location,'N/A') ASC  "
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@OID", OID)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustomerTbl")

            GetCustomerByCriteriaandText = MsgDs.Tables("CustomerTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetDayEndTime(ByRef Err_No As Long, ByRef Err_Desc As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable
        Dim sRetVal As String = ""
        Try

            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("select Control_Value from TBL_App_Control where Control_Key='DAY_END_TIME'", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text

            objSQLDA.Fill(dtDivConfig)
            objSQLDA.Dispose()
            If dtDivConfig.Rows.Count > 0 Then
                sRetVal = dtDivConfig.Rows(0)("Control_Value").ToString
            End If
        Catch ex As Exception
            Err_No = "74206"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return sRetVal
    End Function
    Public Function GetMonths(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            'Dim QueryString As String = String.Format("select  A.SalesRep_ID,A.Survey_Timestamp , B.Survey_Title,D.Question_Text,A.Response,E.Response_Type_ID,A.Question_ID,CASE ISNULL (E.Response_Type_ID,'') when '' Then A.Response Else E.Response_Text End as ResponseText,V.Emp_Name,V.Emp_Code,U.UserName,V.Sales_Org_ID,H.Site from TBL_Survey_Audit_Responses as A INNER JOIN TBL_Survey as B ON A.Survey_ID=B.Survey_ID INNER JOIN TBL_Survey_Questions as D on A.Question_ID=D.Question_ID  LEFT OUTER JOIN TBL_Survey_Responses as E ON A.Response=LTRIM(STR(E.Response_ID)) LEFT OUTER JOIN TBL_Van_Info V  on V.Emp_Code=A.Emp_Code LEFT OUTER JOIN TBL_User U on U.User_ID=A.Surveyed_BY  LEFT OUTER JOIN dbo.TBL_Org_CTL_H H on H.ORG_HE_ID=V.Sales_Org_ID WHERE 1=1 and  Survey_Timestamp=(select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses A Where  1=1 and {0}  ORDER BY Survey_Timestamp DESC)  and {0} ORDER BY A.Question_ID", QueryStr)
            'Dim QueryString As String = String.Format("select  A.SalesRep_ID,A.Survey_Timestamp , B.Survey_Title,D.Question_Text,A.Response,E.Response_Type_ID,A.Question_ID,CASE ISNULL (E.Response_Type_ID,'') when '' Then A.Response Else E.Response_Text End as ResponseText,V.Emp_Name,V.Emp_Code,U.UserName,V.Sales_Org_ID,H.Site,'0' AS IsDefault from TBL_Survey_Audit_Responses as A INNER JOIN TBL_Survey as B ON A.Survey_ID=B.Survey_ID INNER JOIN TBL_Survey_Questions as D on A.Question_ID=D.Question_ID  LEFT OUTER JOIN TBL_Survey_Responses as E ON A.Question_Id=E.question_id LEFT OUTER JOIN TBL_Van_Info V  on V.Emp_Code=A.Emp_Code LEFT OUTER JOIN TBL_User U on U.User_ID=A.Surveyed_BY  LEFT OUTER JOIN dbo.TBL_Org_CTL_H H on H.ORG_HE_ID=V.Sales_Org_ID WHERE 1=1 and  Survey_Timestamp=(select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses A Where  1=1 and {0}  ORDER BY Survey_Timestamp DESC)  and {0} ORDER BY A.Question_ID", QueryStr)
            'Dim QueryString As String = String.Format("select  A.SalesRep_ID,A.Survey_Timestamp ,A.Response AS ResText, B.Survey_Title,D.Question_Text,CASE ISNULL (E.Response_Type_ID,'') when '' Then A.Response Else E.Response_Id End as Response,E.Response_Type_ID,A.Question_ID,CASE ISNULL (E.Response_Type_ID,'') when '' Then A.Response Else E.Response_Text End as ResponseText,V.Emp_Name,V.Emp_Code,U.UserName,V.Sales_Org_ID,H.Site,(CASE WHEN  E.Response_Type_ID=1  THEN '0' ELSE (CASE WHEN E.Response_Type_ID<>1 AND A.Response= E.Response_Id THEN '1' ELSE '0' End) END)  AS ISDefault,A.Survey_Id,A.SalesRep_Id,(SELECT Top 1 SalesRep_Name from TBL_FSR WHERE SalesRep_Id=A.SalesRep_Id)AS SalesRepName from TBL_Survey_Audit_Responses as A INNER JOIN TBL_Survey as B ON A.Survey_ID=B.Survey_ID INNER JOIN TBL_Survey_Questions as D on A.Question_ID=D.Question_ID  LEFT OUTER JOIN TBL_Survey_Responses as E ON A.Question_Id=E.question_id AND A.Response=CAST(E.Response_id AS Varchar) AND E.Response_type_id<>1 LEFT OUTER JOIN TBL_Van_Info V  on V.Emp_Code=A.Emp_Code LEFT OUTER JOIN TBL_User U on U.User_ID=A.Surveyed_BY  LEFT OUTER JOIN dbo.TBL_Org_CTL_H H on H.ORG_HE_ID=V.Sales_Org_ID WHERE 1=1 and  Survey_Timestamp=(select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses  A Where Status='N' AND  1=1 and {0}     ORDER BY Survey_Timestamp DESC) AND  Survey_Timestamp>= CAST(ISNULL((select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses  A Where Status='C' AND  1=1 and {0}  ORDER BY Survey_Timestamp DESC),'01-01-1900') AS Datetime)  and A.Status='N' AND {0} UNION ALL select  A.SalesRep_ID,A.Survey_Timestamp ,A.Response AS ResText, B.Survey_Title,D.Question_Text,CASE ISNULL (E.Response_Type_ID,'') when '' Then A.Response Else E.Response_Id End as Response,E.Response_Type_ID,A.Question_ID,CASE ISNULL (E.Response_Type_ID,'') when '' Then A.Response Else E.Response_Text End as ResponseText,V.Emp_Name,V.Emp_Code,U.UserName,V.Sales_Org_ID,H.Site,'0'  AS ISDefault,A.Survey_Id,A.SalesRep_Id,(SELECT Top 1 SalesRep_Name from TBL_FSR WHERE SalesRep_Id=A.SalesRep_Id)AS SalesRepName from TBL_Survey_Audit_Responses as A INNER JOIN TBL_Survey as B ON A.Survey_ID=B.Survey_ID INNER JOIN TBL_Survey_Questions as D on A.Question_ID=D.Question_ID  LEFT OUTER JOIN TBL_Survey_Responses as E ON A.Question_Id=E.question_id AND A.Response=CAST(E.Response_id AS Varchar) AND E.Response_type_id=1  LEFT OUTER JOIN TBL_Van_Info V  on V.Emp_Code=A.Emp_Code LEFT OUTER JOIN TBL_User U on U.User_ID=A.Surveyed_BY  LEFT OUTER JOIN dbo.TBL_Org_CTL_H H on H.ORG_HE_ID=V.Sales_Org_ID WHERE 1=1 and  Survey_Timestamp=(select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses  A Where Status='N' AND  1=1 and {0}     ORDER BY Survey_Timestamp DESC) AND  Survey_Timestamp>= CAST(ISNULL((select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses  A Where Status='C' AND  1=1 and {0}  ORDER BY Survey_Timestamp DESC),'01-01-1900') AS Datetime)  and A.Status='N' AND {0} ORDER BY A.Question_ID", QueryStr)
            '            Dim QueryString As String = String.Format("select  A.SalesRep_ID,A.Survey_Timestamp ,A.Response AS ResText, B.Survey_Title,D.Question_Text, E.Response_text as Response,E.Response_Type_ID,A.Question_ID, A.Response as ResponseText,V.Emp_Name,V.Emp_Code,U.UserName,V.Sales_Org_ID,H.Site,(CASE WHEN CAST(E.Response_type_id AS VARCHAR)<>'1' AND A.Response= E.Response_Id THEN '1' ELSE '0' End)  AS ISDefault,A.Survey_Id,A.SalesRep_Id,(SELECT Top 1 SalesRep_Name from TBL_FSR WHERE SalesRep_Id=A.SalesRep_Id)AS SalesRepName from TBL_Survey_Audit_Responses as A INNER JOIN TBL_Survey as B ON A.Survey_ID=B.Survey_ID INNER JOIN TBL_Survey_Questions as D on A.Question_ID=D.Question_ID  LEFT OUTER JOIN TBL_Survey_Responses as E ON A.Question_Id=E.question_id LEFT OUTER JOIN TBL_Van_Info V  on V.Emp_Code=A.Emp_Code LEFT OUTER JOIN TBL_User U on U.User_ID=A.Surveyed_BY  LEFT OUTER JOIN dbo.TBL_Org_CTL_H H on H.ORG_HE_ID=V.Sales_Org_ID WHERE (CASE WHEN  CASt(A.Response AS VARCHAR)=CAST(E.Response_ID AS Varchar) THEN E.Response_type_id ELSE '1' END)<>'1'  AND   1=1 and  Survey_Timestamp=(select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses  A Where Status='N' AND  1=1 and {0}       ORDER BY Survey_Timestamp DESC) AND  Survey_Timestamp>= CAST(ISNULL((select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses  A Where Status='C' AND  1=1 and {0}     ORDER BY Survey_Timestamp DESC),'01-01-1900') AS Datetime)   and A.Status='N'  and {0}     UNION ALL select  A.SalesRep_ID,A.Survey_Timestamp ,A.Response AS ResText, B.Survey_Title,D.Question_Text, A.Response  as Response,E.Response_Type_ID,A.Question_ID,A.Response as ResponseText,V.Emp_Name,V.Emp_Code,U.UserName,V.Sales_Org_ID,H.Site,'0'  AS ISDefault,A.Survey_Id,A.SalesRep_Id,(SELECT Top 1 SalesRep_Name from TBL_FSR WHERE SalesRep_Id=A.SalesRep_Id)AS SalesRepName from TBL_Survey_Audit_Responses as A INNER JOIN TBL_Survey as B ON A.Survey_ID=B.Survey_ID INNER JOIN TBL_Survey_Questions as D on A.Question_ID=D.Question_ID  LEFT OUTER JOIN TBL_Survey_Responses as E ON A.Question_Id=E.question_id  AND E.Response_type_id='1'  LEFT OUTER JOIN TBL_Van_Info V  on V.Emp_Code=A.Emp_Code LEFT OUTER JOIN TBL_User U on U.User_ID=A.Surveyed_BY  LEFT OUTER JOIN dbo.TBL_Org_CTL_H H on H.ORG_HE_ID=V.Sales_Org_ID WHERE 1=1 and  Survey_Timestamp=(select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses  A Where Status='N' AND  1=1 and {0}    ORDER BY Survey_Timestamp DESC) AND  Survey_Timestamp>= CAST(ISNULL((select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses  A Where Status='C' AND  1=1 and {0}     ORDER BY Survey_Timestamp DESC),'01-01-1900') AS Datetime)  and A.Status='N'  and {0}   AND E.Response_type_id='1' ORDER BY A.Question_ID", QueryStr)
            Dim QueryString As String = String.Format("Rep_GetMonths ", QueryString)

            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetMonths = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetYear_Distribution(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            'Dim QueryString As String = String.Format("select  A.SalesRep_ID,A.Survey_Timestamp , B.Survey_Title,D.Question_Text,A.Response,E.Response_Type_ID,A.Question_ID,CASE ISNULL (E.Response_Type_ID,'') when '' Then A.Response Else E.Response_Text End as ResponseText,V.Emp_Name,V.Emp_Code,U.UserName,V.Sales_Org_ID,H.Site from TBL_Survey_Audit_Responses as A INNER JOIN TBL_Survey as B ON A.Survey_ID=B.Survey_ID INNER JOIN TBL_Survey_Questions as D on A.Question_ID=D.Question_ID  LEFT OUTER JOIN TBL_Survey_Responses as E ON A.Response=LTRIM(STR(E.Response_ID)) LEFT OUTER JOIN TBL_Van_Info V  on V.Emp_Code=A.Emp_Code LEFT OUTER JOIN TBL_User U on U.User_ID=A.Surveyed_BY  LEFT OUTER JOIN dbo.TBL_Org_CTL_H H on H.ORG_HE_ID=V.Sales_Org_ID WHERE 1=1 and  Survey_Timestamp=(select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses A Where  1=1 and {0}  ORDER BY Survey_Timestamp DESC)  and {0} ORDER BY A.Question_ID", QueryStr)
            'Dim QueryString As String = String.Format("select  A.SalesRep_ID,A.Survey_Timestamp , B.Survey_Title,D.Question_Text,A.Response,E.Response_Type_ID,A.Question_ID,CASE ISNULL (E.Response_Type_ID,'') when '' Then A.Response Else E.Response_Text End as ResponseText,V.Emp_Name,V.Emp_Code,U.UserName,V.Sales_Org_ID,H.Site,'0' AS IsDefault from TBL_Survey_Audit_Responses as A INNER JOIN TBL_Survey as B ON A.Survey_ID=B.Survey_ID INNER JOIN TBL_Survey_Questions as D on A.Question_ID=D.Question_ID  LEFT OUTER JOIN TBL_Survey_Responses as E ON A.Question_Id=E.question_id LEFT OUTER JOIN TBL_Van_Info V  on V.Emp_Code=A.Emp_Code LEFT OUTER JOIN TBL_User U on U.User_ID=A.Surveyed_BY  LEFT OUTER JOIN dbo.TBL_Org_CTL_H H on H.ORG_HE_ID=V.Sales_Org_ID WHERE 1=1 and  Survey_Timestamp=(select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses A Where  1=1 and {0}  ORDER BY Survey_Timestamp DESC)  and {0} ORDER BY A.Question_ID", QueryStr)
            'Dim QueryString As String = String.Format("select  A.SalesRep_ID,A.Survey_Timestamp ,A.Response AS ResText, B.Survey_Title,D.Question_Text,CASE ISNULL (E.Response_Type_ID,'') when '' Then A.Response Else E.Response_Id End as Response,E.Response_Type_ID,A.Question_ID,CASE ISNULL (E.Response_Type_ID,'') when '' Then A.Response Else E.Response_Text End as ResponseText,V.Emp_Name,V.Emp_Code,U.UserName,V.Sales_Org_ID,H.Site,(CASE WHEN  E.Response_Type_ID=1  THEN '0' ELSE (CASE WHEN E.Response_Type_ID<>1 AND A.Response= E.Response_Id THEN '1' ELSE '0' End) END)  AS ISDefault,A.Survey_Id,A.SalesRep_Id,(SELECT Top 1 SalesRep_Name from TBL_FSR WHERE SalesRep_Id=A.SalesRep_Id)AS SalesRepName from TBL_Survey_Audit_Responses as A INNER JOIN TBL_Survey as B ON A.Survey_ID=B.Survey_ID INNER JOIN TBL_Survey_Questions as D on A.Question_ID=D.Question_ID  LEFT OUTER JOIN TBL_Survey_Responses as E ON A.Question_Id=E.question_id AND A.Response=CAST(E.Response_id AS Varchar) AND E.Response_type_id<>1 LEFT OUTER JOIN TBL_Van_Info V  on V.Emp_Code=A.Emp_Code LEFT OUTER JOIN TBL_User U on U.User_ID=A.Surveyed_BY  LEFT OUTER JOIN dbo.TBL_Org_CTL_H H on H.ORG_HE_ID=V.Sales_Org_ID WHERE 1=1 and  Survey_Timestamp=(select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses  A Where Status='N' AND  1=1 and {0}     ORDER BY Survey_Timestamp DESC) AND  Survey_Timestamp>= CAST(ISNULL((select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses  A Where Status='C' AND  1=1 and {0}  ORDER BY Survey_Timestamp DESC),'01-01-1900') AS Datetime)  and A.Status='N' AND {0} UNION ALL select  A.SalesRep_ID,A.Survey_Timestamp ,A.Response AS ResText, B.Survey_Title,D.Question_Text,CASE ISNULL (E.Response_Type_ID,'') when '' Then A.Response Else E.Response_Id End as Response,E.Response_Type_ID,A.Question_ID,CASE ISNULL (E.Response_Type_ID,'') when '' Then A.Response Else E.Response_Text End as ResponseText,V.Emp_Name,V.Emp_Code,U.UserName,V.Sales_Org_ID,H.Site,'0'  AS ISDefault,A.Survey_Id,A.SalesRep_Id,(SELECT Top 1 SalesRep_Name from TBL_FSR WHERE SalesRep_Id=A.SalesRep_Id)AS SalesRepName from TBL_Survey_Audit_Responses as A INNER JOIN TBL_Survey as B ON A.Survey_ID=B.Survey_ID INNER JOIN TBL_Survey_Questions as D on A.Question_ID=D.Question_ID  LEFT OUTER JOIN TBL_Survey_Responses as E ON A.Question_Id=E.question_id AND A.Response=CAST(E.Response_id AS Varchar) AND E.Response_type_id=1  LEFT OUTER JOIN TBL_Van_Info V  on V.Emp_Code=A.Emp_Code LEFT OUTER JOIN TBL_User U on U.User_ID=A.Surveyed_BY  LEFT OUTER JOIN dbo.TBL_Org_CTL_H H on H.ORG_HE_ID=V.Sales_Org_ID WHERE 1=1 and  Survey_Timestamp=(select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses  A Where Status='N' AND  1=1 and {0}     ORDER BY Survey_Timestamp DESC) AND  Survey_Timestamp>= CAST(ISNULL((select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses  A Where Status='C' AND  1=1 and {0}  ORDER BY Survey_Timestamp DESC),'01-01-1900') AS Datetime)  and A.Status='N' AND {0} ORDER BY A.Question_ID", QueryStr)
            '            Dim QueryString As String = String.Format("select  A.SalesRep_ID,A.Survey_Timestamp ,A.Response AS ResText, B.Survey_Title,D.Question_Text, E.Response_text as Response,E.Response_Type_ID,A.Question_ID, A.Response as ResponseText,V.Emp_Name,V.Emp_Code,U.UserName,V.Sales_Org_ID,H.Site,(CASE WHEN CAST(E.Response_type_id AS VARCHAR)<>'1' AND A.Response= E.Response_Id THEN '1' ELSE '0' End)  AS ISDefault,A.Survey_Id,A.SalesRep_Id,(SELECT Top 1 SalesRep_Name from TBL_FSR WHERE SalesRep_Id=A.SalesRep_Id)AS SalesRepName from TBL_Survey_Audit_Responses as A INNER JOIN TBL_Survey as B ON A.Survey_ID=B.Survey_ID INNER JOIN TBL_Survey_Questions as D on A.Question_ID=D.Question_ID  LEFT OUTER JOIN TBL_Survey_Responses as E ON A.Question_Id=E.question_id LEFT OUTER JOIN TBL_Van_Info V  on V.Emp_Code=A.Emp_Code LEFT OUTER JOIN TBL_User U on U.User_ID=A.Surveyed_BY  LEFT OUTER JOIN dbo.TBL_Org_CTL_H H on H.ORG_HE_ID=V.Sales_Org_ID WHERE (CASE WHEN  CASt(A.Response AS VARCHAR)=CAST(E.Response_ID AS Varchar) THEN E.Response_type_id ELSE '1' END)<>'1'  AND   1=1 and  Survey_Timestamp=(select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses  A Where Status='N' AND  1=1 and {0}       ORDER BY Survey_Timestamp DESC) AND  Survey_Timestamp>= CAST(ISNULL((select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses  A Where Status='C' AND  1=1 and {0}     ORDER BY Survey_Timestamp DESC),'01-01-1900') AS Datetime)   and A.Status='N'  and {0}     UNION ALL select  A.SalesRep_ID,A.Survey_Timestamp ,A.Response AS ResText, B.Survey_Title,D.Question_Text, A.Response  as Response,E.Response_Type_ID,A.Question_ID,A.Response as ResponseText,V.Emp_Name,V.Emp_Code,U.UserName,V.Sales_Org_ID,H.Site,'0'  AS ISDefault,A.Survey_Id,A.SalesRep_Id,(SELECT Top 1 SalesRep_Name from TBL_FSR WHERE SalesRep_Id=A.SalesRep_Id)AS SalesRepName from TBL_Survey_Audit_Responses as A INNER JOIN TBL_Survey as B ON A.Survey_ID=B.Survey_ID INNER JOIN TBL_Survey_Questions as D on A.Question_ID=D.Question_ID  LEFT OUTER JOIN TBL_Survey_Responses as E ON A.Question_Id=E.question_id  AND E.Response_type_id='1'  LEFT OUTER JOIN TBL_Van_Info V  on V.Emp_Code=A.Emp_Code LEFT OUTER JOIN TBL_User U on U.User_ID=A.Surveyed_BY  LEFT OUTER JOIN dbo.TBL_Org_CTL_H H on H.ORG_HE_ID=V.Sales_Org_ID WHERE 1=1 and  Survey_Timestamp=(select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses  A Where Status='N' AND  1=1 and {0}    ORDER BY Survey_Timestamp DESC) AND  Survey_Timestamp>= CAST(ISNULL((select top 1 Survey_Timestamp from TBL_Survey_Audit_Responses  A Where Status='C' AND  1=1 and {0}     ORDER BY Survey_Timestamp DESC),'01-01-1900') AS Datetime)  and A.Status='N'  and {0}   AND E.Response_type_id='1' ORDER BY A.Question_ID", QueryStr)
            Dim QueryString As String = String.Format("select distinct year(checked_on) as yr  from TBL_Distribution_Check order by  year(checked_on)", QueryString)

            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetYear_Distribution = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function GetMinTransDate(ByRef Err_No As Long, ByRef Err_Desc As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable
        Dim sRetVal As String = ""
        Try

            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("select min(isnull(creation_date,'1900/01/01')) as creation_date from tbl_order", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text

            objSQLDA.Fill(dtDivConfig)
            objSQLDA.Dispose()
            If dtDivConfig.Rows.Count > 0 Then
                sRetVal = dtDivConfig.Rows(0)("creation_date").ToString
            End If
        Catch ex As Exception
            Err_No = "74206"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return sRetVal
    End Function

    Public Function GetOutlet(ByRef Err_No As Long, ByRef Err_Desc As String)
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "SELECT CONVERT(VARCHAR(25),cast(Customer_ID as varchar) + '$' + cast(Site_Use_ID as varchar)) AS CustomerID ,CONVERT(VARCHAR(200),Customer_No + ' - ' + Customer_Name) AS Outlet from TBL_Customer_Ship_Address order by Outlet asc"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustomerTbl")

            GetOutlet = MsgDs.Tables("CustomerTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74258"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    Public Function GetOutlet(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Orgid As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "select *,CONVERT(VARCHAR(25),cast(Customer_ID as varchar) + '$' + cast(Site_Use_ID as varchar)) AS CustomerID ,CONVERT(VARCHAR(200), Customer_No + '-' + Customer_Name) AS Outlet from [app_GetOrgCustomerShipAddress]('" & Orgid & "') order by Customer_No"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustomerTbl")

            GetOutlet = MsgDs.Tables("CustomerTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74258"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function

    Public Function GetSKU(ByRef Err_No As Long, ByRef Err_Desc As String)
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "SELECT Inventory_Item_ID , CONVERT(VARCHAR(200),Item_No + ' - ' + Description)AS SKU FROM TBL_Product  ORDER BY SKU"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustomerTbl")

            GetSKU = MsgDs.Tables("CustomerTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74258"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetSalesOrgbyFsr(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRep_ID As String)
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String

            QueryString = "SELECT MAS_Org_ID from TBL_Org_CTL_DTL where SalesRep_ID=" & SalesRep_ID

            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustomerTbl")

            GetSalesOrgbyFsr = MsgDs.Tables("CustomerTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74258"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetSKU(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_ID As String)
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String
            If Org_ID = "0" Then
                QueryString = "SELECT Inventory_Item_ID , CONVERT(VARCHAR(200),Item_No + ' - ' + Description)AS SKU FROM TBL_Product ORDER BY SKU"
            Else
                QueryString = "SELECT Inventory_Item_ID , CONVERT(VARCHAR(200),Item_No + ' - ' + Description)AS SKU FROM TBL_Product where Organization_ID='" & Org_ID & "'  ORDER BY SKU"
            End If
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustomerTbl")

            GetSKU = MsgDs.Tables("CustomerTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74258"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetAgencyList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_id As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String
            If Org_id <> "0" Then
                QueryString = String.Format("Select distinct Agency From tbl_product where Organization_ID='" & Org_id & "' order by Agency")
            Else
                QueryString = String.Format("Select distinct Agency From tbl_product  order by Agency")
            End If
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "PrdLstTbl")

            GetAgencyList = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74061"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    Public Function GetProductsByOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByRef OrgID As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String
            QueryString = String.Format(" Select Inventory_Item_ID,item_code +'-'+ Description as Description,cast(Inventory_Item_ID as varchar)+'$'+item_code as item_code , 0 as Qty, 0 as MAS_Org_ID  from TBL_Product Where Organization_ID='{0}' ", OrgID)

            QueryString = QueryString & " ORDER BY item_no ASC"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "Product")

            GetProductsByOrg = MsgDs.Tables("Product")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function GetProductsByOrg_Agency(ByRef Err_No As Long, ByRef Err_Desc As String, ByRef OrgID As Integer, Optional ByVal Agency As String = "0") As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String
            QueryString = String.Format(" Select Inventory_Item_ID,item_code +'-'+ Description as Description,cast(Inventory_Item_ID as varchar)+'$'+item_code as item_code  from TBL_Product Where Organization_ID='{0}' ", OrgID, Agency)
            If Agency <> "0" Then
                QueryString = QueryString & " and Agency in(" & Agency & ")"
            End If
            QueryString = QueryString & " ORDER BY item_no ASC"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "Product")

            GetProductsByOrg_Agency = MsgDs.Tables("Product")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetProductsByOrgFromAgencyCategory(ByRef Err_No As Long, ByRef Err_Desc As String, ByRef OrgID As Integer, Optional ByVal Agency As String = "0", Optional ByVal Category As String = "0") As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String
            QueryString = String.Format(" Select Inventory_Item_ID,item_code +'-'+ Description as Description,cast(Inventory_Item_ID as varchar)+'$'+item_code as item_code  from TBL_Product Where Organization_ID='{0}' ", OrgID)
            If Agency <> "0" And Agency <> "" Then
                QueryString = QueryString & " and Agency in(" & Agency & ")"
            End If
            If Category <> "0" And Category <> "" Then
                QueryString = QueryString & " and Category in(" & Category & ")"
            End If
            QueryString = QueryString & " ORDER BY item_no ASC"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "Product")

            GetProductsByOrgFromAgencyCategory = MsgDs.Tables("Product")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetProductsByOrg_Agency_UOM(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As Integer, Optional ByVal Agency As String = "0", Optional ByVal UOM As String = "0") As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String
            QueryString = String.Format(" Select Inventory_Item_ID,TBL_Product.item_code +'-'+ Description as Description,TBL_Product.item_code from TBL_Product inner join TBL_Item_UOM on TBL_Product.Item_Code=TBL_Item_UOM.Item_Code Where TBL_Product.Organization_ID='{0}' ", OrgID, Agency)
            If Agency <> "0" Then
                QueryString = QueryString & " and Agency='" & Agency & "'"
            End If
            If UOM <> "0" Then
                QueryString = QueryString & " and Item_UOM='" & UOM & "'"
            End If
            QueryString = QueryString & " ORDER BY item_no ASC"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "Product")

            GetProductsByOrg_Agency_UOM = MsgDs.Tables("Product")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetCustomerSegments(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SID As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String

            QueryString = "Rep_GetCustSegment '" & SID & "'"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "Product")

            GetCustomerSegments = MsgDs.Tables("Product")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function GetYear(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String

            QueryString = "select distinct year(Start_date) as Yr from TBL_Route_Plan_Default order by year(Start_date)"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "Year")

            GetYear = MsgDs.Tables("Year")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74158"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetYearForRevDispersion(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String

            QueryString = "select distinct year(Creation_Date) as Yr from TBL_Order where Creation_Date is not null order by  year(Creation_Date)"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "Year")

            GetYearForRevDispersion = MsgDs.Tables("Year")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74158"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function GetYearForReceivables(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String

            QueryString = "Select distinct A.Yr from (select   year(Creation_Date) as Yr from TBL_Order UNION select  year(Creation_Date) as Yr from TBL_RMA UNION select  year(Collected_On) as Yr from TBL_Collection)A"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "Year")

            GetYearForReceivables = MsgDs.Tables("Year")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74158"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetFsrCustRelation(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String

            QueryString = "Select * from TBL_App_Control Where Control_Key='DIRECT_FSR_CUST_REL'"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs)

            GetFsrCustRelation = MsgDs.Tables(0)
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74158"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetSummaryforExport(ByRef Err_No As Long, ByRef Err_Desc As String, ByRef Fromdate As String, ByVal Todate As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim objSQLDA As SqlDataAdapter
        Dim ds As New DataSet
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("rep_GetSummary", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@fromdate", Fromdate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Todate", Todate)

            objSQLDA.Fill(ds)
            objSQLDA.Dispose()
            Return ds
        Catch ex As Exception
            Err_No = "74061"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function

    Public Function GetYearforMonthlySales(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String

            QueryString = "Select distinct A.Yr from (select   year(Creation_Date) as Yr from TBL_Order union all select   year(Creation_Date) as Yr from TBL_rma ) A ORDER BY YR"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "Year")

            GetYearforMonthlySales = MsgDs.Tables("Year")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74158"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    Public Function GetYearforOrder(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String

            QueryString = "Select distinct A.Yr from (select   year(Creation_Date) as Yr from TBL_Order ) A ORDER BY YR"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "Year")

            GetYearforOrder = MsgDs.Tables("Year")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74158"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetYearforLoad(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String

            QueryString = "Select distinct A.Yr from (select   year(transfer_date) as Yr from TBL_Stock_Transfer where Transfer_Description like 'Van Load%' ) A ORDER BY YR"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "Year")

            GetYearforLoad = MsgDs.Tables("Year")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74158"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetArea(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String

            QueryString = "Select distinct Address from tbl_Customer where isnull(Address,'')<>'' ORDER BY Address"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "Year")

            GetArea = MsgDs.Tables("Year")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74158"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetCustomerLocationByOrg(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "SELECT DISTINCT CASE WHEN Location IS NULL or Location='' THEN 'N/A' ELSE Location END AS Location,CASE WHEN Location IS NULL or Location='' THEN 'N/A' ELSE Location END as Location from dbo.app_GetOrgCustomers ('" & OrgID & "')"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustomerTbl")

            GetCustomerLocationByOrg = MsgDs.Tables("CustomerTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetCustomerfromOrg_Loc(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Location As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "SELECT A.Customer_ID  as CustomerID,'['+IsNULL(A.Customer_No,'N/A')+']-'+IsNULL(A.Customer_Name,'N/A') as Customer from dbo.app_GetOrgCustomerShipAddress('" & OrgID & "') A INNER JOIN  app_GetOrgCustomers('" & OrgID & "') B on A.Customer_ID=B.Customer_ID where 1=1"
            If Location <> "-1" Then
                QueryString = QueryString & " and Isnull(B.Location,'N/A')in(" & Location & ")"
            End If
            QueryString = QueryString & "  order by A.Customer_No"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetCustomerfromOrg_Loc = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function


    Public Function GetCountry(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QueryStr As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("select Distinct Country,Country +'$'+CurrencyCode+'$'+CAST(DecimalDigits AS varchar)AS Mas_Org_ID from dbo.app_GetOrgCountry({0}) ORDER BY Country Desc", QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")

            GetCountry = MsgDs.Tables(0)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "740791"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    Public Function GetProductsByOrg_AgencyNew(ByRef Err_No As Long, ByRef Err_Desc As String, ByRef OrgID As Integer, Optional ByVal Agency As String = "0") As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String
            QueryString = String.Format(" Select Inventory_Item_ID,item_code +'-'+ Description as Description,cast(Inventory_Item_ID as varchar)+'$'+item_code as item_code  from TBL_Product Where Organization_ID='{0}' ", OrgID, Agency)
            If Agency <> "0" And Agency <> "" Then
                QueryString = QueryString & " and Agency in('" & Agency & "')"
            End If
            QueryString = QueryString & " ORDER BY item_no ASC"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "Product")

            GetProductsByOrg_AgencyNew = MsgDs.Tables("Product")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function LoadAgency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "SELECT distinct Agency from tbl_product where isnull(Agency,'')<>'' and Organization_ID='" & OrgID & "'"

            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            LoadAgency = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function



    Public Function LoadCustomerType(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim custtype_Dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "SELECT * FROM  TBL_App_Codes WHERE Code_Type='CUSTOMER_TYPE'"

            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text


            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(custtype_Dt)


            objSQLCmd.Dispose()
            Return custtype_Dt
        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
            Return custtype_Dt
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function LoadCustomerClass(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim custclass_Dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "SELECT * FROM  TBL_App_Codes WHERE Code_Type='CUSTOMER_CLASS'"

            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text


            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(custclass_Dt)


            objSQLCmd.Dispose()
            Return custclass_Dt
        Catch ex As Exception
            Err_No = "74059"
            Err_Desc = ex.Message
            Return custclass_Dt
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function CheckCustNOGeneration(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim Dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "SELECT * FROM  TBL_App_Control WHERE  CONTROL_KEY='GENERATE_CUSTOMER_NO'"

            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text


            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(Dt)


            objSQLCmd.Dispose()
            Return Dt
        Catch ex As Exception
            Err_No = "74060"
            Err_Desc = ex.Message
            Return Dt
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function


    Public Function CheckCustOutStanding(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustID As String, ByVal SiteID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim Dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "SELECT SUM(isnull(Pending_Amount,0)) FROM dbo.TBL_Open_Invoices WHERE Customer_ID=" + CustID + " AND Site_Use_ID=" + SiteID + ""

            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text


            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(Dt)


            objSQLCmd.Dispose()
            Return Dt
        Catch ex As Exception
            Err_No = "74061"
            Err_Desc = ex.Message
            Return Dt
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetResponsesTextByID(ByVal ResponseID As String) As String

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dts As New DataTable
        Dim rslt As String = ""
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT * from TBL_Survey_Responses  WHERE Response_ID=@ResponseID", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ResponseID", ResponseID)
            objSQLDA.Fill(dts)
            If dts.Rows.Count > 0 Then
                rslt = dts.Rows(0)("Response_Text").ToString()
            End If

            objSQLDA.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetResponsesTextByID = rslt
    End Function

    Public Function GetCreditDetailUsers(ByRef Err_No As Long, ByRef Err_Desc As String, Org As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("SELECT User_ID, Username FROM  TBL_User WHERE Is_SS = 'A' OR ( Is_SS <> 'N' AND Org_HE_ID = @Org) ORDER BY Username")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@Org", Org)

            Dim UserDs As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(UserDs)
            Dim r As DataRow = UserDs.NewRow()
            r(0) = "0"
            r(1) = "Select User"

            UserDs.Rows.InsertAt(r, 0)
            GetCreditDetailUsers = UserDs
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "174091"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    'Task1 Rakesh


    Public Function GetSyncTypeForSync(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim Dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "select control_value  from TBL_App_Control where Control_Key='ENABLE_BACKGROUND_SYNC'"

            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text


            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(Dt)


            objSQLCmd.Dispose()
            Return Dt
        Catch ex As Exception
            Err_No = "74060"
            Err_Desc = ex.Message
            Return Dt
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function InsertUpdateBackSyncSettings(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal RowID As String, ByVal BacksyncValue As String, ByVal DefaultStateValue As String, ByVal SalePerid As String, ByVal CreatedBy As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String


        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            Dim sb As StringBuilder = New StringBuilder()
            sb.Append("INSERT INTO Customers(Name, Country)")
            sb.Append("VALUES ( @Name , @Country)")

            sb.Append("if exists (select * from TBL_Custom_Info where Info_Type='USER_BACKGROUND_SYNC_CONFIG' and Info_Key='vancs01' )")



            sQry = "UPDATE TBL_FSR_Device_Config  SET Config_Value=@Value ,Last_Updated_At=GetDate(),Last_Updated_By=@CreatedBy WHERE Row_ID=@RowID"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@RowID", RowID)
            objSQLCmd.Parameters.AddWithValue("@Value", BacksyncValue)
            objSQLCmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()



            sucess = True
        Catch ex As Exception
            Error_No = 75014

        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return sucess

    End Function


    Public Function InsertUpdateBackSyncSettings1(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal RowID As String, ByVal BacksyncValue As String, ByVal DefaultStateValue As String, ByVal SalePerid As String, ByVal CreatedBy As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("InsertUpdateUserWiseSyncSettings", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@SalesRep", SqlDbType.VarChar))
            objSQLCmd.Parameters("@SalesRep").Value = SalePerid
            objSQLCmd.Parameters.Add(New SqlParameter("@BacksyncStatus", SqlDbType.VarChar))
            objSQLCmd.Parameters("@BacksyncStatus").Value = BacksyncValue
            objSQLCmd.Parameters.Add(New SqlParameter("@DefaultStatus", SqlDbType.VarChar))
            objSQLCmd.Parameters("@DefaultStatus").Value = DefaultStateValue
            objSQLCmd.Parameters.Add(New SqlParameter("@UpdatedBy", SqlDbType.VarChar))
            objSQLCmd.Parameters("@UpdatedBy").Value = CreatedBy

            Dim iRows As Integer
            iRows = objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
            If iRows > 0 Then
                bRetVal = True
            Else
                bRetVal = True
            End If
        Catch ex As Exception
            'Err_No = "74206"
            'Err_Desc = ex.Message
            'Log.Debug(ex.ToString)
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function

    Public Function GetUserSyncConfig(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRepID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            'Dim QueryString As String = String.Format("SELECT Row_ID, A.Info_Key,A.Value_1 Config_Type,A.Value_2 Config_Value,F.SalesRep_name FROM TBL_Custom_Info A   inner join tbl_fsr F  on cast( F.SalesRep_ID as varchar) =A.Value_3     WHERE A.Value_3 in ( select SalesRep_ID from tbl_fsr where SalesRep_ID in (Select item from dbo.SplitQuotedString(@SID)))")
            Dim QueryString As String = String.Format("select fs.SalesRep_ID Row_ID,isnull(cf.Info_Key,'N') as Info_Key,isnull(cf.Value_1,'N') as Config_Type,isnull(cf.Value_2,'N') as Config_Value , fs.salesRep_Name as SalesRep_name from tbl_fsr fs Full Join TBL_Custom_Info cf on fs.SalesRep_Number=cf.Info_Key and cf.Info_Type='USER_BACKGROUND_SYNC_CONFIG' where fs.SalesRep_ID in (Select item from dbo.SplitQuotedString(@SID)) ")



            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@SID", SalesRepID)

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetUserSyncConfig = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function GetDeviceConfig1(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRepID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            'SELECT Row_ID,'Y' Config_Type,Config_Key, 'Y' Config_Value,F.SalesRep_name FROM TBL_FSR_Device_Config A inner join tbl_fsr F on F.SalesRep_ID=A.SalesRep_ID  WHERE A.Info_Key  in (select SalesRep_Number from tbl_fsr where SalesRep_ID  in (Select item from dbo.SplitQuotedString(@SID))  and Info_Type='USER_BACKGROUND_SYNC_CONFIG')
            Dim QueryString As String = String.Format("SELECT Row_ID,'Y' Config_Type,Config_Key, 'Y' Config_Value,F.SalesRep_name FROM TBL_FSR_Device_Config A inner join tbl_fsr F on F.SalesRep_ID=A.SalesRep_ID  WHERE A.Info_Key  in (select SalesRep_Number from tbl_fsr where SalesRep_ID  in (Select item from dbo.SplitQuotedString(@SID))  and Info_Type='USER_BACKGROUND_SYNC_CONFIG')")
            ' Dim QueryString As String = String.Format("SELECT Row_ID,'Y' Config_Type,Config_Key, 'Y' Config_Value,F.SalesRep_name FROM TBL_FSR_Device_Config A inner join tbl_fsr F on F.SalesRep_ID=A.SalesRep_ID WHERE A.SalesRep_ID in (Select item from dbo.SplitQuotedString(@SID))")


            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@SID", SalesRepID)

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetDeviceConfig1 = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function GetDefaultBackSyncSettings1(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRepID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Dim Dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("getUserWiseSyncSettings", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@SID", SqlDbType.VarChar))
            objSQLCmd.Parameters("@SID").Value = SalesRepID

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")
            Dt = MsgDs.Tables("VanTbl")


            objSQLCmd.Dispose()
            Return Dt

        Catch ex As Exception
            'Err_No = "74206"
            'Err_Desc = ex.Message
            'Log.Debug(ex.ToString)
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    Public Function GetCustomerfromOrgForOrderLvlFOC(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "SELECT (LTRIM(STR(Customer_ID)) + '$' + LTRIM(STR(Site_Use_ID))) as CustomerID,'['+IsNULL(Customer_No,'N/A')+']-'+IsNULL(Customer_Name,'N/A') as Customer from dbo.app_GetOrgCustomers ('" & OrgID & "') A"
            QueryString = QueryString & " where not exists(Select 1 from TBL_Customer_Addl_Info adl where a.Customer_ID=adl.Customer_ID and A.Site_use_ID=adl.Site_use_ID and adl.Attrib_Name='CUST_ORDER_DISC_LIMIT')"

            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetCustomerfromOrgForOrderLvlFOC = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetSyncTypeForDeviceSync(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("select distinct Sync_Type  from TBL_Device_DB_Sync_Log ORDER BY Sync_Type ASC")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "SyncTypeTbl")

            GetSyncTypeForDeviceSync = MsgDs.Tables("SyncTypeTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74059"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetvalueForPushNotification(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ConfigKey As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
 
        Try
            objSQLConn = _objDB.GetSQLConnection
        'Dim QueryString As String = String.Format("SELECT Row_ID, A.Info_Key,A.Value_1 Config_Type,A.Value_2 Config_Value,F.SalesRep_name FROM TBL_Custom_Info A   inner join tbl_fsr F  on cast( F.SalesRep_ID as varchar) =A.Value_3     WHERE A.Value_3 in ( select SalesRep_ID from tbl_fsr where SalesRep_ID in (Select item from dbo.SplitQuotedString(@SID)))")
        Dim QueryString As String = String.Format("select * from tbl_app_Codes where code_type=@configKey  ")



        objSQLCmd = New SqlCommand(QueryString, objSQLConn)
        objSQLCmd.CommandType = CommandType.Text
        objSQLCmd.Parameters.AddWithValue("@configKey", ConfigKey)

        Dim MsgDs As New DataSet
        Dim SqlAd As SqlDataAdapter
        SqlAd = New SqlDataAdapter(objSQLCmd)
        SqlAd.Fill(MsgDs, "VanTbl")

        GetvalueForPushNotification = MsgDs.Tables("VanTbl")
        objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetVanfromSalesRepID(ByRef Err_No As Long, ByRef Err_Desc As String, SalesrepID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT * FROM TBL_FSR WHERE SalesRep_ID=" & SalesrepID)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    'task 1 Product Stock
    Public Function GetProductsByOrgById(ByRef Err_No As Long, ByRef Err_Desc As String, ByRef OrgID As Integer, ByRef ProductId As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String
            If ProductId = "0" Then
                ' QueryString = String.Format(" Select Inventory_Item_ID,item_code +'-'+ Description as Description,cast(Inventory_Item_ID as varchar)+'$'+item_code as item_code , 0 as Qty, 0 as MAS_Org_ID  from TBL_Product Where Organization_ID='{0}' ", OrgID)
                QueryString = String.Format("Select b.Inventory_Item_ID,item_code +'-'+ Description as Description,cast(b.Inventory_Item_ID as varchar)+'$'+item_code as item_code , b.[Attrib_Value] as Qty, b.Organization_ID as MAS_Org_ID ,item_code ICode  from [TBL_Product_Addl_Info]  b  join  [dbo].TBL_Product a on a.Inventory_Item_ID=b.Inventory_Item_ID  where  b.[Attrib_Name]='MS' and b.Organization_ID='{0}' ", OrgID)

            Else
                'QueryString = String.Format(" Select Inventory_Item_ID,item_code +'-'+ Description as Description,cast(Inventory_Item_ID as varchar)+'$'+item_code as item_code , 0 as Qty, 0 as MAS_Org_ID  from TBL_Product Where Organization_ID='{0}' and item_code='{1}' ", OrgID, ProductId)
                QueryString = String.Format("Select b.Inventory_Item_ID,item_code +'-'+ Description as Description,cast(b.Inventory_Item_ID as varchar)+'$'+item_code as item_code , b.[Attrib_Value] as Qty, b.Organization_ID as MAS_Org_ID ,item_code ICode from [TBL_Product_Addl_Info]  b  join  [dbo].TBL_Product a on a.Inventory_Item_ID=b.Inventory_Item_ID  where  b.[Attrib_Name]='MS' and b.Organization_ID='{0}'and a.item_code='{1}' ", OrgID, ProductId)

            End If


            QueryString = QueryString & " ORDER BY item_no ASC"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "Product")

            GetProductsByOrgById = MsgDs.Tables("Product")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function

    Public Function GetProductsDetailsByOrgById(ByRef Err_No As Long, ByRef Err_Desc As String, ByRef OrgID As Integer, ByRef ProductId As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String
            If ProductId = "0" Then
                QueryString = String.Format(" Select Inventory_Item_ID,item_code +'-'+ Description as Description,cast(Inventory_Item_ID as varchar)+'$'+item_code as item_code , 0 as Qty, 0 as MAS_Org_ID  from TBL_Product Where Organization_ID='{0}' ", OrgID)

            Else
                QueryString = String.Format(" Select Inventory_Item_ID,item_code +'-'+ Description as Description,cast(Inventory_Item_ID as varchar)+'$'+item_code as item_code , 0 as Qty, 0 as MAS_Org_ID  from TBL_Product Where Organization_ID='{0}' and item_code='{1}' ", OrgID, ProductId)

            End If


            QueryString = QueryString & " ORDER BY item_no ASC"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "Product")

            GetProductsDetailsByOrgById = MsgDs.Tables("Product")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function

    Public Function SaveProductMinimumStock(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal OrgId As String, ByVal SID As String, ByVal Qty As String, ByVal CreatedBy As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try

            objSQLConn = _objDB.GetSQLConnection
            sQry = "app_SaveProductMinimumQty"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLCmd.Parameters.AddWithValue("@InvId", SID)
            objSQLCmd.Parameters.AddWithValue("@Qty", Qty)



            'Dim MsgDs As New DataSet
            'Dim SqlAd As SqlDataAdapter
            'SqlAd = New SqlDataAdapter(objSQLCmd)
            'SqlAd.Fill(MsgDs, "DisCtlTbl")
            'SaveDistribution_CTL = MsgDs.Tables(0)
            'objSQLCmd.Dispose()

            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()

            sucess = True
        Catch ex As Exception
            Error_No = 75014
            ' Error_Desc = String.Format("Error while saving Order", ex.Message)

        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return sucess

    End Function

    Public Function DeleteProductMinimumStock(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal OrgId As String, InventoryItemId As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try

            objSQLConn = _objDB.GetSQLConnection


            sQry = "delete from TBL_Product_Addl_Info where Attrib_Name='MS' and Inventory_Item_ID=@InventoryItemId "

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@InventoryItemId", InventoryItemId)
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()

            sucess = True
        Catch ex As Exception
            Error_No = 75014
            ' Error_Desc = String.Format("Error while saving Order", ex.Message)

        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return sucess

    End Function

    Public Function GetCustfromOrgtext_Distribution_ctl(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal SID As String, Optional text As String = "") As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            Dim QueryString As String
            objSQLConn = _objDB.GetSQLConnection

            QueryString = "SELECT DISTINCT (LTRIM(STR(Customer_ID)) + '$' + LTRIM(STR(Site_Use_ID))) as CustomerID,'['+IsNULL(Customer_No,'N/A')+']-'+IsNULL(Customer_Name,'N/A') as Customer from V_FSR_CustomerShipAddress V  " &
                          " INNER JOIN  TBL_Org_CTL_DTL C ON C.SalesRep_ID=V.SalesRep_ID WHERE MAS_Org_ID ='" & OrgID & "' "

            If SID.Trim() <> "0" And SID.Trim() <> "" Then
                QueryString = QueryString & " AND  V.SalesRep_ID ='" & SID & "' "
            End If

            If text <> "" Then
                QueryString = QueryString & " AND  (Customer_no LIKE '%' + @txt + '%' OR Customer_no +'-'+ Customer_name LIKE '%' + @txt + '%')"
            End If
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            If text <> "" Then
                objSQLCmd.Parameters.Add("@txt", SqlDbType.VarChar, 100).Value = text
            End If
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanTbl")

            GetCustfromOrgtext_Distribution_ctl = MsgDs.Tables("VanTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function ValidateExportProductMinimumStock(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String) As DataTable
        Dim success As Boolean = False
        Dim objSQLConn As SqlConnection
        Dim objSQLCMD As SqlCommand
        Dim MsgDs As New DataSet
        Dim sql As String = ""
        sql = "select * from [dbo].[TBL_Product]  where Inventory_Item_ID='" & (ItemCode.Trim()) & "'"

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCMD = New SqlCommand(sql, objSQLConn)
            objSQLCMD.CommandType = CommandType.Text

            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCMD)
            SqlAd.Fill(MsgDs, "TBL_FSR")

        Catch ex As Exception

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return MsgDs.Tables(0)
    End Function
    Public Function ValidateExportOrganisationId(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String) As DataTable
        Dim success As Boolean = False
        Dim objSQLConn As SqlConnection
        Dim objSQLCMD As SqlCommand
        Dim MsgDs As New DataSet
        Dim sql As String = ""
        sql = "select * from [dbo].[TBL_Product]  where Organization_ID='" & (OrgId.Trim()) & "'"

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCMD = New SqlCommand(sql, objSQLConn)
            objSQLCMD.CommandType = CommandType.Text

            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCMD)
            SqlAd.Fill(MsgDs, "TBL_FSR")

        Catch ex As Exception

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return MsgDs.Tables(0)
    End Function


End Class
