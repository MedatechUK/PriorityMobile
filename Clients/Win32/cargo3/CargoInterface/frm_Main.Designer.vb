<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_Main
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frm_Main))
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.btn_New = New System.Windows.Forms.ToolStripButton
        Me.btn_Open = New System.Windows.Forms.ToolStripButton
        Me.btn_SaveAs = New System.Windows.Forms.ToolStripButton
        Me.btn_Properties = New System.Windows.Forms.ToolStripButton
        Me.btn_Scripts = New System.Windows.Forms.ToolStripButton
        Me.btn_Dbg = New System.Windows.Forms.ToolStripButton
        Me.btn_Run = New System.Windows.Forms.ToolStripButton
        Me.btn_Stop = New System.Windows.Forms.ToolStripButton
        Me.btn_Exit = New System.Windows.Forms.ToolStripButton
        Me.TableLayoutPanel1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        Me.OpenFileDialog1.Filter = "XML Files|*.xml|All files|*.*"
        Me.OpenFileDialog1.Title = "Open Trigger File"
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.FileName = "NewTriggerFile.xml"
        Me.SaveFileDialog1.Filter = "XML Files|*.xml|All files|*.*"
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.ToolStrip1, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(269, 30)
        Me.TableLayoutPanel1.TabIndex = 4
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btn_New, Me.btn_Open, Me.btn_SaveAs, Me.ToolStripSeparator1, Me.btn_Properties, Me.btn_Scripts, Me.btn_Dbg, Me.ToolStripSeparator2, Me.btn_Run, Me.btn_Stop, Me.ToolStripSeparator3, Me.btn_Exit})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(269, 30)
        Me.ToolStrip1.TabIndex = 3
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 30)
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 30)
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 30)
        '
        'btn_New
        '
        Me.btn_New.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_New.Image = Global.cargo3.My.Resources.Resources.PORTRAIT
        Me.btn_New.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_New.Name = "btn_New"
        Me.btn_New.Size = New System.Drawing.Size(23, 27)
        Me.btn_New.Text = "ToolStripButton1"
        Me.btn_New.ToolTipText = "New Trigger File"
        '
        'btn_Open
        '
        Me.btn_Open.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_Open.Image = Global.cargo3.My.Resources.Resources.OPENFOLD
        Me.btn_Open.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_Open.Name = "btn_Open"
        Me.btn_Open.Size = New System.Drawing.Size(23, 27)
        Me.btn_Open.Text = "ToolStripButton2"
        Me.btn_Open.ToolTipText = "Open Trigger File"
        '
        'btn_SaveAs
        '
        Me.btn_SaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_SaveAs.Enabled = False
        Me.btn_SaveAs.Image = Global.cargo3.My.Resources.Resources.DISKS04
        Me.btn_SaveAs.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_SaveAs.Name = "btn_SaveAs"
        Me.btn_SaveAs.Size = New System.Drawing.Size(23, 27)
        Me.btn_SaveAs.Text = "ToolStripButton1"
        Me.btn_SaveAs.ToolTipText = "Save Trigger File As"
        '
        'btn_Properties
        '
        Me.btn_Properties.CheckOnClick = True
        Me.btn_Properties.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_Properties.Enabled = False
        Me.btn_Properties.Image = Global.cargo3.My.Resources.Resources._Property
        Me.btn_Properties.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_Properties.Name = "btn_Properties"
        Me.btn_Properties.Size = New System.Drawing.Size(23, 27)
        Me.btn_Properties.Text = "ToolStripButton2"
        Me.btn_Properties.ToolTipText = "Trigger File Properties"
        '
        'btn_Scripts
        '
        Me.btn_Scripts.CheckOnClick = True
        Me.btn_Scripts.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_Scripts.Enabled = False
        Me.btn_Scripts.Image = Global.cargo3.My.Resources.Resources.MDICHILD
        Me.btn_Scripts.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_Scripts.Name = "btn_Scripts"
        Me.btn_Scripts.Size = New System.Drawing.Size(23, 27)
        Me.btn_Scripts.Text = "ToolStripButton3"
        Me.btn_Scripts.ToolTipText = "Trigger File Scripts"
        '
        'btn_Dbg
        '
        Me.btn_Dbg.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_Dbg.Image = Global.cargo3.My.Resources.Resources.W95MBX02
        Me.btn_Dbg.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_Dbg.Name = "btn_Dbg"
        Me.btn_Dbg.Size = New System.Drawing.Size(23, 27)
        Me.btn_Dbg.Text = "ToolStripButton1"
        Me.btn_Dbg.ToolTipText = "Show Debug"
        '
        'btn_Run
        '
        Me.btn_Run.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_Run.Enabled = False
        Me.btn_Run.Image = Global.cargo3.My.Resources.Resources.ARW01RT
        Me.btn_Run.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_Run.Name = "btn_Run"
        Me.btn_Run.Size = New System.Drawing.Size(23, 27)
        Me.btn_Run.Text = "ToolStripButton1"
        Me.btn_Run.ToolTipText = "Begin Execution"
        '
        'btn_Stop
        '
        Me.btn_Stop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_Stop.Enabled = False
        Me.btn_Stop.Image = Global.cargo3.My.Resources.Resources.MISC17B
        Me.btn_Stop.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_Stop.Name = "btn_Stop"
        Me.btn_Stop.Size = New System.Drawing.Size(23, 27)
        Me.btn_Stop.Text = "ToolStripButton2"
        Me.btn_Stop.ToolTipText = "Stop Execution"
        '
        'btn_Exit
        '
        Me.btn_Exit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_Exit.Image = Global.cargo3.My.Resources.Resources.W95MBX01
        Me.btn_Exit.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_Exit.Name = "btn_Exit"
        Me.btn_Exit.Size = New System.Drawing.Size(23, 27)
        Me.btn_Exit.Text = "ToolStripButton1"
        Me.btn_Exit.ToolTipText = "Exit"
        '
        'frm_Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(269, 30)
        Me.ControlBox = False
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frm_Main"
        Me.ShowIcon = False
        Me.TopMost = True
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents btn_New As System.Windows.Forms.ToolStripButton
    Friend WithEvents btn_Open As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btn_Properties As System.Windows.Forms.ToolStripButton
    Friend WithEvents btn_Scripts As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btn_Run As System.Windows.Forms.ToolStripButton
    Friend WithEvents btn_Stop As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btn_Exit As System.Windows.Forms.ToolStripButton
    Friend WithEvents btn_SaveAs As System.Windows.Forms.ToolStripButton
    Friend WithEvents btn_Dbg As System.Windows.Forms.ToolStripButton

End Class
