Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO

Partial Public Class CustomerListing
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Dim ObjCustomer As Customer
    '  Dim SortField As String = ""
    Private Const PageID As String = "P18"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private Property SortField() As String
        Get
            If ViewState("SortColumn") Is Nothing Then
                ViewState("SortColumn") = ""
            End If
            Return ViewState("SortColumn").ToString()
        End Get
        Set(ByVal value As String)
            ViewState("SortColumn") = value
        End Set
    End Property
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
                ObjCommon = New Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New ListItem("-- Select a value --"))
                ddlSegment.DataSource = ObjCommon.GetCustomerSegmentList(Err_No, Err_Desc, SubQry)
                ddlSegment.DataBind()
                ddlSegment.Items.Insert(0, New ListItem("-- Select a value --"))
                ddlSalesDist.DataSource = ObjCommon.GetSalesDistrictList(Err_No, Err_Desc, SubQry)
                ddlSalesDist.DataBind()
                ddlSalesDist.Items.Insert(0, New ListItem("-- Select a value --"))
                'ddlType.DataSource = ObjCommon.GetCustomerTypeList(Err_No, Err_Desc, SubQry)
                'ddlType.DataBind()
                'ddlType.Items.Insert(0, New ListItem("-- Select a value --"))

                ' BindData()
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
    Private Sub BindData()
        Dim SearchQuery As String = ""
        Try
            ObjCustomer = New Customer()
            If (ddlSegment.SelectedItem.Value = "-- Select a value --" And ddlSalesDist.SelectedItem.Value = "-- Select a value --" And ddlCust_Stat.SelectedItem.Text = "All" And txtCustomerNo.Text = "" And txtCustomerName.Text = "") Then
                SearchQuery = ""
            Else
                If ddlSegment.SelectedValue <> "-- Select a value --" Then
                    SearchQuery = " And B.Customer_Segment_ID=" & ddlSegment.SelectedValue
                End If
                If ddlSalesDist.SelectedValue <> "-- Select a value --" Then
                    SearchQuery = SearchQuery & " And B.Sales_District_ID=" & ddlSalesDist.SelectedValue
                End If
                'If ddlType.SelectedValue <> "-- Select a value --" Then
                '    SearchQuery = SearchQuery & " And A.Customer_Type='" & ddlType.SelectedValue & "'"
                'End If
                If ddlCust_Stat.SelectedValue = 1 Then
                    SearchQuery = SearchQuery & " And A.Cust_Status='A'"
                ElseIf ddlCust_Stat.SelectedValue = 2 Then
                    SearchQuery = SearchQuery & " And A.Cust_Status='B'"
                ElseIf ddlCust_Stat.SelectedValue = 3 Then
                    SearchQuery = SearchQuery & " And A.Cash_Cust='Y'"
                End If
                If txtCustomerNo.Text <> "" Then
                    SearchQuery = SearchQuery & " And A.Customer_No like '" & txtCustomerNo.Text & "%'"
                End If
                If txtCustomerName.Text <> "" Then
                    SearchQuery = SearchQuery & " And A.Customer_Name like '" & txtCustomerName.Text & "%'"
                End If
            End If
            If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then
                SearchQuery = SearchQuery & " And A.Site_Use_ID=" & ddlOrganization.SelectedItem.Value
                Dim ds As New DataSet
                ds = ObjCustomer.GetCustomerList(Err_No, Err_Desc, SearchQuery, "")
                Dim dv As New DataView(ds.Tables("CustLstTbl"))
                If SortField <> "" Then
                    dv.Sort = (SortField & " ") + SortDirection
                Else
                    dv.Sort = "Customer_Name ASC"
                    SortDirection = "ASC"
                    SortField = "Customer_Name"
                End If
                GVCustomerList.DataSource = dv
                GVCustomerList.DataBind()
                ViewState("dt") = dv.ToTable()
                AddSortImage()
            Else
                MessageBoxValidation("Select an organization.")
            End If

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
            ObjCustomer = Nothing
        End Try
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
    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        MpInfoError.Show()
        Exit Sub
    End Sub
    Public Sub AddSortImage()
        If SortField = "" Then
            Exit Sub
        End If
        Dim sortImage As New Image()
        sortImage.Style("padding-left") = "8px"
        sortImage.Style("padding-bottom") = "1px"
        If SortDirection = "ASC" Then
            sortImage.ImageUrl = "~/images/arrowUp.gif"
            sortImage.AlternateText = "Ascending Order"
        Else
            sortImage.ImageUrl = "~/images/arrowDown.gif"
            sortImage.AlternateText = "Descending Order"
        End If
        For i As Integer = 0 To GVCustomerList.Columns.Count - 1
            Dim dcf As DataControlField = GVCustomerList.Columns(i)
            If dcf.SortExpression = SortField Then
                If Not IsNothing(GVCustomerList.HeaderRow) Then
                    GVCustomerList.HeaderRow.Cells(i).Controls.Add(sortImage)
                End If
                Exit For
            End If
        Next
    End Sub

    Private Sub GVCustomerList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GVCustomerList.PageIndexChanging
        GVCustomerList.PageIndex = e.NewPageIndex
        BindData()
    End Sub

    Private Sub GVCustomerList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GVCustomerList.Sorting
        SortField = e.SortExpression
        SortDirection = "flip"
        BindData()
    End Sub

    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        BindData()
    End Sub

    Protected Sub GVCustomerList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVCustomerList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim CashCust As String = DataBinder.Eval(e.Row.DataItem, "Cash_Cust").ToString()
            If CashCust = "N" Then
                Dim img As Image = e.Row.Cells(2).FindControl("imgCashCust")
                img.Visible = False
            End If
        End If
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub


    Protected Sub imgPrint_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgPrint.Click

        GVCustomerList.AllowPaging = False
        Dim dt As New DataTable
        dt = ViewState("dt")
        GVCustomerList.DataSource = dt
        GVCustomerList.DataBind()
        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)
        GVCustomerList.RenderControl(hw)
        Dim gridHTML As String = sw.ToString().Replace("""", "'") _
           .Replace(System.Environment.NewLine, "")
        Dim sb As New StringBuilder()
        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload = new function(){")
        sb.Append("var printWin = window.open('', '', 'left=0")
        sb.Append(",top=0,width=1000,height=1000,status=0');")
        sb.Append("printWin.document.write(""")
        sb.Append(gridHTML)
        sb.Append(""");")
        sb.Append("printWin.document.close();")
        sb.Append("printWin.focus();")
        sb.Append("printWin.print();")
        sb.Append("printWin.close();};")
        sb.Append("</script>")
        ClientScript.RegisterStartupScript(Me.[GetType](), "GridPrint", sb.ToString())
        GVCustomerList.AllowPaging = True
        GVCustomerList.DataBind()

    End Sub

   
End Class