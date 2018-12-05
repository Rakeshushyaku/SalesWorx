Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Partial Public Class Rep_MonthlyVisitReport
    Inherits System.Web.UI.Page

     Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "MonthlyCustomerVisits"

    Private Const PageID As String = "P298"
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

                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))

                ddl_year.DataSource = ObjCommon.GetCustomerVisitMonth(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
                ddl_year.DataTextField = "monthYear"
                ddl_year.DataValueField = "monthYearv"
                ddl_year.DataBind()

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
    Private Function ValidateInputs() As Boolean

        Try
            Dim bRetVal As Boolean = False
            Dim collection As IList(Of RadComboBoxItem) = ddl_Van.CheckedItems

            Dim van As String = ""
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","
            Next

            If (ddlOrganization.SelectedItem.Value = "0") Then
                MessageBoxValidation("Select an Organization.", "Validation")
                Return bRetVal
            End If
            If van = "" Then
                MessageBoxValidation("Select van.", "Validation")
                Return bRetVal
            Else
                van = van.Substring(0, van.Length - 1)
            End If
            Dim seldate() As String
            seldate = ddl_year.SelectedItem.Value.Split("-")
            If seldate(0) = 0 Then
                MessageBoxValidation("Select a month year.", "Validation")
                Return bRetVal
            End If
            bRetVal = True
            Return bRetVal
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
    End Function
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
    Sub Export(format As String)

        Dim SearchParams As String = ""
        SearchParams = BuildQuery()
        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        Dim myParamUserId As New ReportParameter
        myParamUserId = New ReportParameter("Uid", objUserAccess.UserID)

        Dim SID As New ReportParameter
        SID = New ReportParameter("SearchParams", SearchParams)

        Dim OrgId As New ReportParameter
        OrgId = New ReportParameter("OrgId", ddlOrganization.SelectedItem.Value)

        Dim Month As New ReportParameter
        Month = New ReportParameter("Month", ddl_year.SelectedItem.Text)

        rview.ServerReport.SetParameters(New ReportParameter() {SID, OrgId, Month})

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
            Response.AddHeader("Content-disposition", "attachment;filename=CustomerList.pdf")
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=CustomerList.xls")
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()
    End Sub
    Private Function BuildQuery()
        Try
            Dim SearchParams As String = ""
            Dim fromdate As String
            Dim Todate As String
            Dim van As String = ""

            Dim collection As IList(Of RadComboBoxItem) = ddl_Van.CheckedItems

            For Each li As RadComboBoxItem In Collection
                van = van & li.Value & ","
            Next
            van = van.Substring(0, van.Length - 1)

            Dim seldate() As String
            seldate = ddl_year.SelectedItem.Value.Split("-")
            fromdate = CDate(seldate(1) & "/01/" & seldate(0)).ToString("dd-MMM-yyyy")
            Todate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(seldate(1) & "/01/" & seldate(0)))).ToString("dd-MMM-yyyy")

            SearchParams = "  and  A.SalesRep_ID in(" & van & ")"
            SearchParams = SearchParams & " and A.Visit_Start_Date>=convert(datetime,'" & fromdate & "',103) and A.Visit_Start_Date<=convert(datetime,'" & Todate & " 23:59:59',103)"


            Return SearchParams

        Catch Ex As Exception
            '  log.Error(GetExceptionInfo(Ex))
        End Try
    End Function
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            gvRep.Visible = True
            BindReport()
        Else
            gvRep.Visible = False
        End If
    End Sub
     Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Private Sub BindReport()
        Try
            Dim SearchQuery As String = ""
            If Not (ddlOrganization.SelectedItem.Value = "0") Then
                SearchQuery = BuildQuery()
            Else
                MessageBoxValidation("Select an organization.", "Validation")
                Exit Sub
            End If
            rpbFilter.Items(0).Expanded = False
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetMonthlyVisits(Err_No, Err_Desc, SearchQuery, ddlOrganization.SelectedItem.Value)
            gvRep.DataSource = dt
            gvRep.DataBind()


            'Dim dtcurrency As DataTable
            'dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
            'Dim Currency As String = ""
            'If dtcurrency.Rows.Count > 0 Then
            '    Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
            'End If

            Dim StrSummary As String = ""
            
            Dim sumInvCnt = dt.AsEnumerable().Sum((Function(x) x.Field(Of Int32)("InvCnt")))

            StrSummary = StrSummary & "<div class='col-sm-3'><div class='widgetblk'>No Of Orders Taken " & "<div class='text-primary'>" & Format(sumInvCnt, "#,##0") & "</div></div></div>"

            Dim sumtotalOrder = dt.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("OrderAmt")))

            StrSummary = StrSummary & "<div class='col-sm-3'><div class='widgetblk'>Total  Order Value " & "<div class='text-primary'>" & Format(sumtotalOrder, "#,##0.00") & "</div></div></div>"

            Dim sumRmaCnt = dt.AsEnumerable().Sum((Function(x) x.Field(Of Int32)("RMACnt")))

            StrSummary = StrSummary & "<div class='col-sm-3'><div class='widgetblk'>No Of Returns Taken " & "<div class='text-primary'>" & Format(sumRmaCnt, "#,##0") & "</div></div></div>"

            Dim sumtotalRma = dt.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("RMA")))

            StrSummary = StrSummary & "<div class='col-sm-3'><div class='widgetblk'>Total  Return Value " & "<div class='text-primary'>" & Format(sumtotalRma, "#,##0.00") & "</div></div></div>"


            summary.InnerHtml = StrSummary
        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
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
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged


        If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New SalesWorx.BO.Common.Common()


            ddl_Van.DataTextField = "SalesRep_Name"
            ddl_Van.DataValueField = "SalesRep_ID"
            ddl_Van.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)

            ddl_Van.DataBind()

            ddl_year.DataSource = ObjCommon.GetCustomerVisitMonth(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
            ddl_year.DataTextField = "monthYear"
            ddl_year.DataValueField = "monthYearv"
            ddl_year.DataBind()
            ddl_year.Items.Insert(0, New RadComboBoxItem("Select", "0-0"))
        Else
            ddl_Van.Items.Clear()
        End If

    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub

End Class