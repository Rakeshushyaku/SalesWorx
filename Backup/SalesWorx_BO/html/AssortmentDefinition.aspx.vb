
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports Telerik.Web.UI

Partial Public Class AssortmentDefinition
    Inherits System.Web.UI.Page
    Dim Err_No As Integer
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim dtSearch As New DataTable
    Dim objProduct As New Product
    Private dtOrdItems As New DataTable
    Private dtGetItems As New DataTable
    Private dtSlabs As New DataTable

    Private Const PageID As String = "P221"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub ProductGroupListing_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Assortment Plan Details"
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


            Me.lblOrgID.Text = Request.QueryString("ORGID").ToString()
            Me.lblOrgName.Text = Request.QueryString("ORGNAME").ToString()
            Me.lblPlanName.Text = Request.QueryString("Desc").ToString()
            Me.lblPlanId.Text = Request.QueryString("PGID").ToString()
            FillItemsList()
            LoadBonusType()



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
            Session.Remove("dtOrdItems")
            Session.Remove("dtGetItems")
            Session.Remove("dtSlabs")
            SetOrderItemsTable()
            SetGetItemsTable()
            SetSlabsTable()
            BindItemDetails(Me.lblPlanId.Text, Me.lblOrgID.Text)
            BindSlabs(Me.lblPlanId.Text)
        Else
            dtOrdItems = Session("dtOrdItems")
            dtGetItems = Session("dtGetItems")
            dtSlabs = Session("dtSlabs")
        End If
    End Sub
    Private Sub SetSlabsTable()
        Dim col As DataColumn


        col = New DataColumn()
        col.ColumnName = "SlabID"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtSlabs.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "FromQty"
        col.DataType = System.Type.[GetType]("System.Double")
        col.[ReadOnly] = False
        col.Unique = False
        dtSlabs.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "ToQty"
        col.DataType = System.Type.[GetType]("System.Double")
        col.[ReadOnly] = False
        col.Unique = False
        dtSlabs.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "GetQty"
        col.DataType = System.Type.[GetType]("System.Double")
        col.[ReadOnly] = False
        col.Unique = False
        dtSlabs.Columns.Add(col)

       

        col = New DataColumn()
        col.ColumnName = "TypeCode"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtSlabs.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "OldFromQty"
        col.DataType = System.Type.[GetType]("System.Double")
        col.[ReadOnly] = False
        col.Unique = False
        dtSlabs.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "OldToQty"
        col.DataType = System.Type.[GetType]("System.Double")
        col.[ReadOnly] = False
        col.Unique = False
        dtSlabs.Columns.Add(col)


        Session.Remove("dtSlabs")
        Session("dtSlabs") = dtSlabs
    End Sub
    Private Sub SetOrderItemsTable()
        Dim col As DataColumn


        col = New DataColumn()
        col.ColumnName = "ItemCode"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtOrdItems.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "Description"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtOrdItems.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "UOM"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtOrdItems.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "GetItem"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtOrdItems.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "IsMandatory"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtOrdItems.Columns.Add(col)




        Session.Remove("dtOrdItems")
        Session("dtOrdItems") = dtOrdItems
    End Sub
    Private Sub SetGetItemsTable()
        Dim col As DataColumn


        col = New DataColumn()
        col.ColumnName = "ItemCode"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtGetItems.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "Description"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtGetItems.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "UOM"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtGetItems.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "GetItem"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtGetItems.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "IsMandatory"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtGetItems.Columns.Add(col)




        Session.Remove("dtGetItems")
        Session("dtGetItems") = dtGetItems
    End Sub
    Sub FillItemsList()
        Dim x As New DataTable
        x = objProduct.LoadProductList(Err_No, Err_Desc, Me.lblOrgID.Text)

        ddlOrdDesc.DataTextField = "DescText"
        ddlOrdDesc.DataValueField = "DescValue"
        ddlOrdDesc.DataSource = x
        ddlOrdDesc.DataBind()
        ' ddlOrdDesc.SelectedIndex = 0


        ddlOrdCode.DataTextField = "CodeText"
        ddlOrdCode.DataValueField = "CodeValue"
        ddlOrdCode.DataSource = x
        ddlOrdCode.DataBind()
        ' ddlOrdCode.SelectedIndex = 0


        ddlgetDesc.DataTextField = "DescText"
        ddlgetDesc.DataValueField = "DescValue"
        ddlgetDesc.DataSource = x
        ddlgetDesc.DataBind()
        ' ddlgetDesc.SelectedIndex = 0


        ddlGetCode.DataTextField = "CodeText"
        ddlGetCode.DataValueField = "CodeValue"
        ddlGetCode.DataSource = x
        ddlGetCode.DataBind()
        ' ddlGetCode.SelectedIndex = 0


    End Sub


    Public Sub LoadBonusType()
        Dim dtBType As New DataTable
        dtBType.Columns.Add("Code")
        dtBType.Columns.Add("Description")
        Dim dr As DataRow = dtBType.NewRow()
        dr("Code") = "POINT"
        dr("Description") = "POINT"
        dtBType.Rows.InsertAt(dr, 0)

        dr = dtBType.NewRow()
        dr("Code") = "RECURRING"
        dr("Description") = "RECURRING"
        dtBType.Rows.Add(dr)


        'dr = dtBType.NewRow()
        'dr("Code") = "PERCENT"
        'dr("Description") = "PERCENT"
        'dtBType.Rows.Add(dr)




        Me.ddlType.DataSource = dtBType
        Me.ddlType.DataTextField = "Description"
        Me.ddlType.DataValueField = "Code"
        Me.ddlType.DataBind()
        Me.ddlType.SelectedIndex = 0






    End Sub
















    Protected Sub ddlgetCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ddlGetCode.SelectedIndexChanged
        If Me.ddlGetCode.SelectedIndex >= 0 Then
            Me.ddlgetDesc.ClearSelection()
            Me.ddlgetDesc.Text = ""
            Me.ddlgetDesc.SelectedValue = Me.ddlGetCode.SelectedValue
        Else
            Me.ddlgetDesc.ClearSelection()
            Me.ddlgetDesc.Text = ""
        End If
    End Sub

    Protected Sub ddlgetDesc_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ddlgetDesc.SelectedIndexChanged
        If Me.ddlgetDesc.SelectedIndex >= 0 Then
            Me.ddlGetCode.ClearSelection()
            Me.ddlGetCode.Text = ""
            Me.ddlGetCode.SelectedValue = Me.ddlgetDesc.SelectedValue
        Else
            Me.ddlGetCode.ClearSelection()
            Me.ddlGetCode.Text = ""
        End If
    End Sub




    Protected Sub ddlOrdCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ddlOrdCode.SelectedIndexChanged
        If Me.ddlOrdCode.SelectedIndex >= 0 Then
            Me.ddlOrdDesc.ClearSelection()
            Me.ddlOrdDesc.Text = ""
            Me.ddlOrdDesc.SelectedValue = Me.ddlOrdCode.SelectedValue
        Else
            Me.ddlOrdDesc.ClearSelection()
            Me.ddlOrdDesc.Text = ""
        End If
    End Sub

    Protected Sub ddlOrdDesc_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ddlOrdDesc.SelectedIndexChanged
        If Me.ddlOrdDesc.SelectedIndex >= 0 Then
            Me.ddlOrdCode.ClearSelection()
            Me.ddlOrdCode.Text = ""
            Me.ddlOrdCode.SelectedValue = Me.ddlOrdDesc.SelectedValue
        Else
            Me.ddlOrdCode.ClearSelection()
            Me.ddlOrdCode.Text = ""
        End If
    End Sub

    Protected Sub btnOrdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOrdAdd.Click
        dtOrdItems = Session("dtOrdItems")
        If Me.ddlOrdCode.SelectedIndex < 0 Or Me.ddlOrdDesc.SelectedIndex < 0 Then
            Me.lblMessage.Text = "Please select a order item code or description"
            lblMessage.ForeColor = Drawing.Color.Red
            lblinfo.Text = "Information"
            MpInfoError.Show()
            btnClose.Focus()
            clsPnl.Update()
            Exit Sub
        End If

        Dim s() As String = Me.ddlOrdDesc.SelectedValue.Split("$")

        Dim UOM As String = s(1).ToString().Trim()
        Dim ItemCode As String = s(0).ToString().Trim()


        If objProduct.CheckAssortmentItem(Err_No, Err_Desc, Me.lblOrgID.Text, Me.lblPlanId.Text, ItemCode) = True Then
            Me.lblMessage.Text = "The selected item already exist in the another plan"
            lblMessage.ForeColor = Drawing.Color.Red
            lblinfo.Text = "Information"
            MpInfoError.Show()
            btnClose.Focus()
            Exit Sub
        End If

        Dim st As Boolean = True


        For Each r As DataRow In dtOrdItems.Rows
            If r("ItemCode").ToString() = ItemCode Then
                st = False
                Me.lblMessage.Text = "The selected item already exist"
                lblMessage.ForeColor = Drawing.Color.Red
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
                Exit Sub
            End If
        Next

        If st = True Then
            Dim N As DataRow = dtOrdItems.NewRow
            N("ItemCode") = ItemCode
            N("Description") = Me.ddlOrdDesc.SelectedItem.Text
            N("UOM") = UOM
            N("GetItem") = "N"
            N("IsMandatory") = "False"
            dtOrdItems.Rows.Add(N)
        End If


        Me.lstOrderd.DataTextField = "Description"
        Me.lstOrderd.DataValueField = "ItemCode"
        Me.lstOrderd.DataSource = dtOrdItems
        Me.lstOrderd.DataBind()

        For Each itm As RadListBoxItem In lstOrderd.Items
            For Each t As DataRow In dtOrdItems.Rows
                If t("ItemCode").ToString() = itm.Value.ToString() And t("IsMandatory").ToString() = "True" Then
                    itm.Checked = True
                    Exit For
                End If
            Next
        Next

        Me.ddlOrdDesc.ClearSelection()
        Me.ddlOrdDesc.Text = ""

        Me.ddlOrdCode.ClearSelection()
        Me.ddlOrdCode.Text = ""

        Session.Remove("dtOrdItems")
        Session("dtOrdItems") = dtOrdItems


    End Sub

    Protected Sub lstOrderd_ItemCheck(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadListBoxItemEventArgs) Handles lstOrderd.ItemCheck
        BindMandatory()
    End Sub
    Private Sub BindMandatory()
        dtOrdItems = Session("dtOrdItems")
        For Each itm As RadListBoxItem In lstOrderd.Items
            If itm.Checked = True Then

                Dim ItemCode As String = itm.Value

                For Each i As DataRow In dtOrdItems.Rows
                    If i("ItemCode").ToString() = ItemCode Then
                        i("IsMandatory") = "True"
                        Exit For
                    End If
                Next
            Else
                Dim ItemCode As String = itm.Value
                For Each i As DataRow In dtOrdItems.Rows
                    If i("ItemCode").ToString() = ItemCode Then
                        i("IsMandatory") = "False"
                        Exit For
                    End If
                Next
            End If
        Next

        Session.Remove("dtOrdItems")
        Session("dtOrdItems") = dtOrdItems
    End Sub
    Protected Sub btnOrdRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOrdRemove.Click
        If lstOrderd.SelectedItems.Count <= 0 Then
            Me.lblMessage.Text = "Please select atleast one item from the list"
            lblMessage.ForeColor = Drawing.Color.Red
            lblinfo.Text = "Information"
            MpInfoError.Show()
            btnClose.Focus()
            clsPnl.Update()
            Exit Sub
        End If
        dtOrdItems = Session("dtOrdItems")
        For Each itm As RadListBoxItem In lstOrderd.Items
            If itm.Selected = True Then
                Dim ItemCode As String = itm.Value
                For k As Integer = 0 To dtOrdItems.Rows.Count - 1
                    If dtOrdItems.Rows(k)("ItemCode").ToString() = ItemCode Then
                        dtOrdItems.Rows.RemoveAt(k)
                        Exit For
                    End If
                Next
            End If
        Next
        Me.lstOrderd.DataTextField = "Description"
        Me.lstOrderd.DataValueField = "ItemCode"
        Me.lstOrderd.DataSource = dtOrdItems
        Me.lstOrderd.DataBind()


        For Each itm As RadListBoxItem In lstOrderd.Items
            Dim ItemCode As String = itm.Value
            For Each i As DataRow In dtOrdItems.Rows
                If i("ItemCode").ToString() = ItemCode And i("IsMandatory") = "True" Then
                    itm.Checked = True
                    Exit For
                End If
            Next
        Next





        Session.Remove("dtOrdItems")
        Session("dtOrdItems") = dtOrdItems

    End Sub

    Protected Sub btnGetAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetAdd.Click
        dtGetItems = Session("dtGetItems")
        If Me.ddlGetCode.SelectedIndex < 0 Or Me.ddlgetDesc.SelectedIndex < 0 Then
            Me.lblMessage.Text = "Please select a get item code or description"
            lblMessage.ForeColor = Drawing.Color.Red
            lblinfo.Text = "Information"
            MpInfoError.Show()
            btnClose.Focus()
            clsPnl.Update()
            Exit Sub
        End If

        Dim s() As String = Me.ddlgetDesc.SelectedValue.Split("$")

        Dim UOM As String = s(1).ToString().Trim()
        Dim ItemCode As String = s(0).ToString().Trim()

        Dim st As Boolean = True


        For Each r As DataRow In dtGetItems.Rows
            If r("ItemCode").ToString() = ItemCode Then
                st = False
                Me.lblMessage.Text = "The selected item already exist"
                lblMessage.ForeColor = Drawing.Color.Red
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
                Exit Sub
            End If
        Next

        If st = True Then
            Dim N As DataRow = dtGetItems.NewRow
            N("ItemCode") = ItemCode
            N("Description") = Me.ddlgetDesc.SelectedItem.Text
            N("UOM") = UOM
            N("GetItem") = "Y"
            N("IsMandatory") = "False"
            dtGetItems.Rows.Add(N)
        End If


        Me.lstGet.DataTextField = "Description"
        Me.lstGet.DataValueField = "ItemCode"
        Me.lstGet.DataSource = dtGetItems
        Me.lstGet.DataBind()

       

        Me.ddlgetDesc.ClearSelection()
        Me.ddlgetDesc.Text = ""

        Me.ddlGetCode.ClearSelection()
        Me.ddlGetCode.Text = ""

        Session.Remove("dtGetItems")
        Session("dtGetItems") = dtGetItems
    End Sub

    Protected Sub btnGetRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetRemove.Click
        If lstGet.SelectedItems.Count <= 0 Then
            Me.lblMessage.Text = "Please select atleast one item from the list"
            lblMessage.ForeColor = Drawing.Color.Red
            lblinfo.Text = "Information"
            MpInfoError.Show()
            btnClose.Focus()
            clsPnl.Update()
            Exit Sub
        End If
        dtGetItems = Session("dtGetItems")
        For Each itm As RadListBoxItem In lstGet.Items
            If itm.Selected = True Then
                Dim ItemCode As String = itm.Value
                For k As Integer = 0 To dtGetItems.Rows.Count - 1
                    If dtGetItems.Rows(k)("ItemCode").ToString() = ItemCode Then
                        dtGetItems.Rows.RemoveAt(k)
                        Exit For
                    End If
                Next
            End If
        Next
        Me.lstGet.DataTextField = "Description"
        Me.lstGet.DataValueField = "ItemCode"
        Me.lstGet.DataSource = dtGetItems
        Me.lstGet.DataBind()



        Session.Remove("dtGetItems")
        Session("dtGetItems") = dtGetItems

    End Sub

    Protected Sub btnCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        If lstOrderd.SelectedItems.Count <= 0 Then
            Me.lblMessage.Text = "Please select atleast one item from the list"
            lblMessage.ForeColor = Drawing.Color.Red
            lblinfo.Text = "Information"
            MpInfoError.Show()
            btnClose.Focus()
            clsPnl.Update()
            Exit Sub
        End If
        dtOrdItems = Session("dtOrdItems")
        dtGetItems = Session("dtGetItems")
        Dim st As Boolean = True
        For Each itm As RadListBoxItem In lstOrderd.Items
            If itm.Selected = True Then
                Dim ItemCode As String = itm.Value
                For k As Integer = 0 To dtGetItems.Rows.Count - 1
                    If dtGetItems.Rows(k)("ItemCode").ToString() = ItemCode Then
                        st = False
                        Exit For
                    End If
                Next

                If st = True Then
                    Dim UOM As String = Nothing
                    Dim h() As DataRow = dtOrdItems.Select("ItemCode='" & ItemCode & "'")
                    If h.Length > 0 Then
                        UOM = h(0).Item("UOM").ToString()
                    End If
                    Dim N As DataRow = dtGetItems.NewRow
                    N("ItemCode") = ItemCode
                    N("Description") = itm.Text
                    N("UOM") = UOM
                    N("GetItem") = "Y"
                    N("IsMandatory") = "False"
                    dtGetItems.Rows.Add(N)
                End If
                st = True

            End If
        Next
        Me.lstGet.DataTextField = "Description"
        Me.lstGet.DataValueField = "ItemCode"
        Me.lstGet.DataSource = dtGetItems
        Me.lstGet.DataBind()


        Session.Remove("dtGetItems")
        Session("dtGetItems") = dtGetItems

    End Sub

    Protected Sub btnAddSlab_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddSlab.Click
        dtSlabs = Session("dtSlabs")
        If Me.txtFromQty.Text = "" Or CDec(IIf(Me.txtFromQty.Text = "", "0", Me.txtFromQty.Text)) = 0 Or Me.txtFromQty.Text = "" Or CDec(IIf(Me.txtToQty.Text = "", "0", Me.txtToQty.Text)) = 0 Or Me.txtGetQty.Text = "" Or CDec(IIf(Me.txtGetQty.Text = "", "0", Me.txtGetQty.Text)) = 0 Then
            Me.lblMessage.Text = "Please enter a valid from,to and get quantity"
            lblMessage.ForeColor = Drawing.Color.Red
            lblinfo.Text = "Validation"
            MpInfoError.Show()
            btnClose.Focus()
            Exit Sub
        End If


        If Me.txtFromQty.Text <> "" And Me.txtFromQty.Text <> "0" And Me.txtToQty.Text <> "" And Me.txtToQty.Text <> "0" Then
            If CDec(Me.txtToQty.Text) <= CDec(Me.txtFromQty.Text) Then
                Me.lblMessage.Text = "To quantity should be greater than from quantity"
                lblMessage.ForeColor = Drawing.Color.Red
                lblinfo.Text = "Validation"
                MpInfoError.Show()
                btnClose.Focus()
                Exit Sub
            End If
        End If

        Dim st As Boolean = True

        Dim FromQty As Long = 0
        Dim toQty As Long = 0
        Dim GetQty As Long = 0

        FromQty = CLng(IIf(Me.txtFromQty.Text = "", "0", Me.txtFromQty.Text))
        toQty = CLng(IIf(Me.txtToQty.Text = "", "0", Me.txtToQty.Text))
        GetQty = CLng(IIf(Me.txtGetQty.Text = "", "0", Me.txtGetQty.Text))




        'If objProduct.CheckAssortmentSlab(Err_No, Err_Desc, Me.lblPlanId.Text, IIf(Me.hfSlabID.Value = "", "0", Me.hfSlabID.Value), FromQty, toQty) = True Then
        '    Me.lblMessage.Text = "This rule  already exist in the same plan"
        '    lblMessage.ForeColor = Drawing.Color.Red
        '    lblinfo.Text = "Information"
        '    MpInfoError.Show()
        '    btnClose.Focus()
        '    Exit Sub
        'End If





        If Me.btnAddSlab.Text = "Add" Or (Me.btnAddSlab.Text = "Update" And Me.txtFromQty.Text <> hfOldFrom.Value Or Me.txtToQty.Text <> Me.hfOldTo.Value) Then
            For Each r As DataRow In dtSlabs.Rows
                If ((CLng(r("OldFromQty").ToString()) >= FromQty And CLng(r("OldFromQty").ToString()) <= toQty Or (FromQty >= CLng(r("OldFromQty").ToString()) And FromQty <= CLng(r("OldToQty").ToString()))) Or (((CLng(r("OldToQty").ToString())) >= FromQty And CLng(r("OldToQty").ToString()) <= toQty) Or (toQty >= CLng(r("OldFromQty").ToString())) And toQty <= CLng(r("OldToQty").ToString()))) Then

                    Me.lblMessage.Text = "This rule already exist.please define a new rule"
                    lblMessage.ForeColor = Drawing.Color.Red
                    lblinfo.Text = "Information"
                    MpInfoError.Show()
                    btnClose.Focus()
                    Exit Sub
                End If
            Next
        End If

        'If Me.btnAddSlab.Text = "Add" Or (Me.btnAddSlab.Text = "Update" And Me.txtFromQty.Text <> hfOldFrom.Value Or Me.txtToQty.Text <> Me.hfOldTo.Value) Then
        '    For Each r As DataRow In dtSlabs.Rows
        '        If ((CLng(r("OldFromQty").ToString()) >= FromQty And CLng(r("OldFromQty").ToString()) <= toQty Or (FromQty >= CLng(r("OldFromQty").ToString()) And FromQty <= CLng(r("OldToQty").ToString()))) Or (((CLng(r("OldToQty").ToString())) >= FromQty And CLng(r("OldToQty").ToString()) <= toQty) Or (toQty >= CLng(r("OldFromQty").ToString())) And toQty <= CLng(r("OldToQty").ToString()))) Then

        '            Me.lblMessage.Text = "This rule already exist.please define a new rule"
        '            lblMessage.ForeColor = Drawing.Color.Red
        '            lblinfo.Text = "Information"
        '            MpInfoError.Show()
        '            btnClose.Focus()
        '            Exit Sub
        '        End If
        '    Next
        'End If

        If Me.btnAddSlab.Text = "Add" Then
            Dim N As DataRow = dtSlabs.NewRow
            N("FromQty") = FromQty
            N("toQty") = toQty
            N("OldFromQty") = FromQty
            N("OldtoQty") = toQty
            N("TypeCode") = Me.ddlType.SelectedItem.Text
            N("GetQty") = GetQty
            N("SlabID") = "0"
            dtSlabs.Rows.Add(N)
        End If

        If Me.btnAddSlab.Text = "Update" Then
            For Each h As DataRow In dtSlabs.Rows
                If h("SlabID").ToString() = hfSlabID.Value And CLng(h("OldFromQty").ToString()) = CLng(Me.hfOldFrom.Value) And CLng(h("OldToQty").ToString()) = CLng(Me.hfOldTo.Value) Then
                    h("FromQty") = FromQty
                    h("OldFromQty") = FromQty
                    h("toQty") = toQty
                    h("OldToQty") = toQty
                    h("TypeCode") = Me.ddlType.SelectedItem.Text
                    h("GetQty") = GetQty
                    Exit For
                End If
            Next
        End If

        dtSlabs.DefaultView.Sort = "FromQty"
        Me.dgvSlabs.DataSource = dtSlabs
        Me.dgvSlabs.DataBind()


        Me.btnAddSlab.Text = "Add"
        Me.txtFromQty.Text = ""
        Me.txtToQty.Text = ""
        Me.txtGetQty.Text = ""
        Me.hfSlabID.Value = ""
        Me.hfOldFrom.Value = "0"
        Me.hfOldTo.Value = "0"
        LoadBonusType()


        Session.Remove("dtSlabs")
        Session("dtSlabs") = dtSlabs
        Me.txtFromQty.Focus()
    End Sub
    Private Sub dgvSlab_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgvSlabs.RowCommand

        Try
            
            If (e.CommandName = "EditSlab") Then
                Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim SlabID As String = Convert.ToString(dgvSlabs.DataKeys(row.RowIndex).Value)
                
                Dim FromQty As Long = CLng(dgvSlabs.Rows(row.RowIndex).Cells(1).Text)
                Dim ToQty As Long = CLng(dgvSlabs.Rows(row.RowIndex).Cells(2).Text)
                Dim CodeType As String = CStr(dgvSlabs.Rows(row.RowIndex).Cells(3).Text)
                Dim GetQty As Long = CLng(dgvSlabs.Rows(row.RowIndex).Cells(4).Text)


                Me.hfSlabID.Value = SlabID
                Me.txtFromQty.Text = FromQty
                Me.hfOldFrom.Value = FromQty

                Me.txtToQty.Text = ToQty
                Me.hfOldTo.Value = ToQty
                Me.txtGetQty.Text = GetQty
                Me.ddlType.SelectedValue = CodeType

                Me.btnAddSlab.Text = "Update"

            End If
            If (e.CommandName = "DeleteSlab") Then
                Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim SlabID As String = Convert.ToString(dgvSlabs.DataKeys(row.RowIndex).Value)

                Dim FromQty As Long = CLng(dgvSlabs.Rows(row.RowIndex).Cells(1).Text)
                Dim ToQty As Long = CLng(dgvSlabs.Rows(row.RowIndex).Cells(2).Text)
                Dim CodeType As String = CStr(dgvSlabs.Rows(row.RowIndex).Cells(3).Text)
                Dim GetQty As Long = CLng(dgvSlabs.Rows(row.RowIndex).Cells(4).Text)


                dtSlabs = Session("dtSlabs")
             
                For k As Integer = 0 To dtSlabs.Rows.Count - 1
                    If dtSlabs.Rows(k)("SlabID").ToString() = SlabID And CLng(dtSlabs.Rows(k)("OldFromQty").ToString()) = FromQty And CLng(dtSlabs.Rows(k)("OldToQty").ToString()) = ToQty Then
                        dtSlabs.Rows.RemoveAt(k)
                        Exit For
                    End If
                Next
                dtSlabs.DefaultView.Sort = "FromQty"
                Me.dgvSlabs.DataSource = dtSlabs
                Me.dgvSlabs.DataBind()


                Session.Remove("dtSlabs")
                Session("dtSlabs") = dtSlabs



            End If

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("information.aspx?mode=1&errno=" & "74071" & "&msg=" & AppMsgHandler.GetErrorMessage("SWX_BO_Msg_005") & "&next=Welcome.aspx", False)
        Finally
        End Try
    End Sub

    Protected Sub btnCanSlab_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCanSlab.Click
        Me.txtFromQty.Text = ""
        Me.txtGetQty.Text = ""
        Me.txtToQty.Text = ""
        LoadBonusType()
        Me.hfSlabID.Value = ""
        Me.hfOldFrom.Value = "0"
        Me.hfOldTo.Value = "0"
        Me.btnAddSlab.Text = "Add"
        Me.txtFromQty.Focus()
    End Sub

    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirm.Click
        dtOrdItems = Session("dtOrdItems")
        dtGetItems = Session("dtGetItems")
        dtSlabs = Session("dtSlabs")

        If dtOrdItems.Rows.Count <= 0 Then
            Me.lblMessage.Text = "Please add atleast one order item"
            lblMessage.ForeColor = Drawing.Color.Red
            lblinfo.Text = "Information"
            MpInfoError.Show()
            btnClose.Focus()
            Exit Sub
        End If

        If dtGetItems.Rows.Count <= 0 Then
            Me.lblMessage.Text = "Please add atleast one bonus item"
            lblMessage.ForeColor = Drawing.Color.Red
            lblinfo.Text = "Information"
            MpInfoError.Show()
            btnClose.Focus()
            Exit Sub
        End If

        If dtSlabs.Rows.Count <= 0 Then
            Me.lblMessage.Text = "Please add atleast one bonus rule"
            lblMessage.ForeColor = Drawing.Color.Red
            lblinfo.Text = "Information"
            MpInfoError.Show()
            btnClose.Focus()
            Exit Sub
        End If

        If objProduct.SaveAssortment(Err_No, Err_Desc, CInt(lblPlanId.Text), dtOrdItems, dtGetItems, dtSlabs, CType(Session.Item("USER_ACCESS"), UserAccess).UserID) Then
            Response.Redirect("AdminBonusAssortment.aspx?OID=" & Me.lblOrgID.Text, False)
        Else
            Me.lblMessage.Text = "Error while saving record"
            lblMessage.ForeColor = Drawing.Color.Red
            lblinfo.Text = "Information"
            MpInfoError.Show()
            btnClose.Focus()
            Exit Sub
        End If


    End Sub

    Private Sub BindItemDetails(ByVal PlandID As String, ByVal OrgID As String)

        Dim x As New DataTable
        x = objProduct.LoadAssortmentItems(Err_No, Err_Desc, CInt(PlandID), OrgID)

        dtOrdItems = Session("dtOrdItems")
        dtGetItems = Session("dtGetItems")
        If x.Rows.Count > 0 Then
            For Each dr As DataRow In x.Rows
                If dr("GetItem").ToString() = "N" Then
                    Dim oRow As DataRow = dtOrdItems.NewRow()
                    oRow("ItemCode") = dr("ItemCode").ToString()
                    oRow("Description") = dr("Description").ToString()
                    oRow("UOM") = dr("UOM").ToString()
                    oRow("GetItem") = dr("GetItem").ToString()
                    oRow("IsMandatory") = IIf(dr("IsMandatory").ToString() = "Y", "True", "False")
                    dtOrdItems.Rows.Add(oRow)
                ElseIf dr("GetItem").ToString() = "Y" Then
                    Dim oRow As DataRow = dtGetItems.NewRow()
                    oRow("ItemCode") = dr("ItemCode").ToString()
                    oRow("Description") = dr("Description").ToString()
                    oRow("UOM") = dr("UOM").ToString()
                    oRow("GetItem") = dr("GetItem").ToString()
                    oRow("IsMandatory") = "False"
                    dtGetItems.Rows.Add(oRow)
                End If
            Next

            Me.lstGet.DataTextField = "Description"
            Me.lstGet.DataValueField = "ItemCode"
            Me.lstGet.DataSource = dtGetItems
            Me.lstGet.DataBind()

            Me.lstOrderd.DataTextField = "Description"
            Me.lstOrderd.DataValueField = "ItemCode"
            Me.lstOrderd.DataSource = dtOrdItems
            Me.lstOrderd.DataBind()

            For Each itm As RadListBoxItem In lstOrderd.Items
                For Each t As DataRow In dtOrdItems.Rows
                    If t("ItemCode").ToString() = itm.Value.ToString() And t("IsMandatory").ToString() = "True" Then
                        itm.Checked = True
                        Exit For
                    End If
                Next
            Next


            Session.Remove("dtOrdItems")
            Session("dtOrdItems") = dtOrdItems

            Session.Remove("dtGetItems")
            Session("dtGetItems") = dtGetItems

        End If

    End Sub


    Private Sub BindSlabs(ByVal PlandID As String)

        Dim y As New DataTable

        y = objProduct.LoadAssortmentSlabs(Err_No, Err_Desc, CInt(PlandID))
        dtSlabs = Session("dtSlabs")
        If y.Rows.Count > 0 Then
            For Each dr As DataRow In y.Rows
                Dim oRow As DataRow = dtSlabs.NewRow()
                oRow("SlabID") = dr("SlabID").ToString()
                oRow("FromQty") = CDec(dr("FromQty").ToString())
                oRow("ToQty") = CDec(dr("ToQty").ToString())
                oRow("TypeCode") = dr("TypeCode").ToString()
                oRow("GetQty") = CDec(dr("GetQty").ToString())
                oRow("OldFromQty") = CDec(dr("OldFromQty").ToString())
                oRow("OldToQty") = CDec(dr("OldToQty").ToString())
                dtSlabs.Rows.Add(oRow)
            Next
            Me.dgvSlabs.DataSource = Nothing
            Me.dgvSlabs.DataSource = dtSlabs
            Me.dgvSlabs.DataBind()
            Session.Remove("dtSlabs")
            Session("dtSlabs") = dtSlabs
        End If

    End Sub
    Protected Sub btnGoBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGoBack.Click
        Response.Redirect("AdminBonusAssortment.aspx?OID=" & Me.lblOrgID.Text, False)
    End Sub
End Class



