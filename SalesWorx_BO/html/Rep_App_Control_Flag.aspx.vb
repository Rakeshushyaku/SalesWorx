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
Imports OfficeOpenXml

Public Class Rep_App_Control_Flag
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As New SalesWorx.BO.Common.Common
    Dim ObjCollection As Collection
    Private Const PageID As String = "P351"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ErrorResource = New ResourceManager("SWX_ALP.ErrorResource", System.Reflection.Assembly.GetExecutingAssembly())
            Try
                ObjCommon = New SalesWorx.BO.Common.Common()
                Dim HasPermission As Boolean = False
                ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
                If Not HasPermission Then
                    Err_No = 500
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
                End If


                If Not Err_Desc Is Nothing Then
                    log.Error(Err_Desc)
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_001") & "&next=Welcome.aspx&Title=Messages", False)
                End If
                BindReport()

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

    Private Sub BindReport()
        Try

            lbl_Code.Text = IIf(String.IsNullOrEmpty(txt_Control_Key.Text), "All", txt_Control_Key.Text.Trim())
            Args.Visible = True

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetAppControlFlag(Err_No, Err_Desc, txt_Control_Key.Text)
            gvRep.DataSource = dt
            gvRep.DataBind()

        Catch Ex As Exception
            log.Error(GetExceptionInfo(Ex))
            log.Error(Ex.Message.ToString())
        End Try
    End Sub


    Private Sub dgv_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles gvRep.SortCommand
        Try

            ViewState("SortField") = e.SortExpression
            SortDirection = "flip"
            BindReport()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Private Sub dgv_PageIndexChanged(ByVal source As Object, ByVal e As GridPageChangedEventArgs) Handles gvRep.PageIndexChanged
        Try
            BindReport()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
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
    Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click
        Try
            If ValidateInputs() Then
                gvRep.Visible = True

                BindReport()
            Else
                gvRep.Visible = False
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub


    
    Function ValidateInputs() As Boolean
        Dim bretval As Boolean = True
        Return bretval
    End Function

  

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        txt_Control_Key.Text = ""

        Try
            If ValidateInputs() Then
                gvRep.Visible = True

                BindReport()
            Else
                gvRep.Visible = False
            End If
        Catch ex As Exception

        End Try
        ' Args.Visible = False
        '  gvRep.Visible = False
    End Sub
    Private Sub BtnExportBiffExcel_Click(sender As Object, e As EventArgs) Handles BtnExportBiffExcel.Click
        Try


            Dim tblData As New DataTable

            Dim ObjReport As New SalesWorx.BO.Common.Reports
            Dim dt As New DataTable
            dt = ObjReport.GetAppControlFlag(Err_No, Err_Desc, txt_Control_Key.Text)
            tblData = dt.DefaultView.ToTable(False, "Control_Key", "Control_Value", "Custom_Attribute_3")


            For Each col In tblData.Columns
                If col.ColumnName = "Control_Key" Then
                    col.ColumnName = "Key"
                End If

                If col.ColumnName = "Control_Value" Then
                    col.ColumnName = "Value"
                End If
                If col.ColumnName = "Custom_Attribute_3" Then
                    col.ColumnName = "Description"
                End If
               

            Next





            If tblData.Rows.Count > 0 Then

                Using package As New ExcelPackage()
                    ' add a new worksheet to the empty workbook
                    Dim Worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add("Sheet1")
                    Worksheet.Cells("A1").LoadFromDataTable(tblData, True)
                    Worksheet.Cells.AutoFitColumns()
                    Response.Clear()
                    Response.Buffer = True
                    Response.Charset = ""

                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    Response.AddHeader("content-disposition", "attachment;filename= App_Control_flag.xlsx")

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
End Class
