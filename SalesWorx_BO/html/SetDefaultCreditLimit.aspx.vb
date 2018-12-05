Imports SalesWorx.BO.Common
Imports System.Data
Imports System.Web
Imports System.Resources
Imports System.Web.UI.WebControls
Imports log4net
Imports System.Text.RegularExpressions
Imports Telerik.Web.UI
Public Class SetDefaultCreditLimit
    Inherits System.Web.UI.Page
    Dim objReason As New ReasonCode
    Dim objBank As New Bank

    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim Dt As New DataTable
    Private Const PageID As String = "P84"
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
            Dim ObjCommon As SalesWorx.BO.Common.Common
            ObjCommon = New SalesWorx.BO.Common.Common
            txt_CreditLimit.Text = ObjCommon.GetAppConfig(Err_No, Err_Desc, "DEFUALT_CREDIT_LIMIT_CASH_CUST").ToUpper()
        End If
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Try
            If txt_CreditLimit.Text.Trim = "" Then
                MessageBoxValidation("Please enter the credit limit", "Validation")
                Exit Sub
            End If
            If IsNumeric(txt_CreditLimit.Text) = False Then
                MessageBoxValidation("Please enter a valid credit limit", "Validation")
                Exit Sub
            End If

            Dim ObjCommon As SalesWorx.BO.Common.Common
            ObjCommon = New SalesWorx.BO.Common.Common
            ObjCommon.SaveDefaultCL(Err_No, Err_Desc, txt_CreditLimit.Text)
        Catch ex As Exception
            log.Error(ex.ToString)
        End Try
     
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
End Class