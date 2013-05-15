<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ListSort
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
        Me.View = New System.Windows.Forms.ListView
        Me.FormTitle = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'View
        '
        Me.View.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.View.FullRowSelect = True
        Me.View.Location = New System.Drawing.Point(0, 27)
        Me.View.Name = "View"
        Me.View.Size = New System.Drawing.Size(174, 140)
        Me.View.TabIndex = 0
        Me.View.View = System.Windows.Forms.View.Details
        '
        'FormTitle
        '
        Me.FormTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.FormTitle.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.FormTitle.ForeColor = System.Drawing.Color.Blue
        Me.FormTitle.Location = New System.Drawing.Point(0, 0)
        Me.FormTitle.Name = "FormTitle"
        Me.FormTitle.Size = New System.Drawing.Size(174, 20)
        '
        'ListSort
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.FormTitle)
        Me.Controls.Add(Me.View)
        Me.Name = "ListSort"
        Me.Size = New System.Drawing.Size(174, 167)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents View As System.Windows.Forms.ListView
    Friend WithEvents FormTitle As System.Windows.Forms.Label

End Class
