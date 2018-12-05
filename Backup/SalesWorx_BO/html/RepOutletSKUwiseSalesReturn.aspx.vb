Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports System.Globalization

Partial Public Class RepOutletSKUwiseSalesReturn
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "OutletSKUWiseSalesReturn"

    Private Const PageID As String = "P223"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single

    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub RepOutletSKUwiseSalesReturn_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
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


                ddlAgency.DataSource = ObjCommon.GetAgencyList(Err_No, Err_Desc, ddlOrganization.SelectedValue)
                ddlAgency.DataBind()
                ddlAgency.Items.Insert(0, New ListItem("-- Select a value --", "0"))

                txtFromDate.Text = Format(Now().Date, "dd-MMM-yyyy")
                txtToDate.Text = Format(Now().Date, "dd-MMM-yyyy")

                Dim OrgID As New ReportParameter
                Dim OrgName As New ReportParameter
                OrgID = New ReportParameter("OrgID", ddlOrganization.SelectedItem.Value)
                OrgName = New ReportParameter("OrgName", ddlOrganization.SelectedItem.Text)

                Dim objUserAccess As UserAccess
                objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
                ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
                ddlVan.DataTextField = "SalesRep_Name"
                ddlVan.DataValueField = "SalesRep_ID"
                ddlVan.DataBind()

                ''  InitReportViewer()

                If Not Err_Desc Is Nothing Then
                    log.Error(Err_Desc)
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
                End If


            Catch ex As Exception
                Err_No = "74166"
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
    Private Sub InitReportViewer()
        Try


            Dim FDate As New ReportParameter

            If txtFromDate.Text.Trim() IsNot String.Empty Then
                FDate = New ReportParameter("FromDate", txtFromDate.Text.Trim())
            Else
                FDate = New ReportParameter("FromDate", Now.ToString("dd-MMM-yyyy"))
            End If


            Dim TDate As New ReportParameter
            If txtToDate.Text.Trim() IsNot String.Empty Then
                TDate = New ReportParameter("ToDate", txtToDate.Text)
            Else
                TDate = New ReportParameter("ToDate", Now.ToString("dd-MMM-yyyy"))
            End If


            Dim SiteID As New ReportParameter
            Dim Cid As String = Nothing
            Dim collectionCid As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddlCustomer.CheckedItems


            For Each li As Telerik.Web.UI.RadComboBoxItem In collectionCid
                Dim Arr As Array = li.Value.Split("$")
                If String.IsNullOrEmpty(Cid) Then
                    Cid = Arr(0) & "~" & Arr(1)
                Else
                    Cid = Cid & "|" & Arr(0) & "~" & Arr(1)
                End If
            Next

            If String.IsNullOrEmpty(Cid) Then
                SiteID = New ReportParameter("SID", "-1")
            Else
                SiteID = New ReportParameter("SID", Cid & "|")
            End If

            Dim InvID As New ReportParameter
            Dim Invids As String = Nothing
            Dim collection As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddSKU.CheckedItems


            For Each li As Telerik.Web.UI.RadComboBoxItem In collection
                If String.IsNullOrEmpty(Invids) Then
                    Invids = li.Value
                Else
                    Invids = Invids & "|" & li.Value
                End If
            Next
            If String.IsNullOrEmpty(Invids) Then
                InvID = New ReportParameter("InID", "-1")
            Else
                InvID = New ReportParameter("InID", Invids & "|")
            End If

            Dim Agency As New ReportParameter
            Agency = New ReportParameter("Agency", ddlAgency.SelectedItem.Value)

            Dim OrgID As New ReportParameter
            Dim OrgName As New ReportParameter
            OrgID = New ReportParameter("OrgID", ddlOrganization.SelectedItem.Value)
            OrgName = New ReportParameter("OrgName", ddlOrganization.SelectedItem.Text)
            Dim Fsrids As String = Nothing
            Dim collectionFsr As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddlVan.CheckedItems


            For Each li As Telerik.Web.UI.RadComboBoxItem In collectionFsr
                If String.IsNullOrEmpty(Fsrids) Then
                    Fsrids = li.Value
                Else
                    Fsrids = Fsrids & "|" & li.Value
                End If
            Next


            Dim RepID As New ReportParameter
            If String.IsNullOrEmpty(Fsrids) Then
                RepID = New ReportParameter("FSRID", "-1")
            Else
                RepID = New ReportParameter("FSRID", Fsrids & "|")
            End If

            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                .ServerReport.SetParameters(New ReportParameter() {FDate, TDate, SiteID, InvID, OrgID, OrgName, Agency, RepID})
                .ServerReport.Refresh()

            End With


        Catch Ex As Exception
            log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            If ddlOrganization.SelectedValue = "-- Select a value --" Then
                MessageBoxValidation("Select organization.")
                Exit Sub
            End If
            If Not IsDate(txtFromDate.Text) Then
                MessageBoxValidation("Enter a valid from date.")
                Exit Sub
            End If
            If Not IsDate(txtToDate.Text) Then
                MessageBoxValidation("Enter a valid to date.")
                Exit Sub
            End If
            If CDate(txtFromDate.Text) > CDate(txtToDate.Text) Then
                MessageBoxValidation("Start Date should not be greater than End Date.")
                Exit Sub
            End If
            InitReportViewer()
        Catch ex As Exception

        End Try
    End Sub
    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        Exit Sub
    End Sub

    Private Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrganization.SelectedIndexChanged
        Try
            ddlCustomer.DataSource = Nothing
            ddlCustomer.Items.Clear()
            ddSKU.DataSource = Nothing
            ddSKU.Items.Clear()
            If ddlOrganization.SelectedIndex <> 0 Then
                ObjCommon = New Common()
                ddlCustomer.DataSource = ObjCommon.GetOutlet(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
                ddlCustomer.DataTextField = "Outlet"
                ddlCustomer.DataValueField = "CustomerID"
                ddlCustomer.DataBind()


                ddlAgency.DataSource = ObjCommon.GetAgencyList(Err_No, Err_Desc, ddlOrganization.SelectedValue)
                ddlAgency.DataBind()
                ddlAgency.Items.Insert(0, New ListItem("-- Select a value --", "0"))

                ddSKU.DataSource = ObjCommon.GetSKU(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
                ddSKU.DataTextField = "SKU"
                ddSKU.DataValueField = "Inventory_Item_ID"
                ddSKU.DataBind()


                Dim objUserAccess As UserAccess
                objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
                ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
                ddlVan.DataTextField = "SalesRep_Name"
                ddlVan.DataValueField = "SalesRep_ID"
                ddlVan.DataBind()

            Else
                'ddlCustomer.DataSource = Nothing
                'ddlCustomer.Items.Insert(0, New ListItem("-- All --"))
                'ddSKU.DataSource = Nothing
                'ddSKU.Items.Insert(0, New ListItem("-- All --"))
            End If
            RVMain.Reset()
        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            ObjCommon = Nothing
        End Try
    End Sub

    Protected Sub ddlAgency0_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAgency.SelectedIndexChanged
        ObjCommon = New SalesWorx.BO.Common.Common
        ddSKU.DataSource = ObjCommon.GetProductsByOrg_Agency(Err_No, Err_Desc, ddlOrganization.SelectedValue, ddlAgency.SelectedItem.Value)
        ddSKU.DataTextField = "Description"
        ddSKU.DataValueField = "Inventory_Item_ID"
        ddSKU.DataBind()

        RVMain.Reset()
    End Sub
End Class