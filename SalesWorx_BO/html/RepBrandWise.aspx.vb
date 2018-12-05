Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI

Partial Public Class RepBrandWise
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "BrandSummary"

    Private Const PageID As String = "P381"
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
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))



                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                End If
                LoadOrgDetails()
                txtFromDate.SelectedDate = Format(Now().Date.AddDays(-7), "dd-MMM-yyyy")
                txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")

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
    Private Sub BindData()
         

        Try
            rpbFilter.Items(0).Expanded = False

            lbl_org.Text = ddlOrganization.SelectedItem.Text
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


            If van = "0" Then
                lbl_van.Text = "All"
            Else
                lbl_van.Text = vantxt
            End If


            Dim collectionAgency As IList(Of RadComboBoxItem) = ddlBrand.CheckedItems
            Dim Agencytxt As String = ""
            Dim Agency As String = ""
            For Each li As RadComboBoxItem In collectionAgency
                Agency = Agency & li.Value & ","
                Agencytxt = Agencytxt & li.Text & ","
            Next
            If Agencytxt <> "" Then
                Agencytxt = Agencytxt.Substring(0, Agencytxt.Length - 1)
            End If
            If Agency = "" Then
                Agency = "0"
            End If

            If Agency = "0" Then
                lbl_Brand.Text = "All"
            Else
                lbl_Brand.Text = Agencytxt
            End If


            Args.Visible = True



            Dim fromdate As Date
            Dim Todate As Date

            fromdate = CDate(txtFromDate.SelectedDate)
            Todate = CDate(txtToDate.SelectedDate)

            lbl_from.Text = fromdate.ToString("dd-MMM-yyyy")
            lbl_To.Text = Todate.ToString("dd-MMM-yyyy")

            Dim objRep As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable

            dt = objRep.GetSalesbyBrand(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, van, CDate(txtFromDate.SelectedDate).ToString("MM-dd-yyyy"), CDate(txtToDate.SelectedDate).ToString("MM-dd-yyyy"), Agency, ddlMode.SelectedItem.Value)

            If ddlMode.SelectedItem.Value = "Summary" Then
                gvRep.DataSource = dt
                gvRep.DataBind()
                gvRep_Detailed.Visible = False
                gvRep.Visible = True
            Else
                gvRep_Detailed.DataSource = dt
                gvRep_Detailed.DataBind()
                gvRep_Detailed.Visible = True
                gvRep.Visible = False
            End If

            Dim dtcurrency As DataTable
            dtcurrency = objRep.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
            Dim Currency As String = ""
            If dtcurrency.Rows.Count > 0 Then
                lblDecimal.Text = "N" & dtcurrency.Rows(0)("Decimal_Digits")
                Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
            End If
            lbl_Currency.Text = Currency


            Dim StrSummary As String = ""
            Dim sum = dt.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("InvValue")))

            StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>Net Sales " & Currency & "<div class='text-primary'>" & Format(sum, lblDecimal.Text) & "</div></div></div>"

            

            summary.InnerHtml = StrSummary


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
            ObjCustomer = Nothing
        End Try
    End Sub

    Private Sub InitReportViewer(ByVal fromdate As Date, ByVal Todate As Date, ByVal UID As Integer)
        Try

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)



         
            Dim VanStr As String = ""

            Dim BrandStr As String = ""


            For Each item As RadComboBoxItem In ddlVan.Items
                If item.Checked Then
                    VanStr = VanStr & "," & item.Value

                End If
            Next
            If VanStr = "" Then
                For Each item As RadComboBoxItem In ddlVan.Items
                    VanStr = VanStr & "," & item.Value
                Next
            End If


            For Each item As RadComboBoxItem In ddlBrand.Items
                If item.Checked Then
                    BrandStr = BrandStr & "," & item.Value
                End If
            Next

            If BrandStr = "" Then
                For Each item As RadComboBoxItem In ddlBrand.Items
                    BrandStr = BrandStr & "," & item.Value
                Next
            End If


            Dim SalesRepID As New ReportParameter
            SalesRepID = New ReportParameter("SID", VanStr)

            Dim FDate As New ReportParameter
            FDate = New ReportParameter("Dat", fromdate.ToString())
            Dim EDate As New ReportParameter
            EDate = New ReportParameter("Dat1", Todate.ToString())

            Dim OID As New ReportParameter
            OID = New ReportParameter("OID", CStr(ddlOrganization.SelectedValue.ToString()))

            Dim OrgName As New ReportParameter
            OrgName = New ReportParameter("OrgName", CStr(ddlOrganization.SelectedItem.Text))

            Dim Brand As New ReportParameter
            Brand = New ReportParameter("Brand", BrandStr)

            Dim Mode As New ReportParameter
            Mode = New ReportParameter("Mode", CStr(Me.ddlMode.SelectedItem.Value))

            'With RVMain
            '    .Reset()
            '    .ShowParameterPrompts = False
            '    .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            '    .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            '    .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
            '    .ServerReport.SetParameters(New ReportParameter() {OID, SalesRepID, FDate, EDate, Brand, Mode, OrgName})
            '    .ServerReport.Refresh()

            'End With


        Catch Ex As Exception
            '  log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub

    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If (ddlOrganization.SelectedValue = "0") Then
            MessageBoxValidation("Please select the Organisation", "Validation")
            Return bretval
        End If
        If (ddlVan.CheckedItems.Count = "0") Then
            MessageBoxValidation("Please check at lease a van", "Validation")
            Return bretval
        End If
        If (ddlBrand.CheckedItems.Count = "0") Then
            MessageBoxValidation("Please check at lease a brand", "Validation")
            Return bretval
        End If
        If Not IsDate(txtFromDate.SelectedDate) Then
            MessageBoxValidation("Enter valid period.", "Validation")
            Return bretval
        ElseIf Not IsDate(txtToDate.SelectedDate) Then
            MessageBoxValidation("Enter valid period.", "Validation")
            Return bretval
        ElseIf CDate(txtFromDate.SelectedDate) > CDate(txtToDate.SelectedDate) Then
            MessageBoxValidation("From Date should not be greater than To Date.", "Validation")
            Return bretval
        Else
            bretval = True
            Return bretval
        End If
    End Function
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
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

            Args.Visible = True

            BindData()
        Else


            gvRep.Visible = False
            gvRep_Detailed.Visible = False
            Args.Visible = False
            summary.InnerHtml = ""
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

    Private Sub gvRep_Detailed_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvRep_Detailed.SortCommand
        ViewState("SortFieldD") = e.SortExpression
        SortDirectionDetailed = "flip"
        BindData()
    End Sub
    Private Sub gvRep_Detailed_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvRep_Detailed.PageIndexChanged

        BindData()
    End Sub
    Private Property SortDirectionDetailed() As String
        Get
            If ViewState("SortDirectionD") Is Nothing Then
                ViewState("SortDirectionD") = "ASC"
            End If
            Return ViewState("SortDirectionD").ToString()
        End Get
        Set(ByVal value As String)
            Dim s As String = SortDirectionDetailed

            If value = "flip" Then
                s = If(s = "ASC", "DESC", "ASC")
            Else
                s = value
            End If

            ViewState("SortDirectionD") = s
        End Set
    End Property

    Sub MessageBoxValidation(ByVal str As String, Title As String)

        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        LoadOrgDetails()
        gvRep.Visible = False
        gvRep_Detailed.Visible = False
        Args.Visible = False
        summary.InnerHtml = ""
    End Sub
    Sub LoadOrgDetails()
        If Not (ddlOrganization.SelectedItem.Value = "0") Then

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New SalesWorx.BO.Common.Common
            'ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
            'ddlVan.DataBind()
            'ddlVan.Items.Insert(0, New ListItem("-- Select a value --"))

            ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
            ddlVan.DataTextField = "SalesRep_Name"
            ddlVan.DataValueField = "SalesRep_ID"
            ddlVan.DataBind()

            For Each li As RadComboBoxItem In ddlVan.Items
                li.Checked = True
            Next


            Dim ObjProd As New Product
            ddlBrand.DataSource = ObjProd.LoadBrandList(Err_No, Err_Desc, ddlOrganization.SelectedValue)
            ddlBrand.DataTextField = "Description"
            ddlBrand.DataValueField = "Code"
            ddlBrand.DataBind()

            For Each li As RadComboBoxItem In ddlBrand.Items
                li.Checked = True
            Next

        Else
            ddlBrand.Items.Clear()
            ddlVan.Items.Clear()

        End If
    End Sub
    Private Sub BtnExportExcel_Click(sender As Object, e As EventArgs) Handles BtnExportExcel.Click
        Try
            If ValidateInputs() Then
                Export("Excel")
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
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)




            Dim VanStr As String = ""

            Dim BrandStr As String = ""


            For Each item As RadComboBoxItem In ddlVan.Items
                If item.Checked Then
                    VanStr = VanStr & "," & item.Value

                End If
            Next
            If VanStr = "" Then
                For Each item As RadComboBoxItem In ddlVan.Items
                    VanStr = VanStr & "," & item.Value
                Next
            End If


            For Each item As RadComboBoxItem In ddlBrand.Items
                If item.Checked Then
                    BrandStr = BrandStr & "," & item.Value
                End If
            Next

            If BrandStr = "" Then
                For Each item As RadComboBoxItem In ddlBrand.Items
                    BrandStr = BrandStr & "," & item.Value
                Next
            End If


            Dim SalesRepID As New ReportParameter
            SalesRepID = New ReportParameter("SID", VanStr)

            Dim FDate As New ReportParameter
            FDate = New ReportParameter("Dat", CDate(txtFromDate.SelectedDate).ToString("MM-dd-yyyy"))
            Dim EDate As New ReportParameter
            EDate = New ReportParameter("Dat1", CDate(txtToDate.SelectedDate).ToString("MM-dd-yyyy"))

            Dim OID As New ReportParameter
            OID = New ReportParameter("OID", CStr(ddlOrganization.SelectedValue.ToString()))

            Dim OrgName As New ReportParameter
            OrgName = New ReportParameter("OrgName", CStr(ddlOrganization.SelectedItem.Text))

            Dim Brand As New ReportParameter
            Brand = New ReportParameter("Brand", BrandStr)

            Dim Mode As New ReportParameter
            Mode = New ReportParameter("Mode", CStr(Me.ddlMode.SelectedItem.Value))
            rview.ServerReport.SetParameters(New ReportParameter() {OID, SalesRepID, FDate, EDate, Brand, Mode, OrgName})

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
                Response.AddHeader("Content-disposition", "attachment;filename=BrandWiseSales.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=BrandWiseSales.xls")
                Response.AddHeader("Content-Length", bytes.Length)
            End If
            Response.OutputStream.Write(bytes, 0, bytes.Length)
            Response.OutputStream.Flush()
            Response.OutputStream.Close()
            Response.Flush()
            Response.Close()
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub BtnExportPDF_Click(sender As Object, e As EventArgs) Handles BtnExportPDF.Click
        Try
            If ValidateInputs() Then
                Export("PDF")
            End If
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        ddlVan.ClearCheckedItems()
        ddlVan.Items.Clear()

        ddlBrand.ClearCheckedItems()
        ddlBrand.Text = ""
        ddlBrand.Items.Clear()

        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        LoadOrgDetails()
        ddlMode.ClearSelection()

        txtFromDate.SelectedDate = Format(Now().Date.AddDays(-7), "dd-MMM-yyyy")
        txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")

        gvRep.Visible = False
        gvRep_Detailed.Visible = False
        Args.Visible = False
        summary.InnerHtml = ""
        lblmsgUOM.Text = ""
    End Sub

End Class