<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class CtrlList
    Inherits CtrlPartial

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
        Me.List = New System.Windows.Forms.ComboBox
        Me.SuspendLayout()
        '
        'List
        '
        Me.List.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular)
        Me.List.Location = New System.Drawing.Point(0, 0)
        Me.List.Name = "List"
        Me.List.Size = New System.Drawing.Size(140, 21)
        Me.List.TabIndex = 0
        '
        'CtrlList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.LightSteelBlue
        Me.Controls.Add(Me.List)
        Me.Name = "CtrlList"
        Me.Size = New System.Drawing.Size(140, 19)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents List As System.Windows.Forms.ComboBox

End Class
