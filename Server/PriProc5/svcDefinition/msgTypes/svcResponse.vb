Imports System.Xml

Public MustInherit Class svcResponse : Inherits ServiceMessage

    Private _errCode As Integer = 200
    Public Property ErrCde() As Integer
        Get
            Return _errCode
        End Get
        Set(ByVal value As Integer)
            _errCode = value
        End Set
    End Property

    Private _errMsg As String = String.Empty
    Public Property errMsg() As String
        Get
            Return _errMsg
        End Get
        Set(ByVal value As String)
            _errMsg = value
        End Set
    End Property

    Public Overrides ReadOnly Property Verb() As String
        Get
            Return "response"
        End Get
    End Property

    Public Overrides Sub writeErr(ByRef outputStream As System.Xml.XmlWriter)
        With outputStream
            .WriteStartElement("error")
            .WriteElementString("code", _errCode.ToString)
            .WriteElementString("message", _errMsg)
            .WriteEndElement()
        End With
    End Sub

End Class
