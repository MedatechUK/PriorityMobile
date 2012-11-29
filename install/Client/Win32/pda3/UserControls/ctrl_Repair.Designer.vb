<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctrl_Repair
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
        Me.malfunction = New System.Windows.Forms.ComboBox
        Me.resolution = New System.Windows.Forms.ComboBox
        Me.repair = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'malfunction
        '
        Me.malfunction.Dock = System.Windows.Forms.DockStyle.Top
        Me.malfunction.FormattingEnabled = True
        Me.malfunction.Location = New System.Drawing.Point(0, 0)
        Me.malfunction.Name = "malfunction"
        Me.malfunction.Size = New System.Drawing.Size(150, 21)
        Me.malfunction.TabIndex = 0
        '
        'resolution
        '
        Me.resolution.Dock = System.Windows.Forms.DockStyle.Top
        Me.resolution.FormattingEnabled = True
        Me.resolution.Location = New System.Drawing.Point(0, 21)
        Me.resolution.Name = "resolution"
        Me.resolution.Size = New System.Drawing.Size(150, 21)
        Me.resolution.TabIndex = 1
        '
        'repair
        '
        Me.repair.Dock = System.Windows.Forms.DockStyle.Fill
        Me.repair.Location = New System.Drawing.Point(0, 42)
        Me.repair.Multiline = True
        Me.repair.Name = "repair"
        Me.repair.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.repair.Size = New System.Drawing.Size(150, 108)
        Me.repair.TabIndex = 2
        '
        'ctrl_Repair
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.repair)
        Me.Controls.Add(Me.resolution)
        Me.Controls.Add(Me.malfunction)
        Me.Name = "ctrl_Repair"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents malfunction As System.Windows.Forms.ComboBox
    Friend WithEvents resolution As System.Windows.Forms.ComboBox
    Friend WithEvents repair As System.Windows.Forms.TextBox

End Class
