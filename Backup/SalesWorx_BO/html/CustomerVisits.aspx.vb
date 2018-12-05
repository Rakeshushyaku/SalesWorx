Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO

Partial Public Class CustomerVisits
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Dim ObjCustomer As Customer
    'Dim SortField As String = ""
    'Dim SortFieldDtl As String = ""
    Private Const PageID As String = "P21"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single

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
    Private Property SortFieldDtl() As String
        Get
            If ViewState("SortColumn1") Is Nothing Then
                ViewState("SortColumn1") = ""
            End If
            Return ViewState("SortColumn1").ToString()
        End Get
        Set(ByVal value As String)
            ViewState("SortColumn1") = value
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
                txtFromDate.Text = Format(Now().Date, "dd/MM/yyyy")
                txtToDate.Text = Format(Now().Date, "dd/MM/yyyy")
                'ddlVan.DataSource = ObjCommon.GetAllVan(Err_No, Err_Desc, SubQry)
                'ddlVan.DataBind()
                'ddlVan.Items.Insert(0, New ListItem("-- Select a value --"))
                'ddlCustClass.DataSource = ObjCommon.GetCustomerClass(Err_No, Err_Desc, SubQry)
                'ddlCustClass.DataBind()
                'ddlCustClass.Items.Insert(0, New ListItem("-- Select a Customer Class --"))
                'ddlType.DataSource = ObjCommon.GetCustomerTypeList(Err_No, Err_Desc, SubQry)
                'ddlType.DataBind()
                'ddlType.Items.Insert(0, New ListItem("-- Select a Customer Type --"))
                'ddlCustomer.DataSource = ObjCommon.GetCustomer(Err_No, Err_Desc, SubQry)
                'ddlCustomer.DataBind()
                'ddlCustomer.Items.Insert(0, New ListItem("-- Select a value --"))

                'BindData()

                tblTot.Visible = False



                If Not IsNothing(Request.QueryString("OID")) Then
                    Dim OID As String = Request.QueryString("OID")
                    Dim SID As String = "-- Select a value --"
                    If Not IsNothing(Request.QueryString("SID")) Then
                        SID = Request.QueryString("SID").ToString()
                    End If
                    Dim FromDate As String = ""
                    If Not IsNothing(Request.QueryString("FD")) Then
                        FromDate = Request.QueryString("FD").ToString()
                    End If
                    Dim ToDate As String = ""
                    If Not IsNothing(Request.QueryString("TD")) Then
                        ToDate = Request.QueryString("TD").ToString()
                    End If
                    Dim CID As String = "-- Select a value --"
                    If Not IsNothing(Request.QueryString("Ct")) Then
                        CID = Request.QueryString("Ct").ToString()
                    End If


                    ddlOrganization.SelectedValue = OID
                    Dim objUserAccess As UserAccess
                    objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
                    ObjCommon = New Common()
                    ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
                    ddlVan.DataBind()
                    ddlVan.Items.Insert(0, New ListItem("-- Select a value --"))

                    ddlCustomer.DataSource = ObjCommon.GetCustomerByCriteria(Err_No, Err_Desc, ddlOrganization.SelectedValue)
                    ddlCustomer.DataBind()
                    ddlCustomer.Items.Insert(0, New ListItem("-- Select a value --"))

                    ddlVan.SelectedValue = SID
                    txtFromDate.Text = FromDate
                    txtToDate.Text = ToDate
                    ddlCustomer.SelectedValue = CID
                    BindData()
                    RemoveCookie("CVOID")
                    RemoveCookie("CVSID")
                    RemoveCookie("CVFromDate")
                    RemoveCookie("CVToDate")
                    RemoveCookie("CVCustomer")

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
    Private Sub BindData()
        Dim SearchQuery As String = ""
        Dim SalesRepId As Integer = 0
        Dim CustId As Integer = 0
        Dim fromdate As DateTime
        Dim todate As DateTime
        fromdate = System.Data.SqlTypes.SqlDateTime.Null
        todate = System.Data.SqlTypes.SqlDateTime.Null


        Try
            ObjCustomer = New Customer()
            ObjCommon = New Common()
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            If (ddlCustomer.SelectedItem.Value = "-- Select a value --" And ddlVan.SelectedItem.Value = "-- Select a value --" And txtFromDate.Text = "" And txtToDate.Text = "") Then
                SearchQuery = ""
            Else
                If ddlVan.SelectedValue <> "-- Select a value --" Then
                    SearchQuery = " And A.SalesRep_ID=" & ddlVan.SelectedValue
                    SalesRepId = ddlVan.SelectedValue
                Else
                    SearchQuery = " And A.SalesRep_ID in (" & ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID) & ")"
                End If
                'If ddlCustClass.SelectedValue <> "-- Select a Customer Class --" Then
                '    SearchQuery = SearchQuery & " And B.Customer_Class='" & ddlCustClass.SelectedValue & "'"
                'End If
                'If ddlType.SelectedValue <> "-- Select a Customer Type --" Then
                '    SearchQuery = SearchQuery & " And B.Customer_Type='" & ddlType.SelectedValue & "'"
                'End If
                If ddlCustomer.SelectedValue <> "-- Select a value --" Then
                    SearchQuery = SearchQuery & " AND (LTRIM(STR(B.Customer_ID)) + '$' + LTRIM(STR(B.Site_Use_ID)))='" & ddlCustomer.SelectedValue & "' "
                    CustId = Convert.ToInt32(ddlCustomer.SelectedValue.Split("$")(0))
                End If
                If txtFromDate.Text <> "" Then
                    Dim TemFromDateStr As String = txtFromDate.Text
                    Dim DateArr As Array = TemFromDateStr.Split("/")
                    TemFromDateStr = DateArr(1) & "/" & DateArr(0) & "/" & DateArr(2)
                    SearchQuery = SearchQuery & " And A.Visit_Start_Date >= '" & TemFromDateStr & "'"
                    If txtToDate.Text = "" Then
                        SearchQuery = SearchQuery & " And A.Visit_Start_Date <= '" & TemFromDateStr & " 23:59:59'"
                    End If
                    fromdate = Convert.ToDateTime(TemFromDateStr)
                End If
                If txtToDate.Text <> "" Then
                    Dim TemToDateStr As String = txtToDate.Text
                    Dim DateArr As Array = TemToDateStr.Split("/")
                    TemToDateStr = DateArr(1) & "/" & DateArr(0) & "/" & DateArr(2)
                    SearchQuery = SearchQuery & " And A.Visit_Start_Date <= '" & TemToDateStr & " 23:59:59'"
                    todate = Convert.ToDateTime(TemToDateStr)
                End If
            End If
            If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then
                SearchQuery = SearchQuery & " And A.Site_Use_ID=" & ddlOrganization.SelectedItem.Value
                Dim ds As New DataSet
                ds = ObjCustomer.GetCustomerVisits(Err_No, Err_Desc, SearchQuery, "")
                Dim dv As New DataView(ds.Tables("CustVisitsTbl"))
                If SortField <> "" Then
                    dv.Sort = (SortField & " ") + SortDirection
                Else
                    dv.Sort = "Visit_Start_Date ASC"
                    SortDirection = "ASC"
                    SortField = "Visit_Start_Date"
                End If

                TotInvoice = 0
                TotCreditNotes = 0
                TotPayment = 0

                GVCustomerVisits.DataSource = dv
                GVCustomerVisits.DataBind()
                ViewState("dt") = dv.ToTable()
                AddSortImage()

                Dim dtTot As New DataTable
                dtTot = ObjCustomer.GetCustomerVisitReport_TotalSummary(Err_No, Err_Desc, fromdate, todate, ddlOrganization.SelectedItem.Value, SalesRepId, CustId, objUserAccess.UserID)
                If dtTot.Rows.Count > 0 Then
                    lblTotCalls.Text = dtTot.Rows(0)(1).ToString()
                    lblTotProductiveCalls.Text = dtTot.Rows(1)(1).ToString()
                    lblTotSalesCurrMonth.Text = hfCurrency.Value & " " & GetPrice(dtTot.Rows(2)(1).ToString(), hfDecimal.Value)
                    lblTotCallsCurrMonth.Text = dtTot.Rows(3)(1).ToString()
                    lblTotProdCallsCurrMonth.Text = dtTot.Rows(4)(1).ToString()
                    If dtTot.Rows(3)(1).ToString() <> "0" Then
                        lblProductivityPercentage.Text = FormatNumber((Convert.ToInt32(dtTot.Rows(4)(1)) / Convert.ToInt32(dtTot.Rows(3)(1))) * 100, 2) & "%"
                    Else
                        lblProductivityPercentage.Text = "0%"
                    End If

                End If

                lblTotSalesDone.Text = hfCurrency.Value & " " & GetPrice(TotInvoice.ToString(), hfDecimal.Value)
                lblTotCNote.Text = hfCurrency.Value & " " & GetPrice(TotCreditNotes.ToString(), hfDecimal.Value)
                lblTotPayments.Text = hfCurrency.Value & " " & GetPrice(TotPayment.ToString(), hfDecimal.Value)

                If dv.Count = 0 Then
                    tblTot.Visible = False
                Else
                    tblTot.Visible = True
                End If
            Else
                MessageBoxValidation("Select an Organization.")
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

    Protected Function GetPrice(ByVal SP As Object, ByVal dec As Object) As String
        'Dim FormatString As String = "{0:N" + dec.ToString() + "}"
        hfDecimal.Value = Convert.ToInt32(dec).ToString()
        Return FormatNumber(SP, Convert.ToInt32(dec)).ToString()
    End Function
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
        For i As Integer = 0 To GVCustomerVisits.Columns.Count - 1
            Dim dcf As DataControlField = GVCustomerVisits.Columns(i)
            If dcf.SortExpression = SortField Then
                If Not IsNothing(GVCustomerVisits.HeaderRow) Then
                    GVCustomerVisits.HeaderRow.Cells(i).Controls.Add(sortImage)
                End If
                Exit For
            End If
        Next
    End Sub

    Private Sub GVCustomerVisits_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GVCustomerVisits.PageIndexChanging
        GVCustomerVisits.PageIndex = e.NewPageIndex
        BindData()
    End Sub

    Private Sub GVCustomerVisits_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GVCustomerVisits.Sorting
        SortField = e.SortExpression
        SortDirection = "flip"
        BindData()
    End Sub

    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click

        If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then

            Dim TemFromDateStr As String = txtFromDate.Text
            Dim DateArr As Array = TemFromDateStr.Split("/")
            If DateArr.Length = 3 Then
                TemFromDateStr = DateArr(1) & "/" & DateArr(0) & "/" & DateArr(2)
            End If
            Dim TemToDateStr As String = txtToDate.Text
            Dim DateArr1 As Array = TemToDateStr.Split("/")
            If DateArr1.Length = 3 Then
                TemToDateStr = DateArr1(1) & "/" & DateArr1(0) & "/" & DateArr1(2)
            End If

            If Not IsDate(TemFromDateStr) Then
                MessageBoxValidation("Enter valid ""From date"".")
                SetFocus(txtFromDate)
                Exit Sub
            End If

            If Not IsDate(TemToDateStr) Then
                MessageBoxValidation("Enter valid ""To date"".")
                SetFocus(TemToDateStr)
                Exit Sub
            End If

            If CDate(TemFromDateStr) > CDate(TemToDateStr) Then
                MessageBoxValidation("Start Date should not be greater than End Date.")
                Exit Sub
            End If
            BindData()

            Response.Cookies.Add(New HttpCookie("CVOID", ddlOrganization.SelectedValue))
            Response.Cookies.Add(New HttpCookie("CVSID", ddlVan.SelectedValue))
            Response.Cookies.Add(New HttpCookie("CVFromDate", txtFromDate.Text.Trim()))
            Response.Cookies.Add(New HttpCookie("CVToDate", txtToDate.Text.Trim()))
            Response.Cookies.Add(New HttpCookie("CVCustomer", ddlCustomer.SelectedValue))

        Else
            MessageBoxValidation("Select an organization.")
        End If
    End Sub
    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        MpInfoError.Show()
        Exit Sub
    End Sub
    Protected Sub GVCustomerVisits_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVCustomerVisits.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            TotInvoice = TotInvoice + Convert.ToSingle(DataBinder.Eval(e.Row.DataItem, "OrderAmt"))
            TotCreditNotes = TotCreditNotes + Convert.ToSingle(DataBinder.Eval(e.Row.DataItem, "RMA"))
            TotPayment = TotPayment + Convert.ToSingle(DataBinder.Eval(e.Row.DataItem, "Payment"))

            Dim ImgDC As New Image
            ImgDC = CType(e.Row.FindControl("ImgDC"), Image)
            If DataBinder.Eval(e.Row.DataItem, "DC").ToString() = "Y" Then
                ImgDC.ImageUrl = "~/images/yes_icon.gif"
            Else
                ImgDC.ImageUrl = "~/images/no_icon.gif"
            End If

            objInfo = New Vist_Info()
            objInfo.VisitDate = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "Visit_Start_Date")).ToShortDateString().ToString()
            objInfo.CustID = DataBinder.Eval(e.Row.DataItem, "Customer_ID")
            If Not alVisitInfo.Contains(objInfo) Then
                alVisitInfo.Add(objInfo)
            Else
                Dim lbl As New Label
                lbl = CType(e.Row.FindControl("lblPayment"), Label)
                lbl.Text = ""
            End If


        End If
    End Sub
    Public Sub RemoveCookie(ByVal key As String)
        'get cookies value
        Dim cookie As HttpCookie = Nothing
        If HttpContext.Current.Request.Cookies(key) IsNot Nothing Then
            cookie = HttpContext.Current.Request.Cookies(key)
            'You can't directly delte cookie you should
            'set its expiry date to earlier date
            cookie.Expires = DateTime.Now.AddDays(-1)
            HttpContext.Current.Response.Cookies.Add(cookie)
        End If

    End Sub
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged


        If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New Common()
            ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
            ddlVan.DataBind()
            ddlVan.Items.Insert(0, New ListItem("-- Select a value --"))

            ddlCustomer.DataSource = ObjCommon.GetCustomerByCriteria(Err_No, Err_Desc, ddlOrganization.SelectedValue)
            ddlCustomer.DataBind()
            ddlCustomer.Items.Insert(0, New ListItem("-- Select a value --"))

            Dim dt As New DataTable
            dt = ObjCommon.GetCurrencyByOrg(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
            If dt.Rows.Count > 0 Then
                hfCurrency.Value = dt.Rows(0)(0).ToString()
                hfDecimal.Value = dt.Rows(0)(1).ToString()
            End If
        End If

    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub


    Protected Sub imgPrint_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgPrint.Click

        GVCustomerVisits.AllowPaging = False
        Dim dt As New DataTable
        dt = ViewState("dt")
        GVCustomerVisits.DataSource = dt
        GVCustomerVisits.DataBind()
        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)
        GVCustomerVisits.RenderControl(hw)
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
        GVCustomerVisits.AllowPaging = True
        GVCustomerVisits.DataBind()

    End Sub
    Protected Sub Cust_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim btnCust As LinkButton = TryCast(sender, LinkButton)

            Response.Cookies.Add(New HttpCookie("CVOID", ddlOrganization.SelectedValue))
            Response.Cookies.Add(New HttpCookie("CVSID", ddlVan.SelectedValue))
            Response.Cookies.Add(New HttpCookie("CVFromDate", txtFromDate.Text.Trim()))
            Response.Cookies.Add(New HttpCookie("CVToDate", txtToDate.Text.Trim()))
            Response.Cookies.Add(New HttpCookie("CVCustomer", ddlCustomer.SelectedValue))
            Response.Redirect(btnCust.CommandArgument)
        Catch ex As Exception

        End Try
    End Sub
End Class

Public Class Vist_Info
    Public VisitDate As String
    Public CustID As Integer
End Class