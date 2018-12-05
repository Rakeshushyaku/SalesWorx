Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Telerik.Web.UI
Partial Public Class HeldNewCustomer
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As New Customer
  
    Dim dv As New DataView
    Private Const PageID As String = "P91"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
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

                ObjCommon = New SalesWorx.BO.Common.Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("-- Select a value --"))

                txtFromDate.DateInput.Text = "" ' Format(Now().Date.AddDays(-7), "dd/MM/yyyy")
                txtToDate.DateInput.Text = "" 'Format(Now().Date, "dd/MM/yyyy")
            Else
                Me.MapWindow.VisibleOnPageLoad = False
            End If
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_MSL_005") & "&next=Welcome.aspx&Title=Product List Setup", False)
        End Try

    End Sub
  
   
    Private Sub BindData()
        Dim SearchQuery As String = ""
        Try

            ObjCommon = New SalesWorx.BO.Common.Common()
            If (ddlCustomer.SelectedIndex <= 0 And ddlVan.SelectedIndex <= 0 And txtFromDate.DateInput.Text = "" And txtToDate.DateInput.Text = "") Then
                SearchQuery = ""
            Else
                If ddlVan.SelectedIndex > 0 Then
                    SearchQuery = " And A.Created_By=" & ddlVan.SelectedValue
                Else
                    SearchQuery = " And A.Created_By in (" & ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID) & ")"
                End If

                If ddlCustomer.SelectedIndex > 0 Then
                    SearchQuery = SearchQuery & " AND (LTRIM(STR(B.Customer_ID)) + '$' + LTRIM(STR(B.Site_Use_ID)))='" & ddlCustomer.SelectedValue & "' "
                End If


                If txtFromDate.DateInput.Text <> "" Then
                    SearchQuery = SearchQuery & " And A.System_Order_Date >=convert(datetime,'" & CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy") & "',103)"
                End If
                If txtToDate.DateInput.Text <> "" Then
                    SearchQuery = SearchQuery & " And A.System_Order_Date <= convert(datetime,'" & CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy") & " 23:59:59',103)"
                End If
            End If
            Dim ds As New DataTable
            ds = ObjCustomer.GetHeldNewCustomers(Err_No, Err_Desc, SearchQuery, IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue))

            Dim dv As New DataView(ds)
            If ViewState("SortField") <> "" Then
                dv.Sort = (ViewState("SortField") & " ") + SortDirection
            End If
            GVOrders.DataSource = dv
            GVOrders.DataBind()
            Me.Panel.Update()

        Catch ex As Exception
            If Err_Desc IsNot Nothing Then
                log.Error(Err_Desc)
            Else
                log.Error(GetExceptionInfo(ex))
            End If
            Err_No = "74067"
            '  Err_Desc = ex.Message
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
        End Try
    End Sub

    Private Sub BindRefreshData()
        Dim SearchQuery As String = ""
        Try


            Dim ds As New DataTable
            ds = ObjCustomer.GetHeldNewCustomers(Err_No, Err_Desc, SearchQuery, IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue))
            Dim dv As New DataView(ds)
            If ViewState("SortField") <> "" Then
                dv.Sort = (ViewState("SortField") & " ") + SortDirection
            End If
            GVOrders.DataSource = dv
            GVOrders.DataBind()
            Me.Panel.Update()

        Catch ex As Exception
            If Err_Desc IsNot Nothing Then
                log.Error(Err_Desc)
            Else
                log.Error(GetExceptionInfo(ex))
            End If
            Err_No = "74067"
            '  Err_Desc = ex.Message
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
       
        End Try
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
    Private Sub GVOrders_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GVOrders.PageIndexChanging
        GVOrders.PageIndex = e.NewPageIndex
        BindData()
    End Sub
    Function GetDatasource(ByVal OderNo As String) As DataTable
        Dim dt As New DataTable
        dt = ObjCustomer.GetHeldReceipt(Err_No, Err_Desc, OderNo)
        'dt.Columns.Add("DateAdded")
        'dt.Columns.Add("Treatment")
        'dt.Columns.Add("TotalCost")
        'If OderNo = "20121000016" Then
        '    Dim dr As DataRow
        '    dr = dt.NewRow()
        '    dr(0) = "12-12-2015"
        '    dr(1) = "12-12-2015"
        '    dr(1) = "12-12-2015"
        '    dt.Rows.Add(dr)
        'End If
        Return dt
    End Function
    'Private Sub GVOrders_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVOrders.RowDataBound
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '    Dim dt As DataTable
    '    dt = GetDatasource(DirectCast(e.Row.FindControl("lblOrdNo"), Label).Text)
    '    If (dt.Rows.Count > 0) Then
    '            Dim gv As New GridView
    '                gv.DataSource = dt
    '                gv.ID = "grdReceipt" & e.Row.RowIndex 'Since a gridview is 
    '                'being created for each row they each need a unique ID, so append the row index
    '                gv.AutoGenerateColumns = False
    '                gv.CssClass = "subgridview"

    '                Dim bf1 As New BoundField
    '                bf1.DataField = "Collection_Ref_No"

    '                bf1.HeaderText = "Collection RefNo."
    '                gv.Columns.Add(bf1)

    '                Dim bf2 As New BoundField
    '                bf2.DataField = "Collected_On"
    '                bf2.DataFormatString = "{0:dd-MM-yyyy}"
    '                bf2.HeaderText = "Collected On"
    '                gv.Columns.Add(bf2)

    '                Dim bf3 As New BoundField
    '                bf3.DataField = "Amount"
    '                bf3.HeaderText = "Receipt Amt."
    '                bf3.DataFormatString = "{0:###,#.00}"
    '                gv.Columns.Add(bf3)

    '                Dim bf4 As New BoundField
    '                bf4.DataField = "SalesRep_Name"
    '                bf4.HeaderText = "Van"
    '                gv.Columns.Add(bf4)

    '                'Create the show/hide button which will be displayed on each row of the main GridView
    '                Dim btn As Web.UI.WebControls.Image = New Web.UI.WebControls.Image
    '                btn.ID = "btnDetail"
    '                btn.ImageUrl = "~/Images/close.jpg"
    '                btn.Attributes.Add("onclick", "javascript: gvrowtoggle(" & e.Row.RowIndex + 1 & ")") 'Adds the javascript 
    '                'function to the show/hide button, passing the row to be toggled as a parameter

    '                'Add the expanded details row after each record in the main GridView
    '                Dim tbl As Table = DirectCast(e.Row.Parent, Table)
    '                Dim tr As New GridViewRow(e.Row.RowIndex + 1, -1, _
    '                DataControlRowType.EmptyDataRow, DataControlRowState.Normal)

    '                Dim tc1 As New TableCell()
    '                tc1.Text = "&nbsp;"
    '                tc1.ColumnSpan = 3
    '                Dim tc As New TableCell()
    '                tc.ColumnSpan = 6
    '                tc.BorderStyle = BorderStyle.None

    '                'tc.BackColor = Drawing.Color.AliceBlue

    '                tc.Controls.Add(gv) 'Add the expanded details GridView to the newly-created cell
    '                tr.Cells.Add(tc1)

    '                tr.Cells.Add(tc) 'Add the newly-created cell to the newly-created row
    '                tr.CssClass = "tdstylereceipt"
    '                tbl.Rows.Add(tr) ' Add the newly-ccreated row to the main GridView
    '                'e.Row.Cells(0).Controls.Add(btn) 'Add the show/hide button to the main GridView row
    '                gv.CssClass = "tablecellalign"
    '                gv.RowStyle.CssClass = "tdstyle"
    '                gv.AlternatingRowStyle.CssClass = "alttdstyle"
    '                gv.DataBind()
    '    End If
    '   End If
    'End Sub

    Private Sub GVOrders_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GVOrders.Sorting
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindData()
    End Sub

    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If Me.ddlOrganization.SelectedIndex <= 0 Then
            MessageBoxValidation("Please select a organization", "Validation")
            Exit Sub
        End If

        If (Me.txtFromDate.DateInput.Text <> "" And Me.txtToDate.DateInput.Text = "" Or Me.txtFromDate.DateInput.Text = "" And Me.txtToDate.DateInput.Text <> "") Then
            MessageBoxValidation("Please enter from and to date", "Validation")
            Exit Sub
        End If
        If Me.txtFromDate.DateInput.Text <> "" And Me.txtToDate.DateInput.Text <> "" Then
            Dim TemFromDateStr As String = CDate(txtFromDate.SelectedDate).ToString("dd/MM/yyyy")
            Dim DateArr As Array = TemFromDateStr.Split("/")
            If DateArr.Length = 3 Then
                TemFromDateStr = DateArr(1) & "/" & DateArr(0) & "/" & DateArr(2)
            End If
            Dim TemToDateStr As String = CDate(txtToDate.SelectedDate).ToString("dd/MM/yyyy")
            Dim DateArr1 As Array = TemToDateStr.Split("/")
            If DateArr1.Length = 3 Then
                TemToDateStr = DateArr1(1) & "/" & DateArr1(0) & "/" & DateArr1(2)
            End If

            If Not IsDate(TemFromDateStr) Then
                MessageBoxValidation("Enter valid ""From date"".", "Validation")
                SetFocus(txtFromDate)
                Exit Sub
            End If

            If Not IsDate(TemToDateStr) Then
                MessageBoxValidation("Enter valid ""To date"".", "Validation")
                SetFocus(TemToDateStr)
                Exit Sub
            End If
            If CDate(TemFromDateStr) > CDate(TemToDateStr) Then
                MessageBoxValidation("Start Date should not be greater than End Date.", "Validation")
                Exit Sub
            End If
        End If
        If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then
            BindData()
        End If
    End Sub

    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New SalesWorx.BO.Common.Common
            ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())
            ddlVan.DataBind()
            ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --"))

            ddlCustomer.DataSource = ObjCommon.GetCustomerByCriteria(Err_No, Err_Desc, ddlOrganization.SelectedValue)
            ddlCustomer.DataBind()
            ddlCustomer.Items.Insert(0, New ListItem("-- Select a value --"))

        End If
        BindRefreshData()
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Protected Sub btnApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApprove.Click

        If Me.ddlOrganization.SelectedIndex <= 0 Then
            MessageBoxValidation("Please select a organization", "Validation")
            Exit Sub
        End If

        If GVOrders.Rows.Count <= 0 Then
            MessageBoxValidation("There is no data in the list", "Validation")
            Exit Sub
        End If
        Dim bCheck As Boolean = False
        For Each gr As GridViewRow In GVOrders.Rows
            Dim chk As CheckBox = DirectCast(gr.FindControl("chkDelete"), CheckBox)
            If chk.Checked = True Then
                bCheck = True
            End If
        Next

        If bCheck = False Then
            MessageBoxValidation("Please select atleast one order from the list", "Validation")
            Exit Sub
        End If

        Dim bSuccess As Boolean = False

        For Each gr As GridViewRow In GVOrders.Rows
            Dim chk As CheckBox = DirectCast(gr.FindControl("chkDelete"), CheckBox)
            Dim OrdNo As Label = DirectCast(gr.FindControl("lblOrdNo"), Label)
            If chk.Checked = True Then
                If ObjCustomer.UpdateReleaseOrders(Err_No, Err_Desc, OrdNo.Text, CType(Session("User_Access"), UserAccess).UserID) = True Then
                    bSuccess = True
                End If
            End If
        Next
        If bSuccess = True Then
            BindRefreshData()
            MessageBoxValidation("Successfully updated", "Information")
   
        Else
            MessageBoxValidation("Error occured while updating records", "Information")
            Exit Sub
        End If
        Me.Panel.Update()


    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        If Me.ddlOrganization.SelectedIndex <= 0 Then
            MessageBoxValidation("Please select a organization", "Validation")
            Exit Sub
        End If
        If GVOrders.Rows.Count <= 0 Then
            MessageBoxValidation("There is no data in the list", "Validation")
            Exit Sub
        End If
        Dim bCheck As Boolean = False
        For Each gr As GridViewRow In GVOrders.Rows
            Dim chk As CheckBox = DirectCast(gr.FindControl("chkDelete"), CheckBox)
            If chk.Checked = True Then
                bCheck = True
            End If
        Next

        If bCheck = False Then
            MessageBoxValidation("Please select atleast one order from the list", "Validation")
            Exit Sub
        End If

        Dim bSuccess As Boolean = False

        For Each gr As GridViewRow In GVOrders.Rows
            Dim chk As CheckBox = DirectCast(gr.FindControl("chkDelete"), CheckBox)
            Dim OrdNo As Label = DirectCast(gr.FindControl("lblOrdNo"), Label)
            If chk.Checked = True Then
                If ObjCustomer.UpdateReconcileOrders(Err_No, Err_Desc, OrdNo.Text, CType(Session("User_Access"), UserAccess).UserID, "0") = True Then
                    bSuccess = True
                End If
            End If
        Next
        If bSuccess = True Then
            BindRefreshData()
            MessageBoxValidation("Successfully updated", "Information")
        Else
            MessageBoxValidation("Error occured while updating records", "Information")
            Exit Sub
        End If
        Me.Panel.Update()
    End Sub

    Protected Sub lbRelease_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.MapWindow.VisibleOnPageLoad = False
        Try
            If Me.ddlOrganization.SelectedIndex <= 0 Then
                MessageBoxValidation("Please select a organization", "Validation")
                Exit Sub
            End If
            Dim lbRelease As LinkButton = TryCast(sender, LinkButton)
            Dim row As GridViewRow = DirectCast(lbRelease.NamingContainer, GridViewRow)
            Dim OrderNo As Label = DirectCast(row.FindControl("lblOrdNo"), Label)
            If ObjCustomer.UpdateReleaseOrders(Err_No, Err_Desc, OrderNo.Text, CType(Session("User_Access"), UserAccess).UserID) = True Then
                BindRefreshData()
                MessageBoxValidation("Successfully updated", "Information")
            Else
                MessageBoxValidation("Error occured while updating record", "Information")
            End If
        Catch ex As Exception
            Err_No = "34310"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_006") & "&next=HelNewCustomer.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub
    Protected Sub lbOrdNo_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If Me.ddlOrganization.SelectedIndex <= 0 Then
                MessageBoxValidation("Please select a organization", "Information")
                Exit Sub
            End If
            Dim lbOrdNo As LinkButton = TryCast(sender, LinkButton)
            Dim row As GridViewRow = DirectCast(lbOrdNo.NamingContainer, GridViewRow)

            Me.txtRemarks.Text = ""
            Me.lblOrdRef.Text = lbOrdNo.Text
            Me.lblOrderDate.Text = row.Cells(3).Text
            Me.lblCustomerName.Text = row.Cells(5).Text
            Me.lblTelNO.Text = row.Cells(6).Text
            Me.lblOrdAmount.Text = row.Cells(7).Text
            Dim Stime As Label = DirectCast(row.FindControl("lbSTime"), Label)
            Me.lblStartTime.Text = DateTime.Parse(Stime.Text).ToString("dd-MM-yyyy HH:mm")
            Dim Etime As Label = DirectCast(row.FindControl("lbETime"), Label)
            Me.lblEndTime.Text = DateTime.Parse(Etime.Text).ToString("dd-MM-yyyy HH:mm")
            Dim Comment As Label = DirectCast(row.FindControl("lbComment"), Label)
            Me.lblComments.Text = Comment.Text
            Dim PONO As Label = DirectCast(row.FindControl("lbPONo"), Label)
            Me.lblCustPONo.Text = PONO.Text

            Dim OrdLat As Label = DirectCast(row.FindControl("lblOrdLat"), Label)
            Me.lblOrdLat.Text = IIf(OrdLat.Text = "0", "", OrdLat.Text)

            Dim OrdLng As Label = DirectCast(row.FindControl("lblOrdLng"), Label)
            Me.lblOrdLng.Text = IIf(OrdLng.Text = "0", "", OrdLng.Text)

            Dim AppCode As Label = DirectCast(row.FindControl("lblAppCode"), Label)
            Me.lblAppCode.Text = AppCode.Text


            Dim AppBy As Label = DirectCast(row.FindControl("lblAppBy"), Label)
            Me.lblAppby.Text = AppBy.Text

            Dim Usedfor As Label = DirectCast(row.FindControl("lblUsedFor"), Label)
            Me.lblReason.Text = Usedfor.Text


            Me.MapWindow.VisibleOnPageLoad = True
            Dim OrderNo As String = lbOrdNo.Text
            Me.GvOrderDetails.DataSource = ObjCustomer.GetConcileOrderDetails(Err_No, Err_Desc, OrderNo)
            Me.GvOrderDetails.DataBind()

            Me.Gv_Receipts.DataSource = GetDatasource(lbOrdNo.Text)
            Gv_Receipts.DataBind()
            UpdatePanel1.Update()

        Catch ex As Exception
            Err_No = "34310"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_006") & "&next=HelNewCustomer.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub
    Protected Sub lbReconcile_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.MapWindow.VisibleOnPageLoad = False
        Try
            If Me.ddlOrganization.SelectedIndex <= 0 Then
                MessageBoxValidation("Please select a organization", "Validation")
                Exit Sub
            End If
            Dim lbReconcile As LinkButton = TryCast(sender, LinkButton)
            Dim row As GridViewRow = DirectCast(lbReconcile.NamingContainer, GridViewRow)
            Dim OrderNo As Label = DirectCast(row.FindControl("lblOrdNo"), Label)
            If ObjCustomer.UpdateReconcileOrders(Err_No, Err_Desc, OrderNo.Text, CType(Session("User_Access"), UserAccess).UserID, "0") = True Then
                BindRefreshData()
                MessageBoxValidation("Successfully updated", "Information")
            Else
                MessageBoxValidation("Error occured while updating record", "Information")
            End If
        Catch ex As Exception
            Err_No = "34310"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_006") & "&next=HelNewCustomer.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Me.txtToDate.DateInput.Text = ""
        Me.txtFromDate.DateInput.Text = ""
        Me.ddlVan.SelectedIndex = 0
        BindRefreshData()
    End Sub

    Protected Sub btnConcile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConcile.Click
        If Me.lblOrdRef.Text = "" Then
            lbl_msg.text = "There is no order to reconcile"
            Me.MapWindow.VisibleOnPageLoad = True
            Exit Sub
        End If
        If ObjCustomer.UpdateReconcileOrders(Err_No, Err_Desc, Me.lblOrdRef.Text, CType(Session("User_Access"), UserAccess).UserID, If(Me.txtRemarks.Text = "", "0", Me.txtRemarks.Text)) = True Then
            BindRefreshData()
            MessageBoxValidation("Successfully updated", "Information")
            Me.MapWindow.VisibleOnPageLoad = False
        Else
            lbl_msg.text = "Error occured while updating record"
            Me.MapWindow.VisibleOnPageLoad = True
        End If
    End Sub
End Class