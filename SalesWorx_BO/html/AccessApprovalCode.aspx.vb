Imports SalesWorx.BO.Common
Imports System.Resources
Imports log4net
Imports Telerik.Web.UI
Partial Public Class AccessApprovalCode
    Inherits System.Web.UI.Page

    Private Const PageID As String = "P256"
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim ErrorResource As ResourceManager
    Dim Err_No As Long
    Dim Err_Desc As String

    Private Sub DefaultPlan_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Access Approval Code"
    End Sub
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If

        If (Not IsPostBack) Then
            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            If Not HasPermission Then
                Response.Redirect("information.aspx?mode=1&errno=7890&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If
            Dim objMessage As Message = Nothing
            Try
                objMessage = New Message()
                Dim UD_SUB_QRY = "select SalesRep_ID from dbo.app_GetControlInfo(" & CType(Session("User_Access"), UserAccess).UserID & ")"
                SalesRep_ID.DataSource = objMessage.GetSalesRepList(Err_No, Err_Desc, UD_SUB_QRY)
                SalesRep_ID.DataBind()
                SalesRep_ID.Items.Insert(0, New RadComboBoxItem("-- Select a Van/FSR --"))
            Catch ex As Exception
                log.Error(ex.Message)
            Finally
            End Try
        End If
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Private Sub BtnAccess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAccess.Click
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If

        Dim ObjAppCode As New ApprovalCode

        Try
            rowCode.Visible = False
            If SalesRep_ID.SelectedIndex = 0 Then
                MessageBoxValidation("Please select a van/FSR", "Validation")
                Exit Sub
            End If

            ObjAppCode.FSR = SalesRep_ID.SelectedItem.Value
            ObjAppCode.UserID = CType(Session("User_Access"), UserAccess).UserID

            If ObjAppCode.GetAppCode(Err_No, Err_Desc) Then
                If String.IsNullOrEmpty(ObjAppCode.ApprovalCode) Then
                    MessageBoxValidation("Approval Code is not available for selected van/FSR", "Information")
                    Exit Sub
                End If
                rowCode.Visible = True
                lblApprovalCode.Text = ObjAppCode.ApprovalCode
            End If

        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            ObjAppCode = Nothing
        End Try
    End Sub
End Class