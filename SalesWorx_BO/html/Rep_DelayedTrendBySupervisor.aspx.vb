Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Public Class Rep_DelayedTrendBySupervisor
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "DelayedCollectionTrendBySup"

    Private Const PageID As String = "P341"
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
                Dim CountryTbl As DataTable = Nothing
                Dim orgTbl As DataTable = Nothing

                ObjCommon = New SalesWorx.BO.Common.Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                UId.Value = CType(Session("User_Access"), UserAccess).UserID




                CountryTbl = ObjCommon.GetCountry(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)
                ddlCountry.DataSource = CountryTbl
                ddlCountry.DataBind()


                orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                Dim s() As String = Nothing
                Dim Currency As String = Nothing
                Dim DecimalDigits As String = "2"
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


                    ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
                    ddlOrganization.DataBind()
                    Me.hfCurrency.Value = Currency
                    Me.hfDecimal.Value = "N" & DecimalDigits

                ElseIf CountryTbl.Rows.Count > 1 Then
                    ddlCountry.SelectedIndex = 0
                    dvCountry.Visible = True


                    s = ddlCountry.SelectedValue.Split("$")

                    If s.Length > 0 Then
                        country = s(0).ToString()
                        Currency = s(1).ToString()
                        DecimalDigits = s(2).ToString()
                    End If

                    ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
                    ddlOrganization.DataBind()
                    Me.hfCurrency.Value = Currency
                    Me.hfDecimal.Value = "N" & DecimalDigits
                End If

                Dim OrgStr As String = Nothing
                For Each item As RadComboBoxItem In ddlOrganization.Items
                    item.Checked = True
                    If item.Checked Then

                        OrgStr = OrgStr & "," & item.Value

                    End If
                Next

                Dim objRep As SalesWorx.BO.Common.Reports
                objRep = New Reports()
                ddlLocation.DataSource = objRep.GetSalesDist(Err_No, Err_Desc)
                ddlLocation.DataBind()

                txtFromDate.SelectedDate = DateAdd(DateInterval.Month, -3, Now.Date)
                txtToDate.SelectedDate = Now.Date
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


        Dim collectionLoc As IList(Of RadComboBoxItem) = ddlLocation.CheckedItems

        Dim Locstr As String = ""
        For Each li As RadComboBoxItem In collectionLoc
            Locstr = Locstr & li.Value & ","
        Next

        If Locstr = "" Then
            Locstr = "0"
        End If

        Dim OID As New ReportParameter
        OID = New ReportParameter("OID", Org)

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
        Dim Fdate As New ReportParameter
        Fdate = New ReportParameter("Fromdate", fromdate)

        Dim Tdate As New ReportParameter
        Tdate = New ReportParameter("Todate", Todate)

        Dim USRID As New ReportParameter
        USRID = New ReportParameter("Uid", objUserAccess.UserID)


        Dim Loc As New ReportParameter
        Loc = New ReportParameter("Loc", Locstr)

        rview.ServerReport.SetParameters(New ReportParameter() {Fdate, Tdate, OID, USRID, Loc})

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
            Response.AddHeader("Content-disposition", "attachment;filename=CollectionTrendBySuper.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=CollectionTrendBySuper.xls")
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
            BindChart()
            BindReport()
        Else
            Args.Visible = False
            summary.InnerHtml = ""
        End If
    End Sub
    Private Sub BindChart()
        Dim collection As IList(Of RadComboBoxItem) = ddlOrganization.CheckedItems

        Dim Org As String = ""
        Dim Orgtxt As String = ""
        For Each li As RadComboBoxItem In collection
            Org = Org & li.Value & ","
            Orgtxt = Orgtxt & li.Text & ","
        Next
        If Orgtxt <> "" Then
            Orgtxt = Orgtxt.Substring(0, Orgtxt.Length - 1)
        End If
        If Org = "" Then
            Org = "0"
        End If
        If Org = "0" Then
            lbl_org.Text = "All"
        Else
            lbl_org.Text = Orgtxt
        End If

        Dim Locationtxt As String = ""
        Dim LocationStr As String = ""
        For Each item As RadComboBoxItem In ddlLocation.Items
            If item.Checked Then
                If LocationStr Is String.Empty Then
                    LocationStr = String.Format("{0}", item.Value)
                    Locationtxt = String.Format("{0}", item.Text)
                Else
                    LocationStr = String.Format("{1},{0}", item.Value, LocationStr)
                    Locationtxt = String.Format("{1},{0}", item.Text, Locationtxt)
                End If
            End If
        Next

        If Locationtxt <> "" Then
            Locationtxt = Locationtxt.Substring(0, Locationtxt.Length - 1)
        End If
        If LocationStr = "" Then
            lbl_Loc.Text = "All"
        Else
            lbl_Loc.Text = Locationtxt
        End If
        If LocationStr = "" Then
            LocationStr = "0"
        End If

        hfOrg.Value = Org
        hfLoc.Value = LocationStr
        hFrom.Value = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
        Hto.Value = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")

        lbl_Month.Text = CDate(txtFromDate.SelectedDate).ToString("MMM-yyyy")
        lbl_ToMonth.Text = CDate(txtToDate.SelectedDate).ToString("MMM-yyyy")
        HFromDate.Value = CDate(txtFromDate.SelectedDate).ToString("MMM-yyyy")
        HTodate.Value = CDate(txtToDate.SelectedDate).ToString("MMM-yyyy")

        Args.Visible = True
        hfOrg.Value = Org

        ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
        HSearched.Value = "1"
    End Sub

    Sub BindReport()
        summary.InnerHtml = "<h5 class='text-right'>Currency <span class='text-blue'><strong>" & hfCurrency.Value & "</strong></span></h5>"
        Dim tblstr As String = ""
        Dim dt As New DataTable
        dt = (New SalesWorx.BO.Common.Reports).GetDelayedCollectionBySupervisor(Err_No, Err_Desc, hfOrg.Value, CDate(txtFromDate.SelectedDate).ToString("MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("MMM-yyyy"), CType(Session("User_Access"), UserAccess).UserID, hfLoc.Value)
        If dt.Rows.Count Then
            Dim query = (From UserEntry In dt _
                         Group UserEntry By key = UserEntry.Field(Of String)("Description") Into Group _
                         Select Supervisor = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("Delayed"))).ToList



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
            tblstr = "<div class='overflowx'><table class='table table-bordered table-text-wrap table-th-center'>"
            Dim trHdstr = "<tr><th></th>"
            Dim trDatastr = ""

            Dim tfromdate As DateTime
            tfromdate = fromdate

            While tfromdate <= todate
                trHdstr = trHdstr & "<th colspan='3' class='text-center'>" & tfromdate.ToString("MMM-yyyy") & "</th>"
                tfromdate = DateAdd(DateInterval.Month, 1, tfromdate)
            End While

            trHdstr = trHdstr & "</tr>"

            tblstr = tblstr & trHdstr

            trHdstr = "<tr><th></th>"

            tfromdate = fromdate

            While tfromdate <= todate
                trHdstr = trHdstr & "<th>Delayed</th>"
                trHdstr = trHdstr & "<th>Paid On Time</th>"
                trHdstr = trHdstr & "<th>Delayed %</th>"
                tfromdate = DateAdd(DateInterval.Month, 1, tfromdate)
            End While
            trHdstr = trHdstr & "</tr>"


            tblstr = tblstr & trHdstr



            For Each x In query
                trDatastr = trDatastr & "<tr><td class='font-bold text-wrap'>" & x.Supervisor & "</td>"
                tfromdate = fromdate
                While tfromdate <= todate
                    Dim seldr() As DataRow
                    seldr = dt.Select("Description='" & x.Supervisor & "' and m=" & tfromdate.Month & " and yr=" & tfromdate.Year)
                    If seldr.Length > 0 Then
                        trDatastr = trDatastr & "<td class='text-right'>" & Format(Val(seldr(0)("Delayed").ToString), hfDecimal.Value) & "</td>"
                        trDatastr = trDatastr & "<td class='text-right'>" & Format(Val(seldr(0)("PaidOnTime").ToString), hfDecimal.Value) & "</td>"
                        trDatastr = trDatastr & "<td class='text-right'>" & Format(Val(seldr(0)("Perc").ToString), hfDecimal.Value) & "</td>"
                    Else
                        trDatastr = trDatastr & "<td class='text-right'>" & Format(0, hfDecimal.Value) & "</td>"
                        trDatastr = trDatastr & "<td class='text-right'>" & Format(0, hfDecimal.Value) & "</td>"
                        trDatastr = trDatastr & "<td class='text-right'>" & Format(0, hfDecimal.Value) & "</td>"
                    End If
                    tfromdate = DateAdd(DateInterval.Month, 1, tfromdate)
                End While
                trDatastr = trDatastr & "</tr>"
            Next
            tblstr = tblstr & trDatastr & "</table>"
        End If
        summary.InnerHtml = summary.InnerHtml & tblstr
    End Sub
    'Private Sub dgv_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvRep.SortCommand
    '    ViewState("SortField") = e.SortExpression
    '    SortDirection = "flip"
    '    BindReport()
    'End Sub
    'Private Sub dgv_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged

    '    BindReport()
    'End Sub
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
            MessageBoxValidation("Please select the From month.", "Validation")
            Return bRetval
        End If
        If IsNothing(txtToDate.SelectedDate) Then
            MessageBoxValidation("Please select the To month.", "Validation")
            Return bRetval
        End If
        If CDate(txtFromDate.SelectedDate) > CDate(txtToDate.SelectedDate) Then
            MessageBoxValidation("From Month should be leass than or equal to To Month .", "Validation")
            Return bRetval
        End If
        If Math.Abs(DateDiff(DateInterval.Day, CDate(txtFromDate.SelectedDate), LastDayOfMonth(CDate(txtToDate.SelectedDate)))) > 365 Then
            MessageBoxValidation("Please select the date range of one year.", "Validation")
            Return bRetval
        End If
        bRetval = True
        Return bRetval
    End Function
    Private Function LastDayOfMonth(aDate As DateTime) As Date
        Return New DateTime(aDate.Year, _
                            aDate.Month, _
                            DateTime.DaysInMonth(aDate.Year, aDate.Month))
    End Function
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
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
        LoadOrgs()
        ddlLocation.ClearCheckedItems()
        txtFromDate.SelectedDate = DateAdd(DateInterval.Month, -3, Now.Date)
        txtToDate.SelectedDate = Now.Date
        Args.Visible = False
        summary.InnerHtml = ""
    End Sub
End Class