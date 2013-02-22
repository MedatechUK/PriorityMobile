<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ctrl_Payment
    Inherits PriorityMobile.iView

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
        Me.paymentterms = New System.Windows.Forms.TextBox
        Me.Panel11 = New System.Windows.Forms.Panel
        Me.Panel6 = New System.Windows.Forms.Panel
        Me.cheque = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.cash = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.todaysinvoicetotals = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.dueamount = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.overduepayment = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label1 = New System.Windows.Forms.Label
        Me.Panel9 = New System.Windows.Forms.Panel
        Me.Panel12 = New System.Windows.Forms.Panel
        Me.Panel14 = New System.Windows.Forms.Panel
        Me.Panel13 = New System.Windows.Forms.Panel
        Me.Panel10 = New System.Windows.Forms.Panel
        Me.Panel11.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel9.SuspendLayout()
        Me.Panel12.SuspendLayout()
        Me.Panel14.SuspendLayout()
        Me.SuspendLayout()
        '
        'paymentterms
        '
        Me.paymentterms.Dock = System.Windows.Forms.DockStyle.Fill
        Me.paymentterms.Enabled = False
        Me.paymentterms.Location = New System.Drawing.Point(104, 0)
        Me.paymentterms.Name = "paymentterms"
        Me.paymentterms.Size = New System.Drawing.Size(136, 21)
        Me.paymentterms.TabIndex = 2
        '
        'Panel11
        '
        Me.Panel11.AutoScroll = True
        Me.Panel11.Controls.Add(Me.Panel6)
        Me.Panel11.Controls.Add(Me.Panel2)
        Me.Panel11.Controls.Add(Me.Panel5)
        Me.Panel11.Controls.Add(Me.Panel4)
        Me.Panel11.Controls.Add(Me.Panel3)
        Me.Panel11.Controls.Add(Me.Panel1)
        Me.Panel11.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel11.Location = New System.Drawing.Point(0, 0)
        Me.Panel11.Name = "Panel11"
        Me.Panel11.Size = New System.Drawing.Size(240, 173)
        '
        'Panel6
        '
        Me.Panel6.Controls.Add(Me.cheque)
        Me.Panel6.Controls.Add(Me.Label5)
        Me.Panel6.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel6.Location = New System.Drawing.Point(0, 110)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(240, 22)
        '
        'cheque
        '
        Me.cheque.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cheque.Location = New System.Drawing.Point(104, 0)
        Me.cheque.Name = "cheque"
        Me.cheque.Size = New System.Drawing.Size(136, 21)
        Me.cheque.TabIndex = 8
        '
        'Label5
        '
        Me.Label5.Dock = System.Windows.Forms.DockStyle.Left
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Label5.ForeColor = System.Drawing.Color.Blue
        Me.Label5.Location = New System.Drawing.Point(0, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(104, 22)
        Me.Label5.Text = "Cheque: "
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.cash)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 88)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(240, 22)
        '
        'cash
        '
        Me.cash.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cash.Location = New System.Drawing.Point(104, 0)
        Me.cash.Name = "cash"
        Me.cash.Size = New System.Drawing.Size(136, 21)
        Me.cash.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Left
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(0, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(104, 22)
        Me.Label2.Text = "Cash: "
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.todaysinvoicetotals)
        Me.Panel5.Controls.Add(Me.Label8)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel5.Location = New System.Drawing.Point(0, 66)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(240, 22)
        '
        'todaysinvoicetotals
        '
        Me.todaysinvoicetotals.Dock = System.Windows.Forms.DockStyle.Fill
        Me.todaysinvoicetotals.Enabled = False
        Me.todaysinvoicetotals.Location = New System.Drawing.Point(104, 0)
        Me.todaysinvoicetotals.Name = "todaysinvoicetotals"
        Me.todaysinvoicetotals.Size = New System.Drawing.Size(136, 21)
        Me.todaysinvoicetotals.TabIndex = 2
        '
        'Label8
        '
        Me.Label8.Dock = System.Windows.Forms.DockStyle.Left
        Me.Label8.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Label8.ForeColor = System.Drawing.Color.Blue
        Me.Label8.Location = New System.Drawing.Point(0, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(104, 22)
        Me.Label8.Text = "Today: "
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.dueamount)
        Me.Panel4.Controls.Add(Me.Label4)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel4.Location = New System.Drawing.Point(0, 44)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(240, 22)
        '
        'dueamount
        '
        Me.dueamount.Dock = System.Windows.Forms.DockStyle.Top
        Me.dueamount.Enabled = False
        Me.dueamount.Location = New System.Drawing.Point(104, 0)
        Me.dueamount.Name = "dueamount"
        Me.dueamount.Size = New System.Drawing.Size(136, 21)
        Me.dueamount.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.Dock = System.Windows.Forms.DockStyle.Left
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(0, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(104, 22)
        Me.Label4.Text = "Due: "
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.overduepayment)
        Me.Panel3.Controls.Add(Me.Label3)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(0, 22)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(240, 22)
        '
        'overduepayment
        '
        Me.overduepayment.Dock = System.Windows.Forms.DockStyle.Fill
        Me.overduepayment.Enabled = False
        Me.overduepayment.Location = New System.Drawing.Point(104, 0)
        Me.overduepayment.Name = "overduepayment"
        Me.overduepayment.Size = New System.Drawing.Size(136, 21)
        Me.overduepayment.TabIndex = 8
        '
        'Label3
        '
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Left
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Label3.ForeColor = System.Drawing.Color.Blue
        Me.Label3.Location = New System.Drawing.Point(0, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(104, 22)
        Me.Label3.Text = "Overdue: "
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.paymentterms)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(240, 22)
        '
        'Label1
        '
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Left
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(104, 22)
        Me.Label1.Text = "Terms: "
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Panel9
        '
        Me.Panel9.Controls.Add(Me.Panel12)
        Me.Panel9.Controls.Add(Me.Panel10)
        Me.Panel9.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel9.Location = New System.Drawing.Point(0, 0)
        Me.Panel9.Name = "Panel9"
        Me.Panel9.Size = New System.Drawing.Size(245, 183)
        '
        'Panel12
        '
        Me.Panel12.Controls.Add(Me.Panel14)
        Me.Panel12.Controls.Add(Me.Panel13)
        Me.Panel12.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel12.Location = New System.Drawing.Point(0, 0)
        Me.Panel12.Name = "Panel12"
        Me.Panel12.Size = New System.Drawing.Size(240, 183)
        '
        'Panel14
        '
        Me.Panel14.Controls.Add(Me.Panel11)
        Me.Panel14.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel14.Location = New System.Drawing.Point(0, 10)
        Me.Panel14.Name = "Panel14"
        Me.Panel14.Size = New System.Drawing.Size(240, 173)
        '
        'Panel13
        '
        Me.Panel13.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel13.Location = New System.Drawing.Point(0, 0)
        Me.Panel13.Name = "Panel13"
        Me.Panel13.Size = New System.Drawing.Size(240, 10)
        '
        'Panel10
        '
        Me.Panel10.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel10.Location = New System.Drawing.Point(240, 0)
        Me.Panel10.Name = "Panel10"
        Me.Panel10.Size = New System.Drawing.Size(5, 183)
        '
        'ctrl_Payment
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.Panel9)
        Me.Name = "ctrl_Payment"
        Me.Size = New System.Drawing.Size(245, 183)
        Me.Panel11.ResumeLayout(False)
        Me.Panel6.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel5.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel9.ResumeLayout(False)
        Me.Panel12.ResumeLayout(False)
        Me.Panel14.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents paymentterms As System.Windows.Forms.TextBox
    Friend WithEvents Panel11 As System.Windows.Forms.Panel
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents todaysinvoicetotals As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents dueamount As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents overduepayment As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel9 As System.Windows.Forms.Panel
    Friend WithEvents Panel12 As System.Windows.Forms.Panel
    Friend WithEvents Panel14 As System.Windows.Forms.Panel
    Friend WithEvents Panel13 As System.Windows.Forms.Panel
    Friend WithEvents Panel10 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents cash As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Panel6 As System.Windows.Forms.Panel
    Friend WithEvents cheque As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label

End Class
