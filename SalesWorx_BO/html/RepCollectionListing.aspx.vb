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
Imports OfficeOpenXml
Partial Public Class RepCollectionListing
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As New SalesWorx.BO.Common.Common
    Dim ObjCollection As Collection
    Private ReportPath As String = "CollectionList"
    Private Const PageID As String = "P203"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'If Not IsNothing(Me.Master) Then

        '    Dim masterScriptManager As ScriptManager
        '    masterScriptManager = CType(Master.FindControl("ScriptManager1"), ScriptManager)

        '    ' Make sure our master page has the script manager we're looking for
        '    If Not IsNothing(masterScriptManager) Then

        '        ' Turn off partial page postbacks for this page
        '        masterScriptManager.EnablePartialRendering = False
        '    End If

        'End If

    End Sub
   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
            Try
                ObjCommon = New SalesWorx.BO.Common.Common()
                Dim HasPermission As Boolean = False
                ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
                If Not HasPermission Then
                    Err_No = 500
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
                End If

                Dim CountryTbl As DataTable = Nothing
                Dim orgTbl As DataTable = Nothing

                CountryTbl = ObjCommon.GetCountry(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)
                ddlCountry.DataSource = CountryTbl
                ddlCountry.DataBind()


                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
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

                    ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
                    ddlOrganization.DataBind()

                End If

                Dim OrgStr As String = Nothing
                For Each item As RadComboBoxItem In ddlOrganization.Items
                    item.Checked = True
                    If item.Checked Then

                        OrgStr = OrgStr & "," & item.Value

                    End If
                Next


                
                
                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                txtToDate.SelectedDate = Now()

               
                LoadorgDetails()

                'BindData()
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
    Function BuildQuery() As String
        Dim SearchQuery As String = ""
        If (txtCollectionRefNo.Text = "" And txtFromDate.DateInput.Text = "" And txtToDate.DateInput.Text = "") Then
            SearchQuery = ""
        Else
            If txtCollectionRefNo.Text <> "" Then
                SearchQuery = SearchQuery & " And A.Collection_Ref_No like '" & Utility.ProcessSqlParamString(txtCollectionRefNo.Text) & "%'"
            End If
            If txtFromDate.DateInput.Text <> "" Then
                Dim TemFromDateStr As String = CDate(txtFromDate.SelectedDate).ToString("dd/MM/yyyy")
                Dim DateArr As Array = TemFromDateStr.Split("/")
                TemFromDateStr = DateArr(1) & "/" & DateArr(0) & "/" & DateArr(2)
                SearchQuery = SearchQuery & " And A.Collected_On >= '" & TemFromDateStr & "'"
                If txtToDate.DateInput.Text = "" Then
                    SearchQuery = SearchQuery & " And A.Collected_On <= '" & TemFromDateStr & " 23:59:59'"
                End If
            End If
            If txtToDate.DateInput.Text <> "" Then
                Dim TemToDateStr As String = CDate(txtToDate.SelectedDate).ToString("dd/MM/yyyy")
                Dim DateArr As Array = TemToDateStr.Split("/")
                TemToDateStr = DateArr(1) & "/" & DateArr(0) & "/" & DateArr(2)
                SearchQuery = SearchQuery & " And A.Collected_On <= '" & TemToDateStr & " 23:59:59'"
            End If
        End If
        If ddl_Discount.SelectedItem.Value = "1" Then
            SearchQuery = SearchQuery & " And isnull(A.Discount,0)<>0 "
        End If

        Dim collection As IList(Of RadComboBoxItem) = ddVan.CheckedItems
        Dim van As String = ""
        For Each li As RadComboBoxItem In Collection
            van = van & li.Value & ","

        Next
         
        If van = "" Then
            van = "0"
        End If

        If van = "0" Then
            SearchQuery = SearchQuery & " And A.Collected_By in (" & ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID) & ")"
        Else
            SearchQuery = SearchQuery & " And A.Collected_By in(select item from dbo.SplitQuotedString('" & van & "'))"
        End If

        If ddl_heldpdc.SelectedItem.Value = "1" Then
            SearchQuery = SearchQuery & " And A.Collection_Type='PDC' and A.Status='W'"
        End If
        Return SearchQuery
    End Function
    Private Sub BindReport()
        Try
            Dim SearchQuery As String = ""
            If ddlOrganization.CheckedItems.Count > 0 Then
                SearchQuery = BuildQuery()
            Else
                MessageBoxValidation("Select an organization.", "Validation")
                Exit Sub
            End If

            rpbFilter.Items(0).Expanded = False
            Args.Visible = False

            Dim collection As IList(Of RadComboBoxItem) = ddVan.CheckedItems

            Dim vantxt As String = ""
            Dim van As String = ""
            For Each li As RadComboBoxItem In Collection
                van = van & li.Value & ","
                vantxt = vantxt & li.Text & ","
            Next
            If vantxt <> "" Then
                vantxt = vantxt.Substring(0, vantxt.Length - 1)
            End If
            If van = "" Then
                van = "0"
            End If


            Dim Orgtxt As String = ""
            Dim Org As String = ""
            Dim Orgcollection As IList(Of RadComboBoxItem) = ddlOrganization.CheckedItems
            For Each li As RadComboBoxItem In Orgcollection
                Org = Org & li.Value & ","
                Orgtxt = Orgtxt & li.Text & ","
            Next
            If Orgtxt <> "" Then
                Orgtxt = Orgtxt.Substring(0, Orgtxt.Length - 1)
            End If
            If Org = "" Then
                Org = "0"
            End If

            lbl_org.Text = Orgtxt
            If van = "0" Then
                lbl_van.Text = "All"
            Else
                lbl_van.Text = vantxt
            End If

            lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_RefNo.Text = IIf(String.IsNullOrEmpty(txtCollectionRefNo.Text), "All", txtCollectionRefNo.Text)
            If ddl_Discount.SelectedItem.Value = "1" Then
                lbl_Discount.Text = "With Discount only"
            Else
                lbl_Discount.Text = "With and Without Discount"
            End If

            If ddl_heldpdc.SelectedItem.Value = "1" Then
                lbl_Held.Text = "Only Held PDCs"
            Else
                lbl_Held.Text = ""
            End If
            Args.Visible = True

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetCollectionListing(Err_No, Err_Desc, SearchQuery, Org)
            gvRep.DataSource = dt
            gvRep.DataBind()


            Dim dtcurrency As DataTable
            dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.CheckedItems(0).Value)

            Dim Currency As String = ""
            If dtcurrency.Rows.Count > 0 Then
                Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
                lblDecimal.Text = "N" & dtcurrency.Rows(0)("Decimal_Digits")
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

                If x.PayMode.ToString() = "CASH" Then

                    StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>" & "Total " & x.PayMode.ToString & Currency & "<div class='text-primary'>" & Format(x.Total, lblDecimal.Text) & "</div></div></div>"
                End If

                i = i + 1
            Next

            For Each x In query
                'Dim dr As DataRow
                'dr = DtSummary.NewRow
                'dr(0) = "Total " & x.PayMode & Currency
                'dr(1) = Format(x.Total, "#,##0.00")
                'DtSummary.Rows.Add(dr)

                If x.PayMode.ToString() = "CURR-CHQ" Then

                    StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>" & "Total " & x.PayMode.ToString & Currency & "<div class='text-primary'>" & Format(x.Total, lblDecimal.Text) & "</div></div></div>"
                End If

                i = i + 1
            Next


            For Each x In query
                'Dim dr As DataRow
                'dr = DtSummary.NewRow
                'dr(0) = "Total " & x.PayMode & Currency
                'dr(1) = Format(x.Total, "#,##0.00")
                'DtSummary.Rows.Add(dr)

                If x.PayMode.ToString() = "PDC" Then

                    StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>" & "Total " & x.PayMode.ToString & Currency & "<div class='text-primary'>" & Format(x.Total, lblDecimal.Text) & "</div></div></div>"
                End If

                i = i + 1
            Next


            For Each x In query
                'Dim dr As DataRow
                'dr = DtSummary.NewRow
                'dr(0) = "Total " & x.PayMode & Currency
                'dr(1) = Format(x.Total, "#,##0.00")
                'DtSummary.Rows.Add(dr)

                If CType(Session.Item("CONTROL_PARAMS"), ControlParams).CLIENT_CODE = "GYMA" And x.PayMode.ToString() = "CC" Then
                    StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>" & "Total Credit Card " & Currency & "<div class='text-primary'>" & Format(x.Total, lblDecimal.Text) & "</div></div></div>"
                End If

                i = i + 1
            Next

            Dim NoofCustWithDiscount = (From Coll In dt _
                          Where Coll.Field(Of Decimal)("Discount") > 0 _
                          Select CustCount = Coll.Field(Of String)("Customer") Distinct).ToList()

            If Not NoofCustWithDiscount Is Nothing Then
                StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>No. of customers having discount <div class='text-primary'>" & NoofCustWithDiscount.Count & "</div></div></div>"
            End If


            Dim NoofTransWithDiscount = From Coll In dt _
                          Where Coll.Field(Of Decimal)("Discount") > 0 _
                          Select Coll.Field(Of String)("Collection_Ref_No") Distinct

            If Not NoofTransWithDiscount Is Nothing Then
                StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>No. of transactions having discount <div class='text-primary'>" & NoofTransWithDiscount.Count & "</div></div></div>"
            End If

            Dim sum = dt.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("Discount")))

            StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>Total discount " & Currency & "<div class='text-primary'>" & Format(sum, lblDecimal.Text) & "</div></div></div>"

            summary.InnerHtml = StrSummary


        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub
    Private Sub BindPDCReveivables()
        Try
            Dim SearchQuery As String = ""
            If ddlOrganization.CheckedItems.Count > 0 Then
                SearchQuery = BuildQuery()
            Else
                MessageBoxValidation("Select an organization.", "Validation")
                Exit Sub
            End If


            Dim Orgtxt As String = ""
            Dim Org As String = ""
            Dim Orgcollection As IList(Of RadComboBoxItem) = ddlOrganization.CheckedItems
            For Each li As RadComboBoxItem In Orgcollection
                Org = Org & li.Value & ","
                Orgtxt = Orgtxt & li.Text & ","
            Next
            If Orgtxt <> "" Then
                Orgtxt = Orgtxt.Substring(0, Orgtxt.Length - 1)
            End If
            If Org = "" Then
                Org = "0"
            End If

            Dim collection As IList(Of RadComboBoxItem) = ddVan.CheckedItems


            Dim van As String = ""
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","

            Next
            If van = "" Then
                van = "0"
            End If

            Dim original As DateTime = DateTime.Now ' The date you want to get the last day of the month for
            If collection.Count > 10 Then
                ddl_Type.Visible = True
            Else
                ddl_Type.Visible = False
            End If
            Dim fromdate As Date
            Dim todate As Date
            todate = original.Date.AddDays(-(original.Day - 1)).AddMonths(1).AddDays(-1)
            fromdate = DateAdd(DateInterval.Month, -11, DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now))

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetPDCRecievablesbyMonth(Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, Org, van, fromdate.ToString("dd-MMM-yyyy"), todate.ToString("dd-MMM-yyyy"))

            Dim FinalDt As New DataTable
            FinalDt.Columns.Add("Salesrep Name")
            Dim tfromdate As DateTime
            tfromdate = fromdate

            While tfromdate <= todate
                FinalDt.Columns.Add(tfromdate.ToString("MMM-yyyy"))
                tfromdate = DateAdd(DateInterval.Month, 1, tfromdate)
            End While

            Dim query

            If ddl_Type.SelectedItem.Value = "10" Then
                query = (From UserEntry In dt _
                     Group UserEntry By key = UserEntry.Field(Of String)("Salesrep_name") Into Group _
                     Select Salesrep_name = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("Amount")) Order By Total Descending.Take(10)).ToList

            Else
                query = (From UserEntry In dt _
                     Group UserEntry By key = UserEntry.Field(Of String)("Salesrep_name") Into Group _
                     Select Salesrep_name = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("Amount")) Order By Total Descending).ToList

            End If


            For Each x In query
                tfromdate = fromdate
                Dim dr As DataRow
                dr = FinalDt.NewRow
                dr("Salesrep Name") = x.Salesrep_name
                While tfromdate <= todate
                    Dim seldr() As DataRow
                    seldr = dt.Select("Salesrep_name='" & x.Salesrep_name & "' and m=" & tfromdate.Month & " and yr=" & tfromdate.Year)
                    If seldr.Length > 0 Then
                        dr(tfromdate.ToString("MMM-yyyy")) = Format(seldr(0)("Amount"), lblDecimal.Text)
                    Else

                        dr(tfromdate.ToString("MMM-yyyy")) = Format(0, lblDecimal.Text)
                    End If
                    tfromdate = DateAdd(DateInterval.Month, 1, tfromdate)
                End While
                FinalDt.Rows.Add(dr)
            Next


            Dim dtcurrency As DataTable
            dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.CheckedItems(0).Value)
            Dim Currency As String = ""
            If dtcurrency.Rows.Count > 0 Then
                Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
            End If

            lbl_Currency.Text = Currency
            divCurrency.Visible = True

            gvPDC.DataSource = FinalDt
            gvPDC.DataBind()

            hfOrg.Value = Org
            hfVan.Value = van
            HFrom.Value = fromdate
            Hto.Value = todate
            UId.Value = CType(Session.Item("USER_ACCESS"), UserAccess).UserID
            Hcount.Value = ddl_Type.SelectedItem.Value
            BindChart()

        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub
    Protected Sub Collection_TabClick(sender As Object, e As RadTabStripEventArgs) Handles Collectiontab.TabClick
        If Args.Visible = True Then
            If Collectiontab.Tabs(1).Selected = True Then
                ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
            End If
        End If
    End Sub
    Private Sub ddl_Type_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddl_Type.SelectedIndexChanged
        If ValidateInputs() Then
            BindPDCReveivables()

        Else
            gvPDC.Visible = False
            divCurrency.Visible = False
        End If
    End Sub
    Private Sub BindChart()

        ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)

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
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            gvRep.Visible = True
            Collectiontab.Visible = True
            BindReport()
            BindPDCReveivables()

        Else

            Args.Visible = False
            summary.InnerHtml = ""
            gvRep.Visible = False
            Collectiontab.Visible = False
        End If
    End Sub
    Sub LoadorgDetails()
        'If Not (ddlOrganization.SelectedItem.Value = "0") Then
        '    Dim objUserAccess As UserAccess
        '    objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        '    ObjCommon = New SalesWorx.BO.Common.Common()
        '    ddVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())
        '    ddVan.DataBind()

        '    For Each itm As RadComboBoxItem In ddVan.Items
        '        itm.Checked = True
        '    Next

        '    Dim dtcurrency As DataTable
        '    Dim ObjReport As New SalesWorx.BO.Common.Reports
        '    dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

        '    Dim Currency As String = ""
        '    If dtcurrency.Rows.Count > 0 Then
        '        Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
        '        lblDecimal.Text = "N" & dtcurrency.Rows(0)("Decimal_Digits")
        '    End If
        'End If
        If Not ddlOrganization.CheckedItems Is Nothing Then
            If ddlOrganization.CheckedItems.Count > 0 Then
                Dim objUserAccess As UserAccess
                objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
                Dim ObjRep As SalesWorx.BO.Common.Reports = New SalesWorx.BO.Common.Reports()

                Dim OrgStr As String = ""
                For Each li As RadComboBoxItem In ddlOrganization.CheckedItems
                    OrgStr = OrgStr & li.Value & ","
                Next
                ddVan.DataSource = ObjRep.GetAllOrgVan(Err_No, Err_Desc, OrgStr, objUserAccess.UserID.ToString())
                ddVan.DataBind()

                For Each itm As RadComboBoxItem In ddVan.Items
                    itm.Checked = True
                Next

                Dim dtcurrency As DataTable
                Dim ObjReport As New SalesWorx.BO.Common.Reports
                dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.CheckedItems(0).Value)

                Dim Currency As String = ""
                If dtcurrency.Rows.Count > 0 Then
                    Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
                    lblDecimal.Text = "N" & dtcurrency.Rows(0)("Decimal_Digits")
                End If
            Else
                ddVan.Items.Clear()
            End If
        Else
            ddVan.Items.Clear()
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
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If Not ddlOrganization.CheckedItems Is Nothing Then
            If ddlOrganization.CheckedItems.Count > 0 Then
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
        Else
            MessageBoxValidation("Please select the Organisation", "Validation")
            Return bretval
        End If
    End Function
    
    Sub Export(format As String)



        Dim SearchParams As String = ""
        SearchParams = BuildQuery()
        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        Dim Searchvalue As New ReportParameter


        Dim Orgtxt As String = ""
        Dim Org As String = ""
        Dim Orgcollection As IList(Of RadComboBoxItem) = ddlOrganization.CheckedItems
        For Each li As RadComboBoxItem In Orgcollection
            Org = Org & li.Value & ","
            Orgtxt = Orgtxt & li.Text & ","
        Next
        If Orgtxt <> "" Then
            Orgtxt = Orgtxt.Substring(0, Orgtxt.Length - 1)
        End If
        If Org = "" Then
            Org = "0"
        End If

        Searchvalue = New ReportParameter("SearchParams", SearchParams)
        Dim OrgId As New ReportParameter
        OrgId = New ReportParameter("OrgId", Org)

        Dim collection As IList(Of RadComboBoxItem) = ddVan.CheckedItems
        Dim van As String = ""
        For Each li As RadComboBoxItem In collection
            van = van & li.Value & ","

        Next

        If van = "" Then
            van = "0"
        End If

        Dim SID As New ReportParameter
        SID = New ReportParameter("SID", van)

        Dim FromDate As New ReportParameter
        FromDate = New ReportParameter("FromDate", CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"))

        Dim RefNo As New ReportParameter
        RefNo = New ReportParameter("RefNo", txtCollectionRefNo.Text)
        Dim ToDate As New ReportParameter
        ToDate = New ReportParameter("ToDate", CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"))

        Dim WithDiscount As New ReportParameter
        If ddl_Discount.SelectedItem.Value = "1" Then
            WithDiscount = New ReportParameter("WithDiscount", "1")
        Else
            WithDiscount = New ReportParameter("WithDiscount", "0")
        End If

        Dim original As DateTime = DateTime.Now

        Dim PDCfromdate As Date
        Dim PDCtodate As Date
        PDCtodate = original.Date.AddDays(-(original.Day - 1)).AddMonths(1).AddDays(-1)
        PDCfromdate = DateAdd(DateInterval.Month, -11, DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now))

        Dim PDCFrom As New ReportParameter
        PDCFrom = New ReportParameter("PDCFrom", PDCfromdate.ToString("dd-MMM-yyyy"))

        Dim PDCTo As New ReportParameter
        PDCTo = New ReportParameter("PDCTo", PDCtodate.ToString("dd-MMM-yyyy"))

        Dim UID As New ReportParameter
        UID = New ReportParameter("UID", objUserAccess.UserID)

        Dim Type As New ReportParameter
        Type = New ReportParameter("Type", "G")

        rview.ServerReport.SetParameters(New ReportParameter() {Searchvalue, OrgId, SID, FromDate, ToDate, WithDiscount, RefNo, PDCFrom, PDCTo, UID, Type})

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
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=Collections.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()

    End Sub

    Private Sub gvPDC_PreRender(sender As Object, e As EventArgs) Handles gvPDC.PreRender
        For Each column As GridColumn In gvRep.MasterTableView.AutoGeneratedColumns
            If column.UniqueName.ToLower = "salesrep name" Then
                column.ItemStyle.HorizontalAlign = HorizontalAlign.Left
                column.HeaderStyle.HorizontalAlign = HorizontalAlign.Left
            End If
        Next

    End Sub

    Private Sub dgvPDC_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvPDC.SortCommand
        ViewState("SortPDCField") = e.SortExpression
        SortDirectionPDC = "flip"
        BindPDCReveivables()
    End Sub
    Private Sub dgvPDC_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvPDC.PageIndexChanged

        BindPDCReveivables()
    End Sub
    Private Property SortDirectionPDC() As String
        Get
            If ViewState("SortPDCField") Is Nothing Then
                ViewState("SortPDCField") = "ASC"
            End If
            Return ViewState("SortPDCField").ToString()
        End Get
        Set(ByVal value As String)
            Dim s As String = SortDirection

            If value = "flip" Then
                s = If(s = "ASC", "DESC", "ASC")
            Else
                s = value
            End If

            ViewState("SortPDCField") = s
        End Set
    End Property

    

    Private Sub ddlOrganization_CheckAllCheck(sender As Object, e As RadComboBoxCheckAllCheckEventArgs) Handles ddlOrganization.CheckAllCheck
        LoadorgDetails()
        SearchBtn.Enabled = True
    End Sub
    
    Private Sub ddlOrganization_ItemChecked(sender As Object, e As RadComboBoxItemEventArgs) Handles ddlOrganization.ItemChecked
        LoadorgDetails()
        SearchBtn.Enabled = True
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        Dim CountryTbl As DataTable = Nothing
        Dim orgTbl As DataTable = Nothing

        CountryTbl = ObjCommon.GetCountry(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)
        ddlCountry.DataSource = CountryTbl
        ddlCountry.DataBind()


        Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
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

            ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
            ddlOrganization.DataBind()

        End If

        Dim OrgStr As String = Nothing
        For Each item As RadComboBoxItem In ddlOrganization.Items
            item.Checked = True
            If item.Checked Then

                OrgStr = OrgStr & "," & item.Value

            End If
        Next




        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Now()


        LoadorgDetails()
        txtCollectionRefNo.Text = ""
        ddl_Discount.ClearSelection()
        ddl_Type.ClearSelection()

        Args.Visible = False
        summary.InnerHtml = ""
        gvRep.Visible = False
        Collectiontab.Visible = False

    End Sub

    Private Sub ddlCountry_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlCountry.SelectedIndexChanged
        LoadOrgs()
        LoadorgDetails()
    End Sub
    Sub LoadOrgs()
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


        ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
        ddlOrganization.DataBind()

       

        Dim OrgStr As String = Nothing
        For Each item As RadComboBoxItem In ddlOrganization.Items
            item.Checked = True
            If item.Checked Then

                OrgStr = OrgStr & "," & item.Value

            End If
        Next

    End Sub
    Private Sub BtnExportBiffExcel_Click(sender As Object, e As EventArgs) Handles BtnExportBiffExcel.Click
        Dim SearchQuery As String
        SearchQuery = BuildQuery()
        Dim tblData As New DataTable
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        Dim ObjReport As New SalesWorx.BO.Common.Reports

        Dim Org As String = ""
        Dim Orgcollection As IList(Of RadComboBoxItem) = ddlOrganization.CheckedItems
        For Each li As RadComboBoxItem In Orgcollection
            Org = Org & li.Value & ","

        Next
         
        If Org = "" Then
            Org = "0"
        End If


        Dim dt As New DataTable
        dt = ObjReport.GetCollectionListing(Err_No, Err_Desc, SearchQuery, Org)
        tblData = dt.DefaultView.ToTable(False, "Collection_Ref_No", "Customer", "Collected_On", "Collected_By", "Amount", "Collection_Type", "Discount", "Discount_Reason", "Cheque_No", "Cheque_Date", "Bank_Name", "Bank_Branch", "Status")

        If tblData.Rows.Count > 0 Then
            
            Using package As New ExcelPackage()
                ' add a new worksheet to the empty workbook
                Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
                Worksheet.Cells("A1").LoadFromDataTable(tblData, True)
                Worksheet.Column(3).Style.Numberformat.Format = "dd-MMM-yyyy"
                Worksheet.Cells.AutoFitColumns()

                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename= CollectionList.xlsx")

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
