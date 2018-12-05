Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Public Class SurveyAllResponses
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer
    Private ReportPath As String = "VisitDetails"
    Dim dv As New DataView
    Private Const PageID As String = "P211"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If IsNothing(Session("USER_ACCESS")) Then
                Response.Redirect("Login.aspx")
                Exit Sub
          
            End If
            If Not IsPostBack Then
                If Not Request.QueryString("o") Is Nothing AndAlso Not Request.QueryString("s") Is Nothing AndAlso Not Request.QueryString("t") Is Nothing Then
                    hfOrg.Value = Request.QueryString("o")
                    hfSurveyID.Value = Request.QueryString("s")
                    hfType.Value = Request.QueryString("t")
                    LoadDetails()
                Else
                    Response.Redirect("RepSurveyStatistics.aspx")
                End If
            End If
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
    Private Sub dgv_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvRep.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        LoadDetails()
    End Sub

    Private Sub dgv_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        LoadDetails()
    End Sub
    Private Property SortDirection() As String
        Get
            If ViewState("SortDirection") Is Nothing Then
                ViewState("SortDirection") = "ASC"
            End If
            Return ViewState("SortDirection").ToString()
        End Get
        Set(ByVal value As String)
            Dim s As String = SortDirection

            If value = "flip" Then
                s = If(s = "ASC", "DESC", "ASC")
            Else
                s = value
            End If

            ViewState("SortDirection") = s
        End Set
    End Property
    Private Sub LoadDetails()
        Try
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable

            If hfType.Value = "N" Then
                dt = ObjReport.GetCustomerSurvey(Err_No, Err_Desc, hfSurveyID.Value, hfOrg.Value)
                gvRep.DataSource = dt
                gvRep.DataBind()
            Else
                dt = ObjReport.GetAuditSurvey(Err_No, Err_Desc, hfSurveyID.Value, hfOrg.Value, "0", "0")
                gvAudit.DataSource = dt
                gvAudit.DataBind()
            End If

            Details.Visible = True

         

            If dt.Rows.Count > 0 Then
                lbl_startDate.Text = CDate(dt.Rows(0)("StartDate")).ToString("dd-MMM-yyyy")
                lbl_EndDate.Text = CDate(dt.Rows(0)("EndDate")).ToString("dd-MMM-yyyy")
                lbl_Survey1.Text = dt.Rows(0)("SurveyTitle")
            End If
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub BtnExportExcel_Click(sender As Object, e As EventArgs) Handles BtnExportExcel.Click
        Try
            Export("Excel")
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub

    Private Sub BtnExportPDF_Click(sender As Object, e As EventArgs) Handles BtnExportPDF.Click
        Try
            Export("PDF")
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub

    Sub Export(format As String)
        Dim ObjCommon As SalesWorx.BO.Common.Common
        Try
            Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
            rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), IIf(hfType.Value = "N", "CustomerSurveyDetails", "AuditSurveyDetails"))
          


            Dim params As New ReportParameter
            params = New ReportParameter("SearchParams", hfSurveyID.Value)

            Dim OrgID As New ReportParameter
            OrgID = New ReportParameter("OrgID", hfOrg.Value)

            Dim SID As New ReportParameter
            SID = New ReportParameter("SID", "ALL")

            Dim CustID As New ReportParameter
            CustID = New ReportParameter("CustID", "ALL")

            Dim Survey As New ReportParameter
            Survey = New ReportParameter("Survey", lbl_Survey1.Text)

            Dim QuestID As New ReportParameter
            QuestID = New ReportParameter("QuestID", "ALL")

            Dim OrgName As New ReportParameter
            OrgName = New ReportParameter("OrgName", "ALL")

            rview.ServerReport.SetParameters(New ReportParameter() {params, OrgID, SID, CustID, Survey, QuestID, OrgName})

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
                Response.AddHeader("Content-disposition", "attachment;filename=Responses.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=Responses.xls")
                Response.AddHeader("Content-Length", bytes.Length)
            End If
            Response.OutputStream.Write(bytes, 0, bytes.Length)
            Response.OutputStream.Flush()
            Response.OutputStream.Close()
            Response.Flush()
            Response.Close()
        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            ObjCommon = Nothing
        End Try
    End Sub

    Private Sub gvAudit_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvAudit.PageIndexChanged
        LoadDetails()
    End Sub

    Private Sub gvAudit_SortCommand(sender As Object, e As GridSortCommandEventArgs) Handles gvAudit.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        LoadDetails()
    End Sub
End Class