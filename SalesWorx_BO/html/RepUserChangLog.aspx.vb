Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI

Partial Public Class RepUserChangLog
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer
    Private ReportPath As String = "UserLogReport"
    Dim dv As New DataView
    Private Const PageID As String = "P238"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objUser As New User
    Dim objLogin As New SalesWorx.BO.Common.Login

   

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        If Not IsPostBack Then
            Session("OrdersDT") = Nothing
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

                ddlVan.Items.Insert(0, New RadComboBoxItem("Select Van", "0"))

                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                txtToDate.SelectedDate = Now()

                FillModule()
                FillSubModule()
                FillUsers()
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
   
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
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
    Private Sub BindReport()
        Try
            Dim SearchQuery As String = BuildQuery()


            rpbFilter.Items(0).Expanded = False
            If ddlOrganization.SelectedItem.Value = "0" Then
                lbl_org.Text = "All"
            Else
                lbl_org.Text = ddlOrganization.SelectedItem.Text
            End If

            If ddlVan.SelectedItem.Value = "0" Then
                lbl_van.Text = "All"
            Else
                lbl_van.Text = ddlVan.SelectedItem.Text
            End If
            If ddlUser.SelectedItem.Value = "0" Then
                lbl_user.Text = "All"
            Else
                lbl_user.Text = ddlUser.SelectedItem.Text
            End If

            If ddlUser.SelectedItem.Value = "0" Then
                lbl_user.Text = "All"
            Else
                lbl_user.Text = ddlUser.SelectedItem.Text
            End If

            If ddlOrderType.SelectedItem.Value = "0" Then
                lbl_Type.Text = "All"
            Else
                lbl_Type.Text = ddlOrderType.SelectedItem.Text
            End If
            If ddlModule.SelectedIndex = 0 Then
                lbl_Module.Text = "All"
            Else
                lbl_Module.Text = ddlModule.SelectedItem.Text
            End If

            If ddlSubModule.SelectedIndex = 0 Then
                lbl_Submodule.Text = "All"
            Else
                lbl_Submodule.Text = ddlSubModule.SelectedItem.Text
            End If

            lbl_key.Text = txtKeyValue.Text
            lbl_From.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")

            Args.Visible = True

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetUserChangeLog(Err_No, Err_Desc, SearchQuery)

            gvRep.DataSource = dt
            gvRep.DataBind()

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
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Function BuildQuery() As String
        Dim SearchQuery As String = ""
        Try
            ObjCustomer = New Customer()
            ObjCommon = New SalesWorx.BO.Common.Common()

            If ddlVan.SelectedIndex > 0 Then
                Dim VanID As String = ""
                'Dim S As String() = ddlVan.SelectedItem.Text.Split("-")
                'VanID = IIf(S.Length > 1, S(1), "")
                Dim S As String = ddlVan.SelectedItem.Value
                VanID = S
                '' '' SearchQuery = " And A.Van='" & VanID.Trim() & "'"
                SearchQuery = " And (A.Van='" & VanID.Trim() & "' OR A.Van='" & ddlVan.SelectedItem.Text.Trim & "')"
            End If

            If ddlModule.SelectedIndex > 0 Then
                SearchQuery = SearchQuery & " And A.Module='" & ddlModule.SelectedItem.Text.ToUpper() & "'"
            End If
            If ddlSubModule.SelectedIndex > 0 Then
                SearchQuery = SearchQuery & " And A.Sub_Module='" & ddlSubModule.SelectedItem.Text.ToUpper() & "'"
            End If

            If ddlUser.SelectedIndex > 0 Then
                SearchQuery = SearchQuery & " And A.Logged_By='" & ddlUser.SelectedValue.ToString() & "'"
            End If


            If Me.txtKeyValue.Text <> "" Then
                SearchQuery = SearchQuery & " And A.Key_Value  LIKE '%" & Me.txtKeyValue.Text & "%'"
            End If


            If ddlOrderType.SelectedValue = "Inserted" Then
                SearchQuery = SearchQuery & " AND A.Tran_Type='I'"
            ElseIf ddlOrderType.SelectedValue = "Updated" Then
                SearchQuery = SearchQuery & " AND A.Tran_Type='U'"
            ElseIf ddlOrderType.SelectedValue = "Deleted" Then
                SearchQuery = SearchQuery & " AND A.Tran_Type='D'"
            ElseIf ddlOrderType.SelectedValue = "Approved" Then
                SearchQuery = SearchQuery & " AND A.Tran_Type='A'"
            ElseIf ddlOrderType.SelectedValue = "Login" Then
                SearchQuery = SearchQuery & " AND A.Tran_Type='L'"
            ElseIf ddlOrderType.SelectedValue = "Logout" Then
                SearchQuery = SearchQuery & " AND A.Tran_Type='O'"
            End If


            Dim TemFromDateStr As String = CDate(txtFromDate.SelectedDate)
             
            SearchQuery = SearchQuery & " And A.Logged_At >= '" & TemFromDateStr & "'"

            Dim TemToDateStr As String = CDate(txtToDate.SelectedDate)
            
            SearchQuery = SearchQuery & " And A.Logged_At <= '" & TemToDateStr & " 23:59:59'"


            If ddlOrganization.SelectedIndex > 0 Then
                SearchQuery = SearchQuery & " And A.Sales_Org ='" & ddlOrganization.SelectedItem.Value & "'"
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
        Return SearchQuery
    End Function
    Sub Export(format As String)
        Dim FilterValue As String
        FilterValue = BuildQuery()

        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)

        Dim Searchvalue As New ReportParameter
        Searchvalue = New ReportParameter("SearchParams", CStr(IIf(FilterValue Is Nothing, "", FilterValue)))


        Dim FDate As New ReportParameter
        FDate = New ReportParameter("FromDate", CDate(txtFromDate.SelectedDate))

        Dim TDate As New ReportParameter
        TDate = New ReportParameter("ToDate", CDate(txtToDate.SelectedDate))

        rview.ServerReport.SetParameters(New ReportParameter() {Searchvalue, FDate, TDate})

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
            Response.AddHeader("Content-disposition", "attachment;filename=Log.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=Log.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()

    End Sub
    Private Sub SearchBtn_Click(sender As Object, e As EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            gvRep.Visible = True
            BindReport()

        Else
            Args.Visible = False
            gvRep.Visible = False

        End If
    End Sub
    Sub FillUsers()
        ddlUser.DataTextField = "Username"
        ddlUser.DataValueField = "User_ID"
        ddlUser.DataSource = objUser.GetUsers()
        ddlUser.DataBind()
    End Sub

    Sub FillModule()
        ddlModule.DataTextField = "Module"
        ddlModule.DataValueField = "Module"
        ddlModule.DataSource = objLogin.GetModule()
        ddlModule.DataBind()
    End Sub

    Sub FillSubModule()
        ddlSubModule.DataTextField = "Sub_Module"
        ddlSubModule.DataValueField = "Sub_Module"
        ddlSubModule.DataSource = objLogin.GetSubModule
        ddlSubModule.DataBind()
    End Sub

    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        LoadOrgDetails()
    End Sub
    Sub LoadOrgDetails()
        If Not (ddlOrganization.SelectedItem.Value = "0") Then
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New SalesWorx.BO.Common.Common()
            ddlVan.Items.Clear()
            ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())
            ddlVan.DataBind()
            ddlVan.Items.Insert(0, New RadComboBoxItem("Select Van", "0"))
        Else
        End If
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        ddlVan.ClearSelection()
        ddlVan.Items.Clear()
        LoadOrgDetails()
        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Now()
        ddlModule.ClearSelection()
        ddlOrderType.ClearSelection()
        ddlSubModule.ClearSelection()
        ddlUser.ClearSelection()
        txtKeyValue.Text = ""
        Args.Visible = False
        gvRep.Visible = False
    End Sub
End Class