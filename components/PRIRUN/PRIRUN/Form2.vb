Imports System.Diagnostics
Imports System.Management
Imports System.Threading

Public Class Form2

    Private blnIsTransparent As Boolean = False
 

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        openWorker.RunWorkerAsync()
        closeWorker.RunWorkerAsync()
        'Console.Read()
    End Sub

    Public Sub ProcessOpens(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles openWorker.DoWork
        Dim DemoQuery As New WqlEventQuery("__InstanceCreationEvent", New TimeSpan(0, 0, 5), "TargetInstance isa ""Win32_Process""")
        Dim DemoWatcher As New ManagementEventWatcher()

        DemoWatcher.Query = DemoQuery
        DemoWatcher.Options.Timeout = New TimeSpan(0, 0, 10)

        While openWorker.CancellationPending = False
            Try

                Dim bo As ManagementBaseObject = DemoWatcher.WaitForNextEvent()
                'Dim DemoLog As New EventLog("ProcessWatcher")
                'DemoLog.Source = "ApplicationOpen"
                Dim EventName As String = CType(bo("TargetInstance"), ManagementBaseObject)("Name").ToString
                Dim ProcessID As String = CType(bo("TargetInstance"), ManagementBaseObject)("ProcessID").ToString
                openWorker.ReportProgress(0, String.Format("Open: {0}:{1}", EventName, ProcessID))
                'DemoLog.WriteEntry(EventName, EventLogEntryType.Information)

            Catch 'ex As Exception
                'If Not ex.Message = "Timed out " Then
                '    Debug.WriteLine(ex.Message)
                'End If
            End Try
        End While
        Debug.WriteLine("Open Worker Stopped")
        DemoWatcher.Stop()
    End Sub

    Public Sub ProcessCloses(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles closeWorker.DoWork
        Dim DemoQuery As New WqlEventQuery("__InstanceDeletionEvent", New TimeSpan(0, 0, 5), "TargetInstance isa ""Win32_Process""")
        Dim DemoWatcher As New ManagementEventWatcher()

        DemoWatcher.Query = DemoQuery
        DemoWatcher.Options.Timeout = New TimeSpan(0, 0, 10)

        While closeWorker.CancellationPending = False
            Try
                Dim bo As ManagementBaseObject = DemoWatcher.WaitForNextEvent()
                ' Dim DemoLog As New EventLog("ProcessWatcher")
                'DemoLog.Source = "ApplicationClose"
                Dim EventName As String = CType(bo("TargetInstance"), ManagementBaseObject)("Name").ToString
                Dim ProcessID As String = CType(bo("TargetInstance"), ManagementBaseObject)("ProcessID").ToString
                openWorker.ReportProgress(0, String.Format("Close: {0}:{1}", EventName, ProcessID))
                'DemoLog.WriteEntry(EventName, EventLogEntryType.Information)
           
            Catch 'ex As Exception
                'If Not ex.Message = "Timed out " Then
                '    Debug.WriteLine(ex.Message)
                'End If
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

End Class