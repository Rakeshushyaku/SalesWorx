Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Telerik.Web.UI

Public Class Rep_OverallCoverageDetailed
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Request.QueryString("SID") Is Nothing And Not Request.QueryString("OrgID") Is Nothing And Not Request.QueryString("FromDt") Is Nothing And Not Request.QueryString("ToDt") Is Nothing Then
                HSID.Value = Request.QueryString("SID")
                hfOrg.Value = Request.QueryString("OrgID")
                hfSMonth.Value = Request.QueryString("FromDt")
                hfEMonth.Value = Request.QueryString("ToDt")
                lbl_Sp.Text = Request.QueryString("SPName")
                lbl_FromDate.Text = Request.QueryString("FromDt")
                lbl_ToDate.Text = Request.QueryString("ToDt")
                repDiv.Visible = True
                LoadPlanned()
                LoadVisited()
                LoadZeroBilled()
            Else
                repDiv.Visible = False
            End If
        End If
    End Sub
    Sub LoadPlanned()
        Dim dt As New DataTable
        Dim Objreport As New SalesWorx.BO.Common.Reports
        dt = Objreport.GetOverallCoverage_Planned(Err_No, Err_Desc, hfOrg.Value, HSID.Value, hfSMonth.Value, hfEMonth.Value)
        gvRep.DataSource = dt
        gvRep.DataBind()
        Objreport = Nothing
    End Sub
    Sub LoadPlannedFilter()
        Dim dt As New DataTable
        Dim Objreport As New SalesWorx.BO.Common.Reports
        dt = Objreport.GetOverallCoverage_Planned(Err_No, Err_Desc, hfOrg.Value, HSID.Value, hfSMonth.Value, hfEMonth.Value)
        If dt.Rows.Count > 0 Then
            Dim seldr() As DataRow
            seldr = dt.Select("Customer like '%" & txt_CustNo.Text & "%'")

            If seldr.Count > 0 Then
                gvRep.DataSource = seldr.CopyToDataTable()
                gvRep.DataBind()
            Else
                gvRep.DataSource = Nothing
                gvRep.DataBind()
            End If
           
        Else
            gvRep.DataSource = dt
            gvRep.DataBind()
        End If
      
        Objreport = Nothing
    End Sub
    Sub LoadVisited()
        Dim dt As New DataTable
        Dim Objreport As New SalesWorx.BO.Common.Reports
        dt = Objreport.GetOverallCoverage_Visited(Err_No, Err_Desc, hfOrg.Value, HSID.Value, hfSMonth.Value, hfEMonth.Value)
        gvRepVisited.DataSource = dt
        gvRepVisited.DataBind()
        Objreport = Nothing
    End Sub
    Sub LoadZeroBilled()
        Dim dt As New DataTable
        Dim Objreport As New SalesWorx.BO.Common.Reports
        dt = Objreport.GetOverallCoverage_ZeroBilled(Err_No, Err_Desc, hfOrg.Value, HSID.Value, hfSMonth.Value, hfEMonth.Value)
        gvRepZeroBilled.DataSource = dt
        gvRepZeroBilled.DataBind()
        Objreport = Nothing
    End Sub
    Sub LoadZeroBilledFilter()
        Dim dt As New DataTable
        Dim Objreport As New SalesWorx.BO.Common.Reports
        dt = Objreport.GetOverallCoverage_ZeroBilled(Err_No, Err_Desc, hfOrg.Value, HSID.Value, hfSMonth.Value, hfEMonth.Value)
        If dt.Rows.Count > 0 Then
            Dim seldr() As DataRow
            seldr = dt.Select("Customer like '%" & txt_CustNo_B.Text & "%'")

            If seldr.Count > 0 Then
                gvRepZeroBilled.DataSource = seldr.CopyToDataTable()
                gvRepZeroBilled.DataBind()
            Else
                gvRepZeroBilled.DataSource = Nothing
                gvRepZeroBilled.DataBind()
            End If

        Else
            gvRepZeroBilled.DataSource = dt
            gvRepZeroBilled.DataBind()
        End If
        Objreport = Nothing
    End Sub
    Sub LoadVisitedFilter()
        Dim dt As New DataTable
        Dim Objreport As New SalesWorx.BO.Common.Reports
        dt = Objreport.GetOverallCoverage_Visited(Err_No, Err_Desc, hfOrg.Value, HSID.Value, hfSMonth.Value, hfEMonth.Value)
        If dt.Rows.Count > 0 Then
            Dim seldr() As DataRow
            seldr = dt.Select("Customer like '%" & txt_CustNo_V.Text & "%'")

            If seldr.Count > 0 Then
                gvRepVisited.DataSource = seldr.CopyToDataTable()
                gvRepVisited.DataBind()
            Else
                gvRepVisited.DataSource = Nothing
                gvRepVisited.DataBind()
            End If

        Else
            gvRepVisited.DataSource = dt
            gvRepVisited.DataBind()
        End If
        Objreport = Nothing
    End Sub
    Private Sub dgv_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvRep.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        LoadPlanned()
    End Sub
    Private Sub dgv_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged

        LoadPlanned()
    End Sub

    Private Sub gvRepVisited_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvRepVisited.SortCommand
        ViewState("SortFieldVisited") = e.SortExpression
        SortDirectionVisited = "flip"
        LoadVisited()
    End Sub
    Private Sub gvRepVisited_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvRepVisited.PageIndexChanged

        LoadVisited()
    End Sub

    Private Sub gvRepZeroBilled_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvRepZeroBilled.SortCommand
        ViewState("SortFieldZB") = e.SortExpression
        SortDirectionZB = "flip"
        LoadZeroBilled()
    End Sub
    Private Sub gvRepZeroBilled_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvRepZeroBilled.PageIndexChanged

        LoadZeroBilled()
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

    Private Property SortDirectionVisited() As String
        Get
            If ViewState("SortDirectionVisited") Is Nothing Then
                ViewState("SortDirectionVisited") = "ASC"
            End If
            Return ViewState("SortDirectionVisited").ToString()
        End Get
        Set(ByVal value As String)
            Dim s As String = SortDirectionVisited

            If value = "flip" Then
                s = If(s = "ASC", "DESC", "ASC")
            Else
                s = value
            End If

            ViewState("SortDirectionVisited") = s
        End Set
    End Property

    Private Property SortDirectionZB() As String
        Get
            If ViewState("SortDirectionZB") Is Nothing Then
                ViewState("SortDirectionZB") = "ASC"
            End If
            Return ViewState("SortDirectionZB").ToString()
        End Get
        Set(ByVal value As String)
            Dim s As String = SortDirectionZB

            If value = "flip" Then
                s = If(s = "ASC", "DESC", "ASC")
            Else
                s = value
            End If

            ViewState("SortDirectionZB") = s
        End Set
    End Property

    'Private Sub Rep_OverallCoverageDetailed_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
    '    For Each gi As GridItem In rgVisits.MasterTableView.GetItems(GridItemType.GroupHeader)
    '        gi.Expanded = False
    '    Next
    'End Sub

    'Private Sub rgVisits_PreRender(sender As Object, e As EventArgs) Handles rgVisits.PreRender

    'End Sub

    Private Sub Btn_Clear_Click(sender As Object, e As EventArgs) Handles Btn_Clear.Click
        txt_CustNo.Text = ""
        LoadPlanned()
    End Sub

    Private Sub SearchBtn_Click(sender As Object, e As EventArgs) Handles SearchBtn.Click
        LoadPlannedFilter()
    End Sub

    Private Sub Btn_Clear_B_Click(sender As Object, e As EventArgs) Handles Btn_Clear_B.Click
        txt_CustNo_B.Text = ""
        LoadZeroBilled()
    End Sub

    Private Sub SearchBtn_B_Click(sender As Object, e As EventArgs) Handles Btn_search_B.Click
        LoadZeroBilledFilter()
    End Sub

    Private Sub Btn_Cancel_V_Click(sender As Object, e As EventArgs) Handles Btn_Cancel_V.Click
        txt_CustNo_V.Text = ""
        LoadVisited()
    End Sub

    Private Sub Btn_Seacrh_V_Click(sender As Object, e As EventArgs) Handles Btn_Seacrh_V.Click
        LoadVisitedFilter()
    End Sub
End Class