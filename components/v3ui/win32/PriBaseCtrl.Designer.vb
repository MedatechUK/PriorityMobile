<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PriBaseCtrl
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
        Me.Label = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Label
        '
        Me.Label.AutoEllipsis = True
        Me.Label.AutoSize = True
        Me.Label.ForeColor = System.Drawing.Color.Black
        Me.Label.Location = New System.Drawing.Point(3, 0)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(39, 13)
        Me.Label.TabIndex = 2
        Me.Label.Text = "Label1"
        Me.Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'PriBaseCtrl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Label)
        Me.Name = "PriBaseCtrl"
        Me.Size = New System.Drawing.Size(327, 17)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label As System.Windows.Forms.Label

End Class
