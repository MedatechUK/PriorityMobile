<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ct_Repair
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
        Me.txt_Repair = New System.Windows.Forms.TextBox
        Me.lbl_Resolution = New System.Windows.Forms.Label
        Me.lbl_Malfunction = New System.Windows.Forms.Label
        Me.lst_Resolution = New System.Windows.Forms.ComboBox
        Me.lst_Malfunction = New System.Windows.Forms.ComboBox
        Me.SuspendLayout()
        '
        'txt_Repair
        '
        Me.txt_Repair.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.txt_Repair.Location = New System.Drawing.Point(0, 74)
        Me.txt_Repair.Multiline = True
        Me.txt_Repair.Name = "txt_Repair"
        Me.txt_Repair.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txt_Repair.Size = New System.Drawing.Size(222, 208)
        Me.txt_Repair.TabIndex = 10
        '
        'lbl_Resolution
        '
        Me.lbl_Resolution.Location = New System.Drawing.Point(-31, 59)
        Me.lbl_Resolution.Name = "lbl_Resolution"
        Me.lbl_Resolution.Size = New System.Drawing.Size(60, 13)
        Me.lbl_Resolution.Text = "Resolution:"
        '
        'lbl_Malfunction
        '
        Me.lbl_Malfunction.Location = New System.Drawing.Point(-36, 35)
        Me.lbl_Malfunction.Name = "lbl_Malfunction"
        Me.lbl_Malfunction.Size = New System.Drawing.Size(65, 13)
        Me.lbl_Malfunction.Text = "Malfunction:"
        '
        'lst_Resolution
        '
        Me.lst_Resolution.Location = New System.Drawing.Point(46, 56)
        Me.lst_Resolution.Name = "lst_Resolution"
        Me.lst_Resolution.Size = New System.Drawing.Size(152, 22)
        Me.lst_Resolution.TabIndex = 9
        '
        'lst_Malfunction
        '
        Me.lst_Malfunction.Location = New System.Drawing.Point(46, 32)
        Me.lst_Malfunction.Name = "lst_Malfunction"
        Me.lst_Malfunction.Size = New System.Drawing.Size(152, 22)
        Me.lst_Malfunction.TabIndex = 8
        '
        'ct_Repair
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.txt_Repair)
        Me.Controls.Add(Me.lbl_Resolution)
        Me.Controls.Add(Me.lbl_Malfunction)
        Me.Controls.Add(Me.lst_Resolution)
        Me.Controls.Add(Me.lst_Malfunction)
        Me.Name = "ct_Repair"
        Me.Size = New System.Drawing.Size(222, 282)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txt_Repair As System.Windows.Forms.TextBox
    Friend WithEvents lbl_Resolution As System.Windows.Forms.Label
    Friend WithEvents lbl_Malfunction As System.Windows.Forms.Label
    Friend WithEvents lst_Resolution As System.Windows.Forms.ComboBox
    Friend WithEvents lst_Malfunction As System.Windows.Forms.ComboBox

End Class
