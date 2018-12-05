Imports Microsoft.Reporting.WebForms
Imports System.Configuration.ConfigurationManager
Public Class ShowAssetHistoryImages
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String

    Private Sub ShowAssetHistoryImages_Init(sender As Object, e As EventArgs) Handles Me.Init
        Me.Title = "Asset Images"
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Request.QueryString("Asset_ID") Is Nothing Then

                hfAssetID.Value = Request.QueryString("Asset_ID")
                hfRowID.Value = Request.QueryString("Row_ID")
                lbl_CusName.Text = Request.QueryString("CustName")
                lbl_AssetType.Text = Request.QueryString("AssetType")
                Dim logged_at As String = ""
                logged_at = Request.QueryString("LoggAt")
                lbl_logged_at.Text = CDate(logged_at).ToString("dd-MMM-yyyy hh:mm tt")
                BindAssetHistroryImages()
            End If

        End If

    End Sub
    Sub BindAssetHistroryImages()
        Dim dtData As New DataTable

        Dim ObjRpt As New SalesWorx.BO.Common.Reports
        dtData = ObjRpt.GetAssetViewHistory_Images(Err_No, Err_Desc, "", hfRowID.Value)
        If dtData.Rows.Count > 0 Then
            Dim dv As New DataView(dtData)
            If ViewState("SortField") <> "" Then
                dv.Sort = (ViewState("SortField") & " ") + SortDirection
            End If
            Me.ImgList.DataSource = Nothing
            Me.ImgList.DataSource = dv
            Me.ImgList.DataBind()
        End If


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
End Class