Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Imports Telerik.Web.UI.GridExcelBuilder
Imports OfficeOpenXml
Imports Microsoft.ReportingServices.Rendering.ExcelRenderer
Imports System.Drawing

Partial Public Class Rep_TradeEffecSimpleDetail
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "ReportTradeEffectSimpleDetail"

    Private Const PageID As String = "P216"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single

    Dim org As String
    Dim sid As String
    Dim fdate As String
    Dim tdate As String
    Dim channel As String
    Dim mode As String
    Dim uid As Int16
    Dim Pid As Int16
    Dim dt As DataTable
    Dim dt_details As DataTable


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



                If ((Not Request.QueryString("Org") Is Nothing) And (Not Request.QueryString("Pid") Is Nothing)) Then
                    Pid = Request.QueryString("Pid").ToString()
                    org = Request.QueryString("Org").ToString()
                End If
                If ((Not Request.QueryString("Sid") Is Nothing) And (Not Request.QueryString("Channel") Is Nothing) And (Not Request.QueryString("Mode") Is Nothing) And (Not Request.QueryString("Fdate") Is Nothing) And (Not Request.QueryString("Tdate") Is Nothing)) Then
                    sid = Request.QueryString("Sid").ToString()
                    fdate = Request.QueryString("Fdate").ToString()
                    tdate = Request.QueryString("Tdate").ToString()
                    channel = Request.QueryString("Channel").ToString()
                    mode = Request.QueryString("Mode").ToString()
                End If









                ObjCommon = New SalesWorx.BO.Common.Common()

                dt = ObjCommon.GetHeadersForTradeEffects(Err_No, Err_Desc, org, sid, fdate, tdate, channel, mode, Pid, uid)
                dt_details = ObjCommon.GetDetailsForTradeEffects(Err_No, Err_Desc, org, sid, fdate, tdate, channel, mode, Pid, uid)

                ViewState("DT_header") = dt
                ViewState("Dt_Details") = dt_details

                If (dt.Rows.Count() > 0) Then

                    If ((Not dt.Rows(0)("Customer").ToString() Is Nothing)) Then
                        lblUcb.Text = dt.Rows(0)("Customer").ToString()
                    Else
                        lblUcb.Text = "0"
                    End If
                    If ((Not dt.Rows(0)("Invoices").ToString() Is Nothing)) Then
                        lblNui.Text = dt.Rows(0)("Invoices").ToString()
                    Else
                        lblNui.Text = "0"

                    End If

                    If ((Not dt.Rows(0)("SKUOrdered").ToString() Is Nothing)) Then
                        LblSkuF.Text = dt.Rows(0)("SKUOrdered").ToString()
                    Else
                        LblSkuF.Text = "0"
                    End If
                    If ((Not dt.Rows(0)("FOC_ordered").ToString() Is Nothing)) Then
                        LblSkuO.Text = dt.Rows(0)("FOC_ordered").ToString()
                    Else
                        LblSkuO.Text = "0"
                    End If
                Else
                    lblUcb.Text = "0"
                    lblNui.Text = "0"
                    LblSkuF.Text = "0"
                    LblSkuO.Text = "0"
                End If



                gvRep.DataSource = dt_details
                gvRep.DataBind()







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




    Private Sub gvRep_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles gvRep.ItemDataBound
        Try
            If TypeOf e.Item Is GridDataItem Then
                Dim item As GridDataItem = TryCast(e.Item, GridDataItem)
                For Each col As GridColumn In gvRep.MasterTableView.AutoGeneratedColumns

                Next
            End If



            If TypeOf e.Item Is GridGroupHeaderItem Then
                Dim item As GridGroupHeaderItem = CType(e.Item, GridGroupHeaderItem)
                Dim groupDataRow As DataRowView = CType(e.Item.DataItem, DataRowView)

                item.DataCell.Text = "Total From Quantity=" + groupDataRow("Total_From_Quantity").ToString() + "  ,Total To Quantity=" + groupDataRow("Total_To_Quantity").ToString() + " , Total Bonus Quantity=" + groupDataRow("Total_Bonus_Quantity").ToString() + ""
            End If

        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub gvRep_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        ' BindData()
    End Sub


    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub

#Region "Export functionaity"



    Private Sub BtnExportBiffExcel_Click(sender As Object, e As EventArgs) Handles BtnExportBiffExcel.Click

        Dim tblData As New DataTable
        Dim tblHeader As New DataTable

        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        Dim ObjReport As New SalesWorx.BO.Common.Reports




        Dim dttest As DataTable

        tblHeader = ViewState("DT_header")
        tblData = ViewState("Dt_Details")
        tblData.Columns.Remove("PlanID")
        tblData.Columns.Remove("PlanItemID")

        Dim distinctCounts = tblData.AsEnumerable().
    Select(Function(row) row.Field(Of String)("OrderItemCode")).
    Distinct().ToList()



        If tblData.Rows.Count > 0 Then

            Using package As New ExcelPackage()
                Dim Sheetname As String

                If (tblHeader.Rows.Count > 0) Then
                    Sheetname = tblHeader.Rows(0)("Name").ToString()
                Else
                    Sheetname = "Sheet1"
                End If
                Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add(Sheetname)
                Worksheet.Cells("7", "1").LoadFromDataTable(tblData, True)






                Worksheet.Column(9).Style.Numberformat.Format = "#,###.00"
                Worksheet.Column(10).Style.Numberformat.Format = "#,###.00"
                Worksheet.Column(11).Style.Numberformat.Format = "#,###.00"
                Worksheet.Column(12).Style.Numberformat.Format = "#,###.00"
                Worksheet.Cells.AutoFitColumns()

                Worksheet.Cells("1", "1").Style.WrapText = True
                Worksheet.Cells("1", "2").Style.WrapText = True
                Worksheet.Cells("1", "3").Style.WrapText = True
                Worksheet.Cells("1", "4").Style.WrapText = True


                Worksheet.Row(7).Style.Font.Bold = True
                Worksheet.Row(1).Style.Font.Bold = True
                Worksheet.Row(1).Style.Font.Size = 16

                Worksheet.Row(2).Style.Font.Bold = True

                Worksheet.Row(7).Style.Font.Color.SetColor(Color.Black)
                Worksheet.Row(3).Style.Font.Color.SetColor(Color.Black)


                Worksheet.Cells("1", "1", "1", "6").Merge = True

                Worksheet.Cells("1", "1").Value = tblHeader.Rows(0)("Name").ToString()

                Worksheet.Cells("3", "1").Value = "Unique Customers Billed"
                Worksheet.Cells("4", "1").Value = " Billed"
                Worksheet.Cells("3", "1", "4", "2").Merge = True
                ' Worksheet.Cells("4", "1", "4", "2").Merge = True
                Worksheet.Cells("3", "1").Style.WrapText = True
                Worksheet.Cells("3", "1", "5", "2").Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin)
                Worksheet.Cells("5", "1").Value = tblHeader.Rows(0)("Customer").ToString()
                Worksheet.Cells("5", "1", "5", "2").Merge = True


                Worksheet.Cells("3", "4").Value = "Number of Unique Invoices"
                Worksheet.Cells("4", "4").Value = "Invoices"
                Worksheet.Cells("3", "4", "4", "5").Merge = True
                ' Worksheet.Cells("3", "4", "3", "5").Merge = True
                Worksheet.Cells("3", "4").Style.WrapText = True
                Worksheet.Cells("3", "4", "5", "5").Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin)
                Worksheet.Cells("5", "4").Value = tblHeader.Rows(0)("Invoices").ToString()
                Worksheet.Cells("5", "4", "5", "5").Merge = True

                Worksheet.Cells("3", "7").Value = "Unique SKUs ordered"
                Worksheet.Cells("3", "7", "4", "8").Merge = True
                Worksheet.Cells("3", "7").Style.WrapText = True
                Worksheet.Cells("3", "7", "5", "8").Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin)
                Worksheet.Cells("5", "7").Value = tblHeader.Rows(0)("SKUOrdered").ToString()
                Worksheet.Cells("5", "7", "5", "8").Merge = True


                Worksheet.Cells("3", "10").Value = "Unique SKUs given as FOC"
                Worksheet.Cells("3", "10", "4", "11").Merge = True
                Worksheet.Cells("3", "10").Style.WrapText = True
                Worksheet.Cells("3", "10", "5", "11").Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin)
                Worksheet.Cells("5", "10").Value = tblHeader.Rows(0)("FOC_ordered").ToString()
                Worksheet.Cells("5", "10", "5", "11").Merge = True


                Worksheet.Cells("1", "1", "1", "2").Style.Font.Color.SetColor(Color.Black)

                ' ================================= Uncomment the below mention code if want customized column name for deatiled data =================


                Worksheet.Cells("7", "1").Value = "Order ItemCode"
                Worksheet.Cells("7", "2").Value = "Order Desc"
                Worksheet.Cells("7", "3").Value = "Order UOM"
                Worksheet.Cells("7", "4").Value = "Bonus ItemCode"
                Worksheet.Cells("7", "5").Value = "Bonus ItemDec"
                Worksheet.Cells("7", "6").Value = "Bonus UOM"
                Worksheet.Cells("7", "7").Value = "From Quantity"
                Worksheet.Cells("7", "8").Value = "To Quantity"

                Worksheet.Cells("7", "9").Value = "Bonus Quantity"
                Worksheet.Cells("7", "10").Value = "Point Type"
                Worksheet.Cells("7", "11").Value = "Customer"
                Worksheet.Cells("7", "12").Value = "Invoices"
                Worksheet.Cells("7", "13").Value = "SKU Units Ordered"
                Worksheet.Cells("7", "14").Value = "SKU Unit UOM"
                Worksheet.Cells("7", "15").Value = "FOC Ordered"
                Worksheet.Cells("7", "16").Value = "FOC Unit UOM"


                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename= Report.xlsx")

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




#End Region
End Class