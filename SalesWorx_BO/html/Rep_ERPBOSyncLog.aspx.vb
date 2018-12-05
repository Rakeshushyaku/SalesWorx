Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports System.Collections.Generic
Imports Telerik.Web.UI
Partial Public Class Rep_ERPBOSyncLog
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "ERPBOSyncLog"

    Private Const PageID As String = "P380"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single

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
            Try
                ObjCommon = New SalesWorx.BO.Common.Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
              
                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                txtToDate.SelectedDate = Now()

                Me.ddl_ERPTable.DataSource = ObjCommon.GetERPSyncTable(Err_No, Err_Desc)
                Me.ddl_ERPTable.DataTextField = "Value"
                Me.ddl_ERPTable.DataValueField = "Code"
                Me.ddl_ERPTable.DataBind()
                Me.ddl_ERPTable.SelectedIndex = 0

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
            'Else
            '    Me.MapWindow.VisibleOnPageLoad = False
        End If
    End Sub
    Private Sub dgv_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvRep.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindData()
    End Sub
    Private Sub dgv_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged

        BindData()
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
    Private Sub BindData()

        Try

            ObjCommon = New SalesWorx.BO.Common.Common()
         
            Args.Visible = True



            Dim fromdate As Date
            Dim Todate As Date

            fromdate = CDate(txtFromDate.SelectedDate)
            Todate = CDate(txtToDate.SelectedDate)

            lbl_Fromdt.Text = fromdate.ToString("dd-MMM-yyyy")
            lbl_Todt.Text = Todate.ToString("dd-MMM-yyyy")
            Dim ERPTable As String = "ALL"

            If ddl_ERPTable.SelectedIndex > 0 Then
                ERPTable = ddl_ERPTable.SelectedItem.Value
            End If
            lbl_tbl.Text = ERPTable

            Dim objRep As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = objRep.GetERPSyncLog(Err_No, Err_Desc, CDate(txtFromDate.SelectedDate).ToString("MM-dd-yyyy"), CDate(txtToDate.SelectedDate).ToString("MM-dd-yyyy"), ERPTable)

            gvRep.DataSource = dt
            gvRep.DataBind()


        Catch ex As Exception
            If Err_Desc IsNot Nothing Then
                log.Error(Err_Desc)
            Else
                log.Error(GetExceptionInfo(ex))
            End If
            Err_No = "74067"
            '  Err_Desc = ex.Message
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
        Finally

        End Try
    End Sub
    Private Sub BtnExport_Click(sender As Object, e As EventArgs) Handles BtnExportExcel.Click
        Try
            If ValidateInput() Then
                Export("Excel")
            End If
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub
    Private Sub BtnPDF_Click(sender As Object, e As EventArgs) Handles BtnExportPDF.Click
        Try
            If ValidateInput() Then
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
         

        Dim fromdate As Date
        Dim Todate As Date

        fromdate = CDate(txtFromDate.SelectedDate)
        Todate = CDate(txtToDate.SelectedDate)

        Dim SERPTable As String = "ALL"

        If ddl_ERPTable.SelectedIndex > 0 Then
            SERPTable = ddl_ERPTable.SelectedItem.Value
        End If

        Dim FDate As New ReportParameter
        FDate = New ReportParameter("FromDate", fromdate.ToString("MM-dd-yyyy"))

        Dim TDate As New ReportParameter
        TDate = New ReportParameter("ToDate", Todate.ToString("MM-dd-yyyy"))


        Dim ERPTable As New ReportParameter
        ERPTable = New ReportParameter("ERPTable", SERPTable)

        rview.ServerReport.SetParameters(New ReportParameter() {FDate, TDate, ERPTable})

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
            Response.AddHeader("Content-disposition", "attachment;filename=ERPSyncLog.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=ERPSyncLog.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()

    End Sub
    'Private Sub InitReportViewer(ByVal fromdate As Date, ByVal Todate As Date)
    '    Try





    '        Dim FDate As New ReportParameter
    '        FDate = New ReportParameter("FromDate", fromdate.ToString())

    '        Dim TDate As New ReportParameter
    '        TDate = New ReportParameter("ToDate", Todate.ToString())


    '        Dim ERPTable As New ReportParameter
    '        ERPTable = New ReportParameter("ERPTable", CStr(IIf(Me.ddl_ERPTable.SelectedIndex <= 0, "ALL", Me.ddl_ERPTable.SelectedValue)))



    '        With RVMain
    '            .Reset()
    '            .ShowParameterPrompts = False
    '            .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
    '            .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
    '            .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
    '            .ServerReport.SetParameters(New ReportParameter() {FDate, TDate, ERPTable})
    '            .ServerReport.Refresh()

    '        End With


    '    Catch Ex As Exception
    '        '  log.Error(GetExceptionInfo(Ex))
    '    End Try
    'End Sub
    'Protected Sub RVMain_BookmarkNavigation(ByVal sender As Object, ByVal e As Microsoft.Reporting.WebForms.BookmarkNavigationEventArgs) Handles RVMain.BookmarkNavigation
    '    Me.lblPopmsg.Text = ""
    '    Me.lblPopmsg.Text = e.BookmarkId.ToString()

    '    Me.MapWindow.VisibleOnPageLoad = True



    'End Sub
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInput() Then
            rpbFilter.Items(0).Expanded = False
 
            BindData()
        Else


            Args.Visible = False
        End If
       

    End Sub
    Function ValidateInput() As Boolean
        Dim bretval As Boolean = False

        Dim TemFromDateStr As String = CDate(txtFromDate.SelectedDate).ToString("dd/MM/yyyy")
        Dim DateArr As Array = TemFromDateStr.Split("/")
        If DateArr.Length = 3 Then
            TemFromDateStr = DateArr(1) & "/" & DateArr(0) & "/" & DateArr(2)
        End If
        Dim TemToDateStr As String = CDate(txtToDate.SelectedDate).ToString("dd/MM/yyyy")
        Dim DateArr1 As Array = TemToDateStr.Split("/")
        If DateArr1.Length = 3 Then
            TemToDateStr = DateArr1(1) & "/" & DateArr1(0) & "/" & DateArr1(2)
        End If

        If Not IsDate(TemFromDateStr) Then
            MessageBoxValidation("Enter valid ""From date"".", "Validation")
            SetFocus(txtFromDate)
            Return bretval
        End If

        If Not IsDate(TemToDateStr) Then
            MessageBoxValidation("Enter valid ""To date"".", "Validation")
            SetFocus(TemToDateStr)
            Return bretval
        End If

        If CDate(TemFromDateStr) > CDate(TemToDateStr) Then
            MessageBoxValidation("Start Date should not be greater than End Date.", "Validation")
            Return bretval
        End If
        bretval = True
        Return bretval
        
    End Function
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
  
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub

End Class