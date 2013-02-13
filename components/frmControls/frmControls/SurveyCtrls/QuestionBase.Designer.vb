<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Friend Class QuestionBase
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
        Me.QuestionText = New System.Windows.Forms.Label
        Me.ResponsePanel = New System.Windows.Forms.Panel
        Me.SuspendLayout()
        '
        'QuestionText
        '
        Me.QuestionText.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.QuestionText.Location = New System.Drawing.Point(2, 1)
        Me.QuestionText.Name = "QuestionText"
        Me.QuestionText.Size = New System.Drawing.Size(145, 73)
        Me.QuestionText.Text = "123"
        '
        'ResponsePanel
        '
        Me.ResponsePanel.BackColor = System.Drawing.SystemColors.Control
        Me.ResponsePanel.Location = New System.Drawing.Point(3, 76)
        Me.ResponsePanel.Name = "ResponsePanel"
        Me.ResponsePanel.Size = New System.Drawing.Size(144, 24)
        '
        'QuestionBase
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.ResponsePanel)
        Me.Controls.Add(Me.QuestionText)
        Me.Name = "QuestionBase"
        Me.Size = New System.Drawing.Size(150, 106)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents QuestionText As System.Windows.Forms.Label
    Friend WithEvents ResponsePanel As System.Windows.Forms.Panel

End Class
