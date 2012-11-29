<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ct_PartsUsed
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
        Me.pnl_Parts = New System.Windows.Forms.Panel
        Me.UsedParts = New System.Windows.Forms.ListView
        Me.Warehouse = New System.Windows.Forms.ListView
        Me.pnl_Number = New System.Windows.Forms.Panel
        Me.Ct_number1 = New Priority.ct_number
        Me.pnl_Parts.SuspendLayout()
        Me.pnl_Number.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnl_Parts
        '
        Me.pnl_Parts.Controls.Add(Me.UsedParts)
        Me.pnl_Parts.Controls.Add(Me.Warehouse)
        Me.pnl_Parts.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnl_Parts.Location = New System.Drawing.Point(0, 0)
        Me.pnl_Parts.Name = "pnl_Parts"
        Me.pnl_Parts.Size = New System.Drawing.Size(152, 205)
        Me.pnl_Parts.TabIndex = 0
        '
        'UsedParts
        '
        Me.UsedParts.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.UsedParts.FullRowSelect = True
        Me.UsedParts.Location = New System.Drawing.Point(0, 108)
        Me.UsedParts.Name = "UsedParts"
        Me.UsedParts.Size = New System.Drawing.Size(152, 97)
        Me.UsedParts.TabIndex = 3
        Me.UsedParts.UseCompatibleStateImageBehavior = False
        Me.UsedParts.View = System.Windows.Forms.View.Details
        '
        'Warehouse
        '
        Me.Warehouse.Dock = System.Windows.Forms.DockStyle.Top
        Me.Warehouse.FullRowSelect = True
        Me.Warehouse.Location = New System.Drawing.Point(0, 0)
        Me.Warehouse.Name = "Warehouse"
        Me.Warehouse.Size = New System.Drawing.Size(152, 97)
        Me.Warehouse.TabIndex = 2
        Me.Warehouse.UseCompatibleStateImageBehavior = False
        Me.Warehouse.View = System.Windows.Forms.View.Details
        '
        'pnl_Number
        '
        Me.pnl_Number.Controls.Add(Me.Ct_number1)
        Me.pnl_Number.Location = New System.Drawing.Point(197, 0)
        Me.pnl_Number.Name = "pnl_Number"
        Me.pnl_Number.Size = New System.Drawing.Size(150, 205)
        Me.pnl_Number.TabIndex = 2
        Me.pnl_Number.Visible = False
        '
        'Ct_number1
        '
        Me.Ct_number1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Ct_number1.Location = New System.Drawing.Point(0, 0)
        Me.Ct_number1.Max = 0
        Me.Ct_number1.Name = "Ct_number1"
        Me.Ct_number1.Size = New System.Drawing.Size(150, 205)
        Me.Ct_number1.TabIndex = 0
        Me.Ct_number1.Value = 0
        '
        'ct_PartsUsed
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.pnl_Number)
        Me.Controls.Add(Me.pnl_Parts)
        Me.Name = "ct_PartsUsed"
        Me.Size = New System.Drawing.Size(152, 205)
        Me.pnl_Parts.ResumeLayout(False)
        Me.pnl_Number.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnl_Parts As System.Windows.Forms.Panel
    Friend WithEvents UsedParts As System.Windows.Forms.ListView
    Friend WithEvents Warehouse As System.Windows.Forms.ListView
    Friend WithEvents pnl_Number As System.Windows.Forms.Panel
    Friend WithEvents Ct_number1 As Priority.ct_number

End Class
