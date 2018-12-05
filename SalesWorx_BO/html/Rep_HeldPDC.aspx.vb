Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports System.Globalization
Imports Telerik.Web.UI
Partial Public Class Rep_HeldPDC
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager

    Dim ObjCustomer As Customer

    Private ReportPath As String = "HeldPDCs"

    Private Const PageID As String = "P124"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single
    Dim StartDate As Date
    Dim ToyearSelected As String
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    'Private Sub RepSales_WeeklyMonthly_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
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
                ddlOrganization.DataTextField = "Description"
                ddlOrganization.DataValueField = "MAS_Org_ID"
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))

                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                End If
                LoadorgDetails()

                txtfromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
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
    Sub LoadorgDetails()
        If Not (ddlOrganization.SelectedItem.Value = "0") Then

            Dim dtcurrency As DataTable
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

            Dim Currency As String = ""
            If dtcurrency.Rows.Count > 0 Then
                Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
                lblDecimal.Text = "N" & dtcurrency.Rows(0)("Decimal_Digits")
            End If
        End If
    End Sub
    Function ValidateInputs() As Boolean
        Dim bRetVal As Boolean = False
        If ddlOrganization.SelectedItem.Value = "0" Then
            MessageBoxValidation("Please select the organization", "Validation")
            Return bRetVal
        End If

        If IsDate(txtfromDate.SelectedDate) = False Then
            MessageBoxValidation("Please enter a valid from date", "Validation")
            Return bRetVal
        End If
        If IsDate(txtToDate.SelectedDate) = False Then
            MessageBoxValidation("Please enter a valid to date", "Validation")
            Return bRetVal
        End If

        If Not (CDate(txtfromDate.SelectedDate) <= CDate(txtToDate.SelectedDate)) Then
            MessageBoxValidation("Invalid Date range selection", "Validation")
            Return bRetVal
        End If
        bRetVal = True
        Return bRetVal
    End Function
    Private Sub BindReport()
        Try
          
            rpbFilter.Items(0).Expanded = False
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetHeldPDC(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, txt_CollectionRef.Text, CDate(txtfromDate.SelectedDate).ToString(), CDate(txtToDate.SelectedDate).ToString(), objUserAccess.UserID)
            gvRep.DataSource = dt
            gvRep.DataBind()


            Dim dtcurrency As DataTable
            dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
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

                StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>" & "Total " & x.PayMode & Currency & "<div class='text-primary'>" & Format(x.Total, lblDecimal.Text) & "</div></div></div>"

                i = i + 1
            Next

           
            Dim sum = dt.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("Discount")))

            StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>Total discount " & Currency & "<div class='text-primary'>" & Format(sum, lblDecimal.Text) & "</div></div></div>"

            summary.InnerHtml = StrSummary
        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub

    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender
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
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click

        If ValidateInputs() Then
            gvRep.Visible = True
            BindReport()
        Else
            gvRep.Visible = False
        End If
    End Sub
    

    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
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
    Sub Export(format As String)

        
        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
        Dim pFromDate As New ReportParameter
        pFromDate = New ReportParameter("FromDate", CDate(txtfromDate.SelectedDate).ToString("dd-MMM-yyyy"))

        Dim pToDate As New ReportParameter
        pToDate = New ReportParameter("ToDate", CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))

        Dim OrgId As New ReportParameter
        OrgId = New ReportParameter("OID", CStr(ddlOrganization.SelectedItem.Value.ToString()))

        Dim CollRefNo As New ReportParameter
        CollRefNo = New ReportParameter("CollRefNo", txt_CollectionRef.Text)

        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        Dim myParamUserId As New ReportParameter
        myParamUserId = New ReportParameter("Uid", objUserAccess.UserID)

        rview.ServerReport.SetParameters(New ReportParameter() {CollRefNo, OrgId, myParamUserId, pFromDate, pToDate})

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
            Response.AddHeader("Content-disposition", "attachment;filename=Collections.pdf")
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=Collections.xls")
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()
    End Sub

    Private Sub ddlOrganization_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlOrganization.SelectedIndexChanged
        LoadorgDetails()
    End Sub
End Class