Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports Telerik.Web.UI
Public Class AdminDiscountPlan
    Inherits System.Web.UI.Page
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim Err_No As Integer
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim dtSearch As New DataTable
    Dim objProduct As New Product

    Private Const PageID As String = "P266"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub ProductGroupListing_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Discount Plan Listing"
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

            CustBonusflag = objProduct.CheckCustDiscountFlag(Err_No, Err_Desc)
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
            Dim OID As String = "0"
            If Not Request.QueryString("OID") Is Nothing Then
                OID = Request.QueryString("OID").ToString()
            End If
            '  If Not OID Is Nothing Then
            Me.ddl_org.SelectedValue = OID


            Dim ENABLE_DISCOUNT_BY_TRANSACTION_TYPE As String
            ENABLE_DISCOUNT_BY_TRANSACTION_TYPE = (New SalesWorx.BO.Common.Common).GetAppConfig(Err_No, Err_Desc, "ENABLE_DISCOUNT_BY_TRANSACTION_TYPE")

            If ENABLE_DISCOUNT_BY_TRANSACTION_TYPE.ToUpper = "Y" Then
                divtransactiontype.Visible = True
            Else
                divtransactiontype.Visible = False
            End If

            Dim ENABLE_DISCOUNT_BY_CATEGORY As String
            ENABLE_DISCOUNT_BY_CATEGORY = (New SalesWorx.BO.Common.Common).GetAppConfig(Err_No, Err_Desc, "ENABLE_DISCOUNT_BY_CATEGORY")

            If ENABLE_DISCOUNT_BY_CATEGORY.ToUpper = "Y" Then
                divCategorytype.Visible = True
            Else
                divCategorytype.Visible = False
            End If
            LoadCategory()
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
        Else
            MPEAlloc.VisibleOnPageLoad = False
        End If
    End Sub
    Sub LoadCategory()
        Dim objCommon As New SalesWorx.BO.Common.Common
        Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)

        Dim OrgId As String = "0"
        If ddl_org.SelectedIndex <= 0 Then
            OrgId = "0"
        Else
            OrgId = ddl_org.SelectedItem.Value
        End If
        ddl_category.DataSource = (New SalesWorx.BO.Common.Customer).GetCustomerTypefromOrg(Err_No, Err_Desc, OrgId)
        ddl_category.Items.Clear()

        ddl_category.AppendDataBoundItems = True
        ddl_category.DataValueField = "Category"
        ddl_category.DataTextField = "Category"
        ddl_category.DataBind()

    End Sub

    Sub LoadOrgHeads()
        Dim objCommon As New SalesWorx.BO.Common.Common
        Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
        ddl_org.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
        ddl_org.Items.Clear()
        ddl_org.Items.Add(New RadComboBoxItem("-- Select a Organization --"))
        ddl_org.AppendDataBoundItems = True
        ddl_org.DataValueField = "MAS_Org_ID"
        ddl_org.DataTextField = "Description"
        ddl_org.DataBind()

    End Sub
    Protected Sub ddOraganisation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_org.SelectedIndexChanged

        If ddl_org.SelectedItem.Text <> "-- Select a Organization --" Then
            ResetDetails()
            LoadCategory()
            BindData()
        Else

            ResetDetails()
            LoadCategory()
            BindData()
        End If



    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub btnAddItems_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddItems.Click

        Try


            If Me.ddl_org.SelectedIndex <= 0 Then
                MessageBoxValidation("Please select a organization", "Validation")
                clsPnl.Update()
                Exit Sub
            End If


            If Me.txtDescription.Text = "" Or Me.txtDescription.Text = "0" Then
                MessageBoxValidation("Please enter the discount plan name", "Validation")
                clsPnl.Update()
                Exit Sub
            End If

            Err_No = Nothing
            Err_Desc = Nothing
            'Dim success As Boolean
            'Check Bonus plan exist for the org
            If Me.btnAddItems.Text = "Add" Or (Me.btnAddItems.Text = "Update" And Me.txtDescription.Text <> Me.lblPlanName.Text) Then
                If objProduct.CheckDiscountPlan(Err_No, Err_Desc, Me.ddl_org.SelectedValue.ToString(), Me.txtDescription.Text) = True Then
                    MessageBoxValidation("Same discount plan already exist for this organization.Please give different plan name", "Validation")
                    clsPnl.Update()
                    Exit Sub
                End If
            End If

            Dim PlanId As String = Nothing
            If Me.btnAddItems.Text = "Add" Then
                PlanId = objProduct.SaveDiscountPlan(Err_No, Err_Desc, Me.ddl_org.SelectedValue.ToString(), Me.txtDescription.Text, CType(Session("User_Access"), UserAccess).UserID, PlanId, ddl_transactionType.SelectedItem.Value)
                Me.lblPlanId.Text = PlanId
                If Not PlanId Is Nothing Then
                   

                    Me.lblPlanId.Text = PlanId
                    Dim dtCategory As New DataTable
                    dtCategory.Columns.Add("Category")
                    For Each item As RadComboBoxItem In ddl_Category.CheckedItems
                        Dim dr As DataRow
                        dr = dtCategory.NewRow
                        dr(0) = item.Value
                        dtCategory.Rows.Add(dr)
                    Next

                     
                    objProduct.SaveDiscountCategoryMap(Err_No, Err_Desc, Me.lblPlanId.Text, dtCategory, CType(Session("User_Access"), UserAccess).UserID)
                     
                   
                    Me.lblinfo1.Text = "Confirmation"
                    Me.lblmessage1.Text = "Would you like to define the discount for this plan  '" + Me.txtDescription.Text + "'?."
                    Me.lblmessage1.ForeColor = Drawing.Color.Green
                    Me.MPEAlloc.VisibleOnPageLoad = True
                    clsPnl.Update()
                    'ResetDetails()
                    ' BindData()
                Else
                    MessageBoxValidation("Error while saving discount plan", "Information")
                    clsPnl.Update()
                    Exit Sub
                End If
            End If
            If Me.btnAddItems.Text = "Update" Then
                Dim success As Boolean = objProduct.UpdateDiscountPlan(Err_No, Err_Desc, Me.lblPlanId.Text, Me.txtDescription.Text, CType(Session("User_Access"), UserAccess).UserID, ddl_transactionType.SelectedItem.Value)
                Dim dtCategory As New DataTable
                dtCategory.Columns.Add("Category")
                For Each item As RadComboBoxItem In ddl_Category.CheckedItems
                    Dim dr As DataRow
                    dr = dtCategory.NewRow
                    dr(0) = item.Value
                    dtCategory.Rows.Add(dr)
                Next
                Dim dtCurrentCategory As New DataTable
                dtCurrentCategory = objProduct.GetDiscountCategoryMap(Err_No, Err_Desc, Me.lblPlanId.Text)

                Dim CategoryChanged As Boolean = False
                If dtCurrentCategory.Rows.Count <> dtCategory.Rows.Count Then
                    CategoryChanged = True
                Else
                    For Each dr1 As DataRow In dtCurrentCategory.Rows
                        If dtCategory.Select("Category='" & dr1("Category").ToString & "'").Length <= 0 Then
                            CategoryChanged = True
                            Exit For
                        End If
                    Next
                    If CategoryChanged = False Then
                        For Each dr1 As DataRow In dtCategory.Rows
                            If dtCurrentCategory.Select("Category='" & dr1("Category").ToString & "'").Length <= 0 Then
                                CategoryChanged = True
                                Exit For
                            End If
                        Next
                    End If
                End If

                objProduct.SaveDiscountCategoryMap(Err_No, Err_Desc, Me.lblPlanId.Text, dtCategory, CType(Session("User_Access"), UserAccess).UserID)
                Dim Msg As String = ""

                If CategoryChanged = True Then
                    Msg = Msg & ",Category map changed"
                End If
                If H_TransType.Value.ToUpper <> ddl_transactionType.SelectedItem.Value.ToUpper Then
                    Msg = Msg & ",Transaction Type changed"
                End If
                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "DISCOUNT DEFINITION", Me.lblPlanId.Text, "ID: " & Me.lblPlanId.Text & Msg, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")

                If success = True Then
                    MessageBoxValidation("Successfully updated.", "Information")
                    clsPnl.Update()
                    ResetDetails()
                    BindData()
                Else
                    MessageBoxValidation("Error while updating discount plan", "Information")
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

        Response.Redirect("DiscountDefinition.aspx?Desc=" & PlanName & "&PGID=" & PlanID & "&ORGID=" & ORGID & "&ORGNAME=" & ORGName, False)
    End Sub

    Protected Sub btnClose1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose1.Click
        Me.MPEAlloc.VisibleOnPageLoad = False
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
        ddl_transactionType.SelectedIndex = 0
        ddl_Category.ClearCheckedItems()
        TopPnl.Update()
    End Sub

    Private Sub BindData()

        Try
            If dtSearch.Rows.Count > 0 Then
                dtSearch.Rows.Clear()
            End If
            dtSearch = objProduct.LoadDiscountPlan(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID, Me.ddl_org.SelectedValue.ToString())

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
                Response.Redirect("DiscountDefinition.aspx?Desc=" & PlanName.Text & "&PGID=" & PlanID & "&ORGID=" & ORGID.Text & "&ORGNAME=" & ORGName, False)
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
                LoadCategory()
                Dim lblTransType As Label = DirectCast(row.FindControl("lblTransType"), Label)
                H_TransType.Value = lblTransType.Text
                If Not ddl_transactionType.FindItemByValue(lblTransType.Text) Is Nothing Then
                    ddl_transactionType.ClearSelection()
                    ddl_transactionType.FindItemByValue(lblTransType.Text).Selected = True
                End If
                LoadCategory()
                Dim dtCategory As New DataTable
                dtCategory = objProduct.GetDiscountCategoryMap(Err_No, Err_Desc, PlanID)
                ddl_Category.ClearCheckedItems()
                For Each dr As DataRow In dtCategory.Rows
                    If Not ddl_Category.FindItemByValue(dr("Category").ToString()) Is Nothing Then
                        ddl_Category.FindItemByValue(dr("Category").ToString()).Checked = True
                    End If
                Next
                Me.btnAddItems.Text = "Update"
                TopPnl.Update()
            End If
            'If (e.CommandName = "DeleteGroup") Then
            '    Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
            '    Dim PlanID As String = Convert.ToString(dgv.DataKeys(row.RowIndex).Value)
            '    If objProduct.DeleteSimpleBonusPlan(Err_No, Err_Desc, PlanID) = True Then
            '        BindData()
            '    End If
            'End If
            If (e.CommandName = "AssignPlan") Then
                Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim PlanID As String = Convert.ToString(dgv.DataKeys(row.RowIndex).Value)
                Dim PlanName As LinkButton = DirectCast(row.FindControl("LBEditPlan"), LinkButton)
                Dim ORGID As Label = DirectCast(row.FindControl("lblOrgID"), Label)
                Dim ORGName As String = Convert.ToString(dgv.Rows(row.RowIndex).Cells(2).Text)
                Response.Redirect("AssignDiscountPlanToCustomer.aspx?Desc=" & PlanName.Text & "&PGID=" & PlanID & "&ORGID=" & ORGID.Text & "&ORGNAME=" & ORGName, False)
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



