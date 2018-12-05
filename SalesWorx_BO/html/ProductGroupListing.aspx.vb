
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports Telerik.Web.UI
Partial Public Class ProductGroupListing
    Inherits System.Web.UI.Page
Private dtErrors As New DataTable
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const ModuleName As String = "AreaVanMapping.aspx"
    Private Const PageID As String = "P376"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim objProduct As New Product
    Dim dtSearch As New DataTable
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

               Dim HasPermission As Boolean = False
                ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
                If Not HasPermission Then
                    Err_No = 500
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
                End If

                 BindData()
            End If
    End Sub
Private Function HasAuthentication() As Boolean
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
    End Function
    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFilter.Click
        BindData()
    End Sub



    Private Sub BindData()
        Dim obj As New Product
        Try
            If dtSearch.Rows.Count > 0 Then
                dtSearch.Rows.Clear()
            End If
            dtSearch = obj.GetProductGroup(Err_No, Err_Desc)

            Dim dv As New DataView(dtSearch)
            If ViewState("SortField") <> "" Then
                dv.Sort = (ViewState("SortField") & " ") + SortDirection
            End If
            Me.dgv.DataSource = dv
            Me.dgv.DataBind()
            Session.Remove("GroupID")
            Session("GroupID") = dtSearch
            Me.ClassUpdatePnl.Update()


            If Me.ddFilterBy.SelectedIndex = 2 Then
                If Me.txtFilterValue.Text <> "" Then
                    dtSearch.DefaultView.RowFilter = "(CreatedBy LIKE '%" & Me.txtFilterValue.Text & "%')"
                    '  Me.txtFilterValue.Text = ""
                End If
            End If

            If Me.ddFilterBy.SelectedIndex = 1 Then
                If Me.txtFilterValue.Text <> "" Then
                    dtSearch.DefaultView.RowFilter = "(Description LIKE '%" & Me.txtFilterValue.Text & "%')"
                    ' Me.txtFilterValue.Text = ""
                End If
            End If




            dtSearch.AcceptChanges()

            dv = dtSearch.DefaultView
            If ViewState("SortField") <> "" Then
                dv.Sort = (ViewState("SortField") & " ") + SortDirection
            End If
            Me.dgv.DataSource = Nothing
            Me.dgv.DataSource = dv
            Me.dgv.DataBind()
            Me.TopPanel.Update()
            Me.ClassUpdatePnl.Update()
            Session.Remove("GroupID")
            Session("GroupID") = dtSearch

        Catch ex As Exception
            Throw ex
        Finally
            obj = Nothing
        End Try
    End Sub

    Protected Sub dgv_ItemDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles dgv.ItemDataBound
        If e.Item.ItemType.Equals(DataControlRowType.Pager) Then
            Dim pTableRow As TableRow = _
                     CType(e.Item.Cells(0).Controls(0).Controls(0), TableRow)
            For Each cell As TableCell In pTableRow.Cells
                For Each control As Control In cell.Controls
                    If TypeOf control Is LinkButton Then
                        Dim lb As LinkButton = CType(control, LinkButton)
                        'lb.Attributes.Add("onclick", "ScrollToTop();")
                    End If
                Next
            Next
        End If
    End Sub
    Protected Sub dgv_PageSizeChanged(ByVal source As Object, ByVal e As GridPageSizeChangedEventArgs) Handles dgv.PageSizeChanged
        BindData()

    End Sub


    Private Sub dgv_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles dgv.PageIndexChanged
        BindData()
    End Sub



    Private Sub dgv_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles dgv.SortCommand
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


    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click

        Response.Redirect("AdminProductGroup.aspx?Desc=New&PGID=0", False)
    End Sub

    Protected Sub dgv_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles dgv.ItemCommand
        Try
            If (e.CommandName = "View") Then
                Dim row As GridDataItem = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridDataItem)
                Dim PGID As Label = DirectCast(row.FindControl("lblPGID"), Label)
                Dim GroupName As String = Convert.ToString(dgv.Items(row.ItemIndex).Cells(3).Text)
                Dim ORGID As Label = DirectCast(row.FindControl("lblOrgID"), Label)

                Response.Redirect("AdminProductGroup.aspx?Desc=" & GroupName & "&PGID=" & PGID.Text & "&ORGID=" & ORGID.Text, False)
            End If
            If (e.CommandName = "DeleteGroup") Then
                Dim row As GridDataItem = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridDataItem)
                Dim PGID As Label = DirectCast(row.FindControl("lblPGID"), Label)
                Dim obj As New Product
                If obj.DeleteProductGroupALL(Err_No, Err_Desc, PGID.Text) = True Then
                    BindData()
                End If
            End If

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("information.aspx?mode=1&errno=" & "74071" & "&msg=" & AppMsgHandler.GetErrorMessage("STX_BO_Msg_005") & "&next=PutawayListing.aspx", False)
        Finally
        End Try
    End Sub

    Private Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        ddFilterBy.ClearSelection()
        txtFilterValue.Text = ""
        BindData()
    End Sub
End Class