Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Partial Public Class RepCashVanAudit
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Private ReportPath As String = "CashvanAuditSurvey"
    Private Const PageID As String = "P210"
    Public DivHTML As String = "No audit information found."
    ' Private Const PageID As String = "P97"
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
        Dim ClientCode As String
        ClientCode = (New SalesWorx.BO.Common.Common).GetAppConfig(Err_No, Err_Desc, "CLIENT_CODE")
        ClientCode = "ASR"
        If ClientCode.Trim().ToUpper() = "ASR" Then
            Response.Redirect("RepCashVanAudit_Asr.aspx")
            Exit Sub
        End If

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
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))
                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                End If
                LoadOrgDetails()
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
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If ddlOrganization.SelectedIndex <= 0 Then
            MessageBoxValidation("Please select an organization", "Validation")
            Return bretval
        End If
        If ddlVan.SelectedIndex <= 0 Then
            MessageBoxValidation("Please select a van", "Validation")
            Return bretval
        End If
        bretval = True
        Return bretval
    End Function
    Protected Sub SearchBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            gvRep.Visible = True
            BindReport()
        Else
            lbl_noAudit.Visible = False
            Details.Visible = False
            Args.Visible = False
            gvRep.Visible = False
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

        Dim OrgName As New ReportParameter
        OrgName = New ReportParameter("OrgName", ddlOrganization.SelectedItem.Text)

        Dim VanID As New ReportParameter
        VanID = New ReportParameter("VanID", ddlVan.SelectedItem.Value)


        Dim Van As New ReportParameter
        Van = New ReportParameter("Van", ddlVan.SelectedItem.Text)

        rview.ServerReport.SetParameters(New ReportParameter() {OrgName, VanID, Van})

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
            Response.AddHeader("Content-disposition", "attachment;filename=CashVanAudit.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=CashVanAudit.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()
    End Sub

    Sub BindReport()
        lbl_org.Text = ddlOrganization.SelectedItem.Text
        lbl_van.Text = ddlVan.SelectedItem.Text
        Args.Visible = True
        rpbFilter.Items(0).Expanded = False
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        Dim ObjReport As New SalesWorx.BO.Common.Reports



        Dim dt As New DataTable
        dt = ObjReport.GetCashVanAudit(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, ddlVan.SelectedItem.Value)
        Dim finaldt As New DataTable

        finaldt.Columns.Add("Description")
        If dt.Rows.Count > 0 Then
            lbl_noAudit.Visible = False
            lbl_EmpName.Text = dt(0)("Emp_Name").ToString
            lbl_Code.Text = dt(0)("Emp_Code").ToString
            lbl_survey.Text = dt(0)("Survey_Title").ToString
            If Not dt(0)("Survey_Timestamp") Is DBNull.Value Then
                lbl_DateofAudit.Text = CDate(dt(0)("Survey_Timestamp")).ToString("dd-MMM-yyyy")
            Else
                lbl_DateofAudit.Text = ""
            End If
            If Not dt(0)("PrevAuditDate") Is DBNull.Value Then
                lbl_prev.Text = CDate(dt(0)("PrevAuditDate")).ToString("dd-MMM-yyyy")
            Else
                lbl_prev.Text = "N/A"
            End If
            If dt(0)("Status").ToString = "N" Then
                lbl_status.Text = "Not Confirmed"
            Else
                lbl_status.Text = "Confirmed"
            End If

            For Each dr As DataRow In dt.Rows
                Dim newdr As DataRow
                newdr = finaldt.NewRow

                newdr(0) = dr("Question_text").ToString
                finaldt.Rows.Add(newdr)

                Dim newdr1 As DataRow
                newdr1 = finaldt.NewRow
                If dr("Response_Type_ID") = "1" Then
                    newdr1(0) = dr("Response")
                Else
                    newdr1(0) = dr("Response_Text")
                End If

                finaldt.Rows.Add(newdr1)
            Next
            Details.Visible = True
        Else
            lbl_noAudit.Visible = True
            Details.Visible = False
        End If

            gvRep.DataSource = finaldt
            gvRep.DataBind()
    End Sub
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
            Exit Sub
        End If
        Try
            LoadOrgDetails()
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
    Sub LoadOrgDetails()
        If Not (ddlOrganization.SelectedItem.Value = "0") Then

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New SalesWorx.BO.Common.Common()
            ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
            ddlVan.DataBind()
            ddlVan.Items.Insert(0, New RadComboBoxItem("Select Van", "0"))


        Else
            ddlVan.Items.Clear()
            ddlVan.Items.Insert(0, New RadComboBoxItem("Select Van", "0"))

        End If
    End Sub


    Protected Sub ddlDivision_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        LoadDetails()
    End Sub

   
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
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

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        ddlVan.ClearSelection()
        LoadOrgDetails()
        lbl_noAudit.Visible = False
        Details.Visible = False
        Args.Visible = False
        gvRep.Visible = False
    End Sub
End Class