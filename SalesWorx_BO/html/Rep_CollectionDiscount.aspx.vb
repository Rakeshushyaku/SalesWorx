Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Imports OfficeOpenXml
Public Class Rep_CollectionDiscount
    Inherits System.Web.UI.Page

    
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "CollectionDiscount"

    Private Const PageID As String = "P383"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single

    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    'Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
    '    If Not IsNothing(Me.Master) Then

    '        Dim masterScriptManager As ScriptManager
    '        masterScriptManager = CType(Master.FindControl("ScriptManager1"), ScriptManager)

    '        ' Make sure our master page has the script manager we're looking for
    '        If Not IsNothing(masterScriptManager) Then

    '            ' Turn off partial page postbacks for this page
    '            masterScriptManager.EnablePartialRendering = False
    '        End If

    '    End If

    'End Sub
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
                ObjCommon = New SalesWorx.BO.Common.Common()
                Dim CountryTbl As DataTable = Nothing
                Dim orgTbl As DataTable = Nothing

                CountryTbl = ObjCommon.GetCountry(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)
                ddlCountry.DataSource = CountryTbl
                ddlCountry.DataBind()


                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)

                Dim s() As String = Nothing
                Dim Currency As String = Nothing
                Dim DecimalDigits As String = "2"
                Dim country As String = Nothing
                If CountryTbl.Rows.Count = 1 Then

                    ddlCountry.SelectedIndex = 0
                    dvCountry.Visible = False

                    s = ddlCountry.SelectedValue.Split("$")

                    If s.Length > 0 Then
                        country = s(0).ToString()
                        Currency = s(1).ToString()
                        DecimalDigits = s(2).ToString()
                    End If


                    ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
                    ddlOrganization.DataBind()


                ElseIf CountryTbl.Rows.Count > 1 Then
                    ddlCountry.SelectedIndex = 0
                    dvCountry.Visible = True


                    s = ddlCountry.SelectedValue.Split("$")

                    If s.Length > 0 Then
                        country = s(0).ToString()
                        Currency = s(1).ToString()
                        DecimalDigits = s(2).ToString()
                    End If

                    ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
                    ddlOrganization.DataBind()

                End If

                Dim OrgStr As String = Nothing
                For Each item As RadComboBoxItem In ddlOrganization.Items
                    item.Checked = True
                    If item.Checked Then

                        OrgStr = OrgStr & "," & item.Value

                    End If
                Next


                LoadOrgDetails()
                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                txtToDate.SelectedDate = Now



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
        End If
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

        Dim Orgtxt As String = ""
        Dim Org As String = ""
        Dim Orgcollection As IList(Of RadComboBoxItem) = ddlOrganization.CheckedItems
        For Each li As RadComboBoxItem In Orgcollection
            Org = Org & li.Value & ","
            Orgtxt = Orgtxt & li.Text & ","
        Next
        If Orgtxt <> "" Then
            Orgtxt = Orgtxt.Substring(0, Orgtxt.Length - 1)
        End If
        If Org = "" Then
            Org = "0"
        End If

        Dim tCustomerID As String = "0"
        Dim tSiteID As String = "0"

        If Not String.IsNullOrEmpty(ddl_Customer.SelectedValue) Then

            Dim ids() As String
            ids = ddl_Customer.SelectedValue.Split("$")

            tCustomerID = ids(0)
            tSiteID = ids(1)
        End If

        Dim CustID As New ReportParameter
        CustID = New ReportParameter("CustID", tCustomerID)



        Dim SiteID As New ReportParameter
        SiteID = New ReportParameter("SiteID", tSiteID)

        Dim USRID As New ReportParameter
        USRID = New ReportParameter("UID", CType(Session.Item("USER_ACCESS"), UserAccess).UserID)


        Dim SalesRepID As New ReportParameter
        SalesRepID = New ReportParameter("SID", van)

        Dim FDate As New ReportParameter
        FDate = New ReportParameter("FromDate", CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"))

        Dim TDate As New ReportParameter
        TDate = New ReportParameter("ToDate", CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))


        Dim OID As New ReportParameter
        OID = New ReportParameter("OID", Org)

       

        Dim bytes As Byte()


        rview.ServerReport.SetParameters(New ReportParameter() {USRID, OID, SalesRepID, FDate, TDate, CustID, SiteID})

        Dim mimeType As String = Nothing
        Dim encoding As String = Nothing
        Dim extension As String = Nothing
        Dim deviceInfo As String = "<DeviceInfo></DeviceInfo>"
        Dim streamids As String() = Nothing
        Dim warnings As Microsoft.Reporting.WebForms.Warning() = Nothing

        bytes = rview.ServerReport.Render(format, deviceInfo, mimeType, encoding, extension, streamids, warnings)


        Response.Clear()
        If format = "PDF" Then
            Response.ContentType = "application/pdf"
            Response.AddHeader("Content-disposition", "attachment;filename=CollectionDiscount.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=CollectionDiscount.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()
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
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            gvRep.Visible = True
            BindReport()
        Else
            Args.Visible = False
            summary.InnerHtml = ""
            gvRep.Visible = False
        End If
    End Sub
    Private Sub BindReport()
        Try
            Dim SearchQuery As String = ""
            If ValidateInputs() Then
                rpbFilter.Items(0).Expanded = False
                Dim objUserAccess As UserAccess
                objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
                Dim ObjReport As New SalesWorx.BO.Common.Reports

                Dim dt As New DataTable
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


                Dim tCustomerID As String = "0"
                Dim tSiteID As String = "0"

                If Not String.IsNullOrEmpty(ddl_Customer.SelectedValue) Then

                    Dim ids() As String
                    ids = ddl_Customer.SelectedValue.Split("$")

                    tCustomerID = ids(0)
                    tSiteID = ids(1)

                    Dim custdt As New DataTable
                    custdt = (New SalesWorx.BO.Common.Customer).GetCustomerDeatils(Err_No, Err_Desc, ids(0), ids(1))
                    If custdt.Rows.Count > 0 Then
                        lbl_Customer.Text = custdt.Rows(0)("Customer_no").ToString & " - " & custdt.Rows(0)("Customer_name").ToString
                    End If

                Else
                    lbl_Customer.Text = "All"

                End If

                Dim Orgtxt As String = ""
                Dim Org As String = ""
                Dim Orgcollection As IList(Of RadComboBoxItem) = ddlOrganization.CheckedItems
                For Each li As RadComboBoxItem In Orgcollection
                    Org = Org & li.Value & ","
                    Orgtxt = Orgtxt & li.Text & ","
                Next
                If Orgtxt <> "" Then
                    Orgtxt = Orgtxt.Substring(0, Orgtxt.Length - 1)
                End If
                If Org = "" Then
                    Org = "0"
                End If

                lbl_org.Text = Orgtxt

                If van = "0" Then
                    lbl_van.Text = "All"
                Else
                    lbl_van.Text = vantxt
                End If
                lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
                lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")

                Args.Visible = True

                dt = ObjReport.GetCollectionDiscount(Err_No, Err_Desc, Org, van, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"), objUserAccess.UserID, tCustomerID, tSiteID)
                gvRep.DataSource = dt
                gvRep.DataBind()



                Dim dtcurrency As DataTable
                dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.CheckedItems(0).Value)
                Dim Currency As String = ""
                If dtcurrency.Rows.Count > 0 Then
                    Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
                End If

                Dim query = (From UserEntry In dt _
                            Group UserEntry By key = UserEntry.Field(Of String)("Collection_Type") Into Group _
                            Select PayMode = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("Amount"))).ToList

                 
                Dim StrSummary As String = ""
                
                Dim sum = dt.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("Discount")))

                StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>Total discount " & Currency & "<div class='text-primary'>" & Format(sum, lblDecimal.Text) & "</div></div></div>"

                summary.InnerHtml = StrSummary
            End If
        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If ddlOrganization.CheckedItems Is Nothing Then
            MessageBoxValidation("Select an Organisation", "Validation")
            SetFocus(ddlOrganization)
            Return bretval

        Else
            If ddlOrganization.CheckedItems.Count <= 0 Then
                MessageBoxValidation("Select an Organisation", "Validation")
                SetFocus(ddlOrganization)
                Return bretval
            End If
        End If
        If txtFromDate.DateInput.Text = "" Then
            MessageBoxValidation("Enter valid ""From date"".", "Validation")
            SetFocus(txtFromDate)
            Return bretval
        End If

        If txtToDate.DateInput.Text = "" Then
            MessageBoxValidation("Enter valid ""To date"".", "Validation")
            SetFocus(txtToDate)
            Return bretval
        End If

        If CDate(txtFromDate.SelectedDate) > CDate(txtToDate.SelectedDate) Then
            MessageBoxValidation("Start Date should not be greater than End Date.", "Validation")
            Return bretval
        End If
        bretval = True
        Return bretval
    End Function



    Sub LoadOrgDetails()
        'If Not (ddlOrganization.SelectedItem.Value = "0") Then

        '    Dim objUserAccess As UserAccess
        '    objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        '    ObjCommon = New SalesWorx.BO.Common.Common()
        '    ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
        '    ddlVan.DataBind()

        '    For Each itm As RadComboBoxItem In ddlVan.Items
        '        itm.Checked = True
        '    Next

        '    Dim dtcurrency As DataTable
        '    Dim ObjReport As New SalesWorx.BO.Common.Reports
        '    dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

        '    Dim Currency As String = ""
        '    If dtcurrency.Rows.Count > 0 Then
        '        Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
        '        lblDecimal.Text = "N" & dtcurrency.Rows(0)("Decimal_Digits")
        '    End If

        'End If
        If Not ddlOrganization.CheckedItems Is Nothing Then
            If ddlOrganization.CheckedItems.Count > 0 Then
                Dim objUserAccess As UserAccess
                objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
                Dim ObjRep As SalesWorx.BO.Common.Reports = New SalesWorx.BO.Common.Reports()

                Dim OrgStr As String = ""
                For Each li As RadComboBoxItem In ddlOrganization.CheckedItems
                    OrgStr = OrgStr & li.Value & ","
                Next
                ddlVan.DataSource = ObjRep.GetAllOrgVan(Err_No, Err_Desc, OrgStr, objUserAccess.UserID.ToString())
                ddlVan.DataBind()

                For Each itm As RadComboBoxItem In ddlVan.Items
                    itm.Checked = True
                Next

                Dim dtcurrency As DataTable
                Dim ObjReport As New SalesWorx.BO.Common.Reports
                dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.CheckedItems(0).Value)

                Dim Currency As String = ""
                If dtcurrency.Rows.Count > 0 Then
                    Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
                    lblDecimal.Text = "N" & dtcurrency.Rows(0)("Decimal_Digits")
                End If
            Else
                ddlVan.Items.Clear()
            End If
        Else
            ddlVan.Items.Clear()
        End If

    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub

    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender
        For Each column As GridColumn In gvRep.MasterTableView.Columns
            If column.UniqueName = "Amount" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
            End If
            If column.UniqueName = "Discount" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
            End If
        Next
    End Sub
    Private Sub dgv_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvRep.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindReport()
    End Sub
    Private Sub dgv_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged

        BindReport()
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
    Private Sub ddlOrganization_CheckAllCheck(sender As Object, e As RadComboBoxCheckAllCheckEventArgs) Handles ddlOrganization.CheckAllCheck
        LoadOrgDetails()
    End Sub

    Private Sub ddlOrganization_ItemChecked(sender As Object, e As RadComboBoxItemEventArgs) Handles ddlOrganization.ItemChecked
        LoadOrgDetails()
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ObjCommon = New SalesWorx.BO.Common.Common()
        Dim CountryTbl As DataTable = Nothing
        Dim orgTbl As DataTable = Nothing

        CountryTbl = ObjCommon.GetCountry(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)
        ddlCountry.DataSource = CountryTbl
        ddlCountry.DataBind()


        Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
        orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)

        Dim s() As String = Nothing
        Dim Currency As String = Nothing
        Dim DecimalDigits As String = "2"
        Dim country As String = Nothing
        If CountryTbl.Rows.Count = 1 Then

            ddlCountry.SelectedIndex = 0
            dvCountry.Visible = False

            s = ddlCountry.SelectedValue.Split("$")

            If s.Length > 0 Then
                country = s(0).ToString()
                Currency = s(1).ToString()
                DecimalDigits = s(2).ToString()
            End If


            ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
            ddlOrganization.DataBind()


        ElseIf CountryTbl.Rows.Count > 1 Then
            ddlCountry.SelectedIndex = 0
            dvCountry.Visible = True


            s = ddlCountry.SelectedValue.Split("$")

            If s.Length > 0 Then
                country = s(0).ToString()
                Currency = s(1).ToString()
                DecimalDigits = s(2).ToString()
            End If

            ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
            ddlOrganization.DataBind()

        End If

        Dim OrgStr As String = Nothing
        For Each item As RadComboBoxItem In ddlOrganization.Items
            item.Checked = True
            If item.Checked Then

                OrgStr = OrgStr & "," & item.Value

            End If
        Next


        LoadOrgDetails()
        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Now

        ddl_Customer.ClearSelection()
        ddl_Customer.Text = ""
        Args.Visible = False
        summary.InnerHtml = ""
        gvRep.Visible = False
    End Sub
    Private Sub ddlCountry_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlCountry.SelectedIndexChanged
        LoadOrgs()
        LoadOrgDetails()
    End Sub
    Sub LoadOrgs()
        Dim orgTbl As DataTable = Nothing

        ObjCommon = New SalesWorx.BO.Common.Common()


        Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)



        orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)

        Dim s() As String = Nothing
        Dim Currency As String = Nothing
        Dim DecimalDigits As String = "2"
        Dim country As String = Nothing
        s = ddlCountry.SelectedValue.Split("$")

        If s.Length > 0 Then
            country = s(0).ToString()
            Currency = s(1).ToString()
            DecimalDigits = s(2).ToString()
        End If


        ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
        ddlOrganization.DataBind()



        Dim OrgStr As String = Nothing
        For Each item As RadComboBoxItem In ddlOrganization.Items
            item.Checked = True
            If item.Checked Then

                OrgStr = OrgStr & "," & item.Value

            End If
        Next

    End Sub
    Private Sub BtnExportBiffExcel_Click(sender As Object, e As EventArgs) Handles BtnExportBiffExcel.Click
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        Dim ObjReport As New SalesWorx.BO.Common.Reports

        Dim dt As New DataTable
        Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems


        Dim van As String = ""
        For Each li As RadComboBoxItem In collection
            van = van & li.Value & ","

        Next

        If van = "" Then
            van = "0"
        End If


        Dim tCustomerID As String = "0"
        Dim tSiteID As String = "0"

        If Not String.IsNullOrEmpty(ddl_Customer.SelectedValue) Then

            Dim ids() As String
            ids = ddl_Customer.SelectedValue.Split("$")

            tCustomerID = ids(0)
            tSiteID = ids(1)

            Dim custdt As New DataTable
            custdt = (New SalesWorx.BO.Common.Customer).GetCustomerDeatils(Err_No, Err_Desc, ids(0), ids(1))
            If custdt.Rows.Count > 0 Then
                lbl_Customer.Text = custdt.Rows(0)("Customer_no").ToString & " - " & custdt.Rows(0)("Customer_name").ToString
            End If

        Else
            lbl_Customer.Text = "All"

        End If


        Dim Org As String = ""
        Dim Orgcollection As IList(Of RadComboBoxItem) = ddlOrganization.CheckedItems
        For Each li As RadComboBoxItem In Orgcollection
            Org = Org & li.Value & ","

        Next

        If Org = "" Then
            Org = "0"
        End If

        Dim tblData As New DataTable

        dt = ObjReport.GetCollectionDiscount(Err_No, Err_Desc, Org, van, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"), objUserAccess.UserID, tCustomerID, tSiteID)

        tblData = dt.DefaultView.ToTable(False, "Collection_Ref_No", "Invoice_No", "Collected_On", "SalesREp_Name", "Customer_No", "CustomerName", "Amount", "Discount", "Net")

        If tblData.Rows.Count > 0 Then
            Dim drtotal As DataRow
            drtotal = tblData.NewRow
            drtotal("Collection_Ref_No") = "Total"
            Dim sum = tblData.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("Amount")))
            Dim sumDiscount = tblData.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("Discount")))
            Dim sumNet = tblData.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("Net")))

            drtotal("Amount") = sum
            drtotal("Discount") = sumDiscount
            drtotal("Net") = sumNet

            tblData.Rows.Add(drtotal)

            Using package As New ExcelPackage()
                ' add a new worksheet to the empty workbook
                Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
                Worksheet.Cells("A1").LoadFromDataTable(tblData, True)
                Worksheet.Column(3).Style.Numberformat.Format = "dd-MMM-yyyy"
                Worksheet.Cells.AutoFitColumns()
                Worksheet.Cells("A1").Value = "Collection No"
                Worksheet.Cells("B1").Value = "Invoice No"
                Worksheet.Cells("C1").Value = "Collected On"
                Worksheet.Cells("D1").Value = "Van Code"
                Worksheet.Cells("E1").Value = "Customer No"
                Worksheet.Cells("F1").Value = "Customer Name"
                Worksheet.Cells("G1").Value = "Invoice Amount"
                Worksheet.Cells("H1").Value = "Discount"
                Worksheet.Cells("I1").Value = "Net Value"

                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename= CollectionDiscount.xlsx")

                Using MyMemoryStream As New MemoryStream()
                    package.SaveAs(MyMemoryStream)
                    MyMemoryStream.WriteTo(Response.OutputStream)
                    Response.AddHeader("Content-Length", MyMemoryStream.Length)
                    Response.Flush()
                    Response.Close()
                End Using
            End Using
        End If
    End Sub


    Private Sub ddl_Customer_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddl_Customer.ItemsRequested

        Args.Visible = False
        Dim Objrep As New SalesWorx.BO.Common.Reports()
        Dim dt As New DataTable
        dt = Objrep.GetCustomerfromOrgtext(Err_No, Err_Desc, ddlOrganization.SelectedValue, e.Text)


        Dim ItemsPerRequest As Integer = 100 'Set the number of values to show
        Dim itemOffset As Integer = e.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
        e.EndOfItems = endOffset = dt.Rows.Count

        'Loop through the values to populate the combo box
        For i As Integer = itemOffset To endOffset - 1
            Dim item As New RadComboBoxItem()
            item.Text = dt.Rows(i).Item("Customer").ToString
            item.Value = dt.Rows(i).Item("CustomerID").ToString

            ddl_Customer.Items.Add(item)
            item.DataBind()
        Next
    End Sub
End Class