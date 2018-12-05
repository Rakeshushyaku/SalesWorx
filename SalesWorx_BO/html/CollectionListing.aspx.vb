Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports Telerik.Web.UI
Imports log4net
Partial Public Class CollectionListing
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
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

    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
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

                ObjCommon = New SalesWorx.BO.Common.Common()
                Dim CountryTbl As DataTable = Nothing
                Dim orgTbl As DataTable = Nothing

                CountryTbl = ObjCommon.GetCountry(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)
                ddlCountry.DataSource = CountryTbl
                ddlCountry.DataBind()


                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)

                Dim s() As String = Nothing
                Dim Currency As String = Nothing
                Dim DecimalDigits As String = "2"
                Dim country As String = Nothing
                If CountryTbl.Rows.Count = 1 Then

                    ddlCountry.SelectedIndex = 0
                    dvCountry.Visible = False

                    s = ddlCountry.SelectedValue.Split("$")

                    If s.Length > 0 Then
                        country = s(0).ToString()
                        Currency = s(1).ToString()
                        DecimalDigits = s(2).ToString()
                    End If


                    ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
                    ddlOrganization.DataBind()


                ElseIf CountryTbl.Rows.Count > 1 Then
                    ddlCountry.SelectedIndex = 0
                    dvCountry.Visible = True


                    s = ddlCountry.SelectedValue.Split("$")

                    If s.Length > 0 Then
                        country = s(0).ToString()
                        Currency = s(1).ToString()
                        DecimalDigits = s(2).ToString()
                    End If

                    ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
                    ddlOrganization.DataBind()

                End If

                Dim OrgStr As String = Nothing
                For Each item As RadComboBoxItem In ddlOrganization.Items
                    item.Checked = True
                    If item.Checked Then

                        OrgStr = OrgStr & "," & item.Value

                    End If
                Next


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
        Else
            Me.MPECollection.VisibleOnPageLoad = False
        End If
    End Sub
    Private Sub BindData()
        CType(GVCollectionList.Columns(6), BoundField).DataFormatString = CType(Session.Item("USER_ACCESS"), UserAccess).DecimalDigits
        Dim SearchQuery As String = ""
        Try
            ObjCollection = New Collection()
            If (txtCollectionRefNo.Text = "" And txtFromDate.DateInput.Text = "" And txtToDate.DateInput.Text = "") Then
                SearchQuery = ""
            Else
                If txtCollectionRefNo.Text <> "" Then
                    SearchQuery = SearchQuery & " And A.Collection_Ref_No like '" & txtCollectionRefNo.Text & "%'"
                End If
                If txtFromDate.DateInput.Text <> "" Then
                    Dim TemFromDateStr As String = CDate(txtFromDate.SelectedDate).ToString("dd/MM/yyyy")
                    Dim DateArr As Array = TemFromDateStr.Split("/")
                    TemFromDateStr = DateArr(1) & "/" & DateArr(0) & "/" & DateArr(2)
                    SearchQuery = SearchQuery & " And cast(A.Cheque_Date as date) >= '" & TemFromDateStr & "'"
                    If txtToDate.DateInput.Text = "" Then
                        SearchQuery = SearchQuery & " And cast(A.Cheque_Date as date) <= '" & TemFromDateStr & " 23:59:59'"
                    End If
                End If
                If txtToDate.DateInput.Text <> "" Then
                    Dim TemToDateStr As String = CDate(txtToDate.SelectedDate).ToString("dd/MM/yyyy")
                    Dim DateArr As Array = TemToDateStr.Split("/")
                    TemToDateStr = DateArr(1) & "/" & DateArr(0) & "/" & DateArr(2)
                    SearchQuery = SearchQuery & " And cast(A.Cheque_Date as date) <= '" & TemToDateStr & " 23:59:59'"
                End If
            End If
            If Not ddlOrganization.CheckedItems Is Nothing Then
                If Not (ddlOrganization.CheckedItems.Count = "0") Then

                    'SearchQuery = SearchQuery & " And A.Site_Use_ID=" & ddlOrganization.SelectedItem.Value
                    Dim ds As New DataSet

                    Dim orgstr As String = ""
                    For Each itm As RadComboBoxItem In ddlOrganization.CheckedItems
                        orgstr = orgstr & itm.Value & ","
                    Next

                    ds = ObjCollection.GetHeldPDC(Err_No, Err_Desc, SearchQuery, "", orgstr)
                    Dim dv As New DataView(ds.Tables("CollLstTbl"))
                    If SortField <> "" Then
                        dv.Sort = (SortField & " ") + SortDirection
                    End If
                    GVCollectionList.DataSource = dv
                    GVCollectionList.DataBind()
                    AddSortImage()
                End If
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
        If Not ddlOrganization.CheckedItems Is Nothing Then
            If ddlOrganization.CheckedItems.Count = "0" Then
                MessageBoxValidation("Please select the Organisation", "Validation")
            Else
                BindData()
            End If
        Else
            MessageBoxValidation("Please select the Organisation", "Validation")
        End If
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
                MessageBoxValidation("Collection released successfully.", "Information")
                
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
                MessageBoxValidation("Collections released successfully.", "Information")
                
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
            MPECollection.VisibleOnPageLoad = True
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
        Me.MPECollection.VisibleOnPageLoad = False
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
                    hdnCollectionID.Value = ""
                    Me.MPECollection.VisibleOnPageLoad = False
                    MessageBoxValidation("Collection released successfully.", "Information")
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