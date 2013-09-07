<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class Menu
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
        Me.Logo = New System.Windows.Forms.PictureBox
        Me.StatusBar = New System.Windows.Forms.StatusBar
        Me.SuspendLayout()
        '
        'Logo
        '
        Me.Logo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Logo.Location = New System.Drawing.Point(0, 0)
        Me.Logo.Name = "Logo"
        Me.Logo.Size = New System.Drawing.Size(638, 455)
        Me.Logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        '
        'StatusBar
        '
        Me.StatusBar.Location = New System.Drawing.Point(0, 431)
        Me.StatusBar.Name = "StatusBar"
        Me.StatusBar.Size = New System.Drawing.Size(638, 24)
        '
        'Menu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(638, 455)
        Me.ControlBox = False
        Me.Controls.Add(Me.StatusBar)
        Me.Controls.Add(Me.Logo)
        Me.MinimizeBox = False
        Me.Name = "Menu"
        Me.Text = "sfdc3"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Logo As System.Windows.Forms.PictureBox
    Friend WithEvents StatusBar As System.Windows.Forms.StatusBar

End Class
