Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO

Partial Public Class ProductListing
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Dim ObjProduct As Product
    ' Dim SortField As String = ""
    Private Const PageID As String = "P19"
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
                ddlOrganization.Items.Insert(0, New ListItem("-- Select a value --"))
                ' BindData()
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
            ObjProduct = New Product()
            If (ddlOrganization.SelectedItem.Value = "-- Select a value --" And txtItemCode.Text = "" And txtDescription.Text = "") Then
                SearchQuery = ""
            ElseIf (txtItemCode.Text = "" And txtDescription.Text = "" And ddlOrganization.SelectedItem.Value <> "-- Select a Division --") Then
                SearchQuery = " And b.Organization_ID=" & ddlOrganization.SelectedItem.Value
            ElseIf (txtItemCode.Text <> "" And txtDescription.Text = "" And ddlOrganization.SelectedItem.Value = "-- Select a Division --") Then
                SearchQuery = "  and  b.Item_Code like'" & txtItemCode.Text & "%'"
            ElseIf (txtItemCode.Text <> "" And txtDescription.Text <> "" And ddlOrganization.SelectedItem.Value = "-- Select a Division --") Then
                SearchQuery = " and  b.Item_Code like'" & txtItemCode.Text & "%'" & " AND b.[Description] like'" & txtDescription.Text & "%'"
            ElseIf (txtItemCode.Text <> "" And txtDescription.Text = "" And ddlOrganization.SelectedItem.Value <> "-- Select a Division --") Then
                SearchQuery = " And b.Organization_ID=" & ddlOrganization.SelectedItem.Value & " and  b.Item_Code like'" & txtItemCode.Text & "%'"
            ElseIf (txtItemCode.Text <> "" And txtDescription.Text <> "" And ddlOrganization.SelectedItem.Value <> "-- Select a Division --") Then
                SearchQuery = " And b.Organization_ID=" & ddlOrganization.SelectedItem.Value & " and  b.Item_Code like'" & txtItemCode.Text & "%'" & " AND b.[Description] like'" & txtDescription.Text & "%'"
            ElseIf (txtItemCode.Text = "" And txtDescription.Text <> "" And ddlOrganization.SelectedItem.Value <> "-- Select a Division --") Then
                SearchQuery = " And b.Organization_ID=" & ddlOrganization.SelectedItem.Value & " AND b.[Description] like'" & txtDescription.Text & "%'"
            End If
            If ddlProductType.SelectedValue = 1 Then
                SearchQuery = SearchQuery & " And a.Row_ID is not null"
            ElseIf ddlProductType.SelectedValue = 2 Then
                SearchQuery = SearchQuery & " And a.Row_ID is null"
            Else
                SearchQuery = SearchQuery
            End If
            If Not (ddlOrganization.SelectedItem.Value = "-- Select a Division --") Then
                Dim ds As New DataSet
                ds = ObjProduct.GetProductList(Err_No, Err_Desc, SearchQuery, "")
                Dim dv As New DataView(ds.Tables("PrdLstTbl"))
                If SortField <> "" Then
                    dv.Sort = (SortField & " ") + SortDirection
                Else
                    dv.Sort = "Item_Code ASC"
                    SortDirection = "ASC"
                    SortField = "Item_Code"
                End If

                GVProductList.DataSource = dv
                GVProductList.DataBind()
                ViewState("dt") = dv.ToTable()
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
            ObjProduct = Nothing
        End Try
    End Sub
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
        For i As Integer = 0 To GVProductList.Columns.Count - 1
            Dim dcf As DataControlField = GVProductList.Columns(i)
            If dcf.SortExpression = SortField Then
                If Not IsNothing(GVProductList.HeaderRow) Then
                    GVProductList.HeaderRow.Cells(i).Controls.Add(sortImage)
                End If
                Exit For
            End If
        Next
    End Sub

    Private Sub GVProductList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GVProductList.PageIndexChanging
        GVProductList.PageIndex = e.NewPageIndex
        BindData()
    End Sub

    Private Sub GVProductList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GVProductList.Sorting
        SortField = e.SortExpression
        SortDirection = "flip"
        BindData()
    End Sub

    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ddlOrganization.SelectedValue = "-- Select a value --" Then
            MessageBoxValidation("Select organization.")
            Exit Sub
        End If
        BindData()
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub


    Protected Sub imgPrint_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgPrint.Click

        GVProductList.AllowPaging = False
        Dim dt As New DataTable
        dt = ViewState("dt")
        GVProductList.DataSource = dt
        GVProductList.DataBind()
        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)
        GVProductList.RenderControl(hw)
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
        GVProductList.AllowPaging = True
        GVProductList.DataBind()

    End Sub
End Class