Imports SalesWorx.BO.Common
Imports System.Data
Imports System.Web
Imports System.Resources
Imports System.Web.UI.WebControls
Imports log4net
Imports System.Text.RegularExpressions
Partial Public Class MngLatiLongitude
    Inherits System.Web.UI.Page
    Dim objLatitude As New LatiLongitude
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim Dt As New DataTable
    Private Const PageID As String = "P262"
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
            Dt = objLatitude.FillCusShipAddress(Err_No, Err_Desc)
            BindCustomerData()
            Resetfields()

            If Request.QueryString("Src") = "Import" Then
                Me.lblMessage.Text = "Geolocation Data successfully imported."
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
            End If

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
        Me.txtLatitude.Text = ""
        Me.txtLongitude.Text = ""
    End Sub
    Private Sub BindCustomerData()
        Me.gvLatitude.DataSource = Dt
        Me.gvLatitude.DataBind()

        Dim dv As New DataView(Dt)
        If ViewState("SortField") <> "" Then
            dv.Sort = (ViewState("SortField") & " ") + SortDirection
        End If
        gvLatitude.DataSource = dv
        gvLatitude.DataBind()
        Session.Remove("LatLng")
        Session("LatLng") = Dt
    End Sub
    Public Function IsValidLatitude(ByVal LatStr As String) As Boolean
        Dim _isDouble As System.Text.RegularExpressions.Regex = New  _
                Regex("^-?([1-8]?[1-9]|[1-9]0)\.{1}\d{1,6}")
        Return _isDouble.Match(LatStr).Success
    End Function
    Public Function IsValidLongitude(ByVal LatStr As String) As Boolean
        Dim _isDouble As System.Text.RegularExpressions.Regex = New  _
                Regex("^-?([1]?[1-7][1-9]|[1]?[1-8][0]|[1-9]?[0-9])\.{1}\d{1,6}")
        Return _isDouble.Match(LatStr).Success
    End Function

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If IsValidLatitude(Me.txtLatitude.Text.Trim()) = False Then
            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Please enter valid Latitude Value."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Me.MPECurrency.Show()
            Exit Sub
        End If

        If IsValidLatitude(Me.txtLongitude.Text.Trim()) = False Then
            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Please enter valid Longitude Value."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Me.MPECurrency.Show()
            Exit Sub
        End If

        Dim obj As New LatiLongitude

        Dim success As Boolean = False
        Try
            obj.Latitude = Convert.ToDouble(txtLatitude.Text.Trim())
            obj.Longitude = Convert.ToDouble(txtLongitude.Text.Trim())
            obj.CustomerId = HidVal.Value.Trim()
            obj.SiteUserId = hidUseId.Value.Trim()

            If obj.UpdateLatiLongitude(Err_No, Err_Desc) = True Then
                success = True
                Me.lblMessage.Text = "Successfully updated."
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
            End If

            If success = True Then
                '        Dt = objCurrency.FillCurrency(Err_No, Err_Desc)
                '        BindCustomerData()
                'BindWithFilter()
                Dt = objLatitude.FillCusShipAddress(Err_No, Err_Desc)
                BindCustomerData()

                Resetfields()
                Me.MPECurrency.Hide()

            Else

                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_002") & "&next=MngLatiLongitude.aspx&Title=Geolocation Management", False)
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

 

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Try
            If ddFilterBy.SelectedIndex = 0 Then
                txtFilterVal.Text = ""
                Me.lblinfo.Text = "Validation"
                Me.lblMessage.Text = "Please select valid option to filter."
                Me.lblMessage.ForeColor = Drawing.Color.Red
                ClassUpdatePnl.Update()
                Me.MpInfoError.Show()
                Exit Sub
            Else
                If txtFilterVal.Text.Trim() = String.Empty Then
                    Me.lblinfo.Text = "Validation"
                    Me.lblMessage.Text = "Please enter text to filter."
                    Me.lblMessage.ForeColor = Drawing.Color.Red
                    ClassUpdatePnl.Update()
                    Me.MpInfoError.Show()
                    Exit Sub
                End If
                
            End If

            BindWithFilter()
        Catch ex As Exception
            Err_No = "74064"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Private Sub BindWithFilter()
        Dim success As Boolean = False
        Try
            Dt = objLatitude.SearchLatiLongitude(Err_No, Err_Desc, Me.ddFilterBy.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text))
            BindCustomerData()
            ClassUpdatePnl.Update()
            success = True
            If success = False Then
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_005") & "&next=MngLatiLongitude.aspx&Title=Geolocation Management", False)
                Exit Try
            End If
        Catch ex As Exception
            Err_No = "74764"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub






    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Response.Redirect("ImportLatitude.aspx", False)
    End Sub


    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try

            btnUpdate.Visible = True
            Resetfields()
            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
            HidVal.Value = DirectCast(row.FindControl("lblCusId"), Label).Text
            hidUseId.Value = DirectCast(row.FindControl("lblSiteId"), Label).Text
            txtLatitude.Text = CDec(Server.HtmlDecode(Trim(IIf(row.Cells(4).Text Is DBNull.Value Or row.Cells(4).Text = "", "0", row.Cells(4).Text))).Trim()).ToString("0.000000")
            txtLongitude.Text = CDec(Server.HtmlDecode(Trim(IIf(row.Cells(5).Text Is DBNull.Value Or row.Cells(5).Text = "", "0", row.Cells(5).Text))).Trim()).ToString("0.000000")
            MPECurrency.Show()
        Catch ex As Exception
            Err_No = "74066"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_005") & "&next=MngLatiLongitude.aspx&Title=Geolocation Management", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub

    Private Sub gvLatitude_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvLatitude.PageIndexChanging
        gvLatitude.PageIndex = e.NewPageIndex
        ''    Dt = objLatitude.FillCusShipAddress(Err_No, Err_Desc)
        ''  BindCustomerData()
        BindWithFilter()

    End Sub

    Private Sub gvLatitude_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvLatitude.RowDataBound
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
    Private Sub gvLatitude_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvLatitude.Sorting
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        '' Dt = objLatitude.FillCusShipAddress(Err_No, Err_Desc)
        ''BindCustomerData()
        BindWithFilter()
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
        If Me.ddFilterBy.SelectedIndex = 0 Then
            Me.txtFilterVal.Text = ""
        End If
    End Sub

    Private Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        ddFilterBy.SelectedIndex = 0
        txtFilterVal.Text = ""
        BindWithFilter()
    End Sub
End Class