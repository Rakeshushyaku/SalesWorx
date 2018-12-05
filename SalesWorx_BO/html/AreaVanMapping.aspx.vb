Imports log4net
Imports SalesWorx.BO.Common
Partial Public Class AreaVanMapping
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim objVanTerritory As New SalesWorx.BO.Common.VanTerritory
    ' Dim SortField As String = ""
    Private Const ModuleName As String = "AreaVanMapping.aspx"
    Private Const PageID As String = "P83"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objLogin As New SalesWorx.BO.Common.Login
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session.Item("USER_ACCESS") Is Nothing Then
            Session.Add("BringmeBackHere", ModuleName)
            Response.Redirect("Login.aspx", False)
            Exit Sub
        End If
        If Not IsPostBack Then
            If Not HasAuthentication() Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If
            LoadSalesReps()
            LoadCustomersSegments()
            LoadSalesDistricts()
            LoadVanTerritories("1=1")
            ViewState("Criteria") = "1=1"
        Else
            MPEDetails.VisibleOnPageLoad = False
        End If
        lblmsg.Text = ""
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
    Private Sub LoadSalesReps()

        Dim ds As New DataSet
     
        Try
            If objVanTerritory.ReturnAllSalesRep(ds, Err_No, Err_Desc) Then
                drpVan.DataSource = ds
                drpVan.DataTextField = "SalesRep_Name"
                drpVan.DataValueField = "SalesRep_ID"
                drpVan.DataBind()



                ddFilterByVan.DataSource = ds
                ddFilterByVan.DataTextField = "SalesRep_Name"
                ddFilterByVan.DataValueField = "SalesRep_ID"
                ddFilterByVan.DataBind()
            End If
        Catch ex As Exception
            ' Response.Write(Err_No & "-" & Err_Desc)
        Finally
            ds.Dispose()
            ds = Nothing
        End Try

    End Sub
    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)

        btnUpdate.Visible = True
        btnSave.Visible = False
        Resetfields()
        Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
        Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
        hfID.Value = DirectCast(row.Cells(0).FindControl("hfMapID"), HiddenField).Value
        Dim ds1 As New DataSet
        objVanTerritory.ReturnAllVanTerritories(ds1, "AF_Map_ID=" + hfID.Value, Err_No, Err_Desc)
        If ds1.Tables.Count > 0 Then
            If ds1.Tables(0).Rows.Count > 0 Then
                drpCustomerSegment.SelectedValue = ds1.Tables(0).Rows(0)("Customer_Segment_ID").ToString()
                drpSalesDistrict.SelectedValue = ds1.Tables(0).Rows(0)("Sales_District_ID").ToString()
                drpVan.SelectedValue = ds1.Tables(0).Rows(0)("SalesRep_ID").ToString()
                drpVan.Enabled = False
            End If
        End If
        MPEDetails.VisibleOnPageLoad = True
    End Sub
    Private Sub LoadCustomersSegments()

        Dim ds As New DataSet

        Try
            If objVanTerritory.ReturnAllCustomerSegments(ds, Err_No, Err_Desc) Then
                drpCustomerSegment.DataSource = ds
                drpCustomerSegment.DataTextField = "Description"
                drpCustomerSegment.DataValueField = "Customer_Segment_ID"
                drpCustomerSegment.DataBind()


                ddFilterBySegment.DataSource = ds
                ddFilterBySegment.DataTextField = "Description"
                ddFilterBySegment.DataValueField = "Customer_Segment_ID"
                ddFilterBySegment.DataBind()

            End If
        Catch ex As Exception

            ' Response.Write(Err_No & "-" & Err_Desc)
        Finally
            ds.Dispose()
            ds = Nothing
        End Try

    End Sub
    Private Sub LoadSalesDistricts()

        Dim ds As New DataSet
        Try
            If objVanTerritory.ReturnAllSalesDistricts(ds, Err_No, Err_Desc) Then
                drpSalesDistrict.DataSource = ds
                drpSalesDistrict.DataTextField = "Description"
                drpSalesDistrict.DataValueField = "Sales_District_ID"
                drpSalesDistrict.DataBind()


                ddFilterByDistrict.DataSource = ds
                ddFilterByDistrict.DataTextField = "Description"
                ddFilterByDistrict.DataValueField = "Sales_District_ID"
                ddFilterByDistrict.DataBind()

            End If
        Catch ex As Exception
            ' Response.Write(Err_No & "-" & Err_Desc)
        Finally
            ds.Dispose()
            ds = Nothing
        End Try

    End Sub

    Private Sub LoadVanTerritories(ByVal Criteria As String)
        Try
            Dim ds1 As New DataSet
            objVanTerritory.ReturnAllVanTerritories(ds1, Criteria, Err_No, Err_Desc)
            Dim dv As New DataView(ds1.Tables(0))
            If SortField <> "" Then
                dv.Sort = (SortField & " ") + SortDirection
            End If
            grdAreaVanList.DataSource = dv
            grdAreaVanList.DataBind()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Try
            Me.MPEDetails.VisibleOnPageLoad = False
            Resetfields()
        Catch

        End Try
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click

        Try


            If drpVan.SelectedValue = "0" Then
                lblmsg.Text = "Please select a van/FSR from the list."
                Me.MPEDetails.VisibleOnPageLoad = True
                Return
            End If

            'If drpCustomerSegment.SelectedValue = "0" Then
            '    MessageBoxValidation("Please select a customersegment from the list.")
            '    Return
            'End If

            If drpSalesDistrict.SelectedValue = "0" Then
                lblmsg.Text = "Please select a salesdistrict from the list."
                Me.MPEDetails.VisibleOnPageLoad = True
                Return
            End If

            objVanTerritory.CustomerSegment = drpCustomerSegment.SelectedValue
            objVanTerritory.SalesDistrict = drpSalesDistrict.SelectedValue
            objVanTerritory.SalesRepID = drpVan.SelectedValue
            '  Dim s As String() = drpVan.SelectedItem.Text.Split("-")
            Dim VanID As String = drpVan.SelectedValue
            ' If s.Length > 1 Then
            'VanID = s(1)
            ' Else
            ' VanID = s(0)
            ' End If
            
            If (objVanTerritory.AssignVantoTerritory(Err_No, Err_Desc)) Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "USER MANAGEMENT", "AREA VAN MAPPING", VanID.Trim(), "Sales Rep: " & drpVan.SelectedItem.Text & "/ Customer Segment : " & Me.drpCustomerSegment.SelectedItem.Text & "/ Sales District : " & Me.drpSalesDistrict.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), VanID.Trim(), "0")
                MessageBoxValidation("Van successfully assigned to the territory", "Information")
                Me.MPEDetails.VisibleOnPageLoad = False
                LoadVanTerritories("1=1")
            Else
                lblmsg.Text = "Could not save the mapping."
                Me.MPEDetails.VisibleOnPageLoad = True
            End If
               
        Catch ex As Exception
            If ex.Message = "ALREADY EXISTS" Then
                lblmsg.Text = "Van is already assigned to this territory."
                Me.MPEDetails.VisibleOnPageLoad = True
            Else
                lblmsg.Text = "Error while assigning van to territory."
                Me.MPEDetails.VisibleOnPageLoad = True
                log.Error(GetExceptionInfo(ex))
            End If
        End Try
    End Sub


    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click

        Try


            If drpVan.SelectedValue = "0" Then
                lblmsg.Text = "Please select a van/FSR from the list."
                Me.MPEDetails.VisibleOnPageLoad = True
                Return
            End If

            'If drpCustomerSegment.SelectedValue = "0" Then
            '    MessageBoxValidation("Please select a customersegment from the list.")
            '    Return
            'End If

            If drpSalesDistrict.SelectedValue = "0" Then
                lblmsg.Text = "Please select a salesdistrict from the list."
                Me.MPEDetails.VisibleOnPageLoad = True
                Return
            End If

            objVanTerritory.CustomerSegment = drpCustomerSegment.SelectedValue
            objVanTerritory.SalesDistrict = drpSalesDistrict.SelectedValue
            objVanTerritory.SalesRepID = drpVan.SelectedValue
            '  Dim s As String() = drpVan.SelectedItem.Text.Split("-")
            Dim VanID As String = drpVan.SelectedValue
            ' If s.Length > 1 Then
            'VanID = s(1)
            ' Else
            ' VanID = s(0)
            ' End If
            If (objVanTerritory.UpdateAssignVantoTerritory(Convert.ToInt32(hfID.Value), Err_No, Err_Desc)) Then

                objLogin.SaveUserLog(Err_No, Err_Desc, "U", "USER MANAGEMENT", "AREA VAN MAPPING", VanID.Trim(), "Sales Rep: " & drpVan.SelectedItem.Text & "/ Customer Segment : " & Me.drpCustomerSegment.SelectedItem.Text & "/ Sales District : " & Me.drpSalesDistrict.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), VanID.Trim(), "0")
                MessageBoxValidation("Van successfully assigned to the territory", "Information")
                Me.MPEDetails.VisibleOnPageLoad = False
            Else
                lblmsg.Text = "Could not save the mapping."
                Me.MPEDetails.VisibleOnPageLoad = True
            End If

            LoadVanTerritories(ViewState("Criteria"))
        Catch ex As Exception
            If ex.Message = "ALREADY EXISTS" Then
                lblmsg.Text = "Van/FSR is already assigned to this territory."
            Else
                lblmsg.Text = "Error while assigning van/FSR to territory."
                log.Error(GetExceptionInfo(ex))
            End If
        End Try
    End Sub

    Protected Sub SearchBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFilter.Click
        Dim Criteria As String = "1=1"
        If ddFilterBySegment.SelectedValue <> "0" Then
            Criteria += "  and TBL_Area_FSR_Map.Customer_Segment_ID=" + ddFilterBySegment.SelectedValue
        End If
        If ddFilterByVan.SelectedValue <> "0" Then
            Criteria += "  and TBL_Area_FSR_Map.SalesRep_ID=" + ddFilterByVan.SelectedValue
        End If
        If ddFilterByDistrict.SelectedValue <> "0" Then
            Criteria += "  and TBL_Area_FSR_Map.Sales_District_ID=" + ddFilterByDistrict.SelectedValue
        End If
        ViewState("Criteria") = Criteria
        LoadVanTerritories(Criteria)
    End Sub
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
    Protected Sub grdAreaVanList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdAreaVanList.Sorting
        SortField = e.SortExpression
        SortDirection = "flip"
        LoadVanTerritories(ViewState("Criteria").ToString())
    End Sub

    Protected Sub btnDeleteAll_Click()
        Try
            Dim row As GridViewRow
            Dim Success As Boolean = False
            For Each row In grdAreaVanList.Rows
                Dim RowCheckBox As System.Web.UI.WebControls.CheckBox = row.FindControl("chkDelete")

                If RowCheckBox.Checked = True Then
                    Dim VanID As String = DirectCast(row.Cells(0).FindControl("hfSalesrep_ID"), HiddenField).Value
                    Dim idCollection As String = DirectCast(row.Cells(0).FindControl("hfMapID"), HiddenField).Value
                    If objVanTerritory.DeleteVanTerritoryAssignment(Err_No, Err_Desc, idCollection) Then
                        objLogin.SaveUserLog(Err_No, Err_Desc, "D", "USER MANAGEMENT", "AREA VAN MAPPING", VanID.Trim(), "Sales Rep: " & row.Cells(1).Text & "/ Customer Segment : " & row.Cells(2).Text & "/ Sales District : " & row.Cells(3).Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), VanID.Trim(), "0")
                        Success = True
                    End If
                End If
            Next


            If (Success = True) Then
                MessageBoxValidation("Mappings deleted successfully.", "Information")
                LoadVanTerritories(ViewState("Criteria").ToString())
            Else
                 MessageBoxValidation("Error occured while deleting.", "Information")
            End If

            'ClassUpdatePnl.Update()
        Catch ex As Exception
            Err_No = "74063"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim btndelete As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btndelete.NamingContainer, GridViewRow)
            Dim VanID As String = DirectCast(row.Cells(0).FindControl("hfSalesrep_ID"), HiddenField).Value
            Dim idCollection As String = DirectCast(row.Cells(0).FindControl("hfMapID"), HiddenField).Value
            Dim success As Boolean = False
            
                 
            If idCollection <> "" Then
                If objVanTerritory.DeleteVanTerritoryAssignment(Err_No, Err_Desc, idCollection) Then
                    objLogin.SaveUserLog(Err_No, Err_Desc, "D", "USER MANAGEMENT", "AREA VAN MAPPING", VanID.Trim(), "Sales Rep: " & row.Cells(1).Text & "/ Customer Segment : " & row.Cells(2).Text & "/ Sales District : " & row.Cells(3).Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), VanID.Trim(), "0")
                    MessageBoxValidation("Deleted Successfully.", "Information")
                    LoadVanTerritories(ViewState("Criteria").ToString())
                Else
                    MessageBoxValidation("Error occured while deleting.", "Information")
                End If
            End If
            ' rebind the GridView


        Catch ex As Exception

            log.Error(ex.ToString)
            lblmsg.Text = "Error occured while deleting."
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub

   
    Protected Sub grdAreaVanList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdAreaVanList.PageIndexChanging
        grdAreaVanList.PageIndex = e.NewPageIndex
        LoadVanTerritories(ViewState("Criteria").ToString())
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        btnUpdate.Visible = False
        btnSave.Visible = True
        Resetfields()

        Me.lblmsg.Text = ""
        Me.MPEDetails.VisibleOnPageLoad = True
    End Sub

    Public Sub Resetfields()
        drpVan.SelectedIndex = 0
        drpCustomerSegment.SelectedIndex = 0
        drpSalesDistrict.SelectedIndex = 0
        Me.btnSave.Text = "Save"
    End Sub
End Class