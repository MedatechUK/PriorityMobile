Public Class responseEcho
    Inherits CumulocityResponse

    Sub New(ByVal Response As String)
        MyBase.New(Response)
    End Sub

    Public ReadOnly Property Message() As String
        Get
            Return jsonDictionary.CharTrim(Data("value")("echo"))
        End Get
    End Property

End Class
