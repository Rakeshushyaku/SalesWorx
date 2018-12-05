Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Partial Public Class RepSKUwiseSales
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager

    Dim ObjCustomer As Customer

    Private ReportPath As String = "SKUWiseSales"

    Private Const PageID As String = "P229"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single
    Dim StartDate As Date
    Dim ToyearSelected As String
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not IsNothing(Me.Master) Then

            Dim masterScriptManager As ScriptManager
            masterScriptManager = CType(Master.FindControl("ScriptManager1"), ScriptManager)

            ' Make sure our master page has the script manager we're looking for
            If Not IsNothing(masterScriptManager) Then

                ' Turn off partial page postbacks for this page
                masterScriptManager.EnablePartialRendering = False
            End If

        End If

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        If Not IsPostBack Then

            Dim ObjCommon As SalesWorx.BO.Common.Common
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

                HUID.Value = CType(Session("User_Access"), UserAccess).UserID

                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1

                End If


                ddlSKU.DataSource = Nothing
                ddlSKU.DataSource = ObjCommon.GetSKU(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
                ddlSKU.DataTextField = "SKU"
                ddlSKU.DataValueField = "Inventory_item_ID"
                ddlSKU.DataBind()
                ddlSKU.Items.Insert(0, New RadComboBoxItem("Select SKU", "0"))

                txtFromDate.SelectedDate = DateAdd(DateInterval.Month, -3, Now)
                txtToDate.SelectedDate = Now

                If Not Err_Desc Is Nothing Then
                    log.Error(Err_Desc)
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
                End If

                ObjCommon = Nothing
            Catch ex As Exception
                Err_No = "74166"
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
     

    Private Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrganization.SelectedIndexChanged
        Dim ObjCommon As New SalesWorx.BO.Common.Common
        ddlSKU.DataSource = Nothing
        ddlSKU.DataSource = ObjCommon.GetSKU(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
        ddlSKU.DataTextField = "SKU"
        ddlSKU.DataValueField = "Inventory_item_ID"
        ddlSKU.DataBind()
        ddlSKU.Items.Insert(0, New RadComboBoxItem("Select SKU", "0"))
        ObjCommon = Nothing

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
        Args.Visible = False
        reportblocker.Visible = False
        RVMain.Visible = False
        If ValidateInputs() Then
            InitReportViewer()
        End If
    End Sub
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        Dim fromdate As DateTime
        If CDate(txtFromDate.SelectedDate).Day = 1 Then
            fromdate = CDate(txtFromDate.SelectedDate)
        Else
            fromdate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), CDate(txtFromDate.SelectedDate))
        End If

        Dim todate As DateTime
        If CDate(txtToDate.SelectedDate).Day = 1 Then
            todate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(txtToDate.SelectedDate)))
        Else
            todate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, DateAdd(DateInterval.Day, -1 * (Now.Day - 1), CDate(txtToDate.SelectedDate))))
        End If

        If (ddlOrganization.SelectedIndex <= 0) Then
            MessageBoxValidation("Please select the Organisation", "Validation")
            Return bretval
        ElseIf Not IsDate(txtFromDate.SelectedDate) Then
            MessageBoxValidation("Enter a valid from date.", "Validation")
            Return bretval
        ElseIf Not IsDate(txtToDate.SelectedDate) Then
            MessageBoxValidation("Enter a valid to date.", "Validation")
            Return bretval
        ElseIf CDate(fromdate) > CDate(todate) Then
            MessageBoxValidation("Start Date should not be greater than End Date.", "Validation")
            Return bretval
        Else
            bretval = True
            Return bretval
        End If
    End Function
    Private Sub BtnExportExcel_Click(sender As Object, e As EventArgs) Handles BtnExportExcel.Click
        Try
            If ValidateInputs() Then
                Export("Excel")
            End If
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub
    Private Sub InitReportViewer()
        Try
            Args.Visible = True
            reportblocker.Visible = True
            RVMain.Visible = True

            Dim ObjReport As New SalesWorx.BO.Common.Reports

            Dim fromdate As DateTime
            If CDate(txtFromDate.SelectedDate).Day = 1 Then
                fromdate = CDate(txtFromDate.SelectedDate)
            Else
                fromdate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), CDate(txtFromDate.SelectedDate))
            End If

            Dim todate As DateTime
            If CDate(txtToDate.SelectedDate).Day = 1 Then
                todate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(txtToDate.SelectedDate)))
            Else
                todate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, DateAdd(DateInterval.Day, -1 * (Now.Day - 1), CDate(txtToDate.SelectedDate))))
            End If


            lbl_org.Text = ddlOrganization.SelectedItem.Text
            lbl_From.Text = fromdate.ToString("dd-MMM-yyyy")
            lbl_To.Text = todate.ToString("dd-MMM-yyyy")
            Dim invid As String = "0"
            If ddlSKU.SelectedIndex <= 0 Then
                lbl_SKU.Text = "All"
            Else
                lbl_SKU.Text = ddlSKU.SelectedItem.Text
                invid = ddlSKU.SelectedItem.Value
            End If

            Dim pFromDate As New ReportParameter
            pFromDate = New ReportParameter("FromDate", CDate(fromdate).ToString("dd-MMM-yyyy"))

            Dim pToDate As New ReportParameter
            pToDate = New ReportParameter("ToDate", CDate(todate).ToString("dd-MMM-yyyy"))

            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("OrgID", CStr(ddlOrganization.SelectedItem.Value.ToString()))


            Dim SKUID As New ReportParameter
            SKUID = New ReportParameter("SKUID", invid)

            Dim UID As New ReportParameter
            UID = New ReportParameter("UID", CStr(HUID.Value))

            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                .ServerReport.SetParameters(New ReportParameter() {OrgId, SKUID, pFromDate, pToDate, UID})
                .ServerReport.Refresh()

            End With
        Catch Ex As Exception
            '  log.Error(GetExceptionInfo(Ex))
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




            Dim fromdate As DateTime
            If CDate(txtFromDate.SelectedDate).Day = 1 Then
                fromdate = CDate(txtFromDate.SelectedDate)
            Else
                fromdate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), CDate(txtFromDate.SelectedDate))
            End If

            Dim todate As DateTime
            If CDate(txtToDate.SelectedDate).Day = 1 Then
                todate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(txtToDate.SelectedDate)))
            Else
                todate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, DateAdd(DateInterval.Day, -1 * (Now.Day - 1), CDate(txtToDate.SelectedDate))))
            End If


            Dim pFromDate As New ReportParameter
            pFromDate = New ReportParameter("FromDate", CDate(fromdate).ToString("dd-MMM-yyyy"))

            Dim pToDate As New ReportParameter
            pToDate = New ReportParameter("ToDate", CDate(todate).ToString("dd-MMM-yyyy"))

            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("OrgID", CStr(ddlOrganization.SelectedItem.Value.ToString()))

            Dim invid As String = "0"
            If ddlSKU.SelectedIndex <= 0 Then
                invid = 0
            Else
                invid = ddlSKU.SelectedItem.Value
            End If


            Dim SKUID As New ReportParameter
            SKUID = New ReportParameter("SKUID", invid)


            rview.ServerReport.SetParameters(New ReportParameter() {OrgId, SKUID, pFromDate, pToDate})

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
                Response.AddHeader("Content-disposition", "attachment;filename=SKUWiseSales.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=SKUWiseSales.xls")
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
    
     
    Sub MessageBoxValidation(ByVal str As String, Title As String)

        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click

        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        ddlSKU.ClearSelection()
        ddlSKU.Items.Clear()

        txtFromDate.SelectedDate = DateAdd(DateInterval.Month, -3, Now)
        txtToDate.SelectedDate = Now

        Dim ObjCommon As New SalesWorx.BO.Common.Common
        ddlSKU.DataSource = Nothing
        ddlSKU.DataSource = ObjCommon.GetSKU(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
        ddlSKU.DataTextField = "SKU"
        ddlSKU.DataValueField = "Inventory_item_ID"
        ddlSKU.DataBind()
        ddlSKU.Items.Insert(0, New RadComboBoxItem("Select SKU", "0"))
        ObjCommon = Nothing
        lblmsgUOM.Text = ""

        Args.Visible = False
        reportblocker.Visible = False
        RVMain.Visible = False

    End Sub
End Class