Imports SalesWorx.BO.Common
Imports log4net
Imports System.IO
Imports Telerik.Web.UI

Partial Public Class Customer_Product_Code
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim dt As New DataTable
    Dim objCustomerProdcode As New CustomerProductCode
    Dim objLogin As New SalesWorx.BO.Common.Login
    ' Dim SortField As String = ""
    Private Const ModuleName As String = "Customer-Product-Code.aspx"
    Private Const PageID As String = "P218"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private Property SortField() As String
        Get
            If ViewState("SortColumn") Is Nothing Then
                ViewState("SortColumn") = ""
            End If
            Return ViewState("SortColumn").ToString()
        End Get
        Set(ByVal value As String)
            ViewState("SortColumn") = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack() Then
            If Session.Item("USER_ACCESS") Is Nothing Then
                Session.Add("BringmeBackHere", ModuleName)
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            If Not HasAuthentication() Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If
            Filldropdowns()
            ViewState("Criteria") = "1=1"

        End If
        lblmsgPopUp.Text = ""
    End Sub
    Private Function HasAuthentication() As Boolean
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
    End Function

    Sub Filldropdowns()
        dt = New DataTable()
        Dim objCommon As New Common
        dt = objCommon.GetCustomerLocation(Err_No, Err_Desc)

        ddl_FilterCustomer.DataSource = dt
        Dim dr As DataRow
        dr = dt.NewRow()
        dr(0) = "-- Select a Customer --"
        dr(1) = "0"
        dt.Rows.InsertAt(dr, 0)
        ddl_FilterCustomer.DataValueField = "LocCode"
        ddl_FilterCustomer.DataTextField = "Location"
        ddl_FilterCustomer.DataBind()


        ddl_Customer.DataSource = dt
        ddl_Customer.DataValueField = "LocCode"
        ddl_Customer.DataTextField = "Location"
        ddl_Customer.DataBind()

        ddl_custimport.DataSource = dt
        ddl_custimport.DataValueField = "LocCode"
        ddl_custimport.DataTextField = "Location"
        ddl_custimport.DataBind()




        '' ''Dim pdt As New DataTable
        '' ''pdt = objCommon.GetAllProducts(Err_No, Err_Desc)



        '' ''Dim dr1 As DataRow
        '' ''dr1 = pdt.NewRow()
        '' ''dr1(0) = "0"
        '' ''dr1(1) = "-- Select a Product --"
        '' ''pdt.Rows.InsertAt(dr1, 0)

        '' ''ddl_FilterProduct.DataSource = pdt
        '' ''ddl_FilterProduct.DataValueField = "Item_Code"
        '' ''ddl_FilterProduct.DataTextField = "Description"
        '' ''ddl_FilterProduct.DataBind()


        '' ''ddl_Product.DataSource = pdt
        '' ''ddl_Product.DataValueField = "Item_Code"
        '' ''ddl_Product.DataTextField = "Description"
        '' ''ddl_Product.DataBind()


        'ddFilterBy.DataValueField = "MAS_Org_ID"
        'ddFilterBy.DataTextField = "Description"
        'ddFilterBy.DataSource = dt
        'ddFilterBy.DataBind()

    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        btnUpdate.Visible = False
        btnSave.Text = "Save"
        file_import.Enabled = False
        ddl_Product.Enabled = True

        txt_code.Enabled = True
        btnSave.Visible = True
        btnimport.Visible = False
        Resetfields()
        Me.MPEDivConfig.Show()
        ClassUpdatePnl.Update()
    End Sub
    Protected Sub btnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTn_Import.Click
        lblImp.Text = ""
        ClassUpdatePnl.Update()
        Me.MPImport.Show()
    End Sub
    Sub BindGrid(ByVal Criteria As String)
        Try
            Dim dt As New DataTable
            dt = objCustomerProdcode.GetCustomerProdCode(Err_No, Err_Desc, Criteria)
            Dim dv As New DataView(dt)
            If SortField <> "" Then
                dv.Sort = (SortField & " ") + SortDirection
            End If
            gvDivConfig.DataSource = dv
            gvDivConfig.DataBind()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
    Public Sub Resetfields()
        ddl_Customer.ClearSelection()
        ddl_Product.ClearSelection()
        ddl_Product.Items.Clear()
        txt_code.Text = ""
        ddl_Product.Text = ""
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If ddl_Customer.SelectedValue = "0" Then
            ' lblmsgPopUp.Text = "Please select organization / division."
            lblmsgPopUp.Text = "Please select customer."
            MPEDivConfig.Show()
            ClassUpdatePnl.Update()
            Return
        End If
        If btnSave.Text = "Save" Then
            If ddl_Product.SelectedValue = "" Then
                ' lblmsgPopUp.Text = "Please select organization / division."
                lblmsgPopUp.Text = "Please select product."
                MPEDivConfig.Show()
                ClassUpdatePnl.Update()
                Return
            End If
            If String.IsNullOrEmpty(txt_code.Text) Then
                lblmsgPopUp.Text = "Please enter the product code."
                MPEDivConfig.Show()
                ClassUpdatePnl.Update()
                Return
            End If
            'If Me.rbStandard.Checked = False And Me.rbContinue.Checked = False Then
            '    MessageBoxValidation("Please select print format.")
            '    MPEDivConfig.Show()
            '    Return
            'End If
            Dim success As Boolean = False
            Dim ItemCode As String = ddl_Product.SelectedValue
            Try
                objCustomerProdcode.CustomerCode = ddl_Customer.SelectedItem.Value
                objCustomerProdcode.Itemcode = ddl_Product.SelectedValue
                objCustomerProdcode.ProductCode = txt_code.Text
                'If Me.rbContinue.Checked = True Then
                '    objDivConfig.PrintFormat = "Continuous"
                'End If
                'If Me.rbStandard.Checked = True Then
                '    objDivConfig.PrintFormat = "Standard"
                'End If

                If objCustomerProdcode.SaveCustomerProdCode(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID.ToString()) = True Then
                    success = True
                    MessageBoxValidation("Successfully Saved.", "Information")
                    Resetfields()
                    BindGrid("1=1")
                    Me.MPEDivConfig.Hide()
                    ClassUpdatePnl.Update()
                Else
                    MessageBoxValidation("Could not be saved.", "Information")
                    Resetfields()
                    BindGrid("1=1")
                    Me.MPEDivConfig.Hide()
                    ClassUpdatePnl.Update()
                    log.Error(Err_Desc)
                End If

                If success = True Then
                    Dim ItemText As String = (New SalesWorx.BO.Common.Product).GetProdName(Err_No, Err_Desc, ItemCode)
                    objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "CUSTOMER PRODUCT CODE", Me.ddl_Customer.SelectedValue.ToString(), "Product: " & ItemText & "/ Code :  " & Me.txt_code.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")


                End If
            Catch ex1 As SqlClient.SqlException

            Catch ex As Exception
                Err_No = "74205"
                If Err_Desc Is Nothing Then
                    log.Error(GetExceptionInfo(ex))
                Else
                    log.Error(Err_Desc)
                End If
            End Try
        ElseIf btnSave.Text = "Import" Then

        End If
    End Sub
    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try

            btnUpdate.Visible = True
            btnSave.Visible = False

            file_import.Enabled = False
            ddl_Product.Enabled = True
            txt_code.Enabled = True

            btnimport.Visible = False
            Resetfields()
            'Resetfields()
            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)

            Dim CustomerID As String
            CustomerID = CType(row.FindControl("HCustomer"), HiddenField).Value
            If Not ddl_Customer.FindItemByValue(CustomerID) Is Nothing Then
                ddl_Customer.ClearSelection()
                ddl_Customer.FindItemByValue(CustomerID).Selected = True
            End If

            Dim Itemcode As String
            Itemcode = CType(row.FindControl("HItemCode"), HiddenField).Value

            ''loading Item by item code
            Dim Objrep As New SalesWorx.BO.Common.Reports()
            Dim dt As New DataTable
            dt = Objrep.GetAllItems(Err_No, Err_Desc, Itemcode)


            'Loop through the values to populate the combo box
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim item As New RadComboBoxItem()
                item.Text = dt.Rows(i).Item("Description").ToString
                item.Value = dt.Rows(i).Item("itemNo").ToString

                ddl_Product.Items.Add(item)
                item.DataBind()
            Next


            If Not ddl_Product.FindItemByValue(Itemcode) Is Nothing Then
                ddl_Product.ClearSelection()
                ddl_Product.FindItemByValue(Itemcode).Selected = True
            End If

            txt_code.Text = row.Cells(3).Text


            MPEDivConfig.Show()
            ClassUpdatePnl.Update()
        Catch ex As Exception
            Err_No = "74208"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Customer-Product-Code.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub
    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click

        If ddl_Customer.SelectedValue = "0" Then
            ' lblmsgPopUp.Text = "Please select organization / division."
            lblmsgPopUp.Text = "Please select customer."
            MPEDivConfig.Show()
            ClassUpdatePnl.Update()
            Return
        End If
        If ddl_Product.SelectedValue = "" Then
            ' lblmsgPopUp.Text = "Please select organization / division."
            lblmsgPopUp.Text = "Please select product."
            MPEDivConfig.Show()
            ClassUpdatePnl.Update()
            Return
        End If
        If String.IsNullOrEmpty(txt_code.Text) Then
            lblmsgPopUp.Text = "Please enter the product code."
            MPEDivConfig.Show()
            ClassUpdatePnl.Update()
            Return
        End If

        Dim success As Boolean = False
        Try
            objCustomerProdcode.CustomerCode = ddl_Customer.SelectedItem.Value
            objCustomerProdcode.Itemcode = ddl_Product.SelectedValue
            objCustomerProdcode.ProductCode = txt_code.Text
            Dim ItemCode As String = ddl_Product.SelectedValue

            If objCustomerProdcode.SaveCustomerProdCode(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID.ToString()) = True Then
                success = True
                MessageBoxValidation("Successfully Updated.", "Information")
                Resetfields()
                BindGrid(ViewState("Criteria").ToString())
                Me.MPEDivConfig.Hide()
                ClassUpdatePnl.Update()
            Else
                MessageBoxValidation("Could not br updated.", "Information")
                Resetfields()
                BindGrid(ViewState("Criteria").ToString())
                Me.MPEDivConfig.Hide()
                ClassUpdatePnl.Update()
            End If

            If success = True Then
                Dim ItemText As String = (New SalesWorx.BO.Common.Product).GetProdName(Err_No, Err_Desc, ItemCode)
                objLogin.SaveUserLog(Err_No, Err_Desc, "U", "MAIN MENU", "CUSTOMER PRODUCT CODE", Me.ddl_Customer.SelectedValue.ToString(), "Product: " & ItemText & "/ Code :  " & Me.txt_code.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
            End If

        Catch ex As Exception
            Err_No = "74209"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub


    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Resetfields()
        MPEDivConfig.Hide()
    End Sub
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btndelete As ImageButton = TryCast(sender, ImageButton)
        Dim row As GridViewRow = DirectCast(btndelete.NamingContainer, GridViewRow)


        Dim success As Boolean = False
        Try
            Dim CustomerID As String
            CustomerID = CType(row.FindControl("HCustomer"), HiddenField).Value

            Dim ItemID As String
            ItemID = CType(row.FindControl("HItemCode"), HiddenField).Value

            If objCustomerProdcode.DeleteCustomerProdCode(Err_No, Err_Desc, CustomerID, ItemID) = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "D", "MAIN MENU", "CUSTOMER PRODUCT CODE", CustomerID, "Product: " & ItemID, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                success = True
            End If

            If success = True Then
                MessageBoxValidation("Successfully deleted.", "Information")
                BindGrid(ViewState("Criteria").ToString())
                Resetfields()

            Else
                MessageBoxValidation("Error occured while deleting Product Code.", "Information")
                log.Error(Err_Desc)

            End If

        Catch ex As Exception
            Err_No = "74210"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub
    Protected Sub btnDeleteAll_Click()
        Try
            Dim row As GridViewRow
            Dim Success As Boolean = False
            Dim idCollection As String = ""
            For Each row In gvDivConfig.Rows
                Dim RowCheckBox As System.Web.UI.WebControls.CheckBox = row.FindControl("chkDelete")

                If RowCheckBox.Checked = True Then
                    Dim CustomerID As String
                    CustomerID = CType(row.FindControl("HCustomer"), HiddenField).Value

                    Dim ItemID As String
                    ItemID = CType(row.FindControl("HItemCode"), HiddenField).Value
                    idCollection = idCollection & CustomerID & "~" & ItemID & "|"
                    objLogin.SaveUserLog(Err_No, Err_Desc, "D", "MAIN MENU", "CUSTOMER PRODUCT CODE", CustomerID, "Product: " & ItemID, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")

                End If
            Next
            If idCollection.Trim() <> "" Then
                If objCustomerProdcode.DeleteCustomerProdCodes(Err_No, Err_Desc, idCollection) = True Then
                    Success = True
                End If
                If (Success = True) Then
                    MessageBoxValidation("Product Code(s) deleted successfully.", "Information")
                    BindGrid(ViewState("Criteria").ToString())
                    Resetfields()

                Else
                    MessageBoxValidation("Error occured while deleting Product Code(s).", "Information")
                    log.Error(Err_Desc)
                    Exit Try
                End If

            End If

        Catch ex As Exception
            Err_No = "74211"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFilter.Click
        Dim Criteria As String = "1=1"
        If ddl_FilterCustomer.SelectedValue <> "0" Then
            Criteria += " and A.Customer_ID_1='" & ddl_FilterCustomer.SelectedValue & "'"
        End If
        If ddl_FilterProduct.SelectedValue <> "" Then
            Criteria += " and A.Item_ID_1='" & ddl_FilterProduct.SelectedValue & "'"
        End If

        ViewState("Criteria") = Criteria
        BindGrid(ViewState("Criteria"))
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

    Protected Sub gvDivConfig_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvDivConfig.Sorting
        SortField = e.SortExpression
        SortDirection = "flip"
        BindGrid(ViewState("Criteria").ToString())
    End Sub

    Protected Sub gvDivConfig_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDivConfig.PageIndexChanging
        gvDivConfig.PageIndex = e.NewPageIndex
        BindGrid(ViewState("Criteria").ToString())
    End Sub

    Private Sub BTn_Import_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTn_Import.Click

    End Sub

    Private Sub btn_close_Click(sender As Object, e As ImageClickEventArgs) Handles btn_close.Click
        Resetfields()
        MPEDivConfig.Hide()
        ClassUpdatePnl.Update()
    End Sub

    Private Sub btnimport_Click1(sender As Object, e As EventArgs) Handles btnimport.Click

        If ddl_custimport.SelectedValue = "0" Then
            ' lblmsgPopUp.Text = "Please select organization / division."
            lblImp.Text = "Please select customer."
            MPImport.Show()
            ClassUpdatePnl.Update()
            Return
        End If

        If Not file_import.HasFile Then
            '  = "Please select organization / division."
            lblImp.Text = "Please select a file ."
            MPImport.Show()
            ClassUpdatePnl.Update()
            Return
        End If
        Dim spath As String = ""
        spath = ConfigurationManager.AppSettings("ExcelPath")
        If Directory.Exists(spath) = False Then
            Directory.CreateDirectory(spath)
        End If
        spath = spath & "\" & Now.ToString("ddmmyyhhMM") & file_import.FileName
        If (file_import.HasFile) Then
            If file_import.PostedFile.ContentLength > 10485760 Then
                Me.lblImp.Text = "File Size should be less than 10 MB, Please Split files and upload."
                MPImport.Show()
                ClassUpdatePnl.Update()

            Else
                If System.IO.File.Exists(spath) Then
                    System.IO.File.Delete(spath)
                End If
                file_import.SaveAs(spath)
                file_import.FileContent.Close()
                file_import.FileContent.Dispose()
                Dim success As Boolean
                success = objCustomerProdcode.ImportFile(Err_No, Err_Desc, spath, ddl_Customer.SelectedItem.Value, CType(Session("User_Access"), UserAccess).UserID.ToString())
                If success = True Then
                    MessageBoxValidation("Successfully Imported.", "Information")
                    objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "CUSTOMER PRODUCT CODE File imported", Me.ddl_Customer.SelectedValue.ToString(), "FileName: " & spath, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                    Resetfields()
                    BindGrid("1=1")
                    Me.MPImport.Hide()
                    ClassUpdatePnl.Update()
                Else
                    MessageBoxValidation("Error while importing.", "Information")
                    Resetfields()
                    BindGrid("1=1")
                    Me.MPImport.Hide()
                    ClassUpdatePnl.Update()
                    log.Error(Err_Desc)
                End If
            End If
        End If
    End Sub

    Private Sub ddl_Product_ItemsRequested(sender As Object, e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs) Handles ddl_Product.ItemsRequested
        Try

            Dim Objrep As New SalesWorx.BO.Common.Reports()
            Dim dt As New DataTable
            dt = Objrep.GetAllItems(Err_No, Err_Desc, e.Text)


            Dim ItemsPerRequest As Integer = 100 'Set the number of values to show
            Dim itemOffset As Integer = e.NumberOfItems
            Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
            e.EndOfItems = endOffset = dt.Rows.Count

            'Loop through the values to populate the combo box
            For i As Integer = itemOffset To endOffset - 1
                Dim item As New RadComboBoxItem()
                item.Text = dt.Rows(i).Item("Description").ToString
                item.Value = dt.Rows(i).Item("itemNo").ToString

                ddl_Product.Items.Add(item)
                item.DataBind()
            Next

            Me.MPEDivConfig.Show()
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub ddl_FilterProduct_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddl_FilterProduct.ItemsRequested
        Try

            Dim Objrep As New SalesWorx.BO.Common.Reports()
            Dim dt As New DataTable
            dt = Objrep.GetAllItems(Err_No, Err_Desc, e.Text)


            Dim ItemsPerRequest As Integer = 100 'Set the number of values to show
            Dim itemOffset As Integer = e.NumberOfItems
            Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
            e.EndOfItems = endOffset = dt.Rows.Count

            'Loop through the values to populate the combo box
            For i As Integer = itemOffset To endOffset - 1
                Dim item As New RadComboBoxItem()
                item.Text = dt.Rows(i).Item("Description").ToString
                item.Value = dt.Rows(i).Item("itemNo").ToString

                ddl_FilterProduct.Items.Add(item)
                item.DataBind()
            Next


        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub ddl_Product_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddl_Product.SelectedIndexChanged
        Me.MPEDivConfig.Show()
    End Sub
End Class