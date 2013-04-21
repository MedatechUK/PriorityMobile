<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class iForm
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
        Me.MainMenu1 = New System.Windows.Forms.MainMenu
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.CtrlForm = New SFDCData.CtrlForm
        Me.CtrlTable = New SFDCData.CtrlTable
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.Add(Me.MenuItem1)
        '
        'MenuItem1
        '
        Me.MenuItem1.Text = "Close"
        '
        'CtrlForm
        '
        Me.CtrlForm.BackColor = System.Drawing.Color.LightSteelBlue
        Me.CtrlForm.FieldHeight = 0
        Me.CtrlForm.Location = New System.Drawing.Point(2, 2)
        Me.CtrlForm.Name = "CtrlForm"
        Me.CtrlForm.Size = New System.Drawing.Size(180, 26)
        Me.CtrlForm.TabIndex = 1
        '
        'CtrlTable
        '
        Me.CtrlTable.BackColor = System.Drawing.Color.LightSteelBlue
        Me.CtrlTable.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.CtrlTable.EditInPlace = True
        Me.CtrlTable.FieldHeight = 0
        Me.CtrlTable.FromRS = False
        Me.CtrlTable.Location = New System.Drawing.Point(0, 39)
        Me.CtrlTable.Name = "CtrlTable"
        Me.CtrlTable.Size = New System.Drawing.Size(219, 127)
        Me.CtrlTable.TabIndex = 0
        '
        'iForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.Color.LightSteelBlue
        Me.ClientSize = New System.Drawing.Size(219, 166)
        Me.Controls.Add(Me.CtrlForm)
        Me.Controls.Add(Me.CtrlTable)
        Me.MaximizeBox = False
        Me.Menu = Me.MainMenu1
        Me.MinimizeBox = False
        Me.Name = "iForm"
        Me.Text = "iForm"
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents CtrlForm As SFDCData.CtrlForm
    Public WithEvents CtrlTable As SFDCData.CtrlTable
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
End Class
