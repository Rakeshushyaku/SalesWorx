
Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Public Class BlockCustomers
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
    Private Sub BtnFilter_Click(sender As Object, e As EventArgs) Handles BtnFilter.Click
        If ddlOrganization.SelectedIndex > 0 Then
            ObjCustomer = New Customer
            Dt = ObjCustomer.GetBlockingParamsforCust(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, txt_CusName.Text.Trim, txt_CusNo.Text.Trim)
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
    Sub bindGrid()
        Me.lst_Customer.DataSource = Dt
        lst_Customer.DataTextField = "Customer"
        lst_Customer.DataValueField = "Customer_ID"
        Me.lst_Customer.DataBind()

         
    End Sub

    Private Sub Btn_Save_Click(sender As Object, e As EventArgs) Handles Btn_Save.Click
        If lst_Customer.SelectedIndex < 0 Then
            MessageBoxValidation("Please select any customer", "Validation")
            Exit Sub
        End If
        If chk_BlockPramar.SelectedIndex < 0 Then
            MessageBoxValidation("Please select any Blocking Criteria", "Validation")
            Exit Sub
        End If
        Dim StrBlock As String = ""
        For Each li As ListItem In chk_BlockPramar.Items
            If li.Selected = True Then
                StrBlock = StrBlock & li.Value & ","
            End If
        Next
        If StrBlock.Trim <> "" Then
            StrBlock = StrBlock.Substring(0, StrBlock.Length - 1)
        End If
        ObjCustomer = New Customer
        Dim bSaved As Boolean = True
        For Each li As ListItem In lst_Customer.Items
            If li.Selected = True Then
                Dim CustomerID As String
                Dim SiteUSeID As String
                Dim ids() As String
                ids = li.Value.Split("$")
                CustomerID = ids(0)
                SiteUSeID = ids(1)
                If ObjCustomer.UpdateBlockingCriteria(Err_No, Err_Desc, CustomerID, SiteUSeID, StrBlock) Then
                    If bSaved = True Then
                        bSaved = True
                    End If
                Else
                    bSaved = False
                End If

            End If
        Next
        If bSaved = True Then
            MessageBoxValidation("Data saved successfully", "Information")
        Else
            MessageBoxValidation("Unexpected Error Occured while saving", "Information")
        End If
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Private Sub ddlOrganization_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlOrganization.SelectedIndexChanged
        ObjCustomer = New Customer
        Dt = ObjCustomer.GetBlockingParamsforCust(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, txt_CusName.Text.Trim, txt_CusNo.Text.Trim)
        bindGrid()
    End Sub

    Private Sub Btn_Cancel_Click(sender As Object, e As EventArgs) Handles Btn_Cancel.Click
        Response.Redirect("ListingBlockCustomers.aspx")
    End Sub
End Class
