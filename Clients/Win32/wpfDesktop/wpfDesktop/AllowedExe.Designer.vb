<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AllowedExe
    Inherits System.Windows.Forms.Form

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AllowedExe))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.exelist = New System.Windows.Forms.ListView
        Me.exeName = New System.Windows.Forms.ColumnHeader
        Me.exepath = New System.Windows.Forms.ColumnHeader
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.btndel = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.exelist, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel2, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(552, 193)
        Me.TableLayoutPanel1.TabIndex = 1
        '
        'exelist
        '
        Me.exelist.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.exeName, Me.exepath})
        Me.exelist.Dock = System.Windows.Forms.DockStyle.Fill
        Me.exelist.FullRowSelect = True
        Me.exelist.Location = New System.Drawing.Point(3, 3)
        Me.exelist.MultiSelect = False
        Me.exelist.Name = "exelist"
        Me.exelist.Size = New System.Drawing.Size(546, 152)
        Me.exelist.TabIndex = 1
        Me.exelist.UseCompatibleStateImageBehavior = False
        Me.exelist.View = System.Windows.Forms.View.Details
        '
        'exeName
        '
        Me.exeName.Text = "Executable Name"
        Me.exeName.Width = 96
        '
        'exepath
        '
        Me.exepath.Text = "Executable Path"
        Me.exepath.Width = 191
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 3
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.btndel, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Button1, 0, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(3, 161)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(546, 29)
        Me.TableLayoutPanel2.TabIndex = 2
        '
        'btndel
        '
        Me.btndel.Enabled = False
        Me.btndel.Location = New System.Drawing.Point(73, 3)
        Me.btndel.Name = "btndel"
        Me.btndel.Size = New System.Drawing.Size(64, 23)
        Me.btndel.TabIndex = 4
        Me.btndel.Text = "Remove"
        Me.btndel.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(3, 3)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(64, 23)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = "New"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'AllowedExe
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(552, 193)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "AllowedExe"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Allowed Executables"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents exelist As System.Windows.Forms.ListView
    Friend WithEvents exeName As System.Windows.Forms.ColumnHeader
    Friend WithEvents exepath As System.Windows.Forms.ColumnHeader
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents btndel As System.Windows.Forms.Button
End Class
