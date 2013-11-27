Imports System.Xml

Public Class msgSubscGetRequest : Inherits svcRequest

    Public Overrides ReadOnly Property msgType() As String
        Get
            Return "get"
        End Get
    End Property

    Private _logMsg As New List(Of msgLogRequest)
    Public Property logMsg() As List(Of msgLogRequest)
        Get
            Return _logMsg
        End Get
        Set(ByVal value As List(Of msgLogRequest))
            _logMsg = value
        End Set
    End Property

#Region "Initialisation and finalisation"

    Public Sub New(ByRef MsgQ As Queue(Of msgLogRequest))
        TimeStamp = Now.ToString("dd/MM/yyyy hh:mm:ss")
        Source = Environment.MachineName
        Do While MsgQ.Count > 0
            _logMsg.Add(MsgQ.Dequeue())
        Loop
    End Sub

    Public Sub New(ByRef Response As XmlNode)
        With Response
            Source = .SelectSingleNode("source").InnerText
            TimeStamp = .SelectSingleNode("timestamp").InnerText
            For Each msg As XmlNode In .SelectNodes("msg/request")
                _logMsg.Add(New msgLogRequest(msg))
            Next
        End With
    End Sub

#End Region

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
            .WriteStartElement("msg")
            For Each msg As msgLogRequest In _logMsg
                .WriteRaw(msg.toXML.SelectSingleNode("request").OuterXml)
            Next
            .WriteEndElement()
        End With
    End Sub

End Class
