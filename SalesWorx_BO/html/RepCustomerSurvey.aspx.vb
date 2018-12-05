﻿Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports System.Collections.Generic
Imports Telerik.Web.UI

Partial Public Class RepCustomerSurvey
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer
    Dim objSurvey As Survey
    Private ReportPath As String = "CustomerSurveyDetailsMain"
    Private Const PageID As String = "P208"
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
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))
                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                End If

               
                LoadSurvey()
                LoadCustomer()
                 
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

                ErrorResource = Nothing
            End Try
        End If
    End Sub
    Private Sub LoadSurvey()
        Try
            Dim SearchQuery As String = ""
            Dim SurveyType As String = ""
            objSurvey = New Survey()
            SearchQuery = " And Survey_Type_Code='N'"
            ddlSurvey.DataSource = objSurvey.GetAllSurvey(Err_No, Err_Desc, SearchQuery)
            ddlSurvey.DataBind()

            ddlSurvey.Items.Insert(0, New RadComboBoxItem("Select Survey", "0"))

             
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

     
    Private Sub LoadCustomer()
        Try
            Dim SearchQuery As String = ""
            objSurvey = New Survey()
            SearchQuery = " AND B.Survey_ID=" & ddlSurvey.SelectedValue & "  AND a.Customer_ID IN (SELECT Customer_ID FROM app_GetOrgCustomerShipAddress(" & ddlOrganization.SelectedItem.Value & "))"
            ddlCustomer.DataSource = objSurvey.GetCustomerSurvey(Err_No, Err_Desc, SearchQuery)
             
            ddlCustomer.DataBind()
            ddlCustomer.Items.Insert(0, New RadComboBoxItem("Select Customer", "0"))

            objSurvey = Nothing
        Catch ex As Exception
            log.Error(ex.Message)
            Err_No = "74167"
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
        End Try
    End Sub
     

    Private Sub ddlSurvey_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSurvey.SelectedIndexChanged
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
            Exit Sub
        End If
        RepDiv.Visible = False
        If ddlSurvey.SelectedValue <> "0" And ddlOrganization.SelectedValue <> "0" Then
            LoadCustomer()
            ' lblStartDatetxt.Visible = True
            'lblEndDatetxt.Visible = True
        Else
            ddlCustomer.Items.Clear()
            ddlCustomer.Items.Insert(0, New RadComboBoxItem("-- Select Customer--", "0"))
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

    Function BuildQuery() As String
        Dim SearchQuery As String = ""
        If ddlSurvey.SelectedValue <> "0" Then
            SearchQuery = ddlSurvey.SelectedValue
        End If
        Return SearchQuery
    End Function

    Sub Export(format As String)

        Dim FilterValue As String
        FilterValue = BuildQuery()

        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        Dim Searchvalue As New ReportParameter
        Searchvalue = New ReportParameter("SearchParams", CStr(IIf(FilterValue Is Nothing, "", FilterValue)))
        Dim CustID As New ReportParameter
        CustID = New ReportParameter("CustID", CStr(IIf(Me.ddlCustomer.SelectedIndex = 0, "ALL", ddlCustomer.SelectedValue)))

        Dim SID As New ReportParameter
        SID = New ReportParameter("SID", "ALL")
        Dim Survey As New ReportParameter
        Survey = New ReportParameter("Survey", "ALL")

        Dim QuestID As New ReportParameter
        QuestID = New ReportParameter("QuestID", "ALL")

        Dim OrgID As New ReportParameter
        OrgID = New ReportParameter("OrgID", ddlOrganization.SelectedItem.Value)

        Dim OrgName As New ReportParameter
        OrgName = New ReportParameter("OrgName", ddlOrganization.SelectedItem.Text)


        Dim SurveyID As New ReportParameter
        SurveyID = New ReportParameter("SurveyID", ddlSurvey.SelectedItem.Value)


        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)

        rview.ServerReport.SetParameters(New ReportParameter() {Searchvalue, SID, CustID, Survey, QuestID, OrgID, OrgName, SurveyID})

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
            Response.AddHeader("Content-disposition", "attachment;filename=CustomerSurvey.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=CustomerSurvey.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()

    End Sub
    Private Sub BindReport()
        If Not ddlOrganization.SelectedItem Is Nothing Then
            rpbFilter.Items(0).Expanded = False
            Dim SearchQry As String

            SearchQry = BuildQuery()
            lbl_org.Text = ddlOrganization.SelectedItem.Text
            lbl_Survey.Text = ddlSurvey.SelectedItem.Text
            If ddlCustomer.SelectedItem.Value = "0" Then
                lbl_Customer.Text = "All"
            Else
                lbl_Customer.Text = ddlCustomer.SelectedItem.Text
            End If
            Args.Visible = True

            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetCustomerSurvey(Err_No, Err_Desc, SearchQry, ddlOrganization.SelectedItem.Value)
            Details.Visible = True

            Dim NoofCust = (From Coll In dt _
                          Select CustCount = Coll.Field(Of String)("Customer") Distinct).ToList()

            If Not NoofCust Is Nothing Then
                lbl_CustSurveyed.Text = NoofCust.Count
            End If

            If ddlCustomer.SelectedItem.Value <> "0" Then

                Dim ids() As String
                ids = ddlCustomer.SelectedItem.Value.Split("$")
                Dim finaldt As New DataTable
                finaldt = dt.Select("Customer_ID=" & ids(0) & " and Site_Use_ID=" & ids(1)).CopyToDataTable
                gvRep.DataSource = finaldt
                gvRep.DataBind()


                lbl_Survey1.Text = ddlSurvey.Text
                If finaldt.Rows.Count > 0 Then
                    lbl_startDate.Text = CDate(finaldt.Rows(0)("StartDate")).ToString("dd-MMM-yyyy")
                    lbl_EndDate.Text = CDate(finaldt.Rows(0)("EndDate")).ToString("dd-MMM-yyyy")
                End If
            Else
                gvRep.DataSource = dt
                gvRep.DataBind()
                lbl_Survey1.Text = ddlSurvey.Text
                If dt.Rows.Count > 0 Then
                    lbl_startDate.Text = CDate(dt.Rows(0)("StartDate")).ToString("dd-MMM-yyyy")
                    lbl_EndDate.Text = CDate(dt.Rows(0)("EndDate")).ToString("dd-MMM-yyyy")
                End If
            End If


        End If
    End Sub

    Private Sub gvRep_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles gvRep.ItemDataBound
        If TypeOf e.Item Is GridGroupHeaderItem Then
            Dim item As GridGroupHeaderItem = CType(e.Item, GridGroupHeaderItem)
            Dim groupDataRow As DataRowView = CType(e.Item.DataItem, DataRowView)
            item.DataCell.Text = String.Format("{0} &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Surveyed At:&nbsp {1}", item.DataCell.Text,
                                                CDate(groupDataRow("Survey_Timestamp")).ToString("dd-MMM-yyyy hh:mm tt"))
        End If
    End Sub



    Private Sub dgv_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvRep.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindReport()
    End Sub

    Private Sub dgv_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged

        BindReport()
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
        Dim bretval As Boolean = False
        If ddlOrganization.SelectedIndex > 0 Then
            If ddlSurvey.SelectedIndex = 0 Then
                MessageBoxValidation("Please select survey", "Validation")
                Return bretval
            End If
            bretval = True
            Return bretval
        Else
            MessageBoxValidation("Please select the Organisation", "Validation")
            Return bretval
        End If
    End Function
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            gvRep.Visible = True
            RepDiv.Visible = True
            Details.Visible = True
            BindReport()
        Else
            Args.Visible = False
            summary.InnerHtml = ""
            gvRep.Visible = False
            RepDiv.Visible = False
            Details.Visible = False
        End If
    End Sub
    Sub MessageBoxValidation(ByVal str As String, Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub

   
    Private Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrganization.SelectedIndexChanged
        RepDiv.Visible = False
        Details.Visible = False
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddlOrganization.ClearSelection()
        If ddlOrganization.Items.Count = 2 Then
            ddlOrganization.SelectedIndex = 1
        End If
        ddlSurvey.ClearSelection()
        ddlCustomer.ClearSelection()
        ddlCustomer.Text = ""
        RepDiv.Visible = False
        Details.Visible = False
    End Sub
End Class