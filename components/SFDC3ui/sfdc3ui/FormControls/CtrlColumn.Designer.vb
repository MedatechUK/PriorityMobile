<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class CtrlColumn
    Inherits System.Windows.Forms.UserControl

    'UserControl1 overrides dispose to clean up the component list.
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
        Me.edit_Date = New System.Windows.Forms.DateTimePicker
        Me.Edit_List = New System.Windows.Forms.ComboBox
        Me.Edit_Boolean = New System.Windows.Forms.CheckBox
        Me.ControlLabel = New System.Windows.Forms.LinkLabel
        Me.NumericUpDown1 = New System.Windows.Forms.NumericUpDown
        Me.Edit_Text = New System.Windows.Forms.Label
        Me.Display_Value = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'edit_Date
        '
        Me.edit_Date.Location = New System.Drawing.Point(106, 69)
        Me.edit_Date.Name = "edit_Date"
        Me.edit_Date.Size = New System.Drawing.Size(200, 24)
        Me.edit_Date.TabIndex = 0
        Me.edit_Date.Visible = False
        '
        'Edit_List
        '
        Me.Edit_List.Location = New System.Drawing.Point(106, 100)
        Me.Edit_List.Name = "Edit_List"
        Me.Edit_List.Size = New System.Drawing.Size(200, 23)
        Me.Edit_List.TabIndex = 1
        Me.Edit_List.Visible = False
        '
        'Edit_Boolean
        '
        Me.Edit_Boolean.Location = New System.Drawing.Point(101, 130)
        Me.Edit_Boolean.Name = "Edit_Boolean"
        Me.Edit_Boolean.Size = New System.Drawing.Size(100, 20)
        Me.Edit_Boolean.TabIndex = 2
        Me.Edit_Boolean.Visible = False
        '
        'ControlLabel
        '
        Me.ControlLabel.Location = New System.Drawing.Point(1, 2)
        Me.ControlLabel.Name = "ControlLabel"
        Me.ControlLabel.Size = New System.Drawing.Size(100, 20)
        Me.ControlLabel.TabIndex = 4
        Me.ControlLabel.Text = "LinkLabel1"
        '
        'NumericUpDown1
        '
        Me.NumericUpDown1.Location = New System.Drawing.Point(106, 189)
        Me.NumericUpDown1.Name = "NumericUpDown1"
        Me.NumericUpDown1.Size = New System.Drawing.Size(200, 24)
        Me.NumericUpDown1.TabIndex = 5
        Me.NumericUpDown1.Visible = False
        '
        'Edit_Text
        '
        Me.Edit_Text.BackColor = System.Drawing.Color.Red
        Me.Edit_Text.Location = New System.Drawing.Point(104, 1)
        Me.Edit_Text.Name = "Edit_Text"
        Me.Edit_Text.Size = New System.Drawing.Size(201, 26)
        Me.Edit_Text.Text = "Label1"
        Me.Edit_Text.Visible = False
        '
        'Display_Value
        '
        Me.Display_Value.Location = New System.Drawing.Point(116, 31)
        Me.Display_Value.Name = "Display_Value"
        Me.Display_Value.Size = New System.Drawing.Size(100, 20)
        '
        'CtrlColumn
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.Display_Value)
        Me.Controls.Add(Me.Edit_Text)
        Me.Controls.Add(Me.NumericUpDown1)
        Me.Controls.Add(Me.ControlLabel)
        Me.Controls.Add(Me.Edit_Boolean)
        Me.Controls.Add(Me.Edit_List)
        Me.Controls.Add(Me.edit_Date)
        Me.Name = "CtrlColumn"
        Me.Size = New System.Drawing.Size(309, 255)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents edit_Date As System.Windows.Forms.DateTimePicker
    Friend WithEvents Edit_List As System.Windows.Forms.ComboBox
    Friend WithEvents Edit_Boolean As System.Windows.Forms.CheckBox
    Friend WithEvents ControlLabel As System.Windows.Forms.LinkLabel
    Friend WithEvents NumericUpDown1 As System.Windows.Forms.NumericUpDown
    Friend WithEvents Edit_Text As System.Windows.Forms.Label
    Friend WithEvents Display_Value As System.Windows.Forms.Label

End Class
