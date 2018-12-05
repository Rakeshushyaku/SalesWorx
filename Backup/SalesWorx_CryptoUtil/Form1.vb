Imports SalesWorx.BO.Common
Public Class Form1
    Private Sub BtnEncrypt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnEncrypt.Click
        Me.TxtOutput.Text = New Crypto().EncryptReportName(Me.TxtInput.Text)
    End Sub

    Private Sub BtnDecrypt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDecrypt.Click
        Me.TxtOutput.Text = New Crypto().DecryptReportName(Me.TxtInput.Text)
    End Sub

    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub
End Class
