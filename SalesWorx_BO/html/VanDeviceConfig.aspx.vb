Imports SalesWorx.BO.Common
Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports Telerik.Web.UI
Partial Public Class VanDeviceConfig
    Inherits System.Web.UI.Page
    Dim objProduct As New Product
    Dim objCommon As New Common
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const PageID As String = "P261"
    Dim objLogin As New SalesWorx.BO.Common.Login
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub VanDeviceConfig_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Van Device Configuration"
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
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

                Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddOraganisation.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddOraganisation.Items.Clear()
                ddOraganisation.Items.Add(New RadComboBoxItem("-- Select a Organization --"))
                ddOraganisation.AppendDataBoundItems = True
                ddOraganisation.DataValueField = "MAS_Org_ID"
                ddOraganisation.DataTextField = "Description"
                ddOraganisation.DataBind()

                
            

            End If
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_MSL_005") & "&next=Welcome.aspx&Title=Product List Setup", False)
        End Try

    End Sub

    Protected Sub ddOraganisation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddOraganisation.SelectedIndexChanged
        'Try
        If ddOraganisation.SelectedItem.Text <> "-- Select a Organization --" Then
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

            ddlSalesRep.DataSource = objCommon.GetVanByOrg(Err_No, Err_Desc, ddOraganisation.SelectedValue, objUserAccess.UserID.ToString())
            ddlSalesRep.Items.Clear()
            ddlSalesRep.DataValueField = "SalesRep_Id"
            ddlSalesRep.DataTextField = "SalesRep_Name"
            ddlSalesRep.DataBind()
        Else
            ddlSalesRep.Items.Clear()
             
            ddlSalesRep.DataValueField = "SalesRep_Id"
            ddlSalesRep.DataTextField = "SalesRep_Name"
            ddlSalesRep.DataBind()

        End If
        BindData()
    End Sub
    Protected Sub gvConfig_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvConfig.PageIndexChanging
        gvConfig.PageIndex = e.NewPageIndex
        BindData()

    End Sub

    Protected Sub gvConfig_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvConfig.Sorting
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
    Private Sub BindData()

        Dim TempTbl As New DataTable
        Try

            Dim van As String = ""
            Dim collection As IList(Of RadComboBoxItem) = ddlSalesRep.CheckedItems
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","


            Next
            If van = "" Then
                van = "0"
            End If

            TempTbl = objCommon.GetDeviceConfig(Err_No, Err_Desc, van)

            Dim dv As New DataView(TempTbl)
            If ViewState("SortField") <> "" Then
                dv.Sort = (ViewState("SortField") & " ") + SortDirection
            End If
            gvConfig.DataSource = dv
            gvConfig.DataBind()

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        Finally

        End Try
    End Sub


   

    Private Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Me.ddlSalesRep.ClearCheckedItems()
        Me.ddOraganisation.SelectedValue = "0"

        BindData()
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If Me.ddOraganisation.SelectedIndex <= 0 Or Me.ddlSalesRep.CheckedItems.Count <= 0 Then
            MessageBoxValidation("Please select a organization and van", "Validation")

            Exit Sub
        End If

        If gvConfig.Rows.Count <= 0 Then
            MessageBoxValidation("There is no data available to update", "Validation")
            
            Exit Sub
        End If

        Dim s As Boolean = False

        For Each gvr As GridViewRow In gvConfig.Rows
            Dim RowID As Label = DirectCast(gvr.FindControl("lblRowID"), Label)
            Dim ConfigVal As TextBox = DirectCast(gvr.FindControl("txtConfigValue"), TextBox)

            If ConfigVal.Text = "" Then
               
                MessageBoxValidation("Please enter the config value at row no " + (gvr.RowIndex + 1).ToString(), "Validation")

                Exit Sub
            End If
            If objCommon.UpdateDeviceConfig(Err_No, Err_Desc, RowID.Text, ConfigVal.Text, CType(Session("User_Access"), UserAccess).UserID) = True Then
                s = True
            End If
           
        Next
        If s = True Then
            MessageBoxValidation("Successfully updated", "Information")
           
        End If
        BindData()
    End Sub

    Private Sub BtnSearch_Click(sender As Object, e As EventArgs) Handles BtnSearch.Click
        If Me.ddOraganisation.SelectedIndex <= 0 Or Me.ddlSalesRep.CheckedItems.Count <= 0 Then
            MessageBoxValidation("Please select a organization and van", "Validation")

            Exit Sub
        End If
        BindData()
    End Sub
End Class