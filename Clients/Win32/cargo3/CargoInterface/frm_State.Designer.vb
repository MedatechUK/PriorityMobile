<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_State
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
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel
        Me.lst_States = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ToolStrip3 = New System.Windows.Forms.ToolStrip
        Me.NewState = New System.Windows.Forms.ToolStripLabel
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
        Me.SplitContainer3 = New System.Windows.Forms.SplitContainer
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.lst_Actions = New System.Windows.Forms.ListView
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.NewAction = New System.Windows.Forms.ToolStripLabel
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.lst_Conditions = New System.Windows.Forms.ListView
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ToolStrip2 = New System.Windows.Forms.ToolStrip
        Me.NewCondition = New System.Windows.Forms.ToolStripLabel
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.PropertyGrid = New System.Windows.Forms.PropertyGrid
        Me.DeleteState = New System.Windows.Forms.ToolStripButton
        Me.btn_Up = New System.Windows.Forms.ToolStripButton
        Me.btn_Down = New System.Windows.Forms.ToolStripButton
        Me.DeleteAction = New System.Windows.Forms.ToolStripButton
        Me.DeleteCondition = New System.Windows.Forms.ToolStripButton
        Me.btn_TestCondition = New System.Windows.Forms.ToolStripButton
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.ToolStrip3.SuspendLayout()
        Me.SplitContainer3.Panel1.SuspendLayout()
        Me.SplitContainer3.Panel2.SuspendLayout()
        Me.SplitContainer3.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.ToolStrip2.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.SplitContainer2)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.PropertyGrid)
        Me.SplitContainer1.Size = New System.Drawing.Size(627, 498)
        Me.SplitContainer1.SplitterDistance = 218
        Me.SplitContainer1.TabIndex = 2
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.TableLayoutPanel3)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.SplitContainer3)
        Me.SplitContainer2.Size = New System.Drawing.Size(218, 498)
        Me.SplitContainer2.SplitterDistance = 132
        Me.SplitContainer2.TabIndex = 4
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 1
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.Controls.Add(Me.lst_States, 0, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.ToolStrip3, 0, 0)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 2
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(218, 132)
        Me.TableLayoutPanel3.TabIndex = 10
        '
        'lst_States
        '
        Me.lst_States.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.lst_States.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lst_States.FullRowSelect = True
        Me.lst_States.HideSelection = False
        Me.lst_States.Location = New System.Drawing.Point(3, 28)
        Me.lst_States.MultiSelect = False
        Me.lst_States.Name = "lst_States"
        Me.lst_States.Size = New System.Drawing.Size(212, 101)
        Me.lst_States.TabIndex = 9
        Me.lst_States.UseCompatibleStateImageBehavior = False
        Me.lst_States.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "States"
        Me.ColumnHeader1.Width = 200
        '
        'ToolStrip3
        '
        Me.ToolStrip3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStrip3.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewState, Me.ToolStripSeparator4, Me.DeleteState})
        Me.ToolStrip3.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip3.Name = "ToolStrip3"
        Me.ToolStrip3.Size = New System.Drawing.Size(218, 25)
        Me.ToolStrip3.TabIndex = 0
        Me.ToolStrip3.Text = "ToolStrip3"
        '
        'NewState
        '
        Me.NewState.Name = "NewState"
        Me.NewState.Size = New System.Drawing.Size(31, 22)
        Me.NewState.Text = "New"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 25)
        '
        'SplitContainer3
        '
        Me.SplitContainer3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer3.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer3.Name = "SplitContainer3"
        Me.SplitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer3.Panel1
        '
        Me.SplitContainer3.Panel1.Controls.Add(Me.TableLayoutPanel1)
        '
        'SplitContainer3.Panel2
        '
        Me.SplitContainer3.Panel2.Controls.Add(Me.TableLayoutPanel2)
        Me.SplitContainer3.Size = New System.Drawing.Size(218, 362)
        Me.SplitContainer3.SplitterDistance = 186
        Me.SplitContainer3.TabIndex = 0
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.lst_Actions, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.ToolStrip1, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(218, 186)
        Me.TableLayoutPanel1.TabIndex = 9
        '
        'lst_Actions
        '
        Me.lst_Actions.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader2})
        Me.lst_Actions.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lst_Actions.FullRowSelect = True
        Me.lst_Actions.HideSelection = False
        Me.lst_Actions.Location = New System.Drawing.Point(3, 28)
        Me.lst_Actions.MultiSelect = False
        Me.lst_Actions.Name = "lst_Actions"
        Me.lst_Actions.Size = New System.Drawing.Size(212, 155)
        Me.lst_Actions.TabIndex = 12
        Me.lst_Actions.UseCompatibleStateImageBehavior = False
        Me.lst_Actions.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Actions"
        Me.ColumnHeader2.Width = 200
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewAction, Me.ToolStripSeparator1, Me.btn_Up, Me.btn_Down, Me.ToolStripSeparator3, Me.DeleteAction})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(218, 25)
        Me.ToolStrip1.TabIndex = 9
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'NewAction
        '
        Me.NewAction.Name = "NewAction"
        Me.NewAction.Size = New System.Drawing.Size(31, 22)
        Me.NewAction.Text = "New"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 25)
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 1
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.lst_Conditions, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.ToolStrip2, 0, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 2
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(218, 172)
        Me.TableLayoutPanel2.TabIndex = 9
        '
        'lst_Conditions
        '
        Me.lst_Conditions.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader3})
        Me.lst_Conditions.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lst_Conditions.FullRowSelect = True
        Me.lst_Conditions.HideSelection = False
        Me.lst_Conditions.Location = New System.Drawing.Point(3, 28)
        Me.lst_Conditions.MultiSelect = False
        Me.lst_Conditions.Name = "lst_Conditions"
        Me.lst_Conditions.Size = New System.Drawing.Size(212, 141)
        Me.lst_Conditions.TabIndex = 8
        Me.lst_Conditions.UseCompatibleStateImageBehavior = False
        Me.lst_Conditions.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Conditions"
        Me.ColumnHeader3.Width = 200
        '
        'ToolStrip2
        '
        Me.ToolStrip2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewCondition, Me.ToolStripSeparator2, Me.btn_TestCondition, Me.DeleteCondition})
        Me.ToolStrip2.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip2.Name = "ToolStrip2"
        Me.ToolStrip2.Size = New System.Drawing.Size(218, 25)
        Me.ToolStrip2.TabIndex = 0
        Me.ToolStrip2.Text = "ToolStrip2"
        '
        'NewCondition
        '
        Me.NewCondition.Name = "NewCondition"
        Me.NewCondition.Size = New System.Drawing.Size(31, 22)
        Me.NewCondition.Text = "New"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'PropertyGrid
        '
        Me.PropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PropertyGrid.Location = New System.Drawing.Point(0, 0)
        Me.PropertyGrid.Name = "PropertyGrid"
        Me.PropertyGrid.Size = New System.Drawing.Size(405, 498)
        Me.PropertyGrid.TabIndex = 1
        '
        'DeleteState
        '
        Me.DeleteState.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.DeleteState.Image = Global.cargo3.My.Resources.Resources.W95MBX01
        Me.DeleteState.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.DeleteState.Name = "DeleteState"
        Me.DeleteState.Size = New System.Drawing.Size(23, 22)
        Me.DeleteState.Text = "ToolStripButton5"
        Me.DeleteState.ToolTipText = "Delete State"
        '
        'btn_Up
        '
        Me.btn_Up.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_Up.Image = Global.cargo3.My.Resources.Resources.ARW05UP
        Me.btn_Up.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_Up.Name = "btn_Up"
        Me.btn_Up.Size = New System.Drawing.Size(23, 22)
        Me.btn_Up.Text = "Move Up"
        '
        'btn_Down
        '
        Me.btn_Down.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_Down.Image = Global.cargo3.My.Resources.Resources.ARW05DN
        Me.btn_Down.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_Down.Name = "btn_Down"
        Me.btn_Down.Size = New System.Drawing.Size(23, 22)
        Me.btn_Down.Text = "ToolStripButton2"
        Me.btn_Down.ToolTipText = "Move Down"
        '
        'DeleteAction
        '
        Me.DeleteAction.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.DeleteAction.Image = Global.cargo3.My.Resources.Resources.W95MBX01
        Me.DeleteAction.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.DeleteAction.Name = "DeleteAction"
        Me.DeleteAction.Size = New System.Drawing.Size(23, 22)
        Me.DeleteAction.Text = "ToolStripButton4"
        Me.DeleteAction.ToolTipText = "Delete Action"
        '
        'DeleteCondition
        '
        Me.DeleteCondition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.DeleteCondition.Image = Global.cargo3.My.Resources.Resources.W95MBX01
        Me.DeleteCondition.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.DeleteCondition.Name = "DeleteCondition"
        Me.DeleteCondition.Size = New System.Drawing.Size(23, 22)
        Me.DeleteCondition.Text = "ToolStripButton3"
        Me.DeleteCondition.ToolTipText = "Delete Condition"
        '
        'btn_TestCondition
        '
        Me.btn_TestCondition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_TestCondition.Image = Global.cargo3.My.Resources.Resources.W95MBX04
        Me.btn_TestCondition.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_TestCondition.Name = "btn_TestCondition"
        Me.btn_TestCondition.Size = New System.Drawing.Size(23, 22)
        Me.btn_TestCondition.Text = "ToolStripButton1"
        Me.btn_TestCondition.ToolTipText = "Test Condition"
        '
        'frm_State
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(627, 498)
        Me.ControlBox = False
        Me.Controls.Add(Me.SplitContainer1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(400, 300)
        Me.Name = "frm_State"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.ResumeLayout(False)
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel3.PerformLayout()
        Me.ToolStrip3.ResumeLayout(False)
        Me.ToolStrip3.PerformLayout()
        Me.SplitContainer3.Panel1.ResumeLayout(False)
        Me.SplitContainer3.Panel2.ResumeLayout(False)
        Me.SplitContainer3.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.ToolStrip2.ResumeLayout(False)
        Me.ToolStrip2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents PropertyGrid As System.Windows.Forms.PropertyGrid
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainer3 As System.Windows.Forms.SplitContainer
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lst_Actions As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents NewAction As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btn_Up As System.Windows.Forms.ToolStripButton
    Friend WithEvents btn_Down As System.Windows.Forms.ToolStripButton
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ToolStrip2 As System.Windows.Forms.ToolStrip
    Friend WithEvents lst_Conditions As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents NewCondition As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents DeleteCondition As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents DeleteAction As System.Windows.Forms.ToolStripButton
    Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lst_States As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ToolStrip3 As System.Windows.Forms.ToolStrip
    Friend WithEvents NewState As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents DeleteState As System.Windows.Forms.ToolStripButton
    Friend WithEvents btn_TestCondition As System.Windows.Forms.ToolStripButton
End Class
