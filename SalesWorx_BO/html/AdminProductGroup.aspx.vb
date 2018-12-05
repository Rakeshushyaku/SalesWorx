Imports log4net
Imports SalesWorx.BO.Common
Imports Telerik.Web.UI
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports ExcelLibrary.SpreadSheet
Partial Public Class AdminProductGroup
    Inherits System.Web.UI.Page

    Private dtErrors As New DataTable
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const ModuleName As String = "AdminProductGroup.aspx"
    Private Const PageID As String = "P376"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim objProduct As New Product
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session.Item("USER_ACCESS") Is Nothing Then
            Session.Add("BringmeBackHere", ModuleName)
            Response.Redirect("Login.aspx", False)
            Exit Sub
        End If
        If Not IsPostBack Then
            If Not HasAuthentication() Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If

            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            If Not HasPermission Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If

            Dim objCommon As New Common
            Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
            ddl_org.ClearSelection()
            ddl_org.Items.Clear()

            ddl_org.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
            ddl_org.DataValueField = "MAS_Org_ID"
            ddl_org.DataTextField = "Description"
            ddl_org.DataBind()
            ddl_org.Items.Insert(0, New RadComboBoxItem("Select Organisation", 0))

            lstDefault.Items.Clear()
            lstSelected.Items.Clear()


            If Request.QueryString("Desc").ToString() = "New" Then
                Me.lblGroupId.Text = "0"
                Me.lblGroupName.Text = "0"
                Me.ddl_org.Enabled = True
            Else
                Me.txtGroupName.Text = Request.QueryString("Desc").ToString()
                Me.lblGroupName.Text = Me.txtGroupName.Text
                Me.lblGroupId.Text = Request.QueryString("PGID").ToString()
                Me.ddl_org.SelectedValue = Request.QueryString("ORGID").ToString()
                If Me.ddl_org.SelectedIndex > 0 Then
                    Me.ddl_org.Enabled = False
                Else
                    Me.ddl_org.Enabled = True
                End If

                BindDefault()
                BindSelected()

            End If

        Else

            dtErrors = Session("dtAPErrors")

        End If

    End Sub
    Private Function HasAuthentication() As Boolean
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
    End Function
    Private Sub ddOraganisation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_org.SelectedIndexChanged
        'Try
        If ddl_org.SelectedIndex > 0 Then
            BindDefault()


            BindSelected()


        Else
            lstDefault.Items.Clear()
            lstSelected.Items.Clear()
            lblProdAssign.Text = ""
            lblProdAvailed.Text = ""
        End If

    End Sub




    Private Sub BindDefault()
        Me.lblSelectedID.Text = ""
        Me.lblRemovedID.Text = ""
        Dim objProd As New Product
        Dim TempTbl As New DataTable
        Try
            Err_Desc = Nothing
            Err_No = Nothing
            TempTbl = objProd.GetDefaultProdctGroup(IIf(Me.ddl_org.SelectedIndex <= 0, "0", Me.ddl_org.SelectedItem.Value), Me.lblGroupId.Text, Err_No, Err_Desc)
            If Me.txtFilter.Text <> "" Then
                TempTbl.DefaultView.RowFilter = "(Description LIKE '%" & Me.txtFilter.Text & "%')"
            End If

            Dim dv As New DataView
            dv = TempTbl.DefaultView
            dv.Sort = "Description"
            If TempTbl IsNot Nothing Then
                lstDefault.DataSource = dv
                lstDefault.DataTextField = "Description"
                lstDefault.DataValueField = "Inventory_Item_ID"
                lstDefault.DataBind()
            End If
            lblProdAvailed.Text = "Products Available: [" & lstDefault.Items.Count & "]"
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        Finally
            objProd = Nothing
        End Try
    End Sub
    Private Function ValidationPage() As Boolean
        Dim success As Boolean = False
        Dim objProd As New Product
        Dim r As DataRow = Nothing
        Dim GroupName As String = Me.txtGroupName.Text
        r = objProd.CheckProductGroup(GroupName, Err_No, Err_Desc)
        If Not r Is Nothing Then
            ' Me.lblGroupId.Text = r(0).ToString()
            'Me.txtGroupName.Text = r(1).ToString()
            If Me.lblGroupName.Text <> r(1).ToString() Then
                success = True
            End If
        End If
        Return success

    End Function
    Protected Sub imgAddSlected_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Me.ddl_org.SelectedIndex > 0 Then

            If Me.txtGroupName.Text <> "" And Me.txtGroupName.Text <> "0" Then

                If ValidationPage() = True Then
                    MessageBoxValidation("Group name already exist", "Validation")
                    ClassUpdatePnl.Update()

                    Exit Sub
                End If
                Try
                    Err_No = Nothing
                    Err_Desc = Nothing


                    Dim objProd As New Product
                    For Each Item As RadListBoxItem In lstDefault.Items
                        If Item.Selected Then
                            If Item.Value <> "0" Then
                                objProd.InsertProductGroup(Me.ddl_org.SelectedItem.Value, Item.Value, 0, Me.lblGroupId.Text, Me.txtGroupName.Text, Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID)
                                GetGroupName()
                            End If
                        End If
                    Next
                    'GetGroupName()
                    BindDefault()
                    BindSelected()
                Catch ex As Exception
                    log.Error(ex.Message)
                    Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Product List Setup_006") & "&next=AssignProductToFSR.aspx&Title=Assign Product To FSR", False)
                End Try
            Else
                MessageBoxValidation("Please enter a group name", "Validation")
            
                ClassUpdatePnl.Update()

            End If
        Else
            MessageBoxValidation("Please select a organization", "Validation")
            ClassUpdatePnl.Update()

        End If

    End Sub
    Private Sub GetGroupName()
        Dim objProd As New Product
        Dim r As DataRow = Nothing
        Dim GroupName As String = Me.txtGroupName.Text
        r = objProd.CheckProductGroup(GroupName, Err_No, Err_Desc)
        If Not r Is Nothing Then
            Me.lblGroupId.Text = r(0).ToString()
            Me.txtGroupName.Text = r(1).ToString()
            Me.lblGroupName.Text = r(1).ToString()
        End If
    End Sub
    Protected Sub imgRemoveSlected_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Me.ddl_org.SelectedIndex > 0 Then
            If Me.txtGroupName.Text <> "" And Me.txtGroupName.Text <> "0" Then
                If ValidationPage() = True Then
                    MessageBoxValidation("Group name already exist", "Validation")
                    ClassUpdatePnl.Update()
                    Exit Sub
                End If
                Try
                    Err_No = Nothing
                    Err_Desc = Nothing
                    Dim objProd As New Product

                    For Each Item As RadListBoxItem In lstSelected.Items
                        If Item.Selected Then
                            objProd.DeleteProductGroup("Single", ddl_org.SelectedItem.Value, Item.Value, 0, Me.lblGroupId.Text, Err_No, Err_Desc, txtGroupName.Text)
                            'bAdded = True
                            'ValidationPage()
                        End If
                    Next
                    BindDefault()
                    BindSelected()
                Catch ex As Exception
                    log.Error(ex.Message)
                    Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_AdminProductGroup_006") & "&next=AdminProductGroup.aspx&Title=Product Group", False)

                End Try
            Else
                MessageBoxValidation("Please enter a group name", "Validation")
                ClassUpdatePnl.Update()

            End If
        Else
            MessageBoxValidation("Please select a organization", "Validation")
            ClassUpdatePnl.Update()
            Exit Sub
        End If
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub imgMoveAllLeft_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Me.ddl_org.SelectedIndex > 0 Then
            If Me.txtGroupName.Text <> "0" And Me.txtGroupName.Text <> "" Then
                If ValidationPage() = True Then
                    Me.txtGroupName.Text = ""
                    MessageBoxValidation("Group name already exist", "Validation")
                    ClassUpdatePnl.Update()
                    Exit Sub
                End If
                Try
                    Err_No = Nothing
                    Err_Desc = Nothing


                    Dim objProd As New Product
                    For Each Item As RadListBoxItem In lstDefault.Items
                        If Item.Value <> "0" Then
                            objProd.InsertProductGroup(Me.ddl_org.SelectedItem.Value, Item.Value, 0, Me.lblGroupId.Text, Me.txtGroupName.Text, Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID)
                            GetGroupName()
                        End If
                    Next
                    'GetGroupName()
                    BindDefault()
                    BindSelected()
                Catch ex As Exception
                    log.Error(ex.Message)
                    Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_AdminMSL_005") & "&next=AdminProductGroup.aspx&Title=Must Stock List", False)
                End Try
            Else
                MessageBoxValidation("Please enter a group name", "Validation")
                ClassUpdatePnl.Update()

            End If
        Else
            MessageBoxValidation("Please select a organization.", "Validation")
            ClassUpdatePnl.Update()
            Exit Sub
        End If

    End Sub
    Protected Sub imgMoveAllRight_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Me.ddl_org.SelectedIndex > 0 Then
            If Me.txtGroupName.Text <> "" And Me.txtGroupName.Text <> "0" Then
                If ValidationPage() = True Then
                    MessageBoxValidation("Group name already exist", "Validation")
                    ClassUpdatePnl.Update()
                    Exit Sub
                End If
                Try
                    Err_No = Nothing
                    Err_Desc = Nothing

                    Dim objProd As New Product
                    'For Each Item As RadListBoxItem  In lstSelected.Items
                    objProd.DeleteProductGroup("ALL", ddl_org.SelectedItem.Value, 0, 0, Me.lblGroupId.Text, Err_No, Err_Desc, txtGroupName.Text)
                    'bAdded = True
                    'ValidationPage()
                    'Next
                    BindDefault()
                    BindSelected()
                Catch ex As Exception
                    log.Error(ex.Message)
                    Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_AdminMSL_006") & "&next=AdminProductGroup.aspx&Title=Must Stock List", False)

                End Try
            Else
                MessageBoxValidation("Please enter a group name", "Validation")
                ClassUpdatePnl.Update()

            End If
        Else
            MessageBoxValidation("Please select a organization", "Validation")
            ClassUpdatePnl.Update()
            Exit Sub

        End If
    End Sub
    Private Sub BindSelected()
        Me.lblSelectedID.Text = ""
        Me.lblRemovedID.Text = ""
        Dim objProd As New Product
        Dim TempTbl As New DataTable
        Try
            Err_Desc = Nothing
            Err_No = Nothing

            TempTbl = objProd.GetSelectedProductGroup(IIf(Me.ddl_org.SelectedIndex <= 0, "0", Me.ddl_org.SelectedItem.Value), Me.lblGroupId.Text, Err_No, Err_Desc)
            If Me.txtFilter.Text <> "" Then
                TempTbl.DefaultView.RowFilter = "(Description LIKE '%" & Me.txtFilter.Text & "%')"
            End If

            Dim dv As New DataView
            dv = TempTbl.DefaultView
            'dv.Sort = "Description"
            If TempTbl IsNot Nothing Then
                lstSelected.DataSource = dv
                lstSelected.DataTextField = "Description"
                lstSelected.DataValueField = "Inventory_Item_ID"
                lstSelected.DataBind()
            End If

            Dim ItmSelected As Integer = 0
            For Each Item As RadListBoxItem In lstSelected.Items
                If Item.Value <> "0" Then
                    ItmSelected += 1
                End If
            Next


            lblProdAssign.Text = "Products Assigned: [" & ItmSelected & "]"
            lblProdAvailed.Text = "Products Available: [" & lstDefault.Items.Count & "]"
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        Finally
            objProd = Nothing
        End Try
    End Sub

    Private Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        If Me.ddl_org.SelectedIndex > 0 Then
            BindDefault()
            BindSelected()
        Else
            MessageBoxValidation("Please select a organization", "Validation")
            ClassUpdatePnl.Update()
            Exit Sub
        End If
    End Sub

    Private Sub Btn_back_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_back.Click
        Response.Redirect("ProductGroupListing.aspx")
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReset.Click
        If Me.ddl_org.SelectedIndex > 0 Then
            txtFilter.Text = ""
            BindDefault()
            BindSelected()
        Else
            MessageBoxValidation("Please select a organization", "Validation")
            ClassUpdatePnl.Update()
            Exit Sub
        End If
    End Sub

End Class