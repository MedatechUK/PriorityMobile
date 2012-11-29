Imports System
Imports System.Management
Imports system.io
Imports System.threading
Imports System.text

Public Module WINACTIVP

    Dim WINACTIVStartWatcher As ManagementEventWatcher
    Dim WINACTIVStopWatcher As ManagementEventWatcher

    Dim WINRUNStartWatcher As ManagementEventWatcher
    Dim WINRUNStopWatcher As ManagementEventWatcher

    Dim MyProcID As String = ""
    Dim WINRUNProcID As String = ""
    Dim WINACTIVProcID As String = ""
    Dim WINACTIVDone As Boolean = False
    Dim WINRUNDone As Boolean = False

#Region "Private Variables"
    Private _PRIORITYDIR As String
    Private _PRIORITYUSER As String
    Private _PRIORITYPWD As String
    Private _PROC As String
    Private _PRIORITYENV As String
#End Region

#Region "Public Properties"
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
#End Region

    Public Sub MAIN()

        StartWatching()

        Dim sOutput As String = ""
        Dim sErrs As String = ""
        Dim myProcess As Process = New Process()

        With myProcess
            With .StartInfo
                .FileName = "cmd.exe"
                .UseShellExecute = False
                .CreateNoWindow = True
                .RedirectStandardInput = True
            End With
            .Start()

            Console.WriteLine("My process # " & .Id)
            MyProcID = .Id

            Dim sIn As StreamWriter = myProcess.StandardInput
            With sIn
                .AutoFlush = True
                Dim cmd As String = "z:" & "\BIN.95\WINRUN.exe " & _
                    Chr(34) & Chr(34) & _
                    " " & "tabula" & _
                    " " & "Sund1al" & _
                    " " & "z:" & "\system\prep" & _
                    " " & "demo" & _
                    " WINACTIV -P " & Split("ZSFDC_TEST", ":")(0)

                .Write(cmd & _
                    System.Environment.NewLine)

                .Close()

            End With

            .Close()

        End With

        Dim tio As Integer = 0
        Do
            Thread.Sleep(1000)
            tio += 1
        Loop Until (WINRUNDone And WINACTIVDone) Or tio = 10

        If Not (WINRUNDone) Or Not (WINACTIVDone) Then
            If WINRUNProcID.Length > 0 Then KillProcess(WINRUNProcID)
            If WINACTIVProcID.Length > 0 Then KillProcess(WINACTIVProcID)
        End If

        StopWatching()

    End Sub

    Private Sub KillProcess(ByVal ProcessID As String)
        Try
            Dim thisProcess As Process = System.Diagnostics.Process.GetProcessById(CInt(ProcessID))
            thisProcess.Kill()
        Catch ex As Exception

        End Try
    End Sub

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


    Public Sub WINACTIVStarted(ByVal sender As Object, ByVal e As EventArrivedEventArgs)
        'Applications.Add(e.NewEvent.Properties("ProcessID").Value, New Word(e.NewEvent.Properties("ParentProcessID").Value, e.NewEvent.Properties("ProcessID").Value))
        Console.WriteLine(String.Concat("Application=WINACTIV, Start, Parent Process Id=", e.NewEvent.Properties("ParentProcessID").Value.ToString, ", Process Id=", e.NewEvent.Properties("ProcessID").Value.ToString))
        If e.NewEvent.Properties("ParentProcessID").Value.ToString = WINRUNProcID Then
            WINACTIVProcID = e.NewEvent.Properties("ProcessID").Value.ToString
        End If
    End Sub

    Public Sub WINACTIVStopped(ByVal sender As Object, ByVal e As EventArrivedEventArgs)

        'If Applications.ContainsKey(e.NewEvent.Properties("ProcessID").Value) Then
        '    Applications.Remove(e.NewEvent.Properties("ProcessID").Value)
        'End If
        Console.WriteLine(String.Concat("Application=WINACTIV, Stop, Parent Process Id=", e.NewEvent.Properties("ParentProcessID").Value.ToString, ", Process Id=", e.NewEvent.Properties("ProcessID").Value.ToString))
        If e.NewEvent.Properties("ProcessID").Value = WINACTIVProcID Then
            WINACTIVDone = True
        End If

    End Sub

    Public Sub WINRUNStarted(ByVal sender As Object, ByVal e As EventArrivedEventArgs)
        'Applications.Add(e.NewEvent.Properties("ProcessID").Value, New Word(e.NewEvent.Properties("ParentProcessID").Value, e.NewEvent.Properties("ProcessID").Value))
        If e.NewEvent.Properties("ParentProcessID").Value.ToString = MyProcID Then
            WINRUNProcID = e.NewEvent.Properties("ProcessID").Value.ToString
        End If
        Console.WriteLine(String.Concat("Application=WINRUN, Start, Parent Process Id=", e.NewEvent.Properties("ParentProcessID").Value.ToString, ", Process Id=", e.NewEvent.Properties("ProcessID").Value.ToString))
    End Sub

    Public Sub WINRUNStopped(ByVal sender As Object, ByVal e As EventArrivedEventArgs)

        'If Applications.ContainsKey(e.NewEvent.Properties("ProcessID").Value) Then
        '    Applications.Remove(e.NewEvent.Properties("ProcessID").Value)
        'End If
        Console.WriteLine(String.Concat("Application=WINRUN, Stop, Parent Process Id=", e.NewEvent.Properties("ParentProcessID").Value.ToString, ", Process Id=", e.NewEvent.Properties("ProcessID").Value.ToString))
        If e.NewEvent.Properties("ProcessID").Value = WINRUNProcID Then
            WINRUNDone = True
        End If
    End Sub

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

End Module