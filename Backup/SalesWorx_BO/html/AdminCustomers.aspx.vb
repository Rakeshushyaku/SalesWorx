Imports SalesWorx.BO.Common
Imports System.Data
Imports System.Web
Imports System.Resources
Imports System.Web.UI.WebControls
Imports log4net
Imports System.Text.RegularExpressions
Partial Public Class AdminCustomers
    Inherits System.Web.UI.Page
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim ObjCommon As Common
    Dim ObjCustomer As Customer
    Dim oGeocodeList As List(Of String)
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim Dt As New DataTable
    Private Const PageID As String = "P286"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim LocCount As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        If Not Page.IsPostBack Then

ObjCommon = New Common()
        Dim ControlVal = ObjCommon.GetAppConfig(Err_No, Err_Desc, "ENABLE_CUST_PRICE").ToUpper()
        reqCr1.Visible = False
        reqCr2.Visible = False
        If ControlVal = "Y" Then
        tdpr1.Visible = True
        tdpr2.Visible = True
            Else
 tdpr1.Visible = False
        tdpr2.Visible = False
        End If
           LnksetLocation.Attributes.Add("OnClick", "javascript:return showmap()")
           btnCancelLoc.Attributes.Add("OnClick", "javascript:return HideMap()")
           btnUpdateLoc.Attributes.Add("OnClick", "javascript:return SetLocation()")
            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            If Not HasPermission Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If
            Dim dtConfig As New DataTable

            dtConfig = ObjCommon.GetFsrCustRelation(Err_No, Err_Desc)
            If dtConfig.Rows.Count > 0 Then
                FSR_CUST_REL.Value = dtConfig.Rows(0)("Control_Value").ToString
            End If

            LoadOrganization()
            LoadPriceList()
            LoadCustomerSegment()
            LoadSalesDistricts()

            If Not Request.QueryString("Customer_ID") Is Nothing And Not Request.QueryString("Site_Use_ID") Is Nothing Then
                   opt.Value = "2"
                    Customer_ID.Value = Request.QueryString("Customer_ID")
                    SiteUse_ID.Value = Request.QueryString("Site_Use_ID")
                    LoadCustomerDetails()
                    LoadShipAddress()
                    BtnAddShip.Visible = True
            Else
                    opt.Value = "1"
                    BtnAddShip.Visible = False
            End If

 'Me.MapWindow.VisibleOnPageLoad = True
 '            oGeocodeList = New List(Of [String])()
 '           Dim temp_mapinfo
 '           Dim temp_geocode As String = ""
 '           oGeocodeList = New List(Of [String])()
 '           temp_geocode = "'25.000000, 55.000000'"
 '           oGeocodeList.Add(temp_geocode)
 '           Dim oMessageList As New List(Of String)()
 '           temp_mapinfo = " '<span class=formatText></span>' "
 '           oMessageList.Add(temp_mapinfo)
 '           Dim geocodevalues = String.Join(",", oGeocodeList.ToArray())
 '           Dim message As [String] = String.Join(",", oMessageList.ToArray())
 '           'Dim geocodevalues = String.Join(",", oGeocodeList.ToArray())
 '           ClientScript.RegisterArrayDeclaration("locationList", geocodevalues)
 '           'ClientScript.RegisterArrayDeclaration("message", message)
 '           Page.ClientScript.RegisterStartupScript(GetType(String), "Intialization", "initialize();", True)

 '           oGeocodeList = Nothing

        End If
        Me.MapWindow.VisibleOnPageLoad = False
    End Sub
     Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Information"
        lblMessage.Text = str
        MpInfoError.Show()
        Me.Panel.Update()
        Exit Sub
    End Sub
    Sub LoadCustomerDetails()
        Dim dtCust As New DataTable
        dtCust = (New SalesWorx.BO.Common.Customer).GetCustomerDeatils(Err_No, Err_Desc, Customer_ID.Value, SiteUse_ID.Value)
        If dtCust.Rows.Count > 0 Then
            Dim dr As DataRow
            dr = dtCust.Rows(0)
            txt_CusNo.Text = dr("Customer_No").ToString
            txt_CusName.Text = dr("Customer_Name").ToString
            txt_Custaddress.Text = dr("Address").ToString
            txt_CustLocation.Text = dr("Location").ToString
            txt_CustCity.Text = dr("City").ToString
            txt_Contact.Text = dr("Contact").ToString
            txt_phone.Text = dr("Phone").ToString
            txt_CreditPeriod.Text = dr("Bill_Credit_Period").ToString
            txt_CreditLimit.Text = dr("Credit_Limit").ToString
            txt_availBalance.Text = dr("Avail_Bal").ToString
            If Not ddl_Pricelist.Items.FindByValue(dr("Price_List_ID").ToString()) Is Nothing Then
             ddl_Pricelist.ClearSelection()
             ddl_Pricelist.Items.FindByValue(dr("Price_List_ID").ToString()).Selected = True
            End If
            If Not ddlOrganization.Items.FindByValue(dr("Custom_Attribute_1").ToString()) Is Nothing Then
             ddlOrganization.ClearSelection()
             ddlOrganization.Items.FindByValue(dr("Custom_Attribute_1").ToString()).Selected = True
             ddlOrganization.Enabled = False
            End If
            If Not Rdo_CashCust.Items.FindByValue(dr("Cash_Cust").ToString()) Is Nothing Then
             Rdo_CashCust.ClearSelection()
             Rdo_CashCust.Items.FindByValue(dr("Cash_Cust").ToString()).Selected = True
            End If
            If dr("Cash_Cust").ToString() = "Y" Then
                txt_CreditLimit.Enabled = False
                txt_CreditPeriod.Enabled = False
            Else
                txt_CreditLimit.Enabled = True
                txt_CreditPeriod.Enabled = True
            End If
            If Not rdo_CreditHold.Items.FindByValue(dr("Credit_Hold").ToString()) Is Nothing Then
             rdo_CreditHold.ClearSelection()
             rdo_CreditHold.Items.FindByValue(dr("Credit_Hold").ToString()).Selected = True
            End If
        If FSR_CUST_REL.Value.ToUpper = "Y" Then
            lable1.Visible = True
            VanList.Enabled = True
            loadAllVans()
        Else
            lable1.Visible = True
            VanList.Enabled = False
        End If

        End If
    End Sub
    Sub loadAllVans()
    Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        ObjCommon = New Common()
        Dim VanDt As New DataTable
        VanDt = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, objUserAccess.UserID.ToString())
        VanList.DataSource = VanDt
        VanList.DataValueField = "SalesRep_ID"
        VanList.DataTextField = "SalesRep_Name"
        VanList.DataBind()
    End Sub
    Sub LoadOrganization()
        Try
                ObjCommon = New Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New ListItem("-- Select a value --", "0"))
        Catch ex As Exception

        End Try
    End Sub
    Sub LoadPriceList()
        Try
                ObjCommon = New Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddl_Pricelist.DataSource = (New SalesWorx.BO.Common.Price).GetPriceListHeader(Err_No, Err_Desc, "", "")
                ddl_Pricelist.DataTextField = "Description"
                ddl_Pricelist.DataValueField = "Price_List_ID"
                ddl_Pricelist.DataBind()
                ddl_Pricelist.Items.Insert(0, New ListItem("-- Select a value --", "0"))

        Catch ex As Exception


        End Try
    End Sub
    Sub LoadShipAddress()
        Try
            Dim dtShip As New DataTable
            dtShip = (New SalesWorx.BO.Common.Customer).GetCustomerShipAddress(Err_No, Err_Desc, Customer_ID.Value, txt_filterName.Text, txt_filterNo.Text)
        LocCount = dtShip.Rows.Count

        Dim dv As New DataView(dtShip)
        If Not ViewState("SortField") Is Nothing Then
           If ViewState("SortField").ToString <> "" Then
            dv.Sort = (ViewState("SortField") & " ") + SortDirection
           End If
        End If
        GVShipAddress.DataSource = dv
        GVShipAddress.DataBind()

        Catch ex As Exception

        End Try
    End Sub

    Sub LoadCustomerSegment()
        Try
                ddl_Segment.DataSource = (New SalesWorx.BO.Common.CustomerSegment).SearchCustomerSegmentGrid(Err_No, Err_Desc, "")
                ddl_Segment.DataTextField = "Description"
                ddl_Segment.DataValueField = "Customer_Segment_ID"
                ddl_Segment.DataBind()
                ddl_Segment.Items.Insert(0, New ListItem("-- Select a value --", "0"))
        Catch ex As Exception

        End Try
    End Sub

    Sub LoadSalesDistricts()
        Try
                ddl_SalesDistrict.DataSource = (New SalesWorx.BO.Common.SalesDistrict).SearchSalesDistrictGrid(Err_No, Err_Desc, "")
                ddl_SalesDistrict.DataTextField = "Description"
                ddl_SalesDistrict.DataValueField = "Sales_District_ID"
                ddl_SalesDistrict.DataBind()
                ddl_SalesDistrict.Items.Insert(0, New ListItem("-- Select a value --", "0"))
        Catch ex As Exception

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
    Private Sub GVShipAddress_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GVShipAddress.PageIndexChanging
        GVShipAddress.PageIndex = e.NewPageIndex
        LoadShipAddress()
    End Sub
    Private Sub GVShipAddress_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GVShipAddress.Sorting
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        LoadShipAddress()
    End Sub
    Protected Sub BtnAddShip_Click1(ByVal sender As Object, ByVal e As EventArgs) Handles BtnAddShip.Click
        Dim dtShip As New DataTable
        dtShip = (New SalesWorx.BO.Common.Customer).GetCustomerShipAddress(Err_No, Err_Desc, Customer_ID.Value, "", "")
        LocCount = dtShip.Rows.Count

        'If LocCount < 1 Then
            clearShipFileds()
            OptShip.Value = "1"
            Me.MapWindow.VisibleOnPageLoad = True
            'ScriptManager.RegisterStartupScript(Me, GetType(String), "EnableVanlist", "EnableVanlist();", True)
              VanlistTokenBox.Visible = True
        'Else
        '     Me.MapWindow.VisibleOnPageLoad = False
        '     MessageBoxValidation("You Cannot add more than 1 ship address")
        'End If
    End Sub
    Sub clearShipFileds()
        Txt_ShipCustNo.Text = txt_CusNo.Text
        Txt_ShipCustName.Text = ""
        Txt_ShipAddress.Text = ""
        Txt_ShipLocation.Text = ""
        Txt_ShipCity.Text = ""
        txt_ShipLat.Text = ""
        txt_ShipLong.Text = ""
        Txt_ShipPO.Text = ""
        ddl_SalesDistrict.ClearSelection()
        ddl_Segment.ClearSelection()
        
    End Sub
    Sub LoadShipFileds()
        Txt_ShipCustNo.Text = txt_CusNo.Text
        Txt_ShipCustName.Text = ""
        Txt_ShipAddress.Text = ""
        Txt_ShipLocation.Text = ""
        Txt_ShipCity.Text = ""
        txt_ShipLat.Text = ""
        txt_ShipLong.Text = ""
        Txt_ShipPO.Text = ""
        ddl_SalesDistrict.ClearSelection()
        ddl_Segment.ClearSelection()
    End Sub
    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnAdd.Click
    Try
        If ValidateInputCustomer() Then
            ObjCustomer = New SalesWorx.BO.Common.Customer
            Dim customerID As String
            Dim SiteUseID As String

            customerID = Customer_ID.Value
            SiteUseID = SiteUse_ID.Value
            If ObjCustomer.SaveCustomer(Err_No, Err_Desc, opt.Value, customerID, SiteUseID, txt_CusName.Text.Trim(), txt_CusNo.Text.Trim, txt_Contact.Text.Trim, txt_CustLocation.Text.Trim, txt_Custaddress.Text.Trim, txt_CustCity.Text.Trim, txt_phone.Text.Trim, Rdo_CashCust.SelectedItem.Value, txt_CreditPeriod.Text.Trim(), txt_CreditLimit.Text.Trim, txt_availBalance.Text.Trim, rdo_CreditHold.SelectedItem.Value, ddl_Pricelist.SelectedItem.Value, ddlOrganization.SelectedItem.Value) = True Then
                If opt.Value = "1" Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "CUSTOMER", txt_CusName.Text.Trim(), "Code: " & txt_CusNo.Text.Trim & "/ Cash Cust :  " & Rdo_CashCust.SelectedItem.Value & "/ Price List:  " & ddl_Pricelist.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                Else
                objLogin.SaveUserLog(Err_No, Err_Desc, "U", "MAIN MENU", "CUSTOMER", txt_CusName.Text.Trim(), "Code: " & txt_CusNo.Text.Trim & "/ Cash Cust :  " & Rdo_CashCust.SelectedItem.Value & "/ Price List:  " & ddl_Pricelist.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                End If

                opt.Value = "2"
                Customer_ID.Value = customerID
                SiteUse_ID.Value = SiteUseID
                MessageBoxValidation("Customer Saved successfully")
                BtnAddShip.Visible = True
            Else
                If Err_Desc <> "" Then
                    MessageBoxValidation(Err_Desc)
                Else
                    MessageBoxValidation("Error while saving customer. Please contact administrator")
                End If
                BtnAddShip.Visible = False
            End If
        End If

    Catch ex As Exception

    End Try
    End Sub
    Function ValidateInputCustomer() As Boolean
ObjCommon = New Common()
        Dim bretval As Boolean = True
        If ddlOrganization.SelectedItem.Value = "0" Then
            MessageBoxValidation("Please Select the Organiztion")
            bretval = False
            Return bretval
       End If

        If txt_CusName.Text.Trim = "" Then
            MessageBoxValidation("Please Enter the Customer Name")
            bretval = False
            Return bretval
        End If
        If txt_CusNo.Text.Trim = "" Then
            MessageBoxValidation("Please Enter the Customer No.")
            bretval = False
            Return bretval
        End If
         Dim ControlVal = ObjCommon.GetAppConfig(Err_No, Err_Desc, "ENABLE_CUST_PRICE").ToUpper()
        If ControlVal = "Y" Then
        If ddl_Pricelist.SelectedItem.Value = "0" Then
            MessageBoxValidation("Please Select the Price List.")
            bretval = False
            Return bretval
        End If
        End If
        If Rdo_CashCust.SelectedItem.Value = "N" Then

            If txt_CreditLimit.Text.Trim = "" Then
                MessageBoxValidation("Please Enter the Credit Limit.")
                bretval = False
                Return bretval
            End If

            If txt_CreditPeriod.Text.Trim = "" Then
                MessageBoxValidation("Please Enter the Credit Period.")
                bretval = False
                Return bretval
            End If
        End If
        Return bretval
    End Function

Protected Sub btnSaveship_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveship.Click
    ScriptManager.RegisterStartupScript(Me, GetType(String), "EnableVanlist", "EnableVanlist();", True)

    If ValidInputCustomerShipAddress() Then

       Dim vans As String = ""

    If FSR_CUST_REL.Value.ToUpper = "Y" Then
        VanlistTokenBox.Visible = True
       For Each item As ListItem In VanList.Items
         If item.Selected Then
                vans = vans & item.Value & ","
       End If
       Next
       If vans.Trim <> "" Then
           vans = vans.Substring(0, vans.Length - 1)
       End If
          Else

   End If
       ObjCustomer = New SalesWorx.BO.Common.Customer
             Dim customerID As String
            Dim SiteUseID As String

            customerID = Customer_ID.Value
            SiteUseID = SiteUse_IDShip.Value

            If ObjCustomer.SaveCustomerShipAddress(Err_No, Err_Desc, OptShip.Value, customerID, SiteUseID, Txt_ShipCustName.Text.Trim(), Txt_ShipCustNo.Text.Trim, Txt_ShipAddress.Text.Trim, Txt_ShipPO.Text.Trim, Txt_ShipCity.Text.Trim, ddl_Segment.SelectedItem.Value, txt_ShipLat.Text.Trim, txt_ShipLong.Text.Trim, ddlOrganization.SelectedItem.Value, ddl_SalesDistrict.SelectedItem.Value, vans, Txt_ShipLocation.Text) = True Then
                If OptShip.Value = "1" Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "SHIP ADDRESS", Txt_ShipCustName.Text.Trim(), "Code: " & txt_CusNo.Text.Trim & "Status:Y", CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
            Else
                objLogin.SaveUserLog(Err_No, Err_Desc, "U", "MAIN MENU", "SHIP ADDRESS", Txt_ShipCustName.Text.Trim(), "Code: " & txt_CusNo.Text.Trim & "Status:Y", CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
            End If

                MessageBoxValidation("Ship Address saved successfully")
                SiteUse_IDShip.Value = SiteUseID
                LoadShipAddress()
                ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "CloseWindow();", True)
                ScriptManager.RegisterStartupScript(Me, GetType(String), "EnableVanlist", "EnableVanlist();", True)
            Else
                If Err_Desc <> "" Then
                    MessageBoxValidation(Err_Desc)
                Else
                    MessageBoxValidation("Error while saving customer. Please contact administrator")
                End If
                ScriptManager.RegisterStartupScript(Me, GetType(String), "EnableVanlist", "EnableVanlist();", True)
            End If

 End If
End Sub

Protected Sub lbEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
            Dim btnEdit As LinkButton = TryCast(sender, LinkButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
            Dim LblCustomer_ID As System.Web.UI.WebControls.Label = row.FindControl("lblCustomer_ID")
            Dim LblSite_ID As System.Web.UI.WebControls.Label = row.FindControl("lblSite_Use_ID_Ship")
            OptShip.Value = "2"
            SiteUse_IDShip.Value = LblSite_ID.Text
            LoadShipAddressDetails(LblCustomer_ID.Text, LblSite_ID.Text)
            Me.MapWindow.VisibleOnPageLoad = True

            UpdatePanel1.Update()
End Sub
Protected Sub lbChangeStatus_Click(ByVal sender As Object, ByVal e As EventArgs)
            Dim btnChangeStatus As LinkButton = TryCast(sender, LinkButton)
            Dim row As GridViewRow = DirectCast(btnChangeStatus.NamingContainer, GridViewRow)
            Dim LblCustomer_ID As System.Web.UI.WebControls.Label = row.FindControl("lblCustomer_ID")
            Dim LblSite_ID As System.Web.UI.WebControls.Label = row.FindControl("lblSite_Use_ID_Ship")
            Dim LblStatus As System.Web.UI.WebControls.Label = row.FindControl("lblStatus")
            ObjCustomer = New SalesWorx.BO.Common.Customer
           If ObjCustomer.SaveCustomerShipAddress(Err_No, Err_Desc, 3, LblCustomer_ID.Text, LblSite_ID.Text, "", "", "", "", "", "", "", "", "", "", "", "") Then
               If LblStatus.Text = "Y" Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "U", "MAIN MENU", "SHIP ADDRESS", Txt_ShipCustName.Text.Trim(), "Code: " & txt_CusNo.Text.Trim & "Status:N", CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                MessageBoxValidation("Ship address disabled Successfully")
              Else
                objLogin.SaveUserLog(Err_No, Err_Desc, "U", "MAIN MENU", "SHIP ADDRESS", Txt_ShipCustName.Text.Trim(), "Code: " & txt_CusNo.Text.Trim & "Status:Y", CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                MessageBoxValidation("Ship address enabled Successfully")
              End If
           Else
                MessageBoxValidation("Unexpected error occured. Please contact the administrator")
           End If
            LoadShipAddress()
End Sub
Protected Sub btnUpdateLoc_Click(ByVal sender As Object, ByVal e As EventArgs)
 txt_ShipLat.Text = txtLoc_Latitude.Text
 txt_ShipLong.Text = txtLoc_Long.Text
 map.Visible = False
 details.Visible = True
  UpdatePanel1.Update()
End Sub

Protected Sub btnCancelLoc_Click(ByVal sender As Object, ByVal e As EventArgs)
 map.Visible = False
 details.Visible = True
  UpdatePanel1.Update()
End Sub
Sub LoadShipAddressDetails(ByVal customerId As String, ByVal SiteID As String)
    Dim dtCust As New DataTable
        dtCust = (New SalesWorx.BO.Common.Customer).GetCustomerShipAddressDeatils(Err_No, Err_Desc, customerId, SiteID)
        If dtCust.Rows.Count > 0 Then
            Dim dr As DataRow
            dr = dtCust.Rows(0)
            Txt_ShipCustNo.Text = dr("Customer_No").ToString
            Txt_ShipCustNo.Enabled = False
            Txt_ShipCustName.Text = dr("Customer_Name").ToString
            Txt_ShipAddress.Text = dr("Address").ToString
            Txt_ShipCity.Text = dr("City").ToString
            Txt_ShipPO.Text = dr("Postal_Code").ToString
            Txt_ShipLocation.Text = dr("Location").ToString
            txt_ShipLat.Text = dr("Cust_Lat").ToString
            txt_ShipLong.Text = dr("Cust_Long").ToString
            SiteUse_IDShip.Value = SiteID
            If Not ddl_Segment.Items.FindByValue(dr("Customer_Segment_ID").ToString()) Is Nothing Then
             ddl_Segment.ClearSelection()
             ddl_Segment.Items.FindByValue(dr("Customer_Segment_ID").ToString()).Selected = True
            End If
            If Not ddl_SalesDistrict.Items.FindByValue(dr("Sales_District_ID").ToString()) Is Nothing Then
             ddl_SalesDistrict.ClearSelection()
             ddl_SalesDistrict.Items.FindByValue(dr("Sales_District_ID").ToString()).Selected = True
            End If
               If FSR_CUST_REL.Value.ToUpper = "Y" Then
                lable1.Visible = True
                VanList.Enabled = True
                VanList.ClearSelection()
                loadAllVans()
                loadVans(customerId, SiteID, dr("Custom_Attribute_1").ToString())
               Else
                lable1.Visible = True
                VanList.Enabled = False
               End If
       End If
End Sub
    Sub loadVans(ByVal customerId As String, ByVal SiteID As String, ByVal OrgId As String)

        If ddlOrganization.SelectedItem.Value = OrgId Then
            Dim dtvan As New DataTable
            dtvan = (New SalesWorx.BO.Common.Customer).GetShipAddressVans(Err_No, Err_Desc, customerId, SiteID)
            For Each dr In dtvan.Rows
                If Not VanList.Items.FindByValue(dr("SalesRep_ID").ToString) Is Nothing Then
                        VanList.Items.FindByValue(dr("SalesRep_ID").ToString).Selected = True
                End If
            Next
        End If
    End Sub
Function ValidInputCustomerShipAddress() As Boolean
        Dim bretval As Boolean = True
        If Txt_ShipCustName.Text.Trim = "" Then
            MessageBoxValidation("Please Enter the Customer Name")
            bretval = False
            Return bretval
        End If
       If FSR_CUST_REL.Value.ToUpper = "Y" Then
            If VanList.SelectedItem Is Nothing Then
                MessageBoxValidation("Please Add the associated Sales persons.")
                bretval = False
                Return bretval
            End If
       Else
           If ddl_Segment.SelectedItem.Value = "0" Then
            MessageBoxValidation("Please Select the Customer Segment.")
            bretval = False
            Return bretval
            End If
       End If
       'If txt_ShipLat.Text.Trim = "" Then
       '     MessageBoxValidation("Please Enter the Latitude")
       '     bretval = False
       '     Return bretval
       ' End If
       ' If txt_ShipLong.Text.Trim = "" Then
       '     MessageBoxValidation("Please Enter the Longitude")
       '     bretval = False
       '     Return bretval
       ' End If

Return bretval
End Function
    Private Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrganization.SelectedIndexChanged

         If FSR_CUST_REL.Value.ToUpper = "Y" Then
            lable1.Visible = True
            VanList.Enabled = True
            VanList.Items.Clear()
           loadAllVans()
        Else
            'lable1.Visible = False
            VanList.Enabled = False
        End If
        
    End Sub

Protected Sub Rdo_CashCust_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Rdo_CashCust.SelectedIndexChanged
    If Rdo_CashCust.SelectedItem.Value = "Y" Then
        txt_CreditLimit.Enabled = False
        txt_CreditPeriod.Enabled = False
          reqCr1.Visible = False
        reqCr2.Visible = False
    Else
        txt_CreditLimit.Enabled = True
        txt_CreditPeriod.Enabled = True
          reqCr1.Visible = True
        reqCr2.Visible = True
    End If
End Sub

Protected Sub Btncancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Btncancel.Click
  Response.Redirect("ListCustomers.aspx")
End Sub

Protected Sub BtnFilter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnFilter.Click
LoadShipAddress()
End Sub


'Protected Sub SetLocation(ByVal sender As Object, ByVal e As EventArgs) Handles LnksetLocation.Click
'    details.Visible = False
'    map.Visible = True
'     oGeocodeList = New List(Of [String])()
'            Dim oMessageList As New List(Of String)()
'                Dim temp_geocode As String = ""
'                Dim temp_mapinfo
'                oGeocodeList = New List(Of [String])()
'                If CDec(txt_ShipLat.Text) > 0 And CDec(txt_ShipLong.Text) > 0 Then
'                    temp_geocode = " '" & txt_ShipLat.Text.ToString() & "," & txt_ShipLong.Text.ToString() & "'"
'                Else
'                    temp_geocode = "'25.000000, 55.000000'"
'                End If
'                               ' temp_geocode = "'25.264444,55.311667'"
'                ' temp_geocode = " '" & t("Lat") & "," & t("Lng") & "," & t("Alt") & "," & t("Acc") & "' "
'                oGeocodeList.Add(temp_geocode)
'                'End If
'                'temp_geocode = " '" & sdr("Accuracy") & "," & sdr("Altitude") & "," & sdr("Longitude") & "," & sdr("Latitude") & "' "

'                ' If Not temp_mapinfo.ToString().Contains(sdr("Customer")) Then
'                temp_mapinfo = " '<span class=formatText>test</span>' "
'                oMessageList.Add(temp_mapinfo)
'                ' End If
'                ' oMessageList.Add(temp_mapinfo)

'                'End While

'                'temp_mapinfo = " '<span class=formatText>" & temp_mapinfo & "</span>' "

'                '  Else
'                'lblmsg.Text = "No results found."
'                ' End If

'                Dim message As [String] = String.Join(",", oMessageList.ToArray())
'                Dim geocodevalues = String.Join(",", oGeocodeList.ToArray())

'                'Dim geocodevalues = String.Join(",", oGeocodeList.ToArray())
'                ScriptManager.RegisterArrayDeclaration(Me, "locationList", geocodevalues)
'                '  Page.ClientScript.RegisterArrayDeclaration("message", message)

'                ' Page.ClientScript.RegisterStartupScript(GetType(String), "Intialization", "initialize();", True)
'                ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "initialize();", True)





'                oGeocodeList = Nothing
'                ' End If


'            UpdatePanel1.Update()
'End Sub

Protected Sub BtnClearFilter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnClearFilter.Click
    txt_filterName.Text = ""
    txt_filterNo.Text = ""
    LoadShipAddress()
End Sub


End Class
