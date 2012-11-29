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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(priTabs))
        Me.Tabs = New uiCtrls.SlideMenu
        Me.TabForm = New System.Windows.Forms.Panel
        Me.SuspendLayout()
        '
        'Tabs
        '
        Me.Tabs.Dock = System.Windows.Forms.DockStyle.Top
        Me.Tabs.Location = New System.Drawing.Point(0, 0)
        Me.Tabs.MenuFont = New System.Drawing.Font("Tahoma", 10.0!)
        Me.Tabs.Name = "Tabs"
        Me.Tabs.Size = New System.Drawing.Size(150, 18)
        Me.Tabs.sMenuItems = CType(resources.GetObject("Tabs.sMenuItems"), System.Collections.Generic.Dictionary(Of Integer, System.Windows.Forms.LinkLabel))
        Me.Tabs.TabIndex = 0
        '
        'TabForm
        '
        Me.TabForm.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabForm.Location = New System.Drawing.Point(0, 18)
        Me.TabForm.Name = "TabForm"
        Me.TabForm.Size = New System.Drawing.Size(150, 132)
        Me.TabForm.TabIndex = 1
        '
        'priTabs
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.TabForm)
        Me.Controls.Add(Me.Tabs)
        Me.Name = "priTabs"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Tabs As uiCtrls.SlideMenu
    Friend WithEvents TabForm As System.Windows.Forms.Panel

End Class
