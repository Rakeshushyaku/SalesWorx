Imports System.Configuration.ConfigurationManager
Imports SalesWorx.BO.Common
Imports System.Resources
Imports System.Web.UI.WebControls
Imports System.Data
Imports log4net
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports Telerik.Web.UI
Public Class Rep_DelivNotesOfConsoliDateOrder
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim ObjCommon As SalesWorx.BO.Common.Common
    'Dim SortField As String = ""
    'Dim SortFieldDtl As String = ""

    Private RowIdx As Integer = 0
    Dim objLogin As New SalesWorx.BO.Common.Login
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            HRowID.Value = Request.QueryString("ID")
            HOrgID.Value = Request.QueryString("OrgID")
            HType.Value = Request.QueryString("Type")
            LoadDeliveryNote()

        End If
    End Sub
    Sub LoadDeliveryNote()
        Try
            Dim ObjReturn As New SalesWorx.BO.Common.Returns
            Dim ds As New DataSet
            ds = ObjReturn.GetDeliveryNote(Err_No, Err_Desc, HRowID.Value, HOrgID.Value, HType.Value)
            If ds.Tables.Count > 0 Then
                lbl_refno.Text = ds.Tables(0).Rows(0)("Orig_Sys_Document_Ref").ToString
                lbl_Customer.Text = ds.Tables(0).Rows(0)("Customer").ToString
                lbl_Salesep.Text = ds.Tables(0).Rows(0)("Salesrep_name").ToString
                lbl_Date.Text = CDate(ds.Tables(0).Rows(0)("Creation_Date").ToString).ToString("dd-MMM-yyyy hh:mm tt")
                lbl_amount.Text = Format(Val(ds.Tables(0).Rows(0)("Order_Amt").ToString), "#,##0.00")

            End If
            If ds.Tables.Count > 1 Then
                gvItems.DataSource = ds.Tables(1)
                gvItems.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub gvItems_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles gvItems.PageIndexChanged
        LoadDeliveryNote()
    End Sub
End Class