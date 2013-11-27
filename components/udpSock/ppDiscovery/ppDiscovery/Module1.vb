Imports udpSock
Imports tcpSock
Imports System.Xml
Imports svcMsgType

Module Module1

    Private WithEvents list As New udpSock.Listener(8090)
    Public Sub hBroadcast(ByVal broadcast As Byte()) Handles list.Broadcast_Rcvd

        Dim thisBroadcast As svcMsgXML
        Try
            thisBroadcast = New svcMsgXML(broadcast)
            Select Case thisBroadcast.Verb
                Case "request"
                    Select Case thisBroadcast.msgType
                        Case "config"                            
                            Using request As New msgConfigRequest(thisBroadcast.msgNode)

                                Console.WriteLine("Received config broadcast from service type {0} running on {1}.", _
                                  request.svcType, _
                                  request.Source _
                                )

                                Using cor As New Correspondant( _
                                    request.Source, _
                                    request.responsePort _
                                )
                                    Using response As New msgSendConfig(request.svcType)
                                        Using result As New msgGenericResponse(New svcMsgXML(cor.Send(response.toByte)).msgNode)
                                            Select Case result.ErrCde
                                                Case 200
                                                    Console.WriteLine("{0} service on {1} reports configuration ok.", request.svcType, request.Source)
                                                Case Else
                                                    Console.WriteLine("{0} service on {1} reports configuration fail.", request.svcType, request.Source)
                                            End Select
                                        End Using
                                    End Using
                                End Using
                            End Using
                    End Select
            End Select

        Catch ex As Exception

        End Try

    End Sub

    Sub Main()
        Console.ReadLine()
        list.Shutdown()
    End Sub

End Module
