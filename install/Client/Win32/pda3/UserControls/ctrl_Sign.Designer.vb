<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctrl_Sign
    Inherits ViewControl.iView

    'UserControl overrides dispose to clean up the component list.
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
        Me.txt_PrintName = New System.Windows.Forms.TextBox
        Me.Signature = New System.Windows.Forms.PictureBox
        CType(Me.Signature, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txt_PrintName
        '
        Me.txt_PrintName.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.txt_PrintName.Location = New System.Drawing.Point(0, 130)
        Me.txt_PrintName.Name = "txt_PrintName"
        Me.txt_PrintName.Size = New System.Drawing.Size(150, 20)
        Me.txt_PrintName.TabIndex = 0
        '
        'Signature
        '
        Me.Signature.BackColor = System.Drawing.Color.White
        Me.Signature.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Signature.Location = New System.Drawing.Point(0, 0)
        Me.Signature.Name = "Signature"
        Me.Signature.Size = New System.Drawing.Size(150, 130)
        Me.Signature.TabIndex = 1
        Me.Signature.TabStop = False
        '
        'ctrl_Sign
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Signature)
        Me.Controls.Add(Me.txt_PrintName)
        Me.Name = "ctrl_Sign"
        CType(Me.Signature, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txt_PrintName As System.Windows.Forms.TextBox
    Friend WithEvents Signature As System.Windows.Forms.PictureBox

End Class
