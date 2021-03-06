﻿Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Partial Public Class RepProductListing
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjProduct As Product
    Private ReportPath As String = "ProductListing"
    Private Const PageID As String = "P201"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'If Not IsNothing(Me.Master) Then

        '    Dim masterScriptManager As ScriptManager
        '    masterScriptManager = CType(Master.FindControl("ScriptManager1"), ScriptManager)

        '    ' Make sure our master page has the script manager we're looking for
        '    If Not IsNothing(masterScriptManager) Then

        '        ' Turn off partial page postbacks for this page
        '        masterScriptManager.EnablePartialRendering = False
        '    End If

        'End If

    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
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
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))

                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                End If
                LoadOrgdetails()
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
    Private Function BuildQuery() As String
        Dim SearchQuery As String = ""
        Try
            ObjProduct = New Product()
            If (ddlOrganization.SelectedItem.Value = "0" And txtItemCode.Text = "" And txtDescription.Text = "") Then
                SearchQuery = ""
            ElseIf (txtItemCode.Text = "" And txtDescription.Text = "" And ddlOrganization.SelectedItem.Value <> "0") Then
                SearchQuery = " And b.Organization_ID=" & ddlOrganization.SelectedItem.Value
            ElseIf (txtItemCode.Text <> "" And txtDescription.Text = "" And ddlOrganization.SelectedItem.Value = "0") Then
                SearchQuery = "  and  b.Item_Code like '%" & Utility.ProcessSqlParamString(txtItemCode.Text) & "%'"
            ElseIf (txtItemCode.Text <> "" And txtDescription.Text <> "" And ddlOrganization.SelectedItem.Value = "0") Then
                SearchQuery = " and  b.Item_Code like '%" & Utility.ProcessSqlParamString(txtItemCode.Text) & "%'" & " AND b.[Description] like '%" & Utility.ProcessSqlParamString(txtDescription.Text) & "%'"
            ElseIf (txtItemCode.Text <> "" And txtDescription.Text = "" And ddlOrganization.SelectedItem.Value <> "0") Then
                SearchQuery = " And b.Organization_ID=" & ddlOrganization.SelectedItem.Value & " and  b.Item_Code like '%" & Utility.ProcessSqlParamString(txtItemCode.Text) & "%'"
            ElseIf (txtItemCode.Text <> "" And txtDescription.Text <> "" And ddlOrganization.SelectedItem.Value <> "0") Then
                SearchQuery = " And b.Organization_ID=" & ddlOrganization.SelectedItem.Value & " and  b.Item_Code like '%" & Utility.ProcessSqlParamString(txtItemCode.Text) & "%'" & " AND b.[Description] like '%" & Utility.ProcessSqlParamString(txtDescription.Text) & "%'"
            ElseIf (txtItemCode.Text = "" And txtDescription.Text <> "" And ddlOrganization.SelectedItem.Value <> "0") Then
                SearchQuery = " And b.Organization_ID=" & ddlOrganization.SelectedItem.Value & " AND b.[Description] like '%" & Utility.ProcessSqlParamString(txtDescription.Text) & "%'"
            End If
            If Not ddl_Agency.CheckedItems Is Nothing Then
                If ddl_Agency.CheckedItems.Count > 0 Then
                    Dim StrAgency As String = ""
                    For Each itm As RadComboBoxItem In ddl_Agency.CheckedItems
                        StrAgency = StrAgency & itm.Value & ","
                    Next
                    If StrAgency <> "" Then
                        StrAgency = StrAgency.Substring(0, StrAgency.Length - 1)
                    End If
                    SearchQuery = SearchQuery & " and  b.Agency in(select item from dbo.SplitQuotedString('" & StrAgency & "'))"
                End If
            End If
            If ddlProductType.SelectedValue = 1 Then
                SearchQuery = SearchQuery & " And a.Row_ID is not null"
            ElseIf ddlProductType.SelectedValue = 2 Then
                SearchQuery = SearchQuery & " And a.Row_ID is null"
            Else
                SearchQuery = SearchQuery
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
            ObjProduct = Nothing
        End Try
    End Function

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


        Dim agencytxt As String = ""
        For Each itm As RadComboBoxItem In ddl_Agency.CheckedItems
            agencytxt = agencytxt & itm.Text & ","
        Next


        Dim Agency As New ReportParameter

        If agencytxt <> "" Then
            agencytxt = agencytxt.Substring(0, agencytxt.Length - 1)
        End If
        If agencytxt <> "" Then
            lbl_agency.Text = agencytxt
            Agency = New ReportParameter("Agency", agencytxt)
        Else
            Agency = New ReportParameter("Agency", "All")
        End If

        Dim Org As New ReportParameter
        Org = New ReportParameter("Org_ID", CStr(IIf(ddlOrganization.SelectedItem.Value = "0", "All", ddlOrganization.SelectedItem.Text)))

        Dim Type As New ReportParameter
        Type = New ReportParameter("Type", ddlProductType.SelectedItem.Text)


        Dim code As New ReportParameter
        code = New ReportParameter("ItemCode", txtItemCode.Text.Trim())

        Dim desc As New ReportParameter
        desc = New ReportParameter("Desc", txtDescription.Text.Trim())


        rview.ServerReport.SetParameters(New ReportParameter() {Searchvalue, Org, Type, Agency, code, desc})

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
            Response.AddHeader("Content-disposition", "attachment;filename=ProductList.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=ProductList.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()

    End Sub

    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            gvRep.Visible = True
            BindReport()

        Else
            Args.Visible = False
            gvRep.Visible = False

        End If
    End Sub
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If ddlOrganization.SelectedIndex > 0 Then
            bretval = True
            Return bretval
        Else
            MessageBoxValidation("Please select the Organisation", "Validation")
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

            lbl_Type.Text = ddlProductType.SelectedItem.Text
            lbl_org.Text = ddlOrganization.SelectedItem.Text
            lbl_SkU.Text = IIf(String.IsNullOrEmpty(txtItemCode.Text), "All", txtItemCode.Text)
            lbl_Desc.Text = IIf(String.IsNullOrEmpty(txtDescription.Text), "All", txtDescription.Text)

            Dim agencytxt As String = ""
            For Each itm As RadComboBoxItem In ddl_Agency.CheckedItems
                agencytxt = agencytxt & itm.Text & ","
            Next

            If agencytxt <> "" Then
                agencytxt = agencytxt.Substring(0, agencytxt.Length - 1)
            End If
            If agencytxt <> "" Then
                lbl_agency.Text = agencytxt
            Else
                lbl_agency.Text = "All"
            End If

            Args.Visible = True

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetProducts(Err_No, Err_Desc, SearchQuery)
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
    Private Sub ddlOrganization_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlOrganization.SelectedIndexChanged
        LoadOrgdetails()
    End Sub

    Sub LoadOrgdetails()

        ddl_Agency.DataTextField = "Agency"
        ddl_Agency.DataValueField = "Agency"
        Dim Objrep As New SalesWorx.BO.Common.Reports()
        ddl_Agency.DataSource = Objrep.GetAgency(Err_No, Err_Desc, ddlOrganization.SelectedValue)
        ddl_Agency.DataBind()
        Objrep = Nothing
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        LoadOrgdetails()
        ddl_Agency.ClearCheckedItems()
        ddlProductType.ClearSelection()
        txtDescription.Text = ""
        txtItemCode.Text = ""
        Args.Visible = False
        gvRep.Visible = False
    End Sub
End Class