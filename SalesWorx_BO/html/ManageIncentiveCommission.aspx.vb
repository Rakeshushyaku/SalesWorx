Imports SalesWorx.BO.Common
Imports System.Data
Imports System.Web
Imports System.Resources
Imports System.Web.UI.WebControls
Imports log4net
Imports System.Text.RegularExpressions
Imports Telerik.Web.UI
Imports System.IO
Imports System.Data.OleDb
Imports ExcelLibrary.SpreadSheet

Partial Public Class ManageIncentiveCommission
    Inherits System.Web.UI.Page
    Dim objCurrency As New Currency

    Dim objIncentive As New SalesWorx.BO.Common.Incentive
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim objProduct As New SalesWorx.BO.Common.Product
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim Dt As New DataTable
    Private dtErrors As New DataTable
    Private Const PageID As String = "P151"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

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


            FillOrganization()
            Resetfields()
            Session.Remove("dtDsErrors")
            Session.Remove("DsLogFile")
            Session.Remove("Commission")
            SetErrorsTable()
                month_picker.MinDate = DateTime.Now.Date
                Dim s As Date = Convert.ToDateTime(DateTime.Now.Date.ToString("MMM") & DateTime.Now.Date.Year.ToString())
                Me.monthpicker_F.SelectedDate = DateTime.Now.Date


                If ddlorg_F.SelectedIndex = 1 Then
                    BindCommission()
                    ClassUpdatePnl.Update()
                End If
        Else
            Me.MPECommission.VisibleOnPageLoad = False
        End If
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub
    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Dim success As Boolean = False
        Me.MPECommission_import.VisibleOnPageLoad = False
        Me.gvIncentiveCommission.Visible = True
        Try
            Dim m As Integer
            Dim y As Integer
            If monthpicker_F.SelectedDate Is Nothing Then
                m = 0
                y = 0
            Else
                m = monthpicker_F.SelectedDate.Value.Month
                y = monthpicker_F.SelectedDate.Value.Year
            End If
            If ddlorg_F.SelectedIndex <= 0 Then
                MessageBoxValidation("Please select organization", "Information")
                Exit Sub
            End If

            Dt = objIncentive.GetIncentiveCommission(Err_No, Err_Desc, ddlorg_F.SelectedValue, m, y, Me.ddFilterBy.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text))
            BindCommission()
            ClassUpdatePnl.Update()
            success = True
            If success = False Then
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_CurrencyCode_004") & "&next=CurrencyCode.aspx&Title=Message", False)
                Exit Try
            End If
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            Err_No = "74064"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Private Sub ddlClassification_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddlClassification.ItemsRequested

        Try

        
        Dim Objrep As New SalesWorx.BO.Common.Reports()



        Dim dt As New DataTable

        If dt.Rows.Count > 0 Then
            dt.Rows.Clear()
        End If

        dt = objIncentive.GetClassification(Err_No, Err_Desc, ddl_org.SelectedValue, e.Text)

        Dim ItemsPerRequest As Integer = 100 'Set the number of values to show
        Dim itemOffset As Integer = e.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
        e.EndOfItems = endOffset = dt.Rows.Count

        'Loop through the values to populate the combo box
        For i As Integer = itemOffset To endOffset - 1
            Dim item As New RadComboBoxItem()
            item.Text = dt.Rows(i).Item("Classification").ToString
            item.Value = dt.Rows(i).Item("Classification").ToString
            ddlClassification.Items.Add(item)
            item.DataBind()
            Next
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            btnUpdate.Visible = False
        btnSave.Visible = True
        Resetfields()
        Me.gvIncentiveCommission.Visible = True
        Me.ddl_UOM.Enabled = True
        Me.ddlClassification.Enabled = True
            Me.ddlClassification.ClearSelection()
            Me.ddlClassification.Items.Clear()
            Me.ddl_UOM.ClearSelection()
            Me.ddl_UOM.Items.Clear()
        Me.MPECommission_import.VisibleOnPageLoad = False
        Me.lblPop.Text = ""
            If ddl_org.Items.Count = 2 Then
                ddl_org.SelectedIndex = 1
            End If
            ClassUpdatePnl.Update()
            UpdatePanel1.Update()
        Me.MPECommission.VisibleOnPageLoad = True
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Me.gvIncentiveCommission.Visible = True
        If Me.txtCommission.Text = "" Or Me.ddl_org.SelectedIndex <= 0 Or Me.ddlClassification.Text = "" Or Me.month_picker.SelectedDate Is Nothing Then
            Me.lblPop.Text = "Organization, Classification,UOM,Commission and Incentive month are required."
            Me.MPECommission.VisibleOnPageLoad = True
            Exit Sub
        End If

        If ddlClassification.SelectedValue.ToString().Trim() = "" Then
            Me.lblPop.Text = "Select Classification."
            Me.MPECommission.VisibleOnPageLoad = True
            Exit Sub
        End If

        If ddl_UOM.SelectedValue.ToString().Trim() = "" Then
            Me.lblPop.Text = "Select UOM."
            Me.MPECommission.VisibleOnPageLoad = True
            Exit Sub
        End If

        Dim dt_exist As DataTable

        Dim success As Boolean = False
        Try

            dt_exist = objIncentive.CheckIncentive_CommissionExist(Err_No, Err_Desc, ddl_org.SelectedValue, ddlClassification.SelectedValue, month_picker.SelectedDate.Value.Month, month_picker.SelectedDate.Value.Year)
            If dt_exist.Rows.Count = 0 Then
                If objIncentive.ManageIncentive_Commission(Err_No, Err_Desc, "0", ddl_org.SelectedValue, ddlClassification.SelectedValue, txtCommission.Text, ddl_UOM.SelectedValue, month_picker.SelectedDate.Value.Month, month_picker.SelectedDate.Value.Year, CType(Session("User_Access"), UserAccess).UserID) Then
                    MessageBoxValidation("Successfully saved", "Information")
                End If
            Else
                Me.lblPop.Text = "Commission already exist  ."
                Me.MPECommission.VisibleOnPageLoad = True
                Exit Sub
            End If



            Dim m As Integer
            Dim y As Integer
            If monthpicker_F.SelectedDate Is Nothing Then
                m = 0
                y = 0
            Else
                m = monthpicker_F.SelectedDate.Value.Month
                y = monthpicker_F.SelectedDate.Value.Year
            End If
            Dt = objIncentive.GetIncentiveCommission(Err_No, Err_Desc, IIf(ddlorg_F.SelectedValue.ToString().Trim() = "", ddl_org.SelectedValue, ddlorg_F.SelectedValue), m, y, Me.ddFilterBy.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text))
            BindCommission()
            Me.gvIncentiveCommission.Visible = True
            ClassUpdatePnl.Update()

        Catch ex As Exception
            log.Error(ex.Message.ToString())
            Err_No = "74061"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Me.gvIncentiveCommission.Visible = True
            lblPop.Text = ""
            btnUpdate.Visible = True
            btnSave.Visible = False
            Resetfields()


            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
            Dim RowID As System.Web.UI.WebControls.Label = row.FindControl("lblRowID")
            HidVal.Value = RowID.Text

            Dim dt_slab As DataTable
            dt_slab = Session("Commission")

            Dim foundRows() As DataRow
            foundRows = dt_slab.Select("Row_ID='" & RowID.Text & "'")


            If foundRows.Count > 0 Then

                If IIf(foundRows(0)("Incentive_Year") Is DBNull.Value, DateTime.Now.Year, foundRows(0)("Incentive_Year").ToString()) < DateTime.Now.Year Then
                    MessageBoxValidation("Cant edit previous Commission.", "Information")
                    Exit Sub
                End If

                If DateTime.Now.Year = IIf(foundRows(0)("Incentive_Year") Is DBNull.Value, DateTime.Now.Year, foundRows(0)("Incentive_Year").ToString()) Then
                    If DateTime.Now.Month > IIf(foundRows(0)("Incentive_month") Is DBNull.Value, DateTime.Now.Month, foundRows(0)("Incentive_month").ToString()) Then
                        MessageBoxValidation("Cant edit previous Commission.", "Information")
                        Exit Sub
                    End If

                End If

                Me.txtCommission.Text = foundRows(0)("Commission")
                Me.ddl_org.SelectedValue = IIf(foundRows(0)("Organization_ID") Is DBNull.Value, "", foundRows(0)("Organization_ID").ToString())



                Dim item As New RadComboBoxItem()
                item.Text = foundRows(0)("Classification").ToString
                item.Value = foundRows(0)("Classification").ToString
                ddlClassification.Items.Add(item)
                item.DataBind()

                LoadDDL()
                Me.ddlClassification.SelectedValue = foundRows(0)("Classification").ToString
                Me.ddl_UOM.SelectedValue = foundRows(0)("UOM")

                Me.ddl_org.Enabled = False
                Me.month_picker.Enabled = False

                Me.MPECommission.VisibleOnPageLoad = True
                Me.btnSave.Visible = False
                Dim s As Date = Convert.ToDateTime(IIf(foundRows(0)("Tmonth") Is DBNull.Value, DateTime.Now.Month, foundRows(0)("Tmonth").ToString()) & IIf(foundRows(0)("Incentive_Year") Is DBNull.Value, DateTime.Now.Year, foundRows(0)("Incentive_Year").ToString()))
                Me.month_picker.MinDate = s
                Me.month_picker.SelectedDate = s
                Me.ddl_UOM.Enabled = False
                Me.ddlClassification.Enabled = False
            End If
            Me.MPECommission.VisibleOnPageLoad = True
            ClassUpdatePnl.Update()

        Catch ex As Exception
            log.Error(ex.Message.ToString())
            Err_No = "74066"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_CurrencyCode_006") & "&next=CurrencyCode.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub


    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Me.gvIncentiveCommission.Visible = True
        If Me.txtCommission.Text = "" Or Me.ddl_org.SelectedIndex <= 0 Or Me.ddlClassification.Text = "" Or Me.month_picker.SelectedDate Is Nothing Then
            Me.lblPop.Text = "Organization, Classification,UOM,Commission and Incentive month are required."
            Me.MPECommission.VisibleOnPageLoad = True
            Exit Sub
        End If


        If ddlClassification.SelectedValue.ToString().Trim() = "" Then
            Me.lblPop.Text = "Select Classification."
            Me.MPECommission.VisibleOnPageLoad = True
            Exit Sub
        End If

        If ddl_UOM.SelectedValue.ToString().Trim() = "" Then
            Me.lblPop.Text = "Select UOM."
            Me.MPECommission.VisibleOnPageLoad = True
            Exit Sub
        End If

        Dim success As Boolean = False
        Try


            If objIncentive.ManageIncentive_Commission(Err_No, Err_Desc, HidVal.Value, ddl_org.SelectedValue, ddlClassification.SelectedValue, txtCommission.Text, ddl_UOM.SelectedValue, month_picker.SelectedDate.Value.Month, month_picker.SelectedDate.Value.Year, CType(Session("User_Access"), UserAccess).UserID) Then
                Me.MPECommission.VisibleOnPageLoad = False
                Me.ddl_UOM.Enabled = True
                Me.ddlClassification.Enabled = True
                MessageBoxValidation("Successfully updated", "Information")
            Else
                Me.lblPop.Text = "Error in updation ."
                Me.MPECommission.VisibleOnPageLoad = True
                Exit Sub
            End If
            Dim m As Integer
            Dim y As Integer
            If monthpicker_F.SelectedDate Is Nothing Then
                m = 0
                y = 0
            Else
                m = monthpicker_F.SelectedDate.Value.Month
                y = monthpicker_F.SelectedDate.Value.Year
            End If
            Dt = objIncentive.GetIncentiveCommission(Err_No, Err_Desc, IIf(ddlorg_F.SelectedValue.ToString().Trim() = "", ddl_org.SelectedValue, ddlorg_F.SelectedValue), m, y, Me.ddFilterBy.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text))
            BindCommission()
            Me.gvIncentiveCommission.Visible = True
            ClassUpdatePnl.Update()

        Catch ex As Exception
            log.Error(ex.Message.ToString())
            Err_No = "74062"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub


    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Try
            Resetfields()
            Me.MPECommission.VisibleOnPageLoad = False
            ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "HideRadWindow();", True)
            'ClassUpdatePnl.Update()

        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Protected Sub lbChangeStatus_Commission_Click(sender As Object, e As EventArgs)
        Try
            Dim btnChangeStatus As LinkButton = TryCast(sender, LinkButton)
            Dim row As GridViewRow = DirectCast(btnChangeStatus.NamingContainer, GridViewRow)
            Dim RowID As System.Web.UI.WebControls.Label = row.FindControl("lblROW_ID")
            Dim lblActive As System.Web.UI.WebControls.Label = row.FindControl("lblActive")
            HidVal.Value = RowID.Text

            Dim lblOrg As System.Web.UI.WebControls.Label = row.FindControl("lblOrg")
            Dim lblMonth As System.Web.UI.WebControls.Label = row.FindControl("lblMonth")

            If lblActive.Text <> "Y" Then
                Dim dt_rslt As New DataTable
                dt_rslt = objIncentive.CheckIncentive_CommissionExist(Err_No, Err_Desc, lblOrg.Text, row.Cells(6).Text, lblMonth.Text, row.Cells(6).Text)
                If dt_rslt.Rows.Count > 0 Then
                    MessageBoxValidation("Commission already exist.", "Information")
                    Exit Sub
                End If

            End If

            If objIncentive.UpdateIncentiveActive_Commission(Err_No, Err_Desc, HidVal.Value, lblActive.Text, CType(Session("User_Access"), UserAccess).UserID) = True Then

                If lblActive.Text = "Y" Then
                    MessageBoxValidation("Commission  disabled Successfully", "Information")
                Else
                    MessageBoxValidation("Commission enabled Successfully", "Information")
                End If
            Else

            End If
            Resetfields()
            Me.gvIncentiveCommission.Visible = True
            Dim m As Integer
            Dim y As Integer
            If monthpicker_F.SelectedDate Is Nothing Then
                m = 0
                y = 0
            Else
                m = monthpicker_F.SelectedDate.Value.Month
                y = monthpicker_F.SelectedDate.Value.Year
            End If
            Dt = objIncentive.GetIncentiveCommission(Err_No, Err_Desc, ddlorg_F.SelectedValue, m, y, Me.ddFilterBy.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text))
            BindCommission()
            ClassUpdatePnl.Update()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub

    Public Sub Resetfields()
        Try


      
        Me.txtFilterVal.Text = ""
        Me.ddl_org.SelectedIndex = 0
        Me.ddlClassification.Text = ""
        Me.txtCommission.Text = ""
        Me.btnSave.Text = "Save"
        Me.ddl_org.Enabled = True

        Me.month_picker.Enabled = True
        Me.month_picker.MinDate = DateTime.Now.Date
        Me.monthpicker_F.Enabled = True
            ' Me.monthpicker_F.MinDate = DateTime.Now.Date
            '   Me.monthpicker_F.SelectedDate = Me.monthpicker_F.MinDate
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Protected Sub ddlClassification_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs)
        LoadDDL()
    End Sub

    Private Sub gvIncentiveCommission_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvIncentiveCommission.PageIndexChanging
        Try

       
        gvIncentiveCommission.PageIndex = e.NewPageIndex
        Dim m As Integer
        Dim y As Integer
        If monthpicker_F.SelectedDate Is Nothing Then
            m = 0
            y = 0
        Else
            m = monthpicker_F.SelectedDate.Value.Month
            y = monthpicker_F.SelectedDate.Value.Year
        End If
        Dt = objIncentive.GetIncentiveCommission(Err_No, Err_Desc, ddlorg_F.SelectedValue, m, y, Me.ddFilterBy.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text))
        BindCommission()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Private Sub gvIncentiveCommission_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvIncentiveCommission.Sorting
        Try
            ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        Dim m As Integer
        Dim y As Integer
        If monthpicker_F.SelectedDate Is Nothing Then
            m = 0
            y = 0
        Else
            m = monthpicker_F.SelectedDate.Value.Month
            y = monthpicker_F.SelectedDate.Value.Year
        End If
        Dt = objIncentive.GetIncentiveCommission(Err_No, Err_Desc, ddlorg_F.SelectedValue, m, y, Me.ddFilterBy.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text))
            BindCommission()


            Me.gvIncentiveCommission.DataSource = Dt
            Me.gvIncentiveCommission.DataBind()

            Dim dv As New DataView(Dt)
            If ViewState("SortField") <> "" Then
                dv.Sort = (ViewState("SortField") & " ") + SortDirection
            End If
            gvIncentiveCommission.DataSource = dv
            gvIncentiveCommission.DataBind()
            Session.Remove("Target")
            Session("Target") = Dt

        Catch ex As Exception
            log.Error(ex.Message.ToString())
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

 
    Protected Sub ddFilterBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddFilterBy.SelectedIndexChanged
        If Me.ddFilterBy.SelectedIndex <= 0 Then
            Me.txtFilterVal.Text = ""
        End If
    End Sub

    Private Sub BindCommission()
        Try
            Dim m As Integer
            Dim y As Integer
            If monthpicker_F.SelectedDate Is Nothing Then
                m = 0
                y = 0
            Else
                m = monthpicker_F.SelectedDate.Value.Month
                y = monthpicker_F.SelectedDate.Value.Year
            End If
            If ddlorg_F.SelectedIndex <= 0 Then
                MessageBoxValidation("Please select organization", "Information")
                Exit Sub
            End If

            Dt = objIncentive.GetIncentiveCommission(Err_No, Err_Desc, ddlorg_F.SelectedValue, m, y, Me.ddFilterBy.SelectedItem.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text))

            Session("Commission") = Dt
            Me.gvIncentiveCommission.DataSource = Dt
            Me.gvIncentiveCommission.DataBind()
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
            ddl_org.DataSource = dt_org
            ddl_org.Items.Clear()
            ddl_org.Items.Add(New RadComboBoxItem("Select Organization"))
            ddl_org.AppendDataBoundItems = True
            ddl_org.DataValueField = "MAS_Org_ID"
            ddl_org.DataTextField = "Description"
            ddl_org.DataBind()


            ddlorg_F.DataSource = dt_org
            ddlorg_F.Items.Clear()
            ddlorg_F.Items.Add(New RadComboBoxItem("Select Organization"))
            ddlorg_F.AppendDataBoundItems = True
            ddlorg_F.DataValueField = "MAS_Org_ID"
            ddlorg_F.DataTextField = "Description"
            ddlorg_F.DataBind()

            'ddl_orgimport.DataSource = dt_org
            'ddl_orgimport.Items.Clear()
            'ddl_orgimport.Items.Add(New RadComboBoxItem("Select Organization"))
            'ddl_orgimport.AppendDataBoundItems = True
            'ddl_orgimport.DataValueField = "MAS_Org_ID"
            'ddl_orgimport.DataTextField = "Description"
            'ddl_orgimport.DataBind()

            Try
                If ddlorg_F.Items.Count = 2 Then
                    ddlorg_F.SelectedIndex = 1
                End If


            Catch ex As Exception
                log.Error(ex.Message.ToString())
            End Try

        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try

    End Sub

    Sub LoadDDL()
        'ddl_ParameterCode.DataSource = objIncentive.GetIncentiveParameters(Err_No, Err_Desc, ddl_org.SelectedValue)
        'ddl_ParameterCode.Items.Clear()
        'ddl_ParameterCode.Items.Add(New RadComboBoxItem("Select Parameter"))
        'ddl_ParameterCode.AppendDataBoundItems = True
        'ddl_ParameterCode.DataTextField = "Code_Value"
        'ddl_ParameterCode.DataValueField = "Code_Value"
        'ddl_ParameterCode.DataBind()

        ddl_UOM.DataSource = objIncentive.GetUOM(Err_No, Err_Desc, ddl_org.SelectedValue, ddlClassification.SelectedValue)
        ddl_UOM.Items.Clear()
        ddl_UOM.Items.Add(New RadComboBoxItem("Select UOM"))
        ddl_UOM.AppendDataBoundItems = True
        ddl_UOM.DataTextField = "Item_UOM"
        ddl_UOM.DataValueField = "Item_UOM"
        ddl_UOM.DataBind()




    End Sub


    Protected Sub btn_Import_Click(sender As Object, e As EventArgs)
        Try

            btnUpdate.Visible = False
            btnSave.Visible = True
            Resetfields()

            Me.lblPop.Text = ""

            Me.MPECommission_import.VisibleOnPageLoad = True
            ClassUpdatePnl.Update()

        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Protected Sub btn_importFile_Click(sender As Object, e As EventArgs)
        Try


            'If ddl_orgimport.SelectedItem.Value = "0" Then
            '    lblmsgPopUp.Text = "Please Select the Organization."
            '    Me.MPECommission_import.VisibleOnPageLoad = True
            '    ClassUpdatePnl.Update()
            '    Return
            'End If

            If Not file_import.HasFile Then

                lblmsgPopUp.Text = "Please Select a File to upload."
                Me.MPECommission_import.VisibleOnPageLoad = True
                ClassUpdatePnl.Update()
                Return
            End If

            If Not file_import.FileName Is Nothing Then

                Dim fileName As String = file_import.FileName
                Dim exten As String = System.IO.Path.GetExtension(fileName)
                Dim Str As New StringBuilder
                Dim TotSuccess As Integer = 0
                Dim TotFailed As Integer = 0
                Try
                    ViewState("FileType") = file_import.PostedFile.ContentType ' e.File.ContentType

                    If Not (exten.ToLower().Trim() = ".csv" Or exten.ToLower().Trim() = ".xls" Or exten.ToLower().Trim() = ".xlsx") Then
                        Me.MPECommission_import.VisibleOnPageLoad = False
                        MessageBoxValidation("Please upload excel file", "Validation")
                        ' ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)

                        Exit Sub

                    End If

                    If file_import.FileName.ToString.ToLower().Trim().EndsWith(".csv") Or file_import.FileName.ToString.ToLower().Trim().EndsWith(".xls") Or file_import.FileName.ToString.ToLower().Trim().EndsWith(".xlsx") Then

                        Dim Foldername As String = System.Configuration.ConfigurationManager.AppSettings("TemporaryFolder")
                        ' Dim Foldername As String = ExcelFileUpload.TemporaryFolder
                        If Not Foldername.EndsWith("\") Then
                            Foldername = Foldername & "\"
                        End If
                        If Directory.Exists(Foldername.Substring(0, Len(Foldername) - 1)) = False Then
                            Directory.CreateDirectory(Foldername.Substring(0, Len(Foldername) - 1))
                        End If
                        If file_import.FileName.ToString.ToLower().Trim().EndsWith(".csv") Then
                            Dim FName As String
                            FName = Now.Hour & Now.Minute & Now.Second & file_import.FileName
                            ViewState("FileName") = Foldername & FName
                            ViewState("CSVName") = FName
                        Else
                            ViewState("FileName") = Foldername & Now.Hour & Now.Minute & Now.Second & file_import.FileName
                        End If

                        file_import.SaveAs(ViewState("FileName"))

                        Try

                            If ViewState("FileType") IsNot Nothing And ViewState("FileName") IsNot Nothing Then
                                Dim TempTbl As New DataTable
                                If TempTbl.Rows.Count > 0 Then
                                    TempTbl.Rows.Clear()
                                End If
                                Dim col As DataColumn





                                col = New DataColumn
                                col.ColumnName = "Organization_ID"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)

                                col = New DataColumn
                                col.ColumnName = "Classification"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)

                                col = New DataColumn
                                col.ColumnName = "Commission"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)

                                col = New DataColumn
                                col.ColumnName = "UOM"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)

                                col = New DataColumn
                                col.ColumnName = "Incentive_Month"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)

                                col = New DataColumn
                                col.ColumnName = "Incentive_Year"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)




                                If ViewState("FileName").ToString.ToLower().Trim().EndsWith(".csv") Then
                                    TempTbl = DoCSVUpload()
                                ElseIf ViewState("FileName").ToString.ToLower().Trim().EndsWith(".xls") Then
                                    TempTbl = DoXLSUpload()
                                ElseIf ViewState("FileName").ToString.ToLower().Trim().EndsWith(".xlsx") Then
                                    TempTbl = DoXLSXUpload()
                                End If

                                If TempTbl Is Nothing Then

                                    Me.MPECommission_import.VisibleOnPageLoad = False
                                    MessageBoxValidation("Invalid file Template", "Validation")
                                    ' ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                                    Exit Sub
                                End If
                                
                                If TempTbl.Rows.Count = 0 Or TempTbl Is Nothing Then

                                    Me.MPECommission_import.VisibleOnPageLoad = False
                                    MessageBoxValidation("Invalid file Template", "Validation")
                                    ' ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                                    Exit Sub
                                End If

                                '   If TempTbl.Columns.Count = 2 Then
                                If Not (TempTbl.Columns(0).ColumnName = "Organization_ID" Or TempTbl.Columns(1).ColumnName = "Classification" Or TempTbl.Columns(2).ColumnName = "Commission" Or TempTbl.Columns(3).ColumnName = "UOM" Or TempTbl.Columns(4).ColumnName = "Incentive_Month" Or TempTbl.Columns(5).ColumnName = "Incentive_Year") Then

                                    Me.MPECommission_import.VisibleOnPageLoad = False
                                    MessageBoxValidation("Please check the template columns are correct", "Validation")
                                    '  ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                                    Exit Sub

                                End If



                                If TempTbl.Rows.Count = 0 Then
                                    Me.MPECommission_import.VisibleOnPageLoad = False
                                    MessageBoxValidation("There is no data in the file.", "Validation")
                                    ' ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)

                                    Exit Sub
                                End If

                                dtErrors = Session("dtDsErrors")

                                If dtErrors.Rows.Count > 0 Then
                                    dtErrors.Rows.Clear()
                                End If
                                Dim RowNo As String = Nothing
                                Dim ErrorText As String = Nothing

                                If TempTbl.Rows.Count > 0 Then
                                    Dim idx As Integer

                                    Dim Organization_ID As String = Nothing
                                    Dim Classification As String = Nothing
                                    Dim Commission As String = Nothing
                                    Dim UOM As String = Nothing
                                    Dim Incentive_Month As String = Nothing
                                    Dim Incentive_Year As String = Nothing


                                    Dim isValidRow As Boolean = True
                                    For idx = 0 To TempTbl.Rows.Count - 1

                                        RowNo = Nothing
                                        ErrorText = Nothing
                                        isValidRow = True

                                        If TempTbl.Rows(idx)(0) Is DBNull.Value Then
                                            Continue For
                                        End If


                                        Organization_ID = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, "", TempTbl.Rows(idx)(0).ToString())

                                        Classification = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, "", TempTbl.Rows(idx)(1).ToString())

                                        Commission = IIf(TempTbl.Rows(idx)(2) Is DBNull.Value, "0", TempTbl.Rows(idx)(2).ToString())
                                        UOM = IIf(TempTbl.Rows(idx)(3) Is DBNull.Value, "", TempTbl.Rows(idx)(3).ToString())

                                        Incentive_Month = IIf(TempTbl.Rows(idx)(4) Is DBNull.Value, "0", TempTbl.Rows(idx)(4).ToString())

                                        Incentive_Year = IIf(TempTbl.Rows(idx)(5) Is DBNull.Value, "0", TempTbl.Rows(idx)(5).ToString())




                                        If Organization_ID = "" Or Organization_ID Is Nothing Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + " Organization is mandatory" + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End If
                                        If Not (Organization_ID Is Nothing) Then
                                            If objProduct.IsValidOrganization(Organization_ID) = False Then
                                                RowNo = idx + 2
                                                ErrorText = ErrorText + "Invalid Organization" + ","
                                                isValidRow = False
                                                TotFailed += 1
                                            End If
                                        End If

                                        If Classification = "" Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "Classification should not be empty " + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End If

                                        If Classification <> "" Then
                                            If objIncentive.IsValidClassification(Err_No, Err_Desc, Organization_ID, Classification) = False Then
                                                RowNo = idx + 2
                                                ErrorText = ErrorText + " Invalid Classification " + ","
                                                isValidRow = False
                                                TotFailed += 1
                                            End If
                                        End If




                                        If UOM = "" Or UOM Is Nothing Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + " UOM is mandatory" + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End If
                                        If Not (UOM Is Nothing) Then
                                            If objIncentive.IsValidUOM(Err_No, Err_Desc, Organization_ID, Classification, UOM) = False Then
                                                RowNo = idx + 2
                                                ErrorText = ErrorText + "Invalid UOM" + ","
                                                isValidRow = False
                                                TotFailed += 1
                                            End If
                                        End If


                                        If IsNumeric(Commission) = False Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "Commission  should be in numeric " + ","
                                            isValidRow = False
                                            TotFailed += 1


                                        End If


                                        If IsNumeric(Commission) Then

                                            Try
                                                If Convert.ToDecimal(Commission) > 10000 Then
                                                    RowNo = idx + 2
                                                    ErrorText = ErrorText + "Invalid Commission " + ","
                                                    isValidRow = False
                                                    TotFailed += 1
                                                End If
                                            Catch ex As Exception
                                                RowNo = idx + 2
                                                ErrorText = ErrorText + ex.Message.ToString() + "Invalid Commission " + ","
                                                isValidRow = False
                                                TotFailed += 1
                                            End Try
                                        End If

                                        Try

                                        
                                        If IsNumeric(Incentive_Month) = False Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "Incentive Month  should be in integer " + ","
                                            isValidRow = False
                                            TotFailed += 1
                                            ElseIf Convert.ToInt32(Incentive_Month) > 0 And Convert.ToInt32(Incentive_Month) <= 12 Then
                                                RowNo = idx + 2
                                                ErrorText = ErrorText + " Invalid Incentive Month data " + ","
                                                isValidRow = False
                                                TotFailed += 1

                                        End If
                                        Catch ex As Exception
                                            log.Error(ErrorText)
                                            log.Error("16")
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + " Invalid Incentive Month data " + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End Try

                                        If IsNumeric(Incentive_Year) = False Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "Incentive Year  should be in integer " + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End If

                                        If IsNumeric(Incentive_Month) And IsNumeric(Incentive_Year) Then

                                            If Incentive_Year.Length = 4 Or Incentive_Year.Length = 3 Then
                                                Dim chkint As Integer
                                                Dim flgint As Boolean = False
                                                Dim IncentiveMonth As Decimal
                                                IncentiveMonth = Incentive_Month
                                                If Integer.TryParse(chkint, IncentiveMonth) = False Then
                                                    RowNo = idx + 2
                                                    ErrorText = ErrorText + "Incentive_Month  should be in integer " + ","
                                                    isValidRow = False
                                                    TotFailed += 1
                                                    flgint = True
                                                End If
                                                Dim IncentiveYear As Decimal
                                                IncentiveYear = Incentive_Year
                                                If Integer.TryParse(chkint, IncentiveYear) = False Then
                                                    RowNo = idx + 2
                                                    ErrorText = ErrorText + "Incentive_Year  should be in integer " + ","
                                                    isValidRow = False
                                                    TotFailed += 1
                                                    flgint = True
                                                End If

                                                If flgint = False Then




                                                    If Convert.ToInt32(Incentive_Year) < DateTime.Now.Year Then
                                                        RowNo = idx + 2
                                                        ErrorText = ErrorText + "Cant edit previous Commission " + ","
                                                        isValidRow = False
                                                        TotFailed += 1
                                                    End If


                                                    If DateTime.Now.Year = Convert.ToInt32(Incentive_Year) Then
                                                        If DateTime.Now.Month > Convert.ToInt32(Incentive_Month) Then
                                                            RowNo = idx + 2
                                                            ErrorText = ErrorText + "Cant edit previous Commission " + ","
                                                            isValidRow = False
                                                            TotFailed += 1
                                                        End If

                                                    End If
                                                End If

                                            Else
                                                RowNo = idx + 2
                                                ErrorText = ErrorText + "Invalid Incentive Year " + ","
                                                isValidRow = False
                                                TotFailed += 1
                                            End If
                                        Else
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "Invalid Incentive Month/Incentive Year  " + ","
                                            isValidRow = False
                                            TotFailed += 1



                                        End If

                                     

                                        'If ErrorText.Trim() <> "" Then
                                        '    If ErrorText.Trim().Contains(",") Then
                                        '        ErrorText = ErrorText.Remove(ErrorText.LastIndexOf(","))
                                        '    End If

                                        'End If




                                        If Not (RowNo Is Nothing And ErrorText Is Nothing) Then
                                            Dim h As DataRow = dtErrors.NewRow()
                                            h("RowNo") = RowNo

                                            h("LogInfo") = ErrorText
                                            dtErrors.Rows.Add(h)
                                            RowNo = Nothing
                                            ErrorText = Nothing
                                            isValidRow = False
                                        End If






                                        If isValidRow = True Then

                                            Dim dt_exist As DataTable
                                            Dim ROW_ID As String

                                            dt_exist = objIncentive.CheckIncentive_CommissionExist(Err_No, Err_Desc, Organization_ID, Classification, Incentive_Month, Incentive_Year)
                                            If dt_exist.Rows.Count > 0 Then
                                                ROW_ID = dt_exist.Rows(0)("ROW_ID").ToString()
                                                If objIncentive.ManageIncentive_Commission(Err_No, Err_Desc, ROW_ID, Organization_ID, Classification, Commission, UOM, Incentive_Month, Incentive_Year, CType(Session("User_Access"), UserAccess).UserID) Then
                                                    TotSuccess = TotSuccess + 1
                                                    Dim h As DataRow = dtErrors.NewRow()
                                                    h("RowNo") = idx + 2
                                                    h("LogInfo") = "Successfully uploaded"
                                                    dtErrors.Rows.Add(h)
                                                    RowNo = Nothing
                                                    ErrorText = Nothing
                                                    isValidRow = True
                                                Else
                                                    Dim h As DataRow = dtErrors.NewRow()
                                                    h("RowNo") = idx + 2
                                                    h("LogInfo") = "Error occured while saving this row"
                                                    dtErrors.Rows.Add(h)
                                                    RowNo = Nothing
                                                    ErrorText = Nothing
                                                    isValidRow = True


                                                End If

                                            Else

                                                If objIncentive.ManageIncentive_Commission(Err_No, Err_Desc, "0", Organization_ID, Classification, Commission, UOM, Incentive_Month, Incentive_Year, CType(Session("User_Access"), UserAccess).UserID) Then
                                                    TotSuccess = TotSuccess + 1
                                                    Dim h As DataRow = dtErrors.NewRow()
                                                    h("RowNo") = idx + 2
                                                    h("LogInfo") = "Successfully uploaded"
                                                    dtErrors.Rows.Add(h)
                                                    RowNo = Nothing
                                                    ErrorText = Nothing
                                                    isValidRow = True
                                                Else
                                                    Dim h As DataRow = dtErrors.NewRow()
                                                    h("RowNo") = idx + 2
                                                    h("LogInfo") = "Error occured while saving this row"
                                                    dtErrors.Rows.Add(h)
                                                    RowNo = Nothing
                                                    ErrorText = Nothing
                                                    isValidRow = True

                                                End If

                                            End If

                                        End If

                                    Next
                                End If
                                Resetfields()




                                Session.Remove("dtDsErrors")
                                Session("dtDsErrors") = dtErrors

                                Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & "IncentiveCommissionLog" & Now.ToString("yyyyMMdd") + ".txt"

                                DataTable2CSV(dtErrors, fn, vbTab)

                                Session.Remove("IncentiveCommissionLogFile")
                                Session("IncentiveCommissionLogFile") = fn



                                If TotSuccess > 0 Then
                                    DeleteExcel()
                                    ' Dt = objIncentive.GetIncentiveCommission(Err_No, Err_Desc, ddl_orgimport.SelectedValue, Me.ddFilterBy.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text))
                                    ' BindCommission()


                                    ' BindItemUOMData()
                                    Dim lblinfo As String = "Information"
                                    Dim lblMessage As String = IIf(TotFailed = 0, "Successfully uploaded", "One or more rows has invalid data. Please check the log")
                                    Me.MPECommission_import.VisibleOnPageLoad = False
                                    MessageBoxValidation(lblMessage, lblinfo)
                                    'ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)

                                    Exit Sub
                                Else
                                    Me.MPECommission_import.VisibleOnPageLoad = False
                                    MessageBoxValidation("Please check the uploaded log file", "Validation")
                                    ' ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                                    Exit Sub
                                End If
                            End If

                        Catch ex As Exception
                            log.Error((ex.Message.ToString()))
                            Err_No = "92823"
                            If Err_Desc Is Nothing Then
                                log.Error(GetExceptionInfo(ex))
                            Else
                                log.Error(Err_Desc)
                            End If
                        End Try


                    Else
                        Str.Append("<span style='font-color:red'> <b >Please import valid Excel template.</b></span>")
                        MessageBoxValidation(Str.ToString(), "Validation")
                        '  ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                    End If

                Catch ex As Exception
                    log.Error(GetExceptionInfo(ex))
                    log.Error((ex.Message.ToString()))
                End Try
            End If


        Catch ex As Exception
            MessageBoxValidation("Error occured while uploading file", "Validation")
            log.Error((ex.Message.ToString()))
        End Try







        '' '' ''lblmsgPopUp.Text = RetMessage
        '' '' ''BindGridError()
        '' '' ''ddl_org.ClearSelection()
        '' '' ''If Not ddl_org.FindItemByValue(ddl_Organization.SelectedItem.Value) Is Nothing Then
        '' '' ''    ddl_org.FindItemByValue(ddl_Organization.SelectedItem.Value).Selected = True



        '' '' ''    Dim objUserAccess As UserAccess
        '' '' ''    objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        '' '' ''    Dim objCommon As New Common
        '' '' ''    ddlVan.DataSource = objCommon.GetVanByOrg(Err_No, Err_Desc, ddl_Organization.SelectedItem.Value, objUserAccess.UserID)
        '' '' ''    ddlVan.DataValueField = "SalesRep_ID"
        '' '' ''    ddlVan.DataTextField = "SalesRep_Name"
        '' '' ''    ddlVan.DataBind()
        '' '' ''    ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))
        '' '' ''    objCommon = Nothing

        '' '' ''End If

        '' '' ''BindGrid("")
        '' '' ''UpdatePanel2.Update()
        '' '' ''MPEDivConfig.Show()
        '' '' ''ClassUpdatePnl.Update()
        ''End If
        'End If
        'Catch ex As Exception
        '    log.Error(GetExceptionInfo(ex))
        '    log.Error(ex.ToString())
        '' End Try
    End Sub
    Private Function DoCSVUpload() As DataTable
        Dim dtImport As New DataTable
        Try
            Dim strConString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "\;Extended Properties=""text;HDR=Yes;FMT=Delimited"""
            Dim oledbConn As New OleDbConnection(strConString)

            Dim cmd As New OleDbCommand("SELECT * FROM [" & ViewState("CSVName") & "]", oledbConn)

            Dim oleda As New OleDbDataAdapter()

            oleda.SelectCommand = cmd



            oleda.Fill(dtImport)
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
            Throw ex
        End Try
        Return dtImport
    End Function
    Private Function DoXLSUpload() As DataTable
        Dim dtImport As New DataTable
        Try
            Dim connString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & ViewState("FileName") & " ;Extended Properties=""Excel 8.0;HDR=Yes;IMEX=1"""
            Dim oledbConn As New OleDbConnection(connString)
            Try
                oledbConn.Open()

                Dim cmd As New OleDbCommand("SELECT * FROM [IncentiveCommission$] ", oledbConn)

                Dim oleda As New OleDbDataAdapter()

                oleda.SelectCommand = cmd



                oleda.Fill(dtImport)



            Catch ex As Exception
                log.Error(GetExceptionInfo(ex))
                log.Error(ex.Message.ToString())
            Finally
                ' Close connection
                oledbConn.Close()
            End Try
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
            Throw ex
        End Try
        Return dtImport
    End Function
    Private Function DoXLSXUpload() As DataTable
        Dim dtImport As New DataTable
        Try
            Dim connString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & ViewState("FileName") & " ;Extended Properties=""Excel 12.0;HDR=Yes;IMEX=1"""
            Dim oledbConn As New OleDbConnection(connString)
            Try
                oledbConn.Open()

                Dim cmd As New OleDbCommand("SELECT * FROM [IncentiveCommission$] ", oledbConn)

                Dim oleda As New OleDbDataAdapter()

                oleda.SelectCommand = cmd



                oleda.Fill(dtImport)



            Catch ex As Exception
                log.Error(GetExceptionInfo(ex))
                log.Error(ex.Message.ToString())
            Finally
                ' Close connection
                oledbConn.Close()
            End Try
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
            Throw ex
        End Try
        Return dtImport
    End Function
    Private Function SetErrorsTable() As DataTable
        Dim col As DataColumn
        'Dim dtErrors As New DataTable

        col = New DataColumn()
        col.ColumnName = "RowNo"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtErrors.Columns.Add(col)

        col = New DataColumn()
        col.ColumnName = "LogInfo"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtErrors.Columns.Add(col)

        Session.Remove("dtDsErrors")
        Session("dtDsErrors") = dtErrors
        Return dtErrors
    End Function
    Sub DataTable2CSV(ByVal table As DataTable, ByVal filename As String, ByVal sepChar As String)
        Dim writer As System.IO.StreamWriter
        Try
            writer = New System.IO.StreamWriter(filename)

            ' first write a line with the columns name
            Dim sep As String = ""
            Dim builder As New System.Text.StringBuilder
            For Each col As DataColumn In table.Columns
                builder.Append(sep).Append(col.ColumnName)
                sep = sepChar
            Next
            writer.WriteLine(builder.ToString())

            ' then write all the rows
            For Each row As DataRow In table.Rows
                sep = ""
                builder = New System.Text.StringBuilder

                For Each col As DataColumn In table.Columns
                    builder.Append(sep).Append(row(col.ColumnName))
                    sep = sepChar
                Next
                writer.WriteLine(builder.ToString())
            Next
        Finally
            If Not writer Is Nothing Then writer.Close()
        End Try
    End Sub
    Private Sub DeleteExcel()
        Try
            Dim TheFile As FileInfo = New FileInfo(ViewState("FileName"))
            If TheFile.Exists Then
                File.Delete(ViewState("FileName"))
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error((ex.Message.ToString()))
        End Try
    End Sub

    Protected Sub btnExport_Click(sender As Object, e As EventArgs)
        Try

        
        Dim finalDataSet As New DataSet
        Dim dtCommission As New DataTable()


        Dim m As Integer
        Dim y As Integer
        If monthpicker_F.SelectedDate Is Nothing Then
            m = 0
            y = 0
        Else
            m = monthpicker_F.SelectedDate.Value.Month
            y = monthpicker_F.SelectedDate.Value.Year
        End If
        dtCommission = objIncentive.LoadExportIncentiveCommission(Err_No, Err_Desc, ddlorg_F.SelectedValue, m, y, Me.ddFilterBy.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text))
        dtCommission.TableName = "IncentiveCommission"
        finalDataSet.Tables.Add(dtCommission)
        Dim fn As String = "IncentiveCommission" + DateTime.Now.ToString("hhmmss") + ".xls"
        Dim d As New DataSet


        If finalDataSet.Tables.Count <= 0 Then
            MessageBoxValidation("There is no data to export", "Information")
            Exit Sub
        Else
            ExportToExcel(fn, finalDataSet)
            End If
        Catch ex As Exception
            log.Error((ex.Message.ToString()))
        End Try
    End Sub
    Private Sub ExportToExcel(ByVal strFileName As String, ByVal ds As DataSet)

        Try

       
        Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & strFileName
        WriteXLSFile(fn, ds)

        Dim sFileName As String = strFileName
        Dim sFullPath As String = fn
        Dim fileBytes As Byte() = System.IO.File.ReadAllBytes(sFullPath)

        Dim context As System.Web.HttpContext = System.Web.HttpContext.Current
        context.Response.ContentType = "application/vnd.ms-excel"
        context.Response.AddHeader("Content-Disposition", "attachment;filename=" + sFileName)
        context.Response.Clear()
        context.Response.BinaryWrite(fileBytes)
        context.Response.Flush()
        context.ApplicationInstance.CompleteRequest()
        Catch ex As Exception
            log.Error((ex.Message.ToString()))
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub
    Public Function WriteXLSFile(ByVal pFileName As String, ByVal pDataSet As DataSet) As Boolean
        Try
            'This function CreateWorkbook will cause xls file cannot be opened
            'normally when file size below 7 KB, see my work around below
            'ExcelLibrary.DataSetHelper.CreateWorkbook(pFileName, pDataSet)

            'Create a workbook instance
            Dim workbook As Workbook = New Workbook()
            Dim worksheet As Worksheet
            Dim iRow As Integer = 0
            Dim iCol As Integer = 0
            Dim sTemp As String = String.Empty
            Dim dTemp As Double = 0
            Dim iTemp As Integer = 0
            '  Dim dtTime As DateTime
            Dim count As Integer = 0
            Dim iTotalRows As Integer = 0
            Dim iSheetCount As Integer = 0

            'Read DataSet
            If Not pDataSet Is Nothing And pDataSet.Tables.Count > 0 Then

                'Traverse DataTable inside the DataSet
                For Each dt As DataTable In pDataSet.Tables


                    'Create a worksheet instance
                    iSheetCount = iSheetCount + 1

                    'worksheet = New Worksheet("Product")

                    worksheet = New Worksheet(dt.TableName)
                    iCol = 0
                    iRow = 0
                    'Write Table Header
                    For Each dc As DataColumn In dt.Columns
                        worksheet.Cells(iRow, iCol) = New Cell(dc.ColumnName)
                        iCol = iCol + 1
                    Next

                    'Write Table Body
                    iRow = 1
                    For Each dr As DataRow In dt.Rows
                        iCol = 0
                        For Each dc As DataColumn In dt.Columns
                            sTemp = dr(dc.ColumnName).ToString()
                            worksheet.Cells(iRow, iCol) = New Cell(sTemp)
                            iCol = iCol + 1
                        Next
                        iRow = iRow + 1
                    Next

                    'Attach worksheet to workbook
                    workbook.Worksheets.Add(worksheet)
                    iTotalRows = iTotalRows + iRow
                Next
            End If

            'Bug on Excel Library, min file size must be 7 Kb
            'thus we need to add empty row for safety
            If iTotalRows < 100 Then
                worksheet = New Worksheet("Sheet2")
                count = 1
                Do While count < 100
                    worksheet.Cells(count, 0) = New Cell(" ")
                    count = count + 1
                Loop
                workbook.Worksheets.Add(worksheet)
            End If

            workbook.Save(pFileName)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

   

    Protected Sub lbLog_Click(sender As Object, e As EventArgs)
        Try

       
        'Handles lbLog.Click
        If Not Session("IncentiveCommissionLogFile") Is Nothing Then
            Dim fileValue As String = Session("IncentiveCommissionLogFile")

            Dim file As System.IO.FileInfo = New FileInfo(fileValue)

            If file.Exists Then

                'Process.Start("notepad.exe", fileValue)
                Response.Clear()

                Response.AddHeader("Content-Disposition", "attachment; filename=" & file.Name)

                Response.AddHeader("Content-Length", file.Length.ToString())

                Response.WriteFile(file.FullName)

                Response.[End]()

            Else
                MessageBoxValidation("File does not exist", "Information")
                Exit Sub

            End If
        Else
            MessageBoxValidation("There is no log to view.", "Information")
            Exit Sub

            End If
        Catch ex As Exception
            log.Error((ex.Message.ToString()))
        End Try
    End Sub

    Protected Sub btnReset_Click(sender As Object, e As EventArgs)
        Try
            Resetfields()
            Me.ddlorg_F.SelectedIndex = 0
            Me.gvIncentiveCommission.Visible = False
            Me.ClassUpdatePnl.Update()
        Catch ex As Exception
            log.Error((ex.Message.ToString()))
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



    'Private Sub gvIncentiveTarget_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvIncentiveTarget.PageIndexChanging
    '    Try


    '        gvIncentiveTarget.PageIndex = e.NewPageIndex
    '        Dim m As Integer
    '        Dim y As Integer
    '        If monthpicker_F.SelectedDate Is Nothing Then
    '            m = 0
    '            y = 0
    '        Else
    '            m = monthpicker_F.SelectedDate.Value.Month
    '            y = monthpicker_F.SelectedDate.Value.Year
    '        End If
    '        Dt = objIncentive.GetIncentiveTarget(Err_No, Err_Desc, ddlorg_F.SelectedValue, m, y, Me.ddFilterBy.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text))
    '        BindTarget()


    '        Session("Target") = Dt
    '        Me.gvIncentiveTarget.DataSource = Dt
    '        Me.gvIncentiveTarget.DataBind()
    '    Catch ex As Exception
    '        If Err_Desc Is Nothing Then
    '            log.Error(GetExceptionInfo(ex))
    '        Else
    '            log.Error(Err_Desc)
    '        End If
    '    End Try
    'End Sub


    'Private Sub gvIncentiveTarget_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvIncentiveTarget.Sorting
    '    Try
    '        ViewState("SortField") = e.SortExpression
    '        SortDirection = "flip"
    '        Dim m As Integer
    '        Dim y As Integer
    '        If monthpicker_F.SelectedDate Is Nothing Then
    '            m = 0
    '            y = 0
    '        Else
    '            m = monthpicker_F.SelectedDate.Value.Month
    '            y = monthpicker_F.SelectedDate.Value.Year
    '        End If
    '        Dt = objIncentive.GetIncentiveTarget(Err_No, Err_Desc, ddlorg_F.SelectedValue, m, y, Me.ddFilterBy.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text))
    '        BindTarget()


    '        Me.gvIncentiveTarget.DataSource = Dt
    '        Me.gvIncentiveTarget.DataBind()

    '        Dim dv As New DataView(Dt)
    '        If ViewState("SortField") <> "" Then
    '            dv.Sort = (ViewState("SortField") & " ") + SortDirection
    '        End If
    '        gvIncentiveTarget.DataSource = dv
    '        gvIncentiveTarget.DataBind()
    '        Session.Remove("Target")
    '        Session("Target") = Dt



    '    Catch ex As Exception
    '        If Err_Desc Is Nothing Then
    '            log.Error(GetExceptionInfo(ex))
    '        Else
    '            log.Error(Err_Desc)
    '        End If
    '    End Try
    'End Sub

    Private Sub ddl_org_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddl_org.SelectedIndexChanged
        Try


             '' ''Dim Objrep As New SalesWorx.BO.Common.Reports()



             '' ''Dim dt As New DataTable

             '' ''If dt.Rows.Count > 0 Then
             '' ''    dt.Rows.Clear()
             '' ''End If

             '' ''dt = objIncentive.GetClassification(Err_No, Err_Desc, ddl_org.SelectedValue, e.Text)

            ' '' ''Dim ItemsPerRequest As Integer = 100 'Set the number of values to show
             '' '' '' Dim itemOffset As Integer = e.NumberOfItems
            ' '' ''  Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
            ' '' ''e.EndOfItems = endOffset = dt.Rows.Count

            ' '' ''Loop through the values to populate the combo box


             '' ''For Each row As DataRow In dt.Rows
             '' ''    Dim item As New RadComboBoxItem()
             '' ''    item.Text = row.Item("Classification").ToString
             '' ''    item.Value = row.Item("Classification").ToString
             '' ''    ddlClassification.Items.Add(item)
             '' ''    item.DataBind()
             '' ''Next row

            'For i <=0 dt.Rows.Count
            '    Dim item As New RadComboBoxItem()
            '    item.Text = dt.Rows(i).Item("Classification").ToString
            '    item.Value = dt.Rows(i).Item("Classification").ToString
            '    ddlClassification.Items.Add(item)
            '    item.DataBind()
            'Next
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub
End Class