<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ctrl_Repair
    Inherits PriorityMobile.iView

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
        Me.Malfunction = New System.Windows.Forms.ComboBox
        Me.Resolution = New System.Windows.Forms.ComboBox
        Me.Repair = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'Malfunction
        '
        Me.Malfunction.Dock = System.Windows.Forms.DockStyle.Top
        Me.Malfunction.Location = New System.Drawing.Point(0, 0)
        Me.Malfunction.Name = "Malfunction"
        Me.Malfunction.Size = New System.Drawing.Size(150, 22)
        Me.Malfunction.TabIndex = 0
        '
        'Resolution
        '
        Me.Resolution.Dock = System.Windows.Forms.DockStyle.Top
        Me.Resolution.Location = New System.Drawing.Point(0, 22)
        Me.Resolution.Name = "Resolution"
        Me.Resolution.Size = New System.Drawing.Size(150, 22)
        Me.Resolution.TabIndex = 1
        '
        'Repair
        '
        Me.Repair.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Repair.Location = New System.Drawing.Point(0, 44)
        Me.Repair.Multiline = True
        Me.Repair.Name = "Repair"
        Me.Repair.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.Repair.Size = New System.Drawing.Size(150, 106)
        Me.Repair.TabIndex = 2
        '
        'ctrl_Repair
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.Repair)
        Me.Controls.Add(Me.Resolution)
        Me.Controls.Add(Me.Malfunction)
        Me.Name = "ctrl_Repair"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Malfunction As System.Windows.Forms.ComboBox
    Friend WithEvents Resolution As System.Windows.Forms.ComboBox
    Friend WithEvents Repair As System.Windows.Forms.TextBox

End Class
