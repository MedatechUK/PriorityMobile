<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ListSort
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
        Me.View = New System.Windows.Forms.ListView
        Me.SuspendLayout()
        '
        'View
        '
        Me.View.Dock = System.Windows.Forms.DockStyle.Fill
        Me.View.FullRowSelect = True
        Me.View.Location = New System.Drawing.Point(0, 0)
        Me.View.Name = "View"
        Me.View.Size = New System.Drawing.Size(150, 150)
        Me.View.TabIndex = 0
        Me.View.View = System.Windows.Forms.View.Details
        '
        'ListSort
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.View)
        Me.Name = "ListSort"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents View As System.Windows.Forms.ListView

End Class
