
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports Telerik.Web.UI

Partial Public Class AssortmentDefinitionMinQty
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

        Else
            dtOrdItems = Session("dtOrdItems")
            dtGetItems = Session("dtGetItems")
            dtSlabs = Session("dtSlabs")
        End If
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
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

        col = New DataColumn()
        col.ColumnName = "Qty"
        col.DataType = System.Type.[GetType]("System.Decimal")
        col.[ReadOnly] = False
        col.Unique = False
        dtOrdItems.Columns.Add(col)
        col = New DataColumn()
        col.ColumnName = "MaxQty"
        col.DataType = System.Type.[GetType]("System.Decimal")
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

        'col = New DataColumn()
        'col.ColumnName = "TypeCode"
        'col.DataType = System.Type.[GetType]("System.String")
        'col.[ReadOnly] = False
        'col.Unique = False
        'dtGetItems.Columns.Add(col)

        'col = New DataColumn()
        'col.ColumnName = "Getqty"
        'col.DataType = System.Type.[GetType]("System.Decimal")
        'col.[ReadOnly] = False
        'col.Unique = False
        'dtGetItems.Columns.Add(col)

        Session.Remove("dtGetItems")
        Session("dtGetItems") = dtGetItems
    End Sub
    Private Sub ddlOrdCode_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddlOrdCode.ItemsRequested


        Dim Objrep As New SalesWorx.BO.Common.Reports()
        Dim dt As New DataTable
        dt = objProduct.LoadProductList(Err_No, Err_Desc, Me.lblOrgID.Text, e.Text)


        Dim ItemsPerRequest As Integer = 100 'Set the number of values to show
        Dim itemOffset As Integer = e.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
        e.EndOfItems = endOffset = dt.Rows.Count

        'Loop through the values to populate the combo box
        For i As Integer = itemOffset To endOffset - 1
            Dim item As New RadComboBoxItem()
            item.Text = dt.Rows(i).Item("CodeText").ToString
            item.Value = dt.Rows(i).Item("CodeValue").ToString

            ddlOrdCode.Items.Add(item)
            item.DataBind()
        Next

    End Sub
    Private Sub ddlOrdDesc_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddlOrdDesc.ItemsRequested


        Dim Objrep As New SalesWorx.BO.Common.Reports()
        Dim dt As New DataTable
        dt = objProduct.LoadProductList(Err_No, Err_Desc, Me.lblOrgID.Text, e.Text)


        Dim ItemsPerRequest As Integer = 100 'Set the number of values to show
        Dim itemOffset As Integer = e.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
        e.EndOfItems = endOffset = dt.Rows.Count

        'Loop through the values to populate the combo box
        For i As Integer = itemOffset To endOffset - 1
            Dim item As New RadComboBoxItem()
            item.Text = dt.Rows(i).Item("DescText").ToString
            item.Value = dt.Rows(i).Item("DescValue").ToString

            ddlOrdDesc.Items.Add(item)
            item.DataBind()
        Next

    End Sub

    Private Sub ddlgetDesc_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddlgetDesc.ItemsRequested


        Dim Objrep As New SalesWorx.BO.Common.Reports()
        Dim dt As New DataTable
        dt = objProduct.LoadProductList(Err_No, Err_Desc, Me.lblOrgID.Text, e.Text)


        Dim ItemsPerRequest As Integer = 100 'Set the number of values to show
        Dim itemOffset As Integer = e.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
        e.EndOfItems = endOffset = dt.Rows.Count

        'Loop through the values to populate the combo box
        For i As Integer = itemOffset To endOffset - 1
            Dim item As New RadComboBoxItem()
            item.Text = dt.Rows(i).Item("DescText").ToString
            item.Value = dt.Rows(i).Item("DescValue").ToString

            ddlgetDesc.Items.Add(item)
            item.DataBind()
        Next

    End Sub

    Private Sub ddlGetCode_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddlGetCode.ItemsRequested


        Dim Objrep As New SalesWorx.BO.Common.Reports()
        Dim dt As New DataTable
        dt = objProduct.LoadProductList(Err_No, Err_Desc, Me.lblOrgID.Text, e.Text)


        Dim ItemsPerRequest As Integer = 100 'Set the number of values to show
        Dim itemOffset As Integer = e.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
        e.EndOfItems = endOffset = dt.Rows.Count

        'Loop through the values to populate the combo box
        For i As Integer = itemOffset To endOffset - 1
            Dim item As New RadComboBoxItem()
            item.Text = dt.Rows(i).Item("CodeText").ToString
            item.Value = dt.Rows(i).Item("CodeValue").ToString

            ddlGetCode.Items.Add(item)
            item.DataBind()
        Next

    End Sub

    Sub FillItemsList()
        'Dim x As New DataTable
        'x = objProduct.LoadProductList(Err_No, Err_Desc, Me.lblOrgID.Text, )

        ''ddlOrdDesc.DataTextField = "DescText"
        ''ddlOrdDesc.DataValueField = "DescValue"
        ''ddlOrdDesc.DataSource = x
        ''ddlOrdDesc.DataBind()
        '' ddlOrdDesc.SelectedIndex = 0


        ''ddlOrdCode.DataTextField = "CodeText"
        ''ddlOrdCode.DataValueField = "CodeValue"
        ''ddlOrdCode.DataSource = x
        ''ddlOrdCode.DataBind()
        '' ddlOrdCode.SelectedIndex = 0


        ''ddlgetDesc.DataTextField = "DescText"
        ''ddlgetDesc.DataValueField = "DescValue"
        ''ddlgetDesc.DataSource = x
        ''ddlgetDesc.DataBind()
        '' ddlgetDesc.SelectedIndex = 0


        'ddlGetCode.DataTextField = "CodeText"
        'ddlGetCode.DataValueField = "CodeValue"
        'ddlGetCode.DataSource = x
        'ddlGetCode.DataBind()
        '' ddlGetCode.SelectedIndex = 0


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
        If Not String.IsNullOrEmpty(Me.ddlGetCode.SelectedValue) Then
            Me.ddlgetDesc.ClearSelection()
            Me.ddlgetDesc.Text = ""
            Me.ddlgetDesc.SelectedValue = Me.ddlGetCode.SelectedValue
            If Not String.IsNullOrEmpty(ddlGetCode.SelectedValue) Then
                Dim ids() As String
                ids = ddlGetCode.SelectedValue.Split("$")
                ddlgetDesc.Text = (New SalesWorx.BO.Common.Product).GetItemNameFromCode(Err_No, Err_Desc, ids(0), lblOrgID.Text)
            End If
        Else
            Me.ddlgetDesc.ClearSelection()
            Me.ddlgetDesc.Text = ""
        End If
    End Sub

    Protected Sub ddlgetDesc_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ddlgetDesc.SelectedIndexChanged
        If Not String.IsNullOrEmpty(Me.ddlgetDesc.SelectedValue) Then
            Me.ddlGetCode.ClearSelection()
            Me.ddlGetCode.Text = ""
            Me.ddlGetCode.SelectedValue = Me.ddlgetDesc.SelectedValue
            Dim desc() As String
            desc = Me.ddlgetDesc.SelectedValue.Split("$")
            Me.ddlGetCode.Text = desc(0)
        Else
            Me.ddlGetCode.ClearSelection()
            Me.ddlGetCode.Text = ""
        End If
    End Sub
    Protected Sub ddlOrdCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ddlOrdCode.SelectedIndexChanged
        If Not String.IsNullOrEmpty(Me.ddlOrdCode.SelectedValue) Then
            Me.ddlOrdDesc.ClearSelection()
            Me.ddlOrdDesc.Text = ""
            Me.ddlOrdDesc.SelectedValue = Me.ddlOrdCode.SelectedValue
            If Not String.IsNullOrEmpty(ddlOrdCode.SelectedValue) Then
                Dim ids() As String
                ids = ddlOrdCode.SelectedValue.Split("$")
                ddlOrdDesc.Text = (New SalesWorx.BO.Common.Product).GetItemNameFromCode(Err_No, Err_Desc, ids(0), lblOrgID.Text)
            End If
        Else
            Me.ddlOrdDesc.ClearSelection()
            Me.ddlOrdDesc.Text = ""
        End If
    End Sub

    Protected Sub ddlOrdDesc_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ddlOrdDesc.SelectedIndexChanged
        If Not String.IsNullOrEmpty(Me.ddlOrdDesc.SelectedValue) Then
            Me.ddlOrdCode.ClearSelection()
            Me.ddlOrdCode.Text = ""
            Me.ddlOrdCode.SelectedValue = Me.ddlOrdDesc.SelectedValue
            Dim desc() As String
            desc = Me.ddlOrdDesc.SelectedValue.Split("$")
            Me.ddlOrdCode.Text = desc(0)
        Else
            Me.ddlOrdCode.ClearSelection()
            Me.ddlOrdCode.Text = ""
        End If
    End Sub

    Protected Sub btnOrdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOrdAdd.Click
        dtOrdItems = Session("dtOrdItems")
        If String.IsNullOrEmpty(Me.ddlOrdDesc.SelectedValue) Or String.IsNullOrEmpty(Me.ddlOrdDesc.SelectedValue) Then
            MessageBoxValidation("Please select a order item code or description", "Information")
            Exit Sub
        End If


        If Me.txtFromQty.Text = "" Or CDec(IIf(Me.txtFromQty.Text = "", "0", Me.txtFromQty.Text)) = 0 Then
            MessageBoxValidation("Please enter minimum order quantity", "Validation")
            Exit Sub
        End If


        Dim FromQty As Long = 0
        Dim toQty As Long = 0
        Dim GetQty As Long = 0

        FromQty = CLng(IIf(Me.txtFromQty.Text = "", "0", Me.txtFromQty.Text))
        toQty = 9999




        Dim s() As String = Me.ddlOrdDesc.SelectedValue.Split("$")

        Dim UOM As String = s(1).ToString().Trim()
        Dim ItemCode As String = s(0).ToString().Trim()


        If objProduct.CheckAssortmentItem(Err_No, Err_Desc, Me.lblOrgID.Text, Me.lblPlanId.Text, ItemCode) = True Then
            MessageBoxValidation("The selected item already exist in the another plan", "Information")
            Exit Sub
        End If

        Dim st As Boolean = True


        For Each r As DataRow In dtOrdItems.Rows
            If r("ItemCode").ToString() = ItemCode Then
                st = False
                MessageBoxValidation("The selected item already exist", "Information")
                Exit Sub
            End If
        Next

        If st = True Then
            Dim N As DataRow = dtOrdItems.NewRow
            N("ItemCode") = ItemCode
            N("Description") = (New SalesWorx.BO.Common.Product).GetItemNameFromCode(Err_No, Err_Desc, s(0), lblOrgID.Text)
            N("UOM") = UOM
            N("GetItem") = "N"
            N("Qty") = FromQty
            N("MaxQty") = toQty
            N("IsMandatory") = "True"
            dtOrdItems.Rows.Add(N)
        End If


        'Me.lstOrderd.DataTextField = "Description"
        'Me.lstOrderd.DataValueField = "ItemCode"
        Me.rgOrdered.DataSource = dtOrdItems
        Me.rgOrdered.DataBind()

        'For Each itm As RadListBoxItem In lstOrderd.Items
        '    For Each t As DataRow In dtOrdItems.Rows
        '        If t("ItemCode").ToString() = itm.Value.ToString() And t("IsMandatory").ToString() = "True" Then
        '            itm.Checked = True
        '            Exit For
        '        End If
        '    Next
        'Next

        Me.ddlOrdDesc.ClearSelection()
        Me.ddlOrdDesc.Text = ""

        Me.ddlOrdCode.ClearSelection()
        Me.ddlOrdCode.Text = ""
        Me.txtFromQty.Text = ""
        Me.txtToQty.Text = ""


        Session.Remove("dtOrdItems")
        Session("dtOrdItems") = dtOrdItems


    End Sub

    Protected Sub chkIgnore_CheckedChanged(sender As Object, e As EventArgs)
        Dim btn1 As CheckBox = TryCast(sender, CheckBox)
        Dim itm As GridDataItem = DirectCast(btn1.NamingContainer, GridDataItem)
        dtOrdItems = Session("dtOrdItems")
        Dim OItemCode As Label = DirectCast(itm.FindControl("lblOItemCode"), Label)
        If btn1.Checked = False Then
            For Each l As DataRow In dtOrdItems.Rows
                If l("ItemCode").ToString() = OItemCode.Text Then
                    l("IsMandatory") = "False"
                    Exit For
                End If
            Next

        ElseIf btn1.Checked = True Then
            For Each l As DataRow In dtOrdItems.Rows
                If l("ItemCode").ToString() = OItemCode.Text Then
                    l("IsMandatory") = "True"
                    Exit For
                End If
            Next

        End If

        Session.Remove("dtOrderItems")
        Session("dtOrderItems") = dtOrdItems
        rgOrdered.DataSource = dtOrdItems
        rgOrdered.DataBind()

    End Sub
    Private Sub BindMandatory()
        dtOrdItems = Session("dtOrdItems")
        For Each itm As RadListBoxItem In rgOrdered.Items
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
        If rgOrdered.SelectedItems.Count <= 0 Then
            MessageBoxValidation("Please select atleast one item from the list", "Information")
            Exit Sub
        End If
        dtOrdItems = Session("dtOrdItems")
        For Each itm As GridItem In rgOrdered.Items
            If itm.Selected = True Then
                Dim ItemCode As Label = DirectCast(itm.FindControl("lblOItemCode"), Label)
                For k As Integer = 0 To dtOrdItems.Rows.Count - 1
                    If dtOrdItems.Rows(k)("ItemCode").ToString() = ItemCode.Text Then
                        dtOrdItems.Rows.RemoveAt(k)
                        Exit For
                    End If
                Next
            End If
        Next

        'Me.lstOrderd.DataTextField = "Description"
        'Me.lstOrderd.DataValueField = "ItemCode"
        rgOrdered.DataSource = dtOrdItems
        rgOrdered.DataBind()


        'For Each itm As RadListBoxItem In lstOrderd.Items
        '    Dim ItemCode As String = itm.Value
        '    For Each i As DataRow In dtOrdItems.Rows
        '        If i("ItemCode").ToString() = ItemCode And i("IsMandatory") = "True" Then
        '            itm.Checked = True
        '            Exit For
        '        End If
        '    Next
        'Next





        Session.Remove("dtOrdItems")
        Session("dtOrdItems") = dtOrdItems

    End Sub

    Protected Sub btnGetAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetAdd.Click
        dtGetItems = Session("dtGetItems")
        If String.IsNullOrEmpty(Me.ddlGetCode.SelectedValue) Or String.IsNullOrEmpty(Me.ddlgetDesc.SelectedValue) Then
            MessageBoxValidation("Please select a get item code or description", "Information")
            Exit Sub
        End If
        'If Me.txtGetQty.Text = "" Or CDec(IIf(Me.txtGetQty.Text = "", "0", Me.txtGetQty.Text)) = 0 Then
        '    MessageBoxValidation("Please enter bonus quantity", "Validation")
        '    Exit Sub
        'End If



        'Dim GetQty As Long = 0

        'GetQty = CLng(IIf(Me.txtGetQty.Text = "", "0", Me.txtGetQty.Text))



        Dim s() As String = Me.ddlgetDesc.SelectedValue.Split("$")

        Dim UOM As String = s(1).ToString().Trim()
        Dim ItemCode As String = s(0).ToString().Trim()

        Dim st As Boolean = True


        For Each r As DataRow In dtGetItems.Rows
            If r("ItemCode").ToString() = ItemCode Then
                st = False
                MessageBoxValidation("The selected item already exist", "Information")
                Exit Sub
            End If
        Next

        If st = True Then
            Dim N As DataRow = dtGetItems.NewRow
            N("ItemCode") = ItemCode
            N("Description") = (New SalesWorx.BO.Common.Product).GetItemNameFromCode(Err_No, Err_Desc, s(0), lblOrgID.Text)
            N("UOM") = UOM
            N("GetItem") = "Y"
            'N("GetQty") = GetQty
            '  N("TypeCode") = Me.ddlType.SelectedValue
            N("IsMandatory") = "False"
            dtGetItems.Rows.Add(N)
        End If


        Me.rgGet.DataSource = dtGetItems
        Me.rgGet.DataBind()



        Me.ddlgetDesc.ClearSelection()
        Me.ddlgetDesc.Text = ""

        Me.ddlGetCode.ClearSelection()
        Me.ddlGetCode.Text = ""
     
        Session.Remove("dtGetItems")
        Session("dtGetItems") = dtGetItems
    End Sub

    Protected Sub btnGetRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetRemove.Click
        If rgGet.SelectedItems.Count <= 0 Then
            MessageBoxValidation("Please select atleast one item from the list", "Information")
            Exit Sub
        End If
        dtGetItems = Session("dtGetItems")
        For Each itm As GridItem In rgGet.Items
            If itm.Selected = True Then
                Dim ItemCode As Label = DirectCast(itm.FindControl("lblBItemCode"), Label)
                For k As Integer = 0 To dtGetItems.Rows.Count - 1
                    If dtGetItems.Rows(k)("ItemCode").ToString() = ItemCode.Text Then
                        dtGetItems.Rows.RemoveAt(k)
                        Exit For
                    End If
                Next
            End If
        Next
       
        Me.rgGet.DataSource = dtGetItems
        Me.rgGet.DataBind()



        Session.Remove("dtGetItems")
        Session("dtGetItems") = dtGetItems

    End Sub


  

  

    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirm.Click
        dtOrdItems = Session("dtOrdItems")
        dtGetItems = Session("dtGetItems")
        dtSlabs = Session("dtSlabs")

        If dtOrdItems.Rows.Count <= 0 Then
            MessageBoxValidation("Please add atleast one order item", "Information")
            Exit Sub
        End If

        If dtGetItems.Rows.Count <= 0 Then
            MessageBoxValidation("Please add atleast one bonus item", "Information")
            Exit Sub
        End If

        If Me.ddlType.SelectedIndex < 0 Then
            MessageBoxValidation("Please select a type", "Information")
            Exit Sub
        End If

        If Me.txtGetQty.Text = "" Or Me.txtGetQty.Text = "0" Then
            MessageBoxValidation("Please enter bonus quantity", "Information")
            Exit Sub
        End If


        'If dtSlabs.Rows.Count <= 0 Then
        '    MessageBoxValidation("Please add atleast one bonus rule", "Information")
        '    Exit Sub
        'End If

        If objProduct.SaveAssortmentMinQty(Err_No, Err_Desc, CInt(lblPlanId.Text), dtOrdItems, dtGetItems, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, Me.ddlType.SelectedValue, CDec(IIf(Me.txtGetQty.Text = "", "0", Me.txtGetQty.Text))) Then
            Response.Redirect("AdminBonusAssortment.aspx?OID=" & Me.lblOrgID.Text, False)
        Else
            MessageBoxValidation("Error while saving record", "Information")
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
                    oRow("Qty") = dr("Qty").ToString()
                    oRow("MaxQty") = dr("MaxQty").ToString()
                    Me.ddlType.SelectedValue = dr("TypeCode").ToString()
                    Me.txtGetQty.Text = dr("getQty").ToString()
                    oRow("IsMandatory") = IIf(dr("IsMandatory").ToString() = "Y", "True", "False")
                    dtOrdItems.Rows.Add(oRow)
                ElseIf dr("GetItem").ToString() = "Y" Then
                    Dim oRow As DataRow = dtGetItems.NewRow()
                    oRow("ItemCode") = dr("ItemCode").ToString()
                    oRow("Description") = dr("Description").ToString()
                    oRow("UOM") = dr("UOM").ToString()
                    oRow("GetItem") = dr("GetItem").ToString()
                    'oRow("GetQty") = dr("GetQty").ToString()
                    'oRow("TypeCode") = dr("TypeCode").ToString()
                    oRow("IsMandatory") = "False"

                    dtGetItems.Rows.Add(oRow)
                End If
            Next

            'Me.lstGet.DataTextField = "Description"
            'Me.lstGet.DataValueField = "ItemCode"
            Me.rgGet.DataSource = dtGetItems
            Me.rgGet.DataBind()

          
            Me.rgOrdered.DataSource = dtOrdItems
            Me.rgOrdered.DataBind()

            'For Each itm As RadListBoxItem In lstOrderd.Items
            '    For Each t As DataRow In dtOrdItems.Rows
            '        If t("ItemCode").ToString() = itm.Value.ToString() And t("IsMandatory").ToString() = "True" Then
            '            itm.Checked = True
            '            Exit For
            '        End If
            '    Next
            'Next


            Session.Remove("dtOrdItems")
            Session("dtOrdItems") = dtOrdItems

            Session.Remove("dtGetItems")
            Session("dtGetItems") = dtGetItems

        End If

    End Sub


 
    Protected Sub btnGoBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGoBack.Click
        Response.Redirect("AdminBonusAssortment.aspx?OID=" & Me.lblOrgID.Text, False)
    End Sub
End Class



