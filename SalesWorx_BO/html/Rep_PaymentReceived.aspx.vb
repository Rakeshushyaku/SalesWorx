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
Partial Public Class Rep_PaymentReceived
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "PaymentReceivedSummary"

    Private Const PageID As String = "P106"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single

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
            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            If Not HasPermission Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If
            ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
            Try
                ObjCommon = New SalesWorx.BO.Common.Common()
                Dim CountryTbl As DataTable = Nothing
                Dim orgTbl As DataTable = Nothing

                CountryTbl = ObjCommon.GetCountry(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)
                ddlCountry.DataSource = CountryTbl
                ddlCountry.DataBind()


                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)

                Dim s() As String = Nothing
                Dim Currency As String = Nothing
                Dim DecimalDigits As String = "2"
                Dim country As String = Nothing
                If CountryTbl.Rows.Count = 1 Then

                    ddlCountry.SelectedIndex = 0
                    dvCountry.Visible = False

                    s = ddlCountry.SelectedValue.Split("$")

                    If s.Length > 0 Then
                        country = s(0).ToString()
                        Currency = s(1).ToString()
                        DecimalDigits = s(2).ToString()
                    End If


                    ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
                    ddlOrganization.DataBind()


                ElseIf CountryTbl.Rows.Count > 1 Then
                    ddlCountry.SelectedIndex = 0
                    dvCountry.Visible = True


                    s = ddlCountry.SelectedValue.Split("$")

                    If s.Length > 0 Then
                        country = s(0).ToString()
                        Currency = s(1).ToString()
                        DecimalDigits = s(2).ToString()
                    End If

                    ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
                    ddlOrganization.DataBind()

                End If

                Dim OrgStr As String = Nothing
                For Each item As RadComboBoxItem In ddlOrganization.Items
                    item.Checked = True
                    If item.Checked Then

                        OrgStr = OrgStr & "," & item.Value

                    End If
                Next

                If CType(Session.Item("CONTROL_PARAMS"), ControlParams).CLIENT_CODE = "GYMA" Then
                    gvRep.Columns(7).HeaderText = "Cheque/CC Details"
                Else
                    For Each r As RadComboBoxItem In ddlPay.Items
                        If r.Value = "CC" Then
                            r.Remove()
                            Exit For
                        End If
                    Next
                    gvRep.Columns(7).HeaderText = "Cheque Details"
                End If

                LoadOrgDetails()
                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                txtToDate.SelectedDate = Now
                 


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
    Sub Export(format As String)





        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

        Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems

        Dim van As String = ""
        For Each li As RadComboBoxItem In collection
            van = van & li.Value & ","
        Next

        If van = "" Then
            van = "0"
        End If

        Dim Orgtxt As String = ""
        Dim Org As String = ""
        Dim Orgcollection As IList(Of RadComboBoxItem) = ddlOrganization.CheckedItems
        For Each li As RadComboBoxItem In Orgcollection
            Org = Org & li.Value & ","
            Orgtxt = Orgtxt & li.Text & ","
        Next
        If Orgtxt <> "" Then
            Orgtxt = Orgtxt.Substring(0, Orgtxt.Length - 1)
        End If
        If Org = "" Then
            Org = "0"
        End If


        Dim collectionPay As IList(Of RadComboBoxItem) = ddlPay.CheckedItems

        Dim Pay As String = ""
        For Each li As RadComboBoxItem In collectionPay
            If CType(Session.Item("CONTROL_PARAMS"), ControlParams).CLIENT_CODE = "GYMA" And li.Value = "CC" Then
                Pay = Pay & li.Value & ","
            Else
                Pay = Pay & li.Value & ","
            End If
        Next

        If Pay = "" Then
            Pay = "ALL"
        Else
            Pay = Pay.Substring(0, Len(Pay) - 1)
        End If

        Dim USRID As New ReportParameter
        USRID = New ReportParameter("Uid", CType(Session.Item("USER_ACCESS"), UserAccess).UserID)


        Dim SalesRepID As New ReportParameter
        SalesRepID = New ReportParameter("SID", van)

        Dim FDate As New ReportParameter
        FDate = New ReportParameter("FromDate", CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"))

        Dim TDate As New ReportParameter
        TDate = New ReportParameter("ToDate", CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))


        Dim OID As New ReportParameter
        OID = New ReportParameter("OID", Org)

        Dim PayType As New ReportParameter
        PayType = New ReportParameter("PaymentType", Pay)

        Dim bytes As Byte()


        rview.ServerReport.SetParameters(New ReportParameter() {USRID, OID, SalesRepID, FDate, TDate, PayType})

        Dim mimeType As String = Nothing
        Dim encoding As String = Nothing
        Dim extension As String = Nothing
        Dim deviceInfo As String = "<DeviceInfo></DeviceInfo>"
        Dim streamids As String() = Nothing
        Dim warnings As Microsoft.Reporting.WebForms.Warning() = Nothing

        bytes = rview.ServerReport.Render(format, deviceInfo, mimeType, encoding, extension, streamids, warnings)


        Response.Clear()
        If format = "PDF" Then
            Response.ContentType = "application/pdf"
            Response.AddHeader("Content-disposition", "attachment;filename=PaymentSummary.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=PaymentSummary.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()
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
            BindReport()
        Else
            Args.Visible = False
            summary.InnerHtml = ""
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

                Dim vantxt As String = ""
                Dim van As String = ""
                For Each li As RadComboBoxItem In collection
                    van = van & li.Value & ","
                    vantxt = vantxt & li.Text & ","
                Next
                If vantxt <> "" Then
                    vantxt = vantxt.Substring(0, vantxt.Length - 1)
                End If
                If van = "" Then
                    van = "0"
                End If

                Dim collectionPay As IList(Of RadComboBoxItem) = ddlPay.CheckedItems

                Dim Pay As String = ""
                Dim Paytxt As String = ""
                For Each li As RadComboBoxItem In collectionPay
                    Pay = Pay & li.Value & ","
                    Paytxt = Paytxt & li.Value & ","
                Next
                If Paytxt <> "" Then
                    Paytxt = Paytxt.Substring(0, Paytxt.Length - 1)
                Else
                    Paytxt = "All"
                End If
                If Pay = "" Then
                    Pay = "ALL"
                Else
                    Pay = Pay.Substring(0, Len(Pay) - 1)
                End If


                If Pay = "CASH" Then
                    gvRep.Columns(7).Visible = False
                    gvRep.MasterTableView.PageSize = 10
                Else
                    gvRep.Columns(7).Visible = True
                    gvRep.MasterTableView.PageSize = 6
                End If

                Dim Orgtxt As String = ""
                Dim Org As String = ""
                Dim Orgcollection As IList(Of RadComboBoxItem) = ddlOrganization.CheckedItems
                For Each li As RadComboBoxItem In Orgcollection
                    Org = Org & li.Value & ","
                    Orgtxt = Orgtxt & li.Text & ","
                Next
                If Orgtxt <> "" Then
                    Orgtxt = Orgtxt.Substring(0, Orgtxt.Length - 1)
                End If
                If Org = "" Then
                    Org = "0"
                End If

                lbl_org.Text = Orgtxt

                If van = "0" Then
                    lbl_van.Text = "All"
                Else
                    lbl_van.Text = vantxt
                End If
                lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
                lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")
                lbl_payType.Text = Paytxt
                Args.Visible = True

                dt = ObjReport.GetPaymentReceivedSummary(Err_No, Err_Desc, Pay, Org, van, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"), objUserAccess.UserID)
                gvRep.DataSource = dt
                gvRep.DataBind()

                

                Dim dtcurrency As DataTable
                dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.CheckedItems(0).Value)
                Dim Currency As String = ""
                If dtcurrency.Rows.Count > 0 Then
                    Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
                End If

                Dim query = (From UserEntry In dt _
                            Group UserEntry By key = UserEntry.Field(Of String)("Collection_Type") Into Group _
                            Select PayMode = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("Amount"))).ToList

                'Dim DtSummary As New DataTable
                'DtSummary.Columns.Add("Paymode")
                'DtSummary.Columns.Add("Amount")

                Dim StrSummary As String = ""
                Dim StrSummaryR As String = ""
                Dim i As Integer = 0
                For Each x In query
                    'Dim dr As DataRow
                    'dr = DtSummary.NewRow
                    'dr(0) = "Total " & x.PayMode & Currency
                    'dr(1) = Format(x.Total, "#,##0.00")
                    'DtSummary.Rows.Add(dr)

                    If x.PayMode.ToString() = "CASH" Then

                        StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>" & "Total " & x.PayMode.ToString & Currency & "<div class='text-primary'>" & Format(x.Total, lblDecimal.Text) & "</div></div></div>"
                    End If

                    i = i + 1
                Next

                For Each x In query
                    'Dim dr As DataRow
                    'dr = DtSummary.NewRow
                    'dr(0) = "Total " & x.PayMode & Currency
                    'dr(1) = Format(x.Total, "#,##0.00")
                    'DtSummary.Rows.Add(dr)

                    If x.PayMode.ToString() = "CURR-CHQ" Then

                        StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>" & "Total " & x.PayMode.ToString & Currency & "<div class='text-primary'>" & Format(x.Total, lblDecimal.Text) & "</div></div></div>"
                    End If

                    i = i + 1
                Next


                For Each x In query
                    'Dim dr As DataRow
                    'dr = DtSummary.NewRow
                    'dr(0) = "Total " & x.PayMode & Currency
                    'dr(1) = Format(x.Total, "#,##0.00")
                    'DtSummary.Rows.Add(dr)

                    If x.PayMode.ToString() = "PDC" Then

                        StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>" & "Total " & x.PayMode.ToString & Currency & "<div class='text-primary'>" & Format(x.Total, lblDecimal.Text) & "</div></div></div>"
                    End If

                    i = i + 1
                Next


                For Each x In query
                    'Dim dr As DataRow
                    'dr = DtSummary.NewRow
                    'dr(0) = "Total " & x.PayMode & Currency
                    'dr(1) = Format(x.Total, "#,##0.00")
                    'DtSummary.Rows.Add(dr)
                    If CType(Session.Item("CONTROL_PARAMS"), ControlParams).CLIENT_CODE = "GYMA" And x.PayMode = "CC" Then
                        StrSummary = StrSummary & "<div class='col-sm-6 col-md-4 col-lg-3'><div class='widgetblk'>" & "Total Credit Card " & x.PayMode & Currency & "<div class='text-primary'>" & Format(x.Total, lblDecimal.Text) & "</div></div></div>"
                    End If


                    i = i + 1
                Next


                Dim NoofPDC = From Coll In dt _
                        Where Coll.Field(Of String)("Collection_Type") = "PDC" _
                        Select Coll.Field(Of String)("Collection_Ref_No") Distinct

                StrSummary = StrSummary & "<div class='col-sm-6 col-md-4 col-lg-3'><div class='widgetblk'>" & "No. Of PDC <div class='text-primary'>" & NoofPDC.Count & "</div></div></div>"
                summary.InnerHtml = StrSummary
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
        If ddlOrganization.CheckedItems Is Nothing Then
            MessageBoxValidation("Select an Organisation", "Validation")
            SetFocus(ddlOrganization)
            Return bretval

        Else
            If ddlOrganization.CheckedItems.Count <= 0 Then
                MessageBoxValidation("Select an Organisation", "Validation")
                SetFocus(ddlOrganization)
                Return bretval
            End If
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
        bretval = True
        Return bretval
    End Function


    
    Sub LoadOrgDetails()
        'If Not (ddlOrganization.SelectedItem.Value = "0") Then

        '    Dim objUserAccess As UserAccess
        '    objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        '    ObjCommon = New SalesWorx.BO.Common.Common()
        '    ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
        '    ddlVan.DataBind()

        '    For Each itm As RadComboBoxItem In ddlVan.Items
        '        itm.Checked = True
        '    Next

        '    Dim dtcurrency As DataTable
        '    Dim ObjReport As New SalesWorx.BO.Common.Reports
        '    dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

        '    Dim Currency As String = ""
        '    If dtcurrency.Rows.Count > 0 Then
        '        Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
        '        lblDecimal.Text = "N" & dtcurrency.Rows(0)("Decimal_Digits")
        '    End If

        'End If
        If Not ddlOrganization.CheckedItems Is Nothing Then
            If ddlOrganization.CheckedItems.Count > 0 Then
                Dim objUserAccess As UserAccess
                objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
                Dim ObjRep As SalesWorx.BO.Common.Reports = New SalesWorx.BO.Common.Reports()

                Dim OrgStr As String = ""
                For Each li As RadComboBoxItem In ddlOrganization.CheckedItems
                    OrgStr = OrgStr & li.Value & ","
                Next
                ddlVan.DataSource = ObjRep.GetAllOrgVan(Err_No, Err_Desc, OrgStr, objUserAccess.UserID.ToString())
                ddlVan.DataBind()

                For Each itm As RadComboBoxItem In ddlVan.Items
                    itm.Checked = True
                Next

                Dim dtcurrency As DataTable
                Dim ObjReport As New SalesWorx.BO.Common.Reports
                dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.CheckedItems(0).Value)

                Dim Currency As String = ""
                If dtcurrency.Rows.Count > 0 Then
                    Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
                    lblDecimal.Text = "N" & dtcurrency.Rows(0)("Decimal_Digits")
                End If
            Else
                ddlVan.Items.Clear()
            End If
        Else
            ddlVan.Items.Clear()
        End If

    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub

    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender
        For Each column As GridColumn In gvRep.MasterTableView.Columns
            If column.UniqueName = "Amount" Then
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
        BindReport()
    End Sub
    Private Sub dgv_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged

        BindReport()
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
    Private Sub ddlOrganization_CheckAllCheck(sender As Object, e As RadComboBoxCheckAllCheckEventArgs) Handles ddlOrganization.CheckAllCheck
        LoadOrgDetails()
    End Sub

    Private Sub ddlOrganization_ItemChecked(sender As Object, e As RadComboBoxItemEventArgs) Handles ddlOrganization.ItemChecked
        LoadOrgDetails()
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ObjCommon = New SalesWorx.BO.Common.Common()
        Dim CountryTbl As DataTable = Nothing
        Dim orgTbl As DataTable = Nothing

        CountryTbl = ObjCommon.GetCountry(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)
        ddlCountry.DataSource = CountryTbl
        ddlCountry.DataBind()


        Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
        orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)

        Dim s() As String = Nothing
        Dim Currency As String = Nothing
        Dim DecimalDigits As String = "2"
        Dim country As String = Nothing
        If CountryTbl.Rows.Count = 1 Then

            ddlCountry.SelectedIndex = 0
            dvCountry.Visible = False

            s = ddlCountry.SelectedValue.Split("$")

            If s.Length > 0 Then
                country = s(0).ToString()
                Currency = s(1).ToString()
                DecimalDigits = s(2).ToString()
            End If


            ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
            ddlOrganization.DataBind()


        ElseIf CountryTbl.Rows.Count > 1 Then
            ddlCountry.SelectedIndex = 0
            dvCountry.Visible = True


            s = ddlCountry.SelectedValue.Split("$")

            If s.Length > 0 Then
                country = s(0).ToString()
                Currency = s(1).ToString()
                DecimalDigits = s(2).ToString()
            End If

            ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
            ddlOrganization.DataBind()

        End If

        Dim OrgStr As String = Nothing
        For Each item As RadComboBoxItem In ddlOrganization.Items
            item.Checked = True
            If item.Checked Then

                OrgStr = OrgStr & "," & item.Value

            End If
        Next


        LoadOrgDetails()
        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Now

        ddlPay.ClearCheckedItems()

        Args.Visible = False
        summary.InnerHtml = ""
        gvRep.Visible = False
    End Sub
    Private Sub ddlCountry_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlCountry.SelectedIndexChanged
        LoadOrgs()
        LoadOrgDetails()
    End Sub
    Sub LoadOrgs()
        Dim orgTbl As DataTable = Nothing

        ObjCommon = New SalesWorx.BO.Common.Common()


        Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)



        orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)

        Dim s() As String = Nothing
        Dim Currency As String = Nothing
        Dim DecimalDigits As String = "2"
        Dim country As String = Nothing
        s = ddlCountry.SelectedValue.Split("$")

        If s.Length > 0 Then
            country = s(0).ToString()
            Currency = s(1).ToString()
            DecimalDigits = s(2).ToString()
        End If


        ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
        ddlOrganization.DataBind()



        Dim OrgStr As String = Nothing
        For Each item As RadComboBoxItem In ddlOrganization.Items
            item.Checked = True
            If item.Checked Then

                OrgStr = OrgStr & "," & item.Value

            End If
        Next

    End Sub
    Private Sub BtnExportBiffExcel_Click(sender As Object, e As EventArgs) Handles BtnExportBiffExcel.Click
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        Dim ObjReport As New SalesWorx.BO.Common.Reports

        Dim dt As New DataTable
        Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems


        Dim van As String = ""
        For Each li As RadComboBoxItem In collection
            van = van & li.Value & ","

        Next
        
        If van = "" Then
            van = "0"
        End If

        Dim collectionPay As IList(Of RadComboBoxItem) = ddlPay.CheckedItems

        Dim Pay As String = ""

        For Each li As RadComboBoxItem In collectionPay
            Pay = Pay & li.Value & ","

        Next
        
        If Pay = "" Then
            Pay = "ALL"
        Else
            Pay = Pay.Substring(0, Len(Pay) - 1)
        End If




        Dim Org As String = ""
        Dim Orgcollection As IList(Of RadComboBoxItem) = ddlOrganization.CheckedItems
        For Each li As RadComboBoxItem In Orgcollection
            Org = Org & li.Value & ","

        Next
        
        If Org = "" Then
            Org = "0"
        End If

        Dim tblData As New DataTable

        dt = ObjReport.GetPaymentReceivedSummary(Err_No, Err_Desc, Pay, Org, van, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"), objUserAccess.UserID)

        tblData = dt.DefaultView.ToTable(False, "Collected_On", "Collection_Line_Ref", "Customer_No", "CustomerName", "Collection_Type", "Invoice_No", "Amount")

        If tblData.Rows.Count > 0 Then

            Using package As New ExcelPackage()
                ' add a new worksheet to the empty workbook
                Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
                Worksheet.Cells("A1").LoadFromDataTable(tblData, True)
                Worksheet.Column(1).Style.Numberformat.Format = "dd-MMM-yyyy"
                Worksheet.Cells.AutoFitColumns()

                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename= PaymentReceived.xlsx")

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
    
End Class