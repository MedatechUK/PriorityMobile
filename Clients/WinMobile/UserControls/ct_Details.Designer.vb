<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ct_Details
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
        Me.lbl_Terms = New System.Windows.Forms.Label
        Me.lbl_ServTerms = New System.Windows.Forms.Label
        Me.lbl_Requested = New System.Windows.Forms.Label
        Me.lbl_ReqDate = New System.Windows.Forms.Label
        Me.txt_CallDetail = New System.Windows.Forms.WebBrowser
        Me.SuspendLayout()
        '
        'lbl_Terms
        '
        Me.lbl_Terms.Location = New System.Drawing.Point(7, 17)
        Me.lbl_Terms.Name = "lbl_Terms"
        Me.lbl_Terms.Size = New System.Drawing.Size(110, 13)
        Me.lbl_Terms.Text = "Service Terms:"
        '
        'lbl_ServTerms
        '
        Me.lbl_ServTerms.BackColor = System.Drawing.SystemColors.Control
        Me.lbl_ServTerms.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lbl_ServTerms.Location = New System.Drawing.Point(113, 17)
        Me.lbl_ServTerms.Name = "lbl_ServTerms"
        Me.lbl_ServTerms.Size = New System.Drawing.Size(34, 10)
        Me.lbl_ServTerms.Text = "123"
        '
        'lbl_Requested
        '
        Me.lbl_Requested.Location = New System.Drawing.Point(3, 1)
        Me.lbl_Requested.Name = "lbl_Requested"
        Me.lbl_Requested.Size = New System.Drawing.Size(114, 16)
        Me.lbl_Requested.Text = "Call Requested:"
        '
        'lbl_ReqDate
        '
        Me.lbl_ReqDate.BackColor = System.Drawing.SystemColors.Control
        Me.lbl_ReqDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lbl_ReqDate.Location = New System.Drawing.Point(113, 1)
        Me.lbl_ReqDate.Name = "lbl_ReqDate"
        Me.lbl_ReqDate.Size = New System.Drawing.Size(34, 10)
        Me.lbl_ReqDate.Text = "123"
        '
        'txt_CallDetail
        '
        Me.txt_CallDetail.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.txt_CallDetail.Location = New System.Drawing.Point(0, 33)
        Me.txt_CallDetail.Name = "txt_CallDetail"
        Me.txt_CallDetail.Size = New System.Drawing.Size(150, 117)
        '
        'ct_Details
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.Controls.Add(Me.lbl_Terms)
        Me.Controls.Add(Me.lbl_ServTerms)
        Me.Controls.Add(Me.lbl_Requested)
        Me.Controls.Add(Me.lbl_ReqDate)
        Me.Controls.Add(Me.txt_CallDetail)
        Me.Name = "ct_Details"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lbl_Terms As System.Windows.Forms.Label
    Friend WithEvents lbl_ServTerms As System.Windows.Forms.Label
    Friend WithEvents lbl_Requested As System.Windows.Forms.Label
    Friend WithEvents lbl_ReqDate As System.Windows.Forms.Label
    Friend WithEvents txt_CallDetail As System.Windows.Forms.WebBrowser

End Class
