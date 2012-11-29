Imports System.Diagnostics
Imports System.Management
Imports System.Threading

Public Class Form2

    Private blnIsTransparent As Boolean = False
 

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.TextBox1.BackColor = My.Settings.BackgroundColour
        Me.TextBox1.ForeColor = My.Settings.ForegoundColour
        Me.TextBox1.Font = My.Settings.ForegroundText

        Me.ToolStrip1.BackColor = Me.TextBox1.BackColor
        Me.BackColor = Me.TextBox1.BackColor

        openWorker.RunWorkerAsync()
        closeWorker.RunWorkerAsync()
        Console.Read()
    End Sub

    Public Sub ProcessOpens(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles openWorker.DoWork
        Dim DemoQuery As New WqlEventQuery("__InstanceCreationEvent", New TimeSpan(0, 0, 1), "TargetInstance isa ""Win32_Process""")
        Dim DemoWatcher As New ManagementEventWatcher()

        DemoWatcher.Query = DemoQuery
        DemoWatcher.Options.Timeout = New TimeSpan(0, 0, 10)

        While openWorker.CancellationPending = False
            Try
                Dim bo As ManagementBaseObject = DemoWatcher.WaitForNextEvent()
                Dim DemoLog As New EventLog("ProcessWatcher")
                DemoLog.Source = "ApplicationOpen"
                Dim EventName As String = CType(bo("TargetInstance"), ManagementBaseObject)("Name").ToString
                openWorker.ReportProgress(0, "Open: " + EventName)
                DemoLog.WriteEntry(EventName, EventLogEntryType.Information)
           
            Catch ex As Exception
                If Not ex.Message = "Timed out " Then
                    Debug.WriteLine(ex.Message)
                End If
            End Try
        End While
        Debug.WriteLine("Open Worker Stopped")
        DemoWatcher.Stop()
    End Sub

    Public Sub ProcessCloses(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles closeWorker.DoWork
        Dim DemoQuery As New WqlEventQuery("__InstanceDeletionEvent", New TimeSpan(0, 0, 1), "TargetInstance isa ""Win32_Process""")
        Dim DemoWatcher As New ManagementEventWatcher()

        DemoWatcher.Query = DemoQuery
        DemoWatcher.Options.Timeout = New TimeSpan(0, 0, 10)

        While closeWorker.CancellationPending = False
            Try
                Dim bo As ManagementBaseObject = DemoWatcher.WaitForNextEvent()
                Dim DemoLog As New EventLog("ProcessWatcher")
                DemoLog.Source = "ApplicationClose"
                Dim EventName As String = CType(bo("TargetInstance"), ManagementBaseObject)("Name").ToString
                closeWorker.ReportProgress(0, "Close: " + EventName)
                DemoLog.WriteEntry(EventName, EventLogEntryType.Information)
           
            Catch ex As Exception
                If Not ex.Message = "Timed out " Then
                    Debug.WriteLine(ex.Message)
                End If
            End Try
        End While
        Debug.WriteLine("Close Worker Stopped")
        DemoWatcher.Stop()
    End Sub

    Public Sub WorkersReportProgres(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles openWorker.ProgressChanged, closeWorker.ProgressChanged
        Me.TextBox1.AppendText(Environment.NewLine + CType(e.UserState, String))
    End Sub

    Private Sub Form2_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        My.Settings.Save()
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        Me.TextBox1.AppendText(Environment.NewLine + Environment.NewLine + "Closing, please wait...")
        openWorker.CancelAsync()
        closeWorker.CancelAsync()

        While openWorker.IsBusy Or closeWorker.IsBusy
            System.Windows.Forms.Application.DoEvents()
        End While


        
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub

    Private Sub btnPause_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub TextBox1_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.DoubleClick
        If blnIsTransparent = False Then
            'make transparent
            Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
            Me.TransparencyKey = Me.TextBox1.BackColor
            Me.Opacity = 50
            Me.NotifyIcon1.Visible = True
            Me.ToolStrip1.Visible = False
            Me.ToolStripButtonPause.Visible = False
            Me.blnIsTransparent = True
        End If
    End Sub

    Private Sub FontToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FontToolStripMenuItem1.Click
        'Background Colour
        Me.ColorDialog1.Color = My.Settings.BackgroundColour
        If Me.ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            My.Settings.BackgroundColour = Me.ColorDialog1.Color
            Me.TextBox1.BackColor = My.Settings.BackgroundColour
            Me.ToolStrip1.BackColor = My.Settings.BackgroundColour
            Me.BackColor = My.Settings.BackgroundColour
        End If
    End Sub

    Private Sub ColourToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ColourToolStripMenuItem.Click
        'Foreground Colour
        Me.ColorDialog1.Color = My.Settings.ForegoundColour
        If Me.ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            My.Settings.ForegoundColour = Me.ColorDialog1.Color
            Me.TextBox1.ForeColor = My.Settings.ForegoundColour
        End If
    End Sub

    Private Sub FontToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FontToolStripMenuItem.Click
        'Foreground Font
        Me.FontDialog1.Font = My.Settings.ForegroundText
        If Me.FontDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            My.Settings.ForegroundText = Me.FontDialog1.Font
            Me.TextBox1.Font = My.Settings.ForegroundText
        End If
    End Sub

   
    Private Sub NotifyIcon1_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        'Make untransparent
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.TransparencyKey = Nothing
        Me.NotifyIcon1.Visible = False
        Me.Opacity = 100
        Me.ToolStrip1.Visible = True
        Me.ToolStripButtonPause.Visible = True
        Me.blnIsTransparent = False
    End Sub

    Private Sub ToolStripButtonPause_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButtonPause.Click
        If ToolStripButtonPause.Text = "Pause" Then
            openWorker.CancelAsync()
            closeWorker.CancelAsync()
            ToolStripButtonPause.Text = "Continue"
        Else
            Me.Cursor = Windows.Forms.Cursors.WaitCursor
            While openWorker.IsBusy Or closeWorker.IsBusy
                System.Windows.Forms.Application.DoEvents()
            End While
            openWorker.RunWorkerAsync()
            closeWorker.RunWorkerAsync()
            ToolStripButtonPause.Text = "Pause"
            Me.Cursor = Windows.Forms.Cursors.Default
        End If
    End Sub

    Private Sub QuickOverviewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles QuickOverviewToolStripMenuItem.Click
        MsgBox("Double click the text box area to turn form transparent. When transparent, double-click taskbar icon to restore window to normal status. " + Environment.NewLine + Environment.NewLine + "All open/close events are written to a ProcessWatcher Event log.", MsgBoxStyle.Information, "Quick Overview")
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        Dim about As New AboutBox
        about.ShowDialog()
    End Sub
End Class