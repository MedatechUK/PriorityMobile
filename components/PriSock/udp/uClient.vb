Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports System.Text

Public Enum eBroadcastType
    bcPublic
    bcPrivate
End Enum

Public Class uClient : Inherits socketClient

#Region "Private Properties"

    Private Const broadcastAddress As String = "255.255.255.255" 'Sends data to all LOCAL listening clients, to send data over WAN you'll need to enter a public (external) IP address of the other client    
    Private Const LocalAddress As String = "127.0.0.1"
    Private sendingClient As UdpClient                           'Client for sending data    

#End Region

#Region "Overridden Properties"

    Public Overrides ReadOnly Property ProtocolType() As eProtocolType
        Get
            Return eProtocolType.udp
        End Get
    End Property

    Private _ConnectionError As System.Exception = Nothing
    Public Overrides ReadOnly Property ConnectionError() As System.Exception
        Get
            Return _ConnectionError
        End Get
    End Property

#End Region

#Region "Initialisation and finalisation"

    Public Sub New(ByVal Port As Integer, ByVal BroadcastType As eBroadcastType)
        Try
            Select Case BroadcastType
                Case eBroadcastType.bcPrivate
                    sendingClient = New UdpClient(LocalAddress, Port)
                Case eBroadcastType.bcPublic
                    sendingClient = New UdpClient(broadcastAddress, Port)
            End Select

            sendingClient.EnableBroadcast = True

        Catch ex As Exception
            _ConnectionError = ex
        End Try
    End Sub

    Public Overrides Sub disposeMe()
        sendingClient.Close()
        Finalize()
    End Sub

#End Region

#Region "Public Methods"

    Public Overrides Function Send(ByVal data() As Byte) As Byte()
        sendingClient.Send(data, data.Length)               'Send bytes
        Return Nothing
    End Function

#End Region

End Class
