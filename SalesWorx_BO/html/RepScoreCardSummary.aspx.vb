Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Imports System.Linq
Imports System.Threading
Imports System.Globalization
Imports Telerik.Web


Public Class RepScoreCardSummary
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As New SalesWorx.BO.Common.Common
    Dim ObjCollection As Collection
    Private ReportPath As String = "ScoreCardSummary"
    Private Const PageID As String = "P398"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objRep As New Reports
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())


            Try


                ObjCommon = New SalesWorx.BO.Common.Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))

                CType(gvVans.Columns(1), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"

                CType(gvVans.Columns(2), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvVans.Columns(3), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvVans.Columns(4), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvVans.Columns(5), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvVans.Columns(6), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvVans.Columns(7), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvVans.Columns(8), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvVans.Columns(9), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"

                CType(gvVans.Columns(1), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvVans.Columns(2), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvVans.Columns(3), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvVans.Columns(4), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvVans.Columns(5), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvVans.Columns(6), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvVans.Columns(7), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvVans.Columns(8), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvVans.Columns(9), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"


                If Not Session("SCOrgID") Is Nothing And Not Session("SCVan") Is Nothing And Not Session("SCFrom") Is Nothing Then
                    If Not ddlOrganization.FindItemByValue(Session("SCOrgID")) Is Nothing Then
                        ddlOrganization.FindItemByValue(Session("SCOrgID")).Selected = True
                    End If
                    StartTime.SelectedDate = CDate(Session("SCFrom"))
                    hfSMonth.Value = StartTime.SelectedDate
                    hfVans.Value = Session("SCVan")
                    BindCombo(IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue))
                    'BindReport()

                    Me.AgencyTab.Tabs(1).Selected = True
                    Me.RadMultiPage21.SelectedIndex = 1
                    '  BindChart()
                    ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "clickSearch();", True)
                Else
                    If ddlOrganization.Items.Count = 2 Then
                        ddlOrganization.SelectedIndex = 1
                    End If

                    StartTime.SelectedDate = Now
                    BindCombo(IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue))
                    Me.AgencyTab.Tabs(0).Selected = True
                    Me.RadMultiPage21.SelectedIndex = 0
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

    Private Sub BindCombo(OrgStr)


        ddlVan.DataSource = objRep.GetAllOrgVan(Err_No, Err_Desc, IIf(OrgStr Is Nothing, "0", OrgStr), CType(Session("User_Access"), UserAccess).UserID)
        ddlVan.DataTextField = "SalesRep_Name"
        ddlVan.DataValueField = "SalesRep_ID"
        ddlVan.DataBind()

        If Not Session("SCVan") Is Nothing Then
            If Session("SCVan") = "0" Then
                For Each itm As RadComboBoxItem In ddlVan.Items
                    itm.Checked = True
                Next
            Else
                Dim vans() As String
                vans = Session("SCVan").ToString.Split(",")
                For Each id As String In vans
                    If Not ddlVan.FindItemByValue(id) Is Nothing Then
                        Dim item As RadComboBoxItem
                        item = ddlVan.FindItemByValue(id)
                        item.Checked = True
                    End If
                Next
            End If
        Else
            For Each itm As RadComboBoxItem In ddlVan.Items
                itm.Checked = True
            Next
        End If



        Dim dtcurrency As DataTable
        Dim ObjReport As New SalesWorx.BO.Common.Reports
        dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, OrgStr)

        Dim Currency As String = ""
        If dtcurrency.Rows.Count > 0 Then
            Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
            Me.hfCurrency.Value = dtcurrency.Rows(0)("Currency_Code")
            hfDecimal.Value = dtcurrency.Rows(0)("Decimal_Digits")
        End If

        If CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE = "Q" Then
            Me.lblTargetCurr.Text = "Qty"
            Me.lblSalesCurr.Text = "Qty"
            divCurrency.Visible = False
            Me.lblC.Text = "Qty"
            Me.lblC1.Text = "Qty"
        Else
            Me.lblTargetCurr.Text = Me.hfCurrency.Value
            Me.lblSalesCurr.Text = Me.hfCurrency.Value
            divCurrency.Visible = True
            Me.lblC.Text = Me.hfCurrency.Value
            Me.lblC1.Text = Me.hfCurrency.Value
        End If
        Me.lblTarget.Text = "0"
        Me.lblSales.Text = "0"
        Me.lblTeamSize.Text = "0"
    End Sub

    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Private Sub BindReport()

        Dim vanstr As String = ""
        rpbFilter.Items(0).Expanded = False



        Dim vancnt As Integer = 0
        Dim vantxt As String = ""


        For Each item As RadComboBoxItem In ddlVan.Items
            If item.Checked Then
                vancnt = vancnt + 1
                vanstr = vanstr & item.Value & ","
                vantxt = vantxt & item.Text & ","
            End If
        Next


        If vantxt <> "" Then
            vantxt = vantxt.Substring(0, vantxt.Length - 1)
        End If
        If vanstr = "" Then
            vanstr = "0"
        End If
        If vancnt = ddlVan.Items.Count Then
            lbl_van.Text = "All"
        Else
            lbl_van.Text = vantxt
        End If



        Dim Sdate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")

        Me.hfOrgID.Value = IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue)
        hfVans.Value = vanstr

        Me.hfSMonth.Value = Me.StartTime.SelectedDate.Value

        lbl_org.Text = Me.ddlOrganization.SelectedItem.Text

        Me.lbl_from.Text = Me.StartTime.SelectedDate.Value.ToString("MMM-yyyy")



        '    If vancnt > 4 Then
        '    Chartwrapper.Style.Add("height", (CInt(vancnt) * 70).ToString & "px")
        'Else
        '    Chartwrapper.Style.Add("height", "300px")
        'End If

        Args.Visible = True
        rpt.Visible = True

        Dim y As DataTable
        y = objRep.GetScroeCardSummary(Err_No, Err_Desc, Me.hfOrgID.Value, vanstr, Me.hfSMonth.Value, "Summary")

        Dim LabelDecimalDigits As String = "0.00"
        If Me.hfDecimal.Value = 0 Then
            LabelDecimalDigits = "0"
        ElseIf Me.hfDecimal.Value = 1 Then
            LabelDecimalDigits = "0.0"
        ElseIf Me.hfDecimal.Value = 2 Then
            LabelDecimalDigits = "0.00"
        ElseIf Me.hfDecimal.Value = 3 Then
            LabelDecimalDigits = "0.000"
        ElseIf Me.hfDecimal.Value >= 4 Then
            LabelDecimalDigits = "0.0000"
        End If
        Me.lblAchPercent.Text = "0"
        Me.lblTeamSize.Text = vancnt
        If y.Rows.Count > 0 Then

            If CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE = "Q" Then
                Me.lblSales.Text = CDec(y.Rows(0)(1).ToString()).ToString("#,##0")
                Me.lblTarget.Text = CDec(y.Rows(0)(0).ToString()).ToString("#,##0")
            Else
                Me.lblSales.Text = CDec(y.Rows(0)(1).ToString()).ToString("#,##" & LabelDecimalDigits)
                Me.lblTarget.Text = CDec(y.Rows(0)(0).ToString()).ToString("#,##" & LabelDecimalDigits)

            End If
            Me.lblTime.Text = CDec(y.Rows(0)(2).ToString()).ToString("#,##" & LabelDecimalDigits)
            Me.lblTotWorking.Text = y.Rows(0)(4).ToString()
            Me.lblDaysOver.Text = y.Rows(0)(3).ToString()

            Me.lblAchPercent.Text = CInt(Math.Round((CDec(IIf(Me.lblSales.Text = "", "0", Me.lblSales.Text)) / CDec(IIf(CDec(y.Rows(0)(0).ToString()) <= 0, 1, CDec(y.Rows(0)(0).ToString())))) * 100.0))
            Me.lblAchPercent.Text = IIf(CInt(IIf(Me.lblAchPercent.Text = "", "0", Me.lblAchPercent.Text)) > 100, 100, IIf(CInt(IIf(Me.lblAchPercent.Text = "", "0", Me.lblAchPercent.Text)) < 0, 0, CInt(IIf(Me.lblAchPercent.Text = "", "0", Me.lblAchPercent.Text))))



        Else
            Me.lblSales.Text = "0"
            Me.lblTarget.Text = "0"
            Me.lblTeamSize.Text = "0"
            Me.lblAchPercent.Text = "0 "
            Me.lblTotWorking.Text = "0"
            Me.lblDaysOver.Text = "0"
            Me.lblTime.Text = "0"
        End If
        Session.Remove("SCOrgID")
        Session("SCOrgID") = ddlOrganization.SelectedItem.Value
        Session.Remove("SCVan")
        Session("SCVan") = vanstr
        Session.Remove("SCFrom")
        Session("SCFrom") = hfSMonth.Value
        Me.lblSelMonth.Text = Me.StartTime.SelectedDate.Value.ToString("MMM-yyyy")

        BindChart()
    End Sub
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then


            BindReport()

            '   BindChart()
            'Dim k As New DataTable
            'k = objRep.GetScoreCardOutlet(Err_No, Err_Desc, Me.hfOrgID.Value, "30", Me.hfSMonth.Value, "Summary")


            ' '' Formatting result for pivot grid 

            'Dim dtFinal As New DataTable
            'dtFinal.Columns.Add("Outlet")
            'dtFinal.Columns.Add("Category")
            'dtFinal.Columns.Add("LYMTD", Type.GetType("System.Double"))
            'dtFinal.Columns.Add("MTD", Type.GetType("System.Double"))
            'dtFinal.Columns.Add("Percentage", Type.GetType("System.Double"))


            'Dim DtMonths As New DataTable
            'DtMonths = k.DefaultView.ToTable(True, "Category", "Outlet")

            'For Each seldr In DtMonths.Rows
            '    Dim dr As DataRow
            '    dr = dtFinal.NewRow
            '    dr("Outlet") = seldr("Outlet").ToString
            '    dr("Category") = seldr("Category").ToString()

            '    Dim vpdr() As DataRow
            '    vpdr = k.Select("Outlet='" & seldr("Outlet").ToString & "' and Category='" & seldr("Category") & "'")


            '    dr("LYMTD") = Val(vpdr(0)("LYMTD").ToString())
            '    dr("MTD") = Val(vpdr(0)("MTD").ToString())
            '    dr("Percentage") = Val(vpdr(0)("ACH").ToString())

            '    dtFinal.Rows.Add(dr)
            'Next


            'gvRep.DataSource = dtFinal
            'gvRep.DataBind()



           


        Else
            Args.Visible = False
            rpt.Visible = False

            Me.lblSales.Text = "0"
            Me.lblTarget.Text = "0"
            Me.lblTeamSize.Text = "0"
            Me.lblAchPercent.Text = "0"
            Dim Sdate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")


            Me.hfSMonth.Value = Sdate

            Me.hfOrgID.Value = "0"
            Me.hfVans.Value = "0"
            Me.hfAgency.Value = "0"



        End If
        If Not Session("SCOrgID") Is Nothing Then
            Me.AgencyTab.Tabs(1).Selected = True
            Me.RadMultiPage21.SelectedIndex = 1
        Else
            Me.AgencyTab.Tabs(0).Selected = True
            Me.RadMultiPage21.SelectedIndex = 0
        End If
       
    End Sub
    'Private Sub gvRep_CellDataBound(sender As Object, e As PivotGridCellDataBoundEventArgs) Handles gvRep.CellDataBound
    '    If TypeOf e.Cell Is PivotGridColumnHeaderCell Then
    '        If e.Cell.Text.IndexOf("Sum of") >= 0 Then
    '            e.Cell.Text = e.Cell.Text.Replace("Sum of", "")
    '        End If

    '    End If

    '    If TypeOf e.Cell Is PivotGridColumnHeaderCell Then
    '        If e.Cell.DataItem.ToString = "Sum of LYMTD" Then


    '            e.Cell.Attributes.Add("style", "background-image: none")
    '            e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#F97166")
    '            e.Cell.ForeColor = System.Drawing.Color.White


    '        ElseIf e.Cell.DataItem.ToString = "Sum of MTD" Then


    '            e.Cell.Attributes.Add("style", "background-image: none")
    '            e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#36C5B2")
    '            e.Cell.ForeColor = System.Drawing.Color.White


    '        ElseIf e.Cell.DataItem.ToString = "Sum of Percentage" Then


    '            e.Cell.Attributes.Add("style", "background-image: none")
    '            e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#EB963C")
    '            e.Cell.ForeColor = System.Drawing.Color.White
    '        End If



    '    End If


    'End Sub

    Protected Sub ViewDetails_Click(ByVal sender As Object, ByVal e As EventArgs)

        Dim btnEdit As LinkButton = TryCast(sender, LinkButton)
        Dim row As Telerik.Web.UI.GridDataItem = DirectCast(btnEdit.NamingContainer, Telerik.Web.UI.GridDataItem)
        Dim sid As String = CType(row.FindControl("HSID"), HiddenField).Value
        Response.Redirect("RepScoreCardDetails.aspx?SID=" & sid & "&Van=" & btnEdit.Text & "&OrgName=" & Me.ddlOrganization.SelectedItem.Text & "&Month=" & hfSMonth.Value & "&Org_ID=" & ddlOrganization.SelectedItem.Value)
    End Sub

    Protected Sub RadToolBar1_ButtonClick(ByVal source As Object, ByVal e As RadToolBarEventArgs)

    End Sub
    Protected Sub RadGrid1_ItemCommand(ByVal source As Object, ByVal e As GridCommandEventArgs)
        'If e.CommandName = "FilterRadGrid" Then
        '    RadFilter1.FireApplyCommand()
        'End If
    End Sub
    Protected Function GetFilterIcon() As String
        Return SkinRegistrar.GetWebResourceUrl(Page, GetType(RadGrid), "Telerik.Web.UI.Skins.Windows7.Grid.Filter.gif")
    End Function
    Private Sub BindChart()
        Dim ObjReport As New SalesWorx.BO.Common.Reports

        Dim dt1 As New DataTable
        dt1 = ObjReport.GetScroeCardSummary(Err_No, Err_Desc, hfOrgID.Value, hfVans.Value, hfSMonth.Value, "Page")

        Dim dtTargetVSSales As New DataTable
        dtTargetVSSales.Columns.Add("Van", System.Type.GetType("System.String"))
        dtTargetVSSales.Columns.Add("Target", System.Type.GetType("System.Decimal"))
        dtTargetVSSales.Columns.Add("Sales", System.Type.GetType("System.Decimal"))
        dtTargetVSSales.Columns.Add("PercentCP", System.Type.GetType("System.Decimal"))

        Dim maxpercent As Decimal = 0
        For Each sdr As DataRow In dt1.Rows

            Dim dr As DataRow
            dr = dtTargetVSSales.NewRow
            dr("Van") = sdr("Description")
            dr("Target") = Val(sdr("TotValue").ToString)
            dr("Sales") = Val(sdr("DispOrder").ToString)
            'If Val(sdr("TotValue").ToString) <> 0 Then
            '    If (Val(sdr("DispOrder").ToString) / Val(sdr("TotValue").ToString) * 100.0) > 100 Then
            '        dr("PercentCP") = 100
            '    Else

            '        dr("PercentCP") = Format((Val(sdr("DispOrder").ToString) / Val(sdr("TotValue").ToString)) * 100.0, "#,##0.00")
            '    End If


            'Else
            dr("PercentCP") = Val(sdr("LYMTD").ToString)
            ' End If
            'If maxpercent < Val(dr("PercentCP").ToString) Then
            '    maxpercent = dr("PercentCP")
            'End If
            dtTargetVSSales.Rows.Add(dr)
        Next

        If dtTargetVSSales.Rows.Count > 5 Then
            TVSChart.Width = dtTargetVSSales.Rows.Count * 50
        Else
            TVSChart.Width = 600
        End If

        TVSChart.DataSource = dtTargetVSSales
        TVSChart.DataBind()
        'If maxpercent > 103 Then
        '    TVSChart.PlotArea.AdditionalYAxes(0).MaxValue = maxpercent + 3
        'End If
      

    End Sub

    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False


        If Me.ddlOrganization.SelectedIndex <= 0 Then
            MessageBoxValidation("Please select a organization.", "Validation")
            Return bretval
        End If

        Dim VanStr As String = ""


        For Each item As RadComboBoxItem In ddlVan.Items
            If item.Checked Then
                VanStr = VanStr & "," & item.Value

            End If
        Next


        If String.IsNullOrEmpty(VanStr) Then
            MessageBoxValidation("Please select a van(s).", "Validation")
            Return bretval
        End If

        bretval = True

        Return bretval

    End Function



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
        Try
            Dim SearchQuery As String = ""




            rpbFilter.Items(0).Expanded = False
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)




            Dim VanStr As String = ""
            Dim vancnt As Integer = 0
            Dim vantxt As String = ""


            For Each item As RadComboBoxItem In ddlVan.Items
                If item.Checked Then
                    vancnt = vancnt + 1
                    VanStr = VanStr & item.Value & ","
                    vantxt = vantxt & item.Text & ","
                End If
            Next


            If vantxt <> "" Then
                vantxt = vantxt.Substring(0, vantxt.Length - 1)
            End If
            If VanStr = "" Then
                VanStr = "0"
            End If
            If vancnt = ddlVan.Items.Count Then
                lbl_van.Text = "All"
            Else
                lbl_van.Text = vantxt
            End If



            Dim Sdate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")


            lbl_org.Text = Me.ddlOrganization.SelectedItem.Text

            Me.lbl_from.Text = Me.StartTime.SelectedDate.Value.ToString("MMM-yyyy")

            Me.hfSMonth.Value = Me.StartTime.SelectedDate.Value

            Args.Visible = True
            rpt.Visible = True

            Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
            rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)



            Dim y As DataTable
            y = objRep.GetTargetvsSalesSummary(Err_No, Err_Desc, Me.hfOrgID.Value, VanStr, Me.hfSMonth.Value, "Summary")

            Dim LabelDecimalDigits As String = "0.00"
            If Me.hfDecimal.Value = 0 Then
                LabelDecimalDigits = "0"
            ElseIf Me.hfDecimal.Value = 1 Then
                LabelDecimalDigits = "0.0"
            ElseIf Me.hfDecimal.Value = 2 Then
                LabelDecimalDigits = "0.00"
            ElseIf Me.hfDecimal.Value = 3 Then
                LabelDecimalDigits = "0.000"
            ElseIf Me.hfDecimal.Value >= 4 Then
                LabelDecimalDigits = "0.0000"
            End If

            Me.lblTeamSize.Text = vancnt
            Dim achpercent As Integer = 0
            If y.Rows.Count > 0 Then
                If CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE = "Q" Then
                    Me.lblSales.Text = CDec(y.Rows(0)(1).ToString()).ToString("#,##0")
                    Me.lblTarget.Text = CDec(y.Rows(0)(0).ToString()).ToString("#,##0")
                Else
                    Me.lblSales.Text = CDec(y.Rows(0)(1).ToString()).ToString("#,##" & LabelDecimalDigits)
                    Me.lblTarget.Text = CDec(y.Rows(0)(0).ToString()).ToString("#,##" & LabelDecimalDigits)

                End If
                Me.lblAchPercent.Text = CInt(Math.Round((CDec(IIf(Me.lblSales.Text = "", "0", Me.lblSales.Text)) / CDec(IIf(CDec(y.Rows(0)(0).ToString()) <= 0, 1, CDec(y.Rows(0)(0).ToString())))) * 100.0))
                Me.lblAchPercent.Text = IIf(CInt(IIf(Me.lblAchPercent.Text = "", "0", Me.lblAchPercent.Text)) > 100, 100, IIf(CInt(IIf(Me.lblAchPercent.Text = "", "0", Me.lblAchPercent.Text)) < 0, 0, CInt(IIf(Me.lblAchPercent.Text = "", "0", Me.lblAchPercent.Text))))
                achpercent = IIf(CInt(IIf(Me.lblAchPercent.Text = "", "0", Me.lblAchPercent.Text)) > 100, 100, IIf(CInt(IIf(Me.lblAchPercent.Text = "", "0", Me.lblAchPercent.Text)) < 0, 0, CInt(IIf(Me.lblAchPercent.Text = "", "0", Me.lblAchPercent.Text))))

            Else
                Me.lblSales.Text = "0"
                Me.lblTarget.Text = "0"
                Me.lblTeamSize.Text = "0"
                Me.lblAchPercent.Text = "0"
            End If



            Dim org As New ReportParameter
            org = New ReportParameter("OID", CStr(IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue)))


            Dim vans As New ReportParameter
            vans = New ReportParameter("VanList", VanStr)


            Dim sdat As New ReportParameter
            sdat = New ReportParameter("FMonth", Sdate)



            Dim SalesorgName As New ReportParameter
            SalesorgName = New ReportParameter("OrgName", CStr(Me.lbl_org.Text))

            Dim VanName As New ReportParameter
            VanName = New ReportParameter("VanName", CStr(Me.lbl_van.Text))
            Dim Mode As New ReportParameter
            Mode = New ReportParameter("Mode", "SSRS")


            Dim Currency As New ReportParameter
            Currency = New ReportParameter("Currency", CStr(Me.hfCurrency.Value))

            Dim DecimalDigits As New ReportParameter
            DecimalDigits = New ReportParameter("DecimalDigits", CStr(Me.hfDecimal.Value))

            Dim TotTarget As New ReportParameter
            TotTarget = New ReportParameter("TotTarget", CStr(lblTarget.Text))
            Dim TotSales As New ReportParameter
            TotSales = New ReportParameter("TotSales", CStr(lblSales.Text))



         

            Dim AchvPercent As New ReportParameter
            AchvPercent = New ReportParameter("AchvPercent", CStr(achpercent))

            Dim TMode As String = CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE
            Dim TargetMode As New ReportParameter
            TargetMode = New ReportParameter("TargetMode", CStr(TMode))

            rview.ServerReport.SetParameters(New ReportParameter() {org, vans, sdat, SalesorgName, VanName, Mode, Currency, DecimalDigits, TotTarget, TotSales, AchvPercent, TargetMode})

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
                Response.AddHeader("Content-disposition", "attachment;filename=ScoreCardSummaryByVan.pdf")
                Response.AddHeader("Content-Length", bytes.Length)


            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=ScoreCardSummaryByVan.xls")
                Response.AddHeader("Content-Length", bytes.Length)
            End If
            Response.OutputStream.Write(bytes, 0, bytes.Length)
            Response.OutputStream.Flush()
            Response.OutputStream.Close()
            Response.Flush()
            Response.Close()
        Catch ex As Exception
            log.Debug(ex.ToString())
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

    Protected Sub Button_Click(sender As Object, e As ImageClickEventArgs)
        gvVans.MasterTableView.FilterExpression = String.Empty
        gvVans.MasterTableView.Columns(0).CurrentFilterValue = ""
        gvVans.MasterTableView.Rebind()
    End Sub

    Protected Sub ddlOrganization_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlOrganization.SelectedIndexChanged
        BindCombo(IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue))
        Args.Visible = False
        rpt.Visible = False
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        ddlVan.ClearCheckedItems()
        ddlVan.Items.Clear()
        StartTime.SelectedDate = Now

        BindCombo(IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue))
        Args.Visible = False
        rpt.Visible = False
        Session.Remove("SCOrgID")
        Session.Remove("SCVan")
        Session.Remove("SCFrom")

    End Sub

    'Protected Sub AgencyTab_TabClick(sender As Object, e As RadTabStripEventArgs) Handles AgencyTab.TabClick

    '    If AgencyTab.Tabs(0).Selected = True And Not Session("SCOrgID") Is Nothing Then
    '        BindChart()
    '    End If
    'End Sub
End Class