<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ct_Repair
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
        Me.lst_Malfunction = New System.Windows.Forms.ComboBox
        Me.lst_Resolution = New System.Windows.Forms.ComboBox
        Me.lbl_Malfunction = New System.Windows.Forms.Label
        Me.lbl_Resolution = New System.Windows.Forms.Label
        Me.txt_Repair = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'lst_Malfunction
        '
        Me.lst_Malfunction.FormattingEnabled = True
        Me.lst_Malfunction.Location = New System.Drawing.Point(94, 3)
        Me.lst_Malfunction.Name = "lst_Malfunction"
        Me.lst_Malfunction.Size = New System.Drawing.Size(152, 21)
        Me.lst_Malfunction.TabIndex = 0
        '
        'lst_Resolution
        '
        Me.lst_Resolution.FormattingEnabled = True
        Me.lst_Resolution.Location = New System.Drawing.Point(94, 27)
        Me.lst_Resolution.Name = "lst_Resolution"
        Me.lst_Resolution.Size = New System.Drawing.Size(152, 21)
        Me.lst_Resolution.TabIndex = 1
        '
        'lbl_Malfunction
        '
        Me.lbl_Malfunction.AutoSize = True
        Me.lbl_Malfunction.Location = New System.Drawing.Point(12, 6)
        Me.lbl_Malfunction.Name = "lbl_Malfunction"
        Me.lbl_Malfunction.Size = New System.Drawing.Size(65, 13)
        Me.lbl_Malfunction.TabIndex = 3
        Me.lbl_Malfunction.Text = "Malfunction:"
        '
        'lbl_Resolution
        '
        Me.lbl_Resolution.AutoSize = True
        Me.lbl_Resolution.Location = New System.Drawing.Point(17, 30)
        Me.lbl_Resolution.Name = "lbl_Resolution"
        Me.lbl_Resolution.Size = New System.Drawing.Size(60, 13)
        Me.lbl_Resolution.TabIndex = 4
        Me.lbl_Resolution.Text = "Resolution:"
        '
        'txt_Repair
        '
        Me.txt_Repair.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.txt_Repair.Location = New System.Drawing.Point(0, 54)
        Me.txt_Repair.Multiline = True
        Me.txt_Repair.Name = "txt_Repair"
        Me.txt_Repair.Size = New System.Drawing.Size(261, 208)
        Me.txt_Repair.TabIndex = 5
        '
        'ct_Repair
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.txt_Repair)
        Me.Controls.Add(Me.lbl_Resolution)
        Me.Controls.Add(Me.lbl_Malfunction)
        Me.Controls.Add(Me.lst_Resolution)
        Me.Controls.Add(Me.lst_Malfunction)
        Me.Name = "ct_Repair"
        Me.Size = New System.Drawing.Size(261, 262)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lst_Malfunction As System.Windows.Forms.ComboBox
    Friend WithEvents lst_Resolution As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_Malfunction As System.Windows.Forms.Label
    Friend WithEvents lbl_Resolution As System.Windows.Forms.Label
    Friend WithEvents txt_Repair As System.Windows.Forms.TextBox

End Class
