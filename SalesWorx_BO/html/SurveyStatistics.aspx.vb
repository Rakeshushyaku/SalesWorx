Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Partial Public Class SurveyStatistics
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim objSurvey As Survey
    'Dim SortField As String = ""
    'Dim SortFieldDtl As String = ""
    Public DisplayHTML As String
    Private Const PageID As String = "P98"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private Property SortField() As String
        Get
            If ViewState("SortColumn") Is Nothing Then
                ViewState("SortColumn") = ""
            End If
            Return ViewState("SortColumn").ToString()
        End Get
        Set(ByVal value As String)
            ViewState("SortColumn") = value
        End Set
    End Property
    Private Property SortFieldDtl() As String
        Get
            If ViewState("SortColumn1") Is Nothing Then
                ViewState("SortColumn1") = ""
            End If
            Return ViewState("SortColumn1").ToString()
        End Get
        Set(ByVal value As String)
            ViewState("SortColumn1") = value
        End Set
    End Property
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

                Dim cls As New Purpose
                'ddlTypeCode.DataSource = cls.BindSurveyTypeCode
                'ddlTypeCode.DataTextField = "Key"
                'ddlTypeCode.DataValueField = "Value"
                'ddlTypeCode.DataBind()
                cls = Nothing
                LoadSurvey()
                If Not ddlSurvey.Items.Contains(New ListItem("-- Select a Survey --")) Then
                    ddlSurvey.Items.Insert(0, New ListItem("-- Select a Survey --"))
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

                ErrorResource = Nothing
            End Try
        End If
    End Sub
    Private Sub LoadSurvey()
        Dim SearchQuery As String = ""
        objSurvey = New Survey()
        'SearchQuery = " And Survey_Type_Code='" & ddlTypeCode.SelectedValue & "'"
        ddlSurvey.DataSource = objSurvey.GetAllSurvey(Err_No, Err_Desc, SearchQuery)
        ddlSurvey.DataBind()
        If Not ddlSurvey.Items.Contains(New ListItem("-- Select a Survey --")) Then
            ddlSurvey.Items.Insert(0, New ListItem("-- Select a Survey --"))
        End If
        lblStartDateval.Text = ""
        lblEndDateval.Text = ""
        objSurvey = Nothing
    End Sub

    'Private Sub ddlTypeCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTypeCode.SelectedIndexChanged
    '    LoadSurvey()
    'End Sub

    Private Sub ddlSurvey_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSurvey.SelectedIndexChanged
        If ddlSurvey.SelectedValue <> "-- Select a Survey --" Then
            Dim dt As New DataTable
            objSurvey = New Survey()
            dt = objSurvey.GetAllSurvey(Err_No, Err_Desc, "And Survey_ID=" & ddlSurvey.SelectedValue)
            Dim SurveyType As String = ""
            Dim dr As DataRow = Nothing
            If dt.Rows.Count > 0 Then
                dr = dt.Rows(0)
                lblStartDateval.Text = String.Format("{0:dd/MM/yyyy}", dr("Start_Time"))
                lblEndDateval.Text = String.Format("{0:dd/MM/yyyy}", dr("End_Time"))
                SurveyType = dr("Survey_Type_Code").ToString()
            End If
            dr = Nothing
            dt = Nothing
            objSurvey = Nothing
            lblStartDatetxt.Visible = True
            lblEndDatetxt.Visible = True
            If SurveyType = "A" Then
                SurveyAuditStatisticsGraph()
            Else
                SurveyStatisticsGraph()
            End If
        End If
    End Sub
    Private Sub SurveyAuditStatisticsGraph()
        objSurvey = New Survey()
        If ddlSurvey.SelectedValue <> "" Then
            DisplayHTML = "<table width='100%' border='0' cellspacing='1' cellpadding='1'>"
            Dim QuestionTemplate As String, ResponseHeaderTemplate As String, TextResponseTemplate As String, NonTextResponseTemplate As String
            Dim SurveyStats As String
            Dim Survey_ID As Integer
            Dim SurveyedCount As Integer
            Dim ImageUrl As String
            ImageUrl = "../images/"
            Survey_ID = ddlSurvey.SelectedValue
            QuestionTemplate = ""
            ResponseHeaderTemplate = "<tr><td class='txtSMBold' colspan=3 bgcolor='#CFCFCF'>$QUES_TEXT</td><td class='txtSMBold' width='100px' bgcolor='#CFCFCF'>Response %age</td><td class='txtSMBold' width='100px' bgcolor='#CFCFCF'>Response Count</td></tr>"
            TextResponseTemplate = "<tr><td class='txtSM' colspan=5 bgcolor='#CFCFCF'><b>$QUES_TEXT</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class='txtSM' onClick=""showResponses('$QUES_ID','A');"" style='cursor:pointer; text-decoration:underline'>Click here to view the responses</span></td></tr>"
            NonTextResponseTemplate = "<tr><td class='txtSM' colspan=2 align=right>$RESP_TEXT</td><td class='txtSM' align='left'><img src='" & ImageUrl & "imgStatBar.gif' width='$RESP_PERCENTAGE%' height='$STAT_BARpx'></td><td class='txtSM' width='100px' align=center>$RESP_PERCENTAGE%</td><td class='txtSM' width='100px' align=center>$RESP_COUNT</td></tr>"
            SurveyStats = objSurvey.GetSurveyAuditStats(Survey_ID, QuestionTemplate, ResponseHeaderTemplate, TextResponseTemplate, NonTextResponseTemplate)
            DisplayHTML = DisplayHTML & "<tr><td colspan=5 width='100%'><table width='100%' border='1' cellspacing='1' cellpadding='3'>"
            SurveyedCount = objSurvey.SurveyedCount
            If SurveyedCount > 0 Then
                DisplayHTML = DisplayHTML & SurveyStats
            End If

            DisplayHTML = DisplayHTML & "</table></td></tr></table>"
        End If
        objSurvey = Nothing
    End Sub
    Private Sub SurveyStatisticsGraph()
        objSurvey = New Survey()
        If ddlSurvey.SelectedValue <> "" Then
            DisplayHTML = "<table width='100%' border='0' cellspacing='1' cellpadding='1'>"
            Dim QuestionTemplate As String, ResponseHeaderTemplate As String, TextResponseTemplate As String, NonTextResponseTemplate As String
            Dim SurveyStats As String
            Dim Survey_ID As Integer
            Dim SurveyedCount As Integer
            Dim ImageUrl As String
            ImageUrl = "../images/"
            Survey_ID = ddlSurvey.SelectedValue
            QuestionTemplate = ""
            ResponseHeaderTemplate = "<tr><td class='txtSMBold' colspan=3 bgcolor='#CFCFCF'>$QUES_TEXT</td><td class='txtSMBold' width='100px' bgcolor='#CFCFCF'>Response %age</td><td class='txtSMBold' width='100px' bgcolor='#CFCFCF'>Response Count</td></tr>"
            TextResponseTemplate = "<tr><td class='txtSM' colspan=5 bgcolor='#CFCFCF'><b>$QUES_TEXT</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class='txtSM' onClick=""showResponses('$QUES_ID','N');"" style='cursor:pointer; text-decoration:underline'>Click here to view the responses</span></td></tr>"
            NonTextResponseTemplate = "<tr><td class='txtSM' colspan=2 align=right>$RESP_TEXT</td><td class='txtSM' align='left'><img src='" & ImageUrl & "imgStatBar.gif' width='$RESP_PERCENTAGE%' height='$STAT_BARpx'></td><td class='txtSM' width='100px' align=center>$RESP_PERCENTAGE%</td><td class='txtSM' width='100px' align=center>$RESP_COUNT</td></tr>"
            SurveyStats = objSurvey.GetSurveyStats(Survey_ID, QuestionTemplate, ResponseHeaderTemplate, TextResponseTemplate, NonTextResponseTemplate)
            DisplayHTML = DisplayHTML & "<tr><td colspan=5 width='100%'><table width='100%' border='1' cellspacing='1' cellpadding='3'>"
            SurveyedCount = objSurvey.SurveyedCount
            If SurveyedCount > 0 Then
                DisplayHTML = DisplayHTML & SurveyStats
            End If

            DisplayHTML = DisplayHTML & "</table></td></tr></table>"
        End If
        objSurvey = Nothing
    End Sub

    Protected Sub SearchBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SearchBtn.Click
        If ddlSurvey.SelectedValue <> "-- Select a Survey --" Then
            Dim dt As New DataTable
            objSurvey = New Survey()
            dt = objSurvey.GetAllSurvey(Err_No, Err_Desc, "And Survey_ID=" & ddlSurvey.SelectedValue)
            Dim SurveyType As String = ""
            Dim dr As DataRow = Nothing
            If dt.Rows.Count > 0 Then
                dr = dt.Rows(0)
                lblStartDateval.Text = String.Format("{0:dd/MM/yyyy}", dr("Start_Time"))
                lblEndDateval.Text = String.Format("{0:dd/MM/yyyy}", dr("End_Time"))
                SurveyType = dr("Survey_Type_Code").ToString()
            End If
            dr = Nothing
            dt = Nothing
            objSurvey = Nothing
            lblStartDatetxt.Visible = True
            lblEndDatetxt.Visible = True
            If SurveyType = "A" Then
                SurveyAuditStatisticsGraph()
            Else
                SurveyStatisticsGraph()
            End If
        End If
    End Sub

    Protected Sub imgPrint_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgPrint.Click
        If Not IsNothing(DisplayHTML) Then
            Dim gridHTML As String = DisplayHTML.ToString().Replace("""", "'") _
               .Replace(System.Environment.NewLine, "")
            Dim sb As New StringBuilder()
            sb.Append("<script type = 'text/javascript'>")
            sb.Append("window.onload = new function(){")
            sb.Append("var printWin = window.open('', '', 'left=0")
            sb.Append(",top=0,width=1000,height=1000,status=0');")
            sb.Append("printWin.document.write(""")
            sb.Append(gridHTML)
            sb.Append(""");")
            sb.Append("printWin.document.close();")
            sb.Append("printWin.focus();")
            sb.Append("printWin.print();")
            sb.Append("printWin.close();};")
            sb.Append("</script>")
            ClientScript.RegisterStartupScript(Me.[GetType](), "Print", sb.ToString())
        End If
    End Sub
End Class