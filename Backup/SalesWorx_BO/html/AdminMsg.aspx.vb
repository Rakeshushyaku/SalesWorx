Imports SalesWorx.BO.Common
Imports System.Web.UI.WebControls
Imports System.Resources
Imports System.Configuration
Imports log4net

Partial Public Class AdminMsg
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim objMessage As Message
    Private Const PageID As String = "P70"
    Private mExpiryDate As Integer = CInt(ConfigurationSettings.AppSettings("MESSAGE_EXPIRY_DAYS"))
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objLogin As New SalesWorx.BO.Common.Login
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
            MSG_Date.Text = Date.Today.Day & "/" & Date.Today.Month & "/" & Date.Today.Year
        End If
    End Sub
    Private Sub AdminMsg_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Messages"
        If Request.QueryString("NewMsg") = 1 Then
            TopPanel.Visible = False
            MsgSelectPanel.Visible = False
            NewMsgPanel.Visible = True
            RecallBtn.Visible = False
            BackBtn.Visible = False
            SelDateCalendar.SelectedDate = Today.ToString()
            ExpiryDateCalendar.SelectedDate = IIf(mExpiryDate <> 0, Today.AddDays(mExpiryDate - 1), 0)
            MsgTxt.Text = ""
            TitleTxt.Text = ""
            BindSalesRepList()
            ButtonStatus.Value = "New Msg"
        End If
    End Sub
    Protected Sub GetMsgBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles GetMsgBtn.Click
        objMessage = New Message()
        MsgSelectPanel.Visible = True
        ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
        Try

            Dim UD_SUB_QRY = "select SalesRep_ID from dbo.app_GetControlInfo(" & CType(Session("User_Access"), UserAccess).UserID & ")"
            Dim TemDateStr As String = MSG_Date.Text
            Dim DateArr As Array = TemDateStr.Split("/")
            objMessage.MsgDateProp = DateArr(1) & "/" & DateArr(0) & "/" & DateArr(2)
            MessageSelDD.DataSource = objMessage.GetAllMessagesByUD(Err_No, Err_Desc, UD_SUB_QRY)
            MessageSelDD.DataBind()
            MessageSelDD.Items.Insert(0, New ListItem("-- Select a Message --"))
            If Not Err_Desc Is Nothing Then
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=AdminMsg.aspx&Title=Message", False)
            End If
        Catch ex As Exception
            Err_No = "74062"
            Err_Desc = ex.Message
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=AdminMsg.aspx&Title=Message", False)
        Finally
            objMessage = Nothing
        End Try
    End Sub

    Protected Sub ViewandRecallBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ViewandRecallBtn.Click
        TopPanel.Visible = False
        MsgSelectPanel.Visible = False
        SendMsgBtn.Visible = False
        CancelBtn.Visible = False
        RecallBtn.Visible = True
        NewMsgPanel.Visible = True
        BackBtn.Visible = True
        BindSalesRepList()
        MakeEnabled(False)
        GetValues()
    End Sub
    Private Sub GetValues()
        objMessage = New Message()
        Try
            objMessage.MessageID = MessageSelDD.SelectedItem.Value
            objMessage.GetMessage(Err_No, Err_Desc)
            If Err_Desc Is Nothing Then
                SelDateCalendar.SelectedDate = objMessage.MessageDate
                ExpiryDateCalendar.SelectedDate = objMessage.MessageExpiryDate
                TitleTxt.Text = objMessage.MessageTitle
                MsgTxt.Text = objMessage.MessageContent

                If StrComp(TitleTxt.Text, "Comments", vbTextCompare) = 0 Or StrComp(TitleTxt.Text, "Approved", vbTextCompare) = 0 Then
                    RecallBtn.Enabled = False
                Else
                    RecallBtn.Enabled = True
                End If

                Dim TemList As String = objMessage.SalesRepID

                For Each li As ListItem In SalesRepList.Items
                    If TemList.Contains(li.Value) Then
                        li.Selected = True
                    End If
                Next
            Else
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=AdminMsg.aspx&Title=Message", False)
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        Finally
            objMessage = Nothing
        End Try
    End Sub
    Private Sub MakeEnabled(ByVal Status As Boolean)
        Sel_Date.Enabled = Status
        Expiry_Date.Enabled = Status
        TitleTxt.Enabled = Status
        MsgTxt.Enabled = Status
    End Sub
    Protected Sub NewMsgBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles NewMsgBtn.Click
        TopPanel.Visible = False
        MsgSelectPanel.Visible = False
        NewMsgPanel.Visible = True
        RecallBtn.Visible = False
        BackBtn.Visible = False
        SelDateCalendar.SelectedDate = Today.ToString()
        ExpiryDateCalendar.SelectedDate = IIf(mExpiryDate <> 0, Today.AddDays(mExpiryDate - 1), 0)
        MsgTxt.Text = ""
        TitleTxt.Text = ""
        BindSalesRepList()
        ButtonStatus.Value = "New Msg"

    End Sub
    Private Sub BindSalesRepList()
        objMessage = New Message()
        Try
            Dim UD_SUB_QRY = "select SalesRep_ID from dbo.app_GetControlInfo(" & CType(Session("User_Access"), UserAccess).UserID & ")"
            SalesRepList.DataSource = objMessage.GetSalesRepList(Err_No, Err_Desc, UD_SUB_QRY)
            SalesRepList.DataTextField = "SalesRep_Name"
            SalesRepList.DataValueField = "SalesRep_ID"
            SalesRepList.DataBind()
            If Not Err_Desc Is Nothing Then
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=AdminMsg.aspx&Title=Message", False)
            End If
        Catch ex As Exception
            Err_No = "74064"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=AdminMsg.aspx&Title=Message", False)
        Finally
            objMessage = Nothing
        End Try
    End Sub

    Protected Sub CancelBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CancelBtn.Click
        TopPanel.Visible = True
        MsgSelectPanel.Visible = False
        NewMsgPanel.Visible = False
        MsgLbl.Text = ""
        MSG_Date.Text = Date.Today.Day & "/" & Date.Today.Month & "/" & Date.Today.Year
    End Sub

    Protected Sub SendMsgBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SendMsgBtn.Click
        If Page.IsValid Then
            objMessage = New Message()
            Try
                objMessage.MessageTitle = TitleTxt.Text
                objMessage.MessageContent = MsgTxt.Text
                Dim sDate As Array = Sel_Date.Text.Split("/")
                objMessage.MessageDate = CDate(sDate(1) & "/" & sDate(0) & "/" & sDate(2))
                objMessage.SenderIDProp = CType(Session("USER_ACCESS"), UserAccess).SalesRepID
                Dim SalesRep_List As String
                Dim ICount As Integer = 0
                For Each li As ListItem In SalesRepList.Items
                    If li.Selected = True Then
                        SalesRep_List += li.Value & ","
                        ICount += 1
                    End If
                Next

                objMessage.SalesRepID = SalesRep_List
                Dim eDate As Array = Expiry_Date.Text.Split("/")
                objMessage.MessageExpiryDate = CDate(eDate(1) & "/" & eDate(0) & "/" & eDate(2))
                If objMessage.SendMessage(Err_No, Err_Desc) Then
                    objLogin.SaveUserLog(Err_No, Err_Desc, "I", "CALL PLANNING", "SEND MESSAGE", TitleTxt.Text, "Message: " & MsgTxt.Text & "/ Start Date :  " & CStr(CDate(sDate(1) & "/" & sDate(0) & "/" & sDate(2))) & "/ End Date :  " & CDate(eDate(1) & "/" & eDate(0) & "/" & eDate(2)) & "/ Assigned Sales Rep : " & ICount, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                    MsgLbl.Text = "<span class='message'>Message Sent Successfully</span>"
                    TitleTxt.Text = ""
                    MsgTxt.Text = ""
                    For Each li As ListItem In SalesRepList.Items
                        li.Selected = False
                    Next
                Else
                    log.Error(Err_Desc)
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=AdminMsg.aspx&Title=Message", False)
                    Exit Try
                End If

            Catch ex As Exception
                If Err_Desc Is Nothing Then
                    log.Error(GetExceptionInfo(ex))
                Else
                    log.Error(Err_Desc)
                End If
            Finally
                objMessage = Nothing
            End Try
        End If
    End Sub

    Protected Sub RecallBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles RecallBtn.Click
        RecallBtn.Visible = False
        SendMsgBtn.Visible = True
        CancelBtn.Visible = True
        BackBtn.Visible = False
        MakeEnabled(True)
    End Sub

    Protected Sub MsgTxt_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles MsgTxt.TextChanged

    End Sub
End Class