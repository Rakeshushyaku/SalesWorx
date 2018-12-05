
Imports SalesWorx.BO.Common

Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports ExcelLibrary.SpreadSheet
Partial Public Class BonusDefinition
    Inherits System.Web.UI.Page
    Dim objProduct As New Product


    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const PageID As String = "P220"
    Private dt As New DataTable
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objControl As New AppControl

    Protected Sub btnGoBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGo.Click
        Response.Redirect("AdminBonusPromotion.aspx")
    End Sub

    Private Sub BonusDefinition_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Bonus Definition"
    End Sub


    'Protected Sub hdnValue4_ValueChanged(ByVal sender As Object, ByVal e As EventArgs)
    '    Me.txtBItemCode.Text = ""
    '    Dim selectedWidgetID As String = DirectCast(sender, HiddenField).Value
    '    GetBonusProductId()
    'End Sub

    'Protected Sub hdnValue3_ValueChanged(ByVal sender As Object, ByVal e As EventArgs)
    '    Me.txtBDescription.Text = ""
    '    Dim selectedWidgetID As String = DirectCast(sender, HiddenField).Value
    '    GetBonusProductId()
    'End Sub



    'Protected Sub hdnValue2_ValueChanged(ByVal sender As Object, ByVal e As EventArgs)
    '    Me.txtItemCode.Text = ""
    '    Dim selectedWidgetID As String = DirectCast(sender, HiddenField).Value
    '    GetDefProductId()
    'End Sub

    'Protected Sub hdnValue1_ValueChanged(ByVal sender As Object, ByVal e As EventArgs)
    '    Me.txtDescription.Text = ""
    '    Dim selectedWidgetID As String = DirectCast(sender, HiddenField).Value
    '    GetDefProductId()
    'End Sub



    Protected Sub ddl_org_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_org.SelectedIndexChanged

        If ddl_org.SelectedItem.Text <> "-- Select a Organization --" Then
            ResetDetails()
            'ACEtxtItemCode.ContextKey = Me.ddl_org.SelectedValue.ToString()
            'ACEtxtDesc.ContextKey = Me.ddl_org.SelectedValue.ToString()
            'ACEBitemCode.ContextKey = Me.ddl_org.SelectedValue.ToString()
            'ACEBDesc.ContextKey = Me.ddl_org.SelectedValue.ToString()
            Me.hfOrgID.Value = Me.ddl_org.SelectedValue.ToString()
            'Me.txtItemCode.Enabled = True
            'Me.txtDescription.Enabled = True


            Me.ddlOrdDesc.Enabled = True
            Me.ddlOrdCode.Enabled = True

            FillItemsList()
            BindBonusData()


        Else
            'ACEtxtItemCode.ContextKey = ""
            'ACEtxtDesc.ContextKey = ""
            'ACEBitemCode.ContextKey = ""
            'ACEBDesc.ContextKey = ""
            Me.hfOrgID.Value = ""
            Me.ddlOrdDesc.Enabled = False
            Me.ddlOrdCode.Enabled = False
            ResetDetails()
            FillItemsList()
            BindBonusData()

        End If

    End Sub

    Private Sub btnImportWindow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportWindow.Click
        If Me.ddl_org.SelectedIndex > 0 Then
            Me.lblUpMsg.Text = ""
            Session("dtBonusRules") = Nothing
            dgvErros.DataSource = Nothing
            dgvErros.DataBind()
            dgvErros.Visible = False
            lbLog.Visible = False
            Me.MPEImport.Show()
        End If
    End Sub


    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try

            If Me.ddl_org.SelectedIndex > 0 Then
                Dim dtOriginal As New DataTable()
                Dim OrgID As String = IIf(Me.hfOrgID.Value = "", "0", Me.hfOrgID.Value)
                Dim BnsPlanID As String = IIf(Me.hfPlanId.Value = "", "0", Me.hfPlanId.Value)

                dtOriginal = objProduct.ExportBonusData(Err_No, Err_Desc, OrgID, BnsPlanID)

                Dim dtTemp As New DataTable()

                'Creating Header Row
                dtTemp.Columns.Add("OrgID")
                dtTemp.Columns.Add("OrderItem")
                dtTemp.Columns.Add("BonusItem")
                dtTemp.Columns.Add("FromQty")
                dtTemp.Columns.Add("ToQty")
                dtTemp.Columns.Add("Type")
                dtTemp.Columns.Add("GetQty")
                dtTemp.Columns.Add("ValidFrom")
                dtTemp.Columns.Add("ValidTo")


                Dim drAddItem As DataRow
                For i As Integer = 0 To dtOriginal.Rows.Count - 1
                    drAddItem = dtTemp.NewRow()
                    drAddItem(0) = dtOriginal.Rows(i)("OrgID").ToString()
                    drAddItem(1) = dtOriginal.Rows(i)("OrderItem").ToString()
                    drAddItem(2) = dtOriginal.Rows(i)("BonusItem").ToString()
                    drAddItem(3) = CLng(dtOriginal.Rows(i)("FromQty").ToString())
                    drAddItem(4) = CLng(dtOriginal.Rows(i)("ToQty").ToString())
                    drAddItem(5) = dtOriginal.Rows(i)("Type").ToString()
                    drAddItem(6) = CLng(dtOriginal.Rows(i)("GetQty").ToString())
                    drAddItem(7) = DateTime.Parse(dtOriginal.Rows(i)("ValidFrom").ToString()).ToString("MM/dd/yyyy")
                    drAddItem(8) = DateTime.Parse(dtOriginal.Rows(i)("ValidTo").ToString()).ToString("MM/dd/yyyy")

                    dtTemp.Rows.Add(drAddItem)
                Next

                If dtOriginal.Rows.Count = 0 Then

                    Me.lblinfo.Text = "Information"
                    Me.lblMessage.Text = "There is no data."
                    Me.lblMessage.ForeColor = Drawing.Color.Red
                    Me.MpInfoError.Show()
                    Exit Sub

                    drAddItem = dtTemp.NewRow()
                    drAddItem(0) = ""
                    drAddItem(1) = ""
                    drAddItem(2) = ""
                    drAddItem(3) = ""
                    drAddItem(4) = ""
                    drAddItem(5) = ""
                    drAddItem(6) = ""
                    drAddItem(7) = ""
                    drAddItem(8) = ""
                    dtTemp.Rows.Add(drAddItem)
                End If

                'Temp(Grid)
                Dim dg As New DataGrid()
                dg.DataSource = dtTemp
                dg.DataBind()
                If dtTemp.Rows.Count > 0 Then
                    'Dim fn As String = "MSL" & Now.ToString("ddMMMyyHHmmss") + ".xls"
                    Dim fn As String = "SimpleBonusRules" + ".xls"
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





                'Me.txtBItemCode.Enabled = False
                'Me.txtBDescription.Enabled = False
                'Me.txtItemCode.Enabled = False
                'Me.txtDescription.Enabled = False


                Me.ddlGetCode.Enabled = False
                Me.ddlgetDesc.Enabled = False
                Me.ddlOrdDesc.Enabled = False
                Me.ddlOrdCode.Enabled = False

                LoadOrgHeads()

                ResetDetails()

                Me.lblPlanName.Text = Request.QueryString("Desc").ToString()
                Me.hfPlanId.Value = Request.QueryString("PGID").ToString()

                Me.lblOrg.Text = Request.QueryString("ORGNAME").ToString()
                Me.hfOrgID.Value = Request.QueryString("ORGID").ToString()
                Me.ddl_org.SelectedValue = hfOrgID.Value
                FillItemsList()
                Me.ddlOrdDesc.Enabled = True
                Me.ddlOrdCode.Enabled = True

                BindBonusData()

                ViewState("FileType") = Nothing
                ViewState("FileName") = Nothing
                ViewState("CSVName") = Nothing
              

                'Dim objCommon As New Common
                'Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                'ddOraganisation.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
                'ddOraganisation.Items.Clear()
                ' ddOraganisation.Items.Add("-- Select a Organization --")
                ' ddOraganisation.AppendDataBoundItems = True
                'ddOraganisation.DataValueField = "Org_ID"
                'ddOraganisation.DataTextField = "Description"
                'ddOraganisation.DataBind()
                'If ddOraganisation.Items.Count > 1 Then
                'Me.ddOraganisation.SelectedIndex = 1
                'ACEtxtItemCode.ContextKey = Me.hfOrgID.Value
                'ACEtxtDesc.ContextKey = Me.hfOrgID.Value
                'ACEBitemCode.ContextKey = Me.hfOrgID.Value
                'ACEBDesc.ContextKey = Me.hfOrgID.Value
                'Me.txtItemCode.Enabled = True
                'Me.txtDescription.Enabled = True
                Me.ddlOrdCode.Enabled = True
                Me.ddlOrdDesc.Enabled = True
                'ResetDetails()
                BindBonusData()
           
                ' End If
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

        If Me.hfPlanId.Value = "" Or Me.hfOrgID.Value = "" Then
            lblMessage.Text = "Organization and bonus plan id is empty."
            lblMessage.ForeColor = Drawing.Color.Green
            lblinfo.Text = "Information"
            MpInfoError.Show()
            Exit Sub
        End If

        Session("dtBonusRules") = Nothing
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
                        col.ColumnName = "OrgID"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)


                        col = New DataColumn
                        col.ColumnName = "OrderItem"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)



                        col = New DataColumn
                        col.ColumnName = "BonusItem"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)




                        col = New DataColumn
                        col.ColumnName = "FromQty"
                        col.DataType = System.Type.GetType("System.Double")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)


                        col = New DataColumn
                        col.ColumnName = "ToQty"
                        col.DataType = System.Type.GetType("System.Double")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)


                        col = New DataColumn
                        col.ColumnName = "Type"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)


                        col = New DataColumn
                        col.ColumnName = "GetQty"
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



                        If ViewState("FileName").ToString.EndsWith(".csv") Then
                            TempTbl = DoCSVUpload()
                        ElseIf ViewState("FileName").ToString.EndsWith(".xls") Then
                            TempTbl = DoXLSUpload()
                        ElseIf ViewState("FileName").ToString.EndsWith(".xlsx") Then
                            TempTbl = DoXLSXUpload()
                        End If

                        If TempTbl.Columns.Count = 9 Then
                            If Not (TempTbl.Columns(0).ColumnName = "OrgID" And TempTbl.Columns(1).ColumnName = "OrderItem" And TempTbl.Columns(2).ColumnName = "BonusItem" And TempTbl.Columns(3).ColumnName = "FromQty" And TempTbl.Columns(4).ColumnName = "ToQty" And TempTbl.Columns(5).ColumnName = "Type" And TempTbl.Columns(6).ColumnName = "GetQty" And TempTbl.Columns(7).ColumnName = "ValidFrom" And TempTbl.Columns(8).ColumnName = "ValidTo") Then
                                lblMessage.Text = "Please check the template columns are correct"
                                lblMessage.ForeColor = Drawing.Color.Green
                                lblinfo.Text = "Information"
                                MpInfoError.Show()
                                Exit Sub
                            End If
                        Else
                            lblMessage.Text = "Invalid Template"
                            lblMessage.ForeColor = Drawing.Color.Green
                            lblinfo.Text = "Information"
                            MpInfoError.Show()
                            Exit Sub
                        End If




                        If TempTbl.Rows.Count = 0 Then
                            lblMessage.Text = "There is no data in your file."
                            lblMessage.ForeColor = Drawing.Color.Green
                            lblinfo.Text = "Information"
                            MpInfoError.Show()
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
                                Dim OrderItem As String = Nothing
                                Dim OrderUOM As String = "EA"
                                Dim BonusItem As String = Nothing
                                Dim BonusUOM As String = "EA"
                                Dim FromQty As String = Nothing
                                Dim ToQty As String = Nothing
                                Dim Type As String = Nothing
                                Dim GetQty As String = Nothing
                                Dim ValidFrom As String = Nothing
                                Dim ValidTo As String = Nothing

                                Dim FromDate As Date = Nothing
                                Dim ToDate As Date = Nothing

                                Dim isValidRow As Boolean = True

                                OrgID = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, "0", TempTbl.Rows(idx)(0).ToString())
                                OrderItem = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, "0", TempTbl.Rows(idx)(1).ToString())
                                BonusItem = IIf(TempTbl.Rows(idx)(2) Is DBNull.Value, "0", TempTbl.Rows(idx)(2).ToString())
                                FromQty = IIf(TempTbl.Rows(idx)(3) Is DBNull.Value, "0", TempTbl.Rows(idx)(3).ToString())
                                ToQty = IIf(TempTbl.Rows(idx)(4) Is DBNull.Value, "0", TempTbl.Rows(idx)(4).ToString())
                                Type = IIf(TempTbl.Rows(idx)(5) Is DBNull.Value, "0", TempTbl.Rows(idx)(5).ToString())
                                GetQty = IIf(TempTbl.Rows(idx)(6) Is DBNull.Value, "0", TempTbl.Rows(idx)(6).ToString())
                                ValidFrom = Trim(Replace(IIf(TempTbl.Rows(idx)(7) Is DBNull.Value, "0", TempTbl.Rows(idx)(7).ToString()), "12:00:00 AM", ""))
                                ValidTo = Trim(Replace(IIf(TempTbl.Rows(idx)(8) Is DBNull.Value, "0", TempTbl.Rows(idx)(8).ToString()), "12:00:00 AM", ""))

                                If OrgID = 0 Or OrgID Is Nothing Then
                                    Continue For
                                End If

                                If OrgID <> "0" Then

                                    If OrgID = "0" Or objProduct.CheckOrgID(Err_No, Err_Desc, OrgID) = False Then
                                        RowNo = idx + 2
                                        '  ColNo = "1" + ","
                                        ' ColumnName = "OrgID" + ","
                                        ErrorText = "Invalid Org ID" + ","
                                        isValidRow = False
                                        TotFailed += 1
                                    End If

                                    If OrgID <> hfOrgID.Value Then
                                        RowNo = idx + 2
                                        '  ColNo = "1" + ","
                                        ' ColumnName = "OrgID" + ","
                                        ErrorText = "The selected org id does not match with this row org id" + ","
                                        isValidRow = False
                                        TotFailed += 1
                                    End If

                                    If OrderItem = "0" Or objProduct.CheckItemCode(Err_No, Err_Desc, OrderItem, OrgID) = False Then
                                        RowNo = idx + 2
                                        ' ColNo = ColNo + "2" + ","
                                        ' ColumnName = ColumnName + "OrderItem" + ","
                                        ErrorText = ErrorText + "Invalid order Item code" + ","
                                        isValidRow = False
                                        TotFailed += 1
                                    Else
                                        OrderUOM = objProduct.GetItemUOM(Err_No, Err_Desc, OrderItem, OrgID)
                                    End If

                                    If BonusItem = "0" Or objProduct.CheckItemCode(Err_No, Err_Desc, BonusItem, OrgID) = False Then
                                        RowNo = idx + 2
                                        ' ColNo = ColNo + "3" + ","
                                        ' ColumnName = ColumnName + "BonusItem" + ","
                                        ErrorText = ErrorText + "Invalid bonus item code" + ","
                                        isValidRow = False
                                        TotFailed += 1
                                    Else
                                        BonusUOM = objProduct.GetItemUOM(Err_No, Err_Desc, OrderItem, OrgID)
                                    End If

                                    If FromQty = "0" Or IsNumeric(FromQty) = False Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Invalid from qty" + ","
                                        isValidRow = False
                                        TotFailed += 1
                                    End If

                                    If ToQty = "0" Or IsNumeric(ToQty) = False Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Invalid to qty" + ","
                                        isValidRow = False
                                        TotFailed += 1
                                    End If

                                    If FromQty <> "0" And ToQty <> "0" And IsNumeric(FromQty) = True And IsNumeric(ToQty) = True Then
                                        If CLng(ToQty) <= CLng(FromQty) Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "To qty should be greater than from qty" + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End If
                                    End If

                                    'If Type = "0" Or Not (Type.ToUpper() = "POINT" Or Type.ToUpper() = "RECURRING" Or Type.ToUpper() = "PERCENT") Then
                                    If Type = "0" Or Not (Type.ToUpper() = "POINT" Or Type.ToUpper() = "RECURRING") Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Invalid type" + ","
                                        isValidRow = False
                                        TotFailed += 1
                                    End If

                                    If GetQty = "0" Or IsNumeric(GetQty) = False Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Invalid get qty" + ","
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
                                        Dim x As New DataTable
                                        x = objProduct.CheckBonusDataActiveRange(Err_No, Err_Desc, OrderItem, CLng(IIf(FromQty = "", "0", FromQty)), CLng(IIf(ToQty = "", "0", ToQty)), OrgID, FromDate, ToDate, "0")
                                        If x.Rows.Count > 0 Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "Same rule already exist for this item" + ","
                                            TotFailed += 1
                                        End If
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

                                        If objProduct.SaveBonusData(Err_No, Err_Desc, OrderItem, OrgID, OrderUOM, BonusItem, BonusUOM, Type, CLng(FromQty), CLng(ToQty), CLng(GetQty), 0, FromDate, ToDate, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, IIf(Me.hfPlanId.Value = "", "0", Me.hfPlanId.Value)) = True Then
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
                                End If
                            Next
                        End If


                        ' If TotSuccess > 0 Then

                        DeleteExcel()
                        lblUpMsg.Text = IIf(TotFailed = 0, "Successfully uploaded", "One or more rows has invalid data. Please check the log")
                        MPEImport.Show()
                        BindBonusData()
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
                    Session.Remove("dtBonusRules")
                    Session("dtBonusRules") = dtErrors.Copy


                    Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & "SimpleBonusLog_" & Now.ToString("yyyyMMdd") + ".txt"
                    DataTable2CSV(dtErrors, fn, vbTab)
                    If dtErrors.Rows.Count > 0 Then
                        lbLog.Visible = True
                    End If
                    Session.Remove("SimpleLogInfo")
                    Session("SimpleLogInfo") = fn




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
                lblMessage.Text = "Please import valid Excel template."
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
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


            If Not Session("SimpleLogInfo") Is Nothing Then
                Dim fileValue As String = Session("SimpleLogInfo")





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
    Protected Sub ddlgetCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ddlGetCode.SelectedIndexChanged
        If Me.ddlGetCode.SelectedIndex >= 0 Then
            Me.ddlgetDesc.ClearSelection()
            Me.ddlgetDesc.Text = ""
            Me.ddlgetDesc.SelectedValue = Me.ddlGetCode.SelectedValue
            GetBonusProductId()
        Else
            Me.ddlgetDesc.ClearSelection()
            Me.ddlgetDesc.Text = ""
        End If
    End Sub

    Protected Sub ddlgetDesc_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ddlgetDesc.SelectedIndexChanged
        If Me.ddlgetDesc.SelectedIndex >= 0 Then
            Me.ddlGetCode.ClearSelection()
            Me.ddlGetCode.Text = ""
            Me.ddlGetCode.SelectedValue = Me.ddlgetDesc.SelectedValue
            GetBonusProductId()
        Else
            Me.ddlGetCode.ClearSelection()
            Me.ddlGetCode.Text = ""
        End If
    End Sub




    Protected Sub ddlOrdCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ddlOrdCode.SelectedIndexChanged
        If Me.ddlOrdCode.SelectedIndex >= 0 Then
            Me.ddlOrdDesc.ClearSelection()
            Me.ddlOrdDesc.Text = ""

            Me.ddlOrdDesc.SelectedValue = Me.ddlOrdCode.SelectedValue
            GetDefProductId()
        Else
            Me.ddlOrdDesc.ClearSelection()
            Me.ddlOrdDesc.Text = ""
        End If
    End Sub

    Protected Sub ddlOrdDesc_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ddlOrdDesc.SelectedIndexChanged
        If Me.ddlOrdDesc.SelectedIndex >= 0 Then
            Me.ddlOrdCode.ClearSelection()
            Me.ddlOrdCode.Text = ""
            Me.ddlOrdCode.SelectedValue = Me.ddlOrdDesc.SelectedValue
            GetDefProductId()
        Else
            Me.ddlOrdCode.ClearSelection()
            Me.ddlOrdCode.Text = ""
        End If
    End Sub
    Sub FillItemsList()
        Dim x As New DataTable
        x = objProduct.LoadBonusProductList(Err_No, Err_Desc, Me.hfOrgID.Value)

        ddlOrdDesc.DataTextField = "DescText"
        ddlOrdDesc.DataValueField = "DescValue"
        ddlOrdDesc.DataSource = x
        ddlOrdDesc.DataBind()
        ' ddlOrdDesc.SelectedIndex = 0


        ddlOrdCode.DataTextField = "CodeText"
        ddlOrdCode.DataValueField = "CodeValue"
        ddlOrdCode.DataSource = x
        ddlOrdCode.DataBind()
        ' ddlOrdCode.SelectedIndex = 0


        ddlgetDesc.DataTextField = "DescText"
        ddlgetDesc.DataValueField = "DescValue"
        ddlgetDesc.DataSource = x
        ddlgetDesc.DataBind()
        ' ddlgetDesc.SelectedIndex = 0


        ddlGetCode.DataTextField = "CodeText"
        ddlGetCode.DataValueField = "CodeValue"
        ddlGetCode.DataSource = x
        ddlGetCode.DataBind()
        ' ddlGetCode.SelectedIndex = 0


    End Sub
    Sub LoadOrgHeads()
        'ddl_org.DataSource = objProduct.GetOrgsHeads(Err_No, Err_Desc)
        'ddl_org.DataTextField = "Description"
        'ddl_org.DataValueField = "ORG_HE_ID"
        'ddl_org.DataBind()
        'ddl_org.Items.Insert(0, "--Select Organisation--")
        'ddl_org.Items(0).Value = 0

        Dim objCommon As New Common
        Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
        ddl_org.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
        ddl_org.Items.Clear()
        ddl_org.Items.Add("-- Select a Organization --")
        ddl_org.AppendDataBoundItems = True
        ddl_org.DataValueField = "MAS_Org_ID"
        ddl_org.DataTextField = "Description"
        ddl_org.DataBind()

    End Sub

    Private Sub dgvItems_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgvItems.RowCommand

        Try
            If (e.CommandName = "DeActivate") Then

                LoadBonusType()
                Me.txtFromQty.Text = ""
                Me.txtToQty.Text = ""
                Me.txtGetQty.Text = ""
                Me.txtAddPercent.Text = ""
                Me.txtAddPercent.Enabled = False
                ' Me.lblDUOM.Text = ""
                ' Me.lblBUOM.Text = ""
                Me.btnAddItems.Text = "Add"
                ' Me.txtBItemCode.Text = ""
                ' Me.txtBDescription.Text = ""
                Me.lblEditRow.Text = ""
                Me.StartTime.SelectedDate = Now.Date.AddDays(1)
                Me.EndTime.SelectedDate = Me.StartTime.SelectedDate.Value.AddMonths(1)
                UpdatePanel1.Update()

                Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)

                For Each r As GridViewRow In dgvItems.Rows
                    r.BackColor = Drawing.Color.Transparent
                    Dim lbl As LinkButton = DirectCast(r.FindControl("lblStatus"), LinkButton)
                    If lbl.Text = "Activate" Then
                        lbl.ForeColor = Drawing.Color.Red
                    Else
                        lbl.ForeColor = Drawing.Color.Green
                    End If
                Next

                row.BackColor = Drawing.Color.LightGoldenrodYellow
                Me.lblLineId.Text = Convert.ToInt32(dgvItems.DataKeys(row.RowIndex).Value)

                Dim lblStatus As LinkButton = DirectCast(row.FindControl("lblStatus"), LinkButton)

                Dim lblACode As Label = DirectCast(row.FindControl("lblACode"), Label)
                Dim FromQty As Long = CLng(dgvItems.Rows(row.RowIndex).Cells(4).Text)
                Dim ToQty As Long = CLng(dgvItems.Rows(row.RowIndex).Cells(5).Text)
                Dim lblValidFrom As Label = DirectCast(row.FindControl("lblValidFrom"), Label)
                Dim lblValidTo As Label = DirectCast(row.FindControl("lblValidTo"), Label)
                Dim OrgID As String = hfOrgID.Value

                Dim x As New DataTable
                x = objProduct.CheckBonusDataActiveRange(Err_No, Err_Desc, lblACode.Text, FromQty, ToQty, Me.hfOrgID.Value, DateTime.Parse(lblValidFrom.Text), DateTime.Parse(lblValidTo.Text), CInt(lblLineId.Text))
                If x.Rows.Count > 0 Then

                    'Me.lblActiveLineID.Text = ""
                    'Me.lblActiveItem.Text = ""
                    'Me.lblActiveFromQty.Text = ""
                    'Me.lblActiveToQty.Text = ""
                    'Me.lblActiveGetQty.Text = ""
                    'Me.lblActiveFromDate.Text = ""
                    'Me.lblActiveToDate.Text = ""

                    Me.dgvActive.DataSource = x
                    Me.dgvActive.DataBind()
                    'Me.lblActiveLineID.Text = x.Rows(0)("ActiveLineId").ToString()
                    'Me.lblActiveItem.Text = x.Rows(0)("ItemName").ToString()
                    'Me.lblActiveFromQty.Text = CLng(IIf(x.Rows(0)("Prom_Qty_From") Is DBNull.Value, "0", x.Rows(0)("Prom_Qty_From").ToString()))
                    'Me.lblActiveToQty.Text = CLng(IIf(x.Rows(0)("Prom_Qty_To") Is DBNull.Value, "0", x.Rows(0)("Prom_Qty_To").ToString()))
                    'Me.lblActiveGetQty.Text = CLng(IIf(x.Rows(0)("Get_Qty") Is DBNull.Value, "0", x.Rows(0)("Get_Qty").ToString()))
                    'Me.lblActiveFromDate.Text = IIf(x.Rows(0)("Valid_From") Is DBNull.Value, "", DateTime.Parse(x.Rows(0)("Valid_From").ToString()).ToString("dd-MM-yyyy"))
                    'Me.lblActiveToDate.Text = IIf(x.Rows(0)("Valid_To") Is DBNull.Value, "", DateTime.Parse(x.Rows(0)("Valid_To").ToString()).ToString("dd-MM-yyyy"))

                    Me.lblmessage1.Text = "Would you like to deactivate these and apply the new rule?"
                    lblmessage1.ForeColor = Drawing.Color.Red
                    lblinfo1.Text = "Confirmation"
                    lblMsg2.Text = "The following bonus rule(s) for this item are active and applicable within the specified date-range:"
                    btnDeactivate.Text = "Deactivate"
                    MPEAlloc.Show()
                    btnClose1.Focus()
                    Exit Sub
                Else
                    'Me.lblActiveLineID.Text = ""
                    'Me.lblActiveItem.Text = ""
                    'Me.lblActiveFromQty.Text = ""
                    'Me.lblActiveToQty.Text = ""
                    'Me.lblActiveGetQty.Text = ""
                    'Me.lblActiveFromDate.Text = ""
                    'Me.lblActiveToDate.Text = ""
                    Me.dgvActive.DataSource = Nothing
                    Me.dgvActive.DataBind()
                    btnDeactivate.Text = "Deactivate & Apply"
                End If




                If objProduct.UpdateBonusStatus(Err_No, Err_Desc, Me.lblLineId.Text, IIf(lblStatus.Text = "Deactivate", "N", "Y"), CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
                    BindBonusData()
                    UpdatePanel1.Update()
                Else
                    Me.lblMessage.Text = "Error while updating the record"
                    lblMessage.ForeColor = Drawing.Color.Red
                    lblinfo.Text = "Validation"
                    MpInfoError.Show()
                    btnClose.Focus()
                    Exit Sub
                End If




            End If

            If (e.CommandName = "EditRecord") Then



                Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)

                For Each r As GridViewRow In dgvItems.Rows
                    r.BackColor = Drawing.Color.Transparent
                    Dim lbl As LinkButton = DirectCast(r.FindControl("lblStatus"), LinkButton)
                    If lbl.Text = "Activate" Then
                        lbl.ForeColor = Drawing.Color.Red
                    Else
                        lbl.ForeColor = Drawing.Color.Green
                    End If
                Next

                row.BackColor = Drawing.Color.LightGoldenrodYellow
                Me.lblLineId.Text = Convert.ToInt32(dgvItems.DataKeys(row.RowIndex).Value)
                Me.lblEditRow.Text = row.RowIndex
                Dim lblBItem As Label = DirectCast(row.FindControl("lblBItem"), Label)
                Dim lblDItem As Label = DirectCast(row.FindControl("lblDItem"), Label)
                Dim lblACode As Label = DirectCast(row.FindControl("lblACode"), Label)
                Dim lblBCode As Label = DirectCast(row.FindControl("lblBCode"), Label)
                Dim lblADesc As Label = DirectCast(row.FindControl("lblBDesc"), Label)
                Dim lblBDesc As Label = DirectCast(row.FindControl("lblBDesc"), Label)
                Dim lblValidFrom As Label = DirectCast(row.FindControl("lblValidFrom"), Label)
                Dim lblValidTo As Label = DirectCast(row.FindControl("lblValidTo"), Label)


                Me.LblBItemId.Text = lblBItem.Text
                Me.LblDItemId.Text = lblDItem.Text
                ' Dim s() As String = dgvItems.Rows(row.RowIndex).Cells(1).Text.Split("-")
                Me.lblDItemCode.Text = lblACode.Text
                Me.lblDDescription.Text = lblADesc.Text

                ' Dim s1() As String = dgvItems.Rows(row.RowIndex).Cells(2).Text.Split("-")
                Me.lblBItemCode.Text = lblBCode.Text
                Me.lblBDescription.Text = lblBDesc.Text

                'Me.txtItemCode.Text = Me.lblDItemCode.Text
                'Me.txtDescription.Text = Me.lblDDescription.Text
                'Me.txtBItemCode.Text = Me.lblBItemCode.Text
                'Me.txtBDescription.Text = Me.lblBDescription.Text

                Me.ddlOrdCode.SelectedValue = Me.lblDItemCode.Text
                Me.ddlOrdDesc.SelectedValue = Me.lblDItemCode.Text

                Me.ddlgetDesc.ClearSelection()
                Me.ddlgetDesc.Text = ""

                Me.ddlGetCode.ClearSelection()
                Me.ddlgetDesc.Text = ""


                Me.ddlGetCode.SelectedValue = Me.lblBItemCode.Text
                Me.ddlgetDesc.SelectedValue = Me.lblBItemCode.Text

                Me.txtFromQty.Text = CLng(dgvItems.Rows(row.RowIndex).Cells(4).Text)
                Me.txtToQty.Text = CLng(dgvItems.Rows(row.RowIndex).Cells(5).Text)
                Me.ddlType.SelectedValue = dgvItems.Rows(row.RowIndex).Cells(6).Text
                Me.txtGetQty.Text = CLng(dgvItems.Rows(row.RowIndex).Cells(7).Text)
                ' Me.txtAddPercent.Text = IIf(dgvItems.Rows(row.RowIndex).Cells(8).Text Is Nothing, "", CLng(CDec(dgvItems.Rows(row.RowIndex).Cells(8).Text)))
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

                Me.lblF.Text = Me.txtFromQty.Text
                Me.lblT.Text = Me.txtToQty.Text
                Me.lblVF.Text = Me.StartTime.SelectedDate.Value.ToString("dd-MM-yyyy")
                Me.lblVT.Text = Me.EndTime.SelectedDate.Value.ToString("dd-MM-yyyy")

                If dgvItems.Rows(row.RowIndex).Cells(6).Text.ToUpper() = "RECURRING" Then
                    Me.txtAddPercent.Enabled = True
                Else
                    Me.txtAddPercent.Enabled = False
                End If



                UpdatePanel1.Update()
                Me.btnAddItems.Text = "Update"
                'Me.txtItemCode.Enabled = False
                'Me.txtDescription.Enabled = False
                Me.ddlOrdCode.Enabled = False
                Me.ddlOrdDesc.Enabled = False
            End If


            If (e.CommandName = "DeleteRecord") Then



                Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
                Me.lblLineId.Text = Convert.ToInt32(dgvItems.DataKeys(row.RowIndex).Value)
                objProduct.DeleteBonusData(Err_No, Err_Desc, Me.lblLineId.Text)
                'Me.ddlType.SelectedItem.Text = "POINT"
                Me.ddlType.SelectedIndex = 0
                Me.txtFromQty.Text = ""
                Me.txtToQty.Text = ""
                Me.txtGetQty.Text = ""
                Me.txtAddPercent.Text = ""
                Me.btnAddItems.Text = "Add"
                'Me.txtItemCode.Enabled = True
                'Me.txtDescription.Enabled = True
                Me.ddlOrdCode.Enabled = True
                Me.ddlOrdDesc.Enabled = True
                UpdatePanel1.Update()
                BindBonusData()
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("information.aspx?mode=1&errno=" & "74071" & "&msg=" & AppMsgHandler.GetErrorMessage("SWX_BO_Msg_005") & "&next=BonusDefinition.aspx", False)
        Finally
        End Try
    End Sub

    Private Sub dgv_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgvItems.Sorting
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"

        BindBonusData()
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
        BindBonusData()
    End Sub




    Private Sub GetProductDetails(ByVal ItemCode As String, ByVal Description As String, ByVal Type As String)
        Dim r As DataRow = Nothing
        r = objProduct.GetBonusProductDetails(Err_No, Err_Desc, ItemCode, Description, Me.hfOrgID.Value)
        If Not r Is Nothing Then
            'Me.txtBItemCode.Enabled = True
            'Me.txtBDescription.Enabled = True
            Me.ddlGetCode.Enabled = True
            Me.ddlgetDesc.Enabled = True
            If Type = "Default" Then
                Me.lblDUOM.Text = ""
                Me.LblDItemId.Text = ""
                Me.lblDItemCode.Text = ""
                Me.lblDDescription.Text = ""

                Me.lblDUOM.Text = r("UOM").ToString()
                Me.lblDItemCode.Text = r("itemCode").ToString()
                Me.lblDDescription.Text = r("Description").ToString()
                Me.LblDItemId.Text = r("ItemId").ToString()
                Me.ddlOrdCode.SelectedValue = r("itemCode").ToString()
                Me.ddlOrdDesc.SelectedItem.Text = r("Description").ToString()
                Me.lblOrgId.Text = r("OrgId").ToString()


                Me.lblBUOM.Text = r("UOM").ToString()
                Me.lblBItemCode.Text = r("itemCode").ToString()
                Me.lblBDescription.Text = r("Description").ToString()
                Me.LblBItemId.Text = r("ItemId").ToString()

                Me.ddlgetDesc.ClearSelection()
                Me.ddlgetDesc.Text = ""
                Me.ddlGetCode.ClearSelection()
                Me.ddlGetCode.Text = ""

                Me.ddlGetCode.SelectedValue = r("itemCode").ToString()
                Me.ddlgetDesc.SelectedValue = r("itemCode").ToString()
            End If

            If Type = "Bonus" Then
                Me.lblBUOM.Text = r("UOM").ToString()
                Me.lblBItemCode.Text = r("itemCode").ToString()
                Me.lblBDescription.Text = r("Description").ToString()
                Me.LblBItemId.Text = r("ItemId").ToString()
                Me.ddlGetCode.SelectedValue = r("itemCode").ToString()
                Me.ddlgetDesc.SelectedValue = r("itemCode").ToString()
            End If
            BindBonusData()
        Else

            ResetDetails()
        End If
    End Sub


    Private Sub BindBonusData()
        'Dim s() As String = Me.ddlOrdCode.SelectedValue.Split("$")
        Dim ItemCode As String = Me.ddlOrdCode.SelectedValue
        'If Not s Is Nothing Then
        '    If s.Length = 2 Then
        '        ItemCode = s(0).ToString().Trim()
        '    End If
        'End If
        Dim dtData As New DataTable
        dtData = objProduct.GetBonusData(Err_No, Err_Desc, ItemCode, Me.hfOrgID.Value, IIf(chShow.Checked = True, "Y", "N"), IIf(Me.hfPlanId.Value = "", "0", Me.hfPlanId.Value))
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
                Dim fdate As String = Me.StartTime.SelectedDate.Value.ToString("dd-MM-yyyy")
                Dim tdate As String = Me.EndTime.SelectedDate.Value.ToString("dd-MM-yyyy")

                ' Dim s() As String = Me.ddlOrdCode.SelectedValue.Split("$")

                '  Dim UOM As String = s(1).ToString().Trim()
                Dim ItemCode As String = Me.ddlOrdCode.SelectedValue


                'Check Item exist
                If btnAddItems.Text = "Add" Or (btnAddItems.Text = "Update" And (Me.txtFromQty.Text <> Me.lblF.Text Or Me.txtToQty.Text <> Me.lblT.Text Or fdate <> Me.lblVF.Text Or tdate <> Me.lblVT.Text)) Then
                    'If objProduct.CheckBonusData(Err_No, Err_Desc, Me.txtItemCode.Text, CLng(IIf(Me.txtFromQty.Text = "", "0", Me.txtFromQty.Text)), CLng(IIf(Me.txtToQty.Text = "", "0", Me.txtToQty.Text)), Me.hfOrgID.Value, Me.StartTime.SelectedDate.Value, Me.EndTime.SelectedDate.Value) = True Then
                    '    Me.lblMessage.Text = "Bonus already defined with this range."
                    '    lblMessage.ForeColor = Drawing.Color.Red
                    '    lblinfo.Text = "Validation"
                    '    MpInfoError.Show()
                    '    btnClose.Focus()
                    '    Exit Sub
                    'End If


                    Dim x As New DataTable
                    x = objProduct.CheckBonusDataActiveRange(Err_No, Err_Desc, ItemCode, CLng(IIf(Me.txtFromQty.Text = "", "0", Me.txtFromQty.Text)), CLng(IIf(Me.txtToQty.Text = "", "0", Me.txtToQty.Text)), Me.hfOrgID.Value, Me.StartTime.SelectedDate.Value, Me.EndTime.SelectedDate.Value, CInt(IIf(Me.lblLineId.Text = "", "0", Me.lblLineId.Text)))
                    If x.Rows.Count > 0 Then
                        Me.dgvActive.DataSource = x
                        Me.dgvActive.DataBind()
                        'Me.lblActiveLineID.Text = ""
                        'Me.lblActiveItem.Text = ""
                        'Me.lblActiveFromQty.Text = ""
                        'Me.lblActiveToQty.Text = ""
                        'Me.lblActiveGetQty.Text = ""
                        'Me.lblActiveFromDate.Text = ""
                        'Me.lblActiveToDate.Text = ""

                        'Me.lblActiveLineID.Text = x.Rows(0)("ActiveLineId").ToString()
                        'Me.lblActiveItem.Text = x.Rows(0)("ItemName").ToString()
                        'Me.lblActiveFromQty.Text = CLng(IIf(x.Rows(0)("Prom_Qty_From") Is DBNull.Value, "0", x.Rows(0)("Prom_Qty_From").ToString()))
                        'Me.lblActiveToQty.Text = CLng(IIf(x.Rows(0)("Prom_Qty_To") Is DBNull.Value, "0", x.Rows(0)("Prom_Qty_To").ToString()))
                        'Me.lblActiveGetQty.Text = CLng(IIf(x.Rows(0)("Get_Qty") Is DBNull.Value, "0", x.Rows(0)("Get_Qty").ToString()))
                        'Me.lblActiveFromDate.Text = IIf(x.Rows(0)("Valid_From") Is DBNull.Value, "", DateTime.Parse(x.Rows(0)("Valid_From").ToString()).ToString("dd-MM-yyyy"))
                        'Me.lblActiveToDate.Text = IIf(x.Rows(0)("Valid_To") Is DBNull.Value, "", DateTime.Parse(x.Rows(0)("Valid_To").ToString()).ToString("dd-MM-yyyy"))

                        Me.lblmessage1.Text = "Would you like to deactivate these and apply the new rule?"
                        lblmessage1.ForeColor = Drawing.Color.Red
                        lblinfo1.Text = "Confirmation"
                        lblMsg2.Text = "The following bonus rule(s) for this item are active and applicable within the specified date-range:"
                        btnDeactivate.Text = "Deactivate & Apply"
                        MPEAlloc.Show()
                        btnClose1.Focus()
                        Exit Sub
                    Else
                        'Me.lblActiveLineID.Text = ""
                        'Me.lblActiveItem.Text = ""
                        'Me.lblActiveFromQty.Text = ""
                        'Me.lblActiveToQty.Text = ""
                        'Me.lblActiveGetQty.Text = ""
                        'Me.lblActiveFromDate.Text = ""
                        'Me.lblActiveToDate.Text = ""
                        Me.dgvActive.DataSource = Nothing
                        Me.dgvActive.DataBind()
                        btnDeactivate.Text = "Deactivate & Apply"
                    End If
                End If

                If Me.btnAddItems.Text = "Add" Then
                    success = objProduct.SaveBonusData(Err_No, Err_Desc, ItemCode, Me.hfOrgID.Value, Me.lblDUOM.Text, Me.lblBItemCode.Text, Me.lblBUOM.Text, Me.ddlType.SelectedItem.Text, CLng(IIf(Me.txtFromQty.Text = "", "0", Me.txtFromQty.Text)), CLng(IIf(Me.txtToQty.Text = "", "0", Me.txtToQty.Text)), CLng(IIf(Me.txtGetQty.Text = "", "0", Me.txtGetQty.Text)), CDec(IIf(Me.txtAddPercent.Text = "", 0, Me.txtAddPercent.Text)), Me.StartTime.SelectedDate.Value, Me.EndTime.SelectedDate.Value, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, IIf(hfPlanId.Value = "", "0", Me.hfPlanId.Value))

                ElseIf Me.btnAddItems.Text = "Update" Then
                    success = objProduct.UpdateBonusData(Err_No, Err_Desc, Me.lblLineId.Text, Me.lblBItemCode.Text, Me.lblBUOM.Text, Me.ddlType.SelectedItem.Text, CLng(IIf(Me.txtFromQty.Text = "", "0", Me.txtFromQty.Text)), CLng(IIf(Me.txtToQty.Text = "", "0", Me.txtToQty.Text)), CLng(IIf(Me.txtGetQty.Text = "", "0", Me.txtGetQty.Text)), CDec(IIf(Me.txtAddPercent.Text = "", 0, Me.txtAddPercent.Text)), Me.StartTime.SelectedDate.Value, Me.EndTime.SelectedDate.Value, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, IIf(hfPlanId.Value = "", "0", Me.hfPlanId.Value))
                End If
                If success = True Then
                    Me.lblMessage.Text = "Successfully saved."
                    lblMessage.ForeColor = Drawing.Color.Green
                    lblinfo.Text = "Information"
                    MpInfoError.Show()
                    btnClose.Focus()
                    Me.btnAddItems.Text = "Add"
                    Me.txtFromQty.Text = ""
                    Me.txtToQty.Text = ""
                    Me.txtGetQty.Text = ""
                    Me.txtAddPercent.Text = ""
                    Me.StartTime.Enabled = True
                    Me.EndTime.Enabled = True
                    Me.ddlOrdCode.Enabled = True
                    Me.ddlOrdDesc.Enabled = True
                    'Me.ddlType.SelectedValue = "POINT"
                    LoadBonusType()
                    BindBonusData()
                Else
                    Me.lblMessage.Text = "Error while saving bonus details"
                    lblMessage.ForeColor = Drawing.Color.Red
                    lblinfo.Text = "Validation"
                    MpInfoError.Show()
                    btnClose.Focus()
                    Exit Sub
                End If

            Catch ex As Exception
                log.Error(ex.Message)
                Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_OrderEntry_006") & "&next=BonusDefinition.aspx&Title=Bonus Definition", False)
            End Try
        End If
    End Sub

    Protected Sub ResetBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClear.Click
        ResetDetails()
        BindBonusData()
    End Sub

    Public Sub LoadBonusType()
        Dim dtBType As New DataTable
        dtBType.Columns.Add("Code")
        dtBType.Columns.Add("Description")
        Dim dr As DataRow = dtBType.NewRow()
        dr("Code") = "POINT"
        dr("Description") = "POINT"
        dtBType.Rows.InsertAt(dr, 0)

        dr = dtBType.NewRow()
        dr("Code") = "RECURRING"
        dr("Description") = "RECURRING"
        dtBType.Rows.Add(dr)


        'dr = dtBType.NewRow()
        'dr("Code") = "PERCENT"
        'dr("Description") = "PERCENT"
        'dtBType.Rows.Add(dr)




        Me.ddlType.DataSource = dtBType
        Me.ddlType.DataTextField = "Description"
        Me.ddlType.DataValueField = "Code"
        Me.ddlType.DataBind()
        Me.ddlType.SelectedIndex = 0






    End Sub

    Private Sub ResetDetails()
        Me.LblDItemId.Text = ""
        Me.lblDItemCode.Text = ""
        Me.lblDDescription.Text = ""
        Me.LblDItemId.Text = ""
        ' Me.ddlType.SelectedValue = "POINT"
        LoadBonusType()
        Me.txtFromQty.Text = ""
        Me.txtToQty.Text = ""
        Me.txtGetQty.Text = ""
        Me.txtAddPercent.Text = ""
        Me.txtAddPercent.Enabled = False
        Me.lblDUOM.Text = ""
        Me.lblBUOM.Text = ""
        Me.btnAddItems.Text = "Add"
        Me.ddlOrdDesc.ClearSelection()
        Me.ddlOrdDesc.Text = ""

        Me.ddlOrdCode.ClearSelection()
        Me.ddlOrdDesc.Text = ""

        Me.ddlgetDesc.ClearSelection()
        Me.ddlgetDesc.Text = ""

        Me.ddlGetCode.ClearSelection()
        Me.ddlgetDesc.Text = ""

        Me.lblEditRow.Text = ""
        Me.StartTime.SelectedDate = Now.Date.AddDays(1)
        Me.EndTime.SelectedDate = Me.StartTime.SelectedDate.Value.AddMonths(1)
        Me.StartTime.Enabled = True
        Me.EndTime.Enabled = True
        'Me.txtItemCode.Enabled = True
        'Me.txtDescription.Enabled = True
        'Me.hdnValue3.Value = ""
        'Me.hdnValue4.Value = ""
        'Me.hdnValue1.Value = ""
        'Me.hdnValue2.Value = ""
        Me.lblOrgId.Text = ""
        Me.lblLineId.Text = ""
        Me.lblF.Text = ""
        Me.lblT.Text = ""
        Me.lblVF.Text = ""
        Me.lblVT.Text = ""

        'Me.lblActiveLineID.Text = ""
        'Me.lblActiveItem.Text = ""
        'Me.lblActiveFromQty.Text = ""
        'Me.lblActiveToQty.Text = ""
        'Me.lblActiveGetQty.Text = ""
        'Me.lblActiveFromDate.Text = ""
        'Me.lblActiveToDate.Text = ""
        Me.dgvActive.DataSource = Nothing
        Me.dgvActive.DataBind()
        btnDeactivate.Text = "Deactivate & Apply"
        chShow.Checked = False

        Me.dgvItems.DataSource = Nothing
        Me.dgvItems.DataBind()
        PnlGridData.Visible = False
        ' Me.ddOraganisation.Enabled = True
        If Not Me.hfOrgID.Value Is Nothing Then
            'Me.txtItemCode.Enabled = True
            'Me.txtDescription.Enabled = True
            Me.ddlOrdDesc.Enabled = True
            Me.ddlOrdCode.Enabled = True
        End If
    End Sub
    Private Sub RefreshGrid()
        For Each r As GridViewRow In dgvItems.Rows
            Dim lbl As LinkButton = DirectCast(r.FindControl("lblStatus"), LinkButton)
            If lbl.Text = "Activate" Then
                lbl.ForeColor = Drawing.Color.Red
            Else
                lbl.ForeColor = Drawing.Color.Green
            End If
        Next
    End Sub
    Private Function ValidationDetails() As Boolean
        Dim sucess As Boolean = False



        If Me.hfOrgID.Value Is Nothing And btnAddItems.Text = "Add" Then
            Me.lblMessage.Text = "Please select a organization"
            lblMessage.ForeColor = Drawing.Color.Red
            lblinfo.Text = "Information"
            MpInfoError.Show()
            btnClose.Focus()
            sucess = True
            RefreshGrid()
            Return sucess
            Exit Function
        End If






        If Me.LblDItemId.Text = "" Or Me.LblBItemId.Text = "" Then
            Me.lblMessage.Text = "Please enter a valid item code/description"
            lblMessage.ForeColor = Drawing.Color.Red
            lblinfo.Text = "Validation"
            MpInfoError.Show()
            btnClose.Focus()
            sucess = True
            RefreshGrid()
            Return sucess
            Exit Function
        End If


        If Me.txtFromQty.Text = "" Or CDec(IIf(Me.txtFromQty.Text = "", "0", Me.txtFromQty.Text)) = 0 Or Me.txtFromQty.Text = "" Or CDec(IIf(Me.txtToQty.Text = "", "0", Me.txtToQty.Text)) = 0 Or Me.txtGetQty.Text = "" Or CDec(IIf(Me.txtGetQty.Text = "", "0", Me.txtGetQty.Text)) = 0 Then
            Me.lblMessage.Text = "Please enter a valid from,to and get quantity"
            lblMessage.ForeColor = Drawing.Color.Red
            lblinfo.Text = "Validation"
            MpInfoError.Show()
            btnClose.Focus()
            sucess = True
            RefreshGrid()
            Return sucess
            Exit Function
        End If


        If Me.txtFromQty.Text <> "" And Me.txtFromQty.Text <> "0" And Me.txtToQty.Text <> "" And Me.txtToQty.Text <> "0" Then
            If CDec(Me.txtToQty.Text) <= CDec(Me.txtFromQty.Text) Then
                Me.lblMessage.Text = "To quantity should be greater than from quantity"
                lblMessage.ForeColor = Drawing.Color.Red
                lblinfo.Text = "Validation"
                MpInfoError.Show()
                btnClose.Focus()
                sucess = True
                RefreshGrid()
                Return sucess
                Exit Function
            End If
        End If


        If Me.ddlType.SelectedItem.Text = "RECURRING" And Not Me.txtAddPercent.Text Is Nothing Then
            If CDec(IIf(Me.txtAddPercent.Text = "", "0", Me.txtAddPercent.Text)) > 100 Then
                Me.lblMessage.Text = "Additional percentage should be less than or equal to 100"
                lblMessage.ForeColor = Drawing.Color.Red
                lblinfo.Text = "Validation"
                MpInfoError.Show()
                btnClose.Focus()
                sucess = True
                RefreshGrid()
                Return sucess
                Exit Function
            End If
        End If
        If (Me.StartTime.SelectedDate.Value <= Now.Date() And Me.StartTime.Enabled = True) Or Me.EndTime.SelectedDate.Value <= Now.Date() Then
            Me.lblMessage.Text = "Valid from and to date should be greater than current date"
            lblMessage.ForeColor = Drawing.Color.Red
            lblinfo.Text = "Validation"
            MpInfoError.Show()
            btnClose.Focus()
            sucess = True
            RefreshGrid()
            Return sucess
            Exit Function
        End If
        Return sucess
    End Function





    Private Sub GetDefProductId()
        If (Me.ddlOrdCode.SelectedIndex >= 0 Or Me.ddlOrdDesc.SelectedIndex >= 0) And Me.btnAddItems.Text = "Add" Then
            Me.LblDItemId.Text = ""
            Me.lblDItemCode.Text = ""
            Me.lblDDescription.Text = ""
            Me.lblDUOM.Text = ""
            ' Me.ddlType.SelectedItem.Text = "POINT"
            Me.ddlType.SelectedIndex = 0
            Me.txtFromQty.Text = ""
            Me.txtToQty.Text = ""
            Me.txtGetQty.Text = ""
            Me.txtAddPercent.Text = ""
            Me.btnAddItems.Text = "Add"
            GetProductDetails(Me.ddlOrdCode.SelectedValue, Me.ddlOrdDesc.SelectedItem.Text, "Default")
        ElseIf (Me.ddlOrdCode.SelectedIndex < 0 Or Me.ddlOrdDesc.SelectedIndex < 0) And Me.btnAddItems.Text = "Add" Then
            Me.LblDItemId.Text = ""
            Me.lblDItemCode.Text = ""
            Me.lblDDescription.Text = ""
            Me.btnAddItems.Text = "Add"
            Me.ddlOrdDesc.ClearSelection()
            Me.ddlOrdDesc.Text = ""
            Me.ddlOrdCode.ClearSelection()
            Me.ddlOrdCode.Text = ""
            Me.lblDUOM.Text = ""
            'Me.ddlType.SelectedItem.Text = "POINT"
            Me.ddlType.SelectedIndex = 0
            Me.txtFromQty.Text = ""
            Me.txtToQty.Text = ""
            Me.txtGetQty.Text = ""
            Me.txtAddPercent.Text = ""
        End If
    End Sub

    Private Sub GetBonusProductId()
        If (Me.ddlGetCode.SelectedIndex >= 0 Or Me.ddlgetDesc.SelectedIndex >= 0) And Me.btnAddItems.Text = "Add" Then
            Me.LblBItemId.Text = ""
            Me.lblBItemCode.Text = ""
            Me.lblBDescription.Text = ""
            Me.lblBUOM.Text = ""
            ' Me.ddlType.SelectedItem.Text = "POINT"
            Me.ddlType.SelectedIndex = 0
            Me.txtFromQty.Text = ""
            Me.txtToQty.Text = ""
            Me.txtGetQty.Text = ""
            Me.txtAddPercent.Text = ""
            Me.btnAddItems.Text = "Add"
            GetProductDetails(Me.ddlGetCode.SelectedValue, Me.ddlgetDesc.SelectedItem.Text, "Bonus")
        ElseIf (Me.ddlGetCode.SelectedIndex >= 0 Or Me.ddlgetDesc.SelectedIndex >= 0) And Me.btnAddItems.Text = "Update" Then
            GetProductDetails(Me.ddlGetCode.SelectedValue, Me.ddlgetDesc.SelectedItem.Text, "Bonus")
        ElseIf (Me.ddlGetCode.SelectedIndex < 0 Or Me.ddlgetDesc.SelectedIndex < 0) And Me.btnAddItems.Text = "Add" Then
            Me.LblBItemId.Text = ""
            Me.lblBItemCode.Text = ""
            Me.lblBDescription.Text = ""
            Me.btnAddItems.Text = "Add"
            Me.ddlgetDesc.ClearSelection()
            Me.ddlgetDesc.Text = ""
            Me.ddlGetCode.ClearSelection()
            Me.ddlGetCode.Text = ""
            Me.lblDUOM.Text = ""
            'Me.ddlType.SelectedItem.Text = "POINT"
            Me.ddlType.SelectedIndex = 0
            Me.txtFromQty.Text = ""
            Me.txtToQty.Text = ""
            Me.txtGetQty.Text = ""
            Me.txtAddPercent.Text = ""

        End If
    End Sub










    Private Sub ddlType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlType.SelectedIndexChanged
        If Me.ddlType.SelectedIndex >= 0 Then
            If Me.ddlType.SelectedItem.Text = "RECURRING" Then
                Me.txtAddPercent.Text = ""
                Me.txtAddPercent.Enabled = True
            Else
                Me.txtAddPercent.Text = ""
                Me.txtAddPercent.Enabled = False
            End If
        Else

        End If
    End Sub

    Protected Sub btnDeactivate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeactivate.Click
        Dim ActiveLineID As String = Nothing

        For Each gvr As GridViewRow In dgvActive.Rows
            Dim lblALineID As Label = DirectCast(gvr.FindControl("lblActiveLineID"), Label)
            ActiveLineID = ActiveLineID + lblALineID.Text + ","
        Next
        If btnDeactivate.Text = "Deactivate" Then

            If objProduct.UpdateBonusStatus(Err_No, Err_Desc, ActiveLineID, "N", CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
                If objProduct.UpdateBonusStatus(Err_No, Err_Desc, Me.lblLineId.Text, "Y", CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
                    BindBonusData()
                    'Me.lblActiveLineID.Text = ""
                    'Me.lblActiveItem.Text = ""
                    'Me.lblActiveFromQty.Text = ""
                    'Me.lblActiveToQty.Text = ""
                    'Me.lblActiveGetQty.Text = ""
                    'Me.lblActiveFromDate.Text = ""
                    'Me.lblActiveToDate.Text = ""
                    Me.StartTime.Enabled = True
                    Me.EndTime.Enabled = True
                    Me.dgvActive.DataSource = Nothing
                    Me.dgvActive.DataBind()
                    btnDeactivate.Text = "Deactivate & Apply"
                    UpdatePanel1.Update()
                    MPEAlloc.Hide()
                    Exit Sub
                Else
                    Me.lblMessage.Text = "Error while updating the record"
                    lblMessage.ForeColor = Drawing.Color.Red
                    lblinfo.Text = "Validation"
                    MpInfoError.Show()
                    btnClose.Focus()
                    Exit Sub
                End If
            End If

        End If

        If btnDeactivate.Text = "Deactivate & Apply" Then
            If objProduct.UpdateBonusStatus(Err_No, Err_Desc, ActiveLineID, "N", CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
                Dim success As Boolean
                If Me.btnAddItems.Text = "Add" Then
                    success = objProduct.SaveBonusData(Err_No, Err_Desc, Me.ddlOrdCode.SelectedValue, Me.hfOrgID.Value, Me.lblDUOM.Text, Me.lblBItemCode.Text, Me.lblBUOM.Text, Me.ddlType.SelectedItem.Text, CLng(IIf(Me.txtFromQty.Text = "", "0", Me.txtFromQty.Text)), CLng(IIf(Me.txtToQty.Text = "", "0", Me.txtToQty.Text)), CLng(IIf(Me.txtGetQty.Text = "", "0", Me.txtGetQty.Text)), CDec(IIf(Me.txtAddPercent.Text = "", 0, Me.txtAddPercent.Text)), Me.StartTime.SelectedDate.Value, Me.EndTime.SelectedDate.Value, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, IIf(hfPlanId.Value = "", "0", Me.hfPlanId.Value))

                ElseIf Me.btnAddItems.Text = "Update" Then
                    success = objProduct.UpdateBonusData(Err_No, Err_Desc, Me.lblLineId.Text, Me.lblBItemCode.Text, Me.lblBUOM.Text, Me.ddlType.SelectedItem.Text, CLng(IIf(Me.txtFromQty.Text = "", "0", Me.txtFromQty.Text)), CLng(IIf(Me.txtToQty.Text = "", "0", Me.txtToQty.Text)), CLng(IIf(Me.txtGetQty.Text = "", "0", Me.txtGetQty.Text)), CDec(IIf(Me.txtAddPercent.Text = "", 0, Me.txtAddPercent.Text)), Me.StartTime.SelectedDate.Value, Me.EndTime.SelectedDate.Value, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, IIf(hfPlanId.Value = "", "0", Me.hfPlanId.Value))
                End If
                If success = True Then
                    Me.lblMessage.Text = "Successfully saved."
                    lblMessage.ForeColor = Drawing.Color.Green
                    lblinfo.Text = "Information"
                    MpInfoError.Show()
                    btnClose.Focus()
                    Me.btnAddItems.Text = "Add"
                    Me.txtFromQty.Text = ""
                    Me.txtToQty.Text = ""
                    Me.txtGetQty.Text = ""
                    Me.txtAddPercent.Text = ""
                    Me.StartTime.Enabled = True
                    Me.EndTime.Enabled = True
                    'Me.ddlType.SelectedValue = "POINT"

                    'Me.lblActiveLineID.Text = ""
                    'Me.lblActiveItem.Text = ""
                    'Me.lblActiveFromQty.Text = ""
                    'Me.lblActiveToQty.Text = ""
                    'Me.lblActiveGetQty.Text = ""
                    'Me.lblActiveFromDate.Text = ""
                    'Me.lblActiveToDate.Text = ""
                    Me.dgvActive.DataSource = Nothing
                    Me.dgvActive.DataBind()
                    btnDeactivate.Text = "Deactivate & Apply"
                    MPEAlloc.Hide()

                    LoadBonusType()
                    BindBonusData()
                    Me.UpdatePanel1.Update()
                    Exit Sub
                Else
                    Me.lblMessage.Text = "Error while saving bonus details"
                    lblMessage.ForeColor = Drawing.Color.Red
                    lblinfo.Text = "Validation"
                    MpInfoError.Show()
                    btnClose.Focus()
                    Exit Sub
                End If
            End If

        End If
    End Sub

    Protected Sub chShow_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chShow.CheckedChanged
        LoadBonusType()
        Me.txtFromQty.Text = ""
        Me.txtToQty.Text = ""
        Me.txtGetQty.Text = ""
        Me.txtAddPercent.Text = ""
        Me.txtAddPercent.Enabled = False
        '  Me.lblDUOM.Text = ""
        '  Me.lblBUOM.Text = ""
        Me.btnAddItems.Text = "Add"
        ' Me.txtBItemCode.Text = ""
        ' Me.txtBDescription.Text = ""
        Me.lblEditRow.Text = ""
        Me.StartTime.SelectedDate = Now.Date.AddDays(1)
        Me.EndTime.SelectedDate = Me.StartTime.SelectedDate.Value.AddMonths(1)
        Me.StartTime.Enabled = True
        Me.EndTime.Enabled = True
        UpdatePanel1.Update()
        BindBonusData()
    End Sub

    Protected Sub btnClose1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose1.Click
        MPEAlloc.Hide()
        For Each r As GridViewRow In dgvItems.Rows
            Dim lbl As LinkButton = DirectCast(r.FindControl("lblStatus"), LinkButton)
            If lbl.Text = "Activate" Then
                lbl.ForeColor = Drawing.Color.Red
            Else
                lbl.ForeColor = Drawing.Color.Green
            End If
        Next
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("AdminBonusSimple.aspx?OID=", False)
    End Sub
End Class





