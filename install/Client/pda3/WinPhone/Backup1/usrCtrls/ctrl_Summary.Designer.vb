<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ctrl_Summary
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
        Me.DateTimePicker = New System.Windows.Forms.DateTimePicker
        Me.ListSort1 = New PriorityMobile.ListSort
        Me.SuspendLayout()
        '
        'DateTimePicker
        '
        Me.DateTimePicker.Dock = System.Windows.Forms.DockStyle.Top
        Me.DateTimePicker.Location = New System.Drawing.Point(0, 0)
        Me.DateTimePicker.Name = "DateTimePicker"
        Me.DateTimePicker.Size = New System.Drawing.Size(150, 22)
        Me.DateTimePicker.TabIndex = 0
        '
        'ListSort1
        '
        Me.ListSort1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListSort1.Location = New System.Drawing.Point(0, 22)
        Me.ListSort1.Name = "ListSort1"
        Me.ListSort1.Size = New System.Drawing.Size(150, 128)
        Me.ListSort1.TabIndex = 1
        '
        'ctrl_Summary
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.ListSort1)
        Me.Controls.Add(Me.DateTimePicker)
        Me.Name = "ctrl_Summary"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DateTimePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents ListSort1 As PriorityMobile.ListSort

End Class
