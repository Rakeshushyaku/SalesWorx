Imports SalesWorx.BO.Common
Imports Telerik.Web.UI
Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports ExcelLibrary.SpreadSheet
Public Class ImportAssortmentPlan
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
    Private dtOrdItems As New DataTable
    Private dtGetItems As New DataTable
    Private dtSlabs As New DataTable
    Private Property ddl_Org As Object

    Private Property ddl_FilterOrg As Object

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        Try
            If Not IsPostBack Then
                Dim HasPermission As Boolean = False
                ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
                'If Not HasPermission Then
                '    Err_No = 500
                '    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
                'End If



                Me.lblOrgID.Text = "2100" 'Request.QueryString("ORGID").ToString()
                Me.lblOrgName.Text = "Site - 2100" 'Request.QueryString("ORGNAME").ToString()
                Me.lblPlanName.Text = "xyz-Imp" 'Request.QueryString("Desc").ToString()
                Me.lblPlanId.Text = "54" ' Request.QueryString("PGID").ToString()
                Me.lblTransType.Text = "CASH" ' Request.QueryString("TRANTYPE").ToString()

                'LoadOrgHeads()


               ' BindDistribution_ctl()

                ViewState("FileType") = Nothing
                ViewState("FileName") = Nothing
                ViewState("CSVName") = Nothing


                Session.Remove("dtOrdItems")
                Session.Remove("dtGetItems")
                Session.Remove("dtSlabs")
                SetOrderItemsTable()
                SetGetItemsTable()
                SetSlabsTable()
                ' BindItemDetails(Me.lblPlanId.Text, Me.lblOrgID.Text)
                BindSlabs(Me.lblPlanId.Text)

            Else
                MPEImport.VisibleOnPageLoad = False
                MPEAdd.VisibleOnPageLoad = False
            End If


        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Order_005") & "&next=Welcome.aspx&Title=Bonus Definition", False)
        End Try
    End Sub
    'Protected Sub ddl_FilterOrg_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_FilterOrg.SelectedIndexChanged
    '    Try



    '        'ddl_FilterCustomer.ClearSelection()
    '        'ddl_FilterCustomer.Items.Clear()
    '        'ddl_FilterCustomer.Text = ""

    '        'ddl_FilterVan.ClearSelection()
    '        'ddl_FilterVan.Items.Clear()
    '        'ddl_FilterVan.Text = ""



    '        'LoadVan()

    '        'BindDistribution_ctl()
    '        Panel.Update()

    '    Catch ex As Exception
    '        log.Error(ex.Message.ToString())
    '    End Try
    'End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Try

            ddl_OrgAdd.ClearSelection()
            ddl_OrgAdd.SelectedIndex = 0

            ddl_VanAdd.DataSource = Nothing
            ddl_VanAdd.DataBind()


            ddl_VanAdd.ClearSelection()
            ddl_VanAdd.Items.Clear()
            ddl_VanAdd.Text = ""

            ddl_Customer.ClearSelection()
            ddl_Customer.Items.Clear()
            ddl_Customer.Text = ""
            ddl_Customer.Enabled = True

            chkIsOptional.Checked = False

            Me.lblmsgPopUp.Text = ""

            Me.MPEAdd.VisibleOnPageLoad = True
            btnSave.Visible = True
            btnUpdate.Visible = False

            ddl_OrgAdd.Enabled = True
            ddl_VanAdd.Enabled = True
            ddl_Customer.Enabled = True


            Panel.Update()



        Catch ex As Exception

            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Private Sub btnImportWindow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportWindow.Click
        'lbLog.Visible = False
        'If Me.ExcelFileUpload.FileName = Nothing Then

        '    Me.lblUpMsg.Text = "Select filename "

        '    Me.MPEImport.VisibleOnPageLoad = True
        '    Exit Sub
        'End If


        'Session("dtDisCtl") = Nothing
        'Dim dtErrors As New DataTable
        'dtErrors = SetErrorsTable().Copy
        'Dim Str As New StringBuilder

        'Dim TotSuccess As Integer = 0
        'Dim TotFailed As Integer = 0
        'Try


        '    ViewState("FileType") = Me.ExcelFileUpload.PostedFile.ContentType
        '    If ExcelFileUpload.FileName.ToString.ToLower().EndsWith(".xls") Or ExcelFileUpload.FileName.ToString.ToLower().EndsWith(".xlsx") Then

        '        Dim Foldername As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath")
        '        If Not Foldername.EndsWith("\") Then
        '            Foldername = Foldername & "\"
        '        End If
        '        If Directory.Exists(Foldername.Substring(0, Len(Foldername) - 1)) = False Then
        '            Directory.CreateDirectory(Foldername.Substring(0, Len(Foldername) - 1))
        '        End If
        '        If ExcelFileUpload.FileName.ToString.EndsWith(".csv") Then
        '            Dim FName As String
        '            FName = Now.Hour & Now.Minute & Now.Second & ExcelFileUpload.FileName
        '            ViewState("FileName") = Foldername & FName
        '            ViewState("CSVName") = FName
        '        Else
        '            ViewState("FileName") = Foldername & Now.Hour & Now.Minute & Now.Second & ExcelFileUpload.FileName
        '        End If

        '        ExcelFileUpload.SaveAs(ViewState("FileName"))

        '        Try
        '            Dim st As Boolean = False

        '            If ViewState("FileType") IsNot Nothing And ViewState("FileName") IsNot Nothing Then
        '                Dim TempTbl As New DataTable
        '                If TempTbl.Rows.Count > 0 Then
        '                    TempTbl.Rows.Clear()
        '                End If
        '                Dim col As DataColumn

        '                col = New DataColumn
        '                col.ColumnName = "Item_Code"
        '                col.DataType = System.Type.GetType("System.String")
        '                col.ReadOnly = False
        '                col.Unique = False
        '                TempTbl.Columns.Add(col)


        '                col = New DataColumn
        '                col.ColumnName = "UOM"
        '                col.DataType = System.Type.GetType("System.String")
        '                col.ReadOnly = False
        '                col.Unique = False
        '                TempTbl.Columns.Add(col)



        '                col = New DataColumn
        '                col.ColumnName = "Is_Get_Item"
        '                col.DataType = System.Type.GetType("System.String")
        '                col.ReadOnly = False
        '                col.Unique = False
        '                TempTbl.Columns.Add(col)


        '                col = New DataColumn
        '                col.ColumnName = "Is_Mandatory"
        '                col.DataType = System.Type.GetType("System.String")
        '                col.ReadOnly = False
        '                col.Unique = False
        '                TempTbl.Columns.Add(col)




        '                Dim TempTbl_Slab As New DataTable
        '                If TempTbl_Slab.Rows.Count > 0 Then
        '                    TempTbl_Slab.Rows.Clear()
        '                End If
        '                Dim col_Slab As DataColumn

        '                col_Slab = New DataColumn
        '                col_Slab.ColumnName = "From_Qty"""
        '                col_Slab.DataType = System.Type.GetType("System.String")
        '                col_Slab.ReadOnly = False
        '                col_Slab.Unique = False
        '                TempTbl_Slab.Columns.Add(col_Slab)


        '                col_Slab = New DataColumn
        '                col_Slab.ColumnName = "To_Qty"
        '                col_Slab.DataType = System.Type.GetType("System.String")
        '                col_Slab.ReadOnly = False
        '                col_Slab.Unique = False
        '                TempTbl_Slab.Columns.Add(col_Slab)



        '                col_Slab = New DataColumn
        '                col_Slab.ColumnName = "Type"
        '                col_Slab.DataType = System.Type.GetType("System.String")
        '                col_Slab.ReadOnly = False
        '                col_Slab.Unique = False
        '                TempTbl_Slab.Columns.Add(col_Slab)


        '                col_Slab = New DataColumn
        '                col_Slab.ColumnName = "Bonus Qty"
        '                col_Slab.DataType = System.Type.GetType("System.String")
        '                col_Slab.ReadOnly = False
        '                col_Slab.Unique = False
        '                TempTbl_Slab.Columns.Add(col_Slab)


        '                If ViewState("FileName").ToString.EndsWith(".csv") Then
        '                    TempTbl = DoCSVUpload()
        '                ElseIf ViewState("FileName").ToString.EndsWith(".xls") Then
        '                    TempTbl = DoXLSUpload()
        '                ElseIf ViewState("FileName").ToString.EndsWith(".xlsx") Then
        '                    TempTbl = DoXLSXUpload()
        '                End If



        '                If ViewState("FileName").ToString.EndsWith(".csv") Then
        '                    TempTbl_Slab = DoCSVUpload_Slab()
        '                ElseIf ViewState("FileName").ToString.EndsWith(".xls") Then
        '                    TempTbl_Slab = DoXLSUpload_Slab()
        '                ElseIf ViewState("FileName").ToString.EndsWith(".xlsx") Then
        '                    TempTbl_Slab = DoXLSXUpload_Slab()
        '                End If

        '                If TempTbl.Columns.Count >= 4 Then
        '                    If Not (TempTbl.Columns(0).ColumnName.ToLower = "Item_Code" And TempTbl.Columns(1).ColumnName.ToLower = "UOM" And TempTbl.Columns(2).ColumnName.ToLower = "Is_Get_Item" And TempTbl.Columns(3).ColumnName.ToLower = "Is_Mandatory") Then
        '                        Me.lblUpMsg.Text = "Please check the template columns are correct"
        '                        Me.MPEImport.VisibleOnPageLoad = True
        '                        Exit Sub
        '                    End If
        '                Else
        '                    Me.lblUpMsg.Text = "Invalid Template"
        '                    Me.MPEImport.VisibleOnPageLoad = True
        '                    Exit Sub
        '                End If




        '                If TempTbl.Rows.Count = 0 Then
        '                    Me.lblUpMsg.Text = "There is no data in your file."
        '                    Me.MPEImport.VisibleOnPageLoad = True
        '                    Exit Sub
        '                End If

        '                Dim RowNo As String = Nothing
        '                Dim ErrorText As String = Nothing

        '                If TempTbl.Rows.Count > 0 Then
        '                    Dim idx As Integer
        '                    SetOrderItemsTable()
        '                    SetGetItemsTable()
        '                    SetSlabsTable()

        '                    For idx = 0 To TempTbl.Rows.Count - 1
        '                        Dim Item_Code As String = Nothing
        '                        Dim UOM As String = Nothing
        '                        Dim Is_Get_Item As String = Nothing
        '                        Dim Is_Mandatory As String = Nothing

        '                        Dim isValidRow As Boolean = True

        '                        Item_Code = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, "", TempTbl.Rows(idx)(0).ToString().Trim())
        '                        UOM = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, "", TempTbl.Rows(idx)(1).ToString().Trim())
        '                        Is_Get_Item = IIf(TempTbl.Rows(idx)(2) Is DBNull.Value, "", TempTbl.Rows(idx)(2).ToString().Trim())
        '                        Is_Mandatory = IIf(TempTbl.Rows(idx)(3) Is DBNull.Value, "", TempTbl.Rows(idx)(3).ToString().Trim())



        '                        If Item_Code.Trim() = "" Or Item_Code Is Nothing Then
        '                            RowNo = idx + 2
        '                            ErrorText = ErrorText + "Item Code is mandatory " + Item_Code + " ,"
        '                            isValidRow = False
        '                            TotFailed += 1
        '                        End If
        '                        If objProduct.IsProductCodeExists(Item_Code, ddl_FilterOrg.SelectedValue) = False Then
        '                            RowNo = idx + 2
        '                            ErrorText = ErrorText + " Item Code does not exists " + ","
        '                            isValidRow = False
        '                            TotFailed += 1
        '                        End If

        '                        If UOM.Trim() = "" Or UOM Is Nothing Then
        '                            RowNo = idx + 2
        '                            ErrorText = ErrorText + "UOM is mandatory " + UOM + " ,"
        '                            isValidRow = False
        '                            TotFailed += 1
        '                        Else

        '                            If objProduct.IsValidUOM(UOM) = False Then
        '                                RowNo = idx + 2
        '                                ErrorText = ErrorText + "Invalid  UOM: " + UOM + " ,"
        '                                isValidRow = False
        '                                TotFailed += 1
        '                            End If
        '                        End If

        '                        If Is_Get_Item.Trim() = "" Or Is_Get_Item Is Nothing Then
        '                            RowNo = idx + 2
        '                            ErrorText = ErrorText + "Is_Get_Item  is mandatory " + Is_Get_Item + " ,"
        '                            isValidRow = False
        '                            TotFailed += 1
        '                        ElseIf Is_Get_Item.Trim().ToUpper() <> "N" Or Is_Get_Item.Trim().ToUpper() <> "Y" Then
        '                            RowNo = idx + 2
        '                            ErrorText = ErrorText + "Invalid Is_Get_Item : " + Is_Get_Item + " ,"
        '                            isValidRow = False
        '                            TotFailed += 1
        '                        End If

        '                        If Is_Mandatory.Trim().ToUpper() <> "N" Or Is_Mandatory.Trim().ToUpper() <> "Y" Then
        '                            RowNo = idx + 2
        '                            ErrorText = ErrorText + "Invalid Is_Mandatory : " + Is_Mandatory + " ,"
        '                            isValidRow = False
        '                            TotFailed += 1
        '                        End If

        '                        ''     ElseIf Not ValidateQty(Val(qty).ToString(), Item, ddl_Organization.SelectedItem.Value, UOM, Conversion) Then  






        '                        If Not (RowNo Is Nothing And ErrorText Is Nothing) Then
        '                            Dim h As DataRow = dtErrors.NewRow()
        '                            h("RowNo") = RowNo
        '                            h("LogInfo") = ErrorText
        '                            dtErrors.Rows.Add(h)
        '                            RowNo = Nothing
        '                            ErrorText = Nothing
        '                            isValidRow = True
        '                        Else



        '                            If Is_Get_Item.Trim().ToUpper() = "N" Then
        '                                Dim dr_Ord As DataRow = dtOrdItems.NewRow()
        '                                dr_Ord("ItemCode") = Item_Code
        '                                dr_Ord("Description") = (New SalesWorx.BO.Common.Product).GetItemNameFromCode(Err_No, Err_Desc, Item_Code, ddl_FilterOrg.SelectedValue)
        '                                dr_Ord("UOM") = UOM
        '                                dr_Ord("GetItem") = Is_Get_Item
        '                                dr_Ord("IsMandatory") = Is_Mandatory
        '                                dtOrdItems.Rows.Add(dr_Ord)
        '                            ElseIf Is_Get_Item.Trim().ToUpper() = "Y" Then
        '                                Dim dr_Get As DataRow = dtGetItems.NewRow()
        '                                dr_Get("ItemCode") = Item_Code
        '                                dr_Get("Description") = (New SalesWorx.BO.Common.Product).GetItemNameFromCode(Err_No, Err_Desc, Item_Code, ddl_FilterOrg.SelectedValue)
        '                                dr_Get("UOM") = UOM
        '                                dr_Get("GetItem") = Is_Get_Item
        '                                dr_Get("IsMandatory") = Is_Mandatory
        '                                dtGetItems.Rows.Add(dr_Get)
        '                            End If


        '                            'If objCustomer.SaveDistribution_CTL(Err_No, Err_Desc, "", SID, CustomerID, SiteID, Is_Optional.Trim().ToUpper(), CType(Session("User_Access"), UserAccess).UserID) = True Then
        '                            '    TotSuccess = TotSuccess + 1
        '                            '    Dim h As DataRow = dtErrors.NewRow()
        '                            '    h("RowNo") = idx + 2
        '                            '    h("LogInfo") = "Successfully uploaded"
        '                            '    dtErrors.Rows.Add(h)
        '                            '    RowNo = Nothing
        '                            '    ErrorText = Nothing
        '                            '    isValidRow = True
        '                            'Else
        '                            '    Dim h As DataRow = dtErrors.NewRow()
        '                            '    h("RowNo") = idx + 2
        '                            '    h("LogInfo") = "Error occured while uploading this row"
        '                            '    dtErrors.Rows.Add(h)
        '                            '    RowNo = Nothing
        '                            '    ErrorText = Nothing
        '                            '    isValidRow = True
        '                            'End If
        '                        End If

        '                    Next
        '                End If


        '                ' If TotSuccess > 0 Then

        '                DeleteExcel()
        '                lblUpMsg.Text = IIf(TotFailed = 0, "Successfully uploaded", "One or more rows has invalid data. Please check the log")
        '                Me.MPEImport.VisibleOnPageLoad = True
        '                BindDistribution_ctl()
        '                'End If
        '            End If

        '            dgvErros.Visible = False
        '            If dtErrors.Rows.Count > 0 Then
        '                lbLog.Visible = True
        '            Else
        '                lbLog.Visible = False
        '            End If
        '            Me.dgvErros.DataSource = dtErrors
        '            Me.dgvErros.DataBind()
        '            Session.Remove("dtDisCtl")
        '            Session("dtDisCtl") = dtErrors.Copy


        '            Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & "DisCtlLog_" & Now.ToString("yyyyMMdd") + ".txt"
        '            DataTable2CSV(dtErrors, fn, vbTab)
        '            If dtErrors.Rows.Count > 0 Then
        '                lbLog.Visible = True
        '            End If
        '            Session.Remove("SimpleLogInfo")
        '            Session("SimpleLogInfo") = fn




        '        Catch ex As Exception

        '            Err_No = "74085"
        '            If Err_Desc Is Nothing Then
        '                log.Error(GetExceptionInfo(ex))
        '            Else
        '                log.Error(Err_Desc)
        '            End If
        '        End Try
        '    Else

        '        lblUpMsg.Text = "Please import valid Excel template."
        '        Me.MPEImport.VisibleOnPageLoad = True
        '    End If


        'Catch ex As Exception
        '    log.Error(GetExceptionInfo(ex))
        'End Try
        Me.lblUpMsg.Text = ""
        Session("dtDisCtl") = Nothing
        dgvErros.DataSource = Nothing
        dgvErros.DataBind()
        dgvErros.Visible = False
        lbLog.Visible = False
        Me.MPEImport.VisibleOnPageLoad = True

    End Sub


    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        'Try

        '    If Me.ddl_FilterOrg.SelectedIndex > 0 Then
        '        Dim dtData As New DataTable
        '        Dim Customer As String
        '        Dim ProdVal As String = ""
        '        Dim CustomerID As String = ""
        '        Dim SiteID As String = ""
        '        Dim SID As String = ""

        '        Dim IDs() As String
        '        'If ddl_FilterCustomer.SelectedValue <> "" Then
        '        '    Customer = ddl_FilterCustomer.SelectedValue
        '        '    IDs = Customer.Split("$")
        '        '    CustomerID = IDs(0)
        '        '    SiteID = IDs(1)
        '        'Else
        '        '    CustomerID = 0
        '        '    SiteID = 0
        '        'End If

        '        'If ddl_FilterVan.SelectedIndex > 0 Then
        '        '    SID = ddl_FilterVan.SelectedValue
        '        'Else
        '        '    SID = "0"
        '        'End If


        '        Dim dtOriginal As New DataTable()

        '        dtOriginal = objCustomer.GetDistribution_ctl(Err_No, Err_Desc, ddl_FilterOrg.SelectedItem.Value, SID, CustomerID, SiteID)
        '        Dim dtTemp As New DataTable()

        '        'Creating Header Row

        '        dtTemp.Columns.Add("Site_No")
        '        dtTemp.Columns.Add("Van_Code")
        '        dtTemp.Columns.Add("Van")
        '        dtTemp.Columns.Add("Customer_No")
        '        dtTemp.Columns.Add("Customer_Name")
        '        dtTemp.Columns.Add("Is_Optional")


        '        Dim drAddItem As DataRow
        '        For i As Integer = 0 To dtOriginal.Rows.Count - 1
        '            drAddItem = dtTemp.NewRow()
        '            drAddItem(0) = IIf(dtOriginal.Rows(i)("Site_No") Is DBNull.Value, "All", dtOriginal.Rows(i)("Site_No").ToString())
        '            drAddItem(1) = IIf(dtOriginal.Rows(i)("SalesRep_Number") Is DBNull.Value, "All", dtOriginal.Rows(i)("SalesRep_Number").ToString())
        '            drAddItem(2) = IIf(dtOriginal.Rows(i)("SalesRep_Name") Is DBNull.Value, "All", dtOriginal.Rows(i)("SalesRep_Name").ToString())
        '            drAddItem(3) = IIf(dtOriginal.Rows(i)("Customer_No") Is DBNull.Value, "All", dtOriginal.Rows(i)("Customer_No").ToString())
        '            drAddItem(4) = IIf(dtOriginal.Rows(i)("Customer_Name") Is DBNull.Value, "All", dtOriginal.Rows(i)("Customer_Name").ToString())
        '            drAddItem(5) = IIf(dtOriginal.Rows(i)("Is_Optional") Is DBNull.Value, "All", dtOriginal.Rows(i)("Is_Optional").ToString())

        '            dtTemp.Rows.Add(drAddItem)
        '        Next

        '        If dtOriginal.Rows.Count = 0 Then

        '            MessageBoxValidation("There is no data.", "Information")
        '            Exit Sub

        '            drAddItem = dtTemp.NewRow()
        '            drAddItem(0) = ""
        '            drAddItem(1) = ""
        '            drAddItem(2) = ""
        '            drAddItem(3) = ""
        '            drAddItem(4) = ""
        '            drAddItem(5) = ""

        '            dtTemp.Rows.Add(drAddItem)
        '        End If


        '        Dim dg As New DataGrid()
        '        dg.DataSource = dtTemp
        '        dg.DataBind()
        '        If dtTemp.Rows.Count > 0 Then

        '            Dim fn As String = "Distribution_CTL" + Now.ToString("ddMMMyyHHmmss") + ".xls"
        '            Dim d As New DataSet
        '            d.Tables.Add(dtTemp)

        '            ExportToExcel(fn, d)

        '        End If
        '    Else
        '        MessageBoxValidation("Please select a organization", "Information")
        '        Exit Sub
        '    End If

        'Catch ex As Exception
        '    log.Error(GetExceptionInfo(ex))
        '    Response.Redirect("information.aspx?mode=1&errno=" & "74071" & "&msg=" & AppMsgHandler.GetErrorMessage("SWX_BO_Msg_005") & "&next=welcome.aspx", False)
        'Finally
        'End Try
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
                    worksheet = New Worksheet("Sheet" & iSheetCount.ToString())

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
    Private Function SetErrorsTable_Slab() As DataTable
        Dim col As DataColumn
        Dim dtErrors_Slab As New DataTable

        col = New DataColumn()
        col.ColumnName = "RowNo"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtErrors_Slab.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "LogInfo"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtErrors_Slab.Columns.Add(col)

        Return dtErrors_Slab
    End Function
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Private Sub btnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImport.Click
        lbLog.Visible = False
        If Me.ExcelFileUpload.FileName = Nothing Then

            Me.lblUpMsg.Text = "Select filename "

            Me.MPEImport.VisibleOnPageLoad = True
            Exit Sub
        End If

        Session.Remove("dtOrdItems")
        Session.Remove("dtGetItems")
        Session.Remove("dtSlabs")
        SetOrderItemsTable()
        SetGetItemsTable()
        SetSlabsTable()
        ' BindItemDetails(Me.lblPlanId.Text, Me.lblOrgID.Text)
        BindSlabs(Me.lblPlanId.Text)


        Session("dtDisCtl") = Nothing
        Dim dtErrors As New DataTable
        Dim dtErrors_slab As New DataTable
        dtErrors = SetErrorsTable().Copy
        dtErrors_slab = SetErrorsTable_Slab().Copy

        Dim Str As New StringBuilder

        Dim TotSuccess As Integer = 0
        Dim TotFailed As Integer = 0


        Dim TotSuccess_Slab As Integer = 0
        Dim TotFailed_Slab As Integer = 0
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
                        col.ColumnName = "Item_Code"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)


                        col = New DataColumn
                        col.ColumnName = "UOM"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)



                        col = New DataColumn
                        col.ColumnName = "Is_Get_Item"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)


                        col = New DataColumn
                        col.ColumnName = "Is_Mandatory"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)




                        Dim TempTbl_Slab As New DataTable
                        If TempTbl_Slab.Rows.Count > 0 Then
                            TempTbl_Slab.Rows.Clear()
                        End If
                        Dim col_Slab As DataColumn

                        col_Slab = New DataColumn
                        col_Slab.ColumnName = "From_Qty"""
                        col_Slab.DataType = System.Type.GetType("System.String")
                        col_Slab.ReadOnly = False
                        col_Slab.Unique = False
                        TempTbl_Slab.Columns.Add(col_Slab)


                        col_Slab = New DataColumn
                        col_Slab.ColumnName = "To_Qty"
                        col_Slab.DataType = System.Type.GetType("System.String")
                        col_Slab.ReadOnly = False
                        col_Slab.Unique = False
                        TempTbl_Slab.Columns.Add(col_Slab)



                        col_Slab = New DataColumn
                        col_Slab.ColumnName = "Type"
                        col_Slab.DataType = System.Type.GetType("System.String")
                        col_Slab.ReadOnly = False
                        col_Slab.Unique = False
                        TempTbl_Slab.Columns.Add(col_Slab)


                        col_Slab = New DataColumn
                        col_Slab.ColumnName = "Bonus Qty"
                        col_Slab.DataType = System.Type.GetType("System.String")
                        col_Slab.ReadOnly = False
                        col_Slab.Unique = False
                        TempTbl_Slab.Columns.Add(col_Slab)


                        If ViewState("FileName").ToString.EndsWith(".csv") Then
                            TempTbl = DoCSVUpload()
                        ElseIf ViewState("FileName").ToString.EndsWith(".xls") Then
                            TempTbl = DoXLSUpload()
                        ElseIf ViewState("FileName").ToString.EndsWith(".xlsx") Then
                            TempTbl = DoXLSXUpload()
                        End If



                        If ViewState("FileName").ToString.EndsWith(".csv") Then
                            TempTbl_Slab = DoCSVUpload_Slab()
                        ElseIf ViewState("FileName").ToString.EndsWith(".xls") Then
                            TempTbl_Slab = DoXLSUpload_Slab()
                        ElseIf ViewState("FileName").ToString.EndsWith(".xlsx") Then
                            TempTbl_Slab = DoXLSXUpload_Slab()
                        End If

                        If TempTbl.Columns.Count >= 4 Then
                            If Not (TempTbl.Columns(0).ColumnName.ToLower = "item_code" And TempTbl.Columns(1).ColumnName.ToLower = "uom" And TempTbl.Columns(2).ColumnName.ToLower = "is_get_item" And TempTbl.Columns(3).ColumnName.ToLower = "is_mandatory") Then
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

                        Dim RowNo_Slab As String = Nothing
                        Dim ErrorText_Slab As String = Nothing

                        If TempTbl.Rows.Count > 0 Then
                            Dim idx As Integer
                          

                            For idx = 0 To TempTbl.Rows.Count - 1
                                Dim Item_Code As String = Nothing
                                Dim UOM As String = Nothing
                                Dim Is_Get_Item As String = Nothing
                                Dim Is_Mandatory As String = Nothing

                                Dim isValidRow As Boolean = True

                                Item_Code = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, "", TempTbl.Rows(idx)(0).ToString().Trim())
                                UOM = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, "", TempTbl.Rows(idx)(1).ToString().Trim())
                                Is_Get_Item = IIf(TempTbl.Rows(idx)(2) Is DBNull.Value, "", TempTbl.Rows(idx)(2).ToString().Trim())
                                Is_Mandatory = IIf(TempTbl.Rows(idx)(3) Is DBNull.Value, "", TempTbl.Rows(idx)(3).ToString().Trim())



                                If Item_Code.Trim() = "" Or Item_Code Is Nothing Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Item Code is mandatory " + Item_Code + " ,"
                                    isValidRow = False
                                    TotFailed += 1
                                End If
                                If objProduct.IsProductCodeExists(Item_Code, lblOrgID.Text) = False Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + " Item Code does not exists " + ","
                                    isValidRow = False
                                    TotFailed += 1
                                End If

                                If UOM.Trim() = "" Or UOM Is Nothing Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "UOM is mandatory " + UOM + " ,"
                                    isValidRow = False
                                    TotFailed += 1
                                Else

                                    If objProduct.IsValidUOM(UOM) = False Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Invalid  UOM: " + UOM + " ,"
                                        isValidRow = False
                                        TotFailed += 1
                                    End If
                                End If

                                If Is_Get_Item.Trim() = "" Or Is_Get_Item Is Nothing Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Is_Get_Item  is mandatory " + Is_Get_Item + " ,"
                                    isValidRow = False
                                    TotFailed += 1
                                ElseIf Is_Get_Item.Trim().ToUpper() <> "N" And Is_Get_Item.Trim().ToUpper() <> "Y" Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Invalid Is_Get_Item : " + Is_Get_Item + " ,"
                                    isValidRow = False
                                    TotFailed += 1
                                End If

                                If Is_Get_Item.Trim().ToUpper() = "N" Then
                                    If Is_Mandatory.Trim() = "" Or Is_Mandatory Is Nothing Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Is_Mandatory  is mandatory for Order item" + Is_Get_Item + " ,"
                                        isValidRow = False
                                        TotFailed += 1
                                    ElseIf Is_Mandatory.Trim().ToUpper() <> "N" And Is_Mandatory.Trim().ToUpper() <> "Y" Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Invalid Is_Mandatory : " + Is_Mandatory + " ,"
                                        isValidRow = False
                                        TotFailed += 1
                                    End If

                                   
                                End If

                                If Is_Get_Item.Trim().ToUpper() = "Y" Then
                                    Is_Mandatory = "N"
                                End If


                                If objProduct.CheckAssortmentItemNew(Err_No, Err_Desc, Me.lblOrgID.Text, Me.lblPlanId.Text, Item_Code, Me.lblTransType.Text) = True Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "The selected item already exist in the another plan : " + Item_Code + " ,"
                                    isValidRow = False
                                    TotFailed += 1
                                End If


                                For Each r As DataRow In dtOrdItems.Rows
                                    If r("ItemCode").ToString() = Item_Code Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "The selected item already exist: " + Item_Code + " ,"
                                        isValidRow = False
                                        TotFailed += 1

                                    End If
                                Next


                                Dim dt As New DataTable
                                dt.Columns.Add("Response")
                                Dim ds As New DataSet
                                ds = objProduct.CheckAssortmentBonusDataValidity(Err_No, Err_Desc, Item_Code, lblPlanId.Text)
                                Dim dr As DataRow
                                If ds.Tables.Count > 0 Then




                                    For Each seldr As DataRow In ds.Tables(0).Rows
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Plan: " & seldr("PlanName").ToString & ", Item Code: " & seldr("Item_Code").ToString & ", From date: " & seldr("Valid_From").ToString & ", To Date: " & seldr("Valid_To").ToString + " ,"
                                        isValidRow = False
                                        TotFailed += 1
                                    Next


                                    If ds.Tables.Count > 1 Then
                                        For Each seldr As DataRow In ds.Tables(1).Rows
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "Plan: " & seldr("PlanName").ToString & ", Customer No: " & seldr("Customer_No").ToString & ", Customer Name: " & seldr("Customer_Name").ToString & ", Item Code:" & seldr("Item_Code").ToString & ", From date: " & seldr("Valid_From").ToString & ", To Date: " & seldr("Valid_To").ToString + " ,"
                                            isValidRow = False
                                            TotFailed += 1
                                        Next

                                    End If

                                    If ds.Tables.Count > 2 Then

                                        For Each seldr As DataRow In ds.Tables(2).Rows
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "Plan: " & seldr("PlanName").ToString & ", Category:" & seldr("Category").ToString & ", Item Code: " & seldr("Item_Code").ToString & ", From date: " & seldr("Valid_From").ToString & ", To Date: " & seldr("Valid_To").ToString
                                            isValidRow = False
                                            TotFailed += 1
                                        Next

                                    End If
                                End If






                                If TempTbl_Slab.Columns.Count >= 4 Then
                                    If Not (TempTbl_Slab.Columns(0).ColumnName.ToLower = "from_value" And TempTbl_Slab.Columns(1).ColumnName.ToLower = "to_value" And TempTbl_Slab.Columns(2).ColumnName.ToLower = "type" And TempTbl_Slab.Columns(3).ColumnName.ToLower = "bonus_qty") Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Please check the template columns are correct in slab  ,"
                                        isValidRow = False
                                        TotFailed += 1
                                    End If
                                Else
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Invalid Slab Template"
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

                                    TotSuccess = TotSuccess + 1

                                    If Is_Get_Item.Trim().ToUpper() = "N" Then
                                        Dim dr_Ord As DataRow = dtOrdItems.NewRow()
                                        dr_Ord("ItemCode") = Item_Code
                                        dr_Ord("Description") = (New SalesWorx.BO.Common.Product).GetItemNameFromCode(Err_No, Err_Desc, Item_Code, lblOrgID.Text)
                                        dr_Ord("UOM") = UOM
                                        dr_Ord("GetItem") = Is_Get_Item
                                        dr_Ord("IsMandatory") = Is_Mandatory
                                        dtOrdItems.Rows.Add(dr_Ord)
                                    ElseIf Is_Get_Item.Trim().ToUpper() = "Y" Then
                                        Dim dr_Get As DataRow = dtGetItems.NewRow()
                                        dr_Get("ItemCode") = Item_Code
                                        dr_Get("Description") = (New SalesWorx.BO.Common.Product).GetItemNameFromCode(Err_No, Err_Desc, Item_Code, lblOrgID.Text)
                                        dr_Get("UOM") = UOM
                                        dr_Get("GetItem") = Is_Get_Item
                                        dr_Get("IsMandatory") = Is_Mandatory
                                        dtGetItems.Rows.Add(dr_Get)
                                    End If


                                    
                                End If

                            Next

                            If TempTbl_Slab.Rows.Count > 0 Then
                                Dim idx_Slab As Integer
                                For idx_Slab = 0 To TempTbl_Slab.Rows.Count - 1


                                    Dim From_Qty As String = Nothing
                                    Dim To_Qty As String = Nothing
                                    Dim Type As String = Nothing
                                    Dim Bonus_Qty As String = Nothing

                                    Dim isValidRow_Slab As Boolean = True

                                    From_Qty = IIf(TempTbl_Slab.Rows(idx_Slab)(0) Is DBNull.Value, "", TempTbl_Slab.Rows(idx_Slab)(0).ToString().Trim())
                                    To_Qty = IIf(TempTbl_Slab.Rows(idx_Slab)(1) Is DBNull.Value, "", TempTbl_Slab.Rows(idx_Slab)(1).ToString().Trim())
                                    Type = IIf(TempTbl_Slab.Rows(idx_Slab)(2) Is DBNull.Value, "", TempTbl_Slab.Rows(idx_Slab)(2).ToString().Trim())
                                    Bonus_Qty = IIf(TempTbl_Slab.Rows(idx_Slab)(3) Is DBNull.Value, "", TempTbl_Slab.Rows(idx_Slab)(3).ToString().Trim())

                                    If From_Qty.Trim() = "" Or From_Qty Is Nothing Then
                                        RowNo_Slab = idx_Slab + 2
                                        ErrorText_Slab = ErrorText_Slab + "From Qty is mandatory " + From_Qty + " ,"
                                        isValidRow_Slab = False
                                        TotFailed_Slab += 1
                                    End If

                                    If To_Qty.Trim() = "" Or To_Qty Is Nothing Then
                                        RowNo_Slab = idx_Slab + 2
                                        ErrorText_Slab = ErrorText_Slab + "To Qty is mandatory " + To_Qty + " ,"
                                        isValidRow_Slab = False
                                        TotFailed_Slab += 1
                                    End If
                                    If Type.Trim() = "" Or Type Is Nothing Then
                                        RowNo_Slab = idx_Slab + 2
                                        ErrorText_Slab = ErrorText_Slab + "Type is mandatory " + Type + " ,"
                                        isValidRow_Slab = False
                                        TotFailed += 1
                                    End If
                                    If Bonus_Qty.Trim() = "" Or Bonus_Qty Is Nothing Then
                                        RowNo_Slab = idx_Slab + 2
                                        ErrorText_Slab = ErrorText_Slab + "Bonus_Qty is mandatory " + Bonus_Qty + " ,"
                                        isValidRow_Slab = False
                                        TotFailed_Slab += 1
                                    End If

                                    If Not ValidateQty(Val(From_Qty)) Then
                                        RowNo_Slab = idx_Slab + 2
                                        ErrorText_Slab = ErrorText_Slab + "From Qty should be integer : " + From_Qty + " ,"
                                        isValidRow_Slab = False
                                        TotFailed_Slab += 1
                                    End If

                                    If Not ValidateQty(Val(To_Qty)) Then
                                        RowNo_Slab = idx_Slab + 2
                                        ErrorText_Slab = ErrorText_Slab + "To Qty should be integer : " + To_Qty + " ,"
                                        isValidRow_Slab = False
                                        TotFailed_Slab += 1
                                    End If
                                    If Not ValidateQty(Val(Bonus_Qty)) Then
                                        RowNo_Slab = idx_Slab + 2
                                        ErrorText_Slab = ErrorText_Slab + "Bonus Qty should be integer : " + Bonus_Qty + " ,"
                                        isValidRow_Slab = False
                                        TotFailed_Slab += 1
                                    End If


                                    If ValidateQty(Val(From_Qty)) And ValidateQty(Val(To_Qty)) Then
                                        If CDec(Val(To_Qty)) < CDec(Val(From_Qty)) Then
                                            RowNo_Slab = idx_Slab + 2
                                            ErrorText_Slab = ErrorText_Slab + "To quantity should be greater than from quantity ,"
                                            isValidRow_Slab = False
                                            TotFailed_Slab += 1
                                        End If

                                    End If


                                    If Type.Trim().ToUpper() <> "POINT" And Type.Trim().ToUpper() <> "RECURRING" Then
                                        RowNo_Slab = idx + 2
                                        ErrorText_Slab = ErrorText + "Invalid Type : " + Type + " ,"
                                        isValidRow_Slab = False
                                        TotFailed_Slab += 1
                                    End If

                                    If ValidateQty(Val(From_Qty)) And ValidateQty(Val(To_Qty)) And ValidateQty(Val(Bonus_Qty)) Then
                                        For Each r As DataRow In dtSlabs.Rows
                                            If ((CLng(r("OldFromQty").ToString()) >= Val(From_Qty) And CLng(r("OldFromQty").ToString()) <= Val(To_Qty) Or (Val(From_Qty) >= CLng(r("OldFromQty").ToString()) And Val(From_Qty) <= CLng(r("OldToQty").ToString()))) Or (((CLng(r("OldToQty").ToString())) >= Val(From_Qty) And CLng(r("OldToQty").ToString()) <= Val(To_Qty)) Or (Val(To_Qty) >= CLng(r("OldFromQty").ToString())) And Val(To_Qty) <= CLng(r("OldToQty").ToString()))) Then
                                                RowNo_Slab = idx_Slab + 2
                                                ErrorText_Slab = ErrorText_Slab + "This rule already exist.please define a new rule ,"
                                                isValidRow_Slab = False
                                                TotFailed_Slab += 1
                                            End If
                                        Next

                                    End If




                                    If Not (RowNo_Slab Is Nothing And ErrorText_Slab Is Nothing) Then

                                        Dim h As DataRow = dtErrors_slab.NewRow()
                                        h("RowNo") = RowNo_Slab
                                        h("LogInfo") = ErrorText_Slab
                                        dtErrors_slab.Rows.Add(h)
                                        RowNo_Slab = Nothing
                                        ErrorText_Slab = Nothing
                                        isValidRow_Slab = True
                                    Else

                                        TotSuccess_Slab = TotSuccess_Slab + 1
                                        Dim N As DataRow = dtSlabs.NewRow
                                        N("FromQty") = Val(From_Qty)
                                        N("toQty") = Val(To_Qty)
                                        N("OldFromQty") = Val(From_Qty)
                                        N("OldtoQty") = Val(To_Qty)
                                        N("TypeCode") = Type.Trim().ToUpper()
                                        N("GetQty") = Val(Bonus_Qty)
                                        N("SlabID") = "0"
                                        dtSlabs.Rows.Add(N)
                                    End If


                                Next
                            End If
                        End If

                       


                        ''If dtOrdItems.Rows.Count <= 0 Then
                        ''    MessageBoxValidation("Please add atleast one order item", "Information")
                        ''    Exit Sub

                        ''    ' Me.lblUpMsg.Text = "Please check the template columns are correct"
                        ''    ' Me.MPEImport.VisibleOnPageLoad = True
                        ''End If

                        ''If dtGetItems.Rows.Count <= 0 Then
                        ''    MessageBoxValidation("Please add atleast one bonus item", "Information")
                        ''    Exit Sub
                        ''End If

                        ''If dtSlabs.Rows.Count <= 0 Then
                        ''    MessageBoxValidation("Please add atleast one bonus rule", "Information")
                        ''    Exit Sub
                        ''End If

                        If dtOrdItems.Rows.Count > 0 And dtGetItems.Rows.Count > 0 And dtSlabs.Rows.Count > 0 Then
                            If objProduct.SaveAssortment(Err_No, Err_Desc, CInt(lblPlanId.Text), dtOrdItems, dtGetItems, dtSlabs, CType(Session.Item("USER_ACCESS"), UserAccess).UserID) Then
                                lblUpMsg.Text = IIf((TotFailed = 0 And TotFailed_Slab = 0), "Successfully uploaded", "One or more rows has invalid data. Please check the log")
                                Me.MPEImport.VisibleOnPageLoad = True

                                '   Response.Redirect("AdminBonusAssortment.aspx?OID=" & Me.lblOrgID.Text, False)
                            Else
                                MessageBoxValidation("Error while saving record", "Information")
                                Exit Sub
                            End If

                        End If


                      


                        ' If TotSuccess > 0 Then

                        DeleteExcel()
                        lblUpMsg.Text = IIf((TotFailed = 0 And TotFailed_Slab = 0), "Successfully uploaded", "One or more rows has invalid data. Please check the log")
                        Me.MPEImport.VisibleOnPageLoad = True
                        BindDistribution_ctl()
                        'End If
                    End If

                    Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & "DisCtlLog_" & Now.ToString("yyyyMMdd") + ".txt"

                    dtErrors.TableName = "Item"
                    dtErrors_slab.TableName = "Slab"


                    Dim Ds_error As New DataSet
                    Ds_error.Tables.Add(dtErrors.Copy())
                    Ds_error.Tables.Add(dtErrors_slab.Copy())
                    DataTable2CSV_1(Ds_error, fn, vbTab)

                    'dgvErros.Visible = False
                    'If dtErrors.Rows.Count > 0 Then
                    '    lbLog.Visible = True
                    'Else
                    '    lbLog.Visible = False
                    'End If
                    'Me.dgvErros.DataSource = dtErrors
                    'Me.dgvErros.DataBind()
                    'Session.Remove("dtDisCtl")
                    'Session("dtDisCtl") = dtErrors.Copy


                    'Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & "DisCtlLog_" & Now.ToString("yyyyMMdd") + ".txt"
                    'DataTable2CSV(dtErrors, fn, vbTab)
                    If dtErrors.Rows.Count > 0 Or dtErrors_slab.Rows.Count > 0 Then
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
                    'Response.End()
                    '

                    'Response.AddHeader("Content-Disposition", "attachment; filename=" & file.Name)

                    'Response.AddHeader("Content-Length", file.Length.ToString())

                    'Response.WriteFile(file.FullName)


                    'Response.[End]()
                Else
                    lblUpMsg.Text = "File does not exist"
                    'lblMessage.ForeColor = Drawing.Color.Green
                    'lblinfo.Text = "Information"
                    MPEImport.VisibleOnPageLoad = True
                    Exit Sub

                End If

            Else
                lblUpMsg.Text = "There is no log to show."
                'lblMessage.ForeColor = Drawing.Color.Green
                'lblinfo.Text = "Information"
                MPEImport.VisibleOnPageLoad = True
                Exit Sub

            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))

        Finally
        End Try
    End Sub
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

                Dim cmd As New OleDbCommand("SELECT * FROM [Items$]", oledbConn)

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

                Dim cmd As New OleDbCommand("SELECT * FROM [Items$]", oledbConn)

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

    Private Function DoCSVUpload_Slab() As DataTable
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

    Private Function DoXLSUpload_Slab() As DataTable
        Dim dtImport As New DataTable
        Try
            Dim connString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & ViewState("FileName") & " ;Extended Properties=""Excel 8.0;HDR=Yes;IMEX=1"""
            Dim oledbConn As New OleDbConnection(connString)
            Try
                oledbConn.Open()

                Dim cmd As New OleDbCommand("SELECT * FROM [Slab$]", oledbConn)

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
    Private Function DoXLSXUpload_Slab() As DataTable
        Dim dtImport As New DataTable
        Try
            Dim connString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & ViewState("FileName") & " ;Extended Properties=""Excel 12.0;HDR=Yes;IMEX=1"""
            Dim oledbConn As New OleDbConnection(connString)
            Try
                oledbConn.Open()

                Dim cmd As New OleDbCommand("SELECT * FROM [Slab$]", oledbConn)

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
    'Sub LoadOrgHeads()
    '    Try


    '        Dim objCommon As New Common
    '        Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
    '        Dim dt_org As New DataTable
    '        dt_org = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
    '        ddl_FilterOrg.DataSource = dt_org
    '        ddl_FilterOrg.Items.Clear()
    '        ddl_FilterOrg.Items.Add(New RadComboBoxItem("-- Select a Organization --", "0"))
    '        ddl_FilterOrg.AppendDataBoundItems = True
    '        ddl_FilterOrg.DataValueField = "MAS_Org_ID"
    '        ddl_FilterOrg.DataTextField = "Description"
    '        ddl_FilterOrg.DataBind()


    '        ddl_OrgAdd.DataSource = dt_org
    '        ddl_OrgAdd.Items.Clear()
    '        ddl_OrgAdd.Items.Add(New RadComboBoxItem("-- Select a Organization --", "0"))
    '        ddl_OrgAdd.AppendDataBoundItems = True
    '        ddl_OrgAdd.DataValueField = "MAS_Org_ID"
    '        ddl_OrgAdd.DataTextField = "Description"
    '        ddl_OrgAdd.DataBind()

    '    Catch ex As Exception
    '        log.Error(GetExceptionInfo(ex))
    '        log.Error(ex.Message.ToString())
    '    End Try


    'End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click




        If ddl_OrgAdd.SelectedIndex <= 0 Then
            lblmsgPopUp.Text = "Please Select a Organization."
            MPEAdd.VisibleOnPageLoad = True
            Return
        End If

        If ddl_VanAdd.SelectedIndex <= 0 Then
            lblmsgPopUp.Text = "Please Select a Van."
            MPEAdd.VisibleOnPageLoad = True
            Return
        End If




        Dim success As Boolean = False
        Dim customer As String
        Dim cid As String = "0"
        Dim Site As String = "0"
        Dim Is_Optional As String = "Y"
        Try


            customer = ddl_Customer.SelectedValue

            If chkIsOptional.Checked Then
                Is_Optional = "Y"
            Else
                Is_Optional = "N"
            End If

            If Not String.IsNullOrEmpty(ddl_Customer.SelectedValue) Then
                Dim IDs() As String
                IDs = customer.Split("$")
                cid = IDs(0)
                Site = IDs(1)
            End If


            Dim dt_rslt As New DataTable
            Dim rslt As String = ""



            If (objCustomer.SaveDistribution_CTL(Err_No, Err_Desc, ddl_OrgAdd.SelectedItem.Value, ddl_VanAdd.SelectedValue, cid, Site, Is_Optional, CType(Session("User_Access"), UserAccess).UserID)) = True Then
                success = True
                MessageBoxValidation("Successfully Saved.", "Information")
                BindDistribution_ctl()
                MPEAdd.VisibleOnPageLoad = False
                Panel.Update()
            Else
                MessageBoxValidation("Could not be saved.", "Information")
                BindDistribution_ctl()
                MPEAdd.VisibleOnPageLoad = False
                Panel.Update()
                log.Error(Err_Desc)

            End If


            If success = True Then

                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "Distribution Check CTL", "Distribution Check CTL", Me.ddl_OrgAdd.SelectedValue.ToString(), "Customer: " & Me.ddl_Customer.Text & " Van: " & Me.ddl_VanAdd.Text & "Is_Optional :  " & Is_Optional, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")

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

            Dim RowID As String
            RowID = CType(row.FindControl("lblDistribution_CTL_ID"), Label).Text

            If objCustomer.DeleteDistribution_CTL(Err_No, Err_Desc, ddl_FilterOrg.SelectedItem.Value, RowID) = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "D", "Distribution Check CTL", "Distribution Check CTL", Me.ddl_OrgAdd.SelectedValue.ToString(), "Customer: " & Me.ddl_Customer.Text & " Van: " & Me.ddl_VanAdd.Text & "Distribution_CTL_ID :  " & RowID, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")

                success = True
            End If

            If success = True Then
                MessageBoxValidation("Successfully deleted.", "Information")
                BindDistribution_ctl()
            Else
                MessageBoxValidation("Error occured while deleting Distribution Check Control.", "Information")
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

                    Dim RowID As String
                    RowID = CType(row.FindControl("lblDistribution_CTL_ID"), Label).Text

                    If objCustomer.DeleteDistribution_CTL(Err_No, Err_Desc, ddl_FilterOrg.SelectedItem.Value, RowID) = True Then
                        objLogin.SaveUserLog(Err_No, Err_Desc, "D", "Distribution Check CTL", "Distribution Check CTL", Me.ddl_OrgAdd.SelectedValue.ToString(), "Customer: " & Me.ddl_Customer.Text & " Van: " & Me.ddl_VanAdd.Text & "Distribution_CTL_ID :  " & RowID, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                        Success = True
                    End If


                End If
            Next
            If Success = True Then
                MessageBoxValidation("Successfully deleted.", "Information")
                BindDistribution_ctl()
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

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try

            log.Error("1")
            lblmsgPopUp.Text = ""
            lblUpMsg.Text = ""
            btnUpdate.Visible = True
            btnSave.Visible = False

            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)


            Dim Org_ID As String
            Org_ID = CType(row.FindControl("lblMAS_Org_ID"), Label).Text
            log.Error("Org_ID=" & Org_ID)

            Dim SID As String
            SID = CType(row.FindControl("lblSalesRep_ID"), Label).Text

            log.Error("SID=" & SID)

            Dim CustomerID As String
            CustomerID = CType(row.FindControl("lblCustomer"), Label).Text

            log.Error("CustomerID=" & CustomerID)

            Dim SiteID As String
            SiteID = CType(row.FindControl("LblSite_Use_ID"), Label).Text

            log.Error("SiteID=" & SiteID)

            Dim custid As String
            custid = CustomerID & "$" & SiteID

            Dim Objrep As New SalesWorx.BO.Common.Reports()
            Dim dt1 As New DataTable

            ' log.Error(ddl_FilterOrg.SelectedValue & "," & ddl_FilterVan.SelectedValue & "," & CustomerID)
            ' dt1 = objCustomer.GetCustfromOrgtext_Distribution_ctl(Err_No, Err_Desc, ddl_FilterOrg.SelectedValue, ddl_FilterVan.SelectedValue, CustomerID)

            log.Error("2")

            If CustomerID.Trim() <> "0" And SiteID.Trim() <> "0" Then
                'Loop through the values to populate the combo box
                For i As Integer = 0 To dt1.Rows.Count - 1
                    Dim item As New RadComboBoxItem()
                    item.Text = dt1.Rows(i).Item("Customer").ToString
                    item.Value = dt1.Rows(i).Item("CustomerID").ToString

                    ddl_Customer.Items.Add(item)
                    item.DataBind()

                    log.Error("3")

                Next
                If Not ddl_Customer.FindItemByValue(custid) Is Nothing Then
                    ddl_Customer.ClearSelection()
                    ddl_Customer.FindItemByValue(custid).Selected = True

                End If
            End If

            log.Error("4")



            'If Not ddl_Customer.FindItemByValue(custid) Is Nothing Then
            '    ddl_Customer.ClearSelection()
            '    ddl_Customer.FindItemByValue(custid).Selected = True

            'End If

            log.Error("5")


            If Not ddl_OrgAdd.FindItemByValue(Org_ID) Is Nothing Then
                ddl_OrgAdd.ClearSelection()
                ddl_OrgAdd.FindItemByValue(Org_ID).Selected = True
            End If

            log.Error("6")
            LoadVan_Add()

            log.Error("7")
            If Not ddl_VanAdd.FindItemByValue(SID) Is Nothing Then
                ddl_VanAdd.ClearSelection()
                ddl_VanAdd.FindItemByValue(SID).Selected = True
            End If

            Dim Is_Optional As String
            Is_Optional = CType(row.FindControl("lblIs_Optional"), Label).Text

            If Is_Optional.Trim().ToUpper() = "Y" Then
                chkIsOptional.Checked = True
            Else
                chkIsOptional.Checked = False
            End If

            log.Error("8")

            ddl_OrgAdd.Enabled = False
            ddl_VanAdd.Enabled = False
            ddl_Customer.Enabled = False
            MPEAdd.VisibleOnPageLoad = True
            log.Error("9")

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

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs)
        MPEAdd.VisibleOnPageLoad = False
        Panel.Update()
        Return
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs)

        Dim success As Boolean = False
        Dim Is_Optional As String = "Y"
        Try

            Dim cid As String = "0"
            Dim Site As String = "0"
            Dim PID As String = "0"

            Dim customer As String
            customer = ddl_Customer.SelectedValue
            If Not String.IsNullOrEmpty(ddl_Customer.SelectedValue) Then
                Dim IDs() As String
                IDs = customer.Split("$")
                cid = IDs(0)
                Site = IDs(1)
            End If


            If chkIsOptional.Checked Then
                Is_Optional = "Y"
            Else
                Is_Optional = "N"
            End If


            Dim dt_rslt As New DataTable
            Dim rslt As String = ""


            If (objCustomer.SaveDistribution_CTL(Err_No, Err_Desc, ddl_OrgAdd.SelectedItem.Value, ddl_VanAdd.SelectedValue, cid, Site, Is_Optional, CType(Session("User_Access"), UserAccess).UserID)) = True Then
                success = True
                MessageBoxValidation("Successfully Updated.", "Information")
                BindDistribution_ctl()
                MPEAdd.VisibleOnPageLoad = False
                Panel.Update()
            Else
                MessageBoxValidation("Could not be Updated.", "Information")
                BindDistribution_ctl()
                MPEAdd.VisibleOnPageLoad = False
                Panel.Update()
                log.Error(Err_Desc)
            End If

            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "U", "Distribution Check CTL", "Distribution Check CTL", Me.ddl_OrgAdd.SelectedValue.ToString(), "Customer: " & Me.ddl_Customer.Text & " Van: " & Me.ddl_VanAdd.Text & "Is_Optional :  " & Is_Optional, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
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
    End Sub

    Private Sub dgv_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgvItems.Sorting
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"

        BindDistribution_ctl()
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
    Private Sub dgv_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgvItems.PageIndexChanging
        dgvItems.PageIndex = e.NewPageIndex
        BindDistribution_ctl()
    End Sub

    Private Sub BindDistribution_ctl()

        ''Dim dtData As New DataTable
        ''Dim Customer As String
        ''Dim ProdVal As String = ""
        ''Dim CustomerID As String = ""
        ''Dim SiteID As String = ""
        ''Dim SID As String = ""

        ''Dim IDs() As String
        ' ''If ddl_FilterCustomer.SelectedValue <> "" Then
        ' ''    Customer = ddl_FilterCustomer.SelectedValue
        ' ''    IDs = Customer.Split("$")
        ' ''    CustomerID = IDs(0)
        ' ''    SiteID = IDs(1)
        ' ''Else
        ' ''    CustomerID = 0
        ' ''    SiteID = 0
        ' ''End If


        ' ''If ddl_FilterVan.SelectedIndex > 0 Then
        ' ''    SID = ddl_FilterVan.SelectedValue
        ' ''Else
        ' ''    SID = "0"
        ' ''End If

        ''dtData = objCustomer.GetDistribution_ctl(Err_No, Err_Desc, ddl_FilterOrg.SelectedItem.Value, SID, CustomerID, SiteID)
        ''Dim dv As New DataView(dtData)
        ''If ViewState("SortField") <> "" Then
        ''    dv.Sort = (ViewState("SortField") & " ") + SortDirection
        ''End If

        ''Me.dgvItems.DataSource = Nothing
        ''Me.dgvItems.DataSource = dv
        ''Me.dgvItems.DataBind()
        ''PnlGridData.Visible = True

    End Sub







    'Private Sub ddl_FilterCustomer_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddl_FilterCustomer.ItemsRequested
    '    Try

    '        Dim dt As New DataTable
    '        dt = objCustomer.GetCustfromOrgtext_Distribution_ctl(Err_No, Err_Desc, ddl_FilterOrg.SelectedValue, ddl_FilterVan.SelectedValue, e.Text)
    '        Dim ItemsPerRequest As Integer = 100 'Set the number of values to show
    '        Dim itemOffset As Integer = e.NumberOfItems
    '        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
    '        e.EndOfItems = endOffset = dt.Rows.Count
    '        'Loop through the values to populate the combo box
    '        For i As Integer = itemOffset To endOffset - 1
    '            Dim item As New RadComboBoxItem()
    '            item.Text = dt.Rows(i).Item("Customer").ToString
    '            item.Value = dt.Rows(i).Item("CustomerID").ToString

    '            ddl_FilterCustomer.Items.Add(item)
    '            item.DataBind()
    '        Next

    '    Catch ex As Exception
    '        log.Error(ex.Message)
    '    End Try
    'End Sub

    'Private Sub ddl_Customer_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddl_FilterCustomer.SelectedIndexChanged
    '    BindDistribution_ctl()
    '    Panel.Update()
    'End Sub





    'Private Sub btn_clearFilter_Click(sender As Object, e As EventArgs) Handles btn_clearFilter.Click
    '    ddl_FilterOrg.ClearSelection()
    '    ' ddl_FilterCustomer.ClearSelection()
    '    'ddl_FilterVan.ClearSelection()
    '    '  ddl_FilterCustomer.Items.Clear()
    '    ' ddl_FilterCustomer.Text = ""
    '    'ddl_FilterVan.Items.Clear()

    '    dgvItems.DataSource = Nothing
    '    dgvItems.DataBind()

    '    Panel.Update()


    'End Sub

    Private Sub ddl_Customer_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddl_Customer.ItemsRequested
        Try

            Dim Objrep As New SalesWorx.BO.Common.Reports()
            Dim dt As New DataTable


            dt = objCustomer.GetCustfromOrgtext_Distribution_ctl(Err_No, Err_Desc, ddl_OrgAdd.SelectedValue, ddl_VanAdd.SelectedValue, e.Text)


            Dim ItemsPerRequest As Integer = 100 'Set the number of values to show
            Dim itemOffset As Integer = e.NumberOfItems
            Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
            e.EndOfItems = endOffset = dt.Rows.Count

            'Loop through the values to populate the combo box
            For i As Integer = itemOffset To endOffset - 1
                Dim item As New RadComboBoxItem()
                item.Text = dt.Rows(i).Item("Customer").ToString
                item.Value = dt.Rows(i).Item("CustomerID").ToString

                ddl_Customer.Items.Add(item)
                item.DataBind()
            Next

        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub



    Sub LoadVan()
        Try
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            objcommon = New SalesWorx.BO.Common.Common()
            'ddl_FilterVan.DataSource = objcommon.GetVanByOrg(Err_No, Err_Desc, ddl_FilterOrg.SelectedValue, objUserAccess.UserID)
            'ddl_FilterVan.DataBind()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Sub LoadVan_Add()
        Try
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            objcommon = New SalesWorx.BO.Common.Common()
            ddl_VanAdd.DataSource = objcommon.GetVanByOrg(Err_No, Err_Desc, ddl_OrgAdd.SelectedValue, objUserAccess.UserID)
            ddl_VanAdd.DataBind()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    'Private Sub ddl_FilterVan_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddl_FilterVan.SelectedIndexChanged
    '    Try
    '        If ddl_FilterVan.SelectedIndex > 0 Then
    '            ddl_FilterCustomer.ClearSelection()
    '            ddl_FilterCustomer.Items.Clear()
    '            ddl_FilterCustomer.Text = ""

    '            BindDistribution_ctl()
    '            Panel.Update()
    '        End If
    '    Catch ex As Exception
    '        log.Error(GetExceptionInfo(ex))
    '        log.Error(ex.Message.ToString())
    '    End Try
    'End Sub

    Private Sub ddl_OrgAdd_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddl_OrgAdd.SelectedIndexChanged
        Try
            If ddl_OrgAdd.SelectedItem.Value = 0 Then
                ddl_Customer.ClearSelection()
                ddl_Customer.Items.Clear()
                ddl_Customer.Text = ""

                ddl_VanAdd.ClearSelection()
                ddl_VanAdd.Items.Clear()
                ddl_VanAdd.Text = ""
            End If
            LoadVan_Add()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub


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

    Private Sub btndownloadTemp_Click(sender As Object, e As EventArgs) Handles btndownloadTemp.Click
        Dim Filename As String = System.Configuration.ConfigurationManager.AppSettings("ExcelTemplatePath") & "Distribution_CTL.xlsx"
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


    Function ValidateQty(ByVal Qty As String)
        Dim bRetVal As Boolean
        Try
            Dim num As Integer
            If Integer.TryParse(Val(Qty), num) Then
                bRetVal = True
            Else
                bRetVal = False
            End If
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
        Return bRetVal
    End Function

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

            Session.Remove("dtSlabs")
            Session("dtSlabs") = dtSlabs
        End If

    End Sub
End Class





