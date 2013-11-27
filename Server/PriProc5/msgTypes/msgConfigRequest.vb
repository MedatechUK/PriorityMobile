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
        _svcType = svcType
        TimeStamp = Now.ToString("dd/MM/yyyy hh:mm:ss")
        Source = Environment.MachineName
        _responsePort = responsePort
    End Sub

    Public Sub New(ByRef Request As XmlNode)        
        Source = Request.SelectSingleNode("source").InnerText
        TimeStamp = Request.SelectSingleNode("timestamp").InnerText
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

    Private _Source As String
    Public Overrides Property Source() As String
        Get
            Return _Source
        End Get
        Set(ByVal value As String)
            _Source = value
        End Set
    End Property

    Private _TimeStamp As String
    Public Overrides Property TimeStamp() As String
        Get
            Return _TimeStamp
        End Get
        Set(ByVal value As String)
            _TimeStamp = value
        End Set
    End Property

    Public Overrides Sub writeXML(ByRef outputStream As System.Xml.XmlWriter)
        With outputStream
            .WriteElementString("svctype", _svcType)
            .WriteElementString("port", _responsePort.ToString)
        End With
    End Sub

#End Region

End Class
