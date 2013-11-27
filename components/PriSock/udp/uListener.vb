Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports System.Text

Public Class uListener : Inherits socketListener

    Public Event Broadcast_Rcvd(ByVal Broadcast As Byte())

#Region "Private Properties"

    Private _thisEndPoint As IPEndPoint
    Private _receivingClient As UdpClient                         'Client for handling incoming data    
    Private receivingThread As Thread                            'Create a separate thread to listen for incoming data, helps to prevent the form from freezing up
    Private closing As Boolean = False                           'Used to close clients if form is closing
    Private _Port As Integer = 0

#End Region

#Region "Overridden Properties"

    Public Overrides ReadOnly Property ProtocolType() As eProtocolType
        Get
            Return eProtocolType.udp
        End Get
    End Property

#End Region

#Region "Initialisation and finalisation"

    Public Sub New(ByVal Port As Integer, ByRef thisHandler As System.EventHandler(Of byteMsg))

        msgHandler = thisHandler
        _Port = Port
        _thisEndPoint = New IPEndPoint(IPAddress.Any, Port)
        _receivingClient = New UdpClient()
        With _receivingClient.Client
            .SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, True)
            .ExclusiveAddressUse = False
            .Bind(_thisEndPoint)
        End With        

        Dim start As ThreadStart = New ThreadStart(AddressOf Receiver)
        receivingThread = New Thread(start)
        With receivingThread
            .IsBackground = True
            .Start()
        End With

    End Sub

    Public Overrides Sub disposeMe()
        closing = True          'Tells receiving loop to close
        _receivingClient.Close()
        Console.WriteLine("Closing UDP socketListener on {0}.", _Port)
    End Sub

#End Region

#Region "Private Methods"

    Private Sub Receiver()
        While Not closing       'Setup an infinite loop
            Try
                Dim arg As New byteMsg(Me.ProtocolType, _receivingClient.Receive(_thisEndPoint))
                With arg                    
                    If .Message.Length > 0 Then
                        msgHandler.Invoke(Me, arg)
                    End If
                End With
            Catch
            End Try
        End While
    End Sub

#End Region

End Class
