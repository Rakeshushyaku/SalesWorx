Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports Telerik.Web.UI
Partial Public Class ManagePriceList
    Inherits System.Web.UI.Page
    Dim Err_No As Integer
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim dtSearch As New DataTable
    Dim objProduct As New Product

    Private Const PageID As String = "P365"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

   
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

           
            LoadOrgHeads()
            ResetDetails()
            Dim OID As String = Nothing
            OID = Request.QueryString("OID").ToString()
            '  If Not OID Is Nothing Then
            Me.ddl_org.SelectedValue = OID
            BindData()
            'End If
            Session.Remove("PiceListCode")

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

    Sub LoadOrgHeads()
        Dim objCommon As New SalesWorx.BO.Common.Common
        Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
        ddl_org.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
        ddl_org.Items.Clear()
        ddl_org.Items.Add(New RadComboBoxItem("Select Organization"))
        ddl_org.AppendDataBoundItems = True
        ddl_org.DataValueField = "MAS_Org_ID"
        ddl_org.DataTextField = "Description"
        ddl_org.DataBind()

    End Sub
    Protected Sub ddOraganisation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_org.SelectedIndexChanged

        If ddl_org.SelectedItem.Text <> "Select Organization" Then
            ResetDetails()

            BindData()
        Else

            ResetDetails()
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
                '  clsPnl.Update()
                Exit Sub
            End If


            If Me.txtDescription.Text.Trim = "" Or Me.txtDescription.Text.Trim = "0" Then
                MessageBoxValidation("Please enter the price list name", "Validation")
                '  clsPnl.Update()
                Exit Sub
            End If


            If Me.txtcode.Text.Trim = "" Or Me.txtcode.Text.Trim = "0" Then
                MessageBoxValidation("Please enter the price list code", "Validation")
                '  clsPnl.Update()
                Exit Sub
            End If

            Err_No = Nothing
            Err_Desc = Nothing
            'Dim success As Boolean
            'Check Bonus plan exist for the org
            If Me.btnAddItems.Text = "Add" Or (Me.btnAddItems.Text = "Update" And Me.txtDescription.Text.Trim <> Me.lblPlanName.Text.Trim) Then
                If objProduct.CheckPriceListName(Err_No, Err_Desc, Me.ddl_org.SelectedValue.ToString(), Me.txtDescription.Text.Trim) = True Then
                    MessageBoxValidation("Same price list already exist for this organization.Please give different price list name", "Validation")
                    '   clsPnl.Update()
                    Exit Sub
                End If
            End If

            If Me.btnAddItems.Text = "Add" Then
                If (objProduct.ValidatePriceListCode(Me.ddl_org.SelectedValue.ToString(), Me.txtcode.Text)) = True Then
                    MessageBoxValidation("Same price list Code already exist for this organization.Please give different price list Code", "Validation")
                    Exit Sub
                End If
            End If


            Dim PriceListId As String = Nothing
            If Me.btnAddItems.Text = "Add" Then
                PriceListId = objProduct.SavePriceList(Err_No, Err_Desc, Me.ddl_org.SelectedValue.ToString(), Me.txtDescription.Text.Trim, CType(Session("User_Access"), UserAccess).UserID, PriceListId, txtcode.Text)

                If Not PriceListId Is Nothing Then
                    Me.lblPlanId.Text = PriceListId
                    ' MessageBoxValidation("Successfully saved.", "Information")

                    Me.lblinfo1.Text = "Confirmation"
                    Me.lblmessage1.Text = "Would you like to define the item for this price list  '" + Me.txtDescription.Text + "'?."
                    Me.lblmessage1.ForeColor = Drawing.Color.Green
                    Me.MPEAlloc.VisibleOnPageLoad = True
                    '  clsPnl.Update()
                    'ResetDetails()
                    ' BindData()
                Else
                    MessageBoxValidation("Error while saving price list", "Information")
                    ' clsPnl.Update()
                    Exit Sub
                End If
            End If


            If Me.btnAddItems.Text = "Update" Then
                If Session("PiceListCode") <> txtcode.Text Then
                    If (objProduct.ValidatePriceListCode(Me.ddl_org.SelectedValue.ToString(), Me.txtcode.Text)) = True Then
                        MessageBoxValidation("Same price list Code already exist for this organization.Please give different price list Code", "Validation")
                        Exit Sub
                    End If
                End If

            End If

            If Me.btnAddItems.Text = "Update" Then
                Dim success As Boolean = objProduct.UpdatePriceList(Err_No, Err_Desc, Me.lblPlanId.Text, Me.txtDescription.Text, Me.txtcode.Text, CType(Session("User_Access"), UserAccess).UserID)

                If success = True Then
                    MessageBoxValidation("Successfully updated.", "Information")
                    ' clsPnl.Update()
                    ResetDetails()
                    BindData()
                Else
                    MessageBoxValidation("Error while updating price list", "Information")
                    'clsPnl.Update()
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Product_006") & "&next=ManagePriceList.aspx&Title=Bonus Assortment", False)
        End Try

    End Sub
    Protected Sub btnYes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnYes.Click
        Dim PriceListID As String = Me.lblPlanId.Text
        Dim PriceListName As String = Me.txtDescription.Text
        Dim ORGID As String = Me.ddl_org.SelectedValue.ToString()
        Dim ORGName As String = Me.ddl_org.SelectedItem.Text
        Response.Redirect("PriceDefinition.aspx?Desc=" & PriceListName & "&PGID=" & PriceListID & "&ORGID=" & ORGID & "&ORGNAME=" & ORGName, False)

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
        Me.txtcode.Text = ""
        Session.Remove("PiceListCode")
        'TopPnl.Update()
    End Sub






    Private Sub BindData()

        Try
            If dtSearch.Rows.Count > 0 Then
                dtSearch.Rows.Clear()
            End If
            dtSearch = objProduct.LoadPriceList(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID, Me.ddl_org.SelectedValue.ToString())

            Dim dv As New DataView(dtSearch)
            If ViewState("SortField") <> "" Then
                dv.Sort = (ViewState("SortField") & " ") + SortDirection
            End If
            Me.dgv.DataSource = dv
            Me.dgv.DataBind()
            Session.Remove("Pricelist")
            Session("Pricelist") = dtSearch








            ' Me.clsPnl.Update()

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
                Dim ORGName As String = Convert.ToString(dgv.Rows(row.RowIndex).Cells(5).Text)
                Response.Redirect("PriceDefinition.aspx?Desc=" & PlanName.Text & "&PGID=" & PlanID & "&ORGID=" & ORGID.Text & "&ORGNAME=" & ORGName, False)
            End If
            If (e.CommandName = "EditPlan") Then
                Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim PriceListID As String = Convert.ToString(dgv.DataKeys(row.RowIndex).Value)
                Dim PriceListName As LinkButton = DirectCast(row.FindControl("LBEditPlan"), LinkButton)
                Dim ORGID As Label = DirectCast(row.FindControl("lblOrgID"), Label)
                Dim ORGName As String = Convert.ToString(dgv.Rows(row.RowIndex).Cells(5).Text)
                Dim PiceListCode As String = Convert.ToString(dgv.Rows(row.RowIndex).Cells(4).Text)

                Me.ddl_org.SelectedValue = ORGID.Text
                Me.ddl_org.Enabled = False
                Me.lblPlanId.Text = PriceListID
                Me.lblPlanName.Text = PriceListName.Text
                Me.txtDescription.Text = PriceListName.Text
                Me.txtcode.Text = PiceListCode
                Session("PiceListCode") = PiceListCode
                Me.btnAddItems.Text = "Update"
                '  TopPnl.Update()
            End If
            If (e.CommandName = "DeleteGroup") Then
                Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim PriceListID As String = Convert.ToString(dgv.DataKeys(row.RowIndex).Value)
                If objProduct.DeletePriceList(Err_No, Err_Desc, PriceListID) = True Then
                    BindData()
                End If
            End If
            If (e.CommandName = "AssignPlan") Then
                Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim PlanID As String = Convert.ToString(dgv.DataKeys(row.RowIndex).Value)
                Dim PlanName As LinkButton = DirectCast(row.FindControl("LBEditPlan"), LinkButton)
                Dim ORGID As Label = DirectCast(row.FindControl("lblOrgID"), Label)
                Dim ORGName As String = Convert.ToString(dgv.Rows(row.RowIndex).Cells(5).Text)
                Response.Redirect("AssignPriceListToCustomer.aspx?Desc=" & PlanName.Text & "&PGID=" & PlanID & "&ORGID=" & ORGID.Text & "&ORGNAME=" & ORGName, False)
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



