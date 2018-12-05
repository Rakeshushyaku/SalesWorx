Imports log4net
Imports SalesWorx.BO.Common
Imports Telerik.Web.UI
Public Class AdminUserGroup
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim objVanTerritory As New SalesWorx.BO.Common.VanTerritory
    ' Dim SortField As String = ""
    Private Const ModuleName As String = "AdminUserGroup.aspx"
    Private Const PageID As String = "P382"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session.Item("USER_ACCESS") Is Nothing Then
            Session.Add("BringmeBackHere", ModuleName)
            Response.Redirect("Login.aspx", False)
            Exit Sub
        End If
        If Not IsPostBack Then
            If Not HasAuthentication() Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If

            ObjCommon = New SalesWorx.BO.Common.Common()
            Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
            ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
            ddlOrganization.DataBind()
            ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))

            LoadGroup()
        End If

    End Sub
     
     
    Sub LoadGroup()
        Try
            Dim dt1 As New DataTable
            dt1 = objLogin.GetuserGroups(Err_No, Err_Desc, txt_filter.Text.Trim)
            grduserGrp.DataSource = dt1
            grduserGrp.DataBind()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
    Sub LoadOrgDetails()
        If Not (ddlOrganization.SelectedItem.Value = "0") Then

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New SalesWorx.BO.Common.Common
            'ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
            'ddlVan.DataBind()
            'ddlVan.Items.Insert(0, New ListItem("-- Select a value --"))

            ddlVan.DataSource = ObjCommon.GetUsersForGroup(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
            ddlVan.DataTextField = "UserName"
            ddlVan.DataValueField = "UserID"
            ddlVan.DataBind()

            

        Else

            ddlVan.Items.Clear()

        End If
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Private Function HasAuthentication() As Boolean
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
    End Function
    
    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
        Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
        Dim dt As New DataTable
        dt = objLogin.GetUserGroupDetails(Err_No, Err_Desc, DirectCast(row.Cells(0).FindControl("hUG_ID"), HiddenField).Value)
        HID.Value = DirectCast(row.Cells(0).FindControl("hUG_ID"), HiddenField).Value
        btnUpdate.Visible = True
        btnSave.Visible = False
        If dt.Rows.Count > 0 Then
            ddlOrganization.Enabled = False
            txtUserGroups.Text = dt.Rows(0)("Group_Name").ToString
            If Not ddlOrganization.FindItemByValue(dt.Rows(0)("Org_HE_ID").ToString) Is Nothing Then
                ddlOrganization.FindItemByValue(dt.Rows(0)("Org_HE_ID").ToString).Selected = True
            End If
            LoadOrgDetails()
            For Each dr As DataRow In dt.Rows
                If Not ddlVan.FindItemByValue(dr("User_ID").ToString()) Is Nothing Then
                    ddlVan.FindItemByValue(dr("User_ID").ToString()).Checked = True
                End If
            Next
        End If
      
    End Sub

    Private Sub btn_search_Click(sender As Object, e As EventArgs) Handles btn_search.Click
        LoadGroup()
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click

        Try
            If txtUserGroups.Text.Trim = "" Then
                MessageBoxValidation("Please enter the group name", "Validation")
                Exit Sub
            End If

            If ddlVan.CheckedItems.Count = 0 Then
                MessageBoxValidation("Please select at lease a Van", "Validation")
                Exit Sub
            End If
            If objLogin.IfGroupNameExists(Err_No, Err_Desc, "0", txtUserGroups.Text.Trim) Then
                MessageBoxValidation("User Group name already exists", "Validation")
                Exit Sub
            End If
            Dim salesrepId As String = ""
            For Each li As RadComboBoxItem In ddlVan.CheckedItems
                salesrepId = salesrepId & li.Value & ","
            Next

            Dim ID As String = "0"
            If salesrepId.Trim <> "" Then
                salesrepId = salesrepId.Substring(0, salesrepId.Length - 1)
            End If
            If (objLogin.SaveGroup(Err_No, Err_Desc, txtUserGroups.Text.Trim, salesrepId, ID, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, ddlOrganization.SelectedItem.Value)) Then
                MessageBoxValidation("User Group saved successfully", "Information")
                LoadGroup()
                ddlOrganization.Enabled = True
                txtUserGroups.Text = ""
                ddlVan.ClearCheckedItems()
                btnSave.Visible = True
                btnUpdate.Visible = False
            Else
                MessageBoxValidation("Could not save the group.", "Information")
            End If

        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim btndelete As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btndelete.NamingContainer, GridViewRow)
            Dim ID As String = DirectCast(row.Cells(0).FindControl("hUG_ID"), HiddenField).Value
            Dim success As Boolean = False
            If objLogin.DeleteGroup(Err_No, Err_Desc, ID) Then
                MessageBoxValidation("Deleted Successfully.", "Information")
                LoadGroup()
            Else
                MessageBoxValidation("Error occured while deleting.", "Information")
            End If
        Catch ex As Exception
            log.Error(ex.ToString)
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtUserGroups.Text = ""
        ddlOrganization.Enabled = True
        ddlOrganization.ClearSelection()
        ddlVan.Items.Clear()
        btnSave.Visible = True
        btnUpdate.Visible = False
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click

        Try
            If txtUserGroups.Text.Trim = "" Then
                MessageBoxValidation("Please enter the group name", "Validation")
                Exit Sub
            End If

            If ddlVan.CheckedItems.Count = 0 Then
                MessageBoxValidation("Please select at lease a Van", "Validation")
                Exit Sub
            End If
            Dim ID As String = HID.Value

            If objLogin.IfGroupNameExists(Err_No, Err_Desc, HID.Value, txtUserGroups.Text.Trim) Then
                MessageBoxValidation("User Group name already exists", "Validation")
                Exit Sub
            End If
            Dim salesrepId As String = ""
            For Each li As RadComboBoxItem In ddlVan.CheckedItems
                salesrepId = salesrepId & li.Value & ","
            Next


            If salesrepId.Trim <> "" Then
                salesrepId = salesrepId.Substring(0, salesrepId.Length - 1)
            End If
            If (objLogin.SaveGroup(Err_No, Err_Desc, txtUserGroups.Text.Trim, salesrepId, ID, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, ddlOrganization.SelectedItem.Value)) Then
                MessageBoxValidation("User Group saved successfully", "Information")
                LoadGroup()
                ddlOrganization.Enabled = True
                txtUserGroups.Text = ""
                ddlVan.ClearCheckedItems()
                btnSave.Visible = True
                btnUpdate.Visible = False
            Else
                MessageBoxValidation("Could not save the group.", "Information")
            End If
           

        Catch ex As Exception
            log.Error(ex.Message)
        End Try

    End Sub

    Private Sub grduserGrp_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grduserGrp.PageIndexChanging
        grduserGrp.PageIndex = e.NewPageIndex
        LoadGroup()
    End Sub

    Private Sub ddlOrganization_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlOrganization.SelectedIndexChanged
        LoadOrgDetails()
    End Sub

    Private Sub btn_reset_Click(sender As Object, e As EventArgs) Handles btn_reset.Click
        txt_filter.Text = ""
        LoadGroup()
    End Sub
End Class