<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCalc
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.DatePick = New System.Windows.Forms.DateTimePicker
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.txtHr = New System.Windows.Forms.TextBox
        Me.VScrollHour = New System.Windows.Forms.VScrollBar
        Me.VScrollMin = New System.Windows.Forms.VScrollBar
        Me.txtMin = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtPriMin = New System.Windows.Forms.Label
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PasteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.DatePick, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel2, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.txtPriMin, 0, 2)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 3
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(170, 90)
        Me.TableLayoutPanel1.TabIndex = 1
        '
        'DatePick
        '
        Me.DatePick.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DatePick.Location = New System.Drawing.Point(3, 3)
        Me.DatePick.Name = "DatePick"
        Me.DatePick.Size = New System.Drawing.Size(164, 20)
        Me.DatePick.TabIndex = 1
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 5
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.txtHr, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.VScrollHour, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.VScrollMin, 4, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.txtMin, 3, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Label2, 2, 0)
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(3, 29)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(164, 24)
        Me.TableLayoutPanel2.TabIndex = 0
        '
        'txtHr
        '
        Me.txtHr.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtHr.Location = New System.Drawing.Point(3, 3)
        Me.txtHr.Name = "txtHr"
        Me.txtHr.Size = New System.Drawing.Size(46, 20)
        Me.txtHr.TabIndex = 0
        Me.txtHr.Text = "00"
        Me.txtHr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'VScrollHour
        '
        Me.VScrollHour.Dock = System.Windows.Forms.DockStyle.Fill
        Me.VScrollHour.LargeChange = 1
        Me.VScrollHour.Location = New System.Drawing.Point(52, 0)
        Me.VScrollHour.Maximum = 23
        Me.VScrollHour.Name = "VScrollHour"
        Me.VScrollHour.Size = New System.Drawing.Size(20, 24)
        Me.VScrollHour.TabIndex = 2
        Me.VScrollHour.Value = 23
        '
        'VScrollMin
        '
        Me.VScrollMin.Dock = System.Windows.Forms.DockStyle.Fill
        Me.VScrollMin.LargeChange = 1
        Me.VScrollMin.Location = New System.Drawing.Point(144, 0)
        Me.VScrollMin.Maximum = 59
        Me.VScrollMin.Name = "VScrollMin"
        Me.VScrollMin.Size = New System.Drawing.Size(20, 24)
        Me.VScrollMin.TabIndex = 4
        Me.VScrollMin.Value = 59
        '
        'txtMin
        '
        Me.txtMin.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtMin.Location = New System.Drawing.Point(95, 3)
        Me.txtMin.Name = "txtMin"
        Me.txtMin.Size = New System.Drawing.Size(46, 20)
        Me.txtMin.TabIndex = 5
        Me.txtMin.Text = "00"
        Me.txtMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label2.Location = New System.Drawing.Point(75, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(14, 24)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = ":"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPriMin
        '
        Me.txtPriMin.AutoSize = True
        Me.txtPriMin.BackColor = System.Drawing.Color.White
        Me.txtPriMin.ContextMenuStrip = Me.ContextMenuStrip1
        Me.txtPriMin.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtPriMin.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPriMin.Location = New System.Drawing.Point(3, 56)
        Me.txtPriMin.Name = "txtPriMin"
        Me.txtPriMin.Size = New System.Drawing.Size(164, 34)
        Me.txtPriMin.TabIndex = 2
        Me.txtPriMin.Text = "000000000000"
        Me.txtPriMin.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CopyToolStripMenuItem, Me.PasteToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(103, 48)
        '
        'CopyToolStripMenuItem
        '
        Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(102, 22)
        Me.CopyToolStripMenuItem.Text = "Copy"
        '
        'PasteToolStripMenuItem
        '
        Me.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem"
        Me.PasteToolStripMenuItem.Size = New System.Drawing.Size(102, 22)
        Me.PasteToolStripMenuItem.Text = "Paste"
        '
        'frmCalc
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(170, 90)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCalc"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Priority Date Calculator"
        Me.TopMost = True
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents DatePick As System.Windows.Forms.DateTimePicker
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents txtPriMin As System.Windows.Forms.Label
    Friend WithEvents txtHr As System.Windows.Forms.TextBox
    Friend WithEvents VScrollHour As System.Windows.Forms.VScrollBar
    Friend WithEvents VScrollMin As System.Windows.Forms.VScrollBar
    Friend WithEvents txtMin As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PasteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
