Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports System.Text

Public Class Broadcaster

#Region "Private Properties"

    Private _port As Integer                                     'Port number to send/recieve data on
    Private Const broadcastAddress As String = "255.255.255.255" 'Sends data to all LOCAL listening clients, to send data over WAN you'll need to enter a public (external) IP address of the other client    
    Private sendingClient As UdpClient                           'Client for sending data    

#End Region

#Region "Initialisation and finalisation"

    Public Sub New(ByVal Port As Integer)
        _port = Port
        InitializeSender()          'Initializes startup of sender client                                           
    End Sub

    Private Sub InitializeSender()
        sendingClient = New UdpClient(broadcastAddress, _port)
        sendingClient.EnableBroadcast = True
    End Sub

#End Region

#Region "Public Methods"

    Public Sub Broadcast(ByVal strData As String)
        Dim data() As Byte = Encoding.ASCII.GetBytes(strData) 'Convert string to bytes
        sendingClient.Send(data, data.Length)               'Send bytes
    End Sub

    Public Sub Broadcast(ByVal Data As Byte())        
        sendingClient.Send(Data, Data.Length)               'Send bytes
    End Sub

    Public Sub Shutdown()
        sendingClient.Close()
    End Sub

#End Region

End Class
