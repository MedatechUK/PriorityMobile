Imports System.Xml

Public Class cColumns
    Inherits System.Collections.Generic.Dictionary(Of String, cColumn)

    Public Sub New(ByRef Parent As cContainer, ByRef thisNode As XmlNode)
        Try
            For Each col As XmlNode In thisNode.SelectNodes("column")
                Me.Add(col.Attributes("name").Value, New cColumn(Parent, col))
            Next
        Catch ex As Exception
            Throw New cfmtException("Failed to load columns. {0}", ex.Message)
        End Try
    End Sub

End Class
