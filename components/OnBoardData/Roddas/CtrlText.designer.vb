<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ctrlText
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
        Me.DataEntry = New System.Windows.Forms.TextBox
        Me.Value = New System.Windows.Forms.Label
        Me.Title = New System.Windows.Forms.LinkLabel
        Me.SuspendLayout()
        '
        'DataEntry
        '
        Me.DataEntry.BackColor = System.Drawing.Color.Red
        Me.DataEntry.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.DataEntry.Location = New System.Drawing.Point(181, 0)
        Me.DataEntry.Name = "DataEntry"
        Me.DataEntry.Size = New System.Drawing.Size(100, 23)
        Me.DataEntry.TabIndex = 0
        Me.DataEntry.Visible = False
        '
        'Value
        '
        Me.Value.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Value.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.Value.Location = New System.Drawing.Point(93, 0)
        Me.Value.Name = "Value"
        Me.Value.Size = New System.Drawing.Size(56, 23)
        Me.Value.Text = "Label1"
        '
        'Title
        '
        Me.Title.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Title.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular)
        Me.Title.Location = New System.Drawing.Point(0, 0)
        Me.Title.Name = "Title"
        Me.Title.Size = New System.Drawing.Size(77, 23)
        Me.Title.TabIndex = 1
        Me.Title.Text = "LinkLabel1"
        '
        'ctrlText
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.LightSteelBlue
        Me.Controls.Add(Me.DataEntry)
        Me.Controls.Add(Me.Value)
        Me.Controls.Add(Me.Title)
        Me.Name = "ctrlText"
        Me.Size = New System.Drawing.Size(348, 23)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Title As System.Windows.Forms.LinkLabel
    Public WithEvents DataEntry As System.Windows.Forms.TextBox
    Public WithEvents Value As System.Windows.Forms.Label

End Class
