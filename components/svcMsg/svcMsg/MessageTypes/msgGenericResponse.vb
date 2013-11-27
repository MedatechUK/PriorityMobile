Imports System.Xml

Public Class msgGenericResponse : Inherits svcResponse

#Region "Initialisation and finalisation"

    Public Sub New(ByRef Sucsess As Boolean, ByRef ex As Exception)
        Select Case Sucsess
            Case False
                ErrCde = 500
                errMsg = ex.Message
        End Select
    End Sub

    Public Sub New(ByRef Response As XmlNode)
        With Response
            Source = .SelectSingleNode("source").InnerText
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

#End Region

End Class
