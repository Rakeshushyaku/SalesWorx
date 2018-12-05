
Imports Telerik.Web.UI
Imports SalesWorx.BO.Common
Imports Telerik.Charting
Imports System.IO
Imports System.Xml
Imports System.Drawing.Imaging
Imports System.Drawing
Imports log4net
Public Class SWXDashboard
    Inherits System.Web.UI.Page
    Private Const ModuleName As String = "ASRDashboard.aspx"
    Private Const PageID As String = "P87"
    Dim Err_No As Long
    Dim objDash As New DashboardCom()
    Dim ObjCommon As New SalesWorx.BO.Common.Common
    Dim Err_Desc As String

    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Button_Click(sender As Object, e As ImageClickEventArgs)
        gvLog.MasterTableView.FilterExpression = String.Empty
        gvLog.MasterTableView.Columns(0).CurrentFilterValue = ""
        gvLog.MasterTableView.Rebind()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Session.Item("USER_ACCESS") Is Nothing Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If

            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)


            If CType(Session.Item("USER_ACCESS"), UserAccess).IsSS <> "N" Then
                hyp_IncomingMsg.Visible = True
            Else
                hyp_IncomingMsg.Visible = False
            End If

            If Not HasPermission Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If

            Dim CountryTbl As DataTable = Nothing
            Dim orgTbl As DataTable = Nothing

            ObjCommon = New SalesWorx.BO.Common.Common()


            Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)

            CountryTbl = ObjCommon.GetCountry(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)
            ddlCountry.DataSource = CountryTbl
            ddlCountry.DataBind()
            Dim s() As String = Nothing
            Dim Currency As String = Nothing
            Dim DecimalDigits As String = "2"
            Dim country As String = Nothing

            orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)

            If CountryTbl.Rows.Count = 1 Then


                ddlCountry.SelectedIndex = 0
                s = ddlCountry.SelectedValue.Split("$")

                If s.Length > 0 Then
                    country = s(0).ToString()
                    Currency = s(1).ToString()
                    DecimalDigits = s(2).ToString()
                End If

                Me.hfCurrency.Value = Currency
                Me.hfDecimal.Value = DecimalDigits
                tdctry2.Visible = False

                ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
                ddlOrganization.DataBind()


            ElseIf CountryTbl.Rows.Count > 1 Then

                If Not Session("DashCurrency") Is Nothing Then
                    If Not ddlCountry.Items.FindItemByValue(Session("DashCurrency")) Is Nothing Then
                        ddlCountry.Items.FindItemByValue(Session("DashCurrency")).Selected = True
                    End If
                Else
                    ddlCountry.SelectedIndex = 0
                End If


                tdctry2.Visible = True


                s = ddlCountry.SelectedValue.Split("$")

                If s.Length > 0 Then
                    country = s(0).ToString()
                    Currency = s(1).ToString()
                    DecimalDigits = s(2).ToString()
                End If
                Me.hfCurrency.Value = Currency
                Me.hfDecimal.Value = DecimalDigits
                ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
                ddlOrganization.DataBind()

            End If

            Dim OrgStr As String = Nothing
            For Each item As RadComboBoxItem In ddlOrganization.Items
                item.Checked = True
                If item.Checked Then

                    OrgStr = OrgStr & "," & item.Value

                End If
            Next



            Me.lblC.Text = Me.hfCurrency.Value

            If Not Session("DashMonth") Is Nothing Then
                Me.StartTime.SelectedDate = Session("DashMonth")
            Else
                Me.StartTime.SelectedDate = Now.Date
            End If


            Dim Sdate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM") + "-01-" + DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("yyyy")

            Me.hfSMonth.Value = Sdate
            Me.hfUserID.Value = CType(Session.Item("USER_ACCESS"), UserAccess).UserID

            FillAssigenTo(OrgStr)

            Me.lblCollCurr.Text = hfCurrency.Value
            Me.lblOrdCuur.Text = hfCurrency.Value
            lblRMACurr.Text = hfCurrency.Value

            BindStatistics()
            Me.gvLog.Rebind()
            CType(gvLog.Columns(1), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
            CType(gvLog.Columns(2), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"
            CType(gvLog.Columns(3), GridBoundColumn).DataFormatString = "{0:N" & hfDecimal.Value & "}"


            CType(gvLog.Columns(1), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
            CType(gvLog.Columns(2), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"
            CType(gvLog.Columns(3), GridBoundColumn).FooterAggregateFormatString = "{0:N" & hfDecimal.Value & "}"

            gvLog.Columns(1).HeaderText = "Total  Sales (" & Me.lblC.Text & ")"
            gvLog.Columns(2).HeaderText = "Total Returns (" & Me.lblC.Text & ")"
            gvLog.Columns(3).HeaderText = "Total Collections (" & Me.lblC.Text & ")"




            ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)

        Else
            ZeroBilledWindow.VisibleOnPageLoad = False
        End If

    End Sub
    Protected Sub ddlCountry_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlCountry.SelectedIndexChanged
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
        Me.hfCurrency.Value = Currency
        Me.hfDecimal.Value = DecimalDigits

        ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
        ddlOrganization.DataBind()

        Me.lblC.Text = Me.hfCurrency.Value

        Dim OrgStr As String = Nothing
        For Each item As RadComboBoxItem In ddlOrganization.Items
            item.Checked = True
            If item.Checked Then

                OrgStr = OrgStr & "," & item.Value

            End If
        Next

        Me.lblCollCurr.Text = hfCurrency.Value
        Me.lblOrdCuur.Text = hfCurrency.Value
        lblRMACurr.Text = hfCurrency.Value

      

        FillAssigenTo(OrgStr)
        BindStatistics()

       

        ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)


    End Sub
    Private Function BindFSR(OrgStr As String) As DataTable

        Dim dt As New DataTable
        Dim objRep As New Reports
        dt = objRep.GetAllOrgVan(Err_No, Err_Desc, IIf(OrgStr Is Nothing, "0", OrgStr), CType(Session("User_Access"), UserAccess).UserID)

        Dim dr As DataRow = dt.NewRow
        dr(0) = "0"
        dr(1) = "All"
        dt.Rows.InsertAt(dr, 0)

        'Try
        '    Dim TempTbl As New DataTable
        '    Dim MyRow As DataRow
        '    TempTbl.Columns.Add(New DataColumn("SalesRep_ID", _
        '        GetType(Int32)))
        '    TempTbl.Columns.Add(New DataColumn("SalesRep_Name", _
        '        GetType(String)))
        '    Dim obj As UserAccess
        '    obj = CType(Session("User_Access"), UserAccess)
        '    If obj.AssignedSalesReps.Count = obj.VanName.Count Then
        '        For i = 0 To obj.AssignedSalesReps.Count - 1
        '            If obj.AssignedSalesReps(i) IsNot DBNull.Value And obj.VanName(i) IsNot DBNull.Value Then
        '                MyRow = TempTbl.NewRow()
        '                MyRow(0) = obj.AssignedSalesReps(i)
        '                MyRow(1) = obj.VanName(i)
        '                TempTbl.Rows.Add(MyRow)
        '            End If
        '        Next
        '    End If

        '    Return TempTbl
        'Catch ex As Exception
        '    Throw ex
        'End Try
        Return dt
    End Function
    Sub BindStatistics()
        Dim success As Boolean = False
        Dim Sdate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM") + "-01-" + DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("yyyy")

        Me.hfSMonth.Value = Sdate
        Try
            Dim ds As New DataSet
            Dim dt As New DataTable

            Dim LabelDecimalDigits As String = "0.00"
            If Me.hfDecimal.Value = 0 Then
                LabelDecimalDigits = "0"
            ElseIf Me.hfDecimal.Value = 1 Then
                LabelDecimalDigits = "0.0"
            ElseIf Me.hfDecimal.Value = 2 Then
                LabelDecimalDigits = "0.00"
            ElseIf Me.hfDecimal.Value = 3 Then
                LabelDecimalDigits = "0.000"
            ElseIf Me.hfDecimal.Value >= 4 Then
                LabelDecimalDigits = "0.0000"
            End If


            ds = objDash.LoadDashBoard(Err_No, Err_Desc, Me.hfSE.Value, Me.hfSMonth.Value)
            If ds.Tables.Count = 2 Then

                dt = ds.Tables(0).Copy


                Me.lblScheduled.Text = "0"
                Me.lblOutlets.Text = "0"
                Me.lblTotCalls.Text = "0"
                Me.lblProdCalls.Text = "0"
                Me.lblCoverage.Text = "0%"
                Me.lblProdPercent.Text = "0%"
                Me.lblZeroBilled.Text = "0"
                Me.lblOutBilled.Text = "0"
                lblOrdCnt.Text = "0"
                lblRMACount.Text = "0"
                lblColCnt.Text = "0"

                lblOrdValue.Text = "0"
                lblRMAValue.Text = "0"
                lblColValue.Text = "0"

                Me.lblAvgOrd.Text = "0"
                Me.lblAvgCol.Text = "0"
                Me.lblRMAAvg.Text = "0"

                If dt.Rows.Count > 0 Then

                    For Each r As DataRow In dt.Rows
                        If r("Description").ToString() = "Scheduled Outlets" Then
                            Me.lblScheduled.Text = CDec(r("TotValue").ToString()).ToString("#,##0")

                        ElseIf r("Description").ToString() = "Outlets Covered" Then
                            Me.lblOutlets.Text = CDec(r("TotValue").ToString()).ToString("#,##0")
                        ElseIf r("Description").ToString() = "Total Calls" Then
                            Me.lblTotCalls.Text = CDec(r("TotValue").ToString()).ToString("#,##0")
                        ElseIf r("Description").ToString() = "Productive Calls" Then
                            Me.lblProdCalls.Text = CDec(r("TotValue").ToString()).ToString("#,##0")
                        ElseIf r("Description").ToString() = "Orders Count" Then
                            Me.lblOrdCnt.Text = CDec(r("TotValue").ToString()).ToString("#,##0")
                        ElseIf r("Description").ToString() = "Orders Count" Then
                            Me.lblOrdCnt.Text = CDec(r("TotValue").ToString()).ToString("#,##0")
                        ElseIf r("Description").ToString() = "Orders Value" Then
                            Me.lblOrdValue.Text = CDec(r("TotValue").ToString()).ToString("#,##" & LabelDecimalDigits)
                        ElseIf r("Description").ToString() = "RMA Value" Then
                            Me.lblRMAValue.Text = CDec(r("TotValue").ToString()).ToString("#,##" & LabelDecimalDigits)
                        ElseIf r("Description").ToString() = "RMA Count" Then
                            Me.lblRMACount.Text = CDec(r("TotValue").ToString()).ToString("#,##0")
                        ElseIf r("Description").ToString() = "Collections Count" Then
                            Me.lblColCnt.Text = CDec(r("TotValue").ToString()).ToString("#,##0")
                        ElseIf r("Description").ToString() = "Collections Value" Then
                            Me.lblColValue.Text = CDec(r("TotValue").ToString()).ToString("#,##" & LabelDecimalDigits)
                        ElseIf r("Description").ToString() = "Zero Billed" Then
                            Me.lblZeroBilled.Text = CDec(r("TotValue").ToString()).ToString("#,##0")
                        ElseIf r("Description").ToString() = "Outlets Billed" Then
                            Me.lblOutBilled.Text = CDec(r("TotValue").ToString()).ToString("#,##0")
                        End If

                    Next
                    Me.lblCoverage.Text = CStr(CInt(Math.Round((CDec(IIf(Me.lblOutlets.Text = "", "0", Me.lblOutlets.Text)) / CDec(IIf(Me.lblScheduled.Text = "" Or Me.lblScheduled.Text = "0", "1", Me.lblScheduled.Text))) * 100.0))) & "%"
                    Me.lblProdPercent.Text = CStr(CInt(Math.Round((CDec(IIf(Me.lblProdCalls.Text = "", "0", Me.lblProdCalls.Text)) / CDec(IIf(Me.lblTotCalls.Text = "" Or Me.lblTotCalls.Text = "0", "1", Me.lblTotCalls.Text))) * 100.0))) & "%"

                    Me.lblAvgOrd.Text = CDec((CDec(IIf(Me.lblOrdValue.Text = "", "0", Me.lblOrdValue.Text)) / CDec(IIf(Me.lblOrdCnt.Text = "" Or Me.lblOrdCnt.Text = "0", "1", Me.lblOrdCnt.Text)))).ToString("#,##" & LabelDecimalDigits)
                    Me.lblAvgCol.Text = CDec((CDec(IIf(Me.lblColValue.Text = "", "0", Me.lblColValue.Text)) / CDec(IIf(Me.lblColCnt.Text = "" Or Me.lblColCnt.Text = "0", "1", Me.lblColCnt.Text)))).ToString("#,##" & LabelDecimalDigits)
                    Me.lblRMAAvg.Text = CDec((CDec(IIf(Me.lblRMAValue.Text = "", "0", Me.lblRMAValue.Text)) / CDec(IIf(Me.lblRMACount.Text = "" Or Me.lblRMACount.Text = "0", "1", Me.lblRMACount.Text)))).ToString("#,##" & LabelDecimalDigits)

                    ViewState("VanLog") = ds.Tables(1).Copy
                Else
                    Me.lblScheduled.Text = "0"
                    Me.lblOutlets.Text = "0"
                    Me.lblTotCalls.Text = "0"
                    Me.lblProdCalls.Text = "0"
                    Me.lblCoverage.Text = "0%"
                    Me.lblProdPercent.Text = "0%"
                    lblOrdCnt.Text = "0"
                    lblRMACount.Text = "0"
                    lblColCnt.Text = "0"
                    Me.lblZeroBilled.Text = "0"
                    lblOrdValue.Text = "0"
                    lblRMAValue.Text = "0"
                    lblColValue.Text = "0"
                    Me.lblOutBilled.Text = "0"
                    Me.lblAvgOrd.Text = "0"
                    Me.lblAvgCol.Text = "0"
                    Me.lblRMAAvg.Text = "0"

                End If
            Else
                Me.lblScheduled.Text = "0"
                Me.lblOutlets.Text = "0"
                Me.lblTotCalls.Text = "0"
                Me.lblProdCalls.Text = "0"
                Me.lblCoverage.Text = "0%"
                Me.lblProdPercent.Text = "0%"
                lblOrdCnt.Text = "0"
                lblRMACount.Text = "0"
                lblColCnt.Text = "0"
                Me.lblZeroBilled.Text = "0"
                lblOrdValue.Text = "0"
                lblRMAValue.Text = "0"
                lblColValue.Text = "0"
                Me.lblOutBilled.Text = "0"
                Me.lblAvgOrd.Text = "0"
                Me.lblAvgCol.Text = "0"
                Me.lblRMAAvg.Text = "0"
            End If
            'Dim dtTrx As New DataTable
            'dtTrx = objDash.LoadDashTRXStatistics(Err_No, Err_Desc, Me.hfSE.Value, Me.hfSMonth.Value)

            'If dtTrx.Rows.Count > 0 Then
            '    For Each r As DataRow In dtTrx.Rows

            '        If r("Description").ToString() = "Orders Count" Then
            '            Me.lblOrdCnt.Text = CDec(r("TotValue").ToString()).ToString("#,##0")
            '        ElseIf r("Description").ToString() = "Orders Count" Then
            '            Me.lblOrdCnt.Text = CDec(r("TotValue").ToString()).ToString("#,##0")
            '        ElseIf r("Description").ToString() = "Orders Value" Then
            '            Me.lblOrdValue.Text = CDec(r("TotValue").ToString()).ToString("#,##" & LabelDecimalDigits)
            '        ElseIf r("Description").ToString() = "RMA Value" Then
            '            Me.lblRMAValue.Text = CDec(r("TotValue").ToString()).ToString("#,##" & LabelDecimalDigits)
            '        ElseIf r("Description").ToString() = "RMA Count" Then
            '            Me.lblRMACount.Text = CDec(r("TotValue").ToString()).ToString("#,##0")
            '        ElseIf r("Description").ToString() = "Collections Count" Then
            '            Me.lblColCnt.Text = CDec(r("TotValue").ToString()).ToString("#,##0")
            '        ElseIf r("Description").ToString() = "Collections Value" Then
            '            Me.lblColValue.Text = CDec(r("TotValue").ToString()).ToString("#,##" & LabelDecimalDigits)
            '        ElseIf r("Description").ToString() = "Zero Billed" Then
            '            Me.lblZeroBilled.Text = CDec(r("TotValue").ToString()).ToString("#,##0")
            '        ElseIf r("Description").ToString() = "Outlets Billed" Then
            '            Me.lblOutBilled.Text = CDec(r("TotValue").ToString()).ToString("#,##0")
            '        End If

            '    Next
            '    Me.lblAvgOrd.Text = CDec((CDec(IIf(Me.lblOrdValue.Text = "", "0", Me.lblOrdValue.Text)) / CDec(IIf(Me.lblOrdCnt.Text = "" Or Me.lblOrdCnt.Text = "0", "1", Me.lblOrdCnt.Text)))).ToString("#,##" & LabelDecimalDigits)
            '    Me.lblAvgCol.Text = CDec((CDec(IIf(Me.lblColValue.Text = "", "0", Me.lblColValue.Text)) / CDec(IIf(Me.lblColCnt.Text = "" Or Me.lblColCnt.Text = "0", "1", Me.lblColCnt.Text)))).ToString("#,##" & LabelDecimalDigits)
            '    Me.lblRMAAvg.Text = CDec((CDec(IIf(Me.lblRMAValue.Text = "", "0", Me.lblRMAValue.Text)) / CDec(IIf(Me.lblRMACount.Text = "" Or Me.lblRMACount.Text = "0", "1", Me.lblRMACount.Text)))).ToString("#,##" & LabelDecimalDigits)


            'Else

            '    lblOrdCnt.Text = "0"
            '    lblRMACount.Text = "0"
            '    lblColCnt.Text = "0"
            '    Me.lblZeroBilled.Text = "0"
            '    lblOrdValue.Text = "0"
            '    lblRMAValue.Text = "0"
            '    lblColValue.Text = "0"
            '    Me.lblOutBilled.Text = "0"
            '    Me.lblAvgOrd.Text = "0"
            '    Me.lblAvgCol.Text = "0"
            '    Me.lblRMAAvg.Text = "0"

            'End If
            ds = Nothing
        Catch ex As Exception
            Err_No = "70926"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
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

    Sub FillAssigenTo(Org As String)
        ddlFSR.DataSource = BindFSR(Org)
        ddlFSR.DataTextField = "SalesRep_Name"
        ddlFSR.DataValueField = "SalesRep_ID"
        ddlFSR.DataBind()
        ddlFSR.SelectedIndex = 0
        hfRow.Value = ddlFSR.SelectedIndex

        Dim VanList As String = Nothing
        If ddlFSR.SelectedIndex = 0 Then
            For Each itm As RadComboBoxItem In ddlFSR.Items

                VanList = VanList & itm.Value & ","

            Next
        Else
            VanList = Me.ddlFSR.SelectedValue & ","
        End If
        hfSE.Value = VanList
        Dim AllVanList As String = Nothing
        For Each itm As RadComboBoxItem In ddlFSR.Items
            If itm.Value <> "0" Then
                AllVanList = AllVanList & itm.Value & ","
            End If
        Next
        hfAllVan.Value = AllVanList
    End Sub

    Protected Sub StartTime_SelectedDateChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs) Handles StartTime.SelectedDateChanged
        RebindAllData()
    End Sub
    Private Sub RebindAllData()
        Dim Sdate As String = DateTime.Parse(Me.StartTime.SelectedDate.Value.ToString()).ToString("MM-dd-yyyy")
        Me.hfSMonth.Value = Sdate
        gvLog.Columns(1).HeaderText = "Total  Sales (" & Me.lblC.Text & ")"
        gvLog.Columns(2).HeaderText = "Total Returns (" & Me.lblC.Text & ")"
        gvLog.Columns(3).HeaderText = "Total Collections (" & Me.lblC.Text & ")"
        BindStatistics()
        ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
        Me.gvLog.Rebind()

    End Sub



    Protected Sub ddlAssigned_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ddlFSR.SelectedIndexChanged
        Dim VanList As String = Nothing
        If ddlFSR.SelectedIndex = 0 Then
            For Each itm As RadComboBoxItem In ddlFSR.Items
                VanList = VanList & itm.Value & ","
            Next
        Else
            VanList = Me.ddlFSR.SelectedValue & ","
        End If
        hfSE.Value = VanList
        RebindAllData()


    End Sub


    Private Sub lbVanLink_Click(sender As Object, e As EventArgs) Handles lbVanLink.Click
        Session("DashCurrency") = ddlCountry.SelectedItem.Value
        Session("DashMonth") = StartTime.SelectedDate

        Response.Redirect("DashBoardDetailedSalesbyVan.aspx?MonthYr=" & CDate(StartTime.SelectedDate).ToString("MM/dd/yyyy"))
    End Sub

    Protected Sub lbColLink_Click(sender As Object, e As EventArgs) Handles lbColLink.Click
        Session("DashCurrency") = ddlCountry.SelectedItem.Value
        Session("DashMonth") = StartTime.SelectedDate

        Response.Redirect("DashBoardDetailedCollectionbyVan.aspx?MonthYr=" & CDate(StartTime.SelectedDate).ToString("MM/dd/yyyy"))
    End Sub

    Protected Sub lbAgencySaleLink_Click(sender As Object, e As EventArgs) Handles lbAgencySaleLink.Click
        Session("DashCurrency") = ddlCountry.SelectedItem.Value
        Session("DashMonth") = StartTime.SelectedDate
        Response.Redirect("DashBoardDetailedSalesbyAgency.aspx?MonthYr=" & CDate(StartTime.SelectedDate).ToString("MM/dd/yyyy"))
    End Sub


    Private Sub lblZeroBilled_Click(sender As Object, e As EventArgs) Handles lblZeroBilled.Click
        bindZeroBilled()
    End Sub
    Sub bindZeroBilled()
        Dim dt As New DataTable
        dt = (New SalesWorx.BO.Common.Reports).GetAllZeroBilledCustomers(Err_No, Err_Desc, hfAllVan.Value, hfSMonth.Value)
        rgZeroBilled.DataSource = dt
        rgZeroBilled.DataBind()
        ZeroBilledWindow.VisibleOnPageLoad = True
    End Sub

    Private Sub rgZeroBilled_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles rgZeroBilled.PageIndexChanged
        bindZeroBilled()
    End Sub

    Protected Sub sqlLog_Selecting(sender As Object, e As SqlDataSourceSelectingEventArgs)
        e.Command.CommandTimeout = 600
    End Sub

    Protected Sub gvLog_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs)
        If ViewState("VanLog") Is Nothing Then
            gvLog.DataSource = objDash.LoadDashBoardVanLog(Err_No, Err_Desc, Me.hfSE.Value, Me.hfSMonth.Value)
        Else
            gvLog.DataSource = CType(ViewState("VanLog"), DataTable)
        End If
        CType(gvLog.Columns(1), GridBoundColumn).DataFormatString = "{0:f" & hfDecimal.Value & "}"
        CType(gvLog.Columns(2), GridBoundColumn).DataFormatString = "{0:f" & hfDecimal.Value & "}"
        CType(gvLog.Columns(3), GridBoundColumn).DataFormatString = "{0:f" & hfDecimal.Value & "}"


        CType(gvLog.Columns(1), GridBoundColumn).FooterAggregateFormatString = "{0:f" & hfDecimal.Value & "}"
        CType(gvLog.Columns(2), GridBoundColumn).FooterAggregateFormatString = "{0:f" & hfDecimal.Value & "}"
        CType(gvLog.Columns(3), GridBoundColumn).FooterAggregateFormatString = "{0:f" & hfDecimal.Value & "}"
    End Sub
End Class