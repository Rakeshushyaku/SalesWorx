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
        End If
        lblmsg.Text = ""
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
            End If
        Catch ex As Exception
            ' Response.Write(Err_No & "-" & Err_Desc)
        Finally
            ds.Dispose()
            ds = Nothing
        End Try

    End Sub
    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
        Dim Id As String = btnEdit.CommandArgument.ToString()
        btnAdd.Text = "Update"
        btnAdd.CommandName = "Update"
        Dim ds1 As New DataSet
        objVanTerritory.ReturnAllVanTerritories(ds1, "AF_Map_ID=" + Id, Err_No, Err_Desc)
        If ds1.Tables.Count > 0 Then
            If ds1.Tables(0).Rows.Count > 0 Then
                drpCustomerSegment.SelectedValue = ds1.Tables(0).Rows(0)("Customer_Segment_ID").ToString()
                drpSalesDistrict.SelectedValue = ds1.Tables(0).Rows(0)("Sales_District_ID").ToString()
                drpVan.SelectedValue = ds1.Tables(0).Rows(0)("SalesRep_ID").ToString()
                hfID.Value = Id
                btnDelete.Enabled = False
                drpVan.Enabled = False
            End If
        End If

    End Sub
    Private Sub LoadCustomersSegments()

        Dim ds As New DataSet

        Try
            If objVanTerritory.ReturnAllCustomerSegments(ds, Err_No, Err_Desc) Then
                drpCustomerSegment.DataSource = ds
                drpCustomerSegment.DataTextField = "Description"
                drpCustomerSegment.DataValueField = "Customer_Segment_ID"
                drpCustomerSegment.DataBind()
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
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click

        Try


            If drpVan.SelectedValue = "0" Then
                MessageBoxValidation("Please select a van from the list.")
                Return
            End If

            'If drpCustomerSegment.SelectedValue = "0" Then
            '    MessageBoxValidation("Please select a customersegment from the list.")
            '    Return
            'End If

            If drpSalesDistrict.SelectedValue = "0" Then
                MessageBoxValidation("Please select a salesdistrict from the list.")
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
            If btnAdd.CommandName = "Update" Then
                objVanTerritory.UpdateAssignVantoTerritory(Convert.ToInt32(hfID.Value), Err_No, Err_Desc)

                objLogin.SaveUserLog(Err_No, Err_Desc, "U", "USER MANAGEMENT", "AREA VAN MAPPING", VanID.Trim(), "Sales Rep: " & drpVan.SelectedItem.Text & "/ Customer Segment : " & Me.drpCustomerSegment.SelectedItem.Text & "/ Sales District : " & Me.drpSalesDistrict.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), VanID.Trim(), "0")
                btnAdd.Text = "Add"
                btnAdd.CommandName = "Add"
                btnDelete.Enabled = True
                drpVan.Enabled = True
                lblmsg.Text = "Updated successfully. "
            Else
                objVanTerritory.AssignVantoTerritory(Err_No, Err_Desc)
                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "USER MANAGEMENT", "AREA VAN MAPPING", VanID.Trim(), "Sales Rep: " & drpVan.SelectedItem.Text & "/ Customer Segment : " & Me.drpCustomerSegment.SelectedItem.Text & "/ Sales District : " & Me.drpSalesDistrict.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), VanID.Trim(), "0")
                lblmsg.Text = "Van successfully assigned to the territory ."
            End If

            LoadVanTerritories("1=1")
        Catch ex As Exception
            If ex.Message = "ALREADY EXISTS" Then
                lblmsg.Text = "Van is already assigned to this territory."
            Else
                lblmsg.Text = "Error while assigning van to territory."
                log.Error(GetExceptionInfo(ex))
            End If
        End Try
    End Sub

    Protected Sub SearchBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SearchBtn.Click
        Dim Criteria As String = "1=1"
        If drpCustomerSegment.SelectedValue <> "0" Then
            Criteria += "  and TBL_Area_FSR_Map.Customer_Segment_ID=" + drpCustomerSegment.SelectedValue
        End If
        If drpVan.SelectedValue <> "0" Then
            Criteria += "  and TBL_Area_FSR_Map.SalesRep_ID=" + drpVan.SelectedValue
        End If
        If drpSalesDistrict.SelectedValue <> "0" Then
            Criteria += "  and TBL_Area_FSR_Map.Sales_District_ID=" + drpSalesDistrict.SelectedValue
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
    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblMessage.Text = str
        lblinfo.Text = "Validation"
        MpInfoError.Show()
        Exit Sub
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
        Try

            Dim idCollection As String = ""
            Dim strID As String = String.Empty

            'Loop through GridView rows to find checked rows 
            For i As Integer = 0 To grdAreaVanList.Rows.Count - 1

                Dim chkDelete As CheckBox = DirectCast(grdAreaVanList.Rows(i).Cells(0).FindControl("chkSelect"), CheckBox)
                Dim hfMapID As HiddenField = DirectCast(grdAreaVanList.Rows(i).Cells(0).FindControl("hfMapID"), HiddenField)
                If chkDelete IsNot Nothing Then
                    If chkDelete.Checked Then
                        strID = hfMapID.Value
                        If idCollection = "" Then idCollection = strID Else idCollection += "," + strID
                        ' Dim s As String() = grdAreaVanList.Rows(i).Cells(1).Text.Split("-")
                        Dim VanID As String = DirectCast(grdAreaVanList.Rows(i).Cells(0).FindControl("hfSalesrep_ID"), HiddenField).Value
                        ' If s.Length > 1 Then
                        'VanID = s(1)
                        ' Else
                        '  VanID = s(0)
                        ' End If

                        objLogin.SaveUserLog(Err_No, Err_Desc, "D", "USER MANAGEMENT", "AREA VAN MAPPING", VanID.Trim(), "Sales Rep: " & grdAreaVanList.Rows(i).Cells(1).Text & "/ Customer Segment : " & grdAreaVanList.Rows(i).Cells(2).Text & "/ Sales District : " & grdAreaVanList.Rows(i).Cells(3).Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), VanID.Trim(), "0")

                    End If
                End If
            Next

            'Call the method to Delete records 
            If idCollection <> "" Then
                If objVanTerritory.DeleteVanTerritoryAssignment(Err_No, Err_Desc, idCollection) Then lblmsg.Text = "Deleted Successfully." Else lblmsg.Text = "Error occured while deleting."
            End If

            ' rebind the GridView
            LoadVanTerritories(ViewState("Criteria").ToString())

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
End Class