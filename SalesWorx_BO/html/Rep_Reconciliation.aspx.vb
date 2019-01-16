
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
Imports OfficeOpenXml
Imports OfficeOpenXml.Style
Imports System.Drawing


Partial Public Class Rep_Reconciliation
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Private Const PageID As String = "P406"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        If Not IsPostBack Then
            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            'If Not HasPermission Then
            '    Err_No = 500
            '    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            'End If
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


                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                txtToDate.SelectedDate = Now()


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

        End If

    End Sub





    Private Sub BtnExport_Click(sender As Object, e As EventArgs) Handles BtnExportExcel.Click
        Try
            If ValidateInputs() Then


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



                    Args.Visible = True

                    Dim ObjReport As New SalesWorx.BO.Common.Reports
                    Dim ds As New DataSet
                    Dim dt As New DataTable
                    Dim tblData As New DataTable

                    'log.Debug("ddlOrganization.SelectedItem.Value" & ddlOrganization.SelectedItem.Value)
                    'log.Debug("van" & van)
                    'log.Debug("objUserAccess.UserID" & objUserAccess.UserID)
                    'log.Debug("txtFromDate" & CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"))
                    'log.Debug("txtToDate" & CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))

                    ds = ObjReport.Reconciliation(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, van, objUserAccess.UserID, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))
                    If ds.Tables.Count > 0 Then

                        dt = ds.Tables(0)
                        tblData = dt.DefaultView.ToTable(False, "Doc_Ref_No", "Doc_Type", "Doc_Date", "Customer_No", "ImsRef", "SalesRep_Number", "Emp_Name", "Sales_Org", "Creation_date", "Item_Code", "Line_No", "Qty", "Unit_Cost", "Unit_Price", "List_Price", "Lot_Number", "ExpiryDate", "Gross", "Discount_Value", "Discount_Amount", "Net", "VAT_Amount", "Total", "ColumnDiv", "Doc_Ref_No_ERP", "Doc_Type_ERP", "Doc_Date_ERP", "Customer_No_ERP", "SalesRep_Number_ERP", "Emp_Name_ERP", "Sales_Org_ERP", "Creation_Date_ERP", "Item_Code_ERP", "Line_No_ERP", "Qty_ERP", "Unit_Cost_ERP", "Unit_Price_ERP", "List_Price_ERP", "Lot_Number_ERP", "ExpiryDate_erp", "Gross_ERP", "Discount_Value_ERP", "Discount_Amount_ERP", "Net_ERP", "Vat_Amount_ERP", "Total_ERP", "Diff")
                        'tblData.Columns("Doc_Ref_No").ColumnName = "Document No"
                        'tblData.Columns("Doc_Type").ColumnName = "Document Type"
                        'tblData.Columns("Doc_Date").ColumnName = "Doc Date"
                        'tblData.Columns("Customer_No").ColumnName = "Customer No"
                        'tblData.Columns("SalesRep_Number").ColumnName = "Van Id"
                        'tblData.Columns("Emp_Name").ColumnName = "Salesperson Name"
                        'tblData.Columns("Sales_Org").ColumnName = "Sales Org"
                        'tblData.Columns("Creation_date").ColumnName = "Creation Date"
                        'tblData.Columns("Item_Code").ColumnName = "Item Code"
                        'tblData.Columns("Line_No").ColumnName = "Line No"
                        'tblData.Columns("Qty").ColumnName = "Qty"
                        'tblData.Columns("Unit_Price").ColumnName = "Unit Price"
                        'tblData.Columns("List_Price").ColumnName = "List Price"
                        'tblData.Columns("Gross").ColumnName = "Gross Amt"
                        'tblData.Columns("Discount_Value").ColumnName = "Disc %"
                        'tblData.Columns("Discount_Amount").ColumnName = "Discount Amt"
                        'tblData.Columns("Net").ColumnName = "Net Amt"
                        'tblData.Columns("VAT_Amount").ColumnName = "VAT Amt"
                        'tblData.Columns("Total").ColumnName = "Total Amt"
                        'tblData.Columns("Diff").ColumnName = "Difference"
                        'tblData.Columns("ImsRef").ColumnName = "IMS Reference"
                        'tblData.Columns("Unit_Cost").ColumnName = "Unit Cost"
                        'tblData.Columns("Lot_Number").ColumnName = "Lot Number"
                        'tblData.Columns("ExpiryDate").ColumnName = "Expiry Date"

                        Dim DtStock As New DataTable
                        DtStock.Columns.Add("ID")

                        Dim drstock As DataRow
                        If ds.Tables.Count > 1 Then
                            DtStock = ds.Tables(1)

                        End If


                        If tblData.Rows.Count > 0 Or DtStock.Rows.Count > 0 Then

                            Using package As New ExcelPackage()
                                ' add a new worksheet to the empty workbook
                                Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")



                                Worksheet.Cells("A1:AU1").Merge = True
                                Worksheet.Cells("A1:AU1").Value = "VAN Sales Reconciliation Report between Salesworx and ERP"
                                Worksheet.Cells("A1:AU1").Style.Font.Size = 12
                                Worksheet.Cells("A1:AU1").Style.Font.Bold = True
                                Worksheet.Cells("A1:AU1").Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center
                                Dim colFromHex As Color = System.Drawing.ColorTranslator.FromHtml("#FFF2CC")
                                Worksheet.Cells("A1:AU1").Style.Fill.PatternType = ExcelFillStyle.Solid
                                Worksheet.Cells("A1:AU1").Style.Fill.BackgroundColor.SetColor(colFromHex)



                                Worksheet.Cells("A2:W2").Merge = True
                                Worksheet.Cells("A2:W2").Value = "Salesworx Report"
                                Worksheet.Cells("A2:W2").Style.Font.Size = 12
                                Worksheet.Cells("A2:W2").Style.Font.Bold = True
                                Worksheet.Cells("A2:W2").Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center
                                Dim colFromHex_BO As Color = System.Drawing.ColorTranslator.FromHtml("#8EA9DB")
                                Worksheet.Cells("A2:W2").Style.Fill.PatternType = ExcelFillStyle.Solid
                                Worksheet.Cells("A2:W2").Style.Fill.BackgroundColor.SetColor(colFromHex_BO)


                                Worksheet.Cells("Y2:AU2").Merge = True
                                Worksheet.Cells("Y2:AU2").Value = "ERP Report"
                                Worksheet.Cells("Y2:AU2").Style.Font.Size = 12
                                Worksheet.Cells("Y2:AU2").Style.Font.Bold = True
                                Worksheet.Cells("Y2:AU2").Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center
                                Dim colFromHex_ERP As Color = System.Drawing.ColorTranslator.FromHtml("#C6E0B4")
                                Worksheet.Cells("Y2:AU2").Style.Fill.PatternType = ExcelFillStyle.Solid
                                Worksheet.Cells("Y2:AU2").Style.Fill.BackgroundColor.SetColor(colFromHex_ERP)
                                Worksheet.Cells("A4").LoadFromDataTable(tblData, True)


                                Worksheet.Column(3).Style.Numberformat.Format = "dd/MMM/yyyy HH:mm"
                                Worksheet.Column(9).Style.Numberformat.Format = "dd/MMM/yyyy HH:mm"
                                Worksheet.Column(17).Style.Numberformat.Format = "dd/MMM/yyyy HH:mm"
                                Worksheet.Column(28).Style.Numberformat.Format = "dd/MMM/yyyy HH:mm"
                                Worksheet.Column(32).Style.Numberformat.Format = "dd/MMM/yyyy HH:mm"
                                Worksheet.Column(40).Style.Numberformat.Format = "dd/MMM/yyyy HH:mm"
                                Worksheet.Column(14).Style.Numberformat.Format = "#,##0.0#"
                                Worksheet.Column(16).Style.Numberformat.Format = "#,##0.0#"
                                Worksheet.Column(18).Style.Numberformat.Format = "#,##0.0#"
                                Worksheet.Column(19).Style.Numberformat.Format = "#,##0.0#"
                                Worksheet.Column(20).Style.Numberformat.Format = "#,##0.0#"
                                Worksheet.Column(21).Style.Numberformat.Format = "#,##0.0#"
                                Worksheet.Column(22).Style.Numberformat.Format = "#,##0.0#"
                                Worksheet.Column(23).Style.Numberformat.Format = "#,##0.0#"

                                Worksheet.Column(41).Style.Numberformat.Format = "#,##0.0#"
                                Worksheet.Column(37).Style.Numberformat.Format = "#,##0.0#"
                                Worksheet.Column(38).Style.Numberformat.Format = "#,##0.0#"
                                Worksheet.Column(43).Style.Numberformat.Format = "#,##0.0#"
                                Worksheet.Column(42).Style.Numberformat.Format = "#,##0.0#"
                                Worksheet.Column(44).Style.Numberformat.Format = "#,##0.0#"
                                Worksheet.Column(45).Style.Numberformat.Format = "#,##0.0#"
                                Worksheet.Column(46).Style.Numberformat.Format = "#,##0.0#"
                                Worksheet.Column(47).Style.Numberformat.Format = "#,##0.0#"

                                Worksheet.Cells("A3").Value = "Document No"
                                Worksheet.Cells("B3").Value = "Document Type"
                                Worksheet.Cells("C3").Value = "Doc Date"
                                Worksheet.Cells("D3").Value = "Customer No"
                                Worksheet.Cells("E3").Value = "IMS Reference"
                                Worksheet.Cells("F3").Value = "Van Id"
                                Worksheet.Cells("G3").Value = "Salesperson Name"
                                Worksheet.Cells("H3").Value = "Sales Org"
                                Worksheet.Cells("I3").Value = "Creation Date"
                                Worksheet.Cells("J3").Value = "Item Code"
                                Worksheet.Cells("K3").Value = "Line No"
                                Worksheet.Cells("L3").Value = "Qty"
                                Worksheet.Cells("M3").Value = "Cost Price"
                                Worksheet.Cells("N3").Value = "Unit Price"
                                Worksheet.Cells("O3").Value = "List Price"
                                Worksheet.Cells("P3").Value = "Lot Numbre"
                                Worksheet.Cells("Q3").Value = "Expiry Date"
                                Worksheet.Cells("R3").Value = "Gross"
                                Worksheet.Cells("S3").Value = "Disc %"
                                Worksheet.Cells("T3").Value = "Discount Amt"
                                Worksheet.Cells("U3").Value = "Net Amt"
                                Worksheet.Cells("V3").Value = "VAT Amt"
                                Worksheet.Cells("W3").Value = "Total Amt"

                                Worksheet.Cells("Y3").Value = "Document No"
                                Worksheet.Cells("Z3").Value = "Document Type"
                                Worksheet.Cells("AA3").Value = "Doc Date"
                                Worksheet.Cells("AB3").Value = "Customer No"
                                Worksheet.Cells("AC3").Value = "Van Id"
                                Worksheet.Cells("AD3").Value = "Salesperson Name"
                                Worksheet.Cells("AE3").Value = "Sales Org"
                                Worksheet.Cells("AF3").Value = "Creation Date"
                                Worksheet.Cells("AG3").Value = "Item Code"
                                Worksheet.Cells("AH3").Value = "Line No"
                                Worksheet.Cells("AI3").Value = "Qty"
                                Worksheet.Cells("AJ3").Value = "Cost Price"
                                Worksheet.Cells("AK3").Value = "Unit Price"
                                Worksheet.Cells("AL3").Value = "List Price"
                                Worksheet.Cells("AM3").Value = "Lot Numbre"
                                Worksheet.Cells("AN3").Value = "Expiry Date"
                                Worksheet.Cells("AO3").Value = "Gross"
                                Worksheet.Cells("AP3").Value = "Disc %"
                                Worksheet.Cells("AQ3").Value = "Discount Amt"
                                Worksheet.Cells("AR3").Value = "Net Amt"
                                Worksheet.Cells("AS3").Value = "VAT Amt"
                                Worksheet.Cells("AT3").Value = "Total Amt"
                                Worksheet.Cells("AU3").Value = "Difference"

                                Worksheet.Cells("A4:AU4").Value = ""

                                '''''''''''''  Headercolumn Color''''''''''''''''

                                Dim colFromHex_h As Color = System.Drawing.ColorTranslator.FromHtml("#C9C9C9")
                               

                                Worksheet.Cells("A3:W4").Style.Fill.PatternType = ExcelFillStyle.Solid
                                Worksheet.Cells("A3:W4").Style.Fill.BackgroundColor.SetColor(colFromHex_h)
                                Worksheet.Cells("A3:W4").Style.Font.Bold = True

                                Worksheet.Cells("Y3:AU4").Style.Fill.PatternType = ExcelFillStyle.Solid
                                Worksheet.Cells("Y3:AU4").Style.Fill.BackgroundColor.SetColor(colFromHex_h)
                                Worksheet.Cells("Y3:AU4").Style.Font.Bold = True

                                '''''''BO''''''''''''''''''''''

                                Dim end_row As Integer
                                end_row = Worksheet.Dimension.End.Row

                                If DtStock.Rows.Count > 0 Then
                                    Worksheet.Cells("A" & (end_row + 1)).LoadFromDataTable(DtStock, True)

                                End If


                                Worksheet.Cells("A" & (end_row + 1) & ":W" & (end_row + 1)).Merge = True
                                Worksheet.Cells("A" & (end_row + 1)).Value = "Stock Entries"
                                Worksheet.Cells("A" & (end_row + 1) & ":w" & (end_row + 1)).Style.Font.Bold = True
                                Worksheet.Cells("A" & (end_row + 1) & ":w" & (end_row + 1)).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center

                                Worksheet.Cells("A" & (end_row + 1) & ":w" & (end_row + 1)).Style.Font.Size = 12
                                Dim colFromHex_StockEntries As Color = System.Drawing.ColorTranslator.FromHtml("#C6E0B4")
                                Worksheet.Cells("A" & (end_row + 1) & ":w" & (end_row + 1)).Style.Fill.PatternType = ExcelFillStyle.Solid
                                Worksheet.Cells("A" & (end_row + 1) & ":w" & (end_row + 1)).Style.Fill.BackgroundColor.SetColor(colFromHex_StockEntries)


                                end_row = Worksheet.Dimension.End.Row

                                Worksheet.Cells("A" & (end_row + 1) & ":Q" & (end_row + 1)).Merge = True
                                Worksheet.Cells("A" & (end_row + 1) & ":Q" & (end_row + 1)).Value = "Sales Total"
                                Worksheet.Cells("A" & (end_row + 1) & ":Q" & (end_row + 1)).Style.Font.Bold = True
                                Worksheet.Cells("A" & (end_row + 1) & ":Q" & (end_row + 1)).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right


                                Worksheet.Cells("R" & (end_row + 1)).Formula = "SUM(R4:R" & end_row & ")"       'Total Gross
                                Worksheet.Cells("R" & (end_row + 1)).Style.Font.Bold = True

                                Worksheet.Cells("T" & (end_row + 1)).Formula = "SUM(T4:T" & end_row & ")"       'Total Discount Amt
                                Worksheet.Cells("T" & (end_row + 1)).Style.Font.Bold = True

                                Worksheet.Cells("U" & (end_row + 1)).Formula = "SUM(U4:U" & end_row & ")"       'Total Net Amt
                                Worksheet.Cells("U" & (end_row + 1)).Style.Font.Bold = True

                                Worksheet.Cells("V" & (end_row + 1)).Formula = "SUM(V4:V" & end_row & ")"       'Total VAT Amt
                                Worksheet.Cells("V" & (end_row + 1)).Style.Font.Bold = True

                                Worksheet.Cells("W" & (end_row + 1)).Formula = "SUM(W4:W" & end_row & ")"       'Total Total Amt
                                Worksheet.Cells("W" & (end_row + 1)).Style.Font.Bold = True


                                '''''''ERP'''''''''''''''''''''

                                Worksheet.Cells("Y" & (end_row + 1) & ":AP" & (end_row + 1)).Merge = True
                                Worksheet.Cells("Y" & (end_row + 1) & ":AP" & (end_row + 1)).Value = "Total"
                                Worksheet.Cells("Y" & (end_row + 1) & ":AP" & (end_row + 1)).Style.Font.Bold = True
                                Worksheet.Cells("Y" & (end_row + 1) & ":AP" & (end_row + 1)).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right


                                Worksheet.Cells("AQ" & (end_row + 1)).Formula = "SUM(AQ4:AQ" & end_row & ")" 'Total Discount Amt
                                Worksheet.Cells("AQ" & (end_row + 1)).Style.Font.Bold = True

                                Worksheet.Cells("AR" & (end_row + 1)).Formula = "SUM(AR4:AR" & end_row & ")" 'Total Net Amt
                                Worksheet.Cells("AR" & (end_row + 1)).Style.Font.Bold = True

                                Worksheet.Cells("AS" & (end_row + 1)).Formula = "SUM(AS4:AS" & end_row & ")"  'Total VAT Amt
                                Worksheet.Cells("AS" & (end_row + 1)).Style.Font.Bold = True

                                Worksheet.Cells("AT" & (end_row + 1)).Formula = "SUM(AT4:AT" & end_row & ")" 'Total Total Amt
                                Worksheet.Cells("AT" & (end_row + 1)).Style.Font.Bold = True

                                Worksheet.Cells("AU" & (end_row + 1)).Formula = "SUM(AU4:AU" & end_row & ")" 'difference
                                Worksheet.Cells("AU" & (end_row + 1)).Style.Font.Bold = True



                                Worksheet.Cells("AO" & (end_row + 1)).Formula = "SUM(AO4:AO" & end_row & ")"       'Total Gross
                                Worksheet.Cells("AO" & (end_row + 1)).Style.Font.Bold = True

                                ''''BO Border'''''''''''''''''''''''

                                Worksheet.Cells("A2" & ":W" & (end_row + 1)).Style.Border.Top.Style = ExcelBorderStyle.Thin
                                Worksheet.Cells("A2" & ":W" & (end_row + 1)).Style.Border.Top.Color.SetColor(Color.Black)

                                Worksheet.Cells("A" & (end_row + 1) & ":W" & (end_row + 1)).Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                                Worksheet.Cells("A" & (end_row + 1) & ":W" & (end_row + 1)).Style.Border.Bottom.Color.SetColor(Color.Black)


                                Worksheet.Cells("A2" & ":A" & (end_row + 1)).Style.Border.Left.Style = ExcelBorderStyle.Thin
                                Worksheet.Cells("A2" & ":A" & (end_row + 1)).Style.Border.Left.Color.SetColor(Color.Black)


                                Worksheet.Cells("A2" & ":W" & (end_row + 1)).Style.Border.Right.Style = ExcelBorderStyle.Thin
                                Worksheet.Cells("A2" & ":W" & (end_row + 1)).Style.Border.Right.Color.SetColor(Color.Black)


                                ''''ERP Border'''''''''''''''''''''''

                                Worksheet.Cells("Y2" & ":AU" & (end_row + 1)).Style.Border.Top.Style = ExcelBorderStyle.Thin
                                Worksheet.Cells("Y2" & ":AU" & (end_row + 1)).Style.Border.Top.Color.SetColor(Color.Black)

                                Worksheet.Cells("Y" & (end_row + 1) & ":AU" & (end_row + 1)).Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                                Worksheet.Cells("Y" & (end_row + 1) & ":AU" & (end_row + 1)).Style.Border.Bottom.Color.SetColor(Color.Black)


                                Worksheet.Cells("Y2" & ":Y" & (end_row + 1)).Style.Border.Left.Style = ExcelBorderStyle.Thin
                                Worksheet.Cells("Y2" & ":Y" & (end_row + 1)).Style.Border.Left.Color.SetColor(Color.Black)


                                Worksheet.Cells("Y2" & ":AU" & (end_row + 1)).Style.Border.Right.Style = ExcelBorderStyle.Thin
                                Worksheet.Cells("Y2" & ":AU" & (end_row + 1)).Style.Border.Right.Color.SetColor(Color.Black)


                                ''Difference''''''

                                'Worksheet.Cells("AU2" & ":AN" & (end_row + 1)).Style.Border.Top.Style = ExcelBorderStyle.Thin
                                'Worksheet.Cells("AU2" & ":AN" & (end_row + 1)).Style.Border.Top.Color.SetColor(Color.Black)

                                'Worksheet.Cells("AU" & (end_row + 1) & ":AU" & (end_row + 1)).Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                                'Worksheet.Cells("AU" & (end_row + 1) & ":AU" & (end_row + 1)).Style.Border.Bottom.Color.SetColor(Color.Black)


                                'Worksheet.Cells("AU2" & ":AU" & (end_row + 1)).Style.Border.Right.Style = ExcelBorderStyle.Thin
                                'Worksheet.Cells("AU2" & ":AU" & (end_row + 1)).Style.Border.Right.Color.SetColor(Color.Black)


                                ''''open and close (on 1 van selection )''''''

                                'If ddlVan.CheckedItems.Count = 1 Then
                                '    If ds.Tables.Count > 2 Then
                                '        If ds.Tables(2).Rows.Count > 0 Then
                                '            Dim TotalSold As Integer
                                '            Dim TotalReturned As Integer
                                '            Dim TotalLoaded As Integer
                                '            Dim TotalUnloded As Integer

                                '            Worksheet.Cells("A4:K4").Merge = True
                                '            'Worksheet.Cells("A4:K4").Value = "Opening Stock"
                                '            Worksheet.Cells("A4:K4").Style.Font.Bold = True
                                '            Worksheet.Cells("A4:K4").Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right
                                '            '' Worksheet.Cells("L4").Value = Val(ds.Tables(2).Rows(0)(0).ToString)
                                '            ' log.Debug("Val(ds.Tables(2).Rows(0)(0).ToString):" & Val(ds.Tables(2).Rows(0)("openingStock").ToString))
                                '            ' Worksheet.Cells("L4").Value = Val(ds.Tables(2).Rows(0)("openingStock").ToString)

                                '            Dim query = (From UserEntry In tblData _
                                '            Group UserEntry By key = UserEntry.Field(Of String)("Doc_Type") Into Group _
                                '            Select Doc_Type = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("Qty"))).ToList

                                '            For Each x In query
                                '                If x.Doc_Type = "Order" Then
                                '                    TotalSold = x.Total
                                '                End If
                                '                If x.Doc_Type = "Return" Then
                                '                    TotalReturned = x.Total
                                '                End If
                                '            Next

                                '            Dim query1 = (From UserEntry In DtStock _
                                '            Group UserEntry By key = UserEntry.Field(Of String)("Doc_Type") Into Group _
                                '            Select Transfer_Type = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("Qty"))).ToList

                                '            For Each x In query1
                                '                If x.Transfer_Type = "Load Van/L/R" Then
                                '                    TotalLoaded = x.Total
                                '                End If
                                '                If x.Transfer_Type = "Unload" Then
                                '                    TotalUnloded = x.Total
                                '                End If
                                '            Next

                                '            Dim ClosingStock As Integer
                                '            ClosingStock = Val(ds.Tables(2).Rows(0)("openingStock").ToString) + TotalLoaded + TotalReturned - TotalSold - TotalUnloded
                                '            log.Debug("TotalLoaded:" & TotalLoaded)
                                '            log.Debug("TotalReturned:" & TotalReturned)
                                '            log.Debug("TotalSold:" & TotalSold)
                                '            log.Debug("TotalUnloded:" & TotalUnloded)


                                '            Worksheet.Cells("A" & (end_row + 3) & ":K" & (end_row + 3)).Merge = True
                                '            Worksheet.Cells("A" & (end_row + 3) & ":K" & (end_row + 3)).Value = "Inventory Closing Stock"
                                '            Worksheet.Cells("A" & (end_row + 3) & ":K" & (end_row + 3)).Style.Font.Bold = True
                                '            Worksheet.Cells("A" & (end_row + 3) & ":K" & (end_row + 3)).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right
                                '            Worksheet.Cells("L" & (end_row + 3)).Value = ClosingStock


                                '            Worksheet.Cells("A" & (end_row + 3) & ":K" & (end_row + 3)).Style.Fill.PatternType = ExcelFillStyle.Solid
                                '            Worksheet.Cells("A" & (end_row + 3) & ":K" & (end_row + 3)).Style.Fill.BackgroundColor.SetColor(colFromHex_h)
                                '            Worksheet.Cells("A" & (end_row + 3) & ":K" & (end_row + 3)).Style.Font.Bold = True
                                '        End If
                                '    End If
                                'End If

                                '''''''''''''''''''''''''''''''''''''''''''''''

                                Worksheet.Cells.AutoFitColumns()


                                Response.Clear()
                                Response.Buffer = True
                                Response.Charset = ""

                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                                Response.AddHeader("content-disposition", "attachment;filename= Reconciliation.xlsx")

                                Using MyMemoryStream As New MemoryStream()
                                    package.SaveAs(MyMemoryStream)
                                    MyMemoryStream.WriteTo(Response.OutputStream)
                                    Response.AddHeader("Content-Length", MyMemoryStream.Length)
                                    Response.Flush()
                                    Response.Close()
                                End Using
                            End Using
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            log.Debug(ex.Message.ToString())
        End Try
    End Sub



    Private Sub BindReport()
        Try
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



                Args.Visible = True

                Dim ObjReport As New SalesWorx.BO.Common.Reports
                Dim ds As New DataSet
                ds = ObjReport.Reconciliation(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, van, objUserAccess.UserID, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))
                gvRep.DataSource = ds.Tables(0)
                gvRep.DataBind()

                gvRep_Stock.DataSource = ds.Tables(1)
                gvRep_Stock.DataBind()

            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub



    Private Sub dgv_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvRep.SortCommand
        Try
            ViewState("SortField") = e.SortExpression
            SortDirection = "flip"
            BindReport()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Private Sub dgv_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        Try
            BindReport()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
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
    Function ValidateInputs() As Boolean
        Try


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
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Function
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        Try
            If ValidateInputs() Then
                gvRep.Visible = True

                BindReport()
            Else
                Args.Visible = False
                gvRep.Visible = False

            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Sub MessageBoxValidation(ByVal str As String, Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        LoadOrgDetails()
    End Sub
    Sub LoadOrgDetails()
        Try
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New SalesWorx.BO.Common.Common()
            ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
            ddlVan.DataBind()
            For Each itm As RadComboBoxItem In ddlVan.Items
                itm.Checked = True
            Next
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        Try
            ddlOrganization.ClearSelection()
            If ddlOrganization.Items.Count = 2 Then
                ddlOrganization.SelectedIndex = 1
            End If
            txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
            txtToDate.SelectedDate = Now()
            LoadOrgDetails()
            Args.Visible = False
            gvRep.Visible = False
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
End Class