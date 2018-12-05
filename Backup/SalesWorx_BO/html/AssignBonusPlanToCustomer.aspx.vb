

Imports SalesWorx.BO.Common
Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports ExcelLibrary.SpreadSheet

Partial Public Class AssignBonusPlanToCustomer
    Inherits System.Web.UI.Page
    Dim objProduct As New Product
    Dim ObjCustomer As New Customer
    Dim Err_No As Long
    Dim Err_Desc As String
    Private bAdded As Boolean
    Private Const PageID As String = "P220"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub AssignBonusPlanToCustomer_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Assign Bonus Plan To Customer"
    End Sub
    Protected Sub lbLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbLog.Click
        Try


            If Not Session("CustBnsLogInfo") Is Nothing Then
                Dim fileValue As String = Session("CustBnsLogInfo")





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
                    MPEImport.Show()
                    Exit Sub

                End If

            Else
                lblUpMsg.Text = "There is no log to show."
                'lblMessage.ForeColor = Drawing.Color.Green
                'lblinfo.Text = "Information"
                MPEImport.Show()
                Exit Sub

            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))

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
            Me.MPEImport.Show()
            Exit Sub
        End If
        Session("dtCustBonusPlan") = Nothing
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
                        col.ColumnName = "CustomerNo"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)

                        col = New DataColumn
                        col.ColumnName = "SiteID"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)

                        col = New DataColumn
                        col.ColumnName = "BonusPlanID"
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

                            If Not (TempTbl.Columns(0).ColumnName.ToLower = "customerno" And TempTbl.Columns(1).ColumnName.ToLower = "siteid" And TempTbl.Columns(2).ColumnName.ToLower = "bonusplanid") Then
                                lblUpMsg.Text = "Please check the template columns are correct"

                                Me.MPEImport.Show()
                                Exit Sub
                            End If



                        Else
                            lblUpMsg.Text = "Invalid Template"
                            '' lblMessage.ForeColor = Drawing.Color.Green
                            lblinfo.Text = "Information"
                            ' MpInfoError.Show()
                            Me.MPEImport.Show()
                            Exit Sub
                        End If

                        col = New DataColumn
                        col.ColumnName = "Customer_ID"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)

                        col = New DataColumn
                        col.ColumnName = "Site_use_ID"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)

                        TempTbl.Columns.Add("IsValid", GetType(String))

                        If TempTbl.Rows.Count = 0 Then
                            lblUpMsg.Text = "There is no data in the uploaded file."
                            ' lblMessage.ForeColor = Drawing.Color.Green
                            'lblinfo.Text = "Information"
                            'MpInfoError.Show()
                            Me.MPEImport.Show()
                            Exit Sub
                        End If

                        Dim RowNo As String = Nothing
                        ' Dim ColNo As String = Nothing
                        ' Dim ColumnName As String = Nothing
                        Dim ErrorText As String = Nothing
                        Dim OrgID As String = Me.ddOraganisation.SelectedValue



                        If TempTbl.Rows.Count > 0 Then



                            Dim idx As Integer

                            For idx = 0 To TempTbl.Rows.Count - 1

                                Dim CustomerNo As String = Nothing
                                Dim SiteID As String = Nothing
                                Dim BonusPlanID As String = Nothing

                                Dim isValidRow As Boolean = True


                                CustomerNo = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, "0", TempTbl.Rows(idx)(0).ToString())
                                SiteID = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, "0", TempTbl.Rows(idx)(1).ToString())

                                BonusPlanID = IIf(TempTbl.Rows(idx)(2) Is DBNull.Value, "0", TempTbl.Rows(idx)(2).ToString())



                                Dim Customer_ID As String = ""


                                If CustomerNo = "0" Or SiteID = "0" Or BonusPlanID = "0" Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + " Blank Values "
                                    TotFailed += 1
                                    isValidRow = False
                                Else
                                    If ObjCustomer.CheckOrgShipCustomerNoExist(OrgID, CustomerNo, Customer_ID, SiteID) = False Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Invalid Customer No." + ","
                                        TotFailed += 1
                                        isValidRow = False
                                    Else
                                        TempTbl.Rows(idx)("Customer_ID") = Customer_ID
                                        TempTbl.Rows(idx)("Site_use_ID") = SiteID
                                    End If


                                    If IsNumeric(BonusPlanID) = False Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Bonus plan id should be numeric" + ","
                                        TotFailed += 1
                                        isValidRow = False
                                    End If
                                    If ObjCustomer.CheckBonusPlanIsValid(Err_No, Err_Desc, OrgID, BonusPlanID) = False Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Bonus plan id is invalid." + ","
                                        TotFailed += 1
                                        isValidRow = False
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

                        If ObjCustomer.UploadCustBonusPlan(TempTbl, Me.ddOraganisation.SelectedValue, Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID.ToString()) = True Then
                            DeleteExcel()
                            lblUpMsg.Text = IIf(TotFailed = 0, "Successfully uploaded", "One or more rows has invalid data. Please check the log")
                            MPEImport.Show()
                            BindSelected()
                            BindDefault()

                        Else
                            DeleteExcel()
                            lblUpMsg.Text = "Please check the uploaded log"
                            MPEImport.Show()
                            Exit Sub
                        End If
                    End If
                    dgvErros.Visible = False
                    If dtErrors.Rows.Count > 0 Then
                        lbLog.Visible = True
                    Else
                        lbLog.Visible = False
                    End If
                    Me.dgvErros.DataSource = dtErrors
                    Me.dgvErros.DataBind()

                    Session.Remove("dtCustBonusPlan")
                    Session("dtCustBonusPlan") = dtErrors.Copy


                    Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & "CustBonusPlan_" & Now.ToString("yyyyMMdd") + ".txt"
                    DataTable2CSV(dtErrors, fn, vbTab)
                    If dtErrors.Rows.Count > 0 Then
                        lbLog.Visible = True
                    End If
                    Session.Remove("CustBnsLogInfo")
                    Session("CustBnsLogInfo") = fn


                    dtErrors = Nothing

                Catch ex As Exception

                    Err_No = "43552"
                    If Err_Desc Is Nothing Then
                        log.Error(GetExceptionInfo(ex))
                    Else
                        log.Error(Err_Desc)
                    End If
                End Try


            Else
                ' Str.Append("<span style='font-color:red'> <b >Please import valid Excel template.</b></span>")
                lblMessage.Text = "Please import valid Excel template."
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
            End If


        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try

            If Me.ddOraganisation.SelectedIndex > 0 Then
                Dim dtOriginal As New DataTable()
                Dim OrgID As String = Me.ddOraganisation.SelectedValue

                dtOriginal = ObjCustomer.ExportCustomerBonusPlan(Err_No, Err_Desc, OrgID)

                Dim dtTemp As New DataTable()

                'Creating Header Row
                dtTemp.Columns.Add("CustomerNo")
                dtTemp.Columns.Add("SiteID")
                dtTemp.Columns.Add("BonusPlanID")



                Dim drAddItem As DataRow
                For i As Integer = 0 To dtOriginal.Rows.Count - 1
                    drAddItem = dtTemp.NewRow()
                    drAddItem(0) = dtOriginal.Rows(i)("CustomerNo").ToString()
                    drAddItem(1) = dtOriginal.Rows(i)("Site_Use_ID").ToString()
                    drAddItem(2) = CInt(dtOriginal.Rows(i)("BonusPlanID").ToString())
                    dtTemp.Rows.Add(drAddItem)
                Next

                If dtOriginal.Rows.Count = 0 Then

                    Me.lblinfo.Text = "Information"
                    Me.lblMessage.Text = "There is no data for the selected filter criteria"
                    Me.lblMessage.ForeColor = Drawing.Color.Red
                    Me.MpInfoError.Show()
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
                    Dim fn As String = "CustomerBonusPlan" + ".xls"
                    Dim d As New DataSet
                    d.Tables.Add(dtTemp)

                    ExportToExcel(fn, d)

                End If
            Else
                Me.lblinfo.Text = "Information"
                Me.lblMessage.Text = "Please select a organization"
                Me.lblMessage.ForeColor = Drawing.Color.Red
                Me.MpInfoError.Show()
                Exit Sub
            End If

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("information.aspx?mode=1&errno=" & "74071" & "&msg=" & AppMsgHandler.GetErrorMessage("SWX_BO_Msg_005") & "&next=Welcome.aspx", False)
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

    Private Sub btnImportWindow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportWindow.Click
        If Me.ddOraganisation.SelectedIndex > 0 Then
            Me.lblUpMsg.Text = ""
            Session("dtBonusPlan") = Nothing
            dgvErros.DataSource = Nothing
            dgvErros.DataBind()
            dgvErros.Visible = False
            lbLog.Visible = False
            Me.MPEImport.Show()
        Else
            Me.lblinfo.Text = "Information"
            Me.lblMessage.Text = "Please select a organization."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Exit Sub
        End If
    End Sub
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



                Dim CustBonusflag As Boolean = False

                CustBonusflag = objProduct.CheckCustBonusFlag(Err_No, Err_Desc)
                If CustBonusflag = False Then
                    Response.Redirect("information.aspx?mode=0&errno=" & Err_No & "&msg=System will not allow to assigning the bonus plan to the customer" & "&next=Welcome.aspx&Title=Message", False)
                End If



                Dim objCommon As New Common
                Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddOraganisation.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddOraganisation.Items.Clear()
                ddOraganisation.Items.Add("-- Select a Organization --")
                ddOraganisation.AppendDataBoundItems = True
                ddOraganisation.DataValueField = "Mas_Org_ID"
                ddOraganisation.DataTextField = "Description"
                ddOraganisation.DataBind()

                If ddOraganisation.Items.Count > 1 Then
                    Me.ddOraganisation.SelectedIndex = 1
                    LoadBonusPlan()

                End If

                Dim OrgID As String = Request.QueryString("ORGID").ToString()
                Dim PlanID As String = Request.QueryString("PGID").ToString()
                Me.ddOraganisation.SelectedValue = OrgID
                Me.ddOraganisation.Enabled = False
                Me.ddlBonusPlan.SelectedValue = PlanID
                Me.ddlBonusPlan.Enabled = False

                lstDefault.Items.Clear()
                lstSelected.Items.Clear()
                BindDefault()
                BindSelected()

            End If
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_005") & "&next=Welcome.aspx&Title=Product List Setup", False)
        End Try

      

    End Sub

    Private Sub LoadBonusPlan()
        If ddOraganisation.SelectedItem.Text <> "-- Select a Organization --" Then

            ddlBonusPlan.DataSource = objProduct.LoadSimpleBonusPlan(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID, ddOraganisation.SelectedValue)
            ddlBonusPlan.Items.Clear()
            ddlBonusPlan.Items.Add("-- Select a Plan --")
            ddlBonusPlan.AppendDataBoundItems = True
            ddlBonusPlan.DataValueField = "BNS_Plan_ID"
            ddlBonusPlan.DataTextField = "Description"
            ddlBonusPlan.DataBind()

            If ddOraganisation.SelectedItem.Text <> "-- Select a Organization --" And ddlBonusPlan.SelectedItem.Text <> "-- Select a Plan --" Then
                ''Bind Default List
                BindDefault()

                ''Bind Selected List
                BindSelected()

              
            Else
                lstDefault.Items.Clear()
              
                lblProdAvailed.Text = ""
            End If
        Else
            ddlBonusPlan.Items.Clear()
            ddlBonusPlan.Items.Add("-- Select a Plan --")
            ddlBonusPlan.AppendDataBoundItems = True
            ddlBonusPlan.DataValueField = "BNS_Plan_Id"
            ddlBonusPlan.DataTextField = "Description"
            ddlBonusPlan.DataBind()
            lstDefault.Items.Clear()
            lstSelected.Items.Clear()
            lblProdAssign.Text = ""
            lblProdAvailed.Text = ""
        End If
    End Sub


    Private Sub ddOraganisation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddOraganisation.SelectedIndexChanged
        
        LoadBonusPlan()

    End Sub

    Protected Sub ddlBonusPlan_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlBonusPlan.SelectedIndexChanged
        Try
            If ddOraganisation.SelectedItem.Text <> "-- Select a Organization --" And ddlBonusPlan.SelectedItem.Text <> "-- Select a Plan --" Then
                ''Bind Default List
                BindDefault()

                ''Bind Selected List
                BindSelected()


            Else
                lstDefault.Items.Clear()

                lblProdAvailed.Text = ""
            End If
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=Welcome.aspx&Title=Product List Setup", False)
        End Try

    End Sub


   
    Private Sub BindDefault()

        Dim TempTbl As New DataTable
        Try
            Err_Desc = Nothing
            Err_No = Nothing
            TempTbl = ObjCustomer.GetAvailableCustomersBonusPlan(Err_No, Err_Desc, IIf(Me.ddlBonusPlan.SelectedIndex <= 0, "0", Me.ddlBonusPlan.SelectedItem.Value), IIf(Me.ddOraganisation.SelectedIndex <= 0, "0", Me.ddOraganisation.SelectedItem.Value))

            If Me.txtFilter.Text <> "" And TempTbl.Rows.Count > 0 Then
                TempTbl.DefaultView.RowFilter = "(Customer_No LIKE '%" & Me.txtFilter.Text & "%')"
            End If

            Dim dv As New DataView
            dv = TempTbl.DefaultView

            If TempTbl IsNot Nothing Then
                lstDefault.DataSource = dv
                lstDefault.DataTextField = "CustName"
                lstDefault.DataValueField = "CustSiteId"
                lstDefault.DataBind()
            End If
            lblProdAssign.Text = "Customers Assigned: [" & lstSelected.Items.Count & "]"
            lblProdAvailed.Text = "Customers Available: [" & lstDefault.Items.Count & "]"

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        End Try
    End Sub

    Private Sub BindSelected()
        Dim TempTbl As New DataTable
        Try
            Err_Desc = Nothing
            Err_No = Nothing

            TempTbl = ObjCustomer.GetAssignedCustomersBonusPlan(Err_No, Err_Desc, IIf(Me.ddOraganisation.SelectedIndex <= 0, "0", Me.ddOraganisation.SelectedItem.Value), IIf(Me.ddlBonusPlan.SelectedIndex <= 0, "0", Me.ddlBonusPlan.SelectedItem.Value))


            If Me.txtFilter.Text <> "" And TempTbl.Rows.Count > 0 Then
                TempTbl.DefaultView.RowFilter = "(Customer_No LIKE '%" & Me.txtFilter.Text & "%')"
            End If

            Dim dv As New DataView
            dv = TempTbl.DefaultView


            If TempTbl IsNot Nothing Then
                lstSelected.DataSource = dv
                lstSelected.DataTextField = "CustName"
                lstSelected.DataValueField = "CustSiteId"
                lstSelected.DataBind()
            End If
            If lstSelected.Items.Count > 0 Then
                Me.btnRemoveAll.Enabled = True
            Else
                Me.btnRemoveAll.Enabled = False
            End If
            Dim ItmSelected As Integer = 0
            For Each Item As ListItem In lstSelected.Items
                If Item.Value <> "0" Then
                    ItmSelected += 1
                End If
            Next


            lblProdAssign.Text = "Customers Assigned: [" & ItmSelected & "]"
            lblProdAvailed.Text = "Customers Available: [" & lstDefault.Items.Count & "]"
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        Finally

        End Try
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If Me.ddOraganisation.SelectedIndex > 0 And ddlBonusPlan.SelectedItem.Text <> "-- Select a Plan --" Then
            Try
                Err_No = Nothing
                Err_Desc = Nothing
                Dim CustSite() As String


                For Each Item As ListItem In lstDefault.Items
                    If Item.Selected Then
                        If Item.Value <> "0" Then
                            CustSite = Item.Value.Split("$")

                            ObjCustomer.InsertBonusPlanToCustomer(CustSite(0).ToString(), CustSite(1).ToString(), Me.ddOraganisation.SelectedItem.Value, Me.ddlBonusPlan.SelectedItem.Value, "Single", Err_No, Err_Desc)
                        End If
                    End If
                Next
                BindDefault()
                BindSelected()
            Catch ex As Exception
                log.Error(ex.Message)
                Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_AdminProductGroup_005") & "&next=AdminBonusPlanToCustomer.aspx&Title=Bonus Plan", False)
            End Try
        Else
            Me.lblinfo.Text = "Information"
            Me.lblMessage.Text = "Please select organization/plan/customers."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Exit Sub
        End If
    End Sub

    Protected Sub btnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        If Me.ddOraganisation.SelectedIndex > 0 And ddlBonusPlan.SelectedItem.Text <> "-- Select a Plan --" Then

            Try
                Err_No = Nothing
                Err_Desc = Nothing
                Dim CustSite() As String

                For Each Item As ListItem In lstSelected.Items
                    If Item.Selected Then
                        CustSite = Item.Value.Split("$")
                        ObjCustomer.DeleteBonusPlanToCustomer(CustSite(0).ToString(), CustSite(1).ToString(), Me.ddOraganisation.SelectedItem.Value, Me.ddlBonusPlan.SelectedItem.Value, "Single", Err_No, Err_Desc)
                    End If
                Next
                BindDefault()
                BindSelected()
            Catch ex As Exception
                log.Error(ex.Message)
                Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_AdminProductGroup_005") & "&next=AdminBonusPlanToCustomer.aspx&Title=Bonus Plan", False)

            End Try
        Else
            Me.lblinfo.Text = "Information"
            Me.lblMessage.Text = "Please select organization/plan/customers."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Exit Sub
        End If
    End Sub


    Public Sub Resetfields()
        'Me.ddlGroup.SelectedIndex = 0
        ' Me.ddOraganisation.SelectedIndex = 0
        'If ddOraganisation.Items.Count > 1 Then
        '    Me.ddOraganisation.SelectedIndex = 1
        '    LoadBonusPlan()
        'End If
        Me.txtFilter.Text = ""
        BindDefault()
        BindSelected()
        '  Me.lstDefault.Items.Clear()
        '  Me.lstSelected.Items.Clear()
        ' Me.ddlGroup.Focus()
        '  Me.lblProdAssign.Text = ""
        '  Me.lblProdAvailed.Text = ""

    End Sub



    Protected Sub btnAddAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddAll.Click
        If Me.ddOraganisation.SelectedIndex > 0 And ddlBonusPlan.SelectedItem.Text <> "-- Select a Plan --" Then
            Try
                Err_No = Nothing
                Err_Desc = Nothing
                ObjCustomer.InsertBonusPlanToCustomer(0, 0, Me.ddOraganisation.SelectedItem.Value, Me.ddlBonusPlan.SelectedItem.Value, "ALL", Err_No, Err_Desc)

                BindDefault()
                BindSelected()
            Catch ex As Exception
                log.Error(ex.Message)
                Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_AdminMSL_005") & "&next=AssignBonusPlanToCustomer.aspx&Title=Assign Bonus Plan", False)
            End Try
        Else
            Me.lblinfo.Text = "Information"
            Me.lblMessage.Text = "Please select organization/bonus plan/customers."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Exit Sub
        End If
    End Sub

    Protected Sub btnRemoveAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRemoveAll.Click
        If Me.ddOraganisation.SelectedIndex > 0 And ddlBonusPlan.SelectedItem.Text <> "-- Select a Plan --" Then
            Try
                Err_No = Nothing
                Err_Desc = Nothing

                ObjCustomer.DeleteBonusPlanToCustomer(0, 0, Me.ddOraganisation.SelectedItem.Value, Me.ddlBonusPlan.SelectedItem.Value, "ALL", Err_No, Err_Desc)

                BindDefault()
                BindSelected()
            Catch ex As Exception
                log.Error(ex.Message)
                Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_AdminMSL_005") & "&next=AssignBonusPlanToCustomer.aspx&Title=Assign Bonus Plan", False)

            End Try
        Else
            Me.lblinfo.Text = "Information"
            Me.lblMessage.Text = "Please select organization/bonus plan/customers."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Exit Sub

        End If
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReset.Click
        Resetfields()
    End Sub


    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        If Me.ddOraganisation.SelectedIndex > 0 And Me.ddlBonusPlan.SelectedIndex > 0 Then
            BindDefault()
            BindSelected()
        Else
            Me.lblinfo.Text = "Information"
            Me.lblMessage.Text = "Please select organization/bonus plan."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Exit Sub
        End If
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("AdminBonusSimple.aspx?OID=", False)
    End Sub
End Class



