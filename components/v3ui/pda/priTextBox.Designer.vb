<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class priTextBox
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
        Me.TextBox = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'TextBox
        '
        Me.TextBox.Dock = System.Windows.Forms.DockStyle.Right
        Me.TextBox.Location = New System.Drawing.Point(45, 0)
        Me.TextBox.Name = "TextBox"
        Me.TextBox.Size = New System.Drawing.Size(165, 23)
        Me.TextBox.TabIndex = 3
        '
        'priTextBox
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.Controls.Add(Me.TextBox)
        Me.Name = "priTextBox"
        Me.Size = New System.Drawing.Size(210, 22)
        Me.Controls.SetChildIndex(Me.TextBox, 0)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TextBox As System.Windows.Forms.TextBox

End Class
