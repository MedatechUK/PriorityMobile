Imports System.Threading

Public MustInherit Class Subscriber : Inherits subscriberMustInherit

    Public MustOverride ReadOnly Property myFilter() As msgFilter
    Public MustOverride Sub NewLogEntry(ByRef logEntry As msgLogRequest)

    Public Sub Unsubscribe()
        Shutdown()
    End Sub

    Public Overrides Sub doLog()
        Dim retry As Integer = 0
        With LogQueue
            Do
                Try
                    If .Count > 0 Then
                        NewLogEntry(.Dequeue())
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

        With sArg.StartLog.LogData
            .AppendFormat("Starting [{0}] subscriber on {1}.", ServiceType, NetBiosName).AppendLine()            
        End With

    End Sub

    Public Overrides Sub NewUDPMessage(ByRef Request As svcMsgXML)
        With Request
            Select Case .Verb
                Case eVerb.Request
                    Select Case .msgType
                        Case "get"
                            Using response As New msgSubscGetRequest( _
                                New svcMsgXML( _
                                    eProtocolType.tcp, _
                                     Request.toByte _
                                ).msgNode _
                            )

                                For Each msg As msgLogRequest In response.logMsg
                                    If myFilter.Match(msg) Then
                                        LogEvent(msg)
                                    End If
                                Next

                            End Using
                    End Select
            End Select
        End With

    End Sub

    Public Overrides Function BeginConfig(ByRef Request As svcMsgXML) As ServiceMessage


    End Function

End Class
