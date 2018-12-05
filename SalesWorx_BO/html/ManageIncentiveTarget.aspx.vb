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

Partial Public Class ManageIncentiveTarget
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
                Resetfields()
                FillOrganization()

                Session.Remove("dtDsErrors")
                Session.Remove("IncentiveTargetLogFile")
                SetErrorsTable()
                

                ' Me.month_picker.MinDate = DateTime.Now.Date
                Dim s As Date = Convert.ToDateTime(DateTime.Now.Date.ToString("MMM") & DateTime.Now.Date.Year.ToString())
                Me.monthpicker_F.SelectedDate = s
               
               

                If ddlorg_F.SelectedIndex = 1 Then
                    BindTarget()
                    ClassUpdatePnl.Update()
                End If
             

            Else
                MPETarget.VisibleOnPageLoad = False
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
        Me.MPETarget_import.VisibleOnPageLoad = False
        Me.gvIncentiveTarget.Visible = True
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


            Dt = objIncentive.GetIncentiveTarget(Err_No, Err_Desc, ddlorg_F.SelectedValue, m, y, Me.ddFilterBy.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text))
            BindTarget()
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
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            btnUpdate.Visible = False
            btnSave.Visible = True
            Me.MPETarget_import.VisibleOnPageLoad = False

            Resetfields()
            GetWorkingdays()
            Me.lblPop.Text = ""
            If ddl_org.Items.Count = 2 Then
                ddl_org.SelectedIndex = 1
            End If
            ClassUpdatePnl.Update()
            Me.MPETarget.VisibleOnPageLoad = True
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If Me.txttarget.Text = "" Or Me.ddl_org.SelectedIndex <= 0 Or Me.ddl_ParameterCode.SelectedIndex <= 0 Or Me.ddl_empcode.SelectedIndex <= 0 Then
            Me.lblPop.Text = "Organization,Incentive Month,Parameter,Employee Name and Target are required."
            Me.MPETarget.VisibleOnPageLoad = True
            Exit Sub
        End If

        If month_picker.SelectedDate Is Nothing Then
            Me.lblPop.Text = "Incentive month are required."
            Me.MPETarget.VisibleOnPageLoad = True
            Exit Sub
        End If


        Dim success As Boolean = False
        Try
            If objIncentive.CheckIncentive_TargetExist(Err_No, Err_Desc, ddl_org.SelectedValue, ddl_empcode.SelectedValue, ddl_ParameterCode.SelectedValue, month_picker.SelectedDate.Value.Month, month_picker.SelectedDate.Value.Year) = False Then
                If objIncentive.ManageIncentive_Target(Err_No, Err_Desc, "0", ddl_org.SelectedValue, ddl_empcode.SelectedValue, ddl_ParameterCode.SelectedValue, txttarget.Text, month_picker.SelectedDate.Value.Month, month_picker.SelectedDate.Value.Year, "A", CType(Session("User_Access"), UserAccess).UserID) Then
                    MessageBoxValidation("Successfully saved", "Information")
                End If
            Else
                Me.lblPop.Text = "Target already exist."
                Me.MPETarget.VisibleOnPageLoad = True
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
            Dt = objIncentive.GetIncentiveTarget(Err_No, Err_Desc, ddlorg_F.SelectedValue, m, y, Me.ddFilterBy.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text))
            BindTarget()
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
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Try
            Resetfields()
            Me.MPETarget.VisibleOnPageLoad = False
            ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "HideRadWindow();", True)
            ClassUpdatePnl.Update()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub
    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            lblPop.Text = ""
            btnUpdate.Visible = True
            btnSave.Visible = False
            Resetfields()
            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
            Dim RowID As System.Web.UI.WebControls.Label = row.FindControl("lblRowID")
            HidVal.Value = RowID.Text

            Dim dt_slab As DataTable
            dt_slab = Session("Target")

            Dim foundRows() As DataRow
            foundRows = dt_slab.Select("Row_ID='" & RowID.Text & "'")


            If foundRows.Count > 0 Then

                If IIf(foundRows(0)("Incentive_Year") Is DBNull.Value, DateTime.Now.Year, foundRows(0)("Incentive_Year").ToString()) < DateTime.Now.Year Then
                    MessageBoxValidation("Cant edit previous target.", "Information")
                    Exit Sub
                End If

                If DateTime.Now.Year = IIf(foundRows(0)("Incentive_Year") Is DBNull.Value, DateTime.Now.Year, foundRows(0)("Incentive_Year").ToString()) Then
                    If DateTime.Now.Month > IIf(foundRows(0)("Incentive_month") Is DBNull.Value, DateTime.Now.Month, foundRows(0)("Incentive_month").ToString()) Then
                        MessageBoxValidation("Cant edit previous target.", "Information")
                        Exit Sub
                    End If

                End If

                Me.txttarget.Text = foundRows(0)("Target")
                Me.ddl_org.SelectedValue = IIf(foundRows(0)("Organization_ID") Is DBNull.Value, "", foundRows(0)("Organization_ID").ToString())
                LoadDDL()
                Me.ddl_ParameterCode.SelectedValue = IIf(foundRows(0)("Parameter_Code") Is DBNull.Value, "", foundRows(0)("Parameter_Code").ToString())
                Me.ddl_empcode.SelectedValue = IIf(foundRows(0)("Emp_Code") Is DBNull.Value, "", foundRows(0)("Emp_Code").ToString())
                Me.ddl_org.Enabled = False
                Me.ddl_ParameterCode.Enabled = False
                Me.ddl_empcode.Enabled = False


                Me.MPETarget.VisibleOnPageLoad = True
                Me.btnSave.Visible = False
                Dim s As Date = Convert.ToDateTime(IIf(foundRows(0)("Tmonth") Is DBNull.Value, "", foundRows(0)("Tmonth").ToString()) & IIf(foundRows(0)("Incentive_Year") Is DBNull.Value, DateTime.Now.Year, foundRows(0)("Incentive_Year").ToString()))
                Me.month_picker.MinDate = s
                Me.month_picker.SelectedDate = s

                Me.month_picker.Enabled = False
                Me.month_picker.EnableTyping = False

            End If
            Me.MPETarget.VisibleOnPageLoad = True
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
        If Me.txttarget.Text = "" Or Me.ddl_org.SelectedIndex <= 0 Or Me.ddl_ParameterCode.SelectedIndex <= 0 Or Me.ddl_empcode.SelectedIndex <= 0 Then
            Me.lblPop.Text = "Organization,Incentive Month,Parameter,Employee Name and Target are required."
            Me.MPETarget.VisibleOnPageLoad = True
            Exit Sub
        End If
        Dim success As Boolean = False
        Try

            If objIncentive.ManageIncentive_Target(Err_No, Err_Desc, HidVal.Value, ddl_org.SelectedValue, ddl_empcode.SelectedValue, ddl_ParameterCode.SelectedValue, txttarget.Text, month_picker.SelectedDate.Value.Month, month_picker.SelectedDate.Value.Year, "U", CType(Session("User_Access"), UserAccess).UserID) Then
                ' Me.MPETarget.VisibleOnPageLoad = False
                MessageBoxValidation("Successfully updated", "Information")

            Else
                Me.lblPop.Text = "Error in updation ."
                Me.MPETarget.VisibleOnPageLoad = True
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

            Dt = objIncentive.GetIncentiveTarget(Err_No, Err_Desc, ddlorg_F.SelectedValue, m, y, Me.ddFilterBy.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text))
            BindTarget()
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
    Protected Sub lbChangeStatus_Target_Click(sender As Object, e As EventArgs)
        Try
            Dim btnChangeStatus As LinkButton = TryCast(sender, LinkButton)
            Dim row As GridViewRow = DirectCast(btnChangeStatus.NamingContainer, GridViewRow)
            Dim RowID As System.Web.UI.WebControls.Label = row.FindControl("lblROW_ID")
            Dim lblActive As System.Web.UI.WebControls.Label = row.FindControl("lblActive")
            Dim lblOrg As System.Web.UI.WebControls.Label = row.FindControl("lblOrg")
            Dim lblMonth As System.Web.UI.WebControls.Label = row.FindControl("lblMonth")
            Dim lblPcode As System.Web.UI.WebControls.Label = row.FindControl("lblPcode")
            Dim lblempcode As System.Web.UI.WebControls.Label = row.FindControl("lblempcode")
            HidVal.Value = RowID.Text



            Dim pcode As String = row.Cells(2).Text
            Dim emp As String = row.Cells(4).Text
            ' Dim pcode As String = row.Cells(2).Text

            If lblActive.Text <> "Y" Then
                If objIncentive.CheckIncentive_TargetExist(Err_No, Err_Desc, lblOrg.Text, lblempcode.Text, lblPcode.Text, lblMonth.Text, row.Cells(6).Text) Then
                    MessageBoxValidation("Target already exist.", "Information")
                    Exit Sub
                End If

            End If



            If objIncentive.UpdateIncentiveActive_Target(Err_No, Err_Desc, HidVal.Value, lblActive.Text, CType(Session("User_Access"), UserAccess).UserID) = True Then

                If lblActive.Text = "Y" Then
                    MessageBoxValidation("Target disabled Successfully", "Information")
                Else
                    MessageBoxValidation("Target enabled Successfully", "Information")
                End If
            Else

            End If

            Resetfields()
            Dim m As Integer
            Dim y As Integer
            If monthpicker_F.SelectedDate Is Nothing Then
                m = 0
                y = 0
            Else
                m = monthpicker_F.SelectedDate.Value.Month
                y = monthpicker_F.SelectedDate.Value.Year
            End If
            Dt = objIncentive.GetIncentiveTarget(Err_No, Err_Desc, ddlorg_F.SelectedValue, m, y, Me.ddFilterBy.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text))
            BindTarget()
            ClassUpdatePnl.Update()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
    Public Sub Resetfields()
        Try

       
            ' Me.txtFilterVal.Text = ""
            Me.ddl_org.SelectedIndex = 0
            Me.ddl_ParameterCode.SelectedIndex = 0
            Me.ddl_empcode.SelectedIndex = 0
            Me.btnSave.Text = "Save"
            Me.ddl_org.Enabled = True
            Me.ddl_ParameterCode.Enabled = True
            Me.ddl_empcode.Enabled = True
            Me.month_picker.Enabled = True
            Me.lbl_workingdays.Text = ""
            Me.month_picker.MinDate = DateTime.Now.Date
            Me.month_picker.SelectedDate = DateTime.Now.Date
            Me.txttarget.Text = ""
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub




    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Protected Sub ddFilterBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddFilterBy.SelectedIndexChanged
        Try
            If Me.ddFilterBy.SelectedIndex <= 0 Then
                Me.txtFilterVal.Text = ""
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

    Private Sub BindTarget()
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
            Dt = objIncentive.GetIncentiveTarget(Err_No, Err_Desc, ddlorg_F.SelectedValue, m, y, Me.ddFilterBy.SelectedItem.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text))
            Session("Target") = Dt
            Me.gvIncentiveTarget.DataSource = Dt
            Me.gvIncentiveTarget.DataBind()
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

            Try
                If ddlorg_F.Items.Count = 2 Then
                    ddlorg_F.SelectedIndex = 1
                End If

                If ddl_org.Items.Count = 2 Then
                    ddl_org.SelectedIndex = 1
                    LoadDDL()
                End If

            Catch ex As Exception
                log.Error(ex.Message.ToString())
            End Try
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try

    End Sub

    Protected Sub ddl_org_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs)

        If ddl_org.SelectedIndex > 0 Then
            LoadDDL()
        End If
    End Sub
    Sub LoadDDL()
        Try
            log.Error("3.2")
            log.Error("3.3" & ddl_org.SelectedValue.ToString())
            ddl_ParameterCode.DataSource = objIncentive.GetIncentiveParameters(Err_No, Err_Desc, ddl_org.SelectedValue)
            log.Error("3.4")
            ddl_ParameterCode.Items.Clear()
            log.Error("3.5")
            ddl_ParameterCode.Items.Add(New RadComboBoxItem("Select Parameter"))
            log.Error("3.6")
            ddl_ParameterCode.AppendDataBoundItems = True
            log.Error("3.7")
            ddl_ParameterCode.DataTextField = "Code_Description"
            log.Error("3.8")
            ddl_ParameterCode.DataValueField = "Code_Value"
            log.Error("3.9")
            ddl_ParameterCode.DataBind()
            log.Error("3.10")


            ddl_empcode.DataSource = objIncentive.GetEmpCode(Err_No, Err_Desc, ddl_org.SelectedValue)
            ddl_empcode.Items.Clear()
            ddl_empcode.Items.Add(New RadComboBoxItem("Select Emp"))
            ddl_empcode.AppendDataBoundItems = True
            ddl_empcode.DataTextField = "Emp_Name"
            ddl_empcode.DataValueField = "Emp_Code"
            ddl_empcode.DataBind()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub




    Protected Sub btn_Import_Click(sender As Object, e As EventArgs)
        Try
            btnUpdate.Visible = False
            btnSave.Visible = True
            Resetfields()

            Me.lblPop.Text = ""

            Me.MPETarget_import.VisibleOnPageLoad = True
            ClassUpdatePnl.Update()
            ' UpdatePanel2.Update()
            ' UpdatePanelF.Update()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub



    Protected Sub btn_importFile_Click(sender As Object, e As EventArgs)
        Try


            If Not file_import.HasFile Then
                lblmsgPopUp.Text = "Please Select a File to upload."
                Me.MPETarget_import.VisibleOnPageLoad = True
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
                        MessageBoxValidation("Please upload excel or csv file", "Validation")
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
                                col.ColumnName = "Emp_Code"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)


                                col = New DataColumn
                                col.ColumnName = "Parameter_Code"
                                col.DataType = System.Type.GetType("System.String")
                                col.ReadOnly = False
                                col.Unique = False
                                TempTbl.Columns.Add(col)

                                col = New DataColumn
                                col.ColumnName = "Target"
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

                                If TempTbl.Rows.Count = 0 Or TempTbl Is Nothing Then
                                    Me.MPETarget_import.VisibleOnPageLoad = False
                                    MessageBoxValidation("Invalid file Template", "Validation")
                                    ' ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                                    Exit Sub
                                End If

                                '   If TempTbl.Columns.Count = 2 Then
                                If Not (TempTbl.Columns(0).ColumnName = "Organization_ID" Or TempTbl.Columns(1).ColumnName = "Emp_Code" Or TempTbl.Columns(2).ColumnName = "Parameter_Code" Or TempTbl.Columns(3).ColumnName = "Target" Or TempTbl.Columns(4).ColumnName = "Incentive_Month" Or TempTbl.Columns(5).ColumnName = "Incentive_Year") Then
                                    Me.MPETarget_import.VisibleOnPageLoad = False
                                    MessageBoxValidation("Please check the template columns are correct", "Validation")
                                    '  ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                                    Exit Sub

                                End If



                                If TempTbl.Rows.Count = 0 Then
                                    Me.MPETarget_import.VisibleOnPageLoad = False
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
                                    Dim Emp_Code As String = Nothing
                                    Dim Parameter_Code As String = Nothing
                                    Dim Target As String = Nothing
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
                                        Emp_Code = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, "", TempTbl.Rows(idx)(1).ToString())
                                        Parameter_Code = IIf(TempTbl.Rows(idx)(2) Is DBNull.Value, "", TempTbl.Rows(idx)(2).ToString())
                                        Target = IIf(TempTbl.Rows(idx)(3) Is DBNull.Value, "0", TempTbl.Rows(idx)(3).ToString())
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

                                        If Emp_Code = "" Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "Emp Code should not be empty " + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End If

                                        If Emp_Code <> "" And Emp_Code <> "0" Then
                                            If objIncentive.IsValidEmpCode(Err_No, Err_Desc, Organization_ID, Emp_Code) = False Then
                                                RowNo = idx + 2
                                                ErrorText = ErrorText + " Invalid Emp Code " + ","
                                                isValidRow = False
                                                TotFailed += 1
                                            End If
                                        End If




                                        If Parameter_Code = "" Or Parameter_Code Is Nothing Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + " Parameter Code is mandatory" + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End If
                                        If Not (Parameter_Code Is Nothing) Then
                                            If objIncentive.IsParameter_Code(Err_No, Err_Desc, Organization_ID, Parameter_Code) = False Then
                                                RowNo = idx + 2
                                                ErrorText = ErrorText + "Invalid Parameter Code" + ","
                                                isValidRow = False
                                                TotFailed += 1
                                            End If
                                        End If


                                        If IsNumeric(Target) = False Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "Target  should be in numeric " + ","
                                            isValidRow = False
                                            TotFailed += 1
                                        End If


                                        If IsNumeric(Target) Then
                                            Try
                                                If Convert.ToDecimal(Target) > 999999999999 Then
                                                    RowNo = idx + 2
                                                    ErrorText = ErrorText + "Invalid Target " + ","
                                                    isValidRow = False
                                                    TotFailed += 1
                                                End If
                                            Catch ex As Exception
                                                RowNo = idx + 2
                                                ErrorText = ErrorText + ex.Message.ToString() + "Invalid Target " + ","
                                                isValidRow = False
                                                TotFailed += 1
                                            End Try
                                        End If




                                        Try

                                            If IsNumeric(Incentive_Month) = False Then
                                                RowNo = idx + 2
                                                ErrorText = ErrorText + "Incentive_Month  should be in integer " + ","
                                                isValidRow = False
                                                TotFailed += 1
                                            ElseIf Convert.ToInt32(Incentive_Month) > 0 Or Convert.ToInt32(Incentive_Month) <= 12 Then
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
                                                    ErrorText = ErrorText + "Incentive Month  should be in integer " + ","
                                                    isValidRow = False
                                                    TotFailed += 1
                                                    flgint = True
                                                End If
                                                Dim IncentiveYear As Decimal
                                                IncentiveYear = Incentive_Year
                                                If Integer.TryParse(chkint, IncentiveYear) = False Then
                                                    RowNo = idx + 2
                                                    ErrorText = ErrorText + "Incentive Year  should be in integer " + ","
                                                    isValidRow = False
                                                    TotFailed += 1
                                                    flgint = True
                                                End If

                                                If flgint = False Then



                                                    If Convert.ToInt32(Incentive_Year) < DateTime.Now.Year Then
                                                        RowNo = idx + 2
                                                        ErrorText = ErrorText + "Cant edit previous target " + ","
                                                        isValidRow = False
                                                        TotFailed += 1
                                                    End If


                                                    If DateTime.Now.Year = Convert.ToInt32(Incentive_Year) Then
                                                        If DateTime.Now.Month > Convert.ToInt32(Incentive_Month) Then
                                                            RowNo = idx + 2
                                                            ErrorText = ErrorText + "Cant edit previous target " + ","
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

                                            dt_exist = objIncentive.CheckIncentiveTargetExist(Err_No, Err_Desc, Organization_ID, Emp_Code, Parameter_Code, Incentive_Month, Incentive_Year)
                                            If dt_exist.Rows.Count > 0 Then
                                                ROW_ID = dt_exist.Rows(0)("ROW_ID").ToString()
                                                If objIncentive.ManageIncentive_Target(Err_No, Err_Desc, ROW_ID, Organization_ID, Emp_Code, Parameter_Code, Target, Incentive_Month, Incentive_Year, "U", CType(Session("User_Access"), UserAccess).UserID) Then
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

                                                If objIncentive.ManageIncentive_Target(Err_No, Err_Desc, "0", Organization_ID, Emp_Code, Parameter_Code, Target, Incentive_Month, Incentive_Year, "A", CType(Session("User_Access"), UserAccess).UserID) Then
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

                                Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & "IncentiveTargetLog" & Now.ToString("yyyyMMdd") + ".txt"

                                DataTable2CSV(dtErrors, fn, vbTab)

                                Session.Remove("IncentiveTargetLogFile")
                                Session("IncentiveTargetLogFile") = fn



                                If TotSuccess > 0 Then
                                    DeleteExcel()
                                    ' Dt = objIncentive.GetIncentiveTarget(Err_No, Err_Desc, ddl_orgimport.SelectedValue, Me.ddFilterBy.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text))
                                    ' BindTarget()


                                    ' BindItemUOMData()
                                    Dim lblinfo As String = "Information"
                                    Dim lblMessage As String = IIf(TotFailed = 0, "Successfully uploaded", "One or more rows has invalid data. Please check the log")
                                    Me.MPETarget_import.VisibleOnPageLoad = False
                                    MessageBoxValidation(lblMessage, lblinfo)
                                    ' ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                                    Exit Sub
                                Else
                                    Me.MPETarget_import.VisibleOnPageLoad = False
                                    MessageBoxValidation("Please check the uploaded log file", "Validation")
                                    ' ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                                    Exit Sub
                                End If
                            End If

                        Catch ex As Exception
                            log.Error(ex.Message.ToString())
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
                    log.Error(ex.Message.ToString())
                    log.Error(GetExceptionInfo(ex))
                End Try
            End If


        Catch ex As Exception
            MessageBoxValidation("Error occured while uploading file", "Validation")
            log.Error(ex.Message.ToString())
        End Try

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
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
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

                Dim cmd As New OleDbCommand("SELECT * FROM [IncentiveTarget$]", oledbConn)

                Dim oleda As New OleDbDataAdapter()

                oleda.SelectCommand = cmd



                oleda.Fill(dtImport)



            Catch ex As Exception
                log.Error(ex.Message.ToString())
                log.Error(GetExceptionInfo(ex))
            Finally
                ' Close connection
                oledbConn.Close()
            End Try
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
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

                Dim cmd As New OleDbCommand("SELECT * FROM [IncentiveTarget$] ", oledbConn)

                Dim oleda As New OleDbDataAdapter()

                oleda.SelectCommand = cmd



                oleda.Fill(dtImport)



            Catch ex As Exception
                log.Error(ex.Message.ToString())
                log.Error(GetExceptionInfo(ex))
            Finally
                ' Close connection
                oledbConn.Close()
            End Try
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
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
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Protected Sub btnExport_Click(sender As Object, e As EventArgs)

        Dim finalDataSet As New DataSet
        Dim dtTarget As New DataTable()
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
            dtTarget = objIncentive.LoadExportIncentiveTarget(Err_No, Err_Desc, ddlorg_F.SelectedValue, m, y, Me.ddFilterBy.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text))
            dtTarget.TableName = "IncentiveTarget"
            finalDataSet.Tables.Add(dtTarget)
            Dim fn As String = "IncentiveTarget" + DateTime.Now.ToString("hhmmss") + ".xls"
            Dim d As New DataSet


            If finalDataSet.Tables.Count <= 0 Then
                MessageBoxValidation("There is no data to export", "Information")
                Exit Sub
            Else
                ExportToExcel(fn, finalDataSet)
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
            log.Error(ex.Message.ToString())
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
    Function GetWorkingdays() As Integer

        Try
            Dim Total_workingDays As Integer = 0
            Dim f_date As DateTime
            f_date = month_picker.SelectedDate ' New DateTime(month_picker.SelectedDate.Value.Year, month_picker.SelectedDate.Value.Month, 1)
            Dim Sdate As DateTime
            Sdate = DateAdd(DateInterval.Day, -1 * (f_date.Day - 1), f_date) ' 'DateAdd(DateInterval.Day, -1 * (f_date.Day - 1), f_date).ToString("MM-dd-yyyy").to
            Dim Edate As DateTime
            Edate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, Sdate))
            Dim CurrD As DateTime = Sdate

            While (CurrD <= Edate)


                If (IsOffDay(CurrD) = False) Then
                    Total_workingDays = Total_workingDays + 1
                End If
                CurrD = CurrD.AddDays(1)
            End While


            lbl_workingdays.Text = "Total Number of Working Days : " & Total_workingDays.ToString()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Function
       
  
    Protected Sub month_picker_SelectedDateChanged(sender As Object, e As Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs)
        GetWorkingdays()
    End Sub

    Function IsOffDay(ByVal PDate As Date) As Boolean
        Dim OffDays As String
        Dim bRetval As Boolean = False
        Try
            OffDays = (New SalesWorx.BO.Common.RoutePlan).GetOffDays().ToUpper()
            Dim days() As String
            days = OffDays.Split(",")
            For i As Integer = 0 To days.Length - 1
                If PDate.ToString("ddd").ToUpper = days(i) Then
                    bRetval = True
                    Exit For
                End If
            Next

        Catch ex As Exception
            log.Error(ex.Message.ToString())
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
        Return bRetval
    End Function

    Protected Sub lbLog_Click(sender As Object, e As EventArgs)
        'Handles lbLog.Click
        If Not Session("IncentiveTargetLogFile") Is Nothing Then
            Dim fileValue As String = Session("IncentiveTargetLogFile")

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


    End Sub

    Protected Sub btn_CancelImport_Click(sender As Object, e As EventArgs)
        Try
            Resetfields()
            Me.MPETarget_import.VisibleOnPageLoad = False
            ClassUpdatePnl.Update()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
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
            Me.gvIncentiveTarget.Visible = False
            Dim s As Date = Convert.ToDateTime(DateTime.Now.Date.ToString("MMM") & DateTime.Now.Date.Year.ToString())
            Me.monthpicker_F.SelectedDate = s
            Me.ClassUpdatePnl.Update()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
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



    Private Sub gvIncentiveTarget_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvIncentiveTarget.PageIndexChanging
        Try
            gvIncentiveTarget.PageIndex = e.NewPageIndex
        Dim m As Integer
        Dim y As Integer
        If monthpicker_F.SelectedDate Is Nothing Then
            m = 0
            y = 0
        Else
            m = monthpicker_F.SelectedDate.Value.Month
            y = monthpicker_F.SelectedDate.Value.Year
        End If
        Dt = objIncentive.GetIncentiveTarget(Err_No, Err_Desc, ddlorg_F.SelectedValue, m, y, Me.ddFilterBy.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text))
        BindTarget()


        Session("Target") = Dt
        Me.gvIncentiveTarget.DataSource = Dt
        Me.gvIncentiveTarget.DataBind()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub


    Private Sub gvIncentiveTarget_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvIncentiveTarget.Sorting
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
            Dt = objIncentive.GetIncentiveTarget(Err_No, Err_Desc, ddlorg_F.SelectedValue, m, y, Me.ddFilterBy.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text))
            BindTarget()


            Me.gvIncentiveTarget.DataSource = Dt
            Me.gvIncentiveTarget.DataBind()

            Dim dv As New DataView(Dt)
            If ViewState("SortField") <> "" Then
                dv.Sort = (ViewState("SortField") & " ") + SortDirection
            End If
            gvIncentiveTarget.DataSource = dv
            gvIncentiveTarget.DataBind()
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

 
End Class