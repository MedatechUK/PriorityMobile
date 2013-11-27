Imports System.Xml
Imports System.Text
Imports PriPROC

Public Enum eVerb
    Request
    Response
End Enum

Public Class svcMsgXML

    Private msgDocument As New XmlDocument

    Private Function VerbStr(ByVal Verb As eVerb) As String
        Select Case Verb
            Case eVerb.Request
                Return "request"
            Case eVerb.Response
                Return "response"
            Case Else
                Return String.Empty
        End Select
    End Function

#Region "Public Properties"

    Private _verb As eVerb
    Public ReadOnly Property Verb() As eVerb
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
    Public ReadOnly Property msgType() As String
        Get
            Return _msgType
        End Get
    End Property

    Private _Protocol As eProtocolType
    Public ReadOnly Property Protocol() As eProtocolType
        Get
            Return _Protocol
        End Get
    End Property

    Private _toByte As Byte()
    Public ReadOnly Property toByte() As Byte()
        Get
            Return _toByte
        End Get
    End Property

#End Region

#Region "Initialisation and finalisation"

    Public Sub New(ByVal Protocol As eProtocolType, ByVal bytes As Byte())        
        Try
            _toByte = bytes
            With msgDocument
                .LoadXml(Encoding.ASCII.GetString(bytes))
                If Not IsNothing(.SelectSingleNode("request")) Then
                    _verb = eVerb.Request
                ElseIf Not IsNothing(.SelectSingleNode("response")) Then
                    _verb = eVerb.Response
                Else
                    Throw New Exception
                End If

                _msgType = .SelectSingleNode(String.Format("{0}/type", VerbStr(_verb))).InnerText
                _Source = .SelectSingleNode(String.Format("{0}/source", VerbStr(_verb))).InnerText
                _msgNode = .SelectSingleNode(String.Format("{0}", VerbStr(_verb)))
                _Protocol = Protocol

            End With
        Catch
            Throw New Exception("Malformed message.")
        End Try
    End Sub

#End Region

End Class
