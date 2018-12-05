Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports Telerik.Web.UI
Imports log4net
Public Class ApproveReturnNotes
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    'Dim SortField As String = ""
    'Dim SortFieldDtl As String = ""
    Private Const PageID As String = "P417"
    Private RowIdx As Integer = 0
    Dim objLogin As New SalesWorx.BO.Common.Login
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        If Not IsPostBack Then
            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            If Not HasPermission Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If
            ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())


            If Not Request.QueryString("Row_ID") Is Nothing Then
                Try
                    HRowID.Value = Request.QueryString("Row_ID")
                    LoadReturnNote()
                Catch ex As Exception
                    Err_No = "74066"
                    If Err_Desc Is Nothing Then
                        log.Error(GetExceptionInfo(ex))
                    Else
                        log.Error(Err_Desc)
                    End If
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
                Finally
                    ObjCommon = Nothing
                    ErrorResource = Nothing
                End Try
            Else
                Response.Redirect("ListReturnNotesForApproval.aspx")
            End If

            If Not Err_Desc Is Nothing Then
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
            End If
        Else
            MPEDetails.VisibleOnPageLoad = False
        End If
    End Sub
    Sub LoadReturnNote()
        Dim ObjReturn As New SalesWorx.BO.Common.Returns
        Dim ds As New DataSet
        ds = ObjReturn.GetReturnNoteDetails(Err_No, Err_Desc, HRowID.Value)
        If ds.Tables.Count > 0 Then
            lbl_refno.Text = ds.Tables(0).Rows(0)("Orig_Sys_Document_Ref").ToString
            lbl_Customer.Text = ds.Tables(0).Rows(0)("Customer").ToString
            lbl_Salesep.Text = ds.Tables(0).Rows(0)("Salesrep_name").ToString
            lbl_Date.Text = CDate(ds.Tables(0).Rows(0)("Creation_Date").ToString).ToString("dd-MMM-yyyy hh:mm tt")
            lbl_amount.Text = Format(Val(ds.Tables(0).Rows(0)("Transaction_Amt").ToString), "#,##0.00")
            HOrgID.Value = ds.Tables(0).Rows(0)("MAS_Org_ID").ToString
            HPrice_List.Value = ds.Tables(0).Rows(0)("Price_List_ID").ToString
            HCustID.Value = ds.Tables(0).Rows(0)("Ship_To_Customer_ID").ToString
            HSite.Value = ds.Tables(0).Rows(0)("Ship_To_Site_ID").ToString
            HSalesRep.Value = ds.Tables(0).Rows(0)("Created_By").ToString
        End If
        If ds.Tables.Count > 1 Then
            Dim dtitems As New DataTable
            dtitems = ds.Tables(1).Copy
            For Each dr As DataRow In dtitems.Rows
                dr("Unit_Selling_Price") = Math.Round(Val(dr("Unit_Selling_Price").ToString), 2)
            Next
            gvItems.DataSource = dtitems
            gvItems.DataBind()
        End If
    End Sub
    Private Sub btnConfirm_Click(sender As Object, e As EventArgs) Handles btnConfirm.Click
        Try
            Dim ObjReturn As New SalesWorx.BO.Common.Returns
            Dim dt As New DataTable
            If gvItems.Items.Count > 0 Then
                If ValidatedQty() Then
                    dt.Columns.Add("Inventory_item_ID")
                    dt.Columns.Add("Line")
                    dt.Columns.Add("Display_Qty")
                    dt.Columns.Add("Display_UOM")
                    dt.Columns.Add("InvNo")
                    dt.Columns.Add("UnitPrice")
                    dt.Columns.Add("MaxUnitPrice")
                    Dim iCount As Integer = 0
                    For Each gr As GridDataItem In gvItems.Items
                        If Not gr.FindControl("Txt_Display_Order_Quantity") Is Nothing Then
                            Dim dr As DataRow
                            dr = dt.NewRow
                            dr("Inventory_item_ID") = CType(gr.FindControl("HInventory_Item_ID"), HiddenField).Value
                            dr("Line") = CType(gr.FindControl("HLine_Number"), HiddenField).Value
                            dr("Display_Qty") = CType(gr.FindControl("Txt_Display_Order_Quantity"), TextBox).Text.Trim
                            dr("Display_UOM") = CType(gr.FindControl("HUOM"), HiddenField).Value
                            dr("InvNo") = CType(gr.FindControl("HInvNo"), HiddenField).Value
                            dr("UnitPrice") = CType(gr.FindControl("UnitPrice"), HiddenField).Value
                            dr("MaxUnitPrice") = CType(gr.FindControl("MaxPrice"), HiddenField).Value
                            dt.Rows.Add(dr)
                            
                        End If
                    Next
                     
                    If ObjReturn.ConfirmReturnNote(Err_No, Err_Desc, HRowID.Value, dt, CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
                        MessageBoxValidation("Return Note confirmed successfully", "Information")
                        btnConfirm.Enabled = False
                    Else
                        MessageBoxValidation("Unexpected error while confirming Return Note", "Information")
                        btnConfirm.Enabled = True
                    End If
                End If
            End If
        Catch ex As Exception
            log.Debug(ex.ToString)
        End Try
    End Sub
    Function ValidatedQty()
        Dim Bretval As Boolean = True
        Try
            For Each gr As GridDataItem In gvItems.Items
                
                If Not gr.FindControl("Txt_Display_Order_Quantity") Is Nothing Then
                    If CType(gr.FindControl("Txt_Display_Order_Quantity"), TextBox).Text.Trim = "" Or IsNumeric(CType(gr.FindControl("Txt_Display_Order_Quantity"), TextBox).Text.Trim) = False Then
                        Dim line As String
                        line = CType(gr.FindControl("HLine_Number"), HiddenField).Value.Trim
                        MessageBoxValidation("Please enter a valid Qty for line " & line, "Information")
                        Bretval = False
                        Return Bretval
                    End If
                End If

                If Not gr.FindControl("Lbl_Invoice_No") Is Nothing Then
                    Dim line As String
                    line = CType(gr.FindControl("HLine_Number"), HiddenField).Value.Trim
                    If CType(gr.FindControl("Lbl_Invoice_No"), Label).Text.Trim = "" And Val(CType(gr.FindControl("Txt_Display_Order_Quantity"), TextBox).Text) <> 0 Then
                        MessageBoxValidation("Please attach invoice for line " & line, "Information")
                        Bretval = False
                        Return Bretval
                    End If
                End If
            Next

        Catch ex As Exception
            log.Debug(ex.ToString)
        End Try
        Return Bretval
    End Function

    
     

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Response.Redirect("ListReturnNotesForApproval.aspx")
    End Sub

    
    Private Sub Btn_attach_Click(sender As Object, e As EventArgs) Handles Btn_attach.Click
        Dim bVal As Boolean = False

        Dim Conversion As String = "1"
        Conversion = (New SalesWorx.BO.Common.Returns).GetConversion(Err_No, Err_Desc, HItemCode.Value, HOrgID.Value, HUOM.Value)

        If Txt_Quantity.Text.Trim = "" Or IsNumeric(Txt_Quantity.Text.Trim) = False Then
            lbl_msg.Text = "Please enter a valid quantity"
            MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If
        If Val(Txt_Quantity.Text) > 0 Then
            Dim Qty As Integer
            If Not (Integer.TryParse(Val(Txt_Quantity.Text) * Val(Conversion), Qty)) Then
                lbl_msg.Text = "Please enter a valid quantity"
                MPEDetails.VisibleOnPageLoad = True
                Exit Sub
            End If
            If gv_Invoice.SelectedItems.Count <= 0 Then
                lbl_msg.Text = "Please select any one invoice"
                MPEDetails.VisibleOnPageLoad = True
                Exit Sub
            End If


            If Val(Txt_Quantity.Text) * Val(Conversion) > Val(Val(CType(gv_Invoice.SelectedItems(0).FindControl("HReturn_Qty"), HiddenField).Value)) Then
                lbl_msg.Text = "Entered quantity exceeds the remaining invoice quantity"
                MPEDetails.VisibleOnPageLoad = True
                Exit Sub
            End If
            For Each gr As GridDataItem In gvItems.Items
                If CType(gr.FindControl("HInventory_Item_ID"), HiddenField).Value = HInvID.Value And CType(gr.FindControl("HLine_Number"), HiddenField).Value = HLineID.Value Then
                    CType(gr.FindControl("HInvNo"), HiddenField).Value = CType(gv_Invoice.SelectedItems(0).FindControl("HInvoice_No"), HiddenField).Value
                    Dim Uom As String = CType(gr.FindControl("HUOM"), HiddenField).Value
                    Dim ItemCode As String = CType(gr.FindControl("HItem_Code"), HiddenField).Value

                    CType(gr.FindControl("MaxPrice"), HiddenField).Value = Val(CType(gv_Invoice.SelectedItems(0).FindControl("HMax_Unit_Price"), HiddenField).Value)
                    CType(gr.FindControl("UnitPrice"), HiddenField).Value = Math.Round(Val(CType(gv_Invoice.SelectedItems(0).FindControl("HNet_Unit_Price"), HiddenField).Value), 2)
                    CType(gr.FindControl("Lbl_Invoice_No"), Label).Text = CType(gv_Invoice.SelectedItems(0).FindControl("HInvoice_No"), HiddenField).Value
                    CType(gr.FindControl("Lbl_Price"), Label).Text = Format(Math.Round(Val(CType(gv_Invoice.SelectedItems(0).FindControl("HNet_Unit_Price"), HiddenField).Value), 2), "###0.00")
                    CType(gr.FindControl("Txt_Display_Order_Quantity"), TextBox).Text = Val(Txt_Quantity.Text)
                    CType(gr.FindControl("Txt_Display_Order_Quantity"), TextBox).Enabled = False
                    bVal = True
                    Exit For
                End If
            Next
            If bVal = True Then
                MPEDetails.VisibleOnPageLoad = False
            Else
                MPEDetails.VisibleOnPageLoad = True
            End If
        Else
            For Each gr As GridDataItem In gvItems.Items
                If CType(gr.FindControl("HInventory_Item_ID"), HiddenField).Value = HInvID.Value And CType(gr.FindControl("HLine_Number"), HiddenField).Value = HLineID.Value Then
                    CType(gr.FindControl("MaxPrice"), HiddenField).Value = 0
                    CType(gr.FindControl("UnitPrice"), HiddenField).Value = CType(gr.FindControl("HActUP"), HiddenField).Value
                    CType(gr.FindControl("Lbl_Invoice_No"), Label).Text = ""
                    CType(gr.FindControl("Lbl_Price"), Label).Text = CType(gr.FindControl("HActUP"), HiddenField).Value
                    CType(gr.FindControl("Txt_Display_Order_Quantity"), TextBox).Enabled = False
                    CType(gr.FindControl("Txt_Display_Order_Quantity"), TextBox).Text = 0
                    bVal = True
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Sub Btn_Cancel_Click(sender As Object, e As EventArgs) Handles Btn_Cancel.Click
        MPEDetails.VisibleOnPageLoad = False
    End Sub

    Private Sub Btn_release_Click(sender As Object, e As EventArgs) Handles Btn_release.Click
        For Each gr As GridDataItem In gvItems.Items
            If CType(gr.FindControl("HInventory_Item_ID"), HiddenField).Value = HInvID.Value And CType(gr.FindControl("HLine_Number"), HiddenField).Value = HLineID.Value Then
                CType(gr.FindControl("MaxPrice"), HiddenField).Value = 0
                CType(gr.FindControl("UnitPrice"), HiddenField).Value = CType(gr.FindControl("HActUP"), HiddenField).Value
                CType(gr.FindControl("Lbl_Invoice_No"), Label).Text = ""
                CType(gr.FindControl("Lbl_Price"), Label).Text = CType(gr.FindControl("HActUP"), HiddenField).Value
                CType(gr.FindControl("Txt_Display_Order_Quantity"), TextBox).Enabled = False
                CType(gr.FindControl("HInvNo"), HiddenField).Value = ""
                Exit For
            End If
        Next
    End Sub

    Protected Sub ImageButton_Click(sender As Object, e As ImageClickEventArgs)
        lbl_msg.Text = ""
        HInvID.Value = 0
        HLineID.Value = 0
        HItemCode.Value = 0
        HUOM.Value = 0
        Txt_Quantity.Text = ""

        Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
        Dim row As Telerik.Web.UI.GridDataItem = DirectCast(btnEdit.NamingContainer, Telerik.Web.UI.GridDataItem)

        Dim Invid As String = CType(row.FindControl("HInventory_Item_ID"), HiddenField).Value
        Dim Qty As String = CType(row.FindControl("Txt_Display_Order_Quantity"), TextBox).Text
        Dim LineNo As String = CType(row.FindControl("HLine_Number"), HiddenField).Value
        Dim Lbl_Invoice_No As String = CType(row.FindControl("Lbl_Invoice_No"), Label).Text
        Dim Itemcode As String = CType(row.FindControl("HItem_Code"), HiddenField).Value
        Dim UOM As String = CType(row.FindControl("HUOM"), HiddenField).Value
        Dim PrimaryUOM As String = CType(row.FindControl("HPrimaryUOM"), HiddenField).Value
        Dim Conversion As String = "1"
        Conversion = (New SalesWorx.BO.Common.Returns).GetConversion(Err_No, Err_Desc, Itemcode, HOrgID.Value, UOM)

        If Lbl_Invoice_No.Trim <> "" Then
            Btn_release.Visible = True
        Else
            Btn_release.Visible = False
        End If
        HInvID.Value = Invid
        HLineID.Value = LineNo
        HItemCode.Value = Itemcode
        HUOM.Value = UOM
        Txt_Quantity.Text = Qty

        If UOM = PrimaryUOM Then
            lbl_UOM.Text = "Please Enter Returning Quantity ( " & UOM & " )"
        Else
            lbl_UOM.Text = "Please Enter Returning Quantity ( " & UOM & " = " & Conversion & " " & PrimaryUOM & " )"
        End If

        Dim dtinv As New DataTable
        dtinv = (New SalesWorx.BO.Common.Returns).GetInvoiceToAttach(Err_No, Err_Desc, HSalesRep.Value, HCustID.Value, HSite.Value, Invid, Now.ToString("MM-dd-yyyy hh:mm:ss tt"))

        Dim dtFinalinv As New DataTable
        dtFinalinv = dtinv.Copy

        dtFinalinv.Columns.Add("AvlQty", System.Type.GetType("System.Decimal"))
        dtFinalinv.Columns.Add("CInvoice_Qty", System.Type.GetType("System.Decimal"))
        dtFinalinv.Columns.Add("CAvlQty", System.Type.GetType("System.Decimal"))
        dtFinalinv.Columns.Add("CNet_Unit_Price", System.Type.GetType("System.Decimal"))
        dtFinalinv.Columns.Add("CUOM")

        Dim Converion As Decimal = (New SalesWorx.BO.Common.Product).GetConversion(Err_No, Err_Desc, Itemcode, HOrgID.Value, UOM)

        For Each dr As DataRow In dtFinalinv.Rows
            dr("CUOM") = UOM
            dr("Net_Unit_Price") = Val(dr("Net_Unit_Price").ToString)
            dr("AvlQty") = Val(dr("Invoice_qty").ToString) - Val(dr("Return_qty").ToString)
            dr("CInvoice_Qty") = Val(dr("Invoice_qty").ToString) / Converion
            dr("CAvlQty") = Val(dr("AvlQty").ToString) / Converion
            dr("CNet_Unit_Price") = Val(dr("Net_Unit_Price").ToString) * Converion
        Next

        For Each dr As DataRow In dtFinalinv.Rows
            For Each gr As GridDataItem In gvItems.Items
               
                If dr("Invoice_No").ToString.ToLower = CType(gr.FindControl("HInvNo"), HiddenField).Value.ToLower And Invid.ToLower = CType(gr.FindControl("HInventory_Item_ID"), HiddenField).Value.ToLower Then
                    dr("AvlQty") = Val(dr("AvlQty")) - (Val(CType(gr.FindControl("Txt_Display_Order_Quantity"), TextBox).Text) * Converion)
                    dr("CAvlQty") = Val(dr("CAvlQty")) - (Val(CType(gr.FindControl("Txt_Display_Order_Quantity"), TextBox).Text))
                    
                End If
            Next
           
            If dr("Invoice_No").ToString.ToLower = Lbl_Invoice_No.Trim.ToLower Then
                dr("AvlQty") = Val(dr("AvlQty")) + Val(Qty * Converion)
                dr("CAvlQty") = Val(dr("CAvlQty")) + Val(Qty)

            End If
        Next

        Dim finaldt As New DataTable
        finaldt = dtFinalinv.Clone
        Dim seldr() As DataRow
        seldr = dtFinalinv.Select("AvlQty>0")
        If seldr.Length > 0 Then
            finaldt = seldr.CopyToDataTable
        End If

        gv_Invoice.DataSource = finaldt
        gv_Invoice.DataBind()
        If Lbl_Invoice_No.Trim <> "" Then
            If Not gv_Invoice.MasterTableView.FindItemByKeyValue("Invoice_No", Lbl_Invoice_No.Trim) Is Nothing Then
                gv_Invoice.MasterTableView.FindItemByKeyValue("Invoice_No", Lbl_Invoice_No.Trim).Selected = True
            End If
         
        End If
        MPEDetails.VisibleOnPageLoad = True

    End Sub
End Class