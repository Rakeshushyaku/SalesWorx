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

Public Class RepMBRReport
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As New SalesWorx.BO.Common.Common
    Dim ObjCollection As Collection
    Private ReportPath As String = "MBRReport_agency"
    Private Const PageID As String = "P342"
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
                StartTime.SelectedDate = year
                EndTime.SelectedDate = Now

                Me.lblC.Text = Me.hfCurrency.Value
                BindCombo(OrgStr)

                If CType(Session.Item("CONTROL_PARAMS"), ControlParams).FSR_TARGET_TYPE = "A" Then
                    AgencyTab.Tabs(1).Text = "Target vs Sales By Agency"
                    ' gvRep.Fields.Item(0).Caption = "Agency"
                    divAgency.Visible = True
                ElseIf CType(Session.Item("CONTROL_PARAMS"), ControlParams).FSR_TARGET_TYPE = "K" Then
                    divAgency.Visible = False
                    AgencyTab.Tabs(1).Text = "Target vs Sales By Category"
                    ' gvRep.Fields.Item(0).Caption = "Category"

                ElseIf CType(Session.Item("CONTROL_PARAMS"), ControlParams).FSR_TARGET_TYPE = "B" Then
                    divAgency.Visible = False
                    AgencyTab.Tabs(1).Text = "Target vs Sales By Brand"
                    ' gvRep.Fields.Item(0).Caption = "Brand"
                ElseIf CType(Session.Item("CONTROL_PARAMS"), ControlParams).FSR_TARGET_TYPE = "P" Then
                    divAgency.Visible = False
                    AgencyTab.Tabs(1).Text = "Target vs Sales By SKU"
                    '  gvRep.Fields.Item(0).Caption = "SKU"
                End If

                'If ValidateInputs() = True Then
                '    BindAgencySales()
                '    BindSummary()
                '    BindChart()
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
                '    BindChart()
                'End If


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
            BindSummary()
            BindSummaryChart()
            BIndTargetVsSales()
            rpt.Visible = True
             

        Else
            Args.Visible = False
            rpt.Visible = False

            Me.gvRep.DataSource = Nothing
            Me.gvRep.DataBind()

            Me.gvSummary.DataSource = Nothing
            Me.gvSummary.DataBind()
            Dim Sdate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")
            Dim Edate As String = DateTime.Parse(Me.EndTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")

            If Me.EndTime.SelectedDate.Value.Month = 1 Or Me.EndTime.SelectedDate.Value.Month = 3 Or Me.EndTime.SelectedDate.Value.Month = 5 Or Me.EndTime.SelectedDate.Value.Month = 7 Or Me.EndTime.SelectedDate.Value.Month = 8 Or Me.EndTime.SelectedDate.Value.Month = 10 Or Me.EndTime.SelectedDate.Value.Month = 12 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.EndTime.SelectedDate.Value.Month.ToString() + "-31-" + Me.EndTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            End If
            If Me.EndTime.SelectedDate.Value.Month = 4 Or Me.EndTime.SelectedDate.Value.Month = 9 Or Me.EndTime.SelectedDate.Value.Month = 6 Or Me.StartTime.SelectedDate.Value.Month = 11 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.EndTime.SelectedDate.Value.Month.ToString() + "-30-" + Me.EndTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            End If
            If Me.EndTime.SelectedDate.Value.Month = 2 And Me.EndTime.SelectedDate.Value.Year Mod 4 = 0 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.EndTime.SelectedDate.Value.Month.ToString() + "-29-" + Me.EndTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            End If

            If Me.EndTime.SelectedDate.Value.Month = 2 And Me.EndTime.SelectedDate.Value.Year Mod 4 <> 0 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.EndTime.SelectedDate.Value.Month.ToString() + "-28-" + Me.EndTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            End If

            Me.hfSMonth.Value = Sdate
            Me.hfEMonth.Value = Edate
            Me.hfOrgID.Value = "0"
            Me.hfVans.Value = "0"
            Me.hfAgency.Value = "0"
           
        End If
    End Sub



     
    Protected Sub gvSummary_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles gvSummary.ItemDataBound
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

        If TypeOf e.Item Is GridDataItem Then
            Dim dataItem As GridDataItem = DirectCast(e.Item, GridDataItem)
            For i As Integer = 0 To dataItem.Cells.Count - 1
                If IsNumeric(dataItem.Cells(i).Text) Then
                    If dataItem.Cells(3).Text = "Target Value" Or dataItem.Cells(3).Text = "Sales Value" Or dataItem.Cells(3).Text = "Drop size per call" Or dataItem.Cells(3).Text = "Average Outlet Billing" Then
                        dataItem.Cells(i).Text = CDec(IIf(dataItem.Cells(i).Text = "" Or dataItem.Cells(i).Text Is Nothing, "0", dataItem.Cells(i).Text)).ToString("#,##" & LabelDecimalDigits)
                    End If
                End If
            Next


        End If
    End Sub


    Private Sub gvSummary_PreRender(sender As Object, e As EventArgs) Handles gvSummary.PreRender
        For Each column As GridColumn In gvSummary.MasterTableView.AutoGeneratedColumns
            If column.UniqueName = "Description" Then
                column.ItemStyle.HorizontalAlign = HorizontalAlign.Left
                column.HeaderStyle.HorizontalAlign = HorizontalAlign.Left
                column.ItemStyle.Wrap = False
                column.HeaderStyle.Wrap = False
            ElseIf column.UniqueName = "Mode" Then
                column.Visible = False
            Else
                column.ItemStyle.HorizontalAlign = HorizontalAlign.Right
                column.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                column.ItemStyle.Wrap = False
                column.HeaderStyle.Wrap = True
            End If

        Next
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
                        e.Cell.Text = "Target Value (" & hfCurrency.Value & ")"
                    End If

                End If

                If e.Cell.Text.IndexOf("Sales") >= 0 Then
                    e.Cell.CssClass = "salescls"
                    If HTargetType.Value = "Q" Then
                        e.Cell.Text = "Sales Qty"
                    Else
                        e.Cell.Text = "Sales Value (" & hfCurrency.Value & ")"
                    End If

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
    Private Sub gvSummary_SortCommand(sender As Object, e As GridSortCommandEventArgs) Handles gvSummary.SortCommand
        ViewState("SortField1") = e.SortExpression
        SortDirection1 = "flip"
        BindSummary()
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

    Private Property SortDirection1() As String
        Get
            If ViewState("SortDirection1") Is Nothing Then
                ViewState("SortDirection1") = "ASC"
            End If
            Return ViewState("SortDirection1").ToString()
        End Get
        Set(ByVal value As String)
            Dim s As String = SortDirection1

            If value = "flip" Then
                s = If(s = "ASC", "DESC", "ASC")
            Else
                s = value
            End If

            ViewState("SortDirection1") = s
        End Set
    End Property
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

            Dim Sdate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")
            Dim Edate As String = DateTime.Parse(Me.EndTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")

            If Me.EndTime.SelectedDate.Value.Month = 1 Or Me.EndTime.SelectedDate.Value.Month = 3 Or Me.EndTime.SelectedDate.Value.Month = 5 Or Me.EndTime.SelectedDate.Value.Month = 7 Or Me.EndTime.SelectedDate.Value.Month = 8 Or Me.EndTime.SelectedDate.Value.Month = 10 Or Me.EndTime.SelectedDate.Value.Month = 12 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.EndTime.SelectedDate.Value.Month.ToString() + "-31-" + Me.EndTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            End If
            If Me.EndTime.SelectedDate.Value.Month = 4 Or Me.EndTime.SelectedDate.Value.Month = 9 Or Me.EndTime.SelectedDate.Value.Month = 6 Or Me.StartTime.SelectedDate.Value.Month = 11 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.EndTime.SelectedDate.Value.Month.ToString() + "-30-" + Me.EndTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            End If
            If Me.EndTime.SelectedDate.Value.Month = 2 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.EndTime.SelectedDate.Value.Month.ToString() + "-28-" + Me.EndTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

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
            Me.lbl_from.Text = Me.StartTime.SelectedDate.Value.ToString("MMM-yyyy")
            Me.lbl_To.Text = Me.EndTime.SelectedDate.Value.ToString("MMM-yyyy")

            Me.lbl_Country.Text = Me.ddlCountry.SelectedItem.Text

            Args.Visible = True
            rpt.Visible = True
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetMBRByAgency(Err_No, Err_Desc, orgStr, VanStr, AgencyStr, Sdate, Edate)

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
            dtTargetVSSales.Columns.Add("Agency", System.Type.GetType("System.String"))
            dtTargetVSSales.Columns.Add("Target", System.Type.GetType("System.Decimal"))
            dtTargetVSSales.Columns.Add("Sales", System.Type.GetType("System.Decimal"))
            Dim s_target As Decimal = 0
            Dim s_Sales As Decimal = 0
            Dim DtMonths As New DataTable
            DtMonths = dt.DefaultView.ToTable(True, "MnOrder", "Agency")
            For Each sdr As DataRow In DtMonths.Rows
                Dim seldr() As DataRow
                seldr = dt.Select("MnOrder='" & sdr("MnOrder") & "' and Agency='" & sdr("Agency") & "'")
                Dim dr As DataRow
                dr = dtTargetVSSales.NewRow
                dr("Month") = sdr("MnOrder")
                dr("Agency") = sdr("Agency")

                Dim seldrTarget() As DataRow
                Dim seldrSales() As DataRow

                seldrTarget = dt.Select("MnOrder='" & sdr("MnOrder") & "' and Agency='" & sdr("Agency") & "' and Description=' Target'")
                If seldrTarget.Length > 0 Then
                    dr("Target") = seldrTarget(0)("TotValue").ToString
                    If sdr("MnOrder").ToString.ToUpper = "CUMULATIVE" Then
                        s_target = s_target + Val(seldrTarget(0)("TotValue").ToString)
                    End If
                Else
                    dr("Target") = 0
                End If
                seldrSales = dt.Select("MnOrder='" & sdr("MnOrder") & "' and Agency='" & sdr("Agency") & "' and Description='Sales'")
                If seldrSales.Length > 0 Then
                    dr("Sales") = seldrSales(0)("TotValue").ToString
                    If sdr("MnOrder").ToString.ToUpper = "CUMULATIVE" Then
                        s_Sales = s_Sales + Val(seldrSales(0)("TotValue").ToString)
                    End If
                Else
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


            If dt.Rows.Count > 0 Then
                ViewState("AgencySales") = dt
            End If
            gvRep.DataSource = dtTargetVSSales
            gvRep.DataBind()
            ViewState("AgencyTSales") = dtTargetVSSales
            'If dt.Rows.Count > 0 Then
            '    dt.Rows.Clear()
            'End If
            If dtTargetVSSales.Rows.Count > 0 Then
                img_export.Visible = True
            Else
                img_export.Visible = False
            End If

            Me.hfSMonth.Value = Sdate
            Me.hfEMonth.Value = Edate
            Me.hfOrgID.Value = orgStr
            Me.hfVans.Value = VanStr
            Me.hfAgency.Value = AgencyStr
            dt = Nothing
            ObjReport = Nothing
        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub

    Sub BIndTargetVsSales()
        Try
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

            'If String.IsNullOrEmpty(orgStr) Then
            '    MessageBoxValidation("Select organization(s).", "Validation")
            '    Exit Sub
            'End If


            rpbFilter.Items(0).Expanded = False
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

            Dim Sdate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")
            Dim Edate As String = DateTime.Parse(Me.EndTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")

            If Me.EndTime.SelectedDate.Value.Month = 1 Or Me.EndTime.SelectedDate.Value.Month = 3 Or Me.EndTime.SelectedDate.Value.Month = 5 Or Me.EndTime.SelectedDate.Value.Month = 7 Or Me.EndTime.SelectedDate.Value.Month = 8 Or Me.EndTime.SelectedDate.Value.Month = 10 Or Me.EndTime.SelectedDate.Value.Month = 12 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.EndTime.SelectedDate.Value.Month.ToString() + "-31-" + Me.EndTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            End If
            If Me.EndTime.SelectedDate.Value.Month = 4 Or Me.EndTime.SelectedDate.Value.Month = 9 Or Me.EndTime.SelectedDate.Value.Month = 6 Or Me.StartTime.SelectedDate.Value.Month = 11 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.EndTime.SelectedDate.Value.Month.ToString() + "-30-" + Me.EndTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            End If
            If Me.EndTime.SelectedDate.Value.Month = 2 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.EndTime.SelectedDate.Value.Month.ToString() + "-28-" + Me.EndTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            End If

            Dim ObjReport As New SalesWorx.BO.Common.Reports

            Dim dt1 As New DataTable
            dt1 = ObjReport.GetMBRTargetvsSalesByMonths(Err_No, Err_Desc, orgStr, VanStr, AgencyStr, Sdate, Edate)

            Dim dtTargetVSSales As New DataTable
            dtTargetVSSales.Columns.Add("Month", System.Type.GetType("System.String"))
            dtTargetVSSales.Columns.Add("Target", System.Type.GetType("System.Decimal"))
            dtTargetVSSales.Columns.Add("Sales", System.Type.GetType("System.Decimal"))

            Dim DtMonths As New DataTable
            DtMonths = dt1.DefaultView.ToTable(True, "MnOrder")
            For Each sdr As DataRow In DtMonths.Rows
                Dim seldr() As DataRow
                seldr = dt1.Select("MnOrder='" & sdr("MnOrder") & "'")
                Dim dr As DataRow
                dr = dtTargetVSSales.NewRow
                dr("Month") = sdr("MnOrder")

                Dim seldrTarget() As DataRow
                Dim seldrSales() As DataRow

                seldrTarget = dt1.Select("MnOrder='" & sdr("MnOrder") & "' and Description=' Target'")
                If seldrTarget.Length > 0 Then
                    dr("Target") = seldrTarget(0)("TotValue")
                Else
                    dr("Target") = 0
                End If
                seldrSales = dt1.Select("MnOrder='" & sdr("MnOrder") & "' and Description='Sales'")
                If seldrSales.Length > 0 Then
                    dr("Sales") = seldrSales(0)("TotValue")
                Else
                    dr("Sales") = 0
                End If
                dtTargetVSSales.Rows.Add(dr)
            Next

            If DtMonths.Rows.Count > 5 Then
                Chart.Width = DtMonths.Rows.Count * 125
            Else
                Chart.Width = 600
            End If
            Chart.DataSource = dtTargetVSSales
            Chart.DataBind()


            gvtargetSales.DataSource = dtTargetVSSales
            gvtargetSales.DataBind()

            If HTargetType.Value = "Q" Then
                gvtargetSales.Columns(1).HeaderText = "Target Qty"
                gvtargetSales.Columns(2).HeaderText = "Sales Qty"
            Else
                gvtargetSales.Columns(1).HeaderText = "Target Value (" & hfCurrency.Value & ")"
                gvtargetSales.Columns(2).HeaderText = "Sales Value (" & hfCurrency.Value & ")"
            End If

            ViewState("TargetVSSales") = dtTargetVSSales.Copy


            dtTargetVSSales = Nothing
            ObjReport = Nothing
            DtMonths = Nothing
            dt1 = Nothing
        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub
    Private Sub BindSummary()
        Try
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

            'If String.IsNullOrEmpty(orgStr) Then
            '    MessageBoxValidation("Select organization(s).", "Validation")
            '    Exit Sub
            'End If


            rpbFilter.Items(0).Expanded = False
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

            Dim Sdate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")
            Dim Edate As String = DateTime.Parse(Me.EndTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")

            'If Me.EndTime.SelectedDate.Value.Month = 1 Or Me.EndTime.SelectedDate.Value.Month = 3 Or Me.EndTime.SelectedDate.Value.Month = 5 Or Me.EndTime.SelectedDate.Value.Month = 7 Or Me.EndTime.SelectedDate.Value.Month = 8 Or Me.EndTime.SelectedDate.Value.Month = 10 Or Me.EndTime.SelectedDate.Value.Month = 12 Then
            '    Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
            '    Edate = DateTime.Parse(Me.EndTime.SelectedDate.Value.Month.ToString() + "-31-" + Me.EndTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            'End If
            'If Me.EndTime.SelectedDate.Value.Month = 4 Or Me.EndTime.SelectedDate.Value.Month = 9 Or Me.EndTime.SelectedDate.Value.Month = 6 Or Me.StartTime.SelectedDate.Value.Month = 11 Then
            '    Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
            '    Edate = DateTime.Parse(Me.EndTime.SelectedDate.Value.Month.ToString() + "-30-" + Me.EndTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            'End If
            'If Me.EndTime.SelectedDate.Value.Month = 2 Then
            '    Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
            '    Edate = DateTime.Parse(Me.EndTime.SelectedDate.Value.Month.ToString() + "-28-" + Me.EndTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            'End If
            '  Edate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(Edate))).ToString("MM-dd-yyyy")
            Dim ObjReport As New SalesWorx.BO.Common.Reports

            Dim dt1 As New DataSet
            dt1 = ObjReport.GetMBRSummary(Err_No, Err_Desc, orgStr, VanStr, AgencyStr, Sdate, Edate)




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
            For Each r As DataRow In dt1.Tables(0).Rows
                If r("Mode").ToString() = "I" Then

                    For Each col As DataColumn In dt1.Tables(0).Columns
                        If col.ColumnName <> "Mode" And col.ColumnName <> "Description" Then
                            r(col.ColumnName) = CInt(IIf(r(col.ColumnName) Is DBNull.Value, "0", r(col.ColumnName)))
                        End If

                    Next
                ElseIf r("Mode").ToString() = "V" Then
                    For Each col As DataColumn In dt1.Tables(0).Columns
                        If col.ColumnName <> "Mode" And col.ColumnName <> "Description" Then
                            r(col.ColumnName) = CDec(IIf(r(col.ColumnName) Is DBNull.Value, "0", r(col.ColumnName))).ToString("#,##" & LabelDecimalDigits)
                        End If

                    Next
                End If
            Next

            For Each dr As DataRow In dt1.Tables(0).Rows
                If dr("Description").ToString = "Target Value" Or dr("Description").ToString = "Sales Value" Then
                    dr("Description") = dr("Description").ToString & "   *"
                End If
               
                If dr("Description").ToString = "JP Outlets" Or dr("Description").ToString = "Coverage" Or dr("Description").ToString = "Productive Outlets" Then
                    dr("Description") = dr("Description").ToString & "   **"
                End If
                If dr("Description").ToString = "Total calls" Or dr("Description").ToString = "Productive calls" Or dr("Description").ToString = "Drop size per call" Or dr("Description").ToString = "Average Outlet Billing" Or dr("Description").ToString = "JP Adherance %" Or dr("Description").ToString = "Outlet Productivity %" Or dr("Description").ToString = "Call Productivity %" Or dr("Description").ToString = "Growth over last year %" Or dr("Description").ToString = "Average calls per van" Or dr("Description").ToString = "Zero Billed outlets" Then
                    dr("Description") = dr("Description").ToString & "  ***"
                End If
            Next
            ViewState("Summary") = dt1.Tables(1).Copy

            gvSummary.DataSource = dt1
            gvSummary.DataBind()

            'If dt1.Rows.Count > 0 Then
            '    dt1.Rows.Clear()
            'End If



            Me.hfSMonth.Value = Sdate
            Me.hfEMonth.Value = Edate
            Me.hfOrgID.Value = orgStr
            Me.hfVans.Value = VanStr
            Me.hfAgency.Value = AgencyStr
            dt1 = Nothing

            ObjReport = Nothing

        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub
    Private Sub BindSummaryChart()
        Dim ds As New DataSet
        Dim dt_Summary As New DataTable
        If ViewState("Summary") Is Nothing Then
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

            'If String.IsNullOrEmpty(orgStr) Then
            '    MessageBoxValidation("Select organization(s).", "Validation")
            '    Exit Sub
            'End If


            rpbFilter.Items(0).Expanded = False
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

            Dim Sdate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")
            Dim Edate As String = DateTime.Parse(Me.EndTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")

            If Me.EndTime.SelectedDate.Value.Month = 1 Or Me.EndTime.SelectedDate.Value.Month = 3 Or Me.EndTime.SelectedDate.Value.Month = 5 Or Me.EndTime.SelectedDate.Value.Month = 7 Or Me.EndTime.SelectedDate.Value.Month = 8 Or Me.EndTime.SelectedDate.Value.Month = 10 Or Me.EndTime.SelectedDate.Value.Month = 12 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.EndTime.SelectedDate.Value.Month.ToString() + "-31-" + Me.EndTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            End If
            If Me.EndTime.SelectedDate.Value.Month = 4 Or Me.EndTime.SelectedDate.Value.Month = 9 Or Me.EndTime.SelectedDate.Value.Month = 6 Or Me.StartTime.SelectedDate.Value.Month = 11 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.EndTime.SelectedDate.Value.Month.ToString() + "-30-" + Me.EndTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            End If
            If Me.EndTime.SelectedDate.Value.Month = 2 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.EndTime.SelectedDate.Value.Month.ToString() + "-28-" + Me.EndTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            End If

            Dim ObjReport As New SalesWorx.BO.Common.Reports


            ds = ObjReport.GetMBRSummary(Err_No, Err_Desc, orgStr, VanStr, AgencyStr, Sdate, Edate)

            dt_Summary = ds.Tables(1)
        Else
            dt_Summary = CType(ViewState("Summary"), DataTable)
        End If

        Dim dtSummary As New DataTable
        dtSummary.Columns.Add("Month", System.Type.GetType("System.String"))
        dtSummary.Columns.Add("PercentCP", System.Type.GetType("System.Decimal"))
        dtSummary.Columns.Add("PercentGR", System.Type.GetType("System.Decimal"))
        dtSummary.Columns.Add("PercentJP", System.Type.GetType("System.Decimal"))
        dtSummary.Columns.Add("PercentOP", System.Type.GetType("System.Decimal"))


        Dim DtMonths As New DataTable
        DtMonths = dt_Summary.DefaultView.ToTable(True, "MnOrder")
        For Each sdr As DataRow In DtMonths.Rows
            If sdr("MnOrder").ToString.ToUpper <> "CUMULATIVE" Then

                Dim dr As DataRow
                dr = dtSummary.NewRow
                dr("Month") = sdr("MnOrder")

                Dim seldrCP() As DataRow
                Dim seldrGR() As DataRow
                Dim seldrJP() As DataRow
                Dim seldrOP() As DataRow
                seldrCP = dt_Summary.Select("MnOrder='" & sdr("MnOrder") & "' and Description='Call Productivity %'")
                If seldrCP.Length > 0 Then
                    dr("PercentCP") = seldrCP(0)("TotValue")
                Else
                    dr("PercentCP") = 0
                End If
                seldrGR = dt_Summary.Select("MnOrder='" & sdr("MnOrder") & "' and Description='Growth over last year %'")
                If seldrGR.Length > 0 Then
                    dr("PercentGR") = seldrGR(0)("TotValue")
                Else
                    dr("PercentGR") = 0
                End If

                seldrJP = dt_Summary.Select("MnOrder='" & sdr("MnOrder") & "' and Description='JP Adherance %'")
                If seldrJP.Length > 0 Then
                    dr("PercentJP") = seldrJP(0)("TotValue")
                Else
                    dr("PercentJP") = 0
                End If

                seldrOP = dt_Summary.Select("MnOrder='" & sdr("MnOrder") & "' and Description='Outlet Productivity %'")
                If seldrOP.Length > 0 Then
                    dr("PercentOP") = seldrOP(0)("TotValue")
                Else
                    dr("PercentOP") = 0
                End If

                dtSummary.Rows.Add(dr)
            End If
        Next
        RadHtmlSummary.PlotArea.Appearance.TextStyle.Margin = 10
        RadHtmlSummary.DataSource = dtSummary
        RadHtmlSummary.DataBind()

        dtSummary = Nothing
        DtMonths = Nothing
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

        If Me.EndTime.SelectedDate.Value < Me.StartTime.SelectedDate.Value Then
            MessageBoxValidation("Month to should be greater than month from", "Validation")
            Return bretval
        End If

        If Math.Abs(DateDiff(DateInterval.Month, Me.EndTime.SelectedDate.Value, Me.StartTime.SelectedDate.Value)) > 13 Then
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
    'Private Sub dummyOrgBtn_Click(sender As Object, e As EventArgs) Handles dummyOrgBtn.Click
    '    Dim objRep = Nothing
    '    Try
    '        Dim OrgStr As String = String.Empty

    '        For Each item As RadComboBoxItem In ddlOrganization.Items
    '            If item.Checked Then
    '                OrgStr = OrgStr & item.Value & ","
    '            End If
    '        Next

    '        BindCombo(OrgStr)
    '        If ValidateInputs() = True Then
    '            BindAgencySales()
    '            BindSummary()

    '            If AgencyTab.Tabs(2).Selected = True Then

    '                ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart1();", True)
    '            ElseIf AgencyTab.Tabs(0).Selected = True Then
    '                ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
    '            End If
    '        Else
    '            Me.gvAgency1.DataSource = Nothing
    '            Me.gvAgency1.DataBind()

    '            Me.gvSummary.DataSource = Nothing
    '            Me.gvSummary.DataBind()
    '            Dim Sdate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")
    '            Dim Edate As String = DateTime.Parse(Me.EndTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")

    '            If Me.EndTime.SelectedDate.Value.Month = 1 Or Me.EndTime.SelectedDate.Value.Month = 3 Or Me.EndTime.SelectedDate.Value.Month = 5 Or Me.EndTime.SelectedDate.Value.Month = 7 Or Me.EndTime.SelectedDate.Value.Month = 8 Or Me.EndTime.SelectedDate.Value.Month = 10 Or Me.EndTime.SelectedDate.Value.Month = 12 Then
    '                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
    '                Edate = DateTime.Parse(Me.EndTime.SelectedDate.Value.Month.ToString() + "-31-" + Me.EndTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

    '            End If
    '            If Me.EndTime.SelectedDate.Value.Month = 4 Or Me.EndTime.SelectedDate.Value.Month = 9 Or Me.EndTime.SelectedDate.Value.Month = 6 Or Me.StartTime.SelectedDate.Value.Month = 11 Then
    '                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
    '                Edate = DateTime.Parse(Me.EndTime.SelectedDate.Value.Month.ToString() + "-30-" + Me.EndTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

    '            End If
    '            If Me.EndTime.SelectedDate.Value.Month = 2 Then
    '                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
    '                Edate = DateTime.Parse(Me.EndTime.SelectedDate.Value.Month.ToString() + "-28-" + Me.EndTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

    '            End If

    '            Me.hfSMonth.Value = Sdate
    '            Me.hfEMonth.Value = Edate
    '            Me.hfOrgID.Value = "0"
    '            Me.hfVans.Value = "0"
    '            Me.hfAgency.Value = "0"
    '            If AgencyTab.Tabs(2).Selected = True Then

    '                ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart1();", True)
    '            ElseIf AgencyTab.Tabs(0).Selected = True Then
    '                ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Err_No = "7477866"
    '        If Err_Desc Is Nothing Then
    '            log.Error(GetExceptionInfo(ex))
    '        Else
    '            log.Error(Err_Desc)
    '        End If
    '        Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
    '    End Try
    'End Sub


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


            Dim Sdate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")
            Dim Edate As String = DateTime.Parse(Me.EndTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")

            'If Me.EndTime.SelectedDate.Value.Month = 1 Or Me.EndTime.SelectedDate.Value.Month = 3 Or Me.EndTime.SelectedDate.Value.Month = 5 Or Me.EndTime.SelectedDate.Value.Month = 7 Or Me.EndTime.SelectedDate.Value.Month = 8 Or Me.EndTime.SelectedDate.Value.Month = 10 Or Me.EndTime.SelectedDate.Value.Month = 12 Then
            '    Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
            '    Edate = DateTime.Parse(Me.EndTime.SelectedDate.Value.Month.ToString() + "-31-" + Me.EndTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            'End If
            'If Me.EndTime.SelectedDate.Value.Month = 4 Or Me.EndTime.SelectedDate.Value.Month = 9 Or Me.EndTime.SelectedDate.Value.Month = 6 Or Me.StartTime.SelectedDate.Value.Month = 11 Then
            '    Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
            '    Edate = DateTime.Parse(Me.EndTime.SelectedDate.Value.Month.ToString() + "-30-" + Me.EndTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            'End If
            'If Me.EndTime.SelectedDate.Value.Month = 2 And Me.EndTime.SelectedDate.Value.Year Mod 4 = 0 Then
            '    Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
            '    Edate = DateTime.Parse(Me.EndTime.SelectedDate.Value.Month.ToString() + "-29-" + Me.EndTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            'End If

            'If Me.EndTime.SelectedDate.Value.Month = 2 And Me.EndTime.SelectedDate.Value.Year Mod 4 <> 0 Then
            '    Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
            '    Edate = DateTime.Parse(Me.EndTime.SelectedDate.Value.Month.ToString() + "-28-" + Me.EndTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            'End If


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
            Me.lbl_from.Text = Me.StartTime.SelectedDate.Value.ToString("MMM-yyyy")
            Me.lbl_To.Text = Me.EndTime.SelectedDate.Value.ToString("MMM-yyyy")

            Me.lbl_Country.Text = Me.ddlCountry.SelectedItem.Text

            Args.Visible = True
            rpt.Visible = True

            Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
            rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            objRep = New Reports

            If objRep.GetTargetType(Err_No, Err_Desc) = "P" Then
                Me.ReportPath = "MBRReport_itemcode"
            ElseIf objRep.GetTargetType(Err_No, Err_Desc) = "B" Then
                Me.ReportPath = "MBRReport_brand"
            ElseIf objRep.GetTargetType(Err_No, Err_Desc) = "C" Then
                Me.ReportPath = "MBRReport_Category"
            Else
                Me.ReportPath = "MBRReport_agency"
            End If
            rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)



            Dim org As New ReportParameter
            org = New ReportParameter("OID", orgStr)


            Dim vans As New ReportParameter
            vans = New ReportParameter("VanList", VanStr)

            Dim agencyParam As New ReportParameter
            agencyParam = New ReportParameter("Agency", AgencyStr)

            Dim sdat As New ReportParameter
            sdat = New ReportParameter("FMonth", Sdate)

            Dim edat As New ReportParameter
            edat = New ReportParameter("TMonth", Edate)

            Dim SalesorgName As New ReportParameter
            SalesorgName = New ReportParameter("OrgName", CStr(Me.lbl_org.Text))


            Dim Mode As New ReportParameter
            Mode = New ReportParameter("Mode", "Report")


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
            

            Dim TeamSize As New ReportParameter
            TeamSize = New ReportParameter("TeamSize", CStr(vancnt))

            rview.ServerReport.SetParameters(New ReportParameter() {org, vans, agencyParam, sdat, edat, SalesorgName, Mode, Country, VanName, AgencyNameParam, TeamSize})

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
                Response.AddHeader("Content-disposition", "attachment;filename=MBROverall.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
                Response.ContentType = "application/pdf"
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=MBROverall.xls")
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

    Protected Sub AgencyTab_TabClick(sender As Object, e As RadTabStripEventArgs) Handles AgencyTab.TabClick
        If Args.Visible = True Then
            If AgencyTab.Tabs(2).Selected = True Then
                BindSummaryChart()
                'ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart1();", True)
            ElseIf AgencyTab.Tabs(0).Selected = True Then
                ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
            ElseIf AgencyTab.Tabs(1).Selected = True Then
               
                gvRep.DataSource = ViewState("AgencyTSales")
                gvRep.DataBind()
            End If
        End If
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
        StartTime.SelectedDate = year
        EndTime.SelectedDate = Now

        Me.lblC.Text = Me.hfCurrency.Value
        BindCombo(OrgStr)
        Args.Visible = False
        rpt.Visible = False

    End Sub
    Protected Sub gvRep_ItemCommand(sender As Object, e As GridCommandEventArgs)
        'BindSummaryFromViewState()
        If e.CommandName = RadGrid.ExportToExcelCommandName Then
            gvSummary.ExportSettings.Excel.Format = GridExcelExportFormat.Biff
            gvSummary.ExportSettings.IgnorePaging = True
            gvSummary.ExportSettings.ExportOnlyData = True
            gvSummary.ExportSettings.OpenInNewWindow = True
            gvSummary.ExportSettings.FileName = "MBRSummary"
        End If

    End Sub
     
    'Sub BindAgencySalesFromViewState()
    '    If Not ViewState("AgencySales") Is Nothing Then
    '        Dim dtv As New DataTable
    '        dtv = CType(ViewState("AgencySales"), DataTable)
    '        gvAgency1.DataSource = dtv
    '        gvAgency1.DataBind()
    '        dtv = Nothing
    '    Else
    '        BindAgencySales()
    '    End If
    'End Sub
    Sub BindTargetSalesFromViewState()
        If Not ViewState("TargetVSSales") Is Nothing Then
            Dim dtv As New DataTable
            dtv = CType(ViewState("TargetVSSales"), DataTable)
            gvtargetSales.DataSource = dtv
            gvtargetSales.DataBind()
            dtv = Nothing
        Else
            BIndTargetVsSales()
        End If
    End Sub
    Sub BindSummaryFromViewState()
        If Not ViewState("Summary") Is Nothing Then
            Dim dtv As New DataTable
            dtv = CType(ViewState("Summary"), DataTable)
            gvSummary.DataSource = dtv
            gvSummary.DataBind()
            dtv = Nothing
        Else
            BindSummary()
        End If
    End Sub
    Protected Sub gvtargetSales_ItemCommand(sender As Object, e As GridCommandEventArgs)

        If e.CommandName = RadGrid.ExportToExcelCommandName Then
            gvtargetSales.ExportSettings.Excel.Format = GridExcelExportFormat.Biff
            gvtargetSales.ExportSettings.IgnorePaging = True
            gvtargetSales.ExportSettings.ExportOnlyData = True
            gvtargetSales.ExportSettings.OpenInNewWindow = True
            gvtargetSales.ExportSettings.FileName = "TargetVsSales"
        End If

    End Sub
    Private Sub BtnExportBiffExcel_Click(sender As Object, e As EventArgs) Handles BtnExportBiffExcel.Click
        Dim dtv As New DataTable
        If Not ViewState("AgencySales") Is Nothing Then
            dtv = CType(ViewState("AgencySales"), DataTable)
        Else
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

            'If String.IsNullOrEmpty(orgStr) Then
            '    MessageBoxValidation("Select organization(s).", "Validation")
            '    Exit Sub
            'End If


            rpbFilter.Items(0).Expanded = False
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

            Dim Sdate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")
            Dim Edate As String = DateTime.Parse(Me.EndTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")

            If Me.EndTime.SelectedDate.Value.Month = 1 Or Me.EndTime.SelectedDate.Value.Month = 3 Or Me.EndTime.SelectedDate.Value.Month = 5 Or Me.EndTime.SelectedDate.Value.Month = 7 Or Me.EndTime.SelectedDate.Value.Month = 8 Or Me.EndTime.SelectedDate.Value.Month = 10 Or Me.EndTime.SelectedDate.Value.Month = 12 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.EndTime.SelectedDate.Value.Month.ToString() + "-31-" + Me.EndTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            End If
            If Me.EndTime.SelectedDate.Value.Month = 4 Or Me.EndTime.SelectedDate.Value.Month = 9 Or Me.EndTime.SelectedDate.Value.Month = 6 Or Me.StartTime.SelectedDate.Value.Month = 11 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.EndTime.SelectedDate.Value.Month.ToString() + "-30-" + Me.EndTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            End If
            If Me.EndTime.SelectedDate.Value.Month = 2 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.EndTime.SelectedDate.Value.Month.ToString() + "-28-" + Me.EndTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            End If

            Dim ObjReport As New SalesWorx.BO.Common.Reports


            dtv = ObjReport.GetMBRTargetvsSalesByMonths(Err_No, Err_Desc, orgStr, VanStr, AgencyStr, Sdate, Edate)

            
            ObjReport = Nothing

            
        End If

        Dim dtTargetVSSales As New DataTable
        dtTargetVSSales.Columns.Add("Agency", System.Type.GetType("System.String"))
        Dim s_target As Decimal = 0
        Dim s_Sales As Decimal = 0

        Dim DtM As New DataTable
        DtM = dtv.DefaultView.ToTable(True, "MnOrder")

       
        For Each sdr As DataRow In DtM.Rows
            dtTargetVSSales.Columns.Add(sdr("MnOrder").ToString & "Target", System.Type.GetType("System.Decimal"))
            dtTargetVSSales.Columns.Add(sdr("MnOrder").ToString & "Sales", System.Type.GetType("System.Decimal"))
        Next


        Dim DtMonths As New DataTable
        DtMonths = dtv.DefaultView.ToTable(True, "Agency")

        For Each sdr As DataRow In DtMonths.Rows
            Dim dr As DataRow
            dr = dtTargetVSSales.NewRow
            dr("Agency") = sdr("Agency")
            For Each monthdr As DataRow In DtM.Rows
                Dim seldr() As DataRow
                seldr = dtv.Select("MnOrder='" & monthdr("MnOrder") & "' and Agency='" & sdr("Agency") & "'")
                Dim seldrTarget() As DataRow
                Dim seldrSales() As DataRow

                seldrTarget = dtv.Select("MnOrder='" & monthdr("MnOrder") & "' and Agency='" & sdr("Agency") & "' and Description=' Target'")
                If seldrTarget.Length > 0 Then
                    dr(monthdr("MnOrder").ToString & "Target") = seldrTarget(0)("TotValue").ToString

                Else
                    dr(monthdr("MnOrder").ToString & "Target") = 0
                End If
                seldrSales = dtv.Select("MnOrder='" & monthdr("MnOrder") & "' and Agency='" & sdr("Agency") & "' and Description='Sales'")
                If seldrSales.Length > 0 Then
                    dr(monthdr("MnOrder").ToString & "Sales") = seldrSales(0)("TotValue").ToString

                Else
                    dr(monthdr("MnOrder").ToString & "Sales") = 0
                End If

            Next
            dtTargetVSSales.Rows.Add(dr)
        Next
      
        gvRep.DataSource = dtTargetVSSales
        gvRep.DataBind()
        DtMonths = Nothing
        If dtTargetVSSales.Rows.Count > 0 Then
          
            Using package As New ExcelPackage()
                ' add a new worksheet to the empty workbook
                Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
                Worksheet.Cells("A2").LoadFromDataTable(dtTargetVSSales, True)
                Worksheet.Cells.AutoFitColumns()


                Dim j As String = 2

                For i = 0 To DtM.Rows.Count - 1

                    Worksheet.Cells(2, j).Value = "Target"
                    Worksheet.Cells(1, j).Value = DtM.Rows(i)("MnOrder").ToString
                    Worksheet.Cells(1, j).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center
                    Worksheet.Cells(1, j, 1, j + 1).Merge = True
                    Worksheet.Cells(2, j + 1).Value = "Sales"
                    j = j + 2
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