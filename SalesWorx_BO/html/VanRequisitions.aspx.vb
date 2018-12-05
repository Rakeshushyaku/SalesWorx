Imports SalesWorx.BO.Common
Imports log4net

Partial Public Class VanRequisitions
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    ' Dim SortField As String = ""
    Dim objVanReq As New VanRequisition
    Private Const ModuleName As String = "VanRequisitions.aspx"
    Private Const PageID As String = "P43"
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
  Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Session.Item("USER_ACCESS") Is Nothing Then
                If Not HasAuthentication() Then
                    Err_No = 500
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
                End If
                ViewState("Criteria") = "Status='N'"
                LoadDetails("Status='N'")
            End If
        End If
    End Sub

    Private Function HasAuthentication() As Boolean
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
    End Function
    Sub LoadDetails(ByVal Criteria As String)

        Try
            Dim dt As New DataTable
            dt = objVanReq.GetOutstandingStockRequistion(Criteria)
            Dim dv As New DataView(dt)
            If SortField <> "" Then
                dv.Sort = (SortField & " ") + SortDirection
            End If
            gvVanReq.DataSource = dv
            gvVanReq.DataBind()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        Dim Criteria As String = "Status='N'"
        If ddFilterBy.SelectedValue <> "0" Then
            Criteria += " and " + ddFilterBy.SelectedValue + " LIKE '%" + txtSearch.Text.Trim() + "%'"
        End If
        If txtFrom.Text.Trim() <> "" Then
            If IsDate(txtFrom.Text.Trim()) Then
                Criteria += " and Request_Date >= '" + txtFrom.Text.Trim() + "'"
            Else
                MessageBoxValidation("Enter valid 'From Date'")
                txtTo.Focus()
                Return
            End If
        End If
        If txtTo.Text.Trim() <> "" Then
            If IsDate(txtTo.Text.Trim()) Then
                Criteria += " and Request_Date <= '" + txtTo.Text.Trim() + "'"
            Else
                MessageBoxValidation("Enter valid 'To Date'")
                txtFrom.Focus()
                Return
            End If
        End If
        If txtFrom.Text.Trim() <> "" And txtTo.Text.Trim() <> "" Then
            If Not IsDate(txtFrom.Text.Trim()) Then
                MessageBoxValidation("Enter valid 'From Date'")
                Return
            End If
            If Not IsDate(txtTo.Text.Trim()) Then
                MessageBoxValidation("Enter valid 'To Date'")
                Return
            End If

            If CDate(txtFrom.Text.Trim()) > CDate(txtTo.Text.Trim()) Then
                MessageBoxValidation("'From date' should be less than or equal to 'To Date'")
            End If
        End If
        ViewState("Criteria") = Criteria
        LoadDetails(Criteria)
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

    Protected Sub gvVanReq_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvVanReq.Sorting
        SortField = e.SortExpression
        SortDirection = "flip"
        LoadDetails(ViewState("Criteria").ToString())
    End Sub

    Protected Sub gvVanReq_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvVanReq.PageIndexChanging
        gvVanReq.PageIndex = e.NewPageIndex
        LoadDetails(ViewState("Criteria").ToString())
    End Sub
    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        MpInfoError.Show()
        Exit Sub
    End Sub
End Class