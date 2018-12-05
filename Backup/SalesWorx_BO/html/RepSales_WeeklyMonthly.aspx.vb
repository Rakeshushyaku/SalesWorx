Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports System.Globalization
Partial Public Class RepSales_WeeklyMonthly
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager

    Dim ObjCustomer As Customer

    Private ReportPath As String = "SalesWeeklyMonthly"

    Private Const PageID As String = "P228"
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


                ddlCustomer.DataSource = Nothing
                ddlCustomer.DataSource = ObjCommon.GetOutlet(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
                ddlCustomer.DataTextField = "Outlet"
                ddlCustomer.DataValueField = "CustomerID"
                ddlCustomer.DataBind()
                ddlCustomer.Items.Insert(0, New ListItem("-- All --", "0$0"))

                ''  InitReportViewer()
                LoadYear()
                LoadfromAndTo()
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
        ddlCustomer.DataSource = Nothing
        ddlCustomer.DataSource = ObjCommon.GetOutlet(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
        ddlCustomer.DataTextField = "Outlet"
        ddlCustomer.DataValueField = "CustomerID"
        ddlCustomer.DataBind()
        ddlCustomer.Items.Insert(0, New ListItem("-- All --", "0$0"))
        ObjCommon = Nothing
        RVMain.Reset()
    End Sub

    Protected Sub ddlType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlType.SelectedIndexChanged
        LoadfromAndTo()
        RVMain.Reset()
    End Sub
    Sub LoadfromAndTo()
        ddlFromMonth.Enabled = True
        ddltoMonth.Enabled = True
        If ddlType.SelectedItem.Value = "W" Then
            LoadfromMonths()
            LoadMonths()
        ElseIf ddlType.SelectedItem.Value = "M" Then
            LoadfromMonths()
            LoadMonths()
        ElseIf ddlType.SelectedItem.Value = "Q" Then
            LoadQuarter()
        ElseIf ddlType.SelectedItem.Value = "Y" Then
            ddlFromMonth.Items.Clear()
            ddltoMonth.Items.Clear()
            ddlFromMonth.Items.Add("(Select)")
            ddltoMonth.Items.Add("(Select)")
            ddlFromMonth.Enabled = False
            ddltoMonth.Enabled = False
        End If
    End Sub
    Sub LoadQuarter()
        ddlFromMonth.Items.Clear()
        ddltoMonth.Items.Clear()
        For i As Integer = 1 To 4
            ddlFromMonth.Items.Add(New ListItem("Q" & i, i))
            ddltoMonth.Items.Add(New ListItem("Q" & i, i * 3))
        Next
    End Sub
    Sub LoadYear()
        Try
            ddlFromYear.Items.Clear()
            ddltoYear.Items.Clear()

            StartDate = (New SalesWorx.BO.Common.Common).GetMinTransDate(Err_No, Err_Desc)
            Dim CurrYear As Integer = Year(DateTime.Now)
            Dim AddYear As Integer = Year(StartDate)
            While AddYear <= CurrYear
                ddlFromYear.Items.Add(New ListItem(AddYear.ToString, AddYear.ToString))
                ddltoYear.Items.Add(New ListItem(AddYear.ToString, AddYear.ToString))
                AddYear = AddYear + 1
            End While
            If Not ddltoYear.Items.FindByValue(Year(Now)) Is Nothing Then
                ddltoYear.Items.FindByValue(Year(Now)).Selected = True
            End If

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
    Sub LoadMonths()
        
        Try
           
            ddltoMonth.Items.Clear()
            Dim CurDate As String
            CurDate = Today
            Dim i As Integer
            Dim mm As String
            Dim Alldate As Date


            If ddltoYear.SelectedItem.Value = Year(StartDate) Then
                If ddltoYear.SelectedItem.Value = Year(Now) Then
                    For i = Month(StartDate) To Month(Now)
                        mm = CStr(i)
                        mm = mm.PadLeft(2, "0")
                        mm = mm & "/01/2006"
                        Alldate = CDate(mm)
                        Dim lstItem As New ListItem(Alldate.ToString("MMM"), Alldate.ToString("MMM"))
                        ddltoMonth.Items.Add(lstItem)
                        lstItem = Nothing
                    Next
                    mm = CInt(Month(StartDate))
                    mm = mm.PadLeft(2, "0")
                    mm = mm & "/01/2006"
                    Alldate = CDate(mm)
                    If Not ddltoMonth.Items.FindByValue(Alldate.ToString("MMM")) Is Nothing Then
                        ddltoMonth.Items.FindByValue(Alldate.ToString("MMM")).Selected = True
                    End If
                Else
                    For i = Month(StartDate) To 12
                        mm = CStr(i)
                        mm = mm.PadLeft(2, "0")
                        mm = mm & "/01/2006"
                        Alldate = CDate(mm)
                        Dim lstItem As New ListItem(Alldate.ToString("MMM"), Alldate.ToString("MMM"))
                        ddltoMonth.Items.Add(lstItem)
                        lstItem = Nothing
                    Next
                    mm = CInt(Month(StartDate))
                    mm = mm.PadLeft(2, "0")
                    mm = mm & "/01/2006"
                    Alldate = CDate(mm)
                    If Not ddltoMonth.Items.FindByValue(Alldate.ToString("MMM")) Is Nothing Then
                        ddltoMonth.Items.FindByValue(Alldate.ToString("MMM")).Selected = True
                    End If
                End If
            ElseIf ddltoYear.SelectedItem.Value = Year(Now) Then
                For i = 1 To Month(Now)
                    mm = CStr(i)
                    mm = mm.PadLeft(2, "0")
                    mm = mm & "/01/2006"
                    Alldate = CDate(mm)
                    Dim lstItem As New ListItem(Alldate.ToString("MMM"), Alldate.ToString("MMM"))
                    ddltoMonth.Items.Add(lstItem)
                    lstItem = Nothing
                Next
                If ToyearSelected = "0" Then
                    mm = Month(Now)
                    mm = mm.PadLeft(2, "0")
                    mm = mm & "/01/2006"
                    Alldate = CDate(mm)
                    If Not ddltoMonth.Items.FindByValue(Alldate.ToString("MMM")) Is Nothing Then
                        ddltoMonth.Items.FindByValue(Alldate.ToString("MMM")).Selected = True
                    End If
                End If
            Else
                For i = 1 To 12
                    mm = CStr(i)
                    mm = mm.PadLeft(2, "0")
                    mm = mm & "/01/2006"
                    Alldate = CDate(mm)
                    Dim lstItem As New ListItem(Alldate.ToString("MMM"), Alldate.ToString("MMM"))
                    ddltoMonth.Items.Add(lstItem)
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

    Protected Sub ddlTo_Year_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddltoYear.SelectedIndexChanged
        ToyearSelected = "1"
        LoadMonths()
        RVMain.Reset()
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click

        If ddlOrganization.SelectedItem.Value = "0" Then
            MessageBoxValidation("Please select the organization")
            Exit Sub
        End If

        Dim fromdate As String
        Dim todate As String
        If ddlType.SelectedItem.Value = "W" Then
            fromdate = CDate(ddlFromMonth.SelectedValue & "/01/" & ddlFromYear.SelectedItem.Value).ToString("MM/dd/yyyy")
            todate = DateAdd(DateInterval.Month, 1, CDate(ddltoMonth.SelectedValue & "/01/" & ddltoYear.SelectedItem.Value)).ToString("MM/dd/yyyy")
            If DateDiff(DateInterval.Month, CDate(fromdate), CDate(todate)) > 2 Then
                MessageBoxValidation("The date range should not exceed 2 months")
                Exit Sub
            End If
        ElseIf ddlType.SelectedItem.Value = "M" Then
            fromdate = CDate(ddlFromMonth.SelectedValue & "/01/" & ddlFromYear.SelectedItem.Value).ToString("MM/dd/yyyy")
            todate = DateAdd(DateInterval.Month, 1, CDate(ddltoMonth.SelectedValue & "/01/" & ddltoYear.SelectedItem.Value)).ToString("MM/dd/yyyy")
            If DateDiff(DateInterval.Month, CDate(fromdate), CDate(todate)) > 12 Then
                MessageBoxValidation("The date range should not exceed 12 months")
                Exit Sub
            End If
        ElseIf ddlType.SelectedItem.Value = "Q" Then
            fromdate = CDate(ddlFromMonth.SelectedValue & "/01/" & ddlFromYear.SelectedItem.Value).ToString("MM/dd/yyyy")
            todate = DateAdd(DateInterval.Month, 1, CDate(ddltoMonth.SelectedValue & "/01/" & ddltoYear.SelectedItem.Value)).ToString("MM/dd/yyyy")
            If DateDiff(DateInterval.Year, CDate(fromdate), CDate(todate)) > 2 Then
                MessageBoxValidation("The date range should not exceed 2 years")
                Exit Sub
            End If
        ElseIf ddlType.SelectedItem.Value = "Y" Then
            fromdate = "01/01/" & ddlFromYear.SelectedItem.Value
            todate = DateAdd(DateInterval.Day, 1, CDate("12/31/" & ddltoYear.SelectedItem.Value))
            If DateDiff(DateInterval.Year, CDate(fromdate), CDate(todate)) > 12 Then
                MessageBoxValidation("The date range should not exceed 12 years")
                Exit Sub
            End If
        End If

        If Not (CDate(fromdate) <= CDate(todate)) Then
            MessageBoxValidation("Invalid Date range selection")
            Exit Sub
        End If

        Dim SiteID As New ReportParameter
        Dim OutletID As New ReportParameter


        If ddlCustomer.SelectedIndex <> 0 Then
            Dim Arr As Array = ddlCustomer.SelectedItem.Value.Split("$")
            SiteID = New ReportParameter("SiteUseid", CInt(Arr(1)))
            OutletID = New ReportParameter("OutletID", CInt(Arr(0)))
        Else
            SiteID = New ReportParameter("SiteUseid", 0)
            OutletID = New ReportParameter("OutletID", 0)
        End If

        Dim pFromDate As New ReportParameter
        pFromDate = New ReportParameter("FromDate", CDate(fromdate).ToString("dd-MMM-yyyy"))

        Dim pToDate As New ReportParameter
        pToDate = New ReportParameter("ToDate", CDate(todate).ToString("dd-MMM-yyyy"))

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
            .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
            .ServerReport.SetParameters(New ReportParameter() {TypeID, OrgId, OutletID, SiteID, pFromDate, pToDate})
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
End Class