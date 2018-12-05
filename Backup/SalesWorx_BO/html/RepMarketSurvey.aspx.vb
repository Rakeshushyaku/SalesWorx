Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI

Partial Public Class RepMarketSurvey
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim objCommon As New SalesWorx.BO.Common.Common
    Dim objAssetType As New SalesWorx.BO.Common.AssetType

    Private ReportPath As String = "MarketSurveyList"

    Private Const PageID As String = "P268"

    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub RepRouteMaster_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
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

                Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataTextField = "Description"
                ddlOrganization.DataValueField = "MAS_Org_ID"
                ddlOrganization.DataSource = objCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New ListItem("-- Select a value --"))

                LoadSurveyTypes()

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
        Else
            Me.MapWindow.VisibleOnPageLoad = False
        End If
    End Sub

    Private Sub RVMain_BookmarkNavigation(ByVal sender As Object, ByVal e As Microsoft.Reporting.WebForms.BookmarkNavigationEventArgs) Handles RVMain.BookmarkNavigation
     
        Dim imgurl As String = Nothing
        imgurl = e.BookmarkId.ToString()

        ProdImg.ImageUrl = imgurl
        Me.MapWindow.VisibleOnPageLoad = True



    End Sub
    Sub LoadCustomer()

        Dim x As New DataTable
        x = objCommon.GetCustomerByCriteria(Err_No, Err_Desc, IIf(ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue))

        Dim r As DataRow = x.NewRow()
        r(0) = "0"
        r(1) = ""

        x.Rows.InsertAt(r, 0)
        ddlCustomer.ClearSelection()
        ddlCustomer.Items.Clear()
        ddlCustomer.Text = ""
        ddlCustomer.SelectedIndex = 0
        ddlCustomer.DataValueField = "CustomerID"
        ddlCustomer.DataTextField = "Customer"
        ddlCustomer.DataSource = x
        ddlCustomer.DataBind()

    End Sub

    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        Me.MapWindow.VisibleOnPageLoad = False
        LoadCustomer()

    End Sub
    Private Sub LoadSurveyTypes()
        Dim y As New DataTable
        Dim objsurv As New Survey
        y = objsurv.GetMarketSurveys(Err_No, Err_Desc)
       

        ddlSurveyType.SelectedIndex = 0
        ddlSurveyType.DataValueField = "Survey_ID"
        ddlSurveyType.DataTextField = "Survey_Title"
        ddlSurveyType.DataSource = y
        ddlSurveyType.DataBind()
    End Sub
    Protected Sub SearchBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SearchBtn.Click

        If ddlOrganization.SelectedIndex = 0 Or Me.ddlSurveyType.SelectedIndex <= 0 Then
            MessageBoxValidation("Select organization and survey.")
            Exit Sub
        End If


        Dim OrgID As New ReportParameter
        OrgID = New ReportParameter("OrgID", ddlOrganization.SelectedValue)

        Dim SurveyID As New ReportParameter
        SurveyID = New ReportParameter("SurveyID", CStr(IIf(Me.ddlSurveyType.SelectedIndex <= 0, "0", Me.ddlSurveyType.SelectedValue.ToString())))

        Dim Customer As New ReportParameter
        Customer = New ReportParameter("Customer", CStr(IIf(Me.ddlCustomer.SelectedIndex <= 0, "0", Me.ddlCustomer.SelectedValue)))


        Dim OrgName As New ReportParameter
        OrgName = New ReportParameter("OrgName", ddlOrganization.SelectedItem.Text)

     

        With RVMain
            .Reset()
            .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
            .ServerReport.SetParameters(New ReportParameter() {OrgName, OrgID, SurveyID, Customer})
            '.ServerReport.Refresh()
            .Visible = True
        End With
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