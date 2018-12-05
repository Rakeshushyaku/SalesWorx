Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Partial Public Class DashboardPDCReleased
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCollection As Collection
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
    Private Sub BindData()
        Dim SearchQuery As String = ""
        Try
            ObjCollection = New Collection()
            Dim dtNow As DateTime = Now.ToString("MM/dd/yyyy")
            Dim nowdayofweek As Integer = dtNow.DayOfWeek
            Dim weekStartDate, weekEndDate As DateTime
            weekStartDate = DateAdd("d", 0 - dtNow.DayOfWeek, dtNow)
            weekEndDate = DateAdd("d", 6 - dtNow.DayOfWeek, dtNow)

            'SearchQuery = SearchQuery & " And A.Collected_On >= '" & weekStartDate & "'"
            'SearchQuery = SearchQuery & " And A.Collected_On <= '" & weekEndDate & " 23:59:59'"


            Dim obj As UserAccess
            obj = CType(Session("User_Access"), UserAccess)
            Dim strSiteID As String = ""
            For i = 0 To obj.OrgID.Count - 1
                If Not strSiteID.Contains(obj.OrgID(i)) Then
                    strSiteID = strSiteID & obj.OrgID(i) & ","
                End If
            Next
            If strSiteID <> "" Then
                strSiteID = strSiteID.Substring(0, strSiteID.Length - 1)
            End If
            SearchQuery = SearchQuery & " And DATEADD(dd,-1*(SELECT TOP 1 Advance_PDC_Posting FROm TBL_Div_Config WHERE Org_ID IN (" & strSiteID & ") ),A.Cheque_Date) <= DATEADD(dd,(SELECT TOP 1 Advance_PDC_Posting FROm TBL_Div_Config WHERE Org_ID IN (" & strSiteID & ") ),GETDATE()) "
            '            SearchQuery = SearchQuery & " And DATEADD(dd,-1*(SELECT TOP 1 Advance_PDC_Posting FROm TBL_Div_Config WHERE Org_ID=A.Site_Use_ID),A.Cheque_Date) <= DATEADD(dd,(SELECT TOP 1 Advance_PDC_Posting FROm TBL_Div_Config WHERE Org_ID=A.Site_Use_ID),GETDATE()) "
            'SearchQuery = SearchQuery & " And A.Collected_On <= '" & weekEndDate & " 23:59:59'"


            ' SearchQuery = SearchQuery & " And A.Site_Use_ID in (" & strSiteID & ")"
            Dim ds As New DataSet
            ds = ObjCollection.GetCollectionList(Err_No, Err_Desc, SearchQuery, "", strSiteID)
            Dim dv As New DataView(ds.Tables("CollLstTbl"))
            If SortField <> "" Then
                dv.Sort = (SortField & " ") + SortDirection
            End If
            GVCollectionList.DataSource = dv
            GVCollectionList.DataBind()
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
            ObjCollection = Nothing
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
        For i As Integer = 0 To GVCollectionList.Columns.Count - 1
            Dim dcf As DataControlField = GVCollectionList.Columns(i)
            If dcf.SortExpression = SortField Then
                GVCollectionList.HeaderRow.Cells(i).Controls.Add(sortImage)
                Exit For
            End If
        Next
    End Sub

    Private Sub GVCollectionList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GVCollectionList.PageIndexChanging
        GVCollectionList.PageIndex = e.NewPageIndex
        BindData()
    End Sub

    Private Sub GVCollectionList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GVCollectionList.Sorting
        SortField = e.SortExpression
        SortDirection = "flip"
        BindData()
    End Sub
End Class