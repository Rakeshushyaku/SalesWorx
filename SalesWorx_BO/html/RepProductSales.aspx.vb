Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms

Partial Public Class RepProductSales
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Dim ObjCustomer As Customer
    Private ReportPath As String = "SalesPersonProductwise"
    Dim dv As New DataView
    Private Const PageID As String = "P279"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not IsNothing(Me.Master) Then

            Dim masterScriptManager As ScriptManager
            masterScriptManager = CType(Master.FindControl("ScriptManager1"), ScriptManager)

            ' Make sure our master page has the script manager we're looking for
            If Not IsNothing(masterScriptManager) Then

                ' Turn off partial page postbacks for this page
                masterScriptManager.EnablePartialRendering = False
            End If

        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        If Not IsPostBack Then

            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            If Not HasPermission Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If
            ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
            Try
                ObjCommon = New Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New ListItem("-- Select a value --"))
                txtFromDate.Text = Format(Now().Date, "dd-MMM-yyyy")
                txtToDate.Text = Format(Now().Date, "dd-MMM-yyyy")

                LoadChannel()
                LoadSubChannel()
                LoadRegion()

                If Not Err_Desc Is Nothing Then
                    log.Error(Err_Desc)
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
                End If
            Catch ex As Exception
                Err_No = "74066"
                If Err_Desc Is Nothing Then
                    log.Error(GetExceptionInfo(ex))
                Else
                    log.Error(Err_Desc)
                End If
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
            Finally
                ObjCommon = Nothing
                ErrorResource = Nothing
            End Try
        End If
    End Sub

    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        MpInfoError.Show()
        Exit Sub
    End Sub


   

    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click


        If Not IsDate(txtFromDate.Text) Then
            MessageBoxValidation("Enter valid ""From date"".")
            SetFocus(txtFromDate)
            Exit Sub
        End If

        If Not IsDate(txtToDate.Text) Then
            MessageBoxValidation("Enter valid ""To date"".")
            SetFocus(txtToDate)
            Exit Sub
        End If
        If CDate(txtFromDate.Text) > CDate(txtToDate.Text) Then
            MessageBoxValidation("Start Date should not be greater than End Date.")
            Exit Sub
        End If
        If ddlOrganization.SelectedItem.Value = "-- Select a value --" Then
            MessageBoxValidation("Select an Organization.")
            Exit Sub
        End If
        Dim Fromdate As String
        Dim Todate As String
        Fromdate = txtFromDate.Text
        Todate = txtToDate.Text

        Dim OrgID As New ReportParameter
        OrgID = New ReportParameter("OID", ddlOrganization.SelectedValue)

        Dim VanID As New ReportParameter
        VanID = New ReportParameter("CreatedBy", CStr(IIf(Me.ddlVan.SelectedIndex <= 0, "ALL", ddlVan.SelectedItem.Text)))

        Dim Start_Date As New ReportParameter
        Start_Date = New ReportParameter("FDate", Fromdate)

        Dim End_Date As New ReportParameter
        End_Date = New ReportParameter("TDate", Todate)

        Dim OrgName As New ReportParameter
        OrgName = New ReportParameter("OrgName", ddlOrganization.SelectedItem.Text)

        Dim Customer As New ReportParameter
        Customer = New ReportParameter("Customer", CStr(IIf(Me.ddlCustomer.SelectedIndex <= 0, "ALL", ddlCustomer.SelectedItem.Text)))

        Dim Region As New ReportParameter
        Region = New ReportParameter("Region", CStr(IIf(Me.ddlRegion.SelectedIndex <= 0, "ALL", ddlRegion.SelectedItem.Text)))

        Dim Channel As New ReportParameter
        Channel = New ReportParameter("Channel", CStr(IIf(Me.ddlChannel.SelectedIndex <= 0, "ALL", ddlChannel.SelectedItem.Text)))

        Dim SubChannel As New ReportParameter
        SubChannel = New ReportParameter("SubChannel", CStr(IIf(Me.ddlSubChannel.SelectedIndex <= 0, "ALL", ddlSubChannel.SelectedItem.Text)))

        Dim GroupBy As New ReportParameter
        GroupBy = New ReportParameter("GroupBy", ddlGroupType.SelectedItem.Value)

        Dim Product As New ReportParameter
        Product = New ReportParameter("Product", CStr(IIf(Me.ddlProduct.SelectedIndex <= 0, "ALL", ddlProduct.SelectedItem.Text)))

        With RVMain
            .Reset()
            .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
            .ServerReport.SetParameters(New ReportParameter() {Start_Date, End_Date, OrgName, Customer, Product, Channel, GroupBy, SubChannel, Region, OrgID, VanID})
            '.ServerReport.Refresh()
            .Visible = True
        End With
    End Sub
    Sub LoadCustomer()
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        ObjCommon = New Common()
        Dim x As New DataTable
        x = ObjCommon.GetCustomerByCriteria(Err_No, Err_Desc, IIf(ddlOrganization.SelectedIndex <= 0, "0", Me.ddlOrganization.SelectedValue))

        Dim r As DataRow = x.NewRow()
        r(0) = "0"
        r(1) = "ALL"

        x.Rows.InsertAt(r, 0)
        ddlCustomer.ClearSelection()
        ddlCustomer.Items.Clear()
        ddlCustomer.Text = ""
        ddlCustomer.SelectedIndex = 0
        ddlCustomer.DataValueField = "CustomerID"
        ddlCustomer.DataTextField = "Customer"
        ddlCustomer.DataSource = x
        ddlCustomer.DataBind()

    End Sub

    Sub LoadVan()
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        ObjCommon = New Common()
        Dim x As New DataTable
        x = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())

        Dim r As DataRow = x.NewRow()
        r(0) = "0"
        r(1) = "ALL"

        x.Rows.InsertAt(r, 0)
        ddlVan.ClearSelection()
        ddlVan.Items.Clear()
        ddlVan.Text = ""
        ddlVan.SelectedIndex = 0
        ddlVan.DataValueField = "SalesRep_ID"
        ddlVan.DataTextField = "SalesRep_Name"
        ddlVan.DataSource = x
        ddlVan.DataBind()

    End Sub

    Sub LoadProduct()
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        ObjCommon = New Common()
        Dim x As New DataTable
        x = ObjCommon.GetAllProductsByOrgID(Err_No, Err_Desc, ddlOrganization.SelectedValue)

        Dim r As DataRow = x.NewRow()
        r(0) = "0"
        r(1) = "ALL"

        x.Rows.InsertAt(r, 0)
        ddlProduct.ClearSelection()
        ddlProduct.Items.Clear()
        ddlProduct.Text = ""
        ddlProduct.SelectedIndex = 0
        ddlProduct.DataValueField = "Inventory_Item_ID"
        ddlProduct.DataTextField = "Description"
        ddlProduct.DataSource = x
        ddlProduct.DataBind()

    End Sub
    Sub LoadSubChannel()
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        ObjCommon = New Common()
        Dim x As New DataTable
        x = ObjCommon.GetCustomerClass(Err_No, Err_Desc, "")

        Dim r As DataRow = x.NewRow()
        r(0) = "ALL"


        x.Rows.InsertAt(r, 0)
        ddlSubChannel.ClearSelection()
        ddlSubChannel.Items.Clear()
        ddlSubChannel.Text = ""
        ddlSubChannel.SelectedIndex = 0
        ddlSubChannel.DataValueField = "Customer_Class"
        ddlSubChannel.DataTextField = "Customer_Class"
        ddlSubChannel.DataSource = x
        ddlSubChannel.DataBind()

    End Sub
    Sub LoadChannel()
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        ObjCommon = New Common()
        Dim x As New DataTable
        x = ObjCommon.GetCustomerTypeList(Err_No, Err_Desc, "")

        Dim r As DataRow = x.NewRow()
        r(0) = "ALL"


        x.Rows.InsertAt(r, 0)
        ddlChannel.ClearSelection()
        ddlChannel.Items.Clear()
        ddlChannel.Text = ""
        ddlChannel.SelectedIndex = 0
        ddlChannel.DataValueField = "Customer_Type"
        ddlChannel.DataTextField = "Customer_Type"
        ddlChannel.DataSource = x
        ddlChannel.DataBind()

    End Sub
    Sub LoadRegion()
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        ObjCommon = New Common()
        Dim x As New DataTable
        x = ObjCommon.GetSalesDistrictList(Err_No, Err_Desc, "")

        Dim r As DataRow = x.NewRow()
        r(0) = "0"
        r(1) = "ALL"

        x.Rows.InsertAt(r, 0)
        ddlRegion.ClearSelection()
        ddlRegion.Items.Clear()
        ddlRegion.Text = ""
        ddlRegion.SelectedIndex = 0
        ddlRegion.DataValueField = "Sales_District_ID"
        ddlRegion.DataTextField = "Description"
        ddlRegion.DataSource = x
        ddlRegion.DataBind()

    End Sub
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then
        

            LoadVan()
            LoadCustomer()
            LoadProduct()
        End If
        RVMain.Reset()

    End Sub



  



End Class