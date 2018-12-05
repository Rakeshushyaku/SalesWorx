Imports SalesWorx.BO.Common
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Data
Imports System.Resources
Imports log4net
Imports System.Configuration.ConfigurationManager
Partial Public Class PlansForApproval
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Private Const PageID As String = "P69"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim _strReceipient As String = AppSettings("PUSHRECIPIENT")
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
                    Dim Van_Number As String = ""
                    Dim objCommon As New SalesWorx.BO.Common.Common
                    Dim objControl As New SalesWorx.BO.Common.AppControl
                    Dim dtvan As New DataTable
                    dtvan = objCommon.GetVanfromSalesRepID(Err_No, Err_Desc, VID)
                    If dtvan.Rows.Count > 0 Then
                        Van_Number = dtvan.Rows(0)("SalesRep_Number").ToString
                    End If
                    'Task 3  - By Rakesh for autoPush Notification on the bases of control values 
                    Dim dt As New DataTable
                    Dim PUSH_NOTIFIC_ON_ROUTE_APPROVAL = objCommon.GetAppConfig(Err_No, Err_Desc, "PUSH_NOTIFIC_ON_ROUTE_APPROVAL")
                    If PUSH_NOTIFIC_ON_ROUTE_APPROVAL = "Y" Then

                        ' code to get details for auto push
                        Dim dt1 As New DataTable
                        dt1 = objCommon.GetValueForPushNotification(Err_No, "PUSH_NOTIFIC_ON_ROUTE_APPROVAL", Err_Desc)
                        If dt1.Rows.Count > 0 Then
                            Dim Title As String = dt1.Rows(0)("Code_Description").ToString()
                            Dim Description As String = dt1.Rows(0)("Custom_Attribute_1").ToString()
                            Dim UsersList As String = Nothing
                            UsersList = UsersList + "," + _strReceipient & "_" & Van_Number

                            If objControl.SavePushNotification(Err_No, Err_Desc, Title.Trim, Description.Trim, "USER", UsersList, CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
                                ' MessageBoxValidation("Successfully saved.", "Information")
                            Else
                                ' MessageBoxValidation("Error occured while saving.", "Validation")
                                Exit Sub
                            End If
                        End If
                    End If

                    'end Task 3 

                    objLogin.SaveUserLog(Err_No, Err_Desc, "A", "CALL PLANNING", "APPROVE ROUTE PLAN", VID.Trim(), "Desc: " & row.Cells(2).Text.Replace("&gt;", "") & "/ No.Of Visits :  " & row.Cells(5).Text & "/ Status : Approved", CType(Session("User_Access"), UserAccess).UserID.ToString(), VID.Trim(), OID.Trim)
                    '' ConfirmationMsg.Text = "<span class='message'>FSR Plan has been approved successfully</span>"
                    MessageBoxValidation("FSR Plan has been approved successfully", "Information")
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

    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        str = str.Replace("'", "&#39;")
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
End Class