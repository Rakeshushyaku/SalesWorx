Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports System.Collections.Generic
Imports Telerik.Web.UI
Imports OfficeOpenXml

Public Class Rep_MerchandasingSurveyResp
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    Dim ObjCustomer As Customer
    Dim objSurvey As Survey
    Private ReportPath As String = "CustomerSurveyDetailsMain"
    Private Const PageID As String = "P384"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))
                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                End If
                If Not Request.QueryString("OrgID") Is Nothing Then
                    ddlOrganization.ClearSelection()
                    If Not ddlOrganization.FindItemByValue(Request.QueryString("OrgID")) Is Nothing Then
                        ddlOrganization.FindItemByValue(Request.QueryString("OrgID")).Selected = True
                    End If
                End If



                LoadOrgDetails()
                LoadSurvey()
                LoadCustomer()

                If Not Request.QueryString("SurveyID") Is Nothing Then
                    ddlSurvey.ClearSelection()
                    If Not ddlSurvey.FindItemByValue(Request.QueryString("SurveyID")) Is Nothing Then
                        ddlSurvey.FindItemByValue(Request.QueryString("SurveyID")).Selected = True
                    End If
                End If
                If Not Request.QueryString("From") Is Nothing Then
                    txtFromDate.SelectedDate = CDate(Request.QueryString("From"))
                Else
                    txtFromDate.SelectedDate = DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now)
                End If
                If Not Request.QueryString("To") Is Nothing Then
                    txtToDate.SelectedDate = CDate(Request.QueryString("To"))
                Else
                    txtToDate.SelectedDate = Now
                End If
                If Not Request.QueryString("Van") Is Nothing Then
                    ddlVan.ClearSelection()
                    If Not ddlVan.FindItemByValue(Request.QueryString("Van")) Is Nothing Then
                        ddlVan.FindItemByValue(Request.QueryString("Van")).Selected = True
                    End If
                End If
                If Not Request.QueryString("CustID") Is Nothing And Not Request.QueryString("SiteID") Is Nothing Then
                    ddlCustomer.ClearSelection()
                    Dim custid As String
                    custid = Request.QueryString("CustID") & "$" & Request.QueryString("SiteID")
                    If Not ddlCustomer.FindItemByValue(custid) Is Nothing Then
                        ddlCustomer.FindItemByValue(custid).Selected = True
                    End If
                End If

                If Not Request.QueryString("OrgID") Is Nothing Then
                    BindReport()
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

                ErrorResource = Nothing
            End Try
        End If
    End Sub
    Private Sub LoadSurvey()
        Try
            Dim SearchQuery As String = ""
            Dim SurveyType As String = ""
            objSurvey = New Survey()
            SearchQuery = " And Survey_Type_Code='S'"
            ddlSurvey.DataSource = objSurvey.GetAllSurvey(Err_No, Err_Desc, SearchQuery)
            ddlSurvey.DataBind()

            ddlSurvey.Items.Insert(0, New RadComboBoxItem("Select Survey", "0"))


        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Sub LoadOrgDetails()
        'Dim objUserAccess As UserAccess
        'objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        'ObjCommon = New SalesWorx.BO.Common.Common()
        'ddlVan.DataSource = ObjCommon.GetVanByOrg(Err_No, Err_Desc, ddlOrganization.SelectedValue, objUserAccess.UserID)
        'ddlVan.DataBind()

        Try

       
        If Not ddlOrganization.SelectedItem Is Nothing Then

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjRep As SalesWorx.BO.Common.Reports = New SalesWorx.BO.Common.Reports()

            ddlVan.Items.Clear()
            ddlVan.DataSource = ObjRep.GetAllOrgVan(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, objUserAccess.UserID.ToString())
            ddlVan.DataBind()
            'ddlVan.Items.Insert(0, New RadComboBoxItem("Select Van", 0))
            For Each itm As RadComboBoxItem In ddlVan.Items
                itm.Checked = True
            Next
        Else
            ddlVan.Items.Clear()
            End If
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try

    End Sub

    Private Sub LoadCustomer()
        Try
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim SearchQuery As String = ""
            objSurvey = New Survey()
            SearchQuery = " AND B.Survey_ID=" & ddlSurvey.SelectedValue & "  AND a.Customer_ID IN (SELECT Customer_ID FROM app_GetOrgCustomerShipAddress(" & ddlOrganization.SelectedItem.Value & "))"
            ddlCustomer.DataSource = ObjReport.GetMerchandisingSurveyCustomer(Err_No, Err_Desc, SearchQuery)

            ddlCustomer.DataBind()
            ddlCustomer.Items.Insert(0, New RadComboBoxItem("Select Customer", "0$0"))

            objSurvey = Nothing
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            Err_No = "74167"
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
        End Try
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Private Sub SearchBtn_Click(sender As Object, e As EventArgs) Handles SearchBtn.Click
        If ValidateInputs() Then
            BindReport()
        Else
            Args.Visible = False
        End If
    End Sub
    Function ValidateInputs() As Boolean
        Try
            Dim bretval As Boolean = False
        If ddlOrganization.SelectedIndex > 0 Then
            If Not ddlSurvey.SelectedIndex > 0 Then
                MessageBoxValidation("Select the Survey", "Validation")
                Return bretval
            End If
            Dim TemFromDateStr As String = CDate(txtFromDate.SelectedDate).ToString("dd/MM/yyyy")
            Dim DateArr As Array = TemFromDateStr.Split("/")
            If DateArr.Length = 3 Then
                TemFromDateStr = DateArr(1) & "/" & DateArr(0) & "/" & DateArr(2)
            End If
            Dim TemToDateStr As String = CDate(txtToDate.SelectedDate).ToString("dd/MM/yyyy")
            Dim DateArr1 As Array = TemToDateStr.Split("/")
            If DateArr1.Length = 3 Then
                TemToDateStr = DateArr1(1) & "/" & DateArr1(0) & "/" & DateArr1(2)
            End If

            If Not IsDate(TemFromDateStr) Then
                MessageBoxValidation("Enter valid ""From date"".", "Validation")
                SetFocus(txtFromDate)
                Return bretval
            End If

            If Not IsDate(TemToDateStr) Then
                MessageBoxValidation("Enter valid ""To date"".", "Validation")
                SetFocus(TemToDateStr)
                Return bretval
            End If

            If CDate(TemFromDateStr) > CDate(TemToDateStr) Then
                MessageBoxValidation("Start Date should not be greater than End Date.", "Validation")
                Return bretval
            End If
            bretval = True
            Return bretval
        Else
            MessageBoxValidation("Please select the Organisation", "Validation")
            Return bretval
            End If
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Function
    Protected Sub ViewDetails_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim btnEdit As LinkButton = TryCast(sender, LinkButton)
            Dim row As Telerik.Web.UI.GridDataItem = DirectCast(btnEdit.NamingContainer, Telerik.Web.UI.GridDataItem)
            Dim sid As String = CType(row.FindControl("ID"), HiddenField).Value

            Dim url As String = "Rep_MerchandasingSurveyResult.aspx?SurveySession=" & sid

            Dim Van As String = "0"
            If Not ddlVan.SelectedItem Is Nothing Then
                url = url & "&Van=" & ddlVan.SelectedItem.Value
            End If
            If Not ddlSurvey.SelectedItem Is Nothing Then
                url = url & "&SurveyID=" & ddlSurvey.SelectedItem.Value
            End If
            If ddlCustomer.SelectedItem.Value = "0$0" Then

            Else
                Dim ids() As String
                ids = ddlCustomer.SelectedItem.Value.Split("$")
                url = url & "&CustID=" & ids(0)
                url = url & "&SiteID=" & ids(1)
            End If
            url = url & "&From=" & txtFromDate.SelectedDate
            url = url & "&To=" & txtToDate.SelectedDate

            Response.Redirect(url)
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Sub BindReport()



        Dim ObjReport As New SalesWorx.BO.Common.Reports
        Try
            rpbFilter.Items(0).Expanded = False

            lbl_org.Text = ddlOrganization.SelectedItem.Text
            lbl_From.Text = CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy")
            lbl_to.Text = CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy")

            Args.Visible = True

            Dim dt As New DataTable
            Dim ids() As String
            ids = ddlCustomer.SelectedItem.Value.Split("$")

            'Dim Van As String = "0"
            'If ddlVan.SelectedItem Is Nothing Then
            '    Van = "0"
            '    lbl_Van.Text = "All"
            'Else
            '    Van = ddlVan.SelectedItem.Value
            '    lbl_Van.Text = ddlVan.SelectedItem.Text
            'End If

            Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems

            Dim van As String = ""
            For Each li As RadComboBoxItem In collection
                van = van & li.Value & ","
            Next

            If van = "" Then
                van = "0"
            End If



            If ddlCustomer.SelectedItem.Value = "0$0" Then
                lbl_Customer.Text = "All"
            Else
                lbl_Customer.Text = ddlCustomer.SelectedItem.Text
            End If
            dt = ObjReport.GetMerchandasingSessions(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, van, ddlSurvey.SelectedItem.Value, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"), ids(0), ids(1))
            gvRep.DataSource = dt
            gvRep.DataBind()

            ObjReport = Nothing
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            ObjReport = Nothing
        End Try
    End Sub

    Private Sub gvRep_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        BindReport()
    End Sub

    Private Sub ddlOrganization_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlOrganization.SelectedIndexChanged
        LoadOrgDetails()
    End Sub

    Private Sub ddlSurvey_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlSurvey.SelectedIndexChanged
        LoadCustomer()
    End Sub

    Private Sub BtnExportExcel_Click(sender As Object, e As EventArgs) Handles BtnExportExcel.Click
        Try

            If ValidateInputs() Then
                Dim tblData As New DataTable
                Dim ObjReport As New SalesWorx.BO.Common.Reports
                Dim dt As New DataTable



                Dim ids() As String
                ids = ddlCustomer.SelectedItem.Value.Split("$")

                'Dim Van As String = "0"
                'If ddlVan.SelectedItem Is Nothing Then
                '    Van = "0"
                '    lbl_Van.Text = "All"
                'Else
                '    Van = ddlVan.SelectedItem.Value
                '    lbl_Van.Text = ddlVan.SelectedItem.Text
                'End If

                Dim collection As IList(Of RadComboBoxItem) = ddlVan.CheckedItems

                Dim van As String = ""
                For Each li As RadComboBoxItem In collection
                    van = van & li.Value & ","
                Next

                If van = "" Then
                    van = "0"
                End If

                If ddlCustomer.SelectedItem.Value = "0$0" Then
                    lbl_Customer.Text = "All"
                Else
                    lbl_Customer.Text = ddlCustomer.SelectedItem.Text
                End If
                'dt = ObjReport.GetMerchandasingSessions(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, Van, ddlSurvey.SelectedItem.Value, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"), ids(0), ids(1))
                dt = ObjReport.ExportToExcelMerchandasingResult_Blk(Err_No, Err_Desc, ddlOrganization.SelectedValue, ddlSurvey.SelectedValue, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), CDate(txtToDate.SelectedDate).ToString("dd-MMM-yyyy"), van, ids(0), ids(1))




                tblData = dt.DefaultView.ToTable(False, "Survey", "VAN", "Survey_At", "Customer_No", "Customer_Name", "Batch1", "group_text", "Question", "Response1", "Comment")


                For Each col In tblData.Columns
                    If col.ColumnName = "Survey_At" Then
                        col.ColumnName = "Surveyed At"
                    End If
                    If col.ColumnName = "Customer_No" Then
                        col.ColumnName = "Customer No"
                    End If
                    If col.ColumnName = "Customer_Name" Then
                        col.ColumnName = "Customer Name"
                    End If
                    If col.ColumnName = "Batch" Then
                        col.ColumnName = "Agency/Brand"
                    End If
                    If col.ColumnName = "group_text" Then
                        col.ColumnName = "Group Text"
                    End If
                    If col.ColumnName = "Response1" Then
                        col.ColumnName = "Response"
                    End If
                    If col.ColumnName = "Batch1" Then
                        col.ColumnName = "Batch"
                    End If
                Next


                If tblData.Rows.Count > 0 Then

                    Using package As New ExcelPackage()
                        ' add a new worksheet to the empty workbook
                        Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
                        Worksheet.Cells("A1").LoadFromDataTable(tblData, True)
                        Worksheet.Column(3).Style.Numberformat.Format = "dd-MMM-yyyy"

                        Dim str As String
                        For i = 1 To (tblData.Rows.Count + 1)

                            str = Worksheet.Cells.Item("I" & i).Value
                            If str.ToUpper().Contains("HTTP") Then
                                Worksheet.Cells.Item("I" & i).Style.Font.UnderLine = True
                                Worksheet.Cells.Item("I" & i).Style.Font.Color.SetColor(System.Drawing.Color.Blue)
                                Worksheet.Cells(i, 9).Formula = "HYPERLINK(""" & str & """,""" & str & """)"
                            End If

                        Next

                        '  Worksheet.Column(12).Style.Numberformat.Format = "dd-MMM-yyyy"
                        Worksheet.Cells.AutoFitColumns()

                        Response.Clear()
                        Response.Buffer = True
                        Response.Charset = ""

                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                        Response.AddHeader("content-disposition", "attachment;filename= MerchandasingReports.xlsx")

                        Using MyMemoryStream As New MemoryStream()
                            package.SaveAs(MyMemoryStream)
                            MyMemoryStream.WriteTo(Response.OutputStream)
                            Response.AddHeader("Content-Length", MyMemoryStream.Length)
                            Response.Flush()
                            Response.Close()
                        End Using
                    End Using
                Else
                    MessageBoxValidation("No Data.", "Validation")
                End If
            End If
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub
End Class