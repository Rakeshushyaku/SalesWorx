Imports System.Data.OleDb
Imports System.IO
Imports log4net
Imports System.Data.SqlClient
Imports SalesWorx.BO.Common

Partial Public Class ImportBonusRules
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim objProduct As New Product
    Private dtErrors As New DataTable
    Private bReimport As Boolean = False
    Private Const PageID As String = "P222"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub ImportBonusRules_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Import Bonus Rules"
    End Sub

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
            divResult.Visible = False
            ViewState("FileType") = Nothing
            ViewState("FileName") = Nothing
            ViewState("CSVName") = Nothing
            Session.Remove("dtErrors")
            SetErrorsTable()
        Else
            dtErrors = Session("dtErrors")
        End If
    End Sub
    Private Sub SetErrorsTable()
        Dim col As DataColumn


        col = New DataColumn()
        col.ColumnName = "RowNo"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtErrors.Columns.Add(col)

     
        col = New DataColumn()
        col.ColumnName = "ColNo"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtErrors.Columns.Add(col)


        col = New DataColumn()
        col.ColumnName = "ColName"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtErrors.Columns.Add(col)


        col = New DataColumn()
        col.ColumnName = "ErrorText"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtErrors.Columns.Add(col)

        Session.Remove("dtErrors")
        Session("dtErrors") = dtErrors
    End Sub
    Public Function FormatDate(ByVal DateVal As String) As Date
        If (Not IsNothing(DateVal)) AndAlso IsValidInputDate(DateVal) Then
            'Dim TemFromDateStr As String = DateVal
            'Dim DateArr As Array = TemFromDateStr.Split("/")
            'If DateArr.Length = 3 Then
            '    TemFromDateStr = DateArr(1) & "/" & DateArr(0) & "/" & DateArr(2)
            'Else
            '    TemFromDateStr = "01/01/1900"
            'End If
            'Return CType(DateVal, Date).ToString("MM/dd/yyyy")
            Return CDate(DateVal)
        End If
    End Function
    Protected Sub btnImportSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnImport.Click
        If Me.ExcelFileUpload.FileName = Nothing Then
            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Select filename "
            Me.lblMessage.ForeColor = Drawing.Color.Green
            Me.MpInfoError.Show()
            Exit Sub
        End If

        Dim Str As New StringBuilder
        dtErrors = Session("dtErrors")
        Dim TotSuccess As Integer = 0
        If dtErrors.Rows.Count > 0 Then
            dtErrors.Rows.Clear()
            Me.dgvErros.DataSource = dtErrors
            Me.dgvErros.DataBind()
        End If
        Try
            ViewState("FileType") = Me.ExcelFileUpload.PostedFile.ContentType
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
                    ViewState("FileName") = Foldername & FName
                    ViewState("CSVName") = FName
                Else
                    ViewState("FileName") = Foldername & Now.Hour & Now.Minute & Now.Second & ExcelFileUpload.FileName
                End If

                ExcelFileUpload.SaveAs(ViewState("FileName"))

                Try
                    Dim st As Boolean = False

                    If ViewState("FileType") IsNot Nothing And ViewState("FileName") IsNot Nothing Then
                        Dim TempTbl As New DataTable
                        If TempTbl.Rows.Count > 0 Then
                            TempTbl.Rows.Clear()
                        End If
                        Dim col As DataColumn

                        col = New DataColumn
                        col.ColumnName = "OrgID"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)


                        col = New DataColumn
                        col.ColumnName = "OrderItem"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)



                        col = New DataColumn
                        col.ColumnName = "BonusItem"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)




                        col = New DataColumn
                        col.ColumnName = "FromQty"
                        col.DataType = System.Type.GetType("System.Double")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)


                        col = New DataColumn
                        col.ColumnName = "ToQty"
                        col.DataType = System.Type.GetType("System.Double")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)


                        col = New DataColumn
                        col.ColumnName = "Type"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)


                        col = New DataColumn
                        col.ColumnName = "GetQty"
                        col.DataType = System.Type.GetType("System.Double")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)


                        col = New DataColumn
                        col.ColumnName = "ValidFrom"
                        col.DataType = System.Type.GetType("System.DateTime")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)

                        col = New DataColumn
                        col.ColumnName = "ValidTo"
                        col.DataType = System.Type.GetType("System.DateTime")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)

                        col = New DataColumn
                        col.ColumnName = "MaxFOCQty"
                        col.DataType = System.Type.GetType("System.Double")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)

                        If ViewState("FileName").ToString.EndsWith(".csv") Then
                            TempTbl = DoCSVUpload()
                        ElseIf ViewState("FileName").ToString.EndsWith(".xls") Then
                            TempTbl = DoXLSUpload()
                        ElseIf ViewState("FileName").ToString.EndsWith(".xlsx") Then
                            TempTbl = DoXLSXUpload()
                        End If

                        If TempTbl.Columns.Count = 10 Then
                            If Not (TempTbl.Columns(0).ColumnName = "OrgID" And TempTbl.Columns(1).ColumnName = "OrderItem" And TempTbl.Columns(2).ColumnName = "BonusItem" And TempTbl.Columns(3).ColumnName = "FromQty" And TempTbl.Columns(4).ColumnName = "ToQty" And TempTbl.Columns(5).ColumnName = "Type" And TempTbl.Columns(6).ColumnName = "GetQty" And TempTbl.Columns(7).ColumnName = "ValidFrom" And TempTbl.Columns(8).ColumnName = "ValidTo" And TempTbl.Columns(9).ColumnName = "MaxFOCQty") Then
                                lblMessage.Text = "Please check the template columns are correct"
                                lblMessage.ForeColor = Drawing.Color.Green
                                lblinfo.Text = "Information"
                                MpInfoError.Show()
                                Exit Sub
                            End If
                        Else
                            lblMessage.Text = "Invalid Template"
                            lblMessage.ForeColor = Drawing.Color.Green
                            lblinfo.Text = "Information"
                            MpInfoError.Show()
                            Exit Sub
                        End If




                        If TempTbl.Rows.Count = 0 Then
                            lblMessage.Text = "There is no data in your file."
                            lblMessage.ForeColor = Drawing.Color.Green
                            lblinfo.Text = "Information"
                            MpInfoError.Show()
                            Exit Sub
                        End If

                        Dim RowNo As String = Nothing
                        ' Dim ColNo As String = Nothing
                        ' Dim ColumnName As String = Nothing
                        Dim ErrorText As String = Nothing

                        If TempTbl.Rows.Count > 0 Then
                            Dim idx As Integer

                            For idx = 0 To TempTbl.Rows.Count - 1
                                Dim OrgID As String = Nothing
                                Dim OrderItem As String = Nothing
                                Dim OrderUOM As String = "EA"
                                Dim BonusItem As String = Nothing
                                Dim BonusUOM As String = "EA"
                                Dim FromQty As String = Nothing
                                Dim ToQty As String = Nothing
                                Dim Type As String = Nothing
                                Dim GetQty As String = Nothing
                                Dim ValidFrom As String = Nothing
                                Dim ValidTo As String = Nothing
                                Dim MaxFOCQty As String = Nothing
                                Dim FromDate As Date = Nothing
                                Dim ToDate As Date = Nothing

                                Dim isValidRow As Boolean = True

                                OrgID = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, "0", TempTbl.Rows(idx)(0).ToString())
                                OrderItem = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, "0", TempTbl.Rows(idx)(1).ToString())
                                BonusItem = IIf(TempTbl.Rows(idx)(2) Is DBNull.Value, "0", TempTbl.Rows(idx)(2).ToString())
                                FromQty = IIf(TempTbl.Rows(idx)(3) Is DBNull.Value, "0", TempTbl.Rows(idx)(3).ToString())
                                ToQty = IIf(TempTbl.Rows(idx)(4) Is DBNull.Value, "0", TempTbl.Rows(idx)(4).ToString())
                                Type = IIf(TempTbl.Rows(idx)(5) Is DBNull.Value, "0", TempTbl.Rows(idx)(5).ToString())
                                GetQty = IIf(TempTbl.Rows(idx)(6) Is DBNull.Value, "0", TempTbl.Rows(idx)(6).ToString())
                                ValidFrom = Trim(Replace(IIf(TempTbl.Rows(idx)(7) Is DBNull.Value, "0", TempTbl.Rows(idx)(7).ToString()), "12:00:00 AM", ""))
                                ValidTo = Trim(Replace(IIf(TempTbl.Rows(idx)(8) Is DBNull.Value, "0", TempTbl.Rows(idx)(8).ToString()), "12:00:00 AM", ""))
                                MaxFOCQty = IIf(TempTbl.Rows(idx)(9) Is DBNull.Value, "0", TempTbl.Rows(idx)(9).ToString())
                                If OrgID <> "0" Then

                                    If OrgID = "0" Or objProduct.CheckOrgID(Err_No, Err_Desc, OrgID) = False Then
                                        RowNo = idx + 2
                                        '  ColNo = "1" + ","
                                        ' ColumnName = "OrgID" + ","
                                        ErrorText = "Invalid Org ID" + ","
                                        isValidRow = False
                                    End If
                                    If OrderItem = "0" Or objProduct.CheckItemCode(Err_No, Err_Desc, OrderItem, OrgID) = False Then
                                        RowNo = idx + 2
                                        ' ColNo = ColNo + "2" + ","
                                        ' ColumnName = ColumnName + "OrderItem" + ","
                                        ErrorText = ErrorText + "Invalid order Item code" + ","
                                        isValidRow = False
                                    Else
                                        OrderUOM = objProduct.GetItemUOM(Err_No, Err_Desc, OrderItem, OrgID)
                                    End If

                                    If BonusItem = "0" Or objProduct.CheckItemCode(Err_No, Err_Desc, BonusItem, OrgID) = False Then
                                        RowNo = idx + 2
                                        ' ColNo = ColNo + "3" + ","
                                        ' ColumnName = ColumnName + "BonusItem" + ","
                                        ErrorText = ErrorText + "Invalid bonus item code" + ","
                                        isValidRow = False
                                    Else
                                        BonusUOM = objProduct.GetItemUOM(Err_No, Err_Desc, OrderItem, OrgID)
                                    End If

                                    If FromQty = "0" Or IsNumeric(FromQty) = False Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Invalid from qty" + ","
                                        isValidRow = False
                                    End If

                                    If ToQty = "0" Or IsNumeric(ToQty) = False Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Invalid to qty" + ","
                                        isValidRow = False
                                    End If

                                    If FromQty <> "0" And ToQty <> "0" And IsNumeric(FromQty) = True And IsNumeric(ToQty) = True Then
                                        If CLng(ToQty) <= CLng(FromQty) Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "To qty should be greater than from qty" + ","
                                            isValidRow = False
                                        End If
                                    End If

                                    'If Type = "0" Or Not (Type.ToUpper() = "POINT" Or Type.ToUpper() = "RECURRING" Or Type.ToUpper() = "PERCENT") Then
                                    If Type = "0" Or Not (Type.ToUpper() = "POINT" Or Type.ToUpper() = "RECURRING") Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Invalid type" + ","
                                        isValidRow = False
                                    End If

                                    If GetQty = "0" Or IsNumeric(GetQty) = False Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Invalid get qty" + ","
                                        isValidRow = False
                                    End If
                                    If IsNumeric(MaxFOCQty) = False Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Max.FOC qty should be in numeric" + ","
                                        isValidRow = False
                                    End If
                                    If ValidFrom = "0" Or IsValidInputDate(ValidFrom) = False Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Invalid valid from" + ","
                                        isValidRow = False
                                    Else
                                        FromDate = FormatDate(ValidFrom)
                                    End If
                                    If ValidTo = "0" Or IsValidInputDate(ValidTo) = False Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Invalid valid to" + ","
                                        isValidRow = False
                                    Else
                                        ToDate = FormatDate(ValidTo)
                                    End If

                                    If ValidFrom <> "0" And IsValidInputDate(ValidFrom) = True And ValidTo <> "0" And IsValidInputDate(ValidTo) = True Then
                                        If FromDate <= Now.Date Or ToDate <= Now.Date Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "Valid from and to date should be greater than current date" + ","
                                            isValidRow = False
                                        End If
                                    End If

                                    If ValidFrom <> "0" And IsValidInputDate(ValidFrom) = True And ValidTo <> "0" And IsValidInputDate(ValidTo) = True Then
                                        If ToDate <= FromDate Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "Valid to date should be greater than from date" + ","
                                            isValidRow = False
                                        End If
                                    End If

                                    If isValidRow = True Then
                                        Dim x As New DataTable
                                        x = objProduct.CheckBonusDataActiveRange(Err_No, Err_Desc, OrderItem, CLng(IIf(FromQty = "", "0", FromQty)), CLng(IIf(ToQty = "", "0", ToQty)), OrgID, FromDate, ToDate, "0", OrderUOM, "0")
                                        If x.Rows.Count > 0 Then
                                            RowNo = idx + 2
                                            ErrorText = ErrorText + "Same rule already exist for this item" + ","
                                        End If
                                    End If


                                    If Not (RowNo Is Nothing And ErrorText Is Nothing) Then
                                        Dim h As DataRow = dtErrors.NewRow()
                                        h("RowNo") = RowNo
                                        ' h("ColNo") = ColNo
                                        ' h("ColName") = ColumnName
                                        h("Errortext") = ErrorText
                                        dtErrors.Rows.Add(h)
                                        RowNo = Nothing
                                        'ColNo = Nothing
                                        'ColumnName = Nothing
                                        ErrorText = Nothing
                                        isValidRow = True
                                    Else

                                        If objProduct.SaveBonusData(Err_No, Err_Desc, OrderItem, OrgID, OrderUOM, BonusItem, BonusUOM, Type, CLng(FromQty), CLng(ToQty), CLng(GetQty), 0, FromDate, ToDate, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, "0", IIf(MaxFOCQty Is Nothing Or MaxFOCQty = "", 0, MaxFOCQty)) = True Then
                                            TotSuccess = TotSuccess + 1
                                            Dim h As DataRow = dtErrors.NewRow()
                                            h("RowNo") = idx + 2
                                            h("Errortext") = "Successfully uploaded"
                                            dtErrors.Rows.Add(h)
                                            RowNo = Nothing
                                            ErrorText = Nothing
                                            isValidRow = True
                                        Else
                                            Dim h As DataRow = dtErrors.NewRow()
                                            h("RowNo") = idx + 2
                                            h("Errortext") = "Error occured while uploading this row"
                                            dtErrors.Rows.Add(h)
                                            RowNo = Nothing
                                            ErrorText = Nothing
                                            isValidRow = True

                                        End If


                                    End If
                                End If
                            Next
                        End If


                        Me.dgvErros.DataSource = dtErrors
                        Me.dgvErros.DataBind()
                        Session.Remove("dtErrors")
                        Session("dtErrors") = dtErrors

                        If TotSuccess > 0 Then
                            DeleteExcel()
                            lblMessage.Text = TotSuccess.ToString() & " rows successfully uploaded"
                            lblMessage.ForeColor = Drawing.Color.Green
                            lblinfo.Text = "Information"
                            MpInfoError.Show()
                        End If
                    End If


                Catch ex As Exception

                    Err_No = "74085"
                    If Err_Desc Is Nothing Then
                        log.Error(GetExceptionInfo(ex))
                    Else
                        log.Error(Err_Desc)
                    End If
                End Try
            Else
                ' Str.Append("<span style='font-color:red'> <b >Please import valid Excel template.</b></span>")
                lblMessage.Text = "Please import valid Excel template."
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
            End If


        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try

    End Sub
    Public Shared Function IsValidInputDate(ByVal str As String) As Boolean
        'Dim dt As DateTime
        'Dim success As Boolean = False
        'success = DateTime.TryParseExact(str, "dd/MM/yyyy", Nothing, Globalization.DateTimeStyles.None, dt)
        'If success = False Then
        '    success = DateTime.TryParseExact(str, "d/M/yyyy", Nothing, Globalization.DateTimeStyles.None, dt)
        'End If
        'Return success
        Return True
    End Function
   
    Protected Sub DummyImBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles DummyImBtn.Click
       

    End Sub



    Private Function DoCSVUpload() As DataTable
        Dim dtImport As New DataTable
        Try
            Dim strConString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "\;Extended Properties=""text;HDR=Yes;FMT=Delimited"""
            Dim oledbConn As New OleDbConnection(strConString)

            Dim cmd As New OleDbCommand("SELECT * FROM [" & ViewState("CSVName") & "]", oledbConn)

            Dim oleda As New OleDbDataAdapter()

            oleda.SelectCommand = cmd



            oleda.Fill(dtImport)
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        End Try
        Return dtImport
    End Function

    Private Function DoXLSUpload() As DataTable
        Dim dtImport As New DataTable
        Try
            Dim connString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & ViewState("FileName") & " ;Extended Properties=""Excel 8.0;HDR=Yes;IMEX=1"""
            Dim oledbConn As New OleDbConnection(connString)
            Try
                oledbConn.Open()

                Dim cmd As New OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn)

                Dim oleda As New OleDbDataAdapter()

                oleda.SelectCommand = cmd



                oleda.Fill(dtImport)



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
        Return dtImport
    End Function

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
    Private Function DoXLSXUpload() As DataTable
        Dim dtImport As New DataTable
        Try
            Dim connString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & ViewState("FileName") & " ;Extended Properties=""Excel 12.0;HDR=Yes;IMEX=1"""
            Dim oledbConn As New OleDbConnection(connString)
            Try
                oledbConn.Open()

                Dim cmd As New OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn)

                Dim oleda As New OleDbDataAdapter()

                oleda.SelectCommand = cmd



                oleda.Fill(dtImport)



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
        Return dtImport
    End Function
End Class