<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmXpather
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
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.LoadXMLToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LoadFromClipboardToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LoadFromURLToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LoadFromFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TypeInToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TestToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RemoveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Label3 = New System.Windows.Forms.Label
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.ListIndex = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TabName = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.XPATH = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Results = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.MenuStrip1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LoadXMLToolStripMenuItem, Me.TestToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1068, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'LoadXMLToolStripMenuItem
        '
        Me.LoadXMLToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LoadFromClipboardToolStripMenuItem, Me.LoadFromURLToolStripMenuItem, Me.LoadFromFileToolStripMenuItem, Me.TypeInToolStripMenuItem})
        Me.LoadXMLToolStripMenuItem.Name = "LoadXMLToolStripMenuItem"
        Me.LoadXMLToolStripMenuItem.Size = New System.Drawing.Size(72, 20)
        Me.LoadXMLToolStripMenuItem.Text = "Load XML"
        '
        'LoadFromClipboardToolStripMenuItem
        '
        Me.LoadFromClipboardToolStripMenuItem.Name = "LoadFromClipboardToolStripMenuItem"
        Me.LoadFromClipboardToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.LoadFromClipboardToolStripMenuItem.Text = "Load from Clipboard"
        '
        'LoadFromURLToolStripMenuItem
        '
        Me.LoadFromURLToolStripMenuItem.Name = "LoadFromURLToolStripMenuItem"
        Me.LoadFromURLToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.LoadFromURLToolStripMenuItem.Text = "Load from URL"
        '
        'LoadFromFileToolStripMenuItem
        '
        Me.LoadFromFileToolStripMenuItem.Name = "LoadFromFileToolStripMenuItem"
        Me.LoadFromFileToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.LoadFromFileToolStripMenuItem.Text = "Load From File"
        '
        'TypeInToolStripMenuItem
        '
        Me.TypeInToolStripMenuItem.Name = "TypeInToolStripMenuItem"
        Me.TypeInToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.TypeInToolStripMenuItem.Text = "Type In"
        '
        'TestToolStripMenuItem
        '
        Me.TestToolStripMenuItem.Name = "TestToolStripMenuItem"
        Me.TestToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.TestToolStripMenuItem.Text = "Test!"
        '
        'TabControl1
        '
        Me.TabControl1.Location = New System.Drawing.Point(12, 265)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1044, 351)
        Me.TabControl1.TabIndex = 1
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(109, 27)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(940, 20)
        Me.TextBox2.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(16, 30)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(90, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "XPath Expression"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RemoveToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 26)
        '
        'RemoveToolStripMenuItem
        '
        Me.RemoveToolStripMenuItem.Name = "RemoveToolStripMenuItem"
        Me.RemoveToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.RemoveToolStripMenuItem.Text = "Remove"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(16, 111)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(39, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "History"
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ListIndex, Me.TabName, Me.XPATH, Me.Results})
        Me.DataGridView1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.DataGridView1.Location = New System.Drawing.Point(61, 53)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(988, 191)
        Me.DataGridView1.TabIndex = 9
        '
        'ListIndex
        '
        Me.ListIndex.HeaderText = "ID"
        Me.ListIndex.Name = "ListIndex"
        Me.ListIndex.Visible = False
        '
        'TabName
        '
        Me.TabName.HeaderText = "Tab Name"
        Me.TabName.Name = "TabName"
        Me.TabName.Visible = False
        '
        'XPATH
        '
        Me.XPATH.HeaderText = "XPath Text"
        Me.XPATH.Name = "XPATH"
        '
        'Results
        '
        Me.Results.HeaderText = "Results"
        Me.Results.Name = "Results"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.Filter = "XML Files|*.xml"
        '
        'frmXpather
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1068, 628)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmXpather"
        Me.Text = "frmXpather"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents LoadXMLToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TestToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents LoadFromClipboardToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LoadFromURLToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LoadFromFileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TypeInToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents RemoveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents ListIndex As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TabName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents XPATH As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Results As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
End Class
