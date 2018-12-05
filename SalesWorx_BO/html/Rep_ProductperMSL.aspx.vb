Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Partial Public Class Rep_ProductperMSL
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "ProductivityPerMSLItems"

    Private Const PageID As String = "P118"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single
    Dim dtCust As New DataTable

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

                dtCust.Columns.Add("ID")
                dtCust.Columns.Add("Desc")
                ViewState("dtCust") = dtCust

                ObjCommon = New SalesWorx.BO.Common.Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))
                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                End If
                LoadOrgDetails()



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
        Else
            LoadProdcut()
        End If
    End Sub
    Sub LoadProdcut()
        ObjCommon = New SalesWorx.BO.Common.Common()
        Dim dt As New DataTable
        dt = ObjCommon.GetProductsByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue)
        ddl_Product.DataSource = dt
        ddl_Product.DataTextField = "Description"
        ddl_Product.DataValueField = "Inventory_Item_ID"
        ddl_Product.DataBind()
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
    Sub BindReport()
        If Not ddlOrganization.SelectedItem Is Nothing Then
            Dim collection As IList(Of RadComboBoxItem) = ddl_Van.CheckedItems
            rpbFilter.Items(0).Expanded = False

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

            lbl_org.Text = ddlOrganization.SelectedItem.Text

            Args.Visible = True

            HSID.Value = van

            hfOrg.Value = ddlOrganization.SelectedItem.Value

            dtCust = CType(ViewState("dtCust"), DataTable)
            Dim Invid As String = ""
            If dtCust.Rows.Count > 0 Then
                For Each dr In dtCust.Rows

                    Invid = Invid & dr("ID").ToString & ","
                    lbl_Product.Text = lbl_Product.Text & dr("Desc").ToString & ","
                Next
                Invid = Invid.Substring(0, Invid.Length - 1)

                lbl_Product.Text = lbl_Product.Text.Substring(0, lbl_Product.Text.Length - 1)
            Else
                Invid = "0"
                lbl_Product.Text = "All"
            End If

            ItemID.Value = Invid

            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetProductivityperMSL(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, van, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, Invid)

            

            Dim dtFinal As New DataTable

            dtFinal.Columns.Add("Description")
            dtFinal.Columns.Add("Type")
            dtFinal.Columns.Add("Year")
            dtFinal.Columns.Add("C1", Type.GetType("System.Decimal"))
            dtFinal.Columns.Add("SID")
            For Each dr In dt.Rows

                Dim fCustdr2 As DataRow
                fCustdr2 = dtFinal.NewRow()
                fCustdr2("Description") = dr("Product").ToString
                fCustdr2("Type") = "Invoices"
                fCustdr2("Year") = DateAdd(DateInterval.Month, -2, Now).ToString("MMM-yyyy")
                fCustdr2("C1") = Val(dr("IM2").ToString)

                dtFinal.Rows.Add(fCustdr2)

                Dim fVisitdr2 As DataRow
                fVisitdr2 = dtFinal.NewRow()
                fVisitdr2("Description") = dr("Product").ToString
                fVisitdr2("Type") = "Unique Outlets"
                fVisitdr2("Year") = DateAdd(DateInterval.Month, -2, Now).ToString("MMM-yyyy")
                fVisitdr2("C1") = Val(dr("CM2").ToString)

                dtFinal.Rows.Add(fVisitdr2)



                Dim fprodcutivity As DataRow
                fprodcutivity = dtFinal.NewRow()
                fprodcutivity("Description") = dr("Product").ToString
                fprodcutivity("Type") = "Productivity %"
                fprodcutivity("Year") = DateAdd(DateInterval.Month, -2, Now).ToString("MMM-yyyy")
                fprodcutivity("C1") = Val(dr("PM2").ToString)

                dtFinal.Rows.Add(fprodcutivity)

                Dim fCustdr1 As DataRow
                fCustdr1 = dtFinal.NewRow()
                fCustdr1("Description") = dr("Product").ToString
                fCustdr1("Type") = "Invoices"
                fCustdr1("Year") = DateAdd(DateInterval.Month, -1, Now).ToString("MMM-yyyy")
                fCustdr1("C1") = Val(dr("IM1").ToString)

                dtFinal.Rows.Add(fCustdr1)

                Dim fVisitdr1 As DataRow
                fVisitdr1 = dtFinal.NewRow()
                fVisitdr1("Description") = dr("Product").ToString
                fVisitdr1("Type") = "Unique Outlets"
                fVisitdr1("Year") = DateAdd(DateInterval.Month, -1, Now).ToString("MMM-yyyy")
                fVisitdr1("C1") = Val(dr("CM1").ToString)

                dtFinal.Rows.Add(fVisitdr1)

                Dim fprodcutivity1 As DataRow
                fprodcutivity1 = dtFinal.NewRow()
                fprodcutivity1("Description") = dr("Product").ToString
                fprodcutivity1("Type") = "Productivity %"
                fprodcutivity1("Year") = DateAdd(DateInterval.Month, -1, Now).ToString("MMM-yyyy")
                fprodcutivity1("C1") = Val(dr("PM1").ToString)

                dtFinal.Rows.Add(fprodcutivity1)

                Dim fCustdr As DataRow
                fCustdr = dtFinal.NewRow()
                fCustdr("Description") = dr("Product").ToString
                fCustdr("Type") = "Invoices"
                fCustdr("Year") = Now.ToString("MMM-yyyy")
                fCustdr("C1") = Val(dr("IM").ToString)

                dtFinal.Rows.Add(fCustdr)

                Dim fVisitdr As DataRow
                fVisitdr = dtFinal.NewRow()
                fVisitdr("Description") = dr("Product").ToString
                fVisitdr("Type") = "Unique Outlets"
                fVisitdr("Year") = Now.ToString("MMM-yyyy")
                fVisitdr("C1") = Val(dr("CM").ToString)

                dtFinal.Rows.Add(fVisitdr)

                Dim fprodcutivity2 As DataRow
                fprodcutivity2 = dtFinal.NewRow()
                fprodcutivity2("Description") = dr("Product").ToString
                fprodcutivity2("Type") = "Productivity %"
                fprodcutivity2("Year") = Now.ToString("MMM-yyyy")
                fprodcutivity2("C1") = Val(dr("PM").ToString)

                dtFinal.Rows.Add(fprodcutivity2)

            Next

            ViewState("dtFinal") = dtFinal
            gvRep.DataSource = dtFinal
            gvRep.DataBind()

            If dt.Rows.Count > 5 Then
                HTitle.Value = "Top 5 products"
            Else
                HTitle.Value = ""
            End If

            HM.Value = Now.ToString("MMM-yyyy")
            HM1.Value = DateAdd(DateInterval.Month, -1, Now).ToString("MMM-yyyy")
            HM2.Value = DateAdd(DateInterval.Month, -2, Now).ToString("MMM-yyyy")


            Dim dtChart As New DataTable
            Dim dtTop5 As New DataTable

            
            dtChart.Columns.Add("Product")
            dtChart.Columns.Add("M", Type.GetType("System.Decimal"))
            dtChart.Columns.Add("M1", Type.GetType("System.Decimal"))
            dtChart.Columns.Add("M2", Type.GetType("System.Decimal"))
            dtChart.Columns.Add("Tot", Type.GetType("System.Decimal"))
            ProdChart.ChartTitle.Text = "Productivity % - " & HTitle.Value

            ProdChart.PlotArea.Series(0).Name = DateAdd(DateInterval.Month, -2, Now).ToString("MMM-yyyy")
            ProdChart.PlotArea.Series(0).TooltipsAppearance.ClientTemplate = " Productivity % " & ProdChart.PlotArea.Series(0).Name & " :   #=dataItem.M#"
            ProdChart.PlotArea.Series(1).Name = DateAdd(DateInterval.Month, -1, Now).ToString("MMM-yyyy")
            ProdChart.PlotArea.Series(1).TooltipsAppearance.ClientTemplate = " Productivity % " & ProdChart.PlotArea.Series(1).Name & " :   #=dataItem.M1#"
            ProdChart.PlotArea.Series(2).Name = Now.ToString("MMM-yyyy")
            ProdChart.PlotArea.Series(2).TooltipsAppearance.ClientTemplate = " Productivity % " & ProdChart.PlotArea.Series(2).Name & " :   #=dataItem.M2#"

            For Each dr As DataRow In dt.Rows
                Dim drChart As DataRow
                drChart = dtChart.NewRow
                drChart("Product") = dr("Product").ToString
                drChart("M") = Val(dr("PM2").ToString)
                drChart("M1") = Val(dr("PM1").ToString)
                drChart("M2") = Val(dr("PM").ToString)
                drChart("Tot") = Val(dr("PM2").ToString) + Val(dr("PM2").ToString) + Val(dr("PM").ToString)
                dtChart.Rows.Add(drChart)
            Next


            If dtChart.Rows.Count > 5 Then
                dtTop5 = dtChart.Rows.Cast(Of DataRow)().OrderByDescending(Function(x) x("Tot")).Take(5).CopyToDataTable
            Else
                dtTop5 = dtChart.Copy
            End If

            ProdChart.DataSource = dtTop5
            ProdChart.DataBind()





            ' ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "UpdateHeader();", True)
            'ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)

        End If
    End Sub

    Private Sub gvRep_CellDataBound(sender As Object, e As PivotGridCellDataBoundEventArgs) Handles gvRep.CellDataBound
        If TypeOf e.Cell Is PivotGridColumnHeaderCell Then
            Dim txt As String = ""
            If e.Cell.Controls.Count = 2 Then
                txt = CType(e.Cell.Controls(1), Literal).Text
            Else
                txt = e.Cell.Text
            End If

            If txt = "Invoices" Then
                e.Cell.Attributes.Add("style", "background-image: none")
                e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#97c95d")
                e.Cell.ForeColor = Drawing.Color.White
            ElseIf txt = "Unique Outlets" Then
                e.Cell.Attributes.Add("style", "background-image: none")
                e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#14b4fc")
                e.Cell.ForeColor = Drawing.Color.White
            ElseIf txt = "Productivity %" Then
                e.Cell.Attributes.Add("style", "background-image: none")
                e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#ef9933")
                e.Cell.ForeColor = Drawing.Color.White
            End If
        End If

        'If TypeOf e.Cell Is PivotGridRowHeaderCell Then
        '    Dim cell As PivotGridRowHeaderCell = TryCast(e.Cell, PivotGridRowHeaderCell)
        '    If cell.Field.DataField = "Description" Then
        '        Dim dtFinal As New DataTable
        '        If Not ViewState("dtFinal") Is Nothing Then
        '            dtFinal = CType(ViewState("dtFinal"), DataTable)
        '            Dim Selrow() As DataRow
        '            Dim lnk As LinkButton
        '            lnk = CType(cell.Controls(1), LinkButton)
        '            Selrow = dtFinal.Select("Description='" & lnk.Text & "'")
        '            If Selrow.Length > 0 Then
        '                lnk.ID = lnk.ID & "__" & Selrow(0)("SID")
        '            End If
        '        End If
        '    End If
        'End If
        If TypeOf e.Cell Is PivotGridDataCell Then

            Dim cell As PivotGridDataCell = TryCast(e.Cell, PivotGridDataCell)

            If Not cell Is Nothing Then
                If cell.Field.DataField = "C1" Then
                    If cell.ColumnIndex < 6 Then
                        cell.Text = Format(Val(cell.Text), "#,##0")
                    End If
                End If
            End If
        End If

        '        If cell.Field.DataField = "C5" Then
        '            If cell.ColumnIndex = 12 Then
        '                cell.Width = 0
        '                cell.Visible = False
        '                cell.Attributes.Add("style", "display:none")
        '            End If
        '        End If
        '        If cell.Field.DataField = "C6" Then
        '            If cell.ColumnIndex = 13 Then
        '                cell.Width = 0
        '                cell.Visible = False
        '                cell.Attributes.Add("style", "display:none")
        '            End If
        '        End If
        '        If cell.Field.DataField = "C7" Then
        '            If cell.ColumnIndex = 14 Then
        '                cell.Width = 0
        '                cell.Visible = False
        '                cell.Attributes.Add("style", "display:none")
        '            End If
        '        End If
        '        If cell.Field.DataField = "C8" Then
        '            If cell.ColumnIndex = 15 Then
        '                cell.Width = 0
        '                cell.Visible = False
        '                cell.Attributes.Add("style", "display:none")
        '            End If
        '        End If
        '    End If
        'End If
    End Sub

    Private Sub gvRep_PageIndexChanged(sender As Object, e As PivotGridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        BindReport()
    End Sub

    Private Sub gvRep_PageSizeChanged(sender As Object, e As PivotGridPageSizeChangedEventArgs) Handles gvRep.PageSizeChanged
        BindReport()
    End Sub

    Sub Export(format As String)


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


        dtCust = CType(ViewState("dtCust"), DataTable)
        Dim Invid As String = ""
        If dtCust.Rows.Count > 0 Then
            For Each dr In dtCust.Rows
                Invid = Invid & dr("ID").ToString & ","
            Next
            Invid = Invid.Substring(0, Invid.Length - 1)
        Else
            Invid = "0"
        End If

        ItemID.Value = Invid


        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)



        Dim USRID As New ReportParameter
        USRID = New ReportParameter("Uid", objUserAccess.UserID)


        Dim SalesRepID As New ReportParameter
        SalesRepID = New ReportParameter("SID", CStr(van))


        Dim OID As New ReportParameter
        OID = New ReportParameter("OID", CStr(ddlOrganization.SelectedValue.ToString()))


        Dim prod As New ReportParameter
        prod = New ReportParameter("Invid", Invid)

        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)

        rview.ServerReport.SetParameters(New ReportParameter() {USRID, OID, SalesRepID, prod})

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
            Response.AddHeader("Content-disposition", "attachment;filename=ProductivityperMSL.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=ProductivityperMSL.xls")
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
            Salestab.Visible = True
            BindReport()
        Else
            Args.Visible = False
            summary.InnerHtml = ""
            gvRep.Visible = False
            RepDiv.Visible = False
            Salestab.Visible = False
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
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        LoadOrgDetails()
    End Sub
    Sub LoadOrgDetails()
        If Not (ddlOrganization.SelectedItem.Value = "0") Then

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New SalesWorx.BO.Common.Common()


            ddl_Van.DataTextField = "SalesRep_Name"
            ddl_Van.DataValueField = "SalesRep_ID"
            ddl_Van.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)

            ddl_Van.DataBind()

            For Each itm As RadComboBoxItem In ddl_Van.Items
                itm.Checked = True
            Next

        Else
            ddl_Van.Items.Clear()
        End If

    End Sub


    Private Sub Salestab_TabClick(sender As Object, e As RadTabStripEventArgs) Handles Salestab.TabClick
        If Args.Visible = True Then
            If Salestab.Tabs(1).Selected = True Then
                ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "createChart3();", True)
            End If
            
        End If

    End Sub
    Protected Sub ddlCust_EntryAdded(sender As Object, e As AutoCompleteEntryEventArgs) Handles ddl_Product.EntryAdded
        dtCust = CType(ViewState("dtCust"), DataTable)
        Dim seldr() As DataRow
        seldr = dtCust.Select("ID='" & e.Entry.Value & "'")
        If seldr.Length <= 0 Then
            Dim dr As DataRow
            dr = dtCust.NewRow()
            dr(0) = e.Entry.Value
            dr(1) = e.Entry.Text
            dtCust.Rows.Add(dr)
        End If
        ViewState("dtCust") = dtCust
        Salestab.Visible = False
        gvRep.Visible = False
        Args.Visible = False
    End Sub
    Protected Sub ddlCust_EntryRemoved(sender As Object, e As AutoCompleteEntryEventArgs) Handles ddl_Product.EntryRemoved
        dtCust = CType(ViewState("dtCust"), DataTable)
        Dim dr() As DataRow
        dr = dtCust.Select("ID='" & e.Entry.Value & "'")
        If dr.Length > 0 Then
            dtCust.Rows.Remove(dr(0))
        End If
        ViewState("dtCust") = dtCust
        gvRep.Visible = False
        Args.Visible = False
        Salestab.Visible = False
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        ddl_Van.ClearCheckedItems()
        ddl_Van.Items.Clear()
        ddl_Product.Entries.Clear()
        Args.Visible = False
        summary.InnerHtml = ""
        gvRep.Visible = False
        RepDiv.Visible = False
        Salestab.Visible = False
    End Sub
End Class