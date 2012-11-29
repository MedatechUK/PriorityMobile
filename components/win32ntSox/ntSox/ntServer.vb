Imports System.Net
Imports System.Net.Sockets
Imports System.Threading

Public Class ntServer
    Friend server As TcpListener = Nothing
    Public Connections As New Dictionary(Of String, thdListener)

#Region "initialisation"

    Sub New()
        MyBase.New()
    End Sub

    Sub New(ByVal IP As String, ByVal Port As Integer)
        MyBase.New()
        _IP = IP
        _Port = Port
    End Sub

#End Region

#Region "Private Variables"

    Private _stopping As Boolean = False
    Private _IP As String
    Private _Port As Integer

#End Region

#Region "Public Properties"

    Public Property Stopping() As Boolean
        Get
            Return _stopping
        End Get
        Set(ByVal value As Boolean)
            _stopping = value
        End Set
    End Property

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

    Public Property SessionData(ByVal ConnectionId As String, ByVal Parameter As String) As String
        Get
            If Connections.ContainsKey(ConnectionId) Then
                Return Connections(ConnectionId).SessionData(Parameter)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As String)
            If Connections.ContainsKey(ConnectionId) Then
                Connections(ConnectionId).SessionData(Parameter) = value
            End If
        End Set
    End Property

#End Region

#Region "Public subs"

    Public Sub StartSvc()
        Stopping = False

        If IsNothing(server) Then
            Dim localAddr As IPAddress = IPAddress.Parse(IP)
            server = New TcpListener(localAddr, Port)
            server.Start()
        End If

        Dim myThread As Thread
        myThread = New Thread(New ThreadStart(AddressOf listen))
        myThread.Start()
    End Sub

    Public Sub StopSvc()
        Stopping = True
    End Sub

    Public Function Send(ByVal ConnectionID As String, ByVal StrVal As String, Optional ByVal LineEnd As Boolean = True) As Boolean
        If Connections.ContainsKey(ConnectionID) Then
            If Not LineEnd Then
                Connections(ConnectionID).Send(StrVal, "")
            Else
                Connections(ConnectionID).Send(StrVal)
            End If
            Return True
        Else
            Return False
        End If
    End Function

    Public Overloads Function SendFormat(ByVal ConnectionID As String, ByVal StrVal As String, ByVal ParamArray Args() As String) As Boolean
        If Connections.ContainsKey(ConnectionID) Then
            Connections(ConnectionID).Send(String.Format(StrVal, Args))
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub KillConnection(ByVal ConnectionID As String)
        If Connections.ContainsKey(ConnectionID) Then
            Connections(ConnectionID).stopping = True
        End If
    End Sub

#End Region

#Region "Private Subs"

    Private Sub listen()
        ' Enter the listening loop.   

        While Not Stopping
            If server.Pending Then
                Do Until Not server.Pending Or Stopping
                    Dim _id As String = System.Guid.NewGuid().ToString()
                    With Connections
                        .Add(_id, New thdListener(Me))
                    End With
                    With Connections(_id)
                        .ID = _id                        
                        .ClientConnection = server.AcceptSocket()
                        Connect(_id)
                        Dim myThread As Thread
                        myThread = New Thread(New ThreadStart(AddressOf .Connection))
                        myThread.Name = "thdListener"
                        myThread.Start()
                    End With
                Loop
            End If
            Thread.Sleep(1)
        End While

    End Sub

    Public Overridable Sub Connect(ByVal ConnectionID As String)

    End Sub

    Public Overridable Sub Command(ByVal ConnectionID As String, ByVal CmdStr As String, ByVal Args() As String)

    End Sub

    Public Overridable Sub Disconnect(ByVal ConnectionID As String)
        Connections(ConnectionID) = Nothing
        Connections.Remove(ConnectionID)
    End Sub

#End Region

End Class
