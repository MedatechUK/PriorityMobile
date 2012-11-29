<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ct_Sign
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
        Me.UsedParts = New System.Windows.Forms.ListView
        Me.SuspendLayout()
        '
        'UsedParts
        '
        Me.UsedParts.Dock = System.Windows.Forms.DockStyle.Top
        Me.UsedParts.FullRowSelect = True
        Me.UsedParts.Location = New System.Drawing.Point(0, 0)
        Me.UsedParts.Name = "UsedParts"
        Me.UsedParts.Size = New System.Drawing.Size(301, 97)
        Me.UsedParts.TabIndex = 5
        Me.UsedParts.View = System.Windows.Forms.View.Details
        '
        'ct_Sign
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.Controls.Add(Me.UsedParts)
        Me.Name = "ct_Sign"
        Me.Size = New System.Drawing.Size(301, 246)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents UsedParts As System.Windows.Forms.ListView

End Class
