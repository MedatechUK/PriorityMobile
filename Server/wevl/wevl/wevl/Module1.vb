Imports System.Reflection

Module Module1

    Private ev As New ntEvtlog.evt
    Private WithEvents cApp As New ConsoleApp.CA

    Private LogSource As String = Nothing
    Private LogMessage As String = Nothing
    Private LogType As EventLogEntryType = EventLogEntryType.Information

    Enum myRunMode As Integer        
        LOG = 1
        REGISTER = 2
    End Enum

    Sub Main()

        With cApp
            .RunMode = myRunMode.LOG

            ev.AppName = Assembly.GetExecutingAssembly().GetName().Name
            ev.LogVerbosity = ntEvtlog.EvtLogVerbosity.Arcane

            .doWelcome(Assembly.GetExecutingAssembly())

            Try
                .GetArgs(Command)
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            If Not .Quit Then

                Select Case .RunMode
                    Case myRunMode.LOG
                        If IsNothing(LogSource) Then
                            Console.WriteLine("Please specify a name for the event log.")
                            .Quit = True
                        End If
                        If IsNothing(LogMessage) Then
                            Console.WriteLine("Please specify a message for the event log.")
                            .Quit = True
                        End If
                    Case myRunMode.REGISTER
                        If IsNothing(LogSource) Then
                            Console.WriteLine("Please specify a name for the event log.")
                            .Quit = True
                        End If
                End Select



                Select Case .RunMode
                    Case myRunMode.LOG
                        Console.WriteLine(String.Format("Writing [{1}] to event source [{0}]:", LogSource, logtypeDescriptor()))
                        With ev
                            .LogMode = ntEvtlog.EvtLogMode.EventLog
                            .AppName = LogSource
                            .Log( _
                                LogMessage, _
                                 LogType, _
                                ntEvtlog.EvtLogVerbosity.Normal _
                            )
                        End With

                    Case myRunMode.REGISTER
                        With ev
                            .RegisterLog(LogSource)
                        End With
                End Select

            End If

            cApp.Finalize()

        End With

    End Sub

    Private Sub cApp_Switch(ByVal StrVal As String, ByRef State As String, ByRef Valid As Boolean) Handles cApp.Switch
        Try
            With cApp
                Select Case StrVal.ToLower
                    Case "s", "source"
                        State = "s"
                    Case "m", "msg", "message"
                        State = "m"
                    Case "t", "type"
                        State = "t"
                    Case "r", "register"
                        cApp.RunMode = myRunMode.REGISTER
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

            If String.Compare(StrVal.Substring(StrVal.Length - 1), "\") = 0 Then
                StrVal = Left(StrVal, StrVal.Length - 1)
            End If

            With cApp
                Select Case State
                    Case "s"
                        LogSource = StrVal
                    Case "m"
                        LogMessage = StrVal
                    Case "t"
                        Select Case StrVal.ToLower
                            Case "error", "err", "er", "1"
                                LogType = EventLogEntryType.Error
                            Case "failureaudit", "fail", "16"
                                LogType = EventLogEntryType.FailureAudit
                            Case "successaudit", "success", "suc", "ok", "8"
                                LogType = EventLogEntryType.SuccessAudit
                            Case "warning", "warn", "2"
                                LogType = EventLogEntryType.Warning
                            Case "information", "info", "nfo", "4"
                                LogType = EventLogEntryType.Information
                            Case Else
                                Valid = False
                                Console.WriteLine("Unknown Log Type: {0}", StrVal)
                        End Select
                    Case Else
                        Valid = False
                End Select
            End With
        Catch ex As Exception
            Console.Write(ex.Message)
        End Try
    End Sub

    Private Function logtypeDescriptor() As String
        Select Case LogType
            Case EventLogEntryType.Error
                Return "EventLogEntryType.Error"
            Case EventLogEntryType.FailureAudit
                Return "EventLogEntryType.FailureAudit"
            Case EventLogEntryType.SuccessAudit
                Return EventLogEntryType.SuccessAudit
            Case EventLogEntryType.Warning
                Return "EventLogEntryType.Warning"
            Case EventLogEntryType.Information
                Return "EventLogEntryType.Information"
            Case Else
                Return "Unknown Type"
        End Select
    End Function

End Module
