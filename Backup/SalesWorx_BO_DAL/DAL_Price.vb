Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Data
Public Class DAL_Price
    Private _objDB As DatabaseConnection
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Public Function GetPriceList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            'Dim QueryString As String = String.Format("SELECT A.Price_List_Line_ID,A.Price_List_ID,A.Inventory_Item_ID,A.Organization_ID,A.Item_UOM,A.Unit_Selling_Price,B.[Description] as Price_List_Type,P.Item_Code,P.Description,C.Currency_code,isnull(D.Decimal_Digits,0) as Decimal_Digits from dbo.TBL_Price_List As A Left Join dbo.TBL_Price_List_H As B on A.Price_List_ID = B.Price_List_ID Left Join  TBL_Product P on P.Inventory_Item_ID=A.Inventory_Item_ID and  P.Organization_ID=A.Organization_ID Left outer join TBL_Org_CTL_DTL C on C.MAS_Org_ID=A.Organization_ID Left outer join TBL_Currency D on D.Currency_Code=C.Currency_Code   Where 1=1 {0} order by P.Item_Code Asc", _sSearchParams, QueryStr)
            Dim QueryString As String = String.Format("SELECT A.Price_List_Line_ID,A.Price_List_ID,A.Inventory_Item_ID,A.Organization_ID,A.Item_UOM,A.Unit_Selling_Price,B.[Description] as Price_List_Type,P.Item_Code,P.Description,(select top 1 Currency_Code from TBL_Org_CTL_DTL where Mas_Org_ID=A.Organization_ID) AS Currency_code ,(Select top 1 Decimal_Digits from TBL_Org_CTL_DTL X,TBL_Currency Y  where Mas_Org_ID=A.Organization_ID and X.Currency_code=Y.Currency_code) as Decimal_Digits from dbo.TBL_Price_List As A Left Join dbo.TBL_Price_List_H As B on A.Price_List_ID = B.Price_List_ID Left Join  TBL_Product P on P.Inventory_Item_ID=A.Inventory_Item_ID and  P.Organization_ID=A.Organization_ID    Where 1=1 {0} order by P.Item_Code Asc", _sSearchParams, QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "PriceLstTbl")

            GetPriceList = MsgDs
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
Public Function GetDefaultPriceList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal DefaultPriceListID As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            'Dim QueryString As String = String.Format("SELECT A.Price_List_Line_ID,A.Price_List_ID,A.Inventory_Item_ID,A.Organization_ID,A.Item_UOM,A.Unit_Selling_Price,B.[Description] as Price_List_Type,P.Item_Code,P.Description,C.Currency_code,isnull(D.Decimal_Digits,0) as Decimal_Digits from dbo.TBL_Price_List As A Left Join dbo.TBL_Price_List_H As B on A.Price_List_ID = B.Price_List_ID Left Join  TBL_Product P on P.Inventory_Item_ID=A.Inventory_Item_ID and  P.Organization_ID=A.Organization_ID Left outer join TBL_Org_CTL_DTL C on C.MAS_Org_ID=A.Organization_ID Left outer join TBL_Currency D on D.Currency_Code=C.Currency_Code   Where 1=1 {0} order by P.Item_Code Asc", _sSearchParams, QueryStr)
            Dim QueryString As String = String.Format("select a.Unit_Selling_Price,B.item_code from TBL_Price_List a left join tbl_product b on a.inventory_item_ID=b.inventory_item_ID where a.price_list_id=" & Val(DefaultPriceListID))
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "PriceLstTbl")

            GetDefaultPriceList = MsgDs
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
  Public Function GetPriceListHeader(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            'Dim QueryString As String = String.Format("SELECT A.Price_List_Line_ID,A.Price_List_ID,A.Inventory_Item_ID,A.Organization_ID,A.Item_UOM,A.Unit_Selling_Price,B.[Description] as Price_List_Type,P.Item_Code,P.Description,C.Currency_code,isnull(D.Decimal_Digits,0) as Decimal_Digits from dbo.TBL_Price_List As A Left Join dbo.TBL_Price_List_H As B on A.Price_List_ID = B.Price_List_ID Left Join  TBL_Product P on P.Inventory_Item_ID=A.Inventory_Item_ID and  P.Organization_ID=A.Organization_ID Left outer join TBL_Org_CTL_DTL C on C.MAS_Org_ID=A.Organization_ID Left outer join TBL_Currency D on D.Currency_Code=C.Currency_Code   Where 1=1 {0} order by P.Item_Code Asc", _sSearchParams, QueryStr)
            Dim QueryString As String = String.Format("SELECT  * from TBL_Price_List_H {0} order by Description ", _sSearchParams, QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "PriceLstTbl")

            GetPriceListHeader = MsgDs
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
End Class
