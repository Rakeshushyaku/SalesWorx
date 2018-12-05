Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Partial Public Class SurveyListDetail
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCustomer As Customer
    Dim objSurvey As Survey
    'Dim SortField As String = ""
    'Dim SortFieldDtl As String = ""
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
            Try
                If Not Session.Item("USER_ACCESS") Is Nothing Then
                    If Not IsNothing(Request.QueryString("customerID")) Then
                        hdnCustID.Value = Request.QueryString("customerID").ToString()
                    End If
                    If Not IsNothing(Request.QueryString("siteID")) Then
                        hdnSiteID.Value = Request.QueryString("siteID").ToString()
                        If hdnSiteID.Value = "-1" Then
                            lblcustvan.Text = "Van :"
                        Else
                            lblcustvan.Text = "Customer Name:"
                        End If
                    End If
                    If Not IsNothing(Request.QueryString("surcustresdate")) Then
                        Dim TemToDateStr As String = Request.QueryString("surcustresdate").ToString()
                        'Dim DateArr As Array = TemToDateStr.Split("/")
                        'TemToDateStr = DateArr(1) & "/" & DateArr(0) & "/" & DateArr(2)
                        hdnsurveyResDate.Value = TemToDateStr
                    End If
                    If Not IsNothing(Request.QueryString("surid")) Then
                        hdnsurID.Value = Request.QueryString("surid").ToString()
                    End If
                    If Not IsNothing(Request.QueryString("cust")) Then
                        lblCustName.Text = Request.QueryString("cust").ToString()
                    End If
                    DList_SurveyResult_Bind()
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
    Private Sub DList_SurveyResult_Bind()
        Dim SearchQuery As String = ""
        Try
            Dim ds As New DataSet
            objSurvey = New Survey()
            If hdnCustID.Value <> "" And hdnSiteID.Value <> "" And hdnSiteID.Value <> "" Then
                If hdnSiteID.Value = "-1" Then
                    SearchQuery = SearchQuery & " And A.SalesRep_ID=" & hdnCustID.Value
                    If hdnsurID.Value = "" Then
                        SearchQuery = SearchQuery & " And A.Survey_Timestamp >= '" & hdnsurveyResDate.Value & "'"
                        SearchQuery = SearchQuery & " And A.Survey_Timestamp <= '" & hdnsurveyResDate.Value & " 23:59:59'"
                    Else
                        SearchQuery = SearchQuery & " And B.Survey_ID=" & hdnsurID.Value
                    End If
                    ds = objSurvey.GetAuditSurveyListDetail(Err_No, Err_Desc, SearchQuery, "")
                Else
                    SearchQuery = SearchQuery & " And A.Customer_ID=" & hdnCustID.Value & " AND A.Site_Use_ID=" & hdnSiteID.Value
                    If hdnsurID.Value = "" Then
                        SearchQuery = SearchQuery & " And A.Survey_Timestamp >= '" & hdnsurveyResDate.Value & "'"
                        SearchQuery = SearchQuery & " And A.Survey_Timestamp <= '" & hdnsurveyResDate.Value & " 23:59:59'"
                    Else
                        SearchQuery = SearchQuery & " And B.Survey_ID=" & hdnsurID.Value
                    End If
                    ds = objSurvey.GetCustomerSurveyListDetail(Err_No, Err_Desc, SearchQuery, "")
                End If
                DList_SurveyResult.DataSource = ds
                DList_SurveyResult.DataBind()
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
            objSurvey = Nothing
        End Try
    End Sub
    Private Sub DList_SurveyResult_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DList_SurveyResult.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim hdnQuestion As HiddenField = CType(e.Item.FindControl("hdnQuestion"), HiddenField)
            Dim lblQuestion As Label = CType(e.Item.FindControl("lblQuestion"), Label)
            Dim lblQuest As Label = CType(e.Item.FindControl("lblQuest"), Label)
            Dim Questname As String
            Questname = hdnQuestion.Value
            If e.Item.ItemIndex = 0 Then 'first row 
                Me.PrevRowQuestname.Value = Questname
            Else
                If Questname = Me.PrevRowQuestname.Value Then
                    lblQuestion.Visible = False
                    lblQuest.Visible = False
                End If
                Me.PrevRowQuestname.Value = Questname
            End If
        End If
    End Sub
End Class