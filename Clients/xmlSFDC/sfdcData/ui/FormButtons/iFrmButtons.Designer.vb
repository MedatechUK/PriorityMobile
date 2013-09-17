<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class iFrmButtons
    Inherits PrioritySFDC.iFormChild ' PrioritySFDC.iFormChild

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(iFrmButtons))
        Me.btn_Add = New System.Windows.Forms.PictureBox
        Me.btnEnabled = New System.Windows.Forms.ImageList
        Me.btnDisabled = New System.Windows.Forms.ImageList
        Me.btn_Edit = New System.Windows.Forms.PictureBox
        Me.btn_Copy = New System.Windows.Forms.PictureBox
        Me.btn_Delete = New System.Windows.Forms.PictureBox
        Me.btn_Print = New System.Windows.Forms.PictureBox
        Me.btn_Post = New System.Windows.Forms.PictureBox
        Me.SuspendLayout()
        '
        'btn_Add
        '
        Me.btn_Add.Location = New System.Drawing.Point(5, 1)
        Me.btn_Add.Name = "btn_Add"
        Me.btn_Add.Size = New System.Drawing.Size(32, 32)
        Me.btn_Add.Tag = "0"
        '
        'btnEnabled
        '
        Me.btnEnabled.ImageSize = New System.Drawing.Size(64, 64)
        Me.btnEnabled.Images.Clear()
        Me.btnEnabled.Images.Add(CType(resources.GetObject("resource"), System.Drawing.Image))
        Me.btnEnabled.Images.Add(CType(resources.GetObject("resource1"), System.Drawing.Image))
        Me.btnEnabled.Images.Add(CType(resources.GetObject("resource2"), System.Drawing.Image))
        Me.btnEnabled.Images.Add(CType(resources.GetObject("resource3"), System.Drawing.Image))
        Me.btnEnabled.Images.Add(CType(resources.GetObject("resource4"), System.Drawing.Image))
        Me.btnEnabled.Images.Add(CType(resources.GetObject("resource5"), System.Drawing.Image))
        '
        'btnDisabled
        '
        Me.btnDisabled.ImageSize = New System.Drawing.Size(64, 64)
        Me.btnDisabled.Images.Clear()
        Me.btnDisabled.Images.Add(CType(resources.GetObject("resource6"), System.Drawing.Image))
        Me.btnDisabled.Images.Add(CType(resources.GetObject("resource7"), System.Drawing.Image))
        Me.btnDisabled.Images.Add(CType(resources.GetObject("resource8"), System.Drawing.Image))
        Me.btnDisabled.Images.Add(CType(resources.GetObject("resource9"), System.Drawing.Image))
        Me.btnDisabled.Images.Add(CType(resources.GetObject("resource10"), System.Drawing.Image))
        Me.btnDisabled.Images.Add(CType(resources.GetObject("resource11"), System.Drawing.Image))
        '
        'btn_Edit
        '
        Me.btn_Edit.Location = New System.Drawing.Point(41, 1)
        Me.btn_Edit.Name = "btn_Edit"
        Me.btn_Edit.Size = New System.Drawing.Size(32, 32)
        Me.btn_Edit.Tag = "1"
        '
        'btn_Copy
        '
        Me.btn_Copy.Location = New System.Drawing.Point(77, 1)
        Me.btn_Copy.Name = "btn_Copy"
        Me.btn_Copy.Size = New System.Drawing.Size(32, 32)
        Me.btn_Copy.Tag = "2"
        '
        'btn_Delete
        '
        Me.btn_Delete.Location = New System.Drawing.Point(149, 1)
        Me.btn_Delete.Name = "btn_Delete"
        Me.btn_Delete.Size = New System.Drawing.Size(32, 32)
        Me.btn_Delete.Tag = "3"
        '
        'btn_Print
        '
        Me.btn_Print.Location = New System.Drawing.Point(113, 1)
        Me.btn_Print.Name = "btn_Print"
        Me.btn_Print.Size = New System.Drawing.Size(32, 32)
        Me.btn_Print.Tag = "4"
        '
        'btn_Post
        '
        Me.btn_Post.Location = New System.Drawing.Point(185, 1)
        Me.btn_Post.Name = "btn_Post"
        Me.btn_Post.Size = New System.Drawing.Size(32, 32)
        Me.btn_Post.Tag = "5"
        '
        'iFrmButtons
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.btn_Post)
        Me.Controls.Add(Me.btn_Print)
        Me.Controls.Add(Me.btn_Delete)
        Me.Controls.Add(Me.btn_Copy)
        Me.Controls.Add(Me.btn_Edit)
        Me.Controls.Add(Me.btn_Add)
        Me.Name = "iFrmButtons"
        Me.Size = New System.Drawing.Size(220, 34)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btn_Add As System.Windows.Forms.PictureBox
    Friend WithEvents btnEnabled As System.Windows.Forms.ImageList
    Friend WithEvents btnDisabled As System.Windows.Forms.ImageList
    Friend WithEvents btn_Edit As System.Windows.Forms.PictureBox
    Friend WithEvents btn_Copy As System.Windows.Forms.PictureBox
    Friend WithEvents btn_Delete As System.Windows.Forms.PictureBox
    Friend WithEvents btn_Print As System.Windows.Forms.PictureBox
    Friend WithEvents btn_Post As System.Windows.Forms.PictureBox


End Class
