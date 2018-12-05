<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.BtnClose = New System.Windows.Forms.Button
        Me.BtnDecrypt = New System.Windows.Forms.Button
        Me.TxtOutput = New System.Windows.Forms.TextBox
        Me.BtnEncrypt = New System.Windows.Forms.Button
        Me.TxtInput = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'BtnClose
        '
        Me.BtnClose.Location = New System.Drawing.Point(172, 230)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(75, 23)
        Me.BtnClose.TabIndex = 11
        Me.BtnClose.Text = "Close"
        Me.BtnClose.UseVisualStyleBackColor = True
        '
        'BtnDecrypt
        '
        Me.BtnDecrypt.Location = New System.Drawing.Point(248, 164)
        Me.BtnDecrypt.Name = "BtnDecrypt"
        Me.BtnDecrypt.Size = New System.Drawing.Size(75, 23)
        Me.BtnDecrypt.TabIndex = 10
        Me.BtnDecrypt.Text = "Decrypt"
        Me.BtnDecrypt.UseVisualStyleBackColor = True
        '
        'TxtOutput
        '
        Me.TxtOutput.Location = New System.Drawing.Point(9, 86)
        Me.TxtOutput.Multiline = True
        Me.TxtOutput.Name = "TxtOutput"
        Me.TxtOutput.ReadOnly = True
        Me.TxtOutput.Size = New System.Drawing.Size(434, 49)
        Me.TxtOutput.TabIndex = 9
        '
        'BtnEncrypt
        '
        Me.BtnEncrypt.Location = New System.Drawing.Point(96, 164)
        Me.BtnEncrypt.Name = "BtnEncrypt"
        Me.BtnEncrypt.Size = New System.Drawing.Size(75, 23)
        Me.BtnEncrypt.TabIndex = 8
        Me.BtnEncrypt.Text = "Encrypt"
        Me.BtnEncrypt.UseVisualStyleBackColor = True
        '
        'TxtInput
        '
        Me.TxtInput.Location = New System.Drawing.Point(9, 19)
        Me.TxtInput.Multiline = True
        Me.TxtInput.Name = "TxtInput"
        Me.TxtInput.Size = New System.Drawing.Size(434, 49)
        Me.TxtInput.TabIndex = 7
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(451, 272)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.BtnDecrypt)
        Me.Controls.Add(Me.TxtOutput)
        Me.Controls.Add(Me.BtnEncrypt)
        Me.Controls.Add(Me.TxtInput)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnClose As System.Windows.Forms.Button
    Friend WithEvents BtnDecrypt As System.Windows.Forms.Button
    Friend WithEvents TxtOutput As System.Windows.Forms.TextBox
    Friend WithEvents BtnEncrypt As System.Windows.Forms.Button
    Friend WithEvents TxtInput As System.Windows.Forms.TextBox

End Class
