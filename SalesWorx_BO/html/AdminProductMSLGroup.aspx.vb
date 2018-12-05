Imports log4net
Imports SalesWorx.BO.Common
Imports Telerik.Web.UI
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports ExcelLibrary.SpreadSheet
Partial Public Class AdminProductMSLGroup
    Inherits System.Web.UI.Page

    Private dtErrors As New DataTable
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const ModuleName As String = "AdminProductMSLGroup.aspx"
    Private Const PageID As String = "P400"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim objProduct As New Product
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session.Item("USER_ACCESS") Is Nothing Then
            Session.Add("BringmeBackHere", ModuleName)
            Response.Redirect("Login.aspx", False)
            Exit Sub
        End If
        If Not IsPostBack Then
            If Not HasAuthentication() Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If

            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            If Not HasPermission Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If

            Dim objCommon As New Common
            Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
            ddl_org.ClearSelection()
            ddl_org.Items.Clear()

            ddl_org.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
            ddl_org.DataValueField = "MAS_Org_ID"
            ddl_org.DataTextField = "Description"
            ddl_org.DataBind()
            ddl_org.Items.Insert(0, New RadComboBoxItem("Select Organisation", 0))

            lstDefault.Items.Clear()
            lstSelected.Items.Clear()


            If Request.QueryString("Desc").ToString() = "New" Then
                Me.lblGroupId.Text = "0"
                Me.lblGroupName.Text = "0"
                Me.ddl_org.Enabled = True
            Else
                Me.txtGroupName.Text = Request.QueryString("Desc").ToString()
                Me.lblGroupName.Text = Me.txtGroupName.Text
                Me.lblGroupId.Text = Request.QueryString("PGID").ToString()
                Me.ddl_org.SelectedValue = Request.QueryString("ORGID").ToString()
                If Me.ddl_org.SelectedIndex > 0 Then
                    Me.ddl_org.Enabled = False
                Else
                    Me.ddl_org.Enabled = True
                End If

                BindDefault()
                BindSelected()

            End If

        Else


            MPEImport.VisibleOnPageLoad = False
        End If

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

    Private Sub btnImportWindow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportWindow.Click

        If ValidationPage() = True Then
            MessageBoxValidation("Group name already exist", "Validation")
            ClassUpdatePnl.Update()
            Exit Sub
        End If

        If Me.ddl_org.SelectedIndex > 0 And Me.txtGroupName.Text <> "" And Me.txtGroupName.Text <> "0" Then
            Me.lblUpMsg.Text = ""
            Session("dtMSLGroup") = Nothing
            dgvErros.DataSource = Nothing
            dgvErros.DataBind()
            dgvErros.Visible = False
            lbLog.Visible = False
            Me.MPEImport.VisibleOnPageLoad = True
        Else
            MessageBoxValidation("Please select a organization and enter group name", "Information")
            Exit Sub
        End If
    End Sub
    Private Function HasAuthentication() As Boolean
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
    End Function
    Protected Sub btndownloadTemplate_Click(sender As Object, e As EventArgs) Handles btndownloadTemplate.Click

        Dim Filename As String = System.Configuration.ConfigurationManager.AppSettings("ExcelTemplatePath") & "ProductMSLGroup_Info.xls"
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
    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try

            If Me.ddl_org.SelectedIndex > 0 And Me.txtGroupName.Text <> "" Then
                Dim dtOriginal As New DataTable()


                dtOriginal = objProduct.ExportMSLGroup(Err_No, Err_Desc, ddl_org.SelectedValue, IIf(Me.lblGroupId.Text = "", "0", lblGroupId.Text))

                Dim dtTemp As New DataTable()

                'Creating Header Row


                dtTemp.Columns.Add("Item_Code")
                dtTemp.Columns.Add("Description")

                Dim drAddItem As DataRow
                For i As Integer = 0 To dtOriginal.Rows.Count - 1
                    drAddItem = dtTemp.NewRow()

                    drAddItem(0) = dtOriginal.Rows(i)("Item_Code").ToString()
                    drAddItem(1) = dtOriginal.Rows(i)("Description").ToString()
                    dtTemp.Rows.Add(drAddItem)
                Next

                If dtOriginal.Rows.Count = 0 Then

                    MessageBoxValidation("There is no data.", "Information")
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
                    Dim fn As String = "MSLGroup" + Now.ToString("ddMMMyyHHmmss") + ".xls"
                    Dim d As New DataSet
                    d.Tables.Add(dtTemp)

                    ExportToExcel(fn, d)

                End If
            Else
                MessageBoxValidation("Please select a organization and group name", "Information")
                Exit Sub
            End If

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("information.aspx?mode=1&errno=" & "74071" & "&msg=" & AppMsgHandler.GetErrorMessage("SWX_BO_Msg_005") & "&next=welcome.aspx", False)
        Finally
        End Try
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


        Session("dtMSLGroup") = Nothing
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
                        col.ColumnName = "Item_Code"
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

                        If TempTbl.Columns.Count = 1 Then
                            If Not (TempTbl.Columns(0).ColumnName.ToLower = "item_code") Then
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
                        Dim IsDeleted As Boolean = False
                        If TempTbl.Rows.Count > 0 Then
                            Dim idx As Integer

                            For idx = 0 To TempTbl.Rows.Count - 1

                                Dim Item As String = Nothing

                                Dim OrgId As String = Me.ddl_org.SelectedValue

                                Dim isValidRow As Boolean = True


                                Item = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, "0", TempTbl.Rows(idx)(0).ToString())

                                If Item = "0" Then
                                    Continue For
                                End If
                               
                                
                                If objProduct.CheckProductValid("tblProduct", Item, OrgId, "0", Err_No, Err_Desc) = False Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Invalid Item Code" + ","
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
                                    If Item <> "0" And OrgId <> "0" Then
                                        objProduct.InsertProductMSLGroup(Me.ddl_org.SelectedItem.Value, 0, Item, Me.lblGroupId.Text, Me.txtGroupName.Text, Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, IsDeleted, "Single")
                                        GetGroupName()
                                        st = True
                                    End If
                                End If
                                ErrorText = Nothing
                                RowNo = Nothing
                            Next
                        End If


                        DeleteExcel()
                        BindSelected()
                        BindDefault()
                        lblUpMsg.Text = IIf(TotFailed = 0, "Successfully uploaded", "One or more rows has invalid data. Please check the log")
                        Me.MPEImport.VisibleOnPageLoad = True

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
                    Session.Remove("dtMSLGroup")
                    Session("dtMSLGroup") = dtErrors.Copy


                    Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & "MSLGroupLog_" & Now.ToString("yyyyMMdd") + ".txt"
                    DataTable2CSV(dtErrors, fn, vbTab)
                    If dtErrors.Rows.Count > 0 Then
                        lbLog.Visible = True
                    End If
                    Session.Remove("MSLLogInfo")
                    Session("MSLLogInfo") = fn




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


            If Not Session("MSLLogInfo") Is Nothing Then
                Dim fileValue As String = Session("MSLLogInfo")





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

    Private Sub ddOraganisation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_org.SelectedIndexChanged
        'Try
        If ddl_org.SelectedIndex > 0 Then
            BindDefault()


            BindSelected()


        Else
            lstDefault.Items.Clear()
            lstSelected.Items.Clear()
            lblProdAssign.Text = ""
            lblProdAvailed.Text = ""
        End If

    End Sub




    Private Sub BindDefault()
        Me.lblSelectedID.Text = ""
        Me.lblRemovedID.Text = ""
        Dim objProd As New Product
        Dim TempTbl As New DataTable
        Try
            Err_Desc = Nothing
            Err_No = Nothing
            TempTbl = objProd.GetDefaultProdctMSLGroup(IIf(Me.ddl_org.SelectedIndex <= 0, "0", Me.ddl_org.SelectedItem.Value), Me.lblGroupId.Text, Err_No, Err_Desc)
            If Me.txtFilter.Text <> "" Then
                TempTbl.DefaultView.RowFilter = "(Description LIKE '%" & Me.txtFilter.Text & "%')"
            End If

            Dim dv As New DataView
            dv = TempTbl.DefaultView
            dv.Sort = "Description"
            If TempTbl IsNot Nothing Then
                lstDefault.DataSource = dv
                lstDefault.DataTextField = "Description"
                lstDefault.DataValueField = "Inventory_Item_ID"
                lstDefault.DataBind()
            End If
            lblProdAvailed.Text = "Products Available: [" & lstDefault.Items.Count & "]"
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        Finally
            objProd = Nothing
        End Try
    End Sub
    Private Function ValidationPage() As Boolean
        Dim success As Boolean = False
        Dim objProd As New Product
        Dim r As DataRow = Nothing
        Dim GroupName As String = Me.txtGroupName.Text
        r = objProd.CheckProductMSLGroup(GroupName, Err_No, Err_Desc)
        If Not r Is Nothing Then
            ' Me.lblGroupId.Text = r(0).ToString()
            'Me.txtGroupName.Text = r(1).ToString()
            If Me.lblGroupName.Text <> r(1).ToString() Then
                success = True
            End If
        End If
        Return success

    End Function
    Protected Sub imgAddSlected_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Me.ddl_org.SelectedIndex > 0 Then

            If Me.txtGroupName.Text <> "" And Me.txtGroupName.Text <> "0" Then

                If ValidationPage() = True Then
                    MessageBoxValidation("Group name already exist", "Validation")
                    ClassUpdatePnl.Update()

                    Exit Sub
                End If
                Try
                    Err_No = Nothing
                    Err_Desc = Nothing


                    Dim objProd As New Product
                    For Each Item As RadListBoxItem In lstDefault.Items
                        If Item.Selected Then
                            If Item.Value <> "0" Then
                                objProd.InsertProductMSLGroup(Me.ddl_org.SelectedItem.Value, Item.Value, 0, Me.lblGroupId.Text, Me.txtGroupName.Text, Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, True, "Single")
                                GetGroupName()
                            End If
                        End If
                    Next
                    'GetGroupName()
                    BindDefault()
                    BindSelected()
                Catch ex As Exception
                    log.Error(ex.Message)
                    Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Product List Setup_006") & "&next=AssignProductToFSR.aspx&Title=Assign Product To FSR", False)
                End Try
            Else
                MessageBoxValidation("Please enter a group name", "Validation")

                ClassUpdatePnl.Update()

            End If
        Else
            MessageBoxValidation("Please select a organization", "Validation")
            ClassUpdatePnl.Update()

        End If

    End Sub
    Private Sub GetGroupName()
        Dim objProd As New Product
        Dim r As DataRow = Nothing
        Dim GroupName As String = Me.txtGroupName.Text
        r = objProd.CheckProductMSLGroup(GroupName, Err_No, Err_Desc)
        If Not r Is Nothing Then
            Me.lblGroupId.Text = r(0).ToString()
            Me.txtGroupName.Text = r(1).ToString()
            Me.lblGroupName.Text = r(1).ToString()
        End If
    End Sub
    Protected Sub imgRemoveSlected_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Me.ddl_org.SelectedIndex > 0 Then
            If Me.txtGroupName.Text <> "" And Me.txtGroupName.Text <> "0" Then
                If ValidationPage() = True Then
                    MessageBoxValidation("Group name already exist", "Validation")
                    ClassUpdatePnl.Update()
                    Exit Sub
                End If
                Try
                    Err_No = Nothing
                    Err_Desc = Nothing
                    Dim objProd As New Product

                    For Each Item As RadListBoxItem In lstSelected.Items
                        If Item.Selected Then
                            objProd.DeleteProductMSLGroup("Single", ddl_org.SelectedItem.Value, Item.Value, 0, Me.lblGroupId.Text, Err_No, Err_Desc, txtGroupName.Text)
                            'bAdded = True
                            'ValidationPage()
                        End If
                    Next
                    BindDefault()
                    BindSelected()
                Catch ex As Exception
                    log.Error(ex.Message)
                    Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_AdminProductGroup_006") & "&next=AdminProductGroup.aspx&Title=Product Group", False)

                End Try
            Else
                MessageBoxValidation("Please enter a group name", "Validation")
                ClassUpdatePnl.Update()

            End If
        Else
            MessageBoxValidation("Please select a organization", "Validation")
            ClassUpdatePnl.Update()
            Exit Sub
        End If
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub imgMoveAllLeft_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Me.ddl_org.SelectedIndex > 0 Then
            If Me.txtGroupName.Text <> "0" And Me.txtGroupName.Text <> "" Then
                If ValidationPage() = True Then
                    Me.txtGroupName.Text = ""
                    MessageBoxValidation("Group name already exist", "Validation")
                    ClassUpdatePnl.Update()
                    Exit Sub
                End If
                Try
                    Err_No = Nothing
                    Err_Desc = Nothing
                    Dim objProd As New Product
                    If Me.txtFilter.Text <> "" Then

                        For Each Item As RadListBoxItem In lstDefault.Items
                            Item.Selected = True
                        Next

                        For Each Item As RadListBoxItem In lstDefault.Items
                            If Item.Selected = True Then
                                If Item.Value <> "0" Then
                                    objProd.InsertProductMSLGroup(Me.ddl_org.SelectedItem.Value, Item.Value, 0, Me.lblGroupId.Text, Me.txtGroupName.Text, Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, True, "Single")
                                End If
                            End If
                        Next
                        GetGroupName()
                    Else
                        objProd.InsertProductMSLGroup(Me.ddl_org.SelectedItem.Value, 0, 0, Me.lblGroupId.Text, Me.txtGroupName.Text, Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, True, "ALL")
                        GetGroupName()
                    End If


                 
                    BindDefault()
                    BindSelected()
                Catch ex As Exception
                    log.Error(ex.Message)
                    Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_AdminMSL_005") & "&next=AdminProductGroup.aspx&Title=Must Stock List", False)
                End Try
            Else
                MessageBoxValidation("Please enter a group name", "Validation")
                ClassUpdatePnl.Update()

            End If
        Else
            MessageBoxValidation("Please select a organization.", "Validation")
            ClassUpdatePnl.Update()
            Exit Sub
        End If

    End Sub
    Protected Sub imgMoveAllRight_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Me.ddl_org.SelectedIndex > 0 Then
            If Me.txtGroupName.Text <> "" And Me.txtGroupName.Text <> "0" Then
                If ValidationPage() = True Then
                    MessageBoxValidation("Group name already exist", "Validation")
                    ClassUpdatePnl.Update()
                    Exit Sub
                End If
                Try
                    Err_No = Nothing
                    Err_Desc = Nothing

                    ' Dim objProd As New Product
                    'For Each Item As RadListBoxItem  In lstSelected.Items
                    '  objProd.DeleteProductMSLGroup("ALL", ddl_org.SelectedItem.Value, 0, 0, Me.lblGroupId.Text, Err_No, Err_Desc, txtGroupName.Text)
                    'bAdded = True
                    'ValidationPage()
                    'Next

                    Dim objProd As New Product
                    If Me.txtFilter.Text <> "" Then

                        For Each Item As RadListBoxItem In lstSelected.Items
                            Item.Selected = True
                        Next

                        For Each Item As RadListBoxItem In lstSelected.Items
                            If Item.Selected = True Then
                                If Item.Value <> "0" Then
                                    objProd.DeleteProductMSLGroup("Single", ddl_org.SelectedItem.Value, Item.Value, 0, Me.lblGroupId.Text, Err_No, Err_Desc, txtGroupName.Text)
                                End If
                            End If
                        Next

                    Else
                      objProd.DeleteProductMSLGroup("ALL", ddl_org.SelectedItem.Value, 0, 0, Me.lblGroupId.Text, Err_No, Err_Desc, txtGroupName.Text)
                    End If


                    BindDefault()
                    BindSelected()
                Catch ex As Exception
                    log.Error(ex.Message)
                    Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_AdminMSL_006") & "&next=AdminProductGroup.aspx&Title=Must Stock List", False)

                End Try
            Else
                MessageBoxValidation("Please enter a group name", "Validation")
                ClassUpdatePnl.Update()

            End If
        Else
            MessageBoxValidation("Please select a organization", "Validation")
            ClassUpdatePnl.Update()
            Exit Sub

        End If
    End Sub
    Private Sub BindSelected()
        Me.lblSelectedID.Text = ""
        Me.lblRemovedID.Text = ""
        Dim objProd As New Product
        Dim TempTbl As New DataTable
        Try
            Err_Desc = Nothing
            Err_No = Nothing

            TempTbl = objProd.GetSelectedProductMSLGroup(IIf(Me.ddl_org.SelectedIndex <= 0, "0", Me.ddl_org.SelectedItem.Value), Me.lblGroupId.Text, Err_No, Err_Desc)
            If Me.txtFilter.Text <> "" Then
                TempTbl.DefaultView.RowFilter = "(Description LIKE '%" & Me.txtFilter.Text & "%')"
            End If

            Dim dv As New DataView
            dv = TempTbl.DefaultView
            'dv.Sort = "Description"
            If TempTbl IsNot Nothing Then
                lstSelected.DataSource = dv
                lstSelected.DataTextField = "Description"
                lstSelected.DataValueField = "Inventory_Item_ID"
                lstSelected.DataBind()
            End If

            Dim ItmSelected As Integer = 0
            For Each Item As RadListBoxItem In lstSelected.Items
                If Item.Value <> "0" Then
                    ItmSelected += 1
                End If
            Next


            lblProdAssign.Text = "Products Assigned: [" & ItmSelected & "]"
            lblProdAvailed.Text = "Products Available: [" & lstDefault.Items.Count & "]"
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        Finally
            objProd = Nothing
        End Try
    End Sub

    Private Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        If Me.ddl_org.SelectedIndex > 0 Then
            BindDefault()
            BindSelected()
        Else
            MessageBoxValidation("Please select a organization", "Validation")
            ClassUpdatePnl.Update()
            Exit Sub
        End If
    End Sub

    Private Sub Btn_back_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_back.Click
        Response.Redirect("ProductMSLGroupListing.aspx")
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReset.Click
        If Me.ddl_org.SelectedIndex > 0 Then
            txtFilter.Text = ""
            BindDefault()
            BindSelected()
        Else
            MessageBoxValidation("Please select a organization", "Validation")
            ClassUpdatePnl.Update()
            Exit Sub
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
End Class