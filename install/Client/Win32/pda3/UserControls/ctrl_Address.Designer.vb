<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctrl_Address
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
        Me.Gtext = New ViewControl.gtext
        Me.SuspendLayout()
        '
        'Gtext
        '
        Me.Gtext.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Gtext.Location = New System.Drawing.Point(0, 0)
        Me.Gtext.Name = "Gtext"
        Me.Gtext.Size = New System.Drawing.Size(150, 150)
        Me.Gtext.TabIndex = 0
        '
        'ctrl_Address
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Gtext)
        Me.Name = "ctrl_Address"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Gtext As ViewControl.gtext

End Class
