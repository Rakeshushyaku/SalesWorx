Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Partial Public Class Rep_PriceVariation
    Inherits System.Web.UI.Page
 Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "DocsWithPriceVariation"

    Private Const PageID As String = "P296"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single

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


            ddl_Van.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, CType(Session("User_Access"), UserAccess).UserID)

            ddl_Van.DataValueField = "SalesRep_ID"
            ddl_Van.DataTextField = "SalesRep_Name"
               ddl_Van.DataBind()
            ddl_Van.Items.Insert(0, New ListItem("-- Select --", "0"))


            ddl_customer.DataSource = ObjCommon.GetCustomerfromOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue)

            ddl_customer.Items.Clear()
            ddl_customer.DataValueField = "CustomerID"
            ddl_customer.DataTextField = "Customer"
            ddl_customer.DataBind()
            ddl_Customer.Items.Insert(0, New ListItem("-- Select --", "0$0"))

                ddlSKU.DataSource = Nothing
                ddlSKU.DataSource = ObjCommon.GetSKU(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
                ddlSKU.DataTextField = "SKU"
                ddlSKU.DataValueField = "Inventory_item_ID"
                ddlSKU.DataBind()
                ddlSKU.Items.Insert(0, New ListItem("-- All --", "0"))
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
    Private Sub BindData()

        Dim SalesRepId As Integer = 0
        Dim CustId As Integer = 0
        Dim fromdate As DateTime
        Dim todate As DateTime
        fromdate = System.Data.SqlTypes.SqlDateTime.Null
        todate = System.Data.SqlTypes.SqlDateTime.Null


        Try
            ObjCustomer = New Customer()
            ObjCommon = New Common()
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)



            If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then
                InitReportViewer()
            Else
                MessageBoxValidation("Select an Organization.")
            End If


        Catch ex As Exception
            If Err_Desc IsNot Nothing Then
                log.Error(Err_Desc)
            Else
                log.Error(GetExceptionInfo(ex))
            End If
            Err_No = "74067"
            '  Err_Desc = ex.Message
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
        Finally
            ObjCustomer = Nothing
        End Try
    End Sub

    Private Sub InitReportViewer()
        Try
        RVMain.Reset()
        
         If txtFromDate.Text.Trim() = "" Then

                MessageBoxValidation("Enter a valid from date.")
                Exit Sub
        Else
        If Not IsDate(txtFromDate.Text) Then

                MessageBoxValidation("Enter a valid from date.")
                Exit Sub
            End If
        End If

        If txtToDate.Text.Trim() = "" Then

            MessageBoxValidation("Enter a valid to date.")
            Exit Sub
        Else
                If Not IsDate(txtToDate.Text) Then

                    MessageBoxValidation("Enter a valid to date.")
                    Exit Sub
                End If
        End If


            Dim SalesRepID As New ReportParameter
            SalesRepID = New ReportParameter("FSRID", ddl_Van.SelectedItem.Value)

            Dim FDate As New ReportParameter
            If txtFromDate.Text.Trim() <> "" Then
              FDate = New ReportParameter("Fromdate", txtfromDate.Text)
            Else
              FDate = New ReportParameter("Fromdate")
            End If


            Dim TDate As New ReportParameter
            If txtToDate.Text.Trim() <> "" Then
              TDate = New ReportParameter("ToDate", txtToDate.Text)
            Else
              TDate = New ReportParameter("ToDate")
            End If

            Dim OID As New ReportParameter
            OID = New ReportParameter("OrgID", CStr(ddlOrganization.SelectedValue.ToString()))

            Dim Item As New ReportParameter
            Item = New ReportParameter("Item", ddlSKU.SelectedItem.Value)

            Dim Type As New ReportParameter
            Type = New ReportParameter("Type", ddlType.SelectedItem.Value)

              Dim Selcustomer As String
                Selcustomer = ddl_Customer.SelectedItem.Value
                Dim ids() As String
                ids = Selcustomer.Split("$")

              Dim Customer_ID As New ReportParameter
            Customer_ID = New ReportParameter("Customer_ID", ids(0))

            Dim Site_ID As New ReportParameter
            Site_ID = New ReportParameter("Site_ID", ids(1))

            Dim TypeP As New ReportParameter
            TypeP = New ReportParameter("Type", ddlType.SelectedItem.Value)

            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                .ServerReport.SetParameters(New ReportParameter() {OID, SalesRepID, FDate, TDate, Item, Customer_ID, Site_ID, TypeP})
                .ServerReport.Refresh()

            End With


        Catch Ex As Exception
            '  log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub

    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        RVMain.Reset()
        BindData()
    End Sub
    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        MpInfoError.Show()
        Exit Sub
    End Sub
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged


        If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New Common()
            ddl_Van.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)

            ddl_Van.DataValueField = "SalesRep_ID"
            ddl_Van.DataTextField = "SalesRep_Name"
               ddl_Van.DataBind()
            ddl_Van.Items.Insert(0, New ListItem("-- Select --", "0"))

             ddl_Customer.DataSource = ObjCommon.GetCustomerfromOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue)

            ddl_Customer.Items.Clear()
            ddl_Customer.DataValueField = "CustomerID"
            ddl_Customer.DataTextField = "Customer"
            ddl_Customer.DataBind()
            ddl_Customer.Items.Insert(0, New ListItem("-- Select --", "0$0"))

              ddlSKU.DataSource = Nothing
                ddlSKU.DataSource = ObjCommon.GetSKU(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
                ddlSKU.DataTextField = "SKU"
                ddlSKU.DataValueField = "Inventory_item_ID"
                ddlSKU.DataBind()
                ddlSKU.Items.Insert(0, New ListItem("-- All --", "0"))

            RVMain.Reset()

        End If

    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub
End Class