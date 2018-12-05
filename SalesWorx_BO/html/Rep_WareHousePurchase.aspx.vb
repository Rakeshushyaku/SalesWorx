Imports OfficeOpenXml
Imports System.Data
Imports System.IO
Imports Ionic.Zip
Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports log4net
Imports Microsoft.Reporting.WebForms
Imports System.Globalization
Imports System.Drawing
Imports Telerik.Web.UI
Partial Public Class Rep_WareHousePurchase
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As Common
    Dim ObjPrice As Price
    Private ReportPath As String = "WareHousePurchase_Hdr"
    Private Const PageID As String = "P374"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
     Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If

        If Not IsPostBack Then
            txtFromDate.SelectedDate = Now
            Dim HasPermission As Boolean = False


            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            If Not HasPermission Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If
            ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
            Try

                ObjCommon = New Common()


                Dim SubQry As String = ObjCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)


                ddlOrganization.DataSource = ObjCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddlOrganization.DataBind()
                ddlOrganization.Items.Insert(0, New RadComboBoxItem("Select Organization", "0"))

                If ddlOrganization.Items.Count = 2 Then
                    ddlOrganization.SelectedIndex = 1
                End If
                LoadAgency()
                ' BindData()
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
Sub LoadAgency()
        ddlAgency.DataSource = (New SalesWorx.BO.Common.Stock).GetAgencyList(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, txtFromDate.SelectedDate)
        ddlAgency.DataTextField = "Agency"
        ddlAgency.DataValueField = "Agency"
        ddlAgency.DataBind()
        ddlAgency.Items.Insert(0, New RadComboBoxItem("Select Agency", "0"))
        ddlAgency.Items(0).Value = "0"
    End Sub
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
     Dim zipfilename As String = ""


        If ddlOrganization.SelectedItem.Value = "0" Then
            MessageBoxValidation("Please select the organization", "Validation")
            Exit Sub
        End If
        If ddlAgency.SelectedItem.Value = "0" Then
            MessageBoxValidation("Please select the agency", "Validation")
            Exit Sub
        End If
        If txtFromDate.SelectedDate Is Nothing Then
            MessageBoxValidation("Please enter a date", "Validation")
            Exit Sub
        End If
         Try
        Dim i As Integer = 1
        Dim zip As New ZipFile
        Dim dtsp As New DataTable
            dtsp = (New SalesWorx.BO.Common.Stock).GetSalesRepforPurchaseReport(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, ddlAgency.SelectedItem.Value, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"))
        If dtsp.Rows.Count > 0 Then
            For Each spdr As DataRow In dtsp.Rows
                Dim dt As New DataTable
                    dt = (New SalesWorx.BO.Common.Stock).GetPurchaseReport(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value, ddlAgency.SelectedItem.Value, CDate(txtFromDate.SelectedDate).ToString("dd-MMM-yyyy"), Val(spdr("SalesRep_ID").ToString))
                If dt.Rows.Count > 0 Then
                    Dim outfile As String = ""
                    Dim bfilecreated As Boolean
                    bfilecreated = RunSample1(dt, i, spdr("Emp_Code"), outfile)
                    If bfilecreated = True Then
                        zip.AddFile(outfile)
                        'Dim OutFileExcel As New FileInfo(outfile)
                        'OutFileExcel.Delete()
                    End If
                End If
                i = i + 1
            Next
        Else
                MessageBoxValidation("Confirmed Stock requisitions do not exist for this agency on the selected date.", "Information")
             Exit Sub
        End If

            Dim spath As String = ""
            spath = ConfigurationManager.AppSettings("ExcelPath")

            Dim agency() As String
            agency = ddlAgency.SelectedItem.Value.Split(" ")
            Dim agencystr As String = ""
            If agency.Length > 0 Then
                agencystr = agency(0)
            End If

             If agencystr.Trim <> "" Then
                zipfilename = spath & ddlAgency.SelectedItem.Value.Replace(" ", "_") & CDate(txtFromDate.SelectedDate).ToString("dd_MMM_yyyy") & "_" & Now.ToString("hhmm") & ".zip"
            Else
                zipfilename = spath & "Purchase_" & CDate(txtFromDate.SelectedDate).ToString("dd_MMM_yyyy") & "_" & Now.ToString("hhmm") & ".zip"
            End If

            Dim newFile As New FileInfo(zipfilename)
            If newFile.Exists Then
                newFile.Delete()
                ' ensures we create a new workbook
                newFile = New FileInfo(zipfilename)
            End If
            zip.Save(zipfilename)

        Catch ex As Exception
            log.Debug(ex.ToString)
        End Try
        If zipfilename <> "" Then
            Dim File As New FileInfo(zipfilename)
                        Response.ClearContent()
                        Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", File.Name))
                        Response.AddHeader("Content-Length", File.Length.ToString())
                        Response.ContentType = "application/zip"

                        Response.TransmitFile(File.FullName)
                        Response.Flush()
                        'File.Delete()
                        Response.End()
        Else
            MessageBoxValidation("Unexpcted error occurred.", "Information")
        End If
    End Sub
    Private Function RunSample1(ByVal ds As DataTable, ByVal ID As String, ByVal SalesRep_name As String, ByRef Filename As String) As Boolean
        Dim bRetval As Boolean = False
        Try
            Dim spath As String = ""
            spath = ConfigurationManager.AppSettings("ExcelPath")
            Dim agency() As String
            agency = ddlAgency.SelectedItem.Value.Split(" ")
            Dim agencystr As String = ""
            If agency.Length > 0 Then
                agencystr = agency(0)
            End If
            Dim sfilename As String
            If agencystr.Trim <> "" Then
                sfilename = spath & agencystr.Trim & "_" & SalesRep_name.Replace(" ", "_") & "_" & CDate(txtFromDate.SelectedDate).ToString("dd_MMM_yyyy") & "_" & Now.ToString("hhmm") & "_" & ID & ".xlsx"
            Else
                sfilename = spath & "Purchase_" & SalesRep_name.Replace(" ", "_") & "_" & CDate(txtFromDate.SelectedDate).ToString("dd_MMM_yyyy") & "_" & Now.ToString("dd_MMM_yyyy_hhmm") & "_" & ID & ".xlsx"
            End If

            Dim newFile As New FileInfo(sfilename)
            If newFile.Exists Then
                newFile.Delete()
                ' ensures we create a new workbook
                newFile = New FileInfo(Filename)
            End If
            Using package As New ExcelPackage(newFile)
                ' add a new worksheet to the empty workbook
                Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
                Worksheet.Cells("A1").LoadFromDataTable(ds, True)
                package.Save()

            End Using
            Filename = sfilename
            bRetval = True
        Catch ex As Exception

        End Try
        Return bRetval
    End Function
      Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 100, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Private Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrganization.SelectedIndexChanged
          LoadAgency()
    End Sub
End Class