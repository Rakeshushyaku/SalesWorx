Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Partial Public Class RepClientWiseSalesReturns
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer
    Private ReportPath As String = "CustomerWiseSalesReturns"

    Private Const PageID As String = "P247"
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

                Dim objUserAccess As UserAccess
                objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                End If



                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                txtToDate.SelectedDate = Now()


                LoadOrgDetails()
 
               

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
    Sub LoadOrgDetails()
        If Not (ddlOrganization.SelectedItem.Value = "0") Then

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New SalesWorx.BO.Common.Common()


            ddl_Van.DataTextField = "SalesRep_Name"
            ddl_Van.DataValueField = "SalesRep_ID"
            ddl_Van.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)

            ddl_Van.DataBind()

            For Each itm As RadComboBoxItem In ddl_Van.Items
                itm.Checked = True
            Next

            Dim dtcurrency As DataTable
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

            Dim Currency As String = ""
            If dtcurrency.Rows.Count > 0 Then
                Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
                lblDecimal.Text = "N" & dtcurrency.Rows(0)("Decimal_Digits")
            End If


            ddlCustomer.DataSource = ObjCommon.GetCustomerLocation(Err_No, Err_Desc)
            ddlCustomer.DataTextField = "Location"
            ddlCustomer.DataValueField = "Location"
            ddlCustomer.DataBind()

            For Each itm As RadComboBoxItem In ddlCustomer.Items
                itm.Checked = True
            Next


        Else

            ddl_Van.Items.Clear()
        End If
        summary.InnerHtml = ""
        Args.Visible = False
        rptsect.Visible = False
    End Sub
    Private Sub BindData()

        Try
            Dim objRep As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            divCurrency.Visible = True
            Dim dtcurrency As DataTable
            dtcurrency = objRep.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

            Dim Currency As String = ""
            If dtcurrency.Rows.Count > 0 Then
                lblDecimal.Text = "N" & dtcurrency.Rows(0)("Decimal_Digits")
                Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
            End If

            lbl_Currency.Text = Currency



            dt = objRep.GetSalesReturnsbyClient(Err_No, Err_Desc, HorgID.Value, HVan.Value, HUID.Value, "G", CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"), Hcustomer.Value)

            gvRep.DataSource = dt
            gvRep.DataBind()

            Dim StrSummary As String = ""
            Dim sum = dt.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("SValue")))

            StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>Total Sales " & Currency & "<div class='text-primary'>" & Format(sum, lblDecimal.Text) & "</div></div></div>"

            Dim sumR = dt.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("RValue")))

            StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>Total Returns " & Currency & "<div class='text-primary'>" & Format(sumR, lblDecimal.Text) & "</div></div></div>"


            summary.InnerHtml = StrSummary

        Catch ex As Exception
            If Err_Desc IsNot Nothing Then
                log.Error(Err_Desc)
            Else
                log.Error(GetExceptionInfo(ex))
            End If
            Err_No = "74067"
            '  Err_Desc = ex.Message

        Finally
            ObjCustomer = Nothing
        End Try
    End Sub
    'Private Sub InitReportViewer()
    '    Try


    '        Dim FromDate As New ReportParameter
    '        FromDate = New ReportParameter("Fromdate", txtFromDate.Text)

    '        Dim ToDate As New ReportParameter
    '        ToDate = New ReportParameter("Todate", txtToDate.Text)

    '        Dim OrgId As New ReportParameter
    '        OrgId = New ReportParameter("OrgID", CStr(ddlOrganization.SelectedItem.Value.ToString()))

    '        Dim SID As New ReportParameter
    '        SID = New ReportParameter("SalesRep_ID", ddlVan.SelectedItem.Value)


    '        Dim CustomerID As New ReportParameter
    '        CustomerID = New ReportParameter("Customer", ddlCustomer.SelectedItem.Value)

    '        With RVMain
    '            .Reset()
    '            .Visible = True
    '            .ShowParameterPrompts = False
    '            .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
    '            .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
    '            If ddlType.SelectedItem.Value = "T" Then
    '                ReportPath = "CustomerWiseSalesReturns"
    '            ElseIf ddlType.SelectedItem.Value = "BG" Then
    '                ReportPath = "CustomerWiseSalesReturnsGraphBar"
    '            ElseIf ddlType.SelectedItem.Value = "PG" Then
    '                ReportPath = "CustomerWiseSalesReturnsGraphPie"
    '            ElseIf ddlType.SelectedItem.Value = "RG" Then
    '                ReportPath = "CustomerWiseSalesReturnsGraph"
    '            End If
    '            .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
    '            .ServerReport.SetParameters(New ReportParameter() {OrgId, SID, FromDate, ToDate, CustomerID})
    '            '.ServerReport.Refresh()

    '        End With


    '    Catch Ex As Exception
    '        '  log.Error(GetExceptionInfo(Ex))
    '    End Try
    'End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Private Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrganization.SelectedIndexChanged
         LoadOrgDetails()
    End Sub

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

        If ValidateInput() Then
            rpbFilter.Items(0).Expanded = False
            rptsect.Visible = True
            HUID.Value = CType(Session("User_Access"), UserAccess).UserID
            HorgID.Value = ddlOrganization.SelectedItem.Value

            lbl_org.Text = ddlOrganization.SelectedItem.Text
            Dim collection As IList(Of RadComboBoxItem) = ddl_Van.CheckedItems

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
            HVan.Value = van



            Dim collectionAgency As IList(Of RadComboBoxItem) = ddlCustomer.CheckedItems
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
                lbl_customer.Text = "All"
            Else
                lbl_customer.Text = Agencytxt
            End If
            Hcustomer.Value = Agency

            Args.Visible = True



            Dim fromdate As Date
            Dim Todate As Date

            fromdate = CDate(txtFromDate.SelectedDate)
            Todate = CDate(txtToDate.SelectedDate)

            lbl_Fromdt.Text = fromdate.ToString("dd-MMM-yyyy")
            lbl_Todt.Text = Todate.ToString("dd-MMM-yyyy")

            HDate.Value = fromdate.ToString("dd-MMM-yyyy")
            HToDate.Value = Todate.ToString("dd-MMM-yyyy")

            BindChart()
            BindData()
        Else
            rptsect.Visible = False
            summary.InnerHtml = ""
            Args.Visible = False
        End If
    End Sub
    Sub BindChart()

        ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
    End Sub

    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender
        For Each column As GridColumn In gvRep.MasterTableView.Columns
            If column.UniqueName.ToLower = "svalue" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                column.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
            ElseIf column.UniqueName.ToLower = "rvalue" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & lblDecimal.Text & "}"
                col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
            End If
        Next
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

    Function ValidateInput() As Boolean
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
    Private Sub BtnExportExcel_Click(sender As Object, e As EventArgs) Handles BtnExportExcel.Click
        Try
            If ValidateInput() Then
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
            Dim Searchvalue As New ReportParameter



            Dim FromDate As New ReportParameter
            FromDate = New ReportParameter("Fromdate", CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"))

            Dim ToDate As New ReportParameter
            ToDate = New ReportParameter("Todate", CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))

            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("OrgID", CStr(ddlOrganization.SelectedItem.Value.ToString()))


            Dim van As String = ""
            For Each li As RadComboBoxItem In ddl_Van.CheckedItems
                van = van & li.Value & ","
            Next
             
            If van = "" Then
                van = "0"
            End If

            Dim SID As New ReportParameter
            SID = New ReportParameter("SalesRep_ID", van)

            Dim UId As New ReportParameter
            UId = New ReportParameter("UId", objUserAccess.UserID)


            Dim collectionAgency As IList(Of RadComboBoxItem) = ddlCustomer.CheckedItems
            Dim Agency As String = ""
            For Each li As RadComboBoxItem In collectionAgency
                Agency = Agency & li.Value & ","
            Next
            If Agency = "" Then
                Agency = "0"
            End If


            Dim CustomerID As New ReportParameter
            CustomerID = New ReportParameter("Customer", Agency)

            rview.ServerReport.SetParameters(New ReportParameter() {FromDate, ToDate, OrgId, SID, UId, CustomerID})

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
                Response.AddHeader("Content-disposition", "attachment;filename=ClientwiseSalesreturns.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=ClientwiseSalesreturns.xls")
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
            If ValidateInput() Then
                Export("PDF")
            End If
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        ddl_Van.ClearCheckedItems()
        ddl_Van.Items.Clear()

        ddlCustomer.ClearCheckedItems()
        ddlCustomer.Text = ""
        ddlCustomer.Items.Clear()
        lblmsgUOM.Text = ""
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        LoadOrgDetails()
        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Now()

        rptsect.Visible = False
        Args.Visible = False
        summary.InnerHtml = ""
    End Sub
End Class