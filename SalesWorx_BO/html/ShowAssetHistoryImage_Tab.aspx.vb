Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Public Class ShowAssetHistoryImage_Tab
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

     
        If Not IsPostBack Then
            If Not Request.QueryString("SRC1") Is Nothing Then

                RadBinaryImage12.ImageUrl = Request.QueryString("SRC1")

            End If

        End If
        Catch ex As Exception
            log.Error(ex.Message.ToString())
        End Try
    End Sub
    
    
End Class