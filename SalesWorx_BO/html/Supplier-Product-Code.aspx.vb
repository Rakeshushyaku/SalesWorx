Imports SalesWorx.BO.Common
Imports log4net
Imports System.IO
Partial Public Class Supplier_Product_Code
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim dt As New DataTable
    Dim objSupplierProdcode As New SupplierProductCode
    Dim objLogin As New SalesWorx.BO.Common.Login
    ' Dim SortField As String = ""
    Private Const ModuleName As String = "Supplier-Product-Code.aspx"
    Private Const PageID As String = "P330"
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

       Dim objCommon As New Common

       objCommon = New Common()
       Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)

       ddl_Org.DataSource = objCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
       ddl_Org.DataTextField = "Description"
       ddl_Org.DataValueField = "MAS_Org_ID"
       ddl_Org.DataBind()

       ddl_orgFilter.DataSource = objCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
       ddl_orgFilter.DataTextField = "Description"
       ddl_orgFilter.DataValueField = "MAS_Org_ID"
       ddl_orgFilter.DataBind()


       ddl_Organization.DataSource = objCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
       ddl_Organization.DataTextField = "Description"
       ddl_Organization.DataValueField = "MAS_Org_ID"
       ddl_Organization.DataBind()


        dt = New DataTable()

        dt = objCommon.LoadAgency(Err_No, Err_Desc, ddl_orgFilter.SelectedItem.Value)



        Dim dr As DataRow
        dr = dt.NewRow()
        dr(0) = "-- Select a Supplier --"

        dt.Rows.InsertAt(dr, 0)
        ddl_FilterSupplier.DataSource = dt
        ddl_FilterSupplier.DataValueField = "Agency"
        ddl_FilterSupplier.DataTextField = "Agency"
        ddl_FilterSupplier.DataBind()


        ddl_Supplier.DataSource = dt
        ddl_Supplier.DataValueField = "Agency"
        ddl_Supplier.DataTextField = "Agency"
        ddl_Supplier.DataBind()


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
        ddl_Product.Enabled = True
        pnl_File.Visible = False
        txt_code.Enabled = True
        btnSave.Visible = True
        ddl_Supplier.Enabled = True
        Resetfields()
        ClassUpdatePnl.Update()
        adddiv.Visible = True
        uploadDiv.Visible = False
        Me.MPEDivConfig.Show()

    End Sub
    Protected Sub btnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTnImport.Click
        
        ClassUpdatePnl.Update()
        adddiv.Visible = False
        uploadDiv.Visible = True
        Me.MPEDivConfig.Show()
    End Sub
    Protected Sub btnMPCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Resetfields()
        MPEDivConfig.Hide()
        ClassUpdatePnl.Update()
    End Sub

    Private Sub btn_close_Click(sender As Object, e As ImageClickEventArgs) Handles btn_close.Click
        Resetfields()
        MPEDivConfig.Hide()
        ClassUpdatePnl.Update()
    End Sub
    Sub BindGrid(ByVal Criteria As String)
        Try
            log.Debug(Criteria)
            Dim dt As New DataTable
            dt = objSupplierProdcode.GetSupplierProdCode(Err_No, Err_Desc, Criteria)
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
        ddl_Supplier.ClearSelection()
        ddl_Product.ClearSelection()
        txt_code.Text = ""
    End Sub
     Sub CsvExport()
    Dim dt As New DataTable
    dt = CType(Session("Errordt"), DataTable)
        'Build the CSV file data as a Comma separated string.
                    Dim csv As String = String.Empty

                    For Each column As DataColumn In dt.Columns
                        'Add the Header row for CSV file.
                        csv += column.ColumnName + ","c
                    Next

                    'Add new line.
                    csv += vbCr & vbLf

                    For Each row As DataRow In dt.Rows
                        For Each column As DataColumn In dt.Columns
                            'Add the Data rows.
                            csv += row(column.ColumnName).ToString().Replace(",", ";") + ","c
                        Next

                        'Add new line.
                        csv += vbCr & vbLf
                    Next

                    'Download the CSV file.
                    Response.Clear()
                    Response.Buffer = True
                    Response.AddHeader("content-disposition", "attachment;filename=ErrorExport.csv")
                    Response.Charset = ""
                    Response.ContentType = "application/text"
                    Response.Output.Write(csv)
                    Response.Flush()
                    Response.End()
    End Sub


    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        adddiv.Visible = True
        uploadDiv.Visible = False
        If ddl_Org.SelectedValue = "0" Then
            ' lblmsgPopUp.Text = "Please select organization / division."
            lblmsgPopUp.Text = "Please select organisation."
            MPEDivConfig.Show()
            Return
        End If
        If btnSave.Text = "Save" Then
            If ddl_Supplier.SelectedValue = "-- Select a Supplier --" Then
                ' lblmsgPopUp.Text = "Please select organization / division."
                lblmsgPopUp.Text = "Please select Supplier."
                MPEDivConfig.Show()
                Return
            End If
            If ddl_Product.SelectedValue = "0" Then
                lblmsgPopUp.Text = "Please select organization / division."

                MPEDivConfig.Show()
                Return
            End If
            If String.IsNullOrEmpty(txt_code.Text) Then
                lblmsgPopUp.Text = "Please enter the product code."
                MPEDivConfig.Show()
                Return
            End If
            'If Me.rbStandard.Checked = False And Me.rbContinue.Checked = False Then
            '    MessageBoxValidation("Please select print format.")
            '    MPEDivConfig.Show()
            '    Return
            'End If
            Dim success As Boolean = False
            Try
                objSupplierProdcode.SupplierCode = ddl_Supplier.SelectedItem.Value
                objSupplierProdcode.Itemcode = ddl_Product.SelectedItem.Value
                objSupplierProdcode.ProductCode = txt_code.Text
                'If Me.rbContinue.Checked = True Then
                '    objDivConfig.PrintFormat = "Continuous"
                'End If
                'If Me.rbStandard.Checked = True Then
                '    objDivConfig.PrintFormat = "Standard"
                'End If

                If objSupplierProdcode.SaveSupplierProdCode(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID.ToString(), ddl_Org.SelectedItem.Value) = True Then
                    success = True
                    MessageBoxValidation("Successfully Saved.", "Information")
                End If

                If success = True Then
                    objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "SUPPLIER PRODUCT CODE", Me.ddl_Supplier.SelectedValue.ToString(), "Product: " & Me.ddl_Product.SelectedItem.Text & "/ Code :  " & Me.txt_code.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                    Resetfields()
                    BindGrid("1=1")
                    Me.MPEDivConfig.Hide()
                Else
                    log.Error(Err_Desc)
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_003") & "&next=Supplier-Product-Code.aspx&Title=Message", False)
                    Exit Try
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
    Protected Sub btnimportcode_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_importCode.Click
        adddiv.Visible = False
        uploadDiv.Visible = True
    If Not file_import.HasFile Then
                ' lblmsgPopUp.Text = "Please select organization / division."
            lblmsgPopUp.Text = "Please select a file ."
                BtnDownLoad.Visible = False
                MPEDivConfig.Show()
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
                lblmsgPopUp.Text = "File Size should be less than 10 MB, Please Split files and upload."
               
            Else
                If System.IO.File.Exists(spath) Then
                    System.IO.File.Delete(spath)
                End If
                file_import.SaveAs(spath)
                file_import.FileContent.Close()
                file_import.FileContent.Dispose()
                Dim success As Boolean
                Dim ErrorTbl As New DataTable
                success = objSupplierProdcode.ImportFile(Err_No, Err_Desc, spath, CType(Session("User_Access"), UserAccess).UserID.ToString(), ddl_Organization.SelectedItem.Value, ErrorTbl)
                If success = True Then
                    If ErrorTbl.Rows.Count = 0 Then
                        MessageBoxValidation("Successfully Imported.", "Information")
                        objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "SUPPLIER PRODUCT CODE File imported", Me.ddl_Supplier.SelectedValue.ToString(), "FileName: " & spath, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                        Resetfields()
                        BindGrid("1=1")
                        Me.MPEDivConfig.Hide()
                    Else
                        Session("Errordt") = ErrorTbl
                        BtnDownLoad.Visible = True
                        MPEDivConfig.Show()
                    End If
                Else
                    If ErrorTbl.Rows.Count = 0 Then
                        MessageBoxValidation("Error While Importing.", "Information")
                        Resetfields()
                        BindGrid("1=1")
                        Me.MPEDivConfig.Hide()
                    Else
                        Session("Errordt") = ErrorTbl
                        BtnDownLoad.Visible = True
                        MPEDivConfig.Show()
                    End If
                End If
            End If
            End If
    End Sub
    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            adddiv.Visible = True
            uploadDiv.Visible = False

            btnUpdate.Visible = True
            btnSave.Visible = False

            file_import.Enabled = False
            ddl_Product.Enabled = True
            txt_code.Enabled = True
            pnl_File.Visible = False
            Resetfields()
            'Resetfields()
            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)

            Dim SupplierID As String
            SupplierID = CType(row.FindControl("HSupplier"), HiddenField).Value
            If Not ddl_Supplier.FindItemByValue(SupplierID) Is Nothing Then
                ddl_Supplier.ClearSelection()
                ddl_Supplier.FindItemByValue(SupplierID).Selected = True
            End If

            Dim objCommon As New SalesWorx.BO.Common.Common
            Dim pdt As New DataTable
            pdt = objCommon.GetAllProductsByOrg_Agency(Err_No, Err_Desc, ddl_Org.SelectedItem.Value, ddl_Supplier.SelectedItem.Value)
            Dim dr1 As DataRow
            dr1 = pdt.NewRow()
            dr1(0) = "0"
            dr1(1) = "-- Select a Product --"
            pdt.Rows.InsertAt(dr1, 0)

            ddl_Product.DataValueField = "Item_Code"
            ddl_Product.DataTextField = "Description"

            ddl_Product.DataSource = pdt
            ddl_Product.DataBind()
           
            Dim Itemcode As String
            Itemcode = CType(row.FindControl("HItemCode"), HiddenField).Value
            If Not ddl_Product.FindItemByValue(Itemcode) Is Nothing Then
                ddl_Product.ClearSelection()
                ddl_Product.FindItemByValue(Itemcode).Selected = True
            End If

            txt_code.Text = row.Cells(3).Text


            MPEDivConfig.Show()
        Catch ex As Exception
            Err_No = "74208"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Supplier-Product-Code.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub
    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        adddiv.Visible = True
        uploadDiv.Visible = False
        If ddl_Supplier.SelectedValue = "-- Select a Supplier --" Then
            lblmsgPopUp.Text = "Please select organization / division."
            MPEDivConfig.Show()
            Return
        End If
        If ddl_Product.SelectedValue = "0" Then
            lblmsgPopUp.Text = "Please select Product."

            MPEDivConfig.Show()
            Return
        End If
        If String.IsNullOrEmpty(txt_code.Text) Then
            lblmsgPopUp.Text = "Please enter the product code"
            MPEDivConfig.Show()
            Return
        End If

        Dim success As Boolean = False
        Try
            objSupplierProdcode.SupplierCode = ddl_Supplier.SelectedItem.Value
            objSupplierProdcode.Itemcode = ddl_Product.SelectedItem.Value
            objSupplierProdcode.ProductCode = txt_code.Text


            If objSupplierProdcode.SaveSupplierProdCode(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID.ToString(), ddl_Org.SelectedItem.Value) = True Then
                success = True
                MessageBoxValidation("Successfully Updated.", "Information")
                
            End If

            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "U", "MAIN MENU", "SUPPLIER PRODUCT CODE", Me.ddl_Supplier.SelectedValue.ToString(), "Product: " & Me.ddl_Product.SelectedItem.Text & "/ Code :  " & Me.txt_code.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                Resetfields()
                BindGrid(ViewState("Criteria").ToString())
                Me.MPEDivConfig.Hide()
            Else
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_003") & "&next=Supplier-Product-Code.aspx&Title=Message", False)
                Exit Try
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
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
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
            Dim SupplierID As String
            SupplierID = CType(row.FindControl("HSupplier"), HiddenField).Value

            Dim ItemID As String
            ItemID = CType(row.FindControl("HItemCode"), HiddenField).Value

             Dim HOrg As String
            HOrg = CType(row.FindControl("HOrg"), HiddenField).Value

            If objSupplierProdcode.DeleteSupplierProdCode(Err_No, Err_Desc, SupplierID, ItemID, HOrg) = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "D", "MAIN MENU", "SUPPLIER PRODUCT CODE", SupplierID, "Product: " & ItemID, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                success = True
            End If
             
            If success = True Then
                 
                MessageBoxValidation("Successfully deleted.", "Information")
                ' dt = objDivConfig.GetDivisionalConfiguration(Err_No, Err_Desc, "1=1")
                BindGrid(ViewState("Criteria").ToString())
                Resetfields()

            Else
                MessageBoxValidation("Error occured while deleting Product Code.", "Information")
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_ReasonCode_005") & "&next=Supplier-Product-Code.aspx&Title=Message", False)
                Exit Try
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
            Dim HOrg As String

            For Each row In gvDivConfig.Rows
                Dim RowCheckBox As System.Web.UI.WebControls.CheckBox = row.FindControl("chkDelete")

                If RowCheckBox.Checked = True Then
                    Dim SupplierID As String
                    SupplierID = CType(row.FindControl("HSupplier"), HiddenField).Value


                    HOrg = CType(row.FindControl("HOrg"), HiddenField).Value

                    Dim ItemID As String
                    ItemID = CType(row.FindControl("HItemCode"), HiddenField).Value
                    idCollection = idCollection & SupplierID & "~" & ItemID & "|"
                    objLogin.SaveUserLog(Err_No, Err_Desc, "D", "MAIN MENU", "SUPPLIER PRODUCT CODE", SupplierID, "Product: " & ItemID, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")

                End If
            Next
            If idCollection.Trim() <> "" Then
                If objSupplierProdcode.DeleteSupplierProdCodes(Err_No, Err_Desc, idCollection, HOrg) = True Then
                    Success = True
                End If
                If (Success = True) Then
                    MessageBoxValidation("Product Code(s) deleted successfully.", "Information")
                    Resetfields()

                Else
                    MessageBoxValidation("Error occured while deleting Product Code(s).", "Information")
                    log.Error(Err_Desc)
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_ReasonCode_003") & "&next=ReasonCodes.aspx&Title=Message", False)
                    Exit Try
                End If

            End If
            BindGrid(ViewState("Criteria").ToString())
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
        Dim Criteria As String = " and A.Organization_ID='" & ddl_orgFilter.SelectedItem.Value & "'"
        If ddl_FilterSupplier.SelectedValue <> "-- Select a Supplier --" Then
            Criteria += " and A.Custom_Attribute_1='" & ddl_FilterSupplier.SelectedValue & "'"
        End If
        If ddl_FilterProduct.SelectedValue <> "0" And ddl_FilterProduct.SelectedValue <> "" Then
            Criteria += " and B.item_code='" & ddl_FilterProduct.SelectedValue & "'"
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

    

    Private Sub ddl_Org_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ddl_Org.SelectedIndexChanged
             dt = New DataTable()
            Dim objCommon As New SalesWorx.BO.Common.Common
                    dt = objCommon.LoadAgency(Err_No, Err_Desc, ddl_Org.SelectedItem.Value)
        Dim dr As DataRow
        dr = dt.NewRow()
        dr(0) = "-- Select a Supplier --"

        dt.Rows.InsertAt(dr, 0)
        

        ddl_Supplier.DataSource = dt
        ddl_Supplier.DataValueField = "Agency"
        ddl_Supplier.DataTextField = "Agency"
        ddl_Supplier.DataBind()

 Dim pdt As New DataTable
        pdt = objCommon.GetAllProductsByOrg_Agency(Err_No, Err_Desc, ddl_Org.SelectedItem.Value, ddl_Supplier.SelectedItem.Value)
        Dim dr1 As DataRow
        dr1 = pdt.NewRow()
        dr1(0) = "0"
        dr1(1) = "-- Select a Product --"
        pdt.Rows.InsertAt(dr1, 0)

        ddl_Product.DataSource = pdt
        ddl_Product.DataBind()

UpdatePanel4.Update()

    End Sub

    Private Sub ddl_orgFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ddl_orgFilter.SelectedIndexChanged
        dt = New DataTable()
Dim objCommon As New SalesWorx.BO.Common.Common
        dt = objCommon.LoadAgency(Err_No, Err_Desc, ddl_orgFilter.SelectedItem.Value)



        Dim dr As DataRow
        dr = dt.NewRow()
        dr(0) = "-- Select a Supplier --"

        dt.Rows.InsertAt(dr, 0)


        ddl_FilterSupplier.DataSource = dt
        ddl_FilterSupplier.DataValueField = "Agency"
        ddl_FilterSupplier.DataTextField = "Agency"
        ddl_FilterSupplier.DataBind()


        Dim pdt As New DataTable
        pdt = objCommon.GetAllProductsByOrg_Agency(Err_No, Err_Desc, ddl_orgFilter.SelectedItem.Value, ddl_FilterSupplier.SelectedItem.Value)
        Dim dr1 As DataRow
        dr1 = pdt.NewRow()
        dr1(0) = "0"
        dr1(1) = "-- Select a Product --"
        pdt.Rows.InsertAt(dr1, 0)

        ddl_FilterProduct.DataSource = pdt
        ddl_FilterProduct.DataBind()

        TopPanel.Update()
    End Sub

    Private Sub ddl_FilterSupplier_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ddl_FilterSupplier.SelectedIndexChanged
        Dim objCommon As New SalesWorx.BO.Common.Common
        Dim pdt As New DataTable
        pdt = objCommon.GetAllProductsByOrg_Agency(Err_No, Err_Desc, ddl_orgFilter.SelectedItem.Value, ddl_FilterSupplier.SelectedItem.Value)
        Dim dr1 As DataRow
        dr1 = pdt.NewRow()
        dr1(0) = "0"
        dr1(1) = "-- Select a Product --"
        pdt.Rows.InsertAt(dr1, 0)
        ddl_FilterProduct.DataValueField = "Item_Code"
        ddl_FilterProduct.DataTextField = "Description"
        ddl_FilterProduct.DataSource = pdt
        ddl_FilterProduct.DataBind()
        TopPanel.Update()
    End Sub

    Private Sub ddl_Supplier_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ddl_Supplier.SelectedIndexChanged
        Dim objCommon As New SalesWorx.BO.Common.Common
        Dim pdt As New DataTable
        pdt = objCommon.GetAllProductsByOrg_Agency(Err_No, Err_Desc, ddl_Org.SelectedItem.Value, ddl_Supplier.SelectedItem.Value)
        Dim dr1 As DataRow
        dr1 = pdt.NewRow()
        dr1(0) = "0"
        dr1(1) = "-- Select a Product --"
        pdt.Rows.InsertAt(dr1, 0)

        ddl_Product.DataValueField = "Item_Code"
        ddl_Product.DataTextField = "Description"

        ddl_Product.DataSource = pdt
        ddl_Product.DataBind()
UpdatePanel4.Update()
    End Sub

    Private Sub BtnDownLoad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnDownLoad.Click
         CsvExport()
    End Sub
End Class