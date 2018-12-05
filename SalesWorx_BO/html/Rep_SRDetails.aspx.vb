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
Public Class Rep_SRDetails
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                If Request.QueryString("ID") Is Nothing Or Request.QueryString("OrgID") Is Nothing Then
                Else
                    HOrgID.Value = Request.QueryString("OrgID")
                    HDocID.Value = Request.QueryString("ID") '
                    HType.Value = Request.QueryString("Type")
                    LoadCurrency()

                    LoadReturnDetail(Request.QueryString("ID"), Request.QueryString("OrgID"))
                End If
            End If
        Catch ex As Exception
            log.Debug(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub

    Sub LoadCurrency()
        Try
            Dim DtCur As New DataTable
            Dim ObjRept As New SalesWorx.BO.Common.Reports
            DtCur = ObjRept.GetCurrency(Err_No, Err_Desc, HOrgID.Value)
            If DtCur.Rows.Count > 0 Then
                lblDecimal.Value = "N" & DtCur.Rows(0)("Decimal_Digits")
                lblCurrency.Value = " (" & DtCur.Rows(0)("Currency_Code") & ")"
            End If
        Catch ex As Exception
            log.Debug(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
    Sub LoadReturnDetail(RowID As String, OrgId As String)
        Try
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim DtHead As New DataTable
            Dim DtDetail As New DataTable
            Dim DtDiscount As New DataTable
            Dim dtLPOimages As New DataTable
            Dim DtCurrency As New DataTable
            Dim dtfiletype As New DataTable

            DtHead = ObjReport.GetSRHeaderDetails(Err_No, Err_Desc, OrgId, RowID)
            DtDetail = ObjReport.GetSRItemDetails(Err_No, Err_Desc, OrgId, RowID)
            DtDetail.Columns.Add("Product")
            DtDetail.Columns.Add("NetTotal", System.Type.GetType("System.Double"))
            DtDiscount = ObjReport.GetDiscountDetails(Err_No, Err_Desc, OrgId, RowID, HType.Value)

            DtCurrency = ObjReport.GetCurrency(Err_No, Err_Desc, OrgId)
            dtfiletype = (New SalesWorx.BO.Common.Common).GetFileTypes(Err_No, Err_Desc, "SIGNATURE")

            Dim Currency As String = ""
            Dim DecimalDigit As String = "2"
            If DtCurrency.Rows.Count > 0 Then
                Currency = " (" & DtCurrency.Rows(0)("Currency_Code") & ")"
                lblDecimal.Value = "N" & DtCurrency.Rows(0)("Decimal_Digits")
                DecimalDigit = DtCurrency.Rows(0)("Decimal_Digits")
                lblCurrency.Value = Currency
            End If
            If DtHead.Rows.Count > 0 Then
                dtLPOimages = ObjReport.GetLPOImages(Err_No, Err_Desc, OrgId, DtHead.Rows(0)("Orig_Sys_Document_Ref").ToString)
                lbl_refno.Text = DtHead.Rows(0)("Orig_Sys_Document_Ref").ToString
                lbl_Date.Text = CDate(DtHead.Rows(0)("Creation_Date").ToString).ToString("dd-MMM-yyyy")
                lbl_Salesep.Text = DtHead.Rows(0)("SalesRep_Name").ToString
                lbl_Status.Text = DtHead.Rows(0)("Order_Status").ToString

                lbl_Salesep.Text = DtHead.Rows(0)("SalesRep_Name").ToString
                lbl_Status.Text = DtHead.Rows(0)("Order_Status").ToString




                lbl_Customer.Text = DtHead.Rows(0)("Ship_To_Customer").ToString
                lbl_creditCustomername.Text = IIf(DtHead.Rows(0)("CreditCustomer").ToString.Trim = "", "N/A", DtHead.Rows(0)("CreditCustomer").ToString.Trim)
                lbl_CCName.Text = IIf((DtHead.Rows(0)("CCName").ToString + DtHead.Rows(0)("CCTelNo").ToString).Trim = "", "N/A", DtHead.Rows(0)("CCName").ToString + DtHead.Rows(0)("CCTelNo").ToString)




                If DtDiscount.Rows.Count > 0 Then
                    lbl_discount.Text = FormatNumber(Val(DtDiscount.Rows(0)("Discount").ToString), DecimalDigit)
                Else
                    lbl_discount.Text = "0.00"
                End If
                Dim total As Decimal = 0
                Dim vattotal As Decimal = 0
                For Each dr As DataRow In DtDetail.Rows
                    dr("Product") = dr("Item_Code").ToString + dr("Description").ToString
                    dr("NetTotal") = Val(dr("Net_Total_Price").ToString) + Val(dr("VAT_Amount").ToString)
                Next
                gvItems.DataSource = DtDetail
                gvItems.DataBind()
                If DtDetail.Rows.Count > 0 Then
                    Dim sumVal = DtDetail.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("Net_Total_Price")))
                    Dim sumVAt = DtDetail.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("VAT_Amount")))
                    total = Val(sumVal.ToString)
                    vattotal = Val(sumVAt.ToString)
                End If


                lbl_Net.Text = FormatNumber(total, DecimalDigit)
                lbl_vatAmount.Text = FormatNumber(vattotal, DecimalDigit)
                lbl_total.Text = FormatNumber(total + vattotal, DecimalDigit)
                lbl_StartTime.Text = IIf(DtHead.Rows(0)("Start_Time").ToString.Trim = "", "N/A", CDate(DtHead.Rows(0)("Start_Time").ToString.Trim).ToString("HH:mm"))
                lbl_EndTime.Text = IIf(DtHead.Rows(0)("End_Time").ToString.Trim = "", "N/A", CDate(DtHead.Rows(0)("End_Time").ToString.Trim).ToString("HH:mm"))
                lbl_Amount.Text = DtCurrency.Rows(0)("Currency_Code") + " " + FormatNumber(Val(DtHead.Rows(0)("Order_Amt").ToString), DecimalDigit)
                lbl_InvoiceRefNo.Text = IIf(DtHead.Rows(0)("Invoice_Ref_No").ToString.Trim = "", "N/A", DtHead.Rows(0)("Invoice_Ref_No").ToString.Trim)
                lbl_Reason.Text = IIf(DtHead.Rows(0)("Reason").ToString.Trim = "", "N/A", DtHead.Rows(0)("Reason").ToString.Trim)
                lbl_Comments.Text = IIf(DtHead.Rows(0)("Internal_Notes").ToString.Trim = "", "N/A", DtHead.Rows(0)("Internal_Notes").ToString.Trim)
                lbl_CurrDesc.Text = IIf(DtHead.Rows(0)("CurrDesc").ToString.Trim = "", "N/A", DtHead.Rows(0)("CurrDesc").ToString.Trim)
                lbl_CustomerRefNo.Text = IIf(DtHead.Rows(0)("Customer_Ref_No").ToString.Trim = "", "N/A", DtHead.Rows(0)("Customer_Ref_No").ToString.Trim)
                lbl_Signed.Text = IIf(DtHead.Rows(0)("Signee_Name").ToString.Trim = "", "N/A", DtHead.Rows(0)("Signee_Name").ToString.Trim)
                Img_Signature.ImageUrl = dtfiletype(0)("Web_Path").ToString + "/" + DtHead.Rows(0)("Orig_Sys_Document_Ref").ToString + dtfiletype(0)("File_Ext").ToString
                Dim ClientCode As String
                ClientCode = (New SalesWorx.BO.Common.Common).GetAppConfig(Err_No, Err_Desc, "CLIENT_CODE")


                ' gvItems.MasterTableView.Columns.FindByUniqueName("ItemPrice").HeaderText = "Item Price" & lblCurrency.Value
                '  gvItems.MasterTableView.Columns.FindByUniqueName("VAT_Amount").HeaderText = "VAT Amount" & lblCurrency.Value
                ' gvItems.MasterTableView.Columns.FindByUniqueName("NetTotal").HeaderText = "Net Total Price(Discounted " & lblCurrency.Value & " )"

                If ClientCode = "GYMA" Then
                    gvItems.MasterTableView.Columns.FindByUniqueName("ExpiryDate").Visible = True
                Else
                    gvItems.MasterTableView.Columns.FindByUniqueName("ExpiryDate").Visible = False
                End If

                lbl_DiscountHead.Text = lbl_DiscountHead.Text + lblCurrency.Value
                lbl_NetHead.Text = lbl_NetHead.Text + lblCurrency.Value
                lbl_VATHead.Text = lbl_VATHead.Text + lblCurrency.Value
                lbl_TotalHead.Text = lbl_TotalHead.Text + lblCurrency.Value

            End If
        Catch ex As Exception
            log.Debug(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
    Sub BindItems()
        Try
            Dim ObjReport As New SalesWorx.BO.Common.Reports

            Dim DtDetail As New DataTable
            DtDetail = ObjReport.GetSRItemDetails(Err_No, Err_Desc, HOrgID.Value, HDocID.Value)

            DtDetail.Columns.Add("Product")
            DtDetail.Columns.Add("NetTotal", System.Type.GetType("System.Double"))

            For Each dr As DataRow In DtDetail.Rows
                dr("Product") = dr("Item_Code").ToString + dr("Description").ToString
                dr("NetTotal") = Val(dr("ItemPrice").ToString) + Val(dr("VAT_Amount").ToString)
            Next
            gvItems.DataSource = DtDetail
            gvItems.DataBind()
        Catch ex As Exception
            log.Debug(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvItems.PreRender
        Try
            For Each column As GridColumn In gvItems.MasterTableView.Columns
                If column.UniqueName = "ItemPrice" Then
                    Dim col As GridBoundColumn
                    col = (CType(column, GridBoundColumn))
                    col.DataType = System.Type.GetType("System.Double")
                    col.DataFormatString = "{0:" & lblDecimal.Value & "}"
                    Dim ClientCode As String
                    ClientCode = (New SalesWorx.BO.Common.Common).GetAppConfig(Err_No, Err_Desc, "CLIENT_CODE")
                    ' col.HeaderText = "Item Price" + lblCurrency.Value
                    col.HeaderText = "Item Price"
                End If
                If column.UniqueName = "VAT_Amount" Then
                    Dim col As GridBoundColumn
                    col = (CType(column, GridBoundColumn))
                    col.DataType = System.Type.GetType("System.Double")
                    col.DataFormatString = "{0:" & lblDecimal.Value & "}"
                    'col.HeaderText = "VAT Amount" + lblCurrency.Value
                    col.HeaderText = "VAT Amount"
                End If
                If column.UniqueName = "NetTotal" Then
                    Dim col As GridBoundColumn
                    col = (CType(column, GridBoundColumn))
                    col.DataType = System.Type.GetType("System.Double")
                    col.DataFormatString = "{0:" & lblDecimal.Value & "}"
                    'col.HeaderText = "Net Total Price(Discounted " + lblCurrency.Value
                    col.HeaderText = "Net Total Price(Discounted)"
                End If
            Next
        Catch ex As Exception
            log.Debug(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub dgv_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvItems.SortCommand
        Try
            ViewState("SortField") = e.SortExpression
            SortDirection = "flip"
            BindItems()
        Catch ex As Exception
            log.Debug(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
    Private Sub dgv_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvItems.PageIndexChanged
        Try
            BindItems()
        Catch ex As Exception
            log.Debug(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
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
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), "SRDetailed")
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)



        Dim OrgID As New ReportParameter
        OrgID = New ReportParameter("OrgID", HOrgID.Value)

        Dim RowID As New ReportParameter
        RowID = New ReportParameter("RowID", HDocID.Value)

        Dim ApprovalCode As New ReportParameter
        ApprovalCode = New ReportParameter("ApprovalCode", "")

        'Dim type As New ReportParameter
        'type = New ReportParameter("type", "R")


        rview.ServerReport.SetParameters(New ReportParameter() {RowID, OrgID, ApprovalCode})

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
            Response.AddHeader("Content-disposition", "attachment;filename=SRDetails.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=SRDetails.xls")
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

            Export("Excel")

        Catch ex As Exception
            log.Debug(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
    Private Sub BtnPDF_Click(sender As Object, e As EventArgs) Handles BtnExportPDF.Click
        Try

            Export("PDF")

        Catch ex As Exception
            log.Debug(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
End Class