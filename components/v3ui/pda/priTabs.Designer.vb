<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class priTabs
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.TabForm = New System.Windows.Forms.Panel
        Me.tabs = New uiCtrls.SlideMenu
        Me.SuspendLayout()
        '
        'TabForm
        '
        Me.TabForm.BackColor = System.Drawing.Color.White
        Me.TabForm.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.TabForm.Location = New System.Drawing.Point(0, 16)
        Me.TabForm.Name = "TabForm"
        Me.TabForm.Size = New System.Drawing.Size(150, 134)
        '
        'tabs
        '
        Me.tabs.backcolor = System.Drawing.Color.SkyBlue
        Me.tabs.Dock = System.Windows.Forms.DockStyle.Top
        Me.tabs.Location = New System.Drawing.Point(0, 0)
        Me.tabs.MenuFont = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Regular)
        Me.tabs.Name = "tabs"
        Me.tabs.Size = New System.Drawing.Size(150, 18)
        Me.tabs.TabIndex = 1
        '
        'priTabs
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.tabs)
        Me.Controls.Add(Me.TabForm)
        Me.Name = "priTabs"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabForm As System.Windows.Forms.Panel
    Friend WithEvents tabs As uiCtrls.SlideMenu

End Class
