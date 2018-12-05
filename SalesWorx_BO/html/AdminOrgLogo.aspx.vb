Imports SalesWorx.BO.Common
Imports log4net
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO

Partial Public Class AdminOrgLogo
    Inherits System.Web.UI.Page
    Dim objCommon As New Common
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const PageID As String = "P259"
    Dim objLogin As New SalesWorx.BO.Common.Login
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub AdminOrgLogo_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Admin Org Logo"
    End Sub
    Private Sub BindData()
        Try
            Me.gvOrgLogo.DataSource = objCommon.LoadOrgLogo(Err_No, Err_Desc)
            Me.gvOrgLogo.DataBind()

        Catch ex As Exception
            Err_No = "74076"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
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

                Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddOraganisation.DataSource = objCommon.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddOraganisation.Items.Clear()
                ddOraganisation.Items.Add("-- Select a Organization --")
                ddOraganisation.AppendDataBoundItems = True
                ddOraganisation.DataValueField = "MAS_Org_ID"
                ddOraganisation.DataTextField = "Description"
                ddOraganisation.DataBind()
                BindData()

            End If
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_MSL_005") & "&next=Welcome.aspx&Title=Product List Setup", False)
        End Try

    End Sub

    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpload.Click

        Dim success As Boolean = False
        Try


            If Me.ddOraganisation.SelectedIndex <= 0 Then
                Me.lblMessage.Text = "Please select organization"
                lblMessage.ForeColor = Drawing.Color.Red
                lblinfo.Text = "Validation"
                MpInfoError.Show()
                btnClose.Focus()
                Exit Sub
            End If

         

          

            Dim LogoGuid As String = Guid.NewGuid.ToString()



            If fUpload.HasFile Then

                Dim PhysicalPath As String = objCommon.GetOrgLogoPath(Err_No, Err_Desc)
                Dim imgFile As System.Drawing.Image = System.Drawing.Image.FromStream(fUpload.PostedFile.InputStream)
                If imgFile.PhysicalDimension.Width > 1960 OrElse imgFile.PhysicalDimension.Height > 277 Then
                    Me.lblMessage.Text = "The file dimension should be 1960px x 277px"
                    lblMessage.ForeColor = Drawing.Color.Red
                    lblinfo.Text = "Information"
                    MpInfoError.Show()
                    btnClose.Focus()
                    Exit Sub
                End If



                Dim fileName As String = fUpload.FileName
                Dim exten As String = Path.GetExtension(fileName)
                'here we have to restrict file type            
                exten = exten.ToLower()
                Dim acceptedFileTypes As String() = New String(3) {}
                acceptedFileTypes(0) = ".jpg"
                acceptedFileTypes(1) = ".jpeg"
                acceptedFileTypes(2) = ".gif"
                acceptedFileTypes(3) = ".png"
                Dim acceptFile As Boolean = False
                For i As Integer = 0 To 3
                    If exten = acceptedFileTypes(i) Then
                        acceptFile = True
                    End If
                Next
                If Not acceptFile Then
                    Me.lblMessage.Text = "The file you are trying to upload is not a permitted file type!"
                    lblMessage.ForeColor = Drawing.Color.Red
                    lblinfo.Text = "Information"
                    MpInfoError.Show()
                    btnClose.Focus()
                    Exit Sub
                    'ElseIf fUpload.FileContent.Length > 102400 Then
                    '    Me.lblMessage.Text = "The file size should be 100kb only"
                    '    lblMessage.ForeColor = Drawing.Color.Red
                    '    lblinfo.Text = "Information"
                    '    MpInfoError.Show()
                    '    btnClose.Focus()
                    '    Exit Sub

                Else
                    fileName = LogoGuid + exten
                    fUpload.SaveAs(PhysicalPath + "\" + fileName)
                    objCommon.SaveOrgLogo(Err_No, Err_Desc, Me.ddOraganisation.SelectedValue, fileName, CType(Session("User_Access"), UserAccess).UserID)

                End If
            End If

            BindData()
            success = True
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Response.Redirect("information.aspx?mode=1&errno=" & "74071" & "&msg=" & AppMsgHandler.GetErrorMessage("SWX_BO_Msg_005") & "&next=AdminOrgLogo.aspx", False)
        Finally
        End Try
    End Sub

End Class