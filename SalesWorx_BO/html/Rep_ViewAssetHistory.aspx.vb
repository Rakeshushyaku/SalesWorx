Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Public Class Rep_ViewAssetHistory
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer
    Private ReportPath As String = "AssetHistory"
    Dim dv As New DataView
    Private Const PageID As String = "P206"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    'Protected Sub lbTradeImages_Click(sender As Object, e As EventArgs) Handles lbTradeImages.Click

    '    Me.MapWindow.VisibleOnPageLoad = True
    'End Sub

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
            '      ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())


            If Not Request.QueryString("ID") Is Nothing And Not Request.QueryString("ID") Is Nothing Then
                hfAssetID.Value = Request.QueryString("ID")
                hfOrgID.Value = Request.QueryString("OrgID")
                Dim ObjRpt As New SalesWorx.BO.Common.Reports
                Dim dt_dates As DataTable = ObjRpt.GetAssetViewHistory_Dates(Err_No, Err_Desc, hfOrgID.Value, hfAssetID.Value)
                If dt_dates.Rows.Count > 0 Then
                    Dim minstr As String = ""
                    Dim maxstr As String = ""
                    Dim min As DateTime
                    Dim max As DateTime
                    minstr = dt_dates.Rows(0)("MINdate")
                    maxstr = dt_dates.Rows(0)("MAXdate")
                    min = CDate(minstr)
                    max = CDate(maxstr)

                    If max < DateAdd(DateInterval.Day, -1 * 30, Now) Then

                        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * 30, max)
                        txtToDate.SelectedDate = max
                    Else
                        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * 30, Now)
                        txtToDate.SelectedDate = Now()
                    End If
                End If


                
                LoadDetails()
            Else
                Response.Redirect("RepAssets.aspx")
            End If



        End If
    End Sub

    Private Sub LoadDetails()
        Dim ObjReport As New SalesWorx.BO.Common.Reports
        Try
            '' Loading Visit Details  

            Try
                Dim dtAssetHistory As DataTable = ObjReport.GetAssetViewHistory(Err_No, Err_Desc, hfOrgID.Value, hfAssetID.Value, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))

                If dtAssetHistory IsNot Nothing AndAlso dtAssetHistory.Rows.Count > 0 Then
                    Dim dRow As DataRow = dtAssetHistory.Rows(0)
                    lbl_CusName.Text = dRow("CustomerName")
                    lbl_CusNo.Text = dRow("Customer_No")
                    lbl_AssetType.Text = dRow("AssetType")
                    lbl_AssetCode.Text = dRow("Asset_Code")
                    lbl_Description.Text = dRow("Description")
                    lbl_ChangeType.Text = dRow("ChangeType")
                    lbl_FromDate.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
                    lbl_ToDate.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")
                    BindGrid()
                End If

            Catch ex As Exception
                log.ErrorFormat("Error in Visit Loading // {0}", ex.Message)
            End Try


        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
    


   

    Private Sub BindGrid()
        Dim ObjReport As New SalesWorx.BO.Common.Reports
        Try
            Dim dtAssetHistory As DataTable = ObjReport.GetAssetViewHistory(Err_No, Err_Desc, hfOrgID.Value, hfAssetID.Value, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))
            gvRep.DataSource = dtAssetHistory
            gvRep.DataBind()
        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            ObjReport = Nothing
        End Try
    End Sub

    Private Sub gvRep_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles gvRep.ItemDataBound
        Try
            If TypeOf e.Item Is GridDataItem Then
                Dim item As GridDataItem = TryCast(e.Item, GridDataItem)
                If item IsNot Nothing Then
                    item.Cells(5).Text = FormatNumber(CDbl(item.Cells(5).Text), hfDecimal.Value)
                End If
            End If
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub gvRep_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        BindGrid()
    End Sub

    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender


        Dim ObjCommon As SalesWorx.BO.Common.Common
        Try

            ObjCommon = New SalesWorx.BO.Common.Common()
            Dim dtCur As New DataTable
            dtCur = ObjCommon.GetCurrencyByOrg(Err_No, Err_Desc, hfOrgID.Value)
            If dtCur.Rows.Count > 0 Then
                lblDecimal.Text = "N" & dtCur.Rows(0)(1).ToString()
            End If
            For Each column As GridColumn In gvRep.MasterTableView.Columns
                If column.UniqueName = "SalesAmount" Then
                    Dim col As GridBoundColumn
                    col = (CType(column, GridBoundColumn))
                    column.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                    col.DataType = System.Type.GetType("System.Decimal")
                    col.DataFormatString = "{0:" & lblDecimal.Text & "}"
                End If
            Next
        Catch ex As Exception
            log.Error(ex.Message)
        End Try

    End Sub
    Private Sub dgv_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvRep.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindGrid()
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

            ObjCommon = New SalesWorx.BO.Common.Common()
            Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
            rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)


            Dim AssetID As New ReportParameter
            AssetID = New ReportParameter("AssetID", hfAssetID.Value)

            Dim OrgID As New ReportParameter
            OrgID = New ReportParameter("OrgID", hfOrgID.Value)

            Dim Fdate As Date
            Dim Tdate As Date

            Fdate = CDate(txtFromDate.SelectedDate)
            Tdate = CDate(txtToDate.SelectedDate)

            Dim FromDate As New ReportParameter
            FromDate = New ReportParameter("FromDate", Fdate.ToString("dd-MMM-yyyy"))

            Dim ToDate As New ReportParameter
            ToDate = New ReportParameter("ToDate", Tdate.ToString("dd-MMM-yyyy"))

            rview.ServerReport.SetParameters(New ReportParameter() {AssetID, OrgID, FromDate, ToDate})

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
                Response.AddHeader("Content-disposition", "attachment;filename=AssetHistory.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=AssetHistory.xls")
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

    Private Sub SearchBtn_Click(sender As Object, e As EventArgs) Handles SearchBtn.Click
        Try
            LoadDetails()
            BindGrid()
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
End Class