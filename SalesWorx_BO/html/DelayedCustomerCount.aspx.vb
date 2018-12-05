Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Public Class DelayedCustomerCount
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "DelayedCustomerCount"

    Private Const PageID As String = "P328"
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
                UId.Value = CType(Session("User_Access"), UserAccess).UserID
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                For Each item As RadComboBoxItem In ddlOrganization.Items
                    item.Checked = True
                Next

                txtFromDate.SelectedDate = DateAdd(DateInterval.Month, -3, Now.Date)
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

        Dim collection As IList(Of RadComboBoxItem) = ddlOrganization.CheckedItems

        Dim Org As String = ""
        For Each li As RadComboBoxItem In collection
            Org = Org & li.Value & ","
        Next

        If Org = "" Then
            Org = "0"
        End If
        Dim USRID As New ReportParameter
        USRID = New ReportParameter("Uid", CType(Session.Item("USER_ACCESS"), UserAccess).UserID)



        

         
        Dim fromdate As String
        If CDate(txtFromDate.SelectedDate).Day = 1 Then
            fromdate = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
        Else
            fromdate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), CDate(txtFromDate.SelectedDate)).ToString("dd-MMM-yyyy")
        End If

        Dim todate As String
        If CDate(txtToDate.SelectedDate).Day = 1 Then
            todate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(txtToDate.SelectedDate))).ToString("dd-MMM-yyyy")
        Else
            todate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, DateAdd(DateInterval.Day, -1 * (Now.Day - 1), CDate(txtToDate.SelectedDate)))).ToString("dd-MMM-yyyy")
        End If

        Dim FDate As New ReportParameter
        FDate = New ReportParameter("Fromdate", fromdate)

        Dim TDate As New ReportParameter
        TDate = New ReportParameter("ToDate", todate)


        Dim OID As New ReportParameter
        OID = New ReportParameter("OID", Org)



        rview.ServerReport.SetParameters(New ReportParameter() {USRID, OID, FDate, TDate})

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
            Response.AddHeader("Content-disposition", "attachment;filename=DelayedCustCount.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=DelayedCustCount.xls")
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
            BindChart()
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

                Dim fromdate As DateTime
                If CDate(txtFromDate.SelectedDate).Day = 1 Then
                    fromdate = CDate(txtFromDate.SelectedDate)
                Else
                    fromdate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), CDate(txtFromDate.SelectedDate))
                End If

                Dim todate As DateTime
                If CDate(txtToDate.SelectedDate).Day = 1 Then
                    todate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(txtToDate.SelectedDate))).ToString("dd-MMM-yyyy")
                Else
                    todate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, DateAdd(DateInterval.Day, -1 * (Now.Day - 1), CDate(txtToDate.SelectedDate)))).ToString("dd-MMM-yyyy")
                End If

                
                Dim collection As IList(Of RadComboBoxItem) = ddlOrganization.CheckedItems

                Dim Orgtxt As String = ""
                Dim Org As String = ""
                For Each li As RadComboBoxItem In Collection
                    Org = Org & li.Value & ","
                    Orgtxt = Orgtxt & li.Text & ","
                Next

                If Org = "" Then
                    Org = "0"
                End If

                hfOrgID.Value = Org
                hfSMonth.Value = fromdate.ToString("dd-MMM-yyyy")
                hfEMonth.Value = todate.ToString("dd-MMM-yyyy")


                If Orgtxt <> "" Then
                    Orgtxt = Orgtxt.Substring(0, Orgtxt.Length - 1)
                End If
                If Org = "0" Then
                    lbl_org.Text = "All"
                Else
                    lbl_org.Text = Orgtxt
                End If
                lbl_FromMonth.Text = CDate(txtFromDate.SelectedDate).ToString("MMM-yyyy")
                lbl_ToMonth.Text = CDate(txtToDate.SelectedDate).ToString("MMM-yyyy")
                Args.Visible = True
                Dim dt As New DataTable
                dt = ObjReport.GetDelayedCustomerCount(Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, Org, fromdate.ToString("dd-MMM-yyyy"), todate.ToString("dd-MMM-yyyy"))

                Dim FinalDt As New DataTable
                FinalDt.Columns.Add("Organization")
                Dim tfromdate As DateTime
                tfromdate = fromdate

                While tfromdate <= todate
                    FinalDt.Columns.Add(tfromdate.ToString("MMM-yyyy"))
                    tfromdate = DateAdd(DateInterval.Month, 1, tfromdate)
                End While


                Dim query = (From UserEntry In dt _
                          Group UserEntry By key = UserEntry.Field(Of String)("Organization") Into Group _
                          Select Organization = key, Total = Group.Sum(Function(p) p.Field(Of Integer)("CustCount"))).ToList

                For Each x In query
                    tfromdate = fromdate
                    Dim dr As DataRow
                    dr = FinalDt.NewRow
                    dr("Organization") = x.Organization
                    While tfromdate <= todate
                        Dim seldr() As DataRow
                        seldr = dt.Select("Organization='" & x.Organization & "' and m=" & tfromdate.Month & " and yr=" & tfromdate.Year)
                        If seldr.Length > 0 Then
                            dr(tfromdate.ToString("MMM-yyyy")) = Format(seldr(0)("CustCount"), "#,##0")
                        Else

                            dr(tfromdate.ToString("MMM-yyyy")) = "0"
                        End If
                        tfromdate = DateAdd(DateInterval.Month, 1, tfromdate)
                    End While
                    FinalDt.Rows.Add(dr)
                Next



                gvRep.DataSource = FinalDt
                gvRep.DataBind()
 
               
            End If
        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub
    Private Sub BindChart()

        ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If ddlOrganization.CheckedItems.Count = 0 Then
            MessageBoxValidation("Select an Organisation", "Validation")
            SetFocus(ddlOrganization)
            Return bretval
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
        If Math.Abs(DateDiff(DateInterval.Day, CDate(txtFromDate.SelectedDate), LastDayOfMonth(CDate(txtToDate.SelectedDate)))) > 365 Then
            MessageBoxValidation("Please select the date range of one year.", "Validation")
            Return bretval
        End If
        bretval = True
        Return bretval
    End Function

    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender
        For Each column As GridColumn In gvRep.MasterTableView.AutoGeneratedColumns
            If column.UniqueName = "Organization" Then
                column.ItemStyle.HorizontalAlign = HorizontalAlign.Left
            Else
                column.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
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

    Private Function LastDayOfMonth(aDate As DateTime) As Date
        Return New DateTime(aDate.Year, _
                            aDate.Month, _
                            DateTime.DaysInMonth(aDate.Year, aDate.Month))
    End Function

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearCheckedItems()
        For Each item As RadComboBoxItem In ddlOrganization.Items
            item.Checked = True
        Next

        txtFromDate.SelectedDate = DateAdd(DateInterval.Month, -3, Now.Date)
        txtToDate.SelectedDate = Now
        Args.Visible = False
        summary.InnerHtml = ""
        gvRep.Visible = False
    End Sub
End Class