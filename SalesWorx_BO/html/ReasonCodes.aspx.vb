Imports SalesWorx.BO.Common
Imports System.Data
Imports System.Web
Imports System.Resources
Imports System.Web.UI.WebControls
Imports log4net
Imports System.IO
Imports System.Data.OleDb
Imports System.Data.SqlClient


Partial Public Class ReasonCodes
    Inherits System.Web.UI.Page
    Dim objReason As New ReasonCode
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim Dt As New DataTable
    Private Const PageID As String = "P84"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

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

            Dt = objReason.FillReasonCode(Err_No, Err_Desc)
            BindReasonCodeData()
            'ddl_Purpose.Items.Clear()
            'ddl_Purpose.Items.Add("--Select--")
            'ddl_Purpose.Items(0).Value = ""
            'Dim cls As New Purpose
            'ddl_Purpose.DataSource = cls.BindToEnum
            'ddl_Purpose.DataTextField = "Key"
            'ddl_Purpose.DataValueField = "Value"
            'ddl_Purpose.DataBind()

            'BindReasonCodeData()
            'ddl_Purpose.Items.Clear()
            'ddl_Purpose.Items.Add("--Select--")
            'ddl_Purpose.Items(0).Value = ""
            'ddl_Purpose.DataSource = cls.BindToEnum
            'ddl_Purpose.DataTextField = "Key"
            'ddl_Purpose.DataValueField = "Value"
            'ddl_Purpose.DataBind()
            'ddl_Purpose.Items.Clear()
            'ddl_Purpose.DataSource = (New SalesWorx.BO.Common.AppControl).LoadReturnPurpose(Err_No, Err_Desc, "RETURN_PURPOSE")
            'ddl_Purpose.DataTextField = "Code_Description"
            'ddl_Purpose.DataValueField = "Code_Value"
            'ddl_Purpose.DataBind()

            'Resetfields()
            ViewState("FileType") = Nothing
            ViewState("FileName") = Nothing
            ViewState("CSVName") = Nothing
        Else
            MPEDetails.VisibleOnPageLoad = False
        End If

    End Sub


    Public Sub Resetfields()

        ddl_Purpose.Items.Clear()
        ddl_Purpose.DataSource = (New SalesWorx.BO.Common.AppControl).LoadReturnPurpose(Err_No, Err_Desc, "RETURN_PURPOSE")
        ddl_Purpose.DataTextField = "Code_Description"
        ddl_Purpose.DataValueField = "Code_Value"
        ddl_Purpose.DataBind()

        Me.txtReasonCode.Text = ""
        Me.txtDescription.Text = ""

        Me.ddFilterBy.SelectedIndex = 0
        Me.txtReasonCode.Enabled = True
        Me.rbAppend.Checked = False
        Me.rbRebuild.Checked = False
        'Me.ddl_Purpose.SelectedIndex = 0
        'Me.btnAdd.Focus()
        ddFilterBy.Focus()
        ClassUpdatePnl.Update()
    End Sub


    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Me.txtReasonCode.Text = "" Or Me.ddl_purpose.SelectedItem.Text = "--Select--" Or Me.txtDescription.Text = "" Then
            lblPop.Text = "Reason code,description and purpose are required."
            MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If
        Dim success As Boolean = False

        Try

            objReason.ReasonCode = IIf(Me.txtReasonCode.Text = "", "0", Me.txtReasonCode.Text)
            objReason.Description = IIf(Me.txtDescription.Text = "", "0", Me.txtDescription.Text)
            objReason.Purpose = Me.ddl_Purpose.SelectedItem.Text

            If objReason.CheckDuplicate(Err_No, Err_Desc) = False Then
                If objReason.InsertReasonCode(Err_No, Err_Desc) = True Then
                    success = True
                    MessageBoxValidation("Successfully saved.", "Information")

                End If
            Else
                Me.lblPop.Text = "Record already exist."
                Me.MPEDetails.VisibleOnPageLoad = True
                Exit Sub
            End If
            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "REASON CODE", IIf(Me.txtReasonCode.Text = "", "0", Me.txtReasonCode.Text), "Code: " & Me.txtReasonCode.Text & "/ Desc :  " & Me.txtDescription.Text & "/ Purpose:  " & Me.ddl_Purpose.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                Dt = objReason.FillReasonCode(Err_No, Err_Desc)
                BindReasonCodeData()
                Resetfields()
                Me.MPEDetails.VisibleOnPageLoad = False
                ClassUpdatePnl.Update()
            Else

                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_ReasonCode_001") & "&next=ReasonCodes.aspx&Title=Message", False)
                Exit Try
            End If

        Catch ex As Exception
            Err_No = "74071"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Private Sub BindReasonCodeData()
        Me.gvReasonCode.DataSource = Dt
        Me.gvReasonCode.DataBind()

        Dim dv As New DataView(Dt)
        If ViewState("SortField") <> "" Then
            dv.Sort = (ViewState("SortField") & " ") + SortDirection
        End If
        gvReasonCode.DataSource = dv
        gvReasonCode.DataBind()
        Session.Remove("ReasonCode")
        Session("ReasonCode") = Dt
    End Sub


    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If Me.txtReasonCode.Text = "" Or Me.ddl_Purpose.SelectedItem.Text = "--Select--" Or Me.txtDescription.Text = "" Then
            Me.lblPop.Text = "Reason code,description and purpose are required."
            Me.MPEDetails.VisibleOnPageLoad = True
            Exit Sub
        End If
        Dim success As Boolean = False
        Try

            objReason.ReasonCode = IIf(Me.txtReasonCode.Text = "", "0", Me.txtReasonCode.Text)
            objReason.Description = IIf(Me.txtDescription.Text = "", "0", Me.txtDescription.Text)
            objReason.Purpose = Me.ddl_Purpose.SelectedItem.Text


            If objReason.UpdateReasonCode(Err_No, Err_Desc) = True Then
                success = True
                MessageBoxValidation("Successfully updated.", "Information")
            End If

            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "U", "MAIN MENU", "REASON CODE", IIf(Me.txtReasonCode.Text = "", "0", Me.txtReasonCode.Text), "Code: " & Me.txtReasonCode.Text & "/ Desc :  " & Me.txtDescription.Text & "/ Purpose:  " & Me.ddl_Purpose.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                Dt = objReason.FillReasonCode(Err_No, Err_Desc)
                BindReasonCodeData()
                Resetfields()
                Me.MPEDetails.VisibleOnPageLoad = False
                ClassUpdatePnl.Update()
            Else

                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_ReasonCode_002") & "&next=ReasonCodes.aspx&Title=Message", False)
                Exit Try
            End If

        Catch ex As Exception
            Err_No = "74072"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Try
            Me.MPEDetails.VisibleOnPageLoad = False
            Resetfields()
            'ClassUpdatePnl.Update()

        Catch

        End Try
    End Sub
    Protected Sub btnDeleteAll_Click()
        Try
            Dim dr As GridViewRow
            Dim Success As Boolean = False
            Dim Selected As Boolean = False

            For Each dr In gvReasonCode.Rows
                Dim RowCheckBox As System.Web.UI.WebControls.CheckBox = dr.FindControl("chkDelete")

                If RowCheckBox.Checked = True Then
                    Selected = True
                    Dim Lbl As System.Web.UI.WebControls.Label = dr.FindControl("lblReason")
                    HidVal.Value = Lbl.Text
                    objReason.ReasonCode = dr.Cells(1).Text
                    If objReason.DeleteReasonCode(Err_No, Err_Desc) = True Then
                        objLogin.SaveUserLog(Err_No, Err_Desc, "D", "MAIN MENU", "REASON CODE", dr.Cells(1).Text, "Code: " & dr.Cells(1).Text & "/ Desc :  " & dr.Cells(2).Text & "/ Purpose:  " & dr.Cells(3).Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                        Success = True
                    End If
                End If
            Next
            If Selected = False Then
                MessageBoxValidation("Select at least one reason code .", "Information")
                Exit Sub
            End If

            If (Success = True) Then
                MessageBoxValidation("Reason code(s) deleted successfully.", "Information")
                Dt = objReason.FillReasonCode(Err_No, Err_Desc)
                BindReasonCodeData()
                Resetfields()
            Else
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_ReasonCode_003") & "&next=ReasonCodes.aspx&Title=Message", False)
                Exit Try
            End If
        Catch ex As Exception
            Err_No = "74073"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Dim success As Boolean = False
        Try
            Dt = objReason.SearchReasonCode(Err_No, Err_Desc, Me.ddFilterBy.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text))
            BindReasonCodeData()
            ClassUpdatePnl.Update()
            success = True
            If success = False Then
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_ReasonCode_004") & "&next=ReasonCodes.aspx&Title=Message", False)
                Exit Try
            End If
        Catch ex As Exception
            Err_No = "74074"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btndelete As ImageButton = TryCast(sender, ImageButton)
        Dim row As GridViewRow = DirectCast(btndelete.NamingContainer, GridViewRow)

        Dim success As Boolean = False
        objReason.ReasonCode = row.Cells(1).Text
        HidVal.Value = row.Cells(1).Text
        Try

            If objReason.DeleteReasonCode(Err_No, Err_Desc) = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "D", "MAIN MENU", "REASON CODE", row.Cells(1).Text, "Code: " & row.Cells(1).Text & "- Desc :  " & row.Cells(2).Text & "- Purpose:  " & row.Cells(3).Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                success = True
            End If

            If success = True Then
                MessageBoxValidation("Successfully deleted.", "Information")
                Dt = objReason.FillReasonCode(Err_No, Err_Desc)
                BindReasonCodeData()
                Resetfields()
            Else
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_ReasonCode_005") & "&next=ReasonCodes.aspx&Title=Message", False)
                Exit Try
            End If

        Catch ex As Exception
            Err_No = "74075"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub


    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click

        btnUpdate.Visible = False
        btnSave.Visible = True
        Resetfields()
        ClassUpdatePnl.Update()
        MPEDetails.VisibleOnPageLoad = True
    End Sub



    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            lblPop.Text = ""
            btnUpdate.Visible = True
            btnSave.Visible = False
            'Resetfields()
            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
            HidVal.Value = row.Cells(1).Text
            txtReasonCode.Text = Trim(row.Cells(1).Text)
            txtDescription.Text = Trim(row.Cells(2).Text)
            If row.Cells(3).Text = "0" Then
                Me.ddl_Purpose.SelectedItem.Text = "--Select--"
            Else
                Me.ddl_Purpose.SelectedItem.Text = row.Cells(3).Text
            End If
            Me.txtReasonCode.Enabled = False
            ClassUpdatePnl.Update()
            MPEDetails.VisibleOnPageLoad = True
        Catch ex As Exception
            Err_No = "74076"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_ReasonCode_006") & "&next=ReasonCodes.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub

    Private Sub gvReasonCode_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvReasonCode.PageIndexChanging
        gvReasonCode.PageIndex = e.NewPageIndex
        Dt = objReason.FillReasonCode(Err_No, Err_Desc)
        BindReasonCodeData()

    End Sub

    Private Sub gvReasonCode_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvReasonCode.Sorting
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        Dt = objReason.FillReasonCode(Err_No, Err_Desc)
        BindReasonCodeData()
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


    Protected Sub ddFilterBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddFilterBy.SelectedIndexChanged
        If Me.ddFilterBy.SelectedIndex <= 0 Then
            Me.txtFilterVal.Text = ""
        End If
    End Sub

    Protected Sub btnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImport.Click
        Me.MPEImport.Show()
        Resetfields()
    End Sub

    Protected Sub btnImportSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportSave.Click
        If Me.ExcelFileUpload.FileName = Nothing Or Me.ddl_Purpose.SelectedIndex <= 0 Or (Me.rbAppend.Checked = False And Me.rbRebuild.Checked = False) Then
            MessageBoxValidation("Select filename,purpose and rebuild/append data", "Validation")
            'Me.MPEImport.Show()
            Exit Sub
        End If

        Dim Str As New StringBuilder

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
                Dim strScript As String
                strScript = "<script language='javascript'>"
                strScript += "document.aspnetForm('ctl00_ContentPlaceHolder1_DummyImBtn').click();"
                strScript += "</script>"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "StrScript", _
      strScript, False)
            Else
                Str.Append("<span style='font-color:red'> <b >Please import valid Excel template.</b></span>")
                MessageBoxValidation("Please import valid Excel template.", "Validation")
                'MpInfoError.Show()
            End If


        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try

    End Sub

    Protected Sub DummyImBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles DummyImBtn.Click
        Try
            Dim st As Boolean = False

            If ViewState("FileType") IsNot Nothing And ViewState("FileName") IsNot Nothing Then
                Dim TempTbl As New DataTable
                If TempTbl.Rows.Count > 0 Then
                    TempTbl.Rows.Clear()
                End If
                Dim col As DataColumn
                col = New DataColumn()
                col.ColumnName = "Reason_Code"
                col.DataType = System.Type.GetType("System.String")
                col.ReadOnly = False
                col.Unique = False
                TempTbl.Columns.Add(col)

                col = New DataColumn
                col.ColumnName = "Description"
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
                If TempTbl.Rows.Count = 0 Then
                    MessageBoxValidation("There is no data in your file.", "Validation")
                    Exit Sub
                End If
                If TempTbl.Rows.Count > 0 Then
                    Dim idx As Integer
                    If Me.rbAppend.Checked = True Then
                        For idx = 0 To TempTbl.Rows.Count - 1
                            objReason.ReasonCode = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, "0", TempTbl.Rows(idx)(0).ToString())
                            objReason.Description = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, "0", TempTbl.Rows(idx)(1).ToString())
                            objReason.Purpose = Me.ddl_Purpose.SelectedItem.Text

                            If objReason.CheckDuplicate(Err_No, Err_Desc) = False Then
                                If objReason.ReasonCode <> "0" Then
                                    If objReason.InsertReasonCode(Err_No, Err_Desc) = True Then
                                        st = True
                                    End If
                                End If
                            Else
                                If objReason.ReasonCode <> "0" Then
                                    If objReason.UpdateReasonCode(Err_No, Err_Desc) = True Then
                                        st = True
                                    End If
                                End If
                            End If
                        Next
                    ElseIf Me.rbRebuild.Checked = True Then
                        objReason.RebuildAllReasonCode(Err_No, Err_Desc)
                        For idx = 0 To TempTbl.Rows.Count - 1
                            objReason.ReasonCode = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, "0", TempTbl.Rows(idx)(0).ToString())
                            objReason.Description = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, "0", TempTbl.Rows(idx)(1).ToString())
                            objReason.Purpose = Me.ddl_Purpose.SelectedItem.Text
                            If objReason.ReasonCode <> "0" Then
                                If objReason.InsertReasonCode(Err_No, Err_Desc) = True Then
                                    st = True
                                End If
                            End If

                        Next
                    End If
                End If
                If st = True Then
                    DeleteExcel()
                    Me.MPEImport.Hide()
                    MessageBoxValidation("Successfully imported.", "Information")
                    Dt = objReason.FillReasonCode(Err_No, Err_Desc)
                    BindReasonCodeData()
                    ClassUpdatePnl.Update()
                    Resetfields()
                Else
                    log.Error(Err_Desc)
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_ReasonCode_006") & "&next=ReasonCodes.aspx&Title=Message", False)
                    Exit Try
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