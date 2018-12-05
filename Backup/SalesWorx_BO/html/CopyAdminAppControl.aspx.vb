Imports SalesWorx.BO.Common
Imports log4net
Imports Telerik.Web.UI

Partial Public Class CopyAdminAppControl
    Inherits System.Web.UI.Page

    Dim objControl As New SalesWorx.BO.Common.AppControl
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const ModuleName As String = "AdminAppCOntrol.aspx"
    Private Const PageID As String = "P56"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                If Session.Item("USER_ACCESS") Is Nothing Then
                    Session.Add("BringmeBackHere", ModuleName)
                    Response.Redirect("Login.aspx", False)
                    Exit Sub
                End If
                If Not IsPostBack Then
                    Dim HasPermission As Boolean = False
                    ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
                    If Not HasPermission Then
                        Err_No = 500
                        Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
                    End If
                    ' LoadAppControlParams()
                    'BindData()
                    LoadParamTypes()
                End If
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub LoadParamTypes()
        ddlFilterBy.DataSource = objControl.LoadAppParamType(Err_No, Err_Desc)
        ddlFilterBy.Items.Clear()
        ddlFilterBy.Items.Add("--Select--")
        ddlFilterBy.AppendDataBoundItems = True
        ddlFilterBy.DataValueField = "Code"
        ddlFilterBy.DataTextField = "Description"
        ddlFilterBy.DataBind()
        ddlFilterBy.SelectedIndex = 0
    End Sub
    Private Sub BindData()

        Dim ParentNode As String = Me.ddlFilterBy.SelectedValue
        Dim z As New DataTable


        z = objControl.CopyLoadAppControlParams(Err_No, Err_Desc, ParentNode)
        Me.gvParams.DataSource = z
        Me.gvParams.DataBind()
        ClassUpdatePnl.Update()
    End Sub

   

  
    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        Try

            If Me.ddlFilterBy.SelectedIndex <= 0 Then
                MessageBoxValidation("Please select a parameter type.")
                Exit Sub
            End If

            Dim success As Boolean = False

            For Each gvr As GridViewRow In gvParams.Rows

                Dim lblType As Label = DirectCast(gvr.FindControl("lblControlType"), Label)
                Dim ControlKey As String = gvParams.DataKeys(gvr.RowIndex).Value
                Dim ddList As DropDownList = DirectCast(gvr.FindControl("drpValue"), DropDownList)
                Dim txtValue As TextBox = DirectCast(gvr.FindControl("txtValue"), TextBox)
                Dim chkValue As CheckBox = DirectCast(gvr.FindControl("chkValue"), CheckBox)
                Dim chkMulti As RadComboBox = DirectCast(gvr.FindControl("chkMulti"), RadComboBox)
                Dim RTP As RadTimePicker = DirectCast(gvr.FindControl("RTP"), RadTimePicker)

                Dim ControlValue As String = "0"

                If lblType.Text = "Dropdown" Then
                    ControlValue = ddList.SelectedValue
                ElseIf lblType.Text = "TextBox" Then
                    ControlValue = txtValue.Text
                ElseIf lblType.Text = "CheckBox" Then
                    ControlValue = IIf(chkValue.Checked = True, "Y", "N")

                ElseIf lblType.Text = "TimePicker" Then
                    ControlValue = RTP.SelectedDate.Value.ToString("HH:mm")

                ElseIf lblType.Text = "MultiCheck" Then
                    Dim s As String = Nothing
                   

                    For Each itm As RadComboBoxItem In chkMulti.Items
                        If itm.Checked = True Then
                            s = s + itm.Value.ToString() + ","
                        End If
                    Next

                    If Not s Is Nothing Then
                        If s.Length > 0 Then
                            s = s.Remove(s.Length - 1, 1).Trim()
                        End If
                    End If

                    ControlValue = IIf(s Is Nothing, "0", s)

                End If

                If objControl.UpdateAppParams(Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, ControlKey, ControlValue) Then
                    success = True
                    ControlValue = "0"
                Else
                    success = False
                End If

            Next





            If success = True Then
                MessageBoxInfo("Successfully updated.")
                BindData()
            Else
                MessageBoxValidation("Error occured while updating control parameter.")
            End If

        Catch ex As Exception
            MessageBoxValidation("Error occured while updating application control parameters.")
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub


    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        Exit Sub
    End Sub
    Sub MessageBoxInfo(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Green
        lblinfo.Text = "Information"
        lblMessage.Text = str
        MpInfoError.Show()
        Exit Sub
    End Sub

  
    Protected Sub ddlFilterBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFilterBy.SelectedIndexChanged
        If Me.ddlFilterBy.SelectedIndex > 0 Then
            BindData()
        Else
            '  MessageBoxValidation("Please select a parameter type")
            Me.gvParams.DataSource = Nothing
            Me.gvParams.DataBind()
            ClassUpdatePnl.Update()
            Exit Sub
        End If
    End Sub

  

    'Protected Sub gvParams_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvParams.RowCreated
    '    If e.Row.RowType = DataControlRowType.DataRow Then

    '        Dim lblType As Label = DirectCast(e.Row.FindControl("lblControlType"), Label)
    '        Dim ControlKey As String = gvParams.DataKeys(e.Row.RowIndex).Value
    '        Dim ddList As DropDownList = DirectCast(e.Row.FindControl("drpValue"), DropDownList)
    '        If ddList.Visible = True Then

    '            'bind dropdownlist
    '            Dim dt As DataTable
    '            dt = objControl.LoadParamDropdown(Err_No, Err_Desc, ControlKey)
    '            ddList.DataSource = dt
    '            ddList.DataTextField = "Description"
    '            ddList.DataValueField = "Code"
    '            ddList.DataBind()

    '            Dim dr As DataRowView = TryCast(e.Row.DataItem, DataRowView)
    '            'ddList.SelectedItem.Text = dr["category_name"].ToString();
    '            ddList.SelectedValue = dr("txtValue").ToString()
    '        End If

    '    End If
    'End Sub
    Protected Sub RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblType As Label = DirectCast(e.Row.FindControl("lblControlType"), Label)
            Dim ControlKey As String = gvParams.DataKeys(e.Row.RowIndex).Value
            Dim ddList As DropDownList = DirectCast(e.Row.FindControl("drpValue"), DropDownList)
            Dim txtValue As TextBox = DirectCast(e.Row.FindControl("txtValue"), TextBox)
            Dim chkValue As CheckBox = DirectCast(e.Row.FindControl("chkValue"), CheckBox)
            Dim chkMulti As RadComboBox = DirectCast(e.Row.FindControl("chkMulti"), RadComboBox)
            Dim RTP As RadTimePicker = DirectCast(e.Row.FindControl("RTP"), RadTimePicker)
            If lblType.Text = "Dropdown" Then

                'bind dropdownlist
                Dim dt As DataTable
                dt = objControl.LoadParamDropdown(Err_No, Err_Desc, ControlKey)
                ddList.DataSource = dt
                ddList.DataTextField = "Description"
                ddList.DataValueField = "Code"
                ddList.DataBind()

                Dim dr As DataRowView = TryCast(e.Row.DataItem, DataRowView)
                ddList.SelectedValue = dr("ControlValue").ToString()

            End If
            If lblType.Text = "TextBox" Then
                Dim dr As DataRowView = TryCast(e.Row.DataItem, DataRowView)
                txtValue.Text = dr("ControlValue").ToString()
            End If

            If lblType.Text = "CheckBox" Then
                Dim dr As DataRowView = TryCast(e.Row.DataItem, DataRowView)
                chkValue.Checked = IIf(dr("ControlValue").ToString() = "Y", True, False)
            End If

            If lblType.Text = "MultiCheck" Then

                'bind dropdownlist
                Dim dt As DataTable
                dt = objControl.LoadParamDropdown(Err_No, Err_Desc, ControlKey)
                chkMulti.DataSource = dt
                chkMulti.DataTextField = "Description"
                chkMulti.DataValueField = "Code"
                chkMulti.DataBind()

                Dim dr As DataRowView = TryCast(e.Row.DataItem, DataRowView)
                Dim s() As String = dr("ControlValue").ToString().Split(",")
                If Not s Is Nothing Then
                    If s.Length > 0 Then
                        For i As Integer = 0 To s.Length - 1


                            For Each itm As RadComboBoxItem In chkMulti.Items
                                If itm.Value = s(i).ToString() Then
                                    itm.Checked = True
                                End If
                            Next
                        Next
                    End If

                End If

            End If
            If lblType.Text = "TimePicker" Then
                Dim dr As DataRowView = TryCast(e.Row.DataItem, DataRowView)
                '  Dim Time As String = dr("txtValue").ToString()
                RTP.SelectedDate = DateTime.Parse("1900-01-01" + " " + dr("ControlValue").ToString())
            End If
            If lblType.Text = "Dropdown" Then
                txtValue.Visible = False
                chkValue.Visible = False
                ddList.Visible = True
                chkMulti.Visible = False
                RTP.Visible = False
            ElseIf lblType.Text = "TextBox" Then
                txtValue.Visible = True
                chkValue.Visible = False
                ddList.Visible = False
                chkMulti.Visible = False
                RTP.Visible = False
            ElseIf lblType.Text = "CheckBox" Then
                txtValue.Visible = False
                chkValue.Visible = True
                ddList.Visible = False
                chkMulti.Visible = False
                RTP.Visible = False
            ElseIf lblType.Text = "MultiCheck" Then
                txtValue.Visible = False
                chkValue.Visible = False
                ddList.Visible = False
                chkMulti.Visible = True
                RTP.Visible = False
            ElseIf lblType.Text = "TimePicker" Then
                txtValue.Visible = False
                chkValue.Visible = False
                ddList.Visible = False
                chkMulti.Visible = False
                RTP.Visible = True
            End If
        End If
    End Sub
End Class