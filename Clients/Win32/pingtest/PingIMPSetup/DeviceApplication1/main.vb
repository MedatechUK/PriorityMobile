Imports System
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports Microsoft.VisualBasic
Imports System.Threading

Module tcpServ

    Friend server As TcpListener
    Dim stopping As Boolean = False

    Sub MAIN()

        Dim myThread As Thread
        myThread = New Thread(New ThreadStart(AddressOf listen))
        myThread.Start()

        Thread.CurrentThread.Join()

    End Sub

    Private Sub listen()
        ' Enter the listening loop.   
        Dim localAddr As IPAddress = IPAddress.Parse("127.0.0.1")
        server = New TcpListener(localAddr, 8022)
        server.Start()

        While Not stopping
            If server.Pending Then
                Do Until Not server.Pending
                    Dim myThread As Thread
                    myThread = New Thread(New ThreadStart(AddressOf Connect))
                    myThread.Start()
                    Thread.Sleep(10)
                Loop
            End If
            Thread.Sleep(1)
        End While
    End Sub

    Private Sub Connect()

        Dim myConnectionobject As New thdListener()
        With myConnectionobject
            .Client = server.AcceptSocket()
            .Connection()
        End With
        myConnectionobject = Nothing

    End Sub

End Module
