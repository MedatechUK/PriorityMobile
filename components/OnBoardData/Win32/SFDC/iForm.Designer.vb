<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class iForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(iForm))
        Me.CtrlTable = New w32SFDCData.CtrlTable
        Me.CtrlForm = New w32SFDCData.CtrlForm
        Me.SuspendLayout()
        '
        'CtrlTable
        '
        Me.CtrlTable.EditInPlace = True
        Me.CtrlTable.FieldHeight = 0
        Me.CtrlTable.FromRS = False
        Me.CtrlTable.Location = New System.Drawing.Point(6, 45)
        Me.CtrlTable.Name = "CtrlTable"
        Me.CtrlTable.RecordsSQL = Nothing
        Me.CtrlTable.Size = New System.Drawing.Size(268, 180)
        Me.CtrlTable.TabIndex = 1
        '
        'CtrlForm
        '
        Me.CtrlForm.FieldHeight = 0
        Me.CtrlForm.Location = New System.Drawing.Point(6, 2)
        Me.CtrlForm.Name = "CtrlForm"
        Me.CtrlForm.Size = New System.Drawing.Size(268, 37)
        Me.CtrlForm.TabIndex = 0
        '
        'iForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(280, 237)
        Me.Controls.Add(Me.CtrlTable)
        Me.Controls.Add(Me.CtrlForm)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "iForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "iForm"
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents CtrlForm As w32SFDCData.CtrlForm
    Public WithEvents CtrlTable As w32SFDCData.CtrlTable
End Class
