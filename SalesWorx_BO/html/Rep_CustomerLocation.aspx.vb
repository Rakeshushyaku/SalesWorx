Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Public Class Rep_CustomerLocation
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer
    Private ReportPath As String = "CustomerLocation"
    Private Const PageID As String = "P323"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    'Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
    '    If Not IsNothing(Me.Master) Then

    '        Dim masterScriptManager As ScriptManager
    '        masterScriptManager = CType(Master.FindControl("ScriptManager1"), ScriptManager)

    '        ' Make sure our master page has the script manager we're looking for
    '        If Not IsNothing(masterScriptManager) Then

    '            ' Turn off partial page postbacks for this page
    '            masterScriptManager.EnablePartialRendering = False
    '        End If

    '    End If

    'End Sub

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

                ddlSegment.DataSource = ObjCommon.GetCustomerSegmentList(Err_No, Err_Desc, SubQry)
                ddlSegment.DataBind()
                ddlSegment.Items.Insert(0, New RadComboBoxItem("Select Segment", "0"))
                ddlSalesDist.DataSource = ObjCommon.GetSalesDistrictList(Err_No, Err_Desc, SubQry)
                ddlSalesDist.DataBind()
                ddlSalesDist.Items.Insert(0, New RadComboBoxItem("Select Sales District", "0"))
                'ddlType.DataSource = ObjCommon.GetCustomerTypeList(Err_No, Err_Desc, SubQry)
                'ddlType.DataBind()
                'ddlType.Items.Insert(0, New RadComboBoxItem("-- Select a value --"))

                ' BindData()
                LoadChannel()
                LoadSubChannel()
                LoadVan()

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
    Sub Export(format As String)

        Dim SearchParams As String = ""
        SearchParams = BuildQuery()
        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        Dim Searchvalue As New ReportParameter

        Searchvalue = New ReportParameter("SearchParams", SearchParams)
        Dim OrgId As New ReportParameter
        OrgId = New ReportParameter("OrgId", CStr(ddlOrganization.SelectedItem.Value.ToString()))

        rview.ServerReport.SetParameters(New ReportParameter() {Searchvalue, OrgId})

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
            Response.AddHeader("Content-disposition", "attachment;filename=CustomerList.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=CustomerList.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()
    End Sub


    Private Function BuildQuery() As String
        Dim SearchQuery As String = ""
        Try
            ObjCustomer = New Customer()
            If ddlSegment.SelectedItem.Value <> "0" Then
                SearchQuery = " And B.Customer_Segment_ID='" & ddlSegment.SelectedItem.Value & "'"
            End If
            If ddlSalesDist.SelectedItem.Value <> "0" Then
                SearchQuery = SearchQuery & " And B.Sales_District_ID='" & ddlSalesDist.SelectedItem.Value & "'"
            End If
            'If ddlType.SelectedItem.Value <> "-- Select a value --" Then
            '    SearchQuery = SearchQuery & " And A.Customer_Type='" & ddlType.SelectedItem.Value & "'"
            'End If

            If txtCustomerNo.Text <> "" Then
                SearchQuery = SearchQuery & " And A.Customer_No like '%" & Utility.ProcessSqlParamString(txtCustomerNo.Text) & "%'"
            End If
            If txtCustomerName.Text <> "" Then
                SearchQuery = SearchQuery & " And A.Customer_Name like '%" & Utility.ProcessSqlParamString(txtCustomerName.Text) & "%'"
            End If
            If ddl_Channel.SelectedItem.Value <> "0" Then
                SearchQuery = SearchQuery & " And isnull(A.Customer_Type,'N/A')='" & ddl_Channel.SelectedItem.Value & "'"
            End If
            If ddl_SubChannel.SelectedItem.Value <> "0" Then
                SearchQuery = SearchQuery & " And isnull(A.Customer_Class,'N/A')='" & ddl_SubChannel.SelectedItem.Value & "'"
            End If
            If ddl_Van.SelectedItem.Value <> "0" Then
                SearchQuery = SearchQuery & " And EXISTS (Select V.Customer_ID,V.Site_Use_ID from V_FSR_CustomerShipAddress V Where V.Customer_ID=B.Customer_ID and V.Site_Use_ID=B.Site_Use_ID and V.SalesRep_ID=" & ddl_Van.SelectedItem.Value & ")"
            End If
            If ddlCust_Stat.SelectedItem.Value = "2" Then
                SearchQuery = SearchQuery & " And (Credit_Hold='Y' OR  A.Cust_Status='N')"
            End If
            If ddlCust_Stat.SelectedItem.Value = "1" Then
                SearchQuery = SearchQuery & " And not (Credit_Hold='Y' OR  A.Cust_Status='N')"
            End If

            If ddl_CustType.SelectedItem.Value = "1" Then
                SearchQuery = SearchQuery & " And Cash_Cust='Y'"
            End If
            If ddl_CustType.SelectedItem.Value = "2" Then
                SearchQuery = SearchQuery & " And Cash_Cust='N'"
            End If
            If ddl_Gps.SelectedItem.Value = "1" Then
                SearchQuery = SearchQuery & " And ( isnull(Cust_Lat,0)=0 or isnull(Cust_Long,0)=0 )"
            ElseIf ddl_Gps.SelectedItem.Value = "2" Then
                SearchQuery = SearchQuery & " And ( isnull(Cust_Lat,0)<>0 or isnull(Cust_Long,0)<>0 )"
            End If
            Return SearchQuery
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
        If ValidateInputs() Then
            gvRep.Visible = True
            BindReport()
        Else
            Args.Visible = False
            summary.InnerHtml = ""
            gvRep.Visible = False
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

            lbl_org.Text = ddlOrganization.SelectedItem.Text
            lbl_CustNo.Text = IIf(String.IsNullOrEmpty(txtCustomerNo.Text), "All", txtCustomerNo.Text)
            If ddl_Van.SelectedItem.Value = "0" Then
                lbl_van.Text = "All"
            Else
                lbl_van.Text = ddl_Van.SelectedItem.Text
            End If
            If ddlSegment.SelectedItem.Value = "0" Then
                lbl_Segment.Text = "All"
            Else
                lbl_Segment.Text = ddlSegment.SelectedItem.Text
            End If
            If ddlSalesDist.SelectedItem.Value = "0" Then
                lbl_SalesDistrict.Text = "All"
            Else
                lbl_SalesDistrict.Text = ddlSalesDist.SelectedItem.Text
            End If
            If ddl_Channel.SelectedItem.Value = "0" Then
                lbl_Channel.Text = "All"
            Else
                lbl_Channel.Text = ddl_Channel.SelectedItem.Text
            End If

            If ddl_CustType.SelectedItem.Value = "0" Then
                lbl_CustType.Text = "All"
            Else
                lbl_CustType.Text = ddl_CustType.SelectedItem.Text
            End If

            If ddlCust_Stat.SelectedItem.Value = "0" Then
                lbl_Status.Text = "All"
            Else
                lbl_Status.Text = ddlCust_Stat.SelectedItem.Text
            End If

            If ddl_SubChannel.SelectedItem.Value = "0" Then
                lbl_SubChannel.Text = "All"
            Else
                lbl_SubChannel.Text = ddl_SubChannel.SelectedItem.Text
            End If

            lbl_Geocodes.Text = ddl_Gps.SelectedItem.Text
            lbl_CustName.Text = IIf(String.IsNullOrEmpty(txtCustomerName.Text), "All", txtCustomerName.Text)

            Args.Visible = True

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetCustomerListing(Err_No, Err_Desc, SearchQuery, ddlOrganization.SelectedItem.Value)
            gvRep.DataSource = dt
            gvRep.DataBind()

            
        Catch Ex As Exception
            log.Error(Ex.Message)
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
    Private Sub dgv_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvRep.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindReport()
    End Sub
    Private Sub dgv_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged

        BindReport()
    End Sub
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If (ddlOrganization.SelectedItem.Value = "0") Then

            MessageBoxValidation("Please select the Organisation", "Validation")
            Return bretval
        Else
            bretval = True
            Return bretval
        End If
    End Function

    Private Sub ddlOrganization_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlOrganization.SelectedIndexChanged
        LoadChannel()
        LoadSubChannel()
        LoadVan()
    End Sub
    Sub LoadChannel()
        ObjCommon = New SalesWorx.BO.Common.Common()
        ddl_Channel.DataSource = ObjCommon.GetChannel(Err_No, Err_Desc, ddlOrganization.SelectedValue)
        ddl_Channel.DataValueField = "Customer_Type"
        ddl_Channel.DataTextField = "Customer_Type"
        ddl_Channel.DataBind()
        ddl_Channel.Items.Insert(0, New RadComboBoxItem("Select Channel", "0"))
    End Sub
    Sub LoadSubChannel()
        ObjCommon = New SalesWorx.BO.Common.Common()
        ddl_SubChannel.DataSource = ObjCommon.GetSubChannel(Err_No, Err_Desc, ddlOrganization.SelectedValue)
        ddl_SubChannel.DataValueField = "Customer_Class"
        ddl_SubChannel.DataTextField = "Customer_Class"
        ddl_SubChannel.DataBind()
        ddl_SubChannel.Items.Insert(0, New RadComboBoxItem("Select Sub-channel", "0"))
    End Sub

    Sub LoadVan()
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        ObjCommon = New SalesWorx.BO.Common.Common()
        ddl_Van.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)

        ddl_Van.DataValueField = "SalesRep_ID"
        ddl_Van.DataTextField = "SalesRep_Name"
        ddl_Van.DataBind()
        ddl_Van.Items.Insert(0, New RadComboBoxItem("Select Van/FSR", "0"))
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        LoadChannel()
        LoadSubChannel()
        LoadVan()
        ddl_Channel.ClearSelection()
        ddl_CustType.ClearSelection()
        ddl_SubChannel.ClearSelection()
        ddl_Van.ClearSelection()
        ddlCust_Stat.ClearSelection()
        ddlSegment.ClearSelection()
        ddlSalesDist.ClearSelection()
        ddl_Gps.ClearSelection()
        txtCustomerName.Text = ""
        txtCustomerNo.Text = ""
        gvRep.Visible = False
        Args.Visible = False
        summary.InnerHtml = ""
    End Sub
End Class
