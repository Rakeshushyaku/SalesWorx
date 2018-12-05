Imports SalesWorx.BO.Common
Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports Telerik.Web.UI

Partial Public Class ReviewRoutePlan_View
    Inherits System.Web.UI.Page

    Dim objProduct As New Product
    Dim objCommon As New Common
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const PageID As String = "P79"
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim oGeocodeList As List(Of String)
    Private oGeocodeList1 As List(Of String)
    Private Customer As String = Nothing
    Private Supervisor As String = Nothing
    Private VDate As String = Nothing
    Private ShowCustomer As String = Nothing
    Private LastDate As String = Nothing
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub FSRVisitTracking_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Review RoutePlan Report"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        Try
            If Not IsPostBack Then
                Dim HasPermission As Boolean = False
                ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
                If Not HasPermission Then
                    Err_No = 500
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
                End If

                hfPlanID.Value = Request.QueryString("FSRPlanID")
                rgVisits.Rebind()

                lbl_custmapmsg.Text = "Map plotted the customers who have valid geo coordinates."
                Dim objRoute As New RoutePlan
                Dim dt_default As New DataTable
                dt_default = objRoute.GetFSRPlanStartEnd_Map(Err_No, Err_Desc, hfPlanID.Value)

                If dt_default.Rows.Count > 0 Then
                    txtVisitDate.MinDate = CDate(dt_default.Rows(0)("Start_Date"))
                    txtVisitDate.MaxDate = CDate(dt_default.Rows(0)("End_Date"))
                    txtVisitDate.SelectedDate = CDate(dt_default.Rows(0)("Start_Date"))
                    BindMapData("")
                End If
               

            End If
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_MSL_005") & "&next=Welcome.aspx&Title=Product List Setup", False)
        End Try

    End Sub
   


   
   
    Private Sub BindMapData(ByVal callfrom As String)

        divmap.Visible = True
        Dim cmd As SqlCommand = Nothing
        Dim conn As New SqlConnection()
        Dim objDA As New SqlDataAdapter()
        Dim dt As New DataTable()
        Dim oMessageList As New List(Of String)()
        Dim temp_geocode As String = ""
        Dim temp_mapinfo As String
        Try
            oGeocodeList = New List(Of String)()

            conn.ConnectionString = ConfigurationSettings.AppSettings("SQLConnString")
            cmd = New SqlCommand("app_GetFSRVisitStatus_Map", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Connection = conn


            cmd.Parameters.AddWithValue("@FSRPlanID", hfPlanID.Value)
            cmd.Parameters.AddWithValue("@VisitDate", CDate(txtVisitDate.SelectedDate).ToString("dd-MMM-yyyy"))



            objDA.SelectCommand = cmd
            conn.Open()
            objDA.Fill(dt)
            If dt.Rows.Count <= 0 Then
                lbl_errMsg.Text = "No Visits on this date ."
                BindEmptymap()
                '  MessageBoxValidation("No Visits done on this date .", "Information")
                Exit Sub

            End If

            rgCustomer.DataSource = dt
            rgCustomer.DataBind()

            Dim TobeDistinct As String() = {"Latitude", "Longitude"}
            Dim dtDistinct As DataTable = GetDistinctRecords(dt, TobeDistinct)

            Dim dtlist As New DataTable()
            dtlist.Columns.Add("RowID")
            dtlist.Columns.Add("Details")
            dtlist.Columns.Add("Latitude")
            dtlist.Columns.Add("Longitude")
            dtlist.Columns.Add("Visit_Start_Date")
            dtlist.Columns.Add("Visit_End_Date")

            Dim st As Boolean = False
            Dim i As Integer = 0
            Dim pin As String = ""
            Dim totcount As Integer = 0
            Dim line As Integer = 1

            Dim Error_des As String = ""
            Dim Error_No As Long

            For Each prows As DataRow In dtDistinct.Rows
                Dim r As DataRow = dtlist.NewRow()

                r("Latitude") = prows("Latitude").ToString()
                r("Longitude") = prows("Longitude").ToString()
                Dim SelRows() As DataRow
                SelRows = dt.Select(" Latitude='" & prows("Latitude").ToString() & "' and Longitude='" & prows("Longitude").ToString() & "'")

                Dim str As String = "<table  cellspacing='0' cellpadding='0' border='0' class='table'><tr><th style='text-align:center'><b>Sequence</b></th><th style='text-align:center'><b>Customer</b></th><th style='text-align:center'><b>Visit Date</b></th></tr>"
                If SelRows.Length > 0 Then
                    For Each dr As DataRow In SelRows
                        str = str & "<tr><td style='padding: 2px;'>" & dr("Visit_Sequence").ToString() & "</td><td style='padding: 2px;'>" & dr("Customer_Name").ToString() & "</td><td style='padding: 2px;'>" & CDate(dr("Visit_Date").ToString()).ToString("dd-MMM-yyyy hh:mm tt") & "</td></tr>"
                    Next
                End If
                str = str & "</table>"
                r("Details") = str

                dtlist.Rows.Add(r)
            Next

            Dim dtFinal As New DataTable()
            dtFinal.Columns.Add("RowID")
            dtFinal.Columns.Add("Latitude")
            dtFinal.Columns.Add("Longitude")
            dtFinal.Columns.Add("Visit_Start_Date")
            dtFinal.Columns.Add("Visit_End_Date")
            i = 0
            Dim Customer As String = ""

            For Each t As DataRow In dt.Rows
                If i = 0 Then
                    Customer = t("Customer_Name")
                    Dim drnew As DataRow
                    drnew = dtFinal.NewRow()
                    drnew("Latitude") = t("Latitude")
                    drnew("Longitude") = t("Longitude")
                    drnew("Visit_Start_Date") = t("Visit_Date")
                    drnew("Visit_End_Date") = t("Visit_Date")
                    dtFinal.Rows.Add(drnew)
                End If
                If i > 0 Then
                    If Customer <> t("Customer_Name") Then
                        Customer = t("Customer_Name")
                        Dim drnew As DataRow
                        drnew = dtFinal.NewRow()
                        drnew("Latitude") = t("Latitude")
                        drnew("Longitude") = t("Longitude")
                        drnew("Visit_Start_Date") = t("Visit_Date")
                        drnew("Visit_End_Date") = t("Visit_Date")
                        dtFinal.Rows.Add(drnew)
                    End If
                End If
                i = i + 1
            Next
            Dim Time As String = ""
            Dim seq As String = ""
            i = 0
            For Each t As DataRow In dtFinal.Rows
                Time = ""
                If CDec(t("Latitude").ToString()) > 0 And CDec(t("Longitude").ToString()) > 0 Then
                    If i < dtFinal.Rows.Count - 1 Then
                        Dim nextvisit = dtFinal.Rows(i + 1)("Visit_Start_Date").ToString()
                        Time = DateDiff(DateInterval.Second, CDate(t("Visit_Start_Date")), CDate(nextvisit))
                    End If
                    If i = 0 Then
                        seq = "S"
                    ElseIf (i + 1) = dtFinal.Rows.Count Then
                        seq = "E"
                    Else
                        seq = "I"
                    End If
                    temp_geocode = " '" & t("Latitude").ToString() & "," & t("Longitude").ToString() & "," & Time & "," & seq & "'"
                    seq = ""
                    oGeocodeList.Add(temp_geocode)
                    Dim selrows() As DataRow
                    selrows = dtlist.Select(" Latitude='" & t("Latitude").ToString() & "' and Longitude='" & t("Longitude").ToString() & "'")
                    If selrows.Length > 0 Then
                        temp_mapinfo = " '<span class=formatText>" & selrows(0)("Details").ToString().Replace("'", "") & "</span>' "
                        oMessageList.Add(temp_mapinfo)
                    End If
                End If
                i = i + 1
            Next

            Dim message As [String] = String.Join(",", oMessageList.ToArray())
            Dim geocodevalues = String.Join(",", oGeocodeList.ToArray())

            rap.ResponseScripts.Add("locationList = undefined; message = undefined;")

            ScriptManager.RegisterArrayDeclaration(Me, "locationList", geocodevalues)
            ScriptManager.RegisterArrayDeclaration(Me, "message", message)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Intialization1", "clearOverlays();", True)



        Catch ex As Exception
            'lblError.Text = ex.Message
            log.Error(ex.Message.ToString())
        Finally
            conn.Close()
            cmd.Dispose()
        End Try


    End Sub
    Public Shared Function GetDistinctRecords(ByVal dt As DataTable, ByVal Columns As String()) As DataTable
        Dim dtUniqRecords As New DataTable()
        dtUniqRecords = dt.DefaultView.ToTable(True, Columns)
        Return dtUniqRecords
    End Function
    Private Sub BindEmptymap()

        divmap.Visible = False
    End Sub


    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Try
            lbl_errMsg.Text = ""
            rgCustomer.DataSource = Nothing
            rgCustomer.DataBind()
            BindMapData("")
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub

  
End Class