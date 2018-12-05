Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI

Partial Public Class RepDailyReport
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "DailyReport"

    Private Const PageID As String = "P216"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single

    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private Sub RepRouteMaster_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ''If Not IsNothing(Me.Master) Then

        ''    Dim masterScriptManager As ScriptManager
        ''    masterScriptManager = CType(Master.FindControl("ScriptManager1"), ScriptManager)

        ''    ' Make sure our master page has the script manager we're looking for
        ''    If Not IsNothing(masterScriptManager) Then

        ''        ' Turn off partial page postbacks for this page
        ''        masterScriptManager.EnablePartialRendering = False
        ''    End If

        ''End If

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
                ddl_org.DataTextField = "Description"
                ddl_org.DataValueField = "MAS_Org_ID"
                ddl_org.Items.Clear()
                ddl_org.AppendDataBoundItems = True
                ddl_org.Items.Add("Select Organization")
                ddl_org.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddl_org.DataBind()

                If ddl_org.Items.Count = 2 Then
                    ddl_org.SelectedIndex = 1

                    Dim dtCur As New DataTable
                    dtCur = ObjCommon.GetCurrencyByOrg(Err_No, Err_Desc, ddl_org.SelectedValue)
                    If dtCur.Rows.Count > 0 Then
                        hfCurrency.Value = dtCur.Rows(0)(0).ToString()
                        hfDecimal.Value = dtCur.Rows(0)(1).ToString()
                        lbl_Currency.Text = " (" & dtCur.Rows(0)(0).ToString() & ")"
                    End If

                    LoadVan()

                End If

                Dim ClientCode As String
                ClientCode = (New SalesWorx.BO.Common.Common).GetAppConfig(Err_No, Err_Desc, "CLIENT_CODE")

                If ClientCode.ToUpper <> "AST" Then
                    txt_fromDate.SelectedDate = Now().Date
                    txt_ToDate.SelectedDate = Now().Date
                Else
                    txt_fromDate.SelectedDate = FirstDayOfMonth(Now().Date)
                    txt_ToDate.SelectedDate = Now().Date
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
    Public Function FirstDayOfMonth(ByVal sourceDate As DateTime) As DateTime
        Return New DateTime(sourceDate.Year, sourceDate.Month, 1)
    End Function
    Sub LoadVan()
        Dim objUserAccess As UserAccess
        Try
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New SalesWorx.BO.Common.Common()
            ddl_Van.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddl_org.SelectedValue, objUserAccess.UserID)
            ddl_Van.DataValueField = "SalesRep_ID"
            ddl_Van.DataTextField = "SalesRep_Name"
            ddl_Van.DataBind()

            For Each itm As RadComboBoxItem In ddl_Van.Items
                itm.Checked = True
            Next

        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            objUserAccess = Nothing
            ObjCommon = Nothing
        End Try
    End Sub
    Protected Sub Chk_Organization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_org.SelectedIndexChanged
        '' ''RVMain.Visible = False
        '' ''Dim OrgIds As String = ""
        '' ''Dim objUserAccess As UserAccess
        '' ''objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        '' ''ObjCommon = New SalesWorx.BO.Common.Common()
        ' '' ''chkSalesRep.DataTextField = "SalesRep_Name"
        ' '' ''chkSalesRep.DataValueField = "SalesRep_ID"
        ' '' ''chkSalesRep.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddl_org.SelectedItem.Value, objUserAccess.UserID)
        ' '' ''chkSalesRep.DataBind()
        Try
            If ddl_org.SelectedValue <> "" Then
                LoadVan()

                Dim dtCur As New DataTable
                dtCur = ObjCommon.GetCurrencyByOrg(Err_No, Err_Desc, ddl_org.SelectedItem.Value)
                If dtCur.Rows.Count > 0 Then
                    hfCurrency.Value = dtCur.Rows(0)(0).ToString()
                    hfDecimal.Value = dtCur.Rows(0)(1).ToString()
                    lbl_Currency.Text = " (" & dtCur.Rows(0)(0).ToString() & ")"
                End If

            Else
                ddl_Van.ClearSelection()
                ddl_Van.Items.Clear()
            End If
        Catch ex As Exception
            log.Error(ex.Message)
        End Try

    End Sub
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        Try
            If ddl_org.SelectedValue = "" Then
                MessageBoxValidation("Please select the organization", "Validation")
                Return bretval
                Exit Function
            End If

            If Not IsDate(txt_fromDate.SelectedDate) Then
                MessageBoxValidation("Enter valid ""From date"".", "Validation")
                Return bretval
                Exit Function
            End If

            If Not IsDate(txt_ToDate.SelectedDate) Then
                MessageBoxValidation("Enter valid ""To date"".", "Validation")
                Return bretval
                Exit Function
            End If

            If CDate(txt_fromDate.SelectedDate) > CDate(txt_ToDate.SelectedDate) Then
                MessageBoxValidation("Start Date should not be greater than End Date.", "Validation")
                Return bretval
                Exit Function
            End If

            '' ignore the date selection with in same month of the year
            If Not (CDate(txt_fromDate.SelectedDate).Month = CDate(txt_ToDate.SelectedDate).Month AndAlso CDate(txt_fromDate.SelectedDate).Year = CDate(txt_ToDate.SelectedDate).Year) Then
                If DateDiff(DateInterval.Day, CDate(txt_fromDate.SelectedDate), CDate(txt_ToDate.SelectedDate)) > 30 Then
                    MessageBoxValidation("Select dates upto 30 days.", "Validation")
                    Return bretval
                    Exit Function
                End If
            End If

            Return True

        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Function

    Private Sub BindData()
        Dim ObjReport As New SalesWorx.BO.Common.Reports
        Try
            Dim vantxt As String = ""
            Dim van As String = ""
            Dim collection As IList(Of RadComboBoxItem) = ddl_Van.CheckedItems
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","
                vantxt = vantxt & li.Text & ","

            Next
            If vantxt <> "" Then
                vantxt = vantxt.Substring(0, vantxt.Length - 1)
            End If
            If van = "" Then
                van = "-1"
            End If

            lbl_org.Text = ddl_org.SelectedItem.Text
            If van = "-1" Then
                lbl_van.Text = "All"
            Else
                lbl_van.Text = vantxt
            End If


            lbl_from.Text = CDate(txt_fromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_to.Text = CDate(txt_ToDate.SelectedDate).ToString("dd-MMM-yyyy")

            Args.Visible = True
            divCurrency.Visible = True
            gvRep.Visible = True
            DailyReptab.Visible = True
            summaryChart.Visible = True
            prodChart.Visible = True
            saleChart.Visible = True

            rpbFilter.Items(0).Expanded = False

            Dim dt As New DataTable
            dt = ObjReport.GetDailyReport(Err_No, Err_Desc, ddl_org.SelectedItem.Value, txt_fromDate.SelectedDate, txt_ToDate.SelectedDate, van, CType(Session.Item("USER_ACCESS"), UserAccess).UserID)

            Dim sourceTbl As DataTable = dt.Copy()

            Dim dtFinal As New DataTable
            '' dtFinal.Columns.Add("FSRID")
            dtFinal.Columns.Add("FSR")
            dtFinal.Columns.Add("Item")

            '' Dynamically Creating Day Column

            Dim initDay As DateTime = txt_fromDate.SelectedDate

            While initDay <= txt_ToDate.SelectedDate
                dtFinal.Columns.Add(initDay.ToShortDateString())
                initDay = initDay.AddDays(1)
            End While

            '' '' Order by Van

            ''dt = dt.Select("1=1", "FSRID ASC").CopyToDataTable()

            '' Getting Distinct Van
            Dim distinctVanDT As DataTable = dt.DefaultView.ToTable(True, "FSR")


            For Each vanRow As DataRow In distinctVanDT.Rows

                '' For Total no of calls visited
                Dim dRowVisited As DataRow = dtFinal.NewRow()
                dRowVisited("FSR") = vanRow("FSR")
                dRowVisited("Item") = "Total no of calls"

                '' Filling other columns 
                For Each column In dtFinal.Columns
                    If column.ColumnName() <> "FSR" AndAlso column.ColumnName() <> "Item" Then
                        Dim SelRow() As DataRow = dt.Select("VisitDate='" & CDate(column.ColumnName()).ToString() & "' and FSR='" & vanRow("FSR") & "'")
                        If SelRow.Length > 0 Then
                            dRowVisited(column.ColumnName()) = "<div align='right'>" & IIf(SelRow(0)("TotalCallVisited") Is DBNull.Value, 0, SelRow(0)("TotalCallVisited")) & "</div>"
                        Else
                            dRowVisited(column.ColumnName()) = "0"
                        End If
                    End If
                Next

                dtFinal.Rows.Add(dRowVisited)
                dtFinal.AcceptChanges()

                Dim dRowCold As DataRow = dtFinal.NewRow()
                dRowCold("FSR") = vanRow("FSR")
                dRowCold("Item") = "Total No of Cold calls"

                '' Filling other columns 
                For Each column In dtFinal.Columns
                    If column.ColumnName() <> "FSR" AndAlso column.ColumnName() <> "Item" Then
                        Dim SelRow() As DataRow = dt.Select("VisitDate='" & CDate(column.ColumnName()).ToString() & "' and FSR='" & vanRow("FSR") & "'")
                        If SelRow.Length > 0 Then
                            dRowCold(column.ColumnName()) = "<div align='right'>" & IIf(SelRow(0)("ColdCalls") Is DBNull.Value, 0, SelRow(0)("ColdCalls")) & "</div>"
                        Else
                            dRowCold(column.ColumnName()) = "0"
                        End If
                    End If
                Next

                dtFinal.Rows.Add(dRowCold)
                dtFinal.AcceptChanges()

                '' For Total No of Productive calls
                Dim dRowProd As DataRow = dtFinal.NewRow()
                dRowProd("FSR") = vanRow("FSR")
                dRowProd("Item") = "Total No of Productive calls"

                '' Filling other columns 
                For Each column In dtFinal.Columns
                    If column.ColumnName() <> "FSR" AndAlso column.ColumnName() <> "Item" Then
                        Dim SelRow() As DataRow = dt.Select("VisitDate='" & CDate(column.ColumnName()).ToString() & "' and FSR='" & vanRow("FSR") & "'")
                        If SelRow.Length > 0 Then
                            dRowProd(column.ColumnName()) = "<div align='right'>" & IIf(SelRow(0)("TotalProductiveCall") Is DBNull.Value, 0, SelRow(0)("TotalProductiveCall")) & "</div>"
                        Else
                            dRowProd(column.ColumnName()) = "0"
                        End If
                    End If
                Next

                dtFinal.Rows.Add(dRowProd)
                dtFinal.AcceptChanges()


                Dim dRowNonProd As DataRow = dtFinal.NewRow()
                dRowNonProd("FSR") = vanRow("FSR")
                dRowNonProd("Item") = "Total No of Unproductive calls"

                '' Filling other columns 
                For Each column In dtFinal.Columns
                    If column.ColumnName() <> "FSR" AndAlso column.ColumnName() <> "Item" Then
                        Dim SelRow() As DataRow = dt.Select("VisitDate='" & CDate(column.ColumnName()).ToString() & "' and FSR='" & vanRow("FSR") & "'")
                        If SelRow.Length > 0 Then
                            dRowNonProd(column.ColumnName()) = "<div align='right'>" & IIf(SelRow(0)("UnprodCalls") Is DBNull.Value, 0, SelRow(0)("UnprodCalls")) & "</div>"
                        Else
                            dRowNonProd(column.ColumnName()) = "0"
                        End If
                    End If
                Next

                dtFinal.Rows.Add(dRowNonProd)
                dtFinal.AcceptChanges()



                '' For No of collection calls(With no sales)
                Dim dRowCollection As DataRow = dtFinal.NewRow()
                dRowCollection("FSR") = vanRow("FSR")
                dRowCollection("Item") = "No of collection calls"

                '' Filling other columns 
                For Each column In dtFinal.Columns
                    If column.ColumnName() <> "FSR" AndAlso column.ColumnName() <> "Item" Then
                        Dim SelRow() As DataRow = dt.Select("VisitDate='" & CDate(column.ColumnName()).ToString() & "' and FSR='" & vanRow("FSR") & "'")
                        If SelRow.Length > 0 Then
                            dRowCollection(column.ColumnName()) = "<div align='right'>" & IIf(SelRow(0)("CollectionCalls") Is DBNull.Value, 0, SelRow(0)("CollectionCalls")) & "</div>"
                        Else
                            dRowCollection(column.ColumnName()) = "0"
                        End If
                    End If
                Next

                dtFinal.Rows.Add(dRowCollection)
                dtFinal.AcceptChanges()

                Dim dRowSale As DataRow = dtFinal.NewRow()
                dRowSale("FSR") = vanRow("FSR")
                dRowSale("Item") = "Total sales value"

                '' Filling other columns 
                For Each column In dtFinal.Columns
                    If column.ColumnName() <> "FSR" AndAlso column.ColumnName() <> "Item" Then
                        Dim SelRow() As DataRow = dt.Select("VisitDate='" & CDate(column.ColumnName()).ToString() & "' and FSR='" & vanRow("FSR") & "'")
                        If SelRow.Length > 0 Then
                            Dim Sales As Double = IIf(SelRow(0)("Sales") Is DBNull.Value, 0, SelRow(0)("Sales"))

                            dRowSale(column.ColumnName()) = "<div align='right'>" & FormatNumber(CDbl(Sales), hfDecimal.Value) & "</div>"
                        Else
                            dRowSale(column.ColumnName()) = "0"
                        End If
                    End If
                Next
                dtFinal.Rows.Add(dRowSale)
                dtFinal.AcceptChanges()

                Dim dRowReturn As DataRow = dtFinal.NewRow()
                dRowReturn("FSR") = vanRow("FSR")
                dRowReturn("Item") = "Total Return value"

                '' Filling other columns 
                For Each column In dtFinal.Columns
                    If column.ColumnName() <> "FSR" AndAlso column.ColumnName() <> "Item" Then
                        Dim SelRow() As DataRow = dt.Select("VisitDate='" & CDate(column.ColumnName()).ToString() & "' and FSR='" & vanRow("FSR") & "'")
                        If SelRow.Length > 0 Then
                            Dim Sales As Double = IIf(SelRow(0)("TotReturns") Is DBNull.Value, 0, SelRow(0)("TotReturns"))

                            dRowReturn(column.ColumnName()) = "<div align='right'>" & FormatNumber(CDbl(Sales), hfDecimal.Value) & "</div>"
                        Else
                            dRowReturn(column.ColumnName()) = "0"
                        End If
                    End If
                Next
                dtFinal.Rows.Add(dRowReturn)
                dtFinal.AcceptChanges()


                '' For Total sales value
                Dim dRowNetSale As DataRow = dtFinal.NewRow()
                dRowNetSale("FSR") = vanRow("FSR")
                dRowNetSale("Item") = "Net sales value"

                '' Filling other columns 
                For Each column In dtFinal.Columns
                    If column.ColumnName() <> "FSR" AndAlso column.ColumnName() <> "Item" Then
                        Dim SelRow() As DataRow = dt.Select("VisitDate='" & CDate(column.ColumnName()).ToString() & "' and FSR='" & vanRow("FSR") & "'")
                        If SelRow.Length > 0 Then
                            Dim Sales As Double = IIf(SelRow(0)("Sales") Is DBNull.Value, 0, SelRow(0)("Sales"))
                            Dim Returns As Double = IIf(SelRow(0)("TotReturns") Is DBNull.Value, 0, SelRow(0)("TotReturns"))
                            If CDbl(Sales - Returns) > 0 Then
                                dRowNetSale(column.ColumnName()) = "<div align='right'>" & FormatNumber(CDbl(Sales - Returns), hfDecimal.Value) & "</div>"
                            Else
                                dRowNetSale(column.ColumnName()) = "<div align='right' style='color:red;'>" & FormatNumber(CDbl(Sales - Returns), hfDecimal.Value) & "</div>"
                            End If
                        Else
                            dRowNetSale(column.ColumnName()) = "0"
                        End If
                    End If
                Next

                dtFinal.Rows.Add(dRowNetSale)
                dtFinal.AcceptChanges()


                Dim dRowCash As DataRow = dtFinal.NewRow()
                dRowCash("FSR") = vanRow("FSR")
                dRowCash("Item") = "Total Cash Collection"

                '' Filling other columns 
                For Each column In dtFinal.Columns
                    If column.ColumnName() <> "FSR" AndAlso column.ColumnName() <> "Item" Then
                        Dim SelRow() As DataRow = dt.Select("VisitDate='" & CDate(column.ColumnName()).ToString() & "' and FSR='" & vanRow("FSR") & "'")
                        If SelRow.Length > 0 Then
                            Dim Sales As Double = IIf(SelRow(0)("TotalCashCollection") Is DBNull.Value, 0, SelRow(0)("TotalCashCollection"))

                            dRowCash(column.ColumnName()) = "<div align='right'>" & FormatNumber(CDbl(Sales), hfDecimal.Value) & "</div>"
                        Else
                            dRowCash(column.ColumnName()) = "0"
                        End If
                    End If
                Next
                dtFinal.Rows.Add(dRowCash)
                dtFinal.AcceptChanges()

                Dim dRowChq As DataRow = dtFinal.NewRow()
                dRowChq("FSR") = vanRow("FSR")
                dRowChq("Item") = "Total Cheque Collection"

                '' Filling other columns 
                For Each column In dtFinal.Columns
                    If column.ColumnName() <> "FSR" AndAlso column.ColumnName() <> "Item" Then
                        Dim SelRow() As DataRow = dt.Select("VisitDate='" & CDate(column.ColumnName()).ToString() & "' and FSR='" & vanRow("FSR") & "'")
                        If SelRow.Length > 0 Then
                            Dim Sales As Double = IIf(SelRow(0)("TotalChequeCollection") Is DBNull.Value, 0, SelRow(0)("TotalChequeCollection"))

                            dRowChq(column.ColumnName()) = "<div align='right'>" & FormatNumber(CDbl(Sales), hfDecimal.Value) & "</div>"
                        Else
                            dRowChq(column.ColumnName()) = "0"
                        End If
                    End If
                Next
                dtFinal.Rows.Add(dRowChq)
                dtFinal.AcceptChanges()




                Dim dRowColl As DataRow = dtFinal.NewRow()
                dRowColl("FSR") = vanRow("FSR")
                dRowColl("Item") = "Total Collection"

                '' Filling other columns 
                For Each column In dtFinal.Columns
                    If column.ColumnName() <> "FSR" AndAlso column.ColumnName() <> "Item" Then
                        Dim SelRow() As DataRow = dt.Select("VisitDate='" & CDate(column.ColumnName()).ToString() & "' and FSR='" & vanRow("FSR") & "'")
                        If SelRow.Length > 0 Then
                            Dim Sales As Double = IIf(SelRow(0)("TotalCollection") Is DBNull.Value, 0, SelRow(0)("TotalCollection"))

                            dRowColl(column.ColumnName()) = "<div align='right'>" & FormatNumber(CDbl(Sales), hfDecimal.Value) & "</div>"
                        Else
                            dRowColl(column.ColumnName()) = "0"
                        End If
                    End If
                Next
                dtFinal.Rows.Add(dRowColl)

                dtFinal.AcceptChanges()

                Dim dRowDistr As DataRow = dtFinal.NewRow()
                dRowDistr("FSR") = vanRow("FSR")
                dRowDistr("Item") = "Total no of Distribution Checks"

                '' Filling other columns 
                For Each column In dtFinal.Columns
                    If column.ColumnName() <> "FSR" AndAlso column.ColumnName() <> "Item" Then
                        Dim SelRow() As DataRow = dt.Select("VisitDate='" & CDate(column.ColumnName()).ToString() & "' and FSR='" & vanRow("FSR") & "'")
                        If SelRow.Length > 0 Then
                            dRowDistr(column.ColumnName()) = "<div align='right'>" & IIf(SelRow(0)("NoofDistributionChecks") Is DBNull.Value, 0, SelRow(0)("NoofDistributionChecks")) & "</div>"
                        Else
                            dRowDistr(column.ColumnName()) = "0"
                        End If
                    End If
                Next
                dtFinal.Rows.Add(dRowDistr)

                dtFinal.AcceptChanges()

                '' For Start Time
                Dim dRowSTime As DataRow = dtFinal.NewRow()
                dRowSTime("FSR") = vanRow("FSR")
                dRowSTime("Item") = "Start Time"

                '' Filling other columns 
                For Each column In dtFinal.Columns
                    If column.ColumnName() <> "FSR" AndAlso column.ColumnName() <> "Item" Then
                        Dim SelRow() As DataRow = dt.Select("VisitDate='" & CDate(column.ColumnName()).ToString() & "' and FSR='" & vanRow("FSR") & "'")
                        If SelRow.Length > 0 Then
                            If SelRow(0)("StartTime") Is DBNull.Value Then
                                dRowSTime(column.ColumnName()) = ""
                            Else
                                dRowSTime(column.ColumnName()) = "<div align='center'>" & CDate(SelRow(0)("StartTime")).ToString("hh:mm:ss tt") & "</div>"
                            End If

                        Else
                            dRowSTime(column.ColumnName()) = ""
                        End If
                    End If
                Next

                dtFinal.Rows.Add(dRowSTime)
                dtFinal.AcceptChanges()


                '' For End Time
                Dim dRowETime As DataRow = dtFinal.NewRow()
                dRowETime("FSR") = vanRow("FSR")
                dRowETime("Item") = "End Time"

                '' Filling other columns 
                For Each column In dtFinal.Columns
                    If column.ColumnName() <> "FSR" AndAlso column.ColumnName() <> "Item" Then
                        Dim SelRow() As DataRow = dt.Select("VisitDate='" & CDate(column.ColumnName()).ToString() & "' and FSR='" & vanRow("FSR") & "'")
                        If SelRow.Length > 0 Then
                            If SelRow(0)("EndTime") Is DBNull.Value Then
                                dRowETime(column.ColumnName()) = ""
                            Else
                                dRowETime(column.ColumnName()) = "<div align='center'>" & CDate(SelRow(0)("EndTime")).ToString("hh:mm:ss tt") & "</div>"
                            End If

                        Else
                            dRowETime(column.ColumnName()) = ""
                        End If
                    End If
                Next

                dtFinal.Rows.Add(dRowETime)
                dtFinal.AcceptChanges()

            Next



            Dim str As String = ""

            ' ''dtFinal.Columns.Add("CallVisited", Type.GetType("System.Int32"))
            ' ''dtFinal.Columns.Add("ProductiveCall", Type.GetType("System.Int32"))
            ' ''dtFinal.Columns.Add("CollectionCalls", Type.GetType("System.Int32"))
            ' ''dtFinal.Columns.Add("Sales", Type.GetType("System.Double"))
            ' ''dtFinal.Columns.Add("Returns", Type.GetType("System.Double"))
            ' ''dtFinal.Columns.Add("StartTime", Type.GetType("System.String"))
            ' ''dtFinal.Columns.Add("EndTime", Type.GetType("System.String"))
            ' ''dtFinal.Columns.Add("TotSales", Type.GetType("System.Double"))

            ' ''For Each seldr As DataRow In dt.Rows
            ' ''    Dim dr As DataRow
            ' ''    dr = dtFinal.NewRow
            ' ''    dr("FSRID") = seldr("FSRID")
            ' ''    dr("FSR") = seldr("FSR")
            ' ''    dr("Day") = CDate(seldr("VisitDate")).ToString("dd-MMM")
            ' ''    dr("CallVisited") = IIf(seldr("TotalCallVisited") Is DBNull.Value, 0, seldr("TotalCallVisited"))
            ' ''    dr("ProductiveCall") = IIf(seldr("TotalProductiveCall") Is DBNull.Value, 0, seldr("TotalProductiveCall"))
            ' ''    dr("CollectionCalls") = IIf(seldr("CollectionCalls") Is DBNull.Value, 0, seldr("CollectionCalls"))
            ' ''    dr("Sales") = IIf(seldr("Sales") Is DBNull.Value, 0, seldr("Sales"))
            ' ''    dr("Returns") = IIf(seldr("TotReturns") Is DBNull.Value, 0, seldr("TotReturns"))

            ' ''    dr("TotSales") = CDbl(dr("Sales")) + CDbl(dr("Returns"))

            ' ''    If seldr("StartTime") Is DBNull.Value Then
            ' ''        dr("StartTime") = ""
            ' ''    Else
            ' ''        dr("StartTime") = CDate(seldr("StartTime")).ToString("hh:mm tt")
            ' ''    End If
            ' ''    '' dr("StartTime") = IIf(seldr("StartTime") Is DBNull.Value, "", CDate(seldr("StartTime")).ToString("hh:mm tt"))

            ' ''    If seldr("EndTime") Is DBNull.Value Then
            ' ''        dr("EndTime") = ""
            ' ''    Else
            ' ''        dr("EndTime") = CDate(seldr("EndTime")).ToString("hh:mm tt")
            ' ''    End If

            ' ''    ''  dr("EndTime") = IIf(seldr("EndTime") Is DBNull.Value, 0, CDate(seldr("EndTime")).ToString("hh:mm tt"))
            ' ''    dtFinal.Rows.Add(dr)
            ' ''Next

            ' ''dtFinal = dtFinal.Select("1=1", "Day ASC").CopyToDataTable()

            gvRep.DataSource = dtFinal
            gvRep.DataBind()

            Dim query = From row In sourceTbl.Copy()
                Group row By FSR = row.Field(Of String)("FSR") Into VanGroup = Group
                Select New With {
                    Key FSR,
                    .TotalCallVisited = VanGroup.Sum(Function(r) r.Field(Of Int32)("TotalCallVisited")),
                    .TotalProductiveCall = VanGroup.Sum(Function(r) r.Field(Of Int32)("TotalProductiveCall")),
                    .Sales = VanGroup.Sum(Function(r) r.Field(Of Decimal)("Sales")) - VanGroup.Sum(Function(r) r.Field(Of Decimal)("TotReturns"))
               }


            '' Forming Summary Chart
            summaryChart.DataSource = query
            summaryChart.DataBind()

            log.Debug("Summary chart binding done")


            '' Getting Distinct dates for X axis 
            Dim distinctcol = From row In sourceTbl.Copy()
              Group row By Dat = New With {
                         Key .VisitDate = row.Field(Of DateTime)("VisitDate").ToString("dd-MMM")
               } Into VanGroup = Group
              Select New With {
                    Key Dat,
                    Dat.VisitDate
               }

            Dim distinctcolCopy = distinctcol.ToList()

            '' Finding Top 5 Productivity 
            Dim topprodquerycol = From row In sourceTbl.Copy()
               Group row By FSR = row.Field(Of String)("FSR") Into VanGroup = Group
               Select New With {
                   Key FSR,
                   .productivty = VanGroup.Sum(Function(r) r.Field(Of Int32)("TotalProductiveCall")) / VanGroup.Sum(Function(r) r.Field(Of Int32)("TotalCallVisited"))
              }


            Dim topprodquery = topprodquerycol.Where(Function(r) r.productivty > 0).OrderByDescending(Function(r) r.productivty).Take(5)

            Dim prodquery = From row In sourceTbl.Copy()
                Group row By FSR = New With {
                         Key .Van = row.Field(Of String)("FSR"),
                         Key .VisitDate = row.Field(Of DateTime)("VisitDate").ToString("dd-MMM"),
                         Key .Productivity = row.Field(Of Int32)("TotalProductiveCall") / row.Field(Of Int32)("TotalCallVisited") * 100
               } Into VanGroup = Group
                Select New With {
                    Key FSR,
                     FSR.Van, FSR.VisitDate, FSR.Productivity
               }

            prodquery = prodquery.Where(Function(r) topprodquery.Any(Function(s) s.FSR = r.Van))



            '' Adding Serious

            log.Debug("Adding Serious Started")

            Dim CopyProdQuery = prodquery.ToList()

            For Each x In topprodquery.ToList()
                Dim vanData As New LineSeries()
                vanData.Name = x.FSR
                vanData.LabelsAppearance.Visible = False
                vanData.TooltipsAppearance.Color = System.Drawing.Color.White
                'vanData.TooltipsAppearance.DataFormatString = "{0} Volts, {1} mA"
                vanData.TooltipsAppearance.ClientTemplate = "Van : " & x.FSR & " <br/> Productivity  : #= value # "
                vanData.VisibleInLegend = True
                vanData.Appearance.Overlay.Gradient = HtmlChart.Enums.Gradients.None


                '' Adding Seriuos Items

                For Each itm In CopyProdQuery.Where(Function(r) r.Van = x.FSR)
                    If itm.Productivity > 0 Then
                        vanData.Items.Add(Format(itm.Productivity, "##0.00"))
                    Else
                        vanData.Items.Add(0)
                    End If
                Next

                prodChart.PlotArea.Series.Add(vanData)
                vanData = Nothing
            Next
       
            ' '' Forming productivity Chart

            prodChart.DataSource = distinctcolCopy.ToList()
            prodChart.DataBind()




            '' Sales Chart

            '' Finding Top 5 Sales 
            Dim topSalequerycol = From row In sourceTbl.Copy()
               Group row By FSR = row.Field(Of String)("FSR") Into VanGroup = Group
               Select New With {
                   Key FSR,
                   .Sales = VanGroup.Sum(Function(r) r.Field(Of Decimal)("Sales")) - VanGroup.Sum(Function(r) r.Field(Of Decimal)("TotReturns"))
              }


            Dim topsalequery = topSalequerycol.OrderByDescending(Function(r) r.Sales).Take(5)



            Dim salequery = From row In sourceTbl.Copy()
                Group row By FSR = New With {
                         Key .Van = row.Field(Of String)("FSR"),
                         Key .VisitDate = row.Field(Of DateTime)("VisitDate").ToString("dd-MMM"),
                         Key .Sale = row.Field(Of Decimal)("Sales") - row.Field(Of Decimal)("TotReturns")
               } Into VanGroup = Group
                Select New With {
                    Key FSR,
                     FSR.Van, FSR.VisitDate, FSR.Sale
               }

            salequery = salequery.Where(Function(r) topsalequery.Any(Function(s) s.FSR = r.Van))



                '' Adding Serious

            log.Debug("Adding Serious Started")

            Dim CopySaleQuery = salequery.ToList()

            For Each x In topsalequery.ToList()
                Dim vanData As New LineSeries()
                vanData.Name = x.FSR
                vanData.LabelsAppearance.Visible = False
                vanData.TooltipsAppearance.Color = System.Drawing.Color.White
                'vanData.TooltipsAppearance.DataFormatString = "{0} Volts, {1} mA"
                vanData.TooltipsAppearance.ClientTemplate = "Van : " & x.FSR & " <br/> Sales  : #= value # "
                vanData.VisibleInLegend = True
                '' Adding Seriuos Items

                For Each itm In CopySaleQuery.Where(Function(r) r.Van = x.FSR)
                    If IsNumeric(itm.Sale) Then
                        vanData.Items.Add(itm.Sale)
                    Else
                        vanData.Items.Add(0)
                    End If
                    'If itm.Sale > 0 Then
                    '    vanData.Items.Add(itm.Sale)
                    'Else
                    '    vanData.Items.Add(0)
                    'End If
                Next

                saleChart.PlotArea.Series.Add(vanData)
                vanData = Nothing
            Next
           
            saleChart.DataSource = distinctcolCopy.ToList()
            saleChart.DataBind()


        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            ObjReport = Nothing
        End Try
    End Sub

    Private Sub gvRep_ColumnCreated(sender As Object, e As GridColumnCreatedEventArgs) Handles gvRep.ColumnCreated
        Try
            If e.Column.UniqueName = "Item" Then
                e.Column.HeaderText = " "
            End If

            '' Formatting Header

            If e.Column.UniqueName <> "Item" AndAlso e.Column.UniqueName <> "FSR" Then
                If IsDate(e.Column.HeaderText) Then
                    e.Column.HeaderText = "<font color='#0090d9'><b>" & CDate(e.Column.HeaderText).ToString("dd-MMM") & "</b></font>"
                End If
            End If

            ' '' No Wrap
            If TypeOf e.Column Is GridBoundColumn Then
                CType(e.Column, GridBoundColumn).DataFormatString = "<nobr>{0}</nobr>"
            End If
            'CType(e.Column, GridBoundColumn).DataFormatString = "<nobr>{0}</nobr>"

        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub gvRep_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles gvRep.ItemDataBound
        Try
            If TypeOf e.Item Is GridDataItem Then
                Dim item As GridDataItem = TryCast(e.Item, GridDataItem)
                For Each col As GridColumn In gvRep.MasterTableView.AutoGeneratedColumns

                    'If col.UniqueName = "Item" Then
                    '    item(col.UniqueName).Font.Bold = True
                    'End If
                Next
            End If

            If TypeOf e.Item Is GridGroupHeaderItem Then
                Dim groupHeader As GridGroupHeaderItem = DirectCast(e.Item, GridGroupHeaderItem)
                If True Then
                    groupHeader.DataCell.Text = groupHeader.DataCell.Text.Split(":"c)(1).ToString()
                    If groupHeader.DataCell.Text.Trim = "-1" Then
                        groupHeader.DataCell.Text = "N/A"
                    End If
                End If
            End If

        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub gvRep_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        BindData()
    End Sub

    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender
        Try
            gvRep.MasterTableView.GetColumn("FSR").Visible = False
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub gvRep_SortCommand(sender As Object, e As GridSortCommandEventArgs) Handles gvRep.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindData()
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
    Protected Sub SearchBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SearchBtn.Click
        Try
            Args.Visible = False
            divCurrency.Visible = False
            gvRep.Visible = False
            DailyReptab.Visible = False
            summaryChart.Visible = False
            prodChart.Visible = False
            saleChart.Visible = False
            If ValidateInputs() Then
                prodChart.PlotArea.Series.Clear()
                saleChart.PlotArea.Series.Clear()
                BindData()
            End If
        Catch ex As Exception
            log.Error(ex.Message)
        End Try

        '' ''If ddl_org.SelectedItem.Value = "0" Then
        '' ''    MessageBoxValidation("Please select the organization", "Validation")
        '' ''    SetFocus(ddl_org)
        '' ''    Exit Sub
        '' ''End If

        '' ''If Not IsDate(txt_fromDate.SelectedDate) Then
        '' ''    MessageBoxValidation("Enter valid ""From date"".", "Validation")
        '' ''    SetFocus(txt_fromDate)
        '' ''    Exit Sub
        '' ''End If

        '' ''If Not IsDate(txt_ToDate.SelectedDate) Then
        '' ''    MessageBoxValidation("Enter valid ""To date"".", "Validation")
        '' ''    SetFocus(txt_ToDate)
        '' ''    Exit Sub
        '' ''End If

        '' ''If CDate(txt_fromDate.SelectedDate) > CDate(txt_ToDate.SelectedDate) Then
        '' ''    MessageBoxValidation("Start Date should not be greater than End Date.", "Validation")
        '' ''    Exit Sub
        '' ''End If

        '' ''Dim VanIds As String = ""
        '' ''For Each Van As ListItem In chkSalesRep.Items
        '' ''    If Van.Selected = True Then
        '' ''        VanIds = VanIds & Van.Value & ","
        '' ''    End If
        '' ''Next

        '' ''If VanIds = "" Then
        '' ''    VanIds = "-1"
        '' ''End If
        '' ''Dim Fromdate As String
        '' ''Dim Todate As String
        '' ''Fromdate = txt_fromDate.SelectedDate
        '' ''Todate = txt_ToDate.SelectedDate

        '' ''Dim OrgID As New ReportParameter
        '' ''OrgID = New ReportParameter("OrgId", ddl_org.SelectedItem.Value)

        '' ''Dim VanID As New ReportParameter
        '' ''VanID = New ReportParameter("FSRID", VanIds)

        '' ''Dim Start_Date As New ReportParameter
        '' ''Start_Date = New ReportParameter("Start_Date", Fromdate)

        '' ''Dim End_Date As New ReportParameter
        '' ''End_Date = New ReportParameter("End_Date", Todate)

        '' ''With RVMain
        '' ''    .Reset()
        '' ''    .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        '' ''    .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        '' ''    .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
        '' ''    .ServerReport.SetParameters(New ReportParameter() {VanID, Start_Date, End_Date, OrgID})
        '' ''    '.ServerReport.Refresh()
        '' ''    .Visible = True
        '' ''End With
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
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

            Dim vantxt As String = ""
            Dim van As String = ""
            Dim collection As IList(Of RadComboBoxItem) = ddl_Van.CheckedItems
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","
                vantxt = vantxt & li.Text & ","

            Next
            If vantxt <> "" Then
                vantxt = vantxt.Substring(0, vantxt.Length - 1)
            End If

            If van = "" Then
                vantxt = "All"
            End If

            If van = "" Then
                van = "-1"

                '' '' if no items selected
                ''For Each li As RadComboBoxItem In ddl_Van.Items
                ''    If li.Value > 0 Then
                ''        van = van & li.Value & ","
                ''    End If

                ''Next

            End If



            Dim Fromdate As String
            Dim Todate As String
            Fromdate = CDate(txt_fromDate.SelectedDate).ToString("dd-MMM-yyyy")
            Todate = CDate(txt_ToDate.SelectedDate).ToString("dd-MMM-yyyy")

            Dim OrgID As New ReportParameter
            OrgID = New ReportParameter("OrgId", ddl_org.SelectedItem.Value)

            Dim VanID As New ReportParameter
            VanID = New ReportParameter("FSRID", van)

            Dim Start_Date As New ReportParameter
            Start_Date = New ReportParameter("Start_Date", Fromdate)

            Dim End_Date As New ReportParameter
            End_Date = New ReportParameter("End_Date", Todate)

            Dim Uid As New ReportParameter
            Uid = New ReportParameter("Uid", CType(Session.Item("USER_ACCESS"), UserAccess).UserID)



            rview.ServerReport.SetParameters(New ReportParameter() {OrgID, VanID, Start_Date, End_Date, Uid})

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
                Response.AddHeader("Content-disposition", "attachment;filename=DailyReport.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=DailyReport.xls")
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
        ddl_org.ClearSelection()
        If ddl_org.Items.Count = 2 Then
            ddl_org.SelectedIndex = 1
        End If
        ddl_Van.ClearCheckedItems()
        LoadVan()
        txt_fromDate.SelectedDate = FirstDayOfMonth(Now().Date)
        txt_ToDate.SelectedDate = Now().Date
        Args.Visible = False
        divCurrency.Visible = False
        gvRep.Visible = False
        DailyReptab.Visible = False
        summaryChart.Visible = False
        prodChart.Visible = False
        saleChart.Visible = False
    End Sub
End Class