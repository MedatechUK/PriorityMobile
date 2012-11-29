<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MDIChild
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MDIChild))
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.tab = New System.Windows.Forms.TabControl
        Me.tabVariables = New System.Windows.Forms.TabPage
        Me.lstVariables = New System.Windows.Forms.ListView
        Me.cMenuVars = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.TableDictionaryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem10 = New System.Windows.Forms.ToolStripSeparator
        Me.ToggleHiddenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.HideSelectionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ShowSelectionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem13 = New System.Windows.Forms.ToolStripSeparator
        Me.SelectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NoneToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.InvertToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem12 = New System.Windows.Forms.ToolStripSeparator
        Me.VisibleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.HiddenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TableToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DataTypeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.INTToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.REALToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CHARToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DATEToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tabWhere = New System.Windows.Forms.TabPage
        Me.txtWhere = New System.Windows.Forms.TextBox
        Me.cMenuWhere = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.TablesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DataTypeToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem14 = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem15 = New System.Windows.Forms.ToolStripSeparator
        Me.JoinToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem11 = New System.Windows.Forms.ToolStripSeparator
        Me.UndoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CaseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.UpperToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LowerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator
        Me.CutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PasteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SelectAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tabSource = New System.Windows.Forms.TabPage
        Me.data = New System.Windows.Forms.TextBox
        Me.cMenuSource = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem5 = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.ToolStripMenuItem6 = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem7 = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem8 = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem9 = New System.Windows.Forms.ToolStripMenuItem
        Me.tabLoadRef = New System.Windows.Forms.TabPage
        Me.lblNoref = New System.Windows.Forms.Label
        Me.LoadRef = New System.Windows.Forms.ListView
        Me.cMenuLoadRef = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.TToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.output = New System.Windows.Forms.TextBox
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.tab.SuspendLayout()
        Me.tabVariables.SuspendLayout()
        Me.cMenuVars.SuspendLayout()
        Me.tabWhere.SuspendLayout()
        Me.cMenuWhere.SuspendLayout()
        Me.tabSource.SuspendLayout()
        Me.cMenuSource.SuspendLayout()
        Me.tabLoadRef.SuspendLayout()
        Me.cMenuLoadRef.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.tab)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.output)
        Me.SplitContainer1.Size = New System.Drawing.Size(478, 383)
        Me.SplitContainer1.SplitterDistance = 239
        Me.SplitContainer1.TabIndex = 2
        '
        'tab
        '
        Me.tab.Alignment = System.Windows.Forms.TabAlignment.Bottom
        Me.tab.Controls.Add(Me.tabVariables)
        Me.tab.Controls.Add(Me.tabWhere)
        Me.tab.Controls.Add(Me.tabSource)
        Me.tab.Controls.Add(Me.tabLoadRef)
        Me.tab.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tab.Location = New System.Drawing.Point(0, 0)
        Me.tab.Name = "tab"
        Me.tab.SelectedIndex = 0
        Me.tab.Size = New System.Drawing.Size(478, 239)
        Me.tab.TabIndex = 2
        '
        'tabVariables
        '
        Me.tabVariables.Controls.Add(Me.lstVariables)
        Me.tabVariables.Location = New System.Drawing.Point(4, 4)
        Me.tabVariables.Name = "tabVariables"
        Me.tabVariables.Size = New System.Drawing.Size(470, 213)
        Me.tabVariables.TabIndex = 2
        Me.tabVariables.Text = "Variables"
        Me.tabVariables.UseVisualStyleBackColor = True
        '
        'lstVariables
        '
        Me.lstVariables.ContextMenuStrip = Me.cMenuVars
        Me.lstVariables.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstVariables.ForeColor = System.Drawing.Color.Blue
        Me.lstVariables.FullRowSelect = True
        Me.lstVariables.Location = New System.Drawing.Point(0, 0)
        Me.lstVariables.Name = "lstVariables"
        Me.lstVariables.Size = New System.Drawing.Size(470, 213)
        Me.lstVariables.TabIndex = 1
        Me.lstVariables.UseCompatibleStateImageBehavior = False
        Me.lstVariables.View = System.Windows.Forms.View.Details
        '
        'cMenuVars
        '
        Me.cMenuVars.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TableDictionaryToolStripMenuItem, Me.ToolStripMenuItem10, Me.ToggleHiddenToolStripMenuItem, Me.HideSelectionToolStripMenuItem, Me.ShowSelectionToolStripMenuItem, Me.ToolStripMenuItem13, Me.SelectToolStripMenuItem, Me.TableToolStripMenuItem, Me.DataTypeToolStripMenuItem})
        Me.cMenuVars.Name = "cMenuVars"
        Me.cMenuVars.Size = New System.Drawing.Size(164, 170)
        '
        'TableDictionaryToolStripMenuItem
        '
        Me.TableDictionaryToolStripMenuItem.Name = "TableDictionaryToolStripMenuItem"
        Me.TableDictionaryToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
        Me.TableDictionaryToolStripMenuItem.Text = "Table Dictionary"
        '
        'ToolStripMenuItem10
        '
        Me.ToolStripMenuItem10.Name = "ToolStripMenuItem10"
        Me.ToolStripMenuItem10.Size = New System.Drawing.Size(160, 6)
        '
        'ToggleHiddenToolStripMenuItem
        '
        Me.ToggleHiddenToolStripMenuItem.Name = "ToggleHiddenToolStripMenuItem"
        Me.ToggleHiddenToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
        Me.ToggleHiddenToolStripMenuItem.Text = "Toggle Selection"
        '
        'HideSelectionToolStripMenuItem
        '
        Me.HideSelectionToolStripMenuItem.Name = "HideSelectionToolStripMenuItem"
        Me.HideSelectionToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
        Me.HideSelectionToolStripMenuItem.Text = "Hide Selection"
        '
        'ShowSelectionToolStripMenuItem
        '
        Me.ShowSelectionToolStripMenuItem.Name = "ShowSelectionToolStripMenuItem"
        Me.ShowSelectionToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
        Me.ShowSelectionToolStripMenuItem.Text = "Show Selection"
        '
        'ToolStripMenuItem13
        '
        Me.ToolStripMenuItem13.Name = "ToolStripMenuItem13"
        Me.ToolStripMenuItem13.Size = New System.Drawing.Size(160, 6)
        '
        'SelectToolStripMenuItem
        '
        Me.SelectToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AllToolStripMenuItem, Me.NoneToolStripMenuItem, Me.InvertToolStripMenuItem, Me.ToolStripMenuItem12, Me.VisibleToolStripMenuItem, Me.HiddenToolStripMenuItem})
        Me.SelectToolStripMenuItem.Name = "SelectToolStripMenuItem"
        Me.SelectToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
        Me.SelectToolStripMenuItem.Text = "Select"
        '
        'AllToolStripMenuItem
        '
        Me.AllToolStripMenuItem.Name = "AllToolStripMenuItem"
        Me.AllToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.AllToolStripMenuItem.Text = "All"
        '
        'NoneToolStripMenuItem
        '
        Me.NoneToolStripMenuItem.Name = "NoneToolStripMenuItem"
        Me.NoneToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.NoneToolStripMenuItem.Text = "None"
        '
        'InvertToolStripMenuItem
        '
        Me.InvertToolStripMenuItem.Name = "InvertToolStripMenuItem"
        Me.InvertToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.InvertToolStripMenuItem.Text = "Invert"
        '
        'ToolStripMenuItem12
        '
        Me.ToolStripMenuItem12.Name = "ToolStripMenuItem12"
        Me.ToolStripMenuItem12.Size = New System.Drawing.Size(115, 6)
        '
        'VisibleToolStripMenuItem
        '
        Me.VisibleToolStripMenuItem.Name = "VisibleToolStripMenuItem"
        Me.VisibleToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.VisibleToolStripMenuItem.Text = "Visible"
        '
        'HiddenToolStripMenuItem
        '
        Me.HiddenToolStripMenuItem.Name = "HiddenToolStripMenuItem"
        Me.HiddenToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.HiddenToolStripMenuItem.Text = "Hidden"
        '
        'TableToolStripMenuItem
        '
        Me.TableToolStripMenuItem.Name = "TableToolStripMenuItem"
        Me.TableToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
        Me.TableToolStripMenuItem.Text = "Table"
        '
        'DataTypeToolStripMenuItem
        '
        Me.DataTypeToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.INTToolStripMenuItem, Me.REALToolStripMenuItem, Me.CHARToolStripMenuItem, Me.DATEToolStripMenuItem})
        Me.DataTypeToolStripMenuItem.Name = "DataTypeToolStripMenuItem"
        Me.DataTypeToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
        Me.DataTypeToolStripMenuItem.Text = "Data Type"
        '
        'INTToolStripMenuItem
        '
        Me.INTToolStripMenuItem.Name = "INTToolStripMenuItem"
        Me.INTToolStripMenuItem.Size = New System.Drawing.Size(113, 22)
        Me.INTToolStripMenuItem.Text = "INT"
        '
        'REALToolStripMenuItem
        '
        Me.REALToolStripMenuItem.Name = "REALToolStripMenuItem"
        Me.REALToolStripMenuItem.Size = New System.Drawing.Size(113, 22)
        Me.REALToolStripMenuItem.Text = "REAL"
        '
        'CHARToolStripMenuItem
        '
        Me.CHARToolStripMenuItem.Name = "CHARToolStripMenuItem"
        Me.CHARToolStripMenuItem.Size = New System.Drawing.Size(113, 22)
        Me.CHARToolStripMenuItem.Text = "CHAR"
        '
        'DATEToolStripMenuItem
        '
        Me.DATEToolStripMenuItem.Name = "DATEToolStripMenuItem"
        Me.DATEToolStripMenuItem.Size = New System.Drawing.Size(113, 22)
        Me.DATEToolStripMenuItem.Text = "DATE"
        '
        'tabWhere
        '
        Me.tabWhere.Controls.Add(Me.txtWhere)
        Me.tabWhere.Location = New System.Drawing.Point(4, 4)
        Me.tabWhere.Name = "tabWhere"
        Me.tabWhere.Size = New System.Drawing.Size(470, 213)
        Me.tabWhere.TabIndex = 3
        Me.tabWhere.Text = "Where Clause"
        Me.tabWhere.UseVisualStyleBackColor = True
        '
        'txtWhere
        '
        Me.txtWhere.ContextMenuStrip = Me.cMenuWhere
        Me.txtWhere.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtWhere.Font = New System.Drawing.Font("Lucida Console", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWhere.ForeColor = System.Drawing.Color.Blue
        Me.txtWhere.Location = New System.Drawing.Point(0, 0)
        Me.txtWhere.Multiline = True
        Me.txtWhere.Name = "txtWhere"
        Me.txtWhere.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtWhere.Size = New System.Drawing.Size(470, 213)
        Me.txtWhere.TabIndex = 3
        '
        'cMenuWhere
        '
        Me.cMenuWhere.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TablesToolStripMenuItem, Me.DataTypeToolStripMenuItem1, Me.ToolStripMenuItem14, Me.ToolStripMenuItem15, Me.JoinToolStripMenuItem, Me.ToolStripMenuItem11, Me.UndoToolStripMenuItem, Me.CaseToolStripMenuItem, Me.ToolStripMenuItem1, Me.CutToolStripMenuItem, Me.CopyToolStripMenuItem, Me.PasteToolStripMenuItem, Me.SelectAllToolStripMenuItem})
        Me.cMenuWhere.Name = "cMenuWhere"
        Me.cMenuWhere.Size = New System.Drawing.Size(168, 242)
        '
        'TablesToolStripMenuItem
        '
        Me.TablesToolStripMenuItem.Name = "TablesToolStripMenuItem"
        Me.TablesToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
        Me.TablesToolStripMenuItem.Text = "Tables"
        '
        'DataTypeToolStripMenuItem1
        '
        Me.DataTypeToolStripMenuItem1.Name = "DataTypeToolStripMenuItem1"
        Me.DataTypeToolStripMenuItem1.Size = New System.Drawing.Size(167, 22)
        Me.DataTypeToolStripMenuItem1.Text = "Data Type"
        '
        'ToolStripMenuItem14
        '
        Me.ToolStripMenuItem14.Name = "ToolStripMenuItem14"
        Me.ToolStripMenuItem14.Size = New System.Drawing.Size(167, 22)
        Me.ToolStripMenuItem14.Text = "Variables"
        '
        'ToolStripMenuItem15
        '
        Me.ToolStripMenuItem15.Name = "ToolStripMenuItem15"
        Me.ToolStripMenuItem15.Size = New System.Drawing.Size(164, 6)
        '
        'JoinToolStripMenuItem
        '
        Me.JoinToolStripMenuItem.Name = "JoinToolStripMenuItem"
        Me.JoinToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
        Me.JoinToolStripMenuItem.Text = "Join"
        '
        'ToolStripMenuItem11
        '
        Me.ToolStripMenuItem11.Name = "ToolStripMenuItem11"
        Me.ToolStripMenuItem11.Size = New System.Drawing.Size(164, 6)
        '
        'UndoToolStripMenuItem
        '
        Me.UndoToolStripMenuItem.Name = "UndoToolStripMenuItem"
        Me.UndoToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Z), System.Windows.Forms.Keys)
        Me.UndoToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
        Me.UndoToolStripMenuItem.Text = "Undo"
        '
        'CaseToolStripMenuItem
        '
        Me.CaseToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UpperToolStripMenuItem, Me.LowerToolStripMenuItem})
        Me.CaseToolStripMenuItem.Name = "CaseToolStripMenuItem"
        Me.CaseToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
        Me.CaseToolStripMenuItem.Text = "Change Case"
        '
        'UpperToolStripMenuItem
        '
        Me.UpperToolStripMenuItem.Name = "UpperToolStripMenuItem"
        Me.UpperToolStripMenuItem.Size = New System.Drawing.Size(114, 22)
        Me.UpperToolStripMenuItem.Text = "Upper"
        '
        'LowerToolStripMenuItem
        '
        Me.LowerToolStripMenuItem.Name = "LowerToolStripMenuItem"
        Me.LowerToolStripMenuItem.Size = New System.Drawing.Size(114, 22)
        Me.LowerToolStripMenuItem.Text = "Lower"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(164, 6)
        '
        'CutToolStripMenuItem
        '
        Me.CutToolStripMenuItem.Image = CType(resources.GetObject("CutToolStripMenuItem.Image"), System.Drawing.Image)
        Me.CutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black
        Me.CutToolStripMenuItem.Name = "CutToolStripMenuItem"
        Me.CutToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.CutToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
        Me.CutToolStripMenuItem.Text = "Cu&t"
        '
        'CopyToolStripMenuItem
        '
        Me.CopyToolStripMenuItem.Image = CType(resources.GetObject("CopyToolStripMenuItem.Image"), System.Drawing.Image)
        Me.CopyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black
        Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        Me.CopyToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
        Me.CopyToolStripMenuItem.Text = "&Copy"
        '
        'PasteToolStripMenuItem
        '
        Me.PasteToolStripMenuItem.Image = CType(resources.GetObject("PasteToolStripMenuItem.Image"), System.Drawing.Image)
        Me.PasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black
        Me.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem"
        Me.PasteToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.PasteToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
        Me.PasteToolStripMenuItem.Text = "&Paste"
        '
        'SelectAllToolStripMenuItem
        '
        Me.SelectAllToolStripMenuItem.Name = "SelectAllToolStripMenuItem"
        Me.SelectAllToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.SelectAllToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
        Me.SelectAllToolStripMenuItem.Text = "Select &All"
        '
        'tabSource
        '
        Me.tabSource.Controls.Add(Me.data)
        Me.tabSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabSource.ForeColor = System.Drawing.Color.Red
        Me.tabSource.Location = New System.Drawing.Point(4, 4)
        Me.tabSource.Name = "tabSource"
        Me.tabSource.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSource.Size = New System.Drawing.Size(470, 213)
        Me.tabSource.TabIndex = 0
        Me.tabSource.Text = "Source"
        Me.tabSource.UseVisualStyleBackColor = True
        '
        'data
        '
        Me.data.ContextMenuStrip = Me.cMenuSource
        Me.data.Dock = System.Windows.Forms.DockStyle.Fill
        Me.data.Font = New System.Drawing.Font("Lucida Console", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.data.ForeColor = System.Drawing.Color.Blue
        Me.data.Location = New System.Drawing.Point(3, 3)
        Me.data.Multiline = True
        Me.data.Name = "data"
        Me.data.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.data.Size = New System.Drawing.Size(464, 207)
        Me.data.TabIndex = 2
        '
        'cMenuSource
        '
        Me.cMenuSource.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem2, Me.ToolStripMenuItem3, Me.ToolStripSeparator1, Me.ToolStripMenuItem6, Me.ToolStripMenuItem7, Me.ToolStripMenuItem8, Me.ToolStripMenuItem9})
        Me.cMenuSource.Name = "cMenuWhere"
        Me.cMenuSource.Size = New System.Drawing.Size(168, 142)
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Z), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(167, 22)
        Me.ToolStripMenuItem2.Text = "Undo"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem4, Me.ToolStripMenuItem5})
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(167, 22)
        Me.ToolStripMenuItem3.Text = "Change Case"
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(114, 22)
        Me.ToolStripMenuItem4.Text = "Upper"
        '
        'ToolStripMenuItem5
        '
        Me.ToolStripMenuItem5.Name = "ToolStripMenuItem5"
        Me.ToolStripMenuItem5.Size = New System.Drawing.Size(114, 22)
        Me.ToolStripMenuItem5.Text = "Lower"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(164, 6)
        '
        'ToolStripMenuItem6
        '
        Me.ToolStripMenuItem6.Image = CType(resources.GetObject("ToolStripMenuItem6.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem6.ImageTransparentColor = System.Drawing.Color.Black
        Me.ToolStripMenuItem6.Name = "ToolStripMenuItem6"
        Me.ToolStripMenuItem6.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem6.Size = New System.Drawing.Size(167, 22)
        Me.ToolStripMenuItem6.Text = "Cu&t"
        '
        'ToolStripMenuItem7
        '
        Me.ToolStripMenuItem7.Image = CType(resources.GetObject("ToolStripMenuItem7.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem7.ImageTransparentColor = System.Drawing.Color.Black
        Me.ToolStripMenuItem7.Name = "ToolStripMenuItem7"
        Me.ToolStripMenuItem7.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem7.Size = New System.Drawing.Size(167, 22)
        Me.ToolStripMenuItem7.Text = "&Copy"
        '
        'ToolStripMenuItem8
        '
        Me.ToolStripMenuItem8.Image = CType(resources.GetObject("ToolStripMenuItem8.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem8.ImageTransparentColor = System.Drawing.Color.Black
        Me.ToolStripMenuItem8.Name = "ToolStripMenuItem8"
        Me.ToolStripMenuItem8.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem8.Size = New System.Drawing.Size(167, 22)
        Me.ToolStripMenuItem8.Text = "&Paste"
        '
        'ToolStripMenuItem9
        '
        Me.ToolStripMenuItem9.Name = "ToolStripMenuItem9"
        Me.ToolStripMenuItem9.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem9.Size = New System.Drawing.Size(167, 22)
        Me.ToolStripMenuItem9.Text = "Select &All"
        '
        'tabLoadRef
        '
        Me.tabLoadRef.Controls.Add(Me.lblNoref)
        Me.tabLoadRef.Controls.Add(Me.LoadRef)
        Me.tabLoadRef.Location = New System.Drawing.Point(4, 4)
        Me.tabLoadRef.Name = "tabLoadRef"
        Me.tabLoadRef.Padding = New System.Windows.Forms.Padding(3)
        Me.tabLoadRef.Size = New System.Drawing.Size(470, 213)
        Me.tabLoadRef.TabIndex = 1
        Me.tabLoadRef.Text = "Load Reference"
        Me.tabLoadRef.UseVisualStyleBackColor = True
        '
        'lblNoref
        '
        Me.lblNoref.AutoSize = True
        Me.lblNoref.Location = New System.Drawing.Point(180, 99)
        Me.lblNoref.Name = "lblNoref"
        Me.lblNoref.Size = New System.Drawing.Size(132, 13)
        Me.lblNoref.TabIndex = 2
        Me.lblNoref.Text = "No current Load reference"
        Me.lblNoref.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LoadRef
        '
        Me.LoadRef.ContextMenuStrip = Me.cMenuLoadRef
        Me.LoadRef.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LoadRef.ForeColor = System.Drawing.Color.Blue
        Me.LoadRef.FullRowSelect = True
        Me.LoadRef.Location = New System.Drawing.Point(3, 3)
        Me.LoadRef.Name = "LoadRef"
        Me.LoadRef.Size = New System.Drawing.Size(464, 207)
        Me.LoadRef.TabIndex = 0
        Me.LoadRef.UseCompatibleStateImageBehavior = False
        Me.LoadRef.View = System.Windows.Forms.View.Details
        Me.LoadRef.Visible = False
        '
        'cMenuLoadRef
        '
        Me.cMenuLoadRef.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TToolStripMenuItem})
        Me.cMenuLoadRef.Name = "cMenuLoadRef"
        Me.cMenuLoadRef.Size = New System.Drawing.Size(146, 26)
        '
        'TToolStripMenuItem
        '
        Me.TToolStripMenuItem.Name = "TToolStripMenuItem"
        Me.TToolStripMenuItem.Size = New System.Drawing.Size(145, 22)
        Me.TToolStripMenuItem.Text = "To Clipboard"
        '
        'output
        '
        Me.output.BackColor = System.Drawing.SystemColors.InactiveBorder
        Me.output.Dock = System.Windows.Forms.DockStyle.Fill
        Me.output.Font = New System.Drawing.Font("Lucida Console", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.output.ForeColor = System.Drawing.Color.MidnightBlue
        Me.output.Location = New System.Drawing.Point(0, 0)
        Me.output.Multiline = True
        Me.output.Name = "output"
        Me.output.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.output.Size = New System.Drawing.Size(478, 140)
        Me.output.TabIndex = 2
        Me.output.WordWrap = False
        '
        'MDIChild
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(478, 383)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "MDIChild"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "eMerge DBI"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        Me.SplitContainer1.ResumeLayout(False)
        Me.tab.ResumeLayout(False)
        Me.tabVariables.ResumeLayout(False)
        Me.cMenuVars.ResumeLayout(False)
        Me.tabWhere.ResumeLayout(False)
        Me.tabWhere.PerformLayout()
        Me.cMenuWhere.ResumeLayout(False)
        Me.tabSource.ResumeLayout(False)
        Me.tabSource.PerformLayout()
        Me.cMenuSource.ResumeLayout(False)
        Me.tabLoadRef.ResumeLayout(False)
        Me.tabLoadRef.PerformLayout()
        Me.cMenuLoadRef.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents output As System.Windows.Forms.TextBox
    Friend WithEvents tab As System.Windows.Forms.TabControl
    Friend WithEvents tabSource As System.Windows.Forms.TabPage
    Friend WithEvents data As System.Windows.Forms.TextBox
    Friend WithEvents tabLoadRef As System.Windows.Forms.TabPage
    Friend WithEvents LoadRef As System.Windows.Forms.ListView
    Friend WithEvents tabVariables As System.Windows.Forms.TabPage
    Friend WithEvents tabWhere As System.Windows.Forms.TabPage
    Friend WithEvents txtWhere As System.Windows.Forms.TextBox
    Friend WithEvents lstVariables As System.Windows.Forms.ListView
    Friend WithEvents cMenuWhere As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents UndoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CaseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UpperToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LowerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents CutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PasteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SelectAllToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cMenuSource As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem5 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItem6 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem7 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem8 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem9 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cMenuVars As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ToggleHiddenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem10 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SelectToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AllToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NoneToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents InvertToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TableToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DataTypeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents INTToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents REALToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CHARToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DATEToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem11 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TablesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DataTypeToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents JoinToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem12 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents VisibleToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HiddenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TableDictionaryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblNoref As System.Windows.Forms.Label
    Friend WithEvents cMenuLoadRef As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents TToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HideSelectionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ShowSelectionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem13 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItem14 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem15 As System.Windows.Forms.ToolStripSeparator

End Class
