Imports System.Runtime.Serialization.Json
Imports System.Text

Public MustInherit Class CumulocityResponse

#Region "Initialisation and FInalisation"

    Private _Data As jsonDictionary
    Public Sub New(ByVal reponseString As String)
        _Data = New jsonDictionary(Nothing, reponseString)
    End Sub

#End Region

#Region "Public Properties"

    Public ReadOnly Property Data() As jsonDictionary
        Get
            Return _Data
        End Get
    End Property

    Public ReadOnly Property ResponseCode() As Integer
        Get
            Return CInt(Data("responseCode"))
        End Get
    End Property

#End Region

End Class