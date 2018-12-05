Imports log4net
Imports SalesWorx.BO.Common
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports Telerik.Web.UI
Imports System.Data.SqlClient
Imports System.Drawing.Printing
Imports System.Web.UI.WebControls
Imports System.Drawing
Imports System.Data
Imports System.Xml
Imports ExcelLibrary.SpreadSheet
Imports System.IO
Partial Public Class Rep_ExportRightsbyUsers
    Inherits System.Web.UI.Page

    Private Const ModuleName As String = "Rep_ExportRightsbyUsers.aspx"
    Private Const PageID As String = "P314"
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objUser As New User
    Dim objLogin As New SalesWorx.BO.Common.Login
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblmsg.Text = ""
        If Not IsPostBack Then
            If Not Session.Item("USER_ACCESS") Is Nothing Then
                If Not HasAuthentication() Then
                    Err_No = 500
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
                End If
                FillUserTypes()
                LoadRights(0)
                TreeViewRights.ExpandAllItems()
            Else
                Session.Add("BringmeBackHere", ModuleName)
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
        End If
    End Sub
    Private Function HasAuthentication() As Boolean
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
    End Function

    Sub FillUserTypes()
        drpUser.DataTextField = "Username"
        drpUser.DataValueField = "User_ID"
        drpUser.DataSource = objUser.GetUsers()
        drpUser.DataBind()
    End Sub
    Sub LoadRights(ByVal UserId As Integer)

        Try

            Dim ds As New DataSet()
            ds.ReadXml(New XmlNodeReader(XmlDataSource1.GetXmlDocument()))

            Dim dt As New DataTable
            dt.Columns.Add("ID")
            dt.Columns.Add("PID")
            dt.Columns.Add("value")
            dt.Columns.Add("type")
            dt.Columns.Add("Isassigned")
            dt.Columns.Add("Name")
            dt.Columns.Add("Designation")
            dt.Columns.Add("UserType")

            Dim OutDt As New DataTable
            OutDt = objUser.GetAllUsers

            Dim desDt As New DataTable
            desDt = objUser.GetIsSS()

            If UserId = 0 Then
                For Each userdr As DataRow In OutDt.Rows

                    Dim destDr() As DataRow
                    destDr = desDt.Select("id='" & userdr("Is_SS") & "'")

                    Dim dr As DataRow
                    dr = dt.NewRow
                    dr(0) = userdr("User_ID").ToString
                    dr(1) = DBNull.Value
                    dr(2) = ""
                    dr(3) = "U"
                    dr(4) = "0"
                    dr(5) = userdr("Username")
                    If destDr.Length > 0 Then
                    dr(6) = destDr(0)("val")
                    End If
                    dr(7) = userdr("User_Type")
                    dt.Rows.Add(dr)

                    Dim dtRights As New DataTable
                    objUser.UserTypeID = userdr("User_Type_ID").ToString
                    dtRights = objUser.GetUserRights()

                    For Each menudr As DataRow In ds.Tables(0).Rows
                        Dim mdr As DataRow

                        Dim rightdr() As DataRow
                        rightdr = dtRights.Select("Menu_Id='" & menudr("id") & "'")
                        If rightdr.Length > 0 Then
                              mdr = dt.NewRow
                              mdr(0) = userdr("User_ID").ToString & menudr("id")
                              mdr(1) = userdr("User_ID")
                              mdr(2) = menudr("value")
                              mdr(3) = "M"
                              mdr(4) = "1"
                              dt.Rows.Add(mdr)

                        End If


                        Dim pgdr() As DataRow
                        pgdr = ds.Tables(1).Select("mainmenu_id=" & menudr("mainmenu_id"))
                         For Each pagedr As DataRow In pgdr
                            Dim pdr As DataRow

                            Dim prightdr() As DataRow
                            prightdr = dtRights.Select("Page_Id='" & pagedr("id") & "'")
                            If prightdr.Length > 0 Then
                                  pdr = dt.NewRow
                                  pdr(0) = pagedr("id")
                                  pdr(1) = userdr("User_ID").ToString & menudr("id")
                                  pdr(2) = pagedr("value")
                                  pdr(3) = "P"
                                  pdr(4) = "1"
                                  dt.Rows.Add(pdr)


                            End If


                         Next
                         Next
                Next
            Else

                Dim seldr() As DataRow
                seldr = OutDt.Select("User_ID=" & UserId)
                If seldr.Count > 0 Then

                    Dim destDr() As DataRow
                    destDr = desDt.Select("id='" & seldr(0)("Is_SS") & "'")

                    Dim dr As DataRow
                    dr = dt.NewRow
                    dr(0) = seldr(0)("User_ID").ToString
                    dr(1) = DBNull.Value
                    dr(2) = ""
                    dr(3) = "U"
                    dr(4) = "0"
                    dr(5) = seldr(0)("Username")
                    If destDr.Length > 0 Then
                    dr(6) = destDr(0)("val")
                    End If
                    dr(7) = seldr(0)("User_Type")
                    dt.Rows.Add(dr)

                    Dim dtRights As New DataTable
                    objUser.UserTypeID = seldr(0)("User_Type_ID").ToString
                    dtRights = objUser.GetUserRights()

                    For Each menudr As DataRow In ds.Tables(0).Rows
                        Dim mdr As DataRow

                        Dim rightdr() As DataRow
                        rightdr = dtRights.Select("Menu_Id='" & menudr("id") & "'")
                        If rightdr.Length > 0 Then
                            mdr = dt.NewRow
                            mdr(0) = menudr("id")
                            mdr(1) = seldr(0)("User_ID")
                            mdr(2) = menudr("value")
                            mdr(3) = "M"
                            mdr(4) = "1"
                            dt.Rows.Add(mdr)
                        End If

                        Dim pgdr() As DataRow
                        pgdr = ds.Tables(1).Select("mainmenu_id=" & menudr("mainmenu_id"))
                         For Each pagedr As DataRow In pgdr
                            Dim pdr As DataRow

                            Dim prightdr() As DataRow
                            prightdr = dtRights.Select("Page_Id='" & pagedr("id") & "'")
                            If prightdr.Length > 0 Then
                                pdr = dt.NewRow
                                pdr(0) = pagedr("id")
                                pdr(1) = menudr("id")
                                pdr(2) = pagedr("value")
                                pdr(3) = "P"
                                 pdr(4) = "1"
                                 dt.Rows.Add(pdr)

                            End If

                         Next
                    Next
                End If
            End If

            TreeViewRights.DataSource = dt
            TreeViewRights.DataBind()

            dt = Nothing
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            Response.Redirect("Information.aspx?mode=1&errno=" & "740192" & "&msg= " & GetErrorMessage("E_Web_ManageUserType_001") & "&next=ManageUserType.aspx&Title=" & "Manage User Type", False)
        Finally

        End Try
    End Sub

    Sub ExportRights(ByVal UserId As Integer)


            Dim ds As New DataSet()
            ds.ReadXml(New XmlNodeReader(XmlDataSource1.GetXmlDocument()))

            Dim dt As New DataTable
            dt.Columns.Add("UserName")
            dt.Columns.Add("Designation")
            dt.Columns.Add("Usertype")
            dt.Columns.Add("Menu")
            dt.Columns.Add("Page")

            Dim OutDt As New DataTable
            OutDt = objUser.GetAllUsers


            Dim desDt As New DataTable
            desDt = objUser.GetIsSS()

            If UserId = 0 Then
                For Each userdr As DataRow In OutDt.Rows

                    Dim destDr() As DataRow
                    destDr = desDt.Select("id='" & userdr("Is_SS") & "'")

                    Dim dr As DataRow
                     dr = dt.NewRow
                     dr("UserName") = userdr("UserName")
                     dr("Usertype") = userdr("User_Type")
                     If destDr.Length > 0 Then
                        dr("Designation") = destDr(0)("val")
                     End If

                     dr("Menu") = ""
                     dr("Page") = ""
                     dt.Rows.Add(dr)

                    Dim dtRights As New DataTable
                    objUser.UserTypeID = userdr("User_Type_ID").ToString
                    dtRights = objUser.GetUserRights()

                    For Each menudr As DataRow In ds.Tables(0).Rows
                        Dim mdr As DataRow
                        Dim rightdr() As DataRow
                        rightdr = dtRights.Select("Menu_Id='" & menudr("id") & "'")
                        If rightdr.Length > 0 Then
                               mdr = dt.NewRow
                               mdr("UserName") = ""
                               mdr("Designation") = ""
                               mdr("Usertype") = ""
                               mdr("Usertype") = ""
                               mdr("Menu") = menudr("value")
                               mdr("Page") = ""
                               dt.Rows.Add(mdr)
                        End If
                        Dim pgdr() As DataRow
                        pgdr = ds.Tables(1).Select("mainmenu_id=" & menudr("mainmenu_id"))
                         For Each pagedr As DataRow In pgdr
                            Dim pdr As DataRow

                            Dim prightdr() As DataRow
                            prightdr = dtRights.Select("Page_Id='" & pagedr("id") & "'")
                            If prightdr.Length > 0 Then
                                  pdr = dt.NewRow
                                  pdr("UserName") = ""
                                  pdr("Designation") = ""
                                  pdr("Usertype") = ""
                                  pdr("Usertype") = ""
                                  pdr("Menu") = ""
                                  pdr("Page") = pagedr("value")
                                  dt.Rows.Add(pdr)

                            End If
                         Next
                         Next
                Next
            Else

                Dim seldr() As DataRow
                seldr = OutDt.Select("User_ID=" & UserId)
                If seldr.Count > 0 Then

                    Dim destDr() As DataRow
                    destDr = desDt.Select("id='" & seldr(0)("Is_SS") & "'")

                    Dim dr As DataRow
                    dr = dt.NewRow
                    dr("UserName") = seldr(0)("UserName")
                    dr("Usertype") = seldr(0)("User_Type")
                     If destDr.Length > 0 Then
                        dr("Designation") = destDr(0)("val")
                     End If
                    dr("Menu") = ""
                    dr("Page") = ""
                    dt.Rows.Add(dr)

                    Dim dtRights As New DataTable
                    objUser.UserTypeID = seldr(0)("User_Type_Id")
                    dtRights = objUser.GetUserRights()

                    For Each menudr As DataRow In ds.Tables(0).Rows
                        Dim mdr As DataRow

                        Dim rightdr() As DataRow
                        rightdr = dtRights.Select("Menu_Id='" & menudr("id") & "'")
                        If rightdr.Length > 0 Then
                               mdr = dt.NewRow()
                               mdr("UserName") = ""
                               mdr("Designation") = ""
                               mdr("Usertype") = ""
                               mdr("Usertype") = ""
                               mdr("Menu") = menudr("value")
                               mdr("Page") = ""
                               dt.Rows.Add(mdr)

                        End If

                        Dim pgdr() As DataRow
                        pgdr = ds.Tables(1).Select("mainmenu_id=" & menudr("mainmenu_id"))
                         For Each pagedr As DataRow In pgdr
                            Dim pdr As DataRow

                            Dim prightdr() As DataRow
                            prightdr = dtRights.Select("Page_Id='" & pagedr("id") & "'")
                            If prightdr.Length > 0 Then
                                  pdr = dt.NewRow
                                  pdr("UserName") = ""
                                  pdr("Designation") = ""
                                  pdr("Usertype") = ""
                                  pdr("Usertype") = ""
                                  pdr("Menu") = ""
                                  pdr("Page") = pagedr("value")
                                  dt.Rows.Add(pdr)

                            End If

                         Next
                    Next
                End If
            End If
            ExportToExcel("RightsbyUsers.xls", dt)
            dt = Nothing

    End Sub
   Private Sub ExportToExcel(ByVal strFileName As String, ByVal dt As DataTable)


        Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & strFileName
        WriteXLSFile(fn, dt)
        Dim file As New FileInfo(fn)
        Response.Clear()
        Response.ClearHeaders()
        Response.ClearContent()
        Response.AddHeader("content-disposition", "attachment; filename=" + strFileName)
        Response.AddHeader("Content-Type", "application/Excel")
        Response.ContentType = "application/vnd.xls"
        Response.AddHeader("Content-Length", file.Length.ToString())
        Response.WriteFile(file.FullName)
        Response.End()
    End Sub
    Public Function WriteXLSFile(ByVal pFileName As String, ByVal dt As DataTable) As Boolean
        Try
            'This function CreateWorkbook will cause xls file cannot be opened
            'normally when file size below 7 KB, see my work around below
            'ExcelLibrary.DataSetHelper.CreateWorkbook(pFileName, pDataSet)

            'Create a workbook instance
            Dim workbook As Workbook = New Workbook()
            Dim worksheet As Worksheet
            Dim iRow As Integer = 0
            Dim iCol As Integer = 0
            Dim sTemp As String = String.Empty
            Dim dTemp As Double = 0
            Dim iTemp As Integer = 0
            '  Dim dtTime As DateTime
            Dim count As Integer = 0
            Dim iTotalRows As Integer = 0
            Dim iSheetCount As Integer = 0

            'Read DataSet


                    'Create a worksheet instance
                    iSheetCount = iSheetCount + 1
                    worksheet = New Worksheet("Sheet" & iSheetCount.ToString())

                    'Write Table Header
                    For Each dc As DataColumn In dt.Columns
                        worksheet.Cells(iRow, iCol) = New Cell(dc.ColumnName)
                        iCol = iCol + 1
                    Next

                    'Write Table Body
                    iRow = 1
                    For Each dr As DataRow In dt.Rows
                        iCol = 0
                        For Each dc As DataColumn In dt.Columns
                            sTemp = dr(dc.ColumnName).ToString()
                            worksheet.Cells(iRow, iCol) = New Cell(sTemp)
                            iCol = iCol + 1
                        Next
                        iRow = iRow + 1
                    Next

                    'Attach worksheet to workbook
                    workbook.Worksheets.Add(worksheet)
                    iTotalRows = iTotalRows + iRow


            'Bug on Excel Library, min file size must be 7 Kb
            'thus we need to add empty row for safety
            If iTotalRows < 100 Then
                worksheet = New Worksheet("Sheet2")
                count = 1
                Do While count < 100
                    worksheet.Cells(count, 0) = New Cell(" ")
                    count = count + 1
                Loop
                workbook.Worksheets.Add(worksheet)
            End If

            workbook.Save(pFileName)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Private Sub FindNodeByValue(ByVal n As TreeNodeCollection, ByVal val As String)
        Dim intNodeFound As Boolean
        If intNodeFound Then
            Return
        End If

        For i As Integer = 0 To n.Count - 1
            If n(i).Value = val Then
                n(i).[Select]()
                n(i).Checked = True
                intNodeFound = True
                Return
            End If
            ' n(i).Expand()
            FindNodeByValue(n(i).ChildNodes, val)
            If intNodeFound Then
                Return
            End If
            'n(i).Collapse()
        Next
    End Sub

   

    Protected Sub drpUserTypes_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles drpUser.SelectedIndexChanged


        LoadRights(drpUser.SelectedValue)
        TreeViewRights.ExpandAllItems()

    End Sub


    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblmsg.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        Exit Sub
    End Sub


    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        ExportRights(drpUser.SelectedItem.Value)
    End Sub

    Private Sub TreeViewRights_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.TreeListCommandEventArgs) Handles TreeViewRights.ItemCommand
        LoadRights(drpUser.SelectedItem.Value)
    End Sub

    Private Sub TreeViewRights_PageIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.TreeListPageChangedEventArgs) Handles TreeViewRights.PageIndexChanged
       LoadRights(drpUser.SelectedItem.Value)

    End Sub

    Private Sub TreeViewRights_PageSizeChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.TreeListPageSizeChangedEventArgs) Handles TreeViewRights.PageSizeChanged
    If Not drpUser.SelectedItem Is Nothing Then
        LoadRights(drpUser.SelectedItem.Value)
    End If
    End Sub
End Class