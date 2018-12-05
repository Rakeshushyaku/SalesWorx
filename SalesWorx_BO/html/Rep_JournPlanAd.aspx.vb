Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Imports OfficeOpenXml
Partial Public Class Rep_JournPlanAd
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "JourneyPlanAdherance"

    Private Const PageID As String = "P104"
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
                HUseDistributionIncall.Value = CType(Session.Item("CONTROL_PARAMS"), ControlParams).USE_DISTR_IN_CALLS
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


                UId.Value = CType(Session("User_Access"), UserAccess).UserID

                 

                LoadOrgDetails()


                txtFromDate.SelectedDate = Now
                If Not (Request.QueryString("OrgID") Is Nothing And Request.QueryString("SID") Is Nothing And Request.QueryString("FrmDt") Is Nothing And Request.QueryString("Todt") Is Nothing) Then

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
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            gvRep.Visible = True
            M.Value = CDate(txtFromDate.SelectedDate).ToString("MMM-yyyy")
            M1.Value = DateAdd(DateInterval.Month, -1, CDate(txtFromDate.SelectedDate)).ToString("MMM-yyyy")
            M2.Value = DateAdd(DateInterval.Month, -2, CDate(txtFromDate.SelectedDate)).ToString("MMM-yyyy")
            BindReport()
            BindChart()
            RepDiv.Visible = True
        Else
            gvRep.Visible = False
            Args.Visible = False
            RepDiv.Visible = False
        End If
    End Sub

    Sub Export(format As String)
        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

        Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems

        Dim van As String = ""
        For Each li As RadComboBoxItem In collection
            van = van & li.Value & ","
        Next

        If van = "" Then
            van = "0"
        End If


        Dim USRID As New ReportParameter
        USRID = New ReportParameter("Uid", CType(Session.Item("USER_ACCESS"), UserAccess).UserID)


        Dim SalesRepID As New ReportParameter
        SalesRepID = New ReportParameter("SID", van)

        Dim Year As New ReportParameter
        Year = New ReportParameter("Year", CDate(txtFromDate.SelectedDate).Year)

        Dim Month As New ReportParameter
        Month = New ReportParameter("Month", CDate(txtFromDate.SelectedDate).Month)



        Dim Org As String = ""
        Dim Orgcollection As IList(Of RadComboBoxItem) = ddlOrganization.CheckedItems
        For Each li As RadComboBoxItem In Orgcollection
            Org = Org & li.Value & ","

        Next
         
        If Org = "" Then
            Org = "0"
        End If

        Dim OID As New ReportParameter
        OID = New ReportParameter("OID", Org)




        rview.ServerReport.SetParameters(New ReportParameter() {USRID, OID, SalesRepID, Year, Month})

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
            Response.AddHeader("Content-disposition", "attachment;filename=JPAdherence.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=JPAdherence.xls")
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
    Private Sub BindChart()

        ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)

    End Sub
    Private Sub gvRep_PageIndexChanged(sender As Object, e As PivotGridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        BindReport()
    End Sub

    Private Sub gvRep_PageSizeChanged(sender As Object, e As PivotGridPageSizeChangedEventArgs) Handles gvRep.PageSizeChanged
        BindReport()
    End Sub
    Private Sub BindReport()
        Try
            Dim SearchQuery As String = ""
            If ValidateInputs() Then
                rpbFilter.Items(0).Expanded = False

                Dim objUserAccess As UserAccess
                objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
                Dim ObjReport As New SalesWorx.BO.Common.Reports

              
                Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems

                Dim van As String = ""
                Dim vantxt As String = ""
                For Each li As RadComboBoxItem In Collection
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
                lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("MMM-yyyy")

                Args.Visible = True

                hfOrg.Value = Org
                hfVan.Value = van
                HFrom.Value = CDate(txtFromDate.SelectedDate).ToString("MMM-yyyy")

                Dim dt As New DataTable
                dt = ObjReport.GetJPAdherence(Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, Org, van, CDate(txtFromDate.SelectedDate).Month, CDate(txtFromDate.SelectedDate).Year)
                Dim dtFinal As New DataTable

                If dt.Rows.Count > 8 And dt.Rows.Count < 14 Then
                    Chartid.Style.Add("width", (dt.Rows.Count * 45).ToString & "px")
                ElseIf dt.Rows.Count > 14 Then
                    Chartid.Style.Add("width", (dt.Rows.Count * 40).ToString & "px")

                ElseIf dt.Rows.Count > 0 And dt.Rows.Count < 4 Then
                    Chartid.Style.Add("width", "60%")

                Else
                    Chartid.Style.Add("width", "100%")

                End If

                dtFinal.Columns.Add("salesRepName")
                dtFinal.Columns.Add("Year")
                dtFinal.Columns.Add("Planned", Type.GetType("System.Int32"))
                dtFinal.Columns.Add("Calls", Type.GetType("System.Int32"))
                dtFinal.Columns.Add("Adherence", Type.GetType("System.Double"))
                Dim seldate As DateTime
                seldate = CDate(txtFromDate.SelectedDate)

                If dt.Rows.Count Then
                    For Each dr As DataRow In dt.Rows
                        Dim drfinal As DataRow
                        drfinal = dtFinal.NewRow
                        drfinal("SalesRepName") = dr("Van")
                        drfinal("Planned") = dr("M2Planned")
                        drfinal("Calls") = dr("M2Visited")
                        drfinal("Adherence") = dr("M2percentage")
                        drfinal("Year") = DateAdd(DateInterval.Month, -2, seldate).ToString("MMM-yyyy")
                        
                        dtFinal.Rows.Add(drfinal)



                        Dim drfinal1 As DataRow
                        drfinal1 = dtFinal.NewRow
                        drfinal1("SalesRepName") = dr("Van")
                        drfinal1("Planned") = dr("M1Planned")
                        drfinal1("Calls") = dr("M1Visited")
                        drfinal1("Adherence") = dr("M1percentage")
                        drfinal1("Year") = DateAdd(DateInterval.Month, -1, seldate).ToString("MMM-yyyy")

                        dtFinal.Rows.Add(drfinal1)




                        Dim drfinal2 As DataRow
                        drfinal2 = dtFinal.NewRow
                        drfinal2("SalesRepName") = dr("Van")
                        drfinal2("Planned") = dr("MPlanned")
                        drfinal2("Calls") = dr("MVisited")
                        drfinal2("Adherence") = dr("Mpercentage")
                        drfinal2("Year") = (seldate).ToString("MMM-yyyy")


                        dtFinal.Rows.Add(drfinal2)
                    Next
                End If

                gvRep.DataSource = dtFinal
                gvRep.DataBind()

                If dtFinal.Rows.Count > 0 Then
                    img_export.Visible = True
                Else
                    img_export.Visible = False
                End If

            End If
        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
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
        bretval = True
        Return bretval
    End Function

    Private Sub ddlOrganization_CheckAllCheck(sender As Object, e As RadComboBoxCheckAllCheckEventArgs) Handles ddlOrganization.CheckAllCheck

        LoadOrgDetails()


        gvRep.Visible = False
        Args.Visible = False
        RepDiv.Visible = False
    End Sub

    Private Sub ddlOrganization_ItemChecked(sender As Object, e As RadComboBoxItemEventArgs) Handles ddlOrganization.ItemChecked

        LoadOrgDetails()


        gvRep.Visible = False
        Args.Visible = False
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
                ddlVan.DataSource = ObjRep.GetAllOrgVan(Err_No, Err_Desc, OrgStr, objUserAccess.UserID.ToString())
                ddlVan.DataBind()

                For Each itm As RadComboBoxItem In ddlVan.Items
                    itm.Checked = True
                Next

                Dim dtcurrency As DataTable
                Dim ObjReport As New SalesWorx.BO.Common.Reports
                dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.CheckedItems(0).Value)


            Else
                ddlVan.Items.Clear()
            End If
        Else
            ddlVan.Items.Clear()
        End If
    End Sub

    Private Sub gvRep_CellDataBound(sender As Object, e As PivotGridCellDataBoundEventArgs) Handles gvRep.CellDataBound
        If TypeOf e.Cell Is PivotGridColumnHeaderCell Then
            If e.Cell.Text = DateAdd(DateInterval.Month, -2, CDate(txtFromDate.SelectedDate)).ToString("MMM-yyyy") Then
                e.Cell.Attributes.Add("style", "background-image: none")
                e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#ff7663")
                e.Cell.ForeColor = System.Drawing.Color.White
            ElseIf e.Cell.Text = DateAdd(DateInterval.Month, -1, CDate(txtFromDate.SelectedDate)).ToString("MMM-yyyy") Then
                e.Cell.Attributes.Add("style", "background-image: none")
                e.Cell.Attributes.Add("style", "background-image: none")
                e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#10c4b2")
                e.Cell.ForeColor = System.Drawing.Color.White
            ElseIf e.Cell.Text = DateAdd(DateInterval.Month, 0, CDate(txtFromDate.SelectedDate)).ToString("MMM-yyyy") Then
                e.Cell.Attributes.Add("style", "background-image: none")
                e.Cell.Attributes.Add("style", "background-image: none")
                e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#ef9933")
                e.Cell.ForeColor = System.Drawing.Color.White
            End If
        End If
        If TypeOf e.Cell Is PivotGridColumnHeaderCell Then
            If e.Cell.Text.IndexOf("Sum of") >= 0 Then
                e.Cell.Text = e.Cell.Text.Replace("Sum of", "")
            End If
            'If e.Cell.DataItem.ToString = "Sum of Planned" Then

            '    e.Cell.Attributes.Add("style", "background-image: none")
            '    e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#ff7663")
            '    e.Cell.ForeColor = System.Drawing.Color.White
            'End If
            'If e.Cell.DataItem.ToString = "Sum of Calls" Then

            '    e.Cell.Attributes.Add("style", "background-image: none")
            '    e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#ef9933")
            '    e.Cell.ForeColor = System.Drawing.Color.White
            'End If
            'If e.Cell.DataItem.ToString = "Sum of Adherence" Then
            '    e.Cell.Text = "Adherence %"
            '    e.Cell.Attributes.Add("style", "background-image: none")
            '    e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#0090d9")
            '    e.Cell.ForeColor = System.Drawing.Color.White
            'End If
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
        ddlVan.ClearCheckedItems()
        LoadOrgDetails()
        txtFromDate.SelectedDate = Now
        RepDiv.Visible = False
        Args.Visible = False
        summary.InnerHtml = ""
        gvRep.Visible = False
        RepDiv.Visible = False
    End Sub

    Private Sub BtnExportBiffExcel_Click(sender As Object, e As EventArgs) Handles BtnExportBiffExcel.Click

        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        Dim ObjReport As New SalesWorx.BO.Common.Reports


        Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems

        Dim van As String = ""
        Dim vantxt As String = ""
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

        Dim Org As String = ""
        Dim Orgcollection As IList(Of RadComboBoxItem) = ddlOrganization.CheckedItems
        For Each li As RadComboBoxItem In Orgcollection
            Org = Org & li.Value & ","

        Next

        If Org = "" Then
            Org = "0"
        End If
        
        Dim dt As New DataTable
        dt = ObjReport.GetJPAdherence(Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, Org, van, CDate(txtFromDate.SelectedDate).Month, CDate(txtFromDate.SelectedDate).Year)
        Dim dtFinal As New DataTable




        dtFinal.Columns.Add("Description")
        dtFinal.Columns.Add("C1", Type.GetType("System.Int32"))
        dtFinal.Columns.Add("C2", Type.GetType("System.Int32"))
        dtFinal.Columns.Add("C3", Type.GetType("System.Int32"))
        dtFinal.Columns.Add("C4", Type.GetType("System.Int32"))
        dtFinal.Columns.Add("C5", Type.GetType("System.Int32"))
        dtFinal.Columns.Add("C6", Type.GetType("System.Int32"))
        dtFinal.Columns.Add("C7", Type.GetType("System.Decimal"))
        dtFinal.Columns.Add("C8", Type.GetType("System.Decimal"))
        dtFinal.Columns.Add("C9", Type.GetType("System.Decimal"))
      

        For Each dr In dt.Rows
            Dim fCustdr As DataRow
            fCustdr = dtFinal.NewRow()
            fCustdr("Description") = dr("Van").ToString
            fCustdr("C1") = Val(dr("M2Planned").ToString)
            fCustdr("C2") = Val(dr("M2Visited").ToString)
            fCustdr("C3") = Val(dr("M2percentage").ToString)
            fCustdr("C4") = Val(dr("M1Planned").ToString)
            fCustdr("C5") = Val(dr("M1Visited").ToString)
            fCustdr("C6") = Val(dr("M1percentage").ToString)
            fCustdr("C7") = Val(dr("MPlanned").ToString)
            fCustdr("C8") = Val(dr("MVisited").ToString)
            fCustdr("C9") = Val(dr("Mpercentage").ToString)
            dtFinal.Rows.Add(fCustdr)
        Next

        Dim seldate As DateTime
        seldate = CDate(txtFromDate.SelectedDate)

        If dtFinal.Rows.Count > 0 Then


            Using package As New ExcelPackage()
                ' add a new worksheet to the empty workbook
                Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
                Worksheet.Cells("A2").LoadFromDataTable(dtFinal, True)

                Worksheet.Cells("A2").Value = "Van"
                Worksheet.Cells("B2").Value = "Planned"
                Worksheet.Cells("C2").Value = "Calls"
                Worksheet.Cells("D2").Value = "Adherence %"
                Worksheet.Cells("E2").Value = "Planned"
                Worksheet.Cells("F2").Value = "Calls"
                Worksheet.Cells("G2").Value = "Adherence %"
                Worksheet.Cells("H2").Value = "Planned"
                Worksheet.Cells("I2").Value = "Calls"
                Worksheet.Cells("J2").Value = "Adherence %"
                
                Worksheet.Cells("B1:D1").Merge = True
                Worksheet.Cells("E1:G1").Merge = True
                Worksheet.Cells("H1:J1").Merge = True
                Worksheet.Cells("B1").Value = DateAdd(DateInterval.Month, -2, seldate).ToString("MMM-yyyy")
                Worksheet.Cells("E1").Value = DateAdd(DateInterval.Month, -1, seldate).ToString("MMM-yyyy")
                Worksheet.Cells("H1").Value = DateAdd(DateInterval.Month, 0, seldate).ToString("MMM-yyyy")

                Worksheet.Cells("B1").Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center
                Worksheet.Cells("E1").Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center
                Worksheet.Cells("H1").Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center

                Worksheet.Cells.AutoFitColumns()
                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename= OverAllCoverageReport.xlsx")

                Using MyMemoryStream As New MemoryStream()
                    package.SaveAs(MyMemoryStream)
                    MyMemoryStream.WriteTo(Response.OutputStream)
                    Response.AddHeader("Content-Length", MyMemoryStream.Length)
                    Response.Flush()
                    Response.Close()
                End Using
            End Using
        End If
    End Sub
End Class