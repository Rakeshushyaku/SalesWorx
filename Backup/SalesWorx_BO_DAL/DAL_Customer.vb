﻿Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Data
Public Class DAL_Customer
    Private _objDB As DatabaseConnection
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Public Function GetCustomersFromSWX(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal FilterName As String, ByVal FilterNo As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "app_ListCustomersFromSWX"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure

            objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            objSQLCmd.Parameters.AddWithValue("@FilterName", FilterName.Trim)
            objSQLCmd.Parameters.AddWithValue("@FilterNo", FilterNo.Trim)

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustLstTbl")

            GetCustomersFromSWX = MsgDs.Tables(0)
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

    Public Function GetCustomerOrderDiscount(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "Select b.*,CAST(ISNULL(A.Attrib_Value,'0') AS Decimal(18,4)) As MinOrderValue,CAST(ISNULL(A.Custom_Attribute_1,'0')AS  DECIMAL(18,4))*100.0 AS MinDisc,CAST(ISNULL(A.Custom_Attribute_2,'0')AS Decimal(18,4)) * 100.0 AS MaxDisc from TBL_Customer_Addl_Info A inner join app_GetOrgCustomers('" & OrgID & "') B on a.Customer_ID=B.Customer_ID and A.Site_Use_ID=B.Site_Use_ID where A.Attrib_Name='CUST_ORDER_DISC_LIMIT'"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustLstTbl")

            GetCustomerOrderDiscount = MsgDs.Tables(0)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24061"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    Public Function ExportCustomerBonusPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = Nothing


            QueryString = "SELECT A.Customer_No As CustomerNo,A.Site_Use_ID ,ISNULL(B.Bonus_Plan_ID,0) AS BonusPlanID FROM dbo.app_GetOrgCustomerShipAddress(@OrgID) AS A LEFT JOIN TBL_Customer_Bonus_Map AS B On A.Customer_ID =B.Customer_ID AND A.Site_Use_ID =B.Site_Use_ID "


            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID

            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "22068"
            Err_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function GetDiscountDefinition(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "Select b.*,A.Custom_Attribute_1 as DiscType,case A.Custom_Attribute_1 when 'P' then cast(Attrib_Value as decimal(18,2))*100 else cast(Attrib_Value as decimal(18,2)) end as Attrib_Value,Custom_Attribute_4 as minorder from TBL_Customer_Addl_Info A inner join app_GetOrgCustomers('" & OrgID & "') B on a.Customer_ID=B.Customer_ID and A.Site_Use_ID=B.Site_Use_ID where A.Attrib_Name='DISCOUNT'"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustLstTbl")

            GetDiscountDefinition = MsgDs.Tables(0)
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
     Public Function UploadDiscount(ByVal dtData As DataTable, ByVal OrgID As String, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Dim tran As SqlTransaction = Nothing
        Dim QueryString As String = Nothing
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            tran = objSQLConn.BeginTransaction()
            Dim objLogin As New SalesWorx.BO.DAL.DAL_Login

            For Each r As DataRow In dtData.Rows
                If r("IsValid").ToString() = "Y" Then



                    QueryString = "app_SaveCustomoerOrderlvlDiscount"
                    objSQLCmd = New SqlCommand(QueryString, objSQLConn)
                    objSQLCmd.CommandType = CommandType.StoredProcedure
                   objSQLCmd.Parameters.Add(New SqlParameter("@CustomerID", SqlDbType.Int))
                objSQLCmd.Parameters("@CustomerID").Value = r("Customer_ID").ToString()
                objSQLCmd.Parameters.Add(New SqlParameter("@SiteUSeID", SqlDbType.Int))
                objSQLCmd.Parameters("@SiteUSeID").Value = r("Site_use_ID").ToString()

                objSQLCmd.Parameters.Add(New SqlParameter("@DiscountType", SqlDbType.VarChar))
                objSQLCmd.Parameters("@DiscountType").Value = r("DiscountType").ToString()

               objSQLCmd.Parameters.Add(New SqlParameter("@MinOrdervalue", SqlDbType.Decimal))
                objSQLCmd.Parameters("@MinOrdervalue").Value = r("MinOrderValue").ToString()

                objSQLCmd.Parameters.Add(New SqlParameter("@Discount", SqlDbType.Decimal))
                objSQLCmd.Parameters("@Discount").Value = r("Discount").ToString()

                objSQLCmd.Parameters.Add("@Status", SqlDbType.Int)
                objSQLCmd.Parameters("@Status").Direction = ParameterDirection.Output
                    objSQLCmd.Transaction = tran
                    objSQLCmd.ExecuteNonQuery()


            Dim Status As String
                Status = objSQLCmd.Parameters("@Status").Value.ToString()
                 objSQLCmd.Dispose()
            If Status = "2" Then
                    objLogin.InsertUserLog(Err_No, Err_Desc, "U", "MAIN MENU", "ORDER LVL CUST DISC", r("Customer_ID").ToString() & "$" & r("Site_use_ID").ToString(), "Org Val: " & OrgID & " Order Val: " & r("MinOrderValue").ToString() & " Discount:" & r("Discount").ToString() & " Type:" & r("DiscountType").ToString(), UserID.ToString(), "0", "0")
            ElseIf Status = "1" Then
                    objLogin.InsertUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "ORDER LVL CUST DISC", r("Customer_ID").ToString() & r("Site_use_ID").ToString(), "Org Val: " & OrgID & " Order Val: " & r("MinOrderValue").ToString() & " Discount:" & r("Discount").ToString() & " Type:" & r("DiscountType").ToString(), UserID.ToString(), "0", "0")
            End If
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

    Public Function UploadCustBonusPlan(ByVal dtData As DataTable, ByVal OrgID As String, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Dim tran As SqlTransaction = Nothing
        Dim QueryString As String = Nothing
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            tran = objSQLConn.BeginTransaction()
            Dim objLogin As New SalesWorx.BO.DAL.DAL_Login

            For Each r As DataRow In dtData.Rows
                If r("IsValid").ToString() = "Y" Then



                    QueryString = "app_InsertBonusPlanToCustomers"
                    objSQLCmd = New SqlCommand(QueryString, objSQLConn)
                    objSQLCmd.CommandType = CommandType.StoredProcedure

                    objSQLCmd.Parameters.Add("@CustomerID", SqlDbType.BigInt).Value = r("Customer_ID").ToString()
                    objSQLCmd.Parameters.Add("@SiteUseID", SqlDbType.BigInt).Value = r("Site_Use_ID").ToString()
                    objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 20).Value = OrgID
                    objSQLCmd.Parameters.Add("@PlanId", SqlDbType.Int).Value = r("BonusPlanID").ToString()
                    objSQLCmd.Parameters.Add("@InsertMode", SqlDbType.VarChar, 20).Value = "Single"
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
    Public Function UploadCustOrderDiscount(ByVal dtData As DataTable, ByVal OrgID As String, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Dim tran As SqlTransaction = Nothing
        Dim QueryString As String = Nothing
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            tran = objSQLConn.BeginTransaction()
            Dim objLogin As New SalesWorx.BO.DAL.DAL_Login

            For Each r As DataRow In dtData.Rows
                If r("IsValid").ToString() = "Y" Then



                    QueryString = "app_SaveCustOrderDiscount"
                    objSQLCmd = New SqlCommand(QueryString, objSQLConn)
                    objSQLCmd.CommandType = CommandType.StoredProcedure
                    objSQLCmd.Parameters.Add(New SqlParameter("@CustomerID", SqlDbType.Int))
                    objSQLCmd.Parameters("@CustomerID").Value = r("Customer_ID").ToString()
                    objSQLCmd.Parameters.Add(New SqlParameter("@SiteUSeID", SqlDbType.Int))
                    objSQLCmd.Parameters("@SiteUSeID").Value = r("Site_use_ID").ToString()

                    objSQLCmd.Parameters.Add(New SqlParameter("@MinOrdervalue", SqlDbType.Decimal))
                    objSQLCmd.Parameters("@MinOrdervalue").Value = r("MinOrdervalue").ToString()

                    objSQLCmd.Parameters.Add(New SqlParameter("@MinDiscount", SqlDbType.Decimal))
                    objSQLCmd.Parameters("@MinDiscount").Value = Val(r("MinDiscount").ToString()) / 100.0

                    objSQLCmd.Parameters.Add(New SqlParameter("@MaxDiscount", SqlDbType.Decimal))
                    objSQLCmd.Parameters("@MaxDiscount").Value = Val(r("MaxDiscount").ToString()) / 100.0

                    objSQLCmd.Parameters.Add("@Status", SqlDbType.Int)
                    objSQLCmd.Parameters("@Status").Direction = ParameterDirection.Output
                    objSQLCmd.Transaction = tran
                    objSQLCmd.ExecuteNonQuery()


                    Dim Status As String
                    Status = objSQLCmd.Parameters("@Status").Value.ToString()
                    objSQLCmd.Dispose()
                    If Status = "2" Then
                        objLogin.InsertUserLog(Err_No, Err_Desc, "U", "MAIN MENU", "ORDER LVL CUST DISC", r("Customer_ID").ToString() & "$" & r("Site_use_ID").ToString(), "Org Val: " & OrgID & " Order Val: " & r("MinOrderValue").ToString() & " Min Discount:" & r("MinDiscount").ToString() & " Max.Discount:" & r("MaxDiscount").ToString(), UserID.ToString(), "0", "0")
                    ElseIf Status = "1" Then
                        objLogin.InsertUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "ORDER LVL CUST DISC", r("Customer_ID").ToString() & r("Site_use_ID").ToString(), "Org Val: " & OrgID & " Order Val: " & r("MinOrderValue").ToString() & " Min Discount:" & r("MinDiscount").ToString() & " Max Discount:" & r("MaxDiscount").ToString(), UserID.ToString(), "0", "0")
                    End If
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

    Public Function CheckCustomerShipExist(ByVal OrgId As String, ByVal CustomerNo As String, ByRef Customer_ID As String, ByRef Site_ID As String) As Boolean
        Dim success As Boolean = False
        Dim objSQLConn As SqlConnection
        Dim objSQLCMD As SqlCommand

        Dim sql As String = ""
        sql = "SELECT * FROM dbo.app_GetOrgCustomerShipAddress('" & OrgId & "') where  Customer_No=@CustomerNo  AND Site_Use_ID=@SiteID"

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCMD = New SqlCommand(sql, objSQLConn)
            objSQLCMD.CommandType = CommandType.Text
            objSQLCMD.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 50).Value = CustomerNo
            objSQLCMD.Parameters.Add("@SiteID", SqlDbType.VarChar, 50).Value = Site_ID

            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCMD)
            Dim MsgDs As New DataSet
            SqlAd.Fill(MsgDs, "CustLstTbl")
            If MsgDs.Tables.Count > 0 Then
                If MsgDs.Tables(0).Rows.Count > 0 Then
                    success = True
                    Customer_ID = MsgDs.Tables(0).Rows(0)("Customer_ID").ToString
                    Site_ID = MsgDs.Tables(0).Rows(0)("Site_Use_ID").ToString
                End If
            End If
        Catch ex As Exception

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
    Public Function CheckBonusPlanIsValid(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal PlanID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select COUNT(*) FROM TBL_BNS_Plan WHERE  Organization_ID=@OrgID AND Bns_Plan_ID=@BnsPlanID  ")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            objSQLCmd.Parameters.Add("@BnsPlanID", SqlDbType.Int).Value = PlanID
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

    Public Function CheckOrgShipCustomerNoExist(ByVal OrgId As String, ByVal CustomerNo As String, ByRef Customer_ID As String, ByRef Site_ID As String) As Boolean
        Dim success As Boolean = False
        Dim objSQLConn As SqlConnection
        Dim objSQLCMD As SqlCommand

        Dim sql As String = ""
        sql = "SELECT * FROM app_GetOrgCustomerShipAddress('" & OrgId & "') where  Customer_No=@CustomerNo AND Site_Use_ID=@SiteID "

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCMD = New SqlCommand(sql, objSQLConn)
            objSQLCMD.CommandType = CommandType.Text
            objSQLCMD.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 50).Value = CustomerNo
            objSQLCMD.Parameters.Add("@SiteID", SqlDbType.VarChar, 50).Value = Site_ID
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCMD)
            Dim MsgDs As New DataSet
            SqlAd.Fill(MsgDs, "CustLstTbl")
            If MsgDs.Tables.Count > 0 Then
                If MsgDs.Tables(0).Rows.Count > 0 Then
                    success = True
                    Customer_ID = MsgDs.Tables(0).Rows(0)("Customer_ID").ToString
                    Site_ID = MsgDs.Tables(0).Rows(0)("Site_Use_ID").ToString
                End If
            End If
        Catch ex As Exception

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
      Public Function CheckCustomerNoExist(ByVal OrgId As String, ByVal CustomerNo As String, ByRef Customer_ID As String, ByRef Site_ID As String) As Boolean
        Dim success As Boolean = False
        Dim objSQLConn As SqlConnection
        Dim objSQLCMD As SqlCommand

        Dim sql As String = ""
        sql = "SELECT * FROM app_GetOrgCustomers('" & OrgId & "') where  Customer_No=@CustomerNo "

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCMD = New SqlCommand(sql, objSQLConn)
            objSQLCMD.CommandType = CommandType.Text
            objSQLCMD.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 50).Value = CustomerNo

            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCMD)
            Dim MsgDs As New DataSet
            SqlAd.Fill(MsgDs, "CustLstTbl")
            If MsgDs.Tables.Count > 0 Then
                If MsgDs.Tables(0).Rows.Count > 0 Then
                    success = True
                    Customer_ID = MsgDs.Tables(0).Rows(0)("Customer_ID").ToString
                    Site_ID = MsgDs.Tables(0).Rows(0)("Site_Use_ID").ToString
                End If
            End If
        Catch ex As Exception

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
     Public Function GetCustomerDiscountDefinition(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustomerID As String, ByVal SiteUseId As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "Select * from TBL_Customer_Addl_Info A where a.Customer_ID=" & CustomerID & " and A.Site_Use_ID=" & SiteUseId & " and A.Attrib_Name='DISCOUNT'"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustLstTbl")

            GetCustomerDiscountDefinition = MsgDs.Tables(0)
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

    Public Function GetCustomerOrderDiscountByID(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustomerID As String, ByVal SiteUseId As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "Select * from TBL_Customer_Addl_Info A where a.Customer_ID=" & CustomerID & " and A.Site_Use_ID=" & SiteUseId & " and A.Attrib_Name='CUST_ORDER_DISC_LIMIT'"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustLstTbl")

            GetCustomerOrderDiscountByID = MsgDs.Tables(0)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "44061"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function

    Public Function GetCustomerShipAddress(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_ID As String, ByVal FilterName As String, ByVal FilterNo As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "app_ListCustomerShipAdd"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure

            objSQLCmd.Parameters.AddWithValue("@Customer_ID", Customer_ID)
            objSQLCmd.Parameters.AddWithValue("@FilterName", FilterName.Trim)
            objSQLCmd.Parameters.AddWithValue("@FilterNo", FilterNo.Trim)

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustLstTbl")

            GetCustomerShipAddress = MsgDs.Tables(0)
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
     Public Function GetCustomerShipAddressDeatils(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_ID As String, ByVal Site_Use_ID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT * FROM dbo.TBL_Customer_Ship_Address where Customer_ID=@Customer_ID and Site_Use_ID=@Site_Use_ID")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@Customer_ID", Customer_ID)
            objSQLCmd.Parameters.AddWithValue("@Site_Use_ID", Site_Use_ID)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustLstTbl")

            GetCustomerShipAddressDeatils = MsgDs.Tables(0)
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
      Public Function GetShipAddressVans(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_ID As String, ByVal Site_Use_ID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT F.SalesRep_ID,F.SalesRep_Name FROM dbo.TBL_Customer_Van_Map A inner join dbo.TBL_Org_CTL_DTL B on A.Van_Org_ID=B.Org_ID inner join dbo.tbl_FSR F on F.SalesRep_ID=B.SalesRep_ID  where Customer_ID=@Customer_ID and Site_Use_ID=@Site_Use_ID")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@Customer_ID", Customer_ID)
            objSQLCmd.Parameters.AddWithValue("@Site_Use_ID", Site_Use_ID)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustLstTbl")

            GetShipAddressVans = MsgDs.Tables(0)
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
    Public Function GetCustomerDeatils(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_ID As String, ByVal Site_Use_ID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT * FROM dbo.TBL_Customer where Customer_ID=@Customer_ID and Site_Use_ID=@Site_Use_ID")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@Customer_ID", Customer_ID)
            objSQLCmd.Parameters.AddWithValue("@Site_Use_ID", Site_Use_ID)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustLstTbl")

            GetCustomerDeatils = MsgDs.Tables(0)
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
    Public Function GetCustomerList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT DISTINCT A.Customer_ID, A.Site_Use_ID, convert(numeric,A.Customer_No) as Customer_No , B.Location, A.Customer_Name, A.[Address],A.City, B.Postal_Code, A.Cust_Status, B.Customer_Barcode, B.Dept,A.Customer_Type,A.Credit_Limit,A.Credit_Hold,A.Customer_Class,A.Customer_OD_Status,A.Cash_Cust,A.Allow_FOC,C.[Description] as Customer_Segment,D.[Description] as Sales_District FROM dbo.TBL_Customer AS A INNER JOIN TBL_Customer_Ship_Address As B ON A.Customer_Id=B.Customer_Id  LEFT JOIN TBL_Customer_Segments As C ON B.Customer_Segment_ID=C.Customer_Segment_ID LEFT JOIN TBL_Sales_District As D ON B.Sales_District_ID=D.Sales_District_ID  Where 1=1 {0} order by A.Customer_Name Asc", _sSearchParams, QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustLstTbl")

            GetCustomerList = MsgDs
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
    Public Function GetCustomerVisitReport_TotalSummary(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Fromdate As DateTime, ByVal Todate As DateTime, ByVal OrgID As Integer, ByVal SalesRepId As Integer, ByVal CustID As Integer, ByVal UID As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("Rep_CustomerVisit_TotalAmt", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            ' If Fromdate IsNot DBNull.Value Then
            objSQLCmd.Parameters.AddWithValue("@FromDate", Fromdate)
            ' End If
            'If Todate <> SqlDateTime.Null Then
            objSQLCmd.Parameters.AddWithValue("@ToDate", Todate)
            'End If
            objSQLCmd.Parameters.AddWithValue("@OID", OrgID)
            objSQLCmd.Parameters.AddWithValue("@SID", SalesRepId)
            objSQLCmd.Parameters.AddWithValue("@CustID", CustID)
            objSQLCmd.Parameters.AddWithValue("@Uid", UID)

            Dim dt As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            GetCustomerVisitReport_TotalSummary = dt
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
    Public Function GetCustomerVisits(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("Select Actual_Visit_ID,Visit_Start_Date,Visit_End_Date, SalesRep_Name,B.Customer_No,B.Customer_Name,B.Customer_Class,B.Customer_Type, A.Site_Use_ID,Scanned_Closing,0 as OrderTaken,1 as SurveyTaken,case when (SELECT COUNT(DistributionCheck_ID) FROM TBL_Distribution_Check WHERE Visit_ID=Actual_Visit_ID  ) > 0 then 'Y' else 'N' end as DC ,isnull((select sum(Order_Amt)  from  TBL_Order where Visit_Id= A.Actual_Visit_ID),0) as OrderAmt,isnull((select sum(Order_Amt)  from  TBL_RMA where Visit_Id= A.Actual_Visit_ID),0) as RMA,Decimal_Digits,isnull((select sum(Order_Amt)  from  TBL_Proforma_Order where Visit_Id= A.Actual_Visit_ID),0) as UnConfirmedOrderAmt ,isnull((Select sum(Amount) from TBL_Collection where convert(varchar,Collected_On,101)=convert(varchar,A.Visit_Start_Date,101) and Customer_ID=A.Customer_ID),0) as Payment,A.Customer_ID from TBL_FSR_Actual_Visits as A Left Join TBL_Customer as B On B.Customer_ID=A.Customer_ID AND B.Site_Use_ID=A.Site_Use_ID Left JOIN TBL_Customer_Ship_Address As C On C.Customer_Id=B.Customer_Id AND C.Site_Use_ID=B.Site_Use_ID Left Join TBL_FSR As D On A.SalesRep_ID=D.SalesRep_ID Left Outer Join  TBL_Org_CTL_DTL  O on O.SalesRep_ID=A.SalesRep_ID Left Outer Join TBL_Currency E on E.Currency_Code =O.Currency_Code  Where 1=1 {0} order by B.Customer_Name Asc", _sSearchParams, QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustVisitsTbl")

            GetCustomerVisits = MsgDs
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
    Public Function GetCustomerVisitDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select TOP 1 (C.Customer_Name+'-'+ISNULL(C.Location,'N/A')) As Customer_Name, A.Customer_ID, A.Site_Use_ID,A.Visit_Start_Date,A.Visit_End_Date, A.SalesRep_ID,A.Scanned_Closing,A.Text_Notes, A.Voice_Notes, D.SalesRep_Name,ISNULL(A.Odo_Reading,'') As Odo_Reading,  dbo.GetEmpName(A.Emp_Code) as Emp_Name ,CC_Name,CC_TelNo,(Select Customer_Name from TBL_Customer where Site_Use_ID=A.Cash_Site_ID and Customer_ID=A.Cash_Customer_ID and Cash_Cust='N') as Credit_Cust_Name,Cash_Customer_ID from TBL_FSR_Actual_Visits as A, TBL_Customer_Ship_Address as C, TBL_FSR as D where A.Customer_ID=C.Customer_ID AND A.Site_Use_ID=C.Site_Use_ID AND A.SalesRep_ID=D.SalesRep_ID {0}", _sSearchParams, QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim dt As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            GetCustomerVisitDetails = dt
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
    Public Function IsDistributionCheckDone(ByVal VisitId As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim iRetVal As Integer = 0
        Dim sQry As String
        Try
            objSQLConn = _objDB.GetSQLConnection
            sQry = "SELECT COUNT(DistributionCheck_ID) FROM TBL_Distribution_Check WHERE Visit_ID='{0}'"
            sQry = String.Format(sQry, VisitId)
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim tempDBVal As Object = objSQLCmd.ExecuteScalar()

            If Not IsNothing(tempDBVal) AndAlso Not IsDBNull(tempDBVal) Then
                iRetVal = CInt(tempDBVal.ToString())
            End If
            If iRetVal > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return iRetVal
    End Function
    Public Function HasOrder(ByVal VisitId As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim iRetVal As Integer = 0
        Dim sQry As String
        Try
            objSQLConn = _objDB.GetSQLConnection
            sQry = "Select Count(Visit_ID) from TBL_Order  WHERE Visit_ID='{0}'"
            sQry = String.Format(sQry, VisitId)
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim tempDBVal As Object = objSQLCmd.ExecuteScalar()

            If Not IsNothing(tempDBVal) AndAlso Not IsDBNull(tempDBVal) Then
                iRetVal = CInt(tempDBVal.ToString())
            End If
            If iRetVal > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return iRetVal
    End Function
    Public Function HasOrderReturn(ByVal VisitId As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim iRetVal As Integer = 0
        Dim sQry As String
        Try
            objSQLConn = _objDB.GetSQLConnection
            sQry = "Select Count(Visit_ID) from TBL_RMA WHERE Visit_ID='{0}'"
            sQry = String.Format(sQry, VisitId)
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim tempDBVal As Object = objSQLCmd.ExecuteScalar()

            If Not IsNothing(tempDBVal) AndAlso Not IsDBNull(tempDBVal) Then
                iRetVal = CInt(tempDBVal.ToString())
            End If
            If iRetVal > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return iRetVal
    End Function

    Public Function GetDistributionChecks(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            'Dim QueryString As String = String.Format("select c.SalesRep_Name,b.Customer_Name,b.Customer_No,a.DistributionCheck_ID,dbo.GetEmpName(a.Emp_Code)+ '('+ a.Emp_Code + ')' as Emp_Code ,a.Checked_On,a.Visit_ID,dbo.GetStatusDescription('D',a.Status) as Status from TBL_Distribution_Check as a Left Join TBL_Customer as b on b.Customer_ID=a.Customer_ID and b.Site_Use_ID=a.Site_Use_ID Left Join TBL_FSR as c on c.SalesRep_ID=a.SalesRep_ID  Where 1=1 {0} order by b.Customer_Name Asc", _sSearchParams, QueryStr)
            Dim QueryString As String = String.Format("WITH t1 AS( select  c.SalesRep_Name,b.Customer_Name,b.Customer_No,a.DistributionCheck_ID,dbo.GetEmpName(a.Emp_Code)+ '('+ a.Emp_Code + ')' as Emp_Code ,a.Checked_On,a.Visit_ID,dbo.GetStatusDescription('D',a.Status) as Status , a.Customer_Id,ROW_NUMBER() OVER (ORDER BY Customer_Name) AS 'RowNumber'  from  TBL_Distribution_Check as a Left Join TBL_Customer as b on b.Customer_ID=a.Customer_ID and b.Site_Use_ID=a.Site_Use_ID Left Join TBL_FSR as c on c.SalesRep_ID=a.SalesRep_ID  Where 1=1 {0} )select * from t1 where RowNumber in ( select Min(RowNumber)from t1  group by convert(varchar, Checked_On,101) ,Customer_ID)", _sSearchParams, QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "DistriChkTbl")

            GetDistributionChecks = MsgDs
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
    Public Function GetDistributionCheckDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select top 1  c.SalesRep_Name,b.Customer_Name,b.Customer_No,a.DistributionCheck_ID,a.Emp_Code,a.Checked_On,a.Visit_ID,dbo.GetStatusDescription('D',a.Status) as Status from TBL_Distribution_Check as a Left Join TBL_Customer as b on b.Customer_ID=a.Customer_ID and b.Site_Use_ID=a.Site_Use_ID Left Join TBL_FSR as c on c.SalesRep_ID=a.SalesRep_ID  Where 1=1 {0}", _sSearchParams, QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim dt As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            GetDistributionCheckDetails = dt
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
    Public Function GetDistributionChecksLineItem(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select c.Description,c.Item_Code,b.DistributionCheckLine_ID,case when b.Is_Available='Y' Then 'Yes' Else 'No' End as Is_Available,b.Qty,b.Display_UOM,b.Expiry_Dt, case when ( Select count(Inventory_Item_ID)  from TBL_Product_MSL where Inventory_Item_ID=c.Inventory_Item_ID and Organization_ID=c.Organization_ID) > 0 then 'Yes' else 'No' end  as ISMSL , case when (select count(O.Orig_Sys_Document_Ref)  from TBL_Order O inner join TBL_Order_Line_Items  L on O.Orig_Sys_Document_Ref=L.Orig_Sys_Document_Ref Where Ship_To_Customer_ID=a.Customer_ID and Convert(varchar,Start_Time,101)=Convert(varchar,Checked_On,101)and Inventory_Item_ID=c.Inventory_Item_ID and Ship_To_Site_ID=c.Organization_ID ) > 0 then 'Yes' else 'No' end  as ExitInfo from TBL_Distribution_Check as a Inner Join TBL_Distribution_Check_Items as b on b.DistributionCheck_ID=a.DistributionCheck_ID Inner Join TBL_Org_CTL_DTL as d on d.SalesRep_ID=a.SalesRep_ID Inner Join TBL_Product as c on c.Inventory_Item_ID=b.Inventory_Item_ID and c.Organization_ID=d.MAS_Org_ID  Where 1=1 {0} order by c.Item_Code Asc", _sSearchParams, QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "DistChkLineItemTbl")

            GetDistributionChecksLineItem = MsgDs
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
    Public Function GetOrders(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select A.Row_ID,A.Orig_Sys_Document_Ref,A.Order_Status,A.System_Order_Date, SalesRep_Name,B.Customer_No,B.Customer_Name,OrderType from (Select Row_ID,Orig_Sys_Document_Ref,dbo.GetStatusDescription('I',Order_Status) as Order_Status,Creation_Date as System_Order_Date,Ship_To_Customer_ID,Ship_To_Site_ID,Created_By,Visit_ID from TBL_Order UNION Select Row_ID,Orig_Sys_Document_Ref,' ' as Order_Status,Creation_Date as System_Order_Date,'Order' as OrderType,Ship_To_Customer_ID,Ship_To_Site_ID,Created_By,Visit_ID  from TBL_Proforma_Order)A  Left Join TBL_Customer as B On B.Customer_ID=A.Ship_To_Customer_ID AND B.Site_Use_ID=A.Ship_To_Site_ID Left Join TBL_FSR As D On A.Created_By=D.SalesRep_ID  Where 1=1 {0} order by B.Customer_Name Asc", _sSearchParams, QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrdersTbl")

            GetOrders = MsgDs
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
  Public Function GetHeldReceipt(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal InvoiceNo As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "app_HeldReceipt"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@Orig_Sys_Document_Ref", InvoiceNo)

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)

            GetHeldReceipt = dt
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "73061"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetHeldNewCustomers(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "app_HeldNewCustomer"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@SearchParams", _sSearchParams)
            objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)

            GetHeldNewCustomers = dt
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "73061"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function UpdateReleaseOrder(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrderRefNo As String, ByVal UpDatedBy As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim objSQLtrans As SqlTransaction
            objSQLtrans = objSQLConn.BeginTransaction()
            Try


            Dim QueryString As String = "UPDATE TBL_Order SET Order_Status='Y',Last_Update_Date=GETDATE(),Last_Updated_By=@UpdatedBy WHERE Orig_Sys_Document_Ref=@OrderRefNo AND Order_Status='H'"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@OrderRefNo", OrderRefNo)
            objSQLCmd.Parameters.AddWithValue("@UpdatedBy", UpDatedBy)
            objSQLCmd.Transaction = objSQLtrans
            objSQLCmd.ExecuteNonQuery()

            Dim objSQLCmdNew As SqlCommand
            QueryString = "UPDATE TBL_Collection SET Status='Y' WHERE Collection_ID in(select Collection_ID from TBL_Collection_Invoices where Invoice_No=@OrderRefNo) AND Status='H'"
            objSQLCmdNew = New SqlCommand(QueryString, objSQLConn)
            objSQLCmdNew.CommandType = CommandType.Text
            objSQLCmdNew.Parameters.AddWithValue("@OrderRefNo", OrderRefNo)
            objSQLCmdNew.Transaction = objSQLtrans
            objSQLCmdNew.ExecuteNonQuery()
            objSQLtrans.Commit()

            objSQLCmd.Dispose()
            objSQLCmdNew.Dispose()
             success = True
            Catch ex As Exception
                objSQLtrans.Rollback()
            End Try
        Catch ex As Exception
            Err_No = "16121"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function

    Public Function UpdateReConcileOrder(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrderRefNo As String, ByVal UpDatedBy As Integer, ByVal Remarks As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Try

            objSQLConn = _objDB.GetSQLConnection
            Dim objSQLtrans As SqlTransaction
            objSQLtrans = objSQLConn.BeginTransaction()
            Try
            Dim QueryString As String = "UPDATE TBL_Order SET Order_Status='X',Last_Update_Date=GETDATE(),Last_Updated_By=@UpdatedBy WHERE Orig_Sys_Document_Ref=@OrderRefNo AND Order_Status='H'"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Transaction = objSQLtrans
            objSQLCmd.Parameters.AddWithValue("@OrderRefNo", OrderRefNo)
            objSQLCmd.Parameters.AddWithValue("@UpdatedBy", UpDatedBy)
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()

            Dim objSQLCmdNew As SqlCommand
            QueryString = "UPDATE TBL_Collection SET Status='X' WHERE Collection_ID in(select Collection_ID from TBL_Collection_Invoices where Invoice_No=@OrderRefNo) AND Status='H'"
            objSQLCmdNew = New SqlCommand(QueryString, objSQLConn)
            objSQLCmdNew.CommandType = CommandType.Text
            objSQLCmdNew.Transaction = objSQLtrans
            objSQLCmdNew.Parameters.AddWithValue("@OrderRefNo", OrderRefNo)
            objSQLCmdNew.ExecuteNonQuery()
            objSQLtrans.Commit()
            success = True
            Catch ex As Exception
                objSQLtrans.Rollback()
            End Try
            If Remarks <> "" And Remarks <> "0" Then

                Dim QueryString = "INSERT INTO TBL_Doc_Addl_Info(Doc_Type,Doc_No,Attrib_Name,Attrib_Value_1,Last_Updated_At,Last_Updated_By)VALUES('I',@OrderRefNo,'ERP_REF',@Remarks, GETDATE(),@UpdatedBy)"
                objSQLCmd = New SqlCommand(QueryString, objSQLConn)
                objSQLCmd.CommandType = CommandType.Text
                objSQLCmd.Parameters.AddWithValue("@OrderRefNo", OrderRefNo)
                objSQLCmd.Parameters.AddWithValue("@UpdatedBy", UpDatedBy)
                objSQLCmd.Parameters.AddWithValue("@Remarks", Remarks)
                objSQLCmd.ExecuteNonQuery()
                objSQLCmd.Dispose()
            End If


            success = True


        Catch ex As Exception
            Err_No = "21095"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function

    Public Function GetConcileOrderDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrdRefNo As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "app_GetConcileOrderDetails"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@OrgSysRef", OrdRefNo)


            Dim dt As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            GetConcileOrderDetails = dt
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
    Public Function GetOrderDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select top 1  A.Orig_Sys_Document_Ref,dbo.GetStatusDescription('I',A.Order_Status) as Order_Status ,b.Customer_Name as Invoice_To_Customer,c.Customer_Name as Ship_To_Customer,A.System_Order_Date,A.Request_Date,A.Schedule_Ship_Date,A.Shipping_Instructions,A.Packing_Instructions,A.Customer_PO_Number,A.Signee_Name,A.Customer_Signature,A.Start_Time,A.End_Time,A.Order_Amt,O.Currency_code as CurrDesc,R.Decimal_Digits,A.Visit_ID from TBL_Order as A left join TBL_Customer as b on A.Inv_To_Customer_Id=b.Customer_ID AND A.Inv_To_Site_ID=b.Site_Use_ID left join TBL_Customer_Ship_Address as C on A.Ship_To_Customer_ID=C.Customer_ID and A.Ship_To_Site_ID=C.Site_Use_ID  left join TBL_Org_CTL_DTL O on O.SalesRep_ID=A.Created_By left join TBL_Currency R on R.Currency_Code=O.Currency_Code  Where 1=1 {0}", _sSearchParams, QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim dt As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            GetOrderDetails = dt
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
    Public Function GetUnconfirmedOrderDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select top 1  A.Orig_Sys_Document_Ref,'' as Order_Status ,c.Customer_Name as Ship_To_Customer,A.Creation_Date as  System_Order_Date,null as Request_Date,null as Schedule_Ship_Date,'' as Shipping_Instructions,'' as Packing_Instructions,'' as Customer_PO_Number,'' as Signee_Name,'' as Customer_Signature,A.Start_Time,null as End_Time,A.Order_Amt,O.Currency_code as CurrDesc,R.Decimal_Digits,A.Visit_ID from TBL_Proforma_Order as A  left join TBL_Customer_Ship_Address as C on A.Ship_To_Customer_ID=C.Customer_ID and A.Ship_To_Site_ID=C.Site_Use_ID  left join TBL_Org_CTL_DTL O on O.SalesRep_ID=A.Created_By left join TBL_Currency R on R.Currency_Code=O.Currency_Code  Where 1=1 {0}", _sSearchParams, QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim dt As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            GetUnconfirmedOrderDetails = dt
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
    Public Function GetOrdersLineItem(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("Select (B.Ordered_Quantity / isNull(U.Conversion,1)) as Ordered_Quantity  ,B.Order_Quantity_UOM,B.Def_Bonus,B.Calc_Price_Flag,c.Description, c.Item_Code,Display_UOM, ((B.Ordered_Quantity / isNull(U.Conversion,1))*Unit_Selling_Price ) as ItemPrice  from TBL_Order as A inner join TBL_Order_Line_Items as B On A.Orig_Sys_Document_Ref=B.Orig_Sys_Document_Ref INNER JOIN TBL_Org_CTL_DTL As G ON A.Created_By=G.SalesRep_ID  left join TBL_Product As C ON C.Inventory_Item_ID=B.Inventory_Item_ID  left outer join TBL_Item_UOM U on U.Item_UOM=B.Display_UOM and U.Item_Code=C.Item_Code and U.Organization_ID=C.Organization_ID Where 1=1 {0} order by B.Line_Number Asc", _sSearchParams, QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrdersLineItemTbl")

            GetOrdersLineItem = MsgDs
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
    Public Function GetUnconfirmedOrdersLineItem(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("Select (B.Ordered_Quantity * isNull(U.Conversion,0)) as Ordered_Quantity  ,B.Order_Quantity_UOM,B.Def_Bonus,B.Calc_Price_Flag,c.Description, c.Item_Code,Display_UOM, (B.Ordered_Quantity * isNull(U.Conversion,0))*Unit_Selling_Price  as ItemPrice  from TBL_Proforma_Order as A inner join TBL_Proforma_Order_Line_Items as B On A.Orig_Sys_Document_Ref=B.Orig_Sys_Document_Ref INNER JOIN TBL_Org_CTL_DTL As G ON A.Created_By=G.SalesRep_ID  left join TBL_Product As C ON C.Inventory_Item_ID=B.Inventory_Item_ID  left outer join TBL_Item_UOM U on U.Item_UOM=B.Order_Quantity_UOM and U.Item_Code=C.Item_Code and U.Organization_ID=C.Organization_ID Where 1=1 {0} order by B.Line_Number Asc", _sSearchParams, QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrdersLineItemTbl")

            GetUnconfirmedOrdersLineItem = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74063"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetOrdersReturn(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("Select A.Row_ID,A.Orig_Sys_Document_Ref,A.Order_Status,A.Creation_Date,SalesRep_Name,B.Customer_No,B.Customer_Name from TBL_RMA as A  Left Join TBL_Customer as B On B.Customer_ID=A.Ship_To_Customer_ID AND B.Site_Use_ID=A.Ship_To_Site_ID Left Join TBL_FSR As D On A.Created_By=D.SalesRep_ID  Where 1=1 {0} order by B.Customer_Name Asc", _sSearchParams, QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrdersRetTbl")

            GetOrdersReturn = MsgDs
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
    Public Function GetOrderReturnDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select top 1  A.Orig_Sys_Document_Ref,dbo.GetStatusDescription('R',A.Order_Status) as Order_Status ,b.Customer_Name as Invoice_To_Customer,c.Customer_Name as Ship_To_Customer,A.Creation_Date,A.Order_Amt,A.Invoice_Ref_No,d.Customer_Name as Credit_To_Customer,A.Signee_Name, A.Customer_Signature,A.Start_Time,A.End_Time,O.Currency_code as CurrDesc,R.Decimal_Digits,A.Customer_Ref_No,A.Internal_Notes,(select Description from TBL_Reason_Codes where Reason_Code=A.Reason_Code) as Reason  from TBL_RMA as A  left join TBL_Customer as b on A.Inv_To_Customer_Id=b.Customer_ID AND A.Inv_To_Site_ID=b.Site_Use_ID left join TBL_Customer_Ship_Address as C on A.Ship_To_Customer_ID=C.Customer_ID and A.Ship_To_Site_ID=C.Site_Use_ID left join TBL_Customer as d on A.Credit_Customer_ID=d.Customer_ID and A.Credit_Site_ID=d.Site_Use_ID left join TBL_Org_CTL_DTL O on O.SalesRep_ID=A.Created_By left join TBL_Currency R on R.Currency_Code=O.Currency_Code  Where 1=1 {0}", _sSearchParams, QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim dt As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            GetOrderReturnDetails = dt
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
    Public Function GetOrdersReturnLineItem(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("Select (B.Returned_Quantity / isNull(U.Conversion,1)) as Returned_Quantity ,B.Return_Quantity_UOM,B.Unit_Selling_Price,c.Description, c.Item_Code,B.Display_UOM  from TBL_RMA as A inner join TBL_RMA_Line_Items as B On A.Orig_Sys_Document_Ref=B.Orig_Sys_Document_Ref INNER JOIN TBL_Org_CTL_DTL As G ON A.Created_By=G.SalesRep_ID  left join TBL_Product As C ON C.Inventory_Item_ID=B.Inventory_Item_ID left outer join TBL_Item_UOM U on U.Item_UOM=B.Display_UOM and U.Item_Code=C.Item_Code and U.Organization_ID=C.Organization_ID Where 1=1 {0} order by c.Item_Code Asc", _sSearchParams, QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrdersRetLineItemTbl")

            GetOrdersReturnLineItem = MsgDs
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
    Public Function GetCashCustomerDetailsByVisitID(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal VisitId As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("Select CC_Name,CC_TelNo from TBL_FSR_Actual_Visits Where Actual_Visit_ID='{0}'", VisitId)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim dt As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            GetCashCustomerDetailsByVisitID = dt
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "84061"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function SaveCustomerProdCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustomerCode As String, ByVal Itemcode As String, ByVal CustomerProdCode As String, ByVal Userid As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_UpdateCustomerProdCode", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@CustomerCode", SqlDbType.VarChar))
            objSQLCmd.Parameters("@CustomerCode").Value = CustomerCode
            objSQLCmd.Parameters.Add(New SqlParameter("@ItemCode", SqlDbType.VarChar))
            objSQLCmd.Parameters("@ItemCode").Value = Itemcode
            objSQLCmd.Parameters.Add(New SqlParameter("@Cust_Item_Code", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Cust_Item_Code").Value = CustomerProdCode
            objSQLCmd.Parameters.Add(New SqlParameter("@Created_By", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Created_By").Value = Userid
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()
            objSQLCmd = Nothing

            bRetVal = True

        Catch ex As Exception
            Err_No = "74206"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function
    Public Function GetCustomerProdCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Criteria As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_GetCustomerProductCode", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Criteria", Criteria)
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
        GetCustomerProdCode = dtDivConfig
    End Function
    Public Function DeleteCustomerProdCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustomerCode As String, ByVal Itemcode As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_DeleteCustomerProdCode", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@CustomerCode", SqlDbType.VarChar))
            objSQLCmd.Parameters("@CustomerCode").Value = CustomerCode
            objSQLCmd.Parameters.Add(New SqlParameter("@ItemCode", SqlDbType.VarChar))
            objSQLCmd.Parameters("@ItemCode").Value = Itemcode
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()
            objSQLCmd = Nothing

            bRetVal = True

        Catch ex As Exception
            Err_No = "74206"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function
    Public Function SaveCustomer(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal opt As String, ByRef CustomerID As String, ByRef SiteUseID As String, ByVal CustomerName As String, ByVal CustomerNo As String, ByVal Contact As String, ByVal Location As String, ByVal Address As String, ByVal Phone As String, ByVal City As String, ByVal CashCust As String, ByVal CreditPeriod As String, ByVal CreditLimit As String, ByVal AavailBalance As String, ByVal CreditHold As String, ByVal PriceList As String, ByVal OrgId As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False

        Try
            Dim Dr As SqlDataReader
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_ManageCustomer", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
             objSQLCmd.Parameters.Add(New SqlParameter("@OPT", SqlDbType.Int))
            objSQLCmd.Parameters("@OPT").Value = opt
            objSQLCmd.Parameters.Add(New SqlParameter("@Customer_ID", SqlDbType.Int))
            objSQLCmd.Parameters("@Customer_ID").Value = Val(CustomerID)
            objSQLCmd.Parameters.Add(New SqlParameter("@Site_use_ID", SqlDbType.Int))
            objSQLCmd.Parameters("@Site_use_ID").Value = Val(SiteUseID)

            objSQLCmd.Parameters.Add(New SqlParameter("@Customer_No", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Customer_No").Value = CustomerNo

            objSQLCmd.Parameters.Add(New SqlParameter("@Customer_Name", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Customer_Name").Value = CustomerName

            objSQLCmd.Parameters.Add(New SqlParameter("@Contact", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Contact").Value = Contact

            objSQLCmd.Parameters.Add(New SqlParameter("@Address", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Address").Value = Address

            objSQLCmd.Parameters.Add(New SqlParameter("@City", SqlDbType.VarChar))
            objSQLCmd.Parameters("@City").Value = City

            objSQLCmd.Parameters.Add(New SqlParameter("@Phone", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Phone").Value = Phone

            objSQLCmd.Parameters.Add(New SqlParameter("@Location", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Location").Value = Location

            objSQLCmd.Parameters.Add(New SqlParameter("@Credit_Limit", SqlDbType.Decimal))
            If CashCust = "Y" Then
                CreditLimit = 0
                AavailBalance = 0
                CreditPeriod = 0
            End If
            If CreditLimit.Trim <> "" Then
                objSQLCmd.Parameters("@Credit_Limit").Value = CreditLimit
            Else
                  objSQLCmd.Parameters("@Credit_Limit").Value = DBNull.Value
            End If

            objSQLCmd.Parameters.Add(New SqlParameter("@Credit_Hold", SqlDbType.Char))
            objSQLCmd.Parameters("@Credit_Hold").Value = CreditHold

            objSQLCmd.Parameters.Add(New SqlParameter("@Cash_Cust", SqlDbType.Char))
            objSQLCmd.Parameters("@Cash_Cust").Value = CashCust

            objSQLCmd.Parameters.Add(New SqlParameter("@Price_List", SqlDbType.Int))
            objSQLCmd.Parameters("@Price_List").Value = Val(PriceList)

            objSQLCmd.Parameters.Add(New SqlParameter("@Avail_Bal", SqlDbType.Decimal))
            If AavailBalance.Trim <> "" Then
                objSQLCmd.Parameters("@Avail_Bal").Value = AavailBalance
             Else
                objSQLCmd.Parameters("@Avail_Bal").Value = DBNull.Value
             End If

             objSQLCmd.Parameters.Add(New SqlParameter("@Bill_Credit_Period", SqlDbType.Int))
            If CreditPeriod.Trim <> "" Then
                objSQLCmd.Parameters("@Bill_Credit_Period").Value = CreditPeriod
            Else
                  objSQLCmd.Parameters("@Bill_Credit_Period").Value = 0
            End If
            objSQLCmd.Parameters.Add(New SqlParameter("@Custom1", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Custom1").Value = OrgId

            Dr = objSQLCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If Dr.Read Then
                If Val(Dr(0).ToString) > 0 Then
                    CustomerID = Dr(0).ToString
                    SiteUseID = Dr(1).ToString
                    bRetVal = True
                Else
                    If opt = 1 Or opt = 2 Then
                     Err_Desc = "The customer no. already exists."
                    End If
                End If
            End If
            Dr.Close()
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        Catch ex As Exception
            Err_No = "7400920"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function
     Public Function SaveCustomerShipAddress(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal opt As String, ByVal CustomerID As String, ByRef SiteUseID As String, ByVal CustomerName As String, ByVal CustomerNo As String, ByVal Address As String, ByVal PO As String, ByVal City As String, ByVal Customer_Segment_ID As String, ByVal Cust_Lat As String, ByVal Cust_Long As String, ByVal OrgID As String, ByVal Sales_District_ID As String, ByVal Vans As String, ByVal Location As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Dim bVanRel As Boolean = False
        Dim bShipSaved As Boolean = False
        Try

            objSQLConn = _objDB.GetSQLConnection
             Dim objSQLtrans As SqlTransaction
            objSQLtrans = objSQLConn.BeginTransaction()
Try


            objSQLCmd = New SqlCommand("app_ManageCustomerShipAddress", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Transaction = objSQLtrans
             objSQLCmd.Parameters.Add(New SqlParameter("@OPT", SqlDbType.Int))
            objSQLCmd.Parameters("@OPT").Value = opt
            objSQLCmd.Parameters.Add(New SqlParameter("@Customer_ID", SqlDbType.Int))
            objSQLCmd.Parameters("@Customer_ID").Value = Val(CustomerID)
            objSQLCmd.Parameters.Add(New SqlParameter("@Site_use_ID", SqlDbType.Int))
            objSQLCmd.Parameters("@Site_use_ID").Value = Val(SiteUseID)

            objSQLCmd.Parameters.Add(New SqlParameter("@Customer_No", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Customer_No").Value = CustomerNo

            objSQLCmd.Parameters.Add(New SqlParameter("@Customer_Name", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Customer_Name").Value = CustomerName

            objSQLCmd.Parameters.Add(New SqlParameter("@Address", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Address").Value = Address

            objSQLCmd.Parameters.Add(New SqlParameter("@City", SqlDbType.VarChar))
            objSQLCmd.Parameters("@City").Value = City

            objSQLCmd.Parameters.Add(New SqlParameter("@PO", SqlDbType.VarChar))
            objSQLCmd.Parameters("@PO").Value = PO

            objSQLCmd.Parameters.Add(New SqlParameter("@Customer_Segment_ID", SqlDbType.Int))
            If Customer_Segment_ID = "0" Or Customer_Segment_ID.Trim = "" Then
             objSQLCmd.Parameters("@Customer_Segment_ID").Value = DBNull.Value
            Else
            objSQLCmd.Parameters("@Customer_Segment_ID").Value = Customer_Segment_ID
            End If


            objSQLCmd.Parameters.Add(New SqlParameter("@Sales_District_ID", SqlDbType.Int))
            If Sales_District_ID = "0" Or Sales_District_ID.Trim = "" Then
              objSQLCmd.Parameters("@Sales_District_ID").Value = 0
            Else
             objSQLCmd.Parameters("@Sales_District_ID").Value = Sales_District_ID
            End If


            objSQLCmd.Parameters.Add(New SqlParameter("@Cust_Lat", SqlDbType.Decimal))
            objSQLCmd.Parameters("@Cust_Lat").Value = Val(Cust_Lat)

            objSQLCmd.Parameters.Add(New SqlParameter("@Cust_Long", SqlDbType.Decimal))
            objSQLCmd.Parameters("@Cust_Long").Value = Val(Cust_Long)

            objSQLCmd.Parameters.Add(New SqlParameter("@OrgID", SqlDbType.VarChar))
           objSQLCmd.Parameters("@OrgID").Value = OrgID

            objSQLCmd.Parameters.Add(New SqlParameter("@Location", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Location").Value = Location

            objSQLCmd.Parameters.Add("@OSiteUseID", SqlDbType.Int)
            objSQLCmd.Parameters("@OSiteUseID").Direction = ParameterDirection.Output

            objSQLCmd.ExecuteNonQuery()
            Dim id As String
            id = objSQLCmd.Parameters("@OSiteUseID").Value.ToString()

            If Val(id) > 0 Then
                SiteUseID = id
                bShipSaved = True
            Else
                If opt = 1 Or opt = 2 Then
                     Err_Desc = "The customer Ship address already exists."
                    End If
                End If

            If bShipSaved = True Then
                     If Vans.Trim <> "" Then

                         Dim dt As New DataTable
                         dt.Columns.Add("Customer_ID", System.Type.GetType("System.Int32"))
                         dt.Columns.Add("Site_Use_ID", System.Type.GetType("System.Int32"))
                         dt.Columns.Add("Van_Org_ID", System.Type.GetType("System.String"))
                         dt.Columns.Add("Avail_Bal", System.Type.GetType("System.Decimal"))
                         dt.Columns.Add("Daily_Invoice_Limit", System.Type.GetType("System.Decimal"))
                         dt.Columns.Add("Custom_Attribute_1", System.Type.GetType("System.String"))
                         dt.Columns.Add("Custom_Attribute_2", System.Type.GetType("System.String"))
                         dt.Columns.Add("Custom_Attribute_3", System.Type.GetType("System.Int32"))
                         Dim van() As String
                         van = Vans.Split(",")
                         For i = 0 To van.Length - 1
                            If van(i) <> "" Then
                                Dim drVan As DataRow
                                drVan = dt.NewRow
                                drVan(0) = CustomerID
                                drVan(1) = SiteUseID
                                drVan(2) = GetVanOrgID(van(i))
                                drVan(3) = 0
                                drVan(4) = 0
                                dt.Rows.Add(drVan)
                            End If
                         Next

                         Dim objSQLCmdNew As SqlCommand
                         Dim QueryString As String
                         QueryString = "Delete from TBL_Customer_Van_Map WHERE Customer_ID=@Customer_ID AND Site_Use_ID=@Site_Use_ID"
                         objSQLCmdNew = New SqlCommand(QueryString, objSQLConn)
                         objSQLCmdNew.CommandType = CommandType.Text
                         objSQLCmdNew.Parameters.AddWithValue("@Customer_ID", CustomerID)
                         objSQLCmdNew.Parameters.AddWithValue("@Site_Use_ID", SiteUseID)
                         objSQLCmdNew.Transaction = objSQLtrans
                         objSQLCmdNew.ExecuteNonQuery()


                        Using bulkCopy As SqlBulkCopy = _
                          New SqlBulkCopy(objSQLConn, SqlBulkCopyOptions.Default, objSQLtrans)
                            bulkCopy.DestinationTableName = "dbo.TBL_Customer_Van_Map"
                            bulkCopy.WriteToServer(dt)
                            bVanRel = True
                        End Using
                     Else
                        bVanRel = True
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
        If bShipSaved = True And bVanRel = True Then
            bRetVal = True
        End If
        Return bRetVal
    End Function
    Private Function GetVanOrgID(ByVal Salesrep_ID As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select * from TBL_Org_CTL_DTL where SalesRep_ID=" & Salesrep_ID)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustVisitsTbl")
            If MsgDs.Tables(0).Rows.Count > 0 Then
                GetVanOrgID = MsgDs.Tables(0).Rows(0)("Org_ID")
            End If
            objSQLCmd.Dispose()
        Catch ex As Exception
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
 Public Function GetOpenInvoices(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_ID As String, ByVal SiteID As String, ByVal Fromdate As String, ByVal ToDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "select b.Orig_Sys_Document_Ref,a.Invoice_Date ,a.Pending_Amount-isnull(amt,0) as Pending_Amount FROM TBL_Open_Invoices a INNER JOIN TBL_Order B ON A.Invoice_Ref_No=b.Orig_Sys_Document_Ref LEFT JOIN (select CI.Invoice_No,CI.Amount as amt from TBL_Collection C INNER JOIN TBL_Collection_Invoices CI on CI.Collection_ID=C.Collection_ID WHERE C.Status='W' AND Collection_Type='PDC')as X on X.Invoice_No=b.Orig_Sys_Document_Ref  WHERE A.Pending_Amount-isnull(amt,0) <>0 and a.Customer_ID=" & Customer_ID & " AND a.Site_Use_ID=" & SiteID & " and B.Creation_date>=convert(datetime,'" & Fromdate & "',103) and B.Creation_date<=convert(datetime,'" & ToDate & " 23:59:59',103) order by B.Creation_date desc"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text


            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustLstTbl")

            GetOpenInvoices = MsgDs.Tables(0)
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
    Public Function GetOpenReturns(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_ID As String, ByVal SiteID As String, ByVal Fromdate As String, ByVal ToDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "select b.Orig_Sys_Document_Ref,a.Invoice_Date ,abs(a.Pending_Amount)as Pending_Amount FROM TBL_Open_Invoices a INNER JOIN TBL_RMA B ON A.Invoice_Ref_No=b.Orig_Sys_Document_Ref WHERE A.Pending_Amount<>0 and a.Customer_ID=" & Customer_ID & " AND a.Site_Use_ID=" & SiteID & " and B.Creation_date>=convert(datetime,'" & Fromdate & "',103) and B.Creation_date<=convert(datetime,'" & ToDate & " 23:59:59',103) order by B.Creation_date desc"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text


            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustLstTbl")

            GetOpenReturns = MsgDs.Tables(0)
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
     Public Function SaveSettlement(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_ID As String, ByRef Site_Use_ID As String, ByRef userid As String, ByVal Doc_No_1 As String, ByVal Doc_No_2 As String, ByVal Settlement_Amount As String, ByVal Actual_Amount_1 As String, ByVal Actual_Amount_2 As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False

        Try
            Dim Dr As SqlDataReader
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_ReconcileReturns", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
             objSQLCmd.Parameters.Add(New SqlParameter("@Customer_ID", SqlDbType.Int))
            objSQLCmd.Parameters("@Customer_ID").Value = Customer_ID
            objSQLCmd.Parameters.Add(New SqlParameter("@Site_Use_ID", SqlDbType.Int))
            objSQLCmd.Parameters("@Site_Use_ID").Value = Val(Site_Use_ID)
            objSQLCmd.Parameters.Add(New SqlParameter("@userid", SqlDbType.Int))
            objSQLCmd.Parameters("@userid").Value = Val(userid)

            objSQLCmd.Parameters.Add(New SqlParameter("@Doc_No_1", SqlDbType.NVarChar))
            objSQLCmd.Parameters("@Doc_No_1").Value = Doc_No_1

            objSQLCmd.Parameters.Add(New SqlParameter("@Doc_No_2", SqlDbType.NVarChar))
            objSQLCmd.Parameters("@Doc_No_2").Value = Doc_No_2

            objSQLCmd.Parameters.Add(New SqlParameter("@Settlement_Amount", SqlDbType.Decimal))
            objSQLCmd.Parameters("@Settlement_Amount").Value = Settlement_Amount

            objSQLCmd.Parameters.Add(New SqlParameter("@Actual_Amount_1", SqlDbType.Decimal))
            objSQLCmd.Parameters("@Actual_Amount_1").Value = Actual_Amount_1

            objSQLCmd.Parameters.Add(New SqlParameter("@Actual_Amount_2", SqlDbType.Decimal))
            objSQLCmd.Parameters("@Actual_Amount_2").Value = Actual_Amount_2

            objSQLCmd.ExecuteNonQuery()
            bRetVal = True

            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        Catch ex As Exception
            Err_No = "7400920"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function
    Public Function DeleteDiscountDefinition(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_ID As String, ByRef Site_Use_ID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False

        Try
            Dim Dr As SqlDataReader
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("Delete from TBL_Customer_Addl_Info where Customer_ID=" & Customer_ID & " and Site_Use_ID=" & Site_Use_ID & " and Attrib_Name='DISCOUNT'", objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.ExecuteNonQuery()
            bRetVal = True

            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        Catch ex As Exception
            Err_No = "7400920"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function

    Public Function DeleteCustOrderDiscount(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_ID As String, ByRef Site_Use_ID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False

        Try
            Dim Dr As SqlDataReader
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("Delete from TBL_Customer_Addl_Info where Customer_ID=" & Customer_ID & " and Site_Use_ID=" & Site_Use_ID & " and Attrib_Name='CUST_ORDER_DISC_LIMIT'", objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.ExecuteNonQuery()
            bRetVal = True

            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        Catch ex As Exception
            Err_No = "7400925"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function
Public Function SaveOrderLvlCustomerDiscount(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_ID As String, ByRef Site_Use_ID As String, ByVal DiscountType As String, ByVal MinOrderval As String, ByVal Discount As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False

        Try
            Dim Dr As SqlDataReader
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_SaveCustomoerOrderlvlDiscount", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
             objSQLCmd.Parameters.Add(New SqlParameter("@CustomerID", SqlDbType.Int))
            objSQLCmd.Parameters("@CustomerID").Value = Customer_ID
            objSQLCmd.Parameters.Add(New SqlParameter("@SiteUSeID", SqlDbType.Int))
            objSQLCmd.Parameters("@SiteUSeID").Value = Val(Site_Use_ID)

            objSQLCmd.Parameters.Add(New SqlParameter("@DiscountType", SqlDbType.VarChar))
            objSQLCmd.Parameters("@DiscountType").Value = DiscountType

           objSQLCmd.Parameters.Add(New SqlParameter("@MinOrdervalue", SqlDbType.Decimal))
            objSQLCmd.Parameters("@MinOrdervalue").Value = MinOrderval

            objSQLCmd.Parameters.Add(New SqlParameter("@Discount", SqlDbType.Decimal))
            objSQLCmd.Parameters("@Discount").Value = Discount

            objSQLCmd.Parameters.Add("@Status", SqlDbType.Int)
            objSQLCmd.Parameters("@Status").Direction = ParameterDirection.Output


            objSQLCmd.ExecuteNonQuery()

            Dim Status As String
            Status = objSQLCmd.Parameters("@Status").Value.ToString()
            bRetVal = True

            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        Catch ex As Exception
            Err_No = "7400920"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function


    Public Function SaveCustOrderDiscount(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Customer_ID As String, ByRef Site_Use_ID As String, ByVal MinOrderValue As String, ByVal MinDisc As String, ByVal MaxDisc As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False

        Try
            Dim Dr As SqlDataReader
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_SaveCustOrderDiscount", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@CustomerID", SqlDbType.Int))
            objSQLCmd.Parameters("@CustomerID").Value = Customer_ID
            objSQLCmd.Parameters.Add(New SqlParameter("@SiteUSeID", SqlDbType.Int))
            objSQLCmd.Parameters("@SiteUSeID").Value = Val(Site_Use_ID)

            objSQLCmd.Parameters.Add(New SqlParameter("@MinOrdervalue", SqlDbType.Decimal))
            objSQLCmd.Parameters("@MinOrdervalue").Value = MinOrderValue

            objSQLCmd.Parameters.Add(New SqlParameter("@MinDiscount", SqlDbType.Decimal))
            objSQLCmd.Parameters("@MinDiscount").Value = Val(MinDisc) / 100.0

         

            objSQLCmd.Parameters.Add(New SqlParameter("@MaxDiscount", SqlDbType.Decimal))
            objSQLCmd.Parameters("@MaxDiscount").Value = Val(MaxDisc) / 100.0

            objSQLCmd.Parameters.Add("@Status", SqlDbType.Int)
            objSQLCmd.Parameters("@Status").Direction = ParameterDirection.Output


            objSQLCmd.ExecuteNonQuery()

            Dim Status As String
            Status = objSQLCmd.Parameters("@Status").Value.ToString()
            bRetVal = True

            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        Catch ex As Exception
            Err_No = "5400920"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function

    Public Function DeleteBonusPlanToCustomer(ByVal CustID As Integer, ByVal SiteUseID As Integer, ByVal OrgId As String, ByVal PlanId As Integer, ByVal Mode As String, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_DeleteBonusPlanToCustomers"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure

            objSQLCmd.Parameters.Add("@CustomerID", SqlDbType.BigInt).Value = CustID
            objSQLCmd.Parameters.Add("@SiteUseID", SqlDbType.BigInt).Value = SiteUseID
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 20).Value = OrgId
            objSQLCmd.Parameters.Add("@PlanId", SqlDbType.Int).Value = PlanId
            objSQLCmd.Parameters.Add("@DeleteMode", SqlDbType.VarChar, 20).Value = Mode


            objSQLCmd.ExecuteNonQuery()
            success = True
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "44095"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function


    Public Function InsertBonusPlanToCustomer(ByVal CustID As Integer, ByVal SiteUseID As Integer, ByVal OrgId As String, ByVal PlanId As Integer, ByVal Mode As String, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_InsertBonusPlanToCustomers"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure

            objSQLCmd.Parameters.Add("@CustomerID", SqlDbType.BigInt).Value = CustID
            objSQLCmd.Parameters.Add("@SiteUseID", SqlDbType.BigInt).Value = SiteUseID
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 20).Value = OrgId
            objSQLCmd.Parameters.Add("@PlanId", SqlDbType.Int).Value = PlanId
            objSQLCmd.Parameters.Add("@InsertMode", SqlDbType.VarChar, 20).Value = Mode


            objSQLCmd.ExecuteNonQuery()
            success = True
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "44095"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
    Public Function GetAssignedCustomersBonusPlan(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal OrgID As String, ByVal PlanId As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim sQry As String
        Dim sRetVal As String = ""
        Dim dtAssigned As New DataTable
        Try

            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            sQry = "app_GetAssignedBonusPlanCustomer"
            objSQLDA = New SqlDataAdapter(sQry, objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            objSQLDA.SelectCommand.Parameters.Add("@PlanID", SqlDbType.BigInt).Value = PlanId
            objSQLDA.Fill(dtAssigned)
            objSQLDA.Dispose()
        Catch ex As Exception
            sRetVal = sRetVal & ex.Message
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtAssigned
    End Function
    Public Function GetAvailableCustomersBonusPlan(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal PlanId As Integer, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim sQry As String
        Dim sRetVal As String = ""
        Dim dtAvailable As New DataTable
        Try

            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            sQry = "app_GetDefaultBonusPlanCustomer"
            objSQLDA = New SqlDataAdapter(sQry, objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            objSQLDA.SelectCommand.Parameters.Add("@PlanID", SqlDbType.BigInt).Value = PlanId

            objSQLDA.Fill(dtAvailable)
            objSQLDA.Dispose()
        Catch ex As Exception
            sRetVal = sRetVal & ex.Message
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtAvailable
    End Function

End Class
