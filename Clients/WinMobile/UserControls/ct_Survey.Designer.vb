<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ct_Survey
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
        Me.pnl_TextAnswer = New System.Windows.Forms.Panel
        Me.txt_Answer = New System.Windows.Forms.TextBox
        Me.lbl_Question = New System.Windows.Forms.Label
        Me.pnl_SelectSurvey = New System.Windows.Forms.Panel
        Me.lst_Survey = New System.Windows.Forms.ListView
        Me.ColSurvey = New System.Windows.Forms.ColumnHeader
        Me.ColHasData = New System.Windows.Forms.ColumnHeader
        Me.pnl_TextAnswer.SuspendLayout()
        Me.pnl_SelectSurvey.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnl_Survey
        '
        Me.pnl_Survey.AutoScroll = True
        Me.pnl_Survey.Location = New System.Drawing.Point(275, 139)
        Me.pnl_Survey.Name = "pnl_Survey"
        Me.pnl_Survey.Size = New System.Drawing.Size(79, 46)
        Me.pnl_Survey.Visible = False
        '
        'pnl_TextAnswer
        '
        Me.pnl_TextAnswer.Controls.Add(Me.txt_Answer)
        Me.pnl_TextAnswer.Controls.Add(Me.lbl_Question)
        Me.pnl_TextAnswer.Location = New System.Drawing.Point(59, 157)
        Me.pnl_TextAnswer.Name = "pnl_TextAnswer"
        Me.pnl_TextAnswer.Size = New System.Drawing.Size(104, 92)
        Me.pnl_TextAnswer.Visible = False
        '
        'txt_Answer
        '
        Me.txt_Answer.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.txt_Answer.Location = New System.Drawing.Point(0, 51)
        Me.txt_Answer.Multiline = True
        Me.txt_Answer.Name = "txt_Answer"
        Me.txt_Answer.Size = New System.Drawing.Size(104, 41)
        Me.txt_Answer.TabIndex = 8
        '
        'lbl_Question
        '
        Me.lbl_Question.Location = New System.Drawing.Point(4, 4)
        Me.lbl_Question.Name = "lbl_Question"
        Me.lbl_Question.Size = New System.Drawing.Size(39, 13)
        Me.lbl_Question.Text = "Label1"
        '
        'pnl_SelectSurvey
        '
        Me.pnl_SelectSurvey.Controls.Add(Me.lst_Survey)
        Me.pnl_SelectSurvey.Location = New System.Drawing.Point(43, 20)
        Me.pnl_SelectSurvey.Name = "pnl_SelectSurvey"
        Me.pnl_SelectSurvey.Size = New System.Drawing.Size(209, 98)
        '
        'lst_Survey
        '
        Me.lst_Survey.Activation = System.Windows.Forms.ItemActivation.OneClick
        Me.lst_Survey.Columns.Add(Me.ColSurvey)
        Me.lst_Survey.Columns.Add(Me.ColHasData)
        Me.lst_Survey.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lst_Survey.Location = New System.Drawing.Point(0, 0)
        Me.lst_Survey.Name = "lst_Survey"
        Me.lst_Survey.Size = New System.Drawing.Size(209, 98)
        Me.lst_Survey.TabIndex = 1
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
        Me.ColHasData.Width = 64
        '
        'ct_Survey
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.pnl_Survey)
        Me.Controls.Add(Me.pnl_TextAnswer)
        Me.Controls.Add(Me.pnl_SelectSurvey)
        Me.Name = "ct_Survey"
        Me.Size = New System.Drawing.Size(399, 326)
        Me.pnl_TextAnswer.ResumeLayout(False)
        Me.pnl_SelectSurvey.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnl_Survey As System.Windows.Forms.Panel
    Friend WithEvents pnl_TextAnswer As System.Windows.Forms.Panel
    Friend WithEvents pnl_SelectSurvey As System.Windows.Forms.Panel
    Friend WithEvents lst_Survey As System.Windows.Forms.ListView
    Friend WithEvents ColSurvey As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColHasData As System.Windows.Forms.ColumnHeader
    Friend WithEvents txt_Answer As System.Windows.Forms.TextBox
    Friend WithEvents lbl_Question As System.Windows.Forms.Label

End Class
