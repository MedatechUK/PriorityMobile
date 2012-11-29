<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChild
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
        Dim TreeNode1 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Tables", 0, 0)
        Dim TreeNode2 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Forms")
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmChild))
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.library = New System.Windows.Forms.TreeView
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewFormTabToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.FormToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TabToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewTableToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DeleteTableToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RenameToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator
        Me.NewColumnToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewKeyColumnToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewUpdateColumnToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewFieldToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.lib_imges = New System.Windows.Forms.ImageList(Me.components)
        Me.FormDetail = New System.Windows.Forms.PropertyGrid
        Me.FieldDetail = New System.Windows.Forms.SplitContainer
        Me.FieldView = New System.Windows.Forms.DataGridView
        Me.msField = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddFieldToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RemoveFieldToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MoveFieldUpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MoveFieldDownToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.FieldProperties = New System.Windows.Forms.PropertyGrid
        Me.TableDetail = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer
        Me.txtSQL = New System.Windows.Forms.TextBox
        Me.SQLMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RunQueryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.GenerateColumnsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SQLOut = New System.Windows.Forms.DataGridView
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.msColumns = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddColToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RemoveColToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MoveColUpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MoveColDownToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ColumnView = New System.Windows.Forms.DataGridView
        Me.TabPage3 = New System.Windows.Forms.TabPage
        Me.KeyView = New System.Windows.Forms.DataGridView
        Me.msKeys = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddKeyToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.RemoveKeyToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.TabPage4 = New System.Windows.Forms.TabPage
        Me.UpdateView = New System.Windows.Forms.DataGridView
        Me.msUpdate = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddUpdateToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem
        Me.RemoveUpdateToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem
        Me.BuildToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.UpDownToolStripMenuItem = New System.Windows.Forms.ToolStripSeparator
        Me.MoveUpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MoveDownToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BindSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.fieldSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.FieldDetail.Panel1.SuspendLayout()
        Me.FieldDetail.Panel2.SuspendLayout()
        Me.FieldDetail.SuspendLayout()
        CType(Me.FieldView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.msField.SuspendLayout()
        Me.TableDetail.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.SQLMenu.SuspendLayout()
        CType(Me.SQLOut, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        Me.msColumns.SuspendLayout()
        CType(Me.ColumnView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage3.SuspendLayout()
        CType(Me.KeyView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.msKeys.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        CType(Me.UpdateView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.msUpdate.SuspendLayout()
        CType(Me.BindSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.fieldSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.library)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.FormDetail)
        Me.SplitContainer1.Panel2.Controls.Add(Me.FieldDetail)
        Me.SplitContainer1.Panel2.Controls.Add(Me.TableDetail)
        Me.SplitContainer1.Size = New System.Drawing.Size(774, 495)
        Me.SplitContainer1.SplitterDistance = 208
        Me.SplitContainer1.TabIndex = 1
        Me.SplitContainer1.TabStop = False
        '
        'library
        '
        Me.library.ContextMenuStrip = Me.ContextMenuStrip1
        Me.library.Dock = System.Windows.Forms.DockStyle.Fill
        Me.library.ImageIndex = 0
        Me.library.ImageList = Me.lib_imges
        Me.library.Location = New System.Drawing.Point(0, 0)
        Me.library.Name = "library"
        TreeNode1.ImageIndex = 0
        TreeNode1.Name = "Node0"
        TreeNode1.SelectedImageIndex = 0
        TreeNode1.Tag = "lib"
        TreeNode1.Text = "Tables"
        TreeNode2.ImageKey = "MDIPARNT.ICO"
        TreeNode2.Name = "Node1"
        TreeNode2.SelectedImageIndex = 1
        TreeNode2.Tag = "int"
        TreeNode2.Text = "Forms"
        Me.library.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode1, TreeNode2})
        Me.library.SelectedImageIndex = 0
        Me.library.Size = New System.Drawing.Size(208, 495)
        Me.library.TabIndex = 1
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewFormTabToolStripMenuItem, Me.NewTableToolStripMenuItem, Me.DeleteTableToolStripMenuItem, Me.RenameToolStripMenuItem, Me.ToolStripMenuItem1, Me.NewColumnToolStripMenuItem, Me.NewKeyColumnToolStripMenuItem, Me.NewUpdateColumnToolStripMenuItem, Me.NewFieldToolStripMenuItem, Me.UpDownToolStripMenuItem, Me.MoveUpToolStripMenuItem, Me.MoveDownToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(186, 236)
        '
        'NewFormTabToolStripMenuItem
        '
        Me.NewFormTabToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FormToolStripMenuItem, Me.TabToolStripMenuItem})
        Me.NewFormTabToolStripMenuItem.Name = "NewFormTabToolStripMenuItem"
        Me.NewFormTabToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.NewFormTabToolStripMenuItem.Text = "New"
        '
        'FormToolStripMenuItem
        '
        Me.FormToolStripMenuItem.Name = "FormToolStripMenuItem"
        Me.FormToolStripMenuItem.Size = New System.Drawing.Size(102, 22)
        Me.FormToolStripMenuItem.Text = "Form"
        '
        'TabToolStripMenuItem
        '
        Me.TabToolStripMenuItem.Name = "TabToolStripMenuItem"
        Me.TabToolStripMenuItem.Size = New System.Drawing.Size(102, 22)
        Me.TabToolStripMenuItem.Text = "Tab"
        '
        'NewTableToolStripMenuItem
        '
        Me.NewTableToolStripMenuItem.Name = "NewTableToolStripMenuItem"
        Me.NewTableToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Insert
        Me.NewTableToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.NewTableToolStripMenuItem.Text = "New"
        '
        'DeleteTableToolStripMenuItem
        '
        Me.DeleteTableToolStripMenuItem.Name = "DeleteTableToolStripMenuItem"
        Me.DeleteTableToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete
        Me.DeleteTableToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.DeleteTableToolStripMenuItem.Text = "Delete"
        '
        'RenameToolStripMenuItem
        '
        Me.RenameToolStripMenuItem.Name = "RenameToolStripMenuItem"
        Me.RenameToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.RenameToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.RenameToolStripMenuItem.Text = "Rename"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(182, 6)
        '
        'NewColumnToolStripMenuItem
        '
        Me.NewColumnToolStripMenuItem.Name = "NewColumnToolStripMenuItem"
        Me.NewColumnToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.NewColumnToolStripMenuItem.Text = "New Column"
        '
        'NewKeyColumnToolStripMenuItem
        '
        Me.NewKeyColumnToolStripMenuItem.Name = "NewKeyColumnToolStripMenuItem"
        Me.NewKeyColumnToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.NewKeyColumnToolStripMenuItem.Text = "New Key Column"
        '
        'NewUpdateColumnToolStripMenuItem
        '
        Me.NewUpdateColumnToolStripMenuItem.Name = "NewUpdateColumnToolStripMenuItem"
        Me.NewUpdateColumnToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.NewUpdateColumnToolStripMenuItem.Text = "New Update Column"
        '
        'NewFieldToolStripMenuItem
        '
        Me.NewFieldToolStripMenuItem.Name = "NewFieldToolStripMenuItem"
        Me.NewFieldToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.NewFieldToolStripMenuItem.Text = "New Field"
        '
        'lib_imges
        '
        Me.lib_imges.ImageStream = CType(resources.GetObject("lib_imges.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.lib_imges.TransparentColor = System.Drawing.Color.Transparent
        Me.lib_imges.Images.SetKeyName(0, "")
        Me.lib_imges.Images.SetKeyName(1, "MDIPARNT.ICO")
        Me.lib_imges.Images.SetKeyName(2, "TABLE.ico")
        Me.lib_imges.Images.SetKeyName(3, "TABLE_SEL.ico")
        Me.lib_imges.Images.SetKeyName(4, "FORM.ICO")
        Me.lib_imges.Images.SetKeyName(5, "FORM_SEL.ICO")
        Me.lib_imges.Images.SetKeyName(6, "MDICHILD.ICO")
        Me.lib_imges.Images.SetKeyName(7, "MDICHILD_SEL.ICO")
        '
        'FormDetail
        '
        Me.FormDetail.Location = New System.Drawing.Point(283, 144)
        Me.FormDetail.Name = "FormDetail"
        Me.FormDetail.Size = New System.Drawing.Size(232, 130)
        Me.FormDetail.TabIndex = 2
        '
        'FieldDetail
        '
        Me.FieldDetail.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.FieldDetail.Location = New System.Drawing.Point(100, 298)
        Me.FieldDetail.Name = "FieldDetail"
        '
        'FieldDetail.Panel1
        '
        Me.FieldDetail.Panel1.Controls.Add(Me.FieldView)
        '
        'FieldDetail.Panel2
        '
        Me.FieldDetail.Panel2.Controls.Add(Me.FieldProperties)
        Me.FieldDetail.Size = New System.Drawing.Size(287, 116)
        Me.FieldDetail.SplitterDistance = 97
        Me.FieldDetail.TabIndex = 1
        '
        'FieldView
        '
        Me.FieldView.AllowUserToAddRows = False
        Me.FieldView.AllowUserToDeleteRows = False
        Me.FieldView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.FieldView.ContextMenuStrip = Me.msField
        Me.FieldView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FieldView.Location = New System.Drawing.Point(0, 0)
        Me.FieldView.Name = "FieldView"
        Me.FieldView.Size = New System.Drawing.Size(97, 116)
        Me.FieldView.TabIndex = 0
        '
        'msField
        '
        Me.msField.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddFieldToolStripMenuItem, Me.RemoveFieldToolStripMenuItem, Me.MoveFieldUpToolStripMenuItem, Me.MoveFieldDownToolStripMenuItem})
        Me.msField.Name = "msField"
        Me.msField.Size = New System.Drawing.Size(139, 92)
        '
        'AddFieldToolStripMenuItem
        '
        Me.AddFieldToolStripMenuItem.Name = "AddFieldToolStripMenuItem"
        Me.AddFieldToolStripMenuItem.Size = New System.Drawing.Size(138, 22)
        Me.AddFieldToolStripMenuItem.Text = "Add"
        '
        'RemoveFieldToolStripMenuItem
        '
        Me.RemoveFieldToolStripMenuItem.Name = "RemoveFieldToolStripMenuItem"
        Me.RemoveFieldToolStripMenuItem.Size = New System.Drawing.Size(138, 22)
        Me.RemoveFieldToolStripMenuItem.Text = "Remove"
        '
        'MoveFieldUpToolStripMenuItem
        '
        Me.MoveFieldUpToolStripMenuItem.Name = "MoveFieldUpToolStripMenuItem"
        Me.MoveFieldUpToolStripMenuItem.Size = New System.Drawing.Size(138, 22)
        Me.MoveFieldUpToolStripMenuItem.Text = "Move Up"
        '
        'MoveFieldDownToolStripMenuItem
        '
        Me.MoveFieldDownToolStripMenuItem.Name = "MoveFieldDownToolStripMenuItem"
        Me.MoveFieldDownToolStripMenuItem.Size = New System.Drawing.Size(138, 22)
        Me.MoveFieldDownToolStripMenuItem.Text = "Move Down"
        '
        'FieldProperties
        '
        Me.FieldProperties.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FieldProperties.Location = New System.Drawing.Point(0, 0)
        Me.FieldProperties.Name = "FieldProperties"
        Me.FieldProperties.Size = New System.Drawing.Size(186, 116)
        Me.FieldProperties.TabIndex = 0
        '
        'TableDetail
        '
        Me.TableDetail.Alignment = System.Windows.Forms.TabAlignment.Bottom
        Me.TableDetail.Controls.Add(Me.TabPage1)
        Me.TableDetail.Controls.Add(Me.TabPage2)
        Me.TableDetail.Controls.Add(Me.TabPage3)
        Me.TableDetail.Controls.Add(Me.TabPage4)
        Me.TableDetail.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TableDetail.Location = New System.Drawing.Point(0, 0)
        Me.TableDetail.Name = "TableDetail"
        Me.TableDetail.Padding = New System.Drawing.Point(25, 10)
        Me.TableDetail.SelectedIndex = 0
        Me.TableDetail.Size = New System.Drawing.Size(299, 209)
        Me.TableDetail.TabIndex = 0
        Me.TableDetail.Visible = False
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.SplitContainer2)
        Me.TabPage1.Location = New System.Drawing.Point(4, 4)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(291, 162)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "SQL"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(3, 3)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.txtSQL)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.SQLOut)
        Me.SplitContainer2.Size = New System.Drawing.Size(285, 156)
        Me.SplitContainer2.SplitterDistance = 97
        Me.SplitContainer2.TabIndex = 0
        '
        'txtSQL
        '
        Me.txtSQL.ContextMenuStrip = Me.SQLMenu
        Me.txtSQL.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtSQL.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSQL.Location = New System.Drawing.Point(0, 0)
        Me.txtSQL.Multiline = True
        Me.txtSQL.Name = "txtSQL"
        Me.txtSQL.Size = New System.Drawing.Size(285, 97)
        Me.txtSQL.TabIndex = 0
        '
        'SQLMenu
        '
        Me.SQLMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RunQueryToolStripMenuItem, Me.GenerateColumnsToolStripMenuItem})
        Me.SQLMenu.Name = "SQLMenu"
        Me.SQLMenu.Size = New System.Drawing.Size(219, 48)
        '
        'RunQueryToolStripMenuItem
        '
        Me.RunQueryToolStripMenuItem.Name = "RunQueryToolStripMenuItem"
        Me.RunQueryToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5
        Me.RunQueryToolStripMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.RunQueryToolStripMenuItem.Text = "Run Query"
        '
        'GenerateColumnsToolStripMenuItem
        '
        Me.GenerateColumnsToolStripMenuItem.Name = "GenerateColumnsToolStripMenuItem"
        Me.GenerateColumnsToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F5), System.Windows.Forms.Keys)
        Me.GenerateColumnsToolStripMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.GenerateColumnsToolStripMenuItem.Text = "Generate Columns"
        '
        'SQLOut
        '
        Me.SQLOut.AllowUserToAddRows = False
        Me.SQLOut.AllowUserToDeleteRows = False
        Me.SQLOut.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.SQLOut.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SQLOut.Location = New System.Drawing.Point(0, 0)
        Me.SQLOut.Name = "SQLOut"
        Me.SQLOut.ReadOnly = True
        Me.SQLOut.Size = New System.Drawing.Size(285, 55)
        Me.SQLOut.TabIndex = 0
        '
        'TabPage2
        '
        Me.TabPage2.ContextMenuStrip = Me.msColumns
        Me.TabPage2.Controls.Add(Me.ColumnView)
        Me.TabPage2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabPage2.Location = New System.Drawing.Point(4, 4)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(291, 162)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Columns"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'msColumns
        '
        Me.msColumns.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddColToolStripMenuItem, Me.RemoveColToolStripMenuItem, Me.MoveColUpToolStripMenuItem, Me.MoveColDownToolStripMenuItem})
        Me.msColumns.Name = "msColumns"
        Me.msColumns.Size = New System.Drawing.Size(139, 92)
        '
        'AddColToolStripMenuItem
        '
        Me.AddColToolStripMenuItem.Name = "AddColToolStripMenuItem"
        Me.AddColToolStripMenuItem.Size = New System.Drawing.Size(138, 22)
        Me.AddColToolStripMenuItem.Text = "Add"
        '
        'RemoveColToolStripMenuItem
        '
        Me.RemoveColToolStripMenuItem.Name = "RemoveColToolStripMenuItem"
        Me.RemoveColToolStripMenuItem.Size = New System.Drawing.Size(138, 22)
        Me.RemoveColToolStripMenuItem.Text = "Remove"
        '
        'MoveColUpToolStripMenuItem
        '
        Me.MoveColUpToolStripMenuItem.Name = "MoveColUpToolStripMenuItem"
        Me.MoveColUpToolStripMenuItem.Size = New System.Drawing.Size(138, 22)
        Me.MoveColUpToolStripMenuItem.Text = "Move Up"
        '
        'MoveColDownToolStripMenuItem
        '
        Me.MoveColDownToolStripMenuItem.Name = "MoveColDownToolStripMenuItem"
        Me.MoveColDownToolStripMenuItem.Size = New System.Drawing.Size(138, 22)
        Me.MoveColDownToolStripMenuItem.Text = "Move Down"
        '
        'ColumnView
        '
        Me.ColumnView.AllowUserToAddRows = False
        Me.ColumnView.AllowUserToDeleteRows = False
        Me.ColumnView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.ColumnView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.ColumnView.ContextMenuStrip = Me.msColumns
        Me.ColumnView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ColumnView.Location = New System.Drawing.Point(3, 3)
        Me.ColumnView.Name = "ColumnView"
        Me.ColumnView.Size = New System.Drawing.Size(285, 156)
        Me.ColumnView.TabIndex = 2
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.KeyView)
        Me.TabPage3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabPage3.Location = New System.Drawing.Point(4, 4)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(291, 162)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Keys"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'KeyView
        '
        Me.KeyView.AllowUserToAddRows = False
        Me.KeyView.AllowUserToDeleteRows = False
        Me.KeyView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.KeyView.ContextMenuStrip = Me.msKeys
        Me.KeyView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.KeyView.Location = New System.Drawing.Point(3, 3)
        Me.KeyView.Name = "KeyView"
        Me.KeyView.Size = New System.Drawing.Size(285, 156)
        Me.KeyView.TabIndex = 1
        '
        'msKeys
        '
        Me.msKeys.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddKeyToolStripMenuItem1, Me.RemoveKeyToolStripMenuItem1})
        Me.msKeys.Name = "msKeys"
        Me.msKeys.Size = New System.Drawing.Size(118, 48)
        '
        'AddKeyToolStripMenuItem1
        '
        Me.AddKeyToolStripMenuItem1.Name = "AddKeyToolStripMenuItem1"
        Me.AddKeyToolStripMenuItem1.Size = New System.Drawing.Size(117, 22)
        Me.AddKeyToolStripMenuItem1.Text = "Add"
        '
        'RemoveKeyToolStripMenuItem1
        '
        Me.RemoveKeyToolStripMenuItem1.Name = "RemoveKeyToolStripMenuItem1"
        Me.RemoveKeyToolStripMenuItem1.Size = New System.Drawing.Size(117, 22)
        Me.RemoveKeyToolStripMenuItem1.Text = "Remove"
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.UpdateView)
        Me.TabPage4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabPage4.Location = New System.Drawing.Point(4, 4)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage4.Size = New System.Drawing.Size(291, 162)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "Update"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'UpdateView
        '
        Me.UpdateView.AllowUserToAddRows = False
        Me.UpdateView.AllowUserToDeleteRows = False
        Me.UpdateView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.UpdateView.ContextMenuStrip = Me.msUpdate
        Me.UpdateView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UpdateView.Location = New System.Drawing.Point(3, 3)
        Me.UpdateView.Name = "UpdateView"
        Me.UpdateView.Size = New System.Drawing.Size(285, 156)
        Me.UpdateView.TabIndex = 1
        '
        'msUpdate
        '
        Me.msUpdate.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddUpdateToolStripMenuItem2, Me.RemoveUpdateToolStripMenuItem2})
        Me.msUpdate.Name = "msUpdate"
        Me.msUpdate.Size = New System.Drawing.Size(118, 48)
        '
        'AddUpdateToolStripMenuItem2
        '
        Me.AddUpdateToolStripMenuItem2.Name = "AddUpdateToolStripMenuItem2"
        Me.AddUpdateToolStripMenuItem2.Size = New System.Drawing.Size(117, 22)
        Me.AddUpdateToolStripMenuItem2.Text = "Add"
        '
        'RemoveUpdateToolStripMenuItem2
        '
        Me.RemoveUpdateToolStripMenuItem2.Name = "RemoveUpdateToolStripMenuItem2"
        Me.RemoveUpdateToolStripMenuItem2.Size = New System.Drawing.Size(117, 22)
        Me.RemoveUpdateToolStripMenuItem2.Text = "Remove"
        '
        'BuildToolStripMenuItem
        '
        Me.BuildToolStripMenuItem.Name = "BuildToolStripMenuItem"
        Me.BuildToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.BuildToolStripMenuItem.Text = "Build"
        '
        'UpDownToolStripMenuItem
        '
        Me.UpDownToolStripMenuItem.Name = "UpDownToolStripMenuItem"
        Me.UpDownToolStripMenuItem.Size = New System.Drawing.Size(182, 6)
        '
        'MoveUpToolStripMenuItem
        '
        Me.MoveUpToolStripMenuItem.Name = "MoveUpToolStripMenuItem"
        Me.MoveUpToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.MoveUpToolStripMenuItem.Text = "Move Up"
        '
        'MoveDownToolStripMenuItem
        '
        Me.MoveDownToolStripMenuItem.Name = "MoveDownToolStripMenuItem"
        Me.MoveDownToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.MoveDownToolStripMenuItem.Text = "Move Down"
        '
        'frmChild
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(774, 495)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmChild"
        Me.Text = "frmChild"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.FieldDetail.Panel1.ResumeLayout(False)
        Me.FieldDetail.Panel2.ResumeLayout(False)
        Me.FieldDetail.ResumeLayout(False)
        CType(Me.FieldView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.msField.ResumeLayout(False)
        Me.TableDetail.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.PerformLayout()
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.ResumeLayout(False)
        Me.SQLMenu.ResumeLayout(False)
        CType(Me.SQLOut, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage2.ResumeLayout(False)
        Me.msColumns.ResumeLayout(False)
        CType(Me.ColumnView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage3.ResumeLayout(False)
        CType(Me.KeyView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.msKeys.ResumeLayout(False)
        Me.TabPage4.ResumeLayout(False)
        CType(Me.UpdateView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.msUpdate.ResumeLayout(False)
        CType(Me.BindSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.fieldSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents library As System.Windows.Forms.TreeView
    Friend WithEvents lib_imges As System.Windows.Forms.ImageList
    Friend WithEvents TableDetail As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents txtSQL As System.Windows.Forms.TextBox
    Friend WithEvents BindSource As System.Windows.Forms.BindingSource
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewColumnToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewKeyColumnToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewUpdateColumnToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents KeyView As System.Windows.Forms.DataGridView
    Friend WithEvents msColumns As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents msKeys As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents msUpdate As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ColumnView As System.Windows.Forms.DataGridView
    Friend WithEvents UpdateView As System.Windows.Forms.DataGridView
    Friend WithEvents AddColToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoveColToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddKeyToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoveKeyToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddUpdateToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoveUpdateToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewTableToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BuildToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RenameToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SQLMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents RunQueryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SQLOut As System.Windows.Forms.DataGridView
    Friend WithEvents DeleteTableToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents NewFormTabToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FormToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TabToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FieldDetail As System.Windows.Forms.SplitContainer
    Friend WithEvents FieldView As System.Windows.Forms.DataGridView
    Friend WithEvents FieldProperties As System.Windows.Forms.PropertyGrid
    Friend WithEvents FormDetail As System.Windows.Forms.PropertyGrid
    Friend WithEvents fieldSource As System.Windows.Forms.BindingSource
    Friend WithEvents NewFieldToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents msField As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AddFieldToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoveFieldToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MoveFieldUpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MoveFieldDownToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MoveColUpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MoveColDownToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GenerateColumnsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UpDownToolStripMenuItem As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MoveUpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MoveDownToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
