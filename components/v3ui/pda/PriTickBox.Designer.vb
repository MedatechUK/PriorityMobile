<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PriTickBox
    Inherits PriBaseCtrl

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
        Me.CheckBox = New System.Windows.Forms.CheckBox
        Me.SuspendLayout()
        '
        'CheckBox
        '
        Me.CheckBox.Location = New System.Drawing.Point(55, 3)
        Me.CheckBox.Name = "CheckBox"
        Me.CheckBox.Size = New System.Drawing.Size(24, 26)
        Me.CheckBox.TabIndex = 1
        '
        'PriTickBox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        'Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.CheckBox)
        Me.Name = "PriTickBox"
        Me.Size = New System.Drawing.Size(267, 23)
        Me.Controls.SetChildIndex(Me.CheckBox, 0)
        Me.ResumeLayout(False)
        'Me.PerformLayout()

    End Sub
    Friend WithEvents CheckBox As System.Windows.Forms.CheckBox

End Class
