Imports SalesWorx.BO.Common
Imports System.Data
Imports System.Web
Imports System.Resources
Imports System.Web.UI.WebControls
Imports log4net
Imports System.IO
Imports System.Data.OleDb
Imports System.Data.SqlClient


Partial Public Class BankCodes
    Inherits System.Web.UI.Page
    Dim objReason As New ReasonCode
    Dim objBank As New Bank

    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim Dt As New DataTable
    Private Const PageID As String = "P85"
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

            Dt = objBank.FillBankCode(Err_No, Err_Desc)
            BindBankCodeData()
           
            ViewState("FileType") = Nothing
            ViewState("FileName") = Nothing
            ViewState("CSVName") = Nothing
        Else
            MPEDetails.VisibleOnPageLoad = False
        End If

    End Sub



    Public Sub Resetfields()

        ddl_country.Items.Clear()
        ddl_country.DataSource = objBank.FillCountry(Err_No, Err_Desc)
        ddl_country.DataTextField = "Country"
        ddl_country.DataValueField = "Currency_Code"
        ddl_country.DataBind()

        Me.txtBankCode.Text = ""
        Me.txtDescription.Text = ""

        Me.ddFilterBy.SelectedIndex = 0
        Me.txtBankCode.Enabled = True
        Me.rbAppend.Checked = False
        Me.rbRebuild.Checked = False
        Me.lblPop.Text = ""
        ddFilterBy.Focus()
        ClassUpdatePnl.Update()
    End Sub


    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Me.txtBankCode.Text = "" Or Me.txtDescription.Text = "" Then
            lblPop.Text = "Bank code and description are required."
            MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If
        Dim success As Boolean = False

        Try

            objBank.BankCode = IIf(Me.txtBankCode.Text = "", "0", Me.txtBankCode.Text)
            objBank.Description = IIf(Me.txtDescription.Text = "", "0", Me.txtDescription.Text)
            objBank.Currency = Me.ddl_country.SelectedItem.Value

            If objBank.CheckDuplicate(Err_No, Err_Desc) = False Then
                If objBank.InsertBankCode(Err_No, Err_Desc) = True Then
                    success = True
                    MessageBoxValidation("Successfully saved.", "Information")

                End If
            Else
                Me.lblPop.Text = "Record already exist."
                Me.MPEDetails.VisibleOnPageLoad = True
                Exit Sub
            End If
            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "BANK CODE", IIf(Me.txtBankCode.Text = "", "0", Me.txtBankCode.Text), "Code: " & Me.txtBankCode.Text & "/ Desc :  " & Me.txtDescription.Text & "/ Purpose:  " & Me.ddl_country.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                Dt = objBank.FillBankCode(Err_No, Err_Desc)
                BindBankCodeData()
                Resetfields()
                Me.MPEDetails.VisibleOnPageLoad = False
                ClassUpdatePnl.Update()
            Else

                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_BankCode_001") & "&next=BankCodes.aspx&Title=Message", False)
                Exit Try
            End If

        Catch ex As Exception
            Err_No = "74071"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub



 
    Private Sub BindBankCodeData()
        Me.gvBankCode.DataSource = Dt
        Me.gvBankCode.DataBind()

        Dim dv As New DataView(Dt)
        If ViewState("SortField") <> "" Then
            dv.Sort = (ViewState("SortField") & " ") + SortDirection
        End If
        gvBankCode.DataSource = dv
        gvBankCode.DataBind()
        Session.Remove("BankCode")
        Session("BankCode") = Dt
    End Sub


    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If Me.txtBankCode.Text = "" Or Me.ddl_country.SelectedItem.Text = "--Select--" Or Me.txtDescription.Text = "" Then
            Me.lblPop.Text = "Reason code,description and purpose are required."
            Me.MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If
        Dim success As Boolean = False
        Try

            objBank.BankCode = IIf(Me.txtBankCode.Text = "", "0", Me.txtBankCode.Text)
            objBank.Description = IIf(Me.txtDescription.Text = "", "0", Me.txtDescription.Text)
            objBank.Currency = Me.ddl_country.SelectedItem.Value


            If objBank.UpdateBankCode(Err_No, Err_Desc) = True Then
                success = True
                MessageBoxValidation("Successfully updated.", "Information")
            End If

            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "U", "MAIN MENU", "Bank CODE", IIf(Me.txtBankCode.Text = "", "0", Me.txtBankCode.Text), "Code: " & Me.txtBankCode.Text & "/ Desc :  " & Me.txtDescription.Text & "/ Purpose:  " & Me.ddl_country.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                Dt = objBank.FillBankCode(Err_No, Err_Desc)
                BindBankCodeData()
                Resetfields()
                Me.MPEDetails.VisibleOnPageLoad = False
                ClassUpdatePnl.Update()
            Else

                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_BankCode_002") & "&next=BankCodes.aspx&Title=Message", False)
                Exit Try
            End If

        Catch ex As Exception
            Err_No = "74072"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Try
            Me.MPEDetails.VisibleOnPageLoad = False
            Resetfields()
            'ClassUpdatePnl.Update()

        Catch

        End Try
    End Sub
    Protected Sub btnDeleteAll_Click()
        Try
            Dim dr As GridViewRow
            Dim Success As Boolean = False
            Dim Selected As Boolean = False

            For Each dr In gvBankCode.Rows
                Dim RowCheckBox As System.Web.UI.WebControls.CheckBox = dr.FindControl("chkDelete")

                If RowCheckBox.Checked = True Then
                    Selected = True
                    Dim Lbl As System.Web.UI.WebControls.Label = dr.FindControl("lblReason")
                    HidVal.Value = Lbl.Text
                    objBank.BankCode = dr.Cells(1).Text
                    If objBank.DeleteBankCode(Err_No, Err_Desc) = True Then
                        objLogin.SaveUserLog(Err_No, Err_Desc, "D", "MAIN MENU", "BANK CODE", dr.Cells(1).Text, "Code: " & dr.Cells(1).Text & "/ Desc :  " & dr.Cells(2).Text & "/ Purpose:  " & dr.Cells(3).Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                        Success = True
                    End If
                End If
            Next
            If Selected = False Then
                MessageBoxValidation("Select at least one BANK code .", "Information")
                Exit Sub
            End If

            If (Success = True) Then
                MessageBoxValidation("BANK code(s) deleted successfully.", "Information")
                Dt = objBank.FillBankCode(Err_No, Err_Desc)
                BindBankCodeData()
                Resetfields()
            Else
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_BankCode_003") & "&next=BankCodes.aspx&Title=Message", False)
                Exit Try
            End If
        Catch ex As Exception
            Err_No = "74073"
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
            Dt = objBank.SearchBankCode(Err_No, Err_Desc, Me.ddFilterBy.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text))
            BindBankCodeData()
            ClassUpdatePnl.Update()
            success = True
            If success = False Then
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_BankCode_004") & "&next=BankCodes.aspx&Title=Message", False)
                Exit Try
            End If
        Catch ex As Exception
            Err_No = "74074"
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
        objBank.BankCode = row.Cells(1).Text
        HidVal.Value = row.Cells(1).Text
        Try

            If objBank.DeleteBankCode(Err_No, Err_Desc) = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "D", "MAIN MENU", "BANK CODE", row.Cells(1).Text, "Code: " & row.Cells(1).Text & "- Desc :  " & row.Cells(2).Text & "- Purpose:  " & row.Cells(3).Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                success = True
            End If

            If success = True Then
                MessageBoxValidation("Successfully deleted.", "Information")
                Dt = objBank.FillBankCode(Err_No, Err_Desc)
                BindBankCodeData()
                Resetfields()
            Else
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_BankCode_005") & "&next=BankCodes.aspx&Title=Message", False)
                Exit Try
            End If

        Catch ex As Exception
            Err_No = "74075"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub


    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click

        btnUpdate.Visible = False
        btnSave.Visible = True
        Resetfields()
        ClassUpdatePnl.Update()
        MPEDetails.VisibleOnPageLoad = True
    End Sub



    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            lblPop.Text = ""
            btnUpdate.Visible = True
            btnSave.Visible = False
            Resetfields()
            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
            HidVal.Value = row.Cells(1).Text
            txtBankCode.Text = Trim(row.Cells(1).Text)
            txtDescription.Text = Trim(row.Cells(2).Text)
            If row.Cells(3).Text = "0" Then
                Me.ddl_country.SelectedItem.Text = "--Select--"
            Else
                Me.ddl_country.SelectedValue = row.Cells(3).Text
            End If
            Me.txtBankCode.Enabled = False
            ' ClassUpdatePnl.Update()
            MPEDetails.VisibleOnPageLoad = True
        Catch ex As Exception
            Err_No = "74076"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_BankCode_006") & "&next=BankCodes.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub

    Private Sub gvBankCode_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvBankCode.PageIndexChanging
        gvBankCode.PageIndex = e.NewPageIndex
        Dt = objBank.FillBankCode(Err_No, Err_Desc)
        BindBankCodeData()

    End Sub

    Private Sub gvBankCode_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvBankCode.Sorting
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        Dt = objBank.FillBankCode(Err_No, Err_Desc)
        BindBankCodeData()
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


    Protected Sub ddFilterBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddFilterBy.SelectedIndexChanged
        If Me.ddFilterBy.SelectedIndex <= 0 Then
            Me.txtFilterVal.Text = ""
        End If
    End Sub

 
End Class