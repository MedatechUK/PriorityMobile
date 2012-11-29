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
        Me.CtrlForm = New SFDCData.CtrlForm
        Me.CtrlTable = New SFDCData.CtrlTable
        Me.SuspendLayout()
        '
        'CtrlForm
        '
        Me.CtrlForm.FieldHeight = 0
        Me.CtrlForm.Location = New System.Drawing.Point(4, 4)
        Me.CtrlForm.Name = "CtrlForm"
        Me.CtrlForm.Size = New System.Drawing.Size(212, 26)
        Me.CtrlForm.TabIndex = 1
        '
        'CtrlTable
        '
        Me.CtrlTable.EditInPlace = True
        Me.CtrlTable.FieldHeight = 0
        Me.CtrlTable.FromRS = False
        Me.CtrlTable.Location = New System.Drawing.Point(3, 36)
        Me.CtrlTable.Name = "CtrlTable"
        Me.CtrlTable.RecordsSQL = Nothing
        Me.CtrlTable.Size = New System.Drawing.Size(213, 127)
        Me.CtrlTable.TabIndex = 0
        '
        'iForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(219, 166)
        Me.Controls.Add(Me.CtrlForm)
        Me.Controls.Add(Me.CtrlTable)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "iForm"
        Me.Text = "iForm"
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents CtrlForm As SFDCData.CtrlForm
    Public WithEvents CtrlTable As SFDCData.CtrlTable
End Class
