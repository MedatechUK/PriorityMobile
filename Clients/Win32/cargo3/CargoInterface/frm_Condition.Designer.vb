<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_Condition
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
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.lst_AllColours = New System.Windows.Forms.ListView
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.lst_Colour = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Check_Not = New System.Windows.Forms.CheckBox
        Me.lbl_Condition = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.lst_Coord = New System.Windows.Forms.ComboBox
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 74)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.lst_AllColours)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.lst_Colour)
        Me.SplitContainer1.Size = New System.Drawing.Size(287, 188)
        Me.SplitContainer1.SplitterDistance = 144
        Me.SplitContainer1.TabIndex = 7
        '
        'lst_AllColours
        '
        Me.lst_AllColours.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader2})
        Me.lst_AllColours.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lst_AllColours.Location = New System.Drawing.Point(0, 0)
        Me.lst_AllColours.Name = "lst_AllColours"
        Me.lst_AllColours.Size = New System.Drawing.Size(144, 188)
        Me.lst_AllColours.TabIndex = 6
        Me.lst_AllColours.UseCompatibleStateImageBehavior = False
        Me.lst_AllColours.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Colour Name"
        Me.ColumnHeader2.Width = 120
        '
        'lst_Colour
        '
        Me.lst_Colour.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.lst_Colour.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lst_Colour.Location = New System.Drawing.Point(0, 0)
        Me.lst_Colour.Name = "lst_Colour"
        Me.lst_Colour.Size = New System.Drawing.Size(139, 188)
        Me.lst_Colour.TabIndex = 5
        Me.lst_Colour.UseCompatibleStateImageBehavior = False
        Me.lst_Colour.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Colour Name"
        Me.ColumnHeader1.Width = 120
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Check_Not)
        Me.Panel1.Controls.Add(Me.lbl_Condition)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.lst_Coord)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(287, 68)
        Me.Panel1.TabIndex = 8
        '
        'Check_Not
        '
        Me.Check_Not.AutoSize = True
        Me.Check_Not.Location = New System.Drawing.Point(25, 36)
        Me.Check_Not.Name = "Check_Not"
        Me.Check_Not.Size = New System.Drawing.Size(43, 17)
        Me.Check_Not.TabIndex = 10
        Me.Check_Not.Text = "Not"
        Me.Check_Not.UseVisualStyleBackColor = True
        '
        'lbl_Condition
        '
        Me.lbl_Condition.AutoSize = True
        Me.lbl_Condition.Location = New System.Drawing.Point(61, 13)
        Me.lbl_Condition.Name = "lbl_Condition"
        Me.lbl_Condition.Size = New System.Drawing.Size(39, 13)
        Me.lbl_Condition.TabIndex = 9
        Me.lbl_Condition.Text = "Label2"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(54, 13)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Condition:"
        '
        'lst_Coord
        '
        Me.lst_Coord.FormattingEnabled = True
        Me.lst_Coord.Location = New System.Drawing.Point(75, 35)
        Me.lst_Coord.Name = "lst_Coord"
        Me.lst_Coord.Size = New System.Drawing.Size(151, 21)
        Me.lst_Coord.TabIndex = 7
        '
        'frm_Condition
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(287, 262)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.SplitContainer1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(303, 300)
        Me.Name = "frm_Condition"
        Me.Text = "frm_Condition"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents lst_AllColours As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lst_Colour As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Check_Not As System.Windows.Forms.CheckBox
    Friend WithEvents lbl_Condition As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lst_Coord As System.Windows.Forms.ComboBox
End Class
