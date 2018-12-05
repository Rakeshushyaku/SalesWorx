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
Public Class SKUDistribution
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As New SalesWorx.BO.Common.Common
    Dim ObjCollection As Collection
    Private ReportPath As String = "SKUDistribution"
    Private Const PageID As String = "P350"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
            Try
                ObjCommon = New SalesWorx.BO.Common.Common()
                Dim HasPermission As Boolean = False
                ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
                If Not HasPermission Then
                    Err_No = 500
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
                End If
                UId.Value = CType(Session.Item("USER_ACCESS"), UserAccess).UserID
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))
                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                End If
                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                txtToDate.SelectedDate = Now()


                LoadorgDetails()

                'BindData()
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
    
    Private Sub BindChart()
        Try
            ReptDiv.Visible = True
            Dim collection As IList(Of RadComboBoxItem) = ddVan.CheckedItems

            Dim Van As String = ""
            Dim Vantxt As String = ""
            For Each li As RadComboBoxItem In collection
                Van = Van & li.Value & ","
                Vantxt = Vantxt & li.Text & ","
            Next
            If Vantxt <> "" Then
                Vantxt = Vantxt.Substring(0, Vantxt.Length - 1)
            End If
            If Van = "" Then
                Van = "0"
            End If
            If Van = "0" Then
                lblVan.Text = "All"
            Else
                lblVan.Text = Vantxt
            End If

            lbl_org.Text = ddlOrganization.SelectedItem.Text
            lblType.Text = ddl_Type.SelectedItem.Text
            lbl_Argby.Text = ddl_by.SelectedItem.Text
            lbl_Argbytxt.Text = ddl_by.SelectedItem.Text & ":"

            lbl_ArgbyID.Text = ddl_ID.SelectedItem.Text

            lbl_From.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_To.Text = CDate(txtTodate.SelectedDate).ToString("dd-MMM-yyyy")

            hfOrg.Value = ddlOrganization.SelectedItem.Value
            hVan.Value = Van
            hType.Value = ddl_Type.SelectedItem.Value
            Hby.Value = ddl_by.SelectedItem.Value
            If ddl_by.SelectedItem.Value = "P" Then
                hID.Value = ddl_ID.SelectedItem.Value
                HSite.Value = 0
            Else
                Dim sid() As String
                sid = ddl_ID.SelectedItem.Value.Split("$")
                hID.Value = sid(0)
                HSite.Value = sid(1)
            End If

            hfrom.Value = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            Hto.Value = CDate(txtTodate.SelectedDate).ToString("dd-MMM-yyyy")


            Args.Visible = True

            ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub

    
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            BindChart()
        End If
    End Sub
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        LoadorgDetails()
    End Sub
    Sub LoadorgDetails()
        If Not (ddlOrganization.SelectedItem.Value = "0") Then
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New SalesWorx.BO.Common.Common()
            ddVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())
            ddVan.DataBind()

            For Each itm As RadComboBoxItem In ddVan.Items
                itm.Checked = True
            Next

            If ddl_by.SelectedItem.Value = "C" Then
                'ddl_ID.DataSource = ObjCommon.GetCustomerByCriteria(Err_No, Err_Desc, ddlOrganization.SelectedValue)
                'ddl_ID.DataTextField = "Customer"
                'ddl_ID.DataValueField = "CustomerID"
                'ddl_ID.DataBind()

                ddl_ID.Items.Insert(0, New RadComboBoxItem("Select Customer", "0$0"))
            Else

                'ddl_ID.DataSource = (New SalesWorx.BO.Common.Price).GetPriceListHeader(Err_No, Err_Desc, "", "")
                'ddl_ID.DataTextField = "Description"
                'ddl_ID.DataValueField = "Price_List_ID"
                'ddl_ID.DataBind()

                ddl_ID.Items.Insert(0, New RadComboBoxItem("Select Customer", "0"))

            End If
        End If
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

            'If ddl_by.SelectedItem.Value = "C" Then
            '    If ddl_ID.SelectedItem.Value = "0$0" Then
            '        MessageBoxValidation("Please select the Customer", "Validation")
            '        Return bretval
            '    End If
            'Else
            '    If ddl_ID.SelectedItem.Value = "0" Then
            '        MessageBoxValidation("Please select the Price List", "Validation")
            '        Return bretval
            '    End If

            'End If

            Dim TemFromDateStr As String = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            Dim DateArr As Array = TemFromDateStr.Split("-")
            If DateArr.Length = 3 Then
                TemFromDateStr = DateArr(1) & "-" & DateArr(0) & "-" & DateArr(2)
            End If
            Dim TemToDateStr As String = CDate(txtTodate.SelectedDate).ToString("dd-MMM-yyyy")
            Dim DateArr1 As Array = TemToDateStr.Split("-")
            If DateArr1.Length = 3 Then
                TemToDateStr = DateArr1(1) & "-" & DateArr1(0) & "-" & DateArr1(2)
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

    Sub Export(format As String)

        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

        Dim OrgId As New ReportParameter
        OrgId = New ReportParameter("OID", CStr(ddlOrganization.SelectedItem.Value.ToString()))

        Dim collection As IList(Of RadComboBoxItem) = ddVan.CheckedItems
        Dim van As String = ""
        For Each li As RadComboBoxItem In collection
            van = van & li.Value & ","

        Next

        If van = "" Then
            van = "0"
        End If

        Dim SID As New ReportParameter
        SID = New ReportParameter("SID", van)

        Dim FromDate As New ReportParameter
        FromDate = New ReportParameter("FromDate", CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"))

        Dim Uid As New ReportParameter
        Uid = New ReportParameter("Uid", objUserAccess.UserID)

        Dim ToDate As New ReportParameter
        ToDate = New ReportParameter("Todate", CDate(txtTodate.SelectedDate).ToString("dd-MMM-yyyy"))


        Dim Type As New ReportParameter
        Type = New ReportParameter("Type", ddl_Type.SelectedItem.Value)

        Dim By As New ReportParameter
        By = New ReportParameter("By", ddl_by.SelectedItem.Value)

        Dim ID As New ReportParameter
        Dim SiteID As New ReportParameter

        If ddl_by.SelectedItem.Value = "C" Then
            Dim ids() As String
            ids = ddl_ID.SelectedItem.Value.Split("$")
            ID = New ReportParameter("ID", ids(0))
            SiteID = New ReportParameter("SiteID", ids(1))
        Else
            ID = New ReportParameter("ID", ddl_ID.SelectedItem.Value)
            SiteID = New ReportParameter("SiteID", 0)
        End If

        Dim DisplayType As New ReportParameter
        DisplayType = New ReportParameter("DisplayType", "C")


        rview.ServerReport.SetParameters(New ReportParameter() {Uid, OrgId, SID, FromDate, ToDate, Type, By, ID, SiteID, DisplayType})

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
            Response.AddHeader("Content-disposition", "attachment;filename=SKUDistribution.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=SKUDistribution.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()

    End Sub

    Private Sub ddl_by_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddl_by.SelectedIndexChanged
        If ddl_by.SelectedItem.Value = "C" Then
            lbl_by.Text = "Customer"
        Else
            lbl_by.Text = "Price List"
        End If
        If ddl_by.SelectedItem.Value = "C" Then
            ddl_ID.DataSource = ObjCommon.GetCustomerByCriteria(Err_No, Err_Desc, ddlOrganization.SelectedValue)
            ddl_ID.DataTextField = "Customer"
            ddl_ID.DataValueField = "CustomerID"
            ddl_ID.DataBind()

            ddl_ID.Items.Insert(0, New RadComboBoxItem("Select Customer", "0$0"))
        Else

            ddl_ID.DataSource = (New SalesWorx.BO.Common.Price).GetPriceListHeader(Err_No, Err_Desc, "", "")
            ddl_ID.DataTextField = "Description"
            ddl_ID.DataValueField = "Price_List_ID"
            ddl_ID.DataBind()

            ddl_ID.Items.Insert(0, New RadComboBoxItem("Select Customer", "0"))

        End If
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        ddVan.ClearCheckedItems()
        ddVan.Items.Clear()
        LoadorgDetails()
        ddl_Type.ClearSelection()
        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtTodate.SelectedDate = Now()
        Args.Visible = False
        ReptDiv.Visible = False

    End Sub
End Class
