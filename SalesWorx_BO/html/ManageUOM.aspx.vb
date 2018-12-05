Imports SalesWorx.BO.Common
Imports System.Data
Imports System.Web
Imports System.Resources
Imports System.Web.UI.WebControls
Imports log4net
Imports System.Text.RegularExpressions

Partial Public Class ManageUOM
    Inherits System.Web.UI.Page

    Dim objProduct As New SalesWorx.BO.Common.Product
    Dim objUOM As New UOM
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim ProdID As String
    Dim OrgID As String
    Dim Primary_UOM As String
    Dim Item_Code As String
    Dim Err_No As Long
    Dim Err_Desc As String

    Private Const PageID As String = "P80"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        OrgID = Request.QueryString("OrgID")
        Primary_UOM = Request.QueryString("Primary_UOM")
        Item_Code = Request.QueryString("Item_Code")

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

            txtItemCode.Text = Item_Code
            Dim dtorg As New DataTable
            dtorg = (New SalesWorx.BO.Common.Common).GetOrganisationName(Err_No, Err_Desc, Request.QueryString("OrgID"))
            If dtorg.Rows.Count Then
                txtOrgID.Text = dtorg.Rows(0)("Description").ToString
            End If
            HOrgID.Value = Request.QueryString("OrgID")
            txtPrimaryUOM.Text = Primary_UOM
            FillUOM()

            BindItemUOMData()
            txtItemCode.Enabled = False
            txtOrgID.Enabled = False
        Else
            MPEDetails.VisibleOnPageLoad = False
        End If

    End Sub

    Sub FillUOM()
        ddlUOM.DataTextField = "Description"
        ddlUOM.DataValueField = "Code"
        ddlUOM.DataSource = objProduct.GetUOM()
        ddlUOM.DataBind()
        ddlUOM.SelectedIndex = 0
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
        Me.txtRate.Text = ""
        Me.lblPop.Text = ""
        ddlUOM.SelectedIndex = 0
        Me.txtRate.Enabled = True

    End Sub



    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim success As Boolean = False
        Try
            If Me.ddlUOM.SelectedIndex <= 0 Or Me.txtRate.Text = "" Then
                Me.lblPop.Text = "UOM and Convertion rate are required." ' "Select One UOM and converation rate are required."
                Me.txtRate.Enabled = True
                Me.MPEDetails.VisibleOnPageLoad = True
                Exit Sub
            End If
            If Val(txtRate.Text.Trim()) = 1 Then
                If ddlUOM.SelectedValue.Trim().ToUpper() <> Primary_UOM.Trim().ToUpper() Then
                    Me.lblPop.Text = "This is not Primary UOM. Please change conversion"
                    txtRate.Text = ""
                    Me.MPEDetails.VisibleOnPageLoad = True
                    Me.txtRate.Enabled = True
                    Exit Sub
                End If

            End If
            If ddlUOM.SelectedValue.Trim().ToUpper() = Primary_UOM.Trim().ToUpper() Then
                txtRate.Text = 1
            End If

            objUOM.Item_Code = txtItemCode.Text
            objUOM.Organization_ID = HOrgID.Value
            objUOM.Conversion = txtRate.Text
            objUOM.Item_UOM = ddlUOM.SelectedValue
            objUOM.Item_UOM_ID = 0
            objUOM.SellableUom = ddl_Sellable.SelectedValue

            If objUOM.InsertItemUOM(Err_No, Err_Desc) = True Then
                success = True
                MessageBoxValidation("Successfully saved", "Information")
            End If

            If success = True Then

                BindItemUOMData()
                Resetfields()
                Me.MPEDetails.VisibleOnPageLoad = False
                ClassUpdatePnl.Update()
            Else

                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("ManageUOM_001") & "&next=ManageUOM.aspx&Title=Message", False)
                Exit Try
            End If

            Me.txtRate.Enabled = True

        Catch ex As Exception
            log.Error(Err_Desc)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub
    Private Sub BindItemUOMData()
        Dim Dt As New DataTable
        Dt = objUOM.FillItemUOMGrid(Err_No, Err_Desc, txtItemCode.Text, HOrgID.Value)
        Me.gvUOM.DataSource = Dt
        Me.gvUOM.DataBind()

        Dim dv As New DataView(Dt)
        If ViewState("SortField") <> "" Then
            dv.Sort = (ViewState("SortField") & " ") + SortDirection
        End If
        gvUOM.DataSource = dv
        gvUOM.DataBind()
        Session.Remove("ItemUOM")
        Session("ItemUOM") = Dt
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
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

        Dim success As Boolean = False
        Try
            If Me.ddlUOM.SelectedIndex <= -1 Or Me.txtRate.Text = "" Then
                Me.lblPop.Text = "Select One UOM and converation rate are required."
                Me.MPEDetails.VisibleOnPageLoad = True
                Exit Sub
            End If
            If txtRate.Text.Trim() = 1 Then
                If ddlUOM.SelectedValue.Trim() <> Primary_UOM.Trim() Then
                    Me.lblPop.Text = "This is not Primary UOM.Please change conversion "
                    Me.MPEDetails.VisibleOnPageLoad = True
                    txtRate.Text = ""

                    Exit Sub
                End If
            End If
            If ddlUOM.SelectedValue.Trim() = Primary_UOM.Trim() Then
                txtRate.Text = "1"
            End If
            objUOM.Item_Code = txtItemCode.Text
            objUOM.Organization_ID = HOrgID.Value
            objUOM.Conversion = txtRate.Text
            objUOM.Item_UOM = ddlUOM.SelectedValue
            objUOM.Item_UOM_ID = 0
            objUOM.SellableUom = ddl_Sellable.SelectedValue
            If objUOM.UpdateItemUOM(Err_No, Err_Desc) = True Then
                success = True
                MessageBoxValidation("Successfully updated.", "Information")
            End If

            If success = True Then

                BindItemUOMData()
                Resetfields()
                Me.MPEDetails.VisibleOnPageLoad = False
                ClassUpdatePnl.Update()

            Else
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("ManageUOM_002") & "&next=ManageUOM.aspx&Title=Message", False)
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
            Me.MPEDetails.VisibleOnPageLoad = False
            Dim dr As GridViewRow
            Dim Success As Boolean = False
            Dim Selected As Boolean = False

            For Each dr In gvUOM.Rows
                Dim RowCheckBox As System.Web.UI.WebControls.CheckBox = dr.FindControl("chkDelete")

                If RowCheckBox.Checked = True Then
                    Selected = True
                    Dim Lbl As System.Web.UI.WebControls.Label = dr.FindControl("lblUOM")
                    HidVal.Value = Lbl.Text

                    objUOM.Item_Code = dr.Cells(1).Text
                    objUOM.Organization_ID = dr.Cells(2).Text
                    objUOM.Item_UOM = dr.Cells(3).Text
                    If objUOM.DeleteItemUOM(Err_No, Err_Desc) = True Then
                        Success = True
                    End If
                End If
            Next

            If Selected = False Then
                MessageBoxValidation("Select at least one UOM .", "Information")
                Exit Sub
            End If
            If (Success = True) Then
                MessageBoxValidation("UOM code(s) deleted successfully.", "Information")

                BindItemUOMData()
                Resetfields()
                ClassUpdatePnl.Update()
            Else
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_UOM_003") & "&next=ManageUOM.aspx&Title=Message", False)
                Exit Try
            End If
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

            BindItemUOMData()
            Me.MPEDetails.VisibleOnPageLoad = False
            ClassUpdatePnl.Update()
            success = True
            If success = False Then
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("ManageUOM_004") & "&next=ManageUOM.aspx&Title=Message", False)
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
        objUOM.Item_UOM = row.Cells(3).Text
        HidVal.Value = row.Cells(1).Text

        objUOM.Item_Code = txtItemCode.Text
        objUOM.Organization_ID = HOrgID.Value

        Me.MPEDetails.VisibleOnPageLoad = False
        Try

            If objUOM.DeleteItemUOM(Err_No, Err_Desc) = True Then
                success = True
            End If

            If success = True Then
                MessageBoxValidation("Successfully deleted.", "Information")

                BindItemUOMData()
                Resetfields()
            Else
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_ManageUOM_005") & "&next=ManageUOM.aspx&Title=Message", False)
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
        Me.txtRate.Enabled = True
        btnUpdate.Visible = False
        btnSave.Visible = True
        Resetfields()
        Me.lblPop.Text = ""
        ClassUpdatePnl.Update()
        Me.MPEDetails.VisibleOnPageLoad = True
        txtRate.ReadOnly = False
    End Sub


    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            btnUpdate.Visible = True
            btnSave.Visible = False
            Resetfields()
            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
            HidVal.Value = row.Cells(3).Text
            txtItemCode.Text = Trim(row.Cells(1).Text)
            txtOrgID.Text = Trim(row.Cells(2).Text)
            ddlUOM.SelectedValue = Trim(row.Cells(3).Text)
            txtRate.Text = Trim(row.Cells(4).Text)
            ddl_Sellable.SelectedValue = IIf(Trim(row.Cells(6).Text) = "", "N", row.Cells(6).Text)
            Me.MPEDetails.VisibleOnPageLoad = True

        Catch ex As Exception
            log.Error(Err_Desc)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub

    Private Sub gvUOM_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvUOM.PageIndexChanging
        objUOM.Item_Code = txtItemCode.Text
        objUOM.Organization_ID = HOrgID.Value
        gvUOM.PageIndex = e.NewPageIndex

        BindItemUOMData()

    End Sub

    Private Sub gvUOM_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvUOM.RowDataBound
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
    Private Sub gvUOM_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvUOM.Sorting
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        objUOM.Item_Code = txtItemCode.Text
        objUOM.Organization_ID = HOrgID.Value

        BindItemUOMData()
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
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("ManageProducts.aspx?ProdID=" & ProdID & "&OrgID=" & OrgID)
    End Sub
    Protected Sub ddlUOM_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)
        Try
            txtRate.ReadOnly = False
            If ddlUOM.SelectedIndex > -1 Then
                If ddlUOM.SelectedValue = Primary_UOM Then
                    txtRate.Text = "1"
                    txtRate.ReadOnly = True
                Else
                    txtRate.ReadOnly = False
                End If
            End If

        Catch ex As Exception

            log.Error(Err_Desc)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub
End Class
