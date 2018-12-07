

Imports SalesWorx.BO.Common
Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports Telerik.Web.UI
Partial Public Class VanUserSyncSetting
    Inherits System.Web.UI.Page
    Dim objProduct As New Product
    Dim objCommon As New Common
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const PageID As String = "P261"

    Dim a As Int32 = 0
    Dim b As Int32 = 0
    Dim a1 As Int32 = 0
    Dim b1 As Int32 = 0


    Dim objLogin As New SalesWorx.BO.Common.Login
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub VanDeviceConfig_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Van Device Configuration"
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        Try
            If Not IsPostBack Then





                Dim HasPermission As Boolean = False
                ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
                If Not HasPermission Then
                    Err_No = 500
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
                End If







                Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddOraganisation.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddOraganisation.Items.Clear()
                ddOraganisation.Items.Add(New RadComboBoxItem("-- Select a Organization --"))
                ddOraganisation.AppendDataBoundItems = True
                ddOraganisation.DataValueField = "MAS_Org_ID"
                ddOraganisation.DataTextField = "Description"
                ddOraganisation.DataBind()


                ' Task1 Rakesh 
                Dim dt As New DataTable
                dt = objCommon.GetSyncTypeForSync(Err_No, Err_Desc)
                If dt.Rows.Count > 0 Then
                    If dt.Rows(0)("Control_Value").ToString().ToUpper().Trim() = "Y" Then
                        pagenote.Visible = False

                    Else

                        pagenote.Visible = False
                        gvConfig.Enabled = True





                    End If

                End If
                'end task 



            End If
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_MSL_005") & "&next=Welcome.aspx&Title=Product List Setup", False)
        End Try

    End Sub

    Protected Sub ddOraganisation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddOraganisation.SelectedIndexChanged
        'Try
        If ddOraganisation.SelectedItem.Text <> "-- Select a Organization --" Then
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

            ddlSalesRep.DataSource = objCommon.GetVanByOrg(Err_No, Err_Desc, ddOraganisation.SelectedValue, objUserAccess.UserID.ToString())
            ddlSalesRep.Items.Clear()
            ddlSalesRep.DataValueField = "SalesRep_Id"
            ddlSalesRep.DataTextField = "SalesRep_Name"
            ddlSalesRep.DataBind()
        Else
            ddlSalesRep.Items.Clear()

            ddlSalesRep.DataValueField = "SalesRep_Id"
            ddlSalesRep.DataTextField = "SalesRep_Name"
            ddlSalesRep.DataBind()

        End If
        BindData()
    End Sub
    Protected Sub gvConfig_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvConfig.PageIndexChanging
        gvConfig.PageIndex = e.NewPageIndex
        BindData()

    End Sub

    Protected Sub gvConfig_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvConfig.Sorting
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindData()
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
    Private Sub BindData()

        Dim TempTbl As New DataTable
        Try

            Dim van As String = ""
            Dim collection As IList(Of RadComboBoxItem) = ddlSalesRep.CheckedItems
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","


            Next
            If van = "" Then
                van = "0"
            End If

            '
            TempTbl = objCommon.GetUserSyncConfig(Err_No, Err_Desc, van)
            If (TempTbl.Rows.Count = 0) Then
                TempTbl = objCommon.GetDefaultSyncTypeForSync(Err_No, Err_Desc, van)
            End If


            If (lblsync.Text.Trim = "") Then

            Else
                If (lblsync.Text.Trim = "1") Then
                    For Each dr As DataRow In TempTbl.Rows

                        dr.Item("Config_Type") = "Y"


                    Next
                ElseIf (lblsync.Text.Trim = "0") Then

                    For Each dr As DataRow In TempTbl.Rows

                        dr.Item("Config_Type") = "N"


                    Next
                Else

                End If
            End If

            If (lbldefault.Text.Trim = "") Then

            Else
                If (lbldefault.Text.Trim = "1") Then
                    For Each dr As DataRow In TempTbl.Rows

                        dr.Item("Config_Value") = "Y"


                    Next
                ElseIf (lbldefault.Text.Trim = "0") Then

                    For Each dr As DataRow In TempTbl.Rows

                        dr.Item("Config_Value") = "N"


                    Next
                End If
            End If

            Dim dv As New DataView(TempTbl)
            If ViewState("SortField") <> "" Then
                dv.Sort = (ViewState("SortField") & " ") + SortDirection
            End If
            gvConfig.DataSource = dv
            gvConfig.DataBind()

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        Finally

        End Try
    End Sub




    Private Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Me.ddlSalesRep.ClearCheckedItems()
        Me.ddOraganisation.ClearCheckedItems()
        Me.ddOraganisation.SelectedValue = "0"

        a = 0
        b = 0
        a1 = 0
        b1 = 0
        lbldefault.Text = ""
        lblsync.Text = ""

        BindData()
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If Me.ddOraganisation.SelectedIndex <= 0 Or Me.ddlSalesRep.CheckedItems.Count <= 0 Then
            MessageBoxValidation("Please select a organization and van", "Validation")

            Exit Sub
        End If

        If gvConfig.Rows.Count <= 0 Then
            MessageBoxValidation("There is no data available to update", "Validation")

            Exit Sub
        End If

        Dim s As Boolean = False

        For Each gvr As GridViewRow In gvConfig.Rows
            Dim RowID As Label = DirectCast(gvr.FindControl("lblRowID"), Label)

            Dim ConfigVal_enableStatus As RadioButtonList = CType(gvr.FindControl("rdb_enable_sync"), RadioButtonList)
            Dim ConfigVal_DefaultStatus As RadioButtonList = CType(gvr.FindControl("rdb_defaultState"), RadioButtonList)

            ' Dim ConfigVal_enableStatus As RadButtonList = DirectCast(gvr.FindControl("rdb_enable_sync"), RadButtonList)
            ' Dim ConfigVal_DefaultStatus As RadButtonList = DirectCast(gvr.FindControl("rdb_defaultState"), RadButtonList)
            Dim SaleRepname As Label = DirectCast(gvr.FindControl("lblSalesRep_name"), Label)

            ' Dim SaleRepname As TextBox = DirectCast(gvr.FindControl("SalesRep_name"), TextBox)


            'If ConfigVal.SelectedValue = "Y" Then

            '    MessageBoxValidation("Please enter the config value at row no " + (gvr.RowIndex + 1).ToString(), "Validation")

            '    Exit Sub
            'End If
            If RowID Is Nothing Then
                RowID.Text = ""


            End If

            If objCommon.InsertUpdateSyncTable(Err_No, Err_Desc, RowID.Text, ConfigVal_enableStatus.SelectedValue, ConfigVal_DefaultStatus.SelectedValue, SaleRepname.Text, CType(Session("User_Access"), UserAccess).UserID) = True Then
                s = True
            End If
            a = 0
            b = 0
            a1 = 0
            b1 = 0
            lbldefault.Text = ""
            lblsync.Text = ""
        Next
        If s = True Then
            MessageBoxValidation("Successfully updated", "Information")

        End If
        BindData()
    End Sub

    Private Sub BtnSearch_Click(sender As Object, e As EventArgs) Handles BtnSearch.Click
        If Me.ddOraganisation.SelectedIndex <= 0 Or Me.ddlSalesRep.CheckedItems.Count <= 0 Then
            MessageBoxValidation("Please select a organization and van", "Validation")

            Exit Sub
        End If

        a = 0
        b = 0
        a1 = 0
        b1 = 0

        lbldefault.Text = ""
        lblsync.Text = ""
        BindData()
    End Sub

    Protected Sub rdb_Defaultsate_sync_SelectedIndexChanged(sender As Object, e As EventArgs)


        Dim rdb_main_enabl_sync As RadioButtonList = gvConfig.FindControl("rdb_Defaultsate_sync")

        If rdb_main_enabl_sync.SelectedValue().ToString() = "Y" Then
            a = 1
            BindData()
        ElseIf rdb_main_enabl_sync.SelectedValue().ToString() = "Y" Then
            b = 1
            BindData()
        Else

        End If




    End Sub

    Protected Sub rdb_main_enabl_sync_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim rdb_main_enabl_sync As RadioButtonList = gvConfig.FindControl("rdb_main_enabl_sync")

        If rdb_main_enabl_sync.SelectedValue().ToString() = "Y" Then
            a1 = 1
            BindData()
        ElseIf rdb_main_enabl_sync.SelectedValue().ToString() = "Y" Then
            b1 = 1
            BindData()
        Else

        End If
    End Sub

    Protected Sub rdb1_CheckedChanged(sender As Object, e As EventArgs)
        a1 = 1
        b1 = 0
        lblsync.Text = "1"
        BindData()

    End Sub

    Protected Sub rdb2_CheckedChanged(sender As Object, e As EventArgs)

        Session("b1") = "1"
        Session("a1") = "0"
        lblsync.Text = "0"
        b1 = 1
        a1 = 0
        BindData()
    End Sub

    Protected Sub rdb_DefaultsateYes_CheckedChanged(sender As Object, e As EventArgs)
        a = 1
        b = 0
        Session("a") = "1"
        Session("b") = "0"
        lbldefault.Text = "1"
        BindData()
    End Sub

    Protected Sub rdb_DefaultsateNo_CheckedChanged(sender As Object, e As EventArgs)
        a = 0
        b = 1
        Session("a") = "0"
        Session("b") = "1"
        lbldefault.Text = "0"

        BindData()
    End Sub

    Protected Sub gvConfig_RowDataBound(sender As Object, e As GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.Header Then

            Dim rowView As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim rdb_DefaultsateNo As System.Web.UI.WebControls.RadioButton = CType(e.Row.FindControl("rdb_DefaultsateNo"), System.Web.UI.WebControls.RadioButton)
            Dim rdb_DefaultsateYes As System.Web.UI.WebControls.RadioButton = CType(e.Row.FindControl("rdb_DefaultsateYes"), System.Web.UI.WebControls.RadioButton)
            Dim rdb1 As System.Web.UI.WebControls.RadioButton = CType(e.Row.FindControl("rdb1"), System.Web.UI.WebControls.RadioButton)
            Dim rdb2 As System.Web.UI.WebControls.RadioButton = CType(e.Row.FindControl("rdb2"), System.Web.UI.WebControls.RadioButton)




            ' test 
            If (lblsync.Text = "1") Then
                rdb1.Checked = True
                rdb2.Checked = False
            ElseIf (lblsync.Text = "0") Then
                rdb1.Checked = False
                rdb2.Checked = True
            Else

                rdb1.Checked = False
                rdb2.Checked = False
            End If

            ' lblsync

            If (lbldefault.Text = "1") Then
                rdb_DefaultsateNo.Checked = False
                rdb_DefaultsateYes.Checked = True
            ElseIf (lbldefault.Text = "0") Then
                rdb_DefaultsateNo.Checked = True
                rdb_DefaultsateYes.Checked = False
            Else
                rdb_DefaultsateNo.Checked = False
                rdb_DefaultsateYes.Checked = False

            End If





        End If

    End Sub
End Class