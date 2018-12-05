Imports SalesWorx.BO.Common
Imports Telerik.Web.UI
Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports ExcelLibrary.SpreadSheet
Imports System.Threading
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports Telerik.Web
Imports System.Text.RegularExpressions
Imports Telerik

Public Class ManageIncentive
    Inherits System.Web.UI.Page
    Dim objcommon As New SalesWorx.BO.Common.Common
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim objIncentive As New SalesWorx.BO.Common.Incentive
    Dim objDivConfig As New DivConfig
    Private Const ModuleName As String = "ManageIncentive.aspx"
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim HasLots As String
    Dim Allowpricechange As String
    Private Const PageID As String = "P150"
    Private dt As New DataTable
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objControl As New AppControl
    Private _strTempFolder As String = CStr(ConfigurationSettings.AppSettings("ExcelPath"))
    Private PhysicalPath As String = ""
    Private _strMediaFileSize As Long = CLng(ConfigurationSettings.AppSettings("MediaFileSize"))
    Dim T_wtg As Decimal = 0

   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try


        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        If Not Page.IsPostBack Then

            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            If Not HasPermission Then
                Err_No = 500
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If
            Session.Remove("PayoutSlabs")

            FillFilterBy()
                FillOrganization()
                If ddlOrg_I.SelectedIndex = 1 Then
                    BindInParam()
                End If
                If ddlorg_F.SelectedIndex = 1 Then
                    BindSlabs()
                End If

                ' ResetFields()
            Else
                Me.MPEDetails.VisibleOnPageLoad = False
            End If
        Catch ex As Exception
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub




    Protected Sub btnSave_i_Click(sender As Object, e As EventArgs) Handles btnSave_i.Click
        Try
            Me.lblPop.Text = ""
            If ddlOrg_I.SelectedIndex <= 0 Then
                MessageBoxValidation("Please select organization", "Validation") '  Me.lblPop.Text = "Please select organization"
                gvIncentivePara.DataSource = Nothing
                gvIncentivePara.DataBind()
                Exit Sub
            End If

            Dim t_Weightage As Decimal = 0
            For Each item_v As GridDataItem In gvIncentivePara.Items
                Dim Weightage As RadNumericTextBox = (TryCast(item_v.FindControl("txt_incentvalue"), RadNumericTextBox))
                Dim lblActive As Label = DirectCast(item_v.FindControl("lblActive"), Label)

                If lblActive.Text.Trim().ToUpper() = "Y" Then
                    t_Weightage = t_Weightage + Convert.ToDecimal(IIf(Weightage.Text = "", "0", Weightage.Text))
                End If
            Next

            If t_Weightage <> 100 Then
                MessageBoxValidation("Total Weightage Should be equal to 100", "Validation")
                Exit Sub
            End If
            Dim success_cnt As Integer = 0

            For Each item As GridDataItem In gvIncentivePara.Items

                Dim ParameterCode As String = gvIncentivePara.Items(item.ItemIndex).GetDataKeyValue("Code_Value")
                Dim Code_Description As String = gvIncentivePara.Items(item.ItemIndex).GetDataKeyValue("Code_Description")
                '  Dim Weightage As TextBox = (TryCast(item.FindControl("txt_incentvalue"), TextBox))
                Dim Weightage As RadNumericTextBox = (TryCast(item.FindControl("txt_incentvalue"), RadNumericTextBox))
                Dim ROW_ID As String = IIf(gvIncentivePara.Items(item.ItemIndex).GetDataKeyValue("ROW_ID") Is DBNull.Value, "0", gvIncentivePara.Items(item.ItemIndex).GetDataKeyValue("ROW_ID").ToString)


                If objIncentive.ManageIncentive(Err_No, Err_Desc, ROW_ID, ddlOrg_I.SelectedValue, ParameterCode, Weightage.Text, "A", CType(Session("User_Access"), UserAccess).UserID) = True Then
                    '  Me.MPEDetails.VisibleOnPageLoad = True
                    success_cnt = success_cnt + 1
                Else
                    log.Error("Error saving  in ParameterCode : " & ParameterCode & Err_Desc)

                End If
            Next
            If success_cnt = gvIncentivePara.Items.Count Then
                MessageBoxValidation("Incentive  Weightage successfuly saved ", "Information")
            Else
                MessageBoxValidation("Error occured while saving some weightagecplease check log .", "Validation")
            End If
            ResetFields()
        Catch ex As Exception
            MessageBoxValidation("Error occured while saving.", "Validation")
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try

    End Sub
    Protected Sub btnSearch_i_Click(sender As Object, e As EventArgs) Handles btnSearch_i.Click
        Try
            If ddlOrg_I.SelectedIndex <= 0 Then
                MessageBoxValidation("Please select organization", "Validation")
                gvIncentivePara.DataSource = Nothing
                gvIncentivePara.DataBind()
                Exit Sub
            End If

            BindInParam()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
    Protected Sub lbChangeStatus_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Me.lblStatus.Text = "Y"

        Catch ex As Exception
            log.Error(ex.Message)
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
    Protected Sub btnReset_i_Click(sender As Object, e As EventArgs)
        Try
            Me.ddlOrg_I.SelectedIndex = 0
            Me.gvIncentivePara.Visible = False
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub



    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Dim success As Boolean = False
        Try
            Me.gvPercentage_Slabs.Visible = True

            If ddlorg_F.SelectedIndex <= 0 Then
                MessageBoxValidation("Please select organization", "Validation")
                Exit Sub
            End If
            dt = objIncentive.SearchSlab(Err_No, Err_Desc, Me.ddFilterBy.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text), ddlorg_F.SelectedValue)
            Session("PayoutSlabs") = dt
            Me.gvPercentage_Slabs.DataSource = dt
            Me.gvPercentage_Slabs.DataBind()
            Dim dv As New DataView(dt)
            If ViewState("SortField") <> "" Then
                dv.Sort = (ViewState("SortField") & " ") + SortDirection
            End If
            gvPercentage_Slabs.DataSource = dv
            gvPercentage_Slabs.DataBind()
            ClassUpdatePnl.Update()
            success = True
            If success = False Then
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_CurrencyCode_004") & "&next=CurrencyCode.aspx&Title=Message", False)
                Exit Try
            End If
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            Err_No = "94064"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub
    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Try

       
            Me.btnUpdate.Visible = False
            Me.btnSave.Visible = True
        ResetFields()
            Me.lblPop.Text = ""
            Me.ddlPayParam.SelectedIndex = 0
            Me.ddlOrg.SelectedIndex = 0
            If ddlOrg.Items.Count = 2 Then
                ddlOrg.SelectedIndex = 1
            End If
        ClassUpdatePnl.Update()
            Me.MPEDetails.VisibleOnPageLoad = True
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            Me.lblPop.Text = ""
            If ddlOrg.SelectedIndex <= 0 Then
                Me.lblPop.Text = "Please select organization"
                Exit Sub
            End If
            If ddlPayParam.SelectedIndex <= 0 Then
                Me.lblPop.Text = "Please select parameter"
                Exit Sub
            End If
            If txtFrom_p.Text = "" Or txtTo_p.Text = "" Or txtPayout_p.Text = "" Then
                Me.lblPop.Text = "From,To and Payout percentages should not be empty"
                Exit Sub
            End If

            If objIncentive.IsValidslabPercentage(Err_No, Err_Desc, ddlOrg.SelectedValue, "0", ddlPayParam.SelectedValue, txtFrom_p.Text, txtTo_p.Text) Then
                If objIncentive.ManageIncentive_Slabs(Err_No, Err_Desc, "0", ddlOrg.SelectedValue, ddlPayParam.SelectedValue, txtFrom_p.Text, txtTo_p.Text, txtPayout_p.Text, "A", CType(Session("User_Access"), UserAccess).UserID) = True Then
                    MessageBoxValidation("Payout slabs successfuly saved ", "Information")
                    ResetFields()
                    BindSlabs()
                    ClassUpdatePnl.Update()
                End If
            Else
                Me.lblPop.Text = "Percentage slab already exist"
                Exit Sub
            End If


            ResetFields()
            BindSlabs()

        Catch ex As Exception
            MessageBoxValidation("Error occured while saving.", "Validation")
            log.Error(ex.Message.ToString())
        End Try

    End Sub
    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Me.lblPop.Text = ""

            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
            Dim RowID As System.Web.UI.WebControls.Label = row.FindControl("lblRowID")
            RowID_slab.Value = RowID.Text
            Dim dt_slab As DataTable
            dt_slab = Session("PayoutSlabs")

            Dim foundRows() As DataRow
            foundRows = dt_slab.Select("Row_ID='" & RowID.Text & "'")


            If foundRows.Count > 0 Then
                Me.txtFrom_p.Text = foundRows(0)("From_Percentage")
                Me.txtTo_p.Text = foundRows(0)("To_Percentage")
                Me.txtPayout_p.Text = foundRows(0)("Payout_Percentage")

                Me.ddlOrg.SelectedValue = IIf(foundRows(0)("Organization_ID") Is DBNull.Value, "", foundRows(0)("Organization_ID").ToString())
                Me.ddlPayParam.SelectedValue = IIf(foundRows(0)("Parameter_Code") Is DBNull.Value, "", foundRows(0)("Parameter_Code").ToString())
                Me.ddlOrg.Enabled = False
                Me.ddlPayParam.Enabled = False

                Me.MPEDetails.VisibleOnPageLoad = True
                Me.btnSave.Visible = False
                Me.btnUpdate.Visible = True
            End If
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try

    End Sub
    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click

        Try


            Me.lblPop.Text = ""
            If ddlOrg.SelectedIndex <= 0 Then
                Me.lblPop.Text = "Please select organization"
                Exit Sub
            End If
            If ddlPayParam.SelectedIndex <= 0 Then
                Me.lblPop.Text = "Please select parameter"
                Exit Sub
            End If

            If txtFrom_p.Text = "" Or txtTo_p.Text = "" Or txtPayout_p.Text = "" Then
                Me.lblPop.Text = "From,To and Payout percentages should not be empty"
                Exit Sub
            End If

            If objIncentive.IsValidslabPercentage(Err_No, Err_Desc, ddlOrg.SelectedValue, RowID_slab.Value, ddlPayParam.SelectedValue, txtFrom_p.Text, txtTo_p.Text) Then
                If objIncentive.ManageIncentive_Slabs(Err_No, Err_Desc, RowID_slab.Value, ddlOrg.SelectedValue, ddlPayParam.SelectedValue, txtFrom_p.Text, txtTo_p.Text, txtPayout_p.Text, "U", CType(Session("User_Access"), UserAccess).UserID) = True Then
                    MessageBoxValidation("Payout percentages successfuly updated ", "Information")
                    ResetFields()
                    BindSlabs()
                    ClassUpdatePnl.Update()
                    '  Me.MPEDetails.VisibleOnPageLoad = False

                End If
            Else
                Me.lblPop.Text = "Percentage slab already exist"
                Exit Sub
            End If



        Catch ex As Exception
            MessageBoxValidation("Error occured while saving.", "Validation")
            log.Error(ex.Message.ToString())
      
        End Try

    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Try
            ResetFields()
            Me.MPEDetails.VisibleOnPageLoad = False
            ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "Hidewindow();", True)
            ClassUpdatePnl.Update()

        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
    Protected Sub gvIncentivePara_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles gvIncentivePara.ItemCommand
        Try
            If e.CommandName = "Status" Then
                Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
                Dim item As GridDataItem = DirectCast(e.Item, GridDataItem)
                Dim lblROW_ID As Label = DirectCast(item.FindControl("lblROW_ID"), Label)
                Dim lblActive As Label = DirectCast(item.FindControl("lblActive"), Label)

                If Me.lblStatus.Text = "Y" Then
                    Dim Weightage_c As RadNumericTextBox = (TryCast(item.FindControl("txt_incentvalue"), RadNumericTextBox))
                    ' Dim dt_tweightage As New DataTable
                    ' dt_tweightage = objIncentive.GetTotalWeightage(Err_No, Err_Desc, ddlOrg_I.SelectedValue)
                    'If dt_tweightage.Rows.Count > 0 Then
                    '    T_wtg = Convert.ToDecimal(dt_tweightage.Rows(0)(0))
                    'End If



                    Dim t_Weightage As Decimal = 0
                    For Each item_v As GridDataItem In gvIncentivePara.Items
                        Dim Weightage As RadNumericTextBox = (TryCast(item_v.FindControl("txt_incentvalue"), RadNumericTextBox))
                        Dim lbl As Label = DirectCast(item_v.FindControl("lblActive"), Label)

                        If lbl.Text.Trim().ToUpper() = "Y" Then
                            t_Weightage = t_Weightage + Convert.ToDecimal(IIf(Weightage.Text = "", "0", Weightage.Text))
                        End If
                    Next
                    If lblActive.Text = "Y" Then
                        t_Weightage = t_Weightage - Convert.ToDecimal(IIf(Weightage_c.Text = "", "0", Weightage_c.Text))
                    Else
                        t_Weightage = t_Weightage + Convert.ToDecimal(IIf(Weightage_c.Text = "", "0", Weightage_c.Text))
                    End If


                    'If lblActive.Text = "Y" Then
                    '    T_wtg = T_wtg - Convert.ToDecimal(Weightage_c.Text)
                    'Else
                    '    T_wtg = T_wtg + Convert.ToDecimal(Weightage_c.Text)
                    'End If

                    If t_Weightage <> 100 Then
                        MessageBoxValidation("Total active weightage should be equal to 100", "Validation")
                        Exit Sub
                    Else
                        Dim success_cnt As Integer = 0

                        For Each itm As GridDataItem In gvIncentivePara.Items

                            Dim ParameterCode As String = gvIncentivePara.Items(itm.ItemIndex).GetDataKeyValue("Code_Value")
                            Dim Code_Description As String = gvIncentivePara.Items(itm.ItemIndex).GetDataKeyValue("Code_Description")
                            Dim Weightage As RadNumericTextBox = (TryCast(itm.FindControl("txt_incentvalue"), RadNumericTextBox))
                            Dim ROW_ID As String = IIf(gvIncentivePara.Items(itm.ItemIndex).GetDataKeyValue("ROW_ID") Is DBNull.Value, "0", gvIncentivePara.Items(itm.ItemIndex).GetDataKeyValue("ROW_ID").ToString)
                            If objIncentive.ManageIncentive(Err_No, Err_Desc, ROW_ID, ddlOrg_I.SelectedValue, ParameterCode, Weightage.Text, "U", CType(Session("User_Access"), UserAccess).UserID) = True Then
                                success_cnt = success_cnt + 1
                            Else
                                log.Error("Enable/disable section: Error saving  in ParameterCode : " & ParameterCode & Err_Desc)
                            End If
                        Next
                        If success_cnt = gvIncentivePara.Items.Count Then
                            If objIncentive.UpdateIncentiveActive(Err_No, Err_Desc, lblROW_ID.Text, lblActive.Text, CType(Session("User_Access"), UserAccess).UserID) = True Then

                                If lblActive.Text = "Y" Then
                                    MessageBoxValidation("Incentive  Weightage parameter  disabled Successfully", "Information")
                                Else
                                    MessageBoxValidation("Incentive  Weightage parameter enabled Successfully", "Information")
                                End If
                            Else

                            End If
                        Else
                            MessageBoxValidation("Error occured while enable/disable on weightage please check log .", "Validation")
                        End If

                    End If

                End If
                BindInParam()
                Me.lblStatus.Text = "N"
            End If
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try

    End Sub
    Protected Sub lbChangeStatus_slab_Click(sender As Object, e As EventArgs)
        Try
            Dim btnChangeStatus As LinkButton = TryCast(sender, LinkButton)
            Dim row As GridViewRow = DirectCast(btnChangeStatus.NamingContainer, GridViewRow)
            Dim RowID As System.Web.UI.WebControls.Label = row.FindControl("lblROW_ID_slab")
            Dim lblActive As System.Web.UI.WebControls.Label = row.FindControl("lblActive_slab")
            RowID_slab.Value = RowID.Text
            If objIncentive.UpdateIncentiveActive_Slab(Err_No, Err_Desc, RowID_slab.Value, lblActive.Text, CType(Session("User_Access"), UserAccess).UserID) = True Then

                If lblActive.Text = "Y" Then
                    MessageBoxValidation("Incentive payout slab percentage parameter  deleted successfully", "Information") '   MessageBoxValidation("Incentive payout slab percentage parameter  disabled Successfully", "Information")
                Else
                    MessageBoxValidation("Incentive  payout slab percentage parameter enabled successfully", "Information")
                End If
            Else

            End If
            ResetFields()
            BindSlabs()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub




    Private Sub BindInParam()
        Try

            Me.gvIncentivePara.Visible = True
            dt = objIncentive.GetIncentiveParameters(Err_No, Err_Desc, IIf(ddlOrg_I.SelectedIndex <= 0, "0", ddlOrg_I.SelectedValue))
            If dt.Rows.Count > 0 Then
                Me.gvIncentivePara.DataSource = dt
                Me.gvIncentivePara.DataBind()
            End If
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try

    End Sub

    Sub FillFilterBy()
        Try

            Dim objCommon As New SalesWorx.BO.Common.Common
        Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
        Dim dt_param As New DataTable
        dt_param = objIncentive.GetIncentiveParameters(Err_No, Err_Desc, IIf(ddlorg_F.SelectedIndex <= 0, "0", ddlorg_F.SelectedValue))
        ddlPayParam.DataSource = dt_param
        ddlPayParam.Items.Clear()
        ddlPayParam.Items.Add(New RadComboBoxItem("Select Parameter"))
        ddlPayParam.AppendDataBoundItems = True
        ddlPayParam.DataValueField = "Code_Value"
        ddlPayParam.DataTextField = "Code_Description"
            ddlPayParam.DataBind()

        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub

    Sub FillOrganization()
        Try

        
        Dim dt_org As New DataTable
        Dim objCommon As New SalesWorx.BO.Common.Common
        Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)

        dt_org = objIncentive.GetOrganisation(Err_No, Err_Desc, SubQry)
        ddlOrg.DataSource = dt_org
        ddlOrg.Items.Clear()
        ddlOrg.Items.Add(New RadComboBoxItem("Select Organization"))
        ddlOrg.AppendDataBoundItems = True
        ddlOrg.DataValueField = "MAS_Org_ID"
        ddlOrg.DataTextField = "Description"
        ddlOrg.DataBind()


        ddlorg_F.DataSource = dt_org
        ddlorg_F.Items.Clear()
        ddlorg_F.Items.Add(New RadComboBoxItem("Select Organization"))
        ddlorg_F.AppendDataBoundItems = True
        ddlorg_F.DataValueField = "MAS_Org_ID"
        ddlorg_F.DataTextField = "Description"
        ddlorg_F.DataBind()

        ddlOrg_I.DataSource = dt_org
        ddlOrg_I.Items.Clear()
        ddlOrg_I.Items.Add(New RadComboBoxItem("Select Organization"))
        ddlOrg_I.AppendDataBoundItems = True
        ddlOrg_I.DataValueField = "MAS_Org_ID"
        ddlOrg_I.DataTextField = "Description"
        ddlOrg_I.DataBind()

            Try

                If ddlorg_F.Items.Count = 2 Then
                    ddlorg_F.SelectedIndex = 1
                End If
                If ddlOrg_I.Items.Count = 2 Then
                    ddlOrg_I.SelectedIndex = 1
                End If
                If ddlOrg.Items.Count = 2 Then
                    ddlOrg.SelectedIndex = 1
                End If



              
            Catch ex As Exception
                log.Error(ex.Message.ToString())
            End Try

        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub BindSlabs()
        Try
            Me.gvPercentage_Slabs.Visible = True

            dt = objIncentive.SearchSlab(Err_No, Err_Desc, Me.ddFilterBy.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text), ddlorg_F.SelectedValue)
            If dt.Rows.Count = 0 Then
                dt = objIncentive.GetPayoutSlabs(Err_No, Err_Desc, IIf(ddlorg_F.SelectedIndex <= 0 Or ddlorg_F.Text = "", "0", ddlorg_F.SelectedValue))
            End If

            Session("PayoutSlabs") = dt
            Me.gvPercentage_Slabs.DataSource = dt
            Me.gvPercentage_Slabs.DataBind()
            Dim dv As New DataView(dt)
            If ViewState("SortField") <> "" Then
                dv.Sort = (ViewState("SortField") & " ") + SortDirection
            End If
            Me.gvPercentage_Slabs.DataSource = dv
            Me.gvPercentage_Slabs.DataBind()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub ResetFields()
        ' txtFilterVal.Text = ""
        'Me.txtFilterVal.Text = ""
        Me.txtFrom_p.Text = ""
        Me.txtPayout_p.Text = ""
        txtTo_p.Text = ""
        Me.ddlOrg.Enabled = True
        Me.ddlPayParam.Enabled = True
        Me.btnSave.Text = "Save"
        Me.btnSave.Visible = True
        Me.lblPop.Text = ""

    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Private Function HasAuthentication() As Boolean
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
    End Function
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

    
  
    Private Sub gvPercentage_Slabs_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPercentage_Slabs.PageIndexChanging
        Try
            gvPercentage_Slabs.PageIndex = e.NewPageIndex
            dt = objIncentive.SearchSlab(Err_No, Err_Desc, Me.ddFilterBy.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text), ddlorg_F.SelectedValue)
            Session("PayoutSlabs") = dt
            Me.gvPercentage_Slabs.DataSource = dt
            Me.gvPercentage_Slabs.DataBind()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub
  
    Private Sub gvPercentage_Slabs_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvPercentage_Slabs.Sorting
        Try

       
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        dt = objIncentive.SearchSlab(Err_No, Err_Desc, Me.ddFilterBy.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text), ddlorg_F.SelectedValue)
        Me.gvPercentage_Slabs.DataSource = dt
        Me.gvPercentage_Slabs.DataBind()

        Dim dv As New DataView(dt)
        If ViewState("SortField") <> "" Then
            dv.Sort = (ViewState("SortField") & " ") + SortDirection
        End If
        gvPercentage_Slabs.DataSource = dv
        gvPercentage_Slabs.DataBind()
        Session.Remove("PayoutSlabs")
        Session("PayoutSlabs") = dt
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub




  
   


    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try

            Dim btndelete As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btndelete.NamingContainer, GridViewRow)
            Dim RowID As System.Web.UI.WebControls.Label = row.FindControl("lblRowID")
            Dim lblActive As System.Web.UI.WebControls.Label = row.FindControl("lblActive_slab")
            RowID_slab.Value = RowID.Text
            If objIncentive.UpdateIncentiveActive_Slab(Err_No, Err_Desc, RowID_slab.Value, lblActive.Text, CType(Session("User_Access"), UserAccess).UserID) = True Then
                MessageBoxValidation("Incentive payout slab percentage parameter  deleted successfully", "Information")
                ResetFields()
                BindSlabs()
            End If


        Catch ex As Exception
            log.Error(ex.Message.ToString())
            Err_No = "74065"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If

        End Try

    End Sub

    Protected Sub btnReset_Click(sender As Object, e As EventArgs)
        Try
            Me.ddlorg_F.SelectedIndex = 0
            Me.ddFilterBy.SelectedIndex = 0
            Me.txtFilterVal.Text = ""
            Me.gvPercentage_Slabs.Visible = False
            Me.ClassUpdatePnl.Update()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub


   
   

    Private Sub gvIncentivePara_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvIncentivePara.PageIndexChanged
        BindInParam()
    End Sub

    Private Sub gvIncentivePara_SortCommand(sender As Object, e As GridSortCommandEventArgs) Handles gvIncentivePara.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindInParam()
    End Sub

    
End Class
