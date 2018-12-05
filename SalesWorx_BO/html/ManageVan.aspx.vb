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

Public Class ManageVan
    Inherits System.Web.UI.Page
    Dim objcommon As New SalesWorx.BO.Common.Common
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim objProduct As New SalesWorx.BO.Common.Product
    Dim objVanM As New SalesWorx.BO.Common.VanManage
    Dim objUOM As New UOM
    Dim objDivConfig As New DivConfig
    Private dtErrors As New DataTable
    Private dtErrors_u As New DataTable
    Private Const ModuleName As String = "ManageVan.aspx"
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim HasLots As String
    Dim Allowpricechange As String
    Private Const PageID As String = "P94"
    Private dt As New DataTable
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objControl As New AppControl
    Private _strTempFolder As String = CStr(ConfigurationSettings.AppSettings("ExcelPath"))
    Private PhysicalPath As String = ""
    Private _strMediaFileSize As Long = CLng(ConfigurationSettings.AppSettings("MediaFileSize"))
    Private Property SortField() As String
        Get
            If ViewState("SortColumn") Is Nothing Then
                ViewState("SortColumn") = ""
            End If
            Return ViewState("SortColumn").ToString()
        End Get
        Set(ByVal value As String)
            ViewState("SortColumn") = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

       
        If Not Page.IsPostBack Then
            If Not Session.Item("USER_ACCESS") Is Nothing Then

                If Not HasAuthentication() Then
                    Err_No = 500
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
                End If



                FillOrganization()
                ResetFields()

                Bind()


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
            Session.Remove("dtVanErrors")
            Session.Remove("dtVanErrors_u")
            Session.Remove("VanLogFile")
            SetErrorsTable()
            SetErrorsTable_u()
            '' RefreshUI()
            Bind()
            ' ExcelFileUpload.TemporaryFolder = _strTempFolder
        Else
            Me.DocWindow.VisibleOnPageLoad = False
            dtErrors = Session("dtVanErrors")
            dtErrors_u = Session("dtVanErrors_u")

        End If
            ExcelFileUpload.TemporaryFolder = System.Configuration.ConfigurationManager.AppSettings("TemporaryFolder")

        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub

   
    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ResetFields()
        Me.DocWindow.VisibleOnPageLoad = True

    End Sub
    Private Function IsAlphaNum(ByVal strInputText As String) As Boolean
        Dim IsAlpha As Boolean = False
        If System.Text.RegularExpressions.Regex.IsMatch(strInputText, "^[a-zA-Z0-9]+$") Then
            IsAlpha = True
        Else
            IsAlpha = False
        End If
        Return IsAlpha
    End Function
    Private Function IsAlphabets(ByVal strInputText As String) As Boolean
        Dim IsAlpha As Boolean = False
        If System.Text.RegularExpressions.Regex.IsMatch(strInputText, "^[a-zA-Z ]*$") Then
            IsAlpha = True
        Else
            IsAlpha = False
        End If
        Return IsAlpha
    End Function

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            If txtEmpCode.Text = "" Or Me.txtEmpName.Text = "" Or Me.txtVanID.Text = "" Or Me.ddlOrg.SelectedIndex <= 0 Or txtvanname.Text = "" Then
                Me.lblVMsg.Text = "Organization,Van ID, Van name, Emp code and Emp name are required."
                Me.DocWindow.VisibleOnPageLoad = True
                Exit Sub
            End If


            If txtVanID.Text.Contains(" ") Then
                Me.lblVMsg.Text = "Please space is not allowed in the Van ID "
                Me.DocWindow.VisibleOnPageLoad = True
                Exit Sub
            End If

            If IsAlphaNum(txtVanID.Text) = False Then
                Me.lblVMsg.Text = "Please enter alphanumeric values in the Van ID "
                Me.DocWindow.VisibleOnPageLoad = True
                Exit Sub
            End If

            If IsAlphaNum(txtEmpCode.Text) = False Then
                Me.lblVMsg.Text = "Please enter alphanumeric values in the Emp Code "
                Me.DocWindow.VisibleOnPageLoad = True
                Exit Sub
            End If

            'If IsAlphabets(txtEmpName.Text) = False Then
            '    Me.lblVMsg.Text = "Please avoid numeric/special characters values in the Emp Name "
            '    Me.DocWindow.VisibleOnPageLoad = True
            '    Exit Sub
            'End If

            If btnSave.Text = "Save" Then
                If objVanM.CheckVancodeDuplicate(Err_No, Err_Desc, txtVanID.Text) = True Then
                    Me.lblVMsg.Text = "Van ID already exist"
                    Me.DocWindow.VisibleOnPageLoad = True
                    Exit Sub
                End If


                If objVanM.CheckEmpcodeDuplicate(Err_No, Err_Desc, txtEmpCode.Text, "") = True Then
                    Me.lblVMsg.Text = "Emp code already exist"
                    Me.DocWindow.VisibleOnPageLoad = True
                    Exit Sub
                End If
                If objVanM.SaveVan(Err_No, Err_Desc, txtVanID.Text, ddlOrg.SelectedValue, txtvanname.Text, txtEmpCode.Text, txtEmpName.Text, txtEmpphone.Text, txtPrefix.Text) Then
                    Me.DocWindow.VisibleOnPageLoad = False
                    MessageBoxValidation("Successfully saved", "Information")
                    ResetFields()
                    Bind() ' 
                End If



            ElseIf Me.btnSave.Text = "Update" Then
               


                If objVanM.CheckEmpcodeDuplicate(Err_No, Err_Desc, txtEmpCode.Text, txtVanID.Text) = True Then
                    Me.lblVMsg.Text = "Emp code already exist"
                    Me.DocWindow.VisibleOnPageLoad = True
                    Exit Sub
                End If

                If objVanM.UpdateVan(Err_No, Err_Desc, txtVanID.Text, ddlOrg.SelectedValue, txtvanname.Text, txtEmpCode.Text, txtEmpName.Text, txtEmpphone.Text, txtPrefix.Text) Then
                    Me.DocWindow.VisibleOnPageLoad = False
                    MessageBoxValidation("Successfully updated", "Information")
                    ResetFields()
                    Bind()
                Else
                    Me.lblVMsg.Text = "Error occured while updating."
                    Me.DocWindow.VisibleOnPageLoad = True
                    Exit Sub
                End If
            End If

          



            ResetFields()
            Bind()
            TopPanel.Update()
        Catch ex As Exception
            Me.DocWindow.VisibleOnPageLoad = False
            TopPanel.Update()
            MessageBoxValidation("Error occured while saving.Please Check Log", "Validation")
            TopPanel.Update()
            log.Error(ex.Message.ToString())
        End Try

    End Sub

  

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click


        ResetFields()


    End Sub


    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click

        Try

       
        Dim finalDataSet As New DataSet
       

        finalDataSet = objVanM.LoadExportVanTemplate()
        If finalDataSet.Tables.Count() > 1 Then
            finalDataSet.Tables(0).TableName = "Van_Info"

        End If



        Dim fn As String = "Van_Info" + DateTime.Now.ToString("hhmmss") + ".xls"
        Dim d As New DataSet


        If finalDataSet.Tables.Count <= 0 Then
            MessageBoxValidation("There is no data to export", "Information")
            Exit Sub
        Else
            ExportToExcel(fn, finalDataSet)
        End If

        Catch ex As Exception
            log.Error(ex.Message.ToString())
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
        Me.txtEmpCode.Text = ""
        Me.txtEmpName.Text = ""
        Me.txtEmpphone.Text = ""
        Me.txtPrefix.Text = ""
        Me.txtFilterVal.Text = ""
        Me.txtVanID.Text = ""
        Me.txtvanname.Text = ""
       
        Me.lblProdID.Text = ""
        Me.lblMsg.Text = ""
        Me.lblVMsg.Text = ""

        Me.btnSave.Text = "Save"

        Me.ddlOrg.Enabled = True
        Me.DocWindow.Title = "Add Van"
        Me.DocWindow.VisibleOnPageLoad = False
      
        Me.ddlOrg.ClearSelection()
        Me.ddlOrg.Text = ""
        Me.ddlOrg.Focus()


       
        Me.ddlOrg_UOM.Enabled = True
        Me.ddlOrg_UOM.ClearSelection()
        Me.ddlOrg_UOM.Text = ""
        Me.ddlOrg_UOM.Focus()

        Me.txtVanID.Enabled = True
        Me.ddlOrg.Enabled = True

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

        Session.Remove("dtVanErrors")
        Session("dtVanErrors") = dtErrors
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

        Session.Remove("dtVanErrors_u")
        Session("dtVanErrors_u") = dtErrors_u
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

                        'Dim Foldername As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath")
                        Dim Foldername As String = ExcelFileUpload.TemporaryFolder
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
                                col.ColumnName = "Van_Code"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)


                                col = New DataColumn
                                col.ColumnName = "Van_Name"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)

                                col = New DataColumn
                                col.ColumnName = "Doc_Prefix"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)


                                col = New DataColumn
                                col.ColumnName = "Emp_Code"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)


                                col = New DataColumn
                                col.ColumnName = "Emp_Name"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)




                                col = New DataColumn
                                col.ColumnName = "Emp_Phone"
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
                                col.ColumnName = "Sales_Org_Name"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)


                                'col = New DataColumn
                                'col.ColumnName = "Currency_Code"
                                'col.DataType = System.Type.GetType("System.String")
                                'col.ReadOnly = False
                                'col.Unique = False
                                'TempTbl.Columns.Add(col)



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



                                '   If TempTbl.Columns.Count = 13 Then
                                If Not (TempTbl.Columns(0).ColumnName = "Van_Code" Or TempTbl.Columns(1).ColumnName = "Van_Name" Or TempTbl.Columns(2).ColumnName = "Doc_Prefix" Or TempTbl.Columns(3).ColumnName = "Emp_Code" Or TempTbl.Columns(4).ColumnName = "Emp_Name" Or TempTbl.Columns(5).ColumnName = "Emp_Phone" Or TempTbl.Columns(6).ColumnName = "Sales_Org_ID" Or TempTbl.Columns(7).ColumnName = "Sales_Org_Name") Then



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
                                dtErrors = Session("dtVanErrors")

                                If dtErrors.Rows.Count > 0 Then
                                    dtErrors.Rows.Clear()
                                End If
                                Dim RowNo As String = Nothing
                                Dim ErrorText As String = Nothing

                                Dim RowNo_u As String = Nothing
                                Dim ErrorText_u As String = Nothing
                                If TempTbl.Rows.Count > 0 Then
                                    Dim idx As Integer

                                    Dim Van_Code As String = Nothing
                                    Dim Van_Name As String = Nothing
                                    Dim Doc_Prefix As String = Nothing
                                    Dim Emp_Code As String = Nothing
                                    Dim Emp_Name As String = Nothing
                                    Dim Emp_Phone As String = Nothing
                                    Dim Sales_Org_ID As String = Nothing
                                    Dim Sales_Org_Name As String = Nothing
                                    Dim Currency_Code As String = Nothing

                                    Dim isValidRow As Boolean = True
                                    For idx = 0 To TempTbl.Rows.Count - 1

                                        RowNo = Nothing
                                        ErrorText = Nothing
                                        isValidRow = True

                                        If TempTbl.Rows(idx)(0) Is DBNull.Value And TempTbl.Rows(idx)(1) Is DBNull.Value And TempTbl.Rows(idx)(2) Is DBNull.Value And TempTbl.Rows(idx)(3) Is DBNull.Value Then
                                            Continue For
                                        End If

                                        Van_Code = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, "", TempTbl.Rows(idx)(0).ToString())
                                        Van_Name = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, "", TempTbl.Rows(idx)(1).ToString())
                                        Doc_Prefix = IIf(TempTbl.Rows(idx)(2) Is DBNull.Value, "", TempTbl.Rows(idx)(2).ToString())
                                        Emp_Code = IIf(TempTbl.Rows(idx)(3) Is DBNull.Value, "", TempTbl.Rows(idx)(3).ToString())
                                        Emp_Name = IIf(TempTbl.Rows(idx)(4) Is DBNull.Value, "", TempTbl.Rows(idx)(4).ToString())
                                        Emp_Phone = IIf(TempTbl.Rows(idx)(5) Is DBNull.Value, "", TempTbl.Rows(idx)(5).ToString())
                                        Sales_Org_ID = IIf(TempTbl.Rows(idx)(6) Is DBNull.Value, "", TempTbl.Rows(idx)(6).ToString())
                                        Sales_Org_Name = IIf(TempTbl.Rows(idx)(7) Is DBNull.Value, "", TempTbl.Rows(idx)(7).ToString())
                                        'Currency_Code = IIf(TempTbl.Rows(idx)(8) Is DBNull.Value, "", TempTbl.Rows(idx)(8).ToString())



                                        If Van_Code = "0" Or Van_Code Is Nothing Then
                                            Continue For
                                        End If





                                        If Van_Name = "0" Or Van_Name.Trim = "" Then
                                            RowNo = idx + 2
                                            ErrorText = "Van name is mandatory" + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End If


                                        If Sales_Org_ID = "0" Or Sales_Org_ID = "" Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "Organization is mandatory" + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End If

                                        If Emp_Code = "0" Or Emp_Code = "" Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + " Emp Code is  mandatory" + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End If
                                        If Emp_Name = "0" Or Emp_Name = "" Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + " Emp Name is  mandatory" + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End If


                                        If Not Sales_Org_ID = "0" And Not Sales_Org_ID = "" Then
                                            If objVanM.ValidOrgnization(Err_No, Err_Desc, Sales_Org_ID) = False Then
                                                RowNo = idx + 2
                                                ErrorText = ErrorText + " Organization does not exist" + ","
                                                isValidRow = False
                                                TotFailed += 1
                                            End If
                                        End If
                                       



                                        If objVanM.CheckVancodeDuplicate(Err_No, Err_Desc, Van_Code) = True Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "Van ID already exist" + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End If
                                        If objVanM.CheckEmpcodeDuplicate(Err_No, Err_Desc, Emp_Code, "") = True Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "Emp code already exist " + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End If

                                        If Van_Code <> "" Then
                                            If IsAlphaNum(Van_Code) = False Then
                                                RowNo = idx + 2
                                                ErrorText = ErrorText + "Invalid Van ID (only alphanumeric values allowed)" + ","
                                                isValidRow = False
                                                TotFailed += 1
                                            End If
                                        End If

                                        If Emp_Code <> "" Then
                                            If IsAlphaNum(Emp_Code) = False Then

                                                RowNo = idx + 2
                                                ErrorText = ErrorText + "Invalid emp code  (only alphanumeric values allowed)" + ","
                                                isValidRow = False
                                                TotFailed += 1

                                            End If
                                        End If
                                        If Emp_Name <> "" Then
                                            If IsAlphabets(Emp_Name) = False Then
                                                RowNo = idx + 2
                                                ErrorText = ErrorText + "Invalid Emp Name  (only alphanumeric values allowed)" + ","
                                                isValidRow = False
                                                TotFailed += 1
                                            End If
                                        End If

                                        If Van_Code.Contains(" "c) Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "Space is not allowed in the Van ID" + ","
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
                                            isValidRow = False
                                        End If

                                        If isValidRow = True Then

                                            If objVanM.SaveVan(Err_No, Err_Desc, Van_Code, Sales_Org_ID, Van_Name, Emp_Code, Emp_Name, Emp_Phone, Doc_Prefix) Then

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





                                ResetFields()
                                Bind()


                                Session.Remove("dtVanErrors")
                                Session("dtVanErrors") = dtErrors


                                Session.Remove("dtVanErrors_u")
                                Session("dtVanErrors_u") = dtErrors_u

                                Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & "Van_infoLog" & Now.ToString("yyyyMMdd") + ".txt"




                                dtErrors.TableName = "Van_Info"



                                Dim Ds_error As New DataSet
                                Ds_error.Tables.Add(dtErrors.Copy())

                                DataTable2CSV(Ds_error, fn, vbTab)

                                Session.Remove("VanLogFile")
                                Session("VanLogFile") = fn



                                If TotSuccess > 0 Then
                                    DeleteExcel()
                                    Bind()
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
            log.Error(ex.ToString())
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

                Dim cmd As New OleDbCommand("SELECT * FROM [Van_Info$] WHERE [Van_Code] IS NOT NULL", oledbConn)

                Dim oleda As New OleDbDataAdapter()

                oleda.SelectCommand = cmd



                oleda.Fill(dtImport)



            Catch ex As Exception
                log.Error(GetExceptionInfo(ex))
                log.Error(ex.ToString())
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

                Dim cmd As New OleDbCommand("SELECT * FROM [Van_Info$] WHERE [Van_Code] IS NOT NULL", oledbConn)

                Dim oleda As New OleDbDataAdapter()

                oleda.SelectCommand = cmd



                oleda.Fill(dtImport)



            Catch ex As Exception
                log.Error(GetExceptionInfo(ex))
                log.Error(ex.ToString())
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



    Sub DataTable2CSV(ByVal Ds As DataSet, ByVal filename As String, ByVal sepChar As String)
        Dim writer As System.IO.StreamWriter
        Try
            writer = New System.IO.StreamWriter(filename)

            For Each table As DataTable In Ds.Tables


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






    Sub FillOrganization()
        Try
            Dim objCommon As New SalesWorx.BO.Common.Common
            Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
            ddlOrg.DataSource = objProduct.GetOrg(Err_No, Err_Desc)
            ddlOrg.Items.Clear()
            ddlOrg.Items.Add(New RadComboBoxItem("Select Organization"))
            ddlOrg.AppendDataBoundItems = True
            ddlOrg.DataValueField = "ORG_HE_ID"
            ddlOrg.DataTextField = "Description"
            ddlOrg.DataBind()

            If ddlOrg.Items.Count = 2 Then
                ddlOrg.SelectedIndex = 1
            End If
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub




    Sub FillUOM_Exp()
        Try
            Dim objCommon As New SalesWorx.BO.Common.Common
            Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
            ddlOrg_UOM.DataSource = objProduct.GetOrg(Err_No, Err_Desc)
            ddlOrg_UOM.Items.Clear()
            ddlOrg_UOM.DataValueField = "ORG_HE_ID"
            ddlOrg_UOM.DataTextField = "Description"
            ddlOrg_UOM.DataBind()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub


    Protected Sub lbLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbLog.Click
        Try
            If Not Session("VanLogFile") Is Nothing Then
                Dim fileValue As String = Session("VanLogFile")

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
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try

    End Sub

    Protected Sub btndownloadTemplate_Click(sender As Object, e As EventArgs) Handles btndownloadTemplate.Click
        Try
            Dim Filename As String = System.Configuration.ConfigurationManager.AppSettings("ExcelTemplatePath") & "Van_Info.xls"
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
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
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

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Dim success As Boolean = False
        Try
            dt = objVanM.SearchVanGrid(Err_No, Err_Desc, Me.ddFilterBy.Text, IIf(Me.txtFilterVal.Text = "", "", Me.txtFilterVal.Text))
            ' rgProducts.DataSource = dt
            '  rgProducts.DataBind()
            gvVan.DataSource = dt
            gvVan.Rebind()
            success = True
            If success = False Then
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_Managecode_004") & "&next=ManageVan.aspx&Title=Message", False)
                Exit Try
            End If
        Catch ex As Exception
            Err_No = "74074"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub
    Private Sub Bind()
        Try
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt_b As New DataTable
            dt_b = objVanM.SearchVanGrid(Err_No, Err_Desc, Me.ddFilterBy.SelectedItem.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text))
            gvVan.DataSource = dt_b
            gvVan.DataBind()

        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub


    Protected Sub gvVan_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles gvVan.ItemCommand
        Try
            If e.CommandName = "EditSelected" Then
                Dim item As GridDataItem = DirectCast(e.Item, GridDataItem)
                Dim VanID As Label = DirectCast(item.FindControl("lblVanID_e"), Label)
                Dim OrgID As Label = DirectCast(item.FindControl("lblOrgID_e"), Label)
                ResetFields()
                Me.lblProdID.Text = VanID.Text
                Me.lblOrgID.Text = OrgID.Text
                Dim dt_van As New DataTable

                dt_van = objVanM.GetVanDetails(Err_No, Err_Desc, VanID.Text, OrgID.Text)

                If dt_van.Rows.Count > 0 Then
                    Me.txtEmpName.Text = dt_van.Rows(0)("Emp_Name")
                    Me.txtEmpCode.Text = dt_van.Rows(0)("Emp_Code")
                    Me.txtEmpphone.Text = dt_van.Rows(0)("Emp_Phone")
                    Me.txtPrefix.Text = dt_van.Rows(0)("Prefix_No")
                    Me.txtVanID.Text = dt_van.Rows(0)("Van_Org_ID")
                    Me.txtvanname.Text = dt_van.Rows(0)("VanName")
                    ddlOrg.SelectedValue = dt_van.Rows(0)("Sales_Org_ID")
                    Me.txtVanID.Enabled = False
                    Me.ddlOrg.Enabled = False

                End If

                Me.btnSave.Text = "Update"

                Me.DocWindow.Title = "Modify Van"
                Me.DocWindow.VisibleOnPageLoad = True






            End If



            If e.CommandName = "DeleteSelected" Then
                Try






                    Dim item As GridDataItem = DirectCast(e.Item, GridDataItem)
                    Dim VanID As Label = DirectCast(item.FindControl("lblVanID"), Label)
                    Dim OrgID As Label = DirectCast(item.FindControl("lblOrgID"), Label)
                    Dim VanID_d As String = VanID.Text
                    Dim OrgID_d As String = OrgID.Text


                    If objVanM.DeleteVan(Err_No, Err_Desc, VanID_d, OrgID_d) Then
                        MessageBoxValidation("Successfully deleted", "Information")
                        Bind()
                    Else
                        MessageBoxValidation(Err_Desc, "Validation")
                        Exit Sub
                    End If

                Catch ex As Exception
                    Err_No = "63924"
                    log.Error(Err_Desc)
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("PHX_006") & "&next=ManageVan.aspx&Title=Message", False)
                    If Err_Desc Is Nothing Then
                        log.Error(GetExceptionInfo(ex))
                    Else
                        log.Error(Err_Desc)
                    End If
                End Try
            End If

        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try

    End Sub
    Public Function IsNumeric(ByVal inputString As String) As Boolean
        Dim _isNumber As System.Text.RegularExpressions.Regex = New  _
Regex("(^[-+]?\d+(,?\d*)*\.?\d*([Ee][-+]\d*)?$)|(^[-+]?\d?(,?\d*)*\.\d+([Ee][-+]\d*)?$)")
        Return _isNumber.Match(inputString).Success
    End Function

    Public Function IsAlpha(ByVal strToCheck As String) As Boolean
        Dim objAlphaPattern As Regex = New Regex("[^a-zA-Z]")
        Return Not objAlphaPattern.IsMatch(strToCheck)
    End Function

    Private Sub gvVan_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvVan.PageIndexChanged
        Bind()
    End Sub

    Private Sub gvVan_SortCommand(sender As Object, e As GridSortCommandEventArgs) Handles gvVan.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        Bind()
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
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            ResetFields()
            ddFilterBy.SelectedIndex = 0
            Bind()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
End Class
