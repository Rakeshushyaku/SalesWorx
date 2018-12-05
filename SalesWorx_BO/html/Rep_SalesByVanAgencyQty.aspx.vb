Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Partial Public Class Rep_SalesByVanAgencyQty
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "SalesByVanAgencyQtyNew"

    Private Const PageID As String = "P122"
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
        Dim SearchQuery As String = ""
        Try


            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            Args.Visible = True
            rpbFilter.Items(0).Expanded = False
            lbl_org.Text = ddlOrganization.SelectedItem.Text
            dt = ObjReport.GetSalesbyVanAgencyQty(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, CType(Session.Item("USER_ACCESS"), UserAccess).UserID)

            Dim dtfinal As New DataTable
            dtfinal.Columns.Add("Van")
            dtfinal.Columns.Add("Agency")
            dtfinal.Columns.Add("QtyM2", Type.GetType("System.Double"))
            dtfinal.Columns.Add("QtyM1", Type.GetType("System.Double"))
            dtfinal.Columns.Add("QtyM", Type.GetType("System.Double"))
             
            Dim TobeDistinct As String() = {"Van"}
            Dim dtDistinct As DataTable = GetDistinctRecords(dt, TobeDistinct)

            Dim TobeDistinctAgency As String() = {"Agency"}
            Dim TobeDistinctAgencyDt As DataTable = GetDistinctRecords(dt, TobeDistinctAgency)

            If dtDistinct.Rows.Count > 0 Then
                For Each vandr As DataRow In dtDistinct.Rows
                    If TobeDistinctAgencyDt.Rows.Count > 0 Then
                        For Each Agencydr As DataRow In TobeDistinctAgencyDt.Rows
                            Dim seldr() As DataRow
                            seldr = dt.Select("Van='" & vandr("Van").ToString & "' and Agency='" & Agencydr("Agency") & "'")
                            If seldr.Length = 0 Then
                                Dim newdr As DataRow
                                newdr = dtfinal.NewRow
                                newdr("Van") = vandr("Van").ToString
                                newdr("Agency") = Agencydr("Agency")
                                newdr("QtyM2") = 0
                                newdr("QtyM1") = 0
                                newdr("QtyM") = 0
                                dtfinal.Rows.Add(newdr)
                            Else
                                For Each dr In seldr
                                    Dim newdr As DataRow
                                    newdr = dtfinal.NewRow
                                    newdr("Van") = dr("Van")
                                    newdr("Agency") = dr("Agency")
                                    newdr("QtyM2") = dr("QtyM2")
                                    newdr("QtyM1") = dr("QtyM1")
                                    newdr("QtyM") = dr("QtyM")
                                    dtfinal.Rows.Add(newdr)
                                Next
                            End If
                        Next
                    End If
                Next
            End If
            gvRep.DataSource = dtfinal
            gvRep.DataBind()

            ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "UpdateHeader();", True)
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
    Public Shared Function GetDistinctRecords(ByVal dt As DataTable, ByVal Columns As String()) As DataTable
        Dim dtUniqRecords As New DataTable()
        dtUniqRecords = dt.DefaultView.ToTable(True, Columns)
        Return dtUniqRecords
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
            gvRep.Visible = True
            HM2.Value = DateAdd(DateInterval.Month, -2, Now).ToString("MMM-yyyy")
            HM1.Value = DateAdd(DateInterval.Month, -1, Now).ToString("MMM-yyyy")
            HM.Value = DateAdd(DateInterval.Month, 0, Now).ToString("MMM-yyyy")
            BindData()
        Else
            gvRep.Visible = False
            Args.Visible = False
        End If
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
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
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If ddlOrganization.SelectedIndex > 0 Then
            bretval = True
        Else
            MessageBoxValidation("Please select the Organisation", "Validation")
            bretval = False
        End If
        Return bretval
    End Function
    Sub Export(format As String)


        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)

          Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        Dim myParamUserId As New ReportParameter
        myParamUserId = New ReportParameter("Uid", objUserAccess.UserID)

        Dim OrgId As New ReportParameter
        OrgId = New ReportParameter("OrgID", ddlOrganization.SelectedItem.Value)

        rview.ServerReport.SetParameters(New ReportParameter() {myParamUserId, OrgId})

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
            Response.AddHeader("Content-disposition", "attachment;filename=SalesbyVanAgencyQty.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=SalesbyVanAgencyQty.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()
    End Sub
    Private Sub gvRep_PageIndexChanged(sender As Object, e As PivotGridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        BindData()
    End Sub
     
    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        gvRep.Visible = False
        Args.Visible = False
        lblmsgUOM.Text = ""
    End Sub
End Class