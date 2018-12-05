
Imports SalesWorx.BO.Common
Imports Telerik.Web.UI
Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports ExcelLibrary.SpreadSheet
Partial Public Class AdminOrderlvlCustomerFOC
    Inherits System.Web.UI.Page
    Dim objProduct As New Product
    Dim objCustomer As New Customer
    Dim ObjCommon As New Common
    Dim objLogin As New SalesWorx.BO.Common.Login

    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const PageID As String = "P309"
    Private dt As New DataTable
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objControl As New AppControl
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
                LoadBonusType()
                ResetDetails()
                FillICustomerList()
                BindDiscountData()

                ViewState("FileType") = Nothing
                ViewState("FileName") = Nothing
                ViewState("CSVName") = Nothing
            End If
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_005") & "&next=Welcome.aspx&Title=Discount Definition", False)
        End Try
    End Sub
    Sub LoadOrgHeads()
        Dim objCommon As New Common
        Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
        ddl_org.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
        ddl_org.Items.Clear()
        ddl_org.Items.Add(New RadComboBoxItem("-- Select a Organization --", "0"))
        ddl_org.AppendDataBoundItems = True
        ddl_org.DataValueField = "MAS_Org_ID"
        ddl_org.DataTextField = "Description"
        ddl_org.DataBind()

    End Sub
    Private Sub ResetDetails()
        Me.txtDisValue.Text = ""
        ddlCustomer.Text = ""
        Me.ddlType.SelectedIndex = 0
        Me.txtvalue.Text = ""
        Me.btnAddItems.Text = "Add"
        Me.lblUpMsg.Text = ""
        Me.lblLineID.Text = ""
        ddlCustomer.ClearSelection()
        ddl_transactiontype.SelectedIndex = 0
    End Sub
    Sub ResetFOCDetails()
        ddlItems.ClearSelection()
        ddlItems.Text = ""
        btnAddFOCItems.Enabled = True
        lbl_FOCItemMsg.Text = ""
    End Sub
    Public Sub LoadBonusType()
        Dim dtBType As New DataTable
        dtBType.Columns.Add("Code")
        dtBType.Columns.Add("Description")
        Dim dr As DataRow = dtBType.NewRow()
        dr("Code") = "V"
        dr("Description") = "VALUE"
        dtBType.Rows.InsertAt(dr, 0)

        dr = dtBType.NewRow()
        dr("Code") = "P"
        dr("Description") = "PERCENTAGE"
        dtBType.Rows.Add(dr)




        Me.ddlType.DataSource = dtBType
        Me.ddlType.DataTextField = "Description"
        Me.ddlType.DataValueField = "Code"
        Me.ddlType.DataBind()
        Me.ddlType.SelectedIndex = 0

    End Sub
    Sub FillItemList()
        Dim x As New DataTable
        x = objProduct.GetProductListByOrgID(Err_No, Err_Desc, ddl_org.SelectedValue)

        Me.ddlItems.ClearSelection()
        Me.ddlItems.Items.Clear()
        Me.ddlItems.Text = ""

        ddlItems.DataTextField = "Description"
        ddlItems.DataValueField = "ItemID"
        ddlItems.DataSource = x
        ddlItems.DataBind()
    End Sub
    Sub FillICustomerList()
        Dim x As New DataTable
        x = ObjCommon.GetCustomerfromOrgForOrderLvlFOC(Err_No, Err_Desc, ddl_org.SelectedValue)

        Me.ddlCustomer.ClearSelection()
        Me.ddlCustomer.Items.Clear()
        Me.ddlCustomer.Text = ""

        ddlCustomer.DataTextField = "Customer"
        ddlCustomer.DataValueField = "CustomerID"
        ddlCustomer.DataSource = x
        ddlCustomer.DataBind()

    End Sub
    Sub BindFOCData()
        Dim x As New DataTable
        x = objProduct.GetFOCDefinition(Err_No, Err_Desc, ddl_org.SelectedItem.Value)

        Dim dv As New DataView(x)
        dv = x.DefaultView
        If ViewState("SortFieldFOC") <> "" Then
            dv.Sort = (ViewState("SortFieldFOC") & " ") + SortDirection
        End If
        Me.dgvItemsFOC.DataSource = dv
        Me.dgvItemsFOC.DataBind()
    End Sub
    Private Sub BindDiscountData()

        Dim x As New DataTable
        x = objCustomer.GetDiscountDefinition(Err_No, Err_Desc, ddl_org.SelectedItem.Value)

        Dim dv As New DataView(x)
        dv = x.DefaultView
        If ViewState("SortField") <> "" Then
            dv.Sort = (ViewState("SortField") & " ") + SortDirection
        End If

        Me.dgvItems.DataSource = dv
        Me.dgvItems.DataBind()

    End Sub
    Private Sub ddl_org_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_org.SelectedIndexChanged
        FillICustomerList()
        FillItemList()
        BindDiscountData()
        BindFOCData()
        ResetDetails()
        ResetFOCDetails()
    End Sub
    Sub LoadDiscountData()
        Dim id() As String
        If Not ddlCustomer.SelectedItem Is Nothing Then
            If ddlCustomer.SelectedItem.Text <> "" Then
                id = ddlCustomer.SelectedItem.Value.Split("$")
                Dim dt As New DataTable
                dt = objCustomer.GetCustomerDiscountDefinition(Err_No, Err_Desc, id(0), id(1))
                If dt.Rows.Count > 0 Then
                    txtDisValue.Text = dt.Rows(0)("Attrib_Value").ToString
                    txtvalue.Text = dt.Rows(0)("Custom_Attribute_4").ToString
                    If Not ddlType.Items.FindByValue(dt.Rows(0)("Custom_Attribute_1").ToString) Is Nothing Then
                        ddlType.ClearSelection()
                        ddlType.Items.FindByValue(dt.Rows(0)("Custom_Attribute_1").ToString).Selected = True
                    End If
                    btnAddItems.Text = "Update"
                Else
                    txtDisValue.Text = ""
                    txtvalue.Text = ""
                    ddlType.ClearSelection()
                    btnAddItems.Text = "Add"
                End If
            End If
        End If
    End Sub

    Private Sub ddlCustomer_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ddlCustomer.SelectedIndexChanged
        LoadDiscountData()
    End Sub


    Private Sub btnImportWindow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportWindow.Click
        If Me.ddl_org.SelectedIndex > 0 Then
            Me.lblUpMsg.Text = ""
            Session("dtDiscountErrors") = Nothing
            dgvErros.DataSource = Nothing
            dgvErros.DataBind()
            dgvErros.Visible = False
            lbLog.Visible = False
            Me.MPEImport.Show()
        Else
            'Me.lblinfo.Text = "Information"
            'Me.lblMessage.Text = "Please select a organization."
            'Me.lblMessage.ForeColor = Drawing.Color.Red
            'Me.MpInfoError.Show()
            MessageBoxValidation("Please select a organization", "Information")
            Exit Sub
        End If
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
    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try

            If Me.ddl_org.SelectedIndex > 0 Then
                Dim dtOriginal As New DataTable()
                Dim OrgID As String = Me.ddl_org.SelectedValue

                dtOriginal = objCustomer.GetDiscountDefinition(Err_No, Err_Desc, OrgID)

                Dim dtTemp As New DataTable()

                'Creating Header Row
                dtTemp.Columns.Add("Customer_No")
                dtTemp.Columns.Add("MinOrderValue")
                dtTemp.Columns.Add("Discount")
                dtTemp.Columns.Add("DiscountType")
                '  dtTemp.Columns.Add("UOM")

                Dim drAddItem As DataRow
                For i As Integer = 0 To dtOriginal.Rows.Count - 1
                    drAddItem = dtTemp.NewRow()
                    drAddItem(0) = dtOriginal.Rows(i)("Customer_No").ToString()
                    drAddItem(1) = dtOriginal.Rows(i)("minorder").ToString()
                    drAddItem(2) = dtOriginal.Rows(i)("Attrib_Value").ToString()
                    drAddItem(3) = dtOriginal.Rows(i)("DiscType").ToString()
                    dtTemp.Rows.Add(drAddItem)
                Next

                If dtOriginal.Rows.Count = 0 Then

                    'Me.lblinfo.Text = "Information"
                    'Me.lblMessage.Text = "There is no data for the selected filter criteria"
                    'Me.lblMessage.ForeColor = Drawing.Color.Red
                    'Me.MpInfoError.Show()
                    MessageBoxValidation("There is no data for the selected filter criteria", "Information")
                    Exit Sub

                    drAddItem = dtTemp.NewRow()
                    drAddItem(0) = ""
                    drAddItem(1) = ""
                    drAddItem(2) = ""
                    drAddItem(3) = ""
                    drAddItem(4) = ""
                    drAddItem(5) = ""
                    drAddItem(6) = ""
                    '  drAddItem(7) = ""
                    dtTemp.Rows.Add(drAddItem)
                End If

                'Temp(Grid)
                Dim dg As New DataGrid()
                dg.DataSource = dtTemp
                dg.DataBind()
                If dtTemp.Rows.Count > 0 Then
                    'Dim fn As String = "MSL" & Now.ToString("ddMMMyyHHmmss") + ".xls"
                    Dim fn As String = "Discount" + ".xls"
                    Dim d As New DataSet
                    d.Tables.Add(dtTemp)

                    ExportToExcel(fn, d)

                End If
            Else
                'Me.lblinfo.Text = "Information"
                'Me.lblMessage.Text = "Please select a organization"
                'Me.lblMessage.ForeColor = Drawing.Color.Red
                'Me.MpInfoError.Show()
                MessageBoxValidation("Please select a organization", "Information")
                Exit Sub
            End If

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("information.aspx?mode=1&errno=" & "74071" & "&msg=" & AppMsgHandler.GetErrorMessage("SWX_BO_Msg_005") & "&next=AdminOrderlvlCustomerFOC.aspx", False)
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

    Protected Sub btnAddItems_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddItems.Click
        Dim id() As String
        If ddl_org.SelectedItem.Value = "0" Then
            'Me.lblinfo.Text = "Validation"
            'Me.lblMessage.Text = "Please select the Organisation"
            'Me.lblMessage.ForeColor = Drawing.Color.Red
            'Me.MpInfoError.Show()
            'ClassUpdatePnl1.Update()
            'Exit Sub
            MessageBoxValidation("Please select the Organisation", "Validation")
            Exit Sub
        End If
        If ddlCustomer.SelectedItem Is Nothing Then
            'Me.lblinfo.Text = "Validation"
            'Me.lblMessage.Text = "Please select the customer"
            'Me.lblMessage.ForeColor = Drawing.Color.Red
            'Me.MpInfoError.Show()
            'ClassUpdatePnl1.Update()
            MessageBoxValidation("Please select the customer", "Validation")
            Exit Sub
        End If
        If ddlCustomer.Text = "" Then
            'Me.lblinfo.Text = "Validation"
            'Me.lblMessage.Text = "Please select the customer"
            'Me.lblMessage.ForeColor = Drawing.Color.Red
            'Me.MpInfoError.Show()
            'ClassUpdatePnl1.Update()
            MessageBoxValidation("Please select the customer", "Validation")
            Exit Sub
        End If
        If txtvalue.Text.Trim = "" Then
            'Me.lblinfo.Text = "Validation"
            'Me.lblMessage.Text = "Please enter the Minimum Order values"
            'Me.lblMessage.ForeColor = Drawing.Color.Red
            'Me.MpInfoError.Show()
            'ClassUpdatePnl1.Update()
            MessageBoxValidation("Please enter the Minimum Order values", "Validation")
            Exit Sub
        End If
        If txtDisValue.Text.Trim = "" Then
            'Me.lblinfo.Text = "Validation"
            'Me.lblMessage.Text = "Please enter the Discount"
            'Me.lblMessage.ForeColor = Drawing.Color.Red
            'Me.MpInfoError.Show()
            'ClassUpdatePnl1.Update()
            MessageBoxValidation("Please enter the Discount", "Validation")
            Exit Sub
        End If
        If ddlType.SelectedItem.Value = "P" Then
            If Val(txtDisValue.Text) > 100 Then
                'Me.lblinfo.Text = "Validation"
                'Me.lblMessage.Text = "Discount Percentage can not be greater than 100"
                'Me.lblMessage.ForeColor = Drawing.Color.Red
                'Me.MpInfoError.Show()
                'ClassUpdatePnl1.Update()
                MessageBoxValidation("Discount Percentage can not be greater than 100", "Validation")
                Exit Sub
            End If
        End If
        If ddlType.SelectedItem.Value = "V" Then
            If Val(txtDisValue.Text) > Val(txtvalue.Text.Trim) Then
                'Me.lblinfo.Text = "Validation"
                'Me.lblMessage.Text = "Discount Percentage can not be greater than 100"
                'Me.lblMessage.ForeColor = Drawing.Color.Red
                'Me.MpInfoError.Show()
                'ClassUpdatePnl1.Update()
                MessageBoxValidation("Discount value can not be greater than order value", "Validation")
                Exit Sub
            End If
        End If
        If ddlCustomer.SelectedItem.Text <> "" Then
            id = ddlCustomer.SelectedItem.Value.Split("$")
            If objCustomer.SaveOrderLvlCustomerDiscount(Err_No, Err_Desc, id(0), id(1), ddlType.SelectedItem.Value, txtvalue.Text, txtDisValue.Text, ddl_transactiontype.SelectedItem.Value) Then
                BindDiscountData()
                'Me.lblinfo.Text = "Information"
                'Me.lblMessage.Text = "Discount rule saved successfully"
                'Me.lblMessage.ForeColor = Drawing.Color.Red
                'Me.MpInfoError.Show()
                'ClassUpdatePnl1.Update()
                MessageBoxValidation("Discount rule saved successfully", "Information")
                Exit Sub
                If btnAddItems.Text = "Add" Then
                    objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "ORDER LVL CUST DISC", ddlCustomer.SelectedItem.Value, "Org Val: " & ddl_org.SelectedItem.Value & " Order Val: " & txtvalue.Text.Trim & " Discount:" & txtDisValue.Text & " Type:" & ddlType.SelectedItem.Value, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                Else
                    objLogin.SaveUserLog(Err_No, Err_Desc, "U", "MAIN MENU", "ORDER LVL CUST DISC", ddlCustomer.SelectedItem.Value, "Org Val: " & ddl_org.SelectedItem.Value & " Order Val: " & txtvalue.Text.Trim & " Discount:" & txtDisValue.Text & " Type:" & ddlType.SelectedItem.Value, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                End If
            Else
                'Me.lblinfo.Text = "Information"
                'Me.lblMessage.Text = "Error occured while saving data"
                'Me.lblMessage.ForeColor = Drawing.Color.Red
                'Me.MpInfoError.Show()
                'ClassUpdatePnl1.Update()
                MessageBoxValidation("Error occured while saving data", "Information")
                Exit Sub
            End If
        End If
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Private Sub dgvItems_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgvItems.PageIndexChanging
        dgvItems.PageIndex = e.NewPageIndex

        BindDiscountData()
    End Sub

    Private Sub dgvItems_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgvItems.RowCommand
        Try
            If (e.CommandName = "EditRecord") Then

                LoadBonusType()

                Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)


                Dim lblCustomer_ID As Label = DirectCast(row.FindControl("lblCustomer_ID"), Label)
                Dim lblSite_Use_ID As Label = DirectCast(row.FindControl("lblSite_Use_ID"), Label)
                Dim lbl_Custom_Attribute_3 As Label = DirectCast(row.FindControl("lbl_Custom_Attribute_3"), Label)

                Me.ddlCustomer.SelectedValue = lblCustomer_ID.Text & "$" & lblSite_Use_ID.Text
                Me.ddlType.SelectedValue = row.Cells(4).Text
                Me.txtvalue.Text = row.Cells(2).Text
                Me.txtDisValue.Text = row.Cells(3).Text
                If Not ddl_transactiontype.Items.FindByValue(lbl_Custom_Attribute_3.Text) Is Nothing Then
                    ddl_transactiontype.ClearSelection()
                    ddl_transactiontype.Items.FindByValue(lbl_Custom_Attribute_3.Text).Selected = True
                End If
                Me.btnAddItems.Text = "Update"
                UpdatePanel4.Update()
            End If

            If (e.CommandName = "DeleteRecord") Then

                Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim lblCustomer_ID As Label = DirectCast(row.FindControl("lblCustomer_ID"), Label)
                Dim lblSite_Use_ID As Label = DirectCast(row.FindControl("lblSite_Use_ID"), Label)
                If objCustomer.DeleteDiscountDefinition(Err_No, Err_Desc, lblCustomer_ID.Text, lblSite_Use_ID.Text) Then
                    objLogin.SaveUserLog(Err_No, Err_Desc, "D", "MAIN MENU", "ORDER LVL CUST DISC", lblCustomer_ID.Text & "$" & lblSite_Use_ID.Text, "Customer :" & lblCustomer_ID.Text & "$" & lblSite_Use_ID.Text & "Org Val: " & ddl_org.SelectedItem.Value, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                End If
                Me.btnAddItems.Text = "Add"
                Me.txtvalue.Text = ""
                Me.ddlType.SelectedIndex = 0
                Me.txtDisValue.Text = ""
                LoadBonusType()
                BindDiscountData()
                UpdatePanel4.Update()

            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("information.aspx?mode=1&errno=" & "74071" & "&msg=" & AppMsgHandler.GetErrorMessage("SWX_BO_Msg_005") & "&next=DiscountDefinition.aspx", False)
        Finally
        End Try
    End Sub

    Private Sub dgvItems_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgvItems.Sorting
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"

        BindDiscountData()
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClear.Click
        ResetDetails()
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
        Session("dtDiscountErrors") = Nothing
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
                        col.ColumnName = "Customer_No"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)

                        col = New DataColumn
                        col.ColumnName = "MinOrderValue"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)

                        col = New DataColumn
                        col.ColumnName = "Discount"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)

                        col = New DataColumn
                        col.ColumnName = "DiscountType"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)




                        If ViewState("FileName").ToString.EndsWith(".csv") Then
                            TempTbl = DoCSVUploadDiscount()
                        ElseIf ViewState("FileName").ToString.EndsWith(".xls") Then
                            TempTbl = DoXLSUploadDiscount()
                        ElseIf ViewState("FileName").ToString.EndsWith(".xlsx") Then
                            TempTbl = DoXLSXUploadDiscount()
                        End If

                        If TempTbl.Columns.Count = 4 Then

                            If Not (TempTbl.Columns(0).ColumnName.ToLower = "customer_no" And TempTbl.Columns(1).ColumnName.ToLower = "minordervalue" And TempTbl.Columns(2).ColumnName.ToLower = "discount" And TempTbl.Columns(3).ColumnName.ToLower = "discounttype") Then
                                lblUpMsg.Text = "Please check the template columns are correct"

                                Me.MPEImport.Show()
                                Exit Sub
                            End If



                        Else
                            'lblUpMsg.Text = "Invalid Template"
                            ' '' lblMessage.ForeColor = Drawing.Color.Green
                            'lblinfo.Text = "Information"
                            '' MpInfoError.Show()
                            'Me.MPEImport.Show()
                            MessageBoxValidation("Invalid Template", "Validation")
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
                        Dim OrgID As String = Me.ddl_org.SelectedValue



                        If TempTbl.Rows.Count > 0 Then



                            Dim idx As Integer

                            For idx = 0 To TempTbl.Rows.Count - 1

                                Dim CustomerNo As String = Nothing
                                Dim OrderVal As String = Nothing
                                Dim Discount As String = Nothing
                                Dim DiscountType As String = Nothing

                                Dim isValidRow As Boolean = True


                                CustomerNo = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, "0", TempTbl.Rows(idx)(0).ToString())
                                OrderVal = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, "0", TempTbl.Rows(idx)(1).ToString())

                                Discount = IIf(TempTbl.Rows(idx)(2) Is DBNull.Value, "0", TempTbl.Rows(idx)(2).ToString())

                                DiscountType = Trim(Replace(IIf(TempTbl.Rows(idx)(3) Is DBNull.Value, "0", TempTbl.Rows(idx)(3).ToString()), "12:00:00 AM", ""))


                                Dim Customer_ID As String = ""
                                Dim Site_ID As String = ""

                                If CustomerNo = "0" Or OrderVal = "0" Or Discount = "0" Or DiscountType = "0" Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + " Blank Values "
                                    TotFailed += 1
                                    isValidRow = False
                                Else
                                    If objCustomer.CheckCustomerNoExist(OrgID, CustomerNo, Customer_ID, Site_ID) = False Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Invalid Customer No." + ","
                                        TotFailed += 1
                                        isValidRow = False
                                    Else
                                        TempTbl.Rows(idx)("Customer_ID") = Customer_ID
                                        TempTbl.Rows(idx)("Site_use_ID") = Site_ID
                                    End If

                                    If DiscountType = "0" Or Not (DiscountType.ToUpper() = "V" Or DiscountType.ToUpper() = "P") Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Invalid discount type" + ","
                                        TotFailed += 1
                                        isValidRow = False
                                    End If

                                    If IsNumeric(OrderVal) = False Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Minimum Order value should be in numeric" + ","
                                        TotFailed += 1
                                        isValidRow = False
                                    End If


                                    If IsNumeric(Discount) = False Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Discount value should be in numeric" + ","
                                        TotFailed += 1
                                        isValidRow = False
                                    End If


                                    If DiscountType.ToString().ToUpper() = "P" And IsNumeric(Discount) = True Then
                                        If CDec(IIf(Discount = "", "0", Discount)) > 100 Then
                                            ErrorText = ErrorText + "Discount Percentage should be less than or equal to 100"
                                            isValidRow = False
                                            TotFailed += 1
                                        End If
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
                                    'Dim h As DataRow = dtErrors.NewRow()
                                    'h("RowNo") = idx + 2
                                    'h("LogInfo") = "Successfully uploaded"
                                    'dtErrors.Rows.Add(h)
                                    'RowNo = Nothing
                                    ErrorText = Nothing
                                    isValidRow = True
                                End If
                            Next
                        End If

                        If objCustomer.UploadDiscount(TempTbl, Me.ddl_org.SelectedValue, Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID.ToString()) = True Then
                            DeleteExcel()
                            lblUpMsg.Text = IIf(TotFailed = 0, "Successfully uploaded", "One or more rows has invalid data. Please check the log")
                            MPEImport.Show()
                            BindDiscountData()

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
                    ClassUpdatePnl1.Update()
                    Session.Remove("dtDiscountErrors")
                    Session("dtDiscountErrors") = dtErrors.Copy


                    Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & "Discount_" & Now.ToString("yyyyMMdd") + ".txt"
                    DataTable2CSV(dtErrors, fn, vbTab)
                    If dtErrors.Rows.Count > 0 Then
                        lbLog.Visible = True
                    End If
                    Session.Remove("DiscountLogInfo")
                    Session("DiscountLogInfo") = fn


                    dtErrors = Nothing

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
                'lblMessage.Text = "Please import valid Excel template."
                'lblMessage.ForeColor = Drawing.Color.Green
                'lblinfo.Text = "Information"
                'MpInfoError.Show()
                MessageBoxValidation("Please import valid Excel template", "Information")
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
    Private Function DoCSVUploadDiscount() As DataTable
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

    Private Function DoXLSUploadDiscount() As DataTable
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
    Private Function DoXLSXUploadDiscount() As DataTable
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
    Private Sub dgvErros_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgvErros.PageIndexChanging
        dgvErros.PageIndex = e.NewPageIndex
        If Not Session("dtDiscountErrors") Is Nothing Then
            Dim dtErrors As DataTable
            dtErrors = CType(Session("dtDiscountErrors"), DataTable)
            Me.dgvErros.DataSource = dtErrors
            Me.dgvErros.DataBind()
            Me.MPEImport.Show()
            ClassUpdatePnl1.Update()
        End If
    End Sub
    Private Sub btnFOCClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFOCClear.Click
        ddlItems.Text = ""
        Me.btnAddFOCItems.Text = "Add"
        btnAddFOCItems.Enabled = True
        Me.lbl_msgFoc.Text = ""
        ddlItems.ClearSelection()
        lbl_FOCItemMsg.Text = ""
    End Sub
    Private Sub btnAddFOCItems_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddFOCItems.Click
        Try
            If ddl_org.SelectedItem.Value = "0" Then
                'Me.lblinfo.Text = "Validation"
                'Me.lblMessage.Text = "Please select the Organisation"
                'Me.lblMessage.ForeColor = Drawing.Color.Red
                'Me.MpInfoError.Show()
                'ClassUpdatePnl1.Update()
                MessageBoxValidation("Please select the Organisation", "Validation")
                Exit Sub
            End If
            If ddlItems.SelectedItem Is Nothing Then
                'Me.lblinfo.Text = "Validation"
                'Me.lblMessage.Text = "Please select the item"
                'Me.lblMessage.ForeColor = Drawing.Color.Red
                'Me.MpInfoError.Show()
                'ClassUpdatePnl1.Update()
                MessageBoxValidation("Please select the item", "Validation")
                Exit Sub
            End If
            If ddlItems.Text = "" Then
                'Me.lblinfo.Text = "Validation"
                'Me.lblMessage.Text = "Please select the item"
                'Me.lblMessage.ForeColor = Drawing.Color.Red
                'Me.MpInfoError.Show()
                'ClassUpdatePnl1.Update()
                MessageBoxValidation("Please select the item", "Validation")
                Exit Sub
            End If
            If objProduct.SaveDiscountFOC(Err_No, Err_Desc, ddl_org.SelectedItem.Value, ddlItems.SelectedItem.Value, "1") Then
                BindFOCData()
                'Me.lblinfo.Text = "Information"
                'Me.lblMessage.Text = "FOC Item Added"
                'Me.lblMessage.ForeColor = Drawing.Color.Red
                'Me.MpInfoError.Show()
                'ClassUpdatePnl1.Update()
                MessageBoxValidation("FOC Item Added", "Information")
                Exit Sub
                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "DISC FOC ITEM", ddlItems.SelectedItem.Text, "Org: " & ddl_org.SelectedItem.Value, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
            Else
                'Me.lblinfo.Text = "Information"
                'Me.lblMessage.Text = "Error occured while saving data"
                'Me.lblMessage.ForeColor = Drawing.Color.Red
                'Me.MpInfoError.Show()
                'ClassUpdatePnl1.Update()
                MessageBoxValidation("Error occured while saving data", "Information")
                Exit Sub

            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("information.aspx?mode=1&errno=" & "74071" & "&msg=" & AppMsgHandler.GetErrorMessage("SWX_BO_Msg_005") & "&next=AdminOrderlvlCustomerFOC.aspx", False)
        Finally
        End Try
    End Sub

    Private Sub btnImportWindowFOC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportWindowFOC.Click
        Try
            If Me.ddl_org.SelectedIndex > 0 Then
                Me.lbl_msgFoc.Text = ""
                Session("dtFOCErrors") = Nothing
                Gv_errorFOC.DataSource = Nothing
                Gv_errorFOC.DataBind()
                Gv_errorFOC.Visible = False
                Me.MPFOC.Show()
            Else
                'Me.lblinfo.Text = "Information"
                'Me.lblMessage.Text = "Please select a organization."
                'Me.lblMessage.ForeColor = Drawing.Color.Red
                'Me.MpInfoError.Show()
                MessageBoxValidation("Please select a organization", "Information")
                Exit Sub
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("information.aspx?mode=1&errno=" & "74071" & "&msg=" & AppMsgHandler.GetErrorMessage("SWX_BO_Msg_005") & "&next=AdminOrderlvlCustomerFOC.aspx", False)
        Finally
        End Try
    End Sub

    Private Sub btnImportFOC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportFOC.Click

        If Me.FileUploadFOC.FileName = Nothing Then
            ' Me.lblinfo.Text = "Validation"
            Me.lblUpMsg.Text = "Select filename "
            ' Me.lblMessage.ForeColor = Drawing.Color.Green
            ' Me.MpInfoError.Show()
            Me.MPEImport.Show()
            Exit Sub
        End If
        Session("dtFOCErrors") = Nothing
        Dim dtErrors As New DataTable
        Dim Str As New StringBuilder
        dtErrors = SetErrorsTable().Copy
        Dim TotSuccess As Integer = 0
        Dim TotFailed As Integer = 0

        Try
            ViewState("FileType") = Me.FileUploadFOC.PostedFile.ContentType
            If FileUploadFOC.FileName.ToString.EndsWith(".csv") Or FileUploadFOC.FileName.ToString.EndsWith(".xls") Or FileUploadFOC.FileName.ToString.EndsWith(".xlsx") Then

                Dim Foldername As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath")
                If Not Foldername.EndsWith("\") Then
                    Foldername = Foldername & "\"
                End If
                If Directory.Exists(Foldername.Substring(0, Len(Foldername) - 1)) = False Then
                    Directory.CreateDirectory(Foldername.Substring(0, Len(Foldername) - 1))
                End If
                If FileUploadFOC.FileName.ToString.EndsWith(".csv") Then
                    Dim FName As String
                    FName = Now.Hour & Now.Minute & Now.Second & FileUploadFOC.FileName
                    ViewState("FileName") = Foldername & FName
                    ViewState("CSVName") = FName
                Else
                    ViewState("FileName") = Foldername & Now.Hour & Now.Minute & Now.Second & FileUploadFOC.FileName
                End If

                FileUploadFOC.SaveAs(ViewState("FileName"))



                Try
                    Dim st As Boolean = False

                    If ViewState("FileType") IsNot Nothing And ViewState("FileName") IsNot Nothing Then
                        Dim TempTbl As New DataTable
                        If TempTbl.Rows.Count > 0 Then
                            TempTbl.Rows.Clear()
                        End If



                        Dim col As DataColumn


                        col = New DataColumn
                        col.ColumnName = "Item_no"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)


                        If ViewState("FileName").ToString.EndsWith(".csv") Then
                            TempTbl = DoCSVUploadFOC()
                        ElseIf ViewState("FileName").ToString.EndsWith(".xls") Then
                            TempTbl = DoXLSUploadFOC()
                        ElseIf ViewState("FileName").ToString.EndsWith(".xlsx") Then
                            TempTbl = DoXLSXUploadFOC()
                        End If

                        If TempTbl.Columns.Count = 1 Then

                            If Not (TempTbl.Columns(0).ColumnName.ToLower = "item_no") Then
                                lbl_msgFoc.Text = "Please check the template columns are correct"
                                Me.MPFOC.Show()
                                Exit Sub
                            End If

                        Else
                            lbl_msgFoc.Text = "Invalid Template"
                            Me.MPFOC.Show()
                            Exit Sub
                        End If



                        TempTbl.Columns.Add("IsValid", GetType(String))

                        If TempTbl.Rows.Count = 0 Then
                            lblUpMsg.Text = "There is no data in the uploaded file."
                            ' lblMessage.ForeColor = Drawing.Color.Green
                            'lblinfo.Text = "Information"
                            'MpInfoError.Show()
                            Me.MPFOC.Show()
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

                                Dim ItemNo As String = Nothing

                                Dim isValidRow As Boolean = True


                                ItemNo = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, "0", TempTbl.Rows(idx)(0).ToString())

                                If ItemNo = "0" Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + " Blank Values "
                                    TotFailed += 1
                                    isValidRow = False
                                Else
                                    If objProduct.CheckProductValid("tblProduct", ItemNo, OrgID, 0, Err_No, Err_Desc) = False Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Invalid Item No." + ","
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
                                    'Dim h As DataRow = dtErrors.NewRow()
                                    'h("RowNo") = idx + 2
                                    'h("LogInfo") = "Successfully uploaded"
                                    'dtErrors.Rows.Add(h)
                                    'RowNo = Nothing
                                    ErrorText = Nothing
                                    isValidRow = True
                                End If
                            Next
                        End If

                        If objProduct.UploadFOCDiscount(TempTbl, Me.ddl_org.SelectedValue, Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID.ToString()) = True Then
                            DeleteExcel()
                            lbl_msgFoc.Text = IIf(TotFailed = 0, "Successfully uploaded", "One or more rows has invalid data. Please check the log")
                            MPFOC.Show()
                            BindFOCData()

                        Else
                            DeleteExcel()
                            lbl_msgFoc.Text = "Please check the uploaded log"
                            MPFOC.Show()
                            Exit Sub
                        End If
                    End If
                    If dtErrors.Rows.Count > 0 Then
                        lbLogFOC.Visible = True
                    Else
                        lbLogFOC.Visible = False
                    End If
                    Gv_errorFOC.Visible = False
                    Me.Gv_errorFOC.DataSource = dtErrors
                    Me.Gv_errorFOC.DataBind()
                    ClassUpdatePnl1.Update()
                    Session.Remove("dtFOCErrors")
                    Session("dtFOCErrors") = dtErrors


                    Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & "FOC_" & Now.ToString("yyyyMMdd") + ".txt"
                    DataTable2CSV(dtErrors, fn, vbTab)

                    Session.Remove("FOCLogInfo")
                    Session("FOCLogInfo") = fn
                    dtErrors = Nothing

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
                'lblMessage.Text = "Please import valid Excel template."
                'lblMessage.ForeColor = Drawing.Color.Green
                'lblinfo.Text = "Information"
                'MpInfoError.Show()
                MessageBoxValidation("Please import valid Excel template", "Information")
            End If


        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try

    End Sub
    Protected Sub lbLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbLog.Click
        Try


            If Not Session("DiscountLogInfo") Is Nothing Then
                Dim fileValue As String = Session("DiscountLogInfo")





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
    Private Sub dgvItemsFOC_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgvItemsFOC.PageIndexChanging
        dgvItemsFOC.PageIndex = e.NewPageIndex

        BindFOCData()
    End Sub

    Private Sub dgvItemsFOC_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgvItemsFOC.RowCommand
        Try

            If (e.CommandName = "DeleteRecord") Then

                Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim lblItem_ID As Label = DirectCast(row.FindControl("lblItem_ID"), Label)
                If objProduct.SaveDiscountFOC(Err_No, Err_Desc, ddl_org.SelectedItem.Value, lblItem_ID.Text, "2") Then
                    objLogin.SaveUserLog(Err_No, Err_Desc, "D", "MAIN MENU", "DISC FOC ITEM", lblItem_ID.Text, "Org Val: " & ddl_org.SelectedItem.Value, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                End If
                BindFOCData()
                UpdatePanel5.Update()

            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("information.aspx?mode=1&errno=" & "74071" & "&msg=" & AppMsgHandler.GetErrorMessage("SWX_BO_Msg_005") & "&next=AdminOrderlvlCustomerFOC.aspx", False)
        Finally
        End Try
    End Sub

    Private Sub btnExportFOC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportFOC.Click
        If Me.ddl_org.SelectedIndex > 0 Then
            Dim dtOriginal As New DataTable()
            Dim OrgID As String = Me.ddl_org.SelectedValue

            dtOriginal = objProduct.GetFOCDefinition(Err_No, Err_Desc, OrgID)

            Dim dtTemp As New DataTable()

            'Creating Header Row
            dtTemp.Columns.Add("Item_No")

            '  dtTemp.Columns.Add("UOM")

            Dim drAddItem As DataRow
            For i As Integer = 0 To dtOriginal.Rows.Count - 1
                drAddItem = dtTemp.NewRow()
                drAddItem(0) = dtOriginal.Rows(i)("Item_No").ToString()
                dtTemp.Rows.Add(drAddItem)
            Next

            If dtOriginal.Rows.Count = 0 Then

                'Me.lblinfo.Text = "Information"
                'Me.lblMessage.Text = "There is no data for the selected filter criteria"
                'Me.lblMessage.ForeColor = Drawing.Color.Red
                'Me.MpInfoError.Show()
                MessageBoxValidation("There is no data for the selected filter criteria", "Information")
                Exit Sub

                drAddItem = dtTemp.NewRow()
                drAddItem(0) = ""
                dtTemp.Rows.Add(drAddItem)
            End If

            'Temp(Grid)
            Dim dg As New DataGrid()
            dg.DataSource = dtTemp
            dg.DataBind()
            If dtTemp.Rows.Count > 0 Then
                'Dim fn As String = "MSL" & Now.ToString("ddMMMyyHHmmss") + ".xls"
                Dim fn As String = "FOC" + ".xls"
                Dim d As New DataSet
                d.Tables.Add(dtTemp)

                ExportToExcel(fn, d)

            End If
        Else
            'Me.lblinfo.Text = "Information"
            'Me.lblMessage.Text = "Please select a organization"
            'Me.lblMessage.ForeColor = Drawing.Color.Red
            'Me.MpInfoError.Show()
            MessageBoxValidation("Please select a organization", "Information")
            Exit Sub
        End If

    End Sub

    Private Sub Gv_errorFOC_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles Gv_errorFOC.PageIndexChanging
        Gv_errorFOC.PageIndex = e.NewPageIndex
        If Not Session("dtFOCErrors") Is Nothing Then
            Dim dtErrors As DataTable
            dtErrors = CType(Session("dtFOCErrors"), DataTable)
            Me.Gv_errorFOC.DataSource = dtErrors
            Me.Gv_errorFOC.DataBind()
            Me.MPFOC.Show()
            ClassUpdatePnl1.Update()
        End If
    End Sub
    Private Function DoCSVUploadFOC() As DataTable
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

    Private Function DoXLSUploadFOC() As DataTable
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
    Private Function DoXLSXUploadFOC() As DataTable
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

    Private Sub lbLogFOC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbLogFOC.Click
        If Not Session("FOCLogInfo") Is Nothing Then
            Dim fileValue As String = Session("FOCLogInfo")


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
                'Response.Clear()

                'Response.AddHeader("Content-Disposition", "attachment; filename=" & file.Name)

                'Response.AddHeader("Content-Length", file.Length.ToString())

                'Response.WriteFile(file.FullName)


                'Response.[End]()
            Else
                lbl_msgFoc.Text = "File does not exist"
                'lblMessage.ForeColor = Drawing.Color.Green
                'lblinfo.Text = "Information"
                MPFOC.Show()
                Exit Sub

            End If

        Else
            lbl_msgFoc.Text = "There is no log to show."
            'lblMessage.ForeColor = Drawing.Color.Green
            'lblinfo.Text = "Information"
            MPFOC.Show()
            Exit Sub

        End If

    End Sub

    Private Sub ddlItems_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ddlItems.SelectedIndexChanged
        If Not ddlItems.SelectedItem Is Nothing Then
            If ddlItems.SelectedItem.Text <> "" Then

                Dim dt As New DataTable
                dt = objProduct.GetDiscountFOCItem(Err_No, Err_Desc, ddl_org.SelectedItem.Value, ddlItems.SelectedItem.Value)
                If dt.Rows.Count > 0 Then
                    lbl_FOCItemMsg.Text = "This item is already added as FOC item"
                    btnAddFOCItems.Enabled = False
                Else
                    lbl_FOCItemMsg.Text = ""
                    btnAddFOCItems.Enabled = True
                End If
            End If
        End If
    End Sub
End Class