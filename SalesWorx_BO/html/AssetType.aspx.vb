Imports SalesWorx.BO.Common
Imports System.Data
Imports System.Web
Imports System.Resources
Imports System.Web.UI.WebControls
Imports log4net
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Data.OleDb
Imports System.Data.SqlClient

Partial Public Class AssetType
    Inherits System.Web.UI.Page
    Dim objAssetType As New SalesWorx.BO.Common.AssetType
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim Dt As New DataTable
    Private Const PageID As String = "P263"
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
            Dt = objAssetType.FillAssetType(Err_No, Err_Desc)
            BindAssetTypeData()
            Resetfields()
            ViewState("FileType") = Nothing
            ViewState("FileName") = Nothing
            ViewState("CSVName") = Nothing
        Else
            MPEDetails.VisibleOnPageLoad = False

        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Try
            '' ''Me.MPECurrency.Hide()
            MPEDetails.VisibleOnPageLoad = False

            Resetfields()
            ClassUpdatePnl.Update()
        Catch

        End Try
    End Sub

    Public Sub Resetfields()
        Me.txtAssetTypeId.Text = ""
        Me.txtDescription.Text = ""
        Me.txtParam1.Text = ""
        Me.txtParam2.Text = ""
        Me.txtParam3.Text = ""
        Me.txtParam4.Text = ""
        Me.txtParam5.Text = ""
        Me.ddFilterBy.SelectedIndex = 0
        Me.btnSave.Text = "Save"
        lblPop.Text = ""
        lblerr.Text = ""
    End Sub



    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If Me.txtDescription.Text = "" Then
            ''Me.lblinfo.Text = "Validation"
            ''Me.lblMessage.Text = "Please enter asset type."
            ''Me.lblMessage.ForeColor = Drawing.Color.Red
            ''Me.MpInfoError.Show()
            ''Me.MPECurrency.Show()
            lblPop.Text = "Please enter asset type."
            MPEDetails.VisibleOnPageLoad = True

            Exit Sub
        End If


        Dim success As Boolean = False
        Try


            objAssetType.AssetType = IIf(Me.txtDescription.Text = "", "0", Me.txtDescription.Text)
            objAssetType.Param1 = IIf(Me.txtParam1.Text = "", "0", Me.txtParam1.Text)
            objAssetType.Param2 = IIf(Me.txtParam2.Text = "", "0", Me.txtParam2.Text)
            objAssetType.Param3 = IIf(Me.txtParam3.Text = "", "0", Me.txtParam3.Text)
            objAssetType.Param4 = IIf(Me.txtParam4.Text = "", "0", Me.txtParam4.Text)
            objAssetType.Param5 = IIf(Me.txtParam5.Text = "", "0", Me.txtParam5.Text)


            If objAssetType.CheckDuplicate(Err_No, Err_Desc, IIf(Me.txtAssetTypeId.Text = "", "0", Me.txtAssetTypeId.Text)) = False Then
                If objAssetType.InsertAssetType(Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
                    success = True
                    ''Me.lblMessage.Text = "Successfully saved."
                    ''lblMessage.ForeColor = Drawing.Color.Green
                    ''lblinfo.Text = "Information"
                    ''MpInfoError.Show()
                    ''btnClose.Focus()
                    MessageBoxValidation("Successfully saved.", "Information")
                End If
            Else
                ''Me.lblMessage.Text = "Same asset type already exist."
                ''lblMessage.ForeColor = Drawing.Color.Red
                ''lblinfo.Text = "Validation"
                ''MpInfoError.Show()
                ''btnClose.Focus()
                lblPop.Text = "Same asset type already exist."
                MPEDetails.VisibleOnPageLoad = True

                Exit Sub
            End If
            If success = True Then
                ' objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "CURRENCY", Me.txtCurrencyCode.Text, "Code: " & Me.txtCurrencyCode.Text & "/ Desc :  " & Me.txtDescription.Text & "/ Rate:  " & Me.txtRate.Text & "/ Decimal : " & ddlDigits.SelectedValue.ToString(), CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                Dt = objAssetType.FillAssetType(Err_No, Err_Desc)
                BindAssetTypeData()
                Resetfields()
                ' ''Me.MPECurrency.Hide()
                MPEDetails.VisibleOnPageLoad = False

                ClassUpdatePnl.Update()
            Else

                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("AssetType_001") & "&next=AssetType.aspx&Title=Message", False)
                Exit Try
            End If

        Catch ex As Exception
            Err_No = "74061"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        str = str.Replace("'", "&#39;")
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Private Sub BindAssetTypeData()
        Dim dv As New DataView(Dt)
        If ViewState("SortField") <> "" Then
            dv.Sort = (ViewState("SortField") & " ") + SortDirection
        End If
        Me.gvCurrency.DataSource = dv
        Me.gvCurrency.DataBind()
        Session.Remove("AssetType")
        Session("AssetType") = Dt



        If Me.ddFilterBy.SelectedIndex = 1 Then
            Dt.DefaultView.RowFilter = " ( Asset_Type_Id='" + txtFilterVal.Text + "' )"
        End If
        If Me.ddFilterBy.SelectedIndex = 2 Then
            Dt.DefaultView.RowFilter = "(Description LIKE '%" + txtFilterVal.Text + "%')"
        End If

        dv = Dt.DefaultView
        If ViewState("SortField") <> "" Then
            dv.Sort = (ViewState("SortField") & " ") + SortDirection
        End If
        gvCurrency.DataSource = dv
        gvCurrency.DataBind()
        Session.Remove("AssetType")
        Session("AssetType") = Dt
    End Sub

    Public Function IsNumeric(ByVal inputString As String) As Boolean
        Dim _isNumber As System.Text.RegularExpressions.Regex = New  _
Regex("(^[-+]?\d+(,?\d*)*\.?\d*([Ee][-+]\d*)?$)|(^[-+]?\d?(,?\d*)*\.\d+([Ee][-+]\d*)?$)")
        Return _isNumber.Match(inputString).Success
    End Function

    Public Function IsAlpha(ByVal strToCheck As String) As Boolean
        Dim objAlphaPattern As Regex = New Regex("[^a-zA-Z]")
        Return Not objAlphaPattern.IsMatch(strToCheck)
    End Function


    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If Me.txtAssetTypeId.Text = "" Or Me.txtDescription.Text = "" Then
            ''Me.lblinfo.Text = "Validation"
            ''Me.lblMessage.Text = "Please enter asset type"
            ''Me.lblMessage.ForeColor = Drawing.Color.Red
            ''Me.MpInfoError.Show()
            ' ''Me.MPECurrency.Show()
            lblPop.Text = "Please enter asset type."
            MPEDetails.VisibleOnPageLoad = True

            Exit Sub
        End If


        Dim success As Boolean = False
        Try

            objAssetType.AssetTypeId = IIf(Me.txtAssetTypeId.Text = "", "0", Me.txtAssetTypeId.Text)
            objAssetType.AssetType = IIf(Me.txtDescription.Text = "", "0", Me.txtDescription.Text)
            objAssetType.Param1 = IIf(Me.txtParam1.Text = "", "0", Me.txtParam1.Text)
            objAssetType.Param2 = IIf(Me.txtParam2.Text = "", "0", Me.txtParam2.Text)
            objAssetType.Param3 = IIf(Me.txtParam3.Text = "", "0", Me.txtParam3.Text)
            objAssetType.Param4 = IIf(Me.txtParam4.Text = "", "0", Me.txtParam4.Text)
            objAssetType.Param5 = IIf(Me.txtParam5.Text = "", "0", Me.txtParam5.Text)


            If objAssetType.CheckDuplicate(Err_No, Err_Desc, IIf(Me.txtAssetTypeId.Text = "", "0", Me.txtAssetTypeId.Text)) = True Then
                ' ''Me.lblMessage.Text = "Same asset type already exist."
                ' ''lblMessage.ForeColor = Drawing.Color.Red
                ' ''lblinfo.Text = "Validation"
                ' ''MpInfoError.Show()
                ' ''btnClose.Focus()
                lblPop.Text = "Same asset type already exist."
                MPEDetails.VisibleOnPageLoad = True

                Exit Sub
            End If



            If objAssetType.UpdateAssetType(Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
                success = True
                ' ''Me.lblMessage.Text = "Successfully updated."
                ' ''lblMessage.ForeColor = Drawing.Color.Green
                ' ''lblinfo.Text = "Information"
                ' ''MpInfoError.Show()
                ' ''btnClose.Focus()
                MessageBoxValidation("Successfully updated.", "Information")
            End If

            If success = True Then
                Dt = objAssetType.FillAssetType(Err_No, Err_Desc)
                BindAssetTypeData()
                Resetfields()
                ' ''Me.MPECurrency.Hide()
                MPEDetails.VisibleOnPageLoad = False

                ClassUpdatePnl.Update()
            Else

                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("AssetType_002") & "&next=AssetType.aspx&Title=Message", False)
                Exit Try
            End If

        Catch ex As Exception
            Err_No = "74062"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Protected Sub btnDeleteAll_Click()
        Try
            Dim dr As GridViewRow
            Dim Success As Boolean = False
            For Each dr In gvCurrency.Rows
                Dim RowCheckBox As System.Web.UI.WebControls.CheckBox = dr.FindControl("chkDelete")

                If RowCheckBox.Checked = True Then
                    Dim Lbl As System.Web.UI.WebControls.Label = dr.FindControl("lblCurrency")
                    HidVal.Value = Lbl.Text
                    objAssetType.AssetTypeId = dr.Cells(1).Text

                    If objAssetType.CheckExist(Err_No, Err_Desc) = True Then
                        ''lblMessage.Text = "There are some assets link with this Asset type " + "'" + dr.Cells(2).Text.ToString() + "'" + ".Please remove the assets first."
                        ''lblMessage.ForeColor = Drawing.Color.Green
                        ''lblinfo.Text = "Information"
                        ''MpInfoError.Show()
                        ''btnClose.Focus()
                        MessageBoxValidation("There are some assets link with this Asset type " + "'" + dr.Cells(2).Text.ToString() + "'" + ".Please remove the assets first.", "Information")
                        Exit Sub
                    End If



                    If objAssetType.DeleteAssetType(Err_No, Err_Desc) = True Then
                        Success = True
                    End If
                End If
            Next
            If (Success = True) Then
                ''lblMessage.Text = "Asset type(s) deleted successfully."
                ''lblMessage.ForeColor = Drawing.Color.Green
                ''lblinfo.Text = "Information"
                ''MpInfoError.Show()
                ''btnClose.Focus()
                MessageBoxValidation("Asset type(s) deleted successfully.", "Information")
                Dt = objAssetType.FillAssetType(Err_No, Err_Desc)
                BindAssetTypeData()
                Resetfields()
            Else
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("AssetType_003") & "&next=AssetType.aspx&Title=Message", False)
                Exit Try
            End If

            'ClassUpdatePnl.Update()
        Catch ex As Exception
            Err_No = "74063"
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
            'Dt = objAssetType.SearchAssetType(Err_No, Err_Desc, Me.ddFilterBy.Text, IIf(Me.txtFilterVal.Text = "", "0", Me.txtFilterVal.Text))
            Dt = objAssetType.FillAssetType(Err_No, Err_Desc)
            BindAssetTypeData()
            ClassUpdatePnl.Update()
            success = True
            If success = False Then
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("AssetType_004") & "&next=AssetType.aspx&Title=Message", False)
                Exit Try
            End If
        Catch ex As Exception
            Err_No = "74064"
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
        objAssetType.AssetTypeId = row.Cells(1).Text
        HidVal.Value = row.Cells(1).Text
        Try
            If objAssetType.CheckExist(Err_No, Err_Desc) = True Then
                ''lblMessage.Text = "There are some assets link with this type. Please remove the associated assets first."
                ''lblMessage.ForeColor = Drawing.Color.Green
                ''lblinfo.Text = "Information"
                ''MpInfoError.Show()
                ''btnClose.Focus()
                MessageBoxValidation("There are some assets link with this type. Please remove the associated assets first.", "Information")
                Exit Sub
            End If
            If objAssetType.DeleteAssetType(Err_No, Err_Desc) = True Then
                success = True
            End If

            If success = True Then
                ''lblMessage.Text = "Successfully deleted."
                ''lblMessage.ForeColor = Drawing.Color.Green
                ''lblinfo.Text = "Information"
                ''MpInfoError.Show()
                ''btnClose.Focus()
                MessageBoxValidation("Successfully deleted.", "Information")
                Dt = objAssetType.FillAssetType(Err_No, Err_Desc)
                BindAssetTypeData()
                Resetfields()
            Else
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("AssetType_005") & "&next=AssetType.aspx&Title=Message", False)
                Exit Try
            End If

        Catch ex As Exception
            Err_No = "74065"
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
        '' ''Me.MPECurrency.Show()
        MPEDetails.VisibleOnPageLoad = True

    End Sub


    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try

            btnUpdate.Visible = True
            btnSave.Visible = False
            Resetfields()
            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
            Dim param1 As Label = DirectCast(row.FindControl("Param1"), Label)
            Dim param2 As Label = DirectCast(row.FindControl("Param2"), Label)
            Dim param3 As Label = DirectCast(row.FindControl("Param3"), Label)
            Dim param4 As Label = DirectCast(row.FindControl("Param4"), Label)
            Dim param5 As Label = DirectCast(row.FindControl("Param5"), Label)



            HidVal.Value = row.Cells(1).Text
            txtAssetTypeId.Text = Trim(row.Cells(1).Text)
            txtDescription.Text = Trim(row.Cells(2).Text)
            txtParam1.Text = IIf(param1.Text = "0", "", param1.Text)
            txtParam2.Text = IIf(param2.Text = "0", "", param2.Text)
            txtParam3.Text = IIf(param3.Text = "0", "", param3.Text)
            txtParam4.Text = IIf(param4.Text = "0", "", param4.Text)
            txtParam5.Text = IIf(param5.Text = "0", "", param5.Text)
            ' ClassUpdatePnl.Update()
            ' ''MPECurrency.Show()
            MPEDetails.VisibleOnPageLoad = True

        Catch ex As Exception
            Err_No = "74066"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("AssetType_006") & "&next=AssetType.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub

    Private Sub gvCurrency_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvCurrency.PageIndexChanging
        gvCurrency.PageIndex = e.NewPageIndex
        Dt = objAssetType.FillAssetType(Err_No, Err_Desc)
        BindAssetTypeData()

    End Sub

    Private Sub gvCurrency_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCurrency.RowDataBound
        'If e.Row.RowType.Equals(DataControlRowType.Pager) Then
        '    Dim pTableRow As TableRow = _
        '             CType(e.Row.Cells(0).Controls(0).Controls(0), TableRow)
        '    For Each cell As TableCell In pTableRow.Cells
        '        For Each control As Control In cell.Controls
        '            If TypeOf control Is LinkButton Then
        '                Dim lb As LinkButton = CType(control, LinkButton)
        '                lb.Attributes.Add("onclick", "ScrollToTop();")
        '            End If
        '        Next
        '    Next
        'End If
    End Sub
    Private Sub gvCurrency_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvCurrency.Sorting
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        Dt = objAssetType.FillAssetType(Err_No, Err_Desc)
        BindAssetTypeData()
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
        If Me.ExcelFileUpload.FileName = Nothing Then
            ' ''Me.lblinfo.Text = "Validation"
            ' ''Me.lblMessage.Text = "Select filename "
            ' ''Me.lblMessage.ForeColor = Drawing.Color.Green
            ' ''Me.MpInfoError.Show()
            lblerr.Text = "Select filename "
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
                ' Dim strScript As String
                ' strScript = "<script language='javascript'>"
                ' strScript += "document.aspnetForm('ctl00_ContentPlaceHolder1_DummyImBtn').click();"
                'strScript += "</script>"
                ' Page.ClientScript.RegisterStartupScript(Me.GetType(), "StrScript", _
                ' strScript, False)
                Try
                    Dim st As Boolean = False

                    If ViewState("FileType") IsNot Nothing And ViewState("FileName") IsNot Nothing Then
                        Dim TempTbl As New DataTable
                        If TempTbl.Rows.Count > 0 Then
                            TempTbl.Rows.Clear()
                        End If
                        Dim col As DataColumn

                        col = New DataColumn
                        col.ColumnName = "AssetType"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)


                        'col = New DataColumn
                        'col.ColumnName = "Attribute1"
                        'col.DataType = System.Type.GetType("System.String")
                        'col.ReadOnly = False
                        'col.Unique = False
                        'TempTbl.Columns.Add(col)



                        'col = New DataColumn
                        'col.ColumnName = "Attribute2"
                        'col.DataType = System.Type.GetType("System.String")
                        'col.ReadOnly = False
                        'col.Unique = False
                        'TempTbl.Columns.Add(col)




                        'col = New DataColumn
                        'col.ColumnName = "Attribute3"
                        'col.DataType = System.Type.GetType("System.String")
                        'col.ReadOnly = False
                        'col.Unique = False
                        'TempTbl.Columns.Add(col)




                        'col = New DataColumn
                        'col.ColumnName = "Attribute4"
                        'col.DataType = System.Type.GetType("System.String")
                        'col.ReadOnly = False
                        'col.Unique = False
                        'TempTbl.Columns.Add(col)



                        'col = New DataColumn
                        'col.ColumnName = "Attribute5"
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

                        If TempTbl.Columns.Count = 1 Then
                            If Not TempTbl.Columns(0).ColumnName = "AssetType" Then
                                ''lblMessage.Text = "Please check the template columns are correct"
                                ''lblMessage.ForeColor = Drawing.Color.Green
                                ''lblinfo.Text = "Information"
                                ''MpInfoError.Show()
                                lblerr.Text = "Please check the template columns are correct"
                                ClassUpdatePnl.Update()
                                Exit Sub
                            End If



                        Else
                            ' ''lblMessage.Text = "Invalid Template"
                            ' ''lblMessage.ForeColor = Drawing.Color.Green
                            ' ''lblinfo.Text = "Information"
                            ' ''MpInfoError.Show()
                            lblerr.Text = "Please check the template columns are correct"
                            ClassUpdatePnl.Update()
                            Exit Sub
                        End If




                        If TempTbl.Rows.Count = 0 Then
                            ' ''lblMessage.Text = "There is no data in your file."
                            ' ''lblMessage.ForeColor = Drawing.Color.Green
                            ' ''lblinfo.Text = "Information"
                            ' ''MpInfoError.Show()
                            lblerr.Text = "Please check the template columns are correct"
                            Me.ClassUpdatePnl.Update()
                            Exit Sub
                        End If
                        Dim totrow As Integer = 0
                        Dim notimportedrow As Integer = 0
                        If TempTbl.Rows.Count > 0 Then
                            Dim idx As Integer
                            'if Me.rbAppend.Checked = True Then
                            For idx = 0 To TempTbl.Rows.Count - 1
                                objAssetType.AssetType = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, "0", TempTbl.Rows(idx)(0).ToString())
                                'objAssetType.Param1 = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, "0", TempTbl.Rows(idx)(1).ToString())
                                'objAssetType.Param2 = IIf(TempTbl.Rows(idx)(2) Is DBNull.Value, "0", TempTbl.Rows(idx)(2).ToString())
                                'objAssetType.Param3 = IIf(TempTbl.Rows(idx)(3) Is DBNull.Value, "0", TempTbl.Rows(idx)(3).ToString())
                                'objAssetType.Param4 = IIf(TempTbl.Rows(idx)(4) Is DBNull.Value, "0", TempTbl.Rows(idx)(4).ToString())
                                'objAssetType.Param5 = IIf(TempTbl.Rows(idx)(5) Is DBNull.Value, "0", TempTbl.Rows(idx)(5).ToString())

                                objAssetType.Param1 = "0"
                                objAssetType.Param2 = "0"
                                objAssetType.Param3 = "0"
                                objAssetType.Param4 = "0"
                                objAssetType.Param5 = "0"


                                'If IsNumeric(objAssetType.Param4) = False Or IsNumeric(objAssetType.Param5) = False Then
                                '    Me.lblinfo.Text = "Validation"
                                '    Me.lblMessage.Text = "Attribute 4 and 5 should be in numeric ."
                                '    Me.lblMessage.ForeColor = Drawing.Color.Green
                                '    Me.MpInfoError.Show()
                                '    Me.MPEImport.Show()
                                '    Exit Sub
                                'End If

                                If objAssetType.AssetType <> "0" Then
                                    If objAssetType.CheckDuplicate(Err_No, Err_Desc, "0") = False Then
                                        'If objAssetType.AssetType <> "0" And IsNumeric(objAssetType.Param4) = True And IsNumeric(objAssetType.Param5) = True Then

                                        If objAssetType.InsertAssetType(Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
                                            totrow += 1
                                            st = True
                                        End If

                                        'Else
                                        '    If objAssetType.AssetType <> "0" And IsNumeric(objAssetType.Param4) = True And IsNumeric(objAssetType.Param5) = True Then
                                        '        If objAssetType.UpdateAssetType(Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
                                        '            st = True
                                        '        End If
                                        '    End If
                                    Else

                                        notimportedrow = notimportedrow + 1
                                    End If
                                End If
                            Next
                            'ElseIf Me.rbRebuild.Checked = True Then
                            '    objAssetType.RebuildAll(Err_No, Err_Desc)
                            '    For idx = 0 To TempTbl.Rows.Count - 1
                            '        objAssetType.AssetType = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, "0", TempTbl.Rows(idx)(0).ToString())
                            '        objAssetType.Param1 = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, "0", TempTbl.Rows(idx)(1).ToString())
                            '        objAssetType.Param2 = IIf(TempTbl.Rows(idx)(2) Is DBNull.Value, "0", TempTbl.Rows(idx)(2).ToString())
                            '        objAssetType.Param3 = IIf(TempTbl.Rows(idx)(3) Is DBNull.Value, "0", TempTbl.Rows(idx)(3).ToString())
                            '        objAssetType.Param4 = IIf(TempTbl.Rows(idx)(4) Is DBNull.Value, "0", TempTbl.Rows(idx)(4).ToString())
                            '        objAssetType.Param5 = IIf(TempTbl.Rows(idx)(5) Is DBNull.Value, "0", TempTbl.Rows(idx)(5).ToString())





                            '        If objAssetType.AssetType <> "0" And IsNumeric(objAssetType.Param4) = True And IsNumeric(objAssetType.Param5) = True Then
                            '            If objAssetType.InsertAssetType(Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
                            '                st = True
                            '            End If
                            '        End If

                            '    Next
                            'End If
                        End If
                        If st = True Or notimportedrow > 0 Then
                            DeleteExcel()
                            Me.MPEImport.Hide()
                            ' lblMessage.Text = "Successfully imported."
                            ''lblMessage.Text = IIf(totrow > 0, totrow.ToString() + " Rows successfully imported.", "") & IIf(notimportedrow > 0, notimportedrow & " asset type " & " already exist", "")
                            ''lblMessage.ForeColor = Drawing.Color.Green
                            ''lblinfo.Text = "Information"
                            ''MpInfoError.Show()
                            ''btnClose.Focus()
                            MessageBoxValidation(IIf(totrow > 0, totrow.ToString() + " Rows successfully imported.", "") & IIf(notimportedrow > 0, notimportedrow & " asset type " & " already exist", ""), "Information")
                            Dt = objAssetType.FillAssetType(Err_No, Err_Desc)
                            BindAssetTypeData()
                            ClassUpdatePnl.Update()
                            Resetfields()
                        Else
                            ' ''lblMessage.Text = "Please check the template data"
                            ' ''lblMessage.ForeColor = Drawing.Color.Green
                            ' ''lblinfo.Text = "Information"
                            ' ''MpInfoError.Show()
                            lblerr.Text = "Please check the template data"
                            ClassUpdatePnl.Update()
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
            Else
                Str.Append("<span style='font-color:red'> <b >Please import valid Excel template.</b></span>")
                ' ''lblMessage.Text = Str.ToString()
                ' ''lblMessage.ForeColor = Drawing.Color.Green
                ' ''lblinfo.Text = "Information"
                'MpInfoError.Show()
                lblerr.Text = "Please import valid Excel template."
            End If


        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try

    End Sub
    Protected Sub Clear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Clear.Click
        Me.ddFilterBy.SelectedIndex = 0
        Me.txtFilterVal.Text = ""
        Dt = objAssetType.FillAssetType(Err_No, Err_Desc)
        BindAssetTypeData()
        ClassUpdatePnl.Update()
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

                col = New DataColumn
                col.ColumnName = "AssetType"
                col.DataType = System.Type.GetType("System.String")
                col.ReadOnly = False
                col.Unique = False
                TempTbl.Columns.Add(col)


                'col = New DataColumn
                'col.ColumnName = "Attribute1"
                'col.DataType = System.Type.GetType("System.String")
                'col.ReadOnly = False
                'col.Unique = False
                'TempTbl.Columns.Add(col)



                'col = New DataColumn
                'col.ColumnName = "Attribute2"
                'col.DataType = System.Type.GetType("System.String")
                'col.ReadOnly = False
                'col.Unique = False
                'TempTbl.Columns.Add(col)




                'col = New DataColumn
                'col.ColumnName = "Attribute3"
                'col.DataType = System.Type.GetType("System.String")
                'col.ReadOnly = False
                'col.Unique = False
                'TempTbl.Columns.Add(col)




                'col = New DataColumn
                'col.ColumnName = "Attribute4"
                'col.DataType = System.Type.GetType("System.String")
                'col.ReadOnly = False
                'col.Unique = False
                'TempTbl.Columns.Add(col)



                'col = New DataColumn
                'col.ColumnName = "Attribute5"
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

                If TempTbl.Columns.Count = 1 Then
                    If Not TempTbl.Columns(0).ColumnName = "AssetType" Then
                        ' ''lblMessage.Text = "Please check the template columns are correct"
                        ' ''lblMessage.ForeColor = Drawing.Color.Green
                        ' ''lblinfo.Text = "Information"
                        ' ''MpInfoError.Show()
                        MessageBoxValidation("Please check the template columns are correct.", "Information")
                        ClassUpdatePnl.Update()
                        Exit Sub
                    End If



                Else
                    ' ''lblMessage.Text = "Invalid Template"
                    ' ''lblMessage.ForeColor = Drawing.Color.Green
                    ' ''lblinfo.Text = "Information"
                    ' ''MpInfoError.Show()
                    ClassUpdatePnl.Update()
                    MessageBoxValidation("Invalid Template.", "Information")
                    Exit Sub
                End If




                If TempTbl.Rows.Count = 0 Then
                    ''lblMessage.Text = "There is no data in the file."
                    ''lblMessage.ForeColor = Drawing.Color.Green
                    ''lblinfo.Text = "Information"
                    ''MpInfoError.Show()
                    MessageBoxValidation("There is no data in the file.", "Information")
                    Me.ClassUpdatePnl.Update()
                    Exit Sub
                End If
                If TempTbl.Rows.Count > 0 Then
                    Dim idx As Integer
                    'if Me.rbAppend.Checked = True Then
                    For idx = 0 To TempTbl.Rows.Count - 1
                        objAssetType.AssetType = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, "0", TempTbl.Rows(idx)(0).ToString())
                        'objAssetType.Param1 = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, "0", TempTbl.Rows(idx)(1).ToString())
                        'objAssetType.Param2 = IIf(TempTbl.Rows(idx)(2) Is DBNull.Value, "0", TempTbl.Rows(idx)(2).ToString())
                        'objAssetType.Param3 = IIf(TempTbl.Rows(idx)(3) Is DBNull.Value, "0", TempTbl.Rows(idx)(3).ToString())
                        'objAssetType.Param4 = IIf(TempTbl.Rows(idx)(4) Is DBNull.Value, "0", TempTbl.Rows(idx)(4).ToString())
                        'objAssetType.Param5 = IIf(TempTbl.Rows(idx)(5) Is DBNull.Value, "0", TempTbl.Rows(idx)(5).ToString())

                        objAssetType.Param1 = "0"
                        objAssetType.Param2 = "0"
                        objAssetType.Param3 = "0"
                        objAssetType.Param4 = "0"
                        objAssetType.Param5 = "0"


                        'If IsNumeric(objAssetType.Param4) = False Or IsNumeric(objAssetType.Param5) = False Then
                        '    Me.lblinfo.Text = "Validation"
                        '    Me.lblMessage.Text = "Attribute 4 and 5 should be in numeric ."
                        '    Me.lblMessage.ForeColor = Drawing.Color.Green
                        '    Me.MpInfoError.Show()
                        '    Me.MPEImport.Show()
                        '    Exit Sub
                        'End If


                        If objAssetType.CheckDuplicate(Err_No, Err_Desc, "0") = False Then
                            'If objAssetType.AssetType <> "0" And IsNumeric(objAssetType.Param4) = True And IsNumeric(objAssetType.Param5) = True Then
                            If objAssetType.AssetType <> "0" Then
                                If objAssetType.InsertAssetType(Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
                                    st = True
                                End If
                            End If
                            'Else
                            '    If objAssetType.AssetType <> "0" And IsNumeric(objAssetType.Param4) = True And IsNumeric(objAssetType.Param5) = True Then
                            '        If objAssetType.UpdateAssetType(Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
                            '            st = True
                            '        End If
                            '    End If
                        End If
                    Next
                    'ElseIf Me.rbRebuild.Checked = True Then
                    '    objAssetType.RebuildAll(Err_No, Err_Desc)
                    '    For idx = 0 To TempTbl.Rows.Count - 1
                    '        objAssetType.AssetType = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, "0", TempTbl.Rows(idx)(0).ToString())
                    '        objAssetType.Param1 = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, "0", TempTbl.Rows(idx)(1).ToString())
                    '        objAssetType.Param2 = IIf(TempTbl.Rows(idx)(2) Is DBNull.Value, "0", TempTbl.Rows(idx)(2).ToString())
                    '        objAssetType.Param3 = IIf(TempTbl.Rows(idx)(3) Is DBNull.Value, "0", TempTbl.Rows(idx)(3).ToString())
                    '        objAssetType.Param4 = IIf(TempTbl.Rows(idx)(4) Is DBNull.Value, "0", TempTbl.Rows(idx)(4).ToString())
                    '        objAssetType.Param5 = IIf(TempTbl.Rows(idx)(5) Is DBNull.Value, "0", TempTbl.Rows(idx)(5).ToString())





                    '        If objAssetType.AssetType <> "0" And IsNumeric(objAssetType.Param4) = True And IsNumeric(objAssetType.Param5) = True Then
                    '            If objAssetType.InsertAssetType(Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
                    '                st = True
                    '            End If
                    '        End If

                    '    Next
                    'End If
                End If
                If st = True Then
                    DeleteExcel()
                    Me.MPEImport.Hide()
                    ''lblMessage.Text = "Successfully imported."
                    ''lblMessage.ForeColor = Drawing.Color.Green
                    ''lblinfo.Text = "Information"
                    ''MpInfoError.Show()
                    ''btnClose.Focus()
                    MessageBoxValidation("Successfully imported.", "Information")
                    Dt = objAssetType.FillAssetType(Err_No, Err_Desc)
                    BindAssetTypeData()
                    ClassUpdatePnl.Update()
                    Resetfields()
                Else
                    ' ''lblMessage.Text = "Please check the template data"
                    ' ''lblMessage.ForeColor = Drawing.Color.Green
                    ' ''lblinfo.Text = "Information"
                    ' ''MpInfoError.Show()
                    MessageBoxValidation("Please check the template data.", "Information")
                    ClassUpdatePnl.Update()
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