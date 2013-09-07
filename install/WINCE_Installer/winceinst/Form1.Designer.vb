<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class Form1
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
    private mainMenu1 As System.Windows.Forms.MainMenu

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.mainMenu1 = New System.Windows.Forms.MainMenu
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.MenuItem2 = New System.Windows.Forms.MenuItem
        Me.MenuItem3 = New System.Windows.Forms.MenuItem
        Me.MenuItem4 = New System.Windows.Forms.MenuItem
        Me.MenuItem5 = New System.Windows.Forms.MenuItem
        Me.MenuItem6 = New System.Windows.Forms.MenuItem
        Me.MenuItem7 = New System.Windows.Forms.MenuItem
        Me.lookup = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.lbl_DNS_Result = New System.Windows.Forms.Label
        Me.btn_Lookup = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.lbl_myip = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.MenuItem1)
        '
        'MenuItem1
        '
        Me.MenuItem1.MenuItems.Add(Me.MenuItem2)
        Me.MenuItem1.MenuItems.Add(Me.MenuItem5)
        Me.MenuItem1.Text = "File"
        '
        'MenuItem2
        '
        Me.MenuItem2.MenuItems.Add(Me.MenuItem3)
        Me.MenuItem2.MenuItems.Add(Me.MenuItem4)
        Me.MenuItem2.Text = "Debug"
        '
        'MenuItem3
        '
        Me.MenuItem3.Text = "ConmanClient2"
        '
        'MenuItem4
        '
        Me.MenuItem4.Text = "CMaccept"
        '
        'MenuItem5
        '
        Me.MenuItem5.MenuItems.Add(Me.MenuItem6)
        Me.MenuItem5.MenuItems.Add(Me.MenuItem7)
        Me.MenuItem5.Text = "Install"
        '
        'MenuItem6
        '
        Me.MenuItem6.Text = "cf.net"
        '
        'MenuItem7
        '
        Me.MenuItem7.Text = "Error Messages"
        '
        'lookup
        '
        Me.lookup.Location = New System.Drawing.Point(18, 135)
        Me.lookup.Name = "lookup"
        Me.lookup.Size = New System.Drawing.Size(179, 21)
        Me.lookup.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(15, 109)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(181, 23)
        Me.Label1.Text = "DNS Lookup"
        '
        'lbl_DNS_Result
        '
        Me.lbl_DNS_Result.Location = New System.Drawing.Point(21, 165)
        Me.lbl_DNS_Result.Name = "lbl_DNS_Result"
        Me.lbl_DNS_Result.Size = New System.Drawing.Size(174, 20)
        '
        'btn_Lookup
        '
        Me.btn_Lookup.Location = New System.Drawing.Point(203, 136)
        Me.btn_Lookup.Name = "btn_Lookup"
        Me.btn_Lookup.Size = New System.Drawing.Size(28, 20)
        Me.btn_Lookup.TabIndex = 3
        Me.btn_Lookup.Text = "Go"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(19, 28)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(149, 20)
        Me.Label2.Text = "My IP:"
        '
        'lbl_myip
        '
        Me.lbl_myip.Location = New System.Drawing.Point(21, 52)
        Me.lbl_myip.Name = "lbl_myip"
        Me.lbl_myip.Size = New System.Drawing.Size(176, 20)
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.lbl_myip)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btn_Lookup)
        Me.Controls.Add(Me.lbl_DNS_Result)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lookup)
        Me.Menu = Me.mainMenu1
        Me.Name = "Form1"
        Me.Text = "WINCE Installer"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem3 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem4 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem5 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem6 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem7 As System.Windows.Forms.MenuItem
    Friend WithEvents lookup As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lbl_DNS_Result As System.Windows.Forms.Label
    Friend WithEvents btn_Lookup As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lbl_myip As System.Windows.Forms.Label

End Class
