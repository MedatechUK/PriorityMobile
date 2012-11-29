<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctrl_Summary
    Inherits ViewControl.iView

    'Form overrides dispose to clean up the component list.
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
        Me.DateTimePicker = New System.Windows.Forms.DateTimePicker
        Me.ListView = New System.Windows.Forms.ListView
        Me.SuspendLayout()
        '
        'DateTimePicker
        '
        Me.DateTimePicker.Dock = System.Windows.Forms.DockStyle.Top
        Me.DateTimePicker.Location = New System.Drawing.Point(0, 0)
        Me.DateTimePicker.Name = "DateTimePicker"
        Me.DateTimePicker.Size = New System.Drawing.Size(150, 20)
        Me.DateTimePicker.TabIndex = 0
        '
        'ListView
        '
        Me.ListView.AllowColumnReorder = True
        Me.ListView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView.FullRowSelect = True
        Me.ListView.Location = New System.Drawing.Point(0, 20)
        Me.ListView.Name = "ListView"
        Me.ListView.Size = New System.Drawing.Size(150, 130)
        Me.ListView.TabIndex = 1
        Me.ListView.UseCompatibleStateImageBehavior = False
        Me.ListView.View = System.Windows.Forms.View.Details
        '
        'ctrl_Summary
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.Controls.Add(Me.ListView)
        Me.Controls.Add(Me.DateTimePicker)
        Me.Name = "ctrl_Summary"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DateTimePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents ListView As System.Windows.Forms.ListView

End Class
