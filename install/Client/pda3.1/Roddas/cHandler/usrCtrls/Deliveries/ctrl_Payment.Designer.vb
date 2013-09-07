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
        Me.lbl_Credit = New System.Windows.Forms.Label
        Me.pnl_cash = New System.Windows.Forms.Panel
        Me.radio_Cash = New System.Windows.Forms.RadioButton
        Me.cash = New System.Windows.Forms.TextBox
        Me.lbl_Cash = New System.Windows.Forms.Label
        Me.Panel11 = New System.Windows.Forms.Panel
        Me.pnl_Check = New System.Windows.Forms.Panel
        Me.radio_Cheque = New System.Windows.Forms.RadioButton
        Me.cheque = New System.Windows.Forms.TextBox
        Me.lbl_Check = New System.Windows.Forms.Label
        Me.pnl_ChqNum = New System.Windows.Forms.Panel
        Me.Banks = New System.Windows.Forms.ComboBox
        Me.ChqNum = New System.Windows.Forms.TextBox
        Me.lbl_chqNum = New System.Windows.Forms.Label
        Me.pnl_Unallocated = New System.Windows.Forms.Panel
        Me.inc_Unallocated = New System.Windows.Forms.CheckBox
        Me.Unallocated = New System.Windows.Forms.TextBox
        Me.lbl_Unallocated = New System.Windows.Forms.Label
        Me.pnl_Credit = New System.Windows.Forms.Panel
        Me.inc_Credit = New System.Windows.Forms.CheckBox
        Me.Credit = New System.Windows.Forms.TextBox
        Me.pnl_Today = New System.Windows.Forms.Panel
        Me.inc_Today = New System.Windows.Forms.CheckBox
        Me.todaysinvoicetotals = New System.Windows.Forms.TextBox
        Me.lbl_Today = New System.Windows.Forms.Label
        Me.pnl_Due = New System.Windows.Forms.Panel
        Me.inc_Due = New System.Windows.Forms.CheckBox
        Me.dueamount = New System.Windows.Forms.TextBox
        Me.lbl_Due = New System.Windows.Forms.Label
        Me.pnl_Overdue = New System.Windows.Forms.Panel
        Me.inc_Overdue = New System.Windows.Forms.CheckBox
        Me.overduepayment = New System.Windows.Forms.TextBox
        Me.lbl_Overdue = New System.Windows.Forms.Label
        Me.pnl_terms = New System.Windows.Forms.Panel
        Me.Label1 = New System.Windows.Forms.Label
        Me.Panel9 = New System.Windows.Forms.Panel
        Me.Panel12 = New System.Windows.Forms.Panel
        Me.Panel14 = New System.Windows.Forms.Panel
        Me.Panel13 = New System.Windows.Forms.Panel
        Me.Panel10 = New System.Windows.Forms.Panel
        Me.pnl_cash.SuspendLayout()
        Me.Panel11.SuspendLayout()
        Me.pnl_Check.SuspendLayout()
        Me.pnl_ChqNum.SuspendLayout()
        Me.pnl_Unallocated.SuspendLayout()
        Me.pnl_Credit.SuspendLayout()
        Me.pnl_Today.SuspendLayout()
        Me.pnl_Due.SuspendLayout()
        Me.pnl_Overdue.SuspendLayout()
        Me.pnl_terms.SuspendLayout()
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
        Me.paymentterms.Size = New System.Drawing.Size(136, 23)
        Me.paymentterms.TabIndex = 2
        '
        'lbl_Credit
        '
        Me.lbl_Credit.Dock = System.Windows.Forms.DockStyle.Left
        Me.lbl_Credit.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lbl_Credit.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Credit.Location = New System.Drawing.Point(0, 0)
        Me.lbl_Credit.Name = "lbl_Credit"
        Me.lbl_Credit.Size = New System.Drawing.Size(104, 22)
        Me.lbl_Credit.Text = "Credit: "
        Me.lbl_Credit.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'pnl_cash
        '
        Me.pnl_cash.Controls.Add(Me.radio_Cash)
        Me.pnl_cash.Controls.Add(Me.cash)
        Me.pnl_cash.Controls.Add(Me.lbl_Cash)
        Me.pnl_cash.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnl_cash.Location = New System.Drawing.Point(0, 178)
        Me.pnl_cash.Name = "pnl_cash"
        Me.pnl_cash.Size = New System.Drawing.Size(240, 22)
        '
        'radio_Cash
        '
        Me.radio_Cash.Dock = System.Windows.Forms.DockStyle.Right
        Me.radio_Cash.Location = New System.Drawing.Point(217, 0)
        Me.radio_Cash.Name = "radio_Cash"
        Me.radio_Cash.Size = New System.Drawing.Size(23, 22)
        Me.radio_Cash.TabIndex = 11
        '
        'cash
        '
        Me.cash.Enabled = False
        Me.cash.Location = New System.Drawing.Point(104, 0)
        Me.cash.Name = "cash"
        Me.cash.Size = New System.Drawing.Size(114, 23)
        Me.cash.TabIndex = 2
        '
        'lbl_Cash
        '
        Me.lbl_Cash.Dock = System.Windows.Forms.DockStyle.Left
        Me.lbl_Cash.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lbl_Cash.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Cash.Location = New System.Drawing.Point(0, 0)
        Me.lbl_Cash.Name = "lbl_Cash"
        Me.lbl_Cash.Size = New System.Drawing.Size(104, 22)
        Me.lbl_Cash.Text = "Cash: "
        Me.lbl_Cash.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Panel11
        '
        Me.Panel11.AutoScroll = True
        Me.Panel11.Controls.Add(Me.pnl_Check)
        Me.Panel11.Controls.Add(Me.pnl_ChqNum)
        Me.Panel11.Controls.Add(Me.pnl_Unallocated)
        Me.Panel11.Controls.Add(Me.pnl_Credit)
        Me.Panel11.Controls.Add(Me.pnl_cash)
        Me.Panel11.Controls.Add(Me.pnl_Today)
        Me.Panel11.Controls.Add(Me.pnl_Due)
        Me.Panel11.Controls.Add(Me.pnl_Overdue)
        Me.Panel11.Controls.Add(Me.pnl_terms)
        Me.Panel11.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel11.Location = New System.Drawing.Point(0, 0)
        Me.Panel11.Name = "Panel11"
        Me.Panel11.Size = New System.Drawing.Size(240, 200)
        '
        'pnl_Check
        '
        Me.pnl_Check.Controls.Add(Me.radio_Cheque)
        Me.pnl_Check.Controls.Add(Me.cheque)
        Me.pnl_Check.Controls.Add(Me.lbl_Check)
        Me.pnl_Check.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnl_Check.Location = New System.Drawing.Point(0, 134)
        Me.pnl_Check.Name = "pnl_Check"
        Me.pnl_Check.Size = New System.Drawing.Size(240, 22)
        '
        'radio_Cheque
        '
        Me.radio_Cheque.Checked = True
        Me.radio_Cheque.Dock = System.Windows.Forms.DockStyle.Right
        Me.radio_Cheque.Location = New System.Drawing.Point(217, 0)
        Me.radio_Cheque.Name = "radio_Cheque"
        Me.radio_Cheque.Size = New System.Drawing.Size(23, 22)
        Me.radio_Cheque.TabIndex = 10
        '
        'cheque
        '
        Me.cheque.Enabled = False
        Me.cheque.Location = New System.Drawing.Point(104, 0)
        Me.cheque.Name = "cheque"
        Me.cheque.Size = New System.Drawing.Size(114, 23)
        Me.cheque.TabIndex = 8
        '
        'lbl_Check
        '
        Me.lbl_Check.Dock = System.Windows.Forms.DockStyle.Left
        Me.lbl_Check.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lbl_Check.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Check.Location = New System.Drawing.Point(0, 0)
        Me.lbl_Check.Name = "lbl_Check"
        Me.lbl_Check.Size = New System.Drawing.Size(104, 22)
        Me.lbl_Check.Text = "Cheque: "
        Me.lbl_Check.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'pnl_ChqNum
        '
        Me.pnl_ChqNum.Controls.Add(Me.Banks)
        Me.pnl_ChqNum.Controls.Add(Me.ChqNum)
        Me.pnl_ChqNum.Controls.Add(Me.lbl_chqNum)
        Me.pnl_ChqNum.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnl_ChqNum.Location = New System.Drawing.Point(0, 156)
        Me.pnl_ChqNum.Name = "pnl_ChqNum"
        Me.pnl_ChqNum.Size = New System.Drawing.Size(240, 22)
        '
        'Banks
        '
        Me.Banks.Dock = System.Windows.Forms.DockStyle.Left
        Me.Banks.Location = New System.Drawing.Point(104, 0)
        Me.Banks.Name = "Banks"
        Me.Banks.Size = New System.Drawing.Size(50, 23)
        Me.Banks.TabIndex = 4
        '
        'ChqNum
        '
        Me.ChqNum.Dock = System.Windows.Forms.DockStyle.Right
        Me.ChqNum.Location = New System.Drawing.Point(160, 0)
        Me.ChqNum.Name = "ChqNum"
        Me.ChqNum.Size = New System.Drawing.Size(80, 23)
        Me.ChqNum.TabIndex = 2
        '
        'lbl_chqNum
        '
        Me.lbl_chqNum.Dock = System.Windows.Forms.DockStyle.Left
        Me.lbl_chqNum.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lbl_chqNum.ForeColor = System.Drawing.Color.Blue
        Me.lbl_chqNum.Location = New System.Drawing.Point(0, 0)
        Me.lbl_chqNum.Name = "lbl_chqNum"
        Me.lbl_chqNum.Size = New System.Drawing.Size(104, 22)
        Me.lbl_chqNum.Text = "Cheque #: "
        Me.lbl_chqNum.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'pnl_Unallocated
        '
        Me.pnl_Unallocated.Controls.Add(Me.inc_Unallocated)
        Me.pnl_Unallocated.Controls.Add(Me.Unallocated)
        Me.pnl_Unallocated.Controls.Add(Me.lbl_Unallocated)
        Me.pnl_Unallocated.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnl_Unallocated.Location = New System.Drawing.Point(0, 110)
        Me.pnl_Unallocated.Name = "pnl_Unallocated"
        Me.pnl_Unallocated.Size = New System.Drawing.Size(240, 22)
        '
        'inc_Unallocated
        '
        Me.inc_Unallocated.Dock = System.Windows.Forms.DockStyle.Right
        Me.inc_Unallocated.Location = New System.Drawing.Point(214, 0)
        Me.inc_Unallocated.Name = "inc_Unallocated"
        Me.inc_Unallocated.Size = New System.Drawing.Size(26, 22)
        Me.inc_Unallocated.TabIndex = 5
        '
        'Unallocated
        '
        Me.Unallocated.Location = New System.Drawing.Point(104, 0)
        Me.Unallocated.Name = "Unallocated"
        Me.Unallocated.Size = New System.Drawing.Size(114, 23)
        Me.Unallocated.TabIndex = 3
        '
        'lbl_Unallocated
        '
        Me.lbl_Unallocated.Dock = System.Windows.Forms.DockStyle.Left
        Me.lbl_Unallocated.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lbl_Unallocated.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Unallocated.Location = New System.Drawing.Point(0, 0)
        Me.lbl_Unallocated.Name = "lbl_Unallocated"
        Me.lbl_Unallocated.Size = New System.Drawing.Size(104, 22)
        Me.lbl_Unallocated.Text = "On Account: "
        Me.lbl_Unallocated.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'pnl_Credit
        '
        Me.pnl_Credit.Controls.Add(Me.inc_Credit)
        Me.pnl_Credit.Controls.Add(Me.Credit)
        Me.pnl_Credit.Controls.Add(Me.lbl_Credit)
        Me.pnl_Credit.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnl_Credit.Location = New System.Drawing.Point(0, 88)
        Me.pnl_Credit.Name = "pnl_Credit"
        Me.pnl_Credit.Size = New System.Drawing.Size(240, 22)
        '
        'inc_Credit
        '
        Me.inc_Credit.Dock = System.Windows.Forms.DockStyle.Right
        Me.inc_Credit.Location = New System.Drawing.Point(214, 0)
        Me.inc_Credit.Name = "inc_Credit"
        Me.inc_Credit.Size = New System.Drawing.Size(26, 22)
        Me.inc_Credit.TabIndex = 5
        '
        'Credit
        '
        Me.Credit.Enabled = False
        Me.Credit.Location = New System.Drawing.Point(104, 0)
        Me.Credit.Name = "Credit"
        Me.Credit.Size = New System.Drawing.Size(114, 23)
        Me.Credit.TabIndex = 3
        '
        'pnl_Today
        '
        Me.pnl_Today.Controls.Add(Me.inc_Today)
        Me.pnl_Today.Controls.Add(Me.todaysinvoicetotals)
        Me.pnl_Today.Controls.Add(Me.lbl_Today)
        Me.pnl_Today.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnl_Today.Location = New System.Drawing.Point(0, 66)
        Me.pnl_Today.Name = "pnl_Today"
        Me.pnl_Today.Size = New System.Drawing.Size(240, 22)
        '
        'inc_Today
        '
        Me.inc_Today.Dock = System.Windows.Forms.DockStyle.Right
        Me.inc_Today.Location = New System.Drawing.Point(214, 0)
        Me.inc_Today.Name = "inc_Today"
        Me.inc_Today.Size = New System.Drawing.Size(26, 22)
        Me.inc_Today.TabIndex = 6
        '
        'todaysinvoicetotals
        '
        Me.todaysinvoicetotals.Enabled = False
        Me.todaysinvoicetotals.Location = New System.Drawing.Point(104, 0)
        Me.todaysinvoicetotals.Name = "todaysinvoicetotals"
        Me.todaysinvoicetotals.Size = New System.Drawing.Size(114, 23)
        Me.todaysinvoicetotals.TabIndex = 2
        '
        'lbl_Today
        '
        Me.lbl_Today.Dock = System.Windows.Forms.DockStyle.Left
        Me.lbl_Today.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lbl_Today.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Today.Location = New System.Drawing.Point(0, 0)
        Me.lbl_Today.Name = "lbl_Today"
        Me.lbl_Today.Size = New System.Drawing.Size(104, 22)
        Me.lbl_Today.Text = "Today: "
        Me.lbl_Today.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'pnl_Due
        '
        Me.pnl_Due.Controls.Add(Me.inc_Due)
        Me.pnl_Due.Controls.Add(Me.dueamount)
        Me.pnl_Due.Controls.Add(Me.lbl_Due)
        Me.pnl_Due.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnl_Due.Location = New System.Drawing.Point(0, 44)
        Me.pnl_Due.Name = "pnl_Due"
        Me.pnl_Due.Size = New System.Drawing.Size(240, 22)
        '
        'inc_Due
        '
        Me.inc_Due.Dock = System.Windows.Forms.DockStyle.Right
        Me.inc_Due.Location = New System.Drawing.Point(214, 0)
        Me.inc_Due.Name = "inc_Due"
        Me.inc_Due.Size = New System.Drawing.Size(26, 22)
        Me.inc_Due.TabIndex = 6
        '
        'dueamount
        '
        Me.dueamount.Enabled = False
        Me.dueamount.Location = New System.Drawing.Point(104, 0)
        Me.dueamount.Name = "dueamount"
        Me.dueamount.Size = New System.Drawing.Size(114, 23)
        Me.dueamount.TabIndex = 3
        '
        'lbl_Due
        '
        Me.lbl_Due.Dock = System.Windows.Forms.DockStyle.Left
        Me.lbl_Due.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lbl_Due.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Due.Location = New System.Drawing.Point(0, 0)
        Me.lbl_Due.Name = "lbl_Due"
        Me.lbl_Due.Size = New System.Drawing.Size(104, 22)
        Me.lbl_Due.Text = "Due: "
        Me.lbl_Due.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'pnl_Overdue
        '
        Me.pnl_Overdue.Controls.Add(Me.inc_Overdue)
        Me.pnl_Overdue.Controls.Add(Me.overduepayment)
        Me.pnl_Overdue.Controls.Add(Me.lbl_Overdue)
        Me.pnl_Overdue.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnl_Overdue.Location = New System.Drawing.Point(0, 22)
        Me.pnl_Overdue.Name = "pnl_Overdue"
        Me.pnl_Overdue.Size = New System.Drawing.Size(240, 22)
        '
        'inc_Overdue
        '
        Me.inc_Overdue.Dock = System.Windows.Forms.DockStyle.Right
        Me.inc_Overdue.Location = New System.Drawing.Point(214, 0)
        Me.inc_Overdue.Name = "inc_Overdue"
        Me.inc_Overdue.Size = New System.Drawing.Size(26, 22)
        Me.inc_Overdue.TabIndex = 10
        '
        'overduepayment
        '
        Me.overduepayment.Enabled = False
        Me.overduepayment.Location = New System.Drawing.Point(104, 0)
        Me.overduepayment.Name = "overduepayment"
        Me.overduepayment.Size = New System.Drawing.Size(114, 23)
        Me.overduepayment.TabIndex = 8
        '
        'lbl_Overdue
        '
        Me.lbl_Overdue.Dock = System.Windows.Forms.DockStyle.Left
        Me.lbl_Overdue.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lbl_Overdue.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Overdue.Location = New System.Drawing.Point(0, 0)
        Me.lbl_Overdue.Name = "lbl_Overdue"
        Me.lbl_Overdue.Size = New System.Drawing.Size(104, 22)
        Me.lbl_Overdue.Text = "Overdue: "
        Me.lbl_Overdue.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'pnl_terms
        '
        Me.pnl_terms.Controls.Add(Me.paymentterms)
        Me.pnl_terms.Controls.Add(Me.Label1)
        Me.pnl_terms.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnl_terms.Location = New System.Drawing.Point(0, 0)
        Me.pnl_terms.Name = "pnl_terms"
        Me.pnl_terms.Size = New System.Drawing.Size(240, 22)
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
        Me.Panel9.Size = New System.Drawing.Size(245, 210)
        '
        'Panel12
        '
        Me.Panel12.Controls.Add(Me.Panel14)
        Me.Panel12.Controls.Add(Me.Panel13)
        Me.Panel12.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel12.Location = New System.Drawing.Point(0, 0)
        Me.Panel12.Name = "Panel12"
        Me.Panel12.Size = New System.Drawing.Size(240, 210)
        '
        'Panel14
        '
        Me.Panel14.Controls.Add(Me.Panel11)
        Me.Panel14.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel14.Location = New System.Drawing.Point(0, 10)
        Me.Panel14.Name = "Panel14"
        Me.Panel14.Size = New System.Drawing.Size(240, 200)
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
        Me.Panel10.Size = New System.Drawing.Size(5, 210)
        '
        'ctrl_Payment
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.Panel9)
        Me.Name = "ctrl_Payment"
        Me.Size = New System.Drawing.Size(245, 210)
        Me.pnl_cash.ResumeLayout(False)
        Me.Panel11.ResumeLayout(False)
        Me.pnl_Check.ResumeLayout(False)
        Me.pnl_ChqNum.ResumeLayout(False)
        Me.pnl_Unallocated.ResumeLayout(False)
        Me.pnl_Credit.ResumeLayout(False)
        Me.pnl_Today.ResumeLayout(False)
        Me.pnl_Due.ResumeLayout(False)
        Me.pnl_Overdue.ResumeLayout(False)
        Me.pnl_terms.ResumeLayout(False)
        Me.Panel9.ResumeLayout(False)
        Me.Panel12.ResumeLayout(False)
        Me.Panel14.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents paymentterms As System.Windows.Forms.TextBox
    Friend WithEvents Panel11 As System.Windows.Forms.Panel
    Friend WithEvents pnl_Today As System.Windows.Forms.Panel
    Friend WithEvents todaysinvoicetotals As System.Windows.Forms.TextBox
    Friend WithEvents lbl_Today As System.Windows.Forms.Label
    Friend WithEvents pnl_Due As System.Windows.Forms.Panel
    Friend WithEvents dueamount As System.Windows.Forms.TextBox
    Friend WithEvents lbl_Due As System.Windows.Forms.Label
    Friend WithEvents pnl_Overdue As System.Windows.Forms.Panel
    Friend WithEvents overduepayment As System.Windows.Forms.TextBox
    Friend WithEvents lbl_Overdue As System.Windows.Forms.Label
    Friend WithEvents pnl_terms As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel9 As System.Windows.Forms.Panel
    Friend WithEvents Panel12 As System.Windows.Forms.Panel
    Friend WithEvents Panel14 As System.Windows.Forms.Panel
    Friend WithEvents Panel13 As System.Windows.Forms.Panel
    Friend WithEvents Panel10 As System.Windows.Forms.Panel
    Friend WithEvents pnl_Credit As System.Windows.Forms.Panel
    Friend WithEvents Credit As System.Windows.Forms.TextBox
    Friend WithEvents lbl_Credit As System.Windows.Forms.Label
    Friend WithEvents pnl_cash As System.Windows.Forms.Panel
    Friend WithEvents cash As System.Windows.Forms.TextBox
    Friend WithEvents lbl_Cash As System.Windows.Forms.Label
    Friend WithEvents inc_Credit As System.Windows.Forms.CheckBox
    Friend WithEvents radio_Cash As System.Windows.Forms.RadioButton
    Friend WithEvents inc_Today As System.Windows.Forms.CheckBox
    Friend WithEvents inc_Due As System.Windows.Forms.CheckBox
    Friend WithEvents inc_Overdue As System.Windows.Forms.CheckBox
    Friend WithEvents pnl_Unallocated As System.Windows.Forms.Panel
    Friend WithEvents inc_Unallocated As System.Windows.Forms.CheckBox
    Friend WithEvents Unallocated As System.Windows.Forms.TextBox
    Friend WithEvents lbl_Unallocated As System.Windows.Forms.Label
    Friend WithEvents pnl_ChqNum As System.Windows.Forms.Panel
    Friend WithEvents Banks As System.Windows.Forms.ComboBox
    Friend WithEvents ChqNum As System.Windows.Forms.TextBox
    Friend WithEvents lbl_chqNum As System.Windows.Forms.Label
    Friend WithEvents pnl_Check As System.Windows.Forms.Panel
    Friend WithEvents radio_Cheque As System.Windows.Forms.RadioButton
    Friend WithEvents cheque As System.Windows.Forms.TextBox
    Friend WithEvents lbl_Check As System.Windows.Forms.Label

End Class
