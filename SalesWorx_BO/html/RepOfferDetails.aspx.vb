Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI

Partial Public Class RepOfferDetails
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "BonusDetails"

    Private Const PageID As String = "P337"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single

    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
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
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization"))

                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                    BindBrands()
                End If

                txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
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
    Private Sub BindData()
        Dim SearchQuery As String = ""
        Dim SalesRepId As Integer = 0
        Dim CustId As Integer = 0
        Dim fromdate As DateTime
        Dim todate As DateTime
        fromdate = System.Data.SqlTypes.SqlDateTime.Null
        todate = System.Data.SqlTypes.SqlDateTime.Null


        Try
            ObjCustomer = New Customer()
            ObjCommon = New SalesWorx.BO.Common.Common
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

            fromdate = CDate(txtFromDate.SelectedDate)
            todate = CDate(txtToDate.SelectedDate)

            If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then
                InitReportViewer(fromdate, todate, CType(Session("User_Access"), UserAccess).UserID)
            Else
                MessageBoxValidation("Select an Organization.")
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
    End Sub

    Private Sub InitReportViewer(ByVal fromdate As Date, ByVal Todate As Date, ByVal UID As Integer)
        Try

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

            Dim BrandStr As String = ""



            For Each item As RadComboBoxItem In ddlBrand.Items
                If item.Checked Then
                    BrandStr = BrandStr & "," & item.Value
                End If
            Next

            If BrandStr = "" Then
                For Each item As RadComboBoxItem In ddlBrand.Items
                    BrandStr = BrandStr & "," & item.Value
                Next
            End If



            Dim FDate As New ReportParameter
            FDate = New ReportParameter("Dat", fromdate.ToString())
            Dim EDate As New ReportParameter
            EDate = New ReportParameter("Dat1", Todate.ToString())

            Dim OID As New ReportParameter
            OID = New ReportParameter("OID", CStr(ddlOrganization.SelectedValue.ToString()))

            Dim OrgName As New ReportParameter
            OrgName = New ReportParameter("OrgName", CStr(ddlOrganization.SelectedItem.Text))

            Dim Brand As New ReportParameter
            Brand = New ReportParameter("Brand", BrandStr)

            Dim Mode As New ReportParameter
            Mode = New ReportParameter("Mode", CStr(Me.ddlMode.SelectedItem.Value))

            Dim Status As New ReportParameter
            Status = New ReportParameter("Status", CStr(Me.ddlStatus.SelectedItem.Value))


            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                .ServerReport.SetParameters(New ReportParameter() {OID, FDate, EDate, Brand, Mode, OrgName, Status})
                .ServerReport.Refresh()

            End With


        Catch Ex As Exception
            '  log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub

    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        Try
            If ddlOrganization.SelectedValue = "" Then
                MessageBoxValidation("Please select the organization", "Validation")
                Return bretval
                Exit Function
            End If

            If Not IsDate(txtFromDate.SelectedDate) Then
                MessageBoxValidation("Enter valid ""From date"".", "Validation")
                Return bretval
                Exit Function
            End If

            If Not IsDate(txtToDate.SelectedDate) Then
                MessageBoxValidation("Enter valid ""To date"".", "Validation")
                Return bretval
                Exit Function
            End If

            If CDate(txtFromDate.SelectedDate) > CDate(txtToDate.SelectedDate) Then
                MessageBoxValidation("Start Date should not be greater than End Date.", "Validation")
                Return bretval
                Exit Function
            End If

            Return True

        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Function

    Private Sub BindValues()
        Dim ObjReport As New SalesWorx.BO.Common.Reports

        Try


            Dim BrandStr As String = ""
            Dim BrandtxtStr As String = ""
            For Each item As RadComboBoxItem In ddlBrand.Items
                If item.Checked Then
                    BrandStr = BrandStr & "," & item.Value
                    BrandtxtStr = BrandtxtStr & item.Text & ","
                End If
            Next

            If BrandtxtStr <> "" Then
                BrandtxtStr = BrandtxtStr.Substring(0, BrandtxtStr.Length - 1)
            End If

            If BrandStr = "" Then
                For Each item As RadComboBoxItem In ddlBrand.Items
                    BrandStr = BrandStr & "," & item.Value
                Next
                BrandtxtStr = "All"
            End If


            lbl_org.Text = ddlOrganization.SelectedItem.Text
            lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_Type.Text = ddlMode.SelectedValue
            lbl_Brand.Text = BrandtxtStr
            lbl_Include.Text = IIf(ddlStatus.SelectedValue = "Y", "Yes", "No")

            rpbFilter.Items(0).Expanded = False
            Args.Visible = True

            '' For Simple 

            If ddlMode.SelectedValue = "Simple" Then
                Dim dt As DataTable = ObjReport.GetOfferDetails(Err_No, Err_Desc, ddlOrganization.SelectedValue, txtFromDate.SelectedDate, txtToDate.SelectedDate, BrandStr, ddlMode.SelectedValue)

                If dt.Rows.Count > 0 Then
                    If ddlStatus.SelectedValue = "N" Then
                        dt = dt.Select("Status='Active'", " OrderItemCode, FromQty ASC").CopyToDataTable()
                    End If
                End If

                gvSimpleRep.DataSource = dt
                gvSimpleRep.DataBind()

                gvSimpleRep.Visible = True

            Else  '' For Assortment
                Dim dt As DataTable = ObjReport.GeAssortmentDetails(Err_No, Err_Desc, ddlOrganization.SelectedValue, txtFromDate.SelectedDate, txtToDate.SelectedDate, BrandStr, ddlMode.SelectedValue)

                Session("AssortTbl") = dt

                '' Taking distinct Plan 

                Dim ColumnNames(5) As String
                ColumnNames = {"PlanID", "PlanName", "StartDate", "EndDate", "Status"}

                Dim planTbl As DataTable = dt.DefaultView.ToTable(True, ColumnNames)

                If planTbl.Rows.Count > 0 Then
                    gvAssort.DataSource = planTbl.Select("1=1", "PlanName ASC").CopyToDataTable()
                Else
                    gvAssort.DataSource = planTbl
                End If

                gvAssort.DataBind()

                gvAssort.Visible = True

            End If

            InitReportViewer(txtFromDate.SelectedDate, txtToDate.SelectedDate, 1)

        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            ObjReport = Nothing
        End Try
    End Sub

    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        Try
            Session("AssortTbl") = Nothing
            gvSimpleRep.Visible = False
            gvAssort.Visible = False
            Args.Visible = False
            If ValidateInputs() Then
                BindValues()
            End If
        Catch ex As Exception
            log.Error(ex.Message)
        End Try

        ''RVMain.Reset()
        ''If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then

        ''    'If ddlVan.SelectedItem.Value = "-- Select a value --" Then
        ''    '    MessageBoxValidation("Select a van")
        ''    '    SetFocus(txtFromDate)
        ''    '    Exit Sub
        ''    'End If

        ''    If Not IsDate(txtFromDate.SelectedDate) Then
        ''        MessageBoxValidation("Enter valid ""From date"".")
        ''        SetFocus(txtFromDate)
        ''        Exit Sub
        ''    End If

        ''    If Not IsDate(txtToDate.SelectedDate) Then
        ''        MessageBoxValidation("Enter valid ""To date"".")
        ''        SetFocus(txtToDate)
        ''        Exit Sub
        ''    End If

        ''    If CDate(txtFromDate.SelectedDate) > CDate(txtToDate.SelectedDate) Then
        ''        MessageBoxValidation("Start Date should not be greater than End Date.")
        ''        Exit Sub
        ''    End If


        ''    BindData()


        ''Else
        ''    MessageBoxValidation("Select an organization.")
        ''End If
    End Sub
    Sub MessageBoxValidation(ByVal str As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Sub MessageBoxValidation(ByVal str As String, Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged


        If Not (ddlOrganization.SelectedValue = "") Then

            ' ''Dim objUserAccess As UserAccess
            ' ''objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ' ''ObjCommon = New SalesWorx.BO.Common.Common

            BindBrands()
            ' ''RVMain.Reset()
        Else
            ddlBrand.ClearSelection()
            ddlBrand.Items.Clear()
            ddlBrand.Text = ""
            ' ''RVMain.Reset()
        End If

    End Sub

    Private Sub BindBrands()
        Dim ObjProd As New Product
        Try
            ddlBrand.DataSource = ObjProd.LoadBrandList(Err_No, Err_Desc, ddlOrganization.SelectedValue)
            ddlBrand.DataTextField = "Description"
            ddlBrand.DataValueField = "Code"
            ddlBrand.DataBind()

            For Each itm As RadComboBoxItem In ddlBrand.Items
                itm.Checked = True
            Next

        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            ObjProd = Nothing
        End Try
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub

    Private Sub gvSimpleRep_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvSimpleRep.PageIndexChanged
        BindValues()
    End Sub

    Private Sub gvSimpleRep_SortCommand(sender As Object, e As GridSortCommandEventArgs) Handles gvSimpleRep.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindValues()
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


            Dim BrandStr As String = ""



            For Each item As RadComboBoxItem In ddlBrand.Items
                If item.Checked Then
                    BrandStr = BrandStr & item.Value & ","
                End If
            Next

            If BrandStr = "" Then
                For Each item As RadComboBoxItem In ddlBrand.Items
                    BrandStr = BrandStr & item.Value & ","
                Next
            End If


            If BrandStr <> "" Then
                BrandStr = BrandStr.Substring(0, BrandStr.Length - 1)
            End If


            
            Dim FDate As New ReportParameter
            FDate = New ReportParameter("Dat", txtFromDate.SelectedDate)
            Dim EDate As New ReportParameter
            EDate = New ReportParameter("Dat1", txtToDate.SelectedDate)

            Dim OID As New ReportParameter
            OID = New ReportParameter("OID", CStr(ddlOrganization.SelectedValue.ToString()))

            Dim OrgName As New ReportParameter
            OrgName = New ReportParameter("OrgName", CStr(ddlOrganization.SelectedItem.Text))

            Dim Brand As New ReportParameter
            Brand = New ReportParameter("Brand", BrandStr)

            Dim Mode As New ReportParameter
            Mode = New ReportParameter("Mode", CStr(Me.ddlMode.SelectedItem.Value))

            Dim Status As New ReportParameter
            Status = New ReportParameter("Status", CStr(Me.ddlStatus.SelectedItem.Value))

            Dim BrandName As New ReportParameter
            BrandName = New ReportParameter("BrandName", CStr(IIf(ddlBrand.CheckedItems.Count > 5, "Multiple", BrandStr)))


            Dim actItem As New ReportParameter
            actItem = New ReportParameter("DeActText", ddlMode.SelectedItem.Text)



            rview.ServerReport.SetParameters(New ReportParameter() {OID, FDate, EDate, Brand, Mode, OrgName, Status, BrandName, actItem})

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
                Response.AddHeader("Content-disposition", "attachment;filename=FOCPromotion.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=FOCPromotion.xls")
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

    Private Sub gvAssort_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles gvAssort.ItemDataBound
        Try

            If TypeOf e.Item Is GridDataItem Then
                Dim item As GridDataItem = TryCast(e.Item, GridDataItem)
                If item IsNot Nothing Then

                    Dim PlanID As HiddenField = TryCast(item.Cells(2).FindControl("hfID"), HiddenField)
                    Dim OrderDIV As HtmlGenericControl = TryCast(item.Cells(2).FindControl("divOrder"), HtmlGenericControl)
                    Dim RulesDIV As HtmlGenericControl = TryCast(item.Cells(2).FindControl("divRules"), HtmlGenericControl)
                    Dim BonusDIV As HtmlGenericControl = TryCast(item.Cells(2).FindControl("divBonus"), HtmlGenericControl)

                    If Session("AssortTbl") IsNot Nothing AndAlso PlanID IsNot Nothing AndAlso OrderDIV IsNot Nothing AndAlso RulesDIV IsNot Nothing AndAlso BonusDIV IsNot Nothing Then

                        '' Populating Order
                        Dim dtAssort As DataTable = Session("AssortTbl")

                        Dim dtOrder As DataTable = dtAssort.Select("Category='Order' and PlanID=" & PlanID.Value & "").CopyToDataTable()



                        Dim queryOrder = From row In dtOrder
                          Group row By Order = New With {Key .OrderItemCode = row.Field(Of String)("OrderItemCode"),
                      Key .Brand = row.Field(Of String)("Brand"),
                      Key .OrderItemDesc = row.Field(Of String)("OrderItemDesc"),
                      Key .IsMandatory = row.Field(Of String)("IsMandatory")
                    } Into MonthGroup = Group
                       Select Order.OrderItemCode, Order.Brand, Order.OrderItemDesc, Order.IsMandatory


                        If queryOrder.Count > 0 Then
                            Dim htmlStr As String = ""
                            htmlStr &= "<table class='table' cellpadding='0' cellspacing='0' border='0'>"
                            htmlStr &= "<tr>"
                            htmlStr &= "<th colspan='4' class='text-center'>Order Items</th>"
                            htmlStr &= "</tr>"
                            htmlStr &= "<tr>"
                            htmlStr &= "<th>Brand</th>"
                            htmlStr &= "<th>Item Code</th>"
                            htmlStr &= "<th>Description</th>"
                            htmlStr &= "<th>Is Mandatory</th>"
                            htmlStr &= "</tr>"

                            For Each x In queryOrder
                                htmlStr &= "<tr>"
                                htmlStr &= String.Format("<td>{0}</td>", x.Brand)
                                htmlStr &= String.Format("<td>{0}</td>", x.OrderItemCode)
                                htmlStr &= String.Format("<td>{0}</td>", x.OrderItemDesc)
                                htmlStr &= String.Format("<td>{0}</td>", IIf(x.IsMandatory = "Y", "Yes", "No"))
                                htmlStr &= "</tr>"
                            Next

                            htmlStr &= "</table>"

                            OrderDIV.InnerHtml = htmlStr
                        End If

                        '' Populating Rule

                        Dim dtRule As DataTable = dtAssort.Select("Category='Order' and FromQty > 0 and PlanID=" & PlanID.Value & "").CopyToDataTable()

                        Dim queryRule = From row In dtRule
                      Group row By Rule = New With {Key .FromQty = row.Field(Of Decimal)("FromQty"),
                                                    Key .ToQty = row.Field(Of Decimal)("ToQty"),
                                                   Key .Type = row.Field(Of String)("BonusType"),
                                                   Key .BonusQty = row.Field(Of Decimal)("BonusQty")
                } Into MonthGroup = Group
                   Select Rule.FromQty, Rule.ToQty, Rule.Type, Rule.BonusQty



                        If queryRule.Count > 0 Then
                            Dim htmlStr As String = ""
                            htmlStr &= "<table class='table' cellpadding='0' cellspacing='0' border='0'>"
                            htmlStr &= "<tr>"
                            htmlStr &= "<th colspan='4' class='text-center'>Bonus Rules</th>"
                            htmlStr &= "</tr>"
                            htmlStr &= "<tr>"
                            htmlStr &= "<th align='center'>From Qty</th>"
                            htmlStr &= "<th align='center'>To Qty</th>"
                            htmlStr &= "<th>Type</th>"
                            htmlStr &= "<th align='center'>Bonus Qty</th>"
                            htmlStr &= "</tr>"

                            For Each x In queryRule
                                htmlStr &= "<tr>"
                                htmlStr &= String.Format("<td>{0}</td>", FormatNumber(x.FromQty, 0))
                                htmlStr &= String.Format("<td>{0}</td>", FormatNumber(x.ToQty, 0))
                                htmlStr &= String.Format("<td>{0}</td>", x.Type)
                                htmlStr &= String.Format("<td>{0}</td>", FormatNumber(x.BonusQty, 0))
                                htmlStr &= "</tr>"
                            Next

                            htmlStr &= "</table>"

                            RulesDIV.InnerHtml = htmlStr
                        End If




                        '' Populating Bonus

                        Dim dtBonus As DataTable = dtAssort.Select("Category='Bonus' and PlanID=" & PlanID.Value & "").CopyToDataTable()

                        Dim queryBonus = From row In dtBonus
                         Group row By Bonus = New With {Key .OrderItemCode = row.Field(Of String)("OrderItemCode"),
                     Key .Brand = row.Field(Of String)("Brand"),
                     Key .OrderItemDesc = row.Field(Of String)("OrderItemDesc")
                   } Into MonthGroup = Group
                      Select Bonus.OrderItemCode, Bonus.Brand, Bonus.OrderItemDesc


                        If queryBonus.Count > 0 Then
                            Dim htmlStr As String = ""
                            htmlStr &= "<table class='table' cellpadding='0' cellspacing='0' border='0'>"
                            htmlStr &= "<tr>"
                            htmlStr &= "<th colspan='3' class='text-center'>Bonus Items</th>"
                            htmlStr &= "</tr>"
                            htmlStr &= "<tr>"
                            htmlStr &= "<th>Brand</th>"
                            htmlStr &= "<th>Item Code</th>"
                            htmlStr &= "<th>Description</th>"
                            htmlStr &= "</tr>"

                            For Each x In queryBonus
                                htmlStr &= "<tr>"
                                htmlStr &= String.Format("<td>{0}</td>", x.Brand)
                                htmlStr &= String.Format("<td>{0}</td>", x.OrderItemCode)
                                htmlStr &= String.Format("<td>{0}</td>", x.OrderItemDesc)
                                htmlStr &= "</tr>"
                            Next

                            htmlStr &= "</table>"

                            BonusDIV.InnerHtml = htmlStr
                        End If


                    End If

                End If
            End If

            '' Updating Group Header Text

            If TypeOf e.Item Is GridGroupHeaderItem Then
                Dim item As GridGroupHeaderItem = CType(e.Item, GridGroupHeaderItem)
                Dim groupDataRow As DataRowView = CType(e.Item.DataItem, DataRowView)
                item.DataCell.Text = String.Format("{0}&nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp; &nbsp; &nbsp;Start Date : {1}&nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;End Date : {2} &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp; &nbsp; &nbsp;&nbsp;Status : {3} ", item.DataCell.Text, CDate(groupDataRow("StartDate")).ToString("dd-MMM-yyyy"),
                                                    CDate(groupDataRow("EndDate")).ToString("dd-MMM-yyyy"), groupDataRow("Status").ToString())
            End If

        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub gvAssort_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvAssort.PageIndexChanged
        BindValues()
    End Sub

    Private Sub ddlMode_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlMode.SelectedIndexChanged
        If ddlMode.SelectedItem.Value = "Simple" Then
            lbl_Status.Visible = True
            ddlStatus.Visible = True
            ddlStatus.Enabled = True
        Else
            lbl_Status.Visible = False
            ddlStatus.Visible = False
            ddlStatus.Enabled = False
        End If
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        ddlBrand.ClearCheckedItems()
        If Not (ddlOrganization.SelectedValue = "") Then

            ' ''Dim objUserAccess As UserAccess
            ' ''objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ' ''ObjCommon = New SalesWorx.BO.Common.Common

            BindBrands()
            ' ''RVMain.Reset()
        Else
            ddlBrand.ClearSelection()
            ddlBrand.Items.Clear()
            ddlBrand.Text = ""
            ' ''RVMain.Reset()
        End If



        ddlStatus.ClearSelection()
        txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
        txtToDate.SelectedDate = Format(Now().Date, "dd-MMM-yyyy")
        Args.Visible = False
        gvSimpleRep.Visible = False
        gvAssort.Visible = False
    End Sub
End Class