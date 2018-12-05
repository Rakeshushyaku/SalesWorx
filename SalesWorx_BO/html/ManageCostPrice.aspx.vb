
Imports SalesWorx.BO.Common

Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports ExcelLibrary.SpreadSheet
Imports Telerik.Web.UI
Partial Public Class ManageCostPrice
    Inherits System.Web.UI.Page
    Dim objProduct As New Product

    Private dtErrors As New DataTable
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const PageID As String = "P366"
    Private dt As New DataTable
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objControl As New AppControl


    Protected Sub Button_Click(sender As Object, e As ImageClickEventArgs)
        gvSKU.MasterTableView.FilterExpression = String.Empty
        gvSKU.MasterTableView.Columns(0).CurrentFilterValue = ""
        gvSKU.MasterTableView.Columns(2).CurrentFilterValue = ""
        gvSKU.MasterTableView.Columns(3).CurrentFilterValue = ""
        gvSKU.MasterTableView.Rebind()
    End Sub
    



    Protected Sub ddl_org_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_org.SelectedIndexChanged


        gvSKU.Rebind()

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


                ViewState("FileType") = Nothing
                ViewState("FileName") = Nothing
                ViewState("CSVName") = Nothing
                Session.Remove("CostPriceLogInfo")
                Session.Remove("dtCostPriceErrors")
                SetErrorsTable()
            Else
                dtErrors = Session("dtCostPriceErrors")
                Me.MPEImport.VisibleOnPageLoad = False
            End If


        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_005") & "&next=Welcome.aspx&Title=Cost Price", False)
        End Try
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    
    Sub LoadOrgHeads()
        Dim objCommon As New Common
        Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
        ddl_org.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
        ddl_org.Items.Clear()
        ddl_org.Items.Add(New RadComboBoxItem("Select Organization"))
        ddl_org.AppendDataBoundItems = True
        ddl_org.DataValueField = "MAS_Org_ID"
        ddl_org.DataTextField = "Description"
        ddl_org.DataBind()

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
    Protected Sub btnImportSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportSave.Click
        If Me.ExcelFileUpload.FileName = Nothing Then
            ' Me.lblinfo.Text = "Validation"
            Me.lblUpMsg.Text = "Select filename "
            ' Me.lblMessage.ForeColor = Drawing.Color.Green
            ' Me.MpInfoError.Show()
            Me.MPEImport.VisibleOnPageLoad = True
            Exit Sub
        End If

        Dim Str As New StringBuilder
        dtErrors = Session("dtCostPriceErrors")
        Dim TotSuccess As Integer = 0
        Dim TotFailed As Integer = 0
        If dtErrors.Rows.Count > 0 Then
            dtErrors.Rows.Clear()
            Me.dgvErros.DataSource = dtErrors
            Me.dgvErros.DataBind()
        End If
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
                        col.ColumnName = "CostPrice"
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

                        If TempTbl.Columns.Count = 2 Then

                            If Not (TempTbl.Columns(0).ColumnName = "ItemCode" And TempTbl.Columns(1).ColumnName = "CostPrice") Then
                                lblUpMsg.Text = "Please check the template columns are correct"

                                Me.MPEImport.VisibleOnPageLoad = True
                                Exit Sub
                            End If



                        Else
                            lblUpMsg.Text = "Invalid Template"
                            Me.MPEImport.VisibleOnPageLoad = True
                            Exit Sub
                        End If
                        TempTbl.Columns.Add("IsValid", GetType(String))




                        If TempTbl.Rows.Count = 0 Then
                            lblUpMsg.Text = "There is no data in the uploaded file."
                            ' lblMessage.ForeColor = Drawing.Color.Green
                            'lblinfo.Text = "Information"
                            'MpInfoError.Show()
                            Me.MPEImport.VisibleOnPageLoad = True
                            Exit Sub
                        End If

                        Dim RowNo As String = Nothing
                        ' Dim ColNo As String = Nothing
                        ' Dim ColumnName As String = Nothing
                        Dim ErrorText As String = Nothing
                        Dim OrgID As String = Me.ddl_org.SelectedValue



                        If TempTbl.Rows.Count > 0 Then



                            Dim idx As Integer

                            For idx = 0 To TempTbl.Rows.Count - 1

                                Dim itemCode As String = Nothing
                                Dim CostPrice As String = Nothing
                                
                                Dim isValidRow As Boolean = True


                                itemCode = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, "0", TempTbl.Rows(idx)(0).ToString())
                                CostPrice = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, "0", TempTbl.Rows(idx)(1).ToString())

                                If TempTbl.Rows(idx)(0) Is DBNull.Value Then
                                    TempTbl.Rows(idx)("IsValid") = "N"
                                    Continue For
                                End If



                                If itemCode = "0" Then
                                    TempTbl.Rows(idx)("IsValid") = "N"
                                    Continue For
                                End If

                                If objProduct.CheckItemCode(Err_No, Err_Desc, itemCode, OrgID) = False Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Invalid Item code" + ","
                                    TotFailed += 1
                                    isValidRow = False
                                End If

                                
                                If IsNumeric(CostPrice) = False Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Cost price should be in numeric" + ","
                                    TotFailed += 1
                                    isValidRow = False
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
                                    isValidRow = False
                                End If

                                If isValidRow = True Then
                                    TempTbl.Rows(idx)("IsValid") = "Y"

                                    TotSuccess = TotSuccess + 1
                                    Dim h As DataRow = dtErrors.NewRow()
                                    h("RowNo") = idx + 2
                                    h("LogInfo") = "Successfully uploaded"
                                    dtErrors.Rows.Add(h)
                                    RowNo = Nothing
                                    ErrorText = Nothing
                                    isValidRow = True
                                End If





                            Next
                        End If

                        If objProduct.UploadCostPrice(TempTbl, Me.ddl_org.SelectedValue, Err_No, Err_Desc) = True Then
                            DeleteExcel()
                            lblUpMsg.Text = IIf(TotFailed = 0, "Successfully uploaded", "One or more rows has invalid data. Please check the log")
                            MPEImport.VisibleOnPageLoad = True
                            gvSKU.Rebind()

                        Else
                            DeleteExcel()
                            lblUpMsg.Text = "Please check the uploaded log"

                            MPEImport.Visible = True
                            Exit Sub
                        End If
                    End If


                    Me.dgvErros.DataSource = dtErrors
                    Me.dgvErros.DataBind()
                    Session.Remove("dtCostPriceErrors")
                    Session("dtCostPriceErrors") = dtErrors


                    Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & "CostPrice_" & Now.ToString("yyyyMMdd") + ".txt"
                    DataTable2CSV(dtErrors, fn, vbTab)

                    Session.Remove("CostPriceLogInfo")
                    Session("CostPriceLogInfo") = fn




                Catch ex As Exception

                    Err_No = "13552"
                    If Err_Desc Is Nothing Then
                        log.Error(GetExceptionInfo(ex))
                    Else
                        log.Error(Err_Desc)
                    End If
                End Try


            Else
                ' Str.Append("<span style='font-color:red'> <b >Please import valid Excel template.</b></span>")
                lblUpMsg.Text = "Please import valid Excel template."
                MPEImport.VisibleOnPageLoad = True
            End If


        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try


    End Sub
    Protected Sub lbLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbLog.Click
        If Not Session("CostPriceLogInfo") Is Nothing Then
            Dim fileValue As String = Session("CostPriceLogInfo")





            Dim file As System.IO.FileInfo = New FileInfo(fileValue)

            If file.Exists Then

                Dim filePath As String = fileValue
                Response.ContentType = ContentType
                Response.AppendHeader("Content-Disposition", ("attachment; filename=" + Path.GetFileName(file.Name)))
                Response.WriteFile(filePath)
                Response.End()
                'Response.Clear()

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
    Private Sub SetErrorsTable()
        Dim col As DataColumn


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

        Session.Remove("dtCostPriceErrors")
        Session("dtCostPriceErrors") = dtErrors
    End Sub
    Protected Sub btnImportWindow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportWindow.Click
        If Me.ddl_org.SelectedIndex > 0 Then
            Me.lblUpMsg.Text = ""
            Me.MPEImport.VisibleOnPageLoad = True
        Else
            MessageBoxValidation("Please select a organization.", "Validation")
            Exit Sub
        End If
    End Sub


    Protected Sub Export_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click

        If Me.ddl_org.SelectedIndex > 0 Then
            Dim dtOriginal As New DataTable()
            Dim OrgID As String = Me.ddl_org.SelectedValue
            Dim Searchqry As String = " AND B.Organization_ID=" & OrgID

            Dim ds As New DataSet
            ds = objProduct.GetProductList(Err_No, Err_Desc, Searchqry, "")
            dtOriginal = ds.Tables(0)

            Dim dtTemp As New DataTable()

            'Creating Header Row
            dtTemp.Columns.Add("ItemCode")
            dtTemp.Columns.Add("CostPrice")
            
            Dim drAddItem As DataRow
            For i As Integer = 0 To dtOriginal.Rows.Count - 1
                drAddItem = dtTemp.NewRow()
                drAddItem(0) = dtOriginal.Rows(i)("Item_Code").ToString()
                drAddItem(1) = CDec(dtOriginal.Rows(i)("CostPrice").ToString())
                
                dtTemp.Rows.Add(drAddItem)
            Next

            If dtOriginal.Rows.Count = 0 Then

                MessageBoxValidation("There is no data for the selected filter criteria", "Information")
                Exit Sub

                drAddItem = dtTemp.NewRow()
                drAddItem(0) = ""
                drAddItem(1) = ""
                
                dtTemp.Rows.Add(drAddItem)
            End If

            'Temp(Grid)
            Dim dg As New DataGrid()
            dg.DataSource = dtTemp
            dg.DataBind()
            If dtTemp.Rows.Count > 0 Then
                'Dim fn As String = "MSL" & Now.ToString("ddMMMyyHHmmss") + ".xls"
                Dim fn As String = "ProductCostPrice" + Now.ToString("ddMMMyyHHmmss") + ".xls"
                Dim d As New DataSet
                d.Tables.Add(dtTemp)

                ExportToExcel(fn, d)

            End If
        Else
            MessageBoxValidation("Please select a organization", "Information")
            Exit Sub
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

    Protected Sub btnEdit_Click(sender As Object, e As ImageClickEventArgs)
        Try





            Dim btn1 As ImageButton = TryCast(sender, ImageButton)
            Dim item As GridDataItem = DirectCast(btn1.NamingContainer, GridDataItem)
            Dim ItemID As Label = DirectCast(item.FindControl("lblItemID"), Label)
            Dim CostPrice As RadNumericTextBox = DirectCast(item.FindControl("txtCostPrice"), RadNumericTextBox)


            If objProduct.UpdateCostPrice(ItemID.Text, IIf(CostPrice.Text = "" Or CostPrice.Text Is Nothing, "0", CostPrice.Text), Me.ddl_org.SelectedValue, Err_No, Err_Desc) = False Then
                MessageBoxValidation("Error while updating cost price", "Validation")
                Exit Sub
            Else
                Me.gvSKU.Rebind()
            End If



        Catch ex As Exception
            Err_No = "62521"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("PHX_006") & "&next=ManageCostPrice.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try


    End Sub
End Class





