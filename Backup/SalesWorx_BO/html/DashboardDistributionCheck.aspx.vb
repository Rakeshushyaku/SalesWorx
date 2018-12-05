Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Partial Public Class DashboardDistributionCheck
    Inherits System.Web.UI.Page
    Dim Err_No As Long = 0
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjDashboard As DashboardCom
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
                BindChartData()
              
                If Not Err_Desc Is Nothing Then
                    log.Error("Err No " + Err_No.ToString() + "  " + Err_Desc)
                    Response.Redirect("DashInformation.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
                End If
            Catch ex As Exception
                If Err_No = 0 Then
                    Err_No = "74966"
                End If

                If Err_Desc Is Nothing Then
                    log.Error(GetExceptionInfo(ex))
                Else
                    log.Error("Err No " + Err_No.ToString() + "  " + Err_Desc)
                End If
                Response.Redirect("DashInformation.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
            Finally
                ErrorResource = Nothing
            End Try
        End If
    End Sub
    Private Sub BindChartData()
        Try
            ObjDashboard = New DashboardCom()
            Dim yValues As Double()
            Dim xValues As String() = {"Not Visited", "Compliant", "Not Compliant"}
            Dim SearchQuery As String = ""
            Dim ds As New DataSet
            Dim dtNow As DateTime = Now.ToString("MM/dd/yyyy")
            Dim nowdayofweek As Integer = dtNow.DayOfWeek
            Dim monthStartDate, monthEndDate As DateTime
            monthStartDate = dtNow.AddDays(-(dtNow.Day - 1))
            monthEndDate = dtNow.AddMonths(1)
            monthEndDate = monthEndDate.AddDays(-(monthEndDate.Day))

            Dim userid As Integer
            userid = CType(Session("User_Access"), UserAccess).UserID
            ds = ObjDashboard.GetDistributionCheck(Err_No, Err_Desc, userid, monthStartDate, monthEndDate)

            ReDim yValues(2)
            For intCounter As Integer = 0 To ds.Tables(0).Rows.Count - 1
                yValues(0) = ds.Tables(0).Rows(intCounter).Item(0).ToString
                yValues(1) = ds.Tables(0).Rows(intCounter).Item(1).ToString
                yValues(2) = ds.Tables(0).Rows(intCounter).Item(2).ToString
            Next
            Chart1.Series("Default").Points.DataBindXY(xValues, yValues)
            ds = Nothing
        Catch ex As Exception
            If Err_No = 0 Then
                Err_No = "74066"
            End If

            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error("Err No " + Err_No.ToString() + "  " + Err_Desc)
            End If
            Response.Redirect("DashInformation.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
        Finally
            ErrorResource = Nothing
        End Try
    End Sub

End Class