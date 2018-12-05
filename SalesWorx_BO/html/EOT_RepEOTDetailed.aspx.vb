Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Public Class EOT_RepEOTDetailed
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer
    Private ReportPath As String = "EOTReportDetailed_new"
    Dim dv As New DataView
    Private Const PageID As String = "P409"
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
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        If Not IsPostBack Then
            If Not Request.QueryString("SID") Is Nothing And Not Request.QueryString("Org_ID") Is Nothing And Not Request.QueryString("Vdate") Is Nothing Then
                horg.Value = Request.QueryString("Org_ID")
                hvan.Value = Request.QueryString("SID")
                hDate.Value = CDate(Request.QueryString("Vdate")).ToString("dd-MMM-yyyy")
                LoadEOTDetails()
            Else
                'EOT_RepSummary
                'Response.Redirect("Rep_EOTSummary.aspx")
                Response.Redirect("EOT_RepSummary.aspx")

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
        End If
    End Sub
    Sub LoadEOTDetails()

        Dim dtcurrency As DataTable
        Dim ObjReport As New SalesWorx.BO.Common.Reports
        dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, horg.Value)

        Dim Currency As String = ""
        If dtcurrency.Rows.Count > 0 Then
            Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
            lblDecimal.Text = "N" & dtcurrency.Rows(0)("Decimal_Digits")
        End If


        Dim dt As New DataTable
        'Task 5 Rakesh EOT REPORT
        ' dt = ObjReport.GetEOTDetails(Err_No, Err_Desc, horg.Value, hvan.Value, hDate.Value)
        dt = ObjReport.GetEOTReportDetails(Err_No, Err_Desc, horg.Value, hvan.Value, hDate.Value)

        Dim visitStr As String = ""
        Dim transStr As String = ""
        Dim Str As String = ""
        If dt.Rows.Count > 0 Then
            lbl_Date.Text = CDate(hDate.Value).ToString("dd-MMM-yyyy")
            lbl_Sp.Text = dt.Rows(0)("SalesRep_Name").ToString
            lbl_Schcalls.Text = Format(Val(dt.Rows(0)("Planned").ToString), "#,##0")
            lbl_visited.Text = Format(Val(dt.Rows(0)("Visited").ToString), "#,##0")
            lbl_visitOut.Text = Format(Val(dt.Rows(0)("VisitsOutofPlan").ToString), "#,##0")
            lbl_Success.Text = Format(Val(dt.Rows(0)("Successvisits").ToString), "#,##0")
            lbl_plannedvisited.Text = Format(Val(dt.Rows(0)("PlanenedVisits").ToString), "#,##0")
            lbl_unplannedvisited.Text = Format(Val(dt.Rows(0)("VisitsOutofPlan").ToString), "#,##0")
            lbl_adherence.Text = Format(Val(dt.Rows(0)("Adherence").ToString), "#,##0.00")
            lbl_plannedOutlets.Text = Format(Val(dt.Rows(0)("PlannedOutlets").ToString), "#,##0")
            lbl_UnplannedOutlets.Text = Format(Val(dt.Rows(0)("UnPlannedOutlets").ToString), "#,##0")
            lbl_totoutvisited.Text = Format(Val(dt.Rows(0)("OutletVisited").ToString), "#,##0")
            lbl_billed.Text = Format(Val(dt.Rows(0)("BilledCustomer").ToString), "#,##0")
            lbl_productivity.Text = Format(Val(dt.Rows(0)("Prodctivity").ToString), "#,##0.00")
            lblCashSales.Text = Format(Val(dt.Rows(0)("orderCashvalue").ToString), lblDecimal.Text)
            'lblCashSalesCount.Text = Format(Val(dt.Rows(0)("Ordercashcount").ToString), "#,##0")
            lblCreditSales.Text = Format(Val(dt.Rows(0)("orderCreditvalue").ToString), lblDecimal.Text)
            'lblCreditSalesCount.Text = Format(Val(dt.Rows(0)("OrderCreditcount").ToString), "#,##0")
            lblTotalSales.Text = Format(Val(dt.Rows(0)("OrderValue").ToString), lblDecimal.Text)
            'lblTotalSalescount.Text = Format(Val(dt.Rows(0)("OrderCount").ToString), "#,##0")
            lbl_retuns.Text = Format(Val(dt.Rows(0)("ReturnValue").ToString), lblDecimal.Text)
            lbl_Resellable.Text = Format(Val(dt.Rows(0)("Reselleble").ToString), lblDecimal.Text)
            lbl_nonResell.Text = Format(Val(dt.Rows(0)("NoReselleble").ToString), lblDecimal.Text)
            'lbl_retunsCount.Text = Format(Val(dt.Rows(0)("ReturnCount").ToString), "#,##0")
            lblnet.Text = Format(Val(dt.Rows(0)("OrderValue").ToString) - Val(dt.Rows(0)("ReturnValue").ToString), lblDecimal.Text)
            'lbl_CollectionCount.Text = Format(Val(dt.Rows(0)("CollectionCount").ToString), "#,##0")
            lbl_Collection.Text = Format(Val(dt.Rows(0)("CollectionValue").ToString), lblDecimal.Text)

            lbl_Currency.Text = Currency

            LoadVisits()
            LoadOrders()
            LoadReturnss()
            LoadCollection()
        End If

    End Sub
    Sub LoadCollection()
        Dim SearchQuery As String = ""
        SearchQuery = SearchQuery & " And A.Collected_By=" & hvan.Value
        SearchQuery = SearchQuery & " And A.Collected_On >=convert(datetime,'" & CDate(hDate.Value).ToString("dd-MMM-yyyy") & "',103)"
        SearchQuery = SearchQuery & " And A.Collected_On <= convert(datetime,'" & CDate(hDate.Value).ToString("dd-MMM-yyyy") & " 23:59:59',103)"
        Dim ObjReport As New SalesWorx.BO.Common.Reports
        Dim dt As New DataTable
        dt = ObjReport.GetCollectionListing(Err_No, Err_Desc, SearchQuery, horg.Value)

        If dt.Rows.Count > 0 Then
            gvRepcollection.ClientSettings.Scrolling.AllowScroll = True
            gvRepcollection.ClientSettings.Scrolling.UseStaticHeaders = True
        Else
            gvRepcollection.ClientSettings.Scrolling.AllowScroll = False
            gvRepcollection.ClientSettings.Scrolling.UseStaticHeaders = False
        End If

        gvRepcollection.DataSource = dt
        gvRepcollection.DataBind()

        '' Dynamically setting Grid height is disabled as per designer advice and it will be manages through CSS

        ''If dt.Rows.Count > 8 Then
        ''    gvRepcollection.ClientSettings.Scrolling.ScrollHeight = "250"
        ''Else
        ''    gvRepcollection.ClientSettings.Scrolling.ScrollHeight = dt.Rows.Count * 50
        ''End If
        Dim query = (From UserEntry In dt _
                       Group UserEntry By key = UserEntry.Field(Of String)("Collection_Type") Into Group _
                       Select PayMode = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("Amount"))).ToList

        For Each x In query
            If x.PayMode.ToUpper = "CASH" Then
                lbl_cash.Text = Format(x.Total, lblDecimal.Text)
            End If
            If x.PayMode.ToUpper = "CURR-CHQ" Then
                lbl_CDC.Text = Format(x.Total, lblDecimal.Text)
            End If
            If x.PayMode.ToUpper = "PDC" Then
                lbl_PDc.Text = Format(x.Total, lblDecimal.Text)
            End If
        Next

        'Dim NoofCashCollectionCount = (From Coll In dt _
        '                  Where Coll.Field(Of String)("Collection_Type") = "CASH" _
        '                  Select CustCount = Coll.Field(Of String)("Collection_Ref_No") Distinct).ToList()
        'If Not NoofCashCollectionCount Is Nothing Then
        '    lbl_CashCollCount.Text = Format(NoofCashCollectionCount.Count, "#,##0")
        'End If


        'Dim NoofPDCCollectionCount = (From Coll In dt _
        '                 Where Coll.Field(Of String)("Collection_Type") = "PDC" _
        '                 Select CustCount = Coll.Field(Of String)("Collection_Ref_No") Distinct).ToList()
        'If Not NoofPDCCollectionCount Is Nothing Then
        '    lbl_PDCCount.Text = Format(NoofPDCCollectionCount.Count, "#,##0")
        'End If


        'Dim NoofCDCCollectionCount = (From Coll In dt _
        '                Where Coll.Field(Of String)("Collection_Type") = "CURR-CHQ" _
        '                Select CustCount = Coll.Field(Of String)("Collection_Ref_No") Distinct).ToList()
        'If Not NoofCDCCollectionCount Is Nothing Then
        '    lbl_CDCCount.Text = Format(NoofCDCCollectionCount.Count, "#,##0")
        'End If

    End Sub

    Sub LoadReturnss()
        Dim ObjReport As New SalesWorx.BO.Common.Reports
        Dim dtReturn As New DataTable
        dtReturn = ObjReport.GetDailySalesReport_Return(Err_No, Err_Desc, horg.Value, hvan.Value, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, CDate(hDate.Value).ToString("dd-MMM-yyyy"), CDate(hDate.Value).ToString("dd-MMM-yyyy"))

        If dtReturn.Rows.Count > 0 Then
            gvRepReturn.ClientSettings.Scrolling.AllowScroll = True
            gvRepReturn.ClientSettings.Scrolling.UseStaticHeaders = True
        Else
            gvRepReturn.ClientSettings.Scrolling.AllowScroll = False
            gvRepReturn.ClientSettings.Scrolling.UseStaticHeaders = False
        End If

        gvRepReturn.DataSource = dtReturn
        gvRepReturn.DataBind()
        '' Dynamically setting Grid height is disabled as per designer advice and it will be manages through CSS
        ''If dtReturn.Rows.Count > 8 Then
        ''    gvRepReturn.ClientSettings.Scrolling.ScrollHeight = "250"
        ''Else
        ''    gvRepReturn.ClientSettings.Scrolling.ScrollHeight = dtReturn.Rows.Count * 50
        ''End If


    End Sub
    Sub LoadOrders()
        Dim ObjReport As New SalesWorx.BO.Common.Reports
        Dim dt As New DataTable
        dt = ObjReport.GetDailySalesReport_Order(Err_No, Err_Desc, horg.Value, hvan.Value, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, CDate(hDate.Value).ToString("dd-MMM-yyyy"), CDate(hDate.Value).ToString("dd-MMM-yyyy"))

        If dt.Rows.Count > 0 Then
            gvRep.ClientSettings.Scrolling.AllowScroll = True
            gvRep.ClientSettings.Scrolling.UseStaticHeaders = True
        Else
            gvRep.ClientSettings.Scrolling.AllowScroll = False
            gvRep.ClientSettings.Scrolling.UseStaticHeaders = False
        End If

        gvRep.DataSource = dt
        gvRep.DataBind()

        '' Dynamically setting Grid height is disabled as per designer advice and it will be manages through CSS

        ' ''If dt.Rows.Count > 8 Then
        ' ''    gvRep.ClientSettings.Scrolling.ScrollHeight = "250"
        ' ''Else
        ' ''    gvRep.ClientSettings.Scrolling.ScrollHeight = dt.Rows.Count * 50
        ' ''End If
    End Sub
    Sub LoadVisits()
        Dim SearchQuery As String = ""
        SearchQuery = SearchQuery & " And A.SalesRep_ID=" & hvan.Value
        SearchQuery = SearchQuery & " And A.Visit_Start_Date >=convert(datetime,'" & CDate(hDate.Value).ToString("dd-MMM-yyyy") & "',103)"
        SearchQuery = SearchQuery & " And A.Visit_Start_Date <= convert(datetime,'" & CDate(hDate.Value).ToString("dd-MMM-yyyy") & " 23:59:59',103)"
        Dim ObjReport As New SalesWorx.BO.Common.Reports
        Dim dt As New DataTable
        dt = ObjReport.GetCustomerVisitsForEOT(Err_No, Err_Desc, SearchQuery, horg.Value)

        If dt.Rows.Count > 0 Then
            gvRepVisits.ClientSettings.Scrolling.AllowScroll = True
            gvRepVisits.ClientSettings.Scrolling.UseStaticHeaders = True
        Else
            gvRepVisits.ClientSettings.Scrolling.AllowScroll = False
            gvRepVisits.ClientSettings.Scrolling.UseStaticHeaders = False
        End If

        gvRepVisits.DataSource = dt
        gvRepVisits.DataBind()

        '' Dynamically setting Grid height is disabled as per designer advice and it will be manages through CSS
        ''If dt.Rows.Count > 8 Then
        ''    gvRepVisits.ClientSettings.Scrolling.ScrollHeight = "250"
        ''Else
        ''    gvRepVisits.ClientSettings.Scrolling.ScrollHeight = dt.Rows.Count * 56
        ''End If
    End Sub

    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender
        For Each column As GridColumn In gvRep.MasterTableView.Columns
            If column.UniqueName = "Order_amt" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
            End If
        Next
    End Sub

    Private Sub dgv_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvRep.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        LoadOrders()
    End Sub
    Private Sub dgv_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged

        LoadOrders()
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

    Private Sub BtnExport_Click(sender As Object, e As EventArgs) Handles BtnExportExcel.Click
        Try

            Export("Excel")

        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub
    Private Sub BtnPDF_Click(sender As Object, e As EventArgs) Handles BtnExportPDF.Click
        Try

            Export("PDF")

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

        Dim OrgID As New ReportParameter
        OrgID = New ReportParameter("OID", horg.Value)

        Dim FromDate As New ReportParameter
        FromDate = New ReportParameter("FromDate", CDate(hDate.Value).ToString("dd-MMM-yyyy"))



        Dim SalesRep_ID As New ReportParameter
        SalesRep_ID = New ReportParameter("SID", hvan.Value)

        Dim UID As New ReportParameter
        UID = New ReportParameter("Uid", objUserAccess.UserID)

        Dim SearchQuery As String = ""
        SearchQuery = SearchQuery & " And A.SalesRep_ID=" & hvan.Value
        SearchQuery = SearchQuery & " And A.Visit_Start_Date >=convert(datetime,'" & CDate(hDate.Value).ToString("dd-MMM-yyyy") & "',103)"
        SearchQuery = SearchQuery & " And A.Visit_Start_Date <= convert(datetime,'" & CDate(hDate.Value).ToString("dd-MMM-yyyy") & " 23:59:59',103)"

        Dim SearchParams As New ReportParameter
        SearchParams = New ReportParameter("SearchParams", SearchQuery)


        SearchQuery = ""
        SearchQuery = SearchQuery & " And A.Collected_By=" & hvan.Value
        SearchQuery = SearchQuery & " And A.Collected_On >=convert(datetime,'" & CDate(hDate.Value).ToString("dd-MMM-yyyy") & "',103)"
        SearchQuery = SearchQuery & " And A.Collected_On <= convert(datetime,'" & CDate(hDate.Value).ToString("dd-MMM-yyyy") & " 23:59:59',103)"
        Dim SearchparamCol As New ReportParameter
        SearchparamCol = New ReportParameter("SearchparamCol", SearchQuery)


        rview.ServerReport.SetParameters(New ReportParameter() {OrgID, SalesRep_ID, FromDate, UID, SearchParams, SearchparamCol})

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
            Response.AddHeader("Content-disposition", "attachment;filename=EOTDetailed.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=EOTDetailed.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()
    End Sub

    Private Sub gvRepVisits_PreRender(sender As Object, e As EventArgs) Handles gvRepVisits.PreRender
        For Each column As GridColumn In gvRepVisits.MasterTableView.Columns
            If column.UniqueName = "OrderAmt" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
            End If
            If column.UniqueName = "RMA" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
            End If
            If column.UniqueName = "Payment" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
            End If
        Next
    End Sub

    Private Sub gvRepVisits_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvRepVisits.SortCommand
        ViewState("SortFieldVisits") = e.SortExpression
        SortDirectionVisits = "flip"
        LoadVisits()
    End Sub
    Protected Sub ViewDetails_Click(ByVal sender As Object, ByVal e As EventArgs)

        Dim btnEdit As LinkButton = TryCast(sender, LinkButton)
        Dim row As Telerik.Web.UI.GridDataItem = DirectCast(btnEdit.NamingContainer, Telerik.Web.UI.GridDataItem)
        Dim vdate As String = CType(row.FindControl("HVisitDate"), HiddenField).Value
        Dim sid As String = CType(row.FindControl("HSID"), HiddenField).Value

        Response.Redirect("EOT_RepEOTDetailed.aspx?SID=" & sid & "&Vdate=" & vdate)

    End Sub
    Private Sub gvRepVisits_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvRepVisits.PageIndexChanged

        LoadVisits()
    End Sub
    Private Property SortDirectionVisits() As String
        Get
            If ViewState("SortDirectionVisits") Is Nothing Then
                ViewState("SortDirectionVisits") = "ASC"
            End If
            Return ViewState("SortDirectionVisits").ToString()
        End Get
        Set(ByVal value As String)
            Dim s As String = SortDirectionVisits

            If value = "flip" Then
                s = If(s = "ASC", "DESC", "ASC")
            Else
                s = value
            End If

            ViewState("SortDirectionVisits") = s
        End Set
    End Property
    
 
    Private Property SortReturnDirection() As String
        Get
            If ViewState("SortReturnDirection") Is Nothing Then
                ViewState("SortReturnDirection") = "ASC"
            End If
            Return ViewState("SortReturnDirection").ToString()
        End Get
        Set(ByVal value As String)
            Dim s As String = SortReturnDirection

            If value = "flip" Then
                s = If(s = "ASC", "DESC", "ASC")
            Else
                s = value
            End If

            ViewState("SortReturnDirection") = s
        End Set
    End Property
    Private Sub gvRepReturn_PreRender(sender As Object, e As EventArgs) Handles gvRepReturn.PreRender
        For Each column As GridColumn In gvRepReturn.MasterTableView.Columns
            If column.UniqueName = "Resellable" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
            End If
            If column.UniqueName = "NonResellable" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
            End If
            If column.UniqueName = "Total" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
            End If
        Next
    End Sub
    Private Sub gvRepReturn_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvRepReturn.SortCommand
        ViewState("SortField") = e.SortExpression
        SortReturnDirection = "flip"
        LoadReturnss()
    End Sub
    Private Sub gvRepReturn_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvRepReturn.PageIndexChanged
        LoadReturnss()
    End Sub
    Private Sub gvRepcollection_PreRender(sender As Object, e As EventArgs) Handles gvRepcollection.PreRender

        For Each column As GridColumn In gvRep.MasterTableView.Columns
            If column.UniqueName = "Amount" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
            End If
            If column.UniqueName = "Discount" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
            End If
        Next
    End Sub
    Private Sub dgvCollection_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvRepcollection.SortCommand
        ViewState("SortDirectionCollection") = e.SortExpression
        SortDirectionCollection = "flip"
        LoadCollection()
    End Sub
    Private Sub gvRepcollection_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvRepcollection.PageIndexChanged

        LoadCollection()
    End Sub
    Private Property SortDirectionCollection() As String
        Get
            If ViewState("SortDirectionCollection") Is Nothing Then
                ViewState("SortDirectionCollection") = "ASC"
            End If
            Return ViewState("SortDirectionCollection").ToString()
        End Get
        Set(ByVal value As String)
            Dim s As String = SortDirectionCollection

            If value = "flip" Then
                s = If(s = "ASC", "DESC", "ASC")
            Else
                s = value
            End If

            ViewState("SortDirectionCollection") = s
        End Set
    End Property
End Class