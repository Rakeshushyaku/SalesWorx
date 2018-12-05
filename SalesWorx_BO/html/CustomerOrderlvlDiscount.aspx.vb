
Imports SalesWorx.BO.Common

Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports ExcelLibrary.SpreadSheet
Imports Telerik.Web.UI
Partial Public Class CustomerOrderlvlDiscount
    Inherits System.Web.UI.Page
    Dim objProduct As New Product
    Dim objCustomer As New Customer
    Dim ObjCommon As New Common
    Dim objLogin As New SalesWorx.BO.Common.Login

    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const PageID As String = "P315"
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
                ResetDetails()
                FillICustomerList()
                BindDiscountData()

                ViewState("FileType") = Nothing
                ViewState("FileName") = Nothing
                ViewState("CSVName") = Nothing
            Else
                Me.MPEImport.VisibleOnPageLoad = False
            End If
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_005") & "&next=Welcome.aspx&Title=Discount Definition", False)
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
        ddl_org.Items.Add(New RadComboBoxItem("-- Select a Organization --"))
        ddl_org.AppendDataBoundItems = True
        ddl_org.DataValueField = "MAS_Org_ID"
        ddl_org.DataTextField = "Description"
        ddl_org.DataBind()

    End Sub
    Private Sub ResetDetails()
        Me.txtMinValue.Text = ""
        Me.txtMaxValue.Text = ""
        ddlCustomer.Text = ""
        Me.txtvalue.Text = ""
        Me.btnAddItems.Text = "Add"
        Me.lblUpMsg.Text = ""
        Me.lblLineID.Text = ""
        ddlCustomer.ClearSelection()
        ddlCustomer.Items.Clear()
        ddlCustomer.Text = ""
        ddl_transactiontype.SelectedIndex = 0
    End Sub


    Sub FillICustomerList()
        ' ''Dim x As New DataTable
        ' ''x = ObjCommon.GetCustomerfromOrg(Err_No, Err_Desc, ddl_org.SelectedValue)

        ' ''Me.ddlCustomer.ClearSelection()
        ' ''Me.ddlCustomer.Items.Clear()
        ' ''Me.ddlCustomer.Text = ""

        ' ''ddlCustomer.DataTextField = "Customer"
        ' ''ddlCustomer.DataValueField = "CustomerID"
        ' ''ddlCustomer.DataSource = x
        ' ''ddlCustomer.DataBind()

    End Sub

    Private Sub BindDiscountData()

        Dim x As New DataTable
        x = objCustomer.GetCustomerOrderDiscount(Err_No, Err_Desc, ddl_org.SelectedItem.Value)

        Dim dv As New DataView(x)
        dv = x.DefaultView
        If ViewState("SortField") <> "" Then
            dv.Sort = (ViewState("SortField") & " ") + SortDirection
        End If

        Me.dgvItems.DataSource = dv
        Me.dgvItems.DataBind()

    End Sub
    Private Sub ddl_org_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_org.SelectedIndexChanged
        '' FillICustomerList()


        BindDiscountData()
        ResetDetails()
    End Sub
    Sub LoadDiscountData()
        Dim id() As String
        If Not ddlCustomer.SelectedValue Is Nothing Then
            If ddlCustomer.SelectedValue <> "" Then
                id = ddlCustomer.SelectedValue.Split("$")
                Dim dt As New DataTable
                dt = objCustomer.GetCustomerOrderDiscountByID(Err_No, Err_Desc, id(0), id(1))
                If dt.Rows.Count > 0 Then
                    txtvalue.Text = CDec(IIf(dt.Rows(0)("Attrib_Value") Is DBNull.Value, "0", dt.Rows(0)("Attrib_Value").ToString())).ToString("0.00")
                    txtMinValue.Text = CDec(IIf(dt.Rows(0)("Custom_Attribute_1") Is DBNull.Value, "0", dt.Rows(0)("Custom_Attribute_1").ToString())) * 100.0
                    Me.txtMaxValue.Text = CDec(IIf(dt.Rows(0)("Custom_Attribute_2") Is DBNull.Value, "0", dt.Rows(0)("Custom_Attribute_2").ToString())) * 100.0
                    btnAddItems.Text = "Update"
                Else
                    txtMinValue.Text = ""
                    Me.txtMaxValue.Text = ""
                    txtvalue.Text = ""
                    btnAddItems.Text = "Add"
                End If
            End If
        End If
    End Sub

    Private Sub ddlCustomer_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddlCustomer.ItemsRequested
        Try
         
            Dim Objrep As New SalesWorx.BO.Common.Reports()
            Dim dt As New DataTable
            dt = Objrep.GetCustomerfromOrgtextforDiscount(Err_No, Err_Desc, ddl_org.SelectedValue, e.Text)


            Dim ItemsPerRequest As Integer = 100 'Set the number of values to show
            Dim itemOffset As Integer = e.NumberOfItems
            Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
            e.EndOfItems = endOffset = dt.Rows.Count

            'Loop through the values to populate the combo box
            For i As Integer = itemOffset To endOffset - 1
                Dim item As New RadComboBoxItem()
                item.Text = dt.Rows(i).Item("Customer").ToString
                item.Value = dt.Rows(i).Item("CustomerID").ToString

                ddlCustomer.Items.Add(item)
                item.DataBind()
            Next

        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub ddlCustomer_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ddlCustomer.SelectedIndexChanged
        LoadDiscountData()
    End Sub


    Private Sub btnImportWindow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportWindow.Click
        If Me.ddl_org.SelectedIndex > 0 Then
            Me.lblUpMsg.Text = ""
            Session("dtOrderDisErrors") = Nothing
            dgvErros.DataSource = Nothing
            dgvErros.DataBind()
            dgvErros.Visible = False
            lbLog.Visible = False
            Me.MPEImport.VisibleOnPageLoad = True
        Else
            MessageBoxValidation("Please select a organization.", "Validation")
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

                dtOriginal = objCustomer.GetCustomerOrderDiscount(Err_No, Err_Desc, OrgID)

                Dim dtTemp As New DataTable()

                'Creating Header Row
                dtTemp.Columns.Add("Customer_No")
                dtTemp.Columns.Add("MinOrderValue")
                dtTemp.Columns.Add("MinDiscount")
                dtTemp.Columns.Add("MaxDiscount")


                Dim drAddItem As DataRow
                For i As Integer = 0 To dtOriginal.Rows.Count - 1
                    drAddItem = dtTemp.NewRow()
                    drAddItem(0) = dtOriginal.Rows(i)("Customer_No").ToString()
                    drAddItem(1) = CDec(IIf(dtOriginal.Rows(i)("minordervalue") Is DBNull.Value, "0", dtOriginal.Rows(i)("minordervalue").ToString())).ToString("0.00")
                    drAddItem(2) = CDec(IIf(dtOriginal.Rows(i)("MinDisc") Is DBNull.Value, "0", dtOriginal.Rows(i)("MinDisc").ToString())).ToString("0.00")
                    drAddItem(3) = CDec(IIf(dtOriginal.Rows(i)("MaxDisc") Is DBNull.Value, "0", dtOriginal.Rows(i)("MaxDisc").ToString())).ToString("0.00")
                    dtTemp.Rows.Add(drAddItem)
                Next

                If dtOriginal.Rows.Count = 0 Then


                    MessageBoxValidation("There is no data for the selected filter criteria", "Validation")
                    Exit Sub

                    drAddItem = dtTemp.NewRow()
                    drAddItem(0) = ""
                    drAddItem(1) = ""
                    drAddItem(2) = ""
                    drAddItem(3) = ""
                    dtTemp.Rows.Add(drAddItem)
                End If

                'Temp(Grid)
                Dim dg As New DataGrid()
                dg.DataSource = dtTemp
                dg.DataBind()
                If dtTemp.Rows.Count > 0 Then
                    'Dim fn As String = "MSL" & Now.ToString("ddMMMyyHHmmss") + ".xls"
                    Dim fn As String = "CustomerOrderDiscount" + Now.ToString("ddMMMyyHHmmss") + ".xls"
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
            Response.Redirect("information.aspx?mode=1&errno=" & "74071" & "&msg=" & AppMsgHandler.GetErrorMessage("SWX_BO_Msg_005") & "&next=CustomerOrderlvlDiscount.aspx", False)
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

    Protected Sub btnAddItems_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddItems.Click
        Dim id() As String
        If ddl_org.SelectedItem.Value = "-- Select a Organization --" Then
            MessageBoxValidation("Please select the Organisation", "Validation")
            Exit Sub
        End If
        If ddlCustomer.SelectedValue = "" Then
            MessageBoxValidation("Please select the customer", "Validation")
            Exit Sub
        End If
        If ddlCustomer.Text = "" Then
            MessageBoxValidation("Please select the customer", "Validation")
            Exit Sub
        End If
        If txtvalue.Text.Trim = "" Then
            MessageBoxValidation("Please enter the Minimum Order values", "Validation")
            Exit Sub
        End If
        If txtMinValue.Text.Trim = "" Or Me.txtMaxValue.Text = "" Then
            MessageBoxValidation("Please enter the Min/Max Discount", "Validation")
            Exit Sub
        End If

        If Val(txtMinValue.Text) > 100 Or Val(txtMinValue.Text) < 0 Then
            MessageBoxValidation("Minimum Discount Percentage between 0 to 100", "Validation")
            Exit Sub
        End If

        If Val(txtMaxValue.Text) > 100 Or Val(txtMaxValue.Text) < 0 Then
            MessageBoxValidation("Maximum Discount Percentage between 0 to 100", "Validation")
            Exit Sub
        End If

        If Val(txtMinValue.Text) > Val(txtMaxValue.Text) Then
            MessageBoxValidation("Minimum percentage should be less than or equal to maximum percentage", "Validation")
            Exit Sub
        End If

        If ddlCustomer.SelectedValue <> "" Then
            id = ddlCustomer.SelectedValue.Split("$")
            If objCustomer.SaveCustOrderDiscount(Err_No, Err_Desc, id(0), id(1), txtvalue.Text, Me.txtMinValue.Text, Me.txtMaxValue.Text, ddl_transactiontype.SelectedItem.Value) Then
                BindDiscountData()
                MessageBoxValidation("Discount rule saved successfully", "Validation")

                Me.txtMinValue.Text = ""
                Me.txtMaxValue.Text = ""
                ddlCustomer.Text = ""
                Me.txtvalue.Text = ""
                Me.btnAddItems.Text = "Add"
                Me.lblUpMsg.Text = ""
                Me.lblLineID.Text = ""
                ddlCustomer.ClearSelection()

                ClassUpdatePnl1.Update()
                Exit Sub
                If btnAddItems.Text = "Add" Then
                    objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "CUSTOMER ORDER LVL DISC", ddlCustomer.SelectedValue, "Org Val: " & ddl_org.SelectedItem.Value & " Order Val: " & txtvalue.Text.Trim & " Min Discount:" & txtMinValue.Text & " Max Discount:" & Me.txtMaxValue.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                Else
                    objLogin.SaveUserLog(Err_No, Err_Desc, "U", "MAIN MENU", "CUSTOMER ORDER LVL DISC", ddlCustomer.SelectedValue, "Org Val: " & ddl_org.SelectedItem.Value & " Order Val: " & txtvalue.Text.Trim & " Max Discount:" & txtMinValue.Text & " Max Discount:" & Me.txtMaxValue.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                End If
            Else
                MessageBoxValidation("Error occured while saving data", "Validation")

                Exit Sub
            End If
        End If

    End Sub

    Private Sub dgvItems_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgvItems.PageIndexChanging
        dgvItems.PageIndex = e.NewPageIndex

        BindDiscountData()
    End Sub

    Private Sub dgvItems_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgvItems.RowCommand
        Try
            If (e.CommandName = "EditRecord") Then


                Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)


                Dim lblCustomer_ID As Label = DirectCast(row.FindControl("lblCustomer_ID"), Label)
                Dim lblSite_Use_ID As Label = DirectCast(row.FindControl("lblSite_Use_ID"), Label)
                Dim lbl_Custom_Attribute_3 As Label = DirectCast(row.FindControl("lbl_Custom_Attribute_3"), Label)

                ddlCustomer.ClearSelection()
                ddlCustomer.Items.Clear()
                ddlCustomer.Text = ""

                Dim Objrep As New SalesWorx.BO.Common.Reports()
                Dim dt As New DataTable
                dt = Objrep.GetCustomerfromOrgtext(Err_No, Err_Desc, ddl_org.SelectedValue, String.Format("{0}", lblCustomer_ID.Text))



                'Loop through the values to populate the combo box
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim item As New RadComboBoxItem()
                    item.Text = dt.Rows(i).Item("Customer").ToString
                    item.Value = dt.Rows(i).Item("CustomerID").ToString

                    ddlCustomer.Items.Add(item)
                    item.DataBind()
                Next

                If Not ddlCustomer.FindItemByValue(lblCustomer_ID.Text & "$" & lblSite_Use_ID.Text) Is Nothing Then
                    ddlCustomer.ClearSelection()
                    ddlCustomer.FindItemByValue(lblCustomer_ID.Text & "$" & lblSite_Use_ID.Text).Selected = True
                End If

                ' ''  Me.ddlCustomer.SelectedValue = lblCustomer_ID.Text & "$" & lblSite_Use_ID.Text
                Me.txtMinValue.Text = row.Cells(3).Text
                Me.txtvalue.Text = row.Cells(2).Text
                Me.txtMaxValue.Text = row.Cells(4).Text
                If Not ddl_transactiontype.FindItemByValue(lbl_Custom_Attribute_3.Text) Is Nothing Then
                    ddl_transactiontype.ClearSelection()
                    ddl_transactiontype.FindItemByValue(lbl_Custom_Attribute_3.Text).Selected = True
                End If
                Me.btnAddItems.Text = "Update"
                ClassUpdatePnl1.Update()
            End If

            If (e.CommandName = "DeleteRecord") Then

                Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim lblCustomer_ID As Label = DirectCast(row.FindControl("lblCustomer_ID"), Label)
                Dim lblSite_Use_ID As Label = DirectCast(row.FindControl("lblSite_Use_ID"), Label)
                If objCustomer.DeleteCustOrderDiscount(Err_No, Err_Desc, lblCustomer_ID.Text, lblSite_Use_ID.Text) Then
                    objLogin.SaveUserLog(Err_No, Err_Desc, "D", "MAIN MENU", "CUST LVL ORD DISC", lblCustomer_ID.Text & "$" & lblSite_Use_ID.Text, "Customer :" & lblCustomer_ID.Text & "$" & lblSite_Use_ID.Text & "Org Val: " & ddl_org.SelectedItem.Value, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                End If
                Me.btnAddItems.Text = "Add"
                Me.txtvalue.Text = ""
                Me.txtMinValue.Text = ""
                Me.txtMaxValue.Text = ""

                BindDiscountData()
                ClassUpdatePnl1.Update()

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
            Me.MPEImport.VisibleOnPageLoad = True
            Exit Sub
        End If
        Session("dtCustDiscount") = Nothing
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
                        col.ColumnName = "MinDiscount"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)

                        col = New DataColumn
                        col.ColumnName = "MaxDiscount"
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

                            If Not (TempTbl.Columns(0).ColumnName.ToLower = "customer_no" And TempTbl.Columns(1).ColumnName.ToLower = "minordervalue" And TempTbl.Columns(2).ColumnName.ToLower = "mindiscount" And TempTbl.Columns(3).ColumnName.ToLower = "maxdiscount") Then
                                lblUpMsg.Text = "Please check the template columns are correct"

                                Me.MPEImport.VisibleOnPageLoad = True
                                Exit Sub
                            End If



                        Else
                            lblUpMsg.Text = "Invalid Template"
                            '' lblMessage.ForeColor = Drawing.Color.Green

                            ' MpInfoError.Show()
                            Me.MPEImport.VisibleOnPageLoad = True
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

                                Dim CustomerNo As String = Nothing
                                Dim OrderVal As String = Nothing
                                Dim MinDiscount As String = Nothing
                                Dim MaxDiscount As String = Nothing

                                Dim isValidRow As Boolean = True


                                CustomerNo = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, "0", TempTbl.Rows(idx)(0).ToString())
                                OrderVal = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, "0", TempTbl.Rows(idx)(1).ToString())

                                MinDiscount = IIf(TempTbl.Rows(idx)(2) Is DBNull.Value, "0", TempTbl.Rows(idx)(2).ToString())

                                MaxDiscount = IIf(TempTbl.Rows(idx)(3) Is DBNull.Value, "0", TempTbl.Rows(idx)(3).ToString())


                                Dim Customer_ID As String = ""
                                Dim Site_ID As String = ""

                                If CustomerNo = "0" Or OrderVal = "0" Or MinDiscount = "0" Or MaxDiscount = "0" Then
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


                                    If IsNumeric(OrderVal) = False Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Minimum Order value should be in numeric" + ","
                                        TotFailed += 1
                                        isValidRow = False
                                    End If


                                    If IsNumeric(MinDiscount) = False Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Minimum Discount value should be in numeric" + ","
                                        TotFailed += 1
                                        isValidRow = False
                                    End If


                                    If IsNumeric(MaxDiscount) = False Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Maximum Discount value should be in numeric" + ","
                                        TotFailed += 1
                                        isValidRow = False
                                    End If



                                    If IsNumeric(MinDiscount) Then
                                        If CDec(IIf(MinDiscount = "", "0", MinDiscount)) > 100 Then
                                            ErrorText = ErrorText + "Minimum Discount Percentage should be less than or equal to 100"
                                            isValidRow = False
                                            TotFailed += 1
                                            RowNo = idx + 2
                                        End If
                                    End If

                                    If IsNumeric(MaxDiscount) Then
                                        If CDec(IIf(MaxDiscount = "", "0", MaxDiscount)) > 100 Then
                                            ErrorText = ErrorText + "Maximum Discount Percentage should be less than or equal to 100"
                                            isValidRow = False
                                            TotFailed += 1
                                            RowNo = idx + 2
                                        End If
                                    End If


                                    If IsNumeric(MaxDiscount) And IsNumeric(MinDiscount) Then
                                        If CDec(IIf(MinDiscount = "", "0", MinDiscount)) > CDec(IIf(MaxDiscount = "", "0", MaxDiscount)) Then
                                            ErrorText = ErrorText + "Minimum percentage should be less than or equal to maximum percentage"
                                            isValidRow = False
                                            RowNo = idx + 2
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

                        If objCustomer.UploadCustOrderDiscount(TempTbl, Me.ddl_org.SelectedValue, Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID.ToString()) = True Then
                            DeleteExcel()
                            lblUpMsg.Text = IIf(TotFailed = 0, "Successfully uploaded", "One or more rows has invalid data. Please check the log")
                            Me.MPEImport.VisibleOnPageLoad = True
                            BindDiscountData()

                        Else
                            DeleteExcel()
                            lblUpMsg.Text = "Please check the uploaded log"
                            Me.MPEImport.VisibleOnPageLoad = True
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
                    Session.Remove("dtCustDiscount")
                    Session("dtCustDiscount") = dtErrors.Copy


                    Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & "CustOrderDiscount_" & Now.ToString("yyyyMMdd") + ".txt"
                    DataTable2CSV(dtErrors, fn, vbTab)
                    If dtErrors.Rows.Count > 0 Then
                        lbLog.Visible = True
                    End If
                    Session.Remove("CustOrderDiscountLogInfo")
                    Session("CustOrderDiscountLogInfo") = fn


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
                lblUpMsg.Text = "Please import valid Excel template."
                Me.MPEImport.VisibleOnPageLoad = True
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
        If Not Session("dtCustDiscount") Is Nothing Then
            Dim dtErrors As DataTable
            dtErrors = CType(Session("dtCustDiscount"), DataTable)
            Me.dgvErros.DataSource = dtErrors
            Me.dgvErros.DataBind()
            Me.MPEImport.VisibleOnPageLoad = True
            ClassUpdatePnl1.Update()
        End If
    End Sub

    Protected Sub lbLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbLog.Click
        Try


            If Not Session("CustOrderDiscountLogInfo") Is Nothing Then
                Dim fileValue As String = Session("CustOrderDiscountLogInfo")





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
                    Me.MPEImport.VisibleOnPageLoad = True
                    Exit Sub

                End If

            Else
                lblUpMsg.Text = "There is no log to show."
                'lblMessage.ForeColor = Drawing.Color.Green
                'lblinfo.Text = "Information"
                Me.MPEImport.VisibleOnPageLoad = True
                Exit Sub

            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))

        Finally
        End Try
    End Sub
    Private Sub btnCancelImport_Click(sender As Object, e As EventArgs) Handles btnCancelImport.Click

        MPEImport.VisibleOnPageLoad = False
    End Sub
    Private Sub btndownloadTemp_Click(sender As Object, e As EventArgs) Handles btndownloadTemp.Click
        Dim Filename As String = System.Configuration.ConfigurationManager.AppSettings("ExcelTemplatePath") & "Customer_OrderLevel_Discount.xls"
        Dim TheFile As FileInfo = New FileInfo(Filename)
        If TheFile.Exists Then
            Dim strFileName As String = "Template" + Now.ToString("ddMMMyyHHmmss") + ".xls"


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
    Private Sub DeleteExcelTemplate()
        Try
            Dim Filename As String = ViewState("SampleTemplate")

            Dim TheFile As FileInfo = New FileInfo(Filename)
            If TheFile.Exists Then
                File.Delete(Filename)
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))

        End Try
    End Sub
End Class