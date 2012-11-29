<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class priDateSel
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
        Me.DataGrid = New System.Windows.Forms.DataGrid
        Me.SuspendLayout()
        '
        'DatePick
        '
        Me.DatePick.CustomFormat = "dd/MM/yyyy"
        Me.DatePick.Dock = System.Windows.Forms.DockStyle.Top
        Me.DatePick.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DatePick.Location = New System.Drawing.Point(0, 0)
        Me.DatePick.Name = "DatePick"
        Me.DatePick.Size = New System.Drawing.Size(150, 24)
        Me.DatePick.TabIndex = 0
        '
        'DataGrid
        '
        Me.DataGrid.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.DataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGrid.Location = New System.Drawing.Point(0, 24)
        Me.DataGrid.Name = "DataGrid"
        Me.DataGrid.Size = New System.Drawing.Size(150, 126)
        Me.DataGrid.TabIndex = 1
        '
        'priDateSel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.DataGrid)
        Me.Controls.Add(Me.DatePick)
        Me.Name = "priDateSel"
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents DataGrid As System.Windows.Forms.DataGrid
    Public WithEvents DatePick As System.Windows.Forms.DateTimePicker

End Class
