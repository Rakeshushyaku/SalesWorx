Imports log4net
Imports SalesWorx.BO.Common
Imports Telerik.Web.UI
Partial Public Class AssignProductGroupToFSR
    Inherits System.Web.UI.Page

    Private dtErrors As New DataTable
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const ModuleName As String = "AssignProductGroupToFSR.aspx"
    Private Const PageID As String = "P378"
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
                ddl_org.Items.Insert(0, "--Select Organisation--")

               
                If ddl_org.Items.Count = 2 Then
                    Me.ddl_org.SelectedIndex = 1
                    LoadFSR()

                End If

                'Me.ddlSalesRep.Items.Clear()
                'ddlSalesRep.AppendDataBoundItems = True
                'ddlSalesRep.Items.Insert(0, "--Select a FSR--")
                'ddlSalesRep.Items(0).Value = ""

                lstDefault.Items.Clear()
                lstSelected.Items.Clear()
                

            End If

    End Sub
Private Sub LoadFSR()
        If ddl_org.SelectedIndex > 0 Then
            Dim objCommon As New Common
            ddlSalesRep.ClearSelection()
            ddlSalesRep.Items.Clear()

            ddlSalesRep.DataSource = objCommon.GetVanByOrg(Err_No, Err_Desc, ddl_org.SelectedValue, CType(Session("User_Access"), UserAccess).UserID)

            ddlSalesRep.DataValueField = "SalesRep_Id"
            ddlSalesRep.DataTextField = "SalesRep_Name"
            ddlSalesRep.DataBind()
            ddlSalesRep.Items.Insert(0, "--Select FSR--")
            If ddlSalesRep.Items.Count = 2 Then
                ddlSalesRep.SelectedIndex = 1
            End If
            If ddl_org.SelectedIndex > 0 And ddlSalesRep.SelectedIndex > 0 Then
                ''Bind Default List
                BindDefault()

                ''Bind Selected List
                BindSelected()

                'lblProdAssign.Text = "Products Assigned: [" & lstSelected.Items.Count & "]"
                'lblProdAvailed.Text = "Products Available: [" & lstDefault.Items.Count & "]"
            Else
                lstDefault.Items.Clear()
                lstSelected.Items.Clear()
                lblProdAssign.Text = ""
                lblProdAvailed.Text = ""
            End If
        Else
            ddlSalesRep.ClearSelection()
            ddlSalesRep.Items.Clear()

            ddlSalesRep.DataValueField = "SalesRep_Id"
            ddlSalesRep.DataTextField = "SalesRep_Name"
            ddlSalesRep.DataBind()
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

            Dim OrgID As String = "0"
            Dim SID As String = "0"

            If ddl_org.SelectedIndex > 0 Then
                OrgID = ddl_org.SelectedValue
            End If

            If ddlSalesRep.SelectedIndex > 0 Then
                SID = ddlSalesRep.SelectedValue
            End If


            TempTbl = objProd.GetDefaultProdctGroupFSR(OrgID, SID, Err_No, Err_Desc)
            If TempTbl IsNot Nothing Then
                lstDefault.DataSource = TempTbl
                lstDefault.DataTextField = "Description"
                lstDefault.DataValueField = "PG_ID"
                lstDefault.DataBind()
            End If

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        Finally
            objProd = Nothing
        End Try
    End Sub
     Protected Sub imgAddSlected_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Me.ddl_org.SelectedIndex > 0 And ddlSalesRep.SelectedIndex > 0 Then
            Try
                Err_No = Nothing
                Err_Desc = Nothing

                Dim msg1 As String = ""
                Dim objProd As New Product
                For Each Item As RadListBoxItem In lstDefault.Items
                    If Item.Selected Then
                        If Item.Value <> "0" Then
                            Dim msg As String = ""
                            objProd.InsertProductGroupFSR(Me.ddl_org.SelectedItem.Value, Item.Value, Me.ddlSalesRep.SelectedItem.Value, Err_No, Err_Desc, "Single", msg)
                            If msg1 = "" Then
                                msg1 = msg
                            End If
                        End If
                    End If
                Next
                If msg1 <> "" Then
                        lblMessage.Text = msg1
                                lblMessage.ForeColor = Drawing.Color.Green
                                lblinfo.Text = "Information"
                                MpInfoError.Show()
                                btnClose.Focus()
                                ClassUpdatePnl.Update()
                End If
                BindDefault()
                BindSelected()
            Catch ex As Exception
                log.Error(ex.Message)
                Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_AdminProductGroup_005") & "&next=AdminProductGroup.aspx&Title=Product Group", False)
            End Try
        Else

            lblMessage.Text = "Please select a organization/FSR"
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
                ClassUpdatePnl.Update()
            Exit Sub
        End If
    End Sub
Protected Sub imgMoveAllLeft_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Me.ddl_org.SelectedIndex > 0 And ddlSalesRep.SelectedIndex > 0 Then
            Try
                Err_No = Nothing
                Err_Desc = Nothing


                Dim objProd As New Product
                'For Each Item As ListItem In lstDefault.Items
                'If Item.Value <> "0" Then
                Dim msg As String = ""
                objProd.InsertProductGroupFSR(Me.ddl_org.SelectedItem.Value, 0, Me.ddlSalesRep.SelectedItem.Value, Err_No, Err_Desc, "ALL", msg)
                'End If
                'Next
                If msg <> "" Then
                        lblMessage.Text = msg
                                lblMessage.ForeColor = Drawing.Color.Green
                                lblinfo.Text = "Information"
                                MpInfoError.Show()
                                btnClose.Focus()
                                ClassUpdatePnl.Update()
                End If
                BindDefault()
                BindSelected()
            Catch ex As Exception
                log.Error(ex.Message)
                Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_AdminMSL_005") & "&next=AssignProductGroupToFSR.aspx&Title=Must Stock List", False)
            End Try
        Else
            lblMessage.Text = "Please select a organization/FSR"
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
                ClassUpdatePnl.Update()
            Exit Sub
        End If
    End Sub

    Protected Sub imgMoveAllRight_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Me.ddl_org.SelectedIndex > 0 And ddlSalesRep.SelectedIndex > 0 Then
            Try
                Err_No = Nothing
                Err_Desc = Nothing

                Dim objProd As New Product
                ' For Each Item As ListItem In lstSelected.Items
                objProd.DeleteProductGroupFSR("ALL", ddl_org.SelectedItem.Value, 0, Me.ddlSalesRep.SelectedItem.Value, Err_No, Err_Desc)
                ' Next
                BindDefault()
                BindSelected()
            Catch ex As Exception
                log.Error(ex.Message)
                Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_AdminMSL_006") & "&next=AssignProductGroupToFSR.aspx&Title=Must Stock List", False)

            End Try
        Else
           lblMessage.Text = "Please select a organization/FSR"
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
                ClassUpdatePnl.Update()
            Exit Sub

        End If
    End Sub
    Protected Sub imgRemoveSlected_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Me.ddl_org.SelectedIndex > 0 And ddlSalesRep.SelectedIndex > 0 Then

            Try
                Err_No = Nothing
                Err_Desc = Nothing
                Dim objProd As New Product

                For Each Item As RadListBoxItem In lstSelected.Items
                    If Item.Selected Then
                        objProd.DeleteProductGroupFSR("Single", ddl_org.SelectedItem.Value, Item.Value, Me.ddlSalesRep.SelectedItem.Value, Err_No, Err_Desc)

                    End If
                Next
                BindDefault()
                BindSelected()
            Catch ex As Exception
                log.Error(ex.Message)
                Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_AdminProductGroup_006") & "&next=AdminProductGroup.aspx&Title=Product Group", False)

            End Try
        Else
         lblMessage.Text = "Please select a organization/FSR"
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
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

            Dim OrgID As String = "0"
            Dim SID As String = "0"

            If ddl_org.SelectedIndex > 0 Then
                OrgID = ddl_org.SelectedValue
            End If

            If ddlSalesRep.SelectedIndex > 0 Then
                SID = ddlSalesRep.SelectedValue
            End If


            TempTbl = objProd.GetSelectedProductGroupFSR(OrgID, SID, Err_No, Err_Desc)
            If TempTbl IsNot Nothing Then
                lstSelected.DataSource = TempTbl
                lstSelected.DataTextField = "Description"
                lstSelected.DataValueField = "PG_ID"
                lstSelected.DataBind()
            End If

            Dim ItmSelected As Integer = 0
            For Each Item As RadListBoxItem In lstSelected.Items
                If Item.Value <> "0" Then
                    ItmSelected += 1
                End If
            Next


            lblProdAssign.Text = "Groups Assigned: [" & ItmSelected & "]"
            lblProdAvailed.Text = "Groups Available: [" & lstDefault.Items.Count & "]"
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        Finally
            objProd = Nothing
        End Try
    End Sub
     Private Function HasAuthentication() As Boolean
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
    End Function

    Private Sub ddl_org_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_org.SelectedIndexChanged
        LoadFSR()
    End Sub

    Private Sub ddlSalesRep_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSalesRep.SelectedIndexChanged
Try
            If ddl_org.SelectedIndex > 0 And ddlSalesRep.SelectedIndex > 0 Then ' And Me.ddcategory.SelectedIndex > 0 Then

                    BindDefault()



                BindSelected()

            Else
                lstDefault.Items.Clear()
                lstSelected.Items.Clear()
                lblProdAssign.Text = ""
                lblProdAvailed.Text = ""
            End If
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=Welcome.aspx&Title=Product List Setup", False)
        End Try

    End Sub
End Class