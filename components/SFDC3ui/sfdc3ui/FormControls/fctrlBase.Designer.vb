<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class fctrlBase
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
        Me.ColumnLabel = New System.Windows.Forms.LinkLabel
        Me.SuspendLayout()
        '
        'ColumnLabel
        '
        Me.ColumnLabel.Font = New System.Drawing.Font("Tahoma", 13.0!, System.Drawing.FontStyle.Regular)
        Me.ColumnLabel.Location = New System.Drawing.Point(0, 0)
        Me.ColumnLabel.Name = "ColumnLabel"
        Me.ColumnLabel.Size = New System.Drawing.Size(100, 20)
        Me.ColumnLabel.TabIndex = 0
        Me.ColumnLabel.Text = "LinkLabel1"
        '
        'fctrlBase
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.ColumnLabel)
        Me.Name = "fctrlBase"
        Me.Size = New System.Drawing.Size(244, 24)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ColumnLabel As System.Windows.Forms.LinkLabel

End Class
