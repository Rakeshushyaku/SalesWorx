Imports SalesWorx.BO.Common
Imports log4net
Imports Telerik.Web.UI
Partial Public Class ManageOrganization
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim dt As New DataTable
    Dim objDivConfig As New DivConfig
    Dim objOrgCtl As New OrgCtl

    Dim objLogin As New SalesWorx.BO.Common.Login
    ' Dim SortField As String = ""
    Private Const ModuleName As String = "ManageOrganization.aspx"
    Private Const PageID As String = "P93"
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
                'Session.Add("BringmeBackHere", ModuleName)
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            If Not HasAuthentication() Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If
            FillOrgs()
            BindGrid()
            FillCurrency()

        Else
            Me.MPEDetails.VisibleOnPageLoad = False


        End If

        lblPop.Text = ""
    End Sub
    Private Function HasAuthentication() As Boolean
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
    End Function

    Sub FillOrgs()

        Dim ds_org As DataSet
        Dim ObjProduct As New Product

        ds_org = ObjProduct.GetOrgsHeads(Err_No, Err_Desc)

        Dim dr As DataRow
        dr = ds_org.Tables(0).NewRow()
        dr(0) = 0
        dr(1) = "-- Select a Organization --"
        ds_org.Tables(0).Rows.InsertAt(dr, 0)
        ddFilterBy.DataValueField = "ORG_HE_ID"
        ddFilterBy.DataTextField = "Description"
        ddFilterBy.DataSource = ds_org.Tables(0)
        ddFilterBy.DataBind()

    End Sub
    Sub FillCurrency()


        Dim dt_cur As DataTable
        dt_cur = objOrgCtl.FillCurrency(Err_No, Err_Desc)
        Dim dr As DataRow
        dr = dt_cur.NewRow()
        dr(0) = "0"
        dr(1) = "-- Select  Currency --"
        dt_cur.Rows.InsertAt(dr, 0)
        ddl_Currency.DataValueField = "Currency_Code"
        ddl_Currency.DataTextField = "Description"
        ddl_Currency.DataSource = dt_cur
        ddl_Currency.DataBind()

    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        btnUpdate.Visible = False
        btnSave.Visible = True
        Resetfields()

        MPEDetails.VisibleOnPageLoad = True
        ClassUpdatePnl.Update()
        Session("Add") = "1"
    End Sub
    Sub BindGrid()
        Try
            Dim dt As New DataTable
            dt = objOrgCtl.GetOrgCTL(Err_No, Err_Desc)

            Dim dv As New DataView(dt)
            If SortField <> "" Then
                dv.Sort = (SortField & " ") + SortDirection
            End If
            gvOrgCtl.DataSource = dv
            gvOrgCtl.DataBind()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
    Public Sub Resetfields()
        Me.ddl_Currency.SelectedValue = "0"
        Me.txtDescription.Text = ""
        Me.btnAdd.Focus()
        Me.ddFilterBy.SelectedIndex = 0
        Me.txtFilterVal.Text = ""
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click


        If Session("Add") = "1" Then




          

            Dim success As Boolean = False
            Try

                If Me.txtDescription.Text.Trim = "" Then

                    Me.lblPop.Text = "Organization Name is required."
                    Me.MPEDetails.VisibleOnPageLoad = True
                    UpdatePanel1.Update()
                    Exit Sub
                End If

                If objOrgCtl.ValidateDescription(0, Me.txtDescription.Text.Trim) = True Then
                    Me.lblPop.Text = "Organization Name is already exist."
                    Me.MPEDetails.VisibleOnPageLoad = True
                    UpdatePanel1.Update()
                    Exit Sub
                End If

                If ddl_Currency.SelectedValue = "0" Then
                    lblPop.Text = "Please select the Currency"
                    MPEDetails.VisibleOnPageLoad = True
                    UpdatePanel1.Update()
                    Exit Sub
                End If


                If objOrgCtl.InsertOrgCTL(Err_No, Err_Desc, txtDescription.Text, ddl_Currency.SelectedValue) = True Then
                    success = True
                    MessageBoxValidation("Successfully saved.", "Information")
                    Me.MPEDetails.VisibleOnPageLoad = False
                    UpdatePanel1.Update()
                    Resetfields()
                    BindGrid()
                    ClassUpdatePnl.Update()
                    Session("Add") = "0"

                    Exit Sub

                End If



            Catch ex1 As SqlClient.SqlException

                log.Error(ex1.Message.ToString)
                lblPop.Text = "Error in Saving Please check log"
                MPEDetails.VisibleOnPageLoad = True
                UpdatePanel1.Update()
                
            Catch ex As Exception

                Err_No = "74205"
                lblPop.Text = "Error in Saving Please check log"
                'MessageBoxValidation("Error in Saving Please check log", "Information")
                MPEDetails.VisibleOnPageLoad = True
                UpdatePanel1.Update()

                If Err_Desc Is Nothing Then
                    log.Error(ex.ToString)
                Else
                    log.Error(ex.ToString)
                End If
            End Try
        Else
            BindGrid()
            ClassUpdatePnl.Update()
        End If
    End Sub

    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Session("Add") = "1"
            lblPop.Text = ""
            btnUpdate.Visible = True
            btnSave.Visible = False
            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)

            HidVal.Value = btnEdit.CommandArgument.ToString()

            Me.txtDescription.Text = Trim(row.Cells(1).Text)
            ddl_Currency.SelectedValue = Trim(row.Cells(2).Text)

            Dim commaIndex As Int32 = Trim(row.Cells(1).Text).LastIndexOf("-")
            Dim firstPart As String = Trim(row.Cells(1).Text).Substring(0, commaIndex)
            Dim secondPart As String = Trim(row.Cells(1).Text).Substring(commaIndex + 1)


            Dim strarr() As String
            strarr = (Trim(row.Cells(1).Text).ToString()).Split("-"c)
            Me.txtDescription.Text = firstPart
            MPEDetails.VisibleOnPageLoad = True
            UpdatePanel1.Update()
        Catch ex As Exception
            Err_No = "74208"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=ManageOrganization.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub
    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click

        If Session("Add") = "1" Then




            Dim success As Boolean = False
            Try


                If Me.txtDescription.Text.Trim = "" Then
                    Me.lblPop.Text = "Organization Name is required."
                    Me.MPEDetails.VisibleOnPageLoad = True
                    UpdatePanel1.Update()
                    Exit Sub
                End If

                If objOrgCtl.ValidateDescription(Convert.ToInt32(HidVal.Value), Me.txtDescription.Text.Trim) = True Then
                    Me.lblPop.Text = "Organization Name is already exist."
                    Me.MPEDetails.VisibleOnPageLoad = True
                    Exit Sub
                End If

                If ddl_Currency.SelectedValue = "0" Then
                    lblPop.Text = "Please select the Currency"
                    MPEDetails.VisibleOnPageLoad = True
                    UpdatePanel1.Update()
                    Exit Sub
                End If

                If objOrgCtl.UpdateOrgCTL(Err_No, Err_Desc, Convert.ToInt32(HidVal.Value), txtDescription.Text, ddl_Currency.SelectedValue) = True Then
                    success = True
                    MessageBoxValidation("Successfully updated.", "Information")
                    Me.MPEDetails.VisibleOnPageLoad = False
                    UpdatePanel1.Update()
                    Resetfields()
                    BindGrid()
                    ClassUpdatePnl.Update()
                    Session("Add") = "0"
                End If


            Catch ex As Exception
                Err_No = "74209"
                lblPop.Text = "Error in Updation Please check log"
                If Err_Desc Is Nothing Then
                    log.Error(GetExceptionInfo(ex))
                Else
                    log.Error(Err_Desc)
                End If
            End Try
        Else
            BindGrid()
            ClassUpdatePnl.Update()
        End If
    End Sub


    'Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
    '    Resetfields()
    '    '  Me.MPEDetails.VisibleOnPageLoad = False

    'End Sub
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btndelete As ImageButton = TryCast(sender, ImageButton)
        Dim row As GridViewRow = DirectCast(btndelete.NamingContainer, GridViewRow)
        '  Dim s As String() = row.Cells(1).Text.Split("-")
        Dim OID As String = row.Cells(1).Text
        '  If s.Length > 1 Then
        'OID = s(1)
        ' End If


        Dim success As Boolean = False
        Try
            If objOrgCtl.ValidateDescription(Err_No, Err_Desc, Convert.ToInt32(btndelete.CommandArgument.ToString())) = True Then
                If objOrgCtl.DeleteOrgCtl(Err_No, Err_Desc, Convert.ToInt32(btndelete.CommandArgument.ToString())) = True Then
                    ' objLogin.SaveUserLog(Err_No, Err_Desc, "D", "MAIN MENU", "ORG CONTROL", OID.Trim(), "Desc: " & row.Cells(1).Text & "/ MFOC :  " & row.Cells(2).Text & "/ ODO Read:  " & row.Cells(3).Text & "/ PDC Post:  " & row.Cells(4).Text & "/ Collection:  " & IIf(row.Cells(5).Text = "", "", row.Cells(5).Text), CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", OID.Trim())
                    success = True

                End If
            Else
                MessageBoxValidation(Err_Desc, "Information")
                Exit Sub
            End If

            If success = True Then
                MessageBoxValidation("Deleted successfully.", "Information")

                BindGrid()
                Resetfields()
                FillOrgs()
                TopPanel.Update()

                ClassUpdatePnl.Update()
            Else
                MessageBoxValidation("Error occured while deleting Organization.", "Information")

                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_ReasonCode_005") & "&next=ManageOrganization.aspx&Title=Message", False)
                Exit Try
            End If

        Catch ex As Exception
            Err_No = "74210"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub
  

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFilter.Click

        Try
            Dim Criteria As String = "0"

            If ddFilterBy.SelectedValue = "0" Then
                MessageBoxValidation("Please Select Organization.", "Information")
                Exit Sub
            End If
        If ddFilterBy.SelectedValue <> "0" Then
            Criteria = ddFilterBy.SelectedValue
        End If
        Dim dt As New DataTable
            dt = objOrgCtl.GetSearchResultOrg(Err_No, Err_Desc, Criteria, txtFilterVal.Text)
        Dim dv As New DataView(dt)
        If SortField <> "" Then
            dv.Sort = (SortField & " ") + SortDirection
        End If
        gvOrgCtl.DataSource = dv
        gvOrgCtl.DataBind()
            ClassUpdatePnl.Update()
        Catch ex As Exception
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

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

    Protected Sub gvOrgCtl_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvOrgCtl.Sorting
        SortField = e.SortExpression
        SortDirection = "flip"
        BindGrid()
    End Sub

    Protected Sub gvOrgCtl_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvOrgCtl.PageIndexChanging
        gvOrgCtl.PageIndex = e.NewPageIndex
        BindGrid()
    End Sub

  
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            Resetfields()
            BindGrid()
            ClassUpdatePnl.Update()
        Catch ex As Exception
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub
  
End Class