Imports System.Xml
Imports System.Threading

Public Class q : Inherits qMustInherit

    Private ConfigThread As Thread

#Region "Initialisation and Finalisation"

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

#End Region

    Public Overrides Sub Start(ByRef sArg As StartArgs)

        ConfigThread = New Thread(AddressOf ConnectDiscovery)
        With ConfigThread
            .Name = String.Format("{0}_Config", ServiceType)
            .IsBackground = True
            .Start()
        End With
        sArg.StartLog.LogData.AppendFormat("Service [{0}] started on {1}.", ServiceType, NetBiosName).AppendLine()

    End Sub

    Private Sub ConnectDiscovery()

        Console.Write("Broadcasting config request for {0} service", ServiceType)
        Do Until Config Or Closing
            Using msg As New msgConfigRequest(ServiceType, ServicePort)
                Console.Write(".")
                Broadcast(msg.toByte, eBroadcastType.bcPrivate)
            End Using

            For i As Integer = 0 To BroadcastDelay
                Threading.Thread.Sleep(100)
                If Config Or Closing Then Exit Do
            Next
        Loop

    End Sub

    Public Overrides Function BeginConfig(ByRef Request As svcMsgXML) As ServiceMessage

        Console.WriteLine("Received config from discovery server...")
        Dim serverConfig As New msgSendConfig(Request.msgNode)
        Dim ret As ServiceMessage

        With Request.msgNode
            Using log As New msgLogRequest(ServiceType, EvtLogVerbosity.Arcane, LogEntryType.Information)
                Try
                    LogServer = serverConfig.LogServer
                    logPort = serverConfig.logPort

                    log.LogData.Append("Opening local IIS config...")
                    Dim testConnection As String = iisFolder
                    log.LogData.Append("Ok.").AppendLine()

                    Config = True
                    ret = New msgGenericResponse(True, Nothing)

                Catch ex As Exception
                    With log
                        .EntryType = LogEntryType.Err
                        .Verbosity = EvtLogVerbosity.Normal
                        .LogData.Append("Failed.").AppendLine()
                        .LogData.AppendFormat("{0}", ex.Message).AppendLine()
                    End With

                    ret = New msgGenericResponse(False, ex)

                Finally
                    Using cor As New iClient( _
                        LogServer, _
                        logPort _
                    )
                        Dim logResp As msgGenericResponse = New msgGenericResponse( _
                            New svcMsgXML( _
                                eProtocolType.tcp, _
                                cor.Send(log.toByte) _
                            ).msgNode _
                        )
                    End Using

                    If Not Config Then Shutdown()

                End Try

            End Using
        End With
        Return ret

    End Function

    Public Overrides Sub NewTCPMessage(ByRef Request As svcMsgXML, ByRef Response() As Byte)
        With Request
            Select Case .Verb
                Case eVerb.Request
                    Select Case .msgType
                        Case "config"
                            Console.WriteLine()
                            Response = BeginConfig(Request).toByte

                    End Select

                Case eVerb.Response
                    Select Case .msgType
                        Case "config"

                    End Select

            End Select

        End With
    End Sub

End Class