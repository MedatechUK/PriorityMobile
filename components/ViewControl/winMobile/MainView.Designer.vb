<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class MainView
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
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.ContentPanel = New System.Windows.Forms.Panel
        Me.mnu_SubForms = New PriorityMobile.SlideMenu
        Me.ToolStrip = New PriorityMobile.daToolbar
        Me.frmPanel = New System.Windows.Forms.Panel
        Me.mnu_TopForms = New PriorityMobile.SlideMenu
        Me.ProgressBar = New System.Windows.Forms.ProgressBar
        Me.Panel1.SuspendLayout()
        Me.frmPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.ContentPanel)
        Me.Panel1.Controls.Add(Me.mnu_SubForms)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 26)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(270, 288)
        '
        'ContentPanel
        '
        Me.ContentPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ContentPanel.Location = New System.Drawing.Point(0, 0)
        Me.ContentPanel.Name = "ContentPanel"
        Me.ContentPanel.Size = New System.Drawing.Size(270, 262)
        '
        'mnu_SubForms
        '
        Me.mnu_SubForms.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.mnu_SubForms.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.mnu_SubForms.ForeColor = System.Drawing.Color.Black
        Me.mnu_SubForms.Location = New System.Drawing.Point(0, 262)
        Me.mnu_SubForms.Name = "mnu_SubForms"
        Me.mnu_SubForms.Size = New System.Drawing.Size(270, 26)
        Me.mnu_SubForms.TabIndex = 2
        '
        'ToolStrip
        '
        Me.ToolStrip.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.ToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ToolStrip.ForeColor = System.Drawing.Color.FromArgb(CType(CType(186, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(251, Byte), Integer))
        Me.ToolStrip.IconFolder = ""
        Me.ToolStrip.Location = New System.Drawing.Point(0, 314)
        Me.ToolStrip.Name = "ToolStrip"
        Me.ToolStrip.Size = New System.Drawing.Size(270, 22)
        Me.ToolStrip.TabIndex = 3
        '
        'frmPanel
        '
        Me.frmPanel.Controls.Add(Me.ProgressBar)
        Me.frmPanel.Controls.Add(Me.Panel1)
        Me.frmPanel.Controls.Add(Me.ToolStrip)
        Me.frmPanel.Controls.Add(Me.mnu_TopForms)
        Me.frmPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.frmPanel.Location = New System.Drawing.Point(0, 0)
        Me.frmPanel.Name = "frmPanel"
        Me.frmPanel.Size = New System.Drawing.Size(270, 336)
        '
        'mnu_TopForms
        '
        Me.mnu_TopForms.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.mnu_TopForms.Dock = System.Windows.Forms.DockStyle.Top
        Me.mnu_TopForms.ForeColor = System.Drawing.Color.Black
        Me.mnu_TopForms.Location = New System.Drawing.Point(0, 0)
        Me.mnu_TopForms.Name = "mnu_TopForms"
        Me.mnu_TopForms.Size = New System.Drawing.Size(270, 26)
        Me.mnu_TopForms.TabIndex = 1
        '
        'ProgressBar
        '
        Me.ProgressBar.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ProgressBar.Location = New System.Drawing.Point(0, 292)
        Me.ProgressBar.Name = "ProgressBar"
        Me.ProgressBar.Size = New System.Drawing.Size(270, 22)
        Me.ProgressBar.Visible = False
        '
        'MainView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.frmPanel)
        Me.Name = "MainView"
        Me.Size = New System.Drawing.Size(270, 336)
        Me.Panel1.ResumeLayout(False)
        Me.frmPanel.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents frmPanel As System.Windows.Forms.Panel
    Public WithEvents ContentPanel As System.Windows.Forms.Panel
    Public WithEvents mnu_SubForms As PriorityMobile.SlideMenu
    Public WithEvents ToolStrip As PriorityMobile.daToolbar
    Public WithEvents mnu_TopForms As PriorityMobile.SlideMenu
    Friend WithEvents ProgressBar As System.Windows.Forms.ProgressBar

End Class
