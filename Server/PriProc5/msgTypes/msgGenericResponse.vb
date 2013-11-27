Imports System.Xml

Public Class msgGenericResponse : Inherits svcResponse

#Region "Initialisation and finalisation"

    Public Sub New(ByRef Sucsess As Boolean, ByRef ex As Exception)
        TimeStamp = Now.ToString("dd/MM/yyyy hh:mm:ss")
        Source = Environment.MachineName
        Select Case Sucsess
            Case False
                ErrCde = 500
                errMsg = ex.Message
            Case Else
                ErrCde = 200
                errMsg = String.Empty
        End Select
    End Sub

    Public Sub New(ByRef Response As XmlNode)
        With Response
            Source = .SelectSingleNode("source").InnerText
            TimeStamp = .SelectSingleNode("timestamp").InnerText
            ErrCde = .SelectSingleNode("error/code").InnerText
            errMsg = .SelectSingleNode("error/message").InnerText
        End With
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

#End Region

End Class
