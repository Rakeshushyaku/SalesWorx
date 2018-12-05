Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports Telerik.Web.UI
Imports log4net
Public Class ListReturnNotesForApproval
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCollection As Collection
    'Dim SortField As String = ""
    'Dim SortFieldDtl As String = ""
    Private Const PageID As String = "P417"
    Private RowIdx As Integer = 0
    Dim objLogin As New SalesWorx.BO.Common.Login
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
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
                If Not Session("StNOrg") Is Nothing Then
                    If Not ddlOrganization.FindItemByValue(Session("StNOrg").ToString) Is Nothing Then
                        ddlOrganization.ClearSelection()
                        ddlOrganization.FindItemByValue(Session("StNOrg").ToString).Selected = True
                    End If
                Else
                    If ddlOrganization.Items.Count = 2 Then
                        ddlOrganization.SelectedIndex = 1
                    End If
                    
                End If
                If Not Session("StRef") Is Nothing Then
                    txtRefNo.Text = Session("StRef")
                End If
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

                LoadOrgDetails()
                BindData()
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
    Sub LoadOrgDetails()
        If Not (ddlOrganization.SelectedItem.Value = "0") Then

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New SalesWorx.BO.Common.Common()
            ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
            ddlVan.DataBind()
            If Not Session("StVan") Is Nothing Then
                Dim ids() As String
                ids = Session("StVan").ToString.Split(",")
                For Each itm As RadComboBoxItem In ddlVan.Items
                    For Each id As String In ids
                        If itm.Value = id Then
                            itm.Checked = True
                            Exit For
                        End If
                    Next
                Next
            Else
                For Each itm As RadComboBoxItem In ddlVan.Items
                    itm.Checked = True
                Next
            End If
        Else
            ddlVan.Items.Clear()
        End If
         
    End Sub
    Private Sub ddl_Customer_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddlCustomer.ItemsRequested

        Dim Objrep As New SalesWorx.BO.Common.Reports()


        Dim dt As New DataTable

        If dt.Rows.Count > 0 Then
            dt.Rows.Clear()
        End If


        'dt = Objrep.GetCustomerfromOrgtext(Err_No, Err_Desc, ddlOrganization.SelectedValue, e.Text)

        dt = Objrep.GetCustomerfromOrgtext(Err_No, Err_Desc, ddlOrganization.SelectedValue, e.Text)

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
    End Sub

    Private Sub gvRep_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles gvRep.ItemCommand
        If e.CommandName = "ConfirmReturnNote" Then
            Dim row As GridDataItem = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridDataItem)

            Dim HID As String = CType(row.FindControl("HID"), HiddenField).Value

            Session("StNFrom") = CDate(txtFromDate.SelectedDate)
            Session("StNTo") = CDate(txtToDate.SelectedDate)
            Session("StRef") = txtRefNo.Text.Trim

            If ddlOrganization.SelectedIndex >= 0 Then
                Session("StNOrg") = ddlOrganization.SelectedItem.Value
            End If
            Dim van As String = ""
            For Each li As RadComboBoxItem In ddlVan.CheckedItems
                van = van & li.Value & ","

            Next
            If van = "" Then
                van = "0"
            Else
                van = van.Substring(0, van.Length - 1)
            End If
            Session("StVan") = van
            Response.Redirect("ApproveReturnNotes.aspx?Row_ID=" & HID)
        End If
    End Sub

    Private Sub dgv_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvRep.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindData()
    End Sub
    Private Sub dgv_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged

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
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            gvRep.Visible = True
            BindData()
        Else
            gvRep.Visible = False
        End If
    End Sub
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If ddlOrganization.SelectedIndex <= 0 Then
            MessageBoxValidation("Please select the Organisation", "Validation")
            Return bretval
        End If
        If ddlVan.CheckedItems.Count = 0 Then
            MessageBoxValidation("Please select the Van/FSR", "Validation")
            Return bretval
        End If
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

    End Function
    Private Sub BindData()
        Try
            Dim SearchQuery As String = ""

            Dim custID As String = "0"
            Dim SiteID As String = "0"
            If Not String.IsNullOrEmpty(ddlCustomer.SelectedValue) Then
                Dim ids() As String
                ids = ddlCustomer.SelectedValue.Split("$")
                custID = ids(0)
                SiteID = ids(1)
            Else
                custID = 0
                SiteID = 0
            End If
            Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems

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




            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjReturn As New SalesWorx.BO.Common.Returns
            Dim dt As New DataTable
            'dt = ObjReport.GetOrderListing(Err_No, Err_Desc, SearchQuery, ddlOrganization.SelectedItem.Value)
            dt = ObjReturn.GetReturnNotes(Err_No, Err_Desc, custID, SiteID, van, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"), txtRefNo.Text.Trim, objUserAccess.UserID)
            gvRep.DataSource = dt
            gvRep.DataBind()




        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub


    Private Sub ddlOrganization_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlOrganization.SelectedIndexChanged
        LoadOrgDetails()
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        ddlVan.ClearCheckedItems()
        ddlVan.Items.Clear()
        LoadOrgDetails()
        ddlCustomer.ClearSelection()
        ddlCustomer.Text = ""
        txtRefNo.Text = ""
        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Now()
        gvRep.Visible = False

    End Sub
End Class