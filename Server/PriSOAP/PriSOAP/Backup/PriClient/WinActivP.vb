Imports System
Imports System.Management
Imports system.io
Imports System.threading
Imports System.text
Imports ntEvtlog

Public Class WINACTIVP

    Public Shared MonitorLock As Object = New Object()
    Public Event ConsoleMessage(ByVal ConnectionID As String, ByVal str As String)

#Region "Private Variables"

    Private qLock As New Queue

    Dim WINACTIVStartWatcher As ManagementEventWatcher
    Dim WINACTIVStopWatcher As ManagementEventWatcher

    Dim WINRUNStartWatcher As ManagementEventWatcher
    Dim WINRUNStopWatcher As ManagementEventWatcher

    Dim MyProcID As String = ""
    Dim WINRUNProcID As String = ""
    Dim WINACTIVProcID As String = ""
    Dim WINACTIVDone As Boolean = False
    Dim WINRUNDone As Boolean = False

    Private _PRIORITYDIR As String
    Private _PRIORITYUSER As String
    Private _PRIORITYPWD As String
    Private _PROC As String
    Private _PRIORITYENV As String
    Private _ev As ntEvtlog.evt

#End Region

#Region "Public Properties"

    Private _result As Boolean = False
    Public ReadOnly Property result() As Boolean
        Get
            Return _result
        End Get
    End Property

    Public Property PRIORITYDIR() As String
        Get
            Return _PRIORITYDIR
        End Get
        Set(ByVal value As String)
            If _PRIORITYDIR <> value Then
                _PRIORITYDIR = value
            End If
        End Set
    End Property

    Public Property PRIORITYUSER() As String
        Get
            Return _PRIORITYUSER
        End Get
        Set(ByVal value As String)
            If _PRIORITYUSER <> value Then
                _PRIORITYUSER = value
            End If
        End Set
    End Property

    Public Property PRIORITYPWD() As String
        Get
            Return _PRIORITYPWD
        End Get
        Set(ByVal value As String)
            If _PRIORITYPWD <> value Then
                _PRIORITYPWD = value
            End If
        End Set
    End Property

    Public Property PROC() As String
        Get
            Return _PROC
        End Get
        Set(ByVal value As String)
            If _PROC <> value Then
                _PROC = value
            End If
        End Set
    End Property

    Public Property PRIORITYENV() As String
        Get
            Return _PRIORITYENV
        End Get
        Set(ByVal value As String)
            If _PRIORITYENV <> value Then
                _PRIORITYENV = value
            End If
        End Set
    End Property

    Public ReadOnly Property cmd() As String
        Get
            Return PRIORITYDIR & "\BIN.95\WINRUN.exe " & _
                                    Chr(34) & Chr(34) & _
                                    " " & PRIORITYUSER & _
                                    " " & PRIORITYPWD & _
                                    " " & PRIORITYDIR & "\system\prep" & _
                                    " " & PRIORITYENV & _
                                    " WINACTIV -P " & PROC
        End Get
    End Property

    Private _ConnectionID As String = Nothing
    Public Property ConnectionID() As String
        Get
            Return _ConnectionID
        End Get
        Set(ByVal value As String)
            _ConnectionID = value
        End Set
    End Property

    Private _sErrWait As String = ""
    Private Property sErr() As String
        Get
            Return _sErrWait
        End Get
        Set(ByVal value As String)
            _sErrWait = value
        End Set
    End Property

    Private _sOutWait As String = ""
    Private Property sOut() As String
        Get
            Return _sOutWait
        End Get
        Set(ByVal value As String)
            _sOutWait = value
        End Set
    End Property

    Private _timeout As Integer = 60
    Public Property TimeOut() As Integer
        Get
            Return _timeout
        End Get
        Set(ByVal value As Integer)
            _timeout = value
        End Set
    End Property

#End Region

#Region "initialisation"

    Sub New(Optional ByRef ev As ntEvtlog.evt = Nothing)
        If Not IsNothing(ev) Then
            _ev = ev
        End If
        StartWatching()
    End Sub

#End Region

#Region "public Subs"

    Public Sub Start()

        Monitor.Enter(MonitorLock)
        Try
            sOut = ""
            sErr = ""

            Me.SafeLog( _
            String.Format( _
                    "Running loading procedure [{1}] in environment [{2}] with a timeout of {3} seconds.", _
                    vbCrLf, _
                    PROC, _
                    PRIORITYENV, _
                    Me.TimeOut _
                ), _
                 EventLogEntryType.Information, _
                 ntEvtlog.EvtLogVerbosity.Verbose _
            )
            RunCmd()

            If sErr.Length > 0 Then
                Throw New Exception( _
                    String.Format( _
                            "Command [{1}]{0}Returned: [{2}].", _
                            vbCrLf, _
                            cmd, _
                            Left(sErr, sErr.Length - 2) _
                    ) _
                )
            End If

            Dim tio As Integer = 0
            Do
                Thread.Sleep(1000)
                tio += 1
            Loop Until (WINRUNDone And WINACTIVDone) Or tio >= TimeOut

            If Not (WINRUNDone) Or Not (WINACTIVDone) Then
                If WINRUNProcID.Length = 0 Then
                    Me.SafeLog( _
                    String.Format( _
                            "Priority Loading Procedure [{1}] timed-out after {2} seconds, but may have been sucsessful.{0}The WINRUN process start was not reported by the WMI.", _
                            vbCrLf, _
                            PROC, _
                            Me.TimeOut _
                        ), _
                         EventLogEntryType.FailureAudit, _
                         ntEvtlog.EvtLogVerbosity.Normal _
                    )
                Else
                    Me.SafeLog( _
                    String.Format( _
                            "Priority Loading Procedure [{1}] timed-out after {2} seconds.{0}Terminating processes...", _
                            vbCrLf, _
                            PROC, _
                            Me.TimeOut _
                        ), _
                         EventLogEntryType.FailureAudit, _
                         ntEvtlog.EvtLogVerbosity.Normal _
                    )
                    If WINRUNProcID.Length > 0 Then KillProcess("WINRUN", WINRUNProcID)
                    If WINACTIVProcID.Length > 0 Then KillProcess("WINACTIV", WINACTIVProcID)
                End If
            Else
                _result = True
                Me.SafeLog( _
                String.Format( _
                        "Priority Loading Procedure [{1}] completed in environment [{2}] after {3} seconds.", _
                        vbCrLf, _
                        PROC, _
                        PRIORITYENV, _
                        tio _
                    ), _
                     EventLogEntryType.SuccessAudit, _
                     ntEvtlog.EvtLogVerbosity.Verbose _
                )
            End If

        Catch ex As Exception
            Me.SafeLog( _
                String.Format( _
                    "An error occured whilst running Priority Procedure: {0}{1}{0}The error was was:{0}[{2}]", _
                    vbCrLf, _
                    cmd, _
                    ex.Message _
                ), _
                 EventLogEntryType.Error, _
                 ntEvtlog.EvtLogVerbosity.Normal _
            )

        Finally
            StopWatching()
            Monitor.Exit(MonitorLock)
        End Try

    End Sub

#End Region

#Region "Private Subs"

#Region "Start / Stop WMI Listening"

    Private Sub StartWatching()

        WINACTIVStartWatcher = New ManagementEventWatcher(GenerateStartQuery("WINACTIV.EXE"))
        AddHandler WINACTIVStartWatcher.EventArrived, AddressOf WINACTIVStarted
        WINACTIVStartWatcher.Start()

        WINACTIVStopWatcher = New ManagementEventWatcher(GenerateStopQuery("WINACTIV.EXE"))
        AddHandler WINACTIVStopWatcher.EventArrived, AddressOf WINACTIVStopped
        WINACTIVStopWatcher.Start()

        WINRUNStartWatcher = New ManagementEventWatcher(GenerateStartQuery("WINRUN.EXE"))
        AddHandler WINRUNStartWatcher.EventArrived, AddressOf WINRUNStarted
        WINRUNStartWatcher.Start()

        WINRUNStopWatcher = New ManagementEventWatcher(GenerateStopQuery("WINRUN.EXE"))
        AddHandler WINRUNStopWatcher.EventArrived, AddressOf WINRUNStopped
        WINRUNStopWatcher.Start()

    End Sub

    Private Sub StopWatching()

        WINRUNStartWatcher.Stop()
        RemoveHandler WINRUNStartWatcher.EventArrived, AddressOf WINRUNStarted
        WINRUNStartWatcher = Nothing

        WINRUNStopWatcher.Stop()
        RemoveHandler WINRUNStopWatcher.EventArrived, AddressOf WINRUNStopped
        WINRUNStopWatcher = Nothing

        WINACTIVStartWatcher.Stop()
        RemoveHandler WINACTIVStartWatcher.EventArrived, AddressOf WINACTIVStarted
        WINACTIVStartWatcher = Nothing

        WINACTIVStopWatcher.Stop()
        RemoveHandler WINACTIVStopWatcher.EventArrived, AddressOf WINACTIVStopped
        WINACTIVStopWatcher = Nothing

    End Sub

#End Region

#Region "WMI Event Handlers"

    Private Sub WINACTIVStarted(ByVal sender As Object, ByVal e As EventArrivedEventArgs)
        Monitor.Enter(qLock)
        Try
            If Strings.StrComp(e.NewEvent.Properties("ParentProcessID").Value.ToString, WINRUNProcID) = 0 Then
                WINACTIVProcID = e.NewEvent.Properties("ProcessID").Value.ToString
                Me.SafeLog( _
                    String.Format( _
                        "The WINRUN process with PID [{1}]{0}   >Spawned a new WINACTIV process with PID [{2}].", _
                        vbCrLf, _
                        e.NewEvent.Properties("ParentProcessID").Value.ToString, _
                        e.NewEvent.Properties("ProcessID").Value.ToString _
                    ), _
                     EventLogEntryType.Information, _
                     ntEvtlog.EvtLogVerbosity.VeryVerbose _
                )
            End If
        Catch ex As Exception
            Me.SafeLog( _
                String.Format( _
                    "An error occured in the WINACTIV start handler.", _
                    vbCrLf, _
                    cmd, _
                    ex.Message _
                ), _
                 EventLogEntryType.Error, _
                 ntEvtlog.EvtLogVerbosity.Normal _
            )
        Finally
            Monitor.Exit(qLock)
        End Try

    End Sub

    Private Sub WINACTIVStopped(ByVal sender As Object, ByVal e As EventArrivedEventArgs)
        Monitor.Enter(qLock)
        Try
            If Strings.StrComp(e.NewEvent.Properties("ProcessID").Value.ToString, WINACTIVProcID, CompareMethod.Text) = 0 Then
                WINACTIVDone = True
                Me.SafeLog( _
                    String.Format( _
                        "The WINACTIV process with PID [{1}]{0}   >terminated sucsessfully.", _
                        vbCrLf, _
                        e.NewEvent.Properties("ProcessID").Value.ToString _
                    ), _
                     EventLogEntryType.Information, _
                     ntEvtlog.EvtLogVerbosity.VeryVerbose _
                )
            End If
        Catch ex As Exception
            Me.SafeLog( _
                String.Format( _
                    "An error occured in the WINACTIV stop handler.{0}{2}", _
                    vbCrLf, _
                    cmd, _
                    ex.Message _
                ), _
                 EventLogEntryType.Error, _
                 ntEvtlog.EvtLogVerbosity.Normal _
            )
        Finally
            Monitor.Exit(qLock)
        End Try
    End Sub

    Private Sub WINRUNStarted(ByVal sender As Object, ByVal e As EventArrivedEventArgs)
        Monitor.Enter(qLock)
        Try
            If Strings.StrComp(e.NewEvent.Properties("ParentProcessID").Value.ToString, MyProcID, CompareMethod.Text) = 0 Then
                WINRUNProcID = e.NewEvent.Properties("ProcessID").Value.ToString
                Me.SafeLog( _
                    String.Format( _
                        "This thread, with a PID of [{1}]{0}   >Spawned a new WINRUN process with PID of [{2}].", _
                        vbCrLf, _
                        e.NewEvent.Properties("ParentProcessID").Value.ToString, _
                        e.NewEvent.Properties("ProcessID").Value.ToString _
                    ), _
                     EventLogEntryType.Information, _
                     ntEvtlog.EvtLogVerbosity.VeryVerbose _
                )
            End If
        Catch ex As Exception
            Me.SafeLog( _
                String.Format( _
                    "An error occured in the WINRUN start handler.", _
                    vbCrLf, _
                    cmd, _
                    ex.Message _
                ), _
                 EventLogEntryType.Error, _
                 ntEvtlog.EvtLogVerbosity.Normal _
            )
        Finally
            Monitor.Exit(qLock)
        End Try
    End Sub

    Private Sub WINRUNStopped(ByVal sender As Object, ByVal e As EventArrivedEventArgs)
        Monitor.Enter(qLock)
        Try
            If Strings.StrComp(e.NewEvent.Properties("ProcessID").Value.ToString, WINRUNProcID, CompareMethod.Text) = 0 Then
                Me.SafeLog( _
                    String.Format( _
                        "The WINRUN process with PID [{1}]{0}   >Terminated sucsessfully.", _
                        vbCrLf, _
                        e.NewEvent.Properties("ProcessID").Value.ToString _
                    ), _
                     EventLogEntryType.Information, _
                     ntEvtlog.EvtLogVerbosity.VeryVerbose _
                )
                WINRUNDone = True
            End If
        Catch ex As Exception
            Me.SafeLog( _
                String.Format( _
                    "An error occured in the WINRUN stop handler.{0}{2}", _
                    vbCrLf, _
                    cmd, _
                    ex.Message _
                ), _
                 EventLogEntryType.Error, _
                 ntEvtlog.EvtLogVerbosity.Normal _
            )
        Finally
            Monitor.Exit(qLock)
        End Try
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

    Private Sub KillProcess(ByVal ProcessName As String, ByVal ProcessID As String)
        Try
            Me.SafeLog( _
                String.Format( _
                    "Ending process {0} with PID #{1}.", _
                    ProcessName, _
                    ProcessID _
                ), _
                 EventLogEntryType.Information, _
                 ntEvtlog.EvtLogVerbosity.Arcane _
            )
            Dim thisProcess As Process = System.Diagnostics.Process.GetProcessById(CInt(ProcessID))
            thisProcess.Kill()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub SafeLog(ByVal Entry As String, ByVal EventType As LogEntryType, ByVal Verbosity As ntEvtlog.EvtLogVerbosity)
        If Not IsNothing(ConnectionID) Then
            RaiseEvent ConsoleMessage(ConnectionID, Entry)
        End If
        Try
            If IsNothing(_ev) Then Throw New Exception("No event object specified.")
            _ev.Log( _
                Entry, _
                EventType, _
                Verbosity _
              )
        Catch exep As Exception
            Console.WriteLine( _
                "Failed to write to the [{0} {1}] log.{4}" & _
                "The error reported was: {4}{2}{4}" & _
                "The event could not be written to the log because: {4}{3}{4}", _
                _ev.LogName, _ev.AppName, Entry, exep.Message, vbCrLf _
            )
        End Try
    End Sub

    Private Sub RunCmd()

        Dim myProcess As Process = New Process()

        With myProcess

            With .StartInfo
                .FileName = "cmd.exe"
                .UseShellExecute = False
                .CreateNoWindow = True
                .RedirectStandardInput = True
                .RedirectStandardOutput = True
                .RedirectStandardError = True
            End With

            .Start()
            MyProcID = .Id

            AddHandler .OutputDataReceived, AddressOf sOutEndRead
            AddHandler .ErrorDataReceived, AddressOf sErrEndRead

            .BeginOutputReadLine()
            .BeginErrorReadLine()

            Me.SafeLog( _
                String.Format( _
                    "Started Prority Loading thread with PID {1}{0}[{2}]", _
                    vbCrLf, _
                    MyProcID, _
                    cmd _
                ), _
                 EventLogEntryType.Information, _
                 ntEvtlog.EvtLogVerbosity.VeryVerbose _
            )

            Dim sIn As StreamWriter = .StandardInput
            With sIn
                .AutoFlush = True
                .Write(cmd & _
                   System.Environment.NewLine)
                .Write("exit" & System.Environment.NewLine)
            End With

            Do Until myProcess.HasExited
                Thread.Sleep(100)
            Loop

            .CancelOutputRead()
            .CancelErrorRead()

        End With

        'out = Split(Split(out, cmd & vbCrLf)(1), "exit")(0)
        'Dim pt As String = Split(out, vbCrLf)(UBound(Split(out, vbCrLf)))

        'out = Replace(out, pt, "")
        'Do While InStr(out, vbCrLf & vbCrLf) > 0
        '    out = Replace(out, vbCrLf & vbCrLf, vbCrLf)
        'Loop
        'er = Replace(er, pt, "")
        'Do While InStr(er, vbCrLf & vbCrLf) > 0
        '    er = Replace(er, vbCrLf & vbCrLf, vbCrLf)
        'Loop

        'If Right(out, 2) = vbCrLf Then out = Left(out, out.Length - 2)
        'If Right(er, 2) = vbCrLf Then er = Left(er, er.Length - 2)

    End Sub

    Private Sub sOutEndRead(ByVal sendingProcess As Object, _
        ByVal outLine As DataReceivedEventArgs)
        Try
            sOut += outLine.Data.ToString & vbCrLf
        Catch
        End Try
    End Sub

    Private Sub sErrEndRead(ByVal sendingProcess As Object, _
        ByVal outLine As DataReceivedEventArgs)
        Try
            sErr += outLine.Data.ToString & vbCrLf
        Catch
        End Try
    End Sub

#End Region

End Class