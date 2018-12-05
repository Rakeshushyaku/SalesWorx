Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Partial Public Class DashboardVANLog
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjDashboard As DashboardCom
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
                CType(GVVanLog.Columns(1), BoundField).DataFormatString = CType(Session.Item("USER_ACCESS"), UserAccess).DecimalDigits
                CType(GVVanLog.Columns(2), BoundField).DataFormatString = CType(Session.Item("USER_ACCESS"), UserAccess).DecimalDigits
                CType(GVVanLog.Columns(3), BoundField).DataFormatString = CType(Session.Item("USER_ACCESS"), UserAccess).DecimalDigits
                BindData()
                If Not Err_Desc Is Nothing Then
                    log.Error(Err_Desc)
                    Response.Redirect("DashInformation.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
                End If
            Catch ex As Exception
                Err_No = "74066"
                If Err_Desc Is Nothing Then
                    log.Error(GetExceptionInfo(ex))
                Else
                    log.Error(Err_Desc)
                End If
                Response.Redirect("DashInformation.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
            Finally
                ErrorResource = Nothing
            End Try
        End If
    End Sub

    Private Sub DummyBindData()
        'Dim ds As New DataSet()
        'Dim dt As New DataTable()
        'dt.Columns.Add("VAN #")
        'dt.Columns.Add("Total Sales")
        'dt.Columns.Add("Total Credit Note")
        'dt.Columns.Add("Payment")
        'dt.Columns.Add("Productive Calls")
        'dt.Columns.Add("Total Calls")
        'For k As Integer = 1 To 8
        '    dt.Rows.Add(New Object() {"VAN #" & k, 5000, "2.00", 4000, 92, 100})
        'Next
        'ds.Tables.Add(dt)
        'GVVanLog.DataSourceID = Nothing
        'GVVanLog.DataSource = ds.Tables(0).DefaultView
        'GVVanLog.DataBind()
    End Sub

    Private Sub BindData()
        Try
            ObjDashboard = New DashboardCom()
            Dim dtNow As DateTime = Now.ToString("MM/dd/yyyy")
            Dim nowdayofweek As Integer = dtNow.DayOfWeek
            Dim monthStartDate, monthEndDate As DateTime
            monthStartDate = dtNow.AddDays(-(dtNow.Day - 1))
            monthEndDate = dtNow.AddMonths(1)
            monthEndDate = monthEndDate.AddDays(-(monthEndDate.Day))
            Dim userid As Integer
            userid = CType(Session("User_Access"), UserAccess).UserID
            Dim ds As New DataSet
            ds = ObjDashboard.GetVanLog(Err_No, Err_Desc, userid, monthStartDate, monthEndDate)
            Dim dv As New DataView(ds.Tables("VanLogTbl"))
            If SortField <> "" Then
                dv.Sort = (SortField & " ") + SortDirection
            End If
            GVVanLog.DataSource = dv
            GVVanLog.DataBind()
            AddSortImage()
        Catch ex As Exception
            If Err_Desc IsNot Nothing Then
                log.Error(Err_Desc)
            Else
                log.Error(GetExceptionInfo(ex))
            End If
            Err_No = "74067"
            '  Err_Desc = ex.Message
            Response.Redirect("DashInformation.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
        Finally
            ObjDashboard = Nothing
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
        For i As Integer = 0 To GVVanLog.Columns.Count - 1
            Dim dcf As DataControlField = GVVanLog.Columns(i)
            If dcf.SortExpression = SortField Then
                GVVanLog.HeaderRow.Cells(i).Controls.Add(sortImage)
                Exit For
            End If
        Next
    End Sub

    Private Sub GVVanLog_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GVVanLog.PageIndexChanging
        GVVanLog.PageIndex = e.NewPageIndex
        BindData()
    End Sub

    Private Sub GVVanLog_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GVVanLog.Sorting
        SortField = e.SortExpression
        SortDirection = "flip"
        BindData()
    End Sub
End Class