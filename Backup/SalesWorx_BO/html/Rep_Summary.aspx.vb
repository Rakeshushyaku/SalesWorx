Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports System.Globalization
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
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub RepSales_WeeklyMonthly_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not IsNothing(Me.Master) Then

            Dim masterScriptManager As ScriptManager
            masterScriptManager = CType(Master.FindControl("ScriptManager1"), ScriptManager)

            ' Make sure our master page has the script manager we're looking for
            If Not IsNothing(masterScriptManager) Then

                ' Turn off partial page postbacks for this page
                masterScriptManager.EnablePartialRendering = False
            End If

        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        If Not IsPostBack Then
            RVMain.Reset()
            Dim ObjCommon As Common
            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            If Not HasPermission Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If
            ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
            Try
                ObjCommon = New Common()

                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataTextField = "Description"
                ddlOrganization.DataValueField = "MAS_Org_ID"
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New ListItem("-- Select a value --", "0"))


                ddlAgency.DataSource = Nothing
                ddlAgency.DataSource = ObjCommon.GetAgencyByOrg(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, "")
                ddlAgency.DataBind()
                ddlAgency.Items.Insert(0, New ListItem("-- Select a value--", "0"))

                Dim objUserAccess As UserAccess
                objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
                ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())
                ddlVan.DataBind()
                ddlVan.Items.Insert(0, New ListItem("-- Select a value --", "0"))


                ''  InitReportViewer()
                LoadYear()
                LoadfromAndTo()
                txtfromDate.Text = Now.ToString("dd-MMM-yyyy")
                txtToDate.Text = Now.ToString("dd-MMM-yyyy")
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

    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        Dim ObjCommon As New Common
        ddlAgency.DataSource = Nothing
        ddlAgency.DataSource = ObjCommon.GetAgencyByOrg(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, "")
        ddlAgency.DataTextField = "Agency"
        ddlAgency.DataValueField = "Agency"
        ddlAgency.DataBind()
        ddlAgency.Items.Insert(0, New ListItem("-- Select --", "0"))


        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())
        ddlVan.DataBind()
        ddlVan.Items.Insert(0, New ListItem("-- Select a value --", "0"))
        ObjCommon = Nothing
        RVMain.Reset()
    End Sub

    Protected Sub ddlType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlType.SelectedIndexChanged
        LoadfromAndTo()
        RVMain.Reset()
    End Sub
    Sub LoadfromAndTo()
        ddlFromMonth.Enabled = True
        ddlFromMonth.Visible = True
        ddlFromYear.Visible = True
        txtToDate.Visible = False
        txtfromDate.Visible = False
        pnl_daily.Visible = False
        ddlWeek.Visible = False
        If ddlType.SelectedItem.Value = "W" Then
            LoadfromMonths()
            ddlWeek.Visible = True
            Loadweek()
        ElseIf ddlType.SelectedItem.Value = "M" Then
            LoadfromMonths()
        ElseIf ddlType.SelectedItem.Value = "D" Then
            ddlFromMonth.Visible = False
            ddlFromYear.Visible = False
            txtToDate.Visible = True
            txtfromDate.Visible = True
            pnl_daily.Visible = True
        ElseIf ddlType.SelectedItem.Value = "Y" Then
            ddlFromMonth.Items.Clear()
            ddlFromMonth.Items.Add("(Select)")
            ddlFromMonth.Enabled = False
        End If
    End Sub
   
    Sub LoadYear()
        Try
            ddlFromYear.Items.Clear()


            StartDate = (New SalesWorx.BO.Common.Common).GetMinTransDate(Err_No, Err_Desc)
            Dim CurrYear As Integer = Year(DateTime.Now)
            Dim AddYear As Integer = Year(StartDate)
            While AddYear <= CurrYear
                ddlFromYear.Items.Add(New ListItem(AddYear.ToString, AddYear.ToString))
                AddYear = AddYear + 1
            End While
            

        Catch ex As Exception

        Finally

        End Try
    End Sub
    Sub LoadfromMonths()

        Try
            ddlFromMonth.Items.Clear()
            Dim CurDate As String
            CurDate = Today
            Dim i As Integer
            Dim mm As String
            Dim Alldate As Date
            If ddlFromYear.SelectedItem.Value = Year(StartDate) Then
                If ddlFromYear.SelectedItem.Value = Year(Now) Then
                    For i = Month(StartDate) To Month(Now)
                        mm = CStr(i)
                        mm = mm.PadLeft(2, "0")
                        mm = mm & "/01/2006"
                        Alldate = CDate(mm)
                        Dim lstItem As New ListItem(Alldate.ToString("MMM"), Alldate.ToString("MMM"))
                        ddlFromMonth.Items.Add(lstItem)
                        lstItem = Nothing
                    Next
                Else
                    For i = Month(StartDate) To 12
                        mm = CStr(i)
                        mm = mm.PadLeft(2, "0")
                        mm = mm & "/01/2006"
                        Alldate = CDate(mm)
                        Dim lstItem As New ListItem(Alldate.ToString("MMM"), Alldate.ToString("MMM"))
                        ddlFromMonth.Items.Add(lstItem)
                        lstItem = Nothing
                    Next
                End If

            ElseIf ddlFromYear.SelectedItem.Value = Year(Now) Then
                For i = 1 To Month(Now)
                    mm = CStr(i)
                    mm = mm.PadLeft(2, "0")
                    mm = mm & "/01/2006"
                    Alldate = CDate(mm)
                    Dim lstItem As New ListItem(Alldate.ToString("MMM"), Alldate.ToString("MMM"))
                    ddlFromMonth.Items.Add(lstItem)
                    lstItem = Nothing
                Next
            Else
                For i = 1 To 12
                    mm = CStr(i)
                    mm = mm.PadLeft(2, "0")
                    mm = mm & "/01/2006"
                    Alldate = CDate(mm)
                    Dim lstItem As New ListItem(Alldate.ToString("MMM"), Alldate.ToString("MMM"))
                    ddlFromMonth.Items.Add(lstItem)
                    lstItem = Nothing
                Next
            End If

        Catch ex As Exception

        Finally

        End Try
    End Sub
    
    Protected Sub ddlFrom_Year_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFromYear.SelectedIndexChanged
        LoadfromAndTo()
        RVMain.Reset()
    End Sub
    Sub Loadweek()
        Dim todate As DateTime = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(ddlFromMonth.SelectedValue & "/01/" & ddlFromYear.SelectedItem.Value)))
        ddlWeek.Items.Clear()
        Dim noofweeks As Integer = 4
        If (todate.Day / 7) > 4 Then
            noofweeks = 5
        End If
        For i As Int16 = 1 To noofweeks
            ddlWeek.Items.Add(New ListItem("W" & i.ToString, (i - 1) * 7 + 1))
        Next
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch0.Click
       
        If ddlOrganization.SelectedItem.Value = "0" Then
            MessageBoxValidation("Please select the organization")
            Exit Sub
        End If

        Dim fromdate As String
        Dim todate As String

        Dim ObjCommon As New Common
        Dim EndTime As String
        EndTime = ObjCommon.GetDayEndTime(Err_No, Err_Desc)
        ObjCommon = Nothing

        If ddlType.SelectedItem.Value = "D" Then
            If IsDate(txtfromDate.Text) = False Then
                MessageBoxValidation("Please enter a valid from date")
                Exit Sub
            End If
            If IsDate(txtToDate.Text) = False Then
                MessageBoxValidation("Please enter a valid to date")
                Exit Sub
            End If
            fromdate = DateAdd(DateInterval.Day, -1, CDate(txtfromDate.Text)).ToString("MM/dd/yyyy") & " " & EndTime
            todate = CDate(txtToDate.Text).ToString("MM/dd/yyyy") & " " & EndTime
        ElseIf ddlType.SelectedItem.Value = "W" Then
            fromdate = DateAdd(DateInterval.Day, -1, CDate(ddlFromMonth.SelectedValue & "/" & ddlWeek.SelectedItem.Value & "/" & ddlFromYear.SelectedItem.Value)).ToString("MM/dd/yyyy") & " " & EndTime
            todate = DateAdd(DateInterval.Day, 6, CDate(ddlFromMonth.SelectedValue & "/" & ddlWeek.SelectedItem.Value & "/" & ddlFromYear.SelectedItem.Value)).ToString("MM/dd/yyyy") & " " & EndTime
        ElseIf ddlType.SelectedItem.Value = "M" Then
            fromdate = DateAdd(DateInterval.Day, -1, CDate(ddlFromMonth.SelectedValue & "/01/" & ddlFromYear.SelectedItem.Value)).ToString("MM/dd/yyyy") & " " & EndTime
            todate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(ddlFromMonth.SelectedValue & "/01/" & ddlFromYear.SelectedItem.Value))).ToString("MM/dd/yyyy") & " " & EndTime
        ElseIf ddlType.SelectedItem.Value = "Y" Then
            fromdate = DateAdd(DateInterval.Day, -1, CDate("01/01/" & ddlFromYear.SelectedItem.Value)).ToString("MM/dd/yyyy") & " " & EndTime
            todate = CDate("12/31/" & ddlFromYear.SelectedItem.Value) & " " & EndTime
        End If

        If Not (CDate(fromdate) <= CDate(todate)) Then
            MessageBoxValidation("Invalid Date range selection")
            Exit Sub
        End If

        Dim Agency As New ReportParameter
        Agency = New ReportParameter("Agency", ddlAgency.SelectedItem.Value)

        Dim SID As New ReportParameter
        SID = New ReportParameter("SID", ddlVan.SelectedItem.Value)


        Dim pFromDate As New ReportParameter
        pFromDate = New ReportParameter("FromDate", CDate(fromdate).ToString("dd-MMM-yyyy hh:mm tt"))

        Dim pToDate As New ReportParameter
        pToDate = New ReportParameter("ToDate", CDate(todate).ToString("dd-MMM-yyyy hh:mm tt"))

        Dim OrgId As New ReportParameter
        OrgId = New ReportParameter("OrgID", CStr(ddlOrganization.SelectedItem.Value.ToString()))

        Dim TypeID As New ReportParameter
        TypeID = New ReportParameter("type", ddlType.SelectedItem.Value)

        With RVMain
            .Reset()
            .Visible = True
            .ShowParameterPrompts = False
            .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            If ddlRType.SelectedItem.Value = "T" Then
                ReportPath = "SummaryPurchaseSalesReturns"
            Else
                ReportPath = "SummaryPurchaseSalesReturnsGraph"
            End If
            .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
            .ServerReport.SetParameters(New ReportParameter() {TypeID, OrgId, Agency, SID, pFromDate, pToDate})
            .ServerReport.Refresh()

        End With
    End Sub
    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        Exit Sub
    End Sub

    Private Sub ddlFromMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFromMonth.SelectedIndexChanged
        If ddlType.SelectedItem.Value = "W" Then
            Loadweek()
        End If
    End Sub
End Class