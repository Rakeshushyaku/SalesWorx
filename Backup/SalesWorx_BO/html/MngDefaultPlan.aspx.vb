Imports System.Collections
Imports System.Drawing
Imports AjaxControlToolkit
Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web
Imports System.Web.UI.WebControls
Imports SalesWorx.BO.Common
Imports log4net
Imports System.Text.RegularExpressions


Partial Public Class MngDefaultPlan
    Inherits System.Web.UI.Page
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim StartDate As Date
    Dim EndDate As Date

    Dim postbackcheck As Boolean

    Dim BTN_STAT As String

    'helper debug vars.....
    Dim objRecords As ArrayList
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim StartDateInt As Integer
    'default plan obj.......
    Dim objDefPlan As RoutePlan
    Dim ObjRoutePlan As RoutePlan
    Dim CalDataSet As DataSet
    Dim Dtype As String = "X"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub MngDefaultPlan_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Route Planner"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        
        If (Not IsPostBack) Then

            CheckRefresh.Value = "N"
            FromPopUp.Value = "N"
            ISRefreskClick.Value = "N"
            Cell1.Value = "N"
            Cell2.Value = "N"
            Cell3.Value = "N"
            Cell4.Value = "N"
            Cell5.Value = "N"
            Cell6.Value = "N"
            Cell7.Value = "N"
            Cell8.Value = "N"
            Cell9.Value = "N"
            Cell10.Value = "N"
            Cell11.Value = "N"
            Cell12.Value = "N"
            Cell13.Value = "N"
            Cell14.Value = "N"
            Cell15.Value = "N"
            Cell16.Value = "N"
            Cell17.Value = "N"
            Cell18.Value = "N"
            Cell19.Value = "N"
            Cell20.Value = "N"
            Cell21.Value = "N"
            Cell22.Value = "N"
            Cell23.Value = "N"
            Cell24.Value = "N"
            Cell25.Value = "N"
            Cell26.Value = "N"
            Cell27.Value = "N"
            Cell28.Value = "N"
            Cell29.Value = "N"
            Cell30.Value = "N"
            Cell31.Value = "N"

            Action_Mode.Value = Request.QueryString("Mode")
            RP_ID.Value = Request.QueryString("RP_ID")
            ' SaveBtn.Enabled = False
        End If

        If (FromPopUp.Value = "Y" And Not ISRefreskClick.Value = "Y") Then
            Start_Date.Text = Session("Start_Date")
            End_Date.Text = Session("End_Date")
            StartDate = Convert.ToDateTime(Session("Start_Date").Substring(3, 3) & Session("Start_Date").Substring(0, 3) & Session("Start_Date").Substring(6, 4))
            EndDate = Convert.ToDateTime(Session("End_Date").Substring(3, 3) & Session("End_Date").Substring(0, 3) & Session("End_Date").Substring(6, 4))
            ISRefreskClick.Value = "Y"
        End If

      

        If Action_Mode.Value = "ADD" Or Action_Mode.Value = "SAVE" Then
            SetPanelVisibility(True, False)
        ElseIf Action_Mode.Value = "MODIFY" Or Action_Mode.Value = "UPDATE" Then
            SetPanelVisibility(False, True)
            ShowAndRefreshBtn.Enabled = False
            ObjRoutePlan = New RoutePlan

            Try
                UpdateBtn.Visible = True
                ObjRoutePlan.DefPlanId = RP_ID.Value
                If (ObjRoutePlan.IsPlanUsed(Err_No, Err_Desc)) Then
                    UpdateBtn.Enabled = False
                    UpdateBtn.Visible = False
                End If
                If (Err_Desc Is Nothing) Then
                    ObjRoutePlan.GetDefaultPlan(Err_No, Err_Desc)
                    Start_Date.Enabled = False
                    End_Date.Enabled = False
                    Description.Enabled = False

                    Description.Text = ObjRoutePlan.Description
                    Dim TemSDate As Date = ObjRoutePlan.SDate
                    Start_Date.Text = IIf(CInt(TemSDate.Day) < 10, "0" & TemSDate.Day, TemSDate.Day) & "-" & IIf(CInt(TemSDate.Month) < 10, "0" & TemSDate.Month, TemSDate.Month) & "-" & TemSDate.Year
                    Dim TemEDate As Date = ObjRoutePlan.EDate
                    End_Date.Text = IIf(CInt(TemEDate.Day) < 10, "0" & TemEDate.Day, TemEDate.Day) & "-" & IIf(CInt(TemEDate.Month) < 10, "0" & TemEDate.Month, TemEDate.Month) & "-" & TemEDate.Year

                    StartDate = Convert.ToDateTime(Start_Date.Text.Substring(3, 3) & Start_Date.Text.Substring(0, 3) & Start_Date.Text.Substring(6, 4))
                    EndDate = Convert.ToDateTime(End_Date.Text.Substring(3, 3) & End_Date.Text.Substring(0, 3) & End_Date.Text.Substring(6, 4))

                    If (Err_Desc Is Nothing) Then

                        CalDataSet = ObjRoutePlan.GetDefaultCalendarDetails(Err_No, Err_Desc)
                        If (CalDataSet.Tables("DefaultPlan").Rows.Count() > 0) Then
                            CalPanel.Visible = True
                            Session("Start_Date") = Start_Date.Text.Trim()
                            Session("End_Date") = End_Date.Text.Trim()
                            DefPlanCalendar.VisibleDate = CDate(StartDate)
                            Action_Mode.Value = "MODIFY"
                            StartDateRegEx.Display = Web.UI.WebControls.ValidatorDisplay.None
                            ValidStartDateReq.Enabled = False
                            ValidatorCalloutExtender1.Enabled = False
                        End If
                    End If
                Else
                    log.Error(Err_Desc)
                    Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=DefaultPlan.aspx&Title=Route+Planner")
                End If

            Catch ex As Exception
                log.Error(ex.Message)
                Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=DefaultPlan.aspx")
            Finally
                ObjRoutePlan = Nothing
            End Try
        End If
       
       
    End Sub
    Protected Sub SetPanelVisibility(ByVal AddPnl As Boolean, ByVal ModPnl As Boolean)
        AddAndSavePanel.Visible = AddPnl
        ModAndUpdatePanel.Visible = ModPnl
    End Sub
    Private Sub SaveBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveBtn.Click
        System.Threading.Thread.Sleep(1000)
        Dim transaction As SqlTransaction
        Try
            ObjRoutePlan = New RoutePlan
            If (CalPanel.Visible = True) Then
                Dim SDate As String = Start_Date.Text.Trim()
                Dim EDate As String = End_Date.Text.Trim()

                StartDate = Convert.ToDateTime(SDate.Substring(3, 3) & SDate.Substring(0, 3) & SDate.Substring(6, 4))
                EndDate = Convert.ToDateTime(EDate.Substring(3, 3) & EDate.Substring(0, 3) & EDate.Substring(6, 4))

                ObjRoutePlan.SDate = StartDate
                ObjRoutePlan.EDate = EndDate
                ObjRoutePlan.Site = Session("Site")
                ObjRoutePlan.Description = Description.Text

                Dim SqlConn As SqlConnection
                Dim SqlCmd As SqlCommand
                SqlConn = ObjRoutePlan.GetConnection()

                transaction = SqlConn.BeginTransaction("SampleTransaction")

                Dim DefaultID As Integer = ObjRoutePlan.InsertDefaultPlan(objRecords, Err_No, Err_Desc)
                If (DefaultID > 0) Then
                    For i As Integer = Convert.ToInt16(StartDate.Day) To Convert.ToInt16(EndDate.Day)
                        Dim HiddenCon As System.Web.UI.HtmlControls.HtmlInputHidden = TryCast(Page.FindControl("ctl00$ContentPlaceHolder1$HDay" & i.ToString()), System.Web.UI.HtmlControls.HtmlInputHidden)
                        If HiddenCon IsNot Nothing And Err_Desc Is Nothing Then
                            Dim Dat As Date
                            Dat = Convert.ToDateTime(StartDate.Month & "-" & i & "-" & StartDate.Year)
                            If (HiddenCon.Value = "" Or HiddenCon.Value = "W") And (IsOffDay(Dat)) Then
                                HiddenCon.Value = "W"
                            ElseIf (HiddenCon.Value = "M" Or HiddenCon.Value = "") And (IsOffDay(Dat)) Then
                                HiddenCon.Value = "X"
                            ElseIf (HiddenCon.Value = "") Then
                                HiddenCon.Value = "X"
                            End If
                            InsertDefaultDetails(HiddenCon.Value, DefaultID, i, SqlConn, SqlCmd, transaction, Err_Desc, Err_No)
                            If Err_Desc IsNot Nothing Then
                                log.Error(Err_Desc)
                                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_003") & "&next=DefaultPlan.aspx&Title=Route+Planner", False)
                                Exit Sub
                            End If
                        End If
                    Next
                End If
                transaction.Commit()
                ObjRoutePlan.CloseConnection(SqlConn)
                ' Dim s As String() = Session("Site").Split("-")
                Dim OID As String = Session("Site")
                ' If s.Length > 1 Then
                'OID = s(1)
                'End If
                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "CALL PLANNING", "DEFAULT PLAN", OID.Trim(), "Desc: " & Description.Text & "/ Start Date :  " & StartDate & "/ End Date :  " & EndDate, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", OID.Trim())
                Response.Redirect("information.aspx?mode=0&msg=Default+plan+saved+successfully&next=DefaultPlan.aspx&Title=Route+Planner", False)
            End If
        Catch ex As Exception
            If Err_No = 0 Then
                Err_No = "74010"
            End If
            Err_Desc = ex.Message
            log.Error(GetExceptionInfo(ex))
            If (Err_Desc Is Nothing) Then
                Response.Redirect("information.aspx?mode=0&msg=Default+plan+saved+successfully&next=DefaultPlan.aspx?ret=1&Title=Route+Planner", False)
            Else
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_003") & "&next=DefaultPlan.aspx&Title=Route+Planner", False)
            End If
        Finally
            ObjRoutePlan = Nothing
        End Try

    End Sub
    Function IsOffDay(ByVal PDate As Date) As Boolean
        Dim OffDays As String
        Dim bRetval As Boolean = False
        Try
            OffDays = (New SalesWorx.BO.Common.RoutePlan).GetOffDays().ToUpper()
            Dim days() As String
            days = OffDays.Split(",")
            For i As Integer = 0 To days.Length - 1
                If PDate.ToString("ddd").ToUpper = days(i) Then
                    bRetval = True
                    Exit For
                End If
            Next
        Catch ex As Exception

        End Try
        Return bRetval
    End Function
    Private Sub UpdateBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UpdateBtn.Click
        System.Threading.Thread.Sleep("1000")
        ObjRoutePlan = New RoutePlan
        Try
            Dim SDate As String = Start_Date.Text.Trim()
            Dim EDate As String = End_Date.Text.Trim()

            StartDate = Convert.ToDateTime(SDate.Substring(3, 3) & SDate.Substring(0, 3) & SDate.Substring(6, 4))
            EndDate = Convert.ToDateTime(EDate.Substring(3, 3) & EDate.Substring(0, 3) & EDate.Substring(6, 4))

            ObjRoutePlan.SDate = StartDate
            ObjRoutePlan.EDate = EndDate
            ObjRoutePlan.Site = Session("Site")
            ObjRoutePlan.Description = Description.Text

            Dim transaction As SqlTransaction

            Dim SqlConn As SqlConnection
            Dim SqlCmd As SqlCommand
            SqlConn = ObjRoutePlan.GetConnection()

            transaction = SqlConn.BeginTransaction("SampleTransaction")

            For i As Integer = Convert.ToInt16(StartDate.Day) To Convert.ToInt16(EndDate.Day)

                Dim HiddenCon As System.Web.UI.HtmlControls.HtmlInputHidden = TryCast(Page.FindControl("ctl00$ContentPlaceHolder1$HDay" & i.ToString()), System.Web.UI.HtmlControls.HtmlInputHidden)
                If HiddenCon IsNot Nothing And Err_Desc Is Nothing Then
                    If Not (HiddenCon.Value = "") Then
                        Dim foundRows() As DataRow = CalDataSet.Tables("DefaultPlan").Select("Day=" & i & " and Default_Plan_ID=" & RP_ID.Value & "")
                        Dim DefaultDetailID As Integer = CInt(foundRows(0).Item(0))
                        InsertDefaultDetails(HiddenCon.Value, DefaultDetailID, i, SqlConn, SqlCmd, transaction, Err_Desc, Err_No)
                        If Err_Desc IsNot Nothing Then
                            log.Error(Err_Desc)
                            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_004") & "&next=DefaultPlan.aspx&Title=Route+Planner", False)
                            Exit Sub
                        End If
                    End If
                End If
            Next

            transaction.Commit()
            ObjRoutePlan.CloseConnection(SqlConn)
            ' Dim s As String() = Session("Site").Split("-")
            Dim OID As String = Session("Site")
            ' If s.Length > 1 Then
            'OID = s(1)
            ' End If
            objLogin.SaveUserLog(Err_No, Err_Desc, "U", "CALL PLANNING", "DEFAULT PLAN", OID.Trim(), "Desc: " & Description.Text & "/ Start Date :  " & StartDate & "/ End Date :  " & EndDate, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", OID.Trim())
            Response.Redirect("information.aspx?mode=0&msg=Default+plan+updated+successfully&next=DefaultPlan.aspx&Title=Route+Planner")
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            If (Err_Desc Is Nothing) Then
                Response.Redirect("information.aspx?mode=0&msg=Default+plan+updated+successfully&next=DefaultPlan.aspx&Title=Route+Planner")
            Else
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_004") & "&next=DefaultPlan.aspx&Title=Route+Planner")
            End If

        Finally
            ObjRoutePlan = Nothing
        End Try
        ClearSession()
    End Sub

    Private Sub InsertDefaultDetails(ByVal DayValue As String, ByVal Id As Integer, ByVal Day As Integer, ByRef SqlConn As SqlConnection, ByRef sqlcomm As SqlCommand, ByRef transaction As SqlTransaction, ByRef Desc As String, ByRef Err_No As Long)
        Dim stringArr As Array = DayValue.Split("|")
        Dim RoutePlan As New RoutePlan

        Try

            With RoutePlan
                .DayType = IIf(stringArr.Length <> 0, stringArr(0), "")

                If (stringArr.Length > 1) Then
                    .UserComments = stringArr(1)
                Else
                    .UserComments = ""
                End If

                If (stringArr.Length > 2) Then
                    .StartTime = Convert.ToDateTime(StartDate.Month & "-" & Day & "-" & StartDate.Year & " " & stringArr(2))
                Else
                    .StartTime = "1/1/1900"

                End If
                If (stringArr.Length > 3) Then
                    .End_Time = Convert.ToDateTime(StartDate.Month & "-" & Day & "-" & StartDate.Year & " " & stringArr(3))
                Else
                    .End_Time = "1/1/1900"
                End If

                .day = Day


                If ((stringArr(0) = "O" And stringArr.Length > 2) Or stringArr(0) = "M" Or stringArr(0) = "X") Then
                    .CanPlanVisits = "Y"
                Else
                    .CanPlanVisits = "N"
                End If
            End With
            If (Action_Mode.Value = "MODIFY") Then
                RoutePlan.DefPlanDetailID = Id
                RoutePlan.UpdateDefaultPlanDetails(Err_No, Err_Desc, SqlConn, sqlcomm, transaction)
            Else
                RoutePlan.DefPlanId = Id
                RoutePlan.InsertDefaultPlanDetails(objRecords, Err_No, Desc, SqlConn, sqlcomm, transaction)
            End If
        Catch ex As Exception
            Err_No = "74012"
            Desc = ex.Message
        End Try

    End Sub

    Protected Sub DefPlanCalendar_DayRender(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DayRenderEventArgs) Handles DefPlanCalendar.DayRender
        Try

            If (IsPostBack) Then
                If Action_Mode.Value = "ADD" Then

                    DefPlanCalendar.ToolTip = "Default Route Plan"

                    Dim CellText As String = ""
                    Dim CellToolTip As String = ""
                    Dim IsWeekEnd As Boolean = False

                    e.Cell.Controls.Clear()
                    DefPlanCalendar.ToolTip = "Default Route Plan"
                    Dim CalendarCell As TableCell
                    If (EndDate.Day >= e.Day.Date.Day And e.Day.Date.Day >= StartDate.Day And e.Day.Date.Month = StartDate.Month And e.Day.Date.Month = EndDate.Month) Then


                        CalendarCell = e.Cell

                        '                        CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('POP_DefaultPlan.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&IRS=N&IsRefresh=N&dayref=" & e.Day.Date.Day & "&DType=X','CustomerList','width=450,height=180')")

                        If e.Day.Date > Now.Date Then
                            e.Cell.Style.Add("cursor", "Pointer")
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('POP_DefaultPlan.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&IRS=N&IsRefresh=N&dayref=" & e.Day.Date.Day & "&DType=" & Dtype & "','CustomerList','width=450,height=180')")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = "<span class='dateCalendarCntrl'>" & e.Day.Date.Day & "</span>"
                        e.Cell.ToolTip = e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year
                        e.Cell.BackColor = System.Drawing.Color.FromName("#EEEEEE")
                        e.Cell.HorizontalAlign = Web.UI.WebControls.HorizontalAlign.Left

                        If (FromPopUp.Value = "Y") Then
                            Dim CellUrl As String
                            Dim CellBgColor As String = "#EEEEEE"
                            If (HDay1.Value <> "" And e.Day.Date.Day.ToString() = "1") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay1.Value)
                                If (HDay1.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay1.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay1.Value.Substring(0, 1) = "M" Or HDay1.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay1.Value = Replace(HDay1.Value, HDay1.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay1.Value = Replace(HDay1.Value, HDay1.Value.ToString().Substring(0, 1), "W")
                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay1.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If

                            If (HDay2.Value <> "" And e.Day.Date.Day.ToString() = "2") Then

                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay2.Value)
                                If (HDay2.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay2.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay2.Value.Substring(0, 1) = "M" Or HDay2.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay2.Value = Replace(HDay2.Value, HDay2.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay2.Value = Replace(HDay2.Value, HDay2.Value.ToString().Substring(0, 1), "W")
                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay2.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If

                            If (HDay3.Value <> "" And e.Day.Date.Day.ToString() = "3") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay3.Value)
                                If (HDay3.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay3.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay3.Value.Substring(0, 1) = "M" Or HDay3.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay3.Value = Replace(HDay3.Value, HDay3.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay3.Value = Replace(HDay3.Value, HDay3.Value.ToString().Substring(0, 1), "W")
                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay3.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If

                            If (HDay4.Value <> "" And e.Day.Date.Day.ToString() = "4") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay4.Value)
                                If (HDay4.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay4.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay4.Value.Substring(0, 1) = "M" Or HDay4.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay4.Value = Replace(HDay4.Value, HDay4.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay4.Value = Replace(HDay4.Value, HDay4.Value.ToString().Substring(0, 1), "W")
                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay4.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If

                            If (HDay5.Value <> "" And e.Day.Date.Day.ToString() = "5") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay5.Value)
                                If (HDay5.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay5.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay5.Value.Substring(0, 1) = "M" Or HDay5.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay5.Value = Replace(HDay5.Value, HDay5.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay5.Value = Replace(HDay5.Value, HDay5.Value.ToString().Substring(0, 1), "W")
                                End If

                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay5.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If

                            If (HDay6.Value <> "" And e.Day.Date.Day.ToString() = "6") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay6.Value)
                                If (HDay6.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay6.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay6.Value.Substring(0, 1) = "M" Or HDay6.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay6.Value = Replace(HDay6.Value, HDay6.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay6.Value = Replace(HDay6.Value, HDay6.Value.ToString().Substring(0, 1), "W")
                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay6.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If

                            If (HDay7.Value <> "" And e.Day.Date.Day.ToString() = "7") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay7.Value)
                                If (HDay7.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay7.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay7.Value.Substring(0, 1) = "M" Or HDay7.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay7.Value = Replace(HDay7.Value, HDay7.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay7.Value = Replace(HDay7.Value, HDay7.Value.ToString().Substring(0, 1), "W")
                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay7.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If

                            If (HDay8.Value <> "" And e.Day.Date.Day.ToString() = "8") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay8.Value)
                                If (HDay8.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay8.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay8.Value.Substring(0, 1) = "M" Or HDay8.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay8.Value = Replace(HDay8.Value, HDay8.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay8.Value = Replace(HDay8.Value, HDay8.Value.ToString().Substring(0, 1), "W")
                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay8.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If

                            If (HDay9.Value <> "" And e.Day.Date.Day.ToString() = "9") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay9.Value)
                                If (HDay9.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay9.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay9.Value.Substring(0, 1) = "M" Or HDay9.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay9.Value = Replace(HDay9.Value, HDay9.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay9.Value = Replace(HDay9.Value, HDay9.Value.ToString().Substring(0, 1), "W")
                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay9.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If
                            If (HDay10.Value <> "" And e.Day.Date.Day.ToString() = "10") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay10.Value)
                                If (HDay10.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay10.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay10.Value.Substring(0, 1) = "M" Or HDay10.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay10.Value = Replace(HDay10.Value, HDay10.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay10.Value = Replace(HDay10.Value, HDay10.Value.ToString().Substring(0, 1), "W")
                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay10.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If

                            If (HDay11.Value <> "" And e.Day.Date.Day.ToString() = "11") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay11.Value)
                                If (HDay11.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay11.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay11.Value.Substring(0, 1) = "M" Or HDay11.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay11.Value = Replace(HDay11.Value, HDay11.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay11.Value = Replace(HDay11.Value, HDay11.Value.ToString().Substring(0, 1), "W")
                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay11.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If

                            If (HDay12.Value <> "" And e.Day.Date.Day.ToString() = "12") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay12.Value)
                                If (HDay12.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay12.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay12.Value.Substring(0, 1) = "M" Or HDay12.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay12.Value = Replace(HDay12.Value, HDay12.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay12.Value = Replace(HDay12.Value, HDay12.Value.ToString().Substring(0, 1), "W")

                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay12.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If

                            If (HDay13.Value <> "" And e.Day.Date.Day.ToString() = "13") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay13.Value)
                                If (HDay13.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay13.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay13.Value.Substring(0, 1) = "M" Or HDay13.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay13.Value = Replace(HDay13.Value, HDay13.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay13.Value = Replace(HDay13.Value, HDay13.Value.ToString().Substring(0, 1), "W")
                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay13.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If

                            If (HDay14.Value <> "" And e.Day.Date.Day.ToString() = "14") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay14.Value)
                                If (HDay14.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay14.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay14.Value.Substring(0, 1) = "M" Or HDay14.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay14.Value = Replace(HDay14.Value, HDay14.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay14.Value = Replace(HDay14.Value, HDay14.Value.ToString().Substring(0, 1), "W")
                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay14.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If

                            If (HDay15.Value <> "" And e.Day.Date.Day.ToString() = "15") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay15.Value)
                                If (HDay15.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay15.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay15.Value.Substring(0, 1) = "M" Or HDay15.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay15.Value = Replace(HDay15.Value, HDay15.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay15.Value = Replace(HDay15.Value, HDay15.Value.ToString().Substring(0, 1), "W")
                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay15.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If

                            If (HDay16.Value <> "" And e.Day.Date.Day.ToString() = "16") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay16.Value)
                                If (HDay16.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay16.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay16.Value.Substring(0, 1) = "M" Or HDay16.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay16.Value = Replace(HDay16.Value, HDay16.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay16.Value = Replace(HDay16.Value, HDay16.Value.ToString().Substring(0, 1), "W")
                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay16.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If

                            If (HDay17.Value <> "" And e.Day.Date.Day.ToString() = "17") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay17.Value)
                                If (HDay17.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay17.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay17.Value.Substring(0, 1) = "M" Or HDay17.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay17.Value = Replace(HDay17.Value, HDay17.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay17.Value = Replace(HDay17.Value, HDay17.Value.ToString().Substring(0, 1), "W")
                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay17.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If

                            If (HDay18.Value <> "" And e.Day.Date.Day.ToString() = "18") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay18.Value)
                                If (HDay18.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay18.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay18.Value.Substring(0, 1) = "M" Or HDay18.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay18.Value = Replace(HDay18.Value, HDay18.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay18.Value = Replace(HDay18.Value, HDay18.Value.ToString().Substring(0, 1), "W")
                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay18.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If

                            If (HDay19.Value <> "" And e.Day.Date.Day.ToString() = "19") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay19.Value)
                                If (HDay19.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay19.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay19.Value.Substring(0, 1) = "M" Or HDay19.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay19.Value = Replace(HDay19.Value, HDay19.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay19.Value = Replace(HDay19.Value, HDay19.Value.ToString().Substring(0, 1), "W")
                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay19.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If

                            If (HDay20.Value <> "" And e.Day.Date.Day.ToString() = "20") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay20.Value)
                                If (HDay20.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay20.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay20.Value.Substring(0, 1) = "M" Or HDay20.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay20.Value = Replace(HDay20.Value, HDay20.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay20.Value = Replace(HDay20.Value, HDay20.Value.ToString().Substring(0, 1), "W")
                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay20.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If

                            If (HDay21.Value <> "" And e.Day.Date.Day.ToString() = "21") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay21.Value)
                                If (HDay21.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay21.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay21.Value.Substring(0, 1) = "M" Or HDay21.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay21.Value = Replace(HDay21.Value, HDay21.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay21.Value = Replace(HDay21.Value, HDay21.Value.ToString().Substring(0, 1), "W")
                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay21.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If

                            If (HDay22.Value <> "" And e.Day.Date.Day.ToString() = "22") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay22.Value)
                                If (HDay22.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay22.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay22.Value.Substring(0, 1) = "M" Or HDay22.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay22.Value = Replace(HDay22.Value, HDay22.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay22.Value = Replace(HDay22.Value, HDay22.Value.ToString().Substring(0, 1), "W")
                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay22.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If

                            If (HDay23.Value <> "" And e.Day.Date.Day.ToString() = "23") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay23.Value)
                                If (HDay23.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay23.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay23.Value.Substring(0, 1) = "M" Or HDay23.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay23.Value = Replace(HDay23.Value, HDay23.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay23.Value = Replace(HDay23.Value, HDay23.Value.ToString().Substring(0, 1), "W")
                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay23.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If

                            If (HDay24.Value <> "" And e.Day.Date.Day.ToString() = "24") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay24.Value)
                                If (HDay24.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay24.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay24.Value.Substring(0, 1) = "M" Or HDay24.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay24.Value = Replace(HDay24.Value, HDay24.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay24.Value = Replace(HDay24.Value, HDay24.Value.ToString().Substring(0, 1), "W")
                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay24.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If

                            If (HDay25.Value <> "" And e.Day.Date.Day.ToString() = "25") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay25.Value)
                                If (HDay25.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay25.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay25.Value.Substring(0, 1) = "M" Or HDay25.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay25.Value = Replace(HDay25.Value, HDay25.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay25.Value = Replace(HDay25.Value, HDay25.Value.ToString().Substring(0, 1), "W")
                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay25.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If

                            If (HDay26.Value <> "" And e.Day.Date.Day.ToString() = "26") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay26.Value)
                                If (HDay26.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay26.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay26.Value.Substring(0, 1) = "M" Or HDay26.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay26.Value = Replace(HDay26.Value, HDay26.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay26.Value = Replace(HDay26.Value, HDay26.Value.ToString().Substring(0, 1), "W")
                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay26.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If

                            If (HDay27.Value <> "" And e.Day.Date.Day.ToString() = "27") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay27.Value)
                                If (HDay27.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay27.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay27.Value.Substring(0, 1) = "M" Or HDay27.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay27.Value = Replace(HDay27.Value, HDay27.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay27.Value = Replace(HDay27.Value, HDay27.Value.ToString().Substring(0, 1), "W")
                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay27.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If

                            If (HDay28.Value <> "" And e.Day.Date.Day.ToString() = "28") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay28.Value)
                                If (HDay28.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay28.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay28.Value.Substring(0, 1) = "M" Or HDay28.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay28.Value = Replace(HDay28.Value, HDay28.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay28.Value = Replace(HDay28.Value, HDay28.Value.ToString().Substring(0, 1), "W")
                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay28.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If

                            If (HDay29.Value <> "" And e.Day.Date.Day.ToString() = "29") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay29.Value)
                                If (HDay29.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay29.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay29.Value.Substring(0, 1) = "M" Or HDay29.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay29.Value = Replace(HDay29.Value, HDay29.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay29.Value = Replace(HDay29.Value, HDay29.Value.ToString().Substring(0, 1), "W")
                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay29.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If

                            If (HDay30.Value <> "" And e.Day.Date.Day.ToString() = "30") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay30.Value)
                                If (HDay30.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay30.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay30.Value.Substring(0, 1) = "M" Or HDay30.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay30.Value = Replace(HDay30.Value, HDay30.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay30.Value = Replace(HDay30.Value, HDay30.Value.ToString().Substring(0, 1), "W")
                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay30.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If

                            If (HDay31.Value <> "" And e.Day.Date.Day.ToString() = "31") Then
                                CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay31.Value)
                                If (HDay31.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#F3DF80"
                                    CellUrl = "X"
                                ElseIf (HDay31.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                                    CellBgColor = "#A9D7FC"
                                    CellUrl = "X"
                                ElseIf (HDay31.Value.Substring(0, 1) = "M" Or HDay31.Value.Substring(0, 1) = "X") Then
                                    CellBgColor = "#EEEEEE"
                                    CellUrl = "X"
                                    HDay31.Value = Replace(HDay31.Value, HDay31.Value.ToString().Substring(0, 1), "X")
                                End If
                                If (IsOffDay(e.Day.Date)) Then
                                    IsWeekEnd = True
                                    HDay31.Value = Replace(HDay31.Value, HDay31.Value.ToString().Substring(0, 1), "W")
                                End If
                                e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay31.Value)
                                e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                                e.Cell.Text = CellText
                            End If


                        End If

                        If IsWeekEnd = False Then
                            If IsOffDay(e.Day.Date) Then
                                e.Cell.BackColor = System.Drawing.Color.FromName("#A9D7FC")
                                Dtype = "W"
                            End If
                        End If
                        If e.Cell.BackColor = System.Drawing.Color.FromName("#EEEEEE") And (IsOffDay(e.Day.Date)) Then
                            Dtype = "M"
                        End If
                        If e.Cell.BackColor = System.Drawing.Color.FromName("#A9D7FC") And (IsOffDay(e.Day.Date)) Then
                            Dtype = "W"
                        End If
                        If e.Cell.BackColor = System.Drawing.Color.FromName("#EEEEEE") And Not (IsOffDay(e.Day.Date)) Then
                            Dtype = "X"
                        End If


                        If e.Day.Date > Now.Date Then
                            e.Cell.Style.Add("cursor", "Pointer")
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('POP_DefaultPlan.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&IRS=N&IsRefresh=N&dayref=" & e.Day.Date.Day & "&DType=" & Dtype & "','CustomerList','width=450,height=180')")
                        Else

                            e.Cell.Style.Add("cursor", "Cursor")
                        End If

                        Dim HiddenCon As System.Web.UI.HtmlControls.HtmlInputHidden = TryCast(Page.FindControl("ctl00$ContentPlaceHolder1$HDay" & e.Day.Date.Day.ToString()), System.Web.UI.HtmlControls.HtmlInputHidden)
                        Session("HDay" & e.Day.Date.Day & "") = HiddenCon.Value


                    Else
                        e.Cell.Visible = False
                        If ((e.Day.Date < StartDate And (DateDiff(DateInterval.WeekOfYear, e.Day.Date, StartDate) = 0)) Or (e.Day.Date > EndDate And (DateDiff(DateInterval.WeekOfYear, e.Day.Date, EndDate)) = 0)) Then
                            e.Cell.Visible = True
                        End If
                        e.Cell.Text = ""

                        e.Cell.ToolTip = ""
                    End If

                End If
            End If

            If (Action_Mode.Value = "MODIFY") Then
                Dim SDate As String = Start_Date.Text.Trim()
                Dim EDate As String = End_Date.Text.Trim()
                Dim CellText As String
                Dim IsWeekEnd As Boolean = False

                StartDate = Convert.ToDateTime(SDate.Substring(3, 3) & SDate.Substring(0, 3) & SDate.Substring(6, 4))
                EndDate = Convert.ToDateTime(EDate.Substring(3, 3) & EDate.Substring(0, 3) & EDate.Substring(6, 4))

                If (EndDate.Day >= e.Day.Date.Day And e.Day.Date.Day >= StartDate.Day And e.Day.Date.Month = StartDate.Month And e.Day.Date.Month = EndDate.Month) Then

                    Dim DefaultPlanDetailID As Integer
                    Dim DefDayType As Char
                    Dim CanVisit As Char
                    IsWeekEnd = False
                    Dim foundRows() As DataRow = CalDataSet.Tables("DefaultPlan").Select("Day=" & e.Day.Date.Day & "")
                    DefaultPlanDetailID = foundRows(0).Item(0)
                    DefDayType = foundRows(0).Item(5)
                    CanVisit = foundRows(0).Item(7)

                    Dim Str As New StringBuilder
                    Str.Append(foundRows(0).Item(5))
                    If Not (foundRows(0).Item(6) Is DBNull.Value) Then
                        Str.Append("|" & foundRows(0).Item(6))
                    End If
                    If Not (foundRows(0).Item(3) Is DBNull.Value) Then
                        Str.Append("|" & Convert.ToDateTime(foundRows(0).Item(3)).TimeOfDay.ToString())
                    End If
                    If Not (foundRows(0).Item(4) Is DBNull.Value) Then
                        Str.Append("|" & Convert.ToDateTime(foundRows(0).Item(4)).TimeOfDay.ToString())
                    End If

                    If (e.Day.Date.Day.ToString() = "1" And Cell1.Value = "N") Then
                        HDay1.Value = Str.ToString()
                        Session("HDay1") = Str.ToString()
                    Else
                        Session("HDay1") = HDay1.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "2" And Cell2.Value = "N") Then
                        HDay2.Value = Str.ToString()
                        Session("HDay2") = Str.ToString()
                    Else
                        Session("HDay2") = HDay2.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "3" And Cell3.Value = "N") Then
                        HDay3.Value = Str.ToString()
                        Session("HDay3") = Str.ToString()
                    Else
                        Session("HDay3") = HDay3.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "4" And Cell4.Value = "N") Then
                        HDay4.Value = Str.ToString()
                        Session("HDay4") = Str.ToString()
                    Else
                        Session("HDay4") = HDay4.Value
                    End If

                    If (e.Day.Date.Day.ToString() = "5" And Cell5.Value = "N") Then
                        HDay5.Value = Str.ToString()
                        Session("HDay5") = Str.ToString()
                    Else
                        Session("HDay5") = HDay5.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "6" And Cell6.Value = "N") Then
                        HDay6.Value = Str.ToString()
                        Session("HDay6") = Str.ToString()
                    Else
                        Session("HDay6") = HDay6.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "7" And Cell7.Value = "N") Then
                        HDay7.Value = Str.ToString()
                        Session("HDay7") = Str.ToString()
                    Else
                        Session("HDay7") = HDay7.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "8" And Cell8.Value = "N") Then
                        HDay8.Value = Str.ToString()
                        Session("HDay8") = Str.ToString()
                    Else
                        Session("HDay8") = HDay8.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "9" And Cell9.Value = "N") Then
                        HDay9.Value = Str.ToString()
                        Session("HDay9") = Str.ToString()
                    Else
                        Session("HDay9") = HDay9.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "10" And Cell10.Value = "N") Then
                        HDay10.Value = Str.ToString()
                        Session("HDay10") = Str.ToString()
                    Else
                        Session("HDay10") = HDay10.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "11" And Cell11.Value = "N") Then
                        HDay11.Value = Str.ToString()
                        Session("HDay11") = Str.ToString()
                    Else
                        Session("HDay11") = HDay11.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "12" And Cell12.Value = "N") Then
                        HDay12.Value = Str.ToString()
                        Session("HDay12") = Str.ToString()
                    Else
                        Session("HDay12") = HDay12.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "13" And Cell13.Value = "N") Then
                        HDay13.Value = Str.ToString()
                        Session("HDay13") = Str.ToString()
                    Else
                        Session("HDay13") = HDay13.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "14" And Cell14.Value = "N") Then
                        HDay14.Value = Str.ToString()
                        Session("HDay14") = Str.ToString()
                    Else
                        Session("HDay14") = HDay14.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "15" And Cell15.Value = "N") Then
                        HDay15.Value = Str.ToString()
                        Session("HDay15") = Str.ToString()
                    Else
                        Session("HDay15") = HDay15.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "16" And Cell16.Value = "N") Then
                        HDay16.Value = Str.ToString()
                        Session("HDay16") = Str.ToString()
                    Else
                        Session("HDay16") = HDay16.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "17" And Cell17.Value = "N") Then
                        HDay17.Value = Str.ToString()
                        Session("HDay17") = Str.ToString()
                    Else
                        Session("HDay17") = HDay17.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "18" And Cell18.Value = "N") Then
                        HDay18.Value = Str.ToString()
                        Session("HDay18") = Str.ToString()
                    Else
                        Session("HDay18") = HDay18.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "19" And Cell19.Value = "N") Then
                        HDay19.Value = Str.ToString()
                        Session("HDay19") = Str.ToString()
                    Else
                        Session("HDay19") = HDay19.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "20" And Cell20.Value = "N") Then
                        HDay20.Value = Str.ToString()
                        Session("HDay20") = Str.ToString()
                    Else
                        Session("HDay20") = HDay20.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "21" And Cell21.Value = "N") Then
                        HDay21.Value = Str.ToString()
                        Session("HDay21") = Str.ToString()
                    Else
                        Session("HDay21") = HDay21.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "22" And Cell22.Value = "N") Then
                        HDay22.Value = Str.ToString()
                        Session("HDay22") = Str.ToString()
                    Else
                        Session("HDay22") = HDay22.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "23" And Cell23.Value = "N") Then
                        HDay23.Value = Str.ToString()
                        Session("HDay23") = Str.ToString()
                    Else
                        Session("HDay23") = HDay23.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "24" And Cell24.Value = "N") Then
                        HDay24.Value = Str.ToString()
                        Session("HDay24") = Str.ToString()
                    Else
                        Session("HDay24") = HDay24.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "25" And Cell25.Value = "N") Then
                        HDay25.Value = Str.ToString()
                        Session("HDay25") = Str.ToString()
                    Else
                        Session("HDay25") = HDay25.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "26" And Cell26.Value = "N") Then
                        HDay26.Value = Str.ToString()
                        Session("HDay26") = Str.ToString()
                    Else
                        Session("HDay26") = HDay26.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "27" And Cell27.Value = "N") Then
                        HDay27.Value = Str.ToString()
                        Session("HDay27") = Str.ToString()
                    Else
                        Session("HDay27") = HDay27.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "28" And Cell28.Value = "N") Then
                        HDay28.Value = Str.ToString()
                        Session("HDay28") = Str.ToString()
                    Else
                        Session("HDay28") = HDay28.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "29" And Cell29.Value = "N") Then
                        HDay29.Value = Str.ToString()
                        Session("HDay29") = Str.ToString()
                    Else
                        Session("HDay29") = HDay29.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "30" And Cell30.Value = "N") Then
                        HDay30.Value = Str.ToString()
                        Session("HDay30") = Str.ToString()
                    Else
                        Session("HDay30") = HDay30.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "31" And Cell31.Value = "N") Then
                        HDay31.Value = Str.ToString()
                        Session("HDay31") = Str.ToString()
                    Else
                        Session("HDay31") = HDay31.Value
                    End If



                    Dim CalendarCell As TableCell
                    CalendarCell = e.Cell

                    'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('POP_DefaultPlan.aspx?Mode=MODIFY&IRS=N&IsRefresh=N&dayref=" & e.Day.Date.Day & "&DType=X','CustomerList','width=450,height=180')")

                    If e.Day.Date > Now.Date Then
                        e.Cell.Style.Add("cursor", "Pointer")
                        CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('POP_DefaultPlan.aspx?Mode=MODIFY&IRS=N&IsRefresh=N&dayref=" & e.Day.Date.Day & "&DType=" & Dtype & "','CustomerList','width=450,height=180')")
                    Else
                        e.Cell.Style.Add("cursor", "Cursor")
                    End If


                    e.Cell.Text = e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year
                    e.Cell.ToolTip = e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year
                    e.Cell.BackColor = System.Drawing.Color.FromName("#EEEEEE")
                    e.Cell.HorizontalAlign = Web.UI.WebControls.HorizontalAlign.Left

                    Dim CellUrl As String
                    Dim CellBgColor As String = "#F3DF80"
                    If (HDay1.Value <> "" And e.Day.Date.Day.ToString() = "1") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay1.Value)

                        If (HDay1.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay1.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay1.Value.Substring(0, 1) = "M" Or HDay1.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay1.Value = Replace(Session("HDay1").ToString(), Session("HDay1").ToString().Substring(0, 1), "X")
                            Session("HDay1") = HDay1.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay1.Value.Substring(0, 1) = "M" Or HDay1.Value.Substring(0, 1) = "X") Then
                                HDay1.Value = Replace(Session("HDay1").ToString(), Session("HDay1").ToString().Substring(0, 1), "W")
                                Session("HDay1") = HDay1.Value
                            End If
                        End If

                        e.Cell.Style.Add("cursor", "Pointer")
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay1.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    If (HDay2.Value <> "" And e.Day.Date.Day.ToString() = "2") Then

                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay2.Value)

                        If (HDay2.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay2.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay2.Value.Substring(0, 1) = "M" Or HDay2.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay2.Value = Replace(Session("HDay2").ToString(), Session("HDay2").ToString().Substring(0, 1), "X")
                            Session("HDay2") = HDay2.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay2.Value.Substring(0, 1) = "M" Or HDay2.Value.Substring(0, 1) = "X") Then
                                HDay2.Value = Replace(Session("HDay2").ToString(), Session("HDay2").ToString().Substring(0, 1), "W")
                                Session("HDay2") = HDay2.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay2.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    If (HDay3.Value <> "" And e.Day.Date.Day.ToString() = "3") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay3.Value)

                        If (HDay3.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay3.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay3.Value.Substring(0, 1) = "M" Or HDay3.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay3.Value = Replace(Session("HDay3").ToString(), Session("HDay3").ToString().Substring(0, 1), "X")
                            Session("HDay3") = HDay3.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay3.Value.Substring(0, 1) = "M" Or HDay3.Value.Substring(0, 1) = "X") Then
                                HDay3.Value = Replace(Session("HDay3").ToString(), Session("HDay3").ToString().Substring(0, 1), "W")
                                Session("HDay3") = HDay3.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay3.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    If (HDay4.Value <> "" And e.Day.Date.Day.ToString() = "4") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay4.Value)

                        If (HDay4.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay4.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay4.Value.Substring(0, 1) = "M" Or HDay4.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay4.Value = Replace(Session("HDay4").ToString(), Session("HDay4").ToString().Substring(0, 1), "X")
                            Session("HDay4") = HDay4.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay4.Value.Substring(0, 1) = "M" Or HDay4.Value.Substring(0, 1) = "X") Then
                                HDay4.Value = Replace(Session("HDay4").ToString(), Session("HDay4").ToString().Substring(0, 1), "W")
                                Session("HDay4") = HDay4.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay4.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    If (HDay5.Value <> "" And e.Day.Date.Day.ToString() = "5") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay5.Value)

                        If (HDay5.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay5.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay5.Value.Substring(0, 1) = "M" Or HDay5.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay5.Value = Replace(Session("HDay5").ToString(), Session("HDay5").ToString().Substring(0, 1), "X")
                            Session("HDay5") = HDay5.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay5.Value.Substring(0, 1) = "M" Or HDay5.Value.Substring(0, 1) = "X") Then
                                HDay5.Value = Replace(Session("HDay5").ToString(), Session("HDay5").ToString().Substring(0, 1), "W")
                                Session("HDay5") = HDay5.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay5.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    If (HDay6.Value <> "" And e.Day.Date.Day.ToString() = "6") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay6.Value)

                        If (HDay6.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay6.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay6.Value.Substring(0, 1) = "M" Or HDay6.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay6.Value = Replace(Session("HDay6").ToString(), Session("HDay6").ToString().Substring(0, 1), "X")
                            Session("HDay6") = HDay6.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay6.Value.Substring(0, 1) = "M" Or HDay6.Value.Substring(0, 1) = "X") Then
                                HDay6.Value = Replace(Session("HDay6").ToString(), Session("HDay6").ToString().Substring(0, 1), "W")
                                Session("HDay6") = HDay6.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay6.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    If (HDay7.Value <> "" And e.Day.Date.Day.ToString() = "7") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay7.Value)

                        If (HDay7.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay7.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay7.Value.Substring(0, 1) = "M" Or HDay7.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay7.Value = Replace(Session("HDay7").ToString(), Session("HDay7").ToString().Substring(0, 1), "X")
                            Session("HDay7") = HDay7.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay7.Value.Substring(0, 1) = "M" Or HDay7.Value.Substring(0, 1) = "X") Then
                                HDay7.Value = Replace(Session("HDay7").ToString(), Session("HDay7").ToString().Substring(0, 1), "W")
                                Session("HDay7") = HDay7.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay7.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    If (HDay8.Value <> "" And e.Day.Date.Day.ToString() = "8") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay8.Value)

                        If (HDay8.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay8.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay8.Value.Substring(0, 1) = "M" Or HDay8.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay8.Value = Replace(Session("HDay8").ToString(), Session("HDay8").ToString().Substring(0, 1), "X")
                            Session("HDay8") = HDay8.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay8.Value.Substring(0, 1) = "M" Or HDay8.Value.Substring(0, 1) = "X") Then
                                HDay8.Value = Replace(Session("HDay8").ToString(), Session("HDay8").ToString().Substring(0, 1), "W")
                                Session("HDay8") = HDay8.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay8.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    If (HDay9.Value <> "" And e.Day.Date.Day.ToString() = "9") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay9.Value)

                        If (HDay9.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay9.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay9.Value.Substring(0, 1) = "M" Or HDay9.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay9.Value = Replace(Session("HDay9").ToString(), Session("HDay9").ToString().Substring(0, 1), "X")
                            Session("HDay9") = HDay9.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay9.Value.Substring(0, 1) = "M" Or HDay9.Value.Substring(0, 1) = "X") Then
                                HDay9.Value = Replace(Session("HDay9").ToString(), Session("HDay9").ToString().Substring(0, 1), "W")
                                Session("HDay9") = HDay9.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay9.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If
                    If (HDay10.Value <> "" And e.Day.Date.Day.ToString() = "10") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay10.Value)

                        If (HDay10.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay10.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay10.Value.Substring(0, 1) = "M" Or HDay10.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay10.Value = Replace(Session("HDay10").ToString(), Session("HDay10").ToString().Substring(0, 1), "X")
                            Session("HDay10") = HDay10.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay10.Value.Substring(0, 1) = "M" Or HDay10.Value.Substring(0, 1) = "X") Then
                                HDay10.Value = Replace(Session("HDay10").ToString(), Session("HDay10").ToString().Substring(0, 1), "W")
                                Session("HDay10") = HDay10.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay10.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    If (HDay11.Value <> "" And e.Day.Date.Day.ToString() = "11") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay11.Value)

                        If (HDay11.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay11.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay11.Value.Substring(0, 1) = "M" Or HDay11.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay11.Value = Replace(Session("HDay11").ToString(), Session("HDay11").ToString().Substring(0, 1), "X")
                            Session("HDay11") = HDay11.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay11.Value.Substring(0, 1) = "M" Or HDay11.Value.Substring(0, 1) = "X") Then
                                HDay11.Value = Replace(Session("HDay11").ToString(), Session("HDay11").ToString().Substring(0, 1), "W")
                                Session("HDay11") = HDay11.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay11.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    If (HDay12.Value <> "" And e.Day.Date.Day.ToString() = "12") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay12.Value)

                        If (HDay12.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay12.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay12.Value.Substring(0, 1) = "M" Or HDay12.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay12.Value = Replace(Session("HDay12").ToString(), Session("HDay12").ToString().Substring(0, 1), "X")
                            Session("HDay12") = HDay12.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay12.Value.Substring(0, 1) = "M" Or HDay12.Value.Substring(0, 1) = "X") Then
                                HDay12.Value = Replace(Session("HDay12").ToString(), Session("HDay12").ToString().Substring(0, 1), "W")
                                Session("HDay12") = HDay12.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay12.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    If (HDay13.Value <> "" And e.Day.Date.Day.ToString() = "13") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay13.Value)

                        If (HDay13.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay13.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay13.Value.Substring(0, 1) = "M" Or HDay13.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay13.Value = Replace(Session("HDay13").ToString(), Session("HDay13").ToString().Substring(0, 1), "X")
                            Session("HDay13") = HDay13.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay13.Value.Substring(0, 1) = "M" Or HDay13.Value.Substring(0, 1) = "X") Then
                                HDay13.Value = Replace(Session("HDay13").ToString(), Session("HDay13").ToString().Substring(0, 1), "W")
                                Session("HDay13") = HDay13.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay13.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    If (HDay14.Value <> "" And e.Day.Date.Day.ToString() = "14") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay14.Value)

                        If (HDay14.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay14.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay14.Value.Substring(0, 1) = "M" Or HDay14.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay14.Value = Replace(Session("HDay14").ToString(), Session("HDay14").ToString().Substring(0, 1), "X")
                            Session("HDay14") = HDay14.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay14.Value.Substring(0, 1) = "M" Or HDay14.Value.Substring(0, 1) = "X") Then
                                HDay14.Value = Replace(Session("HDay14").ToString(), Session("HDay14").ToString().Substring(0, 1), "W")
                                Session("HDay14") = HDay14.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay14.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    If (HDay15.Value <> "" And e.Day.Date.Day.ToString() = "15") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay15.Value)

                        If (HDay15.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay15.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay15.Value.Substring(0, 1) = "M" Or HDay15.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay15.Value = Replace(Session("HDay15").ToString(), Session("HDay15").ToString().Substring(0, 1), "X")
                            Session("HDay15") = HDay15.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay15.Value.Substring(0, 1) = "M" Or HDay15.Value.Substring(0, 1) = "X") Then
                                HDay15.Value = Replace(Session("HDay15").ToString(), Session("HDay15").ToString().Substring(0, 1), "W")
                                Session("HDay15") = HDay15.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay15.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    If (HDay16.Value <> "" And e.Day.Date.Day.ToString() = "16") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay16.Value)

                        If (HDay16.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay16.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay16.Value.Substring(0, 1) = "M" Or HDay16.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay16.Value = Replace(Session("HDay16").ToString(), Session("HDay16").ToString().Substring(0, 1), "X")
                            Session("HDay16") = HDay16.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay16.Value.Substring(0, 1) = "M" Or HDay16.Value.Substring(0, 1) = "X") Then
                                HDay16.Value = Replace(Session("HDay16").ToString(), Session("HDay16").ToString().Substring(0, 1), "W")
                                Session("HDay16") = HDay16.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay16.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    If (HDay17.Value <> "" And e.Day.Date.Day.ToString() = "17") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay17.Value)

                        If (HDay17.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay17.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay17.Value.Substring(0, 1) = "M" Or HDay17.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay17.Value = Replace(Session("HDay17").ToString(), Session("HDay17").ToString().Substring(0, 1), "X")
                            Session("HDay17") = HDay17.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay17.Value.Substring(0, 1) = "M" Or HDay17.Value.Substring(0, 1) = "X") Then
                                HDay17.Value = Replace(Session("HDay17").ToString(), Session("HDay17").ToString().Substring(0, 1), "W")
                                Session("HDay17") = HDay17.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay17.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    If (HDay18.Value <> "" And e.Day.Date.Day.ToString() = "18") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay18.Value)

                        If (HDay18.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay18.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"

                        ElseIf (HDay18.Value.Substring(0, 1) = "M" Or HDay18.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay18.Value = Replace(Session("HDay18").ToString(), Session("HDay18").ToString().Substring(0, 1), "X")
                            Session("HDay18") = HDay18.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay18.Value.Substring(0, 1) = "M" Or HDay18.Value.Substring(0, 1) = "X") Then
                                HDay18.Value = Replace(Session("HDay18").ToString(), Session("HDay18").ToString().Substring(0, 1), "W")
                                Session("HDay18") = HDay18.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay18.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    If (HDay19.Value <> "" And e.Day.Date.Day.ToString() = "19") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay19.Value)

                        If (HDay19.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay19.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay19.Value.Substring(0, 1) = "M" Or HDay19.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay19.Value = Replace(Session("HDay19").ToString(), Session("HDay19").ToString().Substring(0, 1), "X")
                            Session("HDay19") = HDay19.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay19.Value.Substring(0, 1) = "M" Or HDay19.Value.Substring(0, 1) = "X") Then
                                HDay19.Value = Replace(Session("HDay19").ToString(), Session("HDay19").ToString().Substring(0, 1), "W")
                                Session("HDay19") = HDay19.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay19.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    If (HDay20.Value <> "" And e.Day.Date.Day.ToString() = "20") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay20.Value)

                        If (HDay20.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay20.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay20.Value.Substring(0, 1) = "M" Or HDay20.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay20.Value = Replace(Session("HDay20").ToString(), Session("HDay20").ToString().Substring(0, 1), "X")
                            Session("HDay20") = HDay20.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay20.Value.Substring(0, 1) = "M" Or HDay20.Value.Substring(0, 1) = "X") Then
                                HDay20.Value = Replace(Session("HDay20").ToString(), Session("HDay20").ToString().Substring(0, 1), "W")
                                Session("HDay20") = HDay20.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay20.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    If (HDay21.Value <> "" And e.Day.Date.Day.ToString() = "21") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay21.Value)

                        If (HDay21.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay21.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay21.Value.Substring(0, 1) = "M" Or HDay21.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay21.Value = Replace(Session("HDay21").ToString(), Session("HDay21").ToString().Substring(0, 1), "X")
                            Session("HDay21") = HDay21.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay21.Value.Substring(0, 1) = "M" Or HDay21.Value.Substring(0, 1) = "X") Then
                                HDay21.Value = Replace(Session("HDay21").ToString(), Session("HDay21").ToString().Substring(0, 1), "W")
                                Session("HDay21") = HDay21.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay21.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    If (HDay22.Value <> "" And e.Day.Date.Day.ToString() = "22") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay22.Value)

                        If (HDay22.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay22.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay22.Value.Substring(0, 1) = "M" Or HDay22.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay22.Value = Replace(Session("HDay22").ToString(), Session("HDay22").ToString().Substring(0, 1), "X")
                            Session("HDay22") = HDay22.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay22.Value.Substring(0, 1) = "M" Or HDay22.Value.Substring(0, 1) = "X") Then
                                HDay22.Value = Replace(Session("HDay22").ToString(), Session("HDay22").ToString().Substring(0, 1), "W")
                                Session("HDay22") = HDay22.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay22.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    If (HDay23.Value <> "" And e.Day.Date.Day.ToString() = "23") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay23.Value)

                        If (HDay23.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay23.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay23.Value.Substring(0, 1) = "M" Or HDay23.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay23.Value = Replace(Session("HDay23").ToString(), Session("HDay23").ToString().Substring(0, 1), "X")
                            Session("HDay23") = HDay23.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay23.Value.Substring(0, 1) = "M" Or HDay23.Value.Substring(0, 1) = "X") Then
                                HDay23.Value = Replace(Session("HDay23").ToString(), Session("HDay23").ToString().Substring(0, 1), "W")
                                Session("HDay23") = HDay23.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay23.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    If (HDay24.Value <> "" And e.Day.Date.Day.ToString() = "24") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay24.Value)

                        If (HDay24.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay24.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay24.Value.Substring(0, 1) = "M" Or HDay24.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay24.Value = Replace(Session("HDay24").ToString(), Session("HDay24").ToString().Substring(0, 1), "X")
                            Session("HDay24") = HDay24.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay24.Value.Substring(0, 1) = "M" Or HDay24.Value.Substring(0, 1) = "X") Then
                                HDay24.Value = Replace(Session("HDay24").ToString(), Session("HDay24").ToString().Substring(0, 1), "W")
                                Session("HDay24") = HDay24.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay24.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    If (HDay25.Value <> "" And e.Day.Date.Day.ToString() = "25") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay25.Value)

                        If (HDay25.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay25.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay25.Value.Substring(0, 1) = "M" Or HDay25.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            'HDay25.Value = "X"
                            HDay25.Value = Replace(Session("HDay25").ToString(), Session("HDay25").ToString().Substring(0, 1), "X")
                            Session("HDay25") = HDay25.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay25.Value.Substring(0, 1) = "M" Or HDay25.Value.Substring(0, 1) = "X") Then
                                'HDay25.Value = "W"
                                HDay25.Value = Replace(Session("HDay25").ToString(), Session("HDay25").ToString().Substring(0, 1), "W")
                                Session("HDay25") = HDay25.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay25.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    If (HDay26.Value <> "" And e.Day.Date.Day.ToString() = "26") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay26.Value)

                        If (HDay26.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay26.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay26.Value.Substring(0, 1) = "M" Or HDay26.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay26.Value = Replace(Session("HDay26").ToString(), Session("HDay26").ToString().Substring(0, 1), "X")
                            Session("HDay26") = HDay26.Value

                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay26.Value.Substring(0, 1) = "M" Or HDay26.Value.Substring(0, 1) = "X") Then
                                HDay26.Value = Replace(Session("HDay26").ToString(), Session("HDay26").ToString().Substring(0, 1), "W")
                                Session("HDay26") = HDay26.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay26.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    If (HDay27.Value <> "" And e.Day.Date.Day.ToString() = "27") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay27.Value)

                        If (HDay27.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay27.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay27.Value.Substring(0, 1) = "M" Or HDay27.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay27.Value = Replace(Session("HDay27").ToString(), Session("HDay27").ToString().Substring(0, 1), "X")
                            Session("HDay27") = HDay27.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay27.Value.Substring(0, 1) = "M" Or HDay27.Value.Substring(0, 1) = "X") Then
                                HDay27.Value = Replace(Session("HDay27").ToString(), Session("HDay27").ToString().Substring(0, 1), "W")
                                Session("HDay27") = HDay27.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay27.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    If (HDay28.Value <> "" And e.Day.Date.Day.ToString() = "28") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay28.Value)

                        If (HDay28.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay28.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay28.Value.Substring(0, 1) = "M" Or HDay28.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay28.Value = Replace(Session("HDay28").ToString(), Session("HDay28").ToString().Substring(0, 1), "X")
                            Session("HDay28") = HDay28.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay28.Value.Substring(0, 1) = "M" Or HDay28.Value.Substring(0, 1) = "X") Then
                                HDay28.Value = Replace(Session("HDay28").ToString(), Session("HDay28").ToString().Substring(0, 1), "W")
                                Session("HDay28") = HDay28.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay28.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    If (HDay29.Value <> "" And e.Day.Date.Day.ToString() = "29") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay29.Value)

                        If (HDay29.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay29.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay29.Value.Substring(0, 1) = "M" Or HDay29.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay29.Value = Replace(Session("HDay29").ToString(), Session("HDay29").ToString().Substring(0, 1), "X")
                            Session("HDay29") = HDay29.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay29.Value.Substring(0, 1) = "M" Or HDay29.Value.Substring(0, 1) = "X") Then
                                HDay29.Value = Replace(Session("HDay29").ToString(), Session("HDay29").ToString().Substring(0, 1), "W")
                                Session("HDay29") = HDay29.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay29.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    If (HDay30.Value <> "" And e.Day.Date.Day.ToString() = "30") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay30.Value)

                        If (HDay30.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay30.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay30.Value.Substring(0, 1) = "M" Or HDay30.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay30.Value = Replace(Session("HDay30").ToString(), Session("HDay30").ToString().Substring(0, 1), "X")
                            Session("HDay30") = HDay30.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay30.Value.Substring(0, 1) = "M" Or HDay30.Value.Substring(0, 1) = "X") Then
                                HDay30.Value = Replace(Session("HDay30").ToString(), Session("HDay30").ToString().Substring(0, 1), "W")
                                Session("HDay30") = HDay30.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay30.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    If (HDay31.Value <> "" And e.Day.Date.Day.ToString() = "31") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay31.Value)

                        If (HDay31.Value.Substring(0, 1) = "O") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#F3DF80"
                            CellUrl = "X"
                        ElseIf (HDay31.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#A9D7FC"
                            CellUrl = "X"
                        ElseIf (HDay31.Value.Substring(0, 1) = "M" Or HDay31.Value.Substring(0, 1) = "X") Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                            HDay31.Value = Replace(Session("HDay31").ToString(), Session("HDay31").ToString().Substring(0, 1), "X")
                            Session("HDay31") = HDay31.Value
                        End If
                        If (IsOffDay(e.Day.Date)) Then
                            IsWeekEnd = True
                            If Not (HDay31.Value.Substring(0, 1) = "M" Or HDay31.Value.Substring(0, 1) = "X") Then
                                HDay31.Value = Replace(Session("HDay31").ToString(), Session("HDay31").ToString().Substring(0, 1), "W")
                                Session("HDay31") = HDay31.Value
                            End If
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay31.Value)
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        e.Cell.Text = CellText
                    End If

                    'Weekend Setting
                    If IsWeekEnd = False Then
                        If IsOffDay(e.Day.Date) Then
                            e.Cell.BackColor = System.Drawing.Color.FromName("#A9D7FC")
                            Dtype = "W"
                        End If
                    End If
                    If e.Cell.BackColor = System.Drawing.Color.FromName("#EEEEEE") And (IsOffDay(e.Day.Date)) Then
                        Dtype = "M"
                    End If
                    If e.Cell.BackColor = System.Drawing.Color.FromName("#A9D7FC") And (IsOffDay(e.Day.Date)) Then
                        Dtype = "W"
                    End If
                    If e.Cell.BackColor = System.Drawing.Color.FromName("#EEEEEE") And Not (IsOffDay(e.Day.Date)) Then
                        Dtype = "X"
                    End If

                    e.Cell.Style.Add("cursor", "Pointer")
                    If e.Day.Date > Now.Date Then
                        e.Cell.Style.Add("cursor", "Pointer")
                        CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('POP_DefaultPlan.aspx?Mode=MODIFY&IRS=N&IsRefresh=N&dayref=" & e.Day.Date.Day & "&DType=" & Dtype & "','CustomerList','width=450,height=180')")
                    Else
                        e.Cell.Style.Add("cursor", "Cursor")
                    End If

                Else
                    e.Cell.Visible = False
                    If ((e.Day.Date < StartDate And (DateDiff(DateInterval.WeekOfYear, e.Day.Date, StartDate) = 0)) Or (e.Day.Date > EndDate And (DateDiff(DateInterval.WeekOfYear, e.Day.Date, EndDate)) = 0)) Then
                        e.Cell.Visible = True
                    End If
                    e.Cell.Text = ""
                    e.Cell.ToolTip = ""

                End If
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("Information.aspx?mode=1&errno=" & "74010" & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=DefaultPlan.aspx")
        End Try
    End Sub

    Protected Sub ShowAndRefreshBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ShowAndRefreshBtn.Click
        System.Threading.Thread.Sleep(1000)
        Try

            If (Page.IsValid) Then

                Dim SDate As String = Start_Date.Text.Trim()
                Dim EDate As String = End_Date.Text.Trim()

                StartDate = Convert.ToDateTime(SDate.Substring(3, 3) & SDate.Substring(0, 3) & SDate.Substring(6, 4))
                EndDate = Convert.ToDateTime(EDate.Substring(3, 3) & EDate.Substring(0, 3) & EDate.Substring(6, 4))

                If (Not StartDate > EndDate And StartDate.Month = EndDate.Month) Then

                    ObjRoutePlan = New RoutePlan()
                    With ObjRoutePlan
                        .StartDate = StartDate
                        .EndDate = EndDate
                        .Site = Session("Site")
                    End With
                    If ObjRoutePlan.IsValidDateRange() Then
                        CalPanel.Visible = True
                        FromPopUp.Value = "N"
                        ISRefreskClick.Value = "Y"
                        Session("Start_Date") = Start_Date.Text.Trim()
                        Session("End_Date") = End_Date.Text.Trim()
                        DefPlanCalendar.VisibleDate = StartDate
                        Action_Mode.Value = "ADD"
                        SetPanelVisibility(True, False)
                        SaveBtn.Enabled = True

                    Else
                        Response.Redirect("information.aspx?mode=1&msg=A+plan+with+the+specified+date+range+already+exists&next=DefaultPlan.aspx?ret=1", False)
                    End If
                Else
                    CalPanel.Visible = False
                    Response.Redirect("information.aspx?mode=1&msg=Start+Date+should+not+be+greater+than+End+Date&next=DefaultPlan.aspx?ret=1", False)
                End If
            End If
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & "74008" & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=DefaultPlan.aspx")
        End Try
    End Sub
    Private Function SetText(ByVal CellDate As String, ByVal InnerText As String) As String
        Dim dat As Array = CellDate.Split("-")
        Dim StrBuilder As New StringBuilder
        If (Len(InnerText) < 3) Then
            SetText = "<span class='dateCalendarCntrl'>" & dat(0) & "</span>"
        Else

            Dim stringArr As Array = InnerText.Split("|")
            Dim i As Integer

            For i = 1 To (stringArr.Length - 1)
                Dim Str As String = Convert.ToString(stringArr(i))
                If (i = 1) Then
                    If (Str.Length > 20) Then
                        StrBuilder.Append("<span class='dateCalendarCntrl'>" & dat(0) & "</span>" & "<br/>" & "<span class='visitDtlsCalendarCntrl'>" & Str.Substring(0, 15) & "..." & " </span>")
                    Else
                        StrBuilder.Append("<span class='dateCalendarCntrl'>" & dat(0) & "</span>" & "<br/>" & "<span class='visitDtlsCalendarCntrl'>" & Str & " </span>")
                    End If
                ElseIf (i = 2) Then
                    StrBuilder.Append("<span class='visitDtlsCalendarCntrl'> at " & Str.Substring(0, 5))
                ElseIf (i = 3) Then
                    StrBuilder.Append(" To " & Str.Substring(0, 5) & "</span>")
                End If

            Next
            SetText = Convert.ToString(StrBuilder)
        End If
    End Function
    Private Function SetAmPm(ByVal Time As String) As String
        If (Time.Substring(0, 2) = "12" And Time.Substring(3, 2) >= "00") Then
            SetAmPm = "12:" & Time.Substring(3, 2) & " p.m."
        ElseIf (Time.Substring(0, 2) > "12") Then
            SetAmPm = Convert.ToInt32(Time.Substring(0, 2)) - 12 & ":" & Time.Substring(3, 2) & " p.m."
        Else
            SetAmPm = Time & " a.m."
        End If
    End Function
    Private Function SetToolTip(ByVal CellDate As String, ByVal InnerText As String) As String
        Dim StrBuilder As New StringBuilder
        If (Len(InnerText) < 3) Then
            SetToolTip = CellDate
        Else

            Dim stringArr As Array = InnerText.Split("|")
            Dim i As Integer

            For i = 1 To (stringArr.Length - 1)
                Dim Str As String = Convert.ToString(stringArr(i))
                If (i = 1) Then
                    StrBuilder.Append(CellDate & vbCrLf & Str)
                ElseIf (i = 2) Then
                    StrBuilder.Append(" at " & Str.Substring(0, 5))
                ElseIf (i = 3) Then
                    StrBuilder.Append(" To " & Str.Substring(0, 5))
                End If

            Next
            SetToolTip = Convert.ToString(StrBuilder)

        End If

    End Function

    Private Sub ClearSession()
        For i As Integer = 0 To 31
            Session("HDay" & i & "") = Nothing
        Next

    End Sub

    Private Sub CancelBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelBtn.Click
        ClearSession()
        Response.Redirect("DefaultPlan.aspx")
    End Sub

    Private Sub CancelBtn1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelBtn1.Click
        ClearSession()
        Response.Redirect("DefaultPlan.aspx")
    End Sub
    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
     
        If (FromPopUp.Value = "Y" And Not ISRefreskClick.Value = "Y") Then
            Start_Date.Text = Session("Start_Date")
            End_Date.Text = Session("End_Date")
            StartDate = Convert.ToDateTime(Session("Start_Date").Substring(3, 3) & Session("Start_Date").Substring(0, 3) & Session("Start_Date").Substring(6, 4))
            EndDate = Convert.ToDateTime(Session("End_Date").Substring(3, 3) & Session("End_Date").Substring(0, 3) & Session("End_Date").Substring(6, 4))
            ISRefreskClick.Value = "Y"
        End If

        If Action_Mode.Value = "MODIFY" Or Action_Mode.Value = "UPDATE" Then
            SetPanelVisibility(False, True)
            ShowAndRefreshBtn.Enabled = False
            ObjRoutePlan = New RoutePlan
            Try
                ObjRoutePlan.DefPlanDetailID = RP_ID.Value
                If (ObjRoutePlan.IsPlanUsed(Err_No, Err_Desc)) Then
                    UpdateBtn.Enabled = False
                End If
                If (Err_Desc Is Nothing) Then
                    ObjRoutePlan.GetDefaultPlan(Err_No, Err_Desc)
                    Start_Date.Enabled = False
                    End_Date.Enabled = False
                    Description.Enabled = False

                    Description.Text = ObjRoutePlan.Description
                    Dim TemSDate As Date = ObjRoutePlan.SDate
                    Start_Date.Text = IIf(CInt(TemSDate.Day) < 10, "0" & TemSDate.Day, TemSDate.Day) & "-" & IIf(CInt(TemSDate.Month) < 10, "0" & TemSDate.Month, TemSDate.Month) & "-" & TemSDate.Year
                    Dim TemEDate As Date = ObjRoutePlan.EDate
                    End_Date.Text = IIf(CInt(TemEDate.Day) < 10, "0" & TemEDate.Day, TemEDate.Day) & "-" & IIf(CInt(TemEDate.Month) < 10, "0" & TemEDate.Month, TemEDate.Month) & "-" & TemEDate.Year

                    StartDate = Convert.ToDateTime(Start_Date.Text.Substring(3, 3) & Start_Date.Text.Substring(0, 3) & Start_Date.Text.Substring(6, 4))
                    EndDate = Convert.ToDateTime(End_Date.Text.Substring(3, 3) & End_Date.Text.Substring(0, 3) & End_Date.Text.Substring(6, 4))

                    If (Err_Desc Is Nothing) Then
                        CalDataSet = ObjRoutePlan.GetDefaultCalendarDetails(Err_No, Err_Desc)
                        If (CalDataSet.Tables("DefaultPlan").Rows.Count() > 0) Then
                            CalPanel.Visible = True
                            Session("Start_Date") = Start_Date.Text.Trim()
                            Session("End_Date") = End_Date.Text.Trim()
                            DefPlanCalendar.VisibleDate = CDate(StartDate)
                            Action_Mode.Value = "MODIFY"
                            StartDateRegEx.Display = Web.UI.WebControls.ValidatorDisplay.None
                            ValidStartDateReq.Enabled = False
                            ValidatorCalloutExtender1.Enabled = False
                        End If
                    End If
                Else
                    log.Error(Err_Desc)
                    Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=DefaultPlan.aspx&Title=Route+Planner")
                End If

            Catch ex As Exception
                log.Error(GetExceptionInfo(ex))
                Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=ViewDefPlanNew.aspxDefaultPlan.aspx&Title=Route+Planner")
            Finally
                ObjRoutePlan = Nothing
            End Try
        End If
    End Sub

    Protected Sub Start_Date_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Start_Date.TextChanged
        Const REGULAR_EXP = "^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"

        If Me.Start_Date.Text <> "" Then
            'If IsDate(Me.Start_Date.Text) Then
            If Not Regex.IsMatch(Start_Date.Text, REGULAR_EXP) Then
                Start_Date.Focus()
                Exit Sub
            End If
            Me.End_Date.Text = ""
            DefPlanCalendar.Dispose()
            CalPanel.Visible = False
            Dim sdate() As String = Me.Start_Date.Text.Split("/")
            Dim m As Integer = Integer.Parse(sdate(1))
            Dim y As Integer = Integer.Parse(sdate(2))
            Dim sd As Integer = Integer.Parse(sdate(0))
            Dim stDate As DateTime = DateTime.Parse(y & "/" & m & "/01")
            Dim d As Integer = System.DateTime.DaysInMonth(y, m)
            If sd <> 1 Then
                Me.Start_Date.Text = DateTime.Parse(y & "/" & m & "/01").ToString("dd/MM/yyyy")
                stDate = DateTime.Parse(y & "/" & m & "/01")
            End If
            Me.End_Date.Text = DateTime.Parse(stDate).AddDays(d - 1).ToString("dd/MM/yyyy")

            'End If
        End If
    End Sub
End Class