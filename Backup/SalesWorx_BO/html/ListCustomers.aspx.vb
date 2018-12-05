Imports SalesWorx.BO.Common
Imports System.Data
Imports System.Web
Imports System.Resources
Imports System.Web.UI.WebControls
Imports log4net
Imports System.Text.RegularExpressions
Partial Public Class ListCustomers
    Inherits System.Web.UI.Page
 Dim objSalesDistrict As New SalesDistrict
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim ObjCommon As Common
    Dim ObjCustomer As Customer

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim Dt As New DataTable
    Private Const PageID As String = "P286"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        If Not Page.IsPostBack Then
            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            If Not HasPermission Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If
            LoadOrganization()
            BindData()
        End If

    End Sub
    Sub LoadOrganization()
        Try
                ObjCommon = New Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New ListItem("-- Select a value --", "0"))
        Catch ex As Exception

        End Try
    End Sub
    
    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        MpInfoError.Show()
        Exit Sub
    End Sub
    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnSearch.Click
        If ddlOrganization.SelectedItem.Value = "0" Then
            MessageBoxValidation("Select an Organization.")
            ClassUpdatePnl.Update()
            Exit Sub
        End If
        BindData()
    End Sub

    Private Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click
         Response.Redirect("AdminCustomers.aspx")
    End Sub
    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
       
            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
            Dim LblCustomer_ID As System.Web.UI.WebControls.Label = row.FindControl("lblCustomer_ID")
            Dim LblSite_ID As System.Web.UI.WebControls.Label = row.FindControl("lblSite_Use_ID")

        
        Response.Redirect("AdminCustomers.aspx?Customer_ID=" & LblCustomer_ID.Text & "&Site_Use_ID=" & LblSite_ID.Text)
    End Sub

    Private Sub gvCurrency_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdCustomer.PageIndexChanging
        grdCustomer.PageIndex = e.NewPageIndex
        BindData()

    End Sub

    Private Sub gvCurrency_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdCustomer.Sorting
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindData()
    End Sub
    Private Property SortDirection() As String
        Get
            If ViewState("SortDirection") Is Nothing Then
                ViewState("SortDirection") = "ASC"
            End If
            Return ViewState("SortDirection").ToString()
        End Get
        Set(ByVal value As String)
            Dim s As String = SortDirection

            If value = "flip" Then
                s = If(s = "ASC", "DESC", "ASC")
            Else
                s = value
            End If

            ViewState("SortDirection") = s
        End Set
    End Property
Protected Sub lbChangeStatus_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim btnChangeStatus As LinkButton = TryCast(sender, LinkButton)
            Dim row As GridViewRow = DirectCast(btnChangeStatus.NamingContainer, GridViewRow)
            Dim LblCustomer_ID As System.Web.UI.WebControls.Label = row.FindControl("lblCustomer_ID")
            Dim LblSite_ID As System.Web.UI.WebControls.Label = row.FindControl("lblSite_Use_ID")
            ObjCustomer = New SalesWorx.BO.Common.Customer
            Dim LblStatus As System.Web.UI.WebControls.Label = row.FindControl("lblStatus")
           If ObjCustomer.SaveCustomer(Err_No, Err_Desc, 3, LblCustomer_ID.Text, LblSite_ID.Text, "", "", "", "", "", "", "", "", "", "", "", "", "", "") Then
              If LblStatus.Text = "Y" Then
                MessageBoxValidation("Customer disabled Successfully")
              Else
                MessageBoxValidation("Customer enabled Successfully")
              End If
           Else
                MessageBoxValidation("Unexpected error occured. Please contact the administrator")
           End If
            BindData()
            Catch ex As Exception

            End Try
End Sub
    Sub BindData()
        Try
        Dim dtcustomer As New DataTable
        dtcustomer = (New SalesWorx.BO.Common.Customer).GetCustomersFromSWX(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, txtCustomerName.Text, txtCustomerNo.Text)
        Dim dv As New DataView(dtcustomer)
        If Not ViewState("SortField") Is Nothing Then
           If ViewState("SortField").ToString <> "" Then
            dv.Sort = (ViewState("SortField") & " ") + SortDirection
           End If
        End If
        grdCustomer.DataSource = dv
        grdCustomer.DataBind()
        ClassUpdatePnl.Update()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Btn_Clear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_Clear.Click
        ddlOrganization.ClearSelection()
        txtCustomerName.Text = ""
        txtCustomerNo.Text = ""
        BindData()
    End Sub
End Class