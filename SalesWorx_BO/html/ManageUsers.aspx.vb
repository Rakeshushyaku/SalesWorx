Imports log4net
Imports SalesWorx.BO.Common
Imports System.IO
Imports Telerik.Web.UI
Partial Public Class ManageUsers
    Inherits System.Web.UI.Page

    Private Const ModuleName As String = "ManageUsers.aspx"
    Private Const PageID As String = "P16"
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim objUser As New User
    Dim objCrypt As New Crypto
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objLogin As New SalesWorx.BO.Common.Login

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Not Session.Item("USER_ACCESS") Is Nothing Then

                If Not HasAuthentication() Then
                    Err_No = 500
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
                End If

                FillUsers()
                FillUserTypes()
                FillDesignation()
                FillSalesRep()
                FillOrg()

                DefaultMode()
            Else
                Session.Add("BringmeBackHere", ModuleName)
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
        End If
        lblMsg.Text = ""
    End Sub

    Private Function HasAuthentication() As Boolean
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
    End Function
    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnAdd.Click
        AddMode()
    End Sub

    Sub AddMode()
        txtUser.Visible = True
        txtUser.Text = ""
        drpUser.Visible = False
        pnlModify.Visible = False
        pnlDefault.Visible = False
        pnlAdd.Visible = True
        txtPwd.Enabled = True
        drpDesignation.Enabled = True
        drpUserType.Enabled = True
        chkSalesRep.Enabled = True
        drpSalesRep.Enabled = True
        txtPwd.Text = ""
        drpSalesRep.SelectedValue = "0"
        drpDesignation.SelectedValue = "0"
        drpUserType.SelectedValue = "0"
        chkSalesRep.ClearSelection()
        drpUser.SelectedValue = "0"
        Me.ddl_org.ClearSelection()
        Me.ddl_org.Text = ""
        Me.ddl_org.SelectedIndex = 0
        chkSalesRep.Items.Clear()
        For Each ch As RadListBoxItem In chkSalesRep.Items
            ch.Checked = False
        Next
        ddl_org.Enabled = True
        txtPwd.Attributes.Add("value", "")
        Orgdiv.Visible = False
        SalesRepdiv.Visible = False
    End Sub
    Sub EditMode()
        txtUser.Visible = True
        txtUser.Text = drpUser.SelectedItem.Text
        drpUser.Visible = False
        pnlModify.Visible = True
        pnlAdd.Visible = False
        pnlDefault.Visible = False
        txtPwd.Enabled = True
        drpDesignation.Enabled = True
        drpUserType.Enabled = True
        chkSalesRep.Enabled = True
        drpSalesRep.Enabled = True
        ddl_org.Enabled = True
    End Sub
    Sub DefaultMode()
        txtUser.Visible = False
        txtUser.Text = ""
        drpUser.Visible = True
        pnlAdd.Visible = False
        pnlModify.Visible = False
        pnlDefault.Visible = True
        txtPwd.Enabled = False
        drpDesignation.Enabled = False
        drpUserType.Enabled = False
        chkSalesRep.Enabled = False
        drpSalesRep.Enabled = False
        ddl_org.Enabled = False
        chkSalesRep.Items.Clear()
        For Each ch As RadListBoxItem In chkSalesRep.Items
            ch.Checked = False
        Next
        ddl_org.ClearSelection()
        ddl_org.Text = ""
        ddl_org.SelectedIndex = 0
        txtPwd.Text = ""
        drpSalesRep.SelectedValue = "0"
        drpDesignation.SelectedValue = "0"
        drpUserType.SelectedValue = "0"
        chkSalesRep.ClearSelection()
        drpUser.SelectedValue = "0"
        drpSalesRep.Visible = True
        chkSalesRep.Visible = False
        Panel1.Visible = False
        lbl_van.Visible = False
        lbl_Van1.Visible = True
        txtPwd.Attributes.Add("value", "")
    End Sub
    Sub FillOrg()
        Dim dt As New DataTable
        Dim ObjCommon As New SalesWorx.BO.Common.Common
        Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
        dt = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
        Dim dr As DataRow
        dr = dt.NewRow()
        dr(0) = "0"
        dr(1) = "--Select a value --"
        dt.Rows.InsertAt(dr, 0)
        ddl_org.DataSource = dt
        ddl_org.DataTextField = "Description"
        ddl_org.DataValueField = "MAS_Org_ID"
        ddl_org.DataBind()
        ObjCommon = Nothing
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 350, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Sub FillSalesRep()
        drpSalesRep.DataTextField = "SalesRep_Name"
        drpSalesRep.DataValueField = "SalesRep_ID"
        drpSalesRep.DataSource = objUser.GetSalesReps()
        drpSalesRep.DataBind()
    End Sub
    Sub FillUserTypes()
        drpUserType.DataTextField = "User_Type"
        drpUserType.DataValueField = "User_Type_ID"
        drpUserType.DataSource = objUser.GetUserTypesByDesignation(IIf(Me.drpDesignation.SelectedIndex <= 0, "0", Me.drpDesignation.SelectedValue))
        drpUserType.DataBind()
    End Sub
    Sub FillDesignation()
        drpDesignation.DataTextField = "val"
        drpDesignation.DataValueField = "id"
        drpDesignation.DataSource = objUser.GetIsSS()
        drpDesignation.DataBind()
    End Sub
    Sub FillSalesRepList()
        'chkSalesRep.DataTextField = "SalesRep_Name"
        'chkSalesRep.DataValueField = "SalesRep_ID"
        'chkSalesRep.DataSource = objUser.GetSalesReps_chkbox()
        'chkSalesRep.DataBind()
        chkSalesRep.Items.Clear()
        For Each ch As RadListBoxItem In chkSalesRep.Items
            ch.Checked = False
        Next

        Dim ObjCommon As New SalesWorx.BO.Common.Common
                          Dim objUserAccess As UserAccess
                            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

        chkSalesRep.DataSource = ObjCommon.GetVanByDesignation(Err_No, Err_Desc, ddl_org.SelectedValue, objUserAccess.UserID, drpDesignation.SelectedValue)
                            chkSalesRep.DataValueField = "SalesRep_ID"
                            chkSalesRep.DataTextField = "SalesRep_Name"
                            chkSalesRep.DataBind()

        ObjCommon = Nothing


    End Sub
    Sub FillUsers()
        drpUser.DataTextField = "Username"
        drpUser.DataValueField = "User_ID"
        drpUser.DataSource = objUser.GetUsers()
        drpUser.DataBind()
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        Try
            If txtUser.Text.Trim() = "" Then
                MessageBoxValidation("Please enter user.", "Validation")
                Return
            End If
            If txtPwd.Text.Trim() = "" Then
                MessageBoxValidation("Please enter password.", "Validation")
                Return
            End If
            If drpUserType.SelectedValue = "0" Then
                MessageBoxValidation("Please enter user type.", "Validation")
                txtPwd.Attributes.Add("value", txtPwd.Text)
                Return
            End If
            If drpDesignation.SelectedValue = "0" Then
                MessageBoxValidation("Please enter designation.", "Validation")
                txtPwd.Attributes.Add("value", txtPwd.Text)
                Return
            End If
            If drpDesignation.SelectedValue = "0" Then
                MessageBoxValidation("Please enter designation.", "Validation")
                txtPwd.Attributes.Add("value", txtPwd.Text)
                Return
            End If
            If (drpDesignation.SelectedValue = "Y") Then
             If ddl_org.SelectedItem.Value = "0" Then
                    MessageBoxValidation("Please select organisation.", "Validation")
                Return
             End If
            End If
            objUser.UserName = txtUser.Text.Trim()
            'objUser.Password = objCrypt.encrypt(txtPwd.Text.Trim(), "Unique")
            objUser.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(txtPwd.Text.Trim(), "SHA1")
          

            objUser.IsSS = drpDesignation.SelectedValue
            If objUser.IsSS = "N" Then
                objUser.SalesRepID = drpSalesRep.SelectedValue
            Else
                objUser.SalesRepID = 0
            End If
            objUser.UserTypeID = drpUserType.SelectedValue
            objUser.OrgID = ddl_org.SelectedItem.Value
            objUser.AssignedSalesRep = New ArrayList
            Dim Icount As Integer = 0
            For Each li As RadListBoxItem In chkSalesRep.Items
                If li.Checked = True Then
                    objUser.AssignedSalesRep.Add(li.Value)
                    Icount += 1
                End If
            Next


            If objUser.IsSS = "N" And objUser.IsSalesRepAssigned() Then
                lblMsg.Text = drpSalesRep.SelectedItem.Text + " already assigned to another user."
                Return
            End If
            If Not objUser.UserExists() Then
                objUser.SaveUser(Err_No, Err_Desc)
                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "USER MANAGEMENT", "MANAGE USER", Me.txtUser.Text, "User Name: " & Me.txtUser.Text & "/ User Type: " & Me.drpUserType.SelectedItem.Text & "/ Designation : " & Me.drpDesignation.SelectedItem.Text & "/ Assigned SalesRep : " & Icount, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
            Else
                MessageBoxValidation("User name already exists.", "Validation")
                Return
            End If

            MessageBoxValidation("User added successfully.", "Information")
            FillUsers()
            DefaultMode()

        Catch ex As Exception
            MessageBoxValidation("Error occured while adding user.", "Validation")
            log.Error(ex.Message)
        End Try
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click
        Try
            If drpUser.SelectedValue = "0" Then
                MessageBoxValidation("Please select user.", "Validation")
                Return
            End If
            'If txtPwd.Text.Trim() = "" Then
            '    MessageBoxValidation("Please enter password.")
            '    Return
            'End If
            If drpUserType.SelectedValue = "0" Then
                MessageBoxValidation("Please enter user type.", "Validation")
                txtPwd.Attributes.Add("value", txtPwd.Text)
                Return
            End If
            If (drpDesignation.SelectedValue = "Y") Then
             If ddl_org.SelectedItem.Value = "0" Then
                    MessageBoxValidation("Please select organisation.", "Validation")
                Return
             End If
            End If
            objUser.UserID = drpUser.SelectedItem.Value
            objUser.UserName = txtUser.Text.Trim()
            ' objUser.Password = objCrypt.encrypt(txtPwd.Text.Trim(), "Unique")
            '  objUser.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(txtPwd.Text.Trim(), "SHA1")
            If txtPwd.Text.Trim() <> "" Then
                objUser.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(txtPwd.Text.Trim(), "SHA1")
            Else
                objUser.Password = ViewState("Pwd")
            End If
            '  objUser.SalesRepID = IIf(objUser.IsSS = "N", drpSalesRep.SelectedValue, 0)
        

            objUser.IsSS = drpDesignation.SelectedValue

            If objUser.IsSS = "N" Then
                objUser.SalesRepID = drpSalesRep.SelectedValue
            Else
                objUser.SalesRepID = 0
            End If
            objUser.UserTypeID = drpUserType.SelectedValue
            objUser.OrgID = ddl_org.SelectedItem.Value
            objUser.AssignedSalesRep = New ArrayList
            Dim Icount As Integer = 0
            For Each li As RadListBoxItem In chkSalesRep.Items
                If li.Checked = True Then
                    objUser.AssignedSalesRep.Add(li.Value)
                    Icount += 1
                End If
            Next
            If objUser.IsSS = "N" And objUser.IsSalesRepAssigned_ForUpdate() Then
                lblMsg.Text = drpSalesRep.SelectedItem.Text + " already assigned to another user."
                Return
            End If
            objUser.UpdateUser(Err_No, Err_Desc)
            objLogin.SaveUserLog(Err_No, Err_Desc, "U", "USER MANAGEMENT", "MANAGE USER", Me.txtUser.Text, "User Name: " & Me.txtUser.Text & "/ User Type: " & Me.drpUserType.SelectedItem.Text & "/ Designation : " & Me.drpDesignation.SelectedItem.Text & "/ Assigned SalesRep : " & Icount, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
            MessageBoxValidation("User updated successfully.", "Information")
            FillUsers()
            LoadUser()
            DefaultMode()
        Catch ex1 As SqlClient.SqlException
            If ex1.ErrorCode = "-2146232060" Then
                MessageBoxValidation("Username already exists.", "Validation")
            End If
        Catch ex As Exception
            MessageBoxValidation("Error occured while updating user.", "Validation")
            log.Error(ex.Message)
        End Try
    End Sub

    Protected Sub drpDesignation_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles drpDesignation.SelectedIndexChanged
        If drpDesignation.SelectedValue = "N" Then
            lbl_Van1.Visible = True
            drpSalesRep.Visible = True
            chkSalesRep.Visible = False
            lbl_Org.Visible = False
            lbl_van.Visible = False
            Me.ddl_org.ClearSelection()
            Me.ddl_org.Text = ""
            Me.ddl_org.SelectedIndex = 0
            ddl_org.Visible = False
            Panel1.Visible = False
            SalesRepdiv.Visible = False
            Orgdiv.Visible = False
        ElseIf drpDesignation.SelectedValue = "A" Then
            drpSalesRep.SelectedIndex = 0
            drpSalesRep.Visible = False
            chkSalesRep.Visible = False
            lbl_Org.Visible = False
            lbl_van.Visible = False
            Me.ddl_org.ClearSelection()
            Me.ddl_org.Text = ""
            Me.ddl_org.SelectedIndex = 0
            ddl_org.Visible = False
            Panel1.Visible = False
            lbl_Van1.Visible = False
            SalesRepdiv.Visible = False
            Orgdiv.Visible = False
        ElseIf drpDesignation.SelectedValue = "Y" Or drpDesignation.SelectedValue = "M" Then
            drpSalesRep.SelectedIndex = 0
            lbl_Van1.Visible = False
            drpSalesRep.Visible = False
            chkSalesRep.Visible = True
            lbl_Org.Visible = True
            lbl_van.Visible = True
            Me.ddl_org.ClearSelection()
            Me.ddl_org.Text = ""
            Me.ddl_org.SelectedIndex = 0
            ddl_org.Visible = True
            FillSalesRepList()
            Panel1.Visible = True
            SalesRepdiv.Visible = True
            Orgdiv.Visible = True
        Else
            drpSalesRep.SelectedIndex = 0
            drpSalesRep.Visible = False
            chkSalesRep.Visible = False
            lbl_Org.Visible = False
            lbl_van.Visible = False
            Me.ddl_org.ClearSelection()
            Me.ddl_org.Text = ""
            Me.ddl_org.SelectedIndex = 0
            ddl_org.Visible = False
            Panel1.Visible = False
            lbl_Van1.Visible = False
            SalesRepdiv.Visible = False
            Orgdiv.Visible = False
        End If
        FillUserTypes()
        txtPwd.Attributes.Add("value", txtPwd.Text)
    End Sub

    Protected Sub drpUser_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles drpUser.SelectedIndexChanged
        LoadUser()
    End Sub
   Sub LoadUser()
       objUser.UserID = drpUser.SelectedValue
        If objUser.UserID <> "0" Then
            If objUser.GetUser(Err_No, Err_Desc) Then
                ViewState("Pwd") = objUser.Password
                txtPwd.Text = ""

                drpDesignation.SelectedValue = objUser.IsSS

                FillUserTypes()
                If drpUserType.Items.Count > 1 Then
                    drpUserType.SelectedValue = objUser.UserTypeID
                Else
                    drpUserType.SelectedValue = "0"
                End If

                If objUser.IsSS = "N" Then
                    drpSalesRep.SelectedValue = objUser.SalesRepID
                Else
                    drpSalesRep.SelectedValue = "0"
                End If
                If Not ddl_org.Items.FindItemByValue(objUser.OrgID) Is Nothing Then
                    ddl_org.ClearSelection()
                    ddl_org.Items.FindItemByValue(objUser.OrgID).Selected = True
                    FillSalesRepList()
                End If
                If objUser.IsSS = "N" Then
                    lbl_Van1.Visible = True
                    drpSalesRep.Visible = True
                    chkSalesRep.Visible = False
                    lbl_Org.Visible = False
                    lbl_van.Visible = False
                    ddl_org.Visible = False
                    Panel1.Visible = False
                    SalesRepdiv.Visible = False
                    Orgdiv.Visible = False
                ElseIf objUser.IsSS = "A" Then
                    drpSalesRep.Visible = False
                    chkSalesRep.Visible = False
                    lbl_Org.Visible = False
                    lbl_van.Visible = False
                    ddl_org.Visible = False
                    Panel1.Visible = False
                    lbl_Van1.Visible = False

                    SalesRepdiv.Visible = False
                    Orgdiv.Visible = False
                ElseIf objUser.IsSS = "Y" Or objUser.IsSS = "M" Then
                    lbl_Van1.Visible = False
                    drpSalesRep.Visible = False
                    chkSalesRep.Visible = True
                    lbl_Org.Visible = True
                    lbl_van.Visible = True
                    ddl_org.Visible = True
                    Panel1.Visible = True
                    SalesRepdiv.Visible = True
                    Orgdiv.Visible = True
                    drpSalesRep.SelectedValue = "0"
                    chkSalesRep.ClearSelection()
                    For Each li As RadListBoxItem In chkSalesRep.Items
                        For Each RepID As Integer In objUser.AssignedSalesRep
                            If li.Value = RepID.ToString() Then
                                li.Checked = True
                            End If
                        Next
                    Next
                Else
                    drpSalesRep.Visible = False
                    chkSalesRep.Visible = False
                    lbl_Org.Visible = False
                    lbl_van.Visible = False
                    ddl_org.Visible = False
                    Panel1.Visible = False
                    lbl_Van1.Visible = False
                    SalesRepdiv.Visible = False
                    Orgdiv.Visible = False
                End If
            End If
            Else
                DefaultMode()
            End If
    End Sub
    Protected Sub btnModify_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModify.Click
        If drpUser.SelectedValue = "0" Then
            MessageBoxValidation("Please select user.", "Validation")
            Return
        End If
        EditMode()
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button2.Click
        DefaultMode()
    End Sub

    Protected Sub Button4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button4.Click
        DefaultMode()
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
        Try
            If drpUser.SelectedValue = "0" Then
                MessageBoxValidation("Please select user.", "Validation")
            Else
                Dim Icount As Integer = 0
                For Each li As RadListBoxItem In chkSalesRep.Items
                    If li.Checked = True Then
                        Icount += 1
                    End If
                Next
                objUser.UserID = drpUser.SelectedValue
                objUser.DeleteUser(Err_No, Err_Desc)
                objLogin.SaveUserLog(Err_No, Err_Desc, "D", "USER MANAGEMENT", "MANAGE USER", drpUser.SelectedItem.Text, "User Name: " & drpUser.SelectedItem.Text & "/ User Type: " & Me.drpUserType.SelectedItem.Text & "/ Designation : " & Me.drpDesignation.SelectedItem.Text & "/ Assigned SalesRep : " & Icount, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                MessageBoxValidation("User deleted successfully.", "Information")
                FillUsers()
                DefaultMode()
            End If
        Catch ex As Exception

            MessageBoxValidation("Error occured while deleting user.", "Validation")
            log.Error(ex.Message)
        End Try

    End Sub
    'Sub MessageBoxValidation(ByVal str As String)
    '    lblMessage.ForeColor = Drawing.Color.Red
    '    lblMsg.ForeColor = Drawing.Color.Red
    '    lblinfo.Text = "Validation"
    '    lblMessage.Text = str
    '    MpInfoError.Show()
    '    Exit Sub
    'End Sub

    Protected Sub ddl_org_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_org.SelectedIndexChanged
        If drpDesignation.SelectedValue = "Y" Then
            FillSalesRepList()
        End If
    End Sub
End Class