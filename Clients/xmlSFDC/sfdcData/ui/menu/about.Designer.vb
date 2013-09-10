<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class About
    Inherits PrioritySFDC.BaseForm

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
    Private mainMenu1 As System.Windows.Forms.MainMenu

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(About))
        Me.mainMenu1 = New System.Windows.Forms.MainMenu
        Me.MenuItem2 = New System.Windows.Forms.MenuItem
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.PanelAbout = New System.Windows.Forms.Panel
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.lbl_Handler_vers = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.lbl_sfdc_vers = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.PanelAbout.SuspendLayout()
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.MenuItem2)
        '
        'MenuItem2
        '
        Me.MenuItem2.Text = "Update"
        '
        'PictureBox1
        '
        Me.PictureBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(322, 187)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        '
        'PanelAbout
        '
        Me.PanelAbout.BackColor = System.Drawing.Color.White
        Me.PanelAbout.Controls.Add(Me.Label8)
        Me.PanelAbout.Controls.Add(Me.Label5)
        Me.PanelAbout.Controls.Add(Me.lbl_Handler_vers)
        Me.PanelAbout.Controls.Add(Me.Label4)
        Me.PanelAbout.Controls.Add(Me.lbl_sfdc_vers)
        Me.PanelAbout.Controls.Add(Me.Label1)
        Me.PanelAbout.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelAbout.Location = New System.Drawing.Point(0, 229)
        Me.PanelAbout.Name = "PanelAbout"
        Me.PanelAbout.Size = New System.Drawing.Size(322, 80)
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(8, 7)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(143, 20)
        Me.Label8.Text = "by Simon Barnett."
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Label5.Location = New System.Drawing.Point(8, 27)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(100, 20)
        Me.Label5.Text = "Version:"
        '
        'lbl_Handler_vers
        '
        Me.lbl_Handler_vers.Location = New System.Drawing.Point(68, 62)
        Me.lbl_Handler_vers.Name = "lbl_Handler_vers"
        Me.lbl_Handler_vers.Size = New System.Drawing.Size(100, 20)
        Me.lbl_Handler_vers.Text = "lbl_Handler_vers"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(8, 62)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(110, 20)
        Me.Label4.Text = "Handler:"
        '
        'lbl_sfdc_vers
        '
        Me.lbl_sfdc_vers.Location = New System.Drawing.Point(68, 44)
        Me.lbl_sfdc_vers.Name = "lbl_sfdc_vers"
        Me.lbl_sfdc_vers.Size = New System.Drawing.Size(100, 20)
        Me.lbl_sfdc_vers.Text = "lbl_sfdc_vers"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 44)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 20)
        Me.Label1.Text = "sfdc.net:"
        '
        'About
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(322, 309)
        Me.Controls.Add(Me.PanelAbout)
        Me.Controls.Add(Me.PictureBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Menu = Me.mainMenu1
        Me.MinimizeBox = False
        Me.Name = "About"
        Me.Text = "About SFDC"
        Me.PanelAbout.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents PanelAbout As System.Windows.Forms.Panel
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lbl_Handler_vers As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lbl_sfdc_vers As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
