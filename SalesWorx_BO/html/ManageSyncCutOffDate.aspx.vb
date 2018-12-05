Imports SalesWorx.BO.Common
Imports System.Data
Imports System.Web
Imports System.Resources
Imports System.Web.UI.WebControls
Imports log4net
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Data.OleDb
Imports Telerik.Web.UI
Public Class ManageSyncCutOffDate
    Inherits System.Web.UI.Page
    Dim objSync As New SyncCutOffDate
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim Err_No As Long
    Dim Err_Desc As String

    Private Const PageID As String = "P336"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        If Not Page.IsPostBack Then
            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            If Not HasPermission Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If
            LoadCountry()
            LoadFilterCountry()
            LoadYear()
            LoadFilterYear()
            ddlMonth.Items.Insert(0, New RadComboBoxItem("-- Select --", 0))
            ddlFilterMonth.Items.Insert(0, New RadComboBoxItem("-- Select --", 0))

            BindData()
            Resetfields()
        Else
            MPEDetails.VisibleOnPageLoad = False

        End If

    End Sub
    Sub LoadCountry()
        ddlCountry.Items.Clear()
        ddlCountry.DataSource = objSync.GetCountries(Err_No, Err_Desc)
        ddlCountry.DataTextField = "Code_Description"
        ddlCountry.DataValueField = "Code_Value"
        ddlCountry.DataBind()
        ddlCountry.Items.Insert(0, New RadComboBoxItem("-- Select --", 0))
    End Sub
    Sub LoadFilterCountry()
        ddlFilterCountry.Items.Clear()
        ddlFilterCountry.DataSource = objSync.GetCountries(Err_No, Err_Desc)
        ddlFilterCountry.DataTextField = "Code_Description"
        ddlFilterCountry.DataValueField = "Code_Value"
        ddlFilterCountry.DataBind()
        ddlFilterCountry.Items.Insert(0, New RadComboBoxItem("-- Select --", 0))
    End Sub

    Sub LoadYear()
        ddlYear.Items.Clear()
        If Now.Month <> 12 Then
            For i As Integer = Now.Year To Now.Year + 1
                ddlYear.Items.Add(New RadComboBoxItem(i.ToString, i.ToString))
            Next
        Else
            For i As Integer = Now.Year + 1 To Now.Year + 1
                ddlYear.Items.Add(New RadComboBoxItem(i.ToString, i.ToString))
            Next
        End If
        ddlYear.Items.Insert(0, New RadComboBoxItem("-- Select --", 0))

    End Sub
    Sub LoadMonth()
        ddlMonth.Items.Clear()
        If ddlYear.SelectedItem.Value = Now.Year Then
            For i As Integer = Now.Month To 12
                ddlMonth.Items.Add(New RadComboBoxItem(CDate(i & "/01/1900").ToString("MMM"), i.ToString))
            Next
        Else
            For i As Integer = 1 To 12
                ddlMonth.Items.Add(New RadComboBoxItem(CDate(i & "/01/1900").ToString("MMM"), i.ToString))
            Next
        End If
        ddlMonth.Items.Insert(0, New RadComboBoxItem("-- Select --", 0))

    End Sub
    Sub LoadFilterYear()
        ddlFilterYear.Items.Clear()
        Dim dt As New DataTable
        dt = objSync.GetYears(Err_No, Err_Desc)
        For Each dr As DataRow In dt.Rows
            ddlFilterYear.Items.Add(New RadComboBoxItem(dr(0).ToString, dr(0).ToString))
        Next
        If dt.Rows.Count <= 0 Then
            For i As Integer = Now.Year To Now.Year
                ddlFilterYear.Items.Add(New RadComboBoxItem(i.ToString, i.ToString))
            Next
        End If

        ddlFilterYear.Items.Insert(0, New RadComboBoxItem("-- Select --", 0))

    End Sub
    Sub LoadFilterMonth()
        ddlFilterMonth.Items.Clear()
        If ddlFilterYear.SelectedItem.Value > 0 Then
            For i As Integer = 1 To 12
                ddlFilterMonth.Items.Add(New RadComboBoxItem(CDate(i & "/01/1900").ToString("MMM"), i.ToString))
            Next
        End If

        ddlFilterMonth.Items.Insert(0, New RadComboBoxItem("-- Select --", 0))

    End Sub
    Private Sub ddlFilterYear_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlFilterYear.SelectedIndexChanged
        LoadFilterMonth()
        BindData()
        ClassUpdatePnl.Update()
    End Sub
    Private Sub ddlFilterMonth_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlFilterMonth.SelectedIndexChanged
        BindData()
        ClassUpdatePnl.Update()
    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Try
            Resetfields()
            Me.MPEDetails.VisibleOnPageLoad = False
            'ClassUpdatePnl.Update()

        Catch

        End Try
    End Sub

    Public Sub Resetfields()
        ddlYear.ClearSelection()
        ddlMonth.ClearSelection()
        DatePickerCutoffDate.DateInput.Text = ""
        DatePickerCutoffDate.Clear()
    End Sub

    'Protected Sub CheckRequiredFields()
    '    If Me.txtHolidayCode.Text = "" Or Me.txtDescription.Text = "" Or Me.txtRate.Text = "" Then
    '        Me.lblinfo.Text = "Validation"
    '        Me.lblMessage.Text = "Holiday code,description and conversion rate are required."
    '        Me.lblMessage.ForeColor = Drawing.Color.Red
    '        Me.MpInfoError.Show()
    '        Me.MPEHoliday.Show()
    '        Exit Sub
    '    End If

    '    If IsAlpha(Me.txtHolidayCode.Text) = False Then
    '        Me.lblinfo.Text = "Validation"
    '        Me.lblMessage.Text = "Holiday code should be in characters."
    '        Me.lblMessage.ForeColor = Drawing.Color.Red
    '        Me.MpInfoError.Show()
    '        Me.MPEHoliday.Show()
    '        Exit Sub
    '    End If


    '    If IsNumeric(Me.txtRate.Text) = False Then
    '        Me.lblinfo.Text = "Validation"
    '        Me.lblMessage.Text = "Conversion rate should be in number."
    '        Me.lblMessage.ForeColor = Drawing.Color.Red
    '        Me.MpInfoError.Show()
    '        Me.MPEHoliday.Show()
    '        Exit Sub
    '    End If
    'End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Me.ddlCountry.SelectedIndex <= 0 Then
            Me.lblPop.Text = "Please select the country."
            Me.MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If
        If Me.ddlYear.SelectedIndex <= 0 Then
            Me.lblPop.Text = "Please select the year."
            Me.MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If
        If Me.ddlMonth.SelectedIndex <= 0 Then
            Me.lblPop.Text = "Please select the month."
            Me.MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If

        If Me.DatePickerCutoffDate.SelectedDate = Nothing Or Me.DatePickerCutoffDate.DateInput.Text = "" Then
            Me.lblPop.Text = "Please enter sync cut off date and time."
            Me.MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If
        If Not (Me.DatePickerCutoffDate.SelectedDate >= CDate(ddlYear.SelectedItem.Value & "-" & ddlMonth.SelectedItem.Value & "-01") And Me.DatePickerCutoffDate.SelectedDate < DateAdd(DateInterval.Month, 1, CDate(ddlYear.SelectedItem.Value & "-" & ddlMonth.SelectedItem.Value & "-01"))) Then
            Me.lblPop.Text = "Sync cut off date and time should be in the selected month"
            Me.MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If
        If Me.DatePickerCutoffDate.SelectedDate < DateAdd(DateInterval.Minute, 10, Now) Then
            Me.lblPop.Text = "Please select a future date "
            Me.MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If
        Dim success As Boolean = False
        Try

            objSync.CutOffDate = DatePickerCutoffDate.SelectedDate.Value.ToString("dd-MMM-yyyy hh:mm:ss tt")
            objSync.Year = ddlYear.SelectedItem.Value
            objSync.Month = ddlMonth.SelectedItem.Value
            objSync.Country = ddlCountry.SelectedItem.Value
            If objSync.SaveSyncCutOffDate(Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
                success = True
                MessageBoxValidation("Successfully saved", "Information")
                BindData()
                Resetfields()
                Me.MPEDetails.VisibleOnPageLoad = False
                ClassUpdatePnl.Update()
            Else
                MessageBoxValidation("Could not be saved.", "Information")
                Me.MPEDetails.VisibleOnPageLoad = False
                ClassUpdatePnl.Update()
            End If

            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN", "SYNC CUTOFF", ddlYear.SelectedItem.Value, "Year: " & ddlYear.SelectedItem.Value & "Month:" & ddlMonth.SelectedItem.Value & "Cut off date:" & DatePickerCutoffDate.SelectedDate.Value.ToString("dd-MMM-yyyy"), CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")

            End If

        Catch ex As Exception
            Err_No = "74061"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Private Sub BindData()
        Dim Dt As New DataTable
        Dt = objSync.GetSyncCutOffDates(Err_No, Err_Desc, ddlFilterYear.SelectedItem.Value, ddlFilterMonth.SelectedItem.Value, ddlFilterCountry.SelectedItem.Value)
        Me.gvCutoff.DataSource = Dt
        Me.gvCutoff.DataBind()

        Dim dv As New DataView(Dt)
        If ViewState("SortField") <> "" Then
            dv.Sort = (ViewState("SortField") & " ") + SortDirection
        End If
        gvCutoff.DataSource = dv
        gvCutoff.DataBind()

    End Sub

    Public Function IsNumeric(ByVal inputString As String) As Boolean
        Dim _isNumber As System.Text.RegularExpressions.Regex = New  _
Regex("(^[-+]?\d+(,?\d*)*\.?\d*([Ee][-+]\d*)?$)|(^[-+]?\d?(,?\d*)*\.\d+([Ee][-+]\d*)?$)")
        Return _isNumber.Match(inputString).Success
    End Function

    Public Function IsAlpha(ByVal strToCheck As String) As Boolean
        Dim objAlphaPattern As Regex = New Regex("[^a-zA-Z]")
        Return Not objAlphaPattern.IsMatch(strToCheck)
    End Function


    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If Me.ddlCountry.SelectedIndex <= 0 Then
            Me.lblPop.Text = "Please select the country."
            Me.MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If
        If Me.ddlYear.SelectedIndex < 0 Then
            Me.lblPop.Text = "Please select the year."
            Me.MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If
        If Me.ddlMonth.SelectedIndex < 0 Then
            Me.lblPop.Text = "Please select the month."
            Me.MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If

        If Me.DatePickerCutoffDate.SelectedDate = Nothing Or Me.DatePickerCutoffDate.DateInput.Text = "" Then
            Me.lblPop.Text = "Please enter sync cut off date and time."
            Me.MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If
        If Not (Me.DatePickerCutoffDate.SelectedDate >= CDate(ddlYear.SelectedItem.Value & "-" & ddlMonth.SelectedItem.Value & "-01") And Me.DatePickerCutoffDate.SelectedDate < DateAdd(DateInterval.Month, 1, CDate(ddlYear.SelectedItem.Value & "-" & ddlMonth.SelectedItem.Value & "-01"))) Then
            Me.lblPop.Text = "Sync cut off date and time should be in the selected month"
            Me.MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If
        If Me.DatePickerCutoffDate.SelectedDate < DateAdd(DateInterval.Minute, 10, Now) Then
            Me.lblPop.Text = "Please select a future date "
            Me.MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If
        Dim success As Boolean = False
        Try

            objSync.CutOffDate = DatePickerCutoffDate.SelectedDate.Value.ToString("dd-MMM-yyyy hh:mm:ss tt")
            objSync.Year = ddlYear.SelectedItem.Value
            objSync.Month = ddlMonth.SelectedItem.Value
            objSync.Country = ddlCountry.SelectedItem.Value
            If objSync.SaveSyncCutOffDate(Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
                success = True
                MessageBoxValidation("Successfully updated.", "Information")
                Dim fromdate As String = ""
                BindData()
                Resetfields()
                Me.MPEDetails.VisibleOnPageLoad = False
                ClassUpdatePnl.Update()
            Else
                MessageBoxValidation("Could not be saved.", "Information")
                Me.MPEDetails.VisibleOnPageLoad = False
                ClassUpdatePnl.Update()
            End If

            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN", "SYNC CUTOFF", ddlYear.SelectedItem.Value, "Year: " & ddlYear.SelectedItem.Value & "Month:" & ddlMonth.SelectedItem.Value & "Cut off date" & DatePickerCutoffDate.SelectedDate.Value.ToString("dd-MMM-yyyy"), CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")

            End If

        Catch ex As Exception
            Err_No = "74062"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub




    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btndelete As ImageButton = TryCast(sender, ImageButton)
        Dim row As GridViewRow = DirectCast(btndelete.NamingContainer, GridViewRow)

        Dim success As Boolean = False

        Try
            Dim Lbl As System.Web.UI.WebControls.Label = row.FindControl("lblHoliday")
            If objSync.DeleteSyncCutOffDate(Err_No, Err_Desc, row.Cells(1).Text, row.Cells(2).Text, row.Cells(3).Text) = True Then
                success = True
            End If

            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "D", "MAIN", "SYNC CUTOFF", row.Cells(1).Text, "Country: " & row.Cells(1).Text & "Year: " & row.Cells(2).Text & "Month: " & row.Cells(3).Text & "Cut off date:" & row.Cells(3).Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                MessageBoxValidation("Successfully deleted.", "Information")
                BindData()
                Resetfields()
            Else
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_HolidayCode_005") & "&next=ManageSyncCutOffDate.aspx&Title=Message", False)
                Exit Try
            End If

        Catch ex As Exception
            Err_No = "74065"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub


    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        btnUpdate.Visible = False
        btnSave.Visible = True
        Resetfields()
        ddlYear.Enabled = True
        ddlCountry.Enabled = True
        ddlMonth.Enabled = True
        Me.lblPop.Text = ""
        Me.MPEDetails.VisibleOnPageLoad = True
        ClassUpdatePnl.Update()
    End Sub


    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            lblPop.Text = ""
            btnUpdate.Visible = True
            btnSave.Visible = False
            Resetfields()

            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
            DatePickerCutoffDate.SelectedDate = DateTime.Parse(row.Cells(4).Text)
            ddlYear.ClearSelection()
            If Not ddlCountry.Items.FindItemByValue(row.Cells(1).Text) Is Nothing Then
                ddlCountry.Items.FindItemByValue(row.Cells(1).Text).Selected = True
            End If
            If Not ddlYear.Items.FindItemByValue(row.Cells(2).Text) Is Nothing Then
                ddlYear.Items.FindItemByValue(row.Cells(2).Text).Selected = True
            End If
            ddlCountry.Enabled = False
            ddlYear.Enabled = False
            ddlMonth.Enabled = False
            LoadMonth()
            ddlMonth.ClearSelection()
            If Not ddlMonth.Items.FindItemByValue(row.Cells(3).Text) Is Nothing Then
                ddlMonth.Items.FindItemByValue(row.Cells(3).Text).Selected = True
            End If

            MPEDetails.VisibleOnPageLoad = True
            'ClassUpdatePnl.Update()
        Catch ex As Exception
            Err_No = "74066"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_HolidayCode_006") & "&next=ManageSyncCutOffDate.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub

    Private Sub gv_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvCutoff.PageIndexChanging
        gvCutoff.PageIndex = e.NewPageIndex
        BindData()

    End Sub


    Private Sub gvHoliday_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvCutoff.Sorting
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindData()
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



    Private Sub ddlYear_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlYear.SelectedIndexChanged
        LoadMonth()
    End Sub

    Private Sub ddlFilterCountry_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlFilterCountry.SelectedIndexChanged
        BindData()
        ClassUpdatePnl.Update()
    End Sub
End Class