Imports System.Xml

Public Class iMessages
    Inherits Dictionary(Of Integer, String)

    Public Sub New(ByRef node As XmlNode)
        For Each mess As XmlNode In node.SelectNodes(".//messages/message")
            Add(CInt(mess.Attributes("num").Value), mess.Attributes("text").Value.Replace("'", "' + char(39) + '"))
        Next
    End Sub

End Class
