Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Data
Imports log4net
Public Class DAL_Returns
    Private _objDB As DatabaseConnection
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Public Function GetInvoiceToAttach(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRepID As String, CustomerID As String, ShipSiteID As String, InventoryItemID As String, maxDate As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("sync_MC_ExecGetInvoiceInfo", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@SalesRepID", SalesRepID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CustomerID", CustomerID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@ShipSiteID", ShipSiteID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@InventoryItemID", InventoryItemID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@MaxInvoiceDate", maxDate)
 
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
    Public Function GetReturnNotes(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustomerID As String, SiteID As String, SalesRepID As String, Fromdate As String, Todate As String, RefNo As String, UserID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_GetReturnStockNote", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@Fromdate", Fromdate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Todate", Todate)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SalesRep", SalesRepID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@CustID", CustomerID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SiteID", SiteID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@DocNo", RefNo)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@UserID", UserID)

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
    Public Function GetDeliveryNote(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RefNo As String, OrgID As String, Type As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim ds As New DataSet
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Rep_GetDeliveryNotesForConsolidatOrder", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@Orig_Sys_Document_Ref", RefNo)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Type", Type)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgID)

            objSQLDA.Fill(ds)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return ds
    End Function
    Public Function GetReturnNoteDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal RowID As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim ds As New DataSet
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("app_GetReturnStockNoteDetails", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure

            objSQLDA.SelectCommand.Parameters.AddWithValue("@RowID", RowID)

            objSQLDA.Fill(ds)

        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return ds
    End Function

    Public Function GetConversion(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ItemCode As String, OrgID As String, ByVal UOM As String) As String
        Dim objSQLConn As SqlConnection
        Dim ds As New DataTable
        Dim dr As DataRow = Nothing
        Dim ProductPath As String = Nothing
        Dim objSQLDA As SqlDataAdapter
        Dim Conversion As String = "1"
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("Select * from TBL_Item_UOM where Item_Code=@ItemCode and Organization_ID=@OrgID and Item_UOM=@Item_UOM", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text

            objSQLDA.SelectCommand.Parameters.AddWithValue("@ItemCode", ItemCode)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@OrgID", OrgID)
            objSQLDA.SelectCommand.Parameters.AddWithValue("@Item_UOM", UOM)
            objSQLDA.Fill(ds)
            If ds.Rows.Count > 0 Then
                Conversion = ds.Rows(0)("Conversion").ToString
            End If
        Catch ex As Exception
            Err_No = "24069"
            Err_Desc = ex.Message

            Throw ex
        Finally

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return Conversion
    End Function
    Public Function ConfirmReturnNote(ByRef Err_No As Long, ByRef Err_Desc As String, RowID As String, Dt As DataTable, ByVal Confirmedby As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False
        Try

            Dim Lines As String = "<ReturnNote>"

            For Each dr As DataRow In Dt.Rows
                Lines = Lines & "<Line><C1>" & Val(dr("Line").ToString) & "</C1><C2>" & Val(dr("Inventory_item_ID").ToString) & "</C2><C3>" & Val(dr("Display_Qty").ToString) & "</C3><C4>" & dr("Display_UOM").ToString & "</C4><C5>" & dr("InvNo").ToString & "</C5><C6>" & Val(dr("UnitPrice").ToString) & "</C6><C7>" & Val(dr("MaxUnitPrice").ToString) & "</C7></Line>"
            Next

            Lines = Lines & "</ReturnNote>"

            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("app_ConfirmReturnNote", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@RowID", SqlDbType.VarChar, 50))
            objSQLCmd.Parameters("@RowID").Value = RowID
            objSQLCmd.Parameters.Add(New SqlParameter("@Lines", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Lines").Value = Lines
            objSQLCmd.Parameters.Add(New SqlParameter("@UserID", SqlDbType.Int))
            objSQLCmd.Parameters("@UserID").Value = Confirmedby

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
End Class
