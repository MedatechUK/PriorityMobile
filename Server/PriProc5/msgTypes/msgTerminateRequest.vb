Imports System.Xml

Public Class msgTerminateRequest : Inherits svcRequest

#Region "Message Properies"

    Private _id As String
    Public Property ID() As String
        Get
            Return _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

#End Region

#Region "Initialisation and Finalisation"

    Public Sub New(ByVal ID As String)
        Source = Environment.MachineName
        TimeStamp = Now.ToString("dd/MM/yyyy hh:mm:ss")        
        _id = ID
    End Sub

    Public Sub New(ByRef Request As XmlNode)
        Source = Request.SelectSingleNode("source").InnerText
        TimeStamp = Request.SelectSingleNode("timestamp").InnerText
        _id = Request.SelectSingleNode("id").InnerText        
    End Sub

#End Region

#Region "Overriden Methods"

    Public Overrides ReadOnly Property msgType() As String
        Get
            Return "terminate"
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
            .WriteElementString("id", _id)
        End With
    End Sub

#End Region

End Class
