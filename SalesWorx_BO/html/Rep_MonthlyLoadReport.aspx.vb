Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI

Partial Public Class Rep_MonthlyLoadReport
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "MonthlyLoadReport"

    Private Const PageID As String = "P317"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single

    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private Sub RepRouteMaster_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ' ''If Not IsNothing(Me.Master) Then

        ' ''    Dim masterScriptManager As ScriptManager
        ' ''    masterScriptManager = CType(Master.FindControl("ScriptManager1"), ScriptManager)

        ' ''    ' Make sure our master page has the script manager we're looking for
        ' ''    If Not IsNothing(masterScriptManager) Then

        ' ''        ' Turn off partial page postbacks for this page
        ' ''        masterScriptManager.EnablePartialRendering = False
        ' ''    End If

        ' ''End If

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
                ddlOrganization.DataTextField = "Description"
                ddlOrganization.DataValueField = "MAS_Org_ID"
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization"))

                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                    LoadVan()
                End If

                txtFromDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")

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
    Private Sub LoadVan()
        Try
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New SalesWorx.BO.Common.Common()


            ddl_Van.DataTextField = "SalesRep_Name"
            ddl_Van.DataValueField = "SalesRep_ID"
            ddl_Van.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
            ddl_Van.DataBind()
            ddl_Van.Items.Insert(0, New RadComboBoxItem("Select Van", "0"))

        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
    Protected Sub Chk_Organization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        If Not (ddlOrganization.SelectedItem.Value = "Select Organization") Then
            LoadVan()

        Else
            ddl_Van.ClearSelection()
            ddl_Van.Items.Clear()
            ddl_Van.Text = ""
            ddl_Van.Items.Insert(0, New RadComboBoxItem("Select Van", "0"))
        End If
        ' ''RVMain.Reset()
        Rptdiv.Visible = False
    End Sub
    ' ''Sub LoadMonth()
    ' ''    ddl_Month.Items.Clear()
    ' ''    For i As Integer = 1 To 12
    ' ''        ddl_Month.Items.Add(New ListItem(CDate(i.ToString & "/01/" & Now.Year).ToString("MMM"), i))
    ' ''    Next
    ' ''End Sub
    ' ''Sub loadYear()
    ' ''    Dim objCommon As New Common

    ' ''    ddl_year.DataSource = objCommon.GetYearforLoad(Err_No, Err_Desc)
    ' ''    ddl_year.DataTextField = "Yr"
    ' ''    ddl_year.DataValueField = "Yr"
    ' ''    ddl_year.DataBind()
    ' ''    objCommon = Nothing
    ' ''End Sub


    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False

        Dim d1 As DateTime = txtFromDate.SelectedDate

        If (ddlOrganization.SelectedValue = "0" Or ddlOrganization.SelectedValue = "") Then
            MessageBoxValidation("Please select the Organisation", "Validation")
            Return bretval
        ElseIf (ddl_Van.SelectedValue = "0" Or ddl_Van.SelectedValue = "") Then
            MessageBoxValidation("Please select the Van", "Validation")
            Return bretval
        ElseIf Not IsDate(txtFromDate.SelectedDate) Then
            MessageBoxValidation("Invalid Month", "Validation")
            Return bretval
        Else
            bretval = True
            Return bretval

        End If
    End Function
    Private Sub BindData()
        Dim SearchQuery As String = ""
        Try


            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetMonthlyVanLoad(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, FirstDayOfMonth(txtFromDate.SelectedDate), LastDayOfMonth(txtFromDate.SelectedDate), ddl_Van.SelectedValue)

            Dim dtFinal As New DataTable
            dtFinal.Columns.Add("RowID")
            dtFinal.Columns.Add("Item")
            dtFinal.Columns.Add("Day")
            dtFinal.Columns.Add("LoadQty", Type.GetType("System.Double"))
            'dtFinal.Columns.Add("StockUOM")



            '' **** BELOW CODE SNIPPET IS ADDED FOR REMOVING EMPTY ROW IN PIVOT CONTORL *********

            If dt.Select("Item <> ''").Count > 0 Then
                log.Info("prerna")
                log.Info(dt.Select("Item <> ''").Count)

                Dim itmTbl As DataTable = dt.Select("Item <> ''").CopyToDataTable()

                Dim FirstRow As DataRow = itmTbl.Rows(0)

                ' Placing the rows for dates those are not available
                Dim dat As DateTime = FirstDayOfMonth(txtFromDate.SelectedDate)
                While dat <= LastDayOfMonth(txtFromDate.SelectedDate)
                    Dim dRow() As DataRow = itmTbl.Select("ReqDate = '" & dat & "'")
                    If dRow.Length = 0 Then
                        Dim dr As DataRow
                        dr = itmTbl.NewRow
                        dr("RowID") = 0
                        dr("Item") = FirstRow("Item")
                        'dr("StockUOM") = FirstRow("StockUOM")
                        dr("ReqDate") = dat
                        dr("LoadQty") = 0
                        itmTbl.Rows.Add(dr)
                        itmTbl.AcceptChanges()
                    End If
                    dat = dat.AddDays(1)
                End While

                For Each seldr As DataRow In itmTbl.Rows
                    Dim dr As DataRow
                    dr = dtFinal.NewRow
                    dr("RowID") = seldr("RowID")
                    dr("Item") = seldr("Item")
                    'dr("StockUOM") = seldr("StockUOM")
                    dr("Day") = CDate(seldr("ReqDate")).ToString("dd-MMM")
                    dr("LoadQty") = IIf(seldr("LoadQty") Is DBNull.Value, 0, seldr("LoadQty"))

                    dtFinal.Rows.Add(dr)
                Next

                dtFinal = dtFinal.Select("1=1", "Day ASC").CopyToDataTable()
            End If

            '' **** ENDS HERE *********

            gvRep.DataSource = dtFinal
            gvRep.DataBind()


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
    End Sub
    'Get the first day of the month
    Public Function FirstDayOfMonth(ByVal sourceDate As DateTime) As DateTime
        Return New DateTime(sourceDate.Year, sourceDate.Month, 1)
    End Function

    'Get the last day of the month
    Public Function LastDayOfMonth(ByVal sourceDate As DateTime) As DateTime
        Dim lastDay As DateTime = New DateTime(sourceDate.Year, sourceDate.Month, 1)
        Return lastDay.AddMonths(1).AddDays(-1)
    End Function
    Protected Sub SearchBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SearchBtn.Click
        Dim SHOW_UOM_MSG_BO_REPORTS As String = "N"
        Dim dt_app As New DataTable
        dt_app = (New SalesWorx.BO.Common.Common).GetAppControl(Err_No, Err_Desc, "SHOW_UOM_MSG_BO_REPORTS")
        If dt_app.Rows.Count > 0 Then
            SHOW_UOM_MSG_BO_REPORTS = dt_app.Rows(0)("Control_Value").ToString().ToUpper()
            If SHOW_UOM_MSG_BO_REPORTS = "Y" Then
                lblmsgUOM.Text = "All the quantities displayed in this report are in Stock UOM"
            Else
                lblmsgUOM.Text = ""

            End If
        End If
        Args.Visible = False
        gvRep.Visible = False

        If ValidateInputs() Then
            rpbFilter.Items(0).Expanded = False
            Args.Visible = True
            gvRep.Visible = True
            Rptdiv.Visible = True
            lbl_org.Text = ddlOrganization.SelectedItem.Text
            lbl_van.Text = ddl_Van.SelectedItem.Text
            lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("MMM-yyyy")
            BindData()
        Else
            Rptdiv.Visible = False
        End If

        ' ''If ddlOrganization.SelectedIndex <= 0 Then
        ' ''    MessageBoxValidation("Select an organization.")
        ' ''    Exit Sub
        ' ''End If

        ' ''If ddl_Van.SelectedItem.Value = "0" Then
        ' ''    MessageBoxValidation("Please select a van.")
        ' ''    Exit Sub
        ' ''End If



        ' ''Dim Fromdate As String
        ' ''Fromdate = CDate(ddl_Month.SelectedItem.Value & "/01/" & ddl_year.SelectedItem.Value).ToString("dd-MMM-yyyy")

        ' ''Dim Todate As String
        ' ''Todate = DateAdd(DateInterval.Second, -1, DateAdd(DateInterval.Month, 1, CDate(ddl_Month.SelectedItem.Value & "/01/" & ddl_year.SelectedItem.Value))).ToString("dd-MMM-yyyy")


        '' ''Dim OrgID As New ReportParameter
        '' ''OrgID = New ReportParameter("OID", ddlOrganization.SelectedValue)

        ' ''Dim VanID As New ReportParameter
        ' ''VanID = New ReportParameter("SID", ddl_Van.SelectedItem.Value)

        ' ''Dim Start_Date As New ReportParameter
        ' ''Start_Date = New ReportParameter("FromDate", Fromdate)

        ' ''Dim End_Date As New ReportParameter
        ' ''End_Date = New ReportParameter("Todate", Todate)

        ' ''Dim OrgID As New ReportParameter
        ' ''OrgID = New ReportParameter("OrgID", ddlOrganization.SelectedValue)


        ' ''With RVMain
        ' ''    .Reset()
        ' ''    .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        ' ''    .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        ' ''    .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
        ' ''    .ServerReport.SetParameters(New ReportParameter() {VanID, End_Date, OrgID, Start_Date})
        ' ''    '.ServerReport.Refresh()
        ' ''    .Visible = True
        ' ''End With
    End Sub
    Sub MessageBoxValidation(ByVal str As String, Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Private Sub gvRep_CellDataBound(sender As Object, e As PivotGridCellDataBoundEventArgs) Handles gvRep.CellDataBound
        Try
            If TypeOf e.Cell Is PivotGridDataCell Then
                Dim cell As PivotGridDataCell = TryCast(e.Cell, PivotGridDataCell)

                If cell.CellType = PivotGridDataCellType.DataCell OrElse cell.CellType = PivotGridDataCellType.RowTotalDataCell Then
                    If TryCast(cell.Field, PivotGridAggregateField).DataField = "LoadQty" Then
                        If cell.DataItem Is Nothing Then
                            cell.Text = "0.00"
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub gvRep_PageIndexChanged(sender As Object, e As PivotGridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        BindData()
    End Sub

    Private Sub BtnExportExcel_Click(sender As Object, e As EventArgs) Handles BtnExportExcel.Click
        Try
            If ValidateInputs() Then
                Export("Excel")
            End If
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub
    Sub Export(format As String)

        Try


            Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
            rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim Searchvalue As New ReportParameter

           

            'Dim OrgID As New ReportParameter
            'OrgID = New ReportParameter("OID", ddlOrganization.SelectedValue)

            Dim VanID As New ReportParameter
            VanID = New ReportParameter("SID", ddl_Van.SelectedItem.Value)

            Dim Start_Date As New ReportParameter
            Start_Date = New ReportParameter("FromDate", FirstDayOfMonth(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"))

            Dim End_Date As New ReportParameter
            End_Date = New ReportParameter("Todate", LastDayOfMonth(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"))

            Dim OrgID As New ReportParameter
            OrgID = New ReportParameter("OrgID", ddlOrganization.SelectedValue)

           

            rview.ServerReport.SetParameters(New ReportParameter() {VanID, End_Date, OrgID, Start_Date})

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
                Response.AddHeader("Content-disposition", "attachment;filename=MonthlyLoad.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=MonthlyLoad.xls")
                Response.AddHeader("Content-Length", bytes.Length)
            End If
            Response.OutputStream.Write(bytes, 0, bytes.Length)
            Response.OutputStream.Flush()
            Response.OutputStream.Close()
            Response.Flush()
            Response.Close()
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub BtnExportPDF_Click(sender As Object, e As EventArgs) Handles BtnExportPDF.Click
        Try
            If ValidateInputs() Then
                Export("PDF")
            End If
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        ddl_Van.ClearSelection()
        ddl_Van.Items.Clear()
        LoadVan()
        txtFromDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")
        Args.Visible = False
        Rptdiv.Visible = False
        gvRep.Visible = False
        lblmsgUOM.Text = ""
    End Sub
End Class