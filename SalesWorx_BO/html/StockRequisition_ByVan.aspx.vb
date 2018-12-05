Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Configuration
Imports SalesWorx.BO.Common
Imports log4net
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports Microsoft.Reporting.WebForms
Public Class StockRequisition_ByVan
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim Stock As New SalesWorx.BO.Common.Stock
    Dim ObjProduct As New Product
    Private Const ModuleName As String = "StockRequisition_ByVan.aspx"
    Private Const PageID As String = "P415"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objLogin As New SalesWorx.BO.Common.Login
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
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
            GENERATESTOCK.Value = Stock.GetStockGenerate(Err_No, Err_Desc)
            LoadOrgHeads()
            LadVan()
            ReqAlert.Value = "1"
        
        End If
    End Sub
    Private Function HasAuthentication() As Boolean
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
    End Function
    Sub LoadOrgHeads()

        Dim objCommon As New Common
        Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
        ddl_org.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
        ddl_org.Items.Clear()
        ddl_org.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))
        ddl_org.AppendDataBoundItems = True
        ddl_org.DataValueField = "MAS_Org_ID"
        ddl_org.DataTextField = "Description"
        ddl_org.DataBind()
        ddl_org.Items(0).Value = 0
         
        If ddl_org.Items.Count = 2 Then
            ddl_org.SelectedIndex = 1

        End If

        ddlOrgNew.DataSource = ObjProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
        ddlOrgNew.Items.Clear()
        ddlOrgNew.Items.Insert(0, "Select Organization")
        ddlOrgNew.AppendDataBoundItems = True
        ddlOrgNew.DataValueField = "MAS_Org_ID"
        ddlOrgNew.DataTextField = "Description"
        ddlOrgNew.DataBind()
        ddlOrgNew.Items(0).Value = 0

        If ddlOrgNew.Items.Count = 2 Then
            ddlOrgNew.SelectedIndex = 1

        End If

        ddl_Van.Items.Clear()
        ddl_Van.Items.Insert(0, "Select Van")
        ddl_Van.Items(0).Value = 0

        H_Org_ID.Value = ddl_org.SelectedItem.Value
        HClicked.Value = "0"
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functionb", "hidebtn();", True)
    End Sub
    Sub LadVan()
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

        Dim ObjCommon As SalesWorx.BO.Common.Common
        ObjCommon = New SalesWorx.BO.Common.Common()
        ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddl_org.SelectedValue, objUserAccess.UserID.ToString())
        ddlVan.DataBind()

        For Each itm As RadComboBoxItem In ddlVan.Items
            itm.Checked = True
        Next

        HClicked.Value = "0"
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functionb", "hidebtn();", True)
    End Sub
    Sub LoadStockSheet()
        Try
            Dim Str As String = ""
            If ddl_org.SelectedItem.Value <> "0" And ddlVan.CheckedItems.Count > 0 Then
                If Not Pnl_Content.FindControl("ID1") Is Nothing Then
                    Pnl_Content.Controls.Clear()
                End If
                Dim ConfirmedAt As String = ""


                Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems

                Dim van As String = ""
                For Each li As RadComboBoxItem In collection
                    van = van & li.Value & ","
                Next

                If van = "" Then
                    van = "0"
                End If


                Dim dt As New DataTable
                dt = Stock.GetStockRequisitionbyVan(Err_No, Err_Desc, van, ddl_org.SelectedItem.Value)
                If dt.Rows.Count > 0 Then
                    Str = "<div id='main'><div style='color:red;'>Please click the Product/Sales Man to edit the quantities</div><br/><table cellpadding='0' cellspacing='0' border='0' class='overflow-y'><thead ><tr><th>Product</th><th class='setcolor'>Total</th>"
                    For j As Integer = 2 To dt.Columns.Count - 3
                        Str = Str & "<th class='SM' id='SM_" & j - 2 & "'><span class='t-edit-icon'>" & dt.Columns(j).ColumnName & "</span><input Type='hidden' id=" & "'HSM_" & j - 2 & "' name=" & "'HSM_" & j - 2 & "' value='" & dt.Columns(j).ColumnName & "'/></th>"
                    Next
                    Str = Str & "</tr></thead><tbody>"

                    For i As Integer = 0 To dt.Rows.Count - 1
                        Dim Rtoal As Decimal = 0

                        Str = Str & "<tr class='Qty_" & i.ToString & "'>"
                        Str = Str & "<th class='ProdHdr' id='ProdHdr_" & i & "'><span class='t-edit-icon'>" & dt.Rows(i)(0) & "</span><input Type='hidden' id=" & "'HPR_" & i & "' name=" & "'HPR_" & i & "' value='" & dt.Rows(i)(1) & "'/></th>"
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


                    RowCount.Value = dt.Rows.Count - 1


                    Str = Str & "</tbody></table></div>"
                    btnVisible.Value = 1
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functionb", "showbtn();", True)
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functiona", "showDialogue();", True)
                Else
                    Str = "No pending stock requisition to confirm for the selected van/vans."
                    btnVisible.Value = 0
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functionb", "hidebtn();", True)
                    RowCount.Value = "-1"
                End If
            End If

            Label2.Text = Str
            Pnl_Content.Visible = True
        Catch ex As Exception
            log.Error(ex.ToString)
        End Try
    End Sub

     
    <System.Web.Services.WebMethod()> _
    Public Shared Function SaveStock(ByVal Ostock As List(Of Stock)) As String
        Try
            Dim Err_No As Long
            Dim Err_Desc As String
            Dim Stock As New SalesWorx.BO.Common.Stock
            For Each stk As SalesWorx.BO.Common.Stock In Ostock
                Stock.Product = stk.Product
                Stock.Van = stk.SalesMan
                Stock.Qty = stk.Qty
                Stock.UpdateStockRequisitionByvan(Err_No, Err_Desc, stk.Org_ID)
            Next
        Catch ex As Exception
            log.Error(ex.ToString)
        End Try
        
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
   
    Protected Sub Btn_Confirm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Btn_Confirm.Click
        If ddl_org.SelectedIndex <= 0 Then
            MessageBoxValidation("Please select the organization", "Information")
            Exit Sub
        End If
        If ddlVan.CheckedItems.Count <= 0 Then
            MessageBoxValidation("Please select the van/FSR", "Information")
            Exit Sub
        End If
        Dim Bconfirmed As Boolean = False
        Dim PORefNo As String = ""
        PORefNo = "PO-" & Now.ToString("ddMMyyyyhhmmss")

        Dim dt As New DataTable
        Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems
        If collection.Count > 0 Then
            Dim van As String = ""
            Dim vanStr As String = ""
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","
                vanStr = vanStr & li.Text & ","
            Next

            If van = "" Then
                van = "0"
            Else
                van = van.Substring(0, van.Length - 1)
                vanStr = vanStr.Substring(0, vanStr.Length - 1)
            End If

            dt = Stock.GetStockRequisitionbyVan(Err_No, Err_Desc, van, ddl_org.SelectedItem.Value)
            If dt.Rows.Count > 0 Then
                For j As Integer = 2 To dt.Columns.Count - 3
                    Stock.ConfirmStockRequisitionbyVan(Err_No, Err_Desc, dt.Columns(j).ColumnName, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, PORefNo)
                    objLogin.SaveUserLog(Err_No, Err_Desc, "U", "MAIN MENU", "STOCK REQUISITION", van & "-" & dt.Columns(j).ColumnName, " Van: " & dt.Columns(j).ColumnName & " Date: " & Now.ToString("dd-MMM-yyyy hh:mm tt"), CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                    Bconfirmed = True
                Next
                If Bconfirmed = True Then
                    MessageBoxValidation("Stock Requisitions confirmed succesfully", "Information")
                End If
            End If
        End If

        LoadStockSheet()

    End Sub

    Protected Sub ddl_org_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_org.SelectedIndexChanged
        btnVisible.Value = "0"
        LadVan()
        StockCheck.Value = Stock.GetWH_Type(Err_No, Err_Desc, ddl_org.SelectedItem.Value)
        Pnl_Content.Visible = False
        H_Org_ID.Value = ddl_org.SelectedItem.Value
    End Sub

    

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        LoadStockSheet()


    End Sub
     

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
         
        ddlOrgNew.ClearSelection()
        If ddlOrgNew.Items.Count = 2 Then
            ddlOrgNew.SelectedIndex = 1

        End If

        ddlSKU.Text = ""
        ddl_Van.ClearSelection()

        ddlSKU.ClearSelection()

        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

        Dim ObjCommon As SalesWorx.BO.Common.Common
        ObjCommon = New SalesWorx.BO.Common.Common()
        ddl_Van.Items.Clear()
        ddl_Van.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrgNew.SelectedValue, objUserAccess.UserID.ToString())
        ddl_Van.DataTextField = "SalesRep_Name"
        ddl_Van.DataValueField = "SalesRep_ID"
        ddl_Van.DataBind()
        ddl_Van.Items.Insert(0, New RadComboBoxItem("Select Van", "0"))

        txt_Qty.Text = ""
        lbl_Result.Text = ""

        MpAddnew.Show()
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functiona", "showDialogue();", True)
    End Sub
    Private Sub ddlOrgNew_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrgNew.SelectedIndexChanged
        lbl_Result.Text = ""
        ddl_Van.ClearSelection()
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

        Dim ObjCommon As SalesWorx.BO.Common.Common
        ObjCommon = New SalesWorx.BO.Common.Common()
        ddl_Van.Items.Clear()
        ddl_Van.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrgNew.SelectedValue, objUserAccess.UserID.ToString())
        ddl_Van.DataTextField = "SalesRep_Name"
        ddl_Van.DataValueField = "SalesRep_ID"
        ddl_Van.DataBind()
        ddl_Van.Items.Insert(0, New RadComboBoxItem("Select Van", "0"))

        MpAddnew.Show()
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functiona", "showDialogue();", True)
    End Sub

    Private Sub ddlSKU_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddlSKU.ItemsRequested

        Try



            Dim ObjProuduct As New SalesWorx.BO.Common.Reports()



            Dim dt As New DataTable

            If dt.Rows.Count > 0 Then
                dt.Rows.Clear()
            End If


            'dt = Objrep.GetCustomerfromOrgtext(Err_No, Err_Desc, ddlOrganization.SelectedValue, e.Text)

            dt = ObjProuduct.GetAllActiveItemsbyOrg(Err_No, Err_Desc, ddlOrgNew.SelectedValue, e.Text)

            Dim ItemsPerRequest As Integer = 100 'Set the number of values to show
            Dim itemOffset As Integer = e.NumberOfItems
            Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
            e.EndOfItems = endOffset = dt.Rows.Count

            'Loop through the values to populate the combo box
            For i As Integer = itemOffset To endOffset - 1
                Dim item As New RadComboBoxItem()
                item.Text = dt.Rows(i).Item("Description").ToString
                item.Value = dt.Rows(i).Item("item_code").ToString

                ddlSKU.Items.Add(item)
                item.DataBind()
            Next
            MpAddnew.Show()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub ddlSKU_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSKU.SelectedIndexChanged
        lbl_Result.Text = ""
        ShowQuantity()
        MpAddnew.Show()
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functiona", "showDialogue();", True)
    End Sub
     Sub ShowQuantity()
        If Not String.IsNullOrEmpty(ddlSKU.SelectedValue) Then
            Dim Item() As String = ddlSKU.SelectedValue.Split("$")
            txt_Qty.Text = Stock.GetRequestedQty(Err_No, Err_Desc, ddlOrgNew.SelectedItem.Value, ddl_Van.SelectedItem.Value, Item(0))
        Else
            txt_Qty.Text = ""
        End If
         
    End Sub

    Private Sub btn_Savenew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Savenew.Click
        Dim Err_No As Long
        Dim Err_Desc As String
        Dim Stock As New SalesWorx.BO.Common.Stock
        If ddlOrgNew.SelectedIndex <= 0 Then
            lbl_Result.Text = "Please select the organization"
            MpAddnew.Show()

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functiona", "showDialogue();", True)
            Exit Sub
        End If
        If ddl_Van.SelectedIndex <= 0 Then
            lbl_Result.Text = "Please select the van/FSR"
            MpAddnew.Show()

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functiona", "showDialogue();", True)
            Exit Sub
        End If
        If String.IsNullOrEmpty(ddlSKU.SelectedValue) Then
            lbl_Result.Text = "Please select the SKU"
            MpAddnew.Show()

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functiona", "showDialogue();", True)
            Exit Sub
        End If
        If IsNumeric(txt_Qty.Text.Trim) > 0 Then
            lbl_Result.Text = "Please enter a valid quantity"
            MpAddnew.Show()

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functiona", "showDialogue();", True)
            Exit Sub
        End If
        If Val(txt_Qty.Text.Trim) <= 0 Then
            lbl_Result.Text = "Please enter the quantity"
            MpAddnew.Show()

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functiona", "showDialogue();", True)
            Exit Sub
        End If
        If Not String.IsNullOrEmpty(ddlSKU.SelectedValue) Then

            Dim Item() As String = ddlSKU.SelectedValue.Split("$")

            Stock.Product = Item(1)
            Stock.Van = ddl_Van.SelectedItem.Text
            Stock.Qty = txt_Qty.Text
            If Stock.UpdateStockRequisitionByvan(Err_No, Err_Desc, ddlOrgNew.SelectedItem.Value) = True Then
                lbl_Result.Text = "Stock Requisition Saved"
            Else
                lbl_Result.Text = "Stock Requisition could not saved"
            End If
        End If
        MpAddnew.Show()
        If Clicked.Value = "0" Then
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functiona", "showDialogue();", True)
        Else
            LoadStockSheet()
        End If

        
    End Sub

    Private Sub SearchBtn_Click(sender As Object, e As EventArgs) Handles SearchBtn.Click
        If ddl_org.SelectedIndex <= 0 Then
            MessageBoxValidation("Please select the organization", "Information")
            Label2.Text = ""
            Pnl_Content.Visible = False
            Exit Sub
        End If
        If ddlVan.CheckedItems.Count <= 0 Then
            MessageBoxValidation("Please select the van/FSR", "Information")
            Label2.Text = ""
            Pnl_Content.Visible = False
            Exit Sub
        End If
        Clicked.Value = "1"
        LoadStockSheet()
    End Sub

    Sub Export(format As String)

        Try

            
            Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
            rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), "PrintStockRequisition")
             
             
            Dim OrgID As New ReportParameter
            OrgID = New ReportParameter("OrgID", CStr(ddl_org.SelectedValue.ToString()))

             
            Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems

            Dim van As String = ""
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","
            Next

            If van = "" Then
                van = "0"
            End If

            Dim SalesRep_ID As New ReportParameter
            SalesRep_ID = New ReportParameter("Van", van)

           
            rview.ServerReport.SetParameters(New ReportParameter() {OrgID, SalesRep_ID})

            Dim mimeType As String = Nothing
            Dim encoding As String = Nothing
            Dim extension As String = Nothing
            Dim deviceInfo As String = "<DeviceInfo></DeviceInfo>"
            Dim streamids As String() = Nothing
            Dim warnings As Microsoft.Reporting.WebForms.Warning() = Nothing

            Dim bytes As Byte() = rview.ServerReport.Render(format, deviceInfo, mimeType, encoding, extension, streamids, warnings)


            Response.Clear()
            If format = "PDF" Then
                Response.ContentType = "application/pdf"
                Response.AddHeader("Content-disposition", "attachment;filename=StockRequest.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=StockRequest.xls")
                Response.AddHeader("Content-Length", bytes.Length)
            End If
            Response.OutputStream.Write(bytes, 0, bytes.Length)
            Response.OutputStream.Flush()
            Response.OutputStream.Close()
            Response.Flush()
            Response.Close()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub BtnExportPDF_Click(sender As Object, e As EventArgs) Handles BtnExportPDF.Click
        Try
            If ddl_org.SelectedIndex <= 0 Then
                MessageBoxValidation("Please select the organization", "Information")
                Exit Sub
            End If
            If ddlVan.CheckedItems.Count <= 0 Then
                MessageBoxValidation("Please select the van/FSR", "Information")
                Exit Sub
            End If
            Export("PDF")
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functiona", "showDialogue();", True)
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub

    Private Sub ddl_Van_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddl_Van.SelectedIndexChanged
        txt_Qty.Text = ""
        ddlSKU.Text = ""
        ddlSKU.SelectedIndex = 0
        MpAddnew.Show()
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functiona", "showDialogue();", True)
    End Sub

    Private Sub Closebtnimg_Click(sender As Object, e As ImageClickEventArgs) Handles Closebtnimg.Click
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "functiona", "showDialogue();", True)
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        LoadOrgHeads()
        LadVan()
        Label2.Text = ""
        Clicked.Value = "0"
        Pnl_Content.Visible = False
    End Sub
End Class