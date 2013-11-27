Imports System.Xml
Imports svcMsgType

Module Module1

    Private WithEvents tcpList As New tcpSock.Listener(8091)
    Private bCast As New udpSock.Broadcaster(8090)
    Private config As Boolean = False

    Sub Main()

        Do Until config
            Using msg As New msgConfigRequest("q", 8091)
                Console.WriteLine("Broadcasting q service...")
                bCast.Broadcast(msg.toByte)
            End Using
            For i As Integer = 0 To 600
                Threading.Thread.Sleep(100)
                If config Then Exit Do
            Next
        Loop
        Console.ReadLine()
        tcpList.Shutdown()

    End Sub

    Private Sub hRequest(ByRef Request As Byte(), ByRef Response As Byte()) Handles tcpList.Request

        Try
            Dim thisRequest As New svcMsgXML(Request)
            With thisRequest
                Select Case .Verb
                    Case "request"
                        Select Case .msgType
                            Case "config"
                                Console.WriteLine("Received config from discovery server...")
                                Using configResponse As New msgGenericResponse(True, Nothing)
                                    Response = configResponse.toByte
                                End Using
                                config = True
                        End Select
                End Select
            End With

        Catch ex As Exception
            config = False
            Using msg As New msgGenericResponse(False, ex)
                Response = msg.toByte
            End Using

        End Try

    End Sub

End Module
