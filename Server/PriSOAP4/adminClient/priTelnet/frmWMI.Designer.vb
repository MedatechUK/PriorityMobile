<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWMI
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.btnTest = New System.Windows.Forms.Button
        Me.ProgressBar = New System.Windows.Forms.ProgressBar
        Me.wmiResult = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'btnTest
        '
        Me.btnTest.Location = New System.Drawing.Point(50, 16)
        Me.btnTest.Name = "btnTest"
        Me.btnTest.Size = New System.Drawing.Size(218, 23)
        Me.btnTest.TabIndex = 1
        Me.btnTest.Text = "Test WMI Event Handler"
        Me.btnTest.UseVisualStyleBackColor = True
        '
        'ProgressBar
        '
        Me.ProgressBar.Enabled = False
        Me.ProgressBar.Location = New System.Drawing.Point(14, 52)
        Me.ProgressBar.MarqueeAnimationSpeed = 50
        Me.ProgressBar.Name = "ProgressBar"
        Me.ProgressBar.Size = New System.Drawing.Size(300, 15)
        Me.ProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.ProgressBar.TabIndex = 2
        '
        'wmiResult
        '
        Me.wmiResult.AutoSize = True
        Me.wmiResult.Location = New System.Drawing.Point(14, 76)
        Me.wmiResult.MinimumSize = New System.Drawing.Size(300, 0)
        Me.wmiResult.Name = "wmiResult"
        Me.wmiResult.Size = New System.Drawing.Size(300, 13)
        Me.wmiResult.TabIndex = 3
        Me.wmiResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmWMI
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(325, 112)
        Me.Controls.Add(Me.wmiResult)
        Me.Controls.Add(Me.ProgressBar)
        Me.Controls.Add(Me.btnTest)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmWMI"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Test WMI Events"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnTest As System.Windows.Forms.Button
    Friend WithEvents ProgressBar As System.Windows.Forms.ProgressBar
    Friend WithEvents wmiResult As System.Windows.Forms.Label

End Class
