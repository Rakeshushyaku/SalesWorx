Imports SalesWorx.BO.Common
Imports System.Data
Imports System.Web
Imports System.Resources
Imports System.Web.UI.WebControls
Imports log4net
Imports System.Text.RegularExpressions
Imports System.IO
Imports Telerik.Web.UI

Partial Public Class AdminOrgLvlDiscountRule
    Inherits System.Web.UI.Page

    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim objCommon As New SalesWorx.BO.Common.Common
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim Dt As New DataTable
    Private Const PageID As String = "P308"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        If Not Page.IsPostBack Then
            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            If Not HasPermission Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If
            BindData()
            Resetfields()
        Else
            MPEDetails.VisibleOnPageLoad = False
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Try
            MPEDetails.VisibleOnPageLoad = False
            Resetfields()
            ' ClassUpdatePnl.Update()
        Catch

        End Try
    End Sub

    Public Sub Resetfields()

        Me.txt_min.Text = ""
        Me.txt_max.Text = ""
        txt_OrderValue.Text = ""
        ddl_Client.ClearSelection()
        Me.btnSave.Text = "Save"
        'Me.btnAdd.Focus()
        ''Me.lblMessage.Text = ""

    End Sub
      Function Validatetxt() As Boolean
        Dim bRetVal As Boolean = True
        If Me.txt_OrderValue.Text.Trim = "" Then

            Me.lblMessage.Text = "Minumum Order Value is required."
            MPEDetails.VisibleOnPageLoad = True
            bRetVal = False
        End If
        If Me.txt_min.Text.Trim = "" Then

            Me.lblMessage.Text = "Minumum Percentage is required."
            MPEDetails.VisibleOnPageLoad = True
            bRetVal = False
        End If

         If Me.txt_max.Text.Trim = "" Then

            Me.lblMessage.Text = "Maximum Percentage is required."
              MPEDetails.VisibleOnPageLoad = True
            bRetVal = False
        End If
        If Val(txt_min.Text) > 100 Then

            Me.lblMessage.Text = "Minimum Percentage can not be greater than 100."
            Me.lblMessage.ForeColor = Drawing.Color.Red
             MPEDetails.VisibleOnPageLoad = True
            bRetVal = False
        End If
        If Val(txt_max.Text) > 100 Then

            Me.lblMessage.Text = "Maximum Percentage can not be greater than 100."
            Me.lblMessage.ForeColor = Drawing.Color.Red
             MPEDetails.VisibleOnPageLoad = True
            bRetVal = False
        End If
        If Val(txt_min.Text) > Val(txt_max.Text) Then

            Me.lblMessage.Text = "Minimum Percentage can not be more than Maximum Percentage."
            Me.lblMessage.ForeColor = Drawing.Color.Red
              MPEDetails.VisibleOnPageLoad = True
            bRetVal = False
        End If

Return bRetVal
End Function
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim type As String = ""
        
        If Me.ddl_Client.SelectedItem.Value = "0" Then

            Me.lblMessage.Text = "Please Select Organisation"
             MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If

          If Validatetxt() = False Then
                Exit Sub
           End If
          
        Dim success As Boolean = False
        Try
            If objCommon.SaveDiscountRule(Err_No, Err_Desc, ddl_Client.SelectedItem.Value, txt_min.Text.Trim, txt_max.Text.Trim, txt_OrderValue.Text.Trim, CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
                success = True
                MPEDetails.VisibleOnPageLoad = False
                MessageBoxValidation("Successfully saved.", "Information")
                BindData()
                ClassUpdatePnl.Update()
            Else
                success = False
                MPEDetails.VisibleOnPageLoad = False
                MessageBoxValidation("Error while saving", "Information")

            End If

            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "ORG DISCOUNT", ddl_Client.SelectedItem.Value, Me.ddl_Client.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")

                Resetfields()
            End If

        Catch ex As Exception
            Err_No = "74061"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Private Sub BindData()
        Dt = objCommon.GetDiscountRule(Err_No, Err_Desc)

        Dim dv As New DataView(Dt)
        If ViewState("SortField") <> "" Then
            dv.Sort = (ViewState("SortField") & " ") + SortDirection
        End If

        
        grdCustomerSegment.DataSource = dv
        grdCustomerSegment.DataBind()

    End Sub
    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
           
        If Me.ddl_Client.SelectedItem.Value = "0" Then

            Me.lblMessage.Text = "Please Select Organisation"
            MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If


           
           If Validatetxt() = False Then
                Exit Sub
           End If
          
        Dim success As Boolean = False
        Try
            
            If objCommon.SaveDiscountRule(Err_No, Err_Desc, ddl_Client.SelectedItem.Value, txt_min.Text.Trim, txt_max.Text.Trim, txt_OrderValue.Text.Trim, CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
                    success = True

                MPEDetails.VisibleOnPageLoad = False
                MessageBoxValidation("Successfully Saved.", "Information")
                BindData()
                ClassUpdatePnl.Update()
            Else
                success = False
                MPEDetails.VisibleOnPageLoad = False
                MessageBoxValidation("Error while saving", "Information")
                   
            End If

            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "ORG DISCOUNT", ddl_Client.SelectedItem.Value, Me.ddl_Client.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                Resetfields()
            End If
          
        Catch ex As Exception
            Err_No = "74062"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Protected Sub btnDeleteAll_Click()
        Try
         
            Dim dr As GridViewRow
            Dim Success As Boolean = False
            For Each dr In grdCustomerSegment.Rows
                Dim RowCheckBox As System.Web.UI.WebControls.CheckBox = dr.FindControl("chkDelete")

                If RowCheckBox.Checked = True Then
                    Dim Lbl As System.Web.UI.WebControls.Label = dr.FindControl("lblRow_ID")
                    HidVal.Value = Lbl.Text
                    Dim Des = dr.Cells(1).Text
                    If objCommon.DeleteCustomInfo(Err_No, Err_Desc, HidVal.Value) = True Then
                        objLogin.SaveUserLog(Err_No, Err_Desc, "D", "MAIN MENU", "ORG DISCOUNT", lbl_Info_Key.Text, Des, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                        Success = True
                    End If
                End If
            Next
            If (Success = True) Then
                
                MessageBoxValidation("Discount Rule(s) deleted successfully.", "Information")
                BindData()
                Resetfields()
            Else
                MessageBoxValidation("Discount Rule(s) could not be deleted.", "Information")
                Resetfields()
            End If

            'ClassUpdatePnl.Update()
        Catch ex As Exception
            Err_No = "74063"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
     

        Dim btndelete As ImageButton = TryCast(sender, ImageButton)
        Dim row As GridViewRow = DirectCast(btndelete.NamingContainer, GridViewRow)

        Dim success As Boolean = False

        Dim Lbl As System.Web.UI.WebControls.Label = row.FindControl("lblRow_ID")
        Dim lbl_Info_Key As System.Web.UI.WebControls.Label = row.FindControl("lbl_Info_Key")
        HidVal.Value = Lbl.Text

        Try

            If objCommon.DeleteCustomInfo(Err_No, Err_Desc, HidVal.Value) = True Then
                        success = True
            End If
            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "D", "MAIN MENU", "ORG DISCOUNT", lbl_Info_Key.Text, row.Cells(1).Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                MessageBoxValidation("Successfully deleted.", "Information")
                BindData()
                Resetfields()
            Else
                
                MessageBoxValidation("Error occured.Please try again", "Information")
                Resetfields()
            End If

        Catch ex As Exception
            Err_No = "74065"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        lblMessage.Text = ""
        bindClient()
        btnUpdate.Visible = False
        btnSave.Visible = True
        ddl_Client.Enabled = True
        Resetfields()
        ClassUpdatePnl.Update()
        MPEDetails.VisibleOnPageLoad = True
    End Sub
Sub bindClient()
        ddl_Client.DataSource = objCommon.GetOrgsWthNoDiscount(Err_No, Err_Desc)
        ddl_Client.DataTextField = "Description"
        ddl_Client.DataValueField = "ORG_HE_ID"
        ddl_Client.DataBind()
        ddl_Client.Items.Insert(0, New RadComboBoxItem("(Select)", "0"))
        ddl_Client.Items(0).Value = "0"
        ClassUpdatePnl.Update()
   End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            lblMessage.Text = ""
            btnUpdate.Visible = True
            btnSave.Visible = False
            Resetfields()
            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
            Dim Lbl As System.Web.UI.WebControls.Label = row.FindControl("lblRow_ID")
            Dim LblInfo_Key As System.Web.UI.WebControls.Label = row.FindControl("lbl_Info_Key")
            HidVal.Value = Lbl.Text
            ddl_Client.Items.Add(New RadComboBoxItem(row.Cells(1).Text, LblInfo_Key.Text))
            If Not ddl_Client.FindItemByValue(LblInfo_Key.Text) Is Nothing Then
                ddl_Client.FindItemByValue(LblInfo_Key.Text).Selected = True
            End If
            ddl_Client.Enabled = False
            txt_OrderValue.Text = Trim(row.Cells(2).Text)
            txt_min.Text = Trim(row.Cells(3).Text)
            txt_max.Text = Trim(row.Cells(4).Text)

            MPEDetails.Title = "Org-Level Discount Limits"
            lbl_Info_Key.Text = "Organisation"

            MPEDetails.VisibleOnPageLoad = True
        Catch ex As Exception
            Err_No = "74066"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_CurrencyCode_006") & "&next=AdminCustomerSegment.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub

    Private Sub gvCurrency_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdCustomerSegment.PageIndexChanging
        grdCustomerSegment.PageIndex = e.NewPageIndex
        BindData()

    End Sub

    Private Sub gvCurrency_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdCustomerSegment.Sorting
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindData()
    End Sub
    Private Property SortDirection() As String
        Get
            If ViewState("SortDirection") Is Nothing Then
                ViewState("SortDirection") = "ASC"
            End If
            Return ViewState("SortDirection").ToString()
        End Get
        Set(ByVal value As String)
            Dim s As String = SortDirection

            If value = "flip" Then
                s = If(s = "ASC", "DESC", "ASC")
            Else
                s = value
            End If

            ViewState("SortDirection") = s
        End Set
    End Property

End Class