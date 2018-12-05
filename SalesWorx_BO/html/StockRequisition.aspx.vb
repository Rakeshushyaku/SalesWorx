Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Configuration
Imports SalesWorx.BO.Common
Imports log4net
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Partial Public Class StockReq
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim Stock As New SalesWorx.BO.Common.Stock
    Dim ObjProduct As New Product
    Private Const ModuleName As String = "StockReq.aspx"
    Private Const PageID As String = "P219"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objLogin As New SalesWorx.BO.Common.Login
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session.Item("USER_ACCESS") Is Nothing Then
            Session.Add("BringmeBackHere", ModuleName)
            Response.Redirect("Login.aspx", False)
            Exit Sub
        End If
        If Not HasAuthentication() Then
            Err_No = 500
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
        End If

        If Not IsPostBack Then
            btn_Savenew.Attributes.Add("OnClick", "javascript:return validate()")
            Btn_Confirm.Attributes.Add("OnClick", "javascript:return validatefrm()")
            GENERATESTOCK.Value = Stock.GetStockGenerate(Err_No, Err_Desc)
            LoadOrgHeads()
            LoadAgency()
            If GENERATESTOCK.Value = "Y" Then
              lbl_Agency.Visible = True
              ddl_Agency.Visible = True
              'Button1.Visible = True
            Else
              lbl_Agency.Visible = False
              ddl_Agency.Visible = False
              'Button1.Visible = False
            End If
            ReqAlert.Value = "1"
        Else
            TobeConfirmed()
        End If
    End Sub
    Private Function HasAuthentication() As Boolean
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
    End Function
    Sub LoadOrgHeads()
        'ddl_org.DataSource = Stock.GetOrgsHeads(Err_No, Err_Desc)
        'ddl_org.DataTextField = "Description"
        'ddl_org.DataValueField = "ORG_HE_ID"
        'ddl_org.DataBind()
        'ddl_org.Items.Insert(0, "--Select Organisation--")
        'ddl_org.Items(0).Value = 0


        Dim objCommon As New Common
        Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
        ddl_org.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
        ddl_org.Items.Clear()
        ddl_org.Items.Insert(0, "--Select Organisation--")
        ddl_org.AppendDataBoundItems = True
        ddl_org.DataValueField = "MAS_Org_ID"
        ddl_org.DataTextField = "Description"
        ddl_org.DataBind()
        ddl_org.Items(0).Value = 0


        ddlOrgNew.DataSource = ObjProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
        ddlOrgNew.Items.Clear()
        ddlOrgNew.Items.Insert(0, "--Select Organisation--")
        ddlOrgNew.AppendDataBoundItems = True
        ddlOrgNew.DataValueField = "MAS_Org_ID"
        ddlOrgNew.DataTextField = "Description"
        ddlOrgNew.DataBind()
        ddlOrgNew.Items(0).Value = 0


        ddl_Van.Items.Clear()
        ddl_Van.Items.Insert(0, "--Select Van--")
        ddl_Van.Items(0).Value = 0

        ddlAgencyNew.Items.Clear()
        ddlAgencyNew.Items.Insert(0, "--Select Agency--")
        ddlAgencyNew.Items(0).Value = 0

        ddlSKU.Items.Clear()
        ddlSKU.Items.Insert(0, "--Select item--")
        ddlSKU.Items(0).Value = 0

        HClicked.Value = "0"
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functionb", "hidebtn();", True)
    End Sub
    Sub LoadAgency()
        ddl_Agency.DataSource = Stock.GetAgencyList(Err_No, Err_Desc, ddl_org.SelectedItem.Value)
        ddl_Agency.DataTextField = "Agency"
        ddl_Agency.DataValueField = "Agency"
        ddl_Agency.DataBind()
        ddl_Agency.Items.Insert(0, "--Select Agency--")
        ddl_Agency.Items(0).Value = 0
        HClicked.Value = "0"
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functionb", "hidebtn();", True)
    End Sub
    Sub LoadStockSheets()
        If GENERATESTOCK.Value = "Y" Then
                  LoadStockSheet()
                Else
                  LoadStockSheetFormOrg()
        End If
    End Sub
  
 Sub LoadStockSheet()
       Try
        Dim dsPrice As New DataSet
        Dim dtPrice As DataTable
        dsPrice = (New SalesWorx.BO.Common.Price).GetDefaultPriceList(Err_No, Err_Desc, AppSettings("DefualtPriceList"))
        If (dsPrice.Tables.Count > 0) Then
            dtPrice = New DataTable()
            dtPrice = dsPrice.Tables(0)
        End If
        If ddl_org.SelectedItem.Value <> "0" And ddl_Agency.SelectedItem.Value <> "0" Then
            If Not Pnl_Content.FindControl("ID1") Is Nothing Then
                Pnl_Content.Controls.Clear()
            End If
            Dim ConfirmedAt As String = ""
            Dim Str As String = ""
            If Stock.CheckStockReqConfirmed(Err_No, Err_Desc, ddl_Agency.SelectedItem.Value, ddl_org.SelectedItem.Value, ConfirmedAt) = True Then
                    Str = "Stock Requisition confirmed at " & CDate(ConfirmedAt).ToString("dd-MMM-yyyy hh:mm:ss tt")
                btnVisible.Value = 0
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functionb", "hidebtn();", True)
                RowCount.Value = "-1"
            Else

                Dim dt As New DataTable
                dt = Stock.GetStockRequisition(Err_No, Err_Desc, ddl_Agency.SelectedItem.Value, ddl_org.SelectedItem.Value)
                If dt.Rows.Count > 0 Then
                    Str = "<div id='main'><div style='color:red;'>Please click the Product/Sales Man to edit the quantities</div><br/><table cellpadding='0' cellspacing='0' border='0' class='overflow-y'><thead ><tr><th>Product</th><th class='setcolor'>Total</th>"
                    For j As Integer = 2 To dt.Columns.Count - 3
                        Str = Str & "<th class='SM' id='SM_" & j - 2 & "'>" & dt.Columns(j).ColumnName & "<input Type='hidden' id=" & "'HSM_" & j - 2 & "' name=" & "'HSM_" & j - 2 & "' value='" & dt.Columns(j).ColumnName & "'/></th>"
                    Next
                    Str = Str & "</tr></thead><tbody>"

                    For i As Integer = 0 To dt.Rows.Count - 1
                        Dim Rtoal As Decimal = 0

                        Str = Str & "<tr class='Qty_" & i.ToString & "'>"
                        Str = Str & "<th class='ProdHdr' id='ProdHdr_" & i & "'>" & dt.Rows(i)(0) & "<input Type='hidden' id=" & "'HPR_" & i & "' name=" & "'HPR_" & i & "' value='" & dt.Rows(i)(1) & "'/></th>"
                        Str = Str & "<td class='RTotal' id='RTotal_" & i & "'>  $RTOTAL$ </td>"
                        For j As Integer = 2 To dt.Columns.Count - 3
                            Rtoal = Rtoal + Val(dt.Rows(i)(j).ToString)
                            Str = Str & "<td class='Qty' id='Qty_" & i & "_" & j - 2 & "'>" & Format(Val(dt.Rows(i)(j).ToString), "###0") & "</td>"
                        Next
                        Str = Str.Replace("$RTOTAL$", Format(Rtoal, "###0"))

                        Str = Str & "</tr>"
                    Next
                    Str = Str & "<tr class='TotalQty'><th>Total</th>"
                    Str = Str & "<td class='Total' id='Total'>  </td>"
                    Dim Total As Decimal = 0
                    For i As Integer = 2 To dt.Columns.Count - 3

                        Dim sumObject As Object
                        Dim Expr As String = "Sum([" & dt.Columns(i).ColumnName & "])"
                        sumObject = dt.Compute(Expr, "")
                        Str = Str & "<td class='CTotal' id='CTotal_" & i - 2 & "'>" & Format(Val(sumObject.ToString()), "###0") & "</td>"
                        Total = Total + Val(sumObject.ToString())
                    Next

                    Str = Str.Replace("$TOTAL$", Format(Total, "###0"))
                    Str = Str & "</tr>"

                    Str = Str & "<tr class='TotalQty'><th>Total Value</th>"
                    Str = Str & "<td class='Total' id='Total'>  </td>"

                    Dim TotalVal As Decimal = 0
                    For i As Integer = 2 To dt.Columns.Count - 3
                      TotalVal = 0
                      For Each stdr As DataRow In dt.Rows
                      Dim colname As String = dt.Columns(i).ColumnName
                      Dim pid As String
                      Dim price As String = "0"
                      pid = stdr(1)
                      Dim selpricedr() As DataRow
                      If Not dtPrice Is Nothing Then
                          selpricedr = dtPrice.Select("item_code='" & pid & "'")
                          If selpricedr.Length > 0 Then
                                price = selpricedr(0)("Unit_Selling_Price")
                          End If
                      End If
                      TotalVal = TotalVal + Val(stdr(colname).ToString) * Val(price)

                      Next
                       Str = Str & "<td class='CTotal' id='CTotal_" & i - 2 & "'>" & Format(TotalVal, "###0.00") & "</td>"
                    Next
                    RowCount.Value = dt.Rows.Count - 1

                    Str = Str & "</tr>"
                    Str = Str & "</tbody></table></div>"
                    btnVisible.Value = 1
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functionb", "showbtn();", True)
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functiona", "showDialogue();", True)
                Else
                    Str = "No Stock Requisition added."
                    btnVisible.Value = 0
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functionb", "hidebtn();", True)
                    RowCount.Value = "-1"
                End If
            End If
            'Dim lit As New LiteralControl
            'lit.ID = "ID1"
            'lit.Text = Str

            'Pnl_Content.Controls.Add(lit)

            Label2.Text = Str
            Pnl_Content.Visible = True
        Else
            Label2.Text = ""
            Pnl_Content.Visible = True
            Btn_Confirm.Enabled = False
        End If
        Catch ex As Exception
            log.Error(ex.ToString)
       End Try
    End Sub

 Sub LoadStockSheetFormOrg()
       Try
        Dim dsPrice As New DataSet
        Dim dtPrice As DataTable
        dsPrice = (New SalesWorx.BO.Common.Price).GetDefaultPriceList(Err_No, Err_Desc, AppSettings("DefualtPriceList"))
        If (dsPrice.Tables.Count > 0) Then
            dtPrice = New DataTable()
            dtPrice = dsPrice.Tables(0)
        End If
        If ddl_org.SelectedItem.Value <> "0" Then
            If Not Pnl_Content.FindControl("ID1") Is Nothing Then
                Pnl_Content.Controls.Clear()
            End If
            Dim ConfirmedAt As String = ""
            Dim Str As String = ""
            'If Stock.CheckStockReqConfirmed(Err_No, Err_Desc, ddl_Agency.SelectedItem.Value, ddl_org.SelectedItem.Value, ConfirmedAt) = True Then
            '    Str = "Stock Requistion confirmed at " & CDate(ConfirmedAt).ToString("dd-MMM-yyyy hh:mm:ss tt")
            '    btnVisible.Value = 0
            '    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functionb", "hidebtn();", True)
            '    RowCount.Value = "-1"
            'Else

                Dim dt As New DataTable
                dt = Stock.GetStockRequisition(Err_No, Err_Desc, ddl_Agency.SelectedItem.Value, ddl_org.SelectedItem.Value)
                If dt.Rows.Count > 0 Then
                    Str = "<div id='main'><div style='color:red;'>Please click the Product/Sales Man to edit the quantities</div><br/><table cellpadding='0' cellspacing='0' border='0' class='overflow-y'><thead ><tr><th>Product</th><th class='setcolor'>Total</th>"
                    For j As Integer = 2 To dt.Columns.Count - 3
                        Str = Str & "<th class='SM' id='SM_" & j - 2 & "'>" & dt.Columns(j).ColumnName & "<input Type='hidden' id=" & "'HSM_" & j - 2 & "' name=" & "'HSM_" & j - 2 & "' value='" & dt.Columns(j).ColumnName & "'/></th>"
                    Next
                    Str = Str & "</tr></thead><tbody>"

                    For i As Integer = 0 To dt.Rows.Count - 1
                        Dim Rtoal As Decimal = 0

                        Str = Str & "<tr class='Qty_" & i.ToString & "'>"
                        Str = Str & "<th class='ProdHdr' id='ProdHdr_" & i & "'>" & dt.Rows(i)(0) & "<input Type='hidden' id=" & "'HPR_" & i & "' name=" & "'HPR_" & i & "' value='" & dt.Rows(i)(1) & "'/></th>"
                        Str = Str & "<td class='RTotal' id='RTotal_" & i & "'>  $RTOTAL$ </td>"
                        For j As Integer = 2 To dt.Columns.Count - 3
                            Rtoal = Rtoal + Val(dt.Rows(i)(j).ToString)
                            Str = Str & "<td class='Qty' id='Qty_" & i & "_" & j - 2 & "'>" & Format(Val(dt.Rows(i)(j).ToString), "###0") & "</td>"
                        Next
                        Str = Str.Replace("$RTOTAL$", Format(Rtoal, "###0"))
                        ' Str = Str & "<td style='border:0;'></td>"
                        Str = Str & "</tr>"
                    Next
                    Str = Str & "<tr class='TotalQty'><th>Total</th>"
                    Str = Str & "<td class='Total' id='Total'>  </td>"
                    Dim Total As Decimal = 0
                    For i As Integer = 2 To dt.Columns.Count - 3

                        Dim sumObject As Object
                        Dim Expr As String = "Sum([" & dt.Columns(i).ColumnName & "])"
                        sumObject = dt.Compute(Expr, "")
                        Str = Str & "<td class='CTotal' id='CTotal_" & i - 2 & "'>" & Format(Val(sumObject.ToString()), "###0") & "</td>"
                        Total = Total + Val(sumObject.ToString())
                    Next

                    Str = Str.Replace("$TOTAL$", Format(Total, "###0"))
                    Str = Str & "</tr>"

                    Str = Str & "<tr class='TotalQty'><th>Total Value</th>"
                    Str = Str & "<td class='Total' id='Total'>  </td>"

                    Dim TotalVal As Decimal = 0
                    For i As Integer = 2 To dt.Columns.Count - 3
                      TotalVal = 0
                      For Each stdr As DataRow In dt.Rows
                      Dim colname As String = dt.Columns(i).ColumnName
                      Dim pid As String
                      Dim price As String = "0"
                      pid = stdr(1)
                      Dim selpricedr() As DataRow
                      If Not dtPrice Is Nothing Then
                          selpricedr = dtPrice.Select("item_code='" & pid & "'")
                          If selpricedr.Length > 0 Then
                                price = selpricedr(0)("Unit_Selling_Price")
                          End If
                      End If
                      TotalVal = TotalVal + Val(stdr(colname).ToString) * Val(price)

                      Next
                       Str = Str & "<td class='CTotal' id='CTotal_" & i - 2 & "'>" & Format(TotalVal, "###0.00") & "</td>"
                    Next
                    RowCount.Value = dt.Rows.Count - 1

                    Str = Str & "</tr>"
                    Str = Str & "</tbody></table></div>"
                    btnVisible.Value = 1
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functionb", "showbtn();", True)
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functiona", "showDialogue();", True)
                Else
                    Str = "No Stock Requisition added."
                    btnVisible.Value = 0
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functionb", "hidebtn();", True)
                    RowCount.Value = "-1"
                End If
            'End If
            'Dim lit As New LiteralControl
            'lit.ID = "ID1"
            'lit.Text = Str

            'Pnl_Content.Controls.Add(lit)

            Label2.Text = Str
            Pnl_Content.Visible = True
        Else
            Label2.Text = ""
            Pnl_Content.Visible = True
            Btn_Confirm.Enabled = False
        End If
        Catch ex As Exception
            log.Error(ex.ToString)
       End Try
    End Sub
    <System.Web.Services.WebMethod()> _
    Public Shared Function SaveStock(ByVal Ostock As List(Of Stock)) As String

        Dim Err_No As Long
        Dim Err_Desc As String
        Dim Stock As New SalesWorx.BO.Common.Stock
        For Each stk As SalesWorx.BO.Common.Stock In Ostock
            Stock.Product = stk.Product
            Stock.SalesMan = stk.SalesMan
            Stock.Qty = stk.Qty
            Stock.UpdateStockRequisition(Err_No, Err_Desc, stk.Org_ID)
        Next
    End Function
    <System.Web.Services.WebMethod()> _
    Public Shared Function CheckStockReqConfirmed(ByVal Agency As String, ByVal OrgID As String) As String
        Dim Err_No As Long
        Dim Err_Desc As String
        Dim Stock As New SalesWorx.BO.Common.Stock
        Dim ConfirmedAt As String = ""
        If Stock.CheckStockReqConfirmed(Err_No, Err_Desc, Agency, OrgID, ConfirmedAt) = True Then
            Return "1"
        Else
            Return "0"
        End If
    End Function
    Private Sub ddl_Agency_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Agency.SelectedIndexChanged
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functionb", "enablebtn();", True)
        LoadStockSheet()
    End Sub
    Protected Sub Btn_Confirm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Btn_Confirm.Click
        
        Dim PORefNo As String = ""
        PORefNo = "PO-" & Now.ToString("ddMMyyyyhhmmss")
        Dim dt As New DataTable
        dt = Stock.GetStockRequisition(Err_No, Err_Desc, ddl_Agency.SelectedItem.Value, ddl_org.SelectedItem.Value)
        If StockCheck.Value = "Y" Then
            Dim dtInvStock As New DataTable
            dtInvStock.Columns.Add("Product")
            dtInvStock.Columns.Add("Available Stock")
            dtInvStock.Columns.Add("Requested Quantity")
            Dim Total As Decimal = 0
            For i As Integer = 0 To dt.Rows.Count - 1
                Total = 0
                For j As Integer = 2 To dt.Columns.Count - 3
                    Total = Total + Val(dt.Rows(i)(j).ToString)
                Next
                If Total > Val(dt.Rows(i)("Stock").ToString) Then
                    Dim ndr As DataRow
                    ndr = dtInvStock.NewRow
                    ndr(0) = dt.Rows(i)("ProductName").ToString
                    ndr(1) = Val(dt.Rows(i)("Stock").ToString)
                    ndr(2) = Total
                    dtInvStock.Rows.Add(ndr)
                End If
            Next
            If dtInvStock.Rows.Count > 0 Then
                gv_InvalidStock.DataSource = dtInvStock
                gv_InvalidStock.DataBind()
                MpInfoError.Show()
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functiona", "showDialogue();", True)
            Else
                If dt.Rows.Count > 0 Then
                    For j As Integer = 2 To dt.Columns.Count - 3
                        Stock.ConfirmStockRequisition(Err_No, Err_Desc, dt.Columns(j).ColumnName, ddl_Agency.SelectedItem.Value, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, PORefNo)
                        objLogin.SaveUserLog(Err_No, Err_Desc, "U", "MAIN MENU", "STOCK REQUISITION", ddl_Agency.SelectedItem.Value & "-" & dt.Columns(j).ColumnName, "Agency: " & ddl_Agency.SelectedItem.Value & " Van: " & dt.Columns(j).ColumnName & " Date: " & Now.ToString("dd-MMM-yyyy hh:mm tt"), CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                    Next
                End If
                If GENERATESTOCK.Value = "Y" Then
                  LoadStockSheet()
                Else
                  LoadStockSheetFormOrg()
                End If
                TobeConfirmed()
            End If
        Else
            If dt.Rows.Count > 0 Then
                For j As Integer = 2 To dt.Columns.Count - 3
                    Stock.ConfirmStockRequisition(Err_No, Err_Desc, dt.Columns(j).ColumnName, ddl_Agency.SelectedItem.Value, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, PORefNo)
                    objLogin.SaveUserLog(Err_No, Err_Desc, "U", "MAIN MENU", "STOCK REQUISITION", ddl_Agency.SelectedItem.Value & "-" & dt.Columns(j).ColumnName, "Agency: " & ddl_Agency.SelectedItem.Value & " Van: " & dt.Columns(j).ColumnName & " Date: " & Now.ToString("dd-MMM-yyyy hh:mm tt"), CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                Next
            End If
            If GENERATESTOCK.Value = "Y" Then
              LoadStockSheet()
            Else
              LoadStockSheetFormOrg()
            End If
            TobeConfirmed()
        End If
    End Sub

    Protected Sub ddl_org_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_org.SelectedIndexChanged
        btnVisible.Value = "0"
        LoadAgency()
        StockCheck.Value = Stock.GetWH_Type(Err_No, Err_Desc, ddl_org.SelectedItem.Value)
        Pnl_Content.Visible = False
        'LoadStockSheet()
         If GENERATESTOCK.Value = "N" Then
              ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functionb", "enablebtn();", True)
              LoadStockSheetFormOrg()
        End If
    End Sub
   
    Private Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim PORefNo As String = ""
        PORefNo = "PO-" & Now.ToString("ddMMyyyyhhmmss")
        Dim dt As New DataTable
        dt = Stock.GetStockRequisition(Err_No, Err_Desc, ddl_Agency.SelectedItem.Value, ddl_org.SelectedItem.Value)
        If dt.Rows.Count > 0 Then
            For j As Integer = 2 To dt.Columns.Count - 3
                Stock.ConfirmStockRequisition(Err_No, Err_Desc, dt.Columns(j).ColumnName, ddl_Agency.SelectedItem.Value, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, PORefNo)
                objLogin.SaveUserLog(Err_No, Err_Desc, "U", "MAIN MENU", "STOCK REQUISITION", ddl_Agency.SelectedItem.Value & "-" & dt.Columns(j).ColumnName, "Agency: " & ddl_Agency.SelectedItem.Value & " Van: " & dt.Columns(j).ColumnName & " Date: " & Now.ToString("dd-MMM-yyyy hh:mm tt"), CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
            Next
        End If
        If GENERATESTOCK.Value = "Y" Then
              LoadStockSheet()
            Else
              LoadStockSheetFormOrg()
            End If
        TobeConfirmed()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
       If GENERATESTOCK.Value = "Y" Then
              LoadStockSheet()
            Else
              LoadStockSheetFormOrg()
            End If
        TobeConfirmed()
    End Sub
    Sub TobeConfirmed()
        Dim DtTobeConfirmed As New DataTable
        DtTobeConfirmed = Stock.GetNotConfirmed(Err_No, Err_Desc)
        lbl_Msg.Text = ""
        If DtTobeConfirmed.Rows.Count > 0 Then
            lbl_Msg.Text = ""
        End If
        For Each dr In DtTobeConfirmed.Rows
            Dim txt As String
            If Val(dr("Count").ToString) > 1 Then
                txt = " Agencies"
            Else
                txt = " Agency"
            End If
            lbl_Msg.Text = lbl_Msg.Text & dr("Count").ToString & txt & " under " & dr("Description").ToString & ","
        Next
        If DtTobeConfirmed.Rows.Count > 0 Then
            lbl_Msg.Text = lbl_Msg.Text.Substring(0, Len(lbl_Msg.Text) - 1)
            lbl_Msg.Text = lbl_Msg.Text & " to be confirmed."
        End If
        If DtTobeConfirmed.Rows.Count = 0 Then
            lbl_Msg.Text = "You have no stock requisition to be confirmed"
        End If

    End Sub

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        If GENERATESTOCK.Value = "Y" Then
            Pnl_Agency.Visible = True
        Else
             Pnl_Agency.Visible = False
        End If
        ddlOrgNew.ClearSelection()
        ddl_Van.ClearSelection()
        ddlAgencyNew.ClearSelection()
        ddlSKU.ClearSelection()
        txt_Qty.Text = ""
        lbl_Result.Text = ""
        
        MpAddnew.Show()
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functiona", "showDialogue();", True)
    End Sub
    Private Sub ddlOrgNew_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrgNew.SelectedIndexChanged
        lbl_Result.Text = ""
        ddl_Van.ClearSelection()
        ddl_Van.DataSource = Stock.GetSalesMan(Err_No, Err_Desc, ddlOrgNew.SelectedItem.Value)
        ddl_Van.DataTextField = "SalesRepName"
        ddl_Van.DataValueField = "SalesRepName"
        ddl_Van.DataBind()
        ddl_Van.Items.Insert(0, "--Select Van--")
        ddl_Van.Items(0).Value = 0

        ddlAgencyNew.Items.Clear()
        ddlAgencyNew.Items.Insert(0, "--Select Agency--")
        ddlAgencyNew.Items(0).Value = 0

        ddlSKU.Items.Clear()
        ddlSKU.Items.Insert(0, "--Select item--")
        ddlSKU.Items(0).Value = 0
        ShowQuantity()

        MpAddnew.Show()
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functiona", "showDialogue();", True)
    End Sub

    Private Sub ddlSKU_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSKU.SelectedIndexChanged
        lbl_Result.Text = ""
        ShowQuantity()
        MpAddnew.Show()
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functiona", "showDialogue();", True)
    End Sub

    Private Sub ddlAgencyNew_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAgencyNew.SelectedIndexChanged
        ddlSKU.ClearSelection()
        ddlSKU.Items.Clear()
        lbl_Result.Text = ""
        Dim stock As New SalesWorx.BO.Common.Stock
        If ddlAgencyNew.SelectedItem.Value <> "0" Then
            ddlSKU.DataSource = stock.GetProductsByOrg_Agency(Err_No, Err_Desc, ddlOrgNew.SelectedItem.Value, ddlAgencyNew.SelectedItem.Value)
            ddlSKU.DataTextField = "Description"
            ddlSKU.DataValueField = "item_code"
            ddlSKU.DataBind()
        End If
        ddlSKU.Items.Insert(0, "--Select item--")
        ddlSKU.Items(0).Value = 0
        ShowQuantity()
        MpAddnew.Show()
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functiona", "showDialogue();", True)
    End Sub

    Private Sub ddl_Van_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Van.SelectedIndexChanged
        lbl_Result.Text = ""
         If GENERATESTOCK.Value = "Y" Then
            If ddl_Van.SelectedItem.Value <> "0" Then
            ddlAgencyNew.ClearSelection()
            ddlAgencyNew.Items.Clear()
            ddlAgencyNew.DataSource = Stock.GetAgencyListbySalesMan(Err_No, Err_Desc, ddlOrgNew.SelectedItem.Value, ddl_Van.SelectedItem.Value)
            ddlAgencyNew.DataTextField = "Agency"
            ddlAgencyNew.DataValueField = "Agency"
            ddlAgencyNew.DataBind()
            ddlAgencyNew.Items.Insert(0, "--Select Agency--")
            ddlAgencyNew.Items(0).Value = 0

            ddlSKU.Items.Clear()
            ddlSKU.Items.Insert(0, "--Select item--")
            ddlSKU.Items(0).Value = 0

        Else
            ddlAgencyNew.ClearSelection()
            ddlAgencyNew.Items.Clear()
            ddlAgencyNew.Items.Insert(0, "--Select Agency--")
            ddlAgencyNew.Items(0).Value = 0

            ddlSKU.Items.Clear()
            ddlSKU.Items.Insert(0, "--Select item--")
            ddlSKU.Items(0).Value = 0
        End If
        ShowQuantity()
        MpAddnew.Show()
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functiona", "showDialogue();", True)
        Else

            ddlSKU.Items.Clear()
            Dim stock As New SalesWorx.BO.Common.Stock
ddlSKU.Items.Clear()
                ddlSKU.DataSource = stock.GetItemListbySalesMan(Err_No, Err_Desc, ddlOrgNew.SelectedItem.Value)
                ddlSKU.DataTextField = "Description"
                ddlSKU.DataValueField = "item_code"
                ddlSKU.DataBind()

            ddlSKU.Items.Insert(0, "--Select item--")
            ddlSKU.Items(0).Value = 0
            ShowQuantity()
            MpAddnew.Show()
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functiona", "showDialogue();", True)

        End If
        
    End Sub
    Sub ShowQuantity()
     If GENERATESTOCK.Value = "Y" Then
        If ddlSKU.SelectedItem.Value <> "0" Then
            Dim Item() As String
            Item = ddlSKU.SelectedItem.Value.Split("$")
            txt_Qty.Text = Stock.GetRequestedQty(Err_No, Err_Desc, ddlOrgNew.SelectedItem.Value, ddl_Van.SelectedItem.Value, ddlAgencyNew.SelectedItem.Value, Item(0))
        Else
            txt_Qty.Text = ""
        End If
    Else
        If ddlSKU.SelectedItem.Value <> "0" Then
            Dim Item() As String
            Item = ddlSKU.SelectedItem.Value.Split("$")
            txt_Qty.Text = Stock.GetRequestedQty(Err_No, Err_Desc, ddlOrgNew.SelectedItem.Value, ddl_Van.SelectedItem.Value, "", Item(0))
        Else
            txt_Qty.Text = ""
        End If
    End If
    End Sub

    Private Sub btn_Savenew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Savenew.Click
        Dim Err_No As Long
        Dim Err_Desc As String
        Dim Stock As New SalesWorx.BO.Common.Stock
        If ddlSKU.SelectedItem.Value <> "0" Then
            Dim Item() As String
            Item = ddlSKU.SelectedItem.Value.Split("$")

            Stock.Product = Item(1)
            Stock.SalesMan = ddl_Van.SelectedItem.Value
            Stock.Qty = txt_Qty.Text
            If Stock.UpdateStockRequisition(Err_No, Err_Desc, ddlOrgNew.SelectedItem.Value) = True Then
                lbl_Result.Text = "Stock Requisition Saved"
            Else
                lbl_Result.Text = "Stock Requisition could not saved"
            End If
        End If
        MpAddnew.Show()
        If GENERATESTOCK.Value = "Y" Then
            Dim SelectedAgency As String = ""
            SelectedAgency = ddl_Agency.SelectedItem.Value
            LoadAgency()
            If Not ddl_Agency.Items.FindByValue(SelectedAgency) Is Nothing Then
                ddl_Agency.ClearSelection()
                ddl_Agency.Items.FindByValue(SelectedAgency).Selected = True
            End If
        End If
        TobeConfirmed()
        If GENERATESTOCK.Value = "Y" Then
              LoadStockSheet()
            Else
              LoadStockSheetFormOrg()
        End If
    End Sub
End Class