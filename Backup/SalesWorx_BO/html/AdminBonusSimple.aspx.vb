
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net

Partial Public Class AdminBonusSimple
    Inherits System.Web.UI.Page
    Dim Err_No As Integer
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim dtSearch As New DataTable
    Dim objProduct As New Product

    Private Const PageID As String = "P220"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub ProductGroupListing_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Bonus Simple Plan Listing"
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        ErrorResource = New ResourceManager("STX_BO.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
        If (Not IsPostBack) Then

            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            If Not HasPermission Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If

            Dim CustBonusflag As Boolean = False

            CustBonusflag = objProduct.CheckCustBonusFlag(Err_No, Err_Desc)
            If CustBonusflag = True Then
                pnlBonusheader.Visible = True
                dgv.Columns(4).Visible = True
                dgv.Columns(6).Visible = True
            Else
                pnlBonusheader.Visible = False
                dgv.Columns(4).Visible = False
                dgv.Columns(6).Visible = False
            End If
            LoadOrgHeads()
            ResetDetails()
            Dim OID As String = Nothing
            OID = Request.QueryString("OID").ToString()
            '  If Not OID Is Nothing Then
            Me.ddl_org.SelectedValue = OID
            BindData()
            'End If


            Try
                If Not Err_Desc Is Nothing Then
                    log.Error(Err_Desc)
                    Response.Redirect("information.aspx?mode=1&errno=" & "74056" & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Msg_001") & "&next=Welcome.aspx&Title=Message", False)
                End If
            Catch ex As Exception
                log.Error(GetExceptionInfo(ex))
                Response.Redirect("information.aspx?mode=1&errno=" & "74057" & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Msg_001") & "&next=Welcome.aspx&Title=Message", False)
            Finally
                ErrorResource = Nothing
            End Try

        End If
    End Sub

    Sub LoadOrgHeads()
        Dim objCommon As New Common
        Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
        ddl_org.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
        ddl_org.Items.Clear()
        ddl_org.Items.Add("-- Select a Organization --")
        ddl_org.AppendDataBoundItems = True
        ddl_org.DataValueField = "MAS_Org_ID"
        ddl_org.DataTextField = "Description"
        ddl_org.DataBind()

    End Sub
    Protected Sub ddOraganisation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_org.SelectedIndexChanged

        If ddl_org.SelectedItem.Text <> "-- Select a Organization --" Then
            ResetDetails()

            BindData()
        Else

            ResetDetails()
            BindData()
        End If



    End Sub
    Protected Sub btnAddItems_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddItems.Click

        Try


            If Me.ddl_org.SelectedIndex <= 0 Then
                Me.lblMessage.Text = "Please select a organization"
                lblMessage.ForeColor = Drawing.Color.Red
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
                clsPnl.Update()
                Exit Sub
            End If


            If Me.txtDescription.Text = "" Or Me.txtDescription.Text = "0" Then
                Me.lblMessage.Text = "Please enter the bonus plan name"
                lblMessage.ForeColor = Drawing.Color.Red
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
                clsPnl.Update()
                Exit Sub
            End If

            Err_No = Nothing
            Err_Desc = Nothing
            'Dim success As Boolean
            'Check Bonus plan exist for the org
            If Me.btnAddItems.Text = "Add" Or (Me.btnAddItems.Text = "Update" And Me.txtDescription.Text <> Me.lblPlanName.Text) Then
                If objProduct.CheckSimpleBonusPlan(Err_No, Err_Desc, Me.ddl_org.SelectedValue.ToString(), Me.txtDescription.Text) = True Then
                    Me.lblMessage.Text = "Same bonus plan already exist for this organization.Please give different plan name"
                    lblMessage.ForeColor = Drawing.Color.Red
                    lblinfo.Text = "Validation"
                    MpInfoError.Show()
                    btnClose.Focus()
                    clsPnl.Update()
                    Exit Sub
                End If
            End If

            Dim PlanId As String = Nothing
            If Me.btnAddItems.Text = "Add" Then
                PlanId = objProduct.SaveSimpleBonusPlan(Err_No, Err_Desc, Me.ddl_org.SelectedValue.ToString(), Me.txtDescription.Text, CType(Session("User_Access"), UserAccess).UserID, PlanId)

                If Not PlanId Is Nothing Then
                    Me.lblPlanId.Text = PlanId
                    Me.lblMessage.Text = "Successfully saved."
                    lblMessage.ForeColor = Drawing.Color.Green
                    lblinfo.Text = "Information"
                    MpInfoError.Show()
                    btnClose.Focus()

                    Me.lblinfo1.Text = "Confirmation"
                    Me.lblmessage1.Text = "Would you like to define the bonus for this plan  '" + Me.txtDescription.Text + "'?."
                    Me.lblmessage1.ForeColor = Drawing.Color.Green
                    Me.MPEAlloc.Show()
                    clsPnl.Update()
                    'ResetDetails()
                    ' BindData()
                Else
                    Me.lblMessage.Text = "Error while saving bonus plan"
                    lblMessage.ForeColor = Drawing.Color.Red
                    lblinfo.Text = "Validation"
                    MpInfoError.Show()
                    btnClose.Focus()
                    clsPnl.Update()
                    Exit Sub
                End If
            End If
            If Me.btnAddItems.Text = "Update" Then
                Dim success As Boolean = objProduct.UpdateSimpleBonusPlan(Err_No, Err_Desc, Me.lblPlanId.Text, Me.txtDescription.Text, CType(Session("User_Access"), UserAccess).UserID)

                If success = True Then

                    Me.lblMessage.Text = "Successfully updated."
                    lblMessage.ForeColor = Drawing.Color.Green
                    lblinfo.Text = "Information"
                    MpInfoError.Show()
                    btnClose.Focus()
                    clsPnl.Update()
                    ResetDetails()
                    BindData()
                Else
                    Me.lblMessage.Text = "Error while updating bonus plan"
                    lblMessage.ForeColor = Drawing.Color.Red
                    lblinfo.Text = "Validation"
                    MpInfoError.Show()
                    btnClose.Focus()
                    clsPnl.Update()
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Product_006") & "&next=AdminBonusSimple.aspx&Title=Bonus Assortment", False)
        End Try

    End Sub
    Protected Sub btnYes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnYes.Click
        Dim PlanID As String = Me.lblPlanId.Text
        Dim PlanName As String = Me.txtDescription.Text
        Dim ORGID As String = Me.ddl_org.SelectedValue.ToString()
        Dim ORGName As String = Me.ddl_org.SelectedItem.Text

        Response.Redirect("BonusDefinition.aspx?Desc=" & PlanName & "&PGID=" & PlanID & "&ORGID=" & ORGID & "&ORGNAME=" & ORGName, False)
    End Sub

    Protected Sub btnClose1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose1.Click
        Me.MPEAlloc.Hide()
        ResetDetails()
        BindData()
    End Sub
    Protected Sub ResetBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClear.Click
        ResetDetails()
    End Sub


    Private Sub ResetDetails()
        Me.txtDescription.Text = ""
        Me.lblPlanId.Text = ""
        Me.lblPlanName.Text = ""
        Me.ddl_org.Enabled = True
        Me.btnAddItems.Text = "Add"

        TopPnl.Update()
    End Sub






    Private Sub BindData()

        Try
            If dtSearch.Rows.Count > 0 Then
                dtSearch.Rows.Clear()
            End If
            dtSearch = objProduct.LoadSimpleBonusPlan(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID, Me.ddl_org.SelectedValue.ToString())

            Dim dv As New DataView(dtSearch)
            If ViewState("SortField") <> "" Then
                dv.Sort = (ViewState("SortField") & " ") + SortDirection
            End If
            Me.dgv.DataSource = dv
            Me.dgv.DataBind()
            Session.Remove("SGroupID")
            Session("SGroupID") = dtSearch








            Me.clsPnl.Update()

        Catch ex As Exception
            Throw ex
        Finally
            objProduct = Nothing
        End Try
    End Sub


    Private Sub dgv_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgv.RowCommand

        Try
            If (e.CommandName = "AddItems") Then
                Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim PlanID As String = Convert.ToString(dgv.DataKeys(row.RowIndex).Value)
                Dim PlanName As LinkButton = DirectCast(row.FindControl("LBEditPlan"), LinkButton)
                Dim ORGID As Label = DirectCast(row.FindControl("lblOrgID"), Label)
                Dim ORGName As String = Convert.ToString(dgv.Rows(row.RowIndex).Cells(2).Text)
                Response.Redirect("BonusDefinition.aspx?Desc=" & PlanName.Text & "&PGID=" & PlanID & "&ORGID=" & ORGID.Text & "&ORGNAME=" & ORGName, False)
            End If
            If (e.CommandName = "EditPlan") Then
                Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim PlanID As String = Convert.ToString(dgv.DataKeys(row.RowIndex).Value)
                Dim PlanName As LinkButton = DirectCast(row.FindControl("LBEditPlan"), LinkButton)
                Dim ORGID As Label = DirectCast(row.FindControl("lblOrgID"), Label)
                Dim ORGName As String = Convert.ToString(dgv.Rows(row.RowIndex).Cells(2).Text)
              

                Me.ddl_org.SelectedValue = ORGID.Text
                Me.ddl_org.Enabled = False
                Me.lblPlanId.Text = PlanID
                Me.lblPlanName.Text = PlanName.Text
                Me.txtDescription.Text = PlanName.Text
                
                Me.btnAddItems.Text = "Update"
                TopPnl.Update()
            End If
            If (e.CommandName = "DeleteGroup") Then
                Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim PlanID As String = Convert.ToString(dgv.DataKeys(row.RowIndex).Value)
                If objProduct.DeleteSimpleBonusPlan(Err_No, Err_Desc, PlanID) = True Then
                    BindData()
                End If
            End If
            If (e.CommandName = "AssignPlan") Then
                Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim PlanID As String = Convert.ToString(dgv.DataKeys(row.RowIndex).Value)
                Dim PlanName As LinkButton = DirectCast(row.FindControl("LBEditPlan"), LinkButton)
                Dim ORGID As Label = DirectCast(row.FindControl("lblOrgID"), Label)
                Dim ORGName As String = Convert.ToString(dgv.Rows(row.RowIndex).Cells(2).Text)
                Response.Redirect("AssignBonusPlanToCustomer.aspx?Desc=" & PlanName.Text & "&PGID=" & PlanID & "&ORGID=" & ORGID.Text & "&ORGNAME=" & ORGName, False)
            End If

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("information.aspx?mode=1&errno=" & "74071" & "&msg=" & AppMsgHandler.GetErrorMessage("SWX_BO_Msg_005") & "&next=Welcome.aspx", False)
        Finally
        End Try
    End Sub


    Private Sub dgv_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgv.PageIndexChanging
        dgv.PageIndex = e.NewPageIndex
        BindData()
    End Sub

    Private Sub dgv_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgv.RowDataBound
        If e.Row.RowType.Equals(DataControlRowType.Pager) Then
            Dim pTableRow As TableRow = _
                     CType(e.Row.Cells(0).Controls(0).Controls(0), TableRow)
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

    Private Sub dgv_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgv.Sorting
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



End Class



