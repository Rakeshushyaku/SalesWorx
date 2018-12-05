Imports SalesWorx.BO.Common
Imports log4net
Partial Public Class VanToOrgAndSalesMan
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim dt As New DataTable
    Dim objOrgConfig As New OrgConfig
    Dim objLogin As New SalesWorx.BO.Common.Login
    ' Dim SortField As String = ""
    Private Const ModuleName As String = "VanToOrgAndSalesMan.aspx"
    Private Const PageID As String = "P217"
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack() Then
            If Session.Item("USER_ACCESS") Is Nothing Then
                Session.Add("BringmeBackHere", ModuleName)
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            If Not HasAuthentication() Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If
            FillVanOrgs()
            FillOrgs()

            FillSourceOrgs()
            BindGrid("1=1")
            ViewState("Criteria") = "1=1"
        End If
        lblmsgPopUp.Text = ""
    End Sub
    Private Function HasAuthentication() As Boolean
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
    End Function
    Sub FillSourceOrgs()
        Try

       
            dt = New DataTable()

            Dim ObjeOrgConfig As New OrgConfig
            dt = ObjeOrgConfig.GetDistinctWareHouses(Err_No, Err_Desc)
            Dim dr As DataRow
            dr = dt.NewRow()
            dr(0) = "0"
            dr(1) = "Default"
            dt.Rows.InsertAt(dr, 0)
            drpSourcWH.DataSource = dt
            drpSourcWH.DataValueField = "Org_ID"
            drpSourcWH.DataTextField = "Description"
            drpSourcWH.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    Sub FillVanOrgs()
        Try

            dt = New DataTable()
            Dim ObjeOrgConfig As New OrgConfig
            dt = ObjeOrgConfig.GetDistinctVanOrgs(Err_No, Err_Desc)
            drpOrganization.DataSource = dt
            drpOrganization.DataValueField = "Org_ID"
            drpOrganization.DataTextField = "SalesRep_Name"
            drpOrganization.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    Sub FillEmps(ByVal EmpCode As String, ByVal EmpName As String)
        Try
            dt = New DataTable()
            Dim ObjeOrgConfig As New OrgConfig
            dt = ObjeOrgConfig.GetSalesMan(Err_No, Err_Desc)
            If Trim(EmpName) <> "" And Trim(EmpName) <> "&nbsp;" Then
                Dim dr1 As DataRow
                dr1 = dt.NewRow()
                dr1(0) = EmpCode
                dr1(1) = EmpName
                dt.Rows.InsertAt(dr1, 0)
            End If

            dt.DefaultView.Sort = "Emp_Name" & " " & "Asc"
            dt = dt.DefaultView.ToTable()

            If Trim(EmpName) = "" Or Trim(EmpName) = "&nbsp;" Then
               
                Dim dr As DataRow
                dr = dt.NewRow()
                dr(0) = "-1"
                dr(1) = "--Select Sales Man---"
                dt.Rows.InsertAt(dr, 0)
            End If

            If Trim(EmpName) <> "" And Trim(EmpName) <> "&nbsp;" Then
                Dim Undr As DataRow
                Undr = dt.NewRow()
                Undr(0) = "0"
                Undr(1) = "--Unassign Sales Man---"
                dt.Rows.Add(Undr)
            End If

            DrpSalesMan.DataSource = dt
            DrpSalesMan.DataValueField = "Emp_Code"
            DrpSalesMan.DataTextField = "Emp_Name"
            DrpSalesMan.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    Sub FillOrgs()
        dt = New DataTable()
        Dim objCommon As New Common
        Dim ObjeOrgConfig As New OrgConfig
        Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
        dt = ObjeOrgConfig.GetOrganisation(Err_No, Err_Desc, SubQry)

        Dim dr As DataRow
        dr = dt.NewRow()
        dr(0) = "0"
        dr(1) = "-- Select a Organization --"
        dt.Rows.InsertAt(dr, 0)
        drpLocalOrg.DataSource = dt
        drpLocalOrg.DataValueField = "MAS_Org_ID"
        drpLocalOrg.DataTextField = "Description"
        drpLocalOrg.DataBind()

        'dt = New DataTable()
        'dt = objDivConfig.GetAllDivisions(Err_No, Err_Desc)
        'drpOrganization.DataValueField = "ORG_HE_ID"
        'drpOrganization.DataTextField = "Description"
        'drpOrganization.DataSource = dt
        'drpOrganization.DataBind()
        ddFilterBy.DataValueField = "MAS_Org_ID"
        ddFilterBy.DataTextField = "Description"
        ddFilterBy.DataSource = dt
        ddFilterBy.DataBind()

    End Sub
    Sub BindGrid(ByVal Criteria As String)
        Try
            Dim dt As New DataTable
            dt = objOrgConfig.GetOrgConfiguration(Err_No, Err_Desc, Criteria)
            Dim dv As New DataView(dt)
            If SortField <> "" Then
                dv.Sort = (SortField & " ") + SortDirection
            End If
            gvDivConfig.DataSource = dv
            gvDivConfig.DataBind()

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
    Public Sub Resetfields()
        Me.drpOrganization.ClearSelection()
        Me.drpOrganization.Enabled = True
        drpSourcWH.ClearSelection()
        DrpSalesMan.ClearSelection()

    End Sub
    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try

            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)

            HidVal.Value = btnEdit.CommandArgument.ToString()

            Dim Emp_Code As String
            Emp_Code = CType(row.FindControl("HEmp_Code"), HiddenField).Value
           
           
            FillEmps(Emp_Code, row.Cells(4).Text)
            btnUpdate.Visible = True
            'Resetfields()


            Dim li As ListItem = drpOrganization.Items.FindByValue(HidVal.Value)
            drpOrganization.SelectedIndex = drpOrganization.Items.IndexOf(li)
            drpOrganization.Enabled = False

            
            If Not drpOrganization.Items.FindByValue(HidVal.Value) Is Nothing Then
                drpOrganization.ClearSelection()
                drpOrganization.Items.FindByValue(HidVal.Value).Selected = True
            End If

            Dim Stock_Org_ID As String
            Stock_Org_ID = CType(row.FindControl("HStock_Org_ID"), HiddenField).Value
            If Not drpSourcWH.Items.FindByValue(Stock_Org_ID) Is Nothing Then
                drpSourcWH.ClearSelection()
                drpSourcWH.Items.FindByValue(Stock_Org_ID).Selected = True
            End If

            Dim Org_HE_ID As String
            Org_HE_ID = CType(row.FindControl("HOrg_HE_ID"), HiddenField).Value
            If Not drpLocalOrg.Items.FindByValue(Org_HE_ID) Is Nothing Then
                drpLocalOrg.ClearSelection()
                drpLocalOrg.Items.FindByValue(Org_HE_ID).Selected = True
            End If
            drpLocalOrg.Enabled = False

            If Not DrpSalesMan.Items.FindByValue(Emp_Code) Is Nothing Then
                DrpSalesMan.ClearSelection()
                DrpSalesMan.Items.FindByValue(Emp_Code).Selected = True
            End If
            MPEDivConfig.Show()
        Catch ex As Exception
            Err_No = "74208"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=VanToOrgAndSalesMan.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub
    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click


        Dim success As Boolean = False
        Try
            If DrpSalesMan.SelectedValue = "-1" Then
                Me.lblMessage.Text = "Please select the sales man"
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
                Exit Sub
            End If
            objOrgConfig.Org_ID = drpOrganization.SelectedValue
            objOrgConfig.Stock_Org_ID = drpSourcWH.SelectedValue
            objOrgConfig.Sales_org_ID = drpLocalOrg.SelectedValue
            objOrgConfig.Emp_code = DrpSalesMan.SelectedValue

            If objOrgConfig.UpdateOrgConfig(Err_No, Err_Desc) = True Then
                success = True
                Me.lblMessage.Text = "Successfully Updated."
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
            End If

            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "U", "MAIN MENU", "ORG CONFIGURATION", Me.drpOrganization.SelectedValue.ToString(), "Code: " & Me.drpOrganization.SelectedItem.Text & "/ Sales Org :  " & drpLocalOrg.SelectedItem.Text & "/ Source Org:  " & drpSourcWH.SelectedItem.Text & "/ Sales Man:  " & DrpSalesMan.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", Me.drpOrganization.SelectedValue.ToString())
                'Resetfields()

                BindGrid("1=1")
                Me.MPEDivConfig.Hide()
                ClassUpdatePnl.Update()
            Else
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_003") & "&next=VanToOrgAndSalesMan.aspx&Title=Message", False)
                Exit Try
            End If

        Catch ex As Exception
            Err_No = "74209"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub
    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        MpInfoError.Show()
        Exit Sub
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Resetfields()
        MPEDivConfig.Hide()
    End Sub
    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFilter.Click
        Dim Criteria As String = "1=1"
        If ddFilterBy.SelectedValue <> "0" Then
            Criteria += " and A.Org_HE_ID='" + ddFilterBy.SelectedValue & "'"
        End If
        ViewState("Criteria") = Criteria
        BindGrid(Criteria)
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

    Protected Sub gvDivConfig_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvDivConfig.Sorting
        SortField = e.SortExpression
        SortDirection = "flip"
        BindGrid(ViewState("Criteria").ToString())
    End Sub

    Protected Sub gvDivConfig_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDivConfig.PageIndexChanging
        gvDivConfig.PageIndex = e.NewPageIndex
        BindGrid(ViewState("Criteria").ToString())
    End Sub

    
End Class