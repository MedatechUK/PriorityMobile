<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class frmProvisionDialog
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
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.logo = New System.Windows.Forms.PictureBox
        Me.Panel_Text = New System.Windows.Forms.Panel
        Me.txtProvisionString = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.panel_reprovision = New System.Windows.Forms.Panel
        Me.txtServerName = New System.Windows.Forms.Label
        Me.txtUsername = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnReprovision = New System.Windows.Forms.LinkLabel
        Me.Panel_btnConnect = New System.Windows.Forms.Panel
        Me.btnConnect = New System.Windows.Forms.Button
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel_Text.SuspendLayout()
        Me.panel_reprovision.SuspendLayout()
        Me.Panel_btnConnect.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Controls.Add(Me.Panel_btnConnect)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(640, 480)
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.logo)
        Me.Panel2.Controls.Add(Me.Panel_Text)
        Me.Panel2.Controls.Add(Me.panel_reprovision)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(640, 435)
        '
        'logo
        '
        Me.logo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.logo.Location = New System.Drawing.Point(0, 0)
        Me.logo.Name = "logo"
        Me.logo.Size = New System.Drawing.Size(640, 335)
        Me.logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        '
        'Panel_Text
        '
        Me.Panel_Text.BackColor = System.Drawing.Color.White
        Me.Panel_Text.Controls.Add(Me.txtProvisionString)
        Me.Panel_Text.Controls.Add(Me.Label1)
        Me.Panel_Text.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel_Text.Location = New System.Drawing.Point(0, 335)
        Me.Panel_Text.Name = "Panel_Text"
        Me.Panel_Text.Size = New System.Drawing.Size(640, 40)
        '
        'txtProvisionString
        '
        Me.txtProvisionString.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtProvisionString.Location = New System.Drawing.Point(0, 20)
        Me.txtProvisionString.Name = "txtProvisionString"
        Me.txtProvisionString.Size = New System.Drawing.Size(640, 23)
        Me.txtProvisionString.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(640, 20)
        Me.Label1.Text = "Please scan or enter your provision code:"
        '
        'panel_reprovision
        '
        Me.panel_reprovision.BackColor = System.Drawing.Color.White
        Me.panel_reprovision.Controls.Add(Me.txtServerName)
        Me.panel_reprovision.Controls.Add(Me.txtUsername)
        Me.panel_reprovision.Controls.Add(Me.Label3)
        Me.panel_reprovision.Controls.Add(Me.Label2)
        Me.panel_reprovision.Controls.Add(Me.btnReprovision)
        Me.panel_reprovision.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.panel_reprovision.Location = New System.Drawing.Point(0, 375)
        Me.panel_reprovision.Name = "panel_reprovision"
        Me.panel_reprovision.Size = New System.Drawing.Size(640, 60)
        '
        'txtServerName
        '
        Me.txtServerName.Location = New System.Drawing.Point(50, 23)
        Me.txtServerName.Name = "txtServerName"
        Me.txtServerName.Size = New System.Drawing.Size(174, 20)
        Me.txtServerName.Text = "<ServerName>"
        '
        'txtUsername
        '
        Me.txtUsername.Location = New System.Drawing.Point(50, 4)
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(174, 20)
        Me.txtUsername.Text = "<Username>"
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label3.Location = New System.Drawing.Point(4, 23)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(47, 20)
        Me.Label3.Text = "Server:"
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label2.Location = New System.Drawing.Point(4, 4)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(47, 20)
        Me.Label2.Text = "User:"
        '
        'btnReprovision
        '
        Me.btnReprovision.Location = New System.Drawing.Point(163, 42)
        Me.btnReprovision.Name = "btnReprovision"
        Me.btnReprovision.Size = New System.Drawing.Size(71, 20)
        Me.btnReprovision.TabIndex = 99
        Me.btnReprovision.Text = "Reprovision"
        Me.btnReprovision.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Panel_btnConnect
        '
        Me.Panel_btnConnect.BackColor = System.Drawing.Color.White
        Me.Panel_btnConnect.Controls.Add(Me.btnConnect)
        Me.Panel_btnConnect.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel_btnConnect.Location = New System.Drawing.Point(0, 435)
        Me.Panel_btnConnect.Name = "Panel_btnConnect"
        Me.Panel_btnConnect.Size = New System.Drawing.Size(640, 45)
        '
        'btnConnect
        '
        Me.btnConnect.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.btnConnect.Location = New System.Drawing.Point(0, 10)
        Me.btnConnect.Name = "btnConnect"
        Me.btnConnect.Size = New System.Drawing.Size(640, 35)
        Me.btnConnect.TabIndex = 0
        Me.btnConnect.Text = "Connect"
        '
        'frmProvisionDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(640, 480)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.MinimizeBox = False
        Me.Name = "frmProvisionDialog"
        Me.Text = "Provision"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel_Text.ResumeLayout(False)
        Me.panel_reprovision.ResumeLayout(False)
        Me.Panel_btnConnect.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Panel_btnConnect As System.Windows.Forms.Panel
    Friend WithEvents btnConnect As System.Windows.Forms.Button
    Friend WithEvents logo As System.Windows.Forms.PictureBox
    Friend WithEvents Panel_Text As System.Windows.Forms.Panel
    Friend WithEvents txtProvisionString As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents panel_reprovision As System.Windows.Forms.Panel
    Friend WithEvents txtServerName As System.Windows.Forms.Label
    Friend WithEvents txtUsername As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnReprovision As System.Windows.Forms.LinkLabel
End Class
