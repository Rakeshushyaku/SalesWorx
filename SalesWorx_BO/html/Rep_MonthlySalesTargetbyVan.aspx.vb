Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Imports System.Linq
Imports System.Threading
Imports System.Globalization
Imports System.Xml
Imports OfficeOpenXml
Public Class Rep_MonthlySalesTargetbyVan
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As New SalesWorx.BO.Common.Common
    Dim ObjCollection As Collection
    Private ReportPath As String = "MonthlyTargetvsSalesByVan"
    Private Const PageID As String = "P385"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objRep As New Reports
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())

            Dim CountryTbl As DataTable = Nothing
            Dim orgTbl As DataTable = Nothing
            Try


                ObjCommon = New SalesWorx.BO.Common.Common()


                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)

                CountryTbl = ObjCommon.GetCountry(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)
                ddlCountry.DataSource = CountryTbl
                ddlCountry.DataBind()


                orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)

                HTargetType.Value = CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE

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

                    Me.hfCurrency.Value = Currency
                    Me.hfDecimal.Value = DecimalDigits
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
                    Me.hfCurrency.Value = Currency
                    Me.hfDecimal.Value = DecimalDigits
                    ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
                    ddlOrganization.DataBind()

                End If

                Dim OrgStr As String = Nothing
                For Each item As RadComboBoxItem In ddlOrganization.Items
                    item.Checked = True
                    If item.Checked Then

                        OrgStr = OrgStr & item.Value & ","

                    End If
                Next
                Dim year As New DateTime(DateTime.Now.Year, 1, 1)
                txtFromDate.SelectedDate = year
                txtToDate.SelectedDate = Now

                Me.lblC.Text = Me.hfCurrency.Value
                BindCombo(OrgStr)

                If CType(Session.Item("CONTROL_PARAMS"), ControlParams).FSR_TARGET_TYPE = "A" Then
                   
                    divAgency.Visible = True
                ElseIf CType(Session.Item("CONTROL_PARAMS"), ControlParams).FSR_TARGET_TYPE = "K" Then
                    divAgency.Visible = False
                  
                ElseIf CType(Session.Item("CONTROL_PARAMS"), ControlParams).FSR_TARGET_TYPE = "B" Then
                    divAgency.Visible = False
                 
                ElseIf CType(Session.Item("CONTROL_PARAMS"), ControlParams).FSR_TARGET_TYPE = "P" Then
                    divAgency.Visible = False
                 
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
    Private Sub BindCombo(OrgStr)


        ddlAgency.DataSource = objRep.GetAllOrgAgency(Err_No, Err_Desc, IIf(OrgStr Is Nothing, "0", OrgStr), CType(Session("User_Access"), UserAccess).UserID)
        ddlAgency.DataTextField = "Description"
        ddlAgency.DataValueField = "Code"
        ddlAgency.DataBind()

        For Each item As RadComboBoxItem In ddlAgency.Items
            item.Checked = True
        Next


        ddlVan.DataSource = objRep.GetAllOrgVan(Err_No, Err_Desc, IIf(OrgStr Is Nothing, "0", OrgStr), CType(Session("User_Access"), UserAccess).UserID)
        ddlVan.DataTextField = "SalesRep_Name"
        ddlVan.DataValueField = "SalesRep_ID"
        ddlVan.DataBind()

        For Each item As RadComboBoxItem In ddlVan.Items
            item.Checked = True
        Next

    End Sub

    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then

            BindAgencySales()
             
            rpt.Visible = True


        Else
            Args.Visible = False
            rpt.Visible = False

            Me.gvRep.DataSource = Nothing
            Me.gvRep.DataBind()


        End If
    End Sub

    Private Sub gvbyAgency_CellDataBound(sender As Object, e As PivotGridCellDataBoundEventArgs) Handles gvRep.CellDataBound
        Try


            If TypeOf e.Cell Is PivotGridColumnHeaderCell Then
                If e.Cell.Text.IndexOf("Sum of") >= 0 Then
                    e.Cell.Text = e.Cell.Text.Replace("Sum of", "")
                End If

                If e.Cell.Text.IndexOf("Target") >= 0 Then
                    e.Cell.CssClass = "targetcls"

                    If HTargetType.Value = "Q" Then
                        e.Cell.Text = "Target Qty"
                    Else
                        e.Cell.Text = "Target Value"
                    End If

                End If

                If e.Cell.Text.IndexOf("Sales") >= 0 Then
                    e.Cell.CssClass = "salescls"
                    If HTargetType.Value = "Q" Then
                        e.Cell.Text = "Sales Qty"
                    Else
                        e.Cell.Text = "Sales Value"
                    End If

                End If

                If e.Cell.Text.IndexOf("Achievement") >= 0 Then
                    e.Cell.CssClass = "achcls"
                End If
            End If




            If TypeOf e.Cell Is PivotGridDataCell Then
                Dim cell As PivotGridDataCell = TryCast(e.Cell, PivotGridDataCell)

                If cell.CellType = PivotGridDataCellType.DataCell OrElse cell.CellType = PivotGridDataCellType.RowTotalDataCell Then
                    Select Case TryCast(cell.Field, PivotGridAggregateField).DataField
                        Case "Target"
                            If Not cell.DataItem Is Nothing Then
                                If cell.DataItem.ToString().Length > 0 Then
                                    cell.Text = FormatNumber(Val(cell.DataItem), Me.hfDecimal.Value)
                                End If
                            End If
                        Case "Sales"
                            If Not cell.DataItem Is Nothing Then
                                If cell.DataItem.ToString().Length > 0 Then
                                    cell.Text = FormatNumber(Val(cell.DataItem), Me.hfDecimal.Value)
                                End If
                            End If
                            Exit Select
                    End Select
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub gvRep_PageIndexChanged(sender As Object, e As PivotGridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        BindAgencySales()
    End Sub
    Private Sub BindAgencySales()
        Try
            Dim SearchQuery As String = ""
            Dim orgStr As String = ""
            Dim orgname As String = ""
            Dim orgcnt As Integer = 0
            For Each item As RadComboBoxItem In ddlOrganization.Items
                If item.Checked Then
                    orgStr = orgStr & item.Value & ","
                    orgname = orgname & item.Text & ","
                    orgcnt = orgcnt + 1
                End If
            Next


            'If String.IsNullOrEmpty(orgStr) Then
            '    MessageBoxValidation("Select organization(s).", "Validation")
            '    Exit Sub
            'End If


            rpbFilter.Items(0).Expanded = False
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)



            Dim AgencyStr As String = ""
            Dim VanStr As String = ""
            Dim vancnt As Integer = 0
            Dim vantxt As String = ""
            Dim AgencyCnt As Integer = 0

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



            For Each item As RadComboBoxItem In ddlAgency.Items
                If item.Checked Then
                    AgencyCnt = AgencyCnt + 1
                    AgencyStr = AgencyStr & item.Value & ","
                End If
            Next

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


            Dim Agency As String = ""
            If AgencyStr <> "" Then
                AgencyStr = AgencyStr.Substring(0, AgencyStr.Length - 1)
            End If
            If AgencyStr = "" Then
                AgencyStr = "0"
            End If
            If AgencyCnt = ddlAgency.Items.Count Then
                lbl_Agency.Text = "All"
            Else
                lbl_Agency.Text = AgencyStr
            End If



            If orgname <> "" Then
                orgname = orgname.Substring(0, orgname.Length - 1)
            End If
            If orgname = "" Then
                orgname = "0"
            End If
            If orgcnt = ddlOrganization.Items.Count Then
                lbl_org.Text = "All"
            Else
                lbl_org.Text = orgname
            End If
            Me.lbl_from.Text = Me.txtFromDate.SelectedDate.Value.ToString("MMM-yyyy")
            Me.lbl_To.Text = Me.txtToDate.SelectedDate.Value.ToString("MMM-yyyy")

            Me.lbl_Country.Text = Me.ddlCountry.SelectedItem.Text

            Args.Visible = True
            rpt.Visible = True
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetMonthlyTargetAndSales(Err_No, Err_Desc, orgStr, VanStr, AgencyStr, fromdate.ToString("dd-MMM-yyyy"), todate.ToString("dd-MMM-yyyy"))

            'Session.Remove("dtMBRDetails")
            'Session("dtMBRDetails") = dt.Copy

            Dim dv As New DataView(dt)

            '  dv.Sort = "Sno ASC"
            ' Dim obj As DataSet = New DataSet()
            'obj.Tables.Add(dt)

            'Dim xdd As XmlDataDocument = New XmlDataDocument(obj)

            'obj = xdd.DataSet
            'hfMBRStat.Value = xdd.DataSet.GetXml.ToString()
            'gvAgency.DataSource = dv
            'gvAgency.DataBind()

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


            Dim dtTargetVSSales As New DataTable
            dtTargetVSSales.Columns.Add("Month", System.Type.GetType("System.String"))
            dtTargetVSSales.Columns.Add("Van", System.Type.GetType("System.String"))
            dtTargetVSSales.Columns.Add("Target", System.Type.GetType("System.Decimal"))
            dtTargetVSSales.Columns.Add("Sales", System.Type.GetType("System.Decimal"))
            dtTargetVSSales.Columns.Add("Achievement", System.Type.GetType("System.Decimal"))
            Dim s_target As Decimal = 0
            Dim s_Sales As Decimal = 0
            Dim DtMonths As New DataTable
            DtMonths = dt.DefaultView.ToTable(True, "MonthYear", "Van")
            For Each sdr As DataRow In DtMonths.Rows
                Dim seldr() As DataRow
                seldr = dt.Select("MonthYear='" & sdr("MonthYear") & "' and Van='" & sdr("Van") & "'")
                Dim dr As DataRow
                dr = dtTargetVSSales.NewRow
                dr("Month") = sdr("MonthYear")
                dr("Van") = sdr("Van")

                Dim seldrTarget() As DataRow
                
                seldrTarget = dt.Select("MonthYear='" & sdr("MonthYear") & "' and Van='" & sdr("Van") & "'")
                If seldrTarget.Length > 0 Then
                    dr("Target") = Val(seldrTarget(0)("TargetValue").ToString)
                    dr("Sales") = Val(seldrTarget(0)("SalesValue").ToString)
                    dr("Achievement") = Val(seldrTarget(0)("Perc").ToString)
                    If sdr("MonthYear").ToString.ToUpper = "CUMULATIVE" Then
                        s_target = s_target + Val(seldrTarget(0)("TargetValue").ToString)
                        s_Sales = s_Sales + Val(seldrTarget(0)("SalesValue").ToString)
                    End If
                Else
                    dr("Target") = 0
                    dr("Sales") = 0
                End If
                 
                dtTargetVSSales.Rows.Add(dr)
            Next

            Me.lblSales.Text = "0"
            Me.lblTarget.Text = "0"
            Me.lblTeamSize.Text = "0"

            Me.lblTeamSize.Text = vancnt

            If HTargetType.Value = "Q" Then
                Me.lblTargetCurr.Text = "Qty"
                Me.lblSalesCurr.Text = "Qty"
            Else
                Me.lblTargetCurr.Text = hfCurrency.Value
                Me.lblSalesCurr.Text = hfCurrency.Value
            End If


            Me.lblTarget.Text = CDec(s_target).ToString("#,##" & LabelDecimalDigits)
            Me.lblSales.Text = CDec(s_Sales).ToString("#,##" & LabelDecimalDigits)


            
            gvRep.DataSource = dtTargetVSSales
            gvRep.DataBind()

            'If dt.Rows.Count > 0 Then
            '    dt.Rows.Clear()
            'End If
            If dtTargetVSSales.Rows.Count > 0 Then
                img_export.Visible = True
            Else
                img_export.Visible = False
            End If

            

            ObjReport = Nothing
        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False

        Dim orgStr As String = ""

        For Each item As RadComboBoxItem In ddlOrganization.Items
            If item.Checked Then
                orgStr = orgStr & "," & item.Value
            End If
        Next


        If String.IsNullOrEmpty(orgStr) Then
            MessageBoxValidation("Please select a organization(s).", "Validation")
            Return bretval
        End If

        If CDate(Me.txtFromDate.SelectedDate.Value) > CDate(Me.txtToDate.SelectedDate.Value) Then
            MessageBoxValidation("Month to should be greater than month from", "Validation")
            Return bretval
        End If

        If Math.Abs(DateDiff(DateInterval.Month, CDate(Me.txtToDate.SelectedDate.Value), CDate(Me.txtFromDate.SelectedDate.Value))) > 13 Then
            MessageBoxValidation("Please select a date range of 1 year", "Validation")
            Return bretval
        End If

        Dim AgencyStr As String = ""
        Dim VanStr As String = ""


        For Each item As RadComboBoxItem In ddlVan.Items
            If item.Checked Then
                VanStr = VanStr & "," & item.Value

            End If
        Next

        For Each item As RadComboBoxItem In ddlAgency.Items
            If item.Checked Then
                AgencyStr = AgencyStr & "," & item.Value
            End If
        Next

        If String.IsNullOrEmpty(VanStr) Then
            MessageBoxValidation("Please select a van(s).", "Validation")
            Return bretval
        End If


        If String.IsNullOrEmpty(AgencyStr) Then
            MessageBoxValidation("Please select a agency(s).", "Validation")
            Return bretval
        End If

        Return True

    End Function
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
            Dim SearchQuery As String = ""
            Dim orgStr As String = ""
            Dim orgname As String = ""
            Dim orgcnt As Integer = 0

            For Each item As RadComboBoxItem In ddlOrganization.Items
                If item.Checked Then
                    orgStr = orgStr & item.Value & ","
                    orgname = orgname & item.Text & ","
                    orgcnt = orgcnt + 1
                End If
            Next


            If String.IsNullOrEmpty(orgStr) Then
                MessageBoxValidation("Select organization(s).", "Validation")
                Exit Sub
            End If


            rpbFilter.Items(0).Expanded = False
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)



            Dim AgencyStr As String = ""
            Dim VanStr As String = ""
            Dim vancnt As Integer = 0
            Dim vantxt As String = ""
            Dim AgencyCnt As Integer = 0

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

            If ddlVan.Items.Count = ddlVan.CheckedItems.Count Then

            End If

            For Each item As RadComboBoxItem In ddlAgency.Items
                If item.Checked Then
                    AgencyCnt = AgencyCnt + 1
                    AgencyStr = AgencyStr & item.Value & ","
                End If
            Next


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


            Dim Agency As String = ""
            If AgencyStr <> "" Then
                AgencyStr = AgencyStr.Substring(0, AgencyStr.Length - 1)
            End If
            If AgencyStr = "" Then
                AgencyStr = "0"
            End If
            If AgencyCnt = ddlAgency.Items.Count Then
                lbl_Agency.Text = "All"
            Else
                lbl_Agency.Text = AgencyStr
            End If



            If orgname <> "" Then
                orgname = orgname.Substring(0, orgname.Length - 1)
            End If
            If orgname = "" Then
                orgname = "0"
            End If
            If orgcnt = ddlOrganization.Items.Count Then
                lbl_org.Text = "All"
            Else
                lbl_org.Text = orgname
            End If
            Me.lbl_from.Text = Me.txtFromDate.SelectedDate.Value.ToString("MMM-yyyy")
            Me.lbl_To.Text = Me.txtToDate.SelectedDate.Value.ToString("MMM-yyyy")

            Me.lbl_Country.Text = Me.ddlCountry.SelectedItem.Text

            Args.Visible = True
            rpt.Visible = True

            Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
            rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            objRep = New Reports



           
            rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)



            Dim org As New ReportParameter
            org = New ReportParameter("OID", orgStr)


            Dim vans As New ReportParameter
            vans = New ReportParameter("VanList", VanStr)

            Dim agencyParam As New ReportParameter
            agencyParam = New ReportParameter("AgencyList", AgencyStr)

            Dim sdat As New ReportParameter
            sdat = New ReportParameter("FromMonth", fromdate.ToString("dd-MMM-yyyy"))

            Dim edat As New ReportParameter
            edat = New ReportParameter("ToMonth", todate.ToString("dd-MMM-yyyy"))

            Dim SalesorgName As New ReportParameter
            SalesorgName = New ReportParameter("OrgName", CStr(Me.lbl_org.Text))



            Dim Country As New ReportParameter
            Country = New ReportParameter("Country", CStr(Me.lbl_Country.Text))




            Dim Van_Name As String = ""
            If ddlVan.Items.Count = ddlVan.CheckedItems.Count Then
                Van_Name = "All"
            ElseIf ddlVan.CheckedItems.Count > 5 Then
                Van_Name = "Multiple"
            Else
                Van_Name = CStr(Me.lbl_van.Text)
            End If

            Dim VanName As New ReportParameter
            VanName = New ReportParameter("VanName", Van_Name)

            Dim AgencyNameParam As New ReportParameter
            AgencyNameParam = New ReportParameter("AgencyName", CStr(Me.lbl_Agency.Text))
            'Dim YTDTarget As New ReportParameter
            'YTDTarget = New ReportParameter("YTDTarget", CStr(Me.lblTarget.Text))
            'Dim YTDSales As New ReportParameter
            'YTDSales = New ReportParameter("YTDSales", CStr(Me.lblSales.Text))

            

            rview.ServerReport.SetParameters(New ReportParameter() {org, vans, agencyParam, sdat, edat, SalesorgName, Country, VanName, AgencyNameParam})

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
                Response.AddHeader("Content-disposition", "attachment;filename=TargetvsSales.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
                Response.ContentType = "application/pdf"
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=TargetvsSales.xls")
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

    Private Sub BtnExportExcel_Click(sender As Object, e As EventArgs) Handles BtnExportExcel.Click
        Try
            If ValidateInputs() Then
                Export("Excel")
            End If
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub





    Protected Sub ddlCountry_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlCountry.SelectedIndexChanged
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

        Me.hfCurrency.Value = Currency
        Me.hfDecimal.Value = DecimalDigits
        ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
        ddlOrganization.DataBind()


        Dim OrgStr As String = Nothing
        For Each item As RadComboBoxItem In ddlOrganization.Items
            item.Checked = True
            If item.Checked Then

                OrgStr = OrgStr & item.Value & ","

            End If
        Next
        Me.lblC.Text = Me.hfCurrency.Value
        BindCombo(OrgStr)
        'If ValidateInputs() = True Then

        '    BindAgencySales()
        '    BindSummary()
        '    'Me.AgencyTab.Tabs(0).Selected = True
        '    'Me.RadMultiPage21.SelectedIndex = 0
        '    'BindChart()
        '    If AgencyTab.Tabs(2).Selected = True Then

        '        ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart1();", True)
        '    ElseIf AgencyTab.Tabs(0).Selected = True Then
        '        ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
        '    End If
        'Else
        '    Me.gvAgency1.DataSource = Nothing
        '    Me.gvAgency1.DataBind()

        '    Me.gvSummary.DataSource = Nothing
        '    Me.gvSummary.DataBind()
        '    Dim Sdate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")
        '    Dim Edate As String = DateTime.Parse(Me.EndTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")

        '    If Me.EndTime.SelectedDate.Value.Month = 1 Or Me.EndTime.SelectedDate.Value.Month = 3 Or Me.EndTime.SelectedDate.Value.Month = 5 Or Me.EndTime.SelectedDate.Value.Month = 7 Or Me.EndTime.SelectedDate.Value.Month = 8 Or Me.EndTime.SelectedDate.Value.Month = 10 Or Me.EndTime.SelectedDate.Value.Month = 12 Then
        '        Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
        '        Edate = DateTime.Parse(Me.EndTime.SelectedDate.Value.Month.ToString() + "-31-" + Me.EndTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

        '    End If
        '    If Me.EndTime.SelectedDate.Value.Month = 4 Or Me.EndTime.SelectedDate.Value.Month = 9 Or Me.EndTime.SelectedDate.Value.Month = 6 Or Me.StartTime.SelectedDate.Value.Month = 11 Then
        '        Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
        '        Edate = DateTime.Parse(Me.EndTime.SelectedDate.Value.Month.ToString() + "-30-" + Me.EndTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

        '    End If
        '    If Me.EndTime.SelectedDate.Value.Month = 2 Then
        '        Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
        '        Edate = DateTime.Parse(Me.EndTime.SelectedDate.Value.Month.ToString() + "-28-" + Me.EndTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

        '    End If

        '    Me.hfSMonth.Value = Sdate
        '    Me.hfEMonth.Value = Edate
        '    Me.hfOrgID.Value = "0"
        '    Me.hfVans.Value = "0"
        '    Me.hfAgency.Value = "0"
        '    If AgencyTab.Tabs(2).Selected = True Then

        '        ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart1();", True)
        '    ElseIf AgencyTab.Tabs(0).Selected = True Then
        '        ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
        '    End If
        'End If
    End Sub

    Private Sub ddlOrganization_CheckAllCheck(sender As Object, e As RadComboBoxCheckAllCheckEventArgs) Handles ddlOrganization.CheckAllCheck
        Dim OrgStr As String = String.Empty

        For Each item As RadComboBoxItem In ddlOrganization.Items
            If item.Checked Then
                OrgStr = OrgStr & item.Value & ","
            End If
        Next

        BindCombo(OrgStr)
    End Sub
    Protected Sub ddlOrganization_ItemChecked(sender As Object, e As RadComboBoxItemEventArgs) Handles ddlOrganization.ItemChecked
        Dim OrgStr As String = String.Empty

        For Each item As RadComboBoxItem In ddlOrganization.Items
            If item.Checked Then
                OrgStr = OrgStr & item.Value & ","
            End If
        Next

        BindCombo(OrgStr)
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        Dim CountryTbl As DataTable = Nothing
        Dim orgTbl As DataTable = Nothing


        ObjCommon = New SalesWorx.BO.Common.Common()


        Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)

        CountryTbl = ObjCommon.GetCountry(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)
        ddlCountry.DataSource = CountryTbl
        ddlCountry.DataBind()


        orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)

        HTargetType.Value = CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE

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

            Me.hfCurrency.Value = Currency
            Me.hfDecimal.Value = DecimalDigits
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
            Me.hfCurrency.Value = Currency
            Me.hfDecimal.Value = DecimalDigits
            ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
            ddlOrganization.DataBind()

        End If

        Dim OrgStr As String = Nothing
        For Each item As RadComboBoxItem In ddlOrganization.Items
            item.Checked = True
            If item.Checked Then

                OrgStr = OrgStr & item.Value & ","

            End If
        Next
        Dim year As New DateTime(DateTime.Now.Year, 1, 1)
        txtFromDate.SelectedDate = year
        txtToDate.SelectedDate = Now

        Me.lblC.Text = Me.hfCurrency.Value
        BindCombo(OrgStr)
        Args.Visible = False
        rpt.Visible = False

    End Sub
    Private Sub BtnExportBiffExcel_Click(sender As Object, e As EventArgs) Handles BtnExportBiffExcel.Click

        Dim dtv As New DataTable
         
            Dim SearchQuery As String = ""
            Dim orgStr As String = ""
            Dim orgname As String = ""

            For Each item As RadComboBoxItem In ddlOrganization.Items
                If item.Checked Then
                    orgStr = orgStr & item.Value & ","
                End If
            Next

            For Each item As RadComboBoxItem In ddlOrganization.Items
                If item.Checked Then
                    orgname = orgname & item.Text & ","
                End If
            Next

        '
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)



            Dim AgencyStr As String = ""
            Dim VanStr As String = ""


            For Each item As RadComboBoxItem In ddlVan.Items
                If item.Checked Then
                    VanStr = VanStr & item.Value & ","

                End If
            Next

            For Each item As RadComboBoxItem In ddlAgency.Items
                If item.Checked Then
                    AgencyStr = AgencyStr & item.Value & ","
                End If
            Next

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

            Dim ObjReport As New SalesWorx.BO.Common.Reports




            dtv = ObjReport.GetMonthlyTargetAndSales(Err_No, Err_Desc, orgStr, VanStr, AgencyStr, fromdate.ToString("dd-MMM-yyyy"), todate.ToString("dd-MMM-yyyy")).Copy


            ObjReport = Nothing




        Dim dtTargetSales As New DataTable
        dtTargetSales.Columns.Add("Van", System.Type.GetType("System.String"))
        Dim s_target As Decimal = 0
        Dim s_Sales As Decimal = 0

        Dim DtM As New DataTable
        DtM = dtv.DefaultView.ToTable(True, "MonthYear")


        For Each sdr As DataRow In DtM.Rows
            dtTargetSales.Columns.Add(sdr("MonthYear").ToString & "Target", System.Type.GetType("System.Decimal"))
            dtTargetSales.Columns.Add(sdr("MonthYear").ToString & "Sales", System.Type.GetType("System.Decimal"))
            dtTargetSales.Columns.Add(sdr("MonthYear").ToString & "Acheivement", System.Type.GetType("System.Decimal"))
        Next


        Dim DtMonths As New DataTable
        DtMonths = dtv.DefaultView.ToTable(True, "Van")

        For Each sdr As DataRow In DtMonths.Rows
            Dim dr As DataRow
            dr = dtTargetSales.NewRow
            dr("Van") = sdr("Van")
            For Each monthdr As DataRow In DtM.Rows
                Dim seldr() As DataRow
                seldr = dtv.Select("MonthYear='" & monthdr("MonthYear") & "' and Van='" & sdr("Van") & "'")
                Dim seldrTarget() As DataRow


                seldrTarget = dtv.Select("MonthYear='" & monthdr("MonthYear") & "' and Van='" & sdr("Van") & "'")
                If seldrTarget.Length > 0 Then
                    dr(monthdr("MonthYear").ToString & "Target") = Val(seldrTarget(0)("TargetValue").ToString)
                    dr(monthdr("MonthYear").ToString & "Sales") = Val(seldrTarget(0)("SalesValue").ToString)
                    dr(monthdr("MonthYear").ToString & "Acheivement") = Val(seldrTarget(0)("Perc").ToString)
                Else
                    dr(monthdr("MonthYear").ToString & "Target") = 0
                    dr(monthdr("MonthYear").ToString & "Sales") = 0
                    dr(monthdr("MonthYear").ToString & "Acheivement") = 0
                End If


            Next
            dtTargetSales.Rows.Add(dr)
        Next


        DtMonths = Nothing
        If dtTargetSales.Rows.Count > 0 Then

            Using package As New ExcelPackage()
                ' add a new worksheet to the empty workbook
                Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
                Worksheet.Cells("A2").LoadFromDataTable(dtTargetSales, True)
                Worksheet.Cells.AutoFitColumns()


                Dim j As String = 2

                For i = 0 To DtM.Rows.Count - 1

                    Worksheet.Cells(2, j).Value = "Target"
                    Worksheet.Cells(1, j).Value = DtM.Rows(i)("MonthYear").ToString
                    Worksheet.Cells(1, j).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center
                    Worksheet.Cells(1, j, 1, j + 2).Merge = True
                    Worksheet.Cells(2, j + 1).Value = "Sales"
                    Worksheet.Cells(2, j + 2).Value = "Achievement"
                    j = j + 3
                Next
                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename= TargetAndSalesbyAgency.xlsx")

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