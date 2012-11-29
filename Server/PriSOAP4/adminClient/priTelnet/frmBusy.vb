Imports System.Windows.Forms

Public Class frmBusy

    Private _waitfor As ServiceProcess.ServiceControllerStatus
    Private startTimer As System.Timers.Timer

    Public Sub New(ByVal LabelText As String, ByVal waitfor As ServiceProcess.ServiceControllerStatus)
        InitializeComponent()
        Me.LblWaitfor.Text = LabelText
        _waitfor = waitfor
    End Sub

    Private Sub frmBusy_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        startTimer = New System.Timers.Timer
        With startTimer
            .Interval = 1000
            AddHandler .Elapsed, AddressOf hStartTimer
            .Enabled = True
        End With

    End Sub

    Private Sub hStartTimer(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)

        With startTimer
            .Enabled = False
            .Dispose()
        End With

        Dim i As Integer = 0
        While i < 50 And Not (PriPROCSVC.Status = _waitfor)
            Threading.Thread.Sleep(1000)
            PriPROCSVC = New System.ServiceProcess.ServiceController("PRIPROC4")
            i += 1
        End While
        If Not PriPROCSVC.Status = _waitfor Then
            MsgBox("Error controlling the service.")
        End If
        Invoke(New Action(AddressOf threadSafeClose))

    End Sub

    Private Sub threadSafeClose()
        Me.Close()
    End Sub

End Class
