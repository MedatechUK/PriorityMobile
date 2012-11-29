<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class AppForm
    Inherits cfOnBoardData.BaseForm 'System.Windows.Forms.Form ' cfOnBoardData.BaseForm '

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
        Me.Status = New System.Windows.Forms.StatusBar
        Me.MainStrip1 = New System.Windows.Forms.MainMenu
        Me.SyncToolStripMenuItem = New System.Windows.Forms.MenuItem
        Me.CallsToolStripMenuItem = New System.Windows.Forms.MenuItem
        Me.MoreToolStripMenuItem = New System.Windows.Forms.MenuItem
        Me.AllToolStripMenuItem = New System.Windows.Forms.MenuItem
        Me.MenuItem18 = New System.Windows.Forms.MenuItem
        Me.MalfunctionCodesToolStripMenuItem = New System.Windows.Forms.MenuItem
        Me.ResolutionCodesToolStripMenuItem = New System.Windows.Forms.MenuItem
        Me.StatusesToolStripMenuItem = New System.Windows.Forms.MenuItem
        Me.WarehouseToolStripMenuItem = New System.Windows.Forms.MenuItem
        Me.SurveysToolStripMenuItem = New System.Windows.Forms.MenuItem
        Me.MenuItem6 = New System.Windows.Forms.MenuItem
        Me.SettingsToolStripMenuItem = New System.Windows.Forms.MenuItem
        Me.MenuItem8 = New System.Windows.Forms.MenuItem
        Me.DayEndToolStripMenuItem = New System.Windows.Forms.MenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.MenuItem
        Me.Call_menu = New System.Windows.Forms.MenuItem
        Me.QuitToolStripMenuItem = New System.Windows.Forms.MenuItem
        Me.MenuItem12 = New System.Windows.Forms.MenuItem
        Me.SurveysToolStripMenuItem1 = New System.Windows.Forms.MenuItem
        Me.SettingsMenu = New System.Windows.Forms.MenuItem
        Me.NewActionToolStripMenuItem = New System.Windows.Forms.MenuItem
        Me.SetTimeEnRouteToolStripMenuItem = New System.Windows.Forms.MenuItem
        Me.SetTimeOnSiteToolStripMenuItem = New System.Windows.Forms.MenuItem
        Me.SetTimeFinishedToolStripMenuItem = New System.Windows.Forms.MenuItem
        Me.PostDataToolStripMenuItem = New System.Windows.Forms.MenuItem
        Me.MenuItem15 = New System.Windows.Forms.MenuItem
        Me.SaveSettingToolStripMenuItem = New System.Windows.Forms.MenuItem
        Me.QuitNoSaveToolStripMenuItem = New System.Windows.Forms.MenuItem
        Me.InnerArea = New System.Windows.Forms.Panel
        Me.pnl_Drawing = New System.Windows.Forms.Panel
        Me.pnl_Settings = New System.Windows.Forms.Panel
        Me.pnl_Survey = New System.Windows.Forms.Panel
        Me.pnl_Signature = New System.Windows.Forms.Panel
        Me.pnl_CallTabs = New System.Windows.Forms.Panel
        Me.CallTab = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.pnl_Address = New System.Windows.Forms.Panel
        Me.TabPage3 = New System.Windows.Forms.TabPage
        Me.pnl_Details = New System.Windows.Forms.Panel
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.pnl_Parts = New System.Windows.Forms.Panel
        Me.TabPage4 = New System.Windows.Forms.TabPage
        Me.pnl_Repair = New System.Windows.Forms.Panel
        Me.pnl_DateSelect = New System.Windows.Forms.Panel
        Me.splashlogo = New System.Windows.Forms.PictureBox
        Me.InnerArea.SuspendLayout()
        Me.pnl_CallTabs.SuspendLayout()
        Me.CallTab.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.SuspendLayout()
        '
        'Status
        '
        Me.Status.Location = New System.Drawing.Point(0, 272)
        Me.Status.Name = "Status"
        Me.Status.Size = New System.Drawing.Size(240, 22)
        '
        'MainStrip1
        '
        Me.MainStrip1.MenuItems.Add(Me.SyncToolStripMenuItem)
        Me.MainStrip1.MenuItems.Add(Me.Call_menu)
        Me.MainStrip1.MenuItems.Add(Me.SettingsMenu)
        '
        'SyncToolStripMenuItem
        '
        Me.SyncToolStripMenuItem.MenuItems.Add(Me.CallsToolStripMenuItem)
        Me.SyncToolStripMenuItem.MenuItems.Add(Me.MoreToolStripMenuItem)
        Me.SyncToolStripMenuItem.MenuItems.Add(Me.MenuItem6)
        Me.SyncToolStripMenuItem.MenuItems.Add(Me.SettingsToolStripMenuItem)
        Me.SyncToolStripMenuItem.MenuItems.Add(Me.MenuItem8)
        Me.SyncToolStripMenuItem.MenuItems.Add(Me.DayEndToolStripMenuItem)
        Me.SyncToolStripMenuItem.MenuItems.Add(Me.ExitToolStripMenuItem)
        Me.SyncToolStripMenuItem.Text = "Sync"
        '
        'CallsToolStripMenuItem
        '
        Me.CallsToolStripMenuItem.Text = "Call Data"
        '
        'MoreToolStripMenuItem
        '
        Me.MoreToolStripMenuItem.MenuItems.Add(Me.AllToolStripMenuItem)
        Me.MoreToolStripMenuItem.MenuItems.Add(Me.MenuItem18)
        Me.MoreToolStripMenuItem.MenuItems.Add(Me.MalfunctionCodesToolStripMenuItem)
        Me.MoreToolStripMenuItem.MenuItems.Add(Me.ResolutionCodesToolStripMenuItem)
        Me.MoreToolStripMenuItem.MenuItems.Add(Me.StatusesToolStripMenuItem)
        Me.MoreToolStripMenuItem.MenuItems.Add(Me.WarehouseToolStripMenuItem)
        Me.MoreToolStripMenuItem.MenuItems.Add(Me.SurveysToolStripMenuItem)
        Me.MoreToolStripMenuItem.Text = "More"
        '
        'AllToolStripMenuItem
        '
        Me.AllToolStripMenuItem.Text = "Sync All"
        '
        'MenuItem18
        '
        Me.MenuItem18.Text = "-"
        '
        'MalfunctionCodesToolStripMenuItem
        '
        Me.MalfunctionCodesToolStripMenuItem.Text = "Malfunction Codes"
        '
        'ResolutionCodesToolStripMenuItem
        '
        Me.ResolutionCodesToolStripMenuItem.Text = "Resolution Codes"
        '
        'StatusesToolStripMenuItem
        '
        Me.StatusesToolStripMenuItem.Text = "Statuses"
        '
        'WarehouseToolStripMenuItem
        '
        Me.WarehouseToolStripMenuItem.Text = "Warehouse"
        '
        'SurveysToolStripMenuItem
        '
        Me.SurveysToolStripMenuItem.Text = "Surveys"
        '
        'MenuItem6
        '
        Me.MenuItem6.Text = "-"
        '
        'SettingsToolStripMenuItem
        '
        Me.SettingsToolStripMenuItem.Text = "Settings"
        '
        'MenuItem8
        '
        Me.MenuItem8.Text = "-"
        '
        'DayEndToolStripMenuItem
        '
        Me.DayEndToolStripMenuItem.Text = "Day End"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'Call_menu
        '
        Me.Call_menu.Enabled = False
        Me.Call_menu.MenuItems.Add(Me.QuitToolStripMenuItem)
        Me.Call_menu.MenuItems.Add(Me.MenuItem12)
        Me.Call_menu.MenuItems.Add(Me.SurveysToolStripMenuItem1)
        Me.Call_menu.Text = "Call"
        '
        'QuitToolStripMenuItem
        '
        Me.QuitToolStripMenuItem.Text = "Close"
        '
        'MenuItem12
        '
        Me.MenuItem12.Text = "-"
        '
        'SurveysToolStripMenuItem1
        '
        Me.SurveysToolStripMenuItem1.Text = "Site Survey"
        '
        'SettingsMenu
        '
        Me.SettingsMenu.Enabled = False
        Me.SettingsMenu.MenuItems.Add(Me.NewActionToolStripMenuItem)
        Me.SettingsMenu.MenuItems.Add(Me.MenuItem15)
        Me.SettingsMenu.MenuItems.Add(Me.SaveSettingToolStripMenuItem)
        Me.SettingsMenu.MenuItems.Add(Me.QuitNoSaveToolStripMenuItem)
        Me.SettingsMenu.Text = "Settings"
        '
        'NewActionToolStripMenuItem
        '
        Me.NewActionToolStripMenuItem.MenuItems.Add(Me.SetTimeEnRouteToolStripMenuItem)
        Me.NewActionToolStripMenuItem.MenuItems.Add(Me.SetTimeOnSiteToolStripMenuItem)
        Me.NewActionToolStripMenuItem.MenuItems.Add(Me.SetTimeFinishedToolStripMenuItem)
        Me.NewActionToolStripMenuItem.MenuItems.Add(Me.PostDataToolStripMenuItem)
        Me.NewActionToolStripMenuItem.Text = "New Action"
        '
        'SetTimeEnRouteToolStripMenuItem
        '
        Me.SetTimeEnRouteToolStripMenuItem.Text = "Set Time En-Route"
        '
        'SetTimeOnSiteToolStripMenuItem
        '
        Me.SetTimeOnSiteToolStripMenuItem.Text = "Set Time On-Site"
        '
        'SetTimeFinishedToolStripMenuItem
        '
        Me.SetTimeFinishedToolStripMenuItem.Text = "Set Time Finished"
        '
        'PostDataToolStripMenuItem
        '
        Me.PostDataToolStripMenuItem.Text = "Post Data"
        '
        'MenuItem15
        '
        Me.MenuItem15.Text = "-"
        '
        'SaveSettingToolStripMenuItem
        '
        Me.SaveSettingToolStripMenuItem.Text = "Save Settings"
        '
        'QuitNoSaveToolStripMenuItem
        '
        Me.QuitNoSaveToolStripMenuItem.Text = "Abandon Changes"
        '
        'InnerArea
        '
        Me.InnerArea.Controls.Add(Me.pnl_Drawing)
        Me.InnerArea.Controls.Add(Me.pnl_Settings)
        Me.InnerArea.Controls.Add(Me.pnl_Survey)
        Me.InnerArea.Controls.Add(Me.pnl_Signature)
        Me.InnerArea.Controls.Add(Me.pnl_CallTabs)
        Me.InnerArea.Controls.Add(Me.pnl_DateSelect)
        Me.InnerArea.Controls.Add(Me.splashlogo)
        Me.InnerArea.Dock = System.Windows.Forms.DockStyle.Fill
        Me.InnerArea.Location = New System.Drawing.Point(0, 0)
        Me.InnerArea.Name = "InnerArea"
        Me.InnerArea.Size = New System.Drawing.Size(240, 272)
        '
        'pnl_Drawing
        '
        Me.pnl_Drawing.BackColor = System.Drawing.SystemColors.ActiveBorder
        Me.pnl_Drawing.Location = New System.Drawing.Point(70, 181)
        Me.pnl_Drawing.Name = "pnl_Drawing"
        Me.pnl_Drawing.Size = New System.Drawing.Size(38, 44)
        '
        'pnl_Settings
        '
        Me.pnl_Settings.BackColor = System.Drawing.SystemColors.ActiveBorder
        Me.pnl_Settings.Location = New System.Drawing.Point(70, 136)
        Me.pnl_Settings.Name = "pnl_Settings"
        Me.pnl_Settings.Size = New System.Drawing.Size(38, 38)
        '
        'pnl_Survey
        '
        Me.pnl_Survey.BackColor = System.Drawing.SystemColors.ActiveBorder
        Me.pnl_Survey.Location = New System.Drawing.Point(12, 180)
        Me.pnl_Survey.Name = "pnl_Survey"
        Me.pnl_Survey.Size = New System.Drawing.Size(33, 45)
        '
        'pnl_Signature
        '
        Me.pnl_Signature.BackColor = System.Drawing.SystemColors.ActiveBorder
        Me.pnl_Signature.Location = New System.Drawing.Point(12, 136)
        Me.pnl_Signature.Name = "pnl_Signature"
        Me.pnl_Signature.Size = New System.Drawing.Size(33, 38)
        '
        'pnl_CallTabs
        '
        Me.pnl_CallTabs.Controls.Add(Me.CallTab)
        Me.pnl_CallTabs.Location = New System.Drawing.Point(90, 3)
        Me.pnl_CallTabs.Name = "pnl_CallTabs"
        Me.pnl_CallTabs.Size = New System.Drawing.Size(56, 127)
        '
        'CallTab
        '
        Me.CallTab.Controls.Add(Me.TabPage1)
        Me.CallTab.Controls.Add(Me.TabPage3)
        Me.CallTab.Controls.Add(Me.TabPage2)
        Me.CallTab.Controls.Add(Me.TabPage4)
        Me.CallTab.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CallTab.Location = New System.Drawing.Point(0, 0)
        Me.CallTab.Name = "CallTab"
        Me.CallTab.SelectedIndex = 0
        Me.CallTab.Size = New System.Drawing.Size(56, 127)
        Me.CallTab.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.pnl_Address)
        Me.TabPage1.Location = New System.Drawing.Point(0, 0)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(56, 104)
        Me.TabPage1.Text = "Address"
        '
        'pnl_Address
        '
        Me.pnl_Address.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnl_Address.Location = New System.Drawing.Point(0, 0)
        Me.pnl_Address.Name = "pnl_Address"
        Me.pnl_Address.Size = New System.Drawing.Size(56, 104)
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.pnl_Details)
        Me.TabPage3.Location = New System.Drawing.Point(0, 0)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(48, 101)
        Me.TabPage3.Text = "Details"
        '
        'pnl_Details
        '
        Me.pnl_Details.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnl_Details.Location = New System.Drawing.Point(0, 0)
        Me.pnl_Details.Name = "pnl_Details"
        Me.pnl_Details.Size = New System.Drawing.Size(48, 101)
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.pnl_Parts)
        Me.TabPage2.Location = New System.Drawing.Point(0, 0)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Size = New System.Drawing.Size(48, 101)
        Me.TabPage2.Text = "Parts"
        '
        'pnl_Parts
        '
        Me.pnl_Parts.BackColor = System.Drawing.SystemColors.ActiveBorder
        Me.pnl_Parts.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnl_Parts.Location = New System.Drawing.Point(0, 0)
        Me.pnl_Parts.Name = "pnl_Parts"
        Me.pnl_Parts.Size = New System.Drawing.Size(48, 101)
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.pnl_Repair)
        Me.TabPage4.Location = New System.Drawing.Point(0, 0)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Size = New System.Drawing.Size(48, 101)
        Me.TabPage4.Text = "Repair"
        '
        'pnl_Repair
        '
        Me.pnl_Repair.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnl_Repair.Location = New System.Drawing.Point(0, 0)
        Me.pnl_Repair.Name = "pnl_Repair"
        Me.pnl_Repair.Size = New System.Drawing.Size(48, 101)
        '
        'pnl_DateSelect
        '
        Me.pnl_DateSelect.BackColor = System.Drawing.Color.White
        Me.pnl_DateSelect.Location = New System.Drawing.Point(3, 3)
        Me.pnl_DateSelect.Name = "pnl_DateSelect"
        Me.pnl_DateSelect.Size = New System.Drawing.Size(81, 78)
        '
        'splashlogo
        '
        Me.splashlogo.Location = New System.Drawing.Point(3, 80)
        Me.splashlogo.Name = "splashlogo"
        Me.splashlogo.Size = New System.Drawing.Size(81, 50)
        '
        'AppForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 294)
        Me.Controls.Add(Me.InnerArea)
        Me.Controls.Add(Me.Status)
        Me.Location = New System.Drawing.Point(0, 0)
        Me.Menu = Me.MainStrip1
        Me.Name = "AppForm"
        Me.Text = "Priority PDA"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.InnerArea.ResumeLayout(False)
        Me.pnl_CallTabs.ResumeLayout(False)
        Me.CallTab.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage4.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Status As System.Windows.Forms.StatusBar
    Friend WithEvents MainStrip1 As System.Windows.Forms.MainMenu
    Friend WithEvents SyncToolStripMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents Call_menu As System.Windows.Forms.MenuItem
    Friend WithEvents SettingsMenu As System.Windows.Forms.MenuItem
    Friend WithEvents CallsToolStripMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents MoreToolStripMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem6 As System.Windows.Forms.MenuItem
    Friend WithEvents SettingsToolStripMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem8 As System.Windows.Forms.MenuItem
    Friend WithEvents DayEndToolStripMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents QuitToolStripMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem12 As System.Windows.Forms.MenuItem
    Friend WithEvents SurveysToolStripMenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents NewActionToolStripMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem15 As System.Windows.Forms.MenuItem
    Friend WithEvents SaveSettingToolStripMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents QuitNoSaveToolStripMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents InnerArea As System.Windows.Forms.Panel
    Friend WithEvents pnl_Drawing As System.Windows.Forms.Panel
    Friend WithEvents pnl_Settings As System.Windows.Forms.Panel
    Friend WithEvents pnl_Survey As System.Windows.Forms.Panel
    Friend WithEvents pnl_Signature As System.Windows.Forms.Panel
    Friend WithEvents pnl_CallTabs As System.Windows.Forms.Panel
    Friend WithEvents CallTab As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents pnl_Address As System.Windows.Forms.Panel
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents pnl_Details As System.Windows.Forms.Panel
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents pnl_Parts As System.Windows.Forms.Panel
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents pnl_Repair As System.Windows.Forms.Panel
    Friend WithEvents pnl_DateSelect As System.Windows.Forms.Panel
    Friend WithEvents splashlogo As System.Windows.Forms.PictureBox
    Friend WithEvents AllToolStripMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem18 As System.Windows.Forms.MenuItem
    Friend WithEvents MalfunctionCodesToolStripMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ResolutionCodesToolStripMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents StatusesToolStripMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents WarehouseToolStripMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents SurveysToolStripMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents SetTimeEnRouteToolStripMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents SetTimeOnSiteToolStripMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents SetTimeFinishedToolStripMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents PostDataToolStripMenuItem As System.Windows.Forms.MenuItem
End Class
