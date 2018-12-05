Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Imports System.Linq
Imports OfficeOpenXml

Public Class Rep_ZeroBilledCustomers
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As New SalesWorx.BO.Common.Common
    Dim ObjCollection As Collection
    Private ReportPath As String = "ZeroBilledCustomers"
    Private Const PageID As String = "P422"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
            Try
                ObjCommon = New SalesWorx.BO.Common.Common()
                Dim HasPermission As Boolean = False
                ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
                If Not HasPermission Then
                    Err_No = 500
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
                End If

                ObjCommon = New SalesWorx.BO.Common.Common()
                Dim CountryTbl As DataTable = Nothing
                Dim orgTbl As DataTable = Nothing

                ZeroBilledSlab.Visible = False

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

                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                txtToDate.SelectedDate = Now()


                LoadorgDetails()

                'BindData()
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
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Sub LoadorgDetails()
        Try


            If Not ddlOrganization.CheckedItems Is Nothing Then
                If ddlOrganization.CheckedItems.Count > 0 Then
                    Dim objUserAccess As UserAccess
                    objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
                    Dim ObjRep As SalesWorx.BO.Common.Reports = New SalesWorx.BO.Common.Reports()

                    Dim OrgStr As String = ""
                    For Each li As RadComboBoxItem In ddlOrganization.CheckedItems
                        OrgStr = OrgStr & li.Value & ","
                    Next
                    ddVan.DataSource = ObjRep.GetAllOrgVan(Err_No, Err_Desc, OrgStr, objUserAccess.UserID.ToString())
                    ddVan.DataBind()

                    For Each itm As RadComboBoxItem In ddVan.Items
                        itm.Checked = True
                    Next

                    Dim dtcurrency As DataTable
                    Dim ObjReport As New SalesWorx.BO.Common.Reports
                    dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.CheckedItems(0).Value)


                Else
                    ddVan.Items.Clear()
                End If
            Else
                ddVan.Items.Clear()
            End If
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Private Sub BtnExport_Click(sender As Object, e As EventArgs) Handles BtnExportExcel.Click
        Try
            If ValidateInputs() Then
                ExportBiffExcel() ' Export("Excel")
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
    Function ValidateInputs() As Boolean
        Try
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
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Function

    Sub Export(format As String)




        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)


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

        Dim OrgId As New ReportParameter
        OrgId = New ReportParameter("OrgID", Org)

        Dim OrgName As New ReportParameter
        OrgName = New ReportParameter("OrgName", Orgtxt)

        Dim collection As IList(Of RadComboBoxItem) = ddVan.CheckedItems
        Dim van As String = ""
        For Each li As RadComboBoxItem In collection
            van = van & li.Value & ","

        Next

        If van = "" Then
            van = "0"
        End If

        Dim SID As New ReportParameter
        SID = New ReportParameter("SID", van)

        Dim FromDate As New ReportParameter
        FromDate = New ReportParameter("FromDate", CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"))


        Dim ToDate As New ReportParameter
        ToDate = New ReportParameter("Todate", CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))

        'Dim UID As New ReportParameter
        'UID = New ReportParameter("UID", objUserAccess.UserID)

      

        rview.ServerReport.SetParameters(New ReportParameter() {OrgId, SID, FromDate, ToDate, OrgName})

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
            Response.AddHeader("Content-disposition", "attachment;filename=Zerbobilled.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=Zerbobilled.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()

    End Sub

    Private Sub dgvPDC_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvRep.SortCommand
        Try
            ViewState("SortDirection") = e.SortExpression
            SortDirection = "flip"
            BindReport()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Private Sub dgvPDC_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        Try
            BindReport()
        Catch ex As Exception
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

    Private Sub ddlOrganization_CheckAllCheck(sender As Object, e As RadComboBoxCheckAllCheckEventArgs) Handles ddlOrganization.CheckAllCheck
        Try
            LoadorgDetails()
            repdiv.Visible = False
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try

    End Sub

    Private Sub ddlOrganization_ItemChecked(sender As Object, e As RadComboBoxItemEventArgs) Handles ddlOrganization.ItemChecked
        Try
            LoadorgDetails()
            repdiv.Visible = False
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try

    End Sub

    Private Sub BindReport()
        Try


            rpbFilter.Items(0).Expanded = False
            Args.Visible = False

            Dim collection As IList(Of RadComboBoxItem) = ddVan.CheckedItems

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

            hfVan.Value = van
            hfOrg.Value = Org
            UId.Value = CType(Session.Item("USER_ACCESS"), UserAccess).UserID
            HFrom.Value = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            Hto.Value = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")

            Args.Visible = True

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetZerobilled_Planned(Err_No, Err_Desc, Org, van, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"), CType(Session.Item("USER_ACCESS"), UserAccess).UserID)
            gvRep.DataSource = dt
            gvRep.DataBind()

            Dim dt_visited As New DataTable
            dt_visited = ObjReport.GetZerobilled_NotVisited(Err_No, Err_Desc, Org, van, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"), CType(Session.Item("USER_ACCESS"), UserAccess).UserID)
            gvRep_visited.DataSource = dt_visited
            gvRep_visited.DataBind()

            Dim dt_billed As New DataTable
            dt_billed = ObjReport.GetZerobilled_NotBilled(Err_No, Err_Desc, Org, van, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"), CType(Session.Item("USER_ACCESS"), UserAccess).UserID)
            gvRep_Billed.DataSource = dt_billed
            gvRep_Billed.DataBind()

        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        Try
            If ValidateInputs() Then
                gvRep.Visible = True
                repdiv.Visible = True
                ZeroBilledSlab.Visible = True
                ZeroBilledSlab.Tabs(0).Selected = True
                RadMultiPage21.PageViews(0).Selected = True
                BindReport()


            Else
                Args.Visible = False

                gvRep.Visible = False
                repdiv.Visible = False
            End If
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        Try


            ObjCommon = New SalesWorx.BO.Common.Common()
            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            If Not HasPermission Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If

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
            ddVan.ClearCheckedItems()
            ddVan.Items.Clear()
            LoadorgDetails()
            txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
            txtToDate.SelectedDate = Now()
            gvRep.Visible = False
            Args.Visible = False
            repdiv.Visible = False
            ZeroBilledSlab.Visible = False
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Private Sub ddlCountry_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlCountry.SelectedIndexChanged
        Try
            LoadOrgs()
            LoadorgDetails()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Sub LoadOrgs()
        Try
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
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Private Sub gvRep_visited_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvRep_visited.PageIndexChanged
        Try
            BindReport()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Private Sub gvRep_visited_SortCommand(sender As Object, e As GridSortCommandEventArgs) Handles gvRep_visited.SortCommand
        Try
            ViewState("SortDirection") = e.SortExpression
            SortDirection = "flip"
            BindReport()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Private Sub gvRep_Billed_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvRep_Billed.PageIndexChanged
        Try
            BindReport()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Private Sub gvRep_Billed_SortCommand(sender As Object, e As GridSortCommandEventArgs) Handles gvRep_Billed.SortCommand
        Try
            ViewState("SortDirection") = e.SortExpression
            SortDirection = "flip"
            BindReport()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Sub ExportBiffExcel()
        Try
            Dim collection As IList(Of RadComboBoxItem) = ddVan.CheckedItems

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

            hfVan.Value = van
            hfOrg.Value = Org
            UId.Value = CType(Session.Item("USER_ACCESS"), UserAccess).UserID
            HFrom.Value = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            Hto.Value = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")

            Args.Visible = True

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjReport As New SalesWorx.BO.Common.Reports

            Dim dt As New DataTable
            Dim tblData_panned As New DataTable
            dt = ObjReport.GetZerobilled_Planned(Err_No, Err_Desc, Org, van, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"), CType(Session.Item("USER_ACCESS"), UserAccess).UserID)
            tblData_panned = dt.DefaultView.ToTable(False, "Van", "vdate", "Customer_No", "Customer", "VistedYES", "BilledYES", "Visited", "Billed", "NetSales")

            ' tblData_panned.Columns("SalesRep_Name").ColumnName = "VAN"
            tblData_panned.Columns("vdate").ColumnName = "Plan Date"
            tblData_panned.Columns("Customer_No").ColumnName = "Customer No"
            tblData_panned.Columns("Customer").ColumnName = "Customer Name"
            tblData_panned.Columns("Visited").ColumnName = "Visits"
            tblData_panned.Columns("Billed").ColumnName = "Productive Visits"
            tblData_panned.Columns("VistedYES").ColumnName = "Visited"
            tblData_panned.Columns("BilledYES").ColumnName = "Billed"
            tblData_panned.Columns("NetSales").ColumnName = "Net Sales"

            Dim dt_visited As New DataTable
            Dim tblData_Nvisited As New DataTable
            dt_visited = ObjReport.GetZerobilled_NotVisited(Err_No, Err_Desc, Org, van, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"), CType(Session.Item("USER_ACCESS"), UserAccess).UserID)
            tblData_Nvisited = dt_visited.DefaultView.ToTable(False, "Van", "vdate", "Customer_No", "Customer")
            'tblData_Nvisited.Columns("SalesRep_Name").ColumnName = "VAN"
            tblData_Nvisited.Columns("vdate").ColumnName = "Plan Date"
            tblData_Nvisited.Columns("Customer_No").ColumnName = "Customer No"
            tblData_Nvisited.Columns("Customer").ColumnName = "Customer Name"



            Dim dt_billed As New DataTable
            Dim tblData_Nbilled As New DataTable
            dt_billed = ObjReport.GetZerobilled_NotBilled(Err_No, Err_Desc, Org, van, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"), CType(Session.Item("USER_ACCESS"), UserAccess).UserID)
            tblData_Nbilled = dt_billed.DefaultView.ToTable(False, "Van", "vdate", "Customer_No", "Customer")
            'tblData_Nbilled.Columns("SalesRep_Name").ColumnName = "VAN"
            tblData_Nbilled.Columns("vdate").ColumnName = "Plan Date"
            tblData_Nbilled.Columns("Customer_No").ColumnName = "Customer No"
            tblData_Nbilled.Columns("Customer").ColumnName = "Customer Name"





            If tblData_panned.Rows.Count > 0 Then

                Using package As New ExcelPackage()
                    ' add a new worksheet to the empty workbook
                    Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
                    Worksheet.Name = "Planned Customers"


                    'Worksheet.Cells("A2:M2").Merge = True
                    'Worksheet.Cells("A2:M2").Value = "Distribution Check List"
                    Worksheet.Cells("A1:I1").Style.Font.Size = 12
                    Worksheet.Cells("A1:I1").Style.Font.Bold = True
                    'Worksheet.Cells("C").Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center
                    'Worksheet.Cells("A{row}").Style.VerticalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center



                    Worksheet.Cells("A1").LoadFromDataTable(tblData_panned, True)
                    Worksheet.Column(2).Style.Numberformat.Format = "dd-MMM-yyyy"
                    Worksheet.Column(9).Style.Numberformat.Format = "#,##0.00"
                    Worksheet.Cells.AutoFitColumns()


                    Dim Worksheet2 As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet2")
                    Worksheet2.Name = "Not Visited Customers"
                    Worksheet2.Cells("A1:D1").Style.Font.Size = 12
                    Worksheet2.Cells("A1:D1").Style.Font.Bold = True
                    Worksheet2.Cells("A1").LoadFromDataTable(tblData_Nvisited, True)
                    Worksheet2.Column(2).Style.Numberformat.Format = "dd-MMM-yyyy"
                    Worksheet2.Cells.AutoFitColumns()


                    Dim Worksheet3 As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet3")
                    Worksheet3.Name = "Zero Billed Customers"
                    Worksheet3.Cells("A1:D1").Style.Font.Size = 12
                    Worksheet3.Cells("A1:D1").Style.Font.Bold = True
                    Worksheet3.Cells("A1").LoadFromDataTable(tblData_Nbilled, True)
                    Worksheet3.Column(2).Style.Numberformat.Format = "dd-MMM-yyyy"
                    Worksheet3.Cells.AutoFitColumns()

                    Response.Clear()
                    Response.Buffer = True
                    Response.Charset = ""

                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    Response.AddHeader("content-disposition", "attachment;filename= ZeroBilledCustomers.xlsx")

                    Using MyMemoryStream As New MemoryStream()
                        package.SaveAs(MyMemoryStream)
                        MyMemoryStream.WriteTo(Response.OutputStream)
                        Response.AddHeader("Content-Length", MyMemoryStream.Length)
                        Response.Flush()
                        Response.Close()
                    End Using
                End Using
            End If

        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try





    End Sub

End Class