Imports System.Threading
Imports System.Data.SqlClient

Public Class PingIMP

    Private _args() As String = Nothing
    Private _ip As System.Net.IPAddress
    Private _Timeout As Integer = 2000
    Private _cnRetry As Integer = 30000
    Private _TimeToLive As Integer = 32
    Private _DatSize As Integer = 32
    Private _Constr As String = Nothing

    Dim _AppName As String = "Ping Imp"
    Dim _LogName As String = "Application"

    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.
        _args = args
        StartIt()

    End Sub

    Protected Overrides Sub OnContinue()
        MyBase.OnContinue()

    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
        With BackgroundWorker1
            .CancelAsync()
        End With
        If _Constr.Length > 0 Then
            With BackgroundWorker2
                .CancelAsync()
            End With
        End If
    End Sub

    Private Sub StartIt()

        _ip = Nothing
        Dim state As String = Nothing
        Dim er As Integer = -1
        For Each arg As String In _args
            Select Case Trim(LCase(arg))
                Case "-ip", "/ip"
                    state = "ip"
                Case "-t", "/t"
                    state = "Timeout"
                Case "-ttl", "/ttl"
                    state = "TimeToLive"
                Case "-size", "/size"
                    state = "DatSize"
                Case "-constr", "/constr", "/cn", "-cn"
                    state = "cn"
                Case "/cnretry", "/retry", "-cnretry", "-retry"
                    state = "cnRetry"
                Case Else
                    Dim per As Integer
                    Try
                        With My.Settings
                            Select Case state
                                Case "ip"
                                    per = 2
                                    _ip = System.Net.IPAddress.Parse(arg)
                                    .ip = arg
                                Case "Timeout"
                                    per = 3
                                    _Timeout = CInt(arg)
                                    .TimeOut = _Timeout
                                Case "TimeToLive"
                                    per = 4
                                    _TimeToLive = CInt(arg)
                                    .TTL = _TimeToLive
                                Case "DatSize"
                                    per = 5
                                    _DatSize = CInt(arg)
                                    .Size = _DatSize
                                Case "cn"
                                    per = 6
                                    _Constr = CStr(arg)
                                    .ConStr = _Constr
                                Case "cnRetry"
                                    per = 7
                                    _cnRetry = CInt(arg)
                                    .cnRetry = _cnRetry
                            End Select
                            .Save()
                        End With
                    Catch ex As Exception
                        er = per
                    End Try

            End Select
        Next
        If IsNothing(_ip) Then
            ' No IP set so load from settings
            If My.Settings.ip.Length > 0 Then
                Try
                    _ip = System.Net.IPAddress.Parse(My.Settings.ip)
                Catch
                    er = 2
                End Try
            Else
                er = 1
            End If
        End If

        Select Case er
            Case 1
                ' Missing IP
                WriteToEventLog("Failed to start service: Missing IP address.", _
                    EventLogEntryType.Error)
            Case 2
                ' Bad IP
                WriteToEventLog("Failed to start service: Bad IP address.", _
                    EventLogEntryType.Error)
            Case 3
                ' Bad IP
                WriteToEventLog("Failed to start service: Bad Timeout value.", _
                    EventLogEntryType.Error)
            Case 4
                ' Bad IP
                WriteToEventLog("Failed to start service: Bad TimeToLive value.", _
                    EventLogEntryType.Error)
            Case 5
                ' Bad IP
                WriteToEventLog("Failed to start service: Bad DatSize value.", _
                    EventLogEntryType.Error)
            Case 6
                ' Bad Connection String
                WriteToEventLog("Failed to start service: Bad Connection String value.", _
                    EventLogEntryType.Error)
            Case 7
                ' Bad Connection retry value
                WriteToEventLog("Failed to start service: Bad Connection retry value.", _
                    EventLogEntryType.Error)
            Case Else
                ' All good
                With BackgroundWorker1
                    WriteToEventLog("Started the Ping Imp." & _
                    vbCrLf & "Pinging [" & _ip.ToString & "]", _
                        EventLogEntryType.Information)
                    .RunWorkerAsync()
                End With
                If _cnRetry > 0 And _Constr.Length > 0 Then
                    WriteToEventLog("Started the SQL Imp." & _
                     vbCrLf & "Checking connection [" & _Constr.ToString & "] every " & CStr(_cnRetry / 1000) & " second(s).", _
                         EventLogEntryType.Information)
                    With BackgroundWorker2
                        .RunWorkerAsync()
                    End With
                End If
        End Select

        If er > 0 Then
            Me.ExitCode = 999
            Me.Stop()
        End If

    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

        Dim imp As New ICMPClass
        Dim reply As ICMPClass.ICMPReplyStructure
        Dim opt As ICMPClass.ICMPOptions
        With opt
            .Timeout = My.Settings.TimeOut
            .DatSize = My.Settings.Size
            .TimeToLive = My.Settings.TTL
        End With

        Do While Not Me.BackgroundWorker1.CancellationPending
            reply = imp.Ping(_ip, opt)
            With reply
                If Not .Status = ICMPStatusEnum.Success Then
                    WriteToEventLog("Ping imp could not contact [" & _
                        _ip.ToString & "]" & vbCrLf & _
                        .Message, _
                        EventLogEntryType.Error)
                End If
            End With
            Dim waitTime As Integer = opt.Timeout - reply.RoundTripTime
            If waitTime < 100 Then waitTime = 100
            Thread.Sleep(waitTime)
        Loop

    End Sub

    Public Sub WriteToEventLog(ByVal Entry As String, _
        Optional ByVal EventType As EventLogEntryType = EventLogEntryType.Information)

        Dim objEventLog As New EventLog()

        Try
            'Register the App as an Event Source
            If Not Diagnostics.EventLog.SourceExists(_AppName) Then
                Diagnostics.EventLog.CreateEventSource(_AppName, _LogName)
            End If

            objEventLog.Source = _AppName

            'WriteEntry is overloaded; this is one
            'of 10 ways to call it
            objEventLog.WriteEntry(Entry, EventType)

        Catch Ex As Exception

        End Try

    End Sub

z


End Class
