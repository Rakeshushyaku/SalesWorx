Imports SalesWorx.BO.Common
Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports Telerik.Web.UI
Public Class Van_Audit_Plan
    Inherits System.Web.UI.Page
    Dim objProduct As New Product
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const PageID As String = "P326"
    Dim objLogin As New SalesWorx.BO.Common.Login
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        Try
            If Not IsPostBack Then
                Dim HasPermission As Boolean = False
                ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
                If Not HasPermission Then
                    Err_No = 500
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
                End If
                Dim objCommon As New Common
                Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddOraganisation.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddOraganisation.Items.Clear()
                ddOraganisation.Items.Add(New RadComboBoxItem("-- Select a Organization --"))
                ddOraganisation.AppendDataBoundItems = True
                ddOraganisation.DataValueField = "MAS_Org_ID"
                ddOraganisation.DataTextField = "Description"
                ddOraganisation.DataBind()

                Me.ddlUser.Items.Clear()
                ddlUser.AppendDataBoundItems = True
                ddlUser.Items.Insert(0, New RadComboBoxItem("--Select a Supervisor--"))
                ddlUser.Items(0).Value = ""

                lstDefault.Items.Clear()
                lstSelected.Items.Clear()
                LoadYear()
                ddlMonth.Items.Insert(0, New RadComboBoxItem("-- Select --", 0))
                ''Bind Default List

            End If
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_MSL_005") & "&next=Welcome.aspx&Title=Product List Setup", False)
        End Try

    End Sub

    Private Sub ddOraganisation_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddOraganisation.SelectedIndexChanged
        If ddOraganisation.SelectedItem.Text <> "-- Select a Organization --" Then
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim objCommon As New Common
            ddlUser.DataSource = objCommon.GetVanAuditors(Err_No, Err_Desc, ddOraganisation.SelectedValue)
            ddlUser.Items.Clear()
            ddlUser.Items.Add(New RadComboBoxItem("-- Select a van/FSR --"))
            ddlUser.AppendDataBoundItems = True
            ddlUser.DataValueField = "USER_ID"
            ddlUser.DataTextField = "username"
            ddlUser.DataBind()
            ddlMonth.Items.Insert(0, New RadComboBoxItem("-- Select --", 0))
            ddlYear.ClearSelection()
            ddlMonth.ClearSelection()
            BindDefault()
            BindSelected()

        Else
            ddlUser.Items.Clear()
            ddlUser.Items.Add(New RadComboBoxItem("-- Select a van/FSR--"))
            ddlUser.AppendDataBoundItems = True
            ddlUser.DataValueField = "USER_ID"
            ddlUser.DataTextField = "username"
            ddlUser.DataBind()
            lstDefault.Items.Clear()
            lstSelected.Items.Clear()
            lblFsrAssign.Text = ""
            lblFsrAssign.Text = ""
            ddlMonth.Items.Insert(0, New RadComboBoxItem("-- Select --", 0))
            ddlYear.ClearSelection()
            ddlMonth.ClearSelection()
        End If
    End Sub
    Sub LoadYear()
        ddlYear.Items.Clear()
        If Now.Month <> 12 Then
            For i As Integer = Now.Year To Now.Year + 1
                ddlYear.Items.Add(New RadComboBoxItem(i.ToString, i.ToString))
            Next
        Else
            For i As Integer = Now.Year + 1 To Now.Year + 1
                ddlYear.Items.Add(New RadComboBoxItem(i.ToString, i.ToString))
            Next
        End If
        ddlYear.Items.Insert(0, New RadComboBoxItem("-- Select --", 0))
         
    End Sub
    Sub LoadMonth()
        ddlMonth.Items.Clear()
        If ddlYear.SelectedItem.Value = Now.Year Then
            For i As Integer = Now.Month To 12
                ddlMonth.Items.Add(New RadComboBoxItem(CDate(i & "/01/1900").ToString("MMM"), i.ToString))
            Next
        Else
            For i As Integer = 1 To 12
                ddlMonth.Items.Add(New RadComboBoxItem(CDate(i & "/01/1900").ToString("MMM"), i.ToString))
            Next
        End If
        ddlMonth.Items.Insert(0, New RadComboBoxItem("-- Select --", 0))

    End Sub

    Private Sub ddlYear_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlYear.SelectedIndexChanged
        LoadMonth()
        BindDefault()
        BindSelected()
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        str = str.Replace("'", "&#39;")
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub

   
    Private Sub BindDefault()
        If Me.ddOraganisation.SelectedIndex > 0 And Me.ddlUser.SelectedIndex > 0 And Me.ddlYear.SelectedIndex > 0 And ddlMonth.SelectedIndex > 0 Then
            Dim objSurvey As New Survey
            Dim TempTbl As New DataTable
            Try
                Err_Desc = Nothing
                Err_No = Nothing

                TempTbl = objSurvey.GetFsrForVanAudit(ddOraganisation.SelectedItem.Value, ddlYear.SelectedItem.Value, ddlMonth.SelectedItem.Value, Err_No, Err_Desc)
                If TempTbl IsNot Nothing Then

                    lstDefault.DataSource = TempTbl
                    lstDefault.DataTextField = "Name"
                    lstDefault.DataValueField = "ID"
                    lstDefault.DataBind()
                End If
            
            Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        Finally
            objSurvey = Nothing
            End Try
        Else
            lstDefault.Items.Clear()
        End If
    End Sub

    Private Sub BindSelected()
        If Me.ddOraganisation.SelectedIndex > 0 And Me.ddlUser.SelectedIndex > 0 And Me.ddlYear.SelectedIndex > 0 And ddlMonth.SelectedIndex > 0 Then
            Dim objSurvey As New Survey
            Dim TempTbl As New DataTable
            Try
                Err_Desc = Nothing
                Err_No = Nothing
                TempTbl = objSurvey.GetAssignedFsrForVanAudit(ddOraganisation.SelectedItem.Value, ddlUser.SelectedItem.Value, ddlYear.SelectedItem.Value, ddlMonth.SelectedItem.Value, Err_No, Err_Desc)
                If TempTbl IsNot Nothing Then

                    lstSelected.DataSource = TempTbl
                    lstSelected.DataTextField = "Name"
                    lstSelected.DataValueField = "ID"
                    lstSelected.DataBind()
                End If

                If lstSelected.Items.Count > 0 Then
                    Me.btnRemoveAll.Enabled = True
                Else
                    Me.btnRemoveAll.Enabled = False
                End If
                lblFsrAssign.Text = "Vans Assigned: [" & lstSelected.Items.Count & "]"
                lblFsrAvailed.Text = "Vans Available: [" & lstDefault.Items.Count & "]"
            Catch ex As Exception
                log.Error(GetExceptionInfo(ex))
                Throw ex
            Finally
                objSurvey = Nothing
            End Try
        Else
            lstSelected.Items.Clear()
        End If
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If Me.ddOraganisation.SelectedIndex > 0 And Me.ddlUser.SelectedIndex > 0 And Me.ddlYear.SelectedIndex > 0 And ddlMonth.SelectedIndex > 0 Then
            If CDate(ddlMonth.SelectedItem.Value & "/" & Now.Day & "/" & ddlYear.SelectedItem.Value) >= Today Then


                Try
                    Err_No = Nothing
                    Err_Desc = Nothing

                    Dim objSurvey As New Survey
                    For Each Item As RadListBoxItem In lstDefault.Items
                        If Item.Selected Then
                            objSurvey.InsertVanAuditPlan(ddlUser.SelectedItem.Value, ddlYear.SelectedItem.Value, ddlMonth.SelectedItem.Value, Item.Value, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, Err_No, Err_Desc)
                            objLogin.SaveUserLog(Err_No, Err_Desc, "I", "SURVEY MANAGEMENT", "VAN AUDIT PLAN", ddlUser.SelectedItem.Text.Trim(), "VAN : " & Item.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), ddlUser.SelectedItem.Text.Trim(), Me.ddOraganisation.SelectedValue.ToString())
                        End If
                    Next
                    BindDefault()
                    BindSelected()
                Catch ex As Exception
                    log.Error(ex.Message)
                    MessageBoxValidation("Could not save in database", "Information")
                End Try
            Else
                MessageBoxValidation("You can plan the audit survey only for the future months.", "Validation")
                Exit Sub
            End If
        Else
            MessageBoxValidation("Please select Organization, Auditor , Audit Month and Audit Year.", "Validation")
            Exit Sub
        End If
    End Sub

    Protected Sub btnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        If Me.ddOraganisation.SelectedIndex > 0 And Me.ddlUser.SelectedIndex > 0 And Me.ddlYear.SelectedIndex > 0 And ddlMonth.SelectedIndex > 0 Then
            If CDate(ddlMonth.SelectedItem.Value & "/" & Now.Day & "/" & ddlYear.SelectedItem.Value) >= Today Then
                Try
                    Err_No = Nothing
                    Err_Desc = Nothing

                    Dim objSurvey As New Survey
                    For Each Item As RadListBoxItem In lstSelected.Items
                        If Item.Selected Then
                            objSurvey.DeleteVanAuditPlan(ddlUser.SelectedItem.Value, ddlYear.SelectedItem.Value, ddlMonth.SelectedItem.Value, Item.Value, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, Err_No, Err_Desc)
                            objLogin.SaveUserLog(Err_No, Err_Desc, "D", "SURVEY MANAGEMENT", "VAN AUDIT PLAN", ddlUser.SelectedItem.Text.Trim(), "VAN : " & Item.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), ddlUser.SelectedItem.Text.Trim(), Me.ddOraganisation.SelectedValue.ToString())
                        End If
                    Next
                    BindDefault()
                    BindSelected()
                Catch ex As Exception
                    log.Error(ex.Message)
                    MessageBoxValidation("Could not save in database", "Information")

                End Try
            Else
                MessageBoxValidation("You can plan the audit survey only for the future months.", "Validation")
                Exit Sub
            End If
        Else
            MessageBoxValidation("Please select Organization, Auditor , Audit Month and Audit Year.", "Validation")
            Exit Sub
        End If
    End Sub

    Private Sub btnAddAll_Click(sender As Object, e As ImageClickEventArgs) Handles btnAddAll.Click
        If Me.ddOraganisation.SelectedIndex > 0 And Me.ddlUser.SelectedIndex > 0 And Me.ddlYear.SelectedIndex > 0 And ddlMonth.SelectedIndex > 0 Then
            If CDate(ddlMonth.SelectedItem.Value & "/" & Now.Day & "/" & ddlYear.SelectedItem.Value) >= Today Then
                Try
                    Err_No = Nothing
                    Err_Desc = Nothing

                    Dim objSurvey As New Survey
                    For Each Item As RadListBoxItem In lstDefault.Items
                        objSurvey.InsertVanAuditPlan(ddlUser.SelectedItem.Value, ddlYear.SelectedItem.Value, ddlMonth.SelectedItem.Value, Item.Value, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, Err_No, Err_Desc)
                        objLogin.SaveUserLog(Err_No, Err_Desc, "I", "SURVEY MANAGEMENT", "VAN AUDIT PLAN", ddlUser.SelectedItem.Text.Trim(), "VAN : " & Item.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), ddlUser.SelectedItem.Text.Trim(), Me.ddOraganisation.SelectedValue.ToString())
                    Next
                    BindDefault()
                    BindSelected()
                Catch ex As Exception
                    log.Error(ex.Message)
                    Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_AdminMSL_005") & "&next=AdminMSL.aspx&Title=Must Stock List", False)
                End Try
            Else
                MessageBoxValidation("You can plan the audit survey only for the future months.", "Validation")
                Exit Sub
            End If
        Else
            MessageBoxValidation("Please select Organization, Auditor , Audit Month and Audit Year.", "Validation")
            Exit Sub
        End If
    End Sub

    Private Sub btnRemoveAll_Click(sender As Object, e As ImageClickEventArgs) Handles btnRemoveAll.Click
        If Me.ddOraganisation.SelectedIndex > 0 And Me.ddlUser.SelectedIndex > 0 And Me.ddlYear.SelectedIndex > 0 And ddlMonth.SelectedIndex > 0 Then
            If CDate(ddlMonth.SelectedItem.Value & "/" & Now.Day & "/" & ddlYear.SelectedItem.Value) >= Today Then
                Try
                    Err_No = Nothing
                    Err_Desc = Nothing

                    Dim objSurvey As New Survey
                    For Each Item As RadListBoxItem In lstSelected.Items

                        objSurvey.DeleteVanAuditPlan(ddlUser.SelectedItem.Value, ddlYear.SelectedItem.Value, ddlMonth.SelectedItem.Value, Item.Value, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, Err_No, Err_Desc)
                        objLogin.SaveUserLog(Err_No, Err_Desc, "D", "SURVEY MANAGEMENT", "VAN AUDIT PLAN", ddlUser.SelectedItem.Text.Trim(), "VAN : " & Item.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), ddlUser.SelectedItem.Text.Trim(), Me.ddOraganisation.SelectedValue.ToString())

                    Next
                    BindDefault()
                    BindSelected()
                Catch ex As Exception
                    log.Error(ex.Message)
                    MessageBoxValidation("Could not save in database", "Information")

                End Try
            Else
                MessageBoxValidation("You can plan the audit survey only for the future months.", "Validation")
                Exit Sub
            End If
        Else
            MessageBoxValidation("Please select Organization, Auditor , Audit Month and Audit Year.", "Validation")
            Exit Sub
        End If
    End Sub

    Private Sub ddlUser_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlUser.SelectedIndexChanged
        BindDefault()
        BindSelected()
    End Sub

    Private Sub ddlMonth_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlMonth.SelectedIndexChanged
        BindDefault()
        BindSelected()
    End Sub
End Class