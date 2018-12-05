Imports SalesWorx.BO.Common
Imports log4net
Imports System.IO
Imports Telerik.Web.UI
Partial Public Class AdminBGReports
    Inherits System.Web.UI.Page
 Dim Err_No As Long
    Dim Err_Desc As String
    Dim dt As New DataTable
    Dim objBGReport As New BGReport
    Dim objLogin As New SalesWorx.BO.Common.Login
    ' Dim SortField As String = ""
    Private Const ModuleName As String = "AdminBGReports.aspx"
    Private Const PageID As String = "P299"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try

        If ddl_year.SelectedItem.Value = "-1" Then
                MessageBoxValidation("Please select the Year", "Validation")
            Exit Sub
        End If
        If ddl_Org.SelectedItem.Value = "-1" Then
                MessageBoxValidation("Please select the Division", "Validation")
            Exit Sub
        End If

           Dim Months As IList(Of RadComboBoxItem) = ddlMonth.CheckedItems

            Dim Month As String = ""
            Dim DisplayMonth As String = ""
            For Each li As RadComboBoxItem In Months
                Month = Month & li.Value & ","
                DisplayMonth = DisplayMonth & li.Text & ","
            Next
            If Month.Trim <> "" Then
             Month = Month.Substring(0, Len(Month) - 1)
             DisplayMonth = DisplayMonth.Substring(0, Len(DisplayMonth) - 1)
            End If
            If Month.Trim = "" Then
                DisplayMonth = "All"
                Month = "-1"
            End If


            Dim Vans As IList(Of RadComboBoxItem) = ddl_Van.CheckedItems

            Dim Van As String = ""
            Dim DisplayVan As String = ""
            For Each li As RadComboBoxItem In Vans
                Van = Van & "'" & li.Value & "',"
                DisplayVan = DisplayVan & li.Text & ","
            Next
            If Van.Trim <> "" Then
             Van = Van.Substring(0, Len(Van) - 1)
             DisplayVan = DisplayVan.Substring(0, Len(DisplayVan) - 1)
            End If
            If Van.Trim = "" Then
                Van = "-1"
                DisplayVan = "All"
            End If

            Dim Areas As IList(Of RadComboBoxItem) = ddl_Area.CheckedItems

            Dim Area As String = ""
            Dim DisplayArea As String = ""
            For Each li As RadComboBoxItem In Areas
                Area = Area & "'" & li.Value & "',"
                DisplayArea = DisplayArea & li.Text & ","
            Next
            If Area.Trim <> "" Then
             Area = Area.Substring(0, Len(Area) - 1)
             DisplayArea = DisplayArea.Substring(0, Len(DisplayArea) - 1)
            End If
            If Area.Trim = "" Then
                Area = "-1"
                DisplayArea = "All"
            End If


            Dim collection As IList(Of RadComboBoxItem) = ddl_Location.CheckedItems

            Dim Loc As String = ""
             Dim DisplayLoc As String = ""
            For Each li As RadComboBoxItem In collection
                Loc = Loc & "'" & li.Value & "',"
                DisplayLoc = DisplayLoc & li.Text & ","
            Next
            If Loc.Trim <> "" Then
             Loc = Loc.Substring(0, Len(Loc) - 1)
             DisplayLoc = DisplayLoc.Substring(0, Len(DisplayLoc) - 1)
            End If
            If Loc.Trim = "" Then
                Loc = "-1"
                DisplayLoc = "All"
            End If

            Dim Customers As IList(Of RadComboBoxItem) = ddl_Customer.CheckedItems

            Dim Customer As String = ""
            Dim DisplayCustomer As String = ""
            For Each li As RadComboBoxItem In Customers
                Customer = Customer & "'" & li.Value & "',"
                DisplayCustomer = DisplayCustomer & li.Text & ","
            Next
            If Customer.Trim <> "" Then
             Customer = Customer.Substring(0, Len(Customer) - 1)
             DisplayCustomer = DisplayCustomer.Substring(0, Len(DisplayCustomer) - 1)
            End If
            If Customer.Trim = "" Then
                Customer = "-1"
                DisplayCustomer = "All"
            End If

            Dim Agencys As IList(Of RadComboBoxItem) = ddl_Agency.CheckedItems

            Dim Agency As String = ""
            Dim DisplayAgency As String = ""
            For Each li As RadComboBoxItem In Agencys
                Agency = Agency & "'" & li.Value & "',"
                DisplayAgency = DisplayAgency & li.Text & ","
            Next
            If Agency.Trim <> "" Then
             Agency = Agency.Substring(0, Len(Agency) - 1)
             DisplayAgency = DisplayAgency.Substring(0, Len(DisplayAgency) - 1)
            End If
            If Agency.Trim = "" Then
                Agency = "-1"
                DisplayAgency = "All"
            End If


            Dim Items As IList(Of RadComboBoxItem) = ddl_Product.CheckedItems

            Dim Item As String = ""
            Dim DisplayItem As String = ""
            For Each li As RadComboBoxItem In Items
                Item = Item & "'" & li.Value & "',"
                DisplayItem = DisplayItem & li.Text & ","
            Next
            If Item.Trim <> "" Then
             Item = Item.Substring(0, Len(Item) - 1)
             DisplayItem = DisplayItem.Substring(0, Len(DisplayItem) - 1)
            End If
            If Item.Trim = "" Then
                Item = "-1"
                DisplayItem = "All"
            End If

                         Dim dt As New DataTable
                         dt.Columns.Add("Column_Name", System.Type.GetType("System.String"))
                         dt.Columns.Add("Value", System.Type.GetType("System.String"))
                         dt.Columns.Add("Display_Value", System.Type.GetType("System.String"))

            Dim YrDr As DataRow
            YrDr = dt.NewRow
            YrDr("Column_Name") = "Year"
            YrDr("Value") = ddl_year.SelectedItem.Value
            YrDr("Display_Value") = ddl_year.SelectedItem.Value
            dt.Rows.Add(YrDr)


            Dim MonthDr As DataRow
            MonthDr = dt.NewRow
            MonthDr("Column_Name") = "Month"
            MonthDr("Value") = Month
            MonthDr("Display_Value") = DisplayMonth
            dt.Rows.Add(MonthDr)


            Dim OrgDr As DataRow
            OrgDr = dt.NewRow
            OrgDr("Column_Name") = "Mas_Org_ID"
            OrgDr("Value") = ddl_Org.SelectedItem.Value
            OrgDr("Display_Value") = ddl_Org.SelectedItem.Text
            dt.Rows.Add(OrgDr)

            Dim VanDr As DataRow
            VanDr = dt.NewRow
            VanDr("Column_Name") = "SalesRep_ID"
            VanDr("Value") = Van
            VanDr("Display_Value") = DisplayVan
            dt.Rows.Add(VanDr)

            Dim AreaDr As DataRow
            AreaDr = dt.NewRow
            AreaDr("Column_Name") = "Address"
            AreaDr("Value") = Area
            AreaDr("Display_Value") = DisplayArea
            dt.Rows.Add(AreaDr)

            Dim LocDr As DataRow
            LocDr = dt.NewRow
            LocDr("Column_Name") = "Location"
            LocDr("Value") = Loc
            LocDr("Display_Value") = DisplayLoc
            dt.Rows.Add(LocDr)

            Dim CustDr As DataRow
            CustDr = dt.NewRow
            CustDr("Column_Name") = "Customer"
            CustDr("Value") = Customer
            CustDr("Display_Value") = DisplayCustomer
            dt.Rows.Add(CustDr)

            Dim AgencyDr As DataRow
            AgencyDr = dt.NewRow
            AgencyDr("Column_Name") = "Agency"
            AgencyDr("Value") = Agency
            AgencyDr("Display_Value") = DisplayAgency
            dt.Rows.Add(AgencyDr)

            Dim ItemDr As DataRow
            ItemDr = dt.NewRow
            ItemDr("Column_Name") = "Inventory_Item_ID"
            ItemDr("Value") = Item
            ItemDr("Display_Value") = DisplayItem
            dt.Rows.Add(ItemDr)

            Dim DocTypeDr As DataRow
            DocTypeDr = dt.NewRow
            DocTypeDr("Column_Name") = "Doc_Type"
            DocTypeDr("Value") = ddl_DocType.SelectedItem.Value
            DocTypeDr("Display_Value") = ddl_DocType.SelectedItem.Text
            dt.Rows.Add(DocTypeDr)

          If objBGReport.SaveBGReport(Err_No, Err_Desc, 1, "0", CType(Session("User_Access"), UserAccess).UserID, dt) Then
               BindData()
                MessageBoxValidation("Report Criteria saved sucessfully.", "Information")
              Resetfields()

           Else
                MessageBoxValidation("Error while saving. Please contact the administrator.", "Information")
           End If
           Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try
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
   Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
      
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btndelete As ImageButton = TryCast(sender, ImageButton)
        Dim row As GridViewRow = DirectCast(btndelete.NamingContainer, GridViewRow)


        Dim success As Boolean = False
        Try
            Dim ReportID As String
            ReportID = CType(row.FindControl("HReport_ID"), HiddenField).Value

            

            If objBGReport.SaveBGReport(Err_No, Err_Desc, 2, ReportID, CType(Session("User_Access"), UserAccess).UserID, Nothing) = True Then
                success = True
            End If
            
            If success = True Then
                MessageBoxValidation("Successfully deleted.", "Information")
                ' dt = objDivConfig.GetDivisionalConfiguration(Err_No, Err_Desc, "1=1")
                BindData()
               
            Else
                MessageBoxValidation("Error occured while deleting.", "Information")
                log.Error(Err_Desc)
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
    Sub Resetfields()
    ddl_Org.ClearSelection()
         loadYear()
                LoadMonth()
                LoadArea()
                LoadLocation()
                LoadCustomer()
                Loadvan()
                LoadAgency()
                LoadItems()

         Me.Panel.Update()
    End Sub
     Protected Sub btnDeleteAll_Click()
         Try

         For Each row As GridViewRow In gvDivConfig.Rows
            If Not row.FindControl("chkDelete") Is Nothing Then
                If CType(row.FindControl("chkDelete"), CheckBox).Checked Then
                Dim ReportID As String
                            ReportID = CType(row.FindControl("HReport_ID"), HiddenField).Value
                           objBGReport.SaveBGReport(Err_No, Err_Desc, 2, ReportID, CType(Session("User_Access"), UserAccess).UserID, Nothing)
               End If
            End If
        Next
        BindData()
        Catch ex As Exception
            Err_No = "74210"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
     End Sub
      Private Sub ExportToExcel(ByVal strFileName As String)
        Dim file As New FileInfo(strFileName)
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
     Protected Sub btnDownload_Click(ByVal sender As Object, ByVal e As EventArgs)
         Dim btndelete As ImageButton = TryCast(sender, ImageButton)
        Dim row As GridViewRow = DirectCast(btndelete.NamingContainer, GridViewRow)


        Dim success As Boolean = False
        Try
            Dim OutputFile As String
            OutputFile = CType(row.FindControl("HFilename"), HiddenField).Value
            ExportToExcel(OutputFile)
        Catch ex As Exception
            Err_No = "74210"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
     End Sub
     Protected Sub btnView_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btndelete As LinkButton = TryCast(sender, LinkButton)
        Dim row As GridViewRow = DirectCast(btndelete.NamingContainer, GridViewRow)


        Dim success As Boolean = False
        Try
            Dim ReportID As String
            ReportID = CType(row.FindControl("HReport_ID"), HiddenField).Value
            Dim dt As New DataTable
            dt = objBGReport.GetBGReportDetails(Err_No, Err_Desc, ReportID)
            If dt.Rows.Count > 0 Then
            Dim Year As String
            Dim Months As String
            Dim Org As String
            Dim Vans As String
            Dim Areas As String
            Dim Locs As String
            Dim Custs As String
            Dim Agencys As String
            Dim Items As String
            Dim DocType As String

            Year = dt.Select("Column_Name='Year'")(0)("Display_Value").ToString()
            Months = dt.Select("Column_Name='Month'")(0)("Display_Value").ToString()
            Org = dt.Select("Column_Name='Mas_Org_ID'")(0)("Display_Value").ToString()
            Vans = dt.Select("Column_Name='SalesRep_ID'")(0)("Display_Value").ToString()
            Areas = dt.Select("Column_Name='Address'")(0)("Display_Value").ToString()
            Locs = dt.Select("Column_Name='Location'")(0)("Display_Value").ToString()
            Custs = dt.Select("Column_Name='Customer'")(0)("Display_Value").ToString()
            Agencys = dt.Select("Column_Name='Agency'")(0)("Display_Value").ToString()
            Items = dt.Select("Column_Name='Inventory_Item_ID'")(0)("Display_Value").ToString()
            DocType = dt.Select("Column_Name='Doc_Type'")(0)("Display_Value").ToString()

            lbl_year.Text = Year
            lbl_months.Text = Months
            lbl_Division.Text = Org
            lbl_Vans.Text = Vans
            lbl_Area.Text = Areas
            lbl_Location.Text = Locs
            lbl_Customer.Text = Custs
            lbl_Agency.Text = Agencys
            lbl_Items.Text = Items
            lbl_DocType.Text = DocType

                MPInfoRep.Show()
               Me.Panel.Update()
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
            Filldropdowns()
     
            BindData()
            txt_fromDate.SelectedDate = DateTime.Now
            txt_ToDate.SelectedDate = DateTime.Now
        End If

    End Sub
  Sub BindData()
     Try
           
            Dim dt As New DataTable
            dt = objBGReport.GetBGReports(Err_No, Err_Desc, txt_fromDate.SelectedDate.Value.ToString("dd-MMM-yyyy"), txt_ToDate.SelectedDate.Value.ToString("dd-MMM-yyyy"), ddl_Status.SelectedItem.Value)
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
        BindData()
    End Sub

    Protected Sub gvDivConfig_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDivConfig.PageIndexChanging
        gvDivConfig.PageIndex = e.NewPageIndex
        BindData()
    End Sub
    Sub Filldropdowns()
        dt = New DataTable()
        Dim objCommon As New Common
        Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddl_Org.DataSource = objCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddl_Org.DataTextField = "Description"
                ddl_Org.DataValueField = "MAS_Org_ID"
                ddl_Org.DataBind()
                ddl_Org.Items.Insert(0, New RadComboBoxItem("(Select)", "-1"))
                loadYear()
                LoadMonth()
                LoadArea()
                LoadLocation()
                LoadCustomer()
                Loadvan()
                LoadAgency()
                LoadItems()


    End Sub
 Private Function HasAuthentication() As Boolean
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
    End Function
 

   
    Sub LoadMonth()
      ddlMonth.Items.Clear()
      For i As Integer = 1 To 12
          ddlMonth.Items.Add(New Telerik.Web.UI.RadComboBoxItem(CDate(i.ToString & "/01/" & Now.Year).ToString("MMM"), i))
      Next
    End Sub
    Sub loadYear()
        Dim objCommon As New Common

                ddl_year.DataSource = objCommon.GetYearforOrder(Err_No, Err_Desc)
                ddl_year.DataTextField = "Yr"
                ddl_year.DataValueField = "Yr"
                ddl_year.DataBind()
                ddl_year.Items.Insert(0, New RadComboBoxItem("(Select)", "-1"))
                objCommon = Nothing
    End Sub
    Sub LoadArea()
         Dim objCommon As New Common

                ddl_Area.DataSource = objCommon.GetArea(Err_No, Err_Desc)
                ddl_Area.DataTextField = "Address"
                ddl_Area.DataValueField = "Address"
                ddl_Area.DataBind()

                objCommon = Nothing
    End Sub
    Sub LoadCustomer()

     Dim collection As IList(Of RadComboBoxItem) = ddl_Location.CheckedItems

            Dim Loc As String = ""
            For Each li As RadComboBoxItem In collection
                Loc = Loc & "'" & li.Value & "',"
            Next
            If Loc.Trim <> "" Then
            Loc = Loc.Substring(0, Len(Loc) - 1)
            End If
            If Loc.Trim = "" Then
                Loc = "-1"
            End If
        Dim objCommon As New Common
        ddl_Customer.DataSource = objCommon.GetCustomerfromOrg_Loc(Err_No, Err_Desc, ddl_Org.SelectedValue, Loc)

            ddl_Customer.Items.Clear()
            ddl_Customer.DataValueField = "CustomerID"
            ddl_Customer.DataTextField = "Customer"
            ddl_Customer.DataBind()

             objCommon = Nothing
    End Sub
    Sub Loadvan()
     Dim objCommon As New Common
Dim objUserAccess As UserAccess
                            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

                            ddl_Van.DataSource = objCommon.GetVanByOrg(Err_No, Err_Desc, ddl_Org.SelectedValue, objUserAccess.UserID)
                            ddl_Van.DataValueField = "SalesRep_ID"
                            ddl_Van.DataTextField = "SalesRep_Name"
                            ddl_Van.DataBind()

                             objCommon = Nothing

    End Sub
    Sub LoadAgency()
     Dim objCommon As New Common
     Dim objUserAccess As UserAccess
                            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

                            ddl_Agency.DataSource = objCommon.GetAllAgencyByOrg(Err_No, Err_Desc, ddl_Org.SelectedValue)
                            ddl_Agency.DataValueField = "Agency"
                            ddl_Agency.DataTextField = "Agency"
                            ddl_Agency.DataBind()

                             objCommon = Nothing


    End Sub
    Sub LoadItems()
        Dim collection As IList(Of RadComboBoxItem) = ddl_Agency.CheckedItems

            Dim Agency As String = ""
            For Each li As RadComboBoxItem In collection
                Agency = Agency & "'" & li.Value & "',"
            Next
            If Agency.Trim <> "" Then
            Agency = Agency.Substring(0, Len(Agency) - 1)
            End If
            If Agency.Trim = "" Then
                Agency = "0"
            End If
        Dim objCommon As New Common
        Dim objUserAccess As UserAccess
                            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

                            ddl_Product.DataSource = objCommon.GetProductsByOrgAgency(Err_No, Err_Desc, ddl_Org.SelectedValue, Agency)
                            ddl_Product.DataValueField = "Inventory_Item_ID"
                            ddl_Product.DataTextField = "Description"
                            ddl_Product.DataBind()

                             objCommon = Nothing

    End Sub
    Sub LoadLocation()
         Dim objCommon As New Common
            ddl_Location.DataSource = objCommon.GetCustomerLocationByOrg(Err_No, Err_Desc, ddl_Org.SelectedValue)

            ddl_Location.Items.Clear()
            ddl_Location.DataValueField = "Location"
            ddl_Location.DataTextField = "Location"
            ddl_Location.DataBind()

             objCommon = Nothing
    End Sub

    Private Sub ddl_Org_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ddl_Org.SelectedIndexChanged
        LoadLocation()
                LoadCustomer()
                Loadvan()
                LoadAgency()
                LoadItems()
 Me.Panel.Update()
    End Sub

 Private Sub ddl_Agency_ItemChecked(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxItemEventArgs) Handles ddl_Agency.ItemChecked
        LoadItems()

    End Sub

   
        Private Sub ddl_Location_ItemChecked(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxItemEventArgs) Handles ddl_Location.ItemChecked
           
        LoadCustomer()

    End Sub

    

    Private Sub btnLoadCustomer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoadCustomer.Click
LoadCustomer()

    End Sub

    Private Sub BtnLoadItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnLoadItem.Click
        LoadItems()

    End Sub

Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFilter.Click
        If txt_fromDate.SelectedDate Is Nothing Then
            MessageBoxValidation("Enter from date", "Validation")
            Exit Sub
        
        End If
        If txt_ToDate.SelectedDate Is Nothing Then
            MessageBoxValidation("Enter to date", "Validation")
            Exit Sub
        End If
        If IsDate(txt_fromDate.SelectedDate.Value) > IsDate(txt_ToDate.SelectedDate.Value) Then
            MessageBoxValidation("From date greater than to date", "Validation")
            Exit Sub
        End If
        BindData()
        Me.Panel.Update()
End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Resetfields()
    End Sub

Protected Sub btnClearFilter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClearFilter.Click
        'txt_fromDate.Text = Now.ToString("dd-MMM-yyyy")
        'txt_ToDate.Text = Now.ToString("dd-MMM-yyyy")
    ddl_Status.ClearSelection()
    BindData()
    Me.Panel.Update()
End Sub
End Class