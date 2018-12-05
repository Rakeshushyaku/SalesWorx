Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Partial Public Class SurveyResponses
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim objSurvey As Survey
    'Dim SortField As String = ""
    'Dim SortFieldDtl As String = ""
    Public DisplayHTML As String
    Private Const PageID As String = "P98"
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
                If Not Request.QueryString("Question_ID") Is Nothing Then
                    hdn_QuestionID.Value = Request.QueryString("Question_ID")
                    If Not Request.QueryString("typecode") Is Nothing Then
                        Dim typecode As String
                        typecode = Request.QueryString("typecode")
                        If typecode = "A" Then
                            GetVanResponses()
                        Else
                            GetCustomerResponses()
                        End If
                    End If
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
    Private Sub GetVanResponses()
        objSurvey = New Survey()
        Dim HEADER_TEMPLATE As String
        Dim CUST_RESP_TEMPLATE As String
        Dim RESP_SECTION_START_TEMPLATE As String
        Dim RESP_SECTION_END_TEMPLATE As String

        HEADER_TEMPLATE = "<table width='100%' border='0' cellspacing='1' cellpadding='4'><tr><td class='txtSM'><b>Survey: </b>$SURVEY_NAME</td></tr><tr><td class='txtSM'><b>Question: </b>$QUES_TEXT</td></tr><tr><td class='textSM'><hr width='80%'></td></tr></table>"

        RESP_SECTION_START_TEMPLATE = "<div style='width:400; height:280; overflow:scroll; display:inline'><table width='100%' border='0' cellspacing='1' cellpadding='4'>"

        RESP_SECTION_END_TEMPLATE = "</table></div>"

        CUST_RESP_TEMPLATE = "<tr><td class='txtSM'><b>Van: </b>$CUST_NAME</td></tr><tr><td class='txtSM'><b>Response: </b>$RESP_TEXT</td></tr><tr><td class='textSM' height='6px'></td></tr>"
        DisplayHTML = objSurvey.GetVanAuditResponses(hdn_QuestionID.Value, HEADER_TEMPLATE, RESP_SECTION_START_TEMPLATE, RESP_SECTION_END_TEMPLATE, CUST_RESP_TEMPLATE)
        objSurvey = Nothing
    End Sub
    Private Sub GetCustomerResponses()
        objSurvey = New Survey()
        Dim HEADER_TEMPLATE As String
        Dim CUST_RESP_TEMPLATE As String
        Dim RESP_SECTION_START_TEMPLATE As String
        Dim RESP_SECTION_END_TEMPLATE As String

        HEADER_TEMPLATE = "<table width='100%' border='0' cellspacing='1' cellpadding='4'><tr><td class='txtSM'><b>Survey: </b>$SURVEY_NAME</td></tr><tr><td class='txtSM'><b>Question: </b>$QUES_TEXT</td></tr><tr><td class='textSM'><hr width='80%'></td></tr></table>"

        RESP_SECTION_START_TEMPLATE = "<div style='width:400; height:280; overflow:scroll; display:inline'><table width='100%' border='0' cellspacing='1' cellpadding='4'>"

        RESP_SECTION_END_TEMPLATE = "</table></div>"

        CUST_RESP_TEMPLATE = "<tr><td class='tdstyle'><b>Customer: </b>$CUST_NAME</td></tr><tr><td class='txtSM'><b>Response: </b>$RESP_TEXT</td></tr><tr><td class='textSM' height='6px'></td></tr>"
        DisplayHTML = objSurvey.GetCustomerResponses(hdn_QuestionID.Value, HEADER_TEMPLATE, RESP_SECTION_START_TEMPLATE, RESP_SECTION_END_TEMPLATE, CUST_RESP_TEMPLATE)
        objSurvey = Nothing
    End Sub
End Class