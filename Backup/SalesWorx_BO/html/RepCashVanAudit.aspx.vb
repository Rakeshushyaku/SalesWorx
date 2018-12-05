Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms

Partial Public Class RepCashVanAudit
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Private ReportPath As String = "CashVanAuditSurvey"
    Private Const PageID As String = "P210"
    Public DivHTML As String = "No audit information found."
    ' Private Const PageID As String = "P97"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
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
            Exit Sub
        End If
        If Not IsPostBack Then
            Dim HasPermission As Boolean = False
            'SalesWorx.BO.Common.ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            'If Not HasPermission Then
            'Err_No = 500
            'Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            ' End If
            'ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
            Try
                ObjCommon = New SalesWorx.BO.Common.Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)

                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New ListItem("-- Select a value --"))


                'ddlVan.DataSource = ObjCommon.GetAllVan(Err_No, Err_Desc, SubQry)
                'ddlVan.DataBind()
                'ddlVan.Items.Insert(0, New ListItem("-- Select a Van --"))
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

    Protected Sub SearchBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SearchBtn.Click
        RVMain.Reset()
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
            Exit Sub
        End If
        If ddlOrganization.SelectedIndex = 0 Then
            MessageBoxValidation("Please select an organization")
            Exit Sub
        End If


        If ddlVan.SelectedIndex > 0 Then
            InitReportViewer()
        Else
            MessageBoxValidation("Please select a van")
        End If
    End Sub
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
            Exit Sub
        End If
        Try
            If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then

                Dim objUserAccess As UserAccess
                objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
                ObjCommon = New Common()
                ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
                ddlVan.DataBind()
                ddlVan.Items.Insert(0, New ListItem("-- Select a value --"))

                RVMain.Reset()
            Else
                ddlVan.Items.Clear()
                ddlVan.Items.Insert(0, New ListItem("-- Select a value --"))
                RVMain.Reset()
            End If
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        Exit Sub
    End Sub


    Protected Sub ddlDivision_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        LoadDetails()
    End Sub

    Private Sub InitReportViewer()
        Try

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

            Dim SID As New ReportParameter
            SID = New ReportParameter("VanID", CStr(IIf(Me.ddlVan.SelectedIndex = 0, "ALL", ddlVan.SelectedValue)))

            Dim OrgName As New ReportParameter
            OrgName = New ReportParameter("OrgName", ddlOrganization.SelectedItem.Text)

            Dim Van As New ReportParameter
            Van = New ReportParameter("Van", ddlVan.SelectedItem.Text)

            With RVMain
                .Reset()
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                .ServerReport.SetParameters(New ReportParameter() {SID, OrgName, Van})
                '.ServerReport.Refresh()

            End With


        Catch Ex As Exception
            '  log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub


    Sub LoadDetails()
        ObjCommon = New SalesWorx.BO.Common.Common()
        Dim SubQry As String = "A.SalesRep_ID='" + ddlVan.SelectedValue + "'"
        'Dim dt As New DataTable
        'dt = ObjCommon.GetVanAuditReport(Err_No, Err_Desc, SubQry)

        'Dim HeaderTemplate As String = "<tr><td height='12' width='5%' class='tdstyle' style='border:1px solid;border-color: #FFFFFF'>&nbsp;</td> <td class='tdstyle'  style='border:1px solid;border-color: #FFFFFF'> $INFO$ </td></tr>"
        'Dim RowTemplate As String = "<tr> <td height='12' width='5%' class='tdstyle' style='border:1px solid;border-color: #FFFFFF'>$SLNO$</td><td class='tdstyle' width='60%'  style='border:1px solid;border-color: #FFFFFF'> $QUEST$ </td><td class='tdstyle' width='35%'  style='border:1px solid;border-color: #FFFFFF'>$ANS$</td></tr>"
        'Dim AuditTemplate As String = "<tr><td height='12' width='5%' class='tdstyle' style='border:1px solid;border-color: #FFFFFF'>&nbsp;</td> <td height='12' width='60%'  class='tdstyle' style='border:1px solid;border-color: #FFFFFF'>$INFO$</td><td class='tdstyle'  style='border:1px solid;border-color: #FFFFFF'> $ANS$ </td></tr>"

        'Dim SalesPersonName As String = ""
        'Dim DateOfAudit As String = ""
        'Dim PrevAuditDAte As String = ""
        'Dim Division_SalesOrgID As String = ""
        'Dim CheckedBy As String = ""
        'If Not IsNothing(dt) Then
        '    If dt.Rows.Count > 0 Then
        '        SalesPersonName = dt.Rows(0)("Emp_Name").ToString() & "-" & dt.Rows(0)("Emp_Code").ToString()
        '        DateOfAudit = Convert.ToDateTime(dt.Rows(0)("Survey_Timestamp")).ToString("dd/MM/yyyy")
        '        Division_SalesOrgID = dt.Rows(0)("Site").ToString()
        '        PrevAuditDAte = ObjCommon.GetPrevAuditDate(Err_No, Err_Desc, dt.Rows(0)("Survey_Timestamp").ToString(), dt.Rows(0)("SalesRep_ID"))
        '        CheckedBy = dt.Rows(0)("UserName").ToString()
        '    End If
        'End If


        'Dim sb As New StringBuilder("")
        'sb.Append("<table width='100%'  border='1' style='border:1px solid;border-color: #FFFFFF' cellpadding='6'  cellspacing = '0' > ")
        'sb.Append(HeaderTemplate.Replace("$INFO$", "Name of Salesman : " & SalesPersonName))
        'sb.Append(HeaderTemplate.Replace("$INFO$", "Organization: " & Division_SalesOrgID))
        'sb.Append("</table>")
        'sb.Append("<div height='15'>&nbsp; </div>")
        'sb.Append("<table width='100%'  border='1' style='border:1px solid;border-color: #FFFFFF' cellpadding='6'  cellspacing = '0' > ")
        'sb.Append(AuditTemplate.Replace("$INFO$", "Date of Audit").Replace("$ANS$", DateOfAudit))
        'sb.Append(AuditTemplate.Replace("$INFO$", "Date of previous audit").Replace("$ANS$", PrevAuditDAte))
        'sb.Append("</table>")
        'sb.Append("<div height='15'>&nbsp; </div>")
        'sb.Append("<table width='100%'  border='1' style='border:1px solid;border-color: #FFFFFF' cellpadding='6'  cellspacing = '0' > ")
        'Dim i As Integer = 1
        'For Each dr As DataRow In dt.Rows
        '    sb.Append(RowTemplate.Replace("$SLNO$", i.ToString()).Replace("$QUEST$", dr("Question_text").ToString()).Replace("$ANS$", dr("ResponseText").ToString()))
        '    i += 1
        'Next
        'sb.Append("</table>")
        'sb.Append("<table width='100%'  border='0' style='border:0px solid;border-color: #FFFFFF' cellpadding='6'  cellspacing = '0' > ")
        'sb.Append("<tr><td width='5%'>&nbsp;</td><td width='60%'>Checked By </td><td width='35%'>Sales Manager</td></tr>")
        'sb.Append("<tr><td width='5%'>&nbsp;</td><td width='60%'>[" + CheckedBy + "]</td><td width='35%'>&nbsp;</td></tr>")
        'sb.Append("</table>")

        'DivHTML = sb.ToString()

        '' Literal1.Text = sb.ToString()

        'ObjCommon = New SalesWorx.BO.Common.Common()
        '  Dim SubQry As String = "A.SalesRep_ID='" + ddlVan.SelectedValue + "'"
        'GVVanAudit.DataSource = ObjCommon.GetVanAuditReport(Err_No, Err_Desc, SubQry)
        'GVVanAudit.DataBind()
    End Sub

    'Protected Sub imgPrint_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgPrint.Click
    '    If Not IsNothing(DivHTML) Then
    '        Dim gridHTML As String = DivHTML.ToString().Replace("""", "'") _
    '           .Replace(System.Environment.NewLine, "")
    '        Dim sb As New StringBuilder()
    '        sb.Append("<script type = 'text/javascript'>")
    '        sb.Append("window.onload = new function(){")
    '        sb.Append("var printWin = window.open('', '', 'left=0")
    '        sb.Append(",top=0,width=1000,height=1000,status=0');")
    '        sb.Append("printWin.document.write(""")
    '        sb.Append(gridHTML)
    '        sb.Append(""");")
    '        sb.Append("printWin.document.close();")
    '        sb.Append("printWin.focus();")
    '        sb.Append("printWin.print();")
    '        sb.Append("printWin.close();};")
    '        sb.Append("</script>")
    '        ClientScript.RegisterStartupScript(Me.[GetType](), "Print", sb.ToString())
    '    End If
    'End Sub
End Class