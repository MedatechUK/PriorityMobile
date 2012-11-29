<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class priComboBox
    Inherits PriBaseCtrl

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
        Me.ComboBox = New System.Windows.Forms.ComboBox
        Me.SuspendLayout()
        '
        'ComboBox
        '
        Me.ComboBox.Dock = System.Windows.Forms.DockStyle.Right
        Me.ComboBox.Location = New System.Drawing.Point(50, 0)
        Me.ComboBox.Name = "ComboBox"
        Me.ComboBox.Size = New System.Drawing.Size(277, 23)
        Me.ComboBox.TabIndex = 3
        '
        'priComboBox
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.Controls.Add(Me.ComboBox)
        Me.Name = "priComboBox"
        Me.Controls.SetChildIndex(Me.ComboBox, 0)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ComboBox As System.Windows.Forms.ComboBox

End Class
