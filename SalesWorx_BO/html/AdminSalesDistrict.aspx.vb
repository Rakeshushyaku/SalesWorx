Imports SalesWorx.BO.Common
Imports System.Data
Imports System.Web
Imports System.Resources
Imports System.Web.UI.WebControls
Imports log4net
Imports System.Text.RegularExpressions
Imports ExcelLibrary.SpreadSheet
Imports System.IO
Imports System.Data.OleDb
Imports Telerik.Web.UI

Partial Public Class AdminSalesDistrict
    Inherits System.Web.UI.Page
    Dim objSalesDistrict As New SalesDistrict
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim Err_No As Long
    Dim Err_Desc As String
    Private dtErrors As New DataTable
    Dim Dt As New DataTable
    Private Const PageID As String = "P284"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        If Not Page.IsPostBack Then

            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            If Not HasPermission Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If

            BindData()
            Resetfields()
            Session.Remove("Code")
            Session.Remove("dtDsErrors")
            Session.Remove("DsLogFile")
            SetErrorsTable()
        Else
            MPEDetails.VisibleOnPageLoad = False
            dtErrors = Session("dtDsErrors")
        End If
        ExcelFileUpload.TemporaryFolder = System.Configuration.ConfigurationManager.AppSettings("TemporaryFolder")
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Try
            MPEDetails.VisibleOnPageLoad = False
            Resetfields()
            '            ClassUpdatePnl.Update()
        Catch

        End Try
    End Sub

    Public Sub Resetfields()
        lblMessage.Text = ""
        Me.txtDescription.Text = ""
        Me.btnSave.Text = "Save"
        Me.txtcode.Text = ""
        'Me.btnAdd.Focus()
        ''Me.lblMessage.Text = ""

    End Sub

    'Protected Sub CheckRequiredFields()
    '    If Me.txtCurrencyCode.Text = "" Or Me.txtDescription.Text = "" Or Me.txtRate.Text = "" Then
    '        Me.lblinfo.Text = "Validation"
    '        Me.lblMessage.Text = "Currency code,description and conversion rate are required."
    '        Me.lblMessage.ForeColor = Drawing.Color.Red
    '        Me.MpInfoError.Show()
    '        Me.MPECurrency.Show()
    '        Exit Sub
    '    End If

    '    If IsAlpha(Me.txtCurrencyCode.Text) = False Then
    '        Me.lblinfo.Text = "Validation"
    '        Me.lblMessage.Text = "Currency code should be in characters."
    '        Me.lblMessage.ForeColor = Drawing.Color.Red
    '        Me.MpInfoError.Show()
    '        Me.MPECurrency.Show()
    '        Exit Sub
    '    End If


    '    If IsNumeric(Me.txtRate.Text) = False Then
    '        Me.lblinfo.Text = "Validation"
    '        Me.lblMessage.Text = "Conversion rate should be in number."
    '        Me.lblMessage.ForeColor = Drawing.Color.Red
    '        Me.MpInfoError.Show()
    '        Me.MPECurrency.Show()
    '        Exit Sub
    '    End If
    'End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If Me.txtDescription.Text.Trim = "" Then

            Me.lblMessage.Text = "Sales District Name is required."
            Me.MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If
        If Me.txtcode.Text.Trim = "" Then

            Me.lblMessage.Text = "Sales District Code is required."
            Me.MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If

        If objSalesDistrict.Validatecode(Me.txtcode.Text) = True Then
            Me.lblMessage.Text = "Sales District Code is alreday exists."

            Me.MPEDetails.VisibleOnPageLoad = True
            txtcode.Text = ""
            Exit Sub
        End If


        Dim success As Boolean = False
        Try

            objSalesDistrict.ID = HidVal.Value
            objSalesDistrict.Description = Me.txtDescription.Text
            objSalesDistrict.Code = Me.txtcode.Text
            If objSalesDistrict.InsertSalesDistrict(Err_No, Err_Desc) = True Then
                success = True
                MessageBoxValidation("Successfully saved", "Information")
                BindData()
                Resetfields()
                Me.MPEDetails.VisibleOnPageLoad = False
                ClassUpdatePnl.Update()
            Else
                success = False
                MessageBoxValidation("Could not be saved", "Information")
                Me.MPEDetails.VisibleOnPageLoad = False
                ClassUpdatePnl.Update()
            End If

            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "SALES DISTRICT", HidVal.Value, Me.txtDescription.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")

            End If

        Catch ex As Exception
            Err_No = "74061"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Private Sub BindData()
        Dt = objSalesDistrict.SearchSalesDistrictGrid(Err_No, Err_Desc, Me.ddFilterBy.Text, txtFilterVal.Text)
        Me.grdSalesDistrict.DataSource = Dt
        Me.grdSalesDistrict.DataBind()

        Dim dv As New DataView(Dt)
        If ViewState("SortField") <> "" Then
            dv.Sort = (ViewState("SortField") & " ") + SortDirection
        End If
        grdSalesDistrict.DataSource = dv
        grdSalesDistrict.DataBind()

    End Sub
    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If Me.txtDescription.Text.Trim = "" Then

            Me.lblMessage.Text = "Sales District Name is required."
            Me.MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If
        If Me.txtcode.Text.Trim = "" Then

            Me.lblMessage.Text = "Sales District Code is required."
            Me.MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If

        If Session("Code") <> txtcode.Text Then
            If Me.txtcode.Text.Trim() <> "" Then
                If objSalesDistrict.Validatecode(Me.txtcode.Text) = True Then
                    Me.lblMessage.Text = "Sales District Code is alreday exists."
                    Me.MPEDetails.VisibleOnPageLoad = True
                    txtcode.Text = ""
                    Exit Sub
                End If
            End If
        End If
        Dim success As Boolean = False
        Try

            objSalesDistrict.ID = HidVal.Value
            objSalesDistrict.Description = Me.txtDescription.Text
            objSalesDistrict.Code = Me.txtcode.Text
            If objSalesDistrict.UpdateSalesDistrict(Err_No, Err_Desc) = True Then
                success = True
                MessageBoxValidation("Successfully saved", "Information")
                BindData()
                Resetfields()
                Me.MPEDetails.VisibleOnPageLoad = False
                ClassUpdatePnl.Update()
            Else
                success = False
                MessageBoxValidation("Could not be saved", "Information")
                Me.MPEDetails.VisibleOnPageLoad = False
                ClassUpdatePnl.Update()
            End If
            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "U", "MAIN MENU", "SALES DISTRICT", HidVal.Value, Me.txtDescription.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")

            End If

        Catch ex As Exception
            Err_No = "74062"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Protected Sub btnDeleteAll_Click()
        Try
            Dim dr As GridViewRow
            Dim Success As Boolean = False
            For Each dr In grdSalesDistrict.Rows
                Dim RowCheckBox As System.Web.UI.WebControls.CheckBox = dr.FindControl("chkDelete")

                If RowCheckBox.Checked = True Then
                    Dim Lbl As System.Web.UI.WebControls.Label = dr.FindControl("lblSales_District")
                    HidVal.Value = Lbl.Text
                    Dim Des = dr.Cells(1).Text
                    objSalesDistrict.ID = HidVal.Value
                    If objSalesDistrict.DeleteSalesDistrict(Err_No, Err_Desc) = True Then
                        objLogin.SaveUserLog(Err_No, Err_Desc, "D", "MAIN MENU", "SALES DISTRICT", HidVal.Value, Me.txtDescription.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                        Success = True
                    End If
                End If
            Next
            If (Success = True) Then

                MessageBoxValidation("Sales District(s) deleted successfully.", "Information")

                BindData()
                Resetfields()
            Else

                MessageBoxValidation("Some Sales District(s) could not be deleted.", "Information")
                Resetfields()
            End If

            'ClassUpdatePnl.Update()
        Catch ex As Exception
            Err_No = "74063"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Dim success As Boolean = False
        Try
            BindData()
            ClassUpdatePnl.Update()
            success = True

        Catch ex As Exception
            Err_No = "74064"
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

        Dim Lbl As System.Web.UI.WebControls.Label = row.FindControl("lblSales_District")
        HidVal.Value = Lbl.Text
        objSalesDistrict.ID = HidVal.Value
        Try

            If objSalesDistrict.DeleteSalesDistrict(Err_No, Err_Desc) = True Then
                success = True
            End If

            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "D", "MAIN MENU", "SALES DISTRICT", HidVal.Value, row.Cells(1).Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                MessageBoxValidation("Successfully deleted.", "Information")
                BindData()
                Resetfields()
            Else
                MessageBoxValidation("Dould not be deleted.", "Information")
                Resetfields()
            End If

        Catch ex As Exception
            Err_No = "74065"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub


    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        btnUpdate.Visible = False
        btnSave.Visible = True
        Resetfields()

        Me.MPEDetails.VisibleOnPageLoad = True
        ClassUpdatePnl.Update()
    End Sub


    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try

            btnUpdate.Visible = True
            btnSave.Visible = False
            Resetfields()
            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
            Dim Lbl As System.Web.UI.WebControls.Label = row.FindControl("lblSales_District")
            HidVal.Value = Lbl.Text
            txtDescription.Text = Trim(row.Cells(1).Text)
            If Trim(row.Cells(2).Text.Trim()) = "&nbsp;" Then
                txtcode.Text = ""
                Session("Code") = ""
            Else
                txtcode.Text = Trim(row.Cells(2).Text.Trim())
                Session("Code") = Trim(row.Cells(2).Text)
            End If


            ' ClassUpdatePnl.Update()
            Me.MPEDetails.VisibleOnPageLoad = True
        Catch ex As Exception
            Err_No = "74066"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_CurrencyCode_006") & "&next=AdminSalesDistrict.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub

    Private Sub gvCurrency_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdSalesDistrict.PageIndexChanging
        grdSalesDistrict.PageIndex = e.NewPageIndex
        BindData()

    End Sub

    Private Sub gvCurrency_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdSalesDistrict.RowDataBound
        'If e.Row.RowType.Equals(DataControlRowType.Pager) Then
        '    Dim pTableRow As TableRow = _
        '             CType(e.Row.Cells(0).Controls(0).Controls(0), TableRow)
        '    For Each cell As TableCell In pTableRow.Cells
        '        For Each control As Control In cell.Controls
        '            If TypeOf control Is LinkButton Then
        '                Dim lb As LinkButton = CType(control, LinkButton)
        '                lb.Attributes.Add("onclick", "ScrollToTop();")
        '            End If
        '        Next
        '    Next
        'End If
    End Sub
    Private Sub gvCurrency_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdSalesDistrict.Sorting
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindData()
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

    Protected Sub btnclearFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnclearFilter.Click
        txtFilterVal.Text = ""
        ddFilterBy.SelectedIndex = 0
        ddFilterBy.Text = "All"
        BindData()
        ClassUpdatePnl.Update()
    End Sub


    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click

        Dim finalDataSet As New DataSet
        Dim dtDistrict As New DataTable()


        dtDistrict = objSalesDistrict.LoadExportSalesDistrictTemplate()
        dtDistrict.TableName = "SalesDistrict"
        finalDataSet.Tables.Add(dtDistrict)






        Dim fn As String = "SalesDistrict" + DateTime.Now.ToString("hhmmss") + ".xls"
        Dim d As New DataSet


        If finalDataSet.Tables.Count <= 0 Then
            MessageBoxValidation("There is no data to export", "Information")
            Exit Sub
        Else
            ExportToExcel(fn, finalDataSet)
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

    Protected Sub ExcelFileUpload_FileUploaded(ByVal sender As Object, ByVal e As FileUploadedEventArgs)
        Try
            If Not e.File.FileName Is Nothing Then
                Dim fileName As String = e.File.FileName
                Dim exten As String = System.IO.Path.GetExtension(fileName)
                Dim Str As New StringBuilder
                Dim TotSuccess As Integer = 0
                Dim TotFailed As Integer = 0
                Try
                    ViewState("FileType") = e.File.ContentType

                    If Not (exten.ToLower().Trim() = ".csv" Or exten.ToLower().Trim() = ".xls" Or exten.ToLower().Trim() = ".xlsx") Then
                        MessageBoxValidation("Please upload excel or csv file", "Validation")
                        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)

                        Exit Sub

                    End If

                    If e.File.FileName.ToString.ToLower().Trim().EndsWith(".csv") Or e.File.FileName.ToString.ToLower().Trim().EndsWith(".xls") Or e.File.FileName.ToString.ToLower().Trim().EndsWith(".xlsx") Then

                        Dim Foldername As String = System.Configuration.ConfigurationManager.AppSettings("TemporaryFolder")
                        ' Dim Foldername As String = ExcelFileUpload.TemporaryFolder
                        If Not Foldername.EndsWith("\") Then
                            Foldername = Foldername & "\"
                        End If
                        If Directory.Exists(Foldername.Substring(0, Len(Foldername) - 1)) = False Then
                            Directory.CreateDirectory(Foldername.Substring(0, Len(Foldername) - 1))
                        End If
                        If e.File.FileName.ToString.ToLower().Trim().EndsWith(".csv") Then
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
                                col.ColumnName = "Description"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)

                                col = New DataColumn
                                col.ColumnName = "Code"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)






                                If ViewState("FileName").ToString.ToLower().Trim().EndsWith(".csv") Then
                                    TempTbl = DoCSVUpload()
                                ElseIf ViewState("FileName").ToString.ToLower().Trim().EndsWith(".xls") Then
                                    TempTbl = DoXLSUpload()
                                ElseIf ViewState("FileName").ToString.ToLower().Trim().EndsWith(".xlsx") Then
                                    TempTbl = DoXLSXUpload()
                                End If

                                If TempTbl.Rows.Count = 0 Or TempTbl Is Nothing Then
                                    MessageBoxValidation("Invalid file Template", "Validation")
                                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                                    Exit Sub
                                End If

                                '   If TempTbl.Columns.Count = 2 Then
                                If Not (TempTbl.Columns(0).ColumnName = "Description" Or TempTbl.Columns(1).ColumnName = "Code") Then
                                    MessageBoxValidation("Please check the template columns are correct", "Validation")
                                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                                    Exit Sub

                                End If



                                If TempTbl.Rows.Count = 0 Then
                                    MessageBoxValidation("There is no data in the file.", "Validation")
                                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)

                                    Exit Sub
                                End If
                                dtErrors = Session("dtDsErrors")

                                If dtErrors.Rows.Count > 0 Then
                                    dtErrors.Rows.Clear()
                                End If
                                Dim RowNo As String = Nothing
                                Dim ErrorText As String = Nothing

                                If TempTbl.Rows.Count > 0 Then
                                    Dim idx As Integer

                                    Dim Description As String = Nothing
                                    Dim Code As String = Nothing



                                    Dim isValidRow As Boolean = True
                                    For idx = 0 To TempTbl.Rows.Count - 1

                                        RowNo = Nothing
                                        ErrorText = Nothing
                                        isValidRow = True

                                        If TempTbl.Rows(idx)(0) Is DBNull.Value Then
                                            Continue For
                                        End If


                                        Description = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, "", TempTbl.Rows(idx)(0).ToString())
                                        Code = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, "", TempTbl.Rows(idx)(1).ToString())




                                        If Description = "0" Or Code = "0" Then
                                            Continue For
                                        End If
                                        If Description = "" Then
                                            RowNo = idx + 2
                                            ErrorText = "Description should not be empty " + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End If

                                        If Code = "" Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "Code should not be empty " + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End If

                                        If Description <> "" Then
                                            If objSalesDistrict.ValidateDescription(Err_No, Err_Desc, Description) = True Then
                                                RowNo = idx + 2
                                                ErrorText = ErrorText + "Description Already Exists " + ","
                                                isValidRow = False
                                                TotFailed += 1
                                            End If
                                        End If

                                        If Code <> "" Then
                                            If objSalesDistrict.Validatecode(Code) = True Then
                                                RowNo = idx + 2
                                                ErrorText = ErrorText + "Code Already Exists " + ","
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

                                            If objSalesDistrict.SaveSalesDistrict(Err_No, Err_Desc, Description, Code) Then

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
                                                h("LogInfo") = "Error occured while saving this row"
                                                dtErrors.Rows.Add(h)
                                                RowNo = Nothing
                                                ErrorText = Nothing
                                                isValidRow = True
                                            End If
                                        End If

                                    Next
                                End If
                                Resetfields()




                                Session.Remove("dtDsErrors")
                                Session("dtDsErrors") = dtErrors

                                Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & "SalesDistrictLog" & Now.ToString("yyyyMMdd") + ".txt"

                                DataTable2CSV(dtErrors, fn, vbTab)

                                Session.Remove("SalesDistrictLogFile")
                                Session("SalesDistrictLogFile") = fn



                                If TotSuccess > 0 Then
                                    DeleteExcel()
                                    ' BindItemUOMData()
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

                Dim cmd As New OleDbCommand("SELECT * FROM [SalesDistrict$] WHERE [Description] IS NOT NULL", oledbConn)

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

                Dim cmd As New OleDbCommand("SELECT * FROM [SalesDistrict$] WHERE [Description] IS NOT NULL", oledbConn)

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
    Protected Sub lbLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbLog.Click
        If Not Session("SalesDistrictLogFile") Is Nothing Then
            Dim fileValue As String = Session("SalesDistrictLogFile")

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

        Session.Remove("dtDsErrors")
        Session("dtDsErrors") = dtErrors
        Return dtErrors
    End Function

End Class