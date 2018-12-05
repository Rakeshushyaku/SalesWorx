Imports SalesWorx.BO.Common
Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports Telerik.Web.UI
Imports System.Linq
Imports OfficeOpenXml
Imports System.Drawing
Partial Public Class Rep_ExportSummaryReport
    Inherits System.Web.UI.Page
    Private Const PageID As String = "P291"
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Not IsPostBack() Then
           
    End If
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

                txtFromDate.Text = Now.ToString("dd-MMM-yyyy")
            txtToDate.Text = Now.ToString("dd-MMM-yyyy")


            End If
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_MSL_005") & "&next=Welcome.aspx&Title=Product List Setup", False)
        End Try
    End Sub

    Private Sub btnSearch0_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch0.Click
        If Not IsDate(txtFromDate.Text) Then
            MessageBoxValidation("Please enter a valid from date")
            Exit Sub
        End If
        If Not IsDate(txtToDate.Text) Then
            MessageBoxValidation("Please enter a valid to date")
            Exit Sub
        End If

        Dim exportdt As New DataSet
        exportdt = (New SalesWorx.BO.Common.Common).GetSummaryforExport(err_no, Err_Desc, txtFromDate.Text, txtToDate.Text)

        ExportToExcel("SUMMARYRPEORT" & Now.ToString("ddMMYYYhhmm") & ".xls", exportdt)
    End Sub
    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        MpInfoError.Show()
        Exit Sub
    End Sub

  Public Function RunSample1(ByVal outputDir As DirectoryInfo, ByVal dt As DataTable, ByVal genday As Date) As String
        Dim newFile As New FileInfo(Convert.ToString(outputDir.FullName) & "\CMPGN_RESP_EML.xlsx")
        If newFile.Exists Then
            newFile.Delete()
            ' ensures we create a new workbook
            newFile = New FileInfo(Convert.ToString(outputDir.FullName) & "\CMPGN_RESP_EML.xlsx")
        End If
        Using package As New ExcelPackage(newFile)
            ' add a new worksheet to the empty workbook
            Dim worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("EMAIL")
            worksheet.Cells("A1").LoadFromDataTable(dt, True)

            package.Save()

        End Using

        Return newFile.FullName
    End Function

     Private Sub ExportToExcel(ByVal strFileName As String, ByVal ds As DataSet)
        Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog"
        strFileName = RunSample1(New DirectoryInfo(fn), ds)
        Dim file As New FileInfo(strFileName)
        Response.Clear()
        Response.ClearHeaders()
        Response.ClearContent()
        Response.AddHeader("content-disposition", "attachment; filename=" + strFileName)
        Response.AddHeader("Content-Type", "application/Excel")
        Response.ContentType = "application/vnd.xls"
        Response.AddHeader("Content-Length", File.Length.ToString())
        Response.WriteFile(File.FullName)
        Response.End()
    End Sub
   Public Function RunSample1(ByVal outputDir As DirectoryInfo, ByVal ds As DataSet) As String

        Dim filename As String
        filename = Convert.ToString(outputDir.FullName) & "\SUMMARYRPEORT" & Now.ToString("ddMMyyyyhhmm") & ".xlsx"
        Dim newFile As New FileInfo(filename)

        If newFile.Exists Then
            newFile.Delete()
            ' ensures we create a new workbook
            newFile = New FileInfo(filename)
        End If
        Using package As New ExcelPackage(newFile)
            ' add a new worksheet to the empty workbook
            Dim WorksheetCUSTOMER As ExcelWorksheet = package.Workbook.Worksheets.Add("CUSTOMER")
            WorksheetCUSTOMER.Cells("A1").LoadFromDataTable(ds.Tables(0), True)

            Dim WorksheetPRODUCTS As ExcelWorksheet = package.Workbook.Worksheets.Add("PRODUCTS")
            WorksheetPRODUCTS.Cells("A1").LoadFromDataTable(ds.Tables(1), True)

            Dim WorksheetSALESREPS As ExcelWorksheet = package.Workbook.Worksheets.Add("SALESREPS")
            WorksheetSALESREPS.Cells("A1").LoadFromDataTable(ds.Tables(2), True)

            Dim WorksheetROUTEPALN As ExcelWorksheet = package.Workbook.Worksheets.Add("ROUTEPALN")
            WorksheetROUTEPALN.Cells("A1").LoadFromDataTable(ds.Tables(3), True)
            WorksheetROUTEPALN.Column(4).Style.Numberformat.Format = "dd-MMM-yyyy"

            Dim WorksheetINVOICES As ExcelWorksheet = package.Workbook.Worksheets.Add("INVOICES")
            WorksheetINVOICES.Cells("A1").LoadFromDataTable(ds.Tables(4), True)
            WorksheetINVOICES.Column(3).Style.Numberformat.Format = "dd-MMM-yyyy"
            WorksheetINVOICES.Column(5).Style.Numberformat.Format = "dd-MMM-yyyy"
            WorksheetINVOICES.Column(13).Style.Numberformat.Format = "dd-MMM-yyyy"
            WorksheetINVOICES.Column(14).Style.Numberformat.Format = "dd-MMM-yyyy"
            WorksheetINVOICES.Column(16).Style.Numberformat.Format = "dd-MMM-yyyy hh:mm:ss tt"
            WorksheetINVOICES.Column(17).Style.Numberformat.Format = "dd-MMM-yyyy hh:mm:ss tt"

            Dim WorksheetINVOICE_ITEMS As ExcelWorksheet = package.Workbook.Worksheets.Add("INVOICE_ITEMS")
            WorksheetINVOICE_ITEMS.Cells("A1").LoadFromDataTable(ds.Tables(5), True)

            package.Save()

        End Using

        Return newFile.FullName
    End Function
End Class