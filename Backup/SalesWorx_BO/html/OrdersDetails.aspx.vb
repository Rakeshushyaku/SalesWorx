Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Partial Public Class OrdersDetails
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCustomer As Customer
    'Dim SortField As String = ""
    'Dim SortFieldDtl As String = ""
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
        If Not IsPostBack Then
            ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
            Try
                If Not Session.Item("USER_ACCESS") Is Nothing Then
                    '  btnBack.Attributes.Add("onClick", "javascript:history.back(); return false;")


                    Dim rowid As String = ""
                    If Not IsNothing(Request.QueryString("cust")) Then
                        lblCustName.Text = Request.QueryString("cust").ToString()
                    End If
                    If Not IsNothing(Request.QueryString("rowid")) Then
                        rowid = Request.QueryString("rowid").ToString()
                        LoadDetails(rowid)
                    End If
                    If Not IsNothing(Request.QueryString("OrigRef")) Then
                        hdnOrigRef.Value = Request.QueryString("OrigRef").ToString()
                        BindData()
                    End If
                    Dim VisitID As String = ""
                    Dim Qstring As String = ""
                    If Not IsNothing(Request.Cookies.Item("OID")) Then


                        Dim OID As String = Request.Cookies.Item("OID").Value
                        Dim SID As String = Request.Cookies.Item("SID").Value
                        Dim FromDate As String = Request.Cookies.Item("FromDate").Value
                        Dim ToDate As String = Request.Cookies.Item("ToDate").Value
                        Dim Customer As String = Request.Cookies.Item("Customer").Value
                        Dim OType As String = Request.Cookies.Item("OType").Value

                        Qstring = "?OID=" & OID
                        If SID <> "-- Select a value --" Then
                            Qstring = Qstring & "&SID=" & SID
                        End If
                        If FromDate <> "" Then
                            Qstring = Qstring & "&FD=" & FromDate
                        End If
                        If ToDate <> "" Then
                            Qstring = Qstring & "&TD=" & ToDate
                        End If
                        If Customer <> "-- Select a value --" Then
                            Qstring = Qstring & "&Ct=" & Customer
                        End If
                        If OType <> "0" Then
                            Qstring = Qstring & "&OType=" & OType
                        End If
                        If Not IsNothing(Request.QueryString("visitid")) Then
                            VisitID = Request.QueryString("visitid").ToString()
                            Qstring = Qstring & "&visitid=" & VisitID
                        End If
                    End If
                    If Qstring = "" Then
                        If Not IsNothing(Request.QueryString("visitid")) Then
                            VisitID = Request.QueryString("visitid").ToString()
                            btnBack.PostBackUrl = "~/html/Orders.aspx?visitid=" & VisitID
                        Else
                            btnBack.PostBackUrl = "~/html/Orders.aspx" & Qstring
                        End If
                    Else
                        btnBack.PostBackUrl = "~/html/Orders.aspx" & Qstring
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
                ErrorResource = Nothing
            End Try
        End If
    End Sub
    Sub LoadDetails(ByVal rowid As String)
        ObjCustomer = New Customer()
        Try
            Dim dt As New DataTable
            Dim OrderType As String = ""
            If Not IsNothing(Request.QueryString("Otype")) Then
                OrderType = Request.QueryString("Otype").ToString()
            End If
            If OrderType <> "Order" Then
                dt = ObjCustomer.GetOrderDetails(Err_No, Err_Desc, "And A.Row_ID='" + rowid + "'", "")
                lblOrderType.Text = "Invoice"
            Else
                dt = ObjCustomer.GetUnconfirmedOrderDetails(Err_No, Err_Desc, "And A.Row_ID='" + rowid + "'", "")
                lblOrderType.Text = "Unconfirmed Order"
            End If
           

            Dim dr As DataRow = Nothing
            If dt.Rows.Count > 0 Then
                dr = dt.Rows(0)
                lblOrderRef.Text = dr("Orig_Sys_Document_Ref").ToString()
                lblStatus.Text = dr("Order_Status").ToString()
                lblCustomerPO.Text = IIf(OrderType <> "U", dr("Customer_PO_Number").ToString(), "N/A")
                'lblShipTo.Text = dr("Ship_To_Customer").ToString()
                'lblInvoiceTo.Text = dr("Invoice_To_Customer").ToString()
                lblOrderDate.Text = String.Format("{0:dd/MM/yyyy}", dr("System_Order_Date"))
                lblRequestDate.Text = String.Format("{0:dd/MM/yyyy}", dr("Request_Date"))
                lblShipDate.Text = String.Format("{0:dd/MM/yyyy}", dr("Schedule_Ship_Date"))
                ' lblShipInstr.Text = dr("Shipping_Instructions").ToString()
                lblPackInstr.Text = dr("Packing_Instructions").ToString()
                lblSignedBy.Text = IIf(OrderType <> "U", dr("Signee_Name").ToString(), "N/A")
                Dim frmt As String = "N" + dr("Decimal_Digits").ToString()
                Dim amt As Double = dr("Order_Amt")
                lblOrderAmount.Text = dr("CurrDesc").ToString() + " " + amt.ToString(frmt) ')
                If OrderType <> "Order" Then
                    If Not IsDBNull(dr("Customer_Signature")) Then
                        imgSig.ImageUrl = "ViewImage.aspx?type=order&id=" & rowid
                    Else
                        trSign.Visible = False
                    End If
                Else
                    imgSig.Visible = False
                    trSign.Visible = False
                End If

                Dim VisitId As String = dr("Visit_ID").ToString()
                Dim dtCash As New DataTable
                dtCash = ObjCustomer.GetCashCustomerDetailsByVisitID(Err_No, Err_Desc, VisitId)
                If Not IsNothing(dtCash) Then
                    If dtCash.Rows.Count > 0 Then
                        Dim strName As String = ""
                        If Not IsDBNull(dtCash.Rows(0)(0)) Then strName = "Cash Customer:" + dtCash.Rows(0)(0).ToString()
                        If Not IsDBNull(dtCash.Rows(0)(1)) Then strName = strName + ", Ph.:" + dtCash.Rows(0)(1).ToString()
                        If strName <> "" Then
                            strName = "    (" + strName + ")"
                        End If
                        lblCustName.Text = lblCustName.Text & strName
                    End If
                End If
                GVOrdersDetail.Columns(5).Visible = False

                Dim bndColumn As New BoundField()
                bndColumn.DataField = "ItemPrice"
                bndColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Right
                bndColumn.HtmlEncode = False
                bndColumn.HeaderText = "Price"
                bndColumn.DataFormatString = "{0:N" + dr("Decimal_Digits").ToString() + "}"
                GVOrdersDetail.Columns.Add(bndColumn)

                lblStartTime.Text = String.Format("{0:t}", dr("Start_Time"))
                lblEndTime.Text = IIf(OrderType <> "U", String.Format("{0:t}", dr("End_Time")), "N/A")

                If Not IsNothing(Session("USER_ACCESS")) Then
                    If CType(Session("USER_ACCESS"), UserAccess).Designation <> "A" Then
                        trstat.Visible = False
                    End If
                End If
            End If
            dr = Nothing
            dt = Nothing
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
    Private Sub BindData()
        Dim SearchQuery As String = ""
        Try
            ObjCustomer = New Customer()
            If hdnOrigRef.Value <> "" Then
                SearchQuery = SearchQuery & " And A.Orig_Sys_Document_Ref='" & hdnOrigRef.Value & "'"
                Dim ds As New DataSet
                Dim OrderType As String = ""
                If Not IsNothing(Request.QueryString("Otype")) Then
                    OrderType = Request.QueryString("Otype").ToString()
                End If
                If OrderType <> "Order" Then
                    ds = ObjCustomer.GetOrdersLineItem(Err_No, Err_Desc, SearchQuery, "")
                Else
                    ds = ObjCustomer.GetUnconfirmedOrdersLineItem(Err_No, Err_Desc, SearchQuery, "")
                End If

                Dim dv As New DataView(ds.Tables("OrdersLineItemTbl"))
                If SortField <> "" Then
                    dv.Sort = (SortField & " ") + SortDirection
                End If
                GVOrdersDetail.DataSource = dv
                GVOrdersDetail.DataBind()
                AddSortImage()
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
        For i As Integer = 0 To GVOrdersDetail.Columns.Count - 1
            Dim dcf As DataControlField = GVOrdersDetail.Columns(i)
            If dcf.SortExpression = SortField Then
                GVOrdersDetail.HeaderRow.Cells(i).Controls.Add(sortImage)
                Exit For
            End If
        Next
    End Sub

    Private Sub GVOrdersDetail_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GVOrdersDetail.PageIndexChanging
        GVOrdersDetail.PageIndex = e.NewPageIndex
        BindData()
    End Sub

    Private Sub GVOrdersDetail_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GVOrdersDetail.Sorting
        SortField = e.SortExpression
        SortDirection = "flip"
        BindData()
    End Sub

  
End Class