Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports System.Collections.Generic

Partial Public Class Rep_ERPBOSyncLog
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "ERPBOSyncLog"

    Private Const PageID As String = "P292"
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
              
                txtFromDate.Text = Format(Now().Date, "dd-MMM-yyyy")
                txtToDate.Text = Format(Now().Date, "dd-MMM-yyyy")

                Me.ddl_ERPTable.DataSource = ObjCommon.GetERPSyncTable(Err_No, Err_Desc)
                Me.ddl_ERPTable.DataTextField = "Value"
                Me.ddl_ERPTable.DataValueField = "Code"
                Me.ddl_ERPTable.DataBind()
                Me.ddl_ERPTable.SelectedIndex = 0

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
            'Else
            '    Me.MapWindow.VisibleOnPageLoad = False
        End If
    End Sub
    Private Sub BindData()

    
        Dim fromdate As DateTime
        Dim todate As DateTime
        fromdate = System.Data.SqlTypes.SqlDateTime.Null
        todate = System.Data.SqlTypes.SqlDateTime.Null


        Try

            ObjCommon = New Common()
         

            fromdate = CDate(txtFromDate.Text)
            todate = CDate(txtToDate.Text)

         
            Dim ERPTable As String = "ALL"
          
         

            InitReportViewer(fromdate, todate)


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

        End Try
    End Sub

    Private Sub InitReportViewer(ByVal fromdate As Date, ByVal Todate As Date)
        Try





            Dim FDate As New ReportParameter
            FDate = New ReportParameter("FromDate", fromdate.ToString())

            Dim TDate As New ReportParameter
            TDate = New ReportParameter("ToDate", Todate.ToString())


            Dim ERPTable As New ReportParameter
            ERPTable = New ReportParameter("ERPTable", CStr(IIf(Me.ddl_ERPTable.SelectedIndex <= 0, "ALL", Me.ddl_ERPTable.SelectedValue)))



            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
                .ServerReport.SetParameters(New ReportParameter() {FDate, TDate, ERPTable})
                .ServerReport.Refresh()

            End With


        Catch Ex As Exception
            '  log.Error(GetExceptionInfo(Ex))
        End Try
    End Sub
    'Protected Sub RVMain_BookmarkNavigation(ByVal sender As Object, ByVal e As Microsoft.Reporting.WebForms.BookmarkNavigationEventArgs) Handles RVMain.BookmarkNavigation
    '    Me.lblPopmsg.Text = ""
    '    Me.lblPopmsg.Text = e.BookmarkId.ToString()

    '    Me.MapWindow.VisibleOnPageLoad = True



    'End Sub
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
  
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub

End Class