Imports System.Collections
Imports udpSock

Public Class Loading

End Class

Module Module1

    Private WithEvents bCast As New Broadcaster

    Sub Main()
        Dim q As New Queue(Of Loading)
        q.Enqueue(New Loading)
        While True
            bCast.Broadcast(Console.ReadLine)
        End While

    End Sub

    Public Sub NewBroadcast(ByVal Data As String) Handles bCast.Broadcast_Rcvd
        Console.Write(Data)
    End Sub

End Module
