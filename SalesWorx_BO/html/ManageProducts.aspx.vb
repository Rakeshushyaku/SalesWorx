Imports SalesWorx.BO.Common
Imports Telerik.Web.UI
Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports ExcelLibrary.SpreadSheet
Imports System.Threading
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports Telerik.Web
Imports System.Text.RegularExpressions
Imports Telerik

Public Class ManageProducts
    Inherits System.Web.UI.Page
    Dim objcommon As New SalesWorx.BO.Common.Common
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim objProduct As New SalesWorx.BO.Common.Product
    Dim objUOM As New UOM
    Dim objDivConfig As New DivConfig
    Private dtErrors As New DataTable
    Private dtErrors_u As New DataTable
    Private Const ModuleName As String = "ManageProducts.aspx"
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim HasLots As String
    Private Opt_serach As String
    Dim Allowpricechange As String
    Private Const PageID As String = "P455"
    Private dt As New DataTable
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objControl As New AppControl
    Private _strTempFolder As String = CStr(ConfigurationSettings.AppSettings("ExcelPath"))
    Private PhysicalPath As String = ""
    Private _strMediaFileSize As Long = CLng(ConfigurationSettings.AppSettings("MediaFileSize"))

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '' ****** REDIRECTING TO NEW PRODUCT PAGE ****** 

        Response.Redirect("ManageProductsNew.aspx")
        Exit Sub


        '' ***** ENDS HERE ****************



        If Not Page.IsPostBack Then
            If Not Session.Item("USER_ACCESS") Is Nothing Then

                If Not HasAuthentication() Then
                    Err_No = 500
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
                End If
                Opt_serach = 0

                FillUOM()
                FillOrganization()
                ResetFields()
                FillBrand()
                FillAgency()
                FillCategory()
                ' R FillUOM_Exp()
                FillDefaultUOM()
                FillRestrictiveReturn()
                BindProductGrid()
                btnUpdate.Visible = False

            Else
                Session.Add("BringmeBackHere", ModuleName)
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            If CType(Session.Item("CONTROL_PARAMS"), ControlParams).CLIENT_CODE = "JPH" Then
                If CType(Session.Item("USER_ACCESS"), UserAccess).IsSS = "A" Then
                    btnExport.Visible = True
                Else
                    btnExport.Visible = False
                End If
            End If
            ViewState("FileType") = Nothing
            ViewState("FileName") = Nothing
            ViewState("CSVName") = Nothing
            Session.Remove("dtPrErrors")
            Session.Remove("dtPrErrors_u")
            Session.Remove("PrLogFile")
            SetErrorsTable()
            SetErrorsTable_u()
            '' RefreshUI()
            '''''''''''''''rgProducts.Rebind()
            Session.Remove("Opt_serach")
            BindProductGrid()
            ' ExcelFileUpload.TemporaryFolder = _strTempFolder
        Else
            Me.DocWindow.VisibleOnPageLoad = False
            dtErrors = Session("dtPrErrors")
            dtErrors_u = Session("dtPrErrors_u")

        End If
        ExcelFileUpload.TemporaryFolder = System.Configuration.ConfigurationManager.AppSettings("TemporaryFolder")
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            If txtProdName.Text = "" Or Me.txtProdCode.Text = "" Or Me.ddlUOM.SelectedIndex <= 0 Or Me.ddlOrg.SelectedIndex <= 0 Or Me.ddlRestrictiveR.SelectedIndex <= 0 Or Me.ddlUOM.SelectedIndex <= 0 Then
                Me.lblVMsg.Text = "Please enter organization, product code ,name,UOM and Restrictive Return"
                Me.DocWindow.VisibleOnPageLoad = True
                Exit Sub
            End If




            If btnSave.Text = "Save" Then

                If objProduct.ProductCodeExists(Me.txtProdCode.Text, If(Me.lblProdID.Text = "", "0", Me.lblProdID.Text), ddlOrg.SelectedItem.Value) Then
                    Me.lblVMsg.Text = "Product code already exist"

                    Me.DocWindow.VisibleOnPageLoad = True
                    Exit Sub
                Else




                    If Me.rsHasLots.Checked Then
                        HasLots = "Y"
                    Else

                        HasLots = "N"
                    End If
                    If rsAllowpricechange.Checked Then
                        Allowpricechange = "Y"
                    Else

                        Allowpricechange = "N"
                    End If

                    Dim brand As String = ""
                    If ddlBrand.SelectedIndex >= 0 Then
                        brand = ddlBrand.SelectedItem.Value
                    End If

                    Dim Agency As String = ""
                    If ddlAgency.SelectedIndex >= 0 Then
                        Agency = ddlAgency.SelectedItem.Value
                    End If

                    Dim Category As String = ""
                    If ddlCategory.SelectedIndex >= 0 Then
                        Category = ddlCategory.SelectedItem.Value
                    End If



                    If objProduct.SaveProduct(Err_No, Err_Desc, Me.txtProdCode.Text, Me.txtProdName.Text, brand,
                           Agency, Me.ddlUOM.SelectedValue, Me.txtBarcode.Text,
                           CDec(IIf(Me.txtCost.Text = "", "0", Me.txtCost.Text)), Category,
                           Me.txtProdCode.Text, Me.txtItemSize.Text, Me.txtSubCategory.Text, ddldefaultUOM.SelectedValue, ddlRestrictiveR.SelectedValue, HasLots, Allowpricechange,
                            If(Me.lblProdID.Text = "", "0", Me.lblProdID.Text), Me.ddlOrg.SelectedValue, "") Then


                        MessageBoxValidation("Successfully saved", "Information")
                        ResetFields()
                        BindProductGrid() ''  Me.rgProducts.Rebind()
                        ClassUpdatePnl.Update()
                    Else
                        Me.lblVMsg.Text = "Error occured while saving."

                        Me.DocWindow.VisibleOnPageLoad = True
                        Exit Sub
                    End If

                    Exit Sub
                End If


            ElseIf Me.btnSave.Text = "Update" Then
                If Me.rsHasLots.Checked Then
                    HasLots = "Y"
                Else

                    HasLots = "N"
                End If
                If rsAllowpricechange.Checked Then
                    Allowpricechange = "Y"
                Else

                    Allowpricechange = "N"
                End If
                If objProduct.ProductCodeExists(Me.txtProdCode.Text, If(Me.lblProdID.Text = "", "0", Me.lblProdID.Text), ddlOrg.SelectedItem.Value) Then
                    Me.lblVMsg.Text = "Product code already exist"

                    Me.DocWindow.VisibleOnPageLoad = True

                Else

                    Dim brand As String = ""
                    If ddlBrand.SelectedIndex >= 0 Then
                        brand = ddlBrand.SelectedItem.Value
                    End If

                    Dim Agency As String = ""
                    If ddlAgency.SelectedIndex >= 0 Then
                        Agency = ddlAgency.SelectedItem.Value
                    End If

                    Dim Category As String = ""
                    If ddlCategory.SelectedIndex >= 0 Then
                        Category = ddlCategory.SelectedItem.Value
                    End If


                    If objProduct.UpdateProduct(Err_No, Err_Desc, Me.txtProdCode.Text, Me.txtProdName.Text, brand,
                               Agency, Me.ddlUOM.SelectedValue, Me.txtBarcode.Text,
                               CDec(IIf(Me.txtCost.Text = "", "0", Me.txtCost.Text)), Category,
                               Me.txtProdCode.Text, Me.txtItemSize.Text, Me.txtSubCategory.Text, ddldefaultUOM.SelectedValue, ddlRestrictiveR.SelectedValue, HasLots, Allowpricechange, Me.lblProdID.Text, Me.lblOrgID.Text, "") Then


                        MessageBoxValidation("Successfully updated", "Information")
                        ' ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "HideWindow();", True)
                        ResetFields()
                        BindProductGrid() '''''''Me.rgProducts.Rebind()
                        ClassUpdatePnl.Update()
                    Else
                        Me.lblVMsg.Text = "Error occured while updating."

                        Me.DocWindow.VisibleOnPageLoad = True
                        Exit Sub
                    End If

                End If
            End If




        Catch ex As Exception
            MessageBoxValidation("Error occured while saving.", "Validation")
            log.Error(ex.Message)
        End Try

    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try





            Dim btn1 As ImageButton = TryCast(sender, ImageButton)
            Dim item As GridDataItem = DirectCast(btn1.NamingContainer, GridDataItem)
            Dim ProdID As String = item.GetDataKeyValue("Product_ID").ToString()
            Dim OrgID As String = item.GetDataKeyValue("Sales_Org_ID").ToString()

            ResetFields()
            Me.lblProdID.Text = ProdID


            Me.btnSave.Text = "Update"
            Me.btnUpdate.Visible = True
            Me.btnSave.Visible = False

            Me.DocWindow.Title = "Modify Product"
            Me.DocWindow.VisibleOnPageLoad = True


            lblOrgID.Text = OrgID

            Dim x As DataTable = Nothing
            x = objProduct.GetProductsbyID(Err_No, Err_Desc, Me.lblProdID.Text, lblOrgID.Text)

            If x.Rows.Count > 0 Then

                Me.txtProdCode.Text = IIf(x.Rows(0)("Item_Code") Is DBNull.Value, "", x.Rows(0)("Item_Code").ToString())
                Me.txtProdName.Text = IIf(x.Rows(0)("Description") Is DBNull.Value, "", x.Rows(0)("Description").ToString())
                Me.ddlBrand.SelectedValue = IIf(x.Rows(0)("Brand_Code") Is DBNull.Value, "", x.Rows(0)("Brand_Code").ToString())
                Me.ddlUOM.SelectedValue = IIf(x.Rows(0)("Primary_UOM_Code") Is DBNull.Value, "", x.Rows(0)("Primary_UOM_Code").ToString())
                ddlUOM.Enabled = False
                Me.ddlAgency.SelectedValue = IIf(x.Rows(0)("Agency") Is DBNull.Value, "", x.Rows(0)("Agency").ToString())
                Me.txtBarcode.Text = IIf(x.Rows(0)("EANNO") Is DBNull.Value, "", x.Rows(0)("EANNO").ToString())
                ' ''Me.txtMetaData.Text = IIf(x.Rows(0)("Item_MetaData") Is DBNull.Value, "", x.Rows(0)("Item_MetaData").ToString())
                ' Me.txtNetPrice.Text = IIf(x.Rows(0)("Net_Price") Is DBNull.Value, "", x.Rows(0)("Net_Price").ToString())
                ' Me.txtListprice.Text = IIf(x.Rows(0)("List_Price") Is DBNull.Value, "", x.Rows(0)("List_Price").ToString())
                Me.txtCost.Text = IIf(x.Rows(0)("Cost_Price") Is DBNull.Value, "", x.Rows(0)("Cost_Price").ToString())
                ' Me.txtDiscount.Text = IIf(x.Rows(0)("Discount") Is DBNull.Value, "", x.Rows(0)("Discount").ToString())

                Me.ddlCategory.SelectedValue = IIf(x.Rows(0)("Category") Is DBNull.Value, "", x.Rows(0)("Category").ToString())

                Me.ddlOrg.SelectedValue = IIf(x.Rows(0)("Organization_ID") Is DBNull.Value, "", x.Rows(0)("Organization_ID").ToString())

                ''Me.txtDetailInfo.Text = IIf(x.Rows(0)("DetailInfo") Is DBNull.Value, "", x.Rows(0)("DetailInfo").ToString())
                ''If x.Rows(0)("Is_Active").ToString() = "Y" Then
                ''    Me.rsActive.Checked = True
                ''Else
                ''    Me.rsActive.Checked = False
                ''End If

                Me.ddlOrg.Enabled = False

                Me.txtItemSize.Text = IIf(x.Rows(0)("Item_Size") Is DBNull.Value, "", x.Rows(0)("Item_Size").ToString())
                Me.txtSubCategory.Text = IIf(x.Rows(0)("Sub_Category") Is DBNull.Value, "", x.Rows(0)("Sub_Category").ToString())





                Dim query_lc = From addinfo In x.AsEnumerable() Where addinfo.Field(Of String)("Attrib_Name") = "LC" Select addinfo
                If query_lc.Count() > 0 Then


                    Dim Lc_dt As DataTable = query_lc.CopyToDataTable()

                    If Lc_dt.Rows(0)("Attrib_Value").ToString().ToUpper().Trim() = "Y" Then
                        rsHasLots.Checked = True
                    Else
                        rsHasLots.Checked = False

                    End If
                End If
                Dim query_prc = From addinfo In x.AsEnumerable() Where addinfo.Field(Of String)("Attrib_Name") = "PRC" Select addinfo
                If query_prc.Count() > 0 Then


                    Dim PRC_dt As DataTable = query_prc.CopyToDataTable()

                    If PRC_dt.Rows(0)("Attrib_Value").ToString().ToUpper().Trim() = "Y" Then
                        rsAllowpricechange.Checked = True
                    Else
                        rsAllowpricechange.Checked = False

                    End If
                End If


                Me.ddlCategory.SelectedValue = IIf(x.Rows(0)("Category") Is DBNull.Value, "", x.Rows(0)("Category").ToString())

                Dim query_du = From addinfo In x.AsEnumerable() Where addinfo.Field(Of String)("Attrib_Name") = "DU" Select addinfo
                If query_du.Count() > 0 Then


                    Dim DU_dt As DataTable = query_du.CopyToDataTable()
                    Me.ddldefaultUOM.SelectedValue = DU_dt.Rows(0)("Attrib_Value").ToString()

                End If
                Dim query_RET_MODE = From addinfo In x.AsEnumerable() Where addinfo.Field(Of String)("Attrib_Name") = "RET_MODE" Select addinfo
                If query_RET_MODE.Count() > 0 Then


                    Dim RET_MODE_dt As DataTable = query_RET_MODE.CopyToDataTable()
                    Me.ddlRestrictiveR.SelectedValue = RET_MODE_dt.Rows(0)("Attrib_Value").ToString()

                End If

                Me.txtProdCode.Enabled = False



            End If


            DocWindow.VisibleOnPageLoad = True
            ClassUpdatePnl.Update()
        Catch ex As Exception
            Err_No = "22421"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("PHX_006") & "&next=ManageProducts.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub
    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click


        If txtProdName.Text = "" Or Me.txtProdCode.Text = "" Or Me.ddlUOM.SelectedIndex <= 0 Or Me.ddlOrg.SelectedIndex <= 0 Or Me.ddlRestrictiveR.SelectedIndex <= 0 Or Me.ddldefaultUOM.SelectedIndex <= 0 Then
            Me.lblVMsg.Text = "Please enter organization, product code ,name  and UOM"
            Me.DocWindow.VisibleOnPageLoad = True
            Exit Sub
        End If

        If Me.rsHasLots.Checked Then
            HasLots = "Y"
        Else

            HasLots = "N"
        End If
        If rsAllowpricechange.Checked Then
            Allowpricechange = "Y"
        Else

            Allowpricechange = "N"
        End If
        log.Debug("Me.txtProdCode.Text" & Me.txtProdCode.Text)
        log.Debug("lblProdID.Text" & lblProdID.Text)
        log.Debug(" ddlOrg.SelectedItem.Value" & ddlOrg.SelectedItem.Value)

        If objProduct.ProductCodeExists(Me.txtProdCode.Text, If(Me.lblProdID.Text = "", "0", Me.lblProdID.Text), ddlOrg.SelectedItem.Value) Then
            Me.lblVMsg.Text = "Product code already exist"

            Me.DocWindow.VisibleOnPageLoad = True

        Else

            Dim brand As String = ""
            If ddlBrand.SelectedIndex >= 0 Then
                brand = ddlBrand.SelectedItem.Value
            End If

            Dim Agency As String = ""
            If ddlAgency.SelectedIndex >= 0 Then
                Agency = ddlAgency.SelectedItem.Value
            End If

            Dim Category As String = ""
            If ddlCategory.SelectedIndex >= 0 Then
                Category = ddlCategory.SelectedItem.Value
            End If


            If objProduct.UpdateProduct(Err_No, Err_Desc, Me.txtProdCode.Text, Me.txtProdName.Text, brand,
                       Agency, Me.ddlUOM.SelectedValue, Me.txtBarcode.Text,
                       CDec(IIf(Me.txtCost.Text = "", "0", Me.txtCost.Text)), Category,
                       Me.txtProdCode.Text, Me.txtItemSize.Text, Me.txtSubCategory.Text, ddldefaultUOM.SelectedValue, ddlRestrictiveR.SelectedValue, HasLots, Allowpricechange, Me.lblProdID.Text, Me.lblOrgID.Text, "") Then


                MessageBoxValidation("Successfully updated", "Information")
                ' ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "HideWindow();", True)
                ResetFields()
                BindProductGrid() '''''''Me.rgProducts.Rebind()
                ClassUpdatePnl.Update()
            Else
                Me.lblVMsg.Text = "Error occured while updating."

                Me.DocWindow.VisibleOnPageLoad = True
                Exit Sub
            End If

        End If
    End Sub


    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click


        ResetFields()
        Me.DocWindow.VisibleOnPageLoad = False
    End Sub

    Protected Sub rgProduct_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles rgProducts1.ItemCommand
        If e.CommandName = "DeleteSelected" Then
            Try






                Dim item As GridDataItem = DirectCast(e.Item, GridDataItem)
                Dim ProductID As Label = DirectCast(item.FindControl("lblProductID"), Label)

                'Dim item As GridDataItem = DirectCast(btn1.NamingContainer, GridDataItem)
                Dim ProdID As String = item.GetDataKeyValue("Product_ID").ToString()
                Dim OrgID As String = item.GetDataKeyValue("Sales_Org_ID").ToString()

                If objProduct.DeleteProduct(Err_No, Err_Desc, CInt(IIf(ProductID.Text = "", "0", ProductID.Text)), OrgID) Then
                    MessageBoxValidation("Successfully deleted", "Information")
                    BindProductGrid() '' rgProducts.Rebind()
                    ClassUpdatePnl.Update()
                Else
                    MessageBoxValidation("Error while deleting", "Validation")
                    Exit Sub
                End If

            Catch ex As Exception
                Err_No = "63924"
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("PHX_006") & "&next=ManageProducts.aspx&Title=Message", False)
                If Err_Desc Is Nothing Then
                    log.Error(GetExceptionInfo(ex))
                Else
                    log.Error(Err_Desc)
                End If
            End Try
        End If

        If e.CommandName = "ActivateSelected" Then
            Try


                Dim item As GridDataItem = DirectCast(e.Item, GridDataItem)
                Dim ProductID As Label = DirectCast(item.FindControl("lblProductID"), Label)

                ' Dim item As GridDataItem = DirectCast(btn1.NamingContainer, GridDataItem)
                Dim ProdID As String = item.GetDataKeyValue("Product_ID").ToString()
                Dim OrgID As String = item.GetDataKeyValue("Sales_Org_ID").ToString()

                If objProduct.ActivateProduct(Err_No, Err_Desc, CInt(IIf(ProductID.Text = "", "0", ProductID.Text)), OrgID) Then
                    MessageBoxValidation("Successfully Activated", "Information")
                    BindProductGrid() ''  rgProducts.Rebind()
                    ClassUpdatePnl.Update()
                Else
                    MessageBoxValidation("Error while Activation", "Validation")
                    Exit Sub
                End If

            Catch ex As Exception
                Err_No = "63924"
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("PHX_006") & "&next=ManageProducts.aspx&Title=Message", False)
                If Err_Desc Is Nothing Then
                    log.Error(GetExceptionInfo(ex))
                Else
                    log.Error(Err_Desc)
                End If
            End Try
        End If



        If e.CommandName = "UOMSelected" Then
            Try
                Dim item As GridDataItem = DirectCast(e.Item, GridDataItem)
                Dim ProdID As String = item.GetDataKeyValue("Product_ID").ToString()
                Dim OrgID As String = item.GetDataKeyValue("Sales_Org_ID").ToString()
                Dim Primary_UOM As String
                Dim Item_Code As String

                Dim x As DataTable = Nothing
                x = objProduct.GetProductsbyID(Err_No, Err_Desc, ProdID, OrgID)
                If x.Rows.Count > 0 Then
                    Primary_UOM = IIf(x.Rows(0)("Primary_UOM_Code") Is DBNull.Value, "", x.Rows(0)("Primary_UOM_Code").ToString())
                    Item_Code = IIf(x.Rows(0)("Item_Code") Is DBNull.Value, "", x.Rows(0)("Item_Code").ToString())
                End If

                Response.Redirect("ManageUOM.aspx?Item_Code=" & Item_Code & "&OrgID=" & OrgID & "&Primary_UOM=" & Primary_UOM)
            Catch ex As Exception

            End Try
        End If
    End Sub

    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click

        Dim finalDataSet As New DataSet
        Dim dtProducts As New DataTable()


        ' dtProducts = objProduct.LoadExportProductsTemplate()

        'finalDataSet.Tables.Add(dtProducts)

        finalDataSet = objProduct.LoadExportProductsTemplate(ddlorg_F.SelectedValue, "", "", "", "")
        If finalDataSet.Tables.Count() > 1 Then
            finalDataSet.Tables(0).TableName = "Product"
            finalDataSet.Tables(1).TableName = "Product_UOM"
        End If



        Dim fn As String = "Product_Info" + DateTime.Now.ToString("hhmmss") + ".xls"
        Dim d As New DataSet


        If finalDataSet.Tables.Count <= 0 Then
            MessageBoxValidation("There is no data to export", "Information")
            Exit Sub
        Else
            ExportToExcel(fn, finalDataSet)
        End If



    End Sub

    Public Function WriteXLSFile(ByVal pFileName As String, ByVal pDataSet As DataSet) As Boolean
        Try
            'This function CreateWorkbook will cause xls file cannot be opened
            'normally when file size below 7 KB, see my work around below
            'ExcelLibrary.DataSetHelper.CreateWorkbook(pFileName, pDataSet)

            'Create a workbook instance
            Dim workbook As Workbook = New Workbook()
            Dim worksheet As Worksheet
            Dim iRow As Integer = 0
            Dim iCol As Integer = 0
            Dim sTemp As String = String.Empty
            Dim dTemp As Double = 0
            Dim iTemp As Integer = 0
            '  Dim dtTime As DateTime
            Dim count As Integer = 0
            Dim iTotalRows As Integer = 0
            Dim iSheetCount As Integer = 0

            'Read DataSet
            If Not pDataSet Is Nothing And pDataSet.Tables.Count > 0 Then

                'Traverse DataTable inside the DataSet
                For Each dt As DataTable In pDataSet.Tables


                    'Create a worksheet instance
                    iSheetCount = iSheetCount + 1

                    'worksheet = New Worksheet("Product")

                    worksheet = New Worksheet(dt.TableName)
                    iCol = 0
                    iRow = 0
                    'Write Table Header
                    For Each dc As DataColumn In dt.Columns
                        worksheet.Cells(iRow, iCol) = New Cell(dc.ColumnName)
                        iCol = iCol + 1
                    Next

                    'Write Table Body
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

                    'Attach worksheet to workbook
                    workbook.Worksheets.Add(worksheet)
                    iTotalRows = iTotalRows + iRow
                Next
            End If

            'Bug on Excel Library, min file size must be 7 Kb
            'thus we need to add empty row for safety
            If iTotalRows < 100 Then
                worksheet = New Worksheet("Sheet2")
                count = 1
                Do While count < 100
                    worksheet.Cells(count, 0) = New Cell(" ")
                    count = count + 1
                Loop
                workbook.Worksheets.Add(worksheet)
            End If

            workbook.Save(pFileName)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
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
    Private Sub ResetFields()
        Me.txtProdCode.Text = ""
        Me.txtProdName.Text = ""
        Me.txtBarcode.Text = ""

        Me.txtCost.Text = ""

        Me.txtCost.Text = ""

        Me.lblProdID.Text = ""
        Me.lblMsg.Text = ""
        Me.lblVMsg.Text = ""

        Me.btnSave.Text = "Save"

        Me.ddlOrg.Enabled = True
        Me.DocWindow.Title = "Add Product"
        Me.DocWindow.VisibleOnPageLoad = False
        ddlUOM.Enabled = True
        Me.txtItemSize.Text = ""
        Me.txtSubCategory.Text = ""
        rsAllowpricechange.Checked = False
        rsHasLots.Checked = False
        lbl_msg_f.Text = ""

        Me.ddlOrg.ClearSelection()
        Me.ddlOrg.Text = ""
        Me.ddlOrg.Focus()

        If ddlOrg.Items.Count = 2 Then
            ddlOrg.SelectedIndex = 1
        End If
        Me.ddlUOM.ClearSelection()
        Me.ddlUOM.Text = ""
        Me.ddlUOM.Focus()

        Me.ddlAgency.ClearSelection()
        Me.ddlAgency.Text = ""
        Me.ddlAgency.Focus()

        Me.ddlBrand.ClearSelection()
        Me.ddlBrand.Text = ""
        Me.ddlBrand.Focus()

        Me.ddlCategory.ClearSelection()
        Me.ddlCategory.Text = ""
        Me.ddlCategory.Focus()


        Me.ddldefaultUOM.ClearSelection()
        Me.ddldefaultUOM.Text = ""
        Me.ddldefaultUOM.Focus()


        Me.ddlRestrictiveR.ClearSelection()
        Me.ddlRestrictiveR.Text = ""
        Me.ddlRestrictiveR.Focus()



        Me.btnUpdate.Visible = False
        Me.btnSave.Visible = True



    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Private Function HasAuthentication() As Boolean
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
    End Function
    Private Function SetErrorsTable() As DataTable
        Dim col As DataColumn
        'Dim dtErrors As New DataTable

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

        Session.Remove("dtPrErrors")
        Session("dtPrErrors") = dtErrors
        Return dtErrors
    End Function

    Private Function SetErrorsTable_u() As DataTable
        Dim col As DataColumn

        col = New DataColumn()
        col.ColumnName = "RowNo"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtErrors_u.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "LogInfo"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtErrors_u.Columns.Add(col)

        Session.Remove("dtPrErrors_u")
        Session("dtPrErrors_u") = dtErrors_u
        Return dtErrors_u
    End Function

    Protected Sub ExcelFileUpload_FileUploaded(ByVal sender As Object, ByVal e As FileUploadedEventArgs)
        Try
            If Not e.File.FileName Is Nothing Then
                Dim fileName As String = e.File.FileName
                Dim exten As String = System.IO.Path.GetExtension(fileName)
                Dim Str As New StringBuilder
                Dim TotSuccess As Integer = 0
                Dim TotFailed As Integer = 0
                Dim TotSuccess_u As Integer = 0
                Dim TotFailed_u As Integer = 0
                Try
                    ViewState("FileType") = e.File.ContentType

                    If Not (exten.ToLower().Trim() = ".xls" Or exten.ToLower().Trim() = ".xlsx") Then
                        MessageBoxValidation("Please upload excel file", "Validation")
                        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)

                        Exit Sub

                    End If

                    If e.File.FileName.ToString.ToLower().Trim().EndsWith(".xls") Or e.File.FileName.ToString.ToLower().Trim().EndsWith(".xlsx") Then

                        Dim Foldername As String = System.Configuration.ConfigurationManager.AppSettings("TemporaryFolder")
                        'Dim Foldername As String = ExcelFileUpload.TemporaryFolder
                        If Not Foldername.EndsWith("\") Then
                            Foldername = Foldername & "\"
                        End If
                        If Directory.Exists(Foldername.Substring(0, Len(Foldername) - 1)) = False Then
                            Directory.CreateDirectory(Foldername.Substring(0, Len(Foldername) - 1))
                        End If
                        If e.File.FileName.ToString.EndsWith(".csv") Then
                            Dim FName As String
                            FName = Now.Hour & Now.Minute & Now.Second & e.File.FileName
                            ViewState("FileName") = Foldername & FName
                            ViewState("CSVName") = FName
                        Else
                            ViewState("FileName") = Foldername & Now.Hour & Now.Minute & Now.Second & e.File.FileName
                        End If

                        e.File.SaveAs(ViewState("FileName"))


                        Try

                            If ViewState("FileType") IsNot Nothing And ViewState("FileName") IsNot Nothing Then
                                Dim TempTbl As New DataTable
                                If TempTbl.Rows.Count > 0 Then
                                    TempTbl.Rows.Clear()
                                End If
                                Dim col As DataColumn


                                col = New DataColumn
                                col.ColumnName = "Item_Code"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)


                                col = New DataColumn
                                col.ColumnName = "Sales_Org_ID"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)

                                col = New DataColumn
                                col.ColumnName = "Description"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)


                                col = New DataColumn
                                col.ColumnName = "Brand_Code"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)


                                col = New DataColumn
                                col.ColumnName = "Item_Size"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)




                                col = New DataColumn
                                col.ColumnName = "Barcode"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)


                                col = New DataColumn
                                col.ColumnName = "Base_UOM"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)


                                col = New DataColumn
                                col.ColumnName = "Agency"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)


                                col = New DataColumn
                                col.ColumnName = "Category"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)


                                col = New DataColumn
                                col.ColumnName = "Sub_Category"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)

                                col = New DataColumn
                                col.ColumnName = "Cost_Price"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)



                                col = New DataColumn
                                col.ColumnName = "Default_UOM"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)

                                col = New DataColumn
                                col.ColumnName = "Restrictive_Return"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)

                                col = New DataColumn
                                col.ColumnName = "Has_Lots"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)

                                col = New DataColumn
                                col.ColumnName = "Allow_Price_Change"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)



                                Dim TempTbl_UOM As New DataTable
                                If TempTbl_UOM.Rows.Count > 0 Then
                                    TempTbl_UOM.Rows.Clear()
                                End If
                                Dim col_UOM As DataColumn


                                col_UOM = New DataColumn
                                col_UOM.ColumnName = "Item_Code"
                                col_UOM.DataType = System.Type.GetType("System.String")
                                col_UOM.ReadOnly = False
                                col_UOM.Unique = False
                                TempTbl_UOM.Columns.Add(col_UOM)


                                col_UOM = New DataColumn
                                col_UOM.ColumnName = "UOM"
                                col_UOM.DataType = System.Type.GetType("System.String")
                                col_UOM.ReadOnly = False
                                col_UOM.Unique = False
                                TempTbl_UOM.Columns.Add(col_UOM)

                                col_UOM = New DataColumn
                                col_UOM.ColumnName = "Conversion_Factor"
                                col_UOM.DataType = System.Type.GetType("System.String")
                                col_UOM.ReadOnly = False
                                col_UOM.Unique = False
                                TempTbl_UOM.Columns.Add(col_UOM)


                                col_UOM = New DataColumn
                                col_UOM.ColumnName = "Is_Sellable"
                                col_UOM.DataType = System.Type.GetType("System.String")
                                col_UOM.ReadOnly = False
                                col_UOM.Unique = False
                                TempTbl_UOM.Columns.Add(col_UOM)



                                If ViewState("FileName").ToString.ToLower().Trim().EndsWith(".csv") Then
                                    TempTbl = DoCSVUpload()
                                ElseIf ViewState("FileName").ToString.ToLower().Trim().EndsWith(".xls") Then
                                    TempTbl = DoXLSUpload()
                                ElseIf ViewState("FileName").ToString.ToLower().Trim().EndsWith(".xlsx") Then
                                    TempTbl = DoXLSXUpload()
                                End If



                                If ViewState("FileName").ToString.ToLower().Trim().EndsWith(".csv") Then
                                    TempTbl_UOM = DoCSVUpload_UOM()
                                ElseIf ViewState("FileName").ToString.ToLower().Trim().EndsWith(".xls") Then
                                    TempTbl_UOM = DoXLSUpload_UOM()
                                ElseIf ViewState("FileName").ToString.ToLower().Trim().EndsWith(".xlsx") Then
                                    TempTbl_UOM = DoXLSXUpload_UOM()
                                End If



                                If TempTbl.Rows.Count = 0 Or TempTbl Is Nothing Then
                                    MessageBoxValidation("Invalid file Template", "Validation")
                                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                                    Exit Sub
                                End If

                                '   If TempTbl.Columns.Count = 13 Then
                                If Not (TempTbl.Columns(0).ColumnName = "Item_Code" Or TempTbl.Columns(1).ColumnName = "Sales_Org_ID" Or TempTbl.Columns(2).ColumnName = "Description" Or TempTbl.Columns(3).ColumnName = "Brand_Code" Or TempTbl.Columns(4).ColumnName = "Item_Size" Or TempTbl.Columns(5).ColumnName = "Barcode" Or TempTbl.Columns(6).ColumnName = "Base_UOM" Or TempTbl.Columns(7).ColumnName = "Agency" Or TempTbl.Columns(8).ColumnName = "Category" Or TempTbl.Columns(9).ColumnName = "Sub_Category" Or TempTbl.Columns(10).ColumnName = "Cost_Price" Or TempTbl.Columns(11).ColumnName = "Default_UOM" Or TempTbl.Columns(12).ColumnName = "Restrictive_Return" Or TempTbl.Columns(13).ColumnName = "Has_Lots" Or TempTbl.Columns(14).ColumnName = "Allow_Price_Change") Then



                                    MessageBoxValidation("Please check the template columns are correct", "Validation")
                                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                                    Exit Sub

                                End If

                                'Else
                                '    MessageBoxValidation("The products template sheet should be 13 column only", "Validation")
                                '    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                                '    Exit Sub
                                'End If




                                If TempTbl.Rows.Count = 0 Then
                                    MessageBoxValidation("There is no data in the file.", "Validation")
                                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)

                                    Exit Sub
                                End If
                                dtErrors = Session("dtPrErrors")

                                If dtErrors.Rows.Count > 0 Then
                                    dtErrors.Rows.Clear()
                                End If
                                Dim RowNo As String = Nothing
                                Dim ErrorText As String = Nothing

                                Dim RowNo_u As String = Nothing
                                Dim ErrorText_u As String = Nothing
                                If TempTbl.Rows.Count > 0 Then
                                    Dim idx As Integer

                                    Dim ProdNo As String = Nothing
                                    Dim ProdCode As String = Nothing
                                    Dim Organization As String = Nothing
                                    Dim ProdName As String = Nothing
                                    Dim Brand As String = Nothing
                                    Dim UOM As String = Nothing
                                    Dim Agency As String = Nothing
                                    Dim Category As String = Nothing

                                    Dim CostPrice As String = Nothing

                                    Dim Base_UOM As String = Nothing
                                    Dim Barcode As String = Nothing
                                    Dim ItemSize As String = Nothing
                                    Dim SubCategory As String = Nothing
                                    Dim xDefaultUOM As String = Nothing
                                    Dim xRestrictR As String = Nothing
                                    Dim xHasLots As String = Nothing
                                    Dim xAllowpricechange As String = Nothing

                                    Dim isValidRow As Boolean = True
                                    For idx = 0 To TempTbl.Rows.Count - 1

                                        Dim IfExisits As String = "0"
                                        RowNo = Nothing
                                        ErrorText = Nothing
                                        isValidRow = True

                                        If TempTbl.Rows(idx)(0) Is DBNull.Value Or TempTbl.Rows(idx)(1) Is DBNull.Value Or TempTbl.Rows(idx)(2) Is DBNull.Value Then

                                            RowNo = idx + 2
                                            ErrorText = "Product code,Name and Organization are mandatory" + ","
                                            Dim h As DataRow = dtErrors.NewRow()
                                            h("RowNo") = RowNo

                                            h("LogInfo") = ErrorText
                                            dtErrors.Rows.Add(h)


                                            Continue For
                                        End If

                                        ProdCode = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, Nothing, TempTbl.Rows(idx)(0).ToString())
                                        Organization = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, Nothing, TempTbl.Rows(idx)(1).ToString())
                                        ProdName = IIf(TempTbl.Rows(idx)(2) Is DBNull.Value, Nothing, TempTbl.Rows(idx)(2).ToString())
                                        Brand = IIf(TempTbl.Rows(idx)(3) Is DBNull.Value, "", TempTbl.Rows(idx)(3).ToString())
                                        ItemSize = IIf(TempTbl.Rows(idx)(4) Is DBNull.Value, Nothing, TempTbl.Rows(idx)(4).ToString())
                                        Barcode = IIf(TempTbl.Rows(idx)(5) Is DBNull.Value, Nothing, TempTbl.Rows(idx)(5).ToString())
                                        Base_UOM = IIf(TempTbl.Rows(idx)(6) Is DBNull.Value, Nothing, TempTbl.Rows(idx)(6).ToString())
                                        Agency = IIf(TempTbl.Rows(idx)(7) Is DBNull.Value, "", TempTbl.Rows(idx)(7).ToString())
                                        Category = IIf(TempTbl.Rows(idx)(8) Is DBNull.Value, "", TempTbl.Rows(idx)(8).ToString())
                                        SubCategory = IIf(TempTbl.Rows(idx)(9) Is DBNull.Value, "", TempTbl.Rows(idx)(9).ToString())
                                        CostPrice = IIf(TempTbl.Rows(idx)(10) Is DBNull.Value, "0", TempTbl.Rows(idx)(10).ToString())
                                        xDefaultUOM = IIf(TempTbl.Rows(idx)(11) Is DBNull.Value, Nothing, TempTbl.Rows(idx)(11).ToString())
                                        xRestrictR = IIf(TempTbl.Rows(idx)(12) Is DBNull.Value, Nothing, TempTbl.Rows(idx)(12).ToString())
                                        xHasLots = IIf(TempTbl.Rows(idx)(13) Is DBNull.Value, Nothing, TempTbl.Rows(idx)(13).ToString())
                                        xAllowpricechange = IIf(TempTbl.Rows(idx)(14) Is DBNull.Value, Nothing, TempTbl.Rows(idx)(14).ToString())

                                        ProdNo = ProdCode
                                        If ProdCode = "0" Or ProdCode Is Nothing Then
                                            Continue For
                                        End If





                                        If ProdName = "0" Or ProdName Is Nothing Then
                                            RowNo = idx + 2
                                            ErrorText = "Invalid product name" + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End If

                                        If Organization = "" Or Organization Is Nothing Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + " Organization is mandatory" + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End If
                                        If Not (Organization Is Nothing) Then
                                            If objProduct.IsValidOrganization(Organization) = False Then
                                                RowNo = idx + 2
                                                ErrorText = ErrorText + "Invalid Organization" + ","
                                                isValidRow = False
                                                TotFailed += 1
                                            End If
                                        End If



                                        If Base_UOM = "" Or Base_UOM Is Nothing Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "Base UOM is mandatory" + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End If




                                        If Base_UOM = "0" Or Base_UOM Is Nothing Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "Invalid Base UOM" + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End If
                                        Dim ConversionDefined_Base As Boolean = False
                                        If Not (Base_UOM Is Nothing) Then
                                            If objProduct.IsValidUOM(Base_UOM) = False Then
                                                RowNo = idx + 2
                                                ErrorText = ErrorText + "Invalid Base UOM" + ","
                                                isValidRow = False
                                                TotFailed += 1
                                            Else
                                                If TempTbl_UOM.Rows.Count > 0 Then
                                                    If Not (TempTbl_UOM.Columns(0).ColumnName.ToUpper = "ITEM_CODE" Or TempTbl_UOM.Columns(1).ColumnName.ToUpper = "UOM" Or TempTbl_UOM.Columns(2).ColumnName.ToUpper = "CONVERSION_FACTOR" Or TempTbl_UOM.Columns(3).ColumnName.ToUpper = "IS_SELLABLE") Then
                                                    Else
                                                        Dim SelRow() As DataRow
                                                        SelRow = TempTbl_UOM.Select("ITEM_CODE='" & ProdCode & "' and UOM='" & Base_UOM & "'")
                                                        If SelRow.Length > 0 Then
                                                            If Val(SelRow(0)("CONVERSION_FACTOR").ToString) = 1 Then
                                                                ConversionDefined_Base = True
                                                            End If

                                                        End If

                                                    End If
                                                End If
                                                If ConversionDefined_Base = False Then
                                                    If objProduct.IsValidBaseUOM(ProdCode, Base_UOM) Then
                                                        ConversionDefined_Base = True
                                                    End If
                                                End If
                                                If ConversionDefined_Base = False Then
                                                    RowNo = idx + 2
                                                    ErrorText = ErrorText + " Proper Conversion factor is not defined for Base UOM" + ","
                                                    isValidRow = False
                                                    TotFailed += 1
                                                End If
                                            End If
                                        End If

                                        If xDefaultUOM = "0" Or xDefaultUOM Is Nothing Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "Invalid Default UOM" + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End If
                                        If Not (xDefaultUOM Is Nothing) Then
                                            If objProduct.IsValidUOM(xDefaultUOM) = False Then
                                                RowNo = idx + 2
                                                ErrorText = ErrorText + "Invalid Default UOM" + ","
                                                isValidRow = False
                                                TotFailed += 1
                                            Else
                                                Dim ConversionDefined_Def As Boolean = False
                                                If TempTbl_UOM.Rows.Count > 0 Then
                                                    If Not (TempTbl_UOM.Columns(0).ColumnName.ToUpper = "ITEM_CODE" Or TempTbl_UOM.Columns(1).ColumnName.ToUpper = "UOM" Or TempTbl_UOM.Columns(2).ColumnName.ToUpper = "CONVERSION_FACTOR" Or TempTbl_UOM.Columns(3).ColumnName.ToUpper = "IS_SELLABLE") Then
                                                    Else
                                                        Dim SelRow() As DataRow
                                                        SelRow = TempTbl_UOM.Select("ITEM_CODE='" & ProdCode & "' and UOM='" & Base_UOM & "'")
                                                        If SelRow.Length > 0 Then
                                                            If IsNumeric(SelRow(0)("CONVERSION_FACTOR").ToString) Then
                                                                Dim Conv As Integer
                                                                If Integer.TryParse(Conv, SelRow(0)("CONVERSION_FACTOR").ToString) Then
                                                                    ConversionDefined_Def = True
                                                                End If
                                                            End If

                                                        End If

                                                    End If
                                                End If
                                                If ConversionDefined_Def = False Then
                                                    If objProduct.IsValidDefUOM(ProdCode, Base_UOM) Then
                                                        ConversionDefined_Def = True
                                                    End If
                                                End If
                                                If ConversionDefined_Def = False Then
                                                    RowNo = idx + 2
                                                    ErrorText = ErrorText + "Conversion factor is not defined for Default UOM" + ","
                                                    isValidRow = False
                                                    TotFailed += 1
                                                End If
                                            End If
                                        End If

                                        If xRestrictR = "0" Or xRestrictR Is Nothing Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "Invalid Restrictive Return" + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End If
                                        If Not (xRestrictR Is Nothing) Then
                                            If objProduct.IsValidRestrictiveR(xRestrictR) = False Then
                                                RowNo = idx + 2
                                                ErrorText = ErrorText + "Invalid Restrictive Return" + ","
                                                isValidRow = False
                                                TotFailed += 1
                                            End If
                                        End If


                                        If xHasLots = "0" Or xHasLots Is Nothing Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "Invalid Has Lots" + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End If

                                        If Not (xHasLots Is Nothing) Then
                                            If xHasLots.Trim().ToUpper() <> "Y" And xHasLots.Trim().ToUpper() <> "N" Then
                                                RowNo = idx + 2
                                                ErrorText = ErrorText + "Invalid  Has Lots" + ","
                                                isValidRow = False
                                                TotFailed += 1

                                            End If
                                        End If
                                        If xAllowpricechange = "0" Or xAllowpricechange Is Nothing Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "Invalid Allow Price Change" + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End If

                                        If Not (xAllowpricechange Is Nothing) Then
                                            If xAllowpricechange.Trim().ToUpper() <> "Y" And xAllowpricechange.Trim().ToUpper() <> "N" Then
                                                RowNo = idx + 2
                                                ErrorText = ErrorText + "Invalid Allow Price Change" + ","
                                                isValidRow = False
                                                TotFailed += 1

                                            End If
                                        End If




                                        If objProduct.IsProductCodeExists(ProdCode, Organization) = True Then
                                            IfExisits = "1"
                                        Else
                                            IfExisits = "0"
                                        End If

                                        If Agency.ToString.Trim <> "" Then

                                            If objProduct.IsValidAgency(Agency) = False Then
                                                RowNo = idx + 2
                                                ErrorText = ErrorText + " Agency code doesn't  exist" + ","
                                                isValidRow = False
                                                TotFailed += 1
                                            End If

                                        End If
                                        If Brand.ToString.Trim <> "" Then
                                            If objProduct.IsValidBrand(Brand) = False Then
                                                RowNo = idx + 2
                                                ErrorText = ErrorText + "Brand doesn't  exist" + ","
                                                isValidRow = False
                                                TotFailed += 1
                                            End If
                                        End If
                                        If Category.ToString.Trim <> "" Then
                                            If objProduct.IsValidCategory(Category) = False Then
                                                RowNo = idx + 2
                                                ErrorText = ErrorText + "Category doesn't  exist" + ","
                                                isValidRow = False
                                                TotFailed += 1
                                            End If
                                        End If

                                        If Not (RowNo Is Nothing And ErrorText Is Nothing) Then
                                            Dim h As DataRow = dtErrors.NewRow()
                                            h("RowNo") = RowNo

                                            h("LogInfo") = ErrorText
                                            dtErrors.Rows.Add(h)
                                            RowNo = Nothing
                                            ErrorText = Nothing
                                            isValidRow = False
                                        End If

                                        If isValidRow = True Then
                                            If IfExisits = False Then
                                                If objProduct.SaveProduct(Err_No, Err_Desc, ProdCode, ProdName, Brand, Agency, Base_UOM, Barcode, Val(CostPrice), Category, ProdNo, ItemSize, SubCategory, xDefaultUOM, xRestrictR, xHasLots, xAllowpricechange, "0", Organization, "") Then

                                                    TotSuccess = TotSuccess + 1
                                                    Dim h As DataRow = dtErrors.NewRow()
                                                    h("RowNo") = idx + 2
                                                    h("LogInfo") = "Successfully Inserted"
                                                    dtErrors.Rows.Add(h)
                                                    RowNo = Nothing
                                                    ErrorText = Nothing
                                                    isValidRow = True
                                                Else
                                                    Dim h As DataRow = dtErrors.NewRow()
                                                    h("RowNo") = idx + 2
                                                    h("LogInfo") = "Error occured while saving this row"
                                                    dtErrors.Rows.Add(h)
                                                    RowNo = Nothing
                                                    ErrorText = Nothing
                                                    isValidRow = True
                                                End If
                                            Else
                                                If objProduct.UpdateProduct(Err_No, Err_Desc, ProdCode, ProdName, Brand, Agency, Base_UOM, Barcode, Val(CostPrice), Category, ProdNo, ItemSize, SubCategory, xDefaultUOM, xRestrictR, xHasLots, xAllowpricechange, "0", Organization, "") Then

                                                    TotSuccess = TotSuccess + 1
                                                    Dim h As DataRow = dtErrors.NewRow()
                                                    h("RowNo") = idx + 2
                                                    h("LogInfo") = "Successfully Updated"
                                                    dtErrors.Rows.Add(h)
                                                    RowNo = Nothing
                                                    ErrorText = Nothing
                                                    isValidRow = True
                                                Else
                                                    Dim h As DataRow = dtErrors.NewRow()
                                                    h("RowNo") = idx + 2
                                                    h("LogInfo") = "Error occured while saving this row"
                                                    dtErrors.Rows.Add(h)
                                                    RowNo = Nothing
                                                    ErrorText = Nothing
                                                    isValidRow = True
                                                End If
                                            End If
                                        End If

                                    Next
                                End If


                                '''''''''''''''''''''UOM'''''''''''''''''''''''''''''''

                                dtErrors_u = Session("dtPrErrors_u")

                                If dtErrors_u.Rows.Count > 0 Then
                                    dtErrors_u.Rows.Clear()
                                End If

                                If Not (TempTbl_UOM.Columns(0).ColumnName.ToUpper = "ITEM_CODE" Or TempTbl_UOM.Columns(1).ColumnName.ToUpper = "UOM" Or TempTbl_UOM.Columns(2).ColumnName.ToUpper = "CONVERSION_FACTOR" Or TempTbl_UOM.Columns(3).ColumnName.ToUpper = "IS_SELLABLE") Then



                                    MessageBoxValidation("Please check the template columns are correct", "Validation")
                                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                                    Exit Sub

                                End If

                                If TempTbl_UOM.Rows.Count = 0 Then
                                    MessageBoxValidation("There is no UOM data in the file.", "Validation")
                                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)

                                    Exit Sub
                                End If

                                If TempTbl_UOM.Rows.Count > 0 Then


                                    Dim idx_u As Integer

                                    Dim itemCode_u As String = Nothing
                                    Dim UOM_u As String = Nothing
                                    Dim Conversion_u As String = Nothing
                                    Dim Issellable_u As String = Nothing
                                    Dim dt_org As New DataTable

                                    Dim isValidRow_u As Boolean = True
                                    For idx_u = 0 To TempTbl_UOM.Rows.Count - 1

                                        RowNo_u = Nothing
                                        ErrorText_u = Nothing
                                        isValidRow_u = True

                                        itemCode_u = IIf(TempTbl_UOM.Rows(idx_u)(0) Is DBNull.Value, "", TempTbl_UOM.Rows(idx_u)(0).ToString())
                                        UOM_u = IIf(TempTbl_UOM.Rows(idx_u)(1) Is DBNull.Value, "", TempTbl_UOM.Rows(idx_u)(1).ToString())
                                        Conversion_u = IIf(TempTbl_UOM.Rows(idx_u)(2) Is DBNull.Value, "0", TempTbl_UOM.Rows(idx_u)(2).ToString())
                                        Issellable_u = IIf(TempTbl_UOM.Rows(idx_u)(3) Is DBNull.Value, "", TempTbl_UOM.Rows(idx_u)(3).ToString())



                                        If itemCode_u = "0" Or itemCode_u Is Nothing Then
                                            Continue For
                                        End If


                                        If itemCode_u = "" Or itemCode_u.Trim().Length = 0 Then
                                            RowNo_u = idx_u + 2
                                            ErrorText_u = " Item Code is mandatory" + ","
                                            isValidRow_u = False
                                            TotFailed_u += 1
                                        Else
                                            dt_org = objProduct.GetOrgofITEM(itemCode_u)
                                            Dim OrgID As String = "0"
                                            If dt_org.Rows.Count > 0 Then
                                                OrgID = dt_org.Rows(0)(0).ToString
                                            End If

                                            If objProduct.IsProductCodeExists(itemCode_u, OrgID) = False Then
                                                RowNo_u = idx_u + 2
                                                ErrorText_u = ErrorText_u + " Item Code does not exists " + ","
                                                isValidRow_u = False
                                                TotFailed_u += 1
                                            End If

                                        End If





                                        If UOM_u = "" Or UOM_u.Trim().Length = 0 Then
                                            RowNo_u = idx_u + 2
                                            ErrorText_u = ErrorText_u + " UOM is mandatory" + ","
                                            isValidRow_u = False
                                            TotFailed_u += 1
                                        End If


                                        If Conversion_u = "0" Or Conversion_u.Trim().Length = 0 Then
                                            RowNo_u = idx_u + 2
                                            ErrorText_u = ErrorText_u + " Conversion is mandatory" + ","
                                            isValidRow_u = False
                                            TotFailed_u += 1
                                        Else
                                            If IsNumeric(Conversion_u) = False Then
                                                RowNo_u = idx_u + 2
                                                ErrorText_u = ErrorText_u + " Conversion is not valid " + ","
                                                isValidRow_u = False
                                                TotFailed_u += 1
                                            End If
                                        End If


                                        If UOM_u = "0" Or UOM_u Is Nothing Or objProduct.IsValidUOM(UOM_u) = False Then
                                            RowNo_u = idx_u + 2
                                            ErrorText_u = ErrorText_u + " Invalid UOM" + ","
                                            isValidRow_u = False
                                            TotFailed_u += 1
                                        End If


                                         
                                        If Issellable_u.Trim().ToUpper() <> "Y" And Issellable_u.Trim().ToUpper() <> "N" Then
                                            RowNo_u = idx_u + 2
                                            ErrorText_u = ErrorText_u + "Is sellable value is Invalid" + ","
                                            isValidRow_u = False
                                            TotFailed_u += 1

                                        End If




                                        dt_org = objProduct.GetOrgofITEM(itemCode_u)

                                        If dt_org.Rows.Count > 0 Then



                                            For idx_o = 0 To dt_org.Rows.Count - 1

                                                Dim Organization_u As String
                                                Organization_u = dt_org.Rows(idx_o)(0).ToString()


                                                If objUOM.ValidatePrimaryUOM(itemCode_u, Organization_u, UOM_u) = True Then
                                                    If CDec(Conversion_u) <> 1 Then
                                                        RowNo_u = idx_u + 2
                                                        ErrorText_u = ErrorText_u + "Conversion Rate is 1 for primary UOM " + ","
                                                        isValidRow_u = False
                                                        TotFailed += 1
                                                    End If
                                                End If

                                                'If CDec(Conversion_u) = 1 Then
                                                '    If objUOM.ValidatePrimaryUOM(itemCode_u, Organization_u, UOM_u) = False Then
                                                '        RowNo_u = idx_u + 2
                                                '        ErrorText_u = ErrorText + "Conversion Rate can be 1 for only primary UOM " + ","
                                                '        isValidRow_u = False
                                                '        TotFailed += 1
                                                '    End If
                                                'End If
                                                If isValidRow_u = False Then
                                                    Dim h_u As DataRow = dtErrors_u.NewRow()
                                                    h_u("RowNo") = RowNo_u

                                                    h_u("LogInfo") = ErrorText_u
                                                    dtErrors_u.Rows.Add(h_u)
                                                    RowNo_u = Nothing
                                                    ErrorText_u = Nothing
                                                    isValidRow_u = False
                                                End If


                                                If isValidRow_u = True Then
                                                    log.Debug(itemCode_u)
                                                     
                                                    If objUOM.SaveItemUOM(Err_No, Err_Desc, itemCode_u, Organization_u, UOM_u, CDec(Conversion_u), Issellable_u) Then

                                                        TotSuccess_u = TotSuccess_u + 1
                                                        Dim h_u As DataRow = dtErrors_u.NewRow()
                                                        h_u("RowNo") = idx_u + 2
                                                        h_u("LogInfo") = "Successfully uploaded"
                                                        dtErrors_u.Rows.Add(h_u)
                                                        RowNo_u = Nothing
                                                        ErrorText_u = Nothing
                                                        isValidRow_u = True
                                                    Else
                                                        Dim h_u As DataRow = dtErrors_u.NewRow()
                                                        h_u("RowNo") = idx_u + 2
                                                        h_u("LogInfo") = "Error occured while saving this row"
                                                        dtErrors_u.Rows.Add(h_u)
                                                        RowNo_u = Nothing
                                                        ErrorText_u = Nothing
                                                        isValidRow_u = True
                                                    End If
                                                End If


                                            Next
                                        Else

                                            RowNo_u = idx_u + 2
                                            ErrorText_u = ErrorText_u
                                            isValidRow_u = False
                                            TotFailed_u += 1
                                            Dim h_u As DataRow = dtErrors_u.NewRow()
                                            h_u("RowNo") = RowNo_u

                                            h_u("LogInfo") = ErrorText_u
                                            dtErrors_u.Rows.Add(h_u)
                                            RowNo_u = Nothing
                                            ErrorText_u = Nothing
                                            isValidRow_u = False

                                        End If

                                    Next
                                End If
                                ''''''''''''''''''''''''''''''''''''''''''''''''''



                                ResetFields()
                                BindProductGrid() ' rgProducts.Rebind()
                                ClassUpdatePnl.Update()

                                Session.Remove("dtPrErrors")
                                Session("dtPrErrors") = dtErrors


                                Session.Remove("dtPrErrors_u")
                                Session("dtPrErrors_u") = dtErrors_u

                                Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & "ProductLog" & Now.ToString("yyyyMMdd") + ".txt"

                                ' DataTable2CSV(dtErrors, fn, vbTab)


                                dtErrors.TableName = "Product"
                                dtErrors_u.TableName = "Product_UOM"


                                Dim Ds_error As New DataSet
                                Ds_error.Tables.Add(dtErrors.Copy())
                                Ds_error.Tables.Add(dtErrors_u.Copy())
                                DataTable2CSV_1(Ds_error, fn, vbTab)

                                Session.Remove("PrLogFile")
                                Session("PrLogFile") = fn



                                If TotSuccess > 0 Then
                                    DeleteExcel()
                                    BindProductGrid() ' rgProducts.Rebind()
                                    ClassUpdatePnl.Update()
                                    Dim lblinfo As String = "Information"
                                    Dim lblMessage As String = IIf(TotFailed = 0, "Successfully uploaded", "One or more rows has invalid data. Please check the log")

                                    MessageBoxValidation(lblMessage, lblinfo)
                                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                                    Exit Sub
                                Else

                                    MessageBoxValidation("Please check the uploaded log file", "Validation")
                                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                                    Exit Sub
                                End If
                            End If

                        Catch ex As Exception

                            Err_No = "92823"
                            If Err_Desc Is Nothing Then
                                log.Error(GetExceptionInfo(ex))
                            Else
                                log.Error(Err_Desc)
                            End If
                        End Try


                    Else
                        Str.Append("<span style='font-color:red'> <b >Please import valid Excel template.</b></span>")
                        MessageBoxValidation(Str.ToString(), "Validation")
                        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                    End If

                Catch ex As Exception
                    log.Error(GetExceptionInfo(ex))
                End Try
            End If


        Catch ex As Exception
            MessageBoxValidation("Error occured while uploading file", "Validation")
            log.Error(ex.Message)
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
            MessageBoxValidation("Unexpected error in CSV file please check log", "Validation")
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

                Dim cmd As New OleDbCommand("SELECT * FROM [Product$] WHERE [Item_Code] IS NOT NULL", oledbConn)

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
    Private Function DoXLSXUpload() As DataTable
        Dim dtImport As New DataTable
        Try
            Dim connString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & ViewState("FileName") & " ;Extended Properties=""Excel 12.0;HDR=Yes;IMEX=1"""
            Dim oledbConn As New OleDbConnection(connString)
            Try
                oledbConn.Open()

                Dim cmd As New OleDbCommand("SELECT * FROM [Product$] WHERE [Item_Code] IS NOT NULL", oledbConn)

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
    Sub DataTable2CSV(ByVal table As DataTable, ByVal filename As String, ByVal sepChar As String)
        Dim writer As System.IO.StreamWriter
        Try
            writer = New System.IO.StreamWriter(filename)

            ' first write a line with the columns name
            Dim sep As String = ""
            Dim builder As New System.Text.StringBuilder
            For Each col As DataColumn In table.Columns
                builder.Append(sep).Append(col.ColumnName)
                sep = sepChar
            Next
            writer.WriteLine(builder.ToString())

            ' then write all the rows
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


    Sub DataTable2CSV_1(ByVal Ds As DataSet, ByVal filename As String, ByVal sepChar As String)
        Dim writer As System.IO.StreamWriter
        Try
            writer = New System.IO.StreamWriter(filename)

            For Each table As DataTable In Ds.Tables

                writer.WriteLine("-------------------" + table.TableName + "-------------------------")

                ' first write a line with the columns name
                Dim sep As String = ""
                Dim builder As New System.Text.StringBuilder
                For Each col As DataColumn In table.Columns
                    builder.Append(sep).Append(col.ColumnName)
                    sep = sepChar
                Next
                writer.WriteLine(builder.ToString())

                ' then write all the rows
                For Each row As DataRow In table.Rows
                    sep = ""
                    builder = New System.Text.StringBuilder

                    For Each col As DataColumn In table.Columns
                        builder.Append(sep).Append(row(col.ColumnName))
                        sep = sepChar
                    Next
                    writer.WriteLine(builder.ToString())
                Next



            Next


        Finally
            If Not writer Is Nothing Then writer.Close()
        End Try
    End Sub


    Private Function DoCSVUpload_UOM() As DataTable
        Dim dtImport As New DataTable
        Try
            Dim strConString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "\;Extended Properties=""text;HDR=Yes;FMT=Delimited"""
            Dim oledbConn As New OleDbConnection(strConString)

            Dim cmd As New OleDbCommand("SELECT * FROM [" & ViewState("CSVName") & "]", oledbConn)

            Dim oleda As New OleDbDataAdapter()

            oleda.SelectCommand = cmd



            oleda.Fill(dtImport)
        Catch ex As Exception
            MessageBoxValidation("Unexpected error in CSV file please check log", "Validation")
            log.Error(GetExceptionInfo(ex))
            Throw ex
        End Try
        Return dtImport
    End Function

    Private Function DoXLSUpload_UOM() As DataTable
        Dim dtImport As New DataTable
        Try
            Dim connString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & ViewState("FileName") & " ;Extended Properties=""Excel 8.0;HDR=Yes;IMEX=1"""
            Dim oledbConn As New OleDbConnection(connString)
            Try
                oledbConn.Open()

                Dim cmd As New OleDbCommand("SELECT * FROM [Product_UOM$]", oledbConn)

                Dim oleda As New OleDbDataAdapter()

                oleda.SelectCommand = cmd



                oleda.Fill(dtImport)



            Catch ex As Exception
                '  MessageBoxValidation("Unexpected error in  file please check log", "Validation")
                log.Error(GetExceptionInfo(ex))
            Finally
                ' Close connection
                oledbConn.Close()
            End Try
        Catch ex As Exception
            ' MessageBoxValidation("Unexpected error in  file please check log", "Validation")
            log.Error(GetExceptionInfo(ex))
            Throw ex
        End Try
        Return dtImport
    End Function
    Private Function DoXLSXUpload_UOM() As DataTable
        Dim dtImport As New DataTable
        Try
            Dim connString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & ViewState("FileName") & " ;Extended Properties=""Excel 12.0;HDR=Yes;IMEX=1"""
            Dim oledbConn As New OleDbConnection(connString)
            Try
                oledbConn.Open()

                Dim cmd As New OleDbCommand("SELECT * FROM [Product_UOM$] ", oledbConn)

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
    'Protected Sub btnUOMExp_Click(sender As Object, e As EventArgs) Handles btnUOMExp.Click

    '    Dim finalDataSet As New DataSet
    '    Dim dtItemUOM As New DataTable()


    '    dtItemUOM = objProduct.LoadExportItemUOMTemplate(ddlOrg_UOM.SelectedValue)

    '    finalDataSet.Tables.Add(dtItemUOM)



    '    Dim fn As String = "UOM" + DateTime.Now.ToString("hhmmss") + ".xls"
    '    Dim d As New DataSet


    '    If dtItemUOM.Rows.Count <= 0 Then
    '        MessageBoxValidation("There is no data to export", "Information")
    '        Exit Sub
    '    Else
    '        ExportToExcel_UOM(fn, finalDataSet)
    '    End If



    'End Sub
    Public Function WriteXLSFile_UOM(ByVal pFileName As String, ByVal pDataSet As DataSet) As Boolean
        Try
            'This function CreateWorkbook will cause xls file cannot be opened
            'normally when file size below 7 KB, see my work around below
            'ExcelLibrary.DataSetHelper.CreateWorkbook(pFileName, pDataSet)

            'Create a workbook instance
            Dim workbook As Workbook = New Workbook()
            Dim worksheet As Worksheet
            Dim iRow As Integer = 0
            Dim iCol As Integer = 0
            Dim sTemp As String = String.Empty
            Dim dTemp As Double = 0
            Dim iTemp As Integer = 0
            '  Dim dtTime As DateTime
            Dim count As Integer = 0
            Dim iTotalRows As Integer = 0
            Dim iSheetCount As Integer = 0

            'Read DataSet
            If Not pDataSet Is Nothing And pDataSet.Tables.Count > 0 Then

                'Traverse DataTable inside the DataSet
                For Each dt As DataTable In pDataSet.Tables

                    'Create a worksheet instance
                    iSheetCount = iSheetCount + 1
                    worksheet = New Worksheet("UOM")

                    'Write Table Header
                    For Each dc As DataColumn In dt.Columns
                        worksheet.Cells(iRow, iCol) = New Cell(dc.ColumnName)
                        iCol = iCol + 1
                    Next

                    'Write Table Body
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

                    'Attach worksheet to workbook
                    workbook.Worksheets.Add(worksheet)
                    iTotalRows = iTotalRows + iRow
                Next
            End If

            'Bug on Excel Library, min file size must be 7 Kb
            'thus we need to add empty row for safety
            If iTotalRows < 100 Then
                worksheet = New Worksheet("Sheet2")
                count = 1
                Do While count < 100
                    worksheet.Cells(count, 0) = New Cell(" ")
                    count = count + 1
                Loop
                workbook.Worksheets.Add(worksheet)
            End If

            workbook.Save(pFileName)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Private Sub ExportToExcel_UOM(ByVal strFileName As String, ByVal ds As DataSet)


        Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & strFileName
        WriteXLSFile_UOM(fn, ds)

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


    Sub FillOrganization()
        Dim dt_org As New DataTable
        Dim objCommon As New SalesWorx.BO.Common.Common
        Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)

        dt_org = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
        ddlOrg.DataSource = dt_org
        ddlOrg.Items.Clear()
        ddlOrg.Items.Add(New RadComboBoxItem("Select Organization"))
        ddlOrg.AppendDataBoundItems = True
        ddlOrg.DataValueField = "MAS_Org_ID"
        ddlOrg.DataTextField = "Description"
        ddlOrg.DataBind()

        If ddlOrg.Items.Count = 2 Then
            ddlOrg.SelectedIndex = 1
        End If


        ddlorg_F.DataSource = dt_org
        ddlorg_F.Items.Clear()
        ddlorg_F.Items.Add(New RadComboBoxItem("Select Organization"))
        ddlorg_F.AppendDataBoundItems = True
        ddlorg_F.DataValueField = "MAS_Org_ID"
        ddlorg_F.DataTextField = "Description"
        ddlorg_F.DataBind()
        If ddlorg_F.Items.Count = 2 Then
            ddlorg_F.SelectedIndex = 1
        End If
    End Sub

    Sub FillUOM()
        Dim dt_uom As New DataTable
        dt_uom = objProduct.GetUOM()
        ddlUOM.DataTextField = "Description"
        ddlUOM.DataValueField = "Code"
        ddlUOM.DataSource = dt_uom
        ddlUOM.DataBind()
        ddlUOM.SelectedIndex = 0

        UOM.DataTextField = "Description"
        UOM.DataValueField = "Code"
        UOM.DataSource = dt_uom
        UOM.DataBind()
        UOM.SelectedIndex = 0
    End Sub
    Sub FillBrand()
        ddlBrand.DataTextField = "Description"
        ddlBrand.DataValueField = "Code"
        ddlBrand.DataSource = objProduct.GetBrand()
        ddlBrand.DataBind()
        ddlBrand.SelectedIndex = 0
    End Sub
    Sub FillAgency()

        Dim dt_agency As New DataTable
        dt_agency = objProduct.GetAgency()
        ddlAgency.DataTextField = "Description"
        ddlAgency.DataValueField = "Code"
        ddlAgency.DataSource = dt_agency
        ddlAgency.DataBind()
        ddlAgency.SelectedIndex = 0



        ddlAgency_F.DataTextField = "Description"
        ddlAgency_F.DataValueField = "Code"
        ddlAgency_F.DataSource = dt_agency
        ddlAgency_F.DataBind()
        ddlAgency_F.SelectedIndex = 0
    End Sub
    Sub FillCategory()
        ddlCategory.DataTextField = "Description"
        ddlCategory.DataValueField = "Code"
        ddlCategory.DataSource = objProduct.GetCategory()
        ddlCategory.DataBind()
        ddlCategory.SelectedIndex = 0
    End Sub

    Sub FillUOM_Exp()
        Dim objCommon As New SalesWorx.BO.Common.Common
        Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
        'ddlOrg_UOM.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
        'ddlOrg_UOM.Items.Clear()
        'ddlOrg_UOM.DataValueField = "Org_ID"
        'ddlOrg_UOM.DataTextField = "Description"
        'ddlOrg_UOM.DataBind()
    End Sub

    Sub FillDefaultUOM()


        ddldefaultUOM.DataTextField = "Description"
        ddldefaultUOM.DataValueField = "Code"
        ddldefaultUOM.DataSource = objProduct.GetUOM()
        ddldefaultUOM.DataBind()
        ddldefaultUOM.SelectedIndex = 0

    End Sub

    Sub FillRestrictiveReturn()


        ddlRestrictiveR.DataTextField = "Description"
        ddlRestrictiveR.DataValueField = "Code"
        ddlRestrictiveR.DataSource = objProduct.GetRestrictiveReturn()
        ddlRestrictiveR.DataBind()
        ddlRestrictiveR.SelectedIndex = 0

    End Sub

    Sub BindProductGrid()
        Dim org_id_f As String = ""
        Dim item_code_f As String = ""
        Dim Desc_f As String = ""
        Dim Agency_f As String = ""
        Dim UOM_f As String = ""
        'If Opt_serach Is Nothing Then
        '    Opt_serach = "0"
        'End If
        If Session("Opt_serach") Is Nothing Then
            Session("Opt_serach") = "0"
        End If

        If Session("Opt_serach") = "1" Then
            If ddlAgency_F.SelectedIndex > 0 Then
                Agency_f = ddlAgency_F.SelectedValue
            End If

            If UOM.SelectedIndex > 0 Then
                UOM_f = UOM.SelectedValue
            End If
            item_code_f = txtitem_code.Text
            Desc_f = txtDescription.Text

            org_id_f = ddlorg_F.SelectedValue
        End If
        rgProducts1.DataSource = objProduct.GetProductGrid(Session("Opt_serach"), org_id_f, item_code_f, Desc_f, UOM_f, Agency_f)
        rgProducts1.DataBind()
        ' Opt_serach = 0

    End Sub


    Protected Sub lbLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbLog.Click
        If Not Session("PrLogFile") Is Nothing Then
            Dim fileValue As String = Session("PrLogFile")

            Dim file As System.IO.FileInfo = New FileInfo(fileValue)

            If file.Exists Then

                'Process.Start("notepad.exe", fileValue)
                Response.Clear()

                Response.AddHeader("Content-Disposition", "attachment; filename=" & file.Name)

                Response.AddHeader("Content-Length", file.Length.ToString())

                Response.WriteFile(file.FullName)

                Response.[End]()

            Else
                MessageBoxValidation("File does not exist", "Information")
                Exit Sub

            End If
        Else
            MessageBoxValidation("There is no log to view.", "Information")
            Exit Sub

        End If

    End Sub

    Protected Sub btndownloadTemplate_Click(sender As Object, e As EventArgs) Handles btndownloadTemplate.Click

        Dim Filename As String = System.Configuration.ConfigurationManager.AppSettings("ExcelTemplatePath") & "Product_Info.xls"
        Dim TheFile As FileInfo = New FileInfo(Filename)
        If TheFile.Exists Then
            Dim strFileName As String = "Template" + Now.ToString("ddMMMyyHHmmss") + ".xls"

            'Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "Others\" & strFileName
            'WriteXLSFile(fn, dtRelation)

            'ViewState("AvailableBenefitParameter") = fn

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

    Private Sub DeleteExcelTemplate()
        Try
            Dim Filename As String = ViewState("SampleTemplate")
            'Dim Foldername As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & Filename
            Dim TheFile As FileInfo = New FileInfo(Filename)
            If TheFile.Exists Then
                File.Delete(Filename)
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))

        End Try
    End Sub
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ResetFields()
        txtProdCode.Enabled = True
        Me.DocWindow.Title = "Add Product"
        Me.DocWindow.VisibleOnPageLoad = True
    End Sub

    Private Sub btnFilter_Click(sender As Object, e As EventArgs) Handles btnFilter.Click
        Try
            lbl_msg_f.Text = ""
            Opt_serach = 1
            Session("Opt_serach") = "1"
            If ddlorg_F.SelectedIndex <= 0 Then
                MessageBoxValidation("Please select the organization", "Validation")
                Exit Sub
            End If
            BindProductGrid()
            ClassUpdatePnl.Update()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub btnclear_f_Click(sender As Object, e As EventArgs) Handles btnclear_f.Click
        Opt_serach = 0
        Session("Opt_serach") = "0"
        BindProductGrid()
        lbl_msg_f.Text = ""
        txtDescription.Text = ""
        txtitem_code.Text = ""
        ddlorg_F.ClearSelection()
        ClassUpdatePnl.Update()
    End Sub

    Private Sub rgProducts1_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles rgProducts1.PageIndexChanged
        BindProductGrid()
    End Sub

    Private Sub rgProducts1_SortCommand(sender As Object, e As GridSortCommandEventArgs) Handles rgProducts1.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindProductGrid()
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
