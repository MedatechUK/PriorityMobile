Imports System.Net.Sockets
Imports System.Text
Imports System.Xml

Public Class Correspondant : Implements IDisposable

    Public Event Response(ByVal Response As xmldocument)
    Private disposedValue As Boolean = False        ' To detect redundant calls

    Dim ClientSocket As New TcpClient
    Dim ServerStream As NetworkStream

    Public Sub New(ByVal ServerAddress As String, ByVal PortNumber As Integer)
        ClientSocket.Connect(ServerAddress, PortNumber)
    End Sub

    Public Function Send(ByVal data() As Byte) As Byte()

        Dim respDoc As New XmlDocument
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
            Loop Until content.IndexOf("</request>") > -1 Or _
                content.IndexOf("</response>") > -1 Or _
                content.IndexOf("<endtrans>") > -1
            
        End With

        Return Encoding.ASCII.GetBytes(content.ToString)

    End Function

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: free other state (managed objects).
            End If

            ' TODO: free your own state (unmanaged objects).
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

#Region " IDisposable Support "
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
