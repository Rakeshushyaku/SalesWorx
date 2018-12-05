Imports log4net
Imports SalesWorx.BO.Common
Imports Telerik.Web.UI
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports ExcelLibrary.SpreadSheet

Partial Public Class AssignProductToFSR
    Inherits System.Web.UI.Page
    Private dtErrors As New DataTable
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const ModuleName As String = "AssignProductToFSR.aspx"
    Private Const PageID As String = "P377"
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
            ddl_org.Items.Insert(0, New RadComboBoxItem("Select Organisation", "0"))

                ddl_Organization.ClearSelection()
                ddl_Organization.Items.Clear()
                ddl_Organization.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddl_Organization.DataValueField = "MAS_Org_ID"
                ddl_Organization.DataTextField = "Description"
                ddl_Organization.DataBind()
            ddl_Organization.Items.Insert(0, New RadComboBoxItem("Select Organisation", "0"))

                If ddl_org.Items.Count = 2 Then
                    Me.ddl_org.SelectedIndex = 1
                    LoadFSR()

                End If

                'Me.ddlSalesRep.Items.Clear()
                'ddlSalesRep.AppendDataBoundItems = True
                'ddlSalesRep.Items.Insert(0, "--Select a FSR--")
                'ddlSalesRep.Items(0).Value = ""

                lstDefault.Items.Clear()
                lstSelected.Items.Clear()
                ViewState("FileType") = Nothing
                ViewState("FileName") = Nothing
                ViewState("CSVName") = Nothing
                Session.Remove("APLogInfo")
                Session.Remove("dtAPErrors")
                SetErrorsTable()
            Else

                dtErrors = Session("dtAPErrors")

            End If

    End Sub
    Private Sub btn_close_Click(sender As Object, e As ImageClickEventArgs) Handles btn_close.Click
        Resetfields()
        MPEDivConfig.Hide()
        ClassUpdatePnl.Update()
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

    Private Function DoXLSUpload(ByVal filename As String, ByRef bfileformat As Boolean) As DataTable
        Dim dtImport As New DataTable
        Try
             Dim connString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & filename & " ;Extended Properties=""Excel 8.0;HDR=Yes;IMEX=1"""
            Dim oledbConn As New OleDbConnection(connString)
            Try
                oledbConn.Open()

                Dim cmd As New OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn)

                Dim oleda As New OleDbDataAdapter()

                oleda.SelectCommand = cmd



                oleda.Fill(dtImport)


bfileformat = True
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
    Private Function DoXLSXUpload(ByVal filename As String, ByRef bfileformat As Boolean) As DataTable
        Dim dtImport As New DataTable
        Try
            Dim connString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & filename & " ;Extended Properties=""Excel 12.0;HDR=Yes;IMEX=1"""
            Dim oledbConn As New OleDbConnection(connString)
            Try
                oledbConn.Open()

                Dim cmd As New OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn)

                Dim oleda As New OleDbDataAdapter()

                oleda.SelectCommand = cmd



                oleda.Fill(dtImport)


bfileformat = True
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
            If ddl_Organization.SelectedIndex = 0 Then
                lblmsgPopUp.Text = "Please Select the Organization."
                MPEDivConfig.Show()
                ClassUpdatePnl.Update()
                Return
            End If

            If Not ExcelFileUpload.HasFile Then
                lblmsgPopUp.Text = "Please Select a File to upload."
                MPEDivConfig.Show()
                ClassUpdatePnl.Update()
                Return
            End If
            Dim spath As String = ""
            spath = ConfigurationManager.AppSettings("ExcelPath")
            If Directory.Exists(spath) = False Then
                Directory.CreateDirectory(spath)
            End If
            spath = spath & Now.ToString("ddMMyyyyhhssmm") & ExcelFileUpload.FileName
            If (ExcelFileUpload.HasFile) Then
                If ExcelFileUpload.PostedFile.ContentLength > 10485760 Then
                   lblmsgPopUp.Text = "File Size should be less than 10 MB, Please Split files and upload."
                    MPEDivConfig.Show()
                Else
                    If System.IO.File.Exists(spath) Then
                        System.IO.File.Delete(spath)
                    End If
                    ExcelFileUpload.SaveAs(spath)
                    ExcelFileUpload.FileContent.Close()
                    ExcelFileUpload.FileContent.Dispose()
                    Dim success As Boolean
                    Dim RetMessage As String = ""
                    success = ImportFile(spath, RetMessage)

                    lblmsgPopUp.Text = RetMessage
                       BindGridError()
                       ddl_org.ClearSelection()
                If Not ddl_org.FindItemByValue(ddl_Organization.SelectedItem.Value) Is Nothing Then
                    ddl_org.FindItemByValue(ddl_Organization.SelectedItem.Value).Selected = True
                End If


                       UpdatePanel2.Update()
                       MPEDivConfig.Show()
                    ClassUpdatePnl.Update()
                End If
            End If

    End Sub
    Function ImportFile(ByVal File As String, ByRef RetMessage As String) As Boolean
      Dim OrgID As String
      OrgID = ddl_Organization.SelectedItem.Value
      Dim TempTbl As New DataTable
      Dim FinalTbl As New DataTable
      Dim ErrorTbl As New DataTable
      Dim bRetVal As Boolean = False
      Dim bImported As Boolean = True
      If File.ToLower.EndsWith(".xls") Then
          TempTbl = DoXLSUpload(File, bRetVal)
       ElseIf File.ToLower.EndsWith(".xlsx") Then
           TempTbl = DoXLSXUpload(File, bRetVal)
       End If
       If bRetVal = True Then
           ErrorTbl = TempTbl.Clone
           ErrorTbl.Columns.Add("Reason")
           ErrorTbl.Columns.Add("RowNo")

           FinalTbl = TempTbl.Clone
           FinalTbl.Columns.Add("SalesRep_ID")
           FinalTbl.Columns.Add("OrgID")
           If TempTbl.Columns.Count > 0 And TempTbl.Columns.Count <= 2 Then
            If TempTbl.Columns(0).ColumnName.ToLower = "item_code" Then
           If TempTbl.Rows.Count > 0 Then
               Dim Rowno As Integer = 1
               For Each dr In TempTbl.Rows
                    Dim Item_Code As String
                    Dim SalesRep_ID As String = ""
                    Item_Code = dr(0).ToString

                    Dim Item As String
                    Dim Item_ID As String = ""
                    Item = dr(1).ToString

                    If Item_Code = "" Or Item = "" Then
                        Dim ErrorDr As DataRow
                        ErrorDr = ErrorTbl.NewRow
                        ErrorDr(0) = dr(0)
                        ErrorDr(1) = dr(1)
                        ErrorDr(2) = "Blank Cell value/zero Qty"
                        ErrorDr(3) = Rowno
                        ErrorTbl.Rows.Add(ErrorDr)
                        bImported = False
                    ElseIf objProduct.CheckItemCode(Err_No, Err_Desc, Item_Code, OrgID) = False Then
                        Dim ErrorDr As DataRow
                        ErrorDr = ErrorTbl.NewRow
                        ErrorDr(0) = dr(0)
                        ErrorDr(1) = dr(1)
                        ErrorDr(2) = "Invalid item code"
                        ErrorDr(3) = Rowno
                        ErrorTbl.Rows.Add(ErrorDr)
                        bImported = False
                    Else
                        Dim finalDr As DataRow
                        finalDr = FinalTbl.NewRow
                        finalDr(0) = dr(0).ToString
                        finalDr(1) = dr(1).ToString
                        finalDr(2) = ddlSalesRepImp.SelectedItem.Value
                        finalDr(3) = OrgID
                        FinalTbl.Rows.Add(finalDr)
                    End If
                    Rowno = Rowno + 1
               Next
               If FinalTbl.Rows.Count > 0 Then
                Dim dtNotImported As New DataTable
                  dtNotImported.Columns.Add("VanCode")
                  Dim buploaded As Boolean
                  buploaded = objProduct.UploadFSRProduct(Me.ddl_Organization.SelectedValue, Me.ddlSalesRepImp.SelectedValue, FinalTbl, Err_No, Err_Desc)
                  FinalTbl = Nothing
                  If dtNotImported.Rows.Count > 0 Then
                   For Each notimportedDr As DataRow In dtNotImported.Rows
                     Dim ErrorDr As DataRow
                            ErrorDr = ErrorTbl.NewRow
                            ErrorDr(0) = notimportedDr(0)
                            ErrorDr(1) = ""
                            ErrorDr(2) = 0
                            ErrorDr(3) = "All rows of " & notimportedDr(0)
                            ErrorDr(4) = 0
                            ErrorTbl.Rows.Add(ErrorDr)
                            bImported = False
                  Next
                    RetMessage = "Imported with Errors.Please see the rows not imported below."
                 Else
                     If buploaded = True And bImported = True Then
                        RetMessage = "Imported Succesfully."
                     Else
                        RetMessage = "Imported with Errors.Please see the rows not imported below."
                     End If
                 End If
                    dtNotImported = Nothing
              Else
                     RetMessage = "No Valid Rows in the file to import."
                     bImported = False
              End If

              Session("Errordt") = ErrorTbl.Copy
          Else
             RetMessage = "No Rows in the file to import."
             bImported = False
          End If
          Else
            RetMessage = "Invalid Column Header."
             bImported = False
          End If
          Else
            RetMessage = "Invalid Column Count in the file."
             bImported = False
          End If
      Else
        RetMessage = "Invalid File format."
        bImported = False
      End If
     ErrorTbl = Nothing
     TempTbl = Nothing

    End Function
    Private Sub dgvErros_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgvErros.PageIndexChanging
        dgvErros.PageIndex = e.NewPageIndex
        BindGridError()
        MPEDivConfig.Show()
         ClassUpdatePnl.Update()

    End Sub

     Sub BindGridError()
         Try
            Dim dt As New DataTable
            dt = CType(Session("Errordt"), DataTable)
            Dim dv As New DataView(dt)
            
            dgvErros.DataSource = dv
            dgvErros.DataBind()
            If dt.Rows.Count > 0 Then
                BtnDownLoad.Visible = True
            End If


        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
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

        Session.Remove("dtAPErrors")
        Session("dtAPErrors") = dtErrors
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
    'Private Sub ExportToExcel(ByVal strFileName As String, ByVal ds As DataSet)


    '    Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & strFileName
    '    WriteXLSFile(fn, ds)

    '    Dim myfile As New FileInfo(fn)

    '    Response.ClearContent()
    '    Response.Buffer = True
    '    Response.AddHeader("content-disposition", "attachment;filename=" & strFileName)
    '    Response.AddHeader("Content-Length", fn.Length.ToString())
    '    Response.ContentType = "application/vnd.ms-excel"
    '    Response.TransmitFile(myfile.FullName)
    '    Response.End()

    'End Sub

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

    'Protected Sub lstDefault_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lstDefault.SelectedIndexChanged
    '    If Me.lblSelectedID.Text.Contains(Me.lstDefault.SelectedValue) = False Then
    '        Me.lblSelectedID.Text = Me.lblSelectedID.Text + Me.lstDefault.SelectedValue + ","
    '    ElseIf Me.lblSelectedID.Text.Contains(Me.lstDefault.SelectedValue) = True Then
    '        Me.lblSelectedID.Text = Me.lblSelectedID.Text.Replace(Me.lstDefault.SelectedValue + ",", "")
    '        Dim itm As RadListBoxItem = Me.lstDefault.FindItemByValue(Me.lstDefault.SelectedValue)
    '        itm.Selected = False
    '    End If
    '    Dim s() As String = Me.lblSelectedID.Text.Split(",")

    '    For i As Integer = 0 To s.Length - 1
    '        For Each itm As RadListBoxItem In lstDefault.Items
    '            If itm.Value = s(i).ToString() Then
    '                itm.Selected = True
    '                Exit For
    '            End If
    '        Next
    '    Next
    'End Sub

    'Protected Sub lstSelected_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lstSelected.SelectedIndexChanged



    '    If Me.lblRemovedID.Text.Contains(lstSelected.SelectedValue) = False Then
    '        Me.lblRemovedID.Text = Me.lblRemovedID.Text + lstSelected.SelectedValue + ","
    '    ElseIf Me.lblRemovedID.Text.Contains(lstSelected.SelectedValue) = True Then
    '        Me.lblRemovedID.Text = Me.lblRemovedID.Text.Replace(lstSelected.SelectedValue + ",", "")
    '        Dim itm As RadListBoxItem = lstSelected.FindItemByValue(lstSelected.SelectedValue)
    '        itm.Selected = False
    '    End If
    '    Dim s() As String = Me.lblRemovedID.Text.Split(",")

    '    For i As Integer = 0 To s.Length - 1
    '        For Each itm As RadListBoxItem In lstSelected.Items
    '            If itm.Value = s(i).ToString() Then
    '                itm.Selected = True
    '                Exit For
    '            End If
    '        Next
    '    Next

    'End Sub
     Private Sub LoadFSR()
        If ddl_org.SelectedIndex > 0 Then
            Dim objCommon As New Common
            ddlSalesRep.ClearSelection()
            ddlSalesRep.Items.Clear()

            ddlSalesRep.DataSource = objCommon.GetVanByOrg(Err_No, Err_Desc, ddl_org.SelectedValue, CType(Session("User_Access"), UserAccess).UserID)

            ddlSalesRep.DataValueField = "SalesRep_Id"
            ddlSalesRep.DataTextField = "SalesRep_Name"
            ddlSalesRep.DataBind()
            ddlSalesRep.Items.Insert(0, "Select Van/FSR")
            If ddlSalesRep.Items.Count = 2 Then
                ddlSalesRep.SelectedIndex = 1
            End If
            If ddl_org.SelectedIndex > 0 And ddlSalesRep.SelectedIndex > 0 Then
                ''Bind Default List
                BindDefault()

                ''Bind Selected List
                BindSelected()

                'lblProdAssign.Text = "Products Assigned: [" & lstSelected.Items.Count & "]"
                'lblProdAvailed.Text = "Products Available: [" & lstDefault.Items.Count & "]"
            Else
                lstDefault.Items.Clear()
                lstSelected.Items.Clear()
                lblProdAssign.Text = ""
                lblProdAvailed.Text = ""
            End If
        Else
            ddlSalesRep.ClearSelection()
            ddlSalesRep.Items.Clear()
            ddlSalesRep.Text = ""
            lstDefault.Items.Clear()
            lstSelected.Items.Clear()
            lblProdAssign.Text = ""
            lblProdAvailed.Text = ""
        End If

    End Sub
    Private Sub LoadFSRImport()
        If ddl_Organization.SelectedIndex > 0 Then
            Dim objCommon As New Common
            ddlSalesRepImp.ClearSelection()
            ddlSalesRepImp.Items.Clear()

            ddlSalesRepImp.DataSource = objCommon.GetVanByOrg(Err_No, Err_Desc, ddl_Organization.SelectedValue, CType(Session("User_Access"), UserAccess).UserID)

            ddlSalesRepImp.DataValueField = "SalesRep_Id"
            ddlSalesRepImp.DataTextField = "SalesRep_Name"
            ddlSalesRepImp.DataBind()

            If ddlSalesRepImp.Items.Count = 2 Then
                ddlSalesRepImp.SelectedIndex = 1
            End If
        End If

    End Sub
    Private Function HasAuthentication() As Boolean
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
    End Function

     Private Sub ddOraganisation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_org.SelectedIndexChanged

        LoadFSR()

        lstDefault.Items.Clear()
        lstSelected.Items.Clear()
        lblProdAssign.Text = ""
        lblProdAvailed.Text = ""

    End Sub
    
    Private Sub BindDefault()

        lstDefault.Items.Clear()
        lblProdAvailed.Text = ""

        Dim objProd As New Product
        Dim TempTbl As New DataTable
        Try
            Err_Desc = Nothing
            Err_No = Nothing

            Dim OrgID As String = "0"
            Dim SID As String = "0"

            If ddl_org.SelectedIndex > 0 Then
                OrgID = ddl_org.SelectedValue
            End If

            If ddlSalesRep.SelectedIndex > 0 Then
                SID = ddlSalesRep.SelectedValue
            End If


            TempTbl = objProd.GetDefaultProdctFSR(OrgID, SID, Err_No, Err_Desc)
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

    Private Sub BindSelected()

        lstSelected.Items.Clear()
        lblProdAssign.Text = ""
        Dim objProd As New Product
        Dim TempTbl As New DataTable
        Try
            Err_Desc = Nothing
            Err_No = Nothing

            Dim OrgID As String = "0"
            Dim SID As String = "0"

            If ddl_org.SelectedIndex > 0 Then
                OrgID = ddl_org.SelectedValue
            End If

            If ddlSalesRep.SelectedIndex > 0 Then
                SID = ddlSalesRep.SelectedValue
            End If


            TempTbl = objProd.GetSelectedFSRProduct(OrgID, SID, Err_No, Err_Desc)
            If Me.txtFilter.Text <> "" Then
                TempTbl.DefaultView.RowFilter = "(Description LIKE '%" & Me.txtFilter.Text & "%')"
            End If

            Dim dv As New DataView
            dv = TempTbl.DefaultView
            '  dv.Sort = "Description"
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

    Protected Sub imgAddSlected_Click(ByVal sender As Object, ByVal e As EventArgs)
            If Me.ddlSalesRep.SelectedIndex > 0 And Me.ddl_org.SelectedIndex > 0 Then
                Try
                    Err_No = Nothing
                    Err_Desc = Nothing
                    Dim objProd As New Product
                    Dim strDefault As String = Nothing
                    For Each Item As RadListBoxItem In lstDefault.SelectedItems
                        If Item.Selected Then
                            strDefault = strDefault & Item.Value & ","
                        End If
                    Next
                    If Not strDefault Is Nothing Then
                        strDefault = strDefault.Remove(strDefault.Length - 1, 1)
                        strDefault = strDefault
                        objProd.InsertFSRProduct("Single", Me.ddl_org.SelectedItem.Value, 0, 0, ddlSalesRep.SelectedItem.Value, Err_No, Err_Desc, strDefault)
                    End If

                    'For Each Item As RadListBoxItem In lstDefault.Items
                    '    If Item.Selected Then
                    '        objProd.InsertFSRProduct("Single", Me.ddOraganisation.SelectedItem.Value, Item.Value, 0, ddlSalesRep.SelectedItem.Value, Err_No, Err_Desc, Me.ddcategory.SelectedValue.ToString())
                    '    End If
                    'Next
                    BindDefault()
                    BindSelected()
                Catch ex As Exception
                    log.Error(ex.Message)
                    Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Product List Setup_006") & "&next=AssignProductToFSR.aspx&Title=Assign Product To FSR", False)
                End Try
             
        Else
            MessageBoxValidation("Please select a organization/van", "Information")
        End If

    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub imgRemoveSlected_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Me.ddlSalesRep.SelectedIndex > 0 And Me.ddl_org.SelectedIndex > 0 Then
            Try
                Err_No = Nothing
                Err_Desc = Nothing
                Dim objProd As New Product

                Dim strRemove As String = Nothing
                For Each Item As RadListBoxItem In lstSelected.SelectedItems
                    If Item.Selected Then
                        strRemove = strRemove & Item.Value & ","
                    End If
                Next
                If Not strRemove Is Nothing Then
                    strRemove = strRemove.Remove(strRemove.Length - 1, 1)
                    'objProd.InsertFSRProduct("Single", Me.ddOraganisation.SelectedItem.Value, 0, 0, ddlSalesRep.SelectedItem.Value, Err_No, Err_Desc, Me.ddcategory.SelectedValue.ToString(), strDefault)
                    objProd.DeleteFSRProduct("Single", ddl_org.SelectedItem.Value, 0, 0, ddlSalesRep.SelectedItem.Value, Err_No, Err_Desc, strRemove)
                End If



                'For Each Item As RadListBoxItem In lstSelected.Items
                '    If Item.Selected Then
                '        objProd.DeleteFSRProduct("Single", ddOraganisation.SelectedItem.Value, Item.Value, 0, ddlSalesRep.SelectedItem.Value, Err_No, Err_Desc)
                '    End If
                'Next
                BindDefault()
                BindSelected()
            Catch ex As Exception
                log.Error(ex.Message)
                Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Product List Setup_006") & "&next=AssignProductToFSR.aspx&Title=Assign Product To FSR", False)

            End Try
        Else
            MessageBoxValidation("Please select a organization/Van", "Information")
             

            Exit Sub
        End If
    End Sub


    Public Sub Resetfields()

      

        'Me.ddOraganisation.SelectedIndex = 0
        If ddl_org.Items.Count = 2 Then
            Me.ddl_org.SelectedIndex = 1
        Else
            Me.ddl_org.ClearSelection()
            Me.ddl_org.SelectedIndex = 0

        End If

         
        LoadFSR()

        Me.lstDefault.Items.Clear()
        Me.lstSelected.Items.Clear()
       
        Me.txtFilter.Text = ""
        Me.lblProdAssign.Text = ""
        Me.lblProdAvailed.Text = ""
        Me.lblSelectedID.Text = ""
        Me.lblRemovedID.Text = ""
        Session.Remove("APLogInfo")
    End Sub
    Protected Sub imgMoveAllLeft_Click(ByVal sender As Object, ByVal e As EventArgs)

 
            If Me.ddlSalesRep.SelectedIndex > 0 And Me.ddl_org.SelectedIndex > 0 Then
                Try
                    Err_No = Nothing
                    Err_Desc = Nothing
                    Dim objProd As New Product
                    ' For Each Item As RadListBoxItem In lstDefault.Items
                    objProd.InsertFSRProduct("ALL", ddl_org.SelectedItem.Value, 0, 0, ddlSalesRep.SelectedItem.Value, Err_No, Err_Desc, "ALL")
                    ' Next
                    BindDefault()
                    BindSelected()
                Catch ex As Exception
                    log.Error(ex.Message)
                    Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Product List Setup_006") & "&next=AssignProductToFSR.aspx&Title=Assign Product To FSR", False)
                End Try
        Else
            MessageBoxValidation("Please select a organization/Van", "Information")
             
            Exit Sub
            End If
       
    End Sub

    Protected Sub imgMoveAllRight_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Me.ddlSalesRep.SelectedIndex > 0 And Me.ddl_org.SelectedIndex > 0 Then
            Try
                Err_No = Nothing
                Err_Desc = Nothing
                Dim objProd As New Product
                '  For Each Item As RadListBoxItem In lstSelected.Items
                objProd.DeleteFSRProduct("ALL", ddl_org.SelectedItem.Value, 0, 0, ddlSalesRep.SelectedItem.Value, Err_No, Err_Desc, "ALL")
                'Next
                BindDefault()
                BindSelected()
            Catch ex As Exception
                log.Error(ex.Message)
                Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Product List Setup_006") & "&next=AssignProductToFSR.aspx&Title=Assign Product To FSR", False)

            End Try
        Else
            MessageBoxValidation("Please select a organization/Van", "Information")
             

        End If
    End Sub

    Protected Sub ddlSalesRep_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlSalesRep.SelectedIndexChanged
        Try
            If ddl_org.SelectedIndex > 0 And ddlSalesRep.SelectedIndex > 0 Then ' And Me.ddcategory.SelectedIndex > 0 Then
               
                    BindDefault()
                 


                BindSelected()
 
            Else
                lstDefault.Items.Clear()
                lstSelected.Items.Clear()
                lblProdAssign.Text = ""
                lblProdAvailed.Text = ""
            End If
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=Welcome.aspx&Title=Product List Setup", False)
        End Try

    End Sub

     
    Protected Sub btnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTn_Import.Click
        btnSave.Text = "Import"

        ExcelFileUpload.Enabled = True
        btnSave.Visible = True
        ddl_Organization.ClearSelection()
        dgvErros.DataSource = Nothing
        dgvErros.DataBind()
        ddlSalesRepImp.ClearSelection()
        BtnDownLoad.Visible = False
        Me.MPEDivConfig.Show()
        Session("Errordt") = Nothing
          ClassUpdatePnl.Update()

    End Sub

    Private Sub ddl_Organization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Organization.SelectedIndexChanged
        LoadFSRImport()
        MPEDivConfig.Show()
                ClassUpdatePnl.Update()
                Return
    End Sub

    Private Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        If Me.ddlSalesRep.SelectedIndex > 0 And Me.ddl_org.SelectedIndex > 0 Then
            BindDefault()
            BindSelected()
        Else

            MessageBoxValidation("Please select a organization/van", "Information")
             
        End If
    End Sub

    Private Sub Btn_Reset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_Reset.Click
        If Me.ddlSalesRep.SelectedIndex > 0 And Me.ddl_org.SelectedIndex > 0 Then
            txtFilter.Text = ""
            BindDefault()
            BindSelected()
        Else
            MessageBoxValidation("Please select a organization/van", "Information")
                ClassUpdatePnl.Update()
        End If
    End Sub

    Private Sub BTn_Export_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTn_Export.Click

        If Me.ddl_org.SelectedIndex > 0 And Me.ddlSalesRep.SelectedIndex > 0 Then
            Dim dtOriginal As New DataTable()
            Dim OrgID As String = Me.ddl_org.SelectedValue
            Dim objProd As New Product
            dtOriginal = objProd.LoadFSRProductTemplate(Err_No, Err_Desc, OrgID, Me.ddlSalesRep.SelectedValue)
            'Return Table consisting data
            'Create Tempory Table

            Dim dtTemp As New DataTable()

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
                'Dim fn As String = "AP" & Now.ToString("ddMMMyyHHmmss") + ".xls"
                Dim fn As String = "AssignFSRProduct" + ".xls"
                Dim d As New DataSet
                d.Tables.Add(dtTemp)

                ExportToExcel(fn, d)

            End If
        Else
            MessageBoxValidation("Please select a organization and van", "Information")
             

            Exit Sub
        End If
    End Sub
End Class