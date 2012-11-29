<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctrl_Warehouse
    Inherits ViewControl.iView

    'UserControl overrides dispose to clean up the component list.
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
        Me.Parts = New System.Windows.Forms.DataGridView
        CType(Me.Parts, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Parts
        '
        Me.Parts.AllowUserToAddRows = False
        Me.Parts.AllowUserToDeleteRows = False
        Me.Parts.AllowUserToOrderColumns = True
        Me.Parts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Parts.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Parts.Location = New System.Drawing.Point(0, 0)
        Me.Parts.MultiSelect = False
        Me.Parts.Name = "Parts"
        Me.Parts.ReadOnly = True
        Me.Parts.RowHeadersVisible = False
        Me.Parts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.Parts.Size = New System.Drawing.Size(150, 150)
        Me.Parts.TabIndex = 1
        '
        'ctrl_Warehouse
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Parts)
        Me.Name = "ctrl_Warehouse"
        CType(Me.Parts, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Parts As System.Windows.Forms.DataGridView

End Class
