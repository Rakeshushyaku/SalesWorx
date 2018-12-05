Imports SalesWorx.BO.Common
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Data
Imports System.Resources
Imports log4net

Partial Public Class PlansForApproval
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Private Const PageID As String = "P69"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objLogin As New SalesWorx.BO.Common.Login

    Private Sub PlansForApproval_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Route Planner"
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
            Dim objRoute As New RoutePlan
            Try
                'Dim UD_SUB_QRY As String = "SELECT AB.SalesRep_ID FROM TBL_User_FSR_Map As AB WHERE AB.User_ID=" & CType(Session("User_Access"), UserAccess).UserID
                Dim UD_SUB_QRY = "select SalesRep_ID from dbo.app_GetControlInfo(" & CType(Session("User_Access"), UserAccess).UserID & ")"
                Session.Remove("AppTable")
                Session("AppTable") = objRoute.ShowPlanListForApprovalByUD(Err_No, Err_Desc, UD_SUB_QRY)
                If (CType(Session("AppTable"), DataTable).Rows.Count <> 0) Then
                    ApprovalPlans.DataSource = CType(Session("AppTable"), DataTable)
                    ApprovalPlans.DataBind()
                    MsgLbl.Text = ""
                    MsgLbl.Visible = False
                Else
                    MsgLbl.Text = "No plans available for approval."
                    MsgLbl.Visible = True
                End If
            Catch ex As Exception
                log.Error(GetExceptionInfo(ex))
                Response.Redirect("Information.aspx?mode=1&errno=" & "74060" & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Route+Planner", False)
            Finally
                objRoute = Nothing
            End Try
        End If
    End Sub
    Private Sub ApprovalPlans_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles ApprovalPlans.RowCommand
        Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
        Dim FSR_ID As Integer = Convert.ToInt32(ApprovalPlans.DataKeys(row.RowIndex).Value)
        Dim objRoute As New RoutePlan
        ' Dim UD_SUB_QRY As String = Utils.GetUDSubQuery(Session("USER_ACCESS").Designation, Session("USER_ACCESS").Site, Session("USER_ACCESS").OrgId, Session("USER_ACCESS").SalesRepId)
        Dim UD_SUB_QRY = "select SalesRep_ID from dbo.app_GetControlInfo(" & CType(Session("User_Access"), UserAccess).UserID & ")"
        If Session("AppTable") Is Nothing Then
            Session("AppTable") = objRoute.ShowPlanListForApprovalByUD(Err_No, Err_Desc, UD_SUB_QRY)
        End If
        Dim results() As DataRow = CType(Session("AppTable"), DataTable).Select("FSR_Plan_ID=" & FSR_ID & "")
        Try
            If (e.CommandName = "Approve") Then
                Session("Default_Plan_ID") = results(0).Item(5)
                Session("FSR_ID") = FSR_ID
                Session("SalesRep_Name") = results(0).Item(1)
                Session("SalesRep_ID") = results(0).Item(6)
                Session("ISApproved") = "N"
                Session("RedirectTo") = "PlansForApproval.aspx"
                Response.Redirect("AdminRoutePlan.aspx?Mode=APPROVE", False)
            ElseIf (e.CommandName = "Approval") Then
                ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())

                objRoute.FSRPlanId = FSR_ID
                objRoute.ApprovedBy = CType(Session("USER_ACCESS"), UserAccess).UserID
                If objRoute.ApproveRoutePlan(Err_No, Err_Desc) Then
                    ' Dim s As String() = row.Cells(3).Text.Split("-")
                    Dim OID As String = row.Cells(3).Text
                    '  If s.Length > 1 Then
                    'OID = s(1)
                    ' End If
                    ' Dim s1 As String() = results(0).Item(1).Split("-")
                    Dim VID As String = results(0).Item(6)
                    ' If s1.Length > 1 Then
                    'VID = s1(1)
                    ' End If
                    objLogin.SaveUserLog(Err_No, Err_Desc, "A", "CALL PLANNING", "APPROVE ROUTE PLAN", VID.Trim(), "Desc: " & row.Cells(2).Text.Replace("&gt;", "") & "/ No.Of Visits :  " & row.Cells(5).Text & "/ Status : Approved", CType(Session("User_Access"), UserAccess).UserID.ToString(), VID.Trim(), OID.Trim)
                    ConfirmationMsg.Text = "<span class='message'>FSR Plan has been approved successfully</span>"
                    ApprovalPlans.DataSource = objRoute.ShowPlanListForApprovalByUD(Err_No, Err_Desc, UD_SUB_QRY)
                    ApprovalPlans.DataBind()
            Else
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=RepPlansForApprovalNew.aspx", False)
            End If
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("information.aspx?mode=1&errno=" & "74061" & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=RepPlansForApprovalNew.aspx", False)
        Finally
            objRoute = Nothing
        End Try


    End Sub
End Class