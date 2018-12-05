Imports SalesWorx.BO.Common
Imports System.Data
Imports System.Web
Imports System.Resources
Imports System.Web.UI.WebControls
Imports log4net
Imports System.Text.RegularExpressions
Imports System.IO
Imports Telerik.Web.UI
Partial Public Class AdminPrintHeaders
    Inherits System.Web.UI.Page
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim objCommon As New SalesWorx.BO.Common.Common
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim Dt As New DataTable
    Private Const PageID As String = "P259"
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
            LoadTypeDropDown()
            HCommonHdrforVan.Value = objCommon.GetAppConfig(Err_No, Err_Desc, "COMMON_PRINT_HDR_FOR_VAN").ToUpper()
            Resetfields()
            Dim PrintType = objCommon.GetAppConfig(Err_No, Err_Desc, "PRINTER_TYPE").ToUpper()
            If PrintType = "A4" Then
                a4.Visible = True
                inc4.Visible = False
            Else
                a4.Visible = False
                inc4.Visible = True
            End If
        Else
            MPEDetails.VisibleOnPageLoad = False
        End If
    End Sub
Sub LoadTypeDropDown()
        ddl_Type.Items.Clear()
        Dim ControlVal = objCommon.GetAppConfig(Err_No, Err_Desc, "PRINT_HEADER_CTL").ToUpper()
        ddl_Type.Items.Add(New RadComboBoxItem("Org Logo", "ORG_LOGO"))
        btnAdd.Text = "Add Org Print Header"

        If ControlVal = "CLIENT" Then
            ddl_Type.Items.Add(New RadComboBoxItem("Client Logo", "CLIENT_LOGO"))
             btnAdd.Text = "Add Client Print Header"
        End If
        If ControlVal = "VAN" Then
            ddl_Type.Items.Add(New RadComboBoxItem("Van Logo", "VAN_LOGO"))
             btnAdd.Text = "Add Van Print Header"
        End If
        btnAdd.Visible = True
        BindData()
          ClassUpdatePnl.Update()
End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Try
            MPEDetails.VisibleOnPageLoad = False
            Resetfields()
        Catch

        End Try
    End Sub

    Public Sub Resetfields()

        Me.txt_Line1.Text = ""
        Me.txt_Line2.Text = ""
        Me.txt_Line3.Text = ""
        Me.txt_Line7.Text = ""
        Me.txt_Line8.Text = ""
        Me.txt_Line9.Text = ""

        ddl_Client.ClearSelection()
        Me.btnSave.Text = "Save"
        'Me.btnAdd.Focus()
        ''Me.lblMessage.Text = ""

    End Sub
      Function Validatetxt() As Boolean
        Dim bRetVal As Boolean = True

        If Me.txt_Line1.Text.Trim = "" Then
            Me.lblMessage.Text = "Line1 is required."
            MPEDetails.VisibleOnPageLoad = True
            Return False
        End If

         If Me.txt_Line2.Text.Trim = "" Then

            Me.lblMessage.Text = "Line2 is required."
            MPEDetails.VisibleOnPageLoad = True
            Return False
        End If
        If Me.txt_Line3.Text.Trim = "" Then

            Me.lblMessage.Text = "Line3 is required."
            MPEDetails.VisibleOnPageLoad = True
            Return False

        End If
Return True
End Function
 Function ValidateAddImg()
    Dim bRetVal As Boolean = True
    If Not fUpload.HasFile Then

            Me.lblMessage.Text = "Please Select a file for A4 printer"
            MPEDetails.VisibleOnPageLoad = True
             bRetVal = False
   Else

                Dim fileName As String = fUpload.FileName
                Dim exten As String = Path.GetExtension(fileName)
                'here we have to restrict file type            
                exten = exten.ToLower()
                Dim acceptedFileTypes As String() = New String(3) {}
                acceptedFileTypes(0) = ".png"
                Dim acceptFile As Boolean = False
                For i As Integer = 0 To 3
                    If exten = acceptedFileTypes(i) Then
                        acceptFile = True
                    End If
                Next
                If Not acceptFile Then
                    Me.lblMessage.Text = "The A4 printer file you are trying to upload is not a permitted file type!"
                    lblMessage.ForeColor = Drawing.Color.Red
                MPEDetails.VisibleOnPageLoad = True
                    bRetVal = False
                Else
                    Dim imgFile As System.Drawing.Image = System.Drawing.Image.FromStream(fUpload.PostedFile.InputStream)
                If imgFile.PhysicalDimension.Width <> 1960 OrElse imgFile.PhysicalDimension.Height <> 277 Then
                    Me.lblMessage.Text = "The file dimension of A4 printer should be 1960px x 277px"
                    lblMessage.ForeColor = Drawing.Color.Red
                    MPEDetails.VisibleOnPageLoad = True
                     bRetVal = False
                End If
       End If
   End If
   Return bRetVal
 End Function
 Function ValidateEditImg()

    Dim bRetVal As Boolean = True
    If rdo_keepSamefile.SelectedItem.Value = "N" Then
     If Not fUpload.HasFile Then

            Me.lblMessage.Text = "Please Select a file for A4 printer"
                MPEDetails.VisibleOnPageLoad = True
             bRetVal = False
      Else

                Dim fileName As String = fUpload.FileName
                Dim exten As String = Path.GetExtension(fileName)
                'here we have to restrict file type            
                exten = exten.ToLower()
                Dim acceptedFileTypes As String() = New String(3) {}
                acceptedFileTypes(0) = ".png"
                Dim acceptFile As Boolean = False
                For i As Integer = 0 To 3
                    If exten = acceptedFileTypes(i) Then
                        acceptFile = True
                    End If
                Next
                If Not acceptFile Then
                    Me.lblMessage.Text = "The A4 printer file you are trying to upload is not a permitted file type!"
                    lblMessage.ForeColor = Drawing.Color.Red
                    MPEDetails.VisibleOnPageLoad = True
                    bRetVal = False
                Else
                    Dim imgFile As System.Drawing.Image = System.Drawing.Image.FromStream(fUpload.PostedFile.InputStream)
                 If imgFile.PhysicalDimension.Width <> 1960 OrElse imgFile.PhysicalDimension.Height <> 277 Then
                    Me.lblMessage.Text = "The file dimension of A4 printer should be 1960px x 277px"
                    lblMessage.ForeColor = Drawing.Color.Red
                        MPEDetails.VisibleOnPageLoad = True
                     bRetVal = False
                 End If
                End If


    End If
   Else
        If hImgFile.Value = "" Then

            Me.lblMessage.Text = "Please Select a file"
            Me.lblMessage.ForeColor = Drawing.Color.Red
                MPEDetails.VisibleOnPageLoad = True
             bRetVal = False
        End If
   End If
   Return bRetVal
 End Function


 Function ValidateAddSmallImg()
    Dim bRetVal As Boolean = True

        If Not fUpload4inc.HasFile Then

            Me.lblMessage.Text = "Please Select a file for 4 inch"
            MPEDetails.VisibleOnPageLoad = True
            bRetVal = False
        Else
            Dim fileName As String = fUpload4inc.FileName
            Dim exten As String = Path.GetExtension(fileName)
            'here we have to restrict file type            
            exten = exten.ToLower()
            Dim acceptedFileTypes As String() = New String(3) {}
            acceptedFileTypes(0) = ".png"
            Dim acceptFile As Boolean = False
            For i As Integer = 0 To 3
                If exten = acceptedFileTypes(i) Then
                    acceptFile = True
                End If
            Next
            If Not acceptFile Then
                Me.lblMessage.Text = "The 4 inch printer file you are trying to upload is not a permitted file type!"
                lblMessage.ForeColor = Drawing.Color.Red
                MPEDetails.VisibleOnPageLoad = True
                bRetVal = False
            Else
                Dim imgFile As System.Drawing.Image = System.Drawing.Image.FromStream(fUpload4inc.PostedFile.InputStream)
                If imgFile.PhysicalDimension.Width <> 825 OrElse imgFile.PhysicalDimension.Height <> 285 Then
                    Me.lblMessage.Text = "The file dimension for 4 inch printer should be 825px x 285px"
                    lblMessage.ForeColor = Drawing.Color.Red
                    MPEDetails.VisibleOnPageLoad = True
                    bRetVal = False
                End If
            End If
        End If
        Return bRetVal
    End Function
 Function ValidateEditSmallImg()

    Dim bRetVal As Boolean = True
    If fUpload4inc.HasFile Then


                Dim fileName As String = fUpload4inc.FileName
                Dim exten As String = Path.GetExtension(fileName)
                'here we have to restrict file type            
                exten = exten.ToLower()
                Dim acceptedFileTypes As String() = New String(3) {}
                acceptedFileTypes(0) = ".png"
             
                Dim acceptFile As Boolean = False
                For i As Integer = 0 To 3
                    If exten = acceptedFileTypes(i) Then
                        acceptFile = True
                    End If
                Next
                If Not acceptFile Then
                    Me.lblMessage.Text = "The 4 inch printer file you are trying to upload is not a permitted file type!"
                    lblMessage.ForeColor = Drawing.Color.Red
                     MPEDetails.VisibleOnPageLoad = True
                    bRetVal = False
                Else
                    Dim imgFile As System.Drawing.Image = System.Drawing.Image.FromStream(fUpload4inc.PostedFile.InputStream)
                 If imgFile.PhysicalDimension.Width <> 825 OrElse imgFile.PhysicalDimension.Height <> 285 Then
                    Me.lblMessage.Text = "The file dimension for 4 inch printer should be 825px x 285px"
                    lblMessage.ForeColor = Drawing.Color.Red
                    MPEDetails.VisibleOnPageLoad = True
                     bRetVal = False
                 End If
                End If

        Else
            If hImgFile4inc.Value = "" Then

                Me.lblMessage.Text = "Please provide a image file for 4 inch printer"
                Me.lblMessage.ForeColor = Drawing.Color.Red
                MPEDetails.VisibleOnPageLoad = True
                bRetVal = False
            End If

        End If
  
   Return bRetVal
 End Function
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim type As String = ""
        Dim PhysicalPath As String = ""

        If ddl_Type.SelectedItem.Value = "ORG_LOGO" Then
            type = "Organisation"
            PhysicalPath = objCommon.GetLogoPath(Err_No, Err_Desc, "ORG_LOGO")
        End If
        If ddl_Type.SelectedItem.Value = "VAN_LOGO" Then
            type = "Van"
             PhysicalPath = objCommon.GetLogoPath(Err_No, Err_Desc, "VAN_LOGO")
        End If
        If ddl_Type.SelectedItem.Value = "CLIENT_LOGO" Then
              type = "Client"
              PhysicalPath = objCommon.GetLogoPath(Err_No, Err_Desc, "CLNT_LOGO")
        End If




        If ddl_Type.SelectedItem.Value = "VAN_LOGO" And HCommonHdrforVan.Value = "Y" Then
            If Me.ddl_vans.CheckedItems Is Nothing Then

                Me.lblMessage.Text = "Please Select " & type
                MPEDetails.VisibleOnPageLoad = True
                Exit Sub
            Else
                If Me.ddl_vans.CheckedItems.Count = 0 Then

                    Me.lblMessage.Text = "Please Select " & type
                    MPEDetails.VisibleOnPageLoad = True
                    Exit Sub
                End If
            End If

        Else
                If Me.ddl_Client.SelectedItem.Value = "0" Then

                    Me.lblMessage.Text = "Please Select " & type
                    MPEDetails.VisibleOnPageLoad = True
                    Exit Sub
                End If

        End If
       
        Dim InvPrintHeader As String
            InvPrintHeader = objCommon.GetAppConfig(Err_No, Err_Desc, "INVOICE_PRINT_HEADER")

        Dim PrinterType As String
        PrinterType = objCommon.GetAppConfig(Err_No, Err_Desc, "PRINTER_TYPE")

        If InvPrintHeader = "I" Then
            If PrinterType = "A4" Then
                If ValidateAddImg() = False Then
                    Exit Sub
                End If
            Else
                If ValidateAddSmallImg() = False Then
                    Exit Sub
                End If
            End If
        End If
         If InvPrintHeader = "T" Then
           If Validatetxt() = False Then
                Exit Sub
           End If
         End If
        If InvPrintHeader = "B" Then
            If Validatetxt() = False Then
                Exit Sub
            Else
                If PrinterType = "A4" Then
                    If ValidateAddImg() = False Then
                        Exit Sub
                    End If
                Else
                    If ValidateAddSmallImg() = False Then
                        Exit Sub
                    End If
                End If
            End If
        End If
        Dim success As Boolean = False
        Try

            Dim fileName As String = ""
            Dim fileName4inc As String = ""
            Dim LogoGuid As String = Guid.NewGuid.ToString()
            If InvPrintHeader = "B" Or InvPrintHeader = "I" Then
                Dim exten As String = Path.GetExtension(fUpload.FileName)

                fileName = LogoGuid + exten
                fUpload.SaveAs(PhysicalPath + "\" + fileName)

                If fUpload4inc.HasFile Then
                    exten = Path.GetExtension(fUpload4inc.FileName)

                    fileName4inc = LogoGuid + exten
                    fUpload4inc.SaveAs(PhysicalPath + "\" + fileName4inc)
                End If
            End If
            If ddl_Type.SelectedItem.Value = "VAN_LOGO" And HCommonHdrforVan.Value = "Y" Then
                Dim bSaved As Boolean = True
                Dim commonid As String = Guid.NewGuid().ToString()
                For Each itm As RadComboBoxItem In ddl_vans.CheckedItems
                    If bSaved = objCommon.SaveCommonPrintHeader(Err_No, Err_Desc, itm.Value, txt_Line1.Text.Trim, txt_Line2.Text.Trim, txt_Line3.Text.Trim, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, fileName, ddl_Type.SelectedItem.Value, InvPrintHeader, fileName4inc, commonid, txt_Line7.Text.Trim, txt_Line8.Text.Trim, txt_Line9.Text.Trim) = True Then
                        objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", type.ToUpper() & " LOGO", HidVal.Value, itm.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                        If bSaved = True Then
                            bSaved = True
                        End If
                    Else
                        bSaved = False
                    End If
                Next
                If bSaved Then
                    success = True
                    MessageBoxValidation("Successfully saved.", "Information")
                    Resetfields()
                    MPEDetails.VisibleOnPageLoad = False
                Else

                    success = False
                    MessageBoxValidation("Error while saving", "Information")
                    Resetfields()
                    MPEDetails.VisibleOnPageLoad = False
                End If
                BindData()
            Else

                If objCommon.SavePrintHeader(Err_No, Err_Desc, ddl_Client.SelectedItem.Value, txt_Line1.Text.Trim, txt_Line2.Text.Trim, txt_Line3.Text.Trim, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, fileName, ddl_Type.SelectedItem.Value, InvPrintHeader, fileName4inc, txt_Line7.Text.Trim, txt_Line8.Text.Trim, txt_Line9.Text.Trim) = True Then
                    success = True
                    MessageBoxValidation("Successfully saved.", "Information")
                    Resetfields()
                    MPEDetails.VisibleOnPageLoad = False
                Else
                    success = False
                    MessageBoxValidation("Error while saving", "Information")
                    Resetfields()
                    MPEDetails.VisibleOnPageLoad = False
                End If
                If success = True Then
                    objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", type.ToUpper() & " LOGO", HidVal.Value, Me.ddl_Client.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                    BindData()


                End If
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
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Private Sub BindData()
        Dt = objCommon.GetPrintHeaders(Err_No, Err_Desc, ddl_Type.SelectedItem.Value, txtFilterVal.Text.Trim)

        Dim dv As New DataView(Dt)
        If ViewState("SortField") <> "" Then
            dv.Sort = (ViewState("SortField") & " ") + SortDirection
        End If

        If ddl_Type.SelectedItem.Value = "CLIENT_LOGO" Then
            btnAdd.Text = "Add Client Print Header"
            grdCustomerSegment.Columns(1).HeaderText = "Client"
        End If
        If ddl_Type.SelectedItem.Value = "ORG_LOGO" Then
            btnAdd.Text = "Add Org Print Header"
             grdCustomerSegment.Columns(1).HeaderText = "Organisation"
        End If
        If ddl_Type.SelectedItem.Value = "VAN_LOGO" Then
            btnAdd.Text = "Add Van Print Header"
            grdCustomerSegment.Columns(1).HeaderText = "Van"
        End If
        grdCustomerSegment.DataSource = dv
        grdCustomerSegment.DataBind()
        Dim InvPrintHeader As String
        InvPrintHeader = objCommon.GetAppConfig(Err_No, Err_Desc, "INVOICE_PRINT_HEADER")
        If InvPrintHeader = "I" Then
            ' grdCustomerSegment.Columns(6).Visible = True
            grdCustomerSegment.Columns(9).Visible = True  ' Task4  new line has been added by RK

            grdCustomerSegment.Columns(2).Visible = False
            grdCustomerSegment.Columns(3).Visible = False
            grdCustomerSegment.Columns(4).Visible = False


            grdCustomerSegment.Columns(6).Visible = False    ' Task4  new line has been added by RK
            grdCustomerSegment.Columns(7).Visible = False
            grdCustomerSegment.Columns(8).Visible = False






            img.Visible = True
            txt.Visible = False
        ElseIf InvPrintHeader = "T" Then
            'grdCustomerSegment.Columns(6).Visible = False
            grdCustomerSegment.Columns(9).Visible = False  ' Task4  new line has been added by RK
            grdCustomerSegment.Columns(6).Visible = True
            grdCustomerSegment.Columns(7).Visible = True
            grdCustomerSegment.Columns(8).Visible = True

            grdCustomerSegment.Columns(2).Visible = True
            grdCustomerSegment.Columns(3).Visible = True
            grdCustomerSegment.Columns(4).Visible = True
            img.Visible = False
            txt.Visible = True
        ElseIf InvPrintHeader = "B" Then
            ' grdCustomerSegment.Columns(6).Visible = True
            grdCustomerSegment.Columns(9).Visible = True    ' Task4  new line has been added by RK
            grdCustomerSegment.Columns(6).Visible = True
            grdCustomerSegment.Columns(7).Visible = True
            grdCustomerSegment.Columns(8).Visible = True

            grdCustomerSegment.Columns(2).Visible = True
            grdCustomerSegment.Columns(3).Visible = True
            grdCustomerSegment.Columns(4).Visible = True
            img.Visible = True
            txt.Visible = True
        End If

        Dim PrintType = objCommon.GetAppConfig(Err_No, Err_Desc, "PRINTER_TYPE").ToUpper()
        If PrintType = "A4" Then
            For Each gr As GridViewRow In grdCustomerSegment.Rows
                If Not gr.FindControl("imgLogo") Is Nothing Then
                    CType(gr.FindControl("imgLogo"), ImageButton).Visible = True
                End If
                If Not gr.FindControl("imgLogo2") Is Nothing Then
                    CType(gr.FindControl("imgLogo2"), ImageButton).Visible = False
                End If
            Next
        Else
            For Each gr As GridViewRow In grdCustomerSegment.Rows
                If Not gr.FindControl("imgLogo") Is Nothing Then
                    CType(gr.FindControl("imgLogo"), ImageButton).Visible = False
                End If
                If Not gr.FindControl("imgLogo2") Is Nothing Then
                    CType(gr.FindControl("imgLogo2"), ImageButton).Visible = True
                End If
            Next
        End If
    End Sub
    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
           Dim type As String = ""
            Dim InvPrintHeader As String
        Dim PhysicalPath As String = ""
        Dim PrinterType As String = ""

        If ddl_Type.SelectedItem.Value = "ORG_LOGO" Then
            type = "Organisation"
            PhysicalPath = objCommon.GetLogoPath(Err_No, Err_Desc, "ORG_LOGO")
        End If
        If ddl_Type.SelectedItem.Value = "VAN_LOGO" Then
            type = "Van"
             PhysicalPath = objCommon.GetLogoPath(Err_No, Err_Desc, "VAN_LOGO")
        End If
        If ddl_Type.SelectedItem.Value = "CLIENT_LOGO" Then
              type = "Client"
              PhysicalPath = objCommon.GetLogoPath(Err_No, Err_Desc, "CLNT_LOGO")
        End If

        If ddl_Type.SelectedItem.Value = "VAN_LOGO" And HCommonHdrforVan.Value = "Y" Then
            If Me.ddl_vans.CheckedItems Is Nothing Then

                Me.lblMessage.Text = "Please Select " & type
                MPEDetails.VisibleOnPageLoad = True
                Exit Sub
            Else
                If Me.ddl_vans.CheckedItems.Count = 0 Then

                    Me.lblMessage.Text = "Please Select " & type
                    MPEDetails.VisibleOnPageLoad = True
                    Exit Sub
                End If
            End If

        Else
            If Me.ddl_Client.SelectedItem.Value = "0" Then

                Me.lblMessage.Text = "Please Select " & type
                MPEDetails.VisibleOnPageLoad = True
                Exit Sub
            End If

        End If


            InvPrintHeader = objCommon.GetAppConfig(Err_No, Err_Desc, "INVOICE_PRINT_HEADER")
        PrinterType = objCommon.GetAppConfig(Err_No, Err_Desc, "PRINTER_TYPE")
         If InvPrintHeader = "I" Then
            If PrinterType = "A4" Then
                If ValidateEditImg() = False Then
                    Exit Sub
                End If
            Else
                If ValidateEditSmallImg() = False Then
                    Exit Sub
                End If
            End If

         End If
         If InvPrintHeader = "T" Then
           If Validatetxt() = False Then
                Exit Sub
           End If
         End If
         

        If InvPrintHeader = "B" Then
            If Validatetxt() = False Then
                Exit Sub
            Else
                If PrinterType = "A4" Then
                    If ValidateEditImg() = False Then
                        Exit Sub
                    End If
                Else
                    If ValidateEditSmallImg() = False Then
                        Exit Sub
                    End If
                End If
            End If
        End If


        Dim success As Boolean = False
        Try
            Dim fileName As String = ""
            Dim fileName4inch As String = ""
            Dim LogoGuid As String = Guid.NewGuid.ToString()
            If InvPrintHeader = "B" Or InvPrintHeader = "I" Then
              If rdo_keepSamefile.SelectedItem.Value = "N" Then
                Dim exten As String = Path.GetExtension(fUpload.FileName)

                fileName = LogoGuid + exten
                fUpload.SaveAs(PhysicalPath + "\" + fileName)
              Else
                    fileName = hImgFile.Value
              End If

              If fUpload4inc.HasFile Then
                    Dim exten As String = Path.GetExtension(fUpload4inc.FileName)
                    LogoGuid = Guid.NewGuid.ToString()
                    fileName4inch = LogoGuid + exten
                    fUpload4inc.SaveAs(PhysicalPath + "\" + fileName4inch)
              Else
                    fileName4inch = hImgFile4inc.Value
              End If
            End If
            If ddl_Type.SelectedItem.Value = "VAN_LOGO" And HCommonHdrforVan.Value = "Y" Then
                Dim bSaved As Boolean = True
                Dim commonid As String = Guid.NewGuid().ToString()


                objCommon.DeleteCommonVanLogo(Err_No, Err_Desc, hCommonVanOrgID.Value)
                For Each itm As RadComboBoxItem In ddl_vans.CheckedItems
                    If bSaved = objCommon.SaveCommonPrintHeader(Err_No, Err_Desc, itm.Value, txt_Line1.Text.Trim, txt_Line2.Text.Trim, txt_Line3.Text.Trim, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, fileName, ddl_Type.SelectedItem.Value, InvPrintHeader, fileName4inch, commonid, txt_Line7.Text.Trim, txt_Line8.Text.Trim, txt_Line9.Text.Trim) = True Then
                        objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", type.ToUpper() & " LOGO", HidVal.Value, itm.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                        If bSaved = True Then
                            bSaved = True
                        End If
                    Else
                        bSaved = False
                    End If
                Next
                If bSaved Then
                    success = True
                    MessageBoxValidation("Successfully saved.", "Information")
                    Resetfields()
                    MPEDetails.VisibleOnPageLoad = False
                Else

                    success = False
                    MessageBoxValidation("Error while saving", "Information")
                    Resetfields()
                    MPEDetails.VisibleOnPageLoad = False
                End If
                BindData()

            Else
                If objCommon.SavePrintHeader(Err_No, Err_Desc, ddl_Client.SelectedItem.Value, txt_Line1.Text.Trim, txt_Line2.Text.Trim, txt_Line3.Text.Trim, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, fileName, ddl_Type.SelectedItem.Value, InvPrintHeader, fileName4inch, txt_Line7.Text.Trim, txt_Line8.Text.Trim, txt_Line9.Text.Trim) = True Then
                    success = True
                    MessageBoxValidation("Successfully saved.", "Information")
                    Resetfields()
                    MPEDetails.VisibleOnPageLoad = False
                Else
                    success = False
                    MessageBoxValidation("Error while saving", "Information")
                    Resetfields()
                    MPEDetails.VisibleOnPageLoad = False
                End If

                If success = True Then
                    objLogin.SaveUserLog(Err_No, Err_Desc, "I", "MAIN MENU", type.ToUpper() & " LOGO", HidVal.Value, Me.ddl_Client.SelectedItem.Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                    BindData()
                    Resetfields()

                End If
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
        ClassUpdatePnl.Update()
         Dim type As String = ""
        If ddl_Type.SelectedItem.Value = "ORG_LOGO" Then
            type = "Organisation"
        End If
        If ddl_Type.SelectedItem.Value = "VAN_LOGO" Then
            type = "Van"
        End If
        If ddl_Type.SelectedItem.Value = "CLIENT_LOGO" Then
              type = "Client"
        End If

            Dim dr As GridViewRow
            Dim Success As Boolean = False
            For Each dr In grdCustomerSegment.Rows
                Dim RowCheckBox As System.Web.UI.WebControls.CheckBox = dr.FindControl("chkDelete")

                If RowCheckBox.Checked = True Then
                    Dim Lbl As System.Web.UI.WebControls.Label = dr.FindControl("lblRow_ID")
                    HidVal.Value = Lbl.Text
                    Dim Des = dr.Cells(1).Text
                    If objCommon.DeleteClientLogo(Err_No, Err_Desc, HidVal.Value) = True Then
                        objLogin.SaveUserLog(Err_No, Err_Desc, "D", "MAIN MENU", type.ToUpper & " LOGO", HidVal.Value, Des, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                        Success = True
                    End If
                End If
            Next
            If (Success = True) Then
                MessageBoxValidation("Print Logo(s) deleted successfully.", "Information")
                BindData()
                Resetfields()
            Else

                MessageBoxValidation("Some Print Logo(s) could not be deleted.", "Information")
                BindData()
                Resetfields()
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

        Try
            BindData()
            ClassUpdatePnl.Update()

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
     Dim type As String = ""
        If ddl_Type.SelectedItem.Value = "ORG_LOGO" Then
            type = "Organisation"
        End If
        If ddl_Type.SelectedItem.Value = "VAN_LOGO" Then
            type = "Van"
        End If
        If ddl_Type.SelectedItem.Value = "CLIENT_LOGO" Then
              type = "Client"
        End If

        Dim btndelete As ImageButton = TryCast(sender, ImageButton)
        Dim row As GridViewRow = DirectCast(btndelete.NamingContainer, GridViewRow)

        Dim success As Boolean = False

        Dim Lbl As System.Web.UI.WebControls.Label = row.FindControl("lblRow_ID")
        HidVal.Value = Lbl.Text

        Try

            If objCommon.DeleteClientLogo(Err_No, Err_Desc, HidVal.Value) = True Then
                        success = True
            End If
            If success = True Then
                objLogin.SaveUserLog(Err_No, Err_Desc, "D", "MAIN MENU", type.ToUpper() & " LOGO", HidVal.Value, row.Cells(1).Text, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                MessageBoxValidation("Print Logo(s) deleted successfully.", "Information")
                BindData()
                Resetfields()
            Else
                MessageBoxValidation("Print Logo(s) could not be deleted.", "Information")
                Resetfields()
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
        bindClient()
        btnUpdate.Visible = False
        btnSave.Visible = True
        ddl_Client.Enabled = True
        Resetfields()
        rdo_keepSamefile.Visible = False
        img_Logo.Visible = False
        Label1.Visible = False
        img_Logo4inc.Visible = False
        lbl_logo24inc.Visible = False
        ddl_Client.Visible = True
        ddl_vans.Visible = False
        lblMessage.Text = ""
    If ddl_Type.SelectedItem.Value = "CLIENT_LOGO" Then
            MPEDetails.Title = "Client Logo"
         lbl_Info_Key.Text = "Client"
    End If
    If ddl_Type.SelectedItem.Value = "ORG_LOGO" Then
            MPEDetails.Title = "Organisation Logo"
         lbl_Info_Key.Text = "Organisation"
    End If
    If ddl_Type.SelectedItem.Value = "VAN_LOGO" Then
            MPEDetails.Title = "Van Logo"
            lbl_Info_Key.Text = "Van"

            If HCommonHdrforVan.Value = "Y" Then
                ddl_Client.Visible = False
                ddl_vans.Visible = True
            Else
                ddl_Client.Visible = True
                ddl_vans.Visible = False
            End If

    End If
        ClassUpdatePnl.Update()
        MPEDetails.VisibleOnPageLoad = True
    End Sub
Sub bindClient()
    If ddl_Type.SelectedItem.Value = "CLIENT_LOGO" Then
            ddl_Client.DataSource = objCommon.GetClientsWthNoLogos(Err_No, Err_Desc)
            ddl_Client.DataTextField = "Location"
            ddl_Client.DataValueField = "Location"
            ddl_Client.DataBind()
            ddl_Client.Items.Insert(0, New RadComboBoxItem("(Select)", "0"))
            ddl_Client.Items(0).Value = "0"
            ClassUpdatePnl.Update()
    End If
    If ddl_Type.SelectedItem.Value = "ORG_LOGO" Then
            ddl_Client.DataSource = objCommon.GetOrgsWthNoLogos(Err_No, Err_Desc)
        ddl_Client.DataTextField = "Description"
        ddl_Client.DataValueField = "ORG_HE_ID"
        ddl_Client.DataBind()
            ddl_Client.Items.Insert(0, New RadComboBoxItem("(Select)", "0"))
        ddl_Client.Items(0).Value = "0"
        ClassUpdatePnl.Update()
    End If
    If ddl_Type.SelectedItem.Value = "VAN_LOGO" Then
            ddl_Client.DataSource = objCommon.GetVansWthNoLogos(Err_No, Err_Desc)
        ddl_Client.DataTextField = "Org_ID"
        ddl_Client.DataValueField = "Org_ID"
        ddl_Client.DataBind()
            ddl_Client.Items.Insert(0, New RadComboBoxItem("(Select)", "0"))
            ddl_Client.Items(0).Value = "0"


            ddl_vans.DataSource = objCommon.GetVanfromUser(Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID)
            ddl_vans.DataTextField = "Org_ID"
            ddl_vans.DataValueField = "Org_ID"
            ddl_vans.DataBind()

            ClassUpdatePnl.Update()

    End If
End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            ddl_Client.Visible = True
            ddl_vans.Visible = False
            lblMessage.Text = ""
            Dim RowCheckBox As System.Web.UI.WebControls.CheckBox = grdCustomerSegment.Rows(0).FindControl("chkDelete")
            btnUpdate.Visible = True
            btnSave.Visible = False
            Resetfields()
            Dim btnEdit As ImageButton = TryCast(sender, ImageButton)
            Dim row As GridViewRow = DirectCast(btnEdit.NamingContainer, GridViewRow)
            Dim Lbl As System.Web.UI.WebControls.Label = row.FindControl("lblRow_ID")
            Dim hd As System.Web.UI.WebControls.Label = row.FindControl("hlogo")
            Dim hd4inch As System.Web.UI.WebControls.Label = row.FindControl("hlogo4inch")

            Dim vhd As System.Web.UI.WebControls.Label = row.FindControl("vlogo")
            Dim vhd4inch As System.Web.UI.WebControls.Label = row.FindControl("vLogo4inch")

            Dim lbl_key As System.Web.UI.WebControls.Label = row.FindControl("lbl_key")

            Dim lbl_Value2 As System.Web.UI.WebControls.Label = row.FindControl("lblVal2")


            Dim lbl_Value7 As System.Web.UI.WebControls.Label = row.FindControl("lblVal7")  ' Task 4

            Dim lbl_Value8 As System.Web.UI.WebControls.Label = row.FindControl("lblVal8")

            Dim lbl_Value9 As System.Web.UI.WebControls.Label = row.FindControl("lblVal9")


            hCommonVanOrgID.Value = lbl_Value2.Text

            HidVal.Value = Lbl.Text
            If ddl_Type.SelectedItem.Value = "VAN_LOGO" And HCommonHdrforVan.Value = "Y" Then
                Dim dtvs As New DataTable
                ddl_vans.DataSource = objCommon.GetVanfromUser(Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID)
                ddl_vans.DataTextField = "Org_ID"
                ddl_vans.DataValueField = "Org_ID"
                ddl_vans.DataBind()
                dtvs = objCommon.GetVansSharingCommonHdr(Err_No, Err_Desc, lbl_Value2.Text)
                If dtvs.Rows.Count > 0 Then
                    For Each dr In dtvs.Rows
                        If Not ddl_vans.FindItemByValue(dr(0)) Is Nothing Then
                            ddl_vans.FindItemByValue(dr(0)).Checked = True
                        End If
                    Next
                End If
                dtvs = Nothing
            Else
                ddl_Client.Items.Add(New RadComboBoxItem(row.Cells(1).Text, lbl_key.Text))
                If Not ddl_Client.FindItemByValue(lbl_key.Text) Is Nothing Then
                    ddl_Client.FindItemByValue(lbl_key.Text).Selected = True
                End If
                ddl_Client.Enabled = False
            End If
           
            txt_Line1.Text = Trim(row.Cells(2).Text)
            txt_Line2.Text = Trim(row.Cells(3).Text)
            txt_Line3.Text = Trim(row.Cells(4).Text)
            ' Task 4 
            txt_Line7.Text = Trim(row.Cells(5).Text)
            txt_Line8.Text = Trim(row.Cells(6).Text)
            txt_Line9.Text = Trim(row.Cells(7).Text)




            If txt_Line1.Text = "&nbsp;" Then
                txt_Line1.Text = ""
            End If
            If txt_Line2.Text = "&nbsp;" Then
                txt_Line2.Text = ""
            End If
            If txt_Line3.Text = "&nbsp;" Then
                txt_Line3.Text = ""
            End If
            If hd.Text = "N/A" Then
                img_Logo.Visible = False
                lbl_logo2.Text = "Logo not set"
                hImgFile.Value = ""
            Else
                img_Logo.Visible = True
                img_Logo.ImageUrl = vhd.Text
                lbl_logo2.Text = ""
                hImgFile.Value = hd.Text
                lbl_logo2.Visible = False
            End If
            If hd4inch.Text.Trim = "" Then
                img_Logo4inc.Visible = False
                lbl_logo24inc.Text = "Logo not set"
                hImgFile4inc.Value = ""
            Else
                img_Logo4inc.Visible = True
                img_Logo4inc.ImageUrl = vhd4inch.Text
                lbl_logo24inc.Text = ""
                hImgFile4inc.Value = hd4inch.Text
                lbl_logo24inc.Visible = False
            End If
            Dim InvPrintHeader As String
            InvPrintHeader = objCommon.GetAppConfig(Err_No, Err_Desc, "INVOICE_PRINT_HEADER")
            If ddl_Type.SelectedItem.Value = "CLIENT_LOGO" Then
                MPEDetails.Title = "Client Logo"
                lbl_Info_Key.Text = "Client"
            End If
            If ddl_Type.SelectedItem.Value = "ORG_LOGO" Then
                MPEDetails.Title = "Organisation Logo"
                lbl_Info_Key.Text = "Organisation"
            End If
            If ddl_Type.SelectedItem.Value = "VAN_LOGO" Then
                MPEDetails.Title = "Van Logo"
                lbl_Info_Key.Text = "Van"

                If HCommonHdrforVan.Value = "Y" Then
                    ddl_Client.Visible = False
                    ddl_vans.Visible = True
                Else
                    ddl_Client.Visible = True
                    ddl_vans.Visible = False
                End If

            End If
            rdo_keepSamefile.Items.FindByValue("Y").Selected = True
            If InvPrintHeader = "I" Then
                'grdCustomerSegment.Columns(6).Visible = True
                grdCustomerSegment.Columns(9).Visible = True


                grdCustomerSegment.Columns(2).Visible = False
                grdCustomerSegment.Columns(3).Visible = False
                grdCustomerSegment.Columns(4).Visible = False


                grdCustomerSegment.Columns(6).Visible = False
                grdCustomerSegment.Columns(7).Visible = False
                grdCustomerSegment.Columns(8).Visible = False

                img.Visible = True
                txt.Visible = False
                rdo_keepSamefile.Visible = True
                Label1.Visible = True
                img_Logo.Visible = True
            ElseIf InvPrintHeader = "T" Then
                ' grdCustomerSegment.Columns(6).Visible = False
                grdCustomerSegment.Columns(9).Visible = False


                grdCustomerSegment.Columns(2).Visible = True
                grdCustomerSegment.Columns(3).Visible = True
                grdCustomerSegment.Columns(4).Visible = True

                grdCustomerSegment.Columns(6).Visible = True
                grdCustomerSegment.Columns(7).Visible = True
                grdCustomerSegment.Columns(8).Visible = True

                img.Visible = False
                txt.Visible = True
                rdo_keepSamefile.Visible = False
                Label1.Visible = False
                img_Logo.Visible = False
            ElseIf InvPrintHeader = "B" Then
                ' grdCustomerSegment.Columns(6).Visible = True
                grdCustomerSegment.Columns(9).Visible = True


                grdCustomerSegment.Columns(2).Visible = True
                grdCustomerSegment.Columns(3).Visible = True
                grdCustomerSegment.Columns(4).Visible = True

                grdCustomerSegment.Columns(6).Visible = True
                grdCustomerSegment.Columns(7).Visible = True
                grdCustomerSegment.Columns(8).Visible = True



                img.Visible = True
                txt.Visible = True
                rdo_keepSamefile.Visible = True
                Label1.Visible = True
                img_Logo.Visible = True

            End If
            ' ClassUpdatePnl.Update()
            MPEDetails.VisibleOnPageLoad = True
        Catch ex As Exception
            Err_No = "74066"
            log.Error(Err_Desc)
            Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("C_BO_CurrencyCode_006") & "&next=AdminCustomerSegment.aspx&Title=Message", False)
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub

    Private Sub gvCurrency_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdCustomerSegment.PageIndexChanging
        grdCustomerSegment.PageIndex = e.NewPageIndex
        BindData()

    End Sub

    Private Sub gvCurrency_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdCustomerSegment.Sorting
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

    Protected Sub btnclearFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnclearFilter.Click
        txtFilterVal.Text = ""
        BindData()
         ClassUpdatePnl.Update()
    End Sub

    Private Sub ddl_Type_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Type.Load
      If Not ddl_Type.SelectedItem Is Nothing Then
         BindData()
         ClassUpdatePnl.Update()
         End If
    End Sub
End Class