<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgDeliver
    Inherits PriorityMobile.UserDialog

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
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.Lot = New System.Windows.Forms.ColumnHeader
        Me.Qty = New System.Windows.Forms.ColumnHeader
        Me.LotView = New System.Windows.Forms.ListView
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnOK)
        Me.Panel1.Controls.Add(Me.btnCancel)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 229)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(240, 45)
        '
        'btnOK
        '
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Dock = System.Windows.Forms.DockStyle.Left
        Me.btnOK.Enabled = False
        Me.btnOK.Location = New System.Drawing.Point(0, 0)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(120, 45)
        Me.btnOK.TabIndex = 8
        Me.btnOK.Text = "Ok"
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnCancel.Location = New System.Drawing.Point(127, 0)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(113, 45)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "Cancel"
        '
        'Lot
        '
        Me.Lot.Text = "Lot"
        Me.Lot.Width = 130
        '
        'Qty
        '
        Me.Qty.Text = "Qty"
        Me.Qty.Width = 50
        '
        'LotView
        '
        Me.LotView.Columns.Add(Me.Lot)
        Me.LotView.Columns.Add(Me.Qty)
        Me.LotView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LotView.FullRowSelect = True
        Me.LotView.Location = New System.Drawing.Point(0, 0)
        Me.LotView.Name = "LotView"
        Me.LotView.Size = New System.Drawing.Size(240, 229)
        Me.LotView.TabIndex = 3
        Me.LotView.View = System.Windows.Forms.View.Details
        '
        'dlgDeliver
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.LotView)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "dlgDeliver"
        Me.Size = New System.Drawing.Size(240, 274)
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Lot As System.Windows.Forms.ColumnHeader
    Friend WithEvents Qty As System.Windows.Forms.ColumnHeader
    Friend WithEvents LotView As System.Windows.Forms.ListView

End Class
