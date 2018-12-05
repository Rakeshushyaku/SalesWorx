Imports SalesWorx.BO.Common
Imports System.Web.UI.WebControls
Imports System.Resources
Imports System.Configuration
Imports log4net
Imports Telerik.Web.UI

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
            MSG_Date.SelectedDate = Date.Today.Month & "/" & Date.Today.Day & "/" & Date.Today.Year
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
            Sel_Date.SelectedDate = Today
            Expiry_Date.SelectedDate = IIf(mExpiryDate <> 0, Today.AddDays(mExpiryDate - 1), 0)
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

            Dim UD_SUB_QRY As String = ""
            If CType(Session.Item("CONTROL_PARAMS"), ControlParams).MSG_MODULE_VERSION = "1" Then
                UD_SUB_QRY = "select SalesRep_ID from dbo.app_GetControlInfo(" & CType(Session("User_Access"), UserAccess).UserID & ")"
            Else
                UD_SUB_QRY = "SELECT DISTINCT USER_ID  FROM dbo.app_GetSubUsers(" & CType(Session("User_Access"), UserAccess).UserID & ")"
            End If
            log.Debug(UD_SUB_QRY)
            ' ''Dim TemDateStr As String = MSG_Date.SelectedDate
            ' ''Dim DateArr As Array = TemDateStr.Split("/")
            objMessage.MsgDateProp = MSG_Date.SelectedDate
            MessageSelDD.Items.Clear()
            MessageSelDD.ClearSelection()
            MessageSelDD.Text = ""
            MessageSelDD.DataSource = objMessage.GetAllMessagesByUD(Err_No, Err_Desc, UD_SUB_QRY, CType(Session.Item("CONTROL_PARAMS"), ControlParams).MSG_MODULE_VERSION)
            MessageSelDD.DataBind()
            ' ''MessageSelDD.Items.Insert(0, New RadComboBoxItem("-- Select a Message --"))
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
        If Me.MessageSelDD.SelectedIndex <= 0 Then
            MessageBoxValidation("Please select a message to recall", "Validation")
            Exit Sub
        End If
        TopPanel.Visible = False
        MsgSelectPanel.Visible = False
        SendMsgBtn.Visible = False
        CancelBtn.Visible = False
        RecallBtn.Visible = True
        NewMsgPanel.Visible = True
        BackBtn.Visible = True
        Rdo_userType.Visible = False
        BindSalesRepListForReview()
        MakeEnabled(False)
        If CType(Session.Item("CONTROL_PARAMS"), ControlParams).MSG_MODULE_VERSION = "1" Then
            GetValues()
        Else
            GetValuesByFSR()
        End If
    End Sub
    Private Sub GetValuesByFSR()

        SalesRepList.SelectedValue = Nothing
        For Each itm As RadComboBoxItem In SalesRepList.Items
            itm.Checked = False
        Next

        objMessage = New Message()
        Try

            objMessage.GetMessageForReview(Err_No, Err_Desc, MessageSelDD.SelectedItem.Value)
            If Err_Desc Is Nothing Then
                Sel_Date.SelectedDate = objMessage.MessageDate
                Expiry_Date.SelectedDate = IIf(objMessage.MessageExpiryDate = "1/1/1900", objMessage.MessageDate, objMessage.MessageExpiryDate)
                TitleTxt.Text = objMessage.MessageTitle
                MsgTxt.Text = objMessage.MessageContent

                Dim msgStatus As String = objMessage.MessageStatus

                If StrComp(TitleTxt.Text, "Comments", vbTextCompare) = 0 Or StrComp(TitleTxt.Text, "Approved", vbTextCompare) = 0 Or msgStatus = "Y" Then
                    RecallBtn.Enabled = False
                Else
                    RecallBtn.Enabled = True
                End If

                Dim TemList As String = objMessage.SalesRepID
                Dim s() As String = TemList.Split(",")
                For i As Integer = 0 To s.Length - 1
                    For Each li As RadComboBoxItem In SalesRepList.Items
                        If s(i) = li.Value Then
                            li.Checked = True
                        End If
                    Next
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
    Private Sub GetValues()

        SalesRepList.SelectedValue = Nothing
        For Each itm As RadComboBoxItem In SalesRepList.Items
            itm.Checked = False
        Next

        objMessage = New Message()
        Try
            objMessage.MessageID = MessageSelDD.SelectedItem.Value
            objMessage.GetMessage(Err_No, Err_Desc)
            If Err_Desc Is Nothing Then
                Sel_Date.SelectedDate = objMessage.MessageDate
                Expiry_Date.SelectedDate = IIf(objMessage.MessageExpiryDate = "1/1/1900", objMessage.MessageDate, objMessage.MessageExpiryDate)
                TitleTxt.Text = objMessage.MessageTitle
                MsgTxt.Text = objMessage.MessageContent

                If StrComp(TitleTxt.Text, "Comments", vbTextCompare) = 0 Or StrComp(TitleTxt.Text, "Approved", vbTextCompare) = 0 Then
                    RecallBtn.Enabled = False
                Else
                    RecallBtn.Enabled = True
                End If

                Dim TemList As String = objMessage.SalesRepID
                Dim s() As String = TemList.Split(",")
                For i As Integer = 0 To s.Length - 1
                    For Each li As RadComboBoxItem In SalesRepList.Items
                        If s(i) = li.Value Then
                            li.Checked = True
                        End If
                    Next
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
    'Private Sub GetValues()
    '    objMessage = New Message()
    '    Try
    '        objMessage.MessageID = MessageSelDD.SelectedItem.Value
    '        objMessage.GetMessage(Err_No, Err_Desc)
    '        If Err_Desc Is Nothing Then
    '            Sel_Date.SelectedDate = objMessage.MessageDate
    '            Expiry_Date.SelectedDate = IIf(objMessage.MessageExpiryDate = "1/1/1900", objMessage.MessageDate, objMessage.MessageExpiryDate)
    '            TitleTxt.Text = objMessage.MessageTitle
    '            MsgTxt.Text = objMessage.MessageContent

    '            If StrComp(TitleTxt.Text, "Comments", vbTextCompare) = 0 Or StrComp(TitleTxt.Text, "Approved", vbTextCompare) = 0 Then
    '                RecallBtn.Enabled = False
    '            Else
    '                RecallBtn.Enabled = True
    '            End If

    '            Dim TemList As String = objMessage.SalesRepID

    '            Dim IsAllChecked As Boolean = True

    '            For Each li As RadComboBoxItem In SalesRepList.Items
    '                If TemList.Contains(li.Value) And li.Value <> "0" Then
    '                    li.Checked = True
    '                ElseIf li.Value <> "0" Then
    '                    IsAllChecked = False
    '                End If
    '            Next

    '            If IsAllChecked Then
    '                SalesRepList.Items(0).Checked = True
    '            End If
    '        Else
    '            log.Error(Err_Desc)
    '            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=AdminMsg.aspx&Title=Message", False)
    '        End If
    '    Catch ex As Exception
    '        log.Error(GetExceptionInfo(ex))
    '    Finally
    '        objMessage = Nothing
    '    End Try
    'End Sub
    Private Sub MakeEnabled(ByVal Status As Boolean)
        Sel_Date.Enabled = Status
        Expiry_Date.Enabled = Status
        TitleTxt.Enabled = Status
        MsgTxt.Enabled = Status
        SalesRepList.Enabled = Status
    End Sub
    Protected Sub NewMsgBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles NewMsgBtn.Click
        TopPanel.Visible = False
        MsgSelectPanel.Visible = False
        NewMsgPanel.Visible = True
        RecallBtn.Visible = False
        BackBtn.Visible = False
        MakeEnabled(True)
        SendMsgBtn.Visible = True
        CancelBtn.Visible = True
        Sel_Date.SelectedDate = Today.ToString()
        Expiry_Date.SelectedDate = IIf(mExpiryDate <> 0, Today.AddDays(mExpiryDate - 1), 0)
        MsgTxt.Text = ""
        TitleTxt.Text = ""
        SalesRepList.SelectedValue = Nothing
        For Each itm As RadComboBoxItem In SalesRepList.Items
            itm.Checked = False
        Next
        If CType(Session.Item("CONTROL_PARAMS"), ControlParams).MSG_MODULE_VERSION = "1" Then
            Rdo_userType.Visible = False
        Else
            Rdo_userType.Visible = True
        End If

        BindSalesRepList()

        ButtonStatus.Value = "New Msg"

    End Sub
    Private Sub BindSalesRepListForReview()
        objMessage = New Message()
        Try
            Dim UD_SUB_QRY = "select SalesRep_ID from dbo.app_GetControlInfo(" & CType(Session("User_Access"), UserAccess).UserID & ")"

            Dim t As New DataTable
            If CType(Session.Item("CONTROL_PARAMS"), ControlParams).MSG_MODULE_VERSION = "1" Then
                t = objMessage.GetSalesRepList(Err_No, Err_Desc, UD_SUB_QRY)
                SalesRepList.Items.Clear()
                SalesRepList.DataSource = t
                SalesRepList.DataTextField = "SalesRep_Name"
                SalesRepList.DataValueField = "SalesRep_ID"
                SalesRepList.DataBind()
                For Each itm As RadComboBoxItem In SalesRepList.Items
                    itm.Checked = False
                Next
            Else
                Dim objApp As New AppControl
                t = objApp.LoadRecipientsforReview(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)
                SalesRepList.Items.Clear()
                SalesRepList.DataSource = t
                SalesRepList.DataTextField = "UserName"
                SalesRepList.DataValueField = "UserID"
                SalesRepList.DataBind()
                For Each itm As RadComboBoxItem In SalesRepList.Items
                    itm.Checked = False
                Next
            End If
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
    Private Sub BindSalesRepList()
        objMessage = New Message()
        Try
            Dim UD_SUB_QRY = "select SalesRep_ID from dbo.app_GetControlInfo(" & CType(Session("User_Access"), UserAccess).UserID & ")"

            Dim t As New DataTable
            If CType(Session.Item("CONTROL_PARAMS"), ControlParams).MSG_MODULE_VERSION = "1" Then
                t = objMessage.GetSalesRepList(Err_No, Err_Desc, UD_SUB_QRY)
                SalesRepList.Items.Clear()
                SalesRepList.DataSource = t
                SalesRepList.DataTextField = "SalesRep_Name"
                SalesRepList.DataValueField = "SalesRep_ID"
                SalesRepList.DataBind()
                For Each itm As RadComboBoxItem In SalesRepList.Items
                    itm.Checked = False
                Next
            Else
                If Rdo_userType.SelectedItem.Value = "V" Then
                    Dim objApp As New AppControl
                    t = objMessage.LoadMSgRecipients(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)
                    SalesRepList.Items.Clear()
                    SalesRepList.DataSource = t
                    SalesRepList.DataTextField = "UserName"
                    SalesRepList.DataValueField = "UserID"
                    SalesRepList.DataBind()
                    For Each itm As RadComboBoxItem In SalesRepList.Items
                        itm.Checked = False
                    Next
                Else
                    Dim objLogin As New SalesWorx.BO.Common.Login
                    t = objLogin.GetuserGroups(Err_No, Err_Desc, "")
                    SalesRepList.Items.Clear()
                    SalesRepList.DataSource = t
                    SalesRepList.DataTextField = "Group_Name"
                    SalesRepList.DataValueField = "UG_ID"
                    SalesRepList.DataBind()
                    For Each itm As RadComboBoxItem In SalesRepList.Items
                        itm.Checked = False
                    Next
                End If
            End If


            'SalesRepList.Items.Clear()
            'SalesRepList.DataSource = t
            'SalesRepList.DataTextField = "SalesRep_Name"
            'SalesRepList.DataValueField = "SalesRep_ID"
            'SalesRepList.DataBind()

            'For Each itm As RadComboBoxItem In SalesRepList.Items
            '    itm.Checked = False
            'Next

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
    'Private Sub BindSalesRepList()
    '    objMessage = New Message()
    '    Try
    '        Dim UD_SUB_QRY = "select SalesRep_ID from dbo.app_GetControlInfo(" & CType(Session("User_Access"), UserAccess).UserID & ")"



    '        Dim dt As New DataTable
    '        dt = objMessage.GetSalesRepList(Err_No, Err_Desc, UD_SUB_QRY)
    '        Dim dr As DataRow
    '        dr = dt.NewRow()
    '        dr(0) = "0"
    '        dr(1) = "All"
    '        dt.Rows.InsertAt(dr, 0)

    '        SalesRepList.DataSource = dt
    '        SalesRepList.DataTextField = "SalesRep_Name"
    '        SalesRepList.DataValueField = "SalesRep_ID"
    '        SalesRepList.DataBind()
    '        If Not Err_Desc Is Nothing Then
    '            log.Error(Err_Desc)
    '            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=AdminMsg.aspx&Title=Message", False)
    '        End If
    '    Catch ex As Exception
    '        Err_No = "74064"
    '        If Err_Desc Is Nothing Then
    '            log.Error(GetExceptionInfo(ex))
    '        Else
    '            log.Error(Err_Desc)
    '        End If
    '        Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=AdminMsg.aspx&Title=Message", False)
    '    Finally
    '        objMessage = Nothing
    '    End Try
    'End Sub

    Protected Sub CancelBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CancelBtn.Click
        TopPanel.Visible = True
        MsgSelectPanel.Visible = False
        NewMsgPanel.Visible = False
        MsgLbl.Text = ""
        SendMsgBtn.Visible = True
        SendMsgBtn_Recall.Visible = False
        MSG_Date.SelectedDate = Date.Today.Month & "/" & Date.Today.Day & "/" & Date.Today.Year
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        str = str.Replace("'", "&#39;")
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub SendMsgBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SendMsgBtn.Click
        'If Page.IsValid Then
        '    objMessage = New Message()
        '    Try
        '        objMessage.MessageTitle = TitleTxt.Text
        '        objMessage.MessageContent = MsgTxt.Text
        '        ''Dim sDate As Array = Sel_Date.Text.Split("/")
        '        If CDate(Sel_Date.SelectedDate).ToString("dd/MM/yyyy") = Now.ToString("dd/MM/yyyy") Then
        '            objMessage.MessageDate = Now
        '        Else
        '            objMessage.MessageDate = Sel_Date.SelectedDate
        '        End If
        '        objMessage.SenderIDProp = CType(Session("USER_ACCESS"), UserAccess).UserID
        '        Dim SalesRep_List As String
        '        Dim ICount As Integer = 0
        '        For Each li As RadComboBoxItem In SalesRepList.Items
        '            If li.Checked = True And li.Value.ToString() <> "0" Then
        '                SalesRep_List += li.Value & ","
        '                ICount += 1
        '            End If
        '        Next

        '        If DateTime.Parse(Me.Expiry_Date.SelectedDate) <= DateTime.Parse(Me.Sel_Date.SelectedDate) Then
        '            MessageBoxValidation("Expiry date should be greater than to start date.", "Validation")
        '            Exit Sub
        '        End If
        '        If SalesRep_List Is Nothing Then
        '            MessageBoxValidation("Please select at least one sales rep", "Validation")
        '            Exit Sub
        '        End If

        '        objMessage.SalesRepID = SalesRep_List
        '        ''Dim eDate As Array = Expiry_Date.Text.Split("/")
        '        objMessage.MessageExpiryDate = Expiry_Date.SelectedDate
        '        If objMessage.SendMessage(Err_No, Err_Desc) Then
        '            objLogin.SaveUserLog(Err_No, Err_Desc, "I", "CALL PLANNING", "SEND MESSAGE", TitleTxt.Text, "Message: " & MsgTxt.Text & "/ Start Date :  " & Sel_Date.SelectedDate & "/ End Date :  " & Expiry_Date.SelectedDate & "/ Assigned Sales Rep : " & ICount, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
        '            '' MsgLbl.Text = "<span class='message'>Message Sent Successfully</span>"
        '            MessageBoxValidation("Message Sent Successfully.", "Information")
        '            TitleTxt.Text = ""
        '            MsgTxt.Text = ""
        '            For Each li As RadComboBoxItem In SalesRepList.Items
        '                li.Checked = False
        '            Next
        '        Else
        '            log.Error(Err_Desc)
        '            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=AdminMsg.aspx&Title=Message", False)
        '            Exit Try
        '        End If

        '    Catch ex As Exception
        '        If Err_Desc Is Nothing Then
        '            log.Error(GetExceptionInfo(ex))
        '        Else
        '            log.Error(Err_Desc)
        '        End If
        '    Finally
        '        objMessage = Nothing
        '    End Try
        'End If

        If TitleTxt.Text.Trim = "" Then
            MessageBoxValidation("Please Enter a valid Message Ttile.", "Validation")
            Exit Sub
        End If
        If MsgTxt.Text.Trim = "" Then
            MessageBoxValidation("Please Enter a valid Message Text.", "Validation")
            Exit Sub
        End If

        objMessage = New Message()
        Try
            objMessage.MessageTitle = TitleTxt.Text
            objMessage.MessageContent = MsgTxt.Text
            ' Dim sDate As Array = Sel_Date.SelectedDate
            objMessage.MessageDate = Sel_Date.SelectedDate
            objMessage.SenderIDProp = CType(Session("USER_ACCESS"), UserAccess).SalesRepID
            Dim SalesRep_List As String = Nothing
            For Each li As RadComboBoxItem In SalesRepList.Items
                If li.Checked = True Then
                    SalesRep_List += li.Value & ","
                End If
            Next




            If DateTime.Parse(Me.Expiry_Date.SelectedDate) <= DateTime.Parse(Me.Sel_Date.SelectedDate) Then
                MessageBoxValidation("Expiry date should be greater than to start date.", "Validation")
                Exit Sub
            End If
            If Rdo_userType.SelectedItem.Value = "V" Then
                If SalesRep_List Is Nothing Then
                    MessageBoxValidation("Please select at least one sales rep", "Validation")
                    Exit Sub
                End If
            Else
                If SalesRep_List Is Nothing Then
                    MessageBoxValidation("Please select at least one user group", "Validation")
                    Exit Sub
                End If
            End If
            objMessage.SalesRepID = SalesRep_List
            '  Dim eDate As Array = Expiry_Date.Text.Split("/")
            objMessage.MessageExpiryDate = Expiry_Date.SelectedDate
            Dim SenderName As String = (New SalesWorx.BO.Common.User).GetUserName(Session("USER_ACCESS").UserID).ToString
            Dim SenderID As String = CType(Session("User_Access"), UserAccess).UserID
            objMessage.Sender_ID = SenderID

            If CType(Session.Item("CONTROL_PARAMS"), ControlParams).MSG_MODULE_VERSION = "1" Then
                If objMessage.SendMessage(Err_No, Err_Desc) Then
                    '  MsgLbl.Text = "<span class='message'>Message Sent Successfully</span>"
                    MessageBoxValidation("Message Sent Successfully", "Information")
                    TitleTxt.Text = ""
                    MsgTxt.Text = ""
                    For Each li As RadComboBoxItem In SalesRepList.Items
                        li.Checked = False
                    Next
                Else
                    log.Error(Err_Desc)
                    MessageBoxValidation("Error while sending message", "Information")
                    Exit Sub
                End If
            Else
                If Rdo_userType.SelectedItem.Value = "V" Then
                    If objMessage.RecallMsg(Err_No, Err_Desc, SenderID, SenderName) Then
                        '  MsgLbl.Text = "<span class='message'>Message Sent Successfully</span>"
                        MessageBoxValidation("Message Sent Successfully", "Information")
                        TitleTxt.Text = ""
                        MsgTxt.Text = ""
                        For Each li As RadComboBoxItem In SalesRepList.Items
                            li.Checked = False
                        Next
                    Else
                        log.Error(Err_Desc)
                        MessageBoxValidation("Error while sending message", "Information")
                        Exit Sub
                    End If
                Else
                    If objMessage.SendMsgByGroup(Err_No, Err_Desc, SenderID, SenderName) Then
                        '  MsgLbl.Text = "<span class='message'>Message Sent Successfully</span>"
                        MessageBoxValidation("Message Sent Successfully", "Information")
                        TitleTxt.Text = ""
                        MsgTxt.Text = ""
                        For Each li As RadComboBoxItem In SalesRepList.Items
                            li.Checked = False
                        Next
                    Else
                        log.Error(Err_Desc)
                        MessageBoxValidation("Error while sending message", "Information")
                        Exit Sub
                    End If
                End If
            End If
            Rdo_userType.ClearSelection()
            Rdo_userType.SelectedIndex = 0
            BindSalesRepList()
        Catch ex As Exception
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        Finally
            objMessage = Nothing
        End Try

    End Sub

    Protected Sub RecallBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles RecallBtn.Click
        RecallBtn.Visible = False
        SendMsgBtn.Visible = False
        SendMsgBtn_Recall.Visible = True
        CancelBtn.Visible = True
        BackBtn.Visible = False
        MakeEnabled(True)
    End Sub

    Protected Sub MsgTxt_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles MsgTxt.TextChanged

    End Sub

    Private Sub BackBtn_Click(sender As Object, e As EventArgs) Handles BackBtn.Click
        TopPanel.Visible = True
        MsgSelectPanel.Visible = False
        NewMsgPanel.Visible = False
        MsgLbl.Text = ""
        MSG_Date.SelectedDate = Now.Date
        MakeEnabled(True)
    End Sub

    Private Sub Rdo_userType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Rdo_userType.SelectedIndexChanged
        BindSalesRepList()
    End Sub

    Private Sub SendMsgBtn_Recall_Click(sender As Object, e As EventArgs) Handles SendMsgBtn_Recall.Click
        If TitleTxt.Text.Trim = "" Then
            MessageBoxValidation("Please Enter a valid Message Ttile.", "Validation")
            Exit Sub
        End If
        If MsgTxt.Text.Trim = "" Then
            MessageBoxValidation("Please Enter a valid Message Text.", "Validation")
            Exit Sub
        End If

        objMessage = New Message()
        Try
            objMessage.MessageTitle = TitleTxt.Text
            objMessage.MessageContent = MsgTxt.Text

            ' Dim sDate As Array = Sel_Date.SelectedDate
            objMessage.MessageDate = Sel_Date.SelectedDate
            objMessage.SenderIDProp = CType(Session("USER_ACCESS"), UserAccess).SalesRepID
            Dim SalesRep_List As String = Nothing
            For Each li As RadComboBoxItem In SalesRepList.Items
                If li.Checked = True Then
                    SalesRep_List += li.Value & ","
                End If
            Next




            If DateTime.Parse(Me.Expiry_Date.SelectedDate) <= DateTime.Parse(Me.Sel_Date.SelectedDate) Then
                MessageBoxValidation("Expiry date should be greater than to start date.", "Validation")
                Exit Sub
            End If

            If SalesRep_List Is Nothing Then
                MessageBoxValidation("Please select at least one user", "Validation")
                Exit Sub
            End If
            
            objMessage.SalesRepID = SalesRep_List
            '  Dim eDate As Array = Expiry_Date.Text.Split("/")
            objMessage.MessageExpiryDate = Expiry_Date.SelectedDate
            Dim SenderName As String = (New SalesWorx.BO.Common.User).GetUserName(Session("USER_ACCESS").UserID).ToString
            Dim SenderID As String = CType(Session("User_Access"), UserAccess).UserID
            objMessage.Sender_ID = SenderID

            If CType(Session.Item("CONTROL_PARAMS"), ControlParams).MSG_MODULE_VERSION = "1" Then
                If objMessage.SendMessage(Err_No, Err_Desc) Then
                    '  MsgLbl.Text = "<span class='message'>Message Sent Successfully</span>"
                    MessageBoxValidation("Message Sent Successfully", "Information")
                    TitleTxt.Text = ""
                    MsgTxt.Text = ""
                    For Each li As RadComboBoxItem In SalesRepList.Items
                        li.Checked = False
                    Next
                Else
                    log.Error(Err_Desc)
                    MessageBoxValidation("Error while sending message", "Information")
                    Exit Sub
                End If
            Else

                If objMessage.RecallMsg(Err_No, Err_Desc, SenderID, SenderName) Then
                    '  MsgLbl.Text = "<span class='message'>Message Sent Successfully</span>"
                    MessageBoxValidation("Message Sent Successfully", "Information")
                    TitleTxt.Text = ""
                    MsgTxt.Text = ""
                    For Each li As RadComboBoxItem In SalesRepList.Items
                        li.Checked = False
                    Next
                Else
                    log.Error(Err_Desc)
                    MessageBoxValidation("Error while sending message", "Information")
                    Exit Sub
                End If
            End If
            Rdo_userType.ClearSelection()
            Rdo_userType.SelectedIndex = 0
            BindSalesRepList()
        Catch ex As Exception
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        Finally
            objMessage = Nothing
        End Try
    End Sub
End Class