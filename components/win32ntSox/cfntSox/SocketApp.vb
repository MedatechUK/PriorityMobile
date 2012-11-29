Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading

Public Enum Role
    Client
    Server
End Enum

Public MustInherit Class SocketApp

#Region "Puplic Properties"

    Private _socket As System.Net.Sockets.Socket
    Public Property ClientConnection() As System.Net.Sockets.Socket
        Get
            Return _socket
        End Get
        Set(ByVal value As System.Net.Sockets.Socket)
            _socket = value
        End Set
    End Property

    Private _myrole As Role
    Public Property MyRole() As Role
        Get
            Return _myrole
        End Get
        Set(ByVal value As Role)
            _myrole = value
        End Set
    End Property

    Private _stopping As Boolean = False
    Public Property stopping() As Boolean
        Get
            Return _stopping
        End Get
        Set(ByVal value As Boolean)
            _stopping = value
        End Set
    End Property

    Private _Connected As Boolean
    Public Property Connected() As Boolean
        Get
            Return _Connected
        End Get
        Set(ByVal value As Boolean)
            _Connected = value
        End Set
    End Property

#End Region

#Region "overridable subs"

    Public MustOverride Sub OnConnectionFail(ByVal ErrorMessage As String)
    Public MustOverride Sub OnDisconnection()
    Public MustOverride Sub OnCommand(ByVal CmdStr As String, ByVal Args() As String)
    Public MustOverride Function IsConnected() As Boolean

#End Region

#Region "Local Variables"

    ' Buffers
    Dim buffer(1025) As Byte
    Private _buffer As String

#End Region

#Region "Receive Data"

    Public Sub ReceiveStart()
        Dim myAsyncCallBack As New AsyncCallback(AddressOf ReceiveData)
        ReDim buffer(1025)
        Try
            ClientConnection.BeginReceive _
              (buffer, 0, buffer.Length, 0, _
              myAsyncCallBack, ClientConnection)
        Catch ex As SocketException
            OnConnectionFail(ex.Message)
        End Try
    End Sub

    Private Sub ReceiveData(ByVal pIAsyncResult As IAsyncResult)

        Dim intByte As Integer
        Try
            intByte = ClientConnection.EndReceive(pIAsyncResult)
            If intByte > 0 Then
                ReadBuffer(buffer, intByte)
            End If
            If Not stopping Then ReceiveStart()
        Catch ex As SocketException
            OnConnectionFail(ex.Message)
        End Try

    End Sub

#End Region

#Region "Send data to Sender"

    Public Function Send(ByVal StrVal As String) As Boolean

        If IsConnected() Then
            Dim bteSend() As Byte
            Dim myAsyncCallBack As New AsyncCallback(AddressOf ClientConnectionSendData)

            Try
                bteSend = Encoding.ASCII.GetBytes(StrVal & vbCrLf)
                ClientConnection.BeginSend _
                  (bteSend, 0, bteSend.Length, _
                  SocketFlags.DontRoute, myAsyncCallBack, ClientConnection)
                Return True
            Catch ex As SocketException
                OnConnectionFail(ex.Message)
                Return False
            End Try
        Else
            OnConnectionFail("no socket")
            Return False
        End If

    End Function

    Private Sub ClientConnectionSendData(ByVal pIAsyncResult As IAsyncResult)
        Dim intSend As Integer
        intSend = ClientConnection.EndSend(pIAsyncResult)
        If stopping Then
            Try

#If WindowsCE = True Then
                ClientConnection.Close()
#Else
                Dim myAsyncCallBack As New AsyncCallback(AddressOf ClientDisconnect)
                ClientConnection.BeginDisconnect(True, myAsyncCallBack, ClientConnection)
#End If

            Catch
                ' Already disconnected
            End Try
        End If
    End Sub

#End Region

#Region "Disconnect"

    Public Sub Disconnect()
        Connected = False
        stopping = True
        Select Case MyRole
            Case Role.Client
                Send(Chr(4))
            Case Else
                Try
                    Dim myAsyncCallBack As New AsyncCallback(AddressOf ClientDisconnect)
#If WindowsCE = True Then
                    ClientConnection.Close()
#Else
                Dim myAsyncCallBack As New AsyncCallback(AddressOf ClientDisconnect)
                ClientConnection.BeginDisconnect(True, myAsyncCallBack, ClientConnection)
#End If
                Catch
                    ' Already disconnected
                End Try
        End Select
    End Sub


    Private Sub ClientDisconnect(ByVal pIAsyncResult As IAsyncResult)
        Try
#If WindowsCE = False Then
            ClientConnection.EndDisconnect(pIAsyncResult)
#End If
        Catch ex As Exception

        Finally
            OnDisconnection()
        End Try

    End Sub


#End Region

#Region "Buffer"

    Private Sub ReadBuffer(ByRef buffer() As Byte, ByVal intByte As Integer)
        _buffer = _buffer & Replace(Encoding.ASCII.GetString(buffer, 0, intByte), ChrW(10), "")
        If InStr(_buffer, Chr(13)) > 0 Then
            Dim commands() As String = Split(_buffer, ChrW(13))
            _buffer = commands(UBound(commands))
            For c As Integer = 0 To UBound(commands) - 1
                OnCommand(commands(c), MakeArgs(commands(c)))
            Next
        End If
    End Sub

    Private Function MakeArgs(ByVal Value As String) As String()
        Dim ret() As String = Nothing
        Dim sp() As String = Split(Value, ChrW(34))
        For i As Integer = 0 To UBound(sp)
            If EvenNumber(i + 1) Then
                NewArg(ret, sp(i))
            Else
                Dim tmp() As String = Split(sp(i), ChrW(32))
                For Each str As String In tmp
                    NewArg(ret, str)
                Next
            End If
        Next
        Return ret
    End Function

    Private Function EvenNumber(ByVal Value As Integer) As Boolean
        Return Value Mod 2 = 0
    End Function

    Private Sub NewArg(ByRef ArgArray() As String, ByVal NewValue As String)
        If NewValue.Length > 0 Then
            Try
                If Not IsNothing(ArgArray) Then
                    ReDim Preserve ArgArray(UBound(ArgArray) + 1)
                Else
                    ReDim ArgArray(0)
                End If
            Catch ex As Exception
                ReDim ArgArray(0)
            Finally
                ArgArray(UBound(ArgArray)) = NewValue
            End Try
        End If
    End Sub

#End Region

End Class
