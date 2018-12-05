Imports SalesWorx.BO.Common
Imports Telerik.Web.UI
Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports ExcelLibrary.SpreadSheet
Imports System.Threading
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Public Class ManageProductImage
    Inherits System.Web.UI.Page
    Dim objcommon As New SalesWorx.BO.Common.Common
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim objProduct As New SalesWorx.BO.Common.Product
    Dim objDivConfig As New DivConfig
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const PageID As String = "P335"
    Private dt As New DataTable
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim objControl As New AppControl
    Private _strTempFolder As String = CStr(ConfigurationSettings.AppSettings("ExcelPath"))
    Private PhysicalPath As String = ""
    Private _strMediaFileSize As Long = CLng(ConfigurationSettings.AppSettings("MediaFileSize"))
    Protected Sub ddl_org_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddOraganisation.SelectedIndexChanged
        ''LoadProduct()

        ddlItemCode.ClearSelection()
        ddlItemCode.Items.Clear()
        ddlItemCode.Text = ""

        BindProductImages()
    End Sub
    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As EventArgs)
   
        PhysicalPath = objcommon.GetMediaPath(Err_No, Err_Desc)

        Try
            If Me.ddOraganisation.SelectedIndex <= 0 Then
                MessageBoxValidation("Please select an organization", "Validation")
                Return
            End If
            If Me.ddlItemCode.SelectedValue = "" Then
                MessageBoxValidation("Please select a product", "Validation")
                Return
            End If
            If Me.txtCaption.Text = "" Then
                MessageBoxValidation("Please enter a caption", "Validation")
                Return
            End If
            If HImage.Value = "" Then
                If upMedia.UploadedFiles.Count <= 0 Then
                    MessageBoxValidation("Please upload an image file", "Validation")
                    Return
                End If
            End If
            'If Me.lblMediaFile.Text = "" Or (Me.lblThumbFile.Text = "" And Me.ddlMediaType.SelectedValue <> "Brochure") Then
            '    MessageBoxValidation("Please upload a media and thumbnail file", "Validation")
            '    Return
            'End If

            Me.lblMediaFile.Text = HImage.Value
            Dim filename1 As String = HThumbNail.Value

            Dim bExist As Boolean = False
            Dim fileName As String = ""
            Dim Path As String
            For Each file As UploadedFile In upMedia.UploadedFiles
                fileName = file.FileName
                Dim fName As String = System.IO.Path.GetFileNameWithoutExtension(fileName)
                Dim exten As String = System.IO.Path.GetExtension(fileName)
                Dim MediaGuid As String = Guid.NewGuid.ToString()
                exten = exten.ToLower()
                If Me.ddlItemCode.SelectedValue = "" Then
                    MessageBoxValidation("Please select a product", "Validation")
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", upMedia.ClientID), True)
                    Return
                End If
                If Me.txtCaption.Text = "" Or Me.txtCaption.Text = "0" Then
                    MessageBoxValidation("Please enter a caption", "Validation")
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", upMedia.ClientID), True)
                    Return
                End If



                If Not (exten = ".jpeg" Or exten = ".jpg" Or exten = ".bmp" Or exten = ".png" Or exten = ".gif") Then
                    MessageBoxValidation("Please select a image file only.", "Validation")
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", upMedia.ClientID), True)
                    Return
                End If

                If file.ContentLength > _strMediaFileSize Then

                    MessageBoxValidation("The file size should be less than or equal to " & _strMediaFileSize & " MB only", "Validation")
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "clearUpload", [String].Format("$find('{0}').deleteAllFileInputs()", upMedia.ClientID), True)
                    Exit Sub

                Else

                    Dim path1 As String = Nothing

                    filename1 = MediaGuid & "_" & "T" & "_" & fileName

                    path1 = System.IO.Path.Combine(PhysicalPath, filename1)

                    

                    fileName = MediaGuid & "_" & "M" & "_" & fileName

                    Me.lblMediaFile.Text = fileName
                    Path = System.IO.Path.Combine(PhysicalPath, fileName)
                    file.SaveAs(Path)

                    Dim imgFile As System.Drawing.Image = System.Drawing.Image.FromFile(Path)
                    Dim ThumbNailWidth As Decimal
                    ThumbNailWidth = imgFile.PhysicalDimension.Width
                    Dim ThumbNailHeight As Decimal
                    ThumbNailHeight = imgFile.PhysicalDimension.Height

                    If ThumbNailWidth > 120 Then
                        Dim Per As Decimal
                        Per = (Math.Abs(120.0 - ThumbNailWidth)) / ThumbNailWidth
                        ThumbNailWidth = 120
                        ThumbNailHeight = ThumbNailHeight - (ThumbNailHeight * Per)
                    End If
                    If ThumbNailHeight > 120 Then
                        Dim Per As Decimal
                        Per = (Math.Abs(120.0 - ThumbNailHeight)) / ThumbNailHeight
                        ThumbNailHeight = 120
                        ThumbNailWidth = ThumbNailWidth - (ThumbNailWidth * Per)
                    End If
                    imgFile.Dispose()
                    imgFile = Nothing
                    Dim oThumbNail As System.Drawing.Image = CreateThumbnail(Path, ThumbNailWidth, ThumbNailHeight)
                    oThumbNail.Save(path1, System.Drawing.Imaging.ImageFormat.Jpeg)

                End If


            Next

            Dim MediaFileID As String = Guid.NewGuid.ToString
            Dim ItemCode As String = ddlItemCode.SelectedValue

            If objProduct.SaveProductImage(Err_No, Err_Desc, MediaFileID, Me.ddlItemCode.SelectedValue, Me.ddOraganisation.SelectedValue, "Image", Me.lblMediaFile.Text, filename1, Me.txtCaption.Text, CType(Session("User_Access"), UserAccess).UserID, "Y") Then
                Dim ItemText As String = (New SalesWorx.BO.Common.Product).GetProdName(Err_No, Err_Desc, ItemCode)
                objLogin.SaveUserLog(Err_No, Err_Desc, "I", "PRODUCT MANAGEMENT", "PRODUCT IMAGE", Me.ddOraganisation.SelectedValue.ToString(), "Product: " & ItemText, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                MessageBoxValidation("Successfully saved", "Information")
                Session.Remove("MediaFileId")
                Session.Remove("UpFileName")
                Session.Remove("Path")
                If upMedia.UploadedFiles.Count > 0 Then
                    Me.upMedia.UploadedFiles.Clear()
                End If

                Me.lblMediaFile.Text = ""

                BindProductImages()
            Else
                MessageBoxValidation("Error while saving image", "Validation")
                Return
            End If

        Catch ex As Exception
            MessageBoxValidation("Error occured while saving image", "Validation")
            log.Error(ex.Message)
        End Try

    End Sub
    Public Shared Function CreateThumbnail(lcFilename As String, lnWidth As Integer, lnHeight As Integer) As Bitmap
        Dim bmpOut As System.Drawing.Bitmap = Nothing
        Try
            Dim loBMP As New Bitmap(lcFilename)
            Dim loFormat As ImageFormat = loBMP.RawFormat

            Dim lnRatio As Decimal
            Dim lnNewWidth As Integer = 0
            Dim lnNewHeight As Integer = 0

            ' If the image is smaller than a thumbnail just return it
            If loBMP.Width < lnWidth AndAlso loBMP.Height < lnHeight Then
                Return loBMP
            End If

            If loBMP.Width > loBMP.Height Then
                lnRatio = CDec(lnWidth) / loBMP.Width
                lnNewWidth = lnWidth
                Dim lnTemp As Decimal = loBMP.Height * lnRatio
                lnNewHeight = CInt(lnTemp)
            Else
                lnRatio = CDec(lnHeight) / loBMP.Height
                lnNewHeight = lnHeight
                Dim lnTemp As Decimal = loBMP.Width * lnRatio
                lnNewWidth = CInt(lnTemp)
            End If
            bmpOut = New Bitmap(lnNewWidth, lnNewHeight)
            Dim g As Graphics = Graphics.FromImage(bmpOut)
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
            g.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight)
            g.DrawImage(loBMP, 0, 0, lnNewWidth, lnNewHeight)

            loBMP.Dispose()
        Catch
            Return Nothing
        End Try

        Return bmpOut
    End Function

    Public Function ThumbnailCallback() As Boolean
        Return False
    End Function
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


            Dim SubQry As String = objcommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
            ddOraganisation.ClearSelection()
            ddOraganisation.Items.Clear()
            ddOraganisation.Text = ""
            ddOraganisation.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
            ddOraganisation.Items.Clear()
            ddOraganisation.Items.Add(New RadComboBoxItem("-- Select a Organization --", "0"))
            ddOraganisation.AppendDataBoundItems = True
            ddOraganisation.DataValueField = "MAS_Org_ID"
            ddOraganisation.DataTextField = "Description"
            ddOraganisation.DataBind()

            If ddOraganisation.Items.Count = 2 Then
                ddOraganisation.SelectedIndex = 1
            End If

            ''   ddOraganisation.SelectedIndex = 0
            LoadProduct()
            Me.upMedia.TemporaryFolder = _strTempFolder
        End If
    End Sub
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
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
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btndelete As ImageButton = TryCast(sender, ImageButton)
        Dim row As DataListItem = DirectCast(btndelete.NamingContainer, DataListItem)
        'System.Web.UI.WebControls.' 
        PhysicalPath = objcommon.GetMediaPath(Err_No, Err_Desc)

        Dim success As Boolean = False
        Try
            Dim ID As String
            ID = CType(row.FindControl("HID"), HiddenField).Value

            Dim HThumbFile As String
            HThumbFile = CType(row.FindControl("HThumbNail"), HiddenField).Value

            Dim HFile As String
            HFile = CType(row.FindControl("HFile"), HiddenField).Value

            If objProduct.DeleteProductImage(Err_No, Err_Desc, ID) = True Then
                If (File.Exists(PhysicalPath & "\" & HFile)) Then
                    Kill(PhysicalPath & "\" & HFile)
                End If
                If (File.Exists(PhysicalPath & "\" & HThumbFile)) Then
                    Kill(PhysicalPath & "\" & HThumbFile)
                End If

                objLogin.SaveUserLog(Err_No, Err_Desc, "D", "PRODUCT MANAGEMENT", "PRODUCT IMAGE", Me.ddOraganisation.SelectedValue.ToString(), "Product: " & ddlItemCode.SelectedValue, CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
                success = True
            End If

            If success = True Then
                MessageBoxValidation("Successfully deleted.", "Information")
                BindProductImages()
            Else
                MessageBoxValidation("Error occured while deleting product image.", "Information")
                log.Error(Err_Desc)
            End If

        Catch ex As Exception
            Err_No = "74210"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try

    End Sub
    Sub BindProductImages()
        Dim dtData As New DataTable

        If ddlItemCode.SelectedValue <> "" Then
            dtData = objProduct.GetProductImages(Err_No, Err_Desc, ddOraganisation.SelectedItem.Value, ddlItemCode.SelectedValue)
            Dim dv As New DataView(dtData)
            If ViewState("SortField") <> "" Then
                dv.Sort = (ViewState("SortField") & " ") + SortDirection
            End If

            Me.ImgList.DataSource = Nothing
            Me.ImgList.DataSource = dv
            Me.ImgList.DataBind()
            If ddlItemCode.SelectedValue <> "" Then
                If dtData.Rows.Count > 0 Then
                    HImage.Value = dtData.Rows(0)("Filename").ToString
                    HThumbNail.Value = dtData.Rows(0)("Thumbnail").ToString
                    txtCaption.Text = dtData.Rows(0)("Caption").ToString
                Else
                    HImage.Value = ""
                    HThumbNail.Value = ""
                    txtCaption.Text = ""
                End If

            End If
        End If
    End Sub
    Sub LoadProduct()
        ' ''Dim dt As New DataTable
        ' ''dt = objcommon.GetProductsByOrg(Err_No, Err_Desc, ddOraganisation.SelectedItem.Value)
        ' ''Dim dr As DataRow
        ' ''dr = dt.NewRow
        ' ''dr("Inventory_Item_ID") = "0"
        ' ''dr("Description") = "--Select Product--"
        ' ''dt.Rows.InsertAt(dr, 0)

        ' ''ddlItemCode.Items.Clear()
        ' ''ddlItemCode.DataSource = dt
        ' ''ddlItemCode.DataTextField = "Description"
        ' ''ddlItemCode.DataValueField = "Inventory_Item_ID"
        ' ''ddlItemCode.DataBind()
        ' ''ddlItemCode.SelectedIndex = 0
    End Sub

    Private Sub ddlItemCode_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles ddlItemCode.ItemsRequested
        Try

            Dim Objrep As New SalesWorx.BO.Common.Reports()
            Dim dt As New DataTable
            dt = Objrep.GetAllItemsByOrg(Err_No, Err_Desc, ddOraganisation.SelectedItem.Value, e.Text)


            Dim ItemsPerRequest As Integer = 100 'Set the number of values to show
            Dim itemOffset As Integer = e.NumberOfItems
            Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count)
            e.EndOfItems = endOffset = dt.Rows.Count

            'Loop through the values to populate the combo box
            For i As Integer = itemOffset To endOffset - 1
                Dim item As New RadComboBoxItem()
                item.Text = dt.Rows(i).Item("Description").ToString
                item.Value = dt.Rows(i).Item("Inventory_Item_ID").ToString

                ddlItemCode.Items.Add(item)
                item.DataBind()
            Next


        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub

   
    Private Sub ddlItemCode_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlItemCode.SelectedIndexChanged
        BindProductImages()
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        ddlItemCode.ClearSelection()
        ddlItemCode.SelectedIndex = 0
        ddlItemCode.Items.Clear()
        ddlItemCode.Text = ""
        txtCaption.Text = ""
        Me.upMedia.UploadedFiles.Clear()
        HImage.Value = ""
        HThumbNail.Value = ""
        BindProductImages()
    End Sub
End Class
