<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_Script
    Inherits cargo3.ScriptFrm 'System.Windows.Forms.Form '

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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.btn_New = New System.Windows.Forms.ToolStripLabel
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel
        Me.lst_Scripts = New System.Windows.Forms.ToolStripComboBox
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.btn_Click = New System.Windows.Forms.ToolStripButton
        Me.btn_Drag = New System.Windows.Forms.ToolStripButton
        Me.btn_Keypress = New System.Windows.Forms.ToolStripButton
        Me.btn_Timer = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.btn_Up = New System.Windows.Forms.ToolStripButton
        Me.btn_Down = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.btn_Delete = New System.Windows.Forms.ToolStripButton
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer
        Me.lst_Step = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.PropertyGrid = New System.Windows.Forms.PropertyGrid
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator
        Me.TableLayoutPanel1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.ToolStrip1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.SplitContainer2, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(641, 399)
        Me.TableLayoutPanel1.TabIndex = 5
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btn_New, Me.ToolStripSeparator4, Me.ToolStripLabel1, Me.lst_Scripts, Me.ToolStripSeparator2, Me.ToolStripButton1, Me.ToolStripSeparator5, Me.btn_Click, Me.btn_Drag, Me.btn_Keypress, Me.btn_Timer, Me.ToolStripSeparator1, Me.btn_Up, Me.btn_Down, Me.ToolStripSeparator3, Me.btn_Delete})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(641, 25)
        Me.ToolStrip1.TabIndex = 5
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btn_New
        '
        Me.btn_New.Name = "btn_New"
        Me.btn_New.Size = New System.Drawing.Size(31, 22)
        Me.btn_New.Text = "New"
        Me.btn_New.ToolTipText = "New Script"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(37, 22)
        Me.ToolStripLabel1.Text = "Script"
        '
        'lst_Scripts
        '
        Me.lst_Scripts.Name = "lst_Scripts"
        Me.lst_Scripts.Size = New System.Drawing.Size(121, 25)
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'btn_Click
        '
        Me.btn_Click.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_Click.Enabled = False
        Me.btn_Click.Image = Global.cargo3.My.Resources.Resources.MOUSE02
        Me.btn_Click.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_Click.Name = "btn_Click"
        Me.btn_Click.Size = New System.Drawing.Size(23, 22)
        Me.btn_Click.Text = "ToolStripButton3"
        Me.btn_Click.ToolTipText = "New Click Action"
        '
        'btn_Drag
        '
        Me.btn_Drag.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_Drag.Enabled = False
        Me.btn_Drag.Image = Global.cargo3.My.Resources.Resources.DRAG3PG
        Me.btn_Drag.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_Drag.Name = "btn_Drag"
        Me.btn_Drag.Size = New System.Drawing.Size(23, 22)
        Me.btn_Drag.Text = "ToolStripButton2"
        Me.btn_Drag.ToolTipText = "New Drag Action"
        '
        'btn_Keypress
        '
        Me.btn_Keypress.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_Keypress.Enabled = False
        Me.btn_Keypress.Image = Global.cargo3.My.Resources.Resources.KEY04
        Me.btn_Keypress.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_Keypress.Name = "btn_Keypress"
        Me.btn_Keypress.Size = New System.Drawing.Size(23, 22)
        Me.btn_Keypress.Text = "ToolStripButton1"
        Me.btn_Keypress.ToolTipText = "New Keypress Action"
        '
        'btn_Timer
        '
        Me.btn_Timer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_Timer.Enabled = False
        Me.btn_Timer.Image = Global.cargo3.My.Resources.Resources.CLOCK02
        Me.btn_Timer.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_Timer.Name = "btn_Timer"
        Me.btn_Timer.Size = New System.Drawing.Size(23, 22)
        Me.btn_Timer.Text = "ToolStripButton4"
        Me.btn_Timer.ToolTipText = "New Delay Action"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'btn_Up
        '
        Me.btn_Up.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_Up.Enabled = False
        Me.btn_Up.Image = Global.cargo3.My.Resources.Resources.ARW05UP
        Me.btn_Up.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_Up.Name = "btn_Up"
        Me.btn_Up.Size = New System.Drawing.Size(23, 22)
        Me.btn_Up.Text = "ToolStripButton1"
        Me.btn_Up.ToolTipText = "Move Up"
        '
        'btn_Down
        '
        Me.btn_Down.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_Down.Enabled = False
        Me.btn_Down.Image = Global.cargo3.My.Resources.Resources.ARW05DN
        Me.btn_Down.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_Down.Name = "btn_Down"
        Me.btn_Down.Size = New System.Drawing.Size(23, 22)
        Me.btn_Down.Text = "ToolStripButton2"
        Me.btn_Down.ToolTipText = "Move Down"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 25)
        '
        'btn_Delete
        '
        Me.btn_Delete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_Delete.Image = Global.cargo3.My.Resources.Resources.W95MBX01
        Me.btn_Delete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_Delete.Name = "btn_Delete"
        Me.btn_Delete.Size = New System.Drawing.Size(23, 22)
        Me.btn_Delete.Text = "ToolStripButton1"
        Me.btn_Delete.ToolTipText = "Delete Step"
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(3, 28)
        Me.SplitContainer2.Name = "SplitContainer2"
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.lst_Step)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.PropertyGrid)
        Me.SplitContainer2.Size = New System.Drawing.Size(635, 368)
        Me.SplitContainer2.SplitterDistance = 332
        Me.SplitContainer2.TabIndex = 6
        '
        'lst_Step
        '
        Me.lst_Step.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.lst_Step.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lst_Step.FullRowSelect = True
        Me.lst_Step.HideSelection = False
        Me.lst_Step.Location = New System.Drawing.Point(0, 0)
        Me.lst_Step.MultiSelect = False
        Me.lst_Step.Name = "lst_Step"
        Me.lst_Step.Size = New System.Drawing.Size(332, 368)
        Me.lst_Step.TabIndex = 2
        Me.lst_Step.UseCompatibleStateImageBehavior = False
        Me.lst_Step.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Step"
        Me.ColumnHeader1.Width = 300
        '
        'PropertyGrid
        '
        Me.PropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PropertyGrid.Location = New System.Drawing.Point(0, 0)
        Me.PropertyGrid.Name = "PropertyGrid"
        Me.PropertyGrid.Size = New System.Drawing.Size(299, 368)
        Me.PropertyGrid.TabIndex = 1
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton1.Image = Global.cargo3.My.Resources.Resources.ARW01RT
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton1.Text = "ToolStripButton1"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(6, 25)
        '
        'frm_Script
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(641, 399)
        Me.ControlBox = False
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(500, 400)
        Me.Name = "frm_Script"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents btn_Click As System.Windows.Forms.ToolStripButton
    Friend WithEvents btn_Drag As System.Windows.Forms.ToolStripButton
    Friend WithEvents btn_Keypress As System.Windows.Forms.ToolStripButton
    Friend WithEvents btn_Timer As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btn_Up As System.Windows.Forms.ToolStripButton
    Friend WithEvents btn_Down As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents lst_Scripts As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents lst_Step As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents PropertyGrid As System.Windows.Forms.PropertyGrid
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btn_Delete As System.Windows.Forms.ToolStripButton
    Friend WithEvents btn_New As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripButton1 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
End Class
