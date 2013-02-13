<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ctrl_Checks
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
        Me.Survey = New PriorityMobile.Survey
        Me.SuspendLayout()
        '
        'Survey
        '
        Me.Survey.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Survey.Location = New System.Drawing.Point(0, 0)
        Me.Survey.Name = "Survey"
        Me.Survey.Size = New System.Drawing.Size(150, 150)
        Me.Survey.TabIndex = 0
        '
        'ctrl_Checks
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.Survey)
        Me.Name = "ctrl_Checks"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Survey As PriorityMobile.Survey

End Class
