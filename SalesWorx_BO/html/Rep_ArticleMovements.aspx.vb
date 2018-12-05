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
Partial Public Class Rep_ArticleMovements
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "ArticleMovements"

    Private Const PageID As String = "P100"
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
                'ViewState("DtItem") = dtItem
                ViewState("dtCust") = dtCust

                ObjCommon = New SalesWorx.BO.Common.Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))
                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                End If

                If Not Request.QueryString("ID") Is Nothing Then
                    txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                    txtToDate.SelectedDate = Now()

                    Dim dt As New DataTable
                    dt = (New SalesWorx.BO.Common.Common).GetSalesOrgbyFsr(Err_No, Err_Desc, Request.QueryString("ID"))
                    If dt.Rows.Count > 0 Then
                        If Not ddlOrganization.FindItemByValue(dt.Rows(0)("MAS_Org_ID").ToString) Is Nothing Then
                            ddlOrganization.ClearSelection()
                            ddlOrganization.FindItemByValue(dt.Rows(0)("MAS_Org_ID").ToString).Selected = True
                        End If
                    End If
                Else
                    txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                    txtToDate.SelectedDate = Now()
                End If

                LoadOrgDetails()
                If Not ddlVan.Items.FindItemByValue(Request.QueryString("ID")) Is Nothing Then
                    ddlVan.ClearSelection()
                    ddlVan.Items.FindItemByValue(Request.QueryString("ID")).Selected = True
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

        Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems
        Dim van As String = ""
        For Each li As RadComboBoxItem In collection
            van = van & li.Value & ","

        Next

        If van = "" Then
            van = "0"
        End If


        Dim USRID As New ReportParameter
        USRID = New ReportParameter("Uid", objUserAccess.UserID)


        Dim SalesRepID As New ReportParameter
        SalesRepID = New ReportParameter("SID", van)

        Dim FDate As New ReportParameter
        FDate = New ReportParameter("FromDate", CDate(txtFromDate.SelectedDate).ToString())

        Dim TDate As New ReportParameter
        TDate = New ReportParameter("ToDate", CDate(txtToDate.SelectedDate).ToString())


        Dim OID As New ReportParameter
        OID = New ReportParameter("OID", CStr(ddlOrganization.SelectedValue.ToString()))

        Dim DocType As New ReportParameter
        DocType = New ReportParameter("DocType", ddlDocument.SelectedItem.Value)

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

        Dim InvIDP As New ReportParameter
        InvIDP = New ReportParameter("InvID", Invid)


        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)

        rview.ServerReport.SetParameters(New ReportParameter() {USRID, OID, SalesRepID, FDate, TDate, DocType, InvIDP})

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
            Response.AddHeader("Content-disposition", "attachment;filename=ArticleMovement.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=ArticleMovement.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()

    End Sub
    Private Sub BindReport()
        If Not ddlOrganization.SelectedItem Is Nothing Then

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

            Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems
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
            lbl_org.Text = ddlOrganization.SelectedItem.Text
            lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_van.Text = vantxt

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

            Args.Visible = True

            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.ArtilcleMovement(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, van, objUserAccess.UserID, CDate(txtFromDate.SelectedDate), CDate(txtToDate.SelectedDate), ddlDocument.SelectedItem.Value, Invid)
            gvRep.DataSource = dt
            gvRep.DataBind()
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
        If ddlOrganization.SelectedIndex > 0 Then
            
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
    End Function
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            gvRep.Visible = True

            BindReport()
        Else
            Args.Visible = False
            gvRep.Visible = False

        End If
    End Sub
    Sub MessageBoxValidation(ByVal str As String, Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        LoadOrgDetails()
    End Sub
    Sub LoadOrgDetails()
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        ObjCommon = New SalesWorx.BO.Common.Common()
        ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
        ddlVan.DataBind()
        For Each itm As RadComboBoxItem In ddlVan.Items
            itm.Checked = True
        Next
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If

        If Not Request.QueryString("ID") Is Nothing Then
            txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
            txtToDate.SelectedDate = Now()

            Dim dt As New DataTable
            dt = (New SalesWorx.BO.Common.Common).GetSalesOrgbyFsr(Err_No, Err_Desc, Request.QueryString("ID"))
            If dt.Rows.Count > 0 Then
                If Not ddlOrganization.FindItemByValue(dt.Rows(0)("MAS_Org_ID").ToString) Is Nothing Then
                    ddlOrganization.ClearSelection()
                    ddlOrganization.FindItemByValue(dt.Rows(0)("MAS_Org_ID").ToString).Selected = True
                End If
            End If
        Else
            txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
            txtToDate.SelectedDate = Now()
        End If

        LoadOrgDetails()
        If Not ddlVan.Items.FindItemByValue(Request.QueryString("ID")) Is Nothing Then
            ddlVan.ClearSelection()
            ddlVan.Items.FindItemByValue(Request.QueryString("ID")).Selected = True
        End If
        ddlDocument.ClearSelection()
        ddl_Product.Entries.Clear()
        Args.Visible = False
        gvRep.Visible = False
    End Sub
End Class