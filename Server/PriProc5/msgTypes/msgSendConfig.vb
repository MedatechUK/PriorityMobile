Imports System.Xml

Public Class msgSendConfig : Inherits svcRequest

    Private _svcType As String
    Public Property svcType() As String
        Get
            Return _svcType
        End Get
        Set(ByVal value As String)
            _svcType = value
        End Set
    End Property

    Private _LogPort As Integer = 0
    Public Property logPort() As Integer
        Get
            Return _LogPort
        End Get
        Set(ByVal value As Integer)
            _LogPort = value
        End Set
    End Property

    Private _LogServer As String
    Public Property LogServer() As String
        Get
            Return _LogServer
        End Get
        Set(ByVal value As String)
            _LogServer = value
        End Set
    End Property

#Region "Initialisation and finalisation"

    Public Sub New(ByVal svrType As String, ByVal LogServer As String, ByVal LogPort As Integer)
        _svcType = svrType
        TimeStamp = Now.ToString("dd/MM/yyyy hh:mm:ss")
        Source = Environment.MachineName
        _LogServer = LogServer
        _LogPort = LogPort
    End Sub

    Public Sub New(ByRef Request As XmlNode)
        With Request
            Source = .SelectSingleNode("source").InnerText
            TimeStamp = .SelectSingleNode("timestamp").InnerText
            _svcType = .SelectSingleNode("svctype").InnerText
            _LogServer = .SelectSingleNode("logserver").InnerText
            _LogPort = .SelectSingleNode("logport").InnerText
        End With
    End Sub

#End Region

#Region "overriden Methods"

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
            .WriteElementString("logserver", _LogServer)
            .WriteElementString("logport", _LogPort)
        End With
    End Sub

#End Region

End Class
