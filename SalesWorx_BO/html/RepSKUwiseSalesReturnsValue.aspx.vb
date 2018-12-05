Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Partial Public Class RepSKUwiseSalesReturnsValue
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "SKUWiseSalesReturnValue"

    Private Const PageID As String = "P226"
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
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "-1"))


                Dim objUserAccess As UserAccess
                objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

                HUID.Value = CType(Session.Item("USER_ACCESS"), UserAccess).UserID


                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1


                    ddlAgency.DataSource = ObjCommon.GetAgencyList(Err_No, Err_Desc, ddlOrganization.SelectedValue)
                    ddlAgency.DataBind()
                    ddlAgency.Items.Insert(0, New RadComboBoxItem("Select Agency", "0"))


                    ddSKU.DataSource = Nothing
                    ddSKU.Items.Clear()
                    ddSKU.DataSource = ObjCommon.GetProductsByOrg_Agency(Err_No, Err_Desc, ddlOrganization.SelectedValue, 0)
                    ddSKU.DataTextField = "Description"
                    ddSKU.DataValueField = "Inventory_Item_ID"
                    ddSKU.DataBind()

                    Dim dtcurrency As DataTable
                    Dim ObjReport As New SalesWorx.BO.Common.Reports
                    dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

                    Dim Currency As String = ""
                    If dtcurrency.Rows.Count > 0 Then
                        lblDecimal.Text = "N" & dtcurrency.Rows(0)("Decimal_Digits")
                        Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
                    End If
                    Hcurrency.Value = Currency
                End If

                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                txtToDate.SelectedDate = Now()


                ddlAgency.DataSource = ObjCommon.GetAgencyList(Err_No, Err_Desc, ddlOrganization.SelectedValue)
                ddlAgency.DataBind()
                ddlAgency.Items.Insert(0, New RadComboBoxItem("Select Agency", "0"))



                ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
                ddlVan.DataTextField = "SalesRep_Name"
                ddlVan.DataValueField = "SalesRep_ID"
                ddlVan.DataBind()


                ddlCustomer.DataSource = ObjCommon.GetOutlet(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
                ddlCustomer.DataTextField = "Outlet"
                ddlCustomer.DataValueField = "CustomerID"
                ddlCustomer.DataBind()


                Dim OrgID As New ReportParameter
                Dim OrgName As New ReportParameter
                OrgID = New ReportParameter("OrgID", ddlOrganization.SelectedItem.Value)
                OrgName = New ReportParameter("OrgName", ddlOrganization.SelectedItem.Text)

                ''  InitReportViewer()

                If Not Err_Desc Is Nothing Then
                    log.Error(Err_Desc)
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
                End If


            Catch ex As Exception
                Err_No = "74266"
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
    'Private Sub InitReportViewer()
    '    Try

    '        Dim FDate As New ReportParameter
    '        If txtFromDate.Text.Trim() IsNot String.Empty Then
    '            FDate = New ReportParameter("FromDate", txtFromDate.Text.Trim())
    '        Else
    '            FDate = New ReportParameter("FromDate", Now.ToString("dd-MMM-yyyy"))
    '        End If


    '        Dim TDate As New ReportParameter
    '        If txtToDate.Text.Trim() IsNot String.Empty Then
    '            TDate = New ReportParameter("ToDate", txtToDate.Text)
    '        Else
    '            TDate = New ReportParameter("ToDate", Now.ToString("dd-MMM-yyyy"))
    '        End If


    '        Dim SiteID As New ReportParameter

    '        Dim Outlet As New ReportParameter
    '        Dim SKU As New ReportParameter

    '        Dim InvID As New ReportParameter
    '        Dim Invids As String = Nothing
    '        Dim collection As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddSKU.CheckedItems


    '        For Each li As Telerik.Web.UI.RadComboBoxItem In collection
    '            If String.IsNullOrEmpty(Invids) Then
    '                Invids = li.Value
    '            Else
    '                Invids = Invids & "|" & li.Value
    '            End If
    '        Next
    '        If String.IsNullOrEmpty(Invids) Then
    '            InvID = New ReportParameter("InID", "-1")
    '        Else
    '            InvID = New ReportParameter("InID", Invids & "|")
    '        End If


    '        Dim Agency As New ReportParameter
    '        Agency = New ReportParameter("Agency", ddlAgency.SelectedItem.Value)

    '        Dim OrgID As New ReportParameter
    '        Dim OrgName As New ReportParameter
    '        OrgID = New ReportParameter("OrgID", ddlOrganization.SelectedItem.Value)
    '        OrgName = New ReportParameter("OrgName", ddlOrganization.SelectedItem.Text)


    '        Dim Cid As String = Nothing
    '        Dim collectionCid As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddlCustomer.CheckedItems


    '        For Each li As Telerik.Web.UI.RadComboBoxItem In collectionCid
    '            Dim Arr As Array = li.Value.Split("$")
    '            If String.IsNullOrEmpty(Cid) Then
    '                Cid = Arr(0) & "~" & Arr(1)
    '            Else
    '                Cid = Cid & "|" & Arr(0) & "~" & Arr(1)
    '            End If
    '        Next

    '        If String.IsNullOrEmpty(Cid) Then
    '            SiteID = New ReportParameter("SID", "-1")
    '        Else
    '            SiteID = New ReportParameter("SID", Cid & "|")
    '        End If

    '        Dim Fsrids As String = Nothing
    '        Dim collectionFsr As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddlVan.CheckedItems


    '        For Each li As Telerik.Web.UI.RadComboBoxItem In collectionFsr
    '            If String.IsNullOrEmpty(Fsrids) Then
    '                Fsrids = li.Value
    '            Else
    '                Fsrids = Fsrids & "|" & li.Value
    '            End If
    '        Next


    '        Dim RepID As New ReportParameter
    '        If String.IsNullOrEmpty(Fsrids) Then
    '            RepID = New ReportParameter("FSRID", "-1")
    '        Else
    '            RepID = New ReportParameter("FSRID", Fsrids & "|")
    '        End If

    '        With RVMain
    '            .Reset()
    '            .ShowParameterPrompts = False
    '            .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
    '            .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
    '            .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
    '            .ServerReport.SetParameters(New ReportParameter() {FDate, TDate, InvID, OrgID, OrgName, Agency, SiteID, RepID})
    '            .ServerReport.Refresh()

    '        End With


    '    Catch Ex As Exception
    '        log.Error(GetExceptionInfo(Ex))
    '    End Try
    'End Sub
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
    Sub Export(format As String)
        Try

       

        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
        Dim FDate As New ReportParameter
        FDate = New ReportParameter("FromDate", CDate(txtFromDate.SelectedDate).ToString("MM-dd-yyyy"))
        Dim TDate As New ReportParameter
        TDate = New ReportParameter("ToDate", CDate(txtToDate.SelectedDate).ToString("MM-dd-yyyy"))


        Dim SiteID As New ReportParameter

        Dim Outlet As New ReportParameter
        Dim SKU As New ReportParameter

        Dim InvID As New ReportParameter
        Dim Invids As String = Nothing
        Dim collection As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddSKU.CheckedItems


        For Each li As Telerik.Web.UI.RadComboBoxItem In collection
            If String.IsNullOrEmpty(Invids) Then
                Invids = li.Value
            Else
                Invids = Invids & "|" & li.Value
            End If
        Next
        If String.IsNullOrEmpty(Invids) Then
            InvID = New ReportParameter("InID", "-1")
        Else
            InvID = New ReportParameter("InID", Invids & "|")
        End If


        Dim Agency As New ReportParameter
        If ddlAgency.SelectedIndex >= 0 Then
            Agency = New ReportParameter("Agency", ddlAgency.SelectedItem.Value)
        Else
            Agency = New ReportParameter("Agency", "0")

        End If


        Dim OrgID As New ReportParameter
        Dim OrgName As New ReportParameter
        OrgID = New ReportParameter("OrgID", ddlOrganization.SelectedItem.Value)
        OrgName = New ReportParameter("OrgName", ddlOrganization.SelectedItem.Text)
        Dim UID As New ReportParameter
        UID = New ReportParameter("UID", CStr(HUID.Value))


        Dim Cid As String = Nothing
        Dim collectionCid As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddlCustomer.CheckedItems


        For Each li As Telerik.Web.UI.RadComboBoxItem In collectionCid
            Dim Arr As Array = li.Value.Split("$")
            If String.IsNullOrEmpty(Cid) Then
                Cid = Arr(0) & "~" & Arr(1)
            Else
                Cid = Cid & "|" & Arr(0) & "~" & Arr(1)
            End If
        Next

        If String.IsNullOrEmpty(Cid) Then
            SiteID = New ReportParameter("SID", "-1")
        Else
            SiteID = New ReportParameter("SID", Cid & "|")
        End If

        Dim Fsrids As String = Nothing
        Dim collectionFsr As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddlVan.CheckedItems

        For Each li As RadComboBoxItem In collectionFsr
            Fsrids = Fsrids & li.Value & ","
        Next


        Dim RepID As New ReportParameter
        If String.IsNullOrEmpty(Fsrids) Then
            RepID = New ReportParameter("FSRID", "-1")
        Else
            RepID = New ReportParameter("FSRID", Fsrids)
        End If



        rview.ServerReport.SetParameters(New ReportParameter() {FDate, TDate, InvID, OrgID, OrgName, Agency, SiteID, RepID, UID})

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
            Response.AddHeader("Content-disposition", "attachment;filename=SKUwiseSalesReturnQtyValue.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=SKUwiseSalesReturnQtyValue.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
            Response.Close()


        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub


    Private Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrganization.SelectedIndexChanged
        Try
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

            ObjCommon = New SalesWorx.BO.Common.Common()
            ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
            ddlVan.DataTextField = "SalesRep_Name"
            ddlVan.DataValueField = "SalesRep_ID"
            ddlVan.DataBind()


            ddlCustomer.DataSource = ObjCommon.GetOutlet(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
            ddlCustomer.DataTextField = "Outlet"
            ddlCustomer.DataValueField = "CustomerID"
            ddlCustomer.DataBind()


            ddlAgency.DataSource = ObjCommon.GetAgencyList(Err_No, Err_Desc, ddlOrganization.SelectedValue)
            ddlAgency.DataBind()
            ddlAgency.Items.Insert(0, New RadComboBoxItem("Select Agency", "0"))

            ddSKU.DataSource = Nothing
            ddSKU.Items.Clear()
            ddSKU.DataSource = ObjCommon.GetProductsByOrg_Agency(Err_No, Err_Desc, ddlOrganization.SelectedValue, 0)
            ddSKU.DataTextField = "Description"
            ddSKU.DataValueField = "Inventory_Item_ID"
            ddSKU.DataBind()

            gvRep.Visible = False
            Args.Visible = False
            divCurrency.Visible = False
            summary.InnerHtml = ""
            Dim dtcurrency As DataTable
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

            Dim Currency As String = ""
            If dtcurrency.Rows.Count > 0 Then
                Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
            End If
            Hcurrency.Value = Currency
        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            ObjCommon = Nothing
        End Try
    End Sub

    Private Sub ddlAgency_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAgency.SelectedIndexChanged
        ObjCommon = New SalesWorx.BO.Common.Common
        ddSKU.DataSource = ObjCommon.GetProductsByOrg_Agency(Err_No, Err_Desc, ddlOrganization.SelectedValue, ddlAgency.SelectedItem.Value)
        ddSKU.DataTextField = "Description"
        ddSKU.DataValueField = "Inventory_Item_ID"
        ddSKU.DataBind()


    End Sub

    Private Sub SearchBtn_Click(sender As Object, e As EventArgs) Handles SearchBtn.Click
        Dim SHOW_UOM_MSG_BO_REPORTS As String = "N"
        Dim dt_app As New DataTable
        dt_app = (New SalesWorx.BO.Common.Common).GetAppControl(Err_No, Err_Desc, "SHOW_UOM_MSG_BO_REPORTS")
        If dt_app.Rows.Count > 0 Then
            SHOW_UOM_MSG_BO_REPORTS = dt_app.Rows(0)("Control_Value").ToString().ToUpper()
            If SHOW_UOM_MSG_BO_REPORTS = "Y" Then
                lblmsgUOM.Text = "All the quantities displayed in this report are in Stock UOM"
            Else
                lblmsgUOM.Text = ""

            End If
        End If
        If ValidateInputs() Then
            gvRep.Visible = True
            divCurrency.Visible = True
            BindReport()
        Else
            gvRep.Visible = False
            Args.Visible = False
            divCurrency.Visible = False
            summary.InnerHtml = ""
        End If
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Sub BindReport()
        Try


            Dim SearchQuery As String = ""

            rpbFilter.Items(0).Expanded = False
            lbl_Currency.Text = Hcurrency.Value
            Args.Visible = True

            Dim ObjReport As New SalesWorx.BO.Common.Reports

            Dim SiteID As String


            Dim InvID As String
            Dim Invids As String = Nothing
            Dim collection As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddSKU.CheckedItems


            For Each li As Telerik.Web.UI.RadComboBoxItem In collection
                If String.IsNullOrEmpty(Invids) Then
                    Invids = li.Value
                Else
                    Invids = Invids & "|" & li.Value
                End If
            Next
            If String.IsNullOrEmpty(Invids) Then
                InvID = "-1"
            Else
                InvID = Invids & "|"
            End If

            If collection.Count = 1 Then
                lbl_SKU.Text = collection(0).Text
            ElseIf collection.Count > 1 Then
                lbl_SKU.Text = "Multiple"
            Else
                lbl_SKU.Text = "All"
            End If

            Dim Agency As String = "0"
            If ddlAgency.SelectedIndex >= 0 Then
                Agency = ddlAgency.SelectedItem.Value
                lbl_Principle.Text = ddlAgency.SelectedItem.Text
            Else
                lbl_Principle.Text = "All"
            End If


            Dim OrgID As String
            Dim OrgName As String
            OrgID = ddlOrganization.SelectedItem.Value
            OrgName = ddlOrganization.SelectedItem.Text


            Dim Cid As String = Nothing
            Dim collectionCid As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddlCustomer.CheckedItems


            For Each li As Telerik.Web.UI.RadComboBoxItem In collectionCid
                Dim Arr As Array = li.Value.Split("$")
                If String.IsNullOrEmpty(Cid) Then
                    Cid = Arr(0) & "~" & Arr(1)
                Else
                    Cid = Cid & "|" & Arr(0) & "~" & Arr(1)
                End If
            Next

            If String.IsNullOrEmpty(Cid) Then
                SiteID = "-1"
            Else
                SiteID = Cid & "|"
            End If

            If collectionCid.Count = 1 Then
                lbl_Outlet.Text = collectionCid(0).Text
            ElseIf collectionCid.Count > 1 Then
                lbl_Outlet.Text = "Multiple"
            Else
                lbl_Outlet.Text = "All"
            End If

            Dim Fsrids As String = Nothing
            Dim collectionFsr As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddlVan.CheckedItems

            Dim TFSRID As String
            For Each li As Telerik.Web.UI.RadComboBoxItem In collectionFsr
                If String.IsNullOrEmpty(Fsrids) Then
                    Fsrids = li.Value
                    TFSRID = li.Value
                Else
                    Fsrids = Fsrids & "," & li.Value
                    TFSRID = Fsrids & "|" & li.Value & "," & li.Value
                End If
            Next


            Dim RepID As String
            Dim TRepID As String
            If String.IsNullOrEmpty(Fsrids) Then
                RepID = "-1"

            Else
                RepID = Fsrids & ","

            End If
            If String.IsNullOrEmpty(TFSRID) Then
                TRepID = "-1"

            Else
                TRepID = TFSRID & "|"

            End If

            If collectionFsr.Count = 1 Then
                lbl_van.Text = collectionFsr(0).Text
            ElseIf collectionFsr.Count > 1 Then
                lbl_van.Text = "Multiple"
            Else
                lbl_van.Text = "All"
            End If

            Dim dtcurrency As DataTable

            dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

            Dim Currency As String = ""
            If dtcurrency.Rows.Count > 0 Then
                lblDecimal.Text = "N" & dtcurrency.Rows(0)("Decimal_Digits")
                Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
            End If
            Hcurrency.Value = Currency

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

            HORGID.Value = ddlOrganization.SelectedItem.Value
            HDate.Value = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            HToDate.Value = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")
            HCID.Value = SiteID
            HFSRID.Value = TRepID


            lbl_org.Text = ddlOrganization.SelectedItem.Text
            lbl_From.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")
            Args.Visible = True
            log.Error(ddlOrganization.SelectedItem.Value & " , " & CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy") & " , " & CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy") & " , " & RepID & " , " & Agency & " , " & SiteID & " , " & InvID & " , " & HUID.Value)


            Dim dt As New DataTable
            dt = ObjReport.GetSKUWiseSalesReturns(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"), RepID, Agency, SiteID, InvID, HUID.Value)
            gvRep.DataSource = dt
            gvRep.DataBind()

            log.Error("Rows : " & dt.Rows.Count)

            Dim StrSummary As String = ""
            Dim sum
            If dt.Rows.Count > 0 Then
                sum = dt.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("SalesValue")))
            End If


            StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>Total Sales " & Currency & "<div class='text-primary'>" & Format(sum, lblDecimal.Text) & "</div></div></div>"
            Dim sumR
            If dt.Rows.Count > 0 Then
                sumR = dt.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("ReturnsValue")))
            End If


            StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>Total Returns " & Currency & "<div class='text-primary'>" & Format(sumR, lblDecimal.Text) & "</div></div></div>"


            summary.InnerHtml = StrSummary
        Catch ex As Exception
            log.Error(ex.ToString)
        End Try
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
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If ddlOrganization.SelectedIndex > 0 Then
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
        Else
            MessageBoxValidation("Please select the Organisation", "Validation")
            Return bretval
        End If
    End Function

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        divCurrency.Visible = False
        ddlOrganization.ClearSelection()
        ddlVan.ClearCheckedItems()
        ddlVan.Items.Clear()

        ddlCustomer.ClearCheckedItems()
        ddlCustomer.Items.Clear()

        ddSKU.ClearCheckedItems()
        ddSKU.Items.Clear()

        ddlAgency.ClearSelection()
        ddSKU.Items.Clear()

        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        Args.Visible = False
        gvRep.Visible = False
        summary.InnerHtml = ""
        lblmsgUOM.Text = ""
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

        ObjCommon = New SalesWorx.BO.Common.Common()
        ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
        ddlVan.DataTextField = "SalesRep_Name"
        ddlVan.DataValueField = "SalesRep_ID"
        ddlVan.DataBind()
        objUserAccess = Nothing

        ddlCustomer.DataSource = ObjCommon.GetOutlet(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
        ddlCustomer.DataTextField = "Outlet"
        ddlCustomer.DataValueField = "CustomerID"
        ddlCustomer.DataBind()


        ddlAgency.DataSource = ObjCommon.GetAgencyList(Err_No, Err_Desc, ddlOrganization.SelectedValue)
        ddlAgency.DataBind()
        ddlAgency.Items.Insert(0, New RadComboBoxItem("Select Agency", "0"))

        ddSKU.DataSource = Nothing
        ddSKU.Items.Clear()
        ddSKU.DataSource = ObjCommon.GetProductsByOrg_Agency(Err_No, Err_Desc, ddlOrganization.SelectedValue, 0)
        ddSKU.DataTextField = "Description"
        ddSKU.DataValueField = "Inventory_Item_ID"
        ddSKU.DataBind()

        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Now()
    End Sub

    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender

        For Each column As GridColumn In gvRep.MasterTableView.Columns
            If column.UniqueName = "ReturnsValue" Then

                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Decimal")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
            End If
            If column.UniqueName = "SalesValue" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Decimal")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
            End If
        Next
    End Sub
End Class