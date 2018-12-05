Imports SalesWorx.BO.Common
Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
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
                ddOraganisation.Items.Add("-- Select a Organization --")
                ddOraganisation.AppendDataBoundItems = True
                ddOraganisation.DataValueField = "MAS_Org_ID"
                ddOraganisation.DataTextField = "Description"
                ddOraganisation.DataBind()


                Me.ddlVan.Items.Clear()
                Me.ddlVan.Items.Insert(0, "--Select Van--")
                Me.ddlVan.Items(0).Value = "-1"


                BindEmptymap()

            End If
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_MSL_005") & "&next=Welcome.aspx&Title=Product List Setup", False)
        End Try

    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        resetfields()

        BindEmptymap()

    End Sub

    Private Sub resetfields()
        Me.ddlVan.SelectedIndex = 0
        txtFromDate.Text = ""
    End Sub
    Private Sub BindMapData(ByVal callfrom As String)

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
            cmd.Parameters.AddWithValue("@VisitDate", IIf(Me.txtFromDate.Text.Trim() <> "", Me.txtFromDate.Text.Trim(), DBNull.Value))


            objDA.SelectCommand = cmd
            conn.Open()
            objDA.Fill(dt)
            If dt.Rows.Count <= 0 Then
                BindEmptymap()
                MessageBoxValidation("No Visits done on this date .")
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

                    Dim str As String = "<table border='1' style='border-style:solid;border-width:1px;border-color:grey'  bgcolor='#f6f6f6'  cellspacing='0' cellpadding='6'><tr><td style='text-align:center'><b>Customer</b></td><td style='text-align:center'><b>Visit Start Date</b></td><td style='text-align:center'><b>Visit End Date</b></td></tr>"
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

            ClientScript.RegisterArrayDeclaration("locationList", geocodevalues)
            ClientScript.RegisterArrayDeclaration("message", message)



            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Intialization", "initialize();", True)



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

        If Me.ddOraganisation.SelectedIndex <= 0 Then

            BindEmptymap()

            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Please select a organization/van"
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Exit Sub

        End If
        If ddOraganisation.SelectedItem.Text <> "-- Select a Organization --" Then
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            ddlVan.DataSource = objCommon.GetVanByOrg(Err_No, Err_Desc, ddOraganisation.SelectedValue, objUserAccess.UserID.ToString())

            Me.ddlVan.Items.Clear()
            ddlVan.DataValueField = "SalesRep_Id"
            ddlVan.DataTextField = "SalesRep_Name"
            ddlVan.DataBind()
            ddlVan.Items.Insert(0, New ListItem("-- Select Van --", "-1"))
        End If
        BindEmptymap()

    End Sub

    Private Sub BindEmptymap()
        oGeocodeList = New List(Of [String])()
        Dim temp_mapinfo
        Dim temp_geocode As String = ""
        oGeocodeList = New List(Of [String])()
        temp_geocode = "'25.000000, 55.000000,0'"
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
        BindEmptymap()
    End Sub

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click

        If Me.ddOraganisation.SelectedIndex <= 0 Then

            BindEmptymap()

            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Please select a organization"
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Exit Sub

        End If
         If Me.ddlVan.SelectedIndex <= 0 Then

            BindEmptymap()

            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Please select a Van"
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Exit Sub

        End If
        If txtFromDate.Text.Trim() = "" Then
                BindEmptymap()
                MessageBoxValidation("Enter a valid from date.")
                Exit Sub
        End If


        BindMapData("Filter")

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