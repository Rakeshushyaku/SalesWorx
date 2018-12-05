Imports SalesWorx.BO.Common
Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO

Partial Public Class AdminSurveyFSRCustomers
    Inherits System.Web.UI.Page
    Dim objSurvey As New Survey
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const PageID As String = "P50"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim objRoute As New RoutePlan

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
            Dim UD_SUB_QRY = "select SalesRep_ID from dbo.app_GetControlInfo(" & CType(Session("User_Access"), UserAccess).UserID & ")"
            ddlSalesRep.DataSource = objRoute.ShowSalesRepsByUD(Err_No, Err_Desc, UD_SUB_QRY)
            ' ddlSalesRep.DataSource = objSurvey.LoadSalesRep(Err_No, Err_Desc)
            ddlSalesRep.Items.Insert(0, "-- Select --")
            ddlSalesRep.AppendDataBoundItems = True
            ddlSalesRep.DataValueField = "SalesRep_ID"
            ddlSalesRep.DataTextField = "SalesRep_Name"
            ddlSalesRep.DataBind()
            lstDefault.Items.Clear()
            lstSelected.Items.Clear()
        End If

    End Sub



    Private Sub BindDefault()

        Dim TempTbl As New DataTable
        Try
            Err_Desc = Nothing
            Err_No = Nothing
            TempTbl = objSurvey.LoadDefaultFSRCustomers(Err_No, Err_Desc, Integer.Parse(ddlSalesRep.SelectedValue.ToString()))
            If TempTbl IsNot Nothing Then
                lstDefault.DataSource = TempTbl
                lstDefault.DataTextField = "CustName"
                lstDefault.DataValueField = "CustSiteId"
                lstDefault.DataBind()
            End If
            lblCustAssign.Text = "Customers Assigned: [" & lstSelected.Items.Count & "]"
            lblCustAvailed.Text = "Customers Available: [" & lstDefault.Items.Count & "]"

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        End Try
    End Sub

    Private Sub BindSelected()
        Dim TempTbl As New DataTable
        Try
            Err_Desc = Nothing
            Err_No = Nothing
            TempTbl = objSurvey.LoadAssignedFSRCustomers(Err_No, Err_Desc, Integer.Parse(ddlSalesRep.SelectedValue.ToString()))
            If TempTbl IsNot Nothing Then
                lstSelected.DataSource = TempTbl
                lstSelected.DataTextField = "CustName"
                lstSelected.DataValueField = "CustSiteId"
                lstSelected.DataBind()
            End If
            If lstSelected.Items.Count > 0 Then
                Me.btnRemoveAll.Enabled = True
            Else
                Me.btnRemoveAll.Enabled = False
            End If
            lblCustAssign.Text = "Customers Assigned: [" & lstSelected.Items.Count & "]"
            lblCustAvailed.Text = "Customers Available: [" & lstDefault.Items.Count & "]"

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        End Try
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            Err_No = Nothing
            Err_Desc = Nothing
            Dim CustSite() As String
            For Each Item As ListItem In lstDefault.Items
                If Item.Selected Then
                    CustSite = Item.Value.Split("$")
                    objSurvey.SalesRepID = ddlSalesRep.SelectedValue.ToString()
                    objSurvey.CustomerID = CustSite(0).ToString()
                    objSurvey.SiteUsedID = CustSite(1).ToString()
                    objSurvey.AddFSRCustomers(Err_No, Err_Desc)
                    '  Dim s As String() = ddlSalesRep.SelectedItem.Text.Split("-")
                    Dim VanID As String = ddlSalesRep.SelectedValue
                    '  If s.Length > 1 Then
                    'VanID = s(1)
                    '  End If
                    objLogin.SaveUserLog(Err_No, Err_Desc, "I", "SURVEY", "ASSIGN CUSTOMERS TO VAN", VanID.Trim(), "Customer: " & Item.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), VanID.Trim(), CustSite(1).ToString())
                End If
            Next
            BindDefault()
            BindSelected()
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_AdminFSR_005") & "&next=AdminSurveFSRCustomers.aspx&Title=FSR Customers", False)
        End Try
    End Sub

    Protected Sub btnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        Try
            Err_No = Nothing
            Err_Desc = Nothing
            Dim CustSite() As String
            For Each Item As ListItem In lstSelected.Items
                If Item.Selected Then
                    CustSite = Item.Value.Split("$")
                    objSurvey.SalesRepID = ddlSalesRep.SelectedValue.ToString()
                    objSurvey.CustomerID = CustSite(0).ToString()
                    objSurvey.SiteUsedID = CustSite(1).ToString()
                    objSurvey.RemoveFSRCustomers(Err_No, Err_Desc)
                    ' Dim s As String() = ddlSalesRep.SelectedItem.Text.Split("-")
                    Dim VanID As String = ddlSalesRep.SelectedValue
                    ' If s.Length > 1 Then
                    'VanID = s(1)
                    ' End If
                    objLogin.SaveUserLog(Err_No, Err_Desc, "D", "SURVEY", "ASSIGN CUSTOMERS TO VAN", VanID.Trim(), "Customer: " & Item.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), VanID.Trim(), CustSite(1).ToString())
                End If
            Next
            BindDefault()
            BindSelected()
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_AdminFSRCustomers_006") & "&next=AdminSurveyFSRCustomers.aspx&Title=Assign FSR Customer", False)

        End Try
    End Sub

    Protected Sub ddlSalesRep_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlSalesRep.SelectedIndexChanged
        Try
            If ddlSalesRep.SelectedIndex > 0 Then
                ''Bind Default List
                BindDefault()
                ''Bind Selected List
                BindSelected()
                lblCustAssign.Text = "Customers Assigned: [" & lstSelected.Items.Count & "]"
                lblCustAvailed.Text = "Customers Available: [" & lstDefault.Items.Count & "]"
            Else
                lstDefault.Items.Clear()
                lstSelected.Items.Clear()
                lblCustAssign.Text = ""
                lblCustAvailed.Text = ""
            End If
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_AdminFSR_005") & "&next=Welcome.aspx&Title=Admin FSR", False)
        End Try
    End Sub

    Protected Sub btnAddAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddAll.Click
        Try
            Err_No = Nothing
            Err_Desc = Nothing
            Dim CustSite() As String
            For Each Item As ListItem In lstDefault.Items
                ' If Item.Selected Then
                CustSite = Item.Value.Split("$")
                objSurvey.SalesRepID = ddlSalesRep.SelectedValue.ToString()
                objSurvey.CustomerID = CustSite(0).ToString()
                objSurvey.SiteUsedID = CustSite(1).ToString()
                objSurvey.AddFSRCustomers(Err_No, Err_Desc)
                '  Dim s As String() = ddlSalesRep.SelectedItem.Text.Split("-")
                Dim VanID As String = ddlSalesRep.SelectedValue
                '  If s.Length > 1 Then
                'VanID = s(1)
                '  End If
                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "SURVEY", "ASSIGN CUSTOMERS TO VAN", VanID.Trim(), "Customer: " & Item.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), VanID.Trim(), CustSite(1).ToString())
                ' End If
            Next
            BindDefault()
            BindSelected()
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_AdminFSR_005") & "&next=AdminSurveFSRCustomers.aspx&Title=FSR Customers", False)
        End Try
    End Sub

    Protected Sub btnRemoveAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRemoveAll.Click
        Try
            Err_No = Nothing
            Err_Desc = Nothing
            Dim CustSite() As String
            For Each Item As ListItem In lstSelected.Items
                ' If Item.Selected Then
                CustSite = Item.Value.Split("$")
                objSurvey.SalesRepID = ddlSalesRep.SelectedValue.ToString()
                objSurvey.CustomerID = CustSite(0).ToString()
                objSurvey.SiteUsedID = CustSite(1).ToString()
                objSurvey.RemoveFSRCustomers(Err_No, Err_Desc)
                ' Dim s As String() = ddlSalesRep.SelectedItem.Text.Split("-")
                Dim VanID As String = ddlSalesRep.SelectedValue
                ' If s.Length > 1 Then
                'VanID = s(1)
                ' End If
                objLogin.SaveUserLog(Err_No, Err_Desc, "D", "SURVEY", "ASSIGN CUSTOMERS TO VAN", VanID.Trim(), "Customer: " & Item.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), VanID.Trim(), CustSite(1).ToString())
                '  End If
            Next
            BindDefault()
            BindSelected()
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_AdminFSRCustomers_006") & "&next=AdminSurveyFSRCustomers.aspx&Title=Assign FSR Customer", False)

        End Try
    End Sub
End Class