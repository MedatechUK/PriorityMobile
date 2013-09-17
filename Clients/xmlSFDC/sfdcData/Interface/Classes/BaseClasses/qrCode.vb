Imports System.Xml

Public Class qrCode
    Inherits Dictionary(Of String, String)

    Public Sub New()
        Me.Clear()
    End Sub

    Public Function Encode() As String

        Dim result As New System.Text.StringBuilder

        ' Create XmlWriterSettings.
        Dim settings As XmlWriterSettings = New XmlWriterSettings()
        settings.Indent = False

        ' Create XmlWriter.
        Using writer As XmlWriter = XmlWriter.Create(result, settings)
            ' Begin writing.
            writer.WriteStartDocument()
            writer.WriteStartElement("in") ' Root.

            ' Loop over employees in array.            
            For Each key As String In Me.Keys
                writer.WriteStartElement("i")
                writer.WriteAttributeString("n", key)
                writer.WriteAttributeString("v", Me(key))
                writer.WriteEndElement()
            Next

            ' End document.
            writer.WriteEndElement()
            writer.WriteEndDocument()
        End Using

        Return result.ToString.Split("?").Last.Substring(1)

    End Function

End Class
