<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class table
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Dim ListViewItem1 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"", ""}, -1)
        Me.tbl = New System.Windows.Forms.ListView
        Me.SuspendLayout()
        '
        'tbl
        '
        Me.tbl.Alignment = System.Windows.Forms.ListViewAlignment.SnapToGrid
        Me.tbl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbl.FullRowSelect = True
        Me.tbl.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem1})
        Me.tbl.Location = New System.Drawing.Point(0, 0)
        Me.tbl.Name = "tbl"
        Me.tbl.Size = New System.Drawing.Size(292, 273)
        Me.tbl.TabIndex = 1
        Me.tbl.UseCompatibleStateImageBehavior = False
        Me.tbl.View = System.Windows.Forms.View.Details
        '
        'table
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 273)
        Me.Controls.Add(Me.tbl)
        Me.Name = "table"
        Me.Text = "table"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tbl As System.Windows.Forms.ListView
End Class
