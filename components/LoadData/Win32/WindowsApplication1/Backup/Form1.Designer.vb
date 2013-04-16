<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.Table = New System.Windows.Forms.ListView
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.PopulateTableToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PopulateTableToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.MakeDatagramToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Table
        '
        Me.Table.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Table.Font = New System.Drawing.Font("Arial", 11.0!)
        Me.Table.FullRowSelect = True
        Me.Table.Location = New System.Drawing.Point(0, 24)
        Me.Table.Name = "Table"
        Me.Table.Size = New System.Drawing.Size(292, 249)
        Me.Table.TabIndex = 3
        Me.Table.UseCompatibleStateImageBehavior = False
        Me.Table.View = System.Windows.Forms.View.Details
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PopulateTableToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(292, 24)
        Me.MenuStrip1.TabIndex = 4
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'PopulateTableToolStripMenuItem
        '
        Me.PopulateTableToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PopulateTableToolStripMenuItem1, Me.MakeDatagramToolStripMenuItem})
        Me.PopulateTableToolStripMenuItem.Name = "PopulateTableToolStripMenuItem"
        Me.PopulateTableToolStripMenuItem.Size = New System.Drawing.Size(45, 20)
        Me.PopulateTableToolStripMenuItem.Text = "Menu"
        '
        'PopulateTableToolStripMenuItem1
        '
        Me.PopulateTableToolStripMenuItem1.Name = "PopulateTableToolStripMenuItem1"
        Me.PopulateTableToolStripMenuItem1.Size = New System.Drawing.Size(160, 22)
        Me.PopulateTableToolStripMenuItem1.Text = "Populate Table"
        '
        'MakeDatagramToolStripMenuItem
        '
        Me.MakeDatagramToolStripMenuItem.Name = "MakeDatagramToolStripMenuItem"
        Me.MakeDatagramToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
        Me.MakeDatagramToolStripMenuItem.Text = "Make Datagram"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 273)
        Me.Controls.Add(Me.Table)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Table As System.Windows.Forms.ListView
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents PopulateTableToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PopulateTableToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MakeDatagramToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
