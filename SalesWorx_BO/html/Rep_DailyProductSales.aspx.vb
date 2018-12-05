Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports System.Collections.Generic
Imports Telerik.Web.UI
Imports OfficeOpenXml
Public Class Rep_DailyProductSales
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "ArticleMovements"

    Private Const PageID As String = "P405"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single
    Dim dtProduct As New DataTable
    Dim dtCust As New DataTable
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        If Not IsPostBack Then
            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            If Not HasPermission Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If
            ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
            Try
                dtProduct.Columns.Add("ID")
                dtProduct.Columns.Add("Desc")
                'ViewState("DtItem") = dtItem
                ViewState("dtProduct") = dtProduct

                dtCust.Columns.Add("CustomerID")
                dtCust.Columns.Add("Customer")
                'ViewState("DtItem") = dtItem
                ViewState("dtCust") = dtCust

                ObjCommon = New SalesWorx.BO.Common.Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))
                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                End If

                If Not Request.QueryString("ID") Is Nothing Then
                    txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                    txtToDate.SelectedDate = Now()

                    Dim dt As New DataTable
                    dt = (New SalesWorx.BO.Common.Common).GetSalesOrgbyFsr(Err_No, Err_Desc, Request.QueryString("ID"))
                    If dt.Rows.Count > 0 Then
                        If Not ddlOrganization.FindItemByValue(dt.Rows(0)("MAS_Org_ID").ToString) Is Nothing Then
                            ddlOrganization.ClearSelection()
                            ddlOrganization.FindItemByValue(dt.Rows(0)("MAS_Org_ID").ToString).Selected = True
                        End If
                    End If
                Else
                    txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                    txtToDate.SelectedDate = Now()
                End If

                LoadOrgDetails()
                If Not ddlVan.Items.FindItemByValue(Request.QueryString("ID")) Is Nothing Then
                    ddlVan.ClearSelection()
                    ddlVan.Items.FindItemByValue(Request.QueryString("ID")).Selected = True
                End If
                If Not Err_Desc Is Nothing Then
                    log.Error(Err_Desc)
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
                End If
            Catch ex As Exception
                Err_No = "74066"
                If Err_Desc Is Nothing Then
                    log.Error(GetExceptionInfo(ex))
                Else
                    log.Error(Err_Desc)
                End If
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
            Finally
                ObjCommon = Nothing
                ErrorResource = Nothing
            End Try
        Else
            LoadProdcut()
            LoadCustomer()
        End If

    End Sub
    Sub LoadCustomer()
        ObjCommon = New SalesWorx.BO.Common.Common()
        Dim dt As New DataTable

        dt = ObjCommon.GetCustomerfromOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue)
        ddl_Customer.DataSource = dt
        ddl_Customer.DataTextField = "Customer"
        ddl_Customer.DataValueField = "CustomerID"
        ddl_Customer.DataBind()
    End Sub

    Sub LoadProdcut()
        ObjCommon = New SalesWorx.BO.Common.Common()
        Dim dt As New DataTable
        Dim Agency As String = ""
        Dim Category As String = ""

        If ddl_Agency.CheckedItems.Count >= 1 Then
            If ddl_Agency.CheckedItems.Count = ddl_Agency.Items.Count Then
                Agency = "0"
            Else

                For Each itm As RadComboBoxItem In ddl_Agency.CheckedItems
                    Agency = Agency + "'" + itm.Value + "',"
                Next
            End If
        End If

        If ddl_Category.CheckedItems.Count >= 1 Then
            If ddl_Category.CheckedItems.Count = ddl_Category.Items.Count Then
                Category = "0"
            Else
                Category = ""
                For Each itm As RadComboBoxItem In ddl_Category.CheckedItems
                    Category = Category + "'" + itm.Value + "',"
                Next
            End If
        End If
        If Agency <> "0" And Agency <> "" Then
            Agency = Agency.Substring(0, Agency.Length - 1)

        End If
        If Category <> "0" And Category <> "" Then
            Category = Category.Substring(0, Category.Length - 1)

        End If
        dt = ObjCommon.GetProductsByOrgFromAgencyCategory(Err_No, Err_Desc, ddlOrganization.SelectedValue, Agency, Category)
        ddl_Product.DataSource = dt
        ddl_Product.DataTextField = "Description"
        ddl_Product.DataValueField = "Inventory_Item_ID"
        ddl_Product.DataBind()
    End Sub
    Protected Sub ddl_Customer_EntryAdded(sender As Object, e As AutoCompleteEntryEventArgs) Handles ddl_Customer.EntryAdded
        dtCust = CType(ViewState("dtCust"), DataTable)
        Dim seldr() As DataRow
        seldr = dtCust.Select("CustomerID='" & e.Entry.Value & "'")
        If seldr.Length <= 0 Then
            Dim dr As DataRow
            dr = dtCust.NewRow()
            dr(0) = e.Entry.Value
            dr(1) = e.Entry.Text
            dtCust.Rows.Add(dr)
        End If
        ViewState("dtCust") = dtCust
        gvRep.Visible = False
        Args.Visible = False
    End Sub
    Protected Sub ddl_Customer_EntryRemoved(sender As Object, e As AutoCompleteEntryEventArgs) Handles ddl_Customer.EntryRemoved
        dtCust = CType(ViewState("dtCust"), DataTable)
        Dim dr() As DataRow
        dr = dtCust.Select("CustomerID='" & e.Entry.Value & "'")
        If dr.Length > 0 Then
            dtCust.Rows.Remove(dr(0))
        End If
        ViewState("dtCust") = dtCust
        gvRep.Visible = False
        Args.Visible = False
    End Sub
    Protected Sub ddl_Product_EntryAdded(sender As Object, e As AutoCompleteEntryEventArgs) Handles ddl_Product.EntryAdded
        dtProduct = CType(ViewState("dtProduct"), DataTable)
        Dim seldr() As DataRow
        seldr = dtProduct.Select("ID='" & e.Entry.Value & "'")
        If seldr.Length <= 0 Then
            Dim dr As DataRow
            dr = dtProduct.NewRow()
            dr(0) = e.Entry.Value
            dr(1) = e.Entry.Text
            dtProduct.Rows.Add(dr)
        End If
        ViewState("dtProduct") = dtProduct
        gvRep.Visible = False
        Args.Visible = False
    End Sub
    Protected Sub ddl_Product_EntryRemoved(sender As Object, e As AutoCompleteEntryEventArgs) Handles ddl_Product.EntryRemoved
        dtProduct = CType(ViewState("dtProduct"), DataTable)
        Dim dr() As DataRow
        dr = dtProduct.Select("ID='" & e.Entry.Value & "'")
        If dr.Length > 0 Then
            dtProduct.Rows.Remove(dr(0))
        End If
        ViewState("dtProduct") = dtProduct
        gvRep.Visible = False
        Args.Visible = False
    End Sub

    Private Sub BindReport()
        Try

        
        If Not ddlOrganization.SelectedItem Is Nothing Then

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

            Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems
            Dim vantxt As String = ""
            Dim van As String = ""
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","
                vantxt = vantxt & li.Text & ","
            Next
            If vantxt <> "" Then
                vantxt = vantxt.Substring(0, vantxt.Length - 1)
            End If
            If van = "" Then
                van = "0"
                End If

            lbl_org.Text = ddlOrganization.SelectedItem.Text
            lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_van.Text = vantxt

            dtProduct = CType(ViewState("dtProduct"), DataTable)
                Dim Invid As String = ""
            If dtProduct.Rows.Count > 0 Then
                For Each dr In dtProduct.Rows

                    Invid = Invid & dr("ID").ToString & ","
                    lbl_Product.Text = lbl_Product.Text & dr("Desc").ToString & ","
                Next
                Invid = Invid.Substring(0, Invid.Length - 1)

                lbl_Product.Text = lbl_Product.Text.Substring(0, lbl_Product.Text.Length - 1)
            Else
                Invid = "-1"
                lbl_Product.Text = "All"
            End If


            dtCust = CType(ViewState("dtCust"), DataTable)
                Dim CustID As String = ""
            If dtCust.Rows.Count > 0 Then
                For Each dr In dtCust.Rows

                    CustID = CustID & dr("CustomerID").ToString & ","
                    lbl_Customer.Text = lbl_Customer.Text & dr("Customer").ToString & ","
                Next
                CustID = CustID.Substring(0, CustID.Length - 1)

                lbl_Customer.Text = lbl_Customer.Text.Substring(0, lbl_Customer.Text.Length - 1)
            Else
                CustID = "-1"
                lbl_Customer.Text = "All"
            End If

            Dim Agency As String = "-1"
            Dim Category As String = "-1"

            If ddl_Agency.CheckedItems.Count >= 1 Then
                If ddl_Agency.CheckedItems.Count = ddl_Agency.Items.Count Then
                    Agency = "-1"
                    Else
                        Agency = ""
                        For Each itm As RadComboBoxItem In ddl_Agency.CheckedItems
                            Agency = Agency + itm.Value + ","
                        Next
                End If
            End If
                log.Error("6")
            If ddl_Category.CheckedItems.Count >= 1 Then
                If ddl_Category.CheckedItems.Count = ddl_Category.Items.Count Then
                    Category = "-1"
                    Else
                        Category = ""
                        For Each itm As RadComboBoxItem In ddl_Category.CheckedItems
                            Category = Category + itm.Value + ","
                        Next
                End If
            End If
                log.Error("7")
            lbl_Agency.Text = Agency
            lbl_Category.Text = Category

            Args.Visible = True

            Dim dtcurrency As DataTable
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

            Dim Currency As String = ""
            If dtcurrency.Rows.Count > 0 Then
                hfDecimal.Value = "N" & dtcurrency.Rows(0)("Decimal_Digits")
                Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
            End If

                'log.Debug(CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"))
                'log.Debug(CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))
                'log.Debug(ddlOrganization.SelectedItem.Value)
                'log.Debug(van)
                'log.Debug(Agency)
                'log.Debug(Category)
                'log.Debug(Invid)
                'log.Debug(CustID)
                'log.Debug(objUserAccess.UserID)

            Dim dt As New DataTable
            dt = ObjReport.GetDailyProductSales(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, van, objUserAccess.UserID, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"), Agency, Category, Invid, CustID)
            gvRep.DataSource = dt
                gvRep.DataBind()

            End If
        Catch ex As Exception
            log.Error(ex.ToString)
        End Try
    End Sub

    Private Sub BtnExportBiffExcel_Click(sender As Object, e As EventArgs) Handles BtnExportBiffExcel.Click

        If Not ddlOrganization.SelectedItem Is Nothing Then

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

            Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems
            Dim vantxt As String = ""
            Dim van As String = ""
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","
                vantxt = vantxt & li.Text & ","
            Next
            If vantxt <> "" Then
                vantxt = vantxt.Substring(0, vantxt.Length - 1)
            End If
            If van = "" Then
                van = "0"
            End If

            lbl_org.Text = ddlOrganization.SelectedItem.Text
            lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_van.Text = vantxt

            dtProduct = CType(ViewState("dtProduct"), DataTable)
            Dim Invid As String = ""
            If dtProduct.Rows.Count > 0 Then
                For Each dr In dtProduct.Rows

                    Invid = Invid & dr("ID").ToString & ","
                    lbl_Product.Text = lbl_Product.Text & dr("Desc").ToString & ","
                Next
                Invid = Invid.Substring(0, Invid.Length - 1)

                lbl_Product.Text = lbl_Product.Text.Substring(0, lbl_Product.Text.Length - 1)
            Else
                Invid = "-1"
                lbl_Product.Text = "All"
            End If


            dtCust = CType(ViewState("dtCust"), DataTable)
            Dim CustID As String = ""
            If dtCust.Rows.Count > 0 Then
                For Each dr In dtCust.Rows

                    CustID = CustID & dr("CustomerID").ToString & ","
                    lbl_Customer.Text = lbl_Customer.Text & dr("Customer").ToString & ","
                Next
                CustID = CustID.Substring(0, CustID.Length - 1)

                lbl_Customer.Text = lbl_Customer.Text.Substring(0, lbl_Customer.Text.Length - 1)
            Else
                CustID = "-1"
                lbl_Customer.Text = "All"
            End If

            Dim Agency As String = "-1"
            Dim Category As String = "-1"

            If ddl_Agency.CheckedItems.Count >= 1 Then
                If ddl_Agency.CheckedItems.Count = ddl_Agency.Items.Count Then
                    Agency = "-1"
                Else
                    Agency = ""
                    For Each itm As RadComboBoxItem In ddl_Agency.CheckedItems
                        Agency = Agency + itm.Value + ","
                    Next
                End If
            End If
            log.Error("6")
            If ddl_Category.CheckedItems.Count >= 1 Then
                If ddl_Category.CheckedItems.Count = ddl_Category.Items.Count Then
                    Category = "-1"
                Else
                    Category = ""
                    For Each itm As RadComboBoxItem In ddl_Category.CheckedItems
                        Category = Category + itm.Value + ","
                    Next
                End If
            End If
            log.Error("7")
            lbl_Agency.Text = Agency
            lbl_Category.Text = Category

            Args.Visible = True


            Dim ObjReport As New SalesWorx.BO.Common.Reports
             
             

            Dim dt As New DataTable
            dt = ObjReport.GetDailyProductSales(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, van, objUserAccess.UserID, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"), Agency, Category, Invid, CustID)
            



        Dim dtTargetSales As New DataTable
            dtTargetSales.Columns.Add("Date", System.Type.GetType("System.DateTime"))

        Dim s_target As Decimal = 0
            Dim s_Sales As Decimal = 0

            Dim DtM As New DataTable
            DtM = dt.DefaultView.ToTable(True, "Van")


        For Each sdr As DataRow In DtM.Rows
                dtTargetSales.Columns.Add(sdr("Van").ToString & "Qty", System.Type.GetType("System.Decimal"))
                dtTargetSales.Columns.Add(sdr("Van").ToString & "Amount", System.Type.GetType("System.Decimal"))
                dtTargetSales.Columns.Add(sdr("Van").ToString & "Invoices", System.Type.GetType("System.Decimal"))
                dtTargetSales.Columns.Add(sdr("Van").ToString & "Customers", System.Type.GetType("System.Decimal"))
        Next

            dtTargetSales.Columns.Add("AllVanQty", System.Type.GetType("System.Decimal"))
            dtTargetSales.Columns.Add("AllVanAmount", System.Type.GetType("System.Decimal"))
            dtTargetSales.Columns.Add("AllVanInvoices", System.Type.GetType("System.Decimal"))
            dtTargetSales.Columns.Add("AllVanCustomers", System.Type.GetType("System.Decimal"))

            Dim DtDate As New DataTable
            dt.DefaultView.Sort = "SDate Asc"
            DtDate = dt.DefaultView.ToTable(True, "SDate")

            For Each sdr As DataRow In DtDate.Rows
                Dim dr As DataRow
                dr = dtTargetSales.NewRow
                dr("Date") = sdr("SDate")
                Dim ToatalQty As Decimal = 0
                Dim ToatalAmount As Decimal = 0
                Dim ToatalInvoices As Integer = 0
                Dim ToatalCustomers As Integer = 0


                For Each Vandr As DataRow In DtM.Rows

                    Dim seldrTarget() As DataRow
                    seldrTarget = dt.Select("Van='" & Vandr("Van") & "' and SDate='" & sdr("SDate") & "'")
                    If seldrTarget.Length > 0 Then
                        dr(Vandr("Van").ToString & "Qty") = Val(seldrTarget(0)("Qty").ToString)
                        dr(Vandr("Van").ToString & "Amount") = Val(seldrTarget(0)("Amount").ToString)
                        dr(Vandr("Van").ToString & "Invoices") = Val(seldrTarget(0)("Invoices").ToString)
                        dr(Vandr("Van").ToString & "Customers") = Val(seldrTarget(0)("Customers").ToString)


                        ToatalQty = ToatalQty + Val(seldrTarget(0)("Qty").ToString)
                        ToatalAmount = ToatalAmount + Val(seldrTarget(0)("Amount").ToString)
                        ToatalInvoices = ToatalInvoices + Val(seldrTarget(0)("Invoices").ToString)
                        ToatalCustomers = ToatalCustomers + Val(seldrTarget(0)("Customers").ToString)


                    Else
                        dr(Vandr("Van").ToString & "Qty") = 0
                        dr(Vandr("Van").ToString & "Amount") = 0
                        dr(Vandr("Van").ToString & "Invoices") = 0
                        dr(Vandr("Van").ToString & "Customers") = 0
                    End If


                Next
                dr("AllVanQty") = ToatalQty
                dr("AllVanAmount") = ToatalAmount
                dr("AllVanInvoices") = ToatalInvoices
                dr("AllVanCustomers") = ToatalCustomers


                dtTargetSales.Rows.Add(dr)
            Next


            DtDate = Nothing

        If dtTargetSales.Rows.Count > 0 Then

            Using package As New ExcelPackage()
                ' add a new worksheet to the empty workbook
                    Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
                    Worksheet.Cells("A2").LoadFromDataTable(dtTargetSales, True)
                    Worksheet.Cells.AutoFitColumns()
                    Worksheet.Column(1).Style.Numberformat.Format = "dd-MMM-yyyy"

                    Dim j As String = 2

                    For i = 0 To DtM.Rows.Count - 1

                        Worksheet.Cells(2, j).Value = "Qty"
                        Worksheet.Cells(1, j).Value = DtM.Rows(i)("Van").ToString
                        Worksheet.Cells(1, j).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center
                        Worksheet.Cells(1, j, 1, j + 3).Merge = True
                        Worksheet.Cells(2, j + 1).Value = "Amount"
                        Worksheet.Cells(2, j + 2).Value = "Invoices"
                        Worksheet.Cells(2, j + 3).Value = "Customers"
                        j = j + 4
                    Next

                    Worksheet.Cells(2, j).Value = "Qty"
                    Worksheet.Cells(1, j).Value = "All Vans"
                    Worksheet.Cells(1, j).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center
                    Worksheet.Cells(1, j, 1, j + 3).Merge = True
                    Worksheet.Cells(2, j + 1).Value = "Amount"
                    Worksheet.Cells(2, j + 2).Value = "Invoices"
                    Worksheet.Cells(2, j + 3).Value = "Customers"
                    j = j + 4

                    Response.Clear()
                    Response.Buffer = True
                    Response.Charset = ""

                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    Response.AddHeader("content-disposition", "attachment;filename= DailyProductSales.xlsx")

                    Using MyMemoryStream As New MemoryStream()
                        package.SaveAs(MyMemoryStream)
                        MyMemoryStream.WriteTo(Response.OutputStream)
                        Response.AddHeader("Content-Length", MyMemoryStream.Length)
                        Response.Flush()
                        Response.Close()
                    End Using
            End Using
            End If
        End If

    End Sub
    Private Sub gvbyAgency_CellDataBound(sender As Object, e As PivotGridCellDataBoundEventArgs) Handles gvRep.CellDataBound
        Try


            If TypeOf e.Cell Is PivotGridColumnHeaderCell Then
                If e.Cell.Text.IndexOf("Sum of") >= 0 Then
                    e.Cell.Text = e.Cell.Text.Replace("Sum of", "")
                End If

            End If


        Catch ex As Exception

        End Try
    End Sub
    
    Private Sub gvRep_PageIndexChanged(sender As Object, e As PivotGridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        BindReport()
    End Sub
   
     
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If ddlOrganization.SelectedIndex > 0 Then

            Dim TemFromDateStr As String = CDate(txtFromDate.SelectedDate).ToString("dd/MM/yyyy")
            Dim DateArr As Array = TemFromDateStr.Split("/")
            If DateArr.Length = 3 Then
                TemFromDateStr = DateArr(1) & "/" & DateArr(0) & "/" & DateArr(2)
            End If
            Dim TemToDateStr As String = CDate(txtToDate.SelectedDate).ToString("dd/MM/yyyy")
            Dim DateArr1 As Array = TemToDateStr.Split("/")
            If DateArr1.Length = 3 Then
                TemToDateStr = DateArr1(1) & "/" & DateArr1(0) & "/" & DateArr1(2)
            End If

            If Not IsDate(TemFromDateStr) Then
                MessageBoxValidation("Enter valid ""From date"".", "Validation")
                SetFocus(txtFromDate)
                Return bretval
            End If

            If Not IsDate(TemToDateStr) Then
                MessageBoxValidation("Enter valid ""To date"".", "Validation")
                SetFocus(TemToDateStr)
                Return bretval
            End If

            If CDate(TemFromDateStr) > CDate(TemToDateStr) Then
                MessageBoxValidation("Start Date should not be greater than End Date.", "Validation")
                Return bretval
            End If
            If Math.Abs(DateDiff(DateInterval.Day, CDate(TemFromDateStr), CDate(TemToDateStr))) > 31 Then
                MessageBoxValidation("Your date range selection should not exceed 31 days", "Validation")
                Return bretval
            End If
            bretval = True
            Return bretval
        Else
            MessageBoxValidation("Please select the Organisation", "Validation")
            Return bretval
        End If
    End Function
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            gvRep.Visible = True

            BindReport()
        Else
            Args.Visible = False
            gvRep.Visible = False

        End If
    End Sub
    Sub MessageBoxValidation(ByVal str As String, Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        LoadOrgDetails()
    End Sub
    Sub LoadOrgDetails()
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        ObjCommon = New SalesWorx.BO.Common.Common()
        ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
        ddlVan.DataBind()
        For Each itm As RadComboBoxItem In ddlVan.Items
            itm.Checked = True
        Next

        ddl_Agency.DataSource = ObjCommon.GetAgencyByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
        ddl_Agency.DataBind()
        For Each itm As RadComboBoxItem In ddl_Agency.Items
            itm.Checked = True
        Next
        ddl_Category.DataSource = ObjCommon.GetCategoryByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
        ddl_Category.DataBind()
        For Each itm As RadComboBoxItem In ddl_Category.Items
            itm.Checked = True
        Next

    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If

        If Not Request.QueryString("ID") Is Nothing Then
            txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
            txtToDate.SelectedDate = Now()

            Dim dt As New DataTable
            dt = (New SalesWorx.BO.Common.Common).GetSalesOrgbyFsr(Err_No, Err_Desc, Request.QueryString("ID"))
            If dt.Rows.Count > 0 Then
                If Not ddlOrganization.FindItemByValue(dt.Rows(0)("MAS_Org_ID").ToString) Is Nothing Then
                    ddlOrganization.ClearSelection()
                    ddlOrganization.FindItemByValue(dt.Rows(0)("MAS_Org_ID").ToString).Selected = True
                End If
            End If
        Else
            txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
            txtToDate.SelectedDate = Now()
        End If

        LoadOrgDetails()
        If Not ddlVan.Items.FindItemByValue(Request.QueryString("ID")) Is Nothing Then
            ddlVan.ClearSelection()
            ddlVan.Items.FindItemByValue(Request.QueryString("ID")).Selected = True
        End If

        ddl_Product.Entries.Clear()
        Args.Visible = False
        gvRep.Visible = False
    End Sub
End Class