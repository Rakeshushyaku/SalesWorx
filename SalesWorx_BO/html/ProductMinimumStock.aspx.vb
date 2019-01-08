Imports SalesWorx.BO.Common
Imports Telerik.Web.UI
Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports ExcelLibrary.SpreadSheet
Public Class ProductMinimumStock
    Inherits System.Web.UI.Page
    Dim objcommon As New SalesWorx.BO.Common.Common
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim objCustomer As New SalesWorx.BO.Common.Customer
    Dim objProduct As New SalesWorx.BO.Common.Product
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const PageID As String = "P95"
    Private dt As New DataTable
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objControl As New AppControl

    Private Property ddl_Org As Object

#Region "Events"

#Region "Main Panel Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        Try
            If Not IsPostBack Then
                Dim HasPermission As Boolean = False




                LoadOrgHeads()


                BindProductStock()

                ViewState("FileType") = Nothing
                ViewState("FileName") = Nothing
                ViewState("CSVName") = Nothing


            Else
                MPEImport.VisibleOnPageLoad = False
                MPEAdd.VisibleOnPageLoad = False
            End If


        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Order_005") & "&next=Welcome.aspx&Title=Bonus Definition", False)
        End Try
    End Sub



    Protected Sub ddl_FilterOrg_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_FilterOrg.SelectedIndexChanged
        Try



            ddl_FilterCustomer.ClearSelection()
            ddl_FilterCustomer.Items.Clear()
            ddl_FilterCustomer.Text = ""




            BindProductStock()
            Panel.Update()

        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Private Sub ddl_Customer_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddl_Customer.ItemsRequested
        Try

            Dim Objrep As New SalesWorx.BO.Common.Reports()
            Dim dt As New DataTable



            dt = objcommon.GetProductsByOrg(Err_No, Err_Desc, ddl_OrgAdd.SelectedValue)

            Dim ItemsPerRequest As Integer = 100
            Dim itemOffset As Integer = e.NumberOfItems
            Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
            e.EndOfItems = endOffset = dt.Rows.Count


            For i As Integer = itemOffset To endOffset - 1
                Dim item As New RadComboBoxItem()
                item.Text = dt.Rows(i).Item("Description").ToString
                item.Value = dt.Rows(i).Item("item_code").ToString

                ddl_Customer.Items.Add(item)
                item.DataBind()
            Next

        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Private Sub dgv_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgvItems.Sorting
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"

        BindProductStock()
    End Sub

    Private Sub ddl_FilterCustomer_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddl_FilterCustomer.ItemsRequested
        Try

            Dim dt As New DataTable
            dt = objcommon.GetProductsByOrg(Err_No, Err_Desc, ddl_FilterOrg.SelectedValue)
            Dim ItemsPerRequest As Integer = 100
            Dim itemOffset As Integer = e.NumberOfItems
            Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
            e.EndOfItems = endOffset = dt.Rows.Count

            For i As Integer = itemOffset To endOffset - 1
                Dim item As New RadComboBoxItem()
                item.Text = dt.Rows(i).Item("Description").ToString
                item.Value = dt.Rows(i).Item("item_code").ToString

                ddl_FilterCustomer.Items.Add(item)
                item.DataBind()
            Next
            '  BindProductStock()
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub



    Private Sub btn_clearFilter_Click(sender As Object, e As EventArgs) Handles btn_clearFilter.Click
        ddl_FilterOrg.ClearSelection()
        ddl_FilterCustomer.ClearSelection()

        ddl_FilterCustomer.Items.Clear()
        ddl_FilterCustomer.Text = ""


        dgvItems.DataSource = Nothing
        dgvItems.DataBind()

        Panel.Update()


    End Sub

    Private Sub btn_Search_Click(sender As Object, e As EventArgs) Handles btn_Search.Click
        BindProductStock()
        Panel.Update()

    End Sub


#End Region

#Region "Add Pop UP events"

    Private Sub ddl_OrgAdd_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddl_OrgAdd.SelectedIndexChanged
        Try
            If ddl_OrgAdd.SelectedItem.Value = 0 Then
                ddl_Customer.ClearSelection()
                ddl_Customer.Items.Clear()
                ddl_Customer.Text = ""


            End If

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try


    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Try

            ddl_OrgAdd.ClearSelection()
            ddl_OrgAdd.SelectedIndex = 0



            ddl_Customer.ClearSelection()
            ddl_Customer.Items.Clear()
            ddl_Customer.Text = ""
            ddl_Customer.Enabled = True
            txtQty.Text = ""



            Me.lblmsgPopUp.Text = ""

            Me.MPEAdd.VisibleOnPageLoad = True
            btnSave.Visible = True
            btnUpdate.Visible = False

            ddl_OrgAdd.Enabled = True

            ddl_Customer.Enabled = True


            Panel.Update()



        Catch ex As Exception

            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If ddl_OrgAdd.SelectedIndex <= 0 Then
            lblmsgPopUp.Text = "Please Select a Organization."
            MPEAdd.VisibleOnPageLoad = True
            Return
        End If
        If String.IsNullOrEmpty(txtQty.Text.Trim()) Then
            lblmsgPopUp.Text = "Please Enter Minimum Stock Qunatity."
            MPEAdd.VisibleOnPageLoad = True
            Return
        End If

        Dim success As Boolean = False
        Dim customer As String
        Dim InventoryId As String = "0"
        Dim ProductId As String = "0"
        Dim Is_Optional As String = "Y"
        Try


            customer = ddl_Customer.SelectedValue



            If Not String.IsNullOrEmpty(ddl_Customer.SelectedValue) Then
                Dim IDs() As String
                IDs = customer.Split("$")
                InventoryId = IDs(0)
                ProductId = IDs(1)
            Else
                MessageBoxValidation("Please Select Product .", "Information")
                BindProductStock()
                MPEAdd.VisibleOnPageLoad = False
                Panel.Update()
                log.Error(Err_Desc)

            End If






            Dim dt_rslt As New DataTable
            Dim rslt As String = ""



            If (objcommon.SaveProductMinimumStock(Err_No, Err_Desc, ddl_OrgAdd.SelectedItem.Value, InventoryId, txtQty.Text.Trim(), CType(Session("User_Access"), UserAccess).UserID)) = True Then
                success = True
                MessageBoxValidation("Successfully Saved.", "Information")

                BindProductStock()
                MPEAdd.VisibleOnPageLoad = False
                Panel.Update()
            Else
                MessageBoxValidation("Could not be saved.", "Information")
                BindProductStock()
                MPEAdd.VisibleOnPageLoad = False
                Panel.Update()
                log.Error(Err_Desc)

            End If


            If success = True Then

                objLogin.SaveUserLog(Err_No, Err_Desc, "S", "Product Minimum Stock ", "Organisation", Me.ddl_OrgAdd.SelectedValue.ToString(), "Product: " & Me.ddl_Customer.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")

            End If

        Catch ex1 As SqlClient.SqlException
            log.Error(ex1.Message.ToString())
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            Err_No = "74205"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub


#End Region

#Region "Delete"
    Protected Sub btnDeleteAll_Click()
        Try
            Dim row As GridViewRow
            Dim Success As Boolean = False
            Dim idCollection As String = ""
            For Each row In dgvItems.Rows
                Dim RowCheckBox As System.Web.UI.WebControls.CheckBox = row.FindControl("chkDelete")

                If RowCheckBox.Checked = True Then
                    Dim Customer As String
                    Customer = row.Cells(1).Text

                    Dim Item As String
                    Item = row.Cells(2).Text


                    Dim vat_value As String
                    vat_value = row.Cells(3).Text

                    Dim vat_code As String
                    vat_code = row.Cells(4).Text

                    Dim InventoryItemId As String
                    InventoryItemId = CType(row.FindControl("lblInventoryId"), Label).Text

                    If objcommon.DeleteProductMinimumStock(Err_No, Err_Desc, ddl_FilterOrg.SelectedItem.Value, InventoryItemId) = True Then
                        objLogin.SaveUserLog(Err_No, Err_Desc, "D", "Product Minimum Stock ", "Organisation", Me.ddl_OrgAdd.SelectedValue.ToString(), "Product: " & Me.ddl_Customer.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")

                        Success = True
                    End If


                End If
            Next
            If Success = True Then
                MessageBoxValidation("Successfully deleted.", "Information")
                BindProductStock()
            Else
                MessageBoxValidation("Error occured while deleting VAT rule.", "Information")
                log.Error(Err_Desc)
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
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btndelete As ImageButton = TryCast(sender, ImageButton)
        Dim row As GridViewRow = DirectCast(btndelete.NamingContainer, GridViewRow)


        Dim success As Boolean = False
        Try
            Dim Customer As String
            Customer = row.Cells(1).Text

            Dim Item As String
            Item = row.Cells(2).Text


            Dim vat_value As String
            vat_value = row.Cells(3).Text

            Dim vat_code As String
            vat_code = row.Cells(4).Text

            Dim InventoryItemId As String
            InventoryItemId = CType(row.FindControl("lblInventoryId"), Label).Text

            If objcommon.DeleteProductMinimumStock(Err_No, Err_Desc, ddl_FilterOrg.SelectedItem.Value, InventoryItemId) = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "D", "Product Minimum Stock", "Product Minimum Stock", Me.ddl_OrgAdd.SelectedValue.ToString(), "Product : " & Me.ddl_Customer.Text & " Van: " & "Me.ddl_VanAdd.Text" & "Distribution_CTL_ID :  " & InventoryItemId, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")

                success = True
            End If

            If success = True Then
                MessageBoxValidation("Successfully deleted.", "Information")
                BindProductStock()
            Else
                MessageBoxValidation("Error occured while deleting Product Minimum Stock.", "Information")
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

#End Region

#Region "Edit and Update"

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            lblmsgPopUp.Text = ""
            lblUpMsg.Text = ""
            btnUpdate.Visible = True
            btnSave.Visible = False

            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)


            Dim Org_ID As String
            Org_ID = CType(row.FindControl("lblMAS_Org_ID"), Label).Text

            Dim ProductId As String
            ProductId = CType(row.FindControl("lblItem"), Label).Text

            Dim CustomerID As String
            CustomerID = CType(row.FindControl("lblInventoryId"), Label).Text

            Dim Objrep As New SalesWorx.BO.Common.Reports()
            Dim dt1 As New DataTable

            dt1 = objcommon.GetProductsByOrg(Err_No, Err_Desc, ddl_OrgAdd.SelectedValue)


            For i As Integer = 0 To dt1.Rows.Count - 1
                Dim item As New RadComboBoxItem()
                item.Text = dt1.Rows(i).Item("Description").ToString
                item.Value = dt1.Rows(i).Item("item_code").ToString

                ddl_Customer.Items.Add(item)
                item.DataBind()
            Next






            If Not ddl_Customer.FindItemByValue(ProductId) Is Nothing Then
                ddl_Customer.ClearSelection()
                ddl_Customer.FindItemByValue(ProductId).Selected = True
            End If


            If Not ddl_OrgAdd.FindItemByValue(Org_ID) Is Nothing Then
                ddl_OrgAdd.ClearSelection()
                ddl_OrgAdd.FindItemByValue(Org_ID).Selected = True
            End If






            Dim Qty As String
            Qty = CType(row.FindControl("LblQty"), Label).Text
            txtQty.Text = Qty





            ddl_OrgAdd.Enabled = False

            ddl_Customer.Enabled = False
            MPEAdd.VisibleOnPageLoad = True

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

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs)

        Dim success As Boolean = False
        Dim Is_Optional As String = "Y"



        Dim customer As String
            Dim InventoryId As String = "0"
            Dim ProductId As String = "0"

            Try


                customer = ddl_Customer.SelectedValue



                If Not String.IsNullOrEmpty(ddl_Customer.SelectedValue) Then
                    Dim IDs() As String
                    IDs = customer.Split("$")
                    InventoryId = IDs(0)
                    ProductId = IDs(1)
                Else
                    MessageBoxValidation("Please Select Product .", "Information")
                BindProductStock()
                MPEAdd.VisibleOnPageLoad = False
                    Panel.Update()
                    log.Error(Err_Desc)

                End If





                Dim dt_rslt As New DataTable
            Dim rslt As String = ""


            If (objcommon.SaveProductMinimumStock(Err_No, Err_Desc, ddl_OrgAdd.SelectedItem.Value, InventoryId, txtQty.Text.Trim(), CType(Session("User_Access"), UserAccess).UserID)) = True Then
                success = True
                MessageBoxValidation("Successfully Updated.", "Information")
                BindProductStock()
                MPEAdd.VisibleOnPageLoad = False
                Panel.Update()
            Else
                MessageBoxValidation("Could not be Updated.", "Information")
                BindProductStock()
                MPEAdd.VisibleOnPageLoad = False
                Panel.Update()
                log.Error(Err_Desc)
            End If

            If success = True Then
                    objLogin.SaveUserLog(Err_No, Err_Desc, "U", "Product Minimum Stock ", "Organisation", Me.ddl_OrgAdd.SelectedValue.ToString(), "Product: " & Me.ddl_Customer.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")

            End If



        Catch ex As Exception
            Err_No = "74205"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

#End Region

#Region "Import and Export"
    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try

            If Me.ddl_FilterOrg.SelectedIndex > 0 Then
                Dim dtData As New DataTable
                Dim Customer As String
                Dim ProdVal As String = ""
                Dim CustomerID As String = ""
                Dim SiteID As String = ""
                Dim SID As String = ""

                Dim IDs() As String
                If ddl_FilterCustomer.SelectedValue <> "" Then
                    Customer = ddl_FilterCustomer.SelectedValue
                    IDs = Customer.Split("$")
                    CustomerID = IDs(0)
                    SiteID = IDs(1)
                Else
                    CustomerID = 0
                    SiteID = 0
                End If

                Dim dtOriginal As New DataTable()
                dtOriginal = objcommon.GetAllProductsStock(Err_No, Err_Desc, ddl_FilterOrg.SelectedItem.Value, SiteID)
                Dim dtTemp As New DataTable()
                dtTemp.Columns.Add("Item Code")
                dtTemp.Columns.Add("Item Description")
                dtTemp.Columns.Add("Minimum Stock Qty")
                Dim drAddItem As DataRow
                For i As Integer = 0 To dtOriginal.Rows.Count - 1
                    drAddItem = dtTemp.NewRow()
                    drAddItem(0) = IIf(dtOriginal.Rows(i)("item_code") Is DBNull.Value, "All", dtOriginal.Rows(i)("item_code").ToString())
                    drAddItem(1) = IIf(dtOriginal.Rows(i)("Description") Is DBNull.Value, "All", dtOriginal.Rows(i)("Description").ToString())
                    drAddItem(2) = IIf(dtOriginal.Rows(i)("Qty") Is DBNull.Value, "All", dtOriginal.Rows(i)("Qty").ToString())
                    dtTemp.Rows.Add(drAddItem)
                Next

                If dtOriginal.Rows.Count = 0 Then

                    MessageBoxValidation("There is no data.", "Information")
                    Exit Sub

                    drAddItem = dtTemp.NewRow()
                    drAddItem(0) = ""
                    drAddItem(1) = ""
                    drAddItem(2) = ""


                    dtTemp.Rows.Add(drAddItem)
                End If


                Dim dg As New DataGrid()
                dg.DataSource = dtTemp
                dg.DataBind()
                If dtTemp.Rows.Count > 0 Then

                    Dim fn As String = "Product_Minimum_Stock" + Now.ToString("ddMMMyyHHmmss") + ".xls"
                    Dim d As New DataSet
                    d.Tables.Add(dtTemp)

                    ExportToExcel(fn, d)

                End If
            Else
                MessageBoxValidation("Please select a organization", "Information")
                Exit Sub
            End If

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("information.aspx?mode=1&errno=" & "74071" & "&msg=" & AppMsgHandler.GetErrorMessage("SWX_BO_Msg_005") & "&next=welcome.aspx", False)
        Finally
        End Try
    End Sub


#End Region
#End Region


#Region "Internal Functions"
#Region "Main Panel FXNS"

    Private Sub BindProductStock()

        Dim dtData As New DataTable
        Dim Customer As String
        Dim ProdVal As String = ""
        Dim ProductID As String = ""
        Dim SiteID As String = ""
        Dim SID As String = ""

        Dim IDs() As String
        If ddl_FilterCustomer.SelectedValue <> "" Then
            Customer = ddl_FilterCustomer.SelectedValue
            IDs = Customer.Split("$")
            ProductID = IDs(0)
            SiteID = IDs(1)
        Else
            ProductID = 0
            SiteID = 0
        End If




        dtData = objcommon.GetAllProductsStock(Err_No, Err_Desc, ddl_FilterOrg.SelectedItem.Value, SiteID)
        Dim dv As New DataView(dtData)
        If ViewState("SortField") <> "" Then
            dv.Sort = (ViewState("SortField") & " ") + SortDirection
        End If

        Me.dgvItems.DataSource = Nothing
        Me.dgvItems.DataSource = dv
        Me.dgvItems.DataBind()
        PnlGridData.Visible = True

    End Sub

    Sub LoadOrgHeads()
        Try


            Dim objCommon As New Common
            Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
            Dim dt_org As New DataTable
            dt_org = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
            ddl_FilterOrg.DataSource = dt_org
            ddl_FilterOrg.Items.Clear()
            ddl_FilterOrg.Items.Add(New RadComboBoxItem("-- Select a Organization --", "0"))
            ddl_FilterOrg.AppendDataBoundItems = True
            ddl_FilterOrg.DataValueField = "MAS_Org_ID"
            ddl_FilterOrg.DataTextField = "Description"
            ddl_FilterOrg.DataBind()


            ddl_OrgAdd.DataSource = dt_org
            ddl_OrgAdd.Items.Clear()
            ddl_OrgAdd.Items.Add(New RadComboBoxItem("-- Select a Organization --", "0"))
            ddl_OrgAdd.AppendDataBoundItems = True
            ddl_OrgAdd.DataValueField = "MAS_Org_ID"
            ddl_OrgAdd.DataTextField = "Description"
            ddl_OrgAdd.DataBind()

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try


    End Sub

    Private Sub dgv_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgvItems.PageIndexChanging
        dgvItems.PageIndex = e.NewPageIndex
        BindProductStock()
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



#End Region


#Region "Add POP Fxns"


    Private Sub GetItemDescription()

        Dim dtData As New DataTable
        Dim Customer As String
        Dim ProdVal As String = ""
        Dim ProductID As String = ""
        Dim SiteID As String = ""
        Dim SID As String = ""

        Dim IDs() As String
        If ddl_Customer.SelectedValue <> "" Then
            Customer = ddl_Customer.SelectedValue
            IDs = Customer.Split("$")
            ProductID = IDs(0)
            SiteID = IDs(1)
        Else
            ProductID = 0
            SiteID = 0
        End If

        dtData = objcommon.GetProductsDescription(Err_No, Err_Desc, ddl_OrgAdd.SelectedItem.Value, SiteID)
        txtQty.Text = dtData.Rows(0)("Description").ToString()




    End Sub


#End Region

#Region "Other Function"

    Private Sub ExportToExcel(ByVal strFileName As String, ByVal ds As DataSet)


        Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & strFileName
        WriteXLSFile(fn, ds)

        Dim sFileName As String = strFileName
        Dim sFullPath As String = fn
        Dim fileBytes As Byte() = System.IO.File.ReadAllBytes(sFullPath)

        Dim context As System.Web.HttpContext = System.Web.HttpContext.Current
        context.Response.ContentType = "application/vnd.ms-excel"
        context.Response.AddHeader("Content-Disposition", "attachment;filename=" + sFileName)
        context.Response.Clear()
        context.Response.BinaryWrite(fileBytes)
        context.Response.Flush()
        context.ApplicationInstance.CompleteRequest()


    End Sub
    Public Function WriteXLSFile(ByVal pFileName As String, ByVal pDataSet As DataSet) As Boolean
        Try

            Dim workbook As Workbook = New Workbook()
            Dim worksheet As Worksheet
            Dim iRow As Integer = 0
            Dim iCol As Integer = 0
            Dim sTemp As String = String.Empty
            Dim dTemp As Double = 0
            Dim iTemp As Integer = 0

            Dim count As Integer = 0
            Dim iTotalRows As Integer = 0
            Dim iSheetCount As Integer = 0


            If Not pDataSet Is Nothing And pDataSet.Tables.Count > 0 Then


                For Each dt As DataTable In pDataSet.Tables


                    iSheetCount = iSheetCount + 1
                    worksheet = New Worksheet("Sheet" & iSheetCount.ToString())


                    For Each dc As DataColumn In dt.Columns
                        worksheet.Cells(iRow, iCol) = New Cell(dc.ColumnName)
                        iCol = iCol + 1
                    Next


                    iRow = 1
                    For Each dr As DataRow In dt.Rows
                        iCol = 0
                        For Each dc As DataColumn In dt.Columns
                            sTemp = dr(dc.ColumnName).ToString()
                            worksheet.Cells(iRow, iCol) = New Cell(sTemp)
                            iCol = iCol + 1
                        Next
                        iRow = iRow + 1
                    Next


                    workbook.Worksheets.Add(worksheet)
                    iTotalRows = iTotalRows + iRow
                Next
            End If


            workbook.Save(pFileName)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function


#End Region

#End Region









#Region "Import Functionlaity"
    Private Sub btnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImport.Click
        lbLog.Visible = False
        If Me.ExcelFileUpload.FileName = Nothing Then

            Me.lblUpMsg.Text = "Select filename "

            Me.MPEImport.VisibleOnPageLoad = True
            Exit Sub
        End If


        Session("dtProMinStock") = Nothing
        Dim dtErrors As New DataTable
        dtErrors = SetErrorsTable().Copy
        Dim Str As New StringBuilder

        Dim TotSuccess As Integer = 0
        Dim TotFailed As Integer = 0
        Try


            ViewState("FileType") = Me.ExcelFileUpload.PostedFile.ContentType
            If ExcelFileUpload.FileName.ToString.ToLower().EndsWith(".xls") Or ExcelFileUpload.FileName.ToString.ToLower().EndsWith(".xlsx") Then

                Dim Foldername As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath")
                If Not Foldername.EndsWith("\") Then
                    Foldername = Foldername & "\"
                End If
                If Directory.Exists(Foldername.Substring(0, Len(Foldername) - 1)) = False Then
                    Directory.CreateDirectory(Foldername.Substring(0, Len(Foldername) - 1))
                End If
                If ExcelFileUpload.FileName.ToString.EndsWith(".csv") Then
                    Dim FName As String
                    FName = Now.Hour & Now.Minute & Now.Second & ExcelFileUpload.FileName
                    ViewState("FileName") = Foldername & FName
                    ViewState("CSVName") = FName
                Else
                    ViewState("FileName") = Foldername & Now.Hour & Now.Minute & Now.Second & ExcelFileUpload.FileName
                End If

                ExcelFileUpload.SaveAs(ViewState("FileName"))

                Try
                    Dim st As Boolean = False

                    If ViewState("FileType") IsNot Nothing And ViewState("FileName") IsNot Nothing Then
                        Dim TempTbl As New DataTable
                        If TempTbl.Rows.Count > 0 Then
                            TempTbl.Rows.Clear()
                        End If
                        Dim col As DataColumn

                        col = New DataColumn
                        col.ColumnName = "Organisation Id"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)


                        col = New DataColumn
                        col.ColumnName = "Item Code"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)



                        col = New DataColumn
                        col.ColumnName = "Minimum Stock Qty"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)







                        If ViewState("FileName").ToString.EndsWith(".csv") Then
                            TempTbl = DoCSVUpload()
                        ElseIf ViewState("FileName").ToString.EndsWith(".xls") Then
                            TempTbl = DoXLSUpload()
                        ElseIf ViewState("FileName").ToString.EndsWith(".xlsx") Then
                            TempTbl = DoXLSXUpload()
                        End If

                        If TempTbl.Columns.Count = 3 Then
                            If Not (TempTbl.Columns(0).ColumnName.ToLower = "organisation id" And TempTbl.Columns(1).ColumnName.ToLower = "item code" And TempTbl.Columns(2).ColumnName.ToLower = "minimum stock qty") Then
                                Me.lblUpMsg.Text = "Please check the template columns are correct"
                                Me.MPEImport.VisibleOnPageLoad = True
                                Exit Sub
                            End If
                        Else
                            Me.lblUpMsg.Text = "Invalid Template"
                            Me.MPEImport.VisibleOnPageLoad = True
                            Exit Sub
                        End If




                        If TempTbl.Rows.Count = 0 Then
                            Me.lblUpMsg.Text = "There is no data in your file."
                            Me.MPEImport.VisibleOnPageLoad = True
                            Exit Sub
                        End If

                        Dim RowNo As String = Nothing
                        Dim ErrorText As String = Nothing

                        If TempTbl.Rows.Count > 0 Then
                            Dim idx As Integer

                            For idx = 0 To TempTbl.Rows.Count - 1
                                Dim Item_code As String = Nothing
                                Dim Org_id As String = Nothing
                                Dim Qty As String = ""


                                Dim isValidRow As Boolean = True
                                Org_id = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, "", TempTbl.Rows(idx)(0).ToString().Trim())
                                Item_code = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, "", TempTbl.Rows(idx)(1).ToString().Trim())

                                Qty = IIf(TempTbl.Rows(idx)(2) Is DBNull.Value, "", TempTbl.Rows(idx)(2).ToString().Trim())

                                If Org_id.Trim() = "" Or Org_id Is Nothing Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Organisation Id: is mandatory " + Org_id + " ,"
                                    isValidRow = False
                                    TotFailed += 1
                                Else
                                    Dim dt_fsr As New DataTable
                                    dt_fsr = objcommon.ValidateExportOrganisationId(Err_No, Err_Desc, Org_id)
                                    If dt_fsr.Rows.Count = 0 Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Invalid Organization ID: " + Org_id + " ,"
                                        isValidRow = False
                                        TotFailed += 1
                                    Else

                                    End If
                                End If

                                If Item_code.Trim() = "" Or Item_code Is Nothing Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Item Code: is mandatory " + Item_code + " ,"
                                    isValidRow = False
                                    TotFailed += 1

                                Else
                                Dim dt_fsr As New DataTable
                                dt_fsr = objcommon.ValidateExportProductItemCode(Err_No, Err_Desc, Item_code)
                                If dt_fsr.Rows.Count = 0 Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Invalid Item Code: " + Item_code + " ,"
                                    isValidRow = False
                                    TotFailed += 1
                                Else

                                End If
                                End If


                        If Qty.Trim() = "" Or Qty Is Nothing Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Quantity is mandatory " + Qty + " ,"
                                    isValidRow = False
                                    TotFailed += 1
                                End If






                                If Not (RowNo Is Nothing And ErrorText Is Nothing) Then
                                    Dim h As DataRow = dtErrors.NewRow()
                                    h("RowNo") = RowNo
                                    h("LogInfo") = ErrorText
                                    dtErrors.Rows.Add(h)
                                    RowNo = Nothing
                                    ErrorText = Nothing
                                    isValidRow = True
                                Else


                                    If (objcommon.SaveProductMinimumStock(Err_No, Err_Desc, Org_id, Item_code, Qty, CType(Session("User_Access"), UserAccess).UserID)) = True Then
                                        TotSuccess = TotSuccess + 1
                                        Dim h As DataRow = dtErrors.NewRow()
                                        h("RowNo") = idx + 2
                                        h("LogInfo") = "Successfully uploaded"
                                        dtErrors.Rows.Add(h)
                                        RowNo = Nothing
                                        ErrorText = Nothing
                                        isValidRow = True
                                        BindProductStock()
                                    Else
                                        Dim h As DataRow = dtErrors.NewRow()
                                            h("RowNo") = idx + 2
                                            h("LogInfo") = "Error occured while uploading this row"
                                            dtErrors.Rows.Add(h)
                                            RowNo = Nothing
                                            ErrorText = Nothing
                                            isValidRow = True
                                        End If
                                    End If

                            Next
                        End If




                        DeleteExcel()
                        lblUpMsg.Text = IIf(TotFailed = 0, "Successfully uploaded", "One or more rows has invalid data. Please check the log")
                        Me.MPEImport.VisibleOnPageLoad = True
                        BindProductStock()

                    End If

                    dgvErros.Visible = False
                    If dtErrors.Rows.Count > 0 Then
                        lbLog.Visible = True
                    Else
                        lbLog.Visible = False
                    End If
                    Me.dgvErros.DataSource = dtErrors
                    Me.dgvErros.DataBind()
                    Session.Remove("dtDisCtl")
                    Session("dtProMinStock") = dtErrors.Copy


                    Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & "DisCtlLog_" & Now.ToString("yyyyMMdd") + ".txt"
                    DataTable2CSV(dtErrors, fn, vbTab)
                    If dtErrors.Rows.Count > 0 Then
                        lbLog.Visible = True
                    End If
                    Session.Remove("SimpleLogInfo")
                    Session("SimpleLogInfo") = fn




                Catch ex As Exception

                    Err_No = "74085"
                    If Err_Desc Is Nothing Then
                        log.Error(GetExceptionInfo(ex))
                    Else
                        log.Error(Err_Desc)
                    End If
                End Try
            Else

                lblUpMsg.Text = "Please import valid Excel template."
                Me.MPEImport.VisibleOnPageLoad = True
            End If


        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub btnImportWindow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportWindow.Click

        Me.lblUpMsg.Text = ""
        Session("dtProMinStock") = Nothing
        dgvErros.DataSource = Nothing
        dgvErros.DataBind()
        dgvErros.Visible = False
        lbLog.Visible = False
        Me.MPEImport.VisibleOnPageLoad = True

    End Sub


    Sub DataTable2CSV(ByVal table As DataTable, ByVal filename As String, ByVal sepChar As String)
        Dim writer As System.IO.StreamWriter
        Try
            writer = New System.IO.StreamWriter(filename)


            Dim sep As String = ""
            Dim builder As New System.Text.StringBuilder
            For Each col As DataColumn In table.Columns
                builder.Append(sep).Append(col.ColumnName)
                sep = sepChar
            Next
            writer.WriteLine(builder.ToString())


            For Each row As DataRow In table.Rows
                sep = ""
                builder = New System.Text.StringBuilder

                For Each col As DataColumn In table.Columns
                    builder.Append(sep).Append(row(col.ColumnName))
                    sep = sepChar
                Next
                writer.WriteLine(builder.ToString())
            Next
        Finally
            If Not writer Is Nothing Then writer.Close()
        End Try
    End Sub
    Private Sub DeleteExcel()
        Try
            Dim TheFile As FileInfo = New FileInfo(ViewState("FileName"))
            If TheFile.Exists Then
                File.Delete(ViewState("FileName"))
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))

        End Try
    End Sub
    Private Function DoCSVUpload() As DataTable
        Dim dtImport As New DataTable
        Try
            Dim strConString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "\;Extended Properties=""text;HDR=Yes;FMT=Delimited"""
            Dim oledbConn As New OleDbConnection(strConString)

            Dim cmd As New OleDbCommand("SELECT * FROM [" & ViewState("CSVName") & "]", oledbConn)

            Dim oleda As New OleDbDataAdapter()

            oleda.SelectCommand = cmd



            oleda.Fill(dtImport)
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        End Try
        Return dtImport
    End Function

    Private Function DoXLSUpload() As DataTable
        Dim dtImport As New DataTable
        Try
            Dim connString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & ViewState("FileName") & " ;Extended Properties=""Excel 8.0;HDR=Yes;IMEX=1"""
            Dim oledbConn As New OleDbConnection(connString)
            Try
                oledbConn.Open()

                Dim cmd As New OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn)

                Dim oleda As New OleDbDataAdapter()

                oleda.SelectCommand = cmd



                oleda.Fill(dtImport)



            Catch ex As Exception
                log.Error(GetExceptionInfo(ex))
            Finally
                ' Close connection
                oledbConn.Close()
            End Try
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        End Try
        Return dtImport
    End Function
    Private Function DoXLSXUpload() As DataTable
        Dim dtImport As New DataTable
        Try
            Dim connString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & ViewState("FileName") & " ;Extended Properties=""Excel 12.0;HDR=Yes;IMEX=1"""
            Dim oledbConn As New OleDbConnection(connString)
            Try
                oledbConn.Open()

                Dim cmd As New OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn)

                Dim oleda As New OleDbDataAdapter()

                oleda.SelectCommand = cmd



                oleda.Fill(dtImport)



            Catch ex As Exception
                log.Error(GetExceptionInfo(ex))
            Finally
                ' Close connection
                oledbConn.Close()
            End Try
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        End Try
        Return dtImport
    End Function
    Private Function SetErrorsTable() As DataTable
        Dim col As DataColumn
        Dim dtErrors As New DataTable

        col = New DataColumn()
        col.ColumnName = "RowNo"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtErrors.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "LogInfo"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtErrors.Columns.Add(col)

        Return dtErrors
    End Function


#End Region


#Region "Other Events and functions"
    Private Sub DeleteExcelTemplate()
        Try
            Dim Filename As String = ViewState("SampleTemplate")

            Dim TheFile As FileInfo = New FileInfo(Filename)
            If TheFile.Exists Then
                File.Delete(Filename)
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))

        End Try
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs)
        MPEAdd.VisibleOnPageLoad = False
        Panel.Update()
        Return
    End Sub


    Private Sub btndownloadTemp_Click(sender As Object, e As EventArgs) Handles btndownloadTemp.Click
        Dim Filename As String = System.Configuration.ConfigurationManager.AppSettings("ExcelTemplatePath") & "ProductMinimumStock.xlsx"
        Dim TheFile As FileInfo = New FileInfo(Filename)
        If TheFile.Exists Then
            Dim strFileName As String = "Template" + Now.ToString("ddMMMyyHHmmss") + ".xlsx"


            ViewState("SampleTemplate") = strFileName

            Dim sFileName As String = strFileName
            Dim sFullPath As String = Filename
            Dim fileBytes As Byte() = System.IO.File.ReadAllBytes(sFullPath)

            Dim context As System.Web.HttpContext = System.Web.HttpContext.Current
            context.Response.ContentType = "application/vnd.ms-excel"
            context.Response.AddHeader("Content-Disposition", "attachment;filename=" + sFileName)
            context.Response.Clear()
            context.Response.BinaryWrite(fileBytes)
            context.Response.Flush()
            context.ApplicationInstance.CompleteRequest()
            DeleteExcelTemplate()
        End If
    End Sub

    Protected Sub lbLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbLog.Click
        Try


            If Not Session("SimpleLogInfo") Is Nothing Then
                Dim fileValue As String = Session("SimpleLogInfo")
                Dim file As System.IO.FileInfo = New FileInfo(fileValue)
                If file.Exists Then
                    Response.Clear()
                    Response.ContentType = "text/plain"
                    Dim filePath As String = fileValue
                    Response.ContentType = ContentType
                    Response.AddHeader("Content-Disposition", ("attachment; filename=" + Path.GetFileName(file.Name)))
                    Response.TransmitFile(filePath)
                    Response.Flush()
                    Response.End()

                Else
                    lblUpMsg.Text = "File does not exist"

                    MPEImport.VisibleOnPageLoad = True
                    Exit Sub

                End If

            Else
                lblUpMsg.Text = "There is no log to show."

                MPEImport.VisibleOnPageLoad = True
                Exit Sub

            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))

        Finally
        End Try
    End Sub

#End Region


End Class





