Imports System.Data.OleDb
Imports System.IO
Imports log4net
Imports System.Data.SqlClient
Imports SalesWorx.BO.Common
Partial Public Class ImportLatitude
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim objRoutePlan As New RoutePlan
    Private bReimport As Boolean = False
    Private Const PageID As String = "P262"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        If Not IsPostBack Then
            divResult.Visible = False
            ViewState("FileType") = Nothing
            ViewState("FileName") = Nothing
            ViewState("CSVName") = Nothing
            Session("Tbl") = Nothing
            Session("DoneValidation") = False
        End If
    End Sub

    Private Sub BtnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnImport.Click
        divResult.Visible = True
        Dim Str As New StringBuilder
        Session("Tbl") = Nothing
        Try
            ViewState("FileType") = Me.ExcelFileUpload.PostedFile.ContentType
            '  ViewState("FileName") = Me.ExcelFileUpload.PostedFile.FileName

            ''Finding FileType
            If ExcelFileUpload.FileName.ToString.EndsWith(".csv") Or ExcelFileUpload.FileName.ToString.EndsWith(".xls") Or ExcelFileUpload.FileName.ToString.EndsWith(".xlsx") Then

                Dim Foldername As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath")
                If Not Foldername.EndsWith("\") Then
                    Foldername = Foldername & "\"
                End If
                If Directory.Exists(Foldername.Substring(0, Len(Foldername) - 1)) = False Then
                    Directory.CreateDirectory(Foldername.Substring(0, Len(Foldername) - 1))
                End If
                If ExcelFileUpload.FileName.ToString.EndsWith(".csv") Then
                    Dim FName As String
                    FName = Now.Hour & Now.Minute & Now.Second & ExcelFileUpload.FileName
                    ViewState("FileName") = Foldername & "Latitude" & FName
                    ViewState("CSVName") = "Latitude" & FName
                Else
                    ViewState("FileName") = Foldername & "Latitude" & Now.Hour & Now.Minute & Now.Second & ExcelFileUpload.FileName
                End If

                '   ViewState("FileName") = Foldername & ExcelFileUpload.FileName
                ExcelFileUpload.SaveAs(ViewState("FileName"))

                Try
                    System.Threading.Thread.Sleep(2000)

                    Dim ConfirmStr As StringBuilder
                    If ViewState("FileType") IsNot Nothing And ViewState("FileName") IsNot Nothing Then
                        Dim TempTbl As New DataTable
                        TempTbl = Nothing
                        If ViewState("FileName").ToString.EndsWith(".csv") Then
                            DoCSVUpload(TempTbl)
                        ElseIf ViewState("FileName").ToString.EndsWith(".xls") Then
                            DoXLSUpload(TempTbl)
                        ElseIf ViewState("FileName").ToString.EndsWith(".xlsx") Then
                            DoXLSXUpload(TempTbl)
                        End If
                        Str = New StringBuilder
                        ConfirmStr = New StringBuilder
                        If TempTbl IsNot Nothing Then
                            Str = New StringBuilder
                            ConfirmStr = New StringBuilder
                            If TempTbl.Rows.Count > 0 Then
                                If DoValidation(TempTbl, Str, ConfirmStr) Then
                                    Session("DoneValidation") = True
                                    If Str.ToString() = "" And ConfirmStr.ToString = "" Then
                                        Err_Desc = Nothing
                                        Err_No = Nothing
                                        SaveData(TempTbl, Err_Desc, Err_No)
                                    End If
                                End If
                            Else
                                Session("DoneValidation") = False
                                Str.Append("<span style='font-color:red'> <b >Template does not have data to import</b></span>")
                            End If
                        Else
                            Session("DoneValidation") = False
                            Str.Append("<span style='font-color:red'> <b >Please import valid Excel template.</b></span>")
                        End If
                        lblResult.Text = Str.ToString()
                        DeleteExcel()
                        If lblResult.Text = "" And ConfirmStr.ToString <> "" Then
                            lblResult.Text = "<span style='font-color:red'> <b > Confirm the below Information and Click Import to Continue</b></span>" & ConfirmStr.ToString
                            Session("Tbl") = TempTbl
                        End If
                        Exit Sub
                    End If
                Catch ex As Exception
                    log.Error(GetExceptionInfo(ex))
                    Throw ex
                End Try

                '          Dim strScript As String
                '          strScript = "<script language='javascript'>"
                '          strScript += "document.aspnetForm('ctl00_ContentPlaceHolder1_DummyImBtn').click();"
                '          strScript += "</script>"
                '          Page.ClientScript.RegisterStartupScript(Me.GetType(), "StrScript", _
                'strScript, False)
            Else
                Str.Append("<span style='font-color:red'> <b >Please import valid Excel template.</b></span>")
                lblResult.Text = Str.ToString()
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub

    'Private Sub BtnReimport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnReimport.Click
    '    divResult.Visible = True
    '    If Session("Tbl") IsNot Nothing And Session("DoneValidation") = True Then
    '        bReimport = True
    '        SavePlan(CType(Session("Tbl"), DataTable), Err_Desc, Err_No)
    '        bReimport = False
    '    End If
    'End Sub
    Protected Sub DummyImBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles DummyImBtn.Click
        Try
            System.Threading.Thread.Sleep(2000)
            Dim Str As StringBuilder
            Dim ConfirmStr As StringBuilder
            If ViewState("FileType") IsNot Nothing And ViewState("FileName") IsNot Nothing Then
                Dim TempTbl As New DataTable
                TempTbl = Nothing
                If ViewState("FileName").ToString.EndsWith(".csv") Then
                    DoCSVUpload(TempTbl)
                ElseIf ViewState("FileName").ToString.EndsWith(".xls") Then
                    DoXLSUpload(TempTbl)
                ElseIf ViewState("FileName").ToString.EndsWith(".xlsx") Then
                    DoXLSXUpload(TempTbl)
                End If
                Str = New StringBuilder
                ConfirmStr = New StringBuilder
                If TempTbl IsNot Nothing Then
                    Str = New StringBuilder
                    ConfirmStr = New StringBuilder
                    If TempTbl.Rows.Count > 0 Then
                        If DoValidation(TempTbl, Str, ConfirmStr) Then
                            Session("DoneValidation") = True
                            If Str.ToString() = "" And ConfirmStr.ToString = "" Then
                                Err_Desc = Nothing
                                Err_No = Nothing
                                SaveData(TempTbl, Err_Desc, Err_No)
                            End If
                        End If
                    Else
                        Session("DoneValidation") = False
                        Str.Append("<span style='font-color:red'> <b >Template does not have data to import</b></span>")
                    End If
                Else
                    Session("DoneValidation") = False
                    Str.Append("<span style='font-color:red'> <b >Please import valid Excel template.</b></span>")
                End If
                lblResult.Text = Str.ToString()
                DeleteExcel()
                If lblResult.Text = "" And ConfirmStr.ToString <> "" Then
                    lblResult.Text = "<span style='font-color:red'> <b > Confirm the below Information and Click Import to Continue</b></span>" & ConfirmStr.ToString
                    Session("Tbl") = TempTbl
                End If
                Exit Sub
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        End Try
    End Sub
    Private Sub SaveData(ByVal TempTbl As DataTable, ByVal Err_Desc As String, ByVal Err_No As Long)
        Dim obj As New LatiLongitude
        Try
            TempTbl = TempTbl.DefaultView.ToTable(True)   '' filter duplication of the rows
            If obj.SaveData(TempTbl, Err_No, Err_Desc) = True Then
                Session("TblEmployee") = Nothing
                Response.Redirect("MngLatiLongitude.aspx?Src=Import", False)
            Else
                Response.Redirect("information.aspx?mode=1&errno=" & "74088" & "&msg=" & SalesWorx.BO.Common.AppMsgHandler.GetErrorMessage("E_BO_005") & "&next=ImportLatitude.aspx&Title=Import+Geolocation Data", False)
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("information.aspx?mode=1&errno=" & "74088" & "&msg=" & SalesWorx.BO.Common.AppMsgHandler.GetErrorMessage("E_BO_005") & "&next=ImportLatitude.aspx&Title=Import+Geolocation Data", False)
        End Try
    End Sub
 
    Public Function IsValidLatitude(ByVal LatStr As String) As Boolean
        Dim _isDouble As System.Text.RegularExpressions.Regex = New  _
                Regex("^-?([1-8]?[1-9]|[1-9]0)\.{1}\d{1,6}")
        Return _isDouble.Match(LatStr).Success
    End Function
    Public Function IsValidLongitude(ByVal LatStr As String) As Boolean
        Dim _isDouble As System.Text.RegularExpressions.Regex = New  _
                Regex("^-?([1]?[1-7][1-9]|[1]?[1-8][0]|[1-9]?[0-9])\.{1}\d{1,6}")
        Return _isDouble.Match(LatStr).Success
    End Function
    Private Function DoValidation(ByVal Tbl As DataTable, ByVal Str As StringBuilder, ByRef ConfirmStr As StringBuilder) As Boolean
        Dim obj As New LatiLongitude
        Dim TempTbl As New DataTable
        Try
            If Session("TblEmployee") Is Nothing Then
                TempTbl = obj.FillCusShipAddress(Err_No, Err_Desc)
            Else
                TempTbl = CType(Session("TblEmployee"), DataTable)
            End If


            Dim RowCount As Integer = 1
            For Each dr As DataRow In Tbl.Rows
                If dr(0).ToString <> "Text" Then
                    RowCount += 1
                    Try
                        Dim HasNull As Boolean = False
                        If dr.Item(0) Is DBNull.Value Then
                            Continue For
                        End If
                        If dr.Item(0) Is DBNull.Value Then
                            HasNull = True
                            Session("DoneValidation") = False
                            Str.Append("<br/><span style='font-color:red'> <b >Customer No is Null at Row No: " & RowCount & " </b></span>")
                        End If


                        If dr.Item(1) Is DBNull.Value Then
                            HasNull = True
                            Session("DoneValidation") = False
                            Str.Append("<br/><span style='font-color:red'> <b >Latitude is Null at Row No: " & RowCount & " </b></span>")
                        End If

                        If dr.Item(2) Is DBNull.Value Then
                            HasNull = True
                            Session("DoneValidation") = False
                            Str.Append("<br/><span style='font-color:red'> <b >Longitude is Null at Row No: " & RowCount & " </b></span>")
                        End If


                        If dr.Item(0) IsNot DBNull.Value Then
                            Dim SelRow() As DataRow = TempTbl.Select("Customer_No='" & dr.Item(0).ToString() & "'")
                            If SelRow.Length = 0 Then
                                Session("DoneValidation") = False
                                Str.Append("<br/><span style='font-color:red'> <b >Invalid Customer No at Row No: " & RowCount & " </b></span>")
                            Else
                                If dr.Item(1) IsNot DBNull.Value AndAlso Not IsValidLatitude(dr.Item(1)) Then
                                    Session("DoneValidation") = False
                                    Str.Append("<br/><span style='font-color:red'> <b >Invalid Latitude value at Row No: " & RowCount & " </b></span>")
                                End If

                                If dr.Item(2) IsNot DBNull.Value AndAlso Not IsValidLongitude(dr.Item(2)) Then
                                    Session("DoneValidation") = False
                                    Str.Append("<br/><span style='font-color:red'> <b >Invalid Longitude value at Row No: " & RowCount & " </b></span>")
                                End If
                            End If
                        End If

                    Catch ex As Exception
                        log.Error(GetExceptionInfo(ex))
                        Response.Redirect("information.aspx?mode=1&errno=" & "74089" & "&msg=" & SalesWorx.BO.Common.AppMsgHandler.GetErrorMessage("E_BO_005") & "&next=ImportRoutePlan.aspx&Title=Route+Planner", False)
                    End Try
                End If
            Next
            If Str.ToString() <> "" Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("information.aspx?mode=1&errno=" & "74090" & "&msg=" & SalesWorx.BO.Common.AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=ImportRoutePlan.aspx&Title=Route+Planner", False)
            Throw ex
        End Try
    End Function
    Private Sub DoCSVUpload(ByRef Tbl As DataTable)
        Try
            Dim strConString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "\;Extended Properties=""text;HDR=Yes;FMT=Delimited"""
            Dim oledbConn As New OleDbConnection(strConString)

            Dim cmd As New OleDbCommand("SELECT * FROM [" & ViewState("CSVName") & "]", oledbConn)

            Dim oleda As New OleDbDataAdapter()

            oleda.SelectCommand = cmd

            Dim ds As New DataSet()

            oleda.Fill(ds, "Employee")

            Tbl = ds.Tables(0)
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        End Try

    End Sub
    Private Sub DoXLSUpload(ByRef Tbl As DataTable)
        Try
            Dim connString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & ViewState("FileName") & " ;Extended Properties=""Excel 8.0;HDR=Yes;IMEX=1"""
            Dim oledbConn As New OleDbConnection(connString)
            Try
                oledbConn.Open()

                Dim cmd As New OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn)

                Dim oleda As New OleDbDataAdapter()

                oleda.SelectCommand = cmd

                Dim ds As New DataSet()

                oleda.Fill(ds, "Employee")

                Tbl = ds.Tables(0)

            Catch ex As Exception
                log.Error(GetExceptionInfo(ex))
            Finally
                ' Close connection
                oledbConn.Close()
            End Try
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        End Try
    End Sub
    Private Sub DeleteExcel()
        Try
            Dim TheFile As FileInfo = New FileInfo(ViewState("FileName"))
            If TheFile.Exists Then
                File.Delete(ViewState("FileName"))
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))

        End Try
    End Sub
    Private Sub DoXLSXUpload(ByRef Tbl As DataTable)
        Try
            Dim connString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & ViewState("FileName") & " ;Extended Properties=""Excel 12.0;HDR=Yes;IMEX=1"""
            Dim oledbConn As New OleDbConnection(connString)
            Try
                oledbConn.Open()

                Dim cmd As New OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn)

                Dim oleda As New OleDbDataAdapter()

                oleda.SelectCommand = cmd

                Dim ds As New DataSet()

                oleda.Fill(ds, "Employee")

                Tbl = ds.Tables(0)

            Catch ex As Exception
                log.Error(GetExceptionInfo(ex))
            Finally
                ' Close connection
                oledbConn.Close()
            End Try
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        End Try
    End Sub
    Private Shared Sub setLastValues(ByVal lastValues() As Object, ByVal sourceRow As DataRow, ByVal fieldNames() As String)
        For i As Integer = 0 To fieldNames.Length - 1
            lastValues(i) = sourceRow(fieldNames(i))
        Next
    End Sub
    Private Shared Function createRowClone(ByVal sourceRow As DataRow, ByVal newRow As DataRow, ByVal fieldNames() As String) As DataRow
        For Each field As String In fieldNames
            newRow(field) = sourceRow(field)
        Next

        Return newRow
    End Function
    Private Shared Function fieldValuesAreEqual(ByVal lastValues() As Object, ByVal currentRow As DataRow, ByVal fieldNames() As String) As Boolean
        Dim areEqual As Boolean = True

        For i As Integer = 0 To fieldNames.Length - 1
            If lastValues(i) Is Nothing OrElse Not lastValues(i).Equals(currentRow(fieldNames(i))) Then
                areEqual = False
                Exit For
            End If
        Next

        Return areEqual
    End Function

    Public Shared Function SelectDistinct(ByVal SourceTable As DataTable, ByVal ParamArray FieldNames() As String) As DataTable
        Try
            Dim lastValues() As Object
            Dim newTable As DataTable

            If FieldNames Is Nothing OrElse FieldNames.Length = 0 Then
                Throw New ArgumentNullException("FieldNames")
            End If

            lastValues = New Object(FieldNames.Length - 1) {}
            newTable = New DataTable

            For Each field As String In FieldNames
                newTable.Columns.Add(field, SourceTable.Columns(field).DataType)
            Next

            For Each Row As DataRow In SourceTable.Select("", String.Join(", ", FieldNames))
                If Not fieldValuesAreEqual(lastValues, Row, FieldNames) Then
                    newTable.Rows.Add(createRowClone(Row, newTable.NewRow(), FieldNames))

                    setLastValues(lastValues, Row, FieldNames)
                End If
            Next

            Return newTable
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        End Try
    End Function

    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Session("TblEmployee") = Nothing
        Response.Redirect("MngLatiLongitude.aspx")
    End Sub
End Class