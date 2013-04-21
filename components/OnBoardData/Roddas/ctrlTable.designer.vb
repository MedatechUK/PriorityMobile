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
        Me.ImageList1 = New System.Windows.Forms.ImageList
        Me.BtnPanel = New System.Windows.Forms.Panel
        Me.btn_Approve = New System.Windows.Forms.PictureBox
        Me.btn_Delete = New System.Windows.Forms.PictureBox
        Me.btn_Copy = New System.Windows.Forms.PictureBox
        Me.btn_Edit = New System.Windows.Forms.PictureBox
        Me.btn_Add = New System.Windows.Forms.PictureBox
        Me.btn = New System.Windows.Forms.ImageList
        Me.btn_Grey = New System.Windows.Forms.ImageList
        Me.btn_red = New System.Windows.Forms.ImageList
        Me.BtnPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'Table
        '
        Me.Table.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Table.Font = New System.Drawing.Font("Arial", 11.0!, System.Drawing.FontStyle.Regular)
        Me.Table.FullRowSelect = True
        Me.Table.Location = New System.Drawing.Point(0, 127)
        Me.Table.Name = "Table"
        Me.Table.Size = New System.Drawing.Size(201, 66)
        Me.Table.TabIndex = 2
        Me.Table.View = System.Windows.Forms.View.Details
        '
        'Panel
        '
        Me.Panel.Location = New System.Drawing.Point(172, 151)
        Me.Panel.Name = "Panel"
        Me.Panel.Size = New System.Drawing.Size(10, 10)
        Me.Panel.Visible = False
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
        'BtnPanel
        '
        Me.BtnPanel.BackColor = System.Drawing.Color.LightSteelBlue
        Me.BtnPanel.Controls.Add(Me.btn_Approve)
        Me.BtnPanel.Controls.Add(Me.btn_Delete)
        Me.BtnPanel.Controls.Add(Me.btn_Copy)
        Me.BtnPanel.Controls.Add(Me.btn_Edit)
        Me.BtnPanel.Controls.Add(Me.btn_Add)
        Me.BtnPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.BtnPanel.Location = New System.Drawing.Point(0, 0)
        Me.BtnPanel.Name = "BtnPanel"
        Me.BtnPanel.Size = New System.Drawing.Size(201, 34)
        '
        'btn_Approve
        '
        Me.btn_Approve.Image = CType(resources.GetObject("btn_Approve.Image"), System.Drawing.Image)
        Me.btn_Approve.Location = New System.Drawing.Point(140, 0)
        Me.btn_Approve.Name = "btn_Approve"
        Me.btn_Approve.Size = New System.Drawing.Size(32, 32)
        '
        'btn_Delete
        '
        Me.btn_Delete.Image = CType(resources.GetObject("btn_Delete.Image"), System.Drawing.Image)
        Me.btn_Delete.Location = New System.Drawing.Point(105, 0)
        Me.btn_Delete.Name = "btn_Delete"
        Me.btn_Delete.Size = New System.Drawing.Size(32, 32)
        '
        'btn_Copy
        '
        Me.btn_Copy.Image = CType(resources.GetObject("btn_Copy.Image"), System.Drawing.Image)
        Me.btn_Copy.Location = New System.Drawing.Point(70, 0)
        Me.btn_Copy.Name = "btn_Copy"
        Me.btn_Copy.Size = New System.Drawing.Size(32, 32)
        '
        'btn_Edit
        '
        Me.btn_Edit.Image = CType(resources.GetObject("btn_Edit.Image"), System.Drawing.Image)
        Me.btn_Edit.Location = New System.Drawing.Point(35, 0)
        Me.btn_Edit.Name = "btn_Edit"
        Me.btn_Edit.Size = New System.Drawing.Size(32, 32)
        '
        'btn_Add
        '
        Me.btn_Add.Image = CType(resources.GetObject("btn_Add.Image"), System.Drawing.Image)
        Me.btn_Add.Location = New System.Drawing.Point(0, 0)
        Me.btn_Add.Name = "btn_Add"
        Me.btn_Add.Size = New System.Drawing.Size(32, 32)
        '
        'btn
        '
        Me.btn.ImageSize = New System.Drawing.Size(64, 64)
        Me.btn.Images.Clear()
        Me.btn.Images.Add(CType(resources.GetObject("resource5"), System.Drawing.Image))
        Me.btn.Images.Add(CType(resources.GetObject("resource6"), System.Drawing.Image))
        Me.btn.Images.Add(CType(resources.GetObject("resource7"), System.Drawing.Image))
        Me.btn.Images.Add(CType(resources.GetObject("resource8"), System.Drawing.Image))
        Me.btn.Images.Add(CType(resources.GetObject("resource9"), System.Drawing.Image))
        '
        'btn_Grey
        '
        Me.btn_Grey.ImageSize = New System.Drawing.Size(64, 64)
        Me.btn_Grey.Images.Clear()
        Me.btn_Grey.Images.Add(CType(resources.GetObject("resource10"), System.Drawing.Image))
        Me.btn_Grey.Images.Add(CType(resources.GetObject("resource11"), System.Drawing.Image))
        Me.btn_Grey.Images.Add(CType(resources.GetObject("resource12"), System.Drawing.Image))
        Me.btn_Grey.Images.Add(CType(resources.GetObject("resource13"), System.Drawing.Image))
        Me.btn_Grey.Images.Add(CType(resources.GetObject("resource14"), System.Drawing.Image))
        '
        'btn_red
        '
        Me.btn_red.ImageSize = New System.Drawing.Size(64, 64)
        Me.btn_red.Images.Clear()
        Me.btn_red.Images.Add(CType(resources.GetObject("resource15"), System.Drawing.Image))
        Me.btn_red.Images.Add(CType(resources.GetObject("resource16"), System.Drawing.Image))
        Me.btn_red.Images.Add(CType(resources.GetObject("resource17"), System.Drawing.Image))
        Me.btn_red.Images.Add(CType(resources.GetObject("resource18"), System.Drawing.Image))
        Me.btn_red.Images.Add(CType(resources.GetObject("resource19"), System.Drawing.Image))
        '
        'CtrlTable
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.LightSteelBlue
        Me.Controls.Add(Me.BtnPanel)
        Me.Controls.Add(Me.Panel)
        Me.Controls.Add(Me.Table)
        Me.Name = "CtrlTable"
        Me.Size = New System.Drawing.Size(201, 193)
        Me.BtnPanel.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel As System.Windows.Forms.Panel
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Public WithEvents Table As System.Windows.Forms.ListView
    Friend WithEvents BtnPanel As System.Windows.Forms.Panel
    Friend WithEvents btn_Approve As System.Windows.Forms.PictureBox
    Friend WithEvents btn_Delete As System.Windows.Forms.PictureBox
    Friend WithEvents btn_Copy As System.Windows.Forms.PictureBox
    Friend WithEvents btn_Edit As System.Windows.Forms.PictureBox
    Friend WithEvents btn_Add As System.Windows.Forms.PictureBox
    Friend WithEvents btn As System.Windows.Forms.ImageList
    Friend WithEvents btn_Grey As System.Windows.Forms.ImageList
    Friend WithEvents btn_red As System.Windows.Forms.ImageList

End Class
