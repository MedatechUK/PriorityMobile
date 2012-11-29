Imports System
Imports System.Management
Imports System.Reflection
Imports ConsoleApp
Imports System.threading
Imports Priority

Module hPriority

    Dim ws As New PriWebSVC.Service
    Dim sd As New Priority.SerialData
    Dim p As New Priority.Load

    Enum myRunMode As Integer
        Normal = 0
        Config = 1
    End Enum

    Dim WithEvents cApp As New ConsoleApp.CA
    Dim WINMENUStopWatcher As ManagementEventWatcher
    Dim WINMENUStartWatcher As ManagementEventWatcher
    Dim Closing As Boolean = False
    Dim UserID As Integer = -1
    Dim env As String = ""

    Sub Main()
        With cApp
            .StrDeliminator = "%"
            .RunMode = myRunMode.Normal
            .doWelcome(Assembly.GetExecutingAssembly())

            'Console.WriteLine("")
            'Console.WriteLine("Press any key to continue.")
            'Dim strInput As String = Console.ReadKey(False).ToString
            'While (strInput = "")
            '    Thread.Sleep(100)
            'End While

            Try
                .GetArgs(Command)
            Catch ex As Exception
                Console.WriteLine(ex.Message)
                .Quit = True
            End Try

            If UserID = -1 Then
                .Quit = True
                MsgBox("No Priority User ID was specified.")
            End If

            If Not NoProcess("hPriority") Then .Quit = True

            If Not .Quit Then
                Select Case .RunMode
                    Case myRunMode.Normal
                        PAYLOAD()
                End Select
            End If

            cApp.Finalize()

        End With
    End Sub

#Region "Switches"

    Private Sub cApp_Switch(ByVal StrVal As String, ByRef State As String, ByRef Valid As Boolean) Handles cApp.Switch
        Try
            With cApp
                Select Case StrVal
                    Case "u", "user"
                        State = "u"
                    Case "e", "env"
                        State = "e"
                    Case Else
                        Valid = False
                End Select
            End With
        Catch ex As Exception
            Console.Write(ex.Message)
        End Try
    End Sub

    Private Sub cApp_SwitchVar(ByVal State As String, ByVal StrVal As String, ByRef Valid As Boolean) Handles cApp.SwitchVar
        Try
            With cApp
                Select Case State
                    Case "u"
                        UserID = CInt(StrVal)
                    Case "e"
                        env = StrVal
                    Case Else
                        Valid = False
                End Select
            End With
        Catch ex As Exception
            Console.Write(ex.Message)
        End Try
    End Sub

#End Region

    Sub PAYLOAD()

        StartWatching()
        Do
            Do
                Thread.Sleep(1000)
            Loop Until Closing
        Loop Until NoProcess("winmenud")
        StopWatching()

        'USER (INT,17,'Int 1')
        'TOTIME (DATE,14,'End Time')
        Try
            With p
                ''.DebugFlag = True
                .Procedure = "ZEMG_WKHRREP"
                .Table = "ZEMG_WKHRREP"
                .RecordType1 = "T$USER, TOTIME"
                .RecordTypes = ","
            End With

            ' Type 1 records
            Dim t1() As String = { _
                                UserID, _
                                DateDiff(DateInterval.Minute, #1/1/1988#, Now).ToString _
                                }
            p.AddRecord(1) = t1

            ws.LoadData(sd.SerialiseDataArray(p.Data))

        Catch ex As Exception
            ' transaction failed
            MsgBox(ex.Message, MsgBoxStyle.Critical)

        End Try
    End Sub

#Region "WMI"

    Private Function NoProcess(ByVal processName As String) As Boolean
        NoProcess = True

        For Each p As Process In Process.GetProcesses()
            With p
                If .SessionId = Process.GetCurrentProcess.SessionId Then
                    Console.WriteLine(.ProcessName)
                    If UCase(.ProcessName) = UCase(processName) Then
                        If Not (Process.GetCurrentProcess.Id = .Id) Then
                            NoProcess = False
                            Closing = False
                            Exit Function
                        End If
                    End If
                End If
            End With
        Next

    End Function

#Region "Start / Stop WMI Listening"

    Private Sub StartWatching()

        WINMENUStartWatcher = New ManagementEventWatcher(GenerateStartQuery("WINMENUD.EXE"))
        AddHandler WINMENUStartWatcher.EventArrived, AddressOf WINMENUStarted
        WINMENUStartWatcher.Start()

        WINMENUStopWatcher = New ManagementEventWatcher(GenerateStopQuery("WINMENUD.EXE"))
        AddHandler WINMENUStopWatcher.EventArrived, AddressOf WINMENUStopped
        WINMENUStopWatcher.Start()

    End Sub

    Private Sub StopWatching()

        WINMENUStartWatcher.Stop()
        RemoveHandler WINMENUStartWatcher.EventArrived, AddressOf WINMENUStarted
        WINMENUStartWatcher = Nothing

        WINMENUStopWatcher.Stop()
        RemoveHandler WINMENUStopWatcher.EventArrived, AddressOf WINMENUStopped
        WINMENUStopWatcher = Nothing

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

#Region "WMI Event Handlers"

    Private Sub WINMENUStarted(ByVal sender As Object, ByVal e As EventArrivedEventArgs)

    End Sub
    Private Sub WINMENUStopped(ByVal sender As Object, ByVal e As EventArrivedEventArgs)
        Closing = True
    End Sub

#End Region

#End Region

End Module
