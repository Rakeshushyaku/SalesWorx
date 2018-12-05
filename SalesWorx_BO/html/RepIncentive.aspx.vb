Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Imports OfficeOpenXml

Public Class RepIncentive
    Inherits System.Web.UI.Page


    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer

    Private ReportPath As String = "RepEmpIncentive"
    Dim objIncentive As New SalesWorx.BO.Common.Incentive
    Private Const PageID As String = "P355"
    Dim alVisitInfo As New ArrayList
    Dim objInfo As New Vist_Info
    Dim TotInvoice As Single
    Dim TotCreditNotes As Single
    Dim TotPayment As Single
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

       
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
                Dim orgTbl As DataTable = Nothing
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))
                'dtp_incentiveyear.SelectedDate = DateTime.Now.Date
                Loademp()
                LoadYear()
                orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                Dim OrgStr As String = Nothing
                For Each item As RadComboBoxItem In ddlOrganization.Items
                    item.Checked = True
                    If item.Checked Then

                        OrgStr = OrgStr & "," & item.Value

                    End If
                Next

               
                If ddlOrganization.Items.Count = 2 Then
                        ddlOrganization.SelectedIndex = 1
                        Loademp()
                End If

                    If Not String.IsNullOrEmpty(Request.QueryString("b")) Then
                        If Not Session("Org_ID") Is Nothing Then
                            ddlOrganization.SelectedValue = Session("Org_ID")

                            ddl_year.SelectedItem.Text = Session("Incentive_Year")
                            Loademp()
                            ddl_empcode.SelectedValue = Session("Emp_Code")
                            BindData()

                            'gvRep.Visible = True
                            'Chart.Visible = True
                            'repdiv.Visible = True
                            'BindData()
                            'Chartwrapper.Visible = True
                            BindChart()


                            Session.Remove("Org_ID")
                            Session.Remove("Emp_Code")
                            Session.Remove("Incentive_Year")
                            Session.Remove("Incentive_Month")
                            Session.Remove("Org_Name")
                            Session.Remove("EmpName")
                            'Session.Remove("BackFlg")
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
                    ObjCommon = Nothing
                    ErrorResource = Nothing
                End Try
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Sub LoadOrgs()
        Try
            Dim orgTbl As DataTable = Nothing
            ObjCommon = New SalesWorx.BO.Common.Common()
            Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
            orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
            Dim s() As String = Nothing
            Dim Currency As String = Nothing
            Dim DecimalDigits As String = "2"
            Dim OrgStr As String = Nothing
            For Each item As RadComboBoxItem In ddlOrganization.Items
                item.Checked = True
                If item.Checked Then

                    OrgStr = OrgStr & "," & item.Value

                End If
            Next
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try

    End Sub
    Sub Loademp()
        Try
            ddl_empcode.DataSource = objIncentive.GetEmp(Err_No, Err_Desc, ddlOrganization.SelectedValue)
            ddl_empcode.Items.Clear()
            ddl_empcode.Items.Add(New RadComboBoxItem("Select Employee"))
            ddl_empcode.AppendDataBoundItems = True
            ddl_empcode.DataTextField = "Emp_Name"
            ddl_empcode.DataValueField = "Emp_Code"
            ddl_empcode.DataBind()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Sub LoadYear()
        Try

            Dim tbl_year As Integer
            tbl_year = objIncentive.GetIncentiveMPYear(Err_No, Err_Desc)
            If tbl_year = 0 Then
                For i As Integer = -2 To 2
                    Dim _y As String = DateTime.Now.AddYears(i).Year.ToString()
                    ddl_year.Items.Add(_y)
                Next
            Else
                For i As Integer = tbl_year To DateTime.Now.AddYears(3).Year
                    ddl_year.Items.Add(i)
                Next
            End If

            ddl_year.SelectedIndex = 0

        Catch ex As Exception
            log.Error(ex.Message.ToString())
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub
    Private Sub SearchBtn_Click(sender As Object, e As EventArgs) Handles SearchBtn.Click
        Try
          
            Args.Visible = False
            gvRep.Visible = False
            Chart.Visible = False


            If Not (ddlOrganization.SelectedItem.Value = "0") Then

                If ddl_empcode.SelectedIndex <= 0 Then
                    MessageBoxValidation("Select an Employee.", "Validation")
                    Exit Sub
                End If
                'If Not IsDate(dtp_incentiveyear.SelectedDate) Then
                '    MessageBoxValidation("Enter valid year.", "Validation")
                '    Exit Sub
                'End If
                If ddl_year.SelectedItem.Text = "" Then
                    MessageBoxValidation("Enter valid year.", "Validation")
                    Exit Sub
                End If
                gvRep.Visible = True
                Chart.Visible = True
                repdiv.Visible = True
                BindData()
                Chartwrapper.Visible = True
                BindChart()
            Else
                MessageBoxValidation("Select an organization.", "Validation")
            End If

        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
    Private Function GetMonthName(ByVal monthNum As Integer) As String
        Try
            Dim strDate As New DateTime(1, monthNum, 1)
            Return strDate.ToString("MMM")
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Function
    Private Sub BindChart()
        Try

            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetEmpIncentive(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, ddl_empcode.SelectedValue, ddl_year.SelectedItem.Text)

            'If dt.Rows.Count > 0 Then


            '    For month As Integer = 1 To 12
            '        Dim result() As DataRow = dt.Select("Incentive_Month ='" & month & "'")

            '        ' Organization_ID()

            '        If result.Count = 0 Then
            '            Dim dr As DataRow = dt.NewRow()
            '            dr("Organization_ID") = dt.Rows(0)("Organization_ID")
            '            dr("Emp_Code") = dt.Rows(0)("Emp_Code")
            '            dr("Incentive_Year") = dt.Rows(0)("Incentive_Year")
            '            dr("Incentive_Month") = month.ToString()
            '            dr("NET_SALES_VOLUME") = "0"
            '            dr("SUCCESSFUL_VISITS") = "0"
            '            dr("Sales_Value_Acheived") = "0"
            '            dr("Sales_Volume_Acheived") = "0"
            '            dr("Success_Visits_Acheived") = "0"

            '            Dim xMonth As String = MonthName(month)


            '            dr("Tmonth") = xMonth
            '            dr("TotalCommission") = "0"
            '            dr("CommissionTarget") = "0"

            '            dt.Rows.Add(dr)



            '        End If

            '    Next


            dt.DefaultView.Sort = "Incentive_Month ASC"

            Chart.DataSource = dt
            Chart.DataBind()
            ' End If
            'If dt.Rows.Count > 6 And dt.Rows.Count < 12 Then
            '    Chartwrapper.Style.Add("width", (dt.Rows.Count * 40).ToString & "px")
            'ElseIf dt.Rows.Count > 6 Then
            '    Chartwrapper.Style.Add("width", (dt.Rows.Count * 35).ToString & "px")
            'Else
            '    Chartwrapper.Style.Add("width", "1000px")
            'End If

            Chartwrapper.Style.Add("width", "1000px")

            If dt.Rows.Count > 0 Then
                Chart.Visible = True
            Else
                Chart.Visible = False
            End If
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub BindData()
        Try
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetEmpIncentive(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, ddl_empcode.SelectedValue,ddl_year.SelectedItem.Text )

            Dim dataView As DataView = dt.DefaultView
            dataView.RowFilter = "Incentive_Year > 0"
            gvRep.DataSource = dataView
            gvRep.DataBind()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub

    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False
        If ddlOrganization.CheckedItems Is Nothing Then
            MessageBoxValidation("Select an Organisation", "Validation")
            SetFocus(ddlOrganization)
            bretval = False
            Return bretval
        Else
            If ddlOrganization.CheckedItems.Count <= 0 Then
                MessageBoxValidation("Select an Organisation", "Validation")
                SetFocus(ddlOrganization)
                bretval = False
                Return bretval
            End If
        End If
        bretval = True
        Return bretval
    End Function
    Sub MessageBoxValidation(ByVal str As String, Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Private Sub gvRep_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        BindData()
    End Sub

    Private Sub gvRep_SortCommand(sender As Object, e As GridSortCommandEventArgs) Handles gvRep.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindData()
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
    Private Sub BtnExportExcel_Click(sender As Object, e As EventArgs) Handles BtnExportExcel.Click
        Try
            If ddlOrganization.SelectedValue = "0" Then
                MessageBoxValidation("Select an Organisation", "Validation")
                Exit Sub
            End If
            If ddl_empcode.SelectedIndex <= 0 Then
                MessageBoxValidation("Select an Emp", "Validation")
                Exit Sub
            End If
            Export("Excel")

        Catch ex As Exception
            log.Debug(ex.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
    Sub Export(format As String)

        Try
            Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
            rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
            rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
            rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim Searchvalue As New ReportParameter

            Dim fromdate As DateTime
            Dim todate As DateTime

           

            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("OrgId", ddlOrganization.SelectedItem.Value)



            Dim EMP_CODE As New ReportParameter
            EMP_CODE = New ReportParameter("EMP_CODE", ddl_empcode.SelectedValue)


            Dim YEAR As New ReportParameter
            YEAR = New ReportParameter("YEAR", ddl_year.SelectedItem.Text)


            Dim OrgName As New ReportParameter
            OrgName = New ReportParameter("OrgName", ddlOrganization.SelectedItem.Text)


            Dim EmpName As New ReportParameter
            EmpName = New ReportParameter("EmpName", ddl_empcode.SelectedItem.Text)

            rview.ServerReport.SetParameters(New ReportParameter() {OrgId, EMP_CODE, YEAR, OrgName, EmpName})

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
                Response.AddHeader("Content-disposition", "attachment;filename=Incentive.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
            ElseIf format = "Excel" Then
                Response.ContentType = "application/excel"
                Response.AddHeader("Content-disposition", "filename=Incentive.xls")
                Response.AddHeader("Content-Length", bytes.Length)
            End If
            Response.OutputStream.Write(bytes, 0, bytes.Length)
            Response.OutputStream.Flush()
            Response.OutputStream.Close()
            Response.Flush()
            Response.Close()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub BtnExportPDF_Click(sender As Object, e As EventArgs) Handles BtnExportPDF.Click
        Try
            If ddlOrganization.SelectedValue = "0" Then
                MessageBoxValidation("Select an Organisation", "Validation")
                Exit Sub
            End If
            If ddl_empcode.SelectedIndex <= 0 Then
                MessageBoxValidation("Select an Employee", "Validation")
                Exit Sub
            End If
            Export("PDF")

        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Debug(ex.ToString())
        End Try
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ObjCommon = New SalesWorx.BO.Common.Common()
        Dim CountryTbl As DataTable = Nothing
        Dim orgTbl As DataTable = Nothing
        CountryTbl = ObjCommon.GetCountry(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)
        Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
        orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
        Dim s() As String = Nothing
 
        Dim OrgStr As String = Nothing
        For Each item As RadComboBoxItem In ddlOrganization.Items
            item.Checked = True
            If item.Checked Then

                OrgStr = OrgStr & "," & item.Value

            End If
        Next
        ddl_year.SelectedIndex = 0
        ddl_empcode.SelectedIndex = 0
        ddlOrganization.SelectedIndex = 0
        gvRep.Visible = False
        Args.Visible = False
        Chart.Visible = False
        Chartwrapper.Visible = False
    End Sub

    Private Sub ddlOrganization_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlOrganization.SelectedIndexChanged
        Try

       
        ddl_empcode.ClearSelection()
        gvRep.Visible = False
        Chart.Visible = False
        If Not (ddlOrganization.SelectedItem.Value = "0") Then
            Loademp()
            ddl_empcode.ClearSelection()

            End If
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))

        End Try
    End Sub
    Private Sub BtnExportBiffExcel_Click(sender As Object, e As EventArgs) Handles BtnExportBiffExcel.Click
        Try

       
        Dim tblData As New DataTable

        Dim ObjReport As New SalesWorx.BO.Common.Reports
        Dim dt As New DataTable
        dt = ObjReport.GetEmpIncentive(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, ddl_empcode.SelectedValue, ddl_year.SelectedItem.Text)
        tblData = dt.DefaultView.ToTable(False, "Incentive_Year", "Tmonth", "NET_SALES_VALUE", "NET_SALES_VOLUME", "SUCCESSFUL_VISITS", "Sales_Value_Acheived", "Sales_Volume_Acheived", "Success_Visits_Acheived", "TotalCommission", "CommissionTarget")


        For Each col In tblData.Columns
            If col.ColumnName = "Incentive_Year" Then
                col.ColumnName = "Year"
            End If

            If col.ColumnName = "Tmonth" Then
                col.ColumnName = "Month"
            End If
            If col.ColumnName = "NET_SALES_VALUE" Then
                    col.ColumnName = "Target Sales Value"
            End If
            If col.ColumnName = "NET_SALES_VOLUME" Then
                    col.ColumnName = "Target Sales Volume"
            End If
            If col.ColumnName = "SUCCESSFUL_VISITS" Then
                    col.ColumnName = "Target Visits"
            End If
            If col.ColumnName = "Sales_Value_Acheived" Then
                col.ColumnName = "Acheived Sales Value"
            End If
            If col.ColumnName = "Sales_Volume_Acheived" Then
                col.ColumnName = "Acheived Sales Volume"
            End If
            If col.ColumnName = "Success_Visits_Acheived" Then
                col.ColumnName = "Acheived Visits"
            End If
            If col.ColumnName = "TotalCommission" Then
                col.ColumnName = "Total Commission"
            End If
            If col.ColumnName = "CommissionTarget" Then
                col.ColumnName = "Commission Target"
            End If

        Next





        If tblData.Rows.Count > 0 Then

            Using package As New ExcelPackage()
                ' add a new worksheet to the empty workbook
                Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
                Worksheet.Cells("A1").LoadFromDataTable(tblData, True)
                    Worksheet.Column(3).Style.Numberformat.Format = "#,###.00"
                    Worksheet.Column(4).Style.Numberformat.Format = "#,###.00"
                    Worksheet.Column(5).Style.Numberformat.Format = "#,###.00"
                    Worksheet.Column(6).Style.Numberformat.Format = "#,###.00"
                    Worksheet.Column(7).Style.Numberformat.Format = "#,###.00"
                    Worksheet.Column(8).Style.Numberformat.Format = "#,###.00"
                    Worksheet.Column(9).Style.Numberformat.Format = "#,###.00"
                    Worksheet.Column(10).Style.Numberformat.Format = "#,###.00"

                Worksheet.Cells.AutoFitColumns()

                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename= Incentive.xlsx")

                Using MyMemoryStream As New MemoryStream()
                    package.SaveAs(MyMemoryStream)
                    MyMemoryStream.WriteTo(Response.OutputStream)
                    Response.AddHeader("Content-Length", MyMemoryStream.Length)
                    Response.Flush()
                    Response.Close()
                End Using
            End Using
            End If
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
    Protected Sub btnMonthDetails_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
         

            Dim Lnk_Month As LinkButton = TryCast(sender, LinkButton)
            Dim row As GridViewRow = DirectCast(Lnk_Month.NamingContainer, GridViewRow)
            ' CustID.Value = DirectCast(row.FindControl("lblCusId"), Label).Text
            ' SiteID.Value = DirectCast(row.FindControl("lblSiteId"), Label).Text
            ' txtLoc_Latitude.Text = CDec(Server.HtmlDecode(Trim(IIf(row.Cells(4).Text Is DBNull.Value Or row.Cells(4).Text = "", "0", row.Cells(4).Text))).Trim()).ToString("0.000000")
            '   txtLoc_Long.Text = CDec(Server.HtmlDecode(Trim(IIf(row.Cells(5).Text Is DBNull.Value Or row.Cells(5).Text = "", "0", row.Cells(5).Text))).Trim()).ToString("0.000000")

            Dim geo_mod As String = "L"


            'Dim dt_geomod As New DataTable
            'dt_geomod = objLatitude.GetGEO_loc_mod(Err_No, Err_Desc)
            'If dt_geomod.Rows.Count > 0 Then
            '    If dt_geomod.Rows(0)("Control_Value").ToString().ToUpper().Trim() = "E" Then
            '        geo_mod = "E"
            '        Dim dt_Exgeoloc As New DataTable
            '        dt_Exgeoloc = objLatitude.GetExpGEOLocation(Err_No, Err_Desc, CustID.Value, SiteID.Value)
            '        If dt_Exgeoloc.Rows.Count > 0 Then
            '            hd_last_Lat.Value = dt_Exgeoloc.Rows(0)("Latitude")
            '            hd_last_Long.Value = dt_Exgeoloc.Rows(0)("Longitude")


            '        End If
            '    Else
            '        Dim dt_lastvisited As New DataTable
            '        dt_lastvisited = objLatitude.GetLastVisit(Err_No, Err_Desc, CustID.Value, SiteID.Value)
            '        If dt_lastvisited.Rows.Count > 0 Then
            '            hd_last_Lat.Value = dt_lastvisited.Rows(0)("Last_Latitude")
            '            hd_last_Long.Value = dt_lastvisited.Rows(0)("Last_Longitude")
            '        End If
            '    End If
            'End If



            ScriptManager.RegisterStartupScript(Me, Page.GetType(), "openPopUp", "javascript:OpenLocWindow( 0.0000 ,0.0000,0.0000, 0.0000,0.0000,0.0000);", True)



            'ScriptManager.RegisterStartupScript(Me, Page.GetType(), "openPopUp", "javascript:OpenLocWindow(" & txtLoc_Latitude.Text & "," & txtLoc_Long.Text & "," & hd_last_Lat.Value & "," & hd_last_Long.Value & "," & CustID.Value & "," & SiteID.Value & ",'" & geo_mod.Trim() & "');", True)



        Catch ex As Exception
            Err_No = "74066"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_005") & "&next=MngLatiLongitude.aspx&Title=Geolocation Management", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub
   
    Protected Sub gvRep_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles gvRep.ItemCommand
        Try
            If e.CommandName = "MonthDetails" Then
                Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
                Dim item As GridDataItem = DirectCast(e.Item, GridDataItem)
                Dim lblROW_Month As Label = DirectCast(item.FindControl("lblROW_Month"), Label)

                '  ScriptManager.RegisterStartupScript(Me, Page.GetType(), "openPopUp", "javascript:OpenLocWindow( " & ddlOrganization.SelectedValue & " ," & lblROW_Month.Text & "," & ddl_year.SelectedItem.Text & "," & ddl_empcode.SelectedValue & ");", True)

                Response.Redirect("RepIncentiveMonthDetails.aspx?Org_ID=" & ddlOrganization.SelectedValue & "&Incentive_Month=" & lblROW_Month.Text & "&Incentive_Year=" & ddl_year.SelectedItem.Text & "&Emp_Code=" & ddl_empcode.SelectedValue & "&Org_Name=" & ddlOrganization.SelectedItem.Text & "&EmpName=" & ddl_empcode.SelectedItem.Text)

            End If
        Catch ex As Exception

        End Try
    End Sub
End Class