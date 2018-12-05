Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports System.Globalization
Imports Telerik.Web.UI

Partial Public Class Rep_Summary
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager

    Dim ObjCustomer As Customer

    Private ReportPath As String = "SummaryPurchaseSalesReturns"

    Private Const PageID As String = "P254"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single
    Dim StartDate As Date
    Dim ToyearSelected As String
    Dim ObjCommon As SalesWorx.BO.Common.Common
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
                ddlOrganization.DataTextField = "Description"
                ddlOrganization.DataValueField = "MAS_Org_ID"
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))

                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1

                    Loaddds()
                End If

                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")
                ddWeekMonthYear.SelectedDate = Format(Now().Date, "MMM-yyyy")
                ddWeekMonthYear.MaxDate = Format(Now().Date, "MMM-yyyy")
                RadYear.SelectedDate = Format(Now().Date, "MMM-yyyy")
                RadMonth.SelectedDate = Format(Now().Date, "MMM-yyyy")

                 
                LoadYear()
                LoadfromAndTo()
                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")

                If Not Err_Desc Is Nothing Then
                    log.Error(Err_Desc)
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
                End If

                ObjCommon = Nothing
            Catch ex As Exception
                Err_No = "74166"
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
    Private Sub Loaddds()
        Dim objUserAccess As UserAccess
        Try
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)


            ObjCommon = New SalesWorx.BO.Common.Common()
            ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())
            ddlVan.DataBind()
            ddlVan.Items.Insert(0, New RadComboBoxItem("Select Van"))

            ddlAgency.DataSource = ObjCommon.GetAgencyList(Err_No, Err_Desc, ddlOrganization.SelectedValue)
            ddlAgency.DataBind()
            ddlAgency.Items.Insert(0, New RadComboBoxItem("Select Agency"))

        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            objUserAccess = Nothing
        End Try
    End Sub
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        ''Dim ObjCommon As New Common
        ''ddlAgency.DataSource = Nothing
        ''ddlAgency.DataSource = ObjCommon.GetAgencyByOrg(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, "")
        ''ddlAgency.DataTextField = "Agency"
        ''ddlAgency.DataValueField = "Agency"
        ''ddlAgency.DataBind()
        ''ddlAgency.Items.Insert(0, New ListItem("-- Select --", "0"))


        ''Dim objUserAccess As UserAccess
        ''objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        ''ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())
        ''ddlVan.DataBind()
        ''ddlVan.Items.Insert(0, New ListItem("-- Select a value --", "0"))
        ''ObjCommon = Nothing
        ''RVMain.Reset()
        If Not (ddlOrganization.SelectedItem.Value = "0") Then
            Loaddds()
        Else
            ddlVan.Items.Clear()
            ddlVan.Text = ""
            ddlAgency.Items.Clear()
            ddlAgency.Text = ""
            
        End If
        Args.Visible = False
        gvRep.Visible = False
        divCurrency.Visible = False

       

    End Sub

    Protected Sub ddlType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlType.SelectedIndexChanged
        LoadfromAndTo()
        Args.Visible = False
        gvRep.Visible = False
    End Sub
    Sub LoadfromAndTo()
        divDaily.Visible = False
        divWeekly.Visible = False
        divMonthly.Visible = False
        divYear.Visible = False

        If ddlType.SelectedValue = "W" Then
            divWeekly.Visible = True
            Loadweek()
        ElseIf ddlType.SelectedValue = "D" Then
            divDaily.Visible = True
        ElseIf ddlType.SelectedValue = "M" Then
            divMonthly.Visible = True
        ElseIf ddlType.SelectedValue = "Y" Then
            divYear.Visible = True
        End If

        ''ddlFromMonth.Enabled = True
        ''ddlFromMonth.Visible = True
        ''ddlFromYear.Visible = True
        ''txtToDate.Visible = False
        ''txtFromDate.Visible = False
        ''pnl_daily.Visible = False
        ''ddlWeek.Visible = False
        ''If ddlType.SelectedItem.Value = "W" Then
        ''    LoadfromMonths()
        ''    ddlWeek.Visible = True
        ''    Loadweek()
        ''ElseIf ddlType.SelectedItem.Value = "M" Then
        ''    LoadfromMonths()
        ''ElseIf ddlType.SelectedItem.Value = "D" Then
        ''    ddlFromMonth.Visible = False
        ''    ddlFromYear.Visible = False
        ''    txtToDate.Visible = True
        ''    txtFromDate.Visible = True
        ''    pnl_daily.Visible = True
        ''ElseIf ddlType.SelectedItem.Value = "Y" Then
        ''    ddlFromMonth.Items.Clear()
        ''    ddlFromMonth.Items.Add("(Select)")
        ''    ddlFromMonth.Enabled = False
        ''End If
    End Sub

    Sub LoadYear()
        ''Try
        ''    ddlFromYear.Items.Clear()


        ''    StartDate = (New SalesWorx.BO.Common.Common).GetMinTransDate(Err_No, Err_Desc)
        ''    Dim CurrYear As Integer = Year(DateTime.Now)
        ''    Dim AddYear As Integer = Year(StartDate)
        ''    While AddYear <= CurrYear
        ''        ddlFromYear.Items.Add(New ListItem(AddYear.ToString, AddYear.ToString))
        ''        AddYear = AddYear + 1
        ''    End While


        ''Catch ex As Exception

        ''Finally

        ''End Try
    End Sub
    Sub LoadfromMonths()

        ''Try
        ''    ddlFromMonth.Items.Clear()
        ''    Dim CurDate As String
        ''    CurDate = Today
        ''    Dim i As Integer
        ''    Dim mm As String
        ''    Dim Alldate As Date
        ''    If ddlFromYear.SelectedItem.Value = Year(StartDate) Then
        ''        If ddlFromYear.SelectedItem.Value = Year(Now) Then
        ''            For i = Month(StartDate) To Month(Now)
        ''                mm = CStr(i)
        ''                mm = mm.PadLeft(2, "0")
        ''                mm = mm & "/01/2006"
        ''                Alldate = CDate(mm)
        ''                Dim lstItem As New ListItem(Alldate.ToString("MMM"), Alldate.ToString("MMM"))
        ''                ddlFromMonth.Items.Add(lstItem)
        ''                lstItem = Nothing
        ''            Next
        ''        Else
        ''            For i = Month(StartDate) To 12
        ''                mm = CStr(i)
        ''                mm = mm.PadLeft(2, "0")
        ''                mm = mm & "/01/2006"
        ''                Alldate = CDate(mm)
        ''                Dim lstItem As New ListItem(Alldate.ToString("MMM"), Alldate.ToString("MMM"))
        ''                ddlFromMonth.Items.Add(lstItem)
        ''                lstItem = Nothing
        ''            Next
        ''        End If

        ''    ElseIf ddlFromYear.SelectedItem.Value = Year(Now) Then
        ''        For i = 1 To Month(Now)
        ''            mm = CStr(i)
        ''            mm = mm.PadLeft(2, "0")
        ''            mm = mm & "/01/2006"
        ''            Alldate = CDate(mm)
        ''            Dim lstItem As New ListItem(Alldate.ToString("MMM"), Alldate.ToString("MMM"))
        ''            ddlFromMonth.Items.Add(lstItem)
        ''            lstItem = Nothing
        ''        Next
        ''    Else
        ''        For i = 1 To 12
        ''            mm = CStr(i)
        ''            mm = mm.PadLeft(2, "0")
        ''            mm = mm & "/01/2006"
        ''            Alldate = CDate(mm)
        ''            Dim lstItem As New ListItem(Alldate.ToString("MMM"), Alldate.ToString("MMM"))
        ''            ddlFromMonth.Items.Add(lstItem)
        ''            lstItem = Nothing
        ''        Next
        ''    End If

        ''Catch ex As Exception

        ''Finally

        ''End Try
    End Sub

    ''Protected Sub ddlFrom_Year_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFromYear.SelectedIndexChanged
    ''    LoadfromAndTo()
    ''    RVMain.Reset()
    ''End Sub
    Sub Loadweek()
        ''Dim todate As DateTime = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(ddlFromMonth.SelectedValue & "/01/" & ddlFromYear.SelectedItem.Value)))
        ''ddlWeek.Items.Clear()
        ''Dim noofweeks As Integer = 4
        ''If (todate.Day / 7) > 4 Then
        ''    noofweeks = 5
        ''End If
        ''For i As Int16 = 1 To noofweeks
        ''    ddlWeek.Items.Add(New ListItem("W" & i.ToString, (i - 1) * 7 + 1))
        ''Next

        Try
            Dim todate As DateTime = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(ddWeekMonthYear.SelectedDate)))
            ddlWeek.Items.Clear()
            Dim noofweeks As Integer = 4
            If (todate.Day / 7) > 4 Then
                noofweeks = 5
            End If
            For i As Int16 = 1 To noofweeks
                ddlWeek.Items.Add(New RadComboBoxItem("W" & i.ToString, (i - 1) * 7 + 1))
            Next
            ddlWeek.SelectedIndex = 0
        Catch ex As Exception
            log.Error(ex.Message)
        End Try

    End Sub

    ''Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch0.Click

    ''If ddlOrganization.SelectedItem.Value = "0" Then
    ''    MessageBoxValidation("Please select the organization")
    ''    Exit Sub
    ''End If

    ''Dim fromdate As String
    ''Dim todate As String

    ''Dim ObjCommon As New Common
    ''Dim EndTime As String
    ''EndTime = ObjCommon.GetDayEndTime(Err_No, Err_Desc)
    ''ObjCommon = Nothing

    ''If ddlType.SelectedItem.Value = "D" Then
    ''    If IsDate(txtFromDate.Text) = False Then
    ''        MessageBoxValidation("Please enter a valid from date")
    ''        Exit Sub
    ''    End If
    ''    If IsDate(txtToDate.Text) = False Then
    ''        MessageBoxValidation("Please enter a valid to date")
    ''        Exit Sub
    ''    End If
    ''    fromdate = DateAdd(DateInterval.Day, -1, CDate(txtFromDate.Text)).ToString("MM/dd/yyyy") & " " & EndTime
    ''    todate = CDate(txtToDate.Text).ToString("MM/dd/yyyy") & " " & EndTime
    ''ElseIf ddlType.SelectedItem.Value = "W" Then
    ''    fromdate = DateAdd(DateInterval.Day, -1, CDate(ddlFromMonth.SelectedValue & "/" & ddlWeek.SelectedItem.Value & "/" & ddlFromYear.SelectedItem.Value)).ToString("MM/dd/yyyy") & " " & EndTime
    ''    todate = DateAdd(DateInterval.Day, 6, CDate(ddlFromMonth.SelectedValue & "/" & ddlWeek.SelectedItem.Value & "/" & ddlFromYear.SelectedItem.Value)).ToString("MM/dd/yyyy") & " " & EndTime
    ''ElseIf ddlType.SelectedItem.Value = "M" Then
    ''    fromdate = DateAdd(DateInterval.Day, -1, CDate(ddlFromMonth.SelectedValue & "/01/" & ddlFromYear.SelectedItem.Value)).ToString("MM/dd/yyyy") & " " & EndTime
    ''    todate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(ddlFromMonth.SelectedValue & "/01/" & ddlFromYear.SelectedItem.Value))).ToString("MM/dd/yyyy") & " " & EndTime
    ''ElseIf ddlType.SelectedItem.Value = "Y" Then
    ''    fromdate = DateAdd(DateInterval.Day, -1, CDate("01/01/" & ddlFromYear.SelectedItem.Value)).ToString("MM/dd/yyyy") & " " & EndTime
    ''    todate = CDate("12/31/" & ddlFromYear.SelectedItem.Value) & " " & EndTime
    ''End If

    ''If Not (CDate(fromdate) <= CDate(todate)) Then
    ''    MessageBoxValidation("Invalid Date range selection")
    ''    Exit Sub
    ''End If

    ''Dim Agency As New ReportParameter
    ''Agency = New ReportParameter("Agency", ddlAgency.SelectedItem.Value)

    ''Dim SID As New ReportParameter
    ''SID = New ReportParameter("SID", ddlVan.SelectedItem.Value)


    ''Dim pFromDate As New ReportParameter
    ''pFromDate = New ReportParameter("FromDate", CDate(fromdate).ToString("dd-MMM-yyyy hh:mm tt"))

    ''Dim pToDate As New ReportParameter
    ''pToDate = New ReportParameter("ToDate", CDate(todate).ToString("dd-MMM-yyyy hh:mm tt"))

    ''Dim OrgId As New ReportParameter
    ''OrgId = New ReportParameter("OrgID", CStr(ddlOrganization.SelectedItem.Value.ToString()))

    ''Dim TypeID As New ReportParameter
    ''TypeID = New ReportParameter("type", ddlType.SelectedItem.Value)

    ''With RVMain
    ''    .Reset()
    ''    .Visible = True
    ''    .ShowParameterPrompts = False
    ''    .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
    ''    .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
    ''    If ddlRType.SelectedItem.Value = "T" Then
    ''        ReportPath = "SummaryPurchaseSalesReturns"
    ''    Else
    ''        ReportPath = "SummaryPurchaseSalesReturnsGraph"
    ''    End If
    ''    .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
    ''    .ServerReport.SetParameters(New ReportParameter() {TypeID, OrgId, Agency, SID, pFromDate, pToDate})
    ''    .ServerReport.Refresh()

    ''End With
    ''End Sub
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

        Dim fromdate As String
        Dim todate As String

        Dim ObjCommon As New SalesWorx.BO.Common.Common
        Dim EndTime As String
        EndTime = ObjCommon.GetDayEndTime(Err_No, Err_Desc)
        ObjCommon = Nothing


        If ddlType.SelectedItem.Value = "D" Then
            fromdate = DateAdd(DateInterval.Day, -1, CDate(txtFromDate.SelectedDate)).ToString("MM/dd/yyyy") & " " & EndTime
            todate = CDate(txtToDate.SelectedDate).ToString("MM/dd/yyyy") & " " & EndTime
            lbl_Period.Text = String.Format("{0} to {1}", CDate(fromdate).ToString("dd-MMM-yyyy"), CDate(todate).ToString("dd-MMM-yyyy"))
        ElseIf ddlType.SelectedItem.Value = "W" Then
            fromdate = DateAdd(DateInterval.Day, -1, CDate(ddWeekMonthYear.SelectedDate.Value.Month & "/" & ddlWeek.SelectedItem.Value & "/" & ddWeekMonthYear.SelectedDate.Value.Year)).ToString("MM/dd/yyyy") & " " & EndTime
            todate = DateAdd(DateInterval.Day, 6, CDate(ddWeekMonthYear.SelectedDate.Value.Month & "/" & ddlWeek.SelectedItem.Value & "/" & ddWeekMonthYear.SelectedDate.Value.Year)).ToString("MM/dd/yyyy") & " " & EndTime
            lbl_Period.Text = String.Format("{0} of {1}", ddlWeek.SelectedItem.Text, ddWeekMonthYear.SelectedDate.Value.ToString("MMM-yyyy"))
        ElseIf ddlType.SelectedItem.Value = "M" Then
            fromdate = DateAdd(DateInterval.Day, -1, CDate(RadMonth.SelectedDate.Value.Month & "/01/" & RadMonth.SelectedDate.Value.Year)).ToString("MM/dd/yyyy") & " " & EndTime
            todate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(RadMonth.SelectedDate.Value.Month & "/01/" & RadMonth.SelectedDate.Value.Year))).ToString("MM/dd/yyyy") & " " & EndTime
            lbl_Period.Text = String.Format("{0}", RadMonth.SelectedDate.Value.ToString("MMM-yyyy"))
        ElseIf ddlType.SelectedItem.Value = "Y" Then
            fromdate = DateAdd(DateInterval.Day, -1, CDate("01/01/" & RadYear.SelectedDate.Value.Year)).ToString("MM/dd/yyyy") & " " & EndTime
            todate = CDate("12/31/" & RadYear.SelectedDate.Value.Year) & " " & EndTime
            lbl_Period.Text = String.Format("{0}", RadYear.SelectedDate.Value.ToString("yyyy"))
        End If

        If Not (CDate(fromdate) <= CDate(todate)) Then
            MessageBoxValidation("Invalid Date range selection", "Validation")
            Exit Sub
        End If

        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

        Dim sAgency As String = "0"
        If ddlAgency.SelectedValue <> "" Then
            sAgency = ddlAgency.SelectedValue
        Else
            sAgency = "0"
        End If

        Dim Agency As New ReportParameter
        Agency = New ReportParameter("Agency", sAgency)


        Dim sVan As String = "0"
        If ddlVan.SelectedValue <> "" Then
            sVan = ddlVan.SelectedValue
        Else
            sVan = "0"
        End If

        Dim SID As New ReportParameter
        SID = New ReportParameter("SID", sVan)


        Dim pFromDate As New ReportParameter
        pFromDate = New ReportParameter("FromDate", CDate(fromdate).ToString("dd-MMM-yyyy hh:mm tt"))

        Dim pToDate As New ReportParameter
        pToDate = New ReportParameter("ToDate", CDate(todate).ToString("dd-MMM-yyyy hh:mm tt"))

        Dim OrgId As New ReportParameter
        OrgId = New ReportParameter("OrgID", CStr(ddlOrganization.SelectedItem.Value.ToString()))

        Dim TypeID As New ReportParameter
        TypeID = New ReportParameter("type", ddlType.SelectedItem.Value)

        rview.ServerReport.SetParameters(New ReportParameter() {TypeID, OrgId, Agency, SID, pFromDate, pToDate})

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
    Sub MessageBoxValidation(ByVal str As String, Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    ''Private Sub ddlFromMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFromMonth.SelectedIndexChanged
    ''    If ddlType.SelectedItem.Value = "W" Then
    ''        Loadweek()
    ''    End If
    ''End Sub

    Private Sub ddWeekMonthYear_SelectedDateChanged(sender As Object, e As Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs) Handles ddWeekMonthYear.SelectedDateChanged
        Try
            Loadweek()
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If (ddlOrganization.SelectedValue = "0") Then
            MessageBoxValidation("Please select the Organisation", "Validation")
            Return bretval
        ElseIf ddlType.SelectedValue = "D" And Not IsDate(txtFromDate.SelectedDate) Then
            MessageBoxValidation("Enter valid period.", "Validation")
            Return bretval
        ElseIf ddlType.SelectedValue = "D" And Not IsDate(txtToDate.SelectedDate) Then
            MessageBoxValidation("Enter valid period.", "Validation")
            Return bretval
        ElseIf ddlType.SelectedValue = "D" And CDate(txtFromDate.SelectedDate) > CDate(txtToDate.SelectedDate) Then
            MessageBoxValidation("From Date should not be greater than To Date.", "Validation")
            Return bretval
        Else
            bretval = True
            Return bretval
        End If
    End Function
    Private Sub SearchBtn_Click(sender As Object, e As EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            BindData()
            Args.Visible = True
            gvRep.Visible = True
            divCurrency.Visible = True
        Else
            Args.Visible = False
            gvRep.Visible = False
            divCurrency.Visible = False
        End If
    End Sub

    Private Sub BindData()
        Dim SearchQuery As String = ""
        Try

            lbl_org.Text = ddlOrganization.SelectedItem.Text

            Dim fromdate As String = ""
            Dim todate As String = ""
            Dim EndTime As String
            ObjCommon = New SalesWorx.BO.Common.Common()
            EndTime = ObjCommon.GetDayEndTime(Err_No, Err_Desc)

            lbl_Type.Text = ddlType.Text

            If ddlType.SelectedItem.Value = "D" Then
                fromdate = DateAdd(DateInterval.Day, -1, CDate(txtFromDate.SelectedDate)).ToString("MM/dd/yyyy") & " " & EndTime
                todate = CDate(txtToDate.SelectedDate).ToString("MM/dd/yyyy") & " " & EndTime
                lbl_Period.Text = String.Format("{0} to {1}", CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(todate).ToString("dd-MMM-yyyy"))
            ElseIf ddlType.SelectedItem.Value = "W" Then
                fromdate = DateAdd(DateInterval.Day, -1, CDate(ddWeekMonthYear.SelectedDate.Value.Month & "/" & ddlWeek.SelectedItem.Value & "/" & ddWeekMonthYear.SelectedDate.Value.Year)).ToString("MM/dd/yyyy") & " " & EndTime
                todate = DateAdd(DateInterval.Day, 6, CDate(ddWeekMonthYear.SelectedDate.Value.Month & "/" & ddlWeek.SelectedItem.Value & "/" & ddWeekMonthYear.SelectedDate.Value.Year)).ToString("MM/dd/yyyy") & " " & EndTime
                lbl_Period.Text = String.Format("{0} of {1}", ddlWeek.SelectedItem.Text, ddWeekMonthYear.SelectedDate.Value.ToString("MMM-yyyy"))
            ElseIf ddlType.SelectedItem.Value = "M" Then
                fromdate = DateAdd(DateInterval.Day, -1, CDate(RadMonth.SelectedDate.Value.Month & "/01/" & RadMonth.SelectedDate.Value.Year)).ToString("MM/dd/yyyy") & " " & EndTime
                todate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(RadMonth.SelectedDate.Value.Month & "/01/" & RadMonth.SelectedDate.Value.Year))).ToString("MM/dd/yyyy") & " " & EndTime
                lbl_Period.Text = String.Format("{0}", RadMonth.SelectedDate.Value.ToString("MMM-yyyy"))
            ElseIf ddlType.SelectedItem.Value = "Y" Then
                fromdate = DateAdd(DateInterval.Day, -1, CDate("01/01/" & RadYear.SelectedDate.Value.Year)).ToString("MM/dd/yyyy") & " " & EndTime
                todate = CDate("12/31/" & RadYear.SelectedDate.Value.Year) & " " & EndTime
                lbl_Period.Text = String.Format("{0}", RadYear.SelectedDate.Value.ToString("yyyy"))
            End If

            If Not (CDate(fromdate) <= CDate(todate)) Then
                MessageBoxValidation("Invalid Date range selection", "Validation")
                Exit Sub
            End If


            Dim Agency As String
            If ddlAgency.SelectedValue <> "" Then
                Agency = ddlAgency.SelectedValue
                lbl_Principle.Text = ddlAgency.Text
            Else
                Agency = "0"
                lbl_Principle.Text = "All"
            End If

            Dim SID As String
            If ddlVan.SelectedValue <> "" Then
                SID = ddlVan.SelectedValue
                lbl_van.Text = ddlVan.Text
            Else
                SID = "0"
                lbl_van.Text = "All"
            End If

            Dim dtcurrency As DataTable
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

            Dim Currency As String = ""
            If dtcurrency.Rows.Count > 0 Then
                Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
            End If

            Hcurrency.Value = Currency
            lbl_Currency.Text = Hcurrency.Value
            rpbFilter.Items(0).Expanded = False
            Args.Visible = True



            Dim dt As New DataTable
            dt = ObjReport.GetSummaryPurchaseSalesReturns(Err_No, Err_Desc, ddlOrganization.SelectedValue, fromdate, todate, SID, Agency, ddlType.SelectedValue)
            lbl_Currency.Text = Hcurrency.Value
            Dim dtFinal As New DataTable
            dtFinal.Columns.Add("salesRepName")
            dtFinal.Columns.Add("Agency")
            dtFinal.Columns.Add("Type")
            dtFinal.Columns.Add("Qty", Type.GetType("System.Double"))
            dtFinal.Columns.Add("Value", Type.GetType("System.Double"))

            If dt.Rows.Count Then
                For Each dr As DataRow In dt.Rows
                    Dim drfinal As DataRow
                    drfinal = dtFinal.NewRow
                    drfinal("salesRepName") = dr("SalesRep_name")
                    drfinal("Agency") = dr("Agency")
                    drfinal("Type") = "Purchase"
                    drfinal("Qty") = dr("vanload")
                    drfinal("Value") = dr("vanloadvalue")
                    dtFinal.Rows.Add(drfinal)

                    Dim drfinal1 As DataRow
                    drfinal1 = dtFinal.NewRow
                    drfinal1("salesRepName") = dr("SalesRep_name")
                    drfinal1("Agency") = dr("Agency")
                    drfinal1("Type") = "Sold"
                    drfinal1("Qty") = dr("sold")
                    drfinal1("Value") = dr("soldvalue")
                    dtFinal.Rows.Add(drfinal1)


                    Dim drfinal2 As DataRow
                    drfinal2 = dtFinal.NewRow
                    drfinal2("salesRepName") = dr("SalesRep_name")
                    drfinal2("Agency") = dr("Agency")
                    drfinal2("Type") = "Returned"
                    drfinal2("Qty") = dr("returned")
                    drfinal2("Value") = dr("returnedvalue")
                    dtFinal.Rows.Add(drfinal2)

                    Dim drfinal3 As DataRow
                    drfinal3 = dtFinal.NewRow
                    drfinal3("salesRepName") = dr("SalesRep_name")
                    drfinal3("Agency") = dr("Agency")
                    drfinal3("Type") = "Unloded"
                    drfinal3("Qty") = dr("vanUnload")
                    drfinal3("Value") = dr("vanUnloadvalue")
                    dtFinal.Rows.Add(drfinal3)
                Next
            End If

            gvRep.DataSource = dtFinal
            gvRep.DataBind()

        Catch ex As Exception
            If Err_Desc IsNot Nothing Then
                log.Error(Err_Desc)
            Else
                log.Error(GetExceptionInfo(ex))
            End If
            Err_No = "740167"
            '  Err_Desc = ex.Message
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
        Finally
            ObjCustomer = Nothing
        End Try
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        ddlAgency.ClearSelection()

        ddlVan.ClearSelection()
        ddlVan.Items.Clear()
        ddlVan.Text = ""
        ddlAgency.Items.Clear()
        ddlAgency.Text = ""
        

        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1

            Loaddds()
        End If


        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")
        ddWeekMonthYear.SelectedDate = Format(Now().Date, "MMM-yyyy")
        ddWeekMonthYear.MaxDate = Format(Now().Date, "MMM-yyyy")
        RadYear.SelectedDate = Format(Now().Date, "MMM-yyyy")
        RadMonth.SelectedDate = Format(Now().Date, "MMM-yyyy")

        LoadYear()

        ddlType.SelectedIndex = 0
        LoadfromAndTo()
        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")
        Args.Visible = False
        gvRep.Visible = False
        divCurrency.Visible = False
    End Sub

    Private Sub gvRep_PageIndexChanged(sender As Object, e As PivotGridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        BindData()
    End Sub
    Private Sub RadPivotGrid1_CellDataBound(sender As Object, e As PivotGridCellDataBoundEventArgs) Handles gvRep.CellDataBound

        If TypeOf e.Cell Is PivotGridColumnHeaderCell Then

            If e.Cell.Text.IndexOf("Sum of") >= 0 Then
                e.Cell.Text = e.Cell.Text.Replace("Sum of", "")
            End If

        End If
         
    End Sub
End Class