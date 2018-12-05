Imports SalesWorx.BO.Common
Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO

Partial Public Class FSRVisitTracking
    Inherits System.Web.UI.Page
    Dim objProduct As New Product
    Dim objCommon As New Common
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const PageID As String = "P267"
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
                ddOraganisation.Items.Add("-- Select a Organization --")
                ddOraganisation.AppendDataBoundItems = True
                ddOraganisation.DataValueField = "MAS_Org_ID"
                ddOraganisation.DataTextField = "Description"
                ddOraganisation.DataBind()


                Me.ddlVan.Items.Clear()
                Me.ddlVan.AppendDataBoundItems = True
                Me.ddlVan.Items.Insert(0, "--Select a van--")
                Me.ddlVan.Items(0).Value = ""

                chkVisit.Checked = True
                chkCollection.Checked = False
                chkDC.Checked = False
                chkInvoice.Checked = False
                chkOrder.Checked = False
                chkRMA.Checked = False

                Session.Remove("ListDates")
                Session.Remove("SelDate")
                BindEmptymap()





            End If
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_MSL_005") & "&next=Welcome.aspx&Title=Product List Setup", False)
        End Try

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
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        resetfields()
        chkVisit.Checked = True
        chkCollection.Checked = False
        chkDC.Checked = False
        chkInvoice.Checked = False
        chkOrder.Checked = False
        chkRMA.Checked = False
        BindEmptymap()

    End Sub

    Private Sub resetfields()
      
        Me.ddlVan.SelectedIndex = 0

        Session.Remove("ListDates")
        Session("ListDates") = Nothing
        Session.Remove("SelDate")
        Session("SelDate") = Nothing

        Me.RadCalendar1.SelectedDate = Nothing

    End Sub
 

    'Protected Sub RadCalendar1_DefaultViewChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.Calendar.DefaultViewChangedEventArgs) Handles RadCalendar1.DefaultViewChanged
    '    If Me.RadCalendar1.SelectedDates.Count > 0 And Me.ddlVan.SelectedIndex > 0 And Me.ddOraganisation.SelectedIndex > 0 Then

    '        BindMapData("Filter")
    '    Else
    '        BindEmptymap()
    '        Exit Sub
    '    End If
    'End Sub

    Protected Sub RadCalendar1_SelectionChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.Calendar.SelectedDatesEventArgs) Handles RadCalendar1.SelectionChanged

        'If Me.ddOraganisation.SelectedIndex <= 0 Or Me.ddlVan.SelectedIndex <= 0 Then

        '    Me.lblinfo.Text = "Validation"
        '    Me.lblMessage.Text = "Please select a organization/van"
        '    Me.lblMessage.ForeColor = Drawing.Color.Red
        '    Me.MpInfoError.Show()

        '    BindEmptymap()

        '    Exit Sub
        'End If

        If Me.RadCalendar1.SelectedDates.Count > 0 And Me.ddOraganisation.SelectedIndex > 0 And Me.ddlVan.SelectedIndex > 0 Or Not Session("SelDate") Is Nothing Then
            If Not Session("SelDate") Is Nothing And Me.RadCalendar1.SelectedDates.Count = 0 Then
                Me.RadCalendar1.SelectedDate = Session("SelDate")
            End If
            'BindMapData("Filter")
        Else
            Exit Sub
        End If

    End Sub
    Private Sub BindMapData(ByVal callfrom As String)
      
        'Dim SearchQuery As String = ""
        'Dim CustID As String = "0"
        'Dim SearchQuery1 As String = ""


        Session("SelDate") = Me.RadCalendar1.SelectedDate.Date

        'SearchQuery = SearchQuery + " And COnvert(Varchar(10),A.Start_Time,121) = Convert(Varchar(10),CAST('" + Me.RadCalendar1.SelectedDate.Date + "' AS DATETIME),121)"
        'SearchQuery1 = SearchQuery1 + " And COnvert(Varchar(10),B.Created_At,121) = Convert(Varchar(10),CAST('" + Me.RadCalendar1.SelectedDate.Date + "' AS DATETIME),121)"





        'If Me.ddlVan.SelectedIndex > 0 Then
        '    SearchQuery = SearchQuery & " And A.Personnel_Code = '" & ddlVan.SelectedValue.ToString() & "'"

        'End If



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
            cmd = New SqlCommand("Rep_FSRVisitTracking", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Connection = conn

            cmd.Parameters.AddWithValue("@OrgID", IIf(Me.ddOraganisation.SelectedIndex <= 0, "0", Me.ddOraganisation.SelectedValue))
            cmd.Parameters.AddWithValue("@SID", IIf(Me.ddlVan.SelectedIndex <= 0, "0", Me.ddlVan.SelectedValue))
            cmd.Parameters.AddWithValue("@VisitDate", Me.RadCalendar1.SelectedDate.Date)
            
          
            objDA.SelectCommand = cmd
            conn.Open()
            objDA.Fill(dt)





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
            Dim st As Boolean = False
            Dim i As Integer = 0
            Dim pin As String = ""
            Dim totcount As Integer = 0
            Dim line As Integer = 1
            For Each dr As DataRow In dt.Rows
                If CDec(dr("Lat").ToString()) > 0 And (dr("Mode").ToString() = "Visits" And chkVisit.Checked = True Or dr("Mode").ToString() = "DC" And chkDC.Checked = True Or dr("Mode").ToString() = "Invoice" And chkInvoice.Checked = True Or dr("Mode").ToString() = "BulkOrder" And chkOrder.Checked = True Or dr("Mode").ToString() = "Return" And chkRMA.Checked = True Or dr("Mode").ToString() = "Collection" And chkCollection.Checked = True) Then
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
                                    r("Customer") = "<b>Customer : " & dr("Customer").ToString().Replace("'", "") & "<b><br><table width=""100%"" height=""100%"" border=""1"" cellspacing=""2"" cellpadding=""2"" bgcolor=""Lavender""><tr><th align=""Left"">Purpose</th><th align=""Left"">Start Time</th><th align=""Left"">End Time</th></tr><tr><td>" & dr("Description").ToString() & "</td><td>" & DateTime.Parse(dr("VisitStartDate").ToString()).ToString("HH:mm") & "</td><td>" & DateTime.Parse(dr("VisitEndDate").ToString()).ToString("HH:mm") & "</td></tr>"
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
                                    r("Customer") = "<b>Customer : " & dr("Customer").ToString().Replace("'", "") & "<b><br><table width=""100%"" height=""100%"" border=""1"" cellspacing=""2"" cellpadding=""2"" bgcolor=""Lavender""><tr><th align=""Left"">Purpose</th><th align=""Left"">Description</th><th align=""Left"">Date</th></tr><tr><td>" & dr("Mode").ToString() & "</td><td>" & IIf(dr("Description").ToString() = "", "N/A", dr("Description").ToString()) & "</td><td>" & DateTime.Parse(dr("VisitStartDate").ToString()).ToString("HH:mm") & "</td></tr>"
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
                    temp_geocode = " '" & t("Lat").ToString() & "," & t("Lng").ToString() & ", " & t("Image").ToString() & "," & t("Line").ToString() & "," & t("Status").ToString() & "," & t("SupID").ToString() & "," & t("RowNo").ToString() & "'"

                    oGeocodeList.Add(temp_geocode)

                    temp_mapinfo = " '<span class=formatText>" & t("Customer").ToString().Replace("'", "") & "</span>' "
                    oMessageList.Add(temp_mapinfo)
                End If
            Next






            Dim message As [String] = String.Join(",", oMessageList.ToArray())
            Dim geocodevalues = String.Join(",", oGeocodeList.ToArray())

            ClientScript.RegisterArrayDeclaration("locationList", geocodevalues)
            ClientScript.RegisterArrayDeclaration("message", message)



            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Intialization", "initialize();", True)



        Catch ex As Exception
            ' lblError.Text = ex.Message
        Finally
            conn.Close()
            cmd.Dispose()
        End Try

        If dt.Rows.Count = 0 And callfrom = "Filter" Then
            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "There is no data for the searching criteria"
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Exit Sub

        End If
    End Sub

    Protected Sub ddOraganisation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddOraganisation.SelectedIndexChanged
        'Try
        If Me.ddOraganisation.SelectedIndex <= 0 Then

            BindEmptymap()

            'Me.lblinfo.Text = "Validation"
            'Me.lblMessage.Text = "Please select a organization/van"
            'Me.lblMessage.ForeColor = Drawing.Color.Red
            'Me.MpInfoError.Show()
            'Exit Sub

        End If
        If ddOraganisation.SelectedItem.Text <> "-- Select a Organization --" Then
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

            ddlVan.DataSource = objCommon.GetVanByOrg(Err_No, Err_Desc, ddOraganisation.SelectedValue, objUserAccess.UserID.ToString())
            ddlVan.Items.Clear()
            ddlVan.Items.Add("-- Select a van --")
            ddlVan.AppendDataBoundItems = True
            ddlVan.DataValueField = "SalesRep_Id"
            ddlVan.DataTextField = "SalesRep_Name"
            ddlVan.DataBind()
        Else
            ddlVan.Items.Clear()
            ddlVan.Items.Add("-- Select a van --")
            ddlVan.AppendDataBoundItems = True
            ddlVan.DataValueField = "SalesRep_Id"
            ddlVan.DataTextField = "SalesRep_Name"
            ddlVan.DataBind()

        End If
        BindEmptymap()
        Session("ListDates") = Nothing
    End Sub

    Private Sub BindEmptymap()
        oGeocodeList = New List(Of [String])()
        Dim temp_mapinfo
        Dim temp_geocode As String = ""
        oGeocodeList = New List(Of [String])()
        temp_geocode = "'0.000000, 0.000000,http://maps.google.com/mapfiles/ms/icons/green.png'"
        oGeocodeList.Add(temp_geocode)
        Dim oMessageList As New List(Of String)()
        temp_mapinfo = " '<span class=formatText>Test</span>' "
        oMessageList.Add(temp_mapinfo)
        Dim geocodevalues = String.Join(",", oGeocodeList.ToArray())
        Dim message As [String] = String.Join(",", oMessageList.ToArray())
        'Dim geocodevalues = String.Join(",", oGeocodeList.ToArray())
        ClientScript.RegisterArrayDeclaration("locationList", geocodevalues)
        ClientScript.RegisterArrayDeclaration("message", message)
        Page.ClientScript.RegisterStartupScript(GetType(String), "Intialization", "initialize();", True)

        oGeocodeList = Nothing
    End Sub
    Protected Sub ddlVan_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlVan.SelectedIndexChanged
        If Me.ddlVan.SelectedIndex <= 0 Then

           BindEmptymap()

            'Me.lblinfo.Text = "Validation"
            'Me.lblMessage.Text = "Please select a organization/van"
            'Me.lblMessage.ForeColor = Drawing.Color.Red
            'Me.MpInfoError.Show()
            'Exit Sub

        End If
        If Me.ddlVan.SelectedIndex > 0 Then
            Dim vd As New DataTable
            vd = objCommon.GetFSRVisitedDates(Err_No, Err_Desc, Me.ddlVan.SelectedValue)

            Session("ListDates") = vd
        Else
            Session("ListDates") = Nothing
        End If

        BindEmptymap()
    End Sub

    Protected Sub chkVisit_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkVisit.CheckedChanged

        If Me.ddlVan.SelectedIndex <= 0 Or Me.ddOraganisation.SelectedIndex <= 0 Then

            BindEmptymap()

            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Please select a organization/van"
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Exit Sub

        End If

        BindMapData("Filter")
    End Sub
    Protected Sub chkDC_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDC.CheckedChanged
        If Me.ddlVan.SelectedIndex <= 0 Or Me.ddOraganisation.SelectedIndex <= 0 Then

            BindEmptymap()

            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Please select a organization/van"
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Exit Sub

        End If
        BindMapData("Filter")
    End Sub

    Protected Sub chkOrder_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkOrder.CheckedChanged
        If Me.ddlVan.SelectedIndex <= 0 Or Me.ddOraganisation.SelectedIndex <= 0 Then

            BindEmptymap()

            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Please select a organization/van"
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Exit Sub

        End If
        BindMapData("Filter")
    End Sub

    Protected Sub chkCollection_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCollection.CheckedChanged
        If Me.ddlVan.SelectedIndex <= 0 Or Me.ddOraganisation.SelectedIndex <= 0 Then

            BindEmptymap()

            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Please select a organization/van"
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Exit Sub

        End If
        BindMapData("Filter")
    End Sub

    Protected Sub chkInvoice_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkInvoice.CheckedChanged
        If Me.ddlVan.SelectedIndex <= 0 Or Me.ddOraganisation.SelectedIndex <= 0 Then

            BindEmptymap()

            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Please select a organization/van"
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Exit Sub

        End If
        BindMapData("Filter")

    End Sub
    Protected Sub chkRMA_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkRMA.CheckedChanged
        If Me.ddlVan.SelectedIndex <= 0 Or Me.ddOraganisation.SelectedIndex <= 0 Then

            BindEmptymap()

            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Please select a organization/van"
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Exit Sub

        End If
        BindMapData("Filter")

    End Sub

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click

        If Me.ddlVan.SelectedIndex <= 0 Or Me.ddOraganisation.SelectedIndex <= 0 Or Me.RadCalendar1.SelectedDates.Count <= 0 Then

            BindEmptymap()

            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Please select a organization,van and visit date"
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Exit Sub

        End If

        BindMapData("Filter")


    End Sub
End Class