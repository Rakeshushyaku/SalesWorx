Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Partial Public Class RepRouteMaster
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "RouteMaster"

    Private Const PageID As String = "P214"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single

    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private Sub RepRouteMaster_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
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
                Chk_Organization.DataTextField = "Description"
                Chk_Organization.DataValueField = "MAS_Org_ID"
                Chk_Organization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                Chk_Organization.DataBind()

                ddl_month.Items.Clear()
                Dim Index As Integer = 0
                For i As Integer = Year(Now) - 1 To Year(Now)
                    If i = Year(Now) - 1 Then
                        For j As Integer = 1 To 12
                            ddl_month.Items.Add(New ListItem(CDate(j.ToString & "/01/" & i.ToString).ToString("MMM-yyyy")))
                            Index = Index + 1
                        Next
                    ElseIf i = Year(Now) Then
                        For j As Integer = 1 To Month(Now)
                            ddl_month.Items.Add(New ListItem(CDate(j.ToString & "/01/" & i.ToString).ToString("MMM-yyyy")))
                            Index = Index + 1
                        Next
                    End If
                Next
                If Not ddl_month.Items.FindByText(Now.ToString("MMM-yyyy")) Is Nothing Then
                    ddl_month.Items.FindByText(Now.ToString("MMM-yyyy")).Selected = True
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

    Protected Sub Chk_Organization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Chk_Organization.SelectedIndexChanged
        RVMain.Visible = False
        Dim OrgIds As String = ""
        For Each Org As ListItem In Chk_Organization.Items
            If Org.Selected = True Then
                OrgIds = OrgIds & Org.Value & ","
            End If
        Next
        If Trim(OrgIds) <> "" Then
            OrgIds = OrgIds.Substring(0, Len(OrgIds) - 1)
        End If
        If OrgIds = "" Then
            OrgIds = "-1"
        End If
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        ObjCommon = New Common()
        chkSalesRep.DataTextField = "SalesRep_Name"
        chkSalesRep.DataValueField = "SalesRep_ID"
        chkSalesRep.DataSource = ObjCommon.GetVanFromMultipleOrg(Err_No, Err_Desc, OrgIds, objUserAccess.UserID)
        chkSalesRep.DataBind()

    End Sub

    Protected Sub SearchBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SearchBtn.Click
        Dim VanIds As String = ""
        For Each Van As ListItem In chkSalesRep.Items
            If Van.Selected = True Then
                VanIds = VanIds & Van.Value & ","
            End If
        Next
        If Trim(VanIds) <> "" Then
            VanIds = VanIds.Substring(0, Len(VanIds) - 1)
        End If
        If VanIds = "" Then
            VanIds = "-1"
        End If
        Dim Fromdate As String
        Dim Todate As String
        Fromdate = "01-" & ddl_month.SelectedItem.Text
        Todate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(Fromdate))).ToString("dd-MMM-yyyy")


        Dim VanID As New ReportParameter
        VanID = New ReportParameter("FSRID", VanIds)

        Dim Start_Date As New ReportParameter
        Start_Date = New ReportParameter("Start_Date", Fromdate)

        Dim End_Date As New ReportParameter
        End_Date = New ReportParameter("End_Date", Todate)

        Dim DistributorCode As New ReportParameter
        DistributorCode = New ReportParameter("DistributorCode", txt_distribCode.Text)

        With RVMain
            .Reset()
            .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
            .ServerReport.SetParameters(New ReportParameter() {VanID, Start_Date, End_Date, DistributorCode})
            '.ServerReport.Refresh()
            .Visible = True
        End With
    End Sub
End Class