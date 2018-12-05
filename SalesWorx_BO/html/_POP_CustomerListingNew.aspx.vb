Imports System.Web.UI.WebControls
Imports System.Resources
Imports SalesWorx.BO.Common
Imports System.Web
Imports System.Data
Imports System.Text
Imports System.Collections
Imports System.Web.UI.HtmlControls
Imports System.Web.UI
Imports System.Threading
Imports log4net
Public Class _POP_CustomerListingNew
    Inherits System.Web.UI.Page
    Dim ErrorResource As ResourceManager
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim Arr As Array
    Dim IDArr As New ArrayList
    Dim RetValue As Boolean
    Dim Ucomments As String = ""
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Dim oGeocodeList As List(Of String)

    Private Shared _strDefLat As String = ConfigurationSettings.AppSettings("DefaultLat")
    Private Shared _strDefLong As String = ConfigurationSettings.AppSettings("DefaultLong")
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If IsNothing(Session("USER_ACCESS")) Then
        '    Response.Redirect("Login.aspx")
        'End If
        Try
            '     ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())

            If (Not IsPostBack) Then
                Cache.Remove("CustomerList")
                Cache.Remove("DDDays")
                Cache.Remove("VisitPlannedList")
                Cache.Remove("GeoMarkers")
                DayRef.Value = Request.QueryString("DayRef")
                MultiDropBind()
                LoadCustomerGrid()

                'BindEmptymap()

                If Not (Session("HDay" & Request.QueryString("DayRef") & "") Is Nothing) Then


                    Dim HValu As String = Session("HDay" & Request.QueryString("DayRef") & "")
                    Arr = HValu.Split("|")
                    If (Arr(0) = "O") Then
                        MoveCopyPanel.Visible = True
                    End If
                    If (Arr.Length >= 2) Then
                        If Not (Arr(1) = "") Then
                            Dim SubArr As Array = Arr(1).ToString.Split("@")
                            If (SubArr.Length = 1) Then
                                If Not (SubArr(0) = "*") Then
                                    UserComments.Text = Arr(1).ToString()
                                    ComVal.Value = Arr(1).ToString()
                                    ComString.Value = ComVal.Value
                                End If
                            Else
                                ComVal.Value = Arr(1).ToString()
                                ComString.Value = ComVal.Value
                            End If
                        End If
                        SetValueForDDs(HValu)
                    End If
                    bindVisitsPlanned()
                    Dim geoval = Mapmarkers()  'lOAD map
                    Bindmap(geoval)
                End If
                Ucomments = ComString.Value

            End If
            ' Dim geoval = Mapmarkers()
            '  Bindmap(geoval)
            ' Bindmap(Cache("GeoMarkers"))


            If IsPostBack Then MaintainState()
        Catch ex As Exception
            Err_No = "74034"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
            ' ErrMsg.Text = AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001")
            ErrMsg.Text = "<span class='error'>Error:</SPAN><span class='errormsg' style='font-size:.82em'>" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & ".</span>"
            '  Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & ErrorResource.GetObject("GeneralErrMsg") & "&next=ViewDefPlanNew.aspx", False)
        End Try

    End Sub

    Private Sub MaintainState()
        Try


            Dim ValArr As Array = ValueHolder.Value.Split("||")
            If (ValArr.Length > 1) Then
                For i As Integer = 0 To ValArr.Length - 2
                    Dim ValHolderArr As Array = ValArr(i).ToString.Split("$")
                    If ValHolderArr.Length = 3 Then
                        Dim item As ListItem = New ListItem(ValHolderArr(0) & "$" & ValHolderArr(2))
                        If CBool(ValHolderArr(1)) Then
                            If Not (SelectedList.Items.FindByValue(ValHolderArr(0) & "$" & ValHolderArr(2)) Is Nothing) Then
                                Dim Liitem As ListItem = New ListItem(SelectedList.Items.FindByValue(ValHolderArr(0).ToString() & "$" & ValHolderArr(2).ToString()).Text, ValHolderArr(0).ToString() & "$" & ValHolderArr(2).ToString())
                                SelectedList.Items.Remove(Liitem)
                            End If
                            SelectedList.Items.Add(item)
                        End If
                    End If
                Next
            End If


            Dim NotValArr As Array = ValueNotHolder.Value.Split("||")
            If NotValArr.Length > 1 Then
                For j As Integer = 0 To NotValArr.Length - 2
                    If Not (SelectedList.Items.FindByValue(NotValArr(j)) Is Nothing) Then
                        Dim Liitem As ListItem = New ListItem(SelectedList.Items.FindByValue(NotValArr(j).ToString()).Text, NotValArr(j).ToString())
                        SelectedList.Items.Remove(Liitem)
                    End If
                Next
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            'ErrMsg.Text = AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005")
            ErrMsg.Text = "<span class='error'>Error:</SPAN><span class='errormsg' style='font-size:.82em'>" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & ".</span>"
        End Try
    End Sub
    Protected Sub btnAddtoVisit(ByVal sender As Object, ByVal e As EventArgs)

        'Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
        'Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)

        'If Cache("VisitPlannedList") Is Nothing Then
        '    Dim CustomerDt As New DataTable
        '    CustomerDt.Columns.Add("Customer_ID")
        '    CustomerDt.Columns.Add("Site_Use_ID")
        '    CustomerDt.Columns.Add("Customer_No")
        '    CustomerDt.Columns.Add("Customer_Name")
        '    CustomerDt.Columns.Add("Sequence")
        '    Cache("VisitPlannedList") = CustomerDt
        '    CustomerDt = Nothing
        'End If

        'Dim PlannedVisits As New DataTable
        'PlannedVisits = CType(Cache("VisitPlannedList"), DataTable).Copy


        'If PlannedVisits.Select("Customer_ID=" & CType(row.Cells(0).FindControl("Customer_ID"), HiddenField).Value & " and Site_Use_ID=" & CType(row.Cells(0).FindControl("SiteID"), HiddenField).Value).Length <= 0 Then
        '    Dim dr As DataRow
        '    dr = PlannedVisits.NewRow
        '    dr("Customer_ID") = CType(row.Cells(0).FindControl("Customer_ID"), HiddenField).Value
        '    dr("Site_Use_ID") = CType(row.Cells(0).FindControl("SiteID"), HiddenField).Value
        '    dr("Customer_No") = CType(row.Cells(0).FindControl("Customer_No"), HiddenField).Value
        '    dr("Customer_Name") = CType(row.Cells(1).FindControl("Customer_Name"), Label).Text
        '    dr("Sequence") = PlannedVisits.Rows.Count + 1
        '    PlannedVisits.Rows.Add(dr)
        'End If
        'Cache("VisitPlannedList") = PlannedVisits
        'PlannedVisits = Nothing
        'bindVisitsPlanned()


        Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
        Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)

        If Cache("VisitPlannedList") Is Nothing Then
            Dim CustomerDt As New DataTable
            CustomerDt.Columns.Add("Customer_ID")
            CustomerDt.Columns.Add("Site_Use_ID")
            CustomerDt.Columns.Add("Customer_No")
            CustomerDt.Columns.Add("Customer_Name")
            CustomerDt.Columns.Add("Sequence")
            Cache("VisitPlannedList") = CustomerDt
            CustomerDt = Nothing
        End If

        Dim PlannedVisits As New DataTable
        PlannedVisits = CType(Cache("VisitPlannedList"), DataTable).Copy
        Dim V_custID = CType(row.Cells(0).FindControl("Customer_ID"), HiddenField).Value
        Dim V_site = CType(row.Cells(0).FindControl("SiteID"), HiddenField).Value
        Dim result() As DataRow = PlannedVisits.Select("Customer_ID='" & V_custID & "' AND Site_Use_ID='" & V_site & "'")
        ' Table.Select("Size >= 230 AND Sex = 'm'")
        If result.Count = 0 Then



            'If PlannedVisits.Select("Customer_ID=" & CType(row.Cells(0).FindControl("Customer_ID"), HiddenField).Value & " and Site_Use_ID=" & CType(row.Cells(0).FindControl("SiteID"), HiddenField).Value).Length <= 0 Then
            Dim dr As DataRow
            dr = PlannedVisits.NewRow
            dr("Customer_ID") = CType(row.Cells(0).FindControl("Customer_ID"), HiddenField).Value
            dr("Site_Use_ID") = CType(row.Cells(0).FindControl("SiteID"), HiddenField).Value
            dr("Customer_No") = CType(row.Cells(0).FindControl("Customer_No"), HiddenField).Value
            dr("Customer_Name") = CType(row.Cells(1).FindControl("Customer_Name"), Label).Text
            dr("Sequence") = PlannedVisits.Rows.Count + 1
            PlannedVisits.Rows.Add(dr)
        End If
        Cache("VisitPlannedList") = PlannedVisits
        PlannedVisits = Nothing
        bindVisitsPlanned()
    End Sub
    Sub RemovefromVisitPlan(ByVal sender As Object, ByVal e As EventArgs)
        Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
        Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
        Dim PlannedVisits As New DataTable
        PlannedVisits = CType(Cache("VisitPlannedList"), DataTable).Copy
        Dim seldr() As DataRow
        seldr = PlannedVisits.Select("Customer_ID=" & CType(row.Cells(0).FindControl("CustomerID"), HiddenField).Value & " and Site_Use_ID=" & CType(row.Cells(0).FindControl("SiteID1"), HiddenField).Value)
        If seldr.Length > 0 Then
            For Each dr As DataRow In seldr
                PlannedVisits.Rows.Remove(dr)
            Next
        End If
        Dim index As Integer
        index = 0
        For Each dr As DataRow In PlannedVisits.Rows
            dr("Sequence") = index + 1
            index = index + 1
        Next

        Dim _dv As DataView
        _dv = PlannedVisits.DefaultView()
        _dv.Sort = "Sequence Asc"
        PlannedVisits = _dv.ToTable

        Cache("VisitPlannedList") = PlannedVisits
        PlannedVisits = Nothing
        bindVisitsPlanned()
    End Sub

    Sub MoveUp(ByVal sender As Object, ByVal e As EventArgs)
        Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
        Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)

        Dim PlannedVisits As New DataTable
        PlannedVisits = CType(Cache("VisitPlannedList"), DataTable).Copy
        PlannedVisits.Rows(row.RowIndex)("Sequence") = PlannedVisits.Rows(row.RowIndex)("Sequence") - 1
        PlannedVisits.Rows(row.RowIndex - 1)("Sequence") = PlannedVisits.Rows(row.RowIndex - 1)("Sequence") + 1

        Dim _dv As DataView
        _dv = PlannedVisits.DefaultView()
        _dv.Sort = "Sequence Asc"

        PlannedVisits = _dv.ToTable

        Cache("VisitPlannedList") = PlannedVisits
        PlannedVisits = Nothing
        bindVisitsPlanned()
    End Sub

    Sub MoveDown(ByVal sender As Object, ByVal e As EventArgs)
        Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
        Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)

        Dim PlannedVisits As New DataTable
        PlannedVisits = CType(Cache("VisitPlannedList"), DataTable).Copy
        PlannedVisits.Rows(row.RowIndex)("Sequence") = PlannedVisits.Rows(row.RowIndex)("Sequence") + 1
        PlannedVisits.Rows(row.RowIndex + 1)("Sequence") = PlannedVisits.Rows(row.RowIndex + 1)("Sequence") - 1

        Dim _dv As DataView
        _dv = PlannedVisits.DefaultView()
        _dv.Sort = "Sequence Asc"

        PlannedVisits = _dv.ToTable

        Cache("VisitPlannedList") = PlannedVisits
        PlannedVisits = Nothing
        bindVisitsPlanned()
    End Sub

    Private Sub MultiDropBind()
        Dim SDate As Date
        Dim EDate As Date
        SDate = Convert.ToDateTime(Session("Start_Date").Substring(3, 3) & Session("Start_Date").Substring(0, 3) & Session("Start_Date").Substring(6, 4))
        EDate = Convert.ToDateTime(Session("End_Date").Substring(3, 3) & Session("End_Date").Substring(0, 3) & Session("End_Date").Substring(6, 4))
        Dim objRoute As New RoutePlan
        Try
            objRoute.day = Request.QueryString("DayRef")
            objRoute.DefPlanId = Session("Default_Plan_ID")
            objRoute.SDate = SDate
            objRoute.EDate = EDate
            objRoute.DayRef = DayRef.Value
            If Cache("DDDays") Is Nothing Then
                Cache("DDDays") = objRoute.GetMoveCopyDays(Err_No, Err_Desc)
            End If
            MultiCheck.DataSource = CType(Cache("DDDays"), DataTable)
            MultiCheck.DataBind()
        Catch ex As Exception
            Err_No = "74025"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        Finally
            objRoute = Nothing
        End Try
    End Sub
    Private Sub SetValueForDDs_old(ByVal HValu As String)
        Try
            Dim CusIDArr As New ArrayList
            Dim HValArr As Array = HValu.Split("|")
            Dim str As String = Session("None")
            Dim Chk As CheckBox
            Dim CusID As Label
            Dim CusName As Label
            Dim UserID As Label
            'Dim StartHH As DropDownList
            'Dim StartMM As DropDownList
            'Dim EndHH As DropDownList
            ' Dim EndMM As DropDownList
            Dim CusString As New StringBuilder
            For Each row As GridViewRow In CustomerGrid.Rows
                CusID = TryCast(row.FindControl("Customer_ID"), Label)
                CusName = TryCast(row.FindControl("Customer_Name"), Label)
                UserID = TryCast(row.FindControl("Site_Use_ID"), Label)
                For i As Integer = 1 To HValArr.Length - 1
                    'Dim SubArray As Array = HValArr(i).ToString.Split("@")
                    Dim SubArray As Array = HValArr(i).ToString.Split("-")
                    MoveCopyPanel.Visible = True
                    ' If (SubArray(0).ToString() = CusID.Text.Trim() & "-" & CusName.Text.Trim & "-" & UserID.Text) Then
                    'If (SubArray(0).ToString().Contains(CusID.Text.Trim()) And SubArray(0).ToString().Contains(UserID.Text.Trim())) Then
                    '                    If (SubArray(0).ToString().IndexOf(CusID.Text.Trim() & "-") <> -1 And SubArray(0).ToString().IndexOf("-" & UserID.Text.Trim()) <> -1) Then
                    If SubArray(0).ToString() = CusID.Text.Trim() Then
                        Chk = TryCast(row.FindControl("TimeChk"), CheckBox)

                        '' StartHH = TryCast(row.FindControl("StartHH"), DropDownList)
                        'StartMM = TryCast(row.FindControl("StartMM"), DropDownList)
                        ' EndHH = TryCast(row.FindControl("EndHH"), DropDownList)
                        'EndMM = TryCast(row.FindControl("EndMM"), DropDownList)
                        Chk.Checked = True


                        'If (SubArray.Length > 1) Then
                        '    Dim TimeArray As Array = SubArray(1).ToString.Split("$")
                        '    If (TimeArray(0).ToString.Substring(0, 2) <> "00" Or TimeArray(0).ToString.Substring(3, 2) Or TimeArray(1).ToString.Substring(0, 2) <> "23" Or TimeArray(1).ToString.Substring(3, 2) <> "59") Then
                        '        TimeSelection.Checked = True
                        '        StartHH.SelectedIndex = StartHH.Items.IndexOf(StartHH.Items.FindByText(TimeArray(0).ToString.Substring(0, 2)))
                        '        If Not (TimeArray(0).ToString.Substring(3, 2) = "59") Then
                        '            StartMM.SelectedIndex = StartMM.Items.IndexOf(StartMM.Items.FindByText(TimeArray(0).ToString.Substring(3, 2)))
                        '        End If
                        '    Else
                        '    End If

                        '    'If Not (TimeArray(1).ToString.Substring(0, 2) = "23") Then
                        '    EndHH.SelectedIndex = EndHH.Items.IndexOf(EndHH.Items.FindByText(TimeArray(1).ToString.Substring(0, 2)))
                        '    '    If Not (TimeArray(1).ToString.Substring(3, 2) = "59") Then
                        '    EndMM.SelectedIndex = EndMM.Items.IndexOf(EndMM.Items.FindByText(TimeArray(1).ToString.Substring(3, 2)))

                        'Else

                        'End If

                    End If
                Next

            Next
            ' MakeTimerDDVisible()
        Catch ex As Exception
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If

            ' ErrMsg.Text = AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005")
            ErrMsg.Text = "<span class='error'>Error:</SPAN><span class='errormsg' style='font-size:.82em'>" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & ".</span>"
        End Try
    End Sub

    Private Sub SetValueForDDs(ByVal HValu As String)
        Try
            Dim CusIDArr As New ArrayList
            Dim HValArr As Array = HValu.Split("|")
            Dim str As String = Session("None")
            Dim CustomerDt As New DataTable

            If Cache("VisitPlannedList") Is Nothing Then
                CustomerDt.Columns.Add("Customer_ID")
                CustomerDt.Columns.Add("Site_Use_ID")
                CustomerDt.Columns.Add("Customer_No")
                CustomerDt.Columns.Add("Customer_Name")
                CustomerDt.Columns.Add("Sequence", System.Type.GetType("System.Int32"))
                For i As Integer = 2 To HValArr.Length - 1
                    If HValArr(i).ToString.Trim <> "" Then
                        Dim SubArray As Array = HValArr(i).ToString.Split("^")
                        If SubArray.Length > 0 Then
                            If SubArray(4) = "1" Then
                                chk_Optmize.Checked = True
                            Else
                                chk_Optmize.Checked = False
                            End If
                        End If

                        Dim dr As DataRow
                        dr = CustomerDt.NewRow
                        dr(0) = SubArray(0)
                        dr(1) = SubArray(2)
                        dr(2) = SubArray(5)
                        dr(3) = SubArray(1)
                        dr(4) = Val(SubArray(3))
                        CustomerDt.Rows.Add(dr)
                    End If
                Next
                Cache("VisitPlannedList") = CustomerDt
            Else
                CustomerDt = CType(Cache("VisitPlannedList"), DataTable).Copy()
            End If
            If CustomerDt.Rows.Count > 0 Then
                MoveCopyPanel.Visible = True
            End If
            bindVisitsPlanned()
        Catch ex As Exception
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If

            ' ErrMsg.Text = AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005")
            ErrMsg.Text = "<span class='error'>Error:</SPAN><span class='errormsg' style='font-size:.82em'>" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & ".</span>"
        End Try
    End Sub
    Private Sub LoadCustomerGrid()
        Try

            Dim objRoute As New RoutePlan
            Try
                '  objRoute.SalesRepIDprop = Session("USER_ACCESS").SalesRepID()
                objRoute.SalesRepID = Session("SalesRep_ID")
                If Cache("CustomerList") Is Nothing Then
                    Cache("CustomerList") = objRoute.GetCustomerListForRoutePlan(Err_No, Err_Desc, selectSearchQuery(), txt_Filter.Text)
                End If
                FilterCustomerGrid.DataSource = CType(Cache("CustomerList"), DataTable)
                FilterCustomerGrid.DataBind()
            Catch ex As Exception
                log.Error(GetExceptionInfo(ex))
                ' ErrMsg.Text = AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005")
                ErrMsg.Text = "<span class='error'>Error:</SPAN><span class='errormsg' style='font-size:.82em'>" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & ".</span>"
            Finally
                objRoute = Nothing
            End Try
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            '    ErrMsg.Text = AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005")
            ErrMsg.Text = "<span class='error'>Error:</SPAN><span class='errormsg' style='font-size:.82em'>" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & ".</span>"
        End Try
    End Sub

    Protected Sub DayOffBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles DayOffBtn.Click
        CallCloseEvent("O", "")
    End Sub

    Protected Sub ResetBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ResetBtn.Click
        If Request.QueryString("DType") = "M" Or (Request.QueryString("DType") = "V") Then
            CallCloseEvent("X", "")
        Else
            CallCloseEvent("X", "Reset")
        End If
    End Sub
    Protected Sub CallCloseEvent(ByVal DayValue As String, ByVal ButtonType As String)
        Try

            Dim strScript As String
            Dim Time As String = ""
            'Dim Ucomments As String = ""
            strScript = "<script language='javascript'>"
            If (Request.QueryString("Mode") = "ADD") Then
                ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_Action_Mode.value ='ADD';"
                strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_Action_Mode','ADD');"
            ElseIf (Request.QueryString("Mode") = "MODIFY") Then
                '' strScript += "GetRadWindow().BrowserWindow.MainContent_Action_Mode.value ='MODIFY';"
                strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_Action_Mode','MODIFY');"
            End If
            ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_CheckRefresh.value ='Y';"
            ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_FromPopUp.value ='Y';"
            ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_Cell" & Request.QueryString("DayRef") & ".value ='Y';"

            strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_CheckRefresh','Y');"
            strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_FromPopUp','Y');"
            strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_Cell" & Request.QueryString("DayRef") & "','Y');"

            If (Not ButtonType = "Reset") Then
                If (DayValue = "O") Then
                    If (UserComments.Text <> "") Then
                        Ucomments = "|" & UserComments.Text.Trim()
                    Else
                        Ucomments = "|*"
                    End If
                Else
                    Ucomments = "|" & UserComments.Text
                End If
                If (TimeSelection.Checked And UserComments.Text <> "") Then

                End If
            End If
            Dim CommStr As String

            CommStr = Ucomments
            ComString.Value = Ucomments

            If (DayValue = "V") Then
                Dim CustomeInfrm As String = GetCusInformation()
                ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_HDay" & Request.QueryString("DayRef") & ".value='" & DayValue & CommStr & CustomeInfrm & "';"
                strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_HDay" & Request.QueryString("DayRef") & "','" & DayValue & CommStr & CustomeInfrm & "');"
                Session("HDay" & Request.QueryString("DayRef") & "") = DayValue & CommStr & CustomeInfrm
            Else
                ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_HDay" & Request.QueryString("DayRef") & ".value='" & DayValue & CommStr & "';"
                strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_HDay" & Request.QueryString("DayRef") & "','" & DayValue & CommStr & "');"
                Session("HDay" & Request.QueryString("DayRef") & "") = DayValue & CommStr
            End If
            ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_DayRef.value='" & Request.QueryString("DayRef") & "';"
            strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_DayRef','" & Request.QueryString("DayRef") & "');"
            strScript += "var par = GetRadWindow().BrowserWindow.location.search;"
            strScript += "var buttonid = par.substring(par.indexOf('=') + 1, par.indexOf('&'));"

            ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_Button1.click();"
            strScript += "GetRadWindow().BrowserWindow.ButtonClick();"


            strScript += "var oWindow = GetRadWindow();"
            strScript += " oWindow.close(null);"
            strScript += "</script>"

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "StrScript", _
    strScript, False)
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            '  ErrMsg.Text = AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005")
            ErrMsg.Text = "<span class='error'>Error:</SPAN><span class='errormsg' style='font-size:.82em'>" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & ".</span>"
        End Try
    End Sub

    Private Function GetCusInformation_OLD() As String
        Try


            Dim CusString As New StringBuilder

            '  Dim Chk As CheckBox
            ' Dim CusID As Label
            ' Dim SiteID As Label
            ' Dim CusName As Label
            ' Dim StartHH As DropDownList
            'Dim StartMM As DropDownList
            'Dim EndHH As DropDownList
            ' Dim EndMM As DropDownList
            '  TimeSelPanel.Update()
            'For Each row As GridViewRow In CustomerGrid.Rows
            '    Chk = TryCast(row.FindControl("TimeChk"), CheckBox)
            '    CusID = TryCast(row.FindControl("Customer_ID"), Label)
            '    SiteID = TryCast(row.FindControl("Site_Use_ID"), Label)
            '    CusName = TryCast(row.FindControl("Customer_Name"), Label)
            '    'StartHH = TryCast(row.FindControl("StartHH"), DropDownList)
            '    '  StartMM = TryCast(row.FindControl("StartMM"), DropDownList)
            '    'EndHH = TryCast(row.FindControl("EndHH"), DropDownList)
            '    'EndMM = TryCast(row.FindControl("EndMM"), DropDownList)



            '    If (Chk.Checked) Then
            '        CusString.Append("|" & CusID.Text & "-" & CusName.Text.Trim().Replace("'", "*1*") & "-" & SiteID.Text.Trim())
            '        'If (TimeSelection.Checked) Then
            '        '    CusString.Append("@" & StartHH.SelectedItem.Text & ":" & StartMM.SelectedItem.Text)
            '        '    CusString.Append("$" & EndHH.SelectedItem.Text & ":" & EndMM.SelectedItem.Text)
            '        'End If

            '    End If

            'Next
            Dim objRoute As New RoutePlan
            Dim CustomerDt As New DataTable

            If SelectedList.Items.Count() > 0 Then
                For i As Integer = 0 To SelectedList.Items.Count() - 1
                    Dim Val As String = SelectedList.Items(i).Text
                    Dim ValArr As Array = Val.Split("$")

                    If (ValArr.Length = 2) Then
                        Dim _tempCustomerID As String = ValArr(0)
                        '  Dim _tempSHH As String = ValArr(1)
                        ' Dim _tempSMM As String = ValArr(2)
                        ' Dim _tempEHH As String = ValArr(3)
                        ' Dim _tempEMM As String = ValArr(4)
                        Dim _tempSiteID As String = ValArr(1)
                        Dim CustomerName As String

                        If Cache("CustomerList") Is Nothing Then
                            objRoute.SalesRepID = Session("SalesRep_ID")
                            Cache("CustomerList") = objRoute.GetCustomerListForRoutePlan(Err_No, Err_Desc, selectSearchQuery, txt_Filter.Text)
                        End If
                        CustomerDt = CType(Cache("CustomerList"), DataTable)
                        Dim foundRows() As DataRow = CustomerDt.Select("Customer_ID=" & _tempCustomerID & " and Site_Use_ID=" & _tempSiteID & "")
                        If foundRows.Length > 0 Then
                            CustomerName = foundRows(0).Item("Customer_Name")
                            CusString.Append("|" & _tempCustomerID & "-" & CustomerName.Trim().Replace("'", "*1*") & "-" & _tempSiteID.Trim())
                            If (TimeSelection.Checked) Then
                                'CusString.Append("@" & _tempSHH & ":" & _tempSMM)
                                ' CusString.Append("$" & _tempEHH & ":" & _tempEMM)
                            End If
                        End If
                    End If
                Next
            End If
            objRoute = Nothing
            CustomerDt = Nothing
            Cache.Remove("CustomerList")
            SelectedList.Items.Clear()
            Return CusString.ToString()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            ' ErrMsg.Text = AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005")
            ErrMsg.Text = "<span class='error'>Error:</SPAN><span class='errormsg' style='font-size:.82em'>" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & ".</span>"
        End Try
    End Function
    Private Function GetCusInformation() As String
        Try


            Dim CusString As New StringBuilder
            Dim AllowOptimization As String = "0"
            If chk_Optmize.Checked Then
                AllowOptimization = "1"
            End If
            For Each gr As GridViewRow In CustomerGrid.Rows
                Dim _tempCustomerID As String
                Dim CustomerName As String
                Dim CustomerNo As String
                Dim _tempSiteID As String
                Dim Sequence As String
                _tempCustomerID = CType(gr.FindControl("CustomerID"), HiddenField).Value
                _tempSiteID = CType(gr.FindControl("SiteID1"), HiddenField).Value
                CustomerName = CType(gr.FindControl("CustomerName"), HiddenField).Value
                Sequence = CType(gr.FindControl("Sequence"), HiddenField).Value
                CustomerNo = CType(gr.FindControl("CustomerNo"), HiddenField).Value
                CusString.Append("|" & _tempCustomerID & "^" & CustomerName.Trim().Replace("'", "*1*") & "^" & _tempSiteID.Trim() & "^" & Sequence & "^" & AllowOptimization & "^" & CustomerNo)
            Next
            Cache.Remove("CustomerList")
            Return CusString.ToString()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            ' ErrMsg.Text = AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005")
            ErrMsg.Text = "<span class='error'>Error:</SPAN><span class='errormsg' style='font-size:.82em'>" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & ".</span>"
        End Try
    End Function
    Private Sub CustomerGrid_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles CustomerGrid.RowDataBound
        Try

            'Commented by swpana on 31-may-2015 when new look and feel added
            ''  Dim li As ListItem

            'If e.Row.RowType = DataControlRowType.DataRow Then

            '    Dim CusID As Label = TryCast(e.Row.FindControl("Customer_ID"), Label)
            '    Dim SiteID As Label = TryCast(e.Row.FindControl("Site_Use_ID"), Label)
            '    ' Dim ddSD As DropDownList = TryCast(e.Row.FindControl("StartHH"), DropDownList)
            '    '  Dim ddSDMM As DropDownList = TryCast(e.Row.FindControl("StartMM"), DropDownList)

            '    'For i As Integer = 0 To 23
            '    '    li = New ListItem
            '    '    li.Text = IIf(i <= 9, "0" + i.ToString(), i.ToString())
            '    '    li.Value = IIf(i <= 9, "0" + i.ToString(), i.ToString())
            '    '    ddSD.Items.Add(li)
            '    '    li = Nothing
            '    'Next

            '    'ddSDMM.Items.Add("00")
            '    'ddSDMM.Items.Add("15")
            '    'ddSDMM.Items.Add("30")
            '    'ddSDMM.Items.Add("45")
            '    'ddSDMM.Items.Add("59")


            '    ' Dim ddED As DropDownList = TryCast(e.Row.FindControl("EndHH"), DropDownList)
            '    ' Dim ddEMM As DropDownList = TryCast(e.Row.FindControl("EndMM"), DropDownList)
            '    ' For i As Integer = 0 To 23
            '    'li = New ListItem
            '    'li.Text = IIf(i <= 9, "0" + i.ToString(), i.ToString())
            '    ' li.Value = IIf(i <= 9, "0" + i.ToString(), i.ToString())
            '    '  ddED.Items.Add(li)
            '    ' li = Nothing
            '    ' Next
            '    '
            '    'For MM As Integer = 0 To 59
            '    '    li = New ListItem
            '    '    li.Text = IIf(MM <= 9, "0" + MM.ToString(), MM.ToString())
            '    '    li.Value = IIf(MM <= 9, "0" + MM.ToString(), MM.ToString())
            '    '    ddEMM.Items.Add(li)
            '    '    li = Nothing
            '    'Next MM

            '    ' ddEMM.Items.Add("00")
            '    'ddEMM.Items.Add("15")
            '    'ddEMM.Items.Add("30")
            '    'ddEMM.Items.Add("45")
            '    '.Items.Add("59")

            '    Dim Visits As Label = TryCast(e.Row.FindControl("NoOfVisits"), Label)
            '    Visits.Text = GetNoVisits(CusID.Text, SiteID.Text)
            '    'ddED.SelectedIndex = ddED.Items.IndexOf(ddED.Items.FindByText("23"))
            '    ' ddEMM.SelectedIndex = ddEMM.Items.IndexOf(ddEMM.Items.FindByText("59"))


            '    Dim chkBx As CheckBox = TryCast(e.Row.FindControl("TimeChk"), CheckBox)
            '    Dim lbl As Label = CType(e.Row.FindControl("Customer_ID"), Label)
            '    Dim SiteLbl As Label = CType(e.Row.FindControl("Site_Use_ID"), Label)


            '    ''Starts Here Added for Javascript
            '    'chkBx.Attributes.Add("onclick", "showHideDropDowns(this, '" + ddSD.ClientID + "','" + ddSDMM.ClientID + "','" + ddED.ClientID + "','" + ddEMM.ClientID + "');")
            '    ''End Here

            '    ''Commented For Javascript
            '    If Not SelectedList.Items.FindByValue(lbl.Text & "$" & SiteLbl.Text) Is Nothing Then
            '        chkBx.Checked = True
            '        Dim TimeArr As Array = SelectedList.Items.FindByValue(lbl.Text & "$" & SiteLbl.Text).Text.Split("$")
            '        ' ddSD.SelectedIndex = ddSD.Items.IndexOf(ddSD.Items.FindByText(TimeArr(1)))
            '        ' ddSDMM.SelectedIndex = ddSDMM.Items.IndexOf(ddSDMM.Items.FindByText(TimeArr(2)))
            '        ' ddED.SelectedIndex = ddED.Items.IndexOf(ddED.Items.FindByText(TimeArr(3)))
            '        ' ddEMM.SelectedIndex = ddEMM.Items.IndexOf(ddEMM.Items.FindByText(TimeArr(4)))
            '        If (TimeSelection.Checked) Then
            '            ' ddSD.Attributes.Add("style", "display:inline")
            '            'ddSDMM.Attributes.Add("style", "display:inline")
            '            ' ddED.Attributes.Add("style", "display:inline")
            '            ' ddEMM.Attributes.Add("style", "display:inline")
            '        End If
            '    Else
            '        chkBx.Checked = False
            '    End If

            '    ''Javascript startes here


            '    ''Ends here

            'End If

            Try
                If e.Row.RowType = DataControlRowType.DataRow Then


                    Dim CusID As Label = TryCast(e.Row.FindControl("Customer_ID"), Label)
                    Dim SiteID As Label = TryCast(e.Row.FindControl("Site_Use_ID"), Label)

                    Dim Visits As Label = TryCast(e.Row.FindControl("NoOfVisits"), Label)
                    Visits.Text = GetNoVisits(CusID.Text, SiteID.Text)


                    Dim Sequence As HiddenField = TryCast(e.Row.FindControl("Sequence"), HiddenField)
                    If Sequence.Value = "1" Then
                        TryCast(e.Row.FindControl("btnMoveup"), ImageButton).Visible = False
                    End If
                    If Not Cache("VisitPlannedList") Is Nothing Then
                        Dim dt As New DataTable
                        dt = CType(Cache("VisitPlannedList"), DataTable)
                        If Sequence.Value = dt.Rows.Count Then
                            TryCast(e.Row.FindControl("btnMoveDown"), ImageButton).Visible = False
                        End If
                        If dt.Rows.Count = 1 Then
                            TryCast(e.Row.FindControl("btnMoveup"), ImageButton).Visible = False
                            TryCast(e.Row.FindControl("btnMoveDown"), ImageButton).Visible = False
                        End If
                    End If
                End If
                ' CustomerGrid.Columns(2).ItemStyle.Wrap = True
            Catch ex As Exception

            End Try
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            '  ErrMsg.Text = AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001")
            ErrMsg.Text = "<span class='error'>Error:</SPAN><span class='errormsg' style='font-size:.82em'>" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & ".</span>"
        End Try
    End Sub
    Private Function GetNoVisits(ByVal CusID As String, ByVal SiteID As String) As String
        Try
            Dim Str As New StringBuilder()
            Str.Append("")
            'Dim Val As String
            For i As Integer = 0 To 31

                'Val = Session("HDay" & i & "")

                Dim HValu As String = Session("HDay" & i & "")
                If Not (HValu Is Nothing) Then
                    Dim HValArr As Array = HValu.Split("|")
                    For j As Integer = 1 To HValArr.Length - 1
                        Dim SubArray As Array = HValArr(j).ToString.Split("^")
                        If SubArray.Length > 2 Then
                            If SubArray(0).ToString() = CusID And SubArray(2) = SiteID Then
                                Str.Append(i)
                                Str.Append(",")
                            End If
                        End If
                    Next
                End If

                'If Not (Val Is Nothing) Then
                'If Val.Contains(CusID.ToString().Trim() & "-") And Val.Contains(SiteID.Trim() & "-") Then
                'If (Val.IndexOf(CusID.ToString().Trim() & "-") <> -1 And Val.IndexOf("-" & SiteID.ToString().Trim()) <> -1) Then
                '     Str.Append(i)
                '      Str.Append(",")
                '   End If
                'End If
            Next
            If Not (Str.ToString = "") Then
                Return "[ " & Str.ToString().Substring(0, Str.Length - 1) & " ]"
            Else
                Return ""
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            ' ErrMsg.Text = AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005")
            ErrMsg.Text = "<span class='error'>Error:</SPAN><span class='errormsg' style='font-size:.82em'>" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & ".</span>"
            Return ""
        End Try
    End Function

    Protected Sub TimeChk_MakeTimeVisible()
        ' System.Threading.Thread.Sleep(2000)
        ' Page.EnableViewState = True
        'BindGridForUpdatePanel()
        '  If (TimeSelection.Checked) Then MakeTimerDDVisible()
    End Sub

    Private Sub TimeSelection_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TimeSelection.CheckedChanged
        '  CustomerGrid.EnableViewState = True
        ' BindGridForUpdatePanel()
        '   MakeTimerDDVisible()
    End Sub
    'Private Sub FindChecker()
    '    Dim Chk As New CheckBox
    '    Dim SHHDD As New DropDownList
    '    Dim SMMDD As New DropDownList
    '    Dim EHHDD As New DropDownList
    '    Dim EMMDD As New DropDownList
    '    Dim CusID As New Label

    '    For Each row As GridViewRow In CustomerGrid.Rows
    '        Chk = DirectCast(row.FindControl("TimeChk"), CheckBox)
    '        CusID = DirectCast(row.FindControl("Customer_ID"), Label)
    '        SHHDD = DirectCast(row.FindControl("StartHH"), DropDownList)
    '        SMMDD = DirectCast(row.FindControl("StartMM"), DropDownList)
    '        EHHDD = DirectCast(row.FindControl("EndHH"), DropDownList)
    '        EMMDD = DirectCast(row.FindControl("EndMM"), DropDownList)

    '        If (Chk.Checked) Then
    '            Session("" & CusID.Text.Trim() & "") = Session("" & SHHDD.SelectedItem.Text & "-" & SMMDD.SelectedItem.Text & "-" & EHHDD.SelectedItem.Text & "-" & EMMDD.SelectedItem.Text & "")
    '        Else
    '            Session("" & CusID.Text.Trim() & "") = "None"
    '        End If
    '    Next
    'End Sub

    Sub bindVisitsPlanned()
        Dim objRoute As New RoutePlan
        Try
            Dim CustomerDt As New DataTable

            If Cache("VisitPlannedList") Is Nothing Then
                CustomerDt.Columns.Add("Customer_ID")
                CustomerDt.Columns.Add("Site_Use_ID")
                CustomerDt.Columns.Add("Customer_No")
                CustomerDt.Columns.Add("Customer_Name")
                CustomerDt.Columns.Add("Sequence", System.Type.GetType("System.Int32"))
                Cache("VisitPlannedList") = CustomerDt
            Else
                CustomerDt = CType(Cache("VisitPlannedList"), DataTable).Copy()
            End If



            Dim _dv As DataView
            _dv = CustomerDt.DefaultView()
            _dv.Sort = "Sequence Asc"

            CustomerGrid.DataSource = CustomerDt

            CustomerGrid.DataBind()
            CustomerDt = Nothing
            _dv = Nothing
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            ' ErrMsg.Text = AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005")
            ErrMsg.Text = "<span class='error'>Error:</SPAN><span class='errormsg' style='font-size:.82em'>" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & ".</span>"
        Finally
            objRoute = Nothing
        End Try
    End Sub

    Private Sub bindGrid()
        Dim objRoute As New RoutePlan
        Try
            If Cache("CustomerList") Is Nothing Then
                objRoute.SalesRepID = Session("SalesRep_ID")
                Cache("CustomerList") = objRoute.GetCustomerListForRoutePlan(Err_No, Err_Desc, selectSearchQuery, txt_Filter.Text)
            End If

            Dim CustomerDt As New DataTable
            CustomerDt = CType(Cache("CustomerList"), DataTable).Copy

            FilterCustomerGrid.DataSource = CustomerDt
            FilterCustomerGrid.DataBind()
            CustomerDt = Nothing
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            ' ErrMsg.Text = AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005")
            ErrMsg.Text = "<span class='error'>Error:</SPAN><span class='errormsg' style='font-size:.82em'>" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & ".</span>"
        Finally
            objRoute = Nothing
        End Try
    End Sub
    Private Function selectSearchQuery() As String
        'Try
        '    If FilterType.SelectedItem.Value = "CustomerNo" Then
        '        selectSearchQuery = "Customer_No='" & txt_Filter.Text & "' "
        '    ElseIf FilterType.SelectedItem.Value = "Customer Name" Then
        '        selectSearchQuery = "Customer_Name='" & txt_Filter.Text & "' "
        '    ElseIf FilterType.SelectedItem.Value = "Customer Name" Then
        '        selectSearchQuery = "Customer_Name='" & txt_Filter.Text & "' "
        '    ElseIf FilterType.SelectedItem.Value = "Customer Name" Then
        '        selectSearchQuery = "Customer_Name='" & txt_Filter.Text & "' "
        '    ElseIf FilterType.SelectedItem.Value = "Customer Name" Then
        '        selectSearchQuery = "Customer_Name='" & txt_Filter.Text & "' "
        '    ElseIf FilterType.SelectedItem.Value = "Customer Name" Then
        '        selectSearchQuery = "Customer_Name='" & txt_Filter.Text & "' "

        '    ElseIf (CusCityDD.SelectedItem.Text = "--City--" And CusAddDD.SelectedItem.Text <> "--Address--" And CusClassDD.SelectedItem.Text = "--Class--") Then
        '        selectSearchQuery = "Address='" & CusAddDD.SelectedItem.Text & "'"
        '    ElseIf (CusCityDD.SelectedItem.Text = "--City--" And CusAddDD.SelectedItem.Text = "--Address--" And CusClassDD.SelectedItem.Text <> "--Class--") Then
        '        selectSearchQuery = "Customer_Class='" & CusClassDD.SelectedItem.Text & "'"
        '    ElseIf (CusCityDD.SelectedItem.Text <> "--City--" And CusAddDD.SelectedItem.Text <> "--Address--" And CusClassDD.SelectedItem.Text = "--Class--") Then
        '        selectSearchQuery = "City='" & CusCityDD.SelectedItem.Text & "' and Address='" & CusAddDD.SelectedItem.Text & "'"
        '    ElseIf (CusCityDD.SelectedItem.Text <> "--City--" And CusAddDD.SelectedItem.Text = "--Address--" And CusClassDD.SelectedItem.Text <> "--Class--") Then
        '        selectSearchQuery = "City='" & CusCityDD.SelectedItem.Text & "' and Customer_Class='" & CusClassDD.SelectedItem.Text & "'"
        '    ElseIf (CusCityDD.SelectedItem.Text <> "--City--" And CusAddDD.SelectedItem.Text <> "--Address--" And CusClassDD.SelectedItem.Text <> "--Class--") Then
        '        selectSearchQuery = "City='" & CusCityDD.SelectedItem.Text & "' and Address='" & CusAddDD.SelectedItem.Text & "' and Customer_Class='" & CusClassDD.SelectedItem.Text & "'"
        '    ElseIf (CusCityDD.SelectedItem.Text = "--City--" And CusAddDD.SelectedItem.Text <> "--Address--" And CusClassDD.SelectedItem.Text <> "--Class--") Then
        '        selectSearchQuery = "Address='" & CusAddDD.SelectedItem.Text & "' and Customer_Class='" & CusClassDD.SelectedItem.Text & "'"
        '    End If
        'Catch ex As Exception
        '    log.Error(GetExceptionInfo(ex))
        '    ErrMsg.Text = "<span class='error'>Error:</SPAN><span class='errormsg' style='font-size:.82em'>" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & ".</span>"
        'End Try
        selectSearchQuery = ""
        Try
            If FilterType.SelectedItem.Value = "CustomerNo" Then
                selectSearchQuery = " and A.Customer_No like '%'+@SearchVal+'%' "
            ElseIf FilterType.SelectedItem.Value = "CustomerName" Then
                selectSearchQuery = " and A.Customer_Name like '%'+@SearchVal+'%' "
            ElseIf FilterType.SelectedItem.Value = "Address" Then
                selectSearchQuery = " and A.Address like '%'+@SearchVal+'%' "
            ElseIf FilterType.SelectedItem.Value = "City" Then
                selectSearchQuery = " and A.City like '%'+@SearchVal+'%'"
            ElseIf FilterType.SelectedItem.Value = "Class" Then
                selectSearchQuery = " and B.Customer_Class like '%'+@SearchVal+'%'"
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            ErrMsg.Text = "<span class='error'>Error:</SPAN><span class='errormsg' style='font-size:.82em'>" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & ".</span>"
        End Try
    End Function

    Protected Sub SetVisitsBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SetVisitsBtn.Click


        'UserComments.Text = ""  'COMMENT ON 24/04/2018
        'ComVal.Value = ""
        ' If (ValidateTime()) Then
        CallCloseEvent("V", "")
        'End If
    End Sub

    'Private Sub SelAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SelAll.Click
    '    'BindGridForUpdatePanel()
    '    Dim Chk As CheckBox
    '    For Each row As GridViewRow In CustomerGrid.Rows
    '        Chk = TryCast(row.FindControl("TimeChk"), CheckBox)
    '        Chk.Checked = True
    '    Next
    'End Sub

    'Private Sub DeSelAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DeSelAll.Click
    '   ' BindGridForUpdatePanel()
    '    Dim Chk As CheckBox
    '    For Each row As GridViewRow In CustomerGrid.Rows
    '        Chk = TryCast(row.FindControl("TimeChk"), CheckBox)
    '        Chk.Checked = False
    '    Next
    'End Sub
    'Private Sub MakeChecked()
    '    Dim ID As Label
    '    Dim Chk As CheckBox
    '    Dim SHHDD As New DropDownList
    '    Dim SMMDD As New DropDownList
    '    Dim EHHDD As New DropDownList
    '    Dim EMMDD As New DropDownList
    '    For Each row As GridViewRow In CustomerGrid.Rows
    '        Chk = TryCast(row.FindControl("TimeChk"), CheckBox)
    '        ID = TryCast(row.FindControl("Customer_ID"), Label)
    '        SHHDD = DirectCast(row.FindControl("StartHH"), DropDownList)
    '        SMMDD = DirectCast(row.FindControl("StartMM"), DropDownList)
    '        EHHDD = DirectCast(row.FindControl("EndHH"), DropDownList)
    '        EMMDD = DirectCast(row.FindControl("EndMM"), DropDownList)
    '        If (Session("" & ID.Text & "") <> "None") Then
    '            Chk.Checked = True
    '            Dim Arr As Array = Session("" & ID.Text & "").ToString.Split("-")
    '            SHHDD.SelectedIndex = SHHDD.Items.IndexOf(SHHDD.Items.FindByText("" & Arr(0) & ""))
    '            SMMDD.SelectedIndex = SMMDD.Items.IndexOf(SMMDD.Items.FindByText("" & Arr(1) & ""))
    '            EHHDD.SelectedIndex = EHHDD.Items.IndexOf(EHHDD.Items.FindByText("" & Arr(2) & ""))
    '            EMMDD.SelectedIndex = EMMDD.Items.IndexOf(EMMDD.Items.FindByText("" & Arr(3) & ""))
    '            SHHDD.Visible = True
    '            SMMDD.Visible = True
    '            EHHDD.Visible = True
    '            EMMDD.Visible = True
    '        End If
    '    Next
    'End Sub

    Private Function FromDefaultPlan() As Boolean
        If Not (Session("HDay" & Request.QueryString("DayRef") & "") Is Nothing) Then

            Dim HValu As String = Session("HDay" & Request.QueryString("DayRef") & "")
            Arr = HValu.Split("|")
            If (Arr.Length <= 2) Then
                If (Arr(0) = "O") Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return True
            End If
        End If
    End Function

    Private Sub MoveBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MoveBtn.Click
        Try
            Dim Date_List As String = Nothing
            For Each li As ListItem In MultiCheck.Items
                If li.Selected = True Then
                    Date_List += li.Value & ","
                End If
            Next
            If Not Date_List Is Nothing Then
                If Date_List.Length > 0 Then
                    Date_List = Date_List.Remove(Date_List.Length - 1, 1)
                End If
            Else
                ErrMsg.Text = "<span class='error'>Message:</SPAN><span class='errormsg' style='font-size:.82em'>Please select at least one day to move the visits.<span>"
                LoadCustomerGrid()
                TimeSelPanel.Update()
                Exit Sub
            End If
            If (FromDefaultPlan()) Then

                If Not (Session("HDay" & Request.QueryString("DayRef") & "") Is Nothing) And Not Date_List Is Nothing Then
                    Dim strScript As String
                    strScript = "<script language='javascript'>"
                    If (Request.QueryString("Mode") = "ADD") Then
                        ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_Action_Mode.value ='ADD';"
                        strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_Action_Mode','ADD');"
                    ElseIf (Request.QueryString("Mode") = "MODIFY") Then
                        strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_Action_Mode','MODIFY');"
                    End If
                    ''strScript += "GetRadWindow().BrowserWindow.MainContent_CheckRefresh.value ='Y';"
                    ''strScript += "GetRadWindow().BrowserWindow.MainContent_FromPopUp.value ='Y';"

                    strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_CheckRefresh','Y');"
                    strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_FromPopUp','Y');"

                    Dim DatArr As Array = Date_List.ToString().Split(",")
                    Dim Arr As Array = Session("HDay" & Request.QueryString("DayRef") & "").ToString.Split("|")
                    If (Arr(0) = "O") Then
                        ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_Cell" & Request.QueryString("DayRef") & ".value ='Y';"
                        ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_HDay" & Request.QueryString("DayRef") & ".value='X';"

                        strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_Cell" & Request.QueryString("DayRef") & "','Y');"
                        strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_HDay" & Request.QueryString("DayRef") & "','X');"

                        Dim HValu As String
                        If (UserComments.Text <> "") Then
                            HValu = "O|" & UserComments.Text
                        Else
                            HValu = "O|*"
                        End If
                        For i As Integer = 0 To DatArr.Length - 1
                            ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_HDay" & DatArr(i) & ".value='" & HValu & "';"
                            ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_Cell" & DatArr(i) & ".value ='Y';"

                            strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_HDay" & DatArr(i) & "','" & HValu & "');"
                            strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_Cell" & DatArr(i) & "','Y');"

                        Next
                    Else
                        Dim HValu As String = "V||" & ComString.Value & GetCusInformation()
                        ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_Cell" & Request.QueryString("DayRef") & ".value ='Y';"
                        ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_HDay" & Request.QueryString("DayRef") & ".value='X';"
                        strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_Cell" & Request.QueryString("DayRef") & "','Y');"
                        strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_HDay" & Request.QueryString("DayRef") & "','X');"
                        For i As Integer = 0 To DatArr.Length - 1
                            ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_HDay" & DatArr(i) & ".value='" & HValu & "';"
                            ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_Cell" & DatArr(i) & ".value ='Y';"
                            strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_HDay" & DatArr(i) & "','" & HValu & "');"
                            strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_Cell" & DatArr(i) & "','Y');"
                        Next
                    End If

                    ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_DayRef.value='" & Request.QueryString("DayRef") & "';"
                    strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_DayRef','" & Request.QueryString("DayRef") & "');"

                    ''strScript += "GetRadWindow().BrowserWindow.MainContent_Button1.click();"
                    strScript += "GetRadWindow().BrowserWindow.ButtonClick();"
                    strScript += "var oWindow = GetRadWindow();"
                    strScript += " oWindow.close(null);"
                    strScript += "</script>"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "StrScript", _
             strScript, False)
                    Cache.Remove("CustomerList")
                    SelectedList.Items.Clear()
                End If
            Else

                ErrMsg.Text = "<span class='error'>Error:</SPAN><span class='errormsg' style='font-size:.82em'>There is no visits to move/copy </span>"
            End If
            Cache.Remove("CustomerList")
            SelectedList.Items.Clear()
        Catch ex As Exception
            Err_No = "74030"
            log.Error(GetExceptionInfo(ex))
            '    ErrMsg.Text = AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005")
            ErrMsg.Text = "<span class='error'>Error:</SPAN><span class='errormsg' style='font-size:.82em'>" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & ".</span>"
        End Try
    End Sub

    Private Sub CopyBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CopyBtn.Click
        Try
            Dim Date_List As String = Nothing
            For Each li As ListItem In MultiCheck.Items
                If li.Selected = True Then
                    Date_List += li.Value & ","
                End If
            Next
            If Not Date_List Is Nothing Then
                If Date_List.Length > 0 Then
                    Date_List = Date_List.Remove(Date_List.Length - 1, 1)
                End If
            Else

                ErrMsg.Text = "<span class='error'>Message:</SPAN><span class='errormsg' style='font-size:.82em'>Please select at least one day to copy the visits.<span>"
                LoadCustomerGrid()
                TimeSelPanel.Update()
                Exit Sub
            End If




            If (FromDefaultPlan()) Then
                If Not (Session("HDay" & Request.QueryString("DayRef") & "") Is Nothing) And Not Date_List Is Nothing Then
                    Dim strScript As String
                    strScript = "<script language='javascript'>"
                    If (Request.QueryString("Mode") = "ADD") Then
                        ''strScript += "GetRadWindow().BrowserWindow.MainContent_Action_Mode.value ='ADD';"
                        strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_Action_Mode','ADD');"
                    ElseIf (Request.QueryString("Mode") = "MODIFY") Then
                        ''strScript += "GetRadWindow().BrowserWindow.MainContent_Action_Mode.value ='MODIFY';"
                        strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_Action_Mode','MODIFY');"
                    End If
                    ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_CheckRefresh.value ='Y';"
                    ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_FromPopUp.value ='Y';"

                    strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_CheckRefresh','Y');"
                    strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_FromPopUp','Y');"


                    Dim DatArr As Array = Date_List.Split(",")

                    Dim Arr As Array = Session("HDay" & Request.QueryString("DayRef") & "").ToString.Split("|")
                    If (Arr(0) = "O") Then
                        Dim HValu As String
                        If (UserComments.Text <> "") Then
                            HValu = "O|" & UserComments.Text
                        Else
                            HValu = "O|*"
                        End If

                        ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_Cell" & Request.QueryString("DayRef") & ".value ='Y';"
                        ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_HDay" & Request.QueryString("DayRef") & ".value='" & HValu & "';"

                        strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_Cell" & Request.QueryString("DayRef") & "','Y');"
                        strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_HDay" & Request.QueryString("DayRef") & "','" & HValu & "');"

                        For i As Integer = 0 To DatArr.Length - 1
                            ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_HDay" & DatArr(i) & ".value='" & HValu & "';"
                            ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_Cell" & DatArr(i) & ".value ='Y';"
                            strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_HDay" & DatArr(i) & "','" & HValu & "');"
                            strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_Cell" & DatArr(i) & "','Y');"
                        Next
                    Else
                        Dim HValu As String = "V||" & GetCusInformation()
                        ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_Cell" & Request.QueryString("DayRef") & ".value ='Y';"
                        ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_HDay" & Request.QueryString("DayRef") & ".value='" & HValu & "';"
                        strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_Cell" & Request.QueryString("DayRef") & "','Y');"
                        strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_HDay" & Request.QueryString("DayRef") & "','" & HValu & "');"
                        For i As Integer = 0 To DatArr.Length - 1
                            ''strScript += "GetRadWindow().BrowserWindow.MainContent_HDay" & DatArr(i) & ".value='" & HValu & "';"
                            ''strScript += "GetRadWindow().BrowserWindow.MainContent_Cell" & DatArr(i) & ".value ='Y';"
                            strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_HDay" & DatArr(i) & "','" & HValu & "');"
                            strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_Cell" & DatArr(i) & "','Y');"
                        Next
                    End If

                    'strScript += "GetRadWindow().BrowserWindow.MainContent_DayRef.value='" & Request.QueryString("DayRef") & "';"
                    strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_DayRef','" & Request.QueryString("DayRef") & "');"

                    ''strScript += "GetRadWindow().BrowserWindow.MainContent_Button1.click();"
                    strScript += "GetRadWindow().BrowserWindow.ButtonClick();"
                    strScript += "var oWindow = GetRadWindow();"
                    strScript += " oWindow.close(null);"
                    strScript += "</script>"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "StrScript", _
             strScript, False)
                    Cache.Remove("CustomerList")
                    SelectedList.Items.Clear()
                End If
            End If
            Cache.Remove("CustomerList")
            SelectedList.Items.Clear()
        Catch ex As Exception
            Err_No = "74029"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
            '   ErrMsg.Text = AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005")
            ErrMsg.Text = "<span class='error'>Error:</SPAN><span class='errormsg' style='font-size:.82em'>" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & ".</span>"
        End Try
    End Sub

    Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ' CustomerGrid.EnableViewState = true
    End Sub

    Private Sub ResetAllBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ResetAllBtn.Click
        Try

            Dim strScript As String
            Dim Time As String = ""
            'Dim Ucomments As String = ""
            strScript = "<script language='javascript'>"
            If (Request.QueryString("Mode") = "ADD") Then
                ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_Action_Mode.value ='ADD';"
                strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_Action_Mode','ADD');"
            ElseIf (Request.QueryString("Mode") = "MODIFY") Then
                ''strScript += "GetRadWindow().BrowserWindow.MainContent_Action_Mode.value ='MODIFY';"
                strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_Action_Mode','MODIFY');"
            End If
            ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_CheckRefresh.value ='Y';"
            ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_FromPopUp.value ='Y';"
            strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_CheckRefresh','Y');"
            strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_FromPopUp','Y');"

            Dim CommStr As String

            CommStr = Ucomments
            ComString.Value = Ucomments

            Dim objRoute As New RoutePlan

            If Cache("DDDays") Is Nothing Then
                Cache("DDDays") = objRoute.GetMoveCopyDays(Err_No, Err_Desc)
            End If
            Dim TempTable As New DataTable
            TempTable = CType(Cache("DDDays"), DataTable)

            ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_Cell" & Request.QueryString("DayRef") & ".value ='Y';"
            ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_HDay" & Request.QueryString("DayRef") & ".value='" & "X" & CommStr & "';"

            strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_Cell" & Request.QueryString("DayRef") & "','Y');"
            strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_HDay" & Request.QueryString("DayRef") & "','" & "X" & CommStr & "');"

            For Each Dr As DataRow In TempTable.Rows
                Dim DayValue As Integer = CInt(Dr.Item(0))
                ''strScript += "GetRadWindow().BrowserWindow.MainContent_Cell" & DayValue & ".value ='Y';"
                ''strScript += "GetRadWindow().BrowserWindow.MainContent_HDay" & DayValue & ".value='" & "X" & CommStr & "';"
                strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_Cell" & DayValue & "','Y');"
                strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_HDay" & DayValue & "','" & "X" & CommStr & "');"
                Session("HDay" & DayValue & "") = "X" & CommStr
            Next

            objRoute = Nothing
            TempTable = Nothing
            Cache.Remove("DDDays")

            ' ''strScript += "GetRadWindow().BrowserWindow.MainContent_DayRef.value='" & Request.QueryString("DayRef") & "';"
            strScript += "GetRadWindow().BrowserWindow.SetValue('MainContent_DayRef','" & Request.QueryString("DayRef") & "');"
            strScript += "var par = GetRadWindow().BrowserWindow.location.search;"
            strScript += "var buttonid = par.substring(par.indexOf('=') + 1, par.indexOf('&'));"

            ''strScript += "GetRadWindow().BrowserWindow.MainContent_Button1.click();"
            strScript += "GetRadWindow().BrowserWindow.ButtonClick();"
            strScript += "var oWindow = GetRadWindow();"
            strScript += " oWindow.close(null);"
            strScript += "</script>"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "StrScript", _
     strScript, False)
        Catch ex As Exception
            Err_No = "74028"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
            '    ErrMsg.Text = AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005")
            ErrMsg.Text = "<span class='error'>Error:</SPAN><span class='errormsg' style='font-size:.82em'>" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & ".</span>"
        End Try
    End Sub

    Protected Sub Btn_Filter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Btn_Filter.Click
        'MaintainState()
        Dim objRoute As New RoutePlan
        Try


            '  objRoute.SalesRepIDprop = Session("USER_ACCESS").SalesRepID()
            objRoute.SalesRepID = Session("SalesRep_ID")
            Cache("CustomerList") = objRoute.GetCustomerListForRoutePlan(Err_No, Err_Desc, selectSearchQuery, txt_Filter.Text)


            Dim CustomerDt As New DataTable
            CustomerDt = CType(Cache("CustomerList"), DataTable).Copy

            FilterCustomerGrid.DataSource = CustomerDt
            FilterCustomerGrid.DataBind()
            ' BindmapData()
            Cache.Remove("GeoMarkers")
            Dim geoval = Mapmarkers()
            Bindmap(geoval)


            CustomerDt = Nothing
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            '   ErrMsg.Text = AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005")
            ErrMsg.Text = "<span class='error'>Error:</SPAN><span class='errormsg' style='font-size:.82em'>" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & ".</span>"
        Finally
            objRoute = Nothing
        End Try
    End Sub

    Private Sub FilterCustomerGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles FilterCustomerGrid.PageIndexChanging
        FilterCustomerGrid.PageIndex = e.NewPageIndex
        bindGrid()
    End Sub


   


    Protected Sub Btn_Clear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Btn_Clear.Click
        Dim objRoute As New RoutePlan
        Try


            '  objRoute.SalesRepIDprop = Session("USER_ACCESS").SalesRepID()
            objRoute.SalesRepID = Session("SalesRep_ID")
            txt_Filter.Text = ""
            FilterType.ClearSelection()
            Cache("CustomerList") = objRoute.GetCustomerListForRoutePlan(Err_No, Err_Desc, selectSearchQuery, txt_Filter.Text)


            Dim CustomerDt As New DataTable
            CustomerDt = CType(Cache("CustomerList"), DataTable).Copy

            FilterCustomerGrid.DataSource = CustomerDt
            FilterCustomerGrid.DataBind()
            Dim geoval = Mapmarkers()
            Bindmap(geoval)
            CustomerDt = Nothing
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            '   ErrMsg.Text = AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005")
            ErrMsg.Text = "<span class='error'>Error:</SPAN><span class='errormsg' style='font-size:.82em'>" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & ".</span>"
        Finally
            objRoute = Nothing
        End Try
    End Sub
    Private Sub BindEmptymap()
        Try
            oGeocodeList = New List(Of [String])()
            Dim temp_mapinfo
            Dim temp_geocode As String = ""
            Dim temp_geocode1 As String = ""
            Dim temp_geocode2 As String = ""


            oGeocodeList = New List(Of [String])()
            temp_geocode = " '" & _strDefLat & "," & _strDefLong & ",""'"
            temp_geocode1 = " '25.2920,55.3655,""'"
            temp_geocode2 = " '25.3039,55.3717,""'"

            oGeocodeList.Add(temp_geocode)
            oGeocodeList.Add(temp_geocode1)
            oGeocodeList.Add(temp_geocode2)
            Dim oMessageList As New List(Of String)()
            temp_mapinfo = " '<span class=formatText></span>' "
            oMessageList.Add(temp_mapinfo)
            Dim geocodevalues = String.Join(",", oGeocodeList.ToArray())
            Dim message As [String] = String.Join(",", oMessageList.ToArray())
            'Dim geocodevalues = String.Join(",", oGeocodeList.ToArray())

            rap.ResponseScripts.Add("locationList = undefined; message = undefined;")
            ScriptManager.RegisterArrayDeclaration(Me, "locationList", geocodevalues)
            ScriptManager.RegisterArrayDeclaration(Me, "message", message)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Intialization1", "clearOverlays1();", True)


            oGeocodeList = Nothing
        Catch ex As Exception
            log.Error(ex.Message)
        End Try

    End Sub
    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        Try
            Dim _CustID As String
            Dim _CustSiteID As String
            Dim _MapMode As String
            _CustID = CustID.Value
            _CustSiteID = CustSiteID.Value

            _MapMode = MapMode.Value




            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            ' Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)

            If MapMode.Value.ToString().ToUpper().Contains("ADD") Then


                If Cache("VisitPlannedList") Is Nothing Then
                    Dim CustomerDt As New DataTable
                    CustomerDt.Columns.Add("Customer_ID")
                    CustomerDt.Columns.Add("Site_Use_ID")
                    CustomerDt.Columns.Add("Customer_No")
                    CustomerDt.Columns.Add("Customer_Name")
                    CustomerDt.Columns.Add("Sequence")
                    Cache("VisitPlannedList") = CustomerDt
                    CustomerDt = Nothing
                End If

                Dim PlannedVisits As New DataTable
                PlannedVisits = CType(Cache("VisitPlannedList"), DataTable).Copy
                If PlannedVisits.Select("Customer_ID=" & CustID.Value & " and Site_Use_ID=" & CustSiteID.Value).Length <= 0 Then
                    Dim dr As DataRow
                    dr = PlannedVisits.NewRow

                    Dim _CustomerDt As New DataTable
                    _CustomerDt = CType(Cache("CustomerList"), DataTable).Copy
                    Dim _dr() As System.Data.DataRow
                    _dr = _CustomerDt.Select("Customer_ID='" & CustID.Value & "' And Site_Use_ID='" & CustSiteID.Value & "' ")

                    If _dr.Length > 0 Then

                        dr("Customer_ID") = _dr(0)("Customer_ID").ToString() 'CType(row.Cells(0).FindControl("Customer_ID"), HiddenField).Value
                        dr("Site_Use_ID") = _dr(0)("Site_Use_ID").ToString() 'CType(row.Cells(0).FindControl("SiteID"), HiddenField).Value
                        dr("Customer_No") = _dr(0)("Customer_No").ToString() ' CType(row.Cells(0).FindControl("Customer_No"), HiddenField).Value
                        dr("Customer_Name") = _dr(0)("Customer_Name").ToString() 'CType(row.Cells(1).FindControl("Customer_Name"), Label).Text
                        dr("Sequence") = PlannedVisits.Rows.Count + 1
                        PlannedVisits.Rows.Add(dr)
                    End If

                End If
                Cache("VisitPlannedList") = PlannedVisits
                PlannedVisits = Nothing
                bindVisitsPlanned()

            ElseIf MapMode.Value.ToString().ToUpper().Contains("DELETE") Then


                Dim PlannedVisits As New DataTable
                PlannedVisits = CType(Cache("VisitPlannedList"), DataTable).Copy
                Dim seldr() As DataRow
                seldr = PlannedVisits.Select("Customer_ID='" & CustID.Value & "' and Site_Use_ID='" & CustSiteID.Value & "'")
                If seldr.Length > 0 Then
                    For Each dr As DataRow In seldr
                        PlannedVisits.Rows.Remove(dr)
                    Next
                End If
                Dim index As Integer
                index = 0
                For Each dr As DataRow In PlannedVisits.Rows
                    dr("Sequence") = index + 1
                    index = index + 1
                Next

                Dim _dv As DataView
                _dv = PlannedVisits.DefaultView()
                _dv.Sort = "Sequence Asc"
                PlannedVisits = _dv.ToTable

                Cache("VisitPlannedList") = PlannedVisits
                PlannedVisits = Nothing
                bindVisitsPlanned()
            End If
        Catch ex As Exception

        End Try

        Bindmap(Cache("GeoMarkers"))
    End Sub

    Private Sub BindmapData()
        Try

            Dim PlannedVisits_map As New DataTable
            PlannedVisits_map = CType(Cache("VisitPlannedList"), DataTable).Copy


            oGeocodeList = New List(Of [String])()
            Dim temp_mapinfo
            Dim temp_geocode As String = ""



            oGeocodeList = New List(Of [String])()
            temp_geocode = " '" & _strDefLat & "," & _strDefLong & ",""'"
            ' temp_geocode1 = " '25.2920,55.3655,""'"
            'temp_geocode2 = " '25.3039,55.3717,""'"

            oGeocodeList.Add(temp_geocode)
            'oGeocodeList.Add(temp_geocode1)
            'oGeocodeList.Add(temp_geocode2)


            Dim CustomerDt_map As New DataTable
            CustomerDt_map = CType(Cache("CustomerList"), DataTable).Copy

            For Each dr As DataRow In CustomerDt_map.Rows

                If Not IsDBNull(dr("Lat")) And Not IsDBNull(dr("Long")) Then
                    If Not String.IsNullOrEmpty(dr("Lat")) And Not String.IsNullOrEmpty(dr("Long")) Then
                        If IsNumeric(dr("Lat")) And IsNumeric(dr("Long")) Then
                            If CDec(dr("Lat")) > 0 And CDec(dr("Long")) > 0 Then

                                Dim Visited As String
                                Visited = "0"

                                If PlannedVisits_map.Rows.Count > 0 Then
                                    Dim _dr() As System.Data.DataRow
                                    _dr = PlannedVisits_map.Select("Customer_ID='" & dr("Customer_ID") & "' And Site_Use_ID='" & dr("Site_Use_ID") & "' ")

                                    If _dr.Length > 0 Then

                                        Visited = "1"
                                    End If
                                End If




                                Dim geocode As String = ""
                                geocode = " '" & dr("Lat") & "," & dr("Long") & "," & dr("Customer_Name") & "~" & dr("Customer_ID") & "~" & dr("Site_Use_ID") & "~" & Visited & " '"
                                oGeocodeList.Add(geocode)
                            End If
                        End If
                    End If
                End If
            Next







            Dim oMessageList As New List(Of String)()
            temp_mapinfo = " '<span class=formatText></span>' "
            oMessageList.Add(temp_mapinfo)
            Dim geocodevalues = String.Join(",", oGeocodeList.ToArray())
            Dim message As [String] = String.Join(",", oMessageList.ToArray())

            rap.ResponseScripts.Add("locationList = undefined; message = undefined;")
            ScriptManager.RegisterArrayDeclaration(Me, "locationList", geocodevalues)
            ScriptManager.RegisterArrayDeclaration(Me, "message", message)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Intialization1", "clearOverlays1();", True)


            oGeocodeList = Nothing
        Catch ex As Exception
            log.Error(ex.Message)
        End Try

    End Sub

    Private Sub Bindmap(geocodevalues As String)
        Try

            Dim temp_mapinfo
            Dim oMessageList As New List(Of String)()
            temp_mapinfo = " '<span class=formatText></span>' "
            oMessageList.Add(temp_mapinfo)

            Dim message As [String] = String.Join(",", oMessageList.ToArray())

            rap.ResponseScripts.Add("locationList = undefined; message = undefined;")
            ScriptManager.RegisterArrayDeclaration(Me, "locationList", geocodevalues)
            ScriptManager.RegisterArrayDeclaration(Me, "message", message)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Intialization1", "clearOverlays1();", True)


            oGeocodeList = Nothing
        Catch ex As Exception
            log.Error(ex.Message)
        End Try

    End Sub

    Private Function Mapmarkers() As String
        Dim geocodevalues As String = ""
        Try


            Dim PlannedVisits_map As New DataTable
            PlannedVisits_map = CType(Cache("VisitPlannedList"), DataTable).Copy


            oGeocodeList = New List(Of [String])()

            Dim temp_geocode As String = ""



            oGeocodeList = New List(Of [String])()
            temp_geocode = " '" & _strDefLat & "," & _strDefLong & ",""'"


            oGeocodeList.Add(temp_geocode)



            Dim CustomerDt_map As New DataTable
            CustomerDt_map = CType(Cache("CustomerList"), DataTable).Copy

            For Each dr As DataRow In CustomerDt_map.Rows

                If Not IsDBNull(dr("Lat")) And Not IsDBNull(dr("Long")) Then
                    If Not String.IsNullOrEmpty(dr("Lat")) And Not String.IsNullOrEmpty(dr("Long")) Then
                        If IsNumeric(dr("Lat")) And IsNumeric(dr("Long")) Then
                            If CDec(dr("Lat")) > 0 And CDec(dr("Long")) > 0 Then

                                Dim Visited As String
                                Visited = "0"

                                If PlannedVisits_map.Rows.Count > 0 Then
                                    Dim _dr() As System.Data.DataRow
                                    _dr = PlannedVisits_map.Select("Customer_ID='" & dr("Customer_ID") & "' And Site_Use_ID='" & dr("Site_Use_ID") & "' ")

                                    If _dr.Length > 0 Then
                                        log.Error(dr("Customer_ID") & " , " & dr("Site_Use_ID"))
                                        Visited = "1"
                                    End If
                                End If



                                Dim Customer_Name As String = dr("Customer_Name").Replace(",", " ")

                                Dim geocode As String = ""
                                ' geocode = " '" & dr("Lat") & "," & dr("Long") & "," & dr("Customer_Name") & "~" & dr("Customer_ID") & "~" & dr("Site_Use_ID") & "~" & Visited & " '"
                                geocode = " '" & dr("Lat") & "," & dr("Long") & "," & Customer_Name & "~" & dr("Customer_ID") & "~" & dr("Site_Use_ID") & "~" & Visited & " '"
                                oGeocodeList.Add(geocode)
                            End If
                        End If
                    End If
                End If
            Next

            geocodevalues = String.Join(",", oGeocodeList.ToArray())

            Cache("GeoMarkers") = geocodevalues
            log.Error("Location list " & geocodevalues)
            Return geocodevalues
        Catch ex As Exception

            log.Error(GetExceptionInfo(ex))
            ErrMsg.Text = "<span class='error'>Error:</SPAN><span class='errormsg' style='font-size:.82em'>" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & ".</span>"


            Return geocodevalues
        End Try

    End Function

  


    Private Sub FilterCustomerGrid_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles FilterCustomerGrid.RowDataBound
        Try


            Try
                If e.Row.RowType = DataControlRowType.DataRow Then
                    Dim CusID As String = TryCast(e.Row.FindControl("Customer_ID"), HiddenField).Value
                    Dim SiteID As String = TryCast(e.Row.FindControl("SiteID"), HiddenField).Value

                    Dim Visits As Label = TryCast(e.Row.FindControl("NoOfVisits_F"), Label)
                    Visits.Text = GetNoVisits(CusID, SiteID)
                    Dim Sequence As HiddenField = TryCast(e.Row.FindControl("Sequence"), HiddenField)

                End If
            Catch ex As Exception

            End Try
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            '  ErrMsg.Text = AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001")
            ErrMsg.Text = "<span class='error'>Error:</SPAN><span class='errormsg' style='font-size:.82em'>" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & ".</span>"
        End Try
    End Sub


End Class