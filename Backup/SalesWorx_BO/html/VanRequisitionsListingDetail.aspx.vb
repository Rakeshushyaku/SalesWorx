Imports SalesWorx.BO.Common
Partial Public Class VanRequisitionsListingDetail
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const ModuleName As String = "VanRequisitionsListingDetails.aspx"
    Private Const PageID As String = "P43"
    Dim objVanReqDetails As New SalesWorx.BO.Common.VanRequisitionDetails
    Dim objVanReq As New SalesWorx.BO.Common.VanRequisition
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Session.Item("USER_ACCESS") Is Nothing Then
                Dim ReqID As String = ""
                If Not IsNothing(Request.QueryString("id")) Then
                    ReqID = Request.QueryString("id").ToString()
                    LoadDetails(ReqID)
                End If
            End If
        End If
    End Sub
    Sub LoadDetails(ByVal ReqID As String)
        hfReqID.Value = ReqID
        objVanReq.StockRequisition_ID = ReqID
        objVanReq.Load()
        lblSalesRep.Text = objVanReq.SalesRepName
        lblEmpName.Text = objVanReq.Emp_Name
        lblComments.Text = objVanReq.Comments
        'lblReID.Text = ReqID
        lblReqDate.Text = Format(objVanReq.Request_Date, "dd-MMM-yyyy")
        imgSig.ImageUrl = "ViewImage.aspx?id=" + ReqID
        objVanReqDetails.StockRequisition_ID = ReqID
        gvVanReq.DataSource = objVanReqDetails.GetVanRequisitionDetails(objVanReq.SalesRep_ID)
        gvVanReq.DataBind()
    End Sub
   
    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        Exit Sub
    End Sub
    Sub MessageBoxInfo(ByVal str As String)
        lblMessage1.ForeColor = Drawing.Color.Green
        lblinfo1.Text = "Information"
        lblMessage1.Text = str
        ModalPopupExtender1.Show()
        Exit Sub
    End Sub

End Class