Imports SalesWorx.BO.Common
Imports System.Resources
Imports log4net

Partial Public Class DefaultPlan
    Inherits System.Web.UI.Page
    Dim RPIDs As ArrayList
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Private Const PageID As String = "P65"
    Dim objLogin As New SalesWorx.BO.Common.Login

    Private Sub DefaultPlan_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Route Planner"
    End Sub
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        ModBtn.Attributes.Add("OnClick", "return CheckDropDownDefault();")
        DelBtn.Attributes.Add("OnClick", "return CheckDelItemDefault();")
        Dim objRoute As New RoutePlan
        Dim objCommon As New Common

        objRoute.DefPlanId = 0
        If (Not IsPostBack) Then
            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            If Not HasPermission Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If
            Try
                'If (Session("USER_ACCESS").SalesRepID <> "0") Then
                '    AddBtn.Visible = True
                'End If

                If (Request.QueryString("Mode") = "Del") Then
                    If Not (Session("RP_ID") = Nothing) Then
                        objRoute.DefPlanId = Session("RP_ID")
                        If (objRoute.DeleteDefaultPlanID(Err_No, Err_Desc)) Then
                            Response.Redirect("information.aspx?mode=0&msg=Selected+plan+is+deleted+successfully&next=DefaultPlan.aspx&Title=Route Planner", False)
                        Else
                            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_002") & "&next=DefaultPlan.aspx&Title=Route Planner", False)
                        End If
                        HttpContext.Current.ApplicationInstance.CompleteRequest()
                    End If
                End If

                ''Check if User has Multiple Sites
                Session("HasMultiSite") = "NO"
                If CType(Session("User_Access"), UserAccess).IsSS <> "N" Then
                    Dim Tbl As New DataTable
                    Err_No = Nothing
                    Err_Desc = Nothing
                    Tbl = objCommon.GetDistinct(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)
                    If Tbl IsNot Nothing Then
                        If Tbl.Rows.Count = 1 Then
                            Session("HasMultiSite") = "NO"
                            Session("Site") = Tbl.Rows(0).Item(0)
                            defplan.Visible = True
                            sitePanel.Visible = False
                            BindDefaultPlan(Tbl.Rows(0).Item(0))
                        ElseIf Tbl.Rows.Count > 1 Then
                            Session("HasMultiSite") = "YES"
                            defplan.Visible = False
                            sitePanel.Visible = True
                            ddSite.Items.Clear()
                            'ddSite.Items.Add("-- Select a Site --")
                            ddSite.Items.Add("-- Select a Organization --")
                            ddSite.AppendDataBoundItems = True
                            ddSite.DataValueField = "Site"
                            ddSite.DataTextField = "Description"
                            ddSite.DataSource = Tbl
                            ddSite.DataBind()
                        End If
                    End If
                End If

            Catch ex As Exception
                log.Error(ex.Message)
                Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=DefaultPlan.aspx&Title=Route Planner", False)
            Finally
                objRoute = Nothing
                objCommon = Nothing
            End Try
        End If
    End Sub
    Private Sub BindDefaultPlan(ByVal Site As String)
        Dim objRoute As New RoutePlan
        Try
            RPIDs = objRoute.GetRoutePlanID(Err_No, Err_Desc)
            RP_ID.DataSource = objRoute.ShowExistingPlan(Site, Err_No, Err_Desc)
            RP_ID.DataBind()

            Dim i As Integer
            i = 0
            For i = 0 To RP_ID.Items.Count - 1
                If (RPIDs.Contains(RP_ID.Items(i).Value)) Then
                    RP_ID.Items(i).Attributes.Add("style", "color:Blue")
                Else
                    RP_ID.Items(i).Attributes.Add("style", "color:Black")
                End If

            Next
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        Finally
            objRoute = Nothing
        End Try
    End Sub
    Private Sub AddBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddBtn.Click
        Response.Redirect("MngDefaultPlan.aspx?Mode=ADD&RP_ID=" & RP_ID.SelectedItem.Value & "")
    End Sub

    Private Sub DelBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DelBtn.Click
        Session("RP_ID") = RP_ID.SelectedItem.Value
        'Dim s As String() = ddSite.SelectedValue.ToString().Split("-")
        Dim OID As String = ddSite.SelectedValue
        '   If s.Length > 1 Then
        'OID = s(1)
        '  End If
        objLogin.SaveUserLog(Err_No, Err_Desc, "D", "CALL PLANNING", "DEFAULT PLAN", OID.Trim(), "Desc: " & RP_ID.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", OID.Trim())
        Response.Redirect("DefaultPlan.aspx?Mode=Del")
    End Sub
    Private Sub ModBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ModBtn.Click
        Response.Redirect("MngDefaultPlan.aspx?Mode=MODIFY&RP_ID=" & RP_ID.SelectedItem.Value & "")
    End Sub

    Private Sub ddSite_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddSite.SelectedIndexChanged
        If Not ddSite.SelectedItem.Text = "-- Select a Organization --" Then
            defplan.Visible = True
            '            BindDefaultPlan(ddSite.SelectedItem.Text)
            BindDefaultPlan(ddSite.SelectedValue.ToString())
            Session("Site") = ddSite.SelectedValue.ToString()
        Else
            defplan.Visible = False
        End If
    End Sub
End Class