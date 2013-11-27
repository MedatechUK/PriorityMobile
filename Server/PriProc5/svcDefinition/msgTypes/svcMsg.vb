Imports System.Xml
Imports System.Text

Public MustInherit Class ServiceMessage : Implements IDisposable

#Region "Private Variables"

    Private myEncoder As New System.Text.ASCIIEncoding

#End Region

#Region "Must Override Properties"

    Public MustOverride ReadOnly Property Verb() As String
    Public MustOverride ReadOnly Property msgType() As String
    Public MustOverride Property Source() As String
    Public MustOverride Property TimeStamp() As String

#End Region

#Region "Overridable Methods"

    Public Overridable Sub writeXML(ByRef outputStream As XmlWriter)
    End Sub

    Public Overridable Sub writeErr(ByRef outputStream As XmlWriter)
    End Sub

#End Region

#Region "Public Methods"

    Public Function toByte() As Byte()
        Return myEncoder.GetBytes(BuildXML.ToString)
    End Function

    Public Function toXML() As XmlDocument
        Dim ret As New XmlDocument
        ret.LoadXml(BuildXML.ToString)
        Return ret
    End Function

#End Region

#Region "Private Methods"

    Private Function BuildXML() As System.Text.StringBuilder
        Dim str As New System.Text.StringBuilder
        Dim xw As XmlWriter = XmlWriter.Create(str)
        With xw
            .WriteStartDocument()
            .WriteStartElement(Verb)
            .WriteElementString("type", msgType)
            .WriteElementString("source", Source)
            .WriteElementString("timestamp", TimeStamp)
            writeErr(xw)
            writeXML(xw)
            .WriteEndDocument()
            .Flush()
        End With
        Return str
    End Function

#End Region

#Region " IDisposable Support "

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

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

End Class
