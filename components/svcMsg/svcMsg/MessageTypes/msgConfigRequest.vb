Imports System.Xml

Public Class msgConfigRequest : Inherits svcRequest

#Region "Message Properies"

    Private _svcType As String
    Public Property svcType() As String
        Get
            Return _svcType
        End Get
        Set(ByVal value As String)
            _svcType = value
        End Set
    End Property

    Private _responsePort As Integer
    Public Property responsePort() As Integer
        Get
            Return _responsePort
        End Get
        Set(ByVal value As Integer)
            _responsePort = value
        End Set
    End Property

#End Region

#Region "Initialisation and Finalisation"

    Public Sub New(ByVal svcType As String, ByVal responsePort As Integer)
        Source = Environment.MachineName
        _svcType = svcType
        _responsePort = responsePort
    End Sub

    Public Sub New(ByRef Request As XmlNode)
        Source = Request.SelectSingleNode("source").InnerText
        _svcType = Request.SelectSingleNode("svctype").InnerText
        _responsePort = Request.SelectSingleNode("port").InnerText
    End Sub

#End Region

#Region "Overriden Methods"

    Public Overrides ReadOnly Property msgType() As String
        Get
            Return "config"
        End Get
    End Property

    Public Overrides Sub writeXML(ByRef outputStream As System.Xml.XmlWriter)
        With outputStream
            .WriteElementString("svctype", _svcType)
            .WriteElementString("port", _responsePort.ToString)
        End With
    End Sub

#End Region

End Class
