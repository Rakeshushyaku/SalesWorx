Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Partial Public Class Rep_UnLoadREport
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer
    Private ReportPath As String = "VanUnloadReport"

    Private Const PageID As String = "P115"
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

                loadorgDetials()


                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1, Now)
                txtToDate.SelectedDate = Now()

                 

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
     
     
    Sub loadorgDetials()
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        ObjCommon = New SalesWorx.BO.Common.Common()
        ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())
        ddlVan.DataBind()
        'ddlVan.Items.Insert(0, New RadComboBoxItem("Select Van", "0"))
        For Each li As RadComboBoxItem In ddlVan.Items
            li.Checked = True
        Next

    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Private Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrganization.SelectedIndexChanged
        loadorgDetials()
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

    Private Sub Export(Format As String)
        Try



            Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
            rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)



            Dim item As String = "0"
            If Not String.IsNullOrEmpty(ddl_item.SelectedValue) Then
                item = ddl_item.SelectedValue
                lbl_SkU.Text = (New SalesWorx.BO.Common.Product).GetItemName(Err_No, Err_Desc, ddl_item.SelectedValue, ddlOrganization.SelectedItem.Value)
            Else
                item = "0"
                lbl_SkU.Text = "All"


            End If


            Dim van As String = ""
            For Each li As RadComboBoxItem In ddlVan.CheckedItems
                van = van & li.Value & ","

            Next

            If van = "" Then
                van = "0"
            End If


            Dim SID As New ReportParameter
            SID = New ReportParameter("SID", van)

            Dim FromDate As New ReportParameter
            FromDate = New ReportParameter("FromDate", CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"))

            Dim ToDate As New ReportParameter
            ToDate = New ReportParameter("ToDate", CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))

            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("OID", CStr(ddlOrganization.SelectedItem.Value.ToString()))


            Dim itemid As New ReportParameter
            itemid = New ReportParameter("itemid", item)


            rview.ServerReport.SetParameters(New ReportParameter() {OrgId, SID, FromDate, ToDate, itemid})

            Dim mimeType As String = Nothing
            Dim encoding As String = Nothing
            Dim extension As String = Nothing
            Dim deviceInfo As String = "<DeviceInfo></DeviceInfo>"
            Dim streamids As String() = Nothing
            Dim warnings As Microsoft.Reporting.WebForms.Warning() = Nothing

            Dim bytes As Byte() = rview.ServerReport.Render(Format, deviceInfo, mimeType, encoding, extension, streamids, warnings)


            Response.Clear()
            If Format = "PDF" Then
                Response.ContentType = "application/pdf"
                Response.AddHeader("Content-disposition", "attachment;filename=VanUnLoad.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf Format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=VanUnLoad.xls")
                Response.AddHeader("Content-Length", bytes.Length)
            End If
            Response.OutputStream.Write(bytes, 0, bytes.Length)
            Response.OutputStream.Flush()
            Response.OutputStream.Close()
            Response.Flush()
            Response.Close()
        Catch Ex As Exception
            log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub

    Private Function ValidateInputs() As Boolean
        Dim SearchQuery As String = ""
        Try
            Dim bretval As Boolean = False

            If ddlOrganization.SelectedIndex > 0 Then
                If ddlVan.CheckedItems.Count > 0 Then

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
                    MessageBoxValidation("Select a van.", "Validation")
                    Return bretval

                End If
            Else
                MessageBoxValidation("Select an Organization.", "Validation")
                Return bretval
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
            ObjCustomer = Nothing
        End Try
    End Function
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        Dim SHOW_UOM_MSG_BO_REPORTS As String = "N"
        Dim dt_app As New DataTable
        dt_app = (New SalesWorx.BO.Common.Common).GetAppControl(Err_No, Err_Desc, "SHOW_UOM_MSG_BO_REPORTS")
        If dt_app.Rows.Count > 0 Then
            SHOW_UOM_MSG_BO_REPORTS = dt_app.Rows(0)("Control_Value").ToString().ToUpper()
            If SHOW_UOM_MSG_BO_REPORTS = "Y" Then
                lblmsgUOM.Text = "All the quantities displayed in this report are in Stock UOM"
            Else
                lblmsgUOM.Text = ""

            End If
        End If

        If ValidateInputs() Then
            gvRep.Visible = True
            BindReport()


        Else
            Args.Visible = False

            gvRep.Visible = False

        End If
    End Sub
    Private Sub BindReport()
        Try
            Dim SearchQuery As String = ""
            If Not (ddlOrganization.SelectedItem.Value = "0") Then

            Else
                MessageBoxValidation("Select an organization.", "Validation")
                Exit Sub
            End If

            rpbFilter.Items(0).Expanded = False
            Args.Visible = False

            Dim item As String = "0"
            If Not String.IsNullOrEmpty(ddl_item.SelectedValue) Then
                item = ddl_item.SelectedValue
                lbl_SkU.Text = (New SalesWorx.BO.Common.Product).GetItemName(Err_No, Err_Desc, ddl_item.SelectedValue, ddlOrganization.SelectedItem.Value)
            Else
                item = "0"
                lbl_SkU.Text = "All"
            End If

            Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems

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
            lbl_van.Text = vantxt

            lbl_van.Text = vantxt
            lbl_From.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")

            Args.Visible = True

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetVanUnload(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, van, item, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))
            gvRep.DataSource = dt
            gvRep.DataBind()
            Dim ClientCode As String = ""
            ClientCode = (New SalesWorx.BO.Common.Common).GetAppConfig(Err_No, Err_Desc, "CLIENT_CODE")
            If ClientCode.ToUpper = "AMT" Then
                gvRep.MasterTableView.Columns.FindByUniqueName("Lot_Number").Visible = True
            Else
                gvRep.MasterTableView.Columns.FindByUniqueName("Lot_Number").Visible = False
            End If

        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub

    Private Sub ddl_item_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddl_item.ItemsRequested

        Dim strgency As String = "0"

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
    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        ddl_item.ClearCheckedItems()
        loadorgDetials()

        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1, Now)
        txtToDate.SelectedDate = Now()
        Args.Visible = False
        lblmsgUOM.Text = ""
        gvRep.Visible = False
    End Sub
End Class