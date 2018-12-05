Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI

Partial Public Class RepPrincipalWiseSales
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer
    Private ReportPath As String = "PrincipalWiseSalesReturns"

    Private Const PageID As String = "P250"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub RepPrincipalWiseSales_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ' ''If Not IsNothing(Me.Master) Then

        ' ''    Dim masterScriptManager As ScriptManager
        ' ''    masterScriptManager = CType(Master.FindControl("ScriptManager1"), ScriptManager)

        ' ''    ' Make sure our master page has the script manager we're looking for
        ' ''    If Not IsNothing(masterScriptManager) Then

        ' ''        ' Turn off partial page postbacks for this page
        ' ''        masterScriptManager.EnablePartialRendering = False
        ' ''    End If

        ' ''End If
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
                ''RVMain.Visible = False
                ObjCommon = New SalesWorx.BO.Common.Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization"))

                Dim objUserAccess As UserAccess
                objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1

                    LoadAgency()
                    LoadCurrency()
                End If

              
                'txtFromDate.Text = Format(Now().Date, "dd-MMM-yyyy")
                'txtToDate.Text = Format(Now().Date, "dd-MMM-yyyy")
                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")

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
    Sub LoadAgency()
        Try

            ObjCommon = New SalesWorx.BO.Common.Common()

            ddlAgency.DataSource = ObjCommon.GetAgencyList(Err_No, Err_Desc, ddlOrganization.SelectedValue)
            ddlAgency.DataBind()
            '' ddlAgency.Items.Insert(0, New RadComboBoxItem("Select Principal"))

        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            ObjCommon = Nothing
        End Try
    End Sub

    Private Sub BindData()
        Dim SearchQuery As String = ""
        Dim objUserAccess As UserAccess
        Try
            ''If ddlOrganization.SelectedValue = "" Then
            ''    MessageBoxValidation("Select organization.", "Validation")
            ''    Exit Sub
            ''End If
            ''If Not IsDate(txtFromDate.SelectedDate) Then
            ''    MessageBoxValidation("Enter a valid from date.", "Validation")
            ''    Exit Sub
            ''End If
            ''If Not IsDate(txtToDate.SelectedDate) Then
            ''    MessageBoxValidation("Enter a valid to date.", "Validation")
            ''    Exit Sub
            ''End If
            ''If CDate(txtFromDate.SelectedDate) > CDate(txtToDate.SelectedDate) Then
            ''    MessageBoxValidation("Start Date should not be greater than End Date.", "Validation")
            ''    Exit Sub
            ''End If

            lbl_org.Text = ddlOrganization.SelectedItem.Text
            lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")

            If ddlAgency.SelectedValue = "" Then
                lbl_van.Text = "All"
            Else
                lbl_van.Text = ddlAgency.SelectedItem.Text
            End If

            ''InitReportViewer()


            '' Binding Data

            rpbFilter.Items(0).Expanded = False
            Args.Visible = False


            Args.Visible = True
            divCurrency.Visible = True


            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetSalesPrinciple(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, txtFromDate.SelectedDate, txtToDate.SelectedDate, ddlAgency.SelectedValue)


            gvRep.DataSource = dt
            gvRep.DataBind()
            gvRep.Visible = True


            Dim dtcurrency As DataTable
            dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

            Dim Currency As String = ""
            If dtcurrency.Rows.Count > 0 Then
                lblDecimal.Text = "N" & dtcurrency.Rows(0)("Decimal_Digits")
                Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
            End If

            lbl_Currency.Text = Currency

            Dim StrSummary As String = ""
            Dim sum = dt.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("SValue")))

            StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>Total Sales " & Currency & "<div class='text-primary'>" & Format(sum, lblDecimal.Text) & "</div></div></div>"

            Dim sumR = dt.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("RValue")))

            StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>Total Returns " & Currency & "<div class='text-primary'>" & Format(sumR, lblDecimal.Text) & "</div></div></div>"


            summary.InnerHtml = StrSummary

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
            objUserAccess = Nothing
        End Try
    End Sub
    Private Sub InitReportViewer()
        Try


            Dim FromDate As New ReportParameter
            FromDate = New ReportParameter("Fromdate", txtFromDate.SelectedDate)

            Dim ToDate As New ReportParameter
            ToDate = New ReportParameter("Todate", txtToDate.SelectedDate)

            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("OrgID", CStr(ddlOrganization.SelectedValue.ToString()))


            Dim AgencyID As New ReportParameter
            AgencyID = New ReportParameter("Agency", ddlAgency.SelectedValue)

            ''With RVMain
            ''    .Reset()
            ''    .Visible = True
            ''    .ShowParameterPrompts = False
            ''    .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            ''    .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            ''    If ddlType.SelectedItem.Value = "T" Then
            ''        ReportPath = "PrincipalWiseSalesReturns"
            ''    Else
            ''        ReportPath = "SalesReturnbyAgencyGraph"
            ''    End If
            ''    .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
            ''    .ServerReport.SetParameters(New ReportParameter() {OrgId, FromDate, ToDate, AgencyID})
            ''    '.ServerReport.Refresh()

            ''End With


        Catch Ex As Exception
            '  log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub
    Sub MessageBoxValidation(ByVal str As String, Title As String)
        ''lblMessage.ForeColor = Drawing.Color.Red
        ''lblinfo.Text = "Validation"
        ''lblMessage.Text = str
        ''MpInfoError.Show()
        ''MpInfoError.Show()
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Sub LoadCurrency()
        Try
            Dim dtcurrency As DataTable
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

            Dim Currency As String = ""
            If dtcurrency.Rows.Count > 0 Then
                Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
                hfDigit.Value = "N" & dtcurrency.Rows(0)("Decimal_Digits")
                lbl_Currency.Text = Currency
            End If
            ObjReport = Nothing
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
    Private Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrganization.SelectedIndexChanged
        If Not (ddlOrganization.SelectedItem.Value = "") Then
            LoadAgency()
            LoadCurrency()
        Else
            ddlAgency.Items.Clear()
            ddlAgency.Text = ""
            lbl_Currency.Text = ""
            ddlAgency.Items.Insert(0, New RadComboBoxItem("Select Principal"))
            ''   RVMain.Reset()
        End If
      
    End Sub

    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        Args.Visible = False
        gvRep.Visible = False
        divCurrency.Visible = False
        Chart.Visible = False
        summary.InnerHtml = ""
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
        If ValidateInputs() Then
            BindData()
            BindChart()
            Chart.Visible = True
            Hfrom.Value = txtFromDate.SelectedDate.Value.ToString("dd-MMM-yyyy")
            HTo.Value = txtToDate.SelectedDate.Value.ToString("dd-MMM-yyyy")
            HorgID.Value = ddlOrganization.SelectedValue
        End If

    End Sub
    Private Sub BindChart()
        Try

            Dim dt As DataTable = gvRep.DataSource

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
            ElseIf dt.Rows.Count > 3 And dt.Rows.Count < 8 Then
                Chartwrapper.Style.Add("width", (dt.Rows.Count * 70).ToString & "px")
            Else
                Chart.Style.Add("height", "400px")
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
    Private Sub gvRep_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        BindData()
    End Sub

    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender
        For Each column As GridColumn In gvRep.MasterTableView.Columns
            If column.UniqueName = "SValue" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & hfDigit.Value & "}"
            ElseIf column.UniqueName = "RValue" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & hfDigit.Value & "}"
            End If
        Next
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
        ElseIf Not IsDate(txtFromDate.SelectedDate) Then
            MessageBoxValidation("Enter a valid from date.", "Validation")
            Return bretval
        ElseIf Not IsDate(txtToDate.SelectedDate) Then
            MessageBoxValidation("Enter a valid to date.", "Validation")
            Return bretval
        ElseIf CDate(txtFromDate.SelectedDate) > CDate(txtToDate.SelectedDate) Then
            MessageBoxValidation("Start Date should not be greater than End Date.", "Validation")
            Return bretval
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


            Dim fromdate As DateTime
            Dim todate As DateTime

            fromdate = CDate(txtFromDate.SelectedDate)
            todate = CDate(txtToDate.SelectedDate)

            Dim FDate As New ReportParameter
            FDate = New ReportParameter("Fromdate", fromdate.ToString("dd-MMM-yyyy"))

            Dim TDate As New ReportParameter
            TDate = New ReportParameter("Todate", todate.ToString("dd-MMM-yyyy"))

            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("OrgID", CStr(ddlOrganization.SelectedValue.ToString()))


            Dim AgencyID As New ReportParameter
            AgencyID = New ReportParameter("Agency", CStr(IIf(ddlAgency.SelectedValue = "", "0", ddlAgency.SelectedValue)))






            rview.ServerReport.SetParameters(New ReportParameter() {FDate, TDate, OrgId, AgencyID})

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
                Response.AddHeader("Content-disposition", "attachment;filename=PrincipalWiseSalesReturns.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=PrincipalWiseSalesReturns.xls")
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
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        ddlAgency.ClearSelection()
        If Not (ddlOrganization.SelectedItem.Value = "") Then
            LoadAgency()
            LoadCurrency()
        Else
            ddlAgency.Items.Clear()
            ddlAgency.Text = ""
            lbl_Currency.Text = ""
            ddlAgency.Items.Insert(0, New RadComboBoxItem("Select Principal"))
            ''   RVMain.Reset()
        End If
        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")
        Args.Visible = False
        gvRep.Visible = False
        divCurrency.Visible = False
        Chart.Visible = False
        summary.InnerHtml = ""
        lblmsgUOM.Text = ""
    End Sub
End Class