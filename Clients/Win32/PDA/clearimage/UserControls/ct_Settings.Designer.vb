<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ct_Settings
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
        Me.components = New System.ComponentModel.Container
        Me.lbl_Freq = New System.Windows.Forms.Label
        Me.CheckFreq = New System.Windows.Forms.TrackBar
        Me.txt_UserName = New System.Windows.Forms.TextBox
        Me.lbl_UserName = New System.Windows.Forms.Label
        Me.lbl_WareHouse = New System.Windows.Forms.Label
        Me.txt_Warehouse = New System.Windows.Forms.TextBox
        Me.lbl_ServiceURL = New System.Windows.Forms.Label
        Me.txt_ServiceURL = New System.Windows.Forms.TextBox
        Me.lst_Status = New System.Windows.Forms.ListView
        Me.Action = New System.Windows.Forms.ColumnHeader
        Me.Status = New System.Windows.Forms.ColumnHeader
        Me.Chk_CheckOnStart = New System.Windows.Forms.CheckBox
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        CType(Me.CheckFreq, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lbl_Freq
        '
        Me.lbl_Freq.AutoSize = True
        Me.lbl_Freq.Location = New System.Drawing.Point(100, 150)
        Me.lbl_Freq.Name = "lbl_Freq"
        Me.lbl_Freq.Size = New System.Drawing.Size(39, 13)
        Me.lbl_Freq.TabIndex = 1
        Me.lbl_Freq.Text = "Label2"
        '
        'CheckFreq
        '
        Me.CheckFreq.Location = New System.Drawing.Point(20, 117)
        Me.CheckFreq.Maximum = 4
        Me.CheckFreq.Name = "CheckFreq"
        Me.CheckFreq.Size = New System.Drawing.Size(200, 42)
        Me.CheckFreq.TabIndex = 2
        '
        'txt_UserName
        '
        Me.txt_UserName.Location = New System.Drawing.Point(89, 37)
        Me.txt_UserName.Name = "txt_UserName"
        Me.txt_UserName.Size = New System.Drawing.Size(126, 20)
        Me.txt_UserName.TabIndex = 3
        '
        'lbl_UserName
        '
        Me.lbl_UserName.AutoSize = True
        Me.lbl_UserName.Location = New System.Drawing.Point(20, 41)
        Me.lbl_UserName.Name = "lbl_UserName"
        Me.lbl_UserName.Size = New System.Drawing.Size(63, 13)
        Me.lbl_UserName.TabIndex = 4
        Me.lbl_UserName.Text = "User Name:"
        '
        'lbl_WareHouse
        '
        Me.lbl_WareHouse.AutoSize = True
        Me.lbl_WareHouse.Location = New System.Drawing.Point(18, 66)
        Me.lbl_WareHouse.Name = "lbl_WareHouse"
        Me.lbl_WareHouse.Size = New System.Drawing.Size(65, 13)
        Me.lbl_WareHouse.TabIndex = 6
        Me.lbl_WareHouse.Text = "Warehouse:"
        '
        'txt_Warehouse
        '
        Me.txt_Warehouse.Location = New System.Drawing.Point(89, 62)
        Me.txt_Warehouse.Name = "txt_Warehouse"
        Me.txt_Warehouse.Size = New System.Drawing.Size(126, 20)
        Me.txt_Warehouse.TabIndex = 5
        '
        'lbl_ServiceURL
        '
        Me.lbl_ServiceURL.AutoSize = True
        Me.lbl_ServiceURL.Location = New System.Drawing.Point(12, 14)
        Me.lbl_ServiceURL.Name = "lbl_ServiceURL"
        Me.lbl_ServiceURL.Size = New System.Drawing.Size(71, 13)
        Me.lbl_ServiceURL.TabIndex = 8
        Me.lbl_ServiceURL.Text = "Service URL:"
        '
        'txt_ServiceURL
        '
        Me.txt_ServiceURL.Location = New System.Drawing.Point(89, 10)
        Me.txt_ServiceURL.Name = "txt_ServiceURL"
        Me.txt_ServiceURL.Size = New System.Drawing.Size(126, 20)
        Me.txt_ServiceURL.TabIndex = 7
        '
        'lst_Status
        '
        Me.lst_Status.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Action, Me.Status})
        Me.lst_Status.FullRowSelect = True
        Me.lst_Status.Location = New System.Drawing.Point(15, 172)
        Me.lst_Status.Name = "lst_Status"
        Me.lst_Status.Size = New System.Drawing.Size(205, 151)
        Me.lst_Status.TabIndex = 11
        Me.lst_Status.UseCompatibleStateImageBehavior = False
        Me.lst_Status.View = System.Windows.Forms.View.Details
        '
        'Action
        '
        Me.Action.Text = "Action"
        Me.Action.Width = 92
        '
        'Status
        '
        Me.Status.Text = "Status"
        Me.Status.Width = 101
        '
        'Chk_CheckOnStart
        '
        Me.Chk_CheckOnStart.AutoSize = True
        Me.Chk_CheckOnStart.Location = New System.Drawing.Point(89, 90)
        Me.Chk_CheckOnStart.Name = "Chk_CheckOnStart"
        Me.Chk_CheckOnStart.Size = New System.Drawing.Size(99, 17)
        Me.Chk_CheckOnStart.TabIndex = 12
        Me.Chk_CheckOnStart.Text = "Check On Start"
        Me.Chk_CheckOnStart.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 60000
        '
        'ct_Settings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
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
        Me.Size = New System.Drawing.Size(238, 326)
        CType(Me.CheckFreq, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lbl_Freq As System.Windows.Forms.Label
    Friend WithEvents CheckFreq As System.Windows.Forms.TrackBar
    Friend WithEvents txt_UserName As System.Windows.Forms.TextBox
    Friend WithEvents lbl_UserName As System.Windows.Forms.Label
    Friend WithEvents lbl_WareHouse As System.Windows.Forms.Label
    Friend WithEvents txt_Warehouse As System.Windows.Forms.TextBox
    Friend WithEvents lbl_ServiceURL As System.Windows.Forms.Label
    Friend WithEvents txt_ServiceURL As System.Windows.Forms.TextBox
    Friend WithEvents lst_Status As System.Windows.Forms.ListView
    Friend WithEvents Chk_CheckOnStart As System.Windows.Forms.CheckBox
    Friend WithEvents Status As System.Windows.Forms.ColumnHeader
    Friend WithEvents Action As System.Windows.Forms.ColumnHeader
    Friend WithEvents Timer1 As System.Windows.Forms.Timer

End Class
