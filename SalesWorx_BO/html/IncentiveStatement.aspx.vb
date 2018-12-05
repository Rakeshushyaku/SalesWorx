Imports log4net
Imports SalesWorx.BO.Common
Imports System.IO
Imports Telerik.Web.UI
Public Class IncentiveStatement
    Inherits System.Web.UI.Page

    Private Const ModuleName As String = "IncentiveStatement.aspx"
    Private Const PageID As String = "P154"
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim objUser As New User
    Dim objCrypt As New Crypto
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objCommon As New Common
    Dim objIncentive As New Incentive
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Not Session.Item("USER_ACCESS") Is Nothing Then

                If Not HasAuthentication() Then
                    Err_No = 500
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
                End If

                Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = (New SalesWorx.BO.Common.Product).GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.Items.Clear()
                ddlOrganization.Items.Add(New RadComboBoxItem("Select Organization", "0"))
                ddlOrganization.AppendDataBoundItems = True
                ddlOrganization.DataValueField = "MAS_Org_ID"
                ddlOrganization.DataTextField = "Description"
                ddlOrganization.DataBind()


                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                End If
                LoadOrgDetails()
            Else
                Session.Add("BringmeBackHere", ModuleName)
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
        Else

        End If

    End Sub

    Private Function HasAuthentication() As Boolean
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
    End Function

    Private Sub ddlOrganization_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlOrganization.SelectedIndexChanged
        If Not (ddlOrganization.SelectedItem.Value = "0") Then
            LoadOrgDetails()
        End If
    End Sub
    Sub LoadOrgDetails()
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        objCommon = New SalesWorx.BO.Common.Common()

        ddlEmployee.Items.Clear()
        ddlEmployee.DataSource = objCommon.GetEmployee(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())

        ddlEmployee.DataTextField = "Emp_Name"
        ddlEmployee.DataValueField = "Emp_Code"
        ddlEmployee.DataBind()

        ddlEmployee.Items.Insert(0, New RadComboBoxItem("Select", "0"))

        Dim dtcurrency As DataTable
        Dim ObjReport As New SalesWorx.BO.Common.Reports
        dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

        Dim Currency As String = ""
        If dtcurrency.Rows.Count > 0 Then
            Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
            lblDecimal.Text = "N" & dtcurrency.Rows(0)("Decimal_Digits")
            lbl_Currency.Text = Currency

        End If

    End Sub

    Private Sub SearchBtn_Click(sender As Object, e As EventArgs) Handles SearchBtn.Click
        If ddlOrganization.SelectedIndex <= 0 Then
            MessageBoxValidation("Please select the organization", "Validation")
            Exit Sub
        Else
            divCurrency.Visible = True
            bindgrid()
        End If
    End Sub
    Sub bindgrid()
        Dim dt As New DataTable
        Dim empcode As String = "0"
        If ddlEmployee.SelectedIndex > 0 Then
            empcode = ddlEmployee.SelectedItem.Value
        End If
        dt = (New SalesWorx.BO.Common.Incentive).GetIncentiveStatement(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, empcode)
        gvRep.DataSource = dt
        gvRep.DataBind()
        gvRep.Visible = True
        If dt.Rows.Count > 0 Then
            Btn_pay.Visible = True
        Else
            Btn_pay.Visible = False
        End If
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        LoadOrgDetails()
        ddlEmployee.ClearSelection()
        
        gvRep.Visible = False
    End Sub

    Private Sub Btn_pay_Click(sender As Object, e As EventArgs) Handles Btn_pay.Click
        Try
            Dim bretval As Boolean = True
            Dim bFinalretval As Boolean = True
            Dim BTobeSaved As Boolean = False
            If ddlOrganization.SelectedIndex > 0 Then
                If gvRep.Items.Count > 0 Then
                    Dim ObjInc As New SalesWorx.BO.Common.Incentive
                    If validateInput() = True Then
                        For Each gr As GridDataItem In gvRep.Items
                            If Val(CType(gr.FindControl("Txt_Amount"), TextBox).Text.Trim) <> 0 Then

                                BTobeSaved = True
                                bretval = ObjInc.PayIncentive(Err_No, Err_Desc, CType(gr.FindControl("HEmpCode"), HiddenField).Value, ddlOrganization.SelectedItem.Value, CType(gr.FindControl("Txt_Amount"), TextBox).Text.Trim)
                                If bFinalretval = True Then
                                    bFinalretval = bretval

                                End If
                            End If
                        Next
                        If BTobeSaved=True 
                            If bFinalretval = True Then
                                MessageBoxValidation("Incentive payments saved successfully", "Information")
                                bindgrid()
                            Else
                                MessageBoxValidation("Unexpected error occured ", "Information")
                                bindgrid()
                            End If
                        Else
                            MessageBoxValidation("Please enter incentive amount", "Information")
                            bindgrid()
                        End If
                    End If

                End If
            Else
                MessageBoxValidation("Please select the organization", "Validation")
                Exit Sub
            End If

        Catch ex As Exception

        End Try
        
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
    Function validateInput() As Boolean
        Dim bRetval As Boolean = True
        For Each gr As GridDataItem In gvRep.Items
            If Not gr.FindControl("Txt_Amount") Is Nothing Then
                If CType(gr.FindControl("Txt_Amount"), TextBox).Text.Trim <> "" Then
                    If Not IsNumeric(CType(gr.FindControl("Txt_Amount"), TextBox).Text) Then
                        MessageBoxValidation("Please enter valid amount for " & CType(gr.FindControl("HEmpCode"), HiddenField).Value, "Validation")
                        bRetval = False
                        Exit For
                    ElseIf Val(CType(gr.FindControl("Txt_Amount"), TextBox).Text) > Val(CType(gr.FindControl("HAmt"), HiddenField).Value) Then
                        MessageBoxValidation("The amount entered is greater than the outstanding amount for " & CType(gr.FindControl("HEmpCode"), HiddenField).Value, "Validation")
                        bRetval = False
                        Exit For
                    End If
                End If
            End If
        Next
        Return bRetval
    End Function

    Private Sub gvRep_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        bindgrid()
    End Sub

    Private Sub gvRep_SortCommand(sender As Object, e As GridSortCommandEventArgs) Handles gvRep.SortCommand
        bindgrid()
    End Sub
End Class