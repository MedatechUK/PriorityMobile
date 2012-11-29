Public Class tWidthStr
    Private _width As Integer
    Public Property Width() As Integer
        Get
            Return _width
        End Get
        Set(ByVal value As Integer)
            _width = value
        End Set
    End Property
    Private _WidthRegEx As String = ""
    Public Property WidthRegEx() As String
        Get
            Return _WidthRegEx
        End Get
        Set(ByVal value As String)
            _WidthRegEx = value
        End Set
    End Property
    Public Sub New(ByVal WidthRegEx As String, ByVal Width As Integer)
        With Me
            .Width = Width
            .WidthRegEx = WidthRegEx
        End With
    End Sub
End Class
