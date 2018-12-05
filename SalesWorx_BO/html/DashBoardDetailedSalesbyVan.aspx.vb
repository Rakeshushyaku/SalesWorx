Imports SalesWorx.BO.Common
Imports Telerik.Web.UI
Imports log4net
Public Class DashBoardDetailedSalesbyVan
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const PageID As String = "P87"
    Dim ObjCommon As New SalesWorx.BO.Common.Common
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
            Exit Sub
        End If
        If Not IsPostBack Then
            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            If Not HasPermission Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If

            Try

               
                
                If Not Request.QueryString("MonthYr") Is Nothing Then
                    txtFromDate.SelectedDate = CDate(Request.QueryString("MonthYr")).Date
                    HDate.Value = CDate(txtFromDate.SelectedDate).ToString("MM-yyyy")
                End If

               

                ObjCommon = New SalesWorx.BO.Common.Common()
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)

                Dim CountryTbl As DataTable = Nothing
                Dim orgTbl As DataTable = Nothing

                CountryTbl = ObjCommon.GetCountry(Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID)
                ddlCountry.DataSource = CountryTbl
                ddlCountry.DataBind()

                If Not Session("DashCurrency") Is Nothing Then
                    If Not ddlCountry.Items.FindItemByValue(Session("DashCurrency")) Is Nothing Then
                        ddlCountry.Items.FindItemByValue(Session("DashCurrency")).Selected = True
                    End If
                End If

                orgTbl = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                Dim s() As String = Nothing
                Dim Currency As String = Nothing
                Dim DecimalDigits As String = "N2"
                Dim country As String = Nothing
                If CountryTbl.Rows.Count = 1 Then


                    dvCountry.Visible = False

                    s = ddlCountry.SelectedValue.Split("$")

                    If s.Length > 0 Then
                        country = s(0).ToString()
                        Currency = s(1).ToString()
                        DecimalDigits = s(2).ToString()
                    End If

                    Me.hfCurrency.Value = Currency
                    Me.hfDecimal.Value = "N" & DecimalDigits
                    ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
                    ddlOrganization.DataBind()
                    Dim OrgStr As String = Nothing
                    For Each item As RadComboBoxItem In ddlOrganization.Items
                        item.Checked = True
                        If item.Checked Then

                            OrgStr = OrgStr & "," & item.Value

                        End If
                    Next

                ElseIf CountryTbl.Rows.Count > 1 Then

                    dvCountry.Visible = True


                    s = ddlCountry.SelectedValue.Split("$")

                    If s.Length > 0 Then
                        country = s(0).ToString()
                        Currency = s(1).ToString()
                        DecimalDigits = s(2).ToString()
                    End If
                    Me.hfCurrency.Value = Currency
                    Me.hfDecimal.Value = "N" & DecimalDigits
                    ddlOrganization.DataSource = orgTbl.Select("Country='" & country & "'").CopyToDataTable()
                    ddlOrganization.DataBind()
                    Dim OrgStr As String = Nothing
                    For Each item As RadComboBoxItem In ddlOrganization.Items
                        item.Checked = True
                        If item.Checked Then

                            OrgStr = OrgStr & "," & item.Value

                        End If
                    Next
                End If

               

                UId.Value = CType(Session("User_Access"), UserAccess).UserID
                Dim Orgtxt As String = ""
                HUID.Value = ""
                For Each item As RadComboBoxItem In ddlOrganization.CheckedItems
                    HUID.Value = HUID.Value & item.Value & ","
                    Orgtxt = Orgtxt & item.Text & ","
                Next
                If HUID.Value.Trim.EndsWith(",") Then
                    HUID.Value = HUID.Value.Trim
                    HUID.Value = HUID.Value.Substring(0, HUID.Value.Length - 1)
                End If

                If Orgtxt <> "" Then
                    Orgtxt = Orgtxt.Substring(0, Orgtxt.Length - 1)
                End If



                HCurrency.Value = "(" & Me.hfCurrency.Value & ")"

                lbl_org.Text = Orgtxt
                lbl_Month.Text = CDate(txtFromDate.SelectedDate).ToString("MMM-yyyy")

                Args.Visible = True

                Dim dt As New DataTable
                dt = (New SalesWorx.BO.Common.Reports).GetSalesbyVanForMonth(Err_No, Err_Desc, HUID.Value, CDate(txtFromDate.SelectedDate).ToString("MM-yyyy"))

                'If dt.Rows.Count > 8 And dt.Rows.Count < 14 Then
                '    Chartwrapper.Style.Add("height", (dt.Rows.Count * 40).ToString & "px")
                'ElseIf dt.Rows.Count > 14 Then
                '    Chartwrapper.Style.Add("height", (dt.Rows.Count * 35).ToString & "px")
                'Else
                '    Chartwrapper.Style.Add("height", "400px")
                'End If


                If dt.Rows.Count > 8 And dt.Rows.Count < 14 Then
                    Chartwrapper.Style.Add("width", (dt.Rows.Count * 40).ToString & "px")
                ElseIf dt.Rows.Count > 14 Then
                    Chartwrapper.Style.Add("width", (dt.Rows.Count * 35).ToString & "px")
                Else
                    Chartwrapper.Style.Add("width", "1000px")
                End If


                Dim StrSummary As String = ""
                Dim sumCash = dt.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("Amount")))

                StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>Net Sales " & HCurrency.Value & "<div class='text-primary'>" & Format(sumCash, hfDecimal.Value) & "</div></div></div>"

                summary.InnerHtml = StrSummary
                rpbFilter.Items(0).Expanded = False
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

            End Try
        End If


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
        Me.hfCurrency.Value = Currency
        Me.hfDecimal.Value = "N" & DecimalDigits
        HCurrency.Value = "(" & Me.hfCurrency.Value & ")"

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
    Private Sub BindChart()
        Dim Orgtxt As String = ""
        HUID.Value = ""
        For Each item As RadComboBoxItem In ddlOrganization.CheckedItems
            HUID.Value = HUID.Value & item.Value & ","
            Orgtxt = Orgtxt & item.Text & ","
        Next
        If HUID.Value.Trim.EndsWith(",") Then
            HUID.Value = HUID.Value.Trim
            HUID.Value = HUID.Value.Substring(0, HUID.Value.Length - 1)
        End If

        If Orgtxt <> "" Then
            Orgtxt = Orgtxt.Substring(0, Orgtxt.Length - 1)
        End If

        lbl_org.Text = Orgtxt
        lbl_Month.Text = CDate(txtFromDate.SelectedDate).ToString("MMM-yyyy")
        Args.Visible = True
        Dim dt As New DataTable
        dt = (New SalesWorx.BO.Common.Reports).GetSalesbyVanForMonth(Err_No, Err_Desc, HUID.Value, CDate(txtFromDate.SelectedDate).ToString("MM-yyyy"))

        Dim StrSummary As String = ""
        Dim sumCash = dt.AsEnumerable().Sum((Function(x) x.Field(Of Decimal)("Amount")))

        StrSummary = StrSummary & "<div class='col-sm-4'><div class='widgetblk'>Net Sales " & HCurrency.Value & "<div class='text-primary'>" & Format(sumCash, hfDecimal.Value) & "</div></div></div>"

        summary.InnerHtml = StrSummary
        'If dt.Rows.Count > 8 And dt.Rows.Count < 14 Then
        '    Chartwrapper.Style.Add("height", (dt.Rows.Count * 40).ToString & "px")
        'ElseIf dt.Rows.Count > 14 Then
        '    Chartwrapper.Style.Add("height", (dt.Rows.Count * 35).ToString & "px")
        'Else
        '    Chartwrapper.Style.Add("height", "400px")
        'End If

        If dt.Rows.Count > 8 And dt.Rows.Count < 14 Then
            Chartwrapper.Style.Add("width", (dt.Rows.Count * 40).ToString & "px")
        ElseIf dt.Rows.Count > 14 Then
            Chartwrapper.Style.Add("width", (dt.Rows.Count * 35).ToString & "px")
        Else
            Chartwrapper.Style.Add("width", "1000px")
        End If

        ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
    End Sub

    

    Private Sub ddlCountry_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlCountry.SelectedIndexChanged
        LoadOrgs()
        HDate.Value = CDate(txtFromDate.SelectedDate).ToString("MM-yyyy")
        BindChart()
    End Sub

    Private Sub SearchBtn_Click(sender As Object, e As EventArgs) Handles SearchBtn.Click
        If ValidateInput() Then
            HDate.Value = CDate(txtFromDate.SelectedDate).ToString("MM-yyyy")
            BindChart()
        Else
            summary.InnerHtml = ""
            Args.Visible = False
        End If
        
    End Sub
    Function ValidateInput() As Boolean
        Dim bretval As Boolean = False
        If ddlOrganization.CheckedItems Is Nothing Or ddlOrganization.CheckedItems.Count = 0 Then
            MessageBoxValidation("Please Select the Organisation", "Validation")
            Return bretval
        End If
        bretval = True
        Return bretval
    End Function
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
End Class