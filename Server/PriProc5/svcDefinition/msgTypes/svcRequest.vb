Imports System.Xml

Public MustInherit Class svcRequest : Inherits ServiceMessage

    Public Overrides ReadOnly Property Verb() As String
        Get
            Return "request"
        End Get
    End Property

End Class
