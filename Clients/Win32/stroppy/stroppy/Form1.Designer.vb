<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.MyTreeView = New System.Windows.Forms.TreeView
        Me.MyTreeView2 = New System.Windows.Forms.TreeView
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.MyTreeView)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.MyTreeView2)
        Me.SplitContainer1.Size = New System.Drawing.Size(688, 352)
        Me.SplitContainer1.SplitterDistance = 345
        Me.SplitContainer1.TabIndex = 0
        '
        'MyTreeView
        '
        Me.MyTreeView.AllowDrop = True
        Me.MyTreeView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MyTreeView.Location = New System.Drawing.Point(0, 0)
        Me.MyTreeView.Name = "MyTreeView"
        Me.MyTreeView.Size = New System.Drawing.Size(345, 352)
        Me.MyTreeView.TabIndex = 0
        '
        'MyTreeView2
        '
        Me.MyTreeView2.AllowDrop = True
        Me.MyTreeView2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MyTreeView2.Location = New System.Drawing.Point(0, 0)
        Me.MyTreeView2.Name = "MyTreeView2"
        Me.MyTreeView2.Size = New System.Drawing.Size(339, 352)
        Me.MyTreeView2.TabIndex = 0
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(688, 352)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents MyTreeView As System.Windows.Forms.TreeView
    Friend WithEvents MyTreeView2 As System.Windows.Forms.TreeView

End Class
