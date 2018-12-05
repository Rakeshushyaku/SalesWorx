Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Imports System.Linq

Partial Public Class Rep_VanSummary
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "LoadVsUnload"

    Private Const PageID As String = "P290"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single

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
                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                txtToDate.SelectedDate = Now()

                HUID.Value = CType(Session("User_Access"), UserAccess).UserID
                LoadorgDetails()



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
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If ddlOrganization.SelectedIndex > 0 Then
            If ddl_Van.SelectedItem.Value = "-1" Then
                MessageBoxValidation("Select a Van.", "Validation")
                Return bretval
            End If
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
            bretval = True
            Return bretval
        Else
            MessageBoxValidation("Please select the Organisation", "Validation")
            Return bretval
        End If
    End Function

    Private Sub ddl_item_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddl_item.ItemsRequested

        Dim strgency As String = ""

        strgency = "0"

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

            ddl_item.Items.Add(item)
            item.DataBind()
        Next

    End Sub
    Sub Export(format As String)

        ObjCommon = New SalesWorx.BO.Common.Common()
        Dim EndTime As String
        EndTime = ObjCommon.GetDayEndTime(Err_No, Err_Desc)

        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

        Dim fromdate As String
        Dim todate As String

        fromdate = DateAdd(DateInterval.Day, -1, CDate(txtFromDate.SelectedDate)).ToString("dd-MMM-yyyy") & " " & EndTime
        todate = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy") & " " & EndTime

        Dim SalesRepID As New ReportParameter
        SalesRepID = New ReportParameter("SID", ddl_Van.SelectedItem.Value)

        Dim FDate As New ReportParameter
        FDate = New ReportParameter("FromDate", fromdate)
        


        Dim TDate As New ReportParameter
        TDate = New ReportParameter("ToDate", todate)
         

        Dim OID As New ReportParameter
        OID = New ReportParameter("OrgID", CStr(ddlOrganization.SelectedValue.ToString()))

        Dim PER As New ReportParameter
        PER = New ReportParameter("Per", Val(txt_Per.Text))


        Dim Item As String

        If Not String.IsNullOrEmpty(ddl_item.SelectedValue) Then
            Item = ddl_item.SelectedValue
            lbl_SKU.Text = (New SalesWorx.BO.Common.Product).GetItemName(Err_No, Err_Desc, ddl_item.SelectedValue, ddlOrganization.SelectedItem.Value)
        Else
            Item = "0"
            lbl_SKU.Text = "All"

        End If


        Dim ItemID As New ReportParameter
        ItemID = New ReportParameter("ItemID", Item)

        rview.ServerReport.SetParameters(New ReportParameter() {OID, SalesRepID, FDate, TDate, ItemID, PER})

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
            Response.AddHeader("Content-disposition", "attachment;filename=VanSummary.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=VanSummary.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()

    End Sub

    Private Sub BindReport()

        ObjCommon = New SalesWorx.BO.Common.Common()
        Dim ClientCode As String
        ClientCode = (New SalesWorx.BO.Common.Common).GetAppConfig(Err_No, Err_Desc, "CLIENT_CODE")
        Dim EndTime As String
        If ClientCode = "GFC" Then

            EndTime = ObjCommon.GetDayEndTime(Err_No, Err_Desc)
        Else

            EndTime = " 23:59:59"
        End If


        rpbFilter.Items(0).Expanded = False
        Args.Visible = False

        Dim Item As String

        If Not String.IsNullOrEmpty(ddl_item.SelectedValue) Then
            Item = ddl_item.SelectedValue
            lbl_SKU.Text = (New SalesWorx.BO.Common.Product).GetItemName(Err_No, Err_Desc, ddl_item.SelectedValue, ddlOrganization.SelectedItem.Value)
        Else
            Item = "0"
            lbl_SKU.Text = "All"

        End If


        lbl_org.Text = ddlOrganization.SelectedItem.Text
        lbl_From.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
        lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")
        lbl_Van.Text = ddl_Van.SelectedItem.Text
        lbl_Unload.Text = Val(txt_Per.Text)


        HorgID.Value = ddlOrganization.SelectedItem.Value
        Hfrom.Value = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
        HTo.Value = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy") & " " & EndTime
        HVan.Value = ddl_Van.SelectedItem.Value

        Args.Visible = True
        Dim dt As New DataTable
        Dim objreport As New SalesWorx.BO.Common.Reports
        dt = objreport.GetVanSummary(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, ddl_Van.SelectedItem.Value, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy") & " " & EndTime, txt_Per.Text, Item)


        gvRep.DataSource = dt
        gvRep.DataBind()

        If ClientCode = "ASR" Then
            gvRep.Columns(2).Visible = False
            gvRep.Columns(4).Visible = False
            gvRep.Columns(6).Visible = False
            gvRep.Columns(8).Visible = False
            gvRep.Columns(10).Visible = False
        End If

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
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            Args.Visible = True
            gvRep.Visible = True

            BindReport()
        Else

            Args.Visible = False
            'summary.InnerHtml = ""
            gvRep.Visible = False

        End If
    End Sub
    
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        LoadOrgDetails()
    End Sub
    Sub LoadOrgDetails()
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        ObjCommon = New SalesWorx.BO.Common.Common()
        ddl_Van.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)

        ddl_Van.DataValueField = "SalesRep_ID"
        ddl_Van.DataTextField = "SalesRep_Name"
        ddl_Van.DataBind()
        ddl_Van.Items.Insert(0, New RadComboBoxItem("Select Van/FSR", "-1"))
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        ddl_Van.ClearSelection()
        ddl_Van.Items.Clear()
        LoadOrgDetails()
        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Now()
        txt_Per.Text = ""
        ddl_item.ClearSelection()
        ddl_item.Text = ""
        gvRep.Visible = False
        Args.Visible = False
    End Sub
End Class