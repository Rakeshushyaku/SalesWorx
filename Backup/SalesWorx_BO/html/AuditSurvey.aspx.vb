Imports System.Resources
Imports SalesWorx.BO.Common
Imports log4net

Partial Public Class AuditSurvey
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Dim ObjCustomer As Customer
    Dim objSurvey As Survey
    ' Dim SortField As String = ""
    ' Dim SortFieldDtl As String = ""
    Private Const PageID As String = "P97"
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
        Dim SearchQuery As String = ""
        objSurvey = New Survey()
        SearchQuery = " AND B.Survey_ID=" & ddlSurvey.SelectedValue
        If GetSurveyType() = "A" Then
            ddlCustomer.DataSource = objSurvey.GetSurveyAudit(Err_No, Err_Desc, SearchQuery)
            lblCustVan.Text = "Van :"
        Else
            ddlCustomer.DataSource = objSurvey.GetCustomerSurvey(Err_No, Err_Desc, SearchQuery)
            lblCustVan.Text = "Customer :"
        End If
        ddlCustomer.DataBind()
        If Not ddlCustomer.Items.Contains(New ListItem("-- Select --")) Then
            ddlCustomer.Items.Insert(0, New ListItem("-- Select --"))
        End If
        Dim dt As New DataTable
        dt = objSurvey.GetAllSurvey(Err_No, Err_Desc, "And Survey_ID=" & ddlSurvey.SelectedValue)
        Dim dr As DataRow = Nothing
        If dt.Rows.Count > 0 Then
            dr = dt.Rows(0)
            lblStartDateval.Text = String.Format("{0:dd/MM/yyyy}", dr("Start_Time"))
            lblEndDateval.Text = String.Format("{0:dd/MM/yyyy}", dr("End_Time"))
        End If
        dr = Nothing
        dt = Nothing
        objSurvey = Nothing
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


        If ddlSurvey.SelectedValue <> "-- Select a Survey --" Then
            LoadCustomer()
            lblStartDatetxt.Visible = True
            lblEndDatetxt.Visible = True
        End If
    End Sub
    Private Sub ddlCustomer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCustomer.SelectedIndexChanged
        If ddlCustomer.SelectedValue <> "-- Select --" Then
            BindData()
        End If
    End Sub
    Private Sub BindData()
        Dim SearchQuery As String = ""
        Try
            objSurvey = New Survey()
            If (ddlCustomer.SelectedItem.Value = "-- Select --" Or ddlSurvey.SelectedItem.Value = "-- Select a Survey --") Then
                SearchQuery = ""
                lblStartDatetxt.Visible = False
                lblEndDatetxt.Visible = False
            Else
                If ddlSurvey.SelectedValue <> "-- Select a Survey --" Then
                    SearchQuery = " And A.Survey_Id=" & ddlSurvey.SelectedValue
                End If
                If ddlCustomer.SelectedValue <> "-- Select --" Then
                    If GetSurveyType() = "A" Then
                        SearchQuery = SearchQuery & " And A.SalesRep_ID=" & ddlCustomer.SelectedValue
                    Else
                        SearchQuery = SearchQuery & " AND (LTRIM(STR(A.Customer_ID)) + '$' + LTRIM(STR(A.Site_Use_ID)))='" & ddlCustomer.SelectedValue & "' "
                    End If
                End If
            End If
            If Not (SearchQuery = "") Then
                Dim ds As New DataSet
                If GetSurveyType() = "A" Then
                    ds = objSurvey.GetAuditSurveyList(Err_No, Err_Desc, SearchQuery, "")
                Else
                    ds = objSurvey.GetCustomerSurveyList(Err_No, Err_Desc, SearchQuery, "")
                End If
                Dim dv As New DataView(ds.Tables("CustSurveyListTbl"))
                If SortField <> "" Then
                    dv.Sort = (SortField & " ") + SortDirection
                End If
                GVCustSurvey.DataSource = dv
                GVCustSurvey.DataBind()
                AddSortImage()
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
    Private Property SortField() As String
        Get
            If ViewState("SortColumn") Is Nothing Then
                ViewState("SortColumn") = ""
            End If
            Return ViewState("SortColumn").ToString()
        End Get
        Set(ByVal value As String)
            ViewState("SortColumn") = value
        End Set
    End Property
    Private Property SortFieldDtl() As String
        Get
            If ViewState("SortColumn1") Is Nothing Then
                ViewState("SortColumn1") = ""
            End If
            Return ViewState("SortColumn1").ToString()
        End Get
        Set(ByVal value As String)
            ViewState("SortColumn1") = value
        End Set
    End Property
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
    Public Sub AddSortImage()
        If SortField = "" Then
            Exit Sub
        End If
        Dim sortImage As New Image()
        sortImage.Style("padding-left") = "8px"
        sortImage.Style("padding-bottom") = "1px"
        If SortDirection = "ASC" Then
            sortImage.ImageUrl = "~/images/arrowUp.gif"
            sortImage.AlternateText = "Ascending Order"
        Else
            sortImage.ImageUrl = "~/images/arrowDown.gif"
            sortImage.AlternateText = "Descending Order"
        End If
        For i As Integer = 0 To GVCustSurvey.Columns.Count - 1
            Dim dcf As DataControlField = GVCustSurvey.Columns(i)
            If dcf.SortExpression = SortField Then
                GVCustSurvey.HeaderRow.Cells(i).Controls.Add(sortImage)
                Exit For
            End If
        Next
    End Sub

    Private Sub GVCustSurvey_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GVCustSurvey.PageIndexChanging
        GVCustSurvey.PageIndex = e.NewPageIndex
        BindData()
    End Sub

    Private Sub GVCustSurvey_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GVCustSurvey.Sorting
        SortField = e.SortExpression
        SortDirection = "flip"
        BindData()
    End Sub

    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        BindData()
    End Sub


End Class