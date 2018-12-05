Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Public Class Rep_CustomerVisitDetail
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer
    Private ReportPath As String = "VisitDetails"
    Dim dv As New DataView
    Private Const PageID As String = "P206"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub lbTradeImages_Click(sender As Object, e As EventArgs) Handles lbTradeImages.Click
       
        Me.MapWindow.VisibleOnPageLoad = True
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        If Not IsPostBack Then
            If Not Request.QueryString("ID") Is Nothing And Not Request.QueryString("ID") Is Nothing And Not Request.QueryString("d") Is Nothing Then
                hfVisitID.Value = Request.QueryString("ID")
                hfOrg.Value = Request.QueryString("OID")
                hfDecimal.Value = Request.QueryString("d")
                hFromDate.Value = Request.QueryString("FromDate")
                hToDate.Value = Request.QueryString("ToDate")
                'lbl_CusName.Text = Session("CustName")
                'lbl_Van.Text = Session("Van")
                'lbl_Date.Text = Session("Date")

                'horg.Value = Request.QueryString("Org_ID")
                'hvan.Value = Request.QueryString("SID")
                'hDate.Value = CDate(Request.QueryString("Vdate")).ToString("dd-MMM-yyyy")
                LoadVisitDetails()
            Else
                Response.Redirect("Rep_CustomerVisits.aspx")
            End If

            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            If Not HasPermission Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If
            ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
            Try

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
        Else
            MapWindow.ViewStateMode = False
        End If
    End Sub

    Private Sub LoadVisitDetails()
        Dim ObjReport As New SalesWorx.BO.Common.Reports
        Try
            '' Loading Visit Details  

            Try
                Dim dtVisit As DataTable = ObjReport.GetVisitDetail(Err_No, Err_Desc, hfVisitID.Value)

                If dtVisit IsNot Nothing AndAlso dtVisit.Rows.Count > 0 Then
                    Dim dRow As DataRow = dtVisit.Rows(0)
                    lbl_CusName.Text = dRow("Customer_Name")
                    lbl_CusNo.Text = dRow("Customer_No")
                    lbl_City.Text = IIf(dRow("City") Is DBNull.Value, "N/A", dRow("City"))
                    lbl_CrCus.Text = IIf(dRow("Credit_Cust_Name") Is DBNull.Value, "N/A", dRow("Credit_Cust_Name"))
                    lbl_CashCus.Text = IIf(dRow("CC_Name") Is DBNull.Value, "N/A", dRow("CC_Name"))
                    lbl_Date.Text = CDate(dRow("Visit_Start_Date")).ToString("dd-MMM-yyyy")
                    lbl_Start.Text = CDate(dRow("Visit_Start_Date")).ToString("HH:mm")
                    If dRow("Visit_End_Date") IsNot DBNull.Value Then
                        lbl_End.Text = CDate(dRow("Visit_End_Date")).ToString("HH:mm")
                    End If

                    lbl_Van.Text = dRow("SalesRep_Name")
                    lbl_Scanned.Text = IIf(dRow("Scanned_Closing") Is DBNull.Value, "N/A", dRow("Scanned_Closing"))

                    lbl_Odo.Text = IIf(dRow("Odo_Reading") Is DBNull.Value, "N/A", dRow("Odo_Reading"))
                    lbl_Emp.Text = IIf(dRow("Emp_Name") Is DBNull.Value, "N/A", dRow("Emp_Name"))
                    lbl_CCTelNo.Text = IIf(dRow("CC_TelNo") Is DBNull.Value, "N/A", dRow("CC_TelNo"))
                    lbl_ReasonNPV.Text = IIf(dRow("REASON_FOR_NON_PRD_VISIT") Is DBNull.Value, "N/A", dRow("REASON_FOR_NON_PRD_VISIT"))

                    Dim VisitID As String = hfVisitID.Value
                    ObjCustomer = New Customer()
                    Me.TradeList.DataSource = ObjCustomer.GetTradeImages(Err_No, Err_Desc, VisitID)
                    Me.TradeList.DataBind()

                    If TradeList.Items.Count > 0 Then
                        lbTradeImages.Visible = True
                    Else
                        lbTradeImages.Visible = False
                    End If

                End If

            Catch ex As Exception
                log.ErrorFormat("Error in Visit Loading // {0}", ex.Message)
            End Try

            '' Loading Sales Orders
            BindSO()

            '' Loading Returns
            BindReturns()

            '' Loading Distribution Check
            BindDistribution()

        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
    Private Sub BindReturns()
        Dim ObjReport As New SalesWorx.BO.Common.Reports
        Try

            Dim dtSO As New DataTable
            Dim SOQuery As String = String.Format(" and A.Visit_ID='{0}'", hfVisitID.Value)
            dtSO = ObjReport.GetReturns(Err_No, Err_Desc, SOQuery, hfOrg.Value, hFromDate.Value, hToDate.Value)
            gvReturns.DataSource = dtSO
            gvReturns.DataBind()
        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            ObjReport = Nothing
        End Try
    End Sub

    Private Sub BindDistribution()
        Dim ObjReport As New SalesWorx.BO.Common.Reports
        Try

            Dim dtSO As New DataTable
            Dim SOQuery As String = String.Format(" and A.Visit_ID='{0}'", hfVisitID.Value)
            dtSO = ObjReport.GetDistributionCheckList(Err_No, Err_Desc, hfOrg.Value, SOQuery, "0", CType(Session.Item("USER_ACCESS"), UserAccess).UserID)
            gvDisCheck.DataSource = dtSO
            gvDisCheck.DataBind()
        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            ObjReport = Nothing
        End Try
    End Sub


    Private Sub BindSO()
        Dim ObjReport As New SalesWorx.BO.Common.Reports
        Try

            Dim dtSO As New DataTable
            Dim SOQuery As String = String.Format(" and A.Visit_ID='{0}'", hfVisitID.Value)
            dtSO = ObjReport.GetOrderListing(Err_No, Err_Desc, SOQuery, hfOrg.Value, hFromDate.Value, hToDate.Value)
            gvRep.DataSource = dtSO
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
        BindSO()
    End Sub

    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender


        Dim ObjCommon As SalesWorx.BO.Common.Common
        Try

            ObjCommon = New SalesWorx.BO.Common.Common()
            Dim dtCur As New DataTable
            dtCur = ObjCommon.GetCurrencyByOrg(Err_No, Err_Desc, hfOrg.Value)
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
        BindSO()
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

    Private Sub gvReturns_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles gvReturns.ItemDataBound
        Try
            If TypeOf e.Item Is GridDataItem Then
                Dim item As GridDataItem = TryCast(e.Item, GridDataItem)
                If item IsNot Nothing Then
                    item.Cells(6).Text = FormatNumber(CDbl(item.Cells(6).Text), hfDecimal.Value)
                End If
            End If
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub gvReturns_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvReturns.PageIndexChanged
        BindReturns()
    End Sub

    Private Sub gvReturns_PreRender(sender As Object, e As EventArgs) Handles gvReturns.PreRender
        Dim ObjCommon As SalesWorx.BO.Common.Common
        Try

            ObjCommon = New SalesWorx.BO.Common.Common()
            Dim dtCur As New DataTable
            dtCur = ObjCommon.GetCurrencyByOrg(Err_No, Err_Desc, hfOrg.Value)
            If dtCur.Rows.Count > 0 Then
                lblDecimal.Text = "N" & dtCur.Rows(0)(1).ToString()
            End If
            For Each column As GridColumn In gvRep.MasterTableView.Columns
                If column.UniqueName = "ReturnsAmount" Then
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
    Private Sub gvReturns_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvReturns.SortCommand
        ViewState("SortFieldRet") = e.SortExpression
        SortDirectionRet = "flip"
        BindReturns()
    End Sub
    Private Property SortDirectionRet() As String
        Get
            If ViewState("SortDirectionRet") Is Nothing Then
                ViewState("SortDirectionRet") = "ASC"
            End If
            Return ViewState("SortDirectionRet").ToString()
        End Get
        Set(ByVal value As String)
            Dim s As String = SortDirectionRet

            If value = "flip" Then
                s = If(s = "ASC", "DESC", "ASC")
            Else
                s = value
            End If

            ViewState("SortDirectionRet") = s
        End Set
    End Property

    Private Sub gvDisCheck_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvDisCheck.PageIndexChanged
        BindDistribution()
    End Sub

    Private Sub gvDisCheck_SortCommand(sender As Object, e As GridSortCommandEventArgs) Handles gvDisCheck.SortCommand
        ViewState("SortFieldDis") = e.SortExpression
        SortDirectionDis = "flip"
        BindDistribution()
    End Sub
    Private Property SortDirectionDis() As String
        Get
            If ViewState("SortDirectionDis") Is Nothing Then
                ViewState("SortDirectionDis") = "ASC"
            End If
            Return ViewState("SortDirectionDis").ToString()
        End Get
        Set(ByVal value As String)
            Dim s As String = SortDirectionDis

            If value = "flip" Then
                s = If(s = "ASC", "DESC", "ASC")
            Else
                s = value
            End If

            ViewState("SortDirectionDis") = s
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


            Dim VisitID As New ReportParameter
            VisitID = New ReportParameter("VisitID", hfVisitID.Value)

            Dim OrgID As New ReportParameter
            OrgID = New ReportParameter("OrgID", hfOrg.Value)

            Dim CusNo As New ReportParameter
            CusNo = New ReportParameter("CustNo", lbl_CusNo.Text)

            rview.ServerReport.SetParameters(New ReportParameter() {VisitID, OrgID, CusNo})

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
                Response.AddHeader("Content-disposition", "attachment;filename=CustomerVisitDetails.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=CustomerVisitDetails.xls")
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
End Class