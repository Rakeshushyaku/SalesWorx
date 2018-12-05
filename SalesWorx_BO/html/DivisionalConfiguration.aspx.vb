Imports SalesWorx.BO.Common
Imports log4net
Imports Telerik.Web.UI
Partial Public Class DivisionalConfiguration
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim dt As New DataTable
    Dim objDivConfig As New DivConfig
    Dim objLogin As New SalesWorx.BO.Common.Login
    ' Dim SortField As String = ""
    Private Const ModuleName As String = "DivisionalConfiguration.aspx"
    Private Const PageID As String = "P92"
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
            FillDivisions()
            BindGrid("0")
            ViewState("Criteria") = "0"
        Else
            MPEDetails.VisibleOnPageLoad = False
        End If

        lblPop.Text = ""
    End Sub
    Private Function HasAuthentication() As Boolean
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
    End Function

    Sub FillDivisions()
        dt = New DataTable()
        Dim objCommon As New Common
        Dim ObjProduct As New Product
        Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
        dt = ObjProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
        drpOrganization.DataSource = dt
        Dim dr As DataRow
        dr = dt.NewRow()
        dr(0) = "0"
        dr(1) = "-- Select a Organization --"
        dt.Rows.InsertAt(dr, 0)
        drpOrganization.DataValueField = "MAS_Org_ID"
        drpOrganization.DataTextField = "Description"
        drpOrganization.DataBind()

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

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        btnUpdate.Visible = False
        btnSave.Visible = True
        Resetfields()

        MPEDetails.VisibleOnPageLoad = True
        ClassUpdatePnl.Update()
    End Sub
    Sub BindGrid(ByVal Criteria As String)
        Try
            Dim dt As New DataTable
            dt = objDivConfig.GetDivisionalConfiguration(Err_No, Err_Desc, Criteria)
            'If dt.Rows.Count > 0 And Me.ddFilterBy.SelectedIndex > 0 Then
            '    dt.DefaultView.RowFilter = ("Org_ID='" & Me.ddFilterBy.SelectedValue.ToString() & "'")
            '    dt.AcceptChanges()
            'End If
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
        Me.drpOrganization.SelectedValue = "0"
        Me.drpOrganization.Enabled = True
        Me.chkFOC.Checked = False
        Me.chkOdo.Checked = False
        Me.txtPDCPosting.Text = ""
        Me.txtCRLimit.Text = ""
        Me.txtCollectionOutputFolder.Text = ""
        Me.txtCustNoSeq.Text = ""

        '  Me.rbContinue.Checked = False
        ' Me.rbStandard.Checked = False
        Me.btnAdd.Focus()
        chkSalesRep.Items.Clear()
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If drpOrganization.SelectedValue = "0" Then
            lblPop.Text = "Please select the Organisation"
            MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If
        If String.IsNullOrEmpty(txtCollectionOutputFolder.Text) Then
            lblPop.Text = "Please specify collection output folder."
            MPEDetails.VisibleOnPageLoad = True
            Return
        End If
        'If Me.rbStandard.Checked = False And Me.rbContinue.Checked = False Then
        '    MessageBoxValidation("Please select print format.")
        '    MPEDivConfig.Show()
        '    Return
        'End If
      



       
        Dim success As Boolean = False
        Try
            objDivConfig.Org_ID = drpOrganization.SelectedValue
            objDivConfig.Allow_Manual_FOC = IIf(chkFOC.Checked, "Y", "N")
            objDivConfig.Odo_Reading_At_Visit = IIf(chkOdo.Checked, "Y", "N")
            'If Me.rbContinue.Checked = True Then
            '    objDivConfig.PrintFormat = "Continuous"
            'End If
            'If Me.rbStandard.Checked = True Then
            '    objDivConfig.PrintFormat = "Standard"
            'End If
            objDivConfig.CustomerSequence = Val(txtCustNoSeq.Text)
            If txtPDCPosting.Text.Trim() = "" Then txtPDCPosting.Text = "0"
            objDivConfig.Advance_PDC_Posting = Convert.ToInt32(txtPDCPosting.Text)
            objDivConfig.CreditNoteLimit = CDec(IIf(Me.txtCRLimit.Text = "", "0", Me.txtCRLimit.Text))
            For Each li As RadListBoxItem In chkSalesRep.Items
                If li.Checked Then
                    objDivConfig.DC_Optional.Add(li.Value + "|Y")
                Else
                    objDivConfig.DC_Optional.Add(li.Value + "|N")
                End If
            Next

            objDivConfig.PrintFormat = "Standard"
            objDivConfig.Collection_Output_Folder = txtCollectionOutputFolder.Text.Trim()

            If objDivConfig.CheckDivControl(Err_No, Err_Desc, Me.drpOrganization.SelectedValue.ToString()) Then
                lblPop.Text = "This organization already exists"
                MPEDetails.VisibleOnPageLoad = True
                BindGrid("0")

                Exit Sub
            End If

            objDivConfig.TRN = Me.txtTRN.Text

            If Me.DatePickerDeliveryCutoffTime.SelectedDate Is Nothing Then
                objDivConfig.DeliveryCustoffTime = "0"
            Else
                objDivConfig.DeliveryCustoffTime = Me.DatePickerDeliveryCutoffTime.SelectedDate.Value.ToString("HH:mm")
            End If


            If objDivConfig.InsertDivConfig(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID) = True Then
                success = True
                MessageBoxValidation("Successfully saved.", "Information")
            End If

            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "SALES ORG CONFIGURATION", Me.drpOrganization.SelectedValue.ToString(), "Code: " & Me.drpOrganization.SelectedItem.Text & "/ MFOC :  " & IIf(chkFOC.Checked, "Y", "N") & "/ ODO Read:  " & IIf(chkOdo.Checked, "Y", "N") & "/ PDC Post:  " & IIf(txtPDCPosting.Text = "", "0", Me.txtPDCPosting.Text) & "/ Collection:  " & Me.txtCollectionOutputFolder.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", Me.drpOrganization.SelectedValue.ToString())
                Resetfields()
            Else
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_003") & "&next=DivisionalConfiguration.aspx&Title=Message", False)
                Exit Try
            End If

        Catch ex1 As SqlClient.SqlException
            If ex1.Number = "2601" Then
                lblPop.Text = "Organization already exists."
                MPEDetails.VisibleOnPageLoad = True
                Return
            End If
        Catch ex As Exception
            Err_No = "74205"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            lblPop.Text = ""
            btnUpdate.Visible = True
            btnSave.Visible = False
            'Resetfields()
            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)

            HidVal.Value = btnEdit.CommandArgument.ToString()


            drpOrganization.SelectedIndex = drpOrganization.Items.FindItemIndexByText(Trim(row.Cells(1).Text))
            drpOrganization.Enabled = False
            If Trim(row.Cells(2).Text) = "Y" Then chkFOC.Checked = True Else chkFOC.Checked = False
            If Trim(row.Cells(3).Text) = "Y" Then chkOdo.Checked = True Else chkOdo.Checked = False
            Me.txtPDCPosting.Text = Trim(row.Cells(4).Text)
            Me.txtCollectionOutputFolder.Text = row.Cells(5).Text.Replace("&nbsp;", "")
            Me.txtCRLimit.Text = row.Cells(6).Text.Replace("&nbsp;", "0")
            Me.txtCustNoSeq.Text = row.Cells(7).Text.Replace("&nbsp;", "0")
            Me.txtTRN.Text = Trim(row.Cells(8).Text).Replace("&nbsp;", "")
            Dim dcutofftime As String = Trim(row.Cells(9).Text).Replace("&nbsp;", "")


       


            DatePickerDeliveryCutoffTime.SelectedDate = DateTime.Parse(IIf(Trim(row.Cells(9).Text).Replace("&nbsp;", "") = "", "00:00", Trim(row.Cells(9).Text).Replace("&nbsp;", "")))



            'If Trim(row.Cells(5).Text) = "Standard" Then
            '    rbStandard.Checked = True
            'ElseIf Trim(row.Cells(5).Text) = "Continuous" Then
            '    rbContinue.Checked = True
            'End If

            LoadVanbyDivision()

            MPEDetails.VisibleOnPageLoad = True
        Catch ex As Exception
            Err_No = "74208"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=DivisionalConfiguration.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub
    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click

        If String.IsNullOrEmpty(txtCollectionOutputFolder.Text) Then

            lblPop.Text = "Please specify collection output folder."
            MPEDetails.VisibleOnPageLoad = True
            Return
        End If


      

        Dim success As Boolean = False
        Try
            objDivConfig.Div_Config_ID = Convert.ToInt32(HidVal.Value)
            objDivConfig.Org_ID = drpOrganization.SelectedValue
            objDivConfig.Allow_Manual_FOC = IIf(chkFOC.Checked, "Y", "N")
            'If Me.rbContinue.Checked = True Then
            '    objDivConfig.PrintFormat = "Continuous"
            'End If
            'If Me.rbStandard.Checked = True Then
            '    objDivConfig.PrintFormat = "Standard"
            'End If
            objDivConfig.Odo_Reading_At_Visit = IIf(chkOdo.Checked, "Y", "N")
            If txtPDCPosting.Text.Trim() = "" Then txtPDCPosting.Text = "0"
            objDivConfig.Advance_PDC_Posting = Convert.ToInt32(txtPDCPosting.Text)
            objDivConfig.CreditNoteLimit = CDec(IIf(Me.txtCRLimit.Text = "", "0", Me.txtCRLimit.Text))
            For Each li As RadListBoxItem In chkSalesRep.Items
                If li.Checked Then
                    objDivConfig.DC_Optional.Add(li.Value + "|Y")
                Else
                    objDivConfig.DC_Optional.Add(li.Value + "|N")
                End If
            Next
            objDivConfig.CustomerSequence = Val(txtCustNoSeq.Text)
            objDivConfig.PrintFormat = "Standard"
            objDivConfig.Collection_Output_Folder = txtCollectionOutputFolder.Text.Trim()

            objDivConfig.TRN = Me.txtTRN.Text

            If Me.DatePickerDeliveryCutoffTime.SelectedDate Is Nothing Then
                objDivConfig.DeliveryCustoffTime = "0"
            Else
                objDivConfig.DeliveryCustoffTime = Me.DatePickerDeliveryCutoffTime.SelectedDate.Value.ToString("HH:mm")
            End If



            If objDivConfig.UpdateDivConfig(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID) = True Then
                success = True
                MessageBoxValidation("Successfully Updated.", "Information")
                BindGrid("0")
                Me.MPEDetails.VisibleOnPageLoad = False

            End If

            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "U", "MAIN MENU", "SALES ORG CONFIGURATION", Me.drpOrganization.SelectedValue.ToString(), "Code: " & Me.drpOrganization.SelectedItem.Text & "/ MFOC :  " & IIf(chkFOC.Checked, "Y", "N") & "/ ODO Read:  " & IIf(chkOdo.Checked, "Y", "N") & "/ PDC Post:  " & IIf(txtPDCPosting.Text = "", "0", Me.txtPDCPosting.Text) & "/ Collection:  " & Me.txtCollectionOutputFolder.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", Me.drpOrganization.SelectedValue.ToString())
                Resetfields()

               
            Else
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_RoutePlanner_002") & "&next=DivisionalConfiguration.aspx&Title=Message", False)
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
    

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Resetfields()
        Me.MPEDetails.VisibleOnPageLoad = False

    End Sub
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
            If objDivConfig.DeleteDivisionalConfiguration(Err_No, Err_Desc, btndelete.CommandArgument.ToString()) = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "D", "MAIN MENU", "SALES ORG CONFIGURATION", OID.Trim(), "Desc: " & row.Cells(1).Text & "/ MFOC :  " & row.Cells(2).Text & "/ ODO Read:  " & row.Cells(3).Text & "/ PDC Post:  " & row.Cells(4).Text & "/ Collection:  " & IIf(row.Cells(5).Text = "", "", row.Cells(5).Text), CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", OID.Trim())
                success = True
            End If

            If success = True Then
                MessageBoxValidation("Successfully deleted.", "Information")
                ' dt = objDivConfig.GetDivisionalConfiguration(Err_No, Err_Desc, "1=1")
                BindGrid(ViewState("Criteria").ToString())
                Resetfields()
            Else
                MessageBoxValidation("Error occured while deleting configuration.", "Information")

                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_ReasonCode_005") & "&next=DivisionalConfiguration.aspx&Title=Message", False)
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
    Protected Sub btnDeleteAll_Click()
        Try
            Dim dr As GridViewRow
            Dim Success As Boolean = False
            Dim idCollection As String = ""
            For Each dr In gvDivConfig.Rows
                Dim RowCheckBox As System.Web.UI.WebControls.CheckBox = dr.FindControl("chkDelete")

                If RowCheckBox.Checked = True Then
                    Dim btn As System.Web.UI.WebControls.ImageButton = dr.FindControl("btnDelete")
                    If idCollection = "" Then
                        idCollection = btn.CommandArgument.ToString()
                    Else
                        idCollection += "," + btn.CommandArgument.ToString()
                    End If

                    '  Dim s As String() = dr.Cells(1).Text.Split("-")
                    Dim OID As String = dr.Cells(1).Text
                    '  If s.Length > 1 Then
                    'OID = s(1)
                    ' End If


                    objLogin.SaveUserLog(Err_No, Err_Desc, "D", "MAIN MENU", "SALES ORG CONFIGURATION", OID.Trim(), "Desc: " & dr.Cells(1).Text & "/ MFOC :  " & dr.Cells(2).Text & "/ ODO Read:  " & dr.Cells(3).Text & "/ PDC Post:  " & dr.Cells(4).Text & "/ Collection:  " & IIf(dr.Cells(5).Text = "", "", dr.Cells(5).Text), CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", OID.Trim())

                End If
            Next
            If objDivConfig.DeleteDivisionalConfiguration(Err_No, Err_Desc, idCollection) = True Then
                Success = True
            End If
            If (Success = True) Then
                
                MessageBoxValidation("Sales Org(s) deleted successfully.", "Information")
                '  Dt = objReason.FillReasonCode(Err_No, Err_Desc)
                BindGrid(ViewState("Criteria").ToString())
                Resetfields()
            Else
                MessageBoxValidation("Error occured while deleting configuration(s).", "Information")
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_ReasonCode_003") & "&next=ReasonCodes.aspx&Title=Message", False)
                Exit Try
            End If
        Catch ex As Exception
            Err_No = "74211"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFilter.Click
        Dim Criteria As String = "0"
        If ddFilterBy.SelectedValue <> "0" Then
            Criteria = ddFilterBy.SelectedValue
        End If
        Dim dt As New DataTable
        dt = objDivConfig.GetDivisionalConfiguration(Err_No, Err_Desc, Criteria)
        Dim dv As New DataView(dt)
        If SortField <> "" Then
            dv.Sort = (SortField & " ") + SortDirection
        End If
        gvDivConfig.DataSource = dv
        gvDivConfig.DataBind()
        ClassUpdatePnl.Update()

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

    Protected Sub drpOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles drpOrganization.SelectedIndexChanged
        LoadVanbyDivision()
    End Sub

    Sub LoadVanbyDivision()
        Dim objUser As New User
        Dim dtVan As New DataTable
        dtVan = objUser.GetVanListByOrgID(CLng(drpOrganization.SelectedValue))
        chkSalesRep.DataTextField = "SalesRep_Name"
        chkSalesRep.DataValueField = "SalesRep_ID"
        chkSalesRep.DataSource = dtVan
        chkSalesRep.DataBind()

        For Each dr As DataRow In dtVan.Rows
            Dim chkItem As New RadListBoxItem
            chkItem = chkSalesRep.FindItemByValue(dr("SalesRep_ID").ToString())
            If dr("Is_DC_Optional").ToString() = "Y" Then
                chkItem.Checked = True
            Else
                chkItem.Checked = False
            End If
        Next
    End Sub
End Class