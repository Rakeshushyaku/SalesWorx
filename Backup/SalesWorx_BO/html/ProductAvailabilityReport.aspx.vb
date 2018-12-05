Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports Microsoft.Reporting.WebForms
Imports System.Configuration.ConfigurationManager

Partial Public Class ProductAvailabilityReport
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Private Const PageID As String = "P107"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private ReportPath As String = "ProductAvailabilityReport"
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
    Public Sub SetReportDetails(ByVal path As String)
        Me.ReportPath = path
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

                If Not Err_Desc Is Nothing Then
                    log.Error(Err_Desc)
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
                End If
            Catch ex As Exception
                Err_No = "74099"
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
 

    Protected Sub SearchBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SearchBtn.Click
        RVMain.Reset()
        If (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then
            MessageBoxValidation("Select an organization.")
            SetFocus(txtFromDate)
            Exit Sub
        End If

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

        If ddlOrganization.SelectedIndex > 0 Then
            RVMain.Visible = True
            InitReportViewer()
        Else
            RVMain.Visible = False
            'txtFromDate.Text = ""
            'txtToDate.Text = ""
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
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then
            ObjCommon = New Common()

            'ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue)
            'ddlVan.DataBind()
            'ddlVan.Items.Insert(0, New ListItem("-- Check All --"))
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ''chkVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID.ToString())
            ''chkVan.DataBind()
            ' ComboBox1.Items.Insert(0, New ListItem("-- Select a value --"))


            ddl_Van.DataTextField = "SalesRep_Name"
            ddl_Van.DataValueField = "SalesRep_ID"
            ddl_Van.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)

            ddl_Van.DataBind()


            LoadAgency()
            LoadProducts()
        Else
            ddl_Van.Items.Clear()
            ddlAgency.Items.Clear()
            ddlAgency.Items.Insert(0, New ListItem("-- Select value --"))
            drpProduct.Items.Clear()
            drpProduct.Items.Insert(0, New ListItem("-- Select value --"))
        End If
        RVMain.Reset()
    End Sub

    Protected Sub ddlVan_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) 'Handles ddlVan.SelectedIndexChanged

        LoadAgency()
        LoadProducts()

    End Sub
 
    Sub LoadProducts()
        ObjCommon = New Common()
        drpProduct.Items.Clear()
        If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then

            drpProduct.DataValueField = "Inventory_Item_ID"
            drpProduct.DataTextField = "Description"
            If (hfVanValue.Value <> "") And (ddlAgency.SelectedItem.Value <> "-- Select a value --") Then
                drpProduct.DataSource = ObjCommon.GetAllProductsByOrg_Van_Agency(Err_No, Err_Desc, ddlOrganization.SelectedValue, hfVanValue.Value, ddlAgency.SelectedValue)
            ElseIf (hfVanValue.Value = "") And (ddlAgency.SelectedItem.Value <> "-- Select a value --") Then
                drpProduct.DataSource = ObjCommon.GetAllProductsByOrg_Agency(Err_No, Err_Desc, ddlOrganization.SelectedValue, ddlAgency.SelectedItem.Value)
            ElseIf (hfVanValue.Value <> "") And (ddlAgency.SelectedItem.Value = "-- Select a value --") Then
                drpProduct.DataSource = ObjCommon.GetAllProductsByOrg_Van(Err_No, Err_Desc, ddlOrganization.SelectedValue, hfVanValue.Value)
            Else
                drpProduct.DataSource = ObjCommon.GetAllProductsByOrgID(Err_No, Err_Desc, ddlOrganization.SelectedValue)
            End If
            'drpProduct.DataSource = ObjCommon.GetAllProductsByOrgID(Err_No, Err_Desc, ddlOrganization.SelectedValue)
            drpProduct.DataBind()
            drpProduct.Items.Insert(0, New ListItem("-- Select value --"))
        End If
    End Sub

    Sub LoadAgency()
        ddlAgency.Items.Clear()
        ObjCommon = New Common()
        If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then
            ddlAgency.DataValueField = "Agency"
            ddlAgency.DataTextField = "Agency"
            If hfVanValue.Value = "" Then
                ddlAgency.DataSource = ObjCommon.GetAllAgencyByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue)
            Else
                ddlAgency.DataSource = ObjCommon.GetAllAgencyByOrg_Van(Err_No, Err_Desc, ddlOrganization.SelectedValue, hfVanValue.Value)

            End If
            ddlAgency.DataBind()
            ddlAgency.Items.Insert(0, New ListItem("-- Select a value --"))



        End If

    End Sub


    Protected Sub ddlAgency_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAgency.SelectedIndexChanged
        RVMain.Reset()
        LoadProducts()
    End Sub
   
    Private Sub InitReportViewer()
        Try


            Dim objUserAccess As New UserAccess()
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

            ' If Not Me.IsPostBack Then
            Dim myParamOrg As New ReportParameter
            Dim myParamVan As New ReportParameter
            Dim myParamAgency As New ReportParameter
            Dim myParamProduct As New ReportParameter
            Dim myParamFromDate As New ReportParameter
            Dim myParamToDate As New ReportParameter
            Dim myParamAvail As New ReportParameter
            Dim myParamUid As New ReportParameter

            myParamAgency.Visible = False
            myParamAvail.Visible = False
            myParamFromDate.Visible = False
            myParamToDate.Visible = False
            myParamOrg.Visible = False
            myParamProduct.Visible = False
            myParamVan.Visible = False

            If ddlOrganization.SelectedIndex > 0 Then
                myParamOrg = New ReportParameter("OID", ddlOrganization.SelectedValue)
            Else
                myParamOrg = New ReportParameter("OID", "0")
            End If
            If hfVanValue.Value <> "" Then
                myParamVan = New ReportParameter("SID", hfVanValue.Value)
            Else
                myParamVan = New ReportParameter("SID", "0")
            End If
            If ddlAgency.SelectedIndex > 0 Then
                myParamAgency = New ReportParameter("Agency", ddlAgency.SelectedValue)
            Else
                myParamAgency = New ReportParameter("Agency", "0")
            End If
            If drpProduct.SelectedIndex > 0 Then
                myParamProduct = New ReportParameter("InvID", drpProduct.SelectedValue)
            Else
                myParamProduct = New ReportParameter("InvID", "0")
            End If

            Dim FDate As New ReportParameter
            FDate = New ReportParameter("FromDate", CDate(txtFromDate.Text).ToString())

            Dim TDate As New ReportParameter
            TDate = New ReportParameter("Todate", CDate(txtToDate.Text).ToString())

            myParamAvail = New ReportParameter("Availability", ddlAvail.SelectedValue)
            If objUserAccess.UserID > 0 Then
                myParamUid = New ReportParameter("Uid", objUserAccess.UserID)
            Else
                myParamUid = New ReportParameter("Uid", "0")
            End If


            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                .ServerReport.SetParameters(New ReportParameter() {myParamOrg, myParamVan, myParamAgency, myParamProduct, FDate, TDate, myParamAvail, myParamUid})
                .ServerReport.Refresh()
                'HideParameter("UserRoleID")


            End With
            '  End If
        Catch Ex As Exception
            log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub
   
 
    'Protected Sub chkVan_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkVan.SelectedIndexChanged
    '    hfVanValue.Value = ""
    '    For Each li As ListItem In chkVan.Items
    '        If li.Selected Then
    '            If hfVanValue.Value = "" Then
    '                hfVanValue.Value = li.Value
    '            Else
    '                hfVanValue.Value = hfVanValue.Value + "," + li.Value
    '            End If

    '        End If
    '    Next
    '    LoadAgency()
    '    LoadProducts()
    'End Sub

    'Protected Sub ddlVan_SelectedIndexChanged1(ByVal sender As Object, ByVal e As EventArgs) Handles ddlVan.SelectedIndexChanged

    '    LoadAgency()
    '    LoadProducts()
    'End Sub

    Private Sub ddl_Van_ItemChecked(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxItemEventArgs) Handles ddl_Van.ItemChecked
        Try
            RVMain.Reset()
            hfVanValue.Value = ""

            Dim collection As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddl_Van.CheckedItems

            For Each li As Telerik.Web.UI.RadComboBoxItem In collection
                If hfVanValue.Value = "" Then
                    hfVanValue.Value = li.Value
                Else
                    hfVanValue.Value = hfVanValue.Value + "," + li.Value
                End If
            Next

            LoadAgency()
            LoadProducts()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ddl_Van_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ddl_Van.SelectedIndexChanged

    End Sub
End Class