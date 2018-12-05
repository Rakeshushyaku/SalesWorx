Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Partial Public Class DistributionCheckDetails
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
                    If Not IsNothing(Request.QueryString("distchkid")) Then
                        hdnDistribution_Check_ID.Value = Request.QueryString("distchkid").ToString()
                        BindData()
                        LoadDetails(hdnDistribution_Check_ID.Value)
                    End If
                    Dim rowid As String = ""
                    If Not IsNothing(Request.QueryString("cust")) Then
                        lblCustName.Text = Request.QueryString("cust").ToString()
                    End If
                    Dim VisitID As String = ""
                    Dim Qstring As String = ""
                    If (Not IsNothing(Request.Cookies.Item("DC_OID"))) Then
                        Dim OID As String = Request.Cookies.Item("DC_OID").Value
                        Dim SID As String = Request.Cookies.Item("DC_SID").Value
                        Dim FromDate As String = Request.Cookies.Item("DC_FromDate").Value
                        Dim ToDate As String = Request.Cookies.Item("DC_ToDate").Value
                        Dim Customer As String = Request.Cookies.Item("DC_Customer").Value

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

                    End If
                    If Qstring = "" Then
                        If Not IsNothing(Request.QueryString("visitid")) Then
                            VisitID = Request.QueryString("visitid").ToString()
                            btnBack.PostBackUrl = "~/html/DistributionCheckList.aspx?visitid=" & VisitID
                        Else
                            btnBack.PostBackUrl = "~/html/DistributionCheckList.aspx" & Qstring
                        End If
                    Else
                        btnBack.PostBackUrl = "~/html/DistributionCheckList.aspx" & Qstring
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
            dt = ObjCustomer.GetDistributionCheckDetails(Err_No, Err_Desc, "And A.DistributionCheck_ID='" + rowid + "'", "")
            Dim dr As DataRow = Nothing
            If dt.Rows.Count > 0 Then
                dr = dt.Rows(0)
                lblEmp_Code.Text = dr("Emp_Code").ToString()
                lblStatus.Text = dr("Status").ToString()
                lblCheckedDate.Text = String.Format("{0:dd/MM/yyyy}", dr("Checked_On")) ''
            End If
            If Not IsNothing(Session("USER_ACCESS")) Then
                If CType(Session("USER_ACCESS"), UserAccess).Designation <> "A" Then
                    trstat.Visible = False
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
            If hdnDistribution_Check_ID.Value <> "" Then
                SearchQuery = SearchQuery & " And a.DistributionCheck_ID='" & hdnDistribution_Check_ID.Value & "'"
                Dim ds As New DataSet
                ds = ObjCustomer.GetDistributionChecksLineItem(Err_No, Err_Desc, SearchQuery, "")
                Dim dv As New DataView(ds.Tables("DistChkLineItemTbl"))
                If SortField <> "" Then
                    dv.Sort = (SortField & " ") + SortDirection
                End If
                GVDistributionDetail.DataSource = dv
                GVDistributionDetail.DataBind()
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
        For i As Integer = 0 To GVDistributionDetail.Columns.Count - 1
            Dim dcf As DataControlField = GVDistributionDetail.Columns(i)
            If dcf.SortExpression = SortField Then
                GVDistributionDetail.HeaderRow.Cells(i).Controls.Add(sortImage)
                Exit For
            End If
        Next
    End Sub

    Private Sub GVDistributionDetail_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GVDistributionDetail.PageIndexChanging
        GVDistributionDetail.PageIndex = e.NewPageIndex
        BindData()
    End Sub

    Private Sub GVDistributionDetail_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GVDistributionDetail.Sorting
        SortField = e.SortExpression
        SortDirection = "flip"
        BindData()
    End Sub

    Protected Sub GVDistributionDetail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVDistributionDetail.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim IsAvlDC As String = DataBinder.Eval(e.Row.DataItem, "Is_Available").ToString()
            Dim Ordered As String = DataBinder.Eval(e.Row.DataItem, "ExitInfo").ToString()
            Dim imgEntryAvail As Image = e.Row.FindControl("imgEntryAvail")
            Dim imgExitAvail As Image = e.Row.FindControl("imgAvail")
            If IsAvlDC = "Yes" Then
                imgEntryAvail.ImageUrl = "~/images/yes_icon.gif"
            Else
                imgEntryAvail.ImageUrl = "~/images/no_icon.gif"
            End If
          



            If DataBinder.Eval(e.Row.DataItem, "Qty").ToString() = "0.0000" Then
                e.Row.Cells(3).Text = ""
                e.Row.Cells(5).Text = ""
            End If
            If IsAvlDC = "Yes" And Ordered = "Yes" Then
                imgExitAvail.ImageUrl = "~/images/yes_icon.gif"
            End If
            If IsAvlDC = "No" And Ordered = "Yes" Then
                imgExitAvail.ImageUrl = "~/images/yes_icon.gif"
            End If
            If IsAvlDC = "No" And Ordered = "No" Then
                imgExitAvail.ImageUrl = "~/images/no_icon.gif"
            End If
            If IsAvlDC = "Yes" And Ordered = "No" Then
                imgExitAvail.ImageUrl = "~/images/yes_icon.gif"
            End If
        End If
    End Sub
End Class