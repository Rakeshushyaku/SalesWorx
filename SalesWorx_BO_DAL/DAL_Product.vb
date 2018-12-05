Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Data
Imports log4net
Public Class DAL_Product
    Private _objDB As DatabaseConnection
    Private dtItemUOM As New DataTable
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Function CheckValidFSRID(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRepNo As String, ByVal OrgID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select COUNT(*) FROM TBL_Org_CTL_DTL AS A INNER JOIN TBL_FSR AS B ON A.SalesRep_ID=B.SalesRep_ID WHERE B.Salesrep_Number=@SalesRepNo AND Mas_Org_ID=@OrgID ")
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
    Public Function GetBonusProductDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal Description As String, ByVal OrgId As String) As DataRow
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT Inventory_item_Id as ItemId, A.Item_Code As ItemCode,A.Description  AS Description, Primary_UOM_Code AS UOM ,Organization_Id as OrgId FROM TBL_Product AS A WHERE (A.organization_ID=@OrgID AND A.Item_Code =@ItemCOde) OR (A.organization_ID=@OrgID and A.Description =@Description)")
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

            Dim QueryString As String = String.Format("Select distinct Case When a.Row_ID is null Then 'No' Else 'Yes' End IsMSL,b.Item_No,b.[Description],b.Item_Code,b.Brand_Code,b.Item_Size,b.EANNO,b.Primary_UOM_Code,b.Promo_Item,b.Inventory_Item_ID,ISNULL(b.Cost_Price,0)AS CostPrice From TBL_Product_MSL as a right outer join dbo.TBL_Product as b on a.Inventory_Item_ID = b.Inventory_Item_ID and a.Organization_ID=b.Organization_ID Where 1=1 {0} order by b.Brand_Code Asc, b.[Item_Code] ASC", _sSearchParams, QueryStr)
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
    Public Function UploadDiscount(ByVal dtData As DataTable, ByVal OrgID As String, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PlanID As String) As Boolean
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
                    objSQLCmd.Parameters.AddWithValue("@PlanID", PlanID)
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
    Public Function CheckDiscountDataRange(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal FromQty As Long, ByVal OrgID As String, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, ByVal LineID As Integer, ByVal PlanID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False


        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = Nothing
            If LineID = 0 Then
                QueryString = "SELECT    COUNT(*)  FROM   TBL_Discount AS A  WHERE  isnull(A.Discount_Plan_ID,0)=@PlanID and A.Item_Code=@ItemCode AND A.Organization_ID=@OrgID AND  From_Qty=@FromQty AND  (( @ValidFrom BETWEEN Valid_From AND Valid_To  ) or (Valid_To BETWEEN @ValidFrom AND @ValidTo))"
            Else
                QueryString = "SELECT    COUNT(*)  FROM   TBL_Discount AS A  WHERE  isnull(A.Discount_Plan_ID,0)=@PlanID and A.Item_Code=@ItemCode AND A.Organization_ID=@OrgID AND  From_Qty=@FromQty AND  (( @ValidFrom BETWEEN Valid_From AND Valid_To  ) or (Valid_To BETWEEN @ValidFrom AND @ValidTo)) AND Discount_ID<>@LineID"
            End If
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@LineID", SqlDbType.Int).Value = LineID
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 100).Value = ItemCode
            objSQLCmd.Parameters.Add("@FromQty", SqlDbType.Int).Value = FromQty
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            objSQLCmd.Parameters.Add("@ValidFrom", SqlDbType.DateTime).Value = ValidFrom
            objSQLCmd.Parameters.Add("@ValidTo", SqlDbType.DateTime).Value = ValidTo
            objSQLCmd.Parameters.Add("@PlanID", SqlDbType.Int).Value = PlanID
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

    Public Function CheckBonusInvValueActiveRange(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal MinInvValue As Decimal, ByVal MinItem As Integer, ByVal OrgID As String, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, ByVal LineID As Integer, BnsPlanID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim i As Integer = 0
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = Nothing
            If LineID = 0 Then
                QueryString = "SELECT COUNT(*) FROM         TBL_BNS_BIV AS A WHERE A.Bns_Plan_ID=@PlanID AND  A.Min_Invoice_Value=@MinInvValue and A.Min_Ordered_Items=@MinItem AND  Is_Active='Y'    AND  (( Valid_From BETWEEN   @ValidFrom and @ValidTo or(@ValidFrom BETWEEN Valid_From AND Valid_To  ))  or (Valid_To BETWEEN @ValidFrom AND @ValidTo or(@ValidTo BETWEEN Valid_From AND Valid_To  )))"
            Else
                QueryString = "SELECT COUNT(*) FROM         TBL_BNS_BIV AS A WHERE A.Rule_ID<>@RuleID AND A.Bns_Plan_ID=@PlanID AND  A.Min_Invoice_Value=@MinInvValue and A.Min_Ordered_Items=@MinItem AND  Is_Active='Y'    AND  (( Valid_From BETWEEN   @ValidFrom and @ValidTo or(@ValidFrom BETWEEN Valid_From AND Valid_To  ))  or (Valid_To BETWEEN @ValidFrom AND @ValidTo or(@ValidTo BETWEEN Valid_From AND Valid_To  )))"
            End If
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@RuleID", SqlDbType.Int).Value = LineID
            objSQLCmd.Parameters.Add("@MinInvValue", SqlDbType.Decimal).Value = MinInvValue
            objSQLCmd.Parameters.Add("@MinItem", SqlDbType.Int).Value = MinItem
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            objSQLCmd.Parameters.Add("@ValidFrom", SqlDbType.DateTime).Value = ValidFrom
            objSQLCmd.Parameters.Add("@ValidTo", SqlDbType.DateTime).Value = ValidTo
            objSQLCmd.Parameters.Add("@PlanID", SqlDbType.Int).Value = BnsPlanID
            i = objSQLCmd.ExecuteScalar()
            If i > 0 Then
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
    Public Function CheckAssortmentBonusDataValidity(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, BnsPlanID As Integer) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataSet

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = Nothing
            QueryString = "app_ValidateAssortmentBonusItem"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 100).Value = ItemCode
            objSQLCmd.Parameters.Add("@PlanID", SqlDbType.Int).Value = BnsPlanID
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

    Public Function CheckBonusDataValidity(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal OrgID As String, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, BnsPlanID As Integer) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataSet

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = Nothing
            QueryString = "app_ValidateSimpleBonusItem"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 100).Value = ItemCode
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            objSQLCmd.Parameters.Add("@ValidFrom", SqlDbType.DateTime).Value = ValidFrom
            objSQLCmd.Parameters.Add("@ValidTo", SqlDbType.DateTime).Value = ValidTo
            objSQLCmd.Parameters.Add("@PlanID", SqlDbType.Int).Value = BnsPlanID
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

    Public Function CheckBonusDataActiveRange(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal FromQty As Long, ByVal ToQty As Long, ByVal OrgID As String, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, ByVal LineID As Integer, OUOM As String, BnsPlanID As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = Nothing
            If LineID = 0 Then
                QueryString = "SELECT     A.BNS_Promotion_ID AS ActiveLineID, A.Item_Code +'-'+ B.Description AS ItemName, A.Valid_From, A.Valid_To, A.Prom_Qty_From, A.Prom_Qty_To, A.Get_Qty ,ISNULL(A.Max_FOC_Qty,0)AS MaxQty FROM         TBL_BNS_Promotion AS A INNER JOIN                     TBL_Product AS B ON A.Organization_ID = B.Organization_ID AND A.Item_Code = B.Item_Code  WHERE A.Bns_Plan_ID=@PlanID AND A.Item_UOM=@OUOM AND  A.Item_Code=@ItemCode and A.Organization_ID=@OrgID AND  ( (Prom_Qty_From BETWEEN  @FromQty and @ToQty or (@FromQty BETWEEN Prom_Qty_From AND Prom_Qty_To )) AND  ( Valid_From BETWEEN   @ValidFrom and @ValidTo or(@ValidFrom BETWEEN Valid_From AND Valid_To  ))  AND A.Is_Active='Y'  or ((Prom_Qty_To BETWEEN   @FromQty  and @ToQty or (@ToQty BETWEEN   Prom_Qty_From  and Prom_Qty_To))  AND     (Valid_To BETWEEN @ValidFrom AND @ValidTo or(@ValidTo BETWEEN Valid_From AND Valid_To  ))  AND A.Is_Active='Y') )"
            Else
                QueryString = "SELECT     A.BNS_Promotion_ID AS ActiveLineID, A.Item_Code +'-'+ B.Description AS ItemName, A.Valid_From, A.Valid_To, A.Prom_Qty_From, A.Prom_Qty_To, A.Get_Qty,ISNULL(A.Max_FOC_Qty,0)AS MaxQty  FROM         TBL_BNS_Promotion AS A INNER JOIN                     TBL_Product AS B ON A.Organization_ID = B.Organization_ID AND A.Item_Code = B.Item_Code  WHERE A.Bns_Plan_ID=@PlanID AND A.Item_UOM=@OUOM AND  A.Bns_Promotion_ID<>@LineID AND  A.Item_Code=@ItemCode and A.Organization_ID=@OrgID AND  ( (Prom_Qty_From BETWEEN  @FromQty and @ToQty or (@FromQty BETWEEN Prom_Qty_From AND Prom_Qty_To )) AND  ( Valid_From BETWEEN   @ValidFrom and @ValidTo or(@ValidFrom BETWEEN Valid_From AND Valid_To  ))  AND A.Is_Active='Y'  or ((Prom_Qty_To BETWEEN   @FromQty  and @ToQty or (@ToQty BETWEEN   Prom_Qty_From  and Prom_Qty_To))  AND     (Valid_To BETWEEN @ValidFrom AND @ValidTo or(@ValidTo BETWEEN Valid_From AND Valid_To  ))  AND A.Is_Active='Y') )"
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
            objSQLCmd.Parameters.Add("@OUOM", SqlDbType.VarChar, 100).Value = OUOM
            objSQLCmd.Parameters.Add("@PlanID", SqlDbType.Int).Value = BnsPlanID
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
    Public Function CheckIfBonusDateCanbeChanged(ByRef Err_No As Long, ByRef Err_Desc As String, PlanID As String, ByVal ItemCode As String, ByVal OrgID As String, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = Nothing

            QueryString = "app_CheckIfBonusDateCanbeChanged"

            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@PlanID", SqlDbType.Int).Value = PlanID
            objSQLCmd.Parameters.Add("@itemCode", SqlDbType.VarChar, 100).Value = ItemCode
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            objSQLCmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = ValidFrom
            objSQLCmd.Parameters.Add("@Todate", SqlDbType.DateTime).Value = ValidTo
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
    Public Function SaveBonusData(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal ItemCode As String, ByVal OrgId As String, ByVal DUOm As String, ByVal BItemCode As String, ByVal BUOM As String, ByVal TypeCode As String, ByVal FromQty As Long, ByVal ToQty As Long, ByVal GetQty As Long, ByVal Getper As Decimal, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, ByVal CreatedBy As Integer, ByVal BnsPlanID As Integer, MaxQty As Long) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "INSERT INTO TBL_BNS_Promotion (Item_Code ,Organization_ID ,Item_UOM,Valid_From,Valid_To,Prom_Qty_From ,Prom_Qty_To ,Price_Break_Type_Code ,Get_Item ,Get_UOM,Get_Qty ,Get_Add_Per ,Sync_Timestamp,Created_By,Created_At,Bns_Plan_ID,Max_FOC_Qty)VALUES(@ItemCode,@OrgId,@DUOm,@ValidFrom,@ValidTo,@FromQty,@ToQty,@TypeCode,@BItemCode,@BUOM,@GetQty,@getPer,GetDate(),@CreatedBy,GetDate(),@BnsPlanID,@MaxQty)"
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
            objSQLCmd.Parameters.AddWithValue("@MaxQty", MaxQty)
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

    Public Function SaveInvoiceRule(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal InvValue As Decimal, MinItem As Integer, ByVal OrgId As String, ByVal FocItem As String, ByVal GetQty As Long, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, ByVal CreatedBy As Integer, ByVal BnsPlanID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "app_SaveInvoiceValueRule"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@PlanID", BnsPlanID)
            objSQLCmd.Parameters.AddWithValue("@MinInvValue", InvValue)
            objSQLCmd.Parameters.AddWithValue("@MinItem", MinItem)
            objSQLCmd.Parameters.AddWithValue("@FOCItem", FocItem)
            objSQLCmd.Parameters.AddWithValue("@FOCQty", GetQty)

            objSQLCmd.Parameters.AddWithValue("@ValidFrom", ValidFrom)
            objSQLCmd.Parameters.AddWithValue("@ValidTo", ValidTo)

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

    Public Function SavePriceData(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal ItemID As String, ByVal OrgId As String, ByVal DUOm As String, ByVal UnitPrice As Decimal, ByVal PriceListID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "app_SavePriceData"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@ItemID", ItemID)
            objSQLCmd.Parameters.AddWithValue("@OrgId", OrgId)
            objSQLCmd.Parameters.AddWithValue("@DUOM", DUOm)
            'objSQLCmd.Parameters.AddWithValue("@ValidFrom", Now.Date.ToString("yyyy-MM-dddd"))
            'objSQLCmd.Parameters.AddWithValue("@ValidTo", "9999-12-31 00:00:00.000")
            objSQLCmd.Parameters.AddWithValue("@UnitPrice", UnitPrice)
            objSQLCmd.Parameters.AddWithValue("@PriceListID", PriceListID)
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

    Public Function SaveDiscountData(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal ItemCode As String, ByVal OrgId As String, ByVal TypeCode As String, ByVal FromQty As Long, ByVal Rate As Decimal, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, ByVal PlanID As String) As Boolean
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
            objSQLCmd.Parameters.AddWithValue("@PlanID", PlanID)
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

    Public Function UpdateDiscountData(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal LineID As Integer, ByVal ItemCode As String, ByVal OrgId As String, ByVal TypeCode As String, ByVal FromQty As Long, ByVal Rate As Decimal, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, ByVal PlanID As String) As Boolean
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
            objSQLCmd.Parameters.AddWithValue("@PlanID", PlanID)
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


    Public Function UploadCostPrice(ByVal dtData As DataTable, ByVal OrgID As String, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
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



                    QueryString = "UPDATE TBL_Product SET Cost_Price=@CostPrice,sync_timestamp=GETDATE() WHERE Organization_ID=@OrgID AND Item_Code=@ItemCode"
                    objSQLCmd = New SqlCommand(QueryString, objSQLConn)
                    objSQLCmd.CommandType = CommandType.Text


                    objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 20).Value = OrgID
                    objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 50).Value = r("ItemCode").ToString()
                    objSQLCmd.Parameters.AddWithValue("@CostPrice", CDec(IIf(r("CostPrice") Is DBNull.Value Or r("Costprice").ToString() = "", "0", r("CostPrice").ToString())))
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

    Public Function UpdateCostPrice(ByVal ItemID As Integer, CostPrice As Decimal, ByVal OrgID As String, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Dim QueryString As String = Nothing
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection



            QueryString = "UPDATE TBL_Product SET Cost_Price=@CostPrice,sync_timestamp=GETDATE() WHERE Organization_ID=@OrgID AND Inventory_item_ID=@ItemID"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text


            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 20).Value = OrgID
            objSQLCmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = ItemID
            objSQLCmd.Parameters.AddWithValue("@CostPrice", CostPrice)

            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()

            success = True

        Catch ex As Exception
            Err_No = "75721"
            Err_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function

    Public Function UpdateMultiTransRule(RuleID As String, RedQty As String, GivenQty As String, IsActive As String, UpdatedBy As Integer, ByRef Err_No As Long, ByRef Err_Desc As String, ValidFrom As Date, ValidTo As Date, Transaction_Type As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Dim QueryString As String = Nothing
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection



            QueryString = "UPDATE TBL_BNS_Multi_Trx SET Last_Updated_At =GETDATE(),Last_Updated_By =@UpdatedBy ,Sales_Qty =@RedQty ,Promo_Qty =@GivenQty ,Is_Active =@IsActive,Promo_Start_Date=@ValidFrom,Promo_End_Date=@ValidTo,Transaction_Type=@Transaction_Type WHERE Rule_ID =@RuleID"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text


            objSQLCmd.Parameters.Add("@RuleID", SqlDbType.VarChar, 20).Value = RuleID
            objSQLCmd.Parameters.AddWithValue("@RedQty", RedQty)
            objSQLCmd.Parameters.AddWithValue("@GivenQty", GivenQty)
            objSQLCmd.Parameters.AddWithValue("@IsActive", IsActive)
            objSQLCmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy)
            objSQLCmd.Parameters.AddWithValue("@ValidFrom", ValidFrom)
            objSQLCmd.Parameters.AddWithValue("@ValidTo", ValidTo)
            objSQLCmd.Parameters.AddWithValue("@Transaction_Type", Transaction_Type)

            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()

            success = True

        Catch ex As Exception
            Err_No = "75721"
            Err_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
    Public Function UpdateRedemptionRule(RuleID As String, RedQty As String, GivenQty As String, IsActive As String, UpdatedBy As Integer, ByRef Err_No As Long, ByRef Err_Desc As String, ValidFrom As Date, ValidTo As Date) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Dim QueryString As String = Nothing
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection



            QueryString = "UPDATE TBL_Redemption_Rules SET Last_Updated_At =GETDATE(),Last_Updated_By =@UpdatedBy ,Return_Qty =@RedQty ,Sales_Qty =@GivenQty ,Is_Active =@IsActive,Valid_From=@ValidFrom,Valid_To=@ValidTo WHERE Rule_ID =@RuleID"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text


            objSQLCmd.Parameters.Add("@RuleID", SqlDbType.VarChar, 20).Value = RuleID
            objSQLCmd.Parameters.AddWithValue("@RedQty", RedQty)
            objSQLCmd.Parameters.AddWithValue("@GivenQty", GivenQty)
            objSQLCmd.Parameters.AddWithValue("@IsActive", IsActive)
            objSQLCmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy)
            objSQLCmd.Parameters.AddWithValue("@ValidFrom", ValidFrom)
            objSQLCmd.Parameters.AddWithValue("@ValidTo", ValidTo)

            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()

            success = True

        Catch ex As Exception
            Err_No = "75721"
            Err_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
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

    Public Function DeleteInvoiceRule(ByRef Err_No As Long, ByRef Err_Desc As String, PlanID As Integer, ByVal LineId As String, DeletedBy As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("UPDATE TBL_BNS_BIV SET Is_Active='N' ,Last_Updated_By=@DeletedBY ,last_Updated_At=GETDATE() WHERE Rule_ID=@LineID AND Bns_Plan_ID=@PlanID")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@LineId", SqlDbType.VarChar, 100).Value = LineId
            objSQLCmd.Parameters.Add("@PlanId", SqlDbType.Int).Value = PlanID
            objSQLCmd.Parameters.Add("@DeletedBY", SqlDbType.Int).Value = DeletedBy
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
    Public Function DeletePriceData(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal LineId As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("DELETE FROM TBL_Price_List WHERE Price_List_Line_ID=@LineID")
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
    Public Function UpdateBonusPlan(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal PlanId As String, ByVal PlanName As String, ByVal CreatedBy As Integer, ByVal IsActive As String, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, TransactionType As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "UPDATE TBL_BNS_Assortment_Plan  SET Description=@Description ,Valid_From=@ValidFrom,Valid_To=@ValidTo,Last_Updated_At=GetDate(),Last_Updated_By=@CreatedBy,Transaction_Type=@Transaction_Type WHERE Assortment_Plan_ID=@PlanID"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@PlanID", PlanId)
            objSQLCmd.Parameters.AddWithValue("@Description", PlanName)
            objSQLCmd.Parameters.AddWithValue("@ValidFrom", ValidFrom)
            objSQLCmd.Parameters.AddWithValue("@ValidTo", ValidTo)
            'objSQLCmd.Parameters.AddWithValue("@IsActive", IsActive)
            objSQLCmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
            objSQLCmd.Parameters.AddWithValue("@Transaction_Type", TransactionType)


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
    Public Function SaveBonusPlan(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal OrgId As String, ByVal PlanName As String, ByVal CreatedBy As Integer, ByRef PlanId As String, ByVal IsActive As String, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, PlanType As String, TransactionType As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "INSERT INTO TBL_BNS_Assortment_Plan (Description ,Organization_ID ,Valid_From,Valid_To,Is_Active, Created_At,Created_By,Plan_Type,Transaction_Type)VALUES(@PlanName,@OrgId,@ValidFrom,@ValidTo,@isActive, GETDATE(),@CreatedBy,@PlanType,@Transaction_Type)"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@PlanName", PlanName)
            objSQLCmd.Parameters.AddWithValue("@OrgId", OrgId)
            objSQLCmd.Parameters.AddWithValue("@ValidFrom", ValidFrom)
            objSQLCmd.Parameters.AddWithValue("@ValidTo", ValidTo)
            objSQLCmd.Parameters.AddWithValue("@IsActive", IsActive)
            objSQLCmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
            objSQLCmd.Parameters.AddWithValue("@PlanType", PlanType)
            objSQLCmd.Parameters.AddWithValue("@Transaction_Type", TransactionType)

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

    Public Function UpdateSimpleBonusPlan(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal PlanId As String, ByVal PlanName As String, ByVal CreatedBy As Integer, TransactionType As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "UPDATE TBL_BNS_Plan  SET Description=@Description ,Last_Updated_At=GetDate(),Last_Updated_By=@CreatedBy,Transaction_Type=@Transaction_Type WHERE Bns_Plan_ID=@PlanID"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@PlanID", PlanId)
            objSQLCmd.Parameters.AddWithValue("@Description", PlanName)
            objSQLCmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
            objSQLCmd.Parameters.AddWithValue("@Transaction_Type", TransactionType)

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


    Public Function UpdatePriceList(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal PriceListId As String, ByVal PriceListName As String, ByVal Code As String, ByVal CreatedBy As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "UPDATE TBL_Price_List_H  SET Description=@Description ,Price_List_Code=@Code WHERE Price_List_ID=@PriceListID"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@PriceListID", PriceListId)
            objSQLCmd.Parameters.AddWithValue("@Description", PriceListName)
            objSQLCmd.Parameters.AddWithValue("@Code", Code)

            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()



            sucess = True
        Catch ex As Exception
            Error_No = 25014
            ' Error_Desc = String.Format("Error while saving Order", ex.Message)

        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return sucess

    End Function

    Public Function DeleteVATRule(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal OrgId As String, Vat_Rule_ID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "delete from tbl_VAT_Rule where VAT_Rule_ID=@VAT_Rule_ID and Organization_ID=@OrgID"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLCmd.Parameters.AddWithValue("@VAT_Rule_ID", Vat_Rule_ID)
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
    Public Function SaveMultiTransRule(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal OrgId As String, ByVal RedItem As String, ByRef RedQty As String, GivenItem As String, GivenQty As String, IsActive As String, ByVal CreatedBy As Integer, ValidFrom As Date, ValidTo As Date, ByRef RuleID As String, Transaction_Type As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "app_SaveMultiTransRules"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@OID", OrgId)
            objSQLCmd.Parameters.AddWithValue("@RedItem", RedItem)
            objSQLCmd.Parameters.AddWithValue("@Redqty", RedQty)
            objSQLCmd.Parameters.AddWithValue("@GivenItem", GivenItem)
            objSQLCmd.Parameters.AddWithValue("@GivenQty", GivenQty)
            objSQLCmd.Parameters.AddWithValue("@IsActive", IsActive)
            objSQLCmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
            objSQLCmd.Parameters.AddWithValue("@ValidFrom", ValidFrom)
            objSQLCmd.Parameters.AddWithValue("@ValidTo", ValidTo)
            objSQLCmd.Parameters.AddWithValue("@Transaction_Type", Transaction_Type)
            objSQLCmd.Parameters.Add("@RuleID", SqlDbType.Int)
            objSQLCmd.Parameters("@RuleID").Direction = ParameterDirection.Output
            objSQLCmd.ExecuteNonQuery()
            RuleID = objSQLCmd.Parameters("@RuleID").Value.ToString()
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
    Public Function SaveRedemptionRule(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal OrgId As String, ByVal RedItem As String, ByRef RedQty As String, GivenItem As String, GivenQty As String, IsActive As String, ByVal CreatedBy As Integer, ValidFrom As Date, ValidTo As Date) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "app_SaveRedemptionRules"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@OID", OrgId)
            objSQLCmd.Parameters.AddWithValue("@RedItem", RedItem)
            objSQLCmd.Parameters.AddWithValue("@Redqty", RedQty)
            objSQLCmd.Parameters.AddWithValue("@GivenItem", GivenItem)
            objSQLCmd.Parameters.AddWithValue("@GivenQty", GivenQty)
            objSQLCmd.Parameters.AddWithValue("@IsActive", IsActive)
            objSQLCmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
            objSQLCmd.Parameters.AddWithValue("@ValidFrom", ValidFrom)
            objSQLCmd.Parameters.AddWithValue("@ValidTo", ValidTo)

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
    Public Function SaveVATRule(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal OrgId As String, ByVal CustomerID As String, ByRef SiteUSeID As String, InventoryItemID As String, Vat_value As String, Vat_code As String, ByVal CreatedBy As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "app_SaveVATRule"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@OrgID", OrgId)
            objSQLCmd.Parameters.AddWithValue("@CustomerID", Val(CustomerID))
            objSQLCmd.Parameters.AddWithValue("@SiteUseID", Val(SiteUSeID))
            objSQLCmd.Parameters.AddWithValue("@Inventory_Item_ID", Val(InventoryItemID))
            objSQLCmd.Parameters.AddWithValue("@userID", CreatedBy)
            objSQLCmd.Parameters.AddWithValue("@Vat_value", Val(Vat_value))
            objSQLCmd.Parameters.AddWithValue("@Vat_Code", Vat_code)

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

    Public Function SaveAssortmentBonusCategoryMap(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal PlanID As String, ByVal DtCategory As DataTable, CreatedBy As String, PlanType As String, InsertMode As String, OrgID As String, TransactionType As String, ByRef dt As DataTable) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection
            sQry = "delete from TBL_BNS_Assortment_Category_Map where Assortment_Plan_ID=@Assortment_Plan_ID"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@Assortment_Plan_ID", PlanID)
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()

            For Each dr As DataRow In DtCategory.Rows
                objSQLConn = _objDB.GetSQLConnection
                Dim QueryString As String = String.Format("app_InsertAssortmentBonusCategoryMap")
                objSQLCmd = New SqlCommand(QueryString, objSQLConn)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.Parameters.AddWithValue("@BNS_Plan_ID", PlanID)
                objSQLCmd.Parameters.AddWithValue("@Category", dr(0).ToString)
                objSQLCmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
                objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
                objSQLCmd.Parameters.AddWithValue("@InsertMode", InsertMode)
                objSQLCmd.Parameters.AddWithValue("@PlanType", PlanType)
                objSQLCmd.Parameters.AddWithValue("@TransType", TransactionType)
                Dim MsgDs As New DataSet
                Dim SqlAd As SqlDataAdapter
                SqlAd = New SqlDataAdapter(objSQLCmd)
                SqlAd.Fill(MsgDs, "OrgTbl")
                If MsgDs.Tables(0).Rows.Count > 0 Then
                    For Each drError As DataRow In MsgDs.Tables(0).Rows
                        Dim newdr As DataRow
                        newdr = dt.NewRow
                        newdr("Category") = dr(0).ToString
                        newdr("Status") = "Not added"
                        newdr("Response") = "Overlapping item in Plan:" & dr("PlanName").ToString & "(" & dr("PlanType").ToString & "), Item Code : " & dr("Item_Code") & ", From Date : " & CDate(dr("FromDt1")).ToString("dd-MMM-yyyy") & ", To Date : " & CDate(dr("ToDt1")).ToString("dd-MMM-yyyy")
                    Next
                End If
                objSQLCmd.Dispose()

            Next

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

    Public Function SaveBonusCategoryMap(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal PlanID As String, ByVal DtCategory As DataTable, CreatedBy As String, PlanType As String, InsertMode As String, OrgID As String, TransactionType As String, ByRef dt As DataTable) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection
            sQry = "delete from TBL_BNS_Plan_Category_Map where BNS_Plan_ID=@BNS_Plan_ID"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@BNS_Plan_ID", PlanID)
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()


            For Each dr As DataRow In DtCategory.Rows

                objSQLConn = _objDB.GetSQLConnection
                Dim QueryString As String = String.Format("app_InsertSimpleBonusCategoryMap")
                objSQLCmd = New SqlCommand(QueryString, objSQLConn)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.Parameters.AddWithValue("@BNS_Plan_ID", PlanID)
                objSQLCmd.Parameters.AddWithValue("@Category", dr(0).ToString)
                objSQLCmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
                objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
                objSQLCmd.Parameters.AddWithValue("@InsertMode", InsertMode)
                objSQLCmd.Parameters.AddWithValue("@PlanType", PlanType)
                objSQLCmd.Parameters.AddWithValue("@TransType", TransactionType)
                Dim MsgDs As New DataSet
                Dim SqlAd As SqlDataAdapter
                SqlAd = New SqlDataAdapter(objSQLCmd)
                SqlAd.Fill(MsgDs, "OrgTbl")
                If MsgDs.Tables(0).Rows.Count > 0 Then
                    For Each drError As DataRow In MsgDs.Tables(0).Rows
                        Dim newdr As DataRow
                        newdr = dt.NewRow
                        newdr("Category") = dr(0).ToString
                        newdr("Status") = "Not added"
                        newdr("Response") = "Overlapping item in Plan:" & drError("PlanName").ToString & "(" & drError("PlanType").ToString & "), Item Code : " & drError("Item_Code") & ", From Date : " & CDate(drError("FromDt2")).ToString("dd-MMM-yyyy") & ", To Date : " & CDate(drError("ToDt2")).ToString("dd-MMM-yyyy")
                        dt.Rows.Add(newdr)
                    Next
                End If
                objSQLCmd.Dispose()


            Next

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


    Public Function SaveSimpleBonusPlan(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal OrgId As String, ByVal PlanName As String, ByVal CreatedBy As Integer, ByRef PlanId As String, PlanType As String, TransactionType As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "INSERT INTO TBL_BNS_Plan (Description ,Organization_ID , Created_At,Created_By,Plan_Type,Transaction_Type)VALUES(@PlanName,@OrgId, GETDATE(),@CreatedBy,@PlanType,@Transaction_Type)"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@PlanName", PlanName)
            objSQLCmd.Parameters.AddWithValue("@OrgId", OrgId)
            objSQLCmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
            objSQLCmd.Parameters.AddWithValue("@PlanType", PlanType)
            objSQLCmd.Parameters.AddWithValue("@Transaction_Type", TransactionType)
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

    Public Function SavePriceList(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal OrgId As String, ByVal PriceListName As String, ByVal CreatedBy As Integer, ByRef PriceListId As String, ByVal Code As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "INSERT INTO TBL_Price_List_H (Description ,Organization_ID,Price_List_Code )VALUES(@PriceListName,@OrgId,@Code)"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@PriceListName", PriceListName)
            objSQLCmd.Parameters.AddWithValue("@OrgId", OrgId)
            objSQLCmd.Parameters.AddWithValue("@Code", Code)

            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()

            objSQLCmd = New SqlCommand("select @@IDENTITY as PriceListID", objSQLConn)
            PriceListId = CStr(objSQLCmd.ExecuteScalar())
            objSQLCmd.Dispose()

            sucess = True
        Catch ex As Exception
            Error_No = 75014
            ' Error_Desc = String.Format("Error while saving Order", ex.Message)

        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return PriceListId

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
    Public Function LoadRedemptionProductList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, Optional text As String = "") As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT Item_Code AS CodeValue,Item_Code+'-'+Description AS CodeText FROM TBL_Product WHERE Organization_ID=@OrgID ")
            If text <> "" Then
                QueryString = QueryString & " and (Item_Code LIKE '%' + @ItemCode + '%' OR Item_Code +'-'+ Description LIKE '%' + @ItemCode + '%')"
            End If

            QueryString = QueryString & "  ORDER By Description"

            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            If text <> "" Then
                objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 100).Value = text
            End If

            objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            Dim MsgDs As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs)

            LoadRedemptionProductList = MsgDs
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "740243"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function LoadPriceProductList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, Optional text As String = "") As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT Inventory_Item_Id AS CodeValue,Item_Code+'-'+Description AS CodeText FROM TBL_Product WHERE Organization_ID=@OrgID ")
            If text <> "" Then
                QueryString = QueryString & " and (Item_Code LIKE '%' + @ItemCode + '%' OR Item_Code +'-'+ Description LIKE '%' + @ItemCode + '%')"
            End If

            QueryString = QueryString & "  ORDER By Description"

            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            If text <> "" Then
                objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 100).Value = text
            End If

            objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            Dim MsgDs As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs)

            LoadPriceProductList = MsgDs
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "740243"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function LoadBonusProductList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, Optional text As String = "") As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT Item_Code AS DescValue,Description AS DescText,Item_Code AS CodeValue,Item_Code AS CodeText FROM TBL_Product WHERE Organization_ID=@OrgID ")
            If text <> "" Then
                QueryString = QueryString & " and (Item_Code LIKE '%' + @ItemCode + '%' OR Item_Code +'-'+ Description LIKE '%' + @ItemCode + '%')"
            End If

            QueryString = QueryString & "  ORDER By Description"

            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            If text <> "" Then
                objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 100).Value = text
            End If

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
    Public Function LoadBonusProductListDesc(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, Optional text As String = "") As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT Item_Code AS DescValue,Description AS DescText,Item_Code AS CodeValue,Item_Code AS CodeText FROM TBL_Product WHERE Organization_ID=@OrgID ")
            If text <> "" Then
                QueryString = QueryString & " and ( Description LIKE '%' + @ItemCode + '%')"
            End If

            QueryString = QueryString & "  ORDER By Description"

            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            If text <> "" Then
                objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 100).Value = text
            End If

            objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            Dim MsgDs As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs)

            LoadBonusProductListDesc = MsgDs
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "740243"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function LoadProductList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, Optional text As String = "") As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT Item_Code + '$' + Primary_UOM_Code AS DescValue,Description AS DescText,Item_Code + '$' + Primary_UOM_Code AS CodeValue,Item_Code AS CodeText FROM TBL_Product WHERE Organization_ID=@OrgID  ")
            If text <> "" Then
                QueryString = QueryString & " and (Item_Code LIKE '%' + @ItemCode + '%' OR Item_Code +'-'+ Description LIKE '%' + @ItemCode + '%')"
            End If
            QueryString = QueryString & "  ORDER By Description"

            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            If text <> "" Then
                objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 100).Value = text
            End If
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
    Public Function DeletePriceList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PriceListID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("DELETE FROM TBL_Price_List WHERE Price_List_ID=@PriceListID;DELETE FROM TBL_Price_List_H WHERE Price_List_ID=@PriceListID;UPDATE TBl_Customer SET Price_List_ID=NULL ,Sync_Timestamp=GETDATE() WHERE Price_List_ID=@PriceListID")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@PriceListID", SqlDbType.VarChar, 100).Value = PriceListID
            objSQLCmd.ExecuteNonQuery()
            success = True
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "22064"
            Err_Desc = ex.Message

            success = False
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function

    Public Function DeleteRedemption(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RuleID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("DELETE FROM TBL_Redemption_Rules WHERE Rule_ID=@RuleID")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@RuleID", SqlDbType.VarChar, 100).Value = RuleID
            objSQLCmd.ExecuteNonQuery()
            success = True
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "22066"
            Err_Desc = ex.Message

            success = False
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function

    Public Function DeleteMultiTransRule(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RuleID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("DELETE FROM TBL_BNS_Multi_Trx WHERE Rule_ID=@RuleID")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@RuleID", SqlDbType.VarChar, 100).Value = RuleID
            objSQLCmd.ExecuteNonQuery()
            success = True
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "22066"
            Err_Desc = ex.Message

            success = False
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
    Public Function LoadPriceList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UID As Integer, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT DISTINCT A.Price_List_ID,A.Description,A.Organization_ID AS OrgID,A.Price_List_Code AS Price_List_Code,(SELECT Description FROM TBL_Org_CTL_H WHERE ORG_HE_ID =B.MAS_Org_ID) AS  OrgName,(SELECT COUNT(DISTINCT Inventory_Item_ID) FROM TBL_Price_List WHERE Price_List_ID=A.Price_List_ID)AS TotItems,(SELECT COUNT(*) FROM TBL_Customer AS X WHERE Price_List_ID=A.price_List_ID)AS TotCustomers FROM TBL_Price_List_H  AS A INNER JOIN  app_GetControlInfo(@UID) AS B ON A.Organization_Id=B.Mas_Org_ID ORDER BY Price_List_ID DESC")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@UID", UID)
            ' objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")
            LoadPriceList = MsgDs.Tables(0)
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "140243"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Function GetAssortmentBonusCategoryMap(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PlanID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT * FROM TBL_BNS_Assortment_Category_Map where Assortment_Plan_ID=@Assortment_Plan_ID")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@Assortment_Plan_ID", PlanID)
            ' objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")
            GetAssortmentBonusCategoryMap = MsgDs.Tables(0)
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "540243"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Function GetMultTrxCategoryMap(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Rule_ID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT * FROM TBL_BNS_Multi_Trx_Category_Map where Rule_ID=@Rule_ID")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@Rule_ID", Rule_ID)
            ' objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")
            GetMultTrxCategoryMap = MsgDs.Tables(0)
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "540243"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function


    Function GetDiscountCategoryMap(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PlanID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT * FROM TBL_Discount_Category_Map where Discount_Plan_ID=@Discount_Plan_ID")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@Discount_Plan_ID", PlanID)
            ' objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")
            GetDiscountCategoryMap = MsgDs.Tables(0)
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "540243"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Function GetSimpleBonusCategoryMap(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PlanID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT * FROM TBL_BNS_Plan_Category_Map where BNS_Plan_ID=@BNS_Plan_ID")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@BNS_Plan_ID", PlanID)
            ' objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")
            GetSimpleBonusCategoryMap = MsgDs.Tables(0)
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "540243"
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
            Dim QueryString As String = String.Format("SELECT DISTINCT A.Bns_Plan_ID,A.Description,A.Organization_ID AS OrgID,(SELECT Description FROM TBL_Org_CTL_H WHERE ORG_HE_ID =B.MAS_Org_ID) AS  OrgName,A.Last_Updated_At AS UpdatedAt ,(SELECT userName FROM TBL_user WHERE User_ID=A.Last_Updated_BY)AS UpdatedBy,(SELECT COUNT(DISTINCT Item_Code) FROM TBL_BNS_Promotion WHERE Bns_Plan_ID=A.Bns_Plan_ID)AS TotItems,(SELECT COUNT(*) FROM TBL_Customer_Bonus_Map  AS X INNER JOIN dbo.app_GetOrgCustomerShipAddress(A.Organization_ID)AS Y ON X.Customer_ID =Y.Customer_ID AND X.Site_Use_ID =Y.Site_Use_ID WHERE Bonus_Plan_Id=A.Bns_Plan_ID AND Plan_Type='SIMPLE')AS TotCustomers,ISNULL(Plan_Type,'N')AS PlanType,CASE WHEN ISNULL(Plan_Type,'N')='N' THEN 'By Item Quantity' ELSE 'By Invoice Value' END AS Plan_Type,isnull(Transaction_Type,'0') as Transaction_Type FROM TBL_BNS_Plan  AS A INNER JOIN  app_GetControlInfo(@UID) AS B ON A.Organization_Id=B.Mas_Org_ID ORDER BY A.Last_Updated_At DESC")
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

    Public Function ExportRedemptionRule(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "app_LoadRedemptionRules"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@OID", OrgID)

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")
            ExportRedemptionRule = MsgDs.Tables(0)
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "740243"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function ExportMultiTransRule(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "app_LoadMultiTransRules"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@OID", OrgID)

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")
            ExportMultiTransRule = MsgDs.Tables(0)
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "740243"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetVATRule(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, CustomerID As String, SiteUseID As String, InventoryItemID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "app_GetVATRules"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            objSQLCmd.Parameters.AddWithValue("@CustomerID", CustomerID)
            objSQLCmd.Parameters.AddWithValue("@SiteUseID", SiteUseID)
            objSQLCmd.Parameters.AddWithValue("@Inventory_Item_ID", InventoryItemID)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")
            GetVATRule = MsgDs.Tables(0)
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "740243"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function ExportMSL(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, Salesrep_ID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "app_GetProductMSL"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            objSQLCmd.Parameters.AddWithValue("@SalesRep_ID", Salesrep_ID)

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")
            ExportMSL = MsgDs.Tables(0)
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "740243"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function ExportMSLCustomerGroup(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, PG_ID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "app_ExportCustomerMSLGroup"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            objSQLCmd.Parameters.AddWithValue("@PGID", PG_ID)

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")
            ExportMSLCustomerGroup = MsgDs.Tables(0)
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "740243"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function ExportCustomerVanMap(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "app_ExportCustomerVanMap"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)


            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")
            ExportCustomerVanMap = MsgDs.Tables(0)
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "740243"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function ExportMSLGroup(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, PG_ID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "app_ExportProductMSLGroup"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            objSQLCmd.Parameters.AddWithValue("@PGID", PG_ID)

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")
            ExportMSLGroup = MsgDs.Tables(0)
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "740243"
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
            Dim QueryString As String = String.Format("SELECT DISTINCT A.Assortment_Plan_ID,A.Description,A.Organization_ID AS OrgID,B.Site AS OrgName,A.Valid_From,A.Valid_To,A.Last_Updated_At AS UpdatedAt ,(SELECT userName FROM TBL_user WHERE User_ID=A.Last_Updated_BY)AS UpdatedBy,CASE WHEN A.Is_Active='Y' THEN 'Yes' ELSE 'No' END AS IsActive,CASE WHEN A.Is_Active='Y' THEN CASt('1' AS Bit) ELSE CASt('0' AS Bit) END AS IsAddItems, ISNULL(Plan_Type,'N')AS PlanType,CASE WHEN ISNULL(Plan_Type,'N')='N' THEN 'Overall Quantity' ELSE 'Minimum Quantity' END AS Plan_Type,isnull(Transaction_Type,'0')as Transaction_Type  FROM TBL_BNS_Assortment_Plan  AS A INNER JOIN  app_GetControlInfo(@UID) AS B ON A.Organization_Id=B.Mas_Org_ID WHERE  Is_Active='Y'  ORDER BY A.Last_Updated_At DESC")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@UID", UID)
            '  objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
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

    Public Function SaveAssortmentMinQty(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal PlanID As String, ByVal dtOrdItems As DataTable, ByVal dtGetItems As DataTable, ByVal CreatedBy As Integer, TypeCode As String, GetQty As Decimal) As Boolean

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
            objSQLCmd = New SqlCommand("app_SaveAssortmentItemsMinQty", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Transaction = tran
            objSQLCmd.Parameters.Add("@PlanID", SqlDbType.BigInt)
            objSQLCmd.Parameters.Add("@CreatedBy", SqlDbType.Int)
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 122)
            objSQLCmd.Parameters.Add("@ItemUOM", SqlDbType.VarChar, 30)
            objSQLCmd.Parameters.Add("@IsGetItem", SqlDbType.Char, 1)
            objSQLCmd.Parameters.Add("@IsMandatory", SqlDbType.Char, 1)
            objSQLCmd.Parameters.Add("@TypeCode", SqlDbType.VarChar, 50)
            objSQLCmd.Parameters.Add("@FromQty", SqlDbType.Decimal)
            objSQLCmd.Parameters.Add("@ToQty", SqlDbType.Decimal)
            objSQLCmd.Parameters.Add("@GetQty", SqlDbType.Decimal)
            For Each dr As DataRow In dtOrdItems.Rows
                objSQLCmd.Parameters("@PlanID").Value = PlanID
                objSQLCmd.Parameters("@CreatedBy").Value = CreatedBy
                objSQLCmd.Parameters("@ItemCode").Value = dr("ItemCode").ToString()
                objSQLCmd.Parameters("@ItemUOM").Value = dr("UOM").ToString()
                objSQLCmd.Parameters("@IsGetItem").Value = dr("GetItem").ToString()
                objSQLCmd.Parameters("@IsMandatory").Value = IIf(dr("IsMandatory").ToString() = "False", "N", "Y")
                objSQLCmd.Parameters("@TypeCode").Value = TypeCode
                objSQLCmd.Parameters("@FromQty").Value = CLng(dr("Qty").ToString())
                objSQLCmd.Parameters("@ToQty").Value = CLng(dr("maxQty").ToString())
                objSQLCmd.Parameters("@GetQty").Value = GetQty
                objSQLCmd.ExecuteNonQuery()
            Next






            For Each dr As DataRow In dtGetItems.Rows
                objSQLCmd.Parameters("@PlanID").Value = PlanID
                objSQLCmd.Parameters("@CreatedBy").Value = CreatedBy
                objSQLCmd.Parameters("@ItemCode").Value = dr("ItemCode").ToString()
                objSQLCmd.Parameters("@ItemUOM").Value = dr("UOM").ToString()
                objSQLCmd.Parameters("@IsGetItem").Value = dr("GetItem").ToString()
                objSQLCmd.Parameters("@IsMandatory").Value = "N"
                objSQLCmd.Parameters("@TypeCode").Value = TypeCode
                objSQLCmd.Parameters("@FromQty").Value = 0
                objSQLCmd.Parameters("@ToQty").Value = 0
                objSQLCmd.Parameters("@GetQty").Value = 0
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

    Public Sub GetCustomerID(ByRef Err_No As Long, ByRef Err_Desc As String, OrgID As String, CustomerNo As String, ByRef CustomerID As String, ByRef SiteID As String)
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand


        Try

            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("select * FROM  dbo.app_GetOrgCustomers (@OrgID) where Customer_no=@CustomerNo")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 122).Value = CustomerNo
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            Dim MsgDs As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs)
            If MsgDs.Rows.Count > 0 Then
                CustomerID = MsgDs.Rows(0)("Customer_ID")
                SiteID = MsgDs.Rows(0)("Site_Use_ID")
            End If
            MsgDs = Nothing
            objSQLCmd.Dispose()
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24066"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Sub
    Public Function GetItemName(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemID As String, OrgID As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim Name As String = Nothing

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select item_code +' - ' +Description as Product FROM TBL_Product WHERE Inventory_item_ID=@ItemCode and Organization_ID=@OrgID")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.Int, 122).Value = ItemID
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID

            Name = Convert.ToString(objSQLCmd.ExecuteScalar())

            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24066"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return Name
    End Function
    Public Function GetProdName(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemID As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim Name As String = Nothing

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select item_code +' - ' +Description as Product FROM TBL_Product WHERE Inventory_item_ID=@ItemCode")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.Int, 122).Value = ItemID


            Name = Convert.ToString(objSQLCmd.ExecuteScalar())

            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24066"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return Name
    End Function
    Public Function GetItemNameFromCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, OrgID As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim Name As String = Nothing

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select item_code +' - ' +Description as Product FROM TBL_Product WHERE  item_Code=@ItemCode and Organization_ID=@OrgID")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar).Value = ItemCode
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID

            Name = Convert.ToString(objSQLCmd.ExecuteScalar())

            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24066"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return Name
    End Function
    Public Function GetItemNameOnlyFromCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, OrgID As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim Name As String = Nothing

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select Description as Product FROM TBL_Product WHERE  item_Code=@ItemCode and Organization_ID=@OrgID")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar).Value = ItemCode
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID

            Name = Convert.ToString(objSQLCmd.ExecuteScalar())

            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24066"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return Name
    End Function
    Public Function GetInventoryItemID(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, OrgID As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim UOM As String = Nothing

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select Inventory_Item_ID FROM TBL_Product WHERE Item_Code=@ItemCode and Organization_ID=@OrgID")
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
    Public Function GetConversion(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal OrgID As String, UOM As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Dim Conversion As String = "1"
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select Conversion FROM TBL_Item_UOM WHERE Item_Code=@ItemCode AND Organization_ID=@OrgID and Item_UOM=@UOM ")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 122).Value = ItemCode
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            objSQLCmd.Parameters.Add("@UOM", SqlDbType.VarChar, 100).Value = UOM

            Conversion = Convert.ToString(objSQLCmd.ExecuteScalar())

            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24066"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return Conversion
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
    Public Function GetProdcutPrice(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal OrgID As String, PriceList As String, UOm As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("app_GetProdcutPrice")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 122).Value = ItemCode
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            objSQLCmd.Parameters.Add("@Price_List_ID", SqlDbType.VarChar, 100).Value = PriceList
            objSQLCmd.Parameters.Add("@UOM", SqlDbType.VarChar, 100).Value = UOm

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")

            GetProdcutPrice = MsgDs.Tables(0)
            objSQLCmd.Dispose()


        Catch ex As Exception
            Err_No = "24066"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return GetProdcutPrice
    End Function
    Public Function GetItemUOMsWithPrice(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal OrgID As String, PriceList As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim UOM As String = Nothing

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("app_GetItemUOMsWithPrice")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 122).Value = ItemCode
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            objSQLCmd.Parameters.Add("@Price_List_ID", SqlDbType.VarChar, 100).Value = PriceList

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")

            GetItemUOMsWithPrice = MsgDs.Tables(0)
            objSQLCmd.Dispose()


        Catch ex As Exception
            Err_No = "24066"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return GetItemUOMsWithPrice
    End Function
    Public Function GetItemUOMs(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim UOM As String = Nothing

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select * FROM TBL_Item_UOM WHERE Item_Code=@ItemCode AND Organization_ID=@OrgID ")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 122).Value = ItemCode
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")

            GetItemUOMs = MsgDs.Tables(0)
            objSQLCmd.Dispose()


        Catch ex As Exception
            Err_No = "24066"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return GetItemUOMs
    End Function

    Public Function CheckItemUOMandGetfromDB(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal OrgID As String, ByRef UOM As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select Item_UOM FROM TBL_Item_UOM WHERE Item_Code=@ItemCode AND Organization_ID=@OrgID AND Item_UOM=@UOM")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 122).Value = ItemCode
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            objSQLCmd.Parameters.Add("@UOM", SqlDbType.VarChar, 100).Value = UOM
             
            Dim MsgDs As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs)

            If MsgDs.Rows.Count > 0 Then
                UOM = MsgDs.Rows(0)("Item_UOM").ToString
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
    Public Function CheckItemUOM(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, ByVal OrgID As String, UOM As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select COUNT(*) FROM TBL_Item_UOM WHERE Item_Code=@ItemCode AND Organization_ID=@OrgID AND Item_UOM=@UOM  ")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 122).Value = ItemCode
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            objSQLCmd.Parameters.Add("@UOM", SqlDbType.VarChar, 100).Value = UOM
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
    Public Function CheckValidShipAddress(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustomerNo As String, SiteNo As String, ByVal OrgID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select COUNT(*) from  app_GetOrgCustomerShipAddress(@OrgID) where Customer_no=@CustomerNo AND Dept=@SiteNo")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 122).Value = CustomerNo
            objSQLCmd.Parameters.Add("@SiteNo", SqlDbType.VarChar, 122).Value = SiteNo
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
    Public Function CheckValidCustomer(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustomerNo As String, SiteNo As String, ByVal OrgID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select COUNT(*) from app_GetOrgCustomers(@OrgID) where Customer_no=@CustomerNo")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 122).Value = CustomerNo
            ' objSQLCmd.Parameters.Add("@SiteNo", SqlDbType.VarChar, 122).Value = SiteNo
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
    Public Function CheckCustomerNo(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustomerNo As String, ByVal OrgID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select COUNT(*) from dbo.app_GetOrgCustomers (@OrgID) where Customer_no=@CustomerNo")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 122).Value = CustomerNo
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

    Public Function CheckValidVan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, Van As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select COUNT(*) FROM TBL_org_CTL_DTl WHERE Mas_Org_ID=@OrgID and Org_ID=@Van  ")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            objSQLCmd.Parameters.Add("@Van", SqlDbType.VarChar, 100).Value = Van
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
    Public Function CheckPriceDataExists(ByRef Err_No As Long, ByRef Err_Desc As String, ItemID As Integer, ByVal OrgID As String, ByVal PriceListID As String, UOM As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select COUNT(*) FROM TBL_Price_List  WHERE Price_List_ID=@PriceListID AND Item_UOM=@UOM AND Inventory_Item_ID=@ItemID  and Organization_ID=@OrgID  ")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@PriceListID", SqlDbType.VarChar, 100).Value = PriceListID
            objSQLCmd.Parameters.Add("@ItemID", SqlDbType.VarChar, 100).Value = ItemID
            objSQLCmd.Parameters.Add("@UOM", SqlDbType.VarChar, 100).Value = UOM
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            Dim cnt As Integer = 0
            cnt = Convert.ToInt32(objSQLCmd.ExecuteScalar())
            If cnt > 0 Then
                success = True
            End If
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24064"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
    Public Function CheckMultiTransRuleExists(ByRef Err_No As Long, ByRef Err_Desc As String, RuleID As Integer, SalesItem As String, ByVal OrgID As String, ValidFrom As Date, ValidTo As Date, TransactionType As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = ""
            If RuleID = "0" Then
                QueryString = "SELECT     COUNT(*) FROM TBL_BNS_Multi_Trx  AS A WHERE  A.Sales_Item_Code =@SalesItem and A.Organization_ID=@OrgID AND Transaction_type=@Transaction_type AND  (( Promo_Start_Date BETWEEN   @ValidFrom and @ValidTo or (@ValidFrom BETWEEN Promo_Start_Date AND Promo_End_Date  )) AND Is_Active='Y'  or ((Promo_End_Date BETWEEN @ValidFrom AND @ValidTo or(@ValidTo  BETWEEN Promo_Start_Date AND Promo_End_Date ))  AND Is_Active='Y'))"
            Else
                QueryString = "SELECT     COUNT(*) FROM TBL_BNS_Multi_Trx  AS A WHERE A.Rule_ID<>@RuleID AND  A.Sales_Item_Code =@SalesItem and A.Organization_ID=@OrgID AND Transaction_type=@Transaction_type AND(  ( Promo_Start_Date BETWEEN   @ValidFrom and @ValidTo or (@ValidFrom BETWEEN Promo_Start_Date AND Promo_End_Date  )) AND Is_Active='Y'  or ((Promo_End_Date BETWEEN @ValidFrom AND @ValidTo or(@ValidTo  BETWEEN Promo_Start_Date AND Promo_End_Date ))  AND Is_Active='Y'))"
            End If
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@RuleID", SqlDbType.Int).Value = RuleID
            objSQLCmd.Parameters.Add("@SalesItem", SqlDbType.VarChar, 100).Value = SalesItem
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            objSQLCmd.Parameters.Add("@ValidFrom", SqlDbType.DateTime).Value = ValidFrom
            objSQLCmd.Parameters.Add("@ValidTo", SqlDbType.DateTime).Value = ValidTo
            objSQLCmd.Parameters.Add("@Transaction_type", SqlDbType.VarChar).Value = TransactionType
            Dim cnt As Integer = 0
            cnt = Convert.ToInt32(objSQLCmd.ExecuteScalar())
            If cnt > 0 Then
                success = True
            End If
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24064"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
    Public Function CheckRedemptionItemExists(ByRef Err_No As Long, ByRef Err_Desc As String, RuleID As Integer, RedItem As String, ByVal OrgID As String, ValidFrom As Date, ValidTo As Date) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = ""
            If RuleID = "0" Then
                QueryString = "SELECT     COUNT(*) FROM TBL_Redemption_Rules   AS A WHERE  A.Return_Item_Code =@RedItem and A.Organization_ID=@OrgID AND  (( Valid_From BETWEEN   @ValidFrom and @ValidTo or (@ValidFrom BETWEEN Valid_From AND Valid_To )) AND Is_Active='Y'  or ((Valid_To BETWEEN @ValidFrom AND @ValidTo or(@ValidTo  BETWEEN Valid_From AND Valid_To )) )) AND Is_Active='Y'"
            Else
                QueryString = "SELECT     COUNT(*) FROM TBL_Redemption_Rules   AS A WHERE A.Rule_ID<>@RuleID AND  A.Return_Item_Code =@RedItem and A.Organization_ID=@OrgID AND(  ( Valid_From BETWEEN   @ValidFrom and @ValidTo or (@ValidFrom BETWEEN Valid_From AND Valid_To  )) AND Is_Active='Y'  or ((Valid_To BETWEEN @ValidFrom AND @ValidTo or(@ValidTo  BETWEEN Valid_From AND Valid_To))  AND Is_Active='Y'))"
            End If
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@RuleID", SqlDbType.Int).Value = RuleID
            objSQLCmd.Parameters.Add("@RedItem", SqlDbType.VarChar, 100).Value = RedItem
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
            Err_No = "24064"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
    Public Function CheckPriceListName(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal PriceListName As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select COUNT(*) FROM TBL_Price_List_H WHERE Description=@PriceListName and Organization_ID=@OrgID  ")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@PriceListName", SqlDbType.VarChar, 100).Value = PriceListName
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            Dim cnt As Integer = 0
            cnt = Convert.ToInt32(objSQLCmd.ExecuteScalar())
            If cnt > 0 Then
                success = True
            End If
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "24062"
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
    Public Function DeleteProductImage(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal MediaFileID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim _sUserID As Integer
        Dim iRowsAffected As Integer = 0
        Dim retVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Try
                sQry = "UPDATE TBL_Media_Files SET Is_Deleted='Y' WHERE Media_File_ID=@MediaFileID"

                objSQLCmd = New SqlCommand(sQry, objSQLConn)
                objSQLCmd.CommandType = CommandType.Text
                objSQLCmd.Parameters.AddWithValue("@MediaFileID", MediaFileID)

                iRowsAffected = objSQLCmd.ExecuteNonQuery()
                objSQLCmd.Dispose()


                If iRowsAffected > 0 Then
                    retVal = True
                Else
                    Error_No = 77001
                    Error_Desc = "Unable to delete image."
                End If
            Catch ex As Exception

                Throw ex
            End Try
        Catch ex As Exception
            Error_No = 77001
            Error_Desc = String.Format("Error while deleting product image: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        DeleteProductImage = retVal
    End Function
    Public Function SaveProductImage(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal MediaFileID As String, ByVal ItemID As Integer, ByVal OrgID As Integer, ByVal MediaType As String, ByVal MediaFile As String, ByVal ThumbNail As String, ByVal Caption As String, ByVal CreatedBy As String, IsDefault As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim _sUserID As Integer
        Dim iRowsAffected As Integer = 0
        Dim retVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Try
                sQry = "app_InsertProductImage"

                objSQLCmd = New SqlCommand(sQry, objSQLConn)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.Parameters.AddWithValue("@MediaFileID", MediaFileID)
                objSQLCmd.Parameters.AddWithValue("@ItemID", ItemID)
                objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
                objSQLCmd.Parameters.AddWithValue("@MediaType", MediaType)
                objSQLCmd.Parameters.AddWithValue("@MediaFile", MediaFile)
                objSQLCmd.Parameters.AddWithValue("@Caption", Caption)
                objSQLCmd.Parameters.AddWithValue("@Thumbnail", ThumbNail)
                objSQLCmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
                objSQLCmd.Parameters.AddWithValue("@IsDefault", IsDefault)


                iRowsAffected = objSQLCmd.ExecuteNonQuery()
                objSQLCmd.Dispose()


                If iRowsAffected > 0 Then
                    retVal = True
                Else
                    Error_No = 77001
                    Error_Desc = "Unable to saveproduct image."
                End If
            Catch ex As Exception

                Throw ex
            End Try
        Catch ex As Exception
            Error_No = 77001
            Error_Desc = String.Format("Error while saving product image: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        SaveProductImage = retVal
    End Function
    Public Function GetProductUOM(ByRef Err_No As Long, ByRef Err_Desc As String, OrgID As String, ItemID As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            'Dim QueryString As String = String.Format("SELECT DISTINCT MAS_Org_ID, Site FROM TBL_Org_CTL_DTL WHERE SalesRep_ID IN ({0}) ORDER BY MAS_Org_ID DESC", QueryStr)
            Dim QueryString As String = "SELECT DISTINCT Item_UOM AS UOM FROM TBL_Item_UOM AS A INNER JOIN TBL_Product AS B On A.Organization_ID=B.Organization_ID AND A.Item_Code=B.Item_Code  WHERE A.Organization_ID=@OrgID AND B.Inventory_Item_ID=@ItemID ORDER BY Item_Uom"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            objSQLCmd.Parameters.AddWithValue("@orgid", OrgID)
            objSQLCmd.Parameters.AddWithValue("@Itemid", ItemID)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")

            GetProductUOM = MsgDs.Tables(0)
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
    Public Function GetProductImages(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, Inventory_item_Id As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = Nothing

            QueryString = "app_GetProductMedia"



            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@OrgId", SqlDbType.VarChar, 100).Value = OrgID
            objSQLCmd.Parameters.Add("@Invetory_item_Id", SqlDbType.Int).Value = Inventory_item_Id
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
    Public Function LoadDiscountData(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal PlanID As String) As DataTable
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
            objSQLCmd.Parameters.Add("@PlanID", SqlDbType.Int).Value = PlanID
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


    Public Function ExportPriceData(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal PriceListID As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = Nothing


            QueryString = "SELECT Item_Code AS ItemCode,Item_UOM AS UOM,Unit_Selling_Price AS SellingPrice  FROM TBL_Price_List AS A INNER JOIN TBL_Product AS B On A.Inventory_item_ID=B.Inventory_Item_ID AND A.Organization_ID=B.Organization_ID WHERE A.Organization_ID =@OrgID AND A.Price_List_ID =@PriceListID"


            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            objSQLCmd.Parameters.Add("@PriceListID", SqlDbType.Int).Value = PriceListID

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

    Public Function ExportBonusData(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal BnsPlanID As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = Nothing


            QueryString = "SELECT B.Organization_ID As OrgID,B.Item_Code AS OrderItem,B.Get_Item AS BonusItem,Prom_Qty_From AS FromQty,Prom_Qty_To As ToQty,Price_Break_Type_Code AS Type,Get_Qty AS GetQty,Valid_From AS ValidFrom ,Valid_To As ValidTo,ISNULL(B.Max_FOC_Qty,0)AS MaxQty,B.Item_UOM,B.Get_UOM  FROM TBL_BNS_Plan AS A INNER JOIN TBL_BNS_Promotion AS B On A.BNS_Plan_ID =B.BNS_Plan_ID WHERE A.Organization_ID =@OrgID AND A.BNS_Plan_ID =@BnsPlanID AND B.Is_Active ='Y' "


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
                QueryString = "SELECT     A.BNS_Promotion_ID AS LineID, A.Item_Code + '-'+ (SELECT Description FROM TBL_Product WHERE Item_Code =A.item_Code AND A.Organization_ID=Organization_ID)  AS ItemCode,(SELECT Inventory_Item_Id FROM TBL_Product WHERE Item_Code=A.Item_Code AND A.Organization_ID=Organization_ID) AS DItemId,(SELECT Inventory_Item_Id FROM TBL_Product WHERE Item_Code=A.Get_Item AND  A.Organization_ID=Organization_ID ) AS BItemId, A.Organization_ID AS OrgId,Item_UOM AS DUOM, Prom_Qty_From AS FromQty, Prom_Qty_To As ToQty, Price_Break_Type_Code AS TypeCode, Get_Item  + '-'+ (SELECT Description FROM TBL_Product WHERE Item_Code =A.Get_item AND A.Organization_ID=Organization_ID)  As BItemCode, Get_UOM AS BUOM, Get_Qty AS GetQty, ISNULL(Get_Add_Per,0)*100 AS Get_Add_Per,A.Item_Code AS ACode,A.get_Item AS BCode,(SELECT Description FROM TBL_Product WHERE Item_Code =A.item_Code AND A.Organization_ID=Organization_ID)  AS ADesc,(SELECT Description FROM TBL_Product WHERE Item_Code =A.Get_Item AND A.Organization_ID=Organization_ID)  AS BDesc,A.Valid_From,A.Valid_To,CASE WHEN A.Is_Active='N' THEN 'Activate' ELSE 'Deactivate' END AS IsActive,CASE WHEN A.Is_Active='N' THEN '#FF3300' ELSE '#006600' END AS IsColor,CASE WHEN Is_Active='Y' THEN CAST('1' AS BIT) ELSE CAST('0' AS BIT) END AS IsVisible,ISNULL(A.Max_FOC_qty,0)AS MaxQty FROM TBL_BNS_Promotion AS  A WHERE  A.Item_Code=@ItemCode AND A.Organization_ID=@OrgID AND Bns_Plan_Id=@BnsPlanID Order By    A.item_Code,Prom_Qty_From"
            ElseIf ShowInActive = "Y" And ItemCode = "" Then
                QueryString = "SELECT     A.BNS_Promotion_ID AS LineID, A.Item_Code + '-'+ (SELECT Description FROM TBL_Product WHERE Item_Code =A.item_Code AND A.Organization_ID=Organization_ID)  AS ItemCode,(SELECT Inventory_Item_Id FROM TBL_Product WHERE Item_Code=A.Item_Code AND A.Organization_ID=Organization_ID) AS DItemId,(SELECT Inventory_Item_Id FROM TBL_Product WHERE Item_Code=A.Get_Item AND  A.Organization_ID=Organization_ID ) AS BItemId, A.Organization_ID AS OrgId,Item_UOM AS DUOM, Prom_Qty_From AS FromQty, Prom_Qty_To As ToQty, Price_Break_Type_Code AS TypeCode, Get_Item  + '-'+ (SELECT Description FROM TBL_Product WHERE Item_Code =A.Get_item AND A.Organization_ID=Organization_ID)  As BItemCode, Get_UOM AS BUOM, Get_Qty AS GetQty, ISNULL(Get_Add_Per,0)*100 AS Get_Add_Per,A.Item_Code AS ACode,A.get_Item AS BCode,(SELECT Description FROM TBL_Product WHERE Item_Code =A.item_Code AND A.Organization_ID=Organization_ID)  AS ADesc,(SELECT Description FROM TBL_Product WHERE Item_Code =A.Get_Item AND A.Organization_ID=Organization_ID)  AS BDesc,A.Valid_From,A.Valid_To,CASE WHEN A.Is_Active='N' THEN 'Activate' ELSE 'Deactivate' END AS IsActive,CASE WHEN A.Is_Active='N' THEN '#FF3300' ELSE '#006600' END AS IsColor,CASE WHEN Is_Active='Y' THEN CAST('1' AS BIT) ELSE CAST('0' AS BIT) END AS IsVisible,ISNULL(A.Max_FOC_qty,0)AS MaxQty FROM TBL_BNS_Promotion AS  A WHERE  A.Organization_ID=@OrgID AND Bns_Plan_Id=@BnsPlanID Order By    A.item_Code,Prom_Qty_From"

            ElseIf ShowInActive <> "Y" And ItemCode <> "" Then
                QueryString = "SELECT     A.BNS_Promotion_ID AS LineID, A.Item_Code + '-'+ (SELECT Description FROM TBL_Product WHERE Item_Code =A.item_Code AND A.Organization_ID=Organization_ID)  AS ItemCode,(SELECT Inventory_Item_Id FROM TBL_Product WHERE Item_Code=A.Item_Code AND A.Organization_ID=Organization_ID) AS DItemId,(SELECT Inventory_Item_Id FROM TBL_Product WHERE Item_Code=A.Get_Item AND  A.Organization_ID=Organization_ID ) AS BItemId, A.Organization_ID AS OrgId,Item_UOM AS DUOM, Prom_Qty_From AS FromQty, Prom_Qty_To As ToQty, Price_Break_Type_Code AS TypeCode, Get_Item  + '-'+ (SELECT Description FROM TBL_Product WHERE Item_Code =A.Get_item AND A.Organization_ID=Organization_ID)  As BItemCode, Get_UOM AS BUOM, Get_Qty AS GetQty, ISNULL(Get_Add_Per,0)*100 AS Get_Add_Per,A.Item_Code AS ACode,A.get_Item AS BCode,(SELECT Description FROM TBL_Product WHERE Item_Code =A.item_Code AND A.Organization_ID=Organization_ID)  AS ADesc,(SELECT Description FROM TBL_Product WHERE Item_Code =A.Get_Item AND A.Organization_ID=Organization_ID)  AS BDesc,A.Valid_From,A.Valid_To,CASE WHEN A.Is_Active='N' THEN 'Activate' ELSE 'Deactivate' END AS IsActive,CASE WHEN A.Is_Active='N' THEN '#FF3300' ELSE '#006600' END AS IsColor,CASE WHEN Is_Active='Y' THEN CAST('1' AS BIT) ELSE CAST('0' AS BIT) END AS IsVisible,ISNULL(A.Max_FOC_qty,0)AS MaxQty FROM TBL_BNS_Promotion AS  A WHERE  A.Item_Code=@ItemCode AND A.Organization_ID=@OrgID AND Is_Active='Y' AND Bns_Plan_Id=@BnsPlanID Order By    A.item_Code,Prom_Qty_From"

            ElseIf ShowInActive <> "Y" And ItemCode = "" Then
                QueryString = "SELECT     A.BNS_Promotion_ID AS LineID, A.Item_Code + '-'+ (SELECT Description FROM TBL_Product WHERE Item_Code =A.item_Code AND A.Organization_ID=Organization_ID)  AS ItemCode,(SELECT Inventory_Item_Id FROM TBL_Product WHERE Item_Code=A.Item_Code AND A.Organization_ID=Organization_ID) AS DItemId,(SELECT Inventory_Item_Id FROM TBL_Product WHERE Item_Code=A.Get_Item AND  A.Organization_ID=Organization_ID ) AS BItemId, A.Organization_ID AS OrgId,Item_UOM AS DUOM, Prom_Qty_From AS FromQty, Prom_Qty_To As ToQty, Price_Break_Type_Code AS TypeCode, Get_Item  + '-'+ (SELECT Description FROM TBL_Product WHERE Item_Code =A.Get_item AND A.Organization_ID=Organization_ID)  As BItemCode, Get_UOM AS BUOM, Get_Qty AS GetQty, ISNULL(Get_Add_Per,0)*100 AS Get_Add_Per,A.Item_Code AS ACode,A.get_Item AS BCode,(SELECT Description FROM TBL_Product WHERE Item_Code =A.item_Code AND A.Organization_ID=Organization_ID)  AS ADesc,(SELECT Description FROM TBL_Product WHERE Item_Code =A.Get_Item AND A.Organization_ID=Organization_ID)  AS BDesc,A.Valid_From,A.Valid_To,CASE WHEN A.Is_Active='N' THEN 'Activate' ELSE 'Deactivate' END AS IsActive,CASE WHEN A.Is_Active='N' THEN '#FF3300' ELSE '#006600' END AS IsColor,CASE WHEN Is_Active='Y' THEN CAST('1' AS BIT) ELSE CAST('0' AS BIT) END AS IsVisible,ISNULL(A.Max_FOC_qty,0)AS MaxQty FROM TBL_BNS_Promotion AS  A WHERE   A.Organization_ID=@OrgID AND Is_Active='Y' AND Bns_Plan_Id=@BnsPlanID Order By    A.item_Code,Prom_Qty_From"

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


    Public Function GetPriceListData(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ItemID As String, ByVal PriceListID As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = Nothing

            If ItemID = "0" Or ItemID = "" Then
                QueryString = "SELECT     A.Price_List_Line_ID AS LineID, (SELECT  Item_Code + '-'+Description FROM TBL_Product WHERE Inventory_Item_ID =A.Inventory_Item_ID AND A.Organization_ID=Organization_ID)  AS ItemCode,A.Inventory_Item_ID  AS DItemId,(SELECT Item_Code FROM TBL_Product WHERE Inventory_Item_ID =A.Inventory_Item_ID AND A.Organization_ID=Organization_ID)  AS DItemCode, A.Organization_ID AS OrgId,Item_UOM AS DUOM, Unit_Selling_Price AS UnitSellingPrice FROM TBL_Price_List AS  A WHERE   A.Organization_ID=@OrgID AND  price_List_Id=@PriceListID Order By    A.Inventory_Item_ID"
            Else
                QueryString = "SELECT     A.Price_List_Line_ID AS LineID, (SELECT Item_Code + '-'+Description FROM TBL_Product WHERE Inventory_Item_ID =A.Inventory_Item_ID AND A.Organization_ID=Organization_ID)  AS ItemCode,(SELECT Item_Code FROM TBL_Product WHERE Inventory_Item_ID =A.Inventory_Item_ID AND A.Organization_ID=Organization_ID)  AS DItemCode, A.Inventory_Item_ID  AS DItemId, A.Organization_ID AS OrgId,Item_UOM AS DUOM, Unit_Selling_Price AS UnitSellingPrice FROM TBL_Price_List AS  A WHERE   A.Organization_ID=@OrgID AND  price_List_Id=@PriceListID AND A.Inventory_Item_ID=@ItemID Order By    A.Inventory_Item_ID"
            End If

            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@ItemID", SqlDbType.VarChar, 100).Value = ItemID
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            objSQLCmd.Parameters.Add("@PriceListID", SqlDbType.Int).Value = PriceListID

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
    Public Function UpdateBonusData(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal LineId As String, ByVal BItemCode As String, ByVal BUOM As String, ByVal TypeCode As String, ByVal FromQty As Long, ByVal ToQty As Long, ByVal GetQty As Long, ByVal GetPer As Decimal, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, ByVal UpDatedBy As Integer, ByVal BnsPlanID As Integer, MaxQty As Long) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Update TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "UPDATE TBL_BNS_Promotion SET Max_FOC_Qty=@MaxQty, Prom_Qty_From=@FromQty ,Prom_Qty_To =@ToQty,Price_Break_Type_Code=@TypeCode ,Get_Item=@BitemCode ,Get_UOM=@BUOM,Get_Qty=@GetQty,Get_Add_Per=@GetPer ,Sync_Timestamp =GETDATE(),Valid_From=@ValidFrom,Valid_To=@ValidTo,Last_Updated_At=GetDATE(),Last_Updated_By=@UpdatedBy WHERE BNS_Promotion_ID=@LineID AND Bns_Plan_ID=@BnsPlanID"
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
            objSQLCmd.Parameters.AddWithValue("@MaxQty", MaxQty)
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
    Public Function UpdateInvoiceRule(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal InvValue As Decimal, MinItem As Integer, ByVal OrgId As String, ByVal FocItem As String, ByVal GetQty As Long, ByVal ValidFrom As DateTime, ByVal ValidTo As DateTime, ByVal CreatedBy As Integer, ByVal BnsPlanID As Integer, LineID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Update TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "UPDATE TBL_BNS_BIV SET Min_Invoice_Value=@MinInvValue,Min_Ordered_Items=@MinItem ,FOC_Item_Code =@FocItem,FOC_Qty=@FOCQty ,Valid_From=@ValidFrom,Valid_To=@ValidTo,Last_Updated_At=GetDATE(),Last_Updated_By=@CreatedBy WHERE Rule_ID=@LineID AND Bns_Plan_ID=@BnsPlanID"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@BnsPlanID", BnsPlanID)
            objSQLCmd.Parameters.AddWithValue("@MinInvValue", InvValue)
            objSQLCmd.Parameters.AddWithValue("@MinItem", MinItem)
            objSQLCmd.Parameters.AddWithValue("@FOCItem", FocItem)
            objSQLCmd.Parameters.AddWithValue("@FOCQty", GetQty)

            objSQLCmd.Parameters.AddWithValue("@ValidFrom", ValidFrom)
            objSQLCmd.Parameters.AddWithValue("@ValidTo", ValidTo)
            objSQLCmd.Parameters.AddWithValue("@LineID", LineID)
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


    Public Function UpdatePriceData(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal LineId As String, ByVal ItemID As String, ByVal DUOM As String, ByVal UnitPrice As Decimal, ByVal PriceListID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Update TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "UPDATE TBL_Price_List SET Unit_Selling_Price=@UnitPrice ,Sync_Timestamp =GETDATE() WHERE  Price_List_Line_ID=@LineID AND Price_List_ID=@PriceListID AND Item_UOM=@UOM AND Inventory_Item_ID=@ItemID"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@LineId", LineId)
            objSQLCmd.Parameters.AddWithValue("@UnitPrice", UnitPrice)
            objSQLCmd.Parameters.AddWithValue("@ItemID", ItemID)
            objSQLCmd.Parameters.AddWithValue("@UOM", DUOM)
            objSQLCmd.Parameters.AddWithValue("@PriceListID", PriceListID)
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
    Public Function LoadBrandList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT DISTINCT CASE WHEN Brand_Code='' OR Brand_Code IS NULL THEN 'Others' ELSE Brand_Code END AS Code, CASE WHEN Brand_Code='' OR Brand_Code IS NULL THEN 'Others' ELSE Brand_Code END  AS Description FROM TBL_Product WHERE Organization_ID=@OrgID  ORDER By CASE WHEN Brand_Code='' OR Brand_Code IS NULL THEN 'Others' ELSE Brand_Code END ")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            Dim MsgDs As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs)
            LoadBrandList = MsgDs
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "740243"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function UploadFSRProduct(ByVal OrgID As String, ByVal SalesRepID As Integer, ByVal dtData As DataTable, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Dim tran As SqlTransaction = Nothing
        Dim QueryString As String = Nothing
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            tran = objSQLConn.BeginTransaction()

            'Delete Product FSR exisitng data by FSR
            QueryString = "DELETE FROM  TBL_Product_FSR WHERE Organization_ID=@OrgID AND SalesRep_ID=@SID"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            objSQLCmd.Parameters.AddWithValue("@SID", SalesRepID)
            objSQLCmd.Transaction = tran
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()

            For Each r As DataRow In dtData.Rows

                QueryString = "app_UploadFSRProduct"
                objSQLCmd = New SqlCommand(QueryString, objSQLConn)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 50).Value = r("Item_Code").ToString()
                objSQLCmd.Parameters.Add("@OrganizationID", SqlDbType.VarChar, 50).Value = OrgID
                objSQLCmd.Parameters.Add("@VanId", SqlDbType.Int).Value = SalesRepID
                objSQLCmd.Transaction = tran
                objSQLCmd.ExecuteNonQuery()
                objSQLCmd.Dispose()

            Next



            success = True
            tran.Commit()
        Catch ex As Exception
            Err_No = "87319"
            Err_Desc = ex.Message
            tran.Rollback()
            Throw ex
        Finally
            tran.Dispose()
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
    Public Function LoadFSRProductTemplate(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal OrgID As String, ByVal SalesrepID As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim sQry As String
        Dim sRetVal As String = ""
        Dim dtFSRProducts As New DataTable
        Try

            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            sQry = "app_GetProductFSRTemplate"
            objSQLDA = New SqlDataAdapter(sQry, objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.Add("@OrgID", SqlDbType.VarChar, 100).Value = OrgID
            objSQLDA.SelectCommand.Parameters.Add("@VanID", SqlDbType.BigInt).Value = SalesrepID

            objSQLDA.Fill(dtFSRProducts)
            objSQLDA.Dispose()
        Catch ex As Exception
            sRetVal = sRetVal & ex.Message
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtFSRProducts
    End Function
    Public Function DeleteCustomerVanMap(ByVal DeleteMode As String, ByVal OrgId As String, ByVal VanId As String, ByVal CustomerID As Integer, SiteID As String, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_DeleteCustomerVanMap"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@OrganizationID", SqlDbType.VarChar, 20).Value = OrgId
            objSQLCmd.Parameters.Add("@VanID", SqlDbType.Int).Value = VanId
            objSQLCmd.Parameters.Add("@CustID", SqlDbType.Int).Value = CustomerID
            objSQLCmd.Parameters.Add("@SiteID", SqlDbType.VarChar, 50).Value = SiteID
            objSQLCmd.Parameters.Add("@DeleteMode", SqlDbType.VarChar, 20).Value = DeleteMode
            objSQLCmd.ExecuteNonQuery()
            success = True
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "54098"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
    Public Function DeleteCustomerMSLGroup(ByVal DeleteMode As String, ByVal OrgId As String, ByVal GroupId As Integer, ByVal CustomerID As Integer, SiteID As String, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_DeleteCustomerMSLGroup"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@OrganizationID", SqlDbType.VarChar, 20).Value = OrgId
            objSQLCmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = GroupId
            objSQLCmd.Parameters.Add("@CustID", SqlDbType.Int).Value = CustomerID
            objSQLCmd.Parameters.Add("@SiteID", SqlDbType.VarChar, 50).Value = SiteID
            objSQLCmd.Parameters.Add("@DeleteMode", SqlDbType.VarChar, 20).Value = DeleteMode
            objSQLCmd.ExecuteNonQuery()
            success = True
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "54098"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
    Public Function DeleteProductGroupFSR(ByVal DeleteMode As String, ByVal OrgId As String, ByVal GroupId As Integer, ByVal SalesRepId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_DeleteProductGroupFSR"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            ' objSQLCmd.Parameters.Add("@Inv_ItemID", SqlDbType.Int).Value = ItemId
            'objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 50).Value = ItemCode
            objSQLCmd.Parameters.Add("@OrganizationID", SqlDbType.VarChar, 20).Value = OrgId
            objSQLCmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = GroupId
            objSQLCmd.Parameters.Add("@SalesRepId", SqlDbType.Int).Value = SalesRepId
            objSQLCmd.Parameters.Add("@DeleteMode", SqlDbType.VarChar, 20).Value = DeleteMode
            objSQLCmd.ExecuteNonQuery()
            success = True
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "54098"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function

    Public Function DeleteCollectionGroupFSR(ByVal DeleteMode As String, ByVal OrgId As String, ByVal GroupId As String, ByVal SalesRepId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_DeleteCollectionGroupFSR"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@OrganizationID", SqlDbType.VarChar, 20).Value = OrgId
            objSQLCmd.Parameters.Add("@GroupID", SqlDbType.VarChar, 250).Value = GroupId
            objSQLCmd.Parameters.Add("@SalesRepId", SqlDbType.Int).Value = SalesRepId
            objSQLCmd.Parameters.Add("@DeleteMode", SqlDbType.VarChar, 20).Value = DeleteMode
            objSQLCmd.ExecuteNonQuery()
            success = True
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "54098"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
    Public Function InsertProductGroupFSR(ByVal OrgId As String, ByVal GroupId As Integer, ByVal SalesRepId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Mode As String, ByRef Msg As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_InsertProductGroupFSR"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            ' objSQLCmd.Parameters.Add("@Inv_ItemID", SqlDbType.Int).Value = ItemId
            ' objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 50).Value = ItemCode
            objSQLCmd.Parameters.Add("@OrganizationID", SqlDbType.VarChar, 20).Value = OrgId
            objSQLCmd.Parameters.Add("@GroupId", SqlDbType.Int).Value = GroupId
            objSQLCmd.Parameters.Add("@SalesRepId", SqlDbType.Int).Value = SalesRepId
            objSQLCmd.Parameters.Add("@Mode", SqlDbType.VarChar, 20).Value = Mode
            ' objSQLCmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = CreatedBy
            Dim MsgDs As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs)
            If MsgDs.Rows.Count > 0 Then
                If MsgDs.Rows(0)(0) = "0" Then
                    Msg = "The products from one/more selected groups are already assigned to the FSR"
                End If
            End If
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


    Public Function InserCollectionGroupFSR(ByVal OrgId As String, ByVal GroupId As String, ByVal SalesRepId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Mode As String, CreatedBy As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_InsertCollectionGroupFSR"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure

            objSQLCmd.Parameters.Add("@OrganizationID", SqlDbType.VarChar, 20).Value = OrgId
            objSQLCmd.Parameters.Add("@GroupId", SqlDbType.VarChar, 250).Value = GroupId
            objSQLCmd.Parameters.Add("@SalesRepId", SqlDbType.Int).Value = SalesRepId
            objSQLCmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = CreatedBy
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()
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


    Public Function DeleteProductGroupAll(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal GroupId As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sRetVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("DELETE FROM TBL_Product_Group WHERE PG_ID=@GroupId;DELETE FROM TBL_Product_Group_items WHERE PG_ID=@GroupId;DELETE FROM TBL_Product_FSR WHERE PG_ID=@GroupId", objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = GroupId
            objSQLCmd.ExecuteNonQuery()
            sRetVal = True
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74013"
            Err_Desc = ex.Message
            sRetVal = False
            Throw ex
        Finally
            objSQLCmd = Nothing
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        DeleteProductGroupAll = sRetVal
    End Function
    Public Function DeleteProductMSLGroupAll(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal GroupId As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sRetVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("DELETE FROM TBL_Product_MSL_Group WHERE PG_ID=@GroupId;DELETE FROM TBL_Product_MSL_Group_items WHERE PG_ID=@GroupId;DELETE FROM TBL_Customer_MSL_Group WHERE PG_ID=@GroupId", objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = GroupId
            objSQLCmd.ExecuteNonQuery()
            sRetVal = True
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74013"
            Err_Desc = ex.Message
            sRetVal = False
            Throw ex
        Finally
            objSQLCmd = Nothing
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        DeleteProductMSLGroupAll = sRetVal
    End Function
    Public Function GetProductMSLGroup(ByRef Err_No As Long, ByRef Err_Desc As String, OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = ""
            If OrgID = "0" Then
                QueryString = "SELECT A.PG_ID AS PGID, A.Description,A.Last_Updated_On AS CreatedAt,(SELECT Username FROM TBL_User WHERE User_ID=A.Last_Updated_by)AS CreatedBy,ISNULL((SELECT COUNT(*) FROM TBL_Product_MSL_Group_Items WHERE PG_ID=A.PG_ID),0)AS TotalItems,ISNULL((SELECT COUNT(DISTINCT CAST(Customer_ID as varchar(100)) + '$'+ CAST(Site_USe_ID  AS Varchar(100))) FROM TBL_Customer_MSL_Group WHERE PG_ID=A.PG_ID),0)AS TotalCustomers, ISNULL((SELECT Top 1 Organization_id FROM TBL_Product_MSL_Group_Items WHERE PG_ID=A.PG_ID),'0')AS OrgID FROM TBL_Product_MSL_Group AS A  ORDER By A.PG_ID"
            Else
                QueryString = "SELECT A.PG_ID AS PGID, A.Description,A.Last_Updated_On AS CreatedAt,(SELECT Username FROM TBL_User WHERE User_ID=A.Last_Updated_by)AS CreatedBy,ISNULL((SELECT COUNT(*) FROM TBL_Product_MSL_Group_Items WHERE PG_ID=A.PG_ID),0)AS TotalItems,ISNULL((SELECT COUNT(DISTINCT CAST(Customer_ID as varchar(100)) + '$'+ CAST(Site_USe_ID  AS Varchar(100))) FROM TBL_Customer_MSL_Group WHERE PG_ID=A.PG_ID),0)AS TotalCustomers, ISNULL((SELECT Top 1 Organization_id FROM TBL_Product_MSL_Group_Items WHERE PG_ID=A.PG_ID),'0')AS OrgID FROM TBL_Product_MSL_Group AS A WHERE A.Org_ID=@OrgID  ORDER By A.PG_ID"
            End If
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")
            GetProductMSLGroup = MsgDs.Tables(0)
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "740243"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetProductGroup(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT A.PG_ID AS PGID, A.Description,A.Last_Updated_On AS CreatedAt,(SELECT Username FROM TBL_User WHERE User_ID=A.Last_Updated_by)AS CreatedBy,ISNULL((SELECT COUNT(*) FROM TBL_Product_Group_Items WHERE PG_ID=A.PG_ID),0)AS TotalItems,ISNULL((SELECT Top 1 Organization_id FROM TBL_Product_Group_Items WHERE PG_ID=A.PG_ID),'0')AS OrgID FROM TBL_Product_Group AS A ORDER By A.PG_ID")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")
            GetProductGroup = MsgDs.Tables(0)
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "740243"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function


    Public Function InsertProductMSLGroup(ByVal OrgId As String, ByVal ItemId As Integer, ByVal ItemCode As String, ByVal GroupId As Integer, ByVal GroupName As String, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CreatedBy As Integer, ByRef IsDeleted As Boolean, Mode As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Dim tran As SqlTransaction = Nothing
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            tran = objSQLConn.BeginTransaction()

            Dim QueryString As String = ""
            If IsDeleted = False Then
                QueryString = "DELETE FROM TBL_Product_MSL_Group_Items WHERE PG_ID=@PGID"
                objSQLCmd = New SqlCommand(QueryString, objSQLConn)
                objSQLCmd.CommandType = CommandType.Text
                objSQLCmd.Parameters.Add("@PGId", SqlDbType.Int).Value = GroupId
                objSQLCmd.Transaction = tran
                objSQLCmd.ExecuteNonQuery()
                objSQLCmd.Dispose()
                IsDeleted = True
            End If


            QueryString = "app_InsertProductMSLGroup"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@Inv_ItemID", SqlDbType.Int).Value = ItemId
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 50).Value = ItemCode
            objSQLCmd.Parameters.Add("@OrganizationID", SqlDbType.VarChar, 20).Value = OrgId
            objSQLCmd.Parameters.Add("@GroupId", SqlDbType.Int).Value = GroupId
            objSQLCmd.Parameters.Add("@GroupName", SqlDbType.VarChar, 250).Value = GroupName
            objSQLCmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = CreatedBy
            objSQLCmd.Parameters.Add("@Mode", SqlDbType.VarChar, 50).Value = Mode
            objSQLCmd.Transaction = tran
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()




            success = True

            tran.Commit()
        Catch ex As Exception
            Err_No = "44094"
            Err_Desc = ex.Message

            tran.Rollback()
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
            tran.Dispose()
        End Try
        Return success
    End Function


    Public Function InsertCustomerMSLGroup(ByVal OrgId As String, ByVal CustomerId As Integer, SiteID As String, ByVal CustomerNo As String, ByVal GroupId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CreatedBy As Integer, ByRef IsDeleted As Boolean, Mode As String) As Boolean


        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Dim tran As SqlTransaction = Nothing
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            tran = objSQLConn.BeginTransaction()
            Dim QueryString As String = ""
            If IsDeleted = False Then
                QueryString = "DELETE FROM TBL_Customer_MSL_Group WHERE PG_ID=@PGID"
                objSQLCmd = New SqlCommand(QueryString, objSQLConn)
                objSQLCmd.CommandType = CommandType.Text
                objSQLCmd.Parameters.Add("@PGId", SqlDbType.Int).Value = GroupId
                objSQLCmd.Transaction = tran
                objSQLCmd.ExecuteNonQuery()
                objSQLCmd.Dispose()
                IsDeleted = True
            End If


            QueryString = "app_InsertCustomerMSLGroup"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = CustomerId
            objSQLCmd.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 50).Value = CustomerNo
            objSQLCmd.Parameters.Add("@SiteID", SqlDbType.VarChar, 50).Value = SiteID
            objSQLCmd.Parameters.Add("@OrganizationID", SqlDbType.VarChar, 20).Value = OrgId
            objSQLCmd.Parameters.Add("@GroupId", SqlDbType.Int).Value = GroupId
            objSQLCmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = CreatedBy
            objSQLCmd.Parameters.Add("@Mode", SqlDbType.VarChar, 20).Value = Mode
            objSQLCmd.Transaction = tran
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()





            success = True

            tran.Commit()
        Catch ex As Exception
            Err_No = "44094"
            Err_Desc = ex.Message

            tran.Rollback()
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
            tran.Dispose()
        End Try
        Return success


    End Function

    Public Function InsertCustomerVanMap(CallFrom As String, ByVal OrgId As String, ByVal CustomerId As Integer, SiteID As String, ByVal CustomerNo As String, ByVal SId As String, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CreatedBy As Integer, ByRef IsDeleted As Boolean, Mode As String, VanList As String) As Boolean


        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Dim tran As SqlTransaction = Nothing
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            tran = objSQLConn.BeginTransaction()
            Dim QueryString As String = ""
            If IsDeleted = False Then
                QueryString = "DELETE FROM TBL_Customer_Van_Map  WHERE Van_Org_ID In(SELECT  Value FROM dbo.Split1(0,@VanList ,',') WHERE Value <> '' AND VALUE IS NOT NULL )"
                objSQLCmd = New SqlCommand(QueryString, objSQLConn)
                objSQLCmd.CommandType = CommandType.Text
                objSQLCmd.Parameters.Add("@VanList", SqlDbType.VarChar, 10000).Value = VanList
                objSQLCmd.Transaction = tran
                objSQLCmd.ExecuteNonQuery()
                objSQLCmd.Dispose()
                IsDeleted = True
            End If


            QueryString = "app_InsertCustomerVanMap"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = CustomerId
            objSQLCmd.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 50).Value = CustomerNo
            objSQLCmd.Parameters.Add("@SiteID", SqlDbType.VarChar, 50).Value = SiteID
            objSQLCmd.Parameters.Add("@OrganizationID", SqlDbType.VarChar, 20).Value = OrgId
            objSQLCmd.Parameters.Add("@VanId", SqlDbType.VarChar, 100).Value = SId
            objSQLCmd.Parameters.Add("@Mode", SqlDbType.VarChar, 20).Value = Mode
            objSQLCmd.Parameters.Add("@CallFrom", SqlDbType.VarChar, 20).Value = CallFrom
            objSQLCmd.Transaction = tran
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()





            success = True

            tran.Commit()
        Catch ex As Exception
            Err_No = "44094"
            Err_Desc = ex.Message

            tran.Rollback()
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
            tran.Dispose()
        End Try
        Return success


    End Function
    Public Function InsertProductGroup(ByVal OrgId As String, ByVal ItemId As Integer, ByVal ItemCode As String, ByVal GroupId As Integer, ByVal GroupName As String, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CreatedBy As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_InsertProductGroup"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@Inv_ItemID", SqlDbType.Int).Value = ItemId
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 50).Value = ItemCode
            objSQLCmd.Parameters.Add("@OrganizationID", SqlDbType.VarChar, 20).Value = OrgId
            objSQLCmd.Parameters.Add("@GroupId", SqlDbType.Int).Value = GroupId
            objSQLCmd.Parameters.Add("@GroupName", SqlDbType.VarChar, 250).Value = GroupName
            objSQLCmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = CreatedBy

            objSQLCmd.ExecuteNonQuery()
            success = True
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "44094"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
    Public Function InsertFSRProduct(ByVal InsertMode As String, ByVal OrgId As String, ByVal ItemId As Integer, ByVal ItemCode As String, ByVal VanId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemList As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            If InsertMode = "ALL" Then
                Dim QueryString As String = "app_InsertFSRProduct"
                objSQLCmd = New SqlCommand(QueryString, objSQLConn)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.Parameters.Add("@Inv_ItemID", SqlDbType.Int).Value = ItemId
                objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 50).Value = ItemCode
                objSQLCmd.Parameters.Add("@OrganizationID", SqlDbType.VarChar, 20).Value = OrgId
                objSQLCmd.Parameters.Add("@VanId", SqlDbType.Int).Value = VanId
                objSQLCmd.Parameters.Add("@InsertMode", SqlDbType.VarChar, 20).Value = InsertMode
                objSQLCmd.ExecuteNonQuery()
                objSQLCmd.Dispose()
            Else
                Dim QueryString As String = "Insert into TBL_Product_FSR(Inventory_Item_Id,Organization_Id,SalesRep_Id)(SELECT Inventory_Item_ID,	Organization_ID,@vanID FROM TBL_Product WHERE Organization_Id=@OrgID AND CAST(Inventory_Item_ID AS VARCHAR)   IN  (SELECT CAST(Item AS Varchar)   FROM [dbo].[SplitQuotedString]('" & ItemList & "')))"
                objSQLCmd = New SqlCommand(QueryString, objSQLConn)
                'objSQLCmd.CommandType = CommandType.Text
                objSQLCmd.Parameters.Add("@OrgId", SqlDbType.Int).Value = OrgId
                objSQLCmd.Parameters.Add("@VanId", SqlDbType.Int).Value = VanId
                'objSQLCmd.Parameters.Add("@ListItemId", SqlDbType.VarChar, 8000).Value = ItemList
                objSQLCmd.ExecuteNonQuery()

            End If
            success = True
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "44093"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
    Public Function GetSelectedProductGroupFSR(ByVal OrgID As String, ByVal SalesRepID As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_GetSelectedProductGroupFSR"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@OrgId", SqlDbType.VarChar, 50).Value = OrgID
            objSQLCmd.Parameters.Add("@SalesRepId", SqlDbType.Int).Value = SalesRepID
            'objSQLCmd.Parameters.Add("@GroupId", SqlDbType.VarChar, 50).Value = groupId
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")

            GetSelectedProductGroupFSR = MsgDs.Tables(0)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "34099"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    Public Function GetSelectedCollectionGroupFSR(ByVal OrgID As String, ByVal SalesRepID As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_GetAssignedCollectionGroupFSR"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@OrgId", SqlDbType.VarChar, 50).Value = OrgID
            objSQLCmd.Parameters.Add("@SalesRepId", SqlDbType.Int).Value = SalesRepID
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")

            GetSelectedCollectionGroupFSR = MsgDs.Tables(0)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "34099"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    Public Function GetAssignedMSLCustomer(ByVal OrgID As String, ByVal PGID As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_GetAssignedMSLCustomer"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@OrgId", SqlDbType.VarChar, 50).Value = OrgID
            objSQLCmd.Parameters.Add("@PGID", SqlDbType.VarChar, 50).Value = PGID
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")

            GetAssignedMSLCustomer = MsgDs.Tables(0)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "34099"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    Public Function GetAssignedCustomerByVan(ByVal OrgID As String, ByVal SID As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_GetAssignedCustomerByVan"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@OrgId", SqlDbType.VarChar, 50).Value = OrgID
            objSQLCmd.Parameters.Add("@SID", SqlDbType.VarChar, 50).Value = SID
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")

            GetAssignedCustomerByVan = MsgDs.Tables(0)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "34099"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function

    Public Function GetDefaultCollectionGroupFSR(ByVal OrgId As String, ByVal SalesRepId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_GetDefaultCollectionGroupFSR"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 20).Value = OrgId
            objSQLCmd.Parameters.Add("@SalesRepId", SqlDbType.Int).Value = SalesRepId
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")

            GetDefaultCollectionGroupFSR = MsgDs.Tables(0)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "14093"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    Public Function GetDefaultProductGroupFSR(ByVal OrgId As String, ByVal SalesRepId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_GetDefaultProductGroupFSR"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 20).Value = OrgId
            'objSQLCmd.Parameters.Add("@GroupId", SqlDbType.VarChar, 20).Value = GroupId
            objSQLCmd.Parameters.Add("@SalesRepId", SqlDbType.Int).Value = SalesRepId
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")

            GetDefaultProductGroupFSR = MsgDs.Tables(0)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "14093"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function

    Public Function GetAvailCustomersByVan(ByVal OrgId As String, ByVal SId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_GetAvailCustomersByVan"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 20).Value = OrgId
            objSQLCmd.Parameters.Add("@SId", SqlDbType.VarChar, 20).Value = SId
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")

            GetAvailCustomersByVan = MsgDs.Tables(0)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "14093"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function

    Public Function GetAvailCustomersByMSL(ByVal OrgId As String, ByVal PGId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_GetAvailCustomersByMSL"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 20).Value = OrgId
            objSQLCmd.Parameters.Add("@PGId", SqlDbType.VarChar, 20).Value = PGId
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")

            GetAvailCustomersByMSL = MsgDs.Tables(0)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "14093"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    Public Function GetSelectedProductMSLGroup(ByVal OrgID As String, ByVal groupId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_GetSelectedProductMSLGroup"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@OrgId", SqlDbType.VarChar, 50).Value = OrgID
            objSQLCmd.Parameters.Add("@GroupId", SqlDbType.Int).Value = groupId

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")

            GetSelectedProductMSLGroup = MsgDs.Tables(0)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "34094"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    Public Function GetSelectedProductGroup(ByVal OrgID As String, ByVal groupId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_GetSelectedProductGroup"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@OrgId", SqlDbType.VarChar, 50).Value = OrgID
            objSQLCmd.Parameters.Add("@GroupId", SqlDbType.Int).Value = groupId

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")

            GetSelectedProductGroup = MsgDs.Tables(0)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "34094"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    Public Function GetSelectedFSRProduct(ByVal OrgID As String, ByVal VanId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_GetSelectedFSRProductList"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@OrgId", SqlDbType.VarChar, 50).Value = OrgID
            objSQLCmd.Parameters.Add("@VanId", SqlDbType.Int).Value = VanId

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")

            GetSelectedFSRProduct = MsgDs.Tables(0)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "34093"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    Public Function GetDefaultProductGroup(ByVal OrgId As String, ByVal GroupId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_GetDefaultProductGroup"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 20).Value = OrgId
            objSQLCmd.Parameters.Add("@GroupId", SqlDbType.Int).Value = GroupId

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")

            GetDefaultProductGroup = MsgDs.Tables(0)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "14093"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function

    Public Function GetDefaultProductMSLGroup(ByVal OrgId As String, ByVal GroupId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_GetDefaultProductMSLGroup"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 20).Value = OrgId
            objSQLCmd.Parameters.Add("@GroupId", SqlDbType.Int).Value = GroupId

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")

            GetDefaultProductMSLGroup = MsgDs.Tables(0)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "14093"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function

    Public Function GetDefaultProductFSR(ByVal OrgId As String, ByVal VanId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_GetDefaultProductFSRList"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 20).Value = OrgId
            objSQLCmd.Parameters.Add("@VanId", SqlDbType.Int).Value = VanId
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")

            GetDefaultProductFSR = MsgDs.Tables(0)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "14092"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function

    Public Function DeleteProductMSLGroup(ByVal DeleteMode As String, ByVal OrgId As String, ByVal ItemId As Integer, ByVal ItemCode As String, ByVal GroupId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Groupname As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_DeleteProductMSLGroup"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@Inv_ItemID", SqlDbType.Int).Value = ItemId
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 50).Value = ItemCode
            objSQLCmd.Parameters.Add("@OrganizationID", SqlDbType.VarChar, 20).Value = OrgId
            objSQLCmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = GroupId
            objSQLCmd.Parameters.Add("@DeleteMode", SqlDbType.VarChar, 20).Value = DeleteMode
            objSQLCmd.Parameters.Add("@GroupName", SqlDbType.VarChar, 20).Value = Groupname
            objSQLCmd.ExecuteNonQuery()
            success = True
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "54094"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
    Public Function DeleteProductGroup(ByVal DeleteMode As String, ByVal OrgId As String, ByVal ItemId As Integer, ByVal ItemCode As String, ByVal GroupId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Groupname As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = "app_DeleteProductGroup"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@Inv_ItemID", SqlDbType.Int).Value = ItemId
            objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 50).Value = ItemCode
            objSQLCmd.Parameters.Add("@OrganizationID", SqlDbType.VarChar, 20).Value = OrgId
            objSQLCmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = GroupId
            objSQLCmd.Parameters.Add("@DeleteMode", SqlDbType.VarChar, 20).Value = DeleteMode
            objSQLCmd.Parameters.Add("@GroupName", SqlDbType.VarChar, 20).Value = Groupname
            objSQLCmd.ExecuteNonQuery()
            success = True
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "54094"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
    Public Function DeleteFSRProduct(ByVal DeleteMode As String, ByVal OrgId As String, ByVal ItemId As Integer, ByVal ItemCode As String, ByVal VanId As Integer, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemList As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            If DeleteMode = "ALL" Then
                Dim QueryString As String = "app_DeleteFSRProduct"
                objSQLCmd = New SqlCommand(QueryString, objSQLConn)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.Parameters.Add("@Inv_ItemID", SqlDbType.Int).Value = ItemId
                objSQLCmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 50).Value = ItemCode
                objSQLCmd.Parameters.Add("@OrganizationID", SqlDbType.VarChar, 20).Value = OrgId
                objSQLCmd.Parameters.Add("@VanID", SqlDbType.Int).Value = VanId
                objSQLCmd.Parameters.Add("@DeleteMode", SqlDbType.VarChar, 20).Value = DeleteMode
                objSQLCmd.ExecuteNonQuery()
            Else
                Dim QueryString As String = "DELETE FROM TBL_Product_FSR WHERE CAST(Inventory_Item_ID AS VARCHAR)   IN  (SELECT CAST(Item AS Varchar)   FROM [dbo].[SplitQuotedString]('" & ItemList & "')) AND Organization_Id=@OrganizationId AND SalesRep_Id=@VanId"
                objSQLCmd = New SqlCommand(QueryString, objSQLConn)
                objSQLCmd.Parameters.Add("@OrganizationID", SqlDbType.VarChar, 20).Value = OrgId
                objSQLCmd.Parameters.Add("@VanID", SqlDbType.Int).Value = VanId
                objSQLCmd.ExecuteNonQuery()
            End If
            success = True
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "54093"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function

    Public Function CheckDuplicateMSLGroup(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal GroupName As String) As DataRow
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT A.PG_ID AS PGID, A.Description FROM TBL_Product_MSL_Group AS A WHERE  A.Description=@GroupName")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@Groupname", SqlDbType.VarChar, 250).Value = GroupName
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")
            If MsgDs.Tables(0).Rows.Count > 0 Then
                CheckDuplicateMSLGroup = MsgDs.Tables(0).Rows(0)
            Else
                CheckDuplicateMSLGroup = Nothing
            End If
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "740223"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function CheckDuplicateGroup(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal GroupName As String) As DataRow
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT A.PG_ID AS PGID, A.Description FROM TBL_Product_Group AS A WHERE  A.Description=@GroupName")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@Groupname", SqlDbType.VarChar, 250).Value = GroupName
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")
            If MsgDs.Tables(0).Rows.Count > 0 Then
                CheckDuplicateGroup = MsgDs.Tables(0).Rows(0)
            Else
                CheckDuplicateGroup = Nothing
            End If
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "740223"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function SaveSupplierProdCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SupplierCode As String, ByVal Itemcode As String, ByVal ProdCode As String, ByVal Userid As String, ByVal Org As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_UpdatSupplierProdCode", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@SupplierCode", SqlDbType.VarChar))
            objSQLCmd.Parameters("@SupplierCode").Value = SupplierCode
            objSQLCmd.Parameters.Add(New SqlParameter("@ItemCode", SqlDbType.VarChar))
            objSQLCmd.Parameters("@ItemCode").Value = Itemcode
            objSQLCmd.Parameters.Add(New SqlParameter("@Supplier_Item_Code", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Supplier_Item_Code").Value = ProdCode
            objSQLCmd.Parameters.Add(New SqlParameter("@Created_By", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Created_By").Value = Userid
            objSQLCmd.Parameters.Add(New SqlParameter("@Org", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Org").Value = Org

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
    Public Function DeleteSupplierProdCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SupplierCode As String, ByVal Itemcode As String, ByVal OrgID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_DeleteSupplierCode", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@SupplierCode", SqlDbType.VarChar))
            objSQLCmd.Parameters("@SupplierCode").Value = SupplierCode
            objSQLCmd.Parameters.Add(New SqlParameter("@ItemCode", SqlDbType.VarChar))
            objSQLCmd.Parameters("@ItemCode").Value = Itemcode
            objSQLCmd.Parameters.Add(New SqlParameter("@Org", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Org").Value = OrgID
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

    Public Function GetSupplierProdCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Criteria As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable

        Try

            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_GetSupplierProductCode", objSQLConn)
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
        GetSupplierProdCode = dtDivConfig
    End Function
    Function ValidItemandAgency(ByVal OrgID As String, ByVal SupplierCode As String, ByVal Itemcode As String)
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetval As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT * from tbl_product where item_code='" & Itemcode & "' and isnull(agency,'')='" & SupplierCode & "' and Organization_ID='" & OrgID & "'")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            Dim MsgDs As New DataTable
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs)
            If MsgDs.Rows.Count > 0 Then
                bRetval = True
            End If
            objSQLCmd.Dispose()

        Catch ex As Exception

        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetval
    End Function
    Public Function CheckCustDiscountFlag(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select COUNT(*) FROM TBL_App_Control WHERE Control_Key='ENABLE_CUST_DISCOUNT' AND Control_Value='Y' ")
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
    Public Function CheckDiscountPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal PlanName As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select COUNT(*) FROM TBL_Discount_Plan WHERE Description=@PlanName and Organization_ID=@OrgID  ")
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

    Public Function SaveMultiTrxCategoryMap(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal Rule_ID As String, ByVal DtCategory As DataTable, CreatedBy As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection
            sQry = "delete from TBL_BNS_Multi_Trx_Category_Map where Rule_ID=@Rule_ID"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@Rule_ID", Rule_ID)
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()

            For Each dr As DataRow In DtCategory.Rows
                sQry = "INSERT INTO TBL_BNS_Multi_Trx_Category_Map (Rule_ID ,Category , Created_At,Created_By)VALUES(@Rule_ID,@Category, GETDATE(),@CreatedBy)"
                objSQLCmd = New SqlCommand(sQry, objSQLConn)
                objSQLCmd.Parameters.AddWithValue("@Rule_ID", Rule_ID)
                objSQLCmd.Parameters.AddWithValue("@Category", dr(0).ToString)
                objSQLCmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)

                objSQLCmd.ExecuteNonQuery()
                objSQLCmd.Dispose()
            Next

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

    Public Function SaveDiscountCategoryMap(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal PlanID As String, ByVal DtCategory As DataTable, CreatedBy As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection
            sQry = "delete from TBL_Discount_Category_Map where Discount_Plan_ID=@Discount_Plan_ID"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@Discount_Plan_ID", PlanID)
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()

            For Each dr As DataRow In DtCategory.Rows
                sQry = "INSERT INTO TBL_Discount_Category_Map (Discount_Plan_ID ,Category , Created_At,Created_By)VALUES(@Discount_Plan_ID,@Category, GETDATE(),@CreatedBy)"
                objSQLCmd = New SqlCommand(sQry, objSQLConn)
                objSQLCmd.Parameters.AddWithValue("@Discount_Plan_ID", PlanID)
                objSQLCmd.Parameters.AddWithValue("@Category", dr(0).ToString)
                objSQLCmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)

                objSQLCmd.ExecuteNonQuery()
                objSQLCmd.Dispose()
            Next

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

    Public Function SaveDiscountPlan(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal OrgId As String, ByVal PlanName As String, ByVal CreatedBy As Integer, ByRef PlanId As String, TransactionType As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "INSERT INTO TBL_Discount_Plan (Description ,Organization_ID , Created_At,Created_By,Transaction_Type)VALUES(@PlanName,@OrgId, GETDATE(),@CreatedBy,@Transaction_Type)"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@PlanName", PlanName)
            objSQLCmd.Parameters.AddWithValue("@OrgId", OrgId)
            objSQLCmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
            objSQLCmd.Parameters.AddWithValue("@Transaction_Type", TransactionType) 

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
    Public Function UpdateDiscountPlan(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal PlanId As String, ByVal PlanName As String, ByVal CreatedBy As Integer, TransactionType As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False


        Try
            'getting MSSQL DB connection.....
            'Insert TBL_BNS_promotion
            objSQLConn = _objDB.GetSQLConnection

            sQry = "UPDATE TBL_Discount_Plan  SET Description=@Description ,Last_Updated_At=GetDate(),Last_Updated_By=@CreatedBy,Transaction_Type=@Transaction_Type WHERE Discount_Plan_ID=@PlanID"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@PlanID", PlanId)
            objSQLCmd.Parameters.AddWithValue("@Description", PlanName)
            objSQLCmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
            objSQLCmd.Parameters.AddWithValue("@Transaction_Type", TransactionType)

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
    Public Function LoadDiscountPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UID As Integer, ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT DISTINCT A.Discount_Plan_ID,A.Description,A.Organization_ID AS OrgID,(SELECT Description FROM TBL_Org_CTL_H WHERE ORG_HE_ID =B.MAS_Org_ID) AS  OrgName,A.Last_Updated_At AS UpdatedAt ,(SELECT userName FROM TBL_user WHERE User_ID=A.Last_Updated_BY)AS UpdatedBy,(SELECT COUNT(DISTINCT Item_Code) FROM TBL_Discount WHERE Discount_Plan_ID=A.Discount_Plan_ID)AS TotItems,(SELECT COUNT(*) FROM TBL_Customer_Discount_Map  AS X INNER JOIN dbo.app_GetOrgCustomerShipAddress(A.Organization_ID)AS Y ON X.Customer_ID =Y.Customer_ID AND X.Site_Use_ID =Y.Site_Use_ID WHERE Discount_Plan_ID=A.Discount_Plan_ID)AS TotCustomers,isnull(Transaction_Type,'0')as Transaction_Type FROM TBL_Discount_Plan  AS A INNER JOIN  app_GetControlInfo(@UID) AS B ON A.Organization_Id=B.Mas_Org_ID ORDER BY A.Last_Updated_At DESC")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@UID", UID)
            ' objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")
            LoadDiscountPlan = MsgDs.Tables(0)
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "540243"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function LoadExportProductsTemplate(ByVal Org_ID As String, ByVal Item_code As String, ByVal Description As String, ByVal UOM As String, ByVal Agency As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Dim Ds As New DataSet
        Dim sQry As String

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            sQry = "app_ExportProductsTemplate"



            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@Org_ID", Org_ID)
            objSQLCmd.Parameters.AddWithValue("@Item_code", Item_code)
            objSQLCmd.Parameters.AddWithValue("@Description", Description)
            objSQLCmd.Parameters.AddWithValue("@UOM", UOM)
            objSQLCmd.Parameters.AddWithValue("@Agency", Agency)
            objSQLDa = New SqlDataAdapter(objSQLCmd)

            objSQLDa.Fill(Ds)
            objSQLCmd.Dispose()





            Return Ds
        Catch ex As Exception
            Throw ex
        Finally

            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return Ds
    End Function


    Public Function SaveProduct(ByRef Error_No As Long, ByRef Error_Desc As String, ProdCode As String, ProdName As String, Brand As String, Agency As String, UOM As String, Barcode As String, CostPrice As Decimal, Category As String, ProdNo As String, ItemSize As String, SubCategory As String, DefaultUOM As String, RestrictiveReturn As String, HasLots As String, AllowPriceChange As String, Inv_ID As Integer, Org_ID As String, Stock_UOM As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String


        Dim retVal As Boolean = False
        'getting MSSQL DB connection.....
        objSQLConn = _objDB.GetSQLConnection

        Try
            sQry = "usp_SaveProduct"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@Inv_ID", Inv_ID)
            objSQLCmd.Parameters.AddWithValue("@Org_ID", Org_ID)
            objSQLCmd.Parameters.AddWithValue("@ProdCode", ProdCode)
            objSQLCmd.Parameters.AddWithValue("@ProdNo", ProdNo)
            objSQLCmd.Parameters.AddWithValue("@ProdName", ProdName)
            objSQLCmd.Parameters.AddWithValue("@Brand", IIf(Brand = "", DBNull.Value, Brand))
            objSQLCmd.Parameters.AddWithValue("@UOM", UOM.ToUpper())
            objSQLCmd.Parameters.AddWithValue("@Agency", IIf(Agency = "", DBNull.Value, Agency))
            objSQLCmd.Parameters.AddWithValue("@Barcode", IIf(Barcode = "", DBNull.Value, Barcode))
            objSQLCmd.Parameters.AddWithValue("@CostPrice", IIf(CostPrice = vbEmpty, DBNull.Value, CostPrice))
            objSQLCmd.Parameters.AddWithValue("@Category", IIf(Category = "", DBNull.Value, Category))
            objSQLCmd.Parameters.AddWithValue("@ItemSize", IIf(ItemSize = "", DBNull.Value, ItemSize))
            objSQLCmd.Parameters.AddWithValue("@SubCategory", IIf(SubCategory = "", DBNull.Value, SubCategory))


            objSQLCmd.Parameters.AddWithValue("@DefaultUOM", DefaultUOM.ToUpper())
            objSQLCmd.Parameters.AddWithValue("@RestrictiveReturn", RestrictiveReturn)
            objSQLCmd.Parameters.AddWithValue("@HasLots", HasLots)
            objSQLCmd.Parameters.AddWithValue("@AllowPriceChange", AllowPriceChange)
            objSQLCmd.Parameters.AddWithValue("@StockUOM", Stock_UOM)




            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()

            retVal = True

        Catch ex As Exception

            Error_No = 13351
            Error_Desc = String.Format("Error while saving record: {0}", ex.Message)
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        SaveProduct = retVal
    End Function



    Public Function UpdateProduct(ByRef Error_No As Long, ByRef Error_Desc As String, ProdCode As String, ProdName As String, Brand As String, Agency As String, UOM As String, Barcode As String, CostPrice As Decimal, Category As String, ProdNo As String, ItemSize As String, SubCategory As String, DefaultUOM As String, RestrictiveReturn As String, HasLots As String, AllowPriceChange As String, Inv_ID As Integer, Org_ID As String, Stock_UOM As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String


        Dim retVal As Boolean = False
        'getting MSSQL DB connection.....
        objSQLConn = _objDB.GetSQLConnection

        Try
            sQry = "usp_UpdateProduct"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@Inv_ID", Inv_ID)
            objSQLCmd.Parameters.AddWithValue("@Org_ID", Org_ID)
            objSQLCmd.Parameters.AddWithValue("@ProdCode", ProdCode)
            objSQLCmd.Parameters.AddWithValue("@ProdNo", ProdNo)
            objSQLCmd.Parameters.AddWithValue("@ProdName", ProdName)
            objSQLCmd.Parameters.AddWithValue("@Brand", IIf(Brand = "", DBNull.Value, Brand))
            objSQLCmd.Parameters.AddWithValue("@UOM", UOM.ToUpper())
            objSQLCmd.Parameters.AddWithValue("@Agency", IIf(Agency = "", DBNull.Value, Agency))
            objSQLCmd.Parameters.AddWithValue("@Barcode", IIf(Barcode = "", DBNull.Value, Barcode))
            objSQLCmd.Parameters.AddWithValue("@CostPrice", IIf(CostPrice = vbEmpty, DBNull.Value, CostPrice))
            objSQLCmd.Parameters.AddWithValue("@Category", IIf(Category = "", DBNull.Value, Category))
            objSQLCmd.Parameters.AddWithValue("@ItemSize", IIf(ItemSize = "", DBNull.Value, ItemSize))
            objSQLCmd.Parameters.AddWithValue("@SubCategory", IIf(SubCategory = "", DBNull.Value, SubCategory))


            objSQLCmd.Parameters.AddWithValue("@DefaultUOM", DefaultUOM.ToUpper())
            objSQLCmd.Parameters.AddWithValue("@RestrictiveReturn", RestrictiveReturn)
            objSQLCmd.Parameters.AddWithValue("@HasLots", HasLots)
            objSQLCmd.Parameters.AddWithValue("@AllowPriceChange", AllowPriceChange)
            objSQLCmd.Parameters.AddWithValue("@StockUOM", Stock_UOM)
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()

            retVal = True

        Catch ex As Exception

            Error_No = 23155
            Error_Desc = String.Format("Error while updating record: {0}", ex.Message)
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        UpdateProduct = retVal
    End Function

    Public Function SaveProductNew(ByRef Error_No As Long, ByRef Error_Desc As String, ProdCode As String, ProdName As String, Brand As String, Agency As String, UOM As String, Barcode As String, CostPrice As Decimal, Category As String, ProdNo As String, ItemSize As String, SubCategory As String, DefaultUOM As String, RestrictiveReturn As String, HasLots As String, AllowPriceChange As String, Inv_ID As Integer, Org_ID As String, dtUOM As DataTable, StockUOM As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim sqlTran As SqlTransaction
        Dim retVal As Boolean = False
        'getting MSSQL DB connection.....
        objSQLConn = _objDB.GetSQLConnection
        Dim InvID As Integer = 0

        Try


            objSQLCmd = New SqlCommand(sQry, objSQLConn)

            sqlTran = objSQLConn.BeginTransaction()
            objSQLCmd = objSQLConn.CreateCommand()
            objSQLCmd.Connection = objSQLConn
            objSQLCmd.Transaction = sqlTran

            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandText = "usp_SaveProduct_v2"
            objSQLCmd.Parameters.AddWithValue("@Inv_ID", Inv_ID)
            objSQLCmd.Parameters.AddWithValue("@Org_ID", Org_ID)
            objSQLCmd.Parameters.AddWithValue("@ProdCode", ProdCode)
            objSQLCmd.Parameters.AddWithValue("@ProdNo", ProdNo)
            objSQLCmd.Parameters.AddWithValue("@ProdName", ProdName)
            objSQLCmd.Parameters.AddWithValue("@Brand", IIf(Brand = "", DBNull.Value, Brand))
            objSQLCmd.Parameters.AddWithValue("@UOM", UOM.ToUpper())
            objSQLCmd.Parameters.AddWithValue("@Agency", IIf(Agency = "", DBNull.Value, Agency))
            objSQLCmd.Parameters.AddWithValue("@Barcode", IIf(Barcode = "", DBNull.Value, Barcode))
            objSQLCmd.Parameters.AddWithValue("@CostPrice", IIf(CostPrice = vbEmpty, DBNull.Value, CostPrice))
            objSQLCmd.Parameters.AddWithValue("@Category", IIf(Category = "", DBNull.Value, Category))
            objSQLCmd.Parameters.AddWithValue("@ItemSize", IIf(ItemSize = "", DBNull.Value, ItemSize))
            objSQLCmd.Parameters.AddWithValue("@SubCategory", IIf(SubCategory = "", DBNull.Value, SubCategory))


            objSQLCmd.Parameters.AddWithValue("@DefaultUOM", IIf(DefaultUOM = "0", DBNull.Value, DefaultUOM.ToUpper()))
            objSQLCmd.Parameters.AddWithValue("@RestrictiveReturn", RestrictiveReturn)
            objSQLCmd.Parameters.AddWithValue("@HasLots", HasLots)
            objSQLCmd.Parameters.AddWithValue("@AllowPriceChange", AllowPriceChange)
            objSQLCmd.Parameters.AddWithValue("@StockUOM", IIf(StockUOM = "0", DBNull.Value, StockUOM.ToUpper()))

            Dim dRead As SqlDataReader = objSQLCmd.ExecuteReader()

            If (dRead.Read()) Then
                InvID = dRead(0)
            End If

            dRead.Close()

            If InvID > 0 Then

                For Each droW As DataRow In dtUOM.Rows
                    objSQLCmd.Parameters.Clear()
                    objSQLCmd.CommandText = "app_InsertItemUOM"
                    objSQLCmd.CommandType = CommandType.StoredProcedure
                    objSQLCmd.Parameters.Add(New SqlParameter("@Item_UOM_ID", SqlDbType.SmallInt))
                    objSQLCmd.Parameters("@Item_UOM_ID").Value = 0
                    objSQLCmd.Parameters.Add(New SqlParameter("@Item_Code", SqlDbType.VarChar, 40))
                    objSQLCmd.Parameters("@Item_Code").Value = ProdCode
                    objSQLCmd.Parameters.Add(New SqlParameter("@Organization_ID", SqlDbType.VarChar, 100))
                    objSQLCmd.Parameters("@Organization_ID").Value = Org_ID
                    objSQLCmd.Parameters.Add(New SqlParameter("@Item_UOM", SqlDbType.VarChar, 100))
                    objSQLCmd.Parameters("@Item_UOM").Value = droW("Item_UOM").ToUpper()
                    objSQLCmd.Parameters.Add(New SqlParameter("@Conversion", SqlDbType.Decimal))
                    objSQLCmd.Parameters("@Conversion").Value = droW("Conversion")
                    objSQLCmd.Parameters.Add(New SqlParameter("@Is_Sellable", SqlDbType.VarChar))
                    objSQLCmd.Parameters("@Is_Sellable").Value = IIf(droW("SellableText") = "Yes", "Y", "N")
                    objSQLCmd.ExecuteNonQuery()
                Next
            End If


            sqlTran.Commit()
            retVal = True

        Catch ex As Exception
            If sqlTran IsNot Nothing Then sqlTran.Rollback()
            Error_No = 13351
            Error_Desc = String.Format("Error while saving record: {0}", ex.Message)
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        SaveProductNew = retVal
    End Function

    Public Function UpdateProductNew(ByRef Error_No As Long, ByRef Error_Desc As String, ProdCode As String, ProdName As String, Brand As String, Agency As String, UOM As String, Barcode As String, CostPrice As Decimal, Category As String, ProdNo As String, ItemSize As String, SubCategory As String, DefaultUOM As String, RestrictiveReturn As String, HasLots As String, AllowPriceChange As String, Inv_ID As Integer, Org_ID As String, dtUOM As DataTable, StockUOM As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String

        Dim sqlTran As SqlTransaction
        Dim retVal As Boolean = False
        'getting MSSQL DB connection.....
        objSQLConn = _objDB.GetSQLConnection

        Try

            sqlTran = objSQLConn.BeginTransaction()
            objSQLCmd = objSQLConn.CreateCommand()
            objSQLCmd.Connection = objSQLConn
            objSQLCmd.Transaction = sqlTran


            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandText = "usp_UpdateProduct_v2"

            objSQLCmd.Parameters.AddWithValue("@Inv_ID", Inv_ID)
            objSQLCmd.Parameters.AddWithValue("@Org_ID", Org_ID)
            objSQLCmd.Parameters.AddWithValue("@ProdCode", ProdCode)
            objSQLCmd.Parameters.AddWithValue("@ProdNo", ProdNo)
            objSQLCmd.Parameters.AddWithValue("@ProdName", ProdName)
            objSQLCmd.Parameters.AddWithValue("@Brand", IIf(Brand = "", DBNull.Value, Brand))
            objSQLCmd.Parameters.AddWithValue("@UOM", UOM.ToUpper())
            objSQLCmd.Parameters.AddWithValue("@Agency", IIf(Agency = "", DBNull.Value, Agency))
            objSQLCmd.Parameters.AddWithValue("@Barcode", IIf(Barcode = "", DBNull.Value, Barcode))
            objSQLCmd.Parameters.AddWithValue("@CostPrice", IIf(CostPrice = vbEmpty, DBNull.Value, CostPrice))
            objSQLCmd.Parameters.AddWithValue("@Category", IIf(Category = "", DBNull.Value, Category))
            objSQLCmd.Parameters.AddWithValue("@ItemSize", IIf(ItemSize = "", DBNull.Value, ItemSize))
            objSQLCmd.Parameters.AddWithValue("@SubCategory", IIf(SubCategory = "", DBNull.Value, SubCategory))


            objSQLCmd.Parameters.AddWithValue("@DefaultUOM", IIf(DefaultUOM = "0", DBNull.Value, DefaultUOM.ToUpper()))
            objSQLCmd.Parameters.AddWithValue("@RestrictiveReturn", RestrictiveReturn)
            objSQLCmd.Parameters.AddWithValue("@HasLots", HasLots)
            objSQLCmd.Parameters.AddWithValue("@AllowPriceChange", AllowPriceChange)
            objSQLCmd.Parameters.AddWithValue("@StockUOM", IIf(StockUOM = "0", DBNull.Value, StockUOM.ToUpper()))

            If objSQLCmd.ExecuteNonQuery() > 0 Then

                '' Clearing all existing UOMS


                objSQLCmd.Parameters.Clear()
                objSQLCmd.CommandText = "DELETE FROM TBL_Item_UOM WHERE Item_Code = @Item_Code AND Organization_ID = @Organization_ID"
                objSQLCmd.CommandType = CommandType.Text
                objSQLCmd.Parameters.Add(New SqlParameter("@Item_Code", SqlDbType.VarChar, 40))
                objSQLCmd.Parameters("@Item_Code").Value = ProdCode
                objSQLCmd.Parameters.Add(New SqlParameter("@Organization_ID", SqlDbType.VarChar, 100))
                objSQLCmd.Parameters("@Organization_ID").Value = Org_ID
                objSQLCmd.ExecuteNonQuery()

                '' INSERTING THE ALL

                For Each droW As DataRow In dtUOM.Rows
                    objSQLCmd.Parameters.Clear()
                    objSQLCmd.CommandText = "app_InsertItemUOM"
                    objSQLCmd.CommandType = CommandType.StoredProcedure
                    objSQLCmd.Parameters.Add(New SqlParameter("@Item_UOM_ID", SqlDbType.SmallInt))
                    objSQLCmd.Parameters("@Item_UOM_ID").Value = 0
                    objSQLCmd.Parameters.Add(New SqlParameter("@Item_Code", SqlDbType.VarChar, 40))
                    objSQLCmd.Parameters("@Item_Code").Value = ProdCode
                    objSQLCmd.Parameters.Add(New SqlParameter("@Organization_ID", SqlDbType.VarChar, 100))
                    objSQLCmd.Parameters("@Organization_ID").Value = Org_ID
                    objSQLCmd.Parameters.Add(New SqlParameter("@Item_UOM", SqlDbType.VarChar, 100))
                    objSQLCmd.Parameters("@Item_UOM").Value = droW("Item_UOM").ToUpper()
                    objSQLCmd.Parameters.Add(New SqlParameter("@Conversion", SqlDbType.Decimal))
                    objSQLCmd.Parameters("@Conversion").Value = droW("Conversion")
                    objSQLCmd.Parameters.Add(New SqlParameter("@Is_Sellable", SqlDbType.VarChar))
                    objSQLCmd.Parameters("@Is_Sellable").Value = IIf(droW("SellableText") = "Yes", "Y", "N")
                    objSQLCmd.ExecuteNonQuery()
                Next
                sqlTran.Commit()
                retVal = True

            End If


            

        Catch ex As Exception

            Error_No = 23155
            Error_Desc = String.Format("Error while updating record: {0}", ex.Message)
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        UpdateProductNew = retVal
    End Function


    Public Function DeleteProduct(ByRef Error_No As Long, ByRef Error_Desc As String, Inv_ID As Integer, Org_ID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim iRowsAffected As Integer = 0
        Dim retVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection

            sQry = "usp_DeleteProduct"


            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@Inv_ID", Inv_ID)
            objSQLCmd.Parameters.AddWithValue("@Org_ID", Org_ID)
            objSQLCmd.ExecuteNonQuery()

            retVal = True

        Catch ex As Exception

            Error_No = 61140
            Error_Desc = String.Format("Error while deleting record: {0}", ex.Message)
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return retVal
    End Function


    Public Function ActivateProduct(ByRef Error_No As Long, ByRef Error_Desc As String, Inv_ID As Integer, Org_ID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim iRowsAffected As Integer = 0
        Dim retVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection

            sQry = "usp_ActivateProduct"


            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@Inv_ID", Inv_ID)
            objSQLCmd.Parameters.AddWithValue("@Org_ID", Org_ID)
            objSQLCmd.ExecuteNonQuery()

            retVal = True

        Catch ex As Exception

            Error_No = 61140
            Error_Desc = String.Format("Error while deleting record: {0}", ex.Message)
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return retVal
    End Function
    Public Function InsertItemUOM(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Item_UOM_ID As Integer, ByVal Item_Code As String, ByVal Organization_ID As String, ByVal Item_UOM As String, ByVal Conversion As Decimal, Is_Sellable As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_InsertItemUOM", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@Item_UOM_ID", SqlDbType.SmallInt))
            objSQLCmd.Parameters("@Item_UOM_ID").Value = Item_UOM_ID
            objSQLCmd.Parameters.Add(New SqlParameter("@Item_Code", SqlDbType.VarChar, 40))
            objSQLCmd.Parameters("@Item_Code").Value = Item_Code
            objSQLCmd.Parameters.Add(New SqlParameter("@Organization_ID", SqlDbType.VarChar, 100))
            objSQLCmd.Parameters("@Organization_ID").Value = Organization_ID
            objSQLCmd.Parameters.Add(New SqlParameter("@Item_UOM", SqlDbType.VarChar, 100))
            objSQLCmd.Parameters("@Item_UOM").Value = Item_UOM.ToUpper()
            objSQLCmd.Parameters.Add(New SqlParameter("@Conversion", SqlDbType.Decimal))
            objSQLCmd.Parameters("@Conversion").Value = Conversion
            objSQLCmd.Parameters.Add(New SqlParameter("@Is_Sellable", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Is_Sellable").Value = Is_Sellable
            If Item_Code.ToUpper = "STR-0001" Then
                log.Debug(Item_UOM_ID)
                log.Debug(Item_Code)
                log.Debug(Organization_ID)
                log.Debug(Item_UOM)
                log.Debug(Conversion)
                log.Debug(Is_Sellable)

            End If
            objSQLCmd.ExecuteNonQuery()
            bRetVal = True
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        Catch ex As Exception
            Err_No = "904019"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function




    Public Function UpdateItemUOM(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Item_UOM_ID As Integer, ByVal Item_Code As String, ByVal Organization_ID As String, ByVal Item_UOM As String, ByVal Conversion As Decimal, Is_Sellable As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_UpdateItemUOM", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure

            objSQLCmd.Parameters.Add(New SqlParameter("@Item_Code", SqlDbType.VarChar, 40))
            objSQLCmd.Parameters("@Item_Code").Value = Item_Code
            objSQLCmd.Parameters.Add(New SqlParameter("@Organization_ID", SqlDbType.VarChar, 100))
            objSQLCmd.Parameters("@Organization_ID").Value = Organization_ID
            objSQLCmd.Parameters.Add(New SqlParameter("@Item_UOM", SqlDbType.VarChar, 100))
            objSQLCmd.Parameters("@Item_UOM").Value = Item_UOM.ToUpper()
            objSQLCmd.Parameters.Add(New SqlParameter("@Conversion", SqlDbType.Decimal))
            objSQLCmd.Parameters("@Conversion").Value = Conversion
            objSQLCmd.Parameters.Add(New SqlParameter("@Is_Sellable", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Is_Sellable").Value = Is_Sellable

            objSQLCmd.ExecuteNonQuery()
            bRetVal = True
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
        Catch ex As Exception
            Err_No = "904020"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function

    Public Function DeleteItemUOM(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Item_UOM As String, ByVal Item_Code As String, ByVal Organization_ID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sRetVal As Boolean = False
        Try

            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("app_DeleteItemUOM", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@Item_UOM", SqlDbType.VarChar, 10))
            objSQLCmd.Parameters("@Item_UOM").Value = Item_UOM.ToUpper()
            objSQLCmd.Parameters.Add(New SqlParameter("@Item_Code", SqlDbType.VarChar, 40))
            objSQLCmd.Parameters("@Item_Code").Value = Item_Code
            objSQLCmd.Parameters.Add(New SqlParameter("@Organization_ID", SqlDbType.VarChar, 100))
            objSQLCmd.Parameters("@Organization_ID").Value = Organization_ID
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
        DeleteItemUOM = sRetVal
    End Function

    Public Function LoadExportItemUOMTemplate(ByVal OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable

        Dim sQry As String

        Try

            objSQLConn = _objDB.GetSQLConnection

            sQry = "usp_ExportItemUOMTemplate"



            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@OrgID", SqlDbType.VarChar, 100))
            objSQLCmd.Parameters("@OrgID").Value = OrgID

            objSQLDa = New SqlDataAdapter(objSQLCmd)

            objSQLDa.Fill(dt)
            objSQLCmd.Dispose()
            Return dt
        Catch ex As Exception
            Throw ex
        Finally

            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return dt
    End Function
    Public Function IsValidUOM(ByVal UOM As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim iRowsAffected As Integer = 0
        Dim retVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection

            sQry = "Select count(*) from TBL_App_Codes where Code_Type='UOM' AND Code_Value=@UOM"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@UOM", UOM)
            iRowsAffected = CInt(objSQLCmd.ExecuteScalar())
            If iRowsAffected > 0 Then
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
    End Function

    Public Function IsValidDefUOM(Code As String, ByVal UOM As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim iRowsAffected As Integer = 0
        Dim retVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection

            sQry = "Select count(*) from TBL_Item_UOM where Item_Code=@Code AND Item_UOM=@UOM"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@UOM", UOM)
            objSQLCmd.Parameters.AddWithValue("@Code", Code)
            iRowsAffected = CInt(objSQLCmd.ExecuteScalar())
            If iRowsAffected > 0 Then
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
    End Function
    Public Function IsValidBaseUOM(Code As String, ByVal UOM As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim iRowsAffected As Integer = 0
        Dim retVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection

            sQry = "Select count(*) from TBL_Item_UOM where Item_Code=@Code AND Item_UOM=@UOM and conversion=1"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@UOM", UOM)
            objSQLCmd.Parameters.AddWithValue("@Code", Code)
            iRowsAffected = CInt(objSQLCmd.ExecuteScalar())
            If iRowsAffected > 0 Then
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
    End Function
    Public Function ValidatePrimaryUOM(ByVal Item_code As String, ByVal Org_ID As String, ByVal UOM As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim iRowsAffected As Integer = 0
        Dim retVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection

            sQry = "Select count(*) from TBL_Product where Item_Code=@Item_Code AND Organization_ID=@Org_ID AND Primary_UOM_Code=@UOM"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@Item_Code", Item_code)
            objSQLCmd.Parameters.AddWithValue("@Org_ID", Org_ID)
            objSQLCmd.Parameters.AddWithValue("@UOM", UOM)
            iRowsAffected = CInt(objSQLCmd.ExecuteScalar())
            If iRowsAffected > 0 Then
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
    End Function
    Public Function IsValidRestrictiveR(ByVal RET_MODE_val As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim iRowsAffected As Integer = 0
        Dim retVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection

            sQry = "Select count(*) from TBL_App_Codes where Code_Type='RET_MODE' AND Code_Value=@RET_MODE_val"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@RET_MODE_val", RET_MODE_val)
            iRowsAffected = CInt(objSQLCmd.ExecuteScalar())
            If iRowsAffected > 0 Then
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
    End Function
    Public Function SearchUOMGrid(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal item_cod As String, ByVal Org_ID As String, ByVal FilterBy As String, ByVal FilterValue As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter

        Dim Query As String = ""
        Try
            If FilterBy = "Item UOM" Then

                Query = "SELECT * FROM  TBL_Item_UOM WHERE  Organization_ID ='" + Org_ID + "' AND Item_Code='" + item_cod + "' AND Item_UOM LIKE '%" + FilterValue + "%' "
            ElseIf FilterBy = "Conversion" Then
                Query = "SELECT * FROM TBL_Item_UOM WHERE Organization_ID ='" + Org_ID + "' AND Item_Code='" + item_cod + "' AND Conversion LIKE '%" + FilterValue + "%' "
            Else
                Query = "SELECT * FROM TBL_Item_UOM WHERE Organization_ID ='" + Org_ID + "' AND Item_Code='" + item_cod + "'"
            End If
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter(Query, objSQLConn)
            objSQLDA.Fill(dtItemUOM)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "740022"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtItemUOM
    End Function
    Public Function FillItemUOMGrid(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Item_Code As String, ByVal Organization_ID As String) As DataTable
        Dim objSQLConn As SqlConnection

        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("SELECT *,B.Site,case when isnull(Is_Sellable,'Y')='Y' then 'Yes' else 'No' end as Issellable  FROM  TBL_Item_UOM A inner join TBL_Org_CTL_H B on A.Organization_ID=B.ORG_HE_ID WHERE Item_Code=@Item_Code AND Organization_ID =@Organization_ID", objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@Item_Code", Item_Code)
            objSQLCmd.Parameters.AddWithValue("@Organization_ID", Organization_ID)
            Dim MsgDs As New DataSet
            Dim objSQLDA As SqlDataAdapter
            objSQLDA = New SqlDataAdapter(objSQLCmd)
            objSQLDA.Fill(dtItemUOM)
            objSQLDA.Dispose()




        Catch ex As Exception
            Err_No = "740021"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtItemUOM
    End Function
    Public Function GetProductsByID(ByRef Error_No As Long, ByRef Error_Desc As String, ProdId As String, OrgID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable

        Dim sQry As String

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            sQry = "usp_GetProductsByID"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@ProdID", ProdId)
            objSQLCmd.Parameters.AddWithValue("@Org_ID", OrgID)
            objSQLDa = New SqlDataAdapter(objSQLCmd)

            objSQLDa.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception

            Error_No = 61139
            Error_Desc = String.Format("Error while retrieving  record: {0}", ex.Message)
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return dt
    End Function
    Public Function ProductCodeExists(ByVal ProdCode As String, ProdID As String, Org As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim iRowsAffected As Integer = 0
        Dim retVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection

            sQry = "usp_ProductCodeExist"


            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@ProdCode", ProdCode)
            objSQLCmd.Parameters.AddWithValue("@ProdID", ProdID)
            objSQLCmd.Parameters.AddWithValue("@OrgID", Org)
            iRowsAffected = CInt(objSQLCmd.ExecuteScalar())
            If iRowsAffected > 0 Then
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
    End Function
    Public Function GetBrand() As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable

        Dim sQry As String

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            sQry = "usp_GetProductBrand"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLDa = New SqlDataAdapter(objSQLCmd)

            objSQLDa.Fill(dt)
            objSQLCmd.Dispose()
            Return dt
        Catch ex As Exception
            Throw ex
        Finally

            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return dt
    End Function
    Public Function GetUOM() As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable

        Dim sQry As String

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            sQry = "usp_GetProductUOM"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLDa = New SqlDataAdapter(objSQLCmd)

            objSQLDa.Fill(dt)
            objSQLCmd.Dispose()
            Return dt
        Catch ex As Exception
            Throw ex
        Finally

            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return dt
    End Function

    Public Function GetAgency() As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable

        Dim sQry As String

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            sQry = "usp_GetProductAgency"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLDa = New SqlDataAdapter(objSQLCmd)

            objSQLDa.Fill(dt)
            objSQLCmd.Dispose()
            Return dt
        Catch ex As Exception
            Throw ex
        Finally

            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return dt
    End Function

    Public Function GetCategory() As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable

        Dim sQry As String

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            sQry = "usp_GetProductCategory"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLDa = New SqlDataAdapter(objSQLCmd)

            objSQLDa.Fill(dt)
            objSQLCmd.Dispose()
            Return dt
        Catch ex As Exception
            Throw ex
        Finally

            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return dt
    End Function

    Public Function GetRestrictiveReturn() As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable

        Dim sQry As String

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            sQry = "usp_GetRestrictiveReturn"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLDa = New SqlDataAdapter(objSQLCmd)

            objSQLDa.Fill(dt)
            objSQLCmd.Dispose()
            Return dt
        Catch ex As Exception
            Throw ex
        Finally

            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return dt
    End Function

    Public Function GetOrgofITEM(ByVal Itemcode As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable

        Dim sQry As String
        Try

            objSQLConn = _objDB.GetSQLConnection
            sQry = "usp_GetOrgofItemCode"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@ItemCode", Itemcode)
            objSQLDa = New SqlDataAdapter(objSQLCmd)

            objSQLDa.Fill(dt)
            objSQLCmd.Dispose()
            Return dt
        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
    Public Function SwapInvoiceRule(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal _ScreatedBy As Integer, ByVal PlanID As Integer, ByVal Source As String, ByVal Dest As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim retVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            Dim myTrans As SqlTransaction = objSQLConn.BeginTransaction()
            Try
                sQry = "app_SwapInvoiceRule"

                objSQLCmd = New SqlCommand(sQry, objSQLConn)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.Parameters.AddWithValue("@PlanID", PlanID)
                objSQLCmd.Parameters.AddWithValue("@SourceID", Source)
                objSQLCmd.Parameters.AddWithValue("@DestID", Dest)
                objSQLCmd.Parameters.AddWithValue("@CreatedBy", _ScreatedBy)
                objSQLCmd.Transaction = myTrans

                objSQLCmd.ExecuteNonQuery()

                objSQLCmd.Dispose()

                myTrans.Commit()
                retVal = True


            Catch ex As Exception
                retVal = False
                myTrans.Rollback()
                Throw ex
            End Try
        Catch ex As Exception
            Error_No = 77023
            Error_Desc = String.Format("Error while swaping invoice rule: {0}", ex.Message)

        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        SwapInvoiceRule = retVal
    End Function

    Public Function ValidatePriceListCode(ByVal OrgID As String, ByVal Price_List_Code As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "SELECT COUNT(*) FROM  TBL_Price_List_H WHERE Organization_ID=@OrgID AND  Price_List_Code=@Price_List_Code"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            objSQLCmd.Parameters.AddWithValue("@Price_List_Code", Price_List_Code)

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

    Public Function IsProductCodeExists(ByVal ProdCode As String, ByVal Org As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "SELECT COUNT(*) FROM  TBL_Product WHERE Item_Code=@ProdCode AND  Organization_ID=@Org"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@Org", Org)
            objSQLCmd.Parameters.AddWithValue("@ProdCode", ProdCode)

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
    Public Function IsValidBrand(ByVal BrandCode As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "SELECT COUNT(*) FROM  TBL_App_Codes WHERE Code_Type ='PROD_BRAND' AND Code_Value=@BrandCode "
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@BrandCode", BrandCode)
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

    Public Function IsValidCategory(ByVal CategoryCode As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "SELECT COUNT(*) FROM  TBL_App_Codes WHERE  Code_Type='PROD_CATEGORY' AND Code_Value=@CategoryCode "
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@CategoryCode", CategoryCode)
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

    Public Function IsValidAgency(ByVal AgencyCode As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim success As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = "SELECT COUNT(*) FROM  TBL_App_Codes WHERE Code_Type='PROD_AGENCY' AND Code_Value=@AgencyCode "
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@AgencyCode", AgencyCode)
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

    Public Function GetProductGrid(ByVal opt As String, ByVal Org_ID As String, ByVal Item_code As String, ByVal Description As String, ByVal UOM As String, ByVal Agency As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable

        Dim sQry As String

        Try

            objSQLConn = _objDB.GetSQLConnection
            sQry = "usp_GetProductsListWithFilter"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@OPT", opt)
            objSQLCmd.Parameters.AddWithValue("@Org_ID", Org_ID)
            objSQLCmd.Parameters.AddWithValue("@Item_code", Item_code)
            objSQLCmd.Parameters.AddWithValue("@Description", Description)
            objSQLCmd.Parameters.AddWithValue("@UOM", UOM)
            objSQLCmd.Parameters.AddWithValue("@Agency", Agency)
            objSQLDa = New SqlDataAdapter(objSQLCmd)

            objSQLDa.Fill(dt)
            objSQLCmd.Dispose()
            Return dt
        Catch ex As Exception
            Throw ex
        Finally

            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return dt
    End Function


    Public Function GetOrg(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try

            objSQLConn = _objDB.GetSQLConnection


            Dim QueryString As String = String.Format("SELECT * FROM TBL_Org_CTL_H ")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "OrgTbl")

            GetOrg = MsgDs.Tables(0)
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

    Public Function GetProductsWithUOM(ByRef Error_No As Long, ByRef Error_Desc As String, ProdId As String, OrgID As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataSet

        Dim sQry As String

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            sQry = "usp_GetProductsWithUOM"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@ProdID", ProdId)
            objSQLCmd.Parameters.AddWithValue("@Org_ID", OrgID)
            objSQLDa = New SqlDataAdapter(objSQLCmd)

            objSQLDa.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception

            Error_No = 611399
            Error_Desc = String.Format("Error while retrieving  record: {0}", ex.Message)
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return dt
    End Function
End Class
