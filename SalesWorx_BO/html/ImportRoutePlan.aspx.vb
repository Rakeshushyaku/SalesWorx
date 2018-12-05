Imports System.Data.OleDb
Imports System.IO
Imports log4net
Imports System.Data.SqlClient
Imports SalesWorx.BO.Common
Imports Telerik.Web.UI

Partial Public Class ImportRoutePlan
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim objRoutePlan As New RoutePlan
    Private bReimport As Boolean = False
    Private Const PageID As String = "P82"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub ImportRoutePlan_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Import Route Plan"
    End Sub

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
            divResult.Visible = False
            ViewState("FileType") = Nothing
            ViewState("FileName") = Nothing
            ViewState("CSVName") = Nothing
            Session("Tbl") = Nothing
            Session("DoneValidation") = False
            Me.BtnReimport.Enabled = False
            LoadDefPlan()
        Else
            Me.BtnReimport.Enabled = True
        End If
    End Sub
    Sub LoadDefPlan()

        Default_Plan_DD.DataSource = objRoutePlan.GetAllDefaultPlans(Err_No, Err_Desc)
        Default_Plan_DD.DataValueField = "Default_Plan_ID"
        Default_Plan_DD.DataTextField = "DefPlan"
        Default_Plan_DD.DataBind()
        Default_Plan_DD.Items.Insert(0, New RadComboBoxItem("Select Default Plan", "0"))

    End Sub

    Private Sub BtnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnImport.Click
        BtnDownLoad.Visible = False
        divResult.Visible = True
        Dim Str As New StringBuilder
        Session("Tbl") = Nothing
        Try
            ViewState("FileType") = Me.ExcelFileUpload.PostedFile.ContentType
            '  ViewState("FileName") = Me.ExcelFileUpload.PostedFile.FileName

            ''Finding FileType
            If ExcelFileUpload.FileName.ToString.EndsWith(".csv") Or ExcelFileUpload.FileName.ToString.EndsWith(".xls") Or ExcelFileUpload.FileName.ToString.EndsWith(".xlsx") Then

                Dim Foldername As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath")
                If Not Foldername.EndsWith("\") Then
                    Foldername = Foldername & "\"
                End If
                If Directory.Exists(Foldername.Substring(0, Len(Foldername) - 1)) = False Then
                    Directory.CreateDirectory(Foldername.Substring(0, Len(Foldername) - 1))
                End If
                If ExcelFileUpload.FileName.ToString.EndsWith(".csv") Then
                    Dim FName As String
                    FName = Now.Hour & Now.Minute & Now.Second & ExcelFileUpload.FileName
                    ViewState("FileName") = Foldername & FName
                    ViewState("CSVName") = FName
                Else
                    ViewState("FileName") = Foldername & Now.Hour & Now.Minute & Now.Second & ExcelFileUpload.FileName
                End If

                '   ViewState("FileName") = Foldername & ExcelFileUpload.FileName
                ExcelFileUpload.SaveAs(ViewState("FileName"))

                Try
                    System.Threading.Thread.Sleep(2000)

                    Dim ConfirmStr As StringBuilder
                    If ViewState("FileType") IsNot Nothing And ViewState("FileName") IsNot Nothing Then
                        Dim TempTbl As New DataTable
                        TempTbl = Nothing
                        If ViewState("FileName").ToString.EndsWith(".csv") Then
                            DoCSVUpload(TempTbl)
                        ElseIf ViewState("FileName").ToString.EndsWith(".xls") Then
                            DoXLSUpload(TempTbl)
                        ElseIf ViewState("FileName").ToString.EndsWith(".xlsx") Then
                            DoXLSXUpload(TempTbl)
                        End If
                        Str = New StringBuilder
                        ConfirmStr = New StringBuilder
                        If TempTbl IsNot Nothing Then
                            Str = New StringBuilder
                            ConfirmStr = New StringBuilder
                            TempTbl.Columns.Add("ID", GetType(Int32))
                            TempTbl.Columns.Add("Status", GetType(String))
                            TempTbl.Columns.Add("DefaultPlanID", GetType(Int32))
                            If TempTbl.Rows.Count > 0 Then
                                'If DoValidation(TempTbl, Str, ConfirmStr) Then
                                '    Session("DoneValidation") = True
                                '    If Str.ToString() = "" And ConfirmStr.ToString = "" Then
                                '        Err_Desc = Nothing
                                '        Err_No = Nothing
                                '        SavePlan(TempTbl, Err_Desc, Err_No)
                                '    End If
                                'End If
                                Session("Tbl") = TempTbl
                                Dim BRoutePlanforCurmonth As Boolean = False
                                If rdo_Format.SelectedItem.Value = "D" Then
                                    If Rdo_Type.SelectedItem.Value = "1" Then
                                        Dim ValResult As Integer
                                        ValResult = CurrentRouteplan(TempTbl)
                                        If ValResult <> 0 Then
                                            If ValResult = "1" Then
                                                lblinfo.Text = "The file containes routeplan for current month.<br/> Would you like to import  ?"
                                            Else
                                                lblinfo.Text = "The file containes plan for existing route plan.<br/> Would you like to import  ?"
                                            End If
                                            MpInfoConfirm.Show()
                                        Else
                                            If DoValidation(TempTbl, Str, ConfirmStr) Then

                                                Session("DoneValidation") = True


                                                If Str.ToString() = "" And ConfirmStr.ToString = "" Then
                                                    Err_Desc = Nothing
                                                    Err_No = Nothing
                                                    SavePlan(TempTbl, Err_Desc, Err_No)
                                                End If
                                            End If
                                        End If
                                    Else
                                        Dim ValResult As Integer
                                        ValResult = CurrentRouteplanSplitted(TempTbl)
                                        If ValResult <> 0 Then
                                            If ValResult = "1" Then
                                                lblinfo.Text = "The file containes routeplan for current month.<br/> Would you like to import  ?"
                                            Else
                                                lblinfo.Text = "The file containes plan for existing route plan.<br/> Would you like to import  ?"
                                            End If
                                            MpInfoConfirm.Show()
                                        Else
                                            If DoValidationForSplitedColumn(TempTbl, Str, ConfirmStr) Then
                                                Session("DoneValidation") = True
                                                If Str.ToString() = "" And ConfirmStr.ToString = "" Then
                                                    Err_Desc = Nothing
                                                    Err_No = Nothing
                                                    SavePlanSplittedColumn(TempTbl, Err_Desc, Err_No)
                                                End If
                                            End If
                                        End If
                                    End If
                                Else
                                    Dim tblout As New DataTable
                                    If DoValidationForDays(TempTbl, Str, ConfirmStr, tblout) Then
                                        Session("DoneValidation") = True
                                        If Str.ToString() = "" And ConfirmStr.ToString = "" Then
                                            Err_Desc = Nothing
                                            Err_No = Nothing
                                            SavePlanbyDays(tblout, Err_Desc, Err_No)
                                        End If
                                    End If
                                End If
                                    If Not Session("Errordt") Is Nothing Then
                                        BtnDownLoad.Visible = True
                                    End If
                                Else
                                    Session("DoneValidation") = False
                                    Str.Append("<span style='font-color:red'> <b >Template does not have a plan to import</b></span>")
                                End If
                            Else
                                Session("DoneValidation") = False
                                Str.Append("<span style='font-color:red'> <b >Please import valid Excel template.</b></span>")
                            End If
                            lblResult.Text = Str.ToString()
                            DeleteExcel()
                            If lblResult.Text = "" And ConfirmStr.ToString <> "" Then
                                lblResult.Text = "<span style='font-color:red'> <b > Confirm the below Information and Click Reimport to Continue</b></span>" & ConfirmStr.ToString
                                If rdo_Format.SelectedItem.Value = "D" Then
                                    Me.BtnReimport.Enabled = True
                                    Session("Tbl") = TempTbl

                                End If

                            Else
                                Me.BtnReimport.Enabled = False
                            End If
                            Exit Sub
                        End If
                Catch ex As Exception
                    log.Error(GetExceptionInfo(ex))
                    Throw ex
                End Try

                '          Dim strScript As String
                '          strScript = "<script language='javascript'>"
                '          strScript += "document.aspnetForm('ctl00_ContentPlaceHolder1_DummyImBtn').click();"
                '          strScript += "</script>"
                '          Page.ClientScript.RegisterStartupScript(Me.GetType(), "StrScript", _
                'strScript, False)
            Else
                Str.Append("<span style='font-color:red'> <b >Please import valid Excel template.</b></span>")
                lblResult.Text = Str.ToString()
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub BtnReimport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnReimport.Click
        divResult.Visible = True
        If rdo_Format.SelectedItem.Value = "D" Then
            If Rdo_Type.SelectedItem.Value = "1" Then
                 If Session("Tbl") IsNot Nothing And Session("DoneValidation") = True Then
                    bReimport = True
                    SavePlan(CType(Session("Tbl"), DataTable), Err_Desc, Err_No)
                    bReimport = False
                End If
            Else
                If Session("Tbl") IsNot Nothing And Session("DoneValidation") = True Then
                    bReimport = True
                    SavePlanSplittedColumn(CType(Session("Tbl"), DataTable), Err_Desc, Err_No)
                    bReimport = False
                End If
            End If
        End If
    End Sub
    Protected Sub DummyImBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles DummyImBtn.Click
        Try
            BtnDownLoad.Visible = False
            System.Threading.Thread.Sleep(2000)
            Dim Str As StringBuilder
            Dim ConfirmStr As StringBuilder
            If ViewState("FileType") IsNot Nothing And ViewState("FileName") IsNot Nothing Then
                Dim TempTbl As New DataTable
                TempTbl = Nothing
                If ViewState("FileName").ToString.EndsWith(".csv") Then
                    DoCSVUpload(TempTbl)
                ElseIf ViewState("FileName").ToString.EndsWith(".xls") Then
                    DoXLSUpload(TempTbl)
                ElseIf ViewState("FileName").ToString.EndsWith(".xlsx") Then
                    DoXLSXUpload(TempTbl)
                End If
                Str = New StringBuilder
                ConfirmStr = New StringBuilder
                If TempTbl IsNot Nothing Then
                    Str = New StringBuilder
                    ConfirmStr = New StringBuilder
                    If TempTbl.Rows.Count > 0 Then
                        If rdo_Format.SelectedItem.Value = "D" Then
                            If Rdo_Type.SelectedItem.Value = "1" Then
                                If DoValidation(TempTbl, Str, ConfirmStr) Then
                                    Session("DoneValidation") = True
                                    If Str.ToString() = "" And ConfirmStr.ToString = "" Then
                                        Err_Desc = Nothing
                                        Err_No = Nothing
                                        SavePlan(TempTbl, Err_Desc, Err_No)
                                    End If
                                End If
                            Else
                                If DoValidationForSplitedColumn(TempTbl, Str, ConfirmStr) Then
                                    Session("DoneValidation") = True
                                    If Str.ToString() = "" And ConfirmStr.ToString = "" Then
                                        Err_Desc = Nothing
                                        Err_No = Nothing
                                        SavePlanSplittedColumn(TempTbl, Err_Desc, Err_No)
                                    End If
                                End If
                            End If
                        Else
                            Dim tblout As New DataTable
                            If DoValidationForDays(TempTbl, Str, ConfirmStr, tblout) Then
                                Session("DoneValidation") = True
                                If Str.ToString() = "" And ConfirmStr.ToString = "" Then
                                    Err_Desc = Nothing
                                    Err_No = Nothing
                                    SavePlanbyDays(tblout, Err_Desc, Err_No)
                                End If
                            End If
                        End If
                        If Not Session("Errordt") Is Nothing Then
                            BtnDownLoad.Visible = True
                        End If
                    Else
                        Session("DoneValidation") = False
                        Str.Append("<span style='font-color:red'> <b >Template does not have a plan to import</b></span>")
                    End If
                Else
                    Session("DoneValidation") = False
                    Str.Append("<span style='font-color:red'> <b >Please import valid Excel template.</b></span>")
                End If
                lblResult.Text = Str.ToString()
                DeleteExcel()
                If lblResult.Text = "" And ConfirmStr.ToString <> "" Then
                    lblResult.Text = "<span style='font-color:red'> <b > Confirm the below Information and Click Reimport to Continue</b></span>" & ConfirmStr.ToString
                    Me.BtnReimport.Enabled = True
                    Session("Tbl") = TempTbl
                Else
                    Me.BtnReimport.Enabled = False
                End If
                Exit Sub
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        End Try
    End Sub
    Private Sub BtnDownLoad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnDownLoad.Click
        CsvExport()
    End Sub
    Private Sub SavePlanSplittedColumn(ByVal Tbl As DataTable, ByVal Err_Desc As String, ByVal Err_No As Long)
        Dim FilterTable As New DataTable
        Dim objRoute As New RoutePlan
        Try
            ''Selecting Unique Default Plan IDs
            FilterTable = SelectDistinct(Tbl, "DefaultPlanID", "Van ID")
            If FilterTable IsNot Nothing Then
                Dim transaction As SqlTransaction

                Dim SqlConn As SqlConnection
                Dim SqlCmd As SqlCommand
                SqlConn = objRoutePlan.GetConnection()

                transaction = SqlConn.BeginTransaction("SampleTransaction")
                For Each FilteredRow As DataRow In FilterTable.Rows

                    Try
                        ''Filtering each Default Plan ID's data from parent datatable
                        Dim Express As String = "DefaultPlanID=" & FilteredRow.Item(0) & " and [Van ID]='" & FilteredRow.Item(1) & "'"
                        ' Dim Express As String = "DefaultPlanID=" & FilteredRow(0) & " and [Van ID]=" & FilteredRow(1)
                        Dim SelectedRow() As DataRow = Tbl.Select(Express)
                        If SelectedRow.Length > 0 Then
                            ''Ignore for Current Month
                            Dim FRSID As Integer = 0
                            Dim CanVisit As String = Nothing
                            Dim VDay As Date
                            Dim DeletedFSR As Boolean = False
                            If Not SelectedRow(0).Item(6).ToString.StartsWith("HAVECURRENTMONTH") Or bReimport = True Then
                                Err_Desc = Nothing
                                Err_No = Nothing
                                '                                FRSID = objRoute.GetFSRPlanID(transaction, SqlConn, SqlCmd, FilteredRow.Item(0), SelectedRow(0).Item(0), Err_No, Err_Desc)

                                FRSID = objRoute.GetFSRPlanID(transaction, SqlConn, SqlCmd, FilteredRow.Item(0), SelectedRow(0).Item(0), Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)



                                If Err_Desc Is Nothing Then
                                    If FRSID <> 0 Then
                                        ''Inserting one by one
                                        For i As Integer = 0 To SelectedRow.Length - 1
                                            Try
                                                Err_Desc = Nothing
                                                Err_No = Nothing
                                                With objRoute
                                                    .FSRPlanId = FRSID
                                                    .day = SelectedRow(i).Item(3)
                                                    VDay = CDate(SelectedRow(i).Item(4) & "/" & SelectedRow(i).Item(3) & "/" & SelectedRow(i).Item(5))
                                                    CanVisit = objRoute.GetDayType(transaction, SqlConn, SqlCmd, FilteredRow.Item(0), SelectedRow(i).Item(3), Err_No, Err_Desc)
                                                    If CanVisit = "X" Then
                                                        Dim y As String = objRoute.GetCustomerId(transaction, SqlConn, SqlCmd, SelectedRow(i).Item(1), SelectedRow(i).Item("SiteID"), Err_No, Err_Desc)
                                                        .CustomerID = IIf(y Is Nothing Or y = "", 0, y)
                                                        ' .CustomerID = SelectedRow(i).Item(1)
                                                        .UserSiteID = SelectedRow(i).Item("SiteID")
                                                        If Not SelectedRow(i).Item(1) Is DBNull.Value And Not SelectedRow(i).Item("SiteID") Is DBNull.Value Then
                                                            .DayType = "V"
                                                            CanVisit = "V"
                                                        Else
                                                            .DayType = CanVisit
                                                        End If
                                                        'If SelectedRow(i).Item(3) Is DBNull.Value Then
                                                        .StartTime = "1/1/1900"
                                                        'Else
                                                        '    '.StartTime = Convert.ToDateTime("1/1/1900 " & CDate(SelectedRow(i).Item(3)).TimeOfDay.ToString)
                                                        '    .StartTime = Convert.ToDateTime(CDate(SelectedRow(i).Item(2)).Month.ToString & "-" & CDate(SelectedRow(i).Item(2)).Day.ToString & "-" & CDate(SelectedRow(i).Item(2)).Year.ToString & " " & CDate(SelectedRow(i).Item(3)).TimeOfDay.ToString)
                                                        'End If

                                                        'If SelectedRow(i).Item(4) Is DBNull.Value Then
                                                        .End_Time = "1/1/1900"
                                                        'Else
                                                        '    .End_Time = Convert.ToDateTime(CDate(SelectedRow(i).Item(2)).Month.ToString & "-" & CDate(SelectedRow(i).Item(2)).Day.ToString & "-" & CDate(SelectedRow(i).Item(2)).Year.ToString & " " & CDate(SelectedRow(i).Item(4)).TimeOfDay.ToString)
                                                        'End If
                                                    Else
                                                        .DayType = CanVisit
                                                        .CustomerID = 0
                                                        .UserSiteID = 0
                                                        .End_Time = "1/1/1900"
                                                        .StartTime = "1/1/1900"
                                                    End If

                                                    '.DayType = "V"

                                                End With


                                                If CanVisit = "V" Then
                                                    If objRoute.CheckVisitExist(transaction, SqlConn, SqlCmd, Err_No, Err_Desc) = False Then
                                                        If VDay > Now.Date Then
                                                            Dim Seq As String
                                                            Seq = objRoute.GetSequence(Err_No, Err_Desc, SqlConn, SqlCmd, transaction, SelectedRow(0).Item(0))
                                                            If Val(Seq) > 0 Then
                                                                objRoute.Sequence = CInt(Seq)
                                                                objRoute.AllowOptimization = "0"
                                                                objRoute.InsertFSRPlanDetails(Err_No, Err_Desc, SqlConn, SqlCmd, transaction)
                                                            End If
                                                        End If
                                                    End If
                                                End If
                                                If Err_Desc IsNot Nothing Then
                                                    log.Error(Err_Desc & " For FSR ID : " & SelectedRow(i).Item(0) & " Customer ID : " & SelectedRow(i).Item(1) & " Site ID : " & SelectedRow(i).Item(2))
                                                End If
                                            Catch ex As Exception
                                                transaction.Rollback()
                                                log.Error(GetExceptionInfo(ex) & " For FSR ID : " & SelectedRow(i).Item(0) & " Customer ID : " & SelectedRow(i).Item(1) & " Site ID : " & SelectedRow(i).Item(2))
                                                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & SalesWorx.BO.Common.AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=ImportRoutePlan.aspx&Title=Route+Planner", False)
                                                Exit Sub
                                            End Try
                                        Next
                                    Else
                                        log.Error("FSR ID is 0 For Default Plan ID : " & FilteredRow.Item(0))
                                    End If
                                Else
                                    transaction.Rollback()
                                    log.Error(Err_Desc & " For Default Plan ID : " & FilteredRow.Item(0))
                                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & SalesWorx.BO.Common.AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=ImportRoutePlan.aspx&Title=Route+Planner", False)
                                    Exit Sub
                                End If
                            End If
                        End If

                    Catch ex As Exception
                        log.Error(GetExceptionInfo(ex) & " For Default Plan ID : " & FilteredRow.Item(0))
                        Response.Redirect("information.aspx?mode=1&errno=" & "74087" & "&msg=" & SalesWorx.BO.Common.AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=ImportRoutePlan.aspx&Title=Route+Planner", False)
                    End Try
                Next
                transaction.Commit()
                objRoutePlan.CloseConnection(SqlConn)
                transaction = Nothing
                DeleteExcel()
                Response.Redirect("information.aspx?mode=0&msg=Route+Plan+has+been+imported+successfully&next=ImportRoutePlan.aspx&Title=Route+Planner", False)

            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("information.aspx?mode=1&errno=" & "74088" & "&msg=" & SalesWorx.BO.Common.AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=ImportRoutePlan.aspx&Title=Route+Planner", False)
        Finally
            FilterTable = Nothing
            objRoute = Nothing
        End Try
    End Sub
    Private Sub SavePlanbyDays(ByVal Tbl As DataTable, ByVal Err_Desc As String, ByVal Err_No As Long)
        Dim FilterTable As New DataTable
        Dim objRoute As New RoutePlan
        Try
            ''Selecting Unique Default Plan IDs
            FilterTable = SelectDistinct(Tbl, "DefaultPlanID", "Van ID")
            If FilterTable IsNot Nothing Then
                Dim transaction As SqlTransaction

                Dim SqlConn As SqlConnection
                Dim SqlCmd As SqlCommand
                SqlConn = objRoutePlan.GetConnection()

                transaction = SqlConn.BeginTransaction("SampleTransaction")
                For Each FilteredRow As DataRow In FilterTable.Rows

                    Try
                        ''Filtering each Default Plan ID's data from parent datatable
                        Dim Express As String = "DefaultPlanID=" & FilteredRow.Item(0) & " and [Van ID]='" & FilteredRow.Item(1) & "'"
                        ' Dim Express As String = "DefaultPlanID=" & FilteredRow(0) & " and [Van ID]=" & FilteredRow(1)
                        Dim SelectedRow() As DataRow = Tbl.Select(Express)
                        If SelectedRow.Length > 0 Then
                            ''Ignore for Current Month
                            Dim FRSID As Integer = 0
                            Dim CanVisit As String = Nothing
                            Dim VDay As Date
                            Dim DeletedFSR As Boolean = False
                            If Not SelectedRow(0).Item(4).ToString.StartsWith("HAVECURRENTMONTH") Or bReimport = True Then
                                Err_Desc = Nothing
                                Err_No = Nothing
                                '                                FRSID = objRoute.GetFSRPlanID(transaction, SqlConn, SqlCmd, FilteredRow.Item(0), SelectedRow(0).Item(0), Err_No, Err_Desc)

                                FRSID = objRoute.GetFSRPlanID(transaction, SqlConn, SqlCmd, FilteredRow.Item(0), SelectedRow(0).Item(0), Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)



                                If Err_Desc Is Nothing Then
                                    If FRSID <> 0 Then
                                        ''Inserting one by one
                                        For i As Integer = 0 To SelectedRow.Length - 1
                                            Try
                                                Err_Desc = Nothing
                                                Err_No = Nothing
                                                With objRoute
                                                    .FSRPlanId = FRSID
                                                    .day = CDate(SelectedRow(i).Item(3)).Day
                                                    VDay = CDate(SelectedRow(i).Item(3))
                                                    CanVisit = objRoute.GetDayType(transaction, SqlConn, SqlCmd, FilteredRow.Item(0), CDate(SelectedRow(i).Item(3)).Day, Err_No, Err_Desc)
                                                    If CanVisit = "X" Then
                                                        Dim y As String = objRoute.GetCustomerId(transaction, SqlConn, SqlCmd, SelectedRow(i).Item(1), SelectedRow(i).Item("SiteID"), Err_No, Err_Desc)
                                                        .CustomerID = IIf(y Is Nothing Or y = "", 0, y)
                                                        ' .CustomerID = SelectedRow(i).Item(1)
                                                        .UserSiteID = SelectedRow(i).Item("SiteID")
                                                        If Not SelectedRow(i).Item(1) Is DBNull.Value And Not SelectedRow(i).Item("SiteID") Is DBNull.Value Then
                                                            .DayType = "V"
                                                            CanVisit = "V"
                                                        Else
                                                            .DayType = CanVisit
                                                        End If
                                                        'If SelectedRow(i).Item(3) Is DBNull.Value Then
                                                        .StartTime = "1/1/1900"
                                                        'Else
                                                        '    '.StartTime = Convert.ToDateTime("1/1/1900 " & CDate(SelectedRow(i).Item(3)).TimeOfDay.ToString)
                                                        '    .StartTime = Convert.ToDateTime(CDate(SelectedRow(i).Item(2)).Month.ToString & "-" & CDate(SelectedRow(i).Item(2)).Day.ToString & "-" & CDate(SelectedRow(i).Item(2)).Year.ToString & " " & CDate(SelectedRow(i).Item(3)).TimeOfDay.ToString)
                                                        'End If

                                                        'If SelectedRow(i).Item(4) Is DBNull.Value Then
                                                        .End_Time = "1/1/1900"
                                                        'Else
                                                        '    .End_Time = Convert.ToDateTime(CDate(SelectedRow(i).Item(2)).Month.ToString & "-" & CDate(SelectedRow(i).Item(2)).Day.ToString & "-" & CDate(SelectedRow(i).Item(2)).Year.ToString & " " & CDate(SelectedRow(i).Item(4)).TimeOfDay.ToString)
                                                        'End If
                                                    Else
                                                        .DayType = CanVisit
                                                        .CustomerID = 0
                                                        .UserSiteID = 0
                                                        .End_Time = "1/1/1900"
                                                        .StartTime = "1/1/1900"
                                                    End If

                                                    '.DayType = "V"

                                                End With


                                                If CanVisit = "V" Then
                                                    If objRoute.CheckVisitExist(transaction, SqlConn, SqlCmd, Err_No, Err_Desc) = False Then
                                                        If VDay > Now.Date Then
                                                            Dim Seq As String
                                                            Seq = objRoute.GetSequence(Err_No, Err_Desc, SqlConn, SqlCmd, transaction, SelectedRow(0).Item(0))
                                                            If Val(Seq) > 0 Then
                                                                objRoute.Sequence = CInt(Seq)
                                                                objRoute.AllowOptimization = "0"
                                                                objRoute.InsertFSRPlanDetailsbyDay(Err_No, Err_Desc, SqlConn, SqlCmd, transaction)
                                                            End If
                                                        End If
                                                    End If
                                                End If
                                                If Err_Desc IsNot Nothing Then
                                                    log.Error(Err_Desc & " For FSR ID : " & SelectedRow(i).Item(0) & " Customer ID : " & SelectedRow(i).Item(1) & " Site ID : " & SelectedRow(i).Item(2))
                                                End If
                                            Catch ex As Exception
                                                transaction.Rollback()
                                                log.Error(GetExceptionInfo(ex) & " For FSR ID : " & SelectedRow(i).Item(0) & " Customer ID : " & SelectedRow(i).Item(1) & " Site ID : " & SelectedRow(i).Item(2))
                                                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & SalesWorx.BO.Common.AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=ImportRoutePlan.aspx&Title=Route+Planner", False)
                                                Exit Sub
                                            End Try
                                        Next
                                    Else
                                        log.Error("FSR ID is 0 For Default Plan ID : " & FilteredRow.Item(0))
                                    End If
                                Else
                                    transaction.Rollback()
                                    log.Error(Err_Desc & " For Default Plan ID : " & FilteredRow.Item(0))
                                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & SalesWorx.BO.Common.AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=ImportRoutePlan.aspx&Title=Route+Planner", False)
                                    Exit Sub
                                End If
                            End If
                        End If

                    Catch ex As Exception
                        log.Error(GetExceptionInfo(ex) & " For Default Plan ID : " & FilteredRow.Item(0))
                        Response.Redirect("information.aspx?mode=1&errno=" & "74087" & "&msg=" & SalesWorx.BO.Common.AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=ImportRoutePlan.aspx&Title=Route+Planner", False)
                    End Try
                Next
                transaction.Commit()
                objRoutePlan.CloseConnection(SqlConn)
                transaction = Nothing
                DeleteExcel()
                Response.Redirect("information.aspx?mode=0&msg=Route+Plan+has+been+imported+successfully&next=ImportRoutePlan.aspx&Title=Route+Planner", False)

            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("information.aspx?mode=1&errno=" & "74088" & "&msg=" & SalesWorx.BO.Common.AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=ImportRoutePlan.aspx&Title=Route+Planner", False)
        Finally
            FilterTable = Nothing
            objRoute = Nothing
        End Try
    End Sub
    Private Sub SavePlan(ByVal Tbl As DataTable, ByVal Err_Desc As String, ByVal Err_No As Long)
        Dim FilterTable As New DataTable
        Dim objRoute As New RoutePlan
        Try
            ''Selecting Unique Default Plan IDs
            FilterTable = SelectDistinct(Tbl, "DefaultPlanID", "Van ID")
            If FilterTable IsNot Nothing Then
                Dim transaction As SqlTransaction

                Dim SqlConn As SqlConnection
                Dim SqlCmd As SqlCommand
                SqlConn = objRoutePlan.GetConnection()

                transaction = SqlConn.BeginTransaction("SampleTransaction")
                For Each FilteredRow As DataRow In FilterTable.Rows

                    Try
                        ''Filtering each Default Plan ID's data from parent datatable
                        Dim Express As String = "DefaultPlanID=" & FilteredRow.Item(0) & " and [Van ID]='" & FilteredRow.Item(1) & "'"
                        ' Dim Express As String = "DefaultPlanID=" & FilteredRow(0) & " and [Van ID]=" & FilteredRow(1)
                        Dim SelectedRow() As DataRow = Tbl.Select(Express)
                        If SelectedRow.Length > 0 Then
                            ''Ignore for Current Month
                            Dim FRSID As Integer = 0
                            Dim CanVisit As String = Nothing
                            Dim VDay As Date
                            Dim DeletedFSR As Boolean = False
                            If Not SelectedRow(0).Item(4).ToString.StartsWith("HAVECURRENTMONTH") Or bReimport = True Then
                                Err_Desc = Nothing
                                Err_No = Nothing
                                '                                FRSID = objRoute.GetFSRPlanID(transaction, SqlConn, SqlCmd, FilteredRow.Item(0), SelectedRow(0).Item(0), Err_No, Err_Desc)

                                FRSID = objRoute.GetFSRPlanID(transaction, SqlConn, SqlCmd, FilteredRow.Item(0), SelectedRow(0).Item(0), Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)



                                If Err_Desc Is Nothing Then
                                    If FRSID <> 0 Then
                                        ''Inserting one by one
                                        For i As Integer = 0 To SelectedRow.Length - 1
                                            Try
                                                Err_Desc = Nothing
                                                Err_No = Nothing
                                                With objRoute
                                                    .FSRPlanId = FRSID
                                                    .day = CDate(SelectedRow(i).Item(3)).Day
                                                    VDay = CDate(SelectedRow(i).Item(3))
                                                    CanVisit = objRoute.GetDayType(transaction, SqlConn, SqlCmd, FilteredRow.Item(0), CDate(SelectedRow(i).Item(3)).Day, Err_No, Err_Desc)
                                                    If CanVisit = "X" Then
                                                        Dim y As String = objRoute.GetCustomerId(transaction, SqlConn, SqlCmd, SelectedRow(i).Item(1), SelectedRow(i).Item("SiteID"), Err_No, Err_Desc)
                                                        .CustomerID = IIf(y Is Nothing Or y = "", 0, y)
                                                        ' .CustomerID = SelectedRow(i).Item(1)
                                                        .UserSiteID = SelectedRow(i).Item("SiteID")
                                                        If Not SelectedRow(i).Item(1) Is DBNull.Value And Not SelectedRow(i).Item("SiteID") Is DBNull.Value Then
                                                            .DayType = "V"
                                                            CanVisit = "V"
                                                        Else
                                                            .DayType = CanVisit
                                                        End If
                                                        'If SelectedRow(i).Item(3) Is DBNull.Value Then
                                                        .StartTime = "1/1/1900"
                                                        'Else
                                                        '    '.StartTime = Convert.ToDateTime("1/1/1900 " & CDate(SelectedRow(i).Item(3)).TimeOfDay.ToString)
                                                        '    .StartTime = Convert.ToDateTime(CDate(SelectedRow(i).Item(2)).Month.ToString & "-" & CDate(SelectedRow(i).Item(2)).Day.ToString & "-" & CDate(SelectedRow(i).Item(2)).Year.ToString & " " & CDate(SelectedRow(i).Item(3)).TimeOfDay.ToString)
                                                        'End If

                                                        'If SelectedRow(i).Item(4) Is DBNull.Value Then
                                                        .End_Time = "1/1/1900"
                                                        'Else
                                                        '    .End_Time = Convert.ToDateTime(CDate(SelectedRow(i).Item(2)).Month.ToString & "-" & CDate(SelectedRow(i).Item(2)).Day.ToString & "-" & CDate(SelectedRow(i).Item(2)).Year.ToString & " " & CDate(SelectedRow(i).Item(4)).TimeOfDay.ToString)
                                                        'End If
                                                    Else
                                                        .DayType = CanVisit
                                                        .CustomerID = 0
                                                        .UserSiteID = 0
                                                        .End_Time = "1/1/1900"
                                                        .StartTime = "1/1/1900"
                                                    End If

                                                    '.DayType = "V"

                                                End With


                                                If CanVisit = "V" Then
                                                    If objRoute.CheckVisitExist(transaction, SqlConn, SqlCmd, Err_No, Err_Desc) = False Then
                                                        If VDay > Now.Date Then
                                                            Dim Seq As String
                                                            Seq = objRoute.GetSequence(Err_No, Err_Desc, SqlConn, SqlCmd, transaction, SelectedRow(0).Item(0))
                                                            If Val(Seq) > 0 Then
                                                                objRoute.Sequence = CInt(Seq)
                                                                objRoute.AllowOptimization = "0"
                                                                objRoute.InsertFSRPlanDetails(Err_No, Err_Desc, SqlConn, SqlCmd, transaction)
                                                            End If
                                                        End If
                                                    End If
                                                End If
                                                If Err_Desc IsNot Nothing Then
                                                    log.Error(Err_Desc & " For FSR ID : " & SelectedRow(i).Item(0) & " Customer ID : " & SelectedRow(i).Item(1) & " Site ID : " & SelectedRow(i).Item(2))
                                                End If
                                            Catch ex As Exception
                                                transaction.Rollback()
                                                log.Error(GetExceptionInfo(ex) & " For FSR ID : " & SelectedRow(i).Item(0) & " Customer ID : " & SelectedRow(i).Item(1) & " Site ID : " & SelectedRow(i).Item(2))
                                                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & SalesWorx.BO.Common.AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=ImportRoutePlan.aspx&Title=Route+Planner", False)
                                                Exit Sub
                                            End Try
                                        Next
                                    Else
                                        log.Error("FSR ID is 0 For Default Plan ID : " & FilteredRow.Item(0))
                                    End If
                                Else
                                    transaction.Rollback()
                                    log.Error(Err_Desc & " For Default Plan ID : " & FilteredRow.Item(0))
                                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & SalesWorx.BO.Common.AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=ImportRoutePlan.aspx&Title=Route+Planner", False)
                                    Exit Sub
                                End If
                            End If
                        End If

                    Catch ex As Exception
                        log.Error(GetExceptionInfo(ex) & " For Default Plan ID : " & FilteredRow.Item(0))
                        Response.Redirect("information.aspx?mode=1&errno=" & "74087" & "&msg=" & SalesWorx.BO.Common.AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=ImportRoutePlan.aspx&Title=Route+Planner", False)
                    End Try
                Next
                transaction.Commit()
                objRoutePlan.CloseConnection(SqlConn)
                transaction = Nothing
                DeleteExcel()
                Response.Redirect("information.aspx?mode=0&msg=Route+Plan+has+been+imported+successfully&next=ImportRoutePlan.aspx&Title=Route+Planner", False)

            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("information.aspx?mode=1&errno=" & "74088" & "&msg=" & SalesWorx.BO.Common.AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=ImportRoutePlan.aspx&Title=Route+Planner", False)
        Finally
            FilterTable = Nothing
            objRoute = Nothing
        End Try
    End Sub
    Private Function DoValidationForDays(ByVal Tbl1 As DataTable, ByVal Str As StringBuilder, ByRef ConfirmStr As StringBuilder, ByRef tablOut As DataTable) As Boolean
        Try
            ''Adding identity Column in Datatable
            Session("Errordt") = Nothing
            Dim dtError As New DataTable
            dtError.Columns.Add("Error")

            If Default_Plan_DD.SelectedItem.Value = "0" Then
                Str.Append("<br/><span style='font-color:red'> <b >Please select the deafult Plan </b></span>")
                Return False
            End If

            Dim DefRoutPlDt As New DataTable
            DefRoutPlDt = objRoutePlan.GetDefaultPlanDetails(Err_No, Err_Desc, Default_Plan_DD.SelectedItem.Value)

            Dim Startdate As String
            Dim Enddate As String

            Startdate = DefRoutPlDt.Rows(0)("Start_Date").ToString
            Enddate = DefRoutPlDt.Rows(0)("End_Date").ToString

            Dim TempTbl As New DataTable
            TempTbl.Columns.Add("Van ID", GetType(String))
            TempTbl.Columns.Add("Customer ID", GetType(String))
            TempTbl.Columns.Add("SiteID", GetType(String))
            TempTbl.Columns.Add("Day", GetType(String))

            Dim Tbl As New DataTable
            Tbl.Columns.Add("Van ID", GetType(String))
            Tbl.Columns.Add("Customer ID", GetType(String))
            Tbl.Columns.Add("SiteID", GetType(String))
            Tbl.Columns.Add("Date", GetType(String))
            Tbl.Columns.Add("ID", GetType(Int32))
            Tbl.Columns.Add("Status", GetType(String))
            Tbl.Columns.Add("DefaultPlanID", GetType(Int32))

            Dim RowCount As Integer = 1

            For Each dr As DataRow In Tbl1.Rows
                RowCount += 1

                If Not dr.Item(0) Is DBNull.Value Then
                    Dim HasNull As Boolean = False

                    If dr.Item(0) Is DBNull.Value OrElse dr.Item(0).ToString = "" Then
                        HasNull = True
                        Session("DoneValidation") = False
                        Str.Append("<br/><span style='font-color:red'> <b >Invalid FSR Id is at Row No: " & RowCount & " </b></span>")
                        Dim ErrorDr As DataRow
                        ErrorDr = dtError.NewRow
                        ErrorDr(0) = "Invalid FSR Id is at Row No: " & RowCount
                        dtError.Rows.Add(ErrorDr)
                    End If


                    If dr.Item(1) Is DBNull.Value OrElse dr.Item(1).ToString = "" Then
                        HasNull = True
                        Session("DoneValidation") = False
                        Str.Append("<br/><span style='font-color:red'> <b >Customer Id is Null at Row No: " & RowCount & " </b></span>")
                        Dim ErrorDr As DataRow
                        ErrorDr = dtError.NewRow
                        ErrorDr(0) = "Customer Id is Null at Row No: " & RowCount
                        dtError.Rows.Add(ErrorDr)
                    End If

                    If dr.Item(2) Is DBNull.Value OrElse dr.Item(2).ToString = "" Then
                        HasNull = True
                        Session("DoneValidation") = False
                        Str.Append("<br/><span style='font-color:red'> <b >Site Use Id is Null at Row No: " & RowCount & " </b></span>")
                        Dim ErrorDr As DataRow
                        ErrorDr = dtError.NewRow
                        ErrorDr(0) = "Site Use Id is Null at Row No: " & RowCount
                        dtError.Rows.Add(ErrorDr)
                    End If

                    '

                    If dr.Item(3) Is DBNull.Value OrElse dr.Item(3).ToString = "" Then
                        HasNull = True
                        Session("DoneValidation") = False
                        Str.Append("<br/><span style='font-color:red'> <b >Day is Null at Row No: " & RowCount & " </b></span>")
                        Dim ErrorDr As DataRow
                        ErrorDr = dtError.NewRow
                        ErrorDr(0) = "Day is Null at Row No: " & RowCount
                        dtError.Rows.Add(ErrorDr)
                    Else
                        If Not (dr.Item(3).ToString.ToUpper() = "SUN" Or dr.Item(3).ToString.ToUpper() = "MON" Or dr.Item(3).ToString.ToUpper() = "TUE" Or dr.Item(3).ToString.ToUpper() = "WED" Or dr.Item(3).ToString.ToUpper() = "THU" Or dr.Item(3).ToString.ToUpper() = "MON" Or dr.Item(3).ToString.ToUpper() = "FRI" Or dr.Item(3).ToString.ToUpper() = "SAT") Then
                            HasNull = True
                            Session("DoneValidation") = False
                            Str.Append("<br/><span style='font-color:red'> <b >Invalid Day at Row No: " & RowCount & " </b></span>")
                            Dim ErrorDr As DataRow
                            ErrorDr = dtError.NewRow
                            ErrorDr(0) = "Invalid Day at Row No: " & RowCount
                            dtError.Rows.Add(ErrorDr)
                        End If
                    End If
                    Err_No = Nothing
                    Err_Desc = Nothing
                    If HasNull = False Then
                        Dim Site_ID As Integer = 0
                        Dim FSRExist As Boolean = False
                        Site_ID = objRoutePlan.CheckVanNCustomer(dr.Item(0), dr.Item(1), dr.Item(2), Err_No, Err_Desc)
                        If Site_ID <> 0 Then
                            dr("SiteID") = Site_ID
                            If (objRoutePlan.CheckValidFSR(dr.Item(0), CType(Session("User_Access"), UserAccess).UserID, Err_No, Err_Desc)) = False Then
                                Str.Append("<br/><span style='font-color:red'> <b >The login user do not have link with the FSR at Row No : " & RowCount & " , Van ID : " & dr.Item(0).ToString & " </b></span>")
                                HasNull = True
                                Session("DoneValidation") = False
                                Dim ErrorDr As DataRow
                                ErrorDr = dtError.NewRow
                                ErrorDr(0) = "The login user do not have link with the FSR at Row No : " & RowCount & " , Van ID : " & dr.Item(0).ToString & ""
                                dtError.Rows.Add(ErrorDr)
                            End If
                            Err_No = Nothing
                            Err_Desc = Nothing
                        Else
                            If Err_Desc IsNot Nothing Then log.Error(Err_Desc & " at Row : " & RowCount)
                            HasNull = True
                            Session("DoneValidation") = False
                            Str.Append("<br/><span style='font-color:red'> <b >FSR ID , Customer ID and Site ID is not matching at Row No: " & RowCount & " , Van ID : " & dr.Item(0).ToString & " , Customer No : " & dr.Item(1).ToString & ", Site ID :" & dr.Item(2).ToString & " </b></span>")
                            Dim ErrorDr As DataRow
                            ErrorDr = dtError.NewRow
                            ErrorDr(0) = "FSR ID , Customer ID and Site ID is not matching at Row No: " & RowCount & " , Van ID : " & dr.Item(0).ToString & " , Customer No : " & dr.Item(1).ToString & ", Site ID :" & dr.Item(2).ToString
                            dtError.Rows.Add(ErrorDr)
                        End If
                    End If
                    If HasNull = False Then
                        Dim newdr As DataRow
                        newdr = TempTbl.NewRow
                        newdr("Van ID") = dr.Item(0)
                        newdr("Customer ID") = dr.Item(1)
                        newdr("SiteID") = dr.Item(2)
                        newdr("Day") = dr.Item(3).ToString.ToUpper()
                        TempTbl.Rows.Add(newdr)
                    End If
                End If
            Next

            For i = 0 To 6
                Dim dayname As String
                Dim tdate As Date
                tdate = DateAdd(DateInterval.Day, i, CDate(Startdate))
                dayname = tdate.ToString("ddd")
                Dim seldr() As DataRow
                seldr = TempTbl.Select("Day='" & dayname.ToUpper & "'")
                If seldr.Length > 0 Then
                    For Each dr In seldr
                        For j As Integer = 0 To 6
                            Dim k As Integer
                            k = j * 7
                            Dim dt As Date
                            dt = DateAdd(DateInterval.Day, k, tdate)
                            If dt <= CDate(Enddate) Then
                                Dim newdr As DataRow
                                newdr = Tbl.NewRow
                                newdr("Van ID") = dr.Item(0)
                                newdr("Customer ID") = dr.Item(1)
                                newdr("SiteID") = dr.Item(2)
                                newdr("Date") = dt
                                newdr("DefaultPlanID") = Default_Plan_DD.SelectedItem.Value
                                Tbl.Rows.Add(newdr)
                            End If
                        Next
                    Next
                End If


            Next

            For i As Integer = 1 To Tbl.Rows.Count
                Tbl.Rows(i - 1).Item(6) = i
            Next


            For Each dr As DataRow In Tbl.Rows
                Try
                    Dim Dat As DateTime

                    ''Check having valid date
                    Try
                        Dat = dr.Item(3)
                    Catch ex As Exception

                    End Try

                    Dim Result As String
                    Result = objRoutePlan.RoutePlanExist(Dat, dr.Item(0), Err_No, Err_Desc)

                    dr("Status") = Result

                    If Result.StartsWith("HAVECURRENTMONTH") Then
                        ConfirmStr.Append("<br/><span style='font-color:red'> <b >Row No : " & RowCount & " Current Month Data will be ignored</b></span>" & "")
                        Dim ErrorDr As DataRow
                        ErrorDr = dtError.NewRow
                        ErrorDr(0) = "Row No : " & RowCount & " Current Month Data will be ignored"
                        dtError.Rows.Add(ErrorDr)
                    ElseIf Result.StartsWith("HAVEFUTUREMONTH") Then
                        ConfirmStr.Append("<br/><span style='font-color:red'> <b >Row No : " & RowCount & " Route Plan already exists for given month.</b></span>" & "")
                        Dim ErrorDr As DataRow
                        ErrorDr = dtError.NewRow
                        ErrorDr(0) = "Row No : " & RowCount & " Route Plan already exists for given month."
                        dtError.Rows.Add(ErrorDr)
                    End If


                    If Result.StartsWith("NOROUTEPLAN") Or Result.StartsWith("HAVEFUTUREMONTH") Or Result.StartsWith("HAVECURRENTMONTH") Then

                        Dim ResArray As Array = Result.Split("_")
                        If ResArray.Length = 2 Then
                            dr("DefaultPlanID") = ResArray(1)
                        End If


                    ElseIf Result = "" Then
                        If Err_Desc IsNot Nothing Then log.Error(Err_Desc & " at Row : " & RowCount)
                    ElseIf Result = "NODEFAULT" Then
                        Session("DoneValidation") = False
                        Str.Append("<br/><span style='font-color:red'> <b >There is no default plan for van : " & dr.Item(0) & " at " & RowCount & " </b></span>")
                        Dim ErrorDr As DataRow
                        ErrorDr = dtError.NewRow
                        ErrorDr(0) = "There is no default plan for van : " & dr.Item(0) & " at row :" & RowCount & ""
                        dtError.Rows.Add(ErrorDr)
                        Exit For
                    End If



                Catch ex As Exception
                    log.Error(GetExceptionInfo(ex))
                    Response.Redirect("information.aspx?mode=1&errno=" & "74089" & "&msg=" & SalesWorx.BO.Common.AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=ImportRoutePlan.aspx&Title=Route+Planner", False)
                End Try
            Next
            tablOut = Tbl.Copy
            Session("Errordt") = dtError.Copy
            If Str.ToString() <> "" Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("information.aspx?mode=1&errno=" & "74090" & "&msg=" & SalesWorx.BO.Common.AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=ImportRoutePlan.aspx&Title=Route+Planner", False)
            Throw ex
        End Try
    End Function

    Private Function CurrentRouteplanSplitted(ByVal Tbl As DataTable) As Integer
        Try


            Dim bRetCurMOnth As Integer = 0
            Dim i As Integer = -1
            For Each dr As DataRow In Tbl.Rows
                i = i + 1
                Dim Dat As String
                Dat = dr.Item(4) & "/" & dr.Item(3) & "/" & dr.Item(5)
                Err_No = Nothing
                Err_Desc = Nothing

                Dim Result As String

                Result = objRoutePlan.RoutePlanExist(Dat, dr.Item(0), Err_No, Err_Desc)
                If Result.StartsWith("HAVECURRENTMONTH") Then
                    bRetCurMOnth = 1
                    Exit For
                End If
                If Result.StartsWith("HAVEFUTUREMONTH") Then
                    bRetCurMOnth = 2
                    Exit For
                End If

            Next
            Return bRetCurMOnth
        Catch ex As Exception

        End Try
    End Function
    Private Function CurrentRouteplan(ByVal Tbl As DataTable) As Integer
        Try
            Dim bRetCurMOnth As Integer = 0
            For Each dr As DataRow In Tbl.Rows
                Dim Dat As String
                Dat = dr.Item(3)
                Err_No = Nothing
                Err_Desc = Nothing

                Dim Result As String

                Result = objRoutePlan.RoutePlanExist(Dat, dr.Item(0), Err_No, Err_Desc)
                If Result.StartsWith("HAVECURRENTMONTH") Then
                    bRetCurMOnth = 1
                    Exit For
                End If
                If Result.StartsWith("HAVEFUTUREMONTH") Then
                    bRetCurMOnth = 2
                    Exit For
                End If
            Next
            Return bRetCurMOnth
        Catch ex As Exception

        End Try
        
    End Function

    Private Function DoValidation(ByVal Tbl As DataTable, ByVal Str As StringBuilder, ByRef ConfirmStr As StringBuilder) As Boolean
        Try
            Session("Errordt") = Nothing
            Dim dtError As New DataTable
            dtError.Columns.Add("Error")
            ''Adding identity Column in Datatable
           
            ' Tbl.Columns.Add("SiteID", GetType(Int32))
            For i As Integer = 1 To Tbl.Rows.Count
                Tbl.Rows(i - 1).Item(6) = i
            Next

            If Not (Tbl.Columns(0).ColumnName.ToUpper() = "VAN ID" And Tbl.Columns(1).ColumnName.ToUpper() = "CUSTOMER ID" And Tbl.Columns(2).ColumnName.ToUpper() = "SITEID" And Tbl.Columns(3).ColumnName.ToUpper() = "DATE") Then
                Str.Append("<br/><span style='font-color:red'> <b >Invalid Coulmn Headers</b></span>")
                Dim ErrorDr As DataRow
                ErrorDr = dtError.NewRow
                ErrorDr(0) = "Invalid Coulmn Headers"
                dtError.Rows.Add(ErrorDr)
            Else

                Dim RowCount As Integer = 1
                For Each dr As DataRow In Tbl.Rows
                    RowCount += 1
                    Try
                        If Not dr.Item(0) Is DBNull.Value Then
                            Dim HasNull As Boolean = False

                            If dr.Item(0) Is DBNull.Value Then
                                HasNull = True
                                Session("DoneValidation") = False
                                Str.Append("<br/><span style='font-color:red'> <b >Invalid FSR Id is at Row No: " & RowCount & " </b></span>")
                                Dim ErrorDr As DataRow
                                ErrorDr = dtError.NewRow
                                ErrorDr(0) = "Invalid FSR Id is at Row No: " & RowCount
                                dtError.Rows.Add(ErrorDr)
                            End If


                            If dr.Item(1) Is DBNull.Value Then
                                HasNull = True
                                Session("DoneValidation") = False
                                Str.Append("<br/><span style='font-color:red'> <b >Customer Id is Null at Row No: " & RowCount & " , Van ID : " & dr.Item(0).ToString & " , Customer No : " & dr.Item(1).ToString & ", Site ID :" & dr.Item(2).ToString & " </b></span>")
                                Dim ErrorDr As DataRow
                                ErrorDr = dtError.NewRow
                                ErrorDr(0) = "Customer Id is Null at Row No: " & RowCount & " , Van ID : " & dr.Item(0).ToString & " , Customer No : " & dr.Item(1).ToString & ", Site ID :" & dr.Item(2).ToString & ""
                                dtError.Rows.Add(ErrorDr)
                            End If

                            If dr.Item(2) Is DBNull.Value Then
                                HasNull = True
                                Session("DoneValidation") = False
                                Str.Append("<br/><span style='font-color:red'> <b >Site Use Id is Null at Row No: " & RowCount & " , Van ID : " & dr.Item(0).ToString & " , Customer No : " & dr.Item(1).ToString & ", Site ID :" & dr.Item(2).ToString & " </b></span>")
                                Dim ErrorDr As DataRow
                                ErrorDr = dtError.NewRow
                                ErrorDr(0) = "Site Use Id is Null at Row No: " & RowCount & " , Van ID : " & dr.Item(0).ToString & " , Customer No : " & dr.Item(1).ToString & ", Site ID :" & dr.Item(2).ToString & ""
                                dtError.Rows.Add(ErrorDr)
                            End If

                            '

                            If dr.Item(3) Is DBNull.Value Then
                                HasNull = True
                                Session("DoneValidation") = False
                                Str.Append("<br/><span style='font-color:red'> <b >Date is Null at Row No: " & RowCount & " , Van ID : " & dr.Item(0).ToString & " , Customer No : " & dr.Item(1).ToString & ", Site ID :" & dr.Item(2).ToString & " </b></span>")
                                Dim ErrorDr As DataRow
                                ErrorDr = dtError.NewRow
                                ErrorDr(0) = "Date is Null at Row No: " & RowCount & " , Van ID : " & dr.Item(0).ToString & " , Customer No : " & dr.Item(1).ToString & ", Site ID :" & dr.Item(2).ToString & ""
                                dtError.Rows.Add(ErrorDr)
                            End If
 
                            If Not HasNull Then

                                Dim Dat As DateTime

                                ''Check having valid date
                                Try
                                    Dat = dr.Item(3)
                                Catch ex As Exception
                                    Session("DoneValidation") = False
                                    Str.Append("<br/><span style='font-color:red'> <b >Invalid date at Row No: " & RowCount & " , Van ID : " & dr.Item(0).ToString & " , Customer No : " & dr.Item(1).ToString & ", Site ID :" & dr.Item(2).ToString & " </b></span>")
                                    Dim ErrorDr As DataRow
                                    ErrorDr = dtError.NewRow
                                    ErrorDr(0) = "Invalid date at Row No: " & RowCount & " , Van ID : " & dr.Item(0).ToString & " , Customer No : " & dr.Item(1).ToString & ", Site ID :" & dr.Item(2).ToString & ""
                                    dtError.Rows.Add(ErrorDr)
                                End Try


                                If Dat <> "#12:00:00 AM#" Then

                                    If (Not Dat.Month < Now.Month) OrElse (Dat.Year > Now.Year) Then
                                        'If Not Dat < Now.Date Then

                                        ''Check Default Plan Exist
                                        If (objRoutePlan.DefaultPlanExist(Dat, dr.Item(0), Err_No, Err_Desc)) Then

                                            ''Check Having Valid VaniD and Customer ID


                                            Err_No = Nothing
                                            Err_Desc = Nothing

                                            Dim Site_ID As Integer = 0
                                            Dim FSRExist As Boolean = False
                                            Site_ID = objRoutePlan.CheckVanNCustomer(dr.Item(0), dr.Item(1), dr.Item(2), Err_No, Err_Desc)

                                            If Site_ID <> 0 Then
                                                dr("SiteID") = Site_ID

                                                'Dim dtTbl As New DataTable
                                                'Dim objProduct As New Product
                                                'Dim objCommon As New Common
                                                'Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                                                'dtTbl = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
                                                'If dtTbl IsNot Nothing Then
                                                '    If Tbl.Rows.Count > 0 Then
                                                '        For Each r As DataRow In dtTbl.Rows
                                                '            If Trim(r("Salesrep_Number").ToString()) = dr.Item(0) Then
                                                '                FSRExist = True
                                                '            End If
                                                '        Next
                                                '    End If
                                                'End If

                                                'If FSRExist = False Then
                                                '    Str.Append("<span style='font-color:red'> <b >The user does not have link with FSR " & dr.Item(0).ToString() & "</b></span>")
                                                '    lblResult.Text = Str.ToString()
                                                '    Exit Function
                                                'End If

                                                If (objRoutePlan.CheckValidFSR(dr.Item(0), CType(Session("User_Access"), UserAccess).UserID, Err_No, Err_Desc)) = False Then
                                                    Str.Append("<br/><span style='font-color:red'> <b >The login user do not have link with the FSR at Row No : " & RowCount & " , Van ID : " & dr.Item(0).ToString & " </b></span>")
                                                    Dim ErrorDr As DataRow
                                                    ErrorDr = dtError.NewRow
                                                    ErrorDr(0) = "The login user do not have link with the FSR at Row No : " & RowCount & " , Van ID : " & dr.Item(0).ToString & " , Customer No : " & dr.Item(1).ToString & ", Site ID :" & dr.Item(2).ToString & ""
                                                    dtError.Rows.Add(ErrorDr)
                                                    Exit Function
                                                End If


                                                ''Check Route Plan Exist
                                                Err_No = Nothing
                                                Err_Desc = Nothing

                                                Dim Result As String
                                                Result = objRoutePlan.RoutePlanExist(Dat, dr.Item(0), Err_No, Err_Desc)

                                                dr("Status") = Result

                                                'If Result.StartsWith("HAVECURRENTMONTH") Then
                                                '    ConfirmStr.Append("<br/><span style='font-color:red'> <b >Row No : " & RowCount & " Current Month Data will be ignored</b></span>" & "")
                                                '    Dim ErrorDr As DataRow
                                                '    ErrorDr = dtError.NewRow
                                                '    ErrorDr(0) = "Row No : " & RowCount & " Current Month Data will be ignored"
                                                '    dtError.Rows.Add(ErrorDr)

                                                'ElseIf Result.StartsWith("HAVEFUTUREMONTH") Then
                                                '    ConfirmStr.Append("<br/><span style='font-color:red'> <b >Row No : " & RowCount & " Route Plan already exists for given month.</b></span>" & "")
                                                '    Dim ErrorDr As DataRow
                                                '    ErrorDr = dtError.NewRow
                                                '    ErrorDr(0) = "Row No : " & RowCount & " Route Plan already exists for given month."
                                                '    dtError.Rows.Add(ErrorDr)
                                                'End If


                                                If Result.StartsWith("NOROUTEPLAN") Or Result.StartsWith("HAVEFUTUREMONTH") Or Result.StartsWith("HAVECURRENTMONTH") Then


                                                    Dim ResArray As Array = Result.Split("_")
                                                    If ResArray.Length = 2 Then
                                                        dr("DefaultPlanID") = ResArray(1)
                                                    End If

                                                    ''Validating Start Time and End Time

                                                    'If Not (dr.Item(3) Is DBNull.Value And dr.Item(4) Is DBNull.Value) Then

                                                    '    Dim STime As DateTime = Nothing
                                                    '    Dim ETime As DateTime = Nothing

                                                    '    Try
                                                    '        STime = dr.Item(3)
                                                    '    Catch ex As Exception
                                                    '        log.Error(GetExceptionInfo(ex) & " " & STime)
                                                    '        Session("DoneValidation") = False
                                                    '        Str.Append("<br/><span style='font-color:red'> <b >Invalid Start Time at Row No: " & RowCount & " </b></span>")
                                                    '    End Try


                                                    '    Try
                                                    '        ETime = dr.Item(4)
                                                    '    Catch ex As Exception
                                                    '        log.Error(GetExceptionInfo(ex) & " " & STime)
                                                    '        Session("DoneValidation") = False
                                                    '        Str.Append("<br/><span style='font-color:red'> <b >Invalid End Time at Row No: " & RowCount & " </b></span>")
                                                    '    End Try

                                                    '    If ETime <> "#12:00:00 AM#" And STime <> "#12:00:00 AM#" Then
                                                    '        Dim ParentSDate, ParentEDate As DateTime
                                                    '        ParentSDate = CDate("1/1/1900 " & STime.Hour & ":" & STime.Minute)
                                                    '        ParentEDate = CDate("1/1/1900 " & ETime.Hour & ":" & ETime.Minute)
                                                    '        'If (dr.Item(3) > dr.Item(4)) _
                                                    '        '                 Or (dr.Item(3) = dr.Item(4)) Then
                                                    '        If (ParentSDate > ParentEDate) _
                                                    '                       Or (ParentSDate = ParentEDate) Then
                                                    '            Session("DoneValidation") = False
                                                    '            Str.Append("<br/><span style='font-color:red'> <b >Start Time should be less than End Time at Row No: " & RowCount & " </b></span>")
                                                    '        Else
                                                    '            Dim expr As String = "[Van ID]=" & dr.Item(0) & " and Date='" & dr.Item(2) & "' and ID <> " & RowCount - 1
                                                    '            Dim foundRows() As DataRow = Tbl.Select(expr)
                                                    '            If foundRows.Length > 0 Then
                                                    '                For j As Integer = 0 To foundRows.Length - 1
                                                    '                    If Not (foundRows(j).Item(3) Is DBNull.Value And foundRows(j).Item(4) Is DBNull.Value) Then

                                                    '                        Dim CSTime As DateTime = Nothing
                                                    '                        Dim CETime As DateTime = Nothing

                                                    '                        Try
                                                    '                            CSTime = foundRows(j).Item(3)
                                                    '                        Catch ex As Exception
                                                    '                            CSTime = CDate("12/30/1899 " & CSTime.Hour & ":" & CSTime.Minute)
                                                    '                            log.Error(GetExceptionInfo(ex) & " " & foundRows(j).Item(3))
                                                    '                        End Try

                                                    '                        Try
                                                    '                            CETime = foundRows(j).Item(4)
                                                    '                        Catch ex As Exception
                                                    '                            log.Error(GetExceptionInfo(ex) & " " & foundRows(j).Item(4))
                                                    '                        End Try

                                                    '                        If CETime <> "#12:00:00 AM#" And CSTime <> "#12:00:00 AM#" Then
                                                    '                            Dim ChildStartDate, ChildEndDate As DateTime

                                                    '                            ChildStartDate = CDate("1/1/1900 " & CSTime.Hour & ":" & CSTime.Minute)
                                                    '                            ChildEndDate = CDate("1/1/1900 " & CETime.Hour & ":" & CETime.Minute)
                                                    '                            'If ((dr.Item(3) < foundRows(j).Item(3) And dr.Item(4) > foundRows(j).Item(3)) Or _
                                                    '                            '        (dr.Item(3) < foundRows(j).Item(4) And dr.Item(4) > foundRows(j).Item(4))) Then
                                                    '                            If ((ParentSDate < ChildStartDate And ParentEDate > ChildStartDate) Or _
                                                    '                                                                     (ParentSDate < ChildEndDate And ParentEDate > ChildEndDate)) Then
                                                    '                                Session("DoneValidation") = False
                                                    '                                Str.Append("<br/><span style='font-color:red'> <b >Row No: " & RowCount & " Overlapping another scheduled time. </b></span>")
                                                    '                            End If
                                                    '                        End If
                                                    '                    End If
                                                    '                Next
                                                    '            End If
                                                    '        End If
                                                    '    End If
                                                    'End If
                                                ElseIf Result = "" Then
                                                    If Err_Desc IsNot Nothing Then log.Error(Err_Desc & " at Row : " & RowCount)
                                                ElseIf Result = "NODEFAULT" Then
                                                    Session("DoneValidation") = False
                                                    Str.Append("<br/><span style='font-color:red'> <b >There is no default plan for van : " & dr.Item(0) & "  at row :" & RowCount & " , Van ID : " & dr.Item(0).ToString & " , Customer No : " & dr.Item(1).ToString & ", Site ID :" & dr.Item(2).ToString & " </b></span>")
                                                    Dim ErrorDr As DataRow
                                                    ErrorDr = dtError.NewRow
                                                    ErrorDr(0) = "There is no default plan for at Row No: " & RowCount & " , van : " & dr.Item(0) & " , Customer No : " & dr.Item(1).ToString & ", Site ID :" & dr.Item(2).ToString & ""
                                                    dtError.Rows.Add(ErrorDr)
                                                    Exit For
                                                End If
                                            Else
                                                If Err_Desc IsNot Nothing Then log.Error(Err_Desc & " at Row : " & RowCount)
                                                Session("DoneValidation") = False
                                                Str.Append("<br/><span style='font-color:red'> <b >FSR ID , Customer ID and Site ID is not matching at Row No: " & RowCount & " , Van ID : " & dr.Item(0).ToString & " , Customer No : " & dr.Item(1).ToString & ", Site ID :" & dr.Item(2).ToString & " </b></span>")
                                                Dim ErrorDr As DataRow
                                                ErrorDr = dtError.NewRow
                                                ErrorDr(0) = "FSR ID , Customer ID and Site ID is not matching at Row No: " & RowCount & " , Van ID : " & dr.Item(0).ToString & " , Customer No : " & dr.Item(1).ToString & ", Site ID : " & dr.Item(2).ToString
                                                dtError.Rows.Add(ErrorDr)
                                            End If
                                        Else
                                            If Err_Desc IsNot Nothing Then log.Error(Err_Desc & " at Row : " & RowCount)
                                            Str.Append("<br/><span style='font-color:red'> <b >There is no Default Plan for given date at Row No: " & RowCount & " , Van ID : " & dr.Item(0).ToString & " , Customer No : " & dr.Item(1).ToString & ", Site ID : " & dr.Item(2).ToString & "  </b></span>")
                                            Dim ErrorDr As DataRow
                                            ErrorDr = dtError.NewRow
                                            ErrorDr(0) = "There is no Default Plan for given date at Row No: " & RowCount & " , Van ID : " & dr.Item(0).ToString & " , Customer No : " & dr.Item(1).ToString & ", Site ID : " & dr.Item(2).ToString
                                            dtError.Rows.Add(ErrorDr)
                                            Session("DoneValidation") = False
                                        End If
                                    Else
                                        If Err_Desc IsNot Nothing Then log.Error(Err_Desc & " at Row : " & RowCount)
                                        Str.Append("<br/><span style='font-color:red'> <b >Route Plan Should be for Future Months. Error at Row No: " & RowCount & " , Van ID : " & dr.Item(0).ToString & " , Customer No : " & dr.Item(1).ToString & ", Site ID : " & dr.Item(2).ToString & "</b></span>")
                                        Dim ErrorDr As DataRow
                                        ErrorDr = dtError.NewRow
                                        ErrorDr(0) = "Route Plan Should be for Future Months. Error at Row No: " & RowCount & " , Van ID : " & dr.Item(0).ToString & " , Customer No : " & dr.Item(1).ToString & ", Site ID : " & dr.Item(2).ToString
                                        dtError.Rows.Add(ErrorDr)
                                        Session("DoneValidation") = False
                                    End If
                                End If
                            End If
                        End If
                    Catch ex As Exception
                        log.Error(GetExceptionInfo(ex))
                        Response.Redirect("information.aspx?mode=1&errno=" & "74089" & "&msg=" & SalesWorx.BO.Common.AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=ImportRoutePlan.aspx&Title=Route+Planner", False)
                    End Try
                Next
            End If
            Session("Errordt") = dtError.Copy
            If Str.ToString() <> "" Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("information.aspx?mode=1&errno=" & "74090" & "&msg=" & SalesWorx.BO.Common.AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=ImportRoutePlan.aspx&Title=Route+Planner", False)
            Throw ex
        End Try
    End Function

    Private Function DoValidationForSplitedColumn(ByVal Tbl As DataTable, ByVal Str As StringBuilder, ByRef ConfirmStr As StringBuilder) As Boolean
        Try
            Session("Errordt") = Nothing
            Dim dtError As New DataTable
            dtError.Columns.Add("Error")

            ''Adding identity Column in Datatable
           
            ' Tbl.Columns.Add("SiteID", GetType(Int32))
            For i As Integer = 1 To Tbl.Rows.Count
                Tbl.Rows(i - 1).Item(6) = i
            Next

            Dim RowCount As Integer = 1

            If Not (Tbl.Columns(0).ColumnName.ToUpper() = "VAN ID" And Tbl.Columns(1).ColumnName.ToUpper() = "CUSTOMER ID" And Tbl.Columns(2).ColumnName.ToUpper() = "SITEID" And Tbl.Columns(3).ColumnName.ToUpper() = "DAY" And Tbl.Columns(4).ColumnName.ToUpper() = "MONTH" And Tbl.Columns(5).ColumnName.ToUpper() = "YEAR") Then
                Str.Append("<br/><span style='font-color:red'> <b >Invalid Coulmn Headers</b></span>")
                Dim ErrorDr As DataRow
                ErrorDr = dtError.NewRow
                ErrorDr(0) = "Invalid Coulmn Headers"
                dtError.Rows.Add(ErrorDr)
            Else

                For Each dr As DataRow In Tbl.Rows
                    RowCount += 1
                    Try
                        If Not dr.Item(0) Is DBNull.Value Then
                            Dim HasNull As Boolean = False

                            If dr.Item(0) Is DBNull.Value Then
                                HasNull = True
                                Session("DoneValidation") = False
                                Str.Append("<br/><span style='font-color:red'> <b >Invalid FSR Id is at Row No: " & RowCount & " </b></span>")
                                Dim ErrorDr As DataRow
                                ErrorDr = dtError.NewRow
                                ErrorDr(0) = "Invalid FSR Id is at Row No: " & RowCount
                                dtError.Rows.Add(ErrorDr)
                            End If


                            If dr.Item(1) Is DBNull.Value Then
                                HasNull = True
                                Session("DoneValidation") = False
                                Str.Append("<br/><span style='font-color:red'> <b >Customer Id is Null at Row No: " & RowCount & " </b></span>")
                                Dim ErrorDr As DataRow
                                ErrorDr = dtError.NewRow
                                ErrorDr(0) = "Customer Id is Null at Row No: " & RowCount
                                dtError.Rows.Add(ErrorDr)
                            End If

                            If dr.Item(2) Is DBNull.Value Then
                                HasNull = True
                                Session("DoneValidation") = False
                                Str.Append("<br/><span style='font-color:red'> <b >Site Use Id is Null at Row No: " & RowCount & " </b></span>")
                                Dim ErrorDr As DataRow
                                ErrorDr = dtError.NewRow
                                ErrorDr(0) = "Site Use Id is Null at Row No: " & RowCount
                                dtError.Rows.Add(ErrorDr)
                            End If

                            '

                            If dr.Item(3) Is DBNull.Value Then
                                HasNull = True
                                Session("DoneValidation") = False
                                Str.Append("<br/><span style='font-color:red'> <b >Date is Null at Row No: " & RowCount & " </b></span>")
                                Dim ErrorDr As DataRow
                                ErrorDr = dtError.NewRow
                                ErrorDr(0) = "Date is Null at Row No: " & RowCount
                                dtError.Rows.Add(ErrorDr)
                            End If

                            If dr.Item(4) Is DBNull.Value Then
                                HasNull = True
                                Session("DoneValidation") = False
                                Str.Append("<br/><span style='font-color:red'> <b >Month is Null at Row No: " & RowCount & " </b></span>")
                                Dim ErrorDr As DataRow
                                ErrorDr = dtError.NewRow
                                ErrorDr(0) = "Month is Null at Row No: " & RowCount
                                dtError.Rows.Add(ErrorDr)
                            End If


                            If dr.Item(5) Is DBNull.Value Then
                                HasNull = True
                                Session("DoneValidation") = False
                                Str.Append("<br/><span style='font-color:red'> <b >Year is Null at Row No: " & RowCount & " </b></span>")
                                Dim ErrorDr As DataRow
                                ErrorDr = dtError.NewRow
                                ErrorDr(0) = "Year is Null at Row No: " & RowCount
                                dtError.Rows.Add(ErrorDr)
                            End If

                            'If dr.Item(3) Is DBNull.Value Then
                            '    HasNull = True
                            '    Str.Append("<br/><span style='font-color:red'> <b >Start Time is Null at Row No: " & RowCount & " </b></span>")
                            'End If


                            'If dr.Item(4) Is DBNull.Value Then
                            '    HasNull = True
                            '    Str.Append("<br/><span style='font-color:red'> <b >End Time is Null at Row No: " & RowCount & " </b></span>")
                            'End If

                            If Not HasNull Then
                                Dim tdate As String

                                tdate = dr.Item(4) & "/" & dr.Item(3) & "/" & dr.Item(5)
                                Dim Dat As DateTime

                                ''Check having valid date
                                Try
                                    Dat = tdate
                                Catch ex As Exception
                                    Session("DoneValidation") = False
                                    Str.Append("<br/><span style='font-color:red'> <b >Invalid day/month/year at Row No: " & RowCount & " </b></span>")
                                    Dim ErrorDr As DataRow
                                    ErrorDr = dtError.NewRow
                                    ErrorDr(0) = "Invalid day/month/year at Row No: " & RowCount
                                    dtError.Rows.Add(ErrorDr)
                                End Try


                                If Dat <> "#12:00:00 AM#" Then

                                    If (Not Dat.Month < Now.Month) OrElse (Dat.Year > Now.Year) Then
                                        'If Not Dat < Now.Date Then

                                        ''Check Default Plan Exist
                                        If (objRoutePlan.DefaultPlanExist(Dat, dr.Item(0), Err_No, Err_Desc)) Then

                                            ''Check Having Valid VaniD and Customer ID


                                            Err_No = Nothing
                                            Err_Desc = Nothing

                                            Dim Site_ID As Integer = 0
                                            Dim FSRExist As Boolean = False
                                            Site_ID = objRoutePlan.CheckVanNCustomer(dr.Item(0), dr.Item(1), dr.Item(2), Err_No, Err_Desc)

                                            If Site_ID <> 0 Then
                                                dr("SiteID") = Site_ID

                                                'Dim dtTbl As New DataTable
                                                'Dim objProduct As New Product
                                                'Dim objCommon As New Common
                                                'Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                                                'dtTbl = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
                                                'If dtTbl IsNot Nothing Then
                                                '    If Tbl.Rows.Count > 0 Then
                                                '        For Each r As DataRow In dtTbl.Rows
                                                '            If Trim(r("Salesrep_Number").ToString()) = dr.Item(0) Then
                                                '                FSRExist = True
                                                '            End If
                                                '        Next
                                                '    End If
                                                'End If

                                                'If FSRExist = False Then
                                                '    Str.Append("<span style='font-color:red'> <b >The user does not have link with FSR " & dr.Item(0).ToString() & "</b></span>")
                                                '    lblResult.Text = Str.ToString()
                                                '    Exit Function
                                                'End If

                                                If (objRoutePlan.CheckValidFSR(dr.Item(0), CType(Session("User_Access"), UserAccess).UserID, Err_No, Err_Desc)) = False Then
                                                    Str.Append("<br/><span style='font-color:red'> <b >The login user do not have link with the FSR at Row No : " & RowCount & " , Van ID : " & dr.Item(0).ToString & " </b></span>")
                                                    Dim ErrorDr As DataRow
                                                    ErrorDr = dtError.NewRow
                                                    ErrorDr(0) = "The login user do not have link with the FSR at Row No : " & RowCount & " , Van ID : " & dr.Item(0).ToString & " "
                                                    dtError.Rows.Add(ErrorDr)
                                                    Exit Function
                                                End If


                                                ''Check Route Plan Exist
                                                Err_No = Nothing
                                                Err_Desc = Nothing

                                                Dim Result As String
                                                Result = objRoutePlan.RoutePlanExist(Dat, dr.Item(0), Err_No, Err_Desc)

                                                dr("Status") = Result

                                                'If Result.StartsWith("HAVECURRENTMONTH") Then
                                                '    ConfirmStr.Append("<br/><span style='font-color:red'> <b >Row No : " & RowCount & " Current Month Data will be ignored</b></span>" & "")
                                                '    Dim ErrorDr As DataRow
                                                '    ErrorDr = dtError.NewRow
                                                '    ErrorDr(0) = "Row No : " & RowCount & " Current Month Data will be ignored"
                                                '    dtError.Rows.Add(ErrorDr)
                                                'ElseIf Result.StartsWith("HAVEFUTUREMONTH") Then
                                                '    ConfirmStr.Append("<br/><span style='font-color:red'> <b >Row No : " & RowCount & " Route Plan already exists for given month.</b></span>" & "")
                                                '    Dim ErrorDr As DataRow
                                                '    ErrorDr = dtError.NewRow
                                                '    ErrorDr(0) = "Row No : " & RowCount & " Route Plan already exists for given month."
                                                '    dtError.Rows.Add(ErrorDr)
                                                'End If


                                                If Result.StartsWith("NOROUTEPLAN") Or Result.StartsWith("HAVEFUTUREMONTH") Or Result.StartsWith("HAVECURRENTMONTH") Then

                                                    Dim ResArray As Array = Result.Split("_")
                                                    If ResArray.Length = 2 Then
                                                        dr("DefaultPlanID") = ResArray(1)
                                                    End If

                                                    ''Validating Start Time and End Time

                                                    'If Not (dr.Item(3) Is DBNull.Value And dr.Item(4) Is DBNull.Value) Then

                                                    '    Dim STime As DateTime = Nothing
                                                    '    Dim ETime As DateTime = Nothing

                                                    '    Try
                                                    '        STime = dr.Item(3)
                                                    '    Catch ex As Exception
                                                    '        log.Error(GetExceptionInfo(ex) & " " & STime)
                                                    '        Session("DoneValidation") = False
                                                    '        Str.Append("<br/><span style='font-color:red'> <b >Invalid Start Time at Row No: " & RowCount & " </b></span>")
                                                    '    End Try


                                                    '    Try
                                                    '        ETime = dr.Item(4)
                                                    '    Catch ex As Exception
                                                    '        log.Error(GetExceptionInfo(ex) & " " & STime)
                                                    '        Session("DoneValidation") = False
                                                    '        Str.Append("<br/><span style='font-color:red'> <b >Invalid End Time at Row No: " & RowCount & " </b></span>")
                                                    '    End Try

                                                    '    If ETime <> "#12:00:00 AM#" And STime <> "#12:00:00 AM#" Then
                                                    '        Dim ParentSDate, ParentEDate As DateTime
                                                    '        ParentSDate = CDate("1/1/1900 " & STime.Hour & ":" & STime.Minute)
                                                    '        ParentEDate = CDate("1/1/1900 " & ETime.Hour & ":" & ETime.Minute)
                                                    '        'If (dr.Item(3) > dr.Item(4)) _
                                                    '        '                 Or (dr.Item(3) = dr.Item(4)) Then
                                                    '        If (ParentSDate > ParentEDate) _
                                                    '                       Or (ParentSDate = ParentEDate) Then
                                                    '            Session("DoneValidation") = False
                                                    '            Str.Append("<br/><span style='font-color:red'> <b >Start Time should be less than End Time at Row No: " & RowCount & " </b></span>")
                                                    '        Else
                                                    '            Dim expr As String = "[Van ID]=" & dr.Item(0) & " and Date='" & dr.Item(2) & "' and ID <> " & RowCount - 1
                                                    '            Dim foundRows() As DataRow = Tbl.Select(expr)
                                                    '            If foundRows.Length > 0 Then
                                                    '                For j As Integer = 0 To foundRows.Length - 1
                                                    '                    If Not (foundRows(j).Item(3) Is DBNull.Value And foundRows(j).Item(4) Is DBNull.Value) Then

                                                    '                        Dim CSTime As DateTime = Nothing
                                                    '                        Dim CETime As DateTime = Nothing

                                                    '                        Try
                                                    '                            CSTime = foundRows(j).Item(3)
                                                    '                        Catch ex As Exception
                                                    '                            CSTime = CDate("12/30/1899 " & CSTime.Hour & ":" & CSTime.Minute)
                                                    '                            log.Error(GetExceptionInfo(ex) & " " & foundRows(j).Item(3))
                                                    '                        End Try

                                                    '                        Try
                                                    '                            CETime = foundRows(j).Item(4)
                                                    '                        Catch ex As Exception
                                                    '                            log.Error(GetExceptionInfo(ex) & " " & foundRows(j).Item(4))
                                                    '                        End Try

                                                    '                        If CETime <> "#12:00:00 AM#" And CSTime <> "#12:00:00 AM#" Then
                                                    '                            Dim ChildStartDate, ChildEndDate As DateTime

                                                    '                            ChildStartDate = CDate("1/1/1900 " & CSTime.Hour & ":" & CSTime.Minute)
                                                    '                            ChildEndDate = CDate("1/1/1900 " & CETime.Hour & ":" & CETime.Minute)
                                                    '                            'If ((dr.Item(3) < foundRows(j).Item(3) And dr.Item(4) > foundRows(j).Item(3)) Or _
                                                    '                            '        (dr.Item(3) < foundRows(j).Item(4) And dr.Item(4) > foundRows(j).Item(4))) Then
                                                    '                            If ((ParentSDate < ChildStartDate And ParentEDate > ChildStartDate) Or _
                                                    '                                                                     (ParentSDate < ChildEndDate And ParentEDate > ChildEndDate)) Then
                                                    '                                Session("DoneValidation") = False
                                                    '                                Str.Append("<br/><span style='font-color:red'> <b >Row No: " & RowCount & " Overlapping another scheduled time. </b></span>")
                                                    '                            End If
                                                    '                        End If
                                                    '                    End If
                                                    '                Next
                                                    '            End If
                                                    '        End If
                                                    '    End If
                                                    'End If
                                                ElseIf Result = "" Then
                                                    If Err_Desc IsNot Nothing Then log.Error(Err_Desc & " at Row : " & RowCount & " , Van ID : " & dr.Item(0).ToString)
                                                ElseIf Result = "NODEFAULT" Then
                                                    Session("DoneValidation") = False
                                                    Str.Append("<br/><span style='font-color:red'> <b >There is no default plan for van : " & dr.Item(0) & " </b></span>")
                                                    Dim ErrorDr As DataRow
                                                    ErrorDr = dtError.NewRow
                                                    ErrorDr(0) = "There is no default plan for van : " & dr.Item(0) & " at row: " & RowCount
                                                    dtError.Rows.Add(ErrorDr)
                                                    Exit For
                                                End If
                                            Else
                                                If Err_Desc IsNot Nothing Then log.Error(Err_Desc & " at Row : " & RowCount & " , Van ID : " & dr.Item(0).ToString)
                                                Session("DoneValidation") = False
                                                Str.Append("<br/><span style='font-color:red'> <b >FSR ID , Customer ID and Site ID is not matching at Row No: " & RowCount & " , Van ID : " & dr.Item(0).ToString & " , Customer No : " & dr.Item(1).ToString & ", Site ID :" & dr.Item(2).ToString & " </b></span>")
                                                Dim ErrorDr As DataRow
                                                ErrorDr = dtError.NewRow
                                                ErrorDr(0) = "FSR ID , Customer ID and Site ID is not matching at Row No: " & RowCount & " , Van ID : " & dr.Item(0).ToString & " , Customer No : " & dr.Item(1).ToString & ", Site ID :" & dr.Item(2).ToString
                                                dtError.Rows.Add(ErrorDr)
                                            End If
                                        Else
                                            If Err_Desc IsNot Nothing Then log.Error(Err_Desc & " at Row : " & RowCount)
                                            Str.Append("<br/><span style='font-color:red'> <b >There is no Default Plan for given date at Row No: " & RowCount & " , Van ID : " & dr.Item(0).ToString & " , Customer No : " & dr.Item(1).ToString & ", Site ID :" & dr.Item(2).ToString & " </b></span>")
                                            Dim ErrorDr As DataRow
                                            ErrorDr = dtError.NewRow
                                            ErrorDr(0) = "There is no Default Plan for given date at Row No: " & RowCount & " , Van ID : " & dr.Item(0).ToString & " , Customer No : " & dr.Item(1).ToString & ", Site ID :" & dr.Item(2).ToString
                                            dtError.Rows.Add(ErrorDr)
                                            Session("DoneValidation") = False
                                        End If
                                    Else
                                        If Err_Desc IsNot Nothing Then log.Error(Err_Desc & " at Row : " & RowCount)
                                        Str.Append("<br/><span style='font-color:red'> <b >Route Plan Should be for Future Months. Error at Row No: " & RowCount & " , Van ID : " & dr.Item(0).ToString & " , Customer No : " & dr.Item(1).ToString & ", Site ID :" & dr.Item(2).ToString & " </b></span>")
                                        Dim ErrorDr As DataRow
                                        ErrorDr = dtError.NewRow
                                        ErrorDr(0) = "Route Plan Should be for Future Months. Error at Row No: " & RowCount & " , Van ID : " & dr.Item(0).ToString & " , Customer No : " & dr.Item(1).ToString & ", Site ID :" & dr.Item(2).ToString
                                        dtError.Rows.Add(ErrorDr)
                                        Session("DoneValidation") = False
                                    End If
                                End If
                            End If
                        End If
                    Catch ex As Exception
                        log.Error(GetExceptionInfo(ex))
                        Response.Redirect("information.aspx?mode=1&errno=" & "74089" & "&msg=" & SalesWorx.BO.Common.AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=ImportRoutePlan.aspx&Title=Route+Planner", False)
                    End Try
                Next
            End If
            Session("Errordt") = dtError.Copy
            If Str.ToString() <> "" Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("information.aspx?mode=1&errno=" & "74090" & "&msg=" & SalesWorx.BO.Common.AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=ImportRoutePlan.aspx&Title=Route+Planner", False)
            Throw ex
        End Try
    End Function
    Private Sub DoCSVUpload(ByRef Tbl As DataTable)
        Try
            Dim strConString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "\;Extended Properties=""text;HDR=Yes;FMT=Delimited"""
            Dim oledbConn As New OleDbConnection(strConString)

            Dim cmd As New OleDbCommand("SELECT * FROM [" & ViewState("CSVName") & "]", oledbConn)

            Dim oleda As New OleDbDataAdapter()

            oleda.SelectCommand = cmd

            Dim ds As New DataSet()

            oleda.Fill(ds, "RoutePlan")

            Tbl = ds.Tables(0)
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        End Try

    End Sub
    Private Sub DoXLSUpload(ByRef Tbl As DataTable)
        Try
            Dim connString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & ViewState("FileName") & " ;Extended Properties=""Excel 8.0;HDR=Yes;IMEX=1"""
            Dim oledbConn As New OleDbConnection(connString)
            Try
                oledbConn.Open()

                Dim cmd As New OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn)

                Dim oleda As New OleDbDataAdapter()

                oleda.SelectCommand = cmd

                Dim ds As New DataSet()

                oleda.Fill(ds, "RoutePlan")

                Tbl = ds.Tables(0)

            Catch ex As Exception
                log.Error(GetExceptionInfo(ex))
            Finally
                ' Close connection
                oledbConn.Close()
            End Try
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        End Try
    End Sub
    Private Sub DeleteExcel()
        Try
            Dim TheFile As FileInfo = New FileInfo(ViewState("FileName"))
            If TheFile.Exists Then
                File.Delete(ViewState("FileName"))
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))

        End Try
    End Sub
    Private Sub DoXLSXUpload(ByRef Tbl As DataTable)
        Try
            Dim connString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & ViewState("FileName") & " ;Extended Properties=""Excel 12.0;HDR=Yes;IMEX=1"""
            Dim oledbConn As New OleDbConnection(connString)
            Try
                oledbConn.Open()

                Dim cmd As New OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn)

                Dim oleda As New OleDbDataAdapter()

                oleda.SelectCommand = cmd

                Dim ds As New DataSet()

                oleda.Fill(ds, "RoutePlan")

                Tbl = ds.Tables(0)

            Catch ex As Exception
                log.Error(GetExceptionInfo(ex))
            Finally
                ' Close connection
                oledbConn.Close()
            End Try
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        End Try
    End Sub
    Private Shared Sub setLastValues(ByVal lastValues() As Object, ByVal sourceRow As DataRow, ByVal fieldNames() As String)
        For i As Integer = 0 To fieldNames.Length - 1
            lastValues(i) = sourceRow(fieldNames(i))
        Next
    End Sub
    Private Shared Function createRowClone(ByVal sourceRow As DataRow, ByVal newRow As DataRow, ByVal fieldNames() As String) As DataRow
        For Each field As String In fieldNames
            newRow(field) = sourceRow(field)
        Next

        Return newRow
    End Function
    Private Shared Function fieldValuesAreEqual(ByVal lastValues() As Object, ByVal currentRow As DataRow, ByVal fieldNames() As String) As Boolean
        Dim areEqual As Boolean = True

        For i As Integer = 0 To fieldNames.Length - 1
            If lastValues(i) Is Nothing OrElse Not lastValues(i).Equals(currentRow(fieldNames(i))) Then
                areEqual = False
                Exit For
            End If
        Next

        Return areEqual
    End Function

    Public Shared Function SelectDistinct(ByVal SourceTable As DataTable, ByVal ParamArray FieldNames() As String) As DataTable
        Try
            Dim lastValues() As Object
            Dim newTable As DataTable

            If FieldNames Is Nothing OrElse FieldNames.Length = 0 Then
                Throw New ArgumentNullException("FieldNames")
            End If

            lastValues = New Object(FieldNames.Length - 1) {}
            newTable = New DataTable

            For Each field As String In FieldNames
                newTable.Columns.Add(field, SourceTable.Columns(field).DataType)
            Next

            For Each Row As DataRow In SourceTable.Select("", String.Join(", ", FieldNames))
                If Not fieldValuesAreEqual(lastValues, Row, FieldNames) Then
                    newTable.Rows.Add(createRowClone(Row, newTable.NewRow(), FieldNames))

                    setLastValues(lastValues, Row, FieldNames)
                End If
            Next

            Return newTable
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        End Try
    End Function

    Private Sub rdo_Format_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rdo_Format.SelectedIndexChanged
        If rdo_Format.SelectedItem.Value = "D" Then
            divformat.Visible = True
            divDefaultPlan.Visible = False
        ElseIf rdo_Format.SelectedItem.Value = "Y" Then
            divformat.Visible = False
            divDefaultPlan.Visible = True
        End If
        BtnDownLoad.Visible = False
    End Sub
    Sub CsvExport()
        Dim dt As New DataTable
        dt = CType(Session("Errordt"), DataTable)
        'Build the CSV file data as a Comma separated string.
        Dim csv As String = String.Empty

        For Each column As DataColumn In dt.Columns
            'Add the Header row for CSV file.
            csv += column.ColumnName + ","c
        Next

        'Add new line.
        csv += vbCr & vbLf

        For Each row As DataRow In dt.Rows
            For Each column As DataColumn In dt.Columns
                'Add the Data rows.
                csv += row(column.ColumnName).ToString().Replace(",", ";") + ","c
            Next

            'Add new line.
            csv += vbCr & vbLf
        Next

        'Download the CSV file.
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=ErrorExport.csv")
        Response.Charset = ""
        Response.ContentType = "application/text"
        Response.Output.Write(csv)
        Response.Flush()
        Response.End()
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        divResult.Visible = True
        Dim Str As StringBuilder = New StringBuilder
        Dim ConfirmStr As StringBuilder = New StringBuilder
        If rdo_Format.SelectedItem.Value = "D" Then
            If Rdo_Type.SelectedItem.Value = "1" Then
                If Session("Tbl") IsNot Nothing Then
                    If DoValidation(Session("Tbl"), Str, ConfirmStr) Then
                        If Str.ToString() = "" And ConfirmStr.ToString = "" Then
                            bReimport = True
                            SavePlan(CType(Session("Tbl"), DataTable), Err_Desc, Err_No)
                            bReimport = False
                        End If
                    End If
                End If
            Else
                If Session("Tbl") IsNot Nothing Then
                    If DoValidationForSplitedColumn(Session("Tbl"), Str, ConfirmStr) Then
                        If Str.ToString() = "" And ConfirmStr.ToString = "" Then
                            bReimport = True
                            SavePlanSplittedColumn(CType(Session("Tbl"), DataTable), Err_Desc, Err_No)
                            bReimport = False
                        End If
                    End If
                End If
                 
            End If
            lblResult.Text = Str.ToString()
        End If
    End Sub
End Class