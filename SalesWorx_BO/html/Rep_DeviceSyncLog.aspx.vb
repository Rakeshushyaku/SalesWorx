Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports System.Collections.Generic
Imports Telerik.Web.UI
Partial Public Class Rep_DeviceSyncLog
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "DeviceSyncLog"

    Private Const PageID As String = "P102"
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
                Dim CountryTbl As DataTable = Nothing
                Dim orgTbl As DataTable = Nothing

                CountryTbl = ObjCommon.GetCountry(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)
                ddlCountry.DataSource = CountryTbl
                ddlCountry.DataBind()


                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)

                Dim s() As String = Nothing
                Dim Currency As String = Nothing
                Dim DecimalDigits As String = "2"
                Dim country As String = Nothing
                If CountryTbl.Rows.Count = 1 Then

                    ddlCountry.SelectedIndex = 0
                    dvCountry.Visible = False

                    s = ddlCountry.SelectedValue.Split("$")

                    If s.Length > 0 Then
                        country = s(0).ToString()
                        Currency = s(1).ToString()
                        DecimalDigits = s(2).ToString()
                    End If


                    ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
                    ddlOrganization.DataBind()


                ElseIf CountryTbl.Rows.Count > 1 Then
                    ddlCountry.SelectedIndex = 0
                    dvCountry.Visible = True


                    s = ddlCountry.SelectedValue.Split("$")

                    If s.Length > 0 Then
                        country = s(0).ToString()
                        Currency = s(1).ToString()
                        DecimalDigits = s(2).ToString()
                    End If

                    ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
                    ddlOrganization.DataBind()

                End If

                Dim OrgStr As String = Nothing
                For Each item As RadComboBoxItem In ddlOrganization.Items
                    item.Checked = True
                    If item.Checked Then

                        OrgStr = OrgStr & "," & item.Value

                    End If
                Next

                LoadOrgDetails()
                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")



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
    Private Sub ddlCountry_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlCountry.SelectedIndexChanged
        LoadOrgs()
        LoadOrgDetails()
    End Sub
    Sub LoadOrgs()
        Dim orgTbl As DataTable = Nothing

        ObjCommon = New SalesWorx.BO.Common.Common()


        Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)



        orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)

        Dim s() As String = Nothing
        Dim Currency As String = Nothing
        Dim DecimalDigits As String = "2"
        Dim country As String = Nothing
        s = ddlCountry.SelectedValue.Split("$")

        If s.Length > 0 Then
            country = s(0).ToString()
            Currency = s(1).ToString()
            DecimalDigits = s(2).ToString()
        End If


        ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
        ddlOrganization.DataBind()



        Dim OrgStr As String = Nothing
        For Each item As RadComboBoxItem In ddlOrganization.Items
            item.Checked = True
            If item.Checked Then

                OrgStr = OrgStr & "," & item.Value

            End If
        Next

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


        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)



        Dim USRID As New ReportParameter
        USRID = New ReportParameter("Uid", objUserAccess.UserID)


        Dim collection As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddl_Van.CheckedItems

        Dim van As String = ""
        For Each li As Telerik.Web.UI.RadComboBoxItem In collection
            If String.IsNullOrEmpty(van) Then
                van = li.Value
            Else
                van = van & "," & li.Value
            End If

        Next

        Dim collection_SyncType As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddl_SyncType.CheckedItems
        Dim SyncType As String = ""
        For Each li_SyncType As Telerik.Web.UI.RadComboBoxItem In collection_SyncType
            If String.IsNullOrEmpty(SyncType) Then
                SyncType = li_SyncType.Value
            Else
                SyncType = SyncType & "," & li_SyncType.Value
            End If

        Next




        Dim Orgtxt As String = ""
        Dim Org As String = ""
        Dim Orgcollection As IList(Of RadComboBoxItem) = ddlOrganization.CheckedItems
        For Each li As RadComboBoxItem In Orgcollection
            Org = Org & li.Value & ","
            Orgtxt = Orgtxt & li.Text & ","
        Next
        If Orgtxt <> "" Then
            Orgtxt = Orgtxt.Substring(0, Orgtxt.Length - 1)
        End If
        If Org = "" Then
            Org = "0"
        End If

        Dim SalesRepID As New ReportParameter
        SalesRepID = New ReportParameter("UserName", CStr(IIf(String.IsNullOrEmpty(van), "0", van)))


        Dim Sync_Type As New ReportParameter
        Sync_Type = New ReportParameter("Sync_Type", CStr(IIf(String.IsNullOrEmpty(SyncType), "0", SyncType)))

        Dim FDate As New ReportParameter
        FDate = New ReportParameter("FromDate", CDate(txtFromDate.SelectedDate))

        Dim TDate As New ReportParameter
        TDate = New ReportParameter("ToDate", CDate(txtToDate.SelectedDate))


        Dim OrgName As New ReportParameter
        OrgName = New ReportParameter("OrgName", Orgtxt)

        

        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)

        rview.ServerReport.SetParameters(New ReportParameter() {USRID, SalesRepID, FDate, TDate, OrgName, Sync_Type})

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
            Response.AddHeader("Content-disposition", "attachment;filename=DeviceSyncLog.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=DeviceSyncLog.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()

    End Sub
    Private Sub BindReport()


        rpbFilter.Items(0).Expanded = False
        Dim collection As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddl_Van.CheckedItems

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



        If van = "0" Then
            lbl_van.Text = "All"
        Else
            lbl_van.Text = vantxt
        End If



        Dim collection_Stype As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddl_SyncType.CheckedItems
        Dim Stypetxt As String = ""
        Dim Stype As String = ""
        For Each li_stype As RadComboBoxItem In collection_Stype
            Stype = Stype & li_stype.Value & ","
            Stypetxt = Stypetxt & li_stype.Text & ","
        Next
        If Stypetxt <> "" Then
            Stypetxt = Stypetxt.Substring(0, Stypetxt.Length - 1)
        End If
        If Stype = "" Then
            Stype = "0"
        End If



        If Stype = "0" Then
            lbl_synctype.Text = "All"
        Else
            lbl_synctype.Text = Stypetxt
        End If


        Dim Orgtxt As String = ""
        Dim Org As String = ""
        Dim Orgcollection As IList(Of RadComboBoxItem) = ddlOrganization.CheckedItems
        For Each li As RadComboBoxItem In Orgcollection
            Org = Org & li.Value & ","
            Orgtxt = Orgtxt & li.Text & ","
        Next
        If Orgtxt <> "" Then
            Orgtxt = Orgtxt.Substring(0, Orgtxt.Length - 1)
        End If
        If Org = "" Then
            Org = "0"
        End If

        lbl_org.Text = Orgtxt
        lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
        lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")
        Args.Visible = True

        HSID.Value = van
        hfSMonth.Value = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
        hfEMonth.Value = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")
        hfOrg.Value = Org

        Dim ObjReport As New SalesWorx.BO.Common.Reports
        Dim dt As New DataTable
        dt = ObjReport.GetDeviceSynLog(Err_No, Err_Desc, van, txtFromDate.SelectedDate, txtToDate.SelectedDate, Stype)

        gvRep.DataSource = dt
        gvRep.DataBind()

        Dim ClientCode As String
        ClientCode = (New SalesWorx.BO.Common.Common).GetAppConfig(Err_No, Err_Desc, "CLIENT_CODE")
        If ClientCode = "GYMA" Then
            gvRep.MasterTableView.Columns.FindByUniqueName("Device_ID").Visible = True
            gvRep.MasterTableView.Columns.FindByUniqueName("Failure").Visible = True
        Else
            gvRep.MasterTableView.Columns.FindByUniqueName("Device_ID").Visible = False
            gvRep.MasterTableView.Columns.FindByUniqueName("Failure").Visible = False
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

    
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If Not ddlOrganization.CheckedItems Is Nothing Then
            If ddlOrganization.CheckedItems.Count > 0 Then
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
        Else
            MessageBoxValidation("Please select the Organisation", "Validation")
            Return bretval
        End If
    End Function
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            gvRep.Visible = True
            RepDiv.Visible = True
            BindReport()
        Else
            Args.Visible = False
            summary.InnerHtml = ""
            gvRep.Visible = False
            RepDiv.Visible = False
        End If
    End Sub
    Sub MessageBoxValidation(ByVal str As String, Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
     
    Sub LoadOrgDetails()
        If Not ddlOrganization.CheckedItems Is Nothing Then
            If ddlOrganization.CheckedItems.Count > 0 Then

                Dim objUserAccess As UserAccess
                objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
                ObjCommon = New SalesWorx.BO.Common.Common()

                Dim OrgStr As String = ""
                For Each li As RadComboBoxItem In ddlOrganization.CheckedItems
                    OrgStr = OrgStr & li.Value & ","
                Next

                ddl_Van.DataTextField = "SalesRep_Name"
                ddl_Van.DataValueField = "SalesRep_ID"
                ddl_Van.DataSource = ObjCommon.GetVanByOrgForSync(Err_No, Err_Desc, OrgStr, objUserAccess.UserID)

                ddl_Van.DataBind()

                For Each itm As RadComboBoxItem In ddl_Van.Items
                    itm.Checked = True
                Next

                Dim dt_SyncType As DataTable
                dt_SyncType = ObjCommon.GetSyncTypeForDeviceSync(Err_No, Err_Desc)
                ddl_SyncType.DataSource = dt_SyncType
                ddl_SyncType.DataTextField = "Sync_Type"
                ddl_SyncType.DataValueField = "Sync_Type"
                ddl_SyncType.DataBind()

                For Each itm_stype As RadComboBoxItem In ddl_SyncType.Items
                    itm_stype.Checked = True
                Next


            Else
                ddl_Van.Items.Clear()
                ddl_SyncType.Items.Clear()
            End If
        Else
            ddl_Van.Items.Clear()
            ddl_SyncType.Items.Clear()
        End If

    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ObjCommon = New SalesWorx.BO.Common.Common()
        Dim CountryTbl As DataTable = Nothing
        Dim orgTbl As DataTable = Nothing

        CountryTbl = ObjCommon.GetCountry(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)
        ddlCountry.DataSource = CountryTbl
        ddlCountry.DataBind()


        Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
        orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)

        Dim s() As String = Nothing
        Dim Currency As String = Nothing
        Dim DecimalDigits As String = "2"
        Dim country As String = Nothing

        ddlOrganization.ClearCheckedItems()
        If CountryTbl.Rows.Count = 1 Then

            ddlCountry.SelectedIndex = 0
            dvCountry.Visible = False

            s = ddlCountry.SelectedValue.Split("$")

            If s.Length > 0 Then
                country = s(0).ToString()
                Currency = s(1).ToString()
                DecimalDigits = s(2).ToString()
            End If


            ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
            ddlOrganization.DataBind()


        ElseIf CountryTbl.Rows.Count > 1 Then
            ddlCountry.SelectedIndex = 0
            dvCountry.Visible = True


            s = ddlCountry.SelectedValue.Split("$")

            If s.Length > 0 Then
                country = s(0).ToString()
                Currency = s(1).ToString()
                DecimalDigits = s(2).ToString()
            End If

            ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
            ddlOrganization.DataBind()

        End If

        Dim OrgStr As String = Nothing
        For Each item As RadComboBoxItem In ddlOrganization.Items
            item.Checked = True
            If item.Checked Then

                OrgStr = OrgStr & "," & item.Value

            End If
        Next

        LoadOrgDetails()
        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")
        Args.Visible = False
        summary.InnerHtml = ""
        gvRep.Visible = False
        RepDiv.Visible = False

    End Sub

    Private Sub ddlOrganization_CheckAllCheck(sender As Object, e As RadComboBoxCheckAllCheckEventArgs) Handles ddlOrganization.CheckAllCheck
        LoadOrgDetails()
    End Sub

    Private Sub ddlOrganization_ItemChecked(sender As Object, e As RadComboBoxItemEventArgs) Handles ddlOrganization.ItemChecked
        LoadOrgDetails()
    End Sub
End Class