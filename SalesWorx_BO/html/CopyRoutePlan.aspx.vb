Imports SalesWorx.BO.Common
Imports System.Data
Imports System.Resources
Imports System.Web.UI.WebControls
Imports log4net
Imports Telerik.Web.UI

Partial Public Class CopyRoutePlan
    Inherits System.Web.UI.Page
    Dim objRoute As New RoutePlan
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim Dt As New DataTable
    Dim ErrorResource As ResourceManager
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objLogin As New SalesWorx.BO.Common.Login

    Private Sub CopyRoutePlan_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Copy Route Plan"
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If IsNothing(Session("USER_ACCESS")) Then
        '    Response.Redirect("Login.aspx")
        'End If
        objRoute = New RoutePlan
        If (Not IsPostBack) Then
            Try

                If CType(Session("User_Access"), UserAccess).IsSS <> "N" Then
                    SRepPanel.Visible = True
                    Dim UD_SUB_QRY = "select SalesRep_ID from dbo.app_GetControlInfo(" & CType(Session("User_Access"), UserAccess).UserID & ")"
                    Route_FSR_ID.DataSource = objRoute.ShowSalesRepsByUD(Err_No, Err_Desc, UD_SUB_QRY)
                    Route_FSR_ID.DataBind()
                Else
                    SRepPanel.Visible = False
                    BtnPanel.Visible = True
                    BindPlans(CType(Session("User_Access"), UserAccess).SalesRepID, CType(Session("User_Access"), UserAccess).Site(0))
                End If

                If Not Err_Desc Is Nothing Then
                    log.Error(Err_Desc)
                    Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=Welcome.aspx&Title=Copy+Route+Plan", False)
                End If
            Catch ex As Exception
                If Err_Desc IsNot Nothing Then
                    log.Error(Err_Desc)
                Else
                    log.Error(GetExceptionInfo(ex))
                End If
                Response.Redirect("Information.aspx?mode=1&errno=" & "74067" & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=Welcome.aspx&Title=Copy+Route+Plan", False)
            Finally
                objRoute = Nothing
                ErrorResource = Nothing
            End Try

        End If
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        str = str.Replace("'", "&#39;")
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub CopyBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CopyBtn.Click
        If Route_ID.SelectedItem.Value = "" Or RP_ID.SelectedItem.Value = "" Then
            MessageBoxValidation("Please select a route and default plan", "Validation")
            Exit Sub
        End If

        objRoute = New RoutePlan
        Try
            Dim FSRDt As New DataTable

            If (Session("FSRTable") Is Nothing) Then
                objRoute.SalesRepID = Route_FSR_ID.SelectedItem.Value
                objRoute.Site = GetSite(Route_FSR_ID.SelectedItem.Value, Route_FSR_ID.SelectedItem.Text)
                Session("FSRTable") = objRoute.ShowCopyFromPlans(Err_No, Err_Desc)
            End If

            FSRDt = CType(Session("FSRTable"), DataTable)
            Dim results() As DataRow = CType(Session("FSRTable"), DataTable).Select("FSR_Plan_ID=" & Route_ID.SelectedItem.Value & "")
            objRoute.DefPlanId = results(0).Item(3)
            objRoute.FSRPlanId = Route_ID.SelectedItem.Value
            If CType(Session("User_Access"), UserAccess).IsSS <> "N" Then
                objRoute.SalesRepID = Route_FSR_ID.SelectedItem.Value
                Session("SalesRep_ID") = Route_FSR_ID.SelectedItem.Value
            Else
                objRoute.SalesRepID = CType(Session("User_Access"), UserAccess).SalesRepID
                Session("SalesRep_ID") = CType(Session("User_Access"), UserAccess).SalesRepID
            End If
            'Dim s1 As String() = Route_FSR_ID.SelectedItem.Text.Split("-")
            Dim VID As String = Route_FSR_ID.SelectedValue
            ' If s1.Length > 1 Then
            'VID = s1(1)
            '  End If
            If objRoute.CopyFSRPlan(Err_No, Err_Desc, RP_ID.SelectedItem.Value) Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "CALL PLANNING", "COPY ROUTE PLAN", VID.Trim(), "Copy From: " & Route_ID.SelectedItem.Text & "/ Copy To :  " & RP_ID.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), VID.Trim(), "0")
                Response.Redirect("Information.aspx?mode=0&msg=FSR+Route+plan+copied+successfully&next=CopyRoutePlan.aspx&Title=Copy+Route+Plan", False)
                Session.Remove("FSRTable")
            Else
                log.Error(Err_Desc)
                Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=CopyRoutePlan.aspx&Title=Copy+Route+Plan", False)
            End If
        Catch ex As Exception
            Err_No = "74072"
            If Err_Desc IsNot Nothing Then
                log.Error(Err_Desc)
            Else
                log.Error(GetExceptionInfo(ex))
            End If
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=CopyRoutePlan.aspx&Title=Copy+Route+Plan", False)
        Finally
            objRoute = Nothing
            ErrorResource = Nothing
        End Try
    End Sub
    Private Sub BindPlans(ByVal Id As Integer, ByVal Site As String)
        Try
            objRoute.SalesRepID = Id
            objRoute.Site = Site

            Session.Remove("FSRTable")
            If (Session("FSRTable") Is Nothing) Then
                Session("FSRTable") = objRoute.ShowCopyFromPlans(Err_No, Err_Desc)
            End If

            Dt = CType(Session("FSRTable"), DataTable)
            Route_ID.DataSource = Dt
            Route_ID.DataBind()
            Route_ID.Items.Insert(0, New RadComboBoxItem("  -- Select a route plan -- "))
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
            Dt = CType(objRoute.ShowCopyToPlans(Err_No, Err_Desc), DataTable)
            RP_ID.DataSource = Dt
            RP_ID.DataBind()
            RP_ID.Items.Insert(0, New RadComboBoxItem("  -- Select a default plan -- "))
            If Route_ID.Items.Count > 0 Then
                BtnPanel.Visible = True
            Else
                BtnPanel.Visible = False
            End If

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        End Try
    End Sub
    Protected Sub Route_FSR_ID_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Route_FSR_ID.SelectedIndexChanged
        System.Threading.Thread.Sleep(1000)
        Dim SalesRep_ID As Long = CType(Session("USER_ACCESS"), UserAccess).SalesRepID
        Try
            If (Route_FSR_ID.SelectedItem.Text <> "-- Select a Van --") Then

                BindPlans(Route_FSR_ID.SelectedItem.Value, GetSite(Route_FSR_ID.SelectedItem.Value, Route_FSR_ID.SelectedItem.Text))
            Else
                BtnPanel.Visible = False
            End If
        Catch ex As Exception
            BtnPanel.Visible = False
            If Err_Desc IsNot Nothing Then
                log.Error(Err_Desc)
            Else
                log.Error(GetExceptionInfo(ex))
            End If
            Response.Redirect("Information.aspx?mode=1&errno=" & "74070" & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=CopyRoutePlan.aspx&Title=Copy+Route+Plan", False)
        End Try

    End Sub
    Private Function GetSite(ByVal ID As Integer, ByVal Name As String) As String
        Dim RetVal As String = ""
        Try
            Dim obj As UserAccess
            '  Dim Vname() As String = Nothing
            Dim Van As String = Name
            'Vname = Name.Split("-")
            ' If Vname.Length > 0 Then
            'Van = Trim(Vname(1))
            ' Else
            ' Van = Trim(Vname(0))
            ' End If
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