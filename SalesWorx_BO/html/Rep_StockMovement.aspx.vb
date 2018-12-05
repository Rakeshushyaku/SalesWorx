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
Public Class Rep_StockMovement
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "StockMovement"

    Private Const PageID As String = "P394"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single
    Public DeciVal As Integer = 2
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
                ObjCommon = New SalesWorx.BO.Common.Common

                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))

                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                End If

                Dim objUserAccess As UserAccess
                objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
                ObjCommon = New SalesWorx.BO.Common.Common()
                ddl_Van.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())
                ddl_Van.DataBind()
                For Each li As RadComboBoxItem In ddl_Van.Items
                    li.Checked = True
                Next

                txtToDate.SelectedDate = DateAdd(DateInterval.Day, -1, Now)
                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -30, Now)


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
    Private Sub ddl_item_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddl_item.ItemsRequested

        Dim strgency As String = "0"
        Dim strbrand As String = "0"

        Dim Objrep As New SalesWorx.BO.Common.Reports()
        Dim dt As New DataTable
        dt = Objrep.GetItemFromAgencyandBrand(Err_No, Err_Desc, ddlOrganization.SelectedValue, strgency, e.Text, strbrand)


        Dim ItemsPerRequest As Integer = 100 'Set the number of values to show
        Dim itemOffset As Integer = e.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
        e.EndOfItems = endOffset = dt.Rows.Count

        'Loop through the values to populate the combo box
        For i As Integer = itemOffset To endOffset - 1
            Dim item As New RadComboBoxItem()
            item.Text = dt.Rows(i).Item("Description").ToString
            item.Value = dt.Rows(i).Item("Inventory_Item_ID").ToString

            ddl_item.Items.Add(item)
            item.DataBind()
        Next

    End Sub


    Private Function ValidateInputs() As Boolean

        Dim SearchQuery As String = ""
        Try
            Dim bretval As Boolean = False

            If ddlOrganization.SelectedIndex > 0 Then
                If ddl_Van.SelectedIndex > 0 Then
                    bretval = True
                Else
                    MessageBoxValidation("Select a van.", "Validation")
                    Return bretval
                End If

                If Not IsDate(txtFromDate.SelectedDate) Then
                    MessageBoxValidation("Enter valid Date.", "Validation")
                    Return bretval
                    Exit Function
                End If
                If Not IsDate(txtToDate.SelectedDate) Then
                    MessageBoxValidation("Enter valid Date.", "Validation")
                    Return bretval
                    Exit Function
                End If
                If CDate(txtFromDate.SelectedDate) > CDate(txtToDate.SelectedDate) Then
                    MessageBoxValidation("Invalid date range selection.", "Validation")
                    Return bretval
                    Exit Function
                End If
                If Math.Abs(DateDiff(DateInterval.Day, CDate(txtFromDate.SelectedDate), CDate(txtToDate.SelectedDate))) > 30 Then
                    MessageBoxValidation("You cannot put a range selection of more than 30 days.", "Validation")
                    Return bretval
                    Exit Function
                End If
                Return True

            Else
                MessageBoxValidation("Select an Organization.", "Validation")
                Return bretval
            End If
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
    End Function
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Private Sub BindReport()
        Try
            gvRep.Visible = True
            Dim SearchQuery As String = ""
            If Not (ddlOrganization.SelectedItem.Value = "0") Then

            Else
                MessageBoxValidation("Select an organization.", "Validation")
                Exit Sub
            End If

            rpbFilter.Items(0).Expanded = False
            Args.Visible = False

            Dim item As String = "0"
            If Not String.IsNullOrEmpty(ddl_item.SelectedValue) Then
                item = ddl_item.SelectedValue
                lbl_SKU.Text = (New SalesWorx.BO.Common.Product).GetItemName(Err_No, Err_Desc, ddl_item.SelectedValue, ddlOrganization.SelectedItem.Value)
            Else
                item = "0"
                lbl_SKU.Text = "All"
            End If

            lbl_org.Text = ddlOrganization.SelectedItem.Text



            Dim vantxt As String = ""
            Dim van As String = ""

            van = ddl_Van.SelectedItem.Value
            vantxt = ddl_Van.SelectedItem.Text

            If vantxt <> "" Then
                vantxt = vantxt.Substring(0, vantxt.Length - 1)
            End If
            If van = "" Then
                van = "0"
            End If

            lbl_From.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")

            lbl_Van.Text = vantxt

            Args.Visible = True

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim tdt As New DataTable
            'tdt = ObjReport.GetDailyStockReconciliation(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, van, item, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))
            tdt = ObjReport.GetStockMovementReport(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, van, item, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))


            gvRep.DataSource = tdt
            gvRep.DataBind()

        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub


    Private Sub SearchBtn_Click(sender As Object, e As EventArgs) Handles SearchBtn.Click
        gvRep.Visible = False
        If ValidateInputs() Then
            gvRep.Visible = True
            BindReport()


        Else
            Args.Visible = False
            gvRep.Visible = False

        End If

    End Sub

    Private Sub ddlOrganization_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlOrganization.SelectedIndexChanged

        LoadOrgDetails()
    End Sub
    Sub LoadOrgDetails()
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        ObjCommon = New SalesWorx.BO.Common.Common()
        ddl_Van.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())
        ddl_Van.DataBind()
        For Each li As RadComboBoxItem In ddl_Van.Items
            li.Checked = True
        Next
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



            Dim fromdate As String
            fromdate = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")

            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("OrgID", ddlOrganization.SelectedItem.Value)

            Dim van As String = ""
            Dim collection As IList(Of RadComboBoxItem) = ddl_Van.CheckedItems
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","

            Next

            If van = "" Then
                van = "0"
            End If

            Dim FDate As New ReportParameter
            FDate = New ReportParameter("Fromdate", fromdate)

            Dim Todate As New ReportParameter
            Todate = New ReportParameter("Todate", CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))

            Dim SID As New ReportParameter
            SID = New ReportParameter("SID", van)


            Dim item As String = "0"
            If Not String.IsNullOrEmpty(ddl_item.SelectedValue) Then
                item = ddl_item.SelectedValue
            Else
                item = "0"
            End If

            Dim ItemID As New ReportParameter
            ItemID = New ReportParameter("Itemid", item)




            rview.ServerReport.SetParameters(New ReportParameter() {FDate, Todate, OrgId, SID, ItemID})

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
                Response.AddHeader("Content-disposition", "attachment;filename=DailyStockReport.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=DailyStockReport.xls")
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

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click

        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1, Now)
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        Args.Visible = False
        ddl_Van.ClearCheckedItems()
        ddl_Van.Items.Clear()
        ddl_item.Text = "Please type product code/ name"



        If Not (ddlOrganization.SelectedItem.Value = "0") Then
            LoadOrgDetails()
        Else
            gvRep.Visible = False
        End If
    End Sub

    Private Sub gvRep_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        BindReport()
    End Sub

    Private Sub gvRep_SortCommand(sender As Object, e As GridSortCommandEventArgs) Handles gvRep.SortCommand
        BindReport()
    End Sub
 

End Class