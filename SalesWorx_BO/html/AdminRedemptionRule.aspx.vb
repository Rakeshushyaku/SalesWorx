Imports SalesWorx.BO.Common
Imports Telerik.Web.UI
Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports ExcelLibrary.SpreadSheet
Public Class AdminRedemptionRule
    Inherits System.Web.UI.Page

    Dim objcommon As New SalesWorx.BO.Common.Common
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim objProduct As New SalesWorx.BO.Common.Product
    Dim objDivConfig As New DivConfig
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const PageID As String = "P369"
    Private dt As New DataTable
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objControl As New AppControl
    Protected Sub ddl_org_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Org.SelectedIndexChanged

        If ddl_Org.SelectedItem.Value = 0 Then
            ddlRedItem.ClearSelection()
            ddlRedItem.Items.Clear()
            ddlRedItem.Text = ""

            ddlGivenItem.ClearSelection()
            ddlGivenItem.Items.Clear()
            ddlGivenItem.Text = ""
        End If




        Panel.Update()
    End Sub
 
   

    Private Sub btnImportWindow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportWindow.Click
        If Me.ddl_Org.SelectedIndex > 0 Then
            Me.lblUpMsg.Text = ""
            Session("dtRedRules") = Nothing
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


                dtOriginal = objProduct.ExportRedemptionRule(Err_No, Err_Desc, ddl_Org.SelectedItem.Value)

                Dim dtTemp As New DataTable()

                'Creating Header Row

                dtTemp.Columns.Add("ReturnItemCode")
                dtTemp.Columns.Add("ReturnQty")
                dtTemp.Columns.Add("SalesItemCode")
                dtTemp.Columns.Add("SalesQty")
                dtTemp.Columns.Add("ValidFrom")
                dtTemp.Columns.Add("ValidTo")
                dtTemp.Columns.Add("IsActive")
                
                Dim drAddItem As DataRow
                For i As Integer = 0 To dtOriginal.Rows.Count - 1
                    drAddItem = dtTemp.NewRow()
                    drAddItem(0) = IIf(dtOriginal.Rows(i)("RedID") Is DBNull.Value, "0", dtOriginal.Rows(i)("RedID").ToString())
                    drAddItem(1) = CLng(IIf(dtOriginal.Rows(i)("RedQty") Is DBNull.Value, "0", dtOriginal.Rows(i)("RedQty").ToString()))
                    drAddItem(2) = IIf(dtOriginal.Rows(i)("GivenID") Is DBNull.Value, "0", dtOriginal.Rows(i)("GivenID").ToString())
                    drAddItem(3) = CLng(IIf(dtOriginal.Rows(i)("GivenQty") Is DBNull.Value, "0", dtOriginal.Rows(i)("GivenQty").ToString()))
                    drAddItem(6) = IIf(dtOriginal.Rows(i)("Status") Is DBNull.Value, "Y", dtOriginal.Rows(i)("Status").ToString())
                    drAddItem(4) = DateTime.Parse(dtOriginal.Rows(i)("ValidFrom").ToString()).ToString("yyyy-MM-dd")
                    drAddItem(5) = DateTime.Parse(dtOriginal.Rows(i)("ValidTo").ToString()).ToString("yyyy-MM-dd")
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
                    drAddItem(6) = ""

                    dtTemp.Rows.Add(drAddItem)
                End If

                'Temp(Grid)
                Dim dg As New DataGrid()
                dg.DataSource = dtTemp
                dg.DataBind()
                If dtTemp.Rows.Count > 0 Then
                    'Dim fn As String = "MSL" & Now.ToString("ddMMMyyHHmmss") + ".xls"
                    Dim fn As String = "RedemptionRules" + Now.ToString("ddMMMyyHHmmss") + ".xls"
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
                Me.StartTime.SelectedDate = Now.Date.AddDays(1)
                Me.EndTime.SelectedDate = Me.StartTime.SelectedDate.Value.AddMonths(1)
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


        Session("dtRedRules") = Nothing
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
                        col.ColumnName = "ReturnItemCode"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)


                        col = New DataColumn
                        col.ColumnName = "ReturnQty"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)



                        col = New DataColumn
                        col.ColumnName = "SalesItemCode"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)


                        col = New DataColumn
                        col.ColumnName = "SalesQty"
                        col.DataType = System.Type.GetType("System.Double")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)


                        col = New DataColumn
                        col.ColumnName = "ValidFrom"
                        col.DataType = System.Type.GetType("System.DateTime")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)

                        col = New DataColumn
                        col.ColumnName = "ValidTo"
                        col.DataType = System.Type.GetType("System.DateTime")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)



                        col = New DataColumn
                        col.ColumnName = "IsActive"
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

                        If TempTbl.Columns.Count = 7 Then
                            If Not (TempTbl.Columns(0).ColumnName.ToLower = "returnitemcode" And TempTbl.Columns(1).ColumnName.ToLower = "returnqty" And TempTbl.Columns(2).ColumnName.ToLower = "salesitemcode" And TempTbl.Columns(3).ColumnName.ToLower = "salesqty" And TempTbl.Columns(4).ColumnName.ToLower = "validfrom" And TempTbl.Columns(5).ColumnName.ToLower = "validto" And TempTbl.Columns(6).ColumnName.ToLower = "isactive") Then
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
                                Dim RedItem As String = Nothing
                                Dim RedQty As String = Nothing
                                Dim GivenItem As String = Nothing
                                Dim GivenQty As String = Nothing
                                Dim ISActive As String = "Y"
                                Dim ValidFrom As String = Nothing
                                Dim ValidTo As String = Nothing
                                Dim isValidRow As Boolean = True
                                Dim FromDate As Date = Nothing
                                Dim ToDate As Date = Nothing

                                RedItem = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, "0", TempTbl.Rows(idx)(0).ToString())
                                RedQty = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, "0", TempTbl.Rows(idx)(1).ToString())
                                GivenItem = IIf(TempTbl.Rows(idx)(2) Is DBNull.Value, "0", TempTbl.Rows(idx)(2).ToString())
                                GivenQty = IIf(TempTbl.Rows(idx)(3) Is DBNull.Value, "0", TempTbl.Rows(idx)(3).ToString())
                                ISActive = IIf(TempTbl.Rows(idx)(6) Is DBNull.Value, "Y", TempTbl.Rows(idx)(6).ToString())
                                ValidFrom = Trim(Replace(IIf(TempTbl.Rows(idx)(4) Is DBNull.Value, "0", TempTbl.Rows(idx)(4).ToString()), "12:00:00 AM", ""))
                                ValidTo = Trim(Replace(IIf(TempTbl.Rows(idx)(5) Is DBNull.Value, "0", TempTbl.Rows(idx)(5).ToString()), "12:00:00 AM", ""))


                                
                                If RedItem <> "0" Then
                                    If objProduct.CheckItemCode(Err_No, Err_Desc, RedItem, ddl_Org.SelectedItem.Value) = False Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Invalid return item code" + ","
                                        isValidRow = False
                                        TotFailed += 1
                                    End If
                                End If

                                If GivenItem <> "0" Then
                                    If objProduct.CheckItemCode(Err_No, Err_Desc, GivenItem, ddl_Org.SelectedItem.Value) = False Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Invalid sales item code" + ","
                                        isValidRow = False
                                        TotFailed += 1
                                    End If
                                End If


                                If IsNumeric(RedQty) = False Or Val(RedQty) = "0" Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Invalid return qty" + ","
                                    isValidRow = False
                                    TotFailed += 1
                                End If

                                If IsNumeric(GivenQty) = False Or Val(GivenQty) = "0" Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Invalid sales qty" + ","
                                    isValidRow = False
                                    TotFailed += 1
                                End If

                                If ISActive <> "Y" And ISActive <> "N" Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Is Active should be Y/N" + ","
                                    isValidRow = False
                                    TotFailed += 1
                                End If

                                If ValidFrom = "0" Or IsValidInputDate(ValidFrom) = False Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Invalid valid from" + ","
                                    isValidRow = False
                                    TotFailed += 1
                                Else
                                    FromDate = FormatDate(ValidFrom)
                                End If
                                If ValidTo = "0" Or IsValidInputDate(ValidTo) = False Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Invalid valid to" + ","
                                    isValidRow = False
                                    TotFailed += 1
                                Else
                                    ToDate = FormatDate(ValidTo)
                                End If


                                If ValidFrom <> "0" And IsValidInputDate(ValidFrom) = True And ValidTo <> "0" And IsValidInputDate(ValidTo) = True Then
                                    If FromDate <= Now.Date Or ToDate <= Now.Date Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Valid from and to date should be greater than current date" + ","
                                        isValidRow = False
                                        TotFailed += 1
                                    End If
                                End If

                                If ValidFrom <> "0" And IsValidInputDate(ValidFrom) = True And ValidTo <> "0" And IsValidInputDate(ValidTo) = True Then
                                    If ToDate <= FromDate Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Valid to date should be greater than from date" + ","
                                        isValidRow = False
                                        TotFailed += 1
                                    End If
                                End If
                                If isValidRow = True Then
                                    If objProduct.CheckRedemptionItemExists(Err_No, Err_Desc, "0", RedItem, ddl_Org.SelectedValue, FromDate, ToDate) = True Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Same rule already exist for this item" + ","
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
                                    isValidRow = True
                                Else

                                    If objProduct.SaveRedemptionRule(Err_No, Err_Desc, ddl_Org.SelectedItem.Value, RedItem, RedQty, GivenItem, GivenQty, ISActive, CType(Session.Item("USER_ACCESS"), UserAccess).UserID,FromDate, ToDate) = True Then
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
                        gvRules.Rebind()
                    End If

                    dgvErros.Visible = False
                    If dtErrors.Rows.Count > 0 Then
                        lbLog.Visible = True
                    Else
                        lbLog.Visible = False
                    End If
                    Me.dgvErros.DataSource = dtErrors
                    Me.dgvErros.DataBind()
                    Session.Remove("dtRedRules")
                    Session("dtRedRules") = dtErrors.Copy


                    Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & "RedemptionLog_" & Now.ToString("yyyyMMdd") + ".txt"
                    DataTable2CSV(dtErrors, fn, vbTab)
                    If dtErrors.Rows.Count > 0 Then
                        lbLog.Visible = True
                    End If
                    Session.Remove("RedemptionLogInfo")
                    Session("RedemptionLogInfo") = fn




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


            If Not Session("RedemptionLogInfo") Is Nothing Then
                Dim fileValue As String = Session("RedemptionLogInfo")





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
        ddl_Org.Items.Add(New RadComboBoxItem("Select Organization", "0"))
        ddl_Org.AppendDataBoundItems = True
        ddl_Org.DataValueField = "MAS_Org_ID"
        ddl_Org.DataTextField = "Description"
        ddl_Org.DataBind()






    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If String.IsNullOrEmpty(Me.ddlRedItem.SelectedValue) Then
            lblmsgPopUp.Text = "Please select the return item"
            MPEAdd.VisibleOnPageLoad = True
            Return
        End If

        If String.IsNullOrEmpty(Me.ddlGivenItem.SelectedValue) Then
            lblmsgPopUp.Text = "Please select the sales item"
            MPEAdd.VisibleOnPageLoad = True
            Return
        End If

        If String.IsNullOrEmpty(txtRedQty.Text) Or txtRedQty.Text = "0" Then
            lblmsgPopUp.Text = "Please enter the return quantity"
            MPEAdd.VisibleOnPageLoad = True
            Return
        End If

        If String.IsNullOrEmpty(txtGivenQty.Text) Or txtGivenQty.Text = "0" Then
            lblmsgPopUp.Text = "Please enter the sales quantity"
            MPEAdd.VisibleOnPageLoad = True
            Return
        End If
        If IsNumeric(txtRedQty.Text) = False Or IsNumeric(txtGivenQty.Text) = False Then
            lblmsgPopUp.Text = "Return and sales qty should be in number"
            Me.MPEAdd.VisibleOnPageLoad = True
            Return
        End If

        If (Me.StartTime.SelectedDate.Value <= Now.Date() And Me.StartTime.Enabled = True) Or Me.EndTime.SelectedDate.Value <= Now.Date() Then
            lblmsgPopUp.Text = "Valid from and to date should be greater than current date"
            Me.MPEAdd.VisibleOnPageLoad = True
            Return

        End If

        If objProduct.CheckRedemptionItemExists(Err_No, Err_Desc, "0", ddlRedItem.SelectedValue, ddl_Org.SelectedValue, Me.StartTime.SelectedDate.Value, Me.EndTime.SelectedDate.Value) Then
            lblmsgPopUp.Text = "Same rule already exists with the specified date range"
            Me.MPEAdd.VisibleOnPageLoad = True
            Return
        End If
      
        Dim success As Boolean = False
        Try

         

            If objProduct.SaveRedemptionRule(Err_No, Err_Desc, ddl_Org.SelectedItem.Value, ddlRedItem.SelectedValue, txtRedQty.Text, ddlGivenItem.SelectedValue, txtGivenQty.Text, IIf(rsActive.Checked = True, "Y", "N"), CType(Session("User_Access"), UserAccess).UserID, Me.StartTime.SelectedDate.Value, Me.EndTime.SelectedDate.Value) = True Then
                success = True
                MessageBoxValidation("Successfully Saved.", "Information")
                gvRules.Rebind()
                MPEAdd.VisibleOnPageLoad = False
                Panel.Update()
            Else
                MessageBoxValidation("Could not be saved.", "Information")
                MPEAdd.VisibleOnPageLoad = False
                Panel.Update()
                log.Error(Err_Desc)
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
 
    Protected Sub gvRules_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles gvRules.ItemCommand

        Try
            If e.CommandName = "DeleteSelected" Then
                Dim item As GridDataItem = DirectCast(e.Item, GridDataItem)
                Dim RuleID As Label = DirectCast(item.FindControl("lblRuleID"), Label)


                If objProduct.DeleteRedemptionItem(Err_No, Err_Desc, RuleID.Text) Then
                    MessageBoxValidation("Successfully deleted", "Information")
                    gvRules.Rebind()
                Else
                    MessageBoxValidation("Error while deleting", "Validation")
                    Exit Sub
                End If
            End If

           

        Catch ex As Exception
            Err_No = "64224"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("PHX_006") & "&next=AdminRedemptionRule.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try





            Dim btn1 As ImageButton = TryCast(sender, ImageButton)
            Dim item As GridDataItem = DirectCast(btn1.NamingContainer, GridDataItem)
            Dim RuleID As Label = DirectCast(item.FindControl("lblRuleID"), Label)
            Dim RedID As Label = DirectCast(item.FindControl("lblRedID"), Label)
            Dim RedQty As Label = DirectCast(item.FindControl("lblRedQty"), Label)
            Dim GivenID As Label = DirectCast(item.FindControl("lblGivenID"), Label)
            Dim GivenQty As Label = DirectCast(item.FindControl("lblGivenQty"), Label)
            Dim IsActive As Label = DirectCast(item.FindControl("lblActive"), Label)
            Dim RedItem As Label = DirectCast(item.FindControl("lblRedItem"), Label)
            Dim GivenItem As Label = DirectCast(item.FindControl("lblGivenItem"), Label)
            Dim lblValidFrom As Label = DirectCast(item.FindControl("lblValidFrom"), Label)
            Dim lblValidTo As Label = DirectCast(item.FindControl("lblValidTo"), Label)

            Me.lblRule.Value = RuleID.Text
            Dim dt As New DataTable
            dt = objProduct.LoadRedemptionProductList(Err_No, Err_Desc, Me.ddl_Org.SelectedValue, RedItem.Text)

            ddlRedItem.Items.Clear()
            ddlRedItem.ClearSelection()
            ddlRedItem.Text = ""


            ddlGivenItem.Items.Clear()
            ddlGivenItem.ClearSelection()
            ddlGivenItem.Text = ""

            'Loop through the values to populate the combo box
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim item1 As New RadComboBoxItem()
                item1.Text = dt.Rows(i).Item("CodeText").ToString
                item1.Value = dt.Rows(i).Item("CodeValue").ToString
                ddlRedItem.Items.Add(item1)
                item1.DataBind()
            Next


            ddlRedItem.SelectedValue = RedID.Text
            ddlRedItem.Enabled = False



            Dim dt1 As New DataTable
            dt1 = objProduct.LoadRedemptionProductList(Err_No, Err_Desc, Me.ddl_Org.SelectedValue, GivenItem.Text)



            'Loop through the values to populate the combo box
            For i As Integer = 0 To dt1.Rows.Count - 1
                Dim item2 As New RadComboBoxItem()
                item2.Text = dt1.Rows(i).Item("CodeText").ToString
                item2.Value = dt1.Rows(i).Item("CodeValue").ToString
                ddlGivenItem.Items.Add(item2)
                item2.DataBind()
            Next


            ddlGivenItem.SelectedValue = GivenID.Text
            ddlGivenItem.Enabled = False


            Me.txtRedQty.Text = RedQty.Text
            Me.txtGivenQty.Text = GivenQty.Text

            If IsActive.Text = "Yes" Then
                rsActive.Checked = True
            Else
                rsActive.Checked = False
            End If


            If lblValidFrom.Text = "" Then
                Me.StartTime.SelectedDate = Now.Date.AddDays(1)
            Else
                Me.StartTime.SelectedDate = lblValidFrom.Text
            End If

            If lblValidTo.Text = "" Then
                Me.EndTime.SelectedDate = Me.StartTime.SelectedDate.Value.AddMonths(1)
            Else
                Me.EndTime.SelectedDate = lblValidTo.Text
            End If

            If Me.StartTime.SelectedDate.Value <= Now.Date Then
                Me.StartTime.Enabled = False
            Else
                Me.StartTime.Enabled = True
            End If

            Me.lblmsgPopUp.Text = ""
            btnUpdate.Visible = True
            btnSave.Visible = False
            MPEAdd.VisibleOnPageLoad = True


        Catch ex As Exception
            Err_No = "20421"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("PHX_006") & "&next=AdminRedemptionRule.aspx&Title=Message", False)
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
        ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "HideRadWindow();", True)
        Return
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs)
        If String.IsNullOrEmpty(Me.ddlRedItem.SelectedValue) Then
            lblmsgPopUp.Text = "Please select the return item"
            MPEAdd.VisibleOnPageLoad = True
            Return
        End If

        If String.IsNullOrEmpty(Me.ddlGivenItem.SelectedValue) Then
            lblmsgPopUp.Text = "Please select the sales item"
            MPEAdd.VisibleOnPageLoad = True
            Return
        End If

        If String.IsNullOrEmpty(txtRedQty.Text) Or txtRedQty.Text = "0" Then
            lblmsgPopUp.Text = "Please enter the return quantity"
            MPEAdd.VisibleOnPageLoad = True
            Return
        End If

        If String.IsNullOrEmpty(txtGivenQty.Text) Or txtGivenQty.Text = "0" Then
            lblmsgPopUp.Text = "Please enter the sales quantity"
            MPEAdd.VisibleOnPageLoad = True
            Return
        End If
        If IsNumeric(txtRedQty.Text) = False Or IsNumeric(txtGivenQty.Text) = False Then
            lblmsgPopUp.Text = "Return and sales qty should be in number"
            Me.MPEAdd.VisibleOnPageLoad = True
            Return
        End If

        If (Me.StartTime.SelectedDate.Value <= Now.Date() And Me.StartTime.Enabled = True) Or Me.EndTime.SelectedDate.Value <= Now.Date() Then
            lblmsgPopUp.Text = "Valid from and to date should be greater than current date"
            Me.MPEAdd.VisibleOnPageLoad = True
            Return

        End If
        If objProduct.CheckRedemptionItemExists(Err_No, Err_Desc, Me.lblRule.Value, ddlRedItem.SelectedValue, ddl_Org.SelectedValue, Me.StartTime.SelectedDate.Value, Me.EndTime.SelectedDate.Value) Then
            lblmsgPopUp.Text = "Same rule already exists with the specified date range"
            Me.MPEAdd.VisibleOnPageLoad = True
            Return
        End If


        Dim success As Boolean = False
        Try

            

            If objProduct.UpdateRedemptionRule(Me.lblRule.Value, Me.txtRedQty.Text, Me.txtGivenQty.Text, IIf(Me.rsActive.Checked = True, "Y", "N"), CType(Session("User_Access"), UserAccess).UserID, Err_No, Err_Desc, StartTime.SelectedDate.Value, EndTime.SelectedDate.Value) = True Then
                success = True
                MessageBoxValidation("Successfully Saved.", "Information")
                gvRules.Rebind()
                MPEAdd.VisibleOnPageLoad = False
                Panel.Update()
            Else
                MessageBoxValidation("Could not be saved.", "Information")
                MPEAdd.VisibleOnPageLoad = False
                Panel.Update()
                log.Error(Err_Desc)
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

    Protected Sub Button_Click(sender As Object, e As ImageClickEventArgs)
        gvRules.MasterTableView.FilterExpression = String.Empty
        gvRules.MasterTableView.Columns(0).CurrentFilterValue = ""
        gvRules.MasterTableView.Rebind()
    End Sub
  




    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If Me.ddl_Org.SelectedIndex > 0 Then
            ddlGivenItem.ClearSelection()
            ddlRedItem.ClearSelection()
            ddlRedItem.Items.Clear()
            ddlGivenItem.Items.Clear()
            ddlRedItem.Text = ""
            Me.ddlGivenItem.Text = ""
            Me.lblmsgPopUp.Text = ""
            Me.lblRule.Value = ""
            ddlRedItem.Enabled = True
            ddlGivenItem.Enabled = True
            txtRedQty.Text = ""
            txtGivenQty.Text = ""
            rsActive.Checked = True
            Me.StartTime.SelectedDate = Now.Date.AddDays(1)
            Me.EndTime.SelectedDate = Me.StartTime.SelectedDate.Value.AddMonths(1)
            Me.StartTime.Enabled = True
            Me.EndTime.Enabled = True
            Me.MPEAdd.VisibleOnPageLoad = True
            btnSave.Visible = True
            btnUpdate.Visible = False
            Panel.Update()

        Else
            MessageBoxValidation("Please select a organization", "Information")
            Exit Sub
        End If
    End Sub

  
  

   
    Private Sub ddlRedItem_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddlRedItem.ItemsRequested


        Dim Objrep As New SalesWorx.BO.Common.Reports()
        Dim dt As New DataTable
        dt = objProduct.LoadRedemptionProductList(Err_No, Err_Desc, Me.ddl_Org.SelectedValue, e.Text)


        Dim ItemsPerRequest As Integer = 100 'Set the number of values to show
        Dim itemOffset As Integer = e.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
        e.EndOfItems = endOffset = dt.Rows.Count

        'Loop through the values to populate the combo box
        For i As Integer = itemOffset To endOffset - 1
            Dim item As New RadComboBoxItem()
            item.Text = dt.Rows(i).Item("CodeText").ToString
            item.Value = dt.Rows(i).Item("CodeValue").ToString

            ddlRedItem.Items.Add(item)
            item.DataBind()
        Next

    End Sub

    Private Sub ddlGivenItem_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddlGivenItem.ItemsRequested


        Dim Objrep As New SalesWorx.BO.Common.Reports()
        Dim dt As New DataTable
        dt = objProduct.LoadRedemptionProductList(Err_No, Err_Desc, Me.ddl_Org.SelectedValue, e.Text)


        Dim ItemsPerRequest As Integer = 100 'Set the number of values to show
        Dim itemOffset As Integer = e.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
        e.EndOfItems = endOffset = dt.Rows.Count

        'Loop through the values to populate the combo box
        For i As Integer = itemOffset To endOffset - 1
            Dim item As New RadComboBoxItem()
            item.Text = dt.Rows(i).Item("CodeText").ToString
            item.Value = dt.Rows(i).Item("CodeValue").ToString

            ddlGivenItem.Items.Add(item)
            item.DataBind()
        Next

    End Sub


   
End Class





