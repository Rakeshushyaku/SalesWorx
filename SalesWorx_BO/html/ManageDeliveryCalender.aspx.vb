

Imports SalesWorx.BO.Common
Imports log4net
Imports Telerik.Web.UI
Imports System.IO
Imports System.Data.OleDb
Imports OfficeOpenXml

Partial Public Class ManageDeliveryCalender
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim dt As New DataTable
    Dim objDivConfig As New DivConfig
    Dim objOrgCtl As New OrgCtl
    Dim objDlvclndr As New DeliveryCalender
    Private dtErrors As New DataTable
    Dim objLogin As New SalesWorx.BO.Common.Login
    ' Dim SortField As String = ""
    Private Const ModuleName As String = "ManageDeliveryCalender.aspx"
    Private Const PageID As String = "P81"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        log.Error("1")
        If Not Page.IsPostBack() Then
            log.Error("2")
            If Session.Item("USER_ACCESS") Is Nothing Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            If Not HasAuthentication() Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If

            FillOrganization()
            BindGrid()

            txtExDate.MinDate = Now()
            txtExDate.SelectedDate = Now()
            txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
            txtToDate.SelectedDate = Now()
            ddFilterBy.SelectedIndex = 0

            Session.Remove("Search")
            ViewState("FileType") = Nothing
            ViewState("FileName") = Nothing
            Session.Remove("dtPrErrors")
            Session.Remove("ExDateLogFile")
            SetErrorsTable()

        Else
            Me.MPEDetails.VisibleOnPageLoad = False


        End If

        lblPop.Text = ""
    End Sub
    Private Function HasAuthentication() As Boolean
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
    End Function




    Sub FillOrganization()
        Dim dt_org As New DataTable
        Dim objCommon As New SalesWorx.BO.Common.Common
        Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)

        dt_org = objCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
        ddlOrganization.DataSource = dt_org
        ddlOrganization.Items.Clear()
        ddlOrganization.Items.Add(New RadComboBoxItem("Select Organization"))
        ddlOrganization.AppendDataBoundItems = True
        ddlOrganization.DataValueField = "MAS_Org_ID"
        ddlOrganization.DataTextField = "Description"
        ddlOrganization.DataBind()

        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1

        End If



        ddlOrganization_add.DataSource = dt_org
        ddlOrganization_add.Items.Clear()
        ddlOrganization_add.Items.Add(New RadComboBoxItem("Select Organization"))
        ddlOrganization_add.AppendDataBoundItems = True
        ddlOrganization_add.DataValueField = "MAS_Org_ID"
        ddlOrganization_add.DataTextField = "Description"
        ddlOrganization_add.DataBind()

        If ddlOrganization_add.Items.Count = 2 Then
            ddlOrganization_add.SelectedIndex = 1

        End If


    End Sub


    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        btnUpdate.Visible = False
        btnSave.Visible = True
        ddlOrganization_add.ClearSelection()
        ddlOrganization_add.Enabled = True
        txtExDate.SelectedDate = Now()
        rdo_Working.ClearSelection()
        Resetfields()
        MPEDetails.VisibleOnPageLoad = True
        ClassUpdatePnl.Update()
        Session("Add") = "1"
    End Sub
    Sub BindGrid()
        Try
            If ddlOrganization.SelectedIndex > 0 Then
                Dim dt_exdates As New DataTable

                Dim Fromdate As Date
                Dim Todate As Date

                Fromdate = CDate(txtFromDate.SelectedDate)
                Todate = CDate(txtToDate.SelectedDate)


                If Session("Search") = "1" Then
                    dt_exdates = objDlvclndr.SerachDeliveryCalender(Err_No, Err_Desc, ddlOrganization.SelectedValue, Fromdate.ToString("dd-MMM-yyyy"), Todate.ToString("dd-MMM-yyyy"), IIf(ddFilterBy.SelectedIndex = 0, "ALL", ddFilterBy.SelectedValue))
                Else
                    dt_exdates = objDlvclndr.GetDeliveryCalender(Err_No, Err_Desc, ddlOrganization.SelectedValue, "DATE")
                End If


                Dim dv As New DataView(dt_exdates)
                If SortField <> "" Then
                    dv.Sort = (SortField & " ") + SortDirection
                End If
                gvDlvClndr.DataSource = dv
                gvDlvClndr.DataBind()
          
            End If

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
    Public Sub Resetfields()

        Me.btnAdd.Focus()
        ' Me.ddFilterBy.SelectedIndex = 0
        Me.lblPop.Text = ""


    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click


        If Session("Add") = "1" Then

            Dim success As Boolean = False
            Try
                txtExDate.Enabled = True
                ddlOrganization_add.Enabled = True

                If ddlOrganization_add.SelectedIndex < 1 Then
                    Me.lblPop.Text = "Please select the Organization."
                    Exit Sub
                End If

                Dim TemToDateStr As String = CDate(txtExDate.SelectedDate).ToString("dd/MM/yyyy")
                Dim DateArr1 As Array = TemToDateStr.Split("/")
                If DateArr1.Length = 3 Then
                    TemToDateStr = DateArr1(1) & "/" & DateArr1(0) & "/" & DateArr1(2)
                End If



                If Not IsDate(TemToDateStr) Then
                    MessageBoxValidation("Enter valid ""Ex date"".", "Validation")
                    Me.lblPop.Text = "Enter valid date."
                    SetFocus(TemToDateStr)
                    Exit Sub
                End If



                If txtExDate.SelectedDate <= DateTime.Now.Date Then
                    Me.lblPop.Text = "Select Future date."
                    SetFocus(TemToDateStr)
                    Exit Sub
                End If

                Dim dt_exist As DataTable

                dt_exist = objDlvclndr.CheckDC_ExDateExist(Err_No, Err_Desc, ddlOrganization_add.SelectedValue, CDate(txtExDate.SelectedDate).ToString("yyyy-MM-dd"))
                If dt_exist.Rows.Count = 0 Then
                    If objDlvclndr.ManageDeliveryCalenderDate(Err_No, Err_Desc, "0", ddlOrganization_add.SelectedValue, CDate(txtExDate.SelectedDate).ToString("yyyy-MM-dd"), rdo_Working.SelectedValue, CType(Session("User_Access"), UserAccess).UserID) Then
                        MessageBoxValidation("Successfully saved", "Information")

                        If ddlOrganization.SelectedIndex < 1 Then
                            ddlOrganization.SelectedValue = ddlOrganization_add.SelectedValue
                        End If
                        Me.MPEDetails.VisibleOnPageLoad = False
                        UpdatePanel1.Update()
                        Resetfields()
                        BindGrid()
                        ClassUpdatePnl.Update()
                        Session("Add") = "0"
                    End If
                Else
                    Me.lblPop.Text = "Exception date  already exist  ."
                    Me.MPEDetails.VisibleOnPageLoad = True
                    Exit Sub
                End If








            Catch ex1 As SqlClient.SqlException

                'log.Error(ex1.Message.ToString)
                'lblPop.Text = "Error in Saving Please check log"
                'MPEDetails.VisibleOnPageLoad = True
                'UpdatePanel1.Update()

            Catch ex As Exception

                Err_No = "74205"
                lblPop.Text = "Error in Saving Please check log"
                'MessageBoxValidation("Error in Saving Please check log", "Information")
                MPEDetails.VisibleOnPageLoad = True
                UpdatePanel1.Update()

                If Err_Desc Is Nothing Then
                    log.Error(ex.ToString)
                Else
                    log.Error(ex.ToString)
                End If
            End Try
            'Else
            '    BindGrid()
            '    ClassUpdatePnl.Update()
        End If
    End Sub

    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Session("Add") = "1"
            lblPop.Text = ""
            btnUpdate.Visible = True
            btnSave.Visible = False
            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)

            HidVal.Value = btnEdit.CommandArgument.ToString()





            Dim dt_ As New DataTable
            dt_ = objDlvclndr.GetDeliveryCalender_Details(Err_No, Err_Desc, ddlOrganization.SelectedValue, HidVal.Value)
            If dt_.Rows.Count > 0 Then

                If Convert.ToDateTime(dt_(0)("Delivery_Date")) > DateTime.Now.Date Then
                    txtExDate.SelectedDate = dt_(0)("Delivery_Date")
                    rdo_Working.SelectedValue = dt_(0)("Is_Working")
                    ddlOrganization_add.SelectedValue = dt_(0)("Organization_ID")
                    txtExDate.Enabled = False
                    ddlOrganization_add.Enabled = False
                    MPEDetails.VisibleOnPageLoad = True

                Else
                    MessageBoxValidation("Editing allows only future dates ", "Information")
                End If
            End If



            UpdatePanel1.Update()
        Catch ex As Exception
            Err_No = "74208"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=ManageOrganization.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub
    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click

        If Session("Add") = "1" Then
            Dim success As Boolean = False
            Try

                If objDlvclndr.ManageDeliveryCalenderDate(Err_No, Err_Desc, HidVal.Value, ddlOrganization_add.SelectedValue, CDate(txtExDate.SelectedDate).ToString("yyyy-MM-dd"), rdo_Working.SelectedValue, CType(Session("User_Access"), UserAccess).UserID) Then
                    MessageBoxValidation("Successfully updated", "Information")
                    If ddlOrganization.SelectedIndex < 1 Then
                        ddlOrganization.SelectedValue = ddlOrganization_add.SelectedValue
                        TopPanel.Update()
                    End If
                    Me.MPEDetails.VisibleOnPageLoad = False
                    UpdatePanel1.Update()
                    Resetfields()
                    BindGrid()
                    ClassUpdatePnl.Update()
                    Session("Add") = "0"
                End If


            Catch ex As Exception
                Err_No = "74209"
                lblPop.Text = "Error in Updation Please check log"
                If Err_Desc Is Nothing Then
                    log.Error(GetExceptionInfo(ex))
                Else
                    log.Error(Err_Desc)
                End If
            End Try
        Else
            BindGrid()
            ClassUpdatePnl.Update()
        End If
    End Sub


    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btndelete As ImageButton = TryCast(sender, ImageButton)
        Dim row As GridViewRow = DirectCast(btndelete.NamingContainer, GridViewRow)
        Dim delivery_date As String = row.Cells(1).Text

        If Convert.ToDateTime(delivery_date) <= DateAndTime.Now.Date() Then
            MessageBoxValidation("Delete allows only future dates ", "Information")
            Exit Sub
        End If

        Try


            If objDlvclndr.DeleteDeliveryCalenderDate(Err_No, Err_Desc, btndelete.CommandArgument.ToString(), ddlOrganization.SelectedValue) Then
                MessageBoxValidation("Deleted successfully.", "Information")
                BindGrid()
                ClassUpdatePnl.Update()
            Else
                MessageBoxValidation("Error occured while deleting Organization.", "Information")

                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_ReasonCode_005") & "&next=ManageOrganization.aspx&Title=Message", False)
                Exit Try
            End If

        Catch ex As Exception
            Err_No = "74210"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub


    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFilter.Click

        Try
            Dim Criteria As String = "0"
            Session("Search") = "1"
            If ddlOrganization.SelectedIndex <= 0 Then
                MessageBoxValidation("Please Select Organization.", "Information")
                Exit Sub
            End If
            'If ddFilterBy.SelectedValue <> "0" Then
            '    Criteria = ddFilterBy.SelectedValue
            'End If


            'Dim dt As New DataTable
            'dt = objOrgCtl.GetSearchResultOrg(Err_No, Err_Desc, Criteria, "txtFilterVal.Text")
            'Dim dv As New DataView(dt)
            'If SortField <> "" Then
            '    dv.Sort = (SortField & " ") + SortDirection
            'End If
            'gvDlvClndr.DataSource = dv
            'gvDlvClndr.DataBind()
            BindGrid()
            ClassUpdatePnl.Update()
        Catch ex As Exception
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

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

    Protected Sub gvDlvClndr_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvDlvClndr.Sorting
        SortField = e.SortExpression
        SortDirection = "flip"
        BindGrid()
    End Sub

    Protected Sub gvDlvClndr_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDlvClndr.PageIndexChanging
        gvDlvClndr.PageIndex = e.NewPageIndex
        BindGrid()
    End Sub


    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            Resetfields()
            ddlOrganization.SelectedIndex = 0
            ddlday.ClearSelection()
            Session("Search") = "0"


            For Each itm As RadComboBoxItem In ddlday.Items
                itm.Checked = False
            Next


            txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
            txtToDate.SelectedDate = Now()
            ddFilterBy.ClearSelection()
            BindGrid()

            gvDlvClndr.DataSource = Nothing
            gvDlvClndr.DataBind()

            ClassUpdatePnl.Update()
        Catch ex As Exception
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub

    Private Sub ddlOrganization_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlOrganization.SelectedIndexChanged
        Try
            If ddlOrganization.SelectedIndex > 0 Then
                ddlday.ClearSelection()
                For Each itm As RadComboBoxItem In ddlday.Items
                    itm.Checked = False
                Next


                Dim dt_day As New DataTable
                dt_day = objDlvclndr.GetDeliveryCalender(Err_No, Err_Desc, ddlOrganization.SelectedValue, "DAY")



                If dt_day.Rows.Count > 0 Then
                    For Each itm As RadComboBoxItem In ddlday.Items
                        Dim result() As DataRow = dt_day.Select("Delivery_Day = '" + itm.Value + "'")
                        If result.Count > 0 Then
                            itm.Checked = True
                        End If
                    Next
                Else
                    For Each itm As RadComboBoxItem In ddlday.Items
                        itm.Checked = False
                    Next
                End If


               




                'For Each li As ListItem In chklst_day.Items
                '    Dim result() As DataRow = dt_day.Select("Delivery_Day = '" + li.Value + "'")
                '    If result.Count > 0 Then
                '        li.Selected = True
                '    End If
                'If li.Value = "ValueOfInterest" Then
                '    'Ok, this is the CheckBox we care about to determine if the TextBox should be enabled... is the CheckBox checked?
                '    If li.Selected Then
                '        'Yes, it is! Enable TextBox
                '        MyTextBox.Enabled = True
                '    Else
                '        'It is not checked, disable TextBox
                '        MyTextBox.Enabled = False
                '    End If
                'End If
                ' Next
                BindGrid()
                ClassUpdatePnl.Update()
            End If

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Protected Sub btnSave_holiday_Click(sender As Object, e As EventArgs)
        Try
            If ddlOrganization.SelectedIndex < 1 Then
                MessageBoxValidation("Please Select Organization.", "Information")
                Exit Sub
            End If
            Dim Daylst As String = ""

            'For Each li As ListItem In chklst_day.Items
            '    If li.Selected Then
            '        Daylst = Daylst + li.Value.ToString() + ","
            '    End If
            'Next



            Dim collection As IList(Of RadComboBoxItem) = ddlday.CheckedItems
            Dim Daylsttxt As String = ""

            For Each li As RadComboBoxItem In collection
                Daylst = Daylst & li.Value & ","
                Daylsttxt = Daylsttxt & li.Text & ","
            Next
            If Daylsttxt <> "" Then
                Daylsttxt = Daylsttxt.Substring(0, Daylsttxt.Length - 1)
            End If


            If Daylst.Length <> 0 Then
                Daylst = Daylst.Substring(0, Daylst.Length - 1)
            Else
                MessageBoxValidation("Please Select any of the days.", "Information")
                Exit Sub
            End If

            If objDlvclndr.ManageDeliveryCalenderDays(Err_No, Err_Desc, ddlOrganization.SelectedValue, Daylst, CType(Session("User_Access"), UserAccess).UserID) Then
                MessageBoxValidation("Successfully updated", "Information")
                BindGrid()
                ClassUpdatePnl.Update()
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Protected Sub ExcelFileUpload_FileUploaded(ByVal sender As Object, ByVal e As FileUploadedEventArgs)
        Try
            If Not e.File.FileName Is Nothing Then
                Dim fileName As String = e.File.FileName
                Dim exten As String = System.IO.Path.GetExtension(fileName)
                Dim Str As New StringBuilder
                Dim TotSuccess As Integer = 0
                Dim TotFailed As Integer = 0
                Dim TotSuccess_u As Integer = 0
                Dim TotFailed_u As Integer = 0
                Try
                    ViewState("FileType") = e.File.ContentType

                    If Not (exten.ToLower().Trim() = ".xls" Or exten.ToLower().Trim() = ".xlsx") Then
                        MessageBoxValidation("Please upload excel file", "Validation")
                        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                        Exit Sub

                    End If

                    If e.File.FileName.ToString.ToLower().Trim().EndsWith(".xls") Or e.File.FileName.ToString.ToLower().Trim().EndsWith(".xlsx") Then

                        Dim Foldername As String = System.Configuration.ConfigurationManager.AppSettings("TemporaryFolder")
                        If Not Foldername.EndsWith("\") Then
                            Foldername = Foldername & "\"
                        End If
                        If Directory.Exists(Foldername.Substring(0, Len(Foldername) - 1)) = False Then
                            Directory.CreateDirectory(Foldername.Substring(0, Len(Foldername) - 1))
                        End If
                        If e.File.FileName.ToString.EndsWith(".csv") Then
                            Dim FName As String
                            FName = Now.Hour & Now.Minute & Now.Second & e.File.FileName
                            ViewState("FileName") = Foldername & FName
                            ViewState("CSVName") = FName
                        Else
                            ViewState("FileName") = Foldername & Now.Hour & Now.Minute & Now.Second & e.File.FileName
                        End If

                        e.File.SaveAs(ViewState("FileName"))


                        Try

                            If ViewState("FileType") IsNot Nothing And ViewState("FileName") IsNot Nothing Then





                                Dim TempTbl As New DataTable
                                If TempTbl.Rows.Count > 0 Then
                                    TempTbl.Rows.Clear()
                                End If


                                Dim bRetVal As Boolean = False

                                If ViewState("FileName").ToString.ToLower().Trim().EndsWith(".xls") Then
                                    TempTbl = DoXLSUpload()
                                ElseIf ViewState("FileName").ToString.ToLower().Trim().EndsWith(".xlsx") Then
                                    TempTbl = DoXLSXUpload()
                                End If






                                If TempTbl.Rows.Count = 0 Or TempTbl Is Nothing Then
                                    MessageBoxValidation("Invalid file Template", "Validation")
                                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                                    Exit Sub
                                End If

                                If TempTbl.Columns.Count >= 3 Then
                                    If Not (TempTbl.Columns(0).ColumnName = "Organization_ID" Or TempTbl.Columns(1).ColumnName = "Delivery_Date" Or TempTbl.Columns(2).ColumnName = "Is_Working") Then

                                        MessageBoxValidation("Please check the template columns are correct", "Validation")
                                        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                                        Exit Sub

                                    End If

                                Else
                                    MessageBoxValidation("The exception date template sheet should be 3 column only", "Validation")
                                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                                    Exit Sub
                                End If




                                If TempTbl.Rows.Count = 0 Then
                                    MessageBoxValidation("There is no data in the file.", "Validation")
                                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)

                                    Exit Sub
                                End If
                                dtErrors = Session("dtPrErrors")

                                If dtErrors.Rows.Count > 0 Then
                                    dtErrors.Rows.Clear()
                                End If
                                Dim RowNo As String = Nothing
                                Dim ErrorText As String = Nothing


                                If TempTbl.Rows.Count > 0 Then
                                    Dim idx As Integer

                                    Dim Organization_ID As String = Nothing
                                    Dim Delivery_Date As String = Nothing
                                    Dim Is_Working As String = Nothing


                                    Dim isValidRow As Boolean = True
                                    For idx = 0 To TempTbl.Rows.Count - 1

                                        Dim IfExisits As String = "0"
                                        RowNo = Nothing
                                        ErrorText = Nothing
                                        isValidRow = True

                                        If TempTbl.Rows(idx)(0) Is DBNull.Value Or TempTbl.Rows(idx)(1) Is DBNull.Value Or TempTbl.Rows(idx)(2) Is DBNull.Value Then

                                            RowNo = idx + 2
                                            ErrorText = "Organization,Dates and Is_working are mandatory" + ","
                                            Dim h As DataRow = dtErrors.NewRow()
                                            h("RowNo") = RowNo
                                            h("LogInfo") = ErrorText
                                            dtErrors.Rows.Add(h)
                                            Continue For
                                        End If

                                        Organization_ID = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, Nothing, TempTbl.Rows(idx)(0).ToString().Trim())
                                        Delivery_Date = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, Nothing, TempTbl.Rows(idx)(1).ToString().Trim())
                                        Is_Working = IIf(TempTbl.Rows(idx)(2) Is DBNull.Value, Nothing, TempTbl.Rows(idx)(2).ToString().Trim())



                                        If Organization_ID = "" Or Organization_ID Is Nothing Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + " Organization is mandatory" + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End If
                                        If Not (Organization_ID Is Nothing) Then
                                            If objDlvclndr.IsValidOrganization(Organization_ID) = False Then
                                                RowNo = idx + 2
                                                ErrorText = ErrorText + "Invalid Organization" + ","
                                                isValidRow = False
                                                TotFailed += 1
                                            End If
                                        End If

                                        If Delivery_Date = "0" Or Delivery_Date Is Nothing Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "Date is mandatory" + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        ElseIf IsDate(Delivery_Date) = False Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + " Invalid Date" + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End If



                                        If Is_Working = "" Or Is_Working Is Nothing Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "Is_Working is mandatory" + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End If




                                        If Is_Working = "" Or Is_Working Is Nothing Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "Is_Working is mandatory" + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        ElseIf Is_Working.ToUpper() <> "Y" And Is_Working.ToUpper() <> "N" Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "Invalid Data in Is_Working" + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End If


                                        If Not (RowNo Is Nothing And ErrorText Is Nothing) Then
                                            Dim h As DataRow = dtErrors.NewRow()
                                            h("RowNo") = RowNo

                                            h("LogInfo") = ErrorText
                                            dtErrors.Rows.Add(h)
                                            RowNo = Nothing
                                            ErrorText = Nothing
                                            isValidRow = False
                                        End If




                                        If isValidRow = True Then
                                            Dim dt_exits As New DataTable
                                            dt_exits = objDlvclndr.ExistsDeliveryCalenderDate(Err_No, Err_Desc, Organization_ID, CDate(Delivery_Date).ToString("yyyy-MM-dd"))
                                            If dt_exits Is Nothing Or dt_exits.Rows.Count = 0 Then
                                                If objDlvclndr.ManageDeliveryCalenderDate(Err_No, Err_Desc, "0", Organization_ID, CDate(Delivery_Date).ToString("yyyy-MM-dd"), Is_Working, CType(Session("User_Access"), UserAccess).UserID) Then

                                                    TotSuccess = TotSuccess + 1
                                                    Dim h As DataRow = dtErrors.NewRow()
                                                    h("RowNo") = idx + 2
                                                    h("LogInfo") = "Successfully Inserted"
                                                    dtErrors.Rows.Add(h)
                                                    RowNo = Nothing
                                                    ErrorText = Nothing
                                                    isValidRow = True
                                                Else
                                                    Dim h As DataRow = dtErrors.NewRow()
                                                    h("RowNo") = idx + 2
                                                    h("LogInfo") = Err_Desc + "Error occured while saving this row"
                                                    dtErrors.Rows.Add(h)
                                                    RowNo = Nothing
                                                    ErrorText = Nothing
                                                    isValidRow = True
                                                End If
                                            Else
                                                If objDlvclndr.ManageDeliveryCalenderDate(Err_No, Err_Desc, dt_exits.Rows(0)("Row_ID"), Organization_ID, CDate(Delivery_Date).ToString("yyyy-MM-dd"), Is_Working, CType(Session("User_Access"), UserAccess).UserID) Then

                                                    TotSuccess = TotSuccess + 1
                                                    Dim h As DataRow = dtErrors.NewRow()
                                                    h("RowNo") = idx + 2
                                                    h("LogInfo") = "Successfully Updated"
                                                    dtErrors.Rows.Add(h)
                                                    RowNo = Nothing
                                                    ErrorText = Nothing
                                                    isValidRow = True
                                                Else
                                                    Dim h As DataRow = dtErrors.NewRow()
                                                    h("RowNo") = idx + 2
                                                    h("LogInfo") = "Error occured while saving this row"
                                                    dtErrors.Rows.Add(h)
                                                    RowNo = Nothing
                                                    ErrorText = Nothing
                                                    isValidRow = True
                                                End If
                                            End If
                                        End If

                                    Next
                                End If



                                Resetfields()
                                BindGrid() ' rgProducts.Rebind()
                                UpdatePanel1.Update()
                                ClassUpdatePnl.Update()


                                Session.Remove("dtPrErrors")
                                Session("dtPrErrors") = dtErrors




                                Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & "ExceptionDateLog" & Now.ToString("yyyyMMdd") + ".txt"




                                dtErrors.TableName = "ExceptionDates"



                                DataTable2CSV(dtErrors, fn, vbTab)

                                Session.Remove("ExDateLogFile")
                                Session("ExDateLogFile") = fn



                                If TotSuccess > 0 Then
                                    DeleteExcel()
                                    ''  BindGrid() ' rgProducts.Rebind()
                                    '' ClassUpdatePnl.Update()
                                    Dim lblinfo As String = "Information"
                                    Dim lblMessage As String = IIf(TotFailed = 0, "Successfully uploaded", "One or more rows has invalid data. Please check the log")

                                    MessageBoxValidation(lblMessage, lblinfo)

                                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                                    Exit Sub
                                Else

                                    MessageBoxValidation("Please check the uploaded log file", "Validation")
                                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                                    Exit Sub
                                End If
                            End If

                        Catch ex As Exception

                            Err_No = "92823"
                            If Err_Desc Is Nothing Then
                                log.Error(GetExceptionInfo(ex))
                            Else
                                log.Error(Err_Desc)
                            End If
                        End Try


                    Else
                        Str.Append("<span style='font-color:red'> <b >Please import valid Excel template.</b></span>")
                        MessageBoxValidation(Str.ToString(), "Validation")
                        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                    End If

                Catch ex As Exception
                    log.Error(GetExceptionInfo(ex))
                End Try
            End If


        Catch ex As Exception
            MessageBoxValidation("Error occured while uploading file", "Validation")
            log.Error(ex.Message)
        End Try

    End Sub
    Protected Sub lbLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbLog.Click
        If Not Session("ExDateLogFile") Is Nothing Then
            Dim fileValue As String = Session("ExDateLogFile")

            Dim file As System.IO.FileInfo = New FileInfo(fileValue)

            If file.Exists Then

                'Process.Start("notepad.exe", fileValue)
                Response.Clear()

                Response.AddHeader("Content-Disposition", "attachment; filename=" & file.Name)

                Response.AddHeader("Content-Length", file.Length.ToString())

                Response.WriteFile(file.FullName)

                Response.[End]()

            Else
                MessageBoxValidation("File does not exist", "Information")
                Exit Sub

            End If
        Else
            MessageBoxValidation("There is no log to view.", "Information")
            Exit Sub

        End If

    End Sub

    Private Function DoXLSUpload() As DataTable
        Dim dtImport As New DataTable
        Try

            Dim connString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & ViewState("FileName") & " ;Extended Properties=""Excel 8.0;"""
            Dim oledbConn As New OleDbConnection(connString)
            Try
                oledbConn.Open()
                Dim cmd As New OleDbCommand("SELECT * FROM [Exceptions$]", oledbConn)
                Dim oleda As New OleDbDataAdapter()
                oleda.SelectCommand = cmd
                oleda.Fill(dtImport)
                'If dtImport.Columns.Count = 6 Then
                '    If dtImport.Columns(0).ColumnName.ToUpper = "VAN_CODE" And dtImport.Columns(1).ColumnName.ToUpper = "ITEM_CODE" And dtImport.Columns(2).ColumnName.ToUpper = "LOT_NUMBER" And dtImport.Columns(3).ColumnName.ToUpper = "EXPIRY_DATE" And dtImport.Columns(4).ColumnName.ToUpper = "LOT_QTY" And dtImport.Columns(5).ColumnName.ToUpper = "UOM" Then
                '        bfileformat = True
                '    End If
                'End If
            Catch ex As Exception
                log.Error(GetExceptionInfo(ex))
                log.Error(ex.Message.ToString())
            Finally
                ' Close connection
                oledbConn.Close()
            End Try
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
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

                Dim cmd As New OleDbCommand("SELECT * FROM [Exceptions$]", oledbConn)

                Dim oleda As New OleDbDataAdapter()

                oleda.SelectCommand = cmd



                oleda.Fill(dtImport)


                'If dtImport.Columns.Count = 6 Then
                '    If dtImport.Columns(0).ColumnName.ToUpper = "VAN_CODE" And dtImport.Columns(1).ColumnName.ToUpper = "ITEM_CODE" And dtImport.Columns(2).ColumnName.ToUpper = "LOT_NUMBER" And dtImport.Columns(3).ColumnName.ToUpper = "EXPIRY_DATE" And dtImport.Columns(4).ColumnName.ToUpper = "LOT_QTY" And dtImport.Columns(5).ColumnName.ToUpper = "UOM" Then
                '        bfileformat = True
                '    End If
                'End If
            Catch ex As Exception
                log.Error(GetExceptionInfo(ex))
                log.Error(ex.Message.ToString())
            Finally
                ' Close connection
                oledbConn.Close()
            End Try
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
            Throw ex
        End Try
        Return dtImport
    End Function

    Protected Sub btndownloadTemplate_Click(sender As Object, e As EventArgs) Handles btndownloadTemplate.Click

        Dim Filename As String = System.Configuration.ConfigurationManager.AppSettings("ExcelTemplatePath") & "Exceptions.xls"
        Dim TheFile As FileInfo = New FileInfo(Filename)
        If TheFile.Exists Then
            Dim strFileName As String = "Template" + Now.ToString("ddMMMyyHHmmss") + ".xls"


            ViewState("SampleTemplate") = strFileName

            Dim sFileName As String = strFileName
            Dim sFullPath As String = Filename
            Dim fileBytes As Byte() = System.IO.File.ReadAllBytes(sFullPath)

            Dim context As System.Web.HttpContext = System.Web.HttpContext.Current
            context.Response.ContentType = "application/vnd.ms-excel"
            context.Response.AddHeader("Content-Disposition", "attachment;filename=" + sFileName)
            context.Response.Clear()
            context.Response.BinaryWrite(fileBytes)
            context.Response.Flush()
            context.ApplicationInstance.CompleteRequest()
            DeleteExcelTemplate()
        End If

    End Sub
    Private Sub DeleteExcelTemplate()
        Try
            Dim Filename As String = ViewState("SampleTemplate")

            Dim TheFile As FileInfo = New FileInfo(Filename)
            If TheFile.Exists Then
                File.Delete(Filename)
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))

        End Try
    End Sub

    Private Function SetErrorsTable() As DataTable
        Dim col As DataColumn
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

        Session.Remove("dtPrErrors")
        Session("dtPrErrors") = dtErrors
        Return dtErrors
    End Function
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

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Try
            If ddlOrganization.SelectedIndex > 0 Then
                Dim dt_exdates As New DataTable

                Dim Fromdate As Date
                Dim Todate As Date

                Fromdate = CDate(txtFromDate.SelectedDate)
                Todate = CDate(txtToDate.SelectedDate)


                If Session("Search") = "1" Then
                    dt_exdates = objDlvclndr.SerachDeliveryCalender(Err_No, Err_Desc, ddlOrganization.SelectedValue, Fromdate.ToString("dd-MMM-yyyy"), Todate.ToString("dd-MMM-yyyy"), IIf(ddFilterBy.SelectedIndex = 0, "ALL", ddFilterBy.SelectedValue))
                Else
                    dt_exdates = objDlvclndr.GetDeliveryCalender(Err_No, Err_Desc, ddlOrganization.SelectedValue, "DATE")
                End If


                Dim tblData As New DataTable
                tblData = dt_exdates.DefaultView.ToTable(False, "Organization_ID", "Delivery_Date", "Is_Working")




                If tblData.Rows.Count > 0 Then


                    Using package As New ExcelPackage()

                        Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Exceptions")
                        Worksheet.Cells("A1").LoadFromDataTable(tblData, True)
                        Worksheet.Column(2).Style.Numberformat.Format = "MM/dd/yyyy"
                        Worksheet.Cells.AutoFitColumns()
                        Response.Clear()
                        Response.Buffer = True
                        Response.Charset = ""

                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                        Response.AddHeader("content-disposition", "attachment;filename= Exceptions.xlsx")

                        Using MyMemoryStream As New MemoryStream()
                            package.SaveAs(MyMemoryStream)
                            MyMemoryStream.WriteTo(Response.OutputStream)
                            Response.AddHeader("Content-Length", MyMemoryStream.Length)
                            Response.Flush()
                            Response.Close()
                        End Using
                    End Using
                End If
            Else

                MessageBoxValidation("Please Select Organization.", "Information")
                Exit Sub
            End If

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub

    'Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
    '    Resetfields()
    '    Me.MPEDetails.VisibleOnPageLoad = False
    'End Sub

  

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs)
        Try
            Resetfields()
            Me.MPEDetails.VisibleOnPageLoad = False
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
End Class