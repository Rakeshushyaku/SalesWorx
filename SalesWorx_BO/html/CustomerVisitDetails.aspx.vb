Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO

Partial Public Class CustomerVisitDetails
    Inherits System.Web.UI.Page
    Private Const PageID As String = "P21"
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCustomer As Customer
    'Dim SortField As String = ""
    'Dim SortFieldDtl As String = ""
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
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
    Private Property SortFieldDtl() As String
        Get
            If ViewState("SortColumn1") Is Nothing Then
                ViewState("SortColumn1") = ""
            End If
            Return ViewState("SortColumn1").ToString()
        End Get
        Set(ByVal value As String)
            ViewState("SortColumn1") = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
            Try
                If Not Session.Item("USER_ACCESS") Is Nothing Then
                    If Not HasAuthentication() Then
                        Err_No = 500
                        Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
                    End If
                    Dim ID As String = ""
                    If Not IsNothing(Request.QueryString("visitid")) Then
                        ID = Request.QueryString("visitid").ToString()
                        LoadDetails(ID)

                        Dim VisitID As String = ""
                        Dim Qstring As String = ""
                        If Not IsNothing(Request.Cookies.Item("CVOID")) Then


                            Dim OID As String = Request.Cookies.Item("CVOID").Value
                            Dim SID As String = Request.Cookies.Item("CVSID").Value
                            Dim FromDate As String = Request.Cookies.Item("CVFromDate").Value
                            Dim ToDate As String = Request.Cookies.Item("CVToDate").Value
                            Dim Customer As String = Request.Cookies.Item("CVCustomer").Value

                            Qstring = "?OID=" & OID
                            If SID <> "-- Select a value --" Then
                                Qstring = Qstring & "&SID=" & SID
                            End If
                            If FromDate <> "" Then
                                Qstring = Qstring & "&FD=" & FromDate
                            End If
                            If ToDate <> "" Then
                                Qstring = Qstring & "&TD=" & ToDate
                            End If
                            If Customer <> "-- Select a value --" Then
                                Qstring = Qstring & "&Ct=" & Customer
                            End If

                        End If
                       
                        btnBack.PostBackUrl = "~/html/CustomerVisits.aspx" & Qstring

                    End If
                End If
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
                ErrorResource = Nothing
            End Try
        End If
    End Sub
    Private Function HasAuthentication() As Boolean
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
    End Function
    Sub LoadDetails(ByVal ID As String)
        ObjCustomer = New Customer()
        Try
            Dim dt As New DataTable
            dt = ObjCustomer.GetCustomerVisitDetails(Err_No, Err_Desc, "And A.Actual_Visit_ID='" + ID + "'", "")
            'btnBack.PostBackUrl = "~/html/CustomerVisits.aspx?visitid=" & ID
            btnOrders.PostBackUrl = "~/html/Orders.aspx?visitid=" & ID
            btnReturns.PostBackUrl = "~/html/OrdersReturn.aspx?visitid=" & ID
            btnDistriChk.PostBackUrl = "~/html/DistributionCheckList.aspx?visitid=" & ID
            Dim dr As DataRow = Nothing
            If dt.Rows.Count > 0 Then
                dr = dt.Rows(0)
                lblCustName.Text = dr("Customer_Name").ToString
                btnOrders.PostBackUrl = "~/html/Orders.aspx?visitid=" & ID & "&cust=" & lblCustName.Text
                lblCustID.Text = dr("Customer_ID").ToString()
                lblVisitDate.Text = String.Format("{0:dd/MM/yyyy}", dr("Visit_Start_Date"))
                lblStartedAt.Text = String.Format("{0:t}", dr("Visit_Start_Date"))
                lblEndedAt.Text = String.Format("{0:t}", dr("Visit_End_Date"))
                lblVisitedBy.Text = dr("SalesRep_Name").ToString()
                lblScanned.Text = dr("Scanned_Closing").ToString()
                'lblNotes.Text = dr("Text_Notes").ToString()
                lblOdoReading.Text = dr("Odo_Reading").ToString()
                lblEmpName.Text = dr("Emp_Name").ToString()

                If IsDBNull(dr("CC_Name")) Then
                    If dr("Cash_Customer_ID") <> "0" Then
                        litDynamicPart.Text = AddRow("Associated Credit Customer", dr("Credit_Cust_Name").ToString())
                    End If
                Else
                    litDynamicPart.Text = AddRow("Cash Customer", dr("CC_Name").ToString())
                    litDynamicPart.Text += AddRow("Cash Customer Tel No.", dr("CC_TelNo").ToString())
                End If


                If ObjCustomer.Is_DC_Done(ID) Then
                    btnDistriChk.Enabled = True
                Else
                    btnDistriChk.Enabled = False
                End If

                If ObjCustomer.HasOrder(ID) Then
                    btnOrders.Enabled = True
                Else
                    btnOrders.Enabled = False
                End If

                If ObjCustomer.HasOrderReturn(ID) Then
                    btnReturns.Enabled = True
                Else
                    btnReturns.Enabled = False
                End If
                If Not IsDBNull(dr("Voice_Notes")) Then
                    Dim strOutFile As String
                    strOutFile = ConfigurationSettings.AppSettings("TEMP_WAV_PATH")
                    Dim K As Long
                    K = UBound(dr("Voice_Notes"))
                    Dim WriteFs As New FileStream(strOutFile, FileMode.Create, FileAccess.Write)
                    WriteFs.Write(dr("Voice_Notes"), 0, K)
                    WriteFs.Close()
                End If
            End If
            dr = Nothing
            dt = Nothing
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
    Function AddRow(ByVal key As String, ByVal value As String) As String
        Return " <tr><td class='txtSMBold' colspan='2'>" + key + ":&nbsp; <span class='txtSM'>" + value + " </span></td></tr>"
    End Function
End Class