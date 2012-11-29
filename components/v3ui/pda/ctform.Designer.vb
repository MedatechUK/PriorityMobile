<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctform
    Inherits System.Windows.Forms.UserControl

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
        Me.components = New System.ComponentModel.Container
        Me.UserText = New System.Windows.Forms.TextBox
        Me.WebBrowser = New System.Windows.Forms.WebBrowser
        Me.DataGrid = New System.Windows.Forms.DataGrid
        Me.BindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.PriDateSel = New uiCtrls.priDateSel
        Me.TabForm = New uiCtrls.priTabs
        Me.PriSign = New uiCtrls.priSign
        Me.SubForms = New uiCtrls.SlideMenu
        Me.SuspendLayout()
        '
        'UserText
        '
        Me.UserText.Location = New System.Drawing.Point(30, 24)
        Me.UserText.Multiline = True
        Me.UserText.Name = "UserText"
        Me.UserText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.UserText.Size = New System.Drawing.Size(150, 52)
        Me.UserText.TabIndex = 1
        Me.UserText.Visible = False
        '
        'WebBrowser
        '
        Me.WebBrowser.Location = New System.Drawing.Point(30, 88)
        Me.WebBrowser.Name = "WebBrowser"
        Me.WebBrowser.Size = New System.Drawing.Size(150, 86)
        Me.WebBrowser.Visible = False
        '
        'DataGrid
        '
        Me.DataGrid.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.DataGrid.GridLineColor = System.Drawing.SystemColors.ActiveBorder
        Me.DataGrid.Location = New System.Drawing.Point(361, 192)
        Me.DataGrid.Name = "DataGrid"
        Me.DataGrid.Size = New System.Drawing.Size(138, 96)
        Me.DataGrid.TabIndex = 10
        Me.DataGrid.Visible = False
        '
        'BindingSource
        '
        '
        'PriDateSel
        '
        Me.PriDateSel.Location = New System.Drawing.Point(186, 24)
        Me.PriDateSel.Name = "PriDateSel"
        Me.PriDateSel.Size = New System.Drawing.Size(150, 150)
        Me.PriDateSel.TabIndex = 12
        Me.PriDateSel.Visible = False
        '
        'TabForm
        '
        Me.TabForm.BackColor = System.Drawing.Color.White
        Me.TabForm.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Regular)
        Me.TabForm.Location = New System.Drawing.Point(30, 192)
        Me.TabForm.Name = "TabForm"
        Me.TabForm.Size = New System.Drawing.Size(207, 114)
        Me.TabForm.TabIndex = 9
        Me.TabForm.Visible = False
        '
        'PriSign
        '
        Me.PriSign.Location = New System.Drawing.Point(398, 34)
        Me.PriSign.Name = "PriSign"
        Me.PriSign.Size = New System.Drawing.Size(150, 110)
        Me.PriSign.TabIndex = 5
        Me.PriSign.Visible = False
        '
        'SubForms
        '
        Me.SubForms.BackColor = System.Drawing.Color.SkyBlue
        Me.SubForms.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.SubForms.Location = New System.Drawing.Point(0, 343)
        Me.SubForms.MenuFont = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Regular)
        Me.SubForms.Name = "SubForms"
        Me.SubForms.Size = New System.Drawing.Size(560, 20)
        Me.SubForms.TabIndex = 1
        '
        'ctform
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.Controls.Add(Me.PriDateSel)
        Me.Controls.Add(Me.DataGrid)
        Me.Controls.Add(Me.TabForm)
        Me.Controls.Add(Me.PriSign)
        Me.Controls.Add(Me.WebBrowser)
        Me.Controls.Add(Me.UserText)
        Me.Controls.Add(Me.SubForms)
        Me.Name = "ctform"
        Me.Size = New System.Drawing.Size(560, 363)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SubForms As SlideMenu
    Public WithEvents BindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents UserText As System.Windows.Forms.TextBox
    Friend WithEvents WebBrowser As System.Windows.Forms.WebBrowser
    Friend WithEvents PriSign As priSign
    Friend WithEvents TabForm As priTabs
    Public WithEvents DataGrid As System.Windows.Forms.DataGrid
    Friend WithEvents PriDateSel As uiCtrls.priDateSel


End Class
