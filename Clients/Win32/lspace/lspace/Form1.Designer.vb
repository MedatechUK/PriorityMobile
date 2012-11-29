<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.RotateMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RotateLeftToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RotateRightToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.lstShapes = New System.Windows.Forms.ListBox
        Me.IMG = New System.Windows.Forms.PictureBox
        Me.RotateMenu.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.IMG, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RotateMenu
        '
        Me.RotateMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RotateLeftToolStripMenuItem, Me.RotateRightToolStripMenuItem})
        Me.RotateMenu.Name = "RotateMenu"
        Me.RotateMenu.Size = New System.Drawing.Size(140, 48)
        '
        'RotateLeftToolStripMenuItem
        '
        Me.RotateLeftToolStripMenuItem.Name = "RotateLeftToolStripMenuItem"
        Me.RotateLeftToolStripMenuItem.Size = New System.Drawing.Size(139, 22)
        Me.RotateLeftToolStripMenuItem.Text = "Rotate Left"
        '
        'RotateRightToolStripMenuItem
        '
        Me.RotateRightToolStripMenuItem.Name = "RotateRightToolStripMenuItem"
        Me.RotateRightToolStripMenuItem.Size = New System.Drawing.Size(139, 22)
        Me.RotateRightToolStripMenuItem.Text = "Rotate Right"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.lstShapes)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.IMG)
        Me.SplitContainer1.Size = New System.Drawing.Size(536, 391)
        Me.SplitContainer1.SplitterDistance = 178
        Me.SplitContainer1.TabIndex = 1
        '
        'lstShapes
        '
        Me.lstShapes.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstShapes.FormattingEnabled = True
        Me.lstShapes.Location = New System.Drawing.Point(0, 0)
        Me.lstShapes.Name = "lstShapes"
        Me.lstShapes.Size = New System.Drawing.Size(178, 381)
        Me.lstShapes.TabIndex = 0
        '
        'IMG
        '
        Me.IMG.Dock = System.Windows.Forms.DockStyle.Fill
        Me.IMG.Location = New System.Drawing.Point(0, 0)
        Me.IMG.Name = "IMG"
        Me.IMG.Size = New System.Drawing.Size(354, 391)
        Me.IMG.TabIndex = 1
        Me.IMG.TabStop = False
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(536, 391)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "L-Space"
        Me.RotateMenu.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.IMG, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents RotateMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents RotateLeftToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RotateRightToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents lstShapes As System.Windows.Forms.ListBox
    Friend WithEvents IMG As System.Windows.Forms.PictureBox

End Class
