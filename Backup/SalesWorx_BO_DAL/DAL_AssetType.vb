Imports System.Data.SqlClient
Imports System.Configuration

Public Class DAL_AssetType
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Private _objDB As DatabaseConnection
    Private Shared _strSQLConn As String = ConfigurationSettings.AppSettings("SQLConnString")
    ' Private dtCurrency As New DataTable

    Public Function DeleteAssetType(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal AssetTypeId As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sRetVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("app_DeleteAssetType", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@AssetTypeId", SqlDbType.Int))
            objSQLCmd.Parameters("@AssetTypeId").Value = AssetTypeId
            objSQLCmd.ExecuteNonQuery()
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
        DeleteAssetType = sRetVal
    End Function

    Public Function DeleteAssets(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal AssetID As String, ByVal UserID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sRetVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            '            objSQLCmd = New SqlCommand("UPDATE TBL_Assets SET Is_Deleted='Y',Last_Updated_At=GETDATE(),Last_Updated_By=@UserID WHERE Asset_ID=@AssetID", objSQLConn)
            objSQLCmd = New SqlCommand("DELETE TBL_Asset_History  WHERE Asset_ID=@AssetID;DELETE TBL_Assets  WHERE Asset_ID=@AssetID", objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@UserID", UserID)
            objSQLCmd.Parameters.AddWithValue("@AssetId", AssetID)
            objSQLCmd.ExecuteNonQuery()
            sRetVal = True
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "10215"
            Err_Desc = ex.Message
            sRetVal = False
            Throw ex
        Finally
            objSQLCmd = Nothing
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        DeleteAssets = sRetVal
    End Function
    Public Function CheckExist(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal AssetTypeid As Integer) As Boolean
        Dim success As Boolean = False
        Dim objSQLConn As SqlConnection
        Dim objSQLCMD As SqlCommand
        Dim Rcnt As Integer
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCMD = New SqlCommand("Select COUNT(*) FROm TBL_Assets  WHERE Asset_Type_id=@AssetTypeid", objSQLConn)
            objSQLCMD.Parameters.Add(New SqlParameter("@AssetTypeid", SqlDbType.Int))
            objSQLCMD.Parameters("@AssetTypeid").Value = AssetTypeid
            Rcnt = Convert.ToInt32(objSQLCMD.ExecuteScalar())
            objSQLCMD.Dispose()
            If Rcnt > 0 Then
                success = True
            End If
        Catch ex As Exception
            Err_No = "140023"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
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

    Public Function InsertAssetType(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Description As String, ByVal userid As Integer, ByVal Param1 As String, ByVal param2 As String, ByVal Param3 As String, ByVal Param4 As Integer, ByVal param5 As Decimal) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_InsertAssetType", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@Description", SqlDbType.NVarChar, 100))
            objSQLCmd.Parameters("@Description").Value = Description
            objSQLCmd.Parameters.Add(New SqlParameter("@CustomAttribute1", SqlDbType.NVarChar, 1000))
            objSQLCmd.Parameters("@CustomAttribute1").Value = Param1
            objSQLCmd.Parameters.Add(New SqlParameter("@CustomAttribute2", SqlDbType.NVarChar, 1000))
            objSQLCmd.Parameters("@CustomAttribute2").Value = param2
            objSQLCmd.Parameters.Add(New SqlParameter("@CustomAttribute3", SqlDbType.NVarChar, 1000))
            objSQLCmd.Parameters("@CustomAttribute3").Value = Param3
            objSQLCmd.Parameters.Add(New SqlParameter("@CustomAttribute4", SqlDbType.BigInt))
            objSQLCmd.Parameters("@CustomAttribute4").Value = Param4
            objSQLCmd.Parameters.Add(New SqlParameter("@CustomAttribute5", SqlDbType.Decimal))
            objSQLCmd.Parameters("@CustomAttribute5").Value = param5
            objSQLCmd.Parameters.Add(New SqlParameter("@ModifiedBy", SqlDbType.Int))
            objSQLCmd.Parameters("@ModifiedBy").Value = userid

            objSQLCmd.ExecuteNonQuery()
            bRetVal = True
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        Catch ex As Exception
            Err_No = "740019"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function
    Public Function GetCustomerandsiteId(ByVal CustNo As String, ByVal OrgID As String, ByRef Err_No As Long, ByRef Err_Desc As String) As DataRow
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dt As New DataTable
        Dim row As DataRow = Nothing
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("select Customer_ID,Site_Use_ID  from  dbo.app_GetOrgCustomerShipAddress(@OrgID) where Customer_No=@CustNo AND Custom_Attribute_1 =@OrgID ", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.SelectCommand.Parameters.Add("@CustNo", SqlDbType.VarChar).Value = CustNo
            objSQLDA.SelectCommand.Parameters.Add("@OrgID", SqlDbType.VarChar).Value = OrgID
            objSQLDA.Fill(dt)

            If dt.Rows.Count > 0 Then
                row = dt.Rows(0)

            End If
        Catch ex As Exception
            Err_No = "72532"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
            objSQLDA.Dispose()
            objSQLDA = Nothing
        End Try
        Return row
    End Function
    Public Function InsertAssets(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal userid As Integer, ByVal AssetType As Integer, ByVal CustID As Integer, ByVal SiteID As String, ByVal AssetCode As String, ByVal Description As String, ByVal IsActive As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_InsertAssets", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@CustID", CustID)
            objSQLCmd.Parameters.AddWithValue("@SiteID", SiteID)
            objSQLCmd.Parameters.AddWithValue("@AssetType", AssetType)
            objSQLCmd.Parameters.AddWithValue("@AssetCode", AssetCode)
            objSQLCmd.Parameters.AddWithValue("@Description", Description)
            objSQLCmd.Parameters.AddWithValue("@IsActive", IsActive)
            objSQLCmd.Parameters.AddWithValue("@UserID", userid)
            objSQLCmd.ExecuteNonQuery()
            bRetVal = True
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        Catch ex As Exception
            Err_No = "740013"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function

    Public Function UpdateAssets(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal AssetID As String, ByVal AssetType As Integer, ByVal userid As Integer, ByVal CustID As Integer, ByVal SiteID As String, ByVal AssetCode As String, ByVal Description As String, ByVal IsActive As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_UpdateAssets", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@AssetID", AssetID)
            objSQLCmd.Parameters.AddWithValue("@CustID", CustID)
            objSQLCmd.Parameters.AddWithValue("@SiteID", SiteID)
            objSQLCmd.Parameters.AddWithValue("@AssetType", AssetType)
            objSQLCmd.Parameters.AddWithValue("@AssetCode", AssetCode)
            objSQLCmd.Parameters.AddWithValue("@Description", Description)
            objSQLCmd.Parameters.AddWithValue("@IsActive", IsActive)
            objSQLCmd.Parameters.AddWithValue("@UserID", userid)

            objSQLCmd.ExecuteNonQuery()
            bRetVal = True
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        Catch ex As Exception
            Err_No = "344013"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function


    Public Function UpdateAssetType(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal AssetTypeId As Integer, ByVal Description As String, ByVal userid As Integer, ByVal Param1 As String, ByVal param2 As String, ByVal Param3 As String, ByVal Param4 As Integer, ByVal param5 As Decimal) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_UpdateAssetType", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@AssetTypeId", SqlDbType.Int))
            objSQLCmd.Parameters("@AssetTypeId").Value = AssetTypeId
            objSQLCmd.Parameters.Add(New SqlParameter("@Description", SqlDbType.NVarChar, 500))
            objSQLCmd.Parameters("@Description").Value = Description
            objSQLCmd.Parameters.Add(New SqlParameter("@CustomAttribute1", SqlDbType.NVarChar, 1000))
            objSQLCmd.Parameters("@CustomAttribute1").Value = Param1
            objSQLCmd.Parameters.Add(New SqlParameter("@CustomAttribute2", SqlDbType.NVarChar, 1000))
            objSQLCmd.Parameters("@CustomAttribute2").Value = param2
            objSQLCmd.Parameters.Add(New SqlParameter("@CustomAttribute3", SqlDbType.NVarChar, 1000))
            objSQLCmd.Parameters("@CustomAttribute3").Value = Param3
            objSQLCmd.Parameters.Add(New SqlParameter("@CustomAttribute4", SqlDbType.BigInt))
            objSQLCmd.Parameters("@CustomAttribute4").Value = Param4
            objSQLCmd.Parameters.Add(New SqlParameter("@CustomAttribute5", SqlDbType.Decimal))
            objSQLCmd.Parameters("@CustomAttribute5").Value = param5
            objSQLCmd.Parameters.Add(New SqlParameter("@ModifiedBy", SqlDbType.Int))
            objSQLCmd.Parameters("@ModifiedBy").Value = userid

            objSQLCmd.ExecuteNonQuery()
            bRetVal = True
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        Catch ex As Exception
            Err_No = "740019"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function

    Public Function FillAssetTypeGrid(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim h As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT *,(SELECT UserName FROM TBL_user WHERE user_ID=A.Last_Modified_By)AS UserName  FROM TBL_Asset_types AS A ORDER BY Asset_type_id", objSQLConn)
            objSQLDA.Fill(h)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "740021"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return h
    End Function
    Public Function GetAssetID(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal AssetID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim h As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT * FROM TBL_Assets WHERE Asset_ID=@AssetID", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.SelectCommand.Parameters.AddWithValue("@AssetID", AssetID)
            objSQLDA.Fill(h)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "12352"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return h
    End Function
    Public Function FillAssets(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim h As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_GetAssetsList", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgID)
            'objSQLDA.SelectCommand.Parameters.AddWithValue("@CustID", CustID)
            'objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", SiteID)
            objSQLDA.Fill(h)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "840021"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return h
    End Function

    Public Function CheckValidAssetTypeID(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal AssetTypeID As String) As Boolean
        Dim success As Boolean = False
        Dim objSQLConn As SqlConnection
        Dim objSQLCMD As SqlCommand
        Dim Rcnt As Integer
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim sqry As String = ""

          
            sqry = "SELECT COUNT(*) FROM TBL_Asset_types WHERE  Asset_Type_ID=@TypeID"

            objSQLCMD = New SqlCommand(sqry, objSQLConn)

            objSQLCMD.Parameters.AddWithValue("@TypeID", AssetTypeID)
            Rcnt = Convert.ToInt32(objSQLCMD.ExecuteScalar())
            objSQLCMD.Dispose()
            If Rcnt > 0 Then
                success = True
            End If
        Catch ex As Exception
            Err_No = "740023"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
    Public Function CheckDuplicate(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Descripton As String, ByVal AssetTypeID As String) As Boolean
        Dim success As Boolean = False
        Dim objSQLConn As SqlConnection
        Dim objSQLCMD As SqlCommand
        Dim Rcnt As Integer
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim sqry As String = ""

            If AssetTypeID = "" Or AssetTypeID = "0" Then
                sqry = "SELECT COUNT(*) FROM TBL_Asset_types WHERE Description=@Description"
            Else
                sqry = "SELECT COUNT(*) FROM TBL_Asset_types WHERE Description=@Description AND Asset_Type_ID<>@TypeID"
            End If
            objSQLCMD = New SqlCommand(sqry, objSQLConn)
            objSQLCMD.Parameters.AddWithValue("@Description", Descripton)
            objSQLCMD.Parameters.AddWithValue("@TypeID", AssetTypeID)
            Rcnt = Convert.ToInt32(objSQLCMD.ExecuteScalar())
            objSQLCMD.Dispose()
            If Rcnt > 0 Then
                success = True
            End If
        Catch ex As Exception
            Err_No = "740023"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function

    Public Function UploadAssetsToCustomer(ByVal dtData As DataTable, ByVal OrgID As String, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CreatedBy As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Dim tran As SqlTransaction = Nothing
        Dim QueryString As String = Nothing
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            tran = objSQLConn.BeginTransaction()


            For Each r As DataRow In dtData.Rows
                If r("IsValid").ToString() = "Y" Then

                    QueryString = "app_UploadAssets"
                    objSQLCmd = New SqlCommand(QueryString, objSQLConn)
                    objSQLCmd.CommandType = CommandType.StoredProcedure

                    objSQLCmd.Parameters.AddWithValue("@CustID", CInt(r("CustomerID").ToString()))
                    objSQLCmd.Parameters.AddWithValue("@SiteID", CInt(r("SiteID").ToString()))
                    objSQLCmd.Parameters.AddWithValue("@AssetType", CInt(r("AssetTypeID".ToString())))
                    objSQLCmd.Parameters.AddWithValue("@AssetCode", r("AssetCode").ToString())
                    objSQLCmd.Parameters.AddWithValue("@Description", r("Description").ToString())
                    objSQLCmd.Parameters.AddWithValue("@IsActive", "Y")
                    objSQLCmd.Parameters.AddWithValue("@UserID", CreatedBy)
                    objSQLCmd.Transaction = tran
                    objSQLCmd.ExecuteNonQuery()
                    objSQLCmd.Dispose()
                End If
            Next



            success = True
            tran.Commit()
        Catch ex As Exception
            Err_No = "75721"
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
    Public Function CheckAssetNo(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal AssetNo As String, ByVal AssetID As String) As Boolean
        Dim success As Boolean = False
        Dim objSQLConn As SqlConnection
        Dim objSQLCMD As SqlCommand
        Dim Rcnt As Integer
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim sqry As String = ""

            If AssetID = "" Or AssetID = "0" Then
                sqry = "SELECT COUNT(*) FROM TBL_Assets WHERE Asset_Code=@AssetNo"
            Else
                sqry = "SELECT COUNT(*) FROM TBL_Assets WHERE Asset_Code=@AssetNo AND Asset_ID<>@AssetID"
            End If
            objSQLCMD = New SqlCommand(sqry, objSQLConn)
            objSQLCMD.Parameters.AddWithValue("@AssetNo", AssetNo)
            objSQLCMD.Parameters.AddWithValue("@AssetID", AssetID)
            Rcnt = Convert.ToInt32(objSQLCMD.ExecuteScalar())
            objSQLCMD.Dispose()
            If Rcnt > 0 Then
                success = True
            End If
        Catch ex As Exception
            Err_No = "750423"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function


    Public Function DeleteAll(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sRetVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("DELETE FROM TBL_Asset_types", objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.ExecuteNonQuery()
            sRetVal = True
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74016"
            Err_Desc = ex.Message
            sRetVal = False
            Throw ex
        Finally
            objSQLCmd = Nothing
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        DeleteAll = sRetVal
    End Function

  

End Class
