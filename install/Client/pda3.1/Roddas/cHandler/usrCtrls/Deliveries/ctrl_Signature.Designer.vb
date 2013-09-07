<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ctrl_Signature
    Inherits PriorityMobile.iView

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
        Me.Signature = New PriorityMobile.Sign
        Me.txt_PrintName = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'Signature
        '
        Me.Signature.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Signature.Location = New System.Drawing.Point(0, 21)
        Me.Signature.Name = "Signature"
        Me.Signature.Size = New System.Drawing.Size(150, 129)
        Me.Signature.TabIndex = 3
        '
        'txt_PrintName
        '
        Me.txt_PrintName.Dock = System.Windows.Forms.DockStyle.Top
        Me.txt_PrintName.Location = New System.Drawing.Point(0, 0)
        Me.txt_PrintName.Name = "txt_PrintName"
        Me.txt_PrintName.Size = New System.Drawing.Size(150, 21)
        Me.txt_PrintName.TabIndex = 2
        '
        'ctrl_Signature
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.Signature)
        Me.Controls.Add(Me.txt_PrintName)
        Me.Name = "ctrl_Signature"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Signature As PriorityMobile.Sign
    Friend WithEvents txt_PrintName As System.Windows.Forms.TextBox

End Class
