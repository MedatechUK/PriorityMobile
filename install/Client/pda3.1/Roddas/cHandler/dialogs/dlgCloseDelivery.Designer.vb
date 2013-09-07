<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgCloseDelivery
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
        Me.lbl_Reason = New System.Windows.Forms.Label
        Me.Reason = New System.Windows.Forms.ComboBox
        Me.lbl_Warning = New System.Windows.Forms.Label
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
        'lbl_Reason
        '
        Me.lbl_Reason.Location = New System.Drawing.Point(0, 68)
        Me.lbl_Reason.Name = "lbl_Reason"
        Me.lbl_Reason.Size = New System.Drawing.Size(240, 20)
        Me.lbl_Reason.Text = "Non-Delivery Reason: "
        Me.lbl_Reason.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Reason
        '
        Me.Reason.Location = New System.Drawing.Point(30, 91)
        Me.Reason.Name = "Reason"
        Me.Reason.Size = New System.Drawing.Size(170, 23)
        Me.Reason.TabIndex = 8
        '
        'lbl_Warning
        '
        Me.lbl_Warning.Dock = System.Windows.Forms.DockStyle.Top
        Me.lbl_Warning.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lbl_Warning.ForeColor = System.Drawing.Color.Red
        Me.lbl_Warning.Location = New System.Drawing.Point(0, 0)
        Me.lbl_Warning.Name = "lbl_Warning"
        Me.lbl_Warning.Size = New System.Drawing.Size(240, 60)
        Me.lbl_Warning.Text = "Warning"
        Me.lbl_Warning.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'dlgCloseDelivery
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.lbl_Warning)
        Me.Controls.Add(Me.Reason)
        Me.Controls.Add(Me.lbl_Reason)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "dlgCloseDelivery"
        Me.Size = New System.Drawing.Size(240, 274)
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lbl_Reason As System.Windows.Forms.Label
    Friend WithEvents Reason As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_Warning As System.Windows.Forms.Label

End Class
