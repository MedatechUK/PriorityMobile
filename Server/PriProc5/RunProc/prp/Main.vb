Imports PriPROC.wmiQuery
Imports System.Threading
Imports System.Diagnostics

Module Main

#Region "Private Variables"

    Private HasEnded As Boolean = False
    Private LogBuilder As New System.Text.StringBuilder
    Private TimeWait As Integer = 500

    Private RunProc As Integer = -1
    Private ActProc As Integer = -1

#End Region

#Region "Properties"

    Private ReadOnly Property ThisProcess() As Integer
        Get
            Return Process.GetCurrentProcess().Id
        End Get
    End Property

    Private _RunTime As Integer = 0
    Private ReadOnly Property RunTime() As Integer
        Get
            _RunTime += TimeWait
            Return _RunTime
        End Get
    End Property

    Private _TimeOutMin As Integer = 2
    Private ReadOnly Property TimeOutMillisec()
        Get
            Return _TimeOutMin * 60 * 1000
        End Get
    End Property

#End Region

#Region "Initialisation"

    Sub Main(ByVal Args() As String)

        Dim sArg As New clArg(Args)
        If CheckArgs(sArg) Then
            Using wmi As New wmiQuery
                StartLoad(sArg)
                Do
                    Thread.Sleep(TimeWait)
                    CheckEvents(wmi)
                Loop Until HasEnded _
                    Or RunTime > TimeOutMillisec _
                    Or (ActProc = -1 And RunTime > 10000)

                If Not HasEnded Then
                    CleanUp(wmi)
                    Select Case ActProc = -1
                        Case True
                            Environment.ExitCode = Ex_WINACTIV_Fail
                        Case Else
                            Environment.ExitCode = Ex_Timeout
                    End Select
                Else
                    Environment.Exit(Ex_OK)
                End If
            End Using
        End If

    End Sub

#End Region

#Region "Methods"

    Private Function CheckArgs(ByRef sArg As clArg) As Boolean
        With sArg
            If .Keys.Contains("?") Then
                Console.Write(My.Resources.syntax)
                Environment.Exit(Ex_OK)
            Else
                If Not .Keys.Contains("dir") _
                Or Not .Keys.Contains("user") _
                Or Not .Keys.Contains("pwd") _
                Or Not .Keys.Contains("env") _
                Or Not .Keys.Contains("proc") _
                Then                    
                    Console.Write("Missing Parameter. Please seek /help.")
                    Environment.Exit(Ex_MissingParameter)
                Else
                    If sArg.Keys.Contains("wait") Then
                        If Not IsNumeric(sArg("wait")) Then
                            Console.Write("Invalid Wait value. Please seek /help.")
                            Environment.Exit(Ex_MissingParameter)
                        ElseIf CInt(sArg("wait")) > 5 Then
                            Console.Write("Mamimum wait value is 5 minutes. Please seek /help.")
                            Environment.Exit(Ex_MissingParameter)
                        End If
                    Else
                        _TimeOutMin = CInt(sArg("wait"))
                    End If
                    Return True
                End If
            End If
        End With
    End Function

    Private Sub StartLoad(ByRef sArg As clArg)

        Dim myProcess As Process = New Process()
        With myProcess

            Try
                With .StartInfo
                    .WorkingDirectory = sArg("dir")
                    .FileName = String.Format("{0}\BIN.95\WINRUN.exe", sArg("dir"))
                    .Arguments = String.Format( _
                                    "{5}{5} {1} {2} {0}\system\prep {3} WINACTIV -P {4}", _
                                        sArg("dir"), _
                                        sArg("user"), _
                                        sArg("pwd"), _
                                        sArg("env"), _
                                        sArg("proc"), _
                                        "'" _
                                )
                    .UseShellExecute = False
                    .CreateNoWindow = True
                    .RedirectStandardInput = True

                End With

                .Start()

            Catch ex As Exception
                Console.WriteLine("{0}\BIN.95\WINRUN.exe failed to start.", sArg("dir"))
                Console.WriteLine("{0}", ex.Message)
                Environment.Exit(Ex_WINRUN_Fail)
            End Try

            Console.WriteLine("Started WINRUN with process ID {0} and a loading timeout of {1} minutes.", _
                .Id, _
                _TimeOutMin.ToString _
            )

        End With

    End Sub

    Private Sub CheckEvents(ByRef wmi As wmiQuery)
        With wmi
            If (RunProc = -1) Then
                For Each hld As HandledWMIEvent In .HandledEvents
                    If Not IsNothing(hld) Then
                        If hld.Name = tWMIProcess.WINRUN _
                            And hld.EventState = tWMIEventState.StateStart _
                            And hld.ParentProcess = ThisProcess Then
                            RunProc = hld.Process
                            Console.WriteLine("{2}: Started [{0}] process with ID [{1}]", "WINRUN", RunProc.ToString, Now.ToString("dd/MM/yyy hh:mm:ss"))
                            Exit For
                        End If
                    End If
                Next
            End If
            If (RunProc = -1) Then Exit Sub
            If (ActProc = -1) Then
                For Each hld As HandledWMIEvent In .HandledEvents
                    If Not IsNothing(hld) Then
                        If hld.Name = tWMIProcess.WINACTIV _
                            And hld.EventState = tWMIEventState.StateStart _
                            And hld.ParentProcess = RunProc Then
                            ActProc = hld.Process
                            Console.WriteLine("{2}: Started [{0}] process with ID [{1}]", "WINACTIV", ActProc.ToString, Now.ToString("dd/MM/yyy hh:mm:ss"))
                            Exit For
                        End If
                    End If
                Next
            End If
            If (ActProc = -1) Then Exit Sub
            For Each hld As HandledWMIEvent In .HandledEvents
                If Not IsNothing(hld) Then
                    If hld.Name = tWMIProcess.WINACTIV _
                        And hld.EventState = tWMIEventState.StateStop _
                        And hld.Process = ActProc Then
                        Console.WriteLine("{2}: Finished [{0}] process with ID [{1}]", "WINACTIV", ActProc.ToString, Now.ToString("dd/MM/yyy hh:mm:ss"))

                        HasEnded = True

                        Exit For
                    End If
                End If
            Next
        End With
    End Sub

    Private Sub CleanUp(ByRef wmi As wmiQuery)
        With wmi
            Console.WriteLine("Loading timeout elapsed @{0}.", Now.ToString("dd/MM/yyy hh:mm:ss"))
            Console.WriteLine("Begin Process Clean-up.")
            Console.WriteLine("I saw the following threads...")
            For Each hld As HandledWMIEvent In .HandledEvents
                If Not IsNothing(hld) Then
                    Select Case hld.EventState
                        Case tWMIEventState.StateStart
                            Console.WriteLine("Process [{0}] with PID [{1}] started by PID [{2}]", hld.Name, hld.Process, hld.ParentProcess)
                        Case tWMIEventState.StateStop
                            Console.WriteLine("Process [{0}] with PID [{1}] terminating", hld.Name, hld.Process)
                    End Select
                End If
            Next

            If Not IsNothing(ActProc) Then
                Try
                    Console.WriteLine("Killing Process [{0}] with process ID [{1}]", "WINACTIV", ActProc.ToString)
                    Dim P As Process = Process.GetProcessById(ActProc)
                    P.Kill()
                Catch
                    Console.WriteLine("Kill process by ID failed. Killing all WINACTIV processes.")
                    For Each P As Process In Process.GetProcessesByName("WINACTIV")
                        With P
                            Console.WriteLine("Killing [{0}] process with ID [{1}]", "WINACTIV", P.Id.ToString)
                            P.Kill()
                        End With
                    Next
                End Try
            Else
                Console.WriteLine("A WINACTIV process ID was not found. Killing all WINACTIV processes.")
                For Each P As Process In Process.GetProcessesByName("WINACTIV")
                    With P
                        Console.WriteLine("Killing [{0}] process with ID [{1}]", "WINACTIV", P.Id.ToString)
                        P.Kill()
                    End With
                Next
            End If
        End With
    End Sub

#End Region

End Module
