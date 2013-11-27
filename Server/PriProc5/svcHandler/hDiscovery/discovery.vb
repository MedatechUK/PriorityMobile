Imports System.Threading

Public Class discovery : Inherits discoveryMustInherit

    Private RegisteredServices As Dictionary(Of eServicePorts, RegisteredService)
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

    Private Function StartSequence(ByVal thisRequest As msgConfigRequest) As Boolean
        With RegisteredServices
            If .Keys.Contains(eServicePorts.prisql) Then
                Return True

            Else
                Return False

            End If
        End With
    End Function

    Public Overrides Sub Start(ByRef sArg As StartArgs)

        RegisteredServices = New Dictionary(Of eServicePorts, RegisteredService)
        ConfigThread = New Thread(AddressOf ConnectPriSQL)
        With ConfigThread
            .Name = String.Format("{0}_Config", ServiceType)
            .IsBackground = True
            .Start()
        End With

        sArg.StartLog.LogData.AppendFormat("Service [{0}] started on {1}.", ServiceType, NetBiosName).AppendLine()

    End Sub

    Private Sub ConnectPriSQL()

        Console.WriteLine("{0} service on {1} broadcasting request for data service.", ServiceType, NetBiosName)
        Do Until RegisteredServices.Keys.Contains(eServicePorts.prisql) Or Closing
            Using msg As New msgConfigRequest(ServiceType, ServicePort)
                Console.Write(".")
                Broadcast(msg.toByte, eBroadcastType.bcPublic)
            End Using

            For i As Integer = 0 To BroadcastDelay
                Threading.Thread.Sleep(100)
                If RegisteredServices.Keys.Contains(eServicePorts.prisql) Or Closing Then Exit Do
            Next
        Loop

    End Sub

    Public Overrides Sub NewTCPMessage(ByRef Request As svcMsgXML, ByRef Response() As Byte)

        With Request
            Select Case .Verb
                Case eVerb.Request
                    Select Case .msgType
                        Case "config"
                            RegisteredServices.Add(eServicePorts.prisql, New RegisteredService(eServicePorts.prisql, Request.Source))
                            Response = New msgGenericResponse(True, Nothing).toByte
                            Console.WriteLine()
                            Console.WriteLine("Data service on {0} responded.", Request.Source)
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
                            Using thisRequest As New msgConfigRequest(Request.msgNode)
                                If StartSequence(thisRequest) Then
                                    Using result As msgGenericResponse = ProcessConfigRequest(Request)
                                        If Not (result.ErrCde = "200") Then
                                            Console.Write("Service {0}:{1} failed configuration.", thisRequest.svcType, thisRequest.responsePort)
                                        Else
                                            Console.Write("Service {0}:{1} started.", thisRequest.svcType, thisRequest.responsePort)

                                            RegisteredServices.Add( _
                                                ServiceFromDescriptor( _
                                                    thisRequest.svcType _
                                                ), _
                                                New RegisteredService( _
                                                    ServiceFromDescriptor( _
                                                        thisRequest.svcType _
                                                    ), _
                                                    thisRequest.Source _
                                                ) _
                                            )

                                            With RegisteredServices.Keys
                                                Config = .Contains(eServicePorts.log) And .Contains(eServicePorts.prisql)
                                            End With

                                        End If
                                    End Using
                                End If
                            End Using
                    End Select

            End Select

        End With

    End Sub

    Public Overrides Function BeginConfig(ByRef Request As svcMsgXML) As ServiceMessage

        'Console.WriteLine()

        Using thisRequest As New msgConfigRequest(Request.msgNode)
            LogServer = thisRequest.Source
            logPort = thisRequest.responsePort
        End Using

        Using thisRequest As New msgConfigRequest(Request.msgNode)
            Return New msgSendConfig(thisRequest.svcType, LogServer, logPort)
        End Using

    End Function

    Private Function ProcessConfigRequest(ByRef Request As svcMsgXML) As msgGenericResponse

        Dim ret As ServiceMessage = New msgGenericResponse(True, Nothing)
        Dim thisresponse As ServiceMessage = Nothing

        Using thisRequest As New msgConfigRequest(Request.msgNode)
            Console.WriteLine()
            Console.Write("Received config broadcast from service type {0} running on {1}.", _
              thisRequest.svcType, _
              thisRequest.Source _
            )
            Console.WriteLine()

            If thisRequest.svcType = "log" Then
                thisresponse = BeginConfig(Request)
            Else
                thisresponse = New msgSendConfig(thisRequest.svcType, LogServer, logPort)
            End If

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

    End Function

End Class
