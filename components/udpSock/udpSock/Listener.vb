Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.Xml

Public Class Listener

    Public Event Broadcast_Rcvd(ByVal Broadcast As Byte())

#Region "Private Properties"

    Private _port As Integer                                     'Port number to send/recieve data on
    Private Const broadcastAddress As String = "255.255.255.255" 'Sends data to all LOCAL listening clients, to send data over WAN you'll need to enter a public (external) IP address of the other client
    Private receivingClient As UdpClient                         'Client for handling incoming data    
    Private receivingThread As Thread                            'Create a separate thread to listen for incoming data, helps to prevent the form from freezing up
    Private closing As Boolean = False                           'Used to close clients if form is closing

#End Region

#Region "Initialisation and finalisation"

    Public Sub New(ByVal Port As Integer)
        _port = Port
        InitializeReceiver()        'Starts listening for incoming data                                             
    End Sub

    Private Sub InitializeReceiver()
        receivingClient = New UdpClient(_port)
        Dim start As ThreadStart = New ThreadStart(AddressOf Receiver)
        receivingThread = New Thread(start)
        receivingThread.IsBackground = True
        receivingThread.Start()
    End Sub

#End Region

#Region "Private Methods"

    Private Sub Receiver()
        Dim endPoint As IPEndPoint = New IPEndPoint(IPAddress.Any, _port) 'Listen for incoming data from any IP address on the specified _port (i personally select 9653)
        While Not closing       'Setup an infinite loop
            Try
                RaiseEvent Broadcast_Rcvd(receivingClient.Receive(endPoint))
            Catch
            End Try
        End While

    End Sub

#End Region

#Region "Public Methods"

    Public Sub Shutdown()
        closing = True          'Tells receiving loop to close
        receivingClient.Close()
        Console.WriteLine("Closing connection...")
    End Sub

#End Region

End Class
