Imports System.Net.Sockets
Imports System.Text
Imports System.Xml

Public Class iClient : Inherits socketClient

#Region "Private Properties"

    Dim ClientSocket As New TcpClient
    Dim ServerStream As NetworkStream

#End Region

#Region "Overridden Properties"

    Public Overrides ReadOnly Property ProtocolType() As eProtocolType
        Get
            Return eProtocolType.tcp
        End Get
    End Property

    Private _ConnectionError As Exception = Nothing
    Public Overrides ReadOnly Property ConnectionError() As System.Exception
        Get
            Return _ConnectionError
        End Get
    End Property

#End Region

#Region "Initialisation and finalisation"

    Public Sub New(ByVal ServerAddress As String, ByVal PortNumber As Integer)
        Try
            ClientSocket.Connect(ServerAddress, PortNumber)
        Catch ex As Exception
            _ConnectionError = ex
        End Try
    End Sub

    Public Overrides Sub disposeMe()
        Me.Finalize()
    End Sub

#End Region

#Region "Overridden Methods"

    Public Overrides Function Send(ByVal data() As Byte) As Byte()

        Dim content As String = String.Empty
        Dim sb As New System.Text.StringBuilder
        Dim inStream(1024) As Byte

        ServerStream = ClientSocket.GetStream()
        With ServerStream
            .Write(data, 0, data.Length)
            .Flush()

            Do
                .Read(inStream, 0, inStream.Length)
                sb.Append(Encoding.ASCII.GetString(inStream, 0, inStream.Length))
                content = sb.ToString()

                ' Check for end-of-file tag. If it is not there, read 
                ' more data.
            Loop Until EndTrans(sb)

        End With

        Return Encoding.ASCII.GetBytes(content.ToString)

    End Function

#End Region

End Class
