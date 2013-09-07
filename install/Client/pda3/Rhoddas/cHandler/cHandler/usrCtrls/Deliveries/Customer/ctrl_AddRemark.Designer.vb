<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ctrl_AddRemark
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
        Me.Remark = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'Remark
        '
        Me.Remark.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Remark.Location = New System.Drawing.Point(0, 0)
        Me.Remark.Multiline = True
        Me.Remark.Name = "Remark"
        Me.Remark.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.Remark.Size = New System.Drawing.Size(150, 150)
        Me.Remark.TabIndex = 3
        '
        'ctrl_AddRemark
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.Remark)
        Me.Name = "ctrl_AddRemark"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Remark As System.Windows.Forms.TextBox

End Class
