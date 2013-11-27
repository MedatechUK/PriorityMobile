Public Class prisql : Inherits priSqlMustInherit

#Region "Initialisation and finalisation"

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

    Public Overrides Sub NewTCPMessage(ByRef Request As svcMsgXML, ByRef Response() As Byte)
        With Request
            Select Case .Verb
                Case eVerb.Request
                    Select Case .msgType
                        Case "sql"
                            Response = BeginConfig(Request).toByte

                    End Select
            End Select
        End With
    End Sub

    Public Overrides Sub NewUDPMessage(ByRef Request As svcMsgXML)
        With Request
            Select Case .Verb
                Case eVerb.Request
                    Select Case .msgType
                        Case "config"
                            ProcessConfigRequest(Request)

                    End Select
            End Select
        End With
    End Sub

    Public Overrides Sub Start(ByRef sArg As StartArgs)

        ' todo - populate local variables
        ' SQLInstance
        ' Username
        ' Password
        ' PriDrive        
        ' Environment

        sArg.StartLog.LogData.AppendFormat("Service [{0}] started on {1}.", ServiceType, NetBiosName).AppendLine()

    End Sub

    Public Overrides Function BeginConfig(ByRef Request As svcMsgXML) As ServiceMessage

        Console.WriteLine()
        Using thisRequest As New msgSendConfig(Request.msgNode)
            LogServer = thisRequest.LogServer
            logPort = thisRequest.logPort
        End Using
        Config = True
        Return New msgGenericResponse(True, Nothing)

    End Function

    Private Function ProcessConfigRequest(ByRef Request As svcMsgXML) As msgGenericResponse

        Using thisRequest As New msgConfigRequest(Request.msgNode)
            Console.WriteLine()
            Console.Write("Received config broadcast from service type {0} running on {1}.", _
              thisRequest.svcType, _
              thisRequest.Source _
            )
            Console.WriteLine()

            Using thisresponse = New msgSendConfig(thisRequest.svcType, LogServer, logPort)
                Using cor As New iClient( _
                    thisRequest.Source, _
                    thisRequest.responsePort _
                )
                    Return New msgGenericResponse( _
                        New svcMsgXML( _
                            eProtocolType.tcp, _
                            cor.Send(thisresponse.toByte) _
                        ).msgNode _
                    )
                End Using
            End Using
        End Using

    End Function

End Class
