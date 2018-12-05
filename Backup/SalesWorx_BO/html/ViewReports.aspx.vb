Imports System.Text.RegularExpressions
Imports System.Net
Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports Microsoft.Reporting.WebForms

Partial Public Class ViewReports
    Inherits System.Web.UI.Page

    Private Const ModuleName As String = "ViewReports.aspx"
    Private PageID As String = ""
    Private ReportPath As String = ""
    Dim Err_No As Long
    Dim Err_Desc As String
    Public Sub SetReportDetails(ByVal path As String)
        Me.ReportPath = path
    End Sub

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
        If Session.Item("USER_ACCESS") Is Nothing Then
            Session.Add("BringmeBackHere", ModuleName)
            Response.Redirect("Login.aspx", False)
            Exit Sub
        End If

        GetPageIDAndReportName(Request.QueryString("id"), Me.PageID, Me.ReportPath)
        If ((Not Request.QueryString("title") Is Nothing) And (Request.QueryString("title") <> String.Empty)) Then
            lblTitle.Text = Request.QueryString("title").ToString()
        End If
        If Not HasAuthentication() Then
            Err_No = 500
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
        End If
        'If ((Not Request.QueryString("title") Is Nothing) And (Request.QueryString("title") <> String.Empty)) Then
        '    lblTitle.Text = Request.QueryString("title").ToString()
        'End If


        'Dim HasPermission As Boolean = False
        'CBDSMS.Common.ManageAuthentication.HasPermission(Session("UserRole"), PageID, HasPermission)
        'If Not HasPermission Then

        '    Response.Redirect("Welcome.aspx", False)
        '    Exit Sub
        'End If


        InitReportViewer()
        'RVMain.ShowReportBody = False
        'If IsPostBack Then
        '    RVMain.ShowReportBody = True
        'End If

    End Sub
    Private Function HasAuthentication() As Boolean
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
    End Function
    Private Sub InitReportViewer()
        Try

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim myParamUserId As New ReportParameter
            myParamUserId = New ReportParameter("Uid", objUserAccess.UserID)


            If Not Me.IsPostBack Then
                With RVMain
                    .Reset()
                    .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                    .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                    .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                    .ServerReport.SetParameters(New ReportParameter() {myParamUserId})
                    '.ServerReport.Refresh()

                End With

            End If
        Catch Ex As Exception
            '  log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub

    Private Sub GetPageIDAndReportName(ByVal encryptedID As String, ByRef pageID As String, ByRef reportName As String)
        pageID = ""
        reportName = ""
        encryptedID = encryptedID.Replace(" ", "+")
        If Not String.IsNullOrEmpty(encryptedID) Then
            Dim decryptedID As String = (New Crypto()).DecryptReportName(encryptedID)

            If Not String.IsNullOrEmpty(decryptedID) Then

                Dim oMatch As Match = Regex.Match(decryptedID, "<p>(.*?)</p>")
                If Not oMatch Is Nothing AndAlso Not oMatch.Groups Is Nothing AndAlso oMatch.Groups.Count > 1 Then
                    pageID = oMatch.Groups(1).Value

                    oMatch = Regex.Match(decryptedID, "<r>(.*?)</r>")
                    If Not oMatch Is Nothing AndAlso Not oMatch.Groups Is Nothing AndAlso oMatch.Groups.Count > 1 Then
                        reportName = oMatch.Groups(1).Value

                    End If
                End If
            End If
        End If
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        DatePickers.Value = GetDateParameters()

    End Sub
    Private Function GetDateParameters() As String
        ' I'm assuming report view control id as reportViewer
        Dim dtp As String = ""
        For Each info As ReportParameterInfo In RVMain.ServerReport.GetParameters()
            If info.DataType = ParameterDataType.DateTime Then
                dtp = dtp & String.Format("[{0}]", info.Prompt) & ","
            End If
        Next
       
        Return dtp
    End Function
End Class