Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net

Partial Public Class ApproveRoutePlan
    Inherits System.Web.UI.Page
    Dim Err_No As Integer
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim Dt As New DataTable
    Private Const PageID As String = "P68"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub ApproveRoutePlan_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Route Planner"
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        Dim ObjRoute As New RoutePlan
        ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
        If (Not IsPostBack) Then
            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            If Not HasPermission Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If
            Try
                ''*** Original**  Dim UD_SUB_QRY As String = Utils.GetUDSubQuery(Session("USER_ACCESS").Designation, Session("USER_ACCESS").Site, Session("USER_ACCESS").OrgId, Session("USER_ACCESS").SalesRepId)
                'Dim UD_SUB_QRY As String = "SELECT AB.SalesRep_ID FROM TBL_Org_CTL_DTL As AB WHERE AB.Org_Id=102 "
                'Route_FSR_ID.DataSource = ObjRoute.ShowSalesRepsByUD(Err_No, Err_Desc, UD_SUB_QRY)
                'Route_FSR_ID.DataBind()

                Route_FSR_ID.DataSource = BindFSR()
                Route_FSR_ID.DataBind()
                RoutePlan.Items.Insert(0, New ListItem("  -- Select a route plan -- "))

                If Not Err_Desc Is Nothing Then
                    log.Error(Err_Desc)
                    Response.Redirect("information.aspx?mode=1&errno=" & "74056" & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Route+Planner", False)
                End If
            Catch ex As Exception
                log.Error(GetExceptionInfo(ex))
                Response.Redirect("information.aspx?mode=1&errno=" & "74057" & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Route+Planner", False)
            Finally
                ObjRoute = Nothing
                ErrorResource = Nothing
            End Try

        End If
    End Sub
    Private Function BindFSR() As DataTable
        Try
            Dim TempTbl As New DataTable
            Dim MyRow As DataRow
            TempTbl.Columns.Add(New DataColumn("SalesRep_ID", _
                GetType(Int32)))
            TempTbl.Columns.Add(New DataColumn("SalesRep_Name", _
                GetType(String)))
            Dim obj As UserAccess
            obj = CType(Session("User_Access"), UserAccess)
            If obj.AssignedSalesReps.Count = obj.VanName.Count Then
                For i = 0 To obj.AssignedSalesReps.Count - 1
                    If obj.AssignedSalesReps(i) IsNot DBNull.Value And obj.VanName(i) IsNot DBNull.Value Then
                        MyRow = TempTbl.NewRow()
                        MyRow(0) = obj.AssignedSalesReps(i)
                        MyRow(1) = obj.VanName(i)
                        TempTbl.Rows.Add(MyRow)
                    End If
                Next
            End If

            Return TempTbl
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Protected Sub Route_FSR_ID_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Route_FSR_ID.SelectedIndexChanged
        Dim ObjRoute As New RoutePlan
        ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
        Try
            If (Route_FSR_ID.SelectedItem.Text <> "-- Select a Van --") Then
                ObjRoute.SalesRepID = Route_FSR_ID.SelectedItem.Value
                Session("FSRTable") = ObjRoute.ShowFSRPlans(Err_No, Err_Desc)
                Dt = CType(Session("FSRTable"), DataTable)

                RoutePlan.DataSource = Dt
                RoutePlan.DataBind()
                RoutePlan.Items.Insert(0, New ListItem("  -- Select a route plan -- "))
                Dim i As Integer
                For i = 0 To RoutePlan.Items.Count - 2
                    Dim Color As String
                    If (Dt.Rows(i).Item(2).ToString = "N") Then
                        Color = "Color:Black"
                    ElseIf (Dt.Rows(i).Item(2).ToString = "Y") Then
                        Color = "Color:Green"
                    ElseIf (Dt.Rows(i).Item(2).ToString = "R") Then
                        Color = "Color:Red"
                    ElseIf (Dt.Rows(i).Item(2).ToString = "U") Then
                        Color = "Color:Blue"
                    End If
                    RoutePlan.Items(i + 1).Attributes.Add("style", Color)
                Next
            Else
                RoutePlan.Items.Clear()
                RoutePlan.Items.Insert(0, New ListItem("  -- Select a route plan -- "))
            End If
            If Not Err_Desc Is Nothing Then
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=PlansForApproval.aspx&Title=Route+Planner", False)
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("information.aspx?mode=1&errno=" & "74057" & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=PlansForApproval.aspx&Title=Route+Planner", False)
        Finally
            ObjRoute = Nothing
        End Try

    End Sub


    Private Sub ViewAppBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ViewAppBtn.Click
        Dim ObjRoute As New RoutePlan
        Dim FSRDt As New DataTable

        Try
            If (Session("FSRTable") Is Nothing) Then
                ObjRoute.SalesRepID = Route_FSR_ID.SelectedItem.Value
                Session("FSRTable") = ObjRoute.ShowFSRPlans(Err_No, Err_Desc)
            End If

            FSRDt = CType(Session("FSRTable"), DataTable)
            Dim results() As DataRow = CType(Session("FSRTable"), DataTable).Select("FSR_Plan_ID=" & RoutePlan.SelectedItem.Value & "")
            Session("Default_Plan_ID") = results(0).Item(3)
            Session("FSR_ID") = RoutePlan.SelectedItem.Value
            Session("SalesRep_Name") = Route_FSR_ID.SelectedItem.Text.Trim()
            Session("SalesRep_ID") = Route_FSR_ID.SelectedItem.Value.Trim()
            Session("ApproveStat") = results(0).Item(2)
            '  Session("Approved_By") = results(0).Item(4)
            Session("RedirectTo") = "ApproveRoutePlan.aspx"
            Response.Redirect("AdminRoutePlan.aspx?Mode=APPROVE", False)
        Catch ex As Exception
        Finally
            ObjRoute = Nothing
        End Try

    End Sub

End Class