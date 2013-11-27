Imports System
Imports System.Management

Public Class wmiQuery : Implements IDisposable

#Region "Private Variables"

    Private wmiWatcher(4) As ManagementEventWatcher

#End Region

#Region "Public Properties"

    Private _HldEvents As New List(Of HandledWMIEvent)
    Public ReadOnly Property HandledEvents() As List(Of HandledWMIEvent)
        Get
            Return _HldEvents
        End Get
    End Property

#End Region

#Region "Enumerations"

    Enum tWMIWatcher As Integer
        WINACTIVStartWatcher = 1
        WINACTIVStopWatcher = 2
        WINRUNStartWatcher = 3
        WINRUNStopWatcher = 4
    End Enum

    Public Enum tWMIProcess
        WINRUN = 1
        WINACTIV = 2
    End Enum

    Private Function ProcessName(ByVal Process As tWMIProcess) As String
        Select Case Process
            Case tWMIProcess.WINACTIV
                Return "WINACTIV.EXE"
            Case tWMIProcess.WINRUN
                Return "WINRUN.EXE"
            Case Else
                Throw New Exception("Invalid WMI Process Type.")
        End Select
    End Function

    Public Enum tWMIEventState
        StateStart = 1
        StateStop = 2
    End Enum

    Private Function WMITable(ByVal State As tWMIEventState) As String
        Select Case State
            Case tWMIEventState.StateStart
                Return "Win32_ProcessStartTrace"
            Case tWMIEventState.StateStop
                Return "Win32_ProcessStopTrace"
            Case Else
                Throw New Exception("Invalid WMI State.")
        End Select
    End Function

#End Region

#Region "Initialisation"

    Sub New()

        wmiWatcher(tWMIWatcher.WINRUNStartWatcher) = _
            New ManagementEventWatcher( _
                GenerateWMIQuery( _
                    tWMIProcess.WINRUN, _
                    tWMIEventState.StateStart _
                ) _
            )
        With wmiWatcher(tWMIWatcher.WINRUNStartWatcher)
            AddHandler .EventArrived, AddressOf WINRUNStarted
            .Start()
        End With

        wmiWatcher(tWMIWatcher.WINRUNStopWatcher) = _
            New ManagementEventWatcher( _
                GenerateWMIQuery( _
                    tWMIProcess.WINRUN, _
                    tWMIEventState.StateStop _
                ) _
            )
        With wmiWatcher(tWMIWatcher.WINRUNStopWatcher)
            AddHandler .EventArrived, AddressOf WINRUNStopped
            .Start()
        End With

        wmiWatcher(tWMIWatcher.WINACTIVStartWatcher) = _
            New ManagementEventWatcher( _
                GenerateWMIQuery( _
                    tWMIProcess.WINACTIV, _
                    tWMIEventState.StateStart _
                ) _
            )
        With wmiWatcher(tWMIWatcher.WINACTIVStartWatcher)
            AddHandler .EventArrived, AddressOf WINACTIVStarted
            .Start()
        End With

        wmiWatcher(tWMIWatcher.WINACTIVStopWatcher) = _
            New ManagementEventWatcher( _
                GenerateWMIQuery( _
                    tWMIProcess.WINACTIV, _
                    tWMIEventState.StateStop _
                ) _
            )
        With wmiWatcher(tWMIWatcher.WINACTIVStopWatcher)
            AddHandler .EventArrived, AddressOf WINACTIVStopped
            .Start()
        End With

    End Sub

#End Region

#Region "WMI Queries"

    Private Function GenerateWMIQuery(ByVal Process As tWMIProcess, ByVal State As tWMIEventState) As WqlEventQuery
        Dim wmiQuery As New WqlEventQuery
        With wmiQuery
            .EventClassName = "Win32_ProcessStartTrace"
            .QueryString = String.Concat( _
                String.Format( _
                    "SELECT * FROM {0} ", WMITable(State) _
                ) & _
                "where ProcessName = ", """", ProcessName(Process), """" _
            )
        End With
        Return wmiQuery
    End Function

#End Region

#Region "WMI Event Handlers"

#Region "Base Handlers"

    Private Sub WINACTIVStarted(ByVal sender As Object, ByVal e As EventArrivedEventArgs)
        hNewWMIEvent(tWMIProcess.WINACTIV, tWMIEventState.StateStart, e)
    End Sub

    Private Sub WINACTIVStopped(ByVal sender As Object, ByVal e As EventArrivedEventArgs)
        hNewWMIEvent(tWMIProcess.WINACTIV, tWMIEventState.StateStop, e)
    End Sub

    Private Sub WINRUNStarted(ByVal sender As Object, ByVal e As EventArrivedEventArgs)
        hNewWMIEvent(tWMIProcess.WINRUN, tWMIEventState.StateStart, e)
    End Sub

    Private Sub WINRUNStopped(ByVal sender As Object, ByVal e As EventArrivedEventArgs)
        hNewWMIEvent(tWMIProcess.WINRUN, tWMIEventState.StateStop, e)
    End Sub

#End Region

    Private Sub hNewWMIEvent(ByVal Process As tWMIProcess, ByVal State As tWMIEventState, ByVal e As EventArrivedEventArgs)
        With e.NewEvent
            _HldEvents.Add( _
                New HandledWMIEvent( _
                    Process, _
                    State, _
                    .Properties("ParentProcessID").Value, _
                    .Properties("ProcessID").Value _
                ) _
            )
        End With
    End Sub

#End Region

#Region " IDisposable Support "
    Private disposedValue As Boolean = False        ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: free other state (managed objects).
            End If
            For Each wmiW As ManagementEventWatcher In wmiWatcher
                If Not IsNothing(wmiW) Then
                    With wmiW
                        Try
                            .Stop()
                            .Dispose()
                        Catch
                        End Try
                    End With
                End If
            Next
        End If
        Me.disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
