

Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Public Class ListingBlockCustomers
    Inherits System.Web.UI.Page
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer
    Dim oGeocodeList As List(Of String)
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim Dt As New DataTable
    Private Const PageID As String = "P388"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim LocCount As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        If Not Page.IsPostBack Then
            LoadOrganization()
        End If
    End Sub
    Sub LoadOrganization()
        Try
            ObjCommon = New SalesWorx.BO.Common.Common
            Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
            ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
            ddlOrganization.DataBind()
            ddlOrganization.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))
        Catch ex As Exception

        End Try
    End Sub
    Sub bindGrid()
        ObjCustomer = New Customer
        Dt = ObjCustomer.GetBlockingParamsforCust(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, txt_CusName.Text.Trim, txt_CusNo.Text.Trim)
        Me.gvCustomer.DataSource = Dt
        Me.gvCustomer.DataBind()

        Dim dv As New DataView(Dt)
        If ViewState("SortField") <> "" Then
            dv.Sort = (ViewState("SortField") & " ") + SortDirection
        End If
        gvCustomer.DataSource = dv
        gvCustomer.DataBind()
    End Sub
    Private Sub gvLatitude_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvCustomer.Sorting
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        '' Dt = objLatitude.FillCusShipAddress(Err_No, Err_Desc)
        ''BindCustomerData()
        bindGrid()
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
    Private Sub gvLatitude_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvCustomer.PageIndexChanging
        gvCustomer.PageIndex = e.NewPageIndex
        ''    Dt = objLatitude.FillCusShipAddress(Err_No, Err_Desc)
        ''  BindCustomerData()
        bindGrid()

    End Sub

    Private Sub BtnFilter_Click(sender As Object, e As EventArgs) Handles BtnFilter.Click
        If ddlOrganization.SelectedIndex > 0 Then
           
            bindGrid()
        End If
    End Sub

    Private Sub BtnClearFilter_Click(sender As Object, e As EventArgs) Handles BtnClearFilter.Click
        txt_CusName.Text = ""
        txt_CusNo.Text = ""
        ObjCustomer = New Customer
        Dt = ObjCustomer.GetBlockingParamsforCust(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, txt_CusName.Text.Trim, txt_CusNo.Text.Trim)
        bindGrid()
    End Sub

    Private Sub Brn_Add_Click(sender As Object, e As EventArgs) Handles Brn_Add.Click
        Response.Redirect("BlockCustomers.aspx")
    End Sub

    Private Sub ddlOrganization_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlOrganization.SelectedIndexChanged
        ObjCustomer = New Customer
        Dt = ObjCustomer.GetBlockingParamsforCust(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, txt_CusName.Text.Trim, txt_CusNo.Text.Trim)
        bindGrid()
    End Sub
    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ObjCustomer = New Customer

        Dim btnEdit As Button = TryCast(sender, Button)
        Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
        Dim custID As String
        Dim SID As String
        custID = CType(row.FindControl("HcustID"), HiddenField).Value
        SID = CType(row.FindControl("HsiteID"), HiddenField).Value
        Dim blkParam As String = ""

        If CType(row.FindControl("chk_AB"), CheckBox).Checked Then
            blkParam = "AB"
        End If
        If CType(row.FindControl("chk_CP"), CheckBox).Checked Then
            blkParam = blkParam & ",CP"
        End If
        If CType(row.FindControl("chk_NB"), CheckBox).Checked Then
            blkParam = blkParam & ",NB"
        End If
        If ObjCustomer.UpdateBlockingCriteria(Err_No, Err_Desc, custID, SID, blkParam) Then
            MessageBoxValidation("Data saved successfully", "Information")
        Else
            MessageBoxValidation("Unexpected Error Occured while saving", "Information")
        End If
        Dt = ObjCustomer.GetBlockingParamsforCust(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, txt_CusName.Text.Trim, txt_CusNo.Text.Trim)
        bindGrid()
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
End Class