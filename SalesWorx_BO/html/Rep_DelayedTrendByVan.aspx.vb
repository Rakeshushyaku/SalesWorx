Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Imports System.Linq
Imports System.Threading
Imports System.Globalization


Public Class Rep_DelayedTrendByVan
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As New SalesWorx.BO.Common.Common
    Dim ObjCollection As Collection
    Private ReportPath As String = "DelayedCollectionByVan"
    Private Const PageID As String = "P340"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
            Dim objRep = Nothing
            Dim CountryTbl As DataTable = Nothing
            Dim orgTbl As DataTable = Nothing
            Try


                ObjCommon = New SalesWorx.BO.Common.Common()


                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)



                CountryTbl = ObjCommon.GetCountry(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)
                ddlCountry.DataSource = CountryTbl
                ddlCountry.DataBind()


                orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                Dim s() As String = Nothing
                Dim Currency As String = Nothing
                Dim DecimalDigits As String = "N2"
                Dim country As String = Nothing
                If CountryTbl.Rows.Count = 1 Then

                    ddlCountry.SelectedIndex = 0
                    dvCountry.Visible = False

                    s = ddlCountry.SelectedValue.Split("$")

                    If s.Length > 0 Then
                        country = s(0).ToString()
                        Currency = s(1).ToString()
                        DecimalDigits = s(2).ToString()
                    End If


                    ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
                    ddlOrganization.DataBind()
                    Dim OrgStr As String = Nothing
                    For Each item As RadComboBoxItem In ddlOrganization.Items
                        item.Checked = True
                        If item.Checked Then

                            OrgStr = OrgStr & item.Value & ","

                        End If
                    Next
                    If Not String.IsNullOrEmpty(OrgStr) Then  '' Loading the managers
                        OrgStr = OrgStr.Substring(0, OrgStr.Length - 1)
                        objRep = New Reports()
                        ddManager.DataSource = objRep.GetManagersByOrg(Err_No, Err_Desc, OrgStr)
                        ddManager.DataBind()
                    End If
                ElseIf CountryTbl.Rows.Count > 1 Then
                    ddlCountry.SelectedIndex = 0
                    dvCountry.Visible = True


                    s = ddlCountry.SelectedValue.Split("$")

                    If s.Length > 0 Then
                        country = s(0).ToString()
                        Currency = s(1).ToString()
                        DecimalDigits = s(2).ToString()
                    End If

                    ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
                    ddlOrganization.DataBind()
                    Dim OrgStr As String = Nothing
                    For Each item As RadComboBoxItem In ddlOrganization.Items
                        item.Checked = True
                        If item.Checked Then

                            OrgStr = OrgStr & item.Value & ","

                        End If
                    Next

                    If Not String.IsNullOrEmpty(OrgStr) Then  '' Loading the managers
                        OrgStr = OrgStr.Substring(0, OrgStr.Length - 1)
                        objRep = New Reports()
                        ddManager.DataSource = objRep.GetManagersByOrg(Err_No, Err_Desc, OrgStr)
                        ddManager.DataBind()
                    End If
                End If




                '  ddlOrganization.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))
                txtFromDate.SelectedDate = Now

                'Dim culture = DirectCast(Thread.CurrentThread.CurrentCulture.Clone(), CultureInfo)
                'culture.DateTimeFormat.ShortDatePattern = "MM yyyy"
                'Me.txtFromDate.Culture = culture

                '' Binding Location
                objRep = New Reports()
                ddlLocation.DataSource = objRep.GetSalesDist(Err_No, Err_Desc)
                ddlLocation.DataBind()

                'BindData()
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


    End Sub
    Private Sub ddlCountry_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlCountry.SelectedIndexChanged
        LoadOrgs()
    End Sub
    Sub LoadOrgs()
        Dim orgTbl As DataTable = Nothing

        ObjCommon = New SalesWorx.BO.Common.Common()


        Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)



        orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)

        Dim s() As String = Nothing
        Dim Currency As String = Nothing
        Dim DecimalDigits As String = "2"
        Dim country As String = Nothing
        s = ddlCountry.SelectedValue.Split("$")

        If s.Length > 0 Then
            country = s(0).ToString()
            Currency = s(1).ToString()
            DecimalDigits = s(2).ToString()
        End If

        ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
        ddlOrganization.DataBind()

        Dim OrgStr As String = Nothing
        For Each item As RadComboBoxItem In ddlOrganization.Items
            item.Checked = True
            If item.Checked Then

                OrgStr = OrgStr & "," & item.Value

            End If
        Next

    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            gvRep.Visible = True
            BindReport()
        Else
            Args.Visible = False
            gvRep.Visible = False
        End If
    End Sub

    Private Sub BindReport()
        Try
            Dim SearchQuery As String = ""
            Dim orgStr As String = ""
            Dim orgtxt As String = ""
            For Each item As RadComboBoxItem In ddlOrganization.Items
                If item.Checked Then
                    If orgStr Is String.Empty Then
                        orgStr = String.Format("{0}", item.Value)
                        orgtxt = String.Format("{0}", item.Text)
                    Else
                        orgStr = String.Format("{1},{0}", item.Value, orgStr)
                        orgtxt = String.Format("{1},{0}", item.Text, orgtxt)
                    End If
                End If
            Next


            If String.IsNullOrEmpty(orgStr) Then
                MessageBoxValidation("Select organization(s).", "Validation")
                Exit Sub
            Else
                orgtxt = orgtxt.Substring(0, orgtxt.Length - 1)
            End If


            rpbFilter.Items(0).Expanded = False

            lbl_org.Text = orgtxt


            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)



            Dim LocationStr As String = ""
            Dim MgrStr As String = ""
            Dim Locationtxt As String = ""
            Dim Mgrtxt As String = ""

            Dim StartDate As String
            Dim EndDate As String

            For Each item As RadComboBoxItem In ddManager.Items
                If item.Checked Then
                    If MgrStr Is String.Empty Then
                        MgrStr = String.Format("{0}", item.Value)
                        Mgrtxt = String.Format("{0}", item.Text)
                    Else
                        MgrStr = String.Format("{1},{0}", item.Value, MgrStr)
                        Mgrtxt = String.Format("{1},{0}", item.Text, Mgrtxt)
                    End If
                End If
            Next
            If Mgrtxt <> "" Then
                Mgrtxt = Mgrtxt.Substring(0, Mgrtxt.Length - 1)
            End If
            If MgrStr = "" Then
                lbl_Manager.Text = "All"
            Else
                lbl_Manager.Text = Mgrtxt
            End If
            For Each item As RadComboBoxItem In ddlLocation.Items
                If item.Checked Then
                    If LocationStr Is String.Empty Then
                        LocationStr = String.Format("'{0}'", item.Value)
                        Locationtxt = String.Format("{0}", item.Text)
                    Else
                        LocationStr = String.Format("{1},'{0}'", item.Value, LocationStr)
                        Locationtxt = String.Format("{1},{0}", item.Text, Locationtxt)
                    End If
                End If
            Next

            If Locationtxt <> "" Then
                Locationtxt = Locationtxt.Substring(0, Locationtxt.Length - 1)
            End If
            If LocationStr = "" Then
                lbl_Loc.Text = "All"
            Else
                lbl_Loc.Text = Locationtxt
            End If

            lbl_from.Text = CDate(txtFromDate.SelectedDate).ToString("MMM-yyyy")
            Args.Visible = True

            StartDate = String.Format("{0}-{1}-{2}", "1", txtFromDate.SelectedDate.Value.Month, txtFromDate.SelectedDate.Value.Year)
            EndDate = String.Format("{0}-{1}-{2}", Date.DaysInMonth(txtFromDate.SelectedDate.Value.Year, txtFromDate.SelectedDate.Value.Month), txtFromDate.SelectedDate.Value.Month, txtFromDate.SelectedDate.Value.Year)



            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetDelayedCollectionByVan(Err_No, Err_Desc, orgStr, MgrStr, LocationStr, StartDate, EndDate)
            gvRep.DataSource = dt
            gvRep.DataBind()



        Catch Ex As Exception
            log.Error(Ex.Message)
        End Try
    End Sub


    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = False

        Dim orgStr As String = ""

        For Each item As RadComboBoxItem In ddlOrganization.Items
            If item.Checked Then
                If orgStr Is String.Empty Then
                    orgStr = String.Format("'{0}'", item.Value)
                Else
                    orgStr = String.Format("{1},'{0}'", item.Value, orgStr)
                End If
            End If
        Next


        If String.IsNullOrEmpty(orgStr) Then
            MessageBoxValidation("Select organization(s).", "Validation")
            Return bretval
        End If

        If txtFromDate.SelectedDate Is Nothing Then
            MessageBoxValidation("Select ""Date"".", "Validation")
            SetFocus(txtFromDate)
            Return bretval
        End If

        Return True

    End Function
    Private Sub dummyOrgBtn_Click(sender As Object, e As EventArgs) Handles dummyOrgBtn.Click
        Dim objRep = Nothing
        Try
            Dim OrgStr As String = String.Empty

            For Each item As RadComboBoxItem In ddlOrganization.Items
                If item.Checked Then
                    If OrgStr Is String.Empty Then
                        OrgStr = String.Format("'{0}'", item.Value)
                    Else
                        OrgStr = String.Format("{1},'{0}'", item.Value, OrgStr)
                    End If
                End If
            Next

            If Not String.IsNullOrEmpty(OrgStr) Then  '' Loading the managers
                objRep = New Reports()
                ddManager.DataSource = objRep.GetManagersByOrg(Err_No, Err_Desc, OrgStr)
                ddManager.DataBind()
            End If

        Catch ex As Exception
            Err_No = "7477866"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
        End Try
    End Sub

    Private Sub gvRep_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles gvRep.ItemDataBound


        'If e.Item.ItemType = GridItemType.GroupFooter Then
        '    Dim itm As GridGroupFooterItem = CType(e.Item, GridGroupFooterItem)
        '    If itm IsNot Nothing Then

        '    End If
        'End If

        Try
            If e.Item.ItemType = GridItemType.Item Then
                Dim lblCurr As Label = e.Item.FindControl("Label1")
                Dim lblDec As Label = e.Item.FindControl("lblDigits")
                Dim lblAmount As Label = e.Item.FindControl("lblAmount")
                CType(gvRep.Columns(1), GridTemplateColumn).FooterAggregateFormatString = "Total : " & lblCurr.Text & " {0:N" & lblDec.Text & "}"

            End If
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

    Private Sub gvRep_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        BindReport()
    End Sub

    Private Sub gvRep_SortCommand(sender As Object, e As GridSortCommandEventArgs) Handles gvRep.SortCommand
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindReport()
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

    Private Sub BtnPDF_Click(sender As Object, e As EventArgs) Handles BtnExportPDF.Click
        Try
            If ValidateInputs() Then
                Export("PDF")
            End If
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub
    Sub Export(format As String)

        Dim SearchQuery As String = ""
        Dim orgStr As String = ""

        For Each item As RadComboBoxItem In ddlOrganization.Items
            If item.Checked Then
                If orgStr Is String.Empty Then
                    orgStr = String.Format("{0}", item.Value)
                Else
                    orgStr = String.Format("{1},{0}", item.Value, orgStr)
                End If
            End If
        Next


        If String.IsNullOrEmpty(orgStr) Then
            MessageBoxValidation("Select organization(s).", "Validation")
            Exit Sub
        End If


        rpbFilter.Items(0).Expanded = False
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)



        Dim LocationStr As String = ""
        Dim MgrStr As String = ""
        Dim StartDate As String
        Dim EndDate As String

        For Each item As RadComboBoxItem In ddManager.Items
            If item.Checked Then
                If MgrStr Is String.Empty Then
                    MgrStr = String.Format("{0}", item.Value)
                Else
                    MgrStr = String.Format("{1},{0}", item.Value, MgrStr)
                End If
            End If
        Next

        For Each item As RadComboBoxItem In ddlLocation.Items
            If item.Checked Then
                If LocationStr Is String.Empty Then
                    LocationStr = String.Format("'{0}'", item.Value)
                Else
                    LocationStr = String.Format("{1},'{0}'", item.Value, LocationStr)
                End If
            End If
        Next

        StartDate = String.Format("{0}-{1}-{2}", "1", txtFromDate.SelectedDate.Value.Month, txtFromDate.SelectedDate.Value.Year)
        EndDate = String.Format("{0}-{1}-{2}", Date.DaysInMonth(txtFromDate.SelectedDate.Value.Year, txtFromDate.SelectedDate.Value.Month), txtFromDate.SelectedDate.Value.Month, txtFromDate.SelectedDate.Value.Year)


        Dim rview As New Microsoft.Reporting.WebForms.ReportViewer()
        rview.ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
        rview.ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
        rview.ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Me.ReportPath)



        Dim org As New ReportParameter
        org = New ReportParameter("OrgStr", orgStr)

        If String.IsNullOrEmpty(MgrStr) Then
            MgrStr = "-1"
        End If

        If String.IsNullOrEmpty(LocationStr) Then
            LocationStr = "-1"
        End If

        Dim mgr As New ReportParameter
        mgr = New ReportParameter("MgrStr", MgrStr)

        Dim loc As New ReportParameter
        loc = New ReportParameter("LocStr", LocationStr)

        Dim sdat As New ReportParameter
        sdat = New ReportParameter("SDate", StartDate)

        Dim edat As New ReportParameter
        edat = New ReportParameter("EDate", EndDate)

        rview.ServerReport.SetParameters(New ReportParameter() {org, mgr, loc, sdat, edat})

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
            Response.AddHeader("Content-disposition", "attachment;filename=DelayedCollections.pdf")
            Response.AddHeader("Content-Length", bytes.Length)
        ElseIf format = "Excel" Then
            Response.ContentType = "application/excel"
            Response.AddHeader("Content-disposition", "filename=DelayedCollections.xls")
            Response.AddHeader("Content-Length", bytes.Length)
        End If
        Response.OutputStream.Write(bytes, 0, bytes.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.Flush()
        Response.Close()
    End Sub

    Private Sub BtnExportExcel_Click(sender As Object, e As EventArgs) Handles BtnExportExcel.Click
        Try
            If ValidateInputs() Then
                Export("Excel")
            End If
        Catch ex As Exception
            log.Debug(ex.ToString())
        End Try
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        ObjCommon = New SalesWorx.BO.Common.Common()
        Dim objRep = Nothing
        Dim CountryTbl As DataTable = Nothing
        Dim orgTbl As DataTable = Nothing

        Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)



        CountryTbl = ObjCommon.GetCountry(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)
        ddlCountry.DataSource = CountryTbl
        ddlCountry.DataBind()


        orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
        Dim s() As String = Nothing
        Dim Currency As String = Nothing
        Dim DecimalDigits As String = "N2"
        Dim country As String = Nothing
        If CountryTbl.Rows.Count = 1 Then

            ddlCountry.SelectedIndex = 0
            dvCountry.Visible = False

            s = ddlCountry.SelectedValue.Split("$")

            If s.Length > 0 Then
                country = s(0).ToString()
                Currency = s(1).ToString()
                DecimalDigits = s(2).ToString()
            End If


            ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
            ddlOrganization.DataBind()
            Dim OrgStr As String = Nothing
            For Each item As RadComboBoxItem In ddlOrganization.Items
                item.Checked = True
                If item.Checked Then

                    OrgStr = OrgStr & item.Value & ","

                End If
            Next
            If Not String.IsNullOrEmpty(OrgStr) Then  '' Loading the managers
                OrgStr = OrgStr.Substring(0, OrgStr.Length - 1)
                objRep = New Reports()
                ddManager.DataSource = objRep.GetManagersByOrg(Err_No, Err_Desc, OrgStr)
                ddManager.DataBind()
            End If
        ElseIf CountryTbl.Rows.Count > 1 Then
            ddlCountry.SelectedIndex = 0
            dvCountry.Visible = True


            s = ddlCountry.SelectedValue.Split("$")

            If s.Length > 0 Then
                country = s(0).ToString()
                Currency = s(1).ToString()
                DecimalDigits = s(2).ToString()
            End If

            ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
            ddlOrganization.DataBind()
            Dim OrgStr As String = Nothing
            For Each item As RadComboBoxItem In ddlOrganization.Items
                item.Checked = True
                If item.Checked Then

                    OrgStr = OrgStr & item.Value & ","

                End If
            Next

            If Not String.IsNullOrEmpty(OrgStr) Then  '' Loading the managers
                OrgStr = OrgStr.Substring(0, OrgStr.Length - 1)
                objRep = New Reports()
                ddManager.DataSource = objRep.GetManagersByOrg(Err_No, Err_Desc, OrgStr)
                ddManager.DataBind()
            End If
        End If




        '  ddlOrganization.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))
        txtFromDate.SelectedDate = Now
        ddlLocation.ClearCheckedItems()

        Args.Visible = False
        gvRep.Visible = False
    End Sub
End Class