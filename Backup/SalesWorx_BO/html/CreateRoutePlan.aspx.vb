Imports SalesWorx.BO.Common
Imports log4net

Partial Public Class CreateRoutePlan
    Inherits System.Web.UI.Page
    Dim objRoute As RoutePlan
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub CreateRoutePlan_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Route Planner"
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        objRoute = New RoutePlan()
        CreateBtn.Attributes.Add("OnClick", "return CheckDropDownRoute();")
        Try
            If (Not IsPostBack) Then

                If CType(Session("USER_ACCESS"), UserAccess).IsSS <> "N" Then
                    VanPanel.Visible = True
                    ''   Dim UD_SUB_QRY As String = Utils.GetUDSubQuery(Session("USER_ACCESS").Designation, Session("USER_ACCESS").Site, Session("USER_ACCESS").OrgId, Session("USER_ACCESS").SalesRepId)
                    ' Dim UD_SUB_QRY As String = "SELECT AB.SalesRep_ID FROM TBL_Org_CTL_DTL As AB WHERE AB.Org_Id=102 "
                    '   Route_FSR_ID.DataSource = objRoute.ShowSalesRepsByUD(Err_No, Err_Desc, UD_SUB_QRY)
                    '   Route_FSR_ID.DataBind()

                    Dim UD_SUB_QRY = "select SalesRep_ID from dbo.app_GetControlInfo(" & CType(Session("User_Access"), UserAccess).UserID & ")"
                    Route_FSR_ID.DataSource = objRoute.ShowSalesRepsByUD(Err_No, Err_Desc, UD_SUB_QRY)
                    Route_FSR_ID.DataBind()


                    'Route_FSR_ID.DataSource = BindFSR()
                    'Route_FSR_ID.DataBind()

                Else
                    VanPanel.Visible = False
                    BtnPanel.Visible = True

                    objRoute.SalesRepID = CType(Session("USER_ACCESS"), UserAccess).SalesRepID
                    Session("SalesRep_ID") = CType(Session("USER_ACCESS"), UserAccess).SalesRepID
                    Session("SalesRep_Name") = CType(Session("USER_ACCESS"), UserAccess).SalesRep_Name
                    objRoute.Site = CType(Session("USER_ACCESS"), UserAccess).Site(0)
                    Default_Plan_DD.DataSource = objRoute.ShowDefPlans(Err_No, Err_Desc)
                    Default_Plan_DD.DataBind()
                    If Default_Plan_DD.Items.Count > 0 Then
                        BtnPanel.Visible = True
                    Else
                        BtnPanel.Visible = False
                    End If

                End If

                If Err_Desc IsNot Nothing Then
                    log.Error(Err_Desc)
                    Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Route+Planner", False)
                End If
            End If
        Catch ex As Exception
            Err_No = "74015"
            If Err_Desc Is Nothing Then
                Err_Desc = ex.Message
            End If
            log.Error(Err_Desc)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Route+Planner", False)
        End Try
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
    Protected Sub CreateBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CreateBtn.Click
        Session("Default_Plan_ID") = Default_Plan_DD.SelectedItem.Value
        Response.Redirect("AdminRoutePlan.aspx?Mode=ADD")
    End Sub

    Protected Sub Route_FSR_ID_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Route_FSR_ID.SelectedIndexChanged
        System.Threading.Thread.Sleep(1000)
        Try
            ''     If (CType(Session("USER_ACCESS"), UserAccess).SalesRepID <> "0" And Route_FSR_ID.SelectedItem.Text <> "-- Select a Van --") Then
            If (Route_FSR_ID.SelectedItem.Text <> "-- Select a Van --") Then
                objRoute.SalesRepID = Route_FSR_ID.SelectedItem.Value
                Session("SalesRep_ID") = Route_FSR_ID.SelectedItem.Value
                Session("SalesRep_Name") = Route_FSR_ID.SelectedItem.Text
                objRoute.Site = GetSite(Route_FSR_ID.SelectedItem.Value, Route_FSR_ID.SelectedItem.Text)
                Default_Plan_DD.DataSource = objRoute.ShowDefPlans(Err_No, Err_Desc)
                Default_Plan_DD.DataBind()
                If Default_Plan_DD.Items.Count > 0 Then
                    BtnPanel.Visible = True
                Else
                    BtnPanel.Visible = False
                End If
            Else
                BtnPanel.Visible = False
            End If
        Catch ex As Exception
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
            BtnPanel.Visible = False
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=CreateRoutePlan.aspx&Title=Route+Planner", False)
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