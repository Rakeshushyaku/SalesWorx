Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI

Partial Public Class Rep_StockRequisition
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer
    Private ReportPath As String = "StockRequisition"

    Private Const PageID As String = "P302"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub RepVanLoad_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'If Not IsNothing(Me.Master) Then

        '    Dim masterScriptManager As ScriptManager
        '    masterScriptManager = CType(Master.FindControl("ScriptManager1"), ScriptManager)

        '    ' Make sure our master page has the script manager we're looking for
        '    If Not IsNothing(masterScriptManager) Then

        '        ' Turn off partial page postbacks for this page
        '        masterScriptManager.EnablePartialRendering = False
        '    End If

        'End If
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
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "-1"))


                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                    LoadVan()
                End If


                Dim objUserAccess As UserAccess
                objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

                ''ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
                ''ddlVan.DataValueField = "SalesRep_ID"
                ''ddlVan.DataTextField = "SalesRep_Name"
                ''ddlVan.DataBind()
                ''ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))


                txtfromDate.SelectedDate = Format(DateAdd(DateInterval.Day, IIf(Day(Now) - 1 = 0, 0, -1 * (Day(Now) - 1)), Now().Date), "dd-MMM-yyyy")
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

    Sub LoadVan()
        Try
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New SalesWorx.BO.Common.Common()
            ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
            ddlVan.DataValueField = "SalesRep_ID"
            ddlVan.DataTextField = "SalesRep_Name"
            ddlVan.DataBind()
            ddlVan.Items.Insert(0, New RadComboBoxItem("Select Van", "0"))

            For Each itm As RadComboBoxItem In ddlVan.Items
                itm.Checked = True
            Next
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If ddlOrganization.SelectedValue = "-1" Then
            MessageBoxValidation("Please select an organization", "Validation")
            Return bretval
            'ElseIf ddlVan.SelectedIndex <= 0 Then
            '    MessageBoxValidation("Please select a van", "Validation")
            '    Return bretval
        ElseIf Not IsDate(txtfromDate.SelectedDate) Then
            MessageBoxValidation("Invalid Request start date", "Validation")
            Return bretval
        ElseIf Not IsDate(txtToDate.SelectedDate) Then
            MessageBoxValidation("Invalid Request to date", "Validation")
            Return bretval
        Else
            bretval = True
            Return bretval
        End If



    End Function

    Private Sub BindData()
        Dim SearchQuery As String = ""
        Try

            Dim van As String = "0"
            If ddlVan.SelectedIndex > 0 Then
                van = ddlVan.SelectedItem.Value
            End If

            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetStockRequisition(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, txtfromDate.SelectedDate, txtToDate.SelectedDate, van)
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
            ObjCustomer = Nothing
        End Try
    End Sub
    Private Sub InitReportViewer()
        Try


            Dim Fromdate As New ReportParameter
            Fromdate = New ReportParameter("Date", txtfromDate.SelectedDate)

            Dim ORGID As New ReportParameter
            ORGID = New ReportParameter("OrgID", ddlOrganization.SelectedItem.Value)

            Dim FSRID As New ReportParameter
            FSRID = New ReportParameter("FSRID", ddlVan.SelectedItem.Value)



            ' ''With RVMain
            ' ''    .Reset()
            ' ''    .ShowParameterPrompts = False
            ' ''    .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            ' ''    .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            ' ''    .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
            ' ''    .ServerReport.SetParameters(New ReportParameter() {ORGID, FSRID, Fromdate})
            ' ''    .ServerReport.Refresh()

            ' ''End With


        Catch Ex As Exception
            '  log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub
    Sub MessageBoxValidation(ByVal str As String, Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Private Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrganization.SelectedIndexChanged
        If Not (ddlOrganization.SelectedItem.Value = "-1") Then

            ''Dim objUserAccess As UserAccess
            ''objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ''ObjCommon = New SalesWorx.BO.Common.Common()
            ''ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)

            ''ddlVan.DataValueField = "SalesRep_ID"
            ''ddlVan.DataTextField = "SalesRep_Name"
            ''ddlVan.DataBind()
            ''ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select --", "0"))
            LoadVan()

            ' ''RVMain.Reset()
        Else
            ddlVan.ClearSelection()
            ddlVan.Items.Clear()
            ddlVan.Text = ""
            ddlVan.Items.Insert(0, New RadComboBoxItem("Select Van", "0"))
        End If

    End Sub

    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        Args.Visible = False
        gvRep.Visible = False
        If ValidateInputs() Then

            rpbFilter.Items(0).Expanded = False
            gvRep.Visible = True

            lbl_from.Text = CDate(txtfromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")

            Args.Visible = True

            lbl_org.Text = ddlOrganization.SelectedItem.Text

            If ddlVan.SelectedIndex > 0 Then
                lbl_van.Text = ddlVan.SelectedItem.Text
            Else
                lbl_van.Text = "All"
            End If

            BindData()
        End If
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

            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("OrgID", Me.ddlOrganization.SelectedValue)


            'Dim OrgName As New ReportParameter
            'OrgName = New ReportParameter("OrgName", CStr(ddlOrganization.SelectedItem.Text))


         
            Dim FDate As New ReportParameter
            FDate = New ReportParameter("Date", fromdate.ToString("dd-MMM-yyyy"))

            Dim TDate As New ReportParameter
            TDate = New ReportParameter("ToDate", todate.ToString("dd-MMM-yyyy"))

            Dim SID As New ReportParameter
            SID = New ReportParameter("FSRID", ddlVan.SelectedValue)



            rview.ServerReport.SetParameters(New ReportParameter() {OrgId, FDate, TDate, SID})

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
                Response.AddHeader("Content-disposition", "attachment;filename=StockRequisition.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=StockRequisition.xls")
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
        ddlVan.ClearSelection()
        ddlVan.Items.Clear()
        LoadVan()
        txtfromDate.SelectedDate = Format(DateAdd(DateInterval.Day, IIf(Day(Now) - 1 = 0, 0, -1 * (Day(Now) - 1)), Now().Date), "dd-MMM-yyyy")
        txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")

        Args.Visible = False

        gvRep.Visible = False
    End Sub
End Class