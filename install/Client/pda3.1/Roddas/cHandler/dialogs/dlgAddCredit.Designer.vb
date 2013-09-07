Imports PriorityMobile

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgAddCredit
    Inherits UserDialog

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
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.CreditQty = New System.Windows.Forms.NumericUpDown
        Me.rcvQty = New System.Windows.Forms.NumericUpDown
        Me.PartName = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.CreditReason = New System.Windows.Forms.ComboBox
        Me.Reason = New System.Windows.Forms.Label
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
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(23, 35)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 20)
        Me.Label1.Text = "Credit Quantity:"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(23, 84)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(167, 20)
        Me.Label2.Text = "Received Quantity:"
        '
        'CreditQty
        '
        Me.CreditQty.Location = New System.Drawing.Point(33, 55)
        Me.CreditQty.Name = "CreditQty"
        Me.CreditQty.Size = New System.Drawing.Size(100, 22)
        Me.CreditQty.TabIndex = 7
        Me.CreditQty.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'rcvQty
        '
        Me.rcvQty.Location = New System.Drawing.Point(33, 104)
        Me.rcvQty.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.rcvQty.Name = "rcvQty"
        Me.rcvQty.Size = New System.Drawing.Size(100, 22)
        Me.rcvQty.TabIndex = 8
        '
        'PartName
        '
        Me.PartName.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.PartName.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.PartName.Location = New System.Drawing.Point(146, 10)
        Me.PartName.Name = "PartName"
        Me.PartName.Size = New System.Drawing.Size(90, 20)
        Me.PartName.Text = "123456"
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label3.Location = New System.Drawing.Point(3, 10)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(136, 20)
        Me.Label3.Text = "Credit Note for Part#"
        '
        'CreditReason
        '
        Me.CreditReason.Items.Add("<--Please Select-->")
        Me.CreditReason.Location = New System.Drawing.Point(33, 161)
        Me.CreditReason.Name = "CreditReason"
        Me.CreditReason.Size = New System.Drawing.Size(192, 22)
        Me.CreditReason.TabIndex = 12
        '
        'Reason
        '
        Me.Reason.Location = New System.Drawing.Point(23, 138)
        Me.Reason.Name = "Reason"
        Me.Reason.Size = New System.Drawing.Size(167, 20)
        Me.Reason.Text = "Reason for Credit:"
        '
        'dlgAddCredit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.Reason)
        Me.Controls.Add(Me.CreditReason)
        Me.Controls.Add(Me.PartName)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.rcvQty)
        Me.Controls.Add(Me.CreditQty)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "dlgAddCredit"
        Me.Size = New System.Drawing.Size(240, 274)
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents CreditQty As System.Windows.Forms.NumericUpDown
    Friend WithEvents rcvQty As System.Windows.Forms.NumericUpDown
    Friend WithEvents PartName As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents CreditReason As System.Windows.Forms.ComboBox
    Friend WithEvents Reason As System.Windows.Forms.Label

End Class
