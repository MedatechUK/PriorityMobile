<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ct_Survey
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
        Me.pnl_Survey = New System.Windows.Forms.Panel
        Me.pnl_SelectSurvey = New System.Windows.Forms.Panel
        Me.lst_Survey = New System.Windows.Forms.ListView
        Me.ColSurvey = New System.Windows.Forms.ColumnHeader
        Me.ColHasData = New System.Windows.Forms.ColumnHeader
        Me.pnl_TextAnswer = New System.Windows.Forms.Panel
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.txt_Answer = New System.Windows.Forms.TextBox
        Me.lbl_Question = New System.Windows.Forms.Label
        Me.pnl_SelectSurvey.SuspendLayout()
        Me.pnl_TextAnswer.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnl_Survey
        '
        Me.pnl_Survey.AutoScroll = True
        Me.pnl_Survey.Location = New System.Drawing.Point(3, 109)
        Me.pnl_Survey.Name = "pnl_Survey"
        Me.pnl_Survey.Size = New System.Drawing.Size(200, 27)
        Me.pnl_Survey.TabIndex = 0
        '
        'pnl_SelectSurvey
        '
        Me.pnl_SelectSurvey.Controls.Add(Me.lst_Survey)
        Me.pnl_SelectSurvey.Location = New System.Drawing.Point(3, 3)
        Me.pnl_SelectSurvey.Name = "pnl_SelectSurvey"
        Me.pnl_SelectSurvey.Size = New System.Drawing.Size(200, 100)
        Me.pnl_SelectSurvey.TabIndex = 1
        '
        'lst_Survey
        '
        Me.lst_Survey.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColSurvey, Me.ColHasData})
        Me.lst_Survey.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lst_Survey.Location = New System.Drawing.Point(0, 0)
        Me.lst_Survey.Name = "lst_Survey"
        Me.lst_Survey.Size = New System.Drawing.Size(200, 100)
        Me.lst_Survey.TabIndex = 0
        Me.lst_Survey.UseCompatibleStateImageBehavior = False
        Me.lst_Survey.View = System.Windows.Forms.View.Details
        '
        'ColSurvey
        '
        Me.ColSurvey.Text = "Survey"
        Me.ColSurvey.Width = 136
        '
        'ColHasData
        '
        Me.ColHasData.Text = "HasData"
        '
        'pnl_TextAnswer
        '
        Me.pnl_TextAnswer.Controls.Add(Me.TableLayoutPanel1)
        Me.pnl_TextAnswer.Location = New System.Drawing.Point(4, 143)
        Me.pnl_TextAnswer.Name = "pnl_TextAnswer"
        Me.pnl_TextAnswer.Size = New System.Drawing.Size(200, 79)
        Me.pnl_TextAnswer.TabIndex = 2
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_Answer, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.lbl_Question, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(200, 79)
        Me.TableLayoutPanel1.TabIndex = 3
        '
        'txt_Answer
        '
        Me.txt_Answer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txt_Answer.Location = New System.Drawing.Point(3, 16)
        Me.txt_Answer.Multiline = True
        Me.txt_Answer.Name = "txt_Answer"
        Me.txt_Answer.Size = New System.Drawing.Size(200, 60)
        Me.txt_Answer.TabIndex = 1
        '
        'lbl_Question
        '
        Me.lbl_Question.AutoSize = True
        Me.lbl_Question.Location = New System.Drawing.Point(3, 0)
        Me.lbl_Question.Name = "lbl_Question"
        Me.lbl_Question.Size = New System.Drawing.Size(39, 13)
        Me.lbl_Question.TabIndex = 2
        Me.lbl_Question.Text = "Label1"
        '
        'ct_Survey
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.Controls.Add(Me.pnl_TextAnswer)
        Me.Controls.Add(Me.pnl_SelectSurvey)
        Me.Controls.Add(Me.pnl_Survey)
        Me.Name = "ct_Survey"
        Me.Size = New System.Drawing.Size(207, 227)
        Me.pnl_SelectSurvey.ResumeLayout(False)
        Me.pnl_TextAnswer.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnl_Survey As System.Windows.Forms.Panel
    Friend WithEvents pnl_SelectSurvey As System.Windows.Forms.Panel
    Friend WithEvents lst_Survey As System.Windows.Forms.ListView
    Friend WithEvents ColSurvey As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColHasData As System.Windows.Forms.ColumnHeader
    Friend WithEvents pnl_TextAnswer As System.Windows.Forms.Panel
    Friend WithEvents lbl_Question As System.Windows.Forms.Label
    Friend WithEvents txt_Answer As System.Windows.Forms.TextBox
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel

End Class
