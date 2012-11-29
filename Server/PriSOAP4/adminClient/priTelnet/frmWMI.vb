Imports System.Windows.Forms
Imports System
Imports System.Management

Public Class frmWMI

#Region "Private Variables"

    Private NOTEPADStartWatcher As ManagementEventWatcher
    Private NOTEPADStopWatcher As ManagementEventWatcher
    Private HldEvents As List(Of HandledWMIEvent)

    Private WithEvents tmr As Timers.Timer
    Private WithEvents CheckEventTimer As Timers.Timer
    Private WithEvents KillProcTimer As Timers.Timer

    Private KillProc As Integer

    Private started As Boolean
    Private stopped As Boolean

#End Region

#Region "Initialisation and finalisation"

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        NOTEPADStartWatcher = New ManagementEventWatcher(GenerateStartQuery("NOTEPAD.EXE"))
        AddHandler NOTEPADStartWatcher.EventArrived, AddressOf NOTEPADStarted
        NOTEPADStartWatcher.Start()

        NOTEPADStopWatcher = New ManagementEventWatcher(GenerateStopQuery("NOTEPAD.EXE"))
        AddHandler NOTEPADStartWatcher.EventArrived, AddressOf NOTEPADStopped
        NOTEPADStopWatcher.Start()

    End Sub

    Private Sub DisposeTimers()
        With tmr
            .Stop()
            .Dispose()
        End With

        With CheckEventTimer
            .Stop()
            .Dispose()
        End With

        Invoke(New Action(AddressOf threadSafeTimerDisposed))

    End Sub

#End Region

#Region "Button Handlers"

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Test_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTest.Click

        btnTest.Enabled = False

        HldEvents = New List(Of HandledWMIEvent)

        tmr = New Timers.Timer()
        With tmr
            .Interval = 15 * 1000
            AddHandler .Elapsed, AddressOf TimeOutElapsed
            .Start()
        End With

        CheckEventTimer = New Timers.Timer()
        With CheckEventTimer
            .Interval = 100
            AddHandler .Elapsed, AddressOf CheckEvents
            .Start()
        End With

        With ProgressBar
            .Value = 0
            .Style = ProgressBarStyle.Marquee
            .Enabled = False
        End With

        Invoke(New Action(Of String)(AddressOf threadSafeSetWMIresult), _
            String.Format("Listening for events....", "") _
        )

        started = False
        stopped = False

        For i As Integer = 0 To 100
            Application.DoEvents()
            Threading.Thread.Sleep(1)
        Next
        Using p As New Process
            With p
                With .StartInfo
                    .FileName = "NOTEPAD.EXE"                    
                    .WindowStyle = ProcessWindowStyle.Minimized
                End With
                .Start()
            End With
        End Using

    End Sub

#End Region

#Region "Timer Handlers"

    Private Sub CheckEvents(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)

        Invoke(New Action(AddressOf threadsafeBringToFront))

        For Each hld As HandledWMIEvent In HldEvents
            If Not started _
                And Not stopped _
                And hld.Name = tWMIProcess.NOTEPAD _
                And hld.EventState = tWMIEventState.StateStart _
                And hld.ParentProcess = System.Diagnostics.Process.GetCurrentProcess().Id Then
                started = True
                KillProc = hld.Process
                Invoke(New Action(Of String)(AddressOf threadSafeSetWMIresult), _
                    String.Format("Started [{0}] process with ID [{1}]", "NOTEPAD", hld.Process.ToString) _
                )
                CheckEventTimer.Enabled = False
                KillProcTimer = New Timers.Timer()
                With KillProcTimer
                    .Interval = 2000
                    AddHandler .Elapsed, AddressOf hKillProc
                    .Start()
                End With
                Exit For
            End If

            If started _
                And Not stopped And _
                hld.Name = tWMIProcess.NOTEPAD _
                And hld.EventState = tWMIEventState.StateStop _
                And hld.ParentProcess = System.Diagnostics.Process.GetCurrentProcess().Id Then
                stopped = True
                Invoke(New Action(Of String)(AddressOf threadSafeSetWMIresult), _
                    String.Format("Stopped [{0}] process with ID [{1}]", "NOTEPAD", hld.Process.ToString) _
                )
                DisposeTimers()
                Exit For
            End If
        Next

    End Sub

    Private Sub TimeOutElapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)

        DisposeTimers()
        Invoke(New Action(Of String)(AddressOf threadSafeSetWMIresult), _
                String.Format("Timeout Elapsed.", "") _
            )        

    End Sub

    Private Sub hKillProc(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)

        With KillProcTimer
            .Enabled = False
            .Dispose()
        End With

        Invoke(New Action(AddressOf threadSafeKillProc))
        CheckEventTimer.Enabled = True

    End Sub

#End Region

#Region "Thread Safe Calls"

    Private Sub threadSafeSetWMIresult(ByVal StrVal As String)
        Me.wmiResult.Text = StrVal
    End Sub

    Private Sub threadSafeSetProgress(ByVal IncVal As Integer)
        Me.ProgressBar.Value += IncVal
    End Sub

    Private Sub threadSafeTimerDisposed()
        btnTest.Enabled = True
        With ProgressBar
            .Value = 0
            .Style = ProgressBarStyle.Continuous
            .Enabled = False
        End With
    End Sub

    Private Sub threadsafeBringToFront()
        Me.BringToFront()
    End Sub

    Private Sub threadSafeKillProc()
        Dim P As System.Diagnostics.Process = System.Diagnostics.Process.GetProcessById(KillProc)
        P.Kill()
    End Sub

#End Region

#Region "Start / stop WMI Queries"

    Private Function GenerateStartQuery(ByVal ProcessName As String) As WqlEventQuery
        Dim ApplicationStartQuery As New WqlEventQuery
        ApplicationStartQuery.EventClassName = "Win32_ProcessStartTrace"
        ApplicationStartQuery.QueryString = String.Concat("SELECT * FROM Win32_ProcessStartTrace where ProcessName = ", """", ProcessName, """")
        Return ApplicationStartQuery
    End Function

    Private Function GenerateStopQuery(ByVal ProcessName As String) As WqlEventQuery
        Dim ApplicationStopQuery As New WqlEventQuery
        ApplicationStopQuery.EventClassName = "Win32_ProcessStopTrace"
        ApplicationStopQuery.QueryString = String.Concat("SELECT * FROM Win32_ProcessStopTrace where ProcessName = ", """", ProcessName, """")
        Return ApplicationStopQuery
    End Function

#End Region

#Region "WMI event handlers"

    Private Sub NOTEPADStarted(ByVal sender As Object, ByVal e As EventArrivedEventArgs)
        HldEvents.Add(New HandledWMIEvent(tWMIProcess.NOTEPAD, tWMIEventState.StateStart, e.NewEvent.Properties("ParentProcessID").Value, e.NewEvent.Properties("ProcessID").Value))
    End Sub

    Private Sub NOTEPADStopped(ByVal sender As Object, ByVal e As EventArrivedEventArgs)
        HldEvents.Add(New HandledWMIEvent(tWMIProcess.NOTEPAD, tWMIEventState.StateStop, e.NewEvent.Properties("ParentProcessID").Value, e.NewEvent.Properties("ProcessID").Value))
    End Sub

#End Region

End Class

#Region "Friend Classes and Enumerations"

Friend Enum tWMIProcess
    NOTEPAD = 1
End Enum

Friend Enum tWMIEventState
    StateStart = 1
    StateStop = 2
End Enum

Friend Class HandledWMIEvent

    Public Sub New(ByVal Name As tWMIProcess, ByVal EventState As tWMIEventState, ByVal ParentProcessID As Integer, ByVal ProcessID As Integer)
        _Name = Name
        _EventState = EventState
        _ParentProcessID = ParentProcessID
        _ProcessID = ProcessID
    End Sub
    Private _Name As tWMIProcess
    Public Property Name() As tWMIProcess
        Get
            Return _Name
        End Get
        Set(ByVal value As tWMIProcess)
            _Name = value
        End Set
    End Property
    Private _EventState As tWMIEventState
    Public Property EventState() As tWMIEventState
        Get
            Return _EventState
        End Get
        Set(ByVal value As tWMIEventState)
            _EventState = value
        End Set
    End Property
    Private _ParentProcessID As Integer
    Public Property ParentProcess() As Integer
        Get
            Return _ParentProcessID
        End Get
        Set(ByVal value As Integer)
            _ParentProcessID = value
        End Set
    End Property
    Private _ProcessID
    Public Property Process() As Integer
        Get
            Return _ProcessID
        End Get
        Set(ByVal value As Integer)
            _ProcessID = value
        End Set
    End Property

End Class

#End Region