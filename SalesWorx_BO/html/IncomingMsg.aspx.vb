Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports Telerik.Web.UI

Partial Public Class IncomingMsg
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim objMessage As Message
    ' Dim SortField As String = ""
    Private Const PageID As String = "P88"

    Private Sub ReviewMsg_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Incoming Messages"
    End Sub
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
            objMessage = New Message()
            ' ''From_Message_Date.Text = Date.Today.Day & "/" & Date.Today.Month & "/" & Date.Today.Year
            ' ''To_Message_Date.Text = Date.Today.Day & "/" & Date.Today.Month & "/" & Date.Today.Year
            From_Message_Date.SelectedDate = Now.Date
            To_Message_Date.SelectedDate = Now.Date
            ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
            Try
                Dim UD_SUB_QRY = "select SalesRep_ID from dbo.app_GetControlInfo(" & CType(Session("User_Access"), UserAccess).UserID & ")"
                SalesRep_ID.Items.Clear()
                SalesRep_ID.ClearSelection()
                SalesRep_ID.Text = ""

                If CType(Session.Item("CONTROL_PARAMS"), ControlParams).MSG_MODULE_VERSION = "1" Then
                    Dim t As New DataTable
                    t = objMessage.GetSalesRepList(Err_No, Err_Desc, UD_SUB_QRY)
                    Dim dr As DataRow
                    dr = t.NewRow()
                    dr(0) = "0"
                    dr(1) = ""

                    t.Rows.InsertAt(dr, 0)
                    SalesRep_ID.DataSource = t

                    SalesRep_ID.DataValueField = "SalesRep_ID"
                    SalesRep_ID.DataTextField = "SalesRep_Name"
                    SalesRep_ID.DataBind()
                    SalesRep_ID.SelectedIndex = 0
                Else
                    Dim objApp As New AppControl

                    Dim t As New DataTable
                    t = objApp.LoadRecipientUsers(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)
                    Dim dr As DataRow
                    dr = t.NewRow()
                    dr(0) = "0"
                    dr(1) = ""
                    t.Rows.InsertAt(dr, 0)
                    SalesRep_ID.DataSource = t
                    SalesRep_ID.DataValueField = "UserID"
                    SalesRep_ID.DataTextField = "UserName"
                    SalesRep_ID.DataBind()
                    SalesRep_ID.SelectedIndex = 0
                End If

                ' SalesRep_ID.Items.Insert(0, New RadComboBoxItem("-- Select a Van --"))
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

    'Private Sub BindData()
    '    objMessage = New Message()
    '    Dim SearchQuery As String = ""
    '    Try


    '        If (From_Message_Date.SelectedDate Is Nothing And To_Message_Date.SelectedDate Is Nothing And SalesRep_ID.SelectedIndex <= 0) Then
    '            SearchQuery = ""
    '        ElseIf (From_Message_Date.SelectedDate Is Nothing And To_Message_Date.SelectedDate Is Nothing And SalesRep_ID.SelectedIndex > 0) Then
    '            SearchQuery = " And B.SalesRep_Name='" & SalesRep_ID.SelectedItem.Text & "'"
    '        ElseIf (Not From_Message_Date.SelectedDate Is Nothing And To_Message_Date.SelectedDate Is Nothing And SalesRep_ID.SelectedIndex <= 0) Then
    '            If CType(Session.Item("CONTROL_PARAMS"), ControlParams).MSG_MODULE_VERSION = 1 Then
    '                SearchQuery = "  and  A.Message_Date>='" & From_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 00:00:00' AND A.Message_Date<='" & From_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 23:59:59'"
    '            Else
    '                SearchQuery = "  and  A.Logged_At>='" & From_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 00:00:00' AND A.Logged_At<='" & From_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 23:59:59'"
    '            End If

    '        ElseIf (Not From_Message_Date.SelectedDate Is Nothing And Not To_Message_Date.SelectedDate Is Nothing And SalesRep_ID.SelectedIndex <= 0) Then
    '            If CType(Session.Item("CONTROL_PARAMS"), ControlParams).MSG_MODULE_VERSION = 1 Then
    '                SearchQuery = " and  A.Message_Date>='" & From_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 00:00:00' AND A.Message_Date<='" & To_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 23:59:59'"
    '            Else
    '                SearchQuery = " and  A.Logged_At>='" & From_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 00:00:00' AND A.Logged_At<='" & To_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 23:59:59'"
    '            End If
    '        ElseIf (Not From_Message_Date.SelectedDate Is Nothing And To_Message_Date.SelectedDate Is Nothing And SalesRep_ID.SelectedIndex > 0) Then
    '            SearchQuery = " And B.SalesRep_Name='" & SalesRep_ID.SelectedItem.Text & "' and  A.Message_Date>='" & From_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 00:00:00' AND A.Message_Date<='" & From_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 23:59:59'"
    '        ElseIf (Not From_Message_Date.SelectedDate Is Nothing And Not To_Message_Date.SelectedDate Is Nothing And SalesRep_ID.SelectedIndex > 0) Then
    '            SearchQuery = " And B.SalesRep_Name='" & SalesRep_ID.SelectedItem.Text & "' and  A.Message_Date>='" & From_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 00:00:00' AND A.Message_Date<='" & To_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 23:59:59'"
    '        ElseIf (From_Message_Date.SelectedDate Is Nothing = "" And Not To_Message_Date.SelectedDate Is Nothing And SalesRep_ID.SelectedIndex > 0) Then
    '            SearchQuery = " And B.SalesRep_Name='" & SalesRep_ID.SelectedItem.Text & "'"
    '        End If

    '        Dim RcptUID = CType(Session("User_Access"), UserAccess).UserID
    '        Dim dt As New DataTable
    '        dt = objMessage.GetSearchIncomingMessage(Err_No, Err_Desc, SearchQuery, RcptUID, CType(Session.Item("CONTROL_PARAMS"), ControlParams).MSG_MODULE_VERSION)
    '        Dim dv As New DataView(dt)
    '        If SortField <> "" Then
    '            dv.Sort = (SortField & " ") + SortDirection
    '        End If
    '        SearchResultGridView.DataSource = dv
    '        SearchResultGridView.DataBind()
    '        AddSortImage()
    '    Catch ex As Exception
    '        If Err_Desc IsNot Nothing Then
    '            log.Error(Err_Desc)
    '        Else
    '            log.Error(GetExceptionInfo(ex))
    '        End If
    '        Err_No = "74067"
    '        Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_IncomingMsg_002") & "&next=IncomingMsg.aspx&Title=Incoming Messages", False)
    '    End Try
    'End Sub
    Private Sub BindData()
        objMessage = New Message()
        Dim ds As New DataSet
        Dim SearchQuery As String

        Try
            If CType(Session.Item("CONTROL_PARAMS"), ControlParams).MSG_MODULE_VERSION = "1" Then
                If (From_Message_Date.SelectedDate Is Nothing And To_Message_Date.SelectedDate Is Nothing And SalesRep_ID.SelectedIndex <= 0) Then
                    SearchQuery = ""
                ElseIf (From_Message_Date.SelectedDate Is Nothing And To_Message_Date.SelectedDate Is Nothing And SalesRep_ID.SelectedIndex > 0) Then
                    SearchQuery = " And C.SalesRep_Name='" & SalesRep_ID.SelectedItem.Text & "'"
                ElseIf (Not From_Message_Date.SelectedDate Is Nothing And To_Message_Date.SelectedDate Is Nothing And SalesRep_ID.SelectedIndex <= 0) Then
                    '  Dim MsgDat As Array = From_Message_Date.Text.Split("/")
                    SearchQuery = "  and  A.Message_Date>='" & From_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 00:00:00' AND A.Message_Date<='" & From_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 23:59:59'"
                ElseIf (Not From_Message_Date.SelectedDate Is Nothing And Not To_Message_Date.SelectedDate Is Nothing And SalesRep_ID.SelectedIndex <= 0) Then
                    '   Dim MsgDat As Array = From_Message_Date.Text.Split("/")
                    '   Dim ToDat As Array = To_Message_Date.Text.Split("/")
                    SearchQuery = " and  A.Message_Date>='" & From_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 00:00:00' AND A.Message_Date<='" & To_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 23:59:59'"
                ElseIf (Not From_Message_Date.SelectedDate Is Nothing And To_Message_Date.SelectedDate Is Nothing And SalesRep_ID.SelectedIndex > 0) Then
                    ' Dim MsgDat As Array = From_Message_Date.Text.Split("/")
                    SearchQuery = " And C.SalesRep_Name='" & SalesRep_ID.SelectedItem.Text & "' and  A.Message_Date>='" & From_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 00:00:00' AND A.Message_Date<='" & From_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 23:59:59'"
                ElseIf (Not From_Message_Date.SelectedDate Is Nothing And Not To_Message_Date.SelectedDate Is Nothing And SalesRep_ID.SelectedIndex > 0) Then
                    '  Dim MsgDat As Array = From_Message_Date.Text.Split("/")
                    '   Dim ToDat As Array = To_Message_Date.Text.Split("/")
                    SearchQuery = " And C.SalesRep_Name='" & SalesRep_ID.SelectedItem.Text & "' and  A.Message_Date>='" & From_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 00:00:00' AND A.Message_Date<='" & To_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 23:59:59'"
                ElseIf (From_Message_Date.SelectedDate Is Nothing = "" And Not To_Message_Date.SelectedDate Is Nothing And SalesRep_ID.SelectedIndex > 0) Then
                    SearchQuery = " And C.SalesRep_Name='" & SalesRep_ID.SelectedItem.Text & "'"
                End If

                Dim UD_SUB_QRY = "select SalesRep_ID from dbo.app_GetControlInfo(" & CType(Session("User_Access"), UserAccess).UserID & ")"

                ds = objMessage.GetSearchMessage(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY)
                Dim dv As New DataView(ds.Tables("MSgTbl"))
                If SortField <> "" Then
                    dv.Sort = (SortField & " ") + SortDirection
                End If
                SearchResultGridView.DataSource = dv
                SearchResultGridView.DataBind()

            Else
                If (From_Message_Date.SelectedDate Is Nothing And To_Message_Date.SelectedDate Is Nothing And SalesRep_ID.SelectedIndex <= 0) Then
                    SearchQuery = ""
                ElseIf (From_Message_Date.SelectedDate Is Nothing And To_Message_Date.SelectedDate Is Nothing And SalesRep_ID.SelectedIndex > 0) Then
                    SearchQuery = " And C.User_ID='" & SalesRep_ID.SelectedValue & "'"
                ElseIf (Not From_Message_Date.SelectedDate Is Nothing And To_Message_Date.SelectedDate Is Nothing And SalesRep_ID.SelectedIndex <= 0) Then
                    '  Dim MsgDat As Array = From_Message_Date.Text.Split("/")
                    SearchQuery = "  and  A.Logged_At>='" & From_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 00:00:00' AND A.Logged_At<='" & From_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 23:59:59'"
                ElseIf (Not From_Message_Date.SelectedDate Is Nothing And Not To_Message_Date.SelectedDate Is Nothing And SalesRep_ID.SelectedIndex <= 0) Then
                    '   Dim MsgDat As Array = From_Message_Date.Text.Split("/")
                    '   Dim ToDat As Array = To_Message_Date.Text.Split("/")
                    SearchQuery = " and  A.Logged_At>='" & From_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 00:00:00' AND A.Logged_At<='" & To_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 23:59:59'"
                ElseIf (Not From_Message_Date.SelectedDate Is Nothing And To_Message_Date.SelectedDate Is Nothing And SalesRep_ID.SelectedIndex > 0) Then
                    ' Dim MsgDat As Array = From_Message_Date.Text.Split("/")
                    SearchQuery = " And C.User_ID='" & SalesRep_ID.SelectedValue & "' and  A.Logged_At>='" & From_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 00:00:00' AND A.Logged_At<='" & From_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 23:59:59'"
                ElseIf (Not From_Message_Date.SelectedDate Is Nothing And Not To_Message_Date.SelectedDate Is Nothing And SalesRep_ID.SelectedIndex > 0) Then
                    '  Dim MsgDat As Array = From_Message_Date.Text.Split("/")
                    '   Dim ToDat As Array = To_Message_Date.Text.Split("/")
                    SearchQuery = " And C.User_ID='" & SalesRep_ID.SelectedValue & "' and  A.Logged_At>='" & From_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 00:00:00' AND A.Logged_At<='" & To_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 23:59:59'"
                ElseIf (From_Message_Date.SelectedDate Is Nothing = "" And Not To_Message_Date.SelectedDate Is Nothing And SalesRep_ID.SelectedIndex > 0) Then
                    SearchQuery = " And C.User_ID='" & SalesRep_ID.SelectedValue & "'"
                End If

                Dim UD_SUB_QRY = "SELECT DISTINCT USER_ID  FROM dbo.app_GetSubUsers(" & CType(Session("User_Access"), UserAccess).UserID & ")"

                ds = objMessage.GetSearchMessageByFSR(Err_No, Err_Desc, SearchQuery, CType(Session("User_Access"), UserAccess).UserID)

                Dim dv As New DataView(ds.Tables("MSgOutbox"))
                If SortField <> "" Then
                    dv.Sort = (SortField & " ") + SortDirection
                End If

                ds = objMessage.GetSearchMessageFromFSR(Err_No, Err_Desc, SearchQuery, CType(Session("User_Access"), UserAccess).UserID)
                log.Debug(SearchQuery)
                log.Debug(ds.Tables("MSgInbox").Rows.Count)
                Dim dv1 As New DataView(ds.Tables("MSgInbox"))
                If SortField <> "" Then
                    dv1.Sort = (SortField & " ") + SortDirection
                End If
                SearchResultGridView.DataSource = dv1
                SearchResultGridView.DataBind()

            End If


        Catch ex As Exception

            If Err_Desc IsNot Nothing Then
                log.Error(Err_Desc)
            Else
                log.Error(GetExceptionInfo(ex))
            End If
            Err_No = "74067"
            log.Debug(ex.ToString)
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

    Protected Sub NewMsgBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles NewMsgBtn.Click
        Response.Redirect("AdminMsg.aspx?NewMsg=1")
    End Sub


End Class