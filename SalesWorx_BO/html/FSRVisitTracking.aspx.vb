Imports SalesWorx.BO.Common
Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports Telerik.Web.UI

Partial Public Class FSRVisitTracking
    Inherits System.Web.UI.Page
    Dim objProduct As New Product
    Dim objCommon As New Common
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const PageID As String = "P267"
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim oGeocodeList As List(Of String)

    Private Shared _strDefLat As String = ConfigurationSettings.AppSettings("DefaultLat")
    Private Shared _strDefLong As String = ConfigurationSettings.AppSettings("DefaultLong")

    Private oGeocodeList1 As List(Of String)
    Private Customer As String = Nothing
    Private Supervisor As String = Nothing
    Private VDate As String = Nothing
    Private ShowCustomer As String = Nothing
    Private LastDate As String = Nothing
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub FSRVisitTracking_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Visit Tracking Report"
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

                ''Me.ddlVan.Items.Clear()
                ''Me.ddlVan.AppendDataBoundItems = True
                ''Me.ddlVan.Items.Insert(0, "--Select a van--")
                ''Me.ddlVan.Items(0).Value = ""

                ' ''chkVisit.Checked = True
                ' ''chkCollection.Checked = False
                ' ''chkDC.Checked = False
                ' ''chkInvoice.Checked = False
                ' ''chkOrder.Checked = False
                ' ''chkRMA.Checked = False
                Me.hfDefLat.Value = _strDefLat
                Me.hfDefLng.Value = _strDefLong
                Session.Remove("ListDates")
                Session.Remove("SelDate")
                ' BindEmptymap()
                tblSummary.Visible = False
                '' Loading Options Commbo 
                ' LoadOptions()


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

            ddlVan.Items.Insert(0, New RadComboBoxItem("Select a van"))

        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            objUserAccess = Nothing
            objCommon = Nothing
        End Try
    End Sub

    'Private Sub LoadOptions()
    '    Try
    '        '' Buliding DataTable 
    '        Dim optTbl As New DataTable
    '        optTbl.Columns.Add("OptionID", Type.GetType("System.String"))
    '        optTbl.Columns.Add("Description", Type.GetType("System.String"))
    '        optTbl.Columns.Add("ImagePath", Type.GetType("System.String"))
    '        optTbl.Columns.Add("IsChecked", Type.GetType("System.String"))

    '        '' Adding Visits
    '        Dim drow As DataRow = optTbl.NewRow()
    '        drow("OptionID") = "V"
    '        drow("Description") = "Customer Visited"
    '        drow("ImagePath") = "http://maps.google.com/mapfiles/ms/icons/green.png"
    '        drow("IsChecked") = "true"
    '        optTbl.Rows.Add(drow)

    '        '' Adding Invoice
    '        Dim drowInvoice As DataRow = optTbl.NewRow()
    '        drowInvoice("OptionID") = "I"
    '        drowInvoice("Description") = "Invoice"
    '        drowInvoice("ImagePath") = "http://maps.google.com/mapfiles/ms/icons/purple.png"
    '        drowInvoice("IsChecked") = "false"
    '        optTbl.Rows.Add(drowInvoice)

    '        '' Adding Bulk Order
    '        Dim drowBO As DataRow = optTbl.NewRow()
    '        drowBO("OptionID") = "BO"
    '        drowBO("Description") = "Bulk Order"
    '        drowBO("ImagePath") = "http://maps.google.com/mapfiles/ms/icons/yellow.png"
    '        drowBO("IsChecked") = "false"
    '        optTbl.Rows.Add(drowBO)

    '        '' Adding RMA
    '        Dim drowRMA As DataRow = optTbl.NewRow()
    '        drowRMA("OptionID") = "RMA"
    '        drowRMA("Description") = "Credit Notes"
    '        drowRMA("ImagePath") = "http://maps.google.com/mapfiles/ms/icons/orange.png"
    '        drowRMA("IsChecked") = "false"
    '        optTbl.Rows.Add(drowRMA)

    '        '' Adding Distribution Check
    '        Dim drowDC As DataRow = optTbl.NewRow()
    '        drowDC("OptionID") = "DC"
    '        drowDC("Description") = "Distribution Check"
    '        drowDC("ImagePath") = "http://maps.google.com/mapfiles/ms/icons/blue.png"
    '        drowDC("IsChecked") = "false"
    '        optTbl.Rows.Add(drowDC)

    '        '' Adding Collection
    '        Dim drowC As DataRow = optTbl.NewRow()
    '        drowC("OptionID") = "C"
    '        drowC("Description") = "Collection"
    '        drowC("ImagePath") = "http://maps.google.com/mapfiles/ms/icons/pink.png"
    '        drowC("IsChecked") = "false"
    '        optTbl.Rows.Add(drowC)
    '        optTbl.GetChanges()

    '        ddlOptions.DataSource = optTbl
    '        ddlOptions.DataBind()

    '    Catch ex As Exception
    '        log.Error(ex.Message)
    '    End Try
    'End Sub


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

    ' ''Protected Sub CustomizeDay(ByVal sender As Object, ByVal e As Telerik.Web.UI.Calendar.DayRenderEventArgs) Handles RadCalendar1.DayRender

    ' ''    If Not Session("ListDates") Is Nothing Then

    ' ''        Dim s As DataTable = CType(Session("ListDates"), DataTable)

    ' ''        For Each d As DataRow In s.Rows
    ' ''            If DateTime.Parse(d(0).ToString()) = e.Day.Date Then
    ' ''                Dim currentCell As TableCell = e.Cell
    ' ''                currentCell.Style("background-color") = "#F8E360"
    ' ''            End If
    ' ''        Next
    ' ''    End If

    ' ''End Sub 'CustomizeDay
    ' ''Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
    ' ''    resetfields()
    ' ''    chkVisit.Checked = True
    ' ''    chkCollection.Checked = False
    ' ''    chkDC.Checked = False
    ' ''    chkInvoice.Checked = False
    ' ''    chkOrder.Checked = False
    ' ''    chkRMA.Checked = False
    ' ''    BindEmptymap()

    ' ''End Sub

    Private Sub resetfields()

        Me.ddlVan.SelectedIndex = 0

        Session.Remove("ListDates")
        Session("ListDates") = Nothing
        Session.Remove("SelDate")
        Session("SelDate") = Nothing

        Me.RadCalendar1.SelectedDate = Nothing

    End Sub


    '' ''Protected Sub RadCalendar1_SelectionChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.Calendar.SelectedDatesEventArgs) Handles RadCalendar1.SelectionChanged


    '' ''    If Me.RadCalendar1.SelectedDate IsNot Nothing And Me.ddOraganisation.SelectedIndex > 0 And Me.ddlVan.SelectedIndex > 0 Or Not Session("SelDate") Is Nothing Then
    '' ''        If Not Session("SelDate") Is Nothing And Me.RadCalendar1.SelectedDates.Count = 0 Then
    '' ''            Me.RadCalendar1.SelectedDate = Session("SelDate")
    '' ''        End If

    '' ''    Else
    '' ''        Exit Sub
    '' ''    End If

    '' ''End Sub

    'Private Sub RadCalendar1_SelectedDateChanged(sender As Object, e As Calendar.SelectedDateChangedEventArgs) Handles RadCalendar1.SelectedDateChanged
    '    Try

    '        If Me.RadCalendar1.SelectedDate IsNot Nothing And Me.ddOraganisation.SelectedIndex > 0 And Me.ddlVan.SelectedIndex > 0 Or Not Session("SelDate") Is Nothing Then
    '            If Not Session("SelDate") Is Nothing And Me.RadCalendar1.SelectedDate Is Nothing Then
    '                Me.RadCalendar1.SelectedDate = Session("SelDate")
    '            End If

    '            Args.Visible = False
    '            If ValidateInputs() Then
    '                ' ''If Me.ddlVan.SelectedIndex <= 0 Or Me.ddOraganisation.SelectedIndex <= 0 Or Me.RadCalendar1.SelectedDate Is Nothing Then

    '                ' ''    BindEmptymap()

    '                ' ''    MessageBoxValidation("Please select a organization,van and visit date", "Validation")
    '                ' ''    Exit Sub

    '                ' ''End If

    '                BindMapData("Filter")
    '            Else
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Intialization", "HideMap();", True)
    '            End If



    '        Else
    '            Exit Sub
    '        End If
    '    Catch ex As Exception
    '        log.Error(ex.Message)
    '    End Try
    'End Sub

    Private Sub BindMapData(ByVal callfrom As String)

        Session("SelDate") = Me.RadCalendar1.SelectedDate.Date

        Dim cmd As SqlCommand = Nothing
        Dim conn As New SqlConnection()
        Dim objDA As New SqlDataAdapter()
        Dim dt As New DataTable()
        Dim oMessageList As New List(Of String)()
        Dim temp_geocode As String = ""
        Dim temp_mapinfo As String
        Dim ObjReport As New SalesWorx.BO.Common.Reports
        Try
            oGeocodeList = New List(Of String)()

            '' ''conn.ConnectionString = ConfigurationSettings.AppSettings("SQLConnString")
            '' ''cmd = New SqlCommand("Rep_FSRVisitTracking", conn)
            '' ''cmd.CommandType = CommandType.StoredProcedure
            '' ''cmd.Connection = conn

            '' ''cmd.Parameters.AddWithValue("@OrgID", IIf(Me.ddOraganisation.SelectedIndex <= 0, "0", Me.ddOraganisation.SelectedValue))
            '' ''cmd.Parameters.AddWithValue("@SID", IIf(Me.ddlVan.SelectedIndex <= 0, "0", Me.ddlVan.SelectedValue))
            '' ''cmd.Parameters.AddWithValue("@VisitDate", Me.RadCalendar1.SelectedDate)


            '' ''objDA.SelectCommand = cmd
            '' ''conn.Open()
            '' ''objDA.Fill(dt)
            Dim chkVisit As Boolean = False
            Dim chkDC As Boolean = False
            Dim chkInvoice As Boolean = False
            Dim chkOrder As Boolean = False
            Dim chkRMA As Boolean = False
            Dim chkCollection As Boolean = False
            Dim chkBeacon As Boolean = False
            '' Getting Checked Options

            For Each item As GridItem In rgvLegend.Items
                Dim chk As CheckBox = DirectCast(item.FindControl("chkSelected"), CheckBox)
                Dim lblTranCode As Label = DirectCast(item.FindControl("lbTranCode"), Label)
                If chk.Checked Then
                    If lblTranCode.Text = "V" Then
                        chkVisit = True
                    ElseIf lblTranCode.Text = "I" Then
                        chkInvoice = True
                    ElseIf lblTranCode.Text = "B" Then
                        chkOrder = True
                    ElseIf lblTranCode.Text = "R" Then
                        chkRMA = True
                    ElseIf lblTranCode.Text = "D" Then
                        chkDC = True
                    ElseIf lblTranCode.Text = "C" Then
                        chkCollection = True
                    ElseIf lblTranCode.Text = "O" Then
                        chkBeacon = True
                    End If

                End If
            Next

            If chkVisit = False And chkDC = False And chkOrder = False And chkInvoice = False And chkRMA = False And chkCollection = False And chkBeacon = False Then
                MessageBoxValidation("Please select any one activity.", "Validation")
                BindEmptymap()
                Exit Sub
            End If

            dt = ObjReport.GetFSRVisitTracking(Err_No, Err_Desc, IIf(Me.ddOraganisation.SelectedIndex <= 0, "0", Me.ddOraganisation.SelectedValue), Me.RadCalendar1.SelectedDate,
                                   IIf(Me.ddlVan.SelectedIndex <= 0, "0", Me.ddlVan.SelectedValue))


            'If dt.Rows.Count = 0 Then
            '    MessageBoxValidation("There is no data for the searching criteria", "Validation")
            '    BindEmptymap()
            '    Exit Sub
            'End If
            If dt.Rows.Count = 0 And callfrom = "Filter" Then

                MessageBoxValidation("There is no data for the searching criteria", "Validation")
                BindEmptymap()
                Exit Sub

            End If
            Args.Visible = True
            rpbFilter.Items(0).Expanded = False


            lbl_org.Text = ddOraganisation.SelectedItem.Text
            lbl_van.Text = ddlVan.SelectedItem.Text
            lbl_from.Text = CDate(RadCalendar1.SelectedDate).ToString("dd-MMM-yyyy")
            ' lbl_Activity.Text = ddlOptions.Text


           


            Dim dtlist As New DataTable()
            dtlist.Columns.Add("Customer")
            dtlist.Columns.Add("CustID")
            dtlist.Columns.Add("Lat")
            dtlist.Columns.Add("Lng")
            dtlist.Columns.Add("Status")
            dtlist.Columns.Add("SupID")
            dtlist.Columns.Add("RowNo")
            dtlist.Columns.Add("Image")
            dtlist.Columns.Add("Line")
            dtlist.Columns.Add("Mode")
            dtlist.Columns.Add("BeaconDetected")
            Dim st As Boolean = False
            Dim i As Integer = 0
            Dim pin As String = ""
            Dim totcount As Integer = 0
            Dim line As Integer = 1
            For Each dr As DataRow In dt.Rows
                ''If CDec(dr("Lat").ToString()) > 0 And (dr("Mode").ToString() = "Visits" And chkVisit.Checked = True Or dr("Mode").ToString() = "DC" And chkDC.Checked = True Or dr("Mode").ToString() = "Invoice" And chkInvoice.Checked = True Or dr("Mode").ToString() = "BulkOrder" And chkOrder.Checked = True Or dr("Mode").ToString() = "Return" And chkRMA.Checked = True Or dr("Mode").ToString() = "Collection" And chkCollection.Checked = True) Then
                If CDec(dr("Lat").ToString()) > 0 And (dr("Mode").ToString() = "Visits" And chkVisit = True Or dr("Mode").ToString() = "DC" And chkDC = True Or dr("Mode").ToString() = "Invoice" And chkInvoice = True Or dr("Mode").ToString() = "BulkOrder" And chkOrder = True Or dr("Mode").ToString() = "Return" And chkRMA = True Or dr("Mode").ToString() = "Collection" And chkCollection = True Or dr("Mode").ToString() = "Beacon" And chkBeacon = True) Then
                    LastDate = DateTime.Parse(dt.Rows(dt.Rows.Count - 1)("VisitDate").ToString()).ToString("MM-dd-yyyy HH:mm")
                    Dim r As DataRow = dtlist.NewRow()
                    If dr("A_Visit_ID").ToString() <> "N/A" Then
                        For Each dr1 As DataRow In dtlist.Rows

                            If dr1("CustID").ToString() = dr("CustID").ToString() AndAlso dr("Status").ToString() = "C" And dr("A_Visit_ID").ToString() <> "N/A" And dr1("Mode").ToString() = dr("Mode").ToString() And dr("Lat").ToString() = dr1("Lat").ToString() And dr("Lng").ToString() = dr1("Lng").ToString() Then
                                st = True

                                dr1("Customer") = dr1("Customer").ToString().Replace("'", "") & "<tr><td>" & dr("Description").ToString() & "</td><td>" & DateTime.Parse(dr("VisitStartDate").ToString()).ToString("HH:mm") & "</td><td>" & DateTime.Parse(dr("VisitEndDate").ToString()).ToString("HH:mm") & "</td></tr>"

                                dr1("Customer") = dr1("Customer").ToString().Replace("</Table>", "")

                                dr("Lat") = "0"
                                dr("Lng") = "0"

                                Exit For

                            End If
                        Next

                        If st = False And CDec(dr("Lat").ToString()) > 0 And CDec(dr("Lng").ToString()) > 0 Then

                            totcount = totcount + 1
                            If dtlist.Rows.Count >= 1 And dr("A_Visit_ID").ToString() <> "N/A" Then
                                If dtlist.Rows(dtlist.Rows.Count - 1)("Status").ToString() = "C" Then
                                    dtlist.Rows(dtlist.Rows.Count - 1)("Customer") = Convert.ToString(dtlist.Rows(dtlist.Rows.Count - 1)("Customer")) & "</Table>"
                                End If
                            End If

                            If dr("A_Visit_ID").ToString() <> "N/A" Then
                                If dr("Status").ToString() = "C" Then
                                    Dim bd As String = ""
                                    If dr("BeaconDetected").ToString() = "N/A" Then
                                        bd = "N/A"
                                    ElseIf dr("BeaconDetected").ToString() = "1" Then
                                        bd = "Yes"
                                    Else
                                        bd = "No"
                                    End If
                                    r("Customer") = "<p><b>Customer : " & dr("Customer").ToString().Replace("'", "") & "</b></p><table width=""100%"" height=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" class=""table""><thead><tr><th align=""Left"">Purpose</th><th align=""Left"">Start Time</th><th align=""Left"">End Time</th><th>Beacon Detected</th></tr></tbody></tr></thead><tbody><tr><td>" & dr("Description").ToString() & "</td><td>" & DateTime.Parse(dr("VisitStartDate").ToString()).ToString("HH:mm") & "</td><td>" & DateTime.Parse(dr("VisitEndDate").ToString()).ToString("HH:mm") & "</td><td>" & bd & "</td></tr></tbody>"
                                End If
                            End If
                        End If
                    End If

                    If dr("A_Visit_ID").ToString() = "N/A" Then
                        For Each dr1 As DataRow In dtlist.Rows
                            If dr1("CustID").ToString() = dr("CustID").ToString() AndAlso dr("Status").ToString() = "C" And dr("A_Visit_ID").ToString() = "N/A" And dr1("Mode").ToString() = dr("Mode").ToString() And dr("Lat").ToString() = dr1("Lat").ToString() And dr("Lng").ToString() = dr1("Lng").ToString() Then
                                st = True
                                dr1("Customer") = dr1("Customer").ToString().Replace("'", "") & "<tr><td>" & dr("Mode").ToString() & "</td><td>" & IIf(dr("Description").ToString() = "", "N/A", dr("Description").ToString()) & "</td><td>" & DateTime.Parse(dr("VisitStartDate").ToString()).ToString("HH:mm") & "</td></tr>"
                                dr1("Customer") = dr1("Customer").ToString().Replace("</Table>", "")
                                dr("Lat") = "0"
                                dr("Lng") = "0"
                                Exit For

                            End If
                        Next
                        If st = False And CDec(dr("Lat").ToString()) > 0 And CDec(dr("Lng").ToString()) > 0 Then

                            totcount = totcount + 1

                            If dtlist.Rows.Count >= 1 And dr("A_Visit_ID").ToString() = "N/A" Then
                                If dtlist.Rows(dtlist.Rows.Count - 1)("Status").ToString() = "C" Then
                                    dtlist.Rows(dtlist.Rows.Count - 1)("Customer") = Convert.ToString(dtlist.Rows(dtlist.Rows.Count - 1)("Customer")) & "</Table>"
                                End If
                            End If
                            If dr("A_Visit_ID").ToString() = "N/A" Then
                                If dr("Status").ToString() = "C" Then
                                    r("Customer") = "<p><b>Customer : " & dr("Customer").ToString().Replace("'", "") & "</b></p><br><table width=""100%"" height=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" class=""table""><thead><tr><th align=""Left"">Purpose</th><th align=""Left"">Description</th><th align=""Left"">Date</th></tr></thead><tbody><tr><td>" & dr("Mode").ToString() & "</td><td>" & IIf(dr("Description").ToString() = "", "N/A", dr("Description").ToString()) & "</td><td>" & DateTime.Parse(dr("VisitStartDate").ToString()).ToString("HH:mm") & "</td><td>Beacon Detected</td></tr></tbody>"
                                End If
                            End If
                        End If
                    End If
                    r("CustID") = dr("CustID").ToString()
                    r("Lat") = dr("Lat").ToString()
                    r("lng") = dr("lng").ToString()
                    r("Status") = dr("Status").ToString()
                    r("SupID") = dr("SupID").ToString()
                    r("RowNo") = i
                    r("Mode") = dr("Mode").ToString()
                    r("BeaconDetected") = dr("BeaconDetected").ToString()

                    If dr("Mode").ToString() = "Visits" Then

                        pin = "http://maps.google.com/mapfiles/ms/icons/green.png"

                    ElseIf dr("Mode").ToString() = "Invoice" Then
                        pin = "http://maps.google.com/mapfiles/ms/icons/purple.png"
                    ElseIf dr("Mode").ToString() = "Return" Then
                        pin = "http://maps.google.com/mapfiles/ms/icons/orange.png"

                    ElseIf dr("Mode").ToString() = "DC" Then
                        pin = "http://maps.google.com/mapfiles/ms/icons/blue.png"

                    ElseIf dr("Mode").ToString() = "Collection" Then
                        pin = "http://maps.google.com/mapfiles/ms/icons/pink.png"
                    ElseIf dr("Mode").ToString() = "BulkOrder" Then
                        pin = "http://maps.google.com/mapfiles/ms/icons/yellow.png"
                    ElseIf dr("Mode").ToString() = "Beacon" Then
                        pin = "http://maps.google.com/mapfiles/ms/icons/red.png"
                    End If

                    r("Image") = pin
                    r("Line") = line

                    dtlist.Rows.Add(r)
                End If
                st = False
                pin = ""
                i += 1

            Next

            For Each t As DataRow In dtlist.Rows

                If CDec(t("Lat").ToString()) > 0 And CDec(t("Lng").ToString()) > 0 Then

                    temp_geocode = " '" & t("Lat").ToString() & "," & t("Lng").ToString() & ", " & t("Image").ToString() & "," & t("Line").ToString() & "," & t("Status").ToString() & "," & t("SupID").ToString() & "," & t("RowNo").ToString()

                    If t("BeaconDetected").ToString() = "N/A" Then
                        temp_geocode = temp_geocode & ",N/A"
                    ElseIf t("BeaconDetected").ToString() = "1" Then
                        temp_geocode = temp_geocode & ",Yes"
                    Else
                        temp_geocode = temp_geocode & ",No"
                    End If
                    temp_geocode = temp_geocode & "'"

                    oGeocodeList.Add(temp_geocode)

                    temp_mapinfo = " '<span class=formatText>" & t("Customer").ToString().Replace("'", "") & "</span>' "
                    oMessageList.Add(temp_mapinfo)
                End If
            Next

            If dtlist.Rows.Count = 0 And callfrom = "Filter" Then

                MessageBoxValidation("There is no data for the searching criteria", "Validation")
                BindEmptymap()
                Exit Sub

            End If


            Dim message As [String] = String.Join(",", oMessageList.ToArray())
            Dim geocodevalues = Nothing
            geocodevalues = String.Join(",", oGeocodeList.ToArray())

            ''ClientScript.RegisterArrayDeclaration("locationList", geocodevalues)
            ''ClientScript.RegisterArrayDeclaration("message", message)
            rap.ResponseScripts.Add("locationList = undefined; message = undefined;")

            ScriptManager.RegisterArrayDeclaration(Me, "locationList", geocodevalues)
            ScriptManager.RegisterArrayDeclaration(Me, "message", message)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Intialization1", "clearOverlays();", True)




        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            'conn.Close()
            'cmd.Dispose()
            ObjReport = Nothing
        End Try
   
        
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Protected Sub ddOraganisation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddOraganisation.SelectedIndexChanged

        loadOrgDetails()
    End Sub
    Sub loadOrgDetails()
        Try
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
        Try
            oGeocodeList = New List(Of [String])()
            Dim temp_mapinfo
            Dim temp_geocode As String = ""
            oGeocodeList = New List(Of [String])()
            temp_geocode = " '" & _strDefLat & "," & _strDefLong & ",""'"
            oGeocodeList.Add(temp_geocode)
            Dim oMessageList As New List(Of String)()
            temp_mapinfo = " '<span class=formatText></span>' "
            oMessageList.Add(temp_mapinfo)
            Dim geocodevalues = String.Join(",", oGeocodeList.ToArray())
            Dim message As [String] = String.Join(",", oMessageList.ToArray())
            'Dim geocodevalues = String.Join(",", oGeocodeList.ToArray())
            rap.ResponseScripts.Add("locationList = undefined; message = undefined;")
            ScriptManager.RegisterArrayDeclaration(Me, "locationList", geocodevalues)
            ScriptManager.RegisterArrayDeclaration(Me, "message", message)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Intialization1", "clearOverlays();", True)


            oGeocodeList = Nothing
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
       
    End Sub
    'Protected Sub ddlVan_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlVan.SelectedIndexChanged
    '    Try
    '        If Me.ddlVan.SelectedIndex <= 0 Then
    '            BindEmptymap()
    '        End If
    '        If Me.ddlVan.SelectedIndex > 0 Then
    '            Dim vd As New DataTable
    '            vd = objCommon.GetFSRVisitedDates(Err_No, Err_Desc, Me.ddlVan.SelectedValue)

    '            Session("ListDates") = vd
    '        Else
    '            Session("ListDates") = Nothing
    '        End If

    '        BindEmptymap()
    '    Catch ex As Exception
    '        log.Error(ex.Message)
    '    End Try
    'End Sub

    '' ''Protected Sub chkVisit_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkVisit.CheckedChanged
    '' ''    Try
    '' ''        If Me.ddlVan.SelectedIndex <= 0 Then
    '' ''            BindEmptymap()
    '' ''        End If
    '' ''        If Me.ddlVan.SelectedIndex > 0 Then
    '' ''            Dim vd As New DataTable
    '' ''            vd = objCommon.GetFSRVisitedDates(Err_No, Err_Desc, Me.ddlVan.SelectedValue)

    '' ''            Session("ListDates") = vd
    '' ''        Else
    '' ''            Session("ListDates") = Nothing
    '' ''        End If

    '' ''        BindEmptymap()
    '' ''    Catch ex As Exception
    '' ''        log.Error(ex.Message)
    '' ''    End Try
    '' ''End Sub
    '' ''Protected Sub chkDC_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDC.CheckedChanged
    '' ''    Try
    '' ''        If Me.ddlVan.SelectedIndex <= 0 Or Me.ddOraganisation.SelectedIndex <= 0 Then

    '' ''            BindEmptymap()

    '' ''            MessageBoxValidation("Please select a organization/van", "Validation")
    '' ''            Exit Sub

    '' ''        End If
    '' ''        BindMapData("Filter")
    '' ''    Catch ex As Exception
    '' ''        log.Error(ex.Message)
    '' ''    End Try
    '' ''End Sub

    '' ''Protected Sub chkOrder_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkOrder.CheckedChanged
    '' ''    Try
    '' ''        If Me.ddlVan.SelectedIndex <= 0 Or Me.ddOraganisation.SelectedIndex <= 0 Then

    '' ''            BindEmptymap()

    '' ''            MessageBoxValidation("Please select a organization/van", "Validation")

    '' ''            Exit Sub

    '' ''        End If
    '' ''        BindMapData("Filter")
    '' ''    Catch ex As Exception
    '' ''        log.Error(ex.Message)
    '' ''    End Try

    '' ''End Sub

    '' ''Protected Sub chkCollection_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCollection.CheckedChanged
    '' ''    Try
    '' ''        If Me.ddlVan.SelectedIndex <= 0 Or Me.ddOraganisation.SelectedIndex <= 0 Then

    '' ''            BindEmptymap()

    '' ''            MessageBoxValidation("Please select a organization/van", "Validation")

    '' ''            Exit Sub

    '' ''        End If
    '' ''        BindMapData("Filter")
    '' ''    Catch ex As Exception
    '' ''        log.Error(ex.Message)
    '' ''    End Try

    '' ''End Sub

    '' ''Protected Sub chkInvoice_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkInvoice.CheckedChanged
    '' ''    Try
    '' ''        If Me.ddlVan.SelectedIndex <= 0 Or Me.ddOraganisation.SelectedIndex <= 0 Then

    '' ''            BindEmptymap()

    '' ''            MessageBoxValidation("Please select a organization/van", "Validation")
    '' ''            Exit Sub

    '' ''        End If
    '' ''        BindMapData("Filter")
    '' ''    Catch ex As Exception
    '' ''        log.Error(ex.Message)
    '' ''    End Try


    '' ''End Sub
    '' ''Protected Sub chkRMA_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkRMA.CheckedChanged

    '' ''    Try
    '' ''        If Me.ddlVan.SelectedIndex <= 0 Or Me.ddOraganisation.SelectedIndex <= 0 Then

    '' ''            BindEmptymap()
    '' ''            MessageBoxValidation("Please select a organization/van", "Validation")

    '' ''            Exit Sub

    '' ''        End If
    '' ''        BindMapData("Filter")
    '' ''    Catch ex As Exception
    '' ''        log.Error(ex.Message)
    '' ''    End Try


    '' ''End Sub
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
                tblSummary.Visible = False
                Exit Sub
            End If

            If Me.ddlVan.SelectedIndex <= 0 Then

                MessageBoxValidation("Please select the van", "Validation")
                tblSummary.Visible = False
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
            rpbFilter.Items(0).Expanded = False
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
                MessageBoxValidation("Please select a van", "Validation")

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

 
    Private Sub btnDummy_Click(sender As Object, e As EventArgs) Handles btnDummy.Click

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
                BindMapData("Filter")
            End If
        Else
            BindEmptymap()
        End If
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddOraganisation.ClearSelection()
        If ddOraganisation.Items.Count = 2 Then
            ddOraganisation.SelectedIndex = 1
        End If
        ddlVan.ClearSelection()
        ddlVan.Items.Clear()
        Args.Visible = False
        tblSummary.Visible = False
        loadOrgDetails
    End Sub
End Class