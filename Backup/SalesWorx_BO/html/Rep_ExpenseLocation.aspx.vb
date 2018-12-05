Imports SalesWorx.BO.Common
Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Partial Public Class Rep_ExpenseLocation
    Inherits System.Web.UI.Page
Dim objProduct As New Product
    Dim objCommon As New Common
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const PageID As String = "P283"
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
        Me.Title = "Fuel Expense Location Report"
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
                Me.ddlVan.Items.Insert(0, "--All--")
                Me.ddlVan.Items(0).Value = ""


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
        txtToDate.Text = ""
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
            cmd = New SqlCommand("rep_FuelExpense", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Connection = conn

            cmd.Parameters.AddWithValue("@OrgID", IIf(Me.ddOraganisation.SelectedIndex <= 0, "0", Me.ddOraganisation.SelectedValue))
            cmd.Parameters.AddWithValue("@FsrID", IIf(Me.ddlVan.SelectedIndex <= 0, "-1", Me.ddlVan.SelectedValue))
            cmd.Parameters.AddWithValue("@Fromdate", IIf(Me.txtFromDate.Text.Trim() <> "", Me.txtFromDate.Text.Trim(), DBNull.Value))
            cmd.Parameters.AddWithValue("@ToDate", IIf(Me.txtToDate.Text.Trim() <> "", Me.txtToDate.Text.Trim(), DBNull.Value))


            objDA.SelectCommand = cmd
            conn.Open()
            objDA.Fill(dt)

            Dim TobeDistinct As String() = {"Latitude", "Longitude"}
            Dim dtDistinct As DataTable = GetDistinctRecords(dt, TobeDistinct)

            Dim dtlist As New DataTable()
            dtlist.Columns.Add("RowID")
            dtlist.Columns.Add("Details")
            dtlist.Columns.Add("Latitude")
            dtlist.Columns.Add("Longitude")
            dtlist.Columns.Add("Odo_Reading")
            dtlist.Columns.Add("Fuel_Qty")
            dtlist.Columns.Add("Fuel_Amount")
            dtlist.Columns.Add("Logged_At")
            dtlist.Columns.Add("Image")
            dtlist.Columns.Add("Line")
            Dim st As Boolean = False
            Dim i As Integer = 0
            Dim pin As String = ""
            Dim totcount As Integer = 0
            Dim line As Integer = 1

            Dim Error_des As String = ""
            Dim Error_No As Long
            Dim CurrenctCode As String = ""
            Dim CurrencyDT = (New SalesWorx.BO.Common.OrgConfig).GetCurrency(Error_No, Error_des, Me.ddOraganisation.SelectedValue)
            If (CurrencyDT.Rows.Count > 0) Then
                CurrenctCode = CurrencyDT.Rows(0)("Currency_Code").ToString()
            End If

            For Each prows As DataRow In dtDistinct.Rows
                    Dim r As DataRow = dtlist.NewRow()
                    r("Latitude") = prows("Latitude").ToString()
                    r("Longitude") = prows("Longitude").ToString()
                    Dim SelRows() As DataRow
                    SelRows = dt.Select(" Latitude='" & prows("Latitude").ToString() & "' and Longitude='" & prows("Longitude").ToString() & "'")

                    Dim str As String = "<table border='1' style='border-style:solid;border-width:1px;border-color:grey'  bgcolor='#f6f6f6'  cellspacing='0' cellpadding='6'><tr><td style='text-align:center'><b>Van</b></td><td style='text-align:center'><b>Date</b></td><td style='text-align:center'><b>Odometer</b></td><td style='text-align:center'><b>Fuel Qty<br/>(Liters)</b></td><td style='text-align:center'><b>Fuel Amount<BR/>(" & CurrenctCode & ")</b></td></tr>"
                    If SelRows.Length > 0 Then
                        For Each dr As DataRow In SelRows
                                str = str & "<tr><td style='padding: 2px;'>" & dr("SalesRep_name").ToString() & "</td><td style='padding: 2px;'>" & CDate(dr("Logged_At").ToString()).ToString("dd-MMM-yyyy hh:mm tt") & "</td><td style='padding: 2px;'>" & dr("Odo_Reading").ToString() & "</td><td style='text-align:right'>" & dr("Fuel_Qty").ToString() & "</td><td style='text-align:right'>" & Format(Val(dr("Fuel_Amount").ToString()), "#,##0.00") & "</td></tr>"
                        Next
                    End If
                    str = str & "</table>"
                    r("Details") = str
                    dtlist.Rows.Add(r)
            Next

            For Each t As DataRow In dtlist.Rows

                If CDec(t("Latitude").ToString()) > 0 And CDec(t("Longitude").ToString()) > 0 Then
                    temp_geocode = " '" & t("Latitude").ToString() & "," & t("Longitude").ToString() & "'"

                    oGeocodeList.Add(temp_geocode)

                    temp_mapinfo = " '<span class=formatText>" & t("Details").ToString().Replace("'", "") & "</span>' "
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

'Following function will return Distinct records for Name, City and State column.
Public Shared Function GetDistinctRecords(ByVal dt As DataTable, ByVal Columns As String()) As DataTable
    Dim dtUniqRecords As New DataTable()
    dtUniqRecords = dt.DefaultView.ToTable(True, Columns)
    Return dtUniqRecords
End Function

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

            Me.ddlVan.Items.Clear()
            ddlVan.DataValueField = "SalesRep_Id"
            ddlVan.DataTextField = "SalesRep_Name"
            ddlVan.DataBind()
            ddlVan.Items.Insert(0, New ListItem("-- All --", "-1"))
        End If
        BindEmptymap()

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
        If txtFromDate.Text.Trim() = "" Then
                BindEmptymap()
                MessageBoxValidation("Enter a valid from date.")
                Exit Sub
        Else
        If Not IsDate(txtFromDate.Text) Then
                BindEmptymap()
                MessageBoxValidation("Enter a valid from date.")
                Exit Sub
            End If
        End If

        If txtToDate.Text.Trim() = "" Then
            BindEmptymap()
            MessageBoxValidation("Enter a valid to date.")
            Exit Sub
        Else
                If Not IsDate(txtToDate.Text) Then
                    BindEmptymap()
                    MessageBoxValidation("Enter a valid to date.")
                    Exit Sub
                End If
        End If
        If (txtFromDate.Text.Trim() <> "" And txtFromDate.Text.Trim() <> "") Then
                If CDate(txtFromDate.Text) > CDate(txtToDate.Text) Then
                    BindEmptymap()
                    MessageBoxValidation("Start Date should not be greater than End Date.")
                    Exit Sub
                End If
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