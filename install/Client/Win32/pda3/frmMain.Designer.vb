<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Me.ToolStripContainer1 = New System.Windows.Forms.ToolStripContainer
        Me.mnu_SubForms = New System.Windows.Forms.MenuStrip
        Me.OptionToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnu_TopForms = New System.Windows.Forms.MenuStrip
        Me.OptionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.ToolStripContainer1.BottomToolStripPanel.SuspendLayout()
        Me.ToolStripContainer1.LeftToolStripPanel.SuspendLayout()
        Me.ToolStripContainer1.TopToolStripPanel.SuspendLayout()
        Me.ToolStripContainer1.SuspendLayout()
        Me.mnu_SubForms.SuspendLayout()
        Me.mnu_TopForms.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStripContainer1
        '
        '
        'ToolStripContainer1.BottomToolStripPanel
        '
        Me.ToolStripContainer1.BottomToolStripPanel.Controls.Add(Me.mnu_SubForms)
        '
        'ToolStripContainer1.ContentPanel
        '
        Me.ToolStripContainer1.ContentPanel.BackColor = System.Drawing.Color.White
        Me.ToolStripContainer1.ContentPanel.Size = New System.Drawing.Size(468, 337)
        Me.ToolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        '
        'ToolStripContainer1.LeftToolStripPanel
        '
        Me.ToolStripContainer1.LeftToolStripPanel.Controls.Add(Me.mnu_TopForms)
        Me.ToolStripContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripContainer1.Name = "ToolStripContainer1"
        Me.ToolStripContainer1.Size = New System.Drawing.Size(566, 386)
        Me.ToolStripContainer1.TabIndex = 2
        Me.ToolStripContainer1.Text = "ToolStripContainer1"
        '
        'ToolStripContainer1.TopToolStripPanel
        '
        Me.ToolStripContainer1.TopToolStripPanel.Controls.Add(Me.ToolStrip1)
        '
        'mnu_SubForms
        '
        Me.mnu_SubForms.Dock = System.Windows.Forms.DockStyle.None
        Me.mnu_SubForms.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OptionToolStripMenuItem1})
        Me.mnu_SubForms.Location = New System.Drawing.Point(0, 0)
        Me.mnu_SubForms.Name = "mnu_SubForms"
        Me.mnu_SubForms.Size = New System.Drawing.Size(566, 24)
        Me.mnu_SubForms.TabIndex = 0
        Me.mnu_SubForms.Text = "MenuStrip3"
        '
        'OptionToolStripMenuItem1
        '
        Me.OptionToolStripMenuItem1.Name = "OptionToolStripMenuItem1"
        Me.OptionToolStripMenuItem1.Size = New System.Drawing.Size(56, 20)
        Me.OptionToolStripMenuItem1.Text = "Option"
        '
        'mnu_TopForms
        '
        Me.mnu_TopForms.Dock = System.Windows.Forms.DockStyle.None
        Me.mnu_TopForms.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OptionToolStripMenuItem})
        Me.mnu_TopForms.Location = New System.Drawing.Point(0, 0)
        Me.mnu_TopForms.Name = "mnu_TopForms"
        Me.mnu_TopForms.Size = New System.Drawing.Size(98, 337)
        Me.mnu_TopForms.TabIndex = 0
        Me.mnu_TopForms.Text = "MenuStrip4"
        Me.mnu_TopForms.TextDirection = System.Windows.Forms.ToolStripTextDirection.Vertical270
        '
        'OptionToolStripMenuItem
        '
        Me.OptionToolStripMenuItem.Name = "OptionToolStripMenuItem"
        Me.OptionToolStripMenuItem.Size = New System.Drawing.Size(26, 48)
        Me.OptionToolStripMenuItem.Text = "Option"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Dock = System.Windows.Forms.DockStyle.None
        Me.ToolStrip1.Location = New System.Drawing.Point(3, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(111, 25)
        Me.ToolStrip1.TabIndex = 1
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(566, 386)
        Me.Controls.Add(Me.ToolStripContainer1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMain"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmMain"
        Me.ToolStripContainer1.BottomToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer1.BottomToolStripPanel.PerformLayout()
        Me.ToolStripContainer1.LeftToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer1.LeftToolStripPanel.PerformLayout()
        Me.ToolStripContainer1.TopToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer1.TopToolStripPanel.PerformLayout()
        Me.ToolStripContainer1.ResumeLayout(False)
        Me.ToolStripContainer1.PerformLayout()
        Me.mnu_SubForms.ResumeLayout(False)
        Me.mnu_SubForms.PerformLayout()
        Me.mnu_TopForms.ResumeLayout(False)
        Me.mnu_TopForms.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolStripContainer1 As System.Windows.Forms.ToolStripContainer
    Friend WithEvents mnu_SubForms As System.Windows.Forms.MenuStrip
    Friend WithEvents OptionToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents mnu_TopForms As System.Windows.Forms.MenuStrip
    Friend WithEvents OptionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
