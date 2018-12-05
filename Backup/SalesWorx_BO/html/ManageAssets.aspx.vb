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
Imports Telerik.Web.UI
Imports ExcelLibrary.SpreadSheet
Partial Public Class ManageAssets
    Inherits System.Web.UI.Page
    Dim objAssetType As New SalesWorx.BO.Common.AssetType
    Dim objCommon As New SalesWorx.BO.Common.Common
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim Dt As New DataTable
    Private dtErrors As New DataTable
    Private Const PageID As String = "P264"
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
            Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
            ddOraganisation.DataSource = objCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
            ddOraganisation.DataBind()
            ddOraganisation.Items.Insert(0, New ListItem("-- Select a value --"))

            LoadAssetTypes()
            BindAssets()
            Resetfields()

            ViewState("FileType") = Nothing
            ViewState("FileName") = Nothing
            ViewState("CSVName") = Nothing
            Session.Remove("AssetsLogInfo")
            Session.Remove("dtAssetsErrors")
            SetErrorsTable()
        Else
            MapWindow.VisibleOnPageLoad = False
            dtErrors = Session("dtAssetsErrors")


        End If
    End Sub
  
    Protected Sub ddlOrganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddOraganisation.SelectedIndexChanged
        
        LoadCustomer()
        BindAssets()

    End Sub
    Sub LoadCustomer()

        Dim x As New DataTable
        x = objCommon.GetCustomerByCriteria(Err_No, Err_Desc, IIf(ddOraganisation.SelectedIndex <= 0, "0", Me.ddOraganisation.SelectedValue))

        Dim r As DataRow = x.NewRow()
        r(0) = "0"
        r(1) = ""

        x.Rows.InsertAt(r, 0)
        ddlCustomer.ClearSelection()
        ddlCustomer.Items.Clear()
        ddlCustomer.Text = ""
        ddlCustomer.SelectedIndex = 0
        ddlCustomer.DataValueField = "CustomerID"
        ddlCustomer.DataTextField = "Customer"
        ddlCustomer.DataSource = x
        ddlCustomer.DataBind()

    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Try
            MapWindow.VisibleOnPageLoad = False
            Resetfields()
        Catch

        End Try
    End Sub

    Public Sub Resetfields()
        Me.ddlCustomer.ClearSelection()
        Me.ddlCustomer.Text = ""
        Me.ddlCustomer.SelectedIndex = 0
        Me.ddlAssetType.SelectedIndex = 0
        Me.txtAssetCode.Enabled = True
        Me.txtAssetTypeId.Text = ""
        Me.txtDescription.Text = ""
        Me.ddlAssetType.SelectedIndex = 0
        Me.txtAssetCode.Text = ""
        Me.ChkActive.Checked = True
        Me.lblValMsg.Text = ""
        Me.lblUpMsg.Text = ""
        Me.btnSave.Text = "Save"
    End Sub



    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Me.ddlCustomer.SelectedIndex <= 0 Or Me.ddlAssetType.SelectedIndex <= 0 Then
            'Me.lblinfo.Text = "Validation"
            'Me.lblMessage.Text = "Please select a customer and asset type"
            'Me.lblMessage.ForeColor = Drawing.Color.Red
            'Me.MpInfoError.Show()
            'MapWindow.VisibleOnPageLoad = True
            Me.lblValMsg.Text = "Please select a customer and asset type"
            Me.lblValMsg.ForeColor = Drawing.Color.Red
            Me.MapWindow.VisibleOnPageLoad = True
            Exit Sub
        End If
        If Me.txtDescription.Text = "" Or Me.txtAssetCode.Text = "" Or Me.txtAssetCode.Text = "0" And Me.txtDescription.Text = "0" Then
            'Me.lblinfo.Text = "Validation"
            'Me.lblMessage.Text = "Please enter asset code/description."
            'Me.lblMessage.ForeColor = Drawing.Color.Red
            'Me.MpInfoError.Show()
            Me.lblValMsg.Text = "Please enter asset code/description."
            Me.lblValMsg.ForeColor = Drawing.Color.Red
            Me.MapWindow.VisibleOnPageLoad = True
            Exit Sub
        End If
      
        Dim CustID As Integer = 0
        Dim SiteID As Integer = 0

        If Me.ddlCustomer.SelectedIndex <= 0 Then
            CustID = 0
            SiteID = 0
        Else
            Dim s() As String = Me.ddlCustomer.SelectedValue.ToString().Split("$")
            If s.Length = 2 Then
                CustID = CInt(s(0).ToString())
                SiteID = CInt(s(1).ToString())
            End If
        End If

        Dim success As Boolean = False
        Try



            If objAssetType.CheckAssetNo(Err_No, Err_Desc, Me.txtAssetCode.Text, IIf(Me.txtAssetTypeId.Text = "", "0", Me.txtAssetTypeId.Text)) = False Then
                If objAssetType.InsertAssets(Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, Me.ddlAssetType.SelectedValue, CustID, SiteID, Me.txtAssetCode.Text, Me.txtDescription.Text, IIf(Me.ChkActive.Checked = True, "Y", "N")) = True Then
                    MapWindow.VisibleOnPageLoad = False
                    Me.lblMessage.Text = "Successfully saved."
                    lblMessage.ForeColor = Drawing.Color.Green
                    lblinfo.Text = "Information"
                    MpInfoError.Show()
                    btnClose.Focus()
                    BindAssets()
                    Resetfields()
                End If
            Else
                Me.lblValMsg.Text = "Same asset code already exist."
                Me.lblValMsg.ForeColor = Drawing.Color.Red
                Me.MapWindow.VisibleOnPageLoad = True
                'Me.lblMessage.Text = "Same asset code already exist."
                'lblMessage.ForeColor = Drawing.Color.Red
                'lblinfo.Text = "Validation"
                'MpInfoError.Show()
                'btnClose.Focus()
                'Me.ClassUpdatePnl.Update()
                Exit Sub
            End If
            'If success = True Then
            '    ' objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", "CURRENCY", Me.txtCurrencyCode.Text, "Code: " & Me.txtCurrencyCode.Text & "/ Desc :  " & Me.txtDescription.Text & "/ Rate:  " & Me.txtRate.Text & "/ Decimal : " & ddlDigits.SelectedValue.ToString(), CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")

            TopPanel.Update()


            'Else

            '    log.Error(Err_Desc)
            '    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("AssetType_001") & "&next=AssetType.aspx&Title=Message", False)
            '    Exit Try
            'End If

        Catch ex As Exception
            Err_No = "74061"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Private Sub LoadAssetTypes()
        Dim y As New DataTable
        y = objAssetType.FillAssetType(Err_No, Err_Desc)
        Dim r As DataRow = y.NewRow
        r(0) = "0"
        r(1) = "--Select--"
        y.Rows.InsertAt(r, 0)

        ddlAssetType.SelectedIndex = 0
        ddlAssetType.DataValueField = "Asset_Type_ID"
        ddlAssetType.DataTextField = "Description"
        ddlAssetType.DataSource = y
        ddlAssetType.DataBind()
    End Sub
    Private Sub BindAssets()
        'Dim CustID As Integer = 0
        'Dim SiteID As Integer = 0

        'If Me.ddlCustomer.SelectedIndex <= 0 Then
        '    CustID = 0
        '    SiteID = 0
        'Else
        '    Dim s() As String = Me.ddlCustomer.SelectedValue.ToString().Split("$")
        '    If s.Length = 2 Then
        '        CustID = CInt(s(0).ToString())
        '        SiteID = CInt(s(1).ToString())
        '    End If
        'End If

        Dt = objAssetType.FillAssets(Err_No, Err_Desc, IIf(Me.ddOraganisation.SelectedIndex <= 0, "0", Me.ddOraganisation.SelectedValue))


        If Me.ddlFilterBy.SelectedIndex = 1 Then
            Dt.DefaultView.RowFilter = " ( Customer_No LIKE '%" + txtFilterVal.Text + "%' )"
        End If

        If Me.ddlFilterBy.SelectedIndex = 2 Then
            Dt.DefaultView.RowFilter = " ( CustomerName LIKE '%" + txtFilterVal.Text + "%' )"
        End If

        If Me.ddlFilterBy.SelectedIndex = 3 Then
            Dt.DefaultView.RowFilter = " ( AssetType='" + txtFilterVal.Text + "' )"
        End If

        If Me.ddlFilterBy.SelectedIndex = 4 Then
            Dt.DefaultView.RowFilter = " ( Asset_Code='" + txtFilterVal.Text + "' )"
        End If
        If Me.ddlFilterBy.SelectedIndex = 5 Then
            Dt.DefaultView.RowFilter = "(Description LIKE '%" + txtFilterVal.Text + "%')"
        End If
        If Me.ddlFilterBy.SelectedIndex = 6 Then
            Dt.DefaultView.RowFilter = "(Is_Active = '" + txtFilterVal.Text + "')"
        End If

        Dim dv As New DataView(Dt)
        dv = Dt.DefaultView
        If ViewState("SortField") <> "" Then
            dv.Sort = (ViewState("SortField") & " ") + SortDirection
        End If
        gvAssets.DataSource = dv
        gvAssets.DataBind()
        Session.Remove("Assets")
        Session("Assets") = Dt
        Me.ClassUpdatePnl.Update()

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
        If Me.ddlCustomer.SelectedIndex <= 0 Or Me.ddlAssetType.SelectedIndex <= 0 Then
            'Me.lblinfo.Text = "Validation"
            'Me.lblMessage.Text = "Please select a customer and asset type"
            'Me.lblMessage.ForeColor = Drawing.Color.Red
            'Me.MpInfoError.Show()
            'MapWindow.VisibleOnPageLoad = True
            'Me.ClassUpdatePnl.Update()
            Me.lblValMsg.Text = "Please select a customer and asset type"
            Me.lblValMsg.ForeColor = Drawing.Color.Red
            Me.MapWindow.VisibleOnPageLoad = True
            Exit Sub
        End If
        If Me.txtDescription.Text = "" Or Me.txtAssetCode.Text = "" Or Me.txtAssetCode.Text = "0" And Me.txtDescription.Text = "0" Then
            'Me.lblinfo.Text = "Validation"
            'Me.lblMessage.Text = "Please enter asset code/description."
            'Me.lblMessage.ForeColor = Drawing.Color.Red
            'Me.MpInfoError.Show()
            'MapWindow.VisibleOnPageLoad = True
            'Me.ClassUpdatePnl.Update()
            Me.lblValMsg.Text = "Please enter asset code/description."
            Me.lblValMsg.ForeColor = Drawing.Color.Red
            Me.MapWindow.VisibleOnPageLoad = True
            Exit Sub
        End If

        Dim CustID As Integer = 0
        Dim SiteID As Integer = 0

        If Me.ddlCustomer.SelectedIndex <= 0 Then
            CustID = 0
            SiteID = 0
        Else
            Dim s() As String = Me.ddlCustomer.SelectedValue.ToString().Split("$")
            If s.Length = 2 Then
                CustID = CInt(s(0).ToString())
                SiteID = CInt(s(1).ToString())
            End If
        End If

        Dim success As Boolean = False
        Try

            If objAssetType.CheckAssetNo(Err_No, Err_Desc, Me.txtAssetCode.Text, IIf(Me.txtAssetTypeId.Text = "", "0", Me.txtAssetTypeId.Text)) = True Then
                'Me.lblMessage.Text = "Same asset code already exist."
                'lblMessage.ForeColor = Drawing.Color.Red
                'lblinfo.Text = "Validation"
                'MpInfoError.Show()
                'btnClose.Focus()
                'Me.ClassUpdatePnl.Update()
                Me.lblValMsg.Text = "Same asset code already exist."
                Me.lblValMsg.ForeColor = Drawing.Color.Red
                Me.MapWindow.VisibleOnPageLoad = True
                Exit Sub
            End If



            If objAssetType.UpdateAssets(Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, Me.txtAssetTypeId.Text, Me.ddlAssetType.SelectedValue, CustID, SiteID, Me.txtAssetCode.Text, Me.txtDescription.Text, IIf(Me.ChkActive.Checked = True, "Y", "N")) = True Then
                success = True
               
                MapWindow.VisibleOnPageLoad = False
                Me.lblMessage.Text = "Successfully updated."
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
                BindAssets()
                Resetfields()
            End If

            'If success = True Then

             TopPanel.Update()

            'Else

            '    log.Error(Err_Desc)
            '    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("SWX_002") & "&next=ManageAssets.aspx&Title=Message", False)
            '    Exit Try
            'End If

        Catch ex As Exception
            Err_No = "2062"
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
            For Each dr In gvAssets.Rows
                Dim RowCheckBox As System.Web.UI.WebControls.CheckBox = dr.FindControl("chkDelete")

                If RowCheckBox.Checked = True Then
                    Dim Lbl As System.Web.UI.WebControls.Label = dr.FindControl("lblCurrency")


                    If objAssetType.DeleteAssets(Err_No, Err_Desc, Lbl.Text, CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
                        Success = True
                    End If
                End If
            Next
            If (Success = True) Then
                lblMessage.Text = "Assets deleted successfully."
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()
                Dt = objAssetType.FillAssetType(Err_No, Err_Desc)
                BindAssets()
                Resetfields()
                Me.ClassUpdatePnl.Update()
            Else
                log.Error(Err_Desc)
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("SWX_003") & "&next=ManageAssets.aspx&Title=Message", False)
                Exit Try
            End If

            'ClassUpdatePnl.Update()
        Catch ex As Exception
            Err_No = "34063"
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
        Dim Lbl As System.Web.UI.WebControls.Label = row.FindControl("lblCurrency")
        Try
           
            If objAssetType.DeleteAssets(Err_No, Err_Desc, Lbl.Text, CType(Session.Item("USER_ACCESS"), UserAccess).UserID) = True Then
                success = True
            End If

            If success = True Then
                lblMessage.Text = "Successfully deleted."
                lblMessage.ForeColor = Drawing.Color.Green
                lblinfo.Text = "Information"
                MpInfoError.Show()
                btnClose.Focus()

                BindAssets()
                Resetfields()
                Me.ClassUpdatePnl.Update()
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

        If Me.ddOraganisation.SelectedIndex <= 0 Then
            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Please select organization"
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Me.ClassUpdatePnl.Update()
            Exit Sub
        End If
        btnUpdate.Visible = False
        btnSave.Visible = True
        Resetfields()
        ClassUpdatePnl.Update()
        Me.MapWindow.VisibleOnPageLoad = True
    End Sub


    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try

            btnUpdate.Visible = True
            btnSave.Visible = False
            Resetfields()
            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
            Dim lblAssetId As Label = DirectCast(row.FindControl("lblCurrency"), Label)
            Me.txtAssetTypeId.Text = lblAssetId.Text
            Dim x As New DataTable
            x = objAssetType.GetAssetByID(Err_No, Err_Desc, lblAssetId.Text)

            If x.Rows.Count > 0 Then
                Me.ddlAssetType.SelectedValue = IIf(x.Rows(0)("Asset_type_ID") Is DBNull.Value, "0", x.Rows(0)("Asset_type_ID").ToString())
                Me.ddlCustomer.SelectedValue = x.Rows(0)("Customer_ID").ToString() + "$" + x.Rows(0)("Site_Use_ID").ToString()
                If x.Rows(0)("is_Active").ToString() = "Y" Then
                    Me.ChkActive.Checked = True
                Else
                    Me.ChkActive.Checked = False
                End If
                Me.txtAssetCode.Text = x.Rows(0)("Asset_Code").ToString()
                Me.txtDescription.Text = x.Rows(0)("Description").ToString()
            End If
            Me.txtAssetCode.Enabled = False

         
            Me.MapWindow.VisibleOnPageLoad = True
        Catch ex As Exception
            Err_No = "74066"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("SWX_001") & "&next=ManageAssets.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub

    Private Sub gvAssets_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvAssets.PageIndexChanging
        gvAssets.PageIndex = e.NewPageIndex

        BindAssets()

    End Sub

    Private Sub gvAssets_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAssets.RowDataBound
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
    Private Sub gvAssets_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvAssets.Sorting
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"

        BindAssets()
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


   


    Protected Sub Clear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Clear.Click
        Me.ddlFilterBy.SelectedIndex = 0
        Me.txtFilterVal.Text = ""
        ddlCustomer.ClearSelection()
        ddlCustomer.Items.Clear()
        ddlCustomer.Text = ""
        BindAssets()
        ClassUpdatePnl.Update()
    End Sub
   
  

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
       
        BindAssets()
        ClassUpdatePnl.Update()
       
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
        dtErrors = Session("dtAssetsErrors")
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
                        col.ColumnName = "CustomerNo"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)

                        col = New DataColumn
                        col.ColumnName = "AssetTypeID"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)

                        col = New DataColumn
                        col.ColumnName = "AssetCode"
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

                        col = New DataColumn
                        col.ColumnName = "CustomerName"
                        col.DataType = System.Type.GetType("System.String")
                        col.ReadOnly = False
                        col.Unique = False
                        TempTbl.Columns.Add(col)


                        col = New DataColumn
                        col.ColumnName = "AssetType"
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

                        If TempTbl.Columns.Count = 6 Then

                            If Not (TempTbl.Columns(0).ColumnName = "CustomerNo" And TempTbl.Columns(1).ColumnName = "AssetTypeID" And TempTbl.Columns(2).ColumnName = "AssetCode" And TempTbl.Columns(3).ColumnName = "Description" And TempTbl.Columns(4).ColumnName = "CustomerName" And TempTbl.Columns(5).ColumnName = "AssetType") Then
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
                        TempTbl.Columns.Add("CustomerID", GetType(String))
                        TempTbl.Columns.Add("SiteID", GetType(String))



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
                        Dim OrgID As String = Nothing



                        If TempTbl.Rows.Count > 0 Then



                            Dim idx As Integer

                            For idx = 0 To TempTbl.Rows.Count - 1

                                Dim CustomerNo As String = Nothing
                                Dim AssetTypeID As String = Nothing
                                Dim AssetCode As String = Nothing
                                Dim Description As String = Nothing
                                Dim CustID As String = Nothing
                                Dim SiteID As String = Nothing

                                OrgID = Me.ddOraganisation.SelectedValue

                                Dim isValidRow As Boolean = True


                                If TempTbl.Rows(idx)(0) Is DBNull.Value Or TempTbl.Rows(idx)(1) Is DBNull.Value Or TempTbl.Rows(idx)(2) Is DBNull.Value Or TempTbl.Rows(idx)(3) Is DBNull.Value Then
                                    TempTbl.Rows(idx)("IsValid") = "N"
                                    Continue For
                                End If



                                CustomerNo = IIf(TempTbl.Rows(idx)(0) Is DBNull.Value, "0", TempTbl.Rows(idx)(0).ToString())
                                AssetTypeID = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, "0", TempTbl.Rows(idx)(1).ToString())
                                AssetCode = IIf(TempTbl.Rows(idx)(2) Is DBNull.Value, "0", TempTbl.Rows(idx)(2).ToString())
                                Description = IIf(TempTbl.Rows(idx)(3) Is DBNull.Value, "0", TempTbl.Rows(idx)(3).ToString())


                                If AssetTypeID = "0" Or CustomerNo = "0" Or AssetCode = "0" Or Description = "0" Then
                                    TempTbl.Rows(idx)("IsValid") = "N"
                                    Continue For
                                End If


                                Dim row As DataRow
                                row = objAssetType.GetCustomerandSiteID(Err_No, Err_Desc, CustomerNo, Me.ddOraganisation.SelectedValue)
                                If Not row Is Nothing Then
                                    CustID = row.Item(0).ToString()
                                    SiteID = row.Item(1).ToString()
                                End If

                                If CustID Is Nothing Or SiteID Is Nothing Then
                                    RowNo = idx + 2
                                    TempTbl.Rows(idx)("IsValid") = "N"
                                    TempTbl.Rows(idx)("CustomerID") = "0"
                                    TempTbl.Rows(idx)("SiteID") = "0"
                                    ErrorText = "Invalid customer no"
                                    TotFailed += 1
                                    isValidRow = False
                                End If

                                If IsNumeric(AssetTypeID) = False Then
                                    RowNo = idx + 2
                                    TempTbl.Rows(idx)("IsValid") = "N"
                                    TempTbl.Rows(idx)("CustomerID") = "0"
                                    TempTbl.Rows(idx)("SiteID") = "0"
                                    ErrorText = "Asset type id should be in numeric"
                                    TotFailed += 1
                                    isValidRow = False
                                End If

                                If objAssetType.CheckValidAssetTypeID(Err_No, Err_Desc, AssetTypeID) = False Then
                                    RowNo = idx + 2
                                    TempTbl.Rows(idx)("IsValid") = "N"
                                    ErrorText = "Invalid  asset type id"
                                    TempTbl.Rows(idx)("CustomerID") = "0"
                                    TempTbl.Rows(idx)("SiteID") = "0"
                                    TotFailed += 1
                                    isValidRow = False
                                End If

                                If objAssetType.CheckAssetNo(Err_No, Err_Desc, AssetCode, "0") = True And chkUpdate.Checked = False Then
                                    RowNo = idx + 2
                                    TempTbl.Rows(idx)("IsValid") = "N"
                                    ErrorText = "Same asset code already exist "
                                    TempTbl.Rows(idx)("CustomerID") = "0"
                                    TempTbl.Rows(idx)("SiteID") = "0"
                                    TotFailed += 1
                                    isValidRow = False
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
                                    TempTbl.Rows(idx)("CustomerID") = CustID
                                    TempTbl.Rows(idx)("SiteID") = SiteID
                                    TotSuccess = TotSuccess + 1
                                    Dim h As DataRow = dtErrors.NewRow()
                                    h("RowNo") = idx + 2
                                    h("LogInfo") = "Successfully uploaded"
                                    dtErrors.Rows.Add(h)
                                    RowNo = Nothing
                                    ErrorText = Nothing
                                    isValidRow = True
                                End If





                            Next
                        End If

                        If objAssetType.UploadAssetsToCustomer(TempTbl, Me.ddOraganisation.SelectedValue, Err_No, Err_Desc, CType(Session("User_Access"), UserAccess).UserID) = True Then
                            DeleteExcel()
                            lblUpMsg.Text = IIf(TotFailed = 0, "Successfully uploaded", "One or more rows has invalid data. Please check the log")
                            MPEImport.Show()
                            BindAssets()

                        Else
                            DeleteExcel()
                            lblUpMsg.Text = "Please check the uploaded log"

                            MPEImport.Show()
                            Exit Sub
                        End If
                    End If


                    Me.dgvErros.DataSource = dtErrors
                    Me.dgvErros.DataBind()
                    Session.Remove("dtAssetsErrors")
                    Session("dtAssetsErrors") = dtErrors


                    Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & "Assets_" & Now.ToString("yyyyMMdd") + ".txt"
                    DataTable2CSV(dtErrors, fn, vbTab)

                    Session.Remove("AssetsLogInfo")
                    Session("AssetsLogInfo") = fn




                Catch ex As Exception

                    Err_No = "56752"
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
        If Not Session("AssetsLogInfo") Is Nothing Then
            Dim fileValue As String = Session("AssetsLogInfo")





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

        Session.Remove("dtAssetsErrors")
        Session("dtAssetsErrors") = dtErrors
    End Sub
    Protected Sub btnImportWindow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportWindow.Click
        If Me.ddOraganisation.SelectedIndex > 0 Then
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

        If Me.ddOraganisation.SelectedIndex > 0 Then
            Dim dtOriginal As New DataTable()
            Dim OrgID As String = Me.ddOraganisation.SelectedValue

            dtOriginal = objAssetType.FillAssets(Err_No, Err_Desc, OrgID)
            
            Dim dtTemp As New DataTable()

            'Creating Header Row
            dtTemp.Columns.Add("CustomerNo")
            dtTemp.Columns.Add("AssetTypeID")
            dtTemp.Columns.Add("AssetCode")
            dtTemp.Columns.Add("Description")
            dtTemp.Columns.Add("CustomerName")
            dtTemp.Columns.Add("AssetType")


            Dim drAddItem As DataRow
            For i As Integer = 0 To dtOriginal.Rows.Count - 1
                drAddItem = dtTemp.NewRow()
                drAddItem(0) = dtOriginal.Rows(i)("Customer_No").ToString()
                drAddItem(1) = dtOriginal.Rows(i)("AssetTypeID").ToString()
                drAddItem(2) = dtOriginal.Rows(i)("Asset_Code").ToString()
                drAddItem(3) = dtOriginal.Rows(i)("Description").ToString()
                drAddItem(4) = dtOriginal.Rows(i)("CustomerName").ToString()
                drAddItem(5) = dtOriginal.Rows(i)("AssetType").ToString()
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
                dtTemp.Rows.Add(drAddItem)
            End If

            'Temp(Grid)
            Dim dg As New DataGrid()
            dg.DataSource = dtTemp
            dg.DataBind()
            If dtTemp.Rows.Count > 0 Then
                'Dim fn As String = "MSL" & Now.ToString("ddMMMyyHHmmss") + ".xls"
                Dim fn As String = "AssetsToCustomer" + ".xls"
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