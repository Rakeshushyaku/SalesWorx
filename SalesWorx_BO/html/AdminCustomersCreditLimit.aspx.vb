Imports log4net
Imports SalesWorx.BO.Common

Public Class AdminCustomersCreditLimit
    Inherits System.Web.UI.Page
    Dim objcommon As New SalesWorx.BO.Common.Common
    Dim Customer_ID As String
    Dim SiteUse_ID As String
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        If Not Page.IsPostBack Then
            txt_CreditLimit.Attributes.Add("onBlur", "javascript:RecalAvail()")
            If Not Request.QueryString("Customer_ID") Is Nothing And Not Request.QueryString("Site_Use_ID") Is Nothing Then

                Customer_ID = Request.QueryString("Customer_ID")
                SiteUse_ID = Request.QueryString("Site_Use_ID")
                LoadCustomerDetails()

            Else

                ' BtnAddShip.Visible = False
            End If
        End If
    End Sub
    Sub LoadCustomerDetails()
        Try


            Dim dtCust As New DataTable
            dtCust = (New SalesWorx.BO.Common.Customer).GetCustomerDeatils(Err_No, Err_Desc, Customer_ID, SiteUse_ID)
            If dtCust.Rows.Count > 0 Then
                Dim dr As DataRow
                dr = dtCust.Rows(0)


                Txt_CustNo.Text = dr("Customer_No").ToString
                Txt_CustName.Text = dr("Customer_Name").ToString

                txt_CCreditLimit.Text = dr("Credit_Limit").ToString
                txt_CAvailBal.Text = dr("Avail_Bal").ToString

                Txt_CustNo.Enabled = False
                Txt_CustName.Enabled = False
                txt_CCreditLimit.Enabled = False
                txt_CAvailBal.Enabled = False
            End If

        Catch ex As Exception


            MessageBoxValidation("Error occured while Loading CustomerDetails.", "Validation")
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        Try

            If (Page.IsValid) Then
                If IsNumeric(txt_CreditLimit.Text) = False Then
                    btnSave.Enabled = True
                    MessageBoxValidation("Please enter a valid credit limit", "Information")
                    Exit Sub
                End If
                If IsNumeric(txt_AvailBal.Text) = False Then
                    btnSave.Enabled = True
                    MessageBoxValidation("Please enter a valid available balance", "Information")
                    Exit Sub
                End If

                Customer_ID = Request.QueryString("Customer_ID")
                SiteUse_ID = Request.QueryString("Site_Use_ID")

                If Val(txt_CreditLimit.Text) < Val(txt_CCreditLimit.Text) Then
                    Dim dtinvoices As New DataTable
                    dtinvoices = (New SalesWorx.BO.Common.Customer).GetCustomerOpenInvoices(Err_No, Err_Desc, Customer_ID, SiteUse_ID)
                    If dtinvoices.Rows.Count > 0 Then
                        If Val(dtinvoices.Rows(0)("PendingAmt").ToString()) <> 0 Then
                            btnSave.Enabled = True
                            MessageBoxValidation("Error !!! Customer have open invoices , cant set credit limit", "Information")
                            Exit Sub
                        End If
                    End If
                End If
                Dim rs As Boolean
                rs = (New SalesWorx.BO.Common.Customer).SaveCustomerCreditLimit(Err_No, Err_Desc, "0", Customer_ID, SiteUse_ID, txt_CreditLimit.Text, txt_AvailBal.Text, txt_CCreditLimit.Text, txt_CAvailBal.Text, txt_comment.Text, CType(Session("User_Access"), UserAccess).UserID)
                If rs = True Then

                    MessageBoxValidation("Customer Credit Limit saved successfully", "Information")
                    btnSave.Enabled = False
                Else
                    MessageBoxValidation("Customer Credit Limit saving error !!!", "Information")
                End If

            End If


        Catch ex As Exception
            MessageBoxValidation("Error occured while Saving Customer Credit Limit.", "Validation")
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Try

            Response.Redirect("ListCustomers.aspx")

        Catch ex As Exception
            MessageBoxValidation("Error occured while Cancel Customer Credit Limit.", "Validation")
            log.Error(ex.Message.ToString())
        End Try
    End Sub


End Class