Imports SalesWorx.BO.Common
Imports System.Data
Imports System.Web
Imports System.Resources
Imports System.Web.UI.WebControls
Imports log4net
Imports System.Text.RegularExpressions

Partial Public Class CurrencyCode
    Inherits System.Web.UI.Page
    Dim objCurrency As New Currency
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim Dt As New DataTable
    Private Const PageID As String = "P80"
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
            Dt = objCurrency.FillCurrency(Err_No, Err_Desc)
            BindCurrencyData()
            LoadCountry()
            Resetfields()
        Else
            MPEDetails.VisibleOnPageLoad = False
        End If

    End Sub
    Sub LoadCountry()
        ddl_Country.DataSource = objCurrency.FillCountry(Err_No, Err_Desc)
        ddl_Country.DataTextField = "Code_Value"
        ddl_Country.DataValueField = "Code_Value"
        ddl_Country.DataBind()
    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Try
            Resetfields()
            Me.MPEDetails.VisibleOnPageLoad = False
          
            'ClassUpdatePnl.Update()

        Catch

        End Try
    End Sub

    Public Sub Resetfields()
        Me.txtCurrencyCode.Text = ""
        Me.txtDescription.Text = ""
        Me.txtRate.Text = ""
        Me.ddlDigits.SelectedIndex = 0
        Me.ddFilterBy.SelectedIndex = 0
        Me.btnSave.Text = "Save"
        ddl_Country.ClearSelection()
        Me.txtCurrencyCode.Enabled = True
        'Me.btnAdd.Focus()
        ''Me.lblMessage.Text = ""

    End Sub

    'Protected Sub CheckRequiredFields()
    '    If Me.txtCurrencyCode.Text = "" Or Me.txtDescription.Text = "" Or Me.txtRate.Text = "" Then
    '        Me.lblinfo.Text = "Validation"
    '        Me.lblMessage.Text = "Currency code,description and conversion rate are required."
    '        Me.lblMessage.ForeColor = Drawing.Color.Red
    '        Me.MpInfoError.Show()
    '        Me.MPECurrency.Show()
    '        Exit Sub
    '    End If

    '    If IsAlpha(Me.txtCurrencyCode.Text) = False Then
    '        Me.lblinfo.Text = "Validation"
    '        Me.lblMessage.Text = "Currency code should be in characters."
    '        Me.lblMessage.ForeColor = Drawing.Color.Red
    '        Me.MpInfoError.Show()
    '        Me.MPECurrency.Show()
    '        Exit Sub
    '    End If


    '    If IsNumeric(Me.txtRate.Text) = False Then
    '        Me.lblinfo.Text = "Validation"
    '        Me.lblMessage.Text = "Conversion rate should be in number."
    '        Me.lblMessage.ForeColor = Drawing.Color.Red
    '        Me.MpInfoError.Show()
    '        Me.MPECurrency.Show()
    '        Exit Sub
    '    End If
    'End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If Me.txtCurrencyCode.Text = "" Or Me.txtDescription.Text = "" Or Me.txtRate.Text = "" Then
            Me.lblPop.Text = "Currency code,description and conversion rate are required."
            Me.MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If

        If IsAlpha(Me.txtCurrencyCode.Text) = False Then
            Me.lblPop.Text = "Currency code should be in characters."
            Me.MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If


        If IsNumeric(Me.txtRate.Text) = False Then
            Me.lblPop.Text = "Conversion rate should be in number."
            Me.MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If
        Dim success As Boolean = False
        Try

            objCurrency.CurrencyCode = IIf(Me.txtCurrencyCode.Text = "", "0", Me.txtCurrencyCode.Text.ToUpper())
            objCurrency.CurrencyName = IIf(Me.txtDescription.Text = "", "0", Me.txtDescription.Text)
            objCurrency.ConversionRate = Double.Parse(IIf(Me.txtRate.Text = "", "0", Me.txtRate.Text))
            objCurrency.DecimalDigits = Integer.Parse(IIf(Me.ddlDigits.SelectedIndex <= 0, "0", Me.ddlDigits.Text))
            objCurrency.Country = ddl_Country.SelectedItem.Value
            If objCurrency.CheckDuplicate(Err_No, Err_Desc) = False Then
                If objCurrency.InsertCurrency(Err_No, Err_Desc) = True Then
                    success = True
                   MessageBoxValidation("Successfully saved", "Information")
                End If
            Else
                Me.lblPop.Text = "Record already exist."
                Me.MPEDetails.VisibleOnPageLoad = True

                Exit Sub
            End If
            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "CURRENCY", Me.txtCurrencyCode.Text, "Code: " & Me.txtCurrencyCode.Text & "/ Desc :  " & Me.txtDescription.Text & "/ Rate:  " & Me.txtRate.Text & "/ Decimal : " & ddlDigits.SelectedValue.ToString(), CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                Dt = objCurrency.FillCurrency(Err_No, Err_Desc)
                BindCurrencyData()
                Resetfields()
                Me.MPEDetails.VisibleOnPageLoad = False
                ClassUpdatePnl.Update()
            Else

                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_CurrencyCode_001") & "&next=CurrencyCode.aspx&Title=Message", False)
                Exit Try
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
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Private Sub BindCurrencyData()
        Me.gvCurrency.DataSource = Dt
        Me.gvCurrency.DataBind()

        Dim dv As New DataView(Dt)
        If ViewState("SortField") <> "" Then
            dv.Sort = (ViewState("SortField") & " ") + SortDirection
        End If
        gvCurrency.DataSource = dv
        gvCurrency.DataBind()
        Session.Remove("CurrencyCode")
        Session("CurrencyCode") = Dt
    End Sub

    Public Function IsNumeric(ByVal inputString As String) As Boolean
        Dim _isNumber As System.Text.RegularExpressions.Regex = New  _
Regex("(^[-+]?\d+(,?\d*)*\.?\d*([Ee][-+]\d*)?$)|(^[-+]?\d?(,?\d*)*\.\d+([Ee][-+]\d*)?$)")
        Return _isNumber.Match(inputString).Success
    End Function

    Public Function IsAlpha(ByVal strToCheck As String) As Boolean
        Dim objAlphaPattern As Regex = New Regex("[^a-zA-Z]")
        Return Not objAlphaPattern.IsMatch(strToCheck)
    End Function


    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If Me.txtCurrencyCode.Text = "" Or Me.txtDescription.Text = "" Or Me.txtRate.Text = "" Then
            Me.lblPop.Text = "Currency code,description and conversion rate are required."
            Me.MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If

        If IsAlpha(Me.txtCurrencyCode.Text) = False Then
            Me.lblPop.Text = "Currency code should be in characters."
            Me.MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If


        If IsNumeric(Me.txtRate.Text) = False Then
            Me.lblPop.Text = "Conversion rate should be in number."

            Me.MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If
        Dim success As Boolean = False
        Try

            objCurrency.CurrencyCode = IIf(Me.txtCurrencyCode.Text = "", "0", Me.txtCurrencyCode.Text.ToUpper())
            objCurrency.CurrencyName = IIf(Me.txtDescription.Text = "", "0", Me.txtDescription.Text)
            objCurrency.ConversionRate = Double.Parse(IIf(Me.txtRate.Text = "", "0", Me.txtRate.Text))
            objCurrency.DecimalDigits = Integer.Parse(IIf(Me.ddlDigits.SelectedIndex <= 0, "0", Me.ddlDigits.Text))
            objCurrency.Country = ddl_Country.SelectedItem.Value
            If objCurrency.UpdateCurrency(Err_No, Err_Desc) = True Then
                success = True
                MessageBoxValidation("Successfully updated.", "Information")
            End If

            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "U", "MAIN MENU", "CURRENCY", Me.txtCurrencyCode.Text, "Code: " & Me.txtCurrencyCode.Text & "/ Desc :  " & Me.txtDescription.Text & "/ Rate:  " & Me.txtRate.Text & "/ Decimal : " & ddlDigits.SelectedValue.ToString(), CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                Dt = objCurrency.FillCurrency(Err_No, Err_Desc)
                BindCurrencyData()
                Resetfields()
                Me.MPEDetails.VisibleOnPageLoad = False
                ClassUpdatePnl.Update()
            Else

                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_CurrencyCode_002") & "&next=CurrencyCode.aspx&Title=Message", False)
                Exit Try
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
            For Each dr In gvCurrency.Rows
                Dim RowCheckBox As System.Web.UI.WebControls.CheckBox = dr.FindControl("chkDelete")

                If RowCheckBox.Checked = True Then
                    Dim Lbl As System.Web.UI.WebControls.Label = dr.FindControl("lblCurrency")
                    HidVal.Value = Lbl.Text
                    objCurrency.CurrencyCode = Server.HtmlDecode(dr.Cells(1).Text)
                    If objCurrency.DeleteCurrency(Err_No, Err_Desc) = True Then
                        objLogin.SaveUserLog(Err_No, Err_Desc, "D", "MAIN MENU", "CURRENCY", dr.Cells(1).Text, "Code: " & dr.Cells(1).Text & "/ Desc :  " & dr.Cells(2).Text & "/ Rate:  " & dr.Cells(3).Text & "/ Decimal : " & dr.Cells(4).Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                        Success = True
                    End If
                End If
            Next
            If (Success = True) Then
                MessageBoxValidation("Currency code(s) deleted successfully.", "Information")
                Dt = objCurrency.FillCurrency(Err_No, Err_Desc)
                BindCurrencyData()
                Resetfields()
            Else
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_CurrencyCode_003") & "&next=CurrencyCode.aspx&Title=Message", False)
                Exit Try
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
            Dt = objCurrency.SearchCurrency(Err_No, Err_Desc, Me.ddFilterBy.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text))
            BindCurrencyData()
            ClassUpdatePnl.Update()
            success = True
            If success = False Then
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_CurrencyCode_004") & "&next=CurrencyCode.aspx&Title=Message", False)
                Exit Try
            End If
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
        objCurrency.CurrencyCode = row.Cells(1).Text
        HidVal.Value = row.Cells(1).Text
        Try

            If objCurrency.DeleteCurrency(Err_No, Err_Desc) = True Then
                success = True
            End If

            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "D", "MAIN MENU", "CURRENCY", row.Cells(1).Text, "Code: " & row.Cells(1).Text & "/ Desc :  " & row.Cells(2).Text & "/ Rate:  " & row.Cells(3).Text & "/ Decimal : " & row.Cells(4).Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                MessageBoxValidation("Successfully deleted.", "Information")
                Dt = objCurrency.FillCurrency(Err_No, Err_Desc)
                BindCurrencyData()
                Resetfields()
            Else
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_CurrencyCode_005") & "&next=CurrencyCode.aspx&Title=Message", False)
                Exit Try
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
        btnUpdate.Visible = False
        btnSave.Visible = True
        Resetfields()

        Me.lblPop.Text = ""
        ClassUpdatePnl.Update()
        Me.MPEDetails.VisibleOnPageLoad = True

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
            txtCurrencyCode.Text = Server.HtmlDecode(Trim(row.Cells(1).Text))
            txtDescription.Text = Server.HtmlDecode(Trim(row.Cells(2).Text))
            txtRate.Text = Trim(row.Cells(3).Text)
            If Not ddlDigits.FindItemByValue(Server.HtmlDecode(Trim(row.Cells(4).Text))) Is Nothing Then
                ddlDigits.ClearSelection()
                ddlDigits.FindItemByValue(Server.HtmlDecode(Trim(row.Cells(4).Text))).Selected = True
            End If
            If Not ddl_Country.FindItemByValue(Server.HtmlDecode(Trim(row.Cells(5).Text))) Is Nothing Then
                ddl_Country.ClearSelection()
                ddl_Country.FindItemByValue(Server.HtmlDecode(Trim(row.Cells(5).Text))).Selected = True
            End If

            Me.txtCurrencyCode.Enabled = False
            MPEDetails.VisibleOnPageLoad = True
            ClassUpdatePnl.Update()

        Catch ex As Exception
            Err_No = "74066"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_CurrencyCode_006") & "&next=CurrencyCode.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub

    Private Sub gvCurrency_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvCurrency.PageIndexChanging
        gvCurrency.PageIndex = e.NewPageIndex
        Dt = objCurrency.FillCurrency(Err_No, Err_Desc)
        BindCurrencyData()

    End Sub

    Private Sub gvCurrency_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCurrency.RowDataBound
        'If e.Row.RowType.Equals(DataControlRowType.Pager) Then
        '    Dim pTableRow As TableRow = _
        '             CType(e.Row.Cells(0).Controls(0).Controls(0), TableRow)
        '    For Each cell As TableCell In pTableRow.Cells
        '        For Each control As Control In cell.Controls
        '            If TypeOf control Is LinkButton Then
        '                Dim lb As LinkButton = CType(control, LinkButton)
        '                lb.Attributes.Add("onclick", "ScrollToTop();")
        '            End If
        '        Next
        '    Next
        'End If
    End Sub
    Private Sub gvCurrency_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvCurrency.Sorting
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        Dt = objCurrency.FillCurrency(Err_No, Err_Desc)
        BindCurrencyData()
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