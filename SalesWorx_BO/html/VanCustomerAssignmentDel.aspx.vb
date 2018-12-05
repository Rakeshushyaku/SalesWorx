Imports SalesWorx.BO.Common
Imports Telerik.Web.UI
Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports ExcelLibrary.SpreadSheet
Imports System.Resources
Imports Microsoft.Ajax.Utilities
Imports SalesWorx.BO.DAL


Public Class VanCustomerAssignmentDel
    Inherits System.Web.UI.Page

    Dim objcommon As New SalesWorx.BO.Common.Common
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim objCustomer As New SalesWorx.BO.Common.Customer
    Dim objDivConfig As New DivConfig
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim OrgID As String
    Dim CustomerNo As String
    Dim CustomerId As String
    Dim SiteID As String
    Dim CustomerName As String
    Private Const PageID As String = "P387"
    Private dt As New DataTable
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objControl As New AppControl
    Dim ErrorResource As ResourceManager
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        Try
            If Not IsPostBack Then
                'Dim HasPermission As Boolean = False
                'ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
                'If Not HasPermission Then
                '    Err_No = 500
                '    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
                'End If

                LoadOrg()
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
    Sub LoadVan()
        Try
            If Not (ddlOrg.SelectedItem.Value = "0") Then
                Dim objUserAccess As UserAccess
                objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
                objcommon = New SalesWorx.BO.Common.Common()
                ddlVan.DataSource = objcommon.GetVanByOrg(Err_No, Err_Desc, ddlOrg.SelectedValue, objUserAccess.UserID)
                ddlVan.DataValueField = "SalesRep_ID"
                ddlVan.DataTextField = "SalesRep_Name"
                ddlVan.DataBind()

            End If
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
        Try
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            objcommon = New SalesWorx.BO.Common.Common()
            ddlCustomer.DataSource = objCustomer.GetCustomerListfromOrg(Err_No, Err_Desc, ddlOrg.SelectedValue)
            ddlCustomer.DataValueField = "CustomerID"
            ddlCustomer.DataTextField = "Customer"
            ddlCustomer.DataBind()
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
    Sub LoadOrg()
        Dim objCommon As New Common
        Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
        ddlOrg.DataSource = objCustomer.GetOrganisation(Err_No, Err_Desc, SubQry)
        ddlOrg.Items.Clear()
        ddlOrg.Items.Add(New RadComboBoxItem("Select Organization", "0"))
        ddlOrg.AppendDataBoundItems = True
        ddlOrg.DataValueField = "MAS_Org_ID"
        ddlOrg.DataTextField = "Description"
        ddlOrg.DataBind()
    End Sub
    Protected Sub ddlOrg_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrg.SelectedIndexChanged

        If ddlOrg.SelectedItem.Value = 0 Then
            ddlCustomer.ClearSelection()
            ddlCustomer.Items.Clear()
            ddlCustomer.Text = ""
            ddlCustomer.ClearSelection()
            ddlCustomer.Items.Clear()
            ddlCustomer.Text = ""
        End If
        Panel.Update()

    End Sub

    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If Me.ddlOrg.SelectedIndex > 0 Then
            ddlCustomer.ClearSelection()
            ddlVan.ClearSelection()
            Me.lblmsgPopUp.Text = ""
            Me.lblRule.Value = ""
            Me.MPEAdd.VisibleOnPageLoad = True
            btnSave.Visible = True
            btnUpdate.Visible = False
            Panel.Update()
            LoadVan()


        Else
            MessageBoxValidation("Please select a organization", "Information")
            Exit Sub
        End If
    End Sub

    Protected Sub gvRules_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles gvRules.ItemCommand

        Try
            If e.CommandName = "DeleteSelected" Then
                Dim item As GridDataItem = DirectCast(e.Item, GridDataItem)
                Dim RuleID As Label = DirectCast(item.FindControl("lblRuleID"), Label)


                'If objProduct.DeleteRedemptionItem(Err_No, Err_Desc, RuleID.Text) Then
                '    MessageBoxValidation("Successfully deleted", "Information")
                '    gvRules.Rebind()
                'Else
                '    MessageBoxValidation("Error while deleting", "Validation")
                '    Exit Sub
                'End If
            End If



        Catch ex As Exception
            Err_No = "64224"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("PHX_006") & "&next=VanCustomerAssignment.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub

    Private Sub btnImportWindow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportWindow.Click
        If Me.ddlOrg.SelectedIndex > 0 Then
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

            If Me.ddlOrg.SelectedIndex > 0 Then
                Dim dtOriginal As New DataTable()


                dtOriginal = objCustomer.ExportCustVanMap(Err_No, Err_Desc, ddlOrg.SelectedItem.Value)

                Dim dtTemp As New DataTable()

                'Creating Header Row


                dtTemp.Columns.Add("Customer_No")
                dtTemp.Columns.Add("Site_Use_ID")
                dtTemp.Columns.Add("Van_Org_ID")
                Dim drAddItem As DataRow
                For i As Integer = 0 To dtOriginal.Rows.Count - 1
                    drAddItem = dtTemp.NewRow()
                    drAddItem(0) = CLng(IIf(dtOriginal.Rows(i)("Customer_No") Is DBNull.Value, "0", dtOriginal.Rows(i)("Customer_No").ToString()))
                    drAddItem(1) = IIf(dtOriginal.Rows(i)("Site_Use_ID") Is DBNull.Value, "0", dtOriginal.Rows(i)("Site_Use_ID").ToString())
                    drAddItem(2) = CLng(IIf(dtOriginal.Rows(i)("Van_Org_ID") Is DBNull.Value, "0", dtOriginal.Rows(i)("Van_Org_ID").ToString()))

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
                    Dim fn As String = "Customer Van Map" + Now.ToString("ddMMMyyHHmmss") + ".xls"
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
                        col.ColumnName = "CustomerId"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)


                        col = New DataColumn
                        col.ColumnName = "CustomerName"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)



                        col = New DataColumn
                        col.ColumnName = "Site_Use_Id"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)


                        col = New DataColumn
                        col.ColumnName = "Van_Org_Id"
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

                        If TempTbl.Columns.Count = 7 Then
                            If Not (TempTbl.Columns(0).ColumnName.ToLower = "CustomerId" And TempTbl.Columns(1).ColumnName.ToLower = "CustomerName" And TempTbl.Columns(2).ColumnName.ToLower = "Site_Use_Id" And TempTbl.Columns(3).ColumnName.ToLower = "Van_Org_Id") Then
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
                                Dim CustomerId As String = Nothing
                                Dim Site_Use_ID As String = Nothing
                                Dim Van_Org_ID As String = Nothing
                                Dim isValidRow As Boolean = True


                                CustomerId = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, "0", TempTbl.Rows(idx)(0).ToString())
                                Site_Use_ID = IIf(TempTbl.Rows(idx)(2) Is DBNull.Value, "0", TempTbl.Rows(idx)(1).ToString())
                                Van_Org_ID = IIf(TempTbl.Rows(idx)(3) Is DBNull.Value, "0", TempTbl.Rows(idx)(2).ToString())



                                'If CustomerId <> "0" Then
                                '    If objCustomer.CheckCustomerIdExists(Err_No, Err_Desc, ddlCustomer.SelectedValue, ddlOrg.SelectedValue) = False Then
                                '        RowNo = idx + 2
                                '        ErrorText = ErrorText + "Invalid CustomerId" + ","
                                '        isValidRow = False
                                '        TotFailed += 1
                                '    End If
                                'End If




                                If Site_Use_ID <> "0" Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Invalid Site_Use_ID" + ","
                                    isValidRow = False
                                    TotFailed += 1
                                End If

                                If Van_Org_ID <> "0" Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Invalid Van_Org_ID" + ","
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

                                    If objCustomer.SaveCustVanMap(Err_No, Err_Desc, ddlCustomer.SelectedValue, ddlOrg.SelectedValue, ddlVan.SelectedValue, 0, 0) = True Then
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

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If String.IsNullOrEmpty(Me.ddlVan.SelectedValue) Then
            lblmsgPopUp.Text = "Please select the Van"
            MPEAdd.VisibleOnPageLoad = True
            Return
        End If

        If (Me.ddlCustomer.CheckedItems.Count = 0) Then
            lblmsgPopUp.Text = "Please select the Customer"
            MPEAdd.VisibleOnPageLoad = True
            Return
        End If

        Dim success As Boolean = False

        Try

            Dim itemschecked As Integer = ddlCustomer.CheckedItems.Count
            Dim ServiceTypesArray() As String = New String((itemschecked) - 1) {}

            Dim i As Integer = 0
            For Each item In ddlCustomer.CheckedItems
                Dim value As String = item.Value
                ServiceTypesArray(i) = value
                'store it in the array
                i = (i + 1)

                If objCustomer.SaveCustVanMap(Err_No, Err_Desc, value, ddlOrg.SelectedValue, ddlVan.SelectedValue, 0, 0) = True Then
                    MessageBoxValidation("Successfully Saved.", "Information")
                Else
                    MessageBoxValidation("Could not be saved.", "Information")

                    log.Error(Err_Desc)
                End If
            Next

        Catch ex As Exception
            Err_No = "74205"
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

        If String.IsNullOrEmpty(Me.ddlVan.SelectedValue) Then
            lblmsgPopUp.Text = "Please select the Van ID"
            MPEAdd.VisibleOnPageLoad = True
            Return
        End If

        'If objCustomer.CheckCustVanExists(Err_No, Err_Desc, ddlCustomer.SelectedValue, ddlVan.SelectedValue, ddlOrg.SelectedValue) Then
        '    lblmsgPopUp.Text = "Same rule already exists"
        '    Me.MPEAdd.VisibleOnPageLoad = True
        '    Return
        'End If


        Dim success As Boolean = False
        Try



            If objCustomer.UpdateCustomerVanData(Err_No, Err_Desc, Session("CustomerNo").ToString, ddlVan.SelectedValue, ddlOrg.SelectedValue) = True Then
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

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try

            Dim btn1 As ImageButton = TryCast(sender, ImageButton)
            Dim item As GridDataItem = DirectCast(btn1.NamingContainer, GridDataItem)
            Dim Customer_No As Label = DirectCast(item.FindControl("lblCustomerID"), Label)
            Dim Customer_Name As Label = DirectCast(item.FindControl("lblCustomerName"), Label)
            Dim Site_Use_ID As Label = DirectCast(item.FindControl("lblSiteID"), Label)
            Dim Van_Org_ID As Label = DirectCast(item.FindControl("lblVan"), Label)


            LoadVan()

            'Loop through the values to populate the combo box
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim item1 As New RadComboBoxItem()
                item1.Text = dt.Rows(i).Item("Van_Org_ID").ToString
                item1.Value = dt.Rows(i).Item("Van_Org_ID").ToString
                ddlVan.Items.Add(item1)
                item1.DataBind()
            Next

            Dim dt1 As New DataTable
            dt1 = objCustomer.LoadCustVanMap(Err_No, Err_Desc, Me.ddlOrg.SelectedValue)

            RadTextBox1.Visible = True
            RadTextBox1.Text = Customer_No.Text
            Session("CustomerNo") = Customer_No.Text
            RadTextBox1.Enabled = False
            ddlCustomer.Visible = False
            lblCustomerId.Visible = False
            ' lblcustomerNo.Visible = True


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
End Class