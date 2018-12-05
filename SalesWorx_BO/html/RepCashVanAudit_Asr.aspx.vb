Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Partial Public Class RepCashVanAudit_Asr
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Private ReportPath As String = "CashVanAudit_ASR"
    Private Const PageID As String = "P210"
    Public DivHTML As String = "No audit information found."
    ' Private Const PageID As String = "P97"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

   

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
            Exit Sub
        End If
        If Not IsPostBack Then
            Dim HasPermission As Boolean = False
                        Try
                ObjCommon = New SalesWorx.BO.Common.Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)

                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))
                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                End If
                LoadOrgDetails()
               
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
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If ddlOrganization.SelectedIndex <= 0 Then
            MessageBoxValidation("Please select an organization", "Validation")
            Return bretval
        End If
        If ddlVan.SelectedIndex <= 0 Then
            MessageBoxValidation("Please select a van/FSR", "Validation")
            Return bretval
        End If
        bretval = True
        Return bretval
    End Function
    Protected Sub SearchBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            gvRep.Visible = True
            BindReport()
        Else
            lbl_noAudit.Visible = False
            Details.Visible = False
            Args.Visible = False
            gvRep.Visible = False
        End If

    End Sub
    Private Sub BtnExport_Click(sender As Object, e As EventArgs) Handles BtnExportExcel.Click
        Try
            If ValidateInputs() Then
                Export("Excel")
            End If
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub
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


        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

        Dim SalesRepID As New ReportParameter
        SalesRepID = New ReportParameter("SalesRepID", ddlVan.SelectedItem.Value)

        'Dim VanID As New ReportParameter
        'VanID = New ReportParameter("VanID", ddlVan.SelectedItem.Value)


        'Dim Van As New ReportParameter
        'Van = New ReportParameter("Van", ddlVan.SelectedItem.Text)

        rview.ServerReport.SetParameters(New ReportParameter() {SalesRepID})

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
            Response.AddHeader("Content-disposition", "attachment;filename=CashVanAudit.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=CashVanAudit.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()
    End Sub

    Sub BindReport()
        lbl_org.Text = ddlOrganization.SelectedItem.Text
        lbl_van.Text = ddlVan.SelectedItem.Text
        Args.Visible = True
        rpbFilter.Items(0).Expanded = False
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        Dim ObjReport As New SalesWorx.BO.Common.Reports



        Dim dt As New DataTable
        dt = ObjReport.GetCashVanAudit_Asr(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, ddlVan.SelectedItem.Value)
        Dim finaldt As New DataTable

        finaldt.Columns.Add("Description")
        If dt.Rows.Count > 0 Then
            lbl_noAudit.Visible = False
            lbl_EmpName.Text = dt(0)("Emp_Name").ToString
            lbl_Code.Text = dt(0)("Emp_Code").ToString
            lbl_survey.Text = dt(0)("Survey_Title").ToString
            If Not dt(0)("Survey_Timestamp") Is DBNull.Value Then
                lbl_DateofAudit.Text = CDate(dt(0)("Survey_Timestamp")).ToString("dd-MMM-yyyy")
            Else
                lbl_DateofAudit.Text = ""
            End If
            If Not dt(0)("Previous_Audit") Is DBNull.Value Then
                lbl_prev.Text = CDate(dt(0)("Previous_Audit")).ToString("dd-MMM-yyyy")
            Else
                lbl_prev.Text = "N/A"
            End If
           

            For Each dr As DataRow In dt.Rows
                Dim newdr As DataRow
                newdr = finaldt.NewRow

                newdr(0) = dr("Question_text").ToString
                finaldt.Rows.Add(newdr)

                Dim newdr1 As DataRow
                newdr1 = finaldt.NewRow
                If dr("Response_Type_ID") = "1" Then
                    newdr1(0) = dr("Response")
                Else
                    newdr1(0) = dr("Restext") 'dr("Response_Text")
                End If

                finaldt.Rows.Add(newdr1)
            Next
            Details.Visible = True
        Else
            lbl_noAudit.Visible = True
            Details.Visible = False
        End If

        gvRep.DataSource = finaldt
        gvRep.DataBind()
    End Sub
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
            Exit Sub
        End If
        Try
            LoadOrgDetails()
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
    Sub LoadOrgDetails()
        If Not (ddlOrganization.SelectedItem.Value = "0") Then

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New SalesWorx.BO.Common.Common()
            ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
            ddlVan.DataBind()
            ddlVan.Items.Insert(0, New RadComboBoxItem("Select Van/FSR", "0"))


        Else
            ddlVan.Items.Clear()
            ddlVan.Items.Insert(0, New RadComboBoxItem("Select Van/FSR", "0"))

        End If
    End Sub


    Protected Sub ddlDivision_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        LoadDetails()
    End Sub


    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub


    Sub LoadDetails()
        ObjCommon = New SalesWorx.BO.Common.Common()
        Dim SubQry As String = "A.SalesRep_ID='" + ddlVan.SelectedValue + "'"
        
    End Sub

   

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        ddlVan.ClearSelection()
        LoadOrgDetails()
        lbl_noAudit.Visible = False
        Details.Visible = False
        Args.Visible = False
        gvRep.Visible = False
    End Sub
End Class