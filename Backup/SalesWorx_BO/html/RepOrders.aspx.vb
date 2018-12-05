Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms

Partial Public Class RepOrders
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Dim ObjCustomer As Customer
    Private ReportPath As String = "RepOrders"
    Dim dv As New DataView
    Private Const PageID As String = "P204"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not IsNothing(Me.Master) Then

            Dim masterScriptManager As ScriptManager
            masterScriptManager = CType(Master.FindControl("ScriptManager1"), ScriptManager)

            ' Make sure our master page has the script manager we're looking for
            If Not IsNothing(masterScriptManager) Then

                ' Turn off partial page postbacks for this page
                masterScriptManager.EnablePartialRendering = False
            End If

        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        If Not IsPostBack Then
            Session("OrdersDT") = Nothing
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
                txtFromDate.Text = Format(Now().Date, "dd-MMM-yyyy")
                txtToDate.Text = Format(Now().Date, "dd-MMM-yyyy")
                'ddlVan.DataSource = ObjCommon.GetAllVan(Err_No, Err_Desc, SubQry)
                'ddlVan.DataBind()
                'ddlVan.Items.Insert(0, New ListItem("-- Select a Van --"))
                'ddlCustomer.DataSource = ObjCommon.GetCustomer(Err_No, Err_Desc, SubQry)
                'ddlCustomer.DataBind()
                'ddlCustomer.Items.Insert(0, New ListItem("-- Select a Customer --"))
                'Dim VisitID As String = ""
                'If Not IsNothing(Request.QueryString("visitid")) Then
                '    VisitID = Request.QueryString("visitid").ToString()
                '    If Not VisitID = "" Then
                '        hdnVisitID.Value = VisitID
                '        BindforVisit()
                '        RemoveCookie("OID")
                '        RemoveCookie("SID")
                '        RemoveCookie("FromDate")
                '        RemoveCookie("ToDate")
                '        RemoveCookie("Customer")
                '        RemoveCookie("OType")
                '    Else
                '        hdnVisitID.Value = ""
                '    End If
                'End If
                'If Not IsNothing(Request.QueryString("cust")) Then
                '    lblCustomer.Text = Request.QueryString("cust").ToString()
                'End If
                'If hdnVisitID.Value = "" Then
                '    If Not IsNothing(Request.QueryString("OID")) Then
                '        Dim OID As String = Request.QueryString("OID")
                '        Dim SID As String = "-- Select a value --"
                '        If Not IsNothing(Request.QueryString("SID")) Then
                '            SID = Request.QueryString("SID").ToString()
                '        End If
                '        Dim FromDate As String = ""
                '        If Not IsNothing(Request.QueryString("FD")) Then
                '            FromDate = Request.QueryString("FD").ToString()
                '        End If
                '        Dim ToDate As String = ""
                '        If Not IsNothing(Request.QueryString("TD")) Then
                '            ToDate = Request.QueryString("TD").ToString()
                '        End If
                '        Dim CID As String = "-- Select a value --"
                '        If Not IsNothing(Request.QueryString("Ct")) Then
                '            CID = Request.QueryString("Ct").ToString()
                '        End If
                '        Dim OType As String = "-- Select a value --"
                '        If Not IsNothing(Request.QueryString("OType")) Then
                '            OType = Request.QueryString("OType").ToString()
                '        End If


                '        ddlOrganization.SelectedValue = OID
                '        Dim objUserAccess As UserAccess
                '        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
                '        ObjCommon = New Common()
                '        ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())
                '        ddlVan.DataBind()
                '        ddlVan.Items.Insert(0, New ListItem("-- Select a value --"))

                '        ddlCustomer.DataSource = ObjCommon.GetCustomerByCriteria(Err_No, Err_Desc, " Site_Use_ID = " & ddlOrganization.SelectedValue)
                '        ddlCustomer.DataBind()
                '        ddlCustomer.Items.Insert(0, New ListItem("-- Select a value --"))

                '        ddlVan.SelectedValue = SID
                '        txtFromDate.Text = FromDate
                '        txtToDate.Text = ToDate
                '        ddlCustomer.SelectedValue = CID
                '        ddlOrderType.SelectedValue = OType

                '        BindData()
                '        RemoveCookie("OID")
                '        RemoveCookie("SID")
                '        RemoveCookie("FromDate")
                '        RemoveCookie("ToDate")
                '        RemoveCookie("Customer")
                '        RemoveCookie("OType")

                '    End If
                'End If
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
    'Public Sub RemoveCookie(ByVal key As String)
    '    'get cookies value
    '    Dim cookie As HttpCookie = Nothing
    '    If HttpContext.Current.Request.Cookies(key) IsNot Nothing Then
    '        cookie = HttpContext.Current.Request.Cookies(key)
    '        'You can't directly delte cookie you should
    '        'set its expiry date to earlier date
    '        cookie.Expires = DateTime.Now.AddDays(-1)
    '        HttpContext.Current.Response.Cookies.Add(cookie)
    '    End If

    'End Sub
    'Protected Function GetUrl(ByVal OrigRef As Object, ByVal rowid As Object, ByVal cust As Object, ByVal OType As Object) As String
    '    Dim QString As String = "OrdersDetails.aspx?OrigRef=" & OrigRef.ToString() & "&rowid=" & rowid.ToString() & "&cust=" & cust.ToString() & "&OType=" & OType.ToString()
    '    If Not IsNothing(Request.QueryString("visitid")) Then
    '        Dim VisitID As String = Request.QueryString("visitid").ToString()
    '        QString = QString & "&visitid=" & VisitID.ToString()
    '    End If

    '    Return QString

    'End Function
    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        MpInfoError.Show()
        Exit Sub
    End Sub

    Private Sub InitReportViewer(ByVal FilterValue As String)
        Try

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim Searchvalue As New ReportParameter
            Searchvalue = New ReportParameter("SearchParams", CStr(IIf(FilterValue Is Nothing, "", FilterValue)))

            Dim VisitID As New ReportParameter
            VisitID = New ReportParameter("VisitID", "0")

            Dim OrgID As New ReportParameter
            OrgID = New ReportParameter("OrgID", CStr(ddlOrganization.SelectedValue.ToString()))

            Dim FromDate As New ReportParameter
            FromDate = New ReportParameter("FromDate", txtFromDate.Text)

            Dim ToDate As New ReportParameter
            ToDate = New ReportParameter("ToDate", txtToDate.Text)

            Dim SalesRep_ID As New ReportParameter
            SalesRep_ID = New ReportParameter("SID", CStr(ddlVan.SelectedItem.Value.ToString()))

            Dim OrgName As New ReportParameter
            OrgName = New ReportParameter("OrgName", CStr(IIf(ddlOrganization.SelectedIndex = 0, "0", ddlOrganization.SelectedItem.Text.Trim())))

           
            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                .ServerReport.SetParameters(New ReportParameter() {Searchvalue, OrgID, VisitID, SalesRep_ID, FromDate, ToDate, OrgName})
                .ServerReport.Refresh()

            End With


        Catch Ex As Exception
            '  log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub
    Private Sub BindData()
        Dim SearchQuery As String = ""
        Try
            ObjCustomer = New Customer()
            ObjCommon = New Common()
            If (ddlCustomer.SelectedItem.Value = "-- Select a value --" And ddlVan.SelectedItem.Value = "0" And txtFromDate.Text = "" And txtToDate.Text = "") Then
                SearchQuery = ""
            Else
                If ddlVan.SelectedValue <> "0" Then
                    SearchQuery = " And A.Created_By=" & ddlVan.SelectedValue
                Else
                    SearchQuery = " And A.Created_By in (" & ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID) & ")"
                End If

                If ddlCustomer.SelectedValue <> "-- Select a value --" Then
                    SearchQuery = SearchQuery & " AND (LTRIM(STR(B.Customer_ID)) + '$' + LTRIM(STR(B.Site_Use_ID)))='" & ddlCustomer.SelectedValue & "' "
                End If

                If ddlOrderType.SelectedValue = "Invoice" Then
                    SearchQuery = SearchQuery & " AND OrderType='Invoice'"
                End If
                If ddlOrderType.SelectedValue = "Sales Order" Then
                    SearchQuery = SearchQuery & " AND OrderType='Sales Order'"
                End If
                If ddlOrderType.SelectedValue = "Proforma Order" Then
                    SearchQuery = SearchQuery & " AND OrderType='Proforma Order'"
                End If

                If txtFromDate.Text <> "" Then
                    SearchQuery = SearchQuery & " And A.System_Order_Date >=convert(datetime,'" & txtFromDate.Text & "',103)"
                End If
                If txtToDate.Text <> "" Then
                    SearchQuery = SearchQuery & " And A.System_Order_Date <= convert(datetime,'" & txtToDate.Text & " 23:59:59',103)"
                End If
            End If
                If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then
                ' SearchQuery = SearchQuery & " And A.Ship_To_Site_ID=" & ddlOrganization.SelectedItem.Value
                    InitReportViewer(SearchQuery)
                Else
                    MessageBoxValidation("Select an organization.")
                End If
                '    Dim ds As New DataSet
                '    ds = ObjCustomer.GetOrders(Err_No, Err_Desc, SearchQuery, "")
                '    ' Dim dv As New DataView(ds.Tables("OrdersTbl"))
                '    dv = New DataView(ds.Tables("OrdersTbl"))
                '    If ddlOrderType.SelectedValue = "Invoice" Then
                '        dv.RowFilter = "OrderType='Invoice'"
                '    End If
                '    If ddlOrderType.SelectedValue = "Order" Then
                '        dv.RowFilter = "OrderType='Order'"
                '    End If
                '    If SortField <> "" Then
                '        dv.Sort = (SortField & " ") + SortDirection
                '    Else
                '        dv.Sort = "Orig_Sys_Document_Ref ASC"
                '    End If

                '    GVOrders.DataSource = dv
                '    GVOrders.DataBind()

                '    Session("OrdersDT") = dv.ToTable()

                '    AddSortImage()
                '    hdnVisitID.Value = ""
                '    lblCustomer.Text = ""
                'Else
                '    If hdnVisitID.Value <> "" Then
                '        SearchQuery = SearchQuery & " And A.Visit_ID='" & hdnVisitID.Value & "'"
                '        Dim ds As New DataSet
                '        ds = ObjCustomer.GetOrders(Err_No, Err_Desc, SearchQuery, "")
                '        Dim dv As New DataView(ds.Tables("OrdersTbl"))
                '        If ddlOrderType.SelectedValue = "Invoice" Then
                '            dv.RowFilter = "OrderType='Invoice'"
                '        End If
                '        If ddlOrderType.SelectedValue = "Order" Then
                '            dv.RowFilter = "OrderType='Order'"
                '        End If
                '        If SortField <> "" Then
                '            dv.Sort = (SortField & " ") + SortDirection
                '        Else
                '            dv.Sort = "Orig_Sys_Document_Ref ASC"
                '        End If
                '        GVOrders.DataSource = dv
                '        GVOrders.DataBind()
                '        Session("OrdersDT") = dv.ToTable()
                '        AddSortImage()
                '    End If
            'End If

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

    'Sub BindforVisit()
    '    ObjCustomer = New Customer()
    '    Dim SearchQuery As String = ""
    '    If hdnVisitID.Value <> "" Then
    '        SearchQuery = SearchQuery & " And A.Visit_ID='" & hdnVisitID.Value & "'"
    '        Dim ds As New DataSet
    '        ds = ObjCustomer.GetOrders(Err_No, Err_Desc, SearchQuery, "")
    '        Dim dv As New DataView(ds.Tables("OrdersTbl"))
    '        If ddlOrderType.SelectedValue = "Invoice" Then
    '            dv.RowFilter = "OrderType='Invoice'"
    '        End If
    '        If ddlOrderType.SelectedValue = "Order" Then
    '            dv.RowFilter = "OrderType='Order'"
    '        End If
    '        If SortField <> "" Then
    '            dv.Sort = (SortField & " ") + SortDirection
    '        Else
    '            dv.Sort = "Orig_Sys_Document_Ref ASC"
    '        End If
    '        GVOrders.DataSource = dv
    '        GVOrders.DataBind()
    '        Session("OrdersDT") = dv.ToTable()
    '        AddSortImage()

    '        btnBack.Visible = True
    '        If Not IsNothing(Request.QueryString("visitid")) Then
    '            btnBack.PostBackUrl = "~/html/CustomerVisitDetails.aspx?visitid=" & Request.QueryString("visitid").ToString()
    '        End If
    '    End If
    'End Sub
    'Private Property SortField() As String
    '    Get
    '        If ViewState("SortColumn") Is Nothing Then
    '            ViewState("SortColumn") = ""
    '        End If
    '        Return ViewState("SortColumn").ToString()
    '    End Get
    '    Set(ByVal value As String)
    '        ViewState("SortColumn") = value
    '    End Set
    'End Property
    'Private Property SortFieldDtl() As String
    '    Get
    '        If ViewState("SortColumn1") Is Nothing Then
    '            ViewState("SortColumn1") = ""
    '        End If
    '        Return ViewState("SortColumn1").ToString()
    '    End Get
    '    Set(ByVal value As String)
    '        ViewState("SortColumn1") = value
    '    End Set
    'End Property
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
    '    For i As Integer = 0 To GVOrders.Columns.Count - 1
    '        Dim dcf As DataControlField = GVOrders.Columns(i)
    '        If dcf.SortExpression = SortField Then
    '            GVOrders.HeaderRow.Cells(i).Controls.Add(sortImage)
    '            Exit For
    '        End If
    '    Next
    'End Sub

    'Private Sub GVOrders_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GVOrders.PageIndexChanging
    '    GVOrders.PageIndex = e.NewPageIndex
    '    BindData()
    'End Sub

    'Private Sub GVOrders_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GVOrders.Sorting
    '    SortField = e.SortExpression
    '    SortDirection = "flip"
    '    BindData()
    'End Sub

    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click


        If Not IsDate(txtFromDate.Text) Then
            MessageBoxValidation("Enter valid ""From date"".")
            SetFocus(txtFromDate)
            Exit Sub
        End If

        If Not IsDate(txtToDate.Text) Then
            MessageBoxValidation("Enter valid ""To date"".")
            SetFocus(txtToDate)
            Exit Sub
        End If
        If CDate(txtFromDate.Text) > CDate(txtToDate.Text) Then
            MessageBoxValidation("Start Date should not be greater than End Date.")
            Exit Sub
        End If
        If ddlOrganization.SelectedItem.Value = "-- Select a value --" Then
            MessageBoxValidation("Select an Organization.")
            Exit Sub
        End If
        'If Not IsNothing(Session("USER_ACCESS")) Then
        '    If CType(Session("USER_ACCESS"), UserAccess).Designation <> "A" Then
        '        GVOrders.Columns(6).Visible = False
        '    End If
        'End If
        'If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then
        '    BindData()
        '    Response.Cookies.Add(New HttpCookie("OID", ddlOrganization.SelectedValue))
        '    Response.Cookies.Add(New HttpCookie("SID", ddlVan.SelectedValue))
        '    Response.Cookies.Add(New HttpCookie("FromDate", txtFromDate.Text.Trim()))
        '    Response.Cookies.Add(New HttpCookie("ToDate", txtToDate.Text.Trim()))
        '    Response.Cookies.Add(New HttpCookie("Customer", ddlCustomer.SelectedValue))
        '    Response.Cookies.Add(New HttpCookie("OType", ddlOrderType.SelectedValue))
        'End If
        BindData()
    End Sub

    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New Common()
            ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())
            ddlVan.DataBind()
            ddlVan.Items.Insert(0, New ListItem("-- Select a value --", "0"))

            ddlCustomer.DataSource = ObjCommon.GetCustomerByCriteria(Err_No, Err_Desc, ddlOrganization.SelectedValue)
            ddlCustomer.DataBind()
            ddlCustomer.Items.Insert(0, New ListItem("-- Select a value --"))

        End If
        RVMain.Reset()

    End Sub



    'Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    '    Return
    'End Sub


    'Protected Sub imgPrint_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgPrint.Click
    '    If IsNothing(Session("OrdersDT")) Then
    '        Response.Redirect("Login.aspx")
    '    End If
    '    GVOrders.AllowPaging = False
    '    Dim dt As New DataTable
    '    dt = Session("OrdersDT")
    '    GVOrders.DataSource = dt
    '    GVOrders.DataBind()
    '    Dim sw As New StringWriter()
    '    Dim hw As New HtmlTextWriter(sw)
    '    GVOrders.RenderControl(hw)
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
    '    GVOrders.AllowPaging = True
    '    GVOrders.DataBind()

    'End Sub
    'Protected Sub Cust_Click(ByVal sender As Object, ByVal e As EventArgs)
    '    Try
    '        Dim btnCust As LinkButton = TryCast(sender, LinkButton)

    '        Response.Cookies.Add(New HttpCookie("OID", ddlOrganization.SelectedValue))
    '        Response.Cookies.Add(New HttpCookie("SID", ddlVan.SelectedValue))
    '        Response.Cookies.Add(New HttpCookie("FromDate", txtFromDate.Text.Trim()))
    '        Response.Cookies.Add(New HttpCookie("ToDate", txtToDate.Text.Trim()))
    '        Response.Cookies.Add(New HttpCookie("Customer", ddlCustomer.SelectedValue))
    '        Response.Cookies.Add(New HttpCookie("OType", ddlOrderType.SelectedValue))
    '        Response.Redirect(btnCust.CommandArgument)
    '    Catch ex As Exception

    '    End Try
    'End Sub



End Class