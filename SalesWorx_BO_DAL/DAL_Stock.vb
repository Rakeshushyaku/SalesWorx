Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Data
Imports log4net

Public Class DAL_Stock
    Private _objDB As DatabaseConnection
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Public Function UpdateStockRequisitionByvan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRep As String, ByVal ItemID As String, ByVal Qty As String, ByVal Org_ID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_UpdateStockRequisitionByVan", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@SalesRep", SqlDbType.VarChar))
            objSQLCmd.Parameters("@SalesRep").Value = SalesRep
            objSQLCmd.Parameters.Add(New SqlParameter("@Item_code", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Item_code").Value = ItemID
            objSQLCmd.Parameters.Add(New SqlParameter("@Qty", SqlDbType.Decimal))
            objSQLCmd.Parameters("@Qty").Value = Qty
            objSQLCmd.Parameters.Add(New SqlParameter("@Org_ID", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Org_ID").Value = Org_ID

            Dim iRows As Integer
            iRows = objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
            If iRows > 0 Then
                bRetVal = True
            Else
                bRetVal = False
            End If
        Catch ex As Exception
            Err_No = "74206"
            Err_Desc = ex.Message
            log.Debug(ex.ToString)
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function
    Public Function UpdateStockRequisition(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRep As String, ByVal ItemID As String, ByVal Qty As String, ByVal Org_ID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_UpdateStockRequisition", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@SalesRep", SqlDbType.VarChar))
            objSQLCmd.Parameters("@SalesRep").Value = SalesRep
            objSQLCmd.Parameters.Add(New SqlParameter("@Item_code", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Item_code").Value = ItemID
            objSQLCmd.Parameters.Add(New SqlParameter("@Qty", SqlDbType.Decimal))
            objSQLCmd.Parameters("@Qty").Value = Qty
            objSQLCmd.Parameters.Add(New SqlParameter("@Date", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Date").Value = Now.ToString("dd-MMM-yyyy")
            objSQLCmd.Parameters.Add(New SqlParameter("@Org_ID", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Org_ID").Value = Org_ID
            Dim iRows As Integer
            iRows = objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()
            objSQLCmd = Nothing
            If iRows > 0 Then
                bRetVal = True
            Else
                bRetVal = False
            End If
        Catch ex As Exception
            Err_No = "74206"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
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
    Public Function GetAgencyList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_id As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("Select distinct Agency From V_StockRequisition where SalesOrg='" & Org_id & "' order by Agency")
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
    Public Function GetSalesMan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_id As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("Select distinct SalesRepName From V_StockRequisition where SalesOrg='" & Org_id & "' order by SalesRepName")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "PrdLstTbl")

            GetSalesMan = MsgDs
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
    Public Function GetItemListbySalesMan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_id As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            'Dim QueryString As String = String.Format(" select distinct  Inventory_Item_ID,item_code +'-'+ Description as Description,cast(Inventory_Item_ID as varchar)+'$'+item_code as item_code from TBL_Product where Organization_ID='" & Org_id & "' and Inventory_Item_ID in(select Inventory_Item_ID from TBL_Product where Organization_ID='" & Org_id & "' except select distinct Inventory_Item_ID from V_StockRequisition where SalesRepName='" & SalesMan & "' and StockTransfer_ID is not null union select distinct agency from V_StockRequisition where SalesRepName='" & SalesMan & "' and StockTransfer_ID is null )")
            Dim QueryString As String = String.Format(" select distinct  Inventory_Item_ID,item_code +'-'+ Description as Description,cast(Inventory_Item_ID as varchar)+'$'+item_code as item_code from TBL_Product where Organization_ID='" & Org_id & "'")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "PrdLstTbl")

            GetItemListbySalesMan = MsgDs
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
    Public Function GetAgencyListbySalesMan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_id As String, ByVal SalesMan As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("select distinct isnull(agency,'Others')as agency from TBL_Product where Organization_ID='" & Org_id & "' except select distinct agency from V_StockRequisition where SalesRepName='" & SalesMan & "' and StockTransfer_ID is not null union select distinct agency from V_StockRequisition where SalesRepName='" & SalesMan & "' and StockTransfer_ID is null ")
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "PrdLstTbl")

            GetAgencyListbySalesMan = MsgDs
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
    Public Function GetAgencyList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_id As String, ByVal ReqDate As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("select distinct isnull(Agency,'Others')as Agency  from TBL_Stock_Requisition A inner join TBL_Stock_Requisition_Items B on A.StockRequisition_ID=B.StockRequisition_ID inner join TBL_Product D on B.Inventory_Item_ID=D.Inventory_Item_ID where  b.StockTransfer_ID is not null and Dest_org_ID='" & Org_id & "'")
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
    Public Function GetUnconfirmedStockRequisition(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_id As String, ByVal Van As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_GetStockRequisitionUnconfirmed", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Van", Van)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@orgID", Org_id)
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
        GetUnconfirmedStockRequisition = dtDivConfig
    End Function
    Public Function StockRequisitionItemsbyOrgforExport(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RowID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT D.Org_ID as Van_location,B.Item_Code , A.Qty as Load_Qty FROM TBL_Stock_Requisition_Items AS A INNER JOIN TBL_Stock_Requisition C on C.StockRequisition_ID=A.StockRequisition_ID  INNER JOIN TBL_Product AS B ON A.Inventory_Item_ID = B.Inventory_Item_ID inner join TBL_Org_CTL_DTL D on D.SalesRep_ID=C.SalesRep_ID WHERE ( cast(CONVERT(VARCHAR(10), C.Request_Date, 101) as datetime) =cast(CONVERT(VARCHAR(10), getdate(), 101) as datetime) and C.Status='Y') order by D.Org_ID,B.Item_Code ", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
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
        StockRequisitionItemsbyOrgforExport = dtDivConfig
    End Function
    Public Function StockRequisitionItems(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RowID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT D.Org_ID as Van,B.Item_Code + ' - ' + B.Description AS item, A.Qty as Qty, A.Item_UOM as UOM FROM TBL_Stock_Requisition_Items AS A INNER JOIN TBL_Stock_Requisition C on C.StockRequisition_ID=A.StockRequisition_ID  INNER JOIN TBL_Product AS B ON A.Inventory_Item_ID = B.Inventory_Item_ID inner join TBL_Org_CTL_DTL D on D.SalesRep_ID=C.SalesRep_ID and B.Organization_ID=D.MAS_Org_ID WHERE (A.StockRequisition_ID ='" & RowID & "') order by B.Item_Code ", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
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
        StockRequisitionItems = dtDivConfig
    End Function
    Public Function StockRequisitionItemsForExport(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RowID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT D.Org_ID as Van_location,B.Item_Code , A.Qty as Load_Qty FROM TBL_Stock_Requisition_Items AS A INNER JOIN TBL_Stock_Requisition C on C.StockRequisition_ID=A.StockRequisition_ID  INNER JOIN TBL_Product AS B ON A.Inventory_Item_ID = B.Inventory_Item_ID and C.Dest_org_ID=B.Organization_ID inner join TBL_Org_CTL_DTL D on D.SalesRep_ID=C.SalesRep_ID WHERE (A.StockRequisition_ID ='" & RowID & "') order by B.Item_Code ", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
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
        StockRequisitionItemsForExport = dtDivConfig
    End Function

    Public Function GetStockRequisitionbyVan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Van As String, ByVal Org_id As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_GetStockRequisitionbyVan", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@Van", Van)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@orgID", Org_id)
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
        GetStockRequisitionbyVan = dtDivConfig
    End Function
    Public Function GetStockRequisition(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Agency As String, ByVal Org_id As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_GetStockRequisition", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@date", Now.ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Agency", Agency.Replace("'", "''"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@orgID", Org_id)
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
        GetStockRequisition = dtDivConfig
    End Function
    Public Function GetStockRequisition_UOM(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Agency As String, ByVal Org_id As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_GetStockRequisition_UOM", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.AddWithValue("@date", Now.ToString("dd-MMM-yyyy"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Agency", Agency.Replace("'", "''"))
            objSQLDA.SelectCommand.Parameters.AddWithValue("@orgID", Org_id)
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
        GetStockRequisition_UOM = dtDivConfig
    End Function
    Public Function GetRequestedQty(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal SalesRep As String, ByVal Agency As String, ByVal SKU As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable
        Dim Qty As Decimal = False
        Try
            objSQLConn = _objDB.GetSQLConnection

            objSQLDA = New SqlDataAdapter("select sum(isnull(Qty,0))as Qty from V_StockRequisition where StockTransfer_ID is null  and SalesOrg='" & OrgID & "' and SalesRepName='" & SalesRep & "' and Inventory_Item_ID=" & SKU, objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.Fill(dtDivConfig)
            objSQLDA.Dispose()
            If dtDivConfig.Rows.Count > 0 Then
                Qty = Format(Val(dtDivConfig.Rows(0)("Qty").ToString), "###0")
            End If

        Catch ex As Exception
            Err_No = "74206"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return Qty
    End Function

    Public Function GetRequestedQty(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal SalesRep As String, ByVal SKU As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable
        Dim Qty As Decimal = False
        Try
            objSQLConn = _objDB.GetSQLConnection

            objSQLDA = New SqlDataAdapter("Select sum(qty) as qty from (select  case when b.Custom_Attribute_1 is null then b.Qty else CAST(b.Custom_Attribute_1 as decimal) end / isnull( (Select Conversion from TBL_Item_UOM U where U.Organization_ID='" & OrgID & "' and U.Item_code=P.Item_code and  U.Item_code=P.Item_code and U.Item_UOM =isnull( (select top 1 addl.Custom_Attribute_1 from TBL_Product_Addl_Info addl where Attrib_Name='DU' and addl.Inventory_Item_ID=p.Inventory_Item_ID and addl.Organization_ID='" & OrgID & "' ), p.Primary_UOM_Code)),1)  as Qty  from TBL_Stock_Requisition A inner join TBL_Stock_Requisition_items B on A.StockRequisition_ID=B.StockRequisition_ID inner join tbl_fsr F on F.SalesRep_ID=a.SalesRep_ID inner join TBL_Product P on P.Inventory_Item_ID=b.Inventory_Item_ID and P.Organization_ID='" & OrgID & "' where A.status<>'A' and a.SalesRep_ID='" & SalesRep & "' and P.Inventory_Item_ID=" & SKU & ")as X", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.Fill(dtDivConfig)
            objSQLDA.Dispose()
            If dtDivConfig.Rows.Count > 0 Then
                Qty = Format(Val(dtDivConfig.Rows(0)("Qty").ToString), "###0")
            End If

        Catch ex As Exception
            Err_No = "74206"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return Qty
    End Function

    Public Function ConfirmStockRequisitionbyVan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRep As String, ByVal Confirmedby As String, ByVal PORefno As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_ConfirmStockRequistionbyVan", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@SalesRep_Name", SqlDbType.VarChar))
            objSQLCmd.Parameters("@SalesRep_Name").Value = SalesRep
            objSQLCmd.Parameters.Add(New SqlParameter("@Confimedby", SqlDbType.Decimal))
            objSQLCmd.Parameters("@Confimedby").Value = Confirmedby
            objSQLCmd.Parameters.Add(New SqlParameter("@PoRefno", SqlDbType.VarChar))
            objSQLCmd.Parameters("@PoRefno").Value = PORefno
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

    Public Function ConfirmStockRequisition(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRep As String, ByVal Agency As String, ByVal Confirmedby As String, ByVal PORefno As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_ConfirmStockRequistion", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@Agency", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Agency").Value = Agency
            objSQLCmd.Parameters.Add(New SqlParameter("@SalesRep_Name", SqlDbType.VarChar))
            objSQLCmd.Parameters("@SalesRep_Name").Value = SalesRep
            objSQLCmd.Parameters.Add(New SqlParameter("@Confimedby", SqlDbType.Decimal))
            objSQLCmd.Parameters("@Confimedby").Value = Confirmedby
            objSQLCmd.Parameters.Add(New SqlParameter("@Date", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Date").Value = Now.ToString("dd-MMM-yyyy")
            objSQLCmd.Parameters.Add(New SqlParameter("@PoRefno", SqlDbType.VarChar))
            objSQLCmd.Parameters("@PoRefno").Value = PORefno
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
    Public Function ConfirmStockRequisitionbyID(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal StockRequisition_ID As String, ByVal Confirmedby As String, ByVal PORefno As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_ConfirmStockRequistionByID", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@StockRequisition_ID", SqlDbType.VarChar))
            objSQLCmd.Parameters("@StockRequisition_ID").Value = StockRequisition_ID
            objSQLCmd.Parameters.Add(New SqlParameter("@Confimedby", SqlDbType.Decimal))
            objSQLCmd.Parameters("@Confimedby").Value = Confirmedby
            objSQLCmd.Parameters.Add(New SqlParameter("@PoRefno", SqlDbType.VarChar))
            objSQLCmd.Parameters("@PoRefno").Value = PORefno
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()
            objSQLCmd = Nothing

            bRetVal = True

        Catch ex As Exception
            Err_No = "74206"
            Err_Desc = ex.Message
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function

    Public Function GetWH_Type(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Org_id As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable
        Dim sRetVal As String = ""
        Try

            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("select * from TBL_Org_CTL_H where ORG_HE_ID=@ORG_HE_ID", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ORG_HE_ID", Org_id)
            objSQLDA.Fill(dtDivConfig)
            objSQLDA.Dispose()
            If dtDivConfig.Rows.Count > 0 Then
                sRetVal = dtDivConfig.Rows(0)("WH_Type").ToString
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
    Public Function CheckStockReqConfirmedByVan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Van As String, ByVal Org_id As String, ByRef ConfirmedAt As String) As Boolean
        Dim success As Boolean = False
        Dim objSQLConn As SqlConnection
        Dim dtDivConfig As New DataTable
        Dim dtDivConfig1 As New DataTable

        Dim objSQLDA As SqlDataAdapter
        Dim objSQLDA1 As SqlDataAdapter
        Dim Rcnt As String = ""
        Dim sql As String = ""

        sql = "Select * from TBL_Stock_Requisition A inner join tbl_fsr F on A.SalesRep_ID=F.SalesRep_ID where Status<>'A' and A.SalesRep_ID in(select item from dbo.SplitQuotedString(@van))"
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA1 = New SqlDataAdapter(sql, objSQLConn)
            objSQLDA1.SelectCommand.CommandType = CommandType.Text
            objSQLDA1.SelectCommand.Parameters.AddWithValue("@van", Van)
            objSQLDA1.Fill(dtDivConfig1)

            If dtDivConfig1.Rows.Count > 0 Then
                success = False
            Else
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

    Public Function CheckStockReqConfirmed(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Agency As String, ByVal Org_id As String, ByRef ConfirmedAt As String) As Boolean
        Dim success As Boolean = False
        Dim objSQLConn As SqlConnection
        Dim dtDivConfig As New DataTable
        Dim dtDivConfig1 As New DataTable

        Dim objSQLDA As SqlDataAdapter
        Dim objSQLDA1 As SqlDataAdapter
        Dim Rcnt As String = ""
        Dim sql As String = ""

        sql = "SELECT top 1 * from V_StockRequisition where  StockTransfer_ID is null   and cast(CONVERT(VARCHAR(10), Request_Date, 101) as datetime)=convert(datetime,'" & Now.ToString("dd-MMM-yyyy") & "',103) and isnull(Agency,'Others') ='" & Agency.Replace("'", "''") & "' and  SalesOrg='" & Org_id & "'"
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA1 = New SqlDataAdapter(sql, objSQLConn)
            objSQLDA1.SelectCommand.CommandType = CommandType.Text
            objSQLDA1.Fill(dtDivConfig1)

            If dtDivConfig1.Rows.Count > 0 Then
                success = False
            Else
                success = True
                sql = "SELECT top 1  b.StockTransfer_ID,Transfer_Date  FROM TBL_Stock_Requisition a INNER JOIN TBL_Stock_Requisition_Items b ON A.StockRequisition_ID=B.StockRequisition_ID" & _
                      " INNER JOIN TBL_FSR c ON a.SalesRep_ID=c.SalesRep_ID inner join TBL_Van_Info E on  E.Van_Org_ID = c.SalesRep_Number " & _
                      " inner join TBL_Org_CTL_DTL F on e.Van_Org_ID =F.Org_ID INNER JOIN TBL_Product d ON d.Inventory_Item_ID=B.Inventory_Item_ID AND " & _
                      " D.Primary_UOM_Code = B.Item_UOM Inner join TBL_Stock_Transfer G on G.StockTransfer_ID=b.StockTransfer_ID where cast(CONVERT(VARCHAR(10), a.Request_Date, 101) as datetime)=convert(datetime,'" & Now.ToString("dd-MMM-yyyy") & "',103) and isnull(d.Agency,'Others') ='" & Agency.Replace("'", "''") & "' and  d.Organization_ID='" & Org_id & "'"


                objSQLDA = New SqlDataAdapter(sql, objSQLConn)
                objSQLDA.SelectCommand.CommandType = CommandType.Text
                objSQLDA.Fill(dtDivConfig)

                If dtDivConfig.Rows.Count > 0 Then
                    ConfirmedAt = dtDivConfig.Rows(0)(1).ToString
                End If
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
    Public Function GetNotConfirmed(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable
        Try

            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("select COUNT(distinct isnull(Agency,'Others') )as Count,B.Description from dbo.V_StockRequisition A left join TBL_Org_CTL_H  B on A.SalesOrg=B.ORG_HE_ID   where StockTransfer_ID is null  group by B.Description", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.Fill(dtDivConfig)
            objSQLDA.Dispose()


        Catch ex As Exception
            Err_No = "74206"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtDivConfig
    End Function
    Public Function GetProductsByOrg_Agency(ByRef Err_No As Long, ByRef Err_Desc As String, ByRef OrgID As Integer, Optional ByVal Agency As String = "0") As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String
            QueryString = String.Format(" Select Inventory_Item_ID,item_code +'-'+ Description as Description,cast(Inventory_Item_ID as varchar)+'$'+item_code as item_code from TBL_Product Where Organization_ID={0} ", OrgID, Agency)
            If Agency <> "0" Then
                QueryString = QueryString & " and isnull(Agency,'Others')='" & Agency.Replace("'", "''") & "'"
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
    Public Function GetStockGenerate(ByRef Err_No As Long, ByRef Err_Desc As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable
        Dim sRetVal As String = ""
        Try

            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("select Control_Value from TBL_App_Control where Control_Key='CREATE_STOCK_FROM_REQUISITION'", objSQLConn)
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
    Public Function IsValidVan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Van As String, ByRef Salesrep_ID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable
        Dim BRetval As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection

            objSQLDA = New SqlDataAdapter("select  SalesRep_ID from TBL_Org_CTL_DTL D inner join TBL_Van_Info B  on B.Van_Org_ID =D.Org_ID inner join TBL_Emp_Info A on A.Emp_Code=b.Emp_Code where org_id='" & Van.Trim() & "' and Mas_org_ID='" & OrgID & "'", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.Fill(dtDivConfig)
            objSQLDA.Dispose()
            If dtDivConfig.Rows.Count > 0 Then
                Salesrep_ID = dtDivConfig.Rows(0)("SalesRep_ID").ToString
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
    Public Function AlreadyConfirmed(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Salesrep_ID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable
        Dim BRetval As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection

            objSQLDA = New SqlDataAdapter("Select * from TBL_Stock_Requisition A Inner join TBL_Stock_Requisition_Items B on A.StockRequisition_ID=B.StockRequisition_ID where SalesRep_ID=" & Salesrep_ID & " and  cast(CONVERT(VARCHAR(10), A.Request_Date, 101) as datetime) =cast(CONVERT(VARCHAR(10), getdate(), 101) as datetime) and stocktransfer_ID is not null", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.Fill(dtDivConfig)
            objSQLDA.Dispose()
            If dtDivConfig.Rows.Count > 0 Then
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
    Public Function ValidItemUOM(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Item As String, ByVal UOM As String, ByRef Conversion As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable
        Dim BRetval As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection

            objSQLDA = New SqlDataAdapter("Select Conversion from TBL_Item_UOM where Item_Code='" & Item & "' and Organization_ID='" & OrgID & "' and Item_UOM='" & UOM & "'", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.Fill(dtDivConfig)
            objSQLDA.Dispose()
            If dtDivConfig.Rows.Count > 0 Then
                Conversion = dtDivConfig.Rows(0)("Conversion").ToString
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
    Public Function GetItemUOM(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Item As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable
        Dim Primary_UOM_Code As String = ""
        Try
            objSQLConn = _objDB.GetSQLConnection

            objSQLDA = New SqlDataAdapter("Select Primary_UOM_Code from TBL_Product where Item_Code='" & Item & "' and Organization_ID='" & OrgID & "'", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.Fill(dtDivConfig)
            objSQLDA.Dispose()
            If dtDivConfig.Rows.Count > 0 Then
                Primary_UOM_Code = dtDivConfig.Rows(0)("Primary_UOM_Code").ToString
            End If

        Catch ex As Exception
            Err_No = "74206"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return Primary_UOM_Code
    End Function
    Public Function GetConverion(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Item As String, UOm As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable
        Dim Conversion As String = ""
        Try
            objSQLConn = _objDB.GetSQLConnection

            objSQLDA = New SqlDataAdapter("Select Conversion from TBL_Item_UOM where Item_Code='" & Item & "' and Organization_ID='" & OrgID & "' and Item_UOM='" & UOm & "'", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.Fill(dtDivConfig)
            objSQLDA.Dispose()
            If dtDivConfig.Rows.Count > 0 Then
                Conversion = dtDivConfig.Rows(0)("Conversion").ToString
            End If

        Catch ex As Exception
            Err_No = "74206"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return Conversion
    End Function
    Public Function GetItemUOMs(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Item As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String
            QueryString = "Select x.Item_Code,Item_UOM,Conversion,case when duom is null then p.Primary_UOM_Code  else duom end as DUOM from (Select *,(Select a.Attrib_Value  from TBL_Product_Addl_Info A inner join TBL_Product B on a.Inventory_Item_ID=b.Inventory_Item_ID and a.Organization_ID=b.Organization_ID and a.Attrib_Name='DU' and u.Item_Code=b.Item_Code )as DUOM from TBL_Item_UOM U where Item_Code='" & Item & "' and Organization_ID='" & OrgID & "' ) as x inner join TBL_Product P on x.Item_Code=p.Item_Code and x.Organization_ID=p.Organization_ID order by Conversion"
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "Product")

            GetItemUOMs = MsgDs.Tables("Product")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function ImportStockRequisitions(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal dt As DataTable, ByRef errorDt As DataTable) As Boolean
        Dim bRetval As Boolean = False
        Try
            Dim TobeDistinct As String() = {"Van_Location"}
            Dim dtDistinct As DataTable = GetDistinctRecords(dt, TobeDistinct)
            If dtDistinct.Rows.Count > 0 Then
                For Each vandr As DataRow In dtDistinct.Rows
                    Dim newdt As New DataTable
                    Dim seldr() As DataRow
                    seldr = dt.Select("Van_Location='" & vandr("Van_Location").ToString & "'")
                    If seldr.Length > 0 Then
                        newdt.Columns.Add("StockRequisition_Item_ID", System.Type.GetType("System.Guid"))
                        newdt.Columns.Add("StockRequisition_ID", System.Type.GetType("System.Guid"))
                        newdt.Columns.Add("Inventory_Item_ID", System.Type.GetType("System.String"))
                        newdt.Columns.Add("Qty", System.Type.GetType("System.Decimal"))
                        newdt.Columns.Add("Item_UOM", System.Type.GetType("System.String"))
                        newdt.Columns.Add("Display_UOM", System.Type.GetType("System.String"))
                        newdt.Columns.Add("Display_Qty", System.Type.GetType("System.Decimal"))
                        For Each dr In seldr
                            Dim newdr As DataRow
                            newdr = newdt.NewRow
                            newdr("Inventory_Item_ID") = dr("Item_ID")
                            newdr("Display_Qty") = dr("Load_Quantity")
                            newdr("Display_UOM") = dr("Item_UOM")
                            newdr("Item_UOM") = dr("Item_UOM")
                            newdr("Qty") = dr("UOMQty")
                            newdt.Rows.Add(newdr)
                        Next
                        If Not SaveStockRequisitions(Err_No, Err_Desc, OrgID, newdt, seldr(0)("SalesRep_ID").ToString) Then
                            Dim errorDr As DataRow
                            errorDr = errorDt.NewRow
                            errorDr(0) = vandr("Van_Location").ToString
                            errorDt.Rows.Add(errorDr)

                        End If
                        newdt = Nothing
                    End If
                Next
            End If
            bRetval = True
        Catch ex As Exception
            Err_No = "74206"
            Err_Desc = ex.Message
            Throw ex
        Finally
        End Try
        Return bRetval
    End Function
    Public Shared Function GetDistinctRecords(ByVal dt As DataTable, ByVal Columns As String()) As DataTable
        Dim dtUniqRecords As New DataTable()
        dtUniqRecords = dt.DefaultView.ToTable(True, Columns)
        Return dtUniqRecords
    End Function

    Public Function SaveStockRequisitions(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal dt As DataTable, ByVal SalesRep_ID As String) As Boolean

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
                Dim StockRequisition_ID As String = ""
                objSQLCmd = New SqlCommand("app_SaveStockRequisitions", objSQLConn)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.Transaction = objSQLtrans
                objSQLCmd.Parameters.Add(New SqlParameter("@SalesRep_ID", SqlDbType.BigInt))
                objSQLCmd.Parameters("@SalesRep_ID").Value = Val(SalesRep_ID)
                objSQLCmd.Parameters.Add(New SqlParameter("@Dest_Org_ID", SqlDbType.VarChar))
                objSQLCmd.Parameters("@Dest_Org_ID").Value = OrgID
                objSQLCmd.Parameters.Add("@StockRequisition_ID", SqlDbType.UniqueIdentifier)
                objSQLCmd.Parameters("@StockRequisition_ID").Direction = ParameterDirection.Output

                objSQLCmd.ExecuteNonQuery()
                Dim id As String
                id = objSQLCmd.Parameters("@StockRequisition_ID").Value.ToString()

                If id <> "" Then
                    StockRequisition_ID = id
                    bMainTableSaved = True
                Else
                    bMainTableSaved = False
                End If

                If bMainTableSaved = True Then
                    For Each dr As DataRow In dt.Rows
                        dr(0) = Guid.NewGuid()
                        dr(1) = StockRequisition_ID
                    Next

                    Using bulkCopy As SqlBulkCopy = _
                      New SqlBulkCopy(objSQLConn, SqlBulkCopyOptions.Default, objSQLtrans)
                        bulkCopy.DestinationTableName = "dbo.TBL_Stock_Requisition_Items"
                        bulkCopy.ColumnMappings.Add("StockRequisition_Item_ID", "StockRequisition_Item_ID")
                        bulkCopy.ColumnMappings.Add("StockRequisition_ID", "StockRequisition_ID")
                        bulkCopy.ColumnMappings.Add("Inventory_Item_ID", "Inventory_Item_ID")
                        bulkCopy.ColumnMappings.Add("Qty", "Qty")
                        bulkCopy.ColumnMappings.Add("Item_UOM", "Item_UOM")
                        bulkCopy.ColumnMappings.Add("Display_UOM", "Display_UOM")
                        bulkCopy.ColumnMappings.Add("Display_Qty", "Display_Qty")
                        bulkCopy.WriteToServer(dt)
                        bChilTblSaved = True
                    End Using

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
    Function SalesRepforPurchaseReport(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Agency As String, ByVal SDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("select distinct F.SalesRep_ID,F.SalesRep_Name,o.Emp_Code,o.Emp_name  from TBL_Stock_Requisition A inner join TBL_Stock_Requisition_Items b on a.StockRequisition_ID=b.StockRequisition_ID inner join TBL_Org_CTL_DTL D on D.SalesRep_ID=a.SalesRep_ID inner join TBL_FSR F on D.SalesRep_ID=F.SalesRep_ID inner join TBL_Van_Info O on o.Van_Org_ID=d.Org_ID inner join TBL_Emp_Info E on E.Emp_Code=o.Emp_Code where d.MAS_Org_ID='" & OrgID & "' and b.StockTransfer_ID is not null and A.Request_Date>=convert(datetime,'" + SDate + "',103)", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
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
        SalesRepforPurchaseReport = dtDivConfig
    End Function
    Function GetPurchaseReport(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Agency As String, ByVal SDate As String, ByVal SID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("usp_RepWareHousePurchase", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.Add(New SqlParameter("@Agency", SqlDbType.VarChar))
            objSQLDA.SelectCommand.Parameters("@Agency").Value = Agency
            objSQLDA.SelectCommand.Parameters.Add(New SqlParameter("@Org_ID", SqlDbType.VarChar))
            objSQLDA.SelectCommand.Parameters("@Org_ID").Value = OrgID
            objSQLDA.SelectCommand.Parameters.Add(New SqlParameter("@Date", SqlDbType.VarChar))
            objSQLDA.SelectCommand.Parameters("@Date").Value = SDate
            objSQLDA.SelectCommand.Parameters.Add(New SqlParameter("@SalesrepID", SqlDbType.Int))
            objSQLDA.SelectCommand.Parameters("@SalesrepID").Value = SID
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
        GetPurchaseReport = dtDivConfig
    End Function

    Public Function HasLots(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Item_Code As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dt_haslots As New DataTable
        Dim bRetVal As Boolean = False
        Dim haslots_val As String = ""
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim Qry As String = ""

            Qry = "SELECT * FROM TBL_Product_Addl_Info WHERE  Attrib_Name='LC' AND Inventory_Item_ID=(SELECT Inventory_Item_ID FROM TBL_Product  WHERE Organization_ID='" + OrgID + "' AND Item_Code='" + Item_Code + "')"
            objSQLDA = New SqlDataAdapter(Qry, objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.Fill(dt_haslots)
            objSQLDA.Dispose()
            If dt_haslots.Rows.Count > 0 Then
                haslots_val = dt_haslots.Rows(0)("Attrib_Value").ToString
                If haslots_val.Trim().ToUpper() = "Y" Then
                    bRetVal = True
                End If
            End If

        Catch ex As Exception
            Err_No = "74206"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function


    Public Function GetImportStockTbls(ByRef Err_No As Long, ByRef Err_Desc As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable
        Dim MsgDs As New DataSet

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Get_ImportStockTbls", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.Fill(MsgDs)
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
        GetImportStockTbls = MsgDs
    End Function
    Function GetItemDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Item_code As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dt_product As New DataTable

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT * FROM  TBL_Product WHERE Organization_ID='" + OrgID + "' AND Item_Code='" + Item_code + "'", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.Fill(dt_product)
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
        GetItemDetails = dt_product
    End Function

    Public Function ImportStock(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal xml_str As String, ByVal OrgID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_importstock", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@x", SqlDbType.VarChar))
            objSQLCmd.Parameters("@x").Value = xml_str
            objSQLCmd.Parameters.Add(New SqlParameter("@Org_ID", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Org_ID").Value = OrgID
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()
            objSQLCmd = Nothing

            bRetVal = True

        Catch ex As Exception
            Err_No = "74206"
            Err_Desc = ex.Message
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return bRetVal
    End Function

    Function Get_ImportStockUnConfirm(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal xml_str As String, ByVal OrgID As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable
        Dim MsgDs As New DataSet

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_importstockUnconfirm", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.Add(New SqlParameter("@x", SqlDbType.VarChar))
            objSQLDA.SelectCommand.Parameters("@x").Value = xml_str
            objSQLDA.SelectCommand.Parameters.Add(New SqlParameter("@Org_ID", SqlDbType.VarChar))
            objSQLDA.SelectCommand.Parameters("@Org_ID").Value = OrgID
            objSQLDA.Fill(MsgDs)
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
        Get_ImportStockUnConfirm = MsgDs
    End Function
    Function Get_ImportStockUnConfirmUpdated(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal xml_str As String, ByVal OrgID As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable
        Dim MsgDs As New DataSet

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_ImportStockUnconfirm_new", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
            objSQLDA.SelectCommand.Parameters.Add(New SqlParameter("@x", SqlDbType.VarChar))
            objSQLDA.SelectCommand.Parameters("@x").Value = xml_str
            objSQLDA.SelectCommand.Parameters.Add(New SqlParameter("@Org_ID", SqlDbType.VarChar))
            objSQLDA.SelectCommand.Parameters("@Org_ID").Value = OrgID
            objSQLDA.Fill(MsgDs)
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
        Get_ImportStockUnConfirmUpdated = MsgDs
    End Function
    Function Get_ImportStockConfirm(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal xml_str As String, ByVal OrgID As String, ByVal UID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_importstockconfirm", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@x", SqlDbType.VarChar))
            objSQLCmd.Parameters("@x").Value = xml_str
            objSQLCmd.Parameters.Add(New SqlParameter("@Org_ID", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Org_ID").Value = OrgID
            objSQLCmd.Parameters.Add(New SqlParameter("@UID", SqlDbType.Int))
            objSQLCmd.Parameters("@UID").Value = UID

            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()
            objSQLCmd = Nothing

            bRetVal = True


        Catch ex As Exception
            Err_No = "74204"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        Return bRetVal
    End Function
    Public Function IsValidItem(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Item_Code As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtProd As New DataTable
        Dim BRetval As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT ISNULL( Is_Active,'Y') AS Is_Active  FROM TBL_Product WHERE Organization_ID ='" & OrgID & "' AND  Item_Code ='" & Item_Code & "'", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.Fill(dtProd)
            objSQLDA.Dispose()
            If dtProd.Rows.Count > 0 Then
                If (dtProd.Rows(0)("Is_Active").ToString().ToUpper().Trim() = "Y") Then
                    BRetval = True
                End If
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


    Public Function GetUOMforConverion(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Item As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable
        Dim UOMforConverion As String = ""
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Select ISNULL(( case  when A.Custom_Attribute_1 is null  then   A.Attrib_Value else Custom_Attribute_1 end), P.Primary_UOM_Code ) UOM from TBL_Product_Addl_Info A inner join TBL_Product P on A.Inventory_Item_ID=P.Inventory_Item_ID  and A.Organization_ID=P.Organization_ID and A.Attrib_Name='DU' and P.Item_Code='" & Item & "' and  P.Organization_ID='" & OrgID & "'", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.Fill(dtDivConfig)
            objSQLDA.Dispose()
            If dtDivConfig.Rows.Count > 0 Then
                UOMforConverion = dtDivConfig.Rows(0)("UOM").ToString
            End If

        Catch ex As Exception
            Err_No = "74207"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return UOMforConverion
    End Function

    Public Function GetConversion(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Item As String, ByVal UOM As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable
        Dim Conversion As String = ""
        Try
            objSQLConn = _objDB.GetSQLConnection

            objSQLDA = New SqlDataAdapter("Select Conversion from TBL_Item_UOM where Item_Code='" & Item & "' and Organization_ID='" & OrgID & "' and Item_UOM='" & UOM & "'", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.Fill(dtDivConfig)
            objSQLDA.Dispose()
            If dtDivConfig.Rows.Count > 0 Then
                Conversion = dtDivConfig.Rows(0)("Conversion").ToString

            End If

        Catch ex As Exception
            Err_No = "74206"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return Val(Conversion)
    End Function

    Public Function ValidUOM(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String, ByVal Item As String, ByVal UOM As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtDivConfig As New DataTable
        Dim BRetval As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection

            objSQLDA = New SqlDataAdapter("Select Conversion from TBL_Item_UOM where Item_Code='" & Item & "' and Organization_ID='" & OrgID & "' and Item_UOM='" & UOM & "'", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.Fill(dtDivConfig)
            objSQLDA.Dispose()
            If dtDivConfig.Rows.Count > 0 Then

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
End Class

