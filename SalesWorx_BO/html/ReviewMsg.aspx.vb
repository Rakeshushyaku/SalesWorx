Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports Telerik.Web.UI
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

    Private Property SortField1() As String
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
            From_Message_Date.SelectedDate = Now.Date
            To_Message_Date.SelectedDate = Now.Date
            ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
            Try
                Dim UD_SUB_QRY = "select SalesRep_ID from dbo.app_GetControlInfo(" & CType(Session("User_Access"), UserAccess).UserID & ")"
                SalesRep_ID.Items.Clear()
                SalesRep_ID.ClearSelection()
                SalesRep_ID.Text = ""




                If CType(Session.Item("CONTROL_PARAMS"), ControlParams).MSG_MODULE_VERSION = "1" Then

                    SalesRep_ID.DataSource = objMessage.GetSalesRepList(Err_No, Err_Desc, UD_SUB_QRY)

                    SalesRep_ID.DataValueField = "SalesRep_ID"
                    SalesRep_ID.DataTextField = "SalesRep_Name"
                    SalesRep_ID.DataBind()
                    SalesRep_ID.SelectedIndex = 0
                    SalesRep_ID.Items.Insert(0, New RadComboBoxItem("Select FSR", "0"))
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





                If CType(Session.Item("CONTROL_PARAMS"), ControlParams).MSG_MODULE_VERSION = "1" Then
                    SearchResultGridView.Columns(4).Visible = False
                    SearchResultGridView.Visible = True
                    MessageTab.Visible = False
                Else
                    MessageTab.Visible = True
                    MessageTab.Tabs(0).Selected = True
                    RadMultiPage2.SelectedIndex = 0
                    SearchResultGridView.Visible = False
                    SearchResultGridView.Columns(4).Visible = True
                End If








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
                MessageTab.Visible = False

            Else
                If (From_Message_Date.SelectedDate Is Nothing And To_Message_Date.SelectedDate Is Nothing And SalesRep_ID.SelectedIndex <= 0) Then
                    SearchQuery = ""
                ElseIf (From_Message_Date.SelectedDate Is Nothing And To_Message_Date.SelectedDate Is Nothing And SalesRep_ID.SelectedIndex > 0) Then
                    SearchQuery = " And A.Sender_ID='" & SalesRep_ID.SelectedValue & "'"
                ElseIf (Not From_Message_Date.SelectedDate Is Nothing And To_Message_Date.SelectedDate Is Nothing And SalesRep_ID.SelectedIndex <= 0) Then
                    '  Dim MsgDat As Array = From_Message_Date.Text.Split("/")
                    SearchQuery = "  and  A.Logged_At>='" & From_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 00:00:00' AND A.Logged_At<='" & From_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 23:59:59'"
                ElseIf (Not From_Message_Date.SelectedDate Is Nothing And Not To_Message_Date.SelectedDate Is Nothing And SalesRep_ID.SelectedIndex <= 0) Then
                    '   Dim MsgDat As Array = From_Message_Date.Text.Split("/")
                    '   Dim ToDat As Array = To_Message_Date.Text.Split("/")
                    SearchQuery = " and  A.Logged_At>='" & From_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 00:00:00' AND A.Logged_At<='" & To_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 23:59:59'"
                ElseIf (Not From_Message_Date.SelectedDate Is Nothing And To_Message_Date.SelectedDate Is Nothing And SalesRep_ID.SelectedIndex > 0) Then
                    ' Dim MsgDat As Array = From_Message_Date.Text.Split("/")
                    SearchQuery = " And A.Sender_ID='" & SalesRep_ID.SelectedValue & "' and  A.Logged_At>='" & From_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 00:00:00' AND A.Logged_At<='" & From_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 23:59:59'"
                ElseIf (Not From_Message_Date.SelectedDate Is Nothing And Not To_Message_Date.SelectedDate Is Nothing And SalesRep_ID.SelectedIndex > 0) Then
                    '  Dim MsgDat As Array = From_Message_Date.Text.Split("/")
                    '   Dim ToDat As Array = To_Message_Date.Text.Split("/")
                    SearchQuery = " And A.Sender_ID='" & SalesRep_ID.SelectedValue & "' and  A.Logged_At>='" & From_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 00:00:00' AND A.Logged_At<='" & To_Message_Date.SelectedDate.Value.ToString("MM-dd-yyyy") & " 23:59:59'"
                ElseIf (From_Message_Date.SelectedDate Is Nothing = "" And Not To_Message_Date.SelectedDate Is Nothing And SalesRep_ID.SelectedIndex > 0) Then
                    SearchQuery = " And A.Sender_ID='" & SalesRep_ID.SelectedValue & "'"
                End If


                Dim UD_SUB_QRY = "SELECT DISTINCT USER_ID  FROM dbo.app_GetSubUsers(" & CType(Session("User_Access"), UserAccess).UserID & ")"

                ds = objMessage.GetSearchMessageByFSR_v2(Err_No, Err_Desc, SearchQuery, CType(Session("User_Access"), UserAccess).UserID)

                Dim dv As New DataView(ds.Tables("MSgOutbox"))
                If SortField <> "" Then
                    dv.Sort = (SortField & " ") + SortDirection
                End If
                gvOutBox.DataSource = dv
                gvOutBox.DataBind()

                MessageTab.Visible = True

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
                

                ds = objMessage.GetSearchMessageFromFSR_v2(Err_No, Err_Desc, SearchQuery, CType(Session("User_Access"), UserAccess).UserID)

                Dim dv1 As New DataView(ds.Tables("MSgInbox"))
                If SortField1 <> "" Then
                    dv1.Sort = (SortField1 & " ") + SortDirection1
                End If
                gvInbox.DataSource = dv1
                gvInbox.DataBind()



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
        End Try
    End Sub

    Private Property SortDirection1() As String
        Get
            If ViewState("SortDirection1") Is Nothing Then
                ViewState("SortDirection1") = "ASC"
            End If
            Return ViewState("SortDirection1").ToString()
        End Get
        Set(ByVal value As String)
            Dim s As String = SortDirection1

            If value = "flip" Then
                s = If(s = "ASC", "DESC", "ASC")
            Else
                s = value
            End If

            ViewState("SortDirection1") = s
        End Set
    End Property
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

    Protected Sub gvOutBox_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvOutBox.PageIndexChanging
        gvOutBox.PageIndex = e.NewPageIndex
        BindData()
    End Sub

    Private Sub gvOutBox_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvOutBox.Sorting
        SortField = e.SortExpression
        SortDirection = "flip"
        BindData()
    End Sub

    Protected Sub gvInbox_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvInbox.PageIndexChanging
        gvOutBox.PageIndex = e.NewPageIndex
        BindData()
    End Sub

    Private Sub gvInbox_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvInbox.Sorting
        SortField1 = e.SortExpression
        SortDirection1 = "flip"
        BindData()
    End Sub

End Class