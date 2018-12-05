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


Public Class RepScoreCardDetails
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As New SalesWorx.BO.Common.Common
    Dim ObjCollection As Collection
    Private ReportPath As String = "ScoreCardDetails"
    Private Const PageID As String = "P398"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objRep As New Reports
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())


            Try

                ObjCommon = New SalesWorx.BO.Common.Common()
                If Not Request.QueryString("SID") Is Nothing Then
                    hfSMonth.Value = Request.QueryString("Month")
                    hfVans.Value = Request.QueryString("SID")
                    hfOrgID.Value = Request.QueryString("Org_ID")
                    lbl_Date.Text = DateTime.Parse(Request.QueryString("Month")).ToString("MMM-yyyy")
                    lbl_Sp.Text = Request.QueryString("Van")
                    lbl_org_Text.Text = Request.QueryString("OrgName")

                    Dim dtcurrency As DataTable
                    Dim ObjReport As New SalesWorx.BO.Common.Reports
                    dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, hfOrgID.Value)

                    Dim Currency As String = ""
                    If dtcurrency.Rows.Count > 0 Then
                        Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
                        Me.hfCurrency.Value = dtcurrency.Rows(0)("Currency_Code")
                        hfDecimal.Value = dtcurrency.Rows(0)("Decimal_Digits")
                        Me.lblTargetCurr.Text = Me.hfCurrency.Value
                        Me.lblSalesCurr.Text = Me.hfCurrency.Value
                        lblC1.Text = hfCurrency.Value
                    End If


                    BindData()
                End If





                Me.AgencyTab.Tabs(0).Selected = True
                Me.RadMultiPage21.SelectedIndex = 0


                CType(gvCategory.Columns(1), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"

                CType(gvCategory.Columns(2), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvCategory.Columns(3), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvCategory.Columns(4), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvCategory.Columns(5), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvCategory.Columns(6), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvCategory.Columns(7), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvCategory.Columns(8), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"


                CType(gvCategory.Columns(1), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvCategory.Columns(2), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvCategory.Columns(3), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvCategory.Columns(4), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvCategory.Columns(5), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvCategory.Columns(6), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvCategory.Columns(7), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvCategory.Columns(8), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"


                CType(gvChain.Columns(1), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvChain.Columns(2), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvChain.Columns(3), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvChain.Columns(4), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvChain.Columns(5), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvChain.Columns(6), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvChain.Columns(7), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvChain.Columns(8), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"


                CType(gvChain.Columns(1), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvChain.Columns(2), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvChain.Columns(3), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvChain.Columns(4), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvChain.Columns(5), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvChain.Columns(6), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvChain.Columns(7), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvChain.Columns(8), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"



                CType(gvOutlet.Columns(1), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvOutlet.Columns(2), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvOutlet.Columns(3), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvOutlet.Columns(4), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvOutlet.Columns(5), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvOutlet.Columns(6), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvOutlet.Columns(7), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvOutlet.Columns(8), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"


                CType(gvOutlet.Columns(1), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvOutlet.Columns(2), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvOutlet.Columns(3), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvOutlet.Columns(4), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvOutlet.Columns(5), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvOutlet.Columns(6), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvOutlet.Columns(7), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvOutlet.Columns(8), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"


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

   

    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Private Sub BindData()
       
            Dim y As DataTable
        y = objRep.GetScroeCardSummary(Err_No, Err_Desc, Me.hfOrgID.Value, hfVans.Value, Me.hfSMonth.Value, "Summary")

            Dim LabelDecimalDigits As String = "0.00"
        If Me.hfDecimal.Value = "0" Then
            LabelDecimalDigits = "0"
        ElseIf Me.hfDecimal.Value = "1" Then
            LabelDecimalDigits = "0.0"
        ElseIf Me.hfDecimal.Value = "2" Then
            LabelDecimalDigits = "0.00"
        ElseIf Me.hfDecimal.Value = "3" Then
            LabelDecimalDigits = "0.000"
        ElseIf Me.hfDecimal.Value >= "4" Then
            LabelDecimalDigits = "0.0000"
        End If
            Me.lblAchPercent.Text = "0"

            If y.Rows.Count > 0 Then

                If CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE = "Q" Then
                    Me.lblSales.Text = CDec(y.Rows(0)(1).ToString()).ToString("#,##0")
                    Me.lblTarget.Text = CDec(y.Rows(0)(0).ToString()).ToString("#,##0")
                Else
                    Me.lblSales.Text = CDec(y.Rows(0)(1).ToString()).ToString("#,##" & LabelDecimalDigits)
                    Me.lblTarget.Text = CDec(y.Rows(0)(0).ToString()).ToString("#,##" & LabelDecimalDigits)

                End If
                Me.lblTime.Text = CDec(y.Rows(0)(2).ToString()).ToString("#,##" & LabelDecimalDigits)
            Me.lblTotWorking.Text = y.Rows(0)("DaysWorking").ToString()
            Me.lblDaysOver.Text = y.Rows(0)("DaysPending").ToString()

                Me.lblAchPercent.Text = CInt(Math.Round((CDec(IIf(Me.lblSales.Text = "", "0", Me.lblSales.Text)) / CDec(IIf(CDec(y.Rows(0)(0).ToString()) <= 0, 1, CDec(y.Rows(0)(0).ToString())))) * 100.0))
                Me.lblAchPercent.Text = IIf(CInt(IIf(Me.lblAchPercent.Text = "", "0", Me.lblAchPercent.Text)) > 100, 100, IIf(CInt(IIf(Me.lblAchPercent.Text = "", "0", Me.lblAchPercent.Text)) < 0, 0, CInt(IIf(Me.lblAchPercent.Text = "", "0", Me.lblAchPercent.Text))))



            Else
                Me.lblSales.Text = "0"
                Me.lblTarget.Text = "0"

                Me.lblAchPercent.Text = "0 "
                Me.lblTotWorking.Text = "0"
                Me.lblDaysOver.Text = "0"
                Me.lblTime.Text = "0"
            End If

            Dim k As New DataTable
        k = objRep.GetScoreCardOutlet(Err_No, Err_Desc, Me.hfOrgID.Value, hfVans.Value, Me.hfSMonth.Value, "Summary")


            '' Formatting result for pivot grid 

            Dim dtFinal As New DataTable
            dtFinal.Columns.Add("Outlet")
            dtFinal.Columns.Add("Category")
            dtFinal.Columns.Add("LYMTD", Type.GetType("System.Double"))
            dtFinal.Columns.Add("MTD", Type.GetType("System.Double"))
            dtFinal.Columns.Add("Percentage", Type.GetType("System.Double"))


            Dim DtMonths As New DataTable
            DtMonths = k.DefaultView.ToTable(True, "Category", "Outlet")

            For Each seldr In DtMonths.Rows
                Dim dr As DataRow
                dr = dtFinal.NewRow
                dr("Outlet") = seldr("Outlet").ToString
                dr("Category") = seldr("Category").ToString()

                Dim vpdr() As DataRow
                vpdr = k.Select("Outlet='" & seldr("Outlet").ToString & "' and Category='" & seldr("Category") & "'")


                dr("LYMTD") = Val(vpdr(0)("LYMTD").ToString())
                dr("MTD") = Val(vpdr(0)("MTD").ToString())
                dr("Percentage") = Val(vpdr(0)("ACH").ToString())

                dtFinal.Rows.Add(dr)
            Next


            gvRep.DataSource = dtFinal
            gvRep.DataBind()






        Me.AgencyTab.Tabs(0).Selected = True
        Me.RadMultiPage21.SelectedIndex = 0
    End Sub
    Private Sub gvRep_CellDataBound(sender As Object, e As PivotGridCellDataBoundEventArgs) Handles gvRep.CellDataBound
        If TypeOf e.Cell Is PivotGridColumnHeaderCell Then
            If e.Cell.Text.IndexOf("Sum of") >= 0 Then
                e.Cell.Text = e.Cell.Text.Replace("Sum of", "")
            End If

        End If

        If TypeOf e.Cell Is PivotGridColumnHeaderCell Then
            If e.Cell.DataItem.ToString = "Sum of LYMTD" Then


                e.Cell.Attributes.Add("style", "background-image: none")
                e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#F97166")
                e.Cell.ForeColor = System.Drawing.Color.White


            ElseIf e.Cell.DataItem.ToString = "Sum of MTD" Then


                e.Cell.Attributes.Add("style", "background-image: none")
                e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#36C5B2")
                e.Cell.ForeColor = System.Drawing.Color.White


            ElseIf e.Cell.DataItem.ToString = "Sum of Percentage" Then


                e.Cell.Attributes.Add("style", "background-image: none")
                e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#EB963C")
                e.Cell.ForeColor = System.Drawing.Color.White
            End If



        End If


    End Sub

  

    Protected Function GetFilterIcon() As String
        Return SkinRegistrar.GetWebResourceUrl(Page, GetType(RadGrid), "Telerik.Web.UI.Skins.Windows7.Grid.Filter.gif")
    End Function
  
   


    Private Sub BtnPDF_Click(sender As Object, e As EventArgs) Handles BtnExportPDF.Click
        Try

            Export("PDF")

        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub
    Sub Export(format As String)
        Try
            Dim SearchQuery As String = ""





            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)




            Dim VanStr As String = hfVans.Value
            Dim vancnt As Integer = 1
            Dim vantxt As String = lbl_Sp.Text



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

                Me.lblAchPercent.Text = "0"
            End If



            Dim org As New ReportParameter
            org = New ReportParameter("OID", CStr(hfOrgID.Value))


            Dim vans As New ReportParameter
            vans = New ReportParameter("VanList", VanStr)


            Dim sdat As New ReportParameter
            sdat = New ReportParameter("FMonth", hfSMonth.Value)



            Dim SalesorgName As New ReportParameter
            SalesorgName = New ReportParameter("OrgName", CStr(Me.lbl_org_Text.Text))

            Dim VanName As New ReportParameter
            VanName = New ReportParameter("VanName", CStr(Me.lbl_Sp.Text))
            Dim Mode As New ReportParameter
            Mode = New ReportParameter("Mode", "Summary")


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
                Response.AddHeader("Content-disposition", "attachment;filename=ScoreCardDetails.pdf")
                Response.AddHeader("Content-Length", bytes.Length)


            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=ScoreCardDetails.xls")
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

            Export("Excel")

        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub

    'Protected Sub Button_Click(sender As Object, e As ImageClickEventArgs)
    '    gvVans.MasterTableView.FilterExpression = String.Empty
    '    gvVans.MasterTableView.Columns(0).CurrentFilterValue = ""
    '    gvVans.MasterTableView.Rebind()
    'End Sub

   
 
End Class