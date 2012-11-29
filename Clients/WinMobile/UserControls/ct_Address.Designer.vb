<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ct_Address
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
        Me.lbl_Phone = New System.Windows.Forms.Label
        Me.lbl_PHONENUM = New System.Windows.Forms.Label
        Me.lbl_Cont = New System.Windows.Forms.Label
        Me.lbl_CONTACT = New System.Windows.Forms.Label
        Me.lbl_Customer = New System.Windows.Forms.Label
        Me.lbl_CUSTNAME = New System.Windows.Forms.Label
        Me.lbl_Address = New System.Windows.Forms.Label
        Me.Address = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'lbl_Phone
        '
        Me.lbl_Phone.Location = New System.Drawing.Point(20, 58)
        Me.lbl_Phone.Name = "lbl_Phone"
        Me.lbl_Phone.Size = New System.Drawing.Size(41, 13)
        Me.lbl_Phone.Text = "Phone:"
        '
        'lbl_PHONENUM
        '
        Me.lbl_PHONENUM.BackColor = System.Drawing.SystemColors.Control
        Me.lbl_PHONENUM.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lbl_PHONENUM.Location = New System.Drawing.Point(74, 59)
        Me.lbl_PHONENUM.Name = "lbl_PHONENUM"
        Me.lbl_PHONENUM.Size = New System.Drawing.Size(28, 13)
        Me.lbl_PHONENUM.Text = "123"
        '
        'lbl_Cont
        '
        Me.lbl_Cont.Location = New System.Drawing.Point(20, 35)
        Me.lbl_Cont.Name = "lbl_Cont"
        Me.lbl_Cont.Size = New System.Drawing.Size(47, 13)
        Me.lbl_Cont.Text = "Contact:"
        '
        'lbl_CONTACT
        '
        Me.lbl_CONTACT.BackColor = System.Drawing.SystemColors.Control
        Me.lbl_CONTACT.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lbl_CONTACT.Location = New System.Drawing.Point(74, 36)
        Me.lbl_CONTACT.Name = "lbl_CONTACT"
        Me.lbl_CONTACT.Size = New System.Drawing.Size(28, 13)
        Me.lbl_CONTACT.Text = "123"
        '
        'lbl_Customer
        '
        Me.lbl_Customer.Location = New System.Drawing.Point(14, 9)
        Me.lbl_Customer.Name = "lbl_Customer"
        Me.lbl_Customer.Size = New System.Drawing.Size(54, 13)
        Me.lbl_Customer.Text = "Customer:"
        '
        'lbl_CUSTNAME
        '
        Me.lbl_CUSTNAME.BackColor = System.Drawing.SystemColors.Control
        Me.lbl_CUSTNAME.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lbl_CUSTNAME.Location = New System.Drawing.Point(74, 10)
        Me.lbl_CUSTNAME.Name = "lbl_CUSTNAME"
        Me.lbl_CUSTNAME.Size = New System.Drawing.Size(28, 13)
        Me.lbl_CUSTNAME.Text = "123"
        '
        'lbl_Address
        '
        Me.lbl_Address.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.lbl_Address.Location = New System.Drawing.Point(10, 102)
        Me.lbl_Address.Name = "lbl_Address"
        Me.lbl_Address.Size = New System.Drawing.Size(48, 13)
        Me.lbl_Address.Text = "Address:"
        '
        'Address
        '
        Me.Address.Location = New System.Drawing.Point(0, 118)
        Me.Address.Multiline = True
        Me.Address.Name = "Address"
        Me.Address.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.Address.Size = New System.Drawing.Size(280, 133)
        Me.Address.TabIndex = 8
        '
        'ct_Address
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.Controls.Add(Me.Address)
        Me.Controls.Add(Me.lbl_Phone)
        Me.Controls.Add(Me.lbl_PHONENUM)
        Me.Controls.Add(Me.lbl_Cont)
        Me.Controls.Add(Me.lbl_CONTACT)
        Me.Controls.Add(Me.lbl_Customer)
        Me.Controls.Add(Me.lbl_CUSTNAME)
        Me.Controls.Add(Me.lbl_Address)
        Me.Name = "ct_Address"
        Me.Size = New System.Drawing.Size(280, 251)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lbl_Phone As System.Windows.Forms.Label
    Friend WithEvents lbl_PHONENUM As System.Windows.Forms.Label
    Friend WithEvents lbl_Cont As System.Windows.Forms.Label
    Friend WithEvents lbl_CONTACT As System.Windows.Forms.Label
    Friend WithEvents lbl_Customer As System.Windows.Forms.Label
    Friend WithEvents lbl_CUSTNAME As System.Windows.Forms.Label
    Friend WithEvents lbl_Address As System.Windows.Forms.Label
    Friend WithEvents Address As System.Windows.Forms.TextBox

End Class
