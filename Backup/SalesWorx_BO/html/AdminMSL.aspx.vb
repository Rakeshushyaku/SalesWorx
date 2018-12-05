Imports SalesWorx.BO.Common
Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO

Partial Public Class AdminMSL
    Inherits System.Web.UI.Page
    Dim objProduct As New Product
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const PageID As String = "P8"
    Dim objLogin As New SalesWorx.BO.Common.Login
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub AdminMSL_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Must Stock List Management"
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
                Dim objCommon As New Common
                Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddOraganisation.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddOraganisation.Items.Clear()
                ddOraganisation.Items.Add("-- Select a Organization --")
                ddOraganisation.AppendDataBoundItems = True
                ddOraganisation.DataValueField = "MAS_Org_ID"
                ddOraganisation.DataTextField = "Description"
                ddOraganisation.DataBind()

                Me.ddlSalesRep.Items.Clear()
                ddlSalesRep.AppendDataBoundItems = True
                ddlSalesRep.Items.Insert(0, "--Select a van--")
                ddlSalesRep.Items(0).Value = ""

                lstDefault.Items.Clear()
                lstSelected.Items.Clear()

                ''Bind Default List

            End If
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_MSL_005") & "&next=Welcome.aspx&Title=Product List Setup", False)
        End Try

    End Sub

    Private Sub ddOraganisation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddOraganisation.SelectedIndexChanged
        'Try
        If ddOraganisation.SelectedItem.Text <> "-- Select a Organization --" Then
            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim objCommon As New Common
            ddlSalesRep.DataSource = objCommon.GetVanByOrg(Err_No, Err_Desc, ddOraganisation.SelectedValue, objUserAccess.UserID.ToString())
            ddlSalesRep.Items.Clear()
            ddlSalesRep.Items.Add("-- Select a van --")
            ddlSalesRep.AppendDataBoundItems = True
            ddlSalesRep.DataValueField = "SalesRep_Id"
            ddlSalesRep.DataTextField = "SalesRep_Name"
            ddlSalesRep.DataBind()
        Else
            ddlSalesRep.Items.Clear()
            ddlSalesRep.Items.Add("-- Select a van--")
            ddlSalesRep.AppendDataBoundItems = True
            ddlSalesRep.DataValueField = "SalesRep_Id"
            ddlSalesRep.DataTextField = "SalesRep_Name"
            ddlSalesRep.DataBind()
            lstDefault.Items.Clear()
            lstSelected.Items.Clear()
            lblProdAssign.Text = ""
            lblProdAvailed.Text = ""
        End If
        ' Catch ex As Exception
        'log.Error(ex.Message)
        ' Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=Welcome.aspx&Title=Product List Setup", False)
        ' End Try
    End Sub
    Private Sub BindDefault()
        Dim objProd As New Product
        Dim TempTbl As New DataTable
        Try
            Err_Desc = Nothing
            Err_No = Nothing
           
            TempTbl = objProd.GetDefault(ddOraganisation.SelectedItem.Value, ddlSalesRep.SelectedItem.Value, Err_No, Err_Desc)
            If TempTbl IsNot Nothing Then
                lstDefault.DataSource = TempTbl
                lstDefault.DataTextField = "Description"
                lstDefault.DataValueField = "Inventory_Item_ID"
                lstDefault.DataBind()
            End If

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        Finally
            objProd = Nothing
        End Try
    End Sub

    Private Sub BindSelected()
        Dim objProd As New Product
        Dim TempTbl As New DataTable
        Try
            Err_Desc = Nothing
            Err_No = Nothing
            TempTbl = objProd.GetSelected(ddOraganisation.SelectedItem.Value, ddlSalesRep.SelectedItem.Value, Err_No, Err_Desc)
            If TempTbl IsNot Nothing Then
                lstSelected.DataSource = TempTbl
                lstSelected.DataTextField = "Description"
                lstSelected.DataValueField = "Inventory_Item_ID"
                lstSelected.DataBind()
            End If
            If lstSelected.Items.Count > 0 Then
                Me.btnRemoveAll.Enabled = True
            Else
                Me.btnRemoveAll.Enabled = False
            End If
            lblProdAssign.Text = "Products Assigned: [" & lstSelected.Items.Count & "]"
            lblProdAvailed.Text = "Products Available: [" & lstDefault.Items.Count & "]"
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        Finally
            objProd = Nothing
        End Try
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If Me.ddOraganisation.SelectedIndex > 0 And Me.ddlSalesRep.SelectedIndex > 0 Then
            Try
                Err_No = Nothing
                Err_Desc = Nothing
                ' Dim s As String() = ddlSalesRep.SelectedItem.Text.Split("-")
                Dim VanID As String = ddlSalesRep.SelectedValue
                ' If s.Length > 1 Then
                'VanID = s(1)
                ' End If
                Dim objProd As New Product
                For Each Item As ListItem In lstDefault.Items
                    If Item.Selected Then
                        objProd.InsertProduct(ddOraganisation.SelectedItem.Value, Item.Value, 0, ddlSalesRep.SelectedItem.Value, Err_No, Err_Desc)
                        objLogin.SaveUserLog(Err_No, Err_Desc, "I", "PRODUCT MANAGEMENT", "MSL MANAGEMENT", VanID.Trim(), "Item : " & Item.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), VanID.Trim(), Me.ddOraganisation.SelectedValue.ToString())
                    End If
                Next
                BindDefault()
                BindSelected()
            Catch ex As Exception
                log.Error(ex.Message)
                Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_AdminMSL_005") & "&next=AdminMSL.aspx&Title=Must Stock List", False)
            End Try
        Else
            Me.lblinfo.Text = "Information"
            Me.lblMessage.Text = "Please select organization/Van."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Exit Sub
        End If
    End Sub

    Protected Sub btnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        If Me.ddOraganisation.SelectedIndex > 0 And Me.ddlSalesRep.SelectedIndex > 0 Then
            Try
                Err_No = Nothing
                Err_Desc = Nothing
                ' Dim s As String() = ddlSalesRep.SelectedItem.Text.Split("-")
                Dim VanID As String = ddlSalesRep.SelectedValue
                ' If s.Length > 1 Then
                'VanID = s(1)
                ' End If
                Dim objProd As New Product
                For Each Item As ListItem In lstSelected.Items
                    If Item.Selected Then
                        objProd.DeleteProduct(ddOraganisation.SelectedItem.Value, Item.Value, 0, ddlSalesRep.SelectedItem.Value, Err_No, Err_Desc)
                        objLogin.SaveUserLog(Err_No, Err_Desc, "D", "PRODUCT MANAGEMENT", "MSL MANAGEMENT", VanID.Trim(), "Item : " & Item.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), VanID.Trim(), Me.ddOraganisation.SelectedValue.ToString())
                    End If
                Next
                BindDefault()
                BindSelected()
            Catch ex As Exception
                log.Error(ex.Message)
                Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_AdminMSL_006") & "&next=AdminMSL.aspx&Title=Must Stock List", False)

            End Try
        Else
            Me.lblinfo.Text = "Information"
            Me.lblMessage.Text = "Please select organization/Van."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Exit Sub
        End If
    End Sub

    Protected Sub btnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImport.Click
        Me.MPEImport.Show()
        Resetfields()
    End Sub


    Protected Sub btnImportSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportSave.Click
        If Me.ExcelFileUpload.FileName = Nothing Or (Me.rbAppend.Checked = False And Me.rbRebuild.Checked = False) Then
            Me.lblinfo.Text = "Validation"
            Me.lblMessage.Text = "Select filename and rebuild/append data"
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
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
                lblMessage.Text = Str.ToString()
                lblMessage.ForeColor = Drawing.Color.Red
                lblinfo.Text = "Information"
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
                col = New DataColumn
                col.ColumnName = "VanId"
                col.DataType = System.Type.GetType("System.String")
                col.ReadOnly = False
                col.Unique = False
                TempTbl.Columns.Add(col)


                col = New DataColumn()
                col.ColumnName = "ItemCode"
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
                    lblMessage.Text = "There is no data in your file."
                    lblMessage.ForeColor = Drawing.Color.Red
                    lblinfo.Text = "Information"
                    MpInfoError.Show()
                    Exit Sub
                End If
                Dim ICode As String
                Dim VanId As Integer = 0
                Dim OrgId As String = Nothing
                If TempTbl.Rows.Count > 0 Then
                    Dim dtVan As New DataTable
                    dtVan = objProduct.GetSalesRepID(Err_No, Err_Desc, IIf(TempTbl.Rows(0)(0) Is DBNull.Value, "0", TempTbl.Rows(0)(0).ToString()))
                    If dtVan.Rows.Count > 0 Then
                        VanId = Integer.Parse(dtVan.Rows(0)(0).ToString())
                        OrgId = dtVan.Rows(0)(1).ToString()
                    End If
                    Dim idx As Integer
                    If VanId <> 0 And OrgId <> Nothing Then
                        If Me.rbAppend.Checked = True Then
                            For idx = 0 To TempTbl.Rows.Count - 1

                                ICode = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, "0", TempTbl.Rows(idx)(1).ToString())
                                If objProduct.CheckProductValid("tblProduct", ICode, OrgId, VanId, Err_No, Err_Desc) = True Then
                                    If objProduct.CheckProductValid("tblMSL", ICode, OrgId, VanId, Err_No, Err_Desc) = False Then
                                        If ICode <> "0" And OrgId <> "0" Then
                                            objProduct.InsertProduct(OrgId, 0, ICode, VanId, Err_No, Err_Desc)
                                            objLogin.SaveUserLog(Err_No, Err_Desc, "X", "PRODUCT MANAGEMENT", "MSL MANAGEMENT", VanId, ICode, CType(Session("User_Access"), UserAccess).UserID.ToString(), VanId, OrgId)
                                            st = True
                                        End If
                                    Else
                                        st = True
                                    End If
                                End If
                            Next

                        ElseIf Me.rbRebuild.Checked = True Then
                            objProduct.RebuildAllProduct(Err_No, Err_Desc, VanId)
                            For idx = 0 To TempTbl.Rows.Count - 1
                                ICode = IIf(TempTbl.Rows(idx)(1) Is DBNull.Value, "0", TempTbl.Rows(idx)(1).ToString())
                                If objProduct.CheckProductValid("tblProduct", ICode, OrgId, VanId, Err_No, Err_Desc) = True Then
                                    If objProduct.CheckProductValid("tblMSL", ICode, OrgId, VanId, Err_No, Err_Desc) = False Then
                                        If ICode <> "0" And OrgId <> "0" Then
                                            objProduct.InsertProduct(OrgId, 0, ICode, VanId, Err_No, Err_Desc)
                                            objLogin.SaveUserLog(Err_No, Err_Desc, "X", "PRODUCT MANAGEMENT", "MSL MANAGEMENT", VanId, ICode, CType(Session("User_Access"), UserAccess).UserID.ToString(), VanId, OrgId)
                                            st = True
                                        End If
                                    End If
                                End If
                            Next

                        End If
                    End If
                    If st = True Then
                        DeleteExcel()
                        Me.MPEImport.Hide()
                        lblMessage.Text = "Successfully imported."
                        lblMessage.ForeColor = Drawing.Color.Green
                        lblinfo.Text = "Information"
                        MpInfoError.Show()
                        btnClose.Focus()
                        Resetfields()
                    Else
                        Me.MPEImport.Hide()
                        lblMessage.Text = "Please verify the excel data."
                        lblMessage.ForeColor = Drawing.Color.Red
                        lblinfo.Text = "Information"
                        MpInfoError.Show()
                        btnClose.Focus()
                        Resetfields()
                        'log.Error(Err_Desc)
                        'Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_AdminMSL_006") & "&next=AdminMSL.aspx&Title=Message", False)
                        Exit Try
                    End If
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



    Public Sub Resetfields()
        Me.ddOraganisation.SelectedIndex = 0
        Me.ddlSalesRep.Items.Clear()
        ddlSalesRep.AppendDataBoundItems = True
        ddlSalesRep.Items.Insert(0, "--Select a van--")
        ddlSalesRep.Items(0).Value = ""
        Me.lstDefault.Items.Clear()
        Me.lstSelected.Items.Clear()
        Me.rbAppend.Checked = False
        Me.rbRebuild.Checked = False
        Me.ddOraganisation.Focus()
        Me.lblProdAssign.Text = ""
        Me.lblProdAvailed.Text = ""
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

    Protected Sub btnAddAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddAll.Click
        If Me.ddOraganisation.SelectedIndex > 0 And Me.ddlSalesRep.SelectedIndex > 0 Then
            Try
                Err_No = Nothing
                Err_Desc = Nothing
                '  Dim s As String() = ddlSalesRep.SelectedItem.Text.Split("-")
                Dim VanID As String = ddlSalesRep.SelectedValue
                '  If s.Length > 1 Then
                'VanID = s(1)
                '  End If
                Dim objProd As New Product
                For Each Item As ListItem In lstDefault.Items
                    objProd.InsertProduct(ddOraganisation.SelectedItem.Value, Item.Value, 0, ddlSalesRep.SelectedItem.Value, Err_No, Err_Desc)
                    objLogin.SaveUserLog(Err_No, Err_Desc, "I", "PRODUCT MANAGEMENT", "MSL MANAGEMENT", VanID.Trim(), "Item : " & Item.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), VanID.ToString(), Me.ddOraganisation.SelectedValue.ToString())
                Next
                BindDefault()
                BindSelected()
            Catch ex As Exception
                log.Error(ex.Message)
                Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_AdminMSL_005") & "&next=AdminMSL.aspx&Title=Must Stock List", False)
            End Try
        Else
            Me.lblinfo.Text = "Information"
            Me.lblMessage.Text = "Please select organization/Van."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Exit Sub
        End If
    End Sub

    Protected Sub btnRemoveAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRemoveAll.Click
        If Me.ddOraganisation.SelectedIndex > 0 And Me.ddlSalesRep.SelectedIndex > 0 Then
            Try
                Err_No = Nothing
                Err_Desc = Nothing
                ' Dim s As String() = ddlSalesRep.SelectedItem.Text.Split("-")
                Dim VanID As String = ddlSalesRep.SelectedValue
                ' If s.Length > 1 Then
                'VanID = s(1)
                ' End If
                Dim objProd As New Product
                For Each Item As ListItem In lstSelected.Items
                    objProd.DeleteProduct(ddOraganisation.SelectedItem.Value, Item.Value, 0, ddlSalesRep.SelectedItem.Value, Err_No, Err_Desc)
                    objLogin.SaveUserLog(Err_No, Err_Desc, "D", "PRODUCT MANAGEMENT", "MSL MANAGEMENT", VanID.Trim(), "Item : " & Item.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), VanID.Trim(), Me.ddOraganisation.SelectedValue.ToString())
                Next
                BindDefault()
                BindSelected()
            Catch ex As Exception
                log.Error(ex.Message)
                Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_AdminMSL_006") & "&next=AdminMSL.aspx&Title=Must Stock List", False)

            End Try
        Else
            Me.lblinfo.Text = "Information"
            Me.lblMessage.Text = "Please select organization/Van."
            Me.lblMessage.ForeColor = Drawing.Color.Red
            Me.MpInfoError.Show()
            Exit Sub

        End If
    End Sub

    Protected Sub ddlSalesRep_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlSalesRep.SelectedIndexChanged
        Try
            If ddlSalesRep.SelectedItem.Text <> "-- Select a van --" Then
                ''Bind Default List
                BindDefault()

                ''Bind Selected List
                BindSelected()

                lblProdAssign.Text = "Products Assigned: [" & lstSelected.Items.Count & "]"
                lblProdAvailed.Text = "Products Available: [" & lstDefault.Items.Count & "]"
            Else
                lstDefault.Items.Clear()
                lstSelected.Items.Clear()
                lblProdAssign.Text = ""
                lblProdAvailed.Text = ""
            End If
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=Welcome.aspx&Title=Product List Setup", False)
        End Try
        
    End Sub
End Class