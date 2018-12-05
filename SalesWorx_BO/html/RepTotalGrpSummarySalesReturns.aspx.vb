Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI

Partial Public Class RepTotalGrpSummarySalesReturns
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer
    Private ReportPath As String = "GrpSummaryCustSalesReturns"

    Private Const PageID As String = "P245"
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
                ddlOrganization.DataTextField = "Description"
                ddlOrganization.DataValueField = "MAS_Org_ID"
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))

                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                End If

                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")

                Dim objUserAccess As UserAccess
                objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)


                ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())
                ddlVan.DataBind()
                ddlVan.Items.Insert(0, New RadComboBoxItem("Select Van", "0"))



                ddlAgency.DataSource = ObjCommon.GetAgencyList(Err_No, Err_Desc, ddlOrganization.SelectedValue)
                ddlAgency.DataBind()
                ddlAgency.Items.Insert(0, New RadComboBoxItem("Select Principal", "0"))


                ddlCustomer.DataSource = ObjCommon.GetCustomerLocation(Err_No, Err_Desc)
                ddlCustomer.DataBind()
                ddlCustomer.Items.Insert(0, New RadComboBoxItem("Select Customer", "0"))


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

        

        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
        Dim FromDate As New ReportParameter
        FromDate = New ReportParameter("Fromdate", CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"))

        Dim ToDate As New ReportParameter
        ToDate = New ReportParameter("Todate", CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))

        Dim OrgId As New ReportParameter
        OrgId = New ReportParameter("OrgID", CStr(ddlOrganization.SelectedItem.Value.ToString()))

        Dim SID As New ReportParameter

        If ddlVan.SelectedIndex > 0 Then
            SID = New ReportParameter("SalesRep_ID", ddlVan.SelectedItem.Value)
        Else
            SID = New ReportParameter("SalesRep_ID", "0")
        End If

        Dim AgencyID As New ReportParameter
        If ddlAgency.SelectedIndex > 0 Then
            AgencyID = New ReportParameter("Agency", ddlAgency.SelectedItem.Value)
        Else
            AgencyID = New ReportParameter("Agency", "0")
        End If

        Dim CustomerID As New ReportParameter
        If ddlCustomer.SelectedIndex > 0 Then
            CustomerID = New ReportParameter("Customer", ddlCustomer.SelectedItem.Value)
        Else
            CustomerID = New ReportParameter("Customer", "0")
        End If


        rview.ServerReport.SetParameters(New ReportParameter() {OrgId, SID, FromDate, ToDate, AgencyID, CustomerID})

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
            Response.AddHeader("Content-disposition", "attachment;filename=TotalGrpSummary.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=TotalGrpSummary.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()

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
            BindData()
            Args.Visible = True
            gvRep.Visible = True
            divCurrency.Visible = True
        Else
            Args.Visible = False
            gvRep.Visible = False
            divCurrency.Visible = False
        End If
    End Sub
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If (ddlOrganization.SelectedValue = "0") Then
            MessageBoxValidation("Please select the Organisation", "Validation")
            Return bretval
        ElseIf Not IsDate(txtFromDate.SelectedDate) Then
            MessageBoxValidation("Enter From date.", "Validation")
            Return bretval
        ElseIf Not IsDate(txtToDate.SelectedDate) Then
            MessageBoxValidation("Enter To date.", "Validation")
            Return bretval
        ElseIf CDate(txtFromDate.SelectedDate) > CDate(txtToDate.SelectedDate) Then
            MessageBoxValidation("From Date should not be greater than To Date.", "Validation")
            Return bretval
        Else
            bretval = True
            Return bretval
        End If
    End Function
    Private Sub BindData()
        Dim SearchQuery As String = ""
        Try
             
            rpbFilter.Items(0).Expanded = False
            lbl_org.Text = ddlOrganization.SelectedItem.Text
            lbl_Fromdt.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_ToDate.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")
            Dim Agency As String = "0"
            If ddlAgency.SelectedIndex > 0 Then
                lbl_Principle.Text = ddlAgency.SelectedItem.Text
                Agency = ddlAgency.SelectedItem.Value
            Else
                lbl_Principle.Text = "All"
            End If

            Dim Customer As String = "0"
            Dim SiteID As String = "0"
            If ddlCustomer.SelectedIndex > 0 Then
                lbl_Customer.Text = ddlCustomer.SelectedItem.Text
                Customer = ddlCustomer.SelectedItem.Value
            Else
                lbl_Customer.Text = "All"
            End If

            Dim Van As String = "0"
            If ddlVan.SelectedIndex > 0 Then
                lbl_van.Text = ddlVan.SelectedItem.Text
                Van = ddlVan.SelectedItem.Value
            Else
                lbl_van.Text = "All"
            End If

            Args.Visible = True
            lbl_Currency.Text = HCurrency.Value
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetTotalSummarySalesReturns(Err_No, Err_Desc, ddlOrganization.SelectedValue, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"), Van, Agency, Customer)

            Dim dtFinal As New DataTable
            dtFinal.Columns.Add("Customer")
            dtFinal.Columns.Add("Agency")
            dtFinal.Columns.Add("Type")
            dtFinal.Columns.Add("Qty", Type.GetType("System.Double"))
            dtFinal.Columns.Add("Value", Type.GetType("System.Double"))


            If dt.Rows.Count Then
                For Each dr As DataRow In dt.Rows

                    Dim drfinal1 As DataRow
                    drfinal1 = dtFinal.NewRow
                    drfinal1("Customer") = dr("Location")
                    drfinal1("Agency") = dr("Agency")
                    drfinal1("Type") = "Sales"
                    drfinal1("Qty") = dr("Sqty")
                    drfinal1("Value") = dr("SValue")
                    dtFinal.Rows.Add(drfinal1)


                    Dim drfinal2 As DataRow
                    drfinal2 = dtFinal.NewRow
                    drfinal2("Customer") = dr("Location")
                    drfinal2("Agency") = dr("Agency")
                    drfinal2("Type") = "Returns"
                    drfinal2("Qty") = dr("Rqty")
                    drfinal2("Value") = dr("RValue")
                    dtFinal.Rows.Add(drfinal2)

                     
                Next
            End If

            gvRep.DataSource = dtFinal
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

    '        Dim AgencyID As New ReportParameter
    '        AgencyID = New ReportParameter("Agency", ddlAgency.SelectedItem.Value)

    '        Dim CustomerID As New ReportParameter
    '        CustomerID = New ReportParameter("Customer", ddlCustomer.SelectedItem.Value)

    '        With RVMain
    '            .Reset()
    '            .Visible = True
    '            .ShowParameterPrompts = False
    '            .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
    '            .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
    '            .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
    '            .ServerReport.SetParameters(New ReportParameter() {OrgId, SID, FromDate, ToDate, AgencyID, CustomerID})
    '            '.ServerReport.Refresh()

    '        End With


    '    Catch Ex As Exception
    '        '  log.Error(GetExceptionInfo(Ex))
    '    End Try
    'End Sub
     Sub MessageBoxValidation(ByVal str As String, Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Private Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrganization.SelectedIndexChanged
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        ObjCommon = New SalesWorx.BO.Common.Common()
        ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())
        ddlVan.DataBind()
        ddlVan.Items.Insert(0, New RadComboBoxItem("Select Van", "0"))
        ddlAgency.DataSource = ObjCommon.GetAgencyList(Err_No, Err_Desc, ddlOrganization.SelectedValue)
        ddlAgency.DataBind()
        ddlAgency.Items.Insert(0, New RadComboBoxItem("Select Principal", "0"))


        Dim dtcurrency As DataTable
        Dim ObjReport As New SalesWorx.BO.Common.Reports
        dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

        Dim Currency As String = ""
        If dtcurrency.Rows.Count > 0 Then
            Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
        End If

        HCurrency.Value = Currency

        Args.Visible = False
        gvRep.Visible = False
        divCurrency.Visible = False

    End Sub

    Private Sub gvRep_PageIndexChanged(sender As Object, e As PivotGridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        BindData()
    End Sub
    Private Sub RadPivotGrid1_CellDataBound(sender As Object, e As PivotGridCellDataBoundEventArgs) Handles gvRep.CellDataBound

        If TypeOf e.Cell Is PivotGridColumnHeaderCell Then

            If e.Cell.Text.IndexOf("Sum of") >= 0 Then
                e.Cell.Text = e.Cell.Text.Replace("Sum of", "")
            End If

        End If

    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        ddlAgency.ClearSelection()

        ddlVan.ClearSelection()
        ddlVan.Items.Clear()
        ddlVan.Text = ""
        ddlAgency.Items.Clear()
        ddlAgency.Text = ""

        ddlCustomer.ClearSelection()
        ddlCustomer.Text = ""

        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If

        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        ObjCommon = New SalesWorx.BO.Common.Common()
        ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())
        ddlVan.DataBind()
        ddlVan.Items.Insert(0, New RadComboBoxItem("Select Van", "0"))
        ddlAgency.DataSource = ObjCommon.GetAgencyList(Err_No, Err_Desc, ddlOrganization.SelectedValue)
        ddlAgency.DataBind()
        ddlAgency.Items.Insert(0, New RadComboBoxItem("Select Principal", "0"))

        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")
        Args.Visible = False
        gvRep.Visible = False
        divCurrency.Visible = False
        lblmsgUOM.Text = ""
    End Sub
End Class