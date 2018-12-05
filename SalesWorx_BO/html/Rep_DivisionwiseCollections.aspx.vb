Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Public Class Rep_DivisionwiseCollections
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "DivisionWiseCollection"

    Private Const PageID As String = "P327"
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
            Exit Sub
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

                Dim CountryTbl As DataTable = Nothing
                Dim orgTbl As DataTable = Nothing

                CountryTbl = ObjCommon.GetCountry(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)
                ddlCountry.DataSource = CountryTbl
                ddlCountry.DataBind()


                orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                Dim s() As String = Nothing
                Dim Currency As String = Nothing
                Dim DecimalDigits As String = "N2"
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

                    Me.hfCurrency.Value = Currency
                    Me.hfDecimal.Value = "N" & DecimalDigits
                    ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
                    ddlOrganization.DataBind()
                    Dim OrgStr As String = Nothing
                    For Each item As RadComboBoxItem In ddlOrganization.Items
                        item.Checked = True
                        If item.Checked Then

                            OrgStr = OrgStr & "," & item.Value

                        End If
                    Next

                ElseIf CountryTbl.Rows.Count > 1 Then
                    ddlCountry.SelectedIndex = 0
                    dvCountry.Visible = True


                    s = ddlCountry.SelectedValue.Split("$")

                    If s.Length > 0 Then
                        country = s(0).ToString()
                        Currency = s(1).ToString()
                        DecimalDigits = s(2).ToString()
                    End If
                    Me.hfCurrency.Value = Currency
                    Me.hfDecimal.Value = "N" & DecimalDigits
                    ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
                    ddlOrganization.DataBind()
                    Dim OrgStr As String = Nothing
                    For Each item As RadComboBoxItem In ddlOrganization.Items
                        item.Checked = True
                        If item.Checked Then

                            OrgStr = OrgStr & "," & item.Value

                        End If
                    Next
                End If

                UId.Value = CType(Session("User_Access"), UserAccess).UserID

                txtFromDate.SelectedDate = Now.Date

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

        Dim OID As New ReportParameter
        OID = New ReportParameter("ORGID", Org)



        Dim fromdate As String


        Dim Todate As String


        If CDate(txtFromDate.SelectedDate).Day = 1 Then
            fromdate = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
        Else
            fromdate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), CDate(txtFromDate.SelectedDate)).ToString("dd-MMM-yyyy")
        End If
        Todate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(fromdate))).ToString("dd-MMM-yyyy") & " 23:59:59"
         

        Dim Fdate As New ReportParameter
        Fdate = New ReportParameter("Fromdate", fromdate)

        Dim Tdate As New ReportParameter
        Tdate = New ReportParameter("Todate", Todate)

        Dim USRID As New ReportParameter
        USRID = New ReportParameter("Uid", objUserAccess.UserID)

        rview.ServerReport.SetParameters(New ReportParameter() {Fdate, Tdate, OID, USRID})

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
            Response.AddHeader("Content-disposition", "attachment;filename=DivisionWiseCollection.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=DivisionWiseCollection.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            gvRep.Visible = True
            BindReport()
            BindChart()
        Else
            gvRep.Visible = False
            Args.Visible = False
            summary.InnerHtml = ""

        End If
    End Sub
    Private Sub BindChart()
        Dim collection As IList(Of RadComboBoxItem) = ddlOrganization.CheckedItems

        Dim Org As String = ""
        For Each li As RadComboBoxItem In collection
            Org = Org & li.Value & ","
        Next

        If Org = "" Then
            Org = "0"
        End If
        hfOrgID.Value = Org
        HFrom.Value = CDate(txtFromDate.SelectedDate).ToString("MMM/yyyy")
        ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
        HSearched.Value = "1"
    End Sub
    Private Sub BindReport()
        Try

            rpbFilter.Items(0).Expanded = False


            lbl_Month.Text = CDate(txtFromDate.SelectedDate).ToString("MMM-yyyy")
            Args.Visible = True

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable

            Dim fromdate As String


            Dim Todate As String


            If CDate(txtFromDate.SelectedDate).Day = 1 Then
                fromdate = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            Else
                fromdate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), CDate(txtFromDate.SelectedDate)).ToString("dd-MMM-yyyy")
            End If
            Todate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(fromdate))).ToString("dd-MMM-yyyy") & " 23:59:59"

            Dim collection As IList(Of RadComboBoxItem) = ddlOrganization.CheckedItems

            Dim Org As String = ""
            Dim Orgtxt As String = ""
            For Each li As RadComboBoxItem In collection
                Org = Org & li.Value & ","
                Orgtxt = Orgtxt & li.Text & ","
            Next

            If Org = "" Then
                Org = "0"
            End If

            If Orgtxt <> "" Then
                Orgtxt = Orgtxt.Substring(0, Orgtxt.Length - 1)
            End If
            If Org = "0" Then
                lbl_org.Text = "All"
            Else
                lbl_org.Text = Orgtxt
            End If

            dt = ObjReport.GetDivisionCollection(Err_No, Err_Desc, Org, fromdate, Todate, CType(Session.Item("USER_ACCESS"), UserAccess).UserID)

           

            'Dim DtSummary As New DataTable
            'DtSummary.Columns.Add("Paymode")
            'DtSummary.Columns.Add("Amount")

            Dim StrSummary As String = ""

            'Dim strtbl = "<table class='table' cellpadding='0' cellspacing='0' border='0'>"
            'Dim Hrow As String = "<tr><td></td>"
            'Dim row1 As String = "<tr><td>Payments Delayed</td>"
            'Dim row2 As String = "<tr><td>Payments on time</td>"
            'Dim row3 As String = "<tr><td>Delayed Perc %</td>"
            'For Each dr As DataRow In dt.Rows
            '    Hrow = Hrow & "<td>" & dr("Description").ToString() & "</td>"
            '    row1 = row1 & "<td>" & Val(dr("Delayed").ToString()) & "</td>"
            '    row2 = row2 & "<td>" & Val(dr("PaidOnTIme").ToString()) & "</td>"
            '    Dim Per As Decimal
            '    If (Val(dr("Delayed").ToString) + Val(dr("PaidOnTIme").ToString)) <> 0 Then
            '        Per = Math.Round((Val(dr("Delayed").ToString) / (Val(dr("Delayed").ToString) + Val(dr("PaidOnTIme").ToString))) * 100, 2)
            '    Else
            '        Per = 0
            '    End If
            '    row3 = row3 & "<td>" & Per & "</td>"
            'Next

            'Hrow = Hrow & "</tr>"
            'row1 = row1 & "</tr>"
            'row2 = row2 & "</tr>"
            'row3 = row3 & "</tr>"

            'strtbl = strtbl & Hrow & row1 & row2 & row3 & "</table>"
            'If dt.Rows.Count > 0 Then
            '    Detailed.InnerHtml = strtbl
            'Else
            '    Chartid.Visible = False
            '    Detailed.InnerHtml = "<b>No Data</b>"
            'End If

            Dim FinalDt As New DataTable
            FinalDt.Columns.Add("Description")
            

            For Each dr As DataRow In dt.Rows
                FinalDt.Columns.Add(dr("Description"))
            Next

            FinalDt.Columns.Add("Over All")

            Dim query = (From UserEntry In dt _
                      Group UserEntry By key = UserEntry.Field(Of String)("Description") Into Group _
                      Select Desc = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("Delayed"))).ToList

            Dim Totalpaid As Decimal = 0
            Dim TotalDelayed As Decimal = 0

            Dim drDelayed As DataRow
            drDelayed = FinalDt.NewRow
            drDelayed("Description") = "Delayed"

            Dim Delayed As Decimal = 0
            For Each x In query
                Dim seldr() As DataRow
                seldr = dt.Select("Description='" & x.Desc & "'")
                If seldr.Length > 0 Then
                    drDelayed(x.Desc) = Format(Val(seldr(0)("Delayed").ToString()), hfDecimal.Value)
                    Delayed = Val(seldr(0)("Delayed").ToString())
                    TotalDelayed = TotalDelayed + Delayed
                Else

                    drDelayed(x.Desc) = Format(0, hfDecimal.Value)
                End If
            Next

            If query.Count > 8 And dt.Rows.Count < 14 Then
                Chartid.Style.Add("width", (dt.Rows.Count * 40).ToString & "px")
            ElseIf dt.Rows.Count > 14 Then
                Chartid.Style.Add("width", (dt.Rows.Count * 35).ToString & "px")
            Else
                Chartid.Style.Add("width", "700px")

            End If
            drDelayed("Over All") = Format(TotalDelayed, hfDecimal.Value)
            drDelayed("Description") = "Delayed " & "(" & hfCurrency.Value & ")"

            FinalDt.Rows.Add(drDelayed)

            Dim drpaid As DataRow
            drpaid = FinalDt.NewRow
            drpaid("Description") = "Paid on Time"
            Dim paid As Decimal = 0
            For Each x In query
                Dim seldr() As DataRow
                seldr = dt.Select("Description='" & x.Desc & "'")
                If seldr.Length > 0 Then
                    drpaid(x.Desc) = Format(Val(seldr(0)("PaidOnTIme").ToString()), hfDecimal.Value)
                    paid = Val(seldr(0)("PaidOnTIme").ToString())
                    Totalpaid = Totalpaid + paid
                Else

                    drpaid(x.Desc) = Format(0, hfDecimal.Value)
                End If
            Next

            drpaid("Over All") = Format(Totalpaid, hfDecimal.Value)
            drpaid("Description") = "Paid on Time " & "(" & hfCurrency.Value & ")"

            FinalDt.Rows.Add(drpaid)

            Dim drper As DataRow
            drper = FinalDt.NewRow
            drper("Description") = "Delayed Percentage"
            For Each x In query
                Dim seldr() As DataRow
                seldr = dt.Select("Description='" & x.Desc & "'")
                If seldr.Length > 0 Then
                    paid = Val(seldr(0)("PaidOnTIme").ToString())
                    Delayed = Val(seldr(0)("Delayed").ToString())
                    If paid + Delayed <> 0 Then
                        drper(x.Desc) = Format((Delayed / (paid + Delayed)) * 100.0, hfDecimal.Value)
                    Else
                        drper(x.Desc) = Format(0, hfDecimal.Value)
                    End If
                Else

                    drper(x.Desc) = Format(0, hfDecimal.Value)
                End If
            Next
            If Totalpaid + TotalDelayed <> 0 Then
                drper("Over All") = Format((TotalDelayed / (Totalpaid + TotalDelayed)) * 100.0, hfDecimal.Value)
            Else
                drper("Over All") = Format(0, hfDecimal.Value)
            End If


            FinalDt.Rows.Add(drper)
 


            gvRep.DataSource = FinalDt
            gvRep.DataBind()
            gvRep.Columns(0).HeaderText = ""
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


    Function ValidateInputs()
        Dim bRetval As Boolean = False
        If ddlOrganization.CheckedItems Is Nothing Then
            MessageBoxValidation("Please select the Organization.", "Validation")
            Return bRetval
        End If
        If ddlOrganization.CheckedItems.Count = 0 Then
            MessageBoxValidation("Please select the Organization.", "Validation")
            Return bRetval
        End If

        If IsNothing(txtFromDate.SelectedDate) Then
            MessageBoxValidation("Please select the month.", "Validation")
            Return bRetval
        End If
        bRetval = True
        Return bRetval
    End Function
    
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub

    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender
        For Each column As GridColumn In gvRep.MasterTableView.AutoGeneratedColumns
            If column.UniqueName = "Description" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                column.ItemStyle.HorizontalAlign = HorizontalAlign.Left
                column.HeaderText = ""
            Else
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & hfDecimal.Value & "}"
                col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
            End If
        Next
    End Sub

    Private Sub ddlCountry_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlCountry.SelectedIndexChanged
        LoadOrgs()
        If HSearched.Value = "1" Then
            BindChart()
            BindReport()
        End If
    End Sub
    Sub LoadOrgs()
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
        Me.hfCurrency.Value = Currency
        Me.hfDecimal.Value = "N" & DecimalDigits

        ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
        ddlOrganization.DataBind()

        Dim OrgStr As String = Nothing
        For Each item As RadComboBoxItem In ddlOrganization.Items
            item.Checked = True
            If item.Checked Then

                OrgStr = OrgStr & "," & item.Value

            End If
        Next

    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ObjCommon = New SalesWorx.BO.Common.Common()
        Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)

        Dim CountryTbl As DataTable = Nothing
        Dim orgTbl As DataTable = Nothing

        CountryTbl = ObjCommon.GetCountry(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)
        ddlCountry.DataSource = CountryTbl
        ddlCountry.DataBind()


        orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
        Dim s() As String = Nothing
        Dim Currency As String = Nothing
        Dim DecimalDigits As String = "N2"
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

            Me.hfCurrency.Value = Currency
            Me.hfDecimal.Value = "N" & DecimalDigits
            ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
            ddlOrganization.DataBind()
            Dim OrgStr As String = Nothing
            For Each item As RadComboBoxItem In ddlOrganization.Items
                item.Checked = True
                If item.Checked Then

                    OrgStr = OrgStr & "," & item.Value

                End If
            Next

        ElseIf CountryTbl.Rows.Count > 1 Then
            ddlCountry.SelectedIndex = 0
            dvCountry.Visible = True


            s = ddlCountry.SelectedValue.Split("$")

            If s.Length > 0 Then
                country = s(0).ToString()
                Currency = s(1).ToString()
                DecimalDigits = s(2).ToString()
            End If
            Me.hfCurrency.Value = Currency
            Me.hfDecimal.Value = "N" & DecimalDigits
            ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
            ddlOrganization.DataBind()
            Dim OrgStr As String = Nothing
            For Each item As RadComboBoxItem In ddlOrganization.Items
                item.Checked = True
                If item.Checked Then

                    OrgStr = OrgStr & "," & item.Value

                End If
            Next
        End If

        UId.Value = CType(Session("User_Access"), UserAccess).UserID

        txtFromDate.SelectedDate = Now.Date
        gvRep.Visible = False
        Args.Visible = False
        summary.InnerHtml = ""
    End Sub
End Class