Imports SalesWorx.BO.Common
Imports System.Data
Imports System.Web
Imports System.Resources
Imports System.Web.UI.WebControls
Imports log4net
Imports System.Text.RegularExpressions
Partial Public Class AdminClientLogo
    Inherits System.Web.UI.Page

    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim objCommon As New SalesWorx.BO.Common.Common
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim Dt As New DataTable
    Private Const PageID As String = "P304"
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
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Try
            Me.MPECurrency.Hide()
            Resetfields()
        Catch

        End Try
    End Sub

    Public Sub Resetfields()

        Me.txt_Line1.Text = ""
        Me.txt_Line2.Text = ""
        Me.txt_Line3.Text = ""
        ddl_Client.ClearSelection()
        Me.btnSave.Text = "Save"
        'Me.btnAdd.Focus()
        ''Me.lblMessage.Text = ""

    End Sub

  
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If Me.ddl_Client.SelectedItem.Value = "0" Then
            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Please Select a client"
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Me.MPECurrency.Show()
            Exit Sub
        End If

        If Me.txt_Line1.Text.Trim = "" Then
            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Line1 is required."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Me.MPECurrency.Show()
            Exit Sub
        End If

         If Me.txt_Line2.Text.Trim = "" Then
            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Line2 is required."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Me.MPECurrency.Show()
            Exit Sub
        End If
        If Me.txt_Line3.Text.Trim = "" Then
            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Line3 is required."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Me.MPECurrency.Show()
            Exit Sub
        End If
        Dim success As Boolean = False
        Try

           
            If objCommon.SaveClientLogo(Err_No, Err_Desc, ddl_Client.SelectedItem.Value, txt_Line1.Text.Trim, txt_Line2.Text.Trim, txt_Line3.Text.Trim, CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
                    success = True
                    Me.lblMessage.Text = "Successfully saved."
                    lblMessage.ForeColor = Drawing.Color.Green
                    lblinfo.Text = "Information"
                    MpInfoError.Show()
                    btnClose.Focus()
                    ClassUpdatePnl.Update()
            Else
                    success = False
                    Me.lblMessage.Text = "Error while saving"
                    lblMessage.ForeColor = Drawing.Color.Green
                    lblinfo.Text = "Information"
                    MpInfoError.Show()
                    ClassUpdatePnl.Update()
                    btnClose.Focus()
            End If

            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "CLIENT LOGO", HidVal.Value, Me.ddl_Client.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                BindData()
                Resetfields()
                Me.MPECurrency.Hide()

            Else

                If Err_Desc.Trim <> "" Then
                lblMessage.Text = Err_Desc
                   Else
                lblMessage.Text = "Error occured.Please try again"
               End If
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
                BindData()
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
        Dt = objCommon.GetClientLogos(Err_No, Err_Desc, txtFilterVal.Text.Trim)
        Me.grdCustomerSegment.DataSource = Dt
        Me.grdCustomerSegment.DataBind()

        Dim dv As New DataView(Dt)
        If ViewState("SortField") <> "" Then
            dv.Sort = (ViewState("SortField") & " ") + SortDirection
        End If
        grdCustomerSegment.DataSource = dv
        grdCustomerSegment.DataBind()

    End Sub
    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
          If Me.ddl_Client.SelectedItem.Value = "0" Then
            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Please Select a client"
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Me.MPECurrency.Show()
            Exit Sub
        End If

        If Me.txt_Line1.Text.Trim = "" Then
            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Line1 is required."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
Me.MPECurrency.Show()
            Exit Sub
        End If

         If Me.txt_Line2.Text.Trim = "" Then
            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Line2 is required."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
Me.MPECurrency.Show()
            Exit Sub
        End If
        If Me.txt_Line3.Text.Trim = "" Then
            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Line3 is required."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
Me.MPECurrency.Show()
            Exit Sub
        End If
        Dim success As Boolean = False
        Try

            If objCommon.SaveClientLogo(Err_No, Err_Desc, ddl_Client.SelectedItem.Value, txt_Line1.Text.Trim, txt_Line2.Text.Trim, txt_Line3.Text.Trim, CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
                    success = True
                    Me.lblMessage.Text = "Successfully saved."
                    lblMessage.ForeColor = Drawing.Color.Green
                    lblinfo.Text = "Information"
                    MpInfoError.Show()
                    btnClose.Focus()
            Else
                    success = False
                   Me.lblMessage.Text = "Error while saving"
                    lblMessage.ForeColor = Drawing.Color.Green
                    lblinfo.Text = "Information"
                    MpInfoError.Show()
                    btnClose.Focus()
            End If

            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "CLIENT LOGO", HidVal.Value, Me.ddl_Client.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                BindData()
                Resetfields()
                Me.MPECurrency.Hide()

            Else
               If Err_Desc.Trim <> "" Then
                lblMessage.Text = Err_Desc
                   Else
                lblMessage.Text = "Error occured.Please try again"
               End If
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
                BindData()
                Resetfields()
            End If
            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "U", "MAIN MENU", "CLIENT LOGO", HidVal.Value, Me.ddl_Client.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                BindData()
                Resetfields()
                Me.MPECurrency.Hide()

            Else
               If Err_Desc.Trim <> "" Then
                lblMessage.Text = Err_Desc
                   Else
                lblMessage.Text = "Error occured.Please try again"
               End If

                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
                BindData()
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
                    If objCommon.DeleteClientLogo(Err_No, Err_Desc, HidVal.Value) = True Then
                        objLogin.SaveUserLog(Err_No, Err_Desc, "D", "MAIN MENU", "CLIENT LOGO", HidVal.Value, Des, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                        Success = True
                    End If
                End If
            Next
            If (Success = True) Then
                lblMessage.Text = "Client Logo(s) deleted successfully."
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
                BindData()
                Resetfields()
            Else
                lblMessage.Text = "Some Client Logo(s) could not be deleted."
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
                BindData()
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

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Dim success As Boolean = False
        Try
            BindData()
            ClassUpdatePnl.Update()
            success = True

        Catch ex As Exception
            Err_No = "74064"
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
        HidVal.Value = Lbl.Text
        
        Try

            If objCommon.DeleteClientLogo(Err_No, Err_Desc, HidVal.Value) = True Then
                        success = True
            End If
            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "D", "MAIN MENU", "CLIENT LOGO", HidVal.Value, row.Cells(1).Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                lblMessage.Text = "Successfully deleted."
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
                BindData()
                Resetfields()
            Else
                If Err_Desc.Trim <> "" Then
                lblMessage.Text = Err_Desc
                   Else
                lblMessage.Text = "Error occured.Please try again"
               End If

                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
                BindData()
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
        bindClient()
        btnUpdate.Visible = False
        btnSave.Visible = True
        ddl_Client.Enabled = True
        Resetfields()
        ClassUpdatePnl.Update()
        Me.MPECurrency.Show()
    End Sub
Sub bindClient()
        ddl_Client.DataSource = objCommon.GetClientsWthNoLogos(Err_No, Err_Desc)
        ddl_Client.DataTextField = "Location"
        ddl_Client.DataValueField = "Location"
        ddl_Client.DataBind()
        ddl_Client.Items.Insert(0, "(Select)")
        ddl_Client.Items(0).Value = "0"
        ClassUpdatePnl.Update()
End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            btnUpdate.Visible = True
            btnSave.Visible = False
            Resetfields()
            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
            Dim Lbl As System.Web.UI.WebControls.Label = row.FindControl("lblRow_ID")
            HidVal.Value = Lbl.Text
            ddl_Client.Items.Add(New ListItem(row.Cells(1).Text, row.Cells(1).Text))
            If Not ddl_Client.Items.FindByValue(row.Cells(1).Text) Is Nothing Then
                ddl_Client.Items.FindByValue(row.Cells(1).Text).Selected = True
            End If
            ddl_Client.Enabled = False
            txt_Line1.Text = Trim(row.Cells(2).Text)
            txt_Line2.Text = Trim(row.Cells(3).Text)
            txt_Line3.Text = Trim(row.Cells(4).Text)
            ' ClassUpdatePnl.Update()
            MPECurrency.Show()
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

    Protected Sub btnclearFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnclearFilter.Click
        txtFilterVal.Text = ""
        BindData()
         ClassUpdatePnl.Update()
    End Sub

End Class