<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class frmNumber
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

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Ct_number1 = New ceMyCls.ct_number
        Me.SuspendLayout()
        '
        'Ct_number1
        '
        Me.Ct_number1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Ct_number1.Location = New System.Drawing.Point(0, 0)
        Me.Ct_number1.Max = 0
        Me.Ct_number1.Name = "Ct_number1"
        Me.Ct_number1.Size = New System.Drawing.Size(638, 455)
        Me.Ct_number1.TabIndex = 0
        Me.Ct_number1.Value = 0
        '
        'frmNumber
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(638, 455)
        Me.Controls.Add(Me.Ct_number1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmNumber"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Ct_number1 As ceMyCls.ct_number
End Class
