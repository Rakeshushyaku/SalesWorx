Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO

Partial Public Class CollectionListingAll
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Dim ObjCollection As Collection
    'Dim SortField As String = ""
    'Dim SortFieldDtl As String = ""
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
        If Not IsPostBack Then
            ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
            Try
                ObjCommon = New Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New ListItem("-- Select a value --"))
                txtFromDate.Text = Format(Now().Date, "dd/MM/yyyy")
                txtToDate.Text = Format(Now().Date, "dd/MM/yyyy")

                'BindData()
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
        Dim SearchQuery As String = ""
        Try
            ObjCollection = New Collection()
            ObjCommon = New Common()
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
            If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then
                ' SearchQuery = SearchQuery & " And A.Site_Use_ID=" & ddlOrganization.SelectedItem.Value

                If Not (ddVan.SelectedItem.Value = "-- Select a value --") Then
                    SearchQuery = SearchQuery & " And A.Collected_By=" & ddVan.SelectedItem.Value
                Else
                    SearchQuery = SearchQuery & " And A.Collected_By in (" & ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID) & ")"
                End If
                Dim ds As New DataSet
                ds = ObjCollection.GetCollectionList(Err_No, Err_Desc, SearchQuery, "", ddlOrganization.SelectedItem.Value)
                Dim dv As New DataView(ds.Tables("CollLstTbl"))
                If SortField <> "" Then
                    dv.Sort = (SortField & " ") + SortDirection
                End If
                ViewState("dt") = dv.ToTable()

              
                GVCollectionList.DataSource = dv
                GVCollectionList.DataBind()
                If dv.Count > 0 Then
                    If ds.Tables.Count > 0 Then
                        tblSummary.Visible = True
                        lblTotCash.Text = hfCurrency.Value & " " & FormatNumber(IIf(IsDBNull(ds.Tables(0).Compute("Sum(Amount)", "Collection_Type='CASH'")), 0, ds.Tables(0).Compute("Sum(Amount)", "Collection_Type='CASH'")), hfDC.Value).ToString()
                        lblTotPDC.Text = hfCurrency.Value & " " & FormatNumber(IIf(IsDBNull(ds.Tables(0).Compute("Sum(Amount)", "Collection_Type='PDC'")), 0, ds.Tables(0).Compute("Sum(Amount)", "Collection_Type='PDC'")), hfDC.Value).ToString()
                        lblTotCDC.Text = hfCurrency.Value & " " & FormatNumber(IIf(IsDBNull(ds.Tables(0).Compute("Sum(Amount)", "Collection_Type='CURR-CHQ'")), 0, ds.Tables(0).Compute("Sum(Amount)", "Collection_Type='CURR-CHQ'")), hfDC.Value).ToString()
                        lblTot.Text = hfCurrency.Value & " " & FormatNumber(IIf(IsDBNull(ds.Tables(0).Compute("Sum(Amount)", "")), 0, ds.Tables(0).Compute("Sum(Amount)", "")), hfDC.Value).ToString()
                    End If
                Else
                    tblSummary.Visible = False
                End If
                


                AddSortImage()
            Else
                MessageBoxValidation("Select an organization.")
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
    Protected Function GetPrice(ByVal SP As Object) As String
        'Dim FormatString As String = "{0:N" + dec.ToString() + "}"

        Return FormatNumber(SP, hfDC.Value).ToString()
    End Function
    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        MpInfoError.Show()
        Exit Sub
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

        If ddlOrganization.SelectedIndex > 0 Then


            Dim TemFromDateStr As String = txtFromDate.Text
            Dim DateArr As Array = TemFromDateStr.Split("/")
            If DateArr.Length = 3 Then
                TemFromDateStr = DateArr(1) & "/" & DateArr(0) & "/" & DateArr(2)
            End If
            Dim TemToDateStr As String = txtToDate.Text
            Dim DateArr1 As Array = TemToDateStr.Split("/")
            If DateArr1.Length = 3 Then
                TemToDateStr = DateArr1(1) & "/" & DateArr1(0) & "/" & DateArr1(2)
            End If

            If Not IsDate(TemFromDateStr) Then
                MessageBoxValidation("Enter valid ""From date"".")
                SetFocus(txtFromDate)
                Exit Sub
            End If

            If Not IsDate(TemToDateStr) Then
                MessageBoxValidation("Enter valid ""To date"".")
                SetFocus(TemToDateStr)
                Exit Sub
            End If

            If CDate(TemFromDateStr) > CDate(TemToDateStr) Then
                MessageBoxValidation("Start Date should not be greater than End Date.")
                Exit Sub
            End If

            ObjCommon = New Common()
            Dim dt As New DataTable
            dt = ObjCommon.GetCurrencyByOrg(Err_No, Err_Desc, Convert.ToInt32(ddlOrganization.SelectedValue))
            If dt.Rows.Count > 0 Then
                hfCurrency.Value = dt.Rows(0)(0).ToString()
                hfDC.Value = dt.Rows(0)(1).ToString()
            End If
        End If
        BindData()
    End Sub
    Protected Sub btnDetail_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim lnkbtnCollection_Ref_No As LinkButton = TryCast(sender, LinkButton)
        Dim row As GridViewRow = DirectCast(lnkbtnCollection_Ref_No.NamingContainer, GridViewRow)
        Try
            Dim tempstr As String
            tempstr = DirectCast(row.FindControl("hdnCollection_ID"), HiddenField).Value()
            hdnCollectionID.Value = tempstr
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
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub


    Protected Sub imgPrint_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgPrint.Click

        GVCollectionList.AllowPaging = False
        Dim dt As New DataTable
        dt = ViewState("dt")
        GVCollectionList.DataSource = dt
        GVCollectionList.DataBind()
        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)
        GVCollectionList.RenderControl(hw)
        Dim gridHTML As String = sw.ToString().Replace("""", "'") _
           .Replace(System.Environment.NewLine, "")
        Dim sb As New StringBuilder()
        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload = new function(){")
        sb.Append("var printWin = window.open('', '', 'left=0")
        sb.Append(",top=0,width=1000,height=1000,status=0');")
        sb.Append("printWin.document.write(""")
        sb.Append(gridHTML)
        sb.Append(""");")
        sb.Append("printWin.document.close();")
        sb.Append("printWin.focus();")
        sb.Append("printWin.print();")
        sb.Append("printWin.close();};")
        sb.Append("</script>")
        ClientScript.RegisterStartupScript(Me.[GetType](), "GridPrint", sb.ToString())
        GVCollectionList.AllowPaging = True
        GVCollectionList.DataBind()

    End Sub

    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New Common()
            ddVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())
            ddVan.DataBind()
            ddVan.Items.Insert(0, New ListItem("-- Select a value --"))
        End If
    End Sub
End Class