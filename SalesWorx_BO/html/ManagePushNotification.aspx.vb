

Imports SalesWorx.BO.Common
Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports Telerik.Web.UI
Imports Telerik
Imports SalesWorx_BO.FilterCustomEditors
Imports System.Configuration.ConfigurationManager
Public Class ManagePushNotification
    Inherits System.Web.UI.Page

    Dim objControl As New SalesWorx.BO.Common.AppControl
    Dim Err_No As Long
    Dim Err_Desc As String
    Private bAdded As Boolean
    Private Const PageID As String = "P375"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim _strReceipient As String = AppSettings("PUSHRECIPIENT")
    Dim _strReceipientDevice As String = AppSettings("PUSHRECEIPIENTDEVICE")
    Private Sub ManagePushNotification_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Send Push Notification"
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

                LoadRecipientTypes()
                FillEmployee()
                ResetFields()

            End If
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_005") & "&next=Welcome.aspx&Title=Message", False)
        End Try


    End Sub


    Private Sub LoadRecipientTypes()

        ddlReceType.Items.Clear()
        ddlReceType.ClearSelection()
        ddlReceType.Text = ""

        ddlReceType.DataSource = objControl.LoadRecipientType(Err_No, Err_Desc)
        ddlReceType.DataValueField = "Code"
        ddlReceType.DataTextField = "Description"
        ddlReceType.DataBind()
        ddlReceType.SelectedIndex = 0
    End Sub




    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub




    Private Sub ResetFields()

        Me.txt_Msg.Text = ""
        Me.txtTitle.Text = ""
        Me.txt_Left.Text = ""
        Me.btnSave.Text = "Save"
        Me.ddlReceType.ClearSelection()
        Me.ddlReceType.Text = ""

        txt_Msg.Text = ""
        txt_Left.Text = ""

        '  lbl_length.Text = "* Max length 195 "
        htotal.Value = 195
        If Me.ddlReceType.SelectedValue = "USER" Then
            For Each itm As RadComboBoxItem In Me.ddlFSR.Items
                itm.Checked = False
            Next
            sendto.Visible = True

        Else
            For Each itm As RadComboBoxItem In Me.ddlFSR.Items
                itm.Checked = False
            Next
            sendto.Visible = False

        End If
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        'Dim p As New PushNotifcation
        'p.SendNotifications()

        ResetFields()
    End Sub

    Protected Sub ddlReceType_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlReceType.SelectedIndexChanged

        If Me.ddlReceType.SelectedValue = "USER" Then
            For Each itm As RadComboBoxItem In Me.ddlFSR.Items
                itm.Checked = False
            Next
            sendto.Visible = True

        Else
            For Each itm As RadComboBoxItem In Me.ddlFSR.Items
                itm.Checked = False
            Next
            sendto.Visible = False

        End If
    End Sub


    Function getLength() As Integer
        Dim utf As Byte()
        Dim utf1 As Byte()
        utf = System.Text.Encoding.UTF8.GetBytes(txt_Msg.Text)
        utf1 = System.Text.Encoding.UTF8.GetBytes(txtTitle.Text)
        Return utf.Length + utf1.Length
    End Function
    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If Me.ddlReceType.SelectedIndex < 0 Or Me.txtTitle.Text = "" Or Me.txtTitle.Text = "0" Or Me.txt_Msg.Text = "" Or Me.txt_Msg.Text = "0" Then
            MessageBoxValidation("Please enter the data for all the mandatory fields", "validation")
            Exit Sub
        End If
        If getLength() > 195 Then
            MessageBoxValidation("The combined length of message and title should not exceed 195 characters", "Validation")
            Exit Sub
        End If
        Dim UsersList As String = Nothing

        For Each itm As RadComboBoxItem In ddlFSR.Items
            If itm.Checked = True Then
                UsersList = UsersList + "," + _strReceipient & "_" & itm.Value
            End If
        Next
        If Me.ddlReceType.SelectedValue = "USER" Then
            If UsersList Is Nothing Then
                MessageBoxValidation("Please select a users from the list", "validation")
                Exit Sub
            End If
        End If
        If Me.ddlReceType.SelectedValue = "OS" Then
            UsersList = Nothing
            UsersList = _strReceipient & "_" & _strReceipientDevice
        End If

        If objControl.SavePushNotification(Err_No, Err_Desc, Me.txtTitle.Text.Trim, Me.txt_Msg.Text.Trim, Me.ddlReceType.SelectedValue, UsersList, CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
            MessageBoxValidation("Successfully saved.", "Information")
        Else
            MessageBoxValidation("Error occured while saving.", "Validation")
            Exit Sub
        End If
        ResetFields()
    End Sub
    Sub FillEmployee()

        Me.ddlFSR.ClearSelection()
        Me.ddlFSR.Items.Clear()
        Me.ddlFSR.DataTextField = "UserName"
        Me.ddlFSR.DataValueField = "UserID"
        Me.ddlFSR.DataSource = objControl.LoadRecipients(Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID)
        Me.ddlFSR.DataBind()
        For Each itm As RadComboBoxItem In Me.ddlFSR.Items
            itm.Checked = False
        Next
    End Sub

End Class