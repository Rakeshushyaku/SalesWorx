
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports Telerik.Web.UI
Partial Public Class AdminBonusAssortment
    Inherits System.Web.UI.Page
    Dim Err_No As Integer
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim dtSearch As New DataTable
    Dim objProduct As New Product
    Dim objLogin As New SalesWorx.BO.Common.Login
    Private Const PageID As String = "P221"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub ProductGroupListing_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Bonus Assortment Plan Listing"
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

            LoadBonusType()
            LoadOrgHeads()
            ResetDetails()
            Dim OID As String = Nothing
            OID = Request.QueryString("OID").ToString()
            If Not OID Is Nothing Then
                Me.ddl_org.SelectedValue = OID

            End If

            Dim ENABLE_BONUS_BY_TRANSACTION_TYPE As String
            ENABLE_BONUS_BY_TRANSACTION_TYPE = (New SalesWorx.BO.Common.Common).GetAppConfig(Err_No, Err_Desc, "ENABLE_BONUS_BY_TRANSACTION_TYPE")

            If ENABLE_BONUS_BY_TRANSACTION_TYPE.ToUpper = "Y" Then

                divtransactiontype.Visible = True
            Else
                ddl_transactionType.Items.Insert(0, New RadComboBoxItem("All", "0"))
                ddl_transactionType.Items(0).Selected = True
                divtransactiontype.Visible = False
            End If
            Dim ENABLE_BONUS_BY_CATEGORY As String
            ENABLE_BONUS_BY_CATEGORY = (New SalesWorx.BO.Common.Common).GetAppConfig(Err_No, Err_Desc, "ENABLE_BONUS_BY_CATEGORY")

            If ENABLE_BONUS_BY_CATEGORY.ToUpper = "Y" Then
                divCategorytype.Visible = True
            Else
                divCategorytype.Visible = False
            End If
            BindData()
            LoadCategory()
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
            Me.MPEAlloc.VisibleOnPageLoad = False
        End If
    End Sub
    Sub LoadBonusType()
        ddlType.Items.Clear()
        ' ddlType.DataSource = (New SalesWorx.BO.Common.AppControl).LoadAppCode(Err_No, Err_Desc, "ASSORTMENT_BONUS_TYPE")

        ddlType.DataValueField = "Code_Value"
        ddlType.DataTextField = "Code_Description"
        ddlType.DataBind()
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
        ddl_Category.DataTextField = "Category_Desc"
        ddl_category.DataBind()

    End Sub
    Sub LoadOrgHeads()
        'ddl_org.DataSource = objProduct.GetOrgsHeads(Err_No, Err_Desc)
        'ddl_org.DataTextField = "Description"
        'ddl_org.DataValueField = "ORG_HE_ID"
        'ddl_org.DataBind()
        'ddl_org.Items.Insert(0, "--Select Organisation--")
        'ddl_org.Items(0).Value = 0
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
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
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
    Protected Sub btnAddItems_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddItems.Click
        Try
            If Me.ddl_org.SelectedIndex <= 0 Then
                MessageBoxValidation("Please select a organization", "Validation")
                Exit Sub
            End If


            If Me.txtDescription.Text = "" Or Me.txtDescription.Text = "0" Then
                MessageBoxValidation("Please enter the bonus plan name", "Validation")
                Exit Sub
            End If

            If (Me.StartTime.SelectedDate.Value <= Now.Date() And Me.StartTime.Enabled = True) Or Me.EndTime.SelectedDate.Value <= Now.Date() Then
                MessageBoxValidation("Valid from and to date should be greater than current date", "Validation")
                Exit Sub
            End If
            Err_No = Nothing
            Err_Desc = Nothing
            'Dim success As Boolean
            'Check Bonus plan exist for the org
            If Me.btnAddItems.Text = "Add" Or (Me.btnAddItems.Text = "Update" And Me.txtDescription.Text <> Me.lblPlanName.Text) Then
                If objProduct.CheckBonusPlan(Err_No, Err_Desc, Me.ddl_org.SelectedValue.ToString(), Me.txtDescription.Text) = True Then
                    MessageBoxValidation("Same bonus plan already exist for this organization.Please give different plan name", "Validation")
                    Exit Sub
                End If
            End If
            Dim dt As New DataTable
            dt.Columns.Add("Category")
            dt.Columns.Add("Status")
            dt.Columns.Add("Response")
            Dim PlanId As String = Nothing
            If Me.btnAddItems.Text = "Add" Then
                PlanId = objProduct.SaveBonusPlan(Err_No, Err_Desc, Me.ddl_org.SelectedValue.ToString(), Me.txtDescription.Text, CType(Session("User_Access"), UserAccess).UserID, PlanId, IIf(Me.chActive.Checked = True, "Y", "N"), Me.StartTime.SelectedDate.Value, Me.EndTime.SelectedDate.Value, Me.ddlType.SelectedValue, ddl_transactionType.SelectedItem.Value)

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

                    Me.lblPlanId.Text = PlanId
                    objProduct.SaveAssortmentBonusCategoryMap(Err_No, Err_Desc, Me.lblPlanId.Text, dtCategory, CType(Session("User_Access"), UserAccess).UserID, ddlType.SelectedItem.Value, "N", ddl_org.SelectedItem.Value, ddl_transactionType.SelectedItem.Value, dt)
                    'MessageBoxValidation("Successfully saved.", "Information")
                    Me.lblinfo1.Text = "Confirmation"
                    Me.lblmessage1.Text = "Would you like to define the bonus for this plan  '" + Me.txtDescription.Text + "'?."
                    Me.lblmessage1.ForeColor = Drawing.Color.Green
                    Me.MPEAlloc.VisibleOnPageLoad = True
                    clsPnl.Update()
                    'ResetDetails()
                    ' BindData()
                Else
                    MessageBoxValidation("Error while saving bonus plan", "Information")
                    Exit Sub
                End If
            End If
            If Me.btnAddItems.Text = "Update" Then
                Dim success As Boolean = objProduct.UpdateBonusPlan(Err_No, Err_Desc, Me.lblPlanId.Text, Me.txtDescription.Text, CType(Session("User_Access"), UserAccess).UserID, IIf(Me.chActive.Checked = True, "Y", "N"), Me.StartTime.SelectedDate.Value, Me.EndTime.SelectedDate.Value, ddl_transactionType.SelectedItem.Value)
                Dim dtCategory As New DataTable
                dtCategory.Columns.Add("Category")
                For Each item As RadComboBoxItem In ddl_Category.CheckedItems
                    Dim dr As DataRow
                    dr = dtCategory.NewRow
                    dr(0) = item.Value
                    dtCategory.Rows.Add(dr)
                Next


                Dim dtCurrentCategory As New DataTable
                dtCurrentCategory = objProduct.GetAssortmentBonusCategoryMap(Err_No, Err_Desc, Me.lblPlanId.Text)
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




                success = objProduct.SaveAssortmentBonusCategoryMap(Err_No, Err_Desc, Me.lblPlanId.Text, dtCategory, CType(Session("User_Access"), UserAccess).UserID, ddlType.SelectedItem.Value, "U", ddl_org.SelectedItem.Value, ddl_transactionType.SelectedItem.Value, dt)

                Dim Msg As String = ""

                If CategoryChanged = True Then
                    Msg = Msg & ",Category map changed"
                End If
                If H_TransType.Value.ToUpper <> ddl_transactionType.SelectedItem.Value.ToUpper Then
                    Msg = Msg & ",Transaction Type changed"
                End If
                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "ASSORTMENT BONUS", Me.lblPlanId.Text, "ID: " & Me.lblPlanId.Text & Msg, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")





                Me.lblPlanId.Text = PlanId
                If success = True And dt.Rows.Count <= 0 Then

                    MessageBoxValidation("Successfully updated.", "Information")
                    ResetDetails()
                    BindData()
                ElseIf dt.Rows.Count > 0 Then
                    GV_error.DataSource = dt
                    GV_error.DataBind()
                    MPError.VisibleOnPageLoad = True
                Else
                    MessageBoxValidation("Error while updating bonus plan", "Information")
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Product_006") & "&next=AdminBonusAssortment.aspx&Title=Bonus Assortment", False)
        End Try

    End Sub
    Protected Sub btnYes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnYes.Click
        Dim PlanID As String = Me.lblPlanId.Text
        Dim PlanName As String = Me.txtDescription.Text
        Dim ORGID As String = Me.ddl_org.SelectedValue.ToString()
        Dim ORGName As String = Me.ddl_org.SelectedItem.Text
        Dim lblPlanType As String = ddlType.SelectedValue
        Dim TRANTYPE As String = ddl_transactionType.SelectedValue
        If lblPlanType = "N" Then
            Response.Redirect("AssortmentDefinition.aspx?Desc=" & PlanName & "&PGID=" & PlanID & "&ORGID=" & ORGID & "&ORGNAME=" & ORGName & "&TRANTYPE=" & TRANTYPE, False)
        ElseIf lblPlanType = "I" Then
            Response.Redirect("AssortmentDefinitionMinQty.aspx?Desc=" & PlanName & "&PGID=" & PlanID & "&ORGID=" & ORGID & "&ORGNAME=" & ORGName, False)
        ElseIf lblPlanType = "V" Then
            Response.Redirect("AssortmentDefinitionValue.aspx?Desc=" & PlanName & "&PGID=" & PlanID & "&ORGID=" & ORGID & "&ORGNAME=" & ORGName & "&TRANTYPE=" & TRANTYPE, False)
        End If

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
        Me.StartTime.SelectedDate = Now.Date.AddDays(1)
        Me.EndTime.SelectedDate = Me.StartTime.SelectedDate.Value.AddYears(1)
        Me.chActive.Checked = True
        Me.ddl_org.Enabled = True
        Me.btnAddItems.Text = "Add"
        Me.ddlType.SelectedIndex = 0
        ddl_transactionType.SelectedIndex = 0
        ddl_Category.ClearCheckedItems()
        ddlType.Enabled = True
        ddl_transactionType.Enabled = True
        TopPnl.Update()
    End Sub
    Private Sub BindData()

        Try
            If dtSearch.Rows.Count > 0 Then
                dtSearch.Rows.Clear()
            End If
            dtSearch = objProduct.LoadBonusPlan(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID, Me.ddl_org.SelectedValue.ToString())

            Dim dv As New DataView(dtSearch)
            If ViewState("SortField") <> "" Then
                dv.Sort = (ViewState("SortField") & " ") + SortDirection
            End If
            Me.dgv.DataSource = dv
            Me.dgv.DataBind()
            Session.Remove("GroupID")
            Session("GroupID") = dtSearch
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
                Dim lblPlanType As Label = DirectCast(row.FindControl("lblPlanType"), Label)
                Dim lblTransType As Label = DirectCast(row.FindControl("lblTransType"), Label)

                If lblPlanType.Text = "N" Then
                    Response.Redirect("AssortmentDefinition.aspx?Desc=" & PlanName.Text & "&PGID=" & PlanID & "&ORGID=" & ORGID.Text & "&ORGNAME=" & ORGName & "&TRANTYPE=" & lblTransType.Text, False)
                ElseIf lblPlanType.Text = "I" Then
                    Response.Redirect("AssortmentDefinitionMinQty.aspx?Desc=" & PlanName.Text & "&PGID=" & PlanID & "&ORGID=" & ORGID.Text & "&ORGNAME=" & ORGName, False)
                ElseIf lblPlanType.Text = "V" Then
                    Response.Redirect("AssortmentDefinitionValue.aspx?Desc=" & PlanName.Text & "&PGID=" & PlanID & "&ORGID=" & ORGID.Text & "&ORGNAME=" & ORGName & "&TRANTYPE=" & lblTransType.Text, False)
                End If
            End If
            If (e.CommandName = "EditPlan") Then
                Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim PlanID As String = Convert.ToString(dgv.DataKeys(row.RowIndex).Value)
                Dim PlanName As LinkButton = DirectCast(row.FindControl("LBEditPlan"), LinkButton)
                Dim ORGID As Label = DirectCast(row.FindControl("lblOrgID"), Label)
                Dim ORGName As String = Convert.ToString(dgv.Rows(row.RowIndex).Cells(2).Text)
                Dim PlanType As String = Convert.ToString(dgv.Rows(row.RowIndex).Cells(3).Text)
                Dim lblValidFrom As Label = DirectCast(row.FindControl("lblValidFrom"), Label)
                Dim lblValidTo As Label = DirectCast(row.FindControl("lblValidTo"), Label)
                Dim lblActive As Label = DirectCast(row.FindControl("lblIsActive"), Label)
                Dim lblPlanType As Label = DirectCast(row.FindControl("lblPlanType"), Label)


                Me.ddlType.SelectedValue = lblPlanType.Text
                ddlType.Enabled = False
                Me.ddl_org.SelectedValue = ORGID.Text
                Me.ddl_org.Enabled = False
                Me.lblPlanId.Text = PlanID
                Me.lblPlanName.Text = PlanName.Text
                Me.txtDescription.Text = PlanName.Text
                If lblValidFrom.Text = "" Then
                    Me.StartTime.SelectedDate = Now.Date.AddDays(1)
                Else
                    Me.StartTime.SelectedDate = lblValidFrom.Text
                End If

                If lblValidTo.Text = "" Then
                    Me.EndTime.SelectedDate = Me.StartTime.SelectedDate.Value.AddYears(1)
                Else
                    Me.EndTime.SelectedDate = lblValidTo.Text
                End If

                If Me.StartTime.SelectedDate.Value <= Now.Date Then
                    Me.StartTime.Enabled = False
                Else
                    Me.StartTime.Enabled = True
                End If
                If lblActive.Text = "Yes" Then
                    chActive.Checked = True
                Else
                    chActive.Checked = False
                End If
                Dim lblTransType As Label = DirectCast(row.FindControl("lblTransType"), Label)
                H_TransType.Value = lblTransType.Text

                Dim ENABLE_BONUS_BY_TRANSACTION_TYPE As String
                ENABLE_BONUS_BY_TRANSACTION_TYPE = (New SalesWorx.BO.Common.Common).GetAppConfig(Err_No, Err_Desc, "ENABLE_BONUS_BY_TRANSACTION_TYPE")

                If ENABLE_BONUS_BY_TRANSACTION_TYPE.ToUpper = "Y" Then

                Else
                    ddl_transactionType.Items.Insert(0, New RadComboBoxItem("All", "0"))
                End If

                If Not ddl_transactionType.FindItemByValue(lblTransType.Text) Is Nothing Then
                    ddl_transactionType.ClearSelection()
                    ddl_transactionType.FindItemByValue(lblTransType.Text).Selected = True
                End If
                ddl_transactionType.Enabled = False
                LoadCategory()
                Dim dtCategory As New DataTable
                dtCategory = objProduct.GetAssortmentBonusCategoryMap(Err_No, Err_Desc, PlanID)
                ddl_Category.ClearCheckedItems()
                For Each dr As DataRow In dtCategory.Rows
                    If Not ddl_Category.FindItemByValue(dr("Category").ToString()) Is Nothing Then
                        ddl_Category.FindItemByValue(dr("Category").ToString()).Checked = True
                    End If
                Next
                Me.btnAddItems.Text = "Update"
                TopPnl.Update()
            End If
            If (e.CommandName = "DeleteGroup") Then
                Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim PlanID As String = Convert.ToString(dgv.DataKeys(row.RowIndex).Value)
                If objProduct.DeleteBonusPlan(Err_No, Err_Desc, PlanID, CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
                    BindData()
                End If
            End If
            If (e.CommandName = "AssignPlan") Then
                Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim PlanID As String = Convert.ToString(dgv.DataKeys(row.RowIndex).Value)
                Dim PlanName As LinkButton = DirectCast(row.FindControl("LBEditPlan"), LinkButton)
                Dim ORGID As Label = DirectCast(row.FindControl("lblOrgID"), Label)
                Dim ORGName As String = Convert.ToString(dgv.Rows(row.RowIndex).Cells(2).Text)
                Dim PlanType As String = "ASSORTMENT"
                Response.Redirect("AssignBonusPlanToCustomer.aspx?Desc=" & PlanName.Text & "&PGID=" & PlanID & "&ORGID=" & ORGID.Text & "&ORGNAME=" & ORGName & "&PGName=" & PlanName.Text & "&PlanType=" & PlanType, False)
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



