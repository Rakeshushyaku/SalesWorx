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

Public Class Rep_VanPerformance_ASR
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "VanPerformance_ASR"

    Private Const PageID As String = "P356"
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
                ObjCommon = New SalesWorx.BO.Common.Common()
                Dim CountryTbl As DataTable = Nothing
                Dim orgTbl As DataTable = Nothing

                CountryTbl = ObjCommon.GetCountry(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)
                ddlCountry.DataSource = CountryTbl
                ddlCountry.DataBind()


                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)

                Dim s() As String = Nothing
                Dim Currency As String = Nothing
                Dim DecimalDigits As String = "2"
                Dim country As String = Nothing
                If CountryTbl.Rows.Count = 1 Then

                    ddlCountry.SelectedIndex = 0
                    dvCountry.Visible = False

                    s = ddlCountry.SelectedValue.Split("$")

                    If s.Length > 0 Then
                        country = s(0).ToString()
                        Currency = s(1).ToString()
                        DecimalDigits = s(2).ToString()
                    End If


                    ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
                    ddlOrganization.DataBind()


                ElseIf CountryTbl.Rows.Count > 1 Then
                    ddlCountry.SelectedIndex = 0
                    dvCountry.Visible = True


                    s = ddlCountry.SelectedValue.Split("$")

                    If s.Length > 0 Then
                        country = s(0).ToString()
                        Currency = s(1).ToString()
                        DecimalDigits = s(2).ToString()
                    End If

                    ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
                    ddlOrganization.DataBind()

                End If

                Dim OrgStr As String = Nothing
                For Each item As RadComboBoxItem In ddlOrganization.Items
                    item.Checked = True
                    If item.Checked Then

                        OrgStr = OrgStr & "," & item.Value

                    End If
                Next


                LoadVan()

                ddlSalesDist.DataSource = ObjCommon.GetSalesDistrictList(Err_No, Err_Desc, SubQry)
                ddlSalesDist.DataBind()
                ddlSalesDist.Items.Insert(0, New RadComboBoxItem("Sales District", "0"))
                txtFromDate.SelectedDate = DateAdd(DateInterval.Month, -2, Now)
                txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")
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

    Private Sub ddlCountry_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlCountry.SelectedIndexChanged
        LoadOrgs()
        LoadVan()
    End Sub
    Sub LoadOrgs()
        Dim orgTbl As DataTable = Nothing

        ObjCommon = New SalesWorx.BO.Common.Common()


        Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)



        orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)

        Dim s() As String = Nothing
        Dim Currency As String = Nothing
        Dim DecimalDigits As String = "2"
        Dim country As String = Nothing
        s = ddlCountry.SelectedValue.Split("$")

        If s.Length > 0 Then
            country = s(0).ToString()
            Currency = s(1).ToString()
            DecimalDigits = s(2).ToString()
        End If


        ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
        ddlOrganization.DataBind()



        Dim OrgStr As String = Nothing
        For Each item As RadComboBoxItem In ddlOrganization.Items
            item.Checked = True
            If item.Checked Then

                OrgStr = OrgStr & "," & item.Value

            End If
        Next

    End Sub
    Sub LoadVan()

        Try

            '    If Not (ddlOrganization.SelectedItem.Value = "") Then

            '        Dim objUserAccess As UserAccess
            '        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

            '        ObjCommon = New SalesWorx.BO.Common.Common()

            '        ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
            '        ddlVan.DataValueField = "SalesRep_ID"
            '        ddlVan.DataTextField = "SalesRep_Name"
            '        ddlVan.DataBind()
            '        ''  ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --"))
            '        If Not ddlVan.Items.FindItemByValue(Request.QueryString("ID")) Is Nothing Then
            '            ddlVan.ClearSelection()
            '            ddlVan.Items.FindItemByValue(Request.QueryString("ID")).Selected = True
            '        End If

            '        For Each itm As RadComboBoxItem In ddlVan.Items
            '            itm.Checked = True
            '        Next
            '    Else
            '        ddlVan.Items.Clear()
            '    End If

            If Not ddlOrganization.CheckedItems Is Nothing Then
                If ddlOrganization.CheckedItems.Count > 0 Then
                    Dim objUserAccess As UserAccess
                    objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
                    Dim ObjRep As SalesWorx.BO.Common.Reports = New SalesWorx.BO.Common.Reports()

                    Dim OrgStr As String = ""
                    For Each li As RadComboBoxItem In ddlOrganization.CheckedItems
                        OrgStr = OrgStr & li.Value & ","
                    Next
                    ddlVan.DataSource = ObjRep.GetAllOrgVan(Err_No, Err_Desc, OrgStr, objUserAccess.UserID.ToString())
                    ddlVan.DataBind()

                    For Each itm As RadComboBoxItem In ddlVan.Items
                        itm.Checked = True
                    Next

                    Dim dtcurrency As DataTable
                    Dim ObjReport As New SalesWorx.BO.Common.Reports
                    dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, ddlOrganization.CheckedItems(0).Value)


                Else
                    ddlVan.Items.Clear()
                End If
            Else
                ddlVan.Items.Clear()
            End If

        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub ddlOrganization_CheckAllCheck(sender As Object, e As RadComboBoxCheckAllCheckEventArgs) Handles ddlOrganization.CheckAllCheck
        LoadVan()
    End Sub

    Private Sub ddlOrganization_ItemChecked(sender As Object, e As RadComboBoxItemEventArgs) Handles ddlOrganization.ItemChecked
        LoadVan()
    End Sub

     

    Private Sub SearchBtn_Click(sender As Object, e As EventArgs) Handles SearchBtn.Click
        Try

            Args.Visible = False
            gvRep.Visible = False
            If ValidateInputs() Then
                gvRep.Visible = True
                 
                gvRep.Rebind()
                BindData()
            End If

        Catch ex As Exception
            log.Error(ex.Message())
        End Try
    End Sub

    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False

        Dim d1 As DateTime = txtFromDate.SelectedDate
        Dim d2 As DateTime = txtToDate.SelectedDate
        Dim M As Integer = Math.Abs((d1.Year - d2.Year))
        Dim months As Integer = Math.Abs(DateDiff(DateInterval.Month, d1, d2))

        If ddlOrganization.CheckedItems Is Nothing Then
            MessageBoxValidation("Select an Organisation", "Validation")
            SetFocus(ddlOrganization)
            Return bretval

        Else
            If ddlOrganization.CheckedItems.Count <= 0 Then
                MessageBoxValidation("Select an Organisation", "Validation")
                SetFocus(ddlOrganization)
                Return bretval
            End If
        End If
        bretval = True
        Return bretval
         
    End Function

    Sub MessageBoxValidation(ByVal str As String, Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Private Sub BindData()
        Try

            rpbFilter.Items(0).Expanded = False
            Args.Visible = False

            Dim vantxt As String = ""
            Dim van As String = ""
            Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems
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

            Dim Orgtxt As String = ""
            Dim Org As String = ""
            Dim Orgcollection As IList(Of RadComboBoxItem) = ddlOrganization.CheckedItems
            For Each li As RadComboBoxItem In Orgcollection
                Org = Org & li.Value & ","
                Orgtxt = Orgtxt & li.Text & ","
            Next
            If Orgtxt <> "" Then
                Orgtxt = Orgtxt.Substring(0, Orgtxt.Length - 1)
            End If
            If Org = "" Then
                Org = "0"
            End If

            lbl_org.Text = Orgtxt

            lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("MMM-yyyy")
            lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("MMM-yyyy")
            lbl_district.Text = ddlSalesDist.SelectedItem.Text
            Args.Visible = True


            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable

            Dim fromdate As DateTime
            fromdate = CDate(txtFromDate.SelectedDate).Month & "/01/" & CDate(txtFromDate.SelectedDate).Year

            Dim ToDate As DateTime
            ToDate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(CDate(txtToDate.SelectedDate).Month & "/01/" & CDate(txtToDate.SelectedDate).Year)))


            dt = ObjReport.GetVanPerformance_ASR(Err_No, Err_Desc, Org, fromdate.ToString("dd-MMM-yyyy"), ToDate.ToString("dd-MMM-yyyy"), van, ddlSalesDist.SelectedItem.Value, CType(Session("User_Access"), UserAccess).UserID)


            '' Formatting result for pivot grid 

            Dim dtFinal As New DataTable
            dtFinal.Columns.Add("Description")
            dtFinal.Columns.Add("Year")
            dtFinal.Columns.Add("Type")
            dtFinal.Columns.Add("C1", Type.GetType("System.Int32"))
            dtFinal.Columns.Add("C2", Type.GetType("System.Int32"))
            dtFinal.Columns.Add("C3", Type.GetType("System.Double"))
            dtFinal.Columns.Add("C4", Type.GetType("System.Int32"))
            dtFinal.Columns.Add("C5", Type.GetType("System.Int32"))
            dtFinal.Columns.Add("C6", Type.GetType("System.Double"))


            Dim DtMonths As New DataTable
            DtMonths = dt.DefaultView.ToTable(True, "FromDate", "SalesRep_Name")

            For Each seldr In DtMonths.Rows
                Dim dr As DataRow
                dr = dtFinal.NewRow
                dr("Description") = seldr("SalesRep_Name").ToString
                dr("Year") = CDate(seldr("FromDate").ToString).ToString("MMM-yyyy")

                Dim vpdr() As DataRow
                vpdr = dt.Select("SalesRep_Name='" & seldr("SalesRep_Name").ToString & "' and FromDate='" & seldr("FromDate") & "'")
                dr("Type") = "Calls"
                dr("C1") = CInt(Val(vpdr(0)("TotalCalls").ToString()))
                dr("C2") = CInt(Val(vpdr(0)("TotalProductiveCalls").ToString()))
                dr("C3") = Val(vpdr(0)("CallProductivity").ToString())
                dr("C4") = CInt(Val(vpdr(0)("JPCalls").ToString()))
                dr("C5") = CInt(Val(vpdr(0)("ActJPCalls").ToString()))
                dr("C6") = Val(vpdr(0)("Adherence").ToString())
                dtFinal.Rows.Add(dr)

                Dim drC As DataRow
                drC = dtFinal.NewRow
                drC("Description") = seldr("SalesRep_Name").ToString
                drC("Year") = CDate(seldr("FromDate").ToString).ToString("MMM-yyyy")
                drC("Type") = "Customers"
                drC("C1") = CInt(Val(vpdr(0)("UniqueCustVisted").ToString()))
                drC("C2") = CInt(Val(vpdr(0)("UniqueProdCust").ToString()))
                drC("C3") = Val(vpdr(0)("CustProductivity").ToString())
                drC("C4") = 0
                drC("C5") = 0
                drC("C6") = 0
                dtFinal.Rows.Add(drC)
            Next
             

            gvRep.DataSource = dtFinal
            gvRep.DataBind()
            If dtFinal.Rows.Count > 0 Then
                img_export.Visible = True
            Else
                img_export.Visible = False
            End If
            ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "UpdateHeader();", True)
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
    Private Sub gvRep_CellFormatting(sender As Object, e As PivotGridItemEventArgs)

    End Sub
    Private Sub gvRep_CellDataBound(sender As Object, e As PivotGridCellDataBoundEventArgs) Handles gvRep.CellDataBound
        If TypeOf e.Cell Is PivotGridColumnHeaderCell Then
            If e.Cell.Text.IndexOf("Sum of") >= 0 Then
                e.Cell.Text = e.Cell.Text.Replace("Sum of", "")
            End If

        End If

        If TypeOf e.Cell Is PivotGridColumnHeaderCell Then
            If e.Cell.ColumnSpan = 12 Then
                ' e.Cell.ColumnSpan = 9
                e.Cell.Attributes.Add("style", "background-image: none")
                e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#E7E6E6")
                e.Cell.ForeColor = System.Drawing.Color.Black
            End If
            If e.Cell.Text = "Customers" Then
                e.Cell.Attributes.Add("style", "background-image: none")
                e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#97c95d")
                e.Cell.ForeColor = System.Drawing.Color.White
                'e.Cell.ColumnSpan = 3
            ElseIf e.Cell.Text = "Calls" Then
                e.Cell.Attributes.Add("style", "background-image: none")
                e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#14b4fc")
                e.Cell.ForeColor = System.Drawing.Color.White
            ElseIf e.Cell.DataItem.ToString = "Sum of C1" Then
                Dim k() As Object = DirectCast(e.Cell, Telerik.Web.UI.PivotGridColumnHeaderCell).ParentIndexes
                If k(1).ToString().ToLower = "calls" Then
                    If CType(Session.Item("CONTROL_PARAMS"), ControlParams).USE_DISTR_IN_CALLS = "Y" Then
                        e.Cell.Text = "Total Calls <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Visits in which Distribution Check was performed'></i>"
                    Else
                        e.Cell.Text = "Total Calls <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Visits done'></i>"
                    End If

                    e.Cell.Attributes.Add("style", "background-image: none")
                    e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#F97166")
                    e.Cell.ForeColor = System.Drawing.Color.White
                Else
                    If CType(Session.Item("CONTROL_PARAMS"), ControlParams).USE_DISTR_IN_CALLS = "Y" Then
                        e.Cell.Text = "Unique Customers Covered <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Unique customers visited where Distribution Check was performed'></i>"
                    Else
                        e.Cell.Text = "Unique Customers Covered <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Unique customers visited'></i>"
                    End If
                    e.Cell.Attributes.Add("style", "background-image: none")
                    e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#398C59")
                    e.Cell.ForeColor = System.Drawing.Color.White
                End If
            ElseIf e.Cell.DataItem.ToString = "Sum of C2" Then
                    Dim k() As Object = DirectCast(e.Cell, Telerik.Web.UI.PivotGridColumnHeaderCell).ParentIndexes
                    If k(1).ToString().ToLower = "calls" Then
                    e.Cell.Text = "Total Productive Calls <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Visits in which Sales Invoice was generated'></i>"
                        e.Cell.Attributes.Add("style", "background-image: none")
                        e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#36C5B2")
                        e.Cell.ForeColor = System.Drawing.Color.White
                    Else
                    e.Cell.Text = "Unique Productive Customers <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Unique customers who have billed'></i>"
                        e.Cell.Attributes.Add("style", "background-image: none")
                        e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#A5304D")
                        e.Cell.ForeColor = System.Drawing.Color.White
                    End If
            ElseIf e.Cell.DataItem.ToString = "Sum of C3" Then
                    Dim k() As Object = DirectCast(e.Cell, Telerik.Web.UI.PivotGridColumnHeaderCell).ParentIndexes
                    If k(1).ToString().ToLower = "calls" Then
                    e.Cell.Text = "Call Productive % <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Productive calls Vs Total Calls'></i>"
                        e.Cell.Attributes.Add("style", "background-image: none")
                        e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#EB963C")
                        e.Cell.ForeColor = System.Drawing.Color.White
                    Else
                    e.Cell.Text = "Customer Productive % <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Productive customers Vs Customers covered'></i>"
                        e.Cell.Attributes.Add("style", "background-image: none")
                        e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#F97166")
                        e.Cell.ForeColor = System.Drawing.Color.White
                    End If
            ElseIf e.Cell.DataItem.ToString = "Sum of C4" Then
                    Dim k() As Object = DirectCast(e.Cell, Telerik.Web.UI.PivotGridColumnHeaderCell).ParentIndexes
                    If k(1).ToString().ToLower = "customers" Then
                        e.Cell.Width = 0
                        e.Cell.Text = ""
                        e.Cell.Attributes.Add("style", "visibility:hidden")
                        e.Cell.Attributes.Add("style", "padding: 0 !important")
                    Else
                    e.Cell.Text = "JP Calls <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Visits planned'></i>"
                        e.Cell.Attributes.Add("style", "background-image: none")
                        e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#36C5B2")
                        e.Cell.ForeColor = System.Drawing.Color.White
                    End If
            ElseIf e.Cell.DataItem.ToString = "Sum of C5" Then
                    Dim k() As Object = DirectCast(e.Cell, Telerik.Web.UI.PivotGridColumnHeaderCell).ParentIndexes
                    If k(1).ToString().ToLower = "customers" Then
                        e.Cell.Width = 0
                        e.Cell.Text = ""
                        e.Cell.Attributes.Add("style", "visibility:hidden")
                        e.Cell.Attributes.Add("style", "padding: 0 !important")
                    Else
                    e.Cell.Text = "Actual Calls as per JP <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Visits as per route plan'></i>"
                        e.Cell.Attributes.Add("style", "background-image: none")
                        e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#36C5B2")
                        e.Cell.ForeColor = System.Drawing.Color.White
                    End If
            ElseIf e.Cell.DataItem.ToString = "Sum of C6" Then
                    Dim k() As Object = DirectCast(e.Cell, Telerik.Web.UI.PivotGridColumnHeaderCell).ParentIndexes
                    If k(1).ToString().ToLower = "customers" Then
                        e.Cell.Width = 0
                        e.Cell.Text = ""
                        e.Cell.Attributes.Add("style", "visibility:hidden")
                        e.Cell.Attributes.Add("style", "padding: 0 !important")
                    Else
                    e.Cell.Text = "JP Adherence % <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Route Plan Adherence percentage'></i>"
                        e.Cell.Attributes.Add("style", "background-image: none")
                        e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#36C5B2")
                        e.Cell.ForeColor = System.Drawing.Color.White
                    End If
            End If
        End If

        If TypeOf e.Cell Is PivotGridDataCell Then

            Dim cell As PivotGridDataCell = TryCast(e.Cell, PivotGridDataCell)

            If Not cell Is Nothing Then
                If cell.Field.DataField = "C4" Then
                    Dim k() As Object = cell.ParentColumnIndexes
                    If k(1).ToString().ToLower = "customers" Then
                        cell.Width = 0
                        cell.Text = ""
                        cell.Attributes.Add("style", "visibility:hidden")
                        cell.Attributes.Add("style", "padding: 0 !important")
                    End If
                End If
                If cell.Field.DataField = "C5" Then
                    Dim k() As Object = cell.ParentColumnIndexes
                    If k(1).ToString().ToLower = "customers" Then
                        cell.Width = 0
                        cell.Text = ""
                        cell.Attributes.Add("style", "visibility:hidden")
                        cell.Attributes.Add("style", "padding: 0 !important")
                    End If
                End If
                If cell.Field.DataField = "C6" Then
                    Dim k() As Object = cell.ParentColumnIndexes
                    If k(1).ToString().ToLower = "customers" Then
                        cell.Width = 0
                        cell.Text = ""
                        cell.Attributes.Add("style", "visibility:hidden")
                        cell.Attributes.Add("style", "padding: 0 !important")
                    End If
                End If
            End If

            '        If cell.Field.DataField = "C4" Then
            '            If cell.ColumnIndex = 9 Or cell.ColumnIndex = 21 Or cell.ColumnIndex = 33 Then
            '                cell.Width = 0
            '                cell.Visible = False
            '                cell.Attributes.Add("style", "display:none")
            '            End If
            '        End If
            '        If cell.Field.DataField = "C5" Then
            '            If cell.ColumnIndex = 10 Or cell.ColumnIndex = 22 Or cell.ColumnIndex = 34 Then
            '                cell.Width = 0
            '                cell.Visible = False
            '                cell.Attributes.Add("style", "display:none")
            '            End If
            '        End If
            '        If cell.Field.DataField = "C6" Then
            '            If cell.ColumnIndex = 11 Or cell.ColumnIndex = 23 Or cell.ColumnIndex = 35 Then
            '                cell.Width = 0
            '                cell.Visible = False
            '                cell.Attributes.Add("style", "display:none")
            '            End If
            '        End If

            '    End If
        End If


    End Sub

    Private Sub gvRep_PageIndexChanged(sender As Object, e As PivotGridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        BindData()
    End Sub

    Private Sub gvRep_PageSizeChanged(sender As Object, e As PivotGridPageSizeChangedEventArgs) Handles gvRep.PageSizeChanged
        BindData()
    End Sub

    Private Sub gvRep_PreRender(sender As Object, e As EventArgs) Handles gvRep.PreRender
        Try
            '    Dim pager As PivotGridPagerItem = TryCast(gvRep.GetItems(PivotGridItemType.PagerItem)(0), PivotGridPagerItem)
            '    Dim combo As RadComboBox = TryCast(pager.FindControl("PageSizeComboBox"), RadComboBox)
            '    combo.Visible = False

            '    Dim pagelbl As Label = TryCast(pager.FindControl("ChangePageSizeLabel"), Label)
            '    If pagelbl IsNot Nothing Then
            '        pagelbl.Visible = False
            '    End If

            'For i As Integer = 0 To gvRep.Fields.Count - 1
            '    If gvRep.Fields(i).Caption = "Name" Then
            '        gvRep.Fields(i).Caption = Me.ddlDisplayMode.SelectedItem.Text
            '    End If
            'Next
            'gvRep.Rebind()

        Catch ex As Exception
            log.Error(ex.Message)
        End Try
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

             

            Dim fromdate As DateTime
            fromdate = CDate(txtFromDate.SelectedDate).Month & "/01/" & CDate(txtFromDate.SelectedDate).Year

            Dim ToDate As DateTime
            ToDate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(CDate(txtToDate.SelectedDate).Month & "/01/" & CDate(txtToDate.SelectedDate).Year)))



            Dim Orgtxt As String = ""
            Dim Org As String = ""
            Dim Orgcollection As IList(Of RadComboBoxItem) = ddlOrganization.CheckedItems
            For Each li As RadComboBoxItem In Orgcollection
                Org = Org & li.Value & ","
                Orgtxt = Orgtxt & li.Text & ","
            Next
            If Orgtxt <> "" Then
                Orgtxt = Orgtxt.Substring(0, Orgtxt.Length - 1)
            End If
            If Org = "" Then
                Org = "0"
            End If


            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("OID", Org)



            Dim OrgName As New ReportParameter
            OrgName = New ReportParameter("OrgName", Orgtxt)


            Dim vantxt As String = ""
            Dim van As String = ""
            Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems
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


            If van = "" Then
                vantxt = "All"
            End If

            If ddlSalesDist.SelectedItem.Value = "0" Then
                lbl_district.Text = "All"
            Else
                lbl_district.Text = ddlSalesDist.SelectedItem.Text
            End If


            Dim SalesRep As New ReportParameter
            SalesRep = New ReportParameter("SalesRep", vantxt)


            Dim FDate As New ReportParameter
            FDate = New ReportParameter("FromDate", fromdate.ToString("dd-MMM-yyyy"))

            Dim TDate As New ReportParameter
            TDate = New ReportParameter("ToDate", ToDate.ToString("dd-MMM-yyyy"))

            Dim SID As New ReportParameter
            SID = New ReportParameter("SID", van)


            Dim SalesDistrictID As New ReportParameter
            SalesDistrictID = New ReportParameter("SalesDistrictID", CStr(ddlSalesDist.SelectedItem.Value))


            Dim UID As New ReportParameter
            UID = New ReportParameter("Uid", CType(Session("User_Access"), UserAccess).UserID)
           
            rview.ServerReport.SetParameters(New ReportParameter() {FDate, TDate, OrgId, SID, OrgName, SalesRep, SalesDistrictID, UID})

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
                Response.AddHeader("Content-disposition", "attachment;filename=VanPerformance.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=VanPerformance.xls")
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
        ObjCommon = New SalesWorx.BO.Common.Common()
        Dim CountryTbl As DataTable = Nothing
        Dim orgTbl As DataTable = Nothing

        CountryTbl = ObjCommon.GetCountry(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)
        ddlCountry.DataSource = CountryTbl
        ddlCountry.DataBind()


        Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
        orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)

        Dim s() As String = Nothing
        Dim Currency As String = Nothing
        Dim DecimalDigits As String = "2"
        Dim country As String = Nothing
        If CountryTbl.Rows.Count = 1 Then

            ddlCountry.SelectedIndex = 0
            dvCountry.Visible = False

            s = ddlCountry.SelectedValue.Split("$")

            If s.Length > 0 Then
                country = s(0).ToString()
                Currency = s(1).ToString()
                DecimalDigits = s(2).ToString()
            End If


            ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
            ddlOrganization.DataBind()


        ElseIf CountryTbl.Rows.Count > 1 Then
            ddlCountry.SelectedIndex = 0
            dvCountry.Visible = True


            s = ddlCountry.SelectedValue.Split("$")

            If s.Length > 0 Then
                country = s(0).ToString()
                Currency = s(1).ToString()
                DecimalDigits = s(2).ToString()
            End If

            ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
            ddlOrganization.DataBind()

        End If

        Dim OrgStr As String = Nothing
        For Each item As RadComboBoxItem In ddlOrganization.Items
            item.Checked = True
            If item.Checked Then

                OrgStr = OrgStr & "," & item.Value

            End If
        Next


        LoadVan()
        ddlSalesDist.ClearSelection()

        txtFromDate.SelectedDate = DateAdd(DateInterval.Month, -2, Now)
        txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")
        gvRep.Visible = False
        Args.Visible = False
        img_export.Visible = False
    End Sub
    Private Sub BtnExportBiffExcel_Click(sender As Object, e As EventArgs) Handles BtnExportBiffExcel.Click


        Dim van As String = ""
        Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems
        For Each li As RadComboBoxItem In collection
            van = van & li.Value & ","


        Next
         
        If van = "" Then
            van = "0"
        End If


        


        Dim Org As String = ""
        Dim Orgcollection As IList(Of RadComboBoxItem) = ddlOrganization.CheckedItems
        For Each li As RadComboBoxItem In Orgcollection
            Org = Org & li.Value & ","

        Next
         
        If Org = "" Then
            Org = "0"
        End If


        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        Dim ObjReport As New SalesWorx.BO.Common.Reports
        Dim dt As New DataTable

        Dim fromdate As DateTime
        fromdate = CDate(txtFromDate.SelectedDate).Month & "/01/" & CDate(txtFromDate.SelectedDate).Year

        Dim ToDate As DateTime
        ToDate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(CDate(txtToDate.SelectedDate).Month & "/01/" & CDate(txtToDate.SelectedDate).Year)))


        dt = ObjReport.GetVanPerformance_ASR(Err_No, Err_Desc, Org, fromdate.ToString("dd-MMM-yyyy"), ToDate.ToString("dd-MMM-yyyy"), van, ddlSalesDist.SelectedItem.Value, CType(Session("User_Access"), UserAccess).UserID)

        Dim Dtsp As New DataTable
        Dtsp = dt.DefaultView.ToTable(True, "SalesRep_Name")

        If dt.Rows.Count > 0 Then


            Using package As New ExcelPackage()

                Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")




                Dim tdate As DateTime
                Dim Toprow As Integer = 1
                Dim TopCol1 As Integer = 2
                Dim TopCol2 As Integer = 2
                Dim TopCol3 As Integer = 8
                Dim TopDrow As Integer = 4
                tdate = fromdate
                While tdate <= ToDate
                    Worksheet.Cells(Toprow, TopCol1).Value = tdate.ToString("MMM-yyyy")
                    Worksheet.Cells(Toprow, TopCol2, Toprow, TopCol3 + 2).Merge = True
                    Worksheet.Cells(Toprow, TopCol2, Toprow, TopCol3 + 2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center
                    Worksheet.Cells(Toprow + 1, TopCol2).Value = "Calls"


                    Worksheet.Cells(Toprow + 2, 1).Value = "Van"
                    Worksheet.Cells(Toprow + 2, TopCol2).Value = "Total Calls"
                    Worksheet.Cells(Toprow + 2, TopCol2 + 1).Value = "Total Productive Calls"
                    Worksheet.Cells(Toprow + 2, TopCol2 + 2).Value = "Call Productive %"
                    Worksheet.Cells(Toprow + 2, TopCol2 + 3).Value = "JP Calls"
                    Worksheet.Cells(Toprow + 2, TopCol2 + 4).Value = "Actual Calls as per JP"
                    Worksheet.Cells(Toprow + 2, TopCol2 + 5).Value = "JP Adherence %"

                    Worksheet.Cells(Toprow + 1, TopCol2, Toprow + 1, TopCol3 - 1).Merge = True
                    Worksheet.Cells(Toprow + 1, TopCol2, Toprow + 1, TopCol3 - 1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center
                    Worksheet.Cells(Toprow + 1, TopCol3, Toprow + 1, TopCol3 + 2).Merge = True
                    Worksheet.Cells(Toprow + 1, TopCol3, Toprow + 1, TopCol3 + 2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center

                    Worksheet.Cells(Toprow + 1, TopCol3).Value = "Customer"

                    Worksheet.Cells(Toprow + 2, TopCol3).Value = "Unique Customers Covered"
                    Worksheet.Cells(Toprow + 2, TopCol3 + 1).Value = "Unique Productive Customers"
                    Worksheet.Cells(Toprow + 2, TopCol3 + 2).Value = "Customer Productive %"
                    tdate = DateAdd(DateInterval.Month, 1, tdate)
                    TopCol1 = TopCol1 + 9
                    TopCol2 = TopCol2 + 9
                    TopCol3 = TopCol3 + 9
                End While
                'Worksheet.Cells(Toprow + 1, TopCol3, Toprow + 1, TopCol3 + 3).Merge = True
                For Each sdr In Dtsp.Rows
                    tdate = fromdate
                    TopCol2 = 2
                    TopCol3 = 8
                    While tdate <= ToDate
                        Dim seldr() As DataRow
                        seldr = dt.Select("SalesRep_Name='" & sdr("SalesRep_Name").ToString & "' and Fromdate='" & tdate & "'")
                        Worksheet.Cells(TopDrow, 1).Value = sdr("SalesRep_Name").ToString
                        If seldr.Length > 0 Then
                            Worksheet.Cells(TopDrow, TopCol2).Value = Val(seldr(0)("TotalCalls").ToString)
                            Worksheet.Cells(TopDrow, TopCol2 + 1).Value = Val(seldr(0)("TotalProductiveCalls").ToString)
                            Worksheet.Cells(TopDrow, TopCol2 + 2).Value = Val(seldr(0)("CallProductivity").ToString)
                            Worksheet.Cells(TopDrow, TopCol2 + 3).Value = Val(seldr(0)("JPCalls").ToString)
                            Worksheet.Cells(TopDrow, TopCol2 + 4).Value = Val(seldr(0)("ActJPCalls").ToString)
                            Worksheet.Cells(TopDrow, TopCol2 + 5).Value = Val(seldr(0)("Adherence").ToString)
                            Worksheet.Cells(TopDrow, TopCol3).Value = Val(seldr(0)("UniqueCustVisted").ToString)
                            Worksheet.Cells(TopDrow, TopCol3 + 1).Value = Val(seldr(0)("UniqueProdCust").ToString)
                            Worksheet.Cells(TopDrow, TopCol3 + 2).Value = Val(seldr(0)("CustProductivity").ToString)
                        Else
                            Worksheet.Cells(TopDrow, TopCol2).Value = 0
                            Worksheet.Cells(TopDrow, TopCol2 + 1).Value = 0
                            Worksheet.Cells(TopDrow, TopCol2 + 2).Value = 0
                            Worksheet.Cells(TopDrow, TopCol2 + 3).Value = 0
                            Worksheet.Cells(TopDrow, TopCol2 + 4).Value = 0
                            Worksheet.Cells(TopDrow, TopCol2 + 5).Value = 0
                            Worksheet.Cells(TopDrow, TopCol3).Value = 0
                            Worksheet.Cells(TopDrow, TopCol3 + 1).Value = 0
                            Worksheet.Cells(TopDrow, TopCol3 + 2).Value = 0
                        End If

                        TopCol1 = TopCol1 + 9
                        TopCol2 = TopCol2 + 9
                        TopCol3 = TopCol3 + 9
                        tdate = DateAdd(DateInterval.Month, 1, tdate)
                    End While
                    TopDrow = TopDrow + 1
                Next
                Worksheet.Cells.AutoFitColumns()
                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename= VanPerformance.xlsx")

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
End Class