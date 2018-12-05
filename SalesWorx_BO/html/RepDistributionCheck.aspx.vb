Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Partial Public Class RepDistributionCheck
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer
    Private ReportPath As String = "DistributionCheckList"

    Private Const PageID As String = "P207"
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
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))
                txtFromDate.SelectedDate = FirstDayOfMonth(Now().Date)
                txtToDate.SelectedDate = Now().Date

                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1

                    LoadVan()
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
        End If
    End Sub
    Public Function FirstDayOfMonth(ByVal sourceDate As DateTime) As DateTime
        Return New DateTime(sourceDate.Year, sourceDate.Month, 1)
    End Function
    Sub LoadVan()
        Try
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New SalesWorx.BO.Common.Common()
            ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
            ddlVan.DataValueField = "SalesRep_ID"
            ddlVan.DataTextField = "SalesRep_Name"
            ddlVan.DataBind()

            For Each itm As RadComboBoxItem In ddlVan.Items
                itm.Checked = True
            Next

            ''  ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --"))

        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
    Private Sub ddlCustomer_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddlCustomer.ItemsRequested

        Dim ObjCommon As New SalesWorx.BO.Common.Common()
        Try
            Dim dt As New DataTable


            ddlCustomer.Items.Clear()

            dt = ObjCommon.GetCustomerByCriteriaandText(Err_No, Err_Desc, ddlOrganization.SelectedValue, "", e.Text)

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
        Finally
            ObjCommon = Nothing
        End Try
    End Sub
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If ddlOrganization.SelectedValue = "0" Then
            MessageBoxValidation("Select an Organization.", "Validation")
            Return bretval
        End If
        If txtFromDate.SelectedDate Is Nothing Then
            MessageBoxValidation("Enter a valid From Date", "Validation")
            Return bretval
        End If
        If txtToDate.SelectedDate Is Nothing Then
            MessageBoxValidation("Enter a valid To Date", "Validation")
            Return bretval
        Else
            Return True
        End If

    End Function

    Private Sub BindData()
        Dim SearchQuery As String = ""

        Dim ObjReport As New SalesWorx.BO.Common.Reports
        ObjCommon = New SalesWorx.BO.Common.Common()
        Try

            Dim vantxt As String = ""
            Dim van As String = ""
            Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","
                vantxt = vantxt & li.Text & ","

            Next
            If vantxt <> "" Then
                vantxt = vantxt.Substring(0, vantxt.Length - 1)
                van = van.Substring(0, van.Length - 1)
            End If
            If van = "" Then
                van = "0"
            End If


            If van = "0" Then
                lbl_van.Text = "All"
            Else
                lbl_van.Text = vantxt
            End If

            ''If ddlVan.SelectedValue <> "" Then
            ''    SearchQuery = " And A.SalesRep_ID=" & ddlVan.SelectedValue
            ''    lbl_van.Text = ddlVan.Text
            ''Else
            ''    SearchQuery = " And A.SalesRep_ID in (" & ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID) & ")"
            ''    lbl_van.Text = "All"
            ''End If


            If Val(van) = "0" Then
                SearchQuery = SearchQuery & " And A.SalesRep_ID in (" & ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID) & ")"
            Else
                SearchQuery = SearchQuery & " And A.SalesRep_ID in (select item from SplitQuotedString('" & van & "'))"
            End If


            If ddlCustomer.SelectedValue <> "" Then
                SearchQuery = SearchQuery & " AND (LTRIM(STR(B.Customer_ID)) + '$' + LTRIM(STR(B.Site_Use_ID)))='" & ddlCustomer.SelectedValue & "' "
                lbl_Customer.Text = ddlCustomer.Text
            Else
                lbl_Customer.Text = "All"
            End If
            If Not txtFromDate.SelectedDate Is Nothing Then
                SearchQuery = SearchQuery & " And A.Checked_On >=convert(datetime,'" & CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy") & "',103)"
            End If
            If Not txtToDate.SelectedDate Is Nothing Then
                SearchQuery = SearchQuery & " And A.Checked_On <=convert(datetime,'" & CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy") & " 23:59:59',103)"
            End If

            lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")

            lbl_org.Text = ddlOrganization.SelectedItem.Text

            rpbFilter.Items(0).Expanded = False



            Dim dt As New DataTable
            dt = ObjReport.GetDistributionCheckList(Err_No, Err_Desc, ddlOrganization.SelectedValue, SearchQuery, van, CType(Session.Item("USER_ACCESS"), UserAccess).UserID)

            gvRep.DataSource = dt
            gvRep.DataBind()

        Catch ex As Exception
            If Err_Desc IsNot Nothing Then
                log.Error(Err_Desc)
            Else
                log.Error(GetExceptionInfo(ex))
            End If
            Err_No = "74067"
            '  Err_Desc = ex.Message
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
        Finally
            ObjReport = Nothing
            ObjCommon = Nothing
        End Try
    End Sub


    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click

        gvRep.Visible = False
        Args.Visible = False
        If ValidateInputs() Then
            gvRep.Visible = True
            Args.Visible = True
            BindData()
        End If
    End Sub

    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        If Not (ddlOrganization.SelectedItem.Value = "") Then

            ' ''Dim objUserAccess As UserAccess
            ' ''objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ' ''ObjCommon = New SalesWorx.BO.Common.Common()
            ' ''ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())
            ' ''ddlVan.DataBind()

            ' ''ddlCustomer.DataSource = ObjCommon.GetCustomerByCriteria(Err_No, Err_Desc, ddlOrganization.SelectedValue)
            ' ''ddlCustomer.DataBind()
            ' ''ddlCustomer.Items.Insert(0, New RadComboBoxItem("-- Select a value --", 0))

            LoadVan()
        Else
            ddlVan.ClearSelection()
            ddlVan.Items.Clear()
            ''  ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))

            ddlCustomer.ClearSelection()
            ddlCustomer.Items.Clear()
            ddlCustomer.Text = ""
        End If

        RVMain.Reset()
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Private Sub InitReportViewer(ByVal FilterValue As String)
        Try

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim Searchvalue As New ReportParameter
            Searchvalue = New ReportParameter("SearchParams", CStr(IIf(FilterValue Is Nothing, "", FilterValue)))

            Dim VisitID As New ReportParameter
            VisitID = New ReportParameter("VisitID", "0")

            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("OrgId", CStr(ddlOrganization.SelectedItem.Value.ToString()))

            Dim FromDate As New ReportParameter
            FromDate = New ReportParameter("FromDate", CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"))

            Dim ToDate As New ReportParameter
            ToDate = New ReportParameter("ToDate", CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy") & " 23:59:59'")

            Dim SalesRep_ID As New ReportParameter
            SalesRep_ID = New ReportParameter("SalesRep_ID", CStr(ddlVan.SelectedItem.Value.ToString()))

            Dim OrgName As New ReportParameter
            OrgName = New ReportParameter("OrgName", CStr(IIf(ddlOrganization.SelectedIndex = 0, "0", ddlOrganization.SelectedItem.Text.Trim())))
            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                .ServerReport.SetParameters(New ReportParameter() {Searchvalue, VisitID, OrgId, FromDate, ToDate, SalesRep_ID, OrgName})
                .ServerReport.Refresh()

            End With


        Catch Ex As Exception
            '  log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub


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
            If ValidateInputs() Then
                Export("Excel")
            End If
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub
    Sub Export(format As String)
        Dim ObjCommon As SalesWorx.BO.Common.Common
        Try

            ObjCommon = New SalesWorx.BO.Common.Common()
            Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
            rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)



            Dim SearchQuery As String = ""


            Dim vantxt As String = ""
            Dim van As String = ""
            Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","
            Next
            If van <> "" Then
                van = van.Substring(0, van.Length - 1)
            End If
            If van = "" Then
                van = "0"
            End If




            ''      SearchQuery = ""

            If Val(van) = "0" Then
                SearchQuery = SearchQuery & " And A.SalesRep_ID in (" & ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID) & ")"
            Else
                SearchQuery = SearchQuery & " And A.SalesRep_ID in (select item from SplitQuotedString('" & van & "'))"
            End If


            ''If ddlVan.SelectedValue <> "" Then
            ''    SearchQuery = " And A.SalesRep_ID=" & ddlVan.SelectedValue
            ''    lbl_van.Text = ddlVan.Text
            ''Else
            ''    SearchQuery = " And A.SalesRep_ID in (" & ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID) & ")"
            ''    lbl_van.Text = "All"
            ''End If
            If ddlCustomer.SelectedValue <> "" Then
                SearchQuery = SearchQuery & " AND (LTRIM(STR(B.Customer_ID)) + '$' + LTRIM(STR(B.Site_Use_ID)))='" & ddlCustomer.SelectedValue & "' "
                lbl_Customer.Text = ddlCustomer.Text
            Else
                lbl_Customer.Text = "All"
            End If
            If Not txtFromDate.SelectedDate Is Nothing Then
                SearchQuery = SearchQuery & " And A.Checked_On >=convert(datetime,'" & CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy") & "',103)"
            End If
            If Not txtToDate.SelectedDate Is Nothing Then
                SearchQuery = SearchQuery & " And A.Checked_On <=convert(datetime,'" & CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy") & " 23:59:59',103)"
            End If

            Dim uid As Integer = CType(Session("User_Access"), UserAccess).UserID

            Dim Searchvalue As New ReportParameter
            Searchvalue = New ReportParameter("SearchParams", CStr(IIf(SearchQuery Is Nothing, "", SearchQuery)))

            Dim VisitID As New ReportParameter
            VisitID = New ReportParameter("VisitID", "0")

            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("OrgId", CStr(ddlOrganization.SelectedItem.Value.ToString()))

            Dim FromDate As New ReportParameter
            FromDate = New ReportParameter("FromDate", txtFromDate.SelectedDate.ToString())

            Dim ToDate As New ReportParameter
            ToDate = New ReportParameter("ToDate", txtToDate.SelectedDate.ToString())

            Dim SalesRep_ID As New ReportParameter
            SalesRep_ID = New ReportParameter("SalesRep_ID", van)

            Dim OrgName As New ReportParameter
            OrgName = New ReportParameter("OrgName", CStr(IIf(ddlOrganization.SelectedIndex = 0, "0", ddlOrganization.SelectedItem.Text.Trim())))


            Dim USRID As New ReportParameter
            USRID = New ReportParameter("UID", uid)

            Dim cid As String = "0"
            Dim sid As String = "0"
            If ddlCustomer.SelectedValue <> "" Then
                Dim ids() As String
                ids = ddlCustomer.SelectedValue.Split("$")
                cid = ids(0)
                sid = ids(1)
            End If
            Dim CustID As New ReportParameter
            CustID = New ReportParameter("CustID", cid)

            Dim SIteID As New ReportParameter
            SIteID = New ReportParameter("SIteID", sid)


            rview.ServerReport.SetParameters(New ReportParameter() {Searchvalue, VisitID, OrgId, FromDate, ToDate, SalesRep_ID, OrgName, USRID, SIteID, CustID})

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
                Response.AddHeader("Content-disposition", "attachment;filename=DistributionCheck.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=DistributionCheck.xls")
                Response.AddHeader("Content-Length", bytes.Length)
            End If
            Response.OutputStream.Write(bytes, 0, bytes.Length)
            Response.OutputStream.Flush()
            Response.OutputStream.Close()
            Response.Flush()
            Response.Close()
        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            ObjCommon = Nothing
        End Try
    End Sub

    Private Sub BtnExportPDF_Click(sender As Object, e As EventArgs) Handles BtnExportPDF.Click
        Try
            If ValidateInputs() Then
                Export("PDF")
            End If
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        ddlVan.ClearCheckedItems()
        LoadVan()
        ddlCustomer.ClearSelection()
        ddlCustomer.Text = ""
        gvRep.Visible = False
        Args.Visible = False
        txtFromDate.SelectedDate = FirstDayOfMonth(Now().Date)
        txtToDate.SelectedDate = Now().Date

    End Sub
End Class