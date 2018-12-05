Imports log4net
Imports SalesWorx.BO.Common
Imports System.IO
Imports Telerik.Web.UI

Partial Public Class ManageUserTypes
    Inherits System.Web.UI.Page
    Private Const ModuleName As String = "ManageUserTypes.aspx"
    Private Const PageID As String = "P17"
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objUser As New User
    Dim objLogin As New SalesWorx.BO.Common.Login
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblmsg.Text = ""
        If Not IsPostBack Then
            If Not Session.Item("USER_ACCESS") Is Nothing Then
                If Not HasAuthentication() Then
                    Err_No = 500
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
                End If
                FillUserTypes()
                FillDesignation()
                FillPDARights()
            Else
                Session.Add("BringmeBackHere", ModuleName)
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
        End If
    End Sub
    Private Function HasAuthentication() As Boolean
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
    End Function

    Sub FillPDARights()
        Me.ddlVanRights.ClearSelection()
        Me.ddlVanRights.Items.Clear()
        
        ddlVanRights.DataTextField = "Description"
        ddlVanRights.DataValueField = "Code"
        ddlVanRights.DataSource = objUser.GetPDARights(IIf(Me.drpDesignation.SelectedIndex <= 0, "0", Me.drpDesignation.SelectedValue))
        ddlVanRights.DataBind()

        For Each li As RadComboBoxItem In ddlVanRights.Items
            li.Checked = False
        Next
    End Sub
    Sub FillUserTypes()
        drpUserTypes.DataTextField = "User_Type"
        drpUserTypes.DataValueField = "User_Type_ID"
        drpUserTypes.DataSource = objUser.GetUserTypes()
        drpUserTypes.DataBind()
    End Sub

    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddNew.Click
        AddMode()
    End Sub
    Sub FillDesignation()
        drpDesignation.DataTextField = "val"
        drpDesignation.DataValueField = "id"
        drpDesignation.DataSource = objUser.GetIsSS()
        drpDesignation.DataBind()
    End Sub
    Sub AddMode()
        txtUserType.Text = ""
        Me.drpUserTypes.SelectedIndex = 0
        Me.drpDesignation.SelectedIndex = 0
        txtUserType.Visible = True
        drpUserTypes.Visible = False
        pnlAdd.Visible = True
        pnlModify.Visible = False
        pnlDefault.Visible = False
        TreeViewRights.Enabled = True
        ddlVanRights.Enabled = True
        For Each li As RadComboBoxItem In ddlVanRights.Items
            li.Checked = False
        Next
        LoadRights(0)

    End Sub
    Sub EditMode()
        txtUserType.Visible = True
        drpUserTypes.Visible = False
        pnlAdd.Visible = False
        pnlModify.Visible = True
        pnlDefault.Visible = False
        TreeViewRights.Enabled = True
        ddlVanRights.Enabled = True
        txtUserType.Text = drpUserTypes.SelectedItem.Text
    End Sub
    Sub DefaultMode()
        txtUserType.Visible = False
        drpUserTypes.Visible = True
        Me.drpUserTypes.SelectedIndex = 0
        Me.drpDesignation.SelectedIndex = 0
        pnlAdd.Visible = False
        pnlModify.Visible = False
        pnlDefault.Visible = True
        TreeViewRights.Enabled = False
        For Each li As RadComboBoxItem In ddlVanRights.Items
            li.Checked = False
        Next
        ddlVanRights.Enabled = False
    End Sub

    Protected Sub btnModify_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModify.Click
        If drpUserTypes.SelectedValue = "0" Then
            MessageBoxValidation("Please select user role.")
            Return
        End If
        EditMode()
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click, btnCancelM.Click
        DefaultMode()
        LoadRights(0)
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click

        Try
            If txtUserType.Text.Trim() = "" Then
                MessageBoxValidation("Please enter user role.")
                Return
            End If

            If Me.drpDesignation.SelectedIndex <= 0 Then
                MessageBoxValidation("Please select a designation.")
                Return
            End If
            objUser.UserType = txtUserType.Text.Trim()
            objUser.UserTypeDesignation = IIf(drpDesignation.SelectedIndex <= 0, "U", drpDesignation.SelectedValue)
            Dim dtRights As New DataTable
            dtRights = GetUserRights()
            Dim URights As String = Nothing
            For Each dr As DataRow In dtRights.Rows
                URights = URights & dr("Page_id").ToString() & ","
            Next

            If URights Is Nothing Then
                MessageBoxValidation("Please assign some back office rights to this role.")
                Return
            End If


            Dim collection As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddlVanRights.CheckedItems

            Dim vanRights As String = ""
            For Each li As Telerik.Web.UI.RadComboBoxItem In collection
                If String.IsNullOrEmpty(vanRights) Then
                    vanRights = li.Value
                Else
                    vanRights = vanRights & "," & li.Value
                End If

            Next


            objUser.SaveUserType(Err_No, Err_Desc, dtRights, vanRights)
            objLogin.SaveUserLog(Err_No, Err_Desc, "I", "USER MANAGEMENT", "MANAGE USER TYPE", Me.txtUserType.Text, "User Type: " & Me.txtUserType.Text & "/ Assigned Pages : " & URights & "/ Total Pages:" & dtRights.Rows.Count, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
            txtUserType.Text = ""
            Me.drpDesignation.SelectedIndex = 0

            DefaultMode()
            FillUserTypes()
            FillPDARights()
            LoadRights(0)
            lblmsg.Text = "User Type saved successfully."
        Catch ex As Exception
            If Err_No = 77007 Then
                lblmsg.Text = ex.Message.ToString()
            Else
                lblmsg.Text = "Error occured while saving."
            End If
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
    Sub LoadRights(ByVal UserTypeId As Integer)

        Try


            TreeViewRights.Nodes.Clear()
            TreeViewRights.DataSource = XmlDataSource1
            TreeViewRights.DataBind()
            For Each node As TreeNode In TreeViewRights.Nodes
                SetNodes(node)
            Next
            Dim dtRights As New DataTable
            objUser.UserTypeID = UserTypeId
            dtRights = objUser.GetUserRights()
            For Each dr As DataRow In dtRights.Rows
                FindNodeByValue(TreeViewRights.Nodes, dr(2))
                FindNodeByValue(TreeViewRights.Nodes, dr(3))
                FindNodeByValue(TreeViewRights.Nodes, dr(4))
            Next

            If dtRights.Rows.Count > 0 Then
                Dim PDA As String = dtRights.Rows(0)("PDARights").ToString()
                Me.drpDesignation.SelectedValue = dtRights.Rows(0)("Designation").ToString()
                FillPDARights()
                Dim s() As String = PDA.Split(",")
                For i As Integer = 0 To s.Length - 1
                    For Each li As RadComboBoxItem In ddlVanRights.Items
                        If li.Value = s(i).ToString() Then
                            li.Checked = True
                        End If
                    Next
                Next
            End If
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            Response.Redirect("Information.aspx?mode=1&errno=" & "740192" & "&msg= " & GetErrorMessage("E_Web_ManageUserType_001") & "&next=ManageUserType.aspx&Title=" & "Manage User Type", False)
        Finally

        End Try
    End Sub
    Private Sub SetNodes(ByVal node As TreeNode)
        node.SelectAction = TreeNodeSelectAction.None
        If node.ChildNodes.Count > 0 Then
            For Each ChildNode As TreeNode In node.ChildNodes
                SetNodes(ChildNode)
            Next
        End If
    End Sub

    Private Sub FindNodeByValue(ByVal n As TreeNodeCollection, ByVal val As String)
        Dim intNodeFound As Boolean
        If intNodeFound Then
            Return
        End If

        For i As Integer = 0 To n.Count - 1
            If n(i).Value = val Then
                n(i).[Select]()
                n(i).Checked = True
                intNodeFound = True
                Return
            End If
            ' n(i).Expand()
            FindNodeByValue(n(i).ChildNodes, val)
            If intNodeFound Then
                Return
            End If
            'n(i).Collapse()
        Next
    End Sub

    Private Function GetUserRights() As DataTable


        objUser.UserType = txtUserType.Text.Trim()
        Dim dtRights As New DataTable
        dtRights.Columns.Add("Menu_Id")
        dtRights.Columns.Add("Page_Id")
        dtRights.Columns.Add("Field_Rights")

        Try

            objUser.UserType = txtUserType.Text.Trim()
            Dim node As TreeNode
            Dim chlnode As TreeNode
            Dim subchlnode As TreeNode
            For Each node In TreeViewRights.Nodes
                Dim subchildchecked As Boolean = False
                If node.Checked = True Then
                    For Each chlnode In node.ChildNodes
                        If chlnode.Checked = True Then
                            Dim dr As DataRow
                            dr = dtRights.NewRow()
                            dr(0) = chlnode.Parent.Value
                            dr(1) = chlnode.Value
                            dr(2) = ""
                            dtRights.Rows.Add(dr)

                            For Each subchlnode In chlnode.ChildNodes
                                If subchlnode.Checked = True Then
                                    dr = dtRights.NewRow()
                                    dr(0) = subchlnode.Parent.Parent.Value
                                    dr(1) = subchlnode.Parent.Value
                                    dr(2) = subchlnode.Value
                                    dtRights.Rows.Add(dr)

                                End If
                            Next

                        Else
                            For Each subchlnode In chlnode.ChildNodes
                                If subchlnode.Checked = True Then
                                    subchildchecked = True
                                    Exit For
                                End If
                            Next
                            If subchildchecked Then
                                Dim dr As DataRow
                                dr = dtRights.NewRow()
                                dr(0) = chlnode.Parent.Value
                                dr(1) = chlnode.Value
                                dr(2) = ""
                                dtRights.Rows.Add(dr)


                                For Each subchlnode In chlnode.ChildNodes
                                    If subchlnode.Checked = True Then
                                        dr = dtRights.NewRow()
                                        dr(0) = subchlnode.Parent.Parent.Value
                                        dr(1) = subchlnode.Parent.Value
                                        dr(2) = subchlnode.Value
                                        dtRights.Rows.Add(dr)
                                    End If
                                Next


                            End If

                        End If
                    Next
                Else
                    Dim childchecked As Boolean = False
                    For Each chlnode In node.ChildNodes
                        If chlnode.Checked = True Then
                            childchecked = True
                            Exit For
                        End If
                    Next
                    If childchecked = True Then

                        For Each chlnode In node.ChildNodes
                            Dim dr As DataRow
                            dr = dtRights.NewRow()
                            dr(0) = chlnode.Parent.Value
                            dr(1) = chlnode.Value
                            dr(2) = ""
                            dtRights.Rows.Add(dr)


                            For Each subchlnode In chlnode.ChildNodes
                                If subchlnode.Checked = True Then
                                    dr = dtRights.NewRow()
                                    dr(0) = subchlnode.Parent.Parent.Value
                                    dr(1) = subchlnode.Parent.Value
                                    dr(2) = subchlnode.Value
                                    dtRights.Rows.Add(dr)

                                End If
                            Next

                        Next
                    Else
                        ''in case no child is checked -- check if any subchilds are checked
                        subchildchecked = False
                        For Each chlnode In node.ChildNodes

                            For Each subchlnode In chlnode.ChildNodes
                                If subchlnode.Checked = True Then
                                    subchildchecked = True
                                    Exit For
                                End If
                            Next

                        Next

                        If subchildchecked Then

                            For Each chlnode In node.ChildNodes

                                For Each subchlnode In chlnode.ChildNodes
                                    If subchlnode.Checked = True Then
                                        Dim dr As DataRow
                                        dr = dtRights.NewRow()
                                        dr(0) = subchlnode.Parent.Parent.Value
                                        dr(1) = subchlnode.Parent.Value
                                        dr(2) = subchlnode.Value
                                        dtRights.Rows.Add(dr)

                                    End If
                                Next

                            Next


                        End If

                    End If
                End If
            Next




        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        Finally

        End Try
        Return dtRights
    End Function

    Protected Sub drpUserTypes_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles drpUserTypes.SelectedIndexChanged
        TreeViewRights.Enabled = True
        ddlVanRights.Enabled = True
        LoadRights(drpUserTypes.SelectedValue)
        TreeViewRights.Enabled = False
        ddlVanRights.Enabled = False
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click
        Try
            If Me.drpDesignation.SelectedIndex <= 0 Then
                MessageBoxValidation("Please select a designation.")
                Return
            End If
            If drpUserTypes.SelectedValue = "0" Then
                MessageBoxValidation("Please select user role.")
            ElseIf txtUserType.Text.Trim() = "" Then
                MessageBoxValidation("Please enter user role.")
            Else
                objUser.UserType = txtUserType.Text.Trim()
                Dim dtRights As New DataTable
                dtRights = GetUserRights()
                Dim URights As String = Nothing
                For Each dr As DataRow In dtRights.Rows
                    URights = URights & dr("Page_id").ToString() & ","
                Next
                If URights Is Nothing Then
                    MessageBoxValidation("Please assign some back office rights to this role.")
                    Return
                End If
                objUser.UserTypeID = drpUserTypes.SelectedValue
                objUser.UserTypeDesignation = IIf(drpDesignation.SelectedIndex <= 0, "U", drpDesignation.SelectedValue)
                Dim collection As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddlVanRights.CheckedItems

                Dim vanRights As String = ""
                For Each li As Telerik.Web.UI.RadComboBoxItem In collection
                    If String.IsNullOrEmpty(vanRights) Then
                        vanRights = li.Value
                    Else
                        vanRights = vanRights & "," & li.Value
                    End If

                Next



                objUser.UpdateUserRights(Err_No, Err_Desc, dtRights, vanRights)
                objLogin.SaveUserLog(Err_No, Err_Desc, "U", "USER MANAGEMENT", "MANAGE USER TYPE", Me.txtUserType.Text, "User Type: " & Me.txtUserType.Text & "/ Assigned Pages : " & URights & "/ Total Pages:" & dtRights.Rows.Count, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                txtUserType.Text = ""
                Me.drpDesignation.SelectedIndex = 0
                DefaultMode()
                FillUserTypes()
                FillPDARights()
                drpUserTypes.SelectedValue = objUser.UserTypeID
                LoadRights(objUser.UserTypeID)
                lblmsg.Text = "User type updated successfully."
            End If
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            lblmsg.Text = "Error while updating user type."
        End Try
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
        Try
            If drpUserTypes.SelectedValue = "0" Then
                MessageBoxValidation("Please select user role.")
                Return
            End If

            Dim dtRights As New DataTable
            dtRights = GetUserRights()
            Dim URights As String = Nothing
            For Each dr As DataRow In dtRights.Rows
                URights = URights & dr("Page_id").ToString() & ","
            Next

            objUser.UserTypeID = drpUserTypes.SelectedValue
            If objUser.DeleteUserType(Err_No, Err_Desc) = True Then
                lblmsg.Text = "User type deleted successfully."
                objLogin.SaveUserLog(Err_No, Err_Desc, "D", "USER MANAGEMENT", "MANAGE USER TYPE", drpUserTypes.SelectedItem.Text, "User Type: " & drpUserTypes.SelectedItem.Text & "/ Assigned Pages : " & URights & "/ Total Pages:" & dtRights.Rows.Count, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
            Else
                lblmsg.Text = Err_Desc
                Exit Sub
            End If
            Me.drpDesignation.SelectedIndex = 0

            FillUserTypes()
            LoadRights(0)

        Catch ex As Exception
            log.Error(ex.Message.ToString())
            lblmsg.Text = "Error while deleting user type."
        End Try
    End Sub

    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblmsg.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        Exit Sub
    End Sub




    Protected Sub drpDesignation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpDesignation.SelectedIndexChanged
        FillPDARights()
    End Sub
End Class