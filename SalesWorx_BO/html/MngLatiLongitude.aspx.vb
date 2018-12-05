Imports SalesWorx.BO.Common
Imports System.Data
Imports System.Web
Imports System.Resources
Imports System.Web.UI.WebControls
Imports log4net
Imports System.Text.RegularExpressions
Imports Telerik.Web.UI
Partial Public Class MngLatiLongitude
    Inherits System.Web.UI.Page
    Dim objLatitude As New LatiLongitude
    Dim Err_No As Long
    Dim Err_Desc As String

    Private Const PageID As String = "P262"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim ObjCommon As SalesWorx.BO.Common.Common
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

            ObjCommon = New SalesWorx.BO.Common.Common()
            Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
            ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
            ddlOrganization.DataBind()
            ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))
            If ddlOrganization.Items.Count = 2 Then
                ddlOrganization.SelectedIndex = 1
            End If



            Resetfields()

            If Request.QueryString("Src") = "Import" Then
                MessageBoxValidation("Geolocation Data successfully imported.", "Information")
            End If
        Else
            MPEDetails.VisibleOnPageLoad = False
            MapWindow.VisibleOnPageLoad = False
        End If
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            MPEDetails.VisibleOnPageLoad = False
            Resetfields()
            'ClassUpdatePnl.Update()
        Catch
        End Try
    End Sub
    Protected Sub btnCancelLoc_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            MapWindow.VisibleOnPageLoad = False
            Resetfields()
            'ClassUpdatePnl.Update()
        Catch
        End Try
    End Sub
    Public Sub Resetfields()
        Me.txtLatitude.Text = ""
        Me.txtLongitude.Text = ""
    End Sub
    Private Sub BindCustomerData()
        Dim dt As New DataTable
        Dt = objLatitude.FillCusShipAddress(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
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

            Me.lblMessage.Text = "Please enter valid Latitude Value."
            ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "initialize();", True)
            MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If

        If IsValidLatitude(Me.txtLongitude.Text.Trim()) = False Then
            Me.lblMessage.Text = "Please enter valid Longitude Value."
            ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "initialize();", True)
            MPEDetails.VisibleOnPageLoad = True
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
                MessageBoxValidation("Successfully updated.", "Information")
                MPEDetails.VisibleOnPageLoad = False

                BindCustomerData()
                Resetfields()

                ClassUpdatePnl.Update()
            Else
                success = False
                MessageBoxValidation("Could not update.", "Information")
                MPEDetails.VisibleOnPageLoad = False
                Resetfields()

                BindCustomerData()
                ClassUpdatePnl.Update()
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
    Protected Sub btnSet_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateLoc.Click
        If IsValidLatitude(Me.txtLoc_Latitude.Text.Trim()) = False Then

            Me.lbl_msg.Text = "Please enter valid Latitude Value."
            MapWindow.VisibleOnPageLoad = True
            Exit Sub
        End If

        If IsValidLatitude(Me.txtLoc_Long.Text.Trim()) = False Then
            Me.lbl_msg.Text = "Please enter valid Longitude Value."
            MapWindow.VisibleOnPageLoad = True
            Exit Sub
        End If

        Dim obj As New LatiLongitude

        Dim success As Boolean = False
        Try
            obj.Latitude = Convert.ToDouble(txtLoc_Latitude.Text.Trim())
            obj.Longitude = Convert.ToDouble(txtLoc_Long.Text.Trim())
            obj.CustomerId = CustID.Value.Trim()
            obj.SiteUserId = SiteID.Value.Trim()

            If obj.UpdateLatiLongitude(Err_No, Err_Desc) = True Then
                success = True
                MessageBoxValidation("Successfully updated.", "Information")
                MPEDetails.VisibleOnPageLoad = False
                BindCustomerData()
                Resetfields()

                ClassUpdatePnl.Update()
            Else
                success = False
                MessageBoxValidation("Could not update.", "Information")
                MPEDetails.VisibleOnPageLoad = False
                Resetfields()

                BindCustomerData()
                ClassUpdatePnl.Update()
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
            If ddlOrganization.SelectedIndex <= 0 Then
                MessageBoxValidation("Please Select the Orgainization.", "Validation")
                ClassUpdatePnl.Update()
                Exit Sub
            End If
            If ddFilterBy.SelectedIndex = 0 Then

                BindCustomerData()
                ClassUpdatePnl.Update()
            Else
                If txtFilterVal.Text.Trim() = String.Empty Then
                    MessageBoxValidation("Please enter text to filter.", "Validation")
                    ClassUpdatePnl.Update()
                    Exit Sub
                End If
                BindWithFilter()
                ClassUpdatePnl.Update()
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

    Private Sub BindWithFilter()
        Dim success As Boolean = False
        Try

            ' BindCustomerData()


            Dim dt As New DataTable
            dt = objLatitude.SearchLatiLongitude(Err_No, Err_Desc, IIf(ddFilterBy.SelectedIndex > 0, ddFilterBy.SelectedValue, "0"), txtFilterVal.Text, ddlOrganization.SelectedItem.Value)
            Me.gvLatitude.DataSource = dt
            Me.gvLatitude.DataBind()

            Dim dv As New DataView(dt)
            If ViewState("SortField") <> "" Then
                dv.Sort = (ViewState("SortField") & " ") + SortDirection
            End If
            gvLatitude.DataSource = dv
            gvLatitude.DataBind()
            Session.Remove("LatLng")
            Session("LatLng") = dt





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

    Protected Sub btnSetLocation_Click(ByVal sender As Object, ByVal e As EventArgs)

        Try
            Dim btnEdit As LinkButton = TryCast(sender, LinkButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
            CustID.Value = DirectCast(row.FindControl("lblCusId"), Label).Text
            SiteID.Value = DirectCast(row.FindControl("lblSiteId"), Label).Text
            txtLoc_Latitude.Text = CDec(Server.HtmlDecode(Trim(IIf(row.Cells(4).Text Is DBNull.Value Or row.Cells(4).Text = "", "0", row.Cells(4).Text))).Trim()).ToString("0.000000")
            txtLoc_Long.Text = CDec(Server.HtmlDecode(Trim(IIf(row.Cells(5).Text Is DBNull.Value Or row.Cells(5).Text = "", "0", row.Cells(5).Text))).Trim()).ToString("0.000000")

            Dim geo_mod As String = "L"


            Dim dt_geomod As New DataTable
            dt_geomod = objLatitude.GetGEO_loc_mod(Err_No, Err_Desc)
            If dt_geomod.Rows.Count > 0 Then
                If dt_geomod.Rows(0)("Control_Value").ToString().ToUpper().Trim() = "E" Then
                    geo_mod = "E"
                    Dim dt_Exgeoloc As New DataTable
                    dt_Exgeoloc = objLatitude.GetExpGEOLocation(Err_No, Err_Desc, CustID.Value, SiteID.Value)
                    If dt_Exgeoloc.Rows.Count > 0 Then
                        hd_last_Lat.Value = dt_Exgeoloc.Rows(0)("Latitude")
                        hd_last_Long.Value = dt_Exgeoloc.Rows(0)("Longitude")
                    Else
                        hd_last_Lat.Value = "-1"
                        hd_last_Long.Value = "-1"
                    End If
                Else
                    Dim dt_lastvisited As New DataTable
                    dt_lastvisited = objLatitude.GetLastVisit(Err_No, Err_Desc, CustID.Value, SiteID.Value)
                    If dt_lastvisited.Rows.Count > 0 Then
                        hd_last_Lat.Value = dt_lastvisited.Rows(0)("Last_Latitude")
                        hd_last_Long.Value = dt_lastvisited.Rows(0)("Last_Longitude")
                    Else
                        hd_last_Lat.Value = "-1"
                        hd_last_Long.Value = "-1"
                    End If
                End If
            End If




            '  MapWindow.VisibleOnPageLoad = True

            ScriptManager.RegisterStartupScript(Me, Page.GetType(), "openPopUp", "javascript:OpenLocWindow(" & txtLoc_Latitude.Text & "," & txtLoc_Long.Text & "," & hd_last_Lat.Value & "," & hd_last_Long.Value & "," & CustID.Value & "," & SiteID.Value & ",'" & geo_mod.Trim() & "');", True)


            ' ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "initialize2();", True)
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
    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            lblMessage.Text = ""
            btnUpdate.Visible = True
            Resetfields()
            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
            HidVal.Value = DirectCast(row.FindControl("lblCusId"), Label).Text
            hidUseId.Value = DirectCast(row.FindControl("lblSiteId"), Label).Text
            txtLatitude.Text = CDec(Server.HtmlDecode(Trim(IIf(row.Cells(4).Text Is DBNull.Value Or row.Cells(4).Text = "", "0", row.Cells(4).Text))).Trim()).ToString("0.000000")
            txtLongitude.Text = CDec(Server.HtmlDecode(Trim(IIf(row.Cells(5).Text Is DBNull.Value Or row.Cells(5).Text = "", "0", row.Cells(5).Text))).Trim()).ToString("0.000000")
            MPEDetails.VisibleOnPageLoad = True
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

    Protected Sub btnUpdateLastVisit_Click(sender As Object, e As EventArgs)

        If IsValidLatitude(Me.hd_last_Lat.Value.Trim()) = False Then

            Me.lbl_msg.Text = "Invalid last visited Latitude Value."
            MapWindow.VisibleOnPageLoad = True
            Exit Sub
        End If

        If IsValidLatitude(Me.hd_last_Long.Value.Trim()) = False Then
            Me.lbl_msg.Text = "Invalid last visited Longitude Value."
            MapWindow.VisibleOnPageLoad = True
            Exit Sub
        End If

        Dim obj As New LatiLongitude

        Dim success As Boolean = False
        Try
            obj.Latitude = Convert.ToDouble(hd_last_Lat.Value.Trim())
            obj.Longitude = Convert.ToDouble(hd_last_Long.Value.Trim())
            obj.CustomerId = CustID.Value.Trim()
            obj.SiteUserId = SiteID.Value.Trim()

            If obj.UpdateLatiLongitude(Err_No, Err_Desc) = True Then
                success = True
                MessageBoxValidation("Successfully updated.", "Information")
                MPEDetails.VisibleOnPageLoad = False

                BindCustomerData()
                Resetfields()

                ClassUpdatePnl.Update()
            Else
                success = False
                MessageBoxValidation("Could not update.", "Information")
                MPEDetails.VisibleOnPageLoad = False
                Resetfields()
                BindCustomerData()
                ClassUpdatePnl.Update()
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

    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrganization.SelectedIndexChanged
        Try
            If ddlOrganization.SelectedIndex <= 0 Then
                MessageBoxValidation("Please Select the Orgainization.", "Validation")
                ClassUpdatePnl.Update()
                Exit Sub
            End If
            If ddFilterBy.SelectedIndex = 0 Then

                BindCustomerData()
                ClassUpdatePnl.Update()
            Else
                If txtFilterVal.Text.Trim() = String.Empty Then
                    MessageBoxValidation("Please enter text to filter.", "Validation")
                    ClassUpdatePnl.Update()
                    Exit Sub
                End If
                BindWithFilter()
                ClassUpdatePnl.Update()
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
End Class