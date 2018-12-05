Imports SalesWorx.BO.Common
Imports System.Data
Imports System.Web
Imports System.Resources
Imports System.Web.UI.WebControls
Imports log4net

Partial Public Class ModDelRoutePlan
    Inherits System.Web.UI.Page
    Dim objRoute As New RoutePlan
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim Dt As New DataTable
    Private Const PageID As String = "P67"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objLogin As New SalesWorx.BO.Common.Login
    Private Sub ModDelRoutePlan_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Route Planner"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        ModBtn.Attributes.Add("OnClick", "return CheckDropDown();")
        DelBtn.Attributes.Add("OnClick", "return CheckDelItem();")
        If (Not IsPostBack) Then
            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            If Not HasPermission Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If
            Try
                '*** original*** Session("ROUTE_FSR_ID") = Session("USER_ACCESS").SalesRepID

                objRoute.DefPlanId = 0
                Session.Remove("FSRTable")
                If (Request.QueryString("Mode") = "Del") Then
                    If Not (Session("RP_ID") = Nothing) Then
                        objRoute.FSRPlanId = Session("RP_ID")
                        If (objRoute.DeleteFSRPlanID(Err_No, Err_Desc)) Then
                            Response.Redirect("information.aspx?mode=0&msg=Selected+plan+is+deleted+successfully&next=ModDelRoutePlan.aspx&Title=Route+Planner", False)
                        Else
                            log.Error(Err_Desc)
                            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=ModDelRoutePlan.aspx&Title=Route+Planner", False)
                        End If
                        HttpContext.Current.ApplicationInstance.CompleteRequest()
                    End If
                End If

                If CType(Session("USER_ACCESS"), UserAccess).IsSS <> "N" Then
                    VanPanel.Visible = True
                    ''*** orginal**  ' Dim UD_SUB_QRY As String = Utils.GetUDSubQuery(Session("USER_ACCESS").Designation, Session("USER_ACCESS").Site, Session("USER_ACCESS").OrgId, Session("USER_ACCESS").SalesRepId)
                    'Dim UD_SUB_QRY As String = "SELECT AB.SalesRep_ID FROM TBL_Org_CTL_DTL As AB WHERE AB.Org_Id=102 "
                    'Route_FSR_ID.DataSource = objRoute.ShowSalesRepsByUD(Err_No, Err_Desc, UD_SUB_QRY)
                    'Route_FSR_ID.DataBind()
                    ' Route_FSR_ID.DataSource = BindFSR()
                    ' Route_FSR_ID.DataBind()
                    Dim UD_SUB_QRY = "select SalesRep_ID from dbo.app_GetControlInfo(" & CType(Session("User_Access"), UserAccess).UserID & ")"
                    Route_FSR_ID.DataSource = objRoute.ShowSalesRepsByUD(Err_No, Err_Desc, UD_SUB_QRY)
                    Route_FSR_ID.DataBind()

                Else
                    VanPanel.Visible = False
                    BtnPanel.Visible = True

                    objRoute.SalesRepID = CType(Session("User_Access"), UserAccess).SalesRepID
                    objRoute.Site = CType(Session("USER_ACCESS"), UserAccess).Site(0)
                    Session("SalesRep_ID") = CType(Session("User_Access"), UserAccess).SalesRepID
                    Session("FSRTable") = objRoute.ShowFSRPlans(Err_No, Err_Desc)

                    Dt = CType(Session("FSRTable"), DataTable)
                    Route_ID.DataSource = Dt
                    Route_ID.DataBind()
                    Route_ID.Items.Insert(0, New ListItem("  -- Select an existing plan -- "))
                    Dim i As Integer
                    For i = 0 To Route_ID.Items.Count - 2
                        Dim Color As String
                        If (Dt.Rows(i).Item(2).ToString = "N") Then
                            Color = "Color:Black"
                        ElseIf (Dt.Rows(i).Item(2).ToString = "Y") Then
                            Color = "Color:Green"
                        ElseIf (Dt.Rows(i).Item(2).ToString = "R") Then
                            Color = "Color:Red"
                        End If
                        Route_ID.Items(i + 1).Attributes.Add("style", Color)
                    Next
                    If Route_ID.Items.Count > 0 Then
                        BtnPanel.Visible = True
                    Else
                        BtnPanel.Visible = False
                    End If
                End If

            Catch ex As Exception
                log.Error(GetExceptionInfo(ex))
                Err_No = "74051"
                Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Route+Planner")
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
    Protected Sub ModBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ModBtn.Click
        Try
            Dim FSRDt As New DataTable
            If (Session("FSRTable") Is Nothing) Then
                Session("FSRTable") = objRoute.ShowFSRPlans(Err_No, Err_Desc)
            End If

            FSRDt = CType(Session("FSRTable"), DataTable)

            Dim results() As DataRow = CType(Session("FSRTable"), DataTable).Select("FSR_Plan_ID=" & Route_ID.SelectedItem.Value & "")
            Session("Default_Plan_ID") = results(0).Item(3)
            Session("FSR_ID") = Route_ID.SelectedItem.Value
            Session("SalesRep_Name") = Route_FSR_ID.SelectedItem.Text
            Session.Remove("FSRTable")
            Session("ISApproved") = results(0).Item(2)
            Response.Redirect("AdminRoutePlan.aspx?Mode=MODIFY", False)
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Err_No = "74052"
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=ModDelRoutePlan.aspx&Title=Route+Planner")
        End Try
    End Sub

    Protected Sub DelBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles DelBtn.Click
        Session("RP_ID") = Route_ID.SelectedItem.Value
        Session.Remove("FSRTable")
        objLogin.SaveUserLog(Err_No, Err_Desc, "D", "CALL PLANNING", "EDIT/DELETE ROUTE PLAN", Route_FSR_ID.SelectedItem.Text, "Desc: " & Route_ID.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), Route_FSR_ID.SelectedItem.Text, "0")
        Response.Redirect("ModDelRoutePlan.aspx?Mode=Del", False)
    End Sub

    Protected Sub Route_FSR_ID_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Route_FSR_ID.SelectedIndexChanged
        Try
            System.Threading.Thread.Sleep(1000)
            If (Route_FSR_ID.SelectedItem.Text <> "-- Select a Van --") Then
                objRoute.SalesRepID = Route_FSR_ID.SelectedItem.Value
                objRoute.Site = GetSite(Route_FSR_ID.SelectedItem.Value, Route_FSR_ID.SelectedItem.Text)
                Session("SalesRep_ID") = Route_FSR_ID.SelectedItem.Value
                Session("FSRTable") = objRoute.ShowFSRPlans(Err_No, Err_Desc)


                Dt = CType(Session("FSRTable"), DataTable)
                Route_ID.DataSource = Dt
                Route_ID.DataBind()
                Route_ID.Items.Insert(0, New ListItem("  -- Select an existing plan -- "))
                Dim i As Integer
                For i = 0 To Route_ID.Items.Count - 2
                    Dim Color As String
                    If (Dt.Rows(i).Item(2).ToString = "N") Then
                        Color = "Color:Black"
                    ElseIf (Dt.Rows(i).Item(2).ToString = "Y") Then
                        Color = "Color:Green"
                    ElseIf (Dt.Rows(i).Item(2).ToString = "R") Then
                        Color = "Color:Red"
                    End If
                    Route_ID.Items(i + 1).Attributes.Add("style", Color)
                Next
                If Route_ID.Items.Count > 0 Then
                    BtnPanel.Visible = True
                Else
                    BtnPanel.Visible = False
                End If
            Else
                BtnPanel.Visible = False
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Err_No = "74053"
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=ModDelRoutePlan.aspx&Title=Route+Planner")
        End Try
    End Sub
    Private Function GetSite(ByVal ID As Integer, ByVal Name As String) As String
        Dim RetVal As String = ""
        Try
            Dim obj As UserAccess
            obj = CType(Session("User_Access"), UserAccess)
            If obj.AssignedSalesReps.Count = obj.VanName.Count Then
                For i = 0 To obj.AssignedSalesReps.Count - 1
                    If obj.AssignedSalesReps(i) IsNot DBNull.Value And obj.VanName(i) IsNot DBNull.Value Then
                        If obj.AssignedSalesReps(i) = ID Then
                            RetVal = obj.Site(i)
                            Exit For
                        End If
                    End If
                Next
            End If

            Return RetVal
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        End Try
    End Function
End Class