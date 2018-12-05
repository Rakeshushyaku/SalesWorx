Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Partial Public Class Rep_MonthlySales
    Inherits System.Web.UI.Page

  Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "MonthlySoldvsRetunedReport"

    Private Const PageID As String = "P318"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single

    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private Sub RepRouteMaster_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
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
                ObjCommon = New SalesWorx.BO.Common.Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataTextField = "Description"
                ddlOrganization.DataValueField = "MAS_Org_ID"
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New ListItem("-- Select a value --"))

              LoadMonth()
              loadYear()


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
    Protected Sub Chk_Organization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        If Not (ddlOrganization.SelectedItem.Value = "-- Select a value --") Then

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ObjCommon = New SalesWorx.BO.Common.Common()


            ddl_Van.DataTextField = "SalesRep_Name"
            ddl_Van.DataValueField = "SalesRep_ID"
            ddl_Van.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
            ddl_Van.Items.Insert(0, New ListItem("(Select)", "0"))
            ddl_Van.DataBind()



        Else
            ddl_Van.Items.Clear()
        End If
        RVMain.Reset()

    End Sub
     Sub LoadMonth()
      ddl_Month.Items.Clear()
      For i As Integer = 1 To 12
          ddl_Month.Items.Add(New ListItem(CDate(i.ToString & "/01/" & Now.Year).ToString("MMM"), i))
      Next
    End Sub
    Sub loadYear()
        Dim objCommon As New Common

                ddl_year.DataSource = objCommon.GetYearforMonthlySales(Err_No, Err_Desc)
                ddl_year.DataTextField = "Yr"
                ddl_year.DataValueField = "Yr"
                ddl_year.DataBind()
                objCommon = Nothing
    End Sub
    Protected Sub SearchBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SearchBtn.Click

        If ddlOrganization.SelectedIndex <= 0 Then
            MessageBoxValidation("Select organization.")
            Exit Sub
        End If

        If ddl_Van.SelectedItem.Value = "0" Then
            MessageBoxValidation("Please select a van.")
            Exit Sub
        End If



        Dim Fromdate As String
        Fromdate = CDate(ddl_Month.SelectedItem.Value & "/01/" & ddl_year.SelectedItem.Value).ToString("dd-MMM-yyyy")

        Dim Todate As String
        Todate = DateAdd(DateInterval.Second, -1, DateAdd(DateInterval.Month, 1, CDate(ddl_Month.SelectedItem.Value & "/01/" & ddl_year.SelectedItem.Value))).ToString("dd-MMM-yyyy")


        'Dim OrgID As New ReportParameter
        'OrgID = New ReportParameter("OID", ddlOrganization.SelectedValue)

        Dim VanID As New ReportParameter
        VanID = New ReportParameter("SID", ddl_Van.SelectedItem.Value)

        Dim Start_Date As New ReportParameter
        Start_Date = New ReportParameter("FromDate", Fromdate)

        Dim End_Date As New ReportParameter
        End_Date = New ReportParameter("Todate", Todate)

        Dim OrgID As New ReportParameter
        OrgID = New ReportParameter("OrgID", ddlOrganization.SelectedValue)


        With RVMain
            .Reset()
            .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
            .ServerReport.SetParameters(New ReportParameter() {VanID, End_Date, OrgID, Start_Date})
            '.ServerReport.Refresh()
            .Visible = True
        End With
    End Sub
    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        MpInfoError.Show()
        Exit Sub
    End Sub

End Class