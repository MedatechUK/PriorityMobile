Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading

Public Class ntClient
    Inherits SocketApp
    Dim bidEndPoint As IPEndPoint = Nothing

#Region "initialisation"

    Sub New()        
        MyBase.New()
        MyRole = Role.Client
    End Sub

    Sub New(ByVal IP As String, ByVal Port As Integer)
        MyBase.New()
        MyRole = Role.Client
        _IP = IP
        _Port = Port
    End Sub

#End Region

#Region "Private Variables"

    Private _IP As String
    Private _Port As Integer

#End Region

#Region "Public Properties"

    Public Property IP() As String
        Get
            Return _IP
        End Get
        Set(ByVal value As String)
            _IP = value
        End Set
    End Property

    Public Property Port() As Integer
        Get
            Return _Port
        End Get
        Set(ByVal value As Integer)
            _Port = value
        End Set
    End Property

#End Region

#Region "overrides base"

    Public Overrides Function IsConnected() As Boolean
        Return ClientConnection.Connected
    End Function

    Public Overrides Sub OnCommand(ByVal CmdStr As String, ByVal Args() As String)
        Command(CmdStr, Args)
    End Sub

    Public Overrides Sub OnConnectionFail(ByVal ErrorMessage As String)
        Connected = False
        If Not stopping Then ConnectionFail(ErrorMessage)
    End Sub

    Public Overrides Sub OnDisconnection()
        Disconnection()
    End Sub

    Public Overrides Sub OnStream(ByVal StreamData As String)
        NewStreamData(StreamData)
    End Sub

#End Region

#Region "Overridable subs"

    Public Overridable Sub Command(ByVal Command As String, ByVal Args() As String)

    End Sub

    Public Overridable Sub Connection()

    End Sub

    Public Overridable Sub Disconnection()

    End Sub

    Public Overridable Sub ConnectionFail(ByVal ex As String)

    End Sub

    Public Overridable Sub NewStreamData(ByVal StreamData As String)

    End Sub

#End Region

#Region "Connect"

    Public Sub Connect()

        Try
            bidEndPoint = New IPEndPoint(IPAddress.Parse(IP), Port)
            ClientConnection = New Socket _
                         (AddressFamily.InterNetwork, _
                         SocketType.Stream, _
                         ProtocolType.Tcp)
            Dim myAsyncCallBack As New AsyncCallback(AddressOf ClientConnect)
            ClientConnection.BeginConnect(bidEndPoint, myAsyncCallBack, ClientConnection)

        Catch
            Connected = False
            ConnectionFail("")
        End Try

    End Sub

    Private Sub ClientConnect(ByVal pIAsyncResult As IAsyncResult)
        Try
            ClientConnection.EndConnect(pIAsyncResult)
            If ClientConnection.Connected Then
                ' Connection sucsessful
                Connected = True
                Connection()
                Try
                    ReceiveStart()
                Catch
                End Try
            Else
                Connected = False
                ConnectionFail("")
            End If
        Catch
            Connected = False
            ConnectionFail("")
        End Try
    End Sub

#End Region

End Class
