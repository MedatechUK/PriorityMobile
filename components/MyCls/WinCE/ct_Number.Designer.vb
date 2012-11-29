<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ct_number
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
        Me.txt_Number = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'txt_Number
        '
        Me.txt_Number.BackColor = System.Drawing.Color.White
        Me.txt_Number.Dock = System.Windows.Forms.DockStyle.Top
        Me.txt_Number.Location = New System.Drawing.Point(0, 0)
        Me.txt_Number.Name = "txt_Number"
        Me.txt_Number.ReadOnly = True
        Me.txt_Number.Size = New System.Drawing.Size(150, 23)
        Me.txt_Number.TabIndex = 2
        '
        'ct_number
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.txt_Number)
        Me.Name = "ct_number"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txt_Number As System.Windows.Forms.TextBox

End Class
