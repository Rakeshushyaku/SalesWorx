Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms

Partial Public Class RepAuditSurvey
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Dim ObjCustomer As Customer
    Dim objSurvey As Survey
    Private ReportPath As String = "AuditSurvey"
    Private Const PageID As String = "P209"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not IsNothing(Me.Master) Then

            Dim masterScriptManager As ScriptManager
            masterScriptManager = CType(Master.FindControl("ScriptManager1"), ScriptManager)

            ' Make sure our master page has the script manager we're looking for
            If Not IsNothing(masterScriptManager) Then

                ' Turn off partial page postbacks for this page
                masterScriptManager.EnablePartialRendering = False
            End If

        End If

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
                ObjCommon = New Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New ListItem("-- Select a value --"))

                Dim cls As New Purpose
                'ddlTypeCode.DataSource = cls.BindSurveyTypeCode
                'ddlTypeCode.DataTextField = "Key"
                'ddlTypeCode.DataValueField = "Value"
                'ddlTypeCode.DataBind()
                cls = Nothing
                LoadSurvey()
                If Not ddlSurvey.Items.Contains(New ListItem("-- Select a Survey --")) Then
                    ddlSurvey.Items.Insert(0, New ListItem("-- Select a Survey --"))
                End If
                If Not ddlCustomer.Items.Contains(New ListItem("-- Select --")) Then
                    ddlCustomer.Items.Insert(0, New ListItem("-- Select --"))
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

                ErrorResource = Nothing
            End Try
        End If
    End Sub
    Private Sub LoadSurvey()
        Try
            Dim SearchQuery As String = ""
            Dim SurveyType As String = ""
            objSurvey = New Survey()
            SearchQuery = " And Survey_Type_Code='A'"
            ddlSurvey.DataSource = objSurvey.GetAllSurvey(Err_No, Err_Desc, SearchQuery)
            ddlSurvey.DataBind()
            If Not ddlSurvey.Items.Contains(New ListItem("-- Select a Survey --")) Then
                ddlSurvey.Items.Insert(0, New ListItem("-- Select a Survey --"))
            End If
            lblStartDateval.Text = ""
            lblEndDateval.Text = ""


            objSurvey = Nothing

        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Function GetSurveyType()
        Dim SurveyType As String = ""
        If ddlSurvey.SelectedIndex > 0 Then


            Dim dt As New DataTable
            objSurvey = New Survey()
            dt = objSurvey.GetAllSurvey(Err_No, Err_Desc, " And Survey_ID=" & ddlSurvey.SelectedValue)

            Dim dr As DataRow = Nothing
            If dt.Rows.Count > 0 Then
                dr = dt.Rows(0)
                SurveyType = dr("Survey_Type_Code").ToString()
            End If
        End If
        Return SurveyType
    End Function
    Private Sub LoadCustomer()
        Try
            Dim SearchQuery As String = ""
            objSurvey = New Survey()
            SearchQuery = " AND B.Survey_ID=" & ddlSurvey.SelectedValue & " AND A.SalesRep_ID IN (select SalesRep_ID from TBL_Org_CTL_DTL where MAS_Org_ID ='" & ddlOrganization.SelectedItem.Value & "') "
            'If GetSurveyType() = "A" Then
            ddlCustomer.DataSource = objSurvey.GetSurveyAudit(Err_No, Err_Desc, SearchQuery)
            ' lblCustVan.Text = "Van :"
            'Else
            '    ddlCustomer.DataSource = objSurvey.GetCustomerSurvey(Err_No, Err_Desc, SearchQuery)
            '    lblCustVan.Text = "Customer :"
            'End If
            ddlCustomer.DataBind()
            If Not ddlCustomer.Items.Contains(New ListItem("-- Select --")) Then
                ddlCustomer.Items.Insert(0, New ListItem("-- Select --"))
            End If
            'Dim dt As New DataTable
            'dt = objSurvey.GetAllSurvey(Err_No, Err_Desc, "And Survey_ID=" & ddlSurvey.SelectedValue)
            'Dim dr As DataRow = Nothing
            'If dt.Rows.Count > 0 Then
            '    dr = dt.Rows(0)
            '    lblStartDateval.Text = String.Format("{0:dd/MM/yyyy}", dr("Start_Time"))
            '    lblEndDateval.Text = String.Format("{0:dd/MM/yyyy}", dr("End_Time"))
            'End If
            'dr = Nothing
            'dt = Nothing
            objSurvey = Nothing

        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
    'Private Sub ddlTypeCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTypeCode.SelectedIndexChanged
    '    LoadSurvey()
    '    GVCustSurvey.DataBind()
    '    ddlCustomer.Items.Clear()
    '    If Not ddlCustomer.Items.Contains(New ListItem("-- Select --")) Then
    '        ddlCustomer.Items.Insert(0, New ListItem("-- Select --"))
    '    End If
    'End Sub

    Private Sub ddlSurvey_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSurvey.SelectedIndexChanged
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
            Exit Sub
        End If

        Try
            If ddlSurvey.SelectedValue <> "-- Select a Survey --" And ddlOrganization.SelectedValue <> "-- Select a value --" Then
                LoadCustomer()
                'lblStartDatetxt.Visible = True
                'lblEndDatetxt.Visible = True
            Else
                ddlCustomer.Items.Clear()
                ddlCustomer.Items.Insert(0, New ListItem("-- Select --"))
            End If
            RVMain.Reset()
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
    Private Sub ddlCustomer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCustomer.SelectedIndexChanged
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
            Exit Sub
        End If
        If ddlCustomer.SelectedValue <> "-- Select --" Then
            BindData()
        End If
    End Sub
    Private Sub BindData()
        Dim SearchQuery As String = ""
        Try

            If ddlOrganization.SelectedIndex = 0 Then
                MessageBoxValidation("Please select an organization")
                Exit Sub
            End If

            If ddlSurvey.SelectedIndex = 0 Then
                MessageBoxValidation("Please select survey")
                Exit Sub
            End If


            objSurvey = New Survey()
            'If (ddlCustomer.SelectedItem.Value = "-- Select --" Or ddlSurvey.SelectedItem.Value = "-- Select a Survey --") Then
            '    SearchQuery = ""
            '    lblStartDatetxt.Visible = False
            '    lblEndDatetxt.Visible = False
            'Else
            If ddlSurvey.SelectedValue <> "-- Select a Survey --" Then
                SearchQuery = ddlSurvey.SelectedValue
            End If
            'If ddlCustomer.SelectedValue <> "-- Select --" Then
            '    If GetSurveyType() = "A" Then
            '        SearchQuery = SearchQuery & " And A.SalesRep_ID=" & ddlCustomer.SelectedValue
            '    Else
            '        SearchQuery = SearchQuery & " AND (LTRIM(STR(A.Customer_ID)) + '$' + LTRIM(STR(A.Site_Use_ID)))='" & ddlCustomer.SelectedValue & "' "
            '    End If
            'End If
            'End If
            If Not (SearchQuery = "") Then
                'Dim ds As New DataSet
                'If GetSurveyType() = "A" Then
                '    ds = objSurvey.GetAuditSurveyList(Err_No, Err_Desc, SearchQuery, "")
                'Else
                '    ds = objSurvey.GetCustomerSurveyList(Err_No, Err_Desc, SearchQuery, "")
                'End If
                'Dim dv As New DataView(ds.Tables("CustSurveyListTbl"))
                'If SortField <> "" Then
                '    dv.Sort = (SortField & " ") + SortDirection
                'End If
                'GVCustSurvey.DataSource = dv
                'GVCustSurvey.DataBind()
                'AddSortImage()
                InitReportViewer(SearchQuery)
            Else
                MessageBoxValidation("Please select a survey")
            End If
        Catch ex As Exception
            If Err_Desc IsNot Nothing Then
                log.Error(Err_Desc)
            Else
                log.Error(GetExceptionInfo(ex))
            End If
            Err_No = "74067"
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
        Finally
            objSurvey = Nothing
        End Try
    End Sub
    'Private Property SortField() As String
    '    Get
    '        If ViewState("SortColumn") Is Nothing Then
    '            ViewState("SortColumn") = ""
    '        End If
    '        Return ViewState("SortColumn").ToString()
    '    End Get
    '    Set(ByVal value As String)
    '        ViewState("SortColumn") = value
    '    End Set
    'End Property
    'Private Property SortFieldDtl() As String
    '    Get
    '        If ViewState("SortColumn1") Is Nothing Then
    '            ViewState("SortColumn1") = ""
    '        End If
    '        Return ViewState("SortColumn1").ToString()
    '    End Get
    '    Set(ByVal value As String)
    '        ViewState("SortColumn1") = value
    '    End Set
    'End Property
    'Private Property SortDirection() As String
    '    Get
    '        If ViewState("SortDirection") Is Nothing Then
    '            ViewState("SortDirection") = "ASC"
    '        End If
    '        Return ViewState("SortDirection").ToString()
    '    End Get
    '    Set(ByVal value As String)
    '        Dim s As String = SortDirection

    '        If value = "flip" Then
    '            s = If(s = "ASC", "DESC", "ASC")
    '        Else
    '            s = value
    '        End If

    '        ViewState("SortDirection") = s
    '    End Set
    'End Property
    'Public Sub AddSortImage()
    '    If SortField = "" Then
    '        Exit Sub
    '    End If
    '    Dim sortImage As New Image()
    '    sortImage.Style("padding-left") = "8px"
    '    sortImage.Style("padding-bottom") = "1px"
    '    If SortDirection = "ASC" Then
    '        sortImage.ImageUrl = "~/images/arrowUp.gif"
    '        sortImage.AlternateText = "Ascending Order"
    '    Else
    '        sortImage.ImageUrl = "~/images/arrowDown.gif"
    '        sortImage.AlternateText = "Descending Order"
    '    End If
    '    For i As Integer = 0 To GVCustSurvey.Columns.Count - 1
    '        Dim dcf As DataControlField = GVCustSurvey.Columns(i)
    '        If dcf.SortExpression = SortField Then
    '            GVCustSurvey.HeaderRow.Cells(i).Controls.Add(sortImage)
    '            Exit For
    '        End If
    '    Next
    'End Sub

    'Private Sub GVCustSurvey_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GVCustSurvey.PageIndexChanging
    '    GVCustSurvey.PageIndex = e.NewPageIndex
    '    BindData()
    'End Sub

    'Private Sub GVCustSurvey_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GVCustSurvey.Sorting
    '    SortField = e.SortExpression
    '    SortDirection = "flip"
    '    BindData()
    'End Sub

    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
            Exit Sub
        End If
        RVMain.Reset()
        BindData()
    End Sub
    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        MpInfoError.Show()
        Exit Sub
    End Sub

    Private Sub InitReportViewer(ByVal FilterValue As String)
        Try

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim Searchvalue As New ReportParameter
            Searchvalue = New ReportParameter("SearchParams", CStr(IIf(FilterValue Is Nothing, "", FilterValue)))
            Dim SID As New ReportParameter
            SID = New ReportParameter("SID", CStr(IIf(Me.ddlCustomer.SelectedIndex = 0, "ALL", ddlCustomer.SelectedValue)))

            Dim OrgID As New ReportParameter
            OrgID = New ReportParameter("OrgID", ddlOrganization.SelectedItem.Value)

            Dim OrgName As New ReportParameter
            OrgName = New ReportParameter("OrgName", ddlOrganization.SelectedItem.Text)

            Dim SurveyName As New ReportParameter
            SurveyName = New ReportParameter("SurveyName", ddlSurvey.SelectedItem.Text)


            With RVMain
                .Reset()
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                .ServerReport.SetParameters(New ReportParameter() {Searchvalue, SID, OrgName, OrgID, SurveyName})
                '.ServerReport.Refresh()

            End With


        Catch Ex As Exception
            '  log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub

    Private Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrganization.SelectedIndexChanged
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
            Exit Sub
        End If
        ddlSurvey.SelectedIndex = 0
        ddlCustomer.SelectedIndex = 0
        RVMain.Reset()
    End Sub
End Class