Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports System.Globalization
Partial Public Class RepSKUwiseSales
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager

    Dim ObjCustomer As Customer

    Private ReportPath As String = "SKUWiseSales"

    Private Const PageID As String = "P229"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single
    Dim StartDate As Date
    Dim ToyearSelected As String
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub RepSKUwiseSales_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
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
            RVMain.Visible = False
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


                ddlSKU.DataSource = Nothing
                ddlSKU.DataSource = ObjCommon.GetSKU(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
                ddlSKU.DataTextField = "SKU"
                ddlSKU.DataValueField = "Inventory_item_ID"
                ddlSKU.DataBind()
                ddlSKU.Items.Insert(0, New ListItem("-- All --", "0"))

                ''  InitReportViewer()
                LoadYear()
                LoadfromMonths()
                LoadMonths()
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

    Private Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrganization.SelectedIndexChanged
        Dim ObjCommon As New Common
        ddlSKU.DataSource = Nothing
        ddlSKU.DataSource = ObjCommon.GetSKU(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
        ddlSKU.DataTextField = "SKU"
        ddlSKU.DataValueField = "Inventory_item_ID"
        ddlSKU.DataBind()
        ddlSKU.Items.Insert(0, New ListItem("-- All --", "0"))
        ObjCommon = Nothing
        RVMain.Visible = False
    End Sub

    Private Sub ddlFromYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFromYear.SelectedIndexChanged
        LoadfromMonths()
        LoadMonths()
        RVMain.Visible = False
    End Sub

    Private Sub ddltoYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddltoYear.SelectedIndexChanged
        ToyearSelected = "1"
        LoadMonths()
        RVMain.Visible = False
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If ddlOrganization.SelectedItem.Value = "0" Then
            MessageBoxValidation("Please select the organization")
            Exit Sub
        End If

        Dim fromdate As String
        Dim todate As String
        fromdate = CDate(ddlFromMonth.SelectedValue & "/01/" & ddlFromYear.SelectedItem.Value).ToString("MM/dd/yyyy")
        todate = DateAdd(DateInterval.Month, 1, CDate(ddltoMonth.SelectedValue & "/01/" & ddltoYear.SelectedItem.Value)).ToString("MM/dd/yyyy")
        If DateDiff(DateInterval.Month, CDate(fromdate), CDate(todate)) > 12 Then
            MessageBoxValidation("The date range should not exceed 12 months")
            Exit Sub
        End If
        If Not (CDate(fromdate) <= CDate(todate)) Then
            MessageBoxValidation("Invalid Date range selection")
            Exit Sub
        End If

        Dim pFromDate As New ReportParameter
        pFromDate = New ReportParameter("FromDate", CDate(fromdate).ToString("dd-MMM-yyyy"))

        Dim pToDate As New ReportParameter
        pToDate = New ReportParameter("ToDate", CDate(todate).ToString("dd-MMM-yyyy"))

        Dim OrgId As New ReportParameter
        OrgId = New ReportParameter("OrgID", CStr(ddlOrganization.SelectedItem.Value.ToString()))

        Dim SKUID As New ReportParameter
        SKUID = New ReportParameter("SKUID", ddlSKU.SelectedItem.Value)

        With RVMain
            .Reset()
            .Visible = True
            .ShowParameterPrompts = False
            .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
            .ServerReport.SetParameters(New ReportParameter() {OrgId, SKUID, pFromDate, pToDate})
            '.ServerReport.Refresh()

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