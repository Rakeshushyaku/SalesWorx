Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI

Partial Public Class Rep_ExpenseLocation
    Inherits System.Web.UI.Page
    Dim objProduct As New Product
    Dim objCommon As New SalesWorx.BO.Common.Common
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const PageID As String = "P283"
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim oGeocodeList As List(Of String)
    Private Shared _strDefLat As String = ConfigurationSettings.AppSettings("DefaultLat")
    Private Shared _strDefLong As String = ConfigurationSettings.AppSettings("DefaultLong")

    Private ReportPath As String = "FuelExpense"

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
                ddOraganisation.Items.Add("Select Organization")
                ddOraganisation.AppendDataBoundItems = True
                ddOraganisation.DataValueField = "MAS_Org_ID"
                ddOraganisation.DataTextField = "Description"
                ddOraganisation.DataBind()

                If ddOraganisation.Items.Count = 2 Then
                    ddOraganisation.SelectedIndex = 1

                    ''Filling Currency and decimal
                    Dim dtCur As New DataTable
                    dtCur = objCommon.GetCurrencyByOrg(Err_No, Err_Desc, ddOraganisation.SelectedItem.Value)
                    If dtCur.Rows.Count > 0 Then
                        hfCurrency.Value = dtCur.Rows(0)(0).ToString()
                        hfDecimal.Value = dtCur.Rows(0)(1).ToString()
                        lbl_Currency.Text = dtCur.Rows(0)(0).ToString()
                    End If

                    LoadVan()

                End If


                txtFromDate.SelectedDate = FirstDayOfMonth(Now().Date)
                txtToDate.SelectedDate = Now().Date

                ' ''Me.ddlVan.Items.Clear()
                ' ''Me.ddlVan.Items.Insert(0, "--All--")
                ' ''Me.ddlVan.Items(0).Value = ""

                Me.hfDefLat.Value = _strDefLat
                Me.hfDefLng.Value = _strDefLong
                'BindEmptymap()

            End If
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_MSL_005") & "&next=Welcome.aspx&Title=Product List Setup", False)
        End Try

    End Sub
    Public Function FirstDayOfMonth(ByVal sourceDate As DateTime) As DateTime
        Return New DateTime(sourceDate.Year, sourceDate.Month, 1)
    End Function
    Sub LoadVan()
        Dim objUserAccess As UserAccess
        Try
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            objCommon = New SalesWorx.BO.Common.Common()
            ddlVan.DataSource = objCommon.GetVanByOrg(Err_No, Err_Desc, ddOraganisation.SelectedValue, objUserAccess.UserID)
            ddlVan.DataValueField = "SalesRep_ID"
            ddlVan.DataTextField = "SalesRep_Name"
            ddlVan.DataBind()

            ddlVan.Items.Insert(0, New RadComboBoxItem("Select van/FSR"))

        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            objUserAccess = Nothing
            objCommon = Nothing
        End Try
    End Sub

    ' ''Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
    ' ''    resetfields()

    ' ''    BindEmptymap()

    ' ''End Sub

    Private Sub resetfields()

        Me.ddlVan.SelectedIndex = 0
        ' ''txtFromDate.Text = ""
        ' ''txtToDate.Text = ""
    End Sub
    Private Sub BindMapData(ByVal callfrom As String)

        ' ''Dim cmd As SqlCommand = Nothing
        ' ''Dim conn As New SqlConnection()
        ' ''Dim objDA As New SqlDataAdapter()
        Dim dt As New DataTable()
        Dim oMessageList As New List(Of String)()
        Dim temp_geocode As String = ""
        Dim temp_mapinfo As String
        Dim ObjReport As New SalesWorx.BO.Common.Reports
        Try
            oGeocodeList = New List(Of String)()

            '' ''conn.ConnectionString = ConfigurationSettings.AppSettings("SQLConnString")
            '' ''cmd = New SqlCommand("rep_FuelExpense", conn)
            '' ''cmd.CommandType = CommandType.StoredProcedure
            '' ''cmd.Connection = conn

            '' ''cmd.Parameters.AddWithValue("@OrgID", IIf(Me.ddOraganisation.SelectedIndex <= 0, "0", Me.ddOraganisation.SelectedValue))
            '' ''cmd.Parameters.AddWithValue("@FsrID", IIf(Me.ddlVan.SelectedIndex <= 0, "-1", Me.ddlVan.SelectedValue))
            '' ''cmd.Parameters.AddWithValue("@Fromdate", IIf(Me.txtFromDate.Text.Trim() <> "", Me.txtFromDate.Text.Trim(), DBNull.Value))
            '' ''cmd.Parameters.AddWithValue("@ToDate", IIf(Me.txtToDate.Text.Trim() <> "", Me.txtToDate.Text.Trim(), DBNull.Value))


            '' ''objDA.SelectCommand = cmd
            '' ''conn.Open()
            '' ''objDA.Fill(dt)





            lbl_org.Text = ddOraganisation.SelectedItem.Text
            lbl_van.Text = IIf(ddlVan.SelectedValue = "", "All", ddlVan.SelectedItem.Text)
            lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_To.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")

            dt = ObjReport.GetFuelExpenses(Err_No, Err_Desc, IIf(Me.ddOraganisation.SelectedIndex <= 0, "0", Me.ddOraganisation.SelectedValue), IIf(Me.txtFromDate.SelectedDate IsNot Nothing, Me.txtFromDate.SelectedDate, DBNull.Value), IIf(Me.txtToDate.SelectedDate IsNot Nothing, Me.txtToDate.SelectedDate, DBNull.Value),
                                  IIf(Me.ddlVan.SelectedIndex <= 0, "-1", Me.ddlVan.SelectedValue))


            If dt.Rows.Count = 0 And callfrom = "Filter" Then
                raf.ResponseScripts.Add("locationList = undefined; message = undefined;")
                MessageBoxValidation("There is no data for the searching criteria", "Validation")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Intialization", "HideMap();", True)
                Exit Sub
            End If

            Args.Visible = True
            rpbFilter.Items(0).Expanded = False
            LocationTab.Visible = True
            gvRep.Visible = True
            divCurrency.Visible = True

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

                Dim str As String = "<table width=""100%"" height=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" class=""table""><thead><tr><th style='text-align:center'><b>Van</b></th><th style='text-align:center'><b>Date</b></th><th style='text-align:center'><b>Odometer</b></th><th style='text-align:center'><b>Fuel Qty<br/>(Liters)</b></th><th style='text-align:center'><b>Fuel Amount<BR/>(" & CurrenctCode & ")</b></th></tr></thead>"
                If SelRows.Length > 0 Then
                    For Each dr As DataRow In SelRows
                        str = str & "<tbody><tr><td style='padding: 2px;'>" & dr("SalesRep_name").ToString() & "</td><td style='padding: 2px;'>" & CDate(dr("Logged_At").ToString()).ToString("dd-MMM-yyyy hh:mm tt") & "</td><td style='padding: 2px;'>" & dr("Odo_Reading").ToString() & "</td><td style='text-align:right'>" & dr("Fuel_Qty").ToString() & "</td><td style='text-align:right'>" & Format(Val(dr("Fuel_Amount").ToString()), "#,##0.00") & "</td></tr></tbody>"
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
            Dim geocodevalues = Nothing
            geocodevalues = String.Join(",", oGeocodeList.ToArray())

            'ClientScript.RegisterArrayDeclaration("locationList", geocodevalues)
            'ClientScript.RegisterArrayDeclaration("message", message)

            'ScriptManager.RegisterArrayDeclaration(UpdatePanel1, "locationList", geocodevalues)
            'ScriptManager.RegisterArrayDeclaration(UpdatePanel1, "message", message)

            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Intialization", "initialize();", True)

            raf.ResponseScripts.Add("locationList = undefined; message = undefined;")

            ScriptManager.RegisterArrayDeclaration(Me, "locationList", geocodevalues)
            ScriptManager.RegisterArrayDeclaration(Me, "message", message)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Intialization1", "clearOverlays();", True)
        Catch ex As Exception
            ' lblError.Text = ex.Message
        Finally
            ' ''conn.Close()
            ' ''cmd.Dispose()
            ObjReport = Nothing
        End Try

       
    End Sub

    'Following function will return Distinct records for Name, City and State column.
    Public Shared Function GetDistinctRecords(ByVal dt As DataTable, ByVal Columns As String()) As DataTable
        Dim dtUniqRecords As New DataTable()
        dtUniqRecords = dt.DefaultView.ToTable(True, Columns)
        Return dtUniqRecords
    End Function

    Protected Sub ddOraganisation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddOraganisation.SelectedIndexChanged
        Try
            ' ''If Me.ddOraganisation.SelectedIndex <= 0 Then

            ' ''    BindEmptymap()

            ' ''End If
            If ddOraganisation.SelectedItem.Text <> "-- Select a Organization --" Then
                '' ''Dim objUserAccess As UserAccess
                '' ''objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
                '' ''ddlVan.DataSource = objCommon.GetVanByOrg(Err_No, Err_Desc, ddOraganisation.SelectedValue, objUserAccess.UserID.ToString())

                '' ''Me.ddlVan.Items.Clear()
                '' ''ddlVan.DataValueField = "SalesRep_Id"
                '' ''ddlVan.DataTextField = "SalesRep_Name"
                '' ''ddlVan.DataBind()
                '' ''ddlVan.Items.Insert(0, New RadComboBoxItem("-- All --", "-1"))

                Dim dtCur As New DataTable
                dtCur = objCommon.GetCurrencyByOrg(Err_No, Err_Desc, ddOraganisation.SelectedItem.Value)
                If dtCur.Rows.Count > 0 Then
                    hfCurrency.Value = dtCur.Rows(0)(0).ToString()
                    hfDecimal.Value = dtCur.Rows(0)(1).ToString()
                    lbl_Currency.Text = dtCur.Rows(0)(0).ToString()
                End If

                LoadVan()
            Else
                ddlVan.ClearSelection()
                ddlVan.Items.Clear()
                ddlVan.Items.Insert(0, New RadComboBoxItem("Select Van", "0"))

                ''ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Intialization", "HideMap();", True)
            End If
            '' BindEmptymap()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Intialization", "initialize();", True)
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub BindEmptymap()
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
        raf.ResponseScripts.Add("locationList = undefined; message = undefined;")
        ScriptManager.RegisterArrayDeclaration(Me, "locationList", geocodevalues)
        ScriptManager.RegisterArrayDeclaration(Me, "message", message)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Intialization1", "clearOverlays();", True)

        oGeocodeList = Nothing
    End Sub
    Protected Sub ddlVan_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlVan.SelectedIndexChanged
        BindEmptymap()
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        Try
            If ddOraganisation.SelectedItem.Value = "" Then
                '' BindEmptymap()
                MessageBoxValidation("Please select an organization", "Validation")
                Return bretval
            End If
            If txtFromDate.SelectedDate Is Nothing Then
                ' BindEmptymap()
                MessageBoxValidation("Enter a valid from date.", "Validation")
                Return bretval
            Else
                If Not IsDate(txtFromDate.SelectedDate) Then
                    ''   BindEmptymap()
                    MessageBoxValidation("Enter a valid from date.", "Validation")
                    Return bretval
                End If
            End If

            If txtToDate.SelectedDate Is Nothing Then
                ''  BindEmptymap()
                MessageBoxValidation("Enter a valid to date.", "Validation")
                Return bretval
            Else
                If Not IsDate(txtToDate.SelectedDate) Then
                    '' BindEmptymap()
                    MessageBoxValidation("Enter a valid to date.", "Validation")
                    ''  Exit Function
                    Return bretval
                End If
            End If
            If (txtFromDate.SelectedDate IsNot Nothing AndAlso txtFromDate.SelectedDate IsNot Nothing) Then
                If CDate(txtFromDate.SelectedDate) > CDate(txtToDate.SelectedDate) Then
                    '' BindEmptymap()
                    MessageBoxValidation("Start Date should not be greater than End Date.", "Validation")
                    '' Exit Function
                    Return bretval
                End If
            End If

            Return True
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Function
    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Try
            Args.Visible = False
            LocationTab.Visible = False
            LocationTab.Tabs(0).Selected = True
            LocationTab.SelectedTab.Selected = True
            RadMultiPage21.SelectedIndex = 0
            gvRep.Visible = False
            divCurrency.Visible = False
            If ValidateInputs() Then
                BindMapData("Filter")

                BindGrid()
            Else
                raf.ResponseScripts.Add("locationList = undefined; message = undefined;")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Intialization", "HideMap();", True)
            End If
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub BindGrid()
        Dim objReport As Reports
        Try
            objReport = New Reports()
            Dim van As String = "-1"

            If ddlVan.SelectedValue <> "" Then
                van = ddlVan.SelectedValue
            End If

            Dim dt As DataTable = objReport.GetFuelExpenses(Err_No, Err_Desc, ddOraganisation.SelectedValue, txtFromDate.SelectedDate, txtToDate.SelectedDate, van)

            gvRep.DataSource = dt
            gvRep.DataBind()


        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            objReport = Nothing
        End Try
    End Sub

    Private Function lblinfo() As Object
        Throw New NotImplementedException
    End Function

    Private Sub BtnExportExcel_Click(sender As Object, e As EventArgs) Handles BtnExportExcel.Click
        Try
            If ValidateInputs() Then
                Export("Excel")
            End If
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
    Sub Export(format As String)
        Try
            Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
            rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)


            Dim Van As String

            If ddlVan.SelectedValue = "" Then
                Van = "-1"
            Else
                Van = ddlVan.SelectedValue
            End If

            Dim SalesRepID As New ReportParameter
            SalesRepID = New ReportParameter("FsrID", Van)

            Dim FDate As New ReportParameter
            If txtFromDate.SelectedDate IsNot Nothing Then
                FDate = New ReportParameter("Fromdate", CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"))
            Else
                FDate = New ReportParameter("Fromdate")
            End If


            Dim TDate As New ReportParameter
            If txtToDate.SelectedDate IsNot Nothing Then
                TDate = New ReportParameter("ToDate", CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"))
            Else
                TDate = New ReportParameter("ToDate")
            End If

            Dim OID As New ReportParameter
            OID = New ReportParameter("OrgID", CStr(ddOraganisation.SelectedValue.ToString()))



            rview.ServerReport.SetParameters(New ReportParameter() {OID, SalesRepID, FDate, TDate})

            Dim mimeType As String = Nothing
            Dim encoding As String = Nothing
            Dim extension As String = Nothing
            Dim deviceInfo As String = "<DeviceInfo></DeviceInfo>"
            Dim streamids As String() = Nothing
            Dim warnings As Microsoft.Reporting.WebForms.Warning() = Nothing

            Dim bytes As Byte() = rview.ServerReport.Render(format, deviceInfo, mimeType, encoding, extension, streamids, warnings)


            Response.Clear()
            If format = "PDF" Then
                Response.ContentType = "application/pdf"
                Response.AddHeader("Content-disposition", "attachment;filename=FuelExpense.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=FuelExpense.xls")
                Response.AddHeader("Content-Length", bytes.Length)
            End If
            Response.OutputStream.Write(bytes, 0, bytes.Length)
            Response.OutputStream.Flush()
            Response.OutputStream.Close()
            Response.Flush()
            Response.Close()


        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub BtnExportPDF_Click(sender As Object, e As EventArgs) Handles BtnExportPDF.Click
        Try
            If ValidateInputs() Then
                Export("PDF")
            End If
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub gvRep_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles gvRep.ItemDataBound
        Try
            If TypeOf e.Item Is GridDataItem Then
                Dim item As GridDataItem = TryCast(e.Item, GridDataItem)
                If item IsNot Nothing Then
                    item.Cells(6).Text = FormatNumber(CDbl(item.Cells(6).Text), hfDecimal.Value)
                End If
            End If
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub gvRep_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        BindGrid()
    End Sub

    Private Sub gvRep_SortCommand(sender As Object, e As GridSortCommandEventArgs) Handles gvRep.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindGrid()
    End Sub
    Private Property SortDirection() As String
        Get
            If ViewState("SortDirection") Is Nothing Then
                ViewState("SortDirection") = "ASC"
            End If
            Return ViewState("SortDirection").ToString()
        End Get
        Set(ByVal value As String)
            Dim s As String = SortDirection

            If value = "flip" Then
                s = If(s = "ASC", "DESC", "ASC")
            Else
                s = value
            End If

            ViewState("SortDirection") = s
        End Set
    End Property

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ddOraganisation.ClearSelection()
        If ddOraganisation.Items.Count = 2 Then
            ddOraganisation.SelectedIndex = 1
        End If
        ddlVan.ClearSelection()
        ddlVan.Items.Clear()
        Try
            
            If ddOraganisation.SelectedItem.Text <> "-- Select a Organization --" Then
                Dim dtCur As New DataTable
                dtCur = objCommon.GetCurrencyByOrg(Err_No, Err_Desc, ddOraganisation.SelectedItem.Value)
                If dtCur.Rows.Count > 0 Then
                    hfCurrency.Value = dtCur.Rows(0)(0).ToString()
                    hfDecimal.Value = dtCur.Rows(0)(1).ToString()
                    lbl_Currency.Text = dtCur.Rows(0)(0).ToString()
                End If

                LoadVan()
            Else
                ddlVan.ClearSelection()
                ddlVan.Items.Clear()
                ddlVan.Items.Insert(0, New RadComboBoxItem("Select Van", "0"))
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Intialization", "initialize();", True)
        Catch ex As Exception
            log.Error(ex.Message)
        End Try

        Args.Visible = False
        LocationTab.Visible = False
        LocationTab.Tabs(0).Selected = True
        LocationTab.SelectedTab.Selected = True
        RadMultiPage21.SelectedIndex = 0
        gvRep.Visible = False
        divCurrency.Visible = False
    End Sub
End Class