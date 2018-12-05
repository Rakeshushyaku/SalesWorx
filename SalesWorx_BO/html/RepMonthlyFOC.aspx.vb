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


Public Class RepMonthlyFOC
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As New SalesWorx.BO.Common.Common
    Dim ObjCollection As Collection
    Private ReportPath As String = "MonthlyFOCByVan"
    Private Const PageID As String = "P362"
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


                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                End If

                StartTime.SelectedDate = Now



                BindCombo(IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue))
              


                CType(gvVans.Columns(4), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"

                CType(gvVans.Columns(3), GridBoundColumn).DataFormatString = "{0:N0}"
                CType(gvVans.Columns(5), GridBoundColumn).DataFormatString = "{0:N0}"

                CType(gvVans.Columns(4), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
                CType(gvVans.Columns(3), GridBoundColumn).FooterAggregateFormatString = "{0:N0}"

                CType(gvVans.Columns(5), GridBoundColumn).FooterAggregateFormatString = "{0:N0}"


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

        For Each item As RadComboBoxItem In ddlVan.Items
            item.Checked = True
        Next
        Dim dtcurrency As DataTable
        Dim ObjReport As New SalesWorx.BO.Common.Reports
        dtcurrency = ObjReport.GetCurrency(Err_No, Err_Desc, OrgStr)

        Dim Currency As String = ""
        If dtcurrency.Rows.Count > 0 Then
            Currency = " (" & dtcurrency.Rows(0)("Currency_Code") & ")"
            Me.hfCurrency.Value = dtcurrency.Rows(0)("Currency_Code")
            hfDecimal.Value = dtcurrency.Rows(0)("Decimal_Digits")
        End If
        Me.lblC.Text = Me.hfCurrency.Value
        Me.lblTargetCurr.Text = Me.hfCurrency.Value
        Me.lblSalesCurr.Text = Me.hfCurrency.Value
        Me.lblTarget.Text = "0"
        Me.lblSales.Text = "0"
    End Sub

    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then



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



            Dim Sdate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM") + "-01-" + DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("yyyy")


            Me.hfOrgID.Value = IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue)
            hfVans.Value = vanstr

            Me.hfSMonth.Value = Sdate

            lbl_org.Text = Me.ddlOrganization.SelectedItem.Text

            Me.lbl_from.Text = Me.StartTime.SelectedDate.Value.ToString("MMM-yyyy")



            '    If vancnt > 4 Then
            '    Chartwrapper.Style.Add("height", (CInt(vancnt) * 70).ToString & "px")
            'Else
            '    Chartwrapper.Style.Add("height", "300px")
            'End If

            Args.Visible = True
            rpt.Visible = True
            Me.lblSales.Text = "0"
            Me.lblTarget.Text = "0"
            Dim y As DataTable
            y = objRep.GetMonthlyFOCByVanSummary(Err_No, Err_Desc, Me.hfOrgID.Value, vanstr, Me.hfSMonth.Value)

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


            CType(gvVans.Columns(4), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"

            CType(gvVans.Columns(3), GridBoundColumn).DataFormatString = "{0:N0}"
            CType(gvVans.Columns(5), GridBoundColumn).DataFormatString = "{0:N0}"

            CType(gvVans.Columns(4), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
            CType(gvVans.Columns(3), GridBoundColumn).FooterAggregateFormatString = "{0:N0}"

            CType(gvVans.Columns(5), GridBoundColumn).FooterAggregateFormatString = "{0:N0}"
            If y.Rows.Count > 0 Then

                '  Me.lblSales.Text = CDec(y.Rows(0)(2).ToString()).ToString("#,##" & "0")
                ' Me.lblTarget.Text = CDec(y.Rows(0)(1).ToString()).ToString("#,##" & LabelDecimalDigits)
                For Each r As DataRow In y.Rows



                    Me.lblSales.Text = CDec(Me.lblSales.Text) + CDec(r(2).ToString())


                    Me.lblTarget.Text = CDec(Me.lblTarget.Text) + CDec(r(1).ToString())



                Next

            Else
                Me.lblSales.Text = "0"
                Me.lblTarget.Text = "0"
            
            End If
            Me.lblTarget.Text = CDec(Me.lblTarget.Text).ToString("#,##" & LabelDecimalDigits)
            Me.lblSales.Text = CDec(Me.lblSales.Text).ToString("#,##" & "0")
            'gvVans.Columns(1).HeaderText = "Volume"
            'gvVans.Columns(2).HeaderText = "Value(" & Me.lblC.Text & ")"
            Me.lblSelMonth.Text = Me.StartTime.SelectedDate.Value.ToString("MMM-yyyy")



        Else
            Args.Visible = False
            rpt.Visible = False

            Me.lblSales.Text = "0"
            Me.lblTarget.Text = "0"
         
            Dim Sdate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM") + "-01-" + DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("yyyy")



            Me.hfSMonth.Value = Sdate

            Me.hfOrgID.Value = "0"
            Me.hfVans.Value = "0"
            Me.hfAgency.Value = "0"




        End If
      
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



        Return True

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



            Dim Sdate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM") + "-01-" + DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("yyyy")


            lbl_org.Text = Me.ddlOrganization.SelectedItem.Text

            Me.lbl_from.Text = Me.StartTime.SelectedDate.Value.ToString("MMM-yyyy")

            Me.hfSMonth.Value = Sdate

            Args.Visible = True
            rpt.Visible = True

            Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
            rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)



            Dim y As DataTable
            y = objRep.GetMonthlyFOCByVanSummary(Err_No, Err_Desc, Me.hfOrgID.Value, VanStr, Me.hfSMonth.Value)




            Dim achpercent As Integer = 0
            'If y.Rows.Count > 0 Then
            '    Me.lblSales.Text = CDec(y.Rows(0)(2).ToString())
            '    Me.lblTarget.Text = CDec(y.Rows(0)(1).ToString())

            'Else
            '    Me.lblSales.Text = "0"
            '    Me.lblTarget.Text = "0"
            'End If
            Me.lblSales.Text = "0"
            Me.lblTarget.Text = "0"
            If y.Rows.Count > 0 Then

                For Each r As DataRow In y.Rows



                    Me.lblSales.Text = CDec(Me.lblSales.Text) + CDec(r(2).ToString())


                    Me.lblTarget.Text = CDec(Me.lblTarget.Text) + CDec(r(1).ToString())



                Next

            Else
                Me.lblSales.Text = "0"
                Me.lblTarget.Text = "0"

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


            Dim Currency As New ReportParameter
            Currency = New ReportParameter("Currency", CStr(Me.hfCurrency.Value))

            Dim DecimalDigits As New ReportParameter
            DecimalDigits = New ReportParameter("DecimalDigits", CStr(Me.hfDecimal.Value))

            Dim TotTarget As New ReportParameter
            TotTarget = New ReportParameter("TotValue", CStr(lblTarget.Text))
            Dim TotSales As New ReportParameter
            TotSales = New ReportParameter("TotVolume", CStr(lblSales.Text))





            rview.ServerReport.SetParameters(New ReportParameter() {org, vans, sdat, SalesorgName, VanName, Currency, DecimalDigits, TotTarget, TotSales})

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
                Response.AddHeader("Content-disposition", "attachment;filename=MonthlyFOCReport.pdf")
                Response.AddHeader("Content-Length", bytes.Length)


            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=MonthlyFOCReport.xls")
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
    End Sub
End Class