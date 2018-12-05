
Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Partial Public Class RepPlannedCoverage
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "PlannedCoverage"

    Private Const PageID As String = "P255"
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
    Private Sub ddlOrganization_CheckAllCheck(sender As Object, e As RadComboBoxCheckAllCheckEventArgs) Handles ddlOrganization.CheckAllCheck
        LoadOrgDetails()
        RepDiv.Visible = False
    End Sub

    Private Sub ddlOrganization_ItemChecked(sender As Object, e As RadComboBoxItemEventArgs) Handles ddlOrganization.ItemChecked
        LoadOrgDetails()
        RepDiv.Visible = False
    End Sub
    Sub LoadOrgDetails()
        If Not ddlOrganization.CheckedItems Is Nothing Then
            If ddlOrganization.CheckedItems.Count > 0 Then
                Dim objUserAccess As UserAccess
                objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
                Dim ObjRep As SalesWorx.BO.Common.Reports = New SalesWorx.BO.Common.Reports()

                Dim OrgStr As String = ""
                For Each li As RadComboBoxItem In ddlOrganization.CheckedItems
                    OrgStr = OrgStr & li.Value & ","
                Next
                ddl_Van.DataSource = ObjRep.GetAllOrgVan(Err_No, Err_Desc, OrgStr, objUserAccess.UserID.ToString())
                ddl_Van.DataBind()

                For Each itm As RadComboBoxItem In ddl_Van.Items
                    itm.Checked = True
                Next

                Dim dtcurrency As DataTable
                Dim ObjReport As New SalesWorx.BO.Common.Reports
                dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.CheckedItems(0).Value)


            Else
                ddl_Van.Items.Clear()
            End If
        Else
            ddl_Van.Items.Clear()
        End If

    End Sub
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If ddlOrganization.CheckedItems Is Nothing Then
            MessageBoxValidation("Select an Organisation", "Validation")
            SetFocus(ddlOrganization)
            bretval = False
            Return bretval

        Else
            If ddlOrganization.CheckedItems.Count <= 0 Then
                MessageBoxValidation("Select an Organisation", "Validation")
                SetFocus(ddlOrganization)
                bretval = False
                Return bretval
            End If
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
        
    End Function
    Sub BindReport()
        If Not ddlOrganization.CheckedItems Is Nothing Then
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


            If van = "0" Then
                lbl_van.Text = "All"
            Else
                lbl_van.Text = vantxt
            End If

            If van = "0" Then
                For Each li As RadComboBoxItem In ddl_Van.Items
                    van = van & li.Value & ","
                Next
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
            dt = ObjReport.GetPlannedCoverage(Err_No, Err_Desc, Org, van, txtFromDate.SelectedDate, txtToDate.SelectedDate)

            If dt.Rows.Count > 8 And dt.Rows.Count < 14 Then
                Chartwrapper.Style.Add("width", (dt.Rows.Count * 45).ToString & "px")
                Chartwrapper1.Style.Add("width", (dt.Rows.Count * 45).ToString & "px")
            ElseIf dt.Rows.Count > 14 Then
                Chartwrapper.Style.Add("width", (dt.Rows.Count * 40).ToString & "px")
                Chartwrapper1.Style.Add("width", (dt.Rows.Count * 40).ToString & "px")
            ElseIf dt.Rows.Count > 0 And dt.Rows.Count < 4 Then
                Chartwrapper.Style.Add("width", "60%")
                Chartwrapper1.Style.Add("width", "40%")
            Else
                Chartwrapper.Style.Add("width", "100%")
                Chartwrapper1.Style.Add("width", "100%")

            End If

            Dim dtFinal As New DataTable
            dtFinal.Columns.Add("Description")
            dtFinal.Columns.Add("Type")
            dtFinal.Columns.Add("C1", Type.GetType("System.Int32"))
            dtFinal.Columns.Add("C2", Type.GetType("System.Int32"))
            dtFinal.Columns.Add("C3", Type.GetType("System.Int32"))
            dtFinal.Columns.Add("C4", Type.GetType("System.Decimal"))

            For Each dr In dt.Rows
                Dim fCustdr As DataRow
                fCustdr = dtFinal.NewRow()
                fCustdr("Description") = dr("salesrep_name").ToString
                fCustdr("Type") = "Customers"
                fCustdr("C1") = Val(dr("TotalCustomersPDA").ToString)
                fCustdr("C2") = Val(dr("PlannedCust").ToString)
                fCustdr("C3") = Val(dr("TotalCustomersPDA").ToString) - Val(dr("PlannedCust").ToString)
                If Val(dr("TotalCustomersPDA").ToString) <> 0 Then
                    fCustdr("C4") = Format((Val(dr("PlannedCust").ToString) / Val(dr("TotalCustomersPDA").ToString)) * 100.0, "#,###.00")
                Else
                    fCustdr("C4") = "0.00"
                End If
                dtFinal.Rows.Add(fCustdr)

                Dim fVisitdr As DataRow
                fVisitdr = dtFinal.NewRow()
                fVisitdr("Description") = dr("salesrep_name").ToString
                fVisitdr("Type") = "Calls"
                fVisitdr("C1") = Val(dr("PlannedVisited").ToString)
                fVisitdr("C2") = 0
                fVisitdr("C3") = 0
                If Val(dr("TotalWorkingDays").ToString) <> 0 Then
                    fVisitdr("C4") = Format(((Val(dr("PlannedVisited").ToString)) / Val(dr("TotalWorkingDays").ToString)), "#,###.00")
                Else
                    fVisitdr("C4") = "0.00"
                End If
                dtFinal.Rows.Add(fVisitdr)
            Next

            gvRep.DataSource = dtFinal
            gvRep.DataBind()

            ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "UpdateHeader();", True)
            'ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)

        End If
    End Sub

    Private Sub gvRep_CellDataBound(sender As Object, e As PivotGridCellDataBoundEventArgs) Handles gvRep.CellDataBound
        If TypeOf e.Cell Is PivotGridColumnHeaderCell Then
            If e.Cell.Text = "Customers" Then
                e.Cell.Attributes.Add("style", "background-image: none")
                e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#97c95d")
                e.Cell.ForeColor = Drawing.Color.White
            ElseIf e.Cell.Text = "Calls" Then
                e.Cell.Attributes.Add("style", "background-image: none")
                e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#14b4fc")
                e.Cell.ForeColor = Drawing.Color.White
            End If
        End If

        If TypeOf e.Cell Is PivotGridDataCell Then

            Dim cell As PivotGridDataCell = TryCast(e.Cell, PivotGridDataCell)
            If Not cell Is Nothing Then
                If cell.Field.DataField = "C2" Then
                    If cell.ColumnIndex = 5 Then
                        cell.Width = 0
                        cell.Visible = False
                        cell.Attributes.Add("style", "display:none")
                    End If
                End If
                If cell.Field.DataField = "C3" Then
                    If cell.ColumnIndex = 6 Then
                        cell.Width = 0
                        cell.Visible = False
                        cell.Attributes.Add("style", "display:none")
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub gvRep_PageIndexChanged(sender As Object, e As PivotGridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        BindReport()
    End Sub

    Private Sub gvRep_PageSizeChanged(sender As Object, e As PivotGridPageSizeChangedEventArgs) Handles gvRep.PageSizeChanged
        BindReport()
    End Sub

    Sub Export(format As String)


        Dim Fromdate As String
        Dim Todate As String
        Fromdate = CDate(txtFromDate.SelectedDate)
        Todate = CDate(txtToDate.SelectedDate)

        Dim collection As IList(Of RadComboBoxItem) = ddl_Van.CheckedItems
        Dim van As String = ""
        For Each li As RadComboBoxItem In collection
            van = van & li.Value & ","

        Next

        If van = "" Then
            van = "0"
        End If

        If van = "0" Then
            For Each li As RadComboBoxItem In ddl_Van.Items
                van = van & li.Value & ","
            Next
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


        Dim OrgID As New ReportParameter
        OrgID = New ReportParameter("OID", Org)

        Dim VanID As New ReportParameter
        VanID = New ReportParameter("SID", van)

        Dim Start_Date As New ReportParameter
        Start_Date = New ReportParameter("Dat", Fromdate)

        Dim End_Date As New ReportParameter
        End_Date = New ReportParameter("Dat1", Todate)

        Dim OrgName As New ReportParameter
        OrgName = New ReportParameter("OrgName", Orgtxt)

        Dim Type As New ReportParameter
        Type = New ReportParameter("Type", "C")

        
        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
         
        rview.ServerReport.SetParameters(New ReportParameter() {OrgID, VanID, Start_Date, End_Date, OrgName, Type})

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
            Response.AddHeader("Content-disposition", "attachment;filename=PlannedCoverage.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=PlannedCoverage.xls")
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
    Protected Sub SearchBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SearchBtn.Click
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
        'lblMessage.ForeColor = Drawing.Color.Red
        'lblinfo.Text = "Validation"
        'lblMessage.Text = str
        'MpInfoError.Show()
        'MpInfoError.Show()
        'Exit Sub
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
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
        ddl_Van.ClearCheckedItems()
        LoadOrgDetails()
        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")
        RepDiv.Visible = False
        Args.Visible = False
        summary.InnerHtml = ""
        gvRep.Visible = False
        RepDiv.Visible = False
    End Sub
End Class