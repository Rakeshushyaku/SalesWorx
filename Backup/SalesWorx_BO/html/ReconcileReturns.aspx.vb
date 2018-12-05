Imports log4net
Imports SalesWorx.BO.Common
Imports System.IO
Imports Telerik.Web.UI
Partial Public Class ReconcileReturns
    Inherits System.Web.UI.Page
  Private Const ModuleName As String = "ReconcileReturns.aspx"
    Private Const PageID As String = "P292"
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim objUser As New User
    Dim objCrypt As New Crypto
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objCommon As New Common
    Dim objProduct As New Product
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Not Session.Item("USER_ACCESS") Is Nothing Then

                If Not HasAuthentication() Then
                    Err_No = 500
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
                End If

                 Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.Items.Clear()
                ddlOrganization.Items.Add(New ListItem("-- Select a Organization --", "0"))
                ddlOrganization.AppendDataBoundItems = True
                ddlOrganization.DataValueField = "MAS_Org_ID"
                ddlOrganization.DataTextField = "Description"
                ddlOrganization.DataBind()
                txtfromDate.Text = Now.ToString("dd-MMM-yyyy")
                txtToDate.Text = Now.ToString("dd-MMM-yyyy")
            Else
                Session.Add("BringmeBackHere", ModuleName)
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
        End If
        lblMsg.Text = ""
    End Sub

    Private Function HasAuthentication() As Boolean
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
    End Function
   Private Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrganization.SelectedIndexChanged

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ddl_customer.DataSource = objCommon.GetCustomerfromOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue)

            ddl_customer.Items.Clear()
            ddl_customer.DataValueField = "CustomerID"
            ddl_customer.DataTextField = "Customer"
            ddl_customer.DataBind()
            ddl_customer.Items.Insert(0, New ListItem("-- Select --", "-1"))
            Pnl_details.Visible = False
    End Sub

Protected Sub Btn_Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Btn_Search.Click
        If ddlOrganization.SelectedItem.Value = "0" Then
             MessageBoxValidation("Select an organization", "Validation")
             Exit Sub
        End If
        If ddl_customer.SelectedItem.Value = "-1" Then
             MessageBoxValidation("Select a customer", "Validation")
             Exit Sub
        End If
        BindReturns()

End Sub
Function validDates() As Boolean
    Dim bretval As Boolean = False
    If IsDate(txtfromDate.Text) Then
        bretval = True
    Else
        MessageBoxValidation("Invalid From date", "Validation")
        bretval = False
    End If
    If IsDate(txtToDate.Text) Then
     If bretval = True Then bretval = True
    Else
        MessageBoxValidation("Invalid To date", "Validation")
        bretval = False
    End If
    If CDate(txtfromDate.Text) <= CDate(txtToDate.Text) Then
         If bretval = True Then bretval = True
     Else
        MessageBoxValidation("From date greater than To date", "Validation")
        bretval = False
    End If
    Return bretval
End Function
Sub BindReturns()
    If validDates() Then
    txtAmt.Text = ""
    Dim Selcustomer As String
    Selcustomer = ddl_customer.SelectedItem.Value
    Dim ids() As String
    ids = Selcustomer.Split("$")
    Dim dt As New DataTable
    dt = (New SalesWorx.BO.Common.Customer).GetOpenReturns(Err_No, Err_Desc, ids(0), ids(1), txtfromDate.Text, txtToDate.Text)
    Grd_Returns.DataSource = dt
    Grd_Returns.DataBind()
    If dt.Rows.Count > 0 Then
        Pnl_details.Visible = True
        Dim dtOrder As New DataTable
        dtOrder = (New SalesWorx.BO.Common.Customer).GetOpenInvoices(Err_No, Err_Desc, ids(0), ids(1), txtfromDate.Text, txtToDate.Text)
        Grd_Orders.DataSource = dtOrder
        Grd_Orders.DataBind()
    Else
        Grd_Orders.DataSource = Nothing
        Grd_Orders.DataBind()
        MessageBoxValidation("Unsettled Returns do not exist", "Information")
        Pnl_details.Visible = False
    End If

    Else
        Grd_Returns.DataSource = Nothing
        Grd_Returns.DataBind()
        Grd_Orders.DataSource = Nothing
        Grd_Orders.DataBind()
    End If
    ClassUpdatePnl1.Update()
End Sub
  Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = Title
        lblMessage.Text = str
        MpInfoError.Show()

        Exit Sub
    End Sub

    Private Sub Grd_Returns_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles Grd_Returns.ItemCommand
        If e.CommandName = RadGrid.FilterCommandName Then
        If validDates() Then
            Dim Selcustomer As String
    Selcustomer = ddl_customer.SelectedItem.Value
    Dim ids() As String
    ids = Selcustomer.Split("$")
    Dim dt As New DataTable
    dt = (New SalesWorx.BO.Common.Customer).GetOpenReturns(Err_No, Err_Desc, ids(0), ids(1), txtfromDate.Text, txtToDate.Text)
    Grd_Returns.DataSource = dt
    Grd_Returns.DataBind()
     txtAmt.Text = ""
    End If
        End If
    End Sub
    Private Sub Grd_Orders_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles Grd_Orders.ItemCommand
    If e.CommandName = RadGrid.FilterCommandName Then
        If validDates() Then
            Dim Selcustomer As String
    Selcustomer = ddl_customer.SelectedItem.Value
    Dim ids() As String
    ids = Selcustomer.Split("$")
    Dim dt As New DataTable
    dt = (New SalesWorx.BO.Common.Customer).GetOpenInvoices(Err_No, Err_Desc, ids(0), ids(1), txtfromDate.Text, txtToDate.Text)
    Grd_Orders.DataSource = dt
    Grd_Orders.DataBind()
 txtAmt.Text = ""
    End If
    End If
    End Sub

    Private Sub Grd_Orders_PageIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles Grd_Orders.PageIndexChanged
If validDates() Then
        Dim Selcustomer As String
    Selcustomer = ddl_customer.SelectedItem.Value
    Dim ids() As String
    ids = Selcustomer.Split("$")
    Dim dt As New DataTable
    dt = (New SalesWorx.BO.Common.Customer).GetOpenInvoices(Err_No, Err_Desc, ids(0), ids(1), txtfromDate.Text, txtToDate.Text)
    Grd_Orders.DataSource = dt
    Grd_Orders.DataBind()
     txtAmt.Text = ""
    End If
    End Sub

    Private Sub Grd_Returns_PageIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles Grd_Returns.PageIndexChanged
    If validDates() Then
    Dim Selcustomer As String
    Selcustomer = ddl_customer.SelectedItem.Value
    Dim ids() As String
    ids = Selcustomer.Split("$")
    Dim dt As New DataTable
    dt = (New SalesWorx.BO.Common.Customer).GetOpenReturns(Err_No, Err_Desc, ids(0), ids(1), txtfromDate.Text, txtToDate.Text)
    Grd_Returns.DataSource = dt
    Grd_Returns.DataBind()
     txtAmt.Text = ""
    End If
    End Sub


    Private Sub Btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_Save.Click
      If Grd_Orders.SelectedItems.Count = 0 Then
          MessageBoxValidation("Please select an Invoice", "Validation")
          Exit Sub
      End If
      If Grd_Returns.SelectedItems.Count = 0 Then
          MessageBoxValidation("Please select the return", "Validation")
          Exit Sub
      End If
        Dim orderselected As GridDataItem
        orderselected = Grd_Orders.SelectedItems(0)

        Dim returnselected As GridDataItem
        returnselected = Grd_Returns.SelectedItems(0)

        Dim orderamt, returnamt As String
        orderamt = orderselected.Cells(4).Text
        returnamt = returnselected.Cells(4).Text
        If Val(txtAmt.Text) > Val(orderamt) Or Val(txtAmt.Text) > Val(returnamt) Then
            MessageBoxValidation("You can not settle this amount", "Validation")
            Exit Sub
        End If
        MPSettle.Show()
    End Sub

    Private Sub btn_Yes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Yes.Click
    Dim Selcustomer As String
    Selcustomer = ddl_customer.SelectedItem.Value
    Dim ids() As String
    ids = Selcustomer.Split("$")

     Dim orderselected As GridDataItem
     orderselected = Grd_Orders.SelectedItems(0)

     Dim returnselected As GridDataItem
     returnselected = Grd_Returns.SelectedItems(0)

     Dim orderamt, returnamt As String
     orderamt = orderselected.Cells(4).Text
     returnamt = returnselected.Cells(4).Text


     Dim orderNo, returnNo As String
     orderNo = orderselected.Cells(2).Text
     returnNo = returnselected.Cells(2).Text
    If (New SalesWorx.BO.Common.Customer).SaveSettlement(Err_No, Err_Desc, ids(0), ids(1), CType(Session("User_Access"), UserAccess).UserID, returnNo, orderNo, txtAmt.Text, returnamt, orderamt) Then
        MessageBoxValidation("The Settlement is saved successfully", "Information")
        BindReturns()
    Else
        MessageBoxValidation("The Settlement could not be saved. Please contact the administrator", "Information")
    End If

    End Sub

    Private Sub ddl_customer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_customer.SelectedIndexChanged
         Pnl_details.Visible = False
    End Sub
End Class