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

Public Class RepIncentiveEmp
    Inherits System.Web.UI.Page


    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "RepEmpIncentive"
    Dim objIncentive As New SalesWorx.BO.Common.Incentive
    Private Const PageID As String = "P355"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single

    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Sub LoadOrgs()
        Dim orgTbl As DataTable = Nothing
        ObjCommon = New SalesWorx.BO.Common.Common()
        Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
        orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
        Dim s() As String = Nothing
        Dim Currency As String = Nothing
        Dim DecimalDigits As String = "2"
        Dim OrgStr As String = Nothing
        For Each item As RadComboBoxItem In ddlOrganization.Items
            item.Checked = True
            If item.Checked Then

                OrgStr = OrgStr & "," & item.Value

            End If
        Next

    End Sub
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


                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))
                dtp_incentiveyear.SelectedDate = DateTime.Now.Date
                Loademp()




                'Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)

                Dim s() As String = Nothing
                Dim Currency As String = Nothing
                Dim DecimalDigits As String = "2"
                Dim country As String = Nothing


                Dim OrgStr As String = Nothing
                For Each item As RadComboBoxItem In ddlOrganization.Items
                    item.Checked = True
                    If item.Checked Then

                        OrgStr = OrgStr & "," & item.Value

                    End If
                Next
                '   LoadVan()

                '   txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                ' txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")



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
    Sub Loademp()
        Try

            ddl_empcode.DataSource = objIncentive.GetEmpCode(Err_No, Err_Desc, ddlOrganization.SelectedValue)
            ddl_empcode.Items.Clear()
            ddl_empcode.Items.Add(New RadComboBoxItem("Select Emp"))
            ddl_empcode.AppendDataBoundItems = True
            ddl_empcode.DataTextField = "Emp_Name"
            ddl_empcode.DataValueField = "Emp_Code"
            ddl_empcode.DataBind()
        Catch ex As Exception
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    'Sub LoadVan()

    '    If Not ddlOrganization.CheckedItems Is Nothing Then
    '        If ddlOrganization.CheckedItems.Count > 0 Then
    '            Dim objUserAccess As UserAccess
    '            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
    '            Dim ObjRep As SalesWorx.BO.Common.Reports = New SalesWorx.BO.Common.Reports()

    '            Dim OrgStr As String = ""
    '            For Each li As RadComboBoxItem In ddlOrganization.CheckedItems
    '                OrgStr = OrgStr & li.Value & ","
    '            Next
    '            ddlVan.DataSource = ObjRep.GetAllOrgVan(Err_No, Err_Desc, OrgStr, objUserAccess.UserID.ToString())
    '            ddlVan.DataBind()

    '            For Each itm As RadComboBoxItem In ddlVan.Items
    '                itm.Checked = True
    '            Next

    '            Dim dtcurrency As DataTable
    '            Dim ObjReport As New SalesWorx.BO.Common.Reports
    '            dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.CheckedItems(0).Value)


    '        Else
    '            ddlVan.Items.Clear()
    '        End If
    '    Else
    '        ddlVan.Items.Clear()
    '    End If


    'End Sub

    Private Sub SearchBtn_Click(sender As Object, e As EventArgs) Handles SearchBtn.Click
        Try
            Args.Visible = False
            gvRep.Visible = False
            Chart.Visible = False
            If Not (ddlOrganization.SelectedItem.Value = "0") Then

                If Not IsDate(dtp_incentiveyear.SelectedDate) Then
                    MessageBoxValidation("Enter valid year.", "Validation")
                    Exit Sub
                End If
                gvRep.Visible = True
                Chart.Visible = True
                repdiv.Visible = True
                BindData()
                Chartwrapper.Visible = True
                BindChart()
            Else
                MessageBoxValidation("Select an organization.", "Validation")
            End If

        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
    Private Function GetMonthName(ByVal monthNum As Integer) As String
        Try
            Dim strDate As New DateTime(1, monthNum, 1)
            Return strDate.ToString("MMM")
        Catch ex As Exception
        End Try
    End Function
    Private Sub BindChart()
        Try

            Dim dt As DataTable = gvRep.DataSource

            For month As Integer = 1 To 12
                Dim result() As DataRow = dt.Select("Incentive_Month ='" & month & "'")

                ' Organization_ID()

                If result.Count = 0 Then
                    Dim dr As DataRow = dt.NewRow()
                    dr("Organization_ID") = dt.Rows(0)("Organization_ID")
                    dr("Emp_Code") = dt.Rows(0)("Emp_Code")
                    dr("Incentive_Year") = dt.Rows(0)("Incentive_Year")
                    dr("Incentive_Month") = month.ToString()
                    dr("NET_SALES_VOLUME") = "0"
                    dr("SUCCESSFUL_VISITS") = "0"
                    dr("Sales_Value_Acheived") = "0"
                    dr("Sales_Volume_Acheived") = "0"
                    dr("Success_Visits_Acheived") = "0"

                    Dim xMonth As String = MonthName(month)


                    dr("Tmonth") = xMonth
                    dr("TotalCommission") = "0"
                    dr("CommissionTarget") = "0"

                    dt.Rows.Add(dr)



                End If

            Next


            dt.DefaultView.Sort = "Incentive_Month ASC"

            Chart.DataSource = dt
            Chart.DataBind()

            'If dt.Rows.Count > 8 And dt.Rows.Count < 14 Then
            '    Chart.Style.Add("height", (dt.Rows.Count * 40).ToString & "px")
            'ElseIf dt.Rows.Count > 14 Then
            '    Chart.Style.Add("height", (dt.Rows.Count * 35).ToString & "px")
            'Else
            '    Chart.Style.Add("height", "400px")
            'End If
            If dt.Rows.Count > 8 And dt.Rows.Count < 14 Then
                Chartwrapper.Style.Add("width", (dt.Rows.Count * 40).ToString & "px")
            ElseIf dt.Rows.Count > 14 Then
                Chartwrapper.Style.Add("width", (dt.Rows.Count * 35).ToString & "px")
            Else
                Chartwrapper.Style.Add("width", "1000px")
            End If
            If dt.Rows.Count > 0 Then
                Chart.Visible = True
            Else
                Chart.Visible = True
            End If
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub BindData()
        Try

            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetEmpIncentive(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, ddl_empcode.SelectedValue, 2018)
            gvRep.DataSource = dt
            gvRep.DataBind()


            'rpbFilter.Items(0).Expanded = False
            'Args.Visible = False

            'Dim vantxt As String = ""
            'Dim van As String = ""
            'Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems
            'For Each li As RadComboBoxItem In collection
            '    van = van & li.Value & ","
            '    vantxt = vantxt & li.Text & ","

            'Next
            'If vantxt <> "" Then
            '    vantxt = vantxt.Substring(0, vantxt.Length - 1)
            'End If
            'If van = "" Then
            '    van = "0"
            'End If

            'Dim Orgtxt As String = ""
            'Dim Org As String = ""
            'Dim Orgcollection As IList(Of RadComboBoxItem) = ddlOrganization.CheckedItems
            'For Each li As RadComboBoxItem In Orgcollection
            '    Org = Org & li.Value & ","
            '    Orgtxt = Orgtxt & li.Text & ","
            'Next
            'If Orgtxt <> "" Then
            '    Orgtxt = Orgtxt.Substring(0, Orgtxt.Length - 1)
            'End If
            'If Org = "" Then
            '    Org = "0"
            'End If

            'lbl_org.Text = Orgtxt

            'If van = "0" Then
            '    lbl_van.Text = "All"
            'Else
            '    lbl_van.Text = vantxt
            'End If


            'lbl_org.Text = Orgtxt



            'lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            'lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")

            'Args.Visible = True


            'Dim objUserAccess As UserAccess
            'objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            'Dim ObjReport As New SalesWorx.BO.Common.Reports
            'Dim dt As New DataTable
            'dt = ObjReport.GetCoveredvsBilled(Err_No, Err_Desc, Org, txtFromDate.SelectedDate, txtToDate.SelectedDate, van)
            'If CType(Session.Item("CONTROL_PARAMS"), ControlParams).USE_DISTR_IN_CALLS = "Y" Then
            '    gvRep.Columns(1).HeaderTooltip = "Unique Outlets at which Distribution Check was done"
            'Else
            '    gvRep.Columns(1).HeaderTooltip = "Unique Outlets visited"

            'End If
            'gvRep.DataSource = dt
            'gvRep.DataBind()




        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If ddlOrganization.CheckedItems Is Nothing Then
            MessageBoxValidation("Select an Organisation", "Validation")
            SetFocus(ddlOrganization)
            bretval = False
            Return bretval

        Else
            If ddlOrganization.CheckedItems.Count <= 0 Then
                MessageBoxValidation("Select an Organisation", "Validation")
                SetFocus(ddlOrganization)
                bretval = False
                Return bretval
            End If
        End If
        bretval = True
        Return bretval
    End Function
    Sub MessageBoxValidation(ByVal str As String, Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    'Private Sub ddlOrganization_CheckAllCheck(sender As Object, e As RadComboBoxCheckAllCheckEventArgs) Handles ddlOrganization.CheckAllCheck
    '    LoadVan()
    '    repdiv.Visible = False
    'End Sub

    'Private Sub ddlOrganization_ItemChecked(sender As Object, e As RadComboBoxItemEventArgs) Handles ddlOrganization.ItemChecked
    '    LoadVan()
    '    repdiv.Visible = False
    'End Sub



    Private Sub gvRep_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        BindData()
    End Sub

    Private Sub gvRep_SortCommand(sender As Object, e As GridSortCommandEventArgs) Handles gvRep.SortCommand
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
    Private Sub BtnExportExcel_Click(sender As Object, e As EventArgs) Handles BtnExportExcel.Click
        Try
            If ddlOrganization.SelectedValue = "0" Then
                MessageBoxValidation("Select an Organisation", "Validation")
                Exit Sub
            End If
            If ddl_empcode.SelectedIndex <= 0 Then
                MessageBoxValidation("Select an Emp", "Validation")
                Exit Sub
            End If
            Export("Excel")

        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub
    Sub Export(format As String)

        Try


            Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
            rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim Searchvalue As New ReportParameter

            Dim fromdate As DateTime
            Dim todate As DateTime

            '  fromdate = CDate(txtFromDate.SelectedDate)
            '  todate = CDate(txtToDate.SelectedDate)

            'Dim Orgtxt As String = ""
            'Dim Org As String = ""
            'Dim Orgcollection As IList(Of RadComboBoxItem) = ddlOrganization.CheckedItems
            'For Each li As RadComboBoxItem In Orgcollection
            '    Org = Org & li.Value & ","
            '    Orgtxt = Orgtxt & li.Text & ","
            'Next
            'If Orgtxt <> "" Then
            '    Orgtxt = Orgtxt.Substring(0, Orgtxt.Length - 1)
            'End If
            'If Org = "" Then
            '    Org = "0"
            'End If







            'Dim vantxt As String = ""
            'Dim van As String = ""
            'Dim collection As IList(Of RadComboBoxItem) = "" ' ddlVan.CheckedItems
            'For Each li As RadComboBoxItem In collection
            '    van = van & li.Value & ","
            '    vantxt = vantxt & li.Text & ","

            'Next
            'If vantxt <> "" Then
            '    vantxt = vantxt.Substring(0, vantxt.Length - 1)
            'End If
            'If van = "" Then
            '    van = "0"
            'End If


            'If van = "" Then
            '    vantxt = "All"
            'End If

            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("OrgId", ddlOrganization.SelectedItem.Value)



            Dim EMP_CODE As New ReportParameter
            EMP_CODE = New ReportParameter("EMP_CODE", ddl_empcode.SelectedValue)


            Dim YEAR As New ReportParameter
            YEAR = New ReportParameter("YEAR", "2018")


            Dim OrgName As New ReportParameter
            OrgName = New ReportParameter("OrgName", ddlOrganization.SelectedItem.Text)


            Dim EmpName As New ReportParameter
            EmpName = New ReportParameter("EmpName", ddl_empcode.SelectedItem.Text)

            rview.ServerReport.SetParameters(New ReportParameter() {OrgId, EMP_CODE, YEAR, OrgName, EmpName})

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
                Response.AddHeader("Content-disposition", "attachment;filename=EmpIncentive.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=EmpIncentive.xls")
                Response.AddHeader("Content-Length", bytes.Length)
            End If
            Response.OutputStream.Write(bytes, 0, bytes.Length)
            Response.OutputStream.Flush()
            Response.OutputStream.Close()
            Response.Flush()
            Response.Close()
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub BtnExportPDF_Click(sender As Object, e As EventArgs) Handles BtnExportPDF.Click
        Try
            If ddlOrganization.SelectedValue = "0" Then
                MessageBoxValidation("Select an Organisation", "Validation")
                Exit Sub
            End If
            If ddl_empcode.SelectedIndex <= 0 Then
                MessageBoxValidation("Select an Emp", "Validation")
                Exit Sub
            End If
            Export("PDF")

        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ObjCommon = New SalesWorx.BO.Common.Common()
        Dim CountryTbl As DataTable = Nothing
        Dim orgTbl As DataTable = Nothing

        CountryTbl = ObjCommon.GetCountry(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)
        'ddlCountry.DataSource = CountryTbl
        'ddlCountry.DataBind()


        Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
        orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)

        Dim s() As String = Nothing
        Dim Currency As String = Nothing
        Dim DecimalDigits As String = "2"
        Dim country As String = Nothing
        If CountryTbl.Rows.Count = 1 Then

            'ddlCountry.SelectedIndex = 0
            'dvCountry.Visible = False

            '  s = ddlCountry.SelectedValue.Split("$")

            If s.Length > 0 Then
                country = s(0).ToString()
                Currency = s(1).ToString()
                DecimalDigits = s(2).ToString()
            End If


            ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
            ddlOrganization.DataBind()


        ElseIf CountryTbl.Rows.Count > 1 Then
            '  ddlCountry.SelectedIndex = 0
            ' dvCountry.Visible = True


            ' s = ddlCountry.SelectedValue.Split("$")

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
        'LoadVan()
        ' txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        'txtToDate.SelectedDate = Now()
        gvRep.Visible = False
        Args.Visible = False
        Chart.Visible = False
        Chartwrapper.Visible = False
    End Sub

    Private Sub ddlOrganization_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlOrganization.SelectedIndexChanged
        If Not (ddlOrganization.SelectedItem.Value = "0") Then



            Loademp()


        End If
    End Sub
    Private Sub BtnExportBiffExcel_Click(sender As Object, e As EventArgs) Handles BtnExportBiffExcel.Click

        Dim tblData As New DataTable
        'Dim objUserAccess As UserAccess
        'objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        'Dim ObjReport As New SalesWorx.BO.Common.Reports

        'Dim Org As String = ""
        'Dim Orgcollection As IList(Of RadComboBoxItem) = ddlOrganization.CheckedItems
        'For Each li As RadComboBoxItem In Orgcollection
        '    Org = Org & li.Value & ","

        'Next

        'If Org = "" Then
        '    Org = "0"
        'End If



        'Dim SearchQuery As String = ""
        'ObjCommon = New SalesWorx.BO.Common.Common()

        'Dim vantxt As String = ""
        'Dim van As String = ""
        'Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems
        'For Each li As RadComboBoxItem In collection
        '    van = van & li.Value & ","
        '    vantxt = vantxt & li.Text & ","

        'Next
        'If vantxt <> "" Then
        '    vantxt = vantxt.Substring(0, vantxt.Length - 1)
        '    van = van.Substring(0, van.Length - 1)
        'End If
        'If van = "" Then
        '    van = "0"
        'End If


        'If van = "0" Then
        '    lbl_van.Text = "All"
        'Else
        '    lbl_van.Text = vantxt
        'End If




        'If Val(van) = "0" Then
        '    SearchQuery = SearchQuery & " And A.SalesRep_ID in (" & ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID) & ")"
        'Else
        '    SearchQuery = SearchQuery & " And A.SalesRep_ID in (select item from SplitQuotedString('" & van & "'))"
        'End If


        'If ddlCustomer.SelectedValue <> "" Then
        '    SearchQuery = SearchQuery & " AND (LTRIM(STR(A.Customer_ID)) + '$' + LTRIM(STR(A.Site_Use_ID)))='" & ddlCustomer.SelectedValue & "' "
        '    lbl_Customer.Text = ddlCustomer.Text
        'Else
        '    lbl_Customer.Text = "All"
        'End If
        'If Not txtFromDate.SelectedDate Is Nothing Then
        '    SearchQuery = SearchQuery & " And A.Checked_On >=convert(datetime,'" & CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy") & "',103)"
        'End If
        'If Not txtToDate.SelectedDate Is Nothing Then
        '    SearchQuery = SearchQuery & " And A.Checked_On <=convert(datetime,'" & CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy") & " 23:59:59',103)"
        'End If

        'lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
        'lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")

        'lbl_org.Text = ddlOrganization.SelectedItem.Text

        'rpbFilter.Items(0).Expanded = False

        '' Expiry


        Dim ObjReport As New SalesWorx.BO.Common.Reports
        Dim dt As New DataTable
        dt = ObjReport.GetEmpIncentive(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, ddl_empcode.SelectedValue, 2018)
        tblData = dt.DefaultView.ToTable(False, "Incentive_Year", "Tmonth", "NET_SALES_VALUE", "NET_SALES_VOLUME", "SUCCESSFUL_VISITS", "Sales_Value_Acheived", "Sales_Volume_Acheived", "Success_Visits_Acheived", "TotalCommission", "CommissionTarget")


        For Each col In tblData.Columns
            If col.ColumnName = "Incentive_Year" Then
                col.ColumnName = "Year"
            End If

            If col.ColumnName = "Tmonth" Then
                col.ColumnName = "Month"
            End If
            If col.ColumnName = "NET_SALES_VALUE" Then
                col.ColumnName = "Target Sales VALUE"
            End If
            If col.ColumnName = "NET_SALES_VOLUME" Then
                col.ColumnName = "Target Sales VOLUME"
            End If
            If col.ColumnName = "SUCCESSFUL_VISITS" Then
                col.ColumnName = "Target VISITS"
            End If
            If col.ColumnName = "Sales_Value_Acheived" Then
                col.ColumnName = "Acheived Sales Value"
            End If
            If col.ColumnName = "Sales_Volume_Acheived" Then
                col.ColumnName = "Acheived Sales Volume"
            End If
            If col.ColumnName = "Success_Visits_Acheived" Then
                col.ColumnName = "Acheived Visits"
            End If
            If col.ColumnName = "TotalCommission" Then
                col.ColumnName = "Total Commission"
            End If
            If col.ColumnName = "CommissionTarget" Then
                col.ColumnName = "Commission Target"
            End If
           
        Next





        If tblData.Rows.Count > 0 Then

            Using package As New ExcelPackage()
                ' add a new worksheet to the empty workbook
                Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
                Worksheet.Cells("A1").LoadFromDataTable(tblData, True)
                'Worksheet.Column(3).Style.Numberformat.Format = "dd-MMM-yyyy"
                'Worksheet.Column(12).Style.Numberformat.Format = "dd-MMM-yyyy"
                Worksheet.Cells.AutoFitColumns()

                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename= Incentive.xlsx")

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
End Class