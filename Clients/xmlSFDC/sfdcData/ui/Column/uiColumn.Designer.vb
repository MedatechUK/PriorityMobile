<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class uiColumn
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
        Me.Label = New System.Windows.Forms.LinkLabel
        Me.ContextMenu1 = New System.Windows.Forms.ContextMenu
        Me.Scan = New System.Windows.Forms.MenuItem
        Me.Scan2d = New System.Windows.Forms.MenuItem
        Me.lbl_Value = New System.Windows.Forms.Label
        Me.list = New System.Windows.Forms.ComboBox
        Me.SuspendLayout()
        '
        'Label
        '
        Me.Label.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label.ContextMenu = Me.ContextMenu1
        Me.Label.Font = New System.Drawing.Font("Tahoma", 10.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle))
        Me.Label.Location = New System.Drawing.Point(0, 0)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(100, 23)
        Me.Label.TabIndex = 1
        Me.Label.Text = "Link Text:"
        Me.Label.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'ContextMenu1
        '
        Me.ContextMenu1.MenuItems.Add(Me.Scan)
        Me.ContextMenu1.MenuItems.Add(Me.Scan2d)
        '
        'Scan
        '
        Me.Scan.Text = "Scan"
        '
        'Scan2d
        '
        Me.Scan2d.Text = "2d Scan"
        '
        'lbl_Value
        '
        Me.lbl_Value.Dock = System.Windows.Forms.DockStyle.Right
        Me.lbl_Value.Location = New System.Drawing.Point(103, 0)
        Me.lbl_Value.Name = "lbl_Value"
        Me.lbl_Value.Size = New System.Drawing.Size(173, 20)
        Me.lbl_Value.Text = "#Value"
        '
        'list
        '
        Me.list.Location = New System.Drawing.Point(137, 22)
        Me.list.Name = "list"
        Me.list.Size = New System.Drawing.Size(90, 23)
        Me.list.TabIndex = 3
        Me.list.Visible = False
        '
        'uiColumn
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.list)
        Me.Controls.Add(Me.lbl_Value)
        Me.Controls.Add(Me.Label)
        Me.Name = "uiColumn"
        Me.Size = New System.Drawing.Size(276, 20)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label As System.Windows.Forms.LinkLabel
    Friend WithEvents lbl_Value As System.Windows.Forms.Label
    Friend WithEvents list As System.Windows.Forms.ComboBox
    Friend WithEvents ContextMenu1 As System.Windows.Forms.ContextMenu
    Friend WithEvents Scan As System.Windows.Forms.MenuItem
    Friend WithEvents Scan2d As System.Windows.Forms.MenuItem

End Class
