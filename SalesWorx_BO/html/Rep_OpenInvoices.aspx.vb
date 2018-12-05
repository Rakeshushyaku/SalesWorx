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

Partial Public Class Rep_OpenInvoices
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer
    Private ReportPath As String = "OpenInvoices"
    Private Const PageID As String = "P430"
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
                dtCust.Columns.Add("ID")
                dtCust.Columns.Add("Desc")
                ViewState("dtCust") = dtCust

                ObjCommon = New SalesWorx.BO.Common.Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))
                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                End If

                    txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                    txtToDate.SelectedDate = Now()

                LoadOrgDetails()

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

            LoadCustomer()
        End If

    End Sub
   

    Sub LoadCustomer()
        Try


            Dim Objrep As New SalesWorx.BO.Common.Reports()
        Dim dt As New DataTable
        dt = Objrep.GetCustomerfromOrgtext(Err_No, Err_Desc, ddlOrganization.SelectedValue, "")
        ddl_Product.DataSource = dt
        ddl_Product.DataTextField = "Customer"
        ddl_Product.DataValueField = "CustomerID"
            ddl_Product.DataBind()
        Catch ex As Exception
            log.Debug(ex.Message.ToString())
        End Try
    End Sub


    Private Sub BtnExport_Click(sender As Object, e As EventArgs) Handles BtnExportExcel.Click
        Try
            If ValidateInputs() Then
                Export("Excel")
            End If
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub
    Private Sub BtnPDF_Click(sender As Object, e As EventArgs) Handles BtnExportPDF.Click
        Try
            If ValidateInputs() Then
                Export("PDF")
            End If
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub

    Sub Export(format As String)

        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)

        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

        Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems
        Dim van As String = ""
        For Each li As RadComboBoxItem In collection
            van = van & li.Value & ","

        Next

        If van = "" Then
            van = "0"
        End If


        Dim UID As New ReportParameter
        UID = New ReportParameter("UID", objUserAccess.UserID)


        Dim SID As New ReportParameter
        SID = New ReportParameter("SID", van)

        Dim Fromdate As Date
        Dim Todate As Date

        Fromdate = CDate(txtFromDate.SelectedDate)
        Todate = CDate(txtToDate.SelectedDate)



        Dim FDate As New ReportParameter
        FDate = New ReportParameter("FromDate", Fromdate.ToString("dd-MMM-yyyy"))

        Dim TDate As New ReportParameter
        TDate = New ReportParameter("Todate", Todate.ToString("dd-MMM-yyyy"))


        Dim OrgID As New ReportParameter
        OrgID = New ReportParameter("OrgID", CStr(ddlOrganization.SelectedValue.ToString()))


        Dim showoverdue As String
        If chkActive.Checked = True Then
            lbl_Overdue.Text = "Show Overdues"
            showoverdue = "1"
        Else
            lbl_Overdue.Text = "Not Show Overdues"
            showoverdue = "0"
        End If

        Dim OVERDUE As New ReportParameter
        OVERDUE = New ReportParameter("OVERDUE", showoverdue)

      
        lbl_Customer.Text = ""

        dtCust = CType(ViewState("dtCust"), DataTable)
        Dim Custid_info As String = ""
        If dtCust.Rows.Count > 0 Then
            For Each dr In dtCust.Rows

                Custid_info = Custid_info & dr("ID").ToString & ","
                lbl_Customer.Text = lbl_Customer.Text & dr("Desc").ToString & ","
            Next
            Custid_info = Custid_info.Substring(0, Custid_info.Length - 1)
            lbl_Customer.Text = lbl_Customer.Text.Substring(0, lbl_Customer.Text.Length - 1)
        Else
            Custid_info = "0"
            lbl_Customer.Text = "All"
        End If

        Dim CUSTID As New ReportParameter
        CUSTID = New ReportParameter("CUSTID", Custid_info)

        Dim Customer As New ReportParameter
        Customer = New ReportParameter("Customer", lbl_Customer.Text)
     

        rview.ServerReport.SetParameters(New ReportParameter() {OrgID, SID, FDate, TDate, CUSTID, OVERDUE, UID, Customer})

        Dim mimeType As String = Nothing
        Dim encoding As String = Nothing
        Dim extension As String = Nothing
        Dim deviceInfo As String = "<DeviceInfo></DeviceInfo>"
        Dim streamids As String() = Nothing
        Dim warnings As Microsoft.Reporting.WebForms.Warning() = Nothing

        Dim bytes As Byte() = rview.ServerReport.Render(format, deviceInfo, mimeType, encoding, extension, streamids, warnings)


        Response.Clear()
        If format = "PDF" Then
            Response.ContentType = "application/pdf"
            Response.AddHeader("Content-disposition", "attachment;filename=OpenInvoices.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=OpenInvoices.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()

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

            dtCust = CType(ViewState("dtCust"), DataTable)
            Dim Custid_info As String = ""
            If dtCust.Rows.Count > 0 Then
                For Each dr In dtCust.Rows

                    Custid_info = Custid_info & dr("ID").ToString & ","
                    lbl_Customer.Text = lbl_Customer.Text & dr("Desc").ToString & ","
                Next
                Custid_info = Custid_info.Substring(0, Custid_info.Length - 1)

                lbl_Customer.Text = lbl_Customer.Text.Substring(0, lbl_Customer.Text.Length - 1)
            Else
                Custid_info = "0"
                lbl_Customer.Text = "All"
            End If

            Args.Visible = True


            Dim Fromdate As Date
            Dim Todate As Date


            Dim showoverdue As String
            If chkActive.Checked = True Then
                lbl_Overdue.Text = "Show Overdues"
                showoverdue = "1"
            Else
                lbl_Overdue.Text = "Not Show Overdues"
                showoverdue = "0"
            End If

            Fromdate = CDate(txtFromDate.SelectedDate)
            Todate = CDate(txtToDate.SelectedDate)

            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetOpenInvoice(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, van, objUserAccess.UserID, Fromdate.ToString("dd-MMM-yyyy"), Todate.ToString("dd-MMM-yyyy"), Custid_info, showoverdue)
            gvRep.DataSource = dt
            gvRep.DataBind()
            End If
        Catch ex As Exception
            log.Debug(ex.Message.ToString())
        End Try
    End Sub

    Private Sub gvRep_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles gvRep.ItemDataBound
        'If TypeOf e.Item Is GridDataItem Then

        '    Dim dataItem As GridDataItem = e.Item
        '    Dim myCell As TableCell = dataItem("Doctype")
        '    If myCell.Text = "DUE" Then
        '        dataItem.BackColor = System.Drawing.Color.LightGray
        '        ''  dataItem.ForeColor = Drawing.Color.White
        '    End If
        'End If



        'If TypeOf e.Item Is GridDataItem Then
        '    Dim item As GridDataItem = CType(e.Item, GridDataItem)

        '    If item("Doctype").Text = "DUE" Then
        '        item.BackColor = System.Drawing.Color.LightGray
        '    Else
        '        item.BackColor = System.Drawing.Color.Green
        '    End If
        'End If

        If (TypeOf e.Item Is GridDataItem) Then
            Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
            If (Convert.ToDateTime(dataItem("Due_Date").Text) <= DateTime.Now) Then
                '  dataItem.CssClass = "Dueclass"
                dataItem("Invoice_Ref_No").ForeColor = System.Drawing.Color.Red

            End If
        End If
    End Sub

    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender
        Try
            For Each column As GridColumn In gvRep.MasterTableView.Columns
                If column.UniqueName = "Invoice_Amount" Then
                    Dim col As GridBoundColumn
                    col = (CType(column, GridBoundColumn))
                    column.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                    col.DataType = System.Type.GetType("System.Decimal")
                    col.DataFormatString = "{0:" & lblDecimal.Text & "}"
                ElseIf column.UniqueName = "Pending_Amount" Then
                    Dim col As GridBoundColumn
                    col = (CType(column, GridBoundColumn))
                    col.DataType = System.Type.GetType("System.Decimal")
                    col.DataFormatString = "{0:" & lblDecimal.Text & "}"
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                End If
            Next
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub



    Private Sub dgv_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvRep.SortCommand
        Try

       
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
            BindReport()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Private Sub dgv_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        Try
            BindReport()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
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
    Function ValidateInputs() As Boolean
        Try
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
                bretval = True
                Return bretval
            Else
                MessageBoxValidation("Please select the Organisation", "Validation")
                Return bretval
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Function
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        Try
            If ValidateInputs() Then
                gvRep.Visible = True

                BindReport()
            Else
                Args.Visible = False
                gvRep.Visible = False

            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Sub MessageBoxValidation(ByVal str As String, Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        LoadOrgDetails()
    End Sub
    Sub LoadOrgDetails()
        Try
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New SalesWorx.BO.Common.Common()
            ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
            ddlVan.DataBind()
            For Each itm As RadComboBoxItem In ddlVan.Items
                itm.Checked = True
            Next

            Dim dtcurrency As DataTable
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

            Dim Currency As String = ""
            If dtcurrency.Rows.Count > 0 Then
                Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
                lblDecimal.Text = "N" & dtcurrency.Rows(0)("Decimal_Digits")
            End If

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        Try
            ddlOrganization.ClearSelection()
            If ddlOrganization.Items.Count = 2 Then
                ddlOrganization.SelectedIndex = 1
            End If
            txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
            txtToDate.SelectedDate = Now()

            LoadOrgDetails()
            chkActive.Checked = False
            ddl_Product.Entries.Clear()
            Args.Visible = False
            gvRep.Visible = False
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Private Sub ddl_Product_EntryAdded(sender As Object, e As AutoCompleteEntryEventArgs) Handles ddl_Product.EntryAdded
        dtCust = CType(ViewState("dtCust"), DataTable)
        Dim seldr() As DataRow
        seldr = dtCust.Select("ID='" & e.Entry.Value & "'")
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

    Private Sub ddl_Product_EntryRemoved(sender As Object, e As AutoCompleteEntryEventArgs) Handles ddl_Product.EntryRemoved
        dtCust = CType(ViewState("dtCust"), DataTable)
        Dim dr() As DataRow
        dr = dtCust.Select("ID='" & e.Entry.Value & "'")
        If dr.Length > 0 Then
            dtCust.Rows.Remove(dr(0))
        End If
        ViewState("dtCust") = dtCust
        gvRep.Visible = False
        Args.Visible = False
    End Sub
    Private Sub BtnExportBiffExcel_Click(sender As Object, e As EventArgs) Handles BtnExportBiffExcel.Click
        Try

            If ValidateInputs() Then

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

                dtCust = CType(ViewState("dtCust"), DataTable)
                Dim Custid_info As String = ""
                If dtCust.Rows.Count > 0 Then
                    For Each dr In dtCust.Rows

                        Custid_info = Custid_info & dr("ID").ToString & ","
                        lbl_Customer.Text = lbl_Customer.Text & dr("Desc").ToString & ","
                    Next
                    Custid_info = Custid_info.Substring(0, Custid_info.Length - 1)

                    lbl_Customer.Text = lbl_Customer.Text.Substring(0, lbl_Customer.Text.Length - 1)
                Else
                    Custid_info = "0"
                    lbl_Customer.Text = "All"
                End If

                Args.Visible = True


                Dim Fromdate As Date
                Dim Todate As Date


                Fromdate = CDate(txtFromDate.SelectedDate)
                Todate = CDate(txtToDate.SelectedDate)


                Dim showoverdue As String
                If chkActive.Checked = True Then
                    lbl_Overdue.Text = "Show Overdues"
                    showoverdue = "1"
                Else
                    lbl_Overdue.Text = "Not Show Overdues"
                    showoverdue = "0"
                End If

                Dim ObjReport As New SalesWorx.BO.Common.Reports
                Dim dt As New DataTable
                dt = ObjReport.GetOpenInvoice(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, van, objUserAccess.UserID, Fromdate.ToString("dd-MMM-yyyy"), Todate.ToString("dd-MMM-yyyy"), Custid_info, showoverdue)




                Dim tblData As New DataTable
                tblData = dt.DefaultView.ToTable(False, "Invoice_Ref_No", "Customer_No", "Pending_Amount", "Invoice_Date", "Due_Date", "Invoice_Amount")

                ' tblData.Columns("Invoice_Ref_No").ColumnName = "Total Sales"
                'tblData.Columns("Returns").ColumnName = "Total Returns"
                'tblData.Columns("Net").ColumnName = "Net Sales"

                If tblData.Rows.Count > 0 Then


                    Using package As New ExcelPackage()

                        Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
                        Worksheet.Cells("A1").LoadFromDataTable(tblData, True)

                        Worksheet.Column(3).Style.Numberformat.Format = "#,##0.00"
                        Worksheet.Column(4).Style.Numberformat.Format = "dd-MMM-yyyy"
                        Worksheet.Column(5).Style.Numberformat.Format = "dd-MMM-yyyy"
                        Worksheet.Column(6).Style.Numberformat.Format = "#,##0.00"
                        Worksheet.Cells.AutoFitColumns()
                        Response.Clear()
                        Response.Buffer = True
                        Response.Charset = ""

                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                        Response.AddHeader("content-disposition", "attachment;filename= OpenInvoices.xlsx")

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

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
End Class