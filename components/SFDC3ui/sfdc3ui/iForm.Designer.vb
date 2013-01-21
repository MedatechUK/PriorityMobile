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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(iForm))
        Me.thisForm = New sfdc3ui.ctrlForm
        Me.thisTable = New sfdc3ui.ctrlTable
        Me.SuspendLayout()
        '
        'thisForm
        '
        Me.thisForm.Location = New System.Drawing.Point(0, 0)
        Me.thisForm.Name = "thisForm"
        Me.thisForm.Size = New System.Drawing.Size(150, 150)
        Me.thisForm.TabIndex = 0
        '
        'thisTable
        '
        Me.thisTable.Location = New System.Drawing.Point(0, 185)
        Me.thisTable.Name = "thisTable"
        Me.thisTable.Size = New System.Drawing.Size(150, 150)
        Me.thisTable.TabIndex = 1
        '
        'iForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(284, 335)
        Me.Controls.Add(Me.thisTable)
        Me.Controls.Add(Me.thisForm)
        Me.Name = "iForm"
        Me.Text = "iForm"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents thisForm As sfdc3ui.ctrlForm
    Friend WithEvents thisTable As sfdc3ui.ctrlTable
End Class
