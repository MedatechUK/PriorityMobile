<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NSLookup
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(NSLookup))
        Me.txtHost = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtIP = New System.Windows.Forms.Label
        Me.btn_copy = New System.Windows.Forms.Button
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.btn_lookup = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'txtHost
        '
        Me.txtHost.Location = New System.Drawing.Point(69, 12)
        Me.txtHost.Name = "txtHost"
        Me.txtHost.Size = New System.Drawing.Size(169, 20)
        Me.txtHost.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(11, 46)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(45, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Address"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(11, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(29, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Host"
        '
        'txtIP
        '
        Me.txtIP.AutoSize = True
        Me.txtIP.Location = New System.Drawing.Point(66, 46)
        Me.txtIP.Name = "txtIP"
        Me.txtIP.Size = New System.Drawing.Size(40, 13)
        Me.txtIP.TabIndex = 4
        Me.txtIP.Text = "0.0.0.0"
        '
        'btn_copy
        '
        Me.btn_copy.Enabled = False
        Me.btn_copy.ImageIndex = 0
        Me.btn_copy.ImageList = Me.ImageList1
        Me.btn_copy.Location = New System.Drawing.Point(248, 42)
        Me.btn_copy.Margin = New System.Windows.Forms.Padding(0)
        Me.btn_copy.Name = "btn_copy"
        Me.btn_copy.Size = New System.Drawing.Size(22, 22)
        Me.btn_copy.TabIndex = 5
        Me.btn_copy.UseVisualStyleBackColor = True
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "COPY.BMP")
        Me.ImageList1.Images.SetKeyName(1, "FIND.BMP")
        '
        'btn_lookup
        '
        Me.btn_lookup.ImageIndex = 1
        Me.btn_lookup.ImageList = Me.ImageList1
        Me.btn_lookup.Location = New System.Drawing.Point(248, 11)
        Me.btn_lookup.Margin = New System.Windows.Forms.Padding(0)
        Me.btn_lookup.Name = "btn_lookup"
        Me.btn_lookup.Size = New System.Drawing.Size(22, 22)
        Me.btn_lookup.TabIndex = 6
        Me.btn_lookup.UseVisualStyleBackColor = True
        '
        'NSLookup
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(285, 81)
        Me.Controls.Add(Me.btn_lookup)
        Me.Controls.Add(Me.btn_copy)
        Me.Controls.Add(Me.txtIP)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtHost)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "NSLookup"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "NS Lookup"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtHost As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtIP As System.Windows.Forms.Label
    Friend WithEvents btn_copy As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents btn_lookup As System.Windows.Forms.Button

End Class
