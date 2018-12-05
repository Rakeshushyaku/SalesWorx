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

Public Class RepIncentiveMonthDetails
    Inherits System.Web.UI.Page


    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "RepIncentiveMonthDetails"
    Dim objIncentive As New SalesWorx.BO.Common.Incentive
    Private Const PageID As String = "P355"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try


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

                    Session("Incentive_Year") = Request.QueryString("Incentive_Year")
                    Session("Incentive_Month") = Request.QueryString("Incentive_Month")
                    Session("Emp_Code") = Request.QueryString("Emp_Code")
                    Session("Org_ID") = Request.QueryString("Org_ID")
                    Session("Org_Name") = Request.QueryString("Org_Name")
                    Session("EmpName") = Request.QueryString("EmpName")
                   

                    Dim firstDayOfTheMonth As DateTime = New DateTime(Request.QueryString("Incentive_Year"), Request.QueryString("Incentive_Month"), 1)
                    Dim endDt As New Date(firstDayOfTheMonth.Year, firstDayOfTheMonth.Month, Date.DaysInMonth(firstDayOfTheMonth.Year, firstDayOfTheMonth.Month))
                    txtFromDate.SelectedDate = firstDayOfTheMonth
                    txtToDate.SelectedDate = endDt
                    ObjCommon = New SalesWorx.BO.Common.Common()
                    Dim ClientCode As String
                    ClientCode = (New SalesWorx.BO.Common.Common).GetAppConfig(Err_No, Err_Desc, "CLIENT_CODE")
                    Dim EndTime As String
                    If ClientCode = "GFC" Then

                        EndTime = ObjCommon.GetDayEndTime(Err_No, Err_Desc)
                    Else

                        EndTime = " 23:59:59"
                    End If

                    Dim ObjReport As New SalesWorx.BO.Common.Reports
                    Dim dt As New DataTable
                    dt = ObjReport.GetMonthDetails(Err_No, Err_Desc, 0, Request.QueryString("Org_ID"), Request.QueryString("Incentive_Month"), Request.QueryString("Incentive_Year"), Request.QueryString("Emp_Code"), CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy") & " " & EndTime, "0")
                    txtFromDate.MinDate = firstDayOfTheMonth
                    txtFromDate.MaxDate = endDt
                    txtToDate.MinDate = firstDayOfTheMonth
                    txtToDate.MaxDate = endDt

                    If dt.Rows.Count > 0 Then
                        Me.gvMonthDetails.DataSource = dt
                        Me.gvMonthDetails.DataBind()
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
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Private Sub SearchBtn_Click(sender As Object, e As EventArgs) Handles SearchBtn.Click
        Try

            If ValidateInputs() Then

                BindData()
            End If



        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
    Private Function GetMonthName(ByVal monthNum As Integer) As String
        Try
            Dim strDate As New DateTime(1, monthNum, 1)
            Return strDate.ToString("MMM")
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Function
  
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        Try

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
            If txtFromDate.SelectedDate.Value.Month <> Convert.ToInt32(Session("Incentive_Month")) Then
                MessageBoxValidation("Your selected month is different from Fromdate's month.", "Validation")
                Return bretval
            End If

            If txtToDate.SelectedDate.Value.Month <> Convert.ToInt32(Session("Incentive_Month")) Then
                MessageBoxValidation("Your selected month is different from Todate's month.", "Validation")
                Return bretval
            End If
            If txtFromDate.SelectedDate.Value.Year <> Convert.ToInt32(Session("Incentive_Year")) Then
                MessageBoxValidation("Your selected month is different from Fromdate's year.", "Validation")
                Return bretval
            End If
            If txtToDate.SelectedDate.Value.Year <> Convert.ToInt32(Session("Incentive_Year")) Then
                MessageBoxValidation("Your selected month is different from Todate's year.", "Validation")
                Return bretval
            End If

            bretval = True
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
        Return bretval

    End Function
    Private Sub BindData()
        Try
            Dim ClientCode As String
            ClientCode = (New SalesWorx.BO.Common.Common).GetAppConfig(Err_No, Err_Desc, "CLIENT_CODE")
            Dim EndTime As String
            If ClientCode = "GFC" Then
                EndTime = ObjCommon.GetDayEndTime(Err_No, Err_Desc)
            Else

                EndTime = " 23:59:59"
            End If
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetMonthDetails(Err_No, Err_Desc, 1, Session("Org_ID"), Session("Incentive_Month"), Session("Incentive_Year"), Session("Emp_Code"), CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy") & " " & EndTime, IIf(txtRefNo.Text.Trim() = "", "0", txtRefNo.Text))


            Me.gvMonthDetails.DataSource = dt
            Me.gvMonthDetails.DataBind()

        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub

    Sub MessageBoxValidation(ByVal str As String, Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Private Sub gvMonthDetails_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvMonthDetails.PageIndexChanged
        BindData()
    End Sub

    Private Sub gvMonthDetails_SortCommand(sender As Object, e As GridSortCommandEventArgs) Handles gvMonthDetails.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
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
    Private Sub BtnExportExcel_Click(sender As Object, e As EventArgs) Handles BtnExportExcel.Click
        Try
            If ValidateInputs() Then
                Export("Excel")
            End If



        Catch ex As Exception
            log.Debug(ex.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
    Sub Export(format As String)

        Try
            Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
            rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim Searchvalue As New ReportParameter

           



            Dim Org_ID As New ReportParameter
            Org_ID = New ReportParameter("Org_ID", Session("Org_ID").ToString())



            Dim Emp_Code As New ReportParameter
            Emp_Code = New ReportParameter("Emp_Code", Session("Emp_Code").ToString())


            Dim Incentive_Year As New ReportParameter
            Incentive_Year = New ReportParameter("Incentive_Year", Convert.ToInt32(Session("Incentive_Year")))

            Dim Incentive_Month As New ReportParameter
            Incentive_Month = New ReportParameter("Incentive_Month", Convert.ToInt32(Session("Incentive_Month")))

            Dim OrgName As New ReportParameter
            OrgName = New ReportParameter("OrgName", Session("Org_Name").ToString())


            Dim EmpName As New ReportParameter
            EmpName = New ReportParameter("EmpName", Session("EmpName").ToString())

            Dim FromDate As New ReportParameter
            FromDate = New ReportParameter("FromDate", CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"))


            Dim Opt As New ReportParameter
            Opt = New ReportParameter("Option", 1)


            Dim ClientCode As String
            ClientCode = (New SalesWorx.BO.Common.Common).GetAppConfig(Err_No, Err_Desc, "CLIENT_CODE")
            Dim EndTime As String
            If ClientCode = "GFC" Then
                EndTime = ObjCommon.GetDayEndTime(Err_No, Err_Desc)
            Else

                EndTime = " 23:59:59"
            End If
            Dim ToDate As New ReportParameter
            ToDate = New ReportParameter("ToDate", CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy") & " " & EndTime)



            Dim Ref_No_ As String
            Ref_No_ = IIf(txtRefNo.Text.Trim() = "", "0", txtRefNo.Text)
            Dim Ref_No As New ReportParameter
            Ref_No = New ReportParameter("Ref_No", Ref_No_)

            rview.ServerReport.SetParameters(New ReportParameter() {Opt, Org_ID, Emp_Code, Incentive_Year, Incentive_Month, FromDate, ToDate, OrgName, EmpName, Ref_No})

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
                Response.AddHeader("Content-disposition", "attachment;filename=IncentiveMonthDetails.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=IncentiveMonthDetails.xls")
                Response.AddHeader("Content-Length", bytes.Length)
            End If
            Response.OutputStream.Write(bytes, 0, bytes.Length)
            Response.OutputStream.Flush()
            Response.OutputStream.Close()
            Response.Flush()
            Response.Close()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub BtnExportPDF_Click(sender As Object, e As EventArgs) Handles BtnExportPDF.Click
        Try
            If ValidateInputs() Then
                Export("PDF")
            End If
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Debug(ex.ToString())
        End Try
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        Try


            Dim firstDayOfTheMonth As DateTime = New DateTime(Request.QueryString("Incentive_Year"), Request.QueryString("Incentive_Month"), 1)
            Dim endDt As New Date(firstDayOfTheMonth.Year, firstDayOfTheMonth.Month, Date.DaysInMonth(firstDayOfTheMonth.Year, firstDayOfTheMonth.Month))
            txtFromDate.SelectedDate = firstDayOfTheMonth
            txtToDate.SelectedDate = endDt
            ObjCommon = New SalesWorx.BO.Common.Common()
            Dim ClientCode As String
            ClientCode = (New SalesWorx.BO.Common.Common).GetAppConfig(Err_No, Err_Desc, "CLIENT_CODE")
            Dim EndTime As String
            If ClientCode = "GFC" Then

                EndTime = ObjCommon.GetDayEndTime(Err_No, Err_Desc)
            Else

                EndTime = " 23:59:59"
            End If

            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetMonthDetails(Err_No, Err_Desc, 0, Request.QueryString("Org_ID"), Request.QueryString("Incentive_Month"), Request.QueryString("Incentive_Year"), Request.QueryString("Emp_Code"), CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy") & " " & EndTime, "0")
           
            If dt.Rows.Count > 0 Then
                Me.gvMonthDetails.DataSource = dt
                Me.gvMonthDetails.DataBind()
            End If
            txtRefNo.Text = ""
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try


    End Sub

   
   
   
End Class