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

Public Class Rep_MonthlySalesbyVan
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "MonthlySalesbyVan"

    Private Const PageID As String = "P338"
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
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))


                UId.Value = CType(Session("User_Access"), UserAccess).UserID

                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                End If

                LoadOrgDetails()

                txtFromDate.SelectedDate = DateAdd(DateInterval.Month, -6, Now)
                txtToDate.SelectedDate = Now
                If Not (Request.QueryString("OrgID") Is Nothing And Request.QueryString("SID") Is Nothing And Request.QueryString("FrmDt") Is Nothing And Request.QueryString("Todt") Is Nothing) Then

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
    Sub Export(format As String)


        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

        Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems

        Dim van As String = ""
        For Each li As RadComboBoxItem In collection
            van = van & li.Value & ","
        Next

        If van = "" Then
            van = "0"
        End If
        Dim USRID As New ReportParameter
        USRID = New ReportParameter("UID", CType(Session.Item("USER_ACCESS"), UserAccess).UserID)



        Dim SalesRepID As New ReportParameter
        SalesRepID = New ReportParameter("SID", van)


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
        OID = New ReportParameter("OID", CStr(ddlOrganization.SelectedValue.ToString()))



        rview.ServerReport.SetParameters(New ReportParameter() {USRID, OID, SalesRepID, FDate, TDate})

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
            Response.AddHeader("Content-disposition", "attachment;filename=SalesbyVan.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=SalesbyVan.xls")
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
            ddl_Type.Visible = True
            gvRep.Visible = True
            BindReport()
            BindChart()
        Else
            ddl_Type.Visible = False
            gvRep.Visible = False
            divCurrency.Visible = False
            Args.Visible = False
        End If
    End Sub
    Private Sub BindChart()

        ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)


        

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
                    todate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(txtToDate.SelectedDate)))
                Else
                    todate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, DateAdd(DateInterval.Day, -1 * (Now.Day - 1), CDate(txtToDate.SelectedDate))))
                End If

                Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems

                Dim van As String = ""
                Dim vantxt As String = ""
                For Each li As RadComboBoxItem In Collection
                    van = van & li.Value & ","
                    vantxt = vantxt & li.Text & ","
                Next
                If vantxt <> "" Then
                    vantxt = vantxt.Substring(0, vantxt.Length - 1)
                End If
                If van = "" Then
                    van = "0"
                End If
                If van = "0" Then
                    lbl_van.Text = "All"
                Else
                    lbl_van.Text = vantxt
                End If

                lbl_org.Text = ddlOrganization.SelectedItem.Text
                lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("MMM-yyyy")
                lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("MMM-yyyy")

                Args.Visible = True

                hfOrg.Value = ddlOrganization.SelectedItem.Value
                hfVan.Value = van
                HFrom.Value = fromdate
                Hto.Value = todate
                Hcount.Value = ddl_Type.SelectedItem.Value

                If collection.Count > 10 Then
                    ddl_Type.Visible = True
                Else
                    ddl_Type.Visible = False
                End If
                Dim dt As New DataTable
                dt = ObjReport.GetVanSalesbyMonth(Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, ddlOrganization.SelectedItem.Value, van, fromdate.ToString("dd-MMM-yyyy"), todate.ToString("dd-MMM-yyyy"))

                Dim FinalDt As New DataTable
                FinalDt.Columns.Add("Salesrep Name")
                Dim tfromdate As DateTime
                tfromdate = fromdate

                While tfromdate <= todate
                    FinalDt.Columns.Add(tfromdate.ToString("MMM-yyyy"))
                    tfromdate = DateAdd(DateInterval.Month, 1, tfromdate)
                End While

                Dim query

                If ddl_Type.SelectedItem.Value = "10" Then
                    query = (From UserEntry In dt _
                         Group UserEntry By key = UserEntry.Field(Of String)("Salesrep_name") Into Group _
                         Select Salesrep_name = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("Sales")) Order By Total Descending.Take(10)).ToList

                Else
                    query = (From UserEntry In dt _
                         Group UserEntry By key = UserEntry.Field(Of String)("Salesrep_name") Into Group _
                         Select Salesrep_name = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("Sales")) Order By Total Descending).ToList

                End If

                Dim dtcurrency As DataTable
                dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
                Dim Currency As String = ""
                Dim pos As Integer
                If dtcurrency.Rows.Count > 0 Then
                    pos = Val(dtcurrency.Rows(0)("Decimal_Digits").ToString())
                    Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
                End If

                lbl_Currency.Text = Currency
                divCurrency.Visible = True


                For Each x In query
                    tfromdate = fromdate
                    Dim dr As DataRow
                    dr = FinalDt.NewRow
                    dr("Salesrep Name") = x.Salesrep_name
                    While tfromdate <= todate
                        Dim seldr() As DataRow
                        seldr = dt.Select("Salesrep_name='" & x.Salesrep_name & "' and m=" & tfromdate.Month & " and yr=" & tfromdate.Year)
                        If seldr.Length > 0 Then
                            dr(tfromdate.ToString("MMM-yyyy")) = Format(Math.Round(seldr(0)("Sales"), pos), "N")
                        Else

                            dr(tfromdate.ToString("MMM-yyyy")) = Math.Round(0, pos)
                        End If
                        
                        tfromdate = DateAdd(DateInterval.Month, 1, tfromdate)
                    End While
                    FinalDt.Rows.Add(dr)
                Next



                gvRep.DataSource = FinalDt
                gvRep.DataBind()

                ''''''''''''''''''''''''' CType(gvRep.Columns(1), GridBoundColumn).DataFormatString = "{0:#,###.00}"

              

                'Dim StrSummary As String = ""

                'Dim sum = dt.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("Discount")))

                'StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>Total discount " & Currency & "<div class='text-primary'>" & Format(sum, "#,##0.00") & "</div></div></div>"

                'summary.InnerHtml = StrSummary
            End If
        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If ddlOrganization.SelectedItem.Value = "0" Then
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
        If Math.Abs(DateDiff(DateInterval.Month, CDate(txtFromDate.SelectedDate), CDate(txtToDate.SelectedDate))) > 12 Then
            MessageBoxValidation("Please select a date range in one year", "Validation")
            Return bretval
        End If
        bretval = True
        Return bretval
    End Function

   

     

    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender
        For Each column As GridColumn In gvRep.MasterTableView.AutoGeneratedColumns
            If column.UniqueName = "Salesrep Name" Then
                column.ItemStyle.HorizontalAlign = HorizontalAlign.Left
                column.HeaderStyle.HorizontalAlign = HorizontalAlign.Left
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

    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then
            LoadOrgDetails()

        End If
        gvRep.Visible = False
        Args.Visible = False
        divCurrency.Visible = False
        ddl_Type.Visible = False
    End Sub
    Sub LoadOrgDetails()
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        ObjCommon = New SalesWorx.BO.Common.Common()
        ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
        ddlVan.DataBind()

        For Each itm As RadComboBoxItem In ddlVan.Items
            itm.Checked = True
        Next

        Dim dtcurrency As DataTable
        Dim ObjReport As New SalesWorx.BO.Common.Reports
        dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

        Dim Currency As String = ""
        If dtcurrency.Rows.Count > 0 Then
            Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
            lblDecimal.Text = "N" & dtcurrency.Rows(0)("Decimal_Digits")
        End If
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub

    Private Sub ddl_Type_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddl_Type.SelectedIndexChanged
        If ValidateInputs() Then
            gvRep.Visible = True
            BindReport()
            BindChart()
        Else
            gvRep.Visible = False
        End If
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If

        Args.Visible = False
        ddlVan.ClearCheckedItems()
        ddlVan.Items.Clear()
        LoadOrgDetails()
        txtFromDate.SelectedDate = DateAdd(DateInterval.Month, -6, Now)
        txtToDate.SelectedDate = Now
        ddl_Type.Visible = False
        gvRep.Visible = False
        divCurrency.Visible = False
        Args.Visible = False
    End Sub
    Protected Sub gvRep_ItemCommand(sender As Object, e As GridCommandEventArgs)
        If e.CommandName.ToString() = "ExportToExcel" Then
            BindReport()
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
                todate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(txtToDate.SelectedDate)))
            Else
                todate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, DateAdd(DateInterval.Day, -1 * (Now.Day - 1), CDate(txtToDate.SelectedDate))))
            End If

            Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems

            Dim van As String = ""

            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","

            Next

            If van = "" Then
                van = "0"
            End If




            Hcount.Value = ddl_Type.SelectedItem.Value

            If collection.Count > 10 Then
                ddl_Type.Visible = True
            Else
                ddl_Type.Visible = False
            End If
            Dim dt As New DataTable
            dt = ObjReport.GetVanSalesbyMonth(Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, ddlOrganization.SelectedItem.Value, van, fromdate.ToString("dd-MMM-yyyy"), todate.ToString("dd-MMM-yyyy"))

            Dim FinalDt As New DataTable
            FinalDt.Columns.Add("Salesrep Name")
            Dim tfromdate As DateTime
            tfromdate = fromdate

            While tfromdate <= todate
                FinalDt.Columns.Add(tfromdate.ToString("MMM-yyyy"), System.Type.GetType("System.Decimal"))
                tfromdate = DateAdd(DateInterval.Month, 1, tfromdate)
            End While

            Dim query

            If ddl_Type.SelectedItem.Value = "10" Then
                query = (From UserEntry In dt _
                     Group UserEntry By key = UserEntry.Field(Of String)("Salesrep_name") Into Group _
                     Select Salesrep_name = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("Sales")) Order By Total Descending.Take(10)).ToList

            Else
                query = (From UserEntry In dt _
                     Group UserEntry By key = UserEntry.Field(Of String)("Salesrep_name") Into Group _
                     Select Salesrep_name = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("Sales")) Order By Total Descending).ToList

            End If

            Dim dtcurrency As DataTable
            dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
            Dim Currency As String = ""
            Dim pos As Integer
            If dtcurrency.Rows.Count > 0 Then
                pos = Val(dtcurrency.Rows(0)("Decimal_Digits").ToString())
                Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
            End If

            lbl_Currency.Text = Currency
            divCurrency.Visible = True


            For Each x In query
                tfromdate = fromdate
                Dim dr As DataRow
                dr = FinalDt.NewRow
                dr("Salesrep Name") = x.Salesrep_name
                While tfromdate <= todate
                    Dim seldr() As DataRow
                    seldr = dt.Select("Salesrep_name='" & x.Salesrep_name & "' and m=" & tfromdate.Month & " and yr=" & tfromdate.Year)
                    If seldr.Length > 0 Then
                        dr(tfromdate.ToString("MMM-yyyy")) = Math.Round(seldr(0)("Sales"), pos)
                    Else

                        dr(tfromdate.ToString("MMM-yyyy")) = Math.Round(0, pos)
                    End If

                    tfromdate = DateAdd(DateInterval.Month, 1, tfromdate)
                End While
                FinalDt.Rows.Add(dr)
            Next

            If FinalDt.Rows.Count > 0 Then

                Using package As New ExcelPackage()
                    ' add a new worksheet to the empty workbook
                    Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
                    Worksheet.Cells("A1").LoadFromDataTable(FinalDt, True)

                    Worksheet.Cells.AutoFitColumns()

                    Response.Clear()
                    Response.Buffer = True
                    Response.Charset = ""

                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    Response.AddHeader("content-disposition", "attachment;filename=MonthlySalesbyVan.xlsx")

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
End Class