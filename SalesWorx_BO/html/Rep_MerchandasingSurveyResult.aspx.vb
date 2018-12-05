Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Imports OfficeOpenXml

Public Class Rep_MerchandasingSurveyResult
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer
    Private ReportPath As String = "MerchandasingReport"
    Dim dv As New DataView
    Private Const PageID As String = "P384"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
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

            If Not Request.QueryString("SurveySession") Is Nothing Then
                BindReport()
            End If
            Dim urlstr As String = "Rep_MerchandasingSurveyResp.aspx?id=1"
            If Not Request.QueryString("OrgID") Is Nothing Then
                urlstr = urlstr & "&OrgID=" & Request.QueryString("OrgID")
            End If
            If Not Request.QueryString("SurveyID") Is Nothing Then
                urlstr = urlstr & "&SurveyID=" & Request.QueryString("SurveyID")
            End If
            If Not Request.QueryString("From") Is Nothing Then
                urlstr = urlstr & "&From=" & Request.QueryString("From")
            End If
            If Not Request.QueryString("To") Is Nothing Then
                urlstr = urlstr & "&To=" & Request.QueryString("To")
            End If
            If Not Request.QueryString("Van") Is Nothing Then
                urlstr = urlstr & "&Van=" & Request.QueryString("Van")
            End If
            If Not Request.QueryString("CustID") Is Nothing Then
                urlstr = urlstr & "&CustID=" & Request.QueryString("CustID")
            End If
            If Not Request.QueryString("SiteID") Is Nothing Then
                urlstr = urlstr & "&SiteID=" & Request.QueryString("SiteID")
            End If
            hyp_back.NavigateUrl = urlstr
        End If

    End Sub
    Sub BindReport()

        Dim ObjReport As New SalesWorx.BO.Common.Reports
        Dim dtSession As New DataTable
        dtSession = ObjReport.GetMerchandasingSessionDetails(Err_No, Err_Desc, Request.QueryString("SurveySession"))
        If dtSession.Rows.Count > 0 Then
            Dim dRow As DataRow = dtSession.Rows(0)
            lbl_CusName.Text = dRow("Customer_Name").ToString
            lbl_CusNo.Text = dRow("Customer_No").ToString
            lbl_Date.Text = CDate(dRow("Started_At")).ToString("dd-MMM-yyyy hh:mm tt")
            lbl_Survey.Text = dRow("survey_title").ToString
            lbl_Van.Text = dRow("SalesRep_Name").ToString
        End If
        Dim dt As New DataTable
        dt = ObjReport.GetMerchandasingResult(Err_No, Err_Desc, Request.QueryString("SurveySession"))
        gvRep.DataSource = dt
        gvRep.DataBind()

        ObjReport = Nothing
    End Sub

    Private Sub gvRep_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles gvRep.ItemDataBound
        If TypeOf e.Item Is GridGroupHeaderItem Then
            Dim item As GridGroupHeaderItem = TryCast(e.Item, GridGroupHeaderItem)
            Dim k As String = item.GroupIndex
            If k.IndexOf("_") <= 0 Then
                item.CssClass = "FirstGroupHead"

            End If
        End If
    End Sub

    Private Sub gvRep_PageIndexChanged(sender As Object, e As Telerik.Web.UI.GridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        BindReport()
    End Sub
    Private Sub BtnExportPDF_Click(sender As Object, e As EventArgs) Handles BtnExportPDF.Click
        Try
            Export("PDF")
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub
    Private Sub BtnExportExcel_Click(sender As Object, e As EventArgs) Handles BtnExportExcel.Click
        Try
            Export("Excel")
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub
    Sub Export(format As String)
        Dim ObjCommon As SalesWorx.BO.Common.Common
        Try

            ObjCommon = New SalesWorx.BO.Common.Common()
            Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
            rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)


            Dim Survey_Session_ID As New ReportParameter
            Survey_Session_ID = New ReportParameter("Survey_Session_ID", Request.QueryString("SurveySession"))



            rview.ServerReport.SetParameters(New ReportParameter() {Survey_Session_ID})

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
                Response.AddHeader("Content-disposition", "attachment;filename=MerchandasingReport.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=MerchandasingReport.xls")
                Response.AddHeader("Content-Length", bytes.Length)
            End If
            Response.OutputStream.Write(bytes, 0, bytes.Length)
            Response.OutputStream.Flush()
            Response.OutputStream.Close()
            Response.Flush()
            Response.Close()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        Finally
            ObjCommon = Nothing
        End Try
    End Sub
    Private Sub BtnExportBiffExcel_Click(sender As Object, e As EventArgs) Handles BtnExportBiffExcel.Click
        Try
            Dim tblData As New DataTable
        Dim ObjReport As New SalesWorx.BO.Common.Reports
        Dim dt As New DataTable
        dt = ObjReport.ExportToExcelMerchandasingResult(Err_No, Err_Desc, Request.QueryString("SurveySession"))

            tblData = dt.DefaultView.ToTable(False, "Survey", "VAN", "Survey_At", "Customer_No", "Customer_Name", "Batch1", "group_text", "Question", "Response1", "Comment")


        For Each col In tblData.Columns
            If col.ColumnName = "Survey_At" Then
                    col.ColumnName = "Surveyed At"
            End If
                If col.ColumnName = "Customer_No" Then
                    col.ColumnName = "Customer No"
                End If
            If col.ColumnName = "Customer_Name" Then
                col.ColumnName = "Customer Name"
            End If
            If col.ColumnName = "Batch" Then
                col.ColumnName = "Agency/Brand"
            End If
            If col.ColumnName = "group_text" Then
                col.ColumnName = "Group Text"
            End If
            If col.ColumnName = "Response1" Then
                col.ColumnName = "Response"
            End If
                If col.ColumnName = "Batch1" Then
                    col.ColumnName = "Batch"
                End If
        Next


        If tblData.Rows.Count > 0 Then

            Using package As New ExcelPackage()
                ' add a new worksheet to the empty workbook
                Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
                Worksheet.Cells("A1").LoadFromDataTable(tblData, True)
                    Worksheet.Column(3).Style.Numberformat.Format = "dd-MMM-yyyy"

                    Dim str As String
                    For i = 1 To (tblData.Rows.Count + 1)

                        str = Worksheet.Cells.Item("I" & i).Value
                        If str.ToUpper().Contains("HTTP") Then
                            Worksheet.Cells.Item("I" & i).Style.Font.UnderLine = True
                            Worksheet.Cells.Item("I" & i).Style.Font.Color.SetColor(System.Drawing.Color.Blue)
                            Worksheet.Cells(i, 9).Formula = "HYPERLINK(""" & str & """,""" & str & """)"
                        End If

                    Next

                    '  Worksheet.Column(12).Style.Numberformat.Format = "dd-MMM-yyyy"
                    Worksheet.Cells.AutoFitColumns()

                    Response.Clear()
                    Response.Buffer = True
                    Response.Charset = ""

                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    Response.AddHeader("content-disposition", "attachment;filename= MerchandasingReport.xlsx")

                    Using MyMemoryStream As New MemoryStream()
                        package.SaveAs(MyMemoryStream)
                        MyMemoryStream.WriteTo(Response.OutputStream)
                        Response.AddHeader("Content-Length", MyMemoryStream.Length)
                        Response.Flush()
                        Response.Close()
                    End Using
                End Using
            End If
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub

End Class