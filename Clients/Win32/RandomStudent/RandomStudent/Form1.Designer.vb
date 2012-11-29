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
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.RandomStudentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SettingsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PanelStudents = New System.Windows.Forms.Panel
        Me.txtStudent = New System.Windows.Forms.TextBox
        Me.PanelSettings = New System.Windows.Forms.Panel
        Me.Button1 = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblFlashDelay = New System.Windows.Forms.Label
        Me.tbFlashDelay = New System.Windows.Forms.TrackBar
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblRollDelay = New System.Windows.Forms.Label
        Me.tbRollDelay = New System.Windows.Forms.TrackBar
        Me.MenuStrip1.SuspendLayout()
        Me.PanelStudents.SuspendLayout()
        Me.PanelSettings.SuspendLayout()
        CType(Me.tbFlashDelay, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tbRollDelay, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RandomStudentToolStripMenuItem, Me.SettingsToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(284, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'RandomStudentToolStripMenuItem
        '
        Me.RandomStudentToolStripMenuItem.Name = "RandomStudentToolStripMenuItem"
        Me.RandomStudentToolStripMenuItem.Size = New System.Drawing.Size(108, 20)
        Me.RandomStudentToolStripMenuItem.Text = "Random Student"
        '
        'SettingsToolStripMenuItem
        '
        Me.SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem"
        Me.SettingsToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.SettingsToolStripMenuItem.Text = "Settings"
        '
        'PanelStudents
        '
        Me.PanelStudents.Controls.Add(Me.txtStudent)
        Me.PanelStudents.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelStudents.Location = New System.Drawing.Point(0, 24)
        Me.PanelStudents.Name = "PanelStudents"
        Me.PanelStudents.Size = New System.Drawing.Size(284, 238)
        Me.PanelStudents.TabIndex = 2
        '
        'txtStudent
        '
        Me.txtStudent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtStudent.Location = New System.Drawing.Point(0, 0)
        Me.txtStudent.Multiline = True
        Me.txtStudent.Name = "txtStudent"
        Me.txtStudent.Size = New System.Drawing.Size(284, 238)
        Me.txtStudent.TabIndex = 1
        Me.txtStudent.Text = "Simon" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Joanna" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Emilie" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Sophie"
        '
        'PanelSettings
        '
        Me.PanelSettings.Controls.Add(Me.Button1)
        Me.PanelSettings.Controls.Add(Me.Label1)
        Me.PanelSettings.Controls.Add(Me.lblFlashDelay)
        Me.PanelSettings.Controls.Add(Me.tbFlashDelay)
        Me.PanelSettings.Controls.Add(Me.Label2)
        Me.PanelSettings.Controls.Add(Me.lblRollDelay)
        Me.PanelSettings.Controls.Add(Me.tbRollDelay)
        Me.PanelSettings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelSettings.Location = New System.Drawing.Point(0, 24)
        Me.PanelSettings.Name = "PanelSettings"
        Me.PanelSettings.Size = New System.Drawing.Size(284, 238)
        Me.PanelSettings.TabIndex = 5
        Me.PanelSettings.Visible = False
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(179, 203)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 9
        Me.Button1.Text = "Ok"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(43, 95)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(65, 13)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Flash Delay:"
        '
        'lblFlashDelay
        '
        Me.lblFlashDelay.AutoSize = True
        Me.lblFlashDelay.Location = New System.Drawing.Point(83, 148)
        Me.lblFlashDelay.Name = "lblFlashDelay"
        Me.lblFlashDelay.Size = New System.Drawing.Size(39, 13)
        Me.lblFlashDelay.TabIndex = 7
        Me.lblFlashDelay.Text = "Label1"
        '
        'tbFlashDelay
        '
        Me.tbFlashDelay.Location = New System.Drawing.Point(30, 114)
        Me.tbFlashDelay.Maximum = 200
        Me.tbFlashDelay.Minimum = 10
        Me.tbFlashDelay.Name = "tbFlashDelay"
        Me.tbFlashDelay.Size = New System.Drawing.Size(233, 45)
        Me.tbFlashDelay.TabIndex = 6
        Me.tbFlashDelay.Value = 20
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(43, 21)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(58, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Roll Delay:"
        '
        'lblRollDelay
        '
        Me.lblRollDelay.AutoSize = True
        Me.lblRollDelay.Location = New System.Drawing.Point(83, 74)
        Me.lblRollDelay.Name = "lblRollDelay"
        Me.lblRollDelay.Size = New System.Drawing.Size(39, 13)
        Me.lblRollDelay.TabIndex = 1
        Me.lblRollDelay.Text = "Label1"
        '
        'tbRollDelay
        '
        Me.tbRollDelay.Location = New System.Drawing.Point(30, 40)
        Me.tbRollDelay.Maximum = 200
        Me.tbRollDelay.Minimum = 10
        Me.tbRollDelay.Name = "tbRollDelay"
        Me.tbRollDelay.Size = New System.Drawing.Size(233, 45)
        Me.tbRollDelay.TabIndex = 0
        Me.tbRollDelay.Value = 40
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 262)
        Me.Controls.Add(Me.PanelSettings)
        Me.Controls.Add(Me.PanelStudents)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.Text = "Random Student"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.PanelStudents.ResumeLayout(False)
        Me.PanelStudents.PerformLayout()
        Me.PanelSettings.ResumeLayout(False)
        Me.PanelSettings.PerformLayout()
        CType(Me.tbFlashDelay, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tbRollDelay, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents RandomStudentToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PanelStudents As System.Windows.Forms.Panel
    Friend WithEvents txtStudent As System.Windows.Forms.TextBox
    Friend WithEvents PanelSettings As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblFlashDelay As System.Windows.Forms.Label
    Friend WithEvents tbFlashDelay As System.Windows.Forms.TrackBar
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblRollDelay As System.Windows.Forms.Label
    Friend WithEvents tbRollDelay As System.Windows.Forms.TrackBar
    Friend WithEvents SettingsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Button1 As System.Windows.Forms.Button

End Class
