Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading

Public Class thdListener
    Inherits SocketApp

#Region "initialisation"

    Sub New(ByVal sender As ntServer)
        _Parent = sender
        MyRole = Role.Server
    End Sub

#End Region

#Region "Declarations"

    Private _Parent As ntServer

    Private _id As String
    Public Property ID() As String
        Get
            Return _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    Private _SessionData As New Dictionary(Of String, String)
    Public Property SessionData(ByVal Parameter As String) As String
        Get
            If _SessionData.ContainsKey(Parameter) Then
                Return _SessionData(Parameter)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As String)
            If _SessionData.ContainsKey(Parameter) Then
                _SessionData(Parameter) = value
            Else
                _SessionData.Add(Parameter, value)
            End If
        End Set
    End Property

#End Region

    Public Overrides Function IsConnected() As Boolean
        Return True
    End Function

    Public Overrides Sub OnCommand(ByVal CmdStr As String, ByVal Args() As String)
        If Not IsNothing(Args) Then
            Select Case Args(0)
                Case Chr(4)
                    ' terminate signal
                    stopping = True
                Case Else
                    _Parent.Command(_id, CmdStr, Args)
            End Select
        End If
    End Sub

    Public Overrides Sub OnConnectionFail(ByVal ErrorMessage As String)        
        stopping = True
    End Sub

    Public Overrides Sub OnDisconnection()
        Me.Finalize()
        _Parent.Disconnect(_id)        
    End Sub

#Region "Main Thread Loop"

    Public Sub Connection()

        Try
            ReceiveStart()
        Catch
            Exit Sub
        End Try

        While Not stopping And Not _Parent.Stopping

            Try
                If ClientConnection.Connected Then
                    Thread.Sleep(10)
                Else
                    Exit While
                End If
            Catch e As Exception
                Exit While
            End Try

            Thread.Sleep(1)
        End While

        Disconnect()

    End Sub

#End Region


End Class