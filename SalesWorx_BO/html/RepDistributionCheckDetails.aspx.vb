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

Partial Public Class RepDistributionCheckDetails
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer
    Private ReportPath As String = "DistributionCheckList"

    Private Const PageID As String = "P421"
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
        ElseIf txtFromDate.SelectedDate > txtToDate.SelectedDate Then
            MessageBoxValidation("Enter a valid From and To Date", "Validation")
            Return bretval
        Else
            Return True
        End If

        

    End Function

    Private Sub BindData()
        gvRep_ASR.Visible = False
        gvRep.Visible = False
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


            'If Val(van) = "0" Then
            '    SearchQuery = SearchQuery & " And A.SalesRep_ID in (" & ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID) & ")"
            'Else
            '    SearchQuery = SearchQuery & " And A.SalesRep_ID in (select item from SplitQuotedString('" & van & "'))"
            'End If


            If ddlCustomer.SelectedValue <> "" Then
                SearchQuery = SearchQuery & " AND (LTRIM(STR(A.Customer_ID)) + '$' + LTRIM(STR(A.Site_Use_ID)))='" & ddlCustomer.SelectedValue & "' "
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
            ' dt = ObjReport.GetDistributionCheckDetails(Err_No, Err_Desc, ddlOrganization.SelectedValue, SearchQuery, van, CType(Session.Item("USER_ACCESS"), UserAccess).UserID)
            dt = ObjReport.GetDistributionCheckDetails(Err_No, Err_Desc, ddlOrganization.SelectedValue, SearchQuery, van, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy") & " 23:59:59", CType(Session.Item("USER_ACCESS"), UserAccess).UserID)

            Dim ClientCode As String
            ClientCode = (New SalesWorx.BO.Common.Common).GetAppConfig(Err_No, Err_Desc, "CLIENT_CODE")

            'log.Debug("ClientCode=" & ClientCode)
            'log.Debug(CType(Session.Item("CONTROL_PARAMS"), ControlParams).CLIENT_CODE)




            ''   If CType(Session.Item("CONTROL_PARAMS"), ControlParams).CLIENT_CODE = "ASR" Then
            If ClientCode = "ASR" Then

                For Each row As DataRow In dt.Rows

                    If row("Is_Available").Trim() = "1" And row("ExitInfo").Trim() = "1" Then
                        row("ExitVal") = "1"
                    ElseIf row("Is_Available").Trim() = "0" And row("ExitInfo").Trim() = "1" Then
                        row("ExitVal") = "1"
                    ElseIf row("Is_Available").Trim() = "1" And row("ExitInfo").Trim() = "0" Then
                        row("ExitVal") = "1"
                    Else
                        row("ExitVal") = "0"
                    End If

                Next

                gvRep_ASR.Visible = True
                gvRep.Visible = False
                gvRep_ASR.DataSource = dt
                gvRep_ASR.DataBind()
            Else
                gvRep_ASR.Visible = False
                gvRep.Visible = True
                gvRep.DataSource = dt
                gvRep.DataBind()
            End If





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
        Dim SHOW_UOM_MSG_BO_REPORTS As String = "N"
        Dim dt_app As New DataTable
        dt_app = (New SalesWorx.BO.Common.Common).GetAppControl(Err_No, Err_Desc, "SHOW_UOM_MSG_BO_REPORTS")
        If dt_app.Rows.Count > 0 Then
            SHOW_UOM_MSG_BO_REPORTS = dt_app.Rows(0)("Control_Value").ToString().ToUpper()
            If SHOW_UOM_MSG_BO_REPORTS = "Y" Then
                lblmsgUOM.Text = "All the quantities displayed in this report are in Stock UOM"
            Else
                lblmsgUOM.Text = ""

            End If
        End If
    End Sub

    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        If Not (ddlOrganization.SelectedItem.Value = "") Then

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


            
            If ddlCustomer.SelectedValue <> "" Then
                SearchQuery = SearchQuery & " AND (LTRIM(STR(A.Customer_ID)) + '$' + LTRIM(STR(A.Site_Use_ID)))='" & ddlCustomer.SelectedValue & "' "
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
        lblmsgUOM.Text = ""
    End Sub



    Private Sub gvRep_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles gvRep.ItemDataBound
        Try
            If TypeOf e.Item Is GridDataItem Then
                Dim item As GridDataItem = TryCast(e.Item, GridDataItem)
                Dim btnimg_entry As ImageButton = TryCast(item.FindControl("btnimg_entry"), ImageButton)
                Dim btnimg_Exit As ImageButton = TryCast(item.FindControl("btnimg_Exit"), ImageButton)

                Dim entry_isavil As String = ""
                entry_isavil = CType(item.FindControl("HEntry_Is_Available"), HiddenField).Value
                If entry_isavil.ToUpper().Trim() = "YES" Then
                    btnimg_entry.ImageUrl = "~/images/yes.jpg"
                Else
                    btnimg_entry.ImageUrl = "~/images/no.jpg"
                End If

                Dim exit_isavil As String = ""
                Dim exitInfo As String = ""
                exit_isavil = CType(item.FindControl("HExit_Is_Available"), HiddenField).Value
                exitInfo = CType(item.FindControl("HEntry_ExitInfo"), HiddenField).Value


                If exit_isavil.ToUpper().Trim() = "YES" And exitInfo.ToUpper().Trim() = "YES" Then
                    btnimg_Exit.ImageUrl = "~/images/yes.jpg"

                ElseIf exit_isavil.ToUpper().Trim() = "NO" And exitInfo.ToUpper().Trim() = "YES" Then
                    btnimg_Exit.ImageUrl = "~/images/yes.jpg"
                ElseIf exit_isavil.ToUpper().Trim() = "YES" And exitInfo.ToUpper().Trim() = "NO" Then
                    btnimg_Exit.ImageUrl = "~/images/yes.jpg"
                Else
                    btnimg_Exit.ImageUrl = "~/images/no.jpg"
                End If


            End If
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
  
   
   
    Private Sub BtnExportBiffExcel_Click(sender As Object, e As EventArgs) Handles BtnExportBiffExcel.Click

        Dim tblData As New DataTable
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        Dim ObjReport As New SalesWorx.BO.Common.Reports

        Dim Org As String = ""
        Dim Orgcollection As IList(Of RadComboBoxItem) = ddlOrganization.CheckedItems
        For Each li As RadComboBoxItem In Orgcollection
            Org = Org & li.Value & ","

        Next

        If Org = "" Then
            Org = "0"
        End If



        Dim SearchQuery As String = ""
        ObjCommon = New SalesWorx.BO.Common.Common()

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




        If Val(van) = "0" Then
            SearchQuery = SearchQuery & " And A.SalesRep_ID in (" & ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID) & ")"
        Else
            SearchQuery = SearchQuery & " And A.SalesRep_ID in (select item from SplitQuotedString('" & van & "'))"
        End If


        If ddlCustomer.SelectedValue <> "" Then
            SearchQuery = SearchQuery & " AND (LTRIM(STR(A.Customer_ID)) + '$' + LTRIM(STR(A.Site_Use_ID)))='" & ddlCustomer.SelectedValue & "' "
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

        ' Expiry

        Dim dt As New DataTable
        '''  dt = ObjReport.GetDistributionCheckDetails(Err_No, Err_Desc, ddlOrganization.SelectedValue, SearchQuery, van, CType(Session.Item("USER_ACCESS"), UserAccess).UserID)
        dt = ObjReport.GetDistributionCheckDetails(Err_No, Err_Desc, ddlOrganization.SelectedValue, SearchQuery, van, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy") & " 23:59:59", CType(Session.Item("USER_ACCESS"), UserAccess).UserID)


        Dim ClientCode As String
        ClientCode = (New SalesWorx.BO.Common.Common).GetAppConfig(Err_No, Err_Desc, "CLIENT_CODE")

        'log.Debug("ClientCode=" & ClientCode)
        'log.Debug(CType(Session.Item("CONTROL_PARAMS"), ControlParams).CLIENT_CODE)




        ''  If CType(Session.Item("CONTROL_PARAMS"), ControlParams).CLIENT_CODE = "ASR" Then
        If ClientCode = "ASR" Then



            ''  If CType(Session.Item("CONTROL_PARAMS"), ControlParams).CLIENT_CODE = "ASR" Then

            For Each row As DataRow In dt.Rows

                If row("Is_Available").Trim() = "1" And row("ExitInfo").Trim() = "1" Then
                    row("ExitVal") = "1"
                ElseIf row("Is_Available").Trim() = "0" And row("ExitInfo").Trim() = "1" Then
                    row("ExitVal") = "1"
                ElseIf row("Is_Available").Trim() = "1" And row("ExitInfo").Trim() = "0" Then
                    row("ExitVal") = "1"
                Else
                    row("ExitVal") = "0"
                End If


                If row("Checked_Time").ToString().ToUpper().Contains("AM") Then
                    row("Checked_Time") = row("Checked_Time").ToString().ToUpper().Replace("AM", " AM")
                End If
                If row("Checked_Time").ToString().ToUpper().Contains("PM") Then
                    row("Checked_Time") = row("Checked_Time").ToString().ToUpper().Replace("PM", " PM")
                End If
            Next


            tblData = dt.DefaultView.ToTable(False, "Checked_On", "Checked_Time", "CheckedBy", "Customercode", "Customer_Name", "Item_Code", "Product_Name", "Is_Available", "ExitVal", "Qty", "Qty_2", "Display_UOM", "Expiry_Dt")




            For Each col In tblData.Columns
                If col.ColumnName = "Is_Available" Then
                    col.ColumnName = " Entry"
                End If

                If col.ColumnName = "Expiry_Dt" Then
                    col.ColumnName = "Exp Date"
                End If
                If col.ColumnName = "ExitVal" Then
                    col.ColumnName = "Exit"
                End If
                If col.ColumnName = "Item_Code" Then
                    col.ColumnName = "Item Code"
                End If
                If col.ColumnName = "Product_Name" Then
                    col.ColumnName = "Item Description"
                End If
                If col.ColumnName = "Customer_Name" Then
                    col.ColumnName = "Customer Name"
                End If
                If col.ColumnName = "Customercode" Then
                    col.ColumnName = "Customer Code"
                End If
                If col.ColumnName = "Checked_On" Then
                    col.ColumnName = "Checked On"
                End If
                If col.ColumnName = "Checked_Time" Then
                    col.ColumnName = "Checked Time"
                End If
                If col.ColumnName = "Qty" Then
                    col.ColumnName = "Qty in Shelf"
                End If
                If col.ColumnName = "Qty_2" Then
                    col.ColumnName = "Qty in Store"
                End If
                If col.ColumnName = "Display_UOM" Then
                    col.ColumnName = "UOM"
                End If
                If col.ColumnName = "CheckedBy" Then
                    col.ColumnName = "VAN"
                End If
            Next

            If tblData.Rows.Count > 0 Then

                Using package As New ExcelPackage()
                    ' add a new worksheet to the empty workbook
                    Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")



                    Worksheet.Cells("A2:M2").Merge = True
                    Worksheet.Cells("A2:M2").Value = "Distribution Check List"
                    Worksheet.Cells("A2:M2").Style.Font.Size = 14
                    Worksheet.Cells("A2:M2").Style.Font.Bold = True
                    Worksheet.Cells("A2:M2").Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center

                    Worksheet.Cells("A4").LoadFromDataTable(tblData, True)
                    Worksheet.Column(1).Style.Numberformat.Format = "dd-MMM-yyyy"

                    Worksheet.Column(13).Style.Numberformat.Format = "dd-MMM-yyyy"
                    Worksheet.Cells.AutoFitColumns()

                    Response.Clear()
                    Response.Buffer = True
                    Response.Charset = ""

                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    Response.AddHeader("content-disposition", "attachment;filename= DistributionCheckDetails.xlsx")

                    Using MyMemoryStream As New MemoryStream()
                        package.SaveAs(MyMemoryStream)
                        MyMemoryStream.WriteTo(Response.OutputStream)
                        Response.AddHeader("Content-Length", MyMemoryStream.Length)
                        Response.Flush()
                        Response.Close()
                    End Using
                End Using
            End If
        Else
            tblData = dt.DefaultView.ToTable(False, "Customercode", "Customer_Name", "Checked_On", "CheckedBy", "Product_Name", "Item_Code", "Is_Available", "Exitinfo", "Qty", "Qty_2", "InvoiceQty", "Expiry_Dt")
            For Each row_ex In tblData.Rows
                If row_ex("Is_Available").ToString().Trim().ToUpper() = "YES" And row_ex("ExitInfo").ToString().Trim().ToUpper() = "YES" Then
                    row_ex("ExitInfo") = "Yes"
                ElseIf row_ex("Is_Available").ToString().Trim().ToUpper() = "NO" And row_ex("ExitInfo").ToString().Trim().ToUpper() = "YES" Then
                    row_ex("ExitInfo") = "Yes"
                ElseIf row_ex("Is_Available").ToString().Trim().ToUpper() = "YES" And row_ex("ExitInfo").ToString().Trim().ToUpper() = "NO" Then
                    row_ex("ExitInfo") = "Yes"
                Else
                    row_ex("ExitInfo") = "No"
                End If
            Next
            For Each col In tblData.Columns
                If col.ColumnName = "Is_Available" Then
                    col.ColumnName = " Entry"
                End If

                If col.ColumnName = "Expiry_Dt" Then
                    col.ColumnName = "Expiry"
                End If
                If col.ColumnName = "ExitInfo" Then
                    col.ColumnName = "Exit"
                End If
                If col.ColumnName = "Item_Code" Then
                    col.ColumnName = "Product Code"
                End If
                If col.ColumnName = "Product_Name" Then
                    col.ColumnName = "Product Name"
                End If
                If col.ColumnName = "Customer_Name" Then
                    col.ColumnName = "Customer Name"
                End If
                If col.ColumnName = "Customercode" Then
                    col.ColumnName = "Customer Code"
                End If
                If col.ColumnName = "Checked_On" Then
                    col.ColumnName = "Checked On"
                End If
                If col.ColumnName = "Qty" Then
                    col.ColumnName = "Qty in Shelf"
                End If
                If col.ColumnName = "Qty_2" Then
                    col.ColumnName = "Qty in Store"
                End If
                If col.ColumnName = "InvoiceQty" Then
                    col.ColumnName = "Invoice Qty"
                End If
                If col.ColumnName = "CheckedBy" Then
                    col.ColumnName = "Checked By"
                End If
            Next

            If tblData.Rows.Count > 0 Then

                Using package As New ExcelPackage()
                    ' add a new worksheet to the empty workbook
                    Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
                    Worksheet.Cells("A1").LoadFromDataTable(tblData, True)
                    Worksheet.Column(3).Style.Numberformat.Format = "dd-MMM-yyyy"
                    Worksheet.Column(12).Style.Numberformat.Format = "dd-MMM-yyyy"
                    Worksheet.Cells.AutoFitColumns()

                    Response.Clear()
                    Response.Buffer = True
                    Response.Charset = ""

                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    Response.AddHeader("content-disposition", "attachment;filename= DistributionCheckDetails.xlsx")

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
    Private Sub gvRep_ASR_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvRep_ASR.PageIndexChanged
        BindData()
    End Sub

    Private Sub gvRep_ASR_SortCommand(sender As Object, e As GridSortCommandEventArgs) Handles gvRep_ASR.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindData()
    End Sub
End Class