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
        Me.FormView = New sfdc3.FormView
        Me.TableView = New sfdc3.TableView
        Me.SuspendLayout()
        '
        'FormView
        '
        Me.FormView.AutoScroll = True
        Me.FormView.Dock = System.Windows.Forms.DockStyle.Top
        Me.FormView.Location = New System.Drawing.Point(0, 0)
        Me.FormView.Name = "FormView"
        Me.FormView.Size = New System.Drawing.Size(638, 150)
        Me.FormView.TabIndex = 0
        Me.FormView.ViewForm = Nothing
        '
        'TableView
        '
        Me.TableView.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.TableView.Location = New System.Drawing.Point(0, 305)
        Me.TableView.Name = "TableView"
        Me.TableView.Size = New System.Drawing.Size(638, 150)
        Me.TableView.TabIndex = 1
        Me.TableView.TableView = sfdc3.TableView.eTableView.vTable
        Me.TableView.ViewForm = Nothing
        Me.TableView.ViewTable = Nothing
        '
        'iForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(638, 455)
        Me.Controls.Add(Me.TableView)
        Me.Controls.Add(Me.FormView)
        Me.Name = "iForm"
        Me.Text = "iForm"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents FormView As sfdc3.FormView
    Friend WithEvents TableView As sfdc3.TableView
End Class
