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


Public Class RepVanKPI
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As New SalesWorx.BO.Common.Common
    Dim ObjCollection As Collection
    Private ReportPath As String = "VanKPI"
    Private Const PageID As String = "P404"
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

               

                If Not Session("SCOrgID") Is Nothing And Not Session("SCVan") Is Nothing And Not Session("SCFrom") Is Nothing Then
                    If Not ddlOrganization.FindItemByValue(Session("SCOrgID")) Is Nothing Then
                        ddlOrganization.FindItemByValue(Session("SCOrgID")).Selected = True
                    End If
                    StartTime.SelectedDate = CDate(Session("SCFrom"))
                    hfSMonth.Value = StartTime.SelectedDate
                    hfVans.Value = Session("SCVan")
                    BindCombo(IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue))
                  
                    ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "clickSearch();", True)
                Else
                    If ddlOrganization.Items.Count = 2 Then
                        ddlOrganization.SelectedIndex = 1
                    End If

                    StartTime.SelectedDate = Now
                    BindCombo(IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue))
                   
                End If

                Dim dt As New DataTable
                gvRep.DataSource = dt
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

    Private Sub BindCombo(OrgStr)


        ddlVan.DataSource = objRep.GetAllOrgVan(Err_No, Err_Desc, IIf(OrgStr Is Nothing, "0", OrgStr), CType(Session("User_Access"), UserAccess).UserID)
        ddlVan.DataTextField = "SalesRep_Name"
        ddlVan.DataValueField = "SalesRep_ID"
        ddlVan.DataBind()


        For Each itm As RadComboBoxItem In ddlVan.Items
            itm.Checked = True
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

    End Sub

    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Private Sub BindReport()
        Try


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



            Args.Visible = True
            ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "PivotLoadShow();", True)

            Dim k As New DataTable
            k = objRep.GetVanKPI(Err_No, Err_Desc, Me.hfOrgID.Value, hfVans.Value, Me.hfSMonth.Value, "Summary")




            gvRep.DataSource = k
            gvRep.DataBind()

            ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "PivotLoad();", True)
            'For i As Integer = 0 To gvRep.Fields.Count - 1
            '    Dim st As String = gvRep.Fields(i).Caption
            '    gvRep.Fields(0).Caption = "No"

            '    gvRep.Fields(1).Caption = "Van KPI"
            '    gvRep.Fields(2).Caption = "Target"
            'Next


            Me.lblSelMonth.Text = Me.StartTime.SelectedDate.Value.ToString("MMM-yyyy")
        Catch ex As Exception
            log.Error(ex.ToString)
        End Try
    End Sub
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then


            BindReport()





        Else
            Args.Visible = False
            ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "PivotLoadHide();", True)


            Dim Sdate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")


            Me.hfSMonth.Value = Sdate

            Me.hfOrgID.Value = "0"
            Me.hfVans.Value = "0"
            Me.hfAgency.Value = "0"



        End If


    End Sub


    Private Sub gvRep_CellDataBound(sender As Object, e As PivotGridCellDataBoundEventArgs) Handles gvRep.CellDataBound
        If TypeOf e.Cell Is PivotGridDataCell Then
            If e.Cell.Text = "0" Then
                e.Cell.Text = Nothing
            End If

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
            ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "PivotLoadShow();", True)

            Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
            rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)



            Dim org As New ReportParameter
            org = New ReportParameter("OID", CStr(IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue)))


            Dim vans As New ReportParameter
            vans = New ReportParameter("VanList", VanStr)


            Dim sdat As New ReportParameter
            sdat = New ReportParameter("FDate", Sdate)









            Dim UID As New ReportParameter
            UID = New ReportParameter("Uid", CStr(CType(Session("User_Access"), UserAccess).UserID))

            Dim TMode As String = CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE
            Dim TargetMode As New ReportParameter
            TargetMode = New ReportParameter("TargetMode", CStr(TMode))

            rview.ServerReport.SetParameters(New ReportParameter() {org, vans, sdat, UID})

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
                Response.AddHeader("Content-disposition", "attachment;filename=VanKPI.pdf")
                Response.AddHeader("Content-Length", bytes.Length)


            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=VanKPI.xls")
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



    Protected Sub ddlOrganization_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlOrganization.SelectedIndexChanged
        BindCombo(IIf(Me.ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue))
        Args.Visible = False
        ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "PivotLoadHide();", True)
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



    End Sub
    'Protected Sub gvRep_PreRender(ByVal sender As Object, ByVal e As EventArgs)
    '    For i As Integer = 0 To gvRep.Fields.Count - 1
    '        Dim st As String = gvRep.Fields(i).Caption
    '        gvRep.Fields(0).Caption = "No"

    '        gvRep.Fields(1).Caption = "Van KPI"
    '        gvRep.Fields(2).Caption = "Target"
    '    Next

    '    gvRep.Rebind()
    'End Sub
    'Private Sub gvRep_CellDataBound(sender As Object, e As PivotGridCellDataBoundEventArgs) Handles gvRep.CellDataBound
    '    'If TypeOf e.Cell Is PivotGridColumnHeaderCell Then
    '    '    If e.Cell.Text.IndexOf("Sum of") >= 0 Then
    '    '        e.Cell.Text = e.Cell.Text.Replace("Sum of", "")
    '    '    End If

    '    'End If

    '    'If TypeOf e.Cell Is PivotGridColumnHeaderCell Then
    '    '    If e.Cell.DataItem.ToString = "Sum of LYMTD" Then


    '    '        e.Cell.Attributes.Add("style", "background-image: none")
    '    '        e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#F97166")
    '    '        e.Cell.ForeColor = System.Drawing.Color.White


    '    '    ElseIf e.Cell.DataItem.ToString = "Sum of MTD" Then


    '    '        e.Cell.Attributes.Add("style", "background-image: none")
    '    '        e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#36C5B2")
    '    '        e.Cell.ForeColor = System.Drawing.Color.White


    '    '    ElseIf e.Cell.DataItem.ToString = "Sum of Percentage" Then


    '    '        e.Cell.Attributes.Add("style", "background-image: none")
    '    '        e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#EB963C")
    '    '        e.Cell.ForeColor = System.Drawing.Color.White
    '    '    End If



    '    'End If

    '    For i As Integer = 0 To gvRep.Fields.Count - 1
    '        Dim st As String = gvRep.Fields(i).Caption
    '        gvRep.Fields(0).Caption = "No"

    '        gvRep.Fields(1).Caption = "Van KPI"
    '        gvRep.Fields(2).Caption = "Target"
    '    Next
    'End Sub


    'Protected Sub gvRep_FieldCreated(sender As Object, e As PivotGridFieldCreatedEventArgs) Handles gvRep.FieldCreated
    '    If e.Field.UniqueName = "Sequence" Then
    '        e.Field.Caption = "No"
    '    ElseIf e.Field.UniqueName = "Parameter" Then
    '        e.Field.Caption = "Van KPI"
    '    ElseIf e.Field.UniqueName = "DefaultValue" Then
    '        e.Field.Caption = "Target"
    '    End If

    'End Sub

 
End Class