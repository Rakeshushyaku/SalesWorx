Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Partial Public Class RepBestSellers
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "BestSeller"

    Private Const PageID As String = "P345"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single

    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Protected Sub Button_Click(sender As Object, e As ImageClickEventArgs)
        gvSKU.MasterTableView.FilterExpression = String.Empty
        gvSKU.MasterTableView.Columns(0).CurrentFilterValue = ""
        gvSKU.MasterTableView.Columns(1).CurrentFilterValue = ""
        gvSKU.MasterTableView.Columns(3).CurrentFilterValue = ""
        gvSKU.MasterTableView.Columns(4).CurrentFilterValue = ""
        gvSKU.MasterTableView.Rebind()
    End Sub
    Protected Sub Button1_Click(sender As Object, e As ImageClickEventArgs)
        gvvans.MasterTableView.FilterExpression = String.Empty
        gvvans.MasterTableView.Columns(0).CurrentFilterValue = ""
        gvvans.MasterTableView.Rebind()
    End Sub
   
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
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))


                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                    Me.hfOrg.Value = Me.ddlOrganization.SelectedValue
                End If

                txtFromDate.SelectedDate = DateAdd("m", 0, DateSerial(Year(Today), Month(Today), 1))
                txtToDate.SelectedDate = Now

                LoadOrgDetails()

                Me.hfSMonth.Value = txtFromDate.SelectedDate
                Me.hfEMonth.Value = txtToDate.SelectedDate
                CType(gvRep.Columns(4), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"

                CType(gvRep.Columns(5), GridBoundColumn).DataFormatString = "{0:N0}"
                'CType(gvRep.Columns(0), GridBoundColumn).FooterAggregateFormatString = "Total:"
                CType(gvRep.Columns(4), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvRep.Columns(5), GridBoundColumn).FooterAggregateFormatString = "{0:N0}"


                CType(gvSKU.Columns(5), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvSKU.Columns(6), GridBoundColumn).DataFormatString = "{0:N0}"
                CType(gvSKU.Columns(7), GridBoundColumn).DataFormatString = "{0:N0}"

                CType(gvSKU.Columns(5), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvSKU.Columns(6), GridBoundColumn).FooterAggregateFormatString = "{0:N0}"

                CType(gvSKU.Columns(7), GridBoundColumn).FooterAggregateFormatString = "{0:N0}"

                CType(gvvans.Columns(3), GridBoundColumn).DataFormatString = "{0:N0}"

                CType(gvvans.Columns(3), GridBoundColumn).FooterAggregateFormatString = "{0:N0}"

                CType(gvvans.Columns(4), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"

                CType(gvvans.Columns(4), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"

                CType(gvvans.Columns(5), GridBoundColumn).DataFormatString = "{0:N0}"
                CType(gvvans.Columns(5), GridBoundColumn).FooterAggregateFormatString = "{0:N0}"

                Dim Currency As String = ""
                Dim dtcurrency As New DataTable
                Dim ObjReport As New SalesWorx.BO.Common.Reports
                dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

                If dtcurrency.Rows.Count > 0 Then
                    Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
                    Me.hfCurrency.Value = dtcurrency.Rows(0)("Currency_Code")
                    hfDecimal.Value = dtcurrency.Rows(0)("Decimal_Digits")
                End If

                gvvans.Columns(4).HeaderText = "Value <i class='fa fa-info-circle'></i>"
                gvSKU.Columns(5).HeaderText = "Value <i class='fa fa-info-circle'></i>"
                gvRep.Columns(4).HeaderText = "Value <i class='fa fa-info-circle'></i>"


                Me.ddlChartMode.SelectedIndex = 0

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

    Protected Sub ddlChartMode_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlChartMode.SelectedIndexChanged
        If ValidateInputs() = True And Args.Visible = True Then
            Dim VanStr As String = ""
            Dim vancnt As Integer = 0
            Dim vantxt As String = ""

            For Each item As RadComboBoxItem In ddlVan.Items
                If item.Checked Then
                    vancnt = vancnt + 1
                    VanStr = VanStr & item.Value & ","
                    vantxt = vantxt & item.Text & ","
                End If
            Next
            If Me.ddlChartMode.SelectedValue = "Van" Then
                Me.AgencyTab.Tabs(0).Text = IIf(vancnt > 10, "Top 10 Vans Value vs Volume vs No.of Invoices", "Value vs Volume vs No.of Invoices By Van")
                divChart1.Visible = True
                divChart2.Visible = False
                BindChart()

            Else
                Me.AgencyTab.Tabs(0).Text = "Top 10 SKU Value vs Volume vs No.of Invoices"
                divChart1.Visible = False
                divChart2.Visible = True
                BindChart1()
            End If
        End If
    End Sub
    Sub Export(format As String)

        Try



            Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
            rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)


           


            rpbFilter.Items(0).Expanded = False



            Dim VanStr As String = ""
            Dim vancnt As Integer = 0
            Dim vantxt As String = ""


            For Each item As RadComboBoxItem In ddlVan.Items
                If item.Checked Then
                    vancnt = vancnt + 1
                    VanStr = VanStr & item.Value & ","
                    vantxt = vantxt & item.Text & ","
                End If
            Next


            If vantxt <> "" Then
                vantxt = vantxt.Substring(0, vantxt.Length - 1)
            End If
            If VanStr = "" Then
                VanStr = "0"
            End If
            If vancnt = ddlVan.Items.Count Then
                lbl_van.Text = "All"
            Else
                lbl_van.Text = vantxt
            End If

            lbl_org.Text = ddlOrganization.SelectedItem.Text

            lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")
            Me.hfSMonth.Value = txtFromDate.SelectedDate
            Me.hfEMonth.Value = txtToDate.SelectedDate
            Me.hfOrg.Value = Me.ddlOrganization.SelectedValue
            Args.Visible = True
            rpt.Visible = True

            Dim Sdate As String = DateTime.Parse(Me.txtFromDate.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")
            Dim Edate As String = DateTime.Parse(Me.txtToDate.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")
            Dim org As New ReportParameter
            org = New ReportParameter("OID", CStr(Me.ddlOrganization.SelectedValue))


            Dim vans As New ReportParameter
            vans = New ReportParameter("VanList", VanStr)


            Dim sdat As New ReportParameter
            sdat = New ReportParameter("FMonth", Sdate)

            Dim edat As New ReportParameter
            edat = New ReportParameter("TMonth", Edate)

            Dim SalesorgName As New ReportParameter
            SalesorgName = New ReportParameter("OrgName", CStr(Me.lbl_org.Text))


            Dim VanName As New ReportParameter
            VanName = New ReportParameter("VanName", CStr(Me.lbl_van.Text))
            Dim Currency As New ReportParameter
            Currency = New ReportParameter("Currency", CStr(Me.hfCurrency.Value))

            Dim DecimalDigits As New ReportParameter
            DecimalDigits = New ReportParameter("DecimalDigits", CStr(Me.hfDecimal.Value))

            Dim Mode As New ReportParameter
            Mode = New ReportParameter("Mode", "Summary")

            Dim ChartMode As New ReportParameter
            ChartMode = New ReportParameter("ChartMode", CStr(Me.ddlChartMode.SelectedValue))
            Dim TotVan As New ReportParameter
            TotVan = New ReportParameter("VanCnt", CStr(vancnt))

            If Me.ddlChartMode.SelectedValue = "Van" Then
                Me.AgencyTab.Tabs(0).Text = IIf(vancnt > 10, "Top 10 Vans Value vs Volume vs No.of Invoices", "Value vs Volume vs No.of Invoices By Van")
                divChart1.Visible = True
                divChart2.Visible = False
                BindChart()

            Else
                Me.AgencyTab.Tabs(0).Text = "Top 10 SKU Value vs Volume vs No.of Invoices"
                divChart1.Visible = False
                divChart2.Visible = True
                BindChart1()
            End If


            rview.ServerReport.SetParameters(New ReportParameter() {org, vans, sdat, edat, SalesorgName, VanName, Currency, DecimalDigits, Mode, ChartMode, TotVan})


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
                Response.AddHeader("Content-disposition", "attachment;filename=BestSellers.pdf")
                Response.AddHeader("Content-Length", bytes.Length)

            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=BestSellers.xls")
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
    Private Sub BtnExport_Click(sender As Object, e As EventArgs) Handles BtnExportExcel.Click
        Try
            If ValidateInputs() Then
                Export("Excel")
            End If
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub
    Private Sub BtnPDF_Click(sender As Object, e As EventArgs) Handles BtnExportPDF.Click
        Try
            If ValidateInputs() Then
                Export("PDF")
            End If
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            gvRep.Visible = True
            Args.Visible = True
            rpt.Visible = True
            hfActCnt.Value = "0"
            BindReport()
        Else
            Args.Visible = False
            rpt.Visible = False
            gvRep.Visible = False
        End If
    End Sub
    Private Sub BindReport()
        Try
            Dim SearchQuery As String = ""
            If ValidateInputs() Then
                rpbFilter.Items(0).Expanded = False
                Dim objUserAccess As UserAccess
                objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
                Dim ObjReport As New SalesWorx.BO.Common.Reports

                Dim dt As New DataTable
                Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems

                Dim VanStr As String = ""
                Dim vancnt As Integer = 0
                Dim vantxt As String = ""

                For Each item As RadComboBoxItem In ddlVan.Items
                    If item.Checked Then
                        vancnt = vancnt + 1
                        VanStr = VanStr & item.Value & ","
                        vantxt = vantxt & item.Text & ","
                    End If
                Next


                If vantxt <> "" Then
                    vantxt = vantxt.Substring(0, vantxt.Length - 1)
                End If
                If VanStr = "" Then
                    VanStr = "0"
                End If
                If vancnt = ddlVan.Items.Count Then
                    lbl_van.Text = "All"
                Else
                    lbl_van.Text = vantxt
                End If

               
                Me.hfSE.Value = VanStr
                lbl_org.Text = ddlOrganization.SelectedItem.Text
              
                lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
                lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")
                Me.hfSMonth.Value = txtFromDate.SelectedDate
                Me.hfEMonth.Value = txtToDate.SelectedDate
                Me.hfOrg.Value = Me.ddlOrganization.SelectedValue
                Args.Visible = True
                rpt.Visible = True
                gvRep.Rebind()
                Dim VanActCnt As Integer = 8
                If gvRep.Items.Count > 0 Then

                    Dim lblVanCnt As Label = DirectCast(gvRep.Items(0).FindControl("lblTotVans"), Label)
                    VanActCnt = lblVanCnt.Text.Trim()
                End If
                'If VanActCnt > 4 Then
                '    Chartwrapper.Style.Add("height", (CInt(VanActCnt) * 70).ToString & "px")
                'Else
                '    Chartwrapper.Style.Add("height", "300px")
                'End If
                AgencyTab.Tabs(0).Selected = True
                Me.RadMultiPage21.SelectedIndex = 0



                Me.lblOrdCurr.Text = hfCurrency.Value
                Me.lblSales.Text = "0"
                Me.lblTeamSize.Text = "0"
                Me.lblTotalInvoices.Text = "0"
                Me.lblTotProducts.Text = "0"
                Me.lblVolume.Text = "0"


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

                Dim x As New DataTable
                x = ObjReport.GetBestSellersSummary(Err_No, Err_Desc, hfOrg.Value, VanStr, hfSMonth.Value, hfEMonth.Value, "Summary", Me.ddlChartMode.SelectedValue)

                If x.Rows.Count > 0 Then
                    Me.lblSales.Text = CDec(x.Rows(0)(0)).ToString("#,##" & LabelDecimalDigits)
                    Me.lblVolume.Text = CDec(x.Rows(0)(1)).ToString("#,##0.00")
                    Me.lblTotProducts.Text = x.Rows(0)(2).ToString()
                    Me.lblTotalInvoices.Text = x.Rows(0)(3).ToString()
                    Me.lblTeamSize.Text = x.Rows(0)(5).ToString()
                Else
                    Me.lblSales.Text = "0"
                    Me.lblTotalInvoices.Text = "0"
                    Me.lblTotProducts.Text = "0"
                    Me.lblVolume.Text = "0"
                    Me.lblTeamSize.Text = "0"
                End If

                Dim dtcurrency As New DataTable
                Dim Currency As String = ""
                dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

                If dtcurrency.Rows.Count > 0 Then
                    Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
                    Me.hfCurrency.Value = dtcurrency.Rows(0)("Currency_Code")
                    hfDecimal.Value = dtcurrency.Rows(0)("Decimal_Digits")
                End If

                gvvans.Columns(4).HeaderText = "Value <i class='fa fa-info-circle'></i>"
                gvSKU.Columns(5).HeaderText = "Value <i class='fa fa-info-circle'></i>"
                gvRep.Columns(4).HeaderText = "Value <i class='fa fa-info-circle'></i>"
                If Me.ddlChartMode.SelectedValue = "Van" Then
                    Me.AgencyTab.Tabs(0).Text = IIf(vancnt > 10, "Top 10 Vans Value vs Volume vs No.of Invoices", "Value vs Volume vs No.of Invoices By Van")
                    divChart1.Visible = True
                    divChart2.Visible = False
                    BindChart()

                Else
                    Me.AgencyTab.Tabs(0).Text = "Top 10 SKU Value vs Volume vs No.of Invoices"
                    divChart1.Visible = False
                    divChart2.Visible = True
                    BindChart1()
                End If


            End If
        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If ddlOrganization.SelectedItem.Value = "0" Then
            MessageBoxValidation("Select an Organisation", "Validation")
            SetFocus(ddlOrganization)
            Return bretval
        End If
        If txtFromDate.DateInput.Text = "" Then
            MessageBoxValidation("Enter valid ""From date"".", "Validation")
            SetFocus(txtFromDate)
            Return bretval
        End If

        If txtToDate.DateInput.Text = "" Then
            MessageBoxValidation("Enter valid ""To date"".", "Validation")
            SetFocus(txtToDate)
            Return bretval
        End If

        If CDate(txtFromDate.SelectedDate) > CDate(txtToDate.SelectedDate) Then
            MessageBoxValidation("Start Date should not be greater than End Date.", "Validation")
            Return bretval
        End If
        Dim VanStr As String = ""


        For Each item As RadComboBoxItem In ddlVan.Items
            If item.Checked Then
                VanStr = VanStr & "," & item.Value

            End If
        Next
        If String.IsNullOrEmpty(VanStr) Then
            MessageBoxValidation("Please select a van(s).", "Validation")
            Return bretval
        End If
        bretval = True
        Return bretval
    End Function



    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        LoadOrgDetails()

    End Sub
    Sub LoadOrgDetails()
        If Not (ddlOrganization.SelectedItem.Value = "0") Then
            hfActCnt.Value = "0"
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New SalesWorx.BO.Common.Common()
            ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
            ddlVan.DataValueField = "SalesRep_ID"
            ddlVan.DataTextField = "SalesRep_Name"
            ddlVan.DataBind()

            For Each item As RadComboBoxItem In ddlVan.Items
                item.Checked = True

            Next

            Dim dtcurrency As DataTable
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

            Dim Currency As String = ""
            If dtcurrency.Rows.Count > 0 Then
                Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
                Me.hfCurrency.Value = dtcurrency.Rows(0)("Currency_Code")
                hfDecimal.Value = dtcurrency.Rows(0)("Decimal_Digits")
            End If
            Me.lblC.Text = Me.hfCurrency.Value

            'Dim VanStr As String = ""
            'Dim vancnt As Integer = 0
            'Dim vantxt As String = ""

            'For Each item As RadComboBoxItem In ddlVan.Items
            '    If item.Checked Then
            '        vancnt = vancnt + 1
            '        VanStr = VanStr & item.Value & ","
            '        vantxt = vantxt & item.Text & ","
            '    End If
            'Next


            'If vantxt <> "" Then
            '    vantxt = vantxt.Substring(0, vantxt.Length - 1)
            'End If
            'If VanStr = "" Then
            '    VanStr = "0"
            'End If
            'If vancnt = ddlVan.Items.Count Then
            '    lbl_van.Text = "All"
            'Else
            '    lbl_van.Text = vantxt
            'End If


            'Me.hfSE.Value = VanStr
            'Me.hfOrg.Value = ddlOrganization.SelectedValue
            'Me.hfSMonth.Value = txtFromDate.SelectedDate
            'Me.hfEMonth.Value = txtToDate.SelectedDate

            'lbl_org.Text = ddlOrganization.SelectedItem.Text

            'lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            'lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")

            'Args.Visible = True
            'rpt.Visible = True
            'gvRep.Rebind()
            'Dim VanActCnt As Integer = 8
            'If gvRep.Items.Count > 0 Then

            '    Dim lblVanCnt As Label = DirectCast(gvRep.Items(0).FindControl("lblTotVans"), Label)
            '    VanActCnt = lblVanCnt.Text.Trim()
            'End If
            ''If VanActCnt > 4 Then
            ''    Chartwrapper.Style.Add("height", (CInt(VanActCnt) * 70).ToString & "px")
            ''Else
            ''    Chartwrapper.Style.Add("height", "300px")
            ''End If
            'AgencyTab.Tabs(0).Selected = True
            'Me.RadMultiPage21.SelectedIndex = 0
            'BindChart()

        End If
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub
    Private Sub BindChart()
        ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
    End Sub
    Private Sub BindChart1()
        ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart1();", True)
    End Sub
    Protected Sub gvRep_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles gvRep.ItemDataBound
        

        If TypeOf e.Item Is GridGroupHeaderItem Then
            Dim item As GridGroupHeaderItem = DirectCast(e.Item, GridGroupHeaderItem)
            Dim groupDataRow As DataRowView = DirectCast(e.Item.DataItem, DataRowView)
            item.DataCell.Text = groupDataRow("Van").ToString()

        End If
    End Sub

    'Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender
    '    For Each column As GridColumn In gvRep.MasterTableView.Columns
    '        If column.UniqueName = "Value" Then
    '            Dim col As GridBoundColumn
    '            col = (CType(column, GridBoundColumn))
    '            col.DataType = System.Type.GetType("System.Double")
    '            col.DataFormatString = "{0:" & hfDecimal.Value & "}"
    '        End If
    '    Next
    'End Sub
   
    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        ddlVan.ClearCheckedItems()
        ddlVan.Items.Clear()
        LoadOrgDetails()
        txtFromDate.SelectedDate = DateAdd("m", 0, DateSerial(Year(Today), Month(Today), 1))
        txtToDate.SelectedDate = Now
        Args.Visible = False
        rpt.Visible = False
        gvRep.Visible = False
    End Sub
End Class