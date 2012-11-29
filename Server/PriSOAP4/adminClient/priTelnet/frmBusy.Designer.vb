<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBusy
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
        Me.LblWaitfor = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'LblWaitfor
        '
        Me.LblWaitfor.AutoSize = True
        Me.LblWaitfor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LblWaitfor.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblWaitfor.Location = New System.Drawing.Point(0, 0)
        Me.LblWaitfor.MinimumSize = New System.Drawing.Size(225, 71)
        Me.LblWaitfor.Name = "LblWaitfor"
        Me.LblWaitfor.Size = New System.Drawing.Size(225, 71)
        Me.LblWaitfor.TabIndex = 0
        Me.LblWaitfor.Text = "Waiting for ..."
        Me.LblWaitfor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'fmBusy
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(225, 71)
        Me.Controls.Add(Me.LblWaitfor)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "fmBusy"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "fmBusy"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LblWaitfor As System.Windows.Forms.Label

End Class
