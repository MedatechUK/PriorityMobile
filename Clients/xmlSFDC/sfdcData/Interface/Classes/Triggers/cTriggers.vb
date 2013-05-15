Imports System.Xml

Public Class cTriggers
    Inherits System.Collections.Generic.Dictionary(Of String, ctrigger)

    Public Sub New(ByRef Parent As cNode, ByRef thisNode As XmlNode)
        Try
            For Each tr As XmlNode In thisNode.SelectNodes("triggers")
                With tr
                    Me.Add(.Attributes("trigger").Value, New cTrigger(Parent, tr))
                End With
            Next
        Catch ex As Exception
            Throw New cfmtException("Failed to load triggers. {0}", ex.Message)
        End Try
    End Sub

End Class
