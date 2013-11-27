Imports System
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading
Imports Microsoft.VisualBasic
Imports System.Xml

' State object for reading client data asynchronously

Public Class StateObject
    ' Client  socket.
    Public workSocket As Socket = Nothing
    ' Size of receive buffer.
    Public Const BufferSize As Integer = 1024
    ' Receive buffer.
    Public buffer(BufferSize) As Byte
    ' Received data string.
    Public sb As New StringBuilder
End Class 'StateObject

Public Class Listener

    Public Event Request(ByRef Request As Byte(), ByRef Response As Byte())

    ' Thread signal.
    Private allDone As New ManualResetEvent(False)

    Private Closing As Boolean = False
    Private listener As Socket

    ' This server waits for a connection and then uses  asychronous operations to
    ' accept the connection, get data from the connected client, 
    ' echo that data back to the connected client.
    ' It then disconnects from the client and waits for another client. 
    Public Sub New(ByVal port As Integer)

        ' Data buffer for incoming data.
        Dim bytes() As Byte = New [Byte](1023) {}
        Dim localEndPoint As New IPEndPoint(IPAddress.Any, port)

        ' Create a TCP/IP socket.
        listener = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)

        ' Bind the socket to the local endpoint and listen for incoming connections.
        listener.Bind(localEndPoint)
        listener.Listen(100)

        Dim main As New Thread(AddressOf MainLoop)
        main.Start()

    End Sub 'Main

    Private Sub MainLoop()
        While Not Closing
            ' Set the event to nonsignaled state.
            allDone.Reset()

            ' Start an asynchronous socket to listen for connections.
            'Console.WriteLine("Waiting for a connection...")
            listener.BeginAccept(New AsyncCallback(AddressOf AcceptCallback), listener)

            ' Wait until a connection is made and processed before continuing.
            allDone.WaitOne()
        End While

        Console.WriteLine("Closing connection...")

    End Sub

    Private Sub AcceptCallback(ByVal ar As IAsyncResult)

        ' Get the socket that handles the client request.
        Dim listener As Socket = CType(ar.AsyncState, Socket)
        ' End the operation.
        Try
            Dim handler As Socket = listener.EndAccept(ar)
            ' Create the state object for the async receive.
            Dim state As New StateObject
            state.workSocket = handler
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, New AsyncCallback(AddressOf ReadCallback), state)
        Catch
        End Try
    End Sub 'AcceptCallback


    Private Sub ReadCallback(ByVal ar As IAsyncResult)
        Dim content As String = String.Empty

        ' Retrieve the state object and the handler socket
        ' from the asynchronous state object.
        Dim state As StateObject = CType(ar.AsyncState, StateObject)
        Dim handler As Socket = state.workSocket

        ' Read data from the client socket. 
        Dim bytesRead As Integer = handler.EndReceive(ar)
        If bytesRead > 0 Then
            ' There  might be more data, so store the data received so far.
            state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead))

            ' Check for end-of-file tag. If it is not there, read 
            ' more data.
            content = state.sb.ToString()
            If content.IndexOf("</request>") > -1 Or content.IndexOf("</response>") > -1 Or content.IndexOf("<endtrans>") > -1 Then
                ' All the data has been read from the 
                ' client. Display it on the console.
                'Console.WriteLine("Read {0} bytes from socket. " + vbLf + " Data : {1}", content.Length, content)

                Dim docReponse() As Byte = Nothing
                RaiseEvent Request(Encoding.ASCII.GetBytes(content), docReponse)
                Send(handler, docReponse)

            Else
                ' Not all data received. Get more.
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, New AsyncCallback(AddressOf ReadCallback), state)
            End If
        End If
    End Sub 'ReadCallback

    Private Sub Send(ByVal handler As Socket, ByVal byteData As Byte())
        ' Begin sending the data to the remote device.
        handler.BeginSend(byteData, 0, byteData.Length, 0, New AsyncCallback(AddressOf SendCallback), handler)
    End Sub 'Send

    Private Sub SendCallback(ByVal ar As IAsyncResult)
        ' Retrieve the socket from the state object.
        Dim handler As Socket = CType(ar.AsyncState, Socket)

        ' Complete sending the data to the remote device.
        Dim bytesSent As Integer = handler.EndSend(ar)
        'Console.WriteLine("Sent {0} bytes to client.", bytesSent)

        handler.Shutdown(SocketShutdown.Both)
        handler.Close()
        ' Signal the main thread to continue.
        allDone.Set()
    End Sub 'SendCallback

    Public Sub Shutdown()
        Closing = True
        ' Signal the main thread to continue.
        allDone.Set()
    End Sub

End Class 'AsynchronousSocketListener