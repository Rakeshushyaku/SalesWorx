Imports SalesWorx.BO.Common
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Data
Imports System.Resources
Imports log4net
Imports OfficeOpenXml
Imports System.IO

Public Class ReviewRoutePlan
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Private Const PageID As String = "P79"
    Private dtSearch As New DataTable
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private Sub ReviewRoutePlan_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Review Route Plan"
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

            BindData()
        Else
            Me.MapWindow.VisibleOnPageLoad = False
        End If
    End Sub


    Private Sub ApprovalPlans_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles ApprovalPlans.PageIndexChanging
        ApprovalPlans.PageIndex = e.NewPageIndex
        BindData()
    End Sub

    Private Sub dgv_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles ApprovalPlans.RowDataBound
        If e.Row.RowType.Equals(DataControlRowType.Pager) Then
            Dim pTableRow As TableRow = _
                     CType(e.Row.Cells(0).Controls(0).Controls(0), TableRow)
            For Each cell As TableCell In pTableRow.Cells
                For Each control As Control In cell.Controls
                    If TypeOf control Is LinkButton Then
                        Dim lb As LinkButton = CType(control, LinkButton)
                        'lb.Attributes.Add("onclick", "ScrollToTop();")
                    End If
                Next
            Next
        End If
    End Sub

    Private Sub dgv_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles ApprovalPlans.Sorting
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

    Private Sub BindData()
        Dim objRoute As New RoutePlan
        Try
            'Dim UD_SUB_QRY As String = "SELECT AB.SalesRep_ID FROM TBL_User_FSR_Map As AB WHERE AB.User_ID=" & CType(Session("User_Access"), UserAccess).UserID
            Dim UD_SUB_QRY = "select SalesRep_ID from dbo.app_GetControlInfo(" & CType(Session("User_Access"), UserAccess).UserID & ")"
            Session.Remove("AppTable")
            Session("AppTable") = objRoute.ShowPlanListForReveiwByUD(Err_No, Err_Desc, UD_SUB_QRY)
            If (CType(Session("AppTable"), DataTable).Rows.Count <> 0) Then
                dtSearch = CType(Session("AppTable"), DataTable)




                Dim dv As New DataView(dtSearch)
                If ViewState("SortField") <> "" Then
                    dv.Sort = (ViewState("SortField") & " ") + SortDirection
                End If
                Me.ApprovalPlans.DataSource = Nothing
                Me.ApprovalPlans.DataSource = dv
                Me.ApprovalPlans.DataBind()

                Dim month_f As String = ""

                If Not Me.MonthPicker.SelectedDate Is Nothing Then
                    dtSearch.DefaultView.RowFilter = "(Start_Date_Month =" & Me.MonthPicker.SelectedDate.Value.Month & " And Start_Date_Year=" & Me.MonthPicker.SelectedDate.Value.Year & ")"
                    month_f = " AND (Start_Date_Month =" & Me.MonthPicker.SelectedDate.Value.Month & " And Start_Date_Year=" & Me.MonthPicker.SelectedDate.Value.Year & ")"
                End If

                If Me.ddFilterBy.SelectedIndex = 2 Then
                    If Me.txtFilterValue.Text <> "" Then
                        dtSearch.DefaultView.RowFilter = "(SalesRep_Name LIKE '%" & Me.txtFilterValue.Text & "%')" & month_f
                        Me.txtFilterValue.Text = ""
                    End If
                End If

                If Me.ddFilterBy.SelectedIndex = 1 Then
                    If Me.txtFilterValue.Text <> "" Then
                        dtSearch.DefaultView.RowFilter = "(Route_Plan LIKE '%" & Me.txtFilterValue.Text & "%')" & month_f
                        Me.txtFilterValue.Text = ""
                    End If
                End If

                If Me.ddFilterBy.SelectedIndex = 3 Then
                    If Me.txtFilterValue.Text <> "" Then
                        dtSearch.DefaultView.RowFilter = "(Status LIKE '%" & Me.txtFilterValue.Text & "%')" & month_f
                        Me.txtFilterValue.Text = ""
                    End If
                End If

               


                MsgLbl.Text = ""
                MsgLbl.Visible = False

                dv = dtSearch.DefaultView
                If ViewState("SortField") <> "" Then
                    dv.Sort = (ViewState("SortField") & " ") + SortDirection
                End If
                Me.ApprovalPlans.DataSource = Nothing
                Me.ApprovalPlans.DataSource = dv
                Me.ApprovalPlans.DataBind()
                TopPanel.Update()
                UpdatePanel2.Update()
            Else
                MsgLbl.Text = "No plans available for review."
                MsgLbl.Visible = True
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("Information.aspx?mode=1&errno=" & "74060" & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Route+Planner", False)
        Finally
            objRoute = Nothing
        End Try
    End Sub


    Private Sub ApprovalPlans_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles ApprovalPlans.RowCommand
        Dim objRoute As New RoutePlan
        Try
            If (e.CommandName = "Review") Then
                Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim FSR_ID As Integer = Convert.ToInt32(ApprovalPlans.DataKeys(row.RowIndex).Value)

                ' Dim UD_SUB_QRY As String = Utils.GetUDSubQuery(Session("USER_ACCESS").Designation, Session("USER_ACCESS").Site, Session("USER_ACCESS").OrgId, Session("USER_ACCESS").SalesRepId)
                Dim UD_SUB_QRY = "select SalesRep_ID from dbo.app_GetControlInfo(" & CType(Session("User_Access"), UserAccess).UserID & ")"
                If Session("AppTable") Is Nothing Then
                    Session("AppTable") = objRoute.ShowPlanListForReveiwByUD(Err_No, Err_Desc, UD_SUB_QRY)
                End If
                Dim results() As DataRow = CType(Session("AppTable"), DataTable).Select("FSR_Plan_ID=" & FSR_ID & "")
                Session("Default_Plan_ID") = results(0).Item(5)
                Session("FSR_ID") = FSR_ID
                Session("SalesRep_Name") = results(0).Item(1)
                Session("SalesRep_ID") = results(0).Item(6)
                Session("ISApproved") = "N"
                Session("RedirectTo") = "ReviewRoutePlan.aspx"
                Response.Redirect("AdminRoutePlan.aspx?Mode=REVIEW", False)
                'ElseIf (e.CommandName = "Approval") Then
                '    ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())

                '    objRoute.FSRPlanId = FSR_ID
                '    objRoute.ApprovedBy = CType(Session("USER_ACCESS"), UserAccess).UserID
                '    If objRoute.ApproveRoutePlan(Err_No, Err_Desc) Then
                '        ConfirmationMsg.Text = "<span class='message'>FSR Plan has been approved successfully</span>"
                '        ApprovalPlans.DataSource = objRoute.ShowPlanListForApprovalByUD(Err_No, Err_Desc, UD_SUB_QRY, "Review Plan")
                '        ApprovalPlans.DataBind()
                '    Else
                '        log.Error(Err_Desc)
                '        Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=RepPlansForApprovalNew.aspx", False)
                '    End If
            End If
            If (e.CommandName = "ViewStatus") Then
                Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim FSR_ID As Integer = Convert.ToInt32(ApprovalPlans.DataKeys(row.RowIndex).Value)
                Dim lb As LinkButton = DirectCast(row.FindControl("SalesLink"), LinkButton)
                Me.hfPlanID.Value = FSR_ID
                rgVisits.Rebind()

                ScriptManager.RegisterStartupScript(Me, Page.GetType(), "openPopUp", "javascript:OpenLocWindow(" & FSR_ID & ");", True)




                ' MapWindow.Title = "FSR : " & lb.Text & " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Route Plan : " & row.Cells(2).Text
                '  Me.MapWindow.VisibleOnPageLoad = True
            End If
             
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("information.aspx?mode=1&errno=" & "74061" & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=Welcome.aspx", False)
        Finally
            objRoute = Nothing
        End Try


    End Sub
    Private Sub BtnExportBiffExcel_Click(sender As Object, e As EventArgs) Handles BtnExportBiffExcel.Click
        
        Dim dtRouteplan As New DataSet
        dtRouteplan = (New SalesWorx.BO.Common.RoutePlan).RouteplanForExport(Err_No, Err_Desc, H_FSRPLANID.Value)
        If dtRouteplan.Tables.Count = 2 Then
            If dtRouteplan.Tables(1).Rows.Count > 0 Then

                Using package As New ExcelPackage()
                    ' add a new worksheet to the empty workbook
                    Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")

                    If dtRouteplan.Tables(0).Rows.Count > 0 Then
                        Worksheet.Cells("A5").LoadFromDataTable(dtRouteplan.Tables(1), True)
                        Worksheet.Cells("B1").Value = "Van"
                        Worksheet.Cells("C1").Value = dtRouteplan.Tables(0).Rows(0)("SalesRep_Name").ToString
                        Worksheet.Cells("B2").Value = "Description"
                        Worksheet.Cells("C2").Value = dtRouteplan.Tables(0).Rows(0)("Description").ToString
                        Worksheet.Cells("B3").Value = "Start Date"
                        Worksheet.Cells("C3").Value = CDate(dtRouteplan.Tables(0).Rows(0)("Start_Date").ToString).ToString("dd-MMM-yyyy")
                        
                    Else
                        Worksheet.Cells("A1").LoadFromDataTable(dtRouteplan.Tables(1), True)
                    End If

                    Worksheet.Cells.AutoFitColumns()
                    Worksheet.Column(1).Style.Numberformat.Format = "dd-MMM-yyyy"
                    Response.Clear()
                    Response.Buffer = True
                    Response.Charset = ""

                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    Response.AddHeader("content-disposition", "attachment;filename= RoutePlan.xlsx")

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
            MessageBoxValidation("Unexpected Error occured", "Information")
        End If
    End Sub
    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFilter.Click
        BindData()
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        Try
            Me.ddFilterBy.SelectedIndex = 0
            Me.txtFilterValue.Text = ""
            Me.MonthPicker.SelectedDate = Nothing
            BindData()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
End Class
