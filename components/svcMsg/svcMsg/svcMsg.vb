Imports System.Xml
Imports System.Text

Public MustInherit Class ServiceMessage : Implements IDisposable

    Private myEncoder As New System.Text.ASCIIEncoding

    Public MustOverride ReadOnly Property msgType() As String
    Public MustOverride ReadOnly Property Verb() As String

    Public Overridable Sub writeXML(ByRef outputStream As XmlWriter)
    End Sub

    Public Overridable Sub writeErr(ByRef outputStream As XmlWriter)
    End Sub

    Private _Source As String
    Public Property Source() As String
        Get
            Return _Source
        End Get
        Set(ByVal value As String)
            _Source = value
        End Set
    End Property

    Public Function toByte() As Byte()

        Dim str As New System.Text.StringBuilder
        Dim xw As XmlWriter = XmlWriter.Create(str)
        With xw
            .WriteStartDocument()
            .WriteStartElement(Verb)
            .WriteElementString("type", msgType)
            .WriteElementString("source", Environment.MachineName)
            writeErr(xw)
            writeXML(xw)
            .WriteEndDocument()
            .Flush()
        End With
        Return myEncoder.GetBytes(str.ToString)

    End Function

    Private disposedValue As Boolean = False        ' To detect redundant calls

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
