Imports SalesWorx.BO.Common
Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports Telerik.Web.UI
Imports System.Linq
Imports ExcelLibrary.SpreadSheet
Partial Public Class SalesTargetDefinitionCopy
    Inherits System.Web.UI.Page
    Dim objProduct As New Product
    Dim objCommon As New Common
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const PageID As String = "P288"
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim oGeocodeList As List(Of String)

    Private oGeocodeList1 As List(Of String)
    Private Customer As String = Nothing
    Private Supervisor As String = Nothing
    Private VDate As String = Nothing
    Private ShowCustomer As String = Nothing
    Private LastDate As String = Nothing
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private dtErrors As New DataTable
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
                ddlOrganization.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.Items.Clear()
                ddlOrganization.Items.Add(New RadComboBoxItem("-- Select a Organization --"))
                ddlOrganization.AppendDataBoundItems = True
                ddlOrganization.DataValueField = "MAS_Org_ID"
                ddlOrganization.DataTextField = "Description"
                ddlOrganization.DataBind()


                ddl_importOrg.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddl_importOrg.Items.Clear()
                ddl_importOrg.Items.Add(New RadComboBoxItem("-- Select a Organization --"))
                ddl_importOrg.AppendDataBoundItems = True
                ddl_importOrg.DataValueField = "MAS_Org_ID"
                ddl_importOrg.DataTextField = "Description"
                ddl_importOrg.DataBind()

                ddl_ExportOrg.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddl_ExportOrg.Items.Clear()
                ddl_ExportOrg.Items.Add(New RadComboBoxItem("-- Select a Organization --"))
                ddl_ExportOrg.AppendDataBoundItems = True
                ddl_ExportOrg.DataValueField = "MAS_Org_ID"
                ddl_ExportOrg.DataTextField = "Description"
                ddl_ExportOrg.DataBind()


                Me.ddlVan.Items.Clear()
                Me.ddlVan.Items.Insert(0, (New RadComboBoxItem("--Select--")))
                Me.ddlVan.Items(0).Value = ""


                Me.ddl_ExportVan.Items.Clear()
                Me.ddl_ExportVan.Items.Insert(0, (New RadComboBoxItem("--All--")))
                Me.ddl_ExportVan.Items(0).Value = ""


                LoadYear()

                LoadPriceList()
                HSaveClick.Value = "0"
                RadTabStrip1.Tabs(0).Selected = True
                ViewState("FileType") = Nothing
                ViewState("FileName") = Nothing
                ViewState("CSVName") = Nothing

                If CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE = "Q" Then
                    Btn_SetValues.Visible = False
                    Grd_Products.Columns(3).Visible = True
                    Grd_Products.Columns(4).Visible = False
                    ddl_ValueType.SelectedItem.Value = "Q"
                ElseIf CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE = "V" Then
                    Btn_SetValues.Visible = False
                    Grd_Products.Columns(3).Visible = False
                    Grd_Products.Columns(4).Visible = True
                    ddl_ValueType.SelectedItem.Value = "V"
                ElseIf CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE = "B" Then
                    Btn_SetValues.Visible = True
                    ddl_ValueType.SelectedItem.Value = "B"
                    Grd_Products.Columns(3).Visible = True
                    Grd_Products.Columns(4).Visible = True
                End If

            Else
                MPEImport.VisibleOnPageLoad = False
                MpPricelist.VisibleOnPageLoad = False
            End If
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_MSL_005") & "&next=Welcome.aspx&Title=Product List Setup", False)
        End Try
    End Sub
    Private Sub SetErrorsTable()
        Dim col As DataColumn


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

    End Sub
    Sub LoadPriceList()
        Try
            objCommon = New Common()
            Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
            ddl_priceList.DataSource = (New SalesWorx.BO.Common.Price).GetPriceListHeader(Err_No, Err_Desc, "", "")
            ddl_priceList.DataTextField = "Description"
            ddl_priceList.DataValueField = "Price_List_ID"
            ddl_priceList.DataBind()

        Catch ex As Exception


        End Try
    End Sub
    Protected Sub ddl_ValueType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_ValueType.SelectedIndexChanged
        HSaveClick.Value = "0"
        Dim dt As New DataTable
        If Not Cache("TargetDef") Is Nothing Then
            dt = CType(Cache("TargetDef"), DataTable)
        End If
        If dt.Rows.Count > 0 Then
            dt.Rows(0)("Value_type") = ddl_ValueType.SelectedItem.Value
        End If

        If ddl_ValueType.SelectedItem.Value = "B" Then
            Grd_Products.MasterTableView.GetColumn("TgtQty").Display = True
            Grd_Products.MasterTableView.GetColumn("TgtValue").Display = True
            If dt.Rows.Count > 0 Then
                Btn_SetValues.Visible = True
            End If

        ElseIf ddl_ValueType.SelectedItem.Value = "V" Then
            Grd_Products.MasterTableView.GetColumn("TgtQty").Display = False
            Grd_Products.MasterTableView.GetColumn("TgtValue").Display = True
            If dt.Rows.Count > 0 Then
                Btn_SetValues.Visible = False
            End If
            For Each gr As GridDataItem In Grd_Products.Items
                gr.BackColor = Drawing.Color.White
            Next

        Else
            Grd_Products.MasterTableView.GetColumn("TgtQty").Display = True
            Grd_Products.MasterTableView.GetColumn("TgtValue").Display = False

            If dt.Rows.Count > 0 Then
                Btn_SetValues.Visible = False
            End If
            For Each gr As GridDataItem In Grd_Products.Items
                gr.BackColor = Drawing.Color.White
            Next

        End If
        ClassUpdatePnl.Update()
    End Sub

    Private Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrganization.SelectedIndexChanged
        HSaveClick.Value = "0"
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        ddlVan.DataSource = objCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())

        Me.ddlVan.Items.Clear()
        ddlVan.DataValueField = "SalesRep_Id"
        ddlVan.DataTextField = "SalesRep_Name"
        ddlVan.DataBind()
        ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select --", "-1"))


        Cache.Remove("TargetDef")
        LoadTargetDefinition()
    End Sub
    Sub LoadYear()
        ddlYear.Items.Clear()
        If Now.Month <> 12 Then
            For i As Integer = Now.Year To Now.Year + 1
                ddlYear.Items.Add(New RadComboBoxItem(i.ToString, i.ToString))
            Next
        Else
            For i As Integer = Now.Year + 1 To Now.Year + 1
                ddlYear.Items.Add(New RadComboBoxItem(i.ToString, i.ToString))
            Next
        End If
        ddlYear.Items.Insert(0, New RadComboBoxItem("-- Select --", 0))
        ddlMonth.Items.Clear()
        ddlMonth.Items.Insert(0, New RadComboBoxItem("-- Select --", 0))

        ddl_ImportYear.Items.Clear()

        Dim dtyear As New DataTable
        dtyear = (New SalesWorx.BO.Common.SalesTarget).GetTargetYear(Err_No, Err_Desc)
        ddl_ImportYear.DataSource = dtyear
        ddl_ImportYear.DataTextField = "Target_Year"
        ddl_ImportYear.DataValueField = "Target_Year"
        ddl_ImportYear.DataBind()
        ddl_ImportYear.Items.Insert(0, New RadComboBoxItem("-- Select --", 0))

        ddl_ImportMonth.Items.Clear()
        ddl_ImportMonth.Items.Insert(0, New RadComboBoxItem("-- Select --", 0))




        ddl_ExportYear.DataSource = dtyear
        ddl_ExportYear.DataTextField = "Target_Year"
        ddl_ExportYear.DataValueField = "Target_Year"
        ddl_ExportYear.DataBind()
        ddl_ExportYear.Items.Insert(0, New RadComboBoxItem("-- Select --", 0))

        ddl_ExportMonth.Items.Clear()
        ddl_ExportMonth.Items.Insert(0, New RadComboBoxItem("-- Select --", 0))
    End Sub
    Sub LoadMonth()
        ddlMonth.Items.Clear()
        If ddlYear.SelectedItem.Value = Now.Year Then
            For i As Integer = Now.Month To 12
                ddlMonth.Items.Add(New RadComboBoxItem(CDate(i & "/01/1900").ToString("MMM"), i.ToString))
            Next
        Else
            For i As Integer = 1 To 12
                ddlMonth.Items.Add(New RadComboBoxItem(CDate(i & "/01/1900").ToString("MMM"), i.ToString))
            Next
        End If
        ddlMonth.Items.Insert(0, New RadComboBoxItem("-- Select --", 0))

    End Sub

    'Sub LoadTargetDefinition(Optional ByVal item_Code As String = "", Optional ByVal name As String = "", Optional ByVal Agency As String = "")

    '        Dim newdt As New DataTable
    '        If Not Cache("TargetDef") Is Nothing Then
    '             Dim dt As New DataTable
    '             dt = CType(Cache("TargetDef"), DataTable)
    '             'Dim FilteredDt As New DataTable

    '        '             Dim query = _
    '        'From order In dt.AsEnumerable() _
    '        'Select order
    '        '             Dim view As DataView = query.AsDataView()
    '        '             FilteredDt = view.Table.Copy

    '             'If item_Code.Trim <> "" Then
    '             '   query = From order In FilteredDt.AsEnumerable() Where order.Field(Of String)("Item_code").ToUpper.Contains(item_Code.ToUpper()) Select order
    '             '   view = Nothing
    '             '   view = query.AsDataView()
    '             '   FilteredDt = Nothing
    '             '   FilteredDt = view.ToTable
    '             'End If
    '             'If name.Trim <> "" Then
    '             '   query = From order In FilteredDt.AsEnumerable() Where order.Field(Of String)("description").ToUpper.Contains(item_Code.ToUpper()) Select order
    '             '    view = query.AsDataView()
    '             '    FilteredDt = Nothing
    '             '   FilteredDt = view.ToTable
    '             'End If
    '             'If Agency.Trim <> "" Then
    '             '   query = From order In FilteredDt.AsEnumerable() Where order.Field(Of String)("Agency").ToUpper.Contains(item_Code.ToUpper()) Select order
    '             '    view = query.AsDataView()
    '             '    FilteredDt = Nothing
    '             '   FilteredDt = view.ToTable
    '             'End If

    '             newdt = FilteredDt.Copy

    '        Else
    '            Dim dt As New DataTable

    '            dt = (New SalesWorx.BO.Common.SalesTarget).GetTargetDefinition(Err_No, Err_Desc, ddlVan.SelectedItem.Value, ddlYear.SelectedItem.Value, ddlMonth.SelectedItem.Value)

    '            If ddlVan.SelectedItem.Value = "-1" Then
    '                 dt.Rows.Clear()
    '            End If
    '            If ddlYear.SelectedItem.Value = "0" Then
    '                dt.Rows.Clear()
    '            End If
    '             If ddlMonth.SelectedItem.Value = "0" Then
    '                dt.Rows.Clear()
    '            End If
    '            Cache("TargetDef") = dt.Copy
    '            newdt = dt.Copy
    '        End If

    '            Grd_Products.DataSource = newdt
    '             Grd_Products.DataBind()
    '             ClassUpdatePnl.Update()
    'End Sub

    Sub LoadTargetDefinition()
        Dim newdt As New DataTable
        If Not Cache("TargetDef") Is Nothing Then
            Dim dt As New DataTable
            dt = CType(Cache("TargetDef"), DataTable)
            newdt = dt.Copy
        Else
            Dim dt As New DataTable

            dt = (New SalesWorx.BO.Common.SalesTarget).GetTargetDefinition(Err_No, Err_Desc, ddlVan.SelectedItem.Value, ddlYear.SelectedItem.Value, ddlMonth.SelectedItem.Value)

            If ddlVan.SelectedItem.Value = "-1" Then
                dt.Rows.Clear()
            End If
            If ddlYear.SelectedItem.Value = "0" Then
                dt.Rows.Clear()
            End If
            If ddlMonth.SelectedItem.Value = "0" Then
                dt.Rows.Clear()
            End If

            Cache("TargetDef") = dt.Copy
            newdt = dt.Copy
        End If
        Grd_Products.DataSource = newdt
        Grd_Products.DataBind()
        If newdt.Rows.Count > 0 Then
            If Not IsDBNull(newdt.Rows(0)("Value_type")) Then
                If Not ddl_ValueType.Items.FindByValue(newdt.Rows(0)("Value_type")) Is Nothing Then
                    ddl_ValueType.ClearSelection()
                    ddl_ValueType.Items.FindByValue(newdt.Rows(0)("Value_type")).Selected = True
                End If
            End If
        End If
        If ddl_ValueType.SelectedItem.Value = "B" Then
            Grd_Products.MasterTableView.GetColumn("TgtQty").Display = True
            Grd_Products.MasterTableView.GetColumn("TgtValue").Display = True
            If newdt.Rows.Count > 0 Then
                Btn_SetValues.Visible = True
            End If
        ElseIf ddl_ValueType.SelectedItem.Value = "V" Then
            Grd_Products.MasterTableView.GetColumn("TgtQty").Display = False
            Grd_Products.MasterTableView.GetColumn("TgtValue").Display = True
            If newdt.Rows.Count > 0 Then
                Btn_SetValues.Visible = False
            End If
        Else
            Grd_Products.MasterTableView.GetColumn("TgtQty").Display = True
            Grd_Products.MasterTableView.GetColumn("TgtValue").Display = False
            If newdt.Rows.Count > 0 Then
                Btn_SetValues.Visible = False
            End If
        End If

        If CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE = "Q" Then
            Btn_SetValues.Visible = False
            Grd_Products.Columns(3).Visible = True
            Grd_Products.Columns(4).Visible = False
            ddl_ValueType.SelectedItem.Value = "Q"
        ElseIf CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE = "V" Then
            Btn_SetValues.Visible = False
            Grd_Products.Columns(3).Visible = False
            Grd_Products.Columns(4).Visible = True
            ddl_ValueType.SelectedItem.Value = "V"
        ElseIf CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE = "B" Then
            Btn_SetValues.Visible = True
            ddl_ValueType.SelectedItem.Value = "B"
            Grd_Products.Columns(3).Visible = True
            Grd_Products.Columns(4).Visible = True
        End If
        ClassUpdatePnl.Update()
    End Sub
    Private Sub ddlVan_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlVan.SelectedIndexChanged
        Cache.Remove("TargetDef")
        HSaveClick.Value = "0"
        LoadTargetDefinition()
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Private Sub ddlYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlYear.SelectedIndexChanged
        LoadMonth()
        HSaveClick.Value = "0"
        Cache.Remove("TargetDef")
        LoadTargetDefinition()
    End Sub

    Private Sub ddlMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMonth.SelectedIndexChanged
        Cache.Remove("TargetDef")
        HSaveClick.Value = "0"
        LoadTargetDefinition()
    End Sub

    Private Sub Grd_Products_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles Grd_Products.ItemCommand
        If e.CommandName = RadGrid.FilterCommandName Then
            'Dim filterPair As Pair = DirectCast(e.CommandArgument, Pair)
            'Dim filterBox1 As TextBox = CType((CType(e.Item, GridFilteringItem))("item_code").Controls(0), TextBox)
            'Dim filterBox2 As TextBox = CType((CType(e.Item, GridFilteringItem))("description").Controls(0), TextBox)
            'Dim filterBox3 As TextBox = CType((CType(e.Item, GridFilteringItem))("Agency").Controls(0), TextBox)
            'LoadTargetDefinition(filterBox1.Text, filterBox2.Text, filterBox3.Text)
            Dim dt As New DataTable
            If Not Cache("TargetDef") Is Nothing Then
                dt = CType(Cache("TargetDef"), DataTable)
            End If
            For Each gr As GridDataItem In Grd_Products.Items
                Dim qty = "", value = ""
                Dim code = gr.GetDataKeyValue("Item_Code")
                If Not gr.Cells(3).FindControl("txt_qty") Is Nothing Then
                    qty = CType(gr.Cells(3).FindControl("txt_qty"), TextBox).Text
                End If
                If Not gr.Cells(4).FindControl("txt_value") Is Nothing Then
                    value = CType(gr.Cells(4).FindControl("txt_value"), TextBox).Text
                End If


                Dim seldr() As DataRow
                seldr = dt.Select("Item_code='" & code & "'")
                If seldr.Length > 0 Then
                    seldr(0)("Target_Value_1") = qty
                    seldr(0)("Target_Value_2") = value
                End If
            Next
            Cache("TargetDef") = dt
            LoadTargetDefinition()
        End If
    End Sub
    Private Sub Grd_Products_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles Grd_Products.ItemDataBound
        'If ddl_ValueType.SelectedItem.Value = "B" Or HSaveClick.Value = "1" Then
        '       If TypeOf e.Item Is GridDataItem Then
        '           Dim gr As GridDataItem = e.Item
        '           Dim qty = "", value = ""
        '           If Not gr.Cells(3).FindControl("txt_qty") Is Nothing Then
        '               qty = CType(gr.Cells(3).FindControl("txt_qty"), TextBox).Text
        '           End If
        '           If Not gr.Cells(4).FindControl("txt_value") Is Nothing Then
        '               value = CType(gr.Cells(4).FindControl("txt_value"), TextBox).Text
        '           End If
        '           If qty <> "" And value = "" Then
        '               gr.BackColor = Drawing.Color.Red
        '           End If
        '           If qty = "" And value <> "" Then
        '               gr.BackColor = Drawing.Color.Red
        '           End If
        '       End If
        'Else
        '      If TypeOf e.Item Is GridDataItem Then
        '           Dim gr As GridDataItem = e.Item
        '            gr.BackColor = Drawing.Color.White
        '      End If
        'End If
    End Sub

    Private Sub Grd_Products_PageIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles Grd_Products.PageIndexChanged
        Dim dt As New DataTable
        If Not Cache("TargetDef") Is Nothing Then
            dt = CType(Cache("TargetDef"), DataTable)
        End If
        For Each gr As GridDataItem In Grd_Products.Items
            Dim qty = "", value = ""
            Dim code = gr.GetDataKeyValue("Item_Code")
            If Not gr.Cells(3).FindControl("txt_qty") Is Nothing Then
                qty = CType(gr.Cells(3).FindControl("txt_qty"), TextBox).Text
            End If
            If Not gr.Cells(4).FindControl("txt_value") Is Nothing Then
                value = CType(gr.Cells(4).FindControl("txt_value"), TextBox).Text
            End If
            Dim seldr() As DataRow
            seldr = dt.Select("Item_code='" & code & "'")
            If seldr.Length > 0 Then
                seldr(0)("Target_Value_1") = qty
                seldr(0)("Target_Value_2") = value
            End If

        Next
        Cache("TargetDef") = dt
        LoadTargetDefinition()
    End Sub

    Private Sub Btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_Save.Click
        If ddlOrganization.SelectedItem.Value = "-- Select a Organization --" Then
            MessageBoxValidation("Please select the organization", "Validation")
            ClassUpdatePnl1.Update()
            Exit Sub
        End If
        If ddlVan.SelectedItem.Value = "-1" Then
            MessageBoxValidation("Please select the van", "Validation")
            ClassUpdatePnl1.Update()
            Exit Sub
        End If
        If ddlYear.SelectedItem.Value = "0" Then
            MessageBoxValidation("Please select the target year", "Validation")
            ClassUpdatePnl1.Update()
            Exit Sub
        End If
        If ddlMonth.SelectedItem.Value = "0" Then
            MessageBoxValidation("Please select the target month", "Validation")
            ClassUpdatePnl1.Update()
            Exit Sub
        End If
        HSaveClick.Value = "1"
        Dim dt As New DataTable
        If Not Cache("TargetDef") Is Nothing Then
            dt = CType(Cache("TargetDef"), DataTable)
        End If
        For Each gr As GridDataItem In Grd_Products.Items
            Dim qty = "", value = ""
            Dim code = gr.GetDataKeyValue("Item_Code")
            If Not gr.Cells(3).FindControl("txt_qty") Is Nothing Then
                qty = CType(gr.Cells(3).FindControl("txt_qty"), TextBox).Text
            End If
            If Not gr.Cells(4).FindControl("txt_value") Is Nothing Then
                value = CType(gr.Cells(4).FindControl("txt_value"), TextBox).Text
            End If
            Dim seldr() As DataRow
            seldr = dt.Select("Item_code='" & code & "'")
            If seldr.Length > 0 Then
                seldr(0)("Target_Value_1") = qty
                seldr(0)("Target_Value_2") = value
            End If

        Next
        Cache("TargetDef") = dt
        LoadTargetDefinition()

        If ddl_ValueType.SelectedItem.Value = "Q" Then
            If Not ValidateQty() Then
                MessageBoxValidation("You have not entered any target quantity.Please enter at least one target quantity", "Validation")
                ClassUpdatePnl1.Update()
                Exit Sub
            End If
        ElseIf ddl_ValueType.SelectedItem.Value = "V" Then
            If Not ValidateValue() Then
                MessageBoxValidation("You have not entered any target value.Please enter at least one target value", "Validation")
                ClassUpdatePnl1.Update()
                Exit Sub
            End If
        ElseIf ddl_ValueType.SelectedItem.Value = "B" Then
            Dim bNoRow As Boolean = False
            Dim bNoQtyandValue As Boolean = False
            ValidateBoth(bNoRow, bNoQtyandValue)
            If bNoRow = False Then
                MessageBoxValidation("Please enter at least one target quantity and value", "Validation")
                ClassUpdatePnl1.Update()
                Exit Sub
            End If
            'If bNoQtyandValue = False Then
            '    MessageBoxValidation("There are one/more items for which either Target qunatity or Target Value is not defined")
            '    ClassUpdatePnl1.Update()
            '    Exit Sub
            'End If
        End If
        Dim opt As Integer

        Dim _objSalesTarget As New SalesTarget
        If dt.Rows.Count > 0 Then
            If _objSalesTarget.SaveSalesTarget(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID, ddlVan.SelectedItem.Value, ddlYear.SelectedItem.Value, ddlMonth.SelectedItem.Value, ddl_ValueType.SelectedItem.Value, dt) Then
                MessageBoxValidation("Sales Target saved", "Validation")
                ClassUpdatePnl1.Update()
                Cache.Remove("TargetDef")
                LoadTargetDefinition()
                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "SALES TARGET", ddlVan.SelectedItem.Text, "Year: " & ddlYear.SelectedItem.Text & "Month:" & ddlMonth.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")

            Else
                MessageBoxValidation("Sales Target could not be saved.Please contact the administrator.", "Validation")
            End If
        End If
    End Sub
    Protected Sub lbLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbLog.Click
        If Not Session("FTLogInfo") Is Nothing Then
            If Not Session("FTLogInfo") Is Nothing Then
                Dim fileValue As String = Session("FTLogInfo")

                Dim file As System.IO.FileInfo = New FileInfo(fileValue)

                If file.Exists Then
                    Dim filePath As String = fileValue
                    Response.ContentType = ContentType
                    Response.AppendHeader("Content-Disposition", ("attachment; filename=" + Path.GetFileName(file.Name)))
                    Response.WriteFile(filePath)
                    Response.End()

                    'Response.Clear()

                    'Response.AddHeader("Content-Disposition", "attachment; filename=" & file.Name)

                    'Response.AddHeader("Content-Length", file.Length.ToString())

                    'Response.WriteFile(file.FullName)


                    'Response.[End]()
                Else
                    lblUpMsg.Text = "File does not exist"
                    'lblMessage.ForeColor = Drawing.Color.Green
                    'lblinfo.Text = "Information"
                    MPEImport.Visible = True
                    Exit Sub

                End If
            Else
                lblUpMsg.Text = "There is no log file to download."
            End If
        Else
            lblUpMsg.Text = "There is no log to show."
            'lblMessage.ForeColor = Drawing.Color.Green
            'lblinfo.Text = "Information"
            MPEImport.Visible = True
            Exit Sub

        End If

    End Sub
    Function ValidateQty() As Boolean
        Dim bRetVal As Boolean = False
        Dim dt As New DataTable
        If Not Cache("TargetDef") Is Nothing Then
            dt = CType(Cache("TargetDef"), DataTable)
        End If
        If dt.Rows.Count > 0 Then
            Dim seldr() As DataRow
            seldr = dt.Select("Target_Value_1<>''")
            If seldr.Length > 0 Then
                bRetVal = True
            End If
        End If
        Return bRetVal
    End Function
    Function ValidateValue() As Boolean
        Dim bRetVal As Boolean = False
        Dim dt As New DataTable
        If Not Cache("TargetDef") Is Nothing Then
            dt = CType(Cache("TargetDef"), DataTable)
        End If
        If dt.Rows.Count > 0 Then
            Dim seldr() As DataRow
            seldr = dt.Select("Target_Value_2<>''")
            If seldr.Length > 0 Then
                bRetVal = True
            End If
        End If
        Return bRetVal
    End Function
    Function ValidateBoth(ByRef bNoRow As Boolean, ByRef bNoQtyandValue As Boolean)

        Dim dt As New DataTable
        If Not Cache("TargetDef") Is Nothing Then
            dt = CType(Cache("TargetDef"), DataTable)
        End If
        If dt.Rows.Count > 0 Then
            Dim seldr() As DataRow
            seldr = dt.Select("Target_Value_1<>''")
            If seldr.Length > 0 Then
                bNoRow = True
            End If
            Dim seldrval() As DataRow
            seldrval = dt.Select("Target_Value_2<>''")
            If seldrval.Length > 0 Then
                bNoRow = True
            End If

            If bNoRow = True Then
                bNoQtyandValue = True
            End If

            If seldr.Length > 0 Then
                For Each drqty In seldr
                    If drqty("Target_Value_2").ToString.Trim = "" Then
                        bNoQtyandValue = False
                        Exit For
                    End If
                Next
            End If
            If seldrval.Length > 0 Then
                For Each drval In seldrval
                    If drval("Target_Value_1").ToString.Trim = "" Then
                        bNoQtyandValue = False
                        Exit For
                    End If
                Next
            End If
        End If
    End Function
    Private Sub Btn_Export_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_Export.Click
        If ddl_ExportOrg.SelectedItem.Value = "-- Select a Organization --" Then
            MessageBoxValidation("Please select the organization", "Validation")
            ClassUpdatePnl1.Update()
            Exit Sub
        End If

        If ddl_ExportYear.SelectedItem.Value = "0" Then
            MessageBoxValidation("Please select the target year", "Validation")
            ClassUpdatePnl1.Update()
            Exit Sub
        End If
        If ddl_ExportMonth.SelectedItem.Value = "0" Then
            MessageBoxValidation("Please select the target month", "Validation")
            ClassUpdatePnl1.Update()
            Exit Sub
        End If

        Dim exportdt As New DataSet
        exportdt = (New SalesWorx.BO.Common.SalesTarget).GetTargetDefinitionforExport(Err_No, Err_Desc, ddl_ExportOrg.SelectedItem.Value, ddl_ExportVan.SelectedItem.Value, ddl_ExportYear.SelectedItem.Value, ddl_ExportMonth.SelectedItem.Value)

        ExportToExcel("SalesTarget" + Now.ToString("ddMMMyyHHmmss") + ".xls", exportdt)
    End Sub
    Private Sub ExportToExcel(ByVal strFileName As String, ByVal ds As DataSet)


        Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & strFileName
        WriteXLSFile(fn, ds)
        Dim file As New FileInfo(fn)
        Response.Clear()
        Response.ClearHeaders()
        Response.ClearContent()
        Response.AddHeader("content-disposition", "attachment; filename=" + strFileName)
        Response.AddHeader("Content-Type", "application/Excel")
        Response.ContentType = "application/vnd.xls"
        Response.AddHeader("Content-Length", file.Length.ToString())
        Response.WriteFile(file.FullName)
        Response.End()
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
                    worksheet = New Worksheet("Sheet" & iSheetCount.ToString())

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
    Private Sub Btn_SetValues_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_SetValues.Click
        Dim dt As New DataTable
        If Not Cache("TargetDef") Is Nothing Then
            dt = CType(Cache("TargetDef"), DataTable)
        End If
        For Each gr As GridDataItem In Grd_Products.Items
            Dim qty = "", value = ""
            Dim code = gr.GetDataKeyValue("Item_Code")
            If Not gr.Cells(3).FindControl("txt_qty") Is Nothing Then
                qty = CType(gr.Cells(3).FindControl("txt_qty"), TextBox).Text
            End If
            If Not gr.Cells(4).FindControl("txt_value") Is Nothing Then
                value = CType(gr.Cells(4).FindControl("txt_value"), TextBox).Text
            End If


            Dim seldr() As DataRow
            seldr = dt.Select("Item_code='" & code & "'")
            If seldr.Length > 0 Then
                seldr(0)("Target_Value_1") = qty
                seldr(0)("Target_Value_2") = value
            End If

        Next
        Cache("TargetDef") = dt
        If dt.Rows.Count Then
            Dim seldr() As DataRow
            seldr = dt.Select("Target_Value_1<>''")
            If seldr.Length > 0 Then
                LoadPriceListMP()
            Else
                MessageBoxValidation("You have not entered any Target quantity. You can load target value from quantity only if you define target quantity", "Validation")
            End If
        End If

        ClassUpdatePnl1.Update()
    End Sub
    Sub LoadPriceListMP()
        lbl_Title.Text = "Load Target Value"
        ddl_priceList.ClearSelection()
        MpPricelist.VisibleOnPageLoad = True

        Exit Sub

    End Sub
    Private Sub Btn_applyValue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_applyValue.Click

        Dim dt As New DataTable
        Dim dtPriceList As New DataSet
        dtPriceList = (New SalesWorx.BO.Common.Price).GetDefaultPriceList(Err_No, Err_Desc, ddl_priceList.SelectedItem.Value)

        If Not Cache("TargetDef") Is Nothing Then
            dt = CType(Cache("TargetDef"), DataTable)
        End If
        If dt.Rows.Count > 0 Then
            Dim seldr() As DataRow
            seldr = dt.Select("Target_Value_1<>''")
            If seldr.Length > 0 Then
                For Each dr As DataRow In seldr
                    If dtPriceList.Tables.Count > 0 Then
                        Dim pricelistdr() As DataRow
                        pricelistdr = dtPriceList.Tables(0).Select("Item_code='" & dr("Item_Code").ToString & "'")
                        If pricelistdr.Length > 0 Then
                            dr("Target_Value_2") = Val(dr("Target_Value_1").ToString) * Val(pricelistdr(0)("Unit_Selling_Price").ToString)
                        End If
                    End If
                Next
            End If
        End If
        Cache("TargetDef") = dt
        MpPricelist.VisibleOnPageLoad = False
        LoadTargetDefinition()
    End Sub



    Private Sub ddl_ImportYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_ImportYear.SelectedIndexChanged
        ddl_ImportMonth.Items.Clear()

        For i As Integer = 1 To 12
            ddl_ImportMonth.Items.Add(New RadComboBoxItem(CDate(i & "/01/1900").ToString("MMM"), i.ToString))
        Next
        ddl_ImportMonth.Items.Insert(0, New RadComboBoxItem("-- Select --", 0))
    End Sub

    Private Sub Btn_Import_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_Import.Click
        lbLog.Visible = False
        If ddl_importOrg.SelectedItem.Value = "-- Select a Organization --" Then
            MessageBoxValidation("Please select the organization", "Validation")
            ClassUpdatePnl1.Update()
            Exit Sub
        End If

        If ddl_ImportYear.SelectedItem.Value = "0" Then
            MessageBoxValidation("Please select the target year", "Validation")
            ClassUpdatePnl1.Update()
            Exit Sub
        End If
        If ddl_ImportMonth.SelectedItem.Value = "0" Then
            MessageBoxValidation("Please select the target month", "Validation")
            ClassUpdatePnl1.Update()
            Exit Sub
        End If

        MPEImport.VisibleOnPageLoad = True
        ClassUpdatePnl1.Update()
    End Sub

    Private Sub btn_Upload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImport.Click
        Session("FTLogInfo") = Nothing
        Dim _objSalesTarget As New SalesWorx.BO.Common.SalesTarget
        If Me.ExcelFileUpload.FileName = Nothing Then
            ' Me.lblinfo.Text = "Validation"
            Me.lblUpMsg.Text = "Select filename "
            ' Me.lblMessage.ForeColor = Drawing.Color.Green
            ' Me.MpInfoError.Show()
            MPEImport.VisibleOnPageLoad = True
            Exit Sub
        End If

        Dim Str As New StringBuilder

        Dim TotSuccess As Integer = 0
        Dim TotFailed As Integer = 0
        SetErrorsTable()
        Try
            ViewState("FileType") = Me.ExcelFileUpload.PostedFile.ContentType
            If ExcelFileUpload.FileName.ToString.EndsWith(".csv") Or ExcelFileUpload.FileName.ToString.EndsWith(".xls") Or ExcelFileUpload.FileName.ToString.EndsWith(".xlsx") Then

                Dim Foldername As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath")
                If Not Foldername.EndsWith("\") Then
                    Foldername = Foldername & "\"
                End If
                If Directory.Exists(Foldername.Substring(0, Len(Foldername) - 1)) = False Then
                    Directory.CreateDirectory(Foldername.Substring(0, Len(Foldername) - 1))
                End If
                If ExcelFileUpload.FileName.ToString.EndsWith(".csv") Then
                    Dim FName As String
                    FName = Now.Hour & Now.Minute & Now.Second & ExcelFileUpload.FileName
                    ViewState("FileName") = Foldername & FName
                    ViewState("CSVName") = FName
                Else
                    ViewState("FileName") = Foldername & Now.Hour & Now.Minute & Now.Second & ExcelFileUpload.FileName
                End If

                ExcelFileUpload.SaveAs(ViewState("FileName"))

                Try
                    Dim st As Boolean = False

                    If ViewState("FileType") IsNot Nothing And ViewState("FileName") IsNot Nothing Then
                        Dim TempTbl As New DataTable
                        If TempTbl.Rows.Count > 0 Then
                            TempTbl.Rows.Clear()
                        End If


                        Dim col As DataColumn


                        col = New DataColumn
                        col.ColumnName = "Itemcode"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)

                        col = New DataColumn
                        col.ColumnName = "SalesRepNumber"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)


                        If CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE = "By Quantity" Then

                            col = New DataColumn
                            col.ColumnName = "TargetQty"
                            col.DataType = System.Type.GetType("System.String")
                            col.ReadOnly = False
                            col.Unique = False
                            TempTbl.Columns.Add(col)
                        ElseIf CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE = "V" Then
                            col = New DataColumn
                            col.ColumnName = "TargetValue"
                            col.DataType = System.Type.GetType("System.String")
                            col.ReadOnly = False
                            col.Unique = False
                            TempTbl.Columns.Add(col)
                        ElseIf CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE = "B" Then
                            col = New DataColumn
                            col.ColumnName = "TargetQty"
                            col.DataType = System.Type.GetType("System.String")
                            col.ReadOnly = False
                            col.Unique = False
                            TempTbl.Columns.Add(col)

                            col = New DataColumn
                            col.ColumnName = "TargetValue"
                            col.DataType = System.Type.GetType("System.String")
                            col.ReadOnly = False
                            col.Unique = False
                            TempTbl.Columns.Add(col)
                        End If


                        If ViewState("FileName").ToString.EndsWith(".csv") Then
                            TempTbl = DoCSVUpload()
                        ElseIf ViewState("FileName").ToString.EndsWith(".xls") Then
                            TempTbl = DoXLSUpload()
                        ElseIf ViewState("FileName").ToString.EndsWith(".xlsx") Then
                            TempTbl = DoXLSXUpload()
                        End If

                        If CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE = "B" Then
                            If TempTbl.Columns.Count = 4 Then

                                If Not (TempTbl.Columns(0).ColumnName.ToLower = "itemcode" And TempTbl.Columns(1).ColumnName.ToLower = "salesrepnumber" And TempTbl.Columns(2).ColumnName.ToLower = "targetqty" And TempTbl.Columns(3).ColumnName.ToLower = "targetvalue") Then
                                    lblUpMsg.Text = "Please check the template columns are correct"

                                    MPEImport.VisibleOnPageLoad = True
                                    Exit Sub
                                End If

                            Else
                                lblUpMsg.Text = "Invalid Template"
                                '' lblMessage.ForeColor = Drawing.Color.Green

                                ' MpInfoError.Show()
                                MPEImport.VisibleOnPageLoad = True
                                Exit Sub
                            End If

                        ElseIf CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE = "Q" Then
                            If TempTbl.Columns.Count = 3 Then

                                If Not (TempTbl.Columns(0).ColumnName.ToLower = "itemcode" And TempTbl.Columns(1).ColumnName.ToLower = "salesrepnumber" And TempTbl.Columns(2).ColumnName.ToLower = "targetqty") Then
                                    lblUpMsg.Text = "Please check the template columns are correct"

                                    MPEImport.VisibleOnPageLoad = True
                                    Exit Sub
                                End If

                            Else
                                lblUpMsg.Text = "Invalid Template"
                                '' lblMessage.ForeColor = Drawing.Color.Green

                                ' MpInfoError.Show()
                                MPEImport.VisibleOnPageLoad = True
                                Exit Sub
                            End If
                        ElseIf CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE = "B" Then
                            If TempTbl.Columns.Count = 3 Then

                                If Not (TempTbl.Columns(0).ColumnName.ToLower = "itemcode" And TempTbl.Columns(1).ColumnName.ToLower = "salesrepnumber" And TempTbl.Columns(2).ColumnName.ToLower = "targetvalue") Then
                                    lblUpMsg.Text = "Please check the template columns are correct"

                                    MPEImport.VisibleOnPageLoad = True
                                    Exit Sub
                                End If

                            Else
                                lblUpMsg.Text = "Invalid Template"
                                '' lblMessage.ForeColor = Drawing.Color.Green

                                ' MpInfoError.Show()
                                MPEImport.VisibleOnPageLoad = True
                                Exit Sub
                            End If




                        End If
                        TempTbl.Columns.Add("IsValid", GetType(String))

                        If CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE = "Q" Then
                            TempTbl.Columns.Add("TargetValue", GetType(String))
                        ElseIf CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE = "V" Then
                            TempTbl.Columns.Add("TargetQty", GetType(String))
                        End If


                        If TempTbl.Rows.Count = 0 Then
                            lblUpMsg.Text = "There is no data in the uploaded file."
                            ' lblMessage.ForeColor = Drawing.Color.Green
                            'lblinfo.Text = "Information"
                            'MpInfoError.Show()
                            MPEImport.VisibleOnPageLoad = True
                            Exit Sub
                        End If

                        Dim RowNo As String = Nothing
                        ' Dim ColNo As String = Nothing
                        ' Dim ColumnName As String = Nothing
                        Dim ErrorText As String = Nothing
                        Dim OrgID As String = Nothing




                        If TempTbl.Rows.Count > 0 Then



                            Dim idx As Integer = 0

                            For Each temdr As DataRow In TempTbl.Rows

                                Dim SalesRepID As String = Nothing
                                Dim ItemCode As String = Nothing
                                Dim TargetQty As String = Nothing
                                Dim TargetValue As String = Nothing

                                OrgID = Me.ddl_importOrg.SelectedValue


                                Dim isValidRow As Boolean = True


                                ItemCode = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, "0", TempTbl.Rows(idx)(0).ToString())
                                SalesRepID = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, "0", TempTbl.Rows(idx)(1).ToString())
                                If CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE = "B" Then
                                    TargetQty = IIf(TempTbl.Rows(idx)(2) Is DBNull.Value, "0", TempTbl.Rows(idx)(2).ToString())
                                    TargetValue = IIf(TempTbl.Rows(idx)(3) Is DBNull.Value, "0", TempTbl.Rows(idx)(3).ToString())
                                ElseIf CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE = "Q" Then
                                    TargetQty = IIf(TempTbl.Rows(idx)(2) Is DBNull.Value, "0", TempTbl.Rows(idx)(2).ToString())
                                    TargetValue = "0"
                                    TempTbl.Rows(idx)("TargetValue") = TargetValue
                                ElseIf CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE = "V" Then
                                    TargetValue = IIf(TempTbl.Rows(idx)(2) Is DBNull.Value, "0", TempTbl.Rows(idx)(2).ToString())
                                    TargetQty = "0"
                                    TempTbl.Rows(idx)("TargetQty") = TargetQty
                                End If


                                If SalesRepID = "0" Or ItemCode = "0" Then
                                    TempTbl.Rows(idx)("IsValid") = "N"
                                    Continue For
                                End If
                                ' If IsNumeric(SalesRepID) Then

                                If _objSalesTarget.CheckValidFSRID(Err_No, Err_Desc, SalesRepID, OrgID) = False Then
                                    RowNo = idx + 2
                                    TempTbl.Rows(idx)("IsValid") = "N"
                                    ErrorText = "Invalid salesrep number"
                                    TotFailed += 1
                                    isValidRow = False
                                End If
                                'Else
                                'RowNo = idx + 2
                                'TempTbl.Rows(idx)("IsValid") = "N"
                                'ErrorText = "Salesrep id should be in numeric"
                                'TotFailed += 1
                                'isValidRow = False
                                'End If


                                If _objSalesTarget.CheckValidProductCode(Err_No, Err_Desc, ItemCode) = False Then
                                    RowNo = idx + 2
                                    TempTbl.Rows(idx)("IsValid") = "N"
                                    ErrorText = "The Item code does not exists"
                                    TotFailed += 1
                                    isValidRow = False
                                End If


                                If IsNumeric(TargetValue) = False And IsNumeric(TargetQty) = False Then
                                    RowNo = idx + 2
                                    TempTbl.Rows(idx)("IsValid") = "N"
                                    If CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE = "Q" Then
                                        ErrorText = "Target Qty should be in numeric"
                                    ElseIf CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE = "B" Then
                                        ErrorText = "Target value/ Target Qty should be in numeric"
                                    ElseIf CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE = "V" Then
                                        ErrorText = "Target value should be in numeric"
                                    End If
                                    TotFailed += 1
                                    isValidRow = False
                                End If




                                If Not (RowNo Is Nothing And ErrorText Is Nothing) Then
                                    Dim h As DataRow = dtErrors.NewRow()
                                    h("RowNo") = RowNo
                                    ' h("ColNo") = ColNo
                                    ' h("ColName") = ColumnName
                                    h("LogInfo") = ErrorText
                                    dtErrors.Rows.Add(h)
                                    RowNo = Nothing
                                    'ColNo = Nothing
                                    'ColumnName = Nothing
                                    ErrorText = Nothing
                                    isValidRow = False
                                End If

                                If isValidRow = True Then
                                    TempTbl.Rows(idx)("IsValid") = "Y"
                                    TotSuccess = TotSuccess + 1
                                    Dim h As DataRow = dtErrors.NewRow()
                                    h("RowNo") = idx + 2
                                    h("LogInfo") = "Successfully uploaded"
                                    dtErrors.Rows.Add(h)
                                    RowNo = Nothing
                                    ErrorText = Nothing
                                    isValidRow = True


                                End If

                                idx = idx + 1
                            Next
                        End If
                        Dim TempTblnew As DataTable
                        TempTblnew = TempTbl.Select("IsValid<>'N'").CopyToDataTable()
                        If TempTblnew.Rows.Count > 0 Then
                            If _objSalesTarget.UploadSalesTarget(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID, ddl_ImportYear.SelectedItem.Value, ddl_ImportMonth.SelectedItem.Value, TempTblnew) Then
                                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "SALES TARGET", "Import", "Year: " & ddlYear.SelectedItem.Text & "Month:" & ddlMonth.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                                DeleteExcel()
                                lblUpMsg.Text = IIf(TotFailed = 0, "Successfully uploaded", "One or more rows has invalid data. Please check the log")

                                MPEImport.VisibleOnPageLoad = True
                            Else
                                DeleteExcel()
                                lblUpMsg.Text = "Please check the uploaded log"

                                MPEImport.VisibleOnPageLoad = True
                                Exit Sub
                            End If

                        Else
                            DeleteExcel()
                            lblUpMsg.Text = "Please check the uploaded log"

                            MPEImport.VisibleOnPageLoad = True
                            Exit Sub
                        End If
                    End If
                    lbLog.Visible = True
                    Session("FTLogInfo") = dtErrors.Copy

                    Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & "FTL_" & Now.ToString("yyyyMMdd") + ".txt"
                    DataTable2CSV(dtErrors, fn, vbTab)
                    Session("FTLogInfo") = fn


                Catch ex As Exception

                    Err_No = "74085"
                    If Err_Desc Is Nothing Then
                        log.Error(GetExceptionInfo(ex))
                    Else
                        log.Error(Err_Desc)
                    End If
                End Try
            Else
                ' Str.Append("<span style='font-color:red'> <b >Please import valid Excel template.</b></span>")

                lblUpMsg.Text = "Please import valid Excel template."

                MPEImport.VisibleOnPageLoad = True
            End If


        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try

    End Sub
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

    Protected Sub DummyImBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles DummyImBtn.Click


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

                Dim cmd As New OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn)

                Dim oleda As New OleDbDataAdapter()

                oleda.SelectCommand = cmd



                oleda.Fill(dtImport)



            Catch ex As Exception
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

    Private Sub DeleteExcel()
        Try
            Dim TheFile As FileInfo = New FileInfo(ViewState("FileName"))
            If TheFile.Exists Then
                File.Delete(ViewState("FileName"))
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))

        End Try
    End Sub
    Private Function DoXLSXUpload() As DataTable
        Dim dtImport As New DataTable
        Try
            Dim connString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & ViewState("FileName") & " ;Extended Properties=""Excel 12.0;HDR=Yes;IMEX=1"""
            Dim oledbConn As New OleDbConnection(connString)
            Try
                oledbConn.Open()

                Dim cmd As New OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn)

                Dim oleda As New OleDbDataAdapter()

                oleda.SelectCommand = cmd



                oleda.Fill(dtImport)



            Catch ex As Exception
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

    Private Sub ddl_ExportOrg_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_ExportOrg.SelectedIndexChanged
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        ddl_ExportVan.DataSource = objCommon.GetVanByOrg(Err_No, Err_Desc, ddl_ExportOrg.SelectedValue, objUserAccess.UserID.ToString())

        Me.ddl_ExportVan.Items.Clear()
        ddl_ExportVan.DataValueField = "SalesRep_Id"
        ddl_ExportVan.DataTextField = "SalesRep_Name"
        ddl_ExportVan.DataBind()
        ddl_ExportVan.Items.Insert(0, New RadComboBoxItem("-- All --", "-1"))
    End Sub

    Private Sub ddl_ExportYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_ExportYear.SelectedIndexChanged
        ddl_ExportMonth.Items.Clear()

        For i As Integer = 1 To 12
            ddl_ExportMonth.Items.Add(New RadComboBoxItem(CDate(i & "/01/1900").ToString("MMM"), i.ToString))
        Next
        ddl_ExportMonth.Items.Insert(0, New RadComboBoxItem("-- Select --", 0))
    End Sub
End Class