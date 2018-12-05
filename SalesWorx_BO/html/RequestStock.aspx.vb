'Imports System.Data.OleDb
'Imports System.IO
'Imports log4net
'Imports System.Data.SqlClient
'Imports SalesWorx.BO.Common
'Imports ExcelLibrary.SpreadSheet
'Imports Telerik.Web.UI
'Imports System.Data.SqlTypes
'Imports System.Data
'Public Class RequestStock
'    Inherits System.Web.UI.Page
'    Private _objDB As SalesWorx.BO.DAL.DatabaseConnection
'    Dim ObjCommon As SalesWorx.BO.Common.Common
'    Public Sub New()
'        _objDB = New SalesWorx.BO.DAL.DatabaseConnection
'    End Sub
'    Dim Err_No As Long
'    Dim Err_Desc As String

'    Private Const ModuleName As String = "RequestStock.aspx"
'    Private Const PageID As String = "P3630"
'    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
'    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
'        Try


'            If Not Page.IsPostBack() Then
'                If Session.Item("USER_ACCESS") Is Nothing Then
'                    Session.Add("BringmeBackHere", ModuleName)
'                    Response.Redirect("Login.aspx", False)
'                    Exit Sub
'                End If
'                'If Not HasAuthentication() Then
'                '    Err_No = 500
'                '    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
'                'End If
'                FilldropdownsAndList()
'                If Not Session("StNFrom") Is Nothing Then
'                    txtFromDate.SelectedDate = Session("StNFrom")
'                Else
'                    txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)

'                End If
'                If Not Session("StNTo") Is Nothing Then
'                    txtToDate.SelectedDate = Session("StNTo")
'                Else
'                    txtToDate.SelectedDate = Now()
'                End If
'                ViewState("Criteria") = "1=1"
'                Session.Remove("Stock_Unconfirm")
'            End If

'        Catch ex As Exception
'            log.Error(GetExceptionInfo(ex))
'        End Try
'    End Sub
'    Private Function HasAuthentication() As Boolean
'        Dim objUserAccess As UserAccess
'        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
'        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
'    End Function
'    Private Sub RequestStocktab_TabClick(sender As Object, e As RadTabStripEventArgs) Handles RequestStocktab.TabClick
'        'If Args.Visible = True Then
'        Dim objCommon As New SalesWorx.BO.Common.Common
'        If RequestStocktab.Tabs(0).Selected = True Then


'            If Not Session("StNFrom") Is Nothing Then
'                txtFromDate.SelectedDate = Session("StNFrom")
'            Else
'                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)

'            End If
'            If Not Session("StNTo") Is Nothing Then
'                txtToDate.SelectedDate = Session("StNTo")
'            Else
'                txtToDate.SelectedDate = Now()
'            End If
'            'ddl_org1.ClearSelection()
'            'If ddl_org1.Items.Count = 2 Then
'            '    ddl_org1.SelectedIndex = 1
'            'End If
'            'Args.Visible = False
'            ddlVan1.ClearCheckedItems()
'            ' ddlVan1.Items.Clear()
'            '   ddlVan1.Text = "Please type product code/ name"

'            ddl_Status.ClearSelection()
'            ' BindReport()
'            Dim dt As New DataTable
'            Dim SearchQuery As String = ""

'            dt = ObjCommon.GetStockRequestListFilter(Err_No, Err_Desc, SearchQuery, ddl_org.SelectedItem.Value)
'            rgStockRequests.DataSource = dt
'            rgStockRequests.DataBind()
'        End If
'        If RequestStocktab.Tabs(1).Selected = True Then
'            ddl_org1.ClearSelection()
'            Dim dt1 As New DataTable
'            rgStockRequests.DataSource = dt1
'            rgStockRequests.DataBind()
'            'ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
'        End If

'        'End If
'    End Sub
'    Sub FilldropdownsAndList()
'        Try


'            Dim objCommon As New SalesWorx.BO.Common.Common

'            Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
'            ddl_org.DataSource = (New SalesWorx.BO.Common.Product).GetOrganisation(Err_No, Err_Desc, SubQry)
'            ddl_org.Items.Clear()
'            ddl_org.Items.Insert(0, New RadComboBoxItem("Select Organization"))
'            ddl_org.AppendDataBoundItems = True
'            ddl_org.DataValueField = "MAS_Org_ID"
'            ddl_org.DataTextField = "Description"
'            ddl_org.DataBind()
'            ddl_org.Items(0).Value = 0

'            ddl_org1.DataSource = (New SalesWorx.BO.Common.Product).GetOrganisation(Err_No, Err_Desc, SubQry)
'            ddl_org1.Items.Clear()
'            ddl_org1.Items.Insert(0, New RadComboBoxItem("Select Organization"))
'            ddl_org1.AppendDataBoundItems = True
'            ddl_org1.DataValueField = "MAS_Org_ID"
'            ddl_org1.DataTextField = "Description"
'            ddl_org1.DataBind()
'            ddl_org1.Items(0).Value = 0

'            'ddl_Status.DataSource = objCommon.GetStockRequestStatus(Err_No, Err_Desc, "'R'")
'            'ddl_Status.DataTextField = "Status"
'            'ddl_Status.DataValueField = "StatusKey"
'            'ddl_Status.DataBind()
'            ddl_Status.Items.Insert(0, New RadComboBoxItem("Pending", "N"))
'            ddl_Status.Items.Insert(1, New RadComboBoxItem("Processed", "Y"))

'            Dim dt As New DataTable
'            Dim SearchQuery As String = ""

'            dt = ObjCommon.GetStockRequestListFilter(Err_No, Err_Desc, SearchQuery, ddl_org.SelectedItem.Value)
'            rgStockRequests.DataSource = dt
'            rgStockRequests.DataBind()

'            'Dim objUserAccess As UserAccess
'            'objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

'            'ddlVan.DataSource = objCommon.GetVanByOrg(Err_No, Err_Desc, ddl_org.SelectedValue, objUserAccess.UserID)
'            'ddlVan.DataValueField = "SalesRep_ID"
'            'ddlVan.DataTextField = "SalesRep_Name"
'            'ddlVan.DataBind()
'            'ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))

'            'objCommon = Nothing
'        Catch ex As Exception
'            log.Error(GetExceptionInfo(ex))
'        End Try
'    End Sub
'    Private Sub ddl_org_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_org.SelectedIndexChanged

'        Try


'            Dim objUserAccess As UserAccess
'            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
'            Dim objCommon As New SalesWorx.BO.Common.Common
'            ddlVan.DataSource = objCommon.GetVanOrgIdByOrg(Err_No, Err_Desc, ddl_org.SelectedValue, objUserAccess.UserID)
'            ddlVan.DataValueField = "Org_ID"
'            ddlVan.DataTextField = "SalesRep_Name"
'            ddlVan.DataBind()
'            'ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))
'            For Each itm As RadComboBoxItem In ddlVan.Items
'                itm.Checked = True
'            Next
'            objCommon = Nothing
'        Catch ex As Exception
'            log.Error(GetExceptionInfo(ex))

'        End Try
'    End Sub

'    Protected Sub BTn_Request_Click(sender As Object, e As EventArgs) Handles BTn_Request.Click
'        Dim listPendingRequests As New List(Of String)
'        Dim listNewlyCreatedRequests As New List(Of String)
'        If ddl_org.SelectedItem.Value = "0" Then
'            MessageBoxValidation("Please select organization.", "Validation")
'            Return
'        End If
'        For Each itm As RadComboBoxItem In ddlVan.Items
'            If itm.Checked = True Then

'                Dim VanOrgID As String
'                VanOrgID = itm.Value()
'                If Not StockRequestExists(VanOrgID) Then
'                    If CreateNewStockRequest(VanOrgID) = True Then
'                        VanOrgID = "'" + VanOrgID + "'"
'                        listNewlyCreatedRequests.Add(VanOrgID)

'                        ' MessageBoxValidation("StockRequests added successfully.", "Information")
'                    End If
'                Else
'                    VanOrgID = "'" + VanOrgID + "'"
'                    listPendingRequests.Add(VanOrgID)

'                End If

'            End If

'        Next
'        Dim strNewlyCreatedRequests As String = ""
'        Dim strPendingRequests As String = ""
'        If listPendingRequests.Count > 0 Then


'            strPendingRequests = String.Join(",", listPendingRequests)
'            'Dim dt As New DataTable
'            'dt = GetPendingStockRequests(strPendingRequests)
'            'If dt.Rows.Count > 0 Then
'            '    gv_InvalidStock.DataSource = dt
'            '    gv_InvalidStock.DataBind()
'            '    MpInfoError.Show()
'            'End If
'        End If

'        If listNewlyCreatedRequests.Count > 0 Then


'            strNewlyCreatedRequests = String.Join(",", listNewlyCreatedRequests)
'            'Dim dt As New DataTable
'            'dt = GetPendingStockRequests(strPendingRequests)
'            'If dt.Rows.Count > 0 Then
'            '    gv_InvalidStock.DataSource = dt
'            '    gv_InvalidStock.DataBind()
'            '    MpInfoError.Show()
'            'End If
'        End If

'        Dim dt As New DataTable
'        dt = GetPendingStockRequests(strPendingRequests, strNewlyCreatedRequests)
'        If dt.Rows.Count > 0 Then
'            gv_InvalidStock.DataSource = dt
'            gv_InvalidStock.DataBind()
'            MpInfoError.Show()
'        End If
'        'Response.Redirect("~/html/RequestStock.html")
'        Dim dt1 As New DataTable
'        Dim SearchQuery As String = ""
'        Dim objCommon As New SalesWorx.BO.Common.Common
'        dt1 = ObjCommon.GetStockRequestListFilter(Err_No, Err_Desc, SearchQuery, ddl_org.SelectedItem.Value)
'        rgStockRequests.DataSource = dt1
'        rgStockRequests.DataBind()
'        ddlVan.ClearCheckedItems()
'        'ddlVan.Items.Clear()



'    End Sub
'    Public Function GetPendingStockRequests(ByVal strPendingRequests As String, ByVal strNewlyCreatedRequests As String) As DataTable
'        Dim objSQLConn As SqlConnection
'        Dim objSQLDA As SqlDataAdapter
'        Dim objSQLDA1 As SqlDataAdapter
'        Dim dtDivConfig As New DataTable

'        Try

'            objSQLConn = _objDB.GetSQLConnection
'            If strPendingRequests <> "" Then
'                objSQLDA = New SqlDataAdapter("SELECT B.Logged_At as 'Date Request Created' ,'Can not add a new Request as there already exists a pending request' as message,C.SalesRep_Name as VAN,case  when B.Status='N' then 'Pending' when B.Status='Y' then 'Processed'end  Status FROM TBL_Org_CTL_DTL AS A inner join TBL_Opening_Stock_Requests as B On A.Org_ID =B.Van_Org_ID inner join tbl_fsr as C On A.SalesRep_ID = C.SalesRep_ID where B.Status= 'N' and B.Van_Org_ID in (" + strPendingRequests + ") ", objSQLConn)
'                objSQLDA.SelectCommand.CommandType = CommandType.Text
'                objSQLDA.Fill(dtDivConfig)
'                objSQLDA.Dispose()
'            End If
'            If strNewlyCreatedRequests <> "" Then
'                objSQLDA1 = New SqlDataAdapter("SELECT B.Logged_At as 'Date Request Created' ,'Request added' as message,C.SalesRep_Name as VAN,case  when B.Status='N' then 'Pending' when B.Status='Y' then 'Processed'end  Status FROM TBL_Org_CTL_DTL AS A inner join TBL_Opening_Stock_Requests as B On A.Org_ID =B.Van_Org_ID inner join tbl_fsr as C On A.SalesRep_ID = C.SalesRep_ID where B.Status= 'N' and B.Van_Org_ID in (" + strNewlyCreatedRequests + ") ", objSQLConn)
'                objSQLDA1.SelectCommand.CommandType = CommandType.Text
'                objSQLDA1.Fill(dtDivConfig)
'                objSQLDA1.Dispose()
'            End If
'        Catch ex As Exception
'            Err_No = "74204"
'            Err_Desc = ex.Message
'            Throw ex
'        Finally
'            If objSQLConn IsNot Nothing Then
'                _objDB.CloseSQLConnection(objSQLConn)
'            End If
'        End Try
'        GetPendingStockRequests = dtDivConfig
'    End Function
'    Public Function CreateNewStockRequest(ByVal VanOrgID As String) As Boolean
'        Dim objSQLConn As SqlConnection
'        Dim objSQLCmd As SqlCommand
'        Dim bRetVal As Boolean = False

'        Dim bStockRequestSaved As Boolean = False
'        Try

'            objSQLConn = _objDB.GetSQLConnection
'            Dim objSQLtrans As SqlTransaction
'            objSQLtrans = objSQLConn.BeginTransaction()
'            Try


'                objSQLCmd = New SqlCommand("app_ManageStockRequest", objSQLConn)
'                objSQLCmd.CommandType = CommandType.StoredProcedure
'                objSQLCmd.Transaction = objSQLtrans
'                objSQLCmd.Parameters.Add(New SqlParameter("@VanOrgID", SqlDbType.VarChar, 100))
'                objSQLCmd.Parameters("@VanOrgID").Value = VanOrgID

'                objSQLCmd.Parameters.Add(New SqlParameter("@LoggedAt", SqlDbType.DateTime))
'                objSQLCmd.Parameters("@LoggedAt").Value = System.DateTime.Now()


'                objSQLCmd.Parameters.Add(New SqlParameter("@LoggedBy", SqlDbType.Int))
'                objSQLCmd.Parameters("@LoggedBy").Value = CType(Session("User_Access"), UserAccess).UserID

'                objSQLCmd.Parameters.Add(New SqlParameter("@LastUpdatedAt", SqlDbType.DateTime))
'                objSQLCmd.Parameters("@LastUpdatedAt").Value = System.DateTime.Now()


'                objSQLCmd.Parameters.Add(New SqlParameter("@LastUpdatedBy", SqlDbType.VarChar))
'                objSQLCmd.Parameters("@LastUpdatedBy").Value = CType(Session("User_Access"), UserAccess).UserID


'                objSQLCmd.ExecuteNonQuery()


'                objSQLtrans.Commit()

'                objSQLCmd.Dispose()
'                bStockRequestSaved = True
'                objSQLCmd = Nothing
'            Catch ex As Exception
'                objSQLtrans.Rollback()
'            End Try
'        Catch ex As Exception

'            Err_No = "7400920"
'            Err_Desc = ex.Message
'            Throw ex
'        Finally
'            If objSQLConn IsNot Nothing Then
'                _objDB.CloseSQLConnection(objSQLConn)
'            End If
'        End Try
'        If bStockRequestSaved = True Then
'            bRetVal = True
'        End If
'        Return bRetVal





'    End Function
'    Public Function StockRequestExists(ByVal VanOrgID As String) As Boolean
'        Dim objSQLConn As SqlConnection
'        Dim objSQLCmd As SqlCommand
'        Dim sQry As String
'        Dim iRowsAffected As Integer = 0
'        Dim retVal As Boolean = False
'        Try
'            objSQLConn = _objDB.GetSQLConnection
'            sQry = String.Format("Select count(Van_Org_ID) from  TBL_Opening_Stock_Requests where Van_Org_ID='{0}' and Status='N'", VanOrgID)

'            objSQLCmd = New SqlCommand(sQry, objSQLConn)
'            iRowsAffected = CInt(objSQLCmd.ExecuteScalar())
'            If iRowsAffected > 0 Then
'                Return True
'            Else
'                Return False
'            End If
'        Catch ex As Exception
'            Throw ex
'        Finally
'            If objSQLConn IsNot Nothing Then
'                _objDB.CloseSQLConnection(objSQLConn)
'            End If
'        End Try



'    End Function
'    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
'        RadWindowManager1.RadAlert(str, 350, 100, Title, "alertCallBackFn")
'        Exit Sub
'    End Sub

'    Protected Sub Unnamed1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
'        RefreshReport()


'    End Sub

'    Protected Sub SearchBtn_Click(sender As Object, e As EventArgs) Handles SearchBtn.Click
'        BindReport()
'    End Sub
'    Private Sub RefreshReport()
'        Try
'            Dim SearchQuery As String = ""
'            If Not (ddl_org1.SelectedItem.Value = "0") Then
'                SearchQuery = BuildQuery()
'            Else
'                'MessageBoxValidation("Select an organization.", "Validation")
'                Exit Sub
'            End If



'            'lbl_org.Text = ddlOrganization.SelectedItem.Text
'            'lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
'            'lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")

'            Dim collection As IList(Of RadComboBoxItem) = ddlVan1.CheckedItems

'            Dim van As String = ""
'            Dim vantxt As String = ""
'            For Each li As RadComboBoxItem In collection
'                van = van & li.Value & ","
'                vantxt = vantxt & li.Text & ","
'            Next

'            If vantxt.Trim() <> "" Then
'                vantxt = vantxt.Substring(0, vantxt.Length - 1)
'            End If
'            If van = "" Then
'                van = "0"
'            End If
'            'If van = "0" Then
'            '    lbl_van.Text = "All"
'            'Else
'            '    lbl_van.Text = vantxt
'            'End If


'            'Args.Visible = True

'            Dim objUserAccess As UserAccess
'            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
'            Dim ObjCommon As New SalesWorx.BO.Common.Common

'            Dim dt As New DataTable
'            dt = ObjCommon.GetStockRequestListFilter(Err_No, Err_Desc, SearchQuery, ddl_org1.SelectedItem.Value)
'            rgStockRequests.DataSource = dt
'            rgStockRequests.DataBind()






'        Catch Ex As Exception
'            log.Error(Ex.Message)
'        End Try
'    End Sub
'    Private Sub BindReport()
'        Try
'            Dim SearchQuery As String = ""
'            If Not (ddl_org1.SelectedItem.Value = "0") Then
'                SearchQuery = BuildQuery()
'            Else
'                MessageBoxValidation("Select an organization.", "Validation")
'                Exit Sub
'            End If



'            'lbl_org.Text = ddlOrganization.SelectedItem.Text
'            'lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
'            'lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")

'            Dim collection As IList(Of RadComboBoxItem) = ddlVan1.CheckedItems

'            Dim van As String = ""
'            Dim vantxt As String = ""
'            For Each li As RadComboBoxItem In collection
'                van = van & li.Value & ","
'                vantxt = vantxt & li.Text & ","
'            Next

'            If vantxt.Trim() <> "" Then
'                vantxt = vantxt.Substring(0, vantxt.Length - 1)
'            End If
'            If van = "" Then
'                van = "0"
'            End If
'            'If van = "0" Then
'            '    lbl_van.Text = "All"
'            'Else
'            '    lbl_van.Text = vantxt
'            'End If


'            'Args.Visible = True

'            Dim objUserAccess As UserAccess
'            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
'            Dim ObjCommon As New SalesWorx.BO.Common.Common

'            Dim dt As New DataTable
'            dt = ObjCommon.GetStockRequestListFilter(Err_No, Err_Desc, SearchQuery, ddl_org1.SelectedItem.Value)
'            rgStockRequests.DataSource = dt
'            rgStockRequests.DataBind()






'        Catch Ex As Exception
'            log.Error(Ex.Message)
'        End Try
'    End Sub

'    Private Function BuildQuery() As String
'        Dim SearchQuery As String = ""
'        Try

'            ObjCommon = New SalesWorx.BO.Common.Common()
'            Dim collection As IList(Of RadComboBoxItem) = ddlVan1.CheckedItems

'            Dim van As String = ""
'            For Each li As RadComboBoxItem In collection
'                van = van & li.Value & ","
'            Next

'            If van = "" Then
'                van = "0"
'            End If



'            If van <> "0" Then
'                SearchQuery = SearchQuery & "AND B.Van_Org_ID in(Select item from SplitQuotedString('" & van & "'))"
'            Else

'            End If



'            If txtFromDate.DateInput.Text <> "" Then
'                SearchQuery = SearchQuery & " And B.Logged_At >= '" & CDate(txtFromDate.SelectedDate) & "'"

'            End If
'            If txtToDate.DateInput.Text <> "" Then
'                SearchQuery = SearchQuery & " And B.Logged_At <= '" & CDate(txtToDate.SelectedDate) & " 23:59:59'"
'            End If



'            Dim St As String = ""
'            For Each li As RadComboBoxItem In ddl_Status.CheckedItems
'                St = St & li.Value & ","
'            Next

'            If St.Trim <> "" Then
'                St = St.Substring(0, St.Length - 1)
'            End If

'            If St.Trim <> "" Then
'                SearchQuery = SearchQuery & " And B.Status in(select item from SplitQuotedString('" & St & "'))"
'            End If


'            Return SearchQuery
'        Catch ex As Exception
'            If Err_Desc IsNot Nothing Then
'                log.Error(Err_Desc)
'            Else
'                log.Error(GetExceptionInfo(ex))
'            End If
'            Err_No = "74067"
'            '  Err_Desc = ex.Message
'            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
'        Finally

'        End Try
'    End Function

'    Private Sub dgv_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles rgStockRequests.SortCommand
'        ViewState("SortField") = e.SortExpression
'        SortDirection = "flip"
'        Dim orgid As String
'        If RequestStocktab.Tabs(0).Selected = True Then

'            orgid = ddl_org.SelectedItem.Value
'        End If
'        If RequestStocktab.Tabs(1).Selected = True Then
'            orgid = ddl_org1.SelectedItem.Value

'        End If
'        Dim dt As New DataTable
'        Dim SearchQuery As String = ""
'        SearchQuery = BuildQuery()
'        ObjCommon = New SalesWorx.BO.Common.Common()
'        dt = ObjCommon.GetStockRequestListFilter(Err_No, Err_Desc, SearchQuery, orgid)
'        rgStockRequests.DataSource = dt
'        rgStockRequests.DataBind()
'    End Sub
'    Private Sub dgv_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles rgStockRequests.PageIndexChanged

'        Dim dt As New DataTable

'        Dim orgid As String
'        If RequestStocktab.Tabs(0).Selected = True Then

'            orgid = ddl_org.SelectedItem.Value
'        End If
'        If RequestStocktab.Tabs(1).Selected = True Then
'            orgid = ddl_org1.SelectedItem.Value

'        End If

'        Dim SearchQuery As String = ""
'        SearchQuery = BuildQuery()
'        ObjCommon = New SalesWorx.BO.Common.Common()
'        dt = ObjCommon.GetStockRequestListFilter(Err_No, Err_Desc, SearchQuery, orgid)
'        rgStockRequests.DataSource = dt
'        rgStockRequests.DataBind()
'    End Sub
'    Private Property SortDirection() As String
'        Get
'            If ViewState("SortDirection") Is Nothing Then
'                ViewState("SortDirection") = "ASC"
'            End If
'            Return ViewState("SortDirection").ToString()
'        End Get
'        Set(ByVal value As String)
'            Dim s As String = SortDirection

'            If value = "flip" Then
'                s = If(s = "ASC", "DESC", "ASC")
'            Else
'                s = value
'            End If

'            ViewState("SortDirection") = s
'        End Set
'    End Property

'    Protected Sub ddl_org1_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddl_org1.SelectedIndexChanged
'        Try


'            Dim objUserAccess As UserAccess
'            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
'            Dim objCommon As New SalesWorx.BO.Common.Common
'            ddlVan1.DataSource = objCommon.GetVanOrgIdByOrg(Err_No, Err_Desc, ddl_org1.SelectedValue, objUserAccess.UserID)
'            ddlVan1.DataValueField = "Org_ID"
'            ddlVan1.DataTextField = "SalesRep_Name"
'            ddlVan1.DataBind()
'            'ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))
'            For Each itm As RadComboBoxItem In ddlVan1.Items
'                itm.Checked = True
'            Next
'            objCommon = Nothing
'        Catch ex As Exception
'            log.Error(GetExceptionInfo(ex))

'        End Try
'    End Sub
'    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click

'        If Not Session("StNFrom") Is Nothing Then
'            txtFromDate.SelectedDate = Session("StNFrom")
'        Else
'            txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)

'        End If
'        If Not Session("StNTo") Is Nothing Then
'            txtToDate.SelectedDate = Session("StNTo")
'        Else
'            txtToDate.SelectedDate = Now()
'        End If
'        'ddl_org1.ClearSelection()
'        'If ddl_org1.Items.Count = 2 Then
'        '    ddl_org1.SelectedIndex = 1
'        'End If
'        'Args.Visible = False
'        ddlVan1.ClearCheckedItems()
'        '  ddlVan1.Items.Clear()
'        '   ddlVan1.Text = "Please type product code/ name"

'        ddl_Status.ClearCheckedItems()
'        ddl_org1.ClearSelection()
'        Dim dt1 As New DataTable
'        rgStockRequests.DataSource = dt1
'        rgStockRequests.DataBind()
'        'BindReport()


'        'If Not (ddl_org1.SelectedItem.Value = "0") Then
'        '    LoadOrgDetails()
'        'Else
'        '    rgStockRequests.Visible = False
'        'End If
'    End Sub
'End Class
Imports System.Data.OleDb
Imports System.IO
Imports log4net
Imports System.Data.SqlClient
Imports SalesWorx.BO.Common
Imports ExcelLibrary.SpreadSheet
Imports Telerik.Web.UI
Imports System.Data.SqlTypes
Imports System.Data
Public Class RequestStock
    Inherits System.Web.UI.Page
    Private _objDB As SalesWorx.BO.DAL.DatabaseConnection
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Public Sub New()
        _objDB = New SalesWorx.BO.DAL.DatabaseConnection
    End Sub
    Dim Err_No As Long
    Dim Err_Desc As String

    Private Const ModuleName As String = "RequestStock.aspx"
    Private Const PageID As String = "P3630"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try


            If Not Page.IsPostBack() Then
                If Session.Item("USER_ACCESS") Is Nothing Then
                    Session.Add("BringmeBackHere", ModuleName)
                    Response.Redirect("Login.aspx", False)
                    Exit Sub
                End If
                'If Not HasAuthentication() Then
                '    Err_No = 500
                '    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
                'End If
                FilldropdownsAndList()
                If Not Session("StNFrom") Is Nothing Then
                    txtFromDate.SelectedDate = Session("StNFrom")
                Else
                    txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)

                End If
                If Not Session("StNTo") Is Nothing Then
                    txtToDate.SelectedDate = Session("StNTo")
                Else
                    txtToDate.SelectedDate = Now()
                End If
                ViewState("Criteria") = "1=1"
                Session.Remove("Stock_Unconfirm")
            End If

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
    Private Function HasAuthentication() As Boolean
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
    End Function
    Private Sub RequestStocktab_TabClick(sender As Object, e As RadTabStripEventArgs) Handles RequestStocktab.TabClick
        'If Args.Visible = True Then
        Dim objCommon As New SalesWorx.BO.Common.Common
        If RequestStocktab.Tabs(0).Selected = True Then


            If Not Session("StNFrom") Is Nothing Then
                txtFromDate.SelectedDate = Session("StNFrom")
            Else
                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)

            End If
            If Not Session("StNTo") Is Nothing Then
                txtToDate.SelectedDate = Session("StNTo")
            Else
                txtToDate.SelectedDate = Now()
            End If
            'ddl_org1.ClearSelection()
            'If ddl_org1.Items.Count = 2 Then
            '    ddl_org1.SelectedIndex = 1
            'End If
            'Args.Visible = False
            ddlVan1.ClearCheckedItems()
            ' ddlVan1.Items.Clear()
            '   ddlVan1.Text = "Please type product code/ name"

            ddl_Status.ClearSelection()
            ' BindReport()
            Dim dt As New DataTable
            Dim SearchQuery As String = ""

            dt = ObjCommon.GetStockRequestListFilter(Err_No, Err_Desc, SearchQuery, ddl_org.SelectedItem.Value)
            rgStockRequests.DataSource = dt
            rgStockRequests.DataBind()
        End If
        If RequestStocktab.Tabs(1).Selected = True Then
            ddl_org1.ClearSelection()
            Dim dt1 As New DataTable
            rgStockRequests.DataSource = dt1
            rgStockRequests.DataBind()
            'ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
        End If

        'End If
    End Sub
    Sub FilldropdownsAndList()
        Try


            Dim objCommon As New SalesWorx.BO.Common.Common

            Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
            ddl_org.DataSource = (New SalesWorx.BO.Common.Product).GetOrganisation(Err_No, Err_Desc, SubQry)
            ddl_org.Items.Clear()
            ddl_org.Items.Insert(0, New RadComboBoxItem("Select Organization"))
            ddl_org.AppendDataBoundItems = True
            ddl_org.DataValueField = "MAS_Org_ID"
            ddl_org.DataTextField = "Description"
            ddl_org.DataBind()
            ddl_org.Items(0).Value = 0

            ddl_org1.DataSource = (New SalesWorx.BO.Common.Product).GetOrganisation(Err_No, Err_Desc, SubQry)
            ddl_org1.Items.Clear()
            ddl_org1.Items.Insert(0, New RadComboBoxItem("Select Organization"))
            ddl_org1.AppendDataBoundItems = True
            ddl_org1.DataValueField = "MAS_Org_ID"
            ddl_org1.DataTextField = "Description"
            ddl_org1.DataBind()
            ddl_org1.Items(0).Value = 0

            'ddl_Status.DataSource = objCommon.GetStockRequestStatus(Err_No, Err_Desc, "'R'")
            'ddl_Status.DataTextField = "Status"
            'ddl_Status.DataValueField = "StatusKey"
            'ddl_Status.DataBind()
            ddl_Status.Items.Insert(0, New RadComboBoxItem("Pending", "N"))
            ddl_Status.Items.Insert(1, New RadComboBoxItem("Processed", "Y"))

            Dim dt As New DataTable
            Dim SearchQuery As String = ""

            dt = ObjCommon.GetStockRequestListFilter(Err_No, Err_Desc, SearchQuery, ddl_org.SelectedItem.Value)
            rgStockRequests.DataSource = dt
            rgStockRequests.DataBind()

            'Dim objUserAccess As UserAccess
            'objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

            'ddlVan.DataSource = objCommon.GetVanByOrg(Err_No, Err_Desc, ddl_org.SelectedValue, objUserAccess.UserID)
            'ddlVan.DataValueField = "SalesRep_ID"
            'ddlVan.DataTextField = "SalesRep_Name"
            'ddlVan.DataBind()
            'ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))

            'objCommon = Nothing
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
    Private Sub ddl_org_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_org.SelectedIndexChanged

        Try


            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim objCommon As New SalesWorx.BO.Common.Common
            ddlVan.DataSource = objCommon.GetVanOrgIdByOrg(Err_No, Err_Desc, ddl_org.SelectedValue, objUserAccess.UserID)
            ddlVan.DataValueField = "Org_ID"
            ddlVan.DataTextField = "SalesRep_Name"
            ddlVan.DataBind()
            'ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))
            For Each itm As RadComboBoxItem In ddlVan.Items
                itm.Checked = True
            Next
            objCommon = Nothing
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))

        End Try
    End Sub

    Protected Sub BTn_Request_Click(sender As Object, e As EventArgs) Handles BTn_Request.Click
        Dim listPendingRequests As New List(Of String)
        Dim listNewlyCreatedRequests As New List(Of String)
        If ddl_org.SelectedItem.Value = "0" Then
            MessageBoxValidation("Please select organization.", "Validation")
            Return
        End If
        For Each itm As RadComboBoxItem In ddlVan.Items
            If itm.Checked = True Then

                Dim VanOrgID As String
                VanOrgID = itm.Value()
                If Not StockRequestExists(VanOrgID) Then
                    If CreateNewStockRequest(VanOrgID) = True Then
                        VanOrgID = "'" + VanOrgID + "'"
                        listNewlyCreatedRequests.Add(VanOrgID)

                        ' MessageBoxValidation("StockRequests added successfully.", "Information")
                    End If
                Else
                    VanOrgID = "'" + VanOrgID + "'"
                    listPendingRequests.Add(VanOrgID)

                End If

            End If

        Next
        Dim strNewlyCreatedRequests As String = ""
        Dim strPendingRequests As String = ""
        If listPendingRequests.Count > 0 Then


            strPendingRequests = String.Join(",", listPendingRequests)
            'Dim dt As New DataTable
            'dt = GetPendingStockRequests(strPendingRequests)
            'If dt.Rows.Count > 0 Then
            '    gv_InvalidStock.DataSource = dt
            '    gv_InvalidStock.DataBind()
            '    MpInfoError.Show()
            'End If
        End If

        If listNewlyCreatedRequests.Count > 0 Then


            strNewlyCreatedRequests = String.Join(",", listNewlyCreatedRequests)
            'Dim dt As New DataTable
            'dt = GetPendingStockRequests(strPendingRequests)
            'If dt.Rows.Count > 0 Then
            '    gv_InvalidStock.DataSource = dt
            '    gv_InvalidStock.DataBind()
            '    MpInfoError.Show()
            'End If
        End If

        Dim dt As New DataTable
        dt = GetPendingStockRequests(strPendingRequests, strNewlyCreatedRequests)
        If dt.Rows.Count > 0 Then
            gv_InvalidStock.DataSource = dt
            gv_InvalidStock.DataBind()
            MpInfoError.Show()
        End If
        'Response.Redirect("~/html/RequestStock.html")
        Dim dt1 As New DataTable
        Dim SearchQuery As String = ""
        Dim objCommon As New SalesWorx.BO.Common.Common
        dt1 = ObjCommon.GetStockRequestListFilter(Err_No, Err_Desc, SearchQuery, ddl_org.SelectedItem.Value)
        rgStockRequests.DataSource = dt1
        rgStockRequests.DataBind()
        ddlVan.ClearCheckedItems()
        'ddlVan.Items.Clear()



    End Sub
    Public Function GetPendingStockRequests(ByVal strPendingRequests As String, ByVal strNewlyCreatedRequests As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim objSQLDA1 As SqlDataAdapter
        Dim dtDivConfig As New DataTable

        Try

            objSQLConn = _objDB.GetSQLConnection
            If strPendingRequests <> "" Then
                objSQLDA = New SqlDataAdapter("SELECT B.Logged_At as 'Date Request Created' ,'Can not add a new request as there already exists a pending request' as Message,C.SalesRep_Name as Van,case  when B.Status='N' then 'Pending' when B.Status='Y' then 'Processed'end  Status FROM TBL_Org_CTL_DTL AS A inner join TBL_Opening_Stock_Requests as B On A.Org_ID =B.Van_Org_ID inner join tbl_fsr as C On A.SalesRep_ID = C.SalesRep_ID where B.Status= 'N' and B.Van_Org_ID in (" + strPendingRequests + ") ", objSQLConn)
                objSQLDA.SelectCommand.CommandType = CommandType.Text
                objSQLDA.Fill(dtDivConfig)
                objSQLDA.Dispose()
            End If
            If strNewlyCreatedRequests <> "" Then
                objSQLDA1 = New SqlDataAdapter("SELECT B.Logged_At as 'Date Request Created' ,'Request added' as Message,C.SalesRep_Name as Van,case  when B.Status='N' then 'Pending' when B.Status='Y' then 'Processed'end  Status FROM TBL_Org_CTL_DTL AS A inner join TBL_Opening_Stock_Requests as B On A.Org_ID =B.Van_Org_ID inner join tbl_fsr as C On A.SalesRep_ID = C.SalesRep_ID where B.Status= 'N' and B.Van_Org_ID in (" + strNewlyCreatedRequests + ") ", objSQLConn)
                objSQLDA1.SelectCommand.CommandType = CommandType.Text
                objSQLDA1.Fill(dtDivConfig)
                objSQLDA1.Dispose()
            End If
        Catch ex As Exception
            Err_No = "74204"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetPendingStockRequests = dtDivConfig
    End Function
    Public Function CreateNewStockRequest(ByVal VanOrgID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim bRetVal As Boolean = False

        Dim bStockRequestSaved As Boolean = False
        Try

            objSQLConn = _objDB.GetSQLConnection
            Dim objSQLtrans As SqlTransaction
            objSQLtrans = objSQLConn.BeginTransaction()
            Try


                objSQLCmd = New SqlCommand("app_ManageStockRequest", objSQLConn)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.Transaction = objSQLtrans
                objSQLCmd.Parameters.Add(New SqlParameter("@VanOrgID", SqlDbType.VarChar, 100))
                objSQLCmd.Parameters("@VanOrgID").Value = VanOrgID

                objSQLCmd.Parameters.Add(New SqlParameter("@LoggedAt", SqlDbType.DateTime))
                objSQLCmd.Parameters("@LoggedAt").Value = System.DateTime.Now()


                objSQLCmd.Parameters.Add(New SqlParameter("@LoggedBy", SqlDbType.Int))
                objSQLCmd.Parameters("@LoggedBy").Value = CType(Session("User_Access"), UserAccess).UserID

                objSQLCmd.Parameters.Add(New SqlParameter("@LastUpdatedAt", SqlDbType.DateTime))
                objSQLCmd.Parameters("@LastUpdatedAt").Value = System.DateTime.Now()


                objSQLCmd.Parameters.Add(New SqlParameter("@LastUpdatedBy", SqlDbType.VarChar))
                objSQLCmd.Parameters("@LastUpdatedBy").Value = CType(Session("User_Access"), UserAccess).UserID


                objSQLCmd.ExecuteNonQuery()


                objSQLtrans.Commit()

                objSQLCmd.Dispose()
                bStockRequestSaved = True
                objSQLCmd = Nothing
            Catch ex As Exception
                objSQLtrans.Rollback()
            End Try
        Catch ex As Exception

            Err_No = "7400920"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        If bStockRequestSaved = True Then
            bRetVal = True
        End If
        Return bRetVal





    End Function
    Public Function StockRequestExists(ByVal VanOrgID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim iRowsAffected As Integer = 0
        Dim retVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            sQry = String.Format("Select count(Van_Org_ID) from  TBL_Opening_Stock_Requests where Van_Org_ID='{0}' and Status='N'", VanOrgID)

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            iRowsAffected = CInt(objSQLCmd.ExecuteScalar())
            If iRowsAffected > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try



    End Function
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 350, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Protected Sub Unnamed1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        RefreshReport()


    End Sub

    Protected Sub SearchBtn_Click(sender As Object, e As EventArgs) Handles SearchBtn.Click
        BindReport()
    End Sub
    Private Sub RefreshReport()
        Try
            Dim SearchQuery As String = ""
            If Not (ddl_org1.SelectedItem.Value = "0") Then
                SearchQuery = BuildQuery()
            Else
                'MessageBoxValidation("Select an organization.", "Validation")
                Exit Sub
            End If



            'lbl_org.Text = ddlOrganization.SelectedItem.Text
            'lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            'lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")

            Dim collection As IList(Of RadComboBoxItem) = ddlVan1.CheckedItems

            Dim van As String = ""
            Dim vantxt As String = ""
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","
                vantxt = vantxt & li.Text & ","
            Next

            If vantxt.Trim() <> "" Then
                vantxt = vantxt.Substring(0, vantxt.Length - 1)
            End If
            If van = "" Then
                van = "0"
            End If
            'If van = "0" Then
            '    lbl_van.Text = "All"
            'Else
            '    lbl_van.Text = vantxt
            'End If


            'Args.Visible = True

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjCommon As New SalesWorx.BO.Common.Common

            Dim dt As New DataTable
            dt = ObjCommon.GetStockRequestListFilter(Err_No, Err_Desc, SearchQuery, ddl_org1.SelectedItem.Value)
            rgStockRequests.DataSource = dt
            rgStockRequests.DataBind()






        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub
    Private Sub BindReport()
        Try
            Dim SearchQuery As String = ""
            If Not (ddl_org1.SelectedItem.Value = "0") Then
                SearchQuery = BuildQuery()
            Else
                MessageBoxValidation("Select an organization.", "Validation")
                Exit Sub
            End If



            'lbl_org.Text = ddlOrganization.SelectedItem.Text
            'lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            'lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")

            Dim collection As IList(Of RadComboBoxItem) = ddlVan1.CheckedItems

            Dim van As String = ""
            Dim vantxt As String = ""
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","
                vantxt = vantxt & li.Text & ","
            Next

            If vantxt.Trim() <> "" Then
                vantxt = vantxt.Substring(0, vantxt.Length - 1)
            End If
            If van = "" Then
                van = "0"
            End If
            'If van = "0" Then
            '    lbl_van.Text = "All"
            'Else
            '    lbl_van.Text = vantxt
            'End If


            'Args.Visible = True

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjCommon As New SalesWorx.BO.Common.Common

            Dim dt As New DataTable
            dt = ObjCommon.GetStockRequestListFilter(Err_No, Err_Desc, SearchQuery, ddl_org1.SelectedItem.Value)
            rgStockRequests.DataSource = dt
            rgStockRequests.DataBind()






        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub

    Private Function BuildQuery() As String
        Dim SearchQuery As String = ""
        Try

            ObjCommon = New SalesWorx.BO.Common.Common()
            Dim collection As IList(Of RadComboBoxItem) = ddlVan1.CheckedItems

            Dim van As String = ""
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","
            Next

            If van = "" Then
                van = "0"
            End If



            If van <> "0" Then
                SearchQuery = SearchQuery & "AND B.Van_Org_ID in(Select item from SplitQuotedString('" & van & "'))"
            Else

            End If



            If txtFromDate.DateInput.Text <> "" Then
                SearchQuery = SearchQuery & " And B.Logged_At >= '" & CDate(txtFromDate.SelectedDate) & "'"

            End If
            If txtToDate.DateInput.Text <> "" Then
                SearchQuery = SearchQuery & " And B.Logged_At <= '" & CDate(txtToDate.SelectedDate) & " 23:59:59'"
            End If



            Dim St As String = ""
            For Each li As RadComboBoxItem In ddl_Status.CheckedItems
                St = St & li.Value & ","
            Next

            If St.Trim <> "" Then
                St = St.Substring(0, St.Length - 1)
            End If

            If St.Trim <> "" Then
                SearchQuery = SearchQuery & " And B.Status in(select item from SplitQuotedString('" & St & "'))"
            End If


            Return SearchQuery
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

        End Try
    End Function

    Private Sub dgv_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles rgStockRequests.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        Dim orgid As String
        Dim SearchQuery As String = ""
        If RequestStocktab.Tabs(0).Selected = True Then

            orgid = ddl_org.SelectedItem.Value
        End If
        If RequestStocktab.Tabs(1).Selected = True Then
            orgid = ddl_org1.SelectedItem.Value
            SearchQuery = BuildQuery()

        End If
        Dim dt As New DataTable


        ObjCommon = New SalesWorx.BO.Common.Common()
        dt = ObjCommon.GetStockRequestListFilter(Err_No, Err_Desc, SearchQuery, orgid)
        rgStockRequests.DataSource = dt
        rgStockRequests.DataBind()
    End Sub
    Private Sub dgv_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles rgStockRequests.PageIndexChanged

        Dim dt As New DataTable

        Dim orgid As String
        If RequestStocktab.Tabs(0).Selected = True Then

            orgid = ddl_org.SelectedItem.Value
        End If
        If RequestStocktab.Tabs(1).Selected = True Then
            orgid = ddl_org1.SelectedItem.Value

        End If

        Dim SearchQuery As String = ""
        SearchQuery = BuildQuery()
        ObjCommon = New SalesWorx.BO.Common.Common()
        dt = ObjCommon.GetStockRequestListFilter(Err_No, Err_Desc, SearchQuery, orgid)
        rgStockRequests.DataSource = dt
        rgStockRequests.DataBind()
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

    Protected Sub ddl_org1_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddl_org1.SelectedIndexChanged
        Try


            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim objCommon As New SalesWorx.BO.Common.Common
            ddlVan1.DataSource = objCommon.GetVanOrgIdByOrg(Err_No, Err_Desc, ddl_org1.SelectedValue, objUserAccess.UserID)
            ddlVan1.DataValueField = "Org_ID"
            ddlVan1.DataTextField = "SalesRep_Name"
            ddlVan1.DataBind()
            'ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))
            For Each itm As RadComboBoxItem In ddlVan1.Items
                itm.Checked = True
            Next
            objCommon = Nothing
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))

        End Try
    End Sub
    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click

        If Not Session("StNFrom") Is Nothing Then
            txtFromDate.SelectedDate = Session("StNFrom")
        Else
            txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)

        End If
        If Not Session("StNTo") Is Nothing Then
            txtToDate.SelectedDate = Session("StNTo")
        Else
            txtToDate.SelectedDate = Now()
        End If
        'ddl_org1.ClearSelection()
        'If ddl_org1.Items.Count = 2 Then
        '    ddl_org1.SelectedIndex = 1
        'End If
        'Args.Visible = False
        ddlVan1.ClearCheckedItems()
        '  ddlVan1.Items.Clear()
        '   ddlVan1.Text = "Please type product code/ name"

        ddl_Status.ClearCheckedItems()
        ddl_org1.ClearSelection()
        Dim dt1 As New DataTable
        rgStockRequests.DataSource = dt1
        rgStockRequests.DataBind()
        'BindReport()


        'If Not (ddl_org1.SelectedItem.Value = "0") Then
        '    LoadOrgDetails()
        'Else
        '    rgStockRequests.Visible = False
        'End If
    End Sub
End Class