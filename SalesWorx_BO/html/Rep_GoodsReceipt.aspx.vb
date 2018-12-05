Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI

Partial Public Class Rep_GoodsReceipt
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "GoodsReceipt"

    Private Const PageID As String = "P103"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single

    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ''If Not IsNothing(Me.Master) Then

        ''    Dim masterScriptManager As ScriptManager
        ''    masterScriptManager = CType(Master.FindControl("ScriptManager1"), ScriptManager)

        ''    ' Make sure our master page has the script manager we're looking for
        ''    If Not IsNothing(masterScriptManager) Then

        ''        ' Turn off partial page postbacks for this page
        ''        masterScriptManager.EnablePartialRendering = False
        ''    End If

        ''End If

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
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization"))
                If Not Request.QueryString("ID") Is Nothing Then
                    txtFromDate.SelectedDate = Format(DateAdd(DateInterval.Day, IIf(Day(Now) - 1 = 0, 0, -1 * (Day(Now) - 1)), Now().Date), "dd-MMM-yyyy")
                    txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")

                    Dim dt As New DataTable
                    dt = (New SalesWorx.BO.Common.Common).GetSalesOrgbyFsr(Err_No, Err_Desc, Request.QueryString("ID"))
                    If dt.Rows.Count > 0 Then
                        If Not ddlOrganization.Items.FindItemByValue(dt.Rows(0)("MAS_Org_ID").ToString) Is Nothing Then
                            ddlOrganization.ClearSelection()
                            ddlOrganization.Items.FindItemByValue(dt.Rows(0)("MAS_Org_ID").ToString).Selected = True

                            If ddlOrganization.Items.Count = 2 Then
                                ddlOrganization.SelectedIndex = 1

                                LoadVan()
                                LoadApprovedBy()
                            End If
                        End If
                    End If
                Else
                    If ddlOrganization.Items.Count = 2 Then
                        ddlOrganization.SelectedIndex = 1

                        LoadVan()
                        LoadApprovedBy()
                    End If

                    txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                    txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")
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
    Sub LoadVan()
        Try
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

            ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
            ddlVan.DataValueField = "SalesRep_ID"
            ddlVan.DataTextField = "SalesRep_Name"
            ddlVan.DataBind()
            ''  ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --"))
            If Not ddlVan.Items.FindItemByValue(Request.QueryString("ID")) Is Nothing Then
                ddlVan.ClearSelection()
                ddlVan.Items.FindItemByValue(Request.QueryString("ID")).Selected = True
            End If

            For Each itm As RadComboBoxItem In ddlVan.Items
                itm.Checked = True
            Next
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
    Sub LoadApprovedBy()
        Try
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

            ddApprovedBy.DataSource = ObjCommon.GetAllSS(Err_No, Err_Desc, ddlOrganization.SelectedValue)
            ddApprovedBy.DataValueField = "User_ID"
            ddApprovedBy.DataTextField = "Username"
            ddApprovedBy.DataBind()

        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
    Private Sub BindData()
        Dim SearchQuery As String = ""
        Dim SalesRepId As Integer = 0
        Dim CustId As Integer = 0
        Dim fromdate As DateTime
        Dim todate As DateTime
        fromdate = System.Data.SqlTypes.SqlDateTime.Null
        todate = System.Data.SqlTypes.SqlDateTime.Null


        Try
            ObjCustomer = New Customer()
            ObjCommon = New SalesWorx.BO.Common.Common()
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

            fromdate = CDate(txtFromDate.SelectedDate)
            todate = CDate(txtToDate.SelectedDate)

            If Not (ddlOrganization.SelectedItem.Value = "") Then
                InitReportViewer(fromdate, todate, CType(Session("User_Access"), UserAccess).UserID)
            Else
                MessageBoxValidation("Select an Organization.", "Validation")
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
            ObjCustomer = Nothing
        End Try
    End Sub

    Private Sub InitReportViewer(ByVal fromdate As Date, ByVal Todate As Date, ByVal UID As Integer)
        Try

            ' ''Dim objUserAccess As UserAccess
            ' ''objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

            ' ''Dim OrgID As New ReportParameter
            ' ''OrgID = New ReportParameter("OrgID", Me.ddlOrganization.SelectedValue)

            ' ''Dim RowID As New ReportParameter
            ' ''RowID = New ReportParameter("RowID", "0")

            ' ''Dim AppCode As New ReportParameter
            ' ''AppCode = New ReportParameter("ApprovalCode", "0")

            ' ''Dim OrgName As New ReportParameter
            ' ''OrgName = New ReportParameter("OrgName", CStr(ddlOrganization.SelectedItem.ToString()))


            ' ''Dim SalesRep As New ReportParameter
            ' ''SalesRep = New ReportParameter("SalesRep", ddlVan.SelectedItem.Text)

            ' ''Dim UsedFor As New ReportParameter
            ' ''UsedFor = New ReportParameter("UsedFor", "")

            ' ''Dim UsedCode As New ReportParameter
            ' ''UsedCode = New ReportParameter("UsedCode", "")

            ' ''Dim FDate As New ReportParameter
            ' ''FDate = New ReportParameter("FromDate", fromdate.ToString())

            ' ''Dim TDate As New ReportParameter
            ' ''TDate = New ReportParameter("ToDate", Todate.ToString())

            ' ''Dim SID As New ReportParameter
            ' ''SID = New ReportParameter("SID", ddlVan.SelectedValue)



            ' ''With RVMain
            ' ''    .Reset()
            ' ''    .ShowParameterPrompts = False
            ' ''    .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            ' ''    .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            ' ''    .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
            ' ''    .ServerReport.SetParameters(New ReportParameter() {OrgID, RowID, AppCode, OrgName, SalesRep, UsedFor, UsedCode, FDate, TDate, SID})
            ' ''    .ServerReport.Refresh()

            ' ''End With

            rpbFilter.Items(0).Expanded = False
            Args.Visible = False

            Dim vantxt As String = ""
            Dim van As String = ""

            Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","
                vantxt = vantxt & li.Text & ","
            Next
            If vantxt <> "" Then
                vantxt = vantxt.Substring(0, vantxt.Length - 1)
            End If


            lbl_org.Text = ddlOrganization.SelectedItem.Text
            If van = "" Then
                lbl_van.Text = "All"
            Else
                lbl_van.Text = vantxt
            End If

            If van = "" Then
                ''  van = "0"

                '' if no items selected
                For Each li As RadComboBoxItem In ddlVan.Items
                    If li.Value > 0 Then
                        van = van & li.Value & ","
                    End If

                Next
            End If

            Dim apptxt As String = ""
            Dim appby As String = ""

            Dim collection1 As IList(Of RadComboBoxItem) = ddApprovedBy.CheckedItems
            For Each li As RadComboBoxItem In collection1
                appby = appby & li.Value & ","
                apptxt = apptxt & li.Text & ","
            Next
            If apptxt <> "" Then
                apptxt = apptxt.Substring(0, apptxt.Length - 1)
            End If
            If appby = "" Then
                appby = "0"
            End If

            If appby = "0" Then
                lbl_ApprovedBy.Text = "All"
            Else
                lbl_ApprovedBy.Text = apptxt
            End If

            lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")

            Args.Visible = True


            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetGoodsReceipt(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, txtFromDate.SelectedDate, txtToDate.SelectedDate, van, "0", appby)
            gvRep.DataSource = dt
            gvRep.DataBind()

        Catch Ex As Exception
            log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub

    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        '' RVMain.Reset()

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

        Args.Visible = False
        gvRep.Visible = False
        If Not (ddlOrganization.SelectedItem.Value = "") Then

            ''If ddlVan.SelectedValue = "" Or ddlVan.SelectedValue = "0" Then
            ''    MessageBoxValidation("Select a van", "Validation")
            ''    SetFocus(txtFromDate)
            ''    Exit Sub
            ''End If


            If Not IsDate(txtFromDate.SelectedDate) Then
                MessageBoxValidation("Enter valid ""From date"".", "Validation")
                SetFocus(txtFromDate)
                Exit Sub
            End If

            If Not IsDate(txtToDate.SelectedDate) Then
                MessageBoxValidation("Enter valid ""To date"".", "Validation")
                SetFocus(txtToDate)
                Exit Sub
            End If

            If CDate(txtFromDate.SelectedDate) > CDate(txtToDate.SelectedDate) Then
                MessageBoxValidation("Start Date should not be greater than End Date.", "Validation")
                Exit Sub
            End If
            gvRep.Visible = True
            BindData()
        Else
            MessageBoxValidation("Select an organization.", "Validation")
        End If
    End Sub
    Sub MessageBoxValidation(ByVal str As String, Title As String)
        'lblMessage.ForeColor = Drawing.Color.Red
        'lblinfo.Text = "Validation"
        'lblMessage.Text = str
        'MpInfoError.Show()
        'MpInfoError.Show()
        'Exit Sub
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged


        If Not (ddlOrganization.SelectedItem.Value = "Select Organization") Then

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

            ''  RVMain.Reset()
        Else
            ddlVan.Items.Clear()
            '' ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --"))
            ''   RVMain.Reset()
        End If

    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub

    Private Sub gvRep_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles gvRep.ItemDataBound
        Try
            If TypeOf e.Item Is GridGroupHeaderItem Then
                Dim groupHeader As GridGroupHeaderItem = DirectCast(e.Item, GridGroupHeaderItem)
                If True Then
                    groupHeader.DataCell.Text = groupHeader.DataCell.Text.Split(":"c)(1).ToString()
                    If groupHeader.DataCell.Text.Trim = "-1" Then
                        groupHeader.DataCell.Text = "N/A"
                    End If
                End If
            End If
        Catch ex As Exception
            log.Error(ex.Message)
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

    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If (ddlOrganization.SelectedItem.Value = "") Then

            MessageBoxValidation("Please select the Organisation", "Validation")
            Return bretval
            'ElseIf (ddlVan.SelectedValue = "") Then
            '    MessageBoxValidation("Please select a Van", "Validation")
            '    Return bretval
        Else
            bretval = True
            Return bretval
        End If
    End Function

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

        Try


            Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
            rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim Searchvalue As New ReportParameter

            Dim vantxt As String = ""
            Dim van As String = ""
            Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","
                vantxt = vantxt & li.Text & ","

            Next
            If vantxt <> "" Then
                vantxt = vantxt.Substring(0, vantxt.Length - 1)
            End If

            If van = "" Then
                vantxt = "All"
            End If

            If van = "" Then
                ''  van = "0"

                '' if no items selected
                For Each li As RadComboBoxItem In ddlVan.Items
                    If li.Value > 0 Then
                        van = van & li.Value & ","
                    End If

                Next

            End If


          

            Dim fromdate As DateTime
            Dim todate As DateTime

            fromdate = CDate(txtFromDate.SelectedDate)
            todate = CDate(txtToDate.SelectedDate)

            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("OrgID", Me.ddlOrganization.SelectedValue)


            Dim RowID As New ReportParameter
            RowID = New ReportParameter("RowID", "0")

            Dim AppCode As New ReportParameter
            AppCode = New ReportParameter("ApprovalCode", "0")

            Dim OrgName As New ReportParameter
            OrgName = New ReportParameter("OrgName", CStr(ddlOrganization.SelectedItem.Text))


            Dim SalesRep As New ReportParameter
            SalesRep = New ReportParameter("SalesRep", vantxt)

            Dim UsedFor As New ReportParameter
            UsedFor = New ReportParameter("UsedFor", "")

            Dim UsedCode As New ReportParameter
            UsedCode = New ReportParameter("UsedCode", "")

            Dim FDate As New ReportParameter
            FDate = New ReportParameter("FromDate", fromdate.ToString())

            Dim TDate As New ReportParameter
            TDate = New ReportParameter("ToDate", todate.ToString())

            Dim SID As New ReportParameter
            SID = New ReportParameter("SID", van)


            Dim apptxt As String = ""
            Dim appby As String = ""

            Dim collection1 As IList(Of RadComboBoxItem) = ddApprovedBy.CheckedItems
            For Each li As RadComboBoxItem In collection1
                appby = appby & li.Value & ","
                apptxt = apptxt & li.Text & ","
            Next
            If apptxt <> "" Then
                apptxt = apptxt.Substring(0, apptxt.Length - 1)
            End If
            If appby = "" Then
                appby = "0"
            End If

            If appby = "0" Then
                apptxt = "All"
            End If



            Dim ApprovedBy As New ReportParameter
            ApprovedBy = New ReportParameter("ApprovedBy", appby)

            Dim Approvedtxt As New ReportParameter
            Approvedtxt = New ReportParameter("Approvedtxt", apptxt)




            rview.ServerReport.SetParameters(New ReportParameter() {OrgId, RowID, AppCode, OrgName, SalesRep, UsedFor, UsedCode, FDate, TDate, SID, ApprovedBy, Approvedtxt})

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
                Response.AddHeader("Content-disposition", "attachment;filename=GoodsReceipt.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=GoodsReceipt.xls")
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
            If ValidateInputs() Then
                Export("PDF")
            End If
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        ddlVan.ClearSelection()
        lblmsgUOM.Text = ""
        If Not Request.QueryString("ID") Is Nothing Then
            txtFromDate.SelectedDate = Format(DateAdd(DateInterval.Day, IIf(Day(Now) - 1 = 0, 0, -1 * (Day(Now) - 1)), Now().Date), "dd-MMM-yyyy")
            txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")

            Dim dt As New DataTable
            dt = (New SalesWorx.BO.Common.Common).GetSalesOrgbyFsr(Err_No, Err_Desc, Request.QueryString("ID"))
            If dt.Rows.Count > 0 Then
                If Not ddlOrganization.Items.FindItemByValue(dt.Rows(0)("MAS_Org_ID").ToString) Is Nothing Then
                    ddlOrganization.ClearSelection()
                    ddlOrganization.Items.FindItemByValue(dt.Rows(0)("MAS_Org_ID").ToString).Selected = True

                    If ddlOrganization.Items.Count = 2 Then
                        ddlOrganization.SelectedIndex = 1

                        LoadVan()
                        LoadApprovedBy()
                    End If
                End If
            End If
        Else
            If ddlOrganization.Items.Count = 2 Then
                ddlOrganization.SelectedIndex = 1

                LoadVan()
                LoadApprovedBy()
            End If

            txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
            txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")
        End If
        Args.Visible = False
        gvRep.Visible = False
    End Sub
End Class