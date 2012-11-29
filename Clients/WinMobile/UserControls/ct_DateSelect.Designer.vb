<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ct_DateSelect
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
        Me.DatePick = New System.Windows.Forms.DateTimePicker
        Me.CallList = New System.Windows.Forms.ListView
        Me.SuspendLayout()
        '
        'DatePick
        '
        Me.DatePick.Dock = System.Windows.Forms.DockStyle.Top
        Me.DatePick.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.DatePick.Location = New System.Drawing.Point(0, 0)
        Me.DatePick.Name = "DatePick"
        Me.DatePick.Size = New System.Drawing.Size(326, 29)
        Me.DatePick.TabIndex = 6
        '
        'CallList
        '
        Me.CallList.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.CallList.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular)
        Me.CallList.FullRowSelect = True
        Me.CallList.Location = New System.Drawing.Point(0, 35)
        Me.CallList.Name = "CallList"
        Me.CallList.Size = New System.Drawing.Size(326, 263)
        Me.CallList.TabIndex = 5
        Me.CallList.View = System.Windows.Forms.View.Details
        '
        'ct_DateSelect
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.DatePick)
        Me.Controls.Add(Me.CallList)
        Me.Name = "ct_DateSelect"
        Me.Size = New System.Drawing.Size(326, 298)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DatePick As System.Windows.Forms.DateTimePicker
    Friend WithEvents CallList As System.Windows.Forms.ListView

End Class
