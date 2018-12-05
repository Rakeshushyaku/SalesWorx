
Imports SalesWorx.BO.Common
Imports Telerik.Web.UI
Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports ExcelLibrary.SpreadSheet
Partial Public Class PriceDefinition
    Inherits System.Web.UI.Page
    Dim objProduct As New Product


    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const PageID As String = "P365"
    Private dt As New DataTable
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objControl As New AppControl




    Protected Sub ddl_org_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_org.SelectedIndexChanged

        If ddl_org.SelectedItem.Text <> "Select Organization" Then
            ResetDetails()
            Me.hfOrgID.Value = Me.ddl_org.SelectedValue.ToString()
            
            Me.ddlOrdCode.Enabled = True

            BindPriceData()


        Else
            
            Me.hfOrgID.Value = ""

            Me.ddlOrdCode.Enabled = False
            ResetDetails()
            BindPriceData()

        End If

    End Sub

    Private Sub btnImportWindow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportWindow.Click
        If Me.ddl_org.SelectedIndex > 0 Then
            Me.lblUpMsg.Text = ""
            Session("dtPriceRules") = Nothing
            dgvErros.DataSource = Nothing
            dgvErros.DataBind()
            dgvErros.Visible = False
            lbLog.Visible = False
            Me.MPEImport.VisibleOnPageLoad = True
        End If
    End Sub


    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try

            If Me.ddl_org.SelectedIndex > 0 Then
                Dim dtOriginal As New DataTable()
                Dim OrgID As String = IIf(Me.hfOrgID.Value = "", "0", Me.hfOrgID.Value)
                Dim BnsPlanID As String = IIf(Me.hfPlanId.Value = "", "0", Me.hfPlanId.Value)

                dtOriginal = objProduct.ExportPriceData(Err_No, Err_Desc, OrgID, BnsPlanID)

                Dim dtTemp As New DataTable()

                'Creating Header Row
                dtTemp.Columns.Add("ItemCode")
                dtTemp.Columns.Add("UOM")
                dtTemp.Columns.Add("SellingPrice")
                

                Dim drAddItem As DataRow
                For i As Integer = 0 To dtOriginal.Rows.Count - 1
                    drAddItem = dtTemp.NewRow()
                    drAddItem(0) = dtOriginal.Rows(i)("ItemCode").ToString()
                    drAddItem(1) = dtOriginal.Rows(i)("UOM").ToString()
                    drAddItem(2) = CDec(dtOriginal.Rows(i)("SellingPrice").ToString())
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

                'Temp(Grid)
                Dim dg As New DataGrid()
                dg.DataSource = dtTemp
                dg.DataBind()
                If dtTemp.Rows.Count > 0 Then
                    'Dim fn As String = "MSL" & Now.ToString("ddMMMyyHHmmss") + ".xls"
                    Dim fn As String = "Price" + Now.ToString("ddMMMyyHHmmss") + ".xls"
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


                Me.ddlOrdCode.Enabled = False

                LoadOrgHeads()

                ResetDetails()

                Me.lblPlanName.Text = Request.QueryString("Desc").ToString()
                Me.hfPlanId.Value = Request.QueryString("PGID").ToString()

                Me.lblOrg.Text = Request.QueryString("ORGNAME").ToString()
                Me.hfOrgID.Value = Request.QueryString("ORGID").ToString()
                Me.ddl_org.SelectedValue = hfOrgID.Value


                Me.ddlOrdCode.Enabled = True



                ViewState("FileType") = Nothing
                ViewState("FileName") = Nothing
                ViewState("CSVName") = Nothing


                
                Me.ddlOrdCode.Enabled = True


                BindPriceData()


            Else
                MPEImport.VisibleOnPageLoad = False
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

        If Me.hfPlanId.Value = "" Or Me.hfOrgID.Value = "" Then
            Me.lblUpMsg.Text = "Organization and price list id is empty."
            Me.MPEImport.VisibleOnPageLoad = True
            Exit Sub
        End If

        Session("dtPriceRules") = Nothing
        Dim dtErrors As New DataTable
        dtErrors = SetErrorsTable().Copy
        Dim Str As New StringBuilder

        Dim TotSuccess As Integer = 0
        Dim TotFailed As Integer = 0
        Try
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
                        col.ColumnName = "ItemCode"
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
                        col.ColumnName = "SellingPrice"
                        col.DataType = System.Type.GetType("System.Double")
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
                            If Not (TempTbl.Columns(0).ColumnName = "ItemCode" And TempTbl.Columns(1).ColumnName = "UOM" And TempTbl.Columns(2).ColumnName = "SellingPrice") Then
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
                            Me.lblUpMsg.Text = "There is no data in the file."
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
                                Dim OrgID As String = Nothing
                                Dim ItemCode As String = Nothing
                                Dim UOM As String = "EA"
                                Dim SellingPrice As String = Nothing
                                
                                Dim isValidRow As Boolean = True

                                OrgID = Me.hfOrgID.Value

                                ItemCode = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, "0", TempTbl.Rows(idx)(0).ToString())
                                UOM = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, "0", TempTbl.Rows(idx)(1).ToString())
                                SellingPrice = IIf(TempTbl.Rows(idx)(2) Is DBNull.Value, "0", TempTbl.Rows(idx)(2).ToString())
                                
                                If ItemCode = "0" Or ItemCode Is Nothing Then
                                    Continue For
                                End If



                                If ItemCode = "0" Or objProduct.CheckItemCode(Err_No, Err_Desc, ItemCode, OrgID) = False Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Invalid Item code" + ","
                                    isValidRow = False
                                    TotFailed += 1
                                End If

                                If UOM = "0" Or objProduct.CheckItemUOM(Err_No, Err_Desc, ItemCode, OrgID, UOM) = False Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Invalid UOM" + ","
                                    isValidRow = False
                                    TotFailed += 1
                                End If

                                'If SellingPrice = "0" Or IsNumeric(SellingPrice) = False Then
                                '    RowNo = idx + 2
                                '    ErrorText = ErrorText + "Invalid Selling price" + ","
                                '    isValidRow = False
                                '    TotFailed += 1
                                'End If

                                If IsNumeric(SellingPrice) = False Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Invalid Selling price" + ","
                                    isValidRow = False
                                    TotFailed += 1
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

                                    Dim ItemID As String = objProduct.GetInventoryItemID(Err_No, Err_Desc, ItemCode, OrgID)
                                    If objProduct.SavePriceData(Err_No, Err_Desc, ItemID, OrgID, UOM, CDec(SellingPrice), IIf(Me.hfPlanId.Value = "", "0", Me.hfPlanId.Value)) = True Then
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
                        BindPriceData()
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
                    Session.Remove("dtPriceRules")
                    Session("dtPriceRules") = dtErrors.Copy


                    Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & "PriceLog_" & Now.ToString("yyyyMMdd") + ".txt"
                    DataTable2CSV(dtErrors, fn, vbTab)
                    If dtErrors.Rows.Count > 0 Then
                        lbLog.Visible = True
                    End If
                    Session.Remove("PriceLogInfo")
                    Session("PriceLogInfo") = fn




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
   
    Protected Sub lbLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbLog.Click
        Try


            If Not Session("PriceLogInfo") Is Nothing Then
                Dim fileValue As String = Session("PriceLogInfo")





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
                    lblUpMsg.Text = "File does not exists"
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


  

    Private Sub ddlOrdCode_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddlOrdCode.ItemsRequested


        Dim Objrep As New SalesWorx.BO.Common.Reports()
        Dim dt As New DataTable
        dt = objProduct.LoadPriceProductList(Err_No, Err_Desc, hfOrgID.Value, e.Text)


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




    Protected Sub ddlOrdCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ddlOrdCode.SelectedIndexChanged
        If Not String.IsNullOrEmpty(Me.ddlOrdCode.SelectedValue) Then
           
            If Not String.IsNullOrEmpty(ddlOrdCode.SelectedValue) Then

                Me.LblDItemId.Text = ddlOrdCode.SelectedValue
            End If
            LoadUOM()
        Else
            Me.LblDItemId.Text = ""
        End If
        BindPriceData()
    End Sub

  
    Sub LoadUOM()
        Me.ddlUOM.Items.Clear()
        Me.ddlUOM.DataSource = objProduct.GetProductUOM(Err_No, Err_Desc, IIf(Me.hfOrgID.Value = "", "0", Me.hfOrgID.Value), IIf(Me.LblDItemId.Text = "", "0", Me.LblDItemId.Text))

        ddlUOM.Items.Add(New RadComboBoxItem("Select UOM"))
        ddlUOM.AppendDataBoundItems = True
        Me.ddlUOM.DataValueField = "UOM"
        Me.ddlUOM.DataTextField = "UOM"
        Me.ddlUOM.DataBind()

    End Sub
   
    Sub LoadOrgHeads()
        ddl_org.Items.Clear()
        Dim objCommon As New Common
        Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
        ddl_org.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
        ddl_org.Items.Add(New RadComboBoxItem("Select Organization"))
        ddl_org.AppendDataBoundItems = True
        ddl_org.DataValueField = "MAS_Org_ID"
        ddl_org.DataTextField = "Description"
        ddl_org.DataBind()

    End Sub

    Private Sub dgvItems_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgvItems.RowCommand

        Try
           

            If (e.CommandName = "EditRecord") Then



                Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)

                
                Me.lblLineId.Text = Convert.ToInt32(dgvItems.DataKeys(row.RowIndex).Value)
                Me.lblEditRow.Text = row.RowIndex
                Dim lblDItem As Label = DirectCast(row.FindControl("lblDItem"), Label)
                Dim lblDCode As Label = DirectCast(row.FindControl("lblDCode"), Label)


                Me.LblDItemId.Text = lblDItem.Text
                ddlOrdCode.Items.Clear()
                ddlOrdCode.ClearSelection()
                ddlOrdCode.Text = ""
                Dim Objrep As New SalesWorx.BO.Common.Reports()
                Dim dt As New DataTable
                dt = objProduct.LoadPriceProductList(Err_No, Err_Desc, hfOrgID.Value, lblDCode.Text)



                'Loop through the values to populate the combo box
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim item As New RadComboBoxItem()
                    item.Text = dt.Rows(i).Item("CodeText").ToString
                    item.Value = dt.Rows(i).Item("CodeValue").ToString

                    ddlOrdCode.Items.Add(item)
                    item.DataBind()
                Next

                
                ddlOrdCode.SelectedValue = LblDItemId.Text

                LoadUOM()
                Me.ddlUOM.SelectedValue = dgvItems.Rows(row.RowIndex).Cells(4).Text
                Me.txtUnitPrice.Text = CDec(dgvItems.Rows(row.RowIndex).Cells(5).Text)
                





               




                Me.btnAddItems.Text = "Update"
                Me.ddlOrdCode.Enabled = False
                Me.ddlUOM.Enabled = False
                UpdatePanel1.Update()
            End If


            If (e.CommandName = "DeleteRecord") Then



                Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
                Me.lblLineId.Text = Convert.ToInt32(dgvItems.DataKeys(row.RowIndex).Value)
                objProduct.DeletePriceData(Err_No, Err_Desc, Me.lblLineID.Text)

                Me.btnAddItems.Text = "Add"
                Me.ddlOrdCode.Enabled = True
                Me.ddlUOM.Enabled = True
                BindPriceData()
                UpdatePanel1.Update()
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("information.aspx?mode=1&errno=" & "74071" & "&msg=" & AppMsgHandler.GetErrorMessage("SWX_BO_Msg_005") & "&next=PriceDefinition.aspx", False)
        Finally
        End Try
    End Sub

    Private Sub dgv_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgvItems.Sorting
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"

        BindPriceData()
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
        BindPriceData()
    End Sub




   


    Private Sub BindPriceData()

        Dim ItemId As String = Me.ddlOrdCode.SelectedValue
       
        Dim dtData As New DataTable
        dtData = objProduct.GetPriceListData(Err_No, Err_Desc, Me.hfOrgID.Value, ItemId, IIf(Me.hfPlanId.Value = "", "0", Me.hfPlanId.Value))
        Dim dv As New DataView(dtData)
        If ViewState("SortField") <> "" Then
            dv.Sort = (ViewState("SortField") & " ") + SortDirection
        End If

        Me.dgvItems.DataSource = Nothing
        Me.dgvItems.DataSource = dv
        Me.dgvItems.DataBind()
        PnlGridData.Visible = True

    End Sub













    Protected Sub btnAddItems_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddItems.Click
        If ValidationDetails() = False Then
            Try





                Err_No = Nothing
                Err_Desc = Nothing
                Dim success As Boolean
              
                Dim ItemCode As String = Me.ddlOrdCode.SelectedValue


                'Check Item exists
                If btnAddItems.Text = "Add" Then


                    If objProduct.CheckPriceDataExists(Err_No, Err_Desc, ItemCode, Me.hfOrgID.Value, hfPlanId.Value, IIf(Me.ddlUOM.SelectedIndex <= 0, "0", ddlUOM.SelectedValue)) Then
                        MessageBoxValidation("Price already exists for this item and selected UOM/Price list", "Validation")
                        Exit Sub
                    End If

                End If
                If Me.btnAddItems.Text = "Add" Then
                    success = objProduct.SavePriceData(Err_No, Err_Desc, ItemCode, Me.hfOrgID.Value, IIf(Me.ddlUOM.SelectedIndex <= 0, "0", ddlUOM.SelectedValue), CDec(IIf(Me.txtUnitPrice.Text = "", "0", Me.txtUnitPrice.Text)), IIf(hfPlanId.Value = "", "0", Me.hfPlanId.Value))

                ElseIf Me.btnAddItems.Text = "Update" Then
                    success = objProduct.UpdatePriceData(Err_No, Err_Desc, Me.lblLineID.Text, Me.LblDItemId.Text, IIf(Me.ddlUOM.SelectedIndex <= 0, "0", ddlUOM.SelectedValue), CDec(IIf(Me.txtUnitPrice.Text = "", "0", Me.txtUnitPrice.Text)), IIf(hfPlanId.Value = "", "0", Me.hfPlanId.Value))
                End If
                If success = True Then
                    MessageBoxValidation("Successfully saved.", "Information")
                    Me.btnAddItems.Text = "Add"
                    Me.txtUnitPrice.Text = ""
                    Me.ddlOrdCode.Enabled = True
                    Me.ddlUOM.Enabled = True
                    BindPriceData()
                Else
                    MessageBoxValidation("Error while saving price details", "Information")
                    Exit Sub
                End If
                Me.ddlUOM.ClearSelection()
                Me.ddlUOM.Text = ""

            Catch ex As Exception
                log.Error(ex.Message)
                Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_OrderEntry_006") & "&next=PriceDefinition.aspx&Title=Bonus Definition", False)
            End Try
        End If
    End Sub

    Protected Sub ResetBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClear.Click
        ResetDetails()
        BindPriceData()
    End Sub


    Private Sub ResetDetails()
        Me.LblDItemId.Text = ""
     
      
        Me.txtUnitPrice.Text = ""
        Me.lblDUOM.Text = ""

        Me.btnAddItems.Text = "Add"
        
        ddlOrdCode.Items.Clear()
        ddlOrdCode.ClearSelection()
        ddlOrdCode.Text = ""
        Me.lblEditRow.Text = ""
        Me.lblOrgId.Text = ""
        Me.lblLineId.Text = ""


        Me.ddlUOM.Items.Clear()
        Me.ddlUOM.ClearSelection()
        ddlUOM.Items.Add(New RadComboBoxItem("Select UOM"))
        ddlUOM.AppendDataBoundItems = True


        Me.dgvItems.DataSource = Nothing
        Me.dgvItems.DataBind()
        PnlGridData.Visible = False
        ' Me.ddOraganisation.Enabled = True
        If Not Me.hfOrgID.Value Is Nothing Then
            'Me.txtItemCode.Enabled = True
            'Me.txtDescription.Enabled = True
            Me.ddlOrdCode.Enabled = True
            Me.ddlUOM.Enabled = True
        End If
    End Sub
    
    Private Function ValidationDetails() As Boolean
        Dim sucess As Boolean = False



        If Me.hfOrgID.Value Is Nothing And btnAddItems.Text = "Add" Then
            MessageBoxValidation("Please select a organization", "Validation")
            sucess = True
            Return sucess
            Exit Function
        End If






        If Me.LblDItemId.Text = "" Then
            MessageBoxValidation("Please enter a valid item code/description", "Validation")
            sucess = True

            Return sucess
            Exit Function
        End If


        'If Me.txtUnitPrice.Text = "" Or Me.txtUnitPrice.Text = "0" Then
        '    MessageBoxValidation("Please enter a valid unit selling price", "Validation")
        '    sucess = True

        '    Return sucess
        '    Exit Function
        'End If

        If Me.txtUnitPrice.Text = "" Then
            MessageBoxValidation("Please enter a valid unit selling price", "Validation")
            sucess = True

            Return sucess
            Exit Function
        End If
        If Me.ddlUOM.SelectedIndex <= 0 Then
            MessageBoxValidation("Please select a UOM", "Validation")
            sucess = True

            Return sucess
            Exit Function
        End If


        Return sucess
    End Function

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("ManagePriceList.aspx?OID=", False)
    End Sub
End Class





