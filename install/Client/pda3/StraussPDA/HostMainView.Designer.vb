<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class HostMainView
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
    Private mainMenu1 As System.Windows.Forms.MainMenu

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.mainMenu1 = New System.Windows.Forms.MainMenu
        Me.MainView = New PriorityMobile.MainView
        Me.ContextMenu1 = New System.Windows.Forms.ContextMenu
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.SuspendLayout()
        '
        'MainView
        '
        Me.MainView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MainView.Location = New System.Drawing.Point(0, 0)
        Me.MainView.Name = "MainView"
        Me.MainView.Size = New System.Drawing.Size(240, 294)
        Me.MainView.TabIndex = 0
        '
        'ContextMenu1
        '
        Me.ContextMenu1.MenuItems.Add(Me.MenuItem1)
        '
        'MenuItem1
        '
        Me.MenuItem1.Text = "Close"
        '
        'HostMainView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 294)
        Me.ContextMenu = Me.ContextMenu1
        Me.ControlBox = False
        Me.Controls.Add(Me.MainView)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(0, 0)
        Me.Menu = Me.mainMenu1
        Me.MinimizeBox = False
        Me.Name = "HostMainView"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents MainView As PriorityMobile.MainView
    Friend WithEvents ContextMenu1 As System.Windows.Forms.ContextMenu
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
End Class
