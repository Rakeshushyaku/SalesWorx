Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Partial Public Class CollectionListing
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Dim ObjCollection As Collection
    'Dim SortField As String = ""
    'Dim SortFieldDtl As String = ""
    Private Const PageID As String = "P11"
    Private RowIdx As Integer = 0
    Dim objLogin As New SalesWorx.BO.Common.Login
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private Property SortField() As String
        Get
            If ViewState("SortColumn") Is Nothing Then
                ViewState("SortColumn") = ""
            End If
            Return ViewState("SortColumn").ToString()
        End Get
        Set(ByVal value As String)
            ViewState("SortColumn") = value
        End Set
    End Property
    Private Property SortFieldDtl() As String
        Get
            If ViewState("SortColumn1") Is Nothing Then
                ViewState("SortColumn1") = ""
            End If
            Return ViewState("SortColumn1").ToString()
        End Get
        Set(ByVal value As String)
            ViewState("SortColumn1") = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        If Not IsPostBack Then
            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            If Not HasPermission Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If
            ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
            Try
                ObjCommon = New Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New ListItem("-- Select a Organization --"))
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy")
                txtToDate.Text = DateTime.Now.Date.AddDays(30).ToString("dd/MM/yyyy")

                CType(GVCollectionList.Columns(7), BoundField).DataFormatString = CType(Session.Item("USER_ACCESS"), UserAccess).DecimalDigits
                

                BindData()
                If Not Err_Desc Is Nothing Then
                    log.Error(Err_Desc)
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
                End If
            Catch ex As Exception
                Err_No = "74066"
                If Err_Desc Is Nothing Then
                    log.Error(GetExceptionInfo(ex))
                Else
                    log.Error(Err_Desc)
                End If
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
            Finally
                ObjCommon = Nothing
                ErrorResource = Nothing
            End Try
        End If
    End Sub
    Private Sub BindData()
        CType(GVCollectionList.Columns(6), BoundField).DataFormatString = CType(Session.Item("USER_ACCESS"), UserAccess).DecimalDigits
        Dim SearchQuery As String = ""
        Try
            ObjCollection = New Collection()
            If (txtCollectionRefNo.Text = "" And txtFromDate.Text = "" And txtToDate.Text = "") Then
                SearchQuery = ""
            Else
                If txtCollectionRefNo.Text <> "" Then
                    SearchQuery = SearchQuery & " And A.Collection_Ref_No like '" & txtCollectionRefNo.Text & "%'"
                End If
                If txtFromDate.Text <> "" Then
                    Dim TemFromDateStr As String = txtFromDate.Text
                    Dim DateArr As Array = TemFromDateStr.Split("/")
                    TemFromDateStr = DateArr(1) & "/" & DateArr(0) & "/" & DateArr(2)
                    SearchQuery = SearchQuery & " And A.Collected_On >= '" & TemFromDateStr & "'"
                    If txtToDate.Text = "" Then
                        SearchQuery = SearchQuery & " And A.Collected_On <= '" & TemFromDateStr & " 23:59:59'"
                    End If
                End If
                If txtToDate.Text <> "" Then
                    Dim TemToDateStr As String = txtToDate.Text
                    Dim DateArr As Array = TemToDateStr.Split("/")
                    TemToDateStr = DateArr(1) & "/" & DateArr(0) & "/" & DateArr(2)
                    SearchQuery = SearchQuery & " And A.Collected_On <= '" & TemToDateStr & " 23:59:59'"
                End If
            End If
            If Not (ddlOrganization.SelectedItem.Value = "-- Select a Organization --") Then
                'SearchQuery = SearchQuery & " And A.Site_Use_ID=" & ddlOrganization.SelectedItem.Value
                Dim ds As New DataSet
                ds = ObjCollection.GetHeldPDC(Err_No, Err_Desc, SearchQuery, "", ddlOrganization.SelectedItem.Value)
                Dim dv As New DataView(ds.Tables("CollLstTbl"))
                If SortField <> "" Then
                    dv.Sort = (SortField & " ") + SortDirection
                End If
                GVCollectionList.DataSource = dv
                GVCollectionList.DataBind()
                AddSortImage()
            End If
        Catch ex As Exception
            If Err_Desc IsNot Nothing Then
                log.Error(Err_Desc)
            Else
                log.Error(GetExceptionInfo(ex))
            End If
            Err_No = "74067"
            '  Err_Desc = ex.Message
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
        Finally
            ObjCollection = Nothing
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
    Public Sub AddSortImage()
        If SortField = "" Then
            Exit Sub
        End If
        Dim sortImage As New Image()
        sortImage.Style("padding-left") = "8px"
        sortImage.Style("padding-bottom") = "1px"
        If SortDirection = "ASC" Then
            sortImage.ImageUrl = "~/images/arrowUp.gif"
            sortImage.AlternateText = "Ascending Order"
        Else
            sortImage.ImageUrl = "~/images/arrowDown.gif"
            sortImage.AlternateText = "Descending Order"
        End If
        For i As Integer = 0 To GVCollectionList.Columns.Count - 1
            Dim dcf As DataControlField = GVCollectionList.Columns(i)
            If dcf.SortExpression = SortField Then
                If Not IsNothing(GVCollectionList.HeaderRow) Then
                    GVCollectionList.HeaderRow.Cells(i).Controls.Add(sortImage)
                End If
                Exit For
            End If
        Next
    End Sub

    Private Sub GVCollectionList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GVCollectionList.PageIndexChanging
        GVCollectionList.PageIndex = e.NewPageIndex
        BindData()
    End Sub

    Private Sub GVCollectionList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GVCollectionList.Sorting
        SortField = e.SortExpression
        SortDirection = "flip"
        BindData()
    End Sub

    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        BindData()
    End Sub
    Protected Sub btnRelease_Click(ByVal sender As Object, ByVal e As EventArgs)
        ObjCollection = New Collection()
        Dim btnrelease As ImageButton = TryCast(sender, ImageButton)
        Dim row As GridViewRow = DirectCast(btnrelease.NamingContainer, GridViewRow)
        Try
            Dim Success As Boolean = False
            Dim tempstr As String
            Dim intReleasedby As Integer
            intReleasedby = CType(Session("User_Access"), UserAccess).UserID
            tempstr = DirectCast(row.FindControl("hdnCollection_ID"), HiddenField).Value()
            Dim refno As String = DirectCast(row.FindControl("lnkbtnCollection_Ref_No"), LinkButton).Text
            '  Dim s As String() = row.Cells(4).Text.Split("-")
            Dim VanID As String = row.Cells(4).Text
            '  If s.Length > 1 Then
            'VanID = s(1)
            '  End If
            Success = ObjCollection.ReleaseCollectionList(Err_No, Err_Desc, "'" & tempstr & "'", intReleasedby)
            If (Success = True) Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "A", "APPROVAL", "HELD PDC", refno, "Ref No: " & refno & "/ Collected On :  " & row.Cells(3).Text & "/ Collected By :  " & row.Cells(4).Text & "/ Customer :  " & row.Cells(5).Text & "/ Amount :  " & row.Cells(6).Text & "/ Emp Code :  " & row.Cells(10).Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), VanID.Trim(), Me.ddlOrganization.SelectedValue.ToString())
                lblMessage.Text = "Collection released successfully."
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
                BindData()
            Else
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_CurrencyCode_003") & "&next=CurrencyCode.aspx&Title=Message", False)
                Exit Try
            End If
        Catch ex As Exception
            Err_No = "74065"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        Finally
            ObjCollection = Nothing
        End Try
    End Sub
    Protected Sub btnReleaseAll_Click()
        ObjCollection = New Collection()
        Try
            Dim dr As GridViewRow
            Dim Success As Boolean = False
            For Each dr In GVCollectionList.Rows
                Dim RowCheckBox As System.Web.UI.WebControls.CheckBox = dr.FindControl("chkRelease")

                If RowCheckBox.Checked = True Then
                    Dim tempstr As String
                    Dim intReleasedby As Integer
                    intReleasedby = CType(Session("User_Access"), UserAccess).UserID
                    tempstr = DirectCast(dr.FindControl("hdnCollection_ID"), HiddenField).Value()
                    Dim refno As String = DirectCast(dr.FindControl("lnkbtnCollection_Ref_No"), LinkButton).Text

                    '   Dim s As String() = dr.Cells(4).Text.Split("-")
                    Dim VanID As String = dr.Cells(4).Text
                    '  If s.Length > 1 Then
                    'VanID = s(1)
                    ' End If

                    Success = ObjCollection.ReleaseCollectionList(Err_No, Err_Desc, "'" & tempstr & "'", intReleasedby)
                    objLogin.SaveUserLog(Err_No, Err_Desc, "A", "APPROVAL", "HELD PDC", refno, "Ref No: " & refno & "/ Collected On :  " & dr.Cells(3).Text & "/ Collected By :  " & dr.Cells(4).Text & "/ Customer :  " & dr.Cells(5).Text & "/ Amount :  " & dr.Cells(6).Text & "/ Emp Code :  " & dr.Cells(10).Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), VanID.Trim(), Me.ddlOrganization.SelectedValue.ToString())
                End If
            Next
            If (Success = True) Then
                lblMessage.Text = "Collections released successfully."
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
                BindData()
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
        Finally
            ObjCollection = Nothing
        End Try
    End Sub
    Protected Sub btnDetail_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim lnkbtnCollection_Ref_No As LinkButton = TryCast(sender, LinkButton)
        Dim row As GridViewRow = DirectCast(lnkbtnCollection_Ref_No.NamingContainer, GridViewRow)
        RowIdx = row.RowIndex
        Try
            Dim tempstr As String
            tempstr = DirectCast(row.FindControl("hdnCollection_ID"), HiddenField).Value()
            hdnCollectionID.Value = tempstr
            CType(GVCollDtlList.Columns(2), BoundField).DataFormatString = CType(Session.Item("USER_ACCESS"), UserAccess).DecimalDigits
            BindDetailData()
            MPECollection.Show()
        Catch ex As Exception
            Err_No = "74065"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub
    Private Sub BindDetailData()
        CType(GVCollDtlList.Columns(2), BoundField).DataFormatString = CType(Session.Item("USER_ACCESS"), UserAccess).DecimalDigits
        Dim SearchQuery As String = ""
        Try
            If hdnCollectionID.Value <> "" Then
                ObjCollection = New Collection()
                SearchQuery = "'" & hdnCollectionID.Value & "'"
                Dim ds As New DataSet
                ds = ObjCollection.GetCollectionDetailList(Err_No, Err_Desc, SearchQuery, "")
                Dim dv As New DataView(ds.Tables("CollDtlLstTbl"))
                If SortField <> "" Then
                    dv.Sort = (SortFieldDtl & " ") + SortDirectionDtl
                End If
                GVCollDtlList.DataSource = dv
                GVCollDtlList.DataBind()
                AddSortImageDtl()
            End If
        Catch ex As Exception
            If Err_Desc IsNot Nothing Then
                log.Error(Err_Desc)
            Else
                log.Error(GetExceptionInfo(ex))
            End If
            Err_No = "74067"
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
        Finally
            ObjCollection = Nothing
        End Try
    End Sub
    Private Property SortDirectionDtl() As String
        Get
            If ViewState("SortDirectionDtl") Is Nothing Then
                ViewState("SortDirectionDtl") = "ASC"
            End If
            Return ViewState("SortDirectionDtl").ToString()
        End Get
        Set(ByVal value As String)
            Dim s As String = SortDirectionDtl

            If value = "flip" Then
                s = If(s = "ASC", "DESC", "ASC")
            Else
                s = value
            End If

            ViewState("SortDirectionDtl") = s
        End Set
    End Property
    Public Sub AddSortImageDtl()
        If SortField = "" Then
            Exit Sub
        End If
        Dim sortImage As New Image()
        sortImage.Style("padding-left") = "8px"
        sortImage.Style("padding-bottom") = "1px"
        If SortDirectionDtl = "ASC" Then
            sortImage.ImageUrl = "~/images/arrowUp.gif"
            sortImage.AlternateText = "Ascending Order"
        Else
            sortImage.ImageUrl = "~/images/arrowDown.gif"
            sortImage.AlternateText = "Descending Order"
        End If
        For i As Integer = 0 To GVCollDtlList.Columns.Count - 1
            Dim dcf As DataControlField = GVCollDtlList.Columns(i)
            If dcf.SortExpression = SortFieldDtl Then
                GVCollDtlList.HeaderRow.Cells(i).Controls.Add(sortImage)
                Exit For
            End If
        Next
    End Sub

    Private Sub GVCollDtlList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GVCollDtlList.PageIndexChanging
        GVCollDtlList.PageIndex = e.NewPageIndex
        BindDetailData()
    End Sub

    Private Sub GVCollDtlList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GVCollDtlList.Sorting
        SortFieldDtl = e.SortExpression
        SortDirectionDtl = "flip"
        BindDetailData()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.MPECollection.Hide()
    End Sub

    Private Sub btnReleaseDtl_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReleaseDtl.Click
        ObjCollection = New Collection()
        Try

            If hdnCollectionID.Value <> "" Then
                Dim Success As Boolean = False
                Dim tempstr As String
                Dim intReleasedby As Integer
                intReleasedby = CType(Session("User_Access"), UserAccess).UserID
                tempstr = hdnCollectionID.Value
                Dim refno As String = DirectCast(GVCollectionList.Rows(RowIdx).FindControl("lnkbtnCollection_Ref_No"), LinkButton).Text
                ' Dim s As String() = GVCollectionList.Rows(RowIdx).Cells(4).Text.Split("-")
                Dim VanID As String = GVCollectionList.Rows(RowIdx).Cells(4).Text
                '  If s.Length > 1 Then
                'VanID = s(1)
                ' End If
                Success = ObjCollection.ReleaseCollectionList(Err_No, Err_Desc, "'" & tempstr & "'", intReleasedby)
                'objLogin.SaveUserLog(Err_No, Err_Desc, "A", "APPROVAL", "HELD PDC", Me.ddlOrganization.SelectedValue.ToString(), "Ref No: " & GVCollDtlList.SelectedRow.Cells(0).Text & "- Invoice No :  " & GVCollDtlList.SelectedRow.Cells(1).Text & "- Amount :  " & GVCollDtlList.SelectedRow.Cells(2).Text & "- Status :  " & GVCollDtlList.SelectedRow.Cells(3).Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", Me.ddlOrganization.SelectedItem.Text)
                objLogin.SaveUserLog(Err_No, Err_Desc, "A", "APPROVAL", "HELD PDC", refno, "Ref No: " & refno & "/ Collected On :  " & GVCollectionList.Rows(RowIdx).Cells(3).Text & "/ Collected By :  " & GVCollectionList.Rows(RowIdx).Cells(4).Text & "/ Customer :  " & GVCollectionList.Rows(RowIdx).Cells(5).Text & "/ Amount :  " & GVCollectionList.Rows(RowIdx).Cells(6).Text & "/ Emp Code :  " & GVCollectionList.Rows(RowIdx).Cells(10).Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), VanID.Trim(), Me.ddlOrganization.SelectedValue.ToString())
                If (Success = True) Then
                    lblMessage.Text = "Collection released successfully."
                    lblMessage.ForeColor = Drawing.Color.Green
                    lblinfo.Text = "Information"
                    hdnCollectionID.Value = ""
                    Me.MPECollection.Hide()
                    MpInfoError.Show()
                    btnClose.Focus()
                    BindData()
                    RowIdx = 0
                Else
                    log.Error(Err_Desc)
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_CurrencyCode_003") & "&next=CurrencyCode.aspx&Title=Message", False)
                    Exit Try
                End If
            End If
        Catch ex As Exception
            Err_No = "74065"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        Finally
            ObjCollection = Nothing
        End Try
    End Sub
End Class