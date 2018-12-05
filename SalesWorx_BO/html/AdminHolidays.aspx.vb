Imports SalesWorx.BO.Common
Imports System.Data
Imports System.Web
Imports System.Resources
Imports System.Web.UI.WebControls
Imports log4net
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Data.OleDb
Public Class AdminHolidays
    Inherits System.Web.UI.Page
    Dim objRoute As New RoutePlan
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim Dt As New DataTable
    Private Const PageID As String = "P333"
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
            Dt = objRoute.GetHolidayList(Err_No, Err_Desc, "", "", "")
            BindHolidayData()
            Resetfields()
        Else
            MPEDetails.VisibleOnPageLoad = False
            MPEImport.VisibleOnPageLoad = False
        End If

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
        Me.HolidayDate.SelectedDate = DateTime.Now
        Me.txtDescription.Text = ""
       
        Me.HolidayDate.Enabled = True
        'Me.btnAdd.Focus()
        ''Me.lblMessage.Text = ""

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

        If Me.HolidayDate.SelectedDate = Nothing Or Me.txtDescription.Text = "" Then
            Me.lblPop.Text = "Holiday Date and description are required."
            Me.MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If

        Dim success As Boolean = False
        Try

            objRoute.HolidayDate = HolidayDate.SelectedDate.Value.ToString("dd-MMM-yyyy")
            objRoute.HolidayDescription = IIf(Me.txtDescription.Text = "", "", Me.txtDescription.Text)
          
            If objRoute.SaveHoliday(Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
                success = True
                MessageBoxValidation("Successfully saved", "Information")
                Dim fromdate As String = ""
                If Not txt_fromDate.SelectedDate Is Nothing Then
                    fromdate = txt_fromDate.SelectedDate.Value.ToString("dd-MMM-yyyy")
                End If

                Dim todate As String = ""
                If Not txt_ToDate.SelectedDate Is Nothing Then
                    todate = txt_ToDate.SelectedDate.Value.ToString("dd-MMM-yyyy")
                End If
                Dt = objRoute.GetHolidayList(Err_No, Err_Desc, fromdate, todate, txtFilterVal.Text)
                BindHolidayData()
                Resetfields()
                Me.MPEDetails.VisibleOnPageLoad = False
                ClassUpdatePnl.Update()
            Else
                MessageBoxValidation("Could not be saved.", "Information")
                Me.MPEDetails.VisibleOnPageLoad = False
                ClassUpdatePnl.Update()
            End If

            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "ROUTE PLAN", "Holiday", HolidayDate.SelectedDate.Value.ToString("dd-MMM-yyyy"), "Date: " & HolidayDate.SelectedDate.Value.ToString("dd-MMM-yyyy") & "/ Desc :  " & Me.txtDescription.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")

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
    Private Sub BindHolidayData()
        Me.gvHoliday.DataSource = Dt
        Me.gvHoliday.DataBind()

        Dim dv As New DataView(Dt)
        If ViewState("SortField") <> "" Then
            dv.Sort = (ViewState("SortField") & " ") + SortDirection
        End If
        gvHoliday.DataSource = dv
        gvHoliday.DataBind()
        Session.Remove("HolidayCode")
        Session("HolidayCode") = Dt
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
        If Me.HolidayDate.SelectedDate = Nothing Or Me.txtDescription.Text = "" Then
            Me.lblPop.Text = "Holiday Date and description are required."
            Me.MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If
        Dim success As Boolean = False
        Try

            objRoute.HolidayDate = HolidayDate.SelectedDate.Value.ToString("dd-MMM-yyyy")
            objRoute.HolidayDescription = IIf(Me.txtDescription.Text = "", "", Me.txtDescription.Text)


            If objRoute.SaveHoliday(Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
                success = True
                MessageBoxValidation("Successfully updated.", "Information")
                Dim fromdate As String = ""
                If Not txt_fromDate.SelectedDate Is Nothing Then
                    fromdate = txt_fromDate.SelectedDate.Value.ToString("dd-MMM-yyyy")
                End If

                Dim todate As String = ""
                If Not txt_ToDate.SelectedDate Is Nothing Then
                    todate = txt_ToDate.SelectedDate.Value.ToString("dd-MMM-yyyy")
                End If
                Dt = objRoute.GetHolidayList(Err_No, Err_Desc, fromdate, todate, txtFilterVal.Text)
                BindHolidayData()
                Resetfields()
                Me.MPEDetails.VisibleOnPageLoad = False
                ClassUpdatePnl.Update()
            Else
                MessageBoxValidation("Could not be saved.", "Information")
                Me.MPEDetails.VisibleOnPageLoad = False
                ClassUpdatePnl.Update()
            End If

            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "U", "ROUTE PLAN", "Holiday", HolidayDate.SelectedDate.Value.ToString("dd-MMM-yyyy"), "Date: " & HolidayDate.SelectedDate.Value.ToString("dd-MMM-yyyy") & "/ Desc :  " & Me.txtDescription.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                
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

    Protected Sub btnDeleteAll_Click()
        Try
            Dim dr As GridViewRow
            Dim Success As Boolean = False
            Dim selected As Boolean = False
            For Each dr In gvHoliday.Rows
                Dim RowCheckBox As System.Web.UI.WebControls.CheckBox = dr.FindControl("chkDelete")

                If RowCheckBox.Checked = True Then
                    selected = True
                    Dim Lbl As System.Web.UI.WebControls.Label = dr.FindControl("lblHoliday")
                    If objRoute.DeleteHoliday(Err_No, Err_Desc, Lbl.Text) = True Then
                        objLogin.SaveUserLog(Err_No, Err_Desc, "D", "ROUTE PLAN", "Holiday", dr.Cells(1).Text, "Date: " & dr.Cells(1).Text & "/ Desc :  " & dr.Cells(2).Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                        Success = True
                    End If
                End If
            Next
            If (Success = True) Then
                MessageBoxValidation("Holiday(s) deleted successfully.", "Information")
                Dim fromdate As String = ""
                If Not txt_fromDate.SelectedDate Is Nothing Then
                    fromdate = txt_fromDate.SelectedDate.Value.ToString("dd-MMM-yyyy")
                End If

                Dim todate As String = ""
                If Not txt_ToDate.SelectedDate Is Nothing Then
                    todate = txt_ToDate.SelectedDate.Value.ToString("dd-MMM-yyyy")
                End If
                Dt = objRoute.GetHolidayList(Err_No, Err_Desc, fromdate, todate, txtFilterVal.Text)
                BindHolidayData()
                Resetfields()
            Else
                If selected = True Then
                    log.Error(Err_Desc)
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_HolidayCode_003") & "&next=HolidayCode.aspx&Title=Message", False)
                    Exit Try
                End If
            End If

                'ClassUpdatePnl.Update()
        Catch ex As Exception
            Err_No = "74063"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Dim success As Boolean = False
        Try
            Dim fromdate As String = ""
            If Not txt_fromDate.SelectedDate Is Nothing Then
                fromdate = txt_fromDate.SelectedDate.Value.ToString("dd-MMM-yyyy")
            End If

            Dim todate As String = ""
            If Not txt_ToDate.SelectedDate Is Nothing Then
                todate = txt_ToDate.SelectedDate.Value.ToString("dd-MMM-yyyy")
            End If
            Dt = objRoute.GetHolidayList(Err_No, Err_Desc, fromdate, todate, txtFilterVal.Text)
            BindHolidayData()
            ClassUpdatePnl.Update()
            success = True
            If success = False Then
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_HolidayCode_004") & "&next=HolidayCode.aspx&Title=Message", False)
                Exit Try
            End If
        Catch ex As Exception
            Err_No = "74064"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Protected Sub btnClearFilter_Click(ByVal sender As Object, ByVal e As EventArgs)
        txt_fromDate.Clear()
        txt_fromDate.DateInput.Clear()
        txtFilterVal.Text = ""
        txt_ToDate.Clear()
        txt_ToDate.DateInput.Clear()
        Dt = objRoute.GetHolidayList(Err_No, Err_Desc, "", "", "")
        BindHolidayData()
        ClassUpdatePnl.Update()
    End Sub
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btndelete As ImageButton = TryCast(sender, ImageButton)
        Dim row As GridViewRow = DirectCast(btndelete.NamingContainer, GridViewRow)

        Dim success As Boolean = False

        Try
            Dim Lbl As System.Web.UI.WebControls.Label = row.FindControl("lblHoliday")
            If objRoute.DeleteHoliday(Err_No, Err_Desc, Lbl.Text) = True Then
                success = True
            End If

            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "D", "ROUTE PLAN", "Holiday", row.Cells(1).Text, "Date: " & row.Cells(1).Text & "/ Desc :  " & row.Cells(2).Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                MessageBoxValidation("Successfully deleted.", "Information")
                Dim fromdate As String = ""
                If Not txt_fromDate.SelectedDate Is Nothing Then
                    fromdate = txt_fromDate.SelectedDate.Value.ToString("dd-MMM-yyyy")
                End If

                Dim todate As String = ""
                If Not txt_ToDate.SelectedDate Is Nothing Then
                    todate = txt_ToDate.SelectedDate.Value.ToString("dd-MMM-yyyy")
                End If
                Dt = objRoute.GetHolidayList(Err_No, Err_Desc, fromdate, todate, txtFilterVal.Text)
                BindHolidayData()
                Resetfields()
            Else
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_HolidayCode_005") & "&next=HolidayCode.aspx&Title=Message", False)
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



            HolidayDate.SelectedDate = DateTime.Parse(row.Cells(1).Text)
            txtDescription.Text = Server.HtmlDecode(Trim(row.Cells(2).Text))
            Me.HolidayDate.Enabled = False
            MPEDetails.VisibleOnPageLoad = True
            ' ClassUpdatePnl.Update()
        Catch ex As Exception
            Err_No = "74066"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_HolidayCode_006") & "&next=HolidayCode.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub

    Private Sub gvHoliday_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvHoliday.PageIndexChanging
        gvHoliday.PageIndex = e.NewPageIndex
        Dim fromdate As String = ""
        If Not txt_fromDate.SelectedDate Is Nothing Then
            fromdate = txt_fromDate.SelectedDate.Value.ToString("dd-MMM-yyyy")
        End If

        Dim todate As String = ""
        If Not txt_ToDate.SelectedDate Is Nothing Then
            todate = txt_ToDate.SelectedDate.Value.ToString("dd-MMM-yyyy")
        End If
        Dt = objRoute.GetHolidayList(Err_No, Err_Desc, fromdate, todate, txtFilterVal.Text)
        BindHolidayData()

    End Sub

    Private Sub gvHoliday_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvHoliday.RowDataBound
        'If e.Row.RowType.Equals(DataControlRowType.Pager) Then
        '    Dim pTableRow As TableRow = _
        '             CType(e.Row.Cells(0).Controls(0).Controls(0), TableRow)
        '    For Each cell As TableCell In pTableRow.Cells
        '        For Each control As Control In cell.Controls
        '            If TypeOf control Is LinkButton Then
        '                Dim lb As LinkButton = CType(control, LinkButton)
        '                lb.Attributes.Add("onclick", "ScrollToTop();")
        '            End If
        '        Next
        '    Next
        'End If
    End Sub
    Private Sub gvHoliday_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvHoliday.Sorting
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        Dim fromdate As String = ""
        If Not txt_fromDate.SelectedDate Is Nothing Then
            fromdate = txt_fromDate.SelectedDate.Value.ToString("dd-MMM-yyyy")
        End If

        Dim todate As String = ""
        If Not txt_ToDate.SelectedDate Is Nothing Then
            todate = txt_ToDate.SelectedDate.Value.ToString("dd-MMM-yyyy")
        End If
        Dt = objRoute.GetHolidayList(Err_No, Err_Desc, fromdate, todate, txtFilterVal.Text)
        BindHolidayData()
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

    Private Sub btn_import_Click(sender As Object, e As EventArgs) Handles btn_import.Click
        MPEImport.VisibleOnPageLoad = True
        ClassUpdatePnl.Update()
    End Sub
    Private Function SetErrorsTable() As DataTable
        Dim col As DataColumn
        Dim dtErrors As New DataTable

        col = New DataColumn()
        col.ColumnName = "RowNo"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtErrors.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "LogInfo"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtErrors.Columns.Add(col)

        Return dtErrors
    End Function
    Private Sub btnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImport.Click
        lbLog.Visible = False
        If Me.ExcelFileUpload.FileName = Nothing Then
            ' Me.lblinfo.Text = "Validation"
            Me.lblUpMsg.Text = "Select filename "
            ' Me.lblMessage.ForeColor = Drawing.Color.Green
            ' Me.MpInfoError.Show()
            Me.MPEImport.VisibleOnPageLoad = True
            Exit Sub
        End If


        Session("dtHolidays") = Nothing
        Dim dtErrors As New DataTable
        dtErrors = SetErrorsTable().Copy
        Dim Str As New StringBuilder

        Dim TotSuccess As Integer = 0
        Dim TotFailed As Integer = 0
        Try


            ViewState("FileType") = Me.ExcelFileUpload.PostedFile.ContentType
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

                ExcelFileUpload.SaveAs(ViewState("FileName"))

                Try
                    Dim st As Boolean = False

                    If ViewState("FileType") IsNot Nothing And ViewState("FileName") IsNot Nothing Then
                        Dim TempTbl As New DataTable
                        If TempTbl.Rows.Count > 0 Then
                            TempTbl.Rows.Clear()
                        End If
                        Dim col As DataColumn

                        col = New DataColumn
                        col.ColumnName = "HolidayDate"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)


                        col = New DataColumn
                        col.ColumnName = "Description"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)





                        If ViewState("FileName").ToString.EndsWith(".csv") Then
                            TempTbl = DoCSVUpload()
                        ElseIf ViewState("FileName").ToString.EndsWith(".xls") Then
                            TempTbl = DoXLSUpload()
                        ElseIf ViewState("FileName").ToString.EndsWith(".xlsx") Then
                            TempTbl = DoXLSXUpload()
                        End If

                        If TempTbl.Columns.Count = 2 Then
                            If Not (TempTbl.Columns(0).ColumnName.ToLower = "date" And TempTbl.Columns(1).ColumnName.ToLower = "description") Then
                                Me.lblUpMsg.Text = "Please check the template columns are correct"
                                Me.MPEImport.VisibleOnPageLoad = True
                                Exit Sub
                            End If
                        Else
                            Me.lblUpMsg.Text = "Invalid Template"
                            Me.MPEImport.VisibleOnPageLoad = True
                            Exit Sub
                        End If

                        If TempTbl.Rows.Count = 0 Then
                            Me.lblUpMsg.Text = "There is no data in your file."
                            Me.MPEImport.VisibleOnPageLoad = True
                            Exit Sub
                        End If

                        Dim RowNo As String = Nothing
                        ' Dim ColNo As String = Nothing
                        ' Dim ColumnName As String = Nothing
                        Dim ErrorText As String = Nothing

                        If TempTbl.Rows.Count > 0 Then
                            Dim idx As Integer

                            For idx = 0 To TempTbl.Rows.Count - 1
                                Dim HDate As String = Nothing
                                Dim Description As String = Nothing


                                Dim isValidRow As Boolean = True

                                HDate = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, "", TempTbl.Rows(idx)(0).ToString())
                                Description = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, "", TempTbl.Rows(idx)(1).ToString())

                                Dim CustomerID As String = "0"
                                Dim SiteID As String = "0"
                                Dim InventoryItemID As String = "0"

                                If IsDate(HDate) = False Then
                                    RowNo = idx + 2
                                    ' ColNo = ColNo + "3" + ","
                                    ' ColumnName = ColumnName + "BonusItem" + ","
                                    ErrorText = ErrorText + "Invalid Date" + ","
                                    isValidRow = False
                                    TotFailed += 1
                                End If
                                If String.IsNullOrEmpty(Description) Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Invalid Description" + ","
                                    isValidRow = False
                                    TotFailed += 1
                                End If
                                If Not (RowNo Is Nothing And ErrorText Is Nothing) Then
                                    Dim h As DataRow = dtErrors.NewRow()
                                    h("RowNo") = RowNo
                                    ' h("ColNo") = ColNo
                                    ' h("ColName") = ColumnName
                                    h("LogInfo") = ErrorText
                                    dtErrors.Rows.Add(h)
                                    RowNo = Nothing
                                    'ColNo = Nothing
                                    'ColumnName = Nothing
                                    ErrorText = Nothing
                                    isValidRow = True
                                Else
                                    objRoute.HolidayDate = CDate(HDate).ToString("dd-MMM-yyyy")
                                    objRoute.HolidayDescription = Description
                                    If objRoute.SaveHoliday(Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
                                        TotSuccess = TotSuccess + 1
                                        Dim h As DataRow = dtErrors.NewRow()
                                        h("RowNo") = idx + 2
                                        h("LogInfo") = "Successfully uploaded"
                                        dtErrors.Rows.Add(h)
                                        RowNo = Nothing
                                        ErrorText = Nothing
                                        isValidRow = True
                                    Else
                                        Dim h As DataRow = dtErrors.NewRow()
                                        h("RowNo") = idx + 2
                                        h("LogInfo") = "Error occured while uploading this row"
                                        dtErrors.Rows.Add(h)
                                        RowNo = Nothing
                                        ErrorText = Nothing
                                        isValidRow = True

                                    End If


                                End If

                            Next
                        End If


                        ' If TotSuccess > 0 Then

                        DeleteExcel()
                        lblUpMsg.Text = IIf(TotFailed = 0, "Successfully uploaded", "One or more rows has invalid data. Please check the log")
                        Me.MPEImport.VisibleOnPageLoad = True
                        Dim fromdate As String = ""
                        If Not txt_fromDate.SelectedDate Is Nothing Then
                            fromdate = txt_fromDate.SelectedDate.Value.ToString("dd-MMM-yyyy")
                        End If

                        Dim todate As String = ""
                        If Not txt_ToDate.SelectedDate Is Nothing Then
                            todate = txt_ToDate.SelectedDate.Value.ToString("dd-MMM-yyyy")
                        End If
                        Dt = objRoute.GetHolidayList(Err_No, Err_Desc, fromdate, todate, txtFilterVal.Text)
                        BindHolidayData()
                        'End If
                    End If

                    dgvErros.Visible = False
                    If dtErrors.Rows.Count > 0 Then
                        lbLog.Visible = True
                    Else
                        lbLog.Visible = False
                    End If
                    Me.dgvErros.DataSource = dtErrors
                    Me.dgvErros.DataBind()
                    Session.Remove("dtHolidays")
                    Session("dtHolidays") = dtErrors.Copy


                    Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & "SimpleBonusLog_" & Now.ToString("yyyyMMdd") + ".txt"
                    DataTable2CSV(dtErrors, fn, vbTab)
                    If dtErrors.Rows.Count > 0 Then
                        lbLog.Visible = True
                    End If
                    Session.Remove("SimpleLogInfo")
                    Session("SimpleLogInfo") = fn




                Catch ex As Exception

                    Err_No = "74085"
                    If Err_Desc Is Nothing Then
                        log.Error(GetExceptionInfo(ex))
                    Else
                        log.Error(Err_Desc)
                    End If
                End Try
            Else
                ' Str.Append("<span style='font-color:red'> <b >Please import valid Excel template.</b></span>")
                lblUpMsg.Text = "Please import valid Excel template."
                Me.MPEImport.VisibleOnPageLoad = True
            End If


        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try

    End Sub
    Protected Sub lbLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbLog.Click
        Try


            If Not Session("SimpleLogInfo") Is Nothing Then
                Dim fileValue As String = Session("SimpleLogInfo")





                Dim file As System.IO.FileInfo = New FileInfo(fileValue)

                If file.Exists Then


                    Response.Clear()
                    Response.ContentType = "text/plain"

                    Dim filePath As String = fileValue
                    Response.ContentType = ContentType
                    Response.AddHeader("Content-Disposition", ("attachment; filename=" + Path.GetFileName(file.Name)))
                    Response.TransmitFile(filePath)
                    Response.Flush()
                    Response.End()
                    'Response.End()
                    '

                    'Response.AddHeader("Content-Disposition", "attachment; filename=" & file.Name)

                    'Response.AddHeader("Content-Length", file.Length.ToString())

                    'Response.WriteFile(file.FullName)


                    'Response.[End]()
                Else
                    lblUpMsg.Text = "File does not exist"
                    'lblMessage.ForeColor = Drawing.Color.Green
                    'lblinfo.Text = "Information"
                    MPEImport.VisibleOnPageLoad = True
                    Exit Sub

                End If

            Else
                lblUpMsg.Text = "There is no log to show."
                'lblMessage.ForeColor = Drawing.Color.Green
                'lblinfo.Text = "Information"
                MPEImport.VisibleOnPageLoad = True
                Exit Sub

            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))

        Finally
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
    Sub DataTable2CSV(ByVal table As DataTable, ByVal filename As String, ByVal sepChar As String)
        Dim writer As System.IO.StreamWriter
        Try
            writer = New System.IO.StreamWriter(filename)

            ' first write a line with the columns name
            Dim sep As String = ""
            Dim builder As New System.Text.StringBuilder
            For Each col As DataColumn In table.Columns
                builder.Append(sep).Append(col.ColumnName)
                sep = sepChar
            Next
            writer.WriteLine(builder.ToString())

            ' then write all the rows
            For Each row As DataRow In table.Rows
                sep = ""
                builder = New System.Text.StringBuilder

                For Each col As DataColumn In table.Columns
                    builder.Append(sep).Append(row(col.ColumnName))
                    sep = sepChar
                Next
                writer.WriteLine(builder.ToString())
            Next
        Finally
            If Not writer Is Nothing Then writer.Close()
        End Try
    End Sub
    Private Function DoCSVUpload() As DataTable
        Dim dtImport As New DataTable
        Try
            Dim strConString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "\;Extended Properties=""text;HDR=Yes;FMT=Delimited"""
            Dim oledbConn As New OleDbConnection(strConString)

            Dim cmd As New OleDbCommand("SELECT * FROM [" & ViewState("CSVName") & "]", oledbConn)

            Dim oleda As New OleDbDataAdapter()

            oleda.SelectCommand = cmd



            oleda.Fill(dtImport)
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        End Try
        Return dtImport
    End Function

    Private Function DoXLSUpload() As DataTable
        Dim dtImport As New DataTable
        Try
            Dim connString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & ViewState("FileName") & " ;Extended Properties=""Excel 8.0;HDR=Yes;IMEX=1"""
            Dim oledbConn As New OleDbConnection(connString)
            Try
                oledbConn.Open()

                Dim cmd As New OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn)

                Dim oleda As New OleDbDataAdapter()

                oleda.SelectCommand = cmd



                oleda.Fill(dtImport)



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
        Return dtImport
    End Function
    Private Function DoXLSXUpload() As DataTable
        Dim dtImport As New DataTable
        Try
            Dim connString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & ViewState("FileName") & " ;Extended Properties=""Excel 12.0;HDR=Yes;IMEX=1"""
            Dim oledbConn As New OleDbConnection(connString)
            Try
                oledbConn.Open()

                Dim cmd As New OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn)

                Dim oleda As New OleDbDataAdapter()

                oleda.SelectCommand = cmd



                oleda.Fill(dtImport)



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
        Return dtImport
    End Function

End Class