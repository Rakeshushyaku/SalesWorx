Imports SalesWorx.BO.Common
Imports Telerik.Web.UI
Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports ExcelLibrary.SpreadSheet
Public Class Admin_VATRule
    Inherits System.Web.UI.Page

    Dim objcommon As New SalesWorx.BO.Common.Common
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim objProduct As New SalesWorx.BO.Common.Product
    Dim objDivConfig As New DivConfig
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const PageID As String = "P334"
    Private dt As New DataTable
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objControl As New AppControl
    Protected Sub ddl_org_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Org.SelectedIndexChanged

        If ddl_Org.SelectedItem.Value = 0 Then
            ddl_FilterCustomer.ClearSelection()
            ddl_FilterCustomer.Items.Clear()
            ddl_FilterCustomer.Text = ""

            ddl_FilterProduct.ClearSelection()
            ddl_FilterProduct.Items.Clear()
            ddl_FilterProduct.Text = ""
        End If

        Loadcustomer()
        LoadProducts()
        BindVATRule()
        Panel.Update()
    End Sub
    Sub Loadcustomer()
        '' ''Dim dt As New DataTable
        '' ''dt = objcommon.GetCustomerfromOrg(Err_No, Err_Desc, ddl_Org.SelectedItem.Value)
        '' ''Dim dr As DataRow
        '' ''dr = dt.NewRow
        '' ''dr("CustomerID") = "0$0"
        '' ''dr("Customer") = "--Select Customer--"
        '' ''dt.Rows.InsertAt(dr, 0)

        '' ''ddl_FilterCustomer.DataSource = dt
        '' ''ddl_FilterCustomer.DataTextField = "Customer"
        '' ''ddl_FilterCustomer.DataValueField = "CustomerID"
        '' ''ddl_FilterCustomer.DataBind()


        '' ''ddl_Customer.DataSource = dt
        '' ''ddl_Customer.DataTextField = "Customer"
        '' ''ddl_Customer.DataValueField = "CustomerID"
        '' ''ddl_Customer.DataBind()

    End Sub
    Sub LoadProducts()
        '' ''Dim dt As New DataTable
        '' ''dt = objcommon.GetProductsByOrg(Err_No, Err_Desc, ddl_Org.SelectedItem.Value)
        '' ''Dim dr As DataRow
        '' ''dr = dt.NewRow
        '' ''dr("Inventory_Item_ID") = "0"
        '' ''dr("Description") = "--Select Product--"
        '' ''dt.Rows.InsertAt(dr, 0)

        '' ''ddl_FilterProduct.DataSource = dt
        '' ''ddl_FilterProduct.DataTextField = "Description"
        '' ''ddl_FilterProduct.DataValueField = "Inventory_Item_ID"
        '' ''ddl_FilterProduct.DataBind()


        '' ''ddl_Product.DataSource = dt
        '' ''ddl_Product.DataTextField = "Description"
        '' ''ddl_Product.DataValueField = "Inventory_Item_ID"
        '' ''ddl_Product.DataBind()
    End Sub

    Private Sub btnImportWindow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportWindow.Click
        If Me.ddl_Org.SelectedIndex > 0 Then
            Me.lblUpMsg.Text = ""
            Session("dtVatRules") = Nothing
            dgvErros.DataSource = Nothing
            dgvErros.DataBind()
            dgvErros.Visible = False
            lbLog.Visible = False
            Me.MPEImport.VisibleOnPageLoad = True
        Else
            MessageBoxValidation("Please select a organization", "Information")
            Exit Sub
        End If
    End Sub


    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try

            If Me.ddl_Org.SelectedIndex > 0 Then
                Dim dtOriginal As New DataTable()


                dtOriginal = objProduct.ExportVATRule(Err_No, Err_Desc, ddl_Org.SelectedItem.Value)

                Dim dtTemp As New DataTable()

                'Creating Header Row

                dtTemp.Columns.Add("CustomerNo")
                dtTemp.Columns.Add("CustomerName")
                dtTemp.Columns.Add("ItemCode")
                dtTemp.Columns.Add("ItemDescription")
                dtTemp.Columns.Add("Vat_Value")
                dtTemp.Columns.Add("Vat_Code")

                Dim drAddItem As DataRow
                For i As Integer = 0 To dtOriginal.Rows.Count - 1
                    drAddItem = dtTemp.NewRow()
                    drAddItem(0) = IIf(dtOriginal.Rows(i)("Customer_No") Is DBNull.Value, "All", dtOriginal.Rows(i)("Customer_No").ToString())
                    drAddItem(1) = IIf(dtOriginal.Rows(i)("Customer_Name") Is DBNull.Value, "All", dtOriginal.Rows(i)("Customer_Name").ToString())
                    drAddItem(2) = IIf(dtOriginal.Rows(i)("Item_No") Is DBNull.Value, "All", dtOriginal.Rows(i)("Item_No").ToString())
                    drAddItem(3) = IIf(dtOriginal.Rows(i)("Description") Is DBNull.Value, "All", dtOriginal.Rows(i)("Description").ToString()) 
                    drAddItem(4) = CLng(dtOriginal.Rows(i)("Vat_Value").ToString())
                    drAddItem(5) = dtOriginal.Rows(i)("Vat_Code").ToString()

                    dtTemp.Rows.Add(drAddItem)
                Next

                If dtOriginal.Rows.Count = 0 Then

                    MessageBoxValidation("There is no data.", "Information")
                    Exit Sub

                    drAddItem = dtTemp.NewRow()
                    drAddItem(0) = ""
                    drAddItem(1) = ""
                    drAddItem(2) = ""
                    drAddItem(3) = ""
                    drAddItem(4) = ""
                    drAddItem(5) = ""
                    
                    dtTemp.Rows.Add(drAddItem)
                End If

                'Temp(Grid)
                Dim dg As New DataGrid()
                dg.DataSource = dtTemp
                dg.DataBind()
                If dtTemp.Rows.Count > 0 Then
                    'Dim fn As String = "MSL" & Now.ToString("ddMMMyyHHmmss") + ".xls"
                    Dim fn As String = "VATRules" + Now.ToString("ddMMMyyHHmmss") + ".xls"
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




                LoadOrgHeads()
                Loadcustomer()
                LoadProducts()

                BindVATRule()

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
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Private Sub btnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImport.Click
        lbLog.Visible = False
        If Me.ExcelFileUpload.FileName = Nothing Then
            ' Me.lblinfo.Text = "Validation"
            Me.lblUpMsg.Text = "Select filename "
            ' Me.lblMessage.ForeColor = Drawing.Color.Green
            ' Me.MpInfoError.Show()
            Me.MPEImport.VisibleOnPageLoad = True
            Exit Sub
        End If


        Session("dtVatRules") = Nothing
        Dim dtErrors As New DataTable
        dtErrors = SetErrorsTable().Copy
        Dim Str As New StringBuilder

        Dim TotSuccess As Integer = 0
        Dim TotFailed As Integer = 0
        Try
            Dim VAT_TYPE As String = "V"
            Dim dt As DataTable
            dt = objDivConfig.GetDivisionalConfiguration(Err_No, Err_Desc, ddl_Org.SelectedItem.Value)
            If dt.Rows.Count > 0 Then
                VAT_TYPE = dt.Rows(0)("VAT_TYPE")
            End If

            ViewState("FileType") = Me.ExcelFileUpload.PostedFile.ContentType
            If ExcelFileUpload.FileName.ToString.EndsWith(".csv") Or ExcelFileUpload.FileName.ToString.EndsWith(".xls") Or ExcelFileUpload.FileName.ToString.EndsWith(".xlsx") Then

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
                        col.ColumnName = "OrgID"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)


                        col = New DataColumn
                        col.ColumnName = "CustomerNo"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)



                        col = New DataColumn
                        col.ColumnName = "Item_Code"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)


                        col = New DataColumn
                        col.ColumnName = "VAT_Value"
                        col.DataType = System.Type.GetType("System.Double")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)

                        col = New DataColumn
                        col.ColumnName = "Vat_Code"
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

                        If TempTbl.Columns.Count = 4 Then
                            If Not (TempTbl.Columns(0).ColumnName.ToLower = "customerno" And TempTbl.Columns(1).ColumnName.ToLower = "item_code" And TempTbl.Columns(2).ColumnName.ToLower = "vat_value" And TempTbl.Columns(3).ColumnName.ToLower = "vat_code") Then
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
                        ' Dim ColNo As String = Nothing
                        ' Dim ColumnName As String = Nothing
                        Dim ErrorText As String = Nothing

                        If TempTbl.Rows.Count > 0 Then
                            Dim idx As Integer

                            For idx = 0 To TempTbl.Rows.Count - 1
                                Dim CustomerNo As String = Nothing
                                Dim Item As String = Nothing
                                Dim Vat_value As String = "0"
                                Dim Vat_Code As String = ""

                                Dim isValidRow As Boolean = True

                                CustomerNo = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, "0", TempTbl.Rows(idx)(0).ToString())
                                Item = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, "0", TempTbl.Rows(idx)(1).ToString())
                                Vat_value = IIf(TempTbl.Rows(idx)(2) Is DBNull.Value, "0", TempTbl.Rows(idx)(2).ToString())
                                Vat_Code = IIf(TempTbl.Rows(idx)(3) Is DBNull.Value, "", TempTbl.Rows(idx)(3).ToString())
                                Dim CustomerID As String = "0"
                                Dim SiteID As String = "0"
                                Dim InventoryItemID As String = "0"
                                If CustomerNo <> "0" Then
                                    If objProduct.CheckCustomerNo(Err_No, Err_Desc, CustomerNo, ddl_Org.SelectedItem.Value) = False Then
                                        RowNo = idx + 2
                                        ' ColNo = ColNo + "3" + ","
                                        ' ColumnName = ColumnName + "BonusItem" + ","
                                        ErrorText = ErrorText + "Invalid Customer No: " + CustomerNo + " ,"
                                        isValidRow = False
                                        TotFailed += 1
                                    Else
                                        objProduct.GetCustomerID(Err_No, Err_Desc, ddl_Org.SelectedItem.Value, CustomerNo, CustomerID, SiteID)
                                    End If
                                End If
                                If Item <> "0" Then
                                    If objProduct.CheckItemCode(Err_No, Err_Desc, Item, ddl_Org.SelectedItem.Value) = False Then
                                        RowNo = idx + 2
                                        ' ColNo = ColNo + "2" + ","
                                        ' ColumnName = ColumnName + "OrderItem" + ","
                                        ErrorText = ErrorText + "Invalid Item code: " + Item + " ,"
                                        isValidRow = False
                                        TotFailed += 1
                                    Else
                                        InventoryItemID = objProduct.GetInventoryItemID(Err_No, Err_Desc, Item, ddl_Org.SelectedItem.Value)
                                    End If
                                End If


                                If IsNumeric(Vat_value) = False Or Val(Vat_value) >= 100 Or Val(Vat_value) < 0 Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Invalid VAT value: " + Vat_value + " ,"
                                    isValidRow = False
                                    TotFailed += 1
                                End If
                                If VAT_TYPE = "B" Then
                                    If String.IsNullOrEmpty(Vat_Code) Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Invalid VAT Code:" + Vat_Code + " ,"
                                        isValidRow = False
                                        TotFailed += 1
                                    End If
                                End If

                                If Not (RowNo Is Nothing And ErrorText Is Nothing) Then
                                    Dim h As DataRow = dtErrors.NewRow()
                                    h("RowNo") = RowNo
                                    ' h("ColNo") = ColNo
                                    ' h("ColName") = ColumnName
                                    h("LogInfo") = ErrorText
                                    dtErrors.Rows.Add(h)
                                    RowNo = Nothing
                                    'ColNo = Nothing
                                    'ColumnName = Nothing
                                    ErrorText = Nothing
                                    isValidRow = True
                                Else

                                    If objProduct.SaveVATRule(Err_No, Err_Desc, ddl_Org.SelectedItem.Value, CustomerID, SiteID, InventoryItemID, Vat_value, Vat_Code, CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
                                        TotSuccess = TotSuccess + 1
                                        Dim h As DataRow = dtErrors.NewRow()
                                        h("RowNo") = idx + 2
                                        h("LogInfo") = "Successfully uploaded"
                                        dtErrors.Rows.Add(h)
                                        RowNo = Nothing
                                        ErrorText = Nothing
                                        isValidRow = True
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


                        ' If TotSuccess > 0 Then

                        DeleteExcel()
                        lblUpMsg.Text = IIf(TotFailed = 0, "Successfully uploaded", "One or more rows has invalid data. Please check the log")
                        Me.MPEImport.VisibleOnPageLoad = True
                        BindVATRule()
                        'End If
                    End If

                    dgvErros.Visible = False
                    If dtErrors.Rows.Count > 0 Then
                        lbLog.Visible = True
                    Else
                        lbLog.Visible = False
                    End If
                    Me.dgvErros.DataSource = dtErrors
                    Me.dgvErros.DataBind()
                    Session.Remove("dtVatRules")
                    Session("dtVatRules") = dtErrors.Copy


                    Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & "VATLog_" & Now.ToString("yyyyMMdd") + ".txt"
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
                ' Str.Append("<span style='font-color:red'> <b >Please import valid Excel template.</b></span>")
                lblUpMsg.Text = "Please import valid Excel template."
                Me.MPEImport.VisibleOnPageLoad = True
            End If


        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try

    End Sub

    Public Function FormatDate(ByVal DateVal As String) As Date
        If (Not IsNothing(DateVal)) AndAlso IsValidInputDate(DateVal) Then
            'Dim TemFromDateStr As String = DateVal
            'Dim DateArr As Array = TemFromDateStr.Split("/")
            'If DateArr.Length = 3 Then
            '    TemFromDateStr = DateArr(1) & "/" & DateArr(0) & "/" & DateArr(2)
            'Else
            '    TemFromDateStr = "01/01/1900"
            'End If
            'Return CType(DateVal, Date).ToString("MM/dd/yyyy")
            Return CDate(DateVal)
        End If
    End Function
    Public Shared Function IsValidInputDate(ByVal str As String) As Boolean
        'Dim dt As DateTime
        'Dim success As Boolean = False
        'success = DateTime.TryParseExact(str, "dd/MM/yyyy", Nothing, Globalization.DateTimeStyles.None, dt)
        'If success = False Then
        '    success = DateTime.TryParseExact(str, "d/M/yyyy", Nothing, Globalization.DateTimeStyles.None, dt)
        'End If
        'Return success
        Return True
    End Function
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
    
   
    Sub LoadOrgHeads()
         

        Dim objCommon As New Common
        Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
        ddl_Org.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
        ddl_Org.Items.Clear()
        ddl_Org.Items.Add(New RadComboBoxItem("-- Select a Organization --", "0"))
        ddl_Org.AppendDataBoundItems = True
        ddl_Org.DataValueField = "MAS_Org_ID"
        ddl_Org.DataTextField = "Description"
        ddl_Org.DataBind()


      



    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If String.IsNullOrEmpty(txt_Value.Text) Then
            lblmsgPopUp.Text = "Please enter the VAT value."
            MPEAdd.VisibleOnPageLoad = True
            Return
        End If

        If IsNumeric(txt_Value.Text) = False Then
            lblmsgPopUp.Text = "VAT value should be in number."
            Me.MPEAdd.VisibleOnPageLoad = True
            Return
        End If



        Dim VAT_TYPE As String = "V"
        Dim dt As DataTable
        dt = objDivConfig.GetDivisionalConfiguration(Err_No, Err_Desc, ddl_Org.SelectedItem.Value)
        If dt.Rows.Count > 0 Then
            VAT_TYPE = dt.Rows(0)("VAT_TYPE").ToString
        End If

        If VAT_TYPE = "B" Then
            If String.IsNullOrEmpty(txt_code.Text) Then
                lblmsgPopUp.Text = "Please enter the VAT Code."
                MPEAdd.VisibleOnPageLoad = True
                Return
            End If
        End If

        Dim success As Boolean = False
        Try

            Dim customer As String
            customer = ddl_Customer.SelectedValue


            Dim cid As String = "0"
            Dim Site As String = "0"
            Dim PID As String = "0"

            If Not String.IsNullOrEmpty(ddl_Customer.SelectedValue) Then
                Dim IDs() As String
                IDs = customer.Split("$")
                cid = IDs(0)
                Site = IDs(1)
            End If

            If Not String.IsNullOrEmpty(ddl_Product.SelectedValue) Then
                PID = ddl_Product.SelectedValue
            End If

            If objProduct.SaveVATRule(Err_No, Err_Desc, ddl_Org.SelectedItem.Value, cid, Site, PID, txt_Value.Text, txt_code.Text, CType(Session("User_Access"), UserAccess).UserID) = True Then
                success = True
                MessageBoxValidation("Successfully Saved.", "Information")
                BindVATRule()
                MPEAdd.VisibleOnPageLoad = False
                Panel.Update()
            Else
                MessageBoxValidation("Could not be saved.", "Information")
                BindVATRule()
                MPEAdd.VisibleOnPageLoad = False
                Panel.Update()
                log.Error(Err_Desc)
            End If

            If success = True Then

                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "PRODUCT MANAGEMENT", "VAT RULES", Me.ddl_Org.SelectedValue.ToString(), "Customer: " & Me.ddl_Customer.Text & " Product: " & Me.ddl_Product.Text & "/ Vat Value:" & txt_Value.Text & "Code :  " & Me.txt_code.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")

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
            RowID = CType(row.FindControl("lblVAT_Rule_ID"), Label).Text

            If objProduct.DeleteVATRule(Err_No, Err_Desc, ddl_Org.SelectedItem.Value, RowID) = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "D", "PRODUCT MANAGEMENT", "VAT RULES", Me.ddl_Org.SelectedValue.ToString(), "Customer: " & Customer & " Product: " & Item & "/ Vat Value:" & vat_value & "Code :  " & vat_code, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                success = True
            End If
            
            If success = True Then
                MessageBoxValidation("Successfully deleted.", "Information")
                BindVATRule()
            Else
                MessageBoxValidation("Error occured while deleting VAT rule.", "Information")
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
                    RowID = CType(row.FindControl("lblVAT_Rule_ID"), Label).Text

                    If objProduct.DeleteVATRule(Err_No, Err_Desc, ddl_Org.SelectedItem.Value, RowID) = True Then
                        objLogin.SaveUserLog(Err_No, Err_Desc, "D", "PRODUCT MANAGEMENT", "VAT RULES", Me.ddl_Org.SelectedValue.ToString(), "Customer: " & Customer & " Product: " & Item & "/ Vat Value:" & vat_value & "Code :  " & vat_code, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                        Success = True
                    End If

                   
                End If
            Next
            If Success = True Then
                MessageBoxValidation("Successfully deleted.", "Information")
                BindVATRule()
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
            lblmsgPopUp.Text = ""
            lblUpMsg.Text = ""
            btnUpdate.Visible = True
            btnSave.Visible = False
            ddl_Product.Enabled = True
            txt_code.Enabled = True


            
            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)

            Dim CustomerID As String
            CustomerID = CType(row.FindControl("lblCustomer"), Label).Text
            Dim SiteID As String
            SiteID = CType(row.FindControl("LblSite_Use_ID"), Label).Text

            Dim custid As String
            custid = CustomerID & "$" & SiteID
            ' ''If Not ddl_Customer.FindItemByValue(custid) Is Nothing Then
            ' ''    ddl_Customer.ClearSelection()
            ' ''    ddl_Customer.FindItemByValue(custid).Selected = True
            ' ''End If


            ddl_Product.ClearSelection()
            ddl_Product.Items.Clear()
            ddl_Product.Text = ""

            Dim Objrep As New SalesWorx.BO.Common.Reports()
            Dim dt1 As New DataTable
            dt1 = Objrep.GetCustomerfromOrgtext(Err_No, Err_Desc, ddl_Org.SelectedValue, CustomerID)


           

            'Loop through the values to populate the combo box
            For i As Integer = 0 To dt1.Rows.Count - 1
                Dim item As New RadComboBoxItem()
                item.Text = dt1.Rows(i).Item("Customer").ToString
                item.Value = dt1.Rows(i).Item("CustomerID").ToString

                ddl_Customer.Items.Add(item)
                item.DataBind()
            Next


            If Not ddl_Customer.FindItemByValue(custid) Is Nothing Then
                ddl_Customer.ClearSelection()
                ddl_Customer.FindItemByValue(custid).Selected = True
            End If



            Dim Itemcode As String
            Itemcode = CType(row.FindControl("lblInventory_Item_ID"), Label).Text
            ' ''If Not ddl_Product.FindItemByValue(Itemcode) Is Nothing Then
            ' ''    ddl_Product.ClearSelection()
            ' ''    ddl_Product.FindItemByValue(Itemcode).Selected = True
            ' ''End If


            ''loading Item by item code

            ddl_Product.ClearSelection()
            ddl_Product.Items.Clear()
            ddl_Product.Text = ""


            Dim dt As New DataTable
            dt = Objrep.GetItemByInventory(Err_No, Err_Desc, Itemcode)


            'Loop through the values to populate the combo box
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim item As New RadComboBoxItem()
                item.Text = dt.Rows(i).Item("Description").ToString
                item.Value = dt.Rows(i).Item("Inventory_Item_ID").ToString

                ddl_Product.Items.Add(item)
                item.DataBind()
            Next

            If Not ddl_Product.FindItemByValue(Itemcode) Is Nothing Then
                ddl_Product.ClearSelection()
                ddl_Product.FindItemByValue(Itemcode).Selected = True
            End If


            ddl_Customer.Enabled = False
            ddl_Product.Enabled = False
            txt_Value.Text = Server.HtmlDecode(row.Cells(3).Text)
            txt_code.Text = Server.HtmlDecode(row.Cells(4).Text)
            If txt_code.Text = "&nbsp;" Then
                txt_code.Text = ""
            End If
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

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs)
        MPEAdd.VisibleOnPageLoad = False
        Panel.Update()
        Return
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs)
        If String.IsNullOrEmpty(txt_Value.Text) Then
            lblmsgPopUp.Text = "Please enter the VAT value."
            MPEAdd.VisibleOnPageLoad = True
            Return
        End If
        If IsNumeric(txt_Value.Text) = False Then
            lblmsgPopUp.Text = "VAT value should be in number."
            Me.MPEAdd.VisibleOnPageLoad = True
            Return
        End If
        Dim VAT_TYPE As String = "V"
        Dim dt As DataTable
        dt = objDivConfig.GetDivisionalConfiguration(Err_No, Err_Desc, ddl_Org.SelectedItem.Value)
        If dt.Rows.Count > 0 Then
            VAT_TYPE = dt.Rows(0)("VAT_TYPE")
        End If
        If VAT_TYPE = "B" Then
            If String.IsNullOrEmpty(txt_code.Text) Then
                lblmsgPopUp.Text = "Please enter the VAT Code."
                MPEAdd.VisibleOnPageLoad = True
                Return
            End If
        End If
        Dim success As Boolean = False
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
            If Not String.IsNullOrEmpty(ddl_Product.SelectedValue) Then
                PID = ddl_Product.SelectedValue
            End If



            If objProduct.SaveVATRule(Err_No, Err_Desc, ddl_Org.SelectedItem.Value, cid, Site, PID, txt_Value.Text, txt_code.Text, CType(Session("User_Access"), UserAccess).UserID) = True Then
                success = True
                MessageBoxValidation("Successfully Saved.", "Information")
                BindVATRule()
                MPEAdd.VisibleOnPageLoad = False
                Panel.Update()
            Else
                MessageBoxValidation("Could not be saved.", "Information")
                BindVATRule()
                MPEAdd.VisibleOnPageLoad = False
                Panel.Update()
                log.Error(Err_Desc)
            End If

            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "U", "PRODUCT MANAGEMENT", "VAT RULES", Me.ddl_Org.SelectedValue.ToString(), "Customer: " & Me.ddl_Customer.Text & " Product: " & Me.ddl_Product.Text & "/ Vat Value:" & txt_Value.Text & "Code :  " & Me.txt_code.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")

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

        BindVATRule()
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
        BindVATRule()
    End Sub

    Private Sub BindVATRule()
        
        Dim dtData As New DataTable
        Dim Customer As String
        Dim ProdVal As String = ""
        Dim CustomerID As String = ""
        Dim SiteID As String = ""

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

        If ddl_FilterCustomer.SelectedValue <> "" Then
            ProdVal = ddl_FilterProduct.SelectedValue
        Else
            ProdVal = "0"
        End If

        dtData = objProduct.GetVATRule(Err_No, Err_Desc, ddl_Org.SelectedItem.Value, CustomerID, SiteID, ProdVal)
        Dim dv As New DataView(dtData)
        If ViewState("SortField") <> "" Then
            dv.Sort = (ViewState("SortField") & " ") + SortDirection
        End If

        Me.dgvItems.DataSource = Nothing
        Me.dgvItems.DataSource = dv
        Me.dgvItems.DataBind()
        PnlGridData.Visible = True

    End Sub



     

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If Me.ddl_Org.SelectedIndex > 0 Then
            ddl_Product.ClearSelection()
            ddl_Customer.ClearSelection()
            Me.lblmsgPopUp.Text = ""
            ddl_Customer.Enabled = True
            ddl_Product.Enabled = True
            txt_code.Text = ""
            txt_Value.Text = ""
            Me.MPEAdd.VisibleOnPageLoad = True
            btnSave.Visible = True
            btnUpdate.Visible = False
            Panel.Update()
           
        Else
            MessageBoxValidation("Please select a organization", "Information")
            Exit Sub
        End If
    End Sub

    Private Sub ddl_FilterCustomer_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddl_FilterCustomer.ItemsRequested
        Try

            Dim Objrep As New SalesWorx.BO.Common.Reports()
            Dim dt As New DataTable
            dt = Objrep.GetCustomerfromOrgtext(Err_No, Err_Desc, ddl_Org.SelectedValue, e.Text)


            Dim ItemsPerRequest As Integer = 100 'Set the number of values to show
            Dim itemOffset As Integer = e.NumberOfItems
            Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
            e.EndOfItems = endOffset = dt.Rows.Count

            'Loop through the values to populate the combo box
            For i As Integer = itemOffset To endOffset - 1
                Dim item As New RadComboBoxItem()
                item.Text = dt.Rows(i).Item("Customer").ToString
                item.Value = dt.Rows(i).Item("CustomerID").ToString

                ddl_FilterCustomer.Items.Add(item)
                item.DataBind()
            Next

        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub ddl_Customer_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddl_FilterCustomer.SelectedIndexChanged
        BindVATRule()
        Panel.Update()
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
                item.Value = dt.Rows(i).Item("Inventory_Item_ID").ToString

                ddl_FilterProduct.Items.Add(item)
                item.DataBind()
            Next


        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub ddl_Product_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddl_FilterProduct.SelectedIndexChanged
        BindVATRule()
        Panel.Update()
    End Sub

    Private Sub btn_clearFilter_Click(sender As Object, e As EventArgs) Handles btn_clearFilter.Click
        ddl_Org.ClearSelection()
        ddl_FilterCustomer.ClearSelection()
        ddl_FilterProduct.ClearSelection()
        ddl_FilterCustomer.Items.Clear()
        ddl_FilterCustomer.Text = ""
        ddl_FilterProduct.Items.Clear()
        ddl_FilterProduct.Text = ""
    End Sub

    Private Sub ddl_Customer_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddl_Customer.ItemsRequested
        Try

            Dim Objrep As New SalesWorx.BO.Common.Reports()
            Dim dt As New DataTable
            dt = Objrep.GetCustomerfromOrgtext(Err_No, Err_Desc, ddl_Org.SelectedValue, e.Text)


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
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub ddl_Product_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddl_Product.ItemsRequested
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
                item.Value = dt.Rows(i).Item("Inventory_Item_ID").ToString

                ddl_Product.Items.Add(item)
                item.DataBind()
            Next


        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
    Private Sub btndownloadTemp_Click(sender As Object, e As EventArgs) Handles btndownloadTemp.Click
        Dim Filename As String = System.Configuration.ConfigurationManager.AppSettings("ExcelTemplatePath") & "VAT_Rules.xls"
        Dim TheFile As FileInfo = New FileInfo(Filename)
        If TheFile.Exists Then
            Dim strFileName As String = "Template" + Now.ToString("ddMMMyyHHmmss") + ".xls"


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

            Dim TheFile As FileInfo = New FileInfo(Filename)
            If TheFile.Exists Then
                File.Delete(Filename)
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))

        End Try
    End Sub
End Class





