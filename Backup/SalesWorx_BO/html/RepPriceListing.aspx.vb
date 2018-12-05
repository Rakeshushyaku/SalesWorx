Imports System.Configuration.ConfigurationManager


Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms

Partial Public Class RepPriceListing
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Dim ObjPrice As Price
    Private ReportPath As String = "PriceListing"
    Private Const PageID As String = "P200"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not IsNothing(Me.Master) Then

            Dim masterScriptManager As ScriptManager
            masterScriptManager = CType(Master.FindControl("ScriptManager1"), ScriptManager)

            ' Make sure our master page has the script manager we're looking for
            If Not IsNothing(masterScriptManager) Then

                ' Turn off partial page postbacks for this page
                masterScriptManager.EnablePartialRendering = False
            End If

        End If

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
                ObjCommon = New Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)

                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New ListItem("-- Select a value --", "0"))


                ddlUOM.DataValueField = "Item_UOM"
                ddlUOM.DataTextField = "Item_UOM"
                ddlUOM.DataSource = ObjCommon.GetAllUOM(Err_No, Err_Desc)
                ddlUOM.DataBind()
                ddlUOM.Items.Insert(0, New ListItem("-- Select value --", "0"))

               

                'ddlAgency.DataValueField = "Agency"
                'ddlAgency.DataTextField = "Agency"
                'ddlAgency.DataSource = ObjCommon.GetAllAgency(Err_No, Err_Desc)
                'ddlAgency.DataBind()
                'ddlAgency.Items.Insert(0, New ListItem("-- Select value --"))

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
    Sub MessageBoxValidation(ByVal str As String)
        Me.lblMessage.Text = str
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        MpInfoError.Show()
        btnClose.Focus()
        Exit Sub
    End Sub
    Private Sub BindData()
        Dim SearchQuery As String = ""
        Try
            ObjPrice = New Price()
            'If (ddlType.SelectedItem.Value = "-- Select a value --") Then 'And txtPriceListLineID.Text = "" And txtItemUOM.Text = "") Then
            '    SearchQuery = ""
            'Else

            '    If ddlType.SelectedValue <> "0" Then
            '        SearchQuery = SearchQuery & " And A.Price_List_ID=" & ddlType.SelectedValue
            '    End If

            '    If drpProduct.SelectedValue <> "0" Then
            '        SearchQuery = SearchQuery & " And P.Item_Code ='" & drpProduct.SelectedValue & "'"
            '    End If
            '    If ddlUOM.SelectedValue <> "0" Then
            '        SearchQuery = SearchQuery & " And A.Item_UOM ='" & ddlUOM.SelectedValue & "'"
            '    End If
            '    If ddlAgency.SelectedValue <> "0" Then
            '        SearchQuery = SearchQuery & " And P.Agency ='" & ddlAgency.SelectedValue & "'"
            '    End If
            '    'If txtPriceListLineID.Text <> "" Then
            '    '    SearchQuery = SearchQuery & " And A.Price_List_Line_ID like '" & txtPriceListLineID.Text & "%'"
            '    'End If
            '    'If txtItemUOM.Text <> "" Then
            '    '    SearchQuery = SearchQuery & " And A.Item_UOM like '" & txtItemUOM.Text & "%'"
            '    'End If
            'End If
            If Not (ddlOrganization.SelectedItem.Value = "0") Then
                SearchQuery = SearchQuery & " And A.Organization_ID=" & ddlOrganization.SelectedItem.Value

                If ddlType.SelectedValue <> "0" Then
                    SearchQuery = SearchQuery & " And A.Price_List_ID=" & ddlType.SelectedValue
                End If

                If drpProduct.SelectedValue <> "0" Then
                    SearchQuery = SearchQuery & " And P.Item_Code ='" & drpProduct.SelectedValue & "'"
                End If
                If ddlUOM.SelectedValue <> "0" Then
                    SearchQuery = SearchQuery & " And A.Item_UOM ='" & ddlUOM.SelectedValue & "'"
                End If
                If ddlAgency.SelectedValue <> "0" Then
                    SearchQuery = SearchQuery & " And P.Agency ='" & ddlAgency.SelectedValue & "'"
                End If
                InitReportViewer(SearchQuery)

                'Dim ds As New DataSet
                'ds = ObjPrice.GetPriceList(Err_No, Err_Desc, SearchQuery, "")
                'Dim dv As New DataView(ds.Tables("PriceLstTbl"))
                'If SortField <> "" Then
                '    dv.Sort = (SortField & " ") + SortDirection
                'Else
                '    dv.Sort = "Item_Code ASC"
                '    SortDirection = "ASC"
                '    SortField = "Item_Code"
                'End If
                ' GVPriceList.DataSource = dv
                ' GVPriceList.DataBind()
                ' ViewState("dt") = dv.ToTable()

                'AddSortImage()
            Else
                MessageBoxValidation("Select an organization.")
                ' GVPriceList.DataSource = Nothing
                'GVPriceList.DataBind()
                Return
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
            ObjPrice = Nothing
        End Try
    End Sub


    Private Sub InitReportViewer(ByVal FilterValue As String)
        Try

            'RVMain.ZoomPercent = 150
            'RVMain.ZoomMode = ZoomMode.Percent

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim Searchvalue As New ReportParameter
            Searchvalue = New ReportParameter("SearchParams", CStr(IIf(FilterValue Is Nothing, "", FilterValue)))

            Dim Org As New ReportParameter
            Org = New ReportParameter("Org_ID", CStr(IIf(ddlOrganization.SelectedItem.Value = "0", "All", ddlOrganization.SelectedItem.Text)))

            Dim Agency As New ReportParameter
            Agency = New ReportParameter("Agency", CStr(IIf(ddlAgency.SelectedItem.Value = "0", "All", ddlAgency.SelectedItem.Text)))

            Dim UOM As New ReportParameter
            UOM = New ReportParameter("UOM", CStr(IIf(ddlUOM.SelectedItem.Value = "0", "All", ddlUOM.SelectedItem.Text)))

            Dim PriceList As New ReportParameter
            PriceList = New ReportParameter("PriceList", CStr(IIf(ddlType.SelectedItem.Value = "0", "All", ddlType.SelectedItem.Text)))

            With RVMain

                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                .ServerReport.SetParameters(New ReportParameter() {Searchvalue, Org, Agency, UOM, PriceList})
                .ServerReport.Refresh()

            End With


        Catch Ex As Exception
            '  log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub
    'Protected Function GetSellingPrice(ByVal SP As Object, ByVal dec As Object) As String
    '    'Dim FormatString As String = "{0:N" + dec.ToString() + "}"
    '    Return FormatNumber(SP, Convert.ToInt32(dec)).ToString()
    'End Function

    'Private Property SortDirection() As String
    '    Get
    '        If ViewState("SortDirection") Is Nothing Then
    '            ViewState("SortDirection") = "ASC"
    '        End If
    '        Return ViewState("SortDirection").ToString()
    '    End Get
    '    Set(ByVal value As String)
    '        Dim s As String = SortDirection

    '        If value = "flip" Then
    '            s = If(s = "ASC", "DESC", "ASC")
    '        Else
    '            s = value
    '        End If

    '        ViewState("SortDirection") = s
    '    End Set
    'End Property
    'Private Property SortField() As String
    '    Get
    '        If ViewState("SortColumn") Is Nothing Then
    '            ViewState("SortColumn") = ""
    '        End If
    '        Return ViewState("SortColumn").ToString()
    '    End Get
    '    Set(ByVal value As String)
    '        ViewState("SortColumn") = value
    '    End Set
    ' End Property
    'Public Sub AddSortImage()
    '    If SortField = "" Then
    '        Exit Sub
    '    End If
    '    Dim sortImage As New Image()
    '    sortImage.Style("padding-left") = "8px"
    '    sortImage.Style("padding-bottom") = "1px"
    '    If SortDirection = "ASC" Then
    '        sortImage.ImageUrl = "~/images/arrowUp.gif"
    '        sortImage.AlternateText = "Ascending Order"
    '    Else
    '        sortImage.ImageUrl = "~/images/arrowDown.gif"
    '        sortImage.AlternateText = "Descending Order"
    '    End If
    '    For i As Integer = 0 To GVPriceList.Columns.Count - 1
    '        Dim dcf As DataControlField = GVPriceList.Columns(i)
    '        If dcf.SortExpression = SortField Then
    '            If Not IsNothing(GVPriceList.HeaderRow) Then
    '                GVPriceList.HeaderRow.Cells(i).Controls.Add(sortImage)
    '            End If
    '            Exit For
    '        End If
    '    Next
    'End Sub

    'Private Sub GVPriceList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GVPriceList.PageIndexChanging
    '    GVPriceList.PageIndex = e.NewPageIndex
    '    BindData()
    'End Sub

    'Private Sub GVPriceList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GVPriceList.Sorting
    '    SortField = e.SortExpression
    '    SortDirection = "flip"
    '    BindData()
    'End Sub

    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        BindData()
    End Sub

    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        Dim SearchQuery As String = ""
        If Not (ddlOrganization.SelectedItem.Value = "0") Then
            ObjCommon = New Common()
            SearchQuery = "Where Organization_ID=" & ddlOrganization.SelectedItem.Value
            Dim UD_SUB_QRY As String = "SELECT AB.SalesRep_ID FROM TBL_Org_CTL_DTL As AB WHERE AB.Org_Id=102 "
            ddlType.DataSource = ObjCommon.GetPriceTypeList(Err_No, Err_Desc, SearchQuery, "")
            ddlType.DataBind()
            If ddlType.Items.IndexOf(ddlType.Items.FindByValue("-- Select a value --")) = -1 Then
                ddlType.Items.Insert(0, New ListItem("-- Select a value --", "0"))
            End If

            ddlAgency.DataValueField = "Agency"
            ddlAgency.DataTextField = "Agency"
            ddlAgency.DataSource = ObjCommon.GetAllAgencyByOrg(Err_No, Err_Desc, Convert.ToInt32(ddlOrganization.SelectedItem.Value))
            ddlAgency.DataBind()
            ddlAgency.Items.Insert(0, New ListItem("-- Select value --", "0"))


            drpProduct.DataValueField = "Item_Code"
            drpProduct.DataTextField = "Description"
            drpProduct.DataSource = ObjCommon.GetProductsByOrg_Agency_UOM(Err_No, Err_Desc, Convert.ToInt32(ddlOrganization.SelectedItem.Value), "0", ddlUOM.SelectedItem.Value)
            drpProduct.DataBind()
            drpProduct.Items.Insert(0, New ListItem("-- Select a value --", "0"))


            ObjCommon = Nothing

            RVMain.Reset()
        End If
    End Sub
    'Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    '    Return
    'End Sub


    'Protected Sub imgPrint_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgPrint.Click

    '    GVPriceList.AllowPaging = False
    '    Dim dt As New DataTable
    '    dt = ViewState("dt")
    '    GVPriceList.DataSource = dt
    '    GVPriceList.DataBind()
    '    Dim sw As New StringWriter()
    '    Dim hw As New HtmlTextWriter(sw)
    '    GVPriceList.RenderControl(hw)
    '    Dim gridHTML As String = sw.ToString().Replace("""", "'") _
    '       .Replace(System.Environment.NewLine, "")
    '    Dim sb As New StringBuilder()
    '    sb.Append("<script type = 'text/javascript'>")
    '    sb.Append("window.onload = new function(){")
    '    sb.Append("var printWin = window.open('', '', 'left=0")
    '    sb.Append(",top=0,width=1000,height=1000,status=0');")
    '    sb.Append("printWin.document.write(""")
    '    sb.Append(gridHTML)
    '    sb.Append(""");")
    '    sb.Append("printWin.document.close();")
    '    sb.Append("printWin.focus();")
    '    sb.Append("printWin.print();")
    '    sb.Append("printWin.close();};")
    '    sb.Append("</script>")
    '    ClientScript.RegisterStartupScript(Me.[GetType](), "GridPrint", sb.ToString())
    '    GVPriceList.AllowPaging = True
    '    GVPriceList.DataBind()

    'End Sub

    Private Sub ddlAgency_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAgency.SelectedIndexChanged
        ObjCommon = New Common()
        drpProduct.DataValueField = "Item_Code"
        drpProduct.DataTextField = "Description"
        drpProduct.DataSource = ObjCommon.GetProductsByOrg_Agency_UOM(Err_No, Err_Desc, Convert.ToInt32(ddlOrganization.SelectedItem.Value), ddlAgency.SelectedItem.Value, ddlUOM.SelectedItem.Value)
        drpProduct.DataBind()
        drpProduct.Items.Insert(0, New ListItem("-- Select a value --", "0"))
        RVMain.Reset()
    End Sub

    Private Sub ddlUOM_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUOM.SelectedIndexChanged
        ObjCommon = New Common()
        drpProduct.DataValueField = "Item_Code"
        drpProduct.DataTextField = "Description"
        drpProduct.DataSource = ObjCommon.GetProductsByOrg_Agency_UOM(Err_No, Err_Desc, Convert.ToInt32(ddlOrganization.SelectedItem.Value), ddlAgency.SelectedItem.Value, ddlUOM.SelectedItem.Value)
        drpProduct.DataBind()
        drpProduct.Items.Insert(0, New ListItem("-- Select a value --", "0"))
        RVMain.Reset()
    End Sub
End Class
