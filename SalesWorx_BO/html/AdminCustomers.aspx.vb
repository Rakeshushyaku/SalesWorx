Imports SalesWorx.BO.Common
Imports System.Data
Imports System.Web
Imports System.Resources
Imports System.Web.UI.WebControls
Imports log4net
Imports System.Text.RegularExpressions
Imports Telerik.Web.UI
Partial Public Class AdminCustomers
    Inherits System.Web.UI.Page
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer
    Dim oGeocodeList As List(Of String)
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim Dt As New DataTable
    Private Const PageID As String = "P286"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim LocCount As Integer = 0

    Dim Credit_To_Cash As Boolean = False
    Dim Cust_Type As String
    Private Shared ENABLE_CREDIT_FOR_CASH_CUSTOMER As String = "N"
    Private Shared _strDefLat As String = "0.00000"
    Private Shared _strDefLong As String = "0.0000"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

       
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        If Not Page.IsPostBack Then

            ObjCommon = New SalesWorx.BO.Common.Common
            Dim ControlVal = ObjCommon.GetAppConfig(Err_No, Err_Desc, "ENABLE_CUST_PRICE").ToUpper()


            ENABLE_CREDIT_FOR_CASH_CUSTOMER = ObjCommon.GetAppConfig(Err_No, Err_Desc, "ENABLE_CREDIT_FOR_CASH_CUSTOMER").ToUpper()
            If ENABLE_CREDIT_FOR_CASH_CUSTOMER = "Y" Then
                reqCr1.Visible = True
                reqCr2.Visible = True
                req3.Visible = True
            Else
                reqCr1.Visible = False
                reqCr2.Visible = False
                req3.Visible = False
            End If


            If ControlVal = "Y" Then
                tdpr1.Visible = True

            Else
                tdpr1.Visible = False

            End If

            Dim Enable_def_CL_Cash_cust = ObjCommon.GetAppConfig(Err_No, Err_Desc, "ENABLE_DEF_CREDIT_LIMIT_CASH_CUST").ToUpper()
            Dim Def_Credit_Limit = ObjCommon.GetAppConfig(Err_No, Err_Desc, "DEFUALT_CREDIT_LIMIT_CASH_CUST").ToUpper()
            If ENABLE_CREDIT_FOR_CASH_CUSTOMER = "Y" Then
                If Enable_def_CL_Cash_cust = "Y" Then
                    txt_CreditLimit.Enabled = True
                    txt_availBalance.Enabled = True

                    txt_CreditLimit.Text = Def_Credit_Limit
                    txt_availBalance.Text = Def_Credit_Limit
                Else
                    txt_CreditLimit.Enabled = True
                    txt_availBalance.Enabled = True
                End If
                txt_CreditPeriod.Enabled = True
            Else
                txt_CreditLimit.Enabled = False
                txt_availBalance.Enabled = False
                txt_CreditPeriod.Enabled = False
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
            LoadCustomerClass()
            LoadCustomerType()
            Dim dt_appctrl As New DataTable
            _strDefLat = ConfigurationSettings.AppSettings("DefaultLat")
            _strDefLong = ConfigurationSettings.AppSettings("DefaultLong")
            HLat_Default.Value = _strDefLat
            HLong_Default.Value = _strDefLong

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
            If Not Request.QueryString("Mode") Is Nothing Then
                If Request.QueryString("Mode") = "V" Then
                    DisableControls()
                End If
                If Request.QueryString("Mode") = "E" Then
                    txt_CreditLimit.Enabled = False
                    txt_availBalance.Enabled = False
                End If
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
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Sub DisableControls()
        Try

        
        txt_CusNo.Enabled = False
        txt_CusName.Enabled = False
        txt_Custaddress.Enabled = False
        txt_CustLocation.Enabled = False
        txt_CustCity.Enabled = False
        txt_Contact.Enabled = False
        txt_phone.Enabled = False
        txt_CreditPeriod.Enabled = False
        txt_CreditLimit.Enabled = False
        txt_availBalance.Enabled = False
        ddl_Pricelist.Enabled = False
        ddlOrganization.Enabled = False
        Rdo_CashCust.Enabled = False

        txt_CreditPeriod.Enabled = False

        rdo_CreditHold.Enabled = False

        VanList.Enabled = False

        ddlCustType.Enabled = False
        ddlCustClass.Enabled = False


        txt_collectiongroup.Enabled = False
        txt_trn.Enabled = False
        rdo_GenericCash.Enabled = False
        BtnAddShip.Enabled = False
        GVShipAddress.Enabled = False
        BtnAdd.Enabled = False
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try

    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Sub LoadCustomerDetails()
        Try

       
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

                If dr("Cash_Cust").ToString().Trim().ToUpper() = "Y" Then
                    HCust_Type.Value = "CASH"
                Else
                    HCust_Type.Value = "CREDIT"
                End If


                If Not ddl_Pricelist.Items.FindByValue(dr("Price_List_ID").ToString()) Is Nothing Then
                    ddl_Pricelist.ClearSelection()
                    ddl_Pricelist.Items.FindByValue(dr("Price_List_ID").ToString()).Selected = True
                End If
            If Not ddlOrganization.FindItemByValue(dr("Custom_Attribute_1").ToString()) Is Nothing Then
                ddlOrganization.ClearSelection()
                ddlOrganization.FindItemByValue(dr("Custom_Attribute_1").ToString()).Selected = True
                ddlOrganization.Enabled = False
            End If
            If Not Rdo_CashCust.Items.FindByValue(dr("Cash_Cust").ToString()) Is Nothing Then
                Rdo_CashCust.ClearSelection()
                Rdo_CashCust.Items.FindByValue(dr("Cash_Cust").ToString()).Selected = True
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
            If Not ddlCustType.FindItemByValue(dr("Customer_Type").ToString()) Is Nothing Then
                ddlCustType.ClearSelection()
                ddlCustType.FindItemByValue(dr("Customer_Type").ToString()).Selected = True

            End If


            If Not ddlCustClass.FindItemByValue(dr("Customer_Class").ToString()) Is Nothing Then
                ddlCustClass.ClearSelection()
                ddlCustClass.FindItemByValue(dr("Customer_Class").ToString()).Selected = True

            End If

            txt_collectiongroup.Text = dr("Custom_Attribute_5").ToString
            txt_trn.Text = dr("VAT_REGISTRATION").ToString

            If Not rdo_GenericCash.Items.FindByValue(dr("GENERIC_CASH").ToString()) Is Nothing Then
                rdo_GenericCash.ClearSelection()
                rdo_GenericCash.Items.FindByValue(dr("GENERIC_CASH").ToString()).Selected = True
            End If

                If Rdo_CashCust.SelectedItem.Value = "Y" Then

                    rdo_GenericCash.Enabled = False
                    If ENABLE_CREDIT_FOR_CASH_CUSTOMER = "Y" Then
                        reqCr1.Visible = True
                        reqCr2.Visible = True
                        req3.Visible = True
                    Else
                        reqCr1.Visible = False
                        reqCr2.Visible = False
                        req3.Visible = False
                    End If

                Else
                    reqCr1.Visible = True
                    reqCr2.Visible = True
                    req3.Visible = True
                    rdo_GenericCash.Enabled = False
                End If

            If Rdo_CashCust.SelectedItem.Value = "Y" And rdo_GenericCash.SelectedItem.Value = "Y" Then
                txt_CreditLimit.Enabled = False
                txt_availBalance.Enabled = False
                txt_CreditPeriod.Enabled = False
            ElseIf Rdo_CashCust.SelectedItem.Value = "Y" And rdo_GenericCash.SelectedItem.Value <> "Y" Then
                ObjCommon = New SalesWorx.BO.Common.Common
                Dim Enable_def_CL_Cash_cust = ObjCommon.GetAppConfig(Err_No, Err_Desc, "ENABLE_DEF_CREDIT_LIMIT_CASH_CUST").ToUpper()
                Dim Def_Credit_Limit = ObjCommon.GetAppConfig(Err_No, Err_Desc, "DEFUALT_CREDIT_LIMIT_CASH_CUST").ToUpper()

                If Enable_def_CL_Cash_cust = "Y" Then
                    txt_CreditLimit.Enabled = True
                    txt_availBalance.Enabled = True

                Else
                    txt_CreditLimit.Enabled = True
                    txt_availBalance.Enabled = True
                End If


                txt_CreditPeriod.Enabled = True
            ElseIf Rdo_CashCust.SelectedItem.Value = "N" Then
                txt_CreditLimit.Enabled = True
                txt_availBalance.Enabled = True
                txt_CreditPeriod.Enabled = True
            End If

            If rdo_GenericCash.SelectedValue = "Y" Then
                rdo_GenericCash.Enabled = True
            End If

            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Sub loadAllVans()
        Try

            Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        ObjCommon = New SalesWorx.BO.Common.Common
        Dim VanDt As New DataTable
        VanDt = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, objUserAccess.UserID.ToString())
        VanList.DataSource = VanDt
        VanList.DataValueField = "SalesRep_ID"
        VanList.DataTextField = "SalesRep_Name"
            VanList.DataBind()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Sub LoadOrganization()
        Try
            ObjCommon = New SalesWorx.BO.Common.Common
            Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
            ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
            ddlOrganization.DataBind()
            ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))
            If ddlOrganization.Items.Count = 2 Then
                ddlOrganization.SelectedIndex = 1
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Sub LoadPriceList()
        Try
            ObjCommon = New SalesWorx.BO.Common.Common
            Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
            ddl_Pricelist.DataSource = (New SalesWorx.BO.Common.Price).GetPriceListHeader(Err_No, Err_Desc, "", "")
            ddl_Pricelist.DataTextField = "Description"
            ddl_Pricelist.DataValueField = "Price_List_ID"
            ddl_Pricelist.DataBind()
            ddl_Pricelist.Items.Insert(0, New ListItem("Select Price List", "0"))

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Sub LoadShipAddress()
        Try
            Dim dtShip As New DataTable
            dtShip = (New SalesWorx.BO.Common.Customer).GetCustomerShipAddress(Err_No, Err_Desc, Customer_ID.Value, txt_filterName.Text, txt_filterNo.Text, ddlOrganization.SelectedValue)
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
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Sub LoadCustomerSegment()
        Try
            ddl_Segment.DataSource = (New SalesWorx.BO.Common.CustomerSegment).SearchCustomerSegmentGrid(Err_No, Err_Desc, "", "")
            ddl_Segment.DataTextField = "Description"
            ddl_Segment.DataValueField = "Customer_Segment_ID"
            ddl_Segment.DataBind()
            ddl_Segment.Items.Insert(0, New RadComboBoxItem("Select Segment", "0"))
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Sub LoadSalesDistricts()
        Try
            ddl_SalesDistrict.DataSource = (New SalesWorx.BO.Common.SalesDistrict).SearchSalesDistrictGrid(Err_No, Err_Desc, "", "")
            ddl_SalesDistrict.DataTextField = "Description"
            ddl_SalesDistrict.DataValueField = "Sales_District_ID"
            ddl_SalesDistrict.DataBind()
            ddl_SalesDistrict.Items.Insert(0, New RadComboBoxItem("Select Sales District", "0"))
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Sub LoadCustomerType()
        Try
            ObjCommon = New SalesWorx.BO.Common.Common


            ddlCustType.DataSource = ObjCommon.LoadCustomerType(Err_No, Err_Desc)
            ddlCustType.DataBind()
            ddlCustType.Items.Insert(0, New RadComboBoxItem("Select A Customer Type", "0"))
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Sub LoadCustomerClass()
        Try
            ObjCommon = New SalesWorx.BO.Common.Common
            ddlCustClass.DataSource = ObjCommon.LoadCustomerClass(Err_No, Err_Desc)
            ddlCustClass.DataBind()
            ddlCustClass.Items.Insert(0, New RadComboBoxItem("Select A Customer Class", "0"))
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
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
        Try
            lbl_ship_msg.Text = ""
            Dim dtShip As New DataTable
            dtShip = (New SalesWorx.BO.Common.Customer).GetCustomerShipAddress(Err_No, Err_Desc, Customer_ID.Value, "", "", ddlOrganization.SelectedValue)
            LocCount = dtShip.Rows.Count

            'If LocCount < 1 Then
            clearShipFileds()
            OptShip.Value = "1"
            Me.MapWindow.VisibleOnPageLoad = True
            'ScriptManager.RegisterStartupScript(Me, GetType(String), "EnableVanlist", "EnableVanlist();", True)
            If FSR_CUST_REL.Value.ToUpper = "Y" Then
                VanlistTokenBox.Visible = True
            Else

                VanlistTokenBox.Visible = False
            End If
            'Else
            '     Me.MapWindow.VisibleOnPageLoad = False
            '     MessageBoxValidation("You Cannot add more than 1 ship address")
            'End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Sub clearShipFileds()
        Try

        
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
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Sub LoadShipFileds()
        Try
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
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnAdd.Click
        Try
            If ValidateInputCustomer() Then
                ObjCustomer = New SalesWorx.BO.Common.Customer
                Dim customerID As String
                Dim SiteUseID As String

                customerID = Customer_ID.Value
                SiteUseID = SiteUse_ID.Value


                If Rdo_CashCust.SelectedItem.Value = "Y" Then
                    If HCust_Type.Value = "CREDIT" Then
                        Dim Dt_outstanding As New DataTable
                        Dt_outstanding = ObjCommon.CheckCustOutStanding(Err_No, Err_Desc, Customer_ID.Value, SiteUse_ID.Value)
                        If Dt_outstanding.Rows.Count > 0 Then
                            If CDec(Dt_outstanding.Rows(0)(0).ToString()) <> 0 Then
                                MessageBoxValidation("Pending Outstanding is there So Customer  can not change  Credit to Cash", "Information")
                                Exit Sub
                            End If

                        End If
                    End If

                End If



                If ObjCustomer.SaveCustomer(Err_No, Err_Desc, opt.Value, customerID, SiteUseID, txt_CusName.Text.Trim(), txt_CusNo.Text.Trim, txt_Contact.Text.Trim, txt_CustLocation.Text.Trim, txt_Custaddress.Text.Trim, txt_CustCity.Text.Trim, txt_phone.Text.Trim, Rdo_CashCust.SelectedItem.Value, txt_CreditPeriod.Text.Trim(), txt_CreditLimit.Text.Trim, txt_availBalance.Text.Trim, rdo_CreditHold.SelectedItem.Value, ddl_Pricelist.SelectedItem.Value, ddlOrganization.SelectedItem.Value, IIf(ddlCustType.SelectedItem.Value = "0", "", ddlCustType.SelectedItem.Value), IIf(ddlCustClass.SelectedItem.Value = "0", "", ddlCustClass.SelectedItem.Value), txt_collectiongroup.Text, rdo_GenericCash.SelectedItem.Value, txt_trn.Text) = True Then
                    If opt.Value = "1" Then
                        objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "CUSTOMER", txt_CusName.Text.Trim(), "Code: " & txt_CusNo.Text.Trim & "/ Cash Cust :  " & Rdo_CashCust.SelectedItem.Value & "/ Price List:  " & ddl_Pricelist.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                    Else
                        objLogin.SaveUserLog(Err_No, Err_Desc, "U", "MAIN MENU", "CUSTOMER", txt_CusName.Text.Trim(), "Code: " & txt_CusNo.Text.Trim & "/ Cash Cust :  " & Rdo_CashCust.SelectedItem.Value & "/ Price List:  " & ddl_Pricelist.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                    End If

                    opt.Value = "2"
                    Customer_ID.Value = customerID
                    SiteUse_ID.Value = SiteUseID
                    MessageBoxValidation("Customer Saved successfully", "Information")
                    BtnAddShip.Visible = True
                Else
                    If Err_Desc <> "" Then
                        MessageBoxValidation(Err_Desc, "Information")
                    Else
                        MessageBoxValidation("Error while saving customer. Please contact administrator", "Information")
                    End If
                    BtnAddShip.Visible = False
                End If
            End If

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Function ValidateInputCustomer() As Boolean
        Try
            ObjCommon = New SalesWorx.BO.Common.Common
            Dim bretval As Boolean = True
            If ddlOrganization.SelectedItem.Value = "0" Then
                MessageBoxValidation("Please Select the Organization ", "Validation")
                bretval = False
                Return bretval
            End If
            If txt_CusNo.Text.Trim = "" Then
                MessageBoxValidation("Please Enter the Customer No", "Validation")
                bretval = False
                Return bretval
            End If
            If txt_CusName.Text.Trim = "" Then
                MessageBoxValidation("Please Enter the Customer Name", "Validation")
                bretval = False
                Return bretval
            End If

            Dim ControlVal = ObjCommon.GetAppConfig(Err_No, Err_Desc, "ENABLE_CUST_PRICE").ToUpper()
            If ControlVal = "Y" Then
                If ddl_Pricelist.SelectedItem.Value = "0" Then
                    MessageBoxValidation("Please Select the Price List", "Validation")
                    bretval = False
                    Return bretval
                End If
            End If
            If Rdo_CashCust.SelectedItem.Value = "N" Then

                If txt_CreditLimit.Text.Trim = "" Then
                    MessageBoxValidation("Please Enter the Credit Limit", "Validation")
                    bretval = False
                    Return bretval
                End If

                If txt_CreditPeriod.Text.Trim = "" Then
                    MessageBoxValidation("Please Enter the Credit Period", "Validation")
                    bretval = False
                    Return bretval
                End If

                If txt_availBalance.Text.Trim = "" Then
                    MessageBoxValidation("Please Enter the Available Balance", "Validation")
                    bretval = False
                    Return bretval
                End If
            Else
                If ENABLE_CREDIT_FOR_CASH_CUSTOMER = "Y" Then
                    If rdo_GenericCash.SelectedValue = "N" Then
                        If txt_CreditLimit.Text.Trim = "" Then
                            MessageBoxValidation("Please Enter the Credit Limit", "Validation")
                            bretval = False
                            Return bretval
                        End If

                        If txt_CreditPeriod.Text.Trim = "" Then
                            MessageBoxValidation("Please Enter the Credit Period", "Validation")
                            bretval = False
                            Return bretval
                        End If

                        If txt_availBalance.Text.Trim = "" Then
                            MessageBoxValidation("Please Enter the Available Balance", "Validation")
                            bretval = False
                            Return bretval
                        End If
                    End If

                End If
            End If

            If txt_CreditLimit.Text.Trim <> "" Then
                If IsNumeric(txt_CreditLimit.Text.Trim) = False Then
                    MessageBoxValidation("Please Enter valid Credit Limit", "Validation")
                    bretval = False
                    Return bretval
                End If
            End If
            If txt_CreditPeriod.Text.Trim <> "" Then
                If IsNumeric(txt_CreditPeriod.Text.Trim) = False Then
                    MessageBoxValidation("Please Enter valid Credit Perid", "Validation")
                    bretval = False
                    Return bretval
                End If
            End If
            If txt_availBalance.Text.Trim <> "" Then
                If IsNumeric(txt_availBalance.Text.Trim) = False Then
                    MessageBoxValidation("Please Enter valid Available Balance", "Validation")
                    bretval = False
                    Return bretval
                End If
            End If
            Return bretval
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Function

    Sub MessageBoxConfirm(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadConfirm(str, "confirmCallbackFn", 300, 200, Nothing, "Confirm")
        Exit Sub
    End Sub

    Protected Sub btnSaveship_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveship.Click
        Try
            ScriptManager.RegisterStartupScript(Me, GetType(String), "EnableVanlist", "EnableVanlist();", True)

            If ValidInputCustomerShipAddress() Then

                Dim vans As String = ""

                If FSR_CUST_REL.Value.ToUpper = "Y" Then
                    VanlistTokenBox.Visible = True
                    For Each item As RadListBoxItem In VanList.Items
                        If item.Checked Then
                            vans = vans & item.Value & ","
                        End If
                    Next
                    If vans.Trim <> "" Then
                        vans = vans.Substring(0, vans.Length - 1)
                    End If
                Else
                    VanlistTokenBox.Visible = False
                End If
                ObjCustomer = New SalesWorx.BO.Common.Customer
                Dim customerID As String
                Dim SiteUseID As String

                customerID = Customer_ID.Value
                SiteUseID = SiteUse_IDShip.Value

                If ObjCustomer.SaveCustomerShipAddress(Err_No, Err_Desc, OptShip.Value, customerID, SiteUseID, Txt_ShipCustName.Text.Trim(), Txt_ShipCustNo.Text.Trim, Txt_ShipAddress.Text.Trim, Txt_ShipPO.Text.Trim, Txt_ShipCity.Text.Trim, ddl_Segment.SelectedItem.Value, txt_ShipLat.Text.Trim, txt_ShipLong.Text.Trim, "Y", ddlOrganization.SelectedItem.Value, ddl_SalesDistrict.SelectedItem.Value, vans, Txt_ShipLocation.Text, "0", txt_BeaconUUID.Text, txt_BeaconMajor.Text, txt_BeaconMinor.Text) = True Then
                    If OptShip.Value = "1" Then
                        objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "SHIP ADDRESS", Txt_ShipCustName.Text.Trim(), "Code: " & txt_CusNo.Text.Trim & "Status:Y", CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                    Else
                        objLogin.SaveUserLog(Err_No, Err_Desc, "U", "MAIN MENU", "SHIP ADDRESS", Txt_ShipCustName.Text.Trim(), "Code: " & txt_CusNo.Text.Trim & "Status:Y", CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                    End If


                    SiteUse_IDShip.Value = SiteUseID
                    LoadShipAddress()
                    UpdatePanel1.Update()
                    Panel.Update()
                    MessageBoxValidation("Ship Address saved successfully", "Information")

                    ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "CloseWindow();", True)
                    ScriptManager.RegisterStartupScript(Me, GetType(String), "EnableVanlist", "EnableVanlist();", True)


                Else
                    If Err_Desc <> "" Then
                        lbl_ship_msg.Text = Err_Desc
                    Else
                        lbl_ship_msg.Text = "Error while saving customer. Please contact administrator"
                    End If
                    ScriptManager.RegisterStartupScript(Me, GetType(String), "EnableVanlist", "EnableVanlist();", True)
                End If

            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Protected Sub lbEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            lbl_ship_msg.Text = ""
            Dim btnEdit As LinkButton = TryCast(sender, LinkButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
            Dim LblCustomer_ID As System.Web.UI.WebControls.Label = row.FindControl("lblCustomer_ID")
            Dim LblSite_ID As System.Web.UI.WebControls.Label = row.FindControl("lblSite_Use_ID_Ship")
            OptShip.Value = "2"
            SiteUse_IDShip.Value = LblSite_ID.Text
            LoadShipAddressDetails(LblCustomer_ID.Text, LblSite_ID.Text)
            Me.MapWindow.VisibleOnPageLoad = True

            UpdatePanel1.Update()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Protected Sub lbChangeStatus_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim btnChangeStatus As LinkButton = TryCast(sender, LinkButton)
            Dim row As GridViewRow = DirectCast(btnChangeStatus.NamingContainer, GridViewRow)
            Dim LblCustomer_ID As System.Web.UI.WebControls.Label = row.FindControl("lblCustomer_ID")
            Dim LblSite_ID As System.Web.UI.WebControls.Label = row.FindControl("lblSite_Use_ID_Ship")
            Dim LblStatus As System.Web.UI.WebControls.Label = row.FindControl("lblStatus")
            ObjCustomer = New SalesWorx.BO.Common.Customer
            If ObjCustomer.SaveCustomerShipAddress(Err_No, Err_Desc, 3, LblCustomer_ID.Text, LblSite_ID.Text, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "") Then
                If LblStatus.Text = "Y" Then
                    objLogin.SaveUserLog(Err_No, Err_Desc, "U", "MAIN MENU", "SHIP ADDRESS", Txt_ShipCustName.Text.Trim(), "Code: " & txt_CusNo.Text.Trim & "Status:N", CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                    MessageBoxValidation("Ship address disabled Successfully", "Information")
                Else
                    objLogin.SaveUserLog(Err_No, Err_Desc, "U", "MAIN MENU", "SHIP ADDRESS", Txt_ShipCustName.Text.Trim(), "Code: " & txt_CusNo.Text.Trim & "Status:Y", CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                    MessageBoxValidation("Ship address enabled Successfully", "Information")
                End If
            Else
                MessageBoxValidation("Unexpected error occured. Please contact the administrator", "Information")
            End If
            LoadShipAddress()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Protected Sub btnUpdateLoc_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            txt_ShipLat.Text = txtLoc_Latitude.Text
            txt_ShipLong.Text = txtLoc_Long.Text
            map.Visible = False
            details.Visible = True
            UpdatePanel1.Update()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Protected Sub btnCancelLoc_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            map.Visible = False
            details.Visible = True
            UpdatePanel1.Update()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Sub LoadShipAddressDetails(ByVal customerId As String, ByVal SiteID As String)
        Try
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
                If Not ddl_Segment.FindItemByValue(dr("Customer_Segment_ID").ToString()) Is Nothing Then
                    ddl_Segment.ClearSelection()
                    ddl_Segment.FindItemByValue(dr("Customer_Segment_ID").ToString()).Selected = True
                End If
                If Not ddl_SalesDistrict.FindItemByValue(dr("Sales_District_ID").ToString()) Is Nothing Then
                    ddl_SalesDistrict.ClearSelection()
                    ddl_SalesDistrict.FindItemByValue(dr("Sales_District_ID").ToString()).Selected = True
                End If
                If FSR_CUST_REL.Value.ToUpper = "Y" Then

                    VanList.ClearSelection()
                    loadAllVans()
                    loadVans(customerId, SiteID, dr("Custom_Attribute_1").ToString())

                End If
                If FSR_CUST_REL.Value.ToUpper = "Y" Then
                    VanlistTokenBox.Visible = True
                Else

                    VanlistTokenBox.Visible = False
                End If
                'Else

                txt_BeaconUUID.Text = dr("Beacon_UUID").ToString
                txt_BeaconMinor.Text = dr("Beacon_Major").ToString
                txt_BeaconMajor.Text = dr("Beacon_Minor").ToString

            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Sub loadVans(ByVal customerId As String, ByVal SiteID As String, ByVal OrgId As String)
        Try
            If ddlOrganization.SelectedItem.Value = OrgId Then
                Dim dtvan As New DataTable
                dtvan = (New SalesWorx.BO.Common.Customer).GetShipAddressVans(Err_No, Err_Desc, customerId, SiteID)
                For Each dr In dtvan.Rows
                    If Not VanList.FindItemByValue(dr("SalesRep_ID").ToString) Is Nothing Then
                        VanList.FindItemByValue(dr("SalesRep_ID").ToString).Checked = True
                    End If
                Next
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Function ValidInputCustomerShipAddress() As Boolean
        Try
            Dim bretval As Boolean = True
            If Txt_ShipCustName.Text.Trim = "" Then
                lbl_ship_msg.Text = "Please Enter the Customer Name"
                bretval = False
                Me.MapWindow.VisibleOnPageLoad = True
                Return bretval

            End If
            If FSR_CUST_REL.Value.ToUpper = "Y" Then
                If VanList.CheckedItems.Count <= 0 Then
                    lbl_ship_msg.Text = "Please Add the associated Sales persons"
                    bretval = False
                    Me.MapWindow.VisibleOnPageLoad = True
                    Return bretval
                End If
            Else

            End If
            Dim CUST_VAN_DIR_MAP As String = (New SalesWorx.BO.Common.Common).GetAppConfig(Err_No, Err_Desc, "CUST_VAN_DIR_MAP")
            If CUST_VAN_DIR_MAP = "N" Then
                If ddl_Segment.SelectedItem.Value = "0" Then
                    lbl_ship_msg.Text = "Please Select the Customer Segment"
                    Me.MapWindow.VisibleOnPageLoad = True
                    bretval = False
                    Return bretval
                End If
            End If

            If txt_ShipLat.Text.Trim <> "" Then
                If IsNumeric(txt_ShipLat.Text.Trim) = False Then
                    lbl_ship_msg.Text = "Please enter a valid latitude"
                    Me.MapWindow.VisibleOnPageLoad = True
                    bretval = False
                    Return bretval
                End If
            End If
            If txt_ShipLong.Text.Trim <> "" Then
                If IsNumeric(txt_ShipLong.Text.Trim) = False Then
                    lbl_ship_msg.Text = "Please enter a valid longitude"
                    Me.MapWindow.VisibleOnPageLoad = True
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
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Function
    Private Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrganization.SelectedIndexChanged
        Try
            If ddlOrganization.SelectedItem.Value <> "0" Then

                Dim dt As New DataTable

                ObjCommon = New SalesWorx.BO.Common.Common
                dt = ObjCommon.CheckCustNOGeneration(Err_No, Err_Desc)

                If dt.Rows.Count > 0 Then
                    If dt.Rows(0)("Control_Value").ToString().ToUpper().Trim() = "Y" Then
                        Dim dtCustno As New DataTable
                        ObjCustomer = New SalesWorx.BO.Common.Customer
                        dtCustno = ObjCustomer.GetCustomerNo(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
                        ObjCustomer = Nothing
                        If dtCustno.Rows.Count > 0 Then
                            txt_CusNo.Text = dtCustno.Rows(0)(0).ToString
                        End If
                    Else
                        txt_CusNo.Text = ""
                    End If

                End If
            End If


            If FSR_CUST_REL.Value.ToUpper = "Y" Then
                lable1.Visible = True
                VanList.Enabled = True
                VanList.Items.Clear()
                loadAllVans()



            Else
                'lable1.Visible = False
                VanList.Enabled = False
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Protected Sub Rdo_CashCust_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Rdo_CashCust.SelectedIndexChanged
        Try
            If opt.Value = "1" Then
                If Rdo_CashCust.SelectedItem.Value = "Y" Then

                    If ENABLE_CREDIT_FOR_CASH_CUSTOMER = "Y" Then
                        reqCr1.Visible = True
                        reqCr2.Visible = True
                        req3.Visible = True
                    Else
                        reqCr1.Visible = False
                        reqCr2.Visible = False
                        req3.Visible = False
                    End If
                    rdo_GenericCash.Enabled = True
                Else
                    reqCr1.Visible = True
                    reqCr2.Visible = True
                    req3.Visible = True
                    rdo_GenericCash.Enabled = False
                    rdo_GenericCash.SelectedValue = "N"
                End If



                If Rdo_CashCust.SelectedItem.Value = "Y" And rdo_GenericCash.SelectedItem.Value = "Y" Then
                    txt_CreditLimit.Enabled = False
                    txt_availBalance.Enabled = False
                    txt_CreditPeriod.Enabled = False
                ElseIf Rdo_CashCust.SelectedItem.Value = "Y" And rdo_GenericCash.SelectedItem.Value <> "Y" Then

                    If ENABLE_CREDIT_FOR_CASH_CUSTOMER = "Y" Then
                        ObjCommon = New SalesWorx.BO.Common.Common
                        Dim Enable_def_CL_Cash_cust = ObjCommon.GetAppConfig(Err_No, Err_Desc, "ENABLE_DEF_CREDIT_LIMIT_CASH_CUST").ToUpper()
                        Dim Def_Credit_Limit = ObjCommon.GetAppConfig(Err_No, Err_Desc, "DEFUALT_CREDIT_LIMIT_CASH_CUST").ToUpper()

                        If Enable_def_CL_Cash_cust = "Y" Then
                            txt_CreditLimit.Enabled = True
                            txt_availBalance.Enabled = True
                            txt_CreditLimit.Text = Def_Credit_Limit
                            txt_availBalance.Text = Def_Credit_Limit
                        Else
                            txt_CreditLimit.Enabled = True
                            txt_availBalance.Enabled = True
                        End If


                        txt_CreditPeriod.Enabled = True
                    Else
                        txt_CreditLimit.Enabled = False
                        txt_availBalance.Enabled = False
                        txt_CreditPeriod.Enabled = False
                    End If

                ElseIf Rdo_CashCust.SelectedItem.Value = "N" Then
                    txt_CreditLimit.Enabled = True
                    txt_availBalance.Enabled = True
                    txt_CreditPeriod.Enabled = True

                    txt_CreditLimit.Text = ""
                    txt_availBalance.Text = ""
                End If
            Else
                txt_CreditLimit.Enabled = False
                txt_availBalance.Enabled = False
                If Rdo_CashCust.SelectedValue = "Y" Then
                    txt_CreditPeriod.Enabled = False
                Else
                    txt_CreditPeriod.Enabled = True
                End If
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
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



    Private Sub rdo_GenericCash_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rdo_GenericCash.SelectedIndexChanged
        Try
            If opt.Value = "1" Then
                If rdo_GenericCash.SelectedItem.Value = "Y" Then

                    txt_CreditLimit.Text = ""
                    txt_availBalance.Text = ""
                    txt_CreditLimit.Enabled = False
                    txt_availBalance.Enabled = False
                    txt_CreditPeriod.Enabled = False
                    req3.Visible = False
                    reqCr1.Visible = False
                    reqCr2.Visible = False
                    'If ENABLE_CREDIT_FOR_CASH_CUSTOMER.ToUpper().Trim() = "Y" Then
                    '    If ENABLE_CREDIT_FOR_CASH_CUSTOMER.ToUpper().Trim() = "Y" And Rdo_CashCust.SelectedItem.Value = "Y" And rdo_GenericCash.SelectedItem.Value = "Y" Then
                    '        txt_CreditLimit.Enabled = False
                    '        txt_availBalance.Enabled = False
                    '        txt_CreditPeriod.Enabled = False
                    '        req3.Visible = False
                    '        reqCr1.Visible = False
                    '        reqCr2.Visible = False
                    '        txt_CreditLimit.Text = ""
                    '        txt_availBalance.Text = ""
                    '    ElseIf ENABLE_CREDIT_FOR_CASH_CUSTOMER.ToUpper().Trim() = "Y" And Rdo_CashCust.SelectedItem.Value = "Y" And rdo_GenericCash.SelectedItem.Value <> "Y" Then
                    '        ObjCommon = New SalesWorx.BO.Common.Common
                    '        Dim Enable_def_CL_Cash_cust = ObjCommon.GetAppConfig(Err_No, Err_Desc, "ENABLE_DEF_CREDIT_LIMIT_CASH_CUST").ToUpper()
                    '        Dim Def_Credit_Limit = ObjCommon.GetAppConfig(Err_No, Err_Desc, "DEFUALT_CREDIT_LIMIT_CASH_CUST").ToUpper()

                    '        If Enable_def_CL_Cash_cust = "Y" Then
                    '            txt_CreditLimit.Enabled = False
                    '            txt_availBalance.Enabled = False
                    '            txt_CreditLimit.Text = Def_Credit_Limit
                    '            txt_availBalance.Text = Def_Credit_Limit
                    '        Else
                    '            txt_CreditLimit.Enabled = True
                    '            txt_availBalance.Enabled = True
                    '        End If
                    '        txt_CreditPeriod.Enabled = True
                    '        req3.Visible = True
                    '        reqCr1.Visible = True
                    '        reqCr2.Visible = True
                    '    End If
                    'Else
                    '    req3.Visible = False
                    '    reqCr1.Visible = False
                    '    reqCr2.Visible = False
                    '    txt_CreditLimit.Enabled = False
                    '    txt_availBalance.Enabled = False
                    '    txt_CreditPeriod.Enabled = False
                    'End If
                Else
                    If ENABLE_CREDIT_FOR_CASH_CUSTOMER.ToUpper().Trim() = "Y" Then
                        ObjCommon = New SalesWorx.BO.Common.Common
                        Dim Enable_def_CL_Cash_cust = ObjCommon.GetAppConfig(Err_No, Err_Desc, "ENABLE_DEF_CREDIT_LIMIT_CASH_CUST").ToUpper()
                        Dim Def_Credit_Limit = ObjCommon.GetAppConfig(Err_No, Err_Desc, "DEFUALT_CREDIT_LIMIT_CASH_CUST").ToUpper()

                        If Enable_def_CL_Cash_cust = "Y" Then
                            txt_CreditLimit.Enabled = True
                            txt_availBalance.Enabled = True
                            txt_CreditLimit.Text = Def_Credit_Limit
                            txt_availBalance.Text = Def_Credit_Limit
                        Else
                            txt_CreditLimit.Enabled = True
                            txt_availBalance.Enabled = True
                        End If
                        txt_CreditPeriod.Enabled = True
                        req3.Visible = True
                        reqCr1.Visible = True
                        reqCr2.Visible = True
                    Else
                        req3.Visible = False
                        reqCr1.Visible = False
                        reqCr2.Visible = False
                        txt_CreditLimit.Enabled = False
                        txt_availBalance.Enabled = False
                        txt_CreditPeriod.Enabled = False
                    End If
                End If
            Else
                txt_CreditLimit.Enabled = False
                txt_availBalance.Enabled = False
                If Rdo_CashCust.SelectedValue = "Y" Then
                    txt_CreditPeriod.Enabled = False
                Else
                    txt_CreditPeriod.Enabled = True
                End If
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
End Class
