<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCopies
    Inherits PrioritySFDC.BaseForm

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
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOK = New System.Windows.Forms.Button
        Me.copiesPanel = New System.Windows.Forms.Panel
        Me.lblCopies = New System.Windows.Forms.Label
        Me.Copies = New System.Windows.Forms.NumericUpDown
        Me.Panel1.SuspendLayout()
        Me.copiesPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.Controls.Add(Me.btnCancel)
        Me.Panel1.Controls.Add(Me.btnOK)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 405)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(638, 50)
        '
        'btnCancel
        '
        Me.btnCancel.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnCancel.Location = New System.Drawing.Point(566, 0)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(72, 50)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        '
        'btnOK
        '
        Me.btnOK.Dock = System.Windows.Forms.DockStyle.Left
        Me.btnOK.Location = New System.Drawing.Point(0, 0)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(72, 50)
        Me.btnOK.TabIndex = 0
        Me.btnOK.Text = "Ok"
        '
        'copiesPanel
        '
        Me.copiesPanel.BackColor = System.Drawing.Color.White
        Me.copiesPanel.Controls.Add(Me.lblCopies)
        Me.copiesPanel.Controls.Add(Me.Copies)
        Me.copiesPanel.Location = New System.Drawing.Point(3, 3)
        Me.copiesPanel.Name = "copiesPanel"
        Me.copiesPanel.Size = New System.Drawing.Size(206, 29)
        '
        'lblCopies
        '
        Me.lblCopies.Dock = System.Windows.Forms.DockStyle.Left
        Me.lblCopies.Location = New System.Drawing.Point(0, 0)
        Me.lblCopies.Name = "lblCopies"
        Me.lblCopies.Size = New System.Drawing.Size(100, 29)
        Me.lblCopies.Text = "# Copies:"
        Me.lblCopies.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Copies
        '
        Me.Copies.Dock = System.Windows.Forms.DockStyle.Right
        Me.Copies.Location = New System.Drawing.Point(106, 0)
        Me.Copies.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.Copies.Name = "Copies"
        Me.Copies.Size = New System.Drawing.Size(100, 24)
        Me.Copies.TabIndex = 4
        Me.Copies.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'frmCopies
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(638, 455)
        Me.Controls.Add(Me.copiesPanel)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "frmCopies"
        Me.Panel1.ResumeLayout(False)
        Me.copiesPanel.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents copiesPanel As System.Windows.Forms.Panel
    Friend WithEvents lblCopies As System.Windows.Forms.Label
    Friend WithEvents Copies As System.Windows.Forms.NumericUpDown

End Class
