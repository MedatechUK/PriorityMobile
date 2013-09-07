<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class KeyEditor
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
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.chk_Alt = New System.Windows.Forms.CheckBox
        Me.Chk_Shift = New System.Windows.Forms.CheckBox
        Me.ComboBox1 = New System.Windows.Forms.ComboBox
        Me.Chk_Ctrl = New System.Windows.Forms.CheckBox
        Me.Ctrl = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.txt_Encoded = New System.Windows.Forms.TextBox
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(61, 4)
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 4
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.chk_Alt, 3, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Chk_Shift, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ComboBox1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Chk_Ctrl, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Ctrl, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label2, 2, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label3, 3, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Button1, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_Encoded, 0, 2)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 3
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(246, 181)
        Me.TableLayoutPanel1.TabIndex = 3
        '
        'chk_Alt
        '
        Me.chk_Alt.AutoSize = True
        Me.chk_Alt.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chk_Alt.Location = New System.Drawing.Point(219, 3)
        Me.chk_Alt.Name = "chk_Alt"
        Me.chk_Alt.Size = New System.Drawing.Size(24, 19)
        Me.chk_Alt.TabIndex = 6
        Me.chk_Alt.UseVisualStyleBackColor = True
        '
        'Chk_Shift
        '
        Me.Chk_Shift.AutoSize = True
        Me.Chk_Shift.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Chk_Shift.Location = New System.Drawing.Point(184, 3)
        Me.Chk_Shift.Name = "Chk_Shift"
        Me.Chk_Shift.Size = New System.Drawing.Size(29, 19)
        Me.Chk_Shift.TabIndex = 5
        Me.Chk_Shift.UseVisualStyleBackColor = True
        '
        'ComboBox1
        '
        Me.ComboBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(3, 3)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(145, 21)
        Me.ComboBox1.TabIndex = 0
        '
        'Chk_Ctrl
        '
        Me.Chk_Ctrl.AutoSize = True
        Me.Chk_Ctrl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Chk_Ctrl.Location = New System.Drawing.Point(154, 3)
        Me.Chk_Ctrl.Name = "Chk_Ctrl"
        Me.Chk_Ctrl.Size = New System.Drawing.Size(24, 19)
        Me.Chk_Ctrl.TabIndex = 1
        Me.Chk_Ctrl.UseVisualStyleBackColor = True
        '
        'Ctrl
        '
        Me.Ctrl.AutoSize = True
        Me.Ctrl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Ctrl.Location = New System.Drawing.Point(154, 25)
        Me.Ctrl.Name = "Ctrl"
        Me.Ctrl.Size = New System.Drawing.Size(24, 30)
        Me.Ctrl.TabIndex = 2
        Me.Ctrl.Text = "Ctrl"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label2.Location = New System.Drawing.Point(184, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(29, 30)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Shift"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label3.Location = New System.Drawing.Point(219, 25)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(24, 30)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Alt"
        '
        'Button1
        '
        Me.Button1.Dock = System.Windows.Forms.DockStyle.Right
        Me.Button1.Location = New System.Drawing.Point(112, 28)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(36, 24)
        Me.Button1.TabIndex = 7
        Me.Button1.Text = ">>"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'txt_Encoded
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.txt_Encoded, 4)
        Me.txt_Encoded.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txt_Encoded.Location = New System.Drawing.Point(3, 58)
        Me.txt_Encoded.Multiline = True
        Me.txt_Encoded.Name = "txt_Encoded"
        Me.txt_Encoded.Size = New System.Drawing.Size(240, 120)
        Me.txt_Encoded.TabIndex = 8
        '
        'KeyEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(246, 181)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(226, 167)
        Me.Name = "KeyEditor"
        Me.Text = "KeyEditor"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents Chk_Ctrl As System.Windows.Forms.CheckBox
    Friend WithEvents Ctrl As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents chk_Alt As System.Windows.Forms.CheckBox
    Friend WithEvents Chk_Shift As System.Windows.Forms.CheckBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Public WithEvents txt_Encoded As System.Windows.Forms.TextBox
End Class
