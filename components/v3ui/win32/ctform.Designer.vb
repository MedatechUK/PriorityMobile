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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ctform))
        Me.BindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.UserText = New System.Windows.Forms.TextBox
        Me.WebBrowser = New System.Windows.Forms.WebBrowser
        Me.PriSign = New uiCtrls.priSign
        Me.SubForms = New uiCtrls.SlideMenu
        Me.TabForm = New uiCtrls.priTabs
        Me.DataGrid = New System.Windows.Forms.DataGrid
        CType(Me.BindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BindingSource
        '
        '
        'UserText
        '
        Me.UserText.Location = New System.Drawing.Point(182, 152)
        Me.UserText.Multiline = True
        Me.UserText.Name = "UserText"
        Me.UserText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.UserText.Size = New System.Drawing.Size(150, 96)
        Me.UserText.TabIndex = 1
        '
        'WebBrowser
        '
        Me.WebBrowser.Location = New System.Drawing.Point(0, 152)
        Me.WebBrowser.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser.Name = "WebBrowser"
        Me.WebBrowser.Size = New System.Drawing.Size(150, 86)
        Me.WebBrowser.TabIndex = 4
        '
        'PriSign
        '
        Me.PriSign.Location = New System.Drawing.Point(353, 24)
        Me.PriSign.Name = "PriSign"
        Me.PriSign.Size = New System.Drawing.Size(150, 110)
        Me.PriSign.TabIndex = 5
        '
        'SubForms
        '
        Me.SubForms.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.SubForms.Location = New System.Drawing.Point(0, 343)
        Me.SubForms.MenuFont = New System.Drawing.Font("Tahoma", 10.0!)
        Me.SubForms.Name = "SubForms"
        Me.SubForms.Size = New System.Drawing.Size(560, 20)
        Me.SubForms.sMenuItems = CType(resources.GetObject("SubForms.sMenuItems"), System.Collections.Generic.Dictionary(Of Integer, System.Windows.Forms.LinkLabel))
        Me.SubForms.TabIndex = 1
        '
        'TabForm
        '
        Me.TabForm.FormFont = New System.Drawing.Font("Tahoma", 10.0!)
        Me.TabForm.Location = New System.Drawing.Point(205, 126)
        Me.TabForm.Name = "TabForm"
        Me.TabForm.Size = New System.Drawing.Size(150, 110)
        Me.TabForm.TabIndex = 9
        Me.TabForm.TabPanels = CType(resources.GetObject("TabForm.TabPanels"), System.Collections.Generic.Dictionary(Of String, System.Windows.Forms.Panel))
        '
        'DataGrid
        '
        Me.DataGrid.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.DataGrid.CaptionVisible = False
        Me.DataGrid.DataMember = ""
        Me.DataGrid.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGrid.GridLineColor = System.Drawing.SystemColors.ActiveBorder
        Me.DataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DataGrid.Location = New System.Drawing.Point(211, 133)
        Me.DataGrid.Name = "DataGrid"
        Me.DataGrid.ReadOnly = True
        Me.DataGrid.Size = New System.Drawing.Size(138, 96)
        Me.DataGrid.TabIndex = 10
        '
        'ctform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.DataGrid)
        Me.Controls.Add(Me.TabForm)
        Me.Controls.Add(Me.PriSign)
        Me.Controls.Add(Me.WebBrowser)
        Me.Controls.Add(Me.UserText)
        Me.Controls.Add(Me.SubForms)
        Me.Name = "ctform"
        Me.Size = New System.Drawing.Size(560, 363)
        CType(Me.BindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SubForms As uiCtrls.SlideMenu
    Public WithEvents BindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents UserText As System.Windows.Forms.TextBox
    Friend WithEvents WebBrowser As System.Windows.Forms.WebBrowser
    Friend WithEvents PriSign As uiCtrls.priSign
    Friend WithEvents TabForm As uiCtrls.priTabs
    Public WithEvents DataGrid As System.Windows.Forms.DataGrid


End Class
