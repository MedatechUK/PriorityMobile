<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ct_Settings
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
        Me.Status = New System.Windows.Forms.ColumnHeader
        Me.Action = New System.Windows.Forms.ColumnHeader
        Me.lbl_Freq = New System.Windows.Forms.Label
        Me.Timer1 = New System.Windows.Forms.Timer
        Me.lst_Status = New System.Windows.Forms.ListView
        Me.Chk_CheckOnStart = New System.Windows.Forms.CheckBox
        Me.lbl_ServiceURL = New System.Windows.Forms.Label
        Me.txt_ServiceURL = New System.Windows.Forms.TextBox
        Me.lbl_WareHouse = New System.Windows.Forms.Label
        Me.txt_Warehouse = New System.Windows.Forms.TextBox
        Me.lbl_UserName = New System.Windows.Forms.Label
        Me.txt_UserName = New System.Windows.Forms.TextBox
        Me.CheckFreq = New System.Windows.Forms.TrackBar
        Me.SuspendLayout()
        '
        'Status
        '
        Me.Status.Text = "Status"
        Me.Status.Width = 101
        '
        'Action
        '
        Me.Action.Text = "Action"
        Me.Action.Width = 92
        '
        'lbl_Freq
        '
        Me.lbl_Freq.Location = New System.Drawing.Point(0, 143)
        Me.lbl_Freq.Name = "lbl_Freq"
        Me.lbl_Freq.Size = New System.Drawing.Size(223, 19)
        Me.lbl_Freq.Text = "Label2"
        Me.lbl_Freq.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 60000
        '
        'lst_Status
        '
        Me.lst_Status.Columns.Add(Me.Action)
        Me.lst_Status.Columns.Add(Me.Status)
        Me.lst_Status.FullRowSelect = True
        Me.lst_Status.Location = New System.Drawing.Point(10, 165)
        Me.lst_Status.Name = "lst_Status"
        Me.lst_Status.Size = New System.Drawing.Size(205, 151)
        Me.lst_Status.TabIndex = 21
        Me.lst_Status.View = System.Windows.Forms.View.Details
        '
        'Chk_CheckOnStart
        '
        Me.Chk_CheckOnStart.Location = New System.Drawing.Point(94, 83)
        Me.Chk_CheckOnStart.Name = "Chk_CheckOnStart"
        Me.Chk_CheckOnStart.Size = New System.Drawing.Size(121, 21)
        Me.Chk_CheckOnStart.TabIndex = 22
        Me.Chk_CheckOnStart.Text = "Check On Start"
        '
        'lbl_ServiceURL
        '
        Me.lbl_ServiceURL.Location = New System.Drawing.Point(7, 10)
        Me.lbl_ServiceURL.Name = "lbl_ServiceURL"
        Me.lbl_ServiceURL.Size = New System.Drawing.Size(81, 15)
        Me.lbl_ServiceURL.Text = "Service URL:"
        '
        'txt_ServiceURL
        '
        Me.txt_ServiceURL.Location = New System.Drawing.Point(94, 4)
        Me.txt_ServiceURL.Name = "txt_ServiceURL"
        Me.txt_ServiceURL.Size = New System.Drawing.Size(126, 21)
        Me.txt_ServiceURL.TabIndex = 20
        '
        'lbl_WareHouse
        '
        Me.lbl_WareHouse.Location = New System.Drawing.Point(13, 62)
        Me.lbl_WareHouse.Name = "lbl_WareHouse"
        Me.lbl_WareHouse.Size = New System.Drawing.Size(75, 15)
        Me.lbl_WareHouse.Text = "Warehouse:"
        '
        'txt_Warehouse
        '
        Me.txt_Warehouse.Location = New System.Drawing.Point(94, 56)
        Me.txt_Warehouse.Name = "txt_Warehouse"
        Me.txt_Warehouse.Size = New System.Drawing.Size(126, 21)
        Me.txt_Warehouse.TabIndex = 19
        '
        'lbl_UserName
        '
        Me.lbl_UserName.Location = New System.Drawing.Point(15, 37)
        Me.lbl_UserName.Name = "lbl_UserName"
        Me.lbl_UserName.Size = New System.Drawing.Size(73, 15)
        Me.lbl_UserName.Text = "User Name:"
        '
        'txt_UserName
        '
        Me.txt_UserName.Location = New System.Drawing.Point(94, 31)
        Me.txt_UserName.Name = "txt_UserName"
        Me.txt_UserName.Size = New System.Drawing.Size(126, 21)
        Me.txt_UserName.TabIndex = 18
        '
        'CheckFreq
        '
        Me.CheckFreq.Location = New System.Drawing.Point(15, 110)
        Me.CheckFreq.Maximum = 4
        Me.CheckFreq.Name = "CheckFreq"
        Me.CheckFreq.Size = New System.Drawing.Size(200, 42)
        Me.CheckFreq.TabIndex = 17
        '
        'ct_Settings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.Controls.Add(Me.lbl_Freq)
        Me.Controls.Add(Me.lst_Status)
        Me.Controls.Add(Me.Chk_CheckOnStart)
        Me.Controls.Add(Me.lbl_ServiceURL)
        Me.Controls.Add(Me.txt_ServiceURL)
        Me.Controls.Add(Me.lbl_WareHouse)
        Me.Controls.Add(Me.txt_Warehouse)
        Me.Controls.Add(Me.lbl_UserName)
        Me.Controls.Add(Me.txt_UserName)
        Me.Controls.Add(Me.CheckFreq)
        Me.Name = "ct_Settings"
        Me.Size = New System.Drawing.Size(223, 332)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Status As System.Windows.Forms.ColumnHeader
    Friend WithEvents Action As System.Windows.Forms.ColumnHeader
    Friend WithEvents lbl_Freq As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents lst_Status As System.Windows.Forms.ListView
    Friend WithEvents Chk_CheckOnStart As System.Windows.Forms.CheckBox
    Friend WithEvents lbl_ServiceURL As System.Windows.Forms.Label
    Friend WithEvents txt_ServiceURL As System.Windows.Forms.TextBox
    Friend WithEvents lbl_WareHouse As System.Windows.Forms.Label
    Friend WithEvents txt_Warehouse As System.Windows.Forms.TextBox
    Friend WithEvents lbl_UserName As System.Windows.Forms.Label
    Friend WithEvents txt_UserName As System.Windows.Forms.TextBox
    Friend WithEvents CheckFreq As System.Windows.Forms.TrackBar

End Class
