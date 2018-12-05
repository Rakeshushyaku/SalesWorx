Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Partial Public Class Rep_PurchaseReport
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "PurchaseReport"

    Private Const PageID As String = "P312"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single

    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private Sub RepRouteMaster_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
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
                ddlOrganization.DataTextField = "Description"
                ddlOrganization.DataValueField = "MAS_Org_ID"
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization"))

                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")

                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1

                    LoadVan()
                    LoadCurrency()
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

            ObjCommon = New SalesWorx.BO.Common.Common()

            ddl_Van.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
            ddl_Van.DataValueField = "SalesRep_ID"
            ddl_Van.DataTextField = "SalesRep_Name"
            ddl_Van.DataBind()
            ''  ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --"))
            If Not ddl_Van.Items.FindItemByValue(Request.QueryString("ID")) Is Nothing Then
                ddl_Van.ClearSelection()
                ddl_Van.Items.FindItemByValue(Request.QueryString("ID")).Selected = True
            End If

            For Each itm As RadComboBoxItem In ddl_Van.Items
                itm.Checked = True
            Next
        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            ObjCommon = Nothing
        End Try
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
    Protected Sub Chk_Organization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
         If Not (ddlOrganization.SelectedItem.Value = "") Then
            LoadVan()
            LoadCurrency()
        Else
            ddl_Van.Items.Clear()
            ddl_Van.Text = ""
            lbl_Currency.Text = ""
            '' ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --"))
            ''   RVMain.Reset()
        End If

    End Sub

    'Protected Sub SearchBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SearchBtn.Click

    '    If ddlOrganization.SelectedIndex <= 0 Then
    '        MessageBoxValidation("Select organization.")
    '        Exit Sub
    '    End If
    '    If Not IsDate(txt_fromDate.Text) Then
    '        MessageBoxValidation("Enter a valid from date.")
    '        Exit Sub
    '    End If
    '    If Not IsDate(txt_todate.Text) Then
    '        MessageBoxValidation("Enter a valid to date.")
    '        Exit Sub
    '    End If
    '     If IsDate(txt_todate.Text) < IsDate(txt_fromDate.Text) Then
    '        MessageBoxValidation("Enter a valid date range.")
    '        Exit Sub
    '    End If

    '    Dim collection As IList(Of RadComboBoxItem) = ddl_Van.CheckedItems


    '    Dim VanIds As String = ""
    '    For Each li As RadComboBoxItem In collection
    '        If VanIds = "" Then
    '            VanIds = li.Value & ","
    '        Else
    '            VanIds = VanIds & li.Value & ","
    '        End If
    '    Next
    '    If Trim(VanIds) <> "" Then
    '        VanIds = VanIds.Substring(0, Len(VanIds) - 1)
    '    End If
    '    If VanIds = "" Then
    '        VanIds = "-1"
    '    End If


    '    If VanIds = "-1" Then
    '        MessageBoxValidation("Please select atleast one van.")
    '        Exit Sub
    '    End If


    '    Dim Fromdate As String

    '    Fromdate = txt_fromDate.Text


    '    'Dim OrgID As New ReportParameter
    '    'OrgID = New ReportParameter("OID", ddlOrganization.SelectedValue)

    '    Dim VanID As New ReportParameter
    '    VanID = New ReportParameter("SID", VanIds)

    '    Dim Start_Date As New ReportParameter
    '    Start_Date = New ReportParameter("ReqFromDate", Fromdate)

    '    Dim End_Date As New ReportParameter
    '    End_Date = New ReportParameter("ReqTodateDate", txt_todate.Text)

    '    Dim OrgID As New ReportParameter
    '    OrgID = New ReportParameter("OrgID", ddlOrganization.SelectedValue)


    '    With RVMain
    '        .Reset()
    '        .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
    '        .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
    '        .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
    '        .ServerReport.SetParameters(New ReportParameter() {VanID, End_Date, OrgID, Start_Date})
    '        '.ServerReport.Refresh()
    '        .Visible = True
    '    End With
    'End Sub
    Sub MessageBoxValidation(ByVal str As String, Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Private Sub SearchBtn_Click(sender As Object, e As EventArgs) Handles SearchBtn.Click
        Args.Visible = False
        gvRep.Visible = False
        divCurrency.Visible = False
        If ValidateInputs() Then
            BindData()
        End If
    End Sub

    Private Sub BindData()
        Dim SearchQuery As String = ""
        Dim objUserAccess As UserAccess
        Try
            '' Binding Data

            rpbFilter.Items(0).Expanded = False
            Args.Visible = False

            Dim vantxt As String = ""
            Dim van As String = ""

            Dim collection As IList(Of RadComboBoxItem) = ddl_Van.CheckedItems
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
                For Each li As RadComboBoxItem In ddl_Van.Items
                    If li.Value > 0 Then
                        van = van & li.Value & ","
                    End If

                Next
            End If


            lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")

            Args.Visible = True
            divCurrency.Visible = True


            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetPurchaseReport(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, txtFromDate.SelectedDate, txtToDate.SelectedDate, van)
            gvRep.DataSource = dt
            gvRep.DataBind()
            gvRep.Visible = True
            ''  InitReportViewer()
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

    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender
        For Each column As GridColumn In gvRep.MasterTableView.Columns
            If column.UniqueName = "LoadQty" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & hfDigit.Value & "}"
            ElseIf column.UniqueName = "ReqQty" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & hfDigit.Value & "}"
            End If
        Next
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
    Private Sub gvRep_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles gvRep.ItemDataBound
        Try
            '' Updating Group Header Text

            If TypeOf e.Item Is GridGroupHeaderItem Then
                Dim item As GridGroupHeaderItem = CType(e.Item, GridGroupHeaderItem)
                Dim groupDataRow As DataRowView = CType(e.Item.DataItem, DataRowView)

                If item.DataCell.Text.Contains("ReqDate:") Then
                    item.DataCell.Text = String.Format("Req. Date : {0} ",
                                                  CDate(groupDataRow("ReqDate")).ToString("dd-MMM-yyyy"))
                End If

                If item.DataCell.Text.Contains("SalesRep_Name:") Then
                    item.DataCell.Text = String.Format("Sales Rep. : {0} ",
                                                  CStr(groupDataRow("SalesRep_Name")))
                End If
              
            End If
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
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


            Dim collection As IList(Of RadComboBoxItem) = ddl_Van.CheckedItems


            Dim VanIds As String = ""
            For Each li As RadComboBoxItem In collection
                If VanIds = "" Then
                    VanIds = li.Value & ","
                Else
                    VanIds = VanIds & li.Value & ","
                End If
            Next
            If Trim(VanIds) <> "" Then
                VanIds = VanIds.Substring(0, Len(VanIds) - 1)
            End If
            If VanIds = "" Then
                VanIds = "-1"
            End If

          


            Dim VanID As New ReportParameter
            VanID = New ReportParameter("SID", VanIds)

            Dim Start_Date As New ReportParameter
            Start_Date = New ReportParameter("ReqFromDate", txtFromDate.SelectedDate)

            Dim End_Date As New ReportParameter
            End_Date = New ReportParameter("ReqTodateDate", txtToDate.SelectedDate)

            Dim OrgID As New ReportParameter
            OrgID = New ReportParameter("OrgID", ddlOrganization.SelectedValue)


            rview.ServerReport.SetParameters(New ReportParameter() {Start_Date, End_Date, OrgID, VanID})

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
                Response.AddHeader("Content-disposition", "attachment;filename=PurchaseReport.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=PurchaseReport.xls")
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
        ddl_Van.ClearCheckedItems()
        ddl_Van.Items.Clear()
        LoadVan()
        LoadCurrency()
        
        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")
        Args.Visible = False
        gvRep.Visible = False
        divCurrency.Visible = False
    End Sub
End Class