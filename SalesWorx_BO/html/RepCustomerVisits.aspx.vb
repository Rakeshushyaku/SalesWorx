Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Partial Public Class RepCustomerVisits
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "CustomerVisits"

    Private Const PageID As String = "P206"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single

    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    'Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
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

               

                If Not Request.QueryString("ID") Is Nothing Then
                    Dim s() As String = Request.QueryString("ID").Split("$")
                    Dim SID As String = s(0).ToString()
                    Dim MonthYear As DateTime = DateTime.Parse(s(1).ToString())
                    txtFromDate.SelectedDate = MonthYear
                    Dim endOfMonth As DateTime = New DateTime(MonthYear.Year, MonthYear.Month, DateTime.DaysInMonth(MonthYear.Year, MonthYear.Month))
                    txtToDate.SelectedDate = endOfMonth

                    Dim dt As New DataTable
                    dt = (New SalesWorx.BO.Common.Common).GetSalesOrgbyFsr(Err_No, Err_Desc, SID)
                    If dt.Rows.Count > 0 Then
                        If Not ddlOrganization.FindItemByValue(dt.Rows(0)("MAS_Org_ID").ToString) Is Nothing Then
                            ddlOrganization.ClearSelection()
                            ddlOrganization.Items.FindItemByValue(dt.Rows(0)("MAS_Org_ID").ToString).Selected = True

                            Dim objUserAccess As UserAccess
                            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

                            LoadVan()

                            ''Filling Currency and decimal
                            Dim dtCur As New DataTable
                            dtCur = ObjCommon.GetCurrencyByOrg(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
                            If dtCur.Rows.Count > 0 Then
                                hfCurrency.Value = dtCur.Rows(0)(0).ToString()
                                hfDecimal.Value = dtCur.Rows(0)(1).ToString()
                            End If


                            ' ''ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
                            ' ''ddlVan.DataBind()
                            ' ''ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --"))

                            If Not ddlVan.FindItemByValue(SID) Is Nothing Then
                                ddlVan.ClearSelection()
                                For Each itm As RadComboBoxItem In ddlVan.Items
                                    itm.Checked = False
                                Next
                                ddlVan.FindItemByValue(SID).Checked = True
                                ddlVan.FindItemByValue(SID).Selected = True
                            End If

                            ''ddlCustomer.DataSource = ObjCommon.GetCustomerByCriteria(Err_No, Err_Desc, ddlOrganization.SelectedValue)
                            ''ddlCustomer.DataBind()
                            ''ddlCustomer.Items.Insert(0, New RadComboBoxItem("-- Select a value --"))

                            LoadSummary()
                            BindData()

                            gvRep.Visible = True
                            Args.Visible = True

                        End If
                    End If
                Else
                    txtFromDate.SelectedDate = FirstDayOfMonth(Now().Date)
                    txtToDate.SelectedDate = Now().Date

                    If ddlOrganization.Items.Count = 2 Then
                        ddlOrganization.SelectedIndex = 1

                        LoadVan()

                        ''Filling Currency and decimal
                        Dim dtCur As New DataTable
                        dtCur = ObjCommon.GetCurrencyByOrg(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
                        If dtCur.Rows.Count > 0 Then
                            hfCurrency.Value = dtCur.Rows(0)(0).ToString()
                            hfDecimal.Value = dtCur.Rows(0)(1).ToString()
                        End If

                    End If


                    ' '' if back from details page 
                    If Not String.IsNullOrEmpty(Request.QueryString("b")) Then

                        If Session("CurOrg") IsNot Nothing Then
                            '' Setting Org

                            If Not ddlOrganization.FindItemByValue(Session("CurOrg")) Is Nothing Then
                                ddlOrganization.ClearSelection()
                                ddlOrganization.FindItemByValue(Session("CurOrg")).Selected = True
                            End If


                            '' Setting Van
                            If Not ddlVan.FindItemByValue(Session("CurVan")) Is Nothing Then
                                ddlVan.ClearSelection()
                                ddlVan.FindItemByValue(Session("CurVan")).Selected = True
                            End If

                            '' Setting Customer type
                            If Not ddl_CustType.FindItemByValue(Session("CurCusType")) Is Nothing Then
                                ddl_CustType.ClearSelection()
                                ddl_CustType.FindItemByValue(Session("CurCusType")).Selected = True
                            End If

                            If Session("CurCus") <> "" Then
                                ''  ddlCustomer.Text = Session("CurCus")

                                '' Loading Customer dropDown 
                                Dim ID As String() = Session("CurCus").Split("-")
                                If ID.Length > 1 Then
                                    Dim dt = ObjCommon.GetCustomerByCriteriaandText(Err_No, Err_Desc, ddlOrganization.SelectedValue, "", ID(1))

                                    If dt.Rows.Count > 0 Then
                                        For i As Integer = 0 To dt.Rows.Count - 1
                                            Dim item As New RadComboBoxItem()
                                            item.Text = dt.Rows(i).Item("Customer").ToString
                                            item.Value = dt.Rows(i).Item("CustomerID").ToString
                                            item.Selected = True

                                            ddlCustomer.Items.Add(item)
                                            item.DataBind()
                                        Next

                                        If Not ddlCustomer.FindItemByText(Session("CurCus")) Is Nothing Then
                                            ddlCustomer.ClearSelection()
                                            ddlCustomer.FindItemByText(Session("CurCus")).Selected = True
                                        End If

                                    End If
                                End If
                            End If

                            txtFromDate.SelectedDate = Session("CurFDat")
                            txtToDate.SelectedDate = Session("CurTDat")

                            gvRep.CurrentPageIndex = Session("CurPIndex")


                            LoadSummary()
                            BindData()

                            Session("CurOrg") = Nothing
                            Session("CurVan") = Nothing
                            Session("CurCusType") = Nothing
                            Session("CurCus") = Nothing
                            Session("CurFDat") = Nothing
                            Session("CurEDat") = Nothing

                            gvRep.Visible = True
                            Args.Visible = True
                        End If
                    End If

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
        Else
            Viewimage_Window.VisibleOnPageLoad = False
        End If
    End Sub
    Public Function FirstDayOfMonth(ByVal sourceDate As DateTime) As DateTime
        Return New DateTime(sourceDate.Year, sourceDate.Month, 1)
    End Function
    Sub LoadVan()
        Try
            If Not (ddlOrganization.SelectedItem.Value = "0") Then
                Dim objUserAccess As UserAccess
                objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
                ObjCommon = New SalesWorx.BO.Common.Common()
                ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
                ddlVan.DataValueField = "SalesRep_ID"
                ddlVan.DataTextField = "SalesRep_Name"
                ddlVan.DataBind()

                '' ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --"))
                For Each itm As RadComboBoxItem In ddlVan.Items
                    itm.Checked = True
                Next

            End If
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
    Private Sub BindData()
        Dim SearchQuery As String = ""
        Dim SalesRepId As Integer = 0
        Dim CustId As Integer = 0
        Dim fromdate As DateTime
        Dim todate As DateTime
        fromdate = System.Data.SqlTypes.SqlDateTime.Null
        todate = System.Data.SqlTypes.SqlDateTime.Null
        Dim ObjReport As New SalesWorx.BO.Common.Reports

        Try
            rpbFilter.Items(0).Expanded = False
            lbl_org.Text = ddlOrganization.SelectedItem.Text

            ObjCustomer = New Customer()
            ObjCommon = New SalesWorx.BO.Common.Common()
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)


            Dim vantxt As String = ""
            Dim van As String = ""
            Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","
                vantxt = vantxt & li.Text & ","

            Next
            If vantxt <> "" Then
                vantxt = vantxt.Substring(0, vantxt.Length - 1)
                van = van.Substring(0, van.Length - 1)
            End If
            If van = "" Then
                van = "0"
            End If


            If van = "0" Then
                lbl_van.Text = "All"
            Else
                lbl_van.Text = vantxt
            End If


            SearchQuery = ""

            'If Val(van) = "0" Then
            '    SearchQuery = SearchQuery & " And A.SalesRep_ID in (" & ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID) & ")"
            'Else
            '    SearchQuery = SearchQuery & " And A.SalesRep_ID in (select item from SplitQuotedString('" & van & "'))"
            'End If

            lbl_CustomerType.Text = ddl_CustType.SelectedItem.Text

            If ddl_CustType.SelectedItem.Value = "Y" Then
                SearchQuery = SearchQuery & " And B.Cash_Cust='Y' "
            End If

            If ddl_CustType.SelectedItem.Value = "N" Then
                SearchQuery = SearchQuery & " And B.Cash_Cust='N' "
            End If
            'If ddlCustClass.SelectedValue <> "-- Select a Customer Class --" Then
            '    SearchQuery = SearchQuery & " And B.Customer_Class='" & ddlCustClass.SelectedValue & "'"
            'End If
            'If ddlType.SelectedValue <> "-- Select a Customer Type --" Then
            '    SearchQuery = SearchQuery & " And B.Customer_Type='" & ddlType.SelectedValue & "'"
            'End If
            If ddlCustomer.SelectedValue <> "" Then
                SearchQuery = SearchQuery & " AND (LTRIM(STR(C.Customer_ID)) + '$' + LTRIM(STR(C.Site_Use_ID)))='" & ddlCustomer.SelectedValue & "' "
                CustId = Convert.ToInt32(ddlCustomer.SelectedValue.Split("$")(0))

                lbl_Customer.Text = ddlCustomer.Text
            Else
                CustId = 0
                lbl_Customer.Text = "All"
            End If

            lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")

            If txtFromDate.DateInput.Text <> "" Then
                'Dim TemFromDateStr As String = txtFromDate.Text
                'Dim DateArr As Array = TemFromDateStr.Split("/")
                'TemFromDateStr = DateArr(1) & "/" & DateArr(0) & "/" & DateArr(2)
                SearchQuery = SearchQuery & " And A.Visit_Start_Date >=convert(datetime,'" & CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy") & "',103)"
                If txtToDate.DateInput.Text = "" Then
                    SearchQuery = SearchQuery & " And A.Visit_Start_Date <= convert(datetime,'" & CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy") & " 23:59:59',103)"
                End If
                fromdate = CDate(txtFromDate.SelectedDate)
            End If
            If txtToDate.DateInput.Text <> "" Then

                SearchQuery = SearchQuery & " And A.Visit_Start_Date <= convert(datetime,'" & CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy") & " 23:59:59 ',103)"
                todate = CDate(txtToDate.SelectedDate)
            End If

            If Not (ddlOrganization.SelectedItem.Value = "0") Then
                ' '' '' SearchQuery = SearchQuery & " And A.Site_Use_ID=" & ddlOrganization.SelectedItem.Value
                '' ''InitReportViewer(SearchQuery, fromdate, todate, SalesRepId, CType(Session("User_Access"), UserAccess).UserID, CustId)


                Dim dt As New DataTable
                dt = ObjReport.GetCustomerVisits(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, SearchQuery, van, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))
                

                If CType(Session.Item("CONTROL_PARAMS"), ControlParams).SHOW_GROUPS_IN_BO_REPORTS = "Y" Then


                    'Dim field As GridGroupByField = New GridGroupByField()
                    'field.FieldName = "VisitDate"
                    'field.HeaderText = "Visit Date "
                    'field.FormatString = "{0:dd-MMM-yyyy}"

                    'Dim fields As GridGroupByField = New GridGroupByField()
                    'fields.FieldName = "Row"



                    'Dim ex As GridGroupByExpression = New GridGroupByExpression()

                    'ex.GroupByFields.Add(field)

                    'ex.SelectFields.Add(field)


                    'Dim field1 As GridGroupByField = New GridGroupByField()
                    'field1.FieldName = "Salesrep_name"
                    'field1.HeaderText = "Van"
                    'Dim ex1 As GridGroupByExpression = New GridGroupByExpression()
                    'ex1.GroupByFields.Add(field1)
                    'ex1.SelectFields.Add(field1)

                    'gvRep.MasterTableView.GroupByExpressions.Add(ex)
                    'gvRep.MasterTableView.GroupByExpressions.Add(ex1)
                    'gvRep.Columns(0).Visible = False
                    'gvRep.Columns(1).Visible = False
                    gvRep.Visible = False
                    gvRep_grp.Visible = True
                    gvRep_grp.DataSource = dt
                    gvRep_grp.DataBind()
                    Dim ShowTradeLic As String = "N"
                    ShowTradeLic = (New SalesWorx.BO.Common.Common).GetAppConfig(Err_No, Err_Desc, "SHOW_TRADELIC_IN_BO_REP")
                    If ShowTradeLic = "Y" Then
                        gvRep_grp.MasterTableView.Columns.FindByUniqueName("TradeLic").Visible = True
                    Else
                        gvRep_grp.MasterTableView.Columns.FindByUniqueName("TradeLic").Visible = False
                    End If

                Else
                    If dt.Rows.Count > 0 Then
                        dt = dt.Select("1=1", "VisitDate ASC").CopyToDataTable()
                    End If
                    gvRep.Visible = True
                    gvRep_grp.Visible = False
                    gvRep.DataSource = dt
                    gvRep.DataBind()
                    Dim ShowTradeLic As String = "N"
                    ShowTradeLic = (New SalesWorx.BO.Common.Common).GetAppConfig(Err_No, Err_Desc, "SHOW_TRADELIC_IN_BO_REP")
                    If ShowTradeLic = "Y" Then
                        gvRep.MasterTableView.Columns.FindByUniqueName("TradeLic").Visible = True
                    Else
                        gvRep.MasterTableView.Columns.FindByUniqueName("TradeLic").Visible = False
                    End If

                End If
            Else
                MessageBoxValidation("Select an Organization.", "Validation")
            End If
        Catch ex As Exception
          
            If Err_Desc IsNot Nothing Then
                log.Error(Err_Desc)
            Else
                log.Error(ex.ToString())
            End If
            Err_No = "74067"
            '  Err_Desc = ex.Message
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
        Finally
            ObjCustomer = Nothing
            ObjReport = Nothing
        End Try
    End Sub

    Private Sub gvRep_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles gvRep.ItemCommand
        If e.CommandName = "TradeLic" Then
            Dim row As GridDataItem = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridDataItem)
            Dim HTrdLic As String = CType(row.FindControl("HTrdLic"), HiddenField).Value
            vimg.ImageUrl = HTrdLic
            Viewimage_Window.VisibleOnPageLoad = True
        End If
    End Sub
    Private Sub gvRep_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles gvRep.ItemDataBound
        Try
            If CType(Session.Item("CONTROL_PARAMS"), ControlParams).SHOW_GROUPS_IN_BO_REPORTS = "Y" Then
                If TypeOf e.Item Is GridDataItem Then
                    'Dim groupHeader As GridGroupHeaderItem = DirectCast(e.Item, GridGroupHeaderItem)
                    Dim item As GridDataItem = TryCast(e.Item, GridDataItem)
                    If item IsNot Nothing Then
                        If Not String.IsNullOrEmpty(hfDecimal.Value) Then
                            item.Cells(13).Text = FormatNumber(CDbl(item.Cells(13).Text), hfDecimal.Value)
                            item.Cells(15).Text = FormatNumber(CDbl(item.Cells(15).Text), hfDecimal.Value)
                            item.Cells(17).Text = FormatNumber(CDbl(item.Cells(17).Text), hfDecimal.Value)
                            item.Cells(19).Text = FormatNumber(CDbl(item.Cells(19).Text), hfDecimal.Value)
                            item.Cells(21).Text = FormatNumber(CDbl(item.Cells(21).Text), hfDecimal.Value)
                        End If
                        '' For Location
                        Dim locLink As HyperLink = TryCast(item.Cells(6).FindControl("lnkLocation"), HyperLink)
                        Dim hfLoc As HiddenField = TryCast(item.Cells(7).FindControl("hfLocation"), HiddenField)

                        If locLink IsNot Nothing AndAlso hfLoc IsNot Nothing Then
                            If hfLoc.Value = "Y" Then
                                locLink.Visible = True
                            Else
                                locLink.Visible = False
                            End If
                        End If

                        '' For DC
                        Dim hfDC As HiddenField = TryCast(item.Cells(7).FindControl("HfDC"), HiddenField)
                        Dim ImgYes As Image = TryCast(item.Cells(7).FindControl("DCYes"), Image)
                        Dim ImgNo As Image = TryCast(item.Cells(7).FindControl("DCNo"), Image)

                        If hfDC IsNot Nothing AndAlso ImgYes IsNot Nothing AndAlso ImgNo IsNot Nothing Then
                            If hfDC.Value = "Y" Then
                                ImgYes.Visible = True
                                ImgNo.Visible = False
                            Else
                                ImgYes.Visible = False
                                ImgNo.Visible = True
                            End If
                        End If

                        '' For DC
                        Dim hfCBD As HiddenField = TryCast(item.Cells(7).FindControl("HfCBD"), HiddenField)
                        Dim hfBD As HiddenField = TryCast(item.Cells(7).FindControl("HfBD"), HiddenField)
                        Dim ImgBDYes As Image = TryCast(item.Cells(7).FindControl("BDYes"), Image)
                        Dim ImgBDMaybe As Image = TryCast(item.Cells(7).FindControl("BDMaybe"), Image)
                        Dim ImgBDNo As Image = TryCast(item.Cells(7).FindControl("BDNo"), Image)

                        Dim ShowBeacon As String = "N"
                        ShowBeacon = (New SalesWorx.BO.Common.Common).GetAppConfig(Err_No, Err_Desc, "SHOW_BEACON_IN_CUST_VISIT_BO")
                        If ShowBeacon.ToUpper = "Y" Then
                            If hfCBD IsNot Nothing AndAlso hfBD IsNot Nothing AndAlso ImgBDYes IsNot Nothing AndAlso ImgBDNo IsNot Nothing Then
                                If hfCBD.Value = "Y" Then
                                    ImgBDYes.Visible = True
                                    ImgBDNo.Visible = False
                                    ImgBDMaybe.Visible = False
                                ElseIf hfBD.Value = "Y" Then
                                    ImgBDYes.Visible = False
                                    ImgBDMaybe.Visible = True
                                    ImgBDNo.Visible = False
                                Else
                                    ImgBDYes.Visible = False
                                    ImgBDMaybe.Visible = False
                                    ImgBDNo.Visible = True
                                End If
                            End If
                        Else
                            gvRep.MasterTableView.Columns.FindByUniqueName("BD").Visible = False
                        End If
                        

                    End If

                End If
            Else


                If TypeOf e.Item Is GridDataItem Then
                    'Dim groupHeader As GridGroupHeaderItem = DirectCast(e.Item, GridGroupHeaderItem)
                    Dim item As GridDataItem = TryCast(e.Item, GridDataItem)
                    If item IsNot Nothing Then

                        If Not String.IsNullOrEmpty(hfDecimal.Value) Then
                            item.Cells(11).Text = FormatNumber(CDbl(item.Cells(11).Text), hfDecimal.Value)
                            item.Cells(15).Text = FormatNumber(CDbl(item.Cells(15).Text), hfDecimal.Value)
                            item.Cells(17).Text = FormatNumber(CDbl(item.Cells(17).Text), hfDecimal.Value)
                            item.Cells(19).Text = FormatNumber(CDbl(item.Cells(19).Text), hfDecimal.Value)
                            item.Cells(13).Text = FormatNumber(CDbl(item.Cells(13).Text), hfDecimal.Value)
                        End If
                        '' For Location
                        Dim locLink As HyperLink = TryCast(item.Cells(6).FindControl("lnkLocation"), HyperLink)
                        Dim hfLoc As HiddenField = TryCast(item.Cells(7).FindControl("hfLocation"), HiddenField)

                        If locLink IsNot Nothing AndAlso hfLoc IsNot Nothing Then
                            If hfLoc.Value = "Y" Then
                                locLink.Visible = True
                            Else
                                locLink.Visible = False
                            End If
                        End If

                        '' For DC
                        Dim hfDC As HiddenField = TryCast(item.Cells(7).FindControl("HfDC"), HiddenField)
                        Dim ImgYes As Image = TryCast(item.Cells(7).FindControl("DCYes"), Image)
                        Dim ImgNo As Image = TryCast(item.Cells(7).FindControl("DCNo"), Image)

                        If hfDC IsNot Nothing AndAlso ImgYes IsNot Nothing AndAlso ImgNo IsNot Nothing Then
                            If hfDC.Value = "Y" Then
                                ImgYes.Visible = True
                                ImgNo.Visible = False
                            Else
                                ImgYes.Visible = False
                                ImgNo.Visible = True
                            End If
                        End If

                        '' For DC
                        Dim hfCBD As HiddenField = TryCast(item.Cells(7).FindControl("HfCBD"), HiddenField)
                        Dim hfBD As HiddenField = TryCast(item.Cells(7).FindControl("HfBD"), HiddenField)
                        Dim ImgBDYes As Image = TryCast(item.Cells(7).FindControl("BDYes"), Image)
                        Dim ImgBDMaybe As Image = TryCast(item.Cells(7).FindControl("BDMaybe"), Image)
                        Dim ImgBDNo As Image = TryCast(item.Cells(7).FindControl("BDNo"), Image)
                        Dim ShowBeacon As String = "N"
                        ShowBeacon = (New SalesWorx.BO.Common.Common).GetAppConfig(Err_No, Err_Desc, "SHOW_BEACON_IN_CUST_VISIT_BO")
                        If ShowBeacon.ToUpper = "Y" Then
                            If hfCBD IsNot Nothing AndAlso hfBD IsNot Nothing AndAlso ImgBDYes IsNot Nothing AndAlso ImgBDNo IsNot Nothing Then
                                If hfCBD.Value = "Y" Then
                                    ImgBDYes.Visible = True
                                    ImgBDNo.Visible = False
                                    ImgBDMaybe.Visible = False
                                ElseIf hfBD.Value = "Y" Then
                                    ImgBDYes.Visible = False
                                    ImgBDMaybe.Visible = True
                                    ImgBDNo.Visible = False
                                Else
                                    ImgBDYes.Visible = False
                                    ImgBDMaybe.Visible = False
                                    ImgBDNo.Visible = True
                                End If
                            End If
                        Else
                            gvRep.MasterTableView.Columns.FindByUniqueName("BD").Visible = False
                        End If
                    End If

                    End If
                End If
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
    Private Sub gvRep_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        BindData()
    End Sub

    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender
        Try
            Dim dtCur As New DataTable
            Dim ObjCommon As New SalesWorx.BO.Common.Common()
            dtCur = ObjCommon.GetCurrencyByOrg(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
            If dtCur.Rows.Count > 0 Then
                '  hfCurrency.Value = dtCur.Rows(0)(0).ToString()
                ' hfDecimal.Value = dtCur.Rows(0)(1).ToString()
                lblDecimal.Text = "N" & dtCur.Rows(0)(1).ToString()
            End If
            For Each column As GridColumn In gvRep.MasterTableView.Columns
                If column.UniqueName = "InvoiceValue" Then
                    Dim col As GridBoundColumn
                    col = (CType(column, GridBoundColumn))
                    column.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                    col.DataType = System.Type.GetType("System.Decimal")
                    col.DataFormatString = "{0:" & lblDecimal.Text & "}"
                ElseIf column.UniqueName = "SalesOrderValue" Then
                    Dim col As GridBoundColumn
                    col = (CType(column, GridBoundColumn))
                    col.DataType = System.Type.GetType("System.Decimal")
                    col.DataFormatString = "{0:" & lblDecimal.Text & "}"
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                ElseIf column.UniqueName = "ProformaValue" Then
                    Dim col As GridBoundColumn
                    col = (CType(column, GridBoundColumn))
                    col.DataType = System.Type.GetType("System.Decimal")
                    col.DataFormatString = "{0:" & lblDecimal.Text & "}"
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                ElseIf column.UniqueName = "ReturnValue" Then
                    Dim col As GridBoundColumn
                    col = (CType(column, GridBoundColumn))
                    col.DataType = System.Type.GetType("System.Decimal")
                    col.DataFormatString = "{0:" & lblDecimal.Text & "}"
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                ElseIf column.UniqueName = "PaymentValue" Then
                    Dim col As GridBoundColumn
                    col = (CType(column, GridBoundColumn))
                    col.DataType = System.Type.GetType("System.Decimal")
                    col.DataFormatString = "{0:" & lblDecimal.Text & "}"
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                End If
            Next



        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Private Sub gvRep_SortCommand(sender As Object, e As GridSortCommandEventArgs) Handles gvRep.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindData()
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
    Private Sub InitReportViewer(ByVal FilterValue As String, ByVal fromdate As Date, ByVal Todate As Date, ByVal SID As Integer, ByVal UID As Integer, ByVal CustID As String)
        Try
            '' ''rpbFilter.Items(0).Expanded = False
            '' ''RepSec.Visible = True
            '' ''RVMain.Visible = True
            '' ''Dim objUserAccess As UserAccess
            '' ''objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            '' ''Dim Searchvalue As New ReportParameter
            '' ''Searchvalue = New ReportParameter("SearchParams", CStr(IIf(FilterValue Is Nothing, "", FilterValue)))

            '' ''Dim VisitID As New ReportParameter
            '' ''VisitID = New ReportParameter("VisitID", "0")

            '' ''Dim OrgID As New ReportParameter
            '' ''OrgID = New ReportParameter("OrgID", CStr(ddlOrganization.SelectedValue.ToString()))

            '' ''Dim FDate As New ReportParameter
            '' ''FDate = New ReportParameter("FromDate", fromdate.ToString())

            '' ''Dim TDate As New ReportParameter
            '' ''TDate = New ReportParameter("ToDate", Todate.ToString())

            '' ''Dim SalesRepID As New ReportParameter
            '' ''SalesRepID = New ReportParameter("SID", SID)

            '' ''Dim USRID As New ReportParameter
            '' ''USRID = New ReportParameter("Uid", UID)

            '' ''Dim OID As New ReportParameter
            '' ''OID = New ReportParameter("OID", CStr(ddlOrganization.SelectedValue.ToString()))

            '' ''Dim CID As New ReportParameter
            '' ''CID = New ReportParameter("CustID", CustID)

            '' ''Dim _url As String = "/CustomerLocation.aspx?VisitId="
            '' ''Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            '' ''path = path.Replace("/RepCustomerVisits.aspx", _url)
            '' ''Dim MapPath As New ReportParameter
            '' ''MapPath = New ReportParameter("MapPath", path)

            '' ''Dim OrgName As New ReportParameter
            '' ''OrgName = New ReportParameter("OrgName", CStr(IIf(ddlOrganization.SelectedIndex = 0, "0", ddlOrganization.SelectedItem.Text.Trim())))

            '' ''With RVMain
            '' ''    .Reset()
            '' ''    .ShowParameterPrompts = False
            '' ''    .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            '' ''    .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            '' ''    .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
            '' ''    .ServerReport.SetParameters(New ReportParameter() {Searchvalue, OrgID, FDate, TDate, SalesRepID, OID, USRID, CID, MapPath, OrgName})
            '' ''    .ServerReport.Refresh()

            '' ''End With


        Catch Ex As Exception
            log.Error(Ex.ToString())
        End Try
    End Sub

    'Protected Function GetPrice(ByVal SP As Object, ByVal dec As Object) As String
    '    'Dim FormatString As String = "{0:N" + dec.ToString() + "}"
    '    hfDecimal.Value = Convert.ToInt32(dec).ToString()
    '    Return FormatNumber(SP, Convert.ToInt32(dec)).ToString()
    'End Function
    'Private Property SortDirection() As String
    '    Get
    '        If ViewState("SortDirection") Is Nothing Then
    '            ViewState("SortDirection") = "ASC"
    '        End If
    '        Return ViewState("SortDirection").ToString()
    '    End Get
    '    Set(ByVal value As String)
    '        Dim s As String = SortDirection

    '        If value = "flip" Then
    '            s = If(s = "ASC", "DESC", "ASC")
    '        Else
    '            s = value
    '        End If

    '        ViewState("SortDirection") = s
    '    End Set
    'End Property
    'Public Sub AddSortImage()
    '    If SortField = "" Then
    '        Exit Sub
    '    End If
    '    Dim sortImage As New Image()
    '    sortImage.Style("padding-left") = "8px"
    '    sortImage.Style("padding-bottom") = "1px"
    '    If SortDirection = "ASC" Then
    '        sortImage.ImageUrl = "~/images/arrowUp.gif"
    '        sortImage.AlternateText = "Ascending Order"
    '    Else
    '        sortImage.ImageUrl = "~/images/arrowDown.gif"
    '        sortImage.AlternateText = "Descending Order"
    '    End If
    '    For i As Integer = 0 To GVCustomerVisits.Columns.Count - 1
    '        Dim dcf As DataControlField = GVCustomerVisits.Columns(i)
    '        If dcf.SortExpression = SortField Then
    '            If Not IsNothing(GVCustomerVisits.HeaderRow) Then
    '                GVCustomerVisits.HeaderRow.Cells(i).Controls.Add(sortImage)
    '            End If
    '            Exit For
    '        End If
    '    Next
    'End Sub

    'Private Sub GVCustomerVisits_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GVCustomerVisits.PageIndexChanging
    '    GVCustomerVisits.PageIndex = e.NewPageIndex
    '    BindData()
    'End Sub

    'Private Sub GVCustomerVisits_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GVCustomerVisits.Sorting
    '    SortField = e.SortExpression
    '    SortDirection = "flip"
    '    BindData()
    'End Sub

    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click

        gvRep.Visible = False
        summary.InnerHtml = ""
        Args.Visible = False

        If Not (ddlOrganization.SelectedItem.Value = "0") Then

            If Not IsDate(txtFromDate.SelectedDate) Then
                MessageBoxValidation("Enter valid ""From date"".", "Validation")
                SetFocus(txtFromDate)
                Exit Sub
            End If

            If Not IsDate(txtToDate.SelectedDate) Then
                MessageBoxValidation("Enter valid ""To date"".", "Validation")
                SetFocus(txtToDate)
                Exit Sub
            End If

            If CDate(txtFromDate.SelectedDate) > CDate(txtToDate.SelectedDate) Then
                MessageBoxValidation("Start Date should not be greater than End Date.", "Validation")
                Exit Sub
            End If

            gvRep.Visible = True
            Args.Visible = True

            LoadSummary()

            BindData()

            'Response.Cookies.Add(New HttpCookie("CVOID", ddlOrganization.SelectedValue))
            'Response.Cookies.Add(New HttpCookie("CVSID", ddlVan.SelectedValue))
            'Response.Cookies.Add(New HttpCookie("CVFromDate", txtFromDate.Text.Trim()))
            'Response.Cookies.Add(New HttpCookie("CVToDate", txtToDate.Text.Trim()))
            'Response.Cookies.Add(New HttpCookie("CVCustomer", ddlCustomer.SelectedValue))

        Else
            MessageBoxValidation("Select an organization.", "Validation")
        End If
    End Sub

    Private Sub LoadSummary()
        Dim ObjReport As New SalesWorx.BO.Common.Reports
        Try



            Dim van As String = ""
            Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","
            Next

            If van = "" Then
                van = "0"
            Else
                van = van.Substring(0, van.Length - 1)
            End If



            Dim CustId As String = 0
            If ddlCustomer.SelectedValue <> "" Then
                CustId = Convert.ToInt32(ddlCustomer.SelectedValue.Split("$")(0))
            End If

            Dim dt As New DataTable
            dt = ObjReport.GetCustomerVisitsSummary(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, txtFromDate.SelectedDate, txtToDate.SelectedDate, van, CType(Session("User_Access"), UserAccess).UserID, CustId, ddl_CustType.SelectedItem.Value)

            Dim StrSummary As String = ""
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

                Dim i As Integer = 0
                For Each drow In dt.Rows
                    If drow("Description").ToString().Contains("calls") Then
                        If drow("Description").ToString().Contains("productive") Then
                            StrSummary = StrSummary & "<div class='col-sm-6 col-md-4'><div class='widgetblk'>" & drow("Description") & "<i class='fa fa-info-circle' data-toggle='tooltip' data-container='body' title='No. of Calls in which Invoice is taken'></i> <div class='text-primary'>" & Format(Val(drow("Amount").ToString()), "#,###") & "</div></div></div>"
                        Else
                            If CType(Session.Item("CONTROL_PARAMS"), ControlParams).USE_DISTR_IN_CALLS = "Y" Then
                                StrSummary = StrSummary & "<div class='col-sm-6 col-md-4'><div class='widgetblk'>" & drow("Description") & "<i class='fa fa-info-circle' data-toggle='tooltip' data-container='body' title='No. of Visits in which Distribution check was performed.'></i> <div class='text-primary'>" & Format(Val(drow("Amount").ToString()), "#,###") & "</div></div></div>"
                            Else
                                StrSummary = StrSummary & "<div class='col-sm-6 col-md-4'><div class='widgetblk'>" & drow("Description") & "<i class='fa fa-info-circle' data-toggle='tooltip' data-container='body' title='No. of Visits'></i>  <div class='text-primary'>" & Format(Val(drow("Amount").ToString()), "#,###") & "</div></div></div>"
                            End If
                        End If
                    ElseIf drow("Description").ToString().Contains("Productivity %") Then
                        StrSummary = StrSummary & "<div class='col-sm-6 col-md-4'><div class='widgetblk'>" & drow("Description") & "<i class='fa fa-info-circle' data-toggle='tooltip' data-container='body' title='No. of Productive Calls/No. of Calls'></i>  <div class='text-primary'>" & Format(Val(drow("Amount").ToString()), "N" & hfDecimal.Value) & " %</div></div></div>"
                    Else
                        StrSummary = StrSummary & "<div class='col-sm-6 col-md-4'><div class='widgetblk'>" & drow("Description") & " <div class='text-primary'>" & Format(Val(drow("Amount").ToString()), "N" & hfDecimal.Value) & " </div></div></div>"
                    End If

                    i = i + 1
                Next

            End If

            summary.InnerHtml = StrSummary

        Catch ex As Exception
            log.Error(ex.ToString())
        Finally
            ObjReport = Nothing
        End Try
    End Sub

    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        If Not (ddlOrganization.SelectedItem.Value = "0") Then

            LoadVan()

            '' ''Dim objUserAccess As UserAccess
            '' ''objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            '' ''ObjCommon = New SalesWorx.BO.Common.Common()
            '' ''ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
            '' ''ddlVan.DataBind()
            '' ''ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))


            ' ''Dim sqlstr As String = ""
            ' ''If ddl_CustType.SelectedItem.Value = "Y" Then
            ' ''    sqlstr = " And Cash_Cust='Y'"
            ' ''End If

            ' ''If ddl_CustType.SelectedItem.Value = "N" Then
            ' ''    sqlstr = " And Cash_Cust='N'"
            ' ''End If
            ' ''ddlCustomer.Items.Clear()
            ' ''ddlCustomer.DataSource = ObjCommon.GetCustomerByCriteria(Err_No, Err_Desc, ddlOrganization.SelectedValue, sqlstr)
            ' ''ddlCustomer.DataBind()
            ' ''ddlCustomer.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))

            ''Dim dt As New DataTable
            ' ''dt = ObjCommon.GetCurrencyByOrg(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
            ' ''If dt.Rows.Count > 0 Then
            ' ''    hfCurrency.Value = dt.Rows(0)(0).ToString()
            ' ''    hfDecimal.Value = dt.Rows(0)(1).ToString()
            ' ''End If
            '' ''RVMain.Reset()
        Else
            ddlVan.ClearSelection()
            ddlVan.Items.Clear()
            '' ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))

            ddlCustomer.ClearSelection()
            ddlCustomer.Items.Clear()
            ddlCustomer.Text = ""
        End If

    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub


    'Protected Sub imgPrint_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgPrint.Click

    '    GVCustomerVisits.AllowPaging = False
    '    Dim dt As New DataTable
    '    dt = ViewState("dt")
    '    GVCustomerVisits.DataSource = dt
    '    GVCustomerVisits.DataBind()
    '    Dim sw As New StringWriter()
    '    Dim hw As New HtmlTextWriter(sw)
    '    GVCustomerVisits.RenderControl(hw)
    '    Dim gridHTML As String = sw.ToString().Replace("""", "'") _
    '       .Replace(System.Environment.NewLine, "")
    '    Dim sb As New StringBuilder()
    '    sb.Append("<script type = 'text/javascript'>")
    '    sb.Append("window.onload = new function(){")
    '    sb.Append("var printWin = window.open('', '', 'left=0")
    '    sb.Append(",top=0,width=1000,height=1000,status=0');")
    '    sb.Append("printWin.document.write(""")
    '    sb.Append(gridHTML)
    '    sb.Append(""");")
    '    sb.Append("printWin.document.close();")
    '    sb.Append("printWin.focus();")
    '    sb.Append("printWin.print();")
    '    sb.Append("printWin.close();};")
    '    sb.Append("</script>")
    '    ClientScript.RegisterStartupScript(Me.[GetType](), "GridPrint", sb.ToString())
    '    GVCustomerVisits.AllowPaging = True
    '    GVCustomerVisits.DataBind()

    'End Sub
    'Protected Sub Cust_Click(ByVal sender As Object, ByVal e As EventArgs)
    '    Try
    '        Dim btnCust As LinkButton = TryCast(sender, LinkButton)

    '        Response.Cookies.Add(New HttpCookie("CVOID", ddlOrganization.SelectedValue))
    '        Response.Cookies.Add(New HttpCookie("CVSID", ddlVan.SelectedValue))
    '        Response.Cookies.Add(New HttpCookie("CVFromDate", txtFromDate.Text.Trim()))
    '        Response.Cookies.Add(New HttpCookie("CVToDate", txtToDate.Text.Trim()))
    '        Response.Cookies.Add(New HttpCookie("CVCustomer", ddlCustomer.SelectedValue))
    '        Response.Redirect(btnCust.CommandArgument)
    '    Catch ex As Exception

    '    End Try
    'End Sub

    Private Sub ddl_CustType_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddl_CustType.SelectedIndexChanged
        ' ''Dim sqlstr As String = ""
        ' ''If ddl_CustType.SelectedItem.Value = "Y" Then
        ' ''    sqlstr = " And Cash_Cust='Y'"
        ' ''End If

        ' ''If ddl_CustType.SelectedItem.Value = "N" Then
        ' ''    sqlstr = " And Cash_Cust='N'"
        ' ''End If
        ' ''ddlCustomer.Items.Clear()
        ' ''ObjCommon = New SalesWorx.BO.Common.Common
        ' ''ddlCustomer.DataSource = ObjCommon.GetCustomerByCriteria(Err_No, Err_Desc, ddlOrganization.SelectedValue, sqlstr)
        ' ''ddlCustomer.DataBind()
        ' ''ddlCustomer.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))

        ddlCustomer.ClearSelection()
        ddlCustomer.Items.Clear()
        ddlCustomer.Text = ""

    End Sub

    Private Sub ddlCustomer_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddlCustomer.ItemsRequested

        Dim ObjCommon As New SalesWorx.BO.Common.Common()
        Try
            Dim dt As New DataTable

            Dim sqlstr As String = ""
            If ddl_CustType.SelectedItem.Value = "Y" Then
                sqlstr = " And Cash_Cust='Y'"
            End If

            If ddl_CustType.SelectedItem.Value = "N" Then
                sqlstr = " And Cash_Cust='N'"
            End If
            ddlCustomer.Items.Clear()


            ' ''ddlCustomer.DataSource = ObjCommon.GetCustomerByCriteria(Err_No, Err_Desc, ddlOrganization.SelectedValue, sqlstr)


            ''dt = Objrep.GetCustomerfromOrgtext(Err_No, Err_Desc, ddlOrganization.SelectedValue, e.Text)
            dt = ObjCommon.GetCustomerByCriteriaandText(Err_No, Err_Desc, ddlOrganization.SelectedValue, sqlstr, e.Text)

            Dim ItemsPerRequest As Integer = 100 'Set the number of values to show
            Dim itemOffset As Integer = e.NumberOfItems
            Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
            e.EndOfItems = endOffset = dt.Rows.Count

            'Loop through the values to populate the combo box
            For i As Integer = itemOffset To endOffset - 1
                Dim item As New RadComboBoxItem()
                item.Text = dt.Rows(i).Item("Customer").ToString
                item.Value = dt.Rows(i).Item("CustomerID").ToString

                ddlCustomer.Items.Add(item)
                item.DataBind()
            Next
        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            ObjCommon = Nothing
        End Try

    End Sub


    Private Sub BtnExportExcel_Click(sender As Object, e As EventArgs) Handles BtnExportExcel.Click
        Try
            If ddlOrganization.SelectedValue > 0 Then
                Export("Excel")
            Else
                MessageBoxValidation("Select an organization.", "Validation")
                Exit Sub
            End If
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub
    Sub Export(format As String)
        Dim ObjCommon As SalesWorx.BO.Common.Common
        Try

            ObjCommon = New SalesWorx.BO.Common.Common()
            Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
            rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)



            Dim SalesRepID As String
            Dim SearchQuery As String = ""

            Dim CustId As String
            Dim Site_ID As String
            Dim fromdate As DateTime
            Dim todate As DateTime

            'If ddlVan.SelectedValue <> "" Then
            '    SearchQuery = " And A.SalesRep_ID=" & ddlVan.SelectedValue
            '    SalesRepID = ddlVan.SelectedValue
            '    lbl_van.Text = ddlVan.SelectedItem.Text
            'Else
            '    SalesRepID = 0
            '    SearchQuery = " And A.SalesRep_ID in (" & ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID) & ")"
            '    lbl_van.Text = "All"
            'End If

            Dim vantxt As String = ""
            Dim van As String = ""
            Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","
            Next
            If van <> "" Then
                van = van.Substring(0, van.Length - 1)
            End If
            If van = "" Then
                van = "0"
            End If



            ''      SearchQuery = ""

            'If Val(van) = "0" Then
            '    SearchQuery = SearchQuery & " And A.SalesRep_ID in (" & ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID) & ")"
            'Else
            '    SearchQuery = SearchQuery & " And A.SalesRep_ID in (select item from SplitQuotedString('" & van & "'))"
            'End If


            lbl_CustomerType.Text = ddl_CustType.SelectedItem.Text

            If ddl_CustType.SelectedItem.Value = "Y" Then
                SearchQuery = " And B.Cash_Cust='Y' "
            End If

            If ddl_CustType.SelectedItem.Value = "N" Then
                SearchQuery = " And B.Cash_Cust='N' "
            End If

            If ddlCustomer.SelectedValue <> "" Then
                SearchQuery = SearchQuery & " AND (LTRIM(STR(C.Customer_ID)) + '$' + LTRIM(STR(C.Site_Use_ID)))='" & ddlCustomer.SelectedValue & "' "
                CustId = Convert.ToInt32(ddlCustomer.SelectedValue.Split("$")(0))
                Site_ID = Convert.ToInt32(ddlCustomer.SelectedValue.Split("$")(1))
                lbl_Customer.Text = ddlCustomer.Text
            Else
                CustId = 0
                Site_ID = 0
                lbl_Customer.Text = "All"
            End If

            'lbl_from.Text = txtFromDate.SelectedDate.ToString()
            'lbl_To.Text = txtToDate.SelectedDate.ToString()

            If txtFromDate.DateInput.Text <> "" Then
                SearchQuery = SearchQuery & " And A.Visit_Start_Date >=convert(datetime,'" & CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy") & "',103)"
                If txtToDate.DateInput.Text = "" Then
                    SearchQuery = SearchQuery & " And A.Visit_Start_Date <= convert(datetime,'" & CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy") & " 23:59:59',103)"
                End If
                fromdate = CDate(txtFromDate.SelectedDate)
            End If
            If txtToDate.DateInput.Text <> "" Then

                SearchQuery = SearchQuery & " And A.Visit_Start_Date <= convert(datetime,'" & CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy") & " 23:59:59 ',103)"
                todate = CDate(txtToDate.SelectedDate)
            End If



            Dim uid As Integer = CType(Session("User_Access"), UserAccess).UserID

            Dim VisitID As New ReportParameter
            VisitID = New ReportParameter("VisitID", "0")

            Dim OrgID As New ReportParameter
            OrgID = New ReportParameter("OrgID", CStr(ddlOrganization.SelectedValue.ToString()))

            Dim FDate As New ReportParameter
            FDate = New ReportParameter("FromDate", fromdate.ToString("dd-MMM-yyyy"))

            Dim TDate As New ReportParameter
            TDate = New ReportParameter("ToDate", todate.ToString("dd-MMM-yyyy"))

            Dim SID As New ReportParameter
            SID = New ReportParameter("SID", van)

            Dim USRID As New ReportParameter
            USRID = New ReportParameter("Uid", uid)

            ''Dim OID As New ReportParameter
            ''OID = New ReportParameter("OID", CStr(ddlOrganization.SelectedValue.ToString()))

            Dim CID As New ReportParameter
            CID = New ReportParameter("CustID", CustId)

            Dim SiteID As New ReportParameter
            SiteID = New ReportParameter("SiteID", Site_ID)


            Dim Type As New ReportParameter
            Type = New ReportParameter("Type", ddl_CustType.SelectedItem.Value)

            Dim Searchvalue As New ReportParameter
            Searchvalue = New ReportParameter("SearchParams", SearchQuery)

            Dim _url As String = "/CustomerLocation.aspx?VisitId="
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            path = path.Replace("/RepCustomerVisits.aspx", _url)
            Dim MapPath As New ReportParameter
            MapPath = New ReportParameter("MapPath", path)

            Dim OrgName As New ReportParameter
            OrgName = New ReportParameter("OrgName", CStr(IIf(ddlOrganization.SelectedIndex = 0, "0", ddlOrganization.SelectedItem.Text.Trim())))




            rview.ServerReport.SetParameters(New ReportParameter() {Searchvalue, OrgID, FDate, TDate, SID, USRID, CID, MapPath, OrgName, SiteID, Type})

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
                Response.AddHeader("Content-disposition", "attachment;filename=CustomerVisits.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=CustomerVisits.xls")
                Response.AddHeader("Content-Length", bytes.Length)
            End If
            Response.OutputStream.Write(bytes, 0, bytes.Length)
            Response.OutputStream.Flush()
            Response.OutputStream.Close()
            Response.Flush()
            Response.Close()
        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            ObjCommon = Nothing
        End Try
    End Sub

    Private Sub BtnExportPDF_Click(sender As Object, e As EventArgs) Handles BtnExportPDF.Click
        Try
            If ddlOrganization.SelectedValue > 0 Then
                Export("PDF")
            Else
                MessageBoxValidation("Select an organization.", "Validation")
                Exit Sub
            End If
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub

    Protected Sub ViewDetails_Click(ByVal sender As Object, ByVal e As EventArgs)

        Dim btnEdit As LinkButton = TryCast(sender, LinkButton)
        Dim row As Telerik.Web.UI.GridDataItem = DirectCast(btnEdit.NamingContainer, Telerik.Web.UI.GridDataItem)
        Dim sid As String = CType(row.FindControl("HVisitID"), HiddenField).Value

        Session("CustName") = CType(row.FindControl("HCusName"), HiddenField).Value
        Session("Van") = CType(row.FindControl("HSName"), HiddenField).Value
        Session("Date") = CDate(CType(row.FindControl("HVisitDate"), HiddenField).Value).ToString("dd/MMM/yyyy")

        '' Setting Current selections
        Session("CurOrg") = ddlOrganization.SelectedValue
        Session("CurVan") = ddlVan.SelectedValue
        Session("CurCusType") = ddl_CustType.SelectedValue

        ''If ddlCustomer.SelectedValue <> "" Then
        ''    Dim ID As String() = ddlCustomer.Text.Split("-")
        ''    If ID.Length > 1 Then
        ''        Session("CurCus") = ID(1)
        ''    End If
        ''Else
        ''    Session("CurCus") = ddlCustomer.Text
        ''End If

        Session("CurCus") = ddlCustomer.Text
        Session("CurFDat") = txtFromDate.SelectedDate
        Session("CurTDat") = txtToDate.SelectedDate
        Session("CurPIndex") = gvRep.CurrentPageIndex

        Response.Redirect("Rep_CustomerVisitDetail.aspx?ID=" & sid & "&OID=" & ddlOrganization.SelectedItem.Value & "&d=" & hfDecimal.Value & "&FromDate=" & CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy") & "&ToDate=" & CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))

    End Sub


    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        ddl_CustType.ClearSelection()
        ddlCustomer.ClearSelection()
        ddlCustomer.Text = ""
        If Not Request.QueryString("ID") Is Nothing Then
            Dim s() As String = Request.QueryString("ID").Split("$")
            Dim SID As String = s(0).ToString()
            Dim MonthYear As DateTime = DateTime.Parse(s(1).ToString())
            txtFromDate.SelectedDate = MonthYear
            Dim endOfMonth As DateTime = New DateTime(MonthYear.Year, MonthYear.Month, DateTime.DaysInMonth(MonthYear.Year, MonthYear.Month))
            txtToDate.SelectedDate = endOfMonth
        Else
            txtFromDate.SelectedDate = FirstDayOfMonth(Now().Date)
            txtToDate.SelectedDate = Now().Date

        End If
        ddlVan.ClearCheckedItems()
        LoadVan()
        gvRep.Visible = False
        summary.InnerHtml = ""
        Args.Visible = False
    End Sub

    Private Sub gvRep_grp_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles gvRep_grp.ItemCommand
        If e.CommandName = "TradeLic" Then
            Dim row As GridDataItem = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridDataItem)
            Dim HTrdLic As String = CType(row.FindControl("HTrdLic"), HiddenField).Value
            vimg.ImageUrl = HTrdLic
            Viewimage_Window.VisibleOnPageLoad = True
        End If
    End Sub

    Private Sub gvRep_grp_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles gvRep_grp.ItemDataBound
        Try

            If TypeOf e.Item Is GridDataItem Then
                'Dim groupHeader As GridGroupHeaderItem = DirectCast(e.Item, GridGroupHeaderItem)
                Dim item As GridDataItem = TryCast(e.Item, GridDataItem)
                If item IsNot Nothing Then
                    If Not String.IsNullOrEmpty(hfDecimal.Value) Then
                        item.Cells(11).Text = FormatNumber(CDbl(item.Cells(11).Text), hfDecimal.Value)
                        item.Cells(13).Text = FormatNumber(CDbl(item.Cells(13).Text), hfDecimal.Value)
                        item.Cells(15).Text = FormatNumber(CDbl(item.Cells(15).Text), hfDecimal.Value)
                        item.Cells(17).Text = FormatNumber(CDbl(item.Cells(17).Text), hfDecimal.Value)
                        item.Cells(19).Text = FormatNumber(CDbl(item.Cells(19).Text), hfDecimal.Value)
                    End If
                    '' For Location
                    Dim locLink As HyperLink = TryCast(item.Cells(4).FindControl("lnkLocation"), HyperLink)
                    Dim hfLoc As HiddenField = TryCast(item.Cells(5).FindControl("hfLocation"), HiddenField)

                    If locLink IsNot Nothing AndAlso hfLoc IsNot Nothing Then
                        If hfLoc.Value = "Y" Then
                            locLink.Visible = True
                        Else
                            locLink.Visible = False
                        End If
                    End If

                    '' For DC
                    Dim hfDC As HiddenField = TryCast(item.Cells(5).FindControl("HfDC"), HiddenField)
                    Dim ImgYes As Image = TryCast(item.Cells(5).FindControl("DCYes"), Image)
                    Dim ImgNo As Image = TryCast(item.Cells(5).FindControl("DCNo"), Image)

                    If hfDC IsNot Nothing AndAlso ImgYes IsNot Nothing AndAlso ImgNo IsNot Nothing Then
                        If hfDC.Value = "Y" Then
                            ImgYes.Visible = True
                            ImgNo.Visible = False
                        Else
                            ImgYes.Visible = False
                            ImgNo.Visible = True
                        End If
                    End If

                    '' For DC
                    Dim hfCBD As HiddenField = TryCast(item.Cells(5).FindControl("HfCBD"), HiddenField)
                    Dim hfBD As HiddenField = TryCast(item.Cells(5).FindControl("HfBD"), HiddenField)
                    Dim ImgBDYes As Image = TryCast(item.Cells(5).FindControl("BDYes"), Image)
                    Dim ImgBDMaybe As Image = TryCast(item.Cells(5).FindControl("BDMaybe"), Image)
                    Dim ImgBDNo As Image = TryCast(item.Cells(5).FindControl("BDNo"), Image)
                    Dim ShowBeacon As String = "N"
                    ShowBeacon = (New SalesWorx.BO.Common.Common).GetAppConfig(Err_No, Err_Desc, "SHOW_BEACON_IN_CUST_VISIT_BO")
                    If ShowBeacon.ToUpper = "Y" Then
                        If hfCBD IsNot Nothing AndAlso hfBD IsNot Nothing AndAlso ImgBDYes IsNot Nothing AndAlso ImgBDNo IsNot Nothing Then
                            If hfCBD.Value = "Y" Then
                                ImgBDYes.Visible = True
                                ImgBDNo.Visible = False
                                ImgBDMaybe.Visible = False
                            ElseIf hfBD.Value = "Y" Then
                                ImgBDYes.Visible = False
                                ImgBDMaybe.Visible = True
                                ImgBDNo.Visible = False
                            Else
                                ImgBDYes.Visible = False
                                ImgBDMaybe.Visible = False
                                ImgBDNo.Visible = True
                            End If
                        End If
                    Else
                        gvRep_grp.MasterTableView.Columns.FindByUniqueName("BD").Visible = False
                    End If
                End If

                End If




        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub gvRep_grp_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvRep_grp.PageIndexChanged
        BindData()
    End Sub

    Private Sub gvRep_grp_PreRender(sender As Object, e As EventArgs) Handles gvRep_grp.PreRender
        Try
            Dim dtCur As New DataTable
            Dim ObjCommon As New SalesWorx.BO.Common.Common()
            dtCur = ObjCommon.GetCurrencyByOrg(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
            If dtCur.Rows.Count > 0 Then
                '  hfCurrency.Value = dtCur.Rows(0)(0).ToString()
                ' hfDecimal.Value = dtCur.Rows(0)(1).ToString()
                lblDecimal.Text = "N" & dtCur.Rows(0)(1).ToString()
            End If
            For Each column As GridColumn In gvRep.MasterTableView.Columns
                If column.UniqueName = "InvoiceValue_grp" Then
                    Dim col As GridBoundColumn
                    col = (CType(column, GridBoundColumn))
                    column.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                    col.DataType = System.Type.GetType("System.Decimal")
                    col.DataFormatString = "{0:" & lblDecimal.Text & "}"
                ElseIf column.UniqueName = "SalesOrderValue_grp" Then
                    Dim col As GridBoundColumn
                    col = (CType(column, GridBoundColumn))
                    col.DataType = System.Type.GetType("System.Decimal")
                    col.DataFormatString = "{0:" & lblDecimal.Text & "}"
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                ElseIf column.UniqueName = "ProformaValue_grp" Then
                    Dim col As GridBoundColumn
                    col = (CType(column, GridBoundColumn))
                    col.DataType = System.Type.GetType("System.Decimal")
                    col.DataFormatString = "{0:" & lblDecimal.Text & "}"
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                ElseIf column.UniqueName = "ReturnValue_grp" Then
                    Dim col As GridBoundColumn
                    col = (CType(column, GridBoundColumn))
                    col.DataType = System.Type.GetType("System.Decimal")
                    col.DataFormatString = "{0:" & lblDecimal.Text & "}"
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                ElseIf column.UniqueName = "PaymentValue_grp" Then
                    Dim col As GridBoundColumn
                    col = (CType(column, GridBoundColumn))
                    col.DataType = System.Type.GetType("System.Decimal")
                    col.DataFormatString = "{0:" & lblDecimal.Text & "}"
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                End If
            Next



        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Private Sub gvRep_grp_SortCommand(sender As Object, e As GridSortCommandEventArgs) Handles gvRep_grp.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindData()
    End Sub
End Class

'Public Class Vist_Info
'    Public VisitDate As String
'    Public CustID As Integer
'End Class