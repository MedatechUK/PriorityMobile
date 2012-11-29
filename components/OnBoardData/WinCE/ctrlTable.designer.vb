<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class CtrlTable
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CtrlTable))
        Me.Table = New System.Windows.Forms.ListView
        Me.Panel = New System.Windows.Forms.Panel
        Me.ToolStrip = New System.Windows.Forms.ToolBar
        Me.btnAdd = New System.Windows.Forms.ToolBarButton
        Me.sep1 = New System.Windows.Forms.ToolBarButton
        Me.btnEdit = New System.Windows.Forms.ToolBarButton
        Me.sep2 = New System.Windows.Forms.ToolBarButton
        Me.btnCopy = New System.Windows.Forms.ToolBarButton
        Me.sep3 = New System.Windows.Forms.ToolBarButton
        Me.btnDelete = New System.Windows.Forms.ToolBarButton
        Me.sep4 = New System.Windows.Forms.ToolBarButton
        Me.btnFinish = New System.Windows.Forms.ToolBarButton
        Me.ImageList1 = New System.Windows.Forms.ImageList
        Me.SuspendLayout()
        '
        'Table
        '
        Me.Table.Font = New System.Drawing.Font("Arial", 11.0!, System.Drawing.FontStyle.Regular)
        Me.Table.FullRowSelect = True
        Me.Table.Location = New System.Drawing.Point(0, 30)
        Me.Table.Name = "Table"
        Me.Table.Size = New System.Drawing.Size(184, 66)
        Me.Table.TabIndex = 2
        Me.Table.View = System.Windows.Forms.View.Details
        '
        'Panel
        '
        Me.Panel.Location = New System.Drawing.Point(228, 123)
        Me.Panel.Name = "Panel"
        Me.Panel.Size = New System.Drawing.Size(0, 0)
        Me.Panel.Visible = False
        '
        'ToolStrip
        '
        Me.ToolStrip.Buttons.Add(Me.btnAdd)
        Me.ToolStrip.Buttons.Add(Me.sep1)
        Me.ToolStrip.Buttons.Add(Me.btnEdit)
        Me.ToolStrip.Buttons.Add(Me.sep2)
        Me.ToolStrip.Buttons.Add(Me.btnCopy)
        Me.ToolStrip.Buttons.Add(Me.sep3)
        Me.ToolStrip.Buttons.Add(Me.btnDelete)
        Me.ToolStrip.Buttons.Add(Me.sep4)
        Me.ToolStrip.Buttons.Add(Me.btnFinish)
        Me.ToolStrip.ImageList = Me.ImageList1
        Me.ToolStrip.Name = "ToolStrip"
        '
        'btnAdd
        '
        Me.btnAdd.ImageIndex = 0
        Me.btnAdd.ToolTipText = "Add Record"
        '
        'sep1
        '
        Me.sep1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'btnEdit
        '
        Me.btnEdit.ImageIndex = 1
        Me.btnEdit.ToolTipText = "Edit Record"
        '
        'sep2
        '
        Me.sep2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'btnCopy
        '
        Me.btnCopy.ImageIndex = 2
        Me.btnCopy.ToolTipText = "Copy Record"
        '
        'sep3
        '
        Me.sep3.Pushed = True
        Me.sep3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'btnDelete
        '
        Me.btnDelete.ImageIndex = 3
        Me.btnDelete.ToolTipText = "Delete Record"
        '
        'sep4
        '
        Me.sep4.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'btnFinish
        '
        Me.btnFinish.ImageIndex = 4
        Me.btnFinish.ToolTipText = "Finish Editing"
        '
        'ImageList1
        '
        Me.ImageList1.ImageSize = New System.Drawing.Size(32, 32)
        Me.ImageList1.Images.Clear()
        Me.ImageList1.Images.Add(CType(resources.GetObject("resource"), System.Drawing.Icon))
        Me.ImageList1.Images.Add(CType(resources.GetObject("resource1"), System.Drawing.Icon))
        Me.ImageList1.Images.Add(CType(resources.GetObject("resource2"), System.Drawing.Icon))
        Me.ImageList1.Images.Add(CType(resources.GetObject("resource3"), System.Drawing.Icon))
        Me.ImageList1.Images.Add(CType(resources.GetObject("resource4"), System.Drawing.Icon))
        '
        'CtrlTable
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.ToolStrip)
        Me.Controls.Add(Me.Panel)
        Me.Controls.Add(Me.Table)
        Me.Name = "CtrlTable"
        Me.Size = New System.Drawing.Size(282, 193)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel As System.Windows.Forms.Panel
    Friend WithEvents ToolStrip As System.Windows.Forms.ToolBar
    Friend WithEvents btnAdd As System.Windows.Forms.ToolBarButton
    Friend WithEvents sep1 As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnEdit As System.Windows.Forms.ToolBarButton
    Friend WithEvents sep2 As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnCopy As System.Windows.Forms.ToolBarButton
    Friend WithEvents sep3 As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnDelete As System.Windows.Forms.ToolBarButton
    Friend WithEvents sep4 As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnFinish As System.Windows.Forms.ToolBarButton
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Public WithEvents Table As System.Windows.Forms.ListView

End Class
