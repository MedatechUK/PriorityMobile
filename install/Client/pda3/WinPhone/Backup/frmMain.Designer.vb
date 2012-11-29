<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class frmMain
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
        Me.MainMenu1 = New System.Windows.Forms.MainMenu
        Me.frmPanel = New System.Windows.Forms.Panel
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.ContentPanel = New System.Windows.Forms.Panel
        Me.mnu_SubForms = New PriorityMobile.SlideMenu
        Me.ToolStrip = New PriorityMobile.daToolbar
        Me.mnu_TopForms = New PriorityMobile.SlideMenu
        Me.frmPanel.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'frmPanel
        '
        Me.frmPanel.Controls.Add(Me.Panel1)
        Me.frmPanel.Controls.Add(Me.ToolStrip)
        Me.frmPanel.Controls.Add(Me.mnu_TopForms)
        Me.frmPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.frmPanel.Location = New System.Drawing.Point(0, 0)
        Me.frmPanel.Name = "frmPanel"
        Me.frmPanel.Size = New System.Drawing.Size(240, 294)
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.ContentPanel)
        Me.Panel1.Controls.Add(Me.mnu_SubForms)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 26)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(240, 246)
        '
        'ContentPanel
        '
        Me.ContentPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ContentPanel.Location = New System.Drawing.Point(0, 0)
        Me.ContentPanel.Name = "ContentPanel"
        Me.ContentPanel.Size = New System.Drawing.Size(240, 220)
        '
        'mnu_SubForms
        '
        Me.mnu_SubForms.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.mnu_SubForms.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.mnu_SubForms.ForeColor = System.Drawing.Color.Black
        Me.mnu_SubForms.Location = New System.Drawing.Point(0, 220)
        Me.mnu_SubForms.Name = "mnu_SubForms"
        Me.mnu_SubForms.Size = New System.Drawing.Size(240, 26)
        Me.mnu_SubForms.TabIndex = 2
        '
        'ToolStrip
        '
        Me.ToolStrip.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.ToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ToolStrip.ForeColor = System.Drawing.Color.FromArgb(CType(CType(186, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(251, Byte), Integer))
        Me.ToolStrip.IconFolder = ""
        Me.ToolStrip.Location = New System.Drawing.Point(0, 272)
        Me.ToolStrip.Name = "ToolStrip"
        Me.ToolStrip.Size = New System.Drawing.Size(240, 22)
        Me.ToolStrip.TabIndex = 3
        '
        'mnu_TopForms
        '
        Me.mnu_TopForms.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.mnu_TopForms.Dock = System.Windows.Forms.DockStyle.Top
        Me.mnu_TopForms.ForeColor = System.Drawing.Color.Black
        Me.mnu_TopForms.Location = New System.Drawing.Point(0, 0)
        Me.mnu_TopForms.Name = "mnu_TopForms"
        Me.mnu_TopForms.Size = New System.Drawing.Size(240, 26)
        Me.mnu_TopForms.TabIndex = 1
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 294)
        Me.Controls.Add(Me.frmPanel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(0, 0)
        Me.Menu = Me.MainMenu1
        Me.MinimizeBox = False
        Me.Name = "frmMain"
        Me.frmPanel.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents frmPanel As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ContentPanel As System.Windows.Forms.Panel
    Friend WithEvents mnu_SubForms As PriorityMobile.SlideMenu
    Friend WithEvents ToolStrip As PriorityMobile.daToolbar
    Friend WithEvents mnu_TopForms As PriorityMobile.SlideMenu
End Class
