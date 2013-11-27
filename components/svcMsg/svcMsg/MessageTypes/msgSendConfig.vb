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

#Region "Initialisation and finalisation"

    Public Sub New(ByVal svrType As String)
        _svcType = svrType
    End Sub

    Public Sub New(ByRef Request As XmlNode)
        With Request
            Source = .SelectSingleNode("source").InnerText
            _svcType = .SelectSingleNode("svctype").InnerText
        End With
    End Sub

#End Region

#Region "overriden Methods"

    Public Overrides ReadOnly Property msgType() As String
        Get
            Return "config"
        End Get
    End Property

    Public Overrides Sub writeXML(ByRef outputStream As System.Xml.XmlWriter)
        With outputStream
            .WriteElementString("svctype", _svcType)            
        End With
    End Sub

#End Region

End Class
