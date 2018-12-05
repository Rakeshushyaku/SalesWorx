Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net

Partial Public Class ReviewMsg
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim objMessage As Message
    'Dim SortField As String = ""
    Private Const PageID As String = "P71"
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
   
    Private Sub ReviewMsg_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Review Messages"
    End Sub
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
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
            objMessage = New Message()
            From_Message_Date.Text = Date.Today.Day & "/" & Date.Today.Month & "/" & Date.Today.Year
            To_Message_Date.Text = Date.Today.Day & "/" & Date.Today.Month & "/" & Date.Today.Year
            ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
            Try
                Dim UD_SUB_QRY = "select SalesRep_ID from dbo.app_GetControlInfo(" & CType(Session("User_Access"), UserAccess).UserID & ")"
                SalesRep_ID.DataSource = objMessage.GetSalesRepList(Err_No, Err_Desc, UD_SUB_QRY)
                SalesRep_ID.DataBind()
                SalesRep_ID.Items.Insert(0, New ListItem("-- Select a Van --"))
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
                objMessage = Nothing
                ErrorResource = Nothing
            End Try
        End If
    End Sub

    Private Sub BindData()
        objMessage = New Message()
        Dim SearchQuery As String
        Try
            If (From_Message_Date.Text = "" And To_Message_Date.Text = "" And SalesRep_ID.SelectedItem.Value = "-- Select a Van --") Then
                SearchQuery = ""
            ElseIf (From_Message_Date.Text = "" And To_Message_Date.Text = "" And SalesRep_ID.SelectedItem.Value <> "-- Select a Van --") Then
                SearchQuery = " And SalesRep_Name='" & SalesRep_ID.SelectedItem.Text & "'"
            ElseIf (From_Message_Date.Text <> "" And To_Message_Date.Text = "" And SalesRep_ID.SelectedItem.Value = "-- Select a Van --") Then
                Dim MsgDat As Array = From_Message_Date.Text.Split("/")
                SearchQuery = "  and  Message_Date>='" & MsgDat(1) & "-" & MsgDat(0) & "-" & MsgDat(2) & " 00:00:00' AND Message_Date<='" & MsgDat(1) & "-" & MsgDat(0) & "-" & MsgDat(2) & " 23:59:59'"
            ElseIf (From_Message_Date.Text <> "" And To_Message_Date.Text <> "" And SalesRep_ID.SelectedItem.Value = "-- Select a Van --") Then
                Dim MsgDat As Array = From_Message_Date.Text.Split("/")
                Dim ToDat As Array = To_Message_Date.Text.Split("/")
                SearchQuery = " and  Message_Date>='" & MsgDat(1) & "-" & MsgDat(0) & "-" & MsgDat(2) & " 00:00:00' AND Message_Date<='" & ToDat(1) & "-" & ToDat(0) & "-" & ToDat(2) & " 23:59:59'"
            ElseIf (From_Message_Date.Text <> "" And To_Message_Date.Text = "" And SalesRep_ID.SelectedItem.Value <> "-- Select a Van --") Then
                Dim MsgDat As Array = From_Message_Date.Text.Split("/")
                SearchQuery = " And SalesRep_Name='" & SalesRep_ID.SelectedItem.Text & "' and  Message_Date>='" & MsgDat(1) & "-" & MsgDat(0) & "-" & MsgDat(2) & " 00:00:00' AND Message_Date<='" & MsgDat(1) & "-" & MsgDat(0) & "-" & MsgDat(2) & " 23:59:59'"
            ElseIf (From_Message_Date.Text <> "" And To_Message_Date.Text <> "" And SalesRep_ID.SelectedItem.Value <> "-- Select a Van --") Then
                Dim MsgDat As Array = From_Message_Date.Text.Split("/")
                Dim ToDat As Array = To_Message_Date.Text.Split("/")
                SearchQuery = " And SalesRep_Name='" & SalesRep_ID.SelectedItem.Text & "' and  Message_Date>='" & MsgDat(1) & "-" & MsgDat(0) & "-" & MsgDat(2) & " 00:00:00' AND Message_Date<='" & ToDat(1) & "-" & ToDat(0) & "-" & ToDat(2) & " 23:59:59'"
            ElseIf (From_Message_Date.Text = "" And To_Message_Date.Text <> "" And SalesRep_ID.SelectedItem.Value <> "-- Select a Van --") Then
                SearchQuery = " And SalesRep_Name='" & SalesRep_ID.SelectedItem.Text & "'"
            End If

            Dim UD_SUB_QRY = "select SalesRep_ID from dbo.app_GetControlInfo(" & CType(Session("User_Access"), UserAccess).UserID & ")"
            Dim ds As New DataSet
            ds = objMessage.GetSearchMessage(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY)
            Dim dv As New DataView(ds.Tables("MSgTbl"))
            If SortField <> "" Then
                dv.Sort = (SortField & " ") + SortDirection
            End If
            SearchResultGridView.DataSource = dv
            SearchResultGridView.DataBind()
            AddSortImage()
        Catch ex As Exception
            If Err_Desc IsNot Nothing Then
                log.Error(Err_Desc)
            Else
                log.Error(GetExceptionInfo(ex))
            End If
            Err_No = "74067"
            '  Err_Desc = ex.Message
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
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

        ' Search for the columm with the sort expression 
        For i As Integer = 0 To SearchResultGridView.Columns.Count - 1
            Dim dcf As DataControlField = SearchResultGridView.Columns(i)
            If dcf.SortExpression = SortField Then
                ' Add image to corresponding header row
                SearchResultGridView.HeaderRow.Cells(i).Controls.Add(sortImage)
                Exit For
            End If
        Next
    End Sub
    Protected Sub SearchResultGridView_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles SearchResultGridView.PageIndexChanging
        SearchResultGridView.PageIndex = e.NewPageIndex
        BindData()
    End Sub

    Private Sub SearchResultGridView_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles SearchResultGridView.Sorting
        SortField = e.SortExpression
        SortDirection = "flip"
        BindData()
    End Sub
    Protected Sub SearchBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SearchBtn.Click
        BindData()
    End Sub
End Class