<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class gtext
    Inherits System.Windows.Forms.UserControl

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
        Me.List = New System.Windows.Forms.ListView
        Me.SuspendLayout()
        '
        'List
        '
        Me.List.Dock = System.Windows.Forms.DockStyle.Fill
        Me.List.Location = New System.Drawing.Point(0, 0)
        Me.List.Name = "List"
        Me.List.Size = New System.Drawing.Size(150, 150)
        Me.List.TabIndex = 1        
        Me.List.View = System.Windows.Forms.View.Details
        '
        'gtext
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)        
        Me.Controls.Add(Me.List)
        Me.Name = "gtext"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents List As System.Windows.Forms.ListView

End Class
