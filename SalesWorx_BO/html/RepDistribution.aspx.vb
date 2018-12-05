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


Public Class RepDistribution
    Inherits System.Web.UI.Page
    Dim dtCust As New DataTable
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As New SalesWorx.BO.Common.Common
    Dim ObjCollection As Collection
    Private ReportPath As String = "MonthlyDistribution"
    Private Const PageID As String = "P348"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objRep As New Reports
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
            Try
                dtCust.Columns.Add("ID")
                dtCust.Columns.Add("Desc")
                'ViewState("DtItem") = dtItem
                ViewState("dtCust") = dtCust

                ObjCommon = New SalesWorx.BO.Common.Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))


                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                End If
                Dim year As New DateTime(DateTime.Now.Year, 1, 1)
                StartTime.SelectedDate = year
                EndTime.SelectedDate = Now

                Me.ddlGroupBy.SelectedIndex = 0
                BindCombo(IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue))
                'If ValidateInputs() = True Then
                '    BindAgencySales()
                '    BindChart()
                'Else
                '    Me.gvAgency1.DataSource = Nothing
                '    Me.gvAgency1.DataBind()
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
        Else
            LoadProdcut()
        End If


    End Sub

    Private Sub BindCombo(OrgStr)
        ddlAgency.ClearSelection()
        ddlAgency.Items.Clear()

        ddlAgency.DataSource = objRep.GetAllOrgAgency(Err_No, Err_Desc, IIf(OrgStr Is Nothing, "0", OrgStr), CType(Session("User_Access"), UserAccess).UserID)
        ddlAgency.DataTextField = "Description"
        ddlAgency.DataValueField = "Code"
        ddlAgency.DataBind()

        Dim chkcnt As Integer = 0
        For Each item As RadComboBoxItem In ddlAgency.Items
            item.Checked = True
            chkcnt = chkcnt + 1
        Next

        If ddlAgency.Items.Count > 0 And chkcnt > 0 Then
            BindBrand()
        End If


        'ddlBrand.DataSource = objRep.GetAllOrgBrand(Err_No, Err_Desc, IIf(OrgStr Is Nothing, "0", OrgStr), CType(Session("User_Access"), UserAccess).UserID)
        'ddlBrand.DataTextField = "Description"
        'ddlBrand.DataValueField = "Code"
        'ddlBrand.DataBind()


        'For Each item As RadComboBoxItem In ddlBrand.Items
        '    item.Checked = True
        'Next


        'ddlSKU.DataSource = objRep.GetOrgSKU(Err_No, Err_Desc, IIf(OrgStr Is Nothing, "0", OrgStr), "")
        'ddlSKU.DataTextField = "Description"
        'ddlSKU.DataValueField = "ItemCode"
        'ddlSKU.DataBind()


        'For Each item As RadComboBoxItem In ddlSKU.Items
        '    item.Checked = True
        'Next





        ddlVan.DataSource = objRep.GetAllOrgVan(Err_No, Err_Desc, IIf(OrgStr Is Nothing, "0", OrgStr), CType(Session("User_Access"), UserAccess).UserID)
        ddlVan.DataTextField = "SalesRep_Name"
        ddlVan.DataValueField = "SalesRep_ID"
        ddlVan.DataBind()

        For Each item As RadComboBoxItem In ddlVan.Items
            item.Checked = True
        Next
    
    End Sub
    Sub LoadProdcut()
        ObjCommon = New SalesWorx.BO.Common.Common()
        Dim dt As New DataTable

        Dim BrandList As String = Nothing
        For Each item As RadComboBoxItem In ddlBrand.Items
            If item.Checked = True Then
                BrandList = BrandList + item.Value + ","
            End If
        Next
        dt = objRep.GetOrgSKU(Err_No, Err_Desc, Me.ddlOrganization.SelectedValue, "", IIf(BrandList Is Nothing, "-1", BrandList))
        ddlSKU.DataSource = dt
        ddlSKU.DataTextField = "Description"
        ddlSKU.DataValueField = "Inventory_Item_ID"
        ddlSKU.DataBind()
    End Sub
    'Protected Sub ddlSKU_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddlSKU.ItemsRequested
    '    'This calls the controller, which executes the above stored procedure

    '    Dim BrandList As String = "-1"
    '    For Each item As RadComboBoxItem In ddlBrand.Items
    '        If item.Checked = True Then
    '            BrandList = BrandList + item.Value + ","
    '        End If
    '    Next



    '    Dim dt As New DataTable
    '    ddlSKU.ClearSelection()
    '    ddlSKU.Items.Clear()
    '    ddlSKU.Text = ""
    '    dt = objRep.GetOrgSKU(Err_No, Err_Desc, Me.ddlOrganization.SelectedValue, e.Text, BrandList)

    '    Dim ItemsPerRequest As Integer = 700 'Set the number of values to show
    '    Dim itemOffset As Integer = e.NumberOfItems
    '    Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
    '    e.EndOfItems = endOffset = dt.Rows.Count

    '    'Loop through the values to populate the combo box
    '    For i As Integer = itemOffset To endOffset - 1
    '        Dim item As New RadComboBoxItem()
    '        item.Text = dt.Rows(i).Item("Description").ToString
    '        item.Value = dt.Rows(i).Item("ItemCode").ToString

    '        ddlSKU.Items.Add(item)
    '        item.DataBind()
    '    Next

    'End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then

             BindAgencySales()
            Me.RadMultiPage21.SelectedIndex = 0
            Me.AgencyTab.Tabs(0).Selected = True
            ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)


        Else
            Args.Visible = False
            rpt.Visible = False

            Me.gvAgency1.DataSource = Nothing
            Me.gvAgency1.DataBind()

           
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
            Me.RadMultiPage21.SelectedIndex = 0
            Me.AgencyTab.Tabs(0).Selected = True
            ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
        End If
        
    End Sub



    Private Sub gvAgency1_PreRender(sender As Object, e As EventArgs) Handles gvAgency1.PreRender
        For Each column As GridColumn In gvAgency1.MasterTableView.AutoGeneratedColumns

            If column.UniqueName = "Agy" Then
                column.Visible = False
            ElseIf column.UniqueName = "Description" Then
                column.HeaderText = Me.lbl_View.Text

            End If
        Next

        For Each column As GridColumn In gvAgency1.MasterTableView.AutoGeneratedColumns
            If column.UniqueName = "Description" Or column.UniqueName = "Agy" Or column.UniqueName = "Agency" Or column.UniqueName = "Brand" Or column.UniqueName = "Van" Or column.UniqueName = "SKU" Or column.UniqueName = "Location" Then
                column.ItemStyle.HorizontalAlign = HorizontalAlign.Left
                column.HeaderStyle.HorizontalAlign = HorizontalAlign.Left
                column.ItemStyle.Wrap = False
                column.HeaderStyle.Wrap = False
            Else
                column.ItemStyle.HorizontalAlign = HorizontalAlign.Right
                column.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                column.ItemStyle.Wrap = False
                column.HeaderStyle.Wrap = True
            End If
        Next
    End Sub

    Private Sub gvAgency1_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvAgency1.PageIndexChanged
        BindAgencySales()
    End Sub
 
    Private Sub gvAgency1_SortCommand(sender As Object, e As GridSortCommandEventArgs) Handles gvAgency1.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindAgencySales()
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

  
    Private Sub BindAgencySales()
        Try
            Dim SearchQuery As String = ""
           

            rpbFilter.Items(0).Expanded = False
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)



            Dim AgencyStr As String = ""
            Dim AgencyCnt As Integer = 0


            Dim BrandStr As String = ""
            Dim BrandCnt As Integer = 0

            Dim SKUStr As String = "0"
            Dim SKUCnt As Integer = 0
           

            Dim VanStr As String = ""
            Dim vancnt As Integer = 0
            Dim vantxt As String = ""


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


            For Each item As RadComboBoxItem In ddlBrand.Items
                If item.Checked Then
                    BrandCnt = BrandCnt + 1
                    BrandStr = BrandStr & item.Value & ","
                End If
            Next


            If BrandStr = "" Then
                For Each item As RadComboBoxItem In ddlBrand.Items
                    BrandCnt = BrandCnt + 1
                    BrandStr = BrandStr & item.Value & ","
                Next
            End If



            Dim SKU As String = "ALL"
            'For Each item As RadComboBoxItem In ddlSKU.Items
            '    If item.Checked Then
            '        SKUCnt = SKUCnt + 1
            '        SKUStr = SKUStr & item.Value & ","
            '        SKU = SKU & item.Text & ","
            '    End If
            'Next
            dtCust = CType(ViewState("dtCust"), DataTable)

            If dtCust.Rows.Count > 0 Then
                For Each dr In dtCust.Rows
                    SKUCnt = SKUCnt + 1
                    SKUStr = SKUStr & dr("ID").ToString & ","
                    SKU = SKU & dr("Desc").ToString & ","
                Next
                SKUStr = SKUStr.Substring(0, SKUStr.Length - 1)

                SKU = SKU.Substring(0, SKU.Length - 1)
          
            End If


            'If Not String.IsNullOrEmpty(Me.ddlSKU.SelectedValue) Then
            '    SKUCnt = SKUCnt + 1
            '    SKUStr = SKUStr & Me.ddlSKU.SelectedValue & ","
            '    SKU = SKU & Me.ddlSKU.Text & ","
            'Else
            '    SKUCnt = 0
            '    SKUStr = "0"
            '    SKU = "All"
            'End If



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




            If BrandStr <> "" Then
                BrandStr = BrandStr.Substring(0, BrandStr.Length - 1)
            End If
            If BrandStr = "" Then
                BrandStr = "0"
            End If
            If BrandCnt = ddlBrand.Items.Count Then
                lbl_Brand.Text = "All"
            Else
                lbl_Brand.Text = BrandStr
            End If


            'If SKUStr <> "" Then
            '    SKUStr = SKUStr.Substring(0, SKUStr.Length - 1)
            'End If
            'If SKUStr = "" Then
            '    SKUStr = "0"
            'End If
            'If SKUCnt = ddlSKU.Items.Count Then
            '    lbl_SKU.Text = "All"
            'Else
            '    lbl_SKU.Text = SKUCnt & " Products selected"
            'End If

            lbl_SKU.Text = SKU


            lbl_org.Text = Me.ddlOrganization.SelectedItem.Text

            Me.lbl_from.Text = Me.StartTime.SelectedDate.Value.ToString("MMM-yyyy")
            Me.lbl_To.Text = Me.EndTime.SelectedDate.Value.ToString("MMM-yyyy")

            Me.lbl_View.Text = Me.ddlGroupBy.SelectedItem.Text
            If Me.lbl_View.Text = "SKU" Then
                Me.AgencyTab.Tabs(0).Text = IIf(SKUCnt <= 0, "Top 10 SKU Distribution By Month", "SKU Distribution By Month")
            Else
                Me.AgencyTab.Tabs(0).Text = "Distribution By Month / " & Me.lbl_View.Text
            End If


            Me.AgencyTab.Tabs(1).Text = "Invoices & Outlets By " & Me.lbl_View.Text
            Args.Visible = True
            rpt.Visible = True
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetMonthlyDistribution(Err_No, Err_Desc, IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue), VanStr, AgencyStr, Sdate, Edate, Me.lbl_View.Text, BrandStr, SKUStr, Me.ddlChartMode.SelectedValue)

            Dim dv As New DataView(dt)



            Dim LabelDecimalDigits As String = "0.00"


            'For Each r As DataRow In dt.Rows


            '     For Each col As DataColumn In dt.Columns
            '         If col.ColumnName <> "Agy" And col.ColumnName <> "Description" And col.ColumnName <> "Agency" Then
            '             r(col.ColumnName) = CDec(IIf(r(col.ColumnName) Is DBNull.Value, "0", r(col.ColumnName))).ToString("#,##" & LabelDecimalDigits)
            '         End If

            '     Next

            ' Next

            '  For Each r As DataRow In dt.Rows


            For Each col As DataColumn In dt.Columns
                If col.ColumnName = "Description" Then
                    col.ColumnName = Me.lbl_View.Text
                End If

            Next

            For Each col As DataColumn In dt.Columns
                If col.ColumnName <> "Description" And col.ColumnName <> "Agency" And col.ColumnName <> "Agy" And col.ColumnName <> "Brand" And col.ColumnName <> "Van" And col.ColumnName <> "SKU" And col.ColumnName <> "Location" Then
                    col.ColumnName = IIf(DateTime.Parse(col.ColumnName).ToString("dd-MM-yyyy") = "01-01-2500", "Overall", DateTime.Parse(col.ColumnName).ToString("MMM-yyyy"))
                End If

            Next



            gvAgency1.DataSource = dv
            gvAgency1.DataBind()


            hfBrand.Value = BrandStr
            hfSKU.Value = SKUStr

            hfGroupBy.Value = Me.ddlGroupBy.SelectedItem.Text
            Me.hfShownBy.Value = Me.ddlChartMode.SelectedValue

            Me.hfSMonth.Value = Sdate
            Me.hfEMonth.Value = Edate
            Me.hfOrgID.Value = IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue)
            Me.hfVans.Value = VanStr
            Me.hfAgency.Value = AgencyStr

        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub
    Protected Sub gvAgency1_ItemDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles gvAgency1.ItemDataBound
        Dim LabelDecimalDigits As String = "0"
       
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

  
    Private Sub BindChart()
        ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
    End Sub

    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False

       


        If Me.ddlOrganization.SelectedIndex <= 0 Then
            MessageBoxValidation("Please select a organization.", "Validation")
            Return bretval
        End If

        If Me.EndTime.SelectedDate.Value < Me.StartTime.SelectedDate.Value Then
            MessageBoxValidation("Month to should be greater than month from", "Validation")
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
    Private Sub dummyOrgBtn_Click(sender As Object, e As EventArgs) Handles dummyOrgBtn.Click
        Dim objRep = Nothing
        Try
            Dim OrgStr As String = String.Empty

            For Each item As RadComboBoxItem In ddlOrganization.Items
                If item.Checked Then
                    OrgStr = OrgStr & item.Value & ","
                End If
            Next

            BindCombo(OrgStr)
            If ValidateInputs() = True Then
                BindAgencySales()

                If AgencyTab.Tabs(2).Selected = True Then

                    ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart1();", True)
                ElseIf AgencyTab.Tabs(0).Selected = True Then
                    ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
                End If
            Else
                Me.gvAgency1.DataSource = Nothing
                Me.gvAgency1.DataBind()

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

                Me.hfSMonth.Value = Sdate
                Me.hfEMonth.Value = Edate
                Me.hfOrgID.Value = "0"
                Me.hfVans.Value = "0"
                Me.hfAgency.Value = "0"
                If AgencyTab.Tabs(2).Selected = True Then

                    ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart1();", True)
                ElseIf AgencyTab.Tabs(0).Selected = True Then
                    ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
                End If
            End If
        Catch ex As Exception
            Err_No = "7477866"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
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
        Try
            Dim SearchQuery As String = ""
            Dim AgencyStr As String = ""
            Dim AgencyCnt As Integer = 0


            Dim BrandStr As String = ""
            Dim BrandCnt As Integer = 0

            Dim SKUStr As String = "0"
            Dim SKUCnt As Integer = 0


            Dim VanStr As String = ""
            Dim vancnt As Integer = 0
            Dim vantxt As String = ""


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


            For Each item As RadComboBoxItem In ddlBrand.Items
                If item.Checked Then
                    BrandCnt = BrandCnt + 1
                    BrandStr = BrandStr & item.Value & ","
                End If
            Next

            If BrandStr = "" Then
                For Each item As RadComboBoxItem In ddlBrand.Items
                    BrandCnt = BrandCnt + 1
                    BrandStr = BrandStr & item.Value & ","
                Next
            End If


            Dim SKU As String = "All"
            'For Each item As RadComboBoxItem In ddlSKU.Items
            '    If item.Selected Then
            '        SKUCnt = IIf(Me.ddlSKU.SelectedItem.Text <> "", SKUCnt + 1, SKUCnt)
            '        SKUStr = SKUStr & Me.ddlSKU.SelectedItem.Value & ","
            '        SKU = SKU & Me.ddlSKU.SelectedItem.Text & ","
            '    End If
            'Next

            dtCust = CType(ViewState("dtCust"), DataTable)

            If dtCust.Rows.Count > 0 Then
                For Each dr In dtCust.Rows
                    SKUCnt = SKUCnt + 1
                    SKUStr = SKUStr & dr("ID").ToString & ","
                    SKU = SKU & dr("Desc").ToString & ","
                Next
                SKUStr = SKUStr.Substring(0, SKUStr.Length - 1)

                SKU = SKU.Substring(0, SKU.Length - 1)

            End If
            'If Not String.IsNullOrEmpty(Me.ddlSKU.SelectedValue) Then
            '    SKUCnt = SKUCnt + 1
            '    SKUStr = SKUStr & Me.ddlSKU.SelectedValue & ","
            '    SKU = SKU & Me.ddlSKU.Text & ","
            'Else
            '    SKUCnt = 0
            '    SKUStr = "0"
            '    SKU = "All"
            'End If


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




            If BrandStr <> "" Then
                BrandStr = BrandStr.Substring(0, BrandStr.Length - 1)
            End If
            If BrandStr = "" Then
                BrandStr = "0"
            End If
            If BrandCnt = ddlBrand.Items.Count Then
                lbl_Brand.Text = "All"
            Else
                lbl_Brand.Text = BrandStr
            End If


            'If SKUStr <> "" Then
            '    SKUStr = SKUStr.Substring(0, SKUStr.Length - 1)
            'End If
            'If SKUStr = "" Then
            '    SKUStr = "0"
            'End If
            'If SKUCnt = ddlSKU.Items.Count Then
            '    lbl_SKU.Text = "All"
            'Else
            '    lbl_SKU.Text = SKUCnt & " Products selected"
            'End If
            lbl_SKU.Text = SKU

            Me.lbl_View.Text = Me.ddlGroupBy.SelectedItem.Text
            If Me.lbl_View.Text = "SKU" Then
                '  Me.AgencyTab.Tabs(0).Text = IIf(SKUCnt > 10, "Top 10 SKU Distribution By Month", "SKU Distribution By Month")
                Me.AgencyTab.Tabs(0).Text = IIf(SKUCnt <= 0, "Top 10 SKU Distribution By Month", "SKU Distribution By Month")
            Else
                Me.AgencyTab.Tabs(0).Text = "Distribution By Month / " & Me.lbl_View.Text
            End If

            lbl_org.Text = Me.ddlOrganization.SelectedItem.Text

            Me.lbl_from.Text = Me.StartTime.SelectedDate.Value.ToString("MMM-yyyy")
            Me.lbl_To.Text = Me.EndTime.SelectedDate.Value.ToString("MMM-yyyy")





            Args.Visible = True
            rpt.Visible = True

            Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
            rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)



            Dim org As New ReportParameter
            org = New ReportParameter("OID", CStr(IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue)))


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



            Dim VanName As New ReportParameter
            VanName = New ReportParameter("VanName", CStr(Me.lbl_van.Text))


            Dim BrandParam As New ReportParameter
            BrandParam = New ReportParameter("Brand", BrandStr)

            Dim SKUParam As New ReportParameter
            SKUParam = New ReportParameter("SKUList", SKUStr)

            Dim ViewBy As New ReportParameter
            ViewBy = New ReportParameter("ViewBy", Me.lbl_View.Text)


            Dim ChartMode As New ReportParameter
            ChartMode = New ReportParameter("ChartMode", CStr(Me.ddlChartMode.SelectedValue))

            Dim SKUName As New ReportParameter
            SKUName = New ReportParameter("SKUName", Me.lbl_SKU.Text)

            Dim AgencyName As New ReportParameter
            AgencyName = New ReportParameter("AgencyName", Me.lbl_Agency.Text)

            Dim BrandName As New ReportParameter
            BrandName = New ReportParameter("BrandName", Me.lbl_Brand.Text)

            Dim SKUCount As New ReportParameter
            SKUCount = New ReportParameter("SKUCount", SKUCnt.ToString())

            rview.ServerReport.SetParameters(New ReportParameter() {org, vans, agencyParam, sdat, edat, SalesorgName, Mode, VanName, BrandParam, SKUParam, ViewBy, ChartMode, SKUName, AgencyName, BrandName, SKUCount})

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
                Response.AddHeader("Content-disposition", "attachment;filename=MonthlyDistribution.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
             
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=MonthlyDistribution.xls")
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





  
    Protected Sub ddlOrganization_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlOrganization.SelectedIndexChanged
        ddlBrand.ClearSelection()
        ddlBrand.Items.Clear()


        ddlSKU.Entries.Clear()
        If dtCust.Rows.Count > 0 Then
            dtCust.Rows.Clear()
        End If
        dtCust.Columns.Add("ID")
        dtCust.Columns.Add("Desc")
        ViewState("dtCust") = dtCust

        BindCombo(IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue))




        Args.Visible = False
        rpt.Visible = False
    End Sub

    Protected Sub ddlChartMode_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlChartMode.SelectedIndexChanged
        If ValidateInputs() = True And Args.Visible = True Then
            Me.hfShownBy.Value = Me.ddlChartMode.SelectedItem.Text
            BindChart()
        End If
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        ddlVan.ClearCheckedItems()
        ddlVan.Items.Clear()
        ddlBrand.ClearCheckedItems()
        ddlBrand.Items.Clear()
        ddlAgency.ClearCheckedItems()
        ddlAgency.Items.Clear()

        ddlSKU.Entries.Clear()
        If dtCust.Rows.Count > 0 Then
            dtCust.Rows.Clear()
        End If
        dtCust.Columns.Add("ID")
        dtCust.Columns.Add("Desc")
        ViewState("dtCust") = dtCust
        ddlGroupBy.ClearSelection()
        BindCombo(IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue))
        Dim year As New DateTime(DateTime.Now.Year, 1, 1)
        StartTime.SelectedDate = year
        EndTime.SelectedDate = Now
        Args.Visible = False
        rpt.Visible = False
    End Sub

    Protected Sub ddlAgency_CheckAllCheck(sender As Object, e As RadComboBoxCheckAllCheckEventArgs) Handles ddlAgency.CheckAllCheck
        ddlSKU.Entries.Clear()
        If dtCust.Rows.Count > 0 Then
            dtCust.Rows.Clear()
        End If
        dtCust.Columns.Add("ID")
        dtCust.Columns.Add("Desc")
        ViewState("dtCust") = dtCust
        BindBrand()
    End Sub

    Protected Sub ddlAgency_ItemChecked(sender As Object, e As RadComboBoxItemEventArgs) Handles ddlAgency.ItemChecked
        ddlSKU.Entries.Clear()
        If dtCust.Rows.Count > 0 Then
            dtCust.Rows.Clear()
        End If
        dtCust.Columns.Add("ID")
        dtCust.Columns.Add("Desc")
        ViewState("dtCust") = dtCust
        BindBrand()
    End Sub
    Private Sub BindBrand()

        Dim AgencyList As String = Nothing
        For Each item As RadComboBoxItem In ddlAgency.Items
            If item.Checked = True Then
                AgencyList = AgencyList + item.Value + ","
            End If
        Next

        ddlBrand.DataSource = objRep.GetAllOrgBrand(Err_No, Err_Desc, IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue), CType(Session("User_Access"), UserAccess).UserID, IIf(AgencyList Is Nothing, "-1", AgencyList))
        ddlBrand.DataTextField = "Description"
        ddlBrand.DataValueField = "Code"
        ddlBrand.DataBind()

        Dim chkcnt As Integer = 0
        For Each item As RadComboBoxItem In ddlBrand.Items
            item.Checked = True
            chkcnt = chkcnt + 1
        Next

        If ddlBrand.Items.Count > 0 And chkcnt > 0 Then
            LoadProdcut()
        End If
    End Sub
  
    Protected Sub ddlBrand_CheckAllCheck(sender As Object, e As RadComboBoxCheckAllCheckEventArgs) Handles ddlBrand.CheckAllCheck
        ddlSKU.Entries.Clear()
        If dtCust.Rows.Count > 0 Then
            dtCust.Rows.Clear()
        End If
        dtCust.Columns.Add("ID")
        dtCust.Columns.Add("Desc")
        ViewState("dtCust") = dtCust
        LoadProdcut()
    End Sub

    Protected Sub ddlBrand_ItemChecked(sender As Object, e As RadComboBoxItemEventArgs) Handles ddlBrand.ItemChecked
        ddlSKU.Entries.Clear()
        If dtCust.Rows.Count > 0 Then
            dtCust.Rows.Clear()
        End If
        dtCust.Columns.Add("ID")
        dtCust.Columns.Add("Desc")
        ViewState("dtCust") = dtCust
        LoadProdcut()
    End Sub

    Protected Sub ddlCust_EntryAdded(sender As Object, e As AutoCompleteEntryEventArgs) Handles ddlSKU.EntryAdded
        dtCust = CType(ViewState("dtCust"), DataTable)
        Dim seldr() As DataRow
        seldr = dtCust.Select("ID='" & e.Entry.Value & "'")
        If seldr.Length <= 0 Then
            Dim dr As DataRow
            dr = dtCust.NewRow()
            dr(0) = e.Entry.Value
            dr(1) = e.Entry.Text
            dtCust.Rows.Add(dr)
        Else
            ddlSKU.Entries.Remove(e.Entry)
        End If
        ViewState("dtCust") = dtCust

        Args.Visible = False
    End Sub
    Protected Sub ddlCust_EntryRemoved(sender As Object, e As AutoCompleteEntryEventArgs) Handles ddlSKU.EntryRemoved
        dtCust = CType(ViewState("dtCust"), DataTable)
        Dim dr() As DataRow
        dr = dtCust.Select("ID='" & e.Entry.Value & "'")
        If dr.Length > 0 Then
            dtCust.Rows.Remove(dr(0))
        End If
        ViewState("dtCust") = dtCust

        Args.Visible = False
    End Sub

End Class