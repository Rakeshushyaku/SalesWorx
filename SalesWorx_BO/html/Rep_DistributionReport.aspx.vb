Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Imports System.Linq
Public Class Rep_DistributionReport
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As New SalesWorx.BO.Common.Common
    Dim ObjCollection As Collection
    Private ReportPath As String = "CollectionList"
    Private Const PageID As String = "P203"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
            Try
                ObjCommon = New SalesWorx.BO.Common.Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))
                txtFromDate.SelectedDate = Now.Date
                txtToDate.SelectedDate = Now.Date

                'BindData()
                If Not Err_Desc Is Nothing Then
                    log.Error(Err_Desc)
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
                End If
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

        End If
    End Sub

    Private Sub ddlOrganization_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlOrganization.SelectedIndexChanged
        If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New SalesWorx.BO.Common.Common()
            ddVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())
            ddVan.DataBind()
            ddVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))
            LoadAgency()
            LoadBrand()
            LoadSKU()
        End If
    End Sub
    Sub LoadAgency()
        ddl_Agency.DataSource = (New SalesWorx.BO.Common.Reports).GetAgency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
        ddl_Agency.DataTextField = "Agency"
        ddl_Agency.DataValueField = "Agency"
        ddl_Agency.DataBind()
        
    End Sub
    Sub LoadBrand()
        ddl_brand.DataSource = (New SalesWorx.BO.Common.Reports).GetBrandList(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
        ddl_brand.DataTextField = "Brand_Code"
        ddl_brand.DataValueField = "Brand_Code"
        ddl_brand.DataBind()
    End Sub
    Sub LoadSKU()
        Dim collection As IList(Of RadComboBoxItem) = ddl_Agency.CheckedItems

        Dim Agency As String = ""
        For Each li As RadComboBoxItem In collection
            Agency = Agency & li.Value & ","
        Next

        If Agency = "" Then
            Agency = "0"
        End If


        Dim collectionBrand As IList(Of RadComboBoxItem) = ddl_brand.CheckedItems

        Dim Brand As String = ""
        For Each li As RadComboBoxItem In collectionBrand
            Brand = Brand & li.Value & ","
        Next

        If Brand = "" Then
            Brand = "0"
        End If

        ddl_sku.DataSource = (New SalesWorx.BO.Common.Reports).GetSKU(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, Agency, Brand)
        ddl_sku.DataTextField = "Description"
        ddl_sku.DataValueField = "Inventory_Item_ID"
        ddl_sku.DataBind()
    End Sub

    Private Sub ddl_brand_ItemChecked(sender As Object, e As RadComboBoxItemEventArgs) Handles ddl_brand.ItemChecked
        LoadSKU()
    End Sub

    Private Sub ddl_Agency_ItemChecked(sender As Object, e As RadComboBoxItemEventArgs) Handles ddl_Agency.ItemChecked
        LoadSKU()
    End Sub

    Private Sub SearchBtn_Click(sender As Object, e As EventArgs) Handles SearchBtn.Click
        Dim sku As String
        sku = ddl_sku.SelectedItem.Value
    End Sub
End Class