<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class SelectItems
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
        Me.Items = New System.Windows.Forms.ListView
        Me.ListView2 = New System.Windows.Forms.ListView
        Me.SuspendLayout()
        '
        'Items
        '
        Me.Items.Dock = System.Windows.Forms.DockStyle.Top
        Me.Items.Location = New System.Drawing.Point(0, 0)
        Me.Items.Name = "Items"
        Me.Items.Size = New System.Drawing.Size(228, 107)
        Me.Items.TabIndex = 0
        Me.Items.View = System.Windows.Forms.View.List
        '
        'ListView2
        '
        Me.ListView2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ListView2.Location = New System.Drawing.Point(0, 113)
        Me.ListView2.Name = "ListView2"
        Me.ListView2.Size = New System.Drawing.Size(228, 105)
        Me.ListView2.TabIndex = 1
        Me.ListView2.View = System.Windows.Forms.View.List
        '
        'SelectItems
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.ListView2)
        Me.Controls.Add(Me.Items)
        Me.Name = "SelectItems"
        Me.Size = New System.Drawing.Size(228, 218)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Items As System.Windows.Forms.ListView
    Friend WithEvents ListView2 As System.Windows.Forms.ListView

End Class
