<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class Sign
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Signature = New System.Windows.Forms.PictureBox
        Me.SuspendLayout()
        '
        'Signature
        '
        Me.Signature.BackColor = System.Drawing.Color.White
        Me.Signature.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Signature.Location = New System.Drawing.Point(0, 0)
        Me.Signature.Name = "Signature"
        Me.Signature.Size = New System.Drawing.Size(150, 150)
        '
        'Sign
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.Signature)
        Me.Name = "Sign"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Signature As System.Windows.Forms.PictureBox

End Class
