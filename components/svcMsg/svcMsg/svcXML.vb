Imports System.Xml
Imports System.Text

Public Class svcMsgXML

    Private msgDocument As New XmlDocument

#Region "Public Properties"

    Private _verb As String
    Public ReadOnly Property Verb() As String
        Get
            Return _verb
        End Get
    End Property

    Private _msgNode As XmlNode
    Public ReadOnly Property msgNode() As XmlNode
        Get
            Return _msgNode
        End Get
    End Property

    Private _Source As String
    Public ReadOnly Property Source() As String
        Get
            Return _Source
        End Get
    End Property

    Private _msgType As String
    Public Property msgType() As String
        Get
            Return _msgType
        End Get
        Set(ByVal value As String)
            _msgType = value
        End Set
    End Property

#End Region

#Region "Initialisation and finalisation"

    Public Sub New(ByVal bytes As Byte())
        Try
            With msgDocument
                .LoadXml(Encoding.ASCII.GetString(bytes))
                If Not IsNothing(.SelectSingleNode("request")) Then
                    _verb = "request"
                ElseIf Not IsNothing(.SelectSingleNode("response")) Then
                    _verb = "response"
                Else
                    Throw New Exception
                End If

                _msgType = .SelectSingleNode(String.Format("{0}/type", _verb)).InnerText
                _Source = .SelectSingleNode(String.Format("{0}/source", _verb)).InnerText
                _msgNode = .SelectSingleNode(String.Format("{0}", _verb))

            End With
        Catch
            Throw New Exception("Malformed message.")
        End Try
    End Sub

#End Region

End Class
