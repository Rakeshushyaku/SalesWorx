Imports SalesWorx.BO.Common
Imports System.Collections
Imports System.Data
Imports System.Text
Imports System.Drawing
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports System.Web
Imports System.Resources
Imports log4net

Partial Public Class AdminRoutePlan
    Inherits System.Web.UI.Page
    ' Dim Action_Mode As String

    'def plan vars......
    ' Dim RP_ID As String
    Dim StartDate As Date
    Dim EndDate As Date

    'route plan vars....
    Dim Route_ID As String
    Dim SalesRep_ID As String
    Dim ApprovedBy As String
    Dim ApprovedOn As String


    Dim UPD_BTN_STAT As String
    Dim APP_BTN_STAT As String
    Dim REJ_BTN_STAT As String

    'helper debug vars.....
    Dim objRecords As ArrayList
    Dim Err_No As Long
    Dim Err_Desc As String

    'default plan obj.......
    Dim objDefPlan As RoutePlan
    Dim objRoutePlan As RoutePlan
    Dim CalDataSet As DataSet
    Dim FSRDataSet As DataSet
    Dim ErrorResource As ResourceManager
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim VCount As Integer = 0
    Dim dtHoliday As DataTable
    Private Sub AdminRoutePlan_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Route Planner"
    End Sub
    Public Sub New()
        ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If IsNothing(Session("USER_ACCESS")) Then
        '    Response.Redirect("Login.aspx")
        'End If
        SendCommentsBtn.Attributes.Add("Onclick", "return CheckEmpty()")
        objRoutePlan = New RoutePlan
        If (Not IsPostBack) Then
            sp.Visible = False
            Session.Remove("CustomerList")
            Action_Mode.Value = Request.QueryString("Mode")
            RP_ID.Value = Session("Default_Plan_ID")
            FSR_ID.Value = Session("FSR_ID")
            IsApproved.Value = Session("ISApproved")
            objRoutePlan.SalesRepID = Session("SalesRep_ID")
            '    CommentsUpdatePanel.Visible = objRoutePlan.SetCommentPanelVisibility()
            CommentsUpdatePanel.Visible = False
            CheckRefresh.Value = "N"
            FromPopUp.Value = "N"
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

        End If
        If (Action_Mode.Value = "MODIFY" Or Action_Mode.Value = "APPROVE" Or Action_Mode.Value = "REVIEW") Then
            objRoutePlan.FSRPlanId = FSR_ID.Value
            FSRDataSet = objRoutePlan.GetFSRDetails(Err_No, Err_Desc)
            LnkPanel.Visible = True
            ShowComments()
        End If

        If Action_Mode.Value = "" Or Action_Mode.Value = "ADD" Or Action_Mode.Value = "SAVE" Then
            SaveBtn.Visible = True
            HeaderLbl.Text = "Create Route Plan"
        ElseIf Action_Mode.Value = "MODIFY" Or Action_Mode.Value = "UPDATE" Then
            HeaderLbl.Text = "Edit Route Plan"
        ElseIf Action_Mode.Value = "APPROVAL" Or Action_Mode.Value = "APPROVE" Or Action_Mode.Value = "REJECT" Then
            HeaderLbl.Text = "Approve Route Plan"
        ElseIf Action_Mode.Value = "REVIEW" Then
            HeaderLbl.Text = "Review Route Plan"
        End If

        ''Setting SalesRep Panel Visible according to the Mode
        ' If (Action_Mode.Value = "APPROVAL" Or Action_Mode.Value = "APPROVE") And (Not IsNothing(SalesRep_Name.Text.Trim())) Then
        SalesRepPanel.Visible = True
        SalesRep_Name.Text = Session("SalesRep_Name")
        'Else
        ' SalesRepPanel.Visible = False
        '  End If

        ''Setting Button Panel Visible
        If Action_Mode.Value = "ADD" Or Action_Mode.Value = "SAVE" Then
            SetPanelVisibility(True, False, False, False)
        ElseIf Action_Mode.Value = "MODIFY" Or Action_Mode.Value = "UPDATE" Then
            SetPanelVisibility(False, True, False, False)
        ElseIf Action_Mode.Value = "APPROVAL" Or Action_Mode.Value = "APPROVE" Then
            If (Session("ApproveStat") = "Y" Or IsApproved.Value = "Y") Then
                ApprovalBtn.Enabled = False
            End If
            Session("ApproveStat") = Nothing
            Session("ISApproved") = Nothing
            CommentsUpdatePanel.Visible = True
            SetPanelVisibility(False, False, True, False)
        ElseIf Action_Mode.Value = "REVIEW" Then
            SetPanelVisibility(False, False, False,true)
        End If
        Session("ApproveStat") = Nothing
        Session("ISApproved") = Nothing
        Try
            'If Not (Action_Mode.Value = "APPROVAL" Or Action_Mode.Value = "APPROVE") Then
            objRoutePlan.DefPlanId = RP_ID.Value
            objRoutePlan.GetDefaultPlan(Err_No, Err_Desc)
            'End If

            Description.Text = objRoutePlan.Description
            Dim TemSDate As Date = objRoutePlan.SDate()
            Start_Date.Text = IIf(CInt(TemSDate.Day) < 10, "0" & TemSDate.Day, TemSDate.Day) & "-" & IIf(CInt(TemSDate.Month) < 10, "0" & TemSDate.Month, TemSDate.Month) & "-" & TemSDate.Year
            Dim TemEDate As Date = objRoutePlan.EDate
            End_Date.Text = IIf(CInt(TemEDate.Day) < 10, "0" & TemEDate.Day, TemEDate.Day) & "-" & IIf(CInt(TemEDate.Month) < 10, "0" & TemEDate.Month, TemEDate.Month) & "-" & TemEDate.Year


            StartDate = Convert.ToDateTime(Start_Date.Text.Substring(3, 3) & Start_Date.Text.Substring(0, 3) & Start_Date.Text.Substring(6, 4))
            EndDate = Convert.ToDateTime(End_Date.Text.Substring(3, 3) & End_Date.Text.Substring(0, 3) & End_Date.Text.Substring(6, 4))
            If (Err_Desc Is Nothing) Then
                CalDataSet = objRoutePlan.GetDefaultCalendarDetails_new(Err_No, Err_Desc)
                If (CalDataSet.Tables("DefaultPlan").Rows.Count() > 0) Then
                    CalPanel.Visible = True
                    Session("Start_Date") = Start_Date.Text.Trim()
                    Session("End_Date") = End_Date.Text.Trim()
                    DefPlanCalendar.VisibleDate = CDate(StartDate)
                End If
            End If

            '' Getting the holiday list
            dtHoliday = New DataTable()
            dtHoliday.Rows.Clear()
            dtHoliday = objRoutePlan.GetHolidays(TemSDate.Month, TemSDate.Year)

        Catch ex As Exception
            Err_No = "74018"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=CreateRoutePlan.aspx&Title=Route+Planner", False)
            Response.End()

        End Try
        objRoutePlan = Nothing
    End Sub
    Private Sub DefPlanCalendar_DayRender(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DayRenderEventArgs) Handles DefPlanCalendar.DayRender
        Try

            If (Action_Mode.Value = "ADD") Then
                Dim SDate As String = Start_Date.Text.Trim()
                Dim EDate As String = End_Date.Text.Trim()
                Dim CellText As String


                StartDate = Convert.ToDateTime(SDate.Substring(3, 3) & SDate.Substring(0, 3) & SDate.Substring(6, 4))
                EndDate = Convert.ToDateTime(EDate.Substring(3, 3) & EDate.Substring(0, 3) & EDate.Substring(6, 4))
                Dim CalendarCell As TableCell

                If (EndDate.Day >= e.Day.Date.Day And e.Day.Date.Day >= StartDate.Day And e.Day.Date.Month = StartDate.Month And e.Day.Date.Month = EndDate.Month) Then

                    Dim DefaultPlanDetailID As Integer
                    Dim DefDayType As Char
                    Dim CanVisit As Char

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
                        Str.Append("@" & Convert.ToDateTime(foundRows(0).Item(3)).TimeOfDay.ToString())
                    End If
                    If Not (foundRows(0).Item(4) Is DBNull.Value) Then
                        Str.Append("$" & Convert.ToDateTime(foundRows(0).Item(4)).TimeOfDay.ToString())
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
                    CalendarCell = e.Cell
                    e.Cell.Style.Add("cursor", "Pointer")

                    If e.Day.Date > Now.Date Then
                        e.Cell.Text = "<a href='javascript:void(0);'  style='color: #000000;' onClick=window.open('_POP_CustomerListingNew.aspx?Mode=ADD&IRS=N&IsRefresh=N&dayref=" & e.Day.Date.Day & "&DType=X','CustomerList','width=1024,height=600,scrollbars=yes');>" & e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year & " </a >"
                        'e.Cell.Text = "<a href='javascript:void(0);'  style='color: #000000;' onClick=window.open('Test.aspx?Mode=ADD&IRS=N&IsRefresh=N&dayref=" & e.Day.Date.Day & "&DType=X','CustomerList','width=1024,height=600,scrollbars=yes');>" & e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year & " </a >"

                    End If
                    e.Cell.ToolTip = e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year
                    e.Cell.BackColor = System.Drawing.Color.FromName("#EEEEEE")
                    e.Cell.HorizontalAlign = Web.UI.WebControls.HorizontalAlign.Left

                    Dim CellUrl As String
                    Dim CellBgColor As String = "#EEEEEE"
                    If (HDay1.Value <> "" And e.Day.Date.Day.ToString() = "1") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, Session("HDay1"), Str.ToString())
                        If (HDay1.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay1.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay1.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay1.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay1.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, Session("HDay1"), Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1020,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(1) & ")")
                                'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(1) & ")")

                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If

                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If


                    If (HDay2.Value <> "" And e.Day.Date.Day.ToString() = "2") Then

                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay2.Value, Str.ToString())
                        If (HDay2.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay2.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay2.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay2.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay2.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay2.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay3.Value <> "" And e.Day.Date.Day.ToString() = "3") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay3.Value, Str.ToString())
                        If (HDay3.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay3.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay3.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay3.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay3.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay3.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay4.Value <> "" And e.Day.Date.Day.ToString() = "4") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay4.Value, Str.ToString())
                        If (HDay4.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay4.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay4.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay4.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay4.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If


                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay4.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ' ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                ' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay5.Value <> "" And e.Day.Date.Day.ToString() = "5") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay5.Value, Str.ToString())
                        If (HDay5.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay5.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay5.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay5.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay5.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay5.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                ' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay6.Value <> "" And e.Day.Date.Day.ToString() = "6") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay6.Value, Str.ToString())
                        If (HDay6.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay6.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay6.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay6.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay6.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay6.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                ' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay7.Value <> "" And e.Day.Date.Day.ToString() = "7") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay7.Value, Str.ToString())
                        If (HDay7.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay7.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay7.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay7.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay7.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay7.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''   CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                ' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay8.Value <> "" And e.Day.Date.Day.ToString() = "8") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay8.Value, Str.ToString())
                        If (HDay8.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay8.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay8.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay8.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay8.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay8.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                ' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay9.Value <> "" And e.Day.Date.Day.ToString() = "9") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay9.Value, Str.ToString())
                        If (HDay9.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay9.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay9.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay9.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay9.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay9.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                ' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If
                    If (HDay10.Value <> "" And e.Day.Date.Day.ToString() = "10") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay10.Value, Str.ToString())
                        If (HDay10.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay10.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay10.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay10.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay10.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay10.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay11.Value <> "" And e.Day.Date.Day.ToString() = "11") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay11.Value, Str.ToString())
                        If (HDay11.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay11.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay11.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay11.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay11.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay11.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay12.Value <> "" And e.Day.Date.Day.ToString() = "12") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay12.Value, Str.ToString())
                        If (HDay12.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay12.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay12.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay12.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay12.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay12.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay13.Value <> "" And e.Day.Date.Day.ToString() = "13") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay13.Value, Str.ToString())
                        If (HDay13.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay13.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay13.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay13.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay13.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay13.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                ' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay14.Value <> "" And e.Day.Date.Day.ToString() = "14") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay14.Value, Str.ToString())
                        If (HDay14.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay14.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay14.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay14.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay14.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay14.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay15.Value <> "" And e.Day.Date.Day.ToString() = "15") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay15.Value, Str.ToString())
                        If (HDay15.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay15.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay15.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay15.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay15.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay15.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                ' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay16.Value <> "" And e.Day.Date.Day.ToString() = "16") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay16.Value, Str.ToString())
                        If (HDay16.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay16.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay16.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay16.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay16.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay16.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                ' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay17.Value <> "" And e.Day.Date.Day.ToString() = "17") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay17.Value, Str.ToString())
                        If (HDay17.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay17.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay17.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay17.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay17.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay17.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                ' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay18.Value <> "" And e.Day.Date.Day.ToString() = "18") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay18.Value, Str.ToString())
                        If (HDay18.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay18.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay18.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay18.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay18.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay18.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay19.Value <> "" And e.Day.Date.Day.ToString() = "19") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay19.Value, Str.ToString())
                        If (HDay19.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay19.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay19.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay19.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay19.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay19.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay20.Value <> "" And e.Day.Date.Day.ToString() = "20") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay20.Value, Str.ToString())
                        If (HDay20.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay20.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay20.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay20.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay20.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay20.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay21.Value <> "" And e.Day.Date.Day.ToString() = "21") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay21.Value, Str.ToString())
                        If (HDay21.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay21.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay21.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay21.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay21.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay21.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay22.Value <> "" And e.Day.Date.Day.ToString() = "22") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay22.Value, Str.ToString())
                        If (HDay22.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay22.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay22.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay22.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay22.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay22.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay23.Value <> "" And e.Day.Date.Day.ToString() = "23") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay23.Value, Str.ToString())
                        If (HDay23.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay23.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay23.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay23.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay23.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay23.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                ' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay24.Value <> "" And e.Day.Date.Day.ToString() = "24") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay24.Value, Str.ToString())
                        If (HDay24.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay24.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay24.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay24.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay24.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay24.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay25.Value <> "" And e.Day.Date.Day.ToString() = "25") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay25.Value, Str.ToString())
                        If (HDay25.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay25.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay25.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay25.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay25.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay25.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                ' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay26.Value <> "" And e.Day.Date.Day.ToString() = "26") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay26.Value, Str.ToString())
                        If (HDay26.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay26.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay26.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay26.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay26.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay26.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay27.Value <> "" And e.Day.Date.Day.ToString() = "27") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay27.Value, Str.ToString())
                        If (HDay27.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay27.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay27.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay27.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay27.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay27.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay28.Value <> "" And e.Day.Date.Day.ToString() = "28") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay28.Value, Str.ToString())
                        If (HDay28.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"

                        ElseIf (HDay28.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay28.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay28.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay28.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay28.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay29.Value <> "" And e.Day.Date.Day.ToString() = "29") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay29.Value, Str.ToString())
                        If (HDay29.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay29.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay29.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay29.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay29.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay29.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay30.Value <> "" And e.Day.Date.Day.ToString() = "30") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay30.Value, Str.ToString())
                        If (HDay30.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay30.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay30.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay30.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay30.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay30.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay31.Value <> "" And e.Day.Date.Day.ToString() = "31") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay31.Value, Str.ToString())
                        If (HDay31.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay31.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay31.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay31.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay31.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If

                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay31.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If e.Day.Date > Now.Date Then
                                ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                e.Cell.Style.Add("cursor", "Pointer")
                            Else
                                e.Cell.Style.Add("cursor", "Cursor")
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If
                    If e.Day.Date = Now.Date Then
                        e.Cell.BackColor = System.Drawing.Color.Honeydew
                        sp.Visible = True
                    End If

                Else
                    e.Cell.Visible = False
                    If ((e.Day.Date < StartDate And (DateDiff(DateInterval.WeekOfYear, e.Day.Date, StartDate) = 0)) Or (e.Day.Date > EndDate And (DateDiff(DateInterval.WeekOfYear, e.Day.Date, EndDate)) = 0)) Then
                        e.Cell.Visible = True
                    End If
                    e.Cell.Text = ""
                    e.Cell.ToolTip = ""
                    e.Cell.ToolTip.Remove((0), e.Cell.ToolTip.Length())

                End If

            End If
            If (Action_Mode.Value = "MODIFY") Then
                ' Try
                Dim SDate As String = Start_Date.Text.Trim()
                Dim EDate As String = End_Date.Text.Trim()
                Dim CellText As String


                StartDate = Convert.ToDateTime(SDate.Substring(3, 3) & SDate.Substring(0, 3) & SDate.Substring(6, 4))
                EndDate = Convert.ToDateTime(EDate.Substring(3, 3) & EDate.Substring(0, 3) & EDate.Substring(6, 4))
                Dim CalendarCell As TableCell

                If (StartDate.Month < Now.Month And StartDate.Year <= Now.Year) Then
                    UpdateBtn.Enabled = False
                End If

                If (EndDate.Day >= e.Day.Date.Day And e.Day.Date.Day >= StartDate.Day And e.Day.Date.Month = StartDate.Month And e.Day.Date.Month = EndDate.Month) Then

                    Dim DefaultPlanDetailID As Integer
                    Dim DefDayType As Char
                    Dim CanVisit As Char

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
                        Str.Append("@" & Convert.ToDateTime(foundRows(0).Item(3)).TimeOfDay.ToString())
                    End If
                    If Not (foundRows(0).Item(4) Is DBNull.Value) Then
                        Str.Append("$" & Convert.ToDateTime(foundRows(0).Item(4)).TimeOfDay.ToString())
                    End If

                    Dim foundRouteRows() As DataRow = FSRDataSet.Tables("FSRPlan").Select("Day=" & e.Day.Date.Day & "")
                    Dim StrRoute As New StringBuilder
                    StrRoute.Append("")
                    If (foundRouteRows.Length > 0) Then

                        StrRoute.Append(foundRouteRows(0).Item(7))
                        If (foundRouteRows.Length = 1) Then
                            If Not (foundRouteRows(0).Item(8) Is DBNull.Value) Then
                                StrRoute.Append("|" & foundRouteRows(0).Item(8))
                            Else
                                If (foundRouteRows(0).Item(7) = "V") Then
                                    Dim i As Integer = 0
                                    StrRoute.Append("|")
                                    For Each dr As DataRow In foundRouteRows
                                       

                                        objRoutePlan = New RoutePlan
                                        'objRoutePlan.SalesRepIDprop = Session("USER_ACCESS").SalesRepID()
                                        objRoutePlan.SalesRepID = Session("SalesRep_ID")
                                        Cache.Remove("CustomerList")
                                        If Session("CustomerList") Is Nothing Then
                                            Session("CustomerList") = objRoutePlan.GetCustomerList(Err_No, Err_Desc)
                                        End If

                                        Dim results() As DataRow = CType(Session("CustomerList"), DataTable).Select("Customer_ID=" & dr.Item(3) & " and Site_Use_ID=" & dr.Item(4) & "")

                                        If (results.Length > 0) Then
                                            If Not (dr.Item(3) Is DBNull.Value) Then
                                                StrRoute.Append("|" & dr.Item(3))
                                            End If

                                            StrRoute.Append("^" & results(0).Item(2))
                                            If Not (dr.Item(4) Is DBNull.Value) Then
                                                StrRoute.Append("^" & dr.Item(4))
                                            Else
                                                StrRoute.Append("^" & "")
                                            End If

                                            If Not (dr.Item("Visit_Sequence") Is DBNull.Value) Then
                                                StrRoute.Append("^" & dr.Item("Visit_Sequence"))
                                            Else
                                                StrRoute.Append("^" & "")
                                            End If
                                            If Not (dr.Item("Allow_Optimization") Is DBNull.Value) Then
                                                StrRoute.Append("^" & dr.Item("Allow_Optimization"))
                                            Else
                                                StrRoute.Append("^" & dr.Item("Allow_Optimization"))
                                            End If
                                            StrRoute.Append("^" & results(0).Item("Customer_No"))

                                            If Not (dr.Item(5) Is DBNull.Value) Then
                                                StrRoute.Append("@" & Convert.ToDateTime(dr.Item(5)).TimeOfDay.ToString())
                                            End If
                                            If Not (dr.Item(6) Is DBNull.Value) Then
                                                StrRoute.Append("$" & Convert.ToDateTime(dr.Item(6)).TimeOfDay.ToString())
                                            End If
                                        End If

                                        'If (results.Length = 0) Then
                                        '    StrRoute.Append("^N/A^N/A")
                                        'End If

                                        'If (results.Length = 0) Then
                                        '    StrRoute.Append("^" & "")
                                        'Else
                                        '    StrRoute.Append("^" & results(0).Item("Customer_No"))
                                        'End If


                                        
                                    Next
                                Else
                                    StrRoute.Append("|*")
                                End If

                            End If
                        Else
                            Dim i As Integer = 0
                            If Not (foundRouteRows(0).Item(8) Is DBNull.Value) Then
                                StrRoute.Append("|" & foundRouteRows(0).Item(8))
                            Else
                                StrRoute.Append("|")
                            End If
                            For Each dr As DataRow In foundRouteRows
                               

                                objRoutePlan = New RoutePlan
                                'objRoutePlan.SalesRepIDprop = Session("USER_ACCESS").SalesRepID()
                                objRoutePlan.SalesRepID = Session("SalesRep_ID")
                                Cache.Remove("CustomerList")
                                If Session("CustomerList") Is Nothing Then
                                    Session("CustomerList") = objRoutePlan.GetCustomerList(Err_No, Err_Desc)
                                End If

                                Dim results() As DataRow = CType(Session("CustomerList"), DataTable).Select("Customer_ID=" & dr.Item(3) & " and Site_Use_ID=" & dr.Item(4) & "")

                                If (results.Length > 0) Then
                                    If Not (dr.Item(3) Is DBNull.Value) Then
                                        StrRoute.Append("|" & dr.Item(3))
                                    End If

                                    StrRoute.Append("^" & results(0).Item(2))

                                    If Not (dr.Item(4) Is DBNull.Value) Then
                                        StrRoute.Append("^" & dr.Item(4))
                                    Else
                                        StrRoute.Append("^" & "")
                                    End If

                                    If Not (dr.Item("Visit_Sequence") Is DBNull.Value) Then
                                        StrRoute.Append("^" & dr.Item("Visit_Sequence"))
                                    Else
                                        StrRoute.Append("^" & "")
                                    End If


                                    If Not (dr.Item("Allow_Optimization") Is DBNull.Value) Then
                                        StrRoute.Append("^" & dr.Item("Allow_Optimization"))
                                    Else
                                        StrRoute.Append("^" & dr.Item("Allow_Optimization"))
                                    End If

                                    StrRoute.Append("^" & results(0).Item("Customer_No"))
                                    If Not (dr.Item(5) Is DBNull.Value) Then
                                        StrRoute.Append("@" & Convert.ToDateTime(dr.Item(5)).TimeOfDay.ToString())
                                    End If
                                    If Not (dr.Item(6) Is DBNull.Value) Then
                                        StrRoute.Append("$" & Convert.ToDateTime(dr.Item(6)).TimeOfDay.ToString())
                                    End If

                                End If


                                'If (results.Length = 0) Then
                                '    StrRoute.Append("^N/A^N/A")
                                'End If

                                'If (results.Length = 0) Then
                                '    StrRoute.Append("^" & "")
                                'Else
                                '    StrRoute.Append("^" & results(0).Item("Customer_No"))
                                'End If

                                
                            Next
                        End If

                    End If



                    Dim CheckStr As String = StrRoute.ToString()



                    If (e.Day.Date.Day.ToString() = "1" And Cell1.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay1.Value = Str.ToString()
                            Session("HDay1") = Str.ToString()
                        Else
                            HDay1.Value = StrRoute.ToString()
                            Session("HDay1") = StrRoute.ToString()
                        End If
                    Else
                        Session("HDay1") = HDay1.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "2" And Cell2.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay2.Value = Str.ToString()
                            Session("HDay2") = Str.ToString()
                        Else
                            HDay2.Value = StrRoute.ToString()
                            Session("HDay2") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay2") = HDay2.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "3" And Cell3.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay3.Value = Str.ToString()
                            Session("HDay3") = Str.ToString()
                        Else
                            HDay3.Value = StrRoute.ToString()
                            Session("HDay3") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay3") = HDay3.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "4" And Cell4.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay4.Value = Str.ToString()
                            Session("HDay4") = Str.ToString()
                        Else
                            HDay4.Value = StrRoute.ToString()
                            Session("HDay4") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay4") = HDay4.Value
                    End If

                    If (e.Day.Date.Day.ToString() = "5" And Cell5.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay5.Value = Str.ToString()
                            Session("HDay5") = Str.ToString()
                        Else
                            HDay5.Value = StrRoute.ToString()
                            Session("HDay5") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay5") = HDay5.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "6" And Cell6.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay6.Value = Str.ToString()
                            Session("HDay6") = Str.ToString()
                        Else
                            HDay6.Value = StrRoute.ToString()
                            Session("HDay6") = StrRoute.ToString()
                        End If
                    Else
                        Session("HDay6") = HDay6.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "7" And Cell7.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay7.Value = Str.ToString()
                            Session("HDay7") = Str.ToString()
                        Else
                            HDay7.Value = StrRoute.ToString()
                            Session("HDay7") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay7") = HDay7.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "8" And Cell8.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay8.Value = Str.ToString()
                            Session("HDay8") = Str.ToString()
                        Else
                            HDay8.Value = StrRoute.ToString()
                            Session("HDay8") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay8") = HDay8.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "9" And Cell9.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay9.Value = Str.ToString()
                            Session("HDay9") = Str.ToString()
                        Else
                            HDay9.Value = StrRoute.ToString()
                            Session("HDay9") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay9") = HDay9.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "10" And Cell10.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay10.Value = Str.ToString()
                            Session("HDay10") = Str.ToString()
                        Else
                            HDay10.Value = StrRoute.ToString()
                            Session("HDay10") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay10") = HDay10.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "11" And Cell11.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay11.Value = Str.ToString()
                            Session("HDay11") = Str.ToString()
                        Else
                            HDay11.Value = StrRoute.ToString()
                            Session("HDay11") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay11") = HDay11.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "12" And Cell12.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay12.Value = Str.ToString()
                            Session("HDay12") = Str.ToString()
                        Else
                            HDay12.Value = StrRoute.ToString()
                            Session("HDay12") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay12") = HDay12.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "13" And Cell13.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay13.Value = Str.ToString()
                            Session("HDay13") = Str.ToString()
                        Else
                            HDay13.Value = StrRoute.ToString()
                            Session("HDay13") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay13") = HDay13.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "14" And Cell14.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay14.Value = Str.ToString()
                            Session("HDay14") = Str.ToString()
                        Else
                            HDay14.Value = StrRoute.ToString()
                            Session("HDay14") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay14") = HDay14.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "15" And Cell15.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay15.Value = Str.ToString()
                            Session("HDay15") = Str.ToString()
                        Else
                            HDay15.Value = StrRoute.ToString()
                            Session("HDay15") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay15") = HDay15.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "16" And Cell16.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay16.Value = Str.ToString()
                            Session("HDay16") = Str.ToString()
                        Else
                            HDay16.Value = StrRoute.ToString()
                            Session("HDay16") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay16") = HDay16.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "17" And Cell17.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay17.Value = Str.ToString()
                            Session("HDay17") = Str.ToString()
                        Else
                            HDay17.Value = StrRoute.ToString()
                            Session("HDay17") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay17") = HDay17.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "18" And Cell18.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay18.Value = Str.ToString()
                            Session("HDay18") = Str.ToString()
                        Else
                            HDay18.Value = StrRoute.ToString()
                            Session("HDay18") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay18") = HDay18.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "19" And Cell19.Value = "N") Then

                        If (StrRoute.ToString() = "") Then
                            HDay19.Value = Str.ToString()
                            Session("HDay19") = Str.ToString()
                        Else
                            HDay19.Value = StrRoute.ToString()
                            Session("HDay19") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay19") = HDay19.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "20" And Cell20.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay20.Value = Str.ToString()
                            Session("HDay20") = Str.ToString()
                        Else
                            HDay20.Value = StrRoute.ToString()
                            Session("HDay20") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay20") = HDay20.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "21" And Cell21.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay21.Value = Str.ToString()
                            Session("HDay21") = Str.ToString()
                        Else
                            HDay21.Value = StrRoute.ToString()
                            Session("HDay21") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay21") = HDay21.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "22" And Cell22.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay22.Value = Str.ToString()
                            Session("HDay22") = Str.ToString()
                        Else
                            HDay22.Value = StrRoute.ToString()
                            Session("HDay22") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay22") = HDay22.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "23" And Cell23.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay23.Value = Str.ToString()
                            Session("HDay23") = Str.ToString()
                        Else
                            HDay23.Value = StrRoute.ToString()
                            Session("HDay23") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay23") = HDay23.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "24" And Cell24.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay24.Value = Str.ToString()
                            Session("HDay24") = Str.ToString()
                        Else
                            HDay24.Value = StrRoute.ToString()
                            Session("HDay24") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay24") = HDay24.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "25" And Cell25.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay25.Value = Str.ToString()
                            Session("HDay25") = Str.ToString()
                        Else
                            HDay25.Value = StrRoute.ToString()
                            Session("HDay25") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay25") = HDay25.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "26" And Cell26.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay26.Value = Str.ToString()
                            Session("HDay26") = Str.ToString()
                        Else
                            HDay26.Value = StrRoute.ToString()
                            Session("HDay26") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay26") = HDay26.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "27" And Cell27.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay27.Value = Str.ToString()
                            Session("HDay27") = Str.ToString()
                        Else
                            HDay27.Value = StrRoute.ToString()
                            Session("HDay27") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay27") = HDay27.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "28" And Cell28.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay28.Value = Str.ToString()
                            Session("HDay28") = Str.ToString()
                        Else
                            HDay28.Value = StrRoute.ToString()
                            Session("HDay28") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay28") = HDay28.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "29" And Cell29.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay29.Value = Str.ToString()
                            Session("HDay29") = Str.ToString()
                        Else
                            HDay29.Value = StrRoute.ToString()
                            Session("HDay29") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay29") = HDay29.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "30" And Cell30.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay30.Value = Str.ToString()
                            Session("HDay30") = Str.ToString()
                        Else
                            HDay30.Value = StrRoute.ToString()
                            Session("HDay30") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay30") = HDay30.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "31" And Cell31.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay31.Value = Str.ToString()
                            Session("HDay31") = Str.ToString()
                        Else
                            HDay31.Value = StrRoute.ToString()
                            Session("HDay31") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay31") = HDay31.Value
                    End If
                    CalendarCell = e.Cell
                    e.Cell.Style.Add("cursor", "Pointer")
                    If e.Day.Date > Now.Date Then
                        e.Cell.Text = "<a href='javascript:void(0);'  style='color: #000000;' onClick=window.open('_POP_CustomerListingNew.aspx?Mode=ADD&IRS=N&IsRefresh=N&dayref=" & e.Day.Date.Day & "&DType=X','CustomerList','width=700,height=700,scrollbars=yes');>" & e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year & " </a >"
                        ' e.Cell.Text = "<a href='javascript:void(0);'  style='color: #000000;' onClick=window.open('Test.aspx?Mode=ADD&IRS=N&IsRefresh=N&dayref=" & e.Day.Date.Day & "&DType=X','CustomerList','width=700,height=700,scrollbars=yes');>" & e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year & " </a >"

                    End If
                    e.Cell.ToolTip = e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year
                    e.Cell.BackColor = System.Drawing.Color.FromName("#EEEEEE")
                    e.Cell.HorizontalAlign = Web.UI.WebControls.HorizontalAlign.Left


                    'If (e.Day.Date.DayOfWeek = DayOfWeek.Friday) Then
                    '    e.Cell.BackColor = System.Drawing.Color.FromName("#E1E1E1")
                    '    e.Cell.Text = "<a href='javascript:void(0);'  style='color: #000000;' onClick=window.open('_POP_CustomerListingNew.aspx?Mode=ADD&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=W','CustomerList','width=700,height=700,scrollbars=yes');>" & e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year & " </a >"
                    '    e.Cell.Style.Add("cursor", "Cursor")
                    'End If

                    Dim CellUrl As String
                    Dim CellBgColor As String = "#EEEEEE"
                    If (HDay1.Value <> "" And e.Day.Date.Day.ToString() = "1") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, Session("HDay1"), Str.ToString())
                        If (HDay1.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay1.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay1.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay1.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay1.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, Session("HDay1"), Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    ''CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If

                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If


                    If (HDay2.Value <> "" And e.Day.Date.Day.ToString() = "2") Then

                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay2.Value, Str.ToString())
                        If (HDay2.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay2.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay2.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay2.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay2.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay2.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    ''   CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    ' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay3.Value <> "" And e.Day.Date.Day.ToString() = "3") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay3.Value, Str.ToString())
                        If (HDay3.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay3.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay3.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay3.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay3.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay3.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    '' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    ' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay4.Value <> "" And e.Day.Date.Day.ToString() = "4") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay4.Value, Str.ToString())
                        If (HDay4.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay4.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay4.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay4.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay4.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay4.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    ''   CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay5.Value <> "" And e.Day.Date.Day.ToString() = "5") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay5.Value, Str.ToString())
                        If (HDay5.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay5.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay5.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay5.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay5.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay5.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    ''  CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay6.Value <> "" And e.Day.Date.Day.ToString() = "6") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay6.Value, Str.ToString())
                        If (HDay6.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay6.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay6.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay6.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay6.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay6.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    '' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    ' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay7.Value <> "" And e.Day.Date.Day.ToString() = "7") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay7.Value, Str.ToString())
                        If (HDay7.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay7.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay7.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay7.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay7.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay7.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    ''  CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay8.Value <> "" And e.Day.Date.Day.ToString() = "8") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay8.Value, Str.ToString())
                        If (HDay8.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay8.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay8.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay8.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay8.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay8.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    '' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    ' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay9.Value <> "" And e.Day.Date.Day.ToString() = "9") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay9.Value, Str.ToString())
                        If (HDay9.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay9.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay9.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay9.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay9.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay9.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    ''  CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If
                    If (HDay10.Value <> "" And e.Day.Date.Day.ToString() = "10") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay10.Value, Str.ToString())
                        If (HDay10.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay10.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay10.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay10.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay10.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay10.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    '' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay11.Value <> "" And e.Day.Date.Day.ToString() = "11") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay11.Value, Str.ToString())
                        If (HDay11.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay11.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay11.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay11.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay11.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay11.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    '' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    ' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay12.Value <> "" And e.Day.Date.Day.ToString() = "12") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay12.Value, Str.ToString())
                        If (HDay12.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay12.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay12.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay12.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay12.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay12.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    ''  CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    ' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay13.Value <> "" And e.Day.Date.Day.ToString() = "13") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay13.Value, Str.ToString())
                        If (HDay13.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay13.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay13.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay13.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay13.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay13.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    '' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    ' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay14.Value <> "" And e.Day.Date.Day.ToString() = "14") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay14.Value, Str.ToString())
                        If (HDay14.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay14.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay14.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay14.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay14.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay14.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    ''  CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    ' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay15.Value <> "" And e.Day.Date.Day.ToString() = "15") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay15.Value, Str.ToString())
                        If (HDay15.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay15.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay15.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay15.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay15.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay15.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    '' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay16.Value <> "" And e.Day.Date.Day.ToString() = "16") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay16.Value, Str.ToString())
                        If (HDay16.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay16.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay16.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay16.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay16.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay16.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    ''  CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay17.Value <> "" And e.Day.Date.Day.ToString() = "17") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay17.Value, Str.ToString())
                        If (HDay17.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay17.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay17.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay17.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay17.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay17.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    ''  CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay18.Value <> "" And e.Day.Date.Day.ToString() = "18") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay18.Value, Str.ToString())
                        If (HDay18.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay18.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay18.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay18.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay18.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay18.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    ''  CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay19.Value <> "" And e.Day.Date.Day.ToString() = "19") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay19.Value, Str.ToString())
                        If (HDay19.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay19.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay19.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay19.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay19.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay19.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    ''  CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    ' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay20.Value <> "" And e.Day.Date.Day.ToString() = "20") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay20.Value, Str.ToString())
                        If (HDay20.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay20.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay20.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay20.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay20.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay20.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    ''  CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    ' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay21.Value <> "" And e.Day.Date.Day.ToString() = "21") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay21.Value, Str.ToString())
                        If (HDay21.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay21.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay21.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay21.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay21.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay21.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    '' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay22.Value <> "" And e.Day.Date.Day.ToString() = "22") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay22.Value, Str.ToString())
                        If (HDay22.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay22.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay22.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay22.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay22.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay22.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    ''  CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    ' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay23.Value <> "" And e.Day.Date.Day.ToString() = "23") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay23.Value, Str.ToString())
                        If (HDay23.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay23.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay23.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay23.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay23.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay23.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    '' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    ' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay24.Value <> "" And e.Day.Date.Day.ToString() = "24") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay24.Value, Str.ToString())
                        If (HDay24.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay24.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay24.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay24.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay24.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay24.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    '' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay25.Value <> "" And e.Day.Date.Day.ToString() = "25") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay25.Value, Str.ToString())
                        If (HDay25.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay25.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay25.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay25.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay25.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay25.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    ''  CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay26.Value <> "" And e.Day.Date.Day.ToString() = "26") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay26.Value, Str.ToString())
                        If (HDay26.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay26.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay26.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay26.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay26.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay26.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    ''  CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    ' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay27.Value <> "" And e.Day.Date.Day.ToString() = "27") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay27.Value, Str.ToString())
                        If (HDay27.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay27.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay27.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay27.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay27.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay27.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    '' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay28.Value <> "" And e.Day.Date.Day.ToString() = "28") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay28.Value, Str.ToString())
                        If (HDay28.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay28.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay28.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay28.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay28.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay28.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    '' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    ' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay29.Value <> "" And e.Day.Date.Day.ToString() = "29") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay29.Value, Str.ToString())
                        If (HDay29.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay29.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay29.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay29.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay29.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay29.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    '' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    ' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay30.Value <> "" And e.Day.Date.Day.ToString() = "30") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay30.Value, Str.ToString())
                        If (HDay30.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay30.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay30.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay30.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay30.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay30.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    ''  CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    'CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay31.Value <> "" And e.Day.Date.Day.ToString() = "31") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay31.Value, Str.ToString())
                        If (HDay31.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay31.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                            CellUrl = "V"
                        ElseIf (HDay31.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay31.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay31.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay31.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") Then
                            If (IsApproved.Value = "Y" And CDate(e.Day.Date.Month & "-" & e.Day.Date.Day & "-" & e.Day.Date.Year) <= Today()) Then
                                CalendarCell.Attributes.Add("OnClick", "javascript:alert('This plan is already approved and cannot be updated')")
                            Else
                                If e.Day.Date > Now.Date Then
                                    '' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=1024,height=600,scrollbars=yes')")
                                    CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('_POP_CustomerListingNew.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")
                                    ' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenWindow('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=MODIFY&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "'," & IsHoliday(e.Day.Date.Day) & ")")

                                    e.Cell.Style.Add("cursor", "Pointer")
                                Else
                                    e.Cell.Style.Add("cursor", "Cursor")
                                End If
                            End If
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If
                    If e.Day.Date = Now.Date Then
                        e.Cell.BackColor = System.Drawing.Color.Honeydew
                        sp.Visible = True
                    End If

                Else
                    e.Cell.Visible = False
                    If ((e.Day.Date < StartDate And (DateDiff(DateInterval.WeekOfYear, e.Day.Date, StartDate) = 0)) Or (e.Day.Date > EndDate And (DateDiff(DateInterval.WeekOfYear, e.Day.Date, EndDate)) = 0)) Then
                        e.Cell.Visible = True
                    End If
                    e.Cell.Text = ""
                    e.Cell.ToolTip = ""
                    e.Cell.ToolTip.Remove((0), e.Cell.ToolTip.Length())

                End If



            End If
            If (Action_Mode.Value = "APPROVE") Or (Action_Mode.Value = "REVIEW") Then
                ' Try
                Dim SDate As String = Start_Date.Text.Trim()
                Dim EDate As String = End_Date.Text.Trim()
                Dim CellText As String


                StartDate = Convert.ToDateTime(SDate.Substring(3, 3) & SDate.Substring(0, 3) & SDate.Substring(6, 4))
                EndDate = Convert.ToDateTime(EDate.Substring(3, 3) & EDate.Substring(0, 3) & EDate.Substring(6, 4))
                Dim CalendarCell As TableCell

                If (EndDate.Day >= e.Day.Date.Day And e.Day.Date.Day >= StartDate.Day And e.Day.Date.Month = StartDate.Month And e.Day.Date.Month = EndDate.Month) Then

                    Dim DefaultPlanDetailID As Integer
                    Dim DefDayType As Char
                    Dim CanVisit As Char

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
                        Str.Append("@" & Convert.ToDateTime(foundRows(0).Item(3)).TimeOfDay.ToString())
                    End If
                    If Not (foundRows(0).Item(4) Is DBNull.Value) Then
                        Str.Append("$" & Convert.ToDateTime(foundRows(0).Item(4)).TimeOfDay.ToString())
                    End If

                    Dim foundRouteRows() As DataRow = FSRDataSet.Tables("FSRPlan").Select("Day=" & e.Day.Date.Day & "")

                    Dim StrRoute As New StringBuilder
                    StrRoute.Append("")
                    If (foundRouteRows.Length > 0) Then

                        StrRoute.Append(foundRouteRows(0).Item(7))
                        If (foundRouteRows.Length = 1) Then
                            If Not (foundRouteRows(0).Item(8) Is DBNull.Value) Then
                                StrRoute.Append("|" & foundRouteRows(0).Item(8))
                            Else
                                If (foundRouteRows(0).Item(7) = "V") Then
                                    Dim i As Integer = 0
                                    StrRoute.Append("|")
                                    For Each dr As DataRow In foundRouteRows
                                       

                                        objRoutePlan = New RoutePlan
                                        'objRoutePlan.SalesRepIDprop = Session("USER_ACCESS").SalesRepID()
                                        objRoutePlan.SalesRepID = Session("SalesRep_ID")
                                        Cache.Remove("CustomerList")
                                        If Session("CustomerList") Is Nothing Then
                                            Session("CustomerList") = objRoutePlan.GetCustomerList(Err_No, Err_Desc)
                                        End If

                                        Dim results() As DataRow = CType(Session("CustomerList"), DataTable).Select("Customer_ID=" & dr.Item(3) & " and Site_Use_ID=" & dr.Item(4) & "")

                                        If (results.Length > 0) Then
                                            If Not (dr.Item(3) Is DBNull.Value) Then
                                                StrRoute.Append("|" & dr.Item(3))
                                            End If

                                            StrRoute.Append("^" & results(0).Item(2))
                                            If Not (dr.Item(4) Is DBNull.Value) Then
                                                StrRoute.Append("^" & dr.Item(4))
                                            Else
                                                StrRoute.Append("^" & "")
                                            End If

                                            If Not (dr.Item("Visit_Sequence") Is DBNull.Value) Then
                                                StrRoute.Append("^" & dr.Item("Visit_Sequence"))
                                            Else
                                                StrRoute.Append("^" & "")
                                            End If


                                            If Not (dr.Item("Allow_Optimization") Is DBNull.Value) Then
                                                StrRoute.Append("^" & dr.Item("Allow_Optimization"))
                                            Else
                                                StrRoute.Append("^" & dr.Item("Allow_Optimization"))
                                            End If
                                            StrRoute.Append("^" & results(0).Item("Customer_No"))
                                            If Not (dr.Item(5) Is DBNull.Value) Then
                                                StrRoute.Append("@" & Convert.ToDateTime(dr.Item(5)).TimeOfDay.ToString())
                                            End If
                                            If Not (dr.Item(6) Is DBNull.Value) Then
                                                StrRoute.Append("$" & Convert.ToDateTime(dr.Item(6)).TimeOfDay.ToString())
                                            End If
                                        End If

                                        'If (results.Length = 0) Then
                                        '    StrRoute.Append("^N/A^N/A")
                                        'End If

                                        'If (results.Length = 0) Then
                                        '    StrRoute.Append("^" & "")
                                        'Else
                                        '    StrRoute.Append("^" & results(0).Item("Customer_No"))
                                        'End If

                                       
                                    Next
                                Else
                                    StrRoute.Append("|*")
                                End If
                            End If
                        Else
                            Dim i As Integer = 0
                            StrRoute.Append("|")
                            For Each dr As DataRow In foundRouteRows
                              

                                objRoutePlan = New RoutePlan
                                objRoutePlan.SalesRepID = Session("SalesRep_ID")
                                Cache.Remove("CustomerList")
                                If Session("CustomerList") Is Nothing Then
                                    Session("CustomerList") = objRoutePlan.GetCustomerList(Err_No, Err_Desc)
                                End If

                                Dim results() As DataRow = CType(Session("CustomerList"), DataTable).Select("Customer_ID=" & dr.Item(3) & " and Site_Use_ID=" & dr.Item(4) & "")

                                If (results.Length > 0) Then
                                    If Not (dr.Item(3) Is DBNull.Value) Then
                                        StrRoute.Append("|" & dr.Item(3))
                                    End If
                                    StrRoute.Append("^" & results(0).Item(2))

                                    If Not (dr.Item(4) Is DBNull.Value) Then
                                        StrRoute.Append("^" & dr.Item(4))
                                    End If

                                    If Not (dr.Item("Visit_Sequence") Is DBNull.Value) Then
                                        StrRoute.Append("^" & dr.Item("Visit_Sequence"))
                                    Else
                                        StrRoute.Append("^" & "")
                                    End If


                                    If Not (dr.Item("Allow_Optimization") Is DBNull.Value) Then
                                        StrRoute.Append("^" & dr.Item("Allow_Optimization"))
                                    Else
                                        StrRoute.Append("^" & dr.Item("Allow_Optimization"))
                                    End If

                                    StrRoute.Append("^" & results(0).Item("Customer_No"))
                                    If Not (dr.Item(5) Is DBNull.Value) Then
                                        StrRoute.Append("@" & Convert.ToDateTime(dr.Item(5)).TimeOfDay.ToString())
                                    End If
                                    If Not (dr.Item(6) Is DBNull.Value) Then
                                        StrRoute.Append("$" & Convert.ToDateTime(dr.Item(6)).TimeOfDay.ToString())
                                    End If
                                End If

                                'If (results.Length = 0) Then
                                '    StrRoute.Append("^N/A-N/A")
                                'End If

                                'If (results.Length = 0) Then
                                '    StrRoute.Append("^" & "")
                                'Else
                                '    StrRoute.Append("^" & results(0).Item("Customer_No"))
                                'End If

                               
                            Next
                        End If

                    End If

                    Dim CheckStr As String = StrRoute.ToString()

                    If (e.Day.Date.Day.ToString() = "1" And Cell1.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay1.Value = Str.ToString()
                            Session("HDay1") = Str.ToString()
                        Else
                            HDay1.Value = StrRoute.ToString()
                            Session("HDay1") = StrRoute.ToString()
                        End If
                    Else
                        Session("HDay1") = HDay1.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "2" And Cell2.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay2.Value = Str.ToString()
                            Session("HDay2") = Str.ToString()
                        Else
                            HDay2.Value = StrRoute.ToString()
                            Session("HDay2") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay2") = HDay2.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "3" And Cell3.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay3.Value = Str.ToString()
                            Session("HDay3") = Str.ToString()
                        Else
                            HDay3.Value = StrRoute.ToString()
                            Session("HDay3") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay3") = HDay3.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "4" And Cell4.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay4.Value = Str.ToString()
                            Session("HDay4") = Str.ToString()
                        Else
                            HDay4.Value = StrRoute.ToString()
                            Session("HDay4") = StrRoute.ToString()
                        End If
                    Else
                        Session("HDay4") = HDay4.Value
                    End If

                    If (e.Day.Date.Day.ToString() = "5" And Cell5.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay5.Value = Str.ToString()
                            Session("HDay5") = Str.ToString()
                        Else
                            HDay5.Value = StrRoute.ToString()
                            Session("HDay5") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay5") = HDay5.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "6" And Cell6.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay6.Value = Str.ToString()
                            Session("HDay6") = Str.ToString()
                        Else
                            HDay6.Value = StrRoute.ToString()
                            Session("HDay6") = StrRoute.ToString()
                        End If
                    Else
                        Session("HDay6") = HDay6.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "7" And Cell7.Value = "N") Then

                        If (StrRoute.ToString() = "") Then
                            HDay7.Value = Str.ToString()
                            Session("HDay7") = Str.ToString()
                        Else
                            HDay7.Value = StrRoute.ToString()
                            Session("HDay7") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay7") = HDay7.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "8" And Cell8.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay8.Value = Str.ToString()
                            Session("HDay8") = Str.ToString()
                        Else
                            HDay8.Value = StrRoute.ToString()
                            Session("HDay8") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay8") = HDay8.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "9" And Cell9.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay9.Value = Str.ToString()
                            Session("HDay9") = Str.ToString()
                        Else
                            HDay9.Value = StrRoute.ToString()
                            Session("HDay9") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay9") = HDay9.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "10" And Cell10.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay10.Value = Str.ToString()
                            Session("HDay10") = Str.ToString()
                        Else
                            HDay10.Value = StrRoute.ToString()
                            Session("HDay10") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay10") = HDay10.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "11" And Cell11.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay11.Value = Str.ToString()
                            Session("HDay11") = Str.ToString()
                        Else
                            HDay11.Value = StrRoute.ToString()
                            Session("HDay11") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay11") = HDay11.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "12" And Cell12.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay12.Value = Str.ToString()
                            Session("HDay12") = Str.ToString()
                        Else
                            HDay12.Value = StrRoute.ToString()
                            Session("HDay12") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay12") = HDay12.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "13" And Cell13.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay13.Value = Str.ToString()
                            Session("HDay13") = Str.ToString()
                        Else
                            HDay13.Value = StrRoute.ToString()
                            Session("HDay13") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay13") = HDay13.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "14" And Cell14.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay14.Value = Str.ToString()
                            Session("HDay14") = Str.ToString()
                        Else
                            HDay14.Value = StrRoute.ToString()
                            Session("HDay14") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay14") = HDay14.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "15" And Cell15.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay15.Value = Str.ToString()
                            Session("HDay15") = Str.ToString()
                        Else
                            HDay15.Value = StrRoute.ToString()
                            Session("HDay15") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay15") = HDay15.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "16" And Cell16.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay16.Value = Str.ToString()
                            Session("HDay16") = Str.ToString()
                        Else
                            HDay16.Value = StrRoute.ToString()
                            Session("HDay16") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay16") = HDay16.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "17" And Cell17.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay17.Value = Str.ToString()
                            Session("HDay17") = Str.ToString()
                        Else
                            HDay17.Value = StrRoute.ToString()
                            Session("HDay17") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay17") = HDay17.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "18" And Cell18.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay18.Value = Str.ToString()
                            Session("HDay18") = Str.ToString()
                        Else
                            HDay18.Value = StrRoute.ToString()
                            Session("HDay18") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay18") = HDay18.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "19" And Cell19.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay19.Value = Str.ToString()
                            Session("HDay19") = Str.ToString()
                        Else
                            HDay19.Value = StrRoute.ToString()
                            Session("HDay19") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay19") = HDay19.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "20" And Cell20.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay20.Value = Str.ToString()
                            Session("HDay20") = Str.ToString()
                        Else
                            HDay20.Value = StrRoute.ToString()
                            Session("HDay20") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay20") = HDay20.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "21" And Cell21.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay21.Value = Str.ToString()
                            Session("HDay21") = Str.ToString()
                        Else
                            HDay21.Value = StrRoute.ToString()
                            Session("HDay21") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay21") = HDay21.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "22" And Cell22.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay22.Value = Str.ToString()
                            Session("HDay22") = Str.ToString()
                        Else
                            HDay22.Value = StrRoute.ToString()
                            Session("HDay22") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay22") = HDay22.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "23" And Cell23.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay23.Value = Str.ToString()
                            Session("HDay23") = Str.ToString()
                        Else
                            HDay23.Value = StrRoute.ToString()
                            Session("HDay23") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay23") = HDay23.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "24" And Cell24.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay24.Value = Str.ToString()
                            Session("HDay24") = Str.ToString()
                        Else
                            HDay24.Value = StrRoute.ToString()
                            Session("HDay24") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay24") = HDay24.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "25" And Cell25.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay25.Value = Str.ToString()
                            Session("HDay25") = Str.ToString()
                        Else
                            HDay25.Value = StrRoute.ToString()
                            Session("HDay25") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay25") = HDay25.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "26" And Cell26.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay26.Value = Str.ToString()
                            Session("HDay26") = Str.ToString()
                        Else
                            HDay26.Value = StrRoute.ToString()
                            Session("HDay26") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay26") = HDay26.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "27" And Cell27.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay27.Value = Str.ToString()
                            Session("HDay27") = Str.ToString()
                        Else
                            HDay27.Value = StrRoute.ToString()
                            Session("HDay27") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay27") = HDay27.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "28" And Cell28.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay28.Value = Str.ToString()
                            Session("HDay28") = Str.ToString()
                        Else
                            HDay28.Value = StrRoute.ToString()
                            Session("HDay28") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay28") = HDay28.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "29" And Cell29.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay29.Value = Str.ToString()
                            Session("HDay29") = Str.ToString()
                        Else
                            HDay29.Value = StrRoute.ToString()
                            Session("HDay29") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay29") = HDay29.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "30" And Cell30.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay30.Value = Str.ToString()
                            Session("HDay30") = Str.ToString()
                        Else
                            HDay30.Value = StrRoute.ToString()
                            Session("HDay30") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay30") = HDay30.Value
                    End If
                    If (e.Day.Date.Day.ToString() = "31" And Cell31.Value = "N") Then
                        If (StrRoute.ToString() = "") Then
                            HDay31.Value = Str.ToString()
                            Session("HDay31") = Str.ToString()
                        Else
                            HDay31.Value = StrRoute.ToString()
                            Session("HDay31") = StrRoute.ToString()
                        End If

                    Else
                        Session("HDay31") = HDay31.Value
                    End If
                    CalendarCell = e.Cell
                    e.Cell.Style.Add("cursor", "Cursor")

                    e.Cell.Text = e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year

                    e.Cell.ToolTip = e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year
                    e.Cell.BackColor = System.Drawing.Color.FromName("#EEEEEE")
                    e.Cell.HorizontalAlign = Web.UI.WebControls.HorizontalAlign.Left



                    Dim CellUrl As String
                    Dim CellBgColor As String
                    If (HDay1.Value <> "" And e.Day.Date.Day.ToString() = "1") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, Session("HDay1"), Str.ToString())
                        If (HDay1.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay1.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay1.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay1.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay1.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, Session("HDay1"), Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay1.Value <> "" And HDay1.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=750,height=600,scrollbars=yes')")
                            ' CalendarCell.Attributes.Add("OnClick", "javascript:onClick=window.open('Test.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "','CustomerList','width=750,height=600,scrollbars=yes')")

                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If


                    If (HDay2.Value <> "" And e.Day.Date.Day.ToString() = "2") Then

                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay2.Value, Str.ToString())
                        If (HDay2.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay2.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay2.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay2.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay2.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay2.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay2.Value <> "" And HDay2.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay3.Value <> "" And e.Day.Date.Day.ToString() = "3") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay3.Value, Str.ToString())
                        If (HDay3.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"

                        ElseIf (HDay3.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay3.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay3.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay3.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay3.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay3.Value <> "" And HDay3.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay4.Value <> "" And e.Day.Date.Day.ToString() = "4") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay4.Value, Str.ToString())
                        If (HDay4.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay4.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay4.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay4.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay4.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay4.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay4.Value <> "" And HDay4.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay5.Value <> "" And e.Day.Date.Day.ToString() = "5") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay5.Value, Str.ToString())
                        If (HDay5.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay5.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay5.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay5.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay5.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay5.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay5.Value <> "" And HDay5.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay6.Value <> "" And e.Day.Date.Day.ToString() = "6") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay6.Value, Str.ToString())
                        If (HDay6.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay6.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"

                        ElseIf (HDay6.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay6.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay6.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay6.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay6.Value <> "" And HDay6.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay7.Value <> "" And e.Day.Date.Day.ToString() = "7") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay7.Value, Str.ToString())
                        If (HDay7.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay7.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay7.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay7.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay7.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay7.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay7.Value <> "" And HDay7.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay8.Value <> "" And e.Day.Date.Day.ToString() = "8") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay8.Value, Str.ToString())
                        If (HDay8.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay8.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay8.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay8.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay8.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay8.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay8.Value <> "" And HDay8.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay9.Value <> "" And e.Day.Date.Day.ToString() = "9") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay9.Value, Str.ToString())
                        If (HDay9.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay9.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay9.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay9.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay9.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay9.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay9.Value <> "" And HDay9.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If
                    If (HDay10.Value <> "" And e.Day.Date.Day.ToString() = "10") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay10.Value, Str.ToString())
                        If (HDay10.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay10.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay10.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay10.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay10.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay10.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay10.Value <> "" And HDay10.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay11.Value <> "" And e.Day.Date.Day.ToString() = "11") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay11.Value, Str.ToString())
                        If (HDay11.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay11.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay11.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay11.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay11.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay11.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay11.Value <> "" And HDay11.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay12.Value <> "" And e.Day.Date.Day.ToString() = "12") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay12.Value, Str.ToString())
                        If (HDay12.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay12.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay12.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay12.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay12.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay12.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay12.Value <> "" And HDay12.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay13.Value <> "" And e.Day.Date.Day.ToString() = "13") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay13.Value, Str.ToString())
                        If (HDay13.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay13.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay13.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay13.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay13.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay13.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay13.Value <> "" And HDay13.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay14.Value <> "" And e.Day.Date.Day.ToString() = "14") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay14.Value, Str.ToString())
                        If (HDay14.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay14.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay14.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay14.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay14.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay14.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay14.Value <> "" And HDay14.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay15.Value <> "" And e.Day.Date.Day.ToString() = "15") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay15.Value, Str.ToString())
                        If (HDay15.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay15.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay15.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay15.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay15.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay15.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay15.Value <> "" And HDay15.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay16.Value <> "" And e.Day.Date.Day.ToString() = "16") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay16.Value, Str.ToString())
                        If (HDay16.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay16.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay16.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay16.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay16.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay16.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay16.Value <> "" And HDay16.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay17.Value <> "" And e.Day.Date.Day.ToString() = "17") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay17.Value, Str.ToString())
                        If (HDay17.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay17.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay17.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay17.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay17.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay17.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay17.Value <> "" And HDay17.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay18.Value <> "" And e.Day.Date.Day.ToString() = "18") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay18.Value, Str.ToString())
                        If (HDay18.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay18.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay18.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay18.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay18.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay18.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay18.Value <> "" And HDay18.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay19.Value <> "" And e.Day.Date.Day.ToString() = "19") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay19.Value, Str.ToString())
                        If (HDay19.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay19.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay19.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay19.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay19.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay19.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay19.Value <> "" And HDay19.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay20.Value <> "" And e.Day.Date.Day.ToString() = "20") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay20.Value, Str.ToString())
                        If (HDay20.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay20.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay20.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay20.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay20.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay20.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay20.Value <> "" And HDay20.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay21.Value <> "" And e.Day.Date.Day.ToString() = "21") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay21.Value, Str.ToString())
                        If (HDay21.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay21.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay21.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay21.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay21.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay21.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay21.Value <> "" And HDay21.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay22.Value <> "" And e.Day.Date.Day.ToString() = "22") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay22.Value, Str.ToString())
                        If (HDay22.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay22.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay22.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay22.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay22.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay22.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay22.Value <> "" And HDay22.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay23.Value <> "" And e.Day.Date.Day.ToString() = "23") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay23.Value, Str.ToString())
                        If (HDay23.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay23.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay23.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay23.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay23.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay23.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay23.Value <> "" And HDay23.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay24.Value <> "" And e.Day.Date.Day.ToString() = "24") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay24.Value, Str.ToString())
                        If (HDay24.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay24.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay24.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay24.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay24.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay24.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay24.Value <> "" And HDay24.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay25.Value <> "" And e.Day.Date.Day.ToString() = "25") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay25.Value, Str.ToString())
                        If (HDay25.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay25.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay25.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay25.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay25.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay25.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay25.Value <> "" And HDay25.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay26.Value <> "" And e.Day.Date.Day.ToString() = "26") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay26.Value, Str.ToString())
                        If (HDay26.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay26.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay26.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay26.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay26.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay26.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay26.Value <> "" And HDay26.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay27.Value <> "" And e.Day.Date.Day.ToString() = "27") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay27.Value, Str.ToString())
                        If (HDay27.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay27.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay27.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay27.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay27.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay27.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay27.Value <> "" And HDay27.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay28.Value <> "" And e.Day.Date.Day.ToString() = "28") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay28.Value, Str.ToString())
                        If (HDay28.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay28.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay28.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay28.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay28.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay28.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay28.Value <> "" And HDay28.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay29.Value <> "" And e.Day.Date.Day.ToString() = "29") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay29.Value, Str.ToString())
                        If (HDay29.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay29.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay29.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay29.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay29.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay29.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay29.Value <> "" And HDay29.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay30.Value <> "" And e.Day.Date.Day.ToString() = "30") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay30.Value, Str.ToString())
                        If (HDay30.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay30.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay30.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay30.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay30.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay30.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay30.Value <> "" And HDay30.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If

                    If (HDay31.Value <> "" And e.Day.Date.Day.ToString() = "31") Then
                        CellText = SetText(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay31.Value, Str.ToString())
                        If (HDay31.Value.Substring(0, 1) = "O") Then
                            CellBgColor = "#ffffdd"
                        ElseIf (HDay31.Value.Substring(0, 1) = "V") Then
                            CellBgColor = "#ddedff"
                        ElseIf (HDay31.Value.Substring(0, 1) = "W") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#ffffdd"
                            CellUrl = "O"
                        ElseIf (HDay31.Value.Substring(0, 1) = "X") And (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "M"
                        ElseIf (HDay31.Value.Substring(0, 1) = "X") And Not (IsOffDay(e.Day.Date)) Then
                            CellBgColor = "#EEEEEE"
                            CellUrl = "X"
                        Else
                            CellBgColor = "#EEEEEE"
                        End If
                        e.Cell.ToolTip = SetToolTip(e.Day.Date.Day & "-" & e.Day.Date.Month & "-" & e.Day.Date.Year, HDay31.Value, Str.ToString())
                        e.Cell.BackColor = System.Drawing.Color.FromName(CellBgColor)
                        If (CStr(foundRows(0).Item(7)) = "Y") And HDay31.Value <> "" And HDay31.Value <> "X" Then
                            CalendarCell.Attributes.Add("OnClick", "javascript:onClick=OpenViewWindow('_POP_CustomerListing.aspx?button=" & UpdateCal.FindControl("Button1").ClientID & "&Mode=REVIEW&IRS=N&IsRefresh=" & CheckRefresh.Value & "&dayref=" & e.Day.Date.Day & "&DType=" & CellUrl & "')")
                            e.Cell.Style.Add("cursor", "Pointer")
                        Else
                            e.Cell.Style.Add("cursor", "Cursor")
                        End If
                        e.Cell.Text = CellText
                    End If


                Else
                    e.Cell.Visible = False
                    If ((e.Day.Date < StartDate And (DateDiff(DateInterval.WeekOfYear, e.Day.Date, StartDate) = 0)) Or (e.Day.Date > EndDate And (DateDiff(DateInterval.WeekOfYear, e.Day.Date, EndDate)) = 0)) Then
                        e.Cell.Visible = True
                    End If
                    e.Cell.Text = ""
                    e.Cell.ToolTip = ""
                    e.Cell.ToolTip.Remove((0), e.Cell.ToolTip.Length())

                End If
            End If

        Catch ex As Exception
            log.Error(ex.ToString)
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("Information.aspx?mode=1&errno=" & "74021" & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=CreateRoutePlan.aspx?Title=Route+Planner")
        End Try
    End Sub
    Private Function IsHoliday(day As Integer) As Integer
        Dim Retval As Integer = 0
        Try
            Dim selectedRows As IEnumerable(Of DataRow) = dtHoliday.AsEnumerable().Where(Function(row) (row.Field(Of DateTime)("Holiday_Date").Day = day))
            ''   Dim SelRow() As DataRow = dtHoliday.Select("Day(Holiday_Date)=" & day & "")
            If selectedRows.Count > 0 Then
                Retval = 1
            End If
        Catch ex As Exception
            log.Error(ex.ToString)
        End Try
        Return Retval
    End Function
    Private Sub SaveBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveBtn.Click
        System.Threading.Thread.Sleep(1000)
        Action_Mode.Value = "Save"
        Try

            objRoutePlan = New RoutePlan
            Dim SDate As String = Start_Date.Text.Trim()
            Dim EDate As String = End_Date.Text.Trim()

            StartDate = Convert.ToDateTime(SDate.Substring(3, 3) & SDate.Substring(0, 3) & SDate.Substring(6, 4))
            EndDate = Convert.ToDateTime(EDate.Substring(3, 3) & EDate.Substring(0, 3) & EDate.Substring(6, 4))

            'objRoutePlan.SalesRepIDprop = Session("USER_ACCESS").SalesRepID()
            objRoutePlan.SalesRepID = Session("SalesRep_ID")
            objRoutePlan.DefPlanId = Session("Default_Plan_ID")

            If objRoutePlan.CheckFSRPlanID(Session("Default_Plan_ID"), Session("SalesRep_ID"), Err_No, Err_Desc) = True Then
                MessageBoxValidation("Route plan already created" & Err_Desc & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_003"), "Information") '   Response.Redirect("information.aspx?mode=0&msg=Route+plan+already+created&next=CreateRoutePlan.aspx&Title=Route+Planner", False)
                Exit Sub
            End If


            Dim transaction As SqlTransaction

            Dim SqlConn As SqlConnection
            Dim SqlCmd As SqlCommand
            SqlConn = objRoutePlan.GetConnection()

            transaction = SqlConn.BeginTransaction("SampleTransaction")

            Dim DefaultID As Integer = objRoutePlan.InsertFSRPlan(Err_No, Err_Desc)

            If (DefaultID > 0) Then
                For i As Integer = Convert.ToInt16(StartDate.Day) To Convert.ToInt16(EndDate.Day)

                    Dim HiddenCon As System.Web.UI.HtmlControls.HtmlInputHidden = TryCast(Page.FindControl("ctl00$MainContent$HDay" & i.ToString()), System.Web.UI.HtmlControls.HtmlInputHidden)
                    If HiddenCon IsNot Nothing And Err_Desc Is Nothing Then
                        InsertFSRDetails(HiddenCon.Value, DefaultID, i, SqlConn, SqlCmd, transaction, Err_No, Err_Desc)
                        If Err_Desc IsNot Nothing Then
                            log.Error(Err_Desc)
                            MessageBoxValidation("Error in Route plan saving:" & Err_Desc & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_003"), "Information") ' Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_003") & "&next=CreateRoutePlan.aspx&Title=Route+Planner", False)
                            Exit Sub
                        End If
                    Else
                        log.Error("Hidden Control is not accessable")
                        Exit For
                    End If
                Next
            End If
            transaction.Commit()
            objRoutePlan.CloseConnection(SqlConn)

            objLogin.SaveUserLog(Err_No, Err_Desc, "I", "CALL PLANNING", "CREATE ROUTE PLAN", SalesRep_Name.Text, "Desc: " & Description.Text & "/ Start Date :  " & StartDate & "/ End Date :  " & EndDate & "/ Assigned Visits : " & VCount, CType(Session("User_Access"), UserAccess).UserID.ToString(), SalesRep_Name.Text, "0")
            VCount = 0
            MessageBoxValidation("Route plan saved successfully.", "Information") ' Response.Redirect("information.aspx?mode=0&msg=Route+plan+saved+successfully&next=CreateRoutePlan.aspx&Title=Route+Planner", False)

        Catch ex As Exception
            Err_No = "74042"
            Err_Desc = ex.Message
            If (Err_Desc Is Nothing) Then
                log.Error(GetExceptionInfo(ex))
                'Response.Redirect("information.aspx?mode=0&msg=Route+plan+saved+successfully&next=CreateRoutePlan.aspx?ret=1&Title=Route+Planner", False)
            Else
                log.Error(Err_Desc)
            End If
            MessageBoxValidation("Error in Route plan saving:" & Err_Desc & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_003"), "Information") '  Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_003") & "&next=CreateRoutePlan.aspx&Title=Route+Planner", False)
        Finally
            objRoutePlan = Nothing

        End Try
    End Sub
    Private Sub InsertFSRDetails(ByVal DayValue As String, ByVal Id As Integer, ByVal Day As Integer, ByRef SqlConn As SqlConnection, ByRef sqlcomm As SqlCommand, ByRef transaction As SqlTransaction, ByRef Err_No As Long, ByRef Err_Desc As String)

        Dim RoutePlan As New RoutePlan
        Dim Flak As Boolean = False
        Dim setProp As Boolean = False
        Dim cust_i As Integer = 0
        Dim CusArr As Array = DayValue.Split("|")

        RoutePlan.DayType = CusArr(0)

        For i As Integer = 1 To CusArr.Length - 1
            Try

                Dim SubStrArr As Array = CusArr(i).Split("@")
                Dim IDArr As Array = SubStrArr(0).ToString.Split("^")
                If (CusArr.Length >= 2 Or (CusArr(0) = "O" And SubStrArr.Length = 1)) Then
                    If (CusArr.Length = 1) Then
                        Flak = True
                    ElseIf (SubStrArr(0).ToString() = "") Then
                        Flak = False
                    Else
                        If Not (SubStrArr(0).ToString() = "") Then
                            If (IDArr.Length > 2) Then
                                Flak = True
                                cust_i = cust_i + 1
                                Dim Str As String = Convert.ToString(CusArr(i))
                                RoutePlan.CustomerID = IDArr(0)
                                'RoutePlan.UserSiteID = IDArr(IDArr.Length - 1)
                                RoutePlan.UserSiteID = IDArr(2)
                                'RoutePlan.Sequence = IDArr(3) ''Wrong sequence number when a customer record deleted from the table on 27/08/2018 (Bug:7612)
                                RoutePlan.Sequence = cust_i
                                RoutePlan.AllowOptimization = IDArr(4)

                                If (SubStrArr.Length > 1) Then
                                    Dim TimeArr As Array = SubStrArr(1).ToString.Split("$")
                                    If (TimeArr.Length > 0) Then
                                        RoutePlan.StartTime = Convert.ToDateTime(StartDate.Month & "-" & Day & "-" & StartDate.Year & " " & TimeArr(0))
                                    Else
                                        RoutePlan.StartTime = "1/1/1900"
                                    End If
                                    If (TimeArr.Length > 1) Then
                                        RoutePlan.End_Time = Convert.ToDateTime(StartDate.Month & "-" & Day & "-" & StartDate.Year & " " & TimeArr(1))
                                    Else
                                        RoutePlan.End_Time = "1/1/1900"
                                    End If
                                Else
                                    RoutePlan.StartTime = "1/1/1900"
                                    RoutePlan.End_Time = "1/1/1900"
                                End If

                            ElseIf (SubStrArr.Length = 1) Then
                                Flak = True
                                If Not (SubStrArr(0) = "*") Then
                                    RoutePlan.UserComments = SubStrArr(0)
                                End If

                                RoutePlan.StartTime = "1/1/1900"
                                RoutePlan.End_Time = "1/1/1900"
                            ElseIf (SubStrArr.Length = 2 Or i = 0) Then
                                Flak = False
                            End If
                        Else
                            Flak = True
                            RoutePlan.StartTime = "1/1/1900"
                            RoutePlan.End_Time = "1/1/1900"
                        End If
                    End If


                    If (Flak) Then
                        RoutePlan.day = Day
                        If (Err_Desc Is Nothing) Then
                            RoutePlan.FSRPlanId = Id
                            If (Not RoutePlan.CustomerID = 0) Then
                                RoutePlan.InsertFSRPlanDetails(Err_No, Err_Desc, SqlConn, sqlcomm, transaction)
                                If CusArr(0) = "V" Then
                                    VCount += 1
                                End If
                            End If

                          
                        Else
                            log.Error(Err_Desc)
                            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_003") & "&next=CreateRoutePlan.aspx&Title=Route+Planner", False)
                            HttpContext.Current.ApplicationInstance.CompleteRequest()
                            Exit For
                        End If
                    End If
                End If

            Catch ex As Exception
                Err_No = "74045"
                If Err_Desc Is Nothing Then
                    log.Error(GetExceptionInfo(ex))
                Else
                    log.Error(Err_Desc)
                End If
                Err_Desc = ex.Message
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_003") & "&next=CreateRoutePlan.aspx&Title=Route+Planner", False)
            End Try
            Flak = False
        Next
    End Sub
    Private Function SetText(ByVal CellDate As String, ByVal InnerText As String, ByVal Val As String) As String
        Dim DivCheck As Boolean = False
        Dim SpanCheck As Boolean = False
        Try
            Dim dat As Array = CellDate.Split("-")
            Dim StrBuilder As New StringBuilder
            Dim No_Of_Visits As Integer = 0

            If (Len(InnerText) < 3) Then
                SetText = "<div><span class='dateCalendarCntrl'>" & dat(0) & "</span> <span class='visitCalendarCntrl'> Visits: " & No_Of_Visits & "</span></div>"
            Else
                StrBuilder.Append("<div><span class='dateCalendarCntrl'>" & dat(0) & "</span> ")
                Dim stringArr As Array = InnerText.Split("|")
                Dim i As Integer

                Dim ForValue As Integer
                If (stringArr.Length = 2) Then
                    ForValue = 1
                    StrBuilder.Append("<span class='visitCalendarCntrl'> Visits: " & No_Of_Visits & "</span></div>")
                ElseIf (stringArr.Length > 2) Then
                    If (stringArr(1) = "") Then
                        If (stringArr(2) = "") Then
                            If stringArr.Length > 5 Then ForValue = 5 Else ForValue = stringArr.Length - 1
                        Else
                            If stringArr.Length > 5 Then ForValue = 4 Else ForValue = stringArr.Length - 1
                        End If
                    Else
                        If stringArr.Length > 4 Then ForValue = 3 Else ForValue = stringArr.Length - 1
                    End If
                    If (stringArr(1) = "" And stringArr(2) = "") Then
                        No_Of_Visits = stringArr.Length - 3
                    Else
                        No_Of_Visits = stringArr.Length - 2
                    End If
                    StrBuilder.Append("<span class='visitCalendarCntrl'> Visits: " & No_Of_Visits & "</span></div>")
                End If

                For i = 1 To ForValue
                    Dim Str As String = Convert.ToString(stringArr(i))
                    If (i > 0) Then
                        Dim SubStrArr As Array = Str.Split("@")

                        If Not (SubStrArr(0).ToString() = "") Then
                            Dim DivName As Array = SubStrArr(0).ToString().Split("^")
                            Dim HasTimeVal As Boolean = False
                            If SubStrArr.Length > 1 Then If SubStrArr(1) <> "00:00$23:59" Then If SubStrArr(1) <> "00:00:00$23:59:00" Then HasTimeVal = True
                            If (DivName.Length = 1) Then
                                If Not (DivName(0) = "*") Then
                                    StrBuilder.Append(CountandBreak(DivName(0), HasTimeVal))
                                    DivCheck = True
                                    SpanCheck = True
                                    HasTimeVal = False
                                End If
                            Else
                                If DivName.Length > 4 Then
                                    StrBuilder.Append(vbCrLf & "" & CountandBreak(DivName(1), HasTimeVal))
                                Else
                                    StrBuilder.Append(vbCrLf & "" & CountandBreak(DivName(1), HasTimeVal))
                                End If
                                HasTimeVal = False
                                DivCheck = True
                                SpanCheck = True
                            End If
                        Else
                            If (i = 1) Then

                                Dim ConStr As Array = Val.Split("|")
                                If (ConStr.Length > 1) Then
                                    Dim TimeStr As Array = ConStr(1).ToString.Split("@")
                                    If (TimeStr.Length > 1) Then
                                        StrBuilder.Append(vbCrLf & CountandBreak(TimeStr(0), True))
                                        Dim DivTime As Array = TimeStr(1).ToString.Split("$")
                                        StrBuilder.Append("(" & DivTime(0).ToString.Substring(0, 5))
                                        StrBuilder.Append("-" & DivTime(1).ToString.Substring(0, 5) & ")</span></div>")
                                    End If

                                    '   Exit For
                                End If

                            End If

                        End If

                        If (SubStrArr.Length > 1) Then
                            Dim TimStr As Array = SubStrArr(1).Split("$")
                            Dim AtTime As String
                            Dim ToTime As String
                            Dim HasValue As Boolean = False
                            For J As Integer = 0 To TimStr.Length - 1

                                If Not (TimStr(0).ToString().Substring(0, 5) = "00:00" And TimStr(1).ToString().Substring(0, 5) = "23:59") Then
                                    If (J = 0) Then
                                        AtTime = TimStr(0).ToString().Substring(0, 5)
                                        HasValue = True
                                    End If
                                    If (J = 1) Then
                                        HasValue = True
                                        ToTime = TimStr(1).ToString().Substring(0, 5)
                                    Else
                                    End If

                                End If
                            Next
                            If HasValue Then
                                StrBuilder.Append(" (" & AtTime & "-" & ToTime & ")</span>")
                                HasValue = False
                                SpanCheck = False
                            End If
                        End If
                    End If

                    If SpanCheck Then
                        StrBuilder.Append("</span>")
                        SpanCheck = False
                    End If
                    If (DivCheck) Then
                        StrBuilder.Append("</div>")
                        DivCheck = False
                    End If
                Next

                SetText = "<Div style='height:90px; cursor:pointer; overflow:hidden;' > " & Convert.ToString(StrBuilder) & "</Div>"

            End If

        Catch ex As Exception
            Err_No = "74021"
            Err_Desc = ex.Message
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=CreateRoutePlan.aspx&Title=Route+Planner")
            Response.End()
        End Try
    End Function
    Private Function CountandBreak(ByVal Str As String, ByVal HasTimeVal As Boolean) As String
        Dim ReplacedStr As String = Str.Replace("*1*", "'")
        Dim StrBuilder As New StringBuilder
        If (ReplacedStr.Length > 19) Then
            If HasTimeVal Then
                StrBuilder.Append("<div><span class='visitDtlsCalendarCntrl'>" & ReplacedStr.Substring(0, 8))
            Else
                StrBuilder.Append("<div><span class='visitDtlsCalendarCntrl'>" & ReplacedStr.Substring(0, 18))
            End If
        Else
            If HasTimeVal And ReplacedStr.Length > 8 Then
                StrBuilder.Append("<div><span class='visitDtlsCalendarCntrl'>" & ReplacedStr.Substring(0, 8))
            ElseIf HasTimeVal And ReplacedStr.Length < 8 Then
                StrBuilder.Append("<div><span class='visitDtlsCalendarCntrl'>" & ReplacedStr)
            Else
                StrBuilder.Append("<div><span class='visitDtlsCalendarCntrl'>" & ReplacedStr)
            End If
        End If
        Return StrBuilder.ToString()
    End Function

    Private Function SetAmPm(ByVal Time As String) As String
        Try
            If (Time.Substring(0, 2) = "12" And Time.Substring(3, 2) >= "00") Then
                SetAmPm = "12:" & Time.Substring(3, 2) & " p.m."
            ElseIf (Time.Substring(0, 2) > "12") Then
                SetAmPm = Convert.ToInt32(Time.Substring(0, 2)) - 12 & ":" & Time.Substring(3, 2) & " p.m."
            Else
                SetAmPm = Time & " a.m."
            End If
        Catch ex As Exception
            Err_No = "74022"
            Err_Desc = ex.Message
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=CreateRoutePlan.aspx&Titel=Route+Planner")
            Response.End()
        End Try
    End Function
    Private Function SetToolTip(ByVal CellDate As String, ByVal InnerText As String, ByVal val As String) As String
        Dim StrBuilder As New StringBuilder
        Try
            If (Len(InnerText) < 3) Then
                SetToolTip = CellDate
            Else
                StrBuilder.Append(CellDate)
                Dim stringArr As Array = InnerText.Split("|")
                Dim i As Integer

                For i = 1 To (stringArr.Length - 1)
                    Dim Str As String = Convert.ToString(stringArr(i))
                    If (i > 0) Then
                        Dim SubStrArr As Array = Str.Split("@")

                        'If (SubStrArr(0).Length > 17) Then
                        'StrBuilder.Append(vbCrLf & SubStrArr(0))
                        'Else
                        If Not (SubStrArr(0).ToString() = "") Then
                            Dim DivName As Array = SubStrArr(0).ToString().Split("^")

                            If (DivName.Length = 1) Then
                                If Not (DivName(0) = "*") Then
                                    StrBuilder.Append(vbCrLf & SubStrArr(0))
                                End If
                            Else
                                StrBuilder.Append(vbCrLf & DivName(1) & "-" & DivName(5))
                                If DivName.Length > 4 Then StrBuilder.Append("-" & DivName(3))
                            End If
                        Else
                            If (i = 1) Then

                                Dim ConStr As Array = val.Split("|")
                                If (ConStr.Length > 1) Then
                                    Dim TimeStr As Array = ConStr(1).ToString.Split("@")
                                    If (TimeStr.Length > 1) Then
                                        StrBuilder.Append(vbCrLf & TimeStr(0))
                                        Dim DivTime As Array = TimeStr(1).ToString.Split("$")
                                        StrBuilder.Append(" at " & DivTime(0).ToString.Substring(0, 5))
                                        StrBuilder.Append(" To " & DivTime(1).ToString.Substring(0, 5))
                                    End If
                                End If
                            End If

                        End If

                        ' End If

                        If (SubStrArr.Length > 1) Then
                            Dim TimStr As Array = SubStrArr(1).Split("$")
                            For J As Integer = 0 To TimStr.Length - 1
                                If Not (TimStr(0).ToString().Substring(0, 5) = "00:00" And TimStr(1).ToString().Substring(0, 5) = "23:59") Then
                                    If (J = 0) Then
                                        'If (Not TimStr(0).ToString().Substring(0, 5) = "00:00" Or Not TimStr(1).ToString().Substring(0, 5) = "23:59") Then
                                        StrBuilder.Append(" at " & TimStr(0).ToString().Substring(0, 5))
                                        'End If
                                    End If
                                    If (J = 1) Then
                                        ' If (Not TimStr(1).ToString().Substring(0, 5) = "23:59" Or Not TimStr(0).ToString().Substring(0, 5) = "00:00") Then
                                        StrBuilder.Append(" To " & TimStr(1).ToString().Substring(0, 5))
                                        'End If
                                    End If
                                End If
                            Next
                        End If
                    End If
                Next
                SetToolTip = Convert.ToString(StrBuilder).Replace("*1*", "'")
            End If
        Catch ex As Exception
            Err_No = "74023"
            Err_Desc = ex.Message
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=CreateRoutePlan.aspx&Title=Route+Planner")
            Response.End()
        Finally
            StrBuilder = Nothing
        End Try
    End Function
    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click

    End Sub
    Protected Sub SetPanelVisibility(ByVal AddPnl As Boolean, ByVal ModPnl As Boolean, ByVal AppPanel As Boolean, ReviPanel As Boolean)
        AddandSavePanel.Visible = AddPnl
        ModifyAndUpdatePanel.Visible = ModPnl
        ApprovalPanel.Visible = AppPanel
        ReviewPanel.Visible = ReviPanel
    End Sub
    Private Sub ShowComments()
        ShowCommLnkPanel.Visible = True
        Dim objRoutePlan As New RoutePlan
        Try
            objRoutePlan.FSRPlanId = FSR_ID.Value
            CommentsGridView.DataSource = objRoutePlan.GetCommentsByFSR(Err_No, Err_Desc)
            CommentsGridView.DataBind()
            If (CommentsGridView.Rows.Count > 0) Then
                CommentsGridView.Visible = True
                NoCommentsLbl.Visible = False
            Else
                CommentsGridView.Visible = False
                NoCommentsLbl.Visible = True
            End If
        Catch ex As Exception
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
            Response.Redirect("information.aspx?mode=1&errno=" & "74024" & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=ApproveRoutePlan.aspx&Title=Route+Planner", False)
        Finally
            objRoutePlan = Nothing
        End Try
    End Sub
    Private Sub UpdateBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UpdateBtn.Click
        System.Threading.Thread.Sleep(1000)
        Action_Mode.Value = "MODIFY"
        Try
            objRoutePlan = New RoutePlan
            Dim transaction As SqlTransaction

            Dim SqlConn As SqlConnection
            Dim SqlCmd As SqlCommand
            SqlConn = objRoutePlan.GetConnection()

            transaction = SqlConn.BeginTransaction("SampleTransaction")

            If (IsApproved.Value = "Y") Then
                objRoutePlan.FSRPlanId = FSR_ID.Value
                objRoutePlan.ApprovedBy = CType(Session("User_Access"), UserAccess).UserID
                If Not (objRoutePlan.UpdateFSRPlan(Err_No, Err_Desc, SqlConn, SqlCmd, transaction)) Then
                    ''Need to modify response.redirect and show general message
                    log.Error(Err_Desc)
                    MessageBoxValidation("Error in Route plan updation:" & Err_Desc & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_004"), "Information") ' Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_004") & "&next=ModDelRoutePlan.aspx", False)
                    Exit Try
                End If
            End If

            Dim CheckVal As Boolean = True

            For i As Integer = Convert.ToInt16(StartDate.Day) To Convert.ToInt16(EndDate.Day)

                Dim HiddenCon As System.Web.UI.HtmlControls.HtmlInputHidden = TryCast(Page.FindControl("ctl00$MainContent$HDay" & i.ToString()), System.Web.UI.HtmlControls.HtmlInputHidden)
                If HiddenCon IsNot Nothing And Err_Desc Is Nothing And CheckVal = True Then
                    If Not (HiddenCon.Value = "") Then
                        Dim foundRouteRows() As DataRow = FSRDataSet.Tables("FSRPlan").Select("Day=" & i & " and FSR_Plan_ID=" & FSR_ID.Value & "")

                        If (foundRouteRows.Length > 0) Then
                            objRoutePlan.day = i
                            objRoutePlan.FSRPlanId = FSR_ID.Value

                            If (objRoutePlan.DeleteRoutePlanByFSRID(Err_No, Err_Desc, SqlConn, SqlCmd, transaction)) Then
                                CheckVal = True
                            Else
                                CheckVal = False
                                Exit For
                            End If
                        End If
                        If (CheckVal) Then
                            InsertFSRDetails(HiddenCon.Value, FSR_ID.Value, i, SqlConn, SqlCmd, transaction, Err_No, Err_Desc)
                            If Err_Desc IsNot Nothing Then
                                log.Error(Err_Desc)
                                MessageBoxValidation("Error in Route plan updation:" & Err_Desc, "Information") '  Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_003") & "&next=CreateRoutePlan.aspx&Title=Route+Planner", False)
                                Exit Sub
                            End If
                        End If

                    End If
                End If
            Next
            If (CheckVal = True) Then
                transaction.Commit()
                log.Error(Err_Desc)
            Else
                log.Error(Err_Desc)
                transaction.Rollback()
                MessageBoxValidation("Error in Route plan updation:" & Err_Desc, "Information") '  Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_004") & "&next=ModDelRoutePlan.aspx&Title=Route+Planner", False)
            End If
            objRoutePlan.CloseConnection(SqlConn)
            objLogin.SaveUserLog(Err_No, Err_Desc, "U", "CALL PLANNING", "EDIT/DELETE ROUTE PLAN", SalesRep_Name.Text, "Desc: " & Description.Text & "/ Start Date :  " & StartDate & "/ End Date :  " & EndDate & "/ Assigned Visits : " & VCount, CType(Session("User_Access"), UserAccess).UserID.ToString(), SalesRep_Name.Text, "0")
            VCount = 0
            MessageBoxValidation("Route plan  successfully updated.", "Information") 'Response.Redirect("information.aspx?mode=0&msg=Route+plan+updated+successfully&next=ModDelRoutePlan.aspx?ret=1&Title=Route+Planner", False)
        Catch ex As Exception
            Err_No = "74042"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
            Err_Desc = ex.Message
            If (Err_Desc Is Nothing) Then
                MessageBoxValidation("Route plan  successfully updated.", "Information") '  Response.Redirect("information.aspx?mode=0&msg=Route+plan+updated+successfully&next=ModDelRoutePlan.aspx?ret=1", False)
            Else
                MessageBoxValidation("Error in Route plan updation:" & Err_Desc, "Information") ' Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & Err_Desc & "&next=ModDelRoutePlan.aspx", False)
            End If
        Finally
            objRoutePlan = Nothing

        End Try
    End Sub
    Private Sub ShowCommLnk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ShowCommLnk.Click
        ShowComments()
    End Sub
    Private Sub ApprovalBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ApprovalBtn.Click
        Dim objRoutePlan As New RoutePlan
        ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
        Try
            objRoutePlan.FSRPlanId = FSR_ID.Value
            objRoutePlan.SalesRepID = CType(Session("USER_ACCESS"), UserAccess).SalesRepID
            objRoutePlan.ApprovedBy = CType(Session("USER_ACCESS"), UserAccess).UserID
            '   objRoutePlan.SalesRepID = 1
            If objRoutePlan.ApproveRoutePlan(Err_No, Err_Desc) Then

                ' Dim s As String() = SalesRep_Name.Text.Split("-")
                Dim SID As String = Session("SalesRep_ID")
                Session("FSR_ID") = Nothing
                '  If s.Length > 1 Then
                'SID = s(1)
                ' End If
                objLogin.SaveUserLog(Err_No, Err_Desc, "A", "CALL PLANNING", "APPROVE ROUTE PLAN", SID.Trim(), "Desc: " & Description.Text & "/ Start Date :  " & StartDate & "/ End Date :  " & EndDate & "/ Status: Approved", CType(Session("User_Access"), UserAccess).UserID.ToString(), SID.Trim, "0")
                Response.Redirect("information.aspx?mode=0&msg=FSR+Route+plan+approved+successfully&next=" & Session("RedirectTo") & "&Title=Route+Planner", False)
            Else
                If Err_Desc IsNot Nothing Then
                    log.Error(Err_Desc)
                End If
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=" & Session("RedirectTo") & "&Title=Route+Planner", False)
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("information.aspx?mode=1&errno=" & "74059" & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=" & Session("RedirectTo") & "&Title=Route+Planner", False)
        Finally
            objRoutePlan = Nothing
            RP_ID.Value = Nothing
            FSR_ID.Value = Nothing
        End Try
    End Sub

    Protected Sub SendCommentsBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SendCommentsBtn.Click
        Dim objRoutePlan As New RoutePlan
        Dim SqlConn As SqlConnection
        Dim SqlCmd As SqlCommand
        Dim transaction As SqlTransaction
        SqlConn = objRoutePlan.GetConnection()
        transaction = SqlConn.BeginTransaction("SampleTransaction")
        Try
            objRoutePlan.FSRPlanId = FSR_ID.Value
            objRoutePlan.Message = CommentsTxt.Text
            'objRoutePlan.SenderID = Session("SalesRep_ID")
            objRoutePlan.SenderID = CType(Session("User_Access"), UserAccess).UserID
            Dim MessageID As Integer = objRoutePlan.InsertApprovalComments(Err_No, Err_Desc, SqlConn, SqlCmd, transaction)

            If (MessageID > 0) Then
                objRoutePlan.MessageID = MessageID
                objRoutePlan.SalesRepID = Session("SalesRep_ID")
                If (objRoutePlan.AssignMessage(Err_No, Err_Desc, SqlConn, SqlCmd, transaction)) Then
                    Response.Redirect("information.aspx?mode=0&msg=Your+comment+has+been+sent+successfully&next=" & Session("RedirectTo") & "&Title=Route+Planner", False)
                    transaction.Commit()
                Else
                    transaction.Rollback()
                    log.Error(Err_Desc)
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_003") & "&next=" & Session("RedirectTo") & "&Title=Route+Planner", False)
                End If
            Else
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_003") & "&next=" & Session("RedirectTo") & "&Title=Route+Planner", False)
                Exit Try
            End If
        Catch ex As Exception
            If Err_Desc IsNot Nothing Then
                log.Error(Err_Desc)
            Else
                log.Error(GetExceptionInfo(ex))
            End If
            Response.Redirect("information.aspx?mode=1&errno=" & "74058" & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_003") & "&next=" & Session("RedirectTo") & "&Title=Route+Planner", False)
        Finally
            objRoutePlan.CloseConnection(SqlConn)
            objRoutePlan = Nothing
        End Try

    End Sub
    Private Sub HideCommLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HideCommLink.Click
        CommentsGridView.Visible = False
        NoCommentsLbl.Visible = False
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
            log.Error(GetExceptionInfo(ex))

        End Try
        Return bRetval
    End Function

    Private Sub ReviewButton_Click(sender As Object, e As EventArgs) Handles ReviewButton.Click
        Response.Redirect("ReviewRoutePlan.aspx")
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager2.RadAlert(str, 330, 80, Title, "alertCallBackFn")
        Exit Sub
    End Sub


End Class