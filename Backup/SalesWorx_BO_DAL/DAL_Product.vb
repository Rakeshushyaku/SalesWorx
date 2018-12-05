Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Data
Public Class DAL_Product
    Private _objDB As DatabaseConnection
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Public Function GetBonusProductDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal Description As String, ByVal OrgId As String) As DataRow
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT Inventory_item_Id as ItemId, A.Item_Code As ItemCode,A.Description  AS Description, Primary_UOM_Code AS UOM ,Organization_Id as OrgId FROM TBL_Product AS A WHERE A.organization_ID=@OrgID AND A.Item_Code =@ItemCOde OR A.Description =@Description")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 100).Value = ItemCode
            objSQLCmd.Parameters.Add("@Description", SqlDbType.NVarChar, 500).Value = Description
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = orgid
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            If dt.Rows.Count > 0 Then
                dr = dt.Rows(0)
            End If
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24001"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dr
    End Function
    Public Function GetProductList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("Select distinct Case When a.Row_ID is null Then 'No' Else 'Yes' End IsMSL,b.Item_No,b.[Description],b.Item_Code,b.Brand_Code,b.Item_Size,b.EANNO,b.Primary_UOM_Code,b.Promo_Item,b.Inventory_Item_ID From TBL_Product_MSL as a right outer join dbo.TBL_Product as b on a.Inventory_Item_ID = b.Inventory_Item_ID and a.Organization_ID=b.Organization_ID Where 1=1 {0} order by b.Brand_Code Asc, b.[Item_Code] ASC", _sSearchParams, QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "PrdLstTbl")

            GetProductList = MsgDs
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
    Public Function ValidItem(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Item As String, ByRef Item_ID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable
        Dim BRetval As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection

            objSQLDA = New SqlDataAdapter("Select Inventory_Item_ID from TBL_Product where Item_No='" & Item & "' and Organization_ID='" & OrgID & "'", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.Fill(dtDivConfig)
            objSQLDA.Dispose()
            If dtDivConfig.Rows.Count > 0 Then
                Item_ID = dtDivConfig.Rows(0)("Inventory_Item_ID").ToString
                BRetval = True
            End If

        Catch ex As Exception
            Err_No = "74206"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return BRetval
    End Function
       Public Function UploadFOCDiscount(ByVal dtData As DataTable, ByVal OrgID As String, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Dim tran As SqlTransaction = Nothing
        Dim QueryString As String = Nothing
        Try
            dtData.Columns.Add("Inventory_item_ID")
            For Each r As DataRow In dtData.Rows
                Dim itemID As String = ""
                ValidItem(Err_No, Err_Desc, OrgID, r("Item_no").ToString, itemID)
                r("Inventory_item_ID") = itemID
            Next
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            tran = objSQLConn.BeginTransaction()
            Dim objLogin As New SalesWorx.BO.DAL.DAL_Login

            For Each r As DataRow In dtData.Rows
                If r("IsValid").ToString() = "Y" Then

                    QueryString = "app_SaveDiscFOC"
                    objSQLCmd = New SqlCommand(QueryString, objSQLConn)
                    objSQLCmd.CommandType = CommandType.StoredProcedure
                     objSQLCmd.Parameters.Add(New SqlParameter("@OrgID", SqlDbType.VarChar))
            objSQLCmd.Parameters("@OrgID").Value = OrgID
            objSQLCmd.Parameters.Add(New SqlParameter("@Inventory_item_ID", SqlDbType.Int))
            objSQLCmd.Parameters("@Inventory_item_ID").Value = r("Inventory_item_ID")
            objSQLCmd.Parameters.Add(New SqlParameter("@opt", SqlDbType.Int))
            objSQLCmd.Parameters("@opt").Value = 1

            objSQLCmd.Parameters.Add("@Status", SqlDbType.Int)
            objSQLCmd.Parameters("@Status").Direction = ParameterDirection.Output
                    objSQLCmd.Transaction = tran
                    objSQLCmd.ExecuteNonQuery()

            Dim Status As String
            Status = objSQLCmd.Parameters("@Status").Value.ToString()
            objSQLCmd.Dispose()
            If Status = "2" Then
                    objLogin.InsertUserLog(Err_No, Err_Desc, "U", "MAIN MENU", "DISC FOC ITEM", r("Inventory_item_ID"), "Org: " & OrgID, UserID.ToString(), "0", "0")
            ElseIf Status = "1" Then
                  objLogin.InsertUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "DISC FOC ITEM", r("Inventory_item_ID"), "Org: " & OrgID, UserID.ToString(), "0", "0")
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
    Public Function UploadDiscount(ByVal dtData As DataTable, ByVal OrgID As String, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
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

                    QueryString = "app_SaveDiscountData"
                    objSQLCmd = New SqlCommand(QueryString, objSQLConn)
                    objSQLCmd.CommandType = CommandType.StoredProcedure
                    objSQLCmd.Parameters.AddWithValue("@OrgId", OrgID)
                    objSQLCmd.Parameters.AddWithValue("@ItemCode", r("ItemCode").ToString())
                    objSQLCmd.Parameters.AddWithValue("@DisType", IIf(r("DiscountType").ToString().ToUpper() = "PERCENTAGE", "P", "V"))
                    objSQLCmd.Parameters.AddWithValue("@ValidFrom", CDate(r("ValidFrom").ToString()))
                    objSQLCmd.Parameters.AddWithValue("@ValidTo", CDate(r("ValidTo").ToString()))

                    objSQLCmd.Parameters.AddWithValue("@FromQty", CLng(r("FromQty").ToString()))
                    objSQLCmd.Parameters.AddWithValue("@Rate", CDec(r("Value").ToString()))
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
    Public Function CheckDiscountDataRange(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal FromQty As Long, ByVal OrgID As String, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, ByVal LineID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False


        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = Nothing
            If LineID = 0 Then
                QueryString = "SELECT    COUNT(*)  FROM   TBL_Discount AS A  WHERE  A.Item_Code=@ItemCode AND A.Organization_ID=@OrgID AND  From_Qty=@FromQty AND  (( @ValidFrom BETWEEN Valid_From AND Valid_To  ) or (Valid_To BETWEEN @ValidFrom AND @ValidTo))"
            Else
                QueryString = "SELECT    COUNT(*)  FROM   TBL_Discount AS A  WHERE  A.Item_Code=@ItemCode AND A.Organization_ID=@OrgID AND  From_Qty=@FromQty AND  (( @ValidFrom BETWEEN Valid_From AND Valid_To  ) or (Valid_To BETWEEN @ValidFrom AND @ValidTo)) AND Discount_ID<>@LineID"
            End If
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@LineID", SqlDbType.Int).Value = LineID
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 100).Value = ItemCode
            objSQLCmd.Parameters.Add("@FromQty", SqlDbType.Int).Value = FromQty
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            objSQLCmd.Parameters.Add("@ValidFrom", SqlDbType.DateTime).Value = ValidFrom
            objSQLCmd.Parameters.Add("@ValidTo", SqlDbType.DateTime).Value = ValidTo
            Dim cnt As Integer = 0
            cnt = Convert.ToInt32(objSQLCmd.ExecuteScalar())
            If cnt > 0 Then
                success = True
            End If
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "13062"
            Err_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function

    Public Function CheckBonusDataActiveRange(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal FromQty As Long, ByVal ToQty As Long, ByVal OrgID As String, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, ByVal LineID As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = Nothing
            If LineID = 0 Then
                QueryString = "SELECT     A.BNS_Promotion_ID AS ActiveLineID, A.Item_Code +'-'+ B.Description AS ItemName, A.Valid_From, A.Valid_To, A.Prom_Qty_From, A.Prom_Qty_To, A.Get_Qty FROM         TBL_BNS_Promotion AS A INNER JOIN                     TBL_Product AS B ON A.Organization_ID = B.Organization_ID AND A.Item_Code = B.Item_Code  WHERE  A.Item_Code=@ItemCode and A.Organization_ID=@OrgID AND  ( (Prom_Qty_From BETWEEN  @FromQty and @ToQty or (@FromQty BETWEEN Prom_Qty_From AND Prom_Qty_To )) AND  ( Valid_From BETWEEN   @ValidFrom and @ValidTo or(@ValidFrom BETWEEN Valid_From AND Valid_To  ))  AND Is_Active='Y'  or ((Prom_Qty_To BETWEEN   @FromQty  and @ToQty or (@ToQty BETWEEN   Prom_Qty_From  and Prom_Qty_To))  AND     (Valid_To BETWEEN @ValidFrom AND @ValidTo or(@ValidTo BETWEEN Valid_From AND Valid_To  ))  AND Is_Active='Y') )"
            Else
                QueryString = "SELECT     A.BNS_Promotion_ID AS ActiveLineID, A.Item_Code +'-'+ B.Description AS ItemName, A.Valid_From, A.Valid_To, A.Prom_Qty_From, A.Prom_Qty_To, A.Get_Qty FROM         TBL_BNS_Promotion AS A INNER JOIN                     TBL_Product AS B ON A.Organization_ID = B.Organization_ID AND A.Item_Code = B.Item_Code  WHERE A.Bns_Promotion_ID<>@LineID AND  A.Item_Code=@ItemCode and A.Organization_ID=@OrgID AND  ( (Prom_Qty_From BETWEEN  @FromQty and @ToQty or (@FromQty BETWEEN Prom_Qty_From AND Prom_Qty_To )) AND  ( Valid_From BETWEEN   @ValidFrom and @ValidTo or(@ValidFrom BETWEEN Valid_From AND Valid_To  ))  AND Is_Active='Y'  or ((Prom_Qty_To BETWEEN   @FromQty  and @ToQty or (@ToQty BETWEEN   Prom_Qty_From  and Prom_Qty_To))  AND     (Valid_To BETWEEN @ValidFrom AND @ValidTo or(@ValidTo BETWEEN Valid_From AND Valid_To  ))  AND Is_Active='Y') )"
            End If
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@LineID", SqlDbType.Int).Value = LineID
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 100).Value = ItemCode
            objSQLCmd.Parameters.Add("@FromQty", SqlDbType.Int).Value = FromQty
            objSQLCmd.Parameters.Add("@ToQty", SqlDbType.Int).Value = ToQty
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            objSQLCmd.Parameters.Add("@ValidFrom", SqlDbType.DateTime).Value = ValidFrom
            objSQLCmd.Parameters.Add("@ValidTo", SqlDbType.DateTime).Value = ValidTo

            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24066"
            Err_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function CheckBonusData(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal FromQty As Long, ByVal ToQty As Long, ByVal OrgID As String, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select COUNT(*) FROM TBL_BNS_Promotion WHERE  Item_Code=@ItemCode and Organization_ID=@OrgID AND  (@FromQty BETWEEN   Prom_Qty_From and Prom_Qty_To AND @ValidFrom BETWEEN   Valid_From and Valid_To  AND Is_Valid='Y'  or (@ToQty BETWEEN   Prom_Qty_From and Prom_Qty_To AND  @ValidTo BETWEEN   Valid_From and Valid_To AND IS_Valid='Y') )")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 100).Value = ItemCode
            objSQLCmd.Parameters.Add("@FromQty", SqlDbType.Int).Value = FromQty
            objSQLCmd.Parameters.Add("@ToQty", SqlDbType.Int).Value = ToQty
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            objSQLCmd.Parameters.Add("@ValidFrom", SqlDbType.DateTime).Value = ValidFrom
            objSQLCmd.Parameters.Add("@ValidTo", SqlDbType.DateTime).Value = ValidTo

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
    Public Function SaveBonusData(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal ItemCode As String, ByVal OrgId As String, ByVal DUOm As String, ByVal BItemCode As String, ByVal BUOM As String, ByVal TypeCode As String, ByVal FromQty As Long, ByVal ToQty As Long, ByVal GetQty As Long, ByVal Getper As Decimal, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, ByVal CreatedBy As Integer, ByVal BnsPlanID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "INSERT INTO TBL_BNS_Promotion (Item_Code ,Organization_ID ,Item_UOM,Valid_From,Valid_To,Prom_Qty_From ,Prom_Qty_To ,Price_Break_Type_Code ,Get_Item ,Get_UOM,Get_Qty ,Get_Add_Per ,Sync_Timestamp,Created_By,Created_At,Bns_Plan_ID)VALUES(@ItemCode,@OrgId,@DUOm,@ValidFrom,@ValidTo,@FromQty,@ToQty,@TypeCode,@BItemCode,@BUOM,@GetQty,@getPer,GetDate(),@CreatedBy,GetDate(),@BnsPlanID)"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@ItemCode", ItemCode)
            objSQLCmd.Parameters.AddWithValue("@OrgId", OrgId)
            objSQLCmd.Parameters.AddWithValue("@DUOM", DUOm)
            objSQLCmd.Parameters.AddWithValue("@ValidFrom", ValidFrom)
            objSQLCmd.Parameters.AddWithValue("@ValidTo", ValidTo)
            objSQLCmd.Parameters.AddWithValue("@TypeCode", TypeCode.ToUpper())
            objSQLCmd.Parameters.AddWithValue("@FromQty", FromQty)
            objSQLCmd.Parameters.AddWithValue("@ToQty", ToQty)
            objSQLCmd.Parameters.AddWithValue("@BItemCode", BItemCode)
            objSQLCmd.Parameters.AddWithValue("@BUOM", BUOM)
            objSQLCmd.Parameters.AddWithValue("@GetQty", GetQty)
            objSQLCmd.Parameters.AddWithValue("@GetPer", Getper / 100)
            objSQLCmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
            objSQLCmd.Parameters.AddWithValue("@BnsPlanID", BnsPlanID)
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

    Public Function SaveDiscountData(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal ItemCode As String, ByVal OrgId As String, ByVal TypeCode As String, ByVal FromQty As Long, ByVal Rate As Decimal, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "app_SaveDiscountData"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@OrgId", OrgId)
            objSQLCmd.Parameters.AddWithValue("@ItemCode", ItemCode)
            objSQLCmd.Parameters.AddWithValue("@DisType", TypeCode.ToUpper())
            objSQLCmd.Parameters.AddWithValue("@ValidFrom", ValidFrom)
            objSQLCmd.Parameters.AddWithValue("@ValidTo", ValidTo)

            objSQLCmd.Parameters.AddWithValue("@FromQty", FromQty)
            objSQLCmd.Parameters.AddWithValue("@Rate", Rate)
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()




            sucess = True
        Catch ex As Exception
            Error_No = 55014

        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return sucess

    End Function

    Public Function UpdateDiscountData(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal LineID As Integer, ByVal ItemCode As String, ByVal OrgId As String, ByVal TypeCode As String, ByVal FromQty As Long, ByVal Rate As Decimal, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "app_UpdateDiscountData"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@LineId", LineID)
            objSQLCmd.Parameters.AddWithValue("@OrgId", OrgId)
            objSQLCmd.Parameters.AddWithValue("@ItemCode", ItemCode)
            objSQLCmd.Parameters.AddWithValue("@DisType", TypeCode.ToUpper())
            objSQLCmd.Parameters.AddWithValue("@ValidFrom", ValidFrom)
            objSQLCmd.Parameters.AddWithValue("@ValidTo", ValidTo)

            objSQLCmd.Parameters.AddWithValue("@FromQty", FromQty)
            objSQLCmd.Parameters.AddWithValue("@Rate", Rate)
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()




            sucess = True
        Catch ex As Exception
            Error_No = 55014

        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return sucess

    End Function

    Public Function DeleteDiscountData(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal LineId As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("DELETE FROM TBL_Discount WHERE Discount_ID=@LineID")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@LineId", LineId)
            objSQLCmd.ExecuteNonQuery()
            success = True
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "94061"
            Err_Desc = ex.Message

            success = False
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
    Public Function DeleteBonusData(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal LineId As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("DELETE FROM TBL_BNS_Promotion WHERE BNS_Promotion_ID=@LineID")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@LineId", SqlDbType.VarChar, 100).Value = LineId
            objSQLCmd.ExecuteNonQuery()
            success = True
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "22061"
            Err_Desc = ex.Message

            success = False
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
    Public Function UpdateBonusPlan(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal PlanId As String, ByVal PlanName As String, ByVal CreatedBy As Integer, ByVal IsActive As String, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "UPDATE TBL_BNS_Assortment_Plan  SET Description=@Description ,Valid_From=@ValidFrom,Valid_To=@ValidTo,Last_Updated_At=GetDate(),Last_Updated_By=@CreatedBy WHERE Assortment_Plan_ID=@PlanID"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@PlanID", PlanId)
            objSQLCmd.Parameters.AddWithValue("@Description", PlanName)
            objSQLCmd.Parameters.AddWithValue("@ValidFrom", ValidFrom)
            objSQLCmd.Parameters.AddWithValue("@ValidTo", ValidTo)
            'objSQLCmd.Parameters.AddWithValue("@IsActive", IsActive)
            objSQLCmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)

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
    Public Function SaveBonusPlan(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal OrgId As String, ByVal PlanName As String, ByVal CreatedBy As Integer, ByRef PlanId As String, ByVal IsActive As String, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "INSERT INTO TBL_BNS_Assortment_Plan (Description ,Organization_ID ,Valid_From,Valid_To,Is_Active, Created_At,Created_By)VALUES(@PlanName,@OrgId,@ValidFrom,@ValidTo,@isActive, GETDATE(),@CreatedBy)"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@PlanName", PlanName)
            objSQLCmd.Parameters.AddWithValue("@OrgId", OrgId)
            objSQLCmd.Parameters.AddWithValue("@ValidFrom", ValidFrom)
            objSQLCmd.Parameters.AddWithValue("@ValidTo", ValidTo)
            objSQLCmd.Parameters.AddWithValue("@IsActive", IsActive)
            objSQLCmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)

            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()

            objSQLCmd = New SqlCommand("select @@IDENTITY as PlanID", objSQLConn)
            PlanId = CStr(objSQLCmd.ExecuteScalar())
            objSQLCmd.Dispose()

            sucess = True
        Catch ex As Exception
            Error_No = 75014
            ' Error_Desc = String.Format("Error while saving Order", ex.Message)

        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return PlanId

    End Function

    Public Function UpdateSimpleBonusPlan(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal PlanId As String, ByVal PlanName As String, ByVal CreatedBy As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "UPDATE TBL_BNS_Plan  SET Description=@Description ,Last_Updated_At=GetDate(),Last_Updated_By=@CreatedBy WHERE Bns_Plan_ID=@PlanID"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@PlanID", PlanId)
            objSQLCmd.Parameters.AddWithValue("@Description", PlanName)
            objSQLCmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)

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
    Public Function SaveSimpleBonusPlan(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal OrgId As String, ByVal PlanName As String, ByVal CreatedBy As Integer, ByRef PlanId As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "INSERT INTO TBL_BNS_Plan (Description ,Organization_ID , Created_At,Created_By)VALUES(@PlanName,@OrgId, GETDATE(),@CreatedBy)"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@PlanName", PlanName)
            objSQLCmd.Parameters.AddWithValue("@OrgId", OrgId)
            objSQLCmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)

            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()

            objSQLCmd = New SqlCommand("select @@IDENTITY as PlanID", objSQLConn)
            PlanId = CStr(objSQLCmd.ExecuteScalar())
            objSQLCmd.Dispose()

            sucess = True
        Catch ex As Exception
            Error_No = 75014
            ' Error_Desc = String.Format("Error while saving Order", ex.Message)

        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return PlanId

    End Function
  Public Function GetFOCDefinition(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT Item_Code+'-' + Description AS Description ,A.Inventory_Item_ID AS ItemID,Item_Code,Item_No FROM TBL_Product A INNER JOIN TBL_Product_Addl_Info B on A.Inventory_Item_ID=B.Inventory_Item_ID and A.Organization_ID=B.Organization_ID  WHERE A.Organization_ID=@OrgID and B.Attrib_Name='DISC_FOC'  ORDER By Description ")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            Dim MsgDs As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs)
            GetFOCDefinition = MsgDs
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "92124"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function SaveDiscountFOC(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgId As String, ByRef Inventory_item_ID As String, ByVal Opt As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False

        Try
            Dim Dr As SqlDataReader
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_SaveDiscFOC", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
             objSQLCmd.Parameters.Add(New SqlParameter("@OrgID", SqlDbType.VarChar))
            objSQLCmd.Parameters("@OrgID").Value = OrgId
            objSQLCmd.Parameters.Add(New SqlParameter("@Inventory_item_ID", SqlDbType.Int))
            objSQLCmd.Parameters("@Inventory_item_ID").Value = Val(Inventory_item_ID)
            objSQLCmd.Parameters.Add(New SqlParameter("@opt", SqlDbType.Int))
            objSQLCmd.Parameters("@opt").Value = Val(Opt)

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
    Public Function GetProductListByOrgID(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT Item_Code+'-' + Description AS Description ,Inventory_Item_ID AS ItemID,Item_Code FROM TBL_Product WHERE Organization_ID=@OrgID  ORDER By Description ")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            Dim MsgDs As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs)
            Dim r As DataRow = MsgDs.NewRow()
            r(0) = ""
            r(1) = "0"
            MsgDs.Rows.InsertAt(r, 0)
            GetProductListByOrgID = MsgDs
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "92124"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function LoadBonusProductList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT Item_Code AS DescValue,Description AS DescText,Item_Code AS CodeValue,Item_Code AS CodeText FROM TBL_Product WHERE Organization_ID=@OrgID  ORDER By Description ")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            Dim MsgDs As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs)
         
            LoadBonusProductList = MsgDs
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "740243"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function LoadProductList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT Item_Code + '$' + Primary_UOM_Code AS DescValue,Description AS DescText,Item_Code + '$' + Primary_UOM_Code AS CodeValue,Item_Code AS CodeText FROM TBL_Product WHERE Organization_ID=@OrgID  ORDER By Description ")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            Dim MsgDs As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs)
            LoadProductList = MsgDs
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "740243"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function LoadAssortmentSlabs(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PlanId As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("app_LoadAssortmentSlabs")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@PlanID", PlanId)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")
            LoadAssortmentSlabs = MsgDs.Tables(0)
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "740243"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function LoadAssortmentItems(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PlanId As Integer, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("app_LoadAssortmentItems")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@PlanID", PlanId)
            objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")
            LoadAssortmentItems = MsgDs.Tables(0)
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "740243"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function LoadSimpleBonusPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UID As Integer, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT DISTINCT A.Bns_Plan_ID,A.Description,A.Organization_ID AS OrgID,(SELECT Description FROM TBL_Org_CTL_H WHERE ORG_HE_ID =B.MAS_Org_ID) AS  OrgName,A.Last_Updated_At AS UpdatedAt ,(SELECT userName FROM TBL_user WHERE User_ID=A.Last_Updated_BY)AS UpdatedBy,(SELECT COUNT(DISTINCT Item_Code) FROM TBL_BNS_Promotion WHERE Bns_Plan_ID=A.Bns_Plan_ID)AS TotItems,(SELECT COUNT(*) FROM TBL_Customer_Bonus_Map  AS X INNER JOIN dbo.app_GetOrgCustomerShipAddress(A.Organization_ID)AS Y ON X.Customer_ID =Y.Customer_ID AND X.Site_Use_ID =Y.Site_Use_ID WHERE Bonus_Plan_Id=A.Bns_Plan_ID AND Plan_Type='SIMPLE')AS TotCustomers FROM TBL_BNS_Plan  AS A INNER JOIN  app_GetControlInfo(@UID) AS B ON A.Organization_Id=B.Mas_Org_ID ORDER BY A.Last_Updated_At DESC")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@UID", UID)
            ' objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")
            LoadSimpleBonusPlan = MsgDs.Tables(0)
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "540243"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function LoadBonusPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UID As Integer, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT DISTINCT A.Assortment_Plan_ID,A.Description,A.Organization_ID AS OrgID,B.Site AS OrgName,A.Valid_From,A.Valid_To,A.Last_Updated_At AS UpdatedAt ,(SELECT userName FROM TBL_user WHERE User_ID=A.Last_Updated_BY)AS UpdatedBy,CASE WHEN A.Is_Active='Y' THEN 'Yes' ELSE 'No' END AS IsActive,CASE WHEN A.Is_Active='Y' THEN CASt('1' AS Bit) ELSE CASt('0' AS Bit) END AS IsAddItems FROM TBL_BNS_Assortment_Plan  AS A INNER JOIN  app_GetControlInfo(@UID) AS B ON A.Organization_Id=B.Mas_Org_ID WHERE A.Organization_Id=@OrgID AND Is_Active='Y'  ORDER BY A.Last_Updated_At DESC")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@UID", UID)
            objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")
            LoadBonusPlan = MsgDs.Tables(0)
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "740243"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function DeleteSimpleBonusPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PlanId As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            '            Dim QueryString As String = String.Format("DELETE FROM TBL_BNS_Assortment_Items WHERE Assortment_Plan_ID =@PlanID;DELETE FROM TBL_BNS_Assortment_Slabs WHERE Assortment_Plan_ID=@PlanID;DELETE FROM TBL_BNS_Assortment_Plan WHERE Assortment_Plan_ID=@PlanID")
            Dim QueryString As String = String.Format("DELETE FROM TBL_BNS_Promotion WHERE Bns_Plan_ID=@PlanID;DELETE FROM TBL_BNS_Plan WHERE Bns_Plan_ID=@PlanID;DELETE FROM TBL_Customer_Bonus_Map WHERE Bonus_Plan_ID=@PlanID")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@PlanId", SqlDbType.VarChar, 100).Value = PlanId
            objSQLCmd.ExecuteNonQuery()
            success = True
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "22065"
            Err_Desc = ex.Message

            success = False
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function

    Public Function DeleteBonusPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PlanId As String, ByVal UpdatedBy As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            '            Dim QueryString As String = String.Format("DELETE FROM TBL_BNS_Assortment_Items WHERE Assortment_Plan_ID =@PlanID;DELETE FROM TBL_BNS_Assortment_Slabs WHERE Assortment_Plan_ID=@PlanID;DELETE FROM TBL_BNS_Assortment_Plan WHERE Assortment_Plan_ID=@PlanID")
            Dim QueryString As String = String.Format("UPDATE TBL_BNS_Assortment_Plan SET Is_Active='N',Last_Updated_At=GetDate(),Last_Updated_By=@UpdatedBy WHERE Assortment_Plan_ID=@PlanID")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@PlanId", SqlDbType.VarChar, 100).Value = PlanId
            objSQLCmd.Parameters.Add("@UpdatedBy", SqlDbType.Int).Value = UpdatedBy
            objSQLCmd.ExecuteNonQuery()
            success = True
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "22065"
            Err_Desc = ex.Message

            success = False
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
    Public Function GetOrgsHeads(ByRef Err_No As Long, ByRef Err_Desc As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("Select distinct ORG_HE_ID,Description From TBL_Org_CTL_H order by Description")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "PrdLstTbl")

            GetOrgsHeads = MsgDs
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
    Public Function SaveAssortment(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PlanID As String, ByVal dtOrdItems As DataTable, ByVal dtGetItems As DataTable, ByVal dtSlabs As DataTable, ByVal CreatedBy As Integer) As Boolean

        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False
        Dim tran As SqlTransaction = Nothing


        Try
            'getting MSSQL DB connection.....
            'Delete all items and slabs in assortment table
            objSQLConn = _objDB.GetSQLConnection
            tran = objSQLConn.BeginTransaction()
            sQry = "DELETE FROM TBL_BNS_Assortment_Items WHERE Assortment_Plan_ID =@PlanID;DELETE FROM TBL_BNS_Assortment_Slabs WHERE Assortment_Plan_ID=@PlanID;"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@PlanID", PlanID)
            objSQLCmd.Transaction = tran
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()

            'Insert TBL_assortment_item
            objSQLCmd = New SqlCommand("app_SaveAssortmentItems", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Transaction = tran
            objSQLCmd.Parameters.Add("@PlanID", SqlDbType.BigInt)
            objSQLCmd.Parameters.Add("@CreatedBy", SqlDbType.Int)
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 122)
            objSQLCmd.Parameters.Add("@ItemUOM", SqlDbType.VarChar, 30)
            objSQLCmd.Parameters.Add("@IsGetItem", SqlDbType.Char, 1)
            objSQLCmd.Parameters.Add("@IsMandatory", SqlDbType.Char, 1)
        
            For Each dr As DataRow In dtOrdItems.Rows
                objSQLCmd.Parameters("@PlanID").Value = PlanID
                objSQLCmd.Parameters("@CreatedBy").Value = CreatedBy
                objSQLCmd.Parameters("@ItemCode").Value = dr("ItemCode").ToString()
                objSQLCmd.Parameters("@ItemUOM").Value = dr("UOM").ToString()
                objSQLCmd.Parameters("@IsGetItem").Value = dr("GetItem").ToString()
                objSQLCmd.Parameters("@IsMandatory").Value = IIf(dr("IsMandatory").ToString() = "False", "N", "Y")
                objSQLCmd.ExecuteNonQuery()
            Next


            For Each dr As DataRow In dtGetItems.Rows
                objSQLCmd.Parameters("@PlanID").Value = PlanID
                objSQLCmd.Parameters("@CreatedBy").Value = CreatedBy
                objSQLCmd.Parameters("@ItemCode").Value = dr("ItemCode").ToString()
                objSQLCmd.Parameters("@ItemUOM").Value = dr("UOM").ToString()
                objSQLCmd.Parameters("@IsGetItem").Value = dr("GetItem").ToString()
                objSQLCmd.Parameters("@IsMandatory").Value = "N"
                objSQLCmd.ExecuteNonQuery()
            Next

            objSQLCmd.Dispose()


            'Insert TBL_Sssortment_slabs
            objSQLCmd = New SqlCommand("app_SaveAssortmentSlabs", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Transaction = tran
            objSQLCmd.Parameters.Add("@PlanID", SqlDbType.BigInt)
            objSQLCmd.Parameters.Add("@CreatedBy", SqlDbType.Int)
            objSQLCmd.Parameters.Add("@FromQty", SqlDbType.Decimal)
            objSQLCmd.Parameters.Add("@ToQty", SqlDbType.Decimal)
            objSQLCmd.Parameters.Add("@BreakType", SqlDbType.VarChar, 30)
            objSQLCmd.Parameters.Add("@GetQty", SqlDbType.Decimal)

            For Each h As DataRow In dtSlabs.Rows
                objSQLCmd.Parameters("@PlanID").Value = PlanID
                objSQLCmd.Parameters("@CreatedBy").Value = CreatedBy
                objSQLCmd.Parameters("@FromQty").Value = CLng(h("FromQty").ToString())
                objSQLCmd.Parameters("@ToQty").Value = CLng(h("ToQty").ToString())
                objSQLCmd.Parameters("@BreakType").Value = h("TypeCode").ToString()
                objSQLCmd.Parameters("@GetQty").Value = CLng(h("GetQty").ToString())
                objSQLCmd.ExecuteNonQuery()
            Next

            objSQLCmd.Dispose()







            sucess = True
            tran.Commit()
        Catch ex As Exception
            tran.Rollback()
            Err_No = 75014
            ' Error_Desc = String.Format("Error while saving assortment", ex.Message)

        Finally
            tran.Dispose()
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return sucess

    End Function
    Public Function CheckAssortmentSlab(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PlanID As String, ByVal SlabID As String, ByVal FromQTy As Long, ByVal ToQty As Long) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT    COUNT(*)  FROM         TBL_BNS_Assortment_Slabs  AS A   WHERE CAST(A.Assortment_Slab_ID AS NVARCHAR(100))<>CAST(@SlabID AS NVARCHAR(100)) AND  A.Assortment_Plan_ID =@PlanID    AND  ( (Prom_Qty_From BETWEEN  @FromQty and @ToQty or (@FromQty BETWEEN Prom_Qty_From AND Prom_Qty_To ))       or ((Prom_Qty_To BETWEEN   @FromQty  and @ToQty or (@ToQty BETWEEN   Prom_Qty_From  and Prom_Qty_To)) ) )")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@PlanID", SqlDbType.VarChar, 100).Value = PlanID
            objSQLCmd.Parameters.Add("@SlabID", SqlDbType.NVarChar, 100).Value = SlabID
            objSQLCmd.Parameters.Add("@FromQty", SqlDbType.BigInt).Value = FromQTy
            objSQLCmd.Parameters.Add("@toQty", SqlDbType.BigInt).Value = ToQty
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

    Public Function CheckAssortmentItem(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal PlanID As String, ByVal ItemCode As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT    COUNT(B.Item_Code) AS Total FROM  TBL_BNS_Assortment_Plan AS A INNER JOIN TBL_BNS_Assortment_Items AS B ON A.Assortment_Plan_ID = B.Assortment_Plan_ID  WHERE A.Is_Active ='Y' AND B.Is_Get_Item='N' AND B.Item_Code =@ItemCode AND A.Assortment_Plan_ID <>@PlanID  AND A.Organization_ID =@OrgID ")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@PlanID", SqlDbType.VarChar, 100).Value = PlanID
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 100).Value = ItemCode
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
    Public Function GetItemUOM(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal OrgID As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim UOM As String = Nothing

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select Primary_UOM_Code FROM TBL_Product WHERE Item_Code=@ItemCode AND Organization_ID=@OrgID ")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 122).Value = ItemCode
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID

            UOM = Convert.ToString(objSQLCmd.ExecuteScalar())
           
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24066"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return UOM
    End Function
    Public Function CheckItemCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal OrgID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select COUNT(*) FROM TBL_Product WHERE Item_Code=@ItemCode AND Organization_ID=@OrgID ")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 122).Value = ItemCode
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

    Public Function CheckCustBonusFlag(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select COUNT(*) FROM TBL_App_Control WHERE Control_Key='ENABLE_CUST_BONUS' AND Control_Value='Y' ")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            Dim cnt As Integer = 0
            cnt = Convert.ToInt32(objSQLCmd.ExecuteScalar())
            If cnt > 0 Then
                success = True
            End If
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24266"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
    Public Function CheckOrgID(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select COUNT(*) FROM TBL_Product WHERE Organization_ID=@OrgID  ")
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
    Public Function CheckSimpleBonusPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal PlanName As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select COUNT(*) FROM TBL_BNS_Plan WHERE Description=@PlanName and Organization_ID=@OrgID  ")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@PlanName", SqlDbType.VarChar, 100).Value = PlanName
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
    Public Function CheckBonusPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal PlanName As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select COUNT(*) FROM TBL_BNS_Assortment_Plan WHERE Is_Active='Y' AND Description=@PlanName and Organization_ID=@OrgID  ")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@PlanName", SqlDbType.VarChar, 100).Value = PlanName
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
    Public Function GetOrganisation(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QueryStr As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            'Dim QueryString As String = String.Format("SELECT DISTINCT MAS_Org_ID, Site FROM TBL_Org_CTL_DTL WHERE SalesRep_ID IN ({0}) ORDER BY MAS_Org_ID DESC", QueryStr)
            Dim QueryString As String = String.Format("SELECT DISTINCT MAS_Org_ID,Description FROM TBL_Org_CTL_DTL WHERE SalesRep_ID IN ({0}) ORDER BY MAS_Org_ID DESC", QueryStr)
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
    Public Function GetDefaultProduct(ByVal Id As String, ByVal VanId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_GetDefaultProductList"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@Org_ID", SqlDbType.VarChar, 20).Value = Id
            objSQLCmd.Parameters.Add("@VanId", SqlDbType.Int).Value = VanId
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")

            GetDefaultProduct = MsgDs.Tables(0)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74092"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
 Public Function GetDiscountFOCItem(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal ItemID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = Nothing

            QueryString = "Select * from TBL_Product_Addl_Info where Inventory_Item_ID=@ItemID and Organization_ID=@OrgID and Attrib_Name='DISC_FOC'"

            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            objSQLCmd.Parameters.Add("@ItemID", SqlDbType.VarChar, 100).Value = ItemID
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "14164"
            Err_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function LoadDiscountData(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = Nothing

            QueryString = "app_GetDiscountData"
            


            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID

            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "14164"
            Err_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function

    Public Function ExportBonusData(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal BnsPlanID As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = Nothing
            

            QueryString = "SELECT B.Organization_ID As OrgID,B.Item_Code AS OrderItem,B.Get_Item AS BonusItem,Prom_Qty_From AS FromQty,Prom_Qty_To As ToQty,Price_Break_Type_Code AS Type,Get_Qty AS GetQty,Valid_From AS ValidFrom ,Valid_To As ValidTo  FROM TBL_BNS_Plan AS A INNER JOIN TBL_BNS_Promotion AS B On A.BNS_Plan_ID =B.BNS_Plan_ID WHERE A.Organization_ID =@OrgID AND A.BNS_Plan_ID =@BnsPlanID AND B.Is_Active ='Y' "


            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            objSQLCmd.Parameters.Add("@BnsPlanID", SqlDbType.Int).Value = BnsPlanID

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
    Public Function LoadBonusData(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal OrgID As String, ByVal ShowInActive As String, ByVal BnsPlanID As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = Nothing
            If ShowInActive = "Y" And ItemCode <> "" Then
                QueryString = "SELECT     A.BNS_Promotion_ID AS LineID, A.Item_Code + '-'+ (SELECT Description FROM TBL_Product WHERE Item_Code =A.item_Code AND A.Organization_ID=Organization_ID)  AS ItemCode,(SELECT Inventory_Item_Id FROM TBL_Product WHERE Item_Code=A.Item_Code AND A.Organization_ID=Organization_ID) AS DItemId,(SELECT Inventory_Item_Id FROM TBL_Product WHERE Item_Code=A.Get_Item AND  A.Organization_ID=Organization_ID ) AS BItemId, A.Organization_ID AS OrgId,Item_UOM AS DUOM, Prom_Qty_From AS FromQty, Prom_Qty_To As ToQty, Price_Break_Type_Code AS TypeCode, Get_Item  + '-'+ (SELECT Description FROM TBL_Product WHERE Item_Code =A.Get_item AND A.Organization_ID=Organization_ID)  As BItemCode, Get_UOM AS BUOM, Get_Qty AS GetQty, ISNULL(Get_Add_Per,0)*100 AS Get_Add_Per,A.Item_Code AS ACode,A.get_Item AS BCode,(SELECT Description FROM TBL_Product WHERE Item_Code =A.item_Code AND A.Organization_ID=Organization_ID)  AS ADesc,(SELECT Description FROM TBL_Product WHERE Item_Code =A.Get_Item AND A.Organization_ID=Organization_ID)  AS BDesc,A.Valid_From,A.Valid_To,CASE WHEN A.Is_Active='N' THEN 'Activate' ELSE 'Deactivate' END AS IsActive,CASE WHEN A.Is_Active='N' THEN '#FF3300' ELSE '#006600' END AS IsColor,CASE WHEN Is_Active='Y' THEN CAST('1' AS BIT) ELSE CAST('0' AS BIT) END AS IsVisible FROM TBL_BNS_Promotion AS  A WHERE  A.Item_Code=@ItemCode AND A.Organization_ID=@OrgID AND Bns_Plan_Id=@BnsPlanID Order By    A.item_Code,Prom_Qty_From"
            ElseIf ShowInActive = "Y" And ItemCode = "" Then
                QueryString = "SELECT     A.BNS_Promotion_ID AS LineID, A.Item_Code + '-'+ (SELECT Description FROM TBL_Product WHERE Item_Code =A.item_Code AND A.Organization_ID=Organization_ID)  AS ItemCode,(SELECT Inventory_Item_Id FROM TBL_Product WHERE Item_Code=A.Item_Code AND A.Organization_ID=Organization_ID) AS DItemId,(SELECT Inventory_Item_Id FROM TBL_Product WHERE Item_Code=A.Get_Item AND  A.Organization_ID=Organization_ID ) AS BItemId, A.Organization_ID AS OrgId,Item_UOM AS DUOM, Prom_Qty_From AS FromQty, Prom_Qty_To As ToQty, Price_Break_Type_Code AS TypeCode, Get_Item  + '-'+ (SELECT Description FROM TBL_Product WHERE Item_Code =A.Get_item AND A.Organization_ID=Organization_ID)  As BItemCode, Get_UOM AS BUOM, Get_Qty AS GetQty, ISNULL(Get_Add_Per,0)*100 AS Get_Add_Per,A.Item_Code AS ACode,A.get_Item AS BCode,(SELECT Description FROM TBL_Product WHERE Item_Code =A.item_Code AND A.Organization_ID=Organization_ID)  AS ADesc,(SELECT Description FROM TBL_Product WHERE Item_Code =A.Get_Item AND A.Organization_ID=Organization_ID)  AS BDesc,A.Valid_From,A.Valid_To,CASE WHEN A.Is_Active='N' THEN 'Activate' ELSE 'Deactivate' END AS IsActive,CASE WHEN A.Is_Active='N' THEN '#FF3300' ELSE '#006600' END AS IsColor,CASE WHEN Is_Active='Y' THEN CAST('1' AS BIT) ELSE CAST('0' AS BIT) END AS IsVisible FROM TBL_BNS_Promotion AS  A WHERE  A.Organization_ID=@OrgID AND Bns_Plan_Id=@BnsPlanID Order By    A.item_Code,Prom_Qty_From"

            ElseIf ShowInActive <> "Y" And ItemCode <> "" Then
                QueryString = "SELECT     A.BNS_Promotion_ID AS LineID, A.Item_Code + '-'+ (SELECT Description FROM TBL_Product WHERE Item_Code =A.item_Code AND A.Organization_ID=Organization_ID)  AS ItemCode,(SELECT Inventory_Item_Id FROM TBL_Product WHERE Item_Code=A.Item_Code AND A.Organization_ID=Organization_ID) AS DItemId,(SELECT Inventory_Item_Id FROM TBL_Product WHERE Item_Code=A.Get_Item AND  A.Organization_ID=Organization_ID ) AS BItemId, A.Organization_ID AS OrgId,Item_UOM AS DUOM, Prom_Qty_From AS FromQty, Prom_Qty_To As ToQty, Price_Break_Type_Code AS TypeCode, Get_Item  + '-'+ (SELECT Description FROM TBL_Product WHERE Item_Code =A.Get_item AND A.Organization_ID=Organization_ID)  As BItemCode, Get_UOM AS BUOM, Get_Qty AS GetQty, ISNULL(Get_Add_Per,0)*100 AS Get_Add_Per,A.Item_Code AS ACode,A.get_Item AS BCode,(SELECT Description FROM TBL_Product WHERE Item_Code =A.item_Code AND A.Organization_ID=Organization_ID)  AS ADesc,(SELECT Description FROM TBL_Product WHERE Item_Code =A.Get_Item AND A.Organization_ID=Organization_ID)  AS BDesc,A.Valid_From,A.Valid_To,CASE WHEN A.Is_Active='N' THEN 'Activate' ELSE 'Deactivate' END AS IsActive,CASE WHEN A.Is_Active='N' THEN '#FF3300' ELSE '#006600' END AS IsColor,CASE WHEN Is_Active='Y' THEN CAST('1' AS BIT) ELSE CAST('0' AS BIT) END AS IsVisible FROM TBL_BNS_Promotion AS  A WHERE  A.Item_Code=@ItemCode AND A.Organization_ID=@OrgID AND Is_Active='Y' AND Bns_Plan_Id=@BnsPlanID Order By    A.item_Code,Prom_Qty_From"

            ElseIf ShowInActive <> "Y" And ItemCode = "" Then
                QueryString = "SELECT     A.BNS_Promotion_ID AS LineID, A.Item_Code + '-'+ (SELECT Description FROM TBL_Product WHERE Item_Code =A.item_Code AND A.Organization_ID=Organization_ID)  AS ItemCode,(SELECT Inventory_Item_Id FROM TBL_Product WHERE Item_Code=A.Item_Code AND A.Organization_ID=Organization_ID) AS DItemId,(SELECT Inventory_Item_Id FROM TBL_Product WHERE Item_Code=A.Get_Item AND  A.Organization_ID=Organization_ID ) AS BItemId, A.Organization_ID AS OrgId,Item_UOM AS DUOM, Prom_Qty_From AS FromQty, Prom_Qty_To As ToQty, Price_Break_Type_Code AS TypeCode, Get_Item  + '-'+ (SELECT Description FROM TBL_Product WHERE Item_Code =A.Get_item AND A.Organization_ID=Organization_ID)  As BItemCode, Get_UOM AS BUOM, Get_Qty AS GetQty, ISNULL(Get_Add_Per,0)*100 AS Get_Add_Per,A.Item_Code AS ACode,A.get_Item AS BCode,(SELECT Description FROM TBL_Product WHERE Item_Code =A.item_Code AND A.Organization_ID=Organization_ID)  AS ADesc,(SELECT Description FROM TBL_Product WHERE Item_Code =A.Get_Item AND A.Organization_ID=Organization_ID)  AS BDesc,A.Valid_From,A.Valid_To,CASE WHEN A.Is_Active='N' THEN 'Activate' ELSE 'Deactivate' END AS IsActive,CASE WHEN A.Is_Active='N' THEN '#FF3300' ELSE '#006600' END AS IsColor,CASE WHEN Is_Active='Y' THEN CAST('1' AS BIT) ELSE CAST('0' AS BIT) END AS IsVisible FROM TBL_BNS_Promotion AS  A WHERE   A.Organization_ID=@OrgID AND Is_Active='Y' AND Bns_Plan_Id=@BnsPlanID Order By    A.item_Code,Prom_Qty_From"

            End If
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 100).Value = ItemCode
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            objSQLCmd.Parameters.Add("@BnsPlanID", SqlDbType.Int).Value = BnsPlanID

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
    Public Function UpdateBonusStatus(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal LineId As String, ByVal IsValid As String, ByVal UpDatedBy As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Update TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "UPDATE TBL_BNS_Promotion SET Is_Active=@IsValid,Sync_Timestamp =GETDATE(),Last_Updated_At=GetDATE(),Last_Updated_By=@UpdatedBy WHERE BNS_Promotion_ID IN (SELECT [Value]  FROM dbo.Split1(0,@LineID,',')  WHERE Value <> '' AND VALUE IS NOT NULL )"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@LineId", LineId)
            objSQLCmd.Parameters.AddWithValue("@IsValid", IsValid)
            objSQLCmd.Parameters.AddWithValue("@UpdatedBy", UpDatedBy)
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
    Public Function UpdateBonusData(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal LineId As String, ByVal BItemCode As String, ByVal BUOM As String, ByVal TypeCode As String, ByVal FromQty As Long, ByVal ToQty As Long, ByVal GetQty As Long, ByVal GetPer As Decimal, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, ByVal UpDatedBy As Integer, ByVal BnsPlanID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Update TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "UPDATE TBL_BNS_Promotion SET Prom_Qty_From=@FromQty ,Prom_Qty_To =@ToQty,Price_Break_Type_Code=@TypeCode ,Get_Item=@BitemCode ,Get_UOM=@BUOM,Get_Qty=@GetQty,Get_Add_Per=@GetPer ,Sync_Timestamp =GETDATE(),Valid_From=@ValidFrom,Valid_To=@ValidTo,Last_Updated_At=GetDATE(),Last_Updated_By=@UpdatedBy WHERE BNS_Promotion_ID=@LineID AND Bns_Plan_ID=@BnsPlanID"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@LineId", LineId)
            objSQLCmd.Parameters.AddWithValue("@TypeCode", TypeCode.ToUpper())
            objSQLCmd.Parameters.AddWithValue("@FromQty", FromQty)
            objSQLCmd.Parameters.AddWithValue("@ToQty", ToQty)
            objSQLCmd.Parameters.AddWithValue("@BItemCode", BItemCode)
            objSQLCmd.Parameters.AddWithValue("@BUOM", BUOM)
            objSQLCmd.Parameters.AddWithValue("@GetQty", GetQty)
            objSQLCmd.Parameters.AddWithValue("@GetPer", GetPer / 100)
            objSQLCmd.Parameters.AddWithValue("@ValidFrom", ValidFrom)
            objSQLCmd.Parameters.AddWithValue("@ValidTo", ValidTo)
            objSQLCmd.Parameters.AddWithValue("@UpdatedBy", UpDatedBy)
            objSQLCmd.Parameters.AddWithValue("@BnsPlanID", BnsPlanID)
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
    Public Function GetSelectedProduct(ByVal Id As String, ByVal VanId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_GetSelectedProductList"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@Org_ID", SqlDbType.VarChar, 20).Value = Id
            objSQLCmd.Parameters.Add("@VanId", SqlDbType.Int).Value = VanId
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")

            GetSelectedProduct = MsgDs.Tables(0)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74093"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function


    Public Function InsertMSL(ByVal OrgId As String, ByVal ItemId As Integer, ByVal ItemCode As String, ByVal VanId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_InsertMSL"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@Inv_ItemID", SqlDbType.Int).Value = ItemId
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 50).Value = ItemCode
            objSQLCmd.Parameters.Add("@OrganizationID", SqlDbType.VarChar, 20).Value = OrgId
            objSQLCmd.Parameters.Add("@VanId", SqlDbType.Int).Value = VanId
            Dim dtMSL As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dtMSL)
            success = True
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74093"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function

    Public Function DeleteMSL(ByVal OrgId As String, ByVal ItemId As Integer, ByVal ItemCode As String, ByVal VanId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_DeleteMSL"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@Inv_ItemID", SqlDbType.Int).Value = ItemId
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 50).Value = ItemCode
            objSQLCmd.Parameters.Add("@OrganizationID", SqlDbType.VarChar, 20).Value = OrgId
            objSQLCmd.Parameters.Add("@VanID", SqlDbType.Int).Value = VanId
            Dim dtMSL As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dtMSL)
            success = True
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74093"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function

    Public Function CheckProductExist(ByVal TableName As String, ByVal ItemCode As String, ByVal OrgId As String, ByVal VanId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim success As Boolean = False
        Dim objSQLConn As SqlConnection
        Dim objSQLCMD As SqlCommand
        Dim Rcnt As Integer = 0
        Dim sql As String = ""
        If TableName = "tblProduct" Then
            sql = "SELECT COUNT(*) FROM TBL_Product WHERE Item_Code =@ItemCode AND Organization_Id=@OrgID"
        ElseIf TableName = "tblMSL" Then
            sql = "SELECT COUNT(*) FROM TBL_Product_MSL WHERE Inventory_Item_Id IN (SELECT Inventory_Item_Id FROM tbl_product WHERE Item_Code=@ItemCode AND Organization_Id=@OrgID)AND Organization_Id=@OrgID AND SalesRep_Id=@VanId"
        End If
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCMD = New SqlCommand(sql, objSQLConn)
            objSQLCMD.CommandType = CommandType.Text
            objSQLCMD.Parameters.Add("@ItemCode", SqlDbType.VarChar, 50).Value = ItemCode
            objSQLCMD.Parameters.Add("@OrgID", SqlDbType.VarChar, 20).Value = OrgId
            objSQLCMD.Parameters.Add("@VanID", SqlDbType.Int).Value = VanId
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

    Public Function DeleteAll(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal VanId As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sRetVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("DELETE FROM TBL_Product_MSL WHERE SalesRep_Id=@VanId", objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@VanID", SqlDbType.Int).Value = VanId
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


    Public Function GetSalesRepId(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRepNo As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT A.SalesRep_ID,B.Mas_Org_Id from TBL_FSR AS A INNER JOIN  TBL_Org_CTL_DTL AS B ON A.SalesRep_ID=B.SalesRep_ID Where A.SalesRep_Number=@SalesRepNo")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@SalesRepNo", SqlDbType.VarChar, 100).Value = SalesRepNo
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")
            GetSalesRepId = MsgDs.Tables(0)
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "740234"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    
End Class
