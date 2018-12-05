

Imports SalesWorx.BO.Common
Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports Telerik.Web.UI
Imports Telerik

Public Class ManageAppCodes
    Inherits System.Web.UI.Page
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim objControl As New SalesWorx.BO.Common.AppControl
    Dim Err_No As Long
    Dim Err_Desc As String
    Private bAdded As Boolean
    Private Const PageID As String = "P332"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objUser As New User
    Private Sub ManageAppCodes_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Manage App Codes"
    End Sub

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


                LoadCodeTypes()

                rgv.Rebind()

            End If
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_005") & "&next=Welcome.aspx&Title=Message", False)
        End Try



    End Sub

    Private Sub LoadCodeTypes()

        ddlCodeType.Items.Clear()
        ddlCodeType.ClearSelection()
        ddlCodeType.Text = ""

        ddlCodeType.DataSource = objControl.LoadAppCodeTypes(Err_No, Err_Desc)
        ddlCodeType.DataValueField = "Code"
        ddlCodeType.DataTextField = "Description"
        ddlCodeType.DataBind()

    End Sub



    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub



    Protected Sub ddlCodeType_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlCodeType.SelectedIndexChanged

        rgv.Rebind()

    End Sub
    Protected Sub btnDeleteAll_Click()
        Try
            Dim dr As GridDataItem
            Dim Success As Boolean = False
            Dim Selected As Boolean = False
            If Me.ddlCodeType.SelectedValue = "KPI_PARAMETERS" Then
                MessageBoxValidation("System will not allow to delete the KPI parameters", "Validation")
                Exit Sub
            End If


            For Each dr In rgv.Items
                Dim RowCheckBox As System.Web.UI.WebControls.CheckBox = dr.FindControl("chkDelete")

                If RowCheckBox.Checked = True Then
                    Selected = True
                    Dim Lbl As System.Web.UI.WebControls.Label = dr.FindControl("lblReason")

                    Dim Type As String
                    Type = CType(dr.Cells(2).FindControl("hCodeType"), HiddenField).Value
                    Dim Code As String
                    Code = CType(dr.Cells(3).FindControl("hCodevalue"), HiddenField).Value
                    If objControl.DeleteAppcode(Err_No, Err_Desc, Code, Type) = True Then
                        objLogin.SaveUserLog(Err_No, Err_Desc, "D", "MAIN MENU", "APP CODE", Code, "Type: " & Type & "/ Code :  " & Code, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                        Success = True
                    End If
                End If
            Next
            If Selected = False Then
                MessageBoxValidation("Select at least one code .", "Information")
                Exit Sub
            End If

            If (Success = True) Then
                MessageBoxValidation("App code(s) deleted successfully.", "Information")
                rgv.Rebind()
            Else
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_ReasonCode_003") & "&next=ReasonCodes.aspx&Title=Message", False)
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
    
    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If Me.ddlCodeType.SelectedIndex <= 0 Or Me.txtAppCode.Text = "" Or Me.txtAppCode.Text = "0" Or Me.txtDescription.Text = "0" Or Me.txtDescription.Text = "" Then
            MessageBoxValidation("Please enter all mandatory fields", "Validation")
            Exit Sub
        End If

        If btnSave.Text = "Save" Then
            If objControl.AppCodeExists(Me.ddlCodeType.SelectedValue, Me.txtAppCode.Text) = True Then
                MessageBoxValidation("Same code already exist for the selected code type", "Validation")
                Exit Sub
            End If
        End If
        If objControl.SaveAppCodes(Err_No, Err_Desc, Me.ddlCodeType.SelectedValue, Me.txtAppCode.Text, Me.txtDescription.Text, CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
            MessageBoxValidation("Successfully saved.", "Information")
            ResetFields()
            rgv.Rebind()
        Else
            MessageBoxValidation("Error while saving", "Validation")
            Exit Sub
        End If

    End Sub
    Protected Sub rgv_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles rgv.ItemCommand

        Try
            If (e.CommandName = "EditCode") Then
                Dim row As GridDataItem = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridDataItem)

                Me.ddlCodeType.SelectedValue = CType(row.Cells(2).FindControl("hCodeType"), HiddenField).Value
                Me.txtAppCode.Text = CType(row.Cells(3).FindControl("hCodevalue"), HiddenField).Value
                Me.txtDescription.Text = row.Cells(6).Text
                Me.txtAppCode.Enabled = False
                Me.ddlCodeType.Enabled = False
                Me.btnSave.Text = "Update"

            End If
            If (e.CommandName = "DeleteCode") Then
                Dim dr As GridDataItem = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridDataItem)
                Dim success As Boolean = False
                Dim Type As String
                Type = CType(dr.Cells(2).FindControl("hCodeType"), HiddenField).Value

                Dim Code As String
                Code = CType(dr.Cells(3).FindControl("hCodevalue"), HiddenField).Value

                If Me.ddlCodeType.SelectedValue = "KPI_PARAMETERS" Then
                    MessageBoxValidation("System will not allow to delete the KPI parameters", "Validation")
                    Exit Sub
                End If


                If objControl.DeleteAppcode(Err_No, Err_Desc, Code, Type) = True Then
                    objLogin.SaveUserLog(Err_No, Err_Desc, "D", "MAIN MENU", "APP CODE", Code, "Type: " & Type & "- Code :  " & Code, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                    success = True
                End If

                If success = True Then
                    MessageBoxValidation("Successfully deleted.", "Information")
                    rgv.Rebind()
                Else
                    log.Error(Err_Desc)
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_ReasonCode_005") & "&next=ReasonCodes.aspx&Title=Message", False)
                    Exit Try
                End If

            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("information.aspx?mode=1&errno=" & "74071" & "&msg=" & AppMsgHandler.GetErrorMessage("Swx_BO_Msg_005") & "&next=Welcome.aspx", False)
        Finally
        End Try
    End Sub
    Private Sub ResetFields()
        Me.txtDescription.Text = ""
        Me.txtAppCode.Text = ""
        Me.btnSave.Text = "Save"
        Me.txtAppCode.Enabled = True
        Me.ddlCodeType.Enabled = True
        rgv.Rebind()
    End Sub
    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        ResetFields()
    End Sub

End Class