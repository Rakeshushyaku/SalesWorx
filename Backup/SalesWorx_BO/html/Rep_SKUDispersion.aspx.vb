Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Partial Public Class Rep_SKUDispersion
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "SKUDispersion"

    Private Const PageID As String = "P113"
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

                ddYear.DataSource = ObjCommon.GetYearForRevDispersion(Err_No, Err_Desc)
                ddYear.DataBind()
                ddYear.Items.Insert(0, New ListItem("-- Select a value --"))



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
        Dim SearchQuery As String = ""
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

            Dim collection As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddl_Van.CheckedItems

            Dim van As String = ""
            For Each li As Telerik.Web.UI.RadComboBoxItem In collection
                If van = "" Then
                    van = li.Value
                Else
                    van = van & "," & li.Value
                End If

            Next

            If van = "" Then
                MessageBoxValidation("Select van.")
                Exit Sub
            End If

            If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then
                InitReportViewer(fromdate, todate, CType(Session("User_Access"), UserAccess).UserID, van)
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

    Private Sub InitReportViewer(ByVal fromdate As Date, ByVal Todate As Date, ByVal UID As Integer, ByVal Van As String)
        Try

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)



            Dim USRID As New ReportParameter
            USRID = New ReportParameter("Uid", UID)


            Dim SalesRepID As New ReportParameter
            SalesRepID = New ReportParameter("SID", CStr(Van))

            Dim Year As New ReportParameter
            Year = New ReportParameter("Year", ddYear.SelectedItem.Value)

            Dim Month As New ReportParameter
            Month = New ReportParameter("Month", ddMonth.SelectedItem.Value)


            Dim OID As New ReportParameter
            OID = New ReportParameter("OID", CStr(ddlOrganization.SelectedValue.ToString()))


            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                .ServerReport.SetParameters(New ReportParameter() {USRID, OID, SalesRepID, Year, Month})
                .ServerReport.Refresh()

            End With


        Catch Ex As Exception
            '  log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub

    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        Try
            If IsNothing(Session("USER_ACCESS")) Then
                Response.Redirect("Login.aspx")
                Exit Sub
            End If
            RVMain.Reset()
            If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then

                Dim collection As IList(Of Telerik.Web.UI.RadComboBoxItem) = ddl_Van.CheckedItems

                If collection.Count = 0 Then
                    MessageBoxValidation("Select a van")
                    Exit Sub
                End If

                If ddYear.SelectedItem.Value = "-- Select a value --" Then
                    MessageBoxValidation("Select year")
                    Exit Sub
                End If

                If ddMonth.SelectedItem.Value = "0" Then
                    MessageBoxValidation("Select month")
                    Exit Sub
                End If

                BindData()


            Else
                MessageBoxValidation("Select an organization.")
            End If
        Catch ex As Exception
            log.Debug(ex.Message)
        End Try
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
        Try


            If IsNothing(Session("USER_ACCESS")) Then
                Response.Redirect("Login.aspx")
                Exit Sub
            End If

            If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then

                Dim objUserAccess As UserAccess
                objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
                ObjCommon = New Common()
                ddl_Van.DataTextField = "SalesRep_Name"
                ddl_Van.DataValueField = "SalesRep_ID"
                ddl_Van.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)

                ddl_Van.DataBind()

                RVMain.Reset()
            Else
                ddl_Van.Items.Clear()
                '' ddlVan.Items.Insert(0, New ListItem("-- Select a value --"))
                RVMain.Reset()
            End If
        Catch ex As Exception
            log.Debug(ex.Message)
        End Try
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub

End Class