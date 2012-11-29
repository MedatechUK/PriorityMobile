<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class priSign
    Inherits System.Windows.Forms.UserControl

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
        Me.ASTEXT = New System.Windows.Forms.TextBox
        Me.Sign = New System.Windows.Forms.PictureBox
        'CType(Me.Sign, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ASTEXT
        '
        Me.ASTEXT.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ASTEXT.Location = New System.Drawing.Point(0, 130)
        Me.ASTEXT.Name = "ASTEXT"
        Me.ASTEXT.Size = New System.Drawing.Size(150, 20)
        Me.ASTEXT.TabIndex = 0
        '
        'Sign
        '
        Me.Sign.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Sign.Location = New System.Drawing.Point(0, 0)
        Me.Sign.Name = "Sign"
        Me.Sign.Size = New System.Drawing.Size(150, 130)
        Me.Sign.TabIndex = 1
        Me.Sign.TabStop = False
        '
        'priSign
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        'Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Sign)
        Me.Controls.Add(Me.ASTEXT)
        Me.Name = "priSign"
        'CType(Me.Sign, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        'Me.PerformLayout()

    End Sub
    Friend WithEvents ASTEXT As System.Windows.Forms.TextBox
    Friend WithEvents Sign As System.Windows.Forms.PictureBox

End Class
