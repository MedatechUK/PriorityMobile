<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmConsole
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmConsole))
        Me.rtfConsole = New System.Windows.Forms.RichTextBox
        Me.DialogFont = New System.Windows.Forms.FontDialog
        Me.DialogForeColour = New System.Windows.Forms.ColorDialog
        Me.DialogBackColour = New System.Windows.Forms.ColorDialog
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CloseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator
        Me.QuitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CopyWebFilesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.InternetInformationServerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripSeparator
        Me.PriPROCToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.StartServiceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.StopServiceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem5 = New System.Windows.Forms.ToolStripSeparator
        Me.PauseServiceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ResumeServiceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ServiceManagerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem6 = New System.Windows.Forms.ToolStripSeparator
        Me.WindowsEventViewerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.WMITesterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TestEventHandlerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TestToolToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem7 = New System.Windows.Forms.ToolStripSeparator
        Me.Install2k8r2Win7WMIHotfixToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem8 = New System.Windows.Forms.ToolStripSeparator
        Me.PreferencesToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.ForegroundColourToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BackgroundColourToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.ConsoleFontToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DevWikiToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripSeparator
        Me.SourceCodeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PriProcServiceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AdminTerminalToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.VBNETToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CNETToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.WindowsManagementInstrumentationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.WMIreadme1stToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.WMITestToolToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.HotFixKBToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.StatusStrip = New System.Windows.Forms.StatusStrip
        Me.StatusLabel = New System.Windows.Forms.ToolStripStatusLabel
        Me.CommandLabel = New System.Windows.Forms.ToolStripStatusLabel
        Me.CommandHistoryLabel = New System.Windows.Forms.ToolStripStatusLabel
        Me.FolderBrowser = New System.Windows.Forms.FolderBrowserDialog
        Me.OpenWebConfigToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuStrip1.SuspendLayout()
        Me.StatusStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'rtfConsole
        '
        Me.rtfConsole.BackColor = System.Drawing.Color.Black
        Me.rtfConsole.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtfConsole.ForeColor = System.Drawing.Color.Green
        Me.rtfConsole.Location = New System.Drawing.Point(0, 24)
        Me.rtfConsole.Name = "rtfConsole"
        Me.rtfConsole.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical
        Me.rtfConsole.Size = New System.Drawing.Size(721, 382)
        Me.rtfConsole.TabIndex = 0
        Me.rtfConsole.Text = ""
        '
        'DialogFont
        '
        Me.DialogFont.Font = New System.Drawing.Font("Lucida Console", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        '
        'DialogForeColour
        '
        Me.DialogForeColour.Color = System.Drawing.Color.Chartreuse
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.ToolsToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(721, 24)
        Me.MenuStrip1.TabIndex = 2
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenToolStripMenuItem, Me.CloseToolStripMenuItem, Me.ToolStripMenuItem1, Me.QuitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(66, 20)
        Me.FileToolStripMenuItem.Text = "&Terminal"
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.OpenToolStripMenuItem.Text = "Connect"
        '
        'CloseToolStripMenuItem
        '
        Me.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem"
        Me.CloseToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.CloseToolStripMenuItem.Text = "Disconnect"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(149, 6)
        '
        'QuitToolStripMenuItem
        '
        Me.QuitToolStripMenuItem.Name = "QuitToolStripMenuItem"
        Me.QuitToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.QuitToolStripMenuItem.Text = "Exit"
        '
        'ToolsToolStripMenuItem
        '
        Me.ToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CopyWebFilesToolStripMenuItem, Me.InternetInformationServerToolStripMenuItem, Me.OpenWebConfigToolStripMenuItem, Me.ToolStripMenuItem4, Me.PriPROCToolStripMenuItem, Me.ServiceManagerToolStripMenuItem, Me.ToolStripMenuItem6, Me.WindowsEventViewerToolStripMenuItem, Me.WMITesterToolStripMenuItem, Me.ToolStripMenuItem8, Me.PreferencesToolStripMenuItem1})
        Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(48, 20)
        Me.ToolsToolStripMenuItem.Text = "Tools"
        '
        'CopyWebFilesToolStripMenuItem
        '
        Me.CopyWebFilesToolStripMenuItem.Name = "CopyWebFilesToolStripMenuItem"
        Me.CopyWebFilesToolStripMenuItem.Size = New System.Drawing.Size(216, 22)
        Me.CopyWebFilesToolStripMenuItem.Text = "Copy Web Files"
        '
        'InternetInformationServerToolStripMenuItem
        '
        Me.InternetInformationServerToolStripMenuItem.Name = "InternetInformationServerToolStripMenuItem"
        Me.InternetInformationServerToolStripMenuItem.Size = New System.Drawing.Size(216, 22)
        Me.InternetInformationServerToolStripMenuItem.Text = "Internet Information Server"
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(213, 6)
        '
        'PriPROCToolStripMenuItem
        '
        Me.PriPROCToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StartServiceToolStripMenuItem, Me.StopServiceToolStripMenuItem, Me.ToolStripMenuItem5, Me.PauseServiceToolStripMenuItem, Me.ResumeServiceToolStripMenuItem})
        Me.PriPROCToolStripMenuItem.Name = "PriPROCToolStripMenuItem"
        Me.PriPROCToolStripMenuItem.Size = New System.Drawing.Size(216, 22)
        Me.PriPROCToolStripMenuItem.Text = "PriPROC Service"
        '
        'StartServiceToolStripMenuItem
        '
        Me.StartServiceToolStripMenuItem.Name = "StartServiceToolStripMenuItem"
        Me.StartServiceToolStripMenuItem.Size = New System.Drawing.Size(156, 22)
        Me.StartServiceToolStripMenuItem.Text = "Start Service"
        '
        'StopServiceToolStripMenuItem
        '
        Me.StopServiceToolStripMenuItem.Name = "StopServiceToolStripMenuItem"
        Me.StopServiceToolStripMenuItem.Size = New System.Drawing.Size(156, 22)
        Me.StopServiceToolStripMenuItem.Text = "Stop Service"
        '
        'ToolStripMenuItem5
        '
        Me.ToolStripMenuItem5.Name = "ToolStripMenuItem5"
        Me.ToolStripMenuItem5.Size = New System.Drawing.Size(153, 6)
        '
        'PauseServiceToolStripMenuItem
        '
        Me.PauseServiceToolStripMenuItem.Name = "PauseServiceToolStripMenuItem"
        Me.PauseServiceToolStripMenuItem.Size = New System.Drawing.Size(156, 22)
        Me.PauseServiceToolStripMenuItem.Text = "Pause Service"
        '
        'ResumeServiceToolStripMenuItem
        '
        Me.ResumeServiceToolStripMenuItem.Name = "ResumeServiceToolStripMenuItem"
        Me.ResumeServiceToolStripMenuItem.Size = New System.Drawing.Size(156, 22)
        Me.ResumeServiceToolStripMenuItem.Text = "Resume Service"
        '
        'ServiceManagerToolStripMenuItem
        '
        Me.ServiceManagerToolStripMenuItem.Name = "ServiceManagerToolStripMenuItem"
        Me.ServiceManagerToolStripMenuItem.Size = New System.Drawing.Size(216, 22)
        Me.ServiceManagerToolStripMenuItem.Text = "Service Manager"
        '
        'ToolStripMenuItem6
        '
        Me.ToolStripMenuItem6.Name = "ToolStripMenuItem6"
        Me.ToolStripMenuItem6.Size = New System.Drawing.Size(213, 6)
        '
        'WindowsEventViewerToolStripMenuItem
        '
        Me.WindowsEventViewerToolStripMenuItem.Name = "WindowsEventViewerToolStripMenuItem"
        Me.WindowsEventViewerToolStripMenuItem.Size = New System.Drawing.Size(216, 22)
        Me.WindowsEventViewerToolStripMenuItem.Text = "Windows Event Viewer"
        '
        'WMITesterToolStripMenuItem
        '
        Me.WMITesterToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TestEventHandlerToolStripMenuItem, Me.TestToolToolStripMenuItem, Me.ToolStripMenuItem7, Me.Install2k8r2Win7WMIHotfixToolStripMenuItem})
        Me.WMITesterToolStripMenuItem.Name = "WMITesterToolStripMenuItem"
        Me.WMITesterToolStripMenuItem.Size = New System.Drawing.Size(216, 22)
        Me.WMITesterToolStripMenuItem.Text = "WMI"
        '
        'TestEventHandlerToolStripMenuItem
        '
        Me.TestEventHandlerToolStripMenuItem.Name = "TestEventHandlerToolStripMenuItem"
        Me.TestEventHandlerToolStripMenuItem.Size = New System.Drawing.Size(231, 22)
        Me.TestEventHandlerToolStripMenuItem.Text = "Test Event Handler"
        '
        'TestToolToolStripMenuItem
        '
        Me.TestToolToolStripMenuItem.Name = "TestToolToolStripMenuItem"
        Me.TestToolToolStripMenuItem.Size = New System.Drawing.Size(231, 22)
        Me.TestToolToolStripMenuItem.Text = "Test Tool"
        '
        'ToolStripMenuItem7
        '
        Me.ToolStripMenuItem7.Name = "ToolStripMenuItem7"
        Me.ToolStripMenuItem7.Size = New System.Drawing.Size(228, 6)
        '
        'Install2k8r2Win7WMIHotfixToolStripMenuItem
        '
        Me.Install2k8r2Win7WMIHotfixToolStripMenuItem.Name = "Install2k8r2Win7WMIHotfixToolStripMenuItem"
        Me.Install2k8r2Win7WMIHotfixToolStripMenuItem.Size = New System.Drawing.Size(231, 22)
        Me.Install2k8r2Win7WMIHotfixToolStripMenuItem.Text = "Install 2k8r2/Win7 WMI Hotfix"
        '
        'ToolStripMenuItem8
        '
        Me.ToolStripMenuItem8.Name = "ToolStripMenuItem8"
        Me.ToolStripMenuItem8.Size = New System.Drawing.Size(213, 6)
        '
        'PreferencesToolStripMenuItem1
        '
        Me.PreferencesToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ForegroundColourToolStripMenuItem, Me.BackgroundColourToolStripMenuItem1, Me.ConsoleFontToolStripMenuItem})
        Me.PreferencesToolStripMenuItem1.Name = "PreferencesToolStripMenuItem1"
        Me.PreferencesToolStripMenuItem1.Size = New System.Drawing.Size(216, 22)
        Me.PreferencesToolStripMenuItem1.Text = "Console Preferences"
        '
        'ForegroundColourToolStripMenuItem
        '
        Me.ForegroundColourToolStripMenuItem.Name = "ForegroundColourToolStripMenuItem"
        Me.ForegroundColourToolStripMenuItem.Size = New System.Drawing.Size(177, 22)
        Me.ForegroundColourToolStripMenuItem.Text = "Foreground Colour"
        '
        'BackgroundColourToolStripMenuItem1
        '
        Me.BackgroundColourToolStripMenuItem1.Name = "BackgroundColourToolStripMenuItem1"
        Me.BackgroundColourToolStripMenuItem1.Size = New System.Drawing.Size(177, 22)
        Me.BackgroundColourToolStripMenuItem1.Text = "Background Colour"
        '
        'ConsoleFontToolStripMenuItem
        '
        Me.ConsoleFontToolStripMenuItem.Name = "ConsoleFontToolStripMenuItem"
        Me.ConsoleFontToolStripMenuItem.Size = New System.Drawing.Size(177, 22)
        Me.ConsoleFontToolStripMenuItem.Text = "Console Font"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DevWikiToolStripMenuItem, Me.ToolStripMenuItem3, Me.SourceCodeToolStripMenuItem, Me.LToolStripMenuItem, Me.WindowsManagementInstrumentationToolStripMenuItem, Me.ToolStripMenuItem2, Me.AboutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'DevWikiToolStripMenuItem
        '
        Me.DevWikiToolStripMenuItem.Name = "DevWikiToolStripMenuItem"
        Me.DevWikiToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.DevWikiToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.DevWikiToolStripMenuItem.Text = "Dev Wiki"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(166, 6)
        '
        'SourceCodeToolStripMenuItem
        '
        Me.SourceCodeToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PriProcServiceToolStripMenuItem, Me.AdminTerminalToolStripMenuItem})
        Me.SourceCodeToolStripMenuItem.Name = "SourceCodeToolStripMenuItem"
        Me.SourceCodeToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.SourceCodeToolStripMenuItem.Text = "Source Code"
        '
        'PriProcServiceToolStripMenuItem
        '
        Me.PriProcServiceToolStripMenuItem.Name = "PriProcServiceToolStripMenuItem"
        Me.PriProcServiceToolStripMenuItem.Size = New System.Drawing.Size(165, 22)
        Me.PriProcServiceToolStripMenuItem.Text = "PriPROC4 Service"
        '
        'AdminTerminalToolStripMenuItem
        '
        Me.AdminTerminalToolStripMenuItem.Name = "AdminTerminalToolStripMenuItem"
        Me.AdminTerminalToolStripMenuItem.Size = New System.Drawing.Size(165, 22)
        Me.AdminTerminalToolStripMenuItem.Text = "Admin Terminal"
        '
        'LToolStripMenuItem
        '
        Me.LToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.VBNETToolStripMenuItem, Me.CNETToolStripMenuItem})
        Me.LToolStripMenuItem.Name = "LToolStripMenuItem"
        Me.LToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.LToolStripMenuItem.Text = "Loading Examples"
        '
        'VBNETToolStripMenuItem
        '
        Me.VBNETToolStripMenuItem.Name = "VBNETToolStripMenuItem"
        Me.VBNETToolStripMenuItem.Size = New System.Drawing.Size(114, 22)
        Me.VBNETToolStripMenuItem.Text = "VB.NET"
        '
        'CNETToolStripMenuItem
        '
        Me.CNETToolStripMenuItem.Name = "CNETToolStripMenuItem"
        Me.CNETToolStripMenuItem.Size = New System.Drawing.Size(114, 22)
        Me.CNETToolStripMenuItem.Text = "C#.NET"
        '
        'WindowsManagementInstrumentationToolStripMenuItem
        '
        Me.WindowsManagementInstrumentationToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.WMIreadme1stToolStripMenuItem, Me.WMITestToolToolStripMenuItem, Me.HotFixKBToolStripMenuItem})
        Me.WindowsManagementInstrumentationToolStripMenuItem.Name = "WindowsManagementInstrumentationToolStripMenuItem"
        Me.WindowsManagementInstrumentationToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.WindowsManagementInstrumentationToolStripMenuItem.Text = "WMI Help"
        '
        'WMIreadme1stToolStripMenuItem
        '
        Me.WMIreadme1stToolStripMenuItem.Name = "WMIreadme1stToolStripMenuItem"
        Me.WMIreadme1stToolStripMenuItem.Size = New System.Drawing.Size(165, 22)
        Me.WMIreadme1stToolStripMenuItem.Text = "WMI (readme1st)"
        '
        'WMITestToolToolStripMenuItem
        '
        Me.WMITestToolToolStripMenuItem.Name = "WMITestToolToolStripMenuItem"
        Me.WMITestToolToolStripMenuItem.Size = New System.Drawing.Size(165, 22)
        Me.WMITestToolToolStripMenuItem.Text = "WMI Test Tool"
        '
        'HotFixKBToolStripMenuItem
        '
        Me.HotFixKBToolStripMenuItem.Name = "HotFixKBToolStripMenuItem"
        Me.HotFixKBToolStripMenuItem.Size = New System.Drawing.Size(165, 22)
        Me.HotFixKBToolStripMenuItem.Text = "HotFix KB981314"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(166, 6)
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.AboutToolStripMenuItem.Text = "About"
        '
        'StatusStrip
        '
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLabel, Me.CommandLabel, Me.CommandHistoryLabel})
        Me.StatusStrip.Location = New System.Drawing.Point(0, 384)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Size = New System.Drawing.Size(721, 22)
        Me.StatusStrip.TabIndex = 3
        Me.StatusStrip.Text = "StatusStrip"
        Me.StatusStrip.Visible = False
        '
        'StatusLabel
        '
        Me.StatusLabel.Name = "StatusLabel"
        Me.StatusLabel.Size = New System.Drawing.Size(121, 17)
        Me.StatusLabel.Text = "ToolStripStatusLabel1"
        '
        'CommandLabel
        '
        Me.CommandLabel.Name = "CommandLabel"
        Me.CommandLabel.Size = New System.Drawing.Size(121, 17)
        Me.CommandLabel.Text = "ToolStripStatusLabel1"
        '
        'CommandHistoryLabel
        '
        Me.CommandHistoryLabel.Name = "CommandHistoryLabel"
        Me.CommandHistoryLabel.Size = New System.Drawing.Size(121, 17)
        Me.CommandHistoryLabel.Text = "ToolStripStatusLabel1"
        '
        'OpenWebConfigToolStripMenuItem
        '
        Me.OpenWebConfigToolStripMenuItem.Name = "OpenWebConfigToolStripMenuItem"
        Me.OpenWebConfigToolStripMenuItem.Size = New System.Drawing.Size(216, 22)
        Me.OpenWebConfigToolStripMenuItem.Text = "Open Web Config"
        '
        'frmConsole
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(721, 406)
        Me.Controls.Add(Me.StatusStrip)
        Me.Controls.Add(Me.rtfConsole)
        Me.Controls.Add(Me.MenuStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmConsole"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "PriProc Managment Terminal"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents rtfConsole As System.Windows.Forms.RichTextBox
    Friend WithEvents DialogFont As System.Windows.Forms.FontDialog
    Friend WithEvents DialogForeColour As System.Windows.Forms.ColorDialog
    Friend WithEvents DialogBackColour As System.Windows.Forms.ColorDialog
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CloseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents QuitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents StatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents CommandLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents CommandHistoryLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WindowsEventViewerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents InternetInformationServerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DevWikiToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SourceCodeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PriProcServiceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AdminTerminalToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents VBNETToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CNETToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItem4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents PreferencesToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ForegroundColourToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BackgroundColourToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ConsoleFontToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PriPROCToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StartServiceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StopServiceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents PauseServiceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ResumeServiceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents WMITesterToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TestToolToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents Install2k8r2Win7WMIHotfixToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ServiceManagerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WindowsManagementInstrumentationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WMITestToolToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HotFixKBToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TestEventHandlerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WMIreadme1stToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CopyWebFilesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FolderBrowser As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents ToolStripMenuItem8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents OpenWebConfigToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
