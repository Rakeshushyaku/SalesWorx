Imports SalesWorx.BO.Common
Imports System.Data
Imports System.Web
Imports System.Resources
Imports System.Web.UI.WebControls
Imports log4net
Imports System.Text.RegularExpressions
Imports Telerik.Web.UI
Imports ExcelLibrary.SpreadSheet
Imports System.Data.OleDb
Imports System.IO

Imports System.Threading
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports Telerik.Web

Imports Telerik

Partial Public Class ListCustomers
    Inherits System.Web.UI.Page
    Dim objSalesDistrict As New SalesDistrict
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer
    Private dtErrors As New DataTable
    Private dtErrors_ship As New DataTable
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim Dt As New DataTable
    Private Const PageID As String = "P286"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private Shared ENABLE_CREDIT_FOR_CASH_CUSTOMER As String = "N"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
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
                LoadOrganization()
                BindData()
                Dim dt_appctrl As New DataTable
                ObjCustomer = New SalesWorx.BO.Common.Customer
                dt_appctrl = ObjCustomer.CheckEnableCreditforCashCust(Err_No, Err_Desc)
                If dt_appctrl.Rows.Count > 0 Then
                    ENABLE_CREDIT_FOR_CASH_CUSTOMER = dt_appctrl.Rows(0)("Control_Value").ToString()
                Else
                    ENABLE_CREDIT_FOR_CASH_CUSTOMER = "N"

                End If
                Session.Remove("dtCuErrors")
                Session.Remove("dtCuErrors_ship")
                Session.Remove("CuLogFile")
                SetErrorsTable()
                SetErrorsTable_u()
            End If
            ExcelFileUpload.TemporaryFolder = System.Configuration.ConfigurationManager.AppSettings("TemporaryFolder")
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Sub LoadOrganization()
        Try
            ObjCommon = New SalesWorx.BO.Common.Common
            Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
            ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
            ddlOrganization.DataBind()
            ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select organization", "0"))
            If ddlOrganization.Items.Count = 2 Then
                ddlOrganization.SelectedIndex = 1
            End If

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnSearch.Click
        Try
            If ddlOrganization.SelectedItem.Value = "0" Then
                MessageBoxValidation("Select an Organization.", "Validation")
                ClassUpdatePnl.Update()
                Exit Sub
            End If
            BindData()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Private Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click
        Response.Redirect("AdminCustomers.aspx")
    End Sub
    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
            Dim LblCustomer_ID As System.Web.UI.WebControls.Label = row.FindControl("lblCustomer_ID")
            Dim LblSite_ID As System.Web.UI.WebControls.Label = row.FindControl("lblSite_Use_ID")


            Response.Redirect("AdminCustomers.aspx?Mode=E&Customer_ID=" & LblCustomer_ID.Text & "&Site_Use_ID=" & LblSite_ID.Text)
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Protected Sub btnView_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
            Dim LblCustomer_ID As System.Web.UI.WebControls.Label = row.FindControl("lblCustomer_ID")
            Dim LblSite_ID As System.Web.UI.WebControls.Label = row.FindControl("lblSite_Use_ID")


            Response.Redirect("AdminCustomers.aspx?Mode=V&Customer_ID=" & LblCustomer_ID.Text & "&Site_Use_ID=" & LblSite_ID.Text)
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Private Sub gvCurrency_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdCustomer.PageIndexChanging
        Try
            grdCustomer.PageIndex = e.NewPageIndex
            BindData()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Private Sub gvCurrency_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdCustomer.Sorting
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
    Protected Sub lbChangeStatus_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim btnChangeStatus As LinkButton = TryCast(sender, LinkButton)
            Dim row As GridViewRow = DirectCast(btnChangeStatus.NamingContainer, GridViewRow)
            Dim LblCustomer_ID As System.Web.UI.WebControls.Label = row.FindControl("lblCustomer_ID")
            Dim LblSite_ID As System.Web.UI.WebControls.Label = row.FindControl("lblSite_Use_ID")
            ObjCustomer = New SalesWorx.BO.Common.Customer
            Dim LblStatus As System.Web.UI.WebControls.Label = row.FindControl("lblStatus")
            If ObjCustomer.SaveCustomer(Err_No, Err_Desc, 3, LblCustomer_ID.Text, LblSite_ID.Text, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "") Then
                If LblStatus.Text = "Y" Then
                    MessageBoxValidation("Customer disabled Successfully", "Information")
                Else
                    MessageBoxValidation("Customer enabled Successfully", "Information")
                End If
            Else
                MessageBoxValidation("Unexpected error occured. Please contact the administrator", "Information")
            End If
            BindData()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Sub BindData()
        Try
            Dim dtcustomer As New DataTable
            dtcustomer = (New SalesWorx.BO.Common.Customer).GetCustomersFromSWX(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, txtCustomerName.Text, txtCustomerNo.Text)
            Dim dv As New DataView(dtcustomer)
            If Not ViewState("SortField") Is Nothing Then
                If ViewState("SortField").ToString <> "" Then
                    dv.Sort = (ViewState("SortField") & " ") + SortDirection
                End If
            End If
            grdCustomer.DataSource = dv
            grdCustomer.DataBind()
            ClassUpdatePnl.Update()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Private Sub Btn_Clear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_Clear.Click
        Try
            ddlOrganization.ClearSelection()
            txtCustomerName.Text = ""
            txtCustomerNo.Text = ""
            BindData()
            ClassUpdatePnl.Update()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Try


            Dim finalDataSet As New DataSet
            Dim dtProducts As New DataTable()

            If ddlOrganization.SelectedItem.Value = "0" Then
                MessageBoxValidation("Select an Organization.", "Validation")
                ClassUpdatePnl.Update()
                Exit Sub
            End If


            finalDataSet = (New SalesWorx.BO.Common.Customer).LoadExportCustomersTemplate(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, txtCustomerName.Text, txtCustomerNo.Text)
            If finalDataSet.Tables.Count() > 1 Then
                finalDataSet.Tables(0).TableName = "Customer"
                finalDataSet.Tables(1).TableName = "Customer_Ship_Address"
            End If



            Dim fn As String = "Customer_Info" + DateTime.Now.ToString("hhmmss") + ".xls"
            Dim d As New DataSet


            If finalDataSet.Tables.Count <= 0 Then
                MessageBoxValidation("There is no data to export", "Information")
                Exit Sub
            Else
                ExportToExcel(fn, finalDataSet)
            End If

        Catch ex As Exception
            log.Error(ex.Message.ToString())
            MessageBoxValidation("Unexpected error please check log", "Information")
        End Try

    End Sub
    Private Sub ExportToExcel(ByVal strFileName As String, ByVal ds As DataSet)

        Try



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
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            MessageBoxValidation("Unexpected error please check log", "Information")
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
            log.Error(ex.Message.ToString())
            MessageBoxValidation("Unexpected error please check log", "Information")
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
                Dim TotSuccess_u As Integer = 0
                Dim TotFailed_u As Integer = 0
                Try
                    ViewState("FileType") = e.File.ContentType
                   
                    If Not (exten.ToUpper().Trim() = ".XLS" Or exten.ToUpper().Trim() = ".XLSX") Then
                        MessageBoxValidation("Please upload excel file", "Validation")
                        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)

                        Exit Sub

                    End If

                    If e.File.FileName.ToString.ToUpper().Trim().EndsWith(".CSV") Or e.File.FileName.ToString.ToUpper().Trim().EndsWith(".XLS") Or e.File.FileName.ToString.ToUpper().Trim().EndsWith(".XLSX") Then

                        Dim Foldername As String = System.Configuration.ConfigurationManager.AppSettings("TemporaryFolder")
                        ' Dim Foldername As String = ExcelFileUpload.TemporaryFolder

                        If Not Foldername.EndsWith("\") Then
                            Foldername = Foldername & "\"
                        End If
                        If Directory.Exists(Foldername.Substring(0, Len(Foldername) - 1)) = False Then
                            Directory.CreateDirectory(Foldername.Substring(0, Len(Foldername) - 1))
                        End If

                        If e.File.FileName.ToString.ToUpper().Trim().EndsWith(".CSV") Then
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
                                col.ColumnName = "Customer_No"
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
                                col.ColumnName = "Customer_Name"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)


                                col = New DataColumn
                                col.ColumnName = "Contact"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)


                                col = New DataColumn
                                col.ColumnName = "Address"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)


                                col = New DataColumn
                                col.ColumnName = "City"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)


                                col = New DataColumn
                                col.ColumnName = "Phone"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)


                                col = New DataColumn
                                col.ColumnName = "Credit_Limit"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)


                                col = New DataColumn
                                col.ColumnName = "Credit_Hold"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)


                                col = New DataColumn
                                col.ColumnName = "Customer_Type"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)

                                col = New DataColumn
                                col.ColumnName = "Customer_Class"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)



                                col = New DataColumn
                                col.ColumnName = "Cust_Status"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)

                                col = New DataColumn
                                col.ColumnName = "Is_Cash_Cust"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)

                                col = New DataColumn
                                col.ColumnName = "Is_Generic_Cash"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)

                                col = New DataColumn
                                col.ColumnName = "Price_List_Code"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)


                                col = New DataColumn
                                col.ColumnName = "Avail_Bal"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)


                                col = New DataColumn
                                col.ColumnName = "Bill_Credit_Period"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)

                                col = New DataColumn
                                col.ColumnName = "TRN"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)

                                col = New DataColumn
                                col.ColumnName = "Parent"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)

                                col = New DataColumn
                                col.ColumnName = "Collection_Group"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)

                                Dim TempTbl_Ship As New DataTable
                                If TempTbl_Ship.Rows.Count > 0 Then
                                    TempTbl_Ship.Rows.Clear()
                                End If

                                Dim col_Ship As DataColumn



                                col_Ship = New DataColumn
                                col_Ship.ColumnName = "Customer_No"
                                col_Ship.DataType = System.Type.GetType("System.String")
                                col_Ship.ReadOnly = False
                                col_Ship.Unique = False
                                TempTbl_Ship.Columns.Add(col_Ship)


                                col_Ship = New DataColumn
                                col_Ship.ColumnName = "Site_No"
                                col_Ship.DataType = System.Type.GetType("System.String")
                                col_Ship.ReadOnly = False
                                col_Ship.Unique = False
                                TempTbl_Ship.Columns.Add(col_Ship)

                                col_Ship = New DataColumn
                                col_Ship.ColumnName = "Sales_Org_ID"
                                col_Ship.DataType = System.Type.GetType("System.String")
                                col_Ship.ReadOnly = False
                                col_Ship.Unique = False
                                TempTbl_Ship.Columns.Add(col_Ship)


                                col_Ship = New DataColumn
                                col_Ship.ColumnName = "Customer_Name"
                                col_Ship.DataType = System.Type.GetType("System.String")
                                col_Ship.ReadOnly = False
                                col_Ship.Unique = False
                                TempTbl_Ship.Columns.Add(col_Ship)




                                col_Ship = New DataColumn
                                col_Ship.ColumnName = "Location"
                                col_Ship.DataType = System.Type.GetType("System.String")
                                col_Ship.ReadOnly = False
                                col_Ship.Unique = False
                                TempTbl_Ship.Columns.Add(col_Ship)


                                col_Ship = New DataColumn
                                col_Ship.ColumnName = "Address"
                                col_Ship.DataType = System.Type.GetType("System.String")
                                col_Ship.ReadOnly = False
                                col_Ship.Unique = False
                                TempTbl_Ship.Columns.Add(col_Ship)

                                col_Ship = New DataColumn
                                col_Ship.ColumnName = "City"
                                col_Ship.DataType = System.Type.GetType("System.String")
                                col_Ship.ReadOnly = False
                                col_Ship.Unique = False
                                TempTbl_Ship.Columns.Add(col_Ship)


                                col_Ship = New DataColumn
                                col_Ship.ColumnName = "Postal_Code"
                                col_Ship.DataType = System.Type.GetType("System.String")
                                col_Ship.ReadOnly = False
                                col_Ship.Unique = False
                                TempTbl_Ship.Columns.Add(col_Ship)


                                col_Ship = New DataColumn
                                col_Ship.ColumnName = "Cust_Status"
                                col_Ship.DataType = System.Type.GetType("System.String")
                                col_Ship.ReadOnly = False
                                col_Ship.Unique = False
                                TempTbl_Ship.Columns.Add(col_Ship)


                                col_Ship = New DataColumn
                                col_Ship.ColumnName = "Customer_Barcode"
                                col_Ship.DataType = System.Type.GetType("System.String")
                                col_Ship.ReadOnly = False
                                col_Ship.Unique = False
                                TempTbl_Ship.Columns.Add(col_Ship)

                                col_Ship = New DataColumn
                                col_Ship.ColumnName = "Customer_Segment"
                                col_Ship.DataType = System.Type.GetType("System.String")
                                col_Ship.ReadOnly = False
                                col_Ship.Unique = False
                                TempTbl_Ship.Columns.Add(col_Ship)


                                col_Ship = New DataColumn
                                col_Ship.ColumnName = "Sales_District"
                                col_Ship.DataType = System.Type.GetType("System.String")
                                col_Ship.ReadOnly = False
                                col_Ship.Unique = False
                                TempTbl_Ship.Columns.Add(col_Ship)




                                col_Ship = New DataColumn
                                col_Ship.ColumnName = "Cust_Lat"
                                col_Ship.DataType = System.Type.GetType("System.String")
                                col_Ship.ReadOnly = False
                                col_Ship.Unique = False
                                TempTbl_Ship.Columns.Add(col_Ship)


                                col_Ship = New DataColumn
                                col_Ship.ColumnName = "Cust_Long"
                                col_Ship.DataType = System.Type.GetType("System.String")
                                col_Ship.ReadOnly = False
                                col_Ship.Unique = False
                                TempTbl_Ship.Columns.Add(col_Ship)

                                col_Ship = New DataColumn
                                col_Ship.ColumnName = "Beacon_UUID"
                                col_Ship.DataType = System.Type.GetType("System.String")
                                col_Ship.ReadOnly = False
                                col_Ship.Unique = False
                                TempTbl_Ship.Columns.Add(col_Ship)


                                col_Ship = New DataColumn
                                col_Ship.ColumnName = "Beacon_Major"
                                col_Ship.DataType = System.Type.GetType("System.String")
                                col_Ship.ReadOnly = False
                                col_Ship.Unique = False
                                TempTbl_Ship.Columns.Add(col_Ship)

                                col_Ship = New DataColumn
                                col_Ship.ColumnName = "Beacon_Minor"
                                col_Ship.DataType = System.Type.GetType("System.String")
                                col_Ship.ReadOnly = False
                                col_Ship.Unique = False
                                TempTbl_Ship.Columns.Add(col_Ship)


                                If ViewState("FileName").ToString.ToLower.Trim().EndsWith(".csv") Then
                                    TempTbl = DoCSVUpload()
                                ElseIf ViewState("FileName").ToString.ToLower.Trim().EndsWith(".xls") Then
                                    TempTbl = DoXLSUpload()
                                ElseIf ViewState("FileName").ToString.ToLower.Trim().EndsWith(".xlsx") Then
                                    TempTbl = DoXLSXUpload()
                                End If



                                If ViewState("FileName").ToString.ToUpper().Trim().EndsWith(".CSV") Then
                                    TempTbl_Ship = DoCSVUpload_ShipAddress()
                                ElseIf ViewState("FileName").ToString.ToUpper().Trim().EndsWith(".XLS") Then
                                    TempTbl_Ship = DoXLSUpload_ShipAddress()
                                ElseIf ViewState("FileName").ToString.ToUpper().Trim().EndsWith(".XLSX") Then
                                    TempTbl_Ship = DoXLSXUpload_ShipAddress()
                                End If

                                If TempTbl.Rows.Count = 0 Or TempTbl Is Nothing Then
                                    MessageBoxValidation("Invalid file Template", "Validation")
                                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                                    Exit Sub
                                End If


                                If TempTbl.Columns.Count >= 20 Then

                                    If Not (TempTbl.Columns(0).ColumnName = "Customer_No" Or TempTbl.Columns(1).ColumnName = "Sales_Org_ID" Or TempTbl.Columns(2).ColumnName = "Customer_Name" Or TempTbl.Columns(3).ColumnName = "Contact" Or TempTbl.Columns(4).ColumnName = "Address" Or TempTbl.Columns(5).ColumnName = "City" Or TempTbl.Columns(6).ColumnName = "Phone" Or TempTbl.Columns(7).ColumnName = "Credit_Limit" Or TempTbl.Columns(8).ColumnName = "Credit_Hold" Or TempTbl.Columns(9).ColumnName = "Customer_Type" Or TempTbl.Columns(10).ColumnName = "Customer_Class" Or TempTbl.Columns(11).ColumnName = "Cust_Status" Or TempTbl.Columns(12).ColumnName = "Is_Cash_Cust" Or TempTbl.Columns(13).ColumnName = "Is_Generic_Cash" Or TempTbl.Columns(14).ColumnName = "Price_List_Code" Or TempTbl.Columns(15).ColumnName = "Avail_Bal" Or TempTbl.Columns(16).ColumnName = "Bill_Credit_Period" Or TempTbl.Columns(17).ColumnName = "TRN" Or TempTbl.Columns(18).ColumnName = "Parent" Or TempTbl.Columns(19).ColumnName = "Collection_Group") Then

                                        MessageBoxValidation("Please check the template columns are correct", "Validation")
                                        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                                        Exit Sub

                                    End If

                                Else

                                    MessageBoxValidation("The Customer template sheet should be 20 column only", "Validation")
                                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                                    Exit Sub
                                End If




                                If TempTbl.Rows.Count = 0 Then
                                    MessageBoxValidation("There is no data in the file.", "Validation")
                                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)

                                    Exit Sub
                                End If
                                dtErrors = Session("dtCuErrors")

                                If dtErrors.Rows.Count > 0 Then
                                    dtErrors.Rows.Clear()
                                End If
                                Dim RowNo As String = Nothing
                                Dim ErrorText As String = Nothing

                                Dim RowNo_u As String = Nothing
                                Dim ErrorText_u As String = Nothing
                                If TempTbl.Rows.Count > 0 Then
                                    Dim idx As Integer




                                    Dim Customer_No As String = Nothing
                                    Dim Sales_Org_ID As String = Nothing
                                    Dim Customer_Name As String = Nothing
                                    Dim Contact As String = Nothing
                                    Dim Address As String = Nothing
                                    Dim City As String = Nothing
                                    Dim Phone As String = Nothing
                                    Dim Credit_Limit As String = Nothing
                                    Dim Credit_Hold As String = Nothing
                                    Dim Customer_Type As String = Nothing
                                    Dim Customer_Class As String = Nothing
                                    Dim Cust_Status As String = Nothing
                                    Dim Is_Cash_Cust As String = Nothing
                                    Dim Is_Generic_Cash As String = Nothing
                                    Dim Price_List_Code As String = Nothing
                                    Dim Avail_Bal As String = Nothing
                                    Dim Bill_Credit_Period As String = Nothing
                                    Dim TRN As String = Nothing
                                    Dim Parent As String = Nothing
                                    Dim Collection_Group As String = Nothing
                                    Dim price_List_id As String = Nothing

                                    Dim Customer_ID As String = Nothing
                                    Dim Site_use_ID As String = Nothing
                                    Dim opt As String = Nothing

                                    Dim isValidRow As Boolean = True
                                    For idx = 0 To TempTbl.Rows.Count - 1

                                        RowNo = Nothing
                                        ErrorText = Nothing
                                        isValidRow = True

                                        'If TempTbl.Rows(idx)(0) Is DBNull.Value Or TempTbl.Rows(idx)(1) Is DBNull.Value Or TempTbl.Rows(idx)(2) Is DBNull.Value Then
                                        '    Continue For
                                        'End If

                                        Customer_No = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, Nothing, TempTbl.Rows(idx)(0).ToString())
                                        Sales_Org_ID = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, Nothing, TempTbl.Rows(idx)(1).ToString())
                                        Customer_Name = IIf(TempTbl.Rows(idx)(2) Is DBNull.Value, Nothing, TempTbl.Rows(idx)(2).ToString())
                                        Contact = IIf(TempTbl.Rows(idx)(3) Is "", "", TempTbl.Rows(idx)(3).ToString())
                                        Address = IIf(TempTbl.Rows(idx)(4) Is "", "", TempTbl.Rows(idx)(4).ToString())
                                        City = IIf(TempTbl.Rows(idx)(5) Is "", "", TempTbl.Rows(idx)(5).ToString())
                                        Phone = IIf(TempTbl.Rows(idx)(6) Is "", "", TempTbl.Rows(idx)(6).ToString())
                                        Credit_Limit = IIf(TempTbl.Rows(idx)(7) Is "", "0", TempTbl.Rows(idx)(7).ToString())
                                        Credit_Hold = IIf(TempTbl.Rows(idx)(8) Is "", "", TempTbl.Rows(idx)(8).ToString())
                                        Customer_Type = IIf(TempTbl.Rows(idx)(9) Is "", "", TempTbl.Rows(idx)(9).ToString())
                                        Customer_Class = IIf(TempTbl.Rows(idx)(10) Is "", "", TempTbl.Rows(idx)(10).ToString())
                                        Cust_Status = IIf(TempTbl.Rows(idx)(11) Is "", "", TempTbl.Rows(idx)(11).ToString())
                                        Is_Cash_Cust = IIf(TempTbl.Rows(idx)(12) Is "", "", TempTbl.Rows(idx)(12).ToString())
                                        Is_Generic_Cash = IIf(TempTbl.Rows(idx)(13) Is "", "", TempTbl.Rows(idx)(13).ToString())
                                        Price_List_Code = IIf(TempTbl.Rows(idx)(14) Is DBNull.Value, "0", TempTbl.Rows(idx)(14).ToString())
                                        Avail_Bal = IIf(TempTbl.Rows(idx)(15) Is "", "0", TempTbl.Rows(idx)(15).ToString())
                                        Bill_Credit_Period = IIf(TempTbl.Rows(idx)(16) Is "", "0", TempTbl.Rows(idx)(16).ToString())
                                        TRN = IIf(TempTbl.Rows(idx)(17) Is "", "", TempTbl.Rows(idx)(17).ToString())
                                        Parent = IIf(TempTbl.Rows(idx)(18) Is "", "", TempTbl.Rows(idx)(18).ToString())
                                        Collection_Group = IIf(TempTbl.Rows(idx)(19) Is "", "", TempTbl.Rows(idx)(19).ToString())



                                        If Credit_Hold.Trim() = "" Then
                                            Credit_Hold = "N"
                                        End If

                                        If Is_Cash_Cust.Trim() = "" Then
                                            Is_Cash_Cust = "Y"
                                        End If
                                        If Is_Generic_Cash.Trim() = "" Then
                                            Is_Generic_Cash = "N"
                                        End If

                                        If Is_Cash_Cust.Trim().ToUpper = "Y" Then
                                            Bill_Credit_Period = "0"
                                        End If

                                        If Customer_No = "0" Or Customer_No Is Nothing Then
                                            Continue For
                                        End If


                                        'If (New SalesWorx.BO.Common.Customer).CustomerNoExists(Customer_No, Sales_Org_ID) = True Then
                                        '    RowNo = idx + 2
                                        '    ErrorText = "Customer No already exist" + ","
                                        '    isValidRow = False
                                        '    TotFailed += 1
                                        'End If

                                        If Customer_No Is Nothing Or Sales_Org_ID Is Nothing Then
                                            RowNo = idx + 2
                                            ErrorText = "Invalid/Empty Customer_No or  Sales_Org_ID " + ","
                                            isValidRow = False
                                            TotFailed += 1

                                        Else
                                            If Customer_No.Length > 50 Then
                                                RowNo = idx + 2
                                                ErrorText = ErrorText + "Customer No Max lenght  is 50" + ","
                                                isValidRow = False
                                                TotFailed += 1

                                            Else

                                                Dim Dt_customer As New DataTable
                                                Dt_customer = (New SalesWorx.BO.Common.Customer).GetCustomer(Err_No, Err_Desc, Customer_No, Sales_Org_ID)
                                                If Dt_customer.Rows.Count > 0 Then
                                                    Customer_ID = Dt_customer.Rows(0)("Customer_ID").ToString()
                                                    Site_use_ID = Dt_customer.Rows(0)("Site_use_ID").ToString()

                                                    If (Is_Cash_Cust.Trim().ToUpper = "N") Or (Is_Cash_Cust.Trim().ToUpper = "Y" And ENABLE_CREDIT_FOR_CASH_CUSTOMER.ToUpper().Trim() = "Y" And Is_Generic_Cash.Trim().ToUpper = "N") Then
                                                        Credit_Limit = Dt_customer.Rows(0)("Credit_Limit").ToString()
                                                        Avail_Bal = Dt_customer.Rows(0)("Avail_Bal").ToString()
                                                    End If
                                                    opt = 2
                                                Else
                                                    Customer_ID = ""
                                                    Site_use_ID = ""
                                                    opt = 1

                                                    If (Is_Cash_Cust.Trim().ToUpper = "N") Or (Is_Cash_Cust.Trim().ToUpper = "Y" And ENABLE_CREDIT_FOR_CASH_CUSTOMER.ToUpper().Trim() = "Y" And Is_Generic_Cash.Trim().ToUpper = "N") Then

                                                        If Credit_Limit = "0" Or Credit_Limit = "" Then
                                                            RowNo = idx + 2
                                                            ErrorText = ErrorText + "Credit Limit mandatory" + ","
                                                            isValidRow = False
                                                            TotFailed += 1
                                                        End If

                                                        If Avail_Bal = "0" Or Avail_Bal = "" Then
                                                            RowNo = idx + 2
                                                            ErrorText = ErrorText + "Available Balance mandatory" + ","
                                                            isValidRow = False
                                                            TotFailed += 1
                                                        End If

                                                        'If Bill_Credit_Period = "0" Then
                                                        '    RowNo = idx + 2
                                                        '    ErrorText = ErrorText + "Credit Period mandatory" + ","
                                                        '    isValidRow = False
                                                        '    TotFailed += 1
                                                        'End If
                                                        If Not Bill_Credit_Period = "" Then
                                                            If IsNumeric(Bill_Credit_Period) = False Then
                                                                RowNo = idx + 2
                                                                ErrorText = ErrorText + "Invalid Credit Period " + ","
                                                                isValidRow = False
                                                                TotFailed += 1
                                                            End If
                                                        End If
                                                        If Not Credit_Limit = "" Then
                                                            If IsNumeric(Credit_Limit) = False Then
                                                                RowNo = idx + 2
                                                                ErrorText = ErrorText + "Invalid Credit Limit" + ","
                                                                isValidRow = False
                                                                TotFailed += 1
                                                            End If
                                                        End If
                                                        If Not Avail_Bal = "" Then
                                                            If IsNumeric(Avail_Bal) = False Then
                                                                RowNo = idx + 2
                                                                ErrorText = ErrorText + "Invalid  Available Balance" + ","
                                                                isValidRow = False
                                                                TotFailed += 1
                                                            End If
                                                        End If

                                                    End If


                                                End If
                                            End If

                                        End If

                                If Customer_Name Is Nothing Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Customer name is empty" + ","
                                    isValidRow = False
                                    TotFailed += 1
                                End If


                                If Customer_Name = "0" Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Invalid Customer name" + ","
                                    isValidRow = False
                                    TotFailed += 1
                                End If

                                If Not Sales_Org_ID Is Nothing Then

                                    Dim Sales_Org_ID_test As Integer
                                    If Integer.TryParse(Sales_Org_ID, Sales_Org_ID_test) Then
                                        If (New SalesWorx.BO.Common.Customer).IsValidOrganization(Sales_Org_ID) = False Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "Invalid Organization" + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End If
                                    Else
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Invalid Organization" + ","
                                        isValidRow = False
                                        TotFailed += 1
                                    End If


                                End If

                                If Price_List_Code = "0" Or Price_List_Code.Trim() = "" Or Price_List_Code Is Nothing Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Invalid Price List Code" + ","
                                    isValidRow = False
                                    TotFailed += 1
                                End If
                                If Customer_Type <> "" Then
                                    If (New SalesWorx.BO.Common.Customer).IsValidCustomerType(Customer_Type) = False Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Invalid Customer Type" + ","
                                        isValidRow = False
                                        TotFailed += 1
                                    End If
                                End If
                                If Customer_Class <> "" Then
                                    If (New SalesWorx.BO.Common.Customer).IsValidCustomerClass(Customer_Class) = False Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Invalid Customer Class" + ","
                                        isValidRow = False
                                        TotFailed += 1
                                    End If

                                End If








                                If Not Price_List_Code = "0" And Not Price_List_Code.Trim() = "" And Not Price_List_Code Is Nothing Then

                                    Dim PriceList_Dt As New DataTable

                                    PriceList_Dt = (New SalesWorx.BO.Common.Customer).GetCustomerPriceList_ID(Err_No, Err_Desc, Price_List_Code)
                                    If PriceList_Dt.Rows.Count > 0 Then
                                        price_List_id = PriceList_Dt.Rows(0)("Price_List_ID").ToString()
                                    Else
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Invalid Price List Code" + ","
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



                                    If (New SalesWorx.BO.Common.Customer).SaveCustomer(Err_No, Err_Desc, opt, Customer_ID, Site_use_ID, Customer_Name, Customer_No, Contact, Parent, Address, City, Phone, Is_Cash_Cust, Bill_Credit_Period, Credit_Limit, Avail_Bal, Credit_Hold, price_List_id, Sales_Org_ID, Customer_Type, Customer_Class, Collection_Group, Is_Generic_Cash, TRN) Then

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
                                        h("LogInfo") = Err_Desc + "Error occured while saving this row"
                                        dtErrors.Rows.Add(h)
                                        RowNo = Nothing
                                        ErrorText = Nothing
                                        isValidRow = True
                                    End If
                                End If

                                    Next
                                End If


                                '''''''''''''''''''''Customer ShipAddress'''''''''''''''''''''''''''''''

                                dtErrors_ship = Session("dtCuErrors_ship")

                                If dtErrors_ship.Rows.Count > 0 Then
                                    dtErrors_ship.Rows.Clear()
                                End If





                                If Not (TempTbl_Ship.Columns(0).ColumnName = "Customer_No" Or TempTbl_Ship.Columns(1).ColumnName = "Site_No" Or TempTbl_Ship.Columns(2).ColumnName = "Sales_Org_ID" Or TempTbl_Ship.Columns(3).ColumnName = "Customer_Name" Or TempTbl_Ship.Columns(4).ColumnName = "Location" Or TempTbl_Ship.Columns(4).ColumnName = "Address" Or TempTbl_Ship.Columns(6).ColumnName = "City" Or TempTbl_Ship.Columns(7).ColumnName = "Postal_Code" Or TempTbl_Ship.Columns(8).ColumnName = "Cust_Status" Or TempTbl_Ship.Columns(9).ColumnName = "Customer_Barcode" Or TempTbl_Ship.Columns(10).ColumnName = "Customer_Segment" Or TempTbl_Ship.Columns(11).ColumnName = "Sales_District" Or TempTbl_Ship.Columns(12).ColumnName = "Cust_Lat" Or TempTbl_Ship.Columns(13).ColumnName = "Cust_Long" Or TempTbl_Ship.Columns(14).ColumnName = "Beacon_UUID" Or TempTbl_Ship.Columns(15).ColumnName = "Beacon_Major" Or TempTbl_Ship.Columns(16).ColumnName = "Beacon_Minor") Then
                                    MessageBoxValidation("Please check the template columns are correct", "Validation")
                                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                                    Exit Sub

                                End If

                                ' If TempTbl_Ship.Rows.Count = 0 Then
                                'MessageBoxValidation("There is no Customer ShipAddress data in the file.", "Validation")
                                ' ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)

                                ' Exit Sub
                                'End If

                                If TempTbl_Ship.Rows.Count > 0 Then


                                    Dim idx_u As Integer



                                    Dim Customer_No As String = Nothing
                                    Dim Site_No As String = Nothing
                                    Dim Sales_Org_ID As String = Nothing
                                    Dim Customer_Name As String = Nothing
                                    Dim Location As String = Nothing
                                    Dim Address As String = Nothing
                                    Dim City As String = Nothing
                                    Dim Postal_Code As String = Nothing
                                    Dim Cust_Status As String = Nothing
                                    Dim Customer_Barcode As String = Nothing
                                    Dim Customer_Segment As String = Nothing
                                    Dim Sales_District As String = Nothing
                                    Dim Cust_Lat As String = Nothing
                                    Dim Cust_Long As String = Nothing
                                    Dim Beacon_UUID As String = Nothing
                                    Dim Beacon_Major As String = Nothing
                                    Dim Beacon_Minor As String = Nothing

                                    Dim Sales_District_ID As String = Nothing
                                    Dim Customer_Segment_ID As String = Nothing
                                    Dim Site_Use_ID As String = Nothing
                                    Dim Customer_ID As String = Nothing
                                    Dim Opt As String = Nothing
                                    Dim dt_org As New DataTable

                                    Dim isValidRow_u As Boolean = True
                                    For idx_u = 0 To TempTbl_Ship.Rows.Count - 1

                                        RowNo_u = Nothing
                                        ErrorText_u = Nothing
                                        isValidRow_u = True



                                        'If TempTbl_Ship.Rows(idx_u)(0) Is DBNull.Value Or TempTbl_Ship.Rows(idx_u)(2) Is DBNull.Value Or TempTbl_Ship.Rows(idx_u)(3) Is DBNull.Value Then
                                        '    Continue For
                                        'End If



                                        Customer_No = IIf(TempTbl_Ship.Rows(idx_u)(0) Is DBNull.Value, Nothing, TempTbl_Ship.Rows(idx_u)(0).ToString())
                                        Site_No = IIf(TempTbl_Ship.Rows(idx_u)(1) Is DBNull.Value, Nothing, TempTbl_Ship.Rows(idx_u)(1).ToString())
                                        Sales_Org_ID = IIf(TempTbl_Ship.Rows(idx_u)(2) Is DBNull.Value, Nothing, TempTbl_Ship.Rows(idx_u)(2).ToString())
                                        Customer_Name = IIf(TempTbl_Ship.Rows(idx_u)(3) Is DBNull.Value, Nothing, TempTbl_Ship.Rows(idx_u)(3).ToString())

                                        Location = IIf(TempTbl_Ship.Rows(idx_u)(4) Is "", "", TempTbl_Ship.Rows(idx_u)(4).ToString())
                                        Address = IIf(TempTbl_Ship.Rows(idx_u)(5) Is "", "", TempTbl_Ship.Rows(idx_u)(5).ToString())
                                        City = IIf(TempTbl_Ship.Rows(idx_u)(6) Is "", "", TempTbl_Ship.Rows(idx_u)(6).ToString())
                                        Postal_Code = IIf(TempTbl_Ship.Rows(idx_u)(7) Is "", "", TempTbl_Ship.Rows(idx_u)(7).ToString())
                                        Cust_Status = IIf(TempTbl_Ship.Rows(idx_u)(8) Is "", "", TempTbl_Ship.Rows(idx_u)(8).ToString())
                                        Customer_Barcode = IIf(TempTbl_Ship.Rows(idx_u)(9) Is "", "0", TempTbl_Ship.Rows(idx_u)(9).ToString())
                                        Customer_Segment = IIf(TempTbl_Ship.Rows(idx_u)(10) Is "", "", TempTbl_Ship.Rows(idx_u)(10).ToString())
                                        Sales_District = IIf(TempTbl_Ship.Rows(idx_u)(11) Is "", "", TempTbl_Ship.Rows(idx_u)(11).ToString())
                                        Cust_Lat = IIf(TempTbl_Ship.Rows(idx_u)(12) Is "", "0", TempTbl_Ship.Rows(idx_u)(12).ToString())
                                        Cust_Long = IIf(TempTbl_Ship.Rows(idx_u)(13) Is "", "0", TempTbl_Ship.Rows(idx_u)(13).ToString())
                                        Beacon_UUID = IIf(TempTbl_Ship.Rows(idx_u)(14) Is "", "", TempTbl_Ship.Rows(idx_u)(14).ToString())
                                        Beacon_Major = IIf(TempTbl_Ship.Rows(idx_u)(15) Is "", "0", TempTbl_Ship.Rows(idx_u)(15).ToString())
                                        Beacon_Minor = IIf(TempTbl_Ship.Rows(idx_u)(16) Is "", "0", TempTbl_Ship.Rows(idx_u)(16).ToString())



                                        If Customer_No = "0" Or Customer_No Is Nothing Then
                                            Continue For
                                        End If


                                        Opt = 1

                                        If Customer_No Is Nothing Or Sales_Org_ID Is Nothing Or Customer_Name Is Nothing Then
                                            RowNo_u = idx_u + 2
                                            ErrorText_u = "Customer No /Sales_Org_ID / Customer Name is  empty " + ","
                                            isValidRow_u = False
                                            TotFailed_u += 1
                                        Else

                                            Dim Customer_Dt As New DataTable
                                            Customer_Dt = (New SalesWorx.BO.Common.Customer).GetCustomer(Err_No, Err_Desc, Customer_No, Sales_Org_ID)
                                            If Customer_Dt.Rows.Count > 0 Then
                                                Customer_ID = Customer_Dt.Rows(0)("Customer_ID").ToString()

                                                Dim CustomerShip_Dt As New DataTable
                                                CustomerShip_Dt = (New SalesWorx.BO.Common.Customer).ExistsCustomerShipAddress(Err_No, Err_Desc, Customer_No, Sales_Org_ID, Customer_Name)
                                                If CustomerShip_Dt.Rows.Count > 0 Then
                                                    Opt = 2
                                                    Site_Use_ID = CustomerShip_Dt.Rows(0)("Site_Use_ID").ToString
                                                Else
                                                    Opt = 1

                                                End If
                                            Else
                                                RowNo_u = idx_u + 2
                                                ErrorText_u = ErrorText_u + "Invalid  Customer No/Org ID " + ","
                                                isValidRow_u = False
                                                TotFailed_u += 1
                                            End If



                                        End If

                                       

                                        If Customer_Name = "0" Then
                                            RowNo_u = idx_u + 2
                                            ErrorText_u = ErrorText_u + " Invalid Customer name" + ","
                                            isValidRow_u = False
                                            TotFailed_u += 1
                                        End If

                                        If Sales_District <> "" And Not Sales_District Is Nothing Then

                                            Dim Sales_District_Dt As New DataTable
                                            Sales_District_Dt = (New SalesWorx.BO.Common.Customer).GetCustomerSales_District_ID(Err_No, Err_Desc, Sales_District)
                                            If Sales_District_Dt.Rows.Count > 0 Then
                                                Sales_District_ID = Sales_District_Dt.Rows(0)("Sales_District_ID").ToString()
                                            Else
                                                RowNo_u = idx_u + 2
                                                ErrorText_u = ErrorText_u + "Invalid Sales District Code" + ","
                                                isValidRow_u = False
                                                TotFailed_u += 1
                                            End If
                                            'Else
                                            '    RowNo_u = idx_u + 2
                                            '    ErrorText_u = ErrorText_u + "Invalid/Empty Sales_District " + ","
                                            '    isValidRow_u = False
                                            '    TotFailed_u += 1
                                        End If

                                        If Customer_Segment <> "" And Not Customer_Segment Is Nothing Then

                                            Dim Segment_Dt As New DataTable
                                            Segment_Dt = (New SalesWorx.BO.Common.Customer).GetCustomerSegment_ID(Err_No, Err_Desc, Customer_Segment)
                                            If Segment_Dt.Rows.Count > 0 Then
                                                Customer_Segment_ID = Segment_Dt.Rows(0)("Customer_Segment_ID").ToString()
                                            Else
                                                RowNo_u = idx_u + 2
                                                ErrorText_u = ErrorText_u + "Invalid Segment Code" + ","
                                                isValidRow_u = False
                                                TotFailed_u += 1
                                            End If

                                            'Else
                                            '    RowNo_u = idx_u + 2
                                            '    ErrorText_u = ErrorText_u + "Invalid Segment Code" + ","
                                            '    isValidRow_u = False
                                            '    TotFailed_u += 1

                                        End If


                                        If Val(Cust_Lat) <> 0 Then
                                            If Not IsNumeric(Cust_Lat) Then
                                                RowNo_u = idx_u + 2
                                                ErrorText_u = ErrorText_u + "Invalid Latitude " + ","
                                                isValidRow_u = False
                                                TotFailed_u += 1
                                            End If
                                        End If
                                        If Val(Cust_Long) <> 0 Then
                                            If Not IsNumeric(Cust_Long) Then
                                                RowNo_u = idx_u + 2
                                                ErrorText_u = ErrorText_u + "Invalid Longitude " + ","
                                                isValidRow_u = False
                                                TotFailed_u += 1
                                            End If
                                        End If
                                        If Not (RowNo_u Is Nothing And ErrorText_u Is Nothing) Then
                                            Dim h_u As DataRow = dtErrors_ship.NewRow()
                                            h_u("RowNo") = RowNo_u

                                            h_u("LogInfo") = ErrorText_u
                                            dtErrors_ship.Rows.Add(h_u)
                                            RowNo_u = Nothing
                                            ErrorText_u = Nothing
                                            isValidRow_u = False
                                        End If


                                        If isValidRow_u = True Then

                                            If (New SalesWorx.BO.Common.Customer).SaveCustomerShipAddress(Err_No, Err_Desc, Opt, Customer_ID, Site_Use_ID, Customer_Name, Customer_No, Address, Postal_Code, City, Customer_Segment_ID, Cust_Lat, Cust_Long, Cust_Status, Sales_Org_ID, Sales_District_ID, "", Location, Customer_Barcode, Beacon_UUID, Beacon_Major, Beacon_Minor) Then

                                                TotSuccess_u = TotSuccess_u + 1
                                                Dim h_u As DataRow = dtErrors_ship.NewRow()
                                                h_u("RowNo") = idx_u + 2
                                                h_u("LogInfo") = "Successfully uploaded"
                                                dtErrors_ship.Rows.Add(h_u)
                                                RowNo_u = Nothing
                                                ErrorText_u = Nothing
                                                isValidRow_u = True
                                            Else
                                                Dim h_u As DataRow = dtErrors_ship.NewRow()
                                                h_u("RowNo") = idx_u + 2
                                                h_u("LogInfo") = Err_Desc
                                                dtErrors_ship.Rows.Add(h_u)
                                                RowNo_u = Nothing
                                                ErrorText_u = Nothing
                                                isValidRow_u = True
                                            End If
                                        End If





                                    Next
                                End If
                                ''''''''''''''''''''''''''''''''''''''''''''''''''



                                '   ResetFields()
                                BindData()


                                Session.Remove("dtCuErrors")
                                Session("dtCuErrors") = dtErrors.Copy


                                Session.Remove("dtCuErrors_ship")
                                Session("dtCuErrors_ship") = dtErrors_ship.Copy

                                Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & "CustomerLog" & Now.ToString("yyyyMMdd") + ".txt"

                                ' DataTable2CSV(dtErrors, fn, vbTab)


                                dtErrors.TableName = "Customer"
                                dtErrors_ship.TableName = "Customer_Ship_Address"


                                Dim Ds_error As New DataSet
                                Ds_error.Tables.Clear()
                                Ds_error.Tables.Add(dtErrors)
                                Ds_error.Tables.Add(dtErrors_ship)
                                DataTable2CSV(Ds_error, fn, vbTab)

                                Session.Remove("CuLogFile")
                                Session("CuLogFile") = fn



                                If TotSuccess > 0 Then
                                    DeleteExcel()
                                    BindData()
                                    Dim lblinfo As String = "Information"
                                    Dim lblMessage As String = IIf((TotFailed = 0 And TotFailed_u = 0), "Successfully uploaded", "One or more rows has invalid data. Please check the log")

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
                    log.Error(ex.Message.ToString())
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
                    log.Error(ex.Message.ToString())
                End Try
            End If


        Catch ex As Exception
            MessageBoxValidation("Error occured while uploading file", "Validation")
            log.Error(ex.Message.ToString())
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
            log.Error(ex.Message.ToString())
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

                Dim cmd As New OleDbCommand("SELECT * FROM [Customer$] WHERE [Customer_No] IS NOT NULL", oledbConn)

                Dim oleda As New OleDbDataAdapter()

                oleda.SelectCommand = cmd



                oleda.Fill(dtImport)



            Catch ex As Exception

                log.Error(GetExceptionInfo(ex))
                log.Error(ex.Message.ToString())
            Finally
                ' Close connection
                oledbConn.Close()
            End Try
        Catch ex As Exception

            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
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

                Dim cmd As New OleDbCommand("SELECT * FROM [Customer$] WHERE [Customer_No] IS NOT NULL", oledbConn)

                Dim oleda As New OleDbDataAdapter()

                oleda.SelectCommand = cmd



                oleda.Fill(dtImport)



            Catch ex As Exception

                log.Error(GetExceptionInfo(ex))
                log.Error(ex.Message.ToString())
            Finally
                ' Close connection
                oledbConn.Close()
            End Try
        Catch ex As Exception

            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
            Throw ex
        End Try
        Return dtImport
    End Function



    Private Function DoCSVUpload_ShipAddress() As DataTable
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
            log.Error(ex.Message.ToString())
            Throw ex
        End Try
        Return dtImport
    End Function

    Private Function DoXLSUpload_ShipAddress() As DataTable
        Dim dtImport As New DataTable
        Try
            Dim connString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & ViewState("FileName") & " ;Extended Properties=""Excel 8.0;HDR=Yes;IMEX=1"""
            Dim oledbConn As New OleDbConnection(connString)
            Try
                oledbConn.Open()

                Dim cmd As New OleDbCommand("SELECT * FROM [Customer_Ship_Address$] WHERE [Customer_No] IS NOT NULL", oledbConn)

                Dim oleda As New OleDbDataAdapter()

                oleda.SelectCommand = cmd



                oleda.Fill(dtImport)



            Catch ex As Exception

                log.Error(GetExceptionInfo(ex))
                log.Error(ex.Message.ToString())
            Finally
                ' Close connection
                oledbConn.Close()
            End Try
        Catch ex As Exception

            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
            Throw ex
        End Try
        Return dtImport
    End Function
    Private Function DoXLSXUpload_ShipAddress() As DataTable
        Dim dtImport As New DataTable
        Try
            Dim connString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & ViewState("FileName") & " ;Extended Properties=""Excel 12.0;HDR=Yes;IMEX=1"""
            Dim oledbConn As New OleDbConnection(connString)
            Try
                oledbConn.Open()

                Dim cmd As New OleDbCommand("SELECT * FROM [Customer_Ship_Address$] WHERE [Customer_No] IS NOT NULL", oledbConn)

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
            log.Error(ex.Message.ToString())
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
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Sub DataTable2CSV(ByVal Ds As DataSet, ByVal filename As String, ByVal sepChar As String)
        Dim writer As System.IO.StreamWriter
        Try
            writer = New System.IO.StreamWriter(filename)

            For Each table As DataTable In Ds.Tables

                writer.WriteLine("-------------------" + table.TableName + "-------------------------")

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

        Session.Remove("dtCuErrors")
        Session("dtCuErrors") = dtErrors
        Return dtErrors
    End Function

    Private Function SetErrorsTable_u() As DataTable
        Dim col As DataColumn

        col = New DataColumn()
        col.ColumnName = "RowNo"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtErrors_ship.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "LogInfo"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtErrors_ship.Columns.Add(col)

        Session.Remove("dtCuErrors_ship")
        Session("dtCuErrors_ship") = dtErrors_ship
        Return dtErrors_ship
    End Function


    Protected Sub lbCreditLimit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim btnChangeStatus As LinkButton = TryCast(sender, LinkButton)
            Dim row As GridViewRow = DirectCast(btnChangeStatus.NamingContainer, GridViewRow)
            Dim LblCustomer_ID As System.Web.UI.WebControls.Label = row.FindControl("lblCustomer_ID")
            Dim LblSite_ID As System.Web.UI.WebControls.Label = row.FindControl("lblSite_Use_ID")
            ObjCustomer = New SalesWorx.BO.Common.Customer
            Dim LblStatus As System.Web.UI.WebControls.Label = row.FindControl("lblStatus")

            '  Dim queryString As String = "AdminCustomersCreditLimit.aspx"
            ' Dim newWin As String = "window.open('" & queryString & "');"
            ' ClientScript.RegisterStartupScript(Me.GetType(), "pop", newWin, True)

            Response.Redirect("AdminCustomersCreditLimit.aspx?Customer_ID=" & LblCustomer_ID.Text & "&Site_Use_ID=" & LblSite_ID.Text)

            
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Protected Sub btndownloadTemplate_Click(sender As Object, e As EventArgs) Handles btndownloadTemplate.Click
        Try

      
        Dim Filename As String = System.Configuration.ConfigurationManager.AppSettings("ExcelTemplatePath") & "Customer_Info.xls"
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
            log.Error(GetExceptionInfo(ex))
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
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Protected Sub lbLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbLog.Click
        Try
            If Not Session("CuLogFile") Is Nothing Then
                Dim fileValue As String = Session("CuLogFile")

                Dim file As System.IO.FileInfo = New FileInfo(fileValue)

                If file.Exists Then

                    'Process.Start("notepad.exe", fileValue)
                    Response.Clear()

                    Response.AddHeader("Content-Disposition", "attachment; filename=" & file.Name)

                    Response.AddHeader("Content-Length", file.Length.ToString())

                    Response.WriteFile(file.FullName)

                    Response.End()

                Else
                    MessageBoxValidation("File does not exist", "Information")
                    Exit Sub

                End If
            Else
                MessageBoxValidation("There is no log to view.", "Information")
                Exit Sub

            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
End Class