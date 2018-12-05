
Imports SalesWorx.BO.Common
Imports Telerik.Web.UI
Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports ExcelLibrary.SpreadSheet
Partial Public Class BonusDefinitionByValue
    Inherits System.Web.UI.Page
    Dim objProduct As New Product


    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const PageID As String = "P220"
    Private dt As New DataTable
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objControl As New AppControl

 

    Private Sub BonusDefinition_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Bonus Definition"
    End Sub
  


 


  
   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        Try
            If Not IsPostBack Then
                Dim HasPermission As Boolean = False
                ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
                If Not HasPermission Then
                    Err_No = 500
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
                End If





                'Me.txtBItemCode.Enabled = False
                'Me.txtBDescription.Enabled = False
                'Me.txtItemCode.Enabled = False
                'Me.txtDescription.Enabled = False



                LoadOrgHeads()

                ResetDetails()

                Me.lblPlanName.Text = Request.QueryString("Desc").ToString()
                Me.hfPlanId.Value = Request.QueryString("PGID").ToString()

                Me.lblOrg.Text = Request.QueryString("ORGNAME").ToString()
                Me.hfOrgID.Value = Request.QueryString("ORGID").ToString()
                Me.ddl_org.SelectedValue = hfOrgID.Value

                rgRules.Rebind()


         
            End If


        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Order_005") & "&next=Welcome.aspx&Title=Bonus Definition", False)
        End Try
    End Sub

   
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
   
   

  
    

    Private Sub ddlgetDesc_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddlgetDesc.ItemsRequested


        Dim Objrep As New SalesWorx.BO.Common.Reports()
        Dim dt As New DataTable
        dt = objProduct.LoadBonusProductList(Err_No, Err_Desc, hfOrgID.Value, e.Text)


        Dim ItemsPerRequest As Integer = 100 'Set the number of values to show
        Dim itemOffset As Integer = e.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
        e.EndOfItems = endOffset = dt.Rows.Count

        'Loop through the values to populate the combo box
        For i As Integer = itemOffset To endOffset - 1
            Dim item As New RadComboBoxItem()
            item.Text = dt.Rows(i).Item("DescText").ToString
            item.Value = dt.Rows(i).Item("CodeValue").ToString

            ddlgetDesc.Items.Add(item)
            item.DataBind()
        Next
    End Sub

  

   
    Sub LoadOrgHeads()
        'ddl_org.DataSource = objProduct.GetOrgsHeads(Err_No, Err_Desc)
        'ddl_org.DataTextField = "Description"
        'ddl_org.DataValueField = "ORG_HE_ID"
        'ddl_org.DataBind()
        'ddl_org.Items.Insert(0, "--Select Organisation--")
        'ddl_org.Items(0).Value = 0

        Dim objCommon As New Common
        Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
        ddl_org.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
        ddl_org.Items.Clear()
        ddl_org.Items.Add(New RadComboBoxItem("-- Select a Organization --"))
        ddl_org.AppendDataBoundItems = True
        ddl_org.DataValueField = "MAS_Org_ID"
        ddl_org.DataTextField = "Description"
        ddl_org.DataBind()

    End Sub

  

  


   

   













    Protected Sub btnAddItems_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddItems.Click
        If ValidationDetails() = False Then
            Try



                Err_No = Nothing
                Err_Desc = Nothing
                Dim success As Boolean
                Dim fdate As String = Me.StartTime.SelectedDate.Value.ToString("dd-MM-yyyy")
                Dim tdate As String = Me.EndTime.SelectedDate.Value.ToString("dd-MM-yyyy")

               
                Dim ItemCode As String = Me.ddlgetDesc.SelectedValue


                'Check Item exist
                If btnAddItems.Text = "Add" Or (btnAddItems.Text = "Update" And (Me.txtInvAmount.Text <> Me.lblF.Text Or Me.txtMinItems.Text <> Me.lblT.Text Or fdate <> Me.lblVF.Text Or tdate <> Me.lblVT.Text)) Then


                    If objProduct.CheckBonusInvValueActiveRange(Err_No, Err_Desc, Me.txtInvAmount.Text, Me.txtMinItems.Text, Me.hfOrgID.Value, Me.StartTime.SelectedDate.Value, Me.EndTime.SelectedDate.Value, CInt(IIf(Me.lblLineId.Text = "", "0", Me.lblLineId.Text)), hfPlanID.Value) = True Then
                        MessageBoxValidation("The same bonus rule already exists", "Validation")
                        Exit Sub
                    End If
                End If

                If Me.btnAddItems.Text = "Add" Then
                    success = objProduct.SaveInvoiceRule(Err_No, Err_Desc, Me.txtInvAmount.Text, Me.txtMinItems.Text, Me.hfOrgID.Value, Me.ddlgetDesc.SelectedValue, Me.txtGetQty.Text, Me.StartTime.SelectedDate.Value, Me.EndTime.SelectedDate.Value, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, IIf(hfPlanID.Value = "", "0", Me.hfPlanID.Value))

                ElseIf Me.btnAddItems.Text = "Update" Then
                    success = objProduct.UpdateInvoiceRule(Err_No, Err_Desc, Me.txtInvAmount.Text, Me.txtMinItems.Text, Me.hfOrgID.Value, Me.ddlgetDesc.SelectedValue, Me.txtGetQty.Text, Me.StartTime.SelectedDate.Value, Me.EndTime.SelectedDate.Value, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, IIf(hfPlanID.Value = "", "0", Me.hfPlanID.Value), lblLineId.Text)
                End If
                If success = True Then
                    MessageBoxValidation("Successfully saved.", "Information")
                    Me.btnAddItems.Text = "Add"
                    Me.txtMinItems.Text = ""
                    Me.txtInvAmount.Text = ""
                    Me.txtGetQty.Text = ""
                    Me.StartTime.Enabled = True
                    Me.EndTime.Enabled = True
                    Me.ddlgetDesc.ClearSelection()
                    Me.ddlgetDesc.Text = ""
                    rgRules.Rebind()
                Else
                    MessageBoxValidation("Error while saving bonus details", "Information")
                    Exit Sub
                End If

            Catch ex As Exception
                log.Error(ex.Message)
                Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_OrderEntry_006") & "&next=BonusDefinitionByValue.aspx&Title=Bonus Definition", False)
            End Try
        End If
    End Sub
    Function ValidDate() As Boolean
        'Dim bRetVal As Boolean = False
        'Dim dt As New DataTable
        'dt = objProduct.CheckIfBonusDateCanbeChanged(Err_No, Err_Desc, Me.hfPlanID.Value, Me.hfOrgID.Value, Me.StartTime.SelectedDate.Value, Me.EndTime.SelectedDate.Value)
        'If dt.Rows.Count > 0 Then
        '    bRetVal = False
        'Else
        '    bRetVal = True
        'End If
        'Return bRetVal
    End Function
    Protected Sub ResetBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClear.Click
        ResetDetails()

    End Sub

   

    Private Sub ResetDetails()
      
        Me.txtGetQty.Text = ""
        Me.txtInvAmount.Text = ""
        Me.txtMinItems.Text = ""
   
        Me.btnAddItems.Text = "Add"
      
        Me.ddlgetDesc.ClearSelection()
        Me.ddlgetDesc.Text = ""

        
        Me.StartTime.SelectedDate = Now.Date.AddDays(1)
        Me.EndTime.SelectedDate = "2100-12-31"
        Me.StartTime.Enabled = True
        Me.EndTime.Enabled = True
       
        Me.lblOrgId.Text = ""
        Me.lblLineId.Text = ""
        Me.lblF.Text = ""
        Me.lblT.Text = ""
        Me.lblVF.Text = ""
        Me.lblVT.Text = ""

    
    End Sub
   
    Private Function ValidationDetails() As Boolean
        Dim sucess As Boolean = False



        If Me.hfOrgID.Value Is Nothing And btnAddItems.Text = "Add" Then
            MessageBoxValidation("Please select a organization", "Validation")
            sucess = True

            Return sucess
            Exit Function
        End If
        If Me.txtInvAmount.Text = "" Or Me.txtInvAmount.Text = "0" Then
            MessageBoxValidation("Please enter minimum invoice value", "Validation")
            sucess = True

            Return sucess
            Exit Function
        End If
        If Me.txtMinItems.Text = "" Or Me.txtMinItems.Text = "0" Then
            MessageBoxValidation("Please enter minimum no.of items order", "Validation")
            sucess = True

            Return sucess
            Exit Function
        End If
        If Me.txtGetQty.Text = "" Or Me.txtGetQty.Text = "0" Then
            MessageBoxValidation("Please enter FOC Quantity", "Validation")
            sucess = True

            Return sucess
            Exit Function
        End If

        If IsNothing(Me.ddlgetDesc.SelectedValue) Then
            MessageBoxValidation("Please select an FOC item", "Validation")
            sucess = True
            Return sucess
            Exit Function
        End If

        If (Me.StartTime.SelectedDate.Value <= Now.Date() And Me.StartTime.Enabled = True) Or Me.EndTime.SelectedDate.Value <= Now.Date() Then
            MessageBoxValidation("Valid from and to date should be greater than current date", "Validation")
            sucess = True

            Return sucess
            Exit Function
        End If
        Return sucess
    End Function





  
  







    Protected Sub dgv_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles rgRules.ItemCommand



        Try
         
            If (e.CommandName = "EditRule") Then

               
                Dim row As GridDataItem = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridDataItem)
                Dim lblRule As Label = DirectCast(row.FindControl("lblRule"), Label)
                Dim lblMinValue As Label = DirectCast(row.FindControl("lblMinValue"), Label)
                Dim lblMinItem As Label = DirectCast(row.FindControl("lblMinItem"), Label)
                Dim lblBCode As Label = DirectCast(row.FindControl("lblBCode"), Label)
                Dim lblBQty As Label = DirectCast(row.FindControl("lblBQty"), Label)

                Dim lblValidFrom As Label = DirectCast(row.FindControl("lblValidFrom"), Label)
                Dim lblValidTo As Label = DirectCast(row.FindControl("lblValidTo"), Label)

                Me.txtInvAmount.Text = CDec(lblMinValue.Text)
                Me.txtMinItems.Text = CInt(lblMinItem.Text)
                Me.lblLineId.Text = lblRule.Text

                Dim Objrep As New SalesWorx.BO.Common.Reports()
                Dim dt As New DataTable
                dt = objProduct.LoadBonusProductList(Err_No, Err_Desc, hfOrgID.Value, "")


                ddlgetDesc.DataSource = dt
                ddlgetDesc.DataValueField = "CodeValue"
                ddlgetDesc.DataTextField = "DescText"

                ddlgetDesc.DataBind()

                Me.ddlgetDesc.SelectedValue = lblBCode.Text
                Me.txtGetQty.Text = CLng(lblBQty.Text)
                Me.StartTime.SelectedDate = lblValidFrom.Text
                Me.EndTime.SelectedDate = lblValidTo.Text
                If Me.StartTime.SelectedDate <= Now.Date Then
                    Me.StartTime.Enabled = False
                Else
                    Me.StartTime.Enabled = True
                End If

                Me.lblF.Text = Me.txtInvAmount.Text
                Me.lblT.Text = Me.txtMinItems.Text
                Me.lblVF.Text = Me.StartTime.SelectedDate.Value.ToString("dd-MM-yyyy")
                Me.lblVT.Text = Me.EndTime.SelectedDate.Value.ToString("dd-MM-yyyy")

                Me.btnAddItems.Text = "Update"
            End If

            If (e.CommandName = "DeleteRule") Then
                Dim row As GridDataItem = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridDataItem)
                Dim lblRule As Label = DirectCast(row.FindControl("lblRule"), Label)
                If objProduct.DeleteInvoiceRule(Err_No, Err_Desc, hfPlanID.Value, lblRule.Text, CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
                    MessageBoxValidation("Successfully deleted", "Information")
                    rgRules.Rebind()
                    ResetDetails()
                End If
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("information.aspx?mode=1&errno=" & "74071" & "&msg=" & AppMsgHandler.GetErrorMessage("SWX_BO_Msg_005") & "&next=BonusDefinitionByValue.aspx", False)
        Finally
        End Try

    End Sub


    Protected Sub RadGrid1_RowDrop(ByVal sender As Object, ByVal e As GridDragDropEventArgs)
        If String.IsNullOrEmpty(e.HtmlElement) Then

            If e.DestDataItem IsNot Nothing AndAlso e.DestDataItem.OwnerGridID = rgRules.ClientID Then

                'reorder items in pending grid



                Dim Dest As String = ""
                Dim RuleID As Integer = e.DestDataItem.GetDataKeyValue("Rule_ID")
                Dest = RuleID.ToString()

                Dim source As String = ""
                source = rgRules.SelectedValues("Rule_ID").ToString()

                If source <> "" And Dest <> "" Then
                    objProduct.SwapInvoiceRule(Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, hfPlanID.Value, source, Dest)
                    rgRules.Rebind()
                End If
            End If
        End If

    End Sub


   

    
    Protected Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        Response.Redirect("AdminBonusSimple.aspx?OID=", False)
    End Sub
End Class





