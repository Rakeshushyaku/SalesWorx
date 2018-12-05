Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI

Partial Public Class ProductAvailabilityReport
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Private Const PageID As String = "P107"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private ReportPath As String = "ProductAvailabilityReport"
    
    Public Sub SetReportDetails(ByVal path As String)
        Me.ReportPath = path
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
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))

                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                End If

                LoadOrgDetails()

                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                txtToDate.SelectedDate = Now()

                If Not Err_Desc Is Nothing Then
                    log.Error(Err_Desc)
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
                End If
            Catch ex As Exception
                Err_No = "74099"
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
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            gvRep.Visible = True
            BindReport()
            rptsect.Visible = True
            Bindchart()
        Else
            Args.Visible = False
            gvRep.Visible = False
            rptsect.Visible = False
        End If
       
    End Sub
    Sub BindReport()
        Try
            Dim SearchQuery As String = ""
             

            rpbFilter.Items(0).Expanded = False
            Args.Visible = False

            Dim collection As IList(Of RadComboBoxItem) = ddl_Van.CheckedItems

            Dim vantxt As String = ""
            Dim van As String = ""
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","
                vantxt = vantxt & li.Text & ","
            Next
            If vantxt <> "" Then
                vantxt = vantxt.Substring(0, vantxt.Length - 1)
            End If
            If van = "" Then
                van = "0"
            End If

            lbl_org.Text = ddlOrganization.SelectedItem.Text
            If van = "0" Then
                lbl_van.Text = "All"
            Else
                lbl_van.Text = vantxt
            End If

            If Not String.IsNullOrEmpty(drpProduct.SelectedValue) Then
                HItem.Value = drpProduct.SelectedValue
                lbl_SkU.Text = (New SalesWorx.BO.Common.Product).GetItemName(Err_No, Err_Desc, drpProduct.SelectedValue, ddlOrganization.SelectedItem.Value)
            Else
                HItem.Value = "0"
                lbl_SkU.Text = "All"
            End If

            lbl_fromDate.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_Todate.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")
            hfOrg.Value = ddlOrganization.SelectedItem.Value
            HFrom.Value = CDate(txtFromDate.SelectedDate).ToString("MM-dd-yyyy")
            Hto.Value = CDate(txtToDate.SelectedDate).ToString("MM-dd-yyyy")
            hfVan.Value = van
            If ddlAgency.SelectedItem.Value = "0" Then
                lbl_agency.Text = "All"
            Else
                lbl_agency.Text = ddlAgency.SelectedItem.Value
            End If
            HAgency.Value = ddlAgency.SelectedItem.Value
            HType.Value = ddlAvail.SelectedItem.Value
            UId.Value = CType(Session.Item("USER_ACCESS"), UserAccess).UserID
            lbl_Type.Text = ddlAvail.SelectedItem.Text



            Args.Visible = True

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetProductAvailablity(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, van, objUserAccess.UserID, ddlAgency.SelectedItem.Value, Hitem.Value, CDate(txtFromDate.SelectedDate).ToString("MM/dd/yyyy"), CDate(txtToDate.SelectedDate).ToString("MM/dd/yyyy"), ddlAvail.SelectedItem.Value)


            Dim fromdate As DateTime

            Dim FinalDt As New DataTable
            FinalDt.Columns.Add("Product")
            FinalDt.Columns.Add("Type")
            FinalDt.Columns.Add("Year")
            FinalDt.Columns.Add("C1", Type.GetType("System.Int32"))

            Dim tfromdate As DateTime
            tfromdate = CDate(txtFromDate.SelectedDate)

            Dim todate As DateTime
            todate = CDate(txtToDate.SelectedDate)
            fromdate = CDate(txtFromDate.SelectedDate)

            'While tfromdate <= todate
            '    FinalDt.Columns.Add(tfromdate.ToString("dd-MMM"))
            '    tfromdate = DateAdd(DateInterval.Day, 1, tfromdate)
            'End While

            Dim query
            query = (From UserEntry In dt _
                           Group UserEntry By key = UserEntry.Field(Of String)("Item_Code") Into Group _
                           Select Item_Code = key, Total = Group.Sum(Function(p) p.Field(Of Integer)("cnt")) Order By Total Descending).ToList

            Dim queryDate
            queryDate = (From UserEntry In dt _
                           Group UserEntry By key = UserEntry.Field(Of String)("Checked_On") Into Group _
                           Select Checked_On = key, Total = Group.Sum(Function(p) p.Field(Of Integer)("cnt")) Order By Checked_On Ascending).ToList

            Dim c As Integer = 1
            For Each x In query
                '    Dim dr As DataRow
                '    dr = FinalDt.NewRow

                '    Dim seldr1() As DataRow
                '    seldr1 = dt.Select("Item_Code='" & x.Item_Code & "'")
                '    If seldr1.Length > 0 Then
                '        dr("Product") = x.Item_Code + " -" + seldr1(0)("Description").ToString
                '    End If

                '    tfromdate = fromdate
                '    While tfromdate <= todate
                '        Dim seldr() As DataRow
                '        seldr = dt.Select("Item_Code='" & x.Item_Code & "' and Checked_On='" & tfromdate.ToString("MM/dd/yyyy") & "'")
                '        If seldr.Length > 0 Then
                '            dr(tfromdate.ToString("dd-MMM")) = seldr(0)("cnt")
                '        End If
                '        tfromdate = DateAdd(DateInterval.Day, 1, tfromdate)
                '    End While
                '    FinalDt.Rows.Add(dr)
                c = c + 1
            Next

            If c > 10 Then
                Hdesc.Value = "Product availablity - " & ddlAvail.SelectedItem.Text & " -Top 5 products"
            Else
                Hdesc.Value = "Product availablity - " & ddlAvail.SelectedItem.Text
            End If

            For Each x In query
                
                'tfromdate = fromdate
                'While tfromdate <= todate
                For Each y In queryDate
                    Dim drfinal As DataRow
                    drfinal = FinalDt.NewRow

                    Dim seldr1() As DataRow
                    seldr1 = dt.Select("Item_Code='" & x.Item_Code & "'")
                    If seldr1.Length > 0 Then
                        drfinal("Product") = x.Item_Code + " -" + seldr1(0)("Description").ToString
                    End If
                    drfinal("Type") = "No of Customers"
                    drfinal("Year") = CDate(y.Checked_On).ToString("d-MMM")
                    Dim seldr() As DataRow
                    seldr = dt.Select("Item_Code='" & x.Item_Code & "' and Checked_On='" & CDate(y.Checked_On).ToString("MM/dd/yyyy") & "'")
                    If seldr.Length > 0 Then
                        drfinal("C1") = seldr(0)("Cnt")
                    End If
                    'tfromdate = DateAdd(DateInterval.Day, 1, tfromdate)
                    FinalDt.Rows.Add(drfinal)
                Next
                ' End While

            Next

            gvRep.DataSource = FinalDt
            gvRep.DataBind()


        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub

    Private Sub gvRep_CellDataBound(sender As Object, e As PivotGridCellDataBoundEventArgs) Handles gvRep.CellDataBound
        If TypeOf e.Cell Is PivotGridColumnHeaderCell Then
            If e.Cell.Text.ToLower.IndexOf("no of customers total") >= 0 Then
                e.Cell.Text = "Total <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Total Number of customers at which product is available on Entry/Exit or Invoiced'></i>"
            End If
            If e.Cell.Text = "No of Customers" Then
                e.Cell.Text = "No of Customers <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Number of customers at which product is available on Entry/Exit or Invoiced'></i>"
            End If
        End If
        If TypeOf e.Cell Is PivotGridDataCell Then
            Dim cell As PivotGridDataCell = TryCast(e.Cell, PivotGridDataCell)
            If cell.CellType = PivotGridDataCellType.DataCell OrElse cell.CellType = PivotGridDataCellType.RowTotalDataCell Then
                If e.Cell.Text = "0" Then
                    e.Cell.Text = ""
                End If
            End If
        End If
    End Sub

    'Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender
    '    For Each column As GridColumn In gvRep.MasterTableView.AutoGeneratedColumns
    '        If column.UniqueName <> "Product" Then
    '            column.ItemStyle.HorizontalAlign = HorizontalAlign.Right

    '        End If
    '    Next
    'End Sub
    'Private Sub dgv_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvRep.SortCommand
    '    ViewState("SortField") = e.SortExpression
    '    SortDirection = "flip"
    '    BindReport()
    'End Sub
    'Private Sub dgv_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged

    '    BindReport()
    'End Sub
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
    Private Sub gvRep_PageIndexChanged(sender As Object, e As PivotGridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        BindReport()
    End Sub

    Private Sub gvRep_PageSizeChanged(sender As Object, e As PivotGridPageSizeChangedEventArgs) Handles gvRep.PageSizeChanged
        BindReport()
    End Sub
    Sub Bindchart()
        ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
    End Sub
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If ddlOrganization.SelectedIndex > 0 Then
            Dim TemFromDateStr As String = CDate(txtFromDate.SelectedDate).ToString("dd/MM/yyyy")
            Dim DateArr As Array = TemFromDateStr.Split("/")
            If DateArr.Length = 3 Then
                TemFromDateStr = DateArr(1) & "/" & DateArr(0) & "/" & DateArr(2)
            End If
            Dim TemToDateStr As String = CDate(txtToDate.SelectedDate).ToString("dd/MM/yyyy")
            Dim DateArr1 As Array = TemToDateStr.Split("/")
            If DateArr1.Length = 3 Then
                TemToDateStr = DateArr1(1) & "/" & DateArr1(0) & "/" & DateArr1(2)
            End If

            If Not IsDate(TemFromDateStr) Then
                MessageBoxValidation("Enter valid ""From date"".", "Validation")
                SetFocus(txtFromDate)
                Return bretval
            End If

            If Not IsDate(TemToDateStr) Then
                MessageBoxValidation("Enter valid ""To date"".", "Validation")
                SetFocus(TemToDateStr)
                Return bretval
            End If

            If CDate(TemFromDateStr) > CDate(TemToDateStr) Then
                MessageBoxValidation("Start Date should not be greater than End Date.", "Validation")
                Return bretval
            End If
            If Math.Abs(DateDiff(DateInterval.Day, CDate(TemFromDateStr), CDate(TemToDateStr))) > 31 Then
                MessageBoxValidation("Please select a date range of 1 month.", "Validation")
                Return bretval
            End If
            bretval = True
            Return bretval
        Else
            MessageBoxValidation("Please select the Organisation", "Validation")
            Return bretval
        End If
    End Function
    Sub Export(format As String)

        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)

        Dim myParamOrg As New ReportParameter
        Dim myParamVan As New ReportParameter
        Dim myParamAgency As New ReportParameter
        Dim myParamProduct As New ReportParameter
        Dim myParamFromDate As New ReportParameter
        Dim myParamToDate As New ReportParameter
        Dim myParamAvail As New ReportParameter
        Dim myParamUid As New ReportParameter

        Dim collection As IList(Of RadComboBoxItem) = ddl_Van.CheckedItems


        Dim van As String = ""
        For Each li As RadComboBoxItem In collection
            van = van & li.Value & ","

        Next
        
        If van = "" Then
            van = "0"
        End If

        If ddlOrganization.SelectedIndex > 0 Then
            myParamOrg = New ReportParameter("OID", ddlOrganization.SelectedValue)
        Else
            myParamOrg = New ReportParameter("OID", "0")
        End If

        myParamVan = New ReportParameter("SID", van)
         
        If ddlAgency.SelectedIndex > 0 Then
            myParamAgency = New ReportParameter("Agency", ddlAgency.SelectedValue)
        Else
            myParamAgency = New ReportParameter("Agency", "0")
        End If


        If Not String.IsNullOrEmpty(drpProduct.SelectedValue) Then
            myParamProduct = New ReportParameter("InvID", drpProduct.SelectedValue)
        Else
            myParamProduct = New ReportParameter("InvID", 0)
        End If

        

        Dim FDate As New ReportParameter
        FDate = New ReportParameter("FromDate", CDate(txtFromDate.SelectedDate).ToString("MM/dd/yyyy"))

        Dim TDate As New ReportParameter
        TDate = New ReportParameter("Todate", CDate(txtToDate.SelectedDate).ToString("MM/dd/yyyy"))

        myParamAvail = New ReportParameter("Availability", ddlAvail.SelectedValue)

        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

        If objUserAccess.UserID > 0 Then
            myParamUid = New ReportParameter("Uid", objUserAccess.UserID)
        Else
            myParamUid = New ReportParameter("Uid", "0")
        End If


        rview.ServerReport.SetParameters(New ReportParameter() {myParamOrg, myParamVan, myParamAgency, myParamProduct, FDate, TDate, myParamAvail, myParamUid})

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
            Response.AddHeader("Content-disposition", "attachment;filename=ProductAvailabilty.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=ProductAvailabilty.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()

    End Sub
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
     Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        rptsect.Visible = False
        LoadOrgDetails()
    End Sub
    Sub LoadOrgDetails()
        If Not (ddlOrganization.SelectedItem.Value = "0") Then
            ObjCommon = New SalesWorx.BO.Common.Common()
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

            ddl_Van.DataTextField = "SalesRep_Name"
            ddl_Van.DataValueField = "SalesRep_ID"
            ddl_Van.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)

            ddl_Van.DataBind()

            For Each itm As RadComboBoxItem In ddl_Van.Items
                itm.Checked = True
            Next

            LoadAgency()

        Else
            ddl_Van.Items.Clear()
            ddlAgency.Items.Clear()
            ddlAgency.Items.Insert(0, New RadComboBoxItem("Select Agency", "0"))
        End If
    End Sub
     
    Private Sub ddl_item_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles drpProduct.ItemsRequested

        Dim strgency As String = ""
        strgency = strgency & ddlAgency.SelectedItem.Value
             
        If strgency = "" Then
            strgency = "0"
        End If
        Dim Objrep As New SalesWorx.BO.Common.Reports()
        Dim dt As New DataTable
        dt = Objrep.GetItemFromAgency(Err_No, Err_Desc, ddlOrganization.SelectedValue, strgency, e.Text)


        Dim ItemsPerRequest As Integer = 100 'Set the number of values to show
        Dim itemOffset As Integer = e.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
        e.EndOfItems = endOffset = dt.Rows.Count

        'Loop through the values to populate the combo box
        For i As Integer = itemOffset To endOffset - 1
            Dim item As New RadComboBoxItem()
            item.Text = dt.Rows(i).Item("Description").ToString
            item.Value = dt.Rows(i).Item("Inventory_Item_ID").ToString

            drpProduct.Items.Add(item)
            item.DataBind()
        Next

    End Sub

    Sub LoadAgency()
        ddlAgency.Items.Clear()
        ObjCommon = New SalesWorx.BO.Common.Common()
        If Not (ddlOrganization.SelectedItem.Value = "0") Then
            ddlAgency.DataValueField = "Agency"
            ddlAgency.DataTextField = "Agency"
            If hfVanValue.Value = "" Then
                ddlAgency.DataSource = ObjCommon.GetAllAgencyByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue)
            Else
                ddlAgency.DataSource = ObjCommon.GetAllAgencyByOrg_Van(Err_No, Err_Desc, ddlOrganization.SelectedValue, hfVanValue.Value)

            End If
            ddlAgency.DataBind()
            ddlAgency.Items.Insert(0, New RadComboBoxItem("Select Agency", "0"))
        End If
    End Sub
      
    Private Sub ddlAgency_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlAgency.SelectedIndexChanged
        drpProduct.SelectedValue = 0
        drpProduct.Text = ""
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        ddl_Van.ClearCheckedItems()
        ddl_Van.Items.Clear()
        LoadOrgDetails()
        ddlAgency.ClearSelection()
        ddlAvail.ClearSelection()
        drpProduct.ClearSelection()
        drpProduct.Text = ""
        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Now()
        Args.Visible = False
        gvRep.Visible = False
        rptsect.Visible = False
    End Sub
End Class