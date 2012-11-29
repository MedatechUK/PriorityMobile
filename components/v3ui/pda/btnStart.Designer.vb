<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class btnStart
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.ProgressPanel = New System.Windows.Forms.Panel
        Me.ProgressBar = New System.Windows.Forms.ProgressBar
        Me.lbl_Detail = New System.Windows.Forms.StatusBar
        Me.ButtonPanel = New System.Windows.Forms.Panel
        Me.bttnStart = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lbl_Title = New System.Windows.Forms.StatusBar
        Me.ProgressPanel.SuspendLayout()
        Me.ButtonPanel.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ProgressPanel
        '
        Me.ProgressPanel.Controls.Add(Me.Panel1)
        Me.ProgressPanel.Controls.Add(Me.ProgressBar)
        Me.ProgressPanel.Controls.Add(Me.lbl_Detail)
        Me.ProgressPanel.Location = New System.Drawing.Point(166, 26)
        Me.ProgressPanel.Name = "ProgressPanel"
        Me.ProgressPanel.Size = New System.Drawing.Size(100, 55)
        '
        'ProgressBar
        '
        Me.ProgressBar.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ProgressBar.Location = New System.Drawing.Point(0, 0)
        Me.ProgressBar.Name = "ProgressBar"
        Me.ProgressBar.Size = New System.Drawing.Size(100, 31)
        '
        'lbl_Detail
        '
        Me.lbl_Detail.Location = New System.Drawing.Point(0, 31)
        Me.lbl_Detail.Name = "lbl_Detail"
        Me.lbl_Detail.Size = New System.Drawing.Size(100, 24)
        '
        'ButtonPanel
        '
        Me.ButtonPanel.Controls.Add(Me.bttnStart)
        Me.ButtonPanel.Location = New System.Drawing.Point(14, 26)
        Me.ButtonPanel.Name = "ButtonPanel"
        Me.ButtonPanel.Size = New System.Drawing.Size(100, 55)
        '
        'bttnStart
        '
        Me.bttnStart.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bttnStart.Enabled = False
        Me.bttnStart.Location = New System.Drawing.Point(0, 0)
        Me.bttnStart.Name = "bttnStart"
        Me.bttnStart.Size = New System.Drawing.Size(100, 55)
        Me.bttnStart.TabIndex = 10
        Me.bttnStart.Text = "Start"
        Me.bttnStart.Visible = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.lbl_Title)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(100, 24)
        '
        'lbl_Title
        '
        Me.lbl_Title.Location = New System.Drawing.Point(0, 0)
        Me.lbl_Title.Name = "lbl_Title"
        Me.lbl_Title.Size = New System.Drawing.Size(100, 24)
        '
        'btnStart
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.ProgressPanel)
        Me.Controls.Add(Me.ButtonPanel)
        Me.Name = "btnStart"
        Me.Size = New System.Drawing.Size(266, 81)
        Me.ProgressPanel.ResumeLayout(False)
        Me.ButtonPanel.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ProgressPanel As System.Windows.Forms.Panel
    Friend WithEvents lbl_Detail As System.Windows.Forms.StatusBar
    Friend WithEvents ProgressBar As System.Windows.Forms.ProgressBar
    Friend WithEvents ButtonPanel As System.Windows.Forms.Panel
    Public WithEvents bttnStart As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lbl_Title As System.Windows.Forms.StatusBar

End Class
