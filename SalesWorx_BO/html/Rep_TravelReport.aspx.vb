Imports SalesWorx.BO.Common
Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports Telerik.Web.UI

Partial Public Class Rep_TravelReport
    Inherits System.Web.UI.Page

    Dim objProduct As New Product
    Dim objCommon As New Common
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const PageID As String = "P301"
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
        Me.Title = "Travel Report"
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

                Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddOraganisation.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddOraganisation.Items.Clear()
                ddOraganisation.Items.Add("Select Organization")
                ddOraganisation.AppendDataBoundItems = True
                ddOraganisation.DataValueField = "MAS_Org_ID"
                ddOraganisation.DataTextField = "Description"
                ddOraganisation.DataBind()

                ''  RadCalendar1.SelectedDate = Now().Date

                If ddOraganisation.Items.Count = 2 Then
                    ddOraganisation.SelectedIndex = 1

                    LoadVan()

                End If

                Session.Remove("ListDates")
                Session.Remove("SelDate")


                'tblSummary.Visible = False
            End If
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_MSL_005") & "&next=Welcome.aspx&Title=Product List Setup", False)
        End Try

    End Sub
    Sub LoadVan()
        Dim objUserAccess As UserAccess
        Try
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            objCommon = New SalesWorx.BO.Common.Common()
            ddlVan.DataSource = objCommon.GetVanByOrg(Err_No, Err_Desc, ddOraganisation.SelectedValue, objUserAccess.UserID)
            ddlVan.DataValueField = "SalesRep_ID"
            ddlVan.DataTextField = "SalesRep_Name"
            ddlVan.DataBind()

            ddlVan.Items.Insert(0, New RadComboBoxItem("Select a van/FSR"))

        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            objUserAccess = Nothing
            objCommon = Nothing
        End Try
    End Sub
     

    Protected Sub chkSelected_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Try



            If Me.ddlVan.SelectedIndex <= 0 Or Me.ddOraganisation.SelectedIndex <= 0 Or Me.RadCalendar1.SelectedDates.Count <= 0 Then


                BindEmptymap()
                MessageBoxValidation("Please select a organization,van and visit date", "Validation")
                Exit Sub

            End If
            BindMapData("Filter")



        Catch ex As Exception
            Err_No = "62521"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("PHX_006") & "&next=FSRVisitTracking.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub
    Protected Sub RadCalendar2_DayRender(sender As Object, e As Telerik.Web.UI.Calendar.DayRenderEventArgs)
        Try
            If Not Session("ListDates") Is Nothing Then
                Dim s As DataTable = CType(Session("ListDates"), DataTable)
                For Each d As DataRow In s.Rows
                    If DateTime.Parse(d(0).ToString()) = e.Day.Date Then
                        Dim currentCell As TableCell = e.Cell
                        currentCell.Style("background-color") = "#F8E360"
                    End If
                Next
            End If
        Catch ex As Exception
            log.Error(ex.Message)
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
            cmd = New SqlCommand("Rep_TravelReport", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Connection = conn


            cmd.Parameters.AddWithValue("@FSRID", IIf(Me.ddlVan.SelectedIndex <= 0, "-1", Me.ddlVan.SelectedValue))
            cmd.Parameters.AddWithValue("@VisitDate", CDate(RadCalendar1.SelectedDate).ToString("dd-MMM-yyyy"))


            objDA.SelectCommand = cmd
            conn.Open()
            objDA.Fill(dt)
            If dt.Rows.Count <= 0 Then
                BindEmptymap()
                MessageBoxValidation("No Visits done on this date .", "Information")
                Exit Sub

            End If
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

                Dim str As String = "<table  cellspacing='0' cellpadding='0' border='0' class='table'><tr><th style='text-align:center'><b>Customer</b></th><th style='text-align:center'><b>Visit Start Date</b></th><th style='text-align:center'><b>Visit End Date</b></th></tr>"
                    If SelRows.Length > 0 Then
                        For Each dr As DataRow In SelRows
                                str = str & "<tr><td style='padding: 2px;'>" & dr("Customer").ToString() & "</td><td style='padding: 2px;'>" & CDate(dr("Visit_Start_Date").ToString()).ToString("dd-MMM-yyyy hh:mm tt") & "</td><td style='padding: 2px;'>" & CDate(dr("Visit_End_Date").ToString()).ToString("dd-MMM-yyyy hh:mm tt") & "</td></tr>"
                        Next
                    End If
                    str = str & "</table>"
                    r("Details") = str

                    dtlist.Rows.Add(r)
            Next



           

            'Dim drGPs2 As DataRow
            'drGPs2 = dtlist.NewRow()
            'drGPs2("Latitude") = "25.281954"
            'drGPs2("Longitude") = "55.331955"
            'drGPs2("Details") = "Dubai"
            'dtlist.Rows.Add(drGPs2)

            'Dim drGPs3 As DataRow
            'drGPs3 = dtlist.NewRow()
            'drGPs3("Latitude") = "25.091818"
            'drGPs3("Longitude") = "55.367661"
            'drGPs3("Details") = "Dubai-1"
            'dtlist.Rows.Add(drGPs3)


            'Dim drGPs As DataRow
            'drGPs = dtlist.NewRow()
            'drGPs("Latitude") = "24.984813"
            'drGPs("Longitude") = "55.351181"
            'drGPs("Details") = "Airport"
            'dtlist.Rows.Add(drGPs)


            'Dim drGPs1 As DataRow
            'drGPs1 = dtlist.NewRow()
            'drGPs1("Latitude") = "25.331614"
            'drGPs1("Longitude") = "55.375207"
            'drGPs1("Details") = "Al Nahdha"
            'dtlist.Rows.Add(drGPs1)

            'Dim drGPs5 As DataRow
            'drGPs5 = dtlist.NewRow()
            'drGPs5("Latitude") = "25.331614"
            'drGPs5("Longitude") = "55.375207"
            'drGPs5("Details") = "Al Nahdha"
            'dtlist.Rows.Add(drGPs5)
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
                Customer = t("Customer")
                Dim drnew As DataRow
                drnew = dtFinal.NewRow()
                drnew("Latitude") = t("Latitude")
                drnew("Longitude") = t("Longitude")
                 drnew("Visit_Start_Date") = t("Visit_Start_Date")
                drnew("Visit_End_Date") = t("Visit_End_Date")
                dtFinal.Rows.Add(drnew)
            End If
            If i > 0 Then
                If Customer <> t("Customer") Then
                    Customer = t("Customer")
                    Dim drnew As DataRow
                    drnew = dtFinal.NewRow()
                    drnew("Latitude") = t("Latitude")
                    drnew("Longitude") = t("Longitude")
                      drnew("Visit_Start_Date") = t("Visit_Start_Date")
                drnew("Visit_End_Date") = t("Visit_End_Date")
                    dtFinal.Rows.Add(drnew)
                End If
            End If
            i = i + 1
            Next
            Dim Time As String = ""
            i = 0
            For Each t As DataRow In dtFinal.Rows
                Time = ""
                If CDec(t("Latitude").ToString()) > 0 And CDec(t("Longitude").ToString()) > 0 Then
                 If i < dtFinal.Rows.Count - 1 Then
                    Dim nextvisit = dtFinal.Rows(i + 1)("Visit_Start_Date").ToString()
                    Time = DateDiff(DateInterval.Second, CDate(t("Visit_Start_Date")), CDate(nextvisit))
                 End If
                    temp_geocode = " '" & t("Latitude").ToString() & "," & t("Longitude").ToString() & "," & Time & "'"

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
        Finally
            conn.Close()
            cmd.Dispose()
        End Try

        'If dt.Rows.Count = 0 And callfrom = "Filter" Then
        '    Me.lblinfo.Text = "Validation"
        '    Me.lblMessage.Text = "There is no data for the searching criteria"
        '    Me.lblMessage.ForeColor = Drawing.Color.Red
        '    Me.MpInfoError.Show()
        '    Exit Sub

        'End If
    End Sub
Public Shared Function GetDistinctRecords(ByVal dt As DataTable, ByVal Columns As String()) As DataTable
    Dim dtUniqRecords As New DataTable()
    dtUniqRecords = dt.DefaultView.ToTable(True, Columns)
    Return dtUniqRecords
End Function

    Protected Sub ddOraganisation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddOraganisation.SelectedIndexChanged

        Try
            raddiv.Visible = False
            divmap.Visible = False
            If Me.ddOraganisation.SelectedIndex <= 0 Then

                BindEmptymap()

            End If
            If ddOraganisation.SelectedItem.Text <> "Select Organization" Then
                ' ''Dim objUserAccess As UserAccess
                ' ''objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

                ' ''ddlVan.DataSource = objCommon.GetVanByOrg(Err_No, Err_Desc, ddOraganisation.SelectedValue, objUserAccess.UserID.ToString())
                ' ''ddlVan.Items.Clear()
                ' ''ddlVan.Items.Add("-- Select a van --")
                ' ''ddlVan.AppendDataBoundItems = True
                ' ''ddlVan.DataValueField = "SalesRep_Id"
                ' ''ddlVan.DataTextField = "SalesRep_Name"
                ' ''ddlVan.DataBind()
                LoadVan()
            Else
                ddlVan.ClearSelection()
                ddlVan.Items.Clear()
                ddlVan.Items.Insert(0, New RadComboBoxItem("Select Van", "0"))

                ''ddlVan.Items.Clear()
                ''ddlVan.Items.Add("-- Select a van --")
                ''ddlVan.AppendDataBoundItems = True
                ''ddlVan.DataValueField = "SalesRep_Id"
                ''ddlVan.DataTextField = "SalesRep_Name"
                ''ddlVan.DataBind()

            End If
            BindEmptymap()
            Session("ListDates") = Nothing
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub BindEmptymap()
        'oGeocodeList = New List(Of [String])()
        'Dim temp_mapinfo
        'Dim temp_geocode As String = ""
        'oGeocodeList = New List(Of [String])()
        'temp_geocode = "'25.000000, 55.000000,0'"
        'oGeocodeList.Add(temp_geocode)
        'Dim oMessageList As New List(Of String)()
        'temp_mapinfo = " '<span class=formatText>Test</span>' "
        'oMessageList.Add(temp_mapinfo)
        'Dim geocodevalues = String.Join(",", oGeocodeList.ToArray())
        'Dim message As [String] = String.Join(",", oMessageList.ToArray())
        ''Dim geocodevalues = String.Join(",", oGeocodeList.ToArray())
        'rap.ResponseScripts.Add("locationList = undefined; message = undefined;")
        'ScriptManager.RegisterArrayDeclaration(Me, "locationList", geocodevalues)
        'ScriptManager.RegisterArrayDeclaration(Me, "message", message)
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Intialization1", "clearOverlays();", True)

        'oGeocodeList = Nothing
        divmap.Visible = False
    End Sub

    Protected Sub ddlVan_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlVan.SelectedIndexChanged
        BindEmptymap()
        raddiv.Visible = False
    End Sub

    
  
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub CustomizeDay(ByVal sender As Object, ByVal e As Telerik.Web.UI.Calendar.DayRenderEventArgs) Handles RadCalendar1.DayRender

        If Not Session("ListDates") Is Nothing Then

            Dim s As DataTable = CType(Session("ListDates"), DataTable)

            For Each d As DataRow In s.Rows
                If DateTime.Parse(d(0).ToString()) = e.Day.Date Then
                    Dim currentCell As TableCell = e.Cell
                    currentCell.Style("background-color") = "#F8E360"
                End If
            Next
        End If

    End Sub 'CustomizeDay
    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click

        Try
            If (ddOraganisation.SelectedItem.Value = "") Then
                MessageBoxValidation("Please select the Organisation", "Validation")
                'tblSummary.Visible = False
                Exit Sub
            End If

            If Me.ddlVan.SelectedIndex <= 0 Then

                MessageBoxValidation("Please select the van", "Validation")
                'tblSummary.Visible = False
                Exit Sub
            End If
            If Me.ddlVan.SelectedIndex > 0 Then
                Dim vd As New DataTable
                vd = objCommon.GetFSRVisitedDates(Err_No, Err_Desc, Me.ddlVan.SelectedValue)

                Session("ListDates") = vd
            Else
                Session("ListDates") = Nothing
            End If
            tblSummary.Visible = True
            raddiv.Visible = True

            lbl_org.Text = ddOraganisation.SelectedItem.Text
            lbl_van.Text = ddlVan.SelectedItem.Text
            If Me.RadCalendar1.SelectedDates.Count > 0 Then
                lbl_from.Text = CDate(Me.RadCalendar1.SelectedDate.Date).ToString("dd-MMM-yyyy")
            Else
                lbl_from.Text = "N/A"
            End If
            Args.Visible = True

            BindEmptymap()
        Catch ex As Exception
            log.Error(ex.Message)
        End Try


    End Sub

    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        Try
            If (ddOraganisation.SelectedItem.Value = "") Then
                MessageBoxValidation("Please select the Organisation", "Validation")

                Return bretval
            ElseIf (ddlVan.SelectedItem.Value = "") Then
                MessageBoxValidation("Please select a van/FSR", "Validation")

                Return bretval
            ElseIf Me.RadCalendar1.SelectedDates.Count <= 0 Then
                MessageBoxValidation("Please select visit date", "Validation")

                Return bretval
                'ElseIf Not isActivitySelected() Then
                '    MessageBoxValidation("Please select activity", "Validation")

                '    Return bretval
            Else
                bretval = True
                Return bretval
            End If
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Function

    'Private Function isActivitySelected() As Boolean
    '    Dim bretval As Boolean = False
    '    For Each item As RadComboBoxItem In ddlOptions.Items
    '        Dim chk As CheckBox = DirectCast(item.FindControl("chk1"), CheckBox)
    '        If chk.Checked Then
    '            bretval = True
    '            Exit For
    '        End If
    '    Next
    '    Return bretval
    'End Function


    Private Sub btnDummy_Click(sender As Object, e As EventArgs) Handles BtnDummy.Click

    End Sub

    Protected Sub RadCalendar1_DefaultViewChanged(sender As Object, e As Calendar.DefaultViewChangedEventArgs) Handles RadCalendar1.DefaultViewChanged
        'If ValidateInputs() = True Then
        '    If Me.RadCalendar1.SelectedDates.Count > 0 Then
        '        If Me.ddlVan.SelectedIndex <= 0 Or Me.ddOraganisation.SelectedIndex <= 0 Or Me.RadCalendar1.SelectedDates.Count <= 0 Then
        RadCalendar1.SelectedDate = Nothing
        Session.Remove("SelDate")
        lbl_org.Text = ddOraganisation.SelectedItem.Text
        lbl_van.Text = ddlVan.SelectedItem.Text
        If Me.RadCalendar1.SelectedDates.Count > 0 Then
            lbl_from.Text = CDate(Me.RadCalendar1.SelectedDate.Date).ToString("dd-MMM-yyyy")
        Else
            lbl_from.Text = "N/A"
        End If
        Args.Visible = True
        BindEmptymap()
        ''  MessageBoxValidation("Please select a organization,van and visit date", "Validation")
        'Exit Sub

        '        End If
        'BindMapData("Filter")
        '    End If
        'Else
        'BindEmptymap()
        'End If
    End Sub

    Protected Sub RadCalendar1_SelectionChanged(sender As Object, e As Calendar.SelectedDatesEventArgs) Handles RadCalendar1.SelectionChanged
        If ValidateInputs() = True Then
            If Me.RadCalendar1.SelectedDates.Count > 0 Then
                If Me.ddlVan.SelectedIndex <= 0 Or Me.ddOraganisation.SelectedIndex <= 0 Or Me.RadCalendar1.SelectedDates.Count <= 0 Then


                    BindEmptymap()
                    MessageBoxValidation("Please select a organization,van and visit date", "Validation")
                    Exit Sub

                End If
                rpbFilter.Items(0).Expanded = False
                BindMapData("Filter")
            End If
        Else
            BindEmptymap()
        End If
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        Args.Visible = False
        ddlVan.ClearSelection()
        ddlVan.Items.Clear()
        ddOraganisation.ClearSelection()
        If ddOraganisation.Items.Count = 2 Then
            ddOraganisation.SelectedIndex = 1

        End If
        LoadVan()
        BindEmptymap()
        divmap.Visible = False
        tblSummary.Visible = False

    End Sub
End Class