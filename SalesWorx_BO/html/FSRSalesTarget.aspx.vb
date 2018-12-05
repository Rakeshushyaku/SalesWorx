Imports SalesWorx.BO.Common
Imports System.Data
Imports System.Web
Imports System.Resources
Imports System.Web.UI.WebControls
Imports log4net
Imports System.Text.RegularExpressions
Imports System.IO
Imports ExcelLibrary.SpreadSheet
Imports System.Data.OleDb

Imports Telerik
Imports Telerik.Web.UI

Partial Public Class FSRSalesTarget
    Inherits System.Web.UI.Page
    Dim objProduct As New Product
    Dim obj As New SalesTarget
    Dim Err_No As Long
    Dim Err_Desc As String
    Private dtErrors As New DataTable
    Private Const PageID As String = "P410"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private _tabIndex As Integer = 0

    Private Sub Page_AbortTransaction(sender As Object, e As EventArgs) Handles Me.AbortTransaction

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        If Not Page.IsPostBack Then
            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            If Not HasPermission Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If

            Dim objCommon As New SalesWorx.BO.Common.Common
            Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
            ddOraganisation.ClearSelection()
            ddOraganisation.Items.Clear()
            ddOraganisation.Text = ""
            ddOraganisation.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)

            ddOraganisation.DataValueField = "MAS_Org_ID"
            ddOraganisation.DataTextField = "Description"
            ddOraganisation.DataBind()
            ddOraganisation.Items.Insert(0, New RadComboBoxItem("-- Select a Organization --", "-1"))
            If ddOraganisation.Items.Count = 2 Then
                Me.ddOraganisation.SelectedIndex = 1
                LoadFSR()
            End If

            If CType(Session.Item("CONTROL_PARAMS"), ControlParams).FSR_TARGET_TYPE = "A" Then
                Me.lblSecondary.Text = "Agency"

            ElseIf CType(Session.Item("CONTROL_PARAMS"), ControlParams).FSR_TARGET_TYPE = "K" Then

                Me.lblSecondary.Text = "Category"

            ElseIf CType(Session.Item("CONTROL_PARAMS"), ControlParams).FSR_TARGET_TYPE = "B" Then

                Me.lblSecondary.Text = "Brand"
            ElseIf CType(Session.Item("CONTROL_PARAMS"), ControlParams).FSR_TARGET_TYPE = "P" Then

                Me.lblSecondary.Text = "Product"
            End If

            If CType(Session.Item("CONTROL_PARAMS"), ControlParams).FSR_TARGET_PRIMARY = "None" Then
                Me.lblPrimaryValue.Text = "None"
            ElseIf CType(Session.Item("CONTROL_PARAMS"), ControlParams).FSR_TARGET_PRIMARY = "Area" Then
                Me.lblPrimaryValue.Text = "Area"
            ElseIf CType(Session.Item("CONTROL_PARAMS"), ControlParams).FSR_TARGET_PRIMARY = "Customer" Then
                Me.lblPrimaryValue.Text = "Customer No"
            End If



            Me.ClassUpdatePnl.Update()

            ViewState("FileType") = Nothing
            ViewState("FileName") = Nothing
            ViewState("CSVName") = Nothing
            Session.Remove("FTLogInfo")
            Session.Remove("dtFTErrors")
            SetErrorsTable()
            Me.StartTime.SelectedDate = Now.Date
            'LoadYear()

        Else
            DocWindow.VisibleOnPageLoad = False
            dtErrors = Session("dtFTErrors")
        End If

    End Sub

    Sub DataTable2CSV(ByVal table As DataTable, ByVal filename As String, ByVal sepChar As String)
        Dim writer As System.IO.StreamWriter
        Try
            writer = New System.IO.StreamWriter(filename)

            ' first write a line with the columns name
            Dim sep As String = ""
            Dim builder As New System.Text.StringBuilder
            For Each col As DataColumn In table.Columns
                builder.Append(sep).Append(col.ColumnName)
                sep = sepChar
            Next
            writer.WriteLine(builder.ToString())

            ' then write all the rows
            For Each row As DataRow In table.Rows
                sep = ""
                builder = New System.Text.StringBuilder

                For Each col As DataColumn In table.Columns
                    builder.Append(sep).Append(row(col.ColumnName))
                    sep = sepChar
                Next
                writer.WriteLine(builder.ToString())
            Next
        Finally
            If Not writer Is Nothing Then writer.Close()
        End Try
    End Sub

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

                Dim cmd As New OleDbCommand("SELECT * FROM [Sheet1$] WHERE SalesRepNo IS NOT NULL", oledbConn)

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

                Dim cmd As New OleDbCommand("SELECT * FROM [Sheet1$] WHERE SalesRepNo IS NOT NULL", oledbConn)

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

    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub btnImportSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImport.Click
        If Me.ExcelFileUpload.FileName = Nothing Then
            ' Me.lblinfo.Text = "Validation"
            Me.lblUpMsg.Text = "Select filename "
            ' Me.lblMessage.ForeColor = Drawing.Color.Green
            ' Me.MpInfoError.Show()
            Me.DocWindow.VisibleOnPageLoad = True
            Exit Sub
        End If

        Dim Str As New StringBuilder
        dtErrors = Session("dtFTErrors")
        Dim TotSuccess As Integer = 0
        Dim TotFailed As Integer = 0
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

                        Dim dtDeletedFSR As New DataTable
                        If dtDeletedFSR.Rows.Count > 0 Then
                            dtDeletedFSR.Rows.Clear()
                        End If

                        Dim col1 As DataColumn

                        col1 = New DataColumn
                        col1.ColumnName = "SalesRepID"
                        col1.DataType = System.Type.GetType("System.String")
                        col1.ReadOnly = False
                        col1.Unique = False
                        dtDeletedFSR.Columns.Add(col1)

                        col1 = New DataColumn
                        col1.ColumnName = "Year"
                        col1.DataType = System.Type.GetType("System.String")
                        col1.ReadOnly = False
                        col1.Unique = False
                        dtDeletedFSR.Columns.Add(col1)


                        col1 = New DataColumn
                        col1.ColumnName = "Month"
                        col1.DataType = System.Type.GetType("System.String")
                        col1.ReadOnly = False
                        col1.Unique = False
                        dtDeletedFSR.Columns.Add(col1)


                        Dim col As DataColumn

                        col = New DataColumn
                        col.ColumnName = "SalesRepNo"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)

                        col = New DataColumn
                        col.ColumnName = "SecondaryClassification"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)



                        col = New DataColumn
                        col.ColumnName = "PrimaryClassification"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)


                        col = New DataColumn
                        col.ColumnName = "Year"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)


                        col = New DataColumn
                        col.ColumnName = "Month"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)

                        col = New DataColumn
                        col.ColumnName = "TargetValue"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)

                        col = New DataColumn
                        col.ColumnName = "TargetQty"
                        col.DataType = System.Type.GetType("System.String")
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

                        If TempTbl.Columns.Count = 7 Then

                            If Not (TempTbl.Columns(0).ColumnName = "SalesRepNo" And TempTbl.Columns(1).ColumnName = "SecondaryClassification" And TempTbl.Columns(2).ColumnName = "PrimaryClassification" And TempTbl.Columns(3).ColumnName = "Year" And TempTbl.Columns(4).ColumnName = "Month" And TempTbl.Columns(5).ColumnName = "TargetValue" And TempTbl.Columns(6).ColumnName = "TargetQty") Then
                                lblUpMsg.Text = "Please check the template columns are correct"

                                ' Me.MPEImport.Show()
                                Me.DocWindow.VisibleOnPageLoad = True
                                Exit Sub
                            End If
                        Else
                            lblUpMsg.Text = "Invalid Template"
                            '' lblMessage.ForeColor = Drawing.Color.Green
                            ' lblinfo.Text = "Information"
                            ' MpInfoError.Show()
                            'Me.MPEImport.Show()
                            Me.DocWindow.VisibleOnPageLoad = True
                            Exit Sub
                    End If
                        TempTbl.Columns.Add("IsValid", GetType(String))



                        If TempTbl.Rows.Count = 0 Then
                            lblUpMsg.Text = "There is no data in the uploaded file."
                            ' lblMessage.ForeColor = Drawing.Color.Green
                            'lblinfo.Text = "Information"
                            'MpInfoError.Show()
                            '  Me.MPEImport.Show()
                            Me.DocWindow.VisibleOnPageLoad = True
                            Exit Sub
                        End If

                        Dim RowNo As String = Nothing
                        ' Dim ColNo As String = Nothing
                        ' Dim ColumnName As String = Nothing
                        Dim ErrorText As String = Nothing
                        Dim OrgID As String = Nothing


                        Dim SecondaryClass As String = CType(Session.Item("CONTROL_PARAMS"), ControlParams).FSR_TARGET_TYPE
                        Dim ValueType As String = CType(Session.Item("CONTROL_PARAMS"), ControlParams).TARGET_VALUE_TYPE
                        Dim PrimaryClass As String = CType(Session.Item("CONTROL_PARAMS"), ControlParams).FSR_TARGET_PRIMARY

                        If TempTbl.Rows.Count > 0 Then



                            Dim idx As Integer

                            For idx = 0 To TempTbl.Rows.Count - 1

                                Dim SalesRepID As String = Nothing
                                Dim SecondClassValue As String = Nothing
                                Dim PrimaryClassValue As String = Nothing
                                Dim Year As String = Nothing
                                Dim Month As String = Nothing
                                Dim TargetValue As String = Nothing
                                Dim TargetQty As String = Nothing

                                OrgID = Me.ddOraganisation.SelectedValue


                                Dim isValidRow As Boolean = True



                                If TempTbl.Rows(idx)(0) Is DBNull.Value Or TempTbl.Rows(idx)(1) Is DBNull.Value Or TempTbl.Rows(idx)(3) Is DBNull.Value Or TempTbl.Rows(idx)(4) Is DBNull.Value Then
                                    TempTbl.Rows(idx)("IsValid") = "N"
                                    'TotFailed += 1
                                    Continue For
                                End If


                                SalesRepID = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, "0", TempTbl.Rows(idx)(0).ToString())
                                SecondClassValue = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, "0", TempTbl.Rows(idx)(1).ToString())
                                PrimaryClassValue = IIf(TempTbl.Rows(idx)(2) Is DBNull.Value, "0", TempTbl.Rows(idx)(2).ToString())
                                Year = IIf(TempTbl.Rows(idx)(3) Is DBNull.Value Or TempTbl.Rows(idx)(3).ToString() = "", "0", TempTbl.Rows(idx)(3).ToString())
                                Month = IIf(TempTbl.Rows(idx)(4) Is DBNull.Value Or TempTbl.Rows(idx)(4).ToString() = "", "0", TempTbl.Rows(idx)(4).ToString())
                                TargetValue = IIf(TempTbl.Rows(idx)(5) Is DBNull.Value Or TempTbl.Rows(idx)(5).ToString() = "", "0", TempTbl.Rows(idx)(5).ToString())
                                TargetQty = IIf(TempTbl.Rows(idx)(6) Is DBNull.Value Or TempTbl.Rows(idx)(6).ToString() = "", "0", TempTbl.Rows(idx)(6).ToString())

                                If SalesRepID = "0" Or SecondClassValue = "0" Or (PrimaryClassValue = "0" And PrimaryClass <> "None") Or Year = "0" Or Month = "0" Then
                                    TempTbl.Rows(idx)("IsValid") = "N"
                                    Continue For
                                End If


                                If objProduct.CheckValidFSRID(Err_No, Err_Desc, SalesRepID, OrgID) = False Then
                                    RowNo = idx + 2
                                    TempTbl.Rows(idx)("IsValid") = "N"
                                    ErrorText = "Invalid salesrep number"
                                    TotFailed += 1
                                    isValidRow = False
                                End If
                               


                                If obj.CheckAgencyOrCategory(Err_No, Err_Desc, SecondaryClass, SecondClassValue, SalesRepID) = False Then
                                    RowNo = idx + 2
                                    TempTbl.Rows(idx)("IsValid") = "N"
                                    If SecondaryClass = "A" Then
                                        ErrorText = "The agency does not belongs to the van"
                                    ElseIf SecondaryClass = "C" Then
                                        ErrorText = "The category does not belongs to the van"
                                    ElseIf SecondaryClass = "B" Then
                                        ErrorText = "The brand does not belongs to the van"
                                    ElseIf SecondaryClass = "P" Then
                                        ErrorText = "The product does not belongs to the van"
                                    End If
                                    TotFailed += 1
                                    isValidRow = False
                                End If
                                If PrimaryClass <> "None" Then
                                    If obj.CheckValidCustomer(Err_No, Err_Desc, PrimaryClassValue, SalesRepID, PrimaryClass) = False Then
                                        RowNo = idx + 2
                                        TempTbl.Rows(idx)("IsValid") = "N"
                                        If PrimaryClass = "Area" Then
                                            ErrorText = "The area does not belongs to the van"
                                        ElseIf PrimaryClass = "Customer" Then
                                            ErrorText = "The customer does not belongs to the van"
                                        End If
                                        TotFailed += 1
                                        isValidRow = False
                                    End If
                                End If
                                If IsNumeric(Month) And IsNumeric(Year) Then
                                    If Len(Year) = 4 And Month > 0 And Month <= 12 And (Len(Month) = 1 Or Len(Month) = 2) Then
                                        Dim CurrentDate As Date = DateTime.Parse(Now.Year.ToString() & "-" & Now.Month.ToString() & "-01")
                                        Dim ImpDate As Date = DateTime.Parse(Year.ToString() & "-" & Month.ToString() & "-01")
                                        If Not ImpDate >= CurrentDate Then
                                            RowNo = idx + 2
                                            TempTbl.Rows(idx)("IsValid") = "N"
                                            ErrorText = "Year and month should be greater than or equal to current year and month"
                                            TotFailed += 1
                                            isValidRow = False
                                        End If
                                    Else
                                        RowNo = idx + 2
                                        TempTbl.Rows(idx)("IsValid") = "N"
                                        ErrorText = "Year and month should be in yyyy and MM format"
                                        TotFailed += 1
                                        isValidRow = False
                                    End If

                                Else
                                    RowNo = idx + 2
                                    TempTbl.Rows(idx)("IsValid") = "N"
                                    ErrorText = "Month and year should be in numeric"
                                    TotFailed += 1
                                    isValidRow = False
                                End If

                                If IsNumeric(TargetValue) = False Or IsNumeric(TargetQty) = False Then
                                    RowNo = idx + 2
                                    TempTbl.Rows(idx)("IsValid") = "N"
                                    ErrorText = "Target value & quantity should be in numeric"
                                    TotFailed += 1
                                    isValidRow = False
                                End If

                                If ValueType = "B" Then
                                    If TargetValue = "0" Or TargetQty = "0" Then
                                        RowNo = idx + 2
                                        TempTbl.Rows(idx)("IsValid") = "N"
                                        ErrorText = "Target value & quantity should not be zero"
                                        TotFailed += 1
                                        isValidRow = False
                                    End If
                                End If

                                If ValueType = "V" Then
                                    If TargetValue = "0" Then
                                        RowNo = idx + 2
                                        TempTbl.Rows(idx)("IsValid") = "N"
                                        ErrorText = "Target value should not be zero"
                                        TotFailed += 1
                                        isValidRow = False
                                    End If
                                End If


                                If ValueType = "Q" Then
                                    If TargetQty = "0" Then
                                        RowNo = idx + 2
                                        TempTbl.Rows(idx)("IsValid") = "N"
                                        ErrorText = "Target quantity should not be zero"
                                        TotFailed += 1
                                        isValidRow = False
                                    End If
                                End If



                                If Not (RowNo Is Nothing And ErrorText Is Nothing) Then
                                    Dim h As DataRow = dtErrors.NewRow()
                                    h("RowNo") = RowNo
                                    ' h("ColNo") = ColNo
                                    ' h("ColName") = ColumnName
                                    h("LogInfo") = ErrorText
                                    dtErrors.Rows.Add(h)
                                    RowNo = Nothing
                                    'ColNo = Nothing
                                    'ColumnName = Nothing
                                    ErrorText = Nothing
                                    isValidRow = False
                                End If

                                If isValidRow = True Then
                                    TempTbl.Rows(idx)("IsValid") = "Y"
                                    TempTbl.Rows(idx)("TargetValue") = Math.Round(CDec(TargetValue), 2)
                                    TempTbl.Rows(idx)("TargetQty") = Math.Round(CDec(TargetQty), 2)
                                    TotSuccess = TotSuccess + 1
                                    Dim h As DataRow = dtErrors.NewRow()
                                    h("RowNo") = idx + 2
                                    h("LogInfo") = "Successfully uploaded"
                                    dtErrors.Rows.Add(h)
                                    RowNo = Nothing
                                    ErrorText = Nothing
                                    isValidRow = True

                                    Dim bExist As Boolean = False

                                    For Each t As DataRow In dtDeletedFSR.Rows
                                        If t("SalesRepID").ToString() = SalesRepID And t("Year").ToString() = Year And t("Month").ToString() = Month Then
                                            bExist = True
                                            Exit For
                                        End If
                                    Next

                                    If bExist = False Then
                                        Dim n As DataRow = dtDeletedFSR.NewRow()
                                        n("SalesRepID") = SalesRepID
                                        n("Year") = Year
                                        n("Month") = Month
                                        dtDeletedFSR.Rows.Add(n)
                                    End If

                                End If


                            Next
                        End If

                        If obj.UploadFSRSalesTarget(TempTbl, dtDeletedFSR, SecondaryClass, ValueType, Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID) = True Then
                            DeleteExcel()
                            lblUpMsg.Text = IIf(TotFailed = 0, "Successfully uploaded", "One or more rows has invalid data. Please check the log")

                            'MPEImport.Show()
                            Me.DocWindow.VisibleOnPageLoad = True
                        Else
                            DeleteExcel()
                            lblUpMsg.Text = "Please check the uploaded log"

                            ' MPEImport.Show()
                            Me.DocWindow.VisibleOnPageLoad = True
                            Exit Sub
                        End If
                    End If


                    Me.dgvErros.DataSource = dtErrors
                    Me.dgvErros.DataBind()
                    Session.Remove("dtFTErrors")
                    Session("dtFTErrors") = dtErrors


                    Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & "FTL_" & Now.ToString("yyyyMMdd") + ".txt"
                    DataTable2CSV(dtErrors, fn, vbTab)

                    Session.Remove("FTLogInfo")
                    Session("FTLogInfo") = fn




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

                MessageBoxValidation("Please import valid Excel template.", "Validation")
            End If


        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try

    End Sub

    Protected Sub lbLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbLog.Click
        If Not Session("FTLogInfo") Is Nothing Then
            Dim fileValue As String = Session("FTLogInfo")





            Dim file As System.IO.FileInfo = New FileInfo(fileValue)

            If file.Exists Then
                Dim filePath As String = fileValue
                Response.ContentType = ContentType
                Response.AppendHeader("Content-Disposition", ("attachment; filename=" + Path.GetFileName(file.Name)))
                Response.WriteFile(filePath)
                Response.End()

                'Response.Clear()

                'Response.AddHeader("Content-Disposition", "attachment; filename=" & file.Name)

                'Response.AddHeader("Content-Length", file.Length.ToString())

                'Response.WriteFile(file.FullName)


                'Response.[End]()
            Else
                lblUpMsg.Text = "File does not exist"
                'lblMessage.ForeColor = Drawing.Color.Green
                'lblinfo.Text = "Information"
                DocWindow.VisibleOnPageLoad = True
                Exit Sub

            End If

        Else
            lblUpMsg.Text = "There is no log to show."
            'lblMessage.ForeColor = Drawing.Color.Green
            'lblinfo.Text = "Information"
            DocWindow.VisibleOnPageLoad = True
            Exit Sub

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
        col.ColumnName = "LogInfo"
        col.DataType = System.Type.[GetType]("System.String")
        col.[ReadOnly] = False
        col.Unique = False
        dtErrors.Columns.Add(col)

        Session.Remove("dtFTErrors")
        Session("dtFTErrors") = dtErrors
    End Sub
    Sub LoadFSR()
        Dim objCommon As New SalesWorx.BO.Common.Common
        ddlSalesRep.ClearSelection()
        ddlSalesRep.Items.Clear()
        ddlSalesRep.Text = ""
        ddlSalesRep.DataSource = objCommon.GetVanByOrg(Err_No, Err_Desc, IIf(ddOraganisation.SelectedIndex < 0, "0", ddOraganisation.SelectedValue), CType(Session("User_Access"), UserAccess).UserID)

        ddlSalesRep.DataValueField = "SalesRep_Id"
        ddlSalesRep.DataTextField = "SalesRep_Name"
        ddlSalesRep.DataBind()
        ddlSalesRep.Items.Insert(0, New RadComboBoxItem("-- Select --", "-1"))
        If ddlSalesRep.Items.Count = 2 Then
            ddlSalesRep.SelectedIndex = 1
        End If

        objCommon = Nothing
    End Sub






    Protected Sub btnImportWindow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportWindow.Click
        If Me.ddOraganisation.SelectedIndex > 0 Then
            Me.lblUpMsg.Text = ""
            Me.DocWindow.VisibleOnPageLoad = True
        Else
            MessageBoxValidation("Please select organization.", "Validation")
            Exit Sub
        End If
    End Sub
    Protected Sub Export_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        Dim objTarget As New SalesTarget
        If Me.ddOraganisation.SelectedIndex > 0 Then
            Dim dtOriginal As New DataTable()
            Dim OrgID As String = Me.ddOraganisation.SelectedValue

            Dim TargetMonth As DateTime = Me.StartTime.SelectedDate.Value
            dtOriginal = objTarget.ExportFSRTargetTemplate(Err_No, Err_Desc, OrgID, IIf(Me.ddlSalesRep.SelectedIndex <= 0, "0", Me.ddlSalesRep.SelectedValue), TargetMonth)
            'Return Table consisting data
            'Create Tempory Table

            Dim dtTemp As New DataTable()

            'Creating Header Row
            dtTemp.Columns.Add("SalesRepNo")

            dtTemp.Columns.Add("SecondaryClassification")

            dtTemp.Columns.Add("PrimaryClassification")
            dtTemp.Columns.Add("Year")
            dtTemp.Columns.Add("Month")
            dtTemp.Columns.Add("TargetValue")
            dtTemp.Columns.Add("TargetQty")



            Dim drAddItem As DataRow
            For i As Integer = 0 To dtOriginal.Rows.Count - 1
                drAddItem = dtTemp.NewRow()
                drAddItem(0) = dtOriginal.Rows(i)("SalesRepNo").ToString()

                drAddItem(1) = dtOriginal.Rows(i)("SecondaryClassification").ToString()

                drAddItem(2) = dtOriginal.Rows(i)("PrimaryClassification").ToString()
                drAddItem(3) = dtOriginal.Rows(i)("Year").ToString()
                drAddItem(4) = dtOriginal.Rows(i)("Month").ToString()
                drAddItem(5) = dtOriginal.Rows(i)("TargetValue").ToString()
                drAddItem(6) = dtOriginal.Rows(i)("TargetQty").ToString()
                dtTemp.Rows.Add(drAddItem)
            Next

            If dtOriginal.Rows.Count = 0 Then
                MessageBoxValidation("There is no data for the selected filter criteria", "Validation")
                Exit Sub
                drAddItem = dtTemp.NewRow()
                drAddItem(0) = ""
                drAddItem(1) = ""
                drAddItem(2) = ""
                drAddItem(3) = ""
                drAddItem(4) = ""
                drAddItem(5) = ""
                drAddItem(6) = ""

                dtTemp.Rows.Add(drAddItem)
            End If


            Dim dg As New DataGrid()
            dg.DataSource = dtTemp
            dg.DataBind()
            If dtTemp.Rows.Count > 0 Then

                Dim fn As String = "FSRTarget" + "_" + Now.ToString("ddMMMyyHHmmss") + ".xls"
                Dim d As New DataSet
                d.Tables.Add(dtTemp)

                ExportToExcel(fn, d)

            End If
        Else
            MessageBoxValidation("Please select organization,FSR & Target month year.", "Validation")
            Exit Sub
        End If


    End Sub


    Public Function WriteXLSFile(ByVal pFileName As String, ByVal pDataSet As DataSet) As Boolean
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
            If Not pDataSet Is Nothing And pDataSet.Tables.Count > 0 Then

                'Traverse DataTable inside the DataSet
                For Each dt As DataTable In pDataSet.Tables

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
                Next
            End If

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
    'Private Sub ExportToExcel(ByVal strFileName As String, ByVal ds As DataSet)


    '    Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & strFileName
    '    WriteXLSFile(fn, ds)

    '    Dim myfile As New FileInfo(fn)

    '    Response.ClearContent()
    '    Response.Buffer = True
    '    Response.AddHeader("content-disposition", "attachment;filename=" & strFileName)
    '    Response.AddHeader("Content-Length", fn.Length.ToString())
    '    Response.ContentType = "application/vnd.ms-excel"
    '    Response.TransmitFile(myfile.FullName)
    '    Response.End()

    'End Sub
    Private Sub ExportToExcel(ByVal strFileName As String, ByVal ds As DataSet)


        Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & strFileName
        WriteXLSFile(fn, ds)

        Dim sFileName As String = strFileName
        Dim sFullPath As String = fn
        Dim fileBytes As Byte() = System.IO.File.ReadAllBytes(sFullPath)

        Dim context As System.Web.HttpContext = System.Web.HttpContext.Current
        context.Response.ContentType = "application/vnd.ms-excel"
        context.Response.AddHeader("Content-Disposition", "attachment;filename=" + sFileName)
        context.Response.Clear()
        context.Response.BinaryWrite(fileBytes)
        context.Response.Flush()
        context.ApplicationInstance.CompleteRequest()


    End Sub

    Private Sub ddOraganisation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddOraganisation.SelectedIndexChanged

        LoadFSR()


    End Sub

















End Class