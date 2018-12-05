
Imports SalesWorx.BO.Common

Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports ExcelLibrary.SpreadSheet

Partial Public Class DiscountDefinition
    Inherits System.Web.UI.Page
    Dim objProduct As New Product

    Private dtErrors As New DataTable
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const PageID As String = "P266"
    Private dt As New DataTable
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objControl As New AppControl


    Private Sub DiscountDefinition_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Discount Definition"
    End Sub




    Protected Sub ddl_org_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_org.SelectedIndexChanged


        ResetDetails()
        LoadBonusType()
        FillItemsList()
        BindData()

    End Sub

    Private Sub ClearSelectdRowBackColor()
        For Each gvr As GridViewRow In dgvItems.Rows
            If (gvr.RowType = DataControlRowType.DataRow) Then
                If gvr.BackColor = Drawing.Color.LightGoldenrodYellow Then
                    gvr.BackColor = Drawing.Color.White
                End If
            End If
        Next
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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


                LoadOrgHeads()
                LoadBonusType()
                ResetDetails()
                FillItemsList()
                BindData()

                ViewState("FileType") = Nothing
                ViewState("FileName") = Nothing
                ViewState("CSVName") = Nothing
                Session.Remove("DiscountLogInfo")
                Session.Remove("dtDiscountErrors")
                SetErrorsTable()
            Else
                dtErrors = Session("dtDiscountErrors")

            End If


        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_005") & "&next=Welcome.aspx&Title=Discount Definition", False)
        End Try
    End Sub
 
    Sub FillItemsList()
        Dim x As New DataTable
        x = objProduct.GetProductListByOrgID(Err_No, Err_Desc, IIf(Me.ddl_org.SelectedIndex <= 0, "0", Me.ddl_org.SelectedValue))

        Me.ddlItem.ClearSelection()
        Me.ddlItem.Items.Clear()
        Me.ddlItem.Text = ""

        ddlItem.DataTextField = "Description"
        ddlItem.DataValueField = "Item_Code"
        ddlItem.DataSource = x
        ddlItem.DataBind()

    End Sub
    Sub LoadOrgHeads()
        Dim objCommon As New Common
        Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
        ddl_org.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
        ddl_org.Items.Clear()
        ddl_org.Items.Add("-- Select a Organization --")
        ddl_org.AppendDataBoundItems = True
        ddl_org.DataValueField = "MAS_Org_ID"
        ddl_org.DataTextField = "Description"
        ddl_org.DataBind()

    End Sub

    Private Sub dgvItems_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgvItems.RowCommand

        Try

            If (e.CommandName = "EditRecord") Then

                LoadBonusType()

                Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
                ClearSelectdRowBackColor()
                row.BackColor = Drawing.Color.LightGoldenrodYellow
                Me.lblLineID.Text = Convert.ToInt32(dgvItems.DataKeys(row.RowIndex).Value)
                Dim lblDisType As Label = DirectCast(row.FindControl("lblDType"), Label)

                Me.ddlItem.SelectedValue = row.Cells(0).Text
                Me.ddlType.SelectedValue = lblDisType.Text
                Me.txtFromQty.Text = row.Cells(4).Text
                Me.txtDisValue.Text = row.Cells(5).Text
                Me.StartTime.SelectedDate = DateTime.Parse(row.Cells(6).Text)
                Me.EndTime.SelectedDate = DateTime.Parse(row.Cells(7).Text)


                If Me.StartTime.SelectedDate.Value <= Now.Date Then
                    Me.StartTime.Enabled = False
                Else
                    Me.StartTime.Enabled = True
                End If

                Me.ddlItem.Enabled = False
                Me.btnAddItems.Text = "Update"
                UpdatePanel1.Update()
            End If


            If (e.CommandName = "DeleteRecord") Then



                Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
                Me.lblLineID.Text = Convert.ToInt32(dgvItems.DataKeys(row.RowIndex).Value)
                objProduct.DeleteDiscountData(Err_No, Err_Desc, Me.lblLineID.Text)

                Me.btnAddItems.Text = "Add"
                Me.txtFromQty.Text = ""
                Me.ddlType.SelectedIndex = 0
                Me.txtDisValue.Text = ""
                Me.StartTime.Enabled = True
                Me.EndTime.Enabled = True
                Me.ddlItem.Enabled = True
                Me.lblLineID.Text = ""
                LoadBonusType()
                BindData()
                UpdatePanel1.Update()

            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("information.aspx?mode=1&errno=" & "74071" & "&msg=" & AppMsgHandler.GetErrorMessage("SWX_BO_Msg_005") & "&next=DiscountDefinition.aspx", False)
        Finally
        End Try
    End Sub









    Private Sub BindData()

        Dim x As New DataTable
        x = objProduct.GetDiscountData(Err_No, Err_Desc, IIf(Me.ddl_org.SelectedIndex <= 0, "0", Me.ddl_org.SelectedValue))

        If Me.ddlItem.SelectedIndex > 0 Then
            x.DefaultView.RowFilter = "(Item_Code = '" + Me.ddlItem.SelectedValue + "')"
        End If

        Dim dv As New DataView(x)
        dv = x.DefaultView
        If ViewState("SortField") <> "" Then
            dv.Sort = (ViewState("SortField") & " ") + SortDirection
        End If
        dgvItems.DataSource = dv
        dgvItems.DataBind()

        Me.dgvItems.DataSource = Nothing
        Me.dgvItems.DataSource = dv
        Me.dgvItems.DataBind()

    End Sub
    Public Sub LoadBonusType()
        Dim dtBType As New DataTable
        dtBType.Columns.Add("Code")
        dtBType.Columns.Add("Description")
        Dim dr As DataRow = dtBType.NewRow()
        dr("Code") = "V"
        dr("Description") = "VALUE"
        dtBType.Rows.InsertAt(dr, 0)

        dr = dtBType.NewRow()
        dr("Code") = "P"
        dr("Description") = "PERCENTAGE"
        dtBType.Rows.Add(dr)




        Me.ddlType.DataSource = dtBType
        Me.ddlType.DataTextField = "Description"
        Me.ddlType.DataValueField = "Code"
        Me.ddlType.DataBind()
        Me.ddlType.SelectedIndex = 0

    End Sub
    Private Sub dgvItems_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgvItems.PageIndexChanging
        dgvItems.PageIndex = e.NewPageIndex

        BindData()

    End Sub

    Private Sub dgvItems_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgvItems.RowDataBound

    End Sub
    Private Sub dgvItems_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgvItems.Sorting
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"

        BindData()
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












    Protected Sub btnAddItems_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddItems.Click
        If ValidationDetails() = False Then
            Try





                Err_No = Nothing
                Err_Desc = Nothing
                Dim success As Boolean
                Dim fdate As String = Me.StartTime.SelectedDate.Value.ToString("dd-MM-yyyy")
                Dim tdate As String = Me.EndTime.SelectedDate.Value.ToString("dd-MM-yyyy")

                Dim ItemCode As String = Me.ddlItem.SelectedValue


                'Check Item exist
                If objProduct.CheckDiscountDataActiveRange(Err_No, Err_Desc, ItemCode, CLng(IIf(Me.txtFromQty.Text = "", "0", Me.txtFromQty.Text)), Me.ddl_org.SelectedValue, Me.StartTime.SelectedDate.Value, Me.EndTime.SelectedDate.Value, CInt(IIf(Me.lblLineID.Text = "", "0", Me.lblLineID.Text))) = True Then

                    Me.lblMessage.Text = "Discount already define for this item with same slab"
                    lblMessage.ForeColor = Drawing.Color.Red
                    lblinfo.Text = "Confirmation"
                    MpInfoError.Show()
                    btnClose.Focus()
                    Exit Sub

                End If

                If Me.btnAddItems.Text = "Add" Then
                    success = objProduct.SaveDiscountData(Err_No, Err_Desc, ItemCode, Me.ddl_org.SelectedValue, Me.ddlType.SelectedItem.Value, CLng(IIf(Me.txtFromQty.Text = "", "0", Me.txtFromQty.Text)), CDec(IIf(Me.txtDisValue.Text = "", "0", Me.txtDisValue.Text)), Me.StartTime.SelectedDate.Value, Me.EndTime.SelectedDate.Value)

                ElseIf Me.btnAddItems.Text = "Update" Then
                    success = objProduct.UpdateDiscountData(Err_No, Err_Desc, Me.lblLineID.Text, ItemCode, Me.ddl_org.SelectedValue, Me.ddlType.SelectedItem.Value, CLng(IIf(Me.txtFromQty.Text = "", "0", Me.txtFromQty.Text)), CDec(IIf(Me.txtDisValue.Text = "", "0", Me.txtDisValue.Text)), Me.StartTime.SelectedDate.Value, Me.EndTime.SelectedDate.Value)
                End If
                If success = True Then
                    Me.lblMessage.Text = "Successfully saved."
                    lblMessage.ForeColor = Drawing.Color.Green
                    lblinfo.Text = "Information"
                    MpInfoError.Show()
                    btnClose.Focus()
                    LoadBonusType()
                    Me.btnAddItems.Text = "Add"
                    Me.txtFromQty.Text = ""
                    Me.ddlType.SelectedIndex = 0
                    Me.txtDisValue.Text = ""
                    Me.StartTime.Enabled = True
                    Me.EndTime.Enabled = True
                    Me.ddlItem.Enabled = True
                    Me.lblLineID.Text = ""
                    BindData()
                Else
                    Me.lblMessage.Text = "Error while saving bonus details"
                    lblMessage.ForeColor = Drawing.Color.Red
                    lblinfo.Text = "Validation"
                    MpInfoError.Show()
                    btnClose.Focus()
                    Exit Sub
                End If

            Catch ex As Exception
                log.Error(ex.Message)
                Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_006") & "&next=DiscountDefinition.aspx&Title=Discount Definition", False)
            End Try
        End If
    End Sub

    Protected Sub ResetBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClear.Click
        ResetDetails()
        ClearSelectdRowBackColor()
    End Sub

    Private Sub ResetDetails()
        Me.txtDisValue.Text = ""
        Me.ddlType.SelectedIndex = 0
        Me.txtFromQty.Text = ""
        Me.btnAddItems.Text = "Add"
        Me.StartTime.SelectedDate = Now.Date.AddDays(1)
        Me.EndTime.SelectedDate = Me.StartTime.SelectedDate.Value.AddMonths(1)
        Me.StartTime.Enabled = True
        Me.EndTime.Enabled = True
        Me.ddlItem.Enabled = True
        Me.lblUpMsg.Text = ""
        Me.lblLineID.Text = ""

    End Sub

    Private Function ValidationDetails() As Boolean
        Dim sucess As Boolean = False



        If Me.ddl_org.SelectedIndex <= 0 Then
            Me.lblMessage.Text = "Please select a organization"
            lblMessage.ForeColor = Drawing.Color.Red
            lblinfo.Text = "Information"
            MpInfoError.Show()
            btnClose.Focus()
            sucess = True
            Return sucess
            Exit Function
        End If


        If Me.ddlItem.SelectedIndex <= 0 Then
            Me.lblMessage.Text = "Please select a item code/description"
            lblMessage.ForeColor = Drawing.Color.Red
            lblinfo.Text = "Validation"
            MpInfoError.Show()
            btnClose.Focus()
            sucess = True
            Return sucess
            Exit Function
        End If



        If Me.txtFromQty.Text = "" Or CDec(IIf(Me.txtFromQty.Text = "", "0", Me.txtFromQty.Text)) = 0 Or CDec(IIf(Me.txtDisValue.Text = "", "0", Me.txtDisValue.Text)) = 0 Or Me.txtDisValue.Text = "" Then
            Me.lblMessage.Text = "Please enter a from quantity and discount value"
            lblMessage.ForeColor = Drawing.Color.Red
            lblinfo.Text = "Validation"
            MpInfoError.Show()
            btnClose.Focus()
            sucess = True

            Return sucess
            Exit Function
        End If




        If Me.ddlType.SelectedItem.Text = "PERCENTAGE" Then
            If CDec(IIf(Me.txtDisValue.Text = "", "0", Me.txtDisValue.Text)) > 100 Then
                Me.lblMessage.Text = "Discount Percentage should be less than or equal to 100"
                lblMessage.ForeColor = Drawing.Color.Red
                lblinfo.Text = "Validation"
                MpInfoError.Show()
                btnClose.Focus()
                sucess = True
                Return sucess
                Exit Function
            End If
        End If
        If (Me.StartTime.SelectedDate.Value <= Now.Date() And Me.StartTime.Enabled = True) Or Me.EndTime.SelectedDate.Value <= Now.Date() Then
            Me.lblMessage.Text = "Valid from and to date should be greater than current date"
            lblMessage.ForeColor = Drawing.Color.Red
            lblinfo.Text = "Validation"
            MpInfoError.Show()
            btnClose.Focus()
            sucess = True
            Return sucess
            Exit Function
        End If
        Return sucess
    End Function



  
    Protected Sub ddlItem_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ddlItem.SelectedIndexChanged
        BindData()
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
    Protected Sub btnImportSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportSave.Click
        If Me.ExcelFileUpload.FileName = Nothing Then
            ' Me.lblinfo.Text = "Validation"
            Me.lblUpMsg.Text = "Select filename "
            ' Me.lblMessage.ForeColor = Drawing.Color.Green
            ' Me.MpInfoError.Show()
            Me.MPEImport.Show()
            Exit Sub
        End If

        Dim Str As New StringBuilder
        dtErrors = Session("dtDiscountErrors")
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



                        Dim col As DataColumn


                        col = New DataColumn
                        col.ColumnName = "ItemCode"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)

                        col = New DataColumn
                        col.ColumnName = "DiscountType"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)

                        col = New DataColumn
                        col.ColumnName = "FromQty"
                        col.DataType = System.Type.GetType("System.String")
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
                        col.ColumnName = "Value"
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


                        'col = New DataColumn
                        'col.ColumnName = "UOM"
                        'col.DataType = System.Type.GetType("System.String")
                        'col.ReadOnly = False
                        'col.Unique = False
                        'TempTbl.Columns.Add(col)



                        If ViewState("FileName").ToString.EndsWith(".csv") Then
                            TempTbl = DoCSVUpload()
                        ElseIf ViewState("FileName").ToString.EndsWith(".xls") Then
                            TempTbl = DoXLSUpload()
                        ElseIf ViewState("FileName").ToString.EndsWith(".xlsx") Then
                            TempTbl = DoXLSXUpload()
                        End If

                        If TempTbl.Columns.Count = 7 Then

                            If Not (TempTbl.Columns(0).ColumnName = "ItemCode" And TempTbl.Columns(1).ColumnName = "DiscountType" And TempTbl.Columns(2).ColumnName = "FromQty" And TempTbl.Columns(3).ColumnName = "ValidFrom" And TempTbl.Columns(4).ColumnName = "ValidTo" And TempTbl.Columns(5).ColumnName = "Value" And TempTbl.Columns(6).ColumnName = "Description") Then
                                lblUpMsg.Text = "Please check the template columns are correct"

                                Me.MPEImport.Show()
                                Exit Sub
                            End If



                        Else
                            lblUpMsg.Text = "Invalid Template"
                            '' lblMessage.ForeColor = Drawing.Color.Green
                            lblinfo.Text = "Information"
                            ' MpInfoError.Show()
                            Me.MPEImport.Show()
                            Exit Sub
                        End If
                        TempTbl.Columns.Add("IsValid", GetType(String))
                        



                        If TempTbl.Rows.Count = 0 Then
                            lblUpMsg.Text = "There is no data in the uploaded file."
                            ' lblMessage.ForeColor = Drawing.Color.Green
                            'lblinfo.Text = "Information"
                            'MpInfoError.Show()
                            Me.MPEImport.Show()
                            Exit Sub
                        End If

                        Dim RowNo As String = Nothing
                        ' Dim ColNo As String = Nothing
                        ' Dim ColumnName As String = Nothing
                        Dim ErrorText As String = Nothing
                        Dim OrgID As String = Me.ddl_org.SelectedValue



                        If TempTbl.Rows.Count > 0 Then



                            Dim idx As Integer

                            For idx = 0 To TempTbl.Rows.Count - 1

                                Dim itemCode As String = Nothing
                                Dim Type As String = Nothing
                                Dim FromQty As String = Nothing
                                Dim Value As String = Nothing
                                Dim ValidFrom As String = Nothing
                                Dim ValidTo As String = Nothing

                                Dim FromDate As Date = Nothing
                                Dim ToDate As Date = Nothing
                                Dim isValidRow As Boolean = True


                                itemCode = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, "0", TempTbl.Rows(idx)(0).ToString())
                                Type = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, "0", TempTbl.Rows(idx)(1).ToString())

                                FromQty = IIf(TempTbl.Rows(idx)(2) Is DBNull.Value, "0", TempTbl.Rows(idx)(2).ToString())

                                ValidFrom = Trim(Replace(IIf(TempTbl.Rows(idx)(3) Is DBNull.Value, "0", TempTbl.Rows(idx)(3).ToString()), "12:00:00 AM", ""))
                                ValidTo = Trim(Replace(IIf(TempTbl.Rows(idx)(4) Is DBNull.Value, "0", TempTbl.Rows(idx)(4).ToString()), "12:00:00 AM", ""))

                                Value = IIf(TempTbl.Rows(idx)(5) Is DBNull.Value, "0", TempTbl.Rows(idx)(5).ToString())



                                If TempTbl.Rows(idx)(0) Is DBNull.Value Or TempTbl.Rows(idx)(1) Is DBNull.Value Or TempTbl.Rows(idx)(2) Is DBNull.Value Or TempTbl.Rows(idx)(3) Is DBNull.Value Or TempTbl.Rows(idx)(4) Is DBNull.Value Or TempTbl.Rows(idx)(5) Is DBNull.Value Then
                                    TempTbl.Rows(idx)("IsValid") = "N"
                                    Continue For
                                End If



                                If itemCode = "0" Or Type = "0" Or FromQty = "0" Or Value = "0" Then
                                    TempTbl.Rows(idx)("IsValid") = "N"
                                    Continue For
                                End If

                                If objProduct.CheckItemCode(Err_No, Err_Desc, itemCode, OrgID) = False Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Invalid Item code" + ","
                                    TotFailed += 1
                                    isValidRow = False
                                End If

                                If Type = "0" Or Not (Type.ToUpper() = "VALUE" Or Type.ToUpper() = "PERCENTAGE") Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Invalid discount type" + ","
                                    TotFailed += 1
                                    isValidRow = False
                                End If

                                If IsNumeric(FromQty) = False Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "From quantity should be in numeric" + ","
                                    TotFailed += 1
                                    isValidRow = False
                                End If


                                If IsNumeric(Value) = False Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Discount value should be in numeric" + ","
                                    TotFailed += 1
                                    isValidRow = False
                                End If

                                If ValidFrom = "0" Or IsValidInputDate(ValidFrom) = False Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Invalid valid from" + ","
                                    isValidRow = False
                                    TotFailed += 1
                                Else
                                    FromDate = FormatDate(ValidFrom)
                                End If
                                If ValidTo = "0" Or IsValidInputDate(ValidTo) = False Then
                                    RowNo = idx + 2
                                    ErrorText = ErrorText + "Invalid valid to" + ","
                                    isValidRow = False
                                    TotFailed += 1
                                Else
                                    ToDate = FormatDate(ValidTo)
                                End If

                                If ValidFrom <> "0" And IsValidInputDate(ValidFrom) = True And ValidTo <> "0" And IsValidInputDate(ValidTo) = True Then
                                    If FromDate <= Now.Date Or ToDate <= Now.Date Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Valid from and to date should be greater than current date" + ","
                                        isValidRow = False
                                        TotFailed += 1
                                    End If
                                End If

                                If ValidFrom <> "0" And IsValidInputDate(ValidFrom) = True And ValidTo <> "0" And IsValidInputDate(ValidTo) = True Then
                                    If ToDate <= FromDate Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Valid to date should be greater than from date" + ","
                                        isValidRow = False
                                        TotFailed += 1
                                    End If
                                End If

                                If Type.ToString().ToUpper() = "PERCENTAGE" And IsNumeric(Value) = True Then
                                    If CDec(IIf(Value = "", "0", Value)) > 100 Then
                                        ErrorText = ErrorText + "Discount Percentage should be less than or equal to 100"
                                        isValidRow = False
                                        TotFailed += 1
                                    End If
                                End If

                                If isValidRow = True Then
                                    If objProduct.CheckDiscountDataActiveRange(Err_No, Err_Desc, itemCode, CLng(IIf(FromQty = "", "0", FromQty)), Me.ddl_org.SelectedValue, FromDate, ToDate, "0") = True Then
                                        RowNo = idx + 2
                                        ErrorText = ErrorText + "Discount already define for this item with same slab" + ","
                                        isValidRow = False
                                        TotFailed += 1
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

                                    TotSuccess = TotSuccess + 1
                                    Dim h As DataRow = dtErrors.NewRow()
                                    h("RowNo") = idx + 2
                                    h("LogInfo") = "Successfully uploaded"
                                    TempTbl.Rows(idx)("ValidFrom") = FromDate
                                    TempTbl.Rows(idx)("ValidTo") = ToDate
                                    dtErrors.Rows.Add(h)
                                    RowNo = Nothing
                                    ErrorText = Nothing
                                    isValidRow = True
                                End If





                            Next
                        End If

                        If objProduct.UploadDiscount(TempTbl, Me.ddl_org.SelectedValue, Err_No, Err_Desc) = True Then
                            DeleteExcel()
                            lblUpMsg.Text = IIf(TotFailed = 0, "Successfully uploaded", "One or more rows has invalid data. Please check the log")
                            MPEImport.Show()
                            BindData()

                        Else
                            DeleteExcel()
                            lblUpMsg.Text = "Please check the uploaded log"

                            MPEImport.Show()
                            Exit Sub
                        End If
                    End If


                    Me.dgvErros.DataSource = dtErrors
                    Me.dgvErros.DataBind()
                    Session.Remove("dtDiscountErrors")
                    Session("dtDiscountErrors") = dtErrors


                    Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & "Discount_" & Now.ToString("yyyyMMdd") + ".txt"
                    DataTable2CSV(dtErrors, fn, vbTab)

                    Session.Remove("DiscountLogInfo")
                    Session("DiscountLogInfo") = fn




                Catch ex As Exception

                    Err_No = "13552"
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
    Protected Sub lbLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbLog.Click
        If Not Session("DiscountLogInfo") Is Nothing Then
            Dim fileValue As String = Session("DiscountLogInfo")





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
                MPEImport.Show()
                Exit Sub

            End If

        Else
            lblUpMsg.Text = "There is no log to show."
            'lblMessage.ForeColor = Drawing.Color.Green
            'lblinfo.Text = "Information"
            MPEImport.Show()
            Exit Sub

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

        Session.Remove("dtDiscountErrors")
        Session("dtDiscountErrors") = dtErrors
    End Sub
    Protected Sub btnImportWindow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportWindow.Click
        If Me.ddl_org.SelectedIndex > 0 Then
            Me.lblUpMsg.Text = ""
            Me.MPEImport.Show()
        Else
            Me.lblinfo.Text = "Information"
            Me.lblMessage.Text = "Please select a organization."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Exit Sub
        End If
    End Sub


    Protected Sub Export_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click

        If Me.ddl_org.SelectedIndex > 0 Then
            Dim dtOriginal As New DataTable()
            Dim OrgID As String = Me.ddl_org.SelectedValue

            dtOriginal = objProduct.GetDiscountData(Err_No, Err_Desc, OrgID)

            Dim dtTemp As New DataTable()

            'Creating Header Row
            dtTemp.Columns.Add("ItemCode")
            dtTemp.Columns.Add("DiscountType")
            dtTemp.Columns.Add("FromQty")
            dtTemp.Columns.Add("ValidFrom")
            dtTemp.Columns.Add("ValidTo")
            dtTemp.Columns.Add("Value")
            dtTemp.Columns.Add("Description")
            '  dtTemp.Columns.Add("UOM")

            Dim drAddItem As DataRow
            For i As Integer = 0 To dtOriginal.Rows.Count - 1
                drAddItem = dtTemp.NewRow()
                drAddItem(0) = dtOriginal.Rows(i)("Item_Code").ToString()
                drAddItem(1) = dtOriginal.Rows(i)("DisType").ToString()
                drAddItem(2) = dtOriginal.Rows(i)("FromQty").ToString()
                drAddItem(3) = DateTime.Parse(dtOriginal.Rows(i)("Valid_From").ToString()).ToString("dd/MM/yyyy")
                drAddItem(4) = DateTime.Parse(dtOriginal.Rows(i)("Valid_To").ToString()).ToString("dd/MM/yyyy")
                drAddItem(5) = dtOriginal.Rows(i)("DisValue").ToString()
                drAddItem(6) = dtOriginal.Rows(i)("ItemName").ToString()
                '  drAddItem(7) = dtOriginal.Rows(i)("UOM").ToString()
                dtTemp.Rows.Add(drAddItem)
            Next

            If dtOriginal.Rows.Count = 0 Then

                Me.lblinfo.Text = "Information"
                Me.lblMessage.Text = "There is no data for the selected filter criteria"
                Me.lblMessage.ForeColor = Drawing.Color.Red
                Me.MpInfoError.Show()
                Exit Sub

                drAddItem = dtTemp.NewRow()
                drAddItem(0) = ""
                drAddItem(1) = ""
                drAddItem(2) = ""
                drAddItem(3) = ""
                drAddItem(4) = ""
                drAddItem(5) = ""
                drAddItem(6) = ""
                '  drAddItem(7) = ""
                dtTemp.Rows.Add(drAddItem)
            End If

            'Temp(Grid)
            Dim dg As New DataGrid()
            dg.DataSource = dtTemp
            dg.DataBind()
            If dtTemp.Rows.Count > 0 Then
                'Dim fn As String = "MSL" & Now.ToString("ddMMMyyHHmmss") + ".xls"
                Dim fn As String = "Discount" + ".xls"
                Dim d As New DataSet
                d.Tables.Add(dtTemp)

                ExportToExcel(fn, d)

            End If
        Else
            Me.lblinfo.Text = "Information"
            Me.lblMessage.Text = "Please select a organization"
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
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
End Class





