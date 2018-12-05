
Imports Telerik.Web.UI
Imports SalesWorx.BO.Common
Imports Telerik.Charting
Imports System.IO
Imports System.Xml
Imports System.Drawing.Imaging
Imports System.Drawing
Imports log4net
Imports Microsoft.Reporting.WebForms
Imports System.Configuration.ConfigurationManager
Imports OfficeOpenXml

Partial Public Class RepSMScoreCard
    Inherits System.Web.UI.Page
    Private Const ModuleName As String = "RepSMScoreCard.aspx"
    Private Const PageID As String = "P343"
    Dim Err_No As Long
    Dim objRep As New Reports
    Dim ObjCommon As New SalesWorx.BO.Common.Common
    Dim Err_Desc As String
    Private ReportPath As String = "SMScoreCard"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Button_Click(sender As Object, e As ImageClickEventArgs)
        gvLog.MasterTableView.FilterExpression = String.Empty
        gvLog.MasterTableView.Columns(0).CurrentFilterValue = ""
        gvLog.MasterTableView.Rebind()
    End Sub
    Protected Sub Button1_Click(sender As Object, e As ImageClickEventArgs)
        gvCustomers.MasterTableView.FilterExpression = String.Empty
        gvCustomers.MasterTableView.Columns(0).CurrentFilterValue = ""
        gvCustomers.MasterTableView.Rebind()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Session.Item("USER_ACCESS") Is Nothing Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If

            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)




            If Not HasPermission Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If

            Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
            ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
            ddlOrganization.DataBind()
            ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))
            If ddlOrganization.Items.Count = 2 Then
                ddlOrganization.SelectedIndex = 1
                BindMGR()
            Else
                Me.ddlOrganization.SelectedIndex = 0
            End If
            Me.hfRow.Value = IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue)




            Me.StartTime.SelectedDate = Now.Date

            Dim Sdate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM") + "-01-" + DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("yyyy")

            Me.hfSMonth.Value = Sdate
            Me.hfUserID.Value = CType(Session.Item("USER_ACCESS"), UserAccess).UserID
            AgencyTab.Tabs(0).Selected = True
            RadMultiPage21.SelectedIndex = 0
            GrowthTab.Tabs(0).Selected = True
            RadMultiPage1.SelectedIndex = 0

            Me.lblTopCustMonth.Text = Me.StartTime.SelectedDate.Value.ToString("MMM-yyyy")
            Me.lblTransMonth.Text = Me.StartTime.SelectedDate.Value.ToString("MMM-yyyy")
            Me.lblTeamMonth.Text = Me.StartTime.SelectedDate.Value.ToString("MMM-yyyy")

            CType(gvLog.Columns(1), GridBoundColumn).DataFormatString = "{0:N0}"
            CType(gvLog.Columns(2), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
            CType(gvLog.Columns(3), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
            CType(gvLog.Columns(4), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
            CType(gvLog.Columns(5), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"

            CType(gvLog.Columns(1), GridBoundColumn).FooterAggregateFormatString = "{0:N0}"
            CType(gvLog.Columns(2), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
            CType(gvLog.Columns(3), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
            CType(gvLog.Columns(4), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
            CType(gvLog.Columns(5), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"

            CType(gvCustomers.Columns(1), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"




            gvLog.Columns(2).HeaderText = "Gross  Sales (" & Me.lblC.Text & ")"
            gvLog.Columns(3).HeaderText = "Total Returns (" & Me.lblC.Text & ")"
            gvLog.Columns(5).HeaderText = "Total Collections (" & Me.lblC.Text & ")"
            gvLog.Columns(4).HeaderText = "Net Sales (" & Me.lblC.Text & ")"
            gvCustomers.Columns(1).HeaderText = "Sales (" & Me.lblC.Text & ")"
            ' CType(gvCustomers.Columns(1), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
        End If

    End Sub
    Protected Sub btnShowAll_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        gvLog.MasterTableView.FilterExpression = String.Empty

        For Each column As GridColumn In gvLog.MasterTableView.RenderColumns
            If TypeOf column Is GridBoundColumn Then
                Dim boundColumn As GridBoundColumn = TryCast(column, GridBoundColumn)
                boundColumn.CurrentFilterValue = String.Empty
            End If
        Next
      
        gvLog.MasterTableView.Rebind()

    End Sub
    Private Sub MainContent_BtnExportLog_Click(sender As Object, e As EventArgs) Handles BtnExportLog.Click
        Dim dt As New DataTable
        dt = objRep.SMVanLog(Err_No, Err_Desc, IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue), IIf(Me.ddlMgr.SelectedIndex <= 0, "0", Me.ddlMgr.SelectedValue), Me.hfSMonth.Value)
        Dim dtFinal As New DataTable
        dtFinal = dt.DefaultView.ToTable(False, "SalesRep_Name", "TotCalls", "TSales", "TCreditNote", "TNet", "Payment", "RouteAdh")
        If dtFinal.Rows.Count > 0 Then
            Using package As New ExcelPackage()
                ' add a new worksheet to the empty workbook
                Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
                Worksheet.Cells("A1").LoadFromDataTable(dtFinal, True)
                Worksheet.Cells.AutoFitColumns()
                Worksheet.Cells("A1").Value = "Van"
                Worksheet.Cells("B1").Value = "Total Calls"
                Worksheet.Cells("C1").Value = "Gross Sales"
                Worksheet.Cells("D1").Value = "Total Returns"
                Worksheet.Cells("E1").Value = "Net Sales"
                Worksheet.Cells("F1").Value = "Total Collection"
                Worksheet.Cells("G1").Value = "JP Adherence %"


                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename= VanTransactionSummary.xlsx")

                Using MyMemoryStream As New MemoryStream()
                    package.SaveAs(MyMemoryStream)
                    MyMemoryStream.WriteTo(Response.OutputStream)
                    Response.AddHeader("Content-Length", MyMemoryStream.Length)
                    Response.Flush()
                    Response.Close()
                End Using
            End Using
        End If
        dt = Nothing
    End Sub
    Private Sub BtnExportBiffExcel_Click(sender As Object, e As EventArgs) Handles BtnExportBiffExcel.Click
       Dim dtsalesGrowth As New DataTable
        dtsalesGrowth = objRep.SMLast3MonthsSalesgrowth(Err_No, Err_Desc, IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue), IIf(Me.ddlMgr.SelectedIndex <= 0, "0", Me.ddlMgr.SelectedValue), Me.hfSMonth.Value)
        Dim dtFinal As New DataTable
        dtFinal.Columns.Add("Description", System.Type.GetType("System.String"))
        Dim DtM As New DataTable
        DtM = dtsalesGrowth.DefaultView.ToTable(True, "MonthYear")
        For Each sdr As DataRow In DtM.Rows
            dtFinal.Columns.Add(sdr("MonthYear").ToString, System.Type.GetType("System.Decimal"))
        Next
        For i As Integer = 1 To 4
            Dim seldR() As DataRow
            seldR = dtsalesGrowth.Select("DispOrder=" & i)
            If seldR.Length > 0 Then
                Dim dr As DataRow
                dr = dtFinal.NewRow
                dr("Description") = seldR(0)("Description")
                For Each sdr As DataRow In DtM.Rows
                    If seldR.CopyToDataTable.Select("MonthYear='" & sdr("MonthYear") & "'").Length > 0 Then
                        dr(sdr("MonthYear")) = seldR.CopyToDataTable.Select("MonthYear='" & sdr("MonthYear") & "'")(0)("TotValue")
                    Else
                        dr(sdr("MonthYear")) = 0
                    End If
                Next
                dtFinal.Rows.Add(dr)
            End If
        Next
        dtsalesGrowth = Nothing
        If dtFinal.Rows.Count > 0 Then

            Using package As New ExcelPackage()
                ' add a new worksheet to the empty workbook
                Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
                Worksheet.Cells("A1").LoadFromDataTable(dtFinal, True)
                Worksheet.Cells.AutoFitColumns()

                 
                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename= SalesGrowth3months.xlsx")

                Using MyMemoryStream As New MemoryStream()
                    package.SaveAs(MyMemoryStream)
                    MyMemoryStream.WriteTo(Response.OutputStream)
                    Response.AddHeader("Content-Length", MyMemoryStream.Length)
                    Response.Flush()
                    Response.Close()
                End Using
            End Using
        End If
    End Sub
    Private Sub BtnExportTop10_Click(sender As Object, e As EventArgs) Handles BtnExportTop10.Click
        Dim dt As New DataTable
        dt = objRep.SMTop10Customers(Err_No, Err_Desc, IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue), IIf(Me.ddlMgr.SelectedIndex <= 0, "0", Me.ddlMgr.SelectedValue), Me.hfSMonth.Value)
        
        If dt.Rows.Count > 0 Then
            Using package As New ExcelPackage()
                ' add a new worksheet to the empty workbook
                Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
                Worksheet.Cells("A1").LoadFromDataTable(dt, True)
                Worksheet.Cells.AutoFitColumns()
                Worksheet.Cells("A1").Value = "Customer"
                Worksheet.Cells("B1").Value = "Net Sales"
                


                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename= SalesbyCustomers.xlsx")

                Using MyMemoryStream As New MemoryStream()
                    package.SaveAs(MyMemoryStream)
                    MyMemoryStream.WriteTo(Response.OutputStream)
                    Response.AddHeader("Content-Length", MyMemoryStream.Length)
                    Response.Flush()
                    Response.Close()
                End Using
            End Using
        End If
        dt = Nothing
    End Sub
    Sub BindStatistics()
        Dim success As Boolean = False
        Dim Sdate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM") + "-01-" + DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("yyyy")

        Me.hfSMonth.Value = Sdate
        Try
            Dim dt As New DataTable




            dt = objRep.SMTeamPerformance(Err_No, Err_Desc, IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue), IIf(Me.ddlMgr.SelectedIndex <= 0, "0", Me.ddlMgr.SelectedValue), Me.hfSMonth.Value)
            Me.lblSales.Text = "0"
            Me.lblSalesAvg.Text = "0"
            Me.lblRetAvg.Text = "0"
            Me.lblReturn.Text = "0"
            Me.lblColAvg.Text = "0"
            Me.lblCollection.Text = "0"
            Me.lblColCurr.Text = ""
            Me.lblOrdCurr.Text = ""
            Me.lblRetCurr.Text = ""
            Me.lblTeamSize.Text = "0"
            Me.lblTotalCalls.Text = "0"
            Me.lblAvgCalls.Text = "0"
            Me.lblTCount.Text = ""
            Me.lblVCnt.Text = ""


           

            If dt.Rows.Count > 0 Then

                Me.hfDecimal.Value = dt.Rows(0)("DecimalDigits").ToString()
                Dim LabelDecimalDigits As String = "0.00"
                If Me.hfDecimal.Value = 0 Then
                    LabelDecimalDigits = "0"
                ElseIf Me.hfDecimal.Value = 1 Then
                    LabelDecimalDigits = "0.0"
                ElseIf Me.hfDecimal.Value = 2 Then
                    LabelDecimalDigits = "0.00"
                ElseIf Me.hfDecimal.Value = 3 Then
                    LabelDecimalDigits = "0.000"
                ElseIf Me.hfDecimal.Value >= 4 Then
                    LabelDecimalDigits = "0.0000"
                End If
                For Each r As DataRow In dt.Rows
                    If r("Description").ToString() = "Total Calls" Then
                        Me.lblTotalCalls.Text = CDec(r("TotValue").ToString()).ToString("#,##0")

                    ElseIf r("Description").ToString() = "Team Size" Then
                        Me.lblTeamSize.Text = CDec(r("TotValue").ToString()).ToString("#,##0")


                    ElseIf r("Description").ToString() = "Sales" Then
                        Me.lblSales.Text = CDec(r("TotValue").ToString()).ToString("#,##" & LabelDecimalDigits)
                        Me.lblOrdCurr.Text = r("Currency").ToString()
                        Me.lblTCount.Text = r("Currency").ToString()
                        Me.lblVCnt.Text = r("Currency").ToString()
                    ElseIf r("Description").ToString() = "Returns" Then
                        Me.lblReturn.Text = CDec(r("TotValue").ToString()).ToString("#,##" & LabelDecimalDigits)
                        Me.lblRetCurr.Text = r("Currency").ToString()

                    ElseIf r("Description").ToString() = "Collections" Then
                        Me.lblCollection.Text = CDec(r("TotValue").ToString()).ToString("#,##" & LabelDecimalDigits)
                        Me.lblColCurr.Text = r("Currency").ToString()
                    ElseIf r("Description").ToString() = "Net Sales" Then
                        lblNet.Text = CDec(r("TotValue").ToString()).ToString("#,##" & LabelDecimalDigits)
                        Me.lblNetCurr.Text = r("Currency").ToString()

                    End If

                Next
                Me.lblAvgCalls.Text = CStr(CInt(Math.Round((CDec(IIf(Me.lblTotalCalls.Text = "", "0", Me.lblTotalCalls.Text)) / CDec(IIf(Me.lblTeamSize.Text = "" Or Me.lblTeamSize.Text = "0", "1", Me.lblTeamSize.Text))))))
                Me.lblSalesAvg.Text = CDec((CDec(IIf(Me.lblSales.Text = "", "0", Me.lblSales.Text)) / CDec(IIf(Me.lblTeamSize.Text = "" Or Me.lblTeamSize.Text = "0", "1", Me.lblTeamSize.Text)))).ToString("#,##" & LabelDecimalDigits)
                Me.lblColAvg.Text = CDec((CDec(IIf(Me.lblCollection.Text = "", "0", Me.lblCollection.Text)) / CDec(IIf(Me.lblTeamSize.Text = "" Or Me.lblTeamSize.Text = "0", "1", Me.lblTeamSize.Text)))).ToString("#,##" & LabelDecimalDigits)
                Me.lblRetAvg.Text = CDec((CDec(IIf(Me.lblReturn.Text = "", "0", Me.lblReturn.Text)) / CDec(IIf(Me.lblTeamSize.Text = "" Or Me.lblTeamSize.Text = "0", "1", Me.lblTeamSize.Text)))).ToString("#,##" & LabelDecimalDigits)
                Me.lbl_NetAvg.Text = CDec((CDec(IIf(Me.lblNet.Text = "", "0", Me.lblNet.Text)) / CDec(IIf(Me.lblTeamSize.Text = "" Or Me.lblTeamSize.Text = "0", "1", Me.lblTeamSize.Text)))).ToString("#,##" & LabelDecimalDigits)

            Else
                Me.lblSales.Text = "0"
                Me.lblSalesAvg.Text = "0"
                Me.lblRetAvg.Text = "0"
                Me.lblReturn.Text = "0"
                Me.lblColAvg.Text = "0"
                Me.lblCollection.Text = "0"
                Me.lblColCurr.Text = ""
                Me.lblOrdCurr.Text = ""
                Me.lblRetCurr.Text = ""
                Me.lblTeamSize.Text = "0"
                Me.lblTotalCalls.Text = "0"
                Me.lblAvgCalls.Text = "0"
                Me.lblTCount.Text = ""
                Me.lblVCnt.Text = ""
                Me.lbl_NetAvg.Text = "0"
                lblNet.Text = ""
            End If

            Dim dtsalesGrowth As New DataTable
            dtsalesGrowth = objRep.SMLast3MonthsSalesgrowth(Err_No, Err_Desc, IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue), IIf(Me.ddlMgr.SelectedIndex <= 0, "0", Me.ddlMgr.SelectedValue), Me.hfSMonth.Value)
            gvSalesGrowth.DataSource = dtsalesGrowth
            gvSalesGrowth.DataBind()
            If dtsalesGrowth.Rows.Count > 0 Then
                img_export.Visible = True
            Else
                img_export.Visible = False
            End If

            dtsalesGrowth = Nothing

            Dim dtTargetVsAchiev As New DataTable
            dtTargetVsAchiev = objRep.SMLast3MonthsTargetVsAchiev(Err_No, Err_Desc, IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue), IIf(Me.ddlMgr.SelectedIndex <= 0, "0", Me.ddlMgr.SelectedValue), Me.hfSMonth.Value)
            gvTargetVsAchiev.DataSource = dtTargetVsAchiev
            gvTargetVsAchiev.DataBind()


            If dtTargetVsAchiev.Rows.Count > 0 Then
                img_export_TargetVsAchiev.Visible = True
            Else
                img_export_TargetVsAchiev.Visible = False
            End If
            dtTargetVsAchiev = Nothing

        Catch ex As Exception
            Err_No = "34926"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub
    Private Sub BindMGR()

        Dim dt As New DataTable

        dt = objRep.GetSalesManagerByOrg(Err_No, Err_Desc, IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue))
        Me.ddlMgr.ClearSelection()
        Me.ddlMgr.Items.Clear()
        Me.ddlMgr.Text = ""
        Me.ddlMgr.DataSource = dt
        Me.ddlMgr.DataTextField = "SalesManagerName"
        Me.ddlMgr.DataValueField = "SalesManagerID"
        Me.ddlMgr.DataBind()
        If Me.ddlMgr.Items.Count = 2 Then
            Me.ddlMgr.SelectedIndex = 1
            Me.lblMGR.Text = Me.ddlMgr.SelectedItem.Text
        Else
            Me.ddlMgr.SelectedIndex = 0
        End If
        Me.hfSE.Value = Me.ddlMgr.SelectedValue
    End Sub






    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub



    Protected Sub ddlOrganization_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlOrganization.SelectedIndexChanged
        BindMGR()

        Me.hfRow.Value = IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue)
        Me.lblSales.Text = "0"
        Me.lblSalesAvg.Text = "0"
        Me.lblRetAvg.Text = "0"
        Me.lblReturn.Text = "0"
        Me.lblColAvg.Text = "0"
        Me.lblCollection.Text = "0"
        Me.lblColCurr.Text = ""
        Me.lblOrdCurr.Text = ""
        Me.lblRetCurr.Text = ""
        Me.lblTeamSize.Text = "0"
        Me.lblTotalCalls.Text = "0"
        Me.lblAvgCalls.Text = "0"
        Me.lblVCnt.Text = ""
        Me.lblTCount.Text = ""

        'BindStatistics()
        'CType(gvLog.Columns(1), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
        'CType(gvLog.Columns(2), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
        'CType(gvLog.Columns(3), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"

        'Me.lblMGR.Text = Me.ddlMgr.SelectedItem.Text
        'Me.lblC.Text = Me.lblOrdCurr.Text
        'CType(gvLog.Columns(1), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
        'CType(gvLog.Columns(2), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
        'CType(gvLog.Columns(3), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"

        'Me.gvLog.Rebind()
        'Me.lbl_org.Text = Me.ddlOrganization.SelectedItem.Text
        'Me.lbl_van.Text = Me.ddlMgr.SelectedItem.Text
        'Me.lbl_from.Text = Me.StartTime.SelectedDate.Value.ToString("MMM-yyyy")
        'Args.Visible = True
        'rpt.Visible = True
        'ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
    End Sub

    Protected Sub SearchBtn_Click(sender As Object, e As EventArgs) Handles SearchBtn.Click
        If Me.ddlOrganization.SelectedIndex <= 0 Then
            MessageBoxValidation("Please select a organisation", "Validation")
            rpt.Visible = False
            Args.Visible = False

            Exit Sub

        End If
        If Me.ddlMgr.SelectedIndex <= 0 Then
            MessageBoxValidation("Please select a sales manager", "Validation")
            rpt.Visible = False
            Args.Visible = False
            Exit Sub
        End If
        Me.hfSE.Value = IIf(Me.ddlMgr.SelectedIndex <= 0, "0", Me.ddlMgr.SelectedValue)

        rpbFilter.Items(0).Expanded = False
        Me.lblSales.Text = "0"
        Me.lblSalesAvg.Text = "0"
        Me.lblRetAvg.Text = "0"
        Me.lblReturn.Text = "0"
        Me.lblColAvg.Text = "0"
        Me.lblCollection.Text = "0"
        Me.lblColCurr.Text = ""
        Me.lblOrdCurr.Text = ""
        Me.lblRetCurr.Text = ""
        Me.lblTeamSize.Text = "0"
        Me.lblTotalCalls.Text = "0"
        Me.lblAvgCalls.Text = "0"

        AgencyTab.Tabs(0).Selected = True
        RadMultiPage21.SelectedIndex = 0
        GrowthTab.Tabs(0).Selected = True
        RadMultiPage1.SelectedIndex = 0
        BindStatistics()
        Me.lblTopCustMonth.Text = Me.StartTime.SelectedDate.Value.ToString("MMM-yyyy")
        Me.lblTransMonth.Text = Me.StartTime.SelectedDate.Value.ToString("MMM-yyyy")
        Me.lblTeamMonth.Text = Me.StartTime.SelectedDate.Value.ToString("MMM-yyyy")
        CType(gvLog.Columns(2), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
        CType(gvLog.Columns(3), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
        CType(gvLog.Columns(4), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"

        Me.lblMGR.Text = Me.ddlMgr.SelectedItem.Text
        Me.lblC.Text = Me.lblOrdCurr.Text
        CType(gvLog.Columns(2), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
        CType(gvLog.Columns(3), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
        CType(gvLog.Columns(4), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"

        gvLog.Columns(2).HeaderText = "Gross Sales (" & Me.lblC.Text & ")"
        gvLog.Columns(3).HeaderText = "Total Returns (" & Me.lblC.Text & ")"
        gvLog.Columns(4).HeaderText = "Net Sales (" & Me.lblC.Text & ")"
        gvLog.Columns(5).HeaderText = "Total Collections (" & Me.lblC.Text & ")"
        If CType(Session.Item("CONTROL_PARAMS"), ControlParams).USE_DISTR_IN_CALLS = "Y" Then
            gvLog.Columns(1).HeaderTooltip = "Total visits in which Distribution check was performed"
        Else
            gvLog.Columns(1).HeaderTooltip = "Total visits done"
        End If
        gvCustomers.Columns(1).HeaderText = "Sales (" & Me.lblC.Text & ")"

        Me.gvLog.Rebind()
        Me.lbl_org.Text = Me.ddlOrganization.SelectedItem.Text
        Me.lbl_van.Text = Me.ddlMgr.SelectedItem.Text
        Me.lbl_from.Text = Me.StartTime.SelectedDate.Value.ToString("MMM-yyyy")

        Args.Visible = True
        rpt.Visible = True

        'Dim dtTargetVsAchiev As New DataTable
        'dtTargetVsAchiev = objRep.SMLast3MonthsTargetVsAchiev(Err_No, Err_Desc, IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue), IIf(Me.ddlMgr.SelectedIndex <= 0, "0", Me.ddlMgr.SelectedValue), Me.hfSMonth.Value)




        'Dim dt_TA As New DataTable
        'dt_TA.Columns.Add("MonYear", GetType(String))
        'dt_TA.Columns.Add("Description", GetType(String))
        'dt_TA.Columns.Add("TargentValue", GetType(String))
        'dt_TA.Columns.Add("AchValue", GetType(String))
        'dt_TA.Columns.Add("TotalCalls", GetType(String))
        'dt_TA.Columns.Add("ProductiveCalls", GetType(String))
        'dt_TA.Columns.Add("MonthYear", GetType(DateTime))
        'dt_TA.Columns.Add("Type", GetType(String))



        'For Each row As DataRow In dtTargetVsAchiev.Rows

        '    Dim result() As DataRow = dt_TA.Select("MonYear = '" & CStr(row("MonthYear")) & "'")
        '    If result.Count = 0 Then
        '        Dim rw As DataRow = dt_TA.NewRow
        '        rw("MonYear") = CStr(row("MonthYear"))
        '        rw("MonthYear") = CDate(row("MonthY"))
        '        If CStr(row("Description")).Contains("Target") And Val(CStr(row("DispOrder"))) = 1 Then
        '            rw("TargentValue") = CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))
        '        End If
        '        If CStr(row("Description")).Contains("Achievement") And Val(CStr(row("DispOrder"))) = 2 Then
        '            rw("AchValue") = CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))
        '        End If
        '        If CStr(row("Description")).Contains("Total") And Val(CStr(row("DispOrder"))) = 3 Then
        '            rw("TotalCalls") = CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))
        '        End If
        '        If CStr(row("Description")).Contains("Productive") And Val(CStr(row("DispOrder"))) = 4 Then
        '            rw("ProductiveCalls") = CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))
        '        End If
        '        dt_TA.Rows.Add(rw)
        '    Else
        '        If CStr(row("Description")).Contains("Target") And Val(CStr(row("DispOrder"))) = 1 Then
        '            result(0)("TargentValue") = CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))
        '        End If
        '        If CStr(row("Description")).Contains("Achievement") And Val(CStr(row("DispOrder"))) = 2 Then
        '            result(0)("AchValue") = CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))
        '        End If

        '        If CStr(row("Description")).Contains("Total") And Val(CStr(row("DispOrder"))) = 3 Then
        '            result(0)("TotalCalls") = CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))
        '        End If
        '        If CStr(row("Description")).Contains("Productive") And Val(CStr(row("DispOrder"))) = 4 Then
        '            result(0)("ProductiveCalls") = CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))
        '        End If


        '    End If
        'Next



        'Dim query = From row In dt_TA.Copy()




        ' '' Forming Summary Chart
        'summaryChart.DataSource = query
        'summaryChart.DataBind()




        Dim ObjReport As New SalesWorx.BO.Common.Reports
        

        Dim dttop10 As New DataTable
        dttop10 = ObjReport.SMTop10CustomersTop10(Err_No, Err_Desc, IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue), IIf(Me.ddlMgr.SelectedIndex <= 0, "0", Me.ddlMgr.SelectedValue), Me.hfSMonth.Value)

        Dim dtsalesGrowth As New DataTable
        dtsalesGrowth = objRep.SMLast3MonthsSalesgrowth(Err_No, Err_Desc, IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue), IIf(Me.ddlMgr.SelectedIndex <= 0, "0", Me.ddlMgr.SelectedValue), Me.hfSMonth.Value)




        Dim dtsalesGrowthFinal As New DataTable
        dtsalesGrowthFinal.Columns.Add("MonthY")
        dtsalesGrowthFinal.Columns.Add("SalesQty", System.Type.GetType("System.Decimal"))
        dtsalesGrowthFinal.Columns.Add("GrossSales", System.Type.GetType("System.Decimal"))
        dtsalesGrowthFinal.Columns.Add("ReturnValue", System.Type.GetType("System.Decimal"))
        dtsalesGrowthFinal.Columns.Add("NetSales", System.Type.GetType("System.Decimal"))

        Dim DtMonths As New DataTable
        DtMonths = dtsalesGrowth.DefaultView.ToTable(True, "MonthY")

        For Each dr In DtMonths.Rows
            Dim newdr As DataRow
            newdr = dtsalesGrowthFinal.NewRow
            Dim Seldr() As DataRow
            Seldr = dtsalesGrowth.Select("MonthY='" & dr("MonthY").ToString & "' and Description=' Sales Quantity'")
            newdr("MonthY") = CDate(dr("MonthY").ToString).ToString("MMM-yyyy")
            If Seldr.Length > 0 Then
                newdr("SalesQty") = Val(Seldr(0)("TotValue").ToString)
            Else
                newdr("SalesQty") = 0
            End If

            Dim SeldrGross() As DataRow
            SeldrGross = dtsalesGrowth.Select("MonthY='" & dr("MonthY").ToString & "' and Description='Gross Sales Value'")

            If SeldrGross.Length > 0 Then
                newdr("GrossSales") = Val(SeldrGross(0)("TotValue").ToString)
            Else
                newdr("GrossSales") = 0
            End If


            Dim SeldrReturn() As DataRow
            SeldrReturn = dtsalesGrowth.Select("MonthY='" & dr("MonthY").ToString & "' and Description='Returns Value'")

            If SeldrReturn.Length > 0 Then
                newdr("ReturnValue") = Val(SeldrReturn(0)("TotValue").ToString)
            Else
                newdr("ReturnValue") = 0
            End If

            Dim SeldrNet() As DataRow
            SeldrNet = dtsalesGrowth.Select("MonthY='" & dr("MonthY").ToString & "' and Description='Net Sales'")

            If SeldrNet.Length > 0 Then
                newdr("NetSales") = Val(SeldrNet(0)("TotValue").ToString)
            Else
                newdr("NetSales") = 0
            End If
            dtsalesGrowthFinal.Rows.Add(newdr)
        Next

        Dim sourceTbl1 As DataTable = dtsalesGrowthFinal.Copy()
        Dim query1 = From row In sourceTbl1.Copy()
                Group row By MonthY = row.Field(Of String)("MonthY") Into VanGroup = Group
                Select New With {
                    Key MonthY,
                    .SalesQty = VanGroup.Sum(Function(r) r.Field(Of Decimal)("SalesQty")),
                    .GrossSales = VanGroup.Sum(Function(r) r.Field(Of Decimal)("GrossSales")),
                    .NetSales = VanGroup.Sum(Function(r) r.Field(Of Decimal)("NetSales")),
                    .ReturnValue = VanGroup.Sum(Function(r) r.Field(Of Decimal)("ReturnValue"))
               }

        RadSalesGrowth.DataSource = query1
        RadSalesGrowth.DataBind()

        radTop10.DataSource = dttop10
        radTop10.DataBind()


        Dim dtTargetVsAchiev As New DataTable
        dtTargetVsAchiev = objRep.SMLast3MonthsTargetVsAchiev(Err_No, Err_Desc, IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue), IIf(Me.ddlMgr.SelectedIndex <= 0, "0", Me.ddlMgr.SelectedValue), Me.hfSMonth.Value)

        Dim dtTargetFinal As New DataTable
        dtTargetFinal.Columns.Add("MonthY")
        dtTargetFinal.Columns.Add("TargetValue", System.Type.GetType("System.Decimal"))
        dtTargetFinal.Columns.Add("AchievementValue", System.Type.GetType("System.Decimal"))
        dtTargetFinal.Columns.Add("TotalCalls", System.Type.GetType("System.Int32"))
        dtTargetFinal.Columns.Add("ProductiveCalls", System.Type.GetType("System.Int32"))

        Dim DtMonths1 As New DataTable
        DtMonths1 = dtTargetVsAchiev.DefaultView.ToTable(True, "MonthY")

        For Each dr In DtMonths1.Rows
            Dim newdr As DataRow
            newdr = dtTargetFinal.NewRow
            Dim Seldr() As DataRow
            Seldr = dtTargetVsAchiev.Select("MonthY='" & dr("MonthY").ToString & "' and Description='Target Value'")
            newdr("MonthY") = CDate(dr("MonthY").ToString).ToString("MMM-yyyy")
            If Seldr.Length > 0 Then
                newdr("TargetValue") = Val(Seldr(0)("TotValue").ToString)
            Else
                newdr("TargetValue") = 0
            End If

            Dim SeldrAchievementValue() As DataRow
            SeldrAchievementValue = dtTargetVsAchiev.Select("MonthY='" & dr("MonthY").ToString & "' and Description='Achievement  Value'")

            If SeldrAchievementValue.Length > 0 Then
                newdr("AchievementValue") = Val(SeldrAchievementValue(0)("TotValue").ToString)
            Else
                newdr("AchievementValue") = 0
            End If


            Dim SeldrTotalCalls() As DataRow
            SeldrTotalCalls = dtTargetVsAchiev.Select("MonthY='" & dr("MonthY").ToString & "' and Description='Total Calls'")

            If SeldrTotalCalls.Length > 0 Then
                newdr("TotalCalls") = Val(SeldrTotalCalls(0)("TotValue").ToString)
            Else
                newdr("TotalCalls") = 0
            End If

            Dim SeldrProductiveCalls() As DataRow
            SeldrProductiveCalls = dtTargetVsAchiev.Select("MonthY='" & dr("MonthY").ToString & "' and Description='Productive Calls'")

            If SeldrProductiveCalls.Length > 0 Then
                newdr("ProductiveCalls") = Val(SeldrProductiveCalls(0)("TotValue").ToString)
            Else
                newdr("ProductiveCalls") = 0
            End If
            dtTargetFinal.Rows.Add(newdr)
        Next

        Dim sourceTbl2 As DataTable = dtTargetFinal.Copy()
        Dim query2 = From row In sourceTbl2.Copy()
                Group row By MonthY = row.Field(Of String)("MonthY") Into VanGroup = Group
                Select New With {
                    Key MonthY,
                    .TargetValue = VanGroup.Sum(Function(r) r.Field(Of Decimal)("TargetValue")),
                    .AchievementValue = VanGroup.Sum(Function(r) r.Field(Of Decimal)("AchievementValue")),
                    .TotalCalls = VanGroup.Sum(Function(r) r.Field(Of Integer)("TotalCalls")),
                    .ProductiveCalls = VanGroup.Sum(Function(r) r.Field(Of Integer)("ProductiveCalls"))
               }

        RadTAchart.DataSource = query2
        RadTAchart.DataBind()
        RadTAchart.Visible = True
            '  ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
    End Sub



    Private Sub BtnPDF_Click(sender As Object, e As EventArgs) Handles BtnExportPDF.Click
        Try
            If Me.ddlOrganization.SelectedIndex <= 0 Then
                MessageBoxValidation("Please select a organisation", "Validation")
                Exit Sub
            End If
            If Me.ddlMgr.SelectedIndex <= 0 Then
                MessageBoxValidation("Please select a sales manager", "Validation")
                Exit Sub
            End If

            Export("PDF")

        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub
    Sub Export(format As String)
        Try


            rpbFilter.Items(0).Expanded = False
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

            Dim dt As New DataTable


            Me.lbl_org.Text = Me.ddlOrganization.SelectedItem.Text
            Me.lbl_van.Text = Me.ddlMgr.SelectedItem.Text
            Me.lbl_from.Text = Me.StartTime.SelectedDate.Value.ToString("MMM-yyyy")

            dt = objRep.SMTeamPerformance(Err_No, Err_Desc, IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue), IIf(Me.ddlMgr.SelectedIndex <= 0, "0", Me.ddlMgr.SelectedValue), Me.hfSMonth.Value)
            Dim VanCount As Integer = 0
            For Each r As DataRow In dt.Rows

                If r("Description").ToString() = "Team Size" Then
                    VanCount = CInt(r("TotValue").ToString())
                End If

            Next
            If dt.Rows.Count > 0 Then
                Me.hfDecimal.Value = dt.Rows(0)("DecimalDigits").ToString()
                Me.hfCurrency.Value = dt.Rows(0)("Currency").ToString()
            End If

            Dim Sdate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM") + "-01-" + DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("yyyy")

            Me.lblC.Text = Me.hfCurrency.Value

            Args.Visible = True


            Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
            rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)



            Dim org As New ReportParameter
            org = New ReportParameter("OID", CStr(IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue)))


            Dim SM As New ReportParameter
            SM = New ReportParameter("SMID", CStr(IIf(Me.ddlMgr.SelectedIndex <= 0, "0", Me.ddlMgr.SelectedValue)))


            Dim sdat As New ReportParameter
            sdat = New ReportParameter("MonthYear", Sdate)


            Dim SalesorgName As New ReportParameter
            SalesorgName = New ReportParameter("OrgName", CStr(Me.lbl_org.Text))

            Dim SMName As New ReportParameter
            SMName = New ReportParameter("SMName", CStr(Me.lbl_van.Text))

            Dim TeamSize As New ReportParameter
            TeamSize = New ReportParameter("TeamSize", CStr(VanCount))

            Dim Currency As New ReportParameter
            Currency = New ReportParameter("Currency", CStr(Me.hfCurrency.Value))

            Dim DecimalDigits As New ReportParameter
            DecimalDigits = New ReportParameter("DecimalDigits", CStr(Me.hfDecimal.Value))

            Dim Mode As New ReportParameter
            Mode = New ReportParameter("Mode", "Top 10")


            rview.ServerReport.SetParameters(New ReportParameter() {org, SM, sdat, SalesorgName, SMName, TeamSize, Currency, DecimalDigits, Mode})

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
                Response.AddHeader("Content-disposition", "attachment;filename=SMScoreCard.pdf")
                Response.AddHeader("Content-Length", bytes.Length)

            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=SMScoreCard.xls")
                Response.AddHeader("Content-Length", bytes.Length)
            End If
            Response.OutputStream.Write(bytes, 0, bytes.Length)
            Response.OutputStream.Flush()
            Response.OutputStream.Close()
            Response.Flush()
            Response.Close()
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub

    Private Sub BtnExportExcel_Click(sender As Object, e As EventArgs) Handles BtnExportExcel.Click
        Try
            If Me.ddlOrganization.SelectedIndex <= 0 Then
                MessageBoxValidation("Please select a organisation", "Validation")
                Exit Sub
            End If
            If Me.ddlMgr.SelectedIndex <= 0 Then
                MessageBoxValidation("Please select a sales manager", "Validation")
                Exit Sub
            End If
            Export("Excel")

        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        BindMGR()

        Me.hfRow.Value = IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue)
        Me.lblSales.Text = "0"
        Me.lblSalesAvg.Text = "0"
        Me.lblRetAvg.Text = "0"
        Me.lblReturn.Text = "0"
        Me.lblColAvg.Text = "0"
        Me.lblCollection.Text = "0"
        Me.lblColCurr.Text = ""
        Me.lblOrdCurr.Text = ""
        Me.lblRetCurr.Text = ""
        Me.lblTeamSize.Text = "0"
        Me.lblTotalCalls.Text = "0"
        Me.lblAvgCalls.Text = "0"
        Me.lblVCnt.Text = ""
        Me.lblTCount.Text = ""
        Me.StartTime.SelectedDate = Now.Date
        rpt.Visible = False
        Args.Visible = False
    End Sub

    Private Sub GrowthTab_TabClick(sender As Object, e As RadTabStripEventArgs) Handles GrowthTab.TabClick
        Try



            If Me.ddlOrganization.SelectedIndex <= 0 Then
                MessageBoxValidation("Please select a organisation", "Validation")
                rpt.Visible = False
                Args.Visible = False

                Exit Sub

            End If
            If Me.ddlMgr.SelectedIndex <= 0 Then
                MessageBoxValidation("Please select a sales manager", "Validation")
                rpt.Visible = False
                Args.Visible = False
                Exit Sub
            End If
            Me.hfRow.Value = IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue)
            Me.hfSE.Value = IIf(Me.ddlMgr.SelectedIndex <= 0, "0", Me.ddlMgr.SelectedValue)

            Dim Sdate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM") + "-01-" + DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("yyyy")

            Me.hfSMonth.Value = Sdate
            Me.lblC.Text = Me.lblOrdCurr.Text

            If GrowthTab.SelectedIndex = 0 Then

                If Tab_Salesgrowth.SelectedIndex = 0 Then
                    RadMultiPage1.SelectedIndex = 0
                ElseIf Tab_Salesgrowth.SelectedIndex = 1 Then
                    RadMultiPage1.SelectedIndex = 1
                Else
                    RadMultiPage1.SelectedIndex = 0
                End If

            ElseIf GrowthTab.SelectedIndex = 1 Then

                If Tab_TA.SelectedIndex = 0 Then
                    RadMultiPage1.SelectedIndex = 2
                ElseIf Tab_TA.SelectedIndex = 1 Then
                    RadMultiPage1.SelectedIndex = 3
                Else
                    RadMultiPage1.SelectedIndex = 2
                End If
            End If






            ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)

        Catch ex As Exception
            log.Debug(ex.Message.ToString())
        End Try

    End Sub

    Private Sub BtnExportBiffExcel_TA_Click(sender As Object, e As EventArgs) Handles BtnExportBiffExcel_TA.Click
        Dim dtTargetVsAchiev As New DataTable
        dtTargetVsAchiev = objRep.SMLast3MonthsTargetVsAchiev(Err_No, Err_Desc, IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue), IIf(Me.ddlMgr.SelectedIndex <= 0, "0", Me.ddlMgr.SelectedValue), Me.hfSMonth.Value)
        Dim dtFinal As New DataTable
        dtFinal.Columns.Add("Description", System.Type.GetType("System.String"))
        Dim DtM As New DataTable
        DtM = dtTargetVsAchiev.DefaultView.ToTable(True, "MonthYear")
        For Each sdr As DataRow In DtM.Rows
            dtFinal.Columns.Add(sdr("MonthYear").ToString, System.Type.GetType("System.Decimal"))
        Next
        For i As Integer = 1 To 4
            Dim seldR() As DataRow
            seldR = dtTargetVsAchiev.Select("DispOrder=" & i)
            If seldR.Length > 0 Then
                Dim dr As DataRow
                dr = dtFinal.NewRow
                dr("Description") = seldR(0)("Description")
                For Each sdr As DataRow In DtM.Rows
                    If seldR.CopyToDataTable.Select("MonthYear='" & sdr("MonthYear") & "'").Length > 0 Then
                        dr(sdr("MonthYear")) = seldR.CopyToDataTable.Select("MonthYear='" & sdr("MonthYear") & "'")(0)("TotValue")
                    Else
                        dr(sdr("MonthYear")) = 0
                    End If
                Next
                dtFinal.Rows.Add(dr)
            End If
        Next
        dtTargetVsAchiev = Nothing
        If dtFinal.Rows.Count > 0 Then

            Using package As New ExcelPackage()
                ' add a new worksheet to the empty workbook
                Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
                Worksheet.Cells("A1").LoadFromDataTable(dtFinal, True)
                Worksheet.Cells.AutoFitColumns()


                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename=TargetVsAchiev3months.xlsx")

                Using MyMemoryStream As New MemoryStream()
                    package.SaveAs(MyMemoryStream)
                    MyMemoryStream.WriteTo(Response.OutputStream)
                    Response.AddHeader("Content-Length", MyMemoryStream.Length)
                    Response.Flush()
                    Response.Close()
                End Using
            End Using
        End If
    End Sub

    Private Sub gvTargetVsAchiev_CellDataBound(sender As Object, e As PivotGridCellDataBoundEventArgs) Handles gvTargetVsAchiev.CellDataBound
        Try

            If TypeOf e.Cell Is PivotGridDataCell Then
                Dim cell As PivotGridDataCell = TryCast(e.Cell, PivotGridDataCell)

                If cell.CellType = PivotGridDataCellType.DataCell OrElse cell.CellType = PivotGridDataCellType.RowTotalDataCell Then
                    Select Case TryCast(cell.Field, PivotGridAggregateField).DataField
                        Case "TotValue"
                            If cell.DataItem.ToString().Length > 0 Then
                                cell.Text = FormatNumber(CDbl(cell.DataItem), hfDecimal.Value)
                            End If
                            Exit Select
                    End Select
                End If
            End If


        Catch ex As Exception
            log.Debug(ex.Message.ToString())
        End Try
    End Sub

    Private Sub gvSalesGrowth_CellDataBound(sender As Object, e As PivotGridCellDataBoundEventArgs) Handles gvSalesGrowth.CellDataBound
        Try

            If TypeOf e.Cell Is PivotGridDataCell Then
                Dim cell As PivotGridDataCell = TryCast(e.Cell, PivotGridDataCell)

                If cell.CellType = PivotGridDataCellType.DataCell OrElse cell.CellType = PivotGridDataCellType.RowTotalDataCell Then
                    Select Case TryCast(cell.Field, PivotGridAggregateField).DataField
                        Case "TotValue"
                            If cell.DataItem.ToString().Length > 0 Then
                                cell.Text = FormatNumber(CDbl(cell.DataItem), hfDecimal.Value)
                            End If
                            Exit Select
                    End Select
                End If
            End If


        Catch ex As Exception
            log.Debug(ex.Message.ToString())
        End Try
    End Sub
End Class