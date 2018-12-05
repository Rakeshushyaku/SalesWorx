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
Public Class Rep_CollectionDetails
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                If Request.QueryString("ID") Is Nothing Then
                Else
                    HDocID.Value = Request.QueryString("ID")
                    LoadCollectionDetails(Request.QueryString("ID"))
                End If
            End If
        Catch ex As Exception
            log.Debug(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
    Sub LoadCollectionDetails(RowID As String)
        Try
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim DtHead As New DataTable
            Dim DtDetail As New DataTable

            Dim dtLPOimages As New DataTable



            DtHead = ObjReport.GetCollectionHeaderDetails(Err_No, Err_Desc, RowID)
            DtDetail = ObjReport.GetCollectionInvoiceDetails(Err_No, Err_Desc, RowID)


             
            If DtHead.Rows.Count > 0 Then
                Dim Currency As String
                Dim DecimalDigit As String = "2"
                Currency = " (" & DtHead.Rows(0)("Currency_Code") & ")"
                lblDecimal.Value = "N" & DtHead.Rows(0)("Decimal_Digits")
                DecimalDigit = DtHead.Rows(0)("Decimal_Digits")
                lblCurrency.Value = " (" + Currency + ")"

                dtLPOimages = ObjReport.GetChequeImages(Err_No, Err_Desc, RowID)
                lbl_refno.Text = DtHead.Rows(0)("Collection_Ref_No").ToString
                lbl_Date.Text = CDate(DtHead.Rows(0)("Collected_On").ToString).ToString("dd-MMM-yyyy hh:mm ss tt")
                lbl_Salesep.Text = DtHead.Rows(0)("Collected_By").ToString
                lbl_Status.Text = DtHead.Rows(0)("Status").ToString
                lbl_Amt.Text = FormatNumber(DtHead.Rows(0)("Amount").ToString, DecimalDigit)
             
                lbl_OrderType.Text = DtHead.Rows(0)("Collection_Type").ToString
                lbl_EmpCode.Text = DtHead.Rows(0)("Emp_Code").ToString

                lbl_Customer.Text = DtHead.Rows(0)("Customer").ToString
                lbl_Discount.Text = FormatNumber(DtHead.Rows(0)("Discount").ToString, DecimalDigit)
                lbl_Discountreason.Text = IIf((DtHead.Rows(0)("Discount_Reason").ToString).Trim = "", "N/A", DtHead.Rows(0)("Discount_Reason").ToString)

                lbl_Bank.Text = IIf(DtHead.Rows(0)("Bank_Name").ToString.Trim = "", "N/A", DtHead.Rows(0)("Bank_Name").ToString.Trim)
                If DtHead.Rows(0)("Collection_Type").ToString.ToUpper = "CC" Then
                    lbl_ChequeNohead.Text = "Card number"
                    lbl_bankbrHead.Text = "Approval code"
                    lbl_expDateHead.Text = "Expiry date"
                Else
                    lbl_ChequeNohead.Text = "Cheque No"
                    lbl_bankbrHead.Text = "Bank Branch"
                    lbl_expDateHead.Text = "Cheque Date"
                End If
                lbl_Cheque.Text = IIf(DtHead.Rows(0)("Cheque_No").ToString.Trim = "", "N/A", DtHead.Rows(0)("Cheque_No").ToString.Trim)
                lbl_ExpDate.Text = IIf(DtHead.Rows(0)("Cheque_Date").ToString.Trim = "", "N/A", DtHead.Rows(0)("Cheque_Date").ToString.Trim)

                 
                gvItems.DataSource = DtDetail
                gvItems.DataBind()
                 

                If dtLPOimages.Rows.Count > 0 Then
                    Dim dv As New DataView(dtLPOimages)
                    If ViewState("SortField") <> "" Then
                        dv.Sort = (ViewState("SortField") & " ") + SortDirection
                    End If

                    Me.ImgList.DataSource = dv
                    Me.ImgList.DataBind()
                Else
                    lbl_msgimg.Text = "No Check images are added to this collection."
                    Me.ImgList.DataSource = Nothing
                    Me.ImgList.DataBind()
                End If
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
        DtDetail = ObjReport.GetCollectionInvoiceDetails(Err_No, Err_Desc, HDocID.Value)

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
                If column.UniqueName = "DueAmount" Then
                    Dim col As GridBoundColumn
                    col = (CType(column, GridBoundColumn))
                    col.DataType = System.Type.GetType("System.Decimal")
                    col.DataFormatString = "{0:" & lblDecimal.Value & "}"
                    col.HeaderText = "Invoice Amount" + lblCurrency.Value
                    col.FooterAggregateFormatString = "{0:" & lblDecimal.Value & "}"
                End If
                If column.UniqueName = "Settlement_Amount" Then
                    Dim col As GridBoundColumn
                    col = (CType(column, GridBoundColumn))
                    col.DataType = System.Type.GetType("System.Decimal")
                    col.DataFormatString = "{0:" & lblDecimal.Value & "}"
                    col.HeaderText = "VAT Amount" + lblCurrency.Value
                    col.FooterAggregateFormatString = "{0:" & lblDecimal.Value & "}"
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
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), "CollectionDetails")
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)


 

        Dim RowID As New ReportParameter
        RowID = New ReportParameter("CollectionID", HDocID.Value)
 


        rview.ServerReport.SetParameters(New ReportParameter() {RowID})

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
            Response.AddHeader("Content-disposition", "attachment;filename=CollectionDetails.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=CollectionDetails.xls")
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