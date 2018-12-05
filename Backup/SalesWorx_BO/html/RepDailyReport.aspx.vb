Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Partial Public Class RepDailyReport
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "DailyReport"

    Private Const PageID As String = "P216"
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
                txt_fromDate.Text = Now.ToString("dd-MMM-yyyy")
                txt_ToDate.Text = Now.ToString("dd-MMM-yyyy")
                ObjCommon = New Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddl_org.DataTextField = "Description"
                ddl_org.DataValueField = "MAS_Org_ID"
                ddl_org.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddl_org.DataBind()
                ddl_org.Items.Insert(0, "(Select)")
                ddl_org.Items(0).Value = 0
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
    Protected Sub Chk_Organization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_org.SelectedIndexChanged
        RVMain.Visible = False
        Dim OrgIds As String = ""
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        ObjCommon = New Common()
        chkSalesRep.DataTextField = "SalesRep_Name"
        chkSalesRep.DataValueField = "SalesRep_ID"
        chkSalesRep.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddl_org.SelectedItem.Value, objUserAccess.UserID)
        chkSalesRep.DataBind()

    End Sub

    Protected Sub SearchBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SearchBtn.Click

        If ddl_org.SelectedItem.Value = "0" Then
            MessageBoxValidation("Please select the organization")
            SetFocus(ddl_org)
            Exit Sub
        End If

        If Not IsDate(txt_fromDate.Text) Then
            MessageBoxValidation("Enter valid ""From date"".")
            SetFocus(txt_fromDate)
            Exit Sub
        End If

        If Not IsDate(txt_ToDate.Text) Then
            MessageBoxValidation("Enter valid ""To date"".")
            SetFocus(txt_ToDate)
            Exit Sub
        End If

        If CDate(txt_fromDate.Text) > CDate(txt_ToDate.Text) Then
            MessageBoxValidation("Start Date should not be greater than End Date.")
            Exit Sub
        End If

        Dim VanIds As String = ""
        For Each Van As ListItem In chkSalesRep.Items
            If Van.Selected = True Then
                VanIds = VanIds & Van.Value & ","
            End If
        Next
        'If Trim(VanIds) <> "" Then
        '    VanIds = VanIds.Substring(0, Len(VanIds) - 1)
        'End If
        If VanIds = "" Then
            VanIds = "-1"
        End If
        Dim Fromdate As String
        Dim Todate As String
        Fromdate = txt_fromDate.Text
        Todate = txt_ToDate.Text

        Dim OrgID As New ReportParameter
        OrgID = New ReportParameter("OrgId", ddl_org.SelectedItem.Value)

        Dim VanID As New ReportParameter
        VanID = New ReportParameter("FSRID", VanIds)

        Dim Start_Date As New ReportParameter
        Start_Date = New ReportParameter("Start_Date", Fromdate)

        Dim End_Date As New ReportParameter
        End_Date = New ReportParameter("End_Date", Todate)

        With RVMain
            .Reset()
            .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
            .ServerReport.SetParameters(New ReportParameter() {VanID, Start_Date, End_Date, OrgID})
            '.ServerReport.Refresh()
            .Visible = True
        End With
    End Sub
    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        MpInfoError.Show()
        Exit Sub
    End Sub
End Class