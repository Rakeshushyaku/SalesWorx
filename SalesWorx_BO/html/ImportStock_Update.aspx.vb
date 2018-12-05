

Imports System.Data.OleDb
Imports System.IO
Imports log4net
Imports System.Data.SqlClient
Imports SalesWorx.BO.Common
Imports ExcelLibrary.SpreadSheet
Imports Telerik.Web.UI
Public Class ImportStock_Update
    Inherits System.Web.UI.Page

    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ObjCommon As Common
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim ObjStock As New SalesWorx.BO.Common.Stock
    Private Const ModuleName As String = "ImportStock_Update.aspx"
    Private Const PageID As String = "P304"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Not Page.IsPostBack() Then
                BTn_StockConfirm.Visible = False
                btnFilter.Visible = False
                If Session.Item("USER_ACCESS") Is Nothing Then
                    Session.Add("BringmeBackHere", ModuleName)
                    Response.Redirect("Login.aspx", False)
                    Exit Sub
                End If
                If Not HasAuthentication() Then
                    Err_No = 500
                    Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
                End If
                Filldropdowns()
                ViewState("Criteria") = "1=1"
                Session.Remove("Stock_Unconfirm")
            End If
            RegisterPostBackControl()
            lblmsgPopUp.Text = ""
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub


    Protected Sub btnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTn_Import.Click
        Try


            btnSave.Text = "Import"
            file_import.Enabled = True
            btnSave.Visible = True
            '   ddl_Organization.ClearSelection()
            dgvErros.DataSource = Nothing
            dgvErros.DataBind()
            BtnDownLoad.Visible = False
            Me.MPEDivConfig.Show()
            Session("Errordt") = Nothing
            ClassUpdatePnl.Update()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Function ImportFile(ByVal File As String, ByRef RetMessage As String) As Boolean
        Try


            log.Info("Van stock Import " & File & " started by " & CType(Session("User_Access"), UserAccess).UserID)
            Dim TempTbl As New DataTable
            Dim FinalTbl As New DataTable
            Dim ErrorTbl As New DataTable
            Dim bRetVal As Boolean = False
            Dim bImported As Boolean = True
            If File.ToLower.EndsWith(".xls") Then
                TempTbl = DoXLSUpload(File, bRetVal)
            ElseIf File.ToLower.EndsWith(".xlsx") Then
                TempTbl = DoXLSXUpload(File, bRetVal)
            End If
            If bRetVal = True Then

                If TempTbl.Rows.Count = 0 Or TempTbl Is Nothing Then
                    ' MessageBoxValidation("Invalid file Template123", "Validation")
                    '   ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", ExcelFileUpload.ClientID), True)
                    RetMessage = "Invalid file Template"
                    Exit Function
                End If

                ErrorTbl = TempTbl.Clone
                ErrorTbl.Columns.Add("Reason")
                ErrorTbl.Columns.Add("RowNo")




                FinalTbl.Columns.Add("Van_Code")
                FinalTbl.Columns.Add("Item_Code")
                FinalTbl.Columns.Add("Lot_Number")
                FinalTbl.Columns.Add("Expiry_Date")
                FinalTbl.Columns.Add("Lot_Qty")
                FinalTbl.Columns.Add("SalesRep_ID")
                FinalTbl.Columns.Add("Item_ID")
                FinalTbl.Columns.Add("UOMQty")
                FinalTbl.Columns.Add("Item_UOM")


                If TempTbl.Rows.Count > 0 Then
                    Dim Rowno As Integer = 2
                    For Each dr In TempTbl.Rows
                        Dim Van As String
                        Dim SalesRep_Number As String = ""
                        Van = dr(0).ToString.Trim()

                        Dim Item As String
                        Dim Item_ID As String = ""
                        Item = dr(1).ToString.Trim()

                        Dim Lot_number As String
                        Lot_number = dr(2).ToString.Trim()



                        Dim Ex_date As String
                        Ex_date = dr(3).ToString.Trim()

                        Dim qty As String
                        qty = dr(4).ToString.Trim()

                        Dim UOM As String
                        UOM = dr(5).ToString.Trim()

                        Dim Conversion As String = "1"
                        'UOM = dr(3).ToString
                        If Van = "" Then
                            Dim ErrorDr As DataRow
                            ErrorDr = ErrorTbl.NewRow
                            ErrorDr(0) = dr(0)
                            ErrorDr(1) = dr(1)
                            ErrorDr(2) = dr(2)
                            ErrorDr(3) = dr(3)
                            ErrorDr(4) = dr(4)
                            ErrorDr(5) = dr(5)
                            ErrorDr(6) = " Van code Can't be empty"
                            ErrorDr(7) = Rowno
                            ErrorTbl.Rows.Add(ErrorDr)
                            bImported = False

                        ElseIf Item = "" Then
                            Dim ErrorDr As DataRow
                            ErrorDr = ErrorTbl.NewRow
                            ErrorDr(0) = dr(0)
                            ErrorDr(1) = dr(1)
                            ErrorDr(2) = dr(2)
                            ErrorDr(3) = dr(3)
                            ErrorDr(4) = dr(4)
                            ErrorDr(5) = dr(5)
                            ErrorDr(6) = "Item code Can't be empty"
                            ErrorDr(7) = Rowno
                            ErrorTbl.Rows.Add(ErrorDr)
                            bImported = False
                        ElseIf IsValidItem(ddl_Organization.SelectedItem.Value, Item) = False Then
                            Dim ErrorDr As DataRow
                            ErrorDr = ErrorTbl.NewRow
                            ErrorDr(0) = dr(0)
                            ErrorDr(1) = dr(1)
                            ErrorDr(2) = dr(2)
                            ErrorDr(3) = dr(3)
                            ErrorDr(4) = dr(4)
                            ErrorDr(5) = dr(5)
                            ErrorDr(6) = "Inactive/Invalid product"
                            ErrorDr(7) = Rowno
                            ErrorTbl.Rows.Add(ErrorDr)
                            bImported = False

                        ElseIf Val(qty) = 0 Then
                            Dim ErrorDr As DataRow
                            ErrorDr = ErrorTbl.NewRow
                            ErrorDr(0) = dr(0)
                            ErrorDr(1) = dr(1)
                            ErrorDr(2) = dr(2)
                            ErrorDr(3) = dr(3)
                            ErrorDr(4) = dr(4)
                            ErrorDr(5) = dr(5)
                            ErrorDr(6) = "Invalid Qty"
                            ErrorDr(7) = Rowno
                            ErrorTbl.Rows.Add(ErrorDr)
                            bImported = False
                        ElseIf Not ValidVan(Van, SalesRep_Number) Then
                            Dim ErrorDr As DataRow
                            ErrorDr = ErrorTbl.NewRow
                            ErrorDr(0) = dr(0)
                            ErrorDr(1) = dr(1)
                            ErrorDr(2) = dr(2)
                            ErrorDr(3) = dr(3)
                            ErrorDr(4) = dr(4)
                            ErrorDr(5) = dr(5)
                            ErrorDr(6) = "Van does not exist"
                            ErrorDr(7) = Rowno
                            ErrorTbl.Rows.Add(ErrorDr)
                            bImported = False
                        ElseIf HasLots(Item) And Lot_number = "" Then


                            Dim ErrorDr As DataRow
                            ErrorDr = ErrorTbl.NewRow
                            ErrorDr(0) = dr(0)
                            ErrorDr(1) = dr(1)
                            ErrorDr(2) = dr(2)
                            ErrorDr(3) = dr(3)
                            ErrorDr(4) = dr(4)
                            ErrorDr(5) = dr(5)
                            ErrorDr(6) = "Lot Number Cannot be empty "
                            ErrorDr(7) = Rowno
                            ErrorTbl.Rows.Add(ErrorDr)
                            bImported = False
                        ElseIf HasLots(Item) And Ex_date.Trim() = "" Then
                            Dim ErrorDr As DataRow
                            ErrorDr = ErrorTbl.NewRow
                            ErrorDr(0) = dr(0)
                            ErrorDr(1) = dr(1)
                            ErrorDr(2) = dr(2)
                            ErrorDr(3) = dr(3)
                            ErrorDr(4) = dr(4)
                            ErrorDr(5) = dr(5)
                            ErrorDr(6) = "Invalid Expiry Date"
                            ErrorDr(7) = Rowno
                            ErrorTbl.Rows.Add(ErrorDr)
                            bImported = False
                        ElseIf HasLots(Item) And IsDate(Ex_date) = False Then

                            Dim ErrorDr As DataRow
                            ErrorDr = ErrorTbl.NewRow
                            ErrorDr(0) = dr(0)
                            ErrorDr(1) = dr(1)
                            ErrorDr(2) = dr(2)
                            ErrorDr(3) = dr(3)
                            ErrorDr(4) = dr(4)
                            ErrorDr(5) = dr(5)
                            ErrorDr(6) = "Invalid Expiry Date"
                            ErrorDr(7) = Rowno
                            ErrorTbl.Rows.Add(ErrorDr)
                            bImported = False

                        ElseIf HasLots(Item) And Lot_number.Length > 50 Then
                            Dim ErrorDr As DataRow
                            ErrorDr = ErrorTbl.NewRow
                            ErrorDr(0) = dr(0)
                            ErrorDr(1) = dr(1)
                            ErrorDr(2) = dr(2)
                            ErrorDr(3) = dr(3)
                            ErrorDr(4) = dr(4)
                            ErrorDr(5) = dr(5)
                            ErrorDr(6) = "Lot number beyond the length"
                            ErrorDr(7) = Rowno
                            ErrorTbl.Rows.Add(ErrorDr)
                            bImported = False
                        ElseIf Not ValidItem(Item, Item_ID) Then
                            Dim ErrorDr As DataRow
                            ErrorDr = ErrorTbl.NewRow
                            ErrorDr(0) = dr(0)
                            ErrorDr(1) = dr(1)
                            ErrorDr(2) = dr(2)
                            ErrorDr(3) = dr(3)
                            ErrorDr(4) = dr(4)
                            ErrorDr(5) = dr(5)
                            ErrorDr(6) = "Item does not exist"
                            ErrorDr(7) = Rowno
                            ErrorTbl.Rows.Add(ErrorDr)
                            bImported = False
                        ElseIf UOM.Trim = "" Then
                            Dim ErrorDr As DataRow
                            ErrorDr = ErrorTbl.NewRow
                            ErrorDr(0) = dr(0)
                            ErrorDr(1) = dr(1)
                            ErrorDr(2) = dr(2)
                            ErrorDr(3) = dr(3)
                            ErrorDr(4) = dr(4)
                            ErrorDr(5) = dr(5)
                            ErrorDr(6) = "Empty UOM"
                            ErrorDr(7) = Rowno
                            ErrorTbl.Rows.Add(ErrorDr)
                            bImported = False
                        ElseIf Not ValidUOM(Item, UOM) Then
                            Dim ErrorDr As DataRow
                            ErrorDr = ErrorTbl.NewRow
                            ErrorDr(0) = dr(0)
                            ErrorDr(1) = dr(1)
                            ErrorDr(2) = dr(2)
                            ErrorDr(3) = dr(3)
                            ErrorDr(4) = dr(4)
                            ErrorDr(5) = dr(5)
                            ErrorDr(6) = "Invalid UOM"
                            ErrorDr(7) = Rowno
                            ErrorTbl.Rows.Add(ErrorDr)
                            bImported = False
                        ElseIf Not ValidateQty(Val(qty).ToString(), Item, ddl_Organization.SelectedItem.Value, UOM, Conversion) Then  ' ElseIf Not ValidQty((Val(qty) * Val(Conversion)).ToString) Then
                            Dim ErrorDr As DataRow
                            ErrorDr = ErrorTbl.NewRow
                            ErrorDr(0) = dr(0)
                            ErrorDr(1) = dr(1)
                            ErrorDr(2) = dr(2)
                            ErrorDr(3) = dr(3)
                            ErrorDr(4) = dr(4)
                            'ErrorDr(4) = "Qty (in BASE UOM) is not valid:(" & (Val(dr(2).ToString()) * Val(Conversion)).ToString & ")"
                            ErrorDr(5) = dr(5)
                            ErrorDr(6) = "Qty is not valid:(" & (Val(qty) * Val(Conversion)).ToString & ")"
                            ErrorDr(7) = Rowno
                            ErrorTbl.Rows.Add(ErrorDr)
                            bImported = False
                        Else
                            Dim finalDr As DataRow
                            finalDr = FinalTbl.NewRow
                            finalDr(0) = dr(0).ToString
                            finalDr(1) = dr(1).ToString
                            finalDr(2) = dr(2).ToString

                            If HasLots(Item) = False Then
                                finalDr(2) = "SWX_LOT_NUM_NA"
                                finalDr(3) = System.DBNull.Value
                            End If

                            If dr(3).ToString().Trim() = "" Then
                                finalDr(3) = System.DBNull.Value
                            Else
                                finalDr(3) = dr(3).ToString
                            End If

                            finalDr(4) = Val(dr(4).ToString()) 'Item_ID
                            finalDr(5) = SalesRep_Number
                            finalDr(6) = Item_ID
                            finalDr(7) = Val(dr(4).ToString()) * Val(Conversion)
                            finalDr(8) = UOM
                            FinalTbl.Rows.Add(finalDr)
                        End If
                        Rowno = Rowno + 1
                    Next
                    If FinalTbl.Rows.Count > 0 Then
                        Dim dtNotImported As New DataTable
                        dtNotImported.Columns.Add("VanCode")

                        Dim ds_rslt As DataSet
                        ds_rslt = StockTran(ddl_Organization.SelectedItem.Value, FinalTbl)

                        Dim xml_s As String
                        xml_s = ds_rslt.GetXml()

                        xml_s = Regex.Replace(xml_s, "<Expiry_Date>(?<year>\d{4})-(?<month>\d{2})-(?<date>\d{2}).*?</Expiry_Date>", "<Expiry_Date>${date}/${month}/${year}</Expiry_Date>", RegexOptions.CultureInvariant Or RegexOptions.IgnoreCase)

                        log.Error(xml_s)

                        Dim ds_st_unconfirm As DataSet
                        ds_st_unconfirm = ObjStock.Get_ImportStockUnConfirm_Updated(Err_No, Err_Desc, xml_s, ddl_Organization.SelectedItem.Value)


                        Session.Remove("Stock_Unconfirm")
                        Session("Stock_Unconfirm") = ds_st_unconfirm

                        If ErrorTbl.Rows.Count > 0 Then
                            RetMessage = "Import completed with some errors. Please see the error logs for more information."
                        Else
                            RetMessage = "Successfully imported. Please review and confirm the van stock."
                        End If

                        Dim buploaded As Boolean
                    Else
                        RetMessage = "No Valid Rows in the file to import."
                        bImported = False
                    End If
                    BTn_StockConfirm.Visible = True
                    btnFilter.Visible = True
                    Session("Errordt") = ErrorTbl.Copy
                Else
                    RetMessage = "No Rows in the file to import."
                    bImported = False
                End If
            Else
                RetMessage = "Invalid File format."
                bImported = False
            End If
            ErrorTbl = Nothing
            TempTbl = Nothing
            log.Info("Van stock Import " & RetMessage)
            log.Info("Van stock Import finished for " & File)
        Catch ex As Exception
            RetMessage = "Could not import the file. Please contact the administrator"
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Function


    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try


            If ddl_Organization.SelectedItem.Value = "0" Then
                lblmsgPopUp.Text = "Please Select the Organization."
                MPEDivConfig.Show()
                ClassUpdatePnl.Update()
                Return
            End If

            If Not file_import.HasFile Then
                lblmsgPopUp.Text = "Please Select a File to upload."
                MPEDivConfig.Show()
                ClassUpdatePnl.Update()
                Return
            End If
            Dim spath As String = ""
            spath = ConfigurationManager.AppSettings("ExcelPath")
            If Directory.Exists(spath) = False Then
                Directory.CreateDirectory(spath)
            End If
            spath = spath & "\VanStockupload\" & Now.ToString("ddMMyyyyhhssmm") & file_import.FileName
            If (file_import.HasFile) Then
                If file_import.PostedFile.ContentLength > 10485760 Then
                    lblmsgPopUp.Text = "File Size should be less than 10 MB, Please Split files and upload."
                    MPEDivConfig.Show()
                Else
                    If System.IO.File.Exists(spath) Then
                        System.IO.File.Delete(spath)
                    End If
                    file_import.SaveAs(spath)
                    file_import.FileContent.Close()
                    file_import.FileContent.Dispose()
                    Dim success As Boolean
                    Dim RetMessage As String = ""
                    success = ImportFile(spath, RetMessage)

                    lblmsgPopUp.Text = RetMessage
                    BindGridError()
                    ddl_org.ClearSelection()
                    If Not ddl_org.FindItemByValue(ddl_Organization.SelectedItem.Value) Is Nothing Then
                        ddl_org.FindItemByValue(ddl_Organization.SelectedItem.Value).Selected = True
                        Dim objUserAccess As UserAccess
                        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
                        Dim objCommon As New Common
                        ddlVan.DataSource = objCommon.GetVanByOrg(Err_No, Err_Desc, ddl_Organization.SelectedItem.Value, objUserAccess.UserID)
                        ddlVan.DataValueField = "SalesRep_ID"
                        ddlVan.DataTextField = "SalesRep_Name"
                        ddlVan.DataBind()
                        ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))
                        objCommon = Nothing

                    End If

                    BindGrid("")
                    UpdatePanel2.Update()
                    MPEDivConfig.Show()
                    ClassUpdatePnl.Update()
                End If
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Protected Sub btnView_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btnConform As LinkButton = TryCast(sender, LinkButton)
        Dim row As GridViewRow = DirectCast(btnConform.NamingContainer, GridViewRow)
        Me.MPStockDetails.Hide()

        Dim success As Boolean = False
        Try
            Dim HRowID As String
            HRowID = CType(row.FindControl("HRowID"), HiddenField).Value
            RowID.Value = HRowID

            Dim HRow_vancode As String
            HRow_vancode = CType(row.FindControl("HRow_vancode"), HiddenField).Value

            lbl_Selvan.Text = row.Cells(2).Text

            Dim ds_view As New DataSet
            ds_view = Session("Stock_Unconfirm")


            Hvcode.Value = HRow_vancode
            Dim dt_ As New DataTable
            dt_ = ds_view.Tables(4).Select("Van_Code= '" + HRow_vancode + "'").CopyToDataTable()

            Dim view As DataView = New DataView(dt_)
            Dim distinctValues As DataTable = view.ToTable(True, "item_code", "Lot_Number", "ExpiryDate", "Item_UOM")


            Dim dt_v As DataTable
            dt_v = New DataTable("ViewDt")

            Dim column1 As DataColumn = New DataColumn("Van_Code")
            column1.DataType = System.Type.GetType("System.String")

            Dim column2 As DataColumn = New DataColumn("Item_Code")
            column2.DataType = System.Type.GetType("System.String")
            Dim column3 As DataColumn = New DataColumn("Lot_Number")
            column3.DataType = System.Type.GetType("System.String")

            Dim column4 As DataColumn = New DataColumn("ExpiryDate")
            column4.DataType = System.Type.GetType("System.DateTime")
            Dim column5 As DataColumn = New DataColumn("Lot_QtyS")
            column5.DataType = System.Type.GetType("System.Decimal")
            Dim column6 As DataColumn = New DataColumn("UOM")
            column6.DataType = System.Type.GetType("System.String")

            dt_v.Columns.Add(column1)
            dt_v.Columns.Add(column2)
            dt_v.Columns.Add(column3)
            dt_v.Columns.Add(column4)
            dt_v.Columns.Add(column5)
            dt_v.Columns.Add(column6)


            For Each r As DataRow In distinctValues.Rows

                If HasLots(r(0).ToString()) Then

                    Try

                        Dim sum As Object
                        sum = dt_.Compute("Sum(Lot_QtyS)", "item_code = '" & r(0).ToString() & "' AND Item_UOM='" & r(3).ToString() & "' AND Lot_Number='" & r(1).ToString() & "' AND ExpiryDate='" & r(2).ToString() & "' ")

                        Dim dth As New DataTable
                        dth = dt_.Select("item_code= '" & r(0).ToString() & "' AND Item_UOM='" & r(3).ToString() & "' AND Lot_Number='" & r(1).ToString() & "' AND ExpiryDate='" & r(2).ToString() & "'").CopyToDataTable()

                        Dim Rw_itm As DataRow
                        Rw_itm = dt_v.NewRow()
                        Rw_itm.Item(0) = HRow_vancode
                        Rw_itm.Item(1) = r(0).ToString()

                        If dth.Rows.Count > 0 Then
                            Rw_itm.Item(2) = dth.Rows(0)("Lot_Number").ToString()
                        End If

                        Rw_itm.Item(3) = r(2).ToString()

                        Rw_itm.Item(4) = sum
                        Rw_itm.Item(5) = r(3).ToString()
                        dt_v.Rows.Add(Rw_itm)


                        'For Each total_1 In totals1
                        '    If total_1.item_code = r(0).ToString() Then
                        '        Dim dt_h As New DataTable
                        '        dt_h = dt_.Select("item_code= '" + r(0).ToString() + "'").CopyToDataTable()
                        '        log.Error("10.0")
                        '        Dim Row_itm As DataRow
                        '        Row_itm = dt_v.NewRow()
                        '        log.Error("10.1")
                        '        Row_itm.Item(0) = HRow_vancode
                        '        Row_itm.Item(1) = r(0).ToString()
                        '        log.Error("10.2")
                        '        If dt_h.Rows.Count > 0 Then
                        '            log.Error("10.3")
                        '            Row_itm.Item(2) = dt_h.Rows(0)("Lot_Number").ToString()
                        '        End If
                        '        log.Error("10.4")
                        '        Row_itm.Item(3) = total_1.Expiry_Date
                        '        Row_itm.Item(4) = total_1.Total
                        '        dt_v.Rows.Add(Row_itm)


                        '    End If


                        'Next

                    Catch ex As Exception
                        log.Error(r(0).ToString() & ex.Message.ToString())
                    End Try







                Else


                    Try


                        Dim sumObject As Object

                        Try
                            Dim drow As DataRow = dt_v.Select("item_code='" & r(0).ToString() & "' AND Item_UOM='" & r(3).ToString() & "'")(0)
                            'Dim tempIndex As Integer = dt_v.Rows.IndexOf(drow)
                            dt_v.Rows.Remove(drow)
                        Catch ex As Exception

                        End Try


                        sumObject = dt_.Compute("Sum(Lot_QtyS)", "item_code = '" & r(0).ToString() & "' AND Item_UOM='" & r(3).ToString() & "'")
                        Dim dt_1 As New DataTable
                        dt_1 = dt_.Select("item_code= '" & r(0).ToString() & "' AND Item_UOM='" & r(3).ToString() & "'").CopyToDataTable()
                        Dim Row_itm As DataRow
                        Row_itm = dt_v.NewRow()
                        Row_itm.Item(0) = HRow_vancode
                        Row_itm.Item(1) = r(0).ToString()
                        If dt_1.Rows.Count > 0 Then
                            Row_itm.Item(2) = dt_1.Rows(0)("Lot_Number").ToString()
                            '   Row_itm.Item(3) =  dt_1.Rows(0)("ExpiryDate").ToString()
                        End If
                        Row_itm.Item(4) = sumObject
                        Row_itm.Item(5) = r(3).ToString()
                        dt_v.Rows.Add(Row_itm)
                    Catch ex As Exception
                        log.Error(r(0).ToString() & ex.Message.ToString())
                    End Try
                End If



            Next

            log.Error("12")


            GVItems.DataSource = dt_v
            GVItems.DataBind()
            '  ClassUpdatePnl.Update()
            Me.MPStockDetails.Show()

        Catch ex As Exception
            log.Error(ex.Message.ToString())
            Err_No = "74210"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub

    Protected Sub btnDownloadSt_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btnConform As LinkButton = TryCast(sender, LinkButton)
        Dim row As GridViewRow = DirectCast(btnConform.NamingContainer, GridViewRow)


        Dim success As Boolean = False
        Try
            Dim HRowID As String
            HRowID = CType(row.FindControl("HRowID"), HiddenField).Value
            RowID.Value = HRowID
            lbl_Selvan.Text = row.Cells(2).Text
            Dim stdt As New DataTable
            stdt = ObjStock.StockRequisitionItemsforExport(Err_No, Err_Desc, HRowID)
            ExportToExcel("VanLoadTemplate.xls", stdt)
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            Err_No = "74210"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub

    Protected Sub BTn_StockConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTn_StockConfirm.Click

        Dim ds_stconfirm As New DataSet
        ds_stconfirm = Session("Stock_Unconfirm")

        Try
            Dim chk_flg As Boolean = False
            Dim row As GridViewRow
            Dim Success As Boolean = False
            Dim chk_error As Integer = 0
            Dim cnt_chk_flg As Integer = 0
            Dim idCollection As String = ""
            For Each row In GvStockRequ.Rows
                Dim RowCheckBox As System.Web.UI.WebControls.CheckBox = row.FindControl("chkDelete")

                If RowCheckBox.Checked = True Then
                    chk_flg = True
                    cnt_chk_flg = cnt_chk_flg + 1
                    Dim HRowID As String
                    HRowID = CType(row.FindControl("HRowID"), HiddenField).Value
                    Dim HRow_vancode As String
                    HRow_vancode = CType(row.FindControl("HRow_vancode"), HiddenField).Value




                    Dim dt_stconfirm_H As New DataTable
                    Dim dt_stconfirm_I As New DataTable
                    Dim dt_stconfirm_L As New DataTable

                    dt_stconfirm_H = ds_stconfirm.Tables(1).Select("StockTransfer_ID='" + HRowID + "'").CopyToDataTable()
                    dt_stconfirm_I = ds_stconfirm.Tables(2).Select("StockTransfer_ID='" + HRowID + "'").CopyToDataTable()
                    dt_stconfirm_L = ds_stconfirm.Tables(3).Clone()


                    For Each r In dt_stconfirm_I.Rows
                        Dim dt_temp As New DataTable
                        dt_temp = ds_stconfirm.Tables(3).Select("StockTransfer_Item_ID='" + r("StockTransfer_Item_ID").ToString() + "'").CopyToDataTable()
                        dt_stconfirm_L.Merge(dt_temp.Copy())
                    Next


                    Dim ds_final As New DataSet
                    ds_final.Tables.Add(dt_stconfirm_H)
                    ds_final.Tables.Add(dt_stconfirm_I)
                    ds_final.Tables.Add(dt_stconfirm_L)

                    Dim x As String
                    x = ds_final.GetXml()

                    x = Regex.Replace(x, "<Expiry_Dt>(?<year>\d{4})-(?<month>\d{2})-(?<date>\d{2}).*?</Expiry_Dt>", "<Expiry_Dt>${month}/${date}/${year}</Expiry_Dt>", RegexOptions.CultureInvariant Or RegexOptions.IgnoreCase)

                    If ObjStock.Get_ImportStockConfirm(Err_No, Err_Desc, x, "", CType(Session("User_Access"), UserAccess).UserID) = True Then
                        Success = True
                        log.Info(HRowID & " Successfully Confirmed.")


                        For Each dr_d In ds_stconfirm.Tables(0).Rows

                            If dr_d("Dest_Org_ID") = HRow_vancode Then
                                dr_d("Status") = "1"


                            End If
                        Next
                    Else
                        chk_error = chk_error + 1

                    End If

                    If Success = True Then
                        Session.Remove("Stock_Unconfirm")
                        Session("Stock_Unconfirm") = ds_stconfirm

                        ' MessageBoxValidation("Successfully Confirmed.", "Information")

                    Else
                        '   MessageBoxValidation("Error occured while confirming.", "Information")
                        log.Error(Err_Desc)

                    End If




                End If
            Next

            If chk_error = 0 And chk_flg = True Then
                MessageBoxValidation("Successfully Confirmed.", "Information")
            ElseIf chk_error > 0 And chk_flg = True Then
                If cnt_chk_flg = chk_error Then
                    MessageBoxValidation("Error occured while confirming stocks.", "Information")
                Else
                    MessageBoxValidation("Error occured while confirming some stocks.", "Information")
                End If

                log.Error(Err_Desc)
            End If


            If chk_flg = False Then
                MessageBoxValidation("No rows were selected. Please select the rows and try again ", "Information")
            End If

            BindGrid("")
            ClassUpdatePnl.Update()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            Err_No = "74211"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFilter.Click
        Try


            If ddl_org.SelectedItem.Value = "0" Then
                MessageBoxValidation("Please select the Organization", "Validation")
                Exit Sub
            End If
            BindGrid(ddlVan.SelectedItem.Value)
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Protected Sub btnConform_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btnConform As ImageButton = TryCast(sender, ImageButton)
        Dim row As GridViewRow = DirectCast(btnConform.NamingContainer, GridViewRow)


        Dim success As Boolean = False
        Try
            Dim HRowID As String
            HRowID = CType(row.FindControl("HRowID"), HiddenField).Value

            Dim HRow_vancode As String
            HRow_vancode = CType(row.FindControl("HRow_vancode"), HiddenField).Value


            Dim ds_stconfirm As New DataSet
            ds_stconfirm = Session("Stock_Unconfirm")

            Dim dt_stconfirm_H As New DataTable
            Dim dt_stconfirm_I As New DataTable
            Dim dt_stconfirm_L As New DataTable

            dt_stconfirm_H = ds_stconfirm.Tables(1).Select("StockTransfer_ID='" + HRowID + "'").CopyToDataTable()
            dt_stconfirm_I = ds_stconfirm.Tables(2).Select("StockTransfer_ID='" + HRowID + "'").CopyToDataTable()
            dt_stconfirm_L = ds_stconfirm.Tables(3).Clone()
            For Each r In dt_stconfirm_I.Rows
                Dim dt_temp As New DataTable
                dt_temp = ds_stconfirm.Tables(3).Select("StockTransfer_Item_ID='" + r("StockTransfer_Item_ID").ToString() + "'").CopyToDataTable()
                dt_stconfirm_L.Merge(dt_temp.Copy())
            Next


            Dim ds_final As New DataSet
            ds_final.Tables.Add(dt_stconfirm_H)
            ds_final.Tables.Add(dt_stconfirm_I)
            ds_final.Tables.Add(dt_stconfirm_L)

            Dim x As String
            x = ds_final.GetXml()

            If ObjStock.Get_ImportStockConfirm(Err_No, Err_Desc, x, "", CType(Session("User_Access"), UserAccess).UserID) = True Then
                success = True
                log.Info(HRowID & " Successfully Confirmed.")
            End If

            If success = True Then
                MessageBoxValidation("Successfully Confirmed.", "Information")
                BindGrid("")
            Else
                MessageBoxValidation("Error occured while confirming.", "Information")
                log.Error(Err_Desc)

            End If

        Catch ex As Exception
            log.Error(ex.Message.ToString())
            Err_No = "74210"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub

    Protected Sub btnConfirmAll_Click()
        Try
            Dim row As GridViewRow
            Dim Success As Boolean = False
            Dim idCollection As String = ""
            For Each row In GvStockRequ.Rows
                Dim RowCheckBox As System.Web.UI.WebControls.CheckBox = row.FindControl("chkDelete")

                If RowCheckBox.Checked = True Then
                    Dim HRowID As String
                    HRowID = CType(row.FindControl("HRowID"), HiddenField).Value
                    Dim PoRefno As String
                    PoRefno = "PO-" & Now.ToString("ddMMyyyyhhmmss")
                    If ObjStock.ConfirmStockRequisitionbyID(Err_No, Err_Desc, HRowID, CType(Session("User_Access"), UserAccess).UserID, PoRefno) = True Then
                        Success = True
                        log.Info(HRowID & " Successfully Confirmed.")
                    End If
                End If
            Next

            BindGrid("")
        Catch ex As Exception
            log.Error(ex.Message.ToString())
            Err_No = "74211"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub

    Private Sub BtnDownLoad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnDownLoad.Click
        CsvExport()
    End Sub

    Private Sub btn_close_Click(sender As Object, e As ImageClickEventArgs) Handles btn_close.Click
        MPEDivConfig.Hide()
        ClassUpdatePnl.Update()
    End Sub

    Private Sub btn_Confirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Confirm.Click



        Dim success As Boolean = False
        Try
            Dim HRowID As String
            HRowID = RowID.Value

            Dim PoRefno As String
            PoRefno = "PO-" & Now.ToString("ddMMyyyyhhmmss")

            If ObjStock.ConfirmStockRequisitionbyID(Err_No, Err_Desc, HRowID, CType(Session("User_Access"), UserAccess).UserID, PoRefno) = True Then
                success = True
                log.Info(HRowID & " Successfully Confirmed.")
            End If

            If success = True Then
                MessageBoxValidation("Successfully Confirmed.", "Information")
                MPStockDetails.Hide()
                BindGrid("")
                log.Info(HRowID & " Successfully Confirmed.")
            Else
                MessageBoxValidation("Error occured while confirming.", "Information")
                MPStockDetails.Hide()
                log.Error(Err_Desc)

            End If

        Catch ex As Exception
            log.Error(ex.Message.ToString())
            Err_No = "74210"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub


    Private Property SortField() As String
        Get
            If ViewState("SortColumn") Is Nothing Then
                ViewState("SortColumn") = ""
            End If
            Return ViewState("SortColumn").ToString()
        End Get
        Set(ByVal value As String)
            ViewState("SortColumn") = value
        End Set
    End Property
    Private Function HasAuthentication() As Boolean
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        If objUserAccess.PageID.Contains(PageID) Then Return True Else Return False
    End Function
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Sub Filldropdowns()
        Try


            Dim objCommon As New Common
            Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
            ddl_org.DataSource = (New SalesWorx.BO.Common.Product).GetOrganisation(Err_No, Err_Desc, SubQry)
            ddl_org.Items.Clear()
            ddl_org.Items.Insert(0, New RadComboBoxItem("--Select Organisation--"))
            ddl_org.AppendDataBoundItems = True
            ddl_org.DataValueField = "MAS_Org_ID"
            ddl_org.DataTextField = "Description"
            ddl_org.DataBind()
            ddl_org.Items(0).Value = 0

            ddl_Organization.DataSource = (New SalesWorx.BO.Common.Product).GetOrganisation(Err_No, Err_Desc, SubQry)
            ddl_Organization.Items.Clear()
            ddl_Organization.Items.Insert(0, New RadComboBoxItem("--Select Organisation--"))
            ddl_Organization.AppendDataBoundItems = True
            ddl_Organization.DataValueField = "MAS_Org_ID"
            ddl_Organization.DataTextField = "Description"
            ddl_Organization.DataBind()
            ddl_Organization.Items(0).Value = 0

            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)

            ddlVan.DataSource = objCommon.GetVanByOrg(Err_No, Err_Desc, ddl_org.SelectedValue, objUserAccess.UserID)
            ddlVan.DataValueField = "SalesRep_ID"
            ddlVan.DataTextField = "SalesRep_Name"
            ddlVan.DataBind()
            ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))



            Try
                If ddl_org.Items.Count = 2 Then
                    ddl_org.SelectedIndex = 1

                    ddlVan.DataSource = objCommon.GetVanByOrg(Err_No, Err_Desc, ddl_org.SelectedValue, objUserAccess.UserID)
                    ddlVan.DataValueField = "SalesRep_ID"
                    ddlVan.DataTextField = "SalesRep_Name"
                    ddlVan.DataBind()
                    ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))

                    objCommon = Nothing

                End If

                If ddl_Organization.Items.Count = 2 Then
                    ddl_Organization.SelectedIndex = 1
                End If


            Catch ex As Exception
                log.Error(ex.Message.ToString())
            End Try

            objCommon = Nothing

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub
    Sub BindGrid(ByVal serch_vancode As String)
        Try
            Dim dt As New DataTable


            Dim ds_St_Un As New DataSet
            Dim dr() As DataRow
            ds_St_Un = Session("Stock_Unconfirm")


            If serch_vancode <> "" And serch_vancode <> "0" Then
                dr = ds_St_Un.Tables(0).Select("Status='0' And SalesRep_ID='" + serch_vancode + "'")
                If dr.Count = 0 Then
                    GvStockRequ.DataSource = Nothing
                    GvStockRequ.DataBind()
                    Exit Sub
                End If

                dt = ds_St_Un.Tables(0).Select("Status='0' And SalesRep_ID='" + serch_vancode + "'").CopyToDataTable()

            Else
                dr = ds_St_Un.Tables(0).Select("Status='0'")
                If dr.Count = 0 Then
                    GvStockRequ.DataSource = Nothing
                    GvStockRequ.DataBind()

                    Exit Sub
                End If
                dt = ds_St_Un.Tables(0).Select("Status='0'").CopyToDataTable()
            End If

            Dim dv As New DataView(dt)
            If SortField <> "" Then
                dv.Sort = (SortField & " ") + SortDirection
            End If

            If ViewState("SortField") <> "" Then
                dv.Sort = (ViewState("SortField") & " ") + SortDirection
            End If
            GvStockRequ.DataSource = dv
            GvStockRequ.DataBind()
            RegisterPostBackControl()
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Sub BindGridError()
        Try
            log.Error("errorgrid")
            Dim dt As New DataTable
            dt = CType(Session("Errordt"), DataTable)
            Dim dv As New DataView(dt)
            If SortField <> "" Then
                dv.Sort = (SortField & " ") + SortDirection
            End If
            dgvErros.DataSource = dv
            dgvErros.DataBind()
            If dt.Rows.Count > 0 Then
                BtnDownLoad.Visible = True
            End If


        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Function IsValidItem(ByVal OrgID As String, ByRef Item_Code As String) As Boolean
        Return ObjStock.IsValidItem(Err_No, Err_Desc, ddl_Organization.SelectedItem.Value, Item_Code)
    End Function
    Function ValidQty(ByVal Qty As String)



        Dim bRetVal As Boolean
        Try
            Dim num As Integer
            If Integer.TryParse(Qty, num) Then
                bRetVal = True
            Else
                bRetVal = False
            End If
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
        Return bRetVal
    End Function
    Function ValidVan(ByVal van As String, ByRef Salesrep_ID As String) As Boolean
        Return ObjStock.IsValidVan(Err_No, Err_Desc, ddl_Organization.SelectedItem.Value, van, Salesrep_ID)
    End Function
    Function ValidItem(ByVal Item As String, ByRef Item_ID As String) As Boolean
        Return ObjStock.ValidItem(Err_No, Err_Desc, ddl_Organization.SelectedItem.Value, Item, Item_ID)
    End Function
    Function ValidItemUOM(ByVal Item As String, ByVal UOM As String, ByRef Conversion As String) As Boolean
        Return ObjStock.ValidItemUOM(Err_No, Err_Desc, ddl_Organization.SelectedItem.Value, Item, UOM, Conversion)
    End Function
    Function AlreadyConfirmed(ByVal Salesrep_ID As String) As Boolean
        Return ObjStock.AlreadyConfirmed(Err_No, Err_Desc, ddl_Organization.SelectedItem.Value, Salesrep_ID)
    End Function
    Function HasLots(ByVal Item As String) As Boolean
        Return ObjStock.HasLots(Err_No, Err_Desc, ddl_Organization.SelectedItem.Value, Item)
    End Function
    Private Function DoXLSUpload(ByVal filename As String, ByRef bfileformat As Boolean) As DataTable
        Dim dtImport As New DataTable
        Try
            'Dim connString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & filename & " ;Extended Properties=""Excel 8.0;HDR=Yes;IMEX=1"""
            Dim connString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & filename & " ;Extended Properties=""Excel 8.0;"""
            Dim oledbConn As New OleDbConnection(connString)
            Try
                oledbConn.Open()
                Dim cmd As New OleDbCommand("SELECT * FROM [Stock$]", oledbConn)
                Dim oleda As New OleDbDataAdapter()
                oleda.SelectCommand = cmd
                oleda.Fill(dtImport)
                If dtImport.Columns.Count = 6 Then
                    If dtImport.Columns(0).ColumnName.ToUpper = "VAN_CODE" And dtImport.Columns(1).ColumnName.ToUpper = "ITEM_CODE" And dtImport.Columns(2).ColumnName.ToUpper = "LOT_NUMBER" And dtImport.Columns(3).ColumnName.ToUpper = "EXPIRY_DATE" And dtImport.Columns(4).ColumnName.ToUpper = "LOT_QTY" And dtImport.Columns(5).ColumnName.ToUpper = "UOM" Then
                        bfileformat = True
                    End If
                End If
            Catch ex As Exception
                log.Error(GetExceptionInfo(ex))
                log.Error(ex.Message.ToString())
            Finally
                ' Close connection
                oledbConn.Close()
            End Try
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
            Throw ex
        End Try
        Return dtImport
    End Function
    Private Sub DeleteExcel(ByVal filename As String)
        Try
            Dim TheFile As FileInfo = New FileInfo(ViewState("FileName"))
            If TheFile.Exists Then
                File.Delete(filename)
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Private Function DoXLSXUpload(ByVal filename As String, ByRef bfileformat As Boolean) As DataTable
        Dim dtImport As New DataTable
        Try
            Dim connString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & filename & " ;Extended Properties=""Excel 12.0;HDR=Yes;IMEX=1"""
            Dim oledbConn As New OleDbConnection(connString)
            Try
                oledbConn.Open()

                Dim cmd As New OleDbCommand("SELECT * FROM [Stock$]", oledbConn)

                Dim oleda As New OleDbDataAdapter()

                oleda.SelectCommand = cmd



                oleda.Fill(dtImport)


                If dtImport.Columns.Count = 6 Then
                    If dtImport.Columns(0).ColumnName.ToUpper = "VAN_CODE" And dtImport.Columns(1).ColumnName.ToUpper = "ITEM_CODE" And dtImport.Columns(2).ColumnName.ToUpper = "LOT_NUMBER" And dtImport.Columns(3).ColumnName.ToUpper = "EXPIRY_DATE" And dtImport.Columns(4).ColumnName.ToUpper = "LOT_QTY" And dtImport.Columns(5).ColumnName.ToUpper = "UOM" Then
                        bfileformat = True
                    End If
                End If
            Catch ex As Exception
                log.Error(GetExceptionInfo(ex))
                log.Error(ex.Message.ToString())
            Finally
                ' Close connection
                oledbConn.Close()
            End Try
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
            Throw ex
        End Try
        Return dtImport
    End Function
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        MPEDivConfig.Hide()

    End Sub
    Sub CsvExport()
        Dim dt As New DataTable
        dt = CType(Session("Errordt"), DataTable)
        'Build the CSV file data as a Comma separated string.
        Dim csv As String = String.Empty

        For Each column As DataColumn In dt.Columns
            'Add the Header row for CSV file.
            csv += column.ColumnName + ","c
        Next

        'Add new line.
        csv += vbCr & vbLf

        For Each row As DataRow In dt.Rows
            For Each column As DataColumn In dt.Columns
                'Add the Data rows.
                csv += row(column.ColumnName).ToString().Replace(",", ";") + ","c
            Next

            'Add new line.
            csv += vbCr & vbLf
        Next

        'Download the CSV file.
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=ErrorExport.csv")
        Response.Charset = ""
        Response.ContentType = "application/text"
        Response.Output.Write(csv)
        Response.Flush()
        Response.End()
    End Sub
    Private Sub RegisterPostBackControl()
        'For Each row As GridViewRow In GvStockRequ.Rows
        '    Dim lnkFull As LinkButton = TryCast(row.FindControl("btnDownloadSt"), LinkButton)
        '    ScriptManager.GetCurrent(Me).RegisterPostBackControl(lnkFull)
        'Next
    End Sub
    Private Sub ExportToExcel(ByVal strFileName As String, ByVal ds As DataTable)

        Try
            Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & strFileName
            WriteXLSFile(fn, ds)
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
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
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
            log.Error(ex.Message.ToString())
            Return False
        End Try
    End Function
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
    Private Sub ddl_org_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_org.SelectedIndexChanged

        Try


            Dim objUserAccess As UserAccess
            objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
            Dim objCommon As New Common
            ddlVan.DataSource = objCommon.GetVanByOrg(Err_No, Err_Desc, ddl_org.SelectedValue, objUserAccess.UserID)
            ddlVan.DataValueField = "SalesRep_ID"
            ddlVan.DataTextField = "SalesRep_Name"
            ddlVan.DataBind()
            ddlVan.Items.Insert(0, New RadComboBoxItem("-- Select a value --", "0"))
            objCommon = Nothing
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub


    Private Function StockTran(ByVal Org_ID As String, ByVal dt_s As DataTable) As DataSet
        Dim Ds_stock As DataSet
        Dim test_s As String
        Try
            Ds_stock = ObjStock.GetImportStockTbls(Err_No, Err_Desc)

            If Ds_stock.Tables.Count = 3 Then
                Ds_stock.Tables(0).TableName = "TBL_Stock_Transfer"
                Ds_stock.Tables(1).TableName = "TBL_Stock_Transfer_Items"
                Ds_stock.Tables(2).TableName = "TBL_Stock_Transfer_Lots"


                Dim rslt_vcode = (From row In dt_s.AsEnumerable() Select Van_code = row.Field(Of String)("Van_code").ToUpper).Distinct().ToList()




                For Each dr_vcode In rslt_vcode

                    Dim dr_H As DataRow
                    dr_H = Ds_stock.Tables("TBL_Stock_Transfer").NewRow()
                    dr_H("Transfer_Description") = "Van Load"
                    dr_H("Transfer_Ref_No") = dr_vcode + " - L - "
                    dr_H("Transfer_Type") = "L"
                    Dim result() As DataRow = dt_s.Select("Van_Code='" & dr_vcode & "'")
                    dr_H("SalesRep_ID") = result(0)(5).ToString()
                    dr_H("Src_Org_ID") = "Org_ID"
                    dr_H("Dest_Org_ID") = dr_vcode
                    dr_H("Status") = "N"
                    Ds_stock.Tables("TBL_Stock_Transfer").Rows.Add(dr_H)


                Next

                For Each dr As DataRow In dt_s.Rows

                    Dim dr_I As DataRow
                    dr_I = Ds_stock.Tables("TBL_Stock_Transfer_Items").NewRow()

                    Dim dt_pr As DataTable
                    dt_pr = ObjStock.GetItemDetails(Err_No, Err_Desc, Org_ID, dr("Item_Code").ToString())

                    If dt_pr.Rows.Count > 0 Then
                        dr_I("Inventory_Item_ID") = dt_pr.Rows(0)("Inventory_Item_ID").ToString()
                        dr_I("Qty") = dr("UOMQty")
                        Ds_stock.Tables("TBL_Stock_Transfer_Items").Rows.Add(dr_I)

                    End If

                Next

                For Each dr As DataRow In dt_s.Rows
                    Dim dr_L As DataRow
                    dr_L = Ds_stock.Tables("TBL_Stock_Transfer_Lots").NewRow()
                    dr_L("Lot_Qty") = dr("UOMQty")
                    dr_L("Lot_Type") = "U"
                    Ds_stock.Tables("TBL_Stock_Transfer_Lots").Rows.Add(dr_L)

                Next


                dt_s.TableName = "ImportVan"
                Ds_stock.Tables.Add(dt_s)

            End If



        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
        Return Ds_stock
    End Function

    Private Sub GVItems_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GVItems.PageIndexChanging
        Try


            GVItems.PageIndex = e.NewPageIndex


            Dim ds_view As New DataSet
            ds_view = Session("Stock_Unconfirm")
            Dim dt_ As New DataTable
            dt_ = ds_view.Tables(4).Select("Van_Code= '" + Hvcode.Value + "'").CopyToDataTable()


            GVItems.DataSource = dt_
            GVItems.DataBind()
            ClassUpdatePnl.Update()
            Me.MPStockDetails.Show()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Private Sub dgvErros_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgvErros.PageIndexChanging
        Try
            dgvErros.PageIndex = e.NewPageIndex
            BindGridError()
            MPEDivConfig.Show()
            ClassUpdatePnl.Update()
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub


    Private Sub GvStockRequ_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GvStockRequ.PageIndexChanging
        GvStockRequ.PageIndex = e.NewPageIndex
        BindGrid("")
    End Sub

    Private Sub GvStockRequ_Sorting(sender As Object, e As GridViewSortEventArgs) Handles GvStockRequ.Sorting
        ViewState("SortField") = e.SortExpression
        SortDirection = "flip"
        BindGrid("")
    End Sub

    Protected Sub btndownloadTemplate_Click(sender As Object, e As EventArgs) Handles btndownloadTemplate.Click
        Try


            Dim Filename As String = System.Configuration.ConfigurationManager.AppSettings("ExcelTemplatePath") & "VanLoadTemplate.xls"
            Dim TheFile As FileInfo = New FileInfo(Filename)
            If TheFile.Exists Then
                Dim strFileName As String = "Template" + Now.ToString("ddMMMyyHHmmss") + ".xls"

                'Dim fn As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "Others\" & strFileName
                'WriteXLSFile(fn, dtRelation)

                'ViewState("AvailableBenefitParameter") = fn

                ViewState("SampleTemplate") = strFileName

                Dim sFileName As String = strFileName
                Dim sFullPath As String = Filename
                Dim fileBytes As Byte() = System.IO.File.ReadAllBytes(sFullPath)

                Dim context As System.Web.HttpContext = System.Web.HttpContext.Current
                context.Response.ContentType = "application/vnd.ms-excel"
                context.Response.AddHeader("Content-Disposition", "attachment;filename=" + sFileName)
                context.Response.Clear()
                context.Response.BinaryWrite(fileBytes)
                context.Response.Flush()
                context.ApplicationInstance.CompleteRequest()
                DeleteExcelTemplate()
            End If
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    Private Sub DeleteExcelTemplate()
        Try
            Dim Filename As String = ViewState("SampleTemplate")
            'Dim Foldername As String = System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & "UploadLog\" & Filename
            Dim TheFile As FileInfo = New FileInfo(Filename)
            If TheFile.Exists Then
                File.Delete(Filename)
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            log.Error(ex.Message.ToString())
        End Try
    End Sub

    Function ValidateQty(ByVal Qty As String, ByVal item_code As String, ByVal Org_ID As String, uom As String, ByRef Conversion_ As String)



        Dim bRetVal As Boolean
        Try

            ''    uom = ObjStock.GetUOMforConverion(Err_No, Err_Desc, Org_ID, item_code)

            Conversion_ = ObjStock.GetConversion(Err_No, Err_Desc, Org_ID, item_code, uom)

            Dim num As Integer
            If Integer.TryParse((Val(Qty) * Val(Conversion_)), num) Then
                bRetVal = True
            Else
                bRetVal = False
            End If
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
        Return bRetVal
    End Function
    Function ValidUOM(ByVal Item As String, ByVal UOM As String) As Boolean
        Return ObjStock.ValidUOM(Err_No, Err_Desc, ddl_Organization.SelectedItem.Value, Item, UOM)
    End Function
End Class
