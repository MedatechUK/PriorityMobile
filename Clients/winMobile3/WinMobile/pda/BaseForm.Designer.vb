<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BaseForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BaseForm))
        Me.ToolBar1 = New System.Windows.Forms.ToolBar
        Me.ToolBarButton1 = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton2 = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton3 = New System.Windows.Forms.ToolBarButton
        Me.sep1 = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton4 = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton5 = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton6 = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton7 = New System.Windows.Forms.ToolBarButton
        Me.sep2 = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton8 = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton9 = New System.Windows.Forms.ToolBarButton
        Me.Buttons = New System.Windows.Forms.ImageList
        Me.btnUp = New System.Windows.Forms.ToolBarButton
        Me.btnView = New System.Windows.Forms.ToolBarButton
        Me.btnSync = New System.Windows.Forms.ToolBarButton
        Me.btnFirst = New System.Windows.Forms.ToolBarButton
        Me.btnBack = New System.Windows.Forms.ToolBarButton
        Me.btnNext = New System.Windows.Forms.ToolBarButton
        Me.btnLast = New System.Windows.Forms.ToolBarButton
        Me.btnAdd = New System.Windows.Forms.ToolBarButton
        Me.btnDelete = New System.Windows.Forms.ToolBarButton
        Me.FormPanel = New System.Windows.Forms.Panel
        Me.SyncMenu = New System.Windows.Forms.ContextMenu
        Me.MenuPanel = New System.Windows.Forms.Panel
        Me.PanelRight = New System.Windows.Forms.Panel
        Me.ProgressPanel = New System.Windows.Forms.Panel
        Me.lbl_Detail = New System.Windows.Forms.Label
        Me.lbl_Title = New System.Windows.Forms.Label
        Me.ProgressBar = New System.Windows.Forms.ProgressBar
        Me.PanelModuleList = New System.Windows.Forms.Panel
        Me.bttnStart = New System.Windows.Forms.Button
        Me.lstModules = New System.Windows.Forms.ListBox
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtVersion = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.MenuPanel.SuspendLayout()
        Me.PanelRight.SuspendLayout()
        Me.ProgressPanel.SuspendLayout()
        Me.PanelModuleList.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolBar1
        '
        Me.ToolBar1.Buttons.Add(Me.ToolBarButton1)
        Me.ToolBar1.Buttons.Add(Me.ToolBarButton2)
        Me.ToolBar1.Buttons.Add(Me.ToolBarButton3)
        Me.ToolBar1.Buttons.Add(Me.sep1)
        Me.ToolBar1.Buttons.Add(Me.ToolBarButton4)
        Me.ToolBar1.Buttons.Add(Me.ToolBarButton5)
        Me.ToolBar1.Buttons.Add(Me.ToolBarButton6)
        Me.ToolBar1.Buttons.Add(Me.ToolBarButton7)
        Me.ToolBar1.Buttons.Add(Me.sep2)
        Me.ToolBar1.Buttons.Add(Me.ToolBarButton8)
        Me.ToolBar1.Buttons.Add(Me.ToolBarButton9)
        Me.ToolBar1.ImageList = Me.Buttons
        Me.ToolBar1.Name = "ToolBar1"
        '
        'ToolBarButton1
        '
        Me.ToolBarButton1.ImageIndex = 0
        Me.ToolBarButton1.Pushed = True
        Me.ToolBarButton1.Tag = "up"
        '
        'ToolBarButton2
        '
        Me.ToolBarButton2.ImageIndex = 1
        Me.ToolBarButton2.Tag = "view"
        '
        'ToolBarButton3
        '
        Me.ToolBarButton3.ImageIndex = 2
        Me.ToolBarButton3.Tag = "sync"
        '
        'sep1
        '
        Me.sep1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'ToolBarButton4
        '
        Me.ToolBarButton4.ImageIndex = 3
        Me.ToolBarButton4.Tag = "first"
        '
        'ToolBarButton5
        '
        Me.ToolBarButton5.ImageIndex = 4
        Me.ToolBarButton5.Tag = "back"
        '
        'ToolBarButton6
        '
        Me.ToolBarButton6.ImageIndex = 5
        Me.ToolBarButton6.Tag = "next"
        '
        'ToolBarButton7
        '
        Me.ToolBarButton7.ImageIndex = 6
        Me.ToolBarButton7.Tag = "last"
        '
        'sep2
        '
        Me.sep2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'ToolBarButton8
        '
        Me.ToolBarButton8.ImageIndex = 7
        Me.ToolBarButton8.Pushed = True
        Me.ToolBarButton8.Tag = "delete"
        '
        'ToolBarButton9
        '
        Me.ToolBarButton9.ImageIndex = 8
        Me.ToolBarButton9.Tag = "add"
        Me.Buttons.Images.Clear()
        Me.Buttons.Images.Add(CType(resources.GetObject("resource"), System.Drawing.Image))
        Me.Buttons.Images.Add(CType(resources.GetObject("resource1"), System.Drawing.Image))
        Me.Buttons.Images.Add(CType(resources.GetObject("resource2"), System.Drawing.Image))
        Me.Buttons.Images.Add(CType(resources.GetObject("resource3"), System.Drawing.Image))
        Me.Buttons.Images.Add(CType(resources.GetObject("resource4"), System.Drawing.Image))
        Me.Buttons.Images.Add(CType(resources.GetObject("resource5"), System.Drawing.Image))
        Me.Buttons.Images.Add(CType(resources.GetObject("resource6"), System.Drawing.Image))
        Me.Buttons.Images.Add(CType(resources.GetObject("resource7"), System.Drawing.Image))
        Me.Buttons.Images.Add(CType(resources.GetObject("resource8"), System.Drawing.Image))
        Me.Buttons.Images.Add(CType(resources.GetObject("resource9"), System.Drawing.Icon))
        Me.Buttons.Images.Add(CType(resources.GetObject("resource10"), System.Drawing.Icon))
        '
        'FormPanel
        '
        Me.FormPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FormPanel.Location = New System.Drawing.Point(0, 0)
        Me.FormPanel.Name = "FormPanel"
        Me.FormPanel.Size = New System.Drawing.Size(240, 268)
        Me.FormPanel.Visible = False
        '
        'MenuPanel
        '
        Me.MenuPanel.Controls.Add(Me.PanelRight)
        Me.MenuPanel.Controls.Add(Me.PictureBox1)
        Me.MenuPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MenuPanel.Location = New System.Drawing.Point(0, 0)
        Me.MenuPanel.Name = "MenuPanel"
        Me.MenuPanel.Size = New System.Drawing.Size(240, 268)
        '
        'PanelRight
        '
        Me.PanelRight.Controls.Add(Me.ProgressPanel)
        Me.PanelRight.Controls.Add(Me.PanelModuleList)
        Me.PanelRight.Controls.Add(Me.Panel3)
        Me.PanelRight.Controls.Add(Me.Label6)
        Me.PanelRight.Controls.Add(Me.Label3)
        Me.PanelRight.Controls.Add(Me.Label2)
        Me.PanelRight.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelRight.Location = New System.Drawing.Point(56, 0)
        Me.PanelRight.Name = "PanelRight"
        Me.PanelRight.Size = New System.Drawing.Size(184, 268)
        '
        'ProgressPanel
        '
        Me.ProgressPanel.Controls.Add(Me.lbl_Detail)
        Me.ProgressPanel.Controls.Add(Me.lbl_Title)
        Me.ProgressPanel.Controls.Add(Me.ProgressBar)
        Me.ProgressPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.ProgressPanel.Location = New System.Drawing.Point(0, 79)
        Me.ProgressPanel.Name = "ProgressPanel"
        Me.ProgressPanel.Size = New System.Drawing.Size(184, 49)
        '
        'lbl_Detail
        '
        Me.lbl_Detail.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lbl_Detail.Location = New System.Drawing.Point(0, 30)
        Me.lbl_Detail.Name = "lbl_Detail"
        Me.lbl_Detail.Size = New System.Drawing.Size(184, 19)
        '
        'lbl_Title
        '
        Me.lbl_Title.Dock = System.Windows.Forms.DockStyle.Top
        Me.lbl_Title.Location = New System.Drawing.Point(0, 0)
        Me.lbl_Title.Name = "lbl_Title"
        Me.lbl_Title.Size = New System.Drawing.Size(184, 20)
        '
        'ProgressBar
        '
        Me.ProgressBar.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ProgressBar.Location = New System.Drawing.Point(0, 0)
        Me.ProgressBar.Name = "ProgressBar"
        Me.ProgressBar.Size = New System.Drawing.Size(184, 49)
        '
        'PanelModuleList
        '
        Me.PanelModuleList.Controls.Add(Me.bttnStart)
        Me.PanelModuleList.Controls.Add(Me.lstModules)
        Me.PanelModuleList.Location = New System.Drawing.Point(0, 131)
        Me.PanelModuleList.Name = "PanelModuleList"
        Me.PanelModuleList.Size = New System.Drawing.Size(184, 134)
        Me.PanelModuleList.Visible = False
        '
        'bttnStart
        '
        Me.bttnStart.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.bttnStart.Enabled = False
        Me.bttnStart.Location = New System.Drawing.Point(0, 109)
        Me.bttnStart.Name = "bttnStart"
        Me.bttnStart.Size = New System.Drawing.Size(184, 25)
        Me.bttnStart.TabIndex = 11
        Me.bttnStart.Text = "Start"
        '
        'lstModules
        '
        Me.lstModules.Dock = System.Windows.Forms.DockStyle.Top
        Me.lstModules.Location = New System.Drawing.Point(0, 0)
        Me.lstModules.Name = "lstModules"
        Me.lstModules.Size = New System.Drawing.Size(184, 100)
        Me.lstModules.TabIndex = 10
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.Label4)
        Me.Panel3.Controls.Add(Me.txtVersion)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(0, 59)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(184, 20)
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(2, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(82, 14)
        Me.Label4.Text = "XML Version:"
        '
        'txtVersion
        '
        Me.txtVersion.Location = New System.Drawing.Point(90, 0)
        Me.txtVersion.Name = "txtVersion"
        Me.txtVersion.Size = New System.Drawing.Size(66, 14)
        Me.txtVersion.Text = "0.0"
        '
        'Label6
        '
        Me.Label6.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label6.Location = New System.Drawing.Point(0, 41)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(184, 18)
        Me.Label6.Text = "by Simon Barnett"
        '
        'Label3
        '
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label3.Location = New System.Drawing.Point(0, 28)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(184, 13)
        Me.Label3.Text = "(c)2011 eMerge-IT"
        '
        'Label2
        '
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label2.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Bold)
        Me.Label2.Location = New System.Drawing.Point(0, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(184, 28)
        Me.Label2.Text = "Priority Mobile"
        '
        'PictureBox1
        '
        Me.PictureBox1.Dock = System.Windows.Forms.DockStyle.Left
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(56, 268)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        '
        'BaseForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.ControlBox = False
        Me.Controls.Add(Me.MenuPanel)
        Me.Controls.Add(Me.FormPanel)
        Me.Controls.Add(Me.ToolBar1)
        Me.Name = "BaseForm"
        Me.MenuPanel.ResumeLayout(False)
        Me.PanelRight.ResumeLayout(False)
        Me.ProgressPanel.ResumeLayout(False)
        Me.PanelModuleList.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolBar1 As System.Windows.Forms.ToolBar
    Friend WithEvents btnUp As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnView As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnSync As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnAdd As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnFirst As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnBack As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnNext As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnLast As System.Windows.Forms.ToolBarButton
    'Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnDelete As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton1 As System.Windows.Forms.ToolBarButton
    Friend WithEvents Buttons As System.Windows.Forms.ImageList
    Friend WithEvents ToolBarButton2 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton3 As System.Windows.Forms.ToolBarButton
    Friend WithEvents sep1 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton4 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton5 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton6 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton7 As System.Windows.Forms.ToolBarButton
    Friend WithEvents sep2 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton8 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton9 As System.Windows.Forms.ToolBarButton
    Friend WithEvents SyncMenu As System.Windows.Forms.ContextMenu
    Friend WithEvents FormPanel As System.Windows.Forms.Panel
    Friend WithEvents MenuPanel As System.Windows.Forms.Panel
    Friend WithEvents PanelRight As System.Windows.Forms.Panel
    Friend WithEvents PanelModuleList As System.Windows.Forms.Panel
    Friend WithEvents lstModules As System.Windows.Forms.ListBox
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtVersion As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Public WithEvents bttnStart As System.Windows.Forms.Button
    Friend WithEvents ProgressPanel As System.Windows.Forms.Panel
    Friend WithEvents lbl_Detail As System.Windows.Forms.Label
    Friend WithEvents lbl_Title As System.Windows.Forms.Label
    Friend WithEvents ProgressBar As System.Windows.Forms.ProgressBar

End Class
