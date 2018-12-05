Public Class ShowBeaconDetails
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Request.QueryString("ID") Is Nothing Then
            Dim objreports As New SalesWorx.BO.Common.Reports
            Maindiv.Visible = True
            Dim dt As New DataTable
            dt = objreports.GetBeaconDetails(Err_No, Err_Desc, Request.QueryString("ID"))
            If dt.Rows.Count > 0 Then
                noinfo.Visible = False
                info.Visible = True
                lbl_Customer.Text = dt.Rows(0)("Customer").ToString
                lbl_date.Text = CDate(dt.Rows(0)("Visit_Start_Date").ToString()).ToString("dd-MMM-yyyy hh:mm:ss tt")
                lbl_CustBID.Text = "Beacon ID: " & dt.Rows(0)("CustBID").ToString
                lbl_CustMajor.Text = "Major: " & dt.Rows(0)("Custmajor").ToString
                lbl_CustMinor.Text = "Minor: " & dt.Rows(0)("CustMinor").ToString

                lbl_VBID.Text = "Beacon ID: " & dt.Rows(0)("VBID").ToString
                lbl_VMajor.Text = "Major: " & dt.Rows(0)("VMajor").ToString
                lbl_VMinor.Text = "Minor: " & dt.Rows(0)("VMinor").ToString

            Else
                noinfo.Visible = True
                info.Visible = False
            End If
        Else
            Maindiv.Visible = False
        End If
    End Sub

End Class