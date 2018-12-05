Imports System.Data.OleDb
Imports System.IO
Imports log4net
Imports System.Data.SqlClient
Imports SalesWorx.BO.Common

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
        Else
            Me.BtnReimport.Enabled = True
        End If
    End Sub

    Private Sub BtnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnImport.Click
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
                            If TempTbl.Rows.Count > 0 Then
                                If DoValidation(TempTbl, Str, ConfirmStr) Then
                                    Session("DoneValidation") = True
                                    If Str.ToString() = "" And ConfirmStr.ToString = "" Then
                                        Err_Desc = Nothing
                                        Err_No = Nothing
                                        SavePlan(TempTbl, Err_Desc, Err_No)
                                    End If
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
        If Session("Tbl") IsNot Nothing And Session("DoneValidation") = True Then
            bReimport = True
            SavePlan(CType(Session("Tbl"), DataTable), Err_Desc, Err_No)
            bReimport = False
        End If
    End Sub
    Protected Sub DummyImBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles DummyImBtn.Click
        Try
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
                        If DoValidation(TempTbl, Str, ConfirmStr) Then
                            Session("DoneValidation") = True
                            If Str.ToString() = "" And ConfirmStr.ToString = "" Then
                                Err_Desc = Nothing
                                Err_No = Nothing
                                SavePlan(TempTbl, Err_Desc, Err_No)
                            End If
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
    Private Function DoValidation(ByVal Tbl As DataTable, ByVal Str As StringBuilder, ByRef ConfirmStr As StringBuilder) As Boolean
        Try
            ''Adding identity Column in Datatable
            Tbl.Columns.Add("ID", GetType(Int32))
            Tbl.Columns.Add("Status", GetType(String))
            Tbl.Columns.Add("DefaultPlanID", GetType(Int32))
            ' Tbl.Columns.Add("SiteID", GetType(Int32))
            For i As Integer = 1 To Tbl.Rows.Count
                Tbl.Rows(i - 1).Item(6) = i
            Next

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
                        End If


                        If dr.Item(1) Is DBNull.Value Then
                            HasNull = True
                            Session("DoneValidation") = False
                            Str.Append("<br/><span style='font-color:red'> <b >Customer Id is Null at Row No: " & RowCount & " </b></span>")
                        End If

                        If dr.Item(2) Is DBNull.Value Then
                            HasNull = True
                            Session("DoneValidation") = False
                            Str.Append("<br/><span style='font-color:red'> <b >Site Use Id is Null at Row No: " & RowCount & " </b></span>")
                        End If

                        '

                        If dr.Item(3) Is DBNull.Value Then
                            HasNull = True
                            Session("DoneValidation") = False
                            Str.Append("<br/><span style='font-color:red'> <b >Date is Null at Row No: " & RowCount & " </b></span>")
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

                            Dim Dat As DateTime

                            ''Check having valid date
                            Try
                                Dat = dr.Item(3)
                            Catch ex As Exception
                                Session("DoneValidation") = False
                                Str.Append("<br/><span style='font-color:red'> <b >Invalid date at Row No: " & RowCount & " </b></span>")
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
                                                Str.Append("<br/><span style='font-color:red'> <b >The login user do not have link with the FSR at Row No : " & RowCount & " </b></span>")
                                                Exit Function
                                            End If


                                            ''Check Route Plan Exist
                                            Err_No = Nothing
                                            Err_Desc = Nothing

                                            Dim Result As String
                                            Result = objRoutePlan.RoutePlanExist(Dat, dr.Item(0), Err_No, Err_Desc)

                                            dr("Status") = Result

                                            If Result.StartsWith("HAVECURRENTMONTH") Then
                                                ConfirmStr.Append("<br/><span style='font-color:red'> <b >Row No : " & RowCount & " Current Month Data will be ignored</b></span>" & "")
                                            ElseIf Result.StartsWith("HAVEFUTUREMONTH") Then
                                                ConfirmStr.Append("<br/><span style='font-color:red'> <b >Row No : " & RowCount & " Route Plan already exists for given month.</b></span>" & "")
                                            End If


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
                                                Str.Append("<br/><span style='font-color:red'> <b >There is no default plan for van : " & dr.Item(0) & " </b></span>")
                                                Exit For
                                            End If
                                        Else
                                            If Err_Desc IsNot Nothing Then log.Error(Err_Desc & " at Row : " & RowCount)
                                            Session("DoneValidation") = False
                                            Str.Append("<br/><span style='font-color:red'> <b >FSR ID , Customer ID and Site ID is not matching at Row No: " & RowCount & " </b></span>")
                                        End If
                                    Else
                                        If Err_Desc IsNot Nothing Then log.Error(Err_Desc & " at Row : " & RowCount)
                                        Str.Append("<br/><span style='font-color:red'> <b >There is no Default Plan for given date at Row No: " & RowCount & " </b></span>")
                                        Session("DoneValidation") = False
                                    End If
                                Else
                                    If Err_Desc IsNot Nothing Then log.Error(Err_Desc & " at Row : " & RowCount)
                                    Str.Append("<br/><span style='font-color:red'> <b >Route Plan Should be for Future Months. Error at Row No: " & RowCount & " </b></span>")
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
End Class