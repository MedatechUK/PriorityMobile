Public Class log : Inherits logMustInherit

    Public Sub New(ByRef sArg As StartArgs)
        Instantiate(sArg)
    End Sub

    Public Overrides Sub onTerminate()
        Using term As New msgLogRequest(ServiceType, EvtLogSource.SYSTEM, EvtLogVerbosity.Normal, LogEntryType.Warning)
            With term.LogData
                .AppendFormat("Stopping [{0}] service on {1}.", ServiceType, NetBiosName).AppendLine()
            End With
            LogEvent(term)
        End Using

    End Sub

    Public Overrides Sub doLog()
        Dim retry As Integer = 0
        With LogQueue
            Do
                Try
                    If .Count > 0 Then
                        Using logBcast As New msgSubscGetRequest(LogQueue)
                            Broadcast(logBcast.toByte, eBroadcastType.bcPublic)
                        End Using
                    End If
                    retry = 0

                Catch
                    Threading.Thread.Sleep(1000)
                    retry += 1

                Finally
                    Threading.Thread.Sleep(1000)

                End Try

            Loop Until (Closing And .Count = 0) Or (Closing And retry > 3)
            LogClose = True

        End With

    End Sub

    Public Overrides Sub Start(ByRef sArg As StartArgs)

        With sArg
            .StartLog.LogData.AppendFormat("Service [{0}] started on {1}.", ServiceType, NetBiosName).AppendLine()            
        End With

    End Sub

    Public Overrides Function BeginConfig(ByRef Request As svcMsgXML) As ServiceMessage

        Config = True
        Return New msgGenericResponse(True, Nothing)

    End Function

    Public Overrides Sub NewTCPMessage(ByRef Request As svcMsgXML, ByRef Response() As Byte)
        With Request
            Select Case .Verb
                Case eVerb.Request
                    Select Case .msgType
                        Case "log"
                            Using logRequest As New msgLogRequest(Request.msgNode)
                                Console.Write(logRequest.LogData.ToString)
                                LogEvent(logRequest)
                            End Using

                            Response = New msgGenericResponse(True, Nothing).toByte

                    End Select

            End Select

        End With

    End Sub

End Class