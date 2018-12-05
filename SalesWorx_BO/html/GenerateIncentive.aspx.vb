Imports log4net
Imports SalesWorx.BO.Common
Imports System.IO
Imports Telerik.Web.UI
Public Class GenerateIncentive
    Inherits System.Web.UI.Page

    Private Const ModuleName As String = "GenerateIncentive.aspx"
    Private Const PageID As String = "P153"
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim objUser As New User
    Dim objCrypt As New Crypto
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objCommon As New Common
    Dim objIncentive As New Incentive
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Not Session.Item("USER_ACCESS") Is Nothing Then

                If Not HasAuthentication() Then
                    Err_No = 500
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
                End If

                Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = (New SalesWorx.BO.Common.Product).GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.Items.Clear()
                ddlOrganization.Items.Add(New RadComboBoxItem("Select Organization", "0"))
                ddlOrganization.AppendDataBoundItems = True
                ddlOrganization.DataValueField = "MAS_Org_ID"
                ddlOrganization.DataTextField = "Description"
                ddlOrganization.DataBind()
                lbl_Month.Text = Now.AddMonths(-1).ToString("MMM-yyyy")

                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                End If
            Else
                Session.Add("BringmeBackHere", ModuleName)
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
        Else
            MPSettle.VisibleOnPageLoad = False
        End If
        lblMsg.Text = ""
    End Sub

    Private Function HasAuthentication() As Boolean
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
    End Function
    Private Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrganization.SelectedIndexChanged
        If ddlOrganization.SelectedIndex > 0 Then
            Dim dt As New DataTable
            dt = objIncentive.GetIncentiveGenerated(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, Now.AddMonths(-1).Month, Now.AddMonths(-1).Year)
            If dt.Rows.Count > 0 Then
                Btn_Generate.Enabled = False
                BindGrid(dt)
            Else

                gvRep.Visible = False
                Btn_Generate.Enabled = True
            End If

        Else
            gvRep.Visible = False

            Btn_Generate.Enabled = False
        End If
    End Sub
    Sub BindGrid(dt)
        gvRep.Visible = True
        gvRep.DataSource = dt
        gvRep.DataBind()
    End Sub
    Private Sub dgv_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvRep.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        Dim dt As New DataTable
        dt = objIncentive.GetIncentiveGenerated(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, Now.AddMonths(-1).Month, Now.AddMonths(-1).Year)
        BindGrid(dt)
    End Sub
    Private Sub dgv_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        Dim dt As New DataTable
        dt = objIncentive.GetIncentiveGenerated(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, Now.AddMonths(-1).Month, Now.AddMonths(-1).Year)
        BindGrid(dt)
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
    Private Sub Btn_Generate_Click(sender As Object, e As EventArgs) Handles Btn_Generate.Click
        Try
         
            If ddlOrganization.SelectedIndex > 0 Then
                Dim dtuser As New DataTable
                dtuser = objIncentive.GetUsersNotFullSynced(Err_Desc, Err_Desc, ddlOrganization.SelectedItem.Value, Now.Month, Now.Year)
                If dtuser.Rows.Count > 0 Then
                    rg_users.DataSource = dtuser
                    rg_users.DataBind()
                    MPSettle.VisibleOnPageLoad = True
                Else
                    If objIncentive.GenerateIncentive(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value) = True Then
                        MessageBoxValidation("Incentive is generated for " & lbl_Month.Text, "Validation")
                        Dim dt As New DataTable
                        dt = objIncentive.GetIncentiveGenerated(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, Now.AddMonths(-1).Month, Now.AddMonths(-1).Year)
                        BindGrid(dt)
                        Btn_Generate.Enabled = False
                    Else
                        MessageBoxValidation("Unexpexted error occured", "Validation")
                    End If
                End If
            Else
                MessageBoxValidation("Please select the organization", "Validation")
            End If
        Catch ex As Exception
            log.Debug(ex.ToString)
        End Try
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Private Sub btn_Yes_Click(sender As Object, e As EventArgs) Handles btn_Yes.Click
        If objIncentive.GenerateIncentive(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value) = True Then
            MPSettle.VisibleOnPageLoad = False
            MessageBoxValidation("Incentive is generated for " & lbl_Month.Text, "Validation")
            Dim dt As New DataTable
            dt = objIncentive.GetIncentiveGenerated(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, Now.AddMonths(-1).Month, Now.AddMonths(-1).Year)
            BindGrid(dt)
            Btn_Generate.Enabled = False
        Else
            MPSettle.VisibleOnPageLoad = False
            MessageBoxValidation("Unexpexted error occured", "Validation")
        End If
    End Sub

    Private Sub rg_users_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles rg_users.PageIndexChanged
        Dim dtuser As New DataTable
        dtuser = objIncentive.GetUsersNotFullSynced(Err_Desc, Err_Desc, ddlOrganization.SelectedItem.Value, Now.Month, Now.Year)
        rg_users.DataSource = dtuser
        rg_users.DataBind()
        MPSettle.VisibleOnPageLoad = True
    End Sub
End Class