Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Partial Public Class RepPriceListing
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjPrice As Price
    Private ReportPath As String = "PriceListing"
    Private Const PageID As String = "P200"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

     

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
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)

                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))

                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                End If


                ddlUOM.DataValueField = "Item_UOM"
                ddlUOM.DataTextField = "Item_UOM"
                ddlUOM.DataSource = ObjCommon.GetAllUOM(Err_No, Err_Desc)
                ddlUOM.DataBind()
                ddlUOM.Items.Insert(0, New RadComboBoxItem("Select UOM", "0"))

                LoadOrgdetails()
                ObjCommon = Nothing
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
     Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Private Function BuildQuery() As String
        Dim SearchQuery As String = ""
        Try
            ObjPrice = New Price()
            If Not (ddlOrganization.SelectedItem.Value = "0") Then
                SearchQuery = SearchQuery & " And A.Organization_ID=" & ddlOrganization.SelectedItem.Value

                If ddlType.SelectedValue <> "0" Then
                    SearchQuery = SearchQuery & " And A.Price_List_ID=" & ddlType.SelectedValue
                End If

                If Not String.IsNullOrEmpty(ddl_item.SelectedValue) Then
                    SearchQuery = SearchQuery & " And P.Item_Code ='" & ddl_item.SelectedValue & "'"
                End If

                If ddlUOM.SelectedValue <> "0" Then
                    SearchQuery = SearchQuery & " And A.Item_UOM ='" & ddlUOM.SelectedValue & "'"
                End If
                If ddlAgency.SelectedValue <> "0" Then
                    SearchQuery = SearchQuery & " And P.Agency ='" & ddlAgency.SelectedValue & "'"
                End If

            End If
        Catch ex As Exception
            log.Debug(ex.ToString)
        Finally
            ObjPrice = Nothing
        End Try
        Return SearchQuery
    End Function

    Sub Export(format As String)


        Dim FilterValue As String = ""
        FilterValue = BuildQuery()

        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        Dim Searchvalue As New ReportParameter
        Searchvalue = New ReportParameter("SearchParams", CStr(IIf(FilterValue Is Nothing, "", FilterValue)))

        Dim Org As New ReportParameter
        Org = New ReportParameter("Org_ID", CStr(IIf(ddlOrganization.SelectedItem.Value = "0", "All", ddlOrganization.SelectedItem.Value)))

        Dim Agency As New ReportParameter
        Agency = New ReportParameter("Agency", CStr(IIf(ddlAgency.SelectedItem.Value = "0", "All", ddlAgency.SelectedItem.Text)))

        Dim UOM As New ReportParameter
        UOM = New ReportParameter("UOM", CStr(IIf(ddlUOM.SelectedItem.Value = "0", "All", ddlUOM.SelectedItem.Text)))

        Dim PriceList As New ReportParameter
        PriceList = New ReportParameter("PriceList", CStr(IIf(ddlType.SelectedItem.Value = "0", "All", ddlType.SelectedItem.Text)))

        Dim PodItem As New ReportParameter
        PodItem = New ReportParameter("ProdName", ddl_item.Text)

        rview.ServerReport.SetParameters(New ReportParameter() {Searchvalue, Org, Agency, UOM, PriceList, PodItem})

        Dim mimeType As String = Nothing
        Dim encoding As String = Nothing
        Dim extension As String = Nothing
        Dim deviceInfo As String = "<DeviceInfo></DeviceInfo>"
        Dim streamids As String() = Nothing
        Dim warnings As Microsoft.Reporting.WebForms.Warning() = Nothing

        Dim bytes As Byte() = rview.ServerReport.Render(format, deviceInfo, mimeType, encoding, extension, streamids, warnings)


        Response.Clear()
        If format = "PDF" Then
            Response.ContentType = "application/pdf"
            Response.AddHeader("Content-disposition", "attachment;filename=PriceListing.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=PriceListing.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()

    End Sub
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If ddlOrganization.SelectedIndex > 0 Then
            bretval = True
            Return bretval
        Else
            MessageBoxValidation("Please select the Organization", "Validation")
            Return bretval
        End If
    End Function
    Private Sub BtnExport_Click(sender As Object, e As EventArgs) Handles BtnExportExcel.Click
        Try
            If ValidateInputs() Then
                Export("Excel")
            End If
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub
    Private Sub BtnPDF_Click(sender As Object, e As EventArgs) Handles BtnExportPDF.Click
        Try
            If ValidateInputs() Then
                Export("PDF")
            End If
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            RepDiv.Visible = True
            gvRep.Visible = True
            BindReport()
            divCurrency.Visible = True

        Else
            RepDiv.Visible = False
            Args.Visible = False
            gvRep.Visible = False
            divCurrency.Visible = False
        End If
    End Sub
    Private Sub ddl_item_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddl_item.ItemsRequested

        If ddlOrganization.SelectedIndex > 0 Then
            Dim strgency As String = ""
            strgency = ddlAgency.SelectedItem.Value

            If strgency = "" Then
                strgency = "0"
            End If
            Dim Objrep As New SalesWorx.BO.Common.Reports()
            Dim dt As New DataTable
            dt = Objrep.GetItemFromAgencyAndUOM(Err_No, Err_Desc, ddlOrganization.SelectedValue, strgency, ddlUOM.SelectedItem.Value, e.Text)


            Dim ItemsPerRequest As Integer = 100 'Set the number of values to show
            Dim itemOffset As Integer = e.NumberOfItems
            Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
            e.EndOfItems = endOffset = dt.Rows.Count

            'Loop through the values to populate the combo box
            For i As Integer = itemOffset To endOffset - 1
                Dim item As New RadComboBoxItem()
                item.Text = dt.Rows(i).Item("Description").ToString
                item.Value = dt.Rows(i).Item("itemNo").ToString

                ddl_item.Items.Add(item)
                item.DataBind()
            Next
        End If
    End Sub

    Private Sub BindReport()
        Try
            Dim SearchQuery As String = ""
            If Not (ddlOrganization.SelectedItem.Value = "0") Then
                SearchQuery = BuildQuery()
            Else
                MessageBoxValidation("Select an organization.", "Validation")
                Exit Sub
            End If

            rpbFilter.Items(0).Expanded = False

            If ddlUOM.SelectedItem.Value = "0" Then
                lbl_UOM.Text = "All"
            Else
                lbl_UOM.Text = ddlUOM.SelectedItem.Text
            End If

            If ddlAgency.SelectedItem.Value = "0" Then
                lbl_agency.Text = "All"
            Else
                lbl_agency.Text = ddlAgency.SelectedItem.Text
            End If

            lbl_org.Text = ddlOrganization.SelectedItem.Text
            Dim Item As String = "0"
            If Not String.IsNullOrEmpty(ddl_item.SelectedValue) Then
                Item = ddl_item.SelectedValue
                lbl_Product.Text = (New SalesWorx.BO.Common.Product).GetItemNameFromCode(Err_No, Err_Desc, ddl_item.SelectedValue, ddlOrganization.SelectedItem.Value)
            Else
                Item = "0"
                lbl_Product.Text = "All"
            End If

            If ddlType.SelectedItem.Value = "0" Then
                lbl_PriceList.Text = "All"
            Else
                lbl_PriceList.Text = ddlType.SelectedItem.Text
            End If


            Args.Visible = True

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetPriceListing(Err_No, Err_Desc, SearchQuery)
            gvRep.DataSource = dt
            gvRep.DataBind()
            Dim dtcurrency As DataTable

            dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

            Dim Currency As String = ""
            Dim lblDecimal As String = ""
            If dtcurrency.Rows.Count > 0 Then
                Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
                lblDecimal = "N" & dtcurrency.Rows(0)("Decimal_Digits")
            End If
            lbl_Currency.Text = Currency
            divCurrency.Visible = True

        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub
    Private Sub dgv_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvRep.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindReport()
    End Sub
    Private Sub dgv_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged

        BindReport()
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
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        LoadOrgdetails()
    End Sub

    Sub LoadOrgdetails()
        Dim SearchQuery As String = ""
        If Not (ddlOrganization.SelectedItem.Value = "0") Then
            ObjCommon = New SalesWorx.BO.Common.Common()
            SearchQuery = "Where Organization_ID=" & ddlOrganization.SelectedItem.Value
            Dim UD_SUB_QRY As String = "SELECT AB.SalesRep_ID FROM TBL_Org_CTL_DTL As AB WHERE AB.Org_Id=102 "

            ddlType.DataSource = ObjCommon.GetPriceTypeList(Err_No, Err_Desc, SearchQuery, "")
            ddlType.DataTextField = "Price_List_Type"
            ddlType.DataValueField = "Price_List_ID"
            ddlType.DataBind()
            If ddlType.Items.IndexOf(ddlType.FindItemByValue("Select Price List")) = -1 Then
                ddlType.Items.Insert(0, New RadComboBoxItem("Select Price List", "0"))
            End If

            ddlAgency.DataValueField = "Agency"
            ddlAgency.DataTextField = "Agency"
            ddlAgency.DataSource = ObjCommon.GetAllAgencyByOrg(Err_No, Err_Desc, Convert.ToInt32(ddlOrganization.SelectedItem.Value))
            ddlAgency.DataBind()
            ddlAgency.Items.Insert(0, New RadComboBoxItem("Select Agency", "0"))

            Dim dtcurrency As DataTable
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

            Dim Currency As String = ""
            If dtcurrency.Rows.Count > 0 Then
                Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
                lblDecimal.Text = "N" & dtcurrency.Rows(0)("Decimal_Digits")
            End If

            ObjCommon = Nothing


        End If
    End Sub

    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender
        Dim dtcurrency As DataTable
        Dim ObjReport As New SalesWorx.BO.Common.Reports
        dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

        Dim Currency As String = ""
        Dim lblDecimal As String = ""
        If dtcurrency.Rows.Count > 0 Then
            Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
            lblDecimal = "N" & dtcurrency.Rows(0)("Decimal_Digits")
        End If
        For Each column As GridColumn In gvRep.MasterTableView.Columns
            If column.UniqueName = "Unit_Selling_Price" Then
                Dim col As GridBoundColumn
                col = (CType(column, GridBoundColumn))
                col.DataType = System.Type.GetType("System.Double")
                col.DataFormatString = "{0:" & lblDecimal & "}"
            End If
        Next
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

    

    
    Private Sub ddlAgency_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlAgency.SelectedIndexChanged
        ddl_item.SelectedIndex = 0
        ddl_item.Text = ""
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        LoadOrgdetails()
        ddlAgency.ClearSelection()
        ddlType.ClearSelection()
        ddlUOM.ClearSelection()
        ddl_item.ClearSelection()
        ddl_item.Text = ""
        gvRep.Visible = False
        Args.Visible = False
        RepDiv.Visible = False
    End Sub
End Class
