Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI

Public Class Rep_VanPerformance
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "VanPerformance"

    Private Const PageID As String = "P356"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single
    Public DeciVal As Integer = 2
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        If Not IsPostBack Then
            Dim ClientCode As String
            ClientCode = (New SalesWorx.BO.Common.Common).GetAppConfig(Err_No, Err_Desc, "CLIENT_CODE")
            If ClientCode <> "TB" Then
                Response.Redirect("Rep_VanPerformance_Asr.aspx")
            End If
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
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization"))

                txtFromDate.SelectedDate = DateAdd(DateInterval.Month, -2, Now)
                txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")

                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1

                    LoadVan()
                End If

                ddlSalesDist.DataSource = ObjCommon.GetSalesDistrictList(Err_No, Err_Desc, SubQry)
                ddlSalesDist.DataBind()
                ddlSalesDist.Items.Insert(0, New RadComboBoxItem("Sales District", "0"))

                Me.ddlDisplayMode.SelectedIndex = 0

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


    Sub LoadVan()

        Try

            If Not (ddlOrganization.SelectedItem.Value = "") Then

                Dim objUserAccess As UserAccess
                objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

                ObjCommon = New SalesWorx.BO.Common.Common()

                ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
                ddlVan.DataValueField = "SalesRep_ID"
                ddlVan.DataTextField = "SalesRep_Name"
                ddlVan.DataBind()
                ''  ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --"))
                If Not ddlVan.Items.FindItemByValue(Request.QueryString("ID")) Is Nothing Then
                    ddlVan.ClearSelection()
                    ddlVan.Items.FindItemByValue(Request.QueryString("ID")).Selected = True
                End If

                For Each itm As RadComboBoxItem In ddlVan.Items
                    itm.Checked = True
                Next
            Else
                ddlVan.Items.Clear()
            End If


        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub ddlOrganization_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlOrganization.SelectedIndexChanged
        LoadVan()
    End Sub

    Private Sub SearchBtn_Click(sender As Object, e As EventArgs) Handles SearchBtn.Click
        Try

            Args.Visible = False
            gvRep.Visible = False
            If ValidateInputs() Then
                gvRep.Visible = True
                For i As Integer = 0 To gvRep.Fields.Count - 1
                    If i = 0 Then
                        gvRep.Fields(i).Caption = Me.ddlDisplayMode.SelectedItem.Text
                    End If
                Next
                gvRep.Rebind()
                BindData()
            End If

        Catch ex As Exception
            log.Error(ex.Message())
        End Try
    End Sub

    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False

        Dim d1 As DateTime = txtFromDate.SelectedDate
        Dim d2 As DateTime = txtToDate.SelectedDate
        Dim M As Integer = Math.Abs((d1.Year - d2.Year))
        Dim months As Integer = Math.Abs(DateDiff(DateInterval.Month, d1, d2))

        If (ddlOrganization.SelectedValue = "") Then

            MessageBoxValidation("Please select the Organisation", "Validation")
            Return bretval
            'ElseIf (ddlVan.SelectedValue = "") Then
            '    MessageBoxValidation("Please select Van", "Validation")
            '    Return bretval
        ElseIf months > 12 Then
            MessageBoxValidation("Month span should be upto 1 year ", "Validation")
            Return bretval
        Else
            bretval = True
            Return bretval
        End If
    End Function

    Sub MessageBoxValidation(ByVal str As String, Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Private Sub BindData()
        Try

            rpbFilter.Items(0).Expanded = False
            Args.Visible = False

            Dim vantxt As String = ""
            Dim van As String = ""
            Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems
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

            lbl_org.Text = ddlOrganization.SelectedItem.Text
            If van = "0" Then
                lbl_van.Text = "All"
            Else
                lbl_van.Text = vantxt
            End If


            lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_district.Text = ddlSalesDist.SelectedItem.Text
            Args.Visible = True


            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetVanPerformance(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, txtFromDate.SelectedDate, txtToDate.SelectedDate, van, ddlSalesDist.SelectedItem.Value, Me.ddlDisplayMode.SelectedValue)


            '' Formatting result for pivot grid 

            Dim dtFinal As New DataTable
            dtFinal.Columns.Add("Description")
            dtFinal.Columns.Add("Year")
            dtFinal.Columns.Add("Calls", Type.GetType("System.Int32"))
            dtFinal.Columns.Add("No of Outlets", Type.GetType("System.Int32"))
            dtFinal.Columns.Add("Outlets Productivity %", Type.GetType("System.Double"))
            dtFinal.Columns.Add("Adherance %", Type.GetType("System.Double"))
            dtFinal.Columns.Add("Revenue", Type.GetType("System.Decimal"))

            For Each seldr As DataRow In dt.Rows
                Dim dr As DataRow
                dr = dtFinal.NewRow
                dr("Description") = seldr("Description")
                dr("Year") = CDate(seldr("Month").ToString & "/01/" & seldr("Year")).ToString("MMM-yyyy")
                dr("Calls") = seldr("Calls")
                dr("No of Outlets") = seldr("Outlets")
                dr("Outlets Productivity %") = seldr("OutletProductivity")
                dr("Adherance %") = seldr("Adherence")
                If seldr("Revenue") <> 0 Then
                    dr("Revenue") = seldr("Revenue")
                Else
                    dr("Revenue") = seldr("Revenue")
                End If

                dtFinal.Rows.Add(dr)
                hfCurrency.Value = seldr("CurrCode")
                hfCurDecimal.Value = seldr("DecimalDigit")
                DeciVal = seldr("DecimalDigit")
            Next

            gvRep.DataSource = dtFinal
            gvRep.DataBind()
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
    Private Sub gvRep_CellFormatting(sender As Object, e As PivotGridItemEventArgs)

    End Sub
    Private Sub gvRep_CellDataBound(sender As Object, e As PivotGridCellDataBoundEventArgs) Handles gvRep.CellDataBound
        If TypeOf e.Cell Is PivotGridColumnHeaderCell Then
            If e.Cell.Text.IndexOf("Sum of") >= 0 Then
                e.Cell.Text = e.Cell.Text.Replace("Sum of", "")
            End If

            If e.Cell.Text.IndexOf("Revenue") >= 0 Then
                e.Cell.Text = "Revenue (" & hfCurrency.Value & ")"
            End If

        End If


       

        If TypeOf e.Cell Is PivotGridDataCell Then
            Dim cell As PivotGridDataCell = TryCast(e.Cell, PivotGridDataCell)

            If cell.CellType = PivotGridDataCellType.DataCell OrElse cell.CellType = PivotGridDataCellType.RowTotalDataCell Then
                Select Case TryCast(cell.Field, PivotGridAggregateField).DataField
                    Case "Revenue"
                        If cell.DataItem.ToString().Length > 0 Then
                            cell.Text = FormatNumber(CDbl(cell.DataItem), hfCurDecimal.Value)
                        End If
                        Exit Select
                End Select
            End If
        End If


    End Sub

    Private Sub gvRep_PageIndexChanged(sender As Object, e As PivotGridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        BindData()
    End Sub

    Private Sub gvRep_PageSizeChanged(sender As Object, e As PivotGridPageSizeChangedEventArgs) Handles gvRep.PageSizeChanged
        BindData()
    End Sub

    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender
        Try
            '    Dim pager As PivotGridPagerItem = TryCast(gvRep.GetItems(PivotGridItemType.PagerItem)(0), PivotGridPagerItem)
            '    Dim combo As RadComboBox = TryCast(pager.FindControl("PageSizeComboBox"), RadComboBox)
            '    combo.Visible = False

            '    Dim pagelbl As Label = TryCast(pager.FindControl("ChangePageSizeLabel"), Label)
            '    If pagelbl IsNot Nothing Then
            '        pagelbl.Visible = False
            '    End If

            'For i As Integer = 0 To gvRep.Fields.Count - 1
            '    If gvRep.Fields(i).Caption = "Name" Then
            '        gvRep.Fields(i).Caption = Me.ddlDisplayMode.SelectedItem.Text
            '    End If
            'Next
            'gvRep.Rebind()

        Catch ex As Exception
            log.Error(ex.Message)
        End Try
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
            Dim Searchvalue As New ReportParameter

            Dim fromdate As DateTime
            Dim todate As DateTime

            fromdate = CDate(txtFromDate.SelectedDate)
            todate = CDate(txtToDate.SelectedDate)

            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("OID", Me.ddlOrganization.SelectedValue)



            Dim OrgName As New ReportParameter
            OrgName = New ReportParameter("OrgName", CStr(ddlOrganization.SelectedItem.Text))


            Dim vantxt As String = ""
            Dim van As String = ""
            Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems
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


            If van = "" Then
                vantxt = "All"
            End If

            If ddlSalesDist.SelectedItem.Value = "0" Then
                lbl_district.Text = "All"
            Else
                lbl_district.Text = ddlSalesDist.SelectedItem.Text
            End If


            Dim SalesRep As New ReportParameter
            SalesRep = New ReportParameter("SalesRep", vantxt)


            Dim FDate As New ReportParameter
            FDate = New ReportParameter("FromDate", fromdate.ToString())

            Dim TDate As New ReportParameter
            TDate = New ReportParameter("ToDate", todate.ToString())

            Dim SID As New ReportParameter
            SID = New ReportParameter("SID", van)


            Dim SalesDistrictID As New ReportParameter
            SalesDistrictID = New ReportParameter("SalesDistrictID", CStr(ddlSalesDist.SelectedItem.Value))

            Dim DisplayMode As New ReportParameter
            DisplayMode = New ReportParameter("DisplayMode", CStr(ddlDisplayMode.SelectedValue))
            rview.ServerReport.SetParameters(New ReportParameter() {FDate, TDate, OrgId, SID, OrgName, SalesRep, SalesDistrictID, DisplayMode})

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
                Response.AddHeader("Content-disposition", "attachment;filename=VanPerformance.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=VanPerformance.xls")
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

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        ddlVan.ClearCheckedItems()
        ddlVan.Items.Clear()
        LoadVan()
        ddlSalesDist.ClearSelection()
        ddlDisplayMode.ClearSelection()
        txtFromDate.SelectedDate = DateAdd(DateInterval.Month, -2, Now)
        txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")
        gvRep.Visible = False
        Args.Visible = False
    End Sub
End Class