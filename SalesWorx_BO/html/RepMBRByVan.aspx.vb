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


Public Class RepMBRByVan
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As New SalesWorx.BO.Common.Common
    Dim ObjCollection As Collection
    Private ReportPath As String = "MBRByVan"
    Private Const PageID As String = "P370"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objRep As New Reports
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())

            Dim CountryTbl As DataTable = Nothing
            Dim orgTbl As DataTable = Nothing
            Try

                HTargetType.Value = CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE

                ObjCommon = New SalesWorx.BO.Common.Common()


                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)

                CountryTbl = ObjCommon.GetCountry(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)
                ddlCountry.DataSource = CountryTbl
                ddlCountry.DataBind()
                hfUID.Value = CType(Session("User_Access"), UserAccess).UserID

                orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
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

                StartTime.SelectedDate = Now


                Me.lblC.Text = Me.hfCurrency.Value
                BindCombo(OrgStr)





                If CType(Session.Item("CONTROL_PARAMS"), ControlParams).FSR_TARGET_TYPE = "A" Then
                    AgencyTab.Tabs(2).Text = "Target vs Sales By Van/Agency"
                    gvAgency1.Columns(0).HeaderText = "Agency"
                ElseIf CType(Session.Item("CONTROL_PARAMS"), ControlParams).FSR_TARGET_TYPE = "K" Then

                    AgencyTab.Tabs(2).Text = "Target vs Sales By Van/Category"
                    gvAgency1.Columns(0).HeaderText = "Category"

                ElseIf CType(Session.Item("CONTROL_PARAMS"), ControlParams).FSR_TARGET_TYPE = "B" Then

                    AgencyTab.Tabs(2).Text = "Target vs Sales By Van/Brand"
                    gvAgency1.Columns(0).HeaderText = "Brand"
                ElseIf CType(Session.Item("CONTROL_PARAMS"), ControlParams).FSR_TARGET_TYPE = "P" Then

                    AgencyTab.Tabs(2).Text = "Target vs Sales By Van/SKU"
                    gvAgency1.Columns(0).HeaderText = "SKU"
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
            'Else
            '    ZeroBilledWindow.VisibleOnPageLoad = False
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
            ' BindAgencySales1()
            ' BindSummary()
            rpt.Visible = True
            If AgencyTab.Tabs(0).Selected = True Then
                ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
            End If

        Else
            Args.Visible = False
            rpt.Visible = False

            Me.gvAgency1.DataSource = Nothing
            Me.gvAgency1.DataBind()

            Me.gvAgency3.DataSource = Nothing
            Me.gvAgency3.DataBind()

            Me.gvSummary.DataSource = Nothing
            Me.gvSummary.DataBind()
            Dim Sdate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")
            Dim Edate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")

            If Me.StartTime.SelectedDate.Value.Month = 1 Or Me.StartTime.SelectedDate.Value.Month = 3 Or Me.StartTime.SelectedDate.Value.Month = 5 Or Me.StartTime.SelectedDate.Value.Month = 7 Or Me.StartTime.SelectedDate.Value.Month = 8 Or Me.StartTime.SelectedDate.Value.Month = 10 Or Me.StartTime.SelectedDate.Value.Month = 12 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-31-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            End If
            If Me.StartTime.SelectedDate.Value.Month = 4 Or Me.StartTime.SelectedDate.Value.Month = 9 Or Me.StartTime.SelectedDate.Value.Month = 6 Or Me.StartTime.SelectedDate.Value.Month = 11 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-30-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            End If
            If Me.StartTime.SelectedDate.Value.Month = 2 And Me.StartTime.SelectedDate.Value.Year Mod 4 = 0 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-29-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            End If

            If Me.StartTime.SelectedDate.Value.Month = 2 And Me.StartTime.SelectedDate.Value.Year Mod 4 <> 0 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-28-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            End If

            Me.hfSMonth.Value = Sdate
            Me.hfEMonth.Value = Edate
            Me.hfOrgID.Value = "0"
            Me.hfVans.Value = "0"
            Me.hfAgency.Value = "0"
            If AgencyTab.Tabs(0).Selected = True Then
                ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
            End If

        End If
    End Sub



    Private Sub gvAgency1_PreRender(sender As Object, e As EventArgs) Handles gvAgency1.PreRender
        For Each column As GridColumn In gvAgency1.MasterTableView.AutoGeneratedColumns

            If column.UniqueName = "Agy" Then
                column.Visible = False
            ElseIf column.UniqueName = "Description" Then
                column.HeaderText = "Agency"

            End If
        Next

        For Each column As GridColumn In gvAgency1.MasterTableView.AutoGeneratedColumns
            If column.UniqueName = "Description" Or column.UniqueName = "Agy" Or column.UniqueName = "Agency" Then
                column.ItemStyle.HorizontalAlign = HorizontalAlign.Left
                column.HeaderStyle.HorizontalAlign = HorizontalAlign.Left
                column.ItemStyle.Wrap = False
                column.HeaderStyle.Wrap = False
            Else
                column.ItemStyle.HorizontalAlign = HorizontalAlign.Right
                column.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                column.ItemStyle.Wrap = False
                column.HeaderStyle.Wrap = False
            End If
        Next
    End Sub
    Private Sub gvAgency3_PreRender(sender As Object, e As EventArgs) Handles gvAgency1.PreRender
        For Each column As GridColumn In gvAgency3.MasterTableView.AutoGeneratedColumns

            If column.UniqueName = "Agy" Then
                column.Visible = False
            ElseIf column.UniqueName = "Description" Then
                column.HeaderText = "Agency"

            End If
        Next

        For Each column As GridColumn In gvAgency3.MasterTableView.AutoGeneratedColumns
            If column.UniqueName = "Description" Or column.UniqueName = "Agy" Or column.UniqueName = "Agency" Then
                column.ItemStyle.HorizontalAlign = HorizontalAlign.Left
                column.HeaderStyle.HorizontalAlign = HorizontalAlign.Left
                column.ItemStyle.Wrap = False
                column.HeaderStyle.Wrap = False
            Else
                column.ItemStyle.HorizontalAlign = HorizontalAlign.Right
                column.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                column.ItemStyle.Wrap = False
                column.HeaderStyle.Wrap = False
            End If
        Next
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
                    If dataItem.Cells(3).Text = "Target Value" Or dataItem.Cells(3).Text = "Sales Value" Or dataItem.Cells(3).Text = "Drop size per call" Or dataItem.Cells(3).Text = "Average Outlet Billing" Or dataItem.Cells(3).Text = "Order Value" Or dataItem.Cells(3).Text = "Return Value" Or dataItem.Cells(3).Text = "Collection Value" Or dataItem.Cells(3).Text = "Avg.Order Value" Or dataItem.Cells(3).Text = "Avg.Return Value" Or dataItem.Cells(3).Text = "Avg.Collection Value" Then
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
                column.HeaderStyle.Wrap = False
            End If

        Next
    End Sub
    Private Sub gvAgency1_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvAgency1.PageIndexChanged
        BindAgencySales()
    End Sub

    'Private Sub gvAgency3_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvAgency3.PageIndexChanged
    '    BindAgencySales1()
    'End Sub

    Private Sub gvSummary_SortCommand(sender As Object, e As GridSortCommandEventArgs) Handles gvSummary.SortCommand
        ViewState("SortField1") = e.SortExpression
        SortDirection1 = "flip"
        BindSummary()
    End Sub
    Private Sub gvAgency1_SortCommand(sender As Object, e As GridSortCommandEventArgs) Handles gvAgency1.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindAgencySales()
    End Sub

    'Private Sub gvAgency3_SortCommand(sender As Object, e As GridSortCommandEventArgs) Handles gvAgency3.SortCommand
    '    ViewState("SortField2") = e.SortExpression
    '    SortDirection2 = "flip"
    '    BindAgencySales1()
    'End Sub

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
    Private Property SortDirection2() As String
        Get
            If ViewState("SortDirection2") Is Nothing Then
                ViewState("SortDirection2") = "ASC"
            End If
            Return ViewState("SortDirection2").ToString()
        End Get
        Set(ByVal value As String)
            Dim s As String = SortDirection2

            If value = "flip" Then
                s = If(s = "ASC", "DESC", "ASC")
            Else
                s = value
            End If

            ViewState("SortDirection2") = s
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
            Dim Edate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")

            If Me.StartTime.SelectedDate.Value.Month = 1 Or Me.StartTime.SelectedDate.Value.Month = 3 Or Me.StartTime.SelectedDate.Value.Month = 5 Or Me.StartTime.SelectedDate.Value.Month = 7 Or Me.StartTime.SelectedDate.Value.Month = 8 Or Me.StartTime.SelectedDate.Value.Month = 10 Or Me.StartTime.SelectedDate.Value.Month = 12 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-31-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            End If
            If Me.StartTime.SelectedDate.Value.Month = 4 Or Me.StartTime.SelectedDate.Value.Month = 9 Or Me.StartTime.SelectedDate.Value.Month = 6 Or Me.StartTime.SelectedDate.Value.Month = 11 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-30-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            End If
            If Me.StartTime.SelectedDate.Value.Month = 2 And Me.StartTime.SelectedDate.Value.Year Mod 4 = 0 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-29-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            End If

            If Me.StartTime.SelectedDate.Value.Month = 2 And Me.StartTime.SelectedDate.Value.Year Mod 4 <> 0 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-28-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

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


            Me.lbl_Country.Text = Me.ddlCountry.SelectedItem.Text

            Args.Visible = True
            rpt.Visible = True
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetMBRByVan(Err_No, Err_Desc, orgStr, VanStr, AgencyStr, Sdate, Edate)

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




            'summary.InnerHtml = "<h5 class='text-right'>Currency <span class='text-blue'><strong>" & hfCurrency.Value & "</strong></span></h5>"
            'Dim tblstr As String = ""
            'If dt.Rows.Count Then
            '    Dim query = (From UserEntry In dt _
            '                 Group UserEntry By key = UserEntry.Field(Of String)("Agency") Into Group _
            '                 Select Supervisor = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("TargetValue"))).ToList



            '    Dim fromdate As DateTime
            '    fromdate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), CDate(StartTime.SelectedDate))

            '    Dim todate As DateTime
            '    todate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, DateAdd(DateInterval.Day, -1 * (Now.Day - 1), CDate(EndTime.SelectedDate)))).ToString("dd-MMM-yyyy")

            '    tblstr = "<div class='overflowx'><table class='table table-bordered table-text-wrap table-th-center'>"
            '    Dim trHdstr = "<tr><th></th>"
            '    Dim trDatastr = ""

            '    Dim tfromdate As DateTime
            '    tfromdate = fromdate

            '    While tfromdate <= todate
            '        trHdstr = trHdstr & "<th colspan='2'>" & tfromdate.ToString("MMM-yyyy") & "</th>"
            '        tfromdate = DateAdd(DateInterval.Month, 1, tfromdate)
            '    End While

            '    trHdstr = trHdstr & "</tr>"

            '    tblstr = tblstr & trHdstr

            '    trHdstr = "<tr><th></th>"

            '    tfromdate = fromdate

            '    While tfromdate <= todate
            '        trHdstr = trHdstr & "<th>Target</td>"
            '        trHdstr = trHdstr & "<th>Sales</td>"

            '        tfromdate = DateAdd(DateInterval.Month, 1, tfromdate)
            '    End While
            '    trHdstr = trHdstr & "</tr>"


            '    tblstr = tblstr & trHdstr



            '    For Each x In query
            '        trDatastr = trDatastr & "<tr><td class='font-bold text-wrap'>" & x.Supervisor & "</td>"
            '        tfromdate = fromdate
            '        While tfromdate <= todate
            '            Dim seldr() As DataRow
            '            seldr = dt.Select("Agency='" & x.Supervisor & "' and m=" & tfromdate.Month & " and yr=" & tfromdate.Year)
            '            If seldr.Length > 0 Then
            '                trDatastr = trDatastr & "<td class='text-right'>" & Format(Val(seldr(0)("TargetValue").ToString), hfDecimal.Value) & "</td>"
            '                trDatastr = trDatastr & "<td class='text-right'>" & Format(Val(seldr(0)("SalesValue").ToString), hfDecimal.Value) & "</td>"

            '            Else
            '                trDatastr = trDatastr & "<td class='text-right'>" & Format(0, hfDecimal.Value) & "</td>"
            '                trDatastr = trDatastr & "<td class='text-right'>" & Format(0, hfDecimal.Value) & "</td>"

            '            End If
            '            tfromdate = DateAdd(DateInterval.Month, 1, tfromdate)
            '        End While
            '        trDatastr = trDatastr & "</tr>"
            '    Next
            '    tblstr = tblstr & trDatastr & "</table></div>"
            'End If
            'summary.InnerHtml = summary.InnerHtml & tblstr





            'Me.lblSales.Text = "0"
            'Me.lblTarget.Text = "0"
            Me.lblTeamSize.Text = "0"

            Me.lblTeamSize.Text = vancnt

            If HTargetType.Value = "Q" Then
                Me.lblTargetCurr.Text = "Qty"
                Me.lblSalesCurr.Text = "Qty"
            Else
                Me.lblTargetCurr.Text = hfCurrency.Value
                Me.lblSalesCurr.Text = hfCurrency.Value
            End If



            For Each r As DataRow In dt.Rows



                Me.lblSales.Text = Val(Me.lblSales.Text) + Val(r(3).ToString())


                Me.lblTarget.Text = Val(Me.lblTarget.Text) + Val(r(2).ToString())



                'For Each col As DataColumn In dt.Columns
                '    If col.ColumnName <> "Agy" And col.ColumnName <> "Description" And col.ColumnName <> "Agency" Then
                '        r(col.ColumnName) = CDec(IIf(r(col.ColumnName) Is DBNull.Value, "0", r(col.ColumnName))).ToString("#,##" & LabelDecimalDigits)
                '    End If

                'Next

            Next
            ' Me.lblTarget.Text = CDec(Me.lblTarget.Text).ToString("#,##" & LabelDecimalDigits)
            ' Me.lblSales.Text = CDec(Me.lblSales.Text).ToString("#,##" & LabelDecimalDigits)
            '  For Each r As DataRow In dt.Rows


            'For Each col As DataColumn In dt.Columns
            '    If col.ColumnName = "Description" Then
            '        col.ColumnName = "Agency"
            '    End If

            'Next

            'For Each col As DataColumn In dt.Columns
            '    If col.ColumnName <> "Description" And col.ColumnName <> "Agency" And col.ColumnName <> "Agy" Then
            '        col.ColumnName = IIf(DateTime.Parse(col.ColumnName).ToString("dd-MM-yyyy") = "01-01-1900", "YTD", DateTime.Parse(col.ColumnName).ToString("MMM-yyyy"))
            '    End If

            'Next

            '  Next
            If HTargetType.Value <> "Q" Then
                CType(gvAgency1.Columns(1), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvAgency1.Columns(2), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"



                CType(gvAgency1.Columns(1), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvAgency1.Columns(2), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvAgency1.Columns(3), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"

                Me.lblTarget.Text = Format(Val(Me.lblTarget.Text), "#,##" & LabelDecimalDigits)
                Me.lblSales.Text = Format(Val(Me.lblSales.Text),"#,##" & LabelDecimalDigits)

            Else
                CType(gvAgency1.Columns(1), GridBoundColumn).DataFormatString = "{0:N2}"
                CType(gvAgency1.Columns(2), GridBoundColumn).DataFormatString = "{0:N2}"



                CType(gvAgency1.Columns(1), GridBoundColumn).FooterAggregateFormatString = "{0:N2}"
                CType(gvAgency1.Columns(2), GridBoundColumn).FooterAggregateFormatString = "{0:N2}"
                CType(gvAgency1.Columns(3), GridBoundColumn).FooterAggregateFormatString = "{0:N2}"

                Me.lblTarget.Text = Format(Val(Me.lblTarget.Text), "#,##.00")
                Me.lblSales.Text = Format(Val(Me.lblSales.Text), "#,##.00")
            End If
            ' CType(gvAgency1.Columns(3), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"

            If HTargetType.Value = "Q" Then
                gvAgency1.Columns(1).HeaderText = "Target Qty"
                gvAgency1.Columns(2).HeaderText = "Sales Qty"
                gvAgency1.Columns(3).Visible = False
            Else
                gvAgency1.Columns(1).HeaderText = "Target Value"
                gvAgency1.Columns(2).HeaderText = "Sales Value"
                gvAgency1.Columns(3).HeaderText = "Sales Qty"
                gvAgency1.Columns(3).Visible = True
            End If

            gvAgency1.DataSource = dv
            gvAgency1.DataBind()

            'If dt.Rows.Count > 0 Then
            '    dt.Rows.Clear()
            'End If

            Me.hfSMonth.Value = Sdate
            Me.hfEMonth.Value = Edate
            Me.hfOrgID.Value = orgStr
            Me.hfVans.Value = VanStr
            Me.hfAgency.Value = AgencyStr
            gvAgency1.Rebind()
        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub
    'Protected Sub lblBilled_Click(sender As Object, e As EventArgs)
    '    Try

    '        Dim lblBilled As LinkButton = TryCast(sender, LinkButton)
    '        Dim row As GridDataItem = DirectCast(lblBilled.NamingContainer, GridDataItem)
    '        Dim SID As Label = DirectCast(row.FindControl("lblVanID"), Label)
    '        hfSelVan.Value = SID.Text
    '        hfItem.Value = row.Cells(1).Text
    '        gvRep.Rebind()
    '        ZeroBilledWindow.VisibleOnPageLoad = True

    '    Catch ex As Exception
    '        Err_No = "74066"
    '        log.Error(Err_Desc)
    '        Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_BillCuts_006") & "&next=RepMBRByVan.aspx&Title=Message", False)
    '        If Err_Desc Is Nothing Then
    '            log.Error(GetExceptionInfo(ex))
    '        Else
    '            log.Error(Err_Desc)
    '        End If
    '    End Try
    'End Sub
    'Private Sub BindAgencySales1()
    '    Try
    '        Dim SearchQuery As String = ""
    '        Dim orgStr As String = ""
    '        Dim orgname As String = ""
    '        Dim orgcnt As Integer = 0
    '        For Each item As RadComboBoxItem In ddlOrganization.Items
    '            If item.Checked Then
    '                orgStr = orgStr & item.Value & ","
    '                orgname = orgname & item.Text & ","
    '                orgcnt = orgcnt + 1
    '            End If
    '        Next


    '        'If String.IsNullOrEmpty(orgStr) Then
    '        '    MessageBoxValidation("Select organization(s).", "Validation")
    '        '    Exit Sub
    '        'End If


    '        rpbFilter.Items(0).Expanded = False
    '        Dim objUserAccess As UserAccess
    '        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)



    '        Dim AgencyStr As String = ""
    '        Dim VanStr As String = ""
    '        Dim vancnt As Integer = 0
    '        Dim vantxt As String = ""
    '        Dim AgencyCnt As Integer = 0

    '        For Each item As RadComboBoxItem In ddlVan.Items
    '            If item.Checked Then
    '                vancnt = vancnt + 1
    '                VanStr = VanStr & item.Value & ","
    '                vantxt = vantxt & item.Text & ","
    '            End If
    '        Next


    '        If vantxt <> "" Then
    '            vantxt = vantxt.Substring(0, vantxt.Length - 1)
    '        End If
    '        If VanStr = "" Then
    '            VanStr = "0"
    '        End If
    '        If vancnt = ddlVan.Items.Count Then
    '            lbl_van.Text = "All"
    '        Else
    '            lbl_van.Text = vantxt
    '        End If



    '        For Each item As RadComboBoxItem In ddlAgency.Items
    '            If item.Checked Then
    '                AgencyCnt = AgencyCnt + 1
    '                AgencyStr = AgencyStr & item.Value & ","
    '            End If
    '        Next

    '        Dim Sdate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")
    '        Dim Edate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")

    '        If Me.StartTime.SelectedDate.Value.Month = 1 Or Me.StartTime.SelectedDate.Value.Month = 3 Or Me.StartTime.SelectedDate.Value.Month = 5 Or Me.StartTime.SelectedDate.Value.Month = 7 Or Me.StartTime.SelectedDate.Value.Month = 8 Or Me.StartTime.SelectedDate.Value.Month = 10 Or Me.StartTime.SelectedDate.Value.Month = 12 Then
    '            Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
    '            Edate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-31-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

    '        End If
    '        If Me.StartTime.SelectedDate.Value.Month = 4 Or Me.StartTime.SelectedDate.Value.Month = 9 Or Me.StartTime.SelectedDate.Value.Month = 6 Or Me.StartTime.SelectedDate.Value.Month = 11 Then
    '            Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
    '            Edate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-30-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

    '        End If
    '        If Me.StartTime.SelectedDate.Value.Month = 2 And Me.StartTime.SelectedDate.Value.Year Mod 4 = 0 Then
    '            Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
    '            Edate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-29-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

    '        End If

    '        If Me.StartTime.SelectedDate.Value.Month = 2 And Me.StartTime.SelectedDate.Value.Year Mod 4 <> 0 Then
    '            Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
    '            Edate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-28-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

    '        End If



    '        Dim Agency As String = ""
    '        If AgencyStr <> "" Then
    '            AgencyStr = AgencyStr.Substring(0, AgencyStr.Length - 1)
    '        End If
    '        If AgencyStr = "" Then
    '            AgencyStr = "0"
    '        End If
    '        If AgencyCnt = ddlAgency.Items.Count Then
    '            lbl_Agency.Text = "All"
    '        Else
    '            lbl_Agency.Text = AgencyStr
    '        End If



    '        If orgname <> "" Then
    '            orgname = orgname.Substring(0, orgname.Length - 1)
    '        End If
    '        If orgname = "" Then
    '            orgname = "0"
    '        End If
    '        If orgcnt = ddlOrganization.Items.Count Then
    '            lbl_org.Text = "All"
    '        Else
    '            lbl_org.Text = orgname
    '        End If
    '        Me.lbl_from.Text = Me.StartTime.SelectedDate.Value.ToString("MMM-yyyy")


    '        Me.lbl_Country.Text = Me.ddlCountry.SelectedItem.Text

    '        Args.Visible = True
    '        rpt.Visible = True
    '        Dim ObjReport As New SalesWorx.BO.Common.Reports
    '        Dim dt As New DataTable
    '        dt = ObjReport.GetMBRByAgencySummary(Err_No, Err_Desc, orgStr, VanStr, AgencyStr, Sdate, Edate)



    '        Dim dv As New DataView(dt)


    '        Dim LabelDecimalDigits As String = "0.00"
    '        If Me.hfDecimal.Value = 0 Then
    '            LabelDecimalDigits = "0"
    '        ElseIf Me.hfDecimal.Value = 1 Then
    '            LabelDecimalDigits = "0.0"
    '        ElseIf Me.hfDecimal.Value = 2 Then
    '            LabelDecimalDigits = "0.00"
    '        ElseIf Me.hfDecimal.Value = 3 Then
    '            LabelDecimalDigits = "0.000"
    '        ElseIf Me.hfDecimal.Value >= 4 Then
    '            LabelDecimalDigits = "0.0000"
    '        End If







    '        Me.lblSales.Text = "0"
    '        Me.lblTarget.Text = "0"
    '        Me.lblTeamSize.Text = "0"

    '        Me.lblTeamSize.Text = vancnt
    '        Me.lblTargetCurr.Text = hfCurrency.Value
    '        Me.lblSalesCurr.Text = hfCurrency.Value






    '        For Each r As DataRow In dt.Rows



    '            Me.lblSales.Text = CDec(Me.lblSales.Text) + CDec(r(2).ToString())


    '            Me.lblTarget.Text = CDec(Me.lblTarget.Text) + CDec(r(1).ToString())

    '        Next
    '        Me.lblTarget.Text = CDec(Me.lblTarget.Text).ToString("#,##" & LabelDecimalDigits)
    '        Me.lblSales.Text = CDec(Me.lblSales.Text).ToString("#,##" & LabelDecimalDigits)


    '        CType(gvAgency3.Columns(1), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
    '        CType(gvAgency3.Columns(2), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"



    '        CType(gvAgency3.Columns(1), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
    '        CType(gvAgency3.Columns(2), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"


    '        gvAgency3.DataSource = dv
    '        gvAgency3.DataBind()

    '        'If dt.Rows.Count > 0 Then
    '        '    dt.Rows.Clear()
    '        'End If

    '        Me.hfSMonth.Value = Sdate
    '        Me.hfEMonth.Value = Edate
    '        Me.hfOrgID.Value = orgStr
    '        Me.hfVans.Value = VanStr
    '        Me.hfAgency.Value = AgencyStr

    '    Catch Ex As Exception
    '        log.Error(Ex.Message)
    '    End Try
    'End Sub
    Protected Sub gvAgency1_ItemDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles gvAgency1.ItemDataBound
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
        If TypeOf e.Item Is GridGroupHeaderItem Then
            Dim groupHeader As GridGroupHeaderItem = DirectCast(e.Item, GridGroupHeaderItem)
            If True Then
                groupHeader.DataCell.Text = groupHeader.DataCell.Text.Split(":"c)(1).ToString()
                If groupHeader.DataCell.Text.Trim = "-1" Then
                    groupHeader.DataCell.Text = "N/A"
                End If
            End If
        End If

        If TypeOf e.Item Is GridFooterItem Then
            Dim footerItem As GridFooterItem = DirectCast(e.Item, GridFooterItem)
            Me.lblSales.Text = "0"
            Me.lblTarget.Text = "0"
            Dim strtarget As String = footerItem("Target").Text
            Dim strSales As String = footerItem("Sales").Text
            Me.lblTarget.Text = CDec(IIf(strtarget = "", "0", strtarget)).ToString("#,##" & LabelDecimalDigits)
            Me.lblSales.Text = CDec(IIf(strSales = "", "0", strSales)).ToString("#,##" & LabelDecimalDigits)
            
        End If

        If TypeOf e.Item Is GridDataItem Then
            Dim dataItem As GridDataItem = DirectCast(e.Item, GridDataItem)
            For i As Integer = 0 To dataItem.Cells.Count - 1
                If IsNumeric(dataItem.Cells(i).Text) Then
                    dataItem.Cells(i).Text = CDec(IIf(dataItem.Cells(i).Text = "" Or dataItem.Cells(i).Text Is Nothing, "0", dataItem.Cells(i).Text)).ToString("#,##" & LabelDecimalDigits)
                End If
            Next


        End If
    End Sub
    Protected Sub gvAgency3_ItemDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles gvAgency3.ItemDataBound
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
        If TypeOf e.Item Is GridGroupHeaderItem Then
            Dim groupHeader As GridGroupHeaderItem = DirectCast(e.Item, GridGroupHeaderItem)
            If True Then
                groupHeader.DataCell.Text = groupHeader.DataCell.Text.Split(":"c)(1).ToString()
                If groupHeader.DataCell.Text.Trim = "-1" Then
                    groupHeader.DataCell.Text = "N/A"
                End If
            End If
        End If
        If TypeOf e.Item Is GridDataItem Then
            Dim dataItem As GridDataItem = DirectCast(e.Item, GridDataItem)
            For i As Integer = 0 To dataItem.Cells.Count - 1
                If IsNumeric(dataItem.Cells(i).Text) Then
                    dataItem.Cells(i).Text = CDec(IIf(dataItem.Cells(i).Text = "" Or dataItem.Cells(i).Text Is Nothing, "0", dataItem.Cells(i).Text)).ToString("#,##" & LabelDecimalDigits)
                End If
            Next


        End If
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
            Dim Edate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")

            If Me.StartTime.SelectedDate.Value.Month = 1 Or Me.StartTime.SelectedDate.Value.Month = 3 Or Me.StartTime.SelectedDate.Value.Month = 5 Or Me.StartTime.SelectedDate.Value.Month = 7 Or Me.StartTime.SelectedDate.Value.Month = 8 Or Me.StartTime.SelectedDate.Value.Month = 10 Or Me.StartTime.SelectedDate.Value.Month = 12 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-31-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            End If
            If Me.StartTime.SelectedDate.Value.Month = 4 Or Me.StartTime.SelectedDate.Value.Month = 9 Or Me.StartTime.SelectedDate.Value.Month = 6 Or Me.StartTime.SelectedDate.Value.Month = 11 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-30-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            End If
            If Me.StartTime.SelectedDate.Value.Month = 2 And Me.StartTime.SelectedDate.Value.Year Mod 4 = 0 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-29-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            End If

            If Me.StartTime.SelectedDate.Value.Month = 2 And Me.StartTime.SelectedDate.Value.Year Mod 4 <> 0 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-28-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            End If

            Dim ObjReport As New SalesWorx.BO.Common.Reports

            Dim dt1 As New DataTable
            dt1 = ObjReport.GetMBRSummaryByVan(Err_No, Err_Desc, orgStr, VanStr, AgencyStr, Sdate, Edate)

            'Session.Remove("dtMBRStat")
            'Session("dtMBRStat") = dt1.Copy



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
            For Each r As DataRow In dt1.Rows
                If r("Mode").ToString() = "I" Then

                    For Each col As DataColumn In dt1.Columns
                        If col.ColumnName <> "Mode" And col.ColumnName <> "Description" Then
                            r(col.ColumnName) = CInt(IIf(r(col.ColumnName) Is DBNull.Value, "0", r(col.ColumnName)))
                        End If

                    Next
                ElseIf r("Mode").ToString() = "V" Then
                    For Each col As DataColumn In dt1.Columns
                        If col.ColumnName <> "Mode" And col.ColumnName <> "Description" Then
                            r(col.ColumnName) = CDec(IIf(r(col.ColumnName) Is DBNull.Value, "0", r(col.ColumnName))).ToString("#,##" & LabelDecimalDigits)
                        End If

                    Next
                End If
            Next


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

        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub
    Private Sub BindChart()
        ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
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
            Dim Edate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")

            If Me.StartTime.SelectedDate.Value.Month = 1 Or Me.StartTime.SelectedDate.Value.Month = 3 Or Me.StartTime.SelectedDate.Value.Month = 5 Or Me.StartTime.SelectedDate.Value.Month = 7 Or Me.StartTime.SelectedDate.Value.Month = 8 Or Me.StartTime.SelectedDate.Value.Month = 10 Or Me.StartTime.SelectedDate.Value.Month = 12 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-31-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            End If
            If Me.StartTime.SelectedDate.Value.Month = 4 Or Me.StartTime.SelectedDate.Value.Month = 9 Or Me.StartTime.SelectedDate.Value.Month = 6 Or Me.StartTime.SelectedDate.Value.Month = 11 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-30-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            End If
            If Me.StartTime.SelectedDate.Value.Month = 2 And Me.StartTime.SelectedDate.Value.Year Mod 4 = 0 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-29-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

            End If

            If Me.StartTime.SelectedDate.Value.Month = 2 And Me.StartTime.SelectedDate.Value.Year Mod 4 <> 0 Then
                Sdate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-01-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")
                Edate = DateTime.Parse(Me.StartTime.SelectedDate.Value.Month.ToString() + "-28-" + Me.StartTime.SelectedDate.Value.Year.ToString()).ToString("MM-dd-yyyy")

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


            Me.lbl_Country.Text = Me.ddlCountry.SelectedItem.Text

            Args.Visible = True
            rpt.Visible = True

            Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
            rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)


            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetMBRByVan(Err_No, Err_Desc, orgStr, VanStr, AgencyStr, Sdate, Edate)


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


            Me.lblSales.Text = "0"
            Me.lblTarget.Text = "0"
            For Each r As DataRow In dt.Rows



                Me.lblSales.Text = CDec(Me.lblSales.Text) + CDec(r(3).ToString())


                Me.lblTarget.Text = CDec(Me.lblTarget.Text) + CDec(r(2).ToString())




            Next
            Me.lblTarget.Text = CDec(Me.lblTarget.Text).ToString("#,##" & LabelDecimalDigits)
            Me.lblSales.Text = CDec(Me.lblSales.Text).ToString("#,##" & LabelDecimalDigits)



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
            'Dim YTDTarget As New ReportParameter
            'YTDTarget = New ReportParameter("YTDTarget", CStr(Me.lblTarget.Text))
            'Dim YTDSales As New ReportParameter
            'YTDSales = New ReportParameter("YTDSales", CStr(Me.lblSales.Text))

            Dim TeamSize As New ReportParameter
            TeamSize = New ReportParameter("TeamSize", CStr(vancnt))

            Dim TotSales As New ReportParameter
            TotSales = New ReportParameter("TotSales", CStr(Me.lblSales.Text))

            Dim TotTarget As New ReportParameter
            TotTarget = New ReportParameter("TotTarget", CStr(Me.lblTarget.Text))

            rview.ServerReport.SetParameters(New ReportParameter() {org, vans, agencyParam, sdat, edat, SalesorgName, Mode, Country, VanName, AgencyNameParam, TeamSize, TotSales, TotTarget})

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
                Response.AddHeader("Content-disposition", "attachment;filename=MBRByVan.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
                Response.ContentType = "application/pdf"
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=MBRByVan.xls")
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
            If AgencyTab.Tabs(3).Selected = True Then

                BindSummary()
            ElseIf AgencyTab.Tabs(0).Selected = True Then
                ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
            ElseIf AgencyTab.Tabs(1).Selected = True Then
                '  BindAgencySales1()
            ElseIf AgencyTab.Tabs(2).Selected = True Then
                'BindAgencySales1()
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

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        Dim CountryTbl As DataTable = Nothing
        Dim orgTbl As DataTable = Nothing


        HTargetType.Value = CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE

        ObjCommon = New SalesWorx.BO.Common.Common()


        Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)

        CountryTbl = ObjCommon.GetCountry(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)
        ddlCountry.DataSource = CountryTbl
        ddlCountry.DataBind()
        hfUID.Value = CType(Session("User_Access"), UserAccess).UserID

        orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
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

        StartTime.SelectedDate = Now


        Me.lblC.Text = Me.hfCurrency.Value
        BindCombo(OrgStr)
        Args.Visible = False
        rpt.Visible = False

    End Sub
End Class