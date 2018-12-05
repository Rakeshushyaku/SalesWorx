Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI

Partial Public Class RepAssets
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim objCommon As New SalesWorx.BO.Common.Common
    Dim objAssetType As New SalesWorx.BO.Common.AssetType

    Private ReportPath As String = "AssetList"

    Private Const PageID As String = "P265"

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

                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataTextField = "Description"
                ddlOrganization.DataValueField = "MAS_Org_ID"
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))
                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                    LoadCurrency()
                End If
                LoadAssetTypes()

                ''Back from view asset histrory
                If Not String.IsNullOrEmpty(Request.QueryString("b")) Then


                    If Session("CurOrg") IsNot Nothing Then
                        '' Setting Org

                        If Not ddlOrganization.FindItemByValue(Session("CurOrg")) Is Nothing Then
                            ddlOrganization.ClearSelection()
                            ddlOrganization.FindItemByValue(Session("CurOrg")).Selected = True
                        End If
                        If Session("CurONLYACTIVE") IsNot Nothing Then
                            If Session("CurONLYACTIVE").ToString().Trim() = "1" Then
                                chkActive.Checked = True
                            Else
                                chkActive.Checked = False
                            End If
                        End If

                        If Session("CurCus") <> "" Then
                            '' Loading Customer dropDown 
                            Dim ID As String() = Session("CurCus").Split("-")
                            If ID.Length > 1 Then
                                Dim Objrep As New SalesWorx.BO.Common.Reports()
                                Dim dt As New DataTable
                                dt = Objrep.GetCustomerfromOrgtext(Err_No, Err_Desc, ddlOrganization.SelectedValue, ID(1))

                                If dt.Rows.Count > 0 Then
                                    For i As Integer = 0 To dt.Rows.Count - 1
                                        Dim item As New RadComboBoxItem()
                                        item.Text = dt.Rows(i).Item("Customer").ToString
                                        item.Value = dt.Rows(i).Item("CustomerID").ToString
                                        item.Selected = True

                                        ddl_Customer.Items.Add(item)
                                        item.DataBind()
                                    Next

                                    If Not ddl_Customer.FindItemByText(Session("CurCus")) Is Nothing Then
                                        ddl_Customer.ClearSelection()
                                        ddl_Customer.FindItemByText(Session("CurCus")).Selected = True
                                    End If

                                End If
                            End If
                        End If


                        If Not ddlAssetType.FindItemByValue(Session("CurAssetType")) Is Nothing Then
                            ddlAssetType.ClearSelection()
                            ddlAssetType.FindItemByValue(Session("CurAssetType")).Selected = True
                        End If
                        'Session("CurOrg") = ddlOrganization.SelectedValue
                        'Session("CurONLYACTIVE") = ONLYACTIVE
                        'Session("CurcustID") = custID
                        'Session("CurSiteID") = SiteID
                        'Session("CurAssetType") = ddlAssetType.SelectedItem.Value
                        BindReport()

                        Session("CurOrg") = Nothing
                        Session("CurONLYACTIVE") = Nothing
                        Session("CurcustID") = Nothing
                        Session("CurSiteID") = Nothing
                        Session("CurAssetType") = Nothing
                        Session("CurCus") = Nothing

                        gvRep.Visible = True
                    End If


                    'Dim ObjReport As New SalesWorx.BO.Common.Reports
                    'Dim dt As New DataTable
                    'dt = ObjReport.GetAssets(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, ONLYACTIVE, custID, SiteID, ddlAssetType.SelectedItem.Value)


                    'gvRep.DataSource = dt
                    'gvRep.DataBind()

                End If

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

                ErrorResource = Nothing
            End Try
        End If
    End Sub
    Private Sub ddl_Customer_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddl_Customer.ItemsRequested
        gvRep.Visible = False
        Args.Visible = False
        Dim Objrep As New SalesWorx.BO.Common.Reports()
        Dim dt As New DataTable
        dt = Objrep.GetCustomerfromOrgtext(Err_No, Err_Desc, ddlOrganization.SelectedValue, e.Text)


        Dim ItemsPerRequest As Integer = 100 'Set the number of values to show
        Dim itemOffset As Integer = e.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
        e.EndOfItems = endOffset = dt.Rows.Count

        'Loop through the values to populate the combo box
        For i As Integer = itemOffset To endOffset - 1
            Dim item As New RadComboBoxItem()
            item.Text = dt.Rows(i).Item("Customer").ToString
            item.Value = dt.Rows(i).Item("CustomerID").ToString

            ddl_Customer.Items.Add(item)
            item.DataBind()
        Next
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Private Sub LoadCurrency()
        Dim dtcurrency As DataTable
        Dim ObjReport As New SalesWorx.BO.Common.Reports
        dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)

        Dim Currency As String = ""
        If dtcurrency.Rows.Count > 0 Then
            Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
            Me.hfCurrency.Value = dtcurrency.Rows(0)("Currency_Code")
            HfDecimal.Value = dtcurrency.Rows(0)("Decimal_Digits")
        End If
        gvRep.Columns(9).HeaderText = "Value (" & Me.hfCurrency.Value & ")"
        CType(gvRep.Columns(9), GridBoundColumn).DataFormatString = "{0:N" & HfDecimal.Value & "}"
        CType(gvRep.Columns(9), GridBoundColumn).FooterAggregateFormatString = "{0:N" & HfDecimal.Value & "}"
    End Sub
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged

        LoadCurrency()
        gvRep.Visible = False
        Args.Visible = False
    End Sub
    Private Sub LoadAssetTypes()
        Dim y As New DataTable
        y = objAssetType.FillAssetType(Err_No, Err_Desc)
        Dim r As DataRow = y.NewRow
        r(0) = "0"
        r(1) = "Select Asset"
        y.Rows.InsertAt(r, 0)

        ddlAssetType.SelectedIndex = 0
        ddlAssetType.DataValueField = "Asset_Type_ID"
        ddlAssetType.DataTextField = "Description"
        ddlAssetType.DataSource = y
        ddlAssetType.DataBind()
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
             

            rpbFilter.Items(0).Expanded = False
            If ddlAssetType.SelectedItem.Value = "0" Then
                lbl_Type.Text = "All"
            Else
                lbl_Type.Text = ddlAssetType.SelectedItem.Text
            End If

            Dim custID As String = "0"
            Dim SiteID As String = "0"
            If Not String.IsNullOrEmpty(ddl_Customer.SelectedValue) Then

                Dim ids() As String
                ids = ddl_Customer.SelectedValue.Split("$")
                Dim custdt As New DataTable
                custdt = (New SalesWorx.BO.Common.Customer).GetCustomerDeatils(Err_No, Err_Desc, ids(0), ids(1))
                If custdt.Rows.Count > 0 Then
                    lbl_Customer.Text = custdt.Rows(0)("Customer_no").ToString & " - " & custdt.Rows(0)("Customer_name").ToString
                End If
                custID = ids(0)
                SiteID = ids(1)
            Else
                custID = "0"
                SiteID = "0"
                lbl_Customer.Text = "All"
            End If
            Dim ONLYACTIVE As String
            If chkActive.Checked = True Then
                lbl_Active.Text = "Show Active Assets Only"
                ONLYACTIVE = "1"
            Else
                lbl_Active.Text = "Show All Assets"
                ONLYACTIVE = "0"
            End If

            lbl_org.Text = ddlOrganization.SelectedItem.Text

            Args.Visible = True

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetAssets(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, ONLYACTIVE, custID, SiteID, ddlAssetType.SelectedItem.Value)
             

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
    Sub Export(format As String)



        

        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)

        Dim OrgID As New ReportParameter
        OrgID = New ReportParameter("OrgID", ddlOrganization.SelectedValue)

        Dim AssetType As New ReportParameter
        AssetType = New ReportParameter("AssetType", CStr(IIf(Me.ddlAssetType.SelectedIndex <= 0, "0", Me.ddlAssetType.SelectedValue.ToString())))


        Dim custID As String = "0"
        Dim SiteID As String = "0"
        If Not String.IsNullOrEmpty(ddl_Customer.SelectedValue) Then
            Dim ids() As String
            ids = ddl_Customer.SelectedValue.Split("$")
            custID = ids(0)
            SiteID = ids(1)
        Else
            custID = "0"
            SiteID = "0"
            lbl_Customer.Text = "All"
        End If

        Dim Cust As New ReportParameter
        Cust = New ReportParameter("CustomerID", custID)

        Dim Site_ID As New ReportParameter
        Site_ID = New ReportParameter("SiteID", SiteID)


        Dim Active As New ReportParameter
        Active = New ReportParameter("OnlyActive", CStr(IIf(Me.chkActive.Checked = True, "1", "0")))

        rview.ServerReport.SetParameters(New ReportParameter() {Cust, OrgID, AssetType, Site_ID, Active})

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
            Response.AddHeader("Content-disposition", "attachment;filename=Assets.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=Assets.xls")
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

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        ddl_Customer.ClearSelection()
        ddl_Customer.Text = ""
        ddlAssetType.ClearSelection()
        chkActive.Checked = False
        gvRep.Visible = False
        Args.Visible = False
    End Sub

    Protected Sub Lnk_RefID_Click(sender As Object, e As EventArgs)

        Dim btnEdit As LinkButton = TryCast(sender, LinkButton)
        Dim row As Telerik.Web.UI.GridDataItem = DirectCast(btnEdit.NamingContainer, Telerik.Web.UI.GridDataItem)
        Dim Asset_ID As String = CType(row.FindControl("HAsset_ID"), HiddenField).Value

        Dim custID As String = "0"
        Dim SiteID As String = "0"
        If Not String.IsNullOrEmpty(ddl_Customer.SelectedValue) Then

            Dim ids() As String
            ids = ddl_Customer.SelectedValue.Split("$")
            Dim custdt As New DataTable
            custdt = (New SalesWorx.BO.Common.Customer).GetCustomerDeatils(Err_No, Err_Desc, ids(0), ids(1))
            If custdt.Rows.Count > 0 Then
                lbl_Customer.Text = custdt.Rows(0)("Customer_no").ToString & " - " & custdt.Rows(0)("Customer_name").ToString
            End If
            custID = ids(0)
            SiteID = ids(1)
        Else
            custID = "0"
            SiteID = "0"
            lbl_Customer.Text = "All"
        End If

        Dim ONLYACTIVE As String
        If chkActive.Checked = True Then
            lbl_Active.Text = "Show Active Assets Only"
            ONLYACTIVE = "1"
        Else
            lbl_Active.Text = "Show All Assets"
            ONLYACTIVE = "0"
        End If

        Session("CurOrg") = ddlOrganization.SelectedValue
        Session("CurONLYACTIVE") = ONLYACTIVE
        '  Session("CurcustID") = custID
        ' Session("CurSiteID") = SiteID
        Session("CurAssetType") = ddlAssetType.SelectedItem.Value
        Session("CurCus") = ddl_Customer.Text

       

        'Session("CurFDat") = txtFromDate.SelectedDate
        'Session("CurTDat") = txtToDate.SelectedDate
        Session("CurPIndex") = gvRep.CurrentPageIndex
        '  URL = 'Rep_ViewAssetHistory.aspx?OrgID=' + combo.get_selectedItem().get_value() + '&Type=Asset&ReportName=AssetHistory&ID=' + RowID;
        Response.Redirect("Rep_ViewAssetHistory.aspx?OrgID=" & ddlOrganization.SelectedValue & "&Type=Asset&ID=" & Asset_ID)

    End Sub
End Class