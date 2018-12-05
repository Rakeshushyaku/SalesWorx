Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Partial Public Class Rep_ProductiveCalls
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "ProductiveCalls"

    Private Const PageID As String = "P117"
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

            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetProductiveCalls(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, van, CType(Session.Item("USER_ACCESS"), UserAccess).UserID)

            If dt.Rows.Count > 8 And dt.Rows.Count < 14 Then
                Chartwrapper.Style.Add("width", (dt.Rows.Count * 48).ToString & "px")
                Chartwrapper1.Style.Add("width", (dt.Rows.Count * 48).ToString & "px")
            ElseIf dt.Rows.Count > 14 Then
                Chartwrapper.Style.Add("width", (dt.Rows.Count * 43).ToString & "px")
                Chartwrapper1.Style.Add("width", (dt.Rows.Count * 43).ToString & "px")
            Else
                Chartwrapper.Style.Add("width", "1000px")
                Chartwrapper1.Style.Add("width", "1000px")
            End If

            Dim dtFinal As New DataTable

            dtFinal.Columns.Add("Description")
            dtFinal.Columns.Add("Type")
            dtFinal.Columns.Add("Year")
            dtFinal.Columns.Add("C1", Type.GetType("System.Int32"))
            dtFinal.Columns.Add("SID")
            For Each dr In dt.Rows

                Dim fCustdr2 As DataRow
                fCustdr2 = dtFinal.NewRow()
                fCustdr2("Description") = dr("Van").ToString
                fCustdr2("Type") = "Customers"
                fCustdr2("Year") = DateAdd(DateInterval.Month, -2, Now).ToString("MMM-yyyy")
                fCustdr2("C1") = Val(dr("M2").ToString)
                fCustdr2("SID") = dr("CreatedBy").ToString
                dtFinal.Rows.Add(fCustdr2)

                Dim fVisitdr2 As DataRow
                fVisitdr2 = dtFinal.NewRow()
                fVisitdr2("Description") = dr("Van").ToString
                fVisitdr2("Type") = "Calls"
                fVisitdr2("Year") = DateAdd(DateInterval.Month, -2, Now).ToString("MMM-yyyy")
                fVisitdr2("C1") = Val(dr("V2").ToString)
                fVisitdr2("SID") = dr("CreatedBy").ToString
                dtFinal.Rows.Add(fVisitdr2)

                Dim fCustdr1 As DataRow
                fCustdr1 = dtFinal.NewRow()
                fCustdr1("Description") = dr("Van").ToString
                fCustdr1("Type") = "Customers"
                fCustdr1("Year") = DateAdd(DateInterval.Month, -1, Now).ToString("MMM-yyyy")
                fCustdr1("C1") = Val(dr("M1").ToString)
                fCustdr1("SID") = dr("CreatedBy").ToString
                dtFinal.Rows.Add(fCustdr1)

                Dim fVisitdr1 As DataRow
                fVisitdr1 = dtFinal.NewRow()
                fVisitdr1("Description") = dr("Van").ToString
                fVisitdr1("Type") = "Calls"
                fVisitdr1("Year") = DateAdd(DateInterval.Month, -1, Now).ToString("MMM-yyyy")
                fVisitdr1("C1") = Val(dr("V1").ToString)
                fVisitdr1("SID") = dr("CreatedBy").ToString
                dtFinal.Rows.Add(fVisitdr1)


                Dim fCustdr As DataRow
                fCustdr = dtFinal.NewRow()
                fCustdr("Description") = dr("Van").ToString
                fCustdr("Type") = "Customers"
                fCustdr("Year") = Now.ToString("MMM-yyyy")
                fCustdr("C1") = Val(dr("M").ToString)
                fCustdr("SID") = dr("CreatedBy").ToString
                dtFinal.Rows.Add(fCustdr)

                Dim fVisitdr As DataRow
                fVisitdr = dtFinal.NewRow()
                fVisitdr("Description") = dr("Van").ToString
                fVisitdr("Type") = "Calls"
                fVisitdr("Year") = Now.ToString("MMM-yyyy")
                fVisitdr("C1") = Val(dr("V").ToString)
                fVisitdr("SID") = dr("CreatedBy").ToString
                dtFinal.Rows.Add(fVisitdr)

            Next

            ViewState("dtFinal") = dtFinal
            gvRep.DataSource = dtFinal
            gvRep.DataBind()

            HM.Value = Now.ToString("MMM-yyyy")
            HM1.Value = DateAdd(DateInterval.Month, -1, Now).ToString("MMM-yyyy")
            HM2.Value = DateAdd(DateInterval.Month, -2, Now).ToString("MMM-yyyy")

            ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "UpdateHeader();", True)
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

            If txt = "Customers" Then
                e.Cell.Text = "Customers <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Number of unique customers who has been billed'></i>"
                e.Cell.Attributes.Add("style", "background-image: none")
                e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#97c95d")
                e.Cell.ForeColor = Drawing.Color.White
            ElseIf txt = "Calls" Then
                e.Cell.Text = "Calls <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Number of visits during which an invoice was generated'></i>"
                e.Cell.Attributes.Add("style", "background-image: none")
                e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#14b4fc")
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
        'If TypeOf e.Cell Is PivotGridDataCell Then

        '    Dim cell As PivotGridDataCell = TryCast(e.Cell, PivotGridDataCell)

        '    If Not cell Is Nothing Then

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

        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)



        Dim USRID As New ReportParameter
        USRID = New ReportParameter("Uid", objUserAccess.UserID)


        Dim SalesRepID As New ReportParameter
        SalesRepID = New ReportParameter("SID", CStr(van))


        Dim OID As New ReportParameter
        OID = New ReportParameter("OID", CStr(ddlOrganization.SelectedValue.ToString()))

        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)

        rview.ServerReport.SetParameters(New ReportParameter() {USRID, OID, SalesRepID})

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
            Response.AddHeader("Content-disposition", "attachment;filename=ProductiveCalls.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=ProductiveCalls.xls")
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
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
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

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        ddl_Van.ClearCheckedItems()
        ddl_Van.Items.Clear()
        LoadOrgDetails()
        Args.Visible = False
        summary.InnerHtml = ""
        gvRep.Visible = False
        RepDiv.Visible = False
    End Sub
End Class