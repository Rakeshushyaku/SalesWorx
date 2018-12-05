Imports System.Data.OleDb
Imports System.IO
Imports log4net
Imports System.Data.SqlClient
Imports SalesWorx.BO.Common
Imports ExcelLibrary.SpreadSheet
Imports Telerik.Web.UI
Partial Public Class ImportVanLoad
    Inherits System.Web.UI.Page


    Dim Err_No As Long
    Dim Err_Desc As String

    Dim ObjCommon As Common
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim ObjStock As New SalesWorx.BO.Common.Stock
    ' Dim SortField As String = ""
    Private Const ModuleName As String = "ImportVanLoad.aspx"
    Private Const PageID As String = "P305"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
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
        If Not Page.IsPostBack() Then
            If Session.Item("USER_ACCESS") Is Nothing Then
                Session.Add("BringmeBackHere", ModuleName)
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            If Not HasAuthentication() Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If
            Filldropdowns()
            ViewState("Criteria") = "1=1"

        End If
        RegisterPostBackControl()
        lblmsgPopUp.Text = ""
    End Sub
    Private Function HasAuthentication() As Boolean
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
    End Function
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Sub Filldropdowns()
        Dim objCommon As New Common
        Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
        ddl_org.DataSource = (New SalesWorx.BO.Common.Product).GetOrganisation(Err_No, Err_Desc, SubQry)
        ddl_org.Items.Clear()
        ddl_org.Items.Insert(0, New RadComboBoxItem("--Select Organisation--"))
        ddl_org.AppendDataBoundItems = True
        ddl_org.DataValueField = "MAS_Org_ID"
        ddl_org.DataTextField = "Description"
        ddl_org.DataBind()
        ddl_org.Items(0).Value = 0

        ddl_Organization.DataSource = (New SalesWorx.BO.Common.Product).GetOrganisation(Err_No, Err_Desc, SubQry)
        ddl_Organization.Items.Clear()
        ddl_Organization.Items.Insert(0, New RadComboBoxItem("--Select Organisation--"))
        ddl_Organization.AppendDataBoundItems = True
        ddl_Organization.DataValueField = "MAS_Org_ID"
        ddl_Organization.DataTextField = "Description"
        ddl_Organization.DataBind()
        ddl_Organization.Items(0).Value = 0

         Dim objUserAccess As UserAccess
                            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

                            ddlVan.DataSource = objCommon.GetVanByOrg(Err_No, Err_Desc, ddl_org.SelectedValue, objUserAccess.UserID)
                            ddlVan.DataValueField = "SalesRep_ID"
                            ddlVan.DataTextField = "SalesRep_Name"
                            ddlVan.DataBind()
        ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))

     objCommon = Nothing

    End Sub
    Protected Sub btnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTn_Import.Click
        btnSave.Text = "Import"
        file_import.Enabled = True
        btnSave.Visible = True
        ddl_Organization.ClearSelection()
        dgvErros.DataSource = Nothing
        dgvErros.DataBind()
        BtnDownLoad.Visible = False
        Me.MPEDivConfig.Show()
        Session("Errordt") = Nothing
          ClassUpdatePnl.Update()
    End Sub
    Sub BindGrid()
        Try
            Dim dt As New DataTable
            dt = ObjStock.GetUnconfirmedStockRequisition(Err_No, Err_Desc, ddl_org.SelectedItem.Value, ddlVan.SelectedItem.Value)
            Dim dv As New DataView(dt)
            If SortField <> "" Then
                dv.Sort = (SortField & " ") + SortDirection
            End If
            GvStockRequ.DataSource = dv
            GvStockRequ.DataBind()
            RegisterPostBackControl()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
   
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
            If ddl_Organization.SelectedItem.Value = "0" Then
                lblmsgPopUp.Text = "Please Select the Organization."
                MPEDivConfig.Show()
                ClassUpdatePnl.Update()
                Return
            End If

            If Not file_import.HasFile Then
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
            spath = spath & "\VanStockupload\" & Now.ToString("ddMMyyyyhhssmm") & file_import.FileName
            If (file_import.HasFile) Then
                If file_import.PostedFile.ContentLength > 10485760 Then
                   lblmsgPopUp.Text = "File Size should be less than 10 MB, Please Split files and upload."
                    MPEDivConfig.Show()
                Else
                    If System.IO.File.Exists(spath) Then
                        System.IO.File.Delete(spath)
                    End If
                    file_import.SaveAs(spath)
                    file_import.FileContent.Close()
                    file_import.FileContent.Dispose()
                    Dim success As Boolean
                    Dim RetMessage As String = ""
                    success = ImportFile(spath, RetMessage)

                    lblmsgPopUp.Text = RetMessage
                       BindGridError()
                       ddl_org.ClearSelection()
                If Not ddl_org.FindItemByValue(ddl_Organization.SelectedItem.Value) Is Nothing Then
                    ddl_org.FindItemByValue(ddl_Organization.SelectedItem.Value).Selected = True
                End If

                       BindGrid()
                       UpdatePanel2.Update()
                       MPEDivConfig.Show()
                    ClassUpdatePnl.Update()
                End If
            End If

    End Sub
    Sub BindGridError()
         Try
            Dim dt As New DataTable
            dt = CType(Session("Errordt"), DataTable)
            Dim dv As New DataView(dt)
            If SortField <> "" Then
                dv.Sort = (SortField & " ") + SortDirection
            End If
            dgvErros.DataSource = dv
            dgvErros.DataBind()
            If dt.Rows.Count > 0 Then
                BtnDownLoad.Visible = True
            End If


        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
    Function ImportFile(ByVal File As String, ByRef RetMessage As String) As Boolean
     log.Info("Van stock Import " & File & " started by " & CType(Session("User_Access"), UserAccess).UserID)
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
           FinalTbl.Columns.Add("Item_ID")
           FinalTbl.Columns.Add("UOMQty")
           FinalTbl.Columns.Add("Item_UOM")

           If TempTbl.Rows.Count > 0 Then
               Dim Rowno As Integer = 1
               For Each dr In TempTbl.Rows
                    Dim Van As String
                    Dim SalesRep_ID As String = ""
                    Van = dr(0).ToString

                    Dim Item As String
                    Dim Item_ID As String = ""
                    Item = dr(1).ToString

                    Dim qty As String
                    qty = dr(2).ToString

                    'Dim UOM As String
                    Dim Conversion As String = "1"
                    'UOM = dr(3).ToString
                    If Van = "" Or Item = "" Or Val(qty) = 0 Then
                        Dim ErrorDr As DataRow
                        ErrorDr = ErrorTbl.NewRow
                        ErrorDr(0) = dr(0)
                        ErrorDr(1) = dr(1)
                        ErrorDr(2) = dr(2)
                        ErrorDr(3) = "Blank Cell value/zero Qty"
                        ErrorDr(4) = Rowno
                        ErrorTbl.Rows.Add(ErrorDr)
                        bImported = False
                    ElseIf Not ValidVan(Van, SalesRep_ID) Then
                        Dim ErrorDr As DataRow
                        ErrorDr = ErrorTbl.NewRow
                        ErrorDr(0) = dr(0)
                        ErrorDr(1) = dr(1)
                        ErrorDr(2) = dr(2)
                        ErrorDr(3) = "Van does not exist"
                        ErrorDr(4) = Rowno
                        ErrorTbl.Rows.Add(ErrorDr)
                        bImported = False
                    'ElseIf AlreadyConfirmed(SalesRep_ID) Then
                    '    Dim ErrorDr As DataRow
                    '    ErrorDr = ErrorTbl.NewRow
                    '    ErrorDr(0) = dr(0)
                    '    ErrorDr(1) = dr(1)
                    '    ErrorDr(2) = dr(2)
                    '    ErrorDr(3) = "Stock Requisition is confirmed for this Van"
                    '    ErrorDr(4) = Rowno
                    '    ErrorTbl.Rows.Add(ErrorDr)
                    '    bImported = False
                    ElseIf Not ValidItem(Item, Item_ID) Then
                        Dim ErrorDr As DataRow
                        ErrorDr = ErrorTbl.NewRow
                        ErrorDr(0) = dr(0)
                        ErrorDr(1) = dr(1)
                        ErrorDr(2) = dr(2)
                        ErrorDr(3) = "Item does not exist"
                        ErrorDr(4) = Rowno
                        ErrorTbl.Rows.Add(ErrorDr)
                        bImported = False
                    'ElseIf Not ValidItemUOM(Item, UOM, Conversion) Then
                    '    Dim ErrorDr As DataRow
                    '    ErrorDr = ErrorTbl.NewRow
                    '    ErrorDr(0) = dr(0)
                    '    ErrorDr(1) = dr(1)
                    '    ErrorDr(2) = dr(2)
                    '    ErrorDr(3) = dr(3)
                    '    ErrorDr(4) = "UOM does not exist for this Item"
                    '    ErrorDr(5) = Rowno
                    '    ErrorTbl.Rows.Add(ErrorDr)
                    '    bImported = False
                    ElseIf Not ValidQty((Val(dr(2).ToString()) * Val(Conversion)).ToString) Then
                        Dim ErrorDr As DataRow
                        ErrorDr = ErrorTbl.NewRow
                        ErrorDr(0) = dr(0)
                        ErrorDr(1) = dr(1)
                        ErrorDr(2) = dr(2)
                        'ErrorDr(4) = "Qty (in BASE UOM) is not valid:(" & (Val(dr(2).ToString()) * Val(Conversion)).ToString & ")"
                        ErrorDr(3) = "Qty is not valid:(" & (Val(dr(2).ToString()) * Val(Conversion)).ToString & ")"
                        ErrorDr(4) = Rowno
                        ErrorTbl.Rows.Add(ErrorDr)
                        bImported = False
                    Else
                        Dim finalDr As DataRow
                        finalDr = FinalTbl.NewRow
                        finalDr(0) = dr(0).ToString
                        finalDr(1) = dr(1).ToString
                        finalDr(2) = Val(dr(2).ToString())
                        finalDr(3) = SalesRep_ID
                        finalDr(4) = Item_ID
                        finalDr(5) = Val(dr(2).ToString()) * Val(Conversion)
                        finalDr(6) = ObjStock.GetItemUOM(Err_No, Err_Desc, ddl_Organization.SelectedItem.Value, dr(1).ToString)
                        FinalTbl.Rows.Add(finalDr)
                    End If
                    Rowno = Rowno + 1
               Next
               If FinalTbl.Rows.Count > 0 Then
                Dim dtNotImported As New DataTable
                  dtNotImported.Columns.Add("VanCode")
                  Dim buploaded As Boolean
                  buploaded = ObjStock.ImportStockRequisitions(Err_No, Err_Desc, ddl_Organization.SelectedItem.Value, FinalTbl, dtNotImported)
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
        RetMessage = "Invalid File format."
        bImported = False
      End If
     ErrorTbl = Nothing
     TempTbl = Nothing
     log.Info("Van stock Import " & RetMessage)
     log.Info("Van stock Import finished for " & File)
    End Function
    Function ValidQty(ByVal Qty As String)
        Dim bRetVal As Boolean
        Dim num As Integer
        If Integer.TryParse(Qty, num) Then
            bRetVal = True
        Else
            bRetVal = False
        End If
        Return bRetVal
    End Function
    Function ValidVan(ByVal van As String, ByRef Salesrep_ID As String) As Boolean
        Return ObjStock.IsValidVan(Err_No, Err_Desc, ddl_Organization.SelectedItem.Value, van, Salesrep_ID)
    End Function
    Function ValidItem(ByVal Item As String, ByRef Item_ID As String) As Boolean
         Return ObjStock.ValidItem(Err_No, Err_Desc, ddl_Organization.SelectedItem.Value, Item, Item_ID)
    End Function
    Function ValidItemUOM(ByVal Item As String, ByVal UOM As String, ByRef Conversion As String) As Boolean
        Return ObjStock.ValidItemUOM(Err_No, Err_Desc, ddl_Organization.SelectedItem.Value, Item, UOM, Conversion)
    End Function
    Function AlreadyConfirmed(ByVal Salesrep_ID As String) As Boolean
        Return ObjStock.AlreadyConfirmed(Err_No, Err_Desc, ddl_Organization.SelectedItem.Value, Salesrep_ID)
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
                'If dtImport.Columns.Count = 4 Then
                ' If dtImport.Columns(0).ColumnName.ToLower = "van_location" And dtImport.Columns(1).ColumnName.ToLower = "item_code" And dtImport.Columns(2).ColumnName.ToLower = "load_quantity" And dtImport.Columns(3).ColumnName.ToLower = "uom" Then
                '    bfileformat = True
                ' End If
                'End If

                If dtImport.Columns.Count = 3 Then
                 If dtImport.Columns(0).ColumnName.ToLower = "van_location" And dtImport.Columns(1).ColumnName.ToLower = "item_code" And dtImport.Columns(2).ColumnName.ToLower = "load_quantity" Then
                    bfileformat = True
                 End If
                End If
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

    Private Sub DeleteExcel(ByVal filename As String)
        Try
            Dim TheFile As FileInfo = New FileInfo(ViewState("FileName"))
            If TheFile.Exists Then
                File.Delete(filename)
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

                
                If dtImport.Columns.Count = 3 Then
                 If dtImport.Columns(0).ColumnName.ToLower = "van_location" And dtImport.Columns(1).ColumnName.ToLower = "item_code" And dtImport.Columns(2).ColumnName.ToLower = "load_quantity" Then
                    bfileformat = True
                 End If
                End If
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
    
    

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        MPEDivConfig.Hide()

    End Sub
    Sub CsvExport()
    Dim dt As New DataTable
    dt = CType(Session("Errordt"), DataTable)
        'Build the CSV file data as a Comma separated string.
                    Dim csv As String = String.Empty

                    For Each column As DataColumn In dt.Columns
                        'Add the Header row for CSV file.
                        csv += column.ColumnName + ","c
                    Next

                    'Add new line.
                    csv += vbCr & vbLf

                    For Each row As DataRow In dt.Rows
                        For Each column As DataColumn In dt.Columns
                            'Add the Data rows.
                            csv += row(column.ColumnName).ToString().Replace(",", ";") + ","c
                        Next

                        'Add new line.
                        csv += vbCr & vbLf
                    Next

                    'Download the CSV file.
                    Response.Clear()
                    Response.Buffer = True
                    Response.AddHeader("content-disposition", "attachment;filename=ErrorExport.csv")
                    Response.Charset = ""
                    Response.ContentType = "application/text"
                    Response.Output.Write(csv)
                    Response.Flush()
                    Response.End()
    End Sub

     Protected Sub btnView_Click(ByVal sender As Object, ByVal e As EventArgs)
         Dim btnConform As LinkButton = TryCast(sender, LinkButton)
        Dim row As GridViewRow = DirectCast(btnConform.NamingContainer, GridViewRow)


        Dim success As Boolean = False
        Try
            Dim HRowID As String
            HRowID = CType(row.FindControl("HRowID"), HiddenField).Value
            RowID.Value = HRowID
            lbl_Selvan.Text = row.Cells(2).Text
            Dim stdt As New DataTable
            stdt = ObjStock.StockRequisitionItems(Err_No, Err_Desc, HRowID)
            GVItems.DataSource = stdt
            GVItems.DataBind()
           ' ClassUpdatePnl.Update()
            Me.MPStockDetails.Show()

        Catch ex As Exception
            Err_No = "74210"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

     End Sub
     Protected Sub btnDownloadSt_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btnConform As LinkButton = TryCast(sender, LinkButton)
        Dim row As GridViewRow = DirectCast(btnConform.NamingContainer, GridViewRow)


        Dim success As Boolean = False
        Try
            Dim HRowID As String
            HRowID = CType(row.FindControl("HRowID"), HiddenField).Value
            RowID.Value = HRowID
            lbl_Selvan.Text = row.Cells(2).Text
            Dim stdt As New DataTable
            stdt = ObjStock.StockRequisitionItemsforExport(Err_No, Err_Desc, HRowID)
            ExportToExcel("VanLoad.xls", stdt)
        Catch ex As Exception
            Err_No = "74210"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

      End Sub
      Private Sub RegisterPostBackControl()
        For Each row As GridViewRow In GvStockRequ.Rows
            Dim lnkFull As LinkButton = TryCast(row.FindControl("btnDownloadSt"), LinkButton)
            ScriptManager.GetCurrent(Me).RegisterPostBackControl(lnkFull)
        Next
    End Sub
       Private Sub ExportToExcel(ByVal strFileName As String, ByVal ds As DataTable)
        Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & strFileName
        WriteXLSFile(fn, ds)
        Dim file As New FileInfo(fn)
        Response.Clear()
        Response.ClearHeaders()
        Response.ClearContent()
        Response.AddHeader("content-disposition", "attachment; filename=" + strFileName)
        Response.AddHeader("Content-Type", "application/Excel")
        Response.ContentType = "application/vnd.xls"
        Response.AddHeader("Content-Length", file.Length.ToString())
        Response.WriteFile(file.FullName)
        Response.End()
    End Sub
     Public Function WriteXLSFile(ByVal pFileName As String, ByVal dt As DataTable) As Boolean
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
    Protected Sub btnConform_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btnConform As ImageButton = TryCast(sender, ImageButton)
        Dim row As GridViewRow = DirectCast(btnConform.NamingContainer, GridViewRow)


        Dim success As Boolean = False
        Try
            Dim HRowID As String
            HRowID = CType(row.FindControl("HRowID"), HiddenField).Value

           Dim PoRefno As String
           PoRefno = "PO-" & Now.ToString("ddMMyyyyhhmmss")

            If ObjStock.ConfirmStockRequisitionbyID(Err_No, Err_Desc, HRowID, CType(Session("User_Access"), UserAccess).UserID, PoRefno) = True Then
                success = True
                log.Info(HRowID & " Successfully Confirmed.")
            End If

            If success = True Then
                MessageBoxValidation("Successfully Confirmed.", "Information")
                BindGrid()
            Else
                MessageBoxValidation("Error occured while confirming.", "Information")
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
    Protected Sub btnConfirmAll_Click()
        Try
            Dim row As GridViewRow
            Dim Success As Boolean = False
            Dim idCollection As String = ""
            For Each row In GvStockRequ.Rows
                Dim RowCheckBox As System.Web.UI.WebControls.CheckBox = row.FindControl("chkDelete")

                If RowCheckBox.Checked = True Then
                     Dim HRowID As String
                     HRowID = CType(row.FindControl("HRowID"), HiddenField).Value
                     Dim PoRefno As String
                     PoRefno = "PO-" & Now.ToString("ddMMyyyyhhmmss")
                     If ObjStock.ConfirmStockRequisitionbyID(Err_No, Err_Desc, HRowID, CType(Session("User_Access"), UserAccess).UserID, PoRefno) = True Then
                            Success = True
                            log.Info(HRowID & " Successfully Confirmed.")
                     End If
               End If
            Next

            BindGrid()
        Catch ex As Exception
            Err_No = "74211"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFilter.Click
    If ddl_org.SelectedItem.Value = "0" Then
            MessageBoxValidation("Please select the Organization", "Validation")
        Exit Sub
    End If
        BindGrid()
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

    Protected Sub gvDivConfig_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GvStockRequ.Sorting
        SortField = e.SortExpression
        SortDirection = "flip"
        BindGrid()
    End Sub


    Protected Sub gvDivConfig_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvStockRequ.PageIndexChanging
        GvStockRequ.PageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Private Sub ddl_org_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_org.SelectedIndexChanged
          Dim objUserAccess As UserAccess
                            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
                             Dim objCommon As New Common
                            ddlVan.DataSource = objCommon.GetVanByOrg(Err_No, Err_Desc, ddl_org.SelectedValue, objUserAccess.UserID)
                            ddlVan.DataValueField = "SalesRep_ID"
                            ddlVan.DataTextField = "SalesRep_Name"
                            ddlVan.DataBind()
        ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))
objCommon = Nothing
    End Sub

    Private Sub GVItems_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GVItems.PageIndexChanging
         GVItems.PageIndex = e.NewPageIndex
         Dim stdt As New DataTable
            stdt = ObjStock.StockRequisitionItems(Err_No, Err_Desc, RowID.Value)
            GVItems.DataSource = stdt
            GVItems.DataBind()
            ClassUpdatePnl.Update()
            Me.MPStockDetails.Show()
    End Sub
    Private Sub btn_close_Click(sender As Object, e As ImageClickEventArgs) Handles btn_close.Click
        MPEDivConfig.Hide()
        ClassUpdatePnl.Update()
    End Sub

    Private Sub btn_Confirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Confirm.Click



        Dim success As Boolean = False
        Try
            Dim HRowID As String
            HRowID = RowID.Value

           Dim PoRefno As String
           PoRefno = "PO-" & Now.ToString("ddMMyyyyhhmmss")

            If ObjStock.ConfirmStockRequisitionbyID(Err_No, Err_Desc, HRowID, CType(Session("User_Access"), UserAccess).UserID, PoRefno) = True Then
                success = True
                log.Info(HRowID & " Successfully Confirmed.")
            End If

            If success = True Then
                MessageBoxValidation("Successfully Confirmed.", "Information")
                MPStockDetails.Hide()
               BindGrid()
               log.Info(HRowID & " Successfully Confirmed.")
            Else
                MessageBoxValidation("Error occured while confirming.", "Information")
                MPStockDetails.Hide()
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

    Private Sub dgvErros_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgvErros.PageIndexChanging
        dgvErros.PageIndex = e.NewPageIndex
        BindGridError()
        MPEDivConfig.Show()
         ClassUpdatePnl.Update()

    End Sub

    Private Sub BtnDownLoad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnDownLoad.Click
        CsvExport()
    End Sub

    Private Sub BTn_Export_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTn_Export.Click
    If ddl_org.SelectedItem.Value = "0" Then
            MessageBoxValidation("Please select the Organization", "Validation")
         ClassUpdatePnl.Update()
        Exit Sub
    End If
    Dim stdt As New DataTable
            stdt = ObjStock.StockRequisitionItemsbyOrgforExport(Err_No, Err_Desc, ddl_org.SelectedItem.Value)
            ExportToExcel("VanLoad.xls", stdt)

   End Sub

End Class
